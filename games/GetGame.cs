using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public static class GetGame
    {
        [FunctionName("GetGame")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Games", "games")] CloudTable games,
            ILogger log)
        {
            var gameId = req.Query["gameId"];

            var retrieve = TableOperation.Retrieve<GameInfo>("games", gameId);
            var retrieveResult = await games.ExecuteAsync(retrieve);
            var result = (GameInfo)retrieveResult.Result;

            return result != null
                ? (ActionResult)new JsonResult(result)
                : new BadRequestObjectResult($"Failed to get {gameId}");
        }
    }
}