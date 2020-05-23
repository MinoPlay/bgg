using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class AddVotingSession
    {
        [FunctionName("AddVotingSession")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("VotingSessions")] IAsyncCollector<VotingSession> votingSessionsTable,
            ILogger log)
        {

            var sessionId = req.Query["sessionId"];

            if (string.IsNullOrEmpty(sessionId))
            {
                return new BadRequestObjectResult($"Failed to retrieve passed parameter 'sessionId' [{sessionId}]");
            }
            var result = new VotingSession()
            {
                PartitionKey = "votingSession",
                RowKey = sessionId,
                SessionId = sessionId,
                Active = false
            };

            await votingSessionsTable.AddAsync(result);

            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return serializeObject != null
                ? (ActionResult)new OkObjectResult($"Added: {serializeObject}")
                : new BadRequestObjectResult($"Failed to add {serializeObject}");
        }
    }
}