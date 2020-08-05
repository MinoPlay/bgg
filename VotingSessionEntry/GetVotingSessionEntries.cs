using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class GetVotingSessionEntries
    {
        [FunctionName("GetVotingSessionEntries")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("VotingSessionEntries", "votingSessionEntry")] CloudTable votingSessionEntriesTable,
            ILogger log)
        {
            var query = new TableQuery<VotingSessionEntry>();
            var result = await votingSessionEntriesTable.ExecuteQuerySegmentedAsync(query, null);

            var votingSessionId = req.Query["votingSessionId"];
            log.LogInformation($"Trying to retrieve '{votingSessionId}'");
            var filteredResult = result.Results.Where(x => x.VotingSessionId == votingSessionId);

            return new JsonResult(filteredResult);
        }
    }
}