# [OKF, MCP, Business Knowledge & Architecture Knowledge](https://chatgpt.com/share/6ae23079-2940-83eb-b528-43fb78d90008)

## User question
Explain google okf

## Discussion

Google's OKF refers to the Open Knowledge Format: a Git-friendly, AI-readable format for packaging structured knowledge using Markdown and YAML metadata. It is intended to make knowledge portable and consumable by humans and AI agents.

A typical OKF structure can contain concepts such as business knowledge, architecture, operational knowledge, and decisions.

Example:

```text
knowledge/
├── index.md
├── business/
│   ├── customer.md
│   ├── pricing.md
│   └── order-lifecycle.md
├── architecture/
│   ├── overview.md
│   ├── services.md
│   └── authentication.md
└── decisions/
    └── adr-001-postgres.md
```

## Tools for initializing and synchronizing OKF

Several tools were discussed:

- `superops-team/okf`: codebase-oriented OKF tooling with initialization, update/sync/watch/hook workflows and MCP support.
- `OKF-Claude-Code`: Claude Code-oriented tooling with creation, update, sync, and drift detection.
- `okfcli/okf`: a vendor-neutral OKF CLI with commands such as init, validate, lint, index, search, backlinks, and graph.
- `okfctl`: OKF authoring/lifecycle tooling with initialization, refresh, indexing, freshness/drift checks, and CI support.
- OpenKnowledge: a more complete knowledge lifecycle approach with GitHub automation and MCP.

The recommended starting point for a codebase was `superops-team/okf`, especially when the requirement is to keep OKF aligned with a changing repository.

A suggested workflow:

```text
code change
    ↓
Git commit / PR
    ↓
drift detection
    ↓
OKF update / regeneration
    ↓
validation + lint
    ↓
knowledge update
    ↓
MCP / agents consume the knowledge
```

An important design principle is to treat code as the source of truth for facts, while OKF stores higher-level knowledge such as architectural intent, business rules, rationale, constraints, and operational knowledge.

## MCP: business knowledge and architecture knowledge

OKF MCP servers generally do not need separate methods such as `get_business_knowledge` and `get_architecture_knowledge`.

Instead, business concepts and architecture concepts can coexist as typed, linked OKF concepts. MCP can expose generic operations such as:

- search
- get/read document
- graph search
- graph relationships
- graph trace
- graph context

For example:

```text
business/customer
        │
        ├── implemented-by ──► domain/CustomerService
        │                           │
        │                           ├── calls ──► auth/UserService
        │                           └── persists-to ──► data/customers
        │
        └── governed-by ──► decisions/customer-retention.md
```

This allows an agent to traverse:

**business → architecture → code/data → decisions**

The `nrkno/okf-mcp` project was discussed as an example of an MCP server with document and graph-oriented operations.

`okf-rs` was also highlighted as particularly interesting for software architecture because it can expose code/architecture-oriented relationships such as callers, callees, modules, layers, domains, paths, APIs, communities, and patterns.

A key caveat: OKF storage alone does not automatically discover architecture from source code. A producer/indexer is needed to extract facts from code, schemas, APIs, databases, etc.

## Which systems can store business + architecture knowledge?

The comparison discussed:

| System | Business knowledge | Architecture | Code-derived | Same graph |
|---|---:|---:|---:|---:|
| OKF bundle | Yes | Yes | Possible | Yes |
| OKF MCP | Yes | Yes | Via producers/indexers | Yes |
| `okf-rs` | Yes | Yes | Strong | Yes |
| OpenKnowledge | Yes | Yes | Some | Yes |
| Backstage | Yes / modeled domains | Strong | Strong catalog integration | Partially |
| Neo4j | Yes | Yes | Yes, if ingested | Yes |
| Vector/RAG | Yes | Yes | Yes | No, primarily retrieval |

## OKF vs Backstage vs Neo4j vs Vector/RAG

### OKF
Best for portable, curated, Git-native knowledge.

Good for:
- business concepts
- business rules
- architecture descriptions
- decisions
- operational guidance
- provenance and documentation

Weakness: it does not automatically infer all knowledge from code.

### Backstage
Best for a service/software catalog.

Good for:
- services
- APIs
- systems
- resources
- ownership
- organizational relationships
- technical documentation

Weakness: nuanced business rules and business intent usually need additional modeling/documentation.

### Neo4j
Best for explicit relationship traversal and impact analysis.

Good for questions such as:
- What depends on this service?
- Which business capabilities are implemented by these systems?
- What would be affected if this API changes?

Weakness: requires building and maintaining ingestion, data modeling, provenance, and synchronization.

### Vector/RAG
Best for semantic retrieval.

Good for:
- finding relevant documentation
- searching code and text by meaning
- natural-language questions

Weakness: semantic similarity alone is not reliable for exact dependencies, ownership, or impact analysis.

## Recommended architecture

The recommended approach was a hybrid architecture:

```text
Business knowledge
        │
        ▼
      OKF
        │
        ├───────────────┐
        ▼               ▼
Architecture/code   Documentation
facts/indexes           │
        │               │
        └───────┬───────┘
                ▼
       Graph + semantic indexes
                │
                ▼
               MCP
                │
                ▼
          Coding agents
```

Use:

- **OKF** for curated business knowledge, architecture intent, decisions, and operational knowledge.
- **Backstage** if an organization-wide service catalog and ownership model are needed.
- **Neo4j** when multi-hop relationship and impact analysis becomes important.
- **Vector search/RAG** for semantic retrieval across code and documentation.
- **MCP** as the agent-facing interface.

Potential MCP operations include:

| MCP tool | Example question |
|---|---|
| `search_business_knowledge` | What does customer eligibility mean? |
| `get_business_rule` | What conditions must an order satisfy? |
| `get_architecture` | How is order processing structured? |
| `find_implementation` | Which code implements the eligibility rule? |
| `trace_dependencies` | What services depend on the pricing API? |
| `trace_business_impact` | Which business capabilities could be affected? |
| `get_knowledge_sources` | Which files/code references support this answer? |

## Synchronization principles

Different knowledge types should have different synchronization rules:

1. Code-derived facts should be regenerated from source code, API specifications, schemas, and dependency analysis.
2. Business knowledge should remain authoritative and versioned, with review for policy/rule changes.
3. Architecture documentation can be checked for drift against actual code and updated through reviewed proposals.
4. Graph and vector indexes should be treated as derived artifacts and rebuilt or incrementally updated.
5. Provenance should include source paths, commit/version information, and freshness so agents can distinguish verified facts from human-authored intent.

## Overall recommendation

For a single codebase, start with:

**OKF + a code-aware indexer + MCP**

Then add:

- **Backstage** for organization-wide service ownership/catalog needs.
- **Neo4j** for complex relationship and impact queries.
- **Vector retrieval** for semantic search.

The central insight is that the valuable system is not merely an OKF repository. It is a **shared knowledge layer connecting business concepts, architecture, implementation, and decisions**, with explicit relationships and provenance.

The most useful agent question becomes:

> "If we change this business rule, which architecture components and code are affected, and why?"

That requires connecting business knowledge to architecture and code, rather than keeping those as separate documentation silos.
