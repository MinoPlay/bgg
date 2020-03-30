using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bgg
{
    public static class AddWishlistSelection
    {
        [FunctionName("AddWishlistSelection")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", "get", Route = null)] HttpRequest req,
            [Table("WishlistSelections")] IAsyncCollector<WishlistSelection> wishlistSelectionTable,
            ILogger log)
        {
            // can use req.Query since I am dealing with simple strings
            var userId = req.Query["UserId"];
            var gamesSelections = req.Query["GameSelections"];

            // string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // dynamic data = JObject.Parse(requestBody);

            // var wishlistSelectionId = Guid.NewGuid().ToString();
            // var userId = data["UserId"].Value;
            // var gamesSelections = data["GameSelections"].Value;

            var result = new WishlistSelection
            {
                PartitionKey = "wishlistSelections",
                RowKey = Guid.NewGuid().ToString(),
                UserId = userId,
                GamesSelections = gamesSelections
            };

            await wishlistSelectionTable.AddAsync(result);

            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return serializeObject != null
                ? (ActionResult)new OkObjectResult($"Added: {serializeObject}")
                : new BadRequestObjectResult($"Failed to add {serializeObject}");
        }
    }
}