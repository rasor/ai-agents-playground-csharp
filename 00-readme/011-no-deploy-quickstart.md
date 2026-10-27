## Using [Instant model](https://learn.microsoft.com/en-us/azure/foundry/concepts/instant-models?tabs=rest%2Crest-api)

The first test will be to create a project in `West US 3` to use instant access models. Instant models have no deployment, so use the model name `gpt-5-mini`.  

### Login to cloud
* First login to [Azure Portal](https://portal.azure.com/) from web, so you are sure you can get in.
or [try for free](https://azure.microsoft.com/en-us/pricing/purchase-options/azure-account?icid=azurefreeaccount&cid=msft_learn_aeb85a95-e8f4-5206-f520-9a258eea21f8)
* Then sign in to [Foundry](https://ai.azure.com/home?cid=learnDocs)
  * That created [two resources in az](https://portal.azure.com/#view/HubsExtension/BrowseAll.ReactView/resourceType/microsoft.resources%2Fresources) of type `Foundry` and `Foundry Project`. 
  * but they are not in `West US 3`, which is required for trying out [instant model](https://learn.microsoft.com/en-us/azure/foundry/quickstarts/get-started-code?tabs=csharp)
  * so instead create a new project in [Foundry](https://ai.azure.com/), where you in advanced select Location `West US 3`

#### Chat with model via web
* Second requirement is that we use model `gpt-5-mini` wherever the samples ask for a deployment name.
* In Fountry - Models
  * Instead of Deploy press Open Playground
  * Select mode `gpt-5-mini` - `save as agent`
  * --> now agent opens. Here I asked: "Is this an instant model?"
    * Warning: deployment_disabledInstant inference quota is still being initialized for this subscription. Please try again in a few minutes.
    * It looks like `save as agent` deployed the model and an agent.
    * .. meaning we should have satyed in the sandbox just to try the chat.

#### Chat with the model via REST
[PS1](../10-projs/011-instant-model-rest/):
```ps1
# Sign in using the CLI az login command to authenticate before running the next command.
az login

# Get a temporary access token. It will expire in 60-90 minutes, you'll need to refresh after that.
$AZURE_AI_AUTH_TOKEN = az account get-access-token --scope https://ai.azure.com/.default --query "accessToken" --output tsv

# Ask LLM
$JSON_REQUEST = '{"model": "gpt-5-mini","input": "What is the size of France in square miles?"}'
curl.exe -X POST https://soren-9590-resource.services.ai.azure.com/api/projects/soren-9590/openai/v1/responses -H "Content-Type: application/json" -H "Authorization: Bearer $AZURE_AI_AUTH_TOKEN" -d $JSON_REQUEST
# {
#   "output": [
#     {
#       "type": "reasoning",
#     },
#     {
#       "type": "message",
#       "id": "msg_00fcf4deb973de33006ad1f51944d481948983c1fd717e3eb7",
#       "response_id": "resp_00fcf4deb973de33006ad1f5118990819496bef00b4bd291a7",
#       "role": "assistant",
#       "content": [
#         {
#           "type": "output_text",
#           "text": "It depends whether you include France’s overseas territories.\n\n- Metropolitan (European) France: 551,695 km² ≈ 213,011 square miles.  \n- France (total, including overseas departments/territories): 643,801 km² ≈ 248,573 square miles.\n\n(Conversion factor: 1 km² ≈ 0.386102 sq mi.)",
#           "annotations": [],
#           "logprobs": []
#         }
#       ],
#       "status": "completed"
#     }
#   ],
# }
``` 

#### Chat with the model via C#

```ps1
# create console project
dotnet new console -n 011-instant-model-cs
cd 011-instant-model-cs
dotnet add package Azure.Identity
dotnet add package Azure.AI.Projects
dotnet add package Azure.AI.Extensions.OpenAI
dotnet add package OpenAI
```

Paste code into Program.cs
```cs
using Azure.Identity;
using Azure.AI.Projects;
using Azure.AI.Extensions.OpenAI;
using OpenAI.Responses;

#pragma warning disable OPENAI001

// Format: "https://resource_name.services.ai.azure.com/api/projects/project_name"
var foundryProjectEndpoint = "https://soren-9590-resource.services.ai.azure.com/api/projects/soren-9590";

// Create project client to call Foundry API
AIProjectClient projectClient = new(
    endpoint: new Uri(foundryProjectEndpoint),
    tokenProvider: new DefaultAzureCredential());

// Run a responses API call
ProjectResponsesClient responseClient = projectClient.ProjectOpenAIClient.GetProjectResponsesClientForModel(
    "gpt-5-mini"); // supports all Foundry direct models
ResponseResult response = await responseClient.CreateResponseAsync(
    "What is the size of Norway in square miles?");
string outputText = response.GetOutputText();
if (string.IsNullOrWhiteSpace(outputText))
{
    throw new InvalidOperationException("Response output text was empty.");
}

Console.WriteLine(outputText);
```

```ps1
dotnet build
az login
dotnet run
# This will throw:
# Unhandled exception. System.MissingMethodException: Method not found: 

# to fix: Downgrade from v2.14.0
dotnet add package OpenAI@2.10.0

az login
dotnet run
# Norway's total area (including Svalbard and Jan Mayen) is about 148,729 square miles (385,207 km²).  
# Mainland Norway alone is about 125,021 square miles (323,808 km²).
```

#### Chat with the model via typescript

```ps1
node -v           
# v24.21.0

cd 10-projs
# Create package.json with tsc in project parent folder
npm install --save-dev typescript
npx tsc --version
# Version 7.0.2 

# Create project folder
mkdir 011-instant-model-ts
cd 011-instant-model-ts

# Create package.json
npm init -y
# Edit package.json for ECMAScript modules
npm pkg set "type=module" "main=dist/index.js"
# Add convenient npm build and start scripts
npm pkg set "scripts.build=tsc" "scripts.start=node dist/index.js"
# Install dependencies
npm install @azure/identity
npm install @azure/ai-projects

# Create tsconfig.json
npx tsc --init --rootDir src --outDir dist --target ES2022 --module es2022 --strict
```

Add code
```ts
// src\index.ts
import { DefaultAzureCredential } from "@azure/identity";
import { AIProjectClient } from "@azure/ai-projects";

// Format: "https://resource_name.services.ai.azure.com/api/projects/project_name"
const FOUNDRY_PROJECT_ENDPOINT = "https://soren-9590-resource.services.ai.azure.com/api/projects/soren-9590";

async function main(): Promise<void> {
    // Create project and openai clients to call Foundry API
    const project = new AIProjectClient(FOUNDRY_PROJECT_ENDPOINT, new DefaultAzureCredential());
    const openai = project.getOpenAIClient();

    // Run a responses API call
    const response = await openai.responses.create({
        model: "gpt-5-mini",
        input: "What is the size of Luxembourg in square miles?",
    });
    console.log(`Response output: ${response.output_text}`);
}

main().catch((error) => {
    console.error(error);
    process.exitCode = 1;
});
```

```ps1
# Compile into `\dist`
npx tsc

# login to azure
az login
# run from \dist
node dist\index.js
# Response output: Luxembourg covers about 2,586 square kilometers, which is approximately 999 square miles (≈998.6 sq mi).
```

For convenient start using npm:
```ps1
# Compile
npm run build
# login to azure
az login
# run
npm start
```
#### Chat with the model via Phyton

Do you need to update? Check [Phython versions](https://devguide.python.org/versions/)

```ps1
uv --version
# Missing
# Installing phyton version manager
winget install --id astral-sh.uv -e
# This package requires the following dependencies:
#       Microsoft.VCRedist.2015+.x64
# Path environment variable modified; restart your shell to use the new value.
# Command line alias added: "uvx"
# Command line alias added: "uv"
# Command line alias added: "uvw"
```

Restart shell
```ps1
uv --version
# uv 0.13.5 (46776a95d 2026-10-21 x86_64-pc-windows-msvc)
uv python list
# cpython-3.9.5-windows-x86_64-none
# cpython-3.13.17-windows-x86_64-none                  <download available>
uv python install 3.13.17
# Installed Python 3.13.17 in 12.32s
```
```ps1
cd 10-projs
# Create .python-version. set python version in this project parent folder
uv python pin 3.13.17
# Create \.venv\pyvenv.cfg and more
uv venv
# Using CPython 3.13.17
# Creating virtual environment at: .venv
# Activate with: .venv\Scripts\activate
.\.venv\Scripts\Activate.ps1

python --version
# Python 3.13.17

# pip also needs installed
python -m ensurepip --upgrade
# Successfully installed pip-26.2.2
python -m pip --version
# pip 26.2.2
```
Initilize project
```ps1
# Create project folder
mkdir 011-instant-model-py
cd 011-instant-model-py

# import dependencies
python -m pip install azure.identity
python -m pip install azure.ai.projects
```

Create `main.py` 
```python
# main.py
from azure.identity import DefaultAzureCredential
from azure.ai.projects import AIProjectClient

# Format: "https://resource_name.services.ai.azure.com/api/projects/project_name"
FOUNDRY_PROJECT_ENDPOINT = "your_project_endpoint"

# Create project and openai clients to call Foundry API
project = AIProjectClient(
    endpoint=FOUNDRY_PROJECT_ENDPOINT,
    credential=DefaultAzureCredential(),
)
openai = project.get_openai_client()

# Run a responses API call
response = openai.responses.create(
    model="gpt-5-mini",  # supports all Foundry direct models
    input="What is the size of Finland in square miles?",
)
if not response.output_text or not response.output_text.strip():
    raise RuntimeError("Response output text was empty.")

print(f"Response output: {response.output_text}")
```

```ps1
# check the project for syntax errors and generate bytecode files
python -m compileall main.py

az login
# Compile and run
python main.py
```
When finished, leave the virtual environment with:
```ps1
deactivate
```

#### List possible instant models
[PS1](../10-projs/011-instant-model-rest/):
```ps1
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
```

## [LinkedIn Post](https://www.linkedin.com/feed/update/urn:li:activity:7519806014993584128/)

---
## Next up
* 012 [BRK241: From prototype to production: build and run agents at scale](012-foundry-from-prototype-to-production.md)

