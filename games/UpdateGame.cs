using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public static class UpdateGame
    {
        [FunctionName("UpdateGame")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Games", "games")] CloudTable games,
            ILogger log)
        {
            var gameId = req.Query["gameId"];

            var retrieve = TableOperation.Retrieve<GameInfo>("games", gameId);
            var retrieveResult = await games.ExecuteAsync(retrieve);

            if (retrieveResult.HttpStatusCode == 404)
            {
                return new BadRequestObjectResult($"Game {gameId} not found.");
            }

            var result = (GameInfo)retrieveResult.Result;

            var minplayers = req.Query["minplayers"];
            if (!string.IsNullOrEmpty(minplayers) && result.MinPlayers != minplayers)
            {
                result.MinPlayers = minplayers;
            }
            var maxplayers = req.Query["maxplayers"];
            if (!string.IsNullOrEmpty(maxplayers) && result.MaxPlayers != maxplayers)
            {
                result.MaxPlayers = maxplayers;
            }
            var minplaytime = req.Query["minplaytime"];
            if (!string.IsNullOrEmpty(minplaytime) && result.MinPlaytime != minplaytime)
            {
                result.MinPlaytime = minplaytime;
            }
            var maxplaytime = req.Query["maxplaytime"];
            if (!string.IsNullOrEmpty(maxplaytime) && result.MaxPlaytime != maxplaytime)
            {
                result.MaxPlaytime = maxplaytime;
            }
            var minage = req.Query["minage"];
            if (!string.IsNullOrEmpty(minage) && result.MinAge != minage)
            {
                result.MinAge = minage;
            }
            var languageDependence = req.Query["languageDependence"];
            if (!string.IsNullOrEmpty(languageDependence) && result.LanguageDependence != languageDependence)
            {
                result.LanguageDependence = languageDependence;
            }
            var averageWeight = req.Query["averageWeight"];
            if (!string.IsNullOrEmpty(averageWeight) && result.AverageWeight != averageWeight)
            {
                result.AverageWeight = averageWeight;
            }
            var gameState = req.Query["gameState"];
            if (!string.IsNullOrEmpty(gameState) && result.GameState != gameState)
            {
                result.GameState = gameState;
            }

            var update = TableOperation.Replace(result);
            var updateResult = await games.ExecuteAsync(update);


            return result != null
                ? (ActionResult)new JsonResult(updateResult)
                : new BadRequestObjectResult($"Failed to update {gameId}");
        }
    }
}