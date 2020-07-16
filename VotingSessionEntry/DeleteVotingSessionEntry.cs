using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class DeleteVotingSessionEntry
    {
        [FunctionName("DeleteVotingSessionEntry")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("VotingSessionEntries", "votingSessionEntry")] CloudTable votingSessionEntriesTable,
            ILogger log)
        {
            var votingSessionId = req.Query["votingSessionId"];
            log.LogInformation($"Trying to delete '{votingSessionId}'");

            var retrieve = TableOperation.Retrieve<VotingSessionEntry>("votingSessionEntry", votingSessionId);
            var retrieveResult = await votingSessionEntriesTable.ExecuteAsync(retrieve);
            log.LogInformation($"retrieveResult: {retrieveResult.HttpStatusCode}");

            var deleteSession = (VotingSessionEntry)retrieveResult.Result;

            var delete = TableOperation.Delete(deleteSession);
            var deleteResult = await votingSessionEntriesTable.ExecuteAsync(delete);

            log.LogInformation($"deleteResult: {deleteResult.HttpStatusCode}");

            return deleteResult.HttpStatusCode.ToString().StartsWith("20")
                ? (ActionResult)new OkObjectResult($"Successfully deleted '{votingSessionId}'")
                : new BadRequestObjectResult($"Failed to delete {votingSessionId}");
        }
    }
}