For testing:

void Main()
{
GetList();
}

public async void GetList()
{
var client = new HttpClient();
var response = await client.GetAsync("https://bgg-api.azurewebsites.net/api/GetWishlist?code=BCQEkykzV0uSZsu8lTjdABr9PyCVh3rFAMuIQYAcGH6I34WM9lvnIA==");
var contentAsByteArray = await response.Content.ReadAsStringAsync();
var jsonResult = JArray.Parse(contentAsByteArray);
var stuff = jsonResult.Children().Select(x => x.Children<JProperty>().Single(y => y.Name == "gameId").Value);

    foreach (var gameId in stuff)
    {
    	var client2 = new HttpClient();
    	Console.WriteLine($"gameID: {gameId}");

    	var success = false;
    	while (!success)
    	{
    		var param = new Dictionary<string, string>();
    		param.Add("gameId", gameId.ToString());
    		var encodedContent = new FormUrlEncodedContent(param);
    		var content = new StringContent("");
    		//var response2 = await client2.PostAsync($"https://bgg-api.azurewebsites.net/api/AddDetailedGame?code=h52Eb2wjrkrXCOv7OY9OvU5FsI3Vgvdk73Dq1wePB479FL2pRHb/Ig==&gameId={gameId}", content);
    		var response2 = await client2.PostAsync($"http://localhost:7071/api/AddDetailedGame?gameId={gameId}", content);
    		Console.WriteLine(response2.StatusCode);
    		if (response2.IsSuccessStatusCode)
    			success = true;
    		else
    			Thread.Sleep(1000);
    	}
    }

}

// Define other methods and classes here
