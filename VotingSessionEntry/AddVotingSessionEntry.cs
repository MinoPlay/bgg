using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class AddVotingSessionEntry
    {
        [FunctionName("AddVotingSessionEntry")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("VotingSessionEntries")] IAsyncCollector<VotingSessionEntry> votingSessionEntriesTable,
            ILogger log)
        {

            var votingSessionEntryId = req.Query["votingSessionEntryId"];
            var votingSessionnId = req.Query["votingSessionnId"];
            var gameId = req.Query["gameId"];

            if (string.IsNullOrEmpty(votingSessionEntryId) || string.IsNullOrEmpty(votingSessionnId) || string.IsNullOrEmpty(gameId))
            {
                return new BadRequestObjectResult($"Failed to retrieve passed parameters: votingSessionEntryId[{votingSessionEntryId}], votingSessionnId[{votingSessionnId}], gameId[{gameId}]");
            }

            var result = new VotingSessionEntry()
            {
                PartitionKey = "votingSessionEntry",
                RowKey = votingSessionEntryId,
                VotingSessionEntryId = votingSessionEntryId,
                VotingSessionnId = votingSessionnId,
                GameId = gameId
            };

            await votingSessionEntriesTable.AddAsync(result);

            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return serializeObject != null
                ? (ActionResult)new OkObjectResult($"Added: {serializeObject}")
                : new BadRequestObjectResult($"Failed to add {serializeObject}");
        }
    }
}