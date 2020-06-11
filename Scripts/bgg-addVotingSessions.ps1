$baseUrl = 'http://localhost:7071/api/';
$games = Invoke-RestMethod "$($baseUrl)GetWishlist";

function AddVotingSessionEntries($amountOfGames) {
	$sessionId = Get-Random;
	$addVotingSessionUrl = "$($baseUrl)AddVotingSession?sessionId=$sessionId";
	Invoke-RestMethod $addVotingSessionUrl;
	
	for (($i = 0); ($i -lt $amountOfGames); $i++) {
		$addVotingSessionEntryId = Get-Random;
		$gameid = $games[$i].gameId;
		$addVotingSessionEntryUrl = "$($baseUrl)AddVotingSessionEntry?votingSessionEntryId=$($addVotingSessionEntryId)&votingSessionId=$($sessionId)&gameId=$gameid";
		Write-Host $addVotingSessionEntryUrl;
		Invoke-RestMethod $addVotingSessionEntryUrl;
	}
}

AddVotingSessionEntries(5)
AddVotingSessionEntries(7)
AddVotingSessionEntries(10)
AddVotingSessionEntries(15)
AddVotingSessionEntries(20)