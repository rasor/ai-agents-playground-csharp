$SUBSCRIPTION_ID = "<your-subscription-id>"
$LOCATION = "westus3"
$URL = "https://management.azure.com/subscriptions/$SUBSCRIPTION_ID/providers/Microsoft.CognitiveServices/locations/$LOCATION/models?api-version=2025-06-01"

$responseJson = az rest --method get --url $URL --output json
if ($LASTEXITCODE -ne 0) {
    throw "az rest failed with exit code $LASTEXITCODE."
}

$response = $responseJson | ConvertFrom-Json
$models = if ($null -ne $response.value) {
    $response.value
} elseif ($null -ne $response.models) {
    $response.models
} else {
    $response
}

$models |
    Where-Object { ([string]$_.model.capabilities.instant) -ieq "true" } |
    ForEach-Object { $_.model.name } |
    Where-Object { $_ } |
    Sort-Object -Unique
