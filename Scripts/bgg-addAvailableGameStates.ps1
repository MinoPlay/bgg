$availableStates = {
    "owned",     
    "wishlist",  
    "voted",     
    "discarded", 
    "previously" 
};

$baseUrl = 'http://localhost:7071/api';
# $baseUrl = 'https://bgg-api-test.azurewebsites.net/api';
#add games
foreach ($availableState in $availableStates) {
    $urlToInvoke = $baseUrl + '/AddAvailableGameStates?newState=' + $availableState
    Write-Host $urlToInvoke
    Invoke-RestMethod $urlToInvoke
}