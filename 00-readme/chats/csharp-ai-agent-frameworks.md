[C# AI Agent Frameworks — Beginner to Expert](https://claude.ai/chat/473fae2a-ef8d-4619-ab47-f6cfca741e54)
# Prompt
* Can you name the most beginner friendly csharp AI agent frameworks?
* and the medium level frameworks?
* and the expert level framework?

# C# AI Agent Frameworks — Beginner to Expert

## Beginner-Friendly C# AI Agent Frameworks

- **Microsoft Semantic Kernel** — official Microsoft SDK, lots of docs/tutorials, simple `Kernel` + plugin model, great first step if you're already in the .NET ecosystem
- **AutoGen for .NET (AutoGen.NET)** — straightforward multi-agent conversation patterns, good examples repo
- **LangChain for .NET (via community ports like LangChain.NET)** — familiar if you've touched Python LangChain, simpler chain-based abstractions

## Medium-Level Frameworks

- **Semantic Kernel + Planners/Process Framework** — once you move past basic plugins into SK's Process Framework (stateful workflows, event-driven steps), it steps up in complexity
- **Microsoft.Extensions.AI** — the newer abstraction layer for chat clients, embeddings, function calling; requires understanding DI patterns and middleware pipelines
- **AutoGen.NET with custom agent roles** — building multi-agent conversations with custom termination conditions, group chats, and tool orchestration

## Expert-Level Framework

- **Semantic Kernel Agent Framework (Agents SDK)** — the full agent orchestration layer: custom agent types, complex multi-agent collaboration, low-level control over planning/memory/tool-calling loops, and integration with Azure AI Agent Service for production-grade deployments

---

## Update: Microsoft Agent Framework (MAF) — the consolidation

Semantic Kernel and AutoGen have officially merged. Microsoft Agent Framework is the direct successor to both, merging AutoGen's agent abstractions with Semantic Kernel's enterprise features into a single framework. It shipped version 1.0 on April 3, 2026, with stable APIs, native MCP and A2A support, and a long-term support commitment. Package namespace: `Microsoft.Agents.AI`.

### Revised Beginner tier
- **Microsoft Agent Framework — basic `AIAgent`** — combines AutoGen's simple abstractions with Semantic Kernel's enterprise-grade features; a single-agent starter script is simple
- **Microsoft.Extensions.AI** — good entry point for a chat client abstraction before touching "agents"

### Revised Medium tier
- **Microsoft Agent Framework — Workflows** — graph-based orchestration (sequential, concurrent, handoff, group chat patterns), plus tool/MCP integration
- **Semantic Kernel (legacy, still maintained)** — relevant if on an existing SK codebase not yet migrated

### Revised Expert tier
- **Microsoft Agent Framework — full multi-agent + MCP/A2A** — native Model Context Protocol support for tools and Agent-to-Agent protocol for cross-framework agent collaboration; six model providers with one-line swap; used for production multi-agent systems with observability, evaluation, and hosting

### Practical takeaway
For any new project, start directly with Microsoft Agent Framework rather than Semantic Kernel or AutoGen individually — it's now the recommended approach for .NET apps needing agentic AI systems with advanced orchestration, multi-agent collaboration, and enterprise-grade security and observability. SK and AutoGen aren't dead, but they now feed into MAF rather than being the primary pick.
