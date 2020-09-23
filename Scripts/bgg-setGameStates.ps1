$response = Invoke-RestMethod 'http://localhost:7071/api/GetWishlist'
$gameIds = $response.gameId


$availableStates = "owned", "wishlist", "voted", "discarded", "previously";

$baseUrl = 'http://localhost:7071/api';
$gameState = "wishlist"
# $baseUrl = 'https://bgg-api-test.azurewebsites.net/api';
#add games
foreach ($gameId in $gameIds) {
    $urlToInvoke = "$baseUrl/SetGameState?gameId=$gameId&gameState=$gameState"
    Write-Host $urlToInvoke
    Invoke-RestMethod $urlToInvoke
}