using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using System.Linq;

namespace bgg
{
    public static class GetWishlist
    {
        [FunctionName("GetWishlist")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Games", "games")] CloudTable games,
            [Table("VotingSessionEntries", "votingSessionEntry")] CloudTable votingSessionEntriesTable,
            ILogger log)
        {
            var query = new TableQuery<GameInfo>();
            var segment = await games.ExecuteQuerySegmentedAsync(query, null);

            var sessionId = req.Query["sessionId"];
            if (!string.IsNullOrEmpty(sessionId))
            {
                var query2 = new TableQuery<VotingSessionEntry>();
                var retrievedSessionEntries = await votingSessionEntriesTable.ExecuteQuerySegmentedAsync(query2, null);

                log.LogInformation($"Trying to retrieve games for session: '{sessionId}'");
                var sessionEntries = retrievedSessionEntries.Results.Where(x => x.VotingSessionId == sessionId);
                var result = segment.Results.Where(x => sessionEntries.Any(y => y.GameId == x.GameId));

                return new JsonResult(result);
            }

            return new JsonResult(segment.Results);
        }
    }
}