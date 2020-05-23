using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class DeleteVotingSession
    {
        [FunctionName("DeleteVotingSession")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("VotingSessions", "votingSession")] CloudTable votingSessions,
            ILogger log)
        {
            var sessionId = req.Query["sessionId"];
            log.LogInformation($"Trying to delete '{sessionId}'");

            var retrieve = TableOperation.Retrieve<VotingSession>("votingSession", sessionId);
            var retrieveResult = await votingSessions.ExecuteAsync(retrieve);
            log.LogInformation($"retrieveResult: {retrieveResult.HttpStatusCode}");

            var deleteSession = (VotingSession)retrieveResult.Result;

            var delete = TableOperation.Delete(deleteSession);
            var deleteResult = await votingSessions.ExecuteAsync(delete);

            log.LogInformation($"deleteResult: {deleteResult.HttpStatusCode}");

            return deleteResult.HttpStatusCode.ToString().StartsWith("20")
                ? (ActionResult)new OkObjectResult($"Successfully deleted '{sessionId}'")
                : new BadRequestObjectResult($"Failed to delete {sessionId}");
        }
    }
}