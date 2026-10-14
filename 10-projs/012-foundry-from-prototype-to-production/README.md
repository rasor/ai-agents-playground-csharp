# Fiber Field Operations Agent

A C#/.NET 10 Microsoft Agent Framework hosted agent for evidence-led fiber fault diagnosis. It consumes Foundry toolbox tools for Work IQ, Fabric IQ, and Foundry IQ, and exposes Azure Content Understanding as an agent-callable analysis tool for HTTPS-accessible field photos and reports.

## What it can do

- Correlate work orders and operational communications through Work IQ.
- Look up service topology and telemetry through Fabric IQ.
- Retrieve approved runbooks and knowledge through the configured Foundry IQ toolbox tool.
- Extract observations from an accessible field image or report with Azure Content Understanding.
- Summarize findings, competing hypotheses, confidence, safe next checks, and escalation criteria.

It does not change network configuration, execute repair commands, or close tickets.

## Prerequisites

- .NET 10 SDK.
- A Microsoft Foundry project and deployed chat model.
- A Foundry toolbox named in `TOOLBOX_NAME`, with project connections and tools for Work IQ, Fabric IQ, and Foundry IQ.
- An Azure Content Understanding resource and analyzer configured for the evidence types used in the field.
- Local Azure authentication through `DefaultAzureCredential` (for example, Azure CLI sign-in) or a Content Understanding API key.

The selected Foundry project available during initial setup had no deployed models. Deploy a suitable model and set its deployment name before running this service.

## Configure the toolbox

Create and configure the toolbox and project connections in Foundry Toolkit or the Foundry portal; this project intentionally does not create connections or request tenant consent. The agent uses Foundry's server-side toolbox integration and discovers the tools from the configured toolbox at startup.

Add:

- **Work IQ** using the `work_iq_preview` toolbox tool. It requires the documented Entra application, tenant admin consent, and eligible user licensing. See [Work IQ tool documentation](https://learn.microsoft.com/azure/foundry/agents/how-to/tools/work-iq).
- **Fabric IQ** using the `fabric_iq_preview` toolbox tool and a published ontology, Fabric data agent, or Power BI semantic model. See [Fabric IQ tool documentation](https://learn.microsoft.com/azure/foundry/agents/how-to/tools/fabric-iq).
- **Foundry IQ** using a generic MCP toolbox tool connected to the knowledge base MCP endpoint:
  `https://<search-service>.search.windows.net/knowledgebases/<knowledge-base>/mcp?api-version=2026-08-01-preview`
  Allow the `knowledge_base_retrieve` tool. Configure the project connection with project-managed identity and grant it the required Azure AI Search data-plane permissions. See [Connect agents to Foundry IQ knowledge bases](https://learn.microsoft.com/azure/foundry/agents/how-to/foundry-iq-connect).

Make these tools available from the same toolbox configured in `TOOLBOX_NAME`. The tool descriptions are discovered at runtime; no remote tool names or credentials are embedded in the agent code.

Grant only the representative's required access in each source. Work IQ and Fabric IQ honor source permissions; verify connection consent and the access model in your tenant before field use. Set the toolbox name in `.env`.

## Configure and run locally

1. Copy `.env.example` to `.env` and fill in the Foundry project endpoint, deployed model name, toolbox name, Content Understanding endpoint, and analyzer ID.
2. Authenticate using `DefaultAzureCredential` (or set `CONTENT_UNDERSTANDING_API_KEY` for Content Understanding).
3. Restore and build:

   ```powershell
   dotnet restore
   dotnet build
   ```

4. Start it using the Foundry Agent Framework hosting workflow appropriate for your project. The hosted Responses endpoint requires the Foundry project, model deployment, and toolbox to be configured; this source-only scaffold does not provision them.

The evidence tool accepts an absolute HTTPS URL with no embedded credentials. The Content Understanding service must be able to retrieve the resource. Use a short-lived, appropriately scoped SAS URL if needed; do not put secret URLs or keys in source control.

## Diagnostic safety

The agent distinguishes source-backed observations from hypotheses and recommends safe checks only. It must not instruct anyone to view active fiber, and it directs representatives to approved inspection equipment and site safety procedures. Validate diagnostic guidance against your approved operational procedures before deployment.
