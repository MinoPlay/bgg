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
    public static class GetWishlistSelections
    {
        [FunctionName("GetWishlistSelections")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("WishlistSelections", "WishlistSelections")] CloudTable wishlistSelections,
            ILogger log)
        {

            var query = new TableQuery<WishlistSelection>();
            var segment = await wishlistSelections.ExecuteQuerySegmentedAsync(query, null);

            return new JsonResult(segment.Results);
        }
    }
}