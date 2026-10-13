## Using [Instant model](https://learn.microsoft.com/en-us/azure/foundry/concepts/instant-models?tabs=rest%2Crest-api)

The first test will be to create a project in `West US 3` to use instant access models. Instant models have no deployment, so use the model name `gpt-5-mini`.  

### Login to cloud
* First login to [Azure Portal](https://portal.azure.com/) from web, so you are sure you can get in.
or [try for free](https://azure.microsoft.com/en-us/pricing/purchase-options/azure-account?icid=azurefreeaccount&cid=msft_learn_aeb85a95-e8f4-5206-f520-9a258eea21f8)
* Then sign in to [Foundry](https://ai.azure.com/home?cid=learnDocs)
  * That created [two resources in az](https://portal.azure.com/#view/HubsExtension/BrowseAll.ReactView/resourceType/microsoft.resources%2Fresources) of type `Foundry` and `Foundry Project`. 
  * but they are not in `West US 3`, which is required for trying out [instant model](https://learn.microsoft.com/en-us/azure/foundry/quickstarts/get-started-code?tabs=csharp)
  * so instead creaate a new project in [Foundry](https://ai.azure.com/), where you in advanced select Location `West US 3`

#### Chat with model
* Second requirement is that we use model `gpt-5-mini` wherever the samples ask for a deployment name.
* In Fountry - Models
  * Instead of Deploy press Open Playground
  * Select mode `gpt-5-mini` - `save as agent`
  * --> now agent opens. Here I asked: "Is this an instant model?"
    * Warning: deployment_disabledInstant inference quota is still being initialized for this subscription. Please try again in a few minutes.
    * It looks like `save as agent` deployed the model and an agent.
    * .. meaning we should have satyed in the sandbox just to try the chat.
  

## Next up
* [Quickstart: Get started with Microsoft Foundry SDK](https://learn.microsoft.com/en-us/azure/foundry/quickstarts/get-started-code?tabs=csharp)

* ChatGpt. Where? [Microsoft AI Agent Quickstart](chats/microsoft-ai-agent-deployment-chat.md)
  * [Quickstart: Deploy your own code as a hosted agent](https://learn.microsoft.com/en-us/azure/foundry/agents/quickstarts/quickstart-deploy-own-code)

---
