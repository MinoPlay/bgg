using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections;

namespace bgg
{
    public static class GetWishlistDetailed
    {
        [FunctionName("GetWishlistDetailed")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function GetWishlistDetailed start processing a request.");
            var wishlist = await GetGameInfoLogic.GetWishlist();
            var wishlistDetails = new ArrayList();

            foreach (var item in wishlist)
            {
                var itemDetails = await GetGameInfoLogic.GetGameDetails(item.Key);
                wishlistDetails.Add(itemDetails);
            }


            return new JsonResult(wishlistDetails);
        }
    }
}
