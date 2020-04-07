using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class DeleteMember
    {
        [FunctionName("DeleteMember")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Members", "member")] CloudTable members,
            ILogger log)
        {
            var reqInitials = req.Query["initials"];
            log.LogInformation($"Trying to delete '{reqInitials}'");

            var retrieve = TableOperation.Retrieve<Member>("member", reqInitials);
            var retrieveResult = await members.ExecuteAsync(retrieve);
            log.LogInformation($"retrieveResult: {retrieveResult.HttpStatusCode}");

            var deleteMember = (Member)retrieveResult.Result;

            var delete = TableOperation.Delete(deleteMember);
            var deleteResult = await members.ExecuteAsync(delete);

            log.LogInformation($"deleteResult: {deleteResult.HttpStatusCode}");

            return deleteResult.HttpStatusCode.ToString().StartsWith("20")
                ? (ActionResult)new OkObjectResult($"Successfully deleted '{reqInitials}'")
                : new BadRequestObjectResult($"Failed to delete {reqInitials}");
        }
    }
}