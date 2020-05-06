#Requires -Version 5
$servurl = 'https://<server address'
$authurl = $servurl + '/auth'
$apiurl = $servurl + '/pe/api/StaffMember/Me'
$appid = '<appid>'
$appkey = '<appkey>'

$response = Invoke-RestMethod -Uri ($authurl +'/.well-known/openid-configuration') -Method Get 
$tokenurl = $response.token_endpoint

$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $appid,$appkey)))

$payload = @{
    grant_type = 'client_credentials'
    scope = 'pe.api'
}

$token = Invoke-RestMethod -Uri $tokenurl -Method Post -Headers @{Authorization=("Basic {0}" -f $base64AuthInfo)} -Body $payload

$peapiheader = @{ 
    'Authorization' = 'Bearer ' + $token.access_token 
}

$me = Invoke-RestMethod -Uri $apiurl -Method Get -Headers $peapiheader
Write-Output $me