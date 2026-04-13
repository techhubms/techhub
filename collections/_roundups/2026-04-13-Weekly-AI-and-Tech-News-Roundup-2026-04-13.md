---
title: "Agents Across Copilot: Controls, MCP Tooling, and Safer Operations Loops"
author: "TechHub"
date: 2026-04-13 09:00:00 +00:00
tags: ["GitHub Copilot","Copilot CLI","VS Code Agents","Copilot Cloud Agent","Model Context Protocol (MCP)","BYOK","Local Models","Azure AI Foundry","Foundry Local","Microsoft Entra ID","Managed Identity","OpenTelemetry","Application Insights","GitHub Actions","DevSecOps"]
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
external_url: "/all/roundups/Weekly-AI-and-Tech-News-Roundup-2026-04-13"
---

Welcome back to this week's roundup. The main thread is that agents are showing up in more places, and teams are getting clearer ways to control how those agents run. Updates across Copilot (IDE, CLI, cloud agent, and mobile) focused on practical autonomy controls, offline/BYOK routing, cross-model review checkpoints, and security remediation loops that end in reviewable pull requests. In parallel, MCP and Azure AI Foundry updates continued to reinforce "run it like software" basics: deployable tool surfaces with real auth, consistent runtimes across cloud and local, and clearer observability and identity boundaries for day-two operations.

<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Copilot in VS Code: more autonomous agents, richer debugging, and better customization](#copilot-in-vs-code-more-autonomous-agents-richer-debugging-and-better-customization)
  - [Copilot CLI: BYOK/local models, cross-model review, plugins, and MCP tool connections](#copilot-cli-byoklocal-models-cross-model-review-plugins-and-mcp-tool-connections)
  - [MCP and agent UI inside Copilot: interactive apps, dashboards, and Azure execution guardrails](#mcp-and-agent-ui-inside-copilot-interactive-apps-dashboards-and-azure-execution-guardrails)
  - [Copilot agents in GitHub workflows: mobile coding, faster validation, and security remediation loops](#copilot-agents-in-github-workflows-mobile-coding-faster-validation-and-security-remediation-loops)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Microsoft Foundry: GA agent runtime, SDK 2.0 shifts, and the “cloud + local” story](#microsoft-foundry-ga-agent-runtime-sdk-20-shifts-and-the-cloud--local-story)
  - [MCP on Azure: from tool hosting to authentication patterns and self-hosted automation](#mcp-on-azure-from-tool-hosting-to-authentication-patterns-and-self-hosted-automation)
  - [Agent operations in practice: observability, real-time UIs, and guarded automation](#agent-operations-in-practice-observability-real-time-uis-and-guarded-automation)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Azure Machine Learning + Azure DevOps: a repeatable training-to-endpoint MLOps pipeline](#azure-machine-learning--azure-devops-a-repeatable-training-to-endpoint-mlops-pipeline)
  - [Microsoft Fabric: real-time ingestion/security upgrades and cleaner warehouse SQL](#microsoft-fabric-real-time-ingestionsecurity-upgrades-and-cleaner-warehouse-sql)
- [Azure](#azure)
  - [Managed identities and least-privilege automation across Azure platforms](#managed-identities-and-least-privilege-automation-across-azure-platforms)
  - [Kubernetes and App Service operations: tracing, alert automation, and simpler deployments](#kubernetes-and-app-service-operations-tracing-alert-automation-and-simpler-deployments)
  - [Reliability and private networking: DNS pitfalls, multi-region choices, and Key Vault continuity](#reliability-and-private-networking-dns-pitfalls-multi-region-choices-and-key-vault-continuity)
  - [Other Azure News](#other-azure-news)
- [.NET](#net)
  - [.NET data access to Microsoft Fabric Spark (ADO.NET preview driver)](#net-data-access-to-microsoft-fabric-spark-adonet-preview-driver)
  - [Blazor in .NET 11: validation model previews (async rules, localization, render-mode flexibility)](#blazor-in-net-11-validation-model-previews-async-rules-localization-render-mode-flexibility)
  - [Other .NET News](#other-net-news)
- [DevOps](#devops)
  - [GitHub Actions, Dependabot, and platform reliability: tighter guardrails and broader ecosystem support](#github-actions-dependabot-and-platform-reliability-tighter-guardrails-and-broader-ecosystem-support)
  - [Azure cost-aware IaC pipelines and agentic operations: shifting governance earlier and into runtime](#azure-cost-aware-iac-pipelines-and-agentic-operations-shifting-governance-earlier-and-into-runtime)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Identity- and token-centric attacks: AiTM, device-code phishing, and the edge as the new MITM](#identity--and-token-centric-attacks-aitm-device-code-phishing-and-the-edge-as-the-new-mitm)
  - [DevSecOps and supply chain workflow updates: prioritize what’s deployed, reduce secrets, and streamline fixes](#devsecops-and-supply-chain-workflow-updates-prioritize-whats-deployed-reduce-secrets-and-streamline-fixes)
  - [Platform identity, data governance, and agent runtime controls: shrinking blast radius across Azure, Fabric, and agent stacks](#platform-identity-data-governance-and-agent-runtime-controls-shrinking-blast-radius-across-azure-fabric-and-agent-stacks)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week's Copilot story was less about one headline and more about Copilot being available in more places: stronger agent controls in VS Code and GitHub Mobile, deeper terminal workflows through Copilot CLI (including offline/BYOK), and more admin/reporting to track adoption and outcomes. It follows last week's theme that as Copilot grows from chat/autocomplete into branch/PR agents, multi-agent CLI orchestration, and MCP-backed tooling, GitHub is closing gaps in control (permissions, firewall/runner placement), traceability (sessions/logs/telemetry), and administration (instructions and usage reporting).

### Copilot in VS Code: more autonomous agents, richer debugging, and better customization

Building on last week's VS Code work (session restore with diffs/undo, background terminal completion notices, integrated browser context, and SSH agent host mode), the March-early April releases (v1.111-v1.115) expanded agent sessions, starting with per-session **Agent permissions** to control autonomy. Sessions can run as **Default**, **Bypass Approvals**, or **Autopilot** (public preview). Autopilot lets the agent approve its own actions, retry on errors, and continue until completion, so you can run longer multi-step work with fewer prompts when that fits the task. It continues the "resumable and auditable sessions" direction from last week, but with an explicit autonomy choice up front.

VS Code workflow changes also go beyond agents. The editor now supports **integrated browser debugging** through a new `editor-browser` debug type that works with many existing Chrome/Edge configurations. With self-signed cert support for local HTTPS and improved tab management (Quick Open, Close All, title-bar shortcuts), web debugging can happen with fewer context switches. This connects to last week's "integrated browser as session context" idea: debugging context and "agent saw these tabs" are starting to land in the same editor-native loop.

On models, reasoning models now have a persistent **"thinking effort"** control in the model picker (for example, Claude Sonnet 4.6 and GPT-5.4). That makes it easier to trade speed/cost for deeper reasoning depending on the task. It mirrors last week's model churn point: explicit, repeatable per-session settings (and now "effort") help teams keep outcomes steadier as model options change. Copilot Chat also gained more multimodal support: you can attach **screenshots and videos**, and agents can return images/recordings in a carousel (zoom/pan/thumbnails). That is useful for UI bugs and visual diffs, and it extends last week's "blob attachments" work in the Copilot SDK preview.

Extensibility and team-scale management also moved forward. **Nested subagents** can invoke other subagents (`chat.subagents.allowInvocationsFromSubagents`), MCP integration has better parity (MCP servers configured in VS Code are available to Copilot CLI and Claude agent sessions), and **session forking** makes it easier to try alternatives without losing the original thread. Customization and diagnostics improved as well: a **Chat customizations editor** (preview) centralizes instructions/agents/skills/plugins plus marketplace browsing, the `#codebase` tool moved to one auto-managed semantic index, and `/troubleshoot` can analyze agent debug logs (including past sessions) to explain issues like ignored instructions or slow responses. Security and governance also improved with **sandboxing for local MCP servers** (macOS/Linux), better monorepo discovery for shared instructions/agents/hooks, and **agent-scoped hooks** (preview) via YAML frontmatter in `.agent.md`. Overall, this reads like the IDE-side follow-through on last week's scaling story: last week expanded repo/org instruction patterns, and this week makes them easier to manage, safer to run, and easier to debug in VS Code.

- [GitHub Copilot in Visual Studio Code, March Releases](https://github.blog/changelog/2026-04-08-github-copilot-in-visual-studio-code-march-releases)

### Copilot CLI: BYOK/local models, cross-model review, plugins, and MCP tool connections

This week's Copilot CLI updates extend last week's "CLI as an agent runtime" direction, where `/fleet` introduced multi-agent orchestration and the Copilot SDK standardized sessions, tool invocation, and tracing. The CLI is moving from "Copilot in the terminal" to a configurable, extensible agent platform with review flows. The main change is that Copilot CLI now supports **BYOK providers and fully offline local-model workflows**. You can route to providers you already use (Azure OpenAI, Anthropic, OpenAI-compatible endpoints) or run locally (Ollama, vLLM, Foundry Local). For regulated or restricted environments, `COPILOT_OFFLINE=true` prevents any GitHub server contact (telemetry disabled) and forces the CLI to use only the configured provider, which helps avoid accidental cloud fallback. GitHub sign-in is optional under BYOK, though GitHub-backed features (like `/delegate`, Code Search, and the GitHub MCP server) still require GitHub connectivity and identity. The CLI also will not silently fall back on invalid configuration, which helps keep CI and locked-down machines more predictable. In practice, last week's "agent sessions need controls" now includes provider/connectivity control, not only model choice.

For developers testing BYOK, the LM Studio walkthrough makes the "local OpenAI-compatible endpoint" path concrete and calls out a common pitfall: if you do not set `COPILOT_OFFLINE=true`, you may still allow cloud routing. The Azure AI Foundry (Azure OpenAI) setup shows the hosted alternative with its own details: you must use the **deployment-scoped URL** (`.../openai/deployments/<deployment>/v1`) and set `COPILOT_MODEL` to the *deployment name*, not the catalog model name. This "details matter" theme matches last week's `/fleet` guidance (separate contexts, shared filesystem, collision avoidance). As CLI use becomes more agent-driven, configuration edge cases matter as much as prompting.

For agent quality, Copilot CLI added an experimental "Rubber Duck" workflow that **reviews an orchestrator model's output using a different model family** at checkpoints (plan review, post-implementation review, test review, or when stuck). In the current experiment, using a Claude model as orchestrator triggers GPT-5.4 as reviewer. The goal is to catch plan mistakes and cross-file edge cases earlier in longer runs without constant interruption. This pairs with last week's "plan first" additions (cloud agent drafting plans, SDK demo emphasizing iterative planning). Rubber Duck turns the "builder vs critic" idea into a structured flow by separating models.

The "GitHub Checkout" episode added more context on the CLI's experimental direction: **plugins and a plugin marketplace**, `/chronicle` for capturing "self-healing" task context across longer runs, and background execution like `/fleet` and "autopilot" for wider refactors. For tool integration, the beginner MCP episode focuses on using `/mcp` with local/remote MCP servers (for example, Playwright and Svelte) so Copilot CLI can invoke tools in an iterative loop rather than only suggesting commands. This continues last week's MCP thread (tools + enforcement + audit) but shifts emphasis from "connect tools" to "make the terminal agent reliably use them."

Finally, the "Copilot CLI for Beginners" post reiterates the core workflow: install (`npm install -g @github/copilot`), `/login`, folder permissions, and `/delegate` to hand a task to the cloud agent to work in a branch and open a draft PR. That sets up the deeper BYOK/MCP/agent features described above. The tie to last week's branch-first cloud agent story is explicit: the CLI is increasingly the "remote control" for branch/PR agent workflows, now with more options for models and tools.

- [GitHub Copilot CLI now supports BYOK and local models](https://github.blog/changelog/2026-04-07-copilot-cli-now-supports-byok-and-local-models)
- [Using GitHub Copilot CLI with Local Models (LM Studio)](https://dev.to/playfulprogramming/using-github-copilot-cli-with-local-models-lm-studio-5e3b)
- [Using GitHub Copilot CLI with Azure AI Foundry (BYOK Models) – Part 2](https://dev.to/playfulprogramming/using-github-copilot-cli-with-azure-ai-foundry-byok-models-part-2-4e5n)
- [GitHub Copilot CLI adds Rubber Duck for cross-model plan and code review (experimental)](https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-combines-model-families-for-a-second-opinion/)
- [Copilot CLI update: chronicle, plugins, and fleet mode | GitHub Checkout](https://www.youtube.com/watch?v=9oAcwmrUE44)
- [How to use MCP servers with GitHub Copilot CLI (Beginner Tutorial)](https://www.youtube.com/watch?v=DtQjVIRRszM)
- [GitHub Copilot CLI for Beginners: Getting started with GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-for-beginners-getting-started-with-github-copilot-cli/)
- [Turning a codebase into an 80s dungeon crawler with Copilot CLI](https://www.youtube.com/watch?v=WekOupjeM6E)

### MCP and agent UI inside Copilot: interactive apps, dashboards, and Azure execution guardrails

This month's MCP work moved in three directions: interactive UI inside Copilot Chat, MCP as a deployment surface, and MCP as a governance boundary for infrastructure actions. It continues last week's MCP narrative (connect tools, then operationalize with versioned config and constrained capabilities), with more attention on what users can see in the client and how ops teams can bound what agents are allowed to do.

For "UI inside chat," the Azure App Service tutorial walks through building an **MCP App** that pairs tools with UI resources so clients like **VS Code Copilot Chat** can render an interactive widget inline (also compatible with Claude Desktop and ChatGPT). The sample is an ASP.NET Core/.NET 9 MCP server with a weather tool. UI is delivered via a `ui://` resource and returned via MCP UI metadata (`_meta.ui.resourceUri`) plus `text/html;profile=mcp-app`. It covers packaging tool + UI, running locally through `.vscode/mcp.json`, and deploying with `azd up` to App Service using Bicep templates, including App Service settings that matter for interactive calls (Always On, Easy Auth with Entra ID, deployment slots, Application Insights). It also links back to last week's azd "Set up with GitHub Copilot" work: azd is becoming a common path for scaffolding and shipping MCP-backed agent apps.

For "visual output," Visuals MCP added a **chart tool** that can render charts or multi-card dashboards in Copilot Chat in VS Code, with Storybook examples for payload validation. With support for line/bar/area/pie/scatter/composed charts, layouts, legends/tooltips/gridlines, dual-axis, theme awareness, and CSV/JSON plus JPG/SVG exports, teams can keep analysis and visualization inside the editor. This complements last week's "deep research" and "cited Markdown report" patterns: once research outputs become artifacts, charts and dashboards are a natural next step for reviewable results without leaving the session.

For "platform actions with guardrails," the platform engineering walkthrough shows Copilot agents packaged in a repo (Azure's `git-ape`) and wired to the **Azure MCP Server** so agents execute Azure workflows through an explicitly enabled surface. The example runs Azure MCP in namespace mode, allows writes (`azureMcp.readOnly: false`), and whitelists services like `deploy`, `group`, `subscription`, `functionapp`, `storage`, `sql`, and `monitor`. That reinforces a pattern where natural-language infra work flows through constrained, reviewable capabilities. This matches last week's governance framing (versioned playbooks and enforcement) and mirrors GitHub-side guardrails (cloud agent firewall and runner controls): agents can act, but only through explicitly allowed surfaces.

- [Build and Host MCP Apps on Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-and-host-mcp-apps-on-azure-app-service/ba-p/4509705)
- [Visuals MCP Update: Charts and Dashboards in VS Code](https://harrybin.de/posts/visuals-mcp-charts-dashboard-update/)
- [Putting Agentic Platform Engineering to the test](https://devblogs.microsoft.com/all-things-azure/putting-agentic-platform-engineering-to-the-test/)

### Copilot agents in GitHub workflows: mobile coding, faster validation, and security remediation loops

This week's workflow updates follow directly from last week's GitHub.com cloud agent changes (branch-first work without opening a PR, plan-first and deep research sessions, commit signing, plus org firewall/runner controls). The agent is quicker at completing safety checks, fits better into security remediation, and is accessible from more places, especially mobile.

GitHub tightened the "agent wrote code" to "safe to merge" loop. Copilot cloud agent validations now run about **20% faster** by running checks in parallel: CodeQL, secret scanning, GitHub Advisory Database checks, and Copilot code review. That shortens iteration time without changing what is enforced, and repo owners can still tune validations in Copilot -> Cloud agent settings. Combined with last week's session logs/validation traceability and signed commits, this helps agent work fit more naturally into normal CI timing.

For remediation, Dependabot alerts can now be **assigned to AI coding agents** from an alert detail page. The agent reviews the advisory and repo usage, opens a **draft PR** with a fix, and tries to resolve test failures. Teams can assign multiple agents in parallel to compare approaches (upgrade paths, breaking-change refactors, or downgrades when no patch exists). The intent stays the same: the output is a PR for humans to review. This complements last week's in-IDE remediation note (Visual Studio fixing vulnerable NuGet packages) by moving the loop directly into the security product.

Copilot also expanded into mobile. GitHub Mobile now supports a cloud agent flow to research a repo, draft a plan, apply changes on a branch, review diffs, and optionally open a PR from the app. It extends last week's branch-first workflow and makes small fixes and follow-ups more realistic away from a laptop, especially with last week's mobile session logs and issue-based agent assignment.

- [Copilot cloud agent’s validation tools are now 20% faster](https://github.blog/changelog/2026-04-10-copilot-cloud-agents-validation-tools-are-now-20-faster)
- [Dependabot alerts can now be assigned to AI coding agents for remediation](https://github.blog/changelog/2026-04-07-dependabot-alerts-are-now-assignable-to-ai-agents-for-remediation)
- [GitHub Mobile: Research and code with Copilot cloud agent anywhere](https://github.blog/changelog/2026-04-08-github-mobile-research-and-code-with-copilot-cloud-agent-anywhere)

### Other GitHub Copilot News

Copilot adoption and governance became more measurable for admins through several usage metrics API updates. This continues last week's admin thread (org-wide custom instructions GA; per-user Copilot CLI activity in org reports) but shifts from "who used Copilot?" to "which workflows changed, and what happened?" Copilot CLI activity is now included in totals and feature breakdowns (`feature=copilot_cli`), which simplifies reporting but means dashboards that assumed totals were IDE-only may need updates to avoid double counting. The API also adds **merged PRs that were Copilot-reviewed** and **median time-to-merge for Copilot-reviewed PRs**, which moves measurement beyond "Copilot created PRs" to "Copilot reviewed PRs." For maturity tracking, user-level reports now split **active vs passive Copilot Code Review**, and enterprise/org reports add daily/weekly/monthly active user counts for the renamed **Copilot cloud agent** (with null-vs-0 behavior ETL needs to handle). Together with last week's CLI activity reporting, it is easier to separate chat usage, terminal automation, and agent execution in rollups.

- [Copilot CLI activity now included in usage metrics totals and feature breakdowns](https://github.blog/changelog/2026-04-10-copilot-cli-activity-now-included-in-usage-metrics-totals-and-feature-breakdowns)
- [Copilot-reviewed pull request merge metrics now in the usage metrics API](https://github.blog/changelog/2026-04-08-copilot-reviewed-pull-request-merge-metrics-now-in-the-usage-metrics-api)
- [Copilot usage metrics now identify active and passive Copilot code review users](https://github.blog/changelog/2026-04-06-copilot-usage-metrics-now-identify-active-and-passive-copilot-code-review-users)
- [Copilot usage metrics now aggregate Copilot cloud agent active user counts](https://github.blog/changelog/2026-04-10-copilot-usage-metrics-now-aggregate-copilot-cloud-agent-active-user-counts)

Security teams also got a more direct workflow: **Ask Copilot** is now available inside Code Security's risk assessment and secret risk assessment results, so admins can open Copilot from a finding to get explanation and remediation guidance without leaving the UI. This fits the "shorter remediation loop" pattern from last week's Copilot Autofix positioning and this week's agent-assigned Dependabot alerts. Copilot is being placed closer to the security queue, not only the coding surface.

- [Ask Copilot in security assessments now available](https://github.blog/changelog/2026-04-09-ask-copilot-in-security-assessments-now-available)

Modernization and long-lived codebases also came up this week. The Copilot Modernization assessment workflow is positioned as the repo's "source of truth" for Assess -> Plan -> Execute migrations for .NET and Java to Azure. It stores reports in `.github/modernize/assessment/` and feeds IaC generation (Bicep/Terraform), containerization, and deployment manifests via the VS Code extension (GA) or the Modernize CLI (public preview) with multi-repo assessments. This builds on last week's modernization content by turning plans into versioned repo artifacts that tools and agents can act on. Microsoft also announced **ASP.NET Core 2.3 end of support on April 7, 2027**, which is a cue to schedule upgrades (for example, toward .NET 10 LTS) and points to Copilot App Modernization as one assessment option.

- [Your Migration’s Source of Truth: The Modernization Assessment](https://devblogs.microsoft.com/dotnet/your-migrations-source-of-truth-the-modernization-assessment/)
- [ASP.NET Core 2.3 end of support announcement](https://devblogs.microsoft.com/dotnet/aspnet-core-2-3-end-of-support/)

Teams building repeatable agent workflows got some useful patterns. Spec-Kit's extensions system shows how to extend spec-driven development with commands and automation, including the "Ralph Loop" that repeatedly runs a Copilot-based agent against `tasks.md`, produces reviewable commits, and tracks context in `progress.md`. It matches last week's "configuration scales" and "repo hygiene" guidance by turning agent work into a predictable loop with reviewable artifacts. On the data/dev side, the Data Exposed episode showed how to make Copilot-generated T-SQL more consistent using **custom instructions**, **Plan Mode** (PRD -> schema plan), **Agent Mode** with the MSSQL extension's Schema Designer against a real DB, and a **skills file** exposed as a slash command, plus checking what context was sent through the Copilot debug panel. That ties back to last week's observability focus: once teams rely on instructions and skills, they need a way to verify what the model received.

- [Spec-Kit Extensions: Making spec-driven development your own](https://hiddedesmet.com/speckit-extensions)
- [Next-Level SQL in VS Code: GitHub Copilot Custom Instructions, Plan Mode & Skills | Data Exposed](https://www.youtube.com/watch?v=noEj1AhZUwE)

A couple of access and cost/ops notes may affect rollout planning. GitHub is enforcing updated Copilot limits with separate "service reliability" vs "model capacity" behaviors (often mitigated by switching models or using Auto mode), and it is retiring **Opus 4.6 Fast** from Copilot Pro+. This reinforces last week's reminder that model availability can change, and deprecations and quota behavior can affect standardized model plans and agent throughput. GitHub also paused new Copilot Pro trials due to trial abuse, pointing evaluators to Copilot Free or paid subscriptions for now. For budgeting context, Microsoft's "Budget Bites" finale discussed AI, cost constraints, and database choices (Azure SQL and Hyperscale), and framed Copilot/agents as a way to reduce repetitive operational work.

- [Enforcing new limits and retiring Opus 4.6 Fast from Copilot Pro+](https://github.blog/changelog/2026-04-10-enforcing-new-limits-and-retiring-opus-4-6-fast-from-copilot-pro)
- [Pausing new GitHub Copilot Pro trials](https://github.blog/changelog/2026-04-10-pausing-new-github-copilot-pro-trials)
- [AI, Budget, and the Future of Databases](https://www.youtube.com/watch?v=vc2kcT-75J8)

## AI

This week's AI updates pushed in two directions: more "agent runtime + tools + governance" building blocks reaching GA, and clearer paths to operationalize them (local models, MCP tool wiring with real auth, and agent-specific observability/grounding patterns that can work in production). It continues last week's "run it like software" framing: stable runtimes, inspectable tool contracts, and day-two controls (cost, identity, evaluation, safety) becoming the default.

### Microsoft Foundry: GA agent runtime, SDK 2.0 shifts, and the “cloud + local” story

Foundry's March update reads like consolidation. Foundry Agent Service is now GA and built on the OpenAI Responses API, which keeps agents wire-compatible while adding enterprise needs like private networking (BYO VNet with no public egress, subnet/container injection), Entra RBAC, tracing, and production-focused evaluations/monitoring. It builds on last week's Agent Framework/Copilot Studio direction: orchestration and governance are moving toward platform capabilities (versionable, observable, enforceable) instead of being rebuilt per application.

For Azure AI Projects SDK users, workflows are increasingly centered on `AIProjectClient`, and SDKs moved to 2.0 stable across Python/JS/TS/Java (with .NET 2.0 shortly after on April 1). That also means migration work: packages and namespaces changed (for example, `AIProjectClient.OpenAI` -> `ProjectOpenAIClient` in .NET, plus renames in JS/TS and Java). Teams should plan the upgrade rather than treating it as a last-minute patch. This matches last week's theme that stable contracts matter, and now the "contract" is often the SDK surface and Responses API shape.

On models and routing, Foundry's catalog continues to expand. GPT-5.4 (GA) is positioned for production agent reliability (tool calling, fewer mid-run failures, better multi-step stability) with up to 272K context and cached-input pricing. GPT-5.4 Mini (GA) targets high-volume classification/extraction and lightweight tool calls. Additions like Phi-4 Reasoning Vision 15B and Fireworks AI integration for open models/BYOW broaden options for "right model per step" architectures. This fits last week's cost/ops thread: once you route by step, provider choice and model tier become routine engineering settings.

Foundry Local reaching GA completes the "cloud + local" story. It keeps an OpenAI-compatible API while moving inference to device/edge. It follows last week's Foundry Local offline blueprints (RAG vs CAG) by pairing local-first patterns with a supported GA runtime: OpenAI-compatible schemas, local caching/streaming, and acceleration selection. The flow is designed to be straightforward: add a thin SDK wrapper (JS/Python/.NET/Rust), build pulls in Foundry Local Core and ONNX Runtime native bits, first run downloads a hardware-optimized model from the catalog, then runs fully offline with caching and token streaming. It chooses acceleration automatically (GPU/NPU with CPU fallback), supports OpenAI-compatible chat completions and audio transcription (plus Open Responses API format), and can run without a local HTTP server unless you enable an OpenAI-compatible endpoint. Platform specifics include WinML on Windows (with OS-managed execution provider/plugin acquisition), Metal on Apple Silicon, and support for Linux/macOS/Windows.

Overall, Foundry's message is less about one new feature and more about "these are the runtimes and SDK shapes to build on": cloud-hosted agents with production controls plus a local runtime that preserves message schemas and tool calling across deployments. It follows the same continuity thread as last week: standardize runtimes, keep tools and governance enforceable, and make local vs cloud a deployment choice rather than a rewrite.

- [What’s new in Microsoft Foundry | March 2026](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-mar-2026/)
- [Foundry Local is now Generally Available](https://devblogs.microsoft.com/foundry/foundry-local-ga/)

### MCP on Azure: from tool hosting to authentication patterns and self-hosted automation

MCP continued to take shape as the connection layer between agents and real systems, with progress focused on making MCP deployments look like supportable services rather than one-off demos. This builds on last week's MCP "tooling glue" theme (Functions hosting, VS Code loops, governance-aware access). The emphasis is shifting from "you can host a tool" to "here is how you secure it, version it, and run it as a shared capability."

For UI-capable MCP "Apps" on Azure Functions, there are now two paths. The TypeScript quickstart shows the serverless workflow: define tools via `app.mcpTool()` and UI resources via `app.mcpResource()` (`text/html;profile=mcp-app`), run locally with `func start`, validate with MCP Inspector, and deploy with `azd` plus Bicep to `/runtime/webhooks/mcp`. In .NET, the Functions MCP extension added a fluent API (preview) to hide brittle protocol wiring between tools and UI. `AsMcpApp(...)` generates the synthetic UI resource function, sets the MIME type, and aligns `_meta.ui` bindings. It also moves security into code: explicit permissions (for example, clipboard read/write), CSP allowlisting, static asset hosting with source maps excluded by default, and visibility controls to keep UI renderable while hiding tools from model tool selection. This matches last week's "remote HTTPS endpoints plus identity" intent, but it pushes safer defaults up into the framework so teams need less custom glue.

Once tools are hosted, safe calling becomes the next focus. The Foundry integration walkthrough focuses on Functions-hosted MCP servers as reusable backends: one server can serve IDE clients (VS Code, Visual Studio, Cursor, etc.) and Foundry agents using the same tool schema. The main value is the auth decision tree, continuing last week's identity options but with concrete fields: function keys for shared-secret access, Entra managed identity when agents act as themselves (agent identity preferred for production; project managed identity often OK for dev), OAuth passthrough for per-user permissions and auditing, and unauthenticated only for public or dev cases. It also lists the specific Foundry fields (audience/App ID URI, tenant endpoints, scopes like `user_impersonation`) to reduce portal guesswork.

Azure MCP Server 2.0 reaching stable is the other key MCP update. It is an open-source MCP server that exposes Azure operations as structured tools (276 tools across 57 services), with 2.0 focusing on remote/self-hosted deployment. This matches last week's "governed tool surfaces" direction. Instead of each dev running local tool defaults, you can run Azure MCP as a centrally managed internal service with consistent tenant/subscription defaults, telemetry policy, and network boundaries. It supports managed identity (with guidance for Foundry-adjacent setups) and an OBO pattern via OpenID Connect delegation to operate in the signed-in user's context. The release also highlights hardening (endpoint validation, injection protections for query-style tools), container image improvements, and sovereign cloud support. These are day-two details that fit treating tools as production infrastructure.

For Entra-authenticated MCP servers (especially with a pre-authorized client like VS Code), the Entra guide is explicit. Since Entra does not support MCP's CIMD/DCR flows today, the recommendation is pre-registration and pre-authorization: register the MCP server as a protected resource (API), define a delegated scope (for example, `user_impersonation`), configure `requested_access_token_version=2`, and add VS Code as a `pre_authorized_application` to avoid extra consent prompts. FastMCP validates JWTs using Entra public keys (no client secret needed for validation), and middleware captures the user `oid` so tools can enforce per-user auth/storage. For downstream calls (like Microsoft Graph) in user context, it shows the OBO flow, including admin consent and a managed-identity plus federated identity credential setup for Azure Container Apps. This is the identity glue implied by last week's hosting patterns, now written out as a workable recipe.

- [Announcing Azure MCP Server 2.0 Stable Release for Self-Hosted Agentic Cloud Automation](https://devblogs.microsoft.com/azure-sdk/announcing-azure-mcp-server-2-0-stable-release/)
- [Give your Foundry Agent Custom Tools with MCP Servers on Azure Functions](https://devblogs.microsoft.com/azure-sdk/give-your-foundry-agent-custom-tools-with-mcp-servers-on-azure-functions/)
- ['MCP Apps on Azure Functions: Quickstart with TypeScript'](https://devblogs.microsoft.com/azure-sdk/mcp-apps-on-azure-functions-quickstart-with-typescript/)
- ['MCP as Easy as 1-2-3: Introducing the Fluent API for MCP Apps'](https://devblogs.microsoft.com/azure-sdk/mcp-as-easy-as-1-2-3-introducing-the-fluent-api-for-mcp-apps/)
- [Building MCP servers with Entra ID and pre-authorized clients](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-mcp-servers-with-entra-id-and-pre-authorized-clients/ba-p/4508453)

### Agent operations in practice: observability, real-time UIs, and guarded automation

As teams move from "an agent that works" to "an agent we can run," two themes stood out: visibility into agent behavior and explicit control points for risky actions. It continues last week's operational thread (Agent Framework 1.0 plus Copilot Studio GA hooks, plus SRE Agent prerequisites/billing). Once agents touch real systems, you need traceability, approval gates, and cost/usage visibility built into the loop.

For observability, Application Insights' new Agents view (preview) shows the platform adapting to agent-focused telemetry. The walkthrough instruments a .NET multi-agent "travel planner" on Azure App Service with OpenTelemetry GenAI semantic conventions so App Insights can answer agent-shaped questions: token usage per agent, per-agent latency/error rate, and end-to-end traces across an API plus WebJob split. The Agents view requires the right GenAI attributes (for example, `gen_ai.agent.name`). It demonstrates two instrumentation layers: `Microsoft.Extensions.AI` for LLM-call spans (token/model/provider attrs) and Microsoft Agent Framework (MAF) for agent-identity spans used for per-agent grouping. It also calls out a tradeoff: double instrumentation can duplicate spans, so you may choose identity grouping vs token detail depending on what you need most. This matches last week's "inspectable and governable" stance: instrumentation is part of system design, not an afterthought.

For making behavior visible and controllable, the AG-UI plus MAF demo streams multi-agent execution events to a live frontend over SSE so users can see which agent is active, what step is running, and why it is waiting. This echoes last week's streaming patterns in Foundry Local samples. Production agents need explicit progress/state signaling. The backend uses an explicit handoff graph (declared routing edges) and interrupts for user-info requests and human approval of sensitive tools. Marking tools with `approval_mode="always_require"` forces an interrupt instead of execution, and the React UI renders an approval modal with tool name/args before resuming. This "declared topology plus interruptible tools plus real-time events" matches last week's versioned orchestration and moderated tool integration, expressed as runtime plus UX.

For day-two ops, the Well-Architected operational excellence discussion reinforced a practical sequence: start with observability (OpenTelemetry), then use AI to summarize incidents and suggest next steps, and only move toward automation once guardrails, evaluation, and human-in-the-loop controls exist for high-impact actions. This aligns with last week's SRE Agent framing: the constraint is not whether an agent can act, but whether you can prove what happened, limit blast radius, and sustain it on-call.

- [Monitor AI Agents on App Service with OpenTelemetry and the New Application Insights Agents View](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/monitor-ai-agents-on-app-service-with-opentelemetry-and-the-new/ba-p/4510023)
- [Building a Real-Time Multi-Agent UI with AG-UI and Microsoft Agent Framework Workflows](https://devblogs.microsoft.com/agent-framework/ag-ui-multi-agent-workflow-demo/)
- [Use AI to Achieve Operational Excellence with the Well-Architected Framework practices](https://www.youtube.com/watch?v=PRpYptDTe4o)

### Other AI News

Data and analytics teams got two options for making data easier to use with agents without adding a separate AI pipeline layer. Fabric Data Warehouse introduced built-in AI functions (preview) that run directly in T-SQL for JSON extraction (`ai_extract`), sentiment (`ai_analyze_sentiment`), classification (`ai_classify`), summarization/translation/grammar fixes, and a prompt-based escape hatch (`ai_generate_response`) that you can wrap in UDFs/stored procedures to standardize prompts. This extends last week's Fabric "trusted data for AI" story by moving agent-enablement into the governed warehouse surface where permissions, lineage, and operational controls already exist.

A related "intelligence platforms" guide described an enterprise agent architecture: unify access with OneLake, enforce meaning/permissions with Fabric semantic models (measures/RLS), expose governed NL querying via Fabric Data Agents (preview), and connect that to Azure AI Foundry agents as a reusable tool, optionally enriched with Microsoft Graph context. It continues last week's governance-and-metadata theme: grounding tends to be more reliable when it is a permission-aware tool call over curated semantics, rather than copied text in prompts.

- [Working with unstructured text in Fabric Data Warehouse with built-in AI functions (Preview)](https://blog.fabric.microsoft.com/en-US/blog/working-with-unstructured-text-in-fabric-data-warehouse-with-built-in-ai-functions-preview/)
- [Why data platforms must become intelligence platforms for AI agents (with Microsoft Fabric + Azure AI Foundry)](https://techcommunity.microsoft.com/t5/microsoft-developer-community/why-data-platforms-must-become-intelligence-platforms-for-ai/ba-p/4505653)

Legacy modernization got a concrete agent example. An IIS migration guide showed using an MCP server to orchestrate Microsoft's IIS-to-App-Service migration scripts with human approvals, producing artifacts like `install.ps1`, adapter ARM templates, and `MigrationSettings.json`. It highlights Managed Instance on App Service specifics (PremiumV4 with `IsCustomMode=true`, plus OS dependencies like COM, MSMQ/SMTP, registry, drive-letter storage). This is a practical "MCP as governed automation interface" pattern: wrap existing scripts as typed tools, host them remotely, and require explicit approval before provisioning billable resources. That is similar to last week's remote-only MCP endpoints and identity boundaries.

- [Agentic IIS Migration to Managed Instance on Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/agentic-iis-migration-to-managed-instance-on-azure-app-service/ba-p/4508969)

On developer workflow, a short Cozy AI Kitchen episode showed design-to-code MCP: wiring a Figma MCP server into VS Code so design artifacts become structured context. The practical goal is fewer handoff cycles by referencing the real source of truth (Figma components/layout metadata) instead of text descriptions, reinforcing the wider shift across these roundups toward tools over prompts.

- [Setting Up Figma MCP Server in VS Code](https://www.youtube.com/shorts/noehsI6cAEc)

## ML

This week's ML thread was about shipping models and data products with fewer operational surprises. Azure ML plus Azure DevOps guidance went deep on repeatable training-to-serving pipelines and the details that tend to break CI/CD. Fabric continued last week's "operationalize the platform" momentum, focusing this time on real-time ingestion security and smoother warehouse querying to reduce glue work once systems move past prototype.

### Azure Machine Learning + Azure DevOps: a repeatable training-to-endpoint MLOps pipeline

An end-to-end template showed how to take a scikit-learn model from local training to reliable serving on Azure ML Managed Online Endpoints using an Azure DevOps multi-stage YAML pipeline. It splits into four stages (DevOps gate -> Train -> Register -> Deploy) so teams can validate and capture metadata early, retrain only when needed, and rerun register/deploy without retraining after transient failures.

In Train, the example standardizes the environment (Python 3.12), pulls data from Azure Blob Storage (CSV/Parquet via `adlfs`/`pyarrow` patterns), and adds basic validation (schema and row counts) before feature engineering and fitting (with `StandardScaler` as the example preprocessing). The output is one serialized artifact: a pickle bundle containing the estimator, fitted preprocessor, expected feature column order, and metadata (timestamps, row counts, scikit-learn version) to prevent silent mismatches and manage pickle compatibility.

Register uses the Azure ML CLI (`az extension add -n ml`, then `az ml model create`) to push the artifact into an Azure ML Registry, using auto version incrementing for re-registers under the same model name. Deploy then creates/updates a Managed Online Endpoint and deploys a specific model version (example: "blue" with all traffic) using `az ml online-endpoint create/show` and `az ml online-deployment create`, and finishes with a smoke test via `az ml online-endpoint invoke` to confirm the endpoint is callable.

It also covers operational details that determine whether this works in a team setting: managed-endpoint scoring script structure (`init()` loading from `AZUREML_MODEL_DIR`, `run()` enforcing feature order, applying the stored scaler, returning predictions), tradeoffs among `pickle`, `joblib`, and ONNX, and a warning on untrusted pickle deserialization. On DevOps/security, it reinforces no secrets in code (env vars/variable groups), managed identity over keys/secrets, least-privilege RBAC, and sample roles (Storage Blob Data Reader, AzureML Registry User, AzureML Data Scientist), plus workload identity federation from Azure DevOps to a user-assigned managed identity. It also flags pitfalls (Windows agent command differences, checkout behaviors, schema mismatches) and suggests extensions like validation gates, batch endpoints, drift monitoring, environment promotion, and blue/green or traffic-splitting.

- [Building an End-to-End MLOps Pipeline: From Training to Managed Endpoints on Azure](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-an-end-to-end-mlops-pipeline-from-training-to-managed/ba-p/4509852)

### Microsoft Fabric: real-time ingestion/security upgrades and cleaner warehouse SQL

Fabric's ML-adjacent updates focused on improving ingestion and querying that feed scoring, enrichment, and analytics, continuing last week's push to reduce bespoke operations. Last week emphasized orchestration surfaces and recovery. This week targets streaming ingestion (networking, certs, retries, fewer embedded secrets) and warehouse SQL ergonomics to reduce production friction.

In Eventstreams (Q1 2026 recap), ingestion expanded and Spark handoff tightened. New preview connectors include DeltaFlow for converting DB CDC events (inserts/updates/deletes) into structured streams, which reduces manual CDC format/schema/destination work. MQTT enhancements add v3.1 and v3.1.1 support to onboard existing brokers/device fleets without upgrades. Anomaly Detection also appears as a preview source, making anomaly signals first-class streaming inputs for routing/enrichment with telemetry. For teams following last week's orchestration theme, these broaden what becomes "just another input" into repeatable pipelines, especially where CDC/anomaly feeds must land reliably before downstream scoring/feature updates.

Eventstreams also improved processing integration with Spark Structured Streaming and Fabric Notebooks to reduce setup friction: discover Eventstreams through Real-Time Hub, auto-generate PySpark connection snippets, and reuse shared notebooks within Eventstreams. Operationally, it pushes safer defaults by reducing embedded connection strings/SAS keys and adding notebook auto-retry policies to restart streaming jobs after failures. This fits last week's "recover fast" theme by adding resilience settings for long-running streams and reducing secret sprawl.

Enterprise connectivity and security also advanced with preview private network ingestion for VNet/on-prem sources using an Azure managed virtual network bridge, supporting VPN, ExpressRoute, peering, private endpoints, and a streaming VNet data gateway experience. Connector security added preview custom CA certificates and mutual TLS (mTLS), with certs stored in Azure Key Vault for centralized rotation. This is called out for Kafka sources including Apache Kafka, Amazon MSK, Confluent Cloud for Apache Kafka, and Confluent Schema Registry. It matches last week's "platform-managed" posture: connectivity and cert rotation move into managed config and Key Vault-backed rotation rather than custom code.

Separately, Fabric Data Warehouse shipped GA support for T-SQL `ANY_VALUE()` as an aggregate/analytic function, which addresses a common reporting and semantic-layer pain point. It returns an arbitrary representative value per `GROUP BY` group (or window partition) when projected columns are functionally dependent on the grouping key. For example, you can group revenue by `GeographyID` while including `City`, `State`, `Country` without expanding the `GROUP BY`. It is clearer than `MIN()`/`MAX()` workarounds and can reduce unnecessary grouping columns, with the guardrail that it is only valid when values are constant in the group. Paired with last week's recovery work, it is another everyday production SQL/ops edge being improved.

- [What’s new in Fabric Eventstream: 2026 Q1 Edition](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-eventstream-2026-q1-edition/)
- [Use ANY_VALUE() for simpler grouping of results in Fabric Data Warehouse (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/use-any_value-for-simpler-grouping-of-results-in-fabric-data-warehouse-generally-available/)

## Azure

Azure's updates this week leaned toward making production operations less brittle, continuing last week's theme of controlled transitions and day-two readiness. Identity continues shifting away from long-lived secrets, ops tooling continues emphasizing "observe first, automate safely," and app hosting continues smoothing runtime upgrades and practical deployment paths. Architecture guidance stayed grounded in scale realities: DNS as a hard dependency in private-first designs and DR choices aligned to real RTO/RPO needs.

### Managed identities and least-privilege automation across Azure platforms

Azure Red Hat OpenShift (ARO) reached an identity/governance milestone: Azure Managed Identities (for platform/operators) and Microsoft Entra Workload Identity (for pods/apps) are now GA. It fits last week's identity/governance thread by moving ARO components off one broadly permissioned service principal and onto multiple user-assigned managed identities, each mapped to dedicated built-in ARO roles so RBAC can be scoped per component. Provisioning is supported via portal (including an all-in-one flow), ARM/Bicep, and `az aro` CLI (Azure CLI 2.84.0+). For workloads, Workload Identity uses OIDC federation to map a Kubernetes service account to a user-assigned managed identity so pods can request short-lived tokens for narrowly scoped access (Key Vault, Storage, Azure SQL) without in-cluster secrets. A key transition note remains: legacy service-principal clusters cannot migrate in-place. You need a new managed-identity cluster and then migrate workloads.

The same move away from secrets shows up in App Service CI/CD guidance. A walkthrough shows deploying to Azure Web Apps from Azure DevOps using an ARM service connection authenticated via a user-assigned managed identity (UAMI). The flow is simple: assign Website Contributor on the app (or resource group), create the service connection with UAMI auth, and verify who deployed via App Service audit logs (AppServiceAuditLogs), where the initiator is the UAMI object ID. It also notes that setup-time interactive sign-in may appear in logs, so "who set it up" can differ from "which identity deployed."

Hybrid onboarding got a similar least-privilege update with a new Azure Arc onboarding role for Ansible. It matches last week's "standardize the baseline" message by letting teams onboard Arc-enabled servers through existing Ansible playbooks with more tightly scoped permissions, which helps with fleet-scale onboarding.

- [Azure Red Hat OpenShift: Managed Identity and Workload Identity now generally available](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-red-hat-openshift-managed-identity-and-workload-identity/ba-p/4504940)
- [Deploying to Azure Web App from Azure DevOps Using UAMI](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploying-to-azure-web-app-from-azure-devops-using-uami/ba-p/4509800)
- [Simplify Azure Arc Server Onboarding with Ansible and the New Onboarding Role](https://techcommunity.microsoft.com/t5/azure-arc-blog/simplify-azure-arc-server-onboarding-with-ansible-and-the-new-onboarding-role/ba-p/4509481)

### Kubernetes and App Service operations: tracing, alert automation, and simpler deployments

AKS troubleshooting got a service-mesh tracing pattern that reinforces last week's "constraints plus observability" message. The guide correlates Istio/Envoy access logs with Application Insights using W3C Trace Context. It enables Istio Telemetry in the managed Istio system namespace, then uses EnvoyFilter resources (inbound/outbound) to emit structured JSON logs to stdout so they land in Log Analytics (ContainerLogV2). The practical win is correlation: parse the `traceparent` header from Envoy logs, extract the 32-hex trace id, and match it to App Insights `operation_Id` via KQL to follow one request across mesh hops and app telemetry without adding another tracing stack.

For incident response, Azure SRE Agent's Azure Monitor integration continued last week's day-two focus. It ingests fired alerts through the Azure Alerts Management REST API ("Get all alerts"), handles multiple alert types, and routes matches into Incident Response Plans in "review" or "autonomous" mode. The demo (AKS Node.js app failing Redis auth due to a bad secret) shows polling every 60s, acknowledging alerts, investigating logs, fixing the secret by retrieving the right Redis key, rolling pods, and resolving. A key nuance is merging: repeated firings from the same rule merge into one active thread (7-day window) rather than creating new incidents, which interacts with whether a rule uses auto-resolve. The advice to keep auto-resolve OFF for persistent failures (bad creds, CrashLoopBackOff-like issues) matters if you want one investigation per ongoing problem.

App Service got two developer-facing improvements that reduce friction at opposite ends of the workflow, building on last week's hosting direction. PHP 8.5 is now supported on App Service for Linux in all public regions, including better fatal error backtraces and language features like the pipe operator (`|>`). For quick deployment, App Service for Linux now offers a simpler zip deployment flow in Kudu/SCM: drag-and-drop a zip, preview contents, choose whether to run a server-side build, and track Upload/Build/Deployment progress with logs and runtime log links. It targets quick tests and initial setup, while still pointing teams to CI/CD for repeatability.

- [Service Mesh-Aware Request Tracing in AKS with Istio and Application Insights](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/service-mesh-aware-request-tracing-in-aks-with-istio-and/ba-p/4509928)
- [Azure Monitor in Azure SRE Agent: Autonomous Alert Investigation and Intelligent Merging](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-monitor-in-azure-sre-agent-autonomous-alert-investigation/ba-p/4509069)
- [PHP 8.5 is now available on Azure App Service for Linux](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/php-8-5-is-now-available-on-azure-app-service-for-linux/ba-p/4510254)
- [A simpler way to deploy your code to Azure App Service for Linux](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/a-simpler-way-to-deploy-your-code-to-azure-app-service-for-linux/ba-p/4510240)

### Reliability and private networking: DNS pitfalls, multi-region choices, and Key Vault continuity

A hub-spoke incident write-up reinforced a point that extends last week's private networking items: DNS is Tier-0, especially with custom hub DNS, centralized egress (firewall/NVA), Private Endpoints (OpenAI, AI Search, Key Vault, Storage), and Azure Container Apps (ACA) with internal ingress. The symptoms looked like inconsistent platform behavior (ACA startup/scaling issues, Terraform stalls, endpoint reachability gaps, secret/auth problems) but came down to DNS gaps: custom resolvers not forwarding to 168.63.129.16, missing conditional forwarders for privatelink zones (`privatelink.openai.azure.com`, `privatelink.vaultcore.azure.net`, `privatelink.search.windows.net`, `privatelink.blob.core.windows.net`), incomplete private DNS zone links across VNets/subscriptions, and missed ACA internal ingress DNS needs (private DNS zone for the environment domain plus wildcard/apex records pointing to the static IP). As with last week's DNS reconciliation pattern, remediation required no application changes, only consistent zone management and correct forwarding/linking.

Azure multi-region resilience guidance continues last week's reliability framing by focusing on matching patterns to requirements: Availability Zones for in-region HA, regional BCDR across paired or non-paired regions based on capacity/latency/residency, and active/active when you can handle the operational complexity. It reinforces that regional failover is often customer-orchestrated, so testing failover/failback and aligning data replication/consistency are still on you. It distinguishes Azure Site Recovery (VM mobility/failover) from Azure Backup (restore-based protection) and points to the Resiliency agent in Azure Copilot (preview) as emerging tooling for coverage gaps and drills.

Key Vault continuity got a practical warning that fits last week's "do not assume day-2 is handled" theme. Paired-region replication improves survivability but does not guarantee continuity for applications that need writes during outages. During Microsoft-managed regional failover, the vault becomes read-only (reads and crypto ops continue, but writes/updates/rotation/cert renewal stop). If you need rotation and deterministic failover/testing, the recommended pattern is multiple independent vaults across regions with customer-managed replication and failover, supported by Terraform reference architectures including event-based sync (variants for private-endpoint-only regulated environments vs simpler public setups).

- [Private DNS and Hub–Spoke Networking for Enterprise AI Workloads on Azure](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/private-dns-and-hub-spoke-networking-for-enterprise-ai-workloads/ba-p/4508835)
- [Choosing the Right Multi-Region Resilience Pattern on Azure](https://techcommunity.microsoft.com/t5/reliability-and-resiliency-in/modern-azure-resilience-with-mark-russinovich/ba-p/4508967)
- [Azure Key Vault Replication: Why Paired Regions Alone Don’t Guarantee Business Continuity](https://techcommunity.microsoft.com/t5/azure/azure-key-vault-replication-why-paired-regions-alone-don-t/m-p/4508945#M22479)

### Other Azure News

Provisioning workflows continue to standardize around repeatability, echoing last week's deterministic dev/test and predictable rollout theme. An Azure Developer CLI guide walks through the `azd` loop (`azd init`, `azd auth login`, `azd up`, `azd show`, `azd down --force --purge`) and the project structure (`azure.yaml`, `infra/`, `.azure/`) so teams can move from local code to reproducible deployments without portal-driven glue.

- [AZD for Beginners: A Practical Introduction to Azure Developer CLI](https://techcommunity.microsoft.com/t5/microsoft-developer-community/azd-for-beginners-a-practical-introduction-to-azure-developer/ba-p/4503747)

JavaScript teams got a dependency planning notice that fits last week's "small notices become real work" reminder. The Azure SDK for JavaScript ends support for Node.js 20.x after July 9, 2026, and newer releases will require Node.js 22.x in `package.json` engines. If CI uses `engine-strict=true`, installs will fail once you adopt those SDK versions, so plan Node upgrades or be ready to pin packages.

- [Announcing the end of support for Node.js 20.x in the Azure SDK for JavaScript](https://devblogs.microsoft.com/azure-sdk/announcing-the-end-of-support-for-node-js-20-x-in-the-azure-sdk-for-javascript/)

Essential Machine Management (public preview) in Compute Infrastructure Hub pulls more machine operations into subscription-level baselines, aligning with last week's governance-by-default and runbook/observability emphasis. It is an "enroll once per subscription" bundle that enables VM Insights plus recommended alerts, Update Manager, Change Tracking and Inventory, Machine Configuration, and a security baseline policy for Azure VMs and Arc-enabled servers, with pricing depending on Arc coverage/licensing.

- [Announcing Public Preview for Essential Machine Management](https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-public-preview-for-essential-machine-management/ba-p/4502721)

For constrained environments, Azure Local Disconnected Operations described running Azure Local fully air-gapped while still supporting VMs, Kubernetes (AKS), and selected Arc-enabled services. It extends last week's sovereign/edge direction (Azure Local modular datacenters, Foundry Local on-site inference) with more operational detail on what "run disconnected" involves.

- [Azure Local Disconnected Operations: Running Sovereign Cloud, Productivity, and AI in Air‑Gapped Environments](https://www.thomasmaurer.ch/2026/04/azure-local-disconnected-operations-running-sovereign-cloud-productivity-and-ai-in-air-gapped-environments/)

This week's broader platform roundup continued last week's messaging/governance theme. Service Bus NSP support appears again, reinforcing "perimeter plus transition mode" as a cross-service pattern to track. The roundup also included AKS networking/ops updates (CNI overlay CIDR expansion, disabling HTTP proxy, observability improvements), a new Azure Functions MCP resource trigger, ARO NVIDIA GPU support, Network Watcher rule impact analysis, Azure Migrate Azure Files assessment, and PostgreSQL updates (maintenance notifications, PgBouncer 1.25.1).

- [Azure Update 10th April 2026](https://www.youtube.com/watch?v=pQ9em70ZHd4)

Cosmos DB cost/performance tuning got a longer optimization walkthrough aligned with last week's FinOps framing: RU budgeting, throughput choice (manual vs autoscale), account throughput limits, reserved capacity, and how partition keys, document shape, and indexing affect RU use and hot partitions. A short companion video is linked, but it adds little detail beyond the main walkthrough.

- [Cosmos DB Optimization](https://www.youtube.com/watch?v=RbH5F_3w47E)
- [Cosmos DB Optimization #cosmosdb #database](https://www.youtube.com/shorts/dD2qVO_xnb8)

Fabric updates continued last week's "modernize without rewrites" and "reduce glue code" storyline. Shortcut transformations in OneLake/Lakehouse are now GA for turning shortcut files (CSV/Parquet/JSON) into managed Delta tables with continuous sync and schema inference/evolution without ETL pipelines. SQL database in Fabric also added a Migration Assistant (preview) using DACPAC schema migration plus Fabric Copy Jobs (optional Data Gateway) for data moves, including compatibility checks and Copilot fix suggestions.

- [Shortcut transformations: Turn files into Delta tables without pipelines (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/shortcut-transformations-and-turn-files-into-delta-tables-without-pipelines-generally-available/)
- [Introducing Migration assistant for SQL database in Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-migration-assistant-for-sql-database-in-fabric-preview/)

For IIS behind Azure Application Gateway, a troubleshooting guide focused on preventing false Unhealthy probe states (and 502/504s) by using a dedicated `/health` endpoint returning 200 anonymously, avoiding redirects/auth, keeping it fast and dependency-free (even static content), and matching probe TLS/host header settings (including "Pick host name from backend" to avoid SNI/CN mismatch). It matches last week's "design around managed dataplane behaviors" lesson: with App Gateway, probe behavior is part of the contract, and small mismatches can look like random outages until probes are made deterministic.

- [Designing Reliable Health Check Endpoints for IIS Behind Azure Application Gateway](https://techcommunity.microsoft.com/t5/azure-architecture-blog/designing-reliable-health-check-endpoints-for-iis-behind-azure/ba-p/4507938)

## .NET

This week's .NET updates focused on practical changes: a new way to run Spark SQL from ADO.NET code, early direction on Blazor validation in .NET 11, and a Windows packaging change for PowerShell that will affect machines and build agents. Compared to last week's "what's next" previews, this week is more "here is what you can trial now," plus a policy shift that can impact pipelines.

### .NET data access to Microsoft Fabric Spark (ADO.NET preview driver)

Microsoft released a preview ADO.NET provider for Microsoft Fabric Data Engineering, letting .NET apps connect to Fabric and run Spark SQL using standard ADO.NET patterns instead of custom HTTP calls to Livy. It uses Fabric's Livy APIs but exposes familiar abstractions (`DbConnection`, `DbCommand`, `DbDataReader`, `DbParameter`, `DbProviderFactory`) so existing ADO.NET-shaped codebases can adapt with less refactoring. It supports typical command/reader flows and parameterized queries, which makes it easier to integrate Spark SQL execution into existing repository layers or ETL/ELT services without maintaining a separate client stack.

The driver supports Microsoft Entra ID across common auth flows: Azure CLI auth, interactive browser login, client credentials, certificate auth, and direct access-token usage. It also targets efficiency and resilience with connection pooling and Spark session reuse, async prefetch for large result sets, and auto-reconnect to recover sessions after failures. It claims broad Spark SQL type coverage, including `ARRAY`, `MAP`, and `STRUCT`, which matters for lakehouse-shaped data in .NET pipelines. Overall, it gives .NET teams a more standard way to query and manage Fabric Spark from services and tools, aligning with the approach of fitting existing .NET idioms rather than requiring a new programming model.

- [Microsoft ADO.NET Driver for Microsoft Fabric Data Engineering (Preview)](https://blog.fabric.microsoft.com/en-US/blog/microsoft-ado-net-driver-for-microsoft-fabric-data-engineering-preview/)

### Blazor in .NET 11: validation model previews (async rules, localization, render-mode flexibility)

In the Blazor Community Standup, the team previewed .NET 11 validation improvements intended to reduce custom plumbing around `EditForm`. Like last week's .NET 11 Preview 2 direction-setting items, this is another look at where core workflows are headed so teams can rely less on homegrown patterns.

A key focus is built-in asynchronous validation so forms can validate against async-backed rules (username availability, DB checks, external lookups) without pushing developers into custom "validate on submit" flows outside Blazor's normal validation model.

They also previewed localized validation messages to reduce manual resource mapping and custom error pipelines in multi-language apps. Finally, they discussed enabling client-side validation without requiring interactive render mode, which helps when teams mix render modes and do not want interactivity only to get validation feedback. Together, the direction is a more flexible validation stack across async rules, localized UX, and modern render-mode composition. It mirrors last week's theme of "stable defaults with a lane for evolution," applied to everyday app UX.

- ['Blazor Community Standup: Blazor validation in .NET 11'](https://www.youtube.com/watch?v=X-qAr4uqAzc)

### Other .NET News

PowerShell packaging on Windows is changing starting with PowerShell 7.7-preview.1 (April 2026): MSIX becomes the primary installer, and new releases will no longer ship an MSI (MSI remains for existing releases like 7.6). Teams that automate PowerShell rollout across dev machines, CI runners, and managed endpoints will need to shift MSI-based deployment/upgrade pipelines to MSIX tooling for future versions. Microsoft cites a more predictable servicing model (updates, differential updates) and accessibility requirements, while noting gaps still being addressed, especially around remoting and system-level execution (Task Scheduler, services). Practically, it is an early warning to review and update rollout pipelines.

- [PowerShell MSI package deprecation and preview updates](https://devblogs.microsoft.com/powershell/powershell-msi-deprecation/)

Visual Studio shared a workflow tip for multi-monitor users: a setting controls whether floating windows are "owned" by the main IDE window (Tools > Options > Environment > Windows > Floating Windows). Setting ownership to "None" can make floating windows behave more like top-level windows (separate taskbar buttons, fewer always-on-top quirks, and staying visible when the IDE is minimized), which pairs well with PowerToys FancyZones. It also notes other modes ("Tool Windows" and "Documents and Tool Windows") and a shortcut: Ctrl + double-click a tool window title bar to toggle dock/float.

- [Take full control of your floating windows in Visual Studio](https://devblogs.microsoft.com/visualstudio/take-full-control-of-your-floating-windows-in-visual-studio/)

## DevOps

This week's DevOps updates centered on practical CI/CD and dependency-maintenance mechanics on GitHub, plus more shift-left thinking for cost control and incident response that often involves agents. Alongside platform changes, guides also focused on making agent workflows safer on laptops and more accountable in IaC pull requests.

### GitHub Actions, Dependabot, and platform reliability: tighter guardrails and broader ecosystem support

Building on last week's Actions work (less CI friction, tighter security via OIDC claims), GitHub added a limit that affects retry-heavy pipelines: a workflow run can be rerun at most 50 times, whether rerunning all jobs or selected jobs. After the 50th rerun, GitHub returns a failed check suite with an annotation that the limit was reached. If bots or scripts auto-rerun until green, update logic to stop before the cap and consider alternatives like backoff/jitter, narrowing retries to specific steps, or starting a fresh run. This supports last week's reliability theme by nudging teams to engineer reliability rather than relying on unlimited reruns.

Dependabot continued last week's ecosystem expansion with support for Nix flakes in version updates. By adding `nix` in `.github/dependabot.yml`, Dependabot can monitor `flake.lock` inputs and open one PR per outdated flake input as upstream Git refs advance (GitHub, GitLab, SourceHut, or generic `git` URLs). The key caveat remains that this is version updates only. Dependabot security updates still do not apply to Nix flakes, so vulnerability-driven automation needs a separate approach for Nix setups.

GitHub's March 2026 availability report reinforced why fallbacks matter, complementing last week's "keep platform usable at scale" theme. It covers incidents affecting github.com and the API (including a cache-write bug causing widespread expiry and cascading load), Actions scheduling delays and infra errors (Redis load balancer misconfig during resiliency updates), Copilot Coding Agent session failures (auth issues to backing datastore, mitigated by credential rotation, then recurring due to incomplete remediation), and Teams integration delivery failures due to an upstream outage. The actionable DevOps takeaway is to treat platform delays as a distinct failure mode: monitor pipeline SLAs, adjust expectations during incidents, and keep alternate notification paths when integrations break.

- [GitHub Actions workflows are limited to 50 reruns](https://github.blog/changelog/2026-04-10-actions-workflows-are-limited-to-50-reruns)
- [Dependabot version updates now support the Nix ecosystem](https://github.blog/changelog/2026-04-07-dependabot-version-updates-now-support-the-nix-ecosystem)
- [GitHub availability report: March 2026](https://github.blog/news-insights/company-news/github-availability-report-march-2026/)

### Azure cost-aware IaC pipelines and agentic operations: shifting governance earlier and into runtime

Last week emphasized more repeatable infrastructure operations (deterministic Terraform plans, drift gates, cross-cloud investigation via SRE Agent plus MCP). This week extends "intent into enforceable gates" by bringing cost into pull request feedback alongside tests and drift. One guide estimates monthly cost delta for Bicep changes in PRs by running `az deployment group what-if` for a structured change set, then mapping changes to prices via the Azure Retail Prices API. It is implemented in GitHub Actions: trigger on PRs touching `infra/**`, authenticate via OIDC (`azure/login@v2` with `id-token: write`), output what-if JSON, run a Python 3.12 script querying `https://prices.azure.com/api/retail/prices` with OData filters, compute monthly cost as `rate * 730`, and post a sticky PR comment with before/after/delta totals. The gate can fail the workflow if `delta_value` exceeds a threshold (for example, 500), making cost regressions enforceable like failing tests. If you added last week's drift gates, this is the adjacent control: not only "did reality drift?" but "will this PR exceed budget boundaries?"

Microsoft also shared more detail on operationalizing Azure SRE Agent for on-call, continuing last week's MCP-based investigation storyline. The focus is on keeping the system workable over time: explicit autonomy levels (assistive investigation, remediation proposals for review, autonomous resolution for selected classes), RBAC constraints, approval checkpoints, and escalation paths. It also frames agentic workflows across SDLC phases (agents for spec drafting/prototyping in Plan & Code, and evaluation loops in Verify/Test/Deploy) so ops is not the only integration point. On extensibility, it calls out Python tools and MCP to connect external systems/context while keeping humans accountable at boundaries. Together with last week's AWS connectivity guide, the storyline is clearer: MCP is the integration mechanism, while autonomy/RBAC/approvals are what make it safe to run.

- ['Building Cost-Aware Azure Infrastructure Pipelines: Estimate Costs Before You Deploy'](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-cost-aware-azure-infrastructure-pipelines-estimate/ba-p/4508776)
- [How we build and use Azure SRE Agent with agentic workflows](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-we-build-and-use-azure-sre-agent-with-agentic-workflows/ba-p/4508753)

### Other DevOps News

Local AI coding agents got a more ops-focused safety pattern using Docker Sandboxes through the `sbx` CLI. Each sandbox runs inside a microVM with its own kernel and separate Docker engine, instead of giving an agent broad host permissions or access to the host Docker socket. This fits last week's theme that as agents spread beyond ops consoles, isolation and auditable boundaries should become a baseline on laptops too. The guide covers Windows 11 setup (enable `HypervisorPlatform`, install `Docker.sbx` via WinGet, log in, choose egress policy Open/Balanced/Locked Down), then `sbx run` to start agents like Claude Code with network controlled via a host proxy. It also covers practical workflow details: using `--branch` to work in a git worktree under `.sbx/...` to reduce risk to the main tree, adding `.sbx/` to gitignore, and handling constraints like performance overhead, restrictive allowlists, and commit signing friction.

- [Running AI agents safely in a microVM using Docker Sandboxes (sbx)](https://andrewlock.net/running-ai-agents-safely-in-a-microvm-using-docker-sandbox/)

GitHub collaboration UX got several triage improvements that reduce friction, continuing last week's pattern of making queues easier to navigate and less noisy. Issues now show release info in the sidebar Development section when linked by a PR, including the first release tag that shipped it and whether it is Latest or Pre-release. Projects added default values for Text, Number, and Single select fields so new items have consistent baseline metadata. PR lists in public repos can now show contributor role labels ("First-time contributor," "Contributor," "Member") with privacy rules to avoid exposing private org membership. Moderation also improved with a "Low Quality" hide option (separate from Spam/Abuse) across Issues, Discussions, PRs, and commit comments. Combined with last week's search improvements, these changes help keep queues usable as repos and bot/AI activity increase.

- [Release information in issue sidebar and default values for project fields](https://github.blog/changelog/2026-04-09-release-info-in-issue-sidebar-and-project-defaults)
- [Repository member role labels now in pull request list view](https://github.blog/changelog/2026-04-09-repository-member-role-labels-now-in-pull-request-list-view)
- [New Low Quality option in the Hide comment menu](https://github.blog/changelog/2026-04-09-new-low-quality-option-in-the-hide-comment-menu)

VS Code Insiders 1.116 continued improving the "Agents app" experience: better keyboard navigation (focus commands for Changes view, its file tree, and Chat Customizations), screen reader help in chat input (including verbosity controls), and "#"-triggered file-context completions scoped to the selected workspace. It also improves CSS `@import` navigation by resolving into `node_modules`, which reduces friction in bundler-heavy repos. GitHub also highlighted GitCity, an open-source Next.js 15 plus Three.js project that turns profile activity into an explorable 3D pixel-art city. It is more inspiration than operations, but it is a creative use of GitHub signals.

- [Visual Studio Code 1.116 (Insiders) release notes (April 2026)](https://code.visualstudio.com/updates/v1_116)
- [Turn your GitHub profile into a 3D city](https://www.youtube.com/shorts/34nBtYNWm4c)

## Security

This week's security thread ranged from incident-response lessons (token replay, device-code phishing, router-based AiTM) to the quieter work of hardening identity, CI/CD, and data platforms. The common pattern is reducing ambient privilege, tightening trust boundaries, and improving automation so teams can respond faster without adding long-lived secrets or brittle owner-based dependencies. It extends last week's identity-first framing: tactics shift, but control points stay consistent (phishing-resistant auth, tighter Conditional Access, shorter-lived tokens, and strong revocation/runbooks).

### Identity- and token-centric attacks: AiTM, device-code phishing, and the edge as the new MITM

Storm-2755's "payroll pirate" activity shows session theft beating password theft. DART describes adversary-in-the-middle phishing that proxies Microsoft 365 sign-in, captures session cookies/OAuth tokens, and replays them to bypass MFA that is not phishing-resistant. It continues last week's AiTM thread: tokens often matter more than passwords, and detections frequently come from sign-in telemetry. Hunting signals include an Entra sign-in interrupt error **50199** right before a successful sign-in, plus a **user-agent shift to Axios** (often `axios/1.7.9`) while the **session ID stays consistent**, which can indicate replay. The response playbook is clear: revoke sessions/tokens, remove malicious Exchange inbox rules hiding HR/payroll messages, then harden with FIDO2/WebAuthn, stricter Conditional Access (device compliance, session controls), and Continuous Access Evaluation (CAE) to reduce replay windows.

Microsoft Defender also covered an **AI-assisted OAuth device code phishing** campaign that generates device codes on demand only after a victim clicks, which preserves the 15-minute validity window and improves success rates. It reinforces last week's point that automation can raise the tempo of older attack flows. The detection anatomy includes tenant recon via `GetCredentialType`, multi-hop redirects using compromised domains and serverless infrastructure (Vercel, Cloudflare Workers, AWS Lambda), and a backend that requests device codes live then polls every 3-5 seconds. Post-compromise behavior (device registration for longer-lived access, Graph recon, inbox-rule persistence) ties to mitigation: block device-code flow where unnecessary via Conditional Access "block authentication flows," enforce Entra ID Protection risk policies, and use Graph session revocation to invalidate stolen refresh tokens.

Forest Blizzard / Storm-2754 broadens the AiTM story with SOHO router compromises where DNS hijacking becomes the MITM stepping stone. The identity outcome is the same (token/session abuse) but achieved by shifting interception to edge devices enterprises often do not monitor closely. By altering router DHCP/DNS settings (likely via **dnsmasq**), the actor can proxy most DNS traffic and selectively spoof responses for target domains. In higher-priority cases, victims are redirected to infrastructure with **invalid TLS certificates**. If users click through, sessions can be intercepted. Defenses focus on limiting blast radius: enforce trusted DNS resolution on endpoints (Windows **Zero Trust DNS**), enable Defender for Endpoint web/network protection, tighten Conditional Access and CAE, and hunt for downstream Entra/M365 anomalies rather than expecting to detect the router compromise directly.

- [Investigating Storm-2755: “Payroll pirate” attacks targeting Canadian employees](https://www.microsoft.com/en-us/security/blog/2026/04/09/investigating-storm-2755-payroll-pirate-attacks-targeting-canadian-employees/)
- [Inside an AI-enabled device code phishing campaign](https://www.microsoft.com/en-us/security/blog/2026/04/06/ai-enabled-device-code-phishing-campaign-april-2026/)
- [SOHO router compromise leads to DNS hijacking and adversary-in-the-middle attacks](https://www.microsoft.com/en-us/security/blog/2026/04/07/soho-router-compromise-leads-to-dns-hijacking-and-adversary-in-the-middle-attacks/)

### DevSecOps and supply chain workflow updates: prioritize what’s deployed, reduce secrets, and streamline fixes

GitHub's updates continued to move security closer to developer workflows while adding automation and governance for platform teams. It continues last week's shift from "turn it on" to "operate it well," with better triage signals, better reporting, and less reliance on long-lived secrets.

GitHub Advanced Security can now ingest **Dynatrace runtime context** to prioritize code scanning and Dependabot alerts based on what is actually deployed and observed at runtime. Instead of treating all alerts equally, teams can filter to findings affecting production artifacts that are *internet-exposed* or touch *sensitive data* (for example, `has:deployment AND runtime-risk:internet-exposed`). This aligns with last week's theme of making security signals match delivery reality so teams spend less time on noise.

GitHub code scanning now supports **batch-applying multiple fix suggestions in a PR** from the "Files changed" tab. The benefit is fewer commits and fewer repeated scans: apply multiple low-risk fixes together, commit once, and validate with one scanning run. It complements last week's faster PR feedback focus by reducing fix-step friction, not only detection friction.

Secret Scanning's REST/webhook updates improve scale operations and auditability, building on last week's detector/push-protection expansion. `exclude_secret_types` on list endpoints makes scripts more resilient as new secret types appear. Location payloads add `html_url` to reduce extra API calls for dashboards/tickets. Delegated workflows gained clearer lifecycle visibility (expiration windows, confirmation emails) plus closure fields (`closure_request_comment`, reviewer metadata) so external systems can track why alerts were dismissed and who approved it.

npm Trusted Publishing also added **CircleCI** as an OIDC provider, extending tokenless publishing beyond GitHub Actions and GitLab CI/CD. After last week's supply-chain push toward short-lived workload identity, this makes it easier for CircleCI pipelines to publish without stored npm tokens through `npm trust`.

- [Prioritize security alerts with runtime context from Dynatrace](https://github.blog/changelog/2026-04-07-prioritize-security-alerts-with-runtime-context-from-dynatrace)
- [Code scanning: Batch apply security alert suggestions on pull requests](https://github.blog/changelog/2026-04-07-code-scanning-batch-apply-security-alert-suggestions-on-pull-requests)
- [Secret scanning improvements to alert APIs, webhooks, and delegated workflows](https://github.blog/changelog/2026-04-08-secret-scanning-improvements-to-alert-apis-webhooks-and-delegated-workflows)
- [npm trusted publishing now supports CircleCI](https://github.blog/changelog/2026-04-06-npm-trusted-publishing-now-supports-circleci)

### Platform identity, data governance, and agent runtime controls: shrinking blast radius across Azure, Fabric, and agent stacks

Several posts converged on a practical point: shared identities and default execution contexts create hidden coupling, and over-sharing can turn a normal incident into cross-environment impact. This matches last week's "guardrails on default paths" theme. Put controls at natural choke points (identity issuance, admission, label APIs) so protections still hold when workflows drift.

In Azure, guidance on **User Assigned Managed Identities (UAMI)** calls out a common enterprise anti-pattern: reusing one UAMI across DEV/UAT/PROD and across compute types (Functions, App Service, VMs, AKS). Since any attached workload can use **IMDS** to get Entra tokens for that UAMI, a lower-environment compromise can become lateral access into production if RBAC is broad (especially at subscription scope). The recommendation is straightforward: separate UAMIs per environment, scope RBAC to resource group/resource, and prefer narrow data-plane roles (Key Vault Secrets User, Storage Blob Data Reader) over broad management roles.

Fabric shipped two identity/encryption-related previews that target similar coupling. Last week's Fabric governance work emphasized operability (labels in REST APIs, CMK options, recovery). This week reduces owner-dependency and improves enterprise transport security. **Associated identities for items** lets Lakehouse and certain Eventstream items run under a specified identity (user, service principal, or managed identity) rather than the creator/owner. That avoids breakage when owners leave or credentials expire and aligns with CI/CD through Fabric REST APIs (`.../identities/default/assign?beta=true`, plus `include=defaultIdentity` in GET). Fabric Eventstream Kafka connectors also gained **custom CA chains and mTLS** support, with certs stored in **Azure Key Vault** and fetched at runtime. For private deployments, that implies a concrete prerequisite: if you are using Eventstream VNet injection, you may need a **Key Vault private endpoint** to keep cert retrieval off public paths.

At the AI runtime layer, the open-source **Agent Governance Toolkit v3.0.0 (Public Preview)** outlines how to run autonomous agents with policy, identity, isolation, and SRE guardrails. It follows last week's agent governance framing by shifting from "agents are risky" to "here is how to enforce boundaries and kill switches." The core risk is that agents can behave like "root" when calling tools/APIs, and the toolkit maps that to controls. **Agent OS** acts as a stateless pre-execution policy interceptor (YAML/OPA Rego/Cedar), classifies intent (for example, `DESTRUCTIVE_DATA`, `DATA_EXFILTRATION`), and supports enforcement modes (block, require approval, downgrade trust). **Agent Mesh** adds cryptographic identity/inter-agent trust using DIDs, Ed25519 signing, and delegation chains that only allow scope narrowing. **Agent Hypervisor** adds privilege-ring-style isolation tied to trust scores (Ring 0-3) with resource limits and saga orchestration for compensating actions. **Agent SRE** adds SLOs/error budgets, circuit breakers, chaos templates, and observability via OpenTelemetry/Prometheus and incident tooling. The result is a blueprint for running agents in Kubernetes/Azure without treating every tool call as implicitly allowed.

- [Enterprise UAMI Design in Azure: Trust Boundaries and Blast Radius](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/enterprise-uami-design-in-azure-trust-boundaries-and-blast/ba-p/4509614)
- [Associated identities for items (Preview)](https://blog.fabric.microsoft.com/en-US/blog/associated-identities-for-items-preview/)
- [Secure data streaming: Custom CA and mTLS in Fabric Eventstream connectors (Preview)](https://blog.fabric.microsoft.com/en-US/blog/secure-data-streaming-custom-ca-and-mtls-in-fabric-eventstream-connectors-preview/)
- [Agent Governance Toolkit: Architecture Deep Dive, Policy Engines, Trust, and SRE for AI Agents](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/agent-governance-toolkit-architecture-deep-dive-policy-engines/ba-p/4510105)

### Other Security News

Ransomware activity reinforced how little time can exist between disclosure and exploitation for web-facing systems. Storm-1175's Medusa intrusions show fast-moving playbooks exploiting newly disclosed edge vulnerabilities (Exchange, Ivanti, ScreenConnect, TeamCity, GoAnywhere MFT, and others), then moving quickly into credential theft (LSASS dumping, Mimikatz), lateral movement (PsExec/Impacket/RDP), and exfiltration (Rclone) before encryption. The operational implication is straightforward: asset inventory, patch pipelines, and hardening controls like Defender tamper protection/ASR rules and WAF/reverse proxy placement are ongoing engineering work. It also echoes last week's "default paths" theme: internet-facing management and file-transfer planes are predictable entry points, so inventory and patch SLAs function as guardrails.

- [Storm-1175 focuses gaze on vulnerable web-facing assets in high-tempo Medusa ransomware operations](https://www.microsoft.com/en-us/security/blog/2026/04/06/storm-1175-focuses-gaze-on-vulnerable-web-facing-assets-in-high-tempo-medusa-ransomware-operations/)

Mobile supply chain risk got a concrete example: an Android intent redirection issue in EngageLab's EngageSDK where a merged-manifest exported activity (`MTCommonActivity`) could let a malicious local app trigger attacker-controlled intents using the victim app's identity/permissions. The fix is to upgrade to EngageSDK **v5.2.1+** (activity non-exported). The broader lesson matches last week's supply-chain incidents: review merged manifests after SDK updates because execution surface can change even when your own manifest looks clean, and build repeatable checks (manifest review, SCA, policy) into default dependency workflows.

- [Intent redirection vulnerability in third-party SDK exposed millions of Android wallets to potential risk](https://www.microsoft.com/en-us/security/blog/2026/04/09/intent-redirection-vulnerability-third-party-sdk-android/)

Azure sovereignty requirements were translated into concrete controls for Belgium Central: IaC region pinning (`belgiumcentral`), AZ-first resiliency in a **non-paired region**, CMK/double encryption with Key Vault/Managed HSM, and confidential computing with attestation-gated key release where available (AMD SEV-SNP previews in-region; Intel TDX not available there). It complements last week's governance-and-keys theme: sovereignty is implemented through residency, key control, and attestation/encryption for data in use.

- [Sovereignty in Azure Belgium Central: A Three-Layer Technical Deep Dive](https://techcommunity.microsoft.com/t5/azure-confidential-computing/sovereignty-in-azure-belgium-central-a-three-layer-technical/ba-p/4506936)

Two GitHub admin items rounded out the week: a new org-level Code Security risk assessment report aggregating findings by severity/rule type/language (and flagging where Copilot Autofix may apply), and a heads-up for `gh` installs via apt/yum/dnf. GitHub published an updated PGP keyring ahead of the current key's 2026-09-05 expiry to avoid future install/update failures in machines and CI images. The report aligns with last week's improved CodeQL reporting by giving a closer org-wide picture beyond default-branch slices. The signing-key update is supply-chain hygiene in the same spirit as last week's CI hardening: key rotation keeps default install paths reliable.

- [Code Security risk assessment available for organizations](https://github.blog/changelog/2026-04-08-code-security-risk-assessment-available-for-organizations)
- [New PGP signing key for GitHub CLI Linux packages](https://github.blog/changelog/2026-04-08-new-pgp-signing-key-for-github-cli-linux-packages)

Microsoft's "agentic SOC" post offered a forward-looking SecOps model: deterministic autonomous disruption for high-confidence threats, plus agents that assemble context and orchestrate investigations so humans focus on governance, tuning, and longer-term posture. It continues last week's agent governance thread. More automation only helps when bounded by policy, identity, and auditability (what agents can do, when approval is required, and how to shut them off). Even without higher autonomy, it provides a useful lens for where engineering effort shifts: policy boundaries, confidence thresholds, and accountable automation over manual triage.

- [The agentic SOC—Rethinking SecOps for the next decade](https://www.microsoft.com/en-us/security/blog/2026/04/09/the-agentic-soc-rethinking-secops-for-the-next-decade/)
