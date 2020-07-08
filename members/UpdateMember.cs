using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class UpdateMember
    {
        [FunctionName("UpdateMember")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Members", "member")] CloudTable members,
            ILogger log)
        {
            var reqInitials = req.Query["initials"].ToString().ToUpper();
            var newRole = int.Parse(req.Query["role"]);

            var retrieve = TableOperation.Retrieve<Member>("member", reqInitials);
            var retrieveResult = await members.ExecuteAsync(retrieve);
            var result = (Member)retrieveResult.Result;

            log.LogInformation($"Trying to update '{reqInitials}' role. From: {result.Role} To: {newRole}");

            result.Role = newRole;

            var update = TableOperation.Replace(result);
            var updateResult = await members.ExecuteAsync(update);


            return result != null
                ? (ActionResult)new OkObjectResult($"Updated member: {result}")
                : new BadRequestObjectResult($"Failed to update {reqInitials}");
        }
    }
}