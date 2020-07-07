$response = Invoke-RestMethod https://bgg-api.azurewebsites.net/api/GetWishlist?code=BCQEkykzV0uSZsu8lTjdABr9PyCVh3rFAMuIQYAcGH6I34WM9lvnIA==
$gameIds = $response.gameId

$baseUrl = 'https://bgg-api.azurewebsites.net/api';
#add games
foreach ($id in $gameids) {
    $urlToInvoke = $baseUrl + '/AddGame?gameId=' + $id
    Write-Host $urlToInvoke
    Invoke-RestMethod $urlToInvoke
}