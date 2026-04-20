---
title: "Operational AI Agents: PR Workflows, Governance Controls, and Tooling Baselines"
author: "TechHub"
date: 2026-04-20 09:00:00 +00:00
tags: ["GitHub Copilot","Copilot Cloud Agent","Copilot CLI","Custom Skills","Model Routing","Data Residency","FedRAMP","MCP (Model Context Protocol)","Microsoft Foundry","Agent Governance","Azure Kubernetes Service (AKS)","Kubernetes Gateway API","Microsoft Fabric","OneLake",".NET 11"]
section_names:
- ai
- github-copilot
- azure
- devops
- security
- dotnet
- ml
primary_section: "github-copilot"
feed_name: "TechHub"
external_url: "/all/roundups/Weekly-AI-and-Tech-News-Roundup-2026-04-20"
---

This week's roundup focuses on agents becoming part of day-to-day operations across PRs, terminals, and production tooling. GitHub Copilot added more workflow-native automation (including PR conflict resolution and per-run model choice) and expanded the controls teams need in practice, such as data residency/FedRAMP routing and admin-gated rollouts. In parallel, Microsoft Foundry and Fabric reinforced a "run it like software" approach with IDE-native agent building, evaluation-as-tests, governed data tooling via MCP, and clearer hosting and governance options. Across Azure and DevOps, the same pattern shows up in day-two readiness work: AKS ingress migration paths, automated backups, evidence-based incident tooling, and security platforms moving toward centralized enablement with automation that behaves more predictably.

<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Copilot agents move deeper into PRs and github.com workflows](#copilot-agents-move-deeper-into-prs-and-githubcom-workflows)
  - [Governance and compliance: data residency, FedRAMP routing, and controlled rollouts](#governance-and-compliance-data-residency-fedramp-routing-and-controlled-rollouts)
  - [The “skills” ecosystem grows up: Custom Skills, `gh skill`, and enterprise plugin catalogs](#the-skills-ecosystem-grows-up-custom-skills-gh-skill-and-enterprise-plugin-catalogs)
  - [Copilot CLI: auto model routing, remote-controlled sessions, and “plan → implement” workflows](#copilot-cli-auto-model-routing-remote-controlled-sessions-and-plan--implement-workflows)
  - [Models and IDE integrations: Opus 4.7 rollout, Visual Studio changes, and Azure MCP built-in](#models-and-ide-integrations-opus-47-rollout-visual-studio-changes-and-azure-mcp-built-in)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Microsoft Foundry in the IDE: end-to-end agent and model workflows](#microsoft-foundry-in-the-ide-end-to-end-agent-and-model-workflows)
  - [Production agent architecture: security, governance, memory, and hosting choices](#production-agent-architecture-security-governance-memory-and-hosting-choices)
  - [MCP and data-connected agents: Fabric OneLake, Postgres, and Oracle Database@Azure](#mcp-and-data-connected-agents-fabric-onelake-postgres-and-oracle-databaseazure)
  - [Model updates and customization: text-to-image efficiency and reinforcement fine-tuning](#model-updates-and-customization-text-to-image-efficiency-and-reinforcement-fine-tuning)
  - [Copilot Studio orchestration: mixing agents with workflows](#copilot-studio-orchestration-mixing-agents-with-workflows)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Microsoft Fabric: lower-friction ingestion and a more consistent analysis surface](#microsoft-fabric-lower-friction-ingestion-and-a-more-consistent-analysis-surface)
- [Azure](#azure)
  - [AKS platform operations: ingress shifts, backups, and AI-assisted investigations](#aks-platform-operations-ingress-shifts-backups-and-ai-assisted-investigations)
  - [Observability and incident response automation: SRE Agent connectors and log ingestion migrations](#observability-and-incident-response-automation-sre-agent-connectors-and-log-ingestion-migrations)
  - [Azure networking and hybrid connectivity: Private DNS fallback, ExpressRoute to MVE, and packet mirroring](#azure-networking-and-hybrid-connectivity-private-dns-fallback-expressroute-to-mve-and-packet-mirroring)
  - [Data services: Fabric schema safety, migration parity, and the next wave of PostgreSQL](#data-services-fabric-schema-safety-migration-parity-and-the-next-wave-of-postgresql)
  - [App and messaging services: Web PubSub wildcard roles and Service Bus request/reply scaling](#app-and-messaging-services-web-pubsub-wildcard-roles-and-service-bus-requestreply-scaling)
  - [Other Azure News](#other-azure-news)
- [.NET](#net)
  - [.NET 11 Preview 3: runtime, SDK/CLI, web/WASM, MAUI, EF Core, and containers](#net-11-preview-3-runtime-sdkcli-webwasm-maui-ef-core-and-containers)
  - [Servicing and support timelines: April 2026 patches and a deadline for ASP.NET Core 2.3 on .NET Framework](#servicing-and-support-timelines-april-2026-patches-and-a-deadline-for-aspnet-core-23-on-net-framework)
- [DevOps](#devops)
  - [AI agents in developer and ops workflows (VS Code + Docker)](#ai-agents-in-developer-and-ops-workflows-vs-code--docker)
  - [GitHub workflow and governance updates (Stacked PRs, rulesets, status transparency)](#github-workflow-and-governance-updates-stacked-prs-rulesets-status-transparency)
  - [Azure SRE Agent automation for AKS incidents and IaC drift](#azure-sre-agent-automation-for-aks-incidents-and-iac-drift)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [GitHub supply chain and code security: OIDC, registries, SBOMs, and better triage mechanics](#github-supply-chain-and-code-security-oidc-registries-sboms-and-better-triage-mechanics)
  - [GitHub secret scanning and rapid exposure baselining](#github-secret-scanning-and-rapid-exposure-baselining)
  - [Azure DevOps Advanced Security: default CodeQL setup and org-wide alert coordination](#azure-devops-advanced-security-default-codeql-setup-and-org-wide-alert-coordination)
  - [Threat research: social engineering via Teams/Quick Assist and macOS AppleScript tradecraft](#threat-research-social-engineering-via-teamsquick-assist-and-macos-applescript-tradecraft)
  - [Azure identity-first security patterns: passwordless AKS secret sync and Entra ID for Storage SFTP](#azure-identity-first-security-patterns-passwordless-aks-secret-sync-and-entra-id-for-storage-sftp)
  - [AI and cryptography readiness: operational playbooks, not just features](#ai-and-cryptography-readiness-operational-playbooks-not-just-features)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week's Copilot updates were less about new chat features and more about making Copilot usable in operational workflows: agents that work in PRs and terminals, stronger admin controls (including data location), and portable "skills" and tool catalogs that keep behavior consistent. This continues last week's thread: as Copilot expands from IDE chat and autocomplete into PR and branch agents, CLI orchestration, and MCP tooling, GitHub is filling in the gaps around control, traceability, and rollout management.

### Copilot agents move deeper into PRs and github.com workflows

After last week's work to make cloud-agent workflows faster and easier (parallel validations, Dependabot-to-agent remediation, and Mobile flows), GitHub is shifting more agent work onto github.com. A new **"Fix with Copilot"** button resolves PR merge conflicts using the **Copilot cloud agent**: click the button, post a prefilled PR comment, and the agent resolves conflicts, runs builds and tests, and pushes updates back to the PR. The goal matches last week's "shorter path to merge-ready" idea: fewer local context switches, while normal commits and repo validations still apply.

The cloud-agent path also expands through `@copilot` mentions in PR conversations for common maintenance tasks such as fixing failing GitHub Actions tests, addressing review comments, or adding unit tests for edge cases. This carries last week's "agent-in-the-workflow" approach beyond security remediation into everyday PR work, while keeping changes inside the standard PR audit trail. Admin control remains a core requirement: Copilot cloud agent is available on paid plans, but **Copilot Business/Enterprise admins must enable cloud agent** before developers can use these PR commands and buttons.

GitHub also added **per-run model selection for the third-party Claude and Codex agents on github.com**, so you choose the model at kickoff instead of relying on a single default. This matches last week's theme of making choices explicit (permission modes, thinking effort, BYOK/provider controls): per-run selection makes it clearer what you ran and why outputs differ. Enterprise enablement remains policy-driven: admins must enable Anthropic Claude and/or OpenAI Codex policies, and repo owners must enable the agent under **Settings -> Copilot -> Cloud agent**.

- [Fix merge conflicts in three clicks with Copilot cloud agent](https://github.blog/changelog/2026-04-13-fix-merge-conflicts-in-three-clicks-with-copilot-cloud-agent)
- [Enable Copilot cloud agent via custom properties](https://github.blog/changelog/2026-04-15-enable-copilot-cloud-agent-via-custom-properties)
- [Model selection for Claude and Codex agents on github.com](https://github.blog/changelog/2026-04-14-model-selection-for-claude-and-codex-agents-on-github-com)

### Governance and compliance: data residency, FedRAMP routing, and controlled rollouts

This week's governance updates continue last week's pattern of controls catching up with autonomy (stronger VS Code agent permissions, offline/BYOK in CLI, more reporting). Copilot now supports **data residency for US and EU regions**, plus routing through **FedRAMP Moderate-authorized model hosts and infrastructure** for US government compliance. GitHub says all GA Copilot capabilities are included (Agent mode, inline suggestions, Copilot Chat, cloud agent, Code Review, PR summaries, Copilot CLI), and requests route to compliant endpoints in the selected geography. In other words, the "where does it go?" control is aligning with the "what can it do?" controls across IDE, CLI, and github.com.

Supported models span OpenAI and Anthropic (including **GPT-5.4**, **Claude Sonnet 4.6**, **Claude Opus 4.6**) with a model-by-region matrix. Teams should expect constraints, especially if they used last week's model configurability: **Gemini isn't supported** (there are no suitable data-resident inference endpoints in this setup), and new models may show up later in resident regions. There is also a cost change: data residency/FedRAMP requests apply a **10% increase to the model multiplier** for premium request accounting, which ties back to last week's budgeting and limits planning (quotas, deprecations, plan standardization).

For customers on **Data Resident GitHub Enterprise Cloud** (ghe.com / "Proxima"), the practical next step is enabling enforcement policies like **"Restrict Copilot to Data Residency Models"** so inference and Copilot data stay in-region. Plan for previews and newly released features to land later in data-residency tenants, so enablement guidance may need to differ between standard Enterprise Cloud and ghe.com. That mirrors last week's split between "the feature exists" and "the feature is manageable at org scale."

- [GitHub Copilot data residency in US + EU and FedRAMP compliance now available](https://github.blog/changelog/2026-04-13-copilot-data-residency-in-us-eu-and-fedramp-compliance-now-available)
- [GitHub Copilot now available with Data Residency in the EU](https://jessehouwing.net/github-copilot-now-with-data-residency-in-europe/)

### The “skills” ecosystem grows up: Custom Skills, `gh skill`, and enterprise plugin catalogs

Copilot customization is moving from one-off prompting to **versioned, shareable runbooks**, which matches last week's IDE and CLI direction around instruction management, better agent hooks, and more repeatable sessions with artifacts. GitHub Copilot **Custom Skills** are the runbook unit for Agent mode, typically a folder under `.github/skills/<name>/` with required **SKILL.md** plus optional scripts, templates, and resources. The guidance is specific: Copilot first reads only the skill's **name + description (~100 tokens)** to decide whether to load it, so descriptions should lead with trigger keywords and phrasing, and the skill should load referenced files only when needed. The "deployment-health" example encodes a real check: a Python script reads endpoints, measures latency and thresholds, generates an HTML report, and Copilot runs it and summarizes results. That aligns with last week's focus on inspectable, repo-resident artifacts.

Managing skills is the next step, and GitHub addresses it with **`gh skill`** in **GitHub CLI v2.90.0+** (public preview). It reads like last week's governance follow-through, but applied to the customization supply chain: install, update, and publish workflows with provenance controls. You can pin installs to a **git tag** or **commit SHA**, detect upstream changes via **git tree SHA**, and store provenance in SKILL.md frontmatter so the origin remains even if copied. For authors, `gh skill publish` validates against the **agentskills.io** spec and pushes security practices (tag protection, secret scanning, code scanning), with optional immutable releases tied to tags. GitHub also calls out the risk model: skills can contain malicious scripts or prompt injection, so `gh skill preview` is recommended before installing. This matches last week's safety framing (sandboxing MCP servers, safer execution, better debugging).

Separately, Azure introduced an **Azure API Center plugin marketplace endpoint** (public preview) to help platform teams distribute approved tool extensions. This connects directly to last week's MCP operationalization theme: API Center provisions a Git-based **`marketplace.git`** endpoint that tools can consume. The post names **Claude Code** and **GitHub Copilot CLI** as consumers: developers add the URL and browse or install plugins from a governed, authenticated source instead of relying on internal docs. The practical result is that skills, MCP servers, and plugins can behave more like managed internal packages with central inventory and consistent installs. That matches what we're also seeing with PR cloud agents and the CLI as a formal runtime.

- [Supercharge Your Dev Workflows with GitHub Copilot Custom Skills](https://techcommunity.microsoft.com/t5/microsoft-developer-community/supercharge-your-dev-workflows-with-github-copilot-custom-skills/ba-p/4510012)
- [Manage agent skills with GitHub CLI](https://github.blog/changelog/2026-04-16-manage-agent-skills-with-github-cli)
- [Introducing the plugin marketplace for Azure API Center](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-the-plugin-marketplace-for-azure-api-center/ba-p/4512231)

### Copilot CLI: auto model routing, remote-controlled sessions, and “plan → implement” workflows

This week's CLI changes extend last week's arc: from "Copilot in the terminal" to a configurable agent runtime (BYOK/local models, offline mode, MCP tools, plugins) with clearer controls. First, **auto model selection** is now GA in Copilot CLI across all plans: set the model to "auto" and each request routes to an appropriate model (currently including **GPT-5.4**, **GPT-5.3-Codex**, **Sonnet 4.6**, **Haiku 4.5**) to improve availability and reduce rate limits. The CLI shows which model handled each request, continuing last week's theme that traceability matters. Billing follows the chosen model multiplier; auto is limited to 0x-1x models and includes a **10% multiplier discount** for paid subscribers (for example, 1x billed as 0.9 under auto), which ties back to last week's cost and ops planning.

Second, GitHub is previewing **remote control for Copilot CLI sessions** via `copilot --remote`. The session streams to GitHub and provides a link or QR code to view or control from the web or GitHub Mobile (with prerelease mobile builds). This continues last week's "Copilot in more places" direction by turning a long-running CLI session into a shared artifact, rather than a separate mobile or web copilot. You can send steering messages (including queued "keep going"), review and edit the plan before execution, switch modes (plan/interactive/autopilot), stop the session, and approve or deny permission prompts, while still respecting existing CLI permissions and Business/Enterprise policies. It is the same "more autonomy with explicit approvals and governance hooks" idea, applied to where supervision happens.

Hands-on build stories show how the modes fit together. One uses **plan mode** to generate `plan.md`, then switches models (for example, Sonnet for planning and **Claude Opus 4.7** for implementation) to build a Node.js terminal UI "emoji list generator" using **@github/copilot-sdk**, **@opentui/core**, and **clipboardy**. That kind of model switching matches last week's cross-model review idea: planning, implementation, and review as separate phases with checkpoints. Another example (a personal "command center" desktop app) shows a split between supervised changes in VS Code interactive agent mode and background work delegated to Copilot Cloud Agent, along with the practical reminder that agents often add code faster than they remove it, so cleanup still benefits from human refactoring. A short onboarding demo shows Copilot CLI generating a plain-English repo overview from the terminal instead of manual file browsing.

- [GitHub Copilot CLI now supports Copilot auto model selection](https://github.blog/changelog/2026-04-17-github-copilot-cli-now-supports-copilot-auto-model-selection)
- [Remote control Copilot CLI sessions on web and mobile (public preview)](https://github.blog/changelog/2026-04-13-remote-control-cli-sessions-on-web-and-mobile-in-public-preview)
- [Remote control GitHub Copilot CLI sessions on web and mobile (public preview)](https://www.youtube.com/watch?v=dRSHSZ53c1g)
- [Building an emoji list generator with the GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/building-an-emoji-list-generator-with-the-github-copilot-cli/)
- [Build a personal organization command center with GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/build-a-personal-organization-command-center-with-github-copilot-cli/)
- [How a GitHub engineer built an AI productivity hub with Copilot CLI](https://www.youtube.com/watch?v=BDZKubrUO1M)
- [How to understand any codebase in seconds](https://www.youtube.com/shorts/CwpmuUh9izg)

### Models and IDE integrations: Opus 4.7 rollout, Visual Studio changes, and Azure MCP built-in

Model availability and IDE ergonomics shifted this week, connecting to last week's themes around model churn, explicit settings, and MCP parity across surfaces. GitHub Copilot is rolling out **Claude Opus 4.7** as a GA model across IDEs, CLI, github.com, GitHub Mobile, and Copilot Coding Agent. The picker is also being simplified: **Opus 4.7 will replace Opus 4.5 and 4.6** in the Pro+ model picker over the coming weeks, which reinforces last week's reminder to document intent (speed/cost/reasoning) rather than hard-coding versions. On cost, Opus 4.7 launches with a **7.5x premium request multiplier** under promo pricing through **April 30**, and Business/Enterprise still require admins to enable the Opus 4.7 policy. That means more model choice, but still behind org policy.

In Visual Studio, two Copilot-adjacent changes stood out. Visual Studio 2022 now ships **Azure MCP tools built in** (via the "Azure development" workload) starting with **VS 17.14.30+**, so you no longer need a separate "Copilot for Azure" extension. This follows last week's trajectory where MCP becomes part of the default IDE surface rather than an add-on. Once enabled in Copilot Chat's tool picker, the Azure MCP Server exposes **230+ tools across 45 Azure services** for tasks like listing resources, deploying via **azd** to App Service, running AppLens and health checks, and generating and running **KQL** queries in Log Analytics. Updates now follow the Visual Studio Installer cadence, but tools are off by default and gated by Azure permissions plus sign-in to GitHub and Azure. That matches last week's framing around explicitly allowed surfaces.

Visual Studio 2026 **18.5** changes completion behavior by prioritizing IntelliSense list completions so Copilot inline suggestions do not compete for the same accept gesture. It also adds a Copilot-driven **debugger agent** workflow: from an issue link or prompt, Copilot forms hypotheses, sets conditional breakpoints, runs under the debugger, and proposes fixes based on what it observes. This matches last week's theme of richer debugging and more autonomy, but with a loop driven by runtime state rather than only static edits. Coverage also notes ongoing friction (reduced theme token granularity impacting readability, more forceful update prompts), which matters because Copilot usage depends on IDE stability.

- [Claude Opus 4.7 is generally available in GitHub Copilot](https://github.blog/changelog/2026-04-16-claude-opus-4-7-is-generally-available)
- [Azure MCP tools now ship built into Visual Studio 2022 — no extension required](https://devblogs.microsoft.com/visualstudio/azure-mcp-tools-now-ship-built-into-visual-studio-2022-no-extension-required/)
- [Visual Studio 18.5 adds Copilot debugger agent workflow, but devs complain about theme contrast and forced updates](https://www.devclass.com/ai-ml/2026/04/17/visual-studio-185-lands-with-ai-debugging-at-a-price-devs-still-feeling-blue/5218068)

### Other GitHub Copilot News

Security and compliance are showing up more often in day-to-day agent adoption guidance, not only as admin settings. This matches last week's shift toward governance that you can measure (usage metrics, security entry points, constrained capabilities with audit trails). A DevOps playbook argues that agents amplify existing engineering discipline, recommends treating repos as explicit agent interfaces (via `.github/copilot-instructions.md` and spec and constraint files), and using CI/CD as an active verifier with structural, semantic, and provenance checks plus defenses against prompt injection and supply-chain abuse.

- [DevOps Playbook for the Agentic Era](https://devblogs.microsoft.com/all-things-azure/agentic-devops-practices-principles-strategic-direction/)

GitHub Code Quality public preview added small but practical UI improvements. Standard findings now support file-path search, bulk dismiss and reopen, and richer diagnostic context, with Copilot Autofix still attached to each finding to shorten the triage-to-fix loop. This continues last week's "shorter remediation loop" story (Ask Copilot in assessments, agent-assigned Dependabot alerts) by moving Copilot closer to fix queues.

- ['GitHub Code Quality: Improvements to standard findings in public preview'](https://github.blog/changelog/2026-04-14-github-code-quality-improvements-to-standard-findings-in-public-preview)

A .NET-focused comparison walks through tradeoffs between a single chat agent, an MCP/function-calling tool-using agent, and a multi-agent "team," using a **.NET Aspire + ASP.NET Core Minimal API** rate-limiting implementation (with integration tests). It complements last week's multi-agent and checkpoint direction (CLI `/fleet`, cross-model review patterns) by helping decide when extra agent structure is worth the added complexity.

- [Single-agent, tools, or a team? A practical comparison of AI coding setups](https://hiddedesmet.com/single-agent-tools-or-a-team)

Docker Desktop's Docker Sandbox microVMs are presented as a way to let Copilot-driven refactoring run `docker build`, Compose, and tests without mounting the host Docker socket (and without giving root-equivalent access to the host daemon). This matches last week's containment theme (sandboxing MCP servers, offline/BYOK controls, runner/firewall placement): as agents do more real work, teams are putting safer execution boundaries around builds and infrastructure access.

- ['Best of Both Worlds for Agentic Refactoring: GitHub Copilot + MicroVMs via Docker Sandbox'](https://devblogs.microsoft.com/all-things-azure/best-of-both-worlds-for-agentic-refactoring-github-copilot-microvms-via-docker-sandbox/)

A GitHub video roundup is mainly useful for two Copilot-adjacent milestones: Copilot SDK public preview and Copilot CLI adding BYOK/local-model support (plus a reminder to tighten dependency hygiene after recent npm incidents). It is effectively a recap of last week's two main threads: SDK-standardized sessions, tools, and tracing, and the CLI as a configurable runtime. It also shows how quickly these topics are turning into baseline knowledge.

- ['The Download: Copilot SDK, Claude Mythos, AI models are protecting each other & more'](https://www.youtube.com/watch?v=0vvY2AGg4sM)

Training content continues to address adoption gaps around customization and safe usage. This includes a beginner CLI tutorial on composing instructions, skills, and custom agents; a migration video showing Copilot assisting Oracle-to-PostgreSQL in VS Code while moving to Azure Database for PostgreSQL; and a VS Live playlist with AI/Copilot workflow talks alongside .NET, ASP.NET Core, and GitHub Actions. Alongside last week's focus on instruction management and troubleshooting (editors, debug logs, verifying sent context), the learning trend is "safe and repeatable," not only "how to prompt."

- [How to use agents, skills, and instructions in Copilot CLI (beginner tutorial)](https://www.youtube.com/watch?v=-yKALFS5ewY)
- ['PostgreSQL Like a Pro: Migrate to managed PostgreSQL on Azure (incl. AI-assisted Oracle-to-Postgres demo)'](https://www.youtube.com/watch?v=Suvakz3yJgM)
- ['From AI to .NET: 20 VS Live! Las Vegas Sessions You Can Watch Now'](https://devblogs.microsoft.com/visualstudio/from-ai-to-net-20-vs-live-las-vegas-sessions-you-can-watch-now/)

## AI

This week's AI news leaned into making agent development look more like normal software engineering: tighter IDE loops for building, debugging, and evaluating; clearer production hosting and orchestration options; and concrete patterns for connecting agents to governed data and automation. This continues last week's "run it like software" framing where stable runtimes, inspectable tool contracts, and day-two controls (identity, policy, cost, evaluation) become the default rather than add-ons. Microsoft Foundry and Fabric also expanded platform capabilities with new models, fine-tuning options, MCP toolchains, and agent experiences that are easier to monitor and audit.

### Microsoft Foundry in the IDE: end-to-end agent and model workflows

Building on last week's Foundry standardization theme (Responses API compatibility, Agent Service GA, cloud+local deployment), Microsoft Foundry Toolkit for VS Code is now GA. It positions VS Code as the place where experimentation, agent engineering, evaluation-as-tests, deployment, and on-device model work (enabled by Foundry Local GA) can live in one loop.

For experimentation, the Model Catalog includes 100+ cloud and local models (OpenAI/Anthropic/Google plus local ONNX/Foundry Local/Ollama), along with a Model Playground for side-by-side comparisons, multimodal testing, optional web search, and streaming. "View Code" generates Python/JavaScript/C#/Java snippets that match what you tested, which mirrors last week's focus on keeping a tight contract between what you test and what you run.

Agent building splits into low-code Agent Builder (prompt optimizer, tool catalog including local MCP servers, tool approvals, save to Foundry) and code-first scaffolding aligned to frameworks like Microsoft Agent Framework and LangGraph. The toolkit treats MCP servers as first-class tool sources inside the IDE, continuing last week's MCP operationalization thread. Agent Inspector adds IDE-native debugging and observability: F5 debugging with breakpoints, stepping and variables, streaming tool-call visualization, workflow graphs, and local span tracing across tool calls. That brings last week's "observability is design" message earlier into local development.

Evaluations show up as tests: pytest-style definitions run in VS Code Test Explorer, results can be analyzed in Data Wrangler, and reused for scaled runs in Foundry. This fits last week's point that evaluation, monitoring, and cost controls are day-two expectations, not something you bolt on later.

The GA deep dive also treats Windows on-device AI as part of the same surface, extending last week's "cloud + local" story with IDE controls. The local pipeline converts, quantizes, and evaluates Hugging Face models into ONNX for Windows ML, and targets execution providers across hardware (OpenVINO, TensorRT, Qualcomm QNN, AMD MIGraphX/VitisAI). Profiling includes CPU/GPU/NPU/memory views, Windows ML event breakdown (startup vs per-request), and operator-level tracing showing placement and timing. It also supports LoRA fine-tuning for Phi Silica via a cloud job on Azure Container Apps, then downloads the adapter for Phi Silica LoRA APIs. The intent is to reduce bespoke ML infrastructure when you only need an adapter.

- [Foundry Toolkit for VS Code: A Deep Dive on GA](https://techcommunity.microsoft.com/t5/microsoft-developer-community/foundry-toolkit-for-vs-code-a-deep-dive-on-ga/ba-p/4509510)

### Production agent architecture: security, governance, memory, and hosting choices

As agents move from prototypes to production, guidance converged on control: observability does not help much without enforceable policies (tool allowlists, least privilege, auditable decisions) and a hosting model that fits scale and operational constraints. This continues last week's shift from "build an agent" to "run an agent," where tracing, identity, evaluation, and guarded automation are treated as core design inputs.

On governance, the Agent Governance Toolkit walkthrough shows deterministic runtime policy enforcement for a multi-agent ASP.NET Core app on Azure App Service (MAF 1.0) using Microsoft.AgentGovernance 3.0.2. The flow is middleware-like: default-deny governance-policies.yaml with allow rules, loaded into a GovernanceKernel at startup (audit + metrics), then builder.UseGovernance(kernel, AgentName) so tool calls are evaluated before execution. This complements last week's "interruptible tools" and human approvals by making governance a runtime gate across agents. Decisions and reasons land in Application Insights alongside OpenTelemetry traces, with KQL to find violations (customDimensions["governance.decision"] != "ALLOWED") and track token budgets via customMetrics ("governance.tokens.consumed"). It also extends into reliability with YAML SLOs and circuit breakers that reduce autonomy as error budgets burn, which matches last week's "guardrails before automation" sequencing.

The Foundry security architecture checklist maps agent risks (prompt injection, tool misuse, exfiltration, over-privilege, drift) into Azure patterns: managed identities and Entra RBAC, private endpoints/Private Link, Key Vault, API Management gateways, tool allowlists, and strict output validation (JSON schema). It also calls for CI/CD evaluation and red-teaming so prompt and model changes trigger regression tests that can block deploys. This reinforces last week's MCP auth patterns (managed identity vs OAuth passthrough vs secrets) in an end-to-end posture: tool auth is not enough without least privilege, network boundaries, and validation.

Statefulness also advanced with Foundry managed memory (preview), positioned as built-in long-term persistence integrated with Microsoft Agent Framework and LangGraph. Hooks include per-user scoping, automatic extraction (the platform decides what to store), and CRUD APIs so apps can inspect and correct memory (including "forget this"). It reduces the need for custom memory stores and standardizes user controls.

For hosting, a guide compared Container Apps, AKS, Functions, App Service, Foundry Agents, and Foundry Hosted Agents, then focused on Hosted Agents as "containerized custom app + agent-native APIs." This ties to last week's Agent Service GA by showing how runtime and code package together while keeping agent concepts like Responses protocols and telemetry export. The walkthrough is implementation-level: LangGraph calculator agent + adapter, agent.yaml (kind: hosted, protocols: responses), Python 3.11-slim container on port 8088, deployed via azd and azure.ai.agents extension (build/push to ACR, create/start Hosted Agent). It also calls out scale-to-zero vs min-replicas cold-start tradeoffs, automatic OpenTelemetry export to App Insights, and RBAC implications (publishing can create a dedicated agent identity that needs separate permissions from the project managed identity).

- [Architecting Secure and Trustworthy AI Agents with Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/architecting-secure-and-trustworthy-ai-agents-with-microsoft/ba-p/4506580)
- [Govern AI Agents on App Service with the Microsoft Agent Governance Toolkit](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/govern-ai-agents-on-app-service-with-the-microsoft-agent/ba-p/4510962)
- [Using Microsoft Agent Framework with Foundry managed memory](https://www.youtube.com/watch?v=DZn9bNDEs4U)
- [Choosing the Right Azure Hosting Option for Your AI Agents](https://devblogs.microsoft.com/all-things-azure/hostedagent/)

### MCP and data-connected agents: Fabric OneLake, Postgres, and Oracle Database@Azure

Tool calling and agent-to-data patterns continue consolidating around MCP, extending last week's shift from "tool glue" to supportable infrastructure (hosting, auth, self-hosted Azure MCP Server 2.0). The emphasis is on explicit discovery (schemas, metadata, permissions) so agents behave predictably, and on identity and RBAC as the primary safety boundary.

In Fabric, OneLake MCP tools are GA: a 19-command toolset for discovering workspaces and items, inspecting schemas and metadata via Table APIs, and browsing, reading, writing, and mapping storage via OneLake File/Access APIs. All access is constrained by the caller's Azure identity and Fabric permissions. This pairs with last week's Fabric "intelligence platform" guidance: instead of copying data into agent pipelines, you expose governed surfaces (semantic models, Table APIs, OneLake files) as tools with enforceable permissions. The example is practical: an agent inventories a mirrored database ("House Price Open Mirror"), documents schemas, distinguishes Parquet landing zones from managed Delta outputs, and checks replication health and monitoring signals to generate docs and basic health reports without manual portal work.

For Postgres, a Foundry + PostgreSQL walkthrough emphasizes MCP as a controlled integration layer for exploring the database, pairing natural-language-to-SQL with vector search for RAG retrieval. This matches last week's Entra-authenticated MCP guidance (pre-authorized clients, JWT validation, OBO): database access becomes a governable tool surface instead of a broad connection string. A related "PostgreSQL Like a Pro" announcement points to more demos on modernizing Postgres apps on Azure, including MCP agent patterns and AI-assisted Oracle-to-Azure Database for PostgreSQL migrations in VS Code, pointing toward IDE loops where the agent helps iterate on conversion issues.

For Oracle estates, an Oracle Database@Azure patterns article lays out options based on how deterministic you need behavior to be: Copilot Studio + Oracle connectors; ORDS + PL/SQL REST APIs for predictable behavior (with DB governance like RLS/VPD); and hybrid Oracle "Select AI" (DBMS_CLOUD_AI.GENERATE) using Azure OpenAI to generate and validate SQL inside Oracle. It includes code-first Azure Functions (JDBC/python-oracledb), Logic Apps/Power Automate orchestration, and an advanced direction where MCP and in-database runtimes participate in ReAct-style loops. That continues last week's "MCP as governed automation interface" thread applied to database operations and policy controls.

- [Give your AI agent the keys to OneLake: OneLake MCP (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/give-your-ai-agent-the-keys-to-onelake-onelake-mcp-generally-available/)
- [Build smart and secure agents with PostgreSQL on Azure using Microsoft Foundry](https://www.youtube.com/watch?v=aMqrpk6ge9A)
- [Take your PostgreSQL-backed apps to the next level](https://devblogs.microsoft.com/blog/take-your-postgresql-backed-apps-to-the-next-level)
- [Six agent integration patterns for Oracle Database@Azure with Microsoft Foundry, Azure OpenAI, Copilot Studio, and Logic Apps](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/your-oracle-data-is-sitting-next-to-microsoft-ai-are-you-using/ba-p/4509438)

### Model updates and customization: text-to-image efficiency and reinforcement fine-tuning

Foundry expanded its model surface in two ways: a higher-throughput text-to-image option intended for production usage, and more workflow controls for reinforcement fine-tuning with graders you can tune over time. This continues last week's "right model per step" and "model choice as an engineering setting" thread, expanding beyond text agents to image pipelines and training workflows.

MAI-Image-2-Efficient (MAI-Image-2e) is now available in Microsoft Foundry without a waitlist, positioned as the throughput-focused sibling to MAI-Image-2 for high-volume and interactive generation. The announcement is explicit about tradeoffs: MAI-Image-2e targets latency and per-image cost (pricing: $5 per 1M text input tokens and $19.50 per 1M image output tokens), while MAI-Image-2 remains positioned for higher fidelity (portraits/photorealism, stylized outputs, longer/complex in-image text). Benchmark context (NVIDIA H100 at 1024x1024 with normalization notes) helps set expectations for how batch size and concurrency affect real deployments.

The April 2026 Foundry fine-tuning update focused on reinforcement fine-tuning (RFT) for o4-mini with api-version=2025-04-01-preview. Key changes include "global training" (trainingType: "globalstandard") across 13+ regions to lower per-token training rates with consistent infrastructure and model quality, plus more grader options. RFT now supports model graders using GPT-4.1, GPT-4.1-mini, and GPT-4.1-nano, alongside deterministic graders (string checks, Python, endpoint-based). Guidance is operational: start deterministic for speed, cost, and reproducibility, use model graders for semantic or multi-dimensional partial credit, and iterate nano -> mini -> full as rubrics stabilize. It also calls out common pitfalls (role ordering, schema mismatches, missing structured response_format when graders reference output_json) and reinforces a best practice aligned with last week's MCP thread: treat tools as part of the environment and consider MCP for production tools even if you also offer a function-calling-compatible interface for fine-tuning.

- [MAI-Image-2-Efficient in Microsoft Foundry: flagship-quality text-to-image with lower cost and faster latency](https://microsoft.ai/news/mai-image-2-efficient/)
- [What’s New in Microsoft Foundry Fine-Tuning | April 2026](https://devblogs.microsoft.com/foundry/whats-new-in-foundry-finetune-april-2026/)

### Copilot Studio orchestration: mixing agents with workflows

Copilot Studio added clearer orchestration primitives for business process automation scenarios where you want agent reasoning but still need deterministic execution and audit trails. This builds on last week's focus on approvals, guarded automation, and explicit topology by formalizing two patterns inside a workflow engine. The update describes "agent-in-workflow" (agent nodes for interpretation/synthesis at specific steps, with request-for-information escalation) and "workflow-as-tool" (agents call workflows as reliable sub-process tools, authored in natural language or reused from a library). The practical benefit is keeping the workflow as the orchestrator (branching, handoffs, approvals stay explicit) while inserting LLM steps where ambiguity is unavoidable.

- [New capabilities help you automate business processes by mixing AI agents and workflows](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/automate-business-processes-with-agents-plus-workflows-in-microsoft-copilot-studio/)

### Other AI News

Visual Studio's Copilot Chat gained a "Debugger Agent" workflow in Visual Studio 18.5 GA that ties an agent into a live debugging session. You provide a GitHub/ADO item (or description), it proposes a hypothesis, sets breakpoints (with approval), observes the repro, inspects runtime state (variables/call stack), then proposes or applies a fix and reruns. This complements the Foundry Toolkit "tight IDE loop" story: with agent inspectors, evaluation-as-tests, and debugger participation, agents can be tested and observed with familiar software engineering workflows.

- [Stop Hunting Bugs: Meet the New Visual Studio Debugger Agent](https://devblogs.microsoft.com/visualstudio/stop-hunting-bugs-meet-the-new-visual-studio-debugger-agent/)

.NET agent framework work introduced a concrete "skills" composition pattern: file-based skills on disk, class-based skills via NuGet, and inline skills in-app unified behind an AgentSkillsProvider so agents can select capabilities without custom routing. This matches last week's tool-contract and governed-automation focus: standardized packaging helps you version and audit what agents can do, especially when combined with approvals and allowlists. The post also stresses safeguards: script runners need sandboxing, limits, and audit, and execution can be gated with human approval (.UseScriptApproval(true)) and filtered via allowlists when loading shared directories.

- [Agent Skills in .NET: Three Ways to Author, One Provider to Run Them](https://devblogs.microsoft.com/agent-framework/agent-skills-in-net-three-ways-to-author-one-provider-to-run-them/)

An architecture overview contrasted single-agent (for example, Semantic Kernel orchestrator) with multi-agent (for example, AutoGen specialization), mapping ops implications to Azure building blocks like Service Bus coordination, AKS scaling for specialized agents, and Azure Monitor tracing. The advice is to start single-agent for MVPs and move to multi-agent when specialization and scaling benefits outweigh token cost and observability complexity. That matches last week's day-two complexity lesson.

- [Single Agent vs Multi-Agent Architectures: When Do You Need Each? (Microsoft Stack)](https://dellenny.com/single-agent-vs-multi-agent-architectures-when-do-you-need-each-with-microsoft-technologies-explained/)

A serverless GPU template shows deploying Gemma 4 via Ollama on Azure Container Apps behind Nginx HTTPS + Basic Auth, exposing an OpenAI-compatible /v1/chat/completions endpoint for OpenAI-API-compatible tools (including OpenCode). This matches last week's "cloud + local" and API-compatibility theme: compatibility reduces integration churn across managed endpoints, your containers, and on-device endpoints. The guide includes GPU fit (T4 vs A100), region notes, `azd up` provisioning, and cost control via scale-to-zero.

- [Gemma 4 on Azure Container Apps Serverless GPU](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/gemma-4-on-azure-container-apps-serverless-gpu/ba-p/4511671)

Fabric Real-Time Intelligence previews outlined "agentic ops" productization, echoing last week's operational sequencing (observe -> decide -> automate with guardrails). Operations Agent setup describes a Teams-delivered, LLM-generated "Agent Playbook" running scheduled checks against KQL/Eventhouse, then recommending actions that may require approval and trigger Power Automate flows (three-day recommendation timeout). An expanded Eventhouse preview positions it as a shared KQL landing and analysis surface with a SQL endpoint, notebooks (Python/Spark), built-in anomaly detection, and data-agent integration to keep investigation and automation close to event and time-series data.

- [Microsoft Fabric Operations Agent: Step-by-step setup and runtime behavior](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/microsoft-fabric-operations-agent-step-by-step-walkthrough/ba-p/4512572)
- [One platform, many insights: How Eventhouse brings analytics together (Preview)](https://blog.fabric.microsoft.com/en-US/blog/one-data-many-insights-how-eventhouse-brings-analytics-together-preview/)

A Foundry case study (Agents League winner) provided a multi-agent reference: CertPrep uses Foundry Agent Service with GPT-4o JSON mode and Pydantic-validated contracts between specialized agents, plus concurrency, Azure Content Safety guardrails, and human-in-the-loop gates. It follows last week's inspectable execution thread by surfacing intermediate artifacts (profiles, plans, readiness scoring, assessments, traces) in the UI for review and debugging rather than only producing a final response.

- [Agents League Winner Spotlight: CertPrep Multi-Agent System on Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/agents-league-winner-spotlight-reasoning-agents-track/ba-p/4511211)

## ML

This week's ML-adjacent Fabric updates focused on reducing two workflow frictions: getting local artifacts into OneLake, and moving between SQL, notebooks, and KQL analysis without re-learning each workload UI. Building on last week's "operationalize the platform" theme (safer ingestion, fewer embedded secrets, smoother Warehouse querying), these changes aim to reduce glue work once teams move beyond prototypes.

### Microsoft Fabric: lower-friction ingestion and a more consistent analysis surface

OneLake File Explorer is now GA, addressing a common prototyping need: early datasets and artifacts often start on a developer machine (Excel, CSV, Parquet, images, intermediate outputs). With Windows File Explorer integration, OneLake mounts in Explorer so teams can browse by workspace/item and use standard file operations like drag-and-drop to place files where they belong. In the context of last week's Eventstreams ingestion and security improvements (private networking, Key Vault certs, fewer embedded connection strings), this is a complementary on-ramp: teams can move local artifacts into governed storage without scripts or portal detours. Once in OneLake, data is immediately usable across Fabric experiences (pipelines, notebooks, semantic models) without one-off uploads during iteration.

In preview, Fabric is reducing UI fragmentation with a unified "Analyze data with" entry point across Lakehouse, Data Warehouse, and Eventhouse. This follows last week's "cleaner warehouse SQL" thread: once data is shared in OneLake, friction often shifts to inconsistent compute and query entry points. Eventhouse Endpoint now appears alongside SQL Endpoint and Notebook options so switching modalities is predictable from the same menu. For Lakehouse and Warehouse, enabling Eventhouse Endpoint provisions an Eventhouse and KQL Database as child artifacts with backend-managed schema sync, which provides a KQL surface over the same data without manual sync or duplication. That matches last week's push for managed configuration over bespoke integration. Eventhouse also gets the same menu at the database level (next to Share), and notebook launching is standardized so opening from Eventhouse/KQL Database auto-adds the database to the notebook environment for consistent Spark notebook behavior across workloads.

- [Bring your local files to OneLake with OneLake File Explorer (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/bring-your-local-files-to-onelake-with-onelake-file-explorer-generally-available/)
- [Unifying “Analyze data with” analytics across Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/unifying-analyze-data-with-analytics-across-fabric-preview/)

## Azure

Azure updates this week leaned into operational work: new ingress, backups, and incident-response building blocks for Kubernetes; deeper looks at private DNS and packet visibility; and Fabric progress on migration gaps plus automation hooks. The theme was reducing toil through standard workflows (one-command setups, self-updating CLIs, policy remediation) and more evidence-based troubleshooting and cutovers. It continues last week's "day-two readiness" thread: fewer brittle secrets and manual steps, more controlled transitions (ingress migration clocks, log ingestion deprecations), and clearer acknowledgement that DNS and telemetry wiring often decide reliability.

### AKS platform operations: ingress shifts, backups, and AI-assisted investigations

AKS ingress is moving toward Kubernetes Gateway API, with the AKS App Routing add-on as the entry point. Building on last week's AKS/Istio direction (mesh-aware tracing, evidence-based troubleshooting), this preview reuses an Istio-managed Envoy gateway stack in a *gateway-only* shape: "App Routing Gateway API" replaces the NGINX Ingress API path with an Istio-managed Envoy gateway, explicitly **not** a full mesh (no sidecars, no workload Istio CRDs). The platform model is simpler: creating a `Gateway` auto-provisions the Envoy deployment plus a `LoadBalancer` service, HPA (2-5 replicas, 80% CPU default), and PDB. The split between `GatewayClass`/`Gateway` and `HTTPRoute` also lets platform teams own gateway infrastructure while app teams own routes, which reduces shared-Ingress contention.

The preview is framed against deadlines: Ingress NGINX retirement is **March 2026**, with extended support for NGINX-based AKS App Routing until **November 2026**. Migration guidance includes parallel controllers, manifest conversion via `kubernetes-sigs/ingress2gateway` v1.0.0, and careful cutover steps (Gateway "programmed" condition, `Host` header tests to the new external IP before DNS, lowering DNS TTL early, keeping old ingress 24-48 hours for rollback). The preview is not feature-complete: current App Routing DNS/TLS automation (Azure DNS + Key Vault cert integration) is not available yet in Gateway API mode, so teams need manual TLS/DNS or alternatives like ExternalDNS for Gateway API. That gap matters given last week's "DNS is Tier-0" warning: moving ingress is often easier than moving DNS and TLS plumbing. There is also a strategic constraint: this mode is mutually exclusive with the AKS Istio service mesh add-on, so clusters choose "gateway-only" or full mesh.

AKS backup enablement also gained a more automation-friendly entry point, consistent with last week's emphasis on repeatable baselines. A single command, `az dataprotection enable-backup trigger --datasource-type AzureKubernetesService --datasource-id <aks-arm-id>`, orchestrates validation and setup: backup RG selection/creation, AKS Backup Extension install, storage account provision/reuse, vault/policy provision/reuse, Trusted Access config, and backup instance creation. Presets (Week/Month/DisasterRecovery/Custom) standardize retention defaults while still supporting enterprise wiring via JSON config (existing vault/policy IDs, tags, RG control).

For on-call work, AKS networking troubleshooting is getting more "agentic" but remains evidence-driven. Following last week's "observe first, automate safely" theme, the **Container Network Insights Agent** (public preview) correlates signals across CoreDNS, service routing, NetworkPolicy/CiliumNetworkPolicy, Cilium/Hubble flows, and host kernel telemetry (ring buffers, packet counters, SoftIRQ distribution, socket buffer utilization). It integrates through the AKS MCP server to run diagnostics via `kubectl`, Cilium, and Hubble within defined boundaries, producing an auditable report tied to pass/fail evidence. It is advisory-only (no changes), uses read-only/minimal RBAC, and may deploy a temporary debug DaemonSet for host visibility. Preview regions are limited (Central US, East US, East US 2, UK South, West US 2), and full capability requires Cilium plus Advanced Container Networking Services. Customers also bring their own Azure OpenAI resource for model configuration and residency control.

Finally, AKS migration guidance reiterated a reliability point consistent with last week's broader framing: "it deployed" is not the same as "ready for cutover." Guidance focuses on what breaks under real traffic (memory limits, region configuration gaps, broken bindings to messaging/ingestion, stale endpoint mappings) and treats cutover as a coordinated dependency transition across compute, networking, storage, messaging, analytics connectivity, and background jobs. It also stresses rehearsing DR/failback mechanics (not only DNS reversal) and running smoke tests that exercise real integrations and background workloads under production-like constraints.

- [AKS App Routing's Next Chapter: Gateway API with Istio](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/aks-app-routing-s-next-chapter-gateway-api-with-istio/ba-p/4512729)
- [Announcing One‑Command Backup Configuration for AKS with Azure Backup](https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-one-command-backup-configuration-for-aks-with-azure-backup/ba-p/4511852)
- ['Introducing the Container Network Insights Agent for AKS: Now in Public Preview'](https://techcommunity.microsoft.com/t5/azure-networking-blog/introducing-the-container-network-insights-agent-for-aks-now-in/ba-p/4512197)
- [Production Cutover in Cloud-Native Migrations](https://techcommunity.microsoft.com/t5/azure-migration-and/production-cutover-in-cloud-native-migrations/ba-p/4509924)

### Observability and incident response automation: SRE Agent connectors and log ingestion migrations

Azure SRE Agent got a usability boost for investigations, extending last week's "Azure Monitor in Azure SRE Agent" story from alert ingestion and merging into faster evidence gathering. New first-party connectors for **Log Analytics** and **Application Insights** let the agent run **KQL directly via MCP-backed tools** instead of shelling out to `az monitor`. Setup also handles RBAC when saving the connector (Log Analytics Reader + Monitoring Reader at the target RG), and queries use native monitor-namespace MCP tools like `monitor_workspace_log_query`, `monitor_resource_log_query`, plus discovery helpers. The model stays read-only (no changes to retention/settings) and can use different managed identities per connector, continuing last week's move away from over-permissioned automation identities.

Azure Monitor log ingestion is also moving off the legacy HTTP Data Collector API, similar to this week's ingress retirement clock. With deprecation set for **September 2026**, the outlined migration path is moving Logic Apps from the Data Collector connector (workspace ID/key) to an HTTP action calling **Logs Ingestion API**, backed by **DCEs** and **DCRs**. Practical issues are already showing up: Logic Apps can "succeed" while data does not land in new custom tables, and new Data Collector API connections may fail with 403. The new pattern includes schema definition via sample upload (JSON array), optional `TimeGenerated` via DCR transformation, ingestion URL built from DCE base + DCR immutable ID + stream name, and assigning the Logic App managed identity **Monitoring Metrics Publisher** on the DCR. Success returns 204, which is useful for validating pipelines.

- ['New in Azure SRE Agent: Log Analytics and Application Insights Connectors'](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/new-in-azure-sre-agent-log-analytics-and-application-insights/ba-p/4509649)
- [Migrate Logic Apps from HTTP Data Collector API to Azure Monitor Log Ingestion API](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/migrate-data-ingestion-from-data-collector-to-log-ingestion/ba-p/4510493)

### Azure networking and hybrid connectivity: Private DNS fallback, ExpressRoute to MVE, and packet mirroring

Hybrid and Private Link-heavy designs keep hitting the same DNS failure mode, continuing last week's hub-spoke postmortem: a linked private DNS zone returns authoritative `NXDOMAIN` for a Private Link name when the needed record is missing (DR failovers, partial replication, cross-boundary layouts, multi-region). The fix highlighted is enabling `resolutionPolicy = NxDomainRedirect` on the private DNS zone's VNet link (portal: "Enable fallback to internet"). Azure DNS then retries public recursive resolution only when the private zone returns `NXDOMAIN`, letting apps resolve the public endpoint again when it exists and is reachable. This is a scoped resolution change (not access), but it can prevent partial DNS inconsistency from turning into an outage, especially when public fallback is part of the intended DR posture.

A connectivity walkthrough covers wiring Azure ExpressRoute into **Megaport Virtual Edge (MVE)** with a **Cisco 8000v** NVA. It is configuration-focused: two VXCs (primary/secondary), distinct VLAN IDs per path, matching VLAN between Megaport and ExpressRoute Private Peering, /30s per path, and Cisco IOS subinterfaces with `encapsulation dot1Q <vlan>` plus eBGP neighbor configuration (example Azure ASN 12076). It also highlights validation steps (ICMP to Azure peer IPs) and common troubleshooting (ARP issues).

Azure Virtual Network TAP (VTAP) also got attention as an option when flow logs are not enough, complementing last week's observability guidance. In public preview in select regions, VTAP mirrors full traffic (including payload) for selected NICs and sends it to a collector using VXLAN over UDP **4789**. The demo shows Wireshark decoding encapsulated flows and notes that the destination NIC can be in the same or a peered VNet, which can help centralize inspection tooling away from application subnets.

- [Enabling fallback to internet for Azure Private DNS Zones in hybrid architectures](https://techcommunity.microsoft.com/t5/azure-networking-blog/enabling-fallback-to-internet-for-azure-private-dns-zones-in/ba-p/4511131)
- [Connecting an ExpressRoute circuit to Megaport Virtual Edge](https://techcommunity.microsoft.com/t5/azure-networking-blog/connecting-an-expressroute-circuit-to-megaport-virtual-edge/ba-p/4510770)
- [A demonstration of Virtual Network TAP](https://techcommunity.microsoft.com/t5/azure-networking-blog/a-demonstration-of-virtual-network-tap/ba-p/4479136)

### Data services: Fabric schema safety, migration parity, and the next wave of PostgreSQL

Fabric's data plane continues closing gaps that show up in CI/CD and migrations, building on last week's themes around modernizing without rewrites and reducing glue code. A practical GA update in Fabric Data Warehouse is that some `ALTER TABLE` operations now work inside **explicit transactions** (`BEGIN TRAN ... COMMIT`). Previously, `ALTER TABLE` under snapshot isolation failed, which forced non-atomic schema changes and increased partial-deploy risk. Supported operations include adding nullable columns, dropping columns, adding/dropping NOT ENFORCED constraints (PK/UNIQUE/FK), multiple `ALTER TABLE` statements in one transaction, and altering distributed temp tables. Exclusions include adding non-nullable columns and `ALTER COLUMN`.

Fabric SQL Database improved migration compatibility with a preview: full support for Azure SQL Database collation sets at database creation time. It is configured in the **creation payload** (`NewSqlDatabaseCreationPayload`) via the Fabric REST API (and wrappers like Fabric CLI/PowerShell). This reduces surprises for multilingual and collation-sensitive workloads (ORDER BY, LIKE, equality, case/accent sensitivity), though it does not change collation for replicated data in the SQL analytics endpoint.

Fabric Data Factory guidance focused on when to move from Azure Data Factory and what changes for developers. The stance is incremental: ADF remains supported, but new work lands in Fabric Data Factory's SaaS authoring and workspace model. Differentiators include Fabric-native **Mirroring** into OneLake for low-latency replication (continuous inserts/updates/deletes) and **Copy Jobs** for config-first bulk and incremental movement (watermarking, CDC, built-in SCD Type 2). For pro-dev flows, managed **Airflow Jobs** and **dbt Jobs** are first-class alongside pipelines and Dataflows Gen2, with an AI integration thread via MCP (Copy Jobs exposed as MCP endpoints and the open-source `microsoft/DataFactory.MCP` server). This mirrors what is happening in operations (AKS, SRE Agent): standardized tool interfaces with guardrails and clearer auditability.

Managed PostgreSQL messaging also hints at a split between "run Postgres well today" and "what's next." One video covers practical Azure Database for PostgreSQL mechanics (HA/failover, read replicas, backup/restore, elastic clusters) plus cost/perf notes like AMD SKUs. Another "sneak peek" introduces **Azure HorizonDB**, a managed PostgreSQL option aimed at very large scale with decoupled compute and storage, replica scaling over shared storage, and multi-zone commit behavior. It is also positioned as "AI-native," with vector indexing and SQL AI functions plus Azure AI Foundry integration and VS Code-centric provisioning/query adaptation.

- [ALTER TABLE inside explicit transactions in Fabric Data Warehouse (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/alter-table-inside-explicit-transactions-in-fabric-data-warehouse-generally-available/)
- [Announcing Full Collation Support for SQL Databases in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.microsoft.com/en-US/blog/announcing-full-collation-support-for-sql-databases-in-microsoft-fabric-preview/)
- [Answers to common questions about Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/answers-to-common-questions-about-fabric-data-factory/)
- ['PostgreSQL Like a Pro: Performance and resilience with Azure Database for PostgreSQL'](https://www.youtube.com/watch?v=jZHWptNT71I)
- ['PostgreSQL Like a Pro: Build mission-critical apps at any scale with Azure HorizonDB'](https://www.youtube.com/watch?v=-iG5a0O5_wk)

### App and messaging services: Web PubSub wildcard roles and Service Bus request/reply scaling

Azure Web PubSub expanded auth in a way that matters for high-cardinality group patterns and matches last week's least-privilege and identity direction. **Wildcard group roles** let backends grant permissions like `webpubsub.joinLeaveGroups.room/*` and `webpubsub.sendToGroups.room/*` instead of issuing tokens with long lists of per-group roles (for example, repeating `webpubsub.sendToGroup.room123`). This reduces token size and simplifies issuance logic for bots and monitoring systems that need broad access across dynamic group namespaces. Guidance is practical: keep literal roles for strict end-user isolation, and use wildcard roles when broad access is intentional and audited.

A Service Bus architecture write-up addressed a scaling trap in sync-over-async gateways: using **Service Bus Sessions** for request/reply correlation can create instance affinity because one gateway instance holds the session lock and must receive the reply. An alternative keeps gateways stateless by routing replies through a topic with **SQL Filter** subscriptions on a custom property like `CorrelationId`. Each request creates a dynamic subscription matching that value; the worker replies to a shared topic with the property; the broker delivers to the right gateway instance without session locks. The trade-off is managing dynamic subscriptions, but it is packaged as a Spring Boot starter for Java gateway teams. It also fits last week's broader Service Bus evolution: safer defaults and boundary controls, plus application patterns that avoid hidden scaling ceilings.

- ['Introducing Wildcard Roles in Azure Web PubSub: simpler, smarter permissions for real-time apps'](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-wildcard-roles-in-azure-web-pubsub-simpler-smarter/ba-p/4509524)
- [Scaling Sync-over-Async Edge Gateways by Bypassing Azure Service Bus Sessions](https://techcommunity.microsoft.com/t5/azure-architecture/architecture-pattern-scaling-sync-over-async-edge-gateways-by/m-p/4510919#M832)

### Other Azure News

Azure Virtual Desktop updates mixed new support with production lessons. App attach now supports Windows Server 2025 and Windows Server 2022 session hosts, extending dynamic app delivery (MSIX/AppX/App-V) to server-based pools and helping reduce golden-image sprawl as App-V Server components approach end of support (April 2026). A real-world AVD deployment in the Perth Azure Extended Zone showed the engineering behind private-only, GPU-backed personal desktops (NVadsA10 v5): IaC-driven Azure Image Builder, Compute Gallery replication where builds stay in the parent region, and a one-time REST API step to associate a user-assigned managed identity to the gallery for Extended Zone replication when portal support lags. That managed identity detail matches last week's push toward clearer scoping and auditing for identities. It also included cost control via IMDS + Azure Automation webhook ("Stop My VDI") so users can deallocate without portal access, paired with "Start VM on Connect" RBAC.

Operationally, there was a reminder that Azure Run Command lets you run commands across VM Scale Set instances without RDP/SSH, aligning with last week's "standardize the baseline, reduce snowflake access" theme. Constraints still apply (VM Agent ready, outbound 443 for results, 4096-byte output limit, one run at a time per VM, 90-minute max). VMSS mode matters: Uniform supports `az vmss run-command invoke` by instance ID, while Flexible typically requires iterating VMs and calling `az vm run-command invoke`.

Hybrid SQL governance automation appeared via an Azure Policy (DeployIfNotExists) pattern enforcing Arc-enabled SQL Server extension `LicenseType` ("Paid", "PAYG", "LicenseOnly"), plus scripts for assignments and remediation across management groups/subscriptions. This aligns with last week's Arc least-privilege onboarding and the broader move from tickets and tribal knowledge to repeatable policy. PAYG has a caveat: policy sets `ConsentToRecurringPAYG`, and once set it cannot be removed even if you switch away, so consent is effectively one-way.

Azure Developer CLI got a small but useful update consistent with last week's `azd` reproducibility theme: `azd update` (azd 1.23.x) updates regardless of install method (winget/Chocolatey/Homebrew/script), and supports switching stable vs daily via `--channel`.

The broader update feed and cost content emphasized operational planning: mentions included StandardV2 NAT Gateway for AKS outbound, Azure Monitor OpenTelemetry for AKS, Bastion MI graphical session recording, ASR NVMe controller support, storage security/tiering changes, and retirements (Azure Batch HBv2/HC/NP; Azure Managed Grafana Basic). Cost guidance reiterated that AI-heavy cost optimization needs continuous visibility, guardrails, rightsizing, and recurring reviews, consistent with last week's FinOps tone. Fabric Eventhouse added a preview Capacity Scheduler for hourly minimum capacity baselines across a 7-day grid while keeping autoscale, to align predictable ingestion/query windows with cost control.

- [App attach in Azure Virtual Desktop now supports Windows Server 2025 and Windows Server 2022](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/app-attach-in-azure-virtual-desktop-now-supports-windows-server/ba-p/4511729)
- ['Azure Virtual Desktop in the Perth Azure Extended Zone: A Real‑World Production Deployment'](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/azure-virtual-desktop-in-the-perth-azure-extended-zone-a-real/ba-p/4508550)
- [Running Commands Across VM Scale Set Instances Without RDP/SSH Using Azure CLI Run Command](https://techcommunity.microsoft.com/t5/azure/running-commands-across-vm-scale-set-instances-without-rdp-ssh/m-p/4511577#M22490)
- [Automating Arc-enabled SQL Server license type configuration with Azure Policy](https://techcommunity.microsoft.com/t5/azure-arc-blog/automating-arc-enabled-sql-server-license-type-configuration/ba-p/4500326)
- [SQL Server enabled by Azure Arc Overview](https://techcommunity.microsoft.com/t5/azure-arc-blog/sql-server-enabled-by-azure-arc-overview/ba-p/4496399)
- [Stop juggling package managers—just run `azd update`](https://devblogs.microsoft.com/azure-sdk/azd-update/)
- [Azure Update 16th April 2026](https://www.youtube.com/watch?v=WjvN_XjMr6U)
- ['Cloud Cost Optimization: Principles that still matter'](https://azure.microsoft.com/en-us/blog/cloud-cost-optimization-principles-that-still-matter/)
- ['Azure Cost Estimation: Navigate Database Pricing'](https://www.youtube.com/watch?v=fZVa-kYJ2i8)
- ['Capacity Scheduler: Smarter capacity control for Eventhouse (Preview)'](https://blog.fabric.microsoft.com/en-US/blog/capacity-scheduler-smarter-capacity-control-for-eventhouse-preview/)
- [Building a Scalable IoT Platform for Facility Management with Azure Serverless Services](https://techcommunity.microsoft.com/t5/azure-architecture-blog/building-a-scalable-iot-platform-for-facility-management-with/ba-p/4495263)

## .NET

This week's .NET updates split between moving forward and staying current. .NET 11 Preview 3 shipped runtime/SDK/library/framework updates aimed at everyday development, while April 2026 servicing releases delivered security fixes across supported .NET and .NET Framework versions. Building on last week's .NET 11 direction-setting items (like Blazor validation previews), this is another preview step you can install and test, alongside reminders to keep production stacks patched. Microsoft also set a deadline for an "ASP.NET Core on .NET Framework" escape hatch, pushing teams toward modern .NET for web workloads.

### .NET 11 Preview 3: runtime, SDK/CLI, web/WASM, MAUI, EF Core, and containers

.NET 11 Preview 3 updates the runtime, SDK/CLI, BCL, C#, ASP.NET Core, .NET MAUI, EF Core, and official container images, with emphasis on performance and faster inner-loop iteration. Following last week's focus on evolving app workflows (for example, Blazor validation), Preview 3 spreads incremental improvements across the stack.

Runtime/JIT changes include optimizations for switch statements, bounds checks, and casts to reduce hot-path overhead without code changes. It also makes "runtime async" available without preview-API opt-in, which reduces friction for teams evaluating async runtime capabilities during previews.

BCL updates mix control and safety. System.Text.Json adds controls for naming and ignore-default handling to reduce the need for custom converters. Compression expands: Zstandard (zstd) support moves into System.IO.Compression, and ZIP reading adds CRC32 validation to surface corrupt archives earlier. Lower-level IO improvements expand pipe support via SafeFileHandle and RandomAccess for infra and interop-heavy code. One behavior change is that Regex now recognizes all Unicode newline sequences, which can change matches on inputs that contain non-ASCII newlines (relevant for cross-platform parsing).

SDK/CLI improvements target large repos and fast iteration. You can edit solution filters from the CLI, which helps in monorepos and focused builds. File-based apps can span multiple files, making script-like prototypes easier to organize. `dotnet run` adds `-e` for setting environment variables directly. `dotnet watch` adds Aspire support, crash recovery, and Windows desktop improvements, aiming for more resilient hot reload and watch workflows across cloud-native and desktop apps. This pairs with last week's pipeline and machine operational notes: teams will notice these changes in local iteration and automation.

For web and browser, ASP.NET Core adds Zstandard response compression and request decompression as an alternative to gzip/brotli. Blazor `Virtualize` can adapt to variable-height items at runtime, reducing jank when item sizes vary and continuing last week's thread of making common UI patterns require less custom code. HTTP/3 processing starts earlier in the pipeline to reduce latency and overhead. Browser/WASM updates add WebCIL support and debugging improvements, affecting packaging and developer experience for Blazor WebAssembly and other .NET-in-browser workloads.

C# previews experimental union types support for discriminated-union-style modeling. .NET MAUI updates include Maps improvements (clustering, styling, richer APIs), XAML/styling tweaks for startup and iteration, and a built-in `LongPressGestureRecognizer`. .NET for Android adds Android 17 / API 37 preview support to validate upcoming platform changes.

EF Core adds performance and configuration controls. `ChangeTracker.GetEntriesForState()` avoids extra change detection when inspecting tracked state. DbContext config can remove providers and add pooled factories for more flexible DI/provider/pooling combinations. Migrations get more control and clearer feedback, and query generation removes unnecessary joins in some cases. SQL Server provider support adds JSON APIs for teams using JSON-centric schema patterns.

Supply-chain hardening shows up in containers: official .NET container images are now signed, improving provenance for CI/CD policies that require signatures. Read alongside last week's PowerShell installer transition warning, it is another reminder that build and deploy inputs (base images, installers, agent tooling) matter as much as application code. Preview 3 guidance points to installing the .NET 11 SDK Preview 3 and using Visual Studio 2026 Insiders on Windows, or VS Code with C# Dev Kit.

- [.NET 11 Preview 3 is now available!](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-3/)

### Servicing and support timelines: April 2026 patches and a deadline for ASP.NET Core 2.3 on .NET Framework

Microsoft shipped April 14, 2026 servicing updates for supported .NET and .NET Framework versions, covering security and non-security fixes with links to release notes, installers/binaries, MCR container images, Linux package guidance, and known issues. After last week's mix of preview features and pipeline-affecting policy shifts, this reinforces the "do not fall behind" track: servicing updates are the immediate production task, and previews are for what comes next.

Patched .NET releases are .NET 10.0.6, .NET 9.0.15, and .NET 8.0.26, with pointers to GitHub release notes and milestone/changelog queries for ASP.NET Core, EF Core, the runtime, and WinForms. For teams running multiple app types, those links help you audit changes in the parts you ship.

Security-wise, the post lists multiple CVEs across .NET and .NET Framework, including denial of service, security feature bypass, and remote code execution (affecting combinations of .NET 10/9/8 and multiple .NET Framework versions). Practically, patching means more than updating dev SDKs: rebuild and redeploy containers on updated MCR base images, update build agents, and pull forward Linux package installs, while checking known issues before broad rollout.

Microsoft also set an end-of-support date for ASP.NET Core 2.3 on .NET Framework: April 7, 2027. After that, it gets no security patches, bug fixes, or support, creating a deadline for orgs that adopted ASP.NET Core but stayed on .NET Framework. The recommended path is modern ASP.NET on .NET 10, aligning with ongoing runtime and framework improvements and enabling cross-platform hosting. Context matters: ASP.NET Core 3.0 dropped .NET Framework support in 2019, and ASP.NET Core 2.3 (early 2025) was a servicing-oriented baseline for Framework users, effectively a re-release of 2.1. In some cases, 2.1 -> 2.3 behaved more like a compatibility tradeoff than a simple bump. With install stats showing these packages still widely used, the EoS date is likely to surface in backlogs soon, especially for stable Windows/IIS apps on long timelines.

- [.NET and .NET Framework April 2026 servicing releases updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-april-2026-servicing-updates/)
- [Microsoft calls time on ASP.NET Core 2.3 on .NET Framework](https://www.devclass.com/devops/2026/04/13/microsoft-calls-time-on-aspnet-core-23-on-net-framework/5216962)

## DevOps

This week's DevOps updates clustered around tighter delivery mechanics (review, shipping, remote work) and more guardrails as automation and agents approach production workflows. GitHub and Azure DevOps shipped reliability and governance updates, while VS Code and Docker continued turning agent-driven work into something more isolated, auditable, and less disruptive to your main working copy.

### AI agents in developer and ops workflows (VS Code + Docker)

Running agents safely is becoming more practical day to day, with focus on isolation and repeatability rather than only chat. After last week's Docker Sandbox introduction, this week's follow-on focused on avoiding repeated setup. Andrew Lock dug into Docker Sandboxes (microVM environments launched via `sbx`) and how to avoid reinstalling toolchains for each agent session. The key is publishing your own sandbox *template images* (OCI images) to a registry (for example, Docker Hub) and referencing them by full name (for example, `sbx run -t docker.io/my-org/my-template:v1 claude`). Because sandboxes do not share your local Docker image store, pushing to a registry is required so sandboxes can pull and cache templates.

The guide stays operational: extending `docker/sandbox-templates:claude-code-docker` (Ubuntu-based) with extra tools, including a .NET example that installs OS packages as `root` but installs user-scoped tooling as non-root `agent` (for example, `dotnet-install.sh --channel 10.0 --no-path`, then `DOTNET_ROOT` and `PATH` to `/home/agent/.dotnet` and `/home/agent/.dotnet/tools`). It also covers minimal variants, starting Docker inside the sandbox via `LABEL com.docker.sandboxes.start-docker=true`, and multi-stage builds so updating Claude Code does not force a full toolchain rebuild (for example, `--no-cache-filter claude`).

VS Code's agent story continued the arc from last week, moving from UI polish to controls that keep agent work contained and accountable. VS Code 1.117 (Insiders) refined session behavior: Autopilot permission mode can persist across sessions, and `chat.permissions.default` lets teams set default permission levels. Agent Host added configurable auto-approvals (including "Bypass Approvals" and "Autopilot (Preview)"), and Agent Host Protocol added support for "subagents" and "agent teams," which signals preparation for multi-agent patterns. For DevOps hygiene, Agent Host sessions can use worktree/Git isolation so agent work does not pollute your main working directory, turning last week's manual safety pattern into an editor workflow.

Terminal execution also tightened: when an agent sends input to a terminal, VS Code now captures terminal output automatically after a short delay, removing extra back-and-forth. Shell recognition now includes Copilot CLI, Claude Code, and Gemini CLI, and Copilot CLI worktrees get more meaningful branch names based on the user prompt. A companion video focused on terminal tools: foreground terminal support (visible/interruptible), better interactive prompt handling, clearer progress for long commands, and smarter notifications so you do not miss prompts while multitasking.

- [Running AI agents with customized templates using docker sandbox](https://andrewlock.net/running-ai-agents-with-customized-templates-in-docker-sandbox/)
- [Visual Studio Code 1.117 (April 2026) release notes](https://code.visualstudio.com/updates/v1_117)
- [VS Code Terminal Agent Tools](https://www.youtube.com/watch?v=0Eq8m63Z5J0)

### GitHub workflow and governance updates (Stacked PRs, rulesets, status transparency)

GitHub changes landed across review workflow, governance monitoring, and outage interpretation. The thread from last week remains consistent: as automation volume rises, GitHub is adding guardrails, visibility, and reliability signals.

GitHub entered private preview for **Stacked PRs**, bringing stacked-diffs workflows into pull requests. The goal is to make "keep PRs small" workable without blocking progress: PRs can be based on other PRs, forming a stack where review stays granular and merge order is enforced (a PR cannot merge until those below merge). GitHub also supports merging an approved stack at once. An optional CLI extension, **`gh stack`** (https://github.github.com/gh-stack/), helps manage stacks and supports AI-agent-friendly workflows that generate and update chains of dependent PRs. This matches last week's theme of keeping queues usable as bot and agent activity increases: smaller diffs reduce review load, and tooling reduces fragility when automation authors changes.

GitHub also added a **Rule insights dashboard** under *Repository Settings -> Rules* for repos using rulesets. It summarizes evaluation activity over time (successes, failures, bypasses) and shows "most active bypassers," with charts linking to filtered detailed views for incident and audit workflows. GitHub also replaced multiple bespoke filtering UIs with a **unified filter bar** across code scanning alert dismissal requests, Dependabot alert dismissal requests, secret scanning alert dismissals, and secret scanning push protection bypass requests at enterprise/org/repo scopes. It supports filtering via custom properties. This continues last week's "tighter guardrails, better triage" theme as policy enforcement and exception handling become daily operations.

GitHub updated its status page to support clearer incident interpretation: a new "Degraded Performance" state, per-service 90-day uptime percentages, and a dedicated Copilot component ("Copilot AI Model Providers"). This matches last week's availability report takeaway where delays can be a distinct failure mode, and it adds vocabulary for "up but slow" while mapping to pipeline SLAs. The uptime math details matter for SLO/vendor-risk discussions: "Major Outage" counts as 100% downtime, "Partial Outage" as 30%, and "Degraded Performance" as 0% downtime (service considered functional), which changes how published uptime compares to internal telemetry.

- [GitHub invokes spirit of Phabricator with preview of Stacked PRs](https://www.devclass.com/development/2026/04/16/github-invokes-spirit-of-phabricator-with-preview-of-stacked-prs/5217921)
- [Rule insights dashboard and unified filter bar](https://github.blog/changelog/2026-04-16-rule-insights-dashboard-and-unified-filter-bar)
- [Bringing more transparency to GitHub’s status page](https://github.blog/news-insights/company-news/bringing-more-transparency-to-githubs-status-page/)

### Azure SRE Agent automation for AKS incidents and IaC drift

Azure SRE Agent guidance leaned into closed-loop ops: trigger from alerts or drift, investigate under governance, apply scoped fixes (optionally autonomously), verify recovery, and leave durable follow-up in GitHub/Teams. Building on last week's safety framing (autonomy levels, RBAC constraints, approval checkpoints, MCP/Python extensibility), these walkthroughs show governance wired end-to-end from real triggers into Azure remediation and back into source control.

In the AKS incident-response walkthrough, safety comes from Azure RBAC + scoped identities + execution modes (Review vs Privileged vs Autonomous), not prompt wording. An Azure Monitor alert (Action Group webhook) triggers the agent, which uses Log Analytics/Kusto, Azure Resource Graph, Azure CLI/ARM, and `kubectl` to diagnose, remediate, and verify.

Two failure modes make it concrete. For CPU starvation, workloads are deployed with very low CPU/memory (requests `cpu: 1m`, limits `cpu: 5m`; memory `6Mi/20Mi`), causing startup probe failures because the process cannot bind in time. The agent uses pod status and exit codes (exit code `1`, not `137`, to rule out OOMKill), finds CPU-throttled pods via `kubectl top`, patches CPU across workloads, and verifies recovery (healthy pods, zero restarts). For OOMKilled, it uses exit code `137`, empty logs, and baseline memory (~50Mi) to justify raising limits from `20Mi` to `128Mi` (and requests `10Mi` to `50Mi`), then verifies stabilization via utilization and restarts. Aftercare is built in: Teams gets milestone updates, and the agent can open GitHub issues and draft PRs so hotfixes are reconciled into source-controlled manifests. That matches last week's emphasis on leaving an artifact trail for post-incident review.

The drift-detection walkthrough applies the same model to Terraform. Terraform Cloud (or another drift system) sends a webhook to an Azure Logic App, which uses Managed Identity to get an Entra ID token and forwards an authenticated request to an Azure SRE Agent HTTP Trigger endpoint. The agent correlates drift diffs with Azure Activity Log and Application Insights, classifies drift as Benign/Risky/Critical, and can recommend not reverting drift if it is mitigating an incident. This continues last week's drift-gates mindset: detect mismatch, but turn it into a governed decision with context (who changed what, why, and what it is doing now).

The demo scenario shows why correlation matters. An App Service on B1 has latency spikes and 502s from a blocking `/api/data`; during mitigation, an engineer changes infra in the portal by adding tags (benign), downgrading TLS 1.2 to TLS 1.0 (risky), and scaling B1 -> S1 (critical cost). Drift triggers later, and the agent recommends reverting TLS immediately, reverting tags anytime, and delaying SKU revert until the performance issue is fixed because scaling is mitigating the incident. It also captures actor context from Activity Log and posts a severity-coded drift table and ordered plan into Teams, with optional GitHub PR follow-up.

- ['Autonomous AKS Incident Response with Azure SRE Agent: From Alert to Verified Recovery in Minutes'](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/autonomous-aks-incident-response-with-azure-sre-agent-from-alert/ba-p/4511343)
- ['Event-Driven IaC Operations with Azure SRE Agent: Terraform Drift Detection via HTTP Triggers'](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/event-driven-iac-operations-with-azure-sre-agent-terraform-drift/ba-p/4512233)

### Other DevOps News

Azure DevOps Server Patch 3 (April 14, 2026) included fixes that affect everyday repo/integration reliability: a null reference that could break PR completion during work item auto-completion, improved sign-out redirect validation to reduce open redirect risk, and a fix for PAT-based connections to GitHub Enterprise Server. Microsoft also included a way to verify install via the patch installer's `CheckInstall` argument.

- [April Patches for Azure DevOps Server](https://devblogs.microsoft.com/devops/april-patches-for-azure-devops-server/)

VS Code Remote Tunnels were highlighted as an alternative to RDP for locked-down customer VMs, matching last week's "safer dev workflows" theme. The guide walks through running `code tunnel` on the remote VM (installing VS Code Server components and creating an outbound tunnel via Microsoft Dev Tunnels), then attaching from local VS Code or `vscode.dev` without inbound SSH. It also notes constraints (single-user, customer policy limits on GitHub/Microsoft auth) and keeping tunnels alive via service mode (`code tunnel service install`).

- ['Stop Coding Through Remote Desktop: Use VS Code Remote Tunnels Instead'](https://dev.to/playfulprogramming/stop-coding-through-remote-desktop-use-vs-code-remote-tunnels-instead-37oc)

GitHub Pages onboarding content (blog + video) emphasized a key operational choice: deploy from a branch for simple static sites, or use GitHub Actions when you need a build (for example, Next.js). In the context of last week's reliability/fallback theme, it is a reminder to treat Pages pipelines like any delivery surface: know whether you depend on Actions and what "degraded" modes mean. Both also cover common production steps (custom domain, DNS verification, "Enforce HTTPS") and note Pages sites are public even if the repo is private.

- ['GitHub for Beginners: Getting started with GitHub Pages'](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-pages/)
- [Getting started with GitHub Pages for beginners | Tutorial](https://www.youtube.com/watch?v=b2r9Cdvssi0)

SSMS 22.5 added SQL projects support, aiming to make schema-as-code more accessible starting from an existing database. The workflow is importing a database into a SQL project, editing and validating changes, and publishing in a controlled way, reusing the same project artifact across SSMS, VS Code, GitHub Actions, and Azure DevOps pipelines.

- [Introducing SQL projects in SSMS (SSMS 22.5) | Data Exposed](https://www.youtube.com/watch?v=HE0t7IpOsuM)

A GitHub-based architecture-as-code workflow outlined a repo structure for ADRs, diagram-as-code (Mermaid/PlantUML/C4), standards, reference architectures, and roadmaps, governed via PRs and CI checks to reduce doc drift. With this week's Stacked PRs preview and last week's GitHub UX/triage refinements, the direction is consistent: more "non-code" work is moving into PR-governed, policy-enforced workflows.

- ['From Diagrams to Decisions: Using GitHub to Power Modern Solution Architecture'](https://dellenny.com/from-diagrams-to-decisions-using-github-to-power-modern-solution-architecture/)

GitHub shared a deployment-safety pattern using eBPF + cgroups to apply per-process network controls to deployment tooling so rollouts do not accidentally depend on github.com during outages. This extends last week's lesson about availability and fallbacks: engineer deployment systems to avoid circular dependencies, not only monitor them. The write-up covers CGROUP_SKB egress enforcement and a domain-centric approach intercepting DNS via CGROUP_SOCK_ADDR routed through a local proxy, plus attribution to map blocked lookups back to PID/command line for actionable logs.

- [How GitHub uses eBPF to improve deployment safety](https://github.blog/engineering/infrastructure/how-github-uses-ebpf-to-improve-deployment-safety/)

## Security

This week's security updates focused on making controls easier to apply consistently at scale across GitHub and Azure DevOps, while threat research highlighted how attackers abuse collaboration tools and OS-native scripting. The broader direction continues toward identity-first access (OIDC, Workload Identity, Entra) to remove long-lived secrets, plus guidance for AI incident response and cryptographic readiness. It continues last week's theme: reduce ambient privilege, tighten trust boundaries, and make secure defaults workable, whether through tokenless CI/CD, org-wide scanning baselines, or faster containment when users are socially engineered into granting access.

### GitHub supply chain and code security: OIDC, registries, SBOMs, and better triage mechanics

GitHub security platform changes reduce per-repo one-off configuration and improve org-wide operations. Building on last week's workload identity direction (for example, npm Trusted Publishing OIDC expansion), orgs can now configure **multiple private registries per ecosystem** (npm, Maven, NuGet, Docker, pip, RubyGems, and others) at the **organization security settings** level. This removes the previous "one registry per ecosystem" limit and reduces repo-by-repo workarounds. GitHub also added **OIDC auth for org-level private registries** (via REST API too), enabling Dependabot and code scanning dependency resolution to use **short-lived federated credentials** instead of stored secrets. Initial integrations include **Azure DevOps Artifacts, AWS CodeArtifact, and JFrog Artifactory**. Together, these changes match last week's "operate it well" direction: fewer secrets and less per-repo drift.

Code scanning moved closer to normal workflow with a new **public preview** that lets teams **link code scanning alerts to GitHub Issues**, with bidirectional navigation (alert "Tracking" section, issue "Security alerts" section). New `has:tracking` and `no:tracking` filters (including in Security Campaigns) help enforce hygiene such as "every actionable alert has a work item" without building custom dashboards.

**SBOM exports** from Dependency Graph are now **asynchronous**, which avoids the previous 10-second timeout on large repos. API users must switch to a two-step flow: `GET /repos/{owner}/{repo}/dependency-graph/sbom/generate-report` returns a `{sbom-uuid}`, then poll `GET /repos/{owner}/{repo}/dependency-graph/sbom/fetch-report/{sbom-uuid}` (HTTP **201** while processing, HTTP **302** redirect when ready). This matters for CI/inventory pipelines that previously retried and triggered duplicate backend work, matching last week's push toward more predictable automation at scale.

Detection and prioritization also improved. **CodeQL 2.25.2** adds **Kotlin support up to 2.3.20**, reduces false positives in Java/Kotlin, C/C++, and C#, and updates **@security-severity** scoring. Notably, multiple **XSS queries moved from 6.1 (medium) to 7.8 (high)** across languages, and several log injection queries dropped from high to medium (Rust log injection rose from low to medium). If you gate builds or triage by severity, scoring shifts can reorder backlogs. This pairs with last week's org-wide reporting message: reprioritization helps only if teams notice it.

GitHub also continues emphasizing what is actually running, extending last week's runtime-context integration (Dynatrace). Repository properties now include **`deployable`** and **`deployed`** signals from artifact/deployment metadata, which can help target **rulesets and branch protection** to repos that ship. Dependabot and code scanning alerts now show **runtime risk context** on alert pages so teams can treat "in prod" differently than "dormant."

- [Dependabot and code scanning: Org-level private registries](https://github.blog/changelog/2026-04-14-dependabot-and-code-scanning-org-level-private-registries)
- [OIDC support for Dependabot and code scanning](https://github.blog/changelog/2026-04-14-oidc-support-for-dependabot-and-code-scanning)
- [Link code scanning alerts to GitHub Issues (public preview)](https://github.blog/changelog/2026-04-14-link-code-scanning-alerts-to-github-issues)
- [SBOM exports are now computed asynchronously](https://github.blog/changelog/2026-04-14-sbom-exports-are-now-computed-asynchronously)
- [CodeQL 2.25.2 adds Kotlin 2.3.20 support and other updates](https://github.blog/changelog/2026-04-15-codeql-2-25-2-adds-kotlin-2-3-20-support-and-other-updates)
- [Deployment context in repository properties and alerts](https://github.blog/changelog/2026-04-14-deployment-context-in-repository-properties-and-alerts)

### GitHub secret scanning and rapid exposure baselining

Secret scanning updates combined new detections, tighter enforcement (notably for fork-heavy enterprises), and API improvements for large-scale automation. This continues last week's operational thread around secret scanning APIs/webhooks and delegated visibility, with an emphasis on consistent automation and audit trails instead of manual chasing.

Detection expanded with **Cloudflare** partner patterns: `cloudflare_account_api_token`, `cloudflare_global_user_api_key`, `cloudflare_user_api_token`. Push protection defaults expanded too: when secret scanning is enabled (including free public repos), additional patterns now **block commits by default**, including Cloudflare tokens and types like `figma_scim_token`, `google_gcp_api_key_bound_service_account`, `langsmith_license_key`, `langsmith_scim_bearer_token`, `openvsx_access_token`, and `posthog_personal_api_key`. This reinforces last week's "default guardrails" approach where default-on reduces reliance on repo owners remembering settings.

For **Enterprise Managed Users (EMU)**, push protection now follows the **fork ancestor chain**: if any repo in the hierarchy has push protection enabled, forks inherit it. This closes a gap where secrets could leak through forks created outside licensed contexts.

REST API improvements support internal tooling at scale:
- Custom pattern alert `validity` can now be set via `PATCH /repos/{owner}/{repo}/secret-scanning/alerts/{alert_number}` (`active`, `inactive`, or `null`), and overrides appear in UI, webhooks, and audit logs.
- Alert list APIs add `provider` and `provider_slug`, plus `providers` / `exclude_providers` filters (mutually exclusive; both returns HTTP 422), enabling routing by issuer.
- Scan history adds AI-powered generic secret backfill via `generic_secrets_backfill_scans`.
- Enterprise reporting adds `GET /enterprises/{enterprise}/dismissal-requests/secret-scanning` for centralized dismissal-request oversight.

GitHub also introduced a free **Code Security Risk Assessment** for org admins/security managers on GitHub Enterprise Cloud and GitHub Team. It runs a one-click CodeQL scan across up to **20 most active repos**, shows results by severity/language/rule, highlights most vulnerable repos, and notes **Copilot Autofix eligibility** to connect discovery to remediation. GitHub says Actions minutes **don't count against** org quota, which reduces baseline cost. This builds on last week's org-level risk assessment reporting by making it easier to start with a baseline and manage remediation via reporting/tracking rather than spreadsheets.

- [Secret scanning pattern updates and product improvements](https://github.blog/changelog/2026-04-14-secret-scanning-pattern-updates-and-product-improvements)
- [How exposed is your code? Find out in minutes—for free](https://github.blog/security/application-security/how-exposed-is-your-code-find-out-in-minutes-for-free/)

### Azure DevOps Advanced Security: default CodeQL setup and org-wide alert coordination

Azure DevOps Advanced Security focused on making CodeQL adoption and triage less pipeline-centric and more centrally managed. In **public preview**, **CodeQL default setup** provides **one-click enablement** at repo/project/org scope, removing the need to author and maintain Azure Pipelines YAML, install CodeQL tasks, wire builds, and keep configuration current. With default setup, scans run automatically on a schedule adjustable at org level. This mirrors GitHub's direction: centralized controls that are easier to audit and harder to forget.

A key control is setting which **agent pool** runs scanning jobs via org-level repo settings, which helps with network/compliance boundaries and capacity management.

The updated **combined alerts** experience in Security Overview adds an **org-wide view** (default branch across repos) and **security campaigns**: shareable, filtered alert views that stay live as new matching findings arrive. This matches last week's org-level reporting theme (and this week's GitHub tracking mechanics): central visibility plus consistent workflows is what makes "fix across the estate" feasible when queries or CVEs reshuffle priorities.

- [One-click security scanning and org-wide alert triage come to Advanced Security](https://devblogs.microsoft.com/devops/one-click-security-scanning-and-org-wide-alert-triage-come-to-advanced-security/)

### Threat research: social engineering via Teams/Quick Assist and macOS AppleScript tradecraft

Two research writeups reinforced a recurring defender reality: attackers abuse legitimate workflows (collaboration, remote help, scripting) and then move quickly into hands-on-keyboard activity using native tooling to blend in. This extends last week's token-focused attack coverage (AiTM, device-code phishing): compromise often comes from trusted UX where users authorize access rather than from password theft.

Microsoft documented a human-operated intrusion chain starting with **cross-tenant Microsoft Teams helpdesk impersonation**, quickly turning into recon, persistence, lateral movement, and exfiltration. The actor convinces a user to accept Teams contact and approve remote assistance (often **Quick Assist**), then within **30-120 seconds** runs cmd.exe/PowerShell recon. Techniques include **DLL side-loading** using vendor-signed binaries from user-writable paths (often `C:\\ProgramData`), registry-stored encrypted config, outbound-HTTPS C2, and lateral movement via **WinRM (5985)**. Exfiltration uses **rclone.exe** to external cloud storage with tuned parameters. The post includes KQL hunts correlating Teams events with remote-assist launches, detecting signed-host sideload patterns in ProgramData, registry breadcrumbs, and rclone command lines, plus mitigations across Teams external collaboration controls, Safe Links for Teams, ASR rules, WDAC, Conditional Access, and WinRM scoping. Practically, it is the "legitimate workflow abuse" sibling of last week's device-code story: both rely on trusted UX and short windows where containment and revocation playbooks must be fast.

On macOS, Microsoft Threat Intelligence detailed a "Sapphire Sleet" campaign driven mostly by **user-initiated execution** of lures (for example, compiled AppleScript `Zoom SDK Update.scpt`) rather than exploits. Tradecraft centers on AppleScript's `do shell script` running staging like "curl -> osascript," with attacker user agents (`mac-cur1`...`mac-cur5`) mapping to stages (AppleScript loaders, ZIP-delivered `.app` bundles). The chain includes credential theft via a fake password prompt app, exfil via **Telegram Bot API**, persistence via **LaunchDaemons**, interactive backdoors opening zsh shells, and a technique to manipulate **TCC** by copying/modifying `~/Library/Application Support/com.apple.TCC/TCC.db` and injecting sqlite rows granting AppleEvents control. The writeup includes IOCs (domains/IPs/paths/hashes) plus Defender XDR/Sentinel KQL hunts for Script Editor spawning curl/osascript, TCC.db changes, suspicious LaunchDaemon creation, Telegram Bot API traffic, and ZIP staging. Alongside last week's router/DNS hijack story, it is a reminder that initial compromise can look like normal user behavior, so hunts often start from downstream process/auth/permission signals.

- [Cross‑tenant helpdesk impersonation to data exfiltration: A human-operated intrusion playbook](https://www.microsoft.com/en-us/security/blog/2026/04/18/crosstenant-helpdesk-impersonation-data-exfiltration-human-operated-intrusion-playbook/)
- [Dissecting Sapphire Sleet’s macOS intrusion from lure to compromise](https://www.microsoft.com/en-us/security/blog/2026/04/16/dissecting-sapphire-sleets-macos-intrusion-from-lure-to-compromise/)

### Azure identity-first security patterns: passwordless AKS secret sync and Entra ID for Storage SFTP

Platform guidance continues replacing embedded credentials with identity federation and scoped authorization, with two practical examples. This also ties to last week's managed identity blast-radius guidance: identity-first helps only when scoping and separation are done well.

For AKS, a guide shows syncing secrets from **Azure Key Vault** into namespaces using **External Secrets Operator (ESO)** with **AKS Workload Identity**, avoiding stored client secrets in-cluster. Steps include enabling AKS OIDC issuer + Workload Identity, creating a **User-Assigned Managed Identity (UAMI)**, binding it to a Kubernetes ServiceAccount via a **Federated Identity Credential** (audience `api://AzureADTokenExchange`), and granting Azure RBAC read access (often **Key Vault Secrets User**). ESO uses `SecretStore` (authType `WorkloadIdentity`) and `ExternalSecret` to materialize values into Kubernetes Opaque Secrets, with `refreshInterval` (example 30s) controlling rotation propagation. Troubleshooting focuses on RBAC scope, federated issuer/subject mismatch, and missing OIDC/workload identity enablement. Last week's "gotcha" still applies: do not reuse the same UAMI broadly. Workload Identity makes token acquisition easier, so identity hygiene (environment separation, narrow RBAC) matters more.

Entra ID integrated auth for **Azure Storage SFTP** was highlighted as a way to stop managing storage-account local users and instead authenticate via **Microsoft Entra ID**, authorizing via Storage data-plane RBAC (and optional **ABAC conditions**). A practical gotcha is that SFTP sessions are token-driven, so token lifetime and refresh behavior affects long transfers and persistent sessions. Validate client behavior before rolling into partner and batch pipelines. This matches last week's AiTM/device-code lessons: token handling (revocation, CAE/session constraints, client refresh behavior) increasingly determines access resilience for both attackers and automation.

- [Passwordless AKS Secrets: Sync Azure Key Vault with ESO + Workload Identity](https://techcommunity.microsoft.com/t5/microsoft-developer-community/passwordless-aks-secrets-sync-azure-key-vault-with-eso-workload/ba-p/4509959)
- [Entra ID Integrated SFTP](https://www.youtube.com/watch?v=pzPqnTHxNPU)
- [SFTP Entra ID Integrated Auth #azure #entraid #azurestorage](https://www.youtube.com/shorts/z1UXtM8HVqk)

### AI and cryptography readiness: operational playbooks, not just features

Two guidance pieces addressed areas where classic security operations needs adaptation (AI systems and cryptographic readiness), echoing last week's governance/operability theme. Features help most when paired with operating models, observability, and clear ownership.

On incident response, Microsoft's "Same fire, different fuel" argues core IR still applies (clear ownership, contain first, calm comms), but AI complicates triage and verification because outputs are probabilistic and harms do not map cleanly to CIA. The practical shift is **AI-specific observability**: anomalous output patterns, safety/classifier score shifts, spikes in user reports, and behavior changes after model/app updates. It recommends staged remediation: "stop the bleed" in the first hour (disable features, throttle, filters/blocklists), "fan out and strengthen" over 24 hours via automation, and "fix at the source" via classifier/model/guardrail updates, then verify with longer watch periods rather than one-off tests. It also calls out responder wellbeing when investigations involve harmful content. This overlaps with last week's agent governance theme: you need kill switches, clear guardrails, and telemetry to separate abuse, misconfiguration, drift, and toolchain issues.

On cryptography, Microsoft outlined building a **cryptographic inventory** as the basis for cryptographic posture management and post-quantum readiness. The inventory spans certificates/keys, protocols/ciphers, embedded libraries, algorithms referenced in source, secrets/credentials, and hardware-backed crypto, and it is continuous: discover -> normalize -> assess -> prioritize -> remediate -> monitor for drift. Developer relevance is work you can backlog: use **GHAS/CodeQL** to find crypto primitives/algorithms, pair with **Defender for Endpoint/Vulnerability Management** for certificate/component inventory, use **Azure Key Vault** as a key/secret/cert source of truth, and centralize signals in **Microsoft Sentinel** to avoid ad hoc ownership. It also complements last week's sovereignty/key-control thread: you cannot govern what you cannot inventory.

- [Incident response for AI: Same fire, different fuel](https://www.microsoft.com/en-us/security/blog/2026/04/15/incident-response-for-ai-same-fire-different-fuel/)
- [Building your cryptographic inventory: A customer strategy for cryptographic posture management](https://www.microsoft.com/en-us/security/blog/2026/04/16/building-your-cryptographic-inventory-a-customer-strategy-for-cryptographic-posture-management/)

### Other Security News

GitHub training content moved from prompt-injection basics to tool-using agent scenarios. Season 4 of the GitHub Secure Code Game introduces a vulnerable "ProdBot" with exploit-then-fix levels covering sandbox escape, untrusted web ingestion, MCP server tool connections, persistent memory risks (including poisoning), and multi-agent environments. It is designed for quick Codespaces runs and maps to OWASP agentic-app risks, complementing last week's agent governance and agentic SOC framing with hands-on failure modes.

- [Hack the AI agent: Build agentic AI security skills with the GitHub Secure Code Game](https://github.blog/security/hack-the-ai-agent-build-agentic-ai-security-skills-with-the-github-secure-code-game/)

Microsoft also published a domain-compromise case study showing how Defender XDR **predictive shielding** (Automatic Attack Disruption) aims to contain high-privilege identities based on exposure signals (for example, credential dumping) before active abuse. The narrative links footholds (IIS web shells, SYSTEM escalation, Mimikatz, NTDS snapshotting, Impacket lateral movement) to the defender "speed gap" versus slow remediation tasks (krbtgt rotation, GPO/ACL cleanup). It fits alongside last week's token/session focus: whether cloud token replay or on-prem credential dumping, advantage comes from fast containment plus practiced revocation/rotation runbooks.

- [Containing a domain compromise: How predictive shielding shut down lateral movement](https://www.microsoft.com/en-us/security/blog/2026/04/17/domain-compromise-predictive-shielding-shut-down-lateral-movement/)
