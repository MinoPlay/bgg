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
    public static class UpdateVotingSessionEntry
    {
        [FunctionName("UpdateVotingSessionEntry")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Games", "games")] CloudTable games,
            [Table("VotingSessionEntries", "votingSessionEntry")] CloudTable votingSessionEntriesTable,
            ILogger log)
        {
            var query = new TableQuery<VotingSessionEntry>();
            var result = await votingSessionEntriesTable.ExecuteQuerySegmentedAsync(query, null);

            var votingSessionId = req.Query["votingSessionId"];
            log.LogInformation($"Trying to retrieve '{votingSessionId}'");
            var filteredResult = result.Results.Where(x => x.VotingSessionId == votingSessionId);

            foreach (var entry in filteredResult)
            {
                // get game title to add as a part of wishlist
                var retrieveGame = TableOperation.Retrieve<GameInfo>("games", entry.GameId);
                var retrieveGameResult = await games.ExecuteAsync(retrieveGame);
                var gameResult = (GameInfo)retrieveGameResult.Result;

                entry.GameTitle = gameResult.gameTitle;
                var update = TableOperation.Replace(entry);
                var updateResult = await votingSessionEntriesTable.ExecuteAsync(update);
            }

            return new JsonResult(filteredResult);
        }
    }
}