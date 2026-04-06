---
title: "Agents as Everyday Teammates, With Practical Guardrails: Copilot, Azure, and Secure Ops"
author: "TechHub"
date: 2026-04-06 09:00:00 +00:00
tags: ["GitHub Copilot","Copilot cloud agent","Copilot CLI","Copilot SDK","Model Context Protocol (MCP)","Visual Studio","VS Code","Azure AI Foundry","Microsoft Agent Framework","Copilot Studio","Microsoft Fabric","GitHub Actions","Supply chain security","CodeQL","Azure Service Bus"]
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
external_url: "/all/roundups/Weekly-AI-and-Tech-News-Roundup-2026-04-06"
description: "This week’s roundup looks at agents showing up across Copilot, Azure, and Fabric, along with the guardrails that make them workable in real teams: planning and research flows, enterprise controls, standard orchestration, and security guidance for dependency installs, CI configuration, and admission-time enforcement."
---

This week’s roundup has a consistent theme: agents are becoming normal participants in day-to-day workflows, and platforms are adding the guardrails that make that workable at scale. Copilot’s cloud agent added branch-first work, plan-first flows, and deep research, plus enterprise controls such as runner placement, firewall policy, and verified commit signing. CLI and SDK updates also moved toward multi-agent orchestration and reusable runtimes. Across Azure and Fabric, the same pattern shows up: more standardized orchestration and stronger day-2 operations, alongside security guidance that focuses on practical risk-reduction points like dependency installs, CI configuration, and admission-time enforcement.

