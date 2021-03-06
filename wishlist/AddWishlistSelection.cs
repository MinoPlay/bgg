using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public static class AddWishlistSelection
    {
        [FunctionName("AddWishlistSelection")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "get", Route = null)] HttpRequest req,
            [Table("Games", "games")] CloudTable games,
            [Table("WishlistSelections")] IAsyncCollector<WishlistSelection> wishlistSelectionTable,
            ILogger log)
        {
            // can use req.Query since I am dealing with simple strings
            var userId = req.Query["UserId"].ToString().ToUpper();
            var gameSelection = req.Query["GameSelection"];
            var gameWeight = req.Query["GameWeight"];

            // get game title to add as a part of wishlist
            var retrieveGame = TableOperation.Retrieve<GameInfo>("games", gameSelection);
            var retrieveGameResult = await games.ExecuteAsync(retrieveGame);
            var gameResult = (GameInfo)retrieveGameResult.Result;

            var result = new WishlistSelection
            {
                PartitionKey = "wishlistSelections",
                RowKey = Guid.NewGuid().ToString(),
                UserId = userId,
                GameSelection = gameSelection,
                GameTitle = gameResult.gameTitle,
                GameWeight = gameWeight
            };

            await wishlistSelectionTable.AddAsync(result);

            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return serializeObject != null
                ? (ActionResult)new OkObjectResult($"Added: {serializeObject}")
                : new BadRequestObjectResult($"Failed to add {serializeObject}");
        }
    }
}