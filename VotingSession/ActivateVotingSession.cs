using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class ActivateVotingSession
    {
        [FunctionName("ActivateVotingSession")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("VotingSessions", "votingSession")] CloudTable votingSessions,
            ILogger log)
        {
            var sessionId = req.Query["sessionId"];

            var retrieve = TableOperation.Retrieve<VotingSession>("votingSession", sessionId);
            var retrieveResult = await votingSessions.ExecuteAsync(retrieve);

            if (retrieveResult.HttpStatusCode == 404)
            {
                return new BadRequestObjectResult($"Session id: {sessionId} not found.");
            }

            // set all to disabled                
            var emptyQuery = new TableQuery<VotingSession>();
            var allSessions = await votingSessions.ExecuteQuerySegmentedAsync(emptyQuery, null);

            foreach (var session in allSessions)
            {
                session.Active = false;
                var updateSession = TableOperation.Replace(session);
                await votingSessions.ExecuteAsync(updateSession);
            }

            var result = (VotingSession)retrieveResult.Result;
            result.Active = true;
            var update = TableOperation.Replace(result);
            var updateResult = await votingSessions.ExecuteAsync(update);

            return new JsonResult(updateResult.Result);
        }
    }
}