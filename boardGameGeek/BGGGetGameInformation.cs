using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class BGGGetGameInformation
    {
        [FunctionName("BGGGetGameInformation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var gameId = req.Query["gameId"];
            var result = await BGG_API.GetGameDetails(gameId);

            return new JsonResult(result);
        }
    }
}