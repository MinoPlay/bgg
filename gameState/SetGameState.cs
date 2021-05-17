using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public static class SetGameState
    {
        [FunctionName("SetGameState")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("GamesState", "gamesState")] CloudTable gamesState,
            [Table("GamesState")] IAsyncCollector<GameState> gamesStateTable,
            ILogger log)
        {
            var gameId = req.Query["gameId"];
            var gameState = req.Query["gameState"];

            // find gameState else create new
            var retrieveGameState = TableOperation.Retrieve<GameState>("gamesState", gameId);
            var retrieveGameStateResult = await gamesState.ExecuteAsync(retrieveGameState);

            if (retrieveGameStateResult.HttpStatusCode == 404)
            {
                var newGameState = new GameState()
                {
                    PartitionKey = "gamesState",
                    RowKey = gameId,
                    gameId = gameId,
                    gameState = gameState
                };

                await gamesStateTable.AddAsync(newGameState);

                var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(newGameState);
                return (ActionResult)new OkObjectResult($"Added: {serializeObject}");
            }

            var result = (GameState)retrieveGameStateResult.Result;
            result.gameState = gameState;

            var update = TableOperation.Replace(result);
            var updateResult = await gamesState.ExecuteAsync(update);


            return result != null
                ? (ActionResult)new JsonResult(updateResult)
                : new BadRequestObjectResult($"Failed to update {gameId}");
        }
    }
}