... This is still WIP ...

* VS Code extension: [GitHub Copilot for Azure](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azure-github-copilot) lets you use
  * [Azure Skills](https://github.com/microsoft/azure-skills) to manage Azure
    * It needs VS Code extension: [Azure MCP Server](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azure-mcp-server), too.

## [Install](https://github.com/microsoft/azure-skills/blob/main/README.md#install-in-60-seconds)

* [APM](https://microsoft.github.io/apm/) lets you install Azure Skills across multiple LLM UIs
  * [Install APM](https://microsoft.github.io/apm/#install-apm)
  ```ps1
  irm https://aka.ms/apm-windows | iex
  ```  
  * [Install Azure Skills](https://github.com/microsoft/azure-skills/blob/main/README.md#apm-one-install-multiple-harnesses)
  ```ps1
  apm install microsoft/azure-skills
  ```  


## What you get

| Component | What it adds | Examples |
| --- | --- | --- |
| **Azure skills** | Azure expertise, workflows, and guardrails | Prepare, validate, deploy, diagnostics, cost, AI, RBAC |
| **Azure MCP Server** | Live Azure tooling | Resource inventory, monitoring, pricing, storage, databases, messaging |
| **Foundry MCP** | Microsoft Foundry workflows | Model catalog, deployments, agents, evaluations |

---


---
## Next up
* [Quickstart: Deploy your first hosted agent](https://learn.microsoft.com/en-us/azure/foundry/agents/quickstarts/quickstart-hosted-agent?pivots=csharp)
* [Quickstart: Chat with an agent in code](https://learn.microsoft.com/en-us/azure/foundry/quickstarts/get-started-code?tabs=csharp)
* [Building a Hosted Agent with GitHub Copilot and Microsoft Foundry](https://code.visualstudio.com/learn/foundry-toolkit-extension/4-building-hosted-agent)
* ChatGpt. Where? [Microsoft AI Agent Quickstart](chats/microsoft-ai-agent-deployment-chat.md)
  * [Quickstart: Deploy your own code as a hosted agent](https://learn.microsoft.com/en-us/azure/foundry/agents/quickstarts/quickstart-deploy-own-code)
