$baseUrl = 'http://bgg-api-test.azurewebsites.net/api/';
$games = Invoke-RestMethod "$($baseUrl)GetWishlist";

function AddVotingSessionEntries {
	$sessionId = Get-Random;
	$addVotingSessionUrl = "$($baseUrl)AddVotingSession?sessionId=$sessionId";
	Invoke-RestMethod $addVotingSessionUrl;
	
	foreach ($game in $games) {
		$addVotingSessionEntryId = Get-Random;
		$gameid = $game.gameId;
		$addVotingSessionEntryUrl = "$($baseUrl)AddVotingSessionEntry?votingSessionEntryId=$($addVotingSessionEntryId)&votingSessionId=$($sessionId)&gameId=$gameid";
		Write-Host $addVotingSessionEntryUrl;
		Invoke-RestMethod $addVotingSessionEntryUrl;
	}
}

AddVotingSessionEntries