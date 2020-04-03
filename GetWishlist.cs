using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace bgg
{
    public static class GetWishlist
    {
        [FunctionName("GetWishlist")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function GetWishlist start processing a request.");
            var result = await GetGameInfoLogic.GetWishlist();
            return new JsonResult(result.Select(x => new { gameId = x.Key, gameTitle = x.Value }));
        }
    }
}
