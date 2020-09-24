$collectionPath = "$PSScriptRoot\..\Data\collection.json"
$games = Get-Content $collectionPath | ConvertFrom-Json
$ownedGames = $games | where { $_.own -eq "1" }
$gameIds = $ownedGames.objectid

Write-Host $gameIds.count

$baseUrl = 'http://localhost:7071/api';
foreach ($id in $gameIds) {

    $addUrl = "$baseUrl/AddGame?gameId=$id"
    Write-Host $addUrl
    Invoke-RestMethod $addUrl

    $setGameStateUrl = "$baseUrl/SetGameState?gameId=$id&gameState=owned"
    Write-Host $setGameStateUrl
    Invoke-RestMethod $setGameStateUrl
}