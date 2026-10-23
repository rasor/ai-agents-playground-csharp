# Sign in using the CLI az login command to authenticate before running the next command.
az login

# Get a temporary access token. It will expire in 60-90 minutes, you'll need to refresh after that.
$AZURE_AI_AUTH_TOKEN = az account get-access-token --scope https://ai.azure.com/.default --query "accessToken" --output tsv
Write-Host "Got Foundry Access token"

# Ask LLM
$JSON_REQUEST = '{"model": "gpt-5-mini","input": "What is the size of France in square miles?"}'
curl.exe -X POST https://soren-9590-resource.services.ai.azure.com/api/projects/soren-9590/openai/v1/responses -H "Content-Type: application/json" -H "Authorization: Bearer $AZURE_AI_AUTH_TOKEN" -d $JSON_REQUEST
