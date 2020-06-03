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
async void Main()
{
	using (var client = new HttpClient())
	{
		var basicUrl = @"https://bgg-api.azurewebsites.net/api/";
		var basicUrlLocal = @"http://localhost:7071/api/";
		// add voting session

		// get games that should be part of the voting session
		var gameIds = GetGameIds(client).GetAwaiter().GetResult();

		await AddVotingSessionEntries(client, basicUrlLocal, gameIds, 0);
		await AddVotingSessionEntries(client, basicUrlLocal, gameIds, 2);
		await AddVotingSessionEntries(client, basicUrlLocal, gameIds, 5);
		await AddVotingSessionEntries(client, basicUrlLocal, gameIds, 10);
		await AddVotingSessionEntries(client, basicUrlLocal, gameIds, 15);
	}
}

public async Task AddVotingSessionEntries(HttpClient client, string basicUrl, IEnumerable<string> gameIds, int amountOfGames = 0)
{
	var random = new Random();

	var sessionId = random.Next(999999);
	var addVotingSessionUrl = $"{basicUrl}AddVotingSession?sessionId={sessionId}";
	await client.GetAsync(addVotingSessionUrl);
	
	foreach (var gameid in (amountOfGames != 0 ? gameIds.Take(amountOfGames) : gameIds))
	{
		// populate voting session
		var addVotingSessionEntryId = random.Next(999999);
		var addVotingSessionEntryUrl = $"{basicUrl}/AddVotingSessionEntry?votingSessionEntryId={addVotingSessionEntryId}votingSessionnId={sessionId}&gameId={gameid}";
		Console.WriteLine(addVotingSessionEntryUrl);
		await client.GetAsync(addVotingSessionEntryUrl);
	}
}

// get game id's
public async Task<IEnumerable<string>> GetGameIds(HttpClient client)
{
	var response = await client.GetAsync("https://bgg-api.azurewebsites.net/api/BGGGetWishlist");
	var contentAsByteArray = await response.Content.ReadAsStringAsync();
	var jsonResult = JArray.Parse(contentAsByteArray);
	var stuff = jsonResult.Children().Select(x => x.Children<JProperty>().Single(y => y.Name == "gameId").Value.ToObject<string>());
	return stuff.ToArray<string>();
}
