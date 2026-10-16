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

#### Chat with the model via `azd`



#### Chat with the model via REST
PS1:
```ps1
# Sign in using the CLI az login command to authenticate before running the next command.
az login

# Get a temporary access token. It will expire in 60-90 minutes, you'll need to refresh after that.
$AZURE_AI_AUTH_TOKEN = az account get-access-token --scope https://ai.azure.com/.default --query "accessToken" --output tsv

# Ask LLM
$JSON_REQUEST = '{"model": "gpt-5-mini","input": "What is the size of France in square miles?"}'
curl.exe -X POST soren-9590-resource.services.ai.azure.com/api/projects/soren-9590/openai/v1/responses -H "Content-Type: application/json" -H "Authorization: Bearer $AZURE_AI_AUTH_TOKEN" -d $JSON_REQUEST
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



## Next up
* 012 [BRK241: From prototype to production: build and run agents at scale](012-foundry-from-prototype-to-production.md)

---