<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Copilot cloud agent on GitHub.com: branch-first workflows, planning, research, and tighter org controls](#copilot-cloud-agent-on-githubcom-branch-first-workflows-planning-research-and-tighter-org-controls)
  - [Copilot in Visual Studio and VS Code: custom agents, agent sessions, and more IDE-native workflows](#copilot-in-visual-studio-and-vs-code-custom-agents-agent-sessions-and-more-ide-native-workflows)
  - [Copilot CLI and Copilot SDK: multi-agent orchestration and a reusable agent runtime](#copilot-cli-and-copilot-sdk-multi-agent-orchestration-and-a-reusable-agent-runtime)
  - [MCP + Azure workflows: Copilot-assisted scaffolding, deployments, and repo-grounded research pipelines](#mcp--azure-workflows-copilot-assisted-scaffolding-deployments-and-repo-grounded-research-pipelines)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Offline, On-Device Agents with Foundry Local (RAG vs CAG)](#offline-on-device-agents-with-foundry-local-rag-vs-cag)
  - [Agent Orchestration Goes Production-Ready (Microsoft Agent Framework 1.0 + Copilot Studio Multi-Agent GA)](#agent-orchestration-goes-production-ready-microsoft-agent-framework-10--copilot-studio-multi-agent-ga)
  - [MCP as the Tooling Glue (VS Code, Azure Functions, and Governed Data/Metadata Access)](#mcp-as-the-tooling-glue-vs-code-azure-functions-and-governed-datametadata-access)
  - [Azure SRE Agent: Provider Choice, Prerequisites, and a Shift to Token-Based Billing](#azure-sre-agent-provider-choice-prerequisites-and-a-shift-to-token-based-billing)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Microsoft Fabric orchestration and operations (Airflow, scheduling, recovery)](#microsoft-fabric-orchestration-and-operations-airflow-scheduling-recovery)
- [Azure](#azure)
  - [Secure boundaries and governance for Azure services](#secure-boundaries-and-governance-for-azure-services)
  - [Compute, networking, and reliability: designing for constraints (capacity, limits, and failure modes)](#compute-networking-and-reliability-designing-for-constraints-capacity-limits-and-failure-modes)
  - [Event-driven integration patterns: from infrastructure drift to payments and durable workflows](#event-driven-integration-patterns-from-infrastructure-drift-to-payments-and-durable-workflows)
  - [Data modernization across Azure and Fabric: migration, real-time feeds, and database features](#data-modernization-across-azure-and-fabric-migration-real-time-feeds-and-database-features)
  - [Developer experience: local testing, PaaS direction, and usage observability](#developer-experience-local-testing-paas-direction-and-usage-observability)
  - [Other Azure News](#other-azure-news)
- [.NET](#net)
  - [C# 15 union types in .NET 11 Preview](#c-15-union-types-in-net-11-preview)
  - [Contextual options: per-request configuration via an experimental extensions package](#contextual-options-per-request-configuration-via-an-experimental-extensions-package)
  - [.NET MAUI’s new home for experiments: maui-labs](#net-mauis-new-home-for-experiments-maui-labs)
- [DevOps](#devops)
  - [GitHub Actions and GitHub platform updates for CI/CD and governance](#github-actions-and-github-platform-updates-for-cicd-and-governance)
  - [GitHub enterprise developer environments and supply chain automation](#github-enterprise-developer-environments-and-supply-chain-automation)
  - [Azure DevOps and Azure operations: publishing automation, work tracking UX, and cross-cloud investigations via MCP](#azure-devops-and-azure-operations-publishing-automation-work-tracking-ux-and-cross-cloud-investigations-via-mcp)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Supply chain pressure: npm dependency installs and GitHub workflow hardening](#supply-chain-pressure-npm-dependency-installs-and-github-workflow-hardening)
  - [Threat research and hunting: stealthy Linux webshells and WhatsApp-delivered Windows chains](#threat-research-and-hunting-stealthy-linux-webshells-and-whatsapp-delivered-windows-chains)
  - [GitHub security operations: secret scanning expansion and CodeQL reporting that matches real branching](#github-security-operations-secret-scanning-expansion-and-codeql-reporting-that-matches-real-branching)
  - [Security governance in Azure and Microsoft Fabric: admission control, labeling, encryption, and recovery](#security-governance-in-azure-and-microsoft-fabric-admission-control-labeling-encryption-and-recovery)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week’s Copilot updates kept moving past the "chat + autocomplete" baseline toward agents that work across the web, IDE, CLI, and mobile, with more governance and observability as usage scales. Building on last week’s shift toward agent work inside PRs/Issues/Projects and better operability (logs, validations, admin controls, reporting), this week extends that direction in two ways: more entry points for agent work (branch-first, mobile/Slack) and tighter enterprise guardrails (runner and firewall controls, signed commits, org-wide instructions). Model availability is also changing quickly, so teams that pin models or enforce policies should plan regular housekeeping to avoid surprises.

### Copilot cloud agent on GitHub.com: branch-first workflows, planning, research, and tighter org controls

After last week’s PR-native improvements (comment `@copilot` on an existing PR, update the current PR by default, and resolve merge conflicts with logs and validations), Copilot cloud agent (formerly the Copilot coding agent) can now work directly on a branch without automatically opening a PR. Teams can iterate privately, review the evolving diff, and open a PR only when they are ready, while still requesting auto-created PRs when that is preferred. This pairs last week’s "iterate within the same PR thread" flow with a pre-PR staging mode.

The agent also added two changes aimed at reducing unexpected diffs. First, it can draft an implementation plan before coding so you can approve or adjust the approach up front. Second, "deep research" sessions let it answer broader repo-grounded questions, which can help with impact analysis and design trade-offs, especially when combined with last week’s expanded agent visibility in Issues/Projects.

As more teams adopt it, GitHub added org-level controls for where the cloud agent runs and what it can reach. Platform teams can set (and optionally lock) which Actions runner executes agent tasks (GitHub-hosted, Large, or self-hosted) without managing runner choice repo-by-repo via `copilot-setup-steps.yml`. Org admins can also centrally manage the agent firewall (on/off, recommended allowlist, custom allowlist, and whether repos can add entries), which affects workflows that fetch dependencies, call external services, or reach internal registries. Together, these continue last week’s "automatable rollout" direction at two early control points: network egress and compute placement.

Supply-chain and compliance workflows got easier as well: the cloud agent now signs every commit it creates. Agent commits show as "Verified" and work in repos that enforce "Require signed commits" via branch protection/rulesets. Combined with last week’s traceability (session logs, issue sidebar status, PR-thread invocation), commit provenance becomes part of the default setup as agents become regular commit authors.

- [Research, plan, and code with Copilot cloud agent](https://github.blog/changelog/2026-04-01-research-plan-and-code-with-copilot-cloud-agent)
- [Put GitHub Copilot cloud agent to work: research, plan, and code on github.com](https://www.youtube.com/watch?v=pn5x1CamKVY)
- [Organization runner controls for Copilot cloud agent](https://github.blog/changelog/2026-04-03-organization-runner-controls-for-copilot-cloud-agent)
- [Organization firewall settings for Copilot cloud agent](https://github.blog/changelog/2026-04-03-organization-firewall-settings-for-copilot-cloud-agent)
- [Copilot cloud agent signs its commits](https://github.blog/changelog/2026-04-03-copilot-cloud-agent-signs-its-commits)

### Copilot in Visual Studio and VS Code: custom agents, agent sessions, and more IDE-native workflows

In Visual Studio, Copilot’s March update focused on making agent behavior portable and repeatable across repos, which mirrors last week’s repo-visible instructions/skills and this week’s GitHub.com plan/research work. Teams can define custom agents as `*.agent.md` files under `*.github/agents/*` and expose them in Visual Studio’s agent picker, with settings such as workspace awareness, tool access, preferred model, and MCP connections. Visual Studio also added "agent skills" as reusable instruction sets stored in the repo (shared defaults) or the user profile (personal defaults), which are auto-discovered during workflows. This fits the "configuration scales" theme and helps keep IDE and GitHub.com agents aligned per repo.

Agent mode also gained a navigation tool that relies on language services rather than text search: `find_symbol` locates and reasons about symbols across the project. It supports C++, C#, Razor, TypeScript, and LSP-backed languages, which helps refactors avoid missed call sites or scope/type mistakes, especially as last week’s PR/issue workflows encourage larger agent changes.

The update also brought Copilot into IDE performance and security loops. "Profile with Copilot" in Test Explorer runs a specific test via a Profiling Agent and analyzes CPU/instrumentation data (called out for .NET tests). During debugging, PerfTips now use live profiling, and a Profiler Agent captures elapsed time/CPU/memory signals so Copilot can suggest optimizations when you hit a slowdown. For dependency hygiene, Visual Studio can remediate vulnerable NuGet packages from Solution Explorer via "Fix with GitHub Copilot," which turns detection into an in-IDE update loop. This continues last week’s "give the agent real signals" thread, but shifts it from external telemetry to built-in diagnostics and security findings.

In VS Code, Copilot kept becoming more "session-aware," matching last week’s traceable sessions/logs (issue sidebar status, PR-linked logs, CLI `/context` and `/resume`). VS Code 1.114 highlights centered on Copilot Chat usability, including richer media in the chat carousel, copy as Markdown, improved troubleshooting, and updates to the `#codebase` grounding command. VS Code 1.115 Insiders added more session state (restore agent edits with diffs and undo/redo), exposed entitlements/usage inside Sessions, and expanded session context to include the integrated browser (tracking tabs the agent used). Terminal automation also improved: background terminals can notify the agent on completion with exit codes/output, input prompts are surfaced to avoid silent stalls, and `send_to_terminal` supports confirmed command dispatch to background terminals. Remote workflows improved with an SSH path that installs the VS Code CLI and starts agent host mode on remote machines. Overall, last week made agents more present in GitHub collaboration surfaces, and this week makes editor/terminal work easier to resume, audit, and drive with more explicit control.

- [GitHub Copilot in Visual Studio — March update](https://github.blog/changelog/2026-04-02-github-copilot-in-visual-studio-march-update)
- [Visual Studio March Update – Build Your Own Custom Agents](https://devblogs.microsoft.com/visualstudio/visual-studio-march-update-build-your-own-custom-agents/)
- [What's hot in VS Code v1.114? 🔥](https://www.youtube.com/shorts/LrQBmrRV_Ro)
- [Visual Studio Code 1.115 (Insiders): agent sessions, Copilot entitlements, SSH agent host mode](https://code.visualstudio.com/updates/v1_115)

### Copilot CLI and Copilot SDK: multi-agent orchestration and a reusable agent runtime

Copilot CLI added multi-agent execution via the `/fleet` command. `/fleet` breaks a goal into work items, runs sub-agents in parallel, then validates and synthesizes results into your working tree. In real repos, the details matter: each sub-agent has its own context window, they share the filesystem, and they do not coordinate directly, so prompts should specify file ownership to avoid collisions (or stage outputs in temp paths and merge). It builds on last week’s CLI focus on session controls (`/model`, `/context`, `/resume`) by adding an orchestration pattern that does not depend on one long context thread.

GitHub also released the Copilot SDK in public preview (the same agent runtime used by Copilot cloud agent and Copilot CLI), so teams can embed agent interactions in internal apps without building orchestration from scratch. This extends last week’s Copilot SDK "IssueCrush" walkthrough into a shared runtime rather than a one-off pattern. The SDK includes tool invocation, stateful multi-turn sessions, streaming, built-in file ops, and a permissions/approval framework (including read-only tools that can bypass approvals). It supports blob attachments (images/screenshots without temp files) and OpenTelemetry tracing with W3C Trace Context propagation, extending last week’s "traceable logs" theme into standard telemetry pipelines. BYOK (OpenAI, Azure AI Foundry, Anthropic) keeps model/provider flexible while standardizing the runtime. The preview ships for Node/TypeScript, Python, Go, .NET, and Java.

GitHub also published an SDK demo that adds planning flows to a Node.js app (meal plans and weekend schedules), with an emphasis on regeneration as constraints change. The takeaway is how to wire an app to SDK sessions and support iterative refinement without restarting, which lines up with this week’s "plan first" in Copilot cloud agent and last week’s repeatable session lifecycles.

- [Run multiple agents at once with /fleet in Copilot CLI](https://github.blog/ai-and-ml/github-copilot/run-multiple-agents-at-once-with-fleet-in-copilot-cli/)
- [Copilot SDK in public preview](https://github.blog/changelog/2026-04-02-copilot-sdk-in-public-preview)
- [Build a Planning App with the GitHub Copilot SDK | demo](https://www.youtube.com/watch?v=ylPjZVaLJYI)
- [Not sure where to start with the GitHub Copilot SDK?](https://www.youtube.com/shorts/R5ouLL_UA3o)

### MCP + Azure workflows: Copilot-assisted scaffolding, deployments, and repo-grounded research pipelines

Copilot’s "agent + tools" story showed up across Azure workflows, extending last week’s MCP thread: connect Copilot to tools, then operationalize access with versioned config, enforcement, and audit. Azure Developer CLI (azd) shipped a preview "Set up with GitHub Copilot" path in `azd init`, using a Copilot agent to scaffold a project and align it to azd conventions (templates, `azure.yaml`, service detection), with guardrails like dirty-directory checks and consent before enabling MCP server tooling. The same release added AI-assisted troubleshooting for failed commands (explain/guidance/troubleshoot/skip), with an option to apply a fix and retry from the terminal. azd’s extension SDK also added MCP utilities and a new `CopilotService` gRPC service so extensions can use agent capabilities (sessions/messages/usage metrics), which matches last week’s "turn playbooks into tool-driven flows" pattern.

An "Azure Skills Plugin" cookbook also published many copy/paste prompts for Copilot Chat (Agent mode) and Copilot CLI to automate Azure tasks end-to-end: repo analysis and infra generation (`azure-prepare`), validation (`azure-validate`), deployment via azd (`azure-deploy`), plus diagnostics via KQL, RBAC/compliance checks, Entra app registrations, and AI service setup (Azure AI Search, API Management as an AI gateway, Foundry tasks). The theme is chaining skills into prepare -> validate -> deploy pipelines while keeping explicit per-skill calls for tighter control, similar to last week’s "standardized prompt files in CI," expressed here as skill chains.

Project Nighthawk provided a concrete pattern for repo-grounded research in VS Code: a multi-agent pipeline that searches locally cloned repos (kept current via `git pull`), consults Microsoft Learn via an MCP server, synthesizes a cited Markdown report, and runs a fact-checker that validates claims and flags unverified statements. For deep technical investigations, this extends last week’s "observability + verification" mindset into reviewable research artifacts, especially now that this week adds "deep research" sessions to Copilot cloud agent.

- [Azure Developer CLI (azd) – March 2026: Run and Debug AI Agents Locally, GitHub Copilot Integration, & Container App Jobs](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-march-2026/)
- [Building with Azure Skills: a prompt cookbook for the Azure plugin (MCP server)](https://devblogs.microsoft.com/all-things-azure/building-with-azure-skills/)
- [Project Nighthawk: A Research Agent Built for Field Engineering](https://devblogs.microsoft.com/all-things-azure/project-nighthawk-a-research-agent-built-for-field-engineering/)

### Other GitHub Copilot News

Org-level governance and reporting kept closing admin gaps, building on last week’s repo allowlisting and usage metrics flagging `used_copilot_coding_agent`. Organization custom instructions are now GA for Copilot Business and Enterprise, so admins can set defaults across Copilot Chat on github.com, Copilot code review, and Copilot cloud agent. Usage reporting also expanded: org reports now include per-user Copilot CLI activity (session/request counts, tokens, average tokens per request, last known CLI version) via the Copilot usage metrics REST API (`apiVersion=2026-03-10`). Together with last week’s agent-user metric, this helps distinguish "used Copilot" from "ran agents" from "drove terminal automation," as entry points expand beyond IDEs.

- [Copilot organization custom instructions are generally available](https://github.blog/changelog/2026-04-02-copilot-organization-custom-instructions-are-generally-available)
- [Copilot usage metrics now includes per-user GitHub Copilot CLI activity in organization reports](https://github.blog/changelog/2026-04-02-copilot-usage-metrics-now-includes-per-user-github-copilot-cli-activity-in-organization-reports)

Model availability shifted again, with two deprecations that matter for teams pinning models or enforcing policies, which continues last week’s reminder that model lifecycles change quickly (for example, Gemini 3 Pro exiting for Gemini 3.1 Pro). GPT-5.1 Codex variants (GPT-5.1-Codex, -Mini, -Max) were deprecated April 1, 2026, with GPT-5.3-Codex as the replacement. Claude Sonnet 4 is scheduled for deprecation on 2026-05-01 with Claude Sonnet 4.6 suggested. On the adoption side, GPT-5.4 mini is now GA for Copilot Student via auto model selection in Copilot Chat across major IDEs.

- [GPT-5.1 Codex, GPT-5.1-Codex-Max, and GPT-5.1-Codex-Mini deprecated](https://github.blog/changelog/2026-04-03-gpt-5-1-codex-gpt-5-1-codex-max-and-gpt-5-1-codex-mini-deprecated)
- [Upcoming deprecation of Claude Sonnet 4 in GitHub Copilot](https://github.blog/changelog/2026-03-31-upcoming-deprecation-of-claude-sonnet-4-in-github-copilot)
- [GPT-5.4 mini is now available in Copilot Student auto model selection](https://github.blog/changelog/2026-04-01-gpt-5-4-mini-is-now-available-in-copilot-student-auto-model-selection)

Copilot’s "where work starts" expanded beyond IDEs into Slack and mobile, extending last week’s "agents show up where teams collaborate" shift into chat and on-the-go triage. The GitHub app for Slack can now create Issues from natural-language prompts (including sub-issues), supports thread-based refinement of metadata, and offers an in-Slack flex pane to view the issue. GitHub Mobile also made sessions and delegation easier: a dedicated Copilot tab (notably on Android), native session logs, stop sessions and create PRs from completed sessions, plus a faster "Assign an Agent" flow from an issue (custom instructions and optional repo selection). The pattern across two weeks is consistent: as agents act in more places, GitHub adds workflow hygiene (logs, status visibility, and mobile-native controls) so activity stays reviewable.

- [Create issues from Slack with Copilot](https://github.blog/changelog/2026-03-30-create-issues-from-slack-with-copilot)
- [GitHub Mobile: Stay in flow with a refreshed Copilot tab and native session logs](https://github.blog/changelog/2026-04-01-github-mobile-stay-in-flow-with-a-refreshed-copilot-tab-and-native-session-logs)
- [GitHub Mobile: Faster, more flexible agent assignment from issues](https://github.blog/changelog/2026-04-01-github-mobile-faster-more-flexible-agent-assignment-from-issues)

Several items focused on practical guidance for shaping Copilot behavior, from agent-first repo practices to prompt patterns and repo-level instruction files. This mirrors last week’s dotnet/runtime lessons ("prepare the repo, document commands/conventions, pair with humans") and this week’s move toward org-wide defaults (custom instructions GA). GitHub’s Applied Science team described an agent-driven loop around Copilot CLI (planning, autopilot-style execution, iterative Copilot Code Review, then human review), with an emphasis on repo navigability (structure, naming, docs, tests, dead-code removal) and CI guardrails (typing, linters, layered tests) so agents can self-correct. Other posts covered prompting Copilot to ask clarifying questions before implementing, and adding a repo "tone of voice"/instructions file to make Copilot review more strict.

- [Agent-driven development in Copilot Applied Science](https://github.blog/ai-and-ml/github-copilot/agent-driven-development-in-copilot-applied-science/)
- [Let GitHub Copilot Ask First](https://www.cooknwithcopilot.com/blog/let-github-copilot-ask-first.html)
- [GitHub Copilot Is Too Nice. Fix It With a Tone of Voice File.](https://dev.to/playfulprogramming/github-copilot-is-too-nice-fix-it-with-a-tone-of-voice-file-39ij)

Copilot also showed up in modernization and security enablement content. Videos covered using Copilot to assess legacy .NET apps, draft modernization plans, and convert them into task lists (including Copilot Modernization in VS Code). A GitHub Security intro positioned Copilot Autofix alongside Dependabot, secret scanning, and code scanning to shorten the initial remediation loop. This follows last week’s thread of turning assistant output into workflow steps, but framed around modernization and remediation rather than feature work.

- [How can AI help me modernize my app?](https://www.youtube.com/watch?v=DgIweLo3-Oo)
- [Using GitHub Copilot in VS Code to plan a .NET Framework to modern .NET migration](https://www.youtube.com/shorts/ppHWnhw2BUM)
- [VS Code Live: Modernizing legacy .NET apps with GitHub Copilot Modernization in VS Code](https://www.youtube.com/watch?v=JQ3x362nc6c)
- [Getting started with GitHub security (secret scanning, Dependabot, code scanning, Copilot Autofix)](https://www.youtube.com/watch?v=zhxXaFzzJYA)

Infrastructure-as-code teams got more "Copilot + guardrails" examples in VS Code, continuing last week’s "agentic platform engineering" message: version playbooks, use deterministic checks, keep humans in approvals. One walkthrough showed a repo-scoped VS Code custom agent (1.99+) for Terraform security scanning with structured findings (severity + stable IDs, file/line, remediation, mappings to CIS/ASB v3/NIST), paired with TFLint and SARIF output. Another tutorial described a VS Code extension that scaffolds Terraform from centrally managed module templates and uses Copilot to draft configuration, while keeping deterministic validation/compliance enforcement in extension code (explicitly avoiding MCP) so AI output stays draftable but constrained.

- [VS Code Custom Agents: AI-Powered Terraform Security Scanning in the IDE](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/vs-code-custom-agents-ai-powered-terraform-security-scanning-in/ba-p/4507903)
- [Building a VS Code extension to scaffold Terraform with guardrails and GitHub Copilot](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/vs-code-extension/ba-p/4500803)

A governance-focused analysis flagged an upcoming Copilot data policy change, following last week’s official policy update and guardrails guidance. Copilot Free/Pro/Pro+ will allow training on "interaction data" unless users opt out, while Copilot Business and Enterprise are not affected. It reviews enforcement options (central licensing, network controls, IDE policy packs, managed identities/data residency) for orgs that want to avoid relying on opt-out behavior and keep usage on commercial tiers.

- [GitHub Copilot will start training on your interactions](https://jessehouwing.net/github-copilot-will-start-training-on-your-interactions/)

A few community and onboarding items fit the two-week trend of agents as normal workflow participants. These include quick-start content for installing Copilot CLI, a beginner workflow using CLI "plan" and "delegate" to hand work to a background cloud agent then review PRs from the terminal, and an event announcement for Copilot Dev Days.

- [How to install GitHub Copilot CLI in seconds](https://www.youtube.com/shorts/9-zk6RNIS6w)
- [Copilot CLI for beginners: Plan, delegate, and review](https://www.youtube.com/watch?v=v8dr7QcIiLU)
- [GitHub Copilot Dev Days are Here! We're in Chennai on 4/11](https://www.youtube.com/shorts/Pl7wYzQzQ-w)

## AI

This week’s AI updates were less about new model behavior and more about making agent systems workable: running locally, standardizing orchestration across languages, and tightening operational controls (tools, governance, cost) so systems hold up in production. It continues last week’s "run it like software" direction (repeatable workflows, inspectable grounding, and day-two controls), with more emphasis on building blocks you can ship: offline templates, stable multi-agent runtimes, and governable tool-integration patterns.

### Offline, On-Device Agents with Foundry Local (RAG vs CAG)

Two local-first assistant blueprints built around Microsoft Foundry Local and `foundry-local-sdk` show how to run entirely on one machine with no API keys and no network after the initial model download. This builds on last week’s Foundry Local thread (OpenAI-compatible local endpoints, stable client code while endpoints swap, lightweight grounding) by showing two concrete app shapes that fit internal tools or offline/field use.

Both samples keep the app intentionally small: Node.js 20+ with Express, a single-page UI, and Server-Sent Events (SSE) used twice. First, SSE streams model download/load status until "Offline Ready." Then it streams tokens into chat. That "operate the loop" approach (status streaming, predictable startup, explicit offline readiness) lines up with last week’s idea that local runtimes should be operable systems, not just demos.

They differ mainly in how grounding works. The CAG version is startup-loaded and straightforward: preload Markdown docs from `docs/`, score documents with keyword scoring (no embeddings/chunking/vector DB), and inject the top docs into the prompt. The trade-offs are explicit: it is limited by the context window, best for "tens of documents," and KB updates require a restart. It also includes practical model selection: filter the local catalog by capability (chat-completion) and a RAM budget policy (for example, "60% of system RAM"), then pick models like `phi-4` on 32 GB or `phi-3.5-mini` on 8 GB, download if needed, load, and run completions in-process. This keeps last week’s "predictable endpoint swaps" idea but adds guidance for "what runs on this machine."

The RAG version adds more components for scale and hot updates, which echoes last week’s point that grounding should be reusable and testable. It chunks Markdown into ~200-token segments with overlap, stores chunks and TF-IDF vectors in a single-file SQLite DB (`better-sqlite3`), and retrieves using TF-IDF + cosine similarity, explicitly avoiding embeddings to stay offline and lightweight. Retrieval is optimized with an inverted index, prepared statements, and caching, and the author reports sub-millisecond retrieval for the target workload. The prompt contract is also strict: safety-first behavior, bans on guessing for procedures/tolerances, a required "This information is not available in the local knowledge base" response when grounding is insufficient, and a structured output format (summary, safety warnings, steps, references) with UI-visible citations and relevance scores. It also supports runtime doc upload (`.md`/`.txt`) with immediate chunk/vector/index updates without restart, which is where the extra RAG complexity pays off.

Both posts include setup details (`winget install Microsoft.FoundryLocal`, model sizes like ~2 GB for Phi-3.5 Mini, `npm test` via Node’s built-in test runner) and close with extension paths such as hybrid retrieval (TF-IDF + embeddings), persisted memory, multimodal input, and PWA packaging for offline install, which matches last week’s "start simple, stay inspectable" direction.

- [Building Your First Local RAG Application with Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-your-first-local-rag-application-with-foundry-local/ba-p/4501968)
- [Build a Fully Offline AI App with Foundry Local and CAG](https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-a-fully-offline-ai-app-with-foundry-local-and-cag/ba-p/4502124)

### Agent Orchestration Goes Production-Ready (Microsoft Agent Framework 1.0 + Copilot Studio Multi-Agent GA)

Microsoft moved multi-agent development toward more stable foundations in two places: Agent Framework 1.0 for developers and Copilot Studio multi-agent GA for makers/developers. This follows last week’s platform-choice framing (Copilot Studio vs Azure AI Agents vs Foundry) by translating "production-ready" into stable APIs across languages, reviewable configs, and evaluation/moderation hooks that fit CI/CD.

Microsoft Agent Framework 1.0 is out for .NET and Python with stable APIs and an LTS/backward-compatibility commitment, positioned as a convergence of Semantic Kernel foundations and AutoGen orchestration patterns. The core value is standardization: build single- or multi-agent systems with the same abstractions in both runtimes, and swap providers via connectors (Foundry, Azure OpenAI, OpenAI, Anthropic, Bedrock, Gemini, Ollama). That matches last week’s theme of keeping app/orchestration contracts stable while endpoints evolve (similar to Foundry Local’s endpoint swap story). It includes core building blocks teams need early: tools/functions, multi-turn session management, and streaming. For orchestration, it provides a graph workflow engine (branch/fan-out/converge), checkpointing/hydration for long-running flows, and patterns like sequential, concurrent, handoff, group chat, and Magentic-One, plus middleware hooks for policy, observability, and compliance logic. Memory is pluggable (history, persistent KV, vector retrieval) with backends like Foundry Agent Service memory, Mem0, Redis, Neo4j, and custom providers. It also introduces YAML-defined agents/workflows (instructions, tools, memory, topology) that can be version-controlled and promoted, which lines up with last week’s repo-first operating model.

Copilot Studio’s multi-agent orchestration is rolling into GA over the next few weeks (targeting full availability for eligible customers by April 2026). It extends last week’s "hybrid approach" framing (Copilot Studio for controlled experiences, programmable layers behind it) into multi-agent coordination. The GA scope emphasizes connected experiences: Fabric integration (Copilot Studio agents coordinate with Fabric agents), orchestration with the Microsoft 365 Agents SDK (reuse retrieval/actions across Microsoft 365 and Copilot Studio), and Agent-to-Agent (A2A) communication via an open protocol for delegating to other agents. Prompt Builder is now GA and integrated into the Tools tab for iterating instructions/models/inputs/knowledge in one place, and prompt-level content moderation controls are GA (supported regions) for managed models, which can help where default filters block legitimate regulated terms. Evaluation automation APIs are GA via Power Platform APIs/connectors for CI/CD gating against regression scores, and connectors like ServiceNow and Azure DevOps are called out as improved to better support operational grounding.

The shared direction is multi-agent work as engineering: stable runtimes (.NET/Python), checkpointed workflow graphs, versioned YAML orchestration, and platform features that make prompt iteration, moderation, and automated evaluation part of regular releases.

- [Microsoft Agent Framework Version 1.0](https://devblogs.microsoft.com/agent-framework/microsoft-agent-framework-version-1-0/)
- [What’s new in Copilot Studio: Updates to multi-agent systems](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/new-and-improved-multi-agent-orchestration-connected-experiences-and-faster-prompt-iteration/)

### MCP as the Tooling Glue (VS Code, Azure Functions, and Governed Data/Metadata Access)

MCP kept showing up as the tool layer that makes agents reusable across products: custom tools in Foundry agents, local development in VS Code, and governed metadata access for data copilots. This continues last week’s MCP storyline (maturity, hosted endpoints, identity-aware access, deterministic tool surfaces), with more emphasis on hosting, auth, and integration with governed systems.

In Azure AI Foundry, the practical pattern is to host an MCP server remotely on Azure Functions, then register that endpoint as a tool in Foundry so agents can discover/invoke it from the Agent Builder Playground. Azure Functions is positioned as the default host because it fits tool workloads (serverless scaling, consumption billing, multiple auth models). The post lays out identity choices teams need to decide early, following last week’s "tool calls need boundaries" theme: key-based auth (simple for dev), Entra ID + managed identity (recommended for production service-to-service calls using the Foundry project managed identity), OAuth identity passthrough (tool calls under each end user identity), and unauthenticated access (dev/public tools only). It also gives a concrete endpoint format for MCP extension-based Functions: `https://<FUNCTION_APP_NAME>.azurewebsites.net/runtime/webhooks/mcp`. The reuse point is explicit: MCP servers built for VS Code/Visual Studio/Cursor can be reused in Foundry without rebuilding integrations.

For local development, a VS Code video walkthrough shows end-to-end MCP server development with Python and FastMCP, including client/server responsibilities, tool discovery and invocation, and STDIO transport (server as a local process over stdin/stdout). This reinforces the schema discipline point: MCP tool schemas enable cross-client discovery, and transport can be local for dev even if production moves to remote HTTPS for governance and networking.

MCP also appears in the Fabric/Purview governance story as a way to expose metadata and governance-aware capabilities to AI agents without bypassing permissions. This aligns with last week’s Fabric direction (semantics, permission-aware context, MCP endpoints on the roadmap): instead of copying catalog/lineage/classification into prompts, you expose controlled tools that enforce Fabric/Purview rules. It is paired with API governance updates (OneLake Catalog Search API GA and Bulk Import/Export of Item Definitions preview) so teams can automate metadata operations instead of relying on UI-heavy workflows.

- [Give your Foundry Agent Custom Tools with MCP Servers on Azure Functions](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/give-your-foundry-agent-custom-tools-with-mcp-servers-on-azure/ba-p/4507828)
- [Create and install an F1 inspired MCP Server in VS Code](https://www.youtube.com/watch?v=ZPaF_6mSp8I)
- [Modern data governance in Fabric: How Purview and AI transform data governance](https://zure.com/blog/whats-new-in-microsoft-fabric-for-data-governance-and-metadata-management-march-2026)

### Azure SRE Agent: Provider Choice, Prerequisites, and a Shift to Token-Based Billing

Azure SRE Agent added operational details that shape safe, sustainable on-call usage: prerequisites, integrations, model provider choice, and billing. This builds on last week’s "external system -> managed identity bridge -> SRE Agent trigger" patterns and cost guardrails by adding rollout constraints, network realities, and cost units that map directly to usage.

One post focuses on preview onboarding prerequisites and infrastructure scenarios, framing the agent as an AI reliability operator that observes Azure telemetry (Azure Monitor, Log Analytics, Application Insights) and Azure service APIs, then helps with incident investigation, correlation, RCA, and optional controlled remediation. Teams can run in recommendation/review mode or enable autonomous execution for pre-approved steps with guardrails, approvals, and specialized subagents (VMs/databases/networking). The actionable content is the checklist: the preview control plane must be created in Sweden Central, Australia East, or US East 2 (monitored workloads can be elsewhere), subscriptions may need allow-listing, and identity/RBAC is the core dependency, often elevated for onboarding and then tightened to least privilege for the managed identity (read for investigation, scoped write for approved remediation). It also calls out integration edges: outbound HTTPS to Azure management endpoints and any third-party systems/MCP servers (custom MCP endpoints must be remote HTTPS, not local endpoints), no guaranteed static egress IPs for firewall allow lists, and allowing domains like `*.azuresre.ai`. Integrations include ServiceNow/PagerDuty, GitHub/Azure DevOps, Grafana, and Azure Data Explorer (Kusto). The "remote-only tool endpoints + identity boundaries" constraint matches the MCP hosting patterns showing up elsewhere.

Two updates moved the product toward more flexible operations and clearer cost planning. First, SRE Agent now supports multiple model providers, adding Anthropic with Claude Opus 4.6 as the baseline when selected, which fits this week’s provider-abstraction theme (Agent Framework). Second, active flow billing shifts from time-based to token-based metering effective April 15, 2026. The unit remains Azure Agent Units (AAUs): always-on flow stays 4 AAUs per agent-hour; active flow becomes "AAUs per million tokens" with rates varying by provider. This ties cost to investigation depth (conversation length, correlated telemetry breadth) and makes provider choice part of cost planning. Monthly AAU allocation limits (Settings -> Agent consumption) remain the key guardrail: when you hit the active flow limit, chat/autonomous actions pause until next month, while always-on continues, which matches last week’s cost-control approach.

- [From Toil to Trust: Azure SRE Agent prerequisites, integrations, and infra scenarios](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/from-toil-to-trust-how-azure-sre-agent-is-redefining-cloud/ba-p/4505875)
- [Azure SRE Agent now supports multiple model providers, including Anthropic Claude](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-now-supports-multiple-model-providers-including/ba-p/4508111)
- [An update to the active flow billing model for Azure SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/an-update-to-the-active-flow-billing-model-for-azure-sre-agent/ba-p/4507866)

### Other AI News

Foundry’s model catalog keeps expanding beyond chat into modality-specific building blocks. This follows last week’s "voice as an operational modality" thread by adding first-party primitives in Foundry so teams can build voice and image features without immediately using third-party hosting.

Microsoft announced MAI models in Azure AI Foundry: MAI-Transcribe-1 (speech-to-text, 25 languages), MAI-Voice-1 (text-to-speech), and MAI-Image-2 (image generation). The goal is first-party options in Foundry’s catalog, with details like parameters/pricing/regions expected in the linked build surfaces rather than the announcement.

- [We’re bringing our growing MAI model family to every developer in Foundry, including: MAI-Transcribe-1, most accurate transcription model in world across 25 languages; MAI-Voice-1, natural, expressive speech generation; MAI-Image-2, our most capable image model yet. Start building…](https://www.linkedin.com/posts/satyanadella_were-bringing-our-growing-mai-model-family-activity-7445475747680411650-T_tO)

Teams working on "custom model in production" got a concrete BYOM walkthrough using Azure Machine Learning as the hosting and governance boundary. It applies the same "treat it like software" posture (reproducible environments, managed endpoints, identity-first access) to teams running their own models instead of using a hosted catalog.

It covers registering a model (example: SmolLM-135M), defining a reproducible conda environment (Python 3.12, `azureml-inference-server-http`, PyTorch/Transformers), deploying to Managed Online Endpoints, and using Entra ID auth via `DefaultAzureCredential`/`MLClient` to avoid secrets. It also notes an alternative scoring shape for token-rank analysis when you need introspection rather than generation.

- [Bring Your Own Model (BYOM) for Azure AI Applications using Azure Machine Learning](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/bring-your-own-model-byom-for-azure-ai-applications-using-azure/ba-p/4508211)

Database-centric AI patterns also got attention, with Azure SQL Managed Instance framed as an AI-enabled PaaS platform. This complements last week’s "governed grounding" and "data readiness" themes with an operational stance: keep retrieval and scoring close to the data boundary.

It highlights native vector types and distance functions for semantic search/RAG, in-database Python/R via Machine Learning Services for training/scoring near governed data, and Copilot-assisted diagnostics via Query Store/DMVs (plus Copilot in SSMS when connected to MI). For teams that want AI to stay inside operational data boundaries, it is a clear "do more where the data lives" path.

- [Azure SQL Managed Instance as an AI-Enabled PaaS Platform](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/azure-sql-managed-instance-as-an-ai-enabled-paas-platform/ba-p/4508380)

Two different takes on AI costs extended last week’s cost-guardrails storyline into broader engineering practice. One focuses on tactics such as token mechanics, caching, and routing, while the other covers the full cost of agentic systems, including human review and governance overhead once agents become part of delivery and operations.

- [Cost Optimization for Copilot and AI Agents on Azure](https://dellenny.com/cost-optimization-for-copilot-and-ai-agents-on-azure/)
- [On .NET Live - AI offers benefits, but at what cost?](https://www.youtube.com/watch?v=yO_TH3R8KMw)

Fabric’s "AI depends on trusted data" story continued with two governance-adjacent items. This matches last week’s Fabric focus on reusable business context (events, ontology, graph reasoning, de-identification) by showing more upstream pipeline work: master data curation and governance/metadata APIs that copilots rely on for permission-aware operation.

- [How Stibo Systems’ MDM powers trusted data for analytics and AI in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/how-stibo-systems-mdm-powers-trusted-data-for-analytics-and-ai-in-microsoft-fabric-preview/)
- [Modern data governance in Fabric: How Purview and AI transform data governance](https://zure.com/blog/whats-new-in-microsoft-fabric-for-data-governance-and-metadata-management-march-2026)

For reference architectures, the March 2026 Innovation Challenge recap points to hackathon projects that mirror production needs: NLQ-to-analytics flows, multi-agent analytics orchestration (including DuckDB pipelines and Microsoft 365 ingestion), and validation layers that enforce structure and block unsafe actions. That validation focus matches last week’s local-agent patterns and this week’s offline RAG prompt contracts: different stacks, similar engineering priorities.

- [The March 2026 Innovation Challenge Winners](https://techcommunity.microsoft.com/t5/azure/the-march-2026-innovation-challenge-winners/m-p/4508498#M22477)

## ML

This week’s Fabric updates focused on production gaps for data and ML-adjacent workloads: more standard orchestration (especially for Airflow teams) and more day-2 guardrails via alerting and recovery to reduce downtime from failures or deletes. This continues last week’s "managed operating surfaces" thread, where dbt Jobs, Activator-triggered actions, and improved diagnostics emphasized repeatable, observable workflows.

### Microsoft Fabric orchestration and operations (Airflow, scheduling, recovery)

Fabric Data Factory’s Apache Airflow integration added native operators to run Fabric artifacts directly from Airflow DAGs. Teams can invoke Fabric Notebooks, Spark job definitions, Fabric pipelines, Semantic Models, and user data functions as first-class tasks, with broader coverage including Copy jobs and dbt jobs. This builds on last week’s emphasis on dbt Jobs as a scheduling/observability plane and Copy job improvements for incremental/CDC ingestion, but it now lets existing Airflow standards orchestrate those Fabric primitives without custom glue. It also complements last week’s Activator direction (event -> action inside Fabric) by giving teams another coordination surface when a DAG view is preferred.

Fabric also added a shortcut, "Run Fabric Artifact" in the Airflow job context menu, that inserts the needed code/config to call a Fabric item. This speeds DAG authoring and reduces boilerplate, which matches the recent push to minimize bespoke integration code.

New Apache Airflow job APIs also support platform automation: programmatic management/monitoring/triggering of DAG runs from external services, including event-driven scenarios. This fits teams integrating Fabric orchestration with CI/CD, internal portals, or runbooks, and matches last week’s API-first posture (dbt Jobs APIs, workspace tags via REST, Notebook Public APIs referenced previously). The direction is increasingly "everything is addressable as an API," which supports consistent promotion, scheduling, and monitoring across many workspaces.

Operationally, Fabric improved both "find out fast" and "recover fast." Scheduled job failure notifications are now GA: configure recipients once per item under Schedule settings, and the list applies to all schedules for that item. Failed scheduled runs email error details plus a link to the Monitoring Hub run, and it works for schedules created in the UI or via the Job Scheduler REST API. The limitation is explicit: only scheduled runs trigger emails, not manual runs, so ad-hoc execution still needs separate practices. This extends last week’s day-2 manageability theme by making managed schedules more actionable without constant dashboard watching.

Fabric Data Warehouse also added preview recovery for a dropped warehouse via the workspace Recycle Bin. Within a tenant-set retention window (7-90 days, default 7), a Workspace Admin can restore a warehouse to its pre-delete state, including schemas/data, snapshots, permissions/security settings, and objects like saved queries, views, and stored procedures. For fast-moving environments, this is a cleaner rollback than rebuilding and replaying pipelines, and it pairs with last week’s "productionize the plumbing" theme by reducing blast radius when mistakes happen.

- [Announcing the latest innovations in Fabric Data Factory: Apache Airflow jobs and pipelines](https://blog.fabric.microsoft.com/en-US/blog/announcing-the-latest-innovations-in-fabric-data-factory-apache-airflow-jobs-and-pipelines/)
- [Get notified when scheduled jobs fail in Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/get-notified-when-scheduled-jobs-fail-in-fabric-generally-available/)
- [Dropped warehouse recovery in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dropped-warehouse-recovery-in-microsoft-fabric-preview/)

## Azure

This week’s Azure items focused on operational guardrails: tighter network boundaries for PaaS, capacity/resiliency planning for IaaS, and event-driven patterns that reduce glue code while improving observability. Microsoft also continued pushing "modernize without rewrites" paths by moving pipelines into Fabric, making durable orchestration easier to consume, and improving local dev/test workflows with emulators and usage logs. It continues last week’s "controlled transitions" framing: adopt new primitives in phases, with "observe first, enforce later" and better day-2 visibility.

### Secure boundaries and governance for Azure services

The private-by-default and guardrail thread from last week continued here. Azure Service Bus now has Network Security Perimeter (NSP) generally available, which adds a perimeter layer so you can associate PaaS resources to a logical boundary and manage ingress/egress rules centrally. Rollout starts in Transition mode (observe/log without blocking) to inventory real traffic, then moves to Enforced mode where outside-perimeter access is denied by default and only allowed via explicit rules (inbound by IP ranges or subscriptions; outbound by FQDNs). For Service Bus with customer-managed keys, allowing PaaS-to-PaaS inside the perimeter can keep Key Vault working without per-resource exceptions while still logging for audit and troubleshooting, which matches last week’s "sequence prerequisites, then enforce" approach.

Azure Landing Zone (ALZ) subscription vending guidance also continued the "guardrails by default" theme and complements last week’s point that migrations go better when identity/network prerequisites and operational baselines are treated as first-class work. The overview treats subscriptions as the core governance boundary, with automated creation via approval + IaC pipelines (often JSON/YAML in source control, provisioned via Terraform). The baseline (management group placement, billing scope via aliases, budgets, provider registrations, RBAC/custom roles) turns subscription creation into a repeatable, auditable workflow and helps avoid provider-registration surprises later.

- [Announcing general availability of Network Security Perimeter for Azure Service Bus](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/announcing-general-availability-of-network-security-perimeter/ba-p/4508179)
- [Subscription Vending in Azure: An Implementation Overview](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/subscription-vending-in-azure-an-implementation-overview/ba-p/4506350)

### Compute, networking, and reliability: designing for constraints (capacity, limits, and failure modes)

A recurring reminder is that "it deployed" does not mean it will scale or restart reliably, which fits last week’s AKS theme around safer rollouts and ingress modernization. On AKS, a concrete scaling failure shows up when using AGIC with a single Azure Application Gateway fronting many apps: App Gateway has a hard limit of 100 backend pools, and common AGIC patterns consume one pool per Kubernetes Service referenced by an Ingress. Apply a 1:1:1 Deployment/Service/Ingress pattern 101 times and AGIC can hit `ApplicationGatewayBackendAddressPoolLimitReached`. Kubernetes objects may still apply successfully, so onboarding looks fine until routing for new apps fails because App Gateway reconciliation cannot complete. Mitigations focus on choosing an ingress architecture that fits service limits (and noting current gaps like private-frontend limitations in some newer controller paths), reinforcing last week’s "modernize, but design around managed dataplane limits" point.

For VM reliability, Azure On-Demand Capacity Reservations (ODCRs) are positioned for workloads that must start during capacity pressure, which is another "predictable ops" lever. Key points include: quota headroom does not guarantee physical capacity, Reserved Instances/Savings Plans do not improve start likelihood, and ODCR billing continues even when VMs are stopped because you are reserving capacity. A practical workflow for protecting existing running VMs is to create a Capacity Reservation Group and a reservation with quantity 0, associate VMs (even if temporarily overassociated), then increase reservation quantity to match running instances, which is often easier because those VMs already occupy host capacity.

Azure Compute also introduced a preview performance/reliability control: ephemeral OS disk with full caching for VM/VMSS. Ephemeral OS disks keep writes local, but reads can still depend on a remote base image; full caching asynchronously pulls the full OS image locally after boot so all OS IO becomes local once caching completes. It fits stateless scale-out services that want consistent OS-disk read latency, with an explicit tradeoff: local storage use is about 2x OS disk size to store the cached image. This matches the broader predictability theme: reduce variance in exchange for explicit capacity planning.

Reliability guidance also pushed more explicit fault planning. A fault-types taxonomy frames Azure failures across partial region faults and management-plane degradations (ARM, Managed Identity) that can break deployment/recovery even while apps still serve traffic. It helps teams design with IaaS building blocks (VMSS across zones, storage redundancy like ZRS/GRS, Backup/Site Recovery) plus detection/runbooks that do not assume a clean region up/down switch, which lines up with last week’s guardrails and health-signal approach.

- [AKS with AGIC hits Azure Application Gateway backend pool limit (100): reproduction and mitigations](https://techcommunity.microsoft.com/t5/azure-architecture-blog/aks-cluster-with-agic-hits-the-azure-application-gateway-backend/ba-p/4508201)
- [Demystifying On-Demand Capacity Reservations](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/demystifying-on-demand-capacity-reservations/ba-p/4504806)
- [Public Preview: Ephemeral OS Disk with full caching for VM/VMSS](https://techcommunity.microsoft.com/t5/azure-compute-blog/public-preview-ephemeral-os-disk-with-full-caching-for-vm-vmss/ba-p/4500191)
- [Proactive Reliability Series - Article 1: Fault Types in Azure](https://techcommunity.microsoft.com/t5/azure-architecture-blog/proactive-reliability-series-article-1-fault-types-in-azure/ba-p/4507006)
- [Azure IaaS: Keep critical applications running with built-in resiliency at scale](https://azure.microsoft.com/en-us/blog/azure-iaas-keep-critical-applications-running-with-built-in-resiliency-at-scale/)

### Event-driven integration patterns: from infrastructure drift to payments and durable workflows

Event Grid showed up in two practical patterns, building on last week’s "ingest once, route to many" direction. The first is infrastructure hygiene: keeping Private DNS accurate for Azure Container Instances in private VNets when container group IPs drift after updates/recreates. The approach avoids polling by subscribing to ARM lifecycle events (for example, `Microsoft.Resources.ResourceWriteSuccess` and `...ResourceDeleteSuccess`), triggering an Event Grid-driven Azure Function (Python), and reconciling forward A and reverse PTR records in Azure Private DNS. Key design details include parsing the ARM resource ID from the Event Grid `subject`, keeping the handler stateless and idempotent (Event Grid is at-least-once), and reconciling DNS to actual ACI state. A drift-tracker observer covers edge cases (manual edits, partial failures, delete/recreate races), and the RBAC breakdown (Reader on ACI RG, Private DNS Zone Contributor on zones, etc.) supports least-privilege deployments. It echoes last week’s private networking lessons: DNS and identity alignment often determine whether "private" works in practice.

The second pattern is business integration: Stripe’s Event Destinations can push payment events directly into Azure via the Event Grid partner integration, which avoids custom webhook hosting. Once in Event Grid, events can route to Functions/Logic Apps, Event Hubs, Service Bus, or Microsoft Fabric Real-Time Intelligence via Event Grid namespaces feeding Eventstreams/KQL. That flexibility matches last week’s mixed-pattern advice: standardize intake while choosing the downstream service per consumer.

Durable Task Scheduler (DTS) Consumption SKU is now GA, providing a managed durable orchestration backend for long-lived workflows and agent-like sessions without managing storage or capacity. Consumption billing is per "actions dispatched," limits are explicit (up to ~500 actions/sec, 30 days history retention), and ops tooling is stronger with a built-in dashboard for orchestration history, filtering, and management actions (pause/resume/terminate/raise events), secured with Entra ID + Azure RBAC. DTS is positioned as "any compute": it can back Durable Functions (including Flex Consumption), run with Azure Container Apps, or be used via Durable Task SDKs (.NET, Python, Java, JavaScript). This continues last week’s "reduce bespoke plumbing" theme by standardizing durable state/orchestration behind a managed control plane.

- [Detecting ACI IP Drift and Auto-Updating Private DNS (A + PTR) with Event Grid + Azure Functions](https://techcommunity.microsoft.com/t5/azure-architecture/detecting-aci-ip-drift-and-auto-updating-private-dns-a-ptr-with/m-p/4507667#M830)
- [Powering Event Driven Payments with Stripe and Azure Event Grid](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/powering-event-driven-payments-with-stripe-and-azure-event-grid/ba-p/4507094)
- [The Durable Task Scheduler Consumption SKU is now Generally Available](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/the-durable-task-scheduler-consumption-sku-is-now-generally/ba-p/4506682)

### Data modernization across Azure and Fabric: migration, real-time feeds, and database features

Fabric and Azure data tooling continued converging around two practical needs: move existing assets forward without rewrites, and get operational data into analytics faster with fewer moving parts. This continues last week’s Fabric theme: assessment-first migrations, triggers disabled by default after migration, and real-time ingestion patterns that reduce glue while improving visibility.

There is now an in-product preview experience to upgrade Azure Data Factory (ADF) and Synapse pipelines into Fabric Data Factory. It starts with assessment (supported vs unsupported activities), then migrates selected pipelines by mounting the current factory into a Fabric workspace and converting linked services into Fabric connections. A key safety default remains: migrated pipelines arrive with triggers disabled, so teams can validate before schedules run, which matches last week’s "validate, then cut over" rhythm.

For real-time analytics, Fabric Eventstreams "DeltaFlow" (preview) targets streaming Azure SQL Database changes (CDC-style inserts/updates/deletes) into analytics-ready tables. The focus is lowering operational overhead through automatic schema registration, destination table creation, and schema evolution when the SQL source changes. For teams maintaining DIY CDC-to-lakehouse pipelines, schema drift is often the failure point, and DeltaFlow is positioned to reduce that risk.

SQL Server 2025 is also adding native regex functions in T-SQL, based on Google’s RE2 engine. That lets more validation/extraction/search logic move into SQL instead of app code, CLR, or complex LIKE/PATINDEX patterns, while requiring awareness that behavior matches RE2 rather than backtracking engines. It aligns with the broader "SQL ergonomics + downstream analytics" direction referenced last week.

- [Modernize your ADF pipelines to unlock Fabric](https://blog.fabric.microsoft.com/en-US/blog/modernize-your-adf-pipelines-to-unlock-fabric/)
- [Turn Azure SQL Database changes into real-time analytics with Fabric Eventstreams DeltaFlow (Data Exposed)](https://www.youtube.com/watch?v=63awEoYxEGg)
- [Native Regex in SQL Server 2025 | Data Exposed: MVP Edition](https://www.youtube.com/watch?v=7-9uAZ8FgCE)

### Developer experience: local testing, PaaS direction, and usage observability

This week’s developer experience items reinforce the recent direction: make workflows repeatable (local test harnesses, fewer hidden dependencies) and add visibility for scaled operations.

Spring Cloud Azure published an emulator-first testing pattern for CI: run Azurite (Blob) and the Service Bus emulator (with required SQL backing store) via Spring Boot Docker Compose integration or Testcontainers. It goes past basics with real-world considerations such as BOM pinning (example uses Spring Cloud Azure 7.1.0), `@ServiceConnection` wiring, readiness timeouts for Service Bus + SQL Edge startup, Awaitility retries, and coverage for messaging clients (ServiceBusTemplate / ServiceBusSenderClient) plus Stream binder flows (manual checkpointing via `AzureHeaders.CHECKPOINTER`). In the context of last week’s standardization theme, this is the dev/test equivalent: fewer environment-specific workarounds and more deterministic validation.

Azure App Service published a planning-oriented direction update: Premium v4 is the newer premium tier with more CPU/memory options and improved price/perf while keeping deployment slots and zone resiliency; Managed Instance remains for Windows apps needing more isolation/private networking/OS customization while staying in the App Service model. Microsoft also highlighted alignment with modern patterns like .NET Aspire distributed apps and AI-backed web/API front ends.

Playwright Workspaces added Browser Activity Logs, recording each cloud browser session (Created -> Active -> Completed/Failed) with traceability and cost fields such as session ID/name, start/end and billable time, source type (test run vs automation tool), source IDs, browser/OS, and creator identity. For scaled cloud browser usage, it provides "who ran what, when, and what it cost" without stitching external logs, which matches last week’s observability thread.

- [Writing Azure service-related unit tests with Docker using Spring Cloud Azure](https://devblogs.microsoft.com/azure-sdk/writing-azure-service-related-unit-tests-with-docker-using-spring-cloud-azure/)
- [Continued Investment in Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/continued-investment-in-azure-app-service/ba-p/4507398)
- [Gain Visibility into Cloud Browser Usage with Browser Activity Logs in Playwright Workspaces](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/gain-visibility-into-cloud-browser-usage-with-browser-activity/ba-p/4506706)

### Other Azure News

Operational troubleshooting and observability got two runbook-friendly additions, continuing last week’s point that day-2 needs should be first-class. Azure CycleCloud Workspace for Slurm now has a blueprint (plus repo) for centralizing Slurm/CycleCloud/OS logs into Azure Monitor Logs using AMA + DCRs, with separate tables per source and VMSS association patterns. Logic Apps also added an automation path to revoke OAuth for API Connections by calling ARM `revokeConnectionKeys`, which is useful for incident response and credential rotation when RBAC is scoped correctly (custom roles for least privilege). This fits last week’s identity/governance focus: security often depends on tested "revoke + rotate" automation.

- [Centralized Log Management for CycleCloud Workspace for Slurm with Azure Monitor Logs](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/simplify-troubleshooting-at-scale-centralized-log-management-for/ba-p/4470658)
- [How to revoke connection OAuth programmatically in Logic Apps](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/how-to-revoke-connection-oauth-programmatically-in-logic-apps/ba-p/4506825)

Migration planning and economics showed up alongside hands-on troubleshooting. One article argues for involving FinOps earlier in AWS->Azure migrations: plan like-for-like, stabilize spend, then apply levers like Dev/Test pricing, Hybrid Benefit (including 180-day overlap), and Reservations once workloads settle, which matches last week’s "assessment first, phased optimization" pattern. Another post tackles Azure Migrate’s `MachineWithSameBiosIdAndFqdnAlreadyExists` error from mixing credential-based discovery with later agent registration, and shows how to realign the Mobility Agent to the original HostId/ResourceID identity so replication continues, which is a reminder that identity/registration details often drive migration timelines.

- [AWS to Azure Migration — From the Cloud Economics & FinOps Lens](https://techcommunity.microsoft.com/t5/azure-migration-and/aws-to-azure-migration-from-the-cloud-economics-finops-lens/ba-p/4506746)
- [Resolving MachineWithSameBiosIdAndFqdnAlreadyExists During Azure Migrate Mobility Agent Registration](https://techcommunity.microsoft.com/t5/azure-migration-and/resolving-machinewithsamebiosidandfqdnalreadyexists-during-azure/ba-p/4492307)

Azure also published new material for specialized scenarios. A DPDK 25.11 performance write-up (and report) highlights what drives predictable throughput for packet workloads: Accelerated Networking, Azure Boost where available, NUMA alignment, hugepages, vCPU pinning, and queue/thread mapping. For sovereign/disconnected deployments, Microsoft described work with Armada to run Azure Local on modular datacenters, pairing an Azure-consistent control plane with edge networking and positioning Foundry Local for on-site inference when public-cloud connectivity is not reliable. It matches last week’s hybrid/sovereign framing: connectivity and control-plane reachability are primary design inputs.

- [DPDK 25.11 Performance on Azure for High-Speed Packet Workloads](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/dpdk-25-11-performance-on-azure-for-high-speed-packet-workloads/ba-p/4424905)
- [Building sovereign AI at the edge: Microsoft and Armada collaborate to deliver Azure Local on Galleon modular datacenters](https://azure.microsoft.com/en-us/blog/building-sovereign-ai-at-the-edge-microsoft-and-armada-collaborate-to-deliver-azure-local-on-galleon-modular-datacenters/)

A couple of broad platform and storage/AI-data notes are useful for roadmap awareness. John Savill’s weekly Azure Update touched compute (ephemeral OS disk caching), config/edge (App Configuration + Front Door), storage (user delegation SAS expansions), and a Cosmos DB for PostgreSQL retirement callout that should trigger dependency checks, which matches last week’s reminder that small notices become real work once you inventory. A Komprise + Azure Storage piece outlines migrating and tiering unstructured data into Blob Storage with governance and ransomware-resilience controls (immutability/object lock/versioning/backup), aiming to make curated datasets easier to feed into AI pipelines. It fits the broader idea that durable storage controls and data hygiene are prerequisites for reliable AI/analytics use.

- [Azure Update 3rd April 2026](https://www.youtube.com/watch?v=x8ULC4uDQos)
- [Unlocking AI-Ready Unstructured Data at Scale with Komprise and Azure](https://techcommunity.microsoft.com/t5/azure-storage-blog/unlocking-ai-ready-unstructured-data-at-scale-with-komprise-and/ba-p/4507422)

## .NET

This week’s .NET items leaned toward "what’s next," with early looks at language features and framework experiments that could change how you model APIs and configure apps. MAUI also clarified how to try new UI ideas without waiting for full releases. This split between stable baselines and previews/experiments continues from last week: alongside GA paths like Aspire on App Service, the .NET 11 Preview 2 thread keeps producing deeper language/runtime experiments, and MAUI is formalizing an "expect churn" lane through an experiments hub.

### C# 15 union types in .NET 11 Preview

C# 15 (starting with .NET 11 Preview 2) introduces union types as a first-class way to define a closed set of value shapes without `object`, marker interfaces, or awkward base-class hierarchies. Following last week’s .NET 11 Preview 2 coverage, this is another Preview 2 feature that is likely to evolve as tooling and runtime pieces land. With `union`, you can declare `public union Pet(Cat, Dog, Bird);`, and the compiler treats the cases as complete: it supports implicit conversions from each case type (for example, `Pet pet = new Dog("Rex");`) and enforces exhaustive pattern matching in `switch` expressions. The maintenance benefit is clear: if you add a new case later, existing switches can warn when they are no longer exhaustive.

The preview includes important semantics and caveats. Patterns generally apply to the generated `Value` (auto-unwrapping), except `var` and `_`, which bind/match the union itself. Nullability matters: the default union value has `Value == null`, and the `null` pattern checks whether `Value` is null; if any cases are nullable (for example, `Bird?`), you must handle `null` explicitly for exhaustiveness. Under the hood, `union` is shorthand for a compiler-generated struct with per-case constructors and a `Value` of type `object?`, so value-type cases box by default.

For library authors and performance-sensitive code, "custom union types" are also supported. If you annotate a type with `[System.Runtime.CompilerServices.Union]` and follow the expected shape (public single-parameter constructors plus a public `Value` property), the compiler treats it as a union. Adding `HasValue` / `TryGetValue` can enable union-aware patterns that avoid boxing for value-type cases. To try it now, install the .NET 11 Preview SDK, target `net11.0`, set `<LangVersion>preview</LangVersion>`, and add runtime polyfills for `UnionAttribute` and `IUnion` (not included in .NET 11 Preview 2 yet). IDE support is expected via upcoming Visual Studio Insiders builds and is already in the latest C# Dev Kit Insiders.

- [Explore union types in C# 15](https://devblogs.microsoft.com/dotnet/csharp-15-union-types/)

### Contextual options: per-request configuration via an experimental extensions package

The options pattern got an experimental add-on: `Microsoft.Extensions.Options.Contextual`, a NuGet package that adds a contextual layer on top of `IOptions<T>`. Building on last week’s theme of code-first workflows across more environments, it keeps configuration DI-driven while letting it adapt to request/tenant/user context. Instead of global or named options, you resolve `IContextualOptions<TOptions, TContext>` and call `GetAsync(context)` to compute options for a specific context. The walkthrough uses an ASP.NET Core "weather forecast" app with an `AppContext` (annotated `[OptionsContext]` and `partial`) carrying fields like `UserId` and `Country`, then derives defaults from context at the call site.

Mechanically, there are three parts: a source generator (`ContextualOptionsGenerator`) emits an `IOptionsContext` implementation; you implement an `IOptionsContextReceiver` that consumes key/value pairs via `Receive<T>(string key, T value)`; and you register an additional contextual configure callback `(IOptionsContext ctx, TOptions opts)` to apply derived values. The post calls out a maintainability risk: receivers are coupled to context properties via string keys (property names), so renames can silently change behavior. There is also the cost of adopting an `[Experimental]` API: the package triggers `EXTEXP0018` unless you opt in, and generated code is also experimental, so teams often suppress warnings broadly (for example, `<NoWarn>$(NoWarn);EXTEXP0018</NoWarn>`). The conclusion is to evaluate it mainly if you already rely on `IOptions` and need true per-context config that named options cannot express; otherwise feature flag tooling (`Microsoft.FeatureManagement`, OpenFeature) may fit better.

- [Configuring contextual options with Microsoft.Extensions.Options.Contextual](https://andrewlock.net/configuring-contextual-options-with-microsoft-extensions-options-contextual/)

### .NET MAUI’s new home for experiments: maui-labs

The .NET MAUI Community Standup introduced maui-labs as the official home for experimental and community-driven MAUI work, including prototypes and in-progress ideas not ready for stable MAUI. This fits the pattern we have been tracking: last week’s MAUI + Avalonia backend preview (Linux and WebAssembly) showed active experimentation around rendering and reach, and maui-labs clarifies where that work should live so teams can try it without confusing it with supported MAUI. The practical benefit is clearer boundaries: developers get one place to follow and test early work on expanded platform support, alternate rendering options, and exploratory features, with a clearer path for what might later graduate into the product.

- ['.NET MAUI Community Standup: Introducing maui-labs'](https://www.youtube.com/watch?v=IfCIubKbyqw)

## DevOps

This week’s DevOps items covered familiar platform concerns: securing CI/CD without extra secrets, making dev environments workable in regulated orgs, and tightening everyday feedback loops. Longer write-ups also looked at operational scale, including cross-cloud incident investigation with agent tooling, release pipeline reliability, and the realities of rendering very large diffs.

### GitHub Actions and GitHub platform updates for CI/CD and governance

GitHub Actions’ early April updates reduced friction in common workflows while tightening cloud-access security controls. Workflow authors can now override a service container’s defaults in YAML using new `entrypoint` and `command` keys under `jobs.<job_id>.services`, similar to Docker Compose, which avoids forking images just to change startup flags. OIDC tokens now include repository custom properties as claims (GA), which enables cloud trust policies tied to governance metadata like `environment`, `owning_team`, or `compliance_tier` instead of long repo allowlists. This builds on last week’s CI-hardening theme: once runner environments are more consistent, identity assumption becomes the next control. For orgs using Azure private networking with GitHub-hosted runners, VNet failover networks (public preview) add resilience by letting you configure a secondary subnet (optionally cross-region) and fail over manually (UI/REST API) or automatically, with audit log and email notifications.

Across the GitHub platform, improved Issues search is now GA with semantic and hybrid modes for titles/bodies. You can use natural-language queries in the UI (repo-scoped or across the Issues dashboard), while tools can call REST `/search/issues` with `search_type=semantic|hybrid` or GraphQL `searchType`. This fits last week’s "review triage and queue management" direction: better discovery helps keep operational work searchable as CI events and bot signals grow. Operationally, semantic/hybrid is rate-limited to 10 requests/minute, so bots and dashboards need to budget; filter-only and quoted searches remain lexical.

- [GitHub Actions: Early April 2026 updates](https://github.blog/changelog/2026-04-02-github-actions-early-april-2026-updates)
- [Improved search for GitHub Issues is now generally available](https://github.blog/changelog/2026-04-02-improved-search-for-github-issues-is-now-generally-available)

### GitHub enterprise developer environments and supply chain automation

Two GitHub releases landed in platform-team territory: enterprise dev environments and dependency hygiene automation. Codespaces is now GA for GitHub Enterprise Cloud with data residency (Australia, EU, Japan, US) and feature parity with standard Codespaces. The key constraint is ownership: only enterprise- or organization-owned codespaces are supported (no user-owned), so admins must set Codespaces policies for compliant provisioning/billing while preserving "devcontainer in minutes" workflows. This continues last week’s push for repeatable environments (runner images in CI) with a "standardize dev" path where workstation variance and data locality have blocked adoption.

Dependabot also added support for SwiftPM dependencies managed inside Xcode bundles, for repos storing config in `.xcodeproj`/`.xcworkspace` rather than top-level `Package.swift`. It can discover `Package.resolved` inside Xcode bundle layouts, read SwiftPM rules from `project.pbxproj`, and open PRs updating the right resolved files. It keeps the existing `dependabot.yml` model (schedules, grouping, ignores). GHES support is planned for 3.22.

- [Codespaces is now generally available for GitHub Enterprise with data residency](https://github.blog/changelog/2026-04-01-codespaces-is-now-generally-available-for-github-enterprise-with-data-residency)
- [Dependabot now supports Xcode projects using SwiftPM with .xcodeproj manifests](https://github.blog/changelog/2026-03-31-dependabot-now-supports-xcode-projects-using-swiftpm-with-xcodeproj-manifests)

### Azure DevOps and Azure operations: publishing automation, work tracking UX, and cross-cloud investigations via MCP

Azure DevOps extension publishing automation got a refresh with the azdo-marketplace v6 rebuild. v6 consolidates multiple tasks into one task/action using an `operation` parameter (`package`, `publish`, `install`, `share`, `unpublish`, various `wait-*` gates), aligns behavior across Azure Pipelines and GitHub Actions, and reduces distribution size (to ~20 MB from ~300 MB) while adding extensive tests and cross-platform CI. A key security improvement is first-class GitHub Actions OIDC support (workload identity federation) to Azure DevOps, which reduces reliance on PATs. PAT/basic auth remain for compatibility, but the direction favors federated identity and service connections, continuing last week’s "reduce secret sprawl" theme for extension publishing supply chains.

Azure Boards is also rolling out a Markdown editor UX change aimed at reducing accidental edits in large text fields. Fields default to preview mode, editing is explicit via an edit icon, and "done" returns to preview. It targets triage flows where double-clicking to read/select text used to create unintended edits, which fits last week’s "reduce review noise" thread (PR dashboards, comment controls).

A deep operations guide showed cross-cloud investigations from one Azure SRE Agent chat by connecting Azure SRE Agent (Azure portal) to AWS via the AWS MCP Server and MCP Proxy for AWS. The setup is lightweight: Azure SRE Agent launches a local stdio connector via `uvx` (Astral `uv`), and the proxy forwards HTTPS to an AWS MCP endpoint (for example, `https://aws-mcp.us-east-1.api.aws/mcp`) with SigV4 signing using IAM creds from environment variables, with no container and no additional hosted infrastructure. This matches the "make ops repeatable and auditable" theme: turn investigations into tool calls rather than portal clicks. Once connected, AWS MCP Server exposes 23 MCP tools (docs lookups, authenticated AWS API execution with validation/error handling, guided Agent SOPs aligned to Well-Architected, and AWS DevOps Agent operations). The guide covers IAM setup (`aws-mcp:InvokeMcp`, `aws-mcp:CallReadOnlyTool`, optionally `aws-mcp:CallReadWriteTool`, plus service permissions), Azure SRE Agent skill configuration, and troubleshooting (403s from missing permissions, 401s from rotated keys, and restart/redeploy needs because MCP connections initialize at startup). It also highlights using AWS DevOps Agent tools (AgentSpace management, investigation/task lifecycle, journal/recommendations, evaluations, chat) alongside Azure telemetry for a unified RCA and remediation plan.

- [Introducing Azure DevOps Marketplace tasks and actions: A Complete Rebuild for Speed, Stability, and Security](https://jessehouwing.net/azure-devops-marketplace-tasks-and-actions/)
- [Improving the Markdown Editor for Work Items](https://devblogs.microsoft.com/devops/improving-the-markdown-editor-for-work-items/)
- [Announcing AWS with Azure SRE Agent: Cross-Cloud Investigation using the brand new AWS DevOps Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-aws-with-azure-sre-agent-cross-cloud-investigation/ba-p/4507413)

### Other DevOps News

IaC workflows got a CI-friendly drift-detection recipe designed for human governance: generate deterministic plan artifacts (`terraform plan -out=tfplan`, `terraform apply tfplan`), add a drift gate with `terraform plan -refresh-only -detailed-exitcode` (0 no drift, 2 drift, 1 error), and use Azure Resource Graph and Azure Policy queries to understand changes and compliance slips. Copilot is framed as helpful for summarizing/triaging noisy outputs (plans, KQL, policy states) without replacing RBAC or approvals. This matches last week’s "repeatable primitives enforce expectations" theme: drift checks and deterministic plans turn "we should notice changes" into a predictable gate.

- [AI‑Assisted Azure Infrastructure Validation and Drift Detection](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-assisted-azure-infrastructure-validation-and-drift-detection/ba-p/4508346)

Release engineering reliability showed up in the PowerShell 7.6 postmortem, explaining why the March 2026 release slipped: late packaging/tooling changes, weaker preview signal, cross-platform validation gaps, and distro constraints (Alpine packaging, RHEL 8 glibc 2.28). For teams depending on distribution channels, the concrete scope is useful: 29 packages across 8 formats, 4 architectures, 8 OSes, published via GitHub, PMC, winget, Store, NuGet. Planned investments include clearer release ownership, better tracking/communication, more consistent previews, simplified packaging, and fewer manual publishing steps, which matches last week’s theme of reducing fragile automation in high-coordination processes.

- [PowerShell 7.6 release postmortem and investments](https://devblogs.microsoft.com/powershell/powershell-7-6-release-postmortem/)

GitHub’s diff rendering performance deep dive is a reminder that developer experience often comes down to systems work. By reducing per-line DOM/component/handler overhead, using event delegation, hiding heavy state behind conditional rendering, enforcing O(1) indexed state access, and adding TanStack Virtual windowing, GitHub improved responsiveness for PRs with 10,000+ lines. It is the engineering counterpart to last week’s workflow UX updates: as triage UX expands, pages still need to stay fast under real repo diff sizes. These patterns translate to many React list/table UIs: simplify per-row structure, avoid per-item effects/handlers, and virtualize when full rendering becomes the bottleneck.

- [The uphill climb of making diff lines performant](https://github.blog/engineering/architecture-optimization/the-uphill-climb-of-making-diff-lines-performant/)

## Security

This week’s security items reflected two pressures: intrusions that abuse everyday automation (dependency installs, hosted web stacks, messaging attachments) and platform changes intended to make those workflows harder to exploit (CI hardening, secret detection, governable data/AI). Building on last week’s theme (attackers using default paths like dependency installs and workflow triggers, defenders adding enforceable guardrails), this week focused on high-leverage control points: npm installs, Actions runs, `kubectl` applies, and REST API inventory jobs.

### Supply chain pressure: npm dependency installs and GitHub workflow hardening

After last week’s Trivy compromise (mutable tags, runner discovery, secret harvesting), the axios npm incident reinforced the same lesson: a supply chain compromise can cause damage even if it never ships in runtime, because install-time scripts can target developer laptops or CI runners. Two malicious releases, axios@1.14.1 and axios@0.30.4, added `plain-crypto-js@^4.2.1`, which runs a postinstall script (`node setup.js`). The loader is obfuscated, fingerprints the OS, then calls `hxxp://sfrclak[.]com:8000/6202033` (142.11.206[.]73) to fetch an OS-specific second stage: a macOS binary dropped to `/Library/Caches/com.apple.act.mond` launched via AppleScript/osascript, a Windows PowerShell RAT staged in `%TEMP%` with persistence at `HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\run\\MicrosoftUpdate` (and camouflage like `C:\\ProgramData\\wt.exe`), or a Linux Python loader written to `/tmp/ld.py`. It also attempts cleanup by removing triggering files and restoring a benign-looking `package.json`.

For teams, the impact mirrors last week’s pipeline guidance: treat dependency updates as incidents until you confirm otherwise, because build hosts and secrets may already be exposed. Recommended actions include downgrading to known-good axios (1.14.0 or earlier; 0.30.3 or earlier), pinning exact versions (avoid `^`/`~`), enforcing with npm overrides, clearing npm cache, reviewing logs for bad versions or `plain-crypto-js@4.2.1`, rotating secrets if runners may be compromised, and considering `npm ci --ignore-scripts` (or `ignore-scripts=true`) where feasible while acknowledging some ecosystems rely on scripts. Microsoft provided hunting guidance (KQL for Defender XDR/Sentinel) for package inventory, suspicious `setup.js` execution, and outbound traffic to the listed C2, plus IOCs and Defender detection names.

In parallel, GitHub’s supply-chain guidance continues last week’s Actions hardening direction: reduce reliance on trust-by-convention (mutable refs, broad secrets) and make workflows more resistant to Trivy-style pivots. It recommends enabling CodeQL scanning for workflow YAML with the Actions query pack, avoiding `pull_request_target` when possible, pinning third-party actions to full commit SHAs (and treating pin changes as high-risk), and hardening against script injection when interpolating user-controlled values (branch names, PR titles). It also emphasizes replacing long-lived secrets with OIDC federated identity and using "trusted publishing" (OpenSSF-aligned) to improve provenance without embedding publish creds in pipelines. Together, axios plus GitHub workflow guidance continues last week’s "guardrails on default paths" thread: dependency resolution and CI config are core attack surfaces, and a dependable mitigation is minimizing secret exposure while tightening what can run.

- [Mitigating the Axios npm supply chain compromise](https://www.microsoft.com/en-us/security/blog/2026/04/01/mitigating-the-axios-npm-supply-chain-compromise/)
- [Securing the open source supply chain across GitHub](https://github.blog/security/supply-chain-security/securing-the-open-source-supply-chain-across-github/)

### Threat research and hunting: stealthy Linux webshells and WhatsApp-delivered Windows chains

Microsoft documented a stealthy Linux hosting technique: PHP webshells that take commands from HTTP cookies instead of query params or POST bodies. This matches last week’s framing that routine-looking traffic can hide execution, so defenses should focus on enforceable controls and observable choke points. Because the trigger is in `$_COOKIE`, shells can stay dormant under normal browsing and avoid obvious log indicators. Variants include obfuscated loaders that reconstruct function names, write second stages, then `include` them, and interactive single-file shells gated by a "key" cookie. Persistence often uses cron "self-healing" to recreate loaders (including in cPanel/jailshell contexts) plus permission tweaks that slow cleanup. Detection guidance maps to ops reality: hunt for web server processes spawning shells and tools (`base64`, `curl`, `wget`), pipelines that decode/write `.php` into web directories, and cron jobs running frequently that write into web paths. Defender for Endpoint on Linux setup and Defender XDR KQL queries are provided.

Another Defender post covered a WhatsApp-delivered Windows campaign that starts with a malicious `.vbs` attachment and ends with unsigned MSI installers used for persistence and remote access. It continues last week’s identity/containment theme: attacker chains blend into normal admin/user behavior, so hunting depends on lineage, metadata, and policy containment. The chain creates hidden folders in `C:\\ProgramData`, drops renamed legitimate utilities (for example, `curl.exe` renamed to `netapi.dll`, `bitsadmin.exe` renamed to `sc.exe`), downloads additional VBS from AWS S3/Tencent COS/Backblaze B2, then tampers with UAC-related registry values to reduce prompts while attempting elevation. Final-stage unsigned MSI packages (including "AnyDesk.msi") blend into typical software installs. A defender tip is detecting renamed binaries via PE metadata mismatches (for example, `VersionInfoOriginalFileName`), plus KQL hunts for `wscript/cscript` from suspicious locations, downloader flags, and `.vbs`/`.msi` drops tied to renamed utilities. Hardening guidance focuses on ASR rules (obfuscated scripts, low-prevalence executables, blocking VBScript/JavaScript launching downloaded executables), script host restrictions, and enabling EDR-in-block-mode and tamper protection.

- ['Cookie-controlled PHP webshells: A stealthy tradecraft in Linux hosting environments'](https://www.microsoft.com/en-us/security/blog/2026/04/02/cookie-controlled-php-webshells-tradecraft-linux-hosting-environments/)
- [WhatsApp malware campaign delivers VBS payloads and MSI backdoors](https://www.microsoft.com/en-us/security/blog/2026/03/31/whatsapp-malware-campaign-delivers-vbs-payloads-msi-backdoors/)

### GitHub security operations: secret scanning expansion and CodeQL reporting that matches real branching

GitHub security tooling continued shifting from "enable it" to "operate it," which matches last week’s faster feedback loops (incremental CodeQL in PRs) and tighter control surfaces (push protection exemptions, credential revocation). Secret Scanning added nine detectors across seven providers (including Figma SCIM tokens, LangSmith keys/tokens, PostHog OAuth tokens, Salesforce Marketing Cloud tokens) and added validity checks for `npm_access_token` so alerts can show whether a token is still active. Push protection also expanded default blocking to more secret types (including Figma SCIM tokens and specific Google/OpenVSX/PostHog patterns) when Secret Scanning is enabled, which reduces the chance secrets land in history. In the context of last week’s "rotate/revoke quickly" theme, the improvement is triage quality: not just "a token exists," but "it still works."

CodeQL 2.25.0 updates align analysis with current toolchains and improve precision: Swift 6.2.4 support; a rewritten Java/Kotlin control flow graph focusing on reachable nodes; C# support for C# 14 partial constructors; and taint tracking that treats `System.Net.WebSockets::ReceiveAsync` as a remote source to improve WebSocket findings. JavaScript/TypeScript also gained browser source kinds (`browser-url-query`, `browser-url-fragment`, `browser-message-event`) for teams extending CodeQL models. Along with last week’s incremental PR scanning push, the direction stays consistent: keep scans fast enough for PR loops while improving modeling fidelity.

GitHub Security Overview’s CodeQL "pull request insights" now aggregates across all protected branches, not just the default branch, and CSV export matches. This fits last week’s "govern rollouts, reduce drift" framing: teams often ship from release/* and maintenance branches, so default-branch-only reporting undercounts both risk and remediation work. After rollout, dashboards (and Copilot Autofix outcome counts) should better reflect what is being fixed and shipped, though historical numbers may shift.

- [GitHub secret scanning — coverage update](https://github.blog/changelog/2026-03-31-github-secret-scanning-nine-new-types-and-more)
- [CodeQL 2.25.0 adds Swift 6.2.4 support](https://github.blog/changelog/2026-03-31-codeql-2-25-0-adds-swift-6-2-4-support)
- [CodeQL pull requests insights on security overview now cover all protected branches](https://github.blog/changelog/2026-03-31-codeql-pull-requests-insights-on-security-overview-now-cover-all-protected-branches)

### Security governance in Azure and Microsoft Fabric: admission control, labeling, encryption, and recovery

AKS guidance emphasized where security checks prevent incidents in practice: admission time, not only CI. This matches last week’s "guardrails on execution paths" framing by shifting enforcement from pipelines to cluster-side controls that still apply when someone uses `kubectl` or when drift accumulates. The approach combines early CI guardrails with Azure Policy for AKS (OPA Gatekeeper) to enforce policies in-cluster. It recommends staged Pod Security Standards rollout (Audit first, then Deny in production namespaces), network policy enforcement to limit lateral movement, and image governance at admission so only approved registries/images run. Runtime coverage comes from Microsoft Defender for Containers (with a reminder that restricted-egress clusters need outbound endpoint planning), while Azure Policy compliance reporting provides continuous audit/drift views across clusters/namespaces.

Fabric governance became easier to automate, continuing last week’s "enforceable controls without rewrites" thread plus better API surfaces for inventory/policy. Sensitivity labels are now returned in Fabric Public REST APIs (List Items, Get Item, Update Item), reducing extra per-item metadata calls for inventory/compliance workflows; label updates still use Admin label management endpoints, including Bulk Set/Remove. This supports cleaner label-aware automation patterns, including for AI/agent workflows that must filter access by "Confidential"/"Restricted" classifications, which we highlighted last week with Purview monitoring and governance.

Fabric also introduced Item Recovery (preview): item-level soft delete with a workspace recycle bin and tenant-configurable retention (7-90 days), with restore via portal or REST API. This extends last week’s identity/recovery reminders (Entra Backup/Restore) into the analytics plane, because governance also includes practical recovery when mistakes or malicious deletions happen. Beyond undoing deletes for notebooks/pipelines/lakehouses, it supports forensics by restoring artifacts (and their lineage/labels) when audit logs only show create/delete. Fabric preview also allows workspace-level Customer-Managed Keys (CMK) even when the workspace is on a BYOK-enabled capacity, removing a constraint that previously forced capacity splits. The separation remains (BYOK for Power BI semantic models at capacity; workspace CMK for other Fabric items), but it simplifies regulated deployments and key runbooks, complementing last week’s CMK GA for Fabric SQL Database.

- ['DevSecOps on AKS: Governance Gates That Actually Prevent Incidents'](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/devsecops-on-aks-governance-gates-that-actually-prevent/ba-p/4508415)
- [Sensitivity labels in Fabric for public APIs (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/sensitivity-labels-in-fabric-for-public-apis-generally-available/)
- [Item Recovery in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/item-recovery-in-microsoft-fabric-preview/)
- [Workspace Customer-Managed Keys for BYOK in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/workspace-customer-managed-keys-for-byok-in-microsoft-fabric-preview/)

### Other Security News

Agent security guidance continued shifting from model behavior to enforceable control surfaces, building on last week’s agent-governance focus on intent, identity, and runtime checks. Microsoft mapped the OWASP Top 10 Risks for Agentic Applications (2026) to Copilot Studio guardrails: restrict allowed connectors/actions, apply DLP to limit data movement, use isolation + republishing to prevent runtime self-modification, and ensure operational "kill switch" controls (restrict/disable/stop sharing). A companion governance layer is Agent 365 (preview, GA noted as May 1) as a lifecycle control plane for monitoring and policy enforcement across deployed agents.

- [Addressing the OWASP Top 10 Risks in Agentic AI with Microsoft Copilot Studio](https://www.microsoft.com/en-us/security/blog/2026/03/30/addressing-the-owasp-top-10-risks-in-agentic-ai-with-microsoft-copilot-studio/)

GitHub enablement content focused on making repo security features part of normal PR work, echoing last week’s "earlier feedback in PRs" direction (incremental CodeQL) and the broader goal of workable controls at scale. A "getting started" guide covers enabling GHAS features (Secret Scanning, Dependabot alerts/security updates, CodeQL) and using Copilot Autofix for eligible CodeQL alerts, while reminding teams remediation needs review and secret leaks require provider rotation/revocation. A Dependabot short reinforces the workflow: let Dependabot open patch PRs, validate with CI/tests, merge to reduce time-to-fix. Secret scanning education reiterates the response loop: find the line, remove the secret, rotate/revoke, and confirm downstream updates.

- ['GitHub for Beginners: Getting started with GitHub security'](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-security/)
- [How to fix vulnerabilities automatically with Dependabot](https://www.youtube.com/shorts/kyjQXPTuvqo)
- [How GitHub secret scanning saves your code](https://www.youtube.com/shorts/wYmTs1LSvTw)

A small GitHub UI change may affect docs: the "Security" tab is now "Security & quality," "Vulnerability alerts" is now "Findings," and a "Code quality" section appears in the repo sidebar. URLs/APIs are unchanged, but internal runbooks, training, and screenshots may need updates; GHES does not get this yet.

- [The Security tab is now Security & quality](https://github.blog/changelog/2026-04-02-the-security-tab-is-now-security-quality)

GitHub Enterprise Cloud’s EU data residency region expands May 1, 2026 to include Azure regions in Norway and Switzerland (EFTA), aligning with Microsoft’s EU Data Boundary. No certification/control changes are claimed, but organizations requiring EU-member-state-only residency should contact support/account teams ahead of the date. It pairs with last week’s governance story: where the platform runs remains part of compliance alongside keys, labels, and auditability.

- [EU data residency region expanding to include EFTA countries](https://github.blog/changelog/2026-03-31-eu-data-residency-region-expanding-to-include-efta-countries)

Microsoft Threat Intelligence also outlined how generative AI is increasingly embedded in attacker workflows, including higher-conversion phishing paired with infrastructure that bypasses MFA via adversary-in-the-middle (AiTM) token theft. This continues last week’s identity-first framing: attackers target what identities can reach, and defenses prioritize phishing-resistant MFA plus context-aware containment. The post highlights Tycoon2FA (Storm-1747) as a modular cybercrime service, notes disruption efforts (including domain seizures), and reinforces that token theft and phishing-resistant MFA are central to modern defense.

- [Threat actor abuse of AI accelerates from tool to cyberattack surface](https://www.microsoft.com/en-us/security/blog/2026/04/02/threat-actor-abuse-of-ai-accelerates-from-tool-to-cyberattack-surface/)
