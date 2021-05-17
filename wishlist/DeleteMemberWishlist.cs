using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public static class DeleteMemberWishlist
    {
        [FunctionName("DeleteMemberWishlist")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("WishlistSelections", "WishlistSelections")] CloudTable wishlists,
            ILogger log)
        {
            var reqInitials = req.Query["initials"].ToString().ToUpper();
            log.LogInformation($"Trying to delete '{reqInitials}' wishlist");

            var query = new TableQuery<WishlistSelection>();
            var segment = await wishlists.ExecuteQuerySegmentedAsync(query, null);

            var wishlistRowsToDelete = segment.Results.Where(x => x.UserId == reqInitials);

            int deleteResultCode = 200;

            if (wishlistRowsToDelete.Any())
            {
                foreach (var entryToDelete in wishlistRowsToDelete)
                {
                    var toDelete = TableOperation.Delete(entryToDelete);
                    var deleteResult = await wishlists.ExecuteAsync(toDelete);
                    deleteResultCode = deleteResult.HttpStatusCode;
                }
            }

            log.LogInformation($"deleteResult: {deleteResultCode}");

            return deleteResultCode.ToString().StartsWith("20")
                ? (ActionResult)new OkObjectResult($"Successfully deleted '{reqInitials}' wishlist")
                : new BadRequestObjectResult($"Failed to delete {reqInitials} wishlist");

        }
    }
}