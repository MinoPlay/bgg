using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace bgg
{
    public static class CheckAccess
    {
        [FunctionName("CheckAccess")]
        public static ActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            // expecting comma seperated initials
            var initials = req.Query["initials"];
            var pass = req.Query["pass"];

            if (string.IsNullOrEmpty(initials) || string.IsNullOrEmpty(pass))
            {
                return new BadRequestObjectResult($"FAILED!");
            }

            return pass == "2020" ? (ActionResult)new OkObjectResult("FINE.") : new BadRequestObjectResult($"FAILED!");
        }
    }
}