using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class UpdateVotingSession
    {
        [FunctionName("UpdateVotingSession")]
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

            var result = (VotingSession)retrieveResult.Result;

            var isActive = req.Query["active"];
            if (!string.IsNullOrEmpty(isActive))
            {
                result.Active = bool.Parse(isActive);

                var update = TableOperation.Replace(result);
                var updateResult = await votingSessions.ExecuteAsync(update);

                return (ActionResult)new JsonResult(updateResult);
            }


            return result != null
                ? (ActionResult)new JsonResult($"Activation state is not changed, hence nothing to update.")
                : new BadRequestObjectResult($"Failed to update {sessionId}");
        }
    }
}