using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class GetAllVotingSessions
    {
        [FunctionName("GetAllVotingSessions")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("VotingSessions", "votingSession")] CloudTable votingSessionsTable,
            ILogger log)
        {
            var query = new TableQuery<VotingSession>();
            var result = await votingSessionsTable.ExecuteQuerySegmentedAsync(query, null);

            var sessionId = req.Query["sessionId"];
            if (!string.IsNullOrEmpty(sessionId) && sessionId == "active")
            {
                var active = result.Results.Where(x => x.Active);
                return new JsonResult(active);
            }

            return new JsonResult(result.Results);
        }
    }
}