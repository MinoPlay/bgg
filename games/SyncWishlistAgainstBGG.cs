using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace bgg
{
    public static class SyncWishlistAgainstBGG
    {
        [FunctionName("SyncWishlistAgainstBGG")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Games", "games")] CloudTable games,
            [Table("Games")] IAsyncCollector<GameInfo> gamesInfoTable,
            ILogger log)
        {
            // get all games from bgg
            var bggDetailedwishlist = await BGG_API.GetDetailedWishlist();

            // get all games from cloudTable
            var query = new TableQuery<GameInfo>();
            var cloudTableResponse = await games.ExecuteQuerySegmentedAsync(query, null);
            var cloudTableGames = cloudTableResponse.Results;

            // check what's missing
            var newlyAddedGames = new List<GameInfo>();
            foreach (var bggWishlistGame in bggDetailedwishlist)
            {
                var match = cloudTableGames.SingleOrDefault(x => x.gameId == bggWishlistGame.gameId);

                if (match == null)
                {
                    // insert the new game
                    log.LogInformation($"SyncAgainstBggWishlist > Inserting new game from the wishlist: ${bggWishlistGame.gameTitle}");
                    await gamesInfoTable.AddAsync(bggWishlistGame);
                    newlyAddedGames.Add(bggWishlistGame);
                }
                else
                {
                    // update info? nooohhh for now
                }
            }

            return (ActionResult)new JsonResult(newlyAddedGames);
        }
    }
}