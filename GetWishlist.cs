using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Xml.Linq;

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
            var conntentAsXml = XElement.Parse(contentAsString);
            Console.WriteLine(conntentAsXml);


            log.LogInformation("C# HTTP trigger function GetWishlist start processing a request.");
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
