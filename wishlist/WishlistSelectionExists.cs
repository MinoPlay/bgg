using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public static class WishlistSelectionExists
    {
        [FunctionName("WishlistSelectionExists")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("WishlistSelections", "WishlistSelections")] CloudTable wishlistSelections,
            ILogger log)
        {
            var reqInitials = req.Query["initials"].ToString().ToUpper();

            var query = new TableQuery<WishlistSelection>();
            var segment = await wishlistSelections.ExecuteQuerySegmentedAsync(query, null);

            var result = segment.Where(x => x.UserId == reqInitials);

            return result.Any()
                ? (ActionResult)new JsonResult(result)
                : new BadRequestObjectResult($"Wishlist selection doesn't exists for {reqInitials}");
        }
    }
}