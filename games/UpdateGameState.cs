using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public static class UpdateGameState
    {
        [FunctionName("UpdateGameState")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Games", "games")] CloudTable games,
            ILogger log)
        {
            var gameId = req.Query["gameId"];

            var retrieve = TableOperation.Retrieve<GameInfo>("games", gameId);
            var retrieveResult = await games.ExecuteAsync(retrieve);

            if (retrieveResult.HttpStatusCode == 404)
            {
                return new BadRequestObjectResult($"Game {gameId} not found.");
            }

            var result = (GameInfo)retrieveResult.Result;

            var newState = req.Query["newState"];
            if (!string.IsNullOrEmpty(newState) && result.GameState != newState)
            {
                result.GameState = newState;
            }

            var update = TableOperation.Replace(result);
            var updateResult = await games.ExecuteAsync(update);

            return result != null
                ? (ActionResult)new JsonResult(updateResult)
                : new BadRequestObjectResult($"Failed to update {gameId}");
        }
    }
}