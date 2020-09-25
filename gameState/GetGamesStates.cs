using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class GetAllGamesStates
    {
        [FunctionName("GetAllGamesStates")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("AvailableStates", "availableState")] CloudTable availableStates,
            ILogger log)
        {
            var query = new TableQuery<State>();
            var segment = await availableStates.ExecuteQuerySegmentedAsync(query, null);

            return new JsonResult(segment.Results);
        }
    }
}