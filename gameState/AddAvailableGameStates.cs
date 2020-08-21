using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class AddAvailableGameStates
    {
        [FunctionName("AddAvailableGameStates")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("AvailableStates")] IAsyncCollector<State> availableStates,
            ILogger log)
        {
            var newState = req.Query["newState"].ToString().ToLower();

            if (string.IsNullOrEmpty(newState))
            {
                return new BadRequestObjectResult($"Failed to retrieve passed parameter 'newState' {newState}");
            }
            var result = new State()
            {
                PartitionKey = "availableState",
                RowKey = newState,
                availableState = newState
            };

            await availableStates.AddAsync(result);

            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return serializeObject != null
                ? (ActionResult)new OkObjectResult($"Added: {serializeObject}")
                : new BadRequestObjectResult($"Failed to add {serializeObject}");
        }
    }
}