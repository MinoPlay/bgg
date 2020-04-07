using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class GetMember
    {
        [FunctionName("GetMember")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Members", "member")] CloudTable members,
            ILogger log)
        {
            var reqInitials = req.Query["initials"];

            var retrieve = TableOperation.Retrieve<Member>("member", reqInitials);
            var retrieveResult = await members.ExecuteAsync(retrieve);
            var result = (Member)retrieveResult.Result;

            return result != null ? new JsonResult(result) : new JsonResult($"Member with initials '{reqInitials}' not found.");
        }
    }
}