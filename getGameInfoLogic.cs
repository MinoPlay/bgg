using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;

namespace bgg
{
    public static class GetGameInfoLogic
    {
        public static async Task<IEnumerable<KeyValuePair<string, string>>> GetWishlist()
        {
            var client = new HttpClient();

            var response = await client.GetAsync("https://boardgamegeek.com/xmlapi2/collection?username=WDHBoardGameClub&wishlistpriority=3");

            var contentAsByteArray = await response.Content.ReadAsByteArrayAsync();
            var contentAsString = System.Text.Encoding.UTF8.GetString(contentAsByteArray);
            var contentAsXml = XElement.Parse(contentAsString);

            var takeWhatIWant = contentAsXml.Elements("item").Select(x => new KeyValuePair<string, string>(x.Attribute("objectid").Value, x.Element("name").Value));

            return takeWhatIWant;
        }

        public static async Task<GameInfo> GetGameDetails(string gameId)
        {
            var client = new HttpClient();

            var response = await client.GetAsync($"https://boardgamegeek.com/xmlapi2/thing?id={gameId}");

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

            var result = new GameInfo
            {
                gameTitle = gameTitle,
                thumbnail = thumbnail,
                description = description,
                minplayers = minplayers,
                maxplayers = maxplayers,
                minplaytime = minplaytime,
                maxplaytime = maxplaytime,
                minage = minage
            };

            return result;
        }
    }
}