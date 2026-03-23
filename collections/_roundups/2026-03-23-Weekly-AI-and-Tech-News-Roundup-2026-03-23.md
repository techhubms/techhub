---
title: "Operating AI Agents Like Software: Governance, Tools, and Operational Loops"
author: "TechHub"
date: 2026-03-23 09:00:00 +00:00
tags: ["GitHub Copilot","Copilot Coding Agent","VS Code Copilot Agents","Model Context Protocol (MCP)","Azure DevOps","GitHub Actions","GitHub Enterprise Server","Azure AI Foundry","Agent Runtime","OpenTelemetry","Secret Scanning","GitHub Advanced Security","Microsoft Fabric","OneLake","AKS"]
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
external_url: "/all/roundups/Weekly-AI-and-Tech-News-Roundup-2026-03-23"
---

This week's roundup across GitHub, Microsoft, and Azure shares a clear theme: teams are starting to run agents and automation the way they run production software, with defaults, controls, and audit trails. Copilot keeps moving from "chat that writes code" toward governed model selection, agent workflows with adjustable validations, and MCP tool connectivity that brings scanners and platform context into the inner loop. At the same time, Azure AI Foundry and Fabric add more runtime and data-plane building blocks teams use for secure deployment (private networking, managed identities, continuous evaluation), while Azure and GitHub DevOps updates focus on operational fundamentals like ingress migrations, routing resiliency, CI scheduling, and security rollout across large estates.

<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Models, defaults, and enterprise governance](#models-defaults-and-enterprise-governance)
  - [Copilot coding agent: faster starts, smarter search, tighter quality gates, and better traceability](#copilot-coding-agent-faster-starts-smarter-search-tighter-quality-gates-and-better-traceability)
  - [VS Code Copilot agents: Autopilot, multi-agent workflows, troubleshooting, and customization management](#vs-code-copilot-agents-autopilot-multi-agent-workflows-troubleshooting-and-customization-management)
  - [MCP and plugins: giving Copilot real tools (Azure DevOps, GitHub secret scanning, Azure resources, Fabric)](#mcp-and-plugins-giving-copilot-real-tools-azure-devops-github-secret-scanning-azure-resources-fabric)
  - [Copilot CLI and repo-native agent workflows: multi-model review, session forensics, and reproducible “agent memory”](#copilot-cli-and-repo-native-agent-workflows-multi-model-review-session-forensics-and-reproducible-agent-memory)
  - [Copilot customizations and “skills” as shared team assets](#copilot-customizations-and-skills-as-shared-team-assets)
  - [Copilot in real workflows: MAUI repo agents, cross-arch C++ migrations, designers prototyping in code, and hosted agent production paths](#copilot-in-real-workflows-maui-repo-agents-cross-arch-c-migrations-designers-prototyping-in-code-and-hosted-agent-production-paths)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Azure AI Foundry Agents move into production: GA runtime, private networking, voice, and built-in evaluation](#azure-ai-foundry-agents-move-into-production-ga-runtime-private-networking-voice-and-built-in-evaluation)
  - [MCP as the agent tool layer: remote servers in Foundry, managed observability gateways, and a C# SDK v1.0](#mcp-as-the-agent-tool-layer-remote-servers-in-foundry-managed-observability-gateways-and-a-c-sdk-v10)
  - [Knowledge-grounded agents with Foundry IQ: permission-aware retrieval via Azure AI Search + MCP](#knowledge-grounded-agents-with-foundry-iq-permission-aware-retrieval-via-azure-ai-search--mcp)
  - [Building, testing, and operating agents: VS Code AI Toolkit, azd local run/invoke, and resilient long-running runs](#building-testing-and-operating-agents-vs-code-ai-toolkit-azd-local-runinvoke-and-resilient-long-running-runs)
  - [Agent patterns in practice: Python agent workflows and realtime voice multi-agent systems in .NET](#agent-patterns-in-practice-python-agent-workflows-and-realtime-voice-multi-agent-systems-in-net)
  - [Microsoft Fabric as an AI execution surface: multimodal functions, Copilot-in-notebooks, real-time intelligence, and pipeline automation](#microsoft-fabric-as-an-ai-execution-surface-multimodal-functions-copilot-in-notebooks-real-time-intelligence-and-pipeline-automation)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Microsoft Fabric: Lakehouse pipelines, runtimes, and notebook automation](#microsoft-fabric-lakehouse-pipelines-runtimes-and-notebook-automation)
  - [Azure Databricks Lakeflow: near-real-time ingestion, SCD transformations, and governance in Delta Lake](#azure-databricks-lakeflow-near-real-time-ingestion-scd-transformations-and-governance-in-delta-lake)
  - [Other ML News](#other-ml-news)
- [Azure](#azure)
  - [AKS ingress modernization: moving off Ingress NGINX to Application Gateway for Containers (AGC)](#aks-ingress-modernization-moving-off-ingress-nginx-to-application-gateway-for-containers-agc)
  - [Edge and routing resiliency: Azure Front Door patterns and Front Door’s faster RTO work](#edge-and-routing-resiliency-azure-front-door-patterns-and-front-doors-faster-rto-work)
  - [Container supply chain reliability: health-aware ACR geo-replication failover](#container-supply-chain-reliability-health-aware-acr-geo-replication-failover)
  - [Azure compute for GPU workloads: NCv6 refresh and GA transition](#azure-compute-for-gpu-workloads-ncv6-refresh-and-ga-transition)
  - [Microsoft Fabric + OneLake modernization: migrations, governance, real-time pipelines, and platform controls](#microsoft-fabric--onelake-modernization-migrations-governance-real-time-pipelines-and-platform-controls)
  - [Other Azure News](#other-azure-news)
- [.NET](#net)
  - [PowerShell 7.6 LTS on .NET 10: production automation baseline, with shell and module refinements](#powershell-76-lts-on-net-10-production-automation-baseline-with-shell-and-module-refinements)
  - [.NET MAUI 11 Preview 2: built-in map pin clustering for Android and iOS/Mac Catalyst](#net-maui-11-preview-2-built-in-map-pin-clustering-for-android-and-iosmac-catalyst)
  - [Other .NET News](#other-net-news)
- [DevOps](#devops)
  - [GitHub Enterprise Server 3.20: tighter governance, safer releases, and backup planning](#github-enterprise-server-320-tighter-governance-safer-releases-and-backup-planning)
  - [GitHub Actions on Kubernetes and in workflow YAML: runner scale sets and fewer scheduling papercuts](#github-actions-on-kubernetes-and-in-workflow-yaml-runner-scale-sets-and-fewer-scheduling-papercuts)
  - [Microsoft Fabric CI/CD: “definitions as code” gets more Git-native and more automatable](#microsoft-fabric-cicd-definitions-as-code-gets-more-git-native-and-more-automatable)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [GitHub security controls: malware-aware dependencies, stricter secret policies, and smoother rollout at scale](#github-security-controls-malware-aware-dependencies-stricter-secret-policies-and-smoother-rollout-at-scale)
  - [Microsoft’s AI security push: observability, Zero Trust guidance, and evaluation for agent-driven detection](#microsofts-ai-security-push-observability-zero-trust-guidance-and-evaluation-for-agent-driven-detection)
  - [Identity-first intrusions and seasonal phishing: concrete defender guidance for real campaigns](#identity-first-intrusions-and-seasonal-phishing-concrete-defender-guidance-for-real-campaigns)
  - [Security for Microsoft Fabric and OneLake: centralized policy enforcement meets AI-era governance](#security-for-microsoft-fabric-and-onelake-centralized-policy-enforcement-meets-ai-era-governance)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week's Copilot story is less about one headline feature and more about Copilot settling into three practical layers teams run every day: (1) clearer model choice and governance, (2) agent workflows with the observability and safety controls teams expect, and (3) broader MCP tool access so Copilot can act with real platform context (Azure DevOps, GitHub scanners, Azure resources, Fabric) instead of relying on chat history guesses. Building on last week's themes (auto model selection across IDEs, repo-visible instruction files and hooks, and enterprise observability), this week adds more of the operational layer needed for scale: stable model windows, adjustable validations, and more traceable agent execution.

### Models, defaults, and enterprise governance

Copilot's model lineup changed in two ways that matter for operations, and both extend last week's model-management thread (JetBrains Auto Model Selection GA, model attribution, and plan/policy routing).

First, OpenAI's **GPT-5.4 mini** is rolling out as generally available as a faster, agentic coding option aimed at repo exploration and "grep-style" tool workflows. It appears in the model picker across VS Code (chat/ask/edit/agent), Visual Studio, JetBrains, Xcode, Eclipse, github.com, GitHub Mobile, and GitHub CLI; availability depends on paid plans and Business/Enterprise admin policy. GitHub also notes a **0.33x premium request multiplier** (subject to change), which can help keep exploration costs lower. That matters more now that auto-routed requests can carry pricing modifiers and teams increasingly treat model choice as a policy and cost-control surface, not just personal preference.

Second, GitHub added an enterprise stability option: **long-term support (LTS) for GPT-5.3-Codex** on Copilot Business and Enterprise. LTS models remain available for **12 months**, and GPT-5.3-Codex is the first, available through **2027-02-04**. GPT-5.3-Codex will also become the **default base model** (replacing GPT-4.1) for orgs that have not explicitly approved or selected alternatives. GitHub notes automatic enablement within 60 days and calls out a **base-model date of 2026-05-17**, so even teams that never touch the model picker should plan a validation window if they care about style, security patterns, or dependency choices. Together with last week's push to show which model responded, this is about fewer surprises: clearer defaults, longer-lived enterprise options, and better attribution for governance and audit.

- [GPT-5.4 mini is now generally available for GitHub Copilot](https://github.blog/changelog/2026-03-17-gpt-5-4-mini-is-now-generally-available-for-github-copilot)
- [GPT-5.3-Codex long-term support in GitHub Copilot](https://github.blog/changelog/2026-03-18-gpt-5-3-codex-long-term-support-in-github-copilot)

### Copilot coding agent: faster starts, smarter search, tighter quality gates, and better traceability

Several updates landed around the Copilot **coding agent** (the hosted/background agent assigned issues, run from Agents, or driven via `@copilot` on PRs). Last week's focus on autonomy trade-offs (auto-approval switches, IDE autopilot modes) and "run agents safely in real repos" continues here, with speed improvements plus more reviewable, repo-tunable controls and audit hooks.

On performance, GitHub says the agent now **starts work ~50% faster**, reducing "time to first action" before it begins changing code. On effectiveness, it now uses **semantic code search** alongside text matching, helping it find code by intent; GitHub says this cut **~2%** off task time in internal tests. This pairs with last week's parallelism story (CLI `/fleet`, multi-agent editor workflows): faster orientation and better code location make delegated tasks easier to use in practice.

The most practical change this week is control and inspectability. Repos can now **configure which validation tools the coding agent runs** when it changes code. By default it runs tests and lint plus GitHub security/quality checks (CodeQL, Advisory Database, secret scanning, Copilot code review) and tries to fix findings before returning results. Admins can now tune that set in **Repository Settings -> Copilot -> Coding agent**, including disabling heavier checks (like slow CodeQL) when they do not match the repo's feedback loop, even though GitHub says these validations are free and on by default. This continues last week's governance theme: as agents act more autonomously, teams want visible "speed vs safety" controls aligned with CI, not buried in prompt habits.

Two related improvements also help with audit and review. Session logs are now clearer and more actionable (explicit setup steps like clone + "agent firewall" start; inline output from `copilot-setup-steps.yml`; subagent work collapsed by default with an "in progress" indicator). And agent commits now include an **`Agent-Logs-Url` trailer** so reviewers can jump from a commit to the exact session logs that produced it, which helps both review context and later traceability. This extends last week's observability push (VS Code OpenTelemetry, hook troubleshooting, debug snapshots) into Git history.

- [Copilot coding agent now starts work 50% faster](https://github.blog/changelog/2026-03-19-copilot-coding-agent-now-starts-work-50-faster)
- [Copilot coding agent works faster with semantic code search](https://github.blog/changelog/2026-03-17-copilot-coding-agent-works-faster-with-semantic-code-search)
- [Configure Copilot coding agent’s validation tools](https://github.blog/changelog/2026-03-18-configure-copilot-coding-agents-validation-tools)
- [More visibility into Copilot coding agent sessions](https://github.blog/changelog/2026-03-19-more-visibility-into-copilot-coding-agent-sessions)
- [Trace any Copilot coding agent commit to its session logs](https://github.blog/changelog/2026-03-20-trace-any-copilot-coding-agent-commit-to-its-session-logs)

### VS Code Copilot agents: Autopilot, multi-agent workflows, troubleshooting, and customization management

VS Code's Copilot agent experience kept moving toward longer-running, more autonomous workflows, while adding guardrails and diagnostics for when agents go off track. This continues last week's 1.111 autonomy modes discussion (default approvals, bypass approvals, autopilot) and the parallel effort to make customization debuggable (instruction discovery conventions, `/troubleshoot`, hook inspection).

In an Insiders walkthrough, VS Code previewed **Autopilot mode** and chat UX updates for multi-step sessions: "shimmers" to clarify generation state, **collapsed containers** to keep long runs readable, and input UI changes that surface agent controls. Autopilot reduces constant confirmations, but adds explicit **approval modes**, a **permissions picker**, max-retry limits, and a "task_complete" stop condition to keep unattended runs bounded. That matches last week's framing that autonomy is a posture you choose, not a single switch.

The VS Code **1.112** update video covered details that show up in CLI/background sessions: you can now steer an in-progress run (steer after current tool call, add to queue, or stop-and-send). Startup is also safer with uncommitted changes: the session prompts what to do and shows an **expandable file list** so you know what will be copied, moved, or skipped. Autopilot extends to **CLI sessions** via `chat.autopilot.enabled`, with guidance to use isolated environments like dev containers or Codespaces when bypassing approvals. That echoes last week's "safer automation" and sandboxing focus.

Troubleshooting also got more concrete: a new **`/troubleshoot`** command analyzes **agent debug logs** (with JSONL logging enabled) to explain why instructions or skills are not applied. Logs can now be **exported/imported** as JSON for team debugging, with a reminder they may contain sensitive content. Version 1.112 also added image analysis for workspace images, symbol paste that preserves location context (`sym:` references), better monorepo customization discovery (including parent `.github` customizations via `chat.useCustomizationsInParentRepositories`), and a unified "Open Customizations" view to manage agents, skills, instructions, and **MCP servers** (including disabling servers per session or workspace). This continues last week's "move customization to files" direction: fewer implicit behaviors, more repo-visible configuration, and fewer mysteries when a skill or instruction does not apply.

For teams testing parallel agents, another video showed **multiple agent sessions side-by-side** as separate workstreams (feature, storage wiring, docs) so you are not blocked on one tool run. It complements last week's CLI `/fleet` story by showing parallelization in-editor as a practical way to deal with tool latency.

- [Autopilot Mode with Justin Chen](https://www.youtube.com/watch?v=ne9l0S-JNE8)
- [Visual Studio Code and GitHub Copilot - What's new in 1.112](https://www.youtube.com/watch?v=BC35VXggNDc)
- [Multi-agent workflows in VS Code](https://www.youtube.com/watch?v=J5KTpq7hVn4)

### MCP and plugins: giving Copilot real tools (Azure DevOps, GitHub secret scanning, Azure resources, Fabric)

MCP keeps becoming the plumbing that connects Copilot to real systems in a way teams can deploy and govern. After last week's MCP momentum (bridging chat to permissioned tool calls) and the reminder that auto-approve is a policy choice, this week's emphasis is moving from local experiments to operational deployments: hosted endpoints, security scanning in the inner loop, and platform skills that need repeatable setup.

For Azure DevOps, Microsoft announced a **public preview Remote Azure DevOps MCP Server**: a hosted MCP endpoint in Azure DevOps that uses streamable HTTP transport and removes the need to run a local server process. Setup is essentially an org-scoped URL in MCP config (for example, `https://mcp.dev.azure.com/{organization}`), but the prerequisite matters: the org must be **Entra-backed** (not MSA-only). Today, it works without extra onboarding in **Visual Studio + Copilot** and **VS Code + Copilot**; other clients (Copilot CLI, Claude Desktop/Code, ChatGPT) depend on upcoming Entra OAuth dynamic registration. Microsoft also signaled that local-server support remains "for now," but the repo is expected to be archived once remote reaches GA. In practice, this is the step that makes MCP easier to run in an enterprise: fewer local daemons, more centrally managed auth and endpoints.

On GitHub's MCP side, a **public preview** lets AI coding agents trigger **GitHub secret scanning via the GitHub MCP Server** to catch leaked credentials in uncommitted changes (pre-commit/pre-PR). It requires **GitHub Secret Protection**. Setup differs by host: in Copilot CLI you install the Advanced Security plugin and add the MCP tool (`run_secret_scanning`); in VS Code you install the advanced-security agent plugin and run `/secret-scanning` from Copilot Chat. The key shift is bringing secret detection into the inner loop while agents generate or refactor config, scripts, and sample code, which is where secrets often slip in. That reinforces last week's defense-in-depth guidance: wire scanners in as tools instead of assuming the model will remember.

For Azure, a guide walked through installing and verifying the **Azure Skills Plugin** across **Copilot CLI**, **VS Code**, and **Claude Code**, with an emphasis on proving tools are actually called. It covers Node.js 18+ (MCP servers via `npx`), `az login`, `azd auth login`, and smoke tests like "list my resource groups." It also calls out a common operational issue: Copilot CLI needs **`/mcp reload`** after install, and token or skill budgets can silently prevent skills from activating when multiple plugins are loaded. This matches last week's "prompt less, context more" point: many "Copilot ignored me" cases are tool/config visibility problems.

Fabric also showed up via the FabCon/SQLCon recap: **Fabric MCP** (local MCP GA as open source; remote MCP in public preview) and "Agent Skills for Fabric" plugins so GitHub Copilot in the terminal can perform Fabric tasks via MCP tooling, alongside Git integration/CI/CD improvements and agent-enabled operational workflows. It follows the same arc as the Azure DevOps remote MCP announcement: move from "connect a tool" to "repeatable platform capability."

- [Azure DevOps Remote MCP Server (public preview)](https://devblogs.microsoft.com/devops/azure-devops-remote-mcp-server-public-preview/)
- [Secret scanning in AI coding agents via the GitHub MCP Server](https://github.blog/changelog/2026-03-17-secret-scanning-in-ai-coding-agents-via-the-github-mcp-server)
- [Azure Skills Plugin – Let’s Get Started!](https://devblogs.microsoft.com/all-things-azure/azure-skills-plugin-lets-get-started/)
- ['FabCon and SQLCon 2026: Unifying databases and Fabric on a single platform'](https://azure.microsoft.com/en-us/blog/fabcon-and-sqlcon-2026_unifying-databases-and-fabric-on-a-single-data-platform/)

### Copilot CLI and repo-native agent workflows: multi-model review, session forensics, and reproducible “agent memory”

Copilot CLI and "agents in the repo" content lined up on one theme: treat agent output as inspectable, repeatable, and standardizable, not one-off chat. This continues last week's CLI direction (requesting Copilot code review from `gh`, `/fleet` parallelism, terminal-first workflows) and pushes further into repeatability and post-hoc analysis.

A Copilot CLI tip showed using `/review` as a local pre-PR gate scoped to **bugs, security, and performance**, then doing multi-agent review by using multiple model backends (for example, Gemini, Codex, Opus) and combining results. That can help surface different failure modes before CI, and it fits last week's point that model choice is becoming explicit and policy-shaped. Here, the goal is coverage rather than debating a single "best model."

Another advanced video focused on forensics: Copilot CLI stores session history in a local **SQLite database**, which you can query to retrieve prior prompts and outputs, reconstruct past work, or feed history back into Copilot to critique prompting patterns. It complements last week's "memory default-on" and "debug snapshots" story: even when memory sharing is not desired, teams still want reproducible records of what happened.

GitHub also highlighted **Squad**, an open-source, Copilot-based multi-agent orchestration approach that lives in the repo. It is repo-native: install a CLI, run `squad init`, and it generates an AI "team" (coordinator plus specialists) that works via branches and PRs, uses repo files for shared memory (for example, a committed "versioned decisions.md"), and keeps an audit trail under `.squad/`. Version-controlled memory (instead of an external vector DB) keeps it inspectable and portable. This matches last week's instruction-file and hook movement: put agent behavior in on-disk artifacts you can review, diff, and govern. The post also describes safety patterns like independent review, where rejected output cannot be self-corrected by the same agent and fixes are pushed to a different persona. That is an organizational analogue to last week's separate approvals and defense-in-depth posture.

- [How to get a multi-agent code review in Copilot CLI](https://www.youtube.com/watch?v=qRXztN1hi1M)
- [How to query your Copilot CLI session history | Advanced tips & tricks](https://www.youtube.com/watch?v=XHTc6XF0gdk)
- [How Squad runs coordinated AI agents inside your repository](https://github.blog/ai-and-ml/github-copilot/how-squad-runs-coordinated-ai-agents-inside-your-repository/)

### Copilot customizations and “skills” as shared team assets

Copilot customization is increasingly treated as reusable tooling, not personal prompt snippets. This builds on last week's shift to repo-discoverable instruction sources (AGENTS.md/CLAUDE.md, `.github/instructions`, hooks) and the emerging skills ecosystem (`dotnet/skills`, skill specs, evaluation loops). The common thread is that "how Copilot behaves" is becoming versioned project configuration, not chat lore.

The community-driven **Awesome GitHub Copilot Customizations** project shipped a website and Learning Hub, plus a plugin marketplace-style flow to discover and adopt agents, skills, instructions, and plugins via PR-reviewed, traceable GitHub workflows. The catalog is now large (hundreds of items), and the site adds search, previews, and one-click installs for VS Code/Insiders, plus CLI install patterns like `copilot plugin install <plugin>@awesome-copilot`. It also reflects an ongoing simplification effort: consolidating formats so skills become the common reusable unit over time, which helps teams package and maintain standard behaviors more easily. That lines up with last week's point that reusable skills tend to produce more reproducible behavior than ad-hoc prompting.

A short VS Code intro reinforced the same product direction: **Agent Skills** bundle instructions and resources into a named capability invoked on demand (for example, `/skill-name`) instead of expanding system prompts or rewriting checklists. The aim is predictability: teams codify conventions, navigation routines, and run/test steps as repeatable skills discoverable in chat. Paired with last week's instruction discovery and troubleshooting, the pattern is explicit, discoverable, and debuggable customization.

- [Awesome GitHub Copilot gets a website, Learning Hub, and plugin marketplace](https://devblogs.microsoft.com/blog/awesome-github-copilot-just-got-a-website-and-a-learning-hub-and-plugins)
- [Want to give your coding agent new capabilities? Use Agent Skills in VS Code](https://www.youtube.com/shorts/7fzCsefkgKk)

### Copilot in real workflows: MAUI repo agents, cross-arch C++ migrations, designers prototyping in code, and hosted agent production paths

A few deeper pieces showed what "Copilot as workflow" looks like when embedded in real contribution pipelines. This extends last week's "portfolio-scale modernization" and "agents as workflow participants" theme, but with concrete layouts, gates, and deployment paths teams can reuse.

The .NET MAUI team described repo-native **Copilot CLI agents and composable skills** under `.github/agents` and `.github/skills`, including a structured `pr-review` skill with phases that enforce tests-first and multi-platform validation. The "Gate" step blocks progress until tests exist and are shown to fail on the unfixed code; "Try-Fix" runs multiple fix attempts (including multi-model sequences) and records outcomes; a final PR-ready report is produced for humans. The post also details test generation (Appium UI tests, XAML runtime/XamlC/source-gen coverage) and CI debugging (Helix logs), anchoring the flow to real build/test infrastructure rather than pure code generation. It aligns with last week's governance direction by making guardrails and phase gates repo-visible and repeatable.

A migration guide walked through using Copilot to refactor C++ during IBM Power/IBM i big-endian to Azure x86 little-endian ports, where apps can "work" while silently corrupting numeric values. Copilot helps with mechanical refactors (finding unsafe `memcpy` into structs, generating byte-swap helpers with C++20 `std::endian` and intrinsics), while the article stresses the parts you cannot outsource (ABI/padding checks via `static_assert`, EBCDIC conversions, packed decimals). It is the modernization arc from last week, with a reminder that correctness comes from platform constraints, tests, and review, not prompting.

A design-focused discussion described designers using VS Code + Copilot (Chat and CLI) for code-based prototyping in repos, using plan-mode-like structure and parallel Copilot sessions paired with Git worktrees to test UI variants without branch friction. It mirrors last week's multi-surface Copilot story (IDE + CLI + web) and shows parallel sessions as a broader collaboration pattern.

A tutorial also showed moving from prompt prototype to hosted agent using **VS Code AI Toolkit + Microsoft Foundry**, with Copilot used for model comparison and scaffolding Agent Framework templates (Python/C#), then using Foundry evaluation, LLM-judge scoring, red teaming, and monitoring in production. This continues last week's "Beyond Prompts" message (execution loops, tools, observability), and it ends in deployment monitoring rather than an interactive session.

- [Accelerating .NET MAUI Development with AI Agents](https://devblogs.microsoft.com/dotnet/accelerating-dotnet-maui-with-ai-agents/)
- ['Porting C++ from IBM Power to Azure x86: fixing silent endianness corruption with GitHub Copilot'](https://techcommunity.microsoft.com/t5/azure-migration-and/porting-c-from-ibm-power-to-x86-solving-silent-endianness/ba-p/4501666)
- ['Why Design Feels Different: Designers using VS Code and GitHub Copilot for code-based prototyping'](https://www.youtube.com/watch?v=CMvnRYgB5Ac)
- ['From Prototype to Production: Building a Hosted Agent with AI Toolkit & Microsoft Foundry'](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-prototype-to-production-building-a-hosted-agent-with-ai/ba-p/4501969)

### Other GitHub Copilot News

Usage reporting and governance kept catching up to how Copilot is actually used across terminals and model pickers. Building on last week's enterprise-management angle (usage metrics, routing/session controls, and transparency into which model responded), this week closes two reporting gaps: org-wide CLI adoption and the "Auto" model bucket.

Org admins can now see **Copilot CLI activity at org scope** (daily active users, sessions/requests, token totals, average tokens/request) in 1-day and 28-day windows, and via the usage metrics REST API. Separately, usage metrics now **resolve "Auto" model selection** into the actual model used in dashboard and API breakdowns, removing an ambiguous bucket that made cost and policy analysis harder (though GitHub says it still does not split auto-selected vs manual). This ties back to last week's JetBrains auto-routing story: once routing is automated, reporting needs to support cost and compliance governance.

- [Copilot usage metrics now includes organization-level GitHub Copilot CLI activity](https://github.blog/changelog/2026-03-17-copilot-usage-metrics-now-includes-organization-level-github-copilot-cli-activity)
- [Copilot usage metrics now resolve auto model selection to actual models](https://github.blog/changelog/2026-03-20-copilot-usage-metrics-now-resolve-auto-model-selection-to-actual-models)

GitHub's planning surface also added workflow changes related to agent delegation. **Projects Hierarchy view** is GA and enabled by default for new views, which makes sub-issue trees easier to manage. Issue templates can also auto-assign **@copilot** via YAML (`assignees: ["@copilot"]`) to route new issues to the coding agent (where permitted). GitHub also adjusted blank-issue behavior to preserve structured intake for contributors while still letting maintainers create blank issues. After last week's "trigger agentic review from CLI" and "agents doing routine triage," this is a practical intake step: route work into an agent-friendly pipeline up front, not after manual rerouting.

- [Hierarchy view in GitHub Projects is now generally available](https://github.blog/changelog/2026-03-19-hierarchy-view-in-github-projects-is-now-generally-available)

Agent monitoring also showed up in Raycast: the GitHub Copilot Raycast extension now lets you **stream agent logs live** from "View Tasks," which mainly reduces context switches and helps spot stalled sessions without living in the web UI. It fits last week's observability trend (debug snapshots, OpenTelemetry traces): as agents run longer, "where do I see what it's doing?" becomes a normal workflow question.

- [Monitor Copilot coding agent logs live in Raycast](https://github.blog/changelog/2026-03-20-monitor-copilot-coding-agent-logs-live-in-raycast)

A couple of getting-started items focused on standardizing Copilot's environment. One video covered installing Copilot CLI on Windows (winget) and adding the **Work IQ** plugin (including EULA acceptance), with notes on interactive vs non-interactive usage, continuing last week's CLI onboarding. Another short reminded maintainers that `.github` is the repo's workflow control plane (Actions, templates, and a home for Copilot instructions and related guidance), echoing last week's move to repo-visible instruction sources.

- [Getting Started with GitHub Copilot CLI and Work IQ](https://www.youtube.com/watch?v=tQlNq8bH674)
- [The powerful GitHub folder most developers ignore](https://www.youtube.com/shorts/X4XiMKott2E)

Microsoft Fabric's extensibility tooling also reached a new baseline with the **Fabric Extensibility Toolkit GA**, including a "Copilot-optimized" starter kit (repo context under `.ai/` plus DevContainer/Codespaces setup) and React/Fluent UI building blocks for Fabric workload items, plus OneLake-backed storage patterns and Entra OBO auth flows. It connects to last week's "structured context + reusable skills" story and this week's Fabric MCP mention: platforms are shipping repo scaffolding that assumes agents are part of the workflow.

- [Microsoft Fabric Extensibility Toolkit is now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-extensibility-toolkit-generally-available/)

For developers building Copilot extensions, a VS Code episode walked through creating a project with the **GitHub Copilot SDK** from scratch, pointing to the canonical repo for templates and APIs. This continues last week's "Copilot SDK = execution loops and tools" message, but as a more direct starter path for teams productizing internal agents.

- ['Let it Cook - GitHub Copilot SDK: Fresh from Scratch'](https://www.youtube.com/watch?v=uzmnpGmR2tg)

## AI

This week's AI updates focused less on feature demos and more on making agent systems easier to run. Microsoft moved Azure AI Foundry's agent runtime into GA with enterprise networking, identity, and evaluation hooks; MCP kept showing up as the tool-wiring layer; and Fabric continued to blend analytics and AI app building with more multimodal, real-time, and Copilot-driven workflows. Overall, it feels like a continuation of last week's "run it like software" focus (approval gates, sandboxing, OpenTelemetry, structured outputs): more of those patterns are arriving as defaults (private networking, managed identity options, continuous eval, and tool connectivity without bespoke glue).

### Azure AI Foundry Agents move into production: GA runtime, private networking, voice, and built-in evaluation

Foundry Agent Service reached GA with a runtime built on the OpenAI Responses API, aiming to be wire-compatible for teams already aligned to Responses/Agents-style interfaces. Last week's "treat agents like deployable software" theme shows up here as consolidation: rather than assembling orchestration, auth, and telemetry by hand (like last week's Agent Framework + Foundry examples), Foundry is standardizing how agents are created, invoked, and governed. For Python, GA also tightens SDK guidance: agents become first-class operations on `AIProjectClient` in `azure-ai-projects`, with an explicit migration off `azure-ai-agents` (remove pin, use `azure-ai-projects`, call via `AIProjectClient.get_openai_client()` and `responses.create(..., extra_body={"agent_reference": ...})`).

The most practical production change is end-to-end private networking with a "Standard Setup" that keeps agent traffic off the public internet once agents start doing retrieval and tool calls. It extends last week's "secure execution + governed gateways" theme: last week emphasized Foundry as a controlled model gateway plus approval and sandbox patterns at the tool layer, and this week adds the network boundary so models, retrieval, and tools can stay on private paths. Microsoft says this covers model runtime traffic and tool connectivity, including MCP servers, Azure AI Search indexes, and Fabric data agents, enabling a BYO VNet design without public egress for the workflow.

Identity and tool access also became more enterprise-shaped. Foundry expanded MCP authentication patterns: key-based access, Entra Agent Identity, Entra Foundry Project Managed Identity (project-level isolation), and OAuth identity passthrough for user-delegated access. This connects to last week's least-privilege and approvals theme: when tools are real actuators (deployments, incidents, repo ops), these identity modes keep "agent can do things" from defaulting to shared secrets and broad access.

On interaction modes, Voice Live arrived as a managed, real-time speech-to-speech channel using the same agent runtime as text. Voice does not change the core problems (tool calling, tracing, approvals, compaction), but it does increase latency and reliability requirements. Having one runtime surface with shared tracing, evaluation, and cost accounting helps avoid building a separate voice stack that is hard to monitor.

Foundry Evaluations are now GA, with built-in evaluators (fluency/coherence, relevance, groundedness, retrieval quality, safety), custom evaluators, and continuous evaluation sampling of production traffic. This is the platform counterpart to last week's operational loops (OpenTelemetry/Aspire dashboards, azd debugging, Fabric telemetry pipelines): instead of per-app eval harnesses, quality checks become continuous and viewable alongside latency, failures, and cost in Azure Monitor / Application Insights.

- [Foundry Agent Service is GA: private networking, Voice Live, and enterprise-grade evaluations](https://devblogs.microsoft.com/foundry/foundry-agent-service-ga/)
- [Microsoft announces new solutions, infrastructure at NVIDIA GTC](https://blogs.microsoft.com/blog/2026/03/16/microsoft-at-nvidia-gtc-new-solutions-for-microsoft-foundry-azure-ai-infrastructure-and-physical-ai/)

### MCP as the agent tool layer: remote servers in Foundry, managed observability gateways, and a C# SDK v1.0

MCP kept moving from "developer curiosity" to product integration, continuing last week's story of MCP as a bridge between agent frameworks and external systems. This week's tone is less "how to wire MCP" and more "here are MCP endpoints you can use," which is typically when protocols become operationally relevant.

Microsoft Foundry added a remote Azure DevOps MCP Server (public preview), letting Foundry agents connect to an Azure DevOps org via the tool catalog and call DevOps operations through MCP. It fits last week's operational-agent direction (investigation, PR-shaped fixes): DevOps is where "agent does work" becomes risky without boundaries. A key control is restricting which DevOps tools the agent can use, which helps prevent early experiments from turning into "agent can access everything," echoing last week's approval-gated tools and structured outputs.

Azure Managed Grafana MCP takes a different angle. Instead of deploying and securing a custom MCP server to expose telemetry, every Azure Managed Grafana instance can provide a built-in managed remote MCP endpoint. This connects to last week's point that agents need day-two loops (telemetry, debugging, evaluation): MCP makes the observability estate queryable by agents while still using Azure RBAC and Grafana access controls. The approach is straightforward: authenticate with managed identity and let agents query Azure Monitor, Application Insights, and Kusto-backed sources without adding another hosted service.

For .NET teams, the MCP C# SDK v1.0 shipped with a community standup walkthrough focused on MCP as a vendor-neutral contract for exchanging context, requests, and responses. This matches last week's polyglot MCP tool-server hint: Python ecosystems help, but a supported .NET SDK makes it easier to standardize tool wiring across enterprise services without tying everything to one runtime.

The ecosystem conversation remains noisy (GitHub's "The Download" jokes about an "MCP funeral"), but Microsoft shipped multiple concrete MCP updates in the same week. Alongside last week's repeated MCP appearances in Agent Framework + Foundry designs, the developer takeaway is that MCP is increasingly a practical option for tool connectivity, with identity, governance, and hosted endpoints becoming less DIY, while still keeping an eye on portability and cross-vendor compatibility.

- [Remote MCP Server preview in Microsoft Foundry](https://devblogs.microsoft.com/devops/remote-mcp-server-preview-in-microsoft-foundry/)
- [Introducing Azure Managed Grafana MCP: The Managed Data Gateway for AI Agents](https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-azure-managed-grafana-mcp-the-managed-data-gateway/ba-p/4503619)
- [.NET AI Community Standup: MCP (Model Context Protocol) C# SDK v1.0](https://www.youtube.com/watch?v=iMcowyYD5Q4)
- [The Download: MCP funeral, Perplexity computer, and Doom on a badge](https://www.youtube.com/watch?v=da8cSPcO7Lw)

### Knowledge-grounded agents with Foundry IQ: permission-aware retrieval via Azure AI Search + MCP

Foundry IQ aims to make "enterprise RAG" less bespoke by formalizing reusable knowledge bases over multiple sources (SharePoint, OneLake, Blob Storage, Azure AI Search, and more). It extends last week's pattern of moving context out of prompts and into governed systems (Azure SRE Agent "Deep Context," Fabric telemetry-as-data-plane). Instead of each agent writing retrieval glue, IQ treats knowledge access as a platform component called through a standard tool surface.

The tutorial shows how this connects to Foundry Agent Service via MCP: the agent calls `knowledge_base_retrieve` exposed through an Azure AI Search endpoint, using a preview API path like `/knowledgebases/{kb-name}/mcp?api-version=2025-11-01-preview`.

The parts that matter most for developers are security and ops patterns. Retrieval is permission-aware: ACLs can sync into the index and be enforced at query time so the agent retrieves only what the current user is allowed to see, with citations generated as part of retrieval. This matches last week's "structured outputs + approvals + least privilege" theme: production RAG is mostly access control, attribution, and repeatability. The sample also shows Entra ID + RBAC setup instead of keys: enable RBAC auth on Azure AI Search, grant project managed identity `Search Index Data Reader`, create an Azure AI Projects project connection as a RemoteTool target authenticated with Project Managed Identity, then attach an `MCPTool` and require tool usage plus citations in instructions.

Microsoft also announced a three-episode "IQ Series: Foundry IQ" starting March 18, 2026, with videos, notebooks, and cookbooks aimed at taking teams from concepts to multi-source knowledge bases and queries. The message is that retrieval is becoming a reusable platform surface (sources/bases + MCP endpoint + RBAC/ACL), not app-specific glue.

- [Building Knowledge-Grounded AI Agents with Foundry IQ](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-knowledge-grounded-ai-agents-with-foundry-iq/ba-p/4499683)
- [Announcing the IQ Series: Foundry IQ](https://techcommunity.microsoft.com/t5/microsoft-developer-community/announcing-the-iq-series-foundry-iq/ba-p/4501862)

### Building, testing, and operating agents: VS Code AI Toolkit, azd local run/invoke, and resilient long-running runs

Agent tooling posts kept shortening the path from prototype to repeatable testing, continuing last week's day-two operability thread (azd status/logs, Aspire dashboards, production harnesses). The VS Code AI Toolkit + Foundry walkthrough shows an end-to-end workflow: start in an Agent Builder UI (assemble agent, attach tools, iterate in playground, ground on business data) then move to a code-first hosted template where you add custom Python functions, debug locally, deploy, run evals, do AI red teaming, and monitor quality/latency/cost across a fleet. It pulls last week's production patterns into an IDE workflow so more teams can apply them consistently.

On CLI workflows, the Azure Developer CLI extension `azure.ai.agents` (v0.1.14-preview) adds `azd ai agent run` and `azd ai agent invoke`. This builds on last week's hosted-agent visibility commands: `run` and `invoke` are what make regression testing and troubleshooting scriptable. `run` detects project type (Python/Node.js) and dependencies before launching. `invoke` supports streaming responses and persistent session IDs so multi-turn testing does not require manual state handling. It fits prompt testing scripts and CI-like agent checks, especially with Foundry Evaluations now GA.

For reliability, Microsoft Agent Framework added background responses for long-running operations without holding client connections open, especially for reasoning models that can take minutes. This extends last week's long-session concerns (compaction, dynamic sessions, latency/cost control) into a resumable job model. In .NET, enable `AllowBackgroundResponses`; in Python, set `background=True`. If supported, you get a continuation token you can poll (non-streaming) or persist during streaming to resume after disconnects from the interruption point. The advice to persist continuation tokens (DB/cache) is a sign that agent runs increasingly behave like durable workflows with checkpoints, retries, and reattachment.

- [Build and Ship a Hosted Agent with VS Code AI Toolkit & Microsoft Foundry](https://www.youtube.com/watch?v=E9Hmk0PLMAQU)
- [Azure Developer CLI (azd): Run and test AI agents locally with azd](https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-run-invoke/)
- [Handling Long-Running Operations with Background Responses](https://devblogs.microsoft.com/agent-framework/handling-long-running-operations-with-background-responses/)

### Agent patterns in practice: Python agent workflows and realtime voice multi-agent systems in .NET

Two longer pieces focused on architecture patterns, mapping closely to last week's "multi-agent composition + observability + evaluation + approvals" blueprint.

Pamela Fox's recap of the "Python + Agents" livestream series is a Microsoft Agent Framework curriculum: tool calling -> MCP servers -> supervisor/subagent patterns -> RAG with SQLite/PostgreSQL -> memory with Redis/Mem0 -> OpenTelemetry via Aspire dashboards -> evaluation with Azure AI Evaluation SDK -> branching/fan-out/fan-in workflows -> human approvals and checkpoint/resume for long-running work. It turns last week's "real apps" examples into a repeatable build approach, including the same ops loops (OTel/Aspire, evaluation SDK) that both weeks keep reinforcing.

On the .NET side, RT.Assistant is a reference for low-latency voice assistants using OpenAI's Realtime API over WebRTC, orchestrated in F#. It complements this week's Foundry Voice Live: both treat voice as a first-class modality, but RT.Assistant focuses on runtime details (WebRTC/OPUS, message bus), while Voice Live emphasizes a managed channel with evaluation and tracing. RT.Assistant also makes multi-agent behavior more predictable with a deterministic state machine ("Flow") and a strongly typed message bus using F# discriminated unions, similar in spirit to last week's structured schemas and explicit handoffs for automatable outputs. It argues for WebRTC + OPUS efficiency versus base64 PCM over WebSockets, and shows "structured RAG" where an LLM generates Prolog queries executed against a local KB (Tau Prolog) instead of embeddings/vector search.

- [Learn how to build agents and workflows in Python](https://techcommunity.microsoft.com/t5/microsoft-developer-community/learn-how-to-build-agents-and-workflows-in-python/ba-p/4502144)
- [RT.Assistant: A Multi-Agent Voice Bot Using .NET and OpenAI](https://devblogs.microsoft.com/dotnet/rt-assistant-a-realtime-multiagent-voice-bot-using-dotnet-and-open-ai-api/)

### Microsoft Fabric as an AI execution surface: multimodal functions, Copilot-in-notebooks, real-time intelligence, and pipeline automation

Fabric's AI story kept moving from individual features to an integrated execution surface for analytics plus AI, extending last week's Fabric thread (operational telemetry/eval loops, ontology automation, schema-controlled extraction). This week mostly deepens that direction with more modalities, more cost and operability surfaces, and more ways to push outputs into governed pipelines instead of one-off notebooks.

Fabric AI Functions added multimodal input (preview), so notebooks and Dataflows Gen2 can process images and PDFs (and common text formats) by passing file paths (including `column_type="path"`). It continues last week's ExtractLabel theme: turn unstructured inputs into pipeline-friendly structured outputs, with schemas as the contract for downstream reliability. Helpers like `aifunc.load()` (folder-to-table with optional prompt/schema), `aifunc.list_file_paths()`, and `ai.infer_schema()` shorten the path from files to reproducible extraction via `ai.extract()`. Operability also improved: a progress bar estimating tokens and Capacity Units (CUs), and clearer capacity attribution in the Fabric Capacity Metrics App under "AI Functions." Evaluation notebooks for LLM-as-judge loops (executor + judge models, with precision/recall/F1/coherence) aim to reduce ad-hoc iteration and complement last week's "reuse telemetry as eval data" guidance.

In notebooks, Fabric previewed an updated Copilot for data engineering/science with always-on context awareness (workspace, Lakehouse, notebook structure, runtime state), Spark performance recommendations based on observed behavior (joins, shuffles), and a "Fix with Copilot" loop that captures failure context, proposes patches, and applies them via diff review, plus a `/Fix` command for a cell or whole notebook. It is a notebook-native version of last week's debugging and operability push: close the loop where people iterate.

Beyond notebooks, Fabric continued pushing real-time and operational AI via Real-Time Intelligence + Fabric IQ (ontology), building on last week's Ontology Rules + Activator "Observe -> Analyze -> Decide -> Act" loop. OneLake ties to Eventstream/Eventhouse/Activator/real-time dashboards with a semantics layer so teams (and agents) interpret events consistently. Developer callouts include Maps GA, Business Events for semantic detection/triggers, Fabric Graph scaling and GQL updates (including shortest-path), and an Eventstream SQL Operator for SQL-based streaming transforms/routing (early April). Microsoft also announced a Microsoft-NVIDIA Omniverse direction (private preview planned in April) to embed 3D scenes into real-time dashboards for digital-twin/physical AI scenarios.

Fabric's pipeline tooling also advanced: Data Factory pipelines added a Lakehouse Utility Suite (preview) with Lakehouse Maintenance activity and Refresh SQL Endpoint activity, while Copilot in the pipeline expression builder is GA for natural-language-to-expression authoring. It is a practical follow-on: once AI extraction and agent signals land in OneLake, you still need scheduled maintenance and automation to keep tables and endpoints healthy.

- [Multimodal support in Fabric AI functions (Preview): process images and PDFs, plus cost and eval updates](https://blog.fabric.microsoft.com/en-US/blog/unlock-insights-from-images-and-pdfs-with-multimodal-support-in-fabric-ai-functions-preview/)
- [Introducing the updated Copilot for data engineering and data science (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-the-updated-copilot-for-data-engineering-and-data-science-preview/)
- [Trusted AI starts with Microsoft Fabric: Unified real-time intelligence and IQ context](https://blog.fabric.microsoft.com/en-US/blog/trusted-ai-starts-with-microsoft-fabric-unified-real-time-intelligence-and-iq-context/)
- [Modernizing pipelines: New activities and innovations in Fabric Data Factory pipelines](https://blog.fabric.microsoft.com/en-US/blog/modernizing-pipelines-new-activities-and-innovations-in-fabric-data-factory-pipelines/)

### Other AI News

Fabric's broader analytics/AI update included many GA vs preview details: Materialized Lake Views GA for incremental, quality-constrained lakehouse transforms; Runtime 2.0 preview moving toward Spark 4.0 / Delta Lake 4.0; new connectivity (JDBC GA, Spark ODBC and ADO.NET preview); and warehouse updates like compute isolation (Custom SQL Pools preview), freshness/stats automation, and AI functions callable from T-SQL. The roundup also introduced open-source "Agent Skills for Fabric" for GitHub Copilot CLI to scaffold and automate Fabric tasks from natural language, similar to last week's Azure Skills direction but focused on Fabric operations.

Fabric Mirroring added opt-in paid "extended capabilities" (preview): Delta Change Data Feed into OneLake and Mirroring Views (Snowflake preview) to materialize source views as Delta tables, supporting incremental processing without custom change tracking. This matches last week's governed data plane theme: keeping OneLake current helps downstream analytics, quality loops, and automation triggers stay aligned.

Fabric previewed Planning in Fabric IQ for budgeting/forecasting/scenarios on governed data and Power BI semantic models, with SQL writeback plus approval/audit/RBAC hooks. It is useful context for end-to-end operational analytics systems and another example of "semantic layer + governed actions," consistent with last week's Ontology Rules theme.

Microsoft introduced MAI-Image-2, a text-to-image model focused on photorealism and more reliable in-image typography, with testing via MAI Playground and broader API access expected via Foundry.

For Foundry model optimization, Microsoft published videos on supervised fine-tuning, improving tool-calling accuracy (synthetic data and distillation), and post-training workflows (custom graders, evaluation, cost planning, deployment). The emphasis on graders, evaluation, and cost planning matches last week's evaluation-loop focus and this week's Foundry Evaluations GA.

For teams hosting inference stacks, AKS "inference at scale" guidance covered tensor/pipeline/data parallelism tradeoffs, quantization-first advice, Ray placement groups (Anyscale on Azure) for shard-aware scheduling, and production security posture (private clusters, Cilium policy, Entra ID + managed identities, Key Vault), plus core metrics (tokens/sec/GPU, tail latency, KV cache hit rate, tokens per GPU-hour). It fits the broader arc: private networking, managed identity, and measurable ops loops become baseline expectations when AI moves from prototypes to services.

- [From Lakehouse to boardroom: Analytics and AI for real insights](https://blog.fabric.microsoft.com/en-US/blog/from-lakehouse-to-boardroom-analytics-and-ai-for-real-insights/)
- [Extended Capabilities in Mirroring in Microsoft Fabric: Optional Enhancements to Core Mirroring](https://blog.fabric.microsoft.com/en-US/blog/extended-capabilities-in-mirroring-in-microsoft-fabric-optional-enhancements-to-core-mirroring/)
- [Introducing Planning in Microsoft Fabric IQ: From historical data to forecasting the future](https://blog.fabric.microsoft.com/en-US/blog/introducing-planning-in-microsoft-fabric-iq-from-historical-data-to-forecasting-the-future/)
- [Introducing MAI-Image-2: for limitless creativity](https://microsoft.ai/news/introducing-mai-image-2/)
- [Model Optimization in Microsoft Foundry: Supervised Fine-Tuning](https://www.youtube.com/watch?v=GyCLzGQwaEc)
- [Model Optimization in Microsoft Foundry: AI Agents Tool Calling Accuracy](https://www.youtube.com/watch?v=vKfG50oLSmk)
- [Model Optimization in Microsoft Foundry: Deployment and Evaluations](https://www.youtube.com/watch?v=6awzUxeG-b0)
- [Building an Enterprise Platform for Inference at Scale](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-an-enterprise-platform-for-inference-at-scale/ba-p/4498820)
- [Building Microsoft’s Sovereign AI on Azure Local with NVIDIA RTX PRO and Next Gen NVIDIA Rubin](https://techcommunity.microsoft.com/t5/azure-arc-blog/building-microsoft-s-sovereign-ai-on-azure-local-with-nvidia-rtx/ba-p/4502383)
- [The power of the Microsoft Fabric ecosystem: ISVs building natively on Fabric](https://blog.fabric.microsoft.com/en-US/blog/the-power-of-the-microsoft-fabric-ecosystem-isvs-building-natively-on-fabric/)

## ML

This week's ML-adjacent data engineering updates were less about model releases and more about tightening pipelines and developer surfaces. Fabric moved Spark and notebook capabilities closer to production usage, and Azure Databricks shared a concrete pattern for consolidating near-real-time ingestion, transformation, and governance into a single Lakeflow workflow.

### Microsoft Fabric: Lakehouse pipelines, runtimes, and notebook automation

Materialized Lake Views (MLVs) GA is Fabric's clearest move toward declarative lakehouse transforms without hand-rolled Spark ETL plus separate orchestration. The GA release focuses on refresh behavior and manageability: Fabric expands incremental refresh support across common patterns (aggregations with `GROUP BY`, left outer joins, left semi joins, CTEs) and decides per run whether incremental or full recompute is cheaper based on change volume and estimated cost. With Change Data Feed enabled by default for new MLVs, incremental processing becomes the default rather than another setting. Operationally, multi-schedule support at the lakehouse level lets you define named schedules for subsets of views (hourly gold vs six-hour lower-priority), with Fabric handling dependencies, parallelizing independent views, and centralizing errors; overlapping triggers are skipped if a refresh is already running. "Replace" allows updating an MLV definition in place without drop/recreate, preserving identity/metadata/lineage and avoiding broken dependencies. Data quality constraints get fuller reporting across refresh history, including richer expression-based constraints for PySpark-authored MLVs (multi-column expressions, arithmetic/functions, session-scoped Python UDFs), while PySpark authoring itself is preview; PySpark-authored MLVs still full-refresh for now.

Fabric Runtime 2.0 entered preview as a new baseline for Spark engineering and science workloads, and it is the kind of upgrade that usually requires retesting. It brings Spark 4.0, Delta 4.0, Azure Linux 3.0 (Mariner 3.0), Java 21, Scala 2.13, and Python 3.12, with Spark 4.1 / Delta 4.1 / Python 3.13 planned soon. Because you can enable preview at workspace or environment level, teams can stage rollout: validate connector JARs with Scala 2.13, check Java 21 requirements, and confirm Python wheels/native dependencies before moving to production.

Fabric also added guidance to make Spark work more reproducible. Environments best practices splits workflows into Quick mode (fast publish, installs at session start, good for iteration and testing overrides) and Full mode (3-6 minute publish for a validated snapshot, then 1-3 minute session startup), with a practical middle ground: Full mode plus a custom live pool for reproducibility with ~5s session startup. It also recommends using Resources folder/inline installs only for early-stage or one-off work, then promoting validated dependencies into Full mode for scheduled jobs and shared production runs.

Notebook automation is also becoming a first-class integration surface with Fabric Notebook Public APIs GA. Teams can manage notebooks via REST (create/update/get/list/delete) and execute them through the Fabric Job Scheduler API with parameters, session config, and explicit execution context (environment/lakehouse). Two details matter for CI/CD-style orchestration: service principal auth for unattended automation, and the Run Notebook API returning an exit value (via notebook utilities) so external orchestrators can branch or gate on structured output, not just success/failure. Together, the story is coherent: define transforms declaratively (MLVs), adopt a new Spark/Delta baseline when ready (Runtime 2.0), control dependencies (Environments), and orchestrate notebooks via APIs.

- [Materialized Lake Views in Microsoft Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/materialized-lake-views-in-microsoft-fabric-generally-available/)
- [Fabric Runtime 2.0 (Preview)](https://blog.fabric.microsoft.com/en-US/blog/fabric-runtime-2-0-preview/)
- [Best Practices for Library Management with Fabric Environments](https://blog.fabric.microsoft.com/en-US/blog/33772/)
- [Fabric Notebook Public APIs (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebook-public-apis-generally-available/)

### Azure Databricks Lakeflow: near-real-time ingestion, SCD transformations, and governance in Delta Lake

Azure Databricks published a detailed walkthrough for collapsing "too many tools" into a Lakeflow-native pipeline, covering ingestion, transformation, orchestration, monitoring, lineage, and Unity Catalog access control. The architecture starts with two Bronze ingestion paths into Delta: application telemetry streamed into Delta via Lakeflow Connect "Zerobus Ingest" over gRPC, and SQL Server CDC ingested incrementally from an on-prem transaction log (assuming ExpressRoute). For telemetry, it includes concrete prerequisites (Unity Catalog + serverless), SQL for creating UC tables (for example, `prod.bronze.telemetry_events`), and service principal grants (`GRANT USE CATALOG/SCHEMA` plus `GRANT MODIFY, SELECT`). It shows deriving a Zerobus endpoint from workspace URL (`<workspace-id>.zerobus.<region>.azuredatabricks.net`) and Python using `databricks-zerobus-ingest-sdk` to stream client-credential auth, define JSON record types, ingest records, and close streams, with targets like sub-5s latency and up to ~100 MB/sec per connection; records become queryable immediately via Unity Catalog.

For SQL Server CDC, the focus is correctness and incremental efficiency: TCP 1433 connectivity, enabling CDC with `sys.sp_cdc_enable_db` / `sys.sp_cdc_enable_table`, plus SQL permissions (CDC read) and Databricks privileges (`CREATE CONNECTION` at metastore, plus destination `USE CATALOG` / `CREATE TABLE`). Setup then uses the Databricks UI (Data Ingestion -> Add Data): configure an ingestion gateway, connection details, select tables, optionally enable SCD Type 2 history per table, map outputs to Bronze tables (`orders_raw`, `customers_raw`), and schedule runs (example every 5 minutes).

Transformations use Lakeflow Spark Declarative Pipelines and a medallion pattern with SQL-defined incremental processing: `CREATE OR REFRESH STREAMING TABLE` for Silver; `APPLY CHANGES INTO` keyed with `SEQUENCE BY updated_at` for SCD Type 1 "latest state" and SCD Type 2 customer history; telemetry data quality constraints with `EXPECT ...` and violating rows dropped. Gold uses `CREATE OR REFRESH MATERIALIZED VIEW` joining orders/customers/telemetry and aggregating metrics (including conditional sums like purchase event counts). Continuous mode keeps it near real time; Unity Catalog registers everything so lineage flows from Gold back to Bronze automatically. Governance details include granting analysts access only to Gold and applying PII masking via UDF (for example, `mask_email`) that reveals full data only for privileged groups, enforced with `ALTER TABLE ... ALTER COLUMN ... SET MASK`.

Orchestration and monitoring use Lakeflow Jobs to chain dependencies (CDC ingestion then transforms) with scheduling and notifications. For day-2 operations, it shows querying system tables like `system.lakeflow.job_run_timeline` for runs, states, and durations. Consumption examples stay inside Databricks (AI/BI Dashboards and Genie NL->SQL) while relying on Unity Catalog permissions, keeping access control and lineage consistent for BI and ML feature preparation off the same Gold layer.

- [Near–Real-Time CDC to Delta Lake for BI and ML with Lakeflow on Azure Databricks](https://techcommunity.microsoft.com/t5/azure-databricks/near-real-time-cdc-to-delta-lake-for-bi-and-ml-with-lakeflow-on/ba-p/4502750)

### Other ML News

Fabric Eventhouse (Real-Time Intelligence) previewed a small but useful KQL workflow update: DB Explorer can browse stored functions, show definitions read-only, and run "Preview results" without manually writing the KQL call (including parameter formatting). Parameter prompts plus a 100-row preview cap make it a quick validation step when iterating on function libraries or reviewing inherited functions before using them in dashboards and reports.

- ['Instantly Run and Preview Functions in Microsoft Fabric Eventhouse: No Code Required'](https://blog.fabric.microsoft.com/en-US/blog/instantly-run-and-preview-functions-in-microsoft-fabric-eventhouse-no-code-required/)

## Azure

This week's Azure story split into two lines: keeping platforms resilient as infrastructure evolves (edge routing, registries, ingress, DR, monitoring, hybrid networking), and modernizing data estates into Fabric/OneLake where migration assistants, governance, and real-time pipelines are becoming standard building blocks. It continues last week's "controlled transitions" framing: change traffic layers, registry behavior, or data platforms in phases, with clearer signals and fewer surprise support boundaries.

### AKS ingress modernization: moving off Ingress NGINX to Application Gateway for Containers (AGC)

Ingress NGINX now has explicit support boundaries: upstream ingress-nginx is best-effort until March 2026, then stops shipping releases, bug fixes, and security patches. AKS Application Routing gets only critical security patches until November 2026 (no new features or general fixes). Azure's recommended replacement is Application Gateway for Containers (AGC), a managed L7 load balancer for AKS (GA late 2024) with WAF support added November 2025. Architecturally, AGC moves the traffic engine out of the cluster: the data plane/control resource lives in Azure (frontends, delegated subnet association, auto-generated FQDNs), while an in-cluster ALB Controller watches Gateway API objects (Gateway/HTTPRoute) and AGC policy CRDs and programs the Azure-side gateway.

Migration guidance emphasizes phased adoption: AGC supports both Gateway API (preferred) and legacy Ingress API, enabling incremental cutover while aiming to land on Gateway/HTTPRoute. It outlines two operating models: "Bring Your Own" (platform teams manage the Azure AGC resource with IaC and configure the controller with a fixed resource ID) and controller-managed provisioning (Kubernetes ApplicationLoadBalancer CR drives Azure-side lifecycle), which is simpler to start but more coupled to cluster objects.

Some teams will hit prerequisites: AGC requires Azure CNI or Azure CNI Overlay, so Kubenet clusters may need a network plugin migration, and workload identity must be enabled for controller auth. This mirrors last week's "dependency-first" sequencing: networking and identity often need to be correct before higher-level platform changes behave predictably. Benefits are practical: less node resource use (data plane outside cluster), fewer ingress proxy patch chores, faster convergence during scale events by routing to pod IPs via EndpointSlice, and integrated WAF that may simplify "WAF in front of AKS" designs.

Microsoft's AGC Migration Utility (Jan 2026) is positioned as the first step: run in "files mode" (manifests directory) or "cluster mode" (reads live Ingress), generate Gateway API YAML plus a coverage report, and use report categories (completed/warning/not-supported/error) to find NGINX annotation dependencies that need redesign. The recommended flow: install/configure AGC + ALB Controller, generate resources for BYO (AGC resource ID) or managed (subnet ID), validate in non-prod, run NGINX and AGC in parallel, DNS cutover to AGC frontend FQDN, then decommission the old controller to avoid multiple reconcilers. The guidance is explicit about what cannot be automated: NGINX snippets and Lua do not translate cleanly, TLS and DNS cutover require manual validation/updates, and GitOps pipelines may need refactoring because kinds and manifests change when moving from Ingress to Gateway API.

- [After Ingress NGINX: Migrating to Application Gateway for Containers](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/after-ingress-nginx-migrating-to-application-gateway-for/ba-p/4503110)

### Edge and routing resiliency: Azure Front Door patterns and Front Door’s faster RTO work

October 2025's Front Door incidents are shaping Azure guidance: treat "global edge routing is unavailable" as a real failure mode, not only origin-region outages. It aligns with last week's day-2 readiness theme: design for diagnosability and runbook-driven failover when the control plane is unhealthy. Field lessons describe DNS-steered fallback patterns where Azure Traffic Manager in "Always Serve" mode is the escape hatch when AFD control plane or DNS resolution fails. A common trade-off is WAF consistency during failover: if policy must remain consistent, teams often keep a regional Application Gateway (WAF) path ready, sometimes requiring a runbook step to switch an AppGW IP config to public because Traffic Manager targets must be public. For stricter uptime targets (and heavy CDN dependence), a multi-CDN pattern (AFD + Akamai, for example) reduces single-provider edge dependency, with advice like keeping a small steady-state split (for example, 90/10) to keep caches warm and avoid cache-miss storms during cutover.

Front Door engineering updates also aim to reduce recovery time after edge restarts. Part 2 focuses on bounding RTO at scale by changing config recovery: durable per-tenant translated config artifacts (FlatBuffers memory-mapped from disk) persist across restarts; per-tenant validation occurs on load; and only failing tenant entries are evicted and retranslated instead of invalidating everything. The second lever is scaling recovery by active tenants using ML-informed lazy loading: workers preload predicted warm tenants per edge site while keeping hostmaps for correctness so cold tenants load on first request. Timelines are concrete: config propagation reduced from ~45 minutes to ~20 minutes (target ~15 minutes by end of April 2026), RTO targeting <10 minutes worst case by April 2026, and a "micro-cellular" tenant isolation redesign targeted for June 2026. For critical services behind AFD, the takeaway is to (1) implement tested DNS failover runbooks and plan for cache-miss surges, and (2) track Front Door recovery behavior so SLO expectations match platform reality.

- [Resiliency Patterns for Azure Front Door: Field Lessons](https://techcommunity.microsoft.com/t5/azure-architecture-blog/resiliency-patterns-for-azure-front-door-field-lessons/ba-p/4501252)
- [Azure Front Door Resiliency (Part 2): Faster recovery (RTO) via durable config cache and ML-driven lazy loading](https://techcommunity.microsoft.com/t5/azure-networking-blog/azure-front-door-resiliency-series-part-2-faster-recovery-rto/ba-p/4503091)

### Container supply chain reliability: health-aware ACR geo-replication failover

This continues last week's ACR thread: after adding deeper health monitoring and Service Health communication so teams can correlate CI/CD and pull failures, ACR is now using that signal to change routing during regional issues.

ACR geo-replication now fails over based on whether a region can serve end-to-end registry operations, not just whether a proxy returns 200 OK. Previously, the global endpoint (for example, `contoso.azurecr.io`) relied on Traffic Manager performance routing with shallow probes, which could keep sending traffic to a "green" reverse proxy while dependencies (storage, auth, caching, metadata) were degraded, causing pulls and pushes to return 500s despite green probes.

ACR now wires Health Monitor into Traffic Manager via a deep health endpoint that rolls up dependency health and marks replicas unhealthy when they cannot serve real registry requests. Operationally important, this is evaluated per registry, not as a blanket region flag. Health Monitor checks the registry's actual backing resources in that region (including feature-specific dependencies like metadata search), so one registry may reroute while another stays local.

For DevOps teams, the change is transparent (no hostname/config changes), but behavior is still DNS-based and measured in minutes: probe cadence (~30s), failure thresholds, TTLs, and client DNS caching matter. It reinforces two active-active realities: replication is eventual (so immediate cross-region pulls after push can return not-found, and retries help), and pushes during failover can fail mid-operation if DNS re-resolution occurs, so publish steps should be idempotent and retryable. The practical result is fewer manual interventions (like `az acr replication update`) during regional issues and fewer "it's up but pulls fail" incidents.

- [Health-Aware Failover for Azure Container Registry Geo-Replication](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/health-aware-failover-for-azure-container-registry-geo/ba-p/4501730)

### Azure compute for GPU workloads: NCv6 refresh and GA transition

Azure's NCv6 GPU VM family is moving from preview toward GA in coming weeks, shifting to SLA-backed production readiness and changing available sizes. NCv6 uses NVIDIA RTX PRO 6000 Blackwell Server Edition GPUs with Intel Xeon 6 "Granite Rapids" 6900P-series CPUs, targeting graphics/VDI, CAD/CAE, and generative AI inference.

The refreshed lineup expands from three preview sizes to seven sizes across two sub-families (General Purpose, Compute Optimized) and adds fractional GPU options (1/2 and 1/4 GPU). Those fractional SKUs help right-size inference endpoints and interactive visualization where full GPUs are unnecessary, scaling down vCPU, memory, temp disk/NVMe, and networking accordingly. In the context of last week's guidance on placing inference where networking and isolation allow it (including isolated AKS patterns and managed GPU node pools preview), smaller shapes can also help place GPU closer to constrained workloads without full-GPU cost. Some top-end shapes increase vCPU counts (for example, 288 vs 256) to align with the CPU architecture and improve high-end VDI. GA regions start with West US 2 and Southeast Asia, with more planned across Q3 2026 (East US, West Europe, East US 2, North Europe, South Central US, Germany West Central, West US, Korea Central). Teams testing preview sizes should plan for shape changes as new sizes replace prior preview offerings.

- [Azure NCv6 Virtual Machines: Enhancements and GA Transition](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-ncv6-virtual-machines-enhancements-and-ga-transition/ba-p/4503578)

### Microsoft Fabric + OneLake modernization: migrations, governance, real-time pipelines, and platform controls

Fabric updates this week focused on making modernization repeatable: assess-first migration assistants, safer cutover defaults, expanded governance/security, and more ready-to-use building blocks for real-time and monitoring. It mirrors last week's migration posture (move in slices, parallel validation, controlled cutovers) with more platform defaults that support phased transitions.

Migration assistants expanded across common Azure data-estate entry points. For Azure Data Factory and Synapse, public-preview assistants assess compatibility, convert pipelines into Fabric Data Factory equivalents (including linked services -> Fabric connections), and intentionally disable triggers after migration so teams can validate without accidentally starting schedules/events. Synapse Spark migrations similarly move artifacts (Spark pools, notebooks, jobs) into Fabric Data Engineering and map lake databases via OneLake shortcuts without moving data, so you can validate in parallel before cutover. On SQL, Fabric is pushing guided in-portal migration for SQL Server into SQL database in Fabric (preview), combining schema checks, remediation guidance (including Copilot help), and data copy in one flow. The broader roundup also reiterates DACPAC schema import and compatibility improvements (expanded compatibility levels, more T-SQL, full-text search, more `ALTER DATABASE`) to reduce application changes.

OneLake is becoming the connect-without-copying layer. Shortcuts and mirroring expand to more sources (SharePoint Lists preview, Azure Monitor mirroring via shortcuts preview, Dremio preview), while mirroring is GA for Oracle and SAP Datasphere. Shortcut transformations are GA (including conversion to Delta), and there is a preview Excel-to-Delta transformation to reduce notebook glue code. Governance and security are expanding alongside: workspace-level IP firewall rules GA; Outbound Access Protection (OAP) GA across more items (including shortcuts and mirrored databases). OneLake Security is expected to reach GA soon with a unified permission model (including RLS/CLS) intended to follow data across Spark, Power BI, and Fabric agent experiences, with APIs planned so third-party engines can integrate with OneLake enforcement instead of rebuilding auth.

For real-time patterns, Fabric Eventstreams adds DeltaFlow (preview) to turn Debezium-style CDC feeds from operational databases (Azure SQL DB/MI, SQL Server on Azure VMs, PostgreSQL) into analytics-ready streaming tables. Eventstreams can handle schema registration, flatten CDC payloads into table-shaped outputs, enrich with CDC metadata, manage schema evolution, and auto-create/update destination tables. This helps teams route to Eventhouse and query with KQL without maintaining custom CDC connectors and transforms. Visualization also advanced: Maps GA in Real-Time Intelligence adds ontology-based modeling through Fabric IQ, new geospatial connections (Planetary Computer Pro imagery, WMS/WMTS raster), and scheduled refresh for vector tile sets. Eventhouse gets built-in workspace monitoring dashboard templates so teams can start with prebuilt KQL and visuals for Eventhouse and Power BI semantic model operations (DirectQuery, XMLA), then customize.

Fabric Data Factory's FabCon recap delivered more platform controls: OAP to restrict pipeline/dataflow/copy destinations; Key Vault integration for the VNet Gateway GA; on-prem gateway auto-updates; broader identity support (GA service principal + workspace identity across activities); and Copy Job improvements (connectors, incremental copy options including more watermark types and query extraction, truncate-on-full-copy to avoid duplicates). Two developer-facing additions stand out: Copy Job audit columns (row-level metadata like extraction time, job/run IDs, incremental window bounds for lineage/compliance), and a Fabric Data Factory MCP Server (preview) exposing Data Factory operations (pipelines, dataflows, Power Query M author/exec, gateway discovery/health) to MCP-compatible tools like GitHub Copilot and Claude, continuing last week's agent-assisted ops thread.

Warehouse updates include Custom SQL Pools (preview) to isolate compute by allocating multiple pools as percentages and routing workloads via Application Name or regex, which helps when ad-hoc analysis competes with reporting and ETL. The SQL engine continues reducing ingestion-related slowdowns via proactive and incremental stats refresh, moving stats updates to background policies and updating histograms incrementally for large append-heavy tables to avoid compile-time stalls; Microsoft says most workspaces saw compilation-time stats updates cut in half by March 2026. Finally, Fabric Extensibility self-service workload publishing is GA for ISVs: upload/validate packages and share with up to 20 customer tenants pre-certification, with name reservation and automated manifest/security validation to tighten pilot-to-Workload-Hub paths.

- [From Azure Synapse and Azure Data Factory to Microsoft Fabric: The Next-Gen Analytics Leap](https://blog.fabric.microsoft.com/en-US/blog/from-azure-synapse-and-azure-data-factory-to-microsoft-fabric-the-next-gen-analytics-leap/)
- [Introducing Migration Assistant for SQL database in Fabric (Preview)](https://www.youtube.com/watch?v=iyNiU7trims)
- [What’s new and improved for SQL database in Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/whats-new-and-improved-for-sql-database-in-fabric-generally-available/)
- [Building real-time, event-driven applications with Database CDC feeds and Fabric Eventstreams DeltaFlow (Preview)](https://blog.fabric.microsoft.com/en-US/blog/building-real-time-event-driven-applications-with-database-cdc-feeds-and-fabric-eventstreams-deltaflow-preview/)
- [Maps in Microsoft Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/maps-in-microsoft-fabric-generally-available/)
- [FabCon and SQLCon 2026: What’s new in Microsoft OneLake](https://blog.fabric.microsoft.com/en-US/blog/fabcon-and-sqlcon-2026-whats-new-in-microsoft-onelake/)
- [Fabric Data Factory at FabCon Atlanta: Built for modern data integration](https://blog.fabric.microsoft.com/en-US/blog/fabric-data-factory-at-fabcon-atlanta-built-for-modern-data-integration/)
- [Audit columns in Copy job in Fabric Data Factory—Every row is traceable for data lineage and compliance](https://blog.fabric.microsoft.com/en-US/blog/audit-columns-in-copy-job-in-fabric-data-factory-every-row-is-traceable-for-data-lineage-and-compliance/)
- [Custom SQL Pools for Fabric Data Warehouse (Preview)](https://blog.fabric.microsoft.com/en-US/blog/custom-sql-pools-for-fabric-data-warehouse-preview/)
- [Proactive and incremental statistics refresh for Fabric Data Warehouse and SQL Analytics Endpoint](https://blog.fabric.microsoft.com/en-US/blog/proactive-and-incremental-statistics-refresh-for-fabric-data-warehouse-and-sql-analytics-endpoint/)
- [Workspace Monitoring dashboard templates in Microsoft Fabric Eventhouse](https://blog.fabric.microsoft.com/en-US/blog/workspace-monitoring-dashboard-templates-in-microsoft-fabric-eventhouse/)
- [Advancing Databases for the Next Generation of Applications](https://blog.fabric.microsoft.com/en-US/blog/advancing-databases-for-the-next-generation-of-applications/)
- [Fabric Extensibility: Self-Service Workload Publishing is now generally available](https://blog.fabric.microsoft.com/en-US/blog/fabric-extensibility-self-service-workload-publishing-generally-available/)

### Other Azure News

Platform migrations showed up in a practical "here is the mapping and CLI" form, building on last week's Heroku exit guidance (App Service for rehost, Container Apps for container-native ops). This week's post is the Container Apps "show your work" version, including the kinds of failure modes (provider registration, secret ordering) that tend to show up during phased cutovers.

- [Heroku Entered Maintenance Mode — Migrating a Node.js + Redis App to Azure Container Apps](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/heroku-entered-maintenance-mode-here-s-your-next-move/ba-p/4504021)

Observability and DR guidance focused on making large estates manageable and failover-ready, connecting to last week's context-aware operations theme (SRE Agent needs scoped resources/logs/knowledge; Managed Grafana Entra auth for least privilege). One post outlines bulk remediation (PowerShell + Azure CLI) to standardize Azure Monitor diagnostic settings across large numbers of resources into one Log Analytics Workspace, which helps when consolidation is complicated by per-resource settings and the lack of historical log migration. A DR guide ties region selection (service parity, SKU availability, latency benchmarking, cost modeling) to execution steps like Terraform IaC, runbooks, and recurring DR drills, and it calls out diagnostics and logging as part of failover design.

- [Centralized Monitoring in Azure: Automating Diagnostic Settings Across All Resources](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/centralized-monitoring-in-azure-automating-diagnostic-settings-across-all-resources/ba-p/4504027)
- [Designing an Azure Disaster Recovery Strategy for Enterprise Workloads](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/designing-an-azure-disaster-recovery-strategy-for-enterprise-workloads/ba-p/4504142)

Hybrid networking got a concrete troubleshooting note: Azure VPN Gateway uses fixed BGP timers (60s keepalive / 180s hold) and will not negotiate down to aggressive customer timers. If your SD-WAN/CPE uses 10/30, sessions can flap when the CPE hold timer expires before Azure sends a keepalive. Align timers to Azure defaults (or higher) rather than trying to approximate BFD with aggressive settings. It complements last week's reminder that "persistent routes" do not override Azure SDN routing: control-plane behavior can be the real limiter.

- [Azure VPN Gateway BGP timer mismatches: why aggressive CPE timers cause session flaps](https://techcommunity.microsoft.com/t5/azure-networking/my-first-techcommunity-post-azure-vpn-gateway-bgp-timer/m-p/4503580#M776)

Database platform direction pushed PostgreSQL modernization through developer workflows: an AI-assisted Oracle-to-PostgreSQL migration tool (preview) via the PostgreSQL VS Code extension, plus broader positioning for Azure Database for PostgreSQL (Citus elastic clusters, SSD v2, Entra auth, private endpoints) and an early look at HorizonDB (private preview) for very high-scale PostgreSQL-compatible workloads. It reinforces last week's "Postgres as bridge" theme and complements last week's VS Code database workflow improvements by keeping migration work inside the editor.

- [From legacy to leadership: How PostgreSQL on Azure powers enterprise agility and innovation](https://azure.microsoft.com/en-us/blog/from-legacy-to-leadership-how-postgresql-on-azure-powers-enterprise-agility-and-innovation/)

Governance guidance reinforced that production-grade metadata needs security and automation foundations (private endpoints, app registrations, Key Vault), and recommends iterative wins like early PII classification scans before expanding into full data products and lineage. It matches last week's governance/compliance thread: make security controls repeatable and automatable so they do not get renegotiated each migration wave.

- [Purview Data Governance: Why It Feels Hard and Why It’s Worth It](https://zure.com/blog/purview-data-governance-why-it-feels-hard-and-why-its-worth-it)

Hybrid/edge evaluation content offered pragmatic ways to test Azure Local before buying hardware: use Azure Jumpstart LocalBox as a cloud-hosted sandbox or a nested Hyper-V HomeLab locally, then move to a certified-hardware PoC via the Azure Local Solutions Catalog. It extends last week's hybrid and edge storyline (Azure Local LENS workbook; Arc server reporting and VM app deployment preview) with a lower-friction way to stand up a representative environment early.

- [How to Evaluate, Test, and Demo Azure Local](https://www.thomasmaurer.ch/2026/03/how-to-evaluate-test-and-demo-azure-local/)

The weekly video roundup remains a useful checklist for retirements and cross-service changes: AKS node OS/image retirements, compute SKU retirements, identity/security updates, and ongoing Azure AI Foundry/model catalog changes. It fits this week's deprecation-driven ingress migration and last week's "watch small contract changes" compute API note: timelines and defaults shifting under stable workloads create real operations work.

- [Azure Update 20th March 2026](https://www.youtube.com/watch?v=jkpcFAYJjvM)

## .NET

This week's .NET updates landed where teams feel everyday friction: shells and editors that drive automation and debugging, and UI controls that need to stay responsive under real data. Building on last week's "apply updates" focus (.NET servicing guidance and the macOS VS Code debugger hotfix) plus "try new features" (.NET 11 Preview 2 wave), this week continues the same two-track story: a clearer production baseline for tooling and a Preview 2 feature that is easy to validate in apps.

### PowerShell 7.6 LTS on .NET 10: production automation baseline, with shell and module refinements

PowerShell 7.6 is now GA as the next LTS release, built on .NET 10 (LTS), giving teams a stable target for production scripting and automation where predictable behavior matters. Like last week's servicing guidance ("keep runtime/tooling patched," including the out-of-band .NET 10.0.5 VS Code macOS debugger fix), 7.6 provides a steadier baseline for build agents, automation runners, and operator scripts, especially for fleets standardizing on .NET 10 LTS.

Alongside engine reliability and cross-platform consistency work, the release updates key in-box modules (PSReadLine, PSResourceGet, ThreadJob), so environments may see behavior changes simply by moving to 7.6 even if modules were not intentionally updated before. This mirrors last week's "hidden dependency" point: module and runtime layers are often where friction appears first, including on build agents and base images.

Much of the polish is focused on interactive authoring and discoverability. Tab completion has many fixes and expansions (paths across providers, parameter value completions, more contexts where completion works, module completion by shortname), which reduces manual lookup and trial-and-error. Automation-visible updates include `Get-Clipboard -Delimiter`, `Register-ArgumentCompleter -NativeFallback` for native-command completion, `Get-Command -ExcludeModule`, `New-Item` treating `-Target` literally, and `Start-Process -Wait` improving polling efficiency to reduce overhead in scripts waiting on child processes.

On platform conventions, 7.6 adds aliases `PSForEach()` and `PSWhere()` for intrinsic `ForEach()` and `Where()` methods and respects `NO_COLOR` for stderr output in the console host. API and compatibility updates include Unix SystemPolicy public APIs being visible but no-op for `PowerShellStandard.Library`, and certificate DNS name handling using `X509SubjectAlternativeNameExtension.EnumerateDnsNames()`.

Several formerly optional or preview features are now treated as mainstream (`PSFeedbackProvider`, `PSNativeWindowsTildeExpansion`, `PSRedirectToVariable`, `PSSubsystemPluginModel`), which reduces "feature flag semantics" surprises in production. As with any LTS bump, there are breaking changes to validate: `Join-Path` changes `-ChildPath` to `string[]`, `WildcardPattern.Escape()` now escapes lone backticks correctly (changing outputs for scripts relying on prior behavior), and the `GetHelpCommand` trace source name drops a trailing space (affecting trace-name matching). The post links install guidance and What's New docs and notes ongoing PowerShell 7.7 previews.

- [Announcing PowerShell 7.6 (LTS) GA Release](https://devblogs.microsoft.com/powershell/announcing-powershell-7-6/)

### .NET MAUI 11 Preview 2: built-in map pin clustering for Android and iOS/Mac Catalyst

Following last week's .NET 11 Preview 2 rundown (including the MAUI Map control landing in Preview 2), this is one of the most visible additions in apps: .NET MAUI 11 Preview 2 adds built-in pin clustering in the `Map` control for Android and iOS/Mac Catalyst. With `IsClusteringEnabled="True"`, overlapping pins collapse into cluster markers with a count when zoomed out, then expand as users zoom in, which improves usability and performance on crowded maps.

The feature supports practical organization via `ClusteringIdentifier` strings per `Pin`: pins with the same identifier cluster together (for example, "coffee"), while different identifiers will not merge even if nearby (keeping "parks" separate). For interaction, `Map` exposes `ClusterClicked`; `ClusterClickedEventArgs` includes the cluster `Pins`, `Location`, and a `Handled` flag to suppress default zoom-to-cluster behavior (the sample shows listing pin labels via `DisplayAlert`).

Under the hood, Android uses a custom grid-based algorithm recalculated on zoom changes and avoids external dependencies, while iOS/Mac Catalyst uses MapKit's native `MKClusterAnnotation` for platform-native behavior and animations. To try it you need .NET 11 Preview 2 and updated MAUI workloads; the post also calls out Visual Studio 2026 Insiders on Windows or VS Code with C# Dev Kit, continuing last week's "validate previews in real toolchains" guidance. Samples are updated (the `maui-samples` Maps demo adds a Clustering page) and Microsoft Learn docs are refreshed.

- [Pin Clustering in .NET MAUI Maps](https://devblogs.microsoft.com/dotnet/pin-clustering-in-dotnet-maui-maps/)

### Other .NET News

Last week's .NET servicing roundup included the out-of-band macOS VS Code debugger fix, and this week's VS Code Insiders notes continue the "make the daily loop smoother" direction for cross-platform debugging and local workflows.

VS Code Insiders kept refining cross-platform workflows that .NET teams tend to hit quickly. The integrated browser can now bypass certificate errors for localhost HTTPS with self-signed certs, which helps local secure-context loops (OAuth redirects, secure cookies, service workers) without switching browsers or reworking trust stores. Debug config sharing is cleaner with new `launch.json` top-level `"windows"`, `"linux"`, `"osx"` properties so repos can keep platform-specific entries in one file while hiding irrelevant configs on other OSes, reducing noise in the Run and Debug UI. There's also a WSL fix for "Reveal in File Explorer," plus UX/accessibility improvements like better screen reader labels and image carousel zoom for inspecting assets.

- ['Visual Studio Code 1.113 (Insiders): update notes for March 2026'](https://code.visualstudio.com/updates/v1_113)

## DevOps

This week's DevOps story split into two threads. GitHub tightened daily shipping and review mechanics (self-hosted runners, scheduling, review ergonomics, GHES governance), while Microsoft Fabric pushed "artifacts as code" with more Git-native workflows and REST APIs for repeatable promotion. Building on last week's "operate safely at scale" theme (runner compliance, OIDC governance signals, reliability learnings), this week focuses on reducing friction once controls exist: clearer GHES merge feedback, more predictable runner targeting on Kubernetes, and more flexible scheduling and environment usage in Actions. On the Microsoft side, Fabric extends last week's "deploy from VS Code / database projects" direction into bulk promotion, event-driven lifecycle automation, and Git-style review loops inside Fabric.

### GitHub Enterprise Server 3.20: tighter governance, safer releases, and backup planning

GitHub Enterprise Server 3.20 GA brings changes teams will notice in merge readiness, release integrity, and admin workflows. After last week's push to move governance into platform controls instead of bespoke scripts, GHES 3.20 makes merge-time policy outcomes easier to see. The PR merge area is tuned for faster triage: required status checks are grouped with failures shown first, ordering is more predictable via natural sorting, and commit metadata rule failures show clearer merge-time errors so developers know what to fix. GitHub also notes accessibility improvements (keyboard navigation, focus handling, landmarks), which matter in keyboard-heavy review flows.

Release management adds immutable releases: after publishing, assets cannot be added, modified, or deleted and the tag cannot be moved or deleted, which reduces post-release tampering risk. A gap remains: release attestations still are not supported on GHES (GitHub.com only), so on-prem teams depending on attestations need compensating controls, which is another example of uneven platform capability across surfaces.

Secret scanning gained enterprise-scale improvements: validity checks can indicate whether secrets are still active; enterprise admins can manage exposure via the Management Console; bypass controls for push protection can be governed centrally; alert assignment supports collaborative triage; and default push protection expands to more secret types with new or improved detectors. It continues last week's direction: security automation runs under governed roles and policies, not "whatever the pipeline can call." "Enterprise teams" also entered public preview as an enterprise-wide grouping model (API/UI managed, assign across orgs, use with custom enterprise roles, add to ruleset bypass lists). It is useful but worth staging due to preview limits and bypass-list implications. For Advanced Security, GHES adds an Enterprise Security Manager role (public preview) to centralize policy and alerts, with an enterprise size limit of 15,000 orgs.

The built-in backup service moved from public preview to GA as a managed alternative to older backup utilities, without requiring a separate backup-software host. It matches last week's reliability posture: "operate safely at scale" includes customer-side DR readiness and simpler runbooks. GitHub also set a timeline: `backup-utils` begins retirement starting in GHES 3.22, giving admins time to evaluate the built-in service and update DR automation.

- [GitHub Enterprise Server 3.20 is now generally available](https://github.blog/changelog/2026-03-17-github-enterprise-server-3-20-is-now-generally-available)

### GitHub Actions on Kubernetes and in workflow YAML: runner scale sets and fewer scheduling papercuts

GitHub Actions self-hosted runners on Kubernetes got a practical update with Actions Runner Controller (ARC) 0.14.0 GA. It continues last week's runner compliance and fleet hygiene narrative: once upgrades and image refreshes are routine, the next pain is operational sprawl from too many pools and labels. Multilabel Runner Scale Sets reduce that: one scale set can advertise multiple labels (OS, hardware tier, compliance zone, network placement), and workflows can target runners via combined `runs-on` attributes. ARC also standardized its API integration around the public `actions/scaleset` Go library, which is useful if you build autoscaling or automation and want to align with a supported client.

ARC 0.14.0 also adds metadata and policy knobs for orgs enforcing cluster hygiene, echoing last week's "governance moves into primitives" trend. Helm charts expose a `resource` interface for applying custom labels and annotations to ARC-managed resources (Roles, RoleBindings, ServiceAccounts, listener pods), with global defaults via `resource.all.metadata` and per-resource overrides. Experimental chart rewrites ship alongside existing charts, aiming for cleaner templates, unified metadata config, and better Docker-based runner configuration.

Two runtime behaviors address common failure modes. The listener pod defaults to `nodeSelector: kubernetes.io/os: linux` to prevent accidental scheduling onto Windows nodes in mixed clusters (overrideable via `listenerTemplate`). ARC can also stop autoscaling for a runner set when runner configuration is outdated (when a runner exits code 7), avoiding repeatedly provisioning stale runners during rollouts; GitHub notes this depends on an upcoming runner change and will not be fully effective until a couple of runner releases after.

At the workflow YAML level, GitHub Actions addressed two recurring design issues. You can now reference an environment for environment-scoped secrets and variables without creating a deployment record by setting `deployment: false`, which is useful for tests, maintenance, and validations where you want environment governance without deployment bookkeeping. Limitation: repos with custom deployment protection rules cannot use `deployment: false`. The other update is cron timezone support: schedules can specify an IANA timezone instead of being fixed to UTC, which reduces DST-related confusion.

- ['Actions Runner Controller (ARC) 0.14.0: multilabel runner scale sets, new scaleset client, Helm and scheduling updates'](https://github.blog/changelog/2026-03-19-actions-runner-controller-release-0-14-0)
- ['GitHub Actions: Late March 2026 updates'](https://github.blog/changelog/2026-03-19-github-actions-late-march-2026-updates)

### Microsoft Fabric CI/CD: “definitions as code” gets more Git-native and more automatable

Fabric's CI/CD surface expanded in preview with a consistent theme: treat workspace artifacts as code and make automation less special-case. Building on last week's push to deploy SQL schema changes from VS Code, this extends from single-artifact publishing to more standardized promotion patterns across many artifact types, using Git-shaped delivery workflows.

For custom workload partners, the Fabric Extensibility Toolkit (GA) adds preview CI/CD support so custom workload items can use the same Git and promotion lifecycle as first-party items. Workload items are now included in Fabric Git integration commits (serialized with metadata and definitions), can be promoted through Deployment Pipelines across dev/test/prod, and can run stage-specific behavior via an optional deployment hook before applying the next-stage definition. Variable Library support reduces environment-specific rewiring (IDs, connections) by providing per-stage workspace values, so definitions reference variables instead of hard-coded IDs.

The same toolkit update adds automation integration points. The Remote Lifecycle Notification API (preview) provides webhooks for Created/Updated/Deleted events regardless of origin (UI, REST, CI/CD promotions, admin cleanups). That helps partner backends provision infrastructure, enforce licensing, or sync catalogs without polling. Fabric Scheduler support for "Remote Jobs" (preview) lets workload items define job types executed on schedules via backend endpoints, using delegated Entra OBO tokens so backends can act as the scheduling user when accessing Fabric APIs, OneLake, or other Entra-protected services, with status visible in Fabric job history.

For core automation, Fabric introduced preview REST APIs for Bulk Export and Bulk Import of item definitions. The intent is to export definitions at scale, version them in Git, validate via PR and policy, then bulk import into target workspaces as repeatable deployments. Bulk Import uses dependency handling to deploy in the correct order, and the APIs support enterprise patterns like async long-running operations and non-interactive auth (service principals/managed identities), which helps with CI/CD, DR, and large-scale promotion.

Fabric Git integration also gained preview developer experiences that make feature-branch workflows less awkward when "workspace is the branch." Branched Workspaces makes source/branch relationships explicit. Selective Branching lets you branch only intended items (pulling required related items automatically). Compare Code Changes adds diff-style review inside Fabric for outgoing changes pre-commit, incoming updates pre-sync, and conflict resolution with side-by-side context. Together, these features support focused workspaces, earlier review before repo commits, and promotion through standard Git and pipeline practices, which mirrors this week's GHES merge UI improvements and review ergonomics.

- ['Fabric Extensibility Toolkit: CI/CD, Remote Lifecycle Notifications, and Fabric Scheduler (Preview)'](https://blog.fabric.microsoft.com/en-US/blog/fabric-extensibility-toolkit-ci-cd-remote-lifecycle-notifications-and-fabric-scheduler-preview/)
- [Introducing Bulk Export and Import APIs for CI/CD in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-bulk-export-and-import-apis-for-ci-cd-in-microsoft-fabric-preview/)
- [Introducing new Git developer experiences in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-new-git-developer-experiences-in-microsoft-fabric-preview/)

### Other DevOps News

GitHub's PR review surface got a workflow update: docked side-by-side panels in "Files changed" keep PR overview, comment threads, merge status, and code scanning alerts visible next to the diff. That reduces tab switching when juggling merge readiness and security findings. Paired with GHES 3.20 merge-area improvements, it continues the practical theme of reducing UI friction around expanded checks and controls.

- [View code and comments side-by-side in pull request Files changed page](https://github.blog/changelog/2026-03-19-view-code-and-comments-side-by-side-in-pull-request-files-changed-page)

Data residency and mobile ergonomics also got incremental updates for distributed teams. Codespaces with data residency expanded public preview to Japan (joining EU and Australia), helping enterprises keep Codespaces data in-region and aligning with last week's identity and governance controls. GitHub Mobile for Android updated navigation to keep bottom tabs consistent and preserve state better between Home/Inbox and related contexts, which helps on-the-go PR and notification triage.

- [Codespaces with data residency now available in Japan](https://github.blog/changelog/2026-03-19-codespaces-with-data-residency-now-available-in-japan)
- [A smoother navigation experience in GitHub Mobile for Android](https://github.blog/changelog/2026-03-20-a-smoother-navigation-experience-in-github-mobile-for-android)

A practical Git migration guide clarified syncing branches/tags (`git push --all` plus `--tags`) versus true mirroring (`git push --mirror`) that pushes all `refs/*` and deletes destination-only refs. It is useful during phased cutovers where CI and hosting systems may create extra refs you do not want wiped, and it complements last week's theme that provider behaviors can create migration gotchas.

- ['Git Mirroring During Migrations: `--all` vs `--mirror`'](https://dev.to/playfulprogramming/git-mirroring-during-migrations-all-vs-mirror-2i4h)

Two GitHub Actions beginner resources aimed at helping teams reach first automation using issue-labeling workflows to teach events, jobs/steps, runners, permissions, and troubleshooting via logs. The written walkthrough reinforces least privilege (`issues: write`, `contents: read`), uses `actions/checkout@v6`, labels via `gh` using `GH_TOKEN` from `secrets.GITHUB_TOKEN`, and reminds you the target label must exist. It counterbalances last week's enterprise operations focus by teaching permission and scope habits early.

- [How to use GitHub Actions | GitHub for Beginners](https://www.youtube.com/watch?v=BQrohJ3PT7I)
- ['GitHub for Beginners: Getting started with GitHub Actions'](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-actions/)

Microsoft shipped an operations-focused Fabric update: on-premises data gateway auto-update (admin triggered) is GA. After upgrading to November 2025 (or later), admins can trigger upgrades on demand for maintenance windows/change control and script cluster member updates via PowerShell (for example, `Update-DataGatewayClusterMember ...`) to standardize rollout. Alongside Fabric bulk import/export and Git-based promotion, it reinforces that repeatable deployments also depend on gateway infrastructure staying current in controlled windows.

- [On-premises data gateway auto-update (admin triggered) (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-auto-update-admin-triggered-generally-available/)

GitHub published maintainer guidance on how AI-assisted coding is shifting open source contributions, proposing a framework around "Comprehension, Context, and Continuity," plus process levers like issue-first gates, AI-use disclosure expectations, and repo-level agent guidance (AGENTS.md) to reduce review load while keeping mentorship sustainable. It connects to last week's agent governance theme from a different angle: whether agents respond to incidents or submit code, teams need boundaries, expectations, and structured context for efficient human review.

- [Rethinking open source mentorship in the AI era](https://github.blog/open-source/maintainers/rethinking-open-source-mentorship-in-the-ai-era/)

VS Code highlighted an experimental "Agentic Browser Tools" capability that lets agent chat interact with the integrated browser (open pages, click UI, verify changes) to keep edit-run-verify loops in-editor. It continues last week's point that assistants are easier to use when they fit existing verification patterns, reducing reliance on undocumented manual checks.

- [Agentic Browser Tools (Experimental) in VS Code](https://www.youtube.com/shorts/DWh7Izwu3wQ)

## Security

This week's security story split between tightening default guardrails in developer platforms and dealing with AI-heavy systems and identity-first attacks. Building on last week's theme of trusted surfaces being tightened while also being abused, these updates land on default paths teams use every day: dependency installs, `git push`, org-wide security rollout, remote support tooling, and AI systems that act on data and tools. GitHub and Azure DevOps shipped changes affecting secrets, dependencies, and auth at scale, while Microsoft security guidance continued last week's move from AI security theory to operations: make behavior observable and governable, and defend against phishing and support-channel compromises.

### GitHub security controls: malware-aware dependencies, stricter secret policies, and smoother rollout at scale

GitHub's code security tooling shipped changes likely to affect daily workflows, especially for orgs standardizing security across many repos. After last week's focus on shifting scanning earlier and treating dev workflows as a control surface, Dependabot now supports **opt-in malware alerts for npm**, comparing your dependency graph to malware advisories in GitHub Advisory Database and producing a separate alert type from CVE vulnerability alerts. The opt-in model (with backfilled results when enabled) is meant to avoid noise that led GitHub to pause similar alerting in 2022. Teams using private registries should tune malware alert rules (ecosystem, package patterns, malicious-version vs malicious-package) to reduce name-collision false positives.

Secret Scanning **push protection** is now more governable at org scale: you can define **exemptions for roles, teams, and GitHub Apps** via security configurations at org or enterprise level. It supports last week's "stop secrets before remote" storyline: as enforcement becomes more universal, exceptions become centrally manageable. Exemptions apply at push time; exempt actors can push detected secrets without enforcement and without bypass requests, which can help for automation and break-glass flows. Those exemptions still need compensating controls (auditing, least privilege, periodic review) so they do not create blind spots.

On rollout, GitHub introduced a guided **organization-level setup** flow for GitHub Advanced Security to streamline enabling GHAS, managing custom configurations, and targeting repos, reducing inconsistent coverage. Code Quality updates also intersect with secure delivery: developers can **batch-apply multiple Code Quality suggestions** from PR "Files changed" into one commit (one follow-up scan instead of many). GitHub also tightened RBAC so the **security manager role cannot enable/disable Code Quality** unless also a repo admin, which platform teams may need to reflect in runbooks and automation. Overall, the direction matches last week: shift checks earlier, then reduce rollout and permission boundary mistakes.

- [Dependabot now detects malware in npm dependencies](https://github.blog/changelog/2026-03-17-dependabot-now-detects-malware-in-npm-dependencies)
- [Push protection exemptions for roles, teams, and apps](https://github.blog/changelog/2026-03-17-push-protection-exemptions-for-apps-teams-and-roles)
- [GitHub Advanced Security setup made simple](https://github.blog/changelog/2026-03-17-github-advanced-security-setup-made-simple)
- ['GitHub Code Quality: Batch apply quality suggestions on pull requests'](https://github.blog/changelog/2026-03-17-github-code-quality-batch-apply-quality-suggestions-on-pull-requests)
- [Code Quality permissions removed from security manager role](https://github.blog/changelog/2026-03-17-code-quality-permissions-removed-from-security-manager-role)

### Microsoft’s AI security push: observability, Zero Trust guidance, and evaluation for agent-driven detection

Microsoft's security guidance kept converging on a practical theme: if AI systems make decisions and call tools, security teams need governance controls plus end-to-end visibility into how context is assembled and actions are taken. It extends last week's day-two focus on prompt-abuse playbooks and agent governance control planes (registry, identity, policy). The missing link is telemetry that can show what an agent saw, why it acted, and which boundaries it crossed.

AI observability guidance argues that classic SRE metrics can be green while an agent violates trust boundaries (for example, indirect prompt injection via retrieved content). The recommendation is to instrument AI apps like distributed systems, with AI-specific capture: correlate by conversation or run across turns, log prompt/response plus identity and tool/data-source provenance, track AI metrics (token usage, retrieval volume, agent turns), and collect traces showing ordered execution from prompt to tool calls. It points to OpenTelemetry conventions and Microsoft options like Foundry agent tracing (preview) and the Agent 365 Observability SDK (Frontier preview).

That visibility thread connects to Microsoft's **Zero Trust for AI** guidance, extending "verify explicitly / least privilege / assume breach" across ingestion, training, deployment, and agent behavior. It is the policy side of last week's story: if agents have identities and tool/data access, control mapping must span Identity, Data, and Network, not just model settings. The Zero Trust Workshop adds an AI pillar with scenario-based control mapping, and the Zero Trust Assessment tool expands beyond Identity/Devices into Data and Network. That reflects how AI rollouts often fail on DLP/governance and network enforcement rather than model config. Microsoft also says a dedicated AI assessment pillar is planned for summer 2026.

Microsoft also released **CTI-REALM**, an open benchmark to test whether tool-using AI agents can go from CTI reports to validated detections (iterating on KQL and producing Sigma) across Linux endpoint, AKS, and Azure telemetry, with scoring across intermediate steps. For teams exploring detection-generation agents, it is positioned as a way to measure failure modes (CTI comprehension vs telemetry exploration vs query specificity) before letting generated detections into production workflows, complementing the guardrails and monitoring emphasis of the last two weeks.

- ['Observability for AI Systems: Strengthening visibility for proactive risk detection'](https://www.microsoft.com/en-us/security/blog/2026/03/18/observability-ai-systems-strengthening-visibility-proactive-risk-detection/)
- ['New tools and guidance: Announcing Zero Trust for AI'](https://www.microsoft.com/en-us/security/blog/2026/03/19/new-tools-and-guidance-announcing-zero-trust-for-ai/)
- ['CTI-REALM: A new benchmark for end-to-end detection rule generation with AI agents'](https://www.microsoft.com/en-us/security/blog/2026/03/20/cti-realm-a-new-benchmark-for-end-to-end-detection-rule-generation-with-ai-agents/)

### Identity-first intrusions and seasonal phishing: concrete defender guidance for real campaigns

Microsoft threat and incident-response reporting stayed focused on how compromises often start: social engineering and identity abuse, not zero-days. It continues last week's pattern of attackers blending into routine engineering and IT habits (interviews, "VPN download" searches, trusted hosting, signed binaries). This week again uses familiar channels (Teams support calls, remote assistance tools, and legitimate cloud infrastructure) to look normal.

Microsoft Incident Response (DART) described a Teams vishing incident: an attacker impersonated IT support, convinced a user to start a Quick Assist session, then redirected them to a spoofed login to steal credentials. The attacker then delivered payloads including a disguised MSI that sideloaded a malicious DLL, then moved via encrypted loaders, proxy connectivity, and living-off-the-land techniques. Mitigations are operational: restrict inbound Teams comms from unmanaged accounts (prefer allowlists for trusted external domains) and inventory/minimize remote assistance tools, potentially disabling Quick Assist where not required.

Microsoft Threat Intelligence also documented multiple tax-season phishing and malware campaigns (Jan-Mar 2026) using W-2/1099/IRS/CPA lures and leveraging legitimate infrastructure (OneDrive, Amazon SES click tracking) plus legitimate remote tools (ScreenConnect, SimpleHelp, Datto-related executables) for hands-on access. Chains use multi-step delivery to evade scanning (Excel -> OneNote on OneDrive -> phishing), QR-based payloads with personalized docs, and bot detection to block sandboxes. Guidance is tactical for Microsoft environments: enable Defender XDR automatic attack disruption, enforce MFA without risky exclusions and use phishing-resistant MFA via Entra Conditional Access where possible, enable ZAP and Safe Links click-time rechecks, turn on Defender for Endpoint network protection, and use the provided KQL/IOCs in Defender XDR and Sentinel (including ASIM and TI Mapping). The operational takeaway matches last week: treat normal workflows as contested and harden identity and execution paths users rely on daily.

- ['Help on the line: How a Microsoft Teams support call led to compromise'](https://www.microsoft.com/en-us/security/blog/2026/03/16/help-on-the-line-how-a-microsoft-teams-support-call-led-to-compromise/)
- ['When tax season becomes cyberattack season: beware these lures'](https://www.microsoft.com/en-us/security/blog/2026/03/19/when-tax-season-becomes-cyberattack-season-phishing-and-malware-campaigns-using-tax-related-lures/)

### Security for Microsoft Fabric and OneLake: centralized policy enforcement meets AI-era governance

Fabric's security and governance surface expanded with two related themes: enforce access consistently across engines, and reduce oversharing risk as AI features consume more data. It echoes last week's "identity moving closer to the data plane" theme: governance needs to be enforceable where data is queried, even outside Microsoft's engines.

Fabric introduced **OneLake security APIs** so third-party query engines can enforce OneLake permissions (table permissions plus RLS/CLS) at query time. The authorized engine model keeps OneLake as the source of truth while external engines retrieve security definitions and apply them during execution. OneLake also pre-computes effective access so engines do not need to reproduce role evaluation. For teams running multiple engines over Delta and Iceberg, this provides a contract for consistent governance without duplicating data or re-implementing auth per tool.

Microsoft Purview added deeper Fabric coverage aimed at reducing leakage risk as Copilots and agents become part of Fabric workflows. Purview DLP policy tips for sensitive data in Fabric Warehouse uploads are GA, with preview enforcement that can restrict access when sensitive data is detected (KQL/SQL databases and Fabric Warehouses). Insider Risk Management expands Fabric coverage to lakehouses (GA) with indicators tied to risky sharing behavior and adds policies/reporting for data theft/exfiltration. For AI inside Fabric, Purview adds preview controls to discover sensitive data in prompts/responses, identify overshared assets via DSPM assessments, and tie AI usage into Audit/eDiscovery/retention and non-compliant usage detection. Unified Catalog updates (publication workflows GA; data quality checks for ungoverned assets) bring trust signals closer to where engineers discover and use data. Combined with last week's agent governance theme, the direction is that as AI touches more data by default, enforcement and audit need to follow the data regardless of the query engine.

- [Third-party support for OneLake security](https://blog.fabric.microsoft.com/en-US/blog/third-party-support-for-onelake-security/)
- [New Microsoft Purview innovations for Fabric to safely accelerate your AI transformation](https://techcommunity.microsoft.com/blog/microsoft-security-blog/new-microsoft-purview-innovations-for-fabric-to-safely-accelerate-your-ai-transf/4502156)

### Other Security News

Azure DevOps integrations got a breaking-change warning: tokens will be further encrypted this summer, so code that decodes token payloads and treats claims as a stable contract should be replaced with supported API lookups (with caching where needed). It fits the "trusted surfaces are being tightened" trend: reduce reliance on internals and push teams toward supported identity boundaries.  
- [Authentication Tokens Are Not a Data Contract](https://devblogs.microsoft.com/devops/authentication-tokens-are-not-a-data-contract/)

A newly reported AutoMapper issue is a reminder that convenience libraries can become DoS surfaces: some mapping over cyclical or deeply self-referential graphs can recurse until StackOverflowException terminates the process, so teams should audit mappings reachable from untrusted inputs and add depth limits/ignores where needed.  
- [How AutoMapper Can Crash Your .NET Server](https://www.youtube.com/watch?v=FQdu5cyvb5k)

Azure compliance automation got attention with built-in CIS Benchmarks framed as platform-integrated baselines you can validate continuously, especially for hybrid/regulated and Linux-heavy fleets.  
- [Built‑In CIS Benchmarks on Microsoft Azure](https://www.thomasmaurer.ch/2026/03/built-in-cis-benchmarks-on-microsoft-azure/)

Microsoft's RSAC 2026 roundup collected "secure agentic AI end-to-end" updates across Entra, Purview, Defender, Sentinel, and Security Copilot, highlighting how network-layer prompt injection policies, expanded DLP for Copilot grounding, and agent governance/inventory are shaping deployment and operations in Microsoft ecosystems. It reads as a recap-and-extension of last week (registry/identity/governance) and this week (observability + Zero Trust mapping): controls are being named, shipped, and connected across identity, data governance, and monitoring.  
- ['Microsoft at RSAC Conference March 22-26: Secure agentic AI end-to-end'](https://www.microsoft.com/en-us/security/blog/2026/03/20/secure-agentic-ai-end-to-end/)
