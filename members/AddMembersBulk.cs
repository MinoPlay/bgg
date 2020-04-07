using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class AddMembersBulk
    {
        [FunctionName("AddMembersBulk")]
        public static async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("Members")] IAsyncCollector<Member> membersTable,
            ILogger log)
        {
            // expecting comma seperated initials
            var initials = req.Query["initials"];

            foreach (var init in ((string)initials).Split(','))
            {
                var result = new Member()
                {
                    PartitionKey = "member",
                    RowKey = init,
                    Initials = init
                };

                await membersTable.AddAsync(result);
            }

            return (ActionResult)new OkObjectResult($"Bulk initials added: {initials}");
        }
    }
}