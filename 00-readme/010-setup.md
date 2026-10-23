## Setup Foundry - [Prepare your development environment](https://learn.microsoft.com/en-us/azure/foundry/how-to/develop/install-cli-sdk?tabs=windows%2Ccsharp%2Cazd-ai)

### What we need
1. git - I use [`Git for windows`](https://git-scm.com/install/) - already installed.  
2. Programming language - I'll go with `C#` and [`.NET 10.0`](https://dotnet.microsoft.com/en-us/download).  
3. Editor - I'll go with `VS Code` - already installed with some extentions.
  * Extension [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)  
4. `Foundry DevPack` can also be used instead of VS Code, but since it installs other needed tools, then install that, too. 
5. Optional: `Foundry Skill` - so you can ask LLM to make your AI app

### 1. git

```bash
# verify git installed
git --version
# git version 2.24.1.windows.2
```

### 2. C# and .net

```bash
dotnet --version        # SDK selected in the current directory
winget install --id Microsoft.DotNet.SDK.10 --exact
# Found Microsoft .NET SDK 10.0 [Microsoft.DotNet.SDK.10] Version 10.0.401
```

### 4. Foundry DevPack
```bash
winget install Microsoft.FoundryDevPack
# Found Microsoft Foundry DevPack [Microsoft.FoundryDevPack] Version 0.1.5
# This package requires the following dependencies:
#   - Packages
#       Microsoft.AzureCLI [>= 2.77.0]
#       Microsoft.Azd [>= 1.34.100]
# (1/2) Found Azure Developer CLI [Microsoft.Azd] Version 1.35.200
# (2/2) Found Microsoft Azure CLI [Microsoft.AzureCLI] Version 2.91.0

# Notes: Winget installs the 64-bit CLI on 64-bit OS by default now. If you have used the 32-bit CLI before, please follow this guide to migrate to 64-bit version: https://learn.microsoft.com/cli/azure/install-azure-cli-windows#migrate-to-64-bit-azure-cli

# Notes: To verify or repair your setup, run:
#     winget repair --exact --id Microsoft.FoundryDevPack

# If Azure CLI or Azure Developer CLI was removed, restore package dependencies with:
#     winget install --exact --id Microsoft.FoundryDevPack --force

# WHAT'S INCLUDED

# - Foundry command-line tools
#   Azure CLI, Azure Developer CLI (azd), and the Foundry extension for azd (azd ai).
# - Microsoft Foundry Skill
#   ~/.agents/skills/
#   ~/.claude/skills/ (if Claude Code is available)
# - Microsoft Foundry Toolkit for Visual Studio Code (if applicable)
# - Foundry Canvas (preview) for GitHub Copilot App (if applicable)
#   ~/.copilot/installed-plugins/

# Learn more at https://aka.ms/foundrydevpack

# GET STARTED

# - Run `azd ai agent init` to create your first hosted agent.
# - Or ask your coding agent: "Create my Foundry hosted agent end to end."

# For in-depth instructions on how to build and deploy your agent, go to: https://aka.ms/foundry-agent-quickstart

az -v
# azure-cli                         2.91.0

azd version   
# azd version 1.37.1

azd --docs
# docs in browser

azd -h       
# The Azure Developer CLI is an open-source tool that helps onboard and manage your project on Azure
# Usage
#   azd [command]
# Commands
#   Getting started
#     init        : Initialize a new application.
#     up          : Provision and deploy your project to Azure with a single command.
#   Create and manage Azure resources
#     auth        : Authenticate with Azure.
#     deploy      : Deploy your project code to Azure.
#     down        : Delete your project's Azure resources.
#     provision   : Provision Azure resources for your project.
#     publish     : Publish a service image or reuse an existing passthrough image.
#   Manage and show settings
#     completion  : Generate shell completion scripts.
#     config      : Manage azd configurations (ex: default Azure subscription, location).
#     env         : Manage environments (ex: default environment, environment variables).
#     exec        : Execute commands and scripts with azd environment context.
#     show        : Display information about your project and its resources.
#     tool        : Manage Azure development tools.
#     version     : Print the version number of Azure Developer CLI.
#   Beta commands
#     add         : Add a component to your project.
#     build       : Builds the application's code.
#     extension   : Manage azd extensions.
#     hooks       : Develop, test and run hooks for a project.
#     infra       : Manage your Infrastructure as Code (IaC).
#     monitor     : Monitor a deployed project.
#     package     : Packages the project's code to be deployed to Azure.
#     pipeline    : Manage and configure your deployment pipelines.
#     restore     : Restores the project's dependencies.
#     template    : Find and view template details.
#     update      : Updates azd to the latest version.
#   Enabled alpha commands
#     copilot     : Manage GitHub Copilot agent settings. (Preview)
#     mcp         : Manage Model Context Protocol (MCP) server. (Alpha)
#   Enabled extensions commands 
#     ai          : Commands for the ai extension namespace.
# Flags
#     -C, --cwd string            : Sets the current working directory.
#         --debug                 : Enables debugging and diagnostics logging.
#     -e, --environment string    : The name of the environment to use.
#         --no-prompt             : Runs without prompts. Uses existing values;fails if any required value or decision cannot be resolved automatically. Automatically enabled when azd detects a CI/CD or AI-agent environment; set AZD_NON_INTERACTIVE=false to opt out of that automatic enablement.
# Global Flags
#         --docs  : Opens the documentation for azd in your web browser.
#     -h, --help  : Gets help for azd.

# Use azd [command] --help to view examples and more information about a specific command.
# Deploying a sample application
# --> Initialize from a sample application by running the azd init --template [template name] command in an empty directory.
# Then, run azd up to get the application up-and-running in Azure.
# To view a curated list of sample templates, run azd template list.
# To view all available sample templates, including those submitted by the azd community, visit: https://azure.github.io/awesome-azd.
```
#### [Foundry Toolkit for VS Code](https://marketplace.visualstudio.com/items?itemName=ms-windows-ai-studio.windows-ai-studio) 
... was installed by FoundryDevPack.  
It is a developer UI for deploying AI applications

Features:  
* 🔍 Discover and evaluate models from providers including Microsoft Foundry, Foundry Local, Anthropic, OpenAI, GitHub, Google, and NVIDIA NIM, or run models locally with ONNX and Ollama.
* ⚡ Build, test, and deploy AI agents with Agent Builder for no-code prompt agents, or create code-based hosted agents with debugging, streaming visualization, and MCP tool integrations.
* ☁️ Manage Microsoft Foundry resources directly from VS Code: browse projects, deploy models from the catalog, and create, deploy, and test agents with the Microsoft Foundry Agent Service.

It has its own left-menu in VS Code.  

It is the UI for the `azd` cli

#### azd cli
... was installed by FoundryDevPack.  

* Learn: [AZD-for-beginners](https://github.com/microsoft/AZD-for-beginners)

#### [Azure Resources](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azureresourcegroups) VS Code Extension
... is a UI DevOps/Infra for your whole Azure collection of resources.  

This, too has its own left-menu in VS Code. I have placed it above Foundry.  

It is the UI for the `az` cli

#### GitHub Codespaces
Foundry Toolkit for VS Code also had a link to [GitHub Codespaces](https://marketplace.visualstudio.com/items?itemName=GitHub.codespaces) VS Code Extension.  
[GitHub Codespaces](https://docs.github.com/en/codespaces) is a remote store (in github repo) for your dev environment.  
It can be accessed from VS Code left-menu `Remote Explorer`, where remote locations - like SSH or WSL also resides.  
I don't think I will be using it.  

### 5. [Foundry Skill](https://learn.microsoft.com/en-us/azure/foundry/how-to/develop/use-microsoft-foundry-skill?tabs=vscode)
So you can ask LLM to make your AI app.

```bash
# you nedd node 18 or later for npx to start MCP servers in compatible hosts
node -v                       
# v24.21.0

az login
# Subscription: xxx

azd auth login
# Logged in to Azure as xxx

npx skills add https://github.com/microsoft/azure-skills --skill microsoft-foundry
# Need to install the following packages:
# skills@1.7.2
# Source: https://github.com/microsoft/azure-skills.git
# ◇  Repository cloned
# ◇  Found 43 skills
# ●  Selected 1 skill: microsoft-foundry
# ◇  79 agents
# ◆  Which agents do you want to install to?
# │  ── Universal (.agents/skills) ── always included ────────────
# ◇  Which agents do you want to install to?
# │  Amp, Cline, Codex, Cursor, Droid, Gemini CLI, GitHub Copilot, Kilo Code, Kimi Code CLI, OpenCode, Pi, Warp, Zed
# ◇  Installation scope
# │  Project
# ◇  Installation Summary ──────────────────────────────────────────────────────────────╮
# │  ~\projs-git\projs-ai\ai-agents-playground-csharp\.agents\skills\microsoft-foundry  │
# │    copy → Amp, Cline, Codex, Cursor, Droid +8 more                                  │
# ├─────────────────────────────────────────────────────────────────────────────────────╯
# ◇  Install the find-skills skill? It helps your agent discover and suggest skills.
# ◇  Source: https://github.com/vercel-labs/skills.git
# ◇  Installation Summary ──────────────────────────────╮
# │  ~\.agents\skills\find-skills                       │
# │    copy → Amp, Cline, Codex, Cursor, Droid +8 more  │
# ├─────────────────────────────────────────────────────╯
# └  Done!  Review skills before use; they run with full agent permissions.
```
---
