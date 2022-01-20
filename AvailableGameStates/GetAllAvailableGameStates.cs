using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public static class GetAllAvailableGameStates
    {
        [FunctionName("GetAllAvailableGameStates")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("AvailableGameStates", "availableGameStates")] CloudTable availableGameStates,
            ILogger log)
        {
            var query = new TableQuery<AvailableGameState>();
            var segment = await availableGameStates.ExecuteQuerySegmentedAsync(query, null);

            return new JsonResult(segment.Results);
        }
    }
}