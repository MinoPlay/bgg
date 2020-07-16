using System;
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
            var votingSessionId = req.Query["votingSessionId"];
            var gameId = req.Query["gameId"];

            if (string.IsNullOrEmpty(votingSessionId) || string.IsNullOrEmpty(gameId))
            {
                return new BadRequestObjectResult($"Failed to retrieve passed parameters: votingSessionnId[{votingSessionId}], gameId[{gameId}]");
            }

            var votingSessionEntryId = Guid.NewGuid().ToString();
            var result = new VotingSessionEntry()
            {
                PartitionKey = "votingSessionEntry",
                RowKey = votingSessionEntryId,
                VotingSessionEntryId = votingSessionEntryId,
                VotingSessionId = votingSessionId,
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