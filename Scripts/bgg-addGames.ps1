$response = Invoke-RestMethod https://bgg-api.azurewebsites.net/api/GetWishlist?code=BCQEkykzV0uSZsu8lTjdABr9PyCVh3rFAMuIQYAcGH6I34WM9lvnIA==
$gameIds = $response.gameId

# $baseUrl = 'https://bgg-api-test.azurewebsites.net/api';
$baseUrl = 'http://localhost:7071/api';
#add games
foreach ($id in $gameids) {
    $urlToInvokeRemove = $baseUrl + '/DeleteGame?gameId=' + $id
    Write-Host 'removing: '$id
    Invoke-RestMethod $urlToInvokeRemove
    $urlToInvoke = $baseUrl + '/AddGame?gameId=' + $id
    Write-Host 'adding: '$id
    Invoke-RestMethod $urlToInvoke
}