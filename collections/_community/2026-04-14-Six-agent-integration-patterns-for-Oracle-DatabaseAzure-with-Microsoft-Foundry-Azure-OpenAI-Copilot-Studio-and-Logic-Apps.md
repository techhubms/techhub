---
section_names:
- ai
- azure
- security
date: 2026-04-14 17:12:16 +00:00
author: ManishChopra
feed_name: Microsoft Tech Community
tags:
- Agentic AI
- AI
- Azure
- Azure AI Foundry
- Azure API Management
- Azure Functions
- Azure Logic Apps
- Azure OpenAI Service
- Community
- Copilot Studio
- JDBC
- Latency
- Managed Identity
- Microsoft Entra ID
- Microsoft Foundry
- Microsoft Purview
- Microsoft Teams
- Multicloud Architecture
- NL2SQL
- OAuth 2.0
- On Premises Data Gateway
- Oracle AI Database@Azure
- Oracle Database@Azure
- ORDS
- PL/SQL
- Python Oracledb
- Row Level Security
- Security
- Virtual Private Database
title: Six agent integration patterns for Oracle Database@Azure with Microsoft Foundry, Azure OpenAI, Copilot Studio, and Logic Apps
primary_section: ai
external_url: https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/your-oracle-data-is-sitting-next-to-microsoft-ai-are-you-using/ba-p/4509438
---

ManishChopra outlines six practical integration patterns for building agents and copilots that query Oracle Database@Azure with sub-millisecond proximity to Microsoft’s AI stack, covering options from Copilot Studio connectors to ORDS/PL/SQL, Azure Functions, and Logic Apps, plus the identity/governance controls typically needed for production.<!--excerpt_end-->

## Overview

This post argues that many Oracle + AI initiatives fail in production not because models are bad, but because **latency and cross-cloud boundaries** make “real-time copilots” and “agentic workflows” unreliable.

The core claim is that **Oracle AI Database@Azure** (Oracle databases hosted in Azure datacenters) puts Oracle data and Microsoft AI services in the **same Azure region / datacenter fabric**, reducing network hops and avoiding cross-cloud egress—making agent-to-data interactions fast enough to be practical.

## The problem the article is trying to solve

Typical symptoms described:

- AI proof-of-concepts look great in demos but fail in production due to cross-cloud boundaries.
- “Copilots return stale data,” “agents time out,” and dashboards refresh too slowly.

Root cause asserted:

- Roughly **200–300 ms latency** between Oracle data (on-prem / OCI / other cloud) and Microsoft AI services.

What changes with Oracle Database@Azure:

- Oracle Exadata and Microsoft AI services are described as being co-located (same datacenter fabric / network switch), reducing latency and removing egress costs.

## Pattern #01 (of 06): In-Place AI Enablement

Goal:

- Let users ask questions in plain English and get answers grounded in live Oracle data, without the traditional “log into Oracle UI and run reports” workflow.

High-level idea:

- Treat the Oracle schema as a live endpoint for an agent/coplanar AI system.

## Six paths to connect agents to Oracle AI Database@Azure

The post frames **six integration paths**. Each path is a different way for an agent (Teams + Copilot/Foundry) to reach Oracle data, trading off speed, governance, and engineering control.

### Path 1 — Copilot Studio + native Oracle connector

**Best for:** Fast start; low-code; business analysts; Teams chatbot.

How it works:

1. **User → Teams → Copilot Studio agent → Oracle**
   - The Copilot Studio agent uses the **native Oracle Database connector** as a knowledge source to query Oracle tables.
2. **Oracle → Copilot agent**
   - Returns live governed data based on user context and access controls.
3. **Copilot agent → Teams → User**
   - Response is assembled and returned to the user.

Strengths called out:

- Rapid rollout with minimal engineering.
- Low-code agent creation across business units.
- Identity governance via **Microsoft Entra ID** (MFA, Conditional Access).
- Governance alignment via **Microsoft Purview**, RBAC access enforcement.
- Fits Microsoft 365 / Power Platform workflow embedding.
- “Data-in-place”: no duplication, grounded in operational data.

Considerations:

- NL2SQL generation is mediated by Copilot Studio’s AI engine (less direct control over SQL generation).
- Grounding limited to table-level sources; less reuse of PL/SQL business logic.

Compatibility notes (as stated):

- Works broadly across Oracle Database@Azure deployment types (Base DB, Exadata, Exascale, ADB Serverless/Dedicated).
- On-prem Oracle reachable via Power Platform on-premises data gateway.

### Path 2 — ORDS + PL/SQL (deterministic REST APIs)

**Best for:** Regulated environments; full control; deterministic output.

How it works:

1. **User → Teams → Foundry** (intent submitted to a Foundry agent)
2. **Foundry → ORDS REST endpoint**
   - Uses OAuth 2.0 or Entra ID managed identity authentication.
   - The post suggests ORDS can be wrapped via Azure Functions / APIM or described via OpenAPI for tool registration.
3. **ORDS → PL/SQL stored procedure**
   - Hand-crafted SQL and business rules; enforcement via RLS/VPD.
4. **Oracle → ORDS → Foundry → Teams → User**

Strengths:

- No AI-generated SQL; explicit SQL statements.
- Strong IAM: OAuth 2.0 and **Entra ID managed identities**.
- Governance with **Row-Level Security (RLS)** and **Virtual Private Database (VPD)**.
- ORDS endpoints can be registered as tools/specs for agent access.
- Central policy/auth with **Azure API Management**.

Considerations:

- Requires PL/SQL skill.
- Endpoint lifecycle management/versioning overhead.
- New use cases require stored procedure/endpoint updates.

Compatibility notes (as stated):

- Works broadly across database estates; ORDS installable on Base DB/Exadata; built-in on Autonomous DB.

### Path 3 — ORDS + Select AI (Oracle NL→SQL using Azure OpenAI)

**Best for:** Production NL2SQL with governance inside Oracle.

How it works:

- Foundry calls an ORDS endpoint.
- Inside Oracle, a PL/SQL wrapper invokes **DBMS_CLOUD_AI.GENERATE** (“Select AI”).
- Select AI asks **Azure OpenAI** to generate SQL, validates it, executes it in Oracle, and returns results.

Key points emphasized:

- Data remains in Oracle (“Your data never leaves Oracle”).
- RLS/VPD enforcement occurs at runtime.

Considerations:

- PL/SQL required.
- Schema changes require profile updates.
- Select AI is described as limited to Autonomous Database.

Compatibility notes (as stated):

- Not supported on Base DB / Exadata / Exascale for Select AI; supported on Autonomous DB.
- “Sidecar” Autonomous DB can be used to unlock NL2SQL against non-Autonomous estates.

### Path 4 — Azure Functions + JDBC / python-oracledb (custom code)

**Best for:** Maximum flexibility; custom logic; multi-source aggregation.

How it works:

1. Foundry calls an HTTP-triggered **Azure Function**.
2. Function connects to Oracle via JDBC or python-oracledb.
3. Function executes direct SQL, custom PL/SQL, or calls DBMS_CLOUD_AI.GENERATE (where supported).

Strengths:

- Full developer control (retries, batching, caching, transformations).
- Fits existing Azure app footprints (Functions / App Service).
- Broad driver ecosystem (incl. ODP.NET mentioned).

Considerations:

- Higher build/operate burden than ORDS.

### Path 5 — Logic Apps (or Power Automate) + Oracle connector

**Best for:** Workflow orchestration where Oracle is one step in a multi-system process.

How it works:

- Foundry triggers a **Logic Apps** workflow.
- Logic Apps uses a pre-built **Oracle connector** for CRUD/stored procedures.
- Supports on-prem via data gateway.

Considerations:

- Connector does not natively invoke Select AI / DBMS_CLOUD_AI.
- On-prem gateway adds an extra runtime component.

### Path 6 — ORDS/MCP + in-database agent (Select AI Agent / PAF)

**Best for:** Complex reasoning workflows (multi-step agentic execution).

How it works (as described):

- Foundry calls Oracle via ORDS or an Oracle **MCP (Model Context Protocol)** server.
- Oracle’s agent runtime performs a ReAct-style loop, calling tools like NL2SQL, vector search, PL/SQL rules, external APIs.

Considerations:

- Requires Oracle 26ai Autonomous Database for in-database agent runtime.
- Higher configuration complexity.
- Higher token consumption/costs.
- MCP support described as maturing/preview; MCP server GA mentioned for March 2026.

## “Sidecar” architecture (concept)

The post describes a “sidecar” Autonomous Database instance on Oracle Database@Azure that:

- Does not store production data.
- Connects to existing databases (Exadata, Base DB, on-prem) via links.
- Provides AI/NL2SQL/agent capabilities without touching production systems.

## References (links preserved)

- Oracle Database@Azure regions: https://learn.microsoft.com/en-us/azure/oracle/oracle-db/oracle-database-regions
- Build Copilots with Copilot Studio + Oracle Database@Azure: https://techcommunity.microsoft.com/blog/oracleonazureblog/build-your-own-custom-copilots-with-microsoft-copilot-studio-and-oracle-database/4468211
- Add Oracle as a knowledge source in Copilot Studio: https://learn.microsoft.com/en-us/power-platform/release-plan/2025wave1/microsoft-copilot-studio/add-oracle-as-knowledge-source
- Connect Logic Apps to Azure OpenAI and AI Search: https://learn.microsoft.com/en-us/azure/logic-apps/connectors/azure-ai
- Microsoft Foundry Agent Service overview: https://learn.microsoft.com/en-us/azure/foundry/agents/overview
- Extend agents with REST APIs / OpenAPI specs (Copilot Studio): https://learn.microsoft.com/en-us/microsoft-copilot-studio/agent-extend-action-rest-api
- Copilot Studio integration guidance: https://learn.microsoft.com/en-us/microsoft-copilot-studio/guidance/integrations

## Disclaimer (as stated)

The post recommends validating capability/compatibility/availability/licensing/support directly with Oracle and Microsoft, and treating the figures and patterns as directional until proven via a PoC/pilot.


[Read the entire article](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/your-oracle-data-is-sitting-next-to-microsoft-ai-are-you-using/ba-p/4509438)

