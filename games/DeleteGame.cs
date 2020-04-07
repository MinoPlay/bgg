using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class DeleteGame
    {
        [FunctionName("DeleteGame")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Table("Games")] CloudTable games,
            ILogger log)
        {
            var gameId = req.Query["gameId"];
            log.LogInformation($"Trying to delete '{gameId}'");

            var retrieve = TableOperation.Retrieve<GameInfo>("games", gameId);
            var retrieveResult = await games.ExecuteAsync(retrieve);
            log.LogInformation($"retrieveResult: {retrieveResult.HttpStatusCode}");

            var deleteGame = (GameInfo)retrieveResult.Result;

            var delete = TableOperation.Delete(deleteGame);
            var deleteResult = await games.ExecuteAsync(delete);

            log.LogInformation($"deleteResult: {deleteResult.HttpStatusCode}");

            return deleteResult.HttpStatusCode.ToString().StartsWith("20")
                ? (ActionResult)new OkObjectResult($"Successfully deleted '{gameId}'")
                : new BadRequestObjectResult($"Failed to delete {gameId}");
        }
    }
}