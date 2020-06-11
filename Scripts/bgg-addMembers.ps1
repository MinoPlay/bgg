$randomNames = 'MKUC', 'ENHE', 'KEAN', 'BOWA', 'MFIG';


#add games
foreach ($initials in $randomNames) {
    $urlToInvoke = 'http://localhost:7071/api/AddMember?initials=' + $initials
    Write-Host $urlToInvoke
    Invoke-RestMethod $urlToInvoke
}