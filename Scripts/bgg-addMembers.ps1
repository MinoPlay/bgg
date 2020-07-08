$members = 'KEAN', 'LTBE', 'JACB', 'RCHE', 'DQEL', 'OLEO', 'ACGO', 'RUGR', 'KAHD', 'AJHA', 'THOH', 'ENHE', 'JOIS', 'CHJA', 'SIJH', 'MKUC', 'GUSL', 'THMO', 'CNEL', 'SEOA', 'YUPO', 'ACRU', 'MRYT', 'PLSO', 'PSZY', 'KIRH', 'TEWO', 'DNHN';

$baseUrl = 'http://localhost:7071/api';
# $baseUrl = 'https://bgg-api.azurewebsites.net/api';
#add games
foreach ($initials in $members) {
    $urlToInvoke = $baseUrl + '/AddMember?initials=' + $initials
    Write-Host $urlToInvoke
    Invoke-RestMethod $urlToInvoke
}