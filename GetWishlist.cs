using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Xml.Linq;
using System.Linq;

namespace bgg
{
    public static class GetWishlist
    {
        [FunctionName("GetWishlist")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function GetWishlist start processing a request.");
            var client = new HttpClient();
            var response = await client.GetAsync("https://boardgamegeek.com/xmlapi2/collection?username=WDHBoardGameClub&wishlistpriority=3");
            var contentAsByteArray = await response.Content.ReadAsByteArrayAsync();
            var contentAsString = System.Text.Encoding.UTF8.GetString(contentAsByteArray);
            var contentAsXml = XElement.Parse(contentAsString);

            var takeWhatIWant = contentAsXml.Elements("item").Select(x => new
            {
                objectId = x.Attribute("objectid").Value,
                nameof = x.Element("name").Value
            });

            return new JsonResult(takeWhatIWant);
        }
    }
}
