using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

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

            // update time
            // var gameTitle = req.Query["gameTitle"].ToString().TrimStart('\'').TrimEnd('\'');
            // if (!string.IsNullOrEmpty(gameTitle) && result.gameTitle != gameTitle)
            // {
            //     result.gameTitle = gameTitle;
            // }
            // var thumbnail = req.Query["thumbnail"];
            // if (!string.IsNullOrEmpty(thumbnail) && result.thumbnail != thumbnail)
            // {
            //     result.thumbnail = thumbnail;
            // }
            // var description = req.Query["description"].ToString().TrimStart('\'').TrimEnd('\'');
            // if (!string.IsNullOrEmpty(description) && result.description != description)
            // {
            //     result.description = description;
            // }
            var minplayers = req.Query["minplayers"];
            if (!string.IsNullOrEmpty(minplayers) && result.minplayers != minplayers)
            {
                result.minplayers = minplayers;
            }
            var maxplayers = req.Query["maxplayers"];
            if (!string.IsNullOrEmpty(maxplayers) && result.maxplayers != maxplayers)
            {
                result.maxplayers = maxplayers;
            }
            var minplaytime = req.Query["minplaytime"];
            if (!string.IsNullOrEmpty(minplaytime) && result.minplaytime != minplaytime)
            {
                result.minplaytime = minplaytime;
            }
            var maxplaytime = req.Query["maxplaytime"];
            if (!string.IsNullOrEmpty(maxplaytime) && result.maxplaytime != maxplaytime)
            {
                result.maxplaytime = maxplaytime;
            }
            var minage = req.Query["minage"];
            if (!string.IsNullOrEmpty(minage) && result.minage != minage)
            {
                result.minage = minage;
            }
            var languageDependence = req.Query["languageDependence"];
            if (!string.IsNullOrEmpty(languageDependence) && result.languageDependence != languageDependence)
            {
                result.languageDependence = languageDependence;
            }
            var averageWeight = req.Query["averageWeight"];
            if (!string.IsNullOrEmpty(averageWeight) && result.averageWeight != averageWeight)
            {
                result.averageWeight = averageWeight;
            }

            var update = TableOperation.Replace(result);
            var updateResult = await games.ExecuteAsync(update);


            return result != null
                ? (ActionResult)new JsonResult(updateResult)
                : new BadRequestObjectResult($"Failed to update {gameId}");
        }
    }
}