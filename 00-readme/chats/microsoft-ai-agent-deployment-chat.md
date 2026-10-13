* ChatGpt. Where? [Microsoft AI Agent Quickstart](https://chatgpt.com/c/6ab4d8a6-5acc-83eb-b659-e87dbc39e02d)
  * [Quickstart: Deploy your own code as a hosted agent](https://learn.microsoft.com/en-us/azure/foundry/agents/quickstarts/quickstart-deploy-own-code)

---
# Chat --- Microsoft AI-agent deployment quick-start

## User

Does microsoft provide a quick-start for deploying AI-agents?

## Assistant

Yes. Microsoft now has a fairly direct **quick-start path for deploying
AI agents to Azure**, centered on **Microsoft Foundry Agent Service**.

The most relevant official guide is:

[Microsoft: Quickstart --- Deploy your first hosted
agent](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/quickstarts/quickstart-hosted-agent)

### The basic deployment flow

Microsoft's current quick-start is roughly:

``` text
Your agent code
      ↓
Azure Developer CLI (azd)
      ↓
Microsoft Foundry
      ↓
Foundry Agent Service
      ↓
Hosted AI agent + endpoint
```

You can get from a basic sample to a deployed agent with commands along
the lines of:

``` bash
azd ai agent init
azd provision
azd ai agent run
azd deploy
azd ai agent invoke
```

The quick-start can create/provision the Foundry resources, run the
agent locally, deploy it to Microsoft's hosted Agent Service, and give
you an endpoint and playground.

### Deploying your existing agent

Microsoft also has a quick-start for deploying your own agent code:

[Microsoft: Deploy your own code as a hosted
agent](https://learn.microsoft.com/en-us/azure/foundry/agents/quickstarts/quickstart-deploy-own-code)

This supports agent code written with, among others:

-   Microsoft Agent Framework
-   LangGraph
-   GitHub Copilot SDK
-   OpenAI Agents SDK
-   Plain code that calls a model directly

The general flow is:

``` bash
azd ai agent init --protocol responses --deploy-mode code
azd provision
azd ai agent run
azd deploy
azd ai agent invoke
```

### VS Code route

Microsoft also provides the Microsoft Foundry Toolkit for VS Code. It
can scaffold a hosted-agent project, let you choose Python or C#, select
a model/framework, run the agent locally, and deploy it to Foundry.

### For .NET / C

For a .NET developer, the C# hosted-agent path is particularly relevant.
Microsoft's current documentation uses a .NET web project and the Azure
Agent Server Responses or Invocations package.

For a source-code deployment, the current quick-start prerequisites
include:

-   Azure subscription
-   Azure Developer CLI (`azd`) 1.27.1 or later
-   `azd microsoft.foundry` extension
-   Authenticated `azd` session
-   .NET 10 SDK or later for C#
-   Existing agent code, or a Microsoft sample

A minimal new hosted-agent project starts with:

``` bash
dotnet new web --name my-agent
cd my-agent
dotnet add package Azure.AI.AgentServer.Responses --prerelease
```

Then initialize the Azure deployment:

``` bash
azd ai agent init --protocol responses --deploy-mode code
```

Provision resources:

``` bash
azd provision
```

Test locally:

``` bash
azd ai agent run
```

Deploy:

``` bash
azd deploy
```

Invoke:

``` bash
azd ai agent invoke "Hello from my hosted agent"
```

Microsoft recommends the **Responses** protocol if you're unsure which
protocol to choose.

### Important distinction

Microsoft's current direction is **Microsoft Foundry Agent Service**.
For new projects, use the current Foundry hosted-agent path rather than
the older/classic Agents service.

------------------------------------------------------------------------

## User

-   yes, please
-   and after that, then save this chat as .md

## Assistant

### Recommended path: C#/.NET + Microsoft Foundry + Azure Developer CLI

For a new .NET agent, the shortest practical path is Microsoft's
**hosted-agent source-code deployment**. It avoids Docker/container work
initially: you add Microsoft's hosting library to your .NET agent,
initialize the `azd` project, provision Azure resources, test locally,
and deploy.

Microsoft's current quick-start explicitly supports deploying your own
Python or .NET/C# agent code to Foundry Agent Service. It can work with
Microsoft Agent Framework, LangGraph, GitHub Copilot SDK, OpenAI Agents
SDK, or plain model-calling code.

### 1. Install / verify prerequisites

You need:

-   An Azure subscription
-   .NET 10 SDK or later
-   Azure Developer CLI (`azd`) 1.27.1+
-   The Microsoft Foundry `azd` extension
-   An Azure login

Install the extension:

``` bash
azd ext install microsoft.foundry
```

Sign in:

``` bash
azd auth login
```

You can also use the Microsoft Foundry Toolkit for VS Code if you prefer
a GUI/scaffolding workflow.

### 2. Create the agent

For a minimal C# hosted agent:

``` bash
mkdir my-agent
cd my-agent

dotnet new web
dotnet add package Azure.AI.AgentServer.Responses --prerelease
```

The web project gives you the HTTP host expected by the hosted-agent
service. The Agent Server Responses package handles the hosted-agent
protocol, HTTP server, health checks, and request/response schema.

Your actual agent logic can then call the model and/or use your
preferred agent framework.

### 3. Initialize Azure deployment

From the agent source directory:

``` bash
azd ai agent init --protocol responses --deploy-mode code
```

The interactive setup asks for things such as:

-   Agent name
-   Existing or new Foundry project
-   Azure subscription
-   Azure region

Microsoft recommends **Responses** as the starting protocol unless you
specifically need the more customizable Invocations protocol.

### 4. Provision Azure resources

``` bash
azd provision
```

This provisions the Azure resources described by the generated
`azure.yaml`.

### 5. Test locally

``` bash
azd ai agent run
```

Microsoft's tooling launches the agent and opens an Agent Inspector so
you can test it interactively.

You can also invoke it from another terminal:

``` bash
azd ai agent invoke --local "Hello from my hosted agent"
```

### 6. Deploy

Once the local test works:

``` bash
azd deploy
```

The source-code deployment path packages the project, uploads it to
Foundry, resolves/builds dependencies remotely, and deploys a
hosted-agent version.

The deployment output includes:

-   Foundry Agent Playground URL
-   Agent endpoint

### 7. Invoke the deployed agent

``` bash
azd ai agent invoke "Hello from my deployed agent"
```

At this point you have a remotely hosted agent running in Microsoft
Foundry Agent Service.

### The whole workflow

``` text
┌───────────────────────┐
│ C# / .NET agent code  │
└───────────┬───────────┘
            │
            │ azd ai agent init
            ▼
┌───────────────────────┐
│ Azure Developer CLI   │
│ + Foundry extension   │
└───────────┬───────────┘
            │
            │ azd provision
            ▼
┌───────────────────────┐
│ Microsoft Foundry     │
│ project + resources   │
└───────────┬───────────┘
            │
            │ azd deploy
            ▼
┌───────────────────────┐
│ Foundry Agent Service │
│ hosted agent          │
└───────────┬───────────┘
            │
            │ endpoint / playground
            ▼
       Your application
```

### What I would use for a first production-oriented prototype

I'd start with:

``` text
C# / .NET 10
    +
Microsoft Agent Framework (if you need multi-step/tool-using agent behavior)
    +
Responses protocol
    +
Microsoft Foundry Agent Service
    +
Azure Developer CLI (azd)
    +
Application Insights / tracing
```

Then add capabilities incrementally:

1.  **Tools / MCP**
2.  **Knowledge / RAG**
3.  **Persistent memory**
4.  **Evaluation**
5.  **Tracing**
6.  **CI/CD**

Microsoft's current Foundry quick-start collection has dedicated guides
for these next steps, including toolboxes/MCP, persistent memory,
knowledge bases, evaluation, tracing, and CI/CD.

### Official Microsoft references

-   Quickstart: Deploy your first hosted agent\
    https://learn.microsoft.com/en-us/azure/ai-foundry/agents/quickstarts/quickstart-hosted-agent

-   Quickstart: Deploy your own code as a hosted agent\
    https://learn.microsoft.com/en-us/azure/foundry/agents/quickstarts/quickstart-deploy-own-code

-   Deploy a hosted agent from source code\
    https://learn.microsoft.com/en-us/azure/foundry/agents/how-to/deploy-hosted-agent-code

-   Microsoft Foundry quickstarts\
    https://learn.microsoft.com/en-us/azure/foundry/quickstarts/quickstarts

### Current Microsoft documentation notes

The current Microsoft documentation (checked October 2026) says:

-   `azd` 1.27.1 or later is required for the current hosted-agent
    quick-start.
-   C# hosted-agent development uses .NET 10 or later.
-   Source-code deployment can be done without building/pushing a Docker
    image.
-   `azd` and the Foundry Toolkit for VS Code are Microsoft's
    recommended fast path for most source-code deployments.
-   The current Foundry quick-start supports C# as well as Python.
-   Microsoft also provides container-based deployment when you need
    full control over the runtime image.
