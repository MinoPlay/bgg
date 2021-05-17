using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public static class GetVotingSession
    {
        [FunctionName("GetVotingSession")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("VotingSessions", "votingSession")] CloudTable votingSessions,
            ILogger log)
        {
            var sessionId = req.Query["sessionId"];
            log.LogInformation($"Trying to retrieve '{sessionId}'");

            if (!string.IsNullOrEmpty(sessionId) && sessionId == "active")
            {
                var allVotingSessions = await votingSessions.ExecuteQuerySegmentedAsync(new TableQuery<VotingSession>(), null);
                var activeSession = allVotingSessions.Results.SingleOrDefault(x => x.Active);
                return activeSession != null ? (ActionResult)new JsonResult(activeSession) : new BadRequestObjectResult($"Failed to get {sessionId}");
            }

            var retrieve = TableOperation.Retrieve<VotingSession>("votingSession", sessionId);
            var retrieveResult = await votingSessions.ExecuteAsync(retrieve);
            var result = (VotingSession)retrieveResult.Result;

            return result != null
                ? (ActionResult)new JsonResult(result)
                : new BadRequestObjectResult($"Failed to get {sessionId}");
        }
    }
}