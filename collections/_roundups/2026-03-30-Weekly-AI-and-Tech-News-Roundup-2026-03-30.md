---
title: "Agents in the Workflow: PR Automation, Governed AI, and Safer CI/CD"
author: "TechHub"
date: 2026-03-30 09:00:00 +00:00
tags: ["GitHub Copilot","AI agents","Pull requests","GitHub Actions","Model Context Protocol (MCP)","Azure AI Foundry","Foundry Local","Microsoft Fabric","Data Factory","Dataflow Gen2","dbt","AKS","Private Link","Supply chain security","CodeQL"]
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
external_url: "/all/roundups/Weekly-AI-and-Tech-News-Roundup-2026-03-30"
---

Welcome to this week's roundup. The common thread is AI and automation showing up in places teams already work: pull requests, issue queues, CI/CD, and governed data platforms. Copilot's coding agent became more usable inside PRs (including conflict resolution) and easier to track in Issues and Projects, and admins got clearer controls and reporting as model options change. On the platform side, Foundry and Fabric updates leaned into "run it like software" practices (IaC scaffolding, local endpoints, event contracts, and traceable reasoning paths). Security coverage also reinforced why dependency pinning, scoped secrets, and tighter runner controls are becoming standard hygiene.

<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Copilot coding agent in pull requests, issues, and project tracking](#copilot-coding-agent-in-pull-requests-issues-and-project-tracking)
  - [Enterprise admin controls and adoption reporting for the coding agent](#enterprise-admin-controls-and-adoption-reporting-for-the-coding-agent)
  - [Model lifecycle and selection: Gemini 3.1 Pro arrives, Gemini 3 Pro exits](#model-lifecycle-and-selection-gemini-31-pro-arrives-gemini-3-pro-exits)
  - [Copilot SDK and Copilot CLI: building blocks for agentic apps and terminal workflows](#copilot-sdk-and-copilot-cli-building-blocks-for-agentic-apps-and-terminal-workflows)
  - [MCP-powered “agentic platform engineering”: Copilot beyond coding into CI/CD enforcement and AKS ops](#mcp-powered-agentic-platform-engineering-copilot-beyond-coding-into-cicd-enforcement-and-aks-ops)
  - [Copilot embedded in Microsoft tooling: SSMS GA, Azure App Service profiling fixes, and Fabric + MCP in VS Code](#copilot-embedded-in-microsoft-tooling-ssms-ga-azure-app-service-profiling-fixes-and-fabric--mcp-in-vs-code)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Azure AI Foundry & Foundry Local: agent delivery from cloud endpoints to offline workflows](#azure-ai-foundry--foundry-local-agent-delivery-from-cloud-endpoints-to-offline-workflows)
  - [Microsoft Fabric for AI agents: governed events, ontology context, and inspectable graph reasoning (Previews + GA)](#microsoft-fabric-for-ai-agents-governed-events-ontology-context-and-inspectable-graph-reasoning-previews--ga)
  - [MCP and agent platform choices: standardizing tool/context access and picking the right builder](#mcp-and-agent-platform-choices-standardizing-toolcontext-access-and-picking-the-right-builder)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Microsoft Fabric’s dbt roadmap: adapters, operational dbt Jobs, and a path to Fusion](#microsoft-fabrics-dbt-roadmap-adapters-operational-dbt-jobs-and-a-path-to-fusion)
  - [Fabric Real-Time Intelligence: Activator grows from alerting into action (Teams, Spark, Dataflows, and UDF triggers)](#fabric-real-time-intelligence-activator-grows-from-alerting-into-action-teams-spark-dataflows-and-udf-triggers)
  - [Fabric Data Factory: Copy job and connector upgrades for incremental movement, CDC, and cross-cloud destinations](#fabric-data-factory-copy-job-and-connector-upgrades-for-incremental-movement-cdc-and-cross-cloud-destinations)
  - [Other ML News](#other-ml-news)
- [Azure](#azure)
  - [AKS, GitOps, and App Containerization Workflows](#aks-gitops-and-app-containerization-workflows)
  - [Microsoft Fabric Data Factory & Dataflow Gen2: Performance, Automation, and Migration Paths](#microsoft-fabric-data-factory--dataflow-gen2-performance-automation-and-migration-paths)
  - [Fabric APIs, Private Connectivity, and Java Integration](#fabric-apis-private-connectivity-and-java-integration)
  - [Streaming and Real-Time Messaging: MQTT, Kafka, and Fabric Eventing](#streaming-and-real-time-messaging-mqtt-kafka-and-fabric-eventing)
  - [Private Networking and Data Movement in Locked-Down Environments](#private-networking-and-data-movement-in-locked-down-environments)
  - [Developer Libraries, Serverless Messaging Patterns, and Observability](#developer-libraries-serverless-messaging-patterns-and-observability)
  - [Other Azure News](#other-azure-news)
- [.NET](#net)
  - [.NET Aspire on Azure App Service (GA)](#net-aspire-on-azure-app-service-ga)
  - [.NET MAUI + AvaloniaUI backend (preview) for Linux and WebAssembly](#net-maui--avaloniaui-backend-preview-for-linux-and-webassembly)
- [DevOps](#devops)
  - [Microsoft Fabric CI/CD and “everything as code” (CLI, Git, pipelines, and environment promotion)](#microsoft-fabric-cicd-and-everything-as-code-cli-git-pipelines-and-environment-promotion)
  - [GitHub Actions and PR workflow ergonomics (runner images, agentic automation, review triage)](#github-actions-and-pr-workflow-ergonomics-runner-images-agentic-automation-review-triage)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [GitHub Actions supply-chain defense: from a real compromise to platform-level hardening](#github-actions-supply-chain-defense-from-a-real-compromise-to-platform-level-hardening)
  - [GitHub Code Security: faster PR scanning, broader detections, and more control over secret blocking](#github-code-security-faster-pr-scanning-broader-detections-and-more-control-over-secret-blocking)
  - [Microsoft Fabric security: network controls, encryption keys, and Purview-driven data protection](#microsoft-fabric-security-network-controls-encryption-keys-and-purview-driven-data-protection)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week's Copilot updates continued the shift from "help me write code" to "help me run the workflow," with more agent work inside pull requests, issues, and project boards. Building on last week's focus on "agents you can operate at scale" (faster starts, configurable validations, traceable logs, and better reporting), this week's changes bring that thread into core GitHub surfaces teams already use: PR comments, issue sidebars, and Projects views. GitHub also expanded model choice (while retiring older models), and Microsoft integrations (SSMS, Azure App Service tooling, Fabric in VS Code) kept positioning Copilot as an embedded assistant where developers already work.

### Copilot coding agent in pull requests, issues, and project tracking

After last week's improvements to execution quality (repo-tunable validations and commit-to-session traceability), Copilot's coding agent now fits PR workflows more naturally. You can invoke it on any existing pull request by commenting **@copilot** with the change you want, and it now updates the current PR by default instead of opening a stacked PR (unless you ask). That keeps common review iterations (fix failing Actions runs, address feedback, add tests) inside the same PR thread and history. It also builds on last week's "Agent-Logs-Url trailer" and clearer session logs: when more work happens in PRs, reviewers need direct links to what ran and why changes were made.

Copilot can also handle a frequent review task: **merge conflict resolution**. You trigger it via comments (for example, "merge in main and resolve the conflicts"), and it works in an isolated cloud dev environment, validates builds, runs tests, and pushes updates back to the PR. Together, "make changes on this PR" plus "resolve conflicts on this PR" shortens the loop for teams that rebase/merge often or maintain long-running branches, especially with last week's validation tuning so "prove it passes" matches the repo's real feedback loop.

GitHub also improved planning visibility so agent work is easier to track. When an agent is assigned to an issue, an **agent session** now appears in the issue sidebar with status (queued/working/waiting for review/completed) plus a link to logs. That signal can also show in GitHub Projects table/board views via "Show agent sessions," letting teams scan what's in progress vs. waiting on humans. This continues last week's observability push (more actionable logs, live streaming in Raycast): as agents run longer and touch more steps, "where is it and where are the logs?" becomes part of normal workflow hygiene.

- [Ask @copilot to make changes to any pull request](https://github.blog/changelog/2026-03-24-ask-copilot-to-make-changes-to-any-pull-request)
- [Ask @copilot to resolve merge conflicts on pull requests](https://github.blog/changelog/2026-03-26-ask-copilot-to-resolve-merge-conflicts-on-pull-requests)
- [Agent activity in GitHub Issues and Projects](https://github.blog/changelog/2026-03-26-agent-activity-in-github-issues-and-projects)

### Enterprise admin controls and adoption reporting for the coding agent

As more teams enable PR- and issue-driven agents (and now see agent status in Issues/Projects), GitHub is adding admin controls for where agents can operate. In public preview, org owners can manage coding agent repository access using new REST endpoints (apiVersion **2026-03-10**), setting it to: enabled for no repos, enabled for all repos, or enabled only for an allowlist (with add/remove endpoints). This matches last week's operational direction (validation "speed vs safety" controls and longer-lived defaults): the coding agent is becoming something you roll out repo-by-repo with automatable, auditable settings.

Usage reporting is also more specific, extending last week's push to close reporting gaps (org-level CLI activity and resolving "Auto" into the actual model). Copilot usage metrics now include **used_copilot_coding_agent** at the user level, so enterprises can separate "used Copilot in an IDE / agent mode" from "triggered the coding agent" (assigning Copilot to an issue or tagging @copilot in a PR comment). For internal dashboards and adoption analysis, this reduces ambiguity and aligns with this week's PR/issue-native invocation patterns.

- [Manage Copilot coding agent repository access via the API](https://github.blog/changelog/2026-03-24-manage-copilot-coding-agent-repository-access-via-the-api)
- [Copilot usage metrics now identify active Copilot coding agent users](https://github.blog/changelog/2026-03-25-copilot-usage-metrics-now-identify-active-copilot-coding-agent-users)

### Model lifecycle and selection: Gemini 3.1 Pro arrives, Gemini 3 Pro exits

Copilot's model picker keeps expanding across clients, following last week's theme of model choice as an admin-governed surface (rollouts, LTS options, clearer attribution). **Gemini 3.1 Pro** is now in public preview as a selectable chat model across github.com, GitHub Mobile, VS Code, Visual Studio, JetBrains IDEs, Xcode, and Eclipse. For Copilot Business and Enterprise, admins must enable the Gemini 3.1 Pro policy before users see it, which helps teams standardize (or restrict) model choice centrally.

At the same time, GitHub deprecated **Gemini 3 Pro** across Copilot experiences (Chat, inline edits, Ask/Agent/Edit modes, and completions) and points users to Gemini 3.1 Pro. This complements last week's "stable defaults" story: even with LTS models reducing churn, the wider lineup still changes, so teams should review internal docs, defaults, and enterprise policies, especially where policy enablement controls whether the replacement is selectable.

- [Gemini 3.1 Pro is now available in JetBrains IDEs, Xcode, and Eclipse](https://github.blog/changelog/2026-03-23-gemini-3-1-pro-is-now-available-in-jetbrains-ides-xcode-and-eclipse)
- [Gemini 3 Pro deprecated](https://github.blog/changelog/2026-03-26-gemini-3-pro-deprecated)

### Copilot SDK and Copilot CLI: building blocks for agentic apps and terminal workflows

On the "build with Copilot" side, this week's Copilot SDK coverage moved from an intro to a full end-to-end app example. GitHub published a walkthrough for an AI-backed issue triage app ("IssueCrush"). The key architecture point is that the Copilot SDK depends on Node and manages a local **Copilot CLI** process via JSON-RPC, so the SDK runs server-side (Node/Express) while a React Native client uses GitHub OAuth + REST APIs to fetch issues. The tutorial emphasizes production-oriented patterns: reuse a long-lived SDK client, manage session lifecycle (`start()`, `createSession()`, `sendAndWait()` with timeouts, `disconnect()`, `stop()` in `finally`), and degrade gracefully when Copilot auth/subscription is missing (HTTP 403 with `requiresCopilot`, plus a metadata-based fallback summary). This echoes last week's "session forensics" mindset: bound, resume, and reason about agent work instead of treating it as chat output.

For terminal workflows, GitHub also highlighted Copilot CLI slash commands: **/model** to switch models mid-session (useful alongside last week's model attribution and this week's Gemini changes), **/context** to view token usage and avoid bloated sessions, and **/resume** to continue a previous session. These fit last week's push for more traceable, inspectable sessions.

- [Building AI-powered GitHub issue triage with the Copilot SDK](https://github.blog/ai-and-ml/github-copilot/building-ai-powered-github-issue-triage-with-the-copilot-sdk/)
- [A beginner's guide to Copilot CLI slash commands](https://www.youtube.com/watch?v=-Yavis20B4Q)

### MCP-powered “agentic platform engineering”: Copilot beyond coding into CI/CD enforcement and AKS ops

This week's "agentic platform engineering" pattern continues last week's MCP/repo-native workflow progression: moving from "connect Copilot to tools" to "operationalize tool access with repeatable config, enforcement, and audit." The approach starts by capturing operational knowledge in repos so Copilot can answer "how do I...?" grounded in conventions, IaC modules, and real environments (including reverse-engineering portal-built Azure into Terraform/Bicep). It then moves into enforcement by wiring Copilot-driven checks into GitHub Actions using standardized prompt files (`.prompt.md` under `.github/prompts/`) so teams can update rules without rewriting pipeline logic. This matches last week's "repo-visible instructions/skills" and "governed workflow participant" themes.

The final stage connects AKS health signals to agent triage. Argo CD detects unhealthy deployments, Argo CD Notifications posts a payload via `repository_dispatch`, an Actions workflow creates/dedupes structured issues, and a second workflow triggers a custom Copilot agent ("Cluster Doctor") when a `cluster-doctor` label is applied. "Cluster Doctor" lives in `.github/agents/cluster-doctor.agent.md` with a collect->verify->diagnose->triage->remediate flow and safety constraints (verify cluster identity; avoid destructive actions without authorization), and it proposes fixes as PRs with humans approving. That lines up with this week's PR-native agent updates: PR-based remediation is more useful when agents can be invoked and iterated directly in the PR thread.

A key enabler is MCP: the repo includes `.copilot/mcp-config.json` to connect Copilot to a GitHub MCP server (issues/PRs) and an in-cluster **AKS MCP server** for telemetry and kubectl-based diagnostics, authenticated via Azure Workload Identity Federation. The emphasis is less "autonomous agent" and more "version operational playbooks as prompts/agents, enforce in CI, then turn GitOps signals into human-reviewed remediation PRs," extending last week's MCP examples (Azure DevOps remote MCP, GitHub secret scanning via MCP, Fabric MCP) into end-to-end platform ops.

- [Agentic Platform Engineering with GitHub Copilot](https://devblogs.microsoft.com/all-things-azure/agentic-platform-engineering-with-github-copilot/)

### Copilot embedded in Microsoft tooling: SSMS GA, Azure App Service profiling fixes, and Fabric + MCP in VS Code

Copilot continued to show up as a first-class feature in Microsoft developer tools, reinforcing last week's theme that customization and tool wiring are becoming portable configuration rather than per-app prompt habits. **SSMS 22.4.1** moved "GitHub Copilot in SSMS" from preview to **GA**, and added support for user-scoped custom instructions at `%USERPROFILE%\\copilot-instructions.md` so Copilot can stay consistent even for ad-hoc scripts outside a repo/solution. This complements last week's repo-visible instructions/skills: when there is no repo context (or you do not want one), user-level instructions can act as a set of defaults you control. The update also added more export formats (Excel, JSON, XML, Markdown), "Save with Encoding...", broader "group objects by schema" support beyond Fabric SQL databases, plus a new "SQL Projects (Preview)" entry and a roadmap note toward future "Agent mode."

In VS Code, the Azure App Service extension added a "Code Optimizations" workflow that converts **Application Insights Profiler** findings into "Fix with Copilot" prompts. Items (CPU/memory, impact %, stack traces) appear under "Code Optimizations"; the extension maps stack traces to local methods, opens the code, and seeds Copilot Chat with the recommendation, stack trace, and method source to drive concrete refactors. It targets .NET web apps on App Service, with Windows/Linux differences in profiling enablement. Conceptually it matches last week's "give the agent real signals and context" thread, but here the signals come from production profiling rather than repo scanning.

On the data side, Microsoft Fabric updated its VS Code extension for workspace artifact management (browse folders, open item definitions; editing behind an "allow editing" setting), and introduced a pre-release **Fabric MCP server** extension for Copilot Chat. With MCP tools enabled, Copilot can interpret item definitions, work with Fabric REST APIs/docs, and run tenant/workspace operations (CRUD items, OneLake file operations, notebook creation/run) inside the editor. This continues last week's Fabric MCP storyline (local MCP GA/open source and remote MCP in preview): "Copilot acts through governed tools," now packaged for day-to-day editor workflows.

- ['SSMS 22.4.1: GitHub Copilot in SSMS GA, custom instructions, and new export formats'](https://blog.fabric.microsoft.com/en-US/blog/sql-server-management-studio-ssms-22-4-1-and-github-copilot-in-ssms-generally-available/)
- [Code Optimizations for Azure App Service Now Available in VS Code](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/code-optimizations-for-azure-app-service-now-available-in-vs/ba-p/4504252)
- [Explore, edit, and organize Item definitions and Fabric MCP server in VS Code](https://blog.fabric.microsoft.com/en-US/blog/explore-edit-and-organize-item-definitions-and-fabric-mcp-server-in-vs-code/)

### Other GitHub Copilot News

Long-running usage data from a large OSS repo added practical "what works at scale?" guidance for coding agents. After last week's focus on operability (validation tuning, traceability, reporting), the .NET team's ten-month review of Copilot Coding Agent in **dotnet/runtime** provides concrete maintenance outcomes: 878 agent-authored PRs (14% of PRs) with ~68% merged rate for decided PRs. Results improved when the repo was prepared (documented build/test commands, conventions in `.github/copilot-instructions.md`, agent access to needed feeds). They also showed "pairing" matters: agent PRs with human commits merged far more often than fully hands-off ones, and strict expectations (like requiring benchmark evidence for performance PRs) still applied. This aligns with this week's PR-native invocation: pairing is easier when the agent works in the same PR thread reviewers already use, not in a separate stacked PR.

- [Ten Months with Copilot Coding Agent in dotnet/runtime](https://devblogs.microsoft.com/dotnet/ten-months-with-cca-in-dotnet-runtime/)

Work tracking integrations kept improving the "agent in the workflow" story. In GitHub Copilot for Jira public preview, setup guidance and errors improved, users can select a supported model directly from Jira comments, and traceability tightened with Jira keys in branch names and PR titles plus links back to tickets. For Confluence-heavy teams, it can pull Confluence context via MCP by configuring an Atlassian MCP server with a Confluence PAT, which is useful but should be evaluated within an org's permissions and data-access model. This parallels GitHub's own shift: as MCP becomes the common way to bring systems into the agent loop, integrations increasingly look like repeatable MCP configuration plus clear policy boundaries.

- [GitHub Copilot for Jira — Public preview enhancements](https://github.blog/changelog/2026-03-25-github-copilot-for-jira-public-preview-enhancements)

Policy and governance stayed central, continuing last week's "defaults, attribution, reporting" direction with a privacy angle. GitHub announced a Copilot interaction data policy change effective **April 24, 2026**: Copilot Free/Pro/Pro+ will use interaction data for model training by default unless users opt out, while Copilot Business/Enterprise are unaffected. Developers using personal plans on proprietary code should review settings at `github.com/settings/copilot`, and teams may want to document which account types are approved for which repos. Related guardrails guidance connected repo controls (like `.github/copilot-instructions.md` and duplicate detection) with CI/CD enforcement and Azure-side governance (Content Safety, Azure AI Foundry, DLP for Copilot Studio, Entra/RBAC, audit/monitoring). Together with last week's model governance/traceability work, the message is consistent: if agents act in real workflows, teams need controls over both capabilities and data flows.

- [Updates to GitHub Copilot interaction data usage policy](https://github.blog/news-insights/company-news/updates-to-github-copilot-interaction-data-usage-policy/)
- ['Guardrails for Generative AI: Securing Developer Workflows'](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/guardrails-for-generative-ai-securing-developer-workflows/ba-p/4505801)

Teams trying multi-model setups got a practical resilience pattern that echoes last week's multi-model review and "skills as shared assets" themes. Run Claude Code and Copilot CLI side-by-side, share as much repo configuration as possible (instructions, skills, MCP servers), and keep only tool-specific glue separate. The guidance includes concrete layouts (for example, `CLAUDE.md` as a single instructions source, syncing agent defs into `.github/agents/*.agent.md`, registering MCP servers in each tool's config) so tool switching during latency/errors does not require rebuilding your setup. It matches last week's lesson: reliability often comes from explicit, versioned configuration, not UI state.

- [Using Claude Code and GitHub Copilot CLI together for resilient agentic coding](https://devblogs.microsoft.com/all-things-azure/your-entire-engineering-floor-just-stopped-coding/)

A DevX Summit talk republished this month used Copilot CLI to frame how agent-heavy development shifts daily work toward intent specification, decomposition, and verification. The practical skill is less prompting and more repeatable review/validation habits that keep humans accountable for what ships. This fits the last two weeks' arc: more agent autonomy and surfaces, paired with more traceability, governance, and "prove it" hooks.

- [The Transformation of the Developer Role with AI Agents](https://www.youtube.com/watch?v=0HI3OIi-YJY)

## AI

This week's AI updates tracked two parallel themes: shipping agents into production with repeatable workflows and governance, and adopting more local-first, inspectable patterns for building and operating AI systems. Across Azure AI Foundry, Foundry Local, and Microsoft Fabric, the common thread was making agent behavior easier to deploy, ground, observe, and control via IaC scaffolding, structured tool plans, ontology/graph grounding, and cost guardrails. This continues last week's "run it like software" arc: last week delivered GA runtimes, private networking, managed identity, evaluation hooks, and MCP tooling glue; this week shows how teams ship and operate those ideas (IaC-first delivery, offline OpenAI-style endpoints, and more traceable retrieval/reasoning).

### Azure AI Foundry & Foundry Local: agent delivery from cloud endpoints to offline workflows

Foundry updates covered both ends of deployment: "code-to-cloud" publishing of agent endpoints and offline/on-device systems that still look like OpenAI-compatible apps. If last week was about the agent runtime being ready for production (GA runtime, private networking, evaluation, managed identities), this week focused on making that posture reproducible from a repo and extending "treat it like a service" to local endpoints.

On the cloud side, the Azure Developer CLI added a direct path from a Python agent repo to a live Azure AI Foundry agent endpoint via the `azd ai agent` extension. Building on last week's `azd ai agent run`/`invoke`, it now covers deployment with infra/identity defaults that match the governance patterns we've been tracking (managed identity + RBAC, scripted flows for CI/CD). The workflow is intentionally opinionated: `azd ai agent init` scaffolds Bicep IaC (notably `infra/main.bicep`), `azure.yaml`, and `agent.yaml` for metadata/env vars. Then `azd up` provisions Foundry, deploys a model config (GPT-4o example), configures managed identity + RBAC, and publishes the endpoint with a portal link for playground validation. The inner loop includes `azd ai agent invoke` (multi-turn), `azd ai agent run` (local execution through the same flow), `azd ai agent monitor`/`--follow` for logs, and `azd down` for cost cleanup. The optional chat frontend wiring highlights using `azd env get-values`/`set` to keep app<->agent connections repeatable across environments and CI/CD (for example, GitHub Actions running `azd up` on `main`), complementing last week's focus on repeatable evaluation/monitoring loops.

Foundry Local also matured as the offline counterpart, with examples that treat "local LLM runtime" as a dependency you operate rather than a demo shortcut. The OpenAI-compatible endpoint detail continues last week's wire-compatibility thread: whether you use cloud Foundry or Foundry Local, client code can stay stable while you swap endpoints/environments.

One guide shows a multi-agent robotics automation pipeline that keeps the LLM away from direct simulator control using a constrained contract: "LLM -> strict JSON plan -> safety validation -> executor." This matches last week's themes (structured outputs, approvals, least-privilege tools) but in a local control loop where safety/determinism matter more. Foundry Local exposes an OpenAI-compatible endpoint, so the main client change is the base URL; the example uses `FoundryLocalManager` and models like `qwen2.5-coder-0.5b` with automatic backend selection (CUDA GPU -> QNN NPU -> CPU). Agents are split cleanly: PlannerAgent emits JSON tool calls, SafetyAgent validates schema/bounds in sub-millisecond time, and ExecutorAgent runs PyBullet behaviors (IK movement, pick/place, scene description). It also includes offline voice commands (browser MediaRecorder, 16kHz mono WAV resampling, server-side ONNX Whisper with caching/chunking) feeding the same flow, which is useful for hands-free or low-latency local control. Compared with last week's Foundry Voice Live, it's a contrast between cloud real-time voice and local capture/transcription, with the same need for traceable plans and safety gates. Model timing comparisons (sub-5s on the smallest vs ~35-45s on larger ones) make the interactive tradeoffs concrete.

A second Foundry Local tutorial applies the OpenAI-compatible runtime to an offline RAG assistant ("Interview Doctor"), using a deliberately lightweight retrieval approach. Instead of embeddings, it chunks docs (~200 tokens + overlap), stores term-frequency vectors in SQLite (`sql.js`), and retrieves via cosine similarity, positioned as ~1ms for small corpora (CV + job descriptions) without running an embedding model alongside the LLM. This pairs with last week's Foundry IQ direction (permission-aware retrieval as a standard tool surface): different stack, same goal of grounding as a reusable, testable component. The app is Node/Express with a single-file web UI streaming via SSE plus a CLI, and it notes an operational gotcha: Foundry Local uses a dynamic port, so use the SDK-discovered endpoint (`manager.endpoint`) instead of hardcoding `localhost`. It also demonstrates testability using `node:test` so core logic can be validated without the local runtime running, another "run it like software" signal applied to offline builds.

- [From code to cloud: Deploy an AI agent to Microsoft Foundry in minutes with azd](https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-end-to-end/)
- [Building real-world AI automation with Foundry Local and the Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-real-world-ai-automation-with-foundry-local-and-the/ba-p/4501898)
- [Building an Offline AI Interview Coach with Foundry Local, RAG, and SQLite](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-an-offline-ai-interview-coach-with-foundry-local-rag/ba-p/4500614)

### Microsoft Fabric for AI agents: governed events, ontology context, and inspectable graph reasoning (Previews + GA)

Fabric's AI updates focused on making real-time signals and business context reusable and governable, so agents, automation, and analytics can share definitions across notebooks, pipelines, and dashboards. It continues last week's Fabric direction: put AI work into shared, governed surfaces (real-time intelligence + IQ context + semantics) instead of isolating it in one notebook or app.

Business Events in Fabric (preview) add a business-level event layer in Real-Time Hub. Instead of raw telemetry sent to tightly coupled consumers, teams define business event types with governed schemas via Schema Registry/Schema Sets, then emit events from Fabric compute (Notebooks and User Data Functions are called out). This extends last week's "Observe -> Analyze -> Decide -> Act" loop by making "observe" a versioned contract: less ad-hoc plumbing, more reusable signals downstream systems (including agents) can trust. The value is decoupled fan-out: one Business Event can drive Activator actions, Power Automate, Notebooks, Spark jobs, Dataflows, and AI/ML enrichment without the publisher knowing consumers. The manufacturing example (anomaly -> "CriticalVibrationDetected" -> safe-mode + ticket + root-cause notebook) illustrates reducing glue code while keeping schemas consistent.

For agent context, Fabric IQ Ontology (preview) positions ontology items as operational context: mapping entities/relationships to OneLake data and events so agents do not rely on inconsistent definitions. This builds on last week's Fabric IQ/ontology push: centralize semantics for humans and agents. The roadmap adds embedded rules/actions (via Activator), Fabric-aligned permissions/sharing (Read/Edit/Reshare), and tenant/workspace Azure Private Link hardening, mirroring last week's move toward private networking and governed access across Foundry/Fabric. It also points to interoperability via upcoming "Ontology MCP endpoints," exposing ontology context through public MCP endpoints so external MCP-capable agents can retrieve the same grounded business context. Given last week's MCP endpoint momentum, this is another step toward "business context as a standard tool surface," not copied into prompts.

Fabric also previewed "graph-powered AI reasoning," combining Fabric Data Agent with Fabric Graph for more inspectable answers via deterministic traversal ("graph RAG"). This matches last week's evaluation/traceability emphasis but constrains the reasoning path: translate natural language to validated GQL via NL2GQL, run deterministic graph traversals, and expose a GQL trace so users can review which relationships produced the answer. The Adventure Works example highlights recommendation logic that can be awkward in SQL but explicit in a graph (including derived nodes like country) and queryable with traceable outputs, which is useful when you need an auditable reasoning path rather than only probabilistic text.

Finally, Fabric's Workload Hub delivered an AI data-readiness GA: Tonic Textual is now generally available for scanning unstructured OneLake text for sensitive entities and applying transformations (redaction, masking, synthetic replacement, custom rules), writing results back to a separate OneLake location. This aligns with last week's "production RAG is access control and repeatability" theme: data needs a standardized privacy/prep step before retrieval/eval pipelines become trustworthy. The practical benefit is OneLake-to-OneLake de-identification that preserves structure (dialogue, contracts) instead of exporting to external tools.

- [Business Events in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/business-events-in-microsoft-fabric-preview/)
- [What’s next for Fabric IQ Ontology: The operational context that powers your AI agents (Preview)](https://blog.fabric.microsoft.com/en-US/blog/whats-next-for-fabric-iq-ontology-the-operational-context-that-powers-your-ai-agents-preview/)
- [Graph-powered AI reasoning (Preview)](https://blog.fabric.microsoft.com/en-US/blog/graph-powered-ai-reasoning-preview/)
- [Preparing unstructured data in Microsoft Fabric with Tonic Textual (GA)](https://blog.fabric.microsoft.com/en-US/blog/from-restricted-to-ai-ready-preparing-unstructured-data-directly-in-microsoft-fabric-with-tonic-textual-generally-available/)

### MCP and agent platform choices: standardizing tool/context access and picking the right builder

A recurring architecture thread this week was how agents get tools and context, and how teams choose an agent-building surface as requirements grow. Last week framed MCP as increasingly operational (remote MCP servers in Foundry, managed Grafana MCP endpoints, and a .NET SDK). This week continued that theme with protocol maturity and tool-surface examples beyond typical enterprise apps.

Two MCP pieces reinforce the momentum. GitHub's Universe video explains MCP as a standardized contract for exposing tools/data to agents, especially private or new information that is not in training data, and notes the official open-source MCP server was rewritten from TypeScript to Go, which changes deployment and contribution details. That kind of reference shift suggests MCP is moving from experimentation into maintainability (runtime footprint, deployment model, contributor workflow), matching last week's shift toward hosted, identity-aware endpoints. Unity-MCP shows MCP-style structured calls in a game engine where "context" is editor/project state (scenes, GameObjects, assets, components), giving AI a more deterministic surface than text alone.

Two "which platform should I use?" pieces also landed, making explicit what tends to trigger migration. When governance, evaluation, observability, and custom tool/knowledge wiring matter, teams often move from simpler builders toward Foundry/Azure AI Agents-style surfaces (often keeping a separate interaction layer). One compares Copilot Studio vs Azure AI Agents: low-code connectors and predictable pricing for well-defined assistants vs developer-built, consumption-based systems with model choice, RAG, orchestration, evaluation, and observability. The other provides a broader framework across Agent Builder, Copilot Studio, and Azure AI Foundry, emphasizing criteria that drive migration: complexity, model flexibility, deployment targets, lifecycle ops (eval/observability), safety/guardrails, memory/state, tool/knowledge integration, and cost control. Together they reinforce a hybrid approach: Copilot Studio for UI/flows, backed by a programmable Foundry/Azure AI Agents layer for intelligence and governance, consistent with last week's standardization on MCP and operational loops under multiple app surfaces.

- [What is MCP and how does it work with AI?](https://www.youtube.com/watch?v=lFQz1hugvHo)
- [Open Source Friday with Unity-MCP](https://www.youtube.com/watch?v=Ng5ltWrSG0M)
- [Copilot Studio vs Azure AI Agents: What Should You Use?](https://dellenny.com/copilot-studio-vs-azure-ai-agents-what-should-you-use/)
- [Picking the right Agent Builder solution](https://www.youtube.com/watch?v=WUgujz0y1K4)

### Other AI News

Cost and operations management showed up as platform guardrails and incident automation, connecting to last week's "day-two" focus (evaluation, observability, private networking, identity). One guide shows an Azure-native spend control loop for Azure OpenAI: Cost Management Budgets trigger Action Groups, which run Azure Automation PowerShell to disable local auth (`Set-AzCognitiveServicesAccount -DisableLocalAuth $true`) when thresholds are hit, with a separate manual runbook to re-enable after review. Separately, Azure SRE Agent HTTP Triggers shows starting an automated investigation from Jira using an Azure Logic App as a Managed Identity auth bridge (the trigger endpoint is Entra-protected and uses SRE Agent data-plane RBAC). The pattern (external system -> Managed Identity bridge -> SRE Agent trigger) keeps credentials out of Jira while preserving audit history, and uses a Jira MCP connector (`mcp-atlassian` 2.0.0 over STDIO). In the context of last week's MCP identity modes and managed endpoints, it's another example of pairing agent actions with identity boundaries and auditability.

- [Automating Azure OpenAI Cost Control Using Budgets, Action Groups, and Automation Runbooks](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/automating-azure-openai-cost-control-using-budgets-action-groups/ba-p/4505164)
- [HTTP Triggers in Azure SRE Agent: From Jira Ticket to Automated Investigation](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/http-triggers-in-azure-sre-agent-from-jira-ticket-to-automated/ba-p/4504960)

.NET developers got an updated learning path: "Generative AI for Beginners .NET" v2 is rebuilt on .NET 10, switches foundational model calling from Semantic Kernel to Microsoft.Extensions.AI (`IChatClient` + middleware pipeline), and standardizes auth with `AzureCliCredential` so you can log in once via Azure CLI instead of distributing keys. This aligns with last week's managed identity/consistent auth defaults and complements last week's MCP/.NET tooling by clarifying the baseline stack before heavier orchestration. RAG content is reworked toward native SDKs, and the agent module uses Microsoft Agent Framework (RC), keeping orchestration as a dedicated topic rather than the default entry point.

- [Generative AI for Beginners .NET: Version 2 on .NET 10](https://devblogs.microsoft.com/dotnet/generative-ai-for-beginners-dotnet-version-2-on-dotnet-10/)

RAG patterns continued to diversify beyond embeddings, echoing last week's themes of governed grounding and traceability. A "vectorless reasoning-based RAG" tutorial uses PageIndex to build a hierarchical document tree for long PDFs, then has an LLM select relevant nodes (pages/sections) via strict JSON before answering only from retrieved text. The goal is fewer moving parts than embeddings + vector DB and better traceability back to page indices and node IDs.

- [Vectorless Reasoning-Based RAG: A New Approach to Retrieval-Augmented Generation](https://techcommunity.microsoft.com/t5/microsoft-developer-community/vectorless-reasoning-based-rag-a-new-approach-to-retrieval/ba-p/4502238)

Foundry Labs introduced a "scout -> evaluate -> graduate" workflow: try early-stage model/agent experiments (30+ projects) with clear maturity expectations, then move promising work into Azure AI Foundry where evaluation, tracing, monitoring, and governance are first-class. This maps to the two-week story: prototype quickly, but capture telemetry early and graduate into a runtime where evaluation/observability are standard (last week's Foundry Evaluations GA and agent runtime GA are the likely destinations). It also ties observability to Azure API Management's genAI gateway controls (token metrics, prompt logging, quotas, safety policies) and suggests capturing telemetry from day one (even JSONL logs).

- [Microsoft Foundry Labs: A Practical Fast Lane from Research to Real Developer Work](https://techcommunity.microsoft.com/t5/microsoft-developer-community/microsoft-foundry-labs-a-practical-fast-lane-from-research-to/ba-p/4502127)

Fabric Real-Time Dashboards added a Copilot preview that generates and iterates KQL tiles from natural-language requests, suggesting a visualization, showing a preview table, and exposing editable KQL. It matches last week's Fabric theme of speeding authoring where teams work while keeping the query layer inspectable, applied to real-time ops dashboards.

- [Use Copilot to create visuals in Real-Time Dashboards (Preview)](https://blog.fabric.microsoft.com/en-US/blog/use-copilot-to-create-visuals-in-real-time-dashboards-preview/)

Two "Budget Bytes" teaser posts pointed to a cost-constrained, hands-on AI app series around Azure SQL Database, linking to playlist/repo/free offer resources rather than going deep technically. In the context of last week's cost/eval/ops focus, it's another signal that budget-aware engineering is now common in AI guidance.

- [Budget Bytes: Azure Data Leaders on AI & Budget (Sneak Peek)](https://www.youtube.com/watch?v=BaGqdGds-eM)
- [What Would You Buy With $25? Answers from Execs](https://www.youtube.com/shorts/RIqLHwfg7oI)

A manufacturing case study highlighted a "copilot for operators" pattern: ARUM's CNC assistant uses Azure AI Speech plus Azure OpenAI hosted in Microsoft Foundry (noted as GPT-5) to provide Japanese, step-by-step guidance for safety-critical setup tasks. It's light on implementation details, but it connects threads we've been tracking: voice modalities (last week's Voice Live vs this week's offline voice pipeline) and production guardrails where procedures and human confirmation matter.

- [Japan’s ARUM turns craftsmanship into scalable AI for precision manufacturing](https://news.microsoft.com/source/asia/features/japans-arum-turns-craftsmanship-into-scalable-ai-for-precision-manufacturing/)

## ML

This week's ML-adjacent momentum mostly came through Microsoft Fabric, with updates that make analytics engineering more like a managed product: repeatable transformation workflows (dbt), more event-driven automation (Activator + UDFs), and steadier ingestion mechanics (Copy job upgrades, more connectors, easier troubleshooting). Building on last week's "pipelines over one-off notebooks" theme (Materialized Lake Views, Environments, Notebook Public APIs), the thread is Fabric turning those building blocks into managed operating surfaces: author in familiar tools, execute in Fabric, and connect actions with less custom glue. Fabric also tightened admin/governance with better workspace organization at scale.

### Microsoft Fabric’s dbt roadmap: adapters, operational dbt Jobs, and a path to Fusion

Fabric continues treating dbt as a first-class workflow, focusing not just on adapter availability but on correctness for Fabric SQL semantics, materializations, and performance. This mirrors last week's shift toward declarative transforms via Materialized Lake Views: dbt is another "transformations as code" path, and Fabric is aiming for clean mapping to Warehouse and (soon) Lakehouse execution. Today, the recommendation for SQL-first managed warehouse work is the Fabric Warehouse dbt core adapter; a Fabric Lakehouse dbt core adapter is "coming soon" as GA for running dbt directly on Lakehouse tables in OneLake, aligned with Fabric governance and compute/storage separation.

Operationally, dbt Jobs in Fabric (public preview since December 2025) is positioned as the control plane for scheduling, retries, environment promotion, and observability. This matches last week's "managed orchestration" focus (Notebook Public APIs + Job Scheduler): less interactive execution, more managed jobs with traceable outputs. Recent additions include public package support, native GitHub support (run jobs from GitHub-hosted dbt projects for CI/CD alignment), and OneLake-based enterprise logging with no size limits (removing the prior 1 MB cap). API support enables automation, and "coming soon" items include dbt Jobs as a Fabric Pipelines activity with parameterization, plus Lakehouse adapter support in dbt Jobs (Warehouse supported today).

Looking ahead, Fabric called out planned dbt Fusion support expected later in calendar Q2 2026, focusing on clean Warehouse/Lakehouse adapter integration and aligned execution metadata/observability as Fusion enters dbt's runtime story. The net effect is a cohesive path: author in GitHub, execute/schedule in Fabric, centralize logs in OneLake, and adopt Fusion-backed execution later without reworking Warehouse/Lakehouse layouts.

- [dbt + Microsoft Fabric: dbt adapters, dbt Jobs on OneLake, and upcoming dbt Fusion support](https://blog.fabric.microsoft.com/en-US/blog/dbt-microsoft-fabric-a-strategic-investment-in-the-modern-analytics-stack/)

### Fabric Real-Time Intelligence: Activator grows from alerting into action (Teams, Spark, Dataflows, and UDF triggers)

Fabric Activator is expanding from "tell me something happened" to "do something when it happens," adding rule actions to send Microsoft Teams messages and trigger compute/pipeline work: run a Spark job, run a User Data Function (UDF), or run a Dataflow (Dataflows Gen2). This reduces glue code by removing the need for custom listener services that translate events into downstream work, especially when teams want event-driven processing instead of scheduled refresh. It follows last week's automation direction: after notebooks became easier to run/manage via APIs, Activator now provides an "event -> execution" surface inside Fabric without external schedulers.

Two additions stand out for operational workflows. First, triggering UDFs from Activator creates a direct event-to-function bridge: rules can pass entity IDs, values, and timestamps into code, enabling incidents/runbooks/custom logic without new infrastructure. This pairs with this week's UDF defaults update: as UDFs become shared primitives invoked by rules, backwards-compatible signatures matter more. Second, Spark job and Dataflow actions can respond to Fabric and Azure Blob Storage events, enabling "data landed, process now" patterns rather than waiting for schedules, similar in spirit to last week's near-real-time pipeline patterns but implemented through Fabric's event/action model.

Authoring surfaces broadened too: Warehouse SQL query monitoring rules (Preview) let rules run on ad-hoc or saved query results on a frequency, and Ontology entity rules (Preview) support entity-level conditions. Rule creation is now embedded in Eventstream, and Power BI integration improved so Activator can alert when a new row appears in a table visual in a published report, which helps when dashboards function as queue views.

- [What’s new with Fabric Activator: more connected and capabilities](https://blog.fabric.microsoft.com/en-US/blog/whats-new-with-fabric-activator-more-connected-and-capabilities/)

### Fabric Data Factory: Copy job and connector upgrades for incremental movement, CDC, and cross-cloud destinations

Fabric Data Factory's Copy job updates targeted ingestion constraints where schemas do not match ideal assumptions. This is Fabric's version of the "productionize the plumbing" story we touched last week (Databricks Lakeflow simplifying ingestion + CDC + SCD): in Fabric, improvements are landing in Copy job incremental and CDC behavior, which often blocks teams before transformations like MLVs or dbt. Incremental copy is now more flexible in GA with additional watermark types: ROWVERSION, date/datetime (with delayed extraction to reduce missed late updates), and string columns interpreted as datetime. This reduces custom query workarounds while still using built-in state tracking and checkpointing.

CDC replication added three practical updates: Oracle as a CDC source, Fabric Data Warehouse as a CDC sink, and an SCD Type 2 write method in Preview as a simple toggle. The SCD2 option provides history-table semantics (new version rows on updates; soft deletes via expiring current versions), reducing per-table MERGE logic and custom frameworks. It echoes last week's SCD2-as-first-class capability in Databricks, but here it's pushed down into ingestion so history tables can be created earlier without bespoke transform code.

Connector and throughput improvements also landed. SharePoint Online File is now GA as source/destination, easing "files in SharePoint" ingestion/publishing. BigQuery, MySQL, and PostgreSQL gained destination write support in Preview for more cross-cloud movement. "Native incremental copy" expanded to more connectors (including RDS variants, ODBC, GCS, SharePoint Lists/Files, Fabric Lakehouse tables/files), and automatic partitioning was introduced to speed large-table loads by parallelizing reads/writes via a selected partition column without manual setup.

- [Incremental copy gets more flexible—New watermark column types in Copy job in Fabric Data Factory (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/incremental-copy-gets-more-flexible-new-watermark-column-types-in-copy-job-in-fabric-data-factory-generally-available/)
- [Richer CDC in Fabric Data Factory Copy job: Oracle source, Fabric Data Warehouse sink, and SCD Type 2 (Preview)](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-movement-across-multiple-clouds-with-richer-cdc-in-copy-job-in-fabric-data-factory-oracle-source-fabric-data-warehouse-sink-and-scd-type-2-preview/)
- [Outstanding connectivity for data movement in Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/outstanding-connectivity-for-data-movement-in-fabric-data-factory/)

### Other ML News

Fabric's programmable surfaces got a small but useful update: User Data Functions (UDFs) now support default arguments in Python. Because inputs are JSON-serialized, defaults must be JSON-serializable (strings, numbers, booleans, arrays/lists, objects/dicts, and datetime-like strings, ideally ISO 8601). The guidance also reiterates standard Python practice for mutable defaults (use `None` then assign inside), which helps teams evolve shared UDFs without breaking callers. This pairs with Activator triggering UDFs: defaults allow signature extension without updating every rule immediately.

- [Support for default arguments in Fabric User data functions](https://blog.fabric.microsoft.com/en-US/blog/support-for-default-arguments-in-fabric-user-data-functions/)

Dataflow Gen2 troubleshooting is becoming more self-service. A Preview feature lets admins/support download a per-run diagnostic package from run history after completion. It bundles metadata, structured logs, execution traces, and runtime/environment signals, reducing time spent collecting evidence across views for failed or slow runs. This continues last week's day-2 manageability thread: as more execution becomes managed and event-driven, diagnostics determine whether failures are quickly explainable.

- [Dataflow Gen2 – Dataflow Diagnostics Download (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dataflow-gen2-dataflow-diagnostics-download-preview/)

Workspace tags are now GA, providing a first-class way to label workspaces (team, project, environment, cost center) and filter them in the workspaces list and OneLake Catalog Explorer. Tags are also exposed via REST APIs (create/apply/remove and included in Get/List Workspaces), supporting automated inventory and governance reporting; Fabric Scanner APIs are expected to include tags later. This complements last week's API-driven ops push: as teams automate notebook/job lifecycles, programmatic workspace organization helps control sprawl.

- [Find and manage workspaces faster with workspace tags (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/find-and-manage-workspaces-faster-with-workspace-tags-generally-available/)

Fabric Open Mirroring added a GA ERP replication option: the BC2Fab Fabric Workload (Navida) replicates Dynamics 365 Business Central tables into Fabric with incremental change detection and schema evolution handling. The goal is lighter transformation-heavy ingestion and reduced load on production ERP, while enabling querying in Fabric engines and Power BI reporting on OneLake-backed copies. Like last week's consolidation of ingestion and governance for near-real-time pipelines, it continues moving replication closer to standardized OneLake landing zones so downstream dbt/MLV work can focus on shaping data, not extraction.

- [Integrating Dynamics 365 Business Central with Microsoft Fabric using Open Mirroring with BC2Fab workload (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/integrating-dynamics-365-business-central-with-microsoft-fabric-using-open-mirroring-with-bc2fab-workload-generally-available/)

## Azure

Azure updates leaned into making platform operations more predictable (containers, networking, observability) and smoothing paths into Microsoft Fabric as teams standardize on it for pipelines, warehousing, and real-time analytics. Much of the change was plumbing (identity, private connectivity, bulk APIs, monitoring) aimed at making migrations and day-2 operations less fragile. This continues last week's "controlled transitions" framing: swap components in phases and invest in guardrails (identity, networking prerequisites, health, runbooks) that keep changes routine.

### AKS, GitOps, and App Containerization Workflows

AKS and Kubernetes tooling stayed focused on safer operations and standardized delivery. Building on last week's ingress modernization thread (moving off Ingress NGINX toward Application Gateway for Containers, and reducing in-cluster plumbing), this week's GitOps additions push the same direction: repeatable, auditable changes with less bespoke cluster setup.

The new Argo CD extension is in public preview for AKS and Azure Arc-enabled Kubernetes, providing an Azure-integrated packaging of Argo CD with Microsoft Entra ID SSO and Workload Identity federation for Azure resource access (ACR, Azure DevOps). That reduces reliance on long-lived secrets in Git, and it echoes last week's AGC prerequisites (workload identity for ALB controller): across ingress and GitOps, Azure's preferred path is federation over service principal secrets. The extension is also aimed at fleet patterns (ApplicationSet, hub-and-spoke multi-cluster GitOps, HA) with a more controlled patching model, and Azure Portal management is expected soon after the initial CLI-based onboarding.

For app modernization, Microsoft introduced Containerization Assist as open-source tools/guidance to help teams move existing apps to containers through assess -> transform -> operationalize phases. It matches the sequencing pattern highlighted last week across AKS and Fabric migrations: assess first, then phased cutover, with an emphasis on repeatability (standard validation and deployment patterns across Kubernetes and Azure container platforms). A separate guide for AI agent solutions argued for containers as the default packaging for agent prototypes that need to become shareable services, shown via a multi-agent FastAPI app deployed to Azure Container Apps using `azd up` (Bicep provisioning, ACR build/push, Cosmos DB, managed HTTPS endpoint). This contrasts managed agent runtimes vs "bring your own container" when you need custom server behavior (SSE/WebSockets, Git ops, custom UI, bespoke orchestration) and complements last week's Container Apps migration write-up by reinforcing "containerize once, deploy consistently."

The weekly Azure update video bundled AKS operational items pointing toward more controlled networking and upgrades: more network logs/filtering, fleet cross-cluster networking, managed GPU metrics, meshless Istio routing options, blue-green agent pool upgrades, and an AKS networking AI agent concept for diagnosing network behavior, plus Arc-enabled Kubernetes recommended alerts for hybrid monitoring. Combined with last week's traffic-layer changes (AGC) and resiliency runbooks (Front Door fallbacks), the through-line is more observability and safer rollout primitives so teams can validate changes incrementally.

- [Announcing Public Preview of Argo CD extension on AKS and Azure Arc enabled Kubernetes clusters](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-public-preview-of-argo-cd-extension-on-aks-and-azure/ba-p/4504497)
- ['Containerization Assist: open-source tooling to accelerate app containerization'](https://www.youtube.com/watch?v=vKS6Uq-LLNs)
- [Containerization Assist - Simplifying Modern App Delivery](https://www.youtube.com/shorts/e9fniCosMDY)
- [Hosted Containers and AI Agent Solutions](https://techcommunity.microsoft.com/t5/microsoft-developer-community/hosted-containers-and-ai-agent-solutions/ba-p/4500627)
- [Azure Update 27th March 2026](https://www.youtube.com/watch?v=rz-7PWle174)

### Microsoft Fabric Data Factory & Dataflow Gen2: Performance, Automation, and Migration Paths

Fabric's integration surface saw updates that reduce friction in authoring/refresh performance, operational visibility, and programmatic lifecycle management. These changes read as follow-through on last week's Fabric modernization posture (migration assistants, safer defaults like triggers disabled post-migration, and more day-2 controls), now extended with performance work and monitoring at scale.

Dataflow Gen2 updates from FabCon Atlanta 2026 combined GA improvements and previews. GA items include Modern Query Evaluator (better refresh performance/predictability for complex Power Query M) and Preview Only Steps (iterate faster by keeping sampling/filters out of production refresh). For promotion, Variable Libraries (GA) and Relative References (GA) reduce hard-coded workspace bindings; scheduled runs now accept parameter values (GA) so one definition covers multiple scenarios. Ops additions include email alerts for failed refreshes (GA) and expanded destinations (GA) including ADLS Gen2 and Lakehouse Files, plus schema-aware targeting for supported destinations.

Automation accelerated, extending last week's agent-assisted ops thread (Data Factory MCP Server preview) into bulk and runtime primitives. A Save As API supports bulk migration from Dataflow Gen1 to Gen2, and an Execute Query (Streaming) API (preview) enables on-demand Power Query execution without full refresh, useful for debugging and event-driven patterns. The Data Factory MCP Server (preview) exposes Dataflow Gen2 and pipeline operations as MCP tools so assistants can drive create/test/deploy locally (positioned as running on your machine so credentials stay local).

Migration tooling is also becoming more assessment-first inside Azure Data Factory and Azure Synapse. Like last week's assistants (ADF/Synapse, Spark, SQL migrations), these flows classify pipelines as "Ready / Needs review / Coming soon / Unsupported," export CSV reports, and require mapping Linked Services to Fabric Connections. Triggers stay disabled after migration by default so teams can validate before re-enabling schedules, preserving a "parallel validate, then cut over" rhythm.

For performance and monitoring, Copy job added Auto-partitioning (preview) for large SQL-oriented sources by selecting partition columns/boundaries for parallel reads in watermark-based incremental scenarios. Lakehouse writes were sped up by disabling V-Order by default during ingestion (with an option to re-enable). Workspace Monitoring (preview) now streams Copy job run logs into a Monitoring Eventhouse automatically, making run/mapping diagnostics queryable via KQL/SQL and enabling Power BI dashboard templates. This aligns with last week's traceability/ops readiness push in data movement: "what happened in this run?" should be answerable without custom logging.

Legacy ETL is also addressed: "Invoke SSIS Package" (preview) lets Fabric pipelines run SSIS packages stored in OneLake without Azure-SSIS IR, with parameter/property overrides for environment deployments. For secure access, Fabric VNET Data Gateway reached GA for certificate-based auth and proxy support, common blockers in locked-down networks. Admin cleanup improves with Connection Recency (preview), adding "Last linked to items" and "Last credentials used" to connection metadata, making cleanup and credential rotation more explicit. This is an admin counterpart to last week's governance theme.

- [A wave of new Dataflow Gen2 capabilities at FabCon Atlanta 2026](https://blog.fabric.microsoft.com/en-US/blog/a-wave-of-new-dataflow-gen2-capabilities-at-fabcon-atlanta-2026/)
- [Upgrade your Synapse pipelines to Microsoft Fabric with confidence (Preview)](https://blog.fabric.microsoft.com/en-US/blog/upgrade-your-synapse-pipelines-to-microsoft-fabric-with-confidence-preview/)
- [New migration experience from Azure Data Factory to Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/new-migration-experience-from-azure-data-factory-to-fabric-preview/)
- [Higher Performance with Copy job in Fabric Data Factory Auto Partitioning (Preview)](https://blog.fabric.microsoft.com/en-US/blog/higher-performance-with-copy-job-in-fabric-data-factory-auto-partitioning-preview/)
- [Gain full visibility into your Copy jobs with Workspace Monitoring in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/gain-full-visibility-into-your-copy-jobs-with-workspace-monitoring-in-microsoft-fabric-preview/)
- [Invoke SSIS Package Activity in Microsoft Fabric Data Factory (Preview)](https://blog.fabric.microsoft.com/en-US/blog/invoke-ssis-package-activity-in-microsoft-fabric-preview/)
- [Certificate and Proxy Support for VNET Data Gateway (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/certificate-and-proxy-support-for-vnet-data-gateway-generally-available/)
- [Connection Recency for improved audit and management (Preview)](https://blog.fabric.microsoft.com/en-US/blog/connection-recency-for-improved-audit-and-management-preview/)

### Fabric APIs, Private Connectivity, and Java Integration

Fabric's developer surface expanded across workspace-scale automation, private networking posture, and supported integration for common runtimes. This builds on last week's OneLake/Fabric governance and platform-controls emphasis (OAP expansion, workspace firewalls, connect-without-copying patterns): once teams standardize on Fabric, they need repeatable deployment mechanics and private-by-default setups for regulated environments.

For CI/CD and automation, Bulk Import/Export Item Definitions APIs (preview) enable exporting/importing item definitions (JSON manifests with Base64 "parts") across many items, supporting workspace cloning, backups, scanning, and deployment pipelines. They use long-running operations (202 + polling via `/operations/{operationId}`) and include constraints teams must plan around: 128 MB payload limits, rate limiting/backoff (429 guidance), item coverage limited to what Git integration/definitions APIs support, and permission nuances (exports only include items where the caller has both read and write). For promotion pipelines, this is the missing bulk primitive complementing single-item create/update flows, aligning with last week's "migrations at scale" storyline.

On networking/compliance, tenant-level Private Link support for Fabric API for GraphQL is now GA, keeping GraphQL traffic private and compatible with "Block Public Internet Access" tenants without rebuilding APIs. Limitations matter: Workspace Monitoring-based API monitoring/logging is not supported in this Private Link setup, and service principals cannot create saved credentials for source auth, so headless automation may need alternate patterns. This reflects the broader tradeoff: private endpoints reduce exposure, but teams must design around visibility and automation gaps.

For app integration, the Microsoft JDBC Driver for Fabric Data Engineering is now GA, providing a supported JDBC 4.2 path for Java apps/tools to run Spark SQL against Lakehouse data via Fabric Livy APIs. It supports JDK 11/17/21, Entra ID auth (interactive, client creds, cert-based, access tokens), and production features (pooling/health recovery, HikariCP integration, retries/circuit breaker behavior, async result prefetching, proxy support, logging). For Java teams, this makes Fabric behave more like a standard JDBC target rather than a one-off integration.

- [Microsoft Fabric Bulk Import/Export Item Definitions APIs (Preview)](https://blog.fabric.microsoft.com/en-US/blog/public-apis-bulk-import-and-export-items-definition-preview/)
- [Tenant level private link support for Microsoft Fabric API for GraphQL (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/34710/)
- [Microsoft JDBC Driver for Microsoft Fabric Data Engineering (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/microsoft-jdbc-driver-for-microsoft-fabric-data-engineering-generally-available/)

### Streaming and Real-Time Messaging: MQTT, Kafka, and Fabric Eventing

Azure's managed streaming updates split between IoT-native MQTT ingestion and Kafka-compatible ingestion, with Fabric increasingly positioned as a downstream target. This continues last week's Fabric real-time push (Eventstreams DeltaFlow for CDC, more Eventhouse monitoring templates): the story is converging on ingest once, route to multiple targets, with Fabric RTI as a default alongside ADX and Event Hubs.

For IoT backends, the March 2026 Event Grid MQTT Broker update covered GA features (MQTT v3.1.1/v5.0, TCP/WebSockets, HTTP Publish over HTTPS, retained messages, LWT) and upcoming previews (shared subscriptions, larger packets, higher egress throughput, autoscale, IPv6, bulk onboarding APIs). Auth options include Entra ID/OIDC JWT, X.509/mTLS, and webhook custom auth, and it highlights assigned client identifiers for auditing/diagnostics in regulated identity scenarios. Architecturally, it supports direct routing into Microsoft Fabric Eventstreams (RTI) without requiring Event Hubs as a middle layer, alongside targets like ADX, Functions, Logic Apps, and Event Hubs. As Fabric RTI matures (last week's DeltaFlow; this week's UI improvements), Azure is reducing required "middle hops" for common pipelines.

In parallel, an Event Hubs analysis reiterated Kafka-compatible ingestion via Event Hubs' Kafka endpoint with tier guidance: Standard for common ingestion; Premium/Dedicated for Kafka transactions/exactly-once semantics and Kafka Streams, plus geo-replication options including RPO=0. It also reinforced Event Hubs as an ingestion hub that can land to ADLS via Capture and feed Fabric for near-real-time analytics, fitting a mixed pattern: direct-to-Fabric for some flows (MQTT Broker -> Eventstreams) and Event Hubs as buffer/compatibility for others.

Within Fabric, Eventstream authoring improved: Activator rule creation/management can now be done inside the Eventstream UI (preview), enabling conditions and actions without switching to Activator, while still allowing deeper testing and history in Activator. This matches Fabric's broader trend toward fewer context switches and more built-in operational primitives.

- ['Azure Messaging: What’s New (March 2026) — Event Grid MQTT Broker'](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/azure-event-grid-mqtt-broker-enterprise-grade-messaging-for-the/ba-p/4504246)
- ['"No-Ops" Kafka Experience: Why Event Hubs is Your Default Destination for Streaming on Azure'](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/quot-no-ops-quot-kafka-experience-why-event-hubs-is-your-default/ba-p/4481501)
- [Configure and manage Activator rules directly in Eventstream (Preview)](https://blog.fabric.microsoft.com/en-US/blog/configure-and-manage-activator-rules-directly-in-eventstream-preview/)

### Private Networking and Data Movement in Locked-Down Environments

A recurring pattern is "keep it private, keep it workable," especially where public endpoints are disallowed. These items extend last week's hybrid/governance guidance (Azure Local evaluation, Arc patterns, security foundations) with concrete failure modes and fixes for private endpoints and isolated networks.

Azure Maps added Private Endpoint support in public preview, allowing Maps traffic to stay on private IPs inside a VNet via Private Link. The key change is using the account-scoped endpoint format `https://{maps-account-client-id}.{location}.account.maps.azure.com`, resolvable privately via the Private DNS zone `privatelink.account.maps.azure.com`. For many apps this is a small configuration change, but often the difference between meeting production policy or being blocked.

An Azure Storage guide tackled a hard topology: syncing blobs between two storage accounts with public access disabled, private endpoints only, no shared private DNS zones, and no shared VNet connectivity. Server-side copy can fail with `403 - CannotVerifyCopySource`. The recommended approach is running AzCopy from a VM in the *target* environment, aligning DNS by adding A records in the target private DNS zone mapping both source and target `*.blob.core.windows.net` hostnames to their private endpoint IPs, and authenticating with managed identity + data-plane RBAC (Storage Blob Data Reader on source, Contributor on target). It includes PowerShell automation (install, per-container `azcopy sync` with `--compare-hash=MD5` and `--include-directory-stub=true`, plus CSV validation reports). It reinforces a recurring operational lesson: if DNS/identity are not aligned, private-only designs fail in ways diagrams do not reveal.

A hybrid/sovereign-cloud networking overview rounded this out with tradeoffs (ExpressRoute vs S2S VPN), routing and latency considerations, and Azure Arc connectivity in constrained networks, including Arc with Private Link, Azure Firewall explicit proxy (preview), and Arc Gateway for centralized egress control. It pairs with last week's VPN Gateway BGP timer mismatch note: in sovereign/hybrid setups, defaults and control-plane behaviors (timers, DNS, proxying) often drive reliability more than app code.

- [Azure Maps Adds Support to Private Endpoints (Preview)](https://techcommunity.microsoft.com/t5/azure-maps-blog/azure-maps-adds-support-to-private-endpoints-preview/ba-p/4505743)
- [Synchronizing Azure Storage Across Isolated Private Endpoint Networks](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/synchronizing-azure-storage-across-isolated-private-endpoint/ba-p/4504564)
- [Azure Hybrid Networking for Sovereign Cloud](https://www.thomasmaurer.ch/2026/03/azure-hybrid-networking-for-sovereign-cloud/)

### Developer Libraries, Serverless Messaging Patterns, and Observability

The March 2026 Azure SDK release includes changes that affect real code paths. Azure.Identity 1.19.0 for .NET can reference certs from the platform certificate store using `cert:/StoreLocation/StoreName/Thumbprint`, reducing the need to ship cert files. Rust Cosmos DB SDK updates add multi-region writes, transactional batch, and fault injection, plus breaking changes (new `CosmosClientBuilder`, new stream-based query iteration) and removal of `wasm32-unknown-unknown` support. Azure.AI.ContentUnderstanding 1.0.0 reached GA for .NET/JS/Python with stronger typing to reduce parsing boilerplate.

A practical Azure Functions guide showed how to avoid "retry the whole batch" behavior for Service Bus batch triggers by using per-message settlement: disable auto completion and explicitly complete/abandon/dead-letter/defer via `ServiceBusMessageActions` (examples for .NET isolated worker, Node.js/TypeScript, Python). For workloads where one poison message causes repeated reprocessing and duplicate side effects, this configuration change improves reliability and cost under failure. It fits last week's resilience thread: gains often come from explicit, granular failure handling rather than default retries.

For observability, Azure Monitor added a portal workflow to copy dashboards from "Dashboards with Grafana" into Azure Managed Grafana without exporting/importing JSON. This eases graduating from portal dashboards to Managed Grafana features (alerting, reporting, plugins, automation), extending last week's observability theme (standardized diagnostic settings, Grafana/Entra auth patterns) by reducing migration friction. On the Fabric side, a monitoring guide for Fabric Data Warehouse pulled together DMVs (live), Query Insights (historical), SQL Pool Insights (pressure/throttling), and the Capacity Metrics app, including a repeatable way to correlate capacity spikes to a specific SQL statement via Operation ID -> `distributed_statement_id` and then analyze regressions via `query_hash`. It reinforces the day-2 story: after migration, teams still need to explain cost and performance behavior.

- [Azure SDK Release (March 2026)](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-march-2026/)
- [Partial failure handling for Azure Functions Service Bus batch triggers with per-message settlement](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/take-control-of-every-message-partial-failure-handling-for/ba-p/4504893)
- [Copy dashboards from Dashboards with Grafana to Azure Managed Grafana](https://techcommunity.microsoft.com/t5/azure-observability-blog/copy-dashboards-from-dashboards-with-grafana-to-azure-managed/ba-p/4505710)
- [Mastering monitoring in Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/mastering-monitoring-in-microsoft-fabric-data-warehouse/)

### Other Azure News

Fabric capacity management added Capacity overage (preview): admins can opt in per capacity and set a 24-hour overage billing cap so workloads continue through compute spikes instead of hitting deeper throttling, with overage billed at 3x PAYG rates. This pairs with upcoming visibility updates in the Capacity Metrics app and capacity events in Real-Time Hub, and connects back to last week's Warehouse controls (Custom SQL Pools preview, ongoing work to reduce ingestion/statistics slowdowns). Fabric is adding knobs and telemetry needed to keep shared capacity predictable as usage grows.

Azure Storage users got a shortcut for periodic reporting: Blob Inventory manifests include `summary.objectCount` and `summary.totalObjectSize`, allowing total blob count and size reporting without processing full CSV/Parquet exports. This complements last week's "make day-2 easier" theme by reducing bespoke processing for routine ops questions.

Fabric RTI expanded into vertical scenarios. An industrial analytics integration describes Fusion Data Hub landing OT historian/edge telemetry into Fabric Eventhouse/KQL databases (handling late data, gaps, normalization). A partner workload preview, Capital Markets DataHub, describes an industry-specific Fabric workload built on the Extensibility Toolkit and public REST APIs, focused on connectors, canonical models, reconciliation, entitlements, and Azure AI Foundry integration. This continues last week's extensibility storyline (self-service workload publishing GA): Fabric is pairing platform primitives with packaged vertical workloads and partner experiences.

Azure VMware Solution shared a deep dive into an autonomous self-healing loop for certain NSX/vCenter control-plane failures, emphasizing dependency graphs, policy-gated remediation, idempotent/checkpointed playbooks, verification-based closure, and an append-only incident ledger. The theme matches last week's resilience items (Front Door durable config recovery, ACR deep health-based failover): deeper signals plus controlled remediation rather than shallow probes or manual runbooks alone.

Cosmos DB users got a date: Azure Cosmos DB Conf 2026 is a free 5-hour live stream on April 28 with sessions on distributed app architecture and performance tuning, plus Azure DocumentDB and the open source DocumentDB project. SQL developers also got a recap video from SQLCon/FabCon covering the SQL family, AI direction for SQL (including a SQL Developers Certification), and the Fabric Database Hub experience, another signal of how tightly Azure databases and Fabric's analytics layer are being positioned together week to week.

- [Introducing capacity overage flexibility when you need it most (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-capacity-overage-preview-flexibility-when-you-need-it-most/)
- [How to get total blob count and total capacity with Azure Blob Inventory](https://techcommunity.microsoft.com/t5/azure-paas-blog/how-to-get-blob-total-blob-count-and-total-capacity-with-blob/ba-p/4485643)
- ['Industrial Analytics delivered at-scale: Powered by Fabric Real-Time Intelligence and Fusion Data Hub'](https://blog.fabric.microsoft.com/en-US/blog/industrial-analytics-delivered-at-scale-powered-by-fabric-real-time-intelligence-and-fusion-data-hub/)
- ['Capital Markets DataHub workload (Preview) in Microsoft Fabric: connectors, canonical model, reconciliation, and Azure AI Foundry integration'](https://blog.fabric.microsoft.com/en-US/blog/unlocking-financial-insights-with-capital-markets-datahub-workload-a-partner-led-innovation-in-microsoft-fabric-preview/)
- [Autonomous Self-Healing for Azure VMware Solution Private Clouds](https://techcommunity.microsoft.com/t5/azure-migration-and/autonomous-self-healing-for-azure-vmware-solution-private-clouds/ba-p/4506374)
- [Azure Cosmos DB Conf 2026 | Live Stream](https://www.youtube.com/watch?v=OdPFriVuKtU)
- [What's New in Microsoft SQL at SQLCon/FabCon | Data Exposed](https://www.youtube.com/watch?v=9YN_7_yx7bs)

## .NET

This week's .NET updates focused on meeting developers where they are: keeping code-first workflows while widening where apps can run. It continues last week's two-track theme: maintain a stable production baseline (PowerShell 7.6 LTS on .NET 10, smoother VS Code Insiders tooling) while trying newer .NET 11 Preview 2 features (like MAUI Maps pin clustering). This week, that split shows up as Aspire getting a supported Azure deployment path and MAUI exploring broader targets via Avalonia's rendering stack.

### .NET Aspire on Azure App Service (GA)

Aspire on Azure App Service reached GA, taking Aspire from local orchestration to a deployable, AppHost-defined topology on App Service without abandoning the Aspire model. Like last week's focus on maintaining a known-good baseline, this GA gives teams already using AppHost locally a clearer production path with fewer conceptual jumps.

The core is the `Aspire.Hosting.Azure.AppService` NuGet package. You define your App Service environment in AppHost code alongside familiar local constructs: project references, health checks (for example, `WithHttpHealthCheck("/health")`), and endpoint exposure (`WithExternalHttpEndpoints()`). For teams standardizing on code-defined configuration, keeping the app's "shape" in AppHost reduces drift between local and hosted environments.

A practical GA detail is support for Deployment Slots, mapping to typical staging/production workflows. From AppHost you can declare a slot (for example, `.WithDeploymentSlot("dev")`) with behavior to note: if production does not exist, deployment creates production and the staging slot with identical config; if production exists, deployment targets only the staging slot. Scaling is supported manually (via AppHost code or portal), and rule-based scaling is available through Azure Monitor Autoscale, but fully automatic scaling is not part of Aspire-on-App-Service yet. Overall, this is a bridge from local composition to managed App Service while keeping AppHost as the source of truth for dependencies and topology.

- [Aspire on Azure App Service is now Generally Available](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/aspire-on-azure-app-service-is-now-generally-available/ba-p/4505549)

### .NET MAUI + AvaloniaUI backend (preview) for Linux and WebAssembly

Last week's .NET 11 Preview 2 notes highlighted MAUI control work (like Maps pin clustering). This week's MAUI update is about platform reach: a preview AvaloniaUI "backend" for .NET MAUI adds Linux desktop and WebAssembly targets. It's based on .NET 11 preview and is expected to remain preview until .NET 11 GA (projected around November), matching the "try it, expect churn" posture implied by last week's preview coverage.

The approach swaps rendering. Instead of only MAUI's native handler model, MAUI apps can render UI using Avalonia's custom-drawn control stack, either alongside or replacing MAUI controls. The trade is clear: keep the MAUI app model while gaining Avalonia's cross-platform reach (Windows, macOS, Linux, iOS, Android, WebAssembly) without rewriting into an Avalonia-first architecture. It also creates a continuation path: the same MAUI app can use new MAUI controls on mobile while experimenting with Linux/browser targets via the backend, though behavior/rendering may differ because the control stack changes.

The preview has constraints teams need to plan for. Microsoft MAUI API coverage is incomplete, especially around storage/media, so workarounds or waiting may be required. Linux relies on X11/XWayland rather than Wayland, which can block modern desktops. On Windows, it does not yet support hosting Avalonia controls inside WinUI (MAUI's Windows stack), limiting hybrid composition. Engineering from this backend is also feeding into Avalonia 12 controls/APIs and efforts to reduce MAUI/Avalonia control differences. For MAUI teams that need Linux desktop or browser targets, it's a workable option if you can tolerate preview gaps and cross-platform UI churn.

- [Avalonia adds Linux and WebAssembly targets to .NET MAUI (preview, .NET 11)](https://www.devclass.com/development/2026/03/24/avaloniaui-enhances-net-maui-with-linux-and-webassembly-support/5209515)

## DevOps

This week's DevOps updates focused on making automation more repeatable and less fragile. Fabric kept closing "treat artifacts like code" gaps (Git, pipelines, environment promotion), while GitHub and VS Code shipped workflow improvements that reduce triage overhead and tighten feedback loops. Infrastructure teams also got a heads-up on Docker storage behavior changes and a pattern for turning Helm chart expectations into CI-enforced tests.

### Microsoft Fabric CI/CD and “everything as code” (CLI, Git, pipelines, and environment promotion)

Fabric updates focused on turning UI-heavy operations into versioned, promotable artifacts across dev/test/prod. Building on last week's Git-shaped delivery push (bulk export/import, branched workspaces, selective branching, diff-style review), the Fabric Data Engineering API for GraphQL now supports source control and CI/CD as GA. GraphQL artifacts can now be committed, reviewed via PRs, and promoted using Azure DevOps or Fabric Deployment pipelines. The result is that API definition/config changes become diffable and rollback-friendly instead of portal-only state.

Fabric's database DevOps tooling also moved toward an end-to-end workflow. Extending last week's "deploy from VS Code / database projects" direction, updated guidance positions a SQL project (.sqlproj with Microsoft.Build.Sql) as the unit of versioning and deployment. Builds validate dependencies and produce a DACPAC; deployments generate a plan/script to align the target database. Fabric's portal can connect a SQL database in Fabric to GitHub/Azure DevOps, generate a SQL project from the current schema, and commit it so changes can flow through PRs/pipelines. Two pipeline-relevant details: (1) pre/post-deployment scripts are supported in SQL database in Fabric (authored as Shared Queries and marked), and (2) tooling is converging. SSMS 22.4 adds a "Database DevOps" workload (preview) using the same project system as VS Code, while the VS Code MSSQL extension publish dialog is now GA and can show the equivalent SqlPackage command for CI reuse.

Automation gained more building blocks that fit last week's "reduce special-case automation" theme. Fabric CLI v1.5 is GA with a new `deploy` command wrapping `fabric-cicd`, enabling full workspace deploys with one command (for example, `fab deploy --config deployment-config.yaml`) suitable for GitHub Actions/Azure DevOps. It also expands Power BI automation (rebinding, semantic model refresh scheduling/triggering), improves notebook execution (including running `fab` inside Fabric notebooks and parsing JSON), and adds agent-facing guardrails like `.ai-assets` and `AGENTS.md` to reduce invented flags in generated commands. A separate preview Azure DevOps Marketplace extension targets pipeline boilerplate by provisioning Fabric CLI via a `FabricCLI@1` task (PowerShell/PowerShell Core/Bash, version pinning), avoiding per-pipeline install scripting.

Notebook Git workflows also improved for promotion. Notebooks can now optionally commit/restore the Resources folder (Python modules, config files, small assets), with Fabric-side exclusion rules plus `.gitignore` support inside Resources. And Lakehouse auto-binding (preview) reduces manual rebinding of lakehouses across Git-connected workspaces by capturing config in a Fabric-managed `notebook-settings.json` (visible for auditing, not intended for manual editing).

- [API for GraphQL source control and CI/CD support (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/api-for-graphql-source-control-and-ci-cd-support-generally-available/)
- ['Batteries included: Database DevOps with SQL projects'](https://blog.fabric.microsoft.com/en-US/blog/batteries-included-database-devops-with-sql-projects/)
- ['Fabric CLI in Azure DevOps: automation without friction (Preview)'](https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-in-azure-devops-automation-without-friction-preview/)
- [Fabric CLI v1.5 is here (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-v1-5-is-here-generally-available/)
- ['Fabric Notebooks: Resources Folder Support in Git'](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebooks-resources-folder-support-in-git/)
- [Fabric notebooks support Lakehouse auto-binding in Git (Preview)](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebooks-support-lakehouse-auto-binding-in-git-preview/)

### GitHub Actions and PR workflow ergonomics (runner images, agentic automation, review triage)

GitHub shipped changes targeting two friction sources: inconsistent CI environments and review/maintenance overhead. Building on last week's runner fleet hygiene and Kubernetes scheduling improvements (ARC multilabel scale sets, safer listener defaults, controls to avoid stale runner reprovisioning), custom images for GitHub-hosted runners are now GA. Orgs can start from GitHub-curated images and bake in toolchains, dependencies, cert roots, and hardened config so workflows install less and fail less due to drift. The gains show up in performance (fewer setup steps) and governance (standardized approved versions). GitHub notes October 2025 preview users should continue without changes.

Agentic workflows are also being previewed as a way to define intent-driven automation in Markdown that runs in Actions via coding agents, for tasks like scheduled maintainer reports or proposing fixes for CI failures. Continuing last week's theme of adding boundaries/context for AI contributions, Actions run summaries now show the exact agentic workflow Markdown used for a run, making auditing/debugging easier without jumping between pages.

For human review flow, GitHub's pull requests dashboard is in public preview with an Inbox-style view (needs your review, needs fixing, ready to merge), saved views built from common queries, and richer search syntax (AND/OR, nested expressions) for cross-repo review queues. It follows last week's PR ergonomics updates: as controls/checks expand, GitHub is also reducing "what do I review next?" overhead.

- [Custom images for GitHub-hosted runners are now generally available](https://github.blog/changelog/2026-03-26-custom-images-for-github-hosted-runners-are-now-generally-available)
- [Automate your repo with GitHub agentic workflows](https://www.youtube.com/shorts/XH8oKA-ZYbU)
- [View Agentic Workflow configs in the Actions run summary](https://github.blog/changelog/2026-03-26-view-agentic-workflow-configs-in-the-actions-run-summary)
- [New pull requests dashboard is in public preview](https://github.blog/changelog/2026-03-26-new-pull-requests-dashboard-is-in-public-preview)

### Other DevOps News

Fabric's environment configuration story advanced for data integration, continuing last week's Variable Library theme for environment rewiring during promotions. Dataflow Gen2 Variable Library integration is now GA, letting teams externalize environment-specific values (endpoints, IDs, paths, thresholds, flags) and resolve them at runtime instead of editing Power Query/M per environment. This supports promoting the same logic across dev/test/prod under CI/CD with centrally governed configuration.

- ['Dataflow Gen2: Variable Library integration in Microsoft Fabric (Generally Available)'](https://blog.fabric.microsoft.com/en-US/blog/dataflow-gen2-variable-library-integration-in-microsoft-fabric-generally-available/)

Gateway lifecycle automation became more pipeline-friendly with the Gateway PowerShell module reaching GA for on-premises and VNet data gateways. Following last week's note on admin-triggered auto-update GA, this moves from "upgrade on demand" to "script upgrades and recovery as runbooks." New/updated cmdlets cover upgrade/recovery tasks like checking versions, pinning upgrade targets, polling update status, and restoring a cluster member, reducing reliance on portal workflows.

- [Gateway PowerShell module is now generally available, with new update and recovery commands](https://blog.fabric.microsoft.com/en-US/blog/powershell-module-for-gateways-with-expanded-automation-capabilities-generally-available/)

Container operators got a troubleshooting note for Docker Engine v29 on Linux. With containerd image store enabled by default on fresh installs, Docker's `data-root` no longer prevents OS disk growth because images/snapshots go under `/var/lib/containerd`. The workaround is relocating/symlinking containerd storage to a data disk in addition to setting Docker's `data-root`. This matters for build agents, VMSS workers, and batch nodes where OS disk saturation impacts availability, especially when standardizing CI environments (like GitHub runner custom images) and needing worker disk behavior to match assumptions.

- ['Docker Engine v29 on Linux: Why data-root No Longer Prevents OS Disk Growth (and How to Fix It)'](https://techcommunity.microsoft.com/t5/azure/docker-engine-v29-on-linux-why-data-root-no-longer-prevents-os/m-p/4504862#M22466)

A Helm chart testing pattern showed how to unit test charts with Terratest by rendering (`helm template`) with base + environment override values, unmarshalling into typed Kubernetes API objects in Go, and asserting on fields (labels/selectors, securityContext hardening, ingress/TLS, HPA bounds). Using typed structs reduces brittle YAML-path assertions, and the post includes an Azure DevOps pipeline pattern for running tests and publishing JUnit results. This aligns with last week's "policy as enforceable primitives" direction: chart invariants can be enforced as tests rather than review-only expectations.

- ['Unit Testing Helm Charts with Terratest: A Pattern Guide for Type-Safe Validation'](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unit-testing-helm-charts-with-terratest-a-pattern-guide-for-type/ba-p/4506165)

Two smaller workflow updates may still affect automation. VS Code Insiders 1.114 adds `${taskVar:name}` so task output (captured via problem matchers) can feed launch/debug configs, helping with dynamic ports/URLs. GitHub also added a repo setting to disable comments on individual commits (via REST and GraphQL), which may require adjustments if bots/CI currently post commit-level comments instead of PR comments, especially as teams reduce review noise with improved PR dashboards and review surfaces.

- ['Visual Studio Code 1.114 (Insiders): pinned chat sessions and new task variables'](https://code.visualstudio.com/updates/v1_114)
- [Disable comments on individual commits (GitHub repository setting)](https://github.blog/changelog/2026-03-25-disable-comments-on-individual-commits)

## Security

This week's security story centered on CI/CD trust and identity/data control. A real supply-chain compromise hit developer pipelines, while GitHub and Microsoft shared concrete steps to reduce drift: dependency locking, tighter secret scope, faster feedback, and more platform-enforced policy. It also continues last week's theme: defenders are adding guardrails to default paths (dependency installs, workflow triggers, org rollouts) where attackers keep showing up.

### GitHub Actions supply-chain defense: from a real compromise to platform-level hardening

Microsoft's incident guidance on the Trivy supply-chain compromise was the most urgent item. A malicious Trivy binary (called out as v0.69.4) and compromised actions (`aquasecurity/trivy-action`, `aquasecurity/setup-trivy`) were used to steal pipeline credentials. The attacker force-pushed version tags (76 of 77 tags for `trivy-action`, all 7 for `setup-trivy`) so workflows pinned to mutable tags like `@v1` could silently run attacker code. On self-hosted runners, the payload performed broad discovery and secret harvesting (cloud env vars, AWS IMDS/ECS metadata endpoints, Kubernetes service account mounts and `kubectl get secrets --all-namespaces -o json`, scans for `.env`/YAML/JSON, webhook URLs, SSH logs, Vault/DB strings), packaged results (`tpcp.tar.gz`) with hybrid encryption (AES-256-CBC + RSA), and exfiltrated via HTTP POST to `scan.aquasecurtiy[.]org`, then ran the legitimate scan to keep jobs green. Mitigations: move to the listed safe versions (Trivy v0.69.2-v0.69.3, `trivy-action` v0.35.0, `setup-trivy` v0.2.6), pin third-party actions to verified commit SHAs, tighten `GITHUB_TOKEN` permissions, restrict allowed actions via org policy, and reduce runner secret exposure (ephemeral runners/clean environments, JIT secret retrieval). Microsoft also provided Defender Advanced Hunting KQL for indicators like the exfil domain, encryption commands, and runner artifact paths.

This context makes GitHub's Actions 2026 security roadmap read like a response to the same failure modes, extending last week's "shift checks earlier, govern rollouts" thread into workflow execution. GitHub plans workflow-level dependency locking via a `dependencies:` section that locks direct/transitive action dependencies to immutable commit SHAs plus cryptographic hashes, compared to Go's `go.mod`/`go.sum`. The intended flow is CLI-based: resolve dependencies, commit lock data, and update via re-resolve + diff review; jobs would fail if hashes mismatch, and composite actions would expose nested dependencies. GitHub also plans rulesets for policy-driven workflow execution (who can trigger, which events are allowed), with examples like restricting `workflow_dispatch` and avoiding `pull_request_target` in favor of `pull_request`, plus an evaluate mode before enforcement. "Scoped secrets" would reduce implicit inheritance (notably with reusable workflows) by binding secrets to explicit contexts, and secret management is planned to move into a dedicated role instead of generic write access. Finally, GitHub is adding runner visibility/containment: an Actions Data Stream for near-real-time telemetry to Amazon S3 and Azure Event Hub/Azure Data Explorer, and a native Layer 7 egress firewall for GitHub-hosted runners with monitor/enforce modes designed to hold even if an attacker gets root in the runner VM. The through-line matches last week's security/observability direction: visibility plus enforceable boundaries under stress.

- [Guidance for detecting, investigating, and defending against the Trivy supply chain compromise](https://www.microsoft.com/en-us/security/blog/2026/03/24/detecting-investigating-defending-against-trivy-supply-chain-compromise/)
- [What’s coming to our GitHub Actions 2026 security roadmap](https://github.blog/news-insights/product-news/whats-coming-to-our-github-actions-2026-security-roadmap/)

### GitHub Code Security: faster PR scanning, broader detections, and more control over secret blocking

GitHub's app security updates focused on making checks faster in PRs while expanding coverage. Continuing last week's "earlier feedback with smoother rollout" theme, CodeQL PR scanning now uses incremental analysis by default for C#, Java, JavaScript/TypeScript, Python, and Ruby. It builds a CodeQL database for the PR diff, combines it with a cached full-repo database, and reports seven-day average speedups that are most noticeable in slower repos (JavaScript/TypeScript up to ~70% for >7-minute baselines; Python ~70%; Ruby ~63%). Constraints: it applies to the default query suite and "build mode none" extraction on github.com, and CodeQL CLI does not support the incremental flow yet.

GitHub is also preparing AI-powered security detections (public preview planned early Q2 2026) to complement CodeQL by covering ecosystems and file types that do not map cleanly to semantic SAST. Initial targets are Shell/Bash, Dockerfiles, Terraform (HCL), and PHP, with findings surfaced in the same PR experience as CodeQL. It tracks with last week's AI security operations framing: if teams rely on AI interpretation/remediation, signals must still be observable and governable in standard workflows. GitHub also plans to connect detections to Copilot Autofix so developers can review/apply suggestions in PRs and gate merges via policy enforcement.

Secret Scanning push protection gained a new policy surface: push protection exemptions can now be configured in a repository's settings, not only at org/enterprise level. This follows last week's central exemption controls by adding repo-level flexibility for the last mile of rollout. Repo admins can exempt Roles, Teams, and GitHub Apps so pushes are not blocked when secrets are detected, with an explicit tradeoff: exemptions are evaluated at push time and exempt pushes will not generate bypass requests, so teams should align exemptions with audit expectations.

- [Faster incremental analysis with CodeQL in pull requests](https://github.blog/changelog/2026-03-24-faster-incremental-analysis-with-codeql-in-pull-requests)
- [GitHub expands application security coverage with AI-powered detections](https://github.blog/security/application-security/github-expands-application-security-coverage-with-ai-powered-detections/)
- [Push protection exemptions from repository settings](https://github.blog/changelog/2026-03-23-push-protection-exemptions-from-repository-settings)

### Microsoft Fabric security: network controls, encryption keys, and Purview-driven data protection

Fabric's security updates added enforceable controls that do not require app rewrites, while expanding API support for automation and governance. It extends last week's Fabric/OneLake security direction: keep OneLake/Fabric as a consistent enforcement point as more tools and AI features touch data. Workspace-level IP firewall rules are now GA, allowing workspace admins to restrict inbound access via public IP allowlists once tenant admin enables it. Workspace scope matters: production can be locked to corporate egress IPs while dev stays flexible, layering with Private Link, Entra Conditional Access, outbound protection, and RBAC. This matters for CI/CD, notebooks, Spark, and external services calling Fabric APIs from controlled networks.

Customer-managed keys (CMK) for Fabric SQL Database are now GA, configured at the workspace level and integrated with TDE. Once enabled, TDE is automatically on for all SQL databases in that workspace (including `tempdb`), encrypting data, logs, and backups. Operational focus shifts to Key Vault lifecycle (permissions, rotation, audit), and the post includes verification via `sys.dm_database_encryption_keys` to confirm `encryption_state_desc` is `ENCRYPTED` (or in progress) and `encryptor_type` is `ASYMMETRIC_KEY`, consistent with Key Vault-backed protectors.

Data protection additions leaned into Purview integration. DLP "restrict access" for OneLake (Preview) expanded to structured OneLake stores (SQL databases, KQL databases, Warehouses), enabling policy-based detection to automatically restrict access across more of the estate. Sensitivity labels are now accessible via Fabric public REST APIs (GA) through Core Items APIs (label IDs surfaced in List/Get/Update and supported on Create), enabling automated inventory and compliant creation, though label updates still require admin bulk label APIs. Purview Insider Risk Management added Fabric Lakehouse indicators (GA) and a faster IRM Data Theft policy creation path (GA), plus a PAYG usage report (GA) for processing-unit costs by workload/indicator. For teams using Fabric Copilot and data agents, Purview DSPM for AI (Preview) adds monitoring for sensitive info in prompts/responses with IRM investigation hooks and governance via audit/eDiscovery/retention, continuing last week's governance-and-observability pattern for day-to-day data work.

- [Workspace level IP firewall rules in Microsoft Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/workspace-level-ip-firewall-rules-in-microsoft-fabric-generally-available/)
- [Customer-managed keys (CMK) in Fabric SQL Database (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-cmk-in-fabric-sql-database-generally-available/)
- ['New data protection capabilities in Microsoft Fabric: Native security for the modern data estate'](https://blog.fabric.microsoft.com/en-US/blog/new-data-protection-capabilities-in-microsoft-fabric-native-security-for-the-modern-data-estate/)

### Other Security News

GitHub's supply-chain data and IR tooling became more actionable for day-to-day ops. GitHub Security Lab's 2025 open source vulnerability trends recommended using CVSS alongside EPSS to prioritize what is likely to be exploited soon, noted increased npm malware advisories, and tied that to Dependabot's ability to alert on known malicious npm package versions. This continues last week's Dependabot malware-aware additions and can help with tuning alert triage. GitHub also expanded the unauthenticated Credential Revocation API to support revoking exposed OAuth app tokens and GitHub App credentials (including refresh tokens) in bulk, with rate limits (60 requests/hour; up to 1,000 tokens/request) and audit visibility via the token owner's security log. This fits the "tighten trusted surfaces" theme from last week's token warnings: rotate/revoke quickly and do not treat token internals as stable contracts.

- ['A year of open source vulnerability trends: CVEs, advisories, and malware'](https://github.blog/security/supply-chain-security/a-year-of-open-source-vulnerability-trends-cves-advisories-and-malware/)
- [Credential revocation API now supports GitHub OAuth and GitHub app credentials](https://github.blog/changelog/2026-03-26-credential-revocation-api-now-supports-github-oauth-and-github-app-credentials)

Microsoft identity and Defender updates emphasized "context + automatic containment," reinforcing last week's identity-first intrusion story. The identity-security analysis argued attackers are shifting from account compromise to exploiting what identities can reach, including non-human and emerging agent identities, and described Microsoft's approach across Entra (control plane + Conditional Access) and Defender XDR (threat protection). Updates include an Identity Security dashboard, unified identity risk score, adaptive risk remediation, and a Security Copilot triage agent for identity investigations. Microsoft also described using High Value Asset (HVA) context from the Security Exposure Management graph to be more aggressive on Tier-0 and internet-facing workloads, with examples like blocking `ntdsutil.exe` credential dumping on domain controllers and fast remediation of targeted webshells on Exchange/SharePoint/IIS. Together with last week's mitigations (phishing-resistant MFA, limiting remote support tools), the thread is consistent: assume initial access looks normal, then use context-aware policy and rapid disruption to stop pivots.

- [Identity security is the new pressure point for modern cyberattacks](https://www.microsoft.com/en-us/security/blog/2026/03/25/identity-security-is-the-new-pressure-point-for-modern-cyberattacks/)
- [How Microsoft Defender protects high-value assets in real-world attack scenarios](https://www.microsoft.com/en-us/security/blog/2026/03/27/microsoft-defender-protects-high-value-assets/)

AI agent governance guidance sharpened around enforceable intent, identity, and runtime checks, building on last week's move from theory to operations (observability + Zero Trust mapping). Microsoft's agent governance model proposed a precedence order (organizational intent overrides role-based intent, which overrides developer intent, which overrides user intent) so "what the agent is allowed to do" becomes evaluable and auditable. Complementing that, Azure AI Foundry guidance described implementing agents as Entra-managed identities (service principals), scoping access via Azure RBAC (for example, Storage Blob Data Reader for read-only summarization), and applying guardrails at user input, tool call (preview), tool response (preview), and output to block prompt-injection exfiltration attempts before tools execute.

- ['Governing AI agent behavior: Aligning user, developer, role, and organizational intent'](https://techcommunity.microsoft.com/blog/microsoft-security-blog/governing-ai-agent-behavior-aligning-user-developer-role-and-organizational-inte/4503551)
- ['Securing Azure AI Agents: Identity, Access Control, and Guardrails in Microsoft Foundry'](https://techcommunity.microsoft.com/t5/microsoft-developer-community/securing-azure-ai-agents-identity-access-control-and-guardrails/ba-p/4500242)

Resilience and ransomware defense rounded out the week with administrator-focused examples. A Defender case study showed how "predictive shielding" combined attack disruption with temporary GPO hardening to pause propagation of new GPO policies after detecting tampering, blocking a ransomware attempt staged via SYSVOL (`run.bat`, `run.exe`, `run.dll`) and a scheduled task chain (`cmd /c start run.bat -> ...run.exe -> rundll32 ...run.dll Encryptor`). A video overview of Entra Backup and Restore described daily snapshots of Entra state plus a diff report to understand changes before restoring, positioned as a safety net alongside soft delete and protected actions for recovering from accidental or malicious identity config changes that break authentication/app access. In the context of the two-week identity focus, it's a reminder that identity control includes recovery, not only prevention.

- ['Case study: How predictive shielding in Defender stopped GPO-based ransomware before it started'](https://www.microsoft.com/en-us/security/blog/2026/03/23/case-study-predictive-shielding-defender-stopped-gpo-based-ransomware-before-started/)
- [Overview of Entra Backup and Recovery](https://www.youtube.com/watch?v=72nowrDIlQU)
