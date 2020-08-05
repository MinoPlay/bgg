$baseUrl = 'http://bgg-api-test.azurewebsites.net/api/';
$games = Invoke-RestMethod "$($baseUrl)GetWishlist";
$members = Invoke-RestMethod "$($baseUrl)GetAllMembers";
$weights = 5, 3, 2, 1;

function AddWislistSelections($blah) {

    # foreach member
    foreach ($member in $members) {
        $memberInitials = $member.initials;
        # foreach weight
        foreach ($weight in $weights) {
        
            # grab random game
            $randomGame = $games | Get-Random;
            $randomGameId = $randomGame.gameId;
        
            $addWishlistSelection = "$($baseUrl)AddWishlistSelection?UserId=$($memberInitials)&GameSelection=$($randomGameId)&GameWeight=$weight";
            Write-Host $addWishlistSelection;
            Invoke-RestMethod $addWishlistSelection;
        }
    }
}
AddWislistSelections(0)