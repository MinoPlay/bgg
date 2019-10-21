
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class cloneBggToLocalTable
    {
        [FunctionName("cloneBggToLocalTable")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] GameInfo game,
            [Table("Games")] IAsyncCollector<GameInfo> gamesInfoTable,
            ILogger log)
        {

            game.PartitionKey = "games";
            game.RowKey = game.gameTitle;
            await gamesInfoTable.AddAsync(game);

            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(game);

            return serializeObject != null
                ? (ActionResult)new OkObjectResult($"Added: {serializeObject}")
                : new BadRequestObjectResult($"Failed to add {serializeObject}");
        }
    }
}