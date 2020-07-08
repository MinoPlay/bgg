using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class AddMember
    {
        [FunctionName("AddMember")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Members")] IAsyncCollector<Member> membersTable,
            ILogger log)
        {

            var initials = req.Query["initials"].ToString().ToUpper();

            if (string.IsNullOrEmpty(initials))
            {
                return new BadRequestObjectResult($"Failed to retrieve passed parameter 'initials' {initials}");
            }
            var result = new Member()
            {
                PartitionKey = "member",
                RowKey = initials,
                Initials = initials,
                Role = 0
            };

            await membersTable.AddAsync(result);

            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return serializeObject != null
                ? (ActionResult)new OkObjectResult($"Added: {serializeObject}")
                : new BadRequestObjectResult($"Failed to add {serializeObject}");
        }
    }
}