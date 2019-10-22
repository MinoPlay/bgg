using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class AddDetailedGame
    {
        [FunctionName("AddDetailedGame")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Table("Games")] IAsyncCollector<GameInfo> gamesInfoTable,
            ILogger log)
        {

            var gameId = req.Query["gameId"];
            var result = await GetGameInfoLogic.GetGameDetails(gameId);

            result.PartitionKey = "games";
            result.RowKey = result.gameTitle;
            await gamesInfoTable.AddAsync(result);

            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return serializeObject != null
                ? (ActionResult)new OkObjectResult($"Added: {serializeObject}")
                : new BadRequestObjectResult($"Failed to add {serializeObject}");
        }
    }
}