using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class BGGGetDetailedWishlist
    {
        [FunctionName("BGGGetDetailedWishlist")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var wishlistDetails = await BGG_API.GetDetailedWishlist();
            return new JsonResult(wishlistDetails);
        }
    }
}
