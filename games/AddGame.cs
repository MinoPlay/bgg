using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class AddGame
    {
        [FunctionName("AddGame")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Games")] IAsyncCollector<GameInfo> gamesInfoTable,
            ILogger log)
        {

            var gameId = req.Query["gameId"];

            if (string.IsNullOrEmpty(gameId))
            {
                return new BadRequestObjectResult($"Failed to retrieve passed parameter 'gameId' {gameId}");
            }

            var result = await BGG_API.GetGameDetails(gameId);

            if (result == null)
            {
                return new BadRequestObjectResult($"Failed to retrieve game with ID {gameId}");
            }

            result.PartitionKey = "games";
            await gamesInfoTable.AddAsync(result);

            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return serializeObject != null
                ? (ActionResult)new OkObjectResult($"Added: {serializeObject}")
                : new BadRequestObjectResult($"Failed to add {serializeObject}");
        }
    }
}