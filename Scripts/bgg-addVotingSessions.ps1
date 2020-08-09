$baseUrl = 'http://localhost:7071/api/';
# $baseUrl = 'https://bgg-api-test.azurewebsites.net/api';
$games = Invoke-RestMethod "$($baseUrl)GetWishlist";

function AddVotingSessionEntries {
	$sessionId = Get-Random;
	$addVotingSessionUrl = "$($baseUrl)AddVotingSession?sessionId=$sessionId";
	Invoke-RestMethod $addVotingSessionUrl;
	
	$numb = 0;
	foreach ($game in $games) {
		$addVotingSessionEntryId = Get-Random;
		$gameid = $game.gameId;
		$addVotingSessionEntryUrl = "$($baseUrl)AddVotingSessionEntry?votingSessionEntryId=$($addVotingSessionEntryId)&votingSessionId=$($sessionId)&gameId=$gameid";
		Write-Host $addVotingSessionEntryUrl;
		Invoke-RestMethod $addVotingSessionEntryUrl;
		$numb += 1;

		# if ($numb -eq 10) {
		# 	return;
		# }
	}
}

AddVotingSessionEntries