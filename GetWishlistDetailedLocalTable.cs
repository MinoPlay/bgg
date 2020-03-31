using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class GetWishlistDetailedLocalTable
    {
        [FunctionName("GetWishlistDetailedLocalTable")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Games", "games")] CloudTable games,
            ILogger log)
        {
            var query = new TableQuery<GameInfo>();
            var segment = await games.ExecuteQuerySegmentedAsync(query, null);

            return new JsonResult(segment.Results);
        }
    }
}