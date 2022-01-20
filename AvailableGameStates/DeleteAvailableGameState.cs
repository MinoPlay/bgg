using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public static class DeleteAvailableGameState
    {
        [FunctionName("DeleteAvailableGameState")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("AvailableGameStates")] CloudTable availableGameStates,
            ILogger log)
        {
            var state = req.Query["state"];
            log.LogInformation($"Trying to delete '{state}'");

            var retrieve = TableOperation.Retrieve<AvailableGameState>("availableGameStates", state);
            var retrieveResult = await availableGameStates.ExecuteAsync(retrieve);
            log.LogInformation($"retrieveResult: {retrieveResult.HttpStatusCode}");

            var deleteAvailableGameState = (AvailableGameState)retrieveResult.Result;

            var delete = TableOperation.Delete(deleteAvailableGameState);
            var deleteResult = await availableGameStates.ExecuteAsync(delete);

            log.LogInformation($"deleteResult: {deleteResult.HttpStatusCode}");

            return deleteResult.HttpStatusCode.ToString().StartsWith("20")
                ? (ActionResult)new OkObjectResult($"Successfully deleted '{state}'")
                : new BadRequestObjectResult($"Failed to delete {state}");
        }
    }
}