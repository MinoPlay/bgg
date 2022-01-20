using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using System.Collections.Generic;

namespace bgg
{
    public static class BGG_API
    {
        public static async Task<List<GameInfo>> GetDetailedWishlist()
        {
            var wishlist = await GetWishlist();
            var wishlistDetails = new List<GameInfo>();

            foreach (var item in wishlist)
            {
                var itemDetails = await GetGameDetails(item.Key);
                wishlistDetails.Add(itemDetails);
            }
            return wishlistDetails;
        }

        public static async Task<IEnumerable<KeyValuePair<string, string>>> GetWishlist()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = null;

                var retries = 0;
                while (retries < 5 && (response == null || response.StatusCode != HttpStatusCode.OK))
                {
                    response = await client.GetAsync("https://boardgamegeek.com/xmlapi2/collection?username=WDHBoardGameClub&wishlistpriority=3");

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Thread.Sleep(1000);
                        retries++;
                    }
                }

                var contentAsByteArray = await response.Content.ReadAsByteArrayAsync();
                var contentAsString = System.Text.Encoding.UTF8.GetString(contentAsByteArray);
                var contentAsXml = XElement.Parse(contentAsString);

                var takeWhatIWant = contentAsXml.Elements("item").Select(x => new KeyValuePair<string, string>(x.Attribute("objectid").Value, x.Element("name").Value));

                return takeWhatIWant;
            }
        }

        public static async Task<GameInfo> GetGameDetails(string gameId)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = null;

                var retries = 0;
                while (retries < 5 && (response == null || response.StatusCode != HttpStatusCode.OK))
                {
                    response = await client.GetAsync($"https://boardgamegeek.com/xmlapi2/thing?id={gameId}&stats=1");

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Thread.Sleep(1000);
                        retries++;
                    }
                }

                var contentAsByteArray = await response.Content.ReadAsByteArrayAsync();
                var contentAsString = System.Text.Encoding.UTF8.GetString(contentAsByteArray);

                var contentAsXml = XElement.Parse(contentAsString);
                var dataDump = contentAsXml.Element("item");

                var gameTitle = dataDump.Element("name").Attribute("value").Value;
                var thumbnail = dataDump.Element("thumbnail").Value;
                var description = dataDump.Element("description").Value;

                var minplayers = dataDump.Element("minplayers").Attribute("value").Value;
                var maxplayers = dataDump.Element("maxplayers").Attribute("value").Value;

                var minplaytime = dataDump.Element("minplaytime").Attribute("value").Value;
                var maxplaytime = dataDump.Element("maxplaytime").Attribute("value").Value;
                var minage = dataDump.Element("minage").Attribute("value").Value;

                var languagedependentPoolResults = dataDump.Elements("poll").Single(x => x.Attribute("name").Value == "language_dependence").Element("results").Elements("result").Select(
                    x => new
                    {
                        value = x.Attribute("value").Value,
                        numvotes = x.Attribute("numvotes").Value
                    });

                //conversion table to shorten text
                var toShortLanguageDependent = new Dictionary<string, string>()
                {
                    { "No necessary in-game text", "none"},
                    { "Some necessary text - easily memorized or small crib sheet", "some" },
                    { "Moderate in-game text - needs crib sheet or paste ups", "medium" },
                    { "Extensive use of text - massive conversion needed to be playable", "extensive" },
                    { "Unplayable in another language", "a must" }
                };

                //if there are no votes?
                var languagedependent = languagedependentPoolResults.All(x => x.numvotes == "0") ?
                    "unknown" :
                    toShortLanguageDependent[languagedependentPoolResults.OrderBy(x => x.numvotes).Last().value];

                var averageweight = dataDump.Element("statistics").Element("ratings").Element("averageweight").Attribute("value").Value;
                var ranking = dataDump.Element("statistics").Element("ratings").Element("average").Attribute("value").Value;

                var result = new GameInfo
                {
                    PartitionKey = "games",
                    RowKey = gameId,
                    GameId = gameId,
                    GameTitle = gameTitle,
                    Thumbnail = thumbnail,
                    Description = description,
                    MinPlayers = minplayers,
                    MaxPlayers = maxplayers,
                    MinPlaytime = minplaytime,
                    MaxPlaytime = maxplaytime,
                    MinAge = minage,
                    LanguageDependence = languagedependent,
                    AverageWeight = averageweight,
                    Ranking = ranking,
                    GameState = "unknown"
                };

                return result;
            }
        }
    }
}