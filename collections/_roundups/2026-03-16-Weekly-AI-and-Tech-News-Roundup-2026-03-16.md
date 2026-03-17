---
title: "Copilot Becomes More Agent-Driven, While Guardrails and Ops Tools Catch Up"
author: "TechHub"
date: 2026-03-16 09:00:00 +00:00
tags: ["GitHub Copilot","Agentic Workflows","VS Code","JetBrains","MCP (Model Context Protocol)","OpenTelemetry","GitHub Actions","Azure SRE Agent","Microsoft Agent Framework","Microsoft Foundry","Microsoft Fabric","Azure Kubernetes Service (AKS)",".NET 11","CodeQL","Secret Scanning"]
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
external_url: "/all/roundups/Weekly-AI-and-Tech-News-Roundup-2026-03-16"
---

Welcome to this week’s roundup. The common thread is agents moving beyond “helpful chat” into real execution across IDEs, terminals, CI, and cloud operations. Copilot’s latest changes focus on autonomy and repeatable behavior through repo-visible instruction files, lifecycle hooks, clearer model routing, and faster PR review workflows, while modernization tooling ties assessments and plans directly to issues and pull requests. In parallel, the rest of the stack is catching up to the day-to-day requirements of running agents like software: traces and debugging loops, structured outputs and schema enforcement, and clearer guardrails around approvals, secrets, and identity-based access.

<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Agentic Copilot in IDEs: customization, autonomy, and observability](#agentic-copilot-in-ides-customization-autonomy-and-observability)
  - [GitHub Copilot CLI and PR workflows: multi-agent parallelism and terminal-first review](#github-copilot-cli-and-pr-workflows-multi-agent-parallelism-and-terminal-first-review)
  - [Modernization agents and migration integration: from repo changes to portfolio orchestration](#modernization-agents-and-migration-integration-from-repo-changes-to-portfolio-orchestration)
  - [Security, governance, and “auto-approval” controls for agentic workflows](#security-governance-and-auto-approval-controls-for-agentic-workflows)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Microsoft Agent Framework: production patterns for multi-agent apps (Python + .NET)](#microsoft-agent-framework-production-patterns-for-multi-agent-apps-python--net)
  - [Azure automation and agent operations: Skills Plugin, SRE Agent “Deep Context,” and azd debugging](#azure-automation-and-agent-operations-skills-plugin-sre-agent-deep-context-and-azd-debugging)
  - [Microsoft Foundry and Microsoft Fabric: model deployment choices and production telemetry/evaluation loops](#microsoft-foundry-and-microsoft-fabric-model-deployment-choices-and-production-telemetryevaluation-loops)
  - [Other AI News](#other-ai-news)
- [Azure](#azure)
  - [SRE, incident response, and observability getting more “context-aware”](#sre-incident-response-and-observability-getting-more-context-aware)
  - [Cloud-native operations: isolated AKS inference and clearer container supply signals](#cloud-native-operations-isolated-aks-inference-and-clearer-container-supply-signals)
  - [Compute security and contracts: Confidential VM workflows, plus an API response change to watch](#compute-security-and-contracts-confidential-vm-workflows-plus-an-api-response-change-to-watch)
  - [App platform and API patterns: Heroku exits, and APIM as a BFF without another microservice](#app-platform-and-api-patterns-heroku-exits-and-apim-as-a-bff-without-another-microservice)
  - [Other Azure News](#other-azure-news)
- [.NET](#net)
  - [.NET 11 Preview 2: runtime, web, and data stack updates](#net-11-preview-2-runtime-web-and-data-stack-updates)
  - [Built-in Zstandard compression in .NET 11 and ASP.NET Core](#built-in-zstandard-compression-in-net-11-and-aspnet-core)
  - [.NET servicing: March security patches and a macOS VS Code debugger hotfix](#net-servicing-march-security-patches-and-a-macos-vs-code-debugger-hotfix)
  - [Other .NET News](#other-net-news)
- [DevOps](#devops)
  - [Azure SRE Agent reaches GA—and adds production governance for agentic ops](#azure-sre-agent-reaches-gaand-adds-production-governance-for-agentic-ops)
  - [GitHub Actions and GitHub platform operations: runner compliance, richer OIDC claims, and reliability learnings](#github-actions-and-github-platform-operations-runner-compliance-richer-oidc-claims-and-reliability-learnings)
  - [Azure DevOps operations: urgent patching and a deadline to migrate Advanced Security automation](#azure-devops-operations-urgent-patching-and-a-deadline-to-migrate-advanced-security-automation)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Identity-centered secure access for file transfer (Azure Blob Storage SFTP + Entra ID)](#identity-centered-secure-access-for-file-transfer-azure-blob-storage-sftp--entra-id)
  - [GitHub Advanced Security updates: sharper code scanning and stronger leak prevention](#github-advanced-security-updates-sharper-code-scanning-and-stronger-leak-prevention)
  - [Threat research: attackers blending into developer workflows (interviews, GitHub assets, and signed installers)](#threat-research-attackers-blending-into-developer-workflows-interviews-github-assets-and-signed-installers)
  - [Securing enterprise AI usage: prompt abuse playbooks and agent governance control planes](#securing-enterprise-ai-usage-prompt-abuse-playbooks-and-agent-governance-control-planes)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week’s Copilot updates continued the move toward agentic workflows: more autonomy in editors and the CLI, more customization through instruction files and hooks, and more attention on running agents safely in real repositories. Building on last week’s VS Code lifecycle hooks, default memory, and MCP tool integrations, these ideas are now showing up across IDEs (notably JetBrains) with more reviewable on-disk configuration plus better observability, troubleshooting, and governance. Copilot also kept improving everyday workflows, including faster terminal-based code review requests, better web repo exploration, and portfolio-scale modernization that ties code changes to migration planning.

### Agentic Copilot in IDEs: customization, autonomy, and observability

GitHub Copilot’s JetBrains plugin moved key in-IDE agent building blocks to GA - Custom Agents, Sub-agents, and a Plan Agent - so teams can define specialized roles inside IntelliJ workflows. This matches last week’s “configurable teammate” theme from VS Code and helps teams using multiple IDEs get more consistent behavior. Customization is also shifting toward files: Copilot can discover AGENTS.md and CLAUDE.md in the workspace (or apply them globally), and a new `/memory` command jumps to settings for those instruction sources. With Copilot Memory now default for Pro/Pro+, the direction is clear: manage agent behavior with explicit, reviewable instruction artifacts instead of relying on transient chat context.

In preview, JetBrains adds Agent Hooks to run automation at lifecycle events (userPromptSubmitted, preToolUse, postToolUse, errorOccurred) via `.github/hooks/hooks.json`, enabling policy checks and integrations during agent execution. This continues last week’s VS Code lifecycle-hook work (auto-linting, output restrictions, safer automation) and brings similar tool-usage hook points to JetBrains in a repo-visible way. For MCP tool usage, Auto-Approve for MCP reduces repeated confirmations by letting you set auto-approval per MCP server/tool in JetBrains settings, but teams should treat it as a policy choice, especially given last week’s emphasis on terminal sandboxing and safety constraints.

JetBrains also has Auto Model Selection GA across all plans. Copilot can route completions among models (including GPT‑5.4, GPT‑5.3‑Codex, Sonnet 4.6, Haiku 4.5) based on availability/performance while respecting entitlements and admin policies, and it shows which model responded. This connects to last week’s model-management thread (explicit model selection for `@copilot` in PR comments, plus deprecations/migrations) by adding policy-bounded routing with transparency. Billing adds a detail: auto-routed requests get a 10% discount on the model’s premium multiplier (and currently routes only to 0x–1x multiplier models).

VS Code’s Copilot surface area evolved in parallel. Insiders 1.111 aligns instruction handling by recursively discovering `*.instructions.md` under `.github/instructions/`, which reduces “why was my instruction ignored?” differences across CLI, agents, and editor. This extends last week’s “shared memory/context” theme toward a clearer repository convention. Customization is also easier to debug and more modular: custom agent frontmatter supports agent-scoped chat hooks, and `/troubleshoot` helps inspect hook/customization behavior from chat. After last week’s conversation forking, live steering, and memory controls, the emphasis this week is diagnosing how hooks, instructions, and tool policies interact.

Insiders also added OpenTelemetry instrumentation for Copilot Chat (subject to telemetry settings), giving teams traces to diagnose latency, failures, or regressions during controlled rollouts. This complements last week’s enterprise management angle (session filtering, network routing, usage metrics) by expanding visibility from “what users did” to “what happened in the agent loop, and why.”

VS Code 1.111 (and discussion of the weekly stable cadence) also put more focus on autonomy modes: default approvals, bypass approvals, and an autopilot mode where Copilot continues end-to-end with auto-approved tool calls and retries. This builds on last week’s autonomy trend (more capable agents, terminal sandboxing, output restrictions) while making trade-offs more explicit: fewer interruptions for multi-step tasks, but a different security posture on developer machines. The same release also expanded hook control (scoping hooks to custom agents behind `chat.useCustomAgentHooks`) and improved debugging with “debug events snapshot” attachments you can carry into a new chat (including references like `#debugEventsSnapshot`) to diagnose issues such as unsupported model config values.

- [Major Agentic Capabilities Improvements in GitHub Copilot for JetBrains IDEs](https://github.blog/changelog/2026-03-11-major-agentic-capabilities-improvements-in-github-copilot-for-jetbrains-ides)
- [GitHub Copilot Auto Model Selection Launches for JetBrains IDEs](https://github.blog/changelog/2026-03-12-copilot-auto-model-selection-is-generally-available-in-jetbrains-ides)
- [What's New in Visual Studio Code Insiders 1.111](https://code.visualstudio.com/updates/v1_111)
- [Visual Studio Code and GitHub Copilot - What's new in 1.111](https://www.youtube.com/watch?v=Z-psHv_W5Yc)
- [Microsoft Accelerates VS Code Releases, Introduces Autopilot AI Mode](https://www.devclass.com/development/2026/03/12/microsoft-ships-vs-code-weekly-adds-autopilot-mode-so-ai-can-wreak-havoc-without-bothering-you/5208978)

### GitHub Copilot CLI and PR workflows: multi-agent parallelism and terminal-first review

Copilot’s terminal workflows kept reducing the “do it without leaving the shell” gaps. After last week’s Copilot CLI GA push (plus tutorials, metrics, dashboards), this week connects terminal work more directly to PR review, including a faster way to request agentic review. GitHub CLI v2.88.0 can request Copilot Code Review from the command line by adding Copilot as a reviewer (`gh pr edit --add-reviewer @copilot`) or selecting it during interactive PR creation. With last week’s focus on Copilot Code Review’s agent architecture and scale, this is a simple way to trigger the same review automation without switching to the web UI.

In the same release, reviewer/assignee selection moved to search-as-you-type instead of preloading large collaborator lists, which improves performance in large orgs and improves accessibility (screen readers avoid massive lists). It also supports the enterprise adoption theme from last week: as Copilot expands, supporting tooling is adjusting to large repositories and workflows.

On the Copilot CLI side, the new `/fleet` command goes beyond single-agent help by running multiple Copilot-powered tasks in parallel from one prompt, targeting maintenance work like dependency upgrades, documentation updates, and batching issues. This builds on last week’s “agent workflows beyond the IDE” direction (CLI automation, repo bots, MCP servers) by moving from sequential help to parallel orchestration, then returning combined output for review before merging.

For onboarding, beginner tutorials covered setup (npm install, auth, folder permissions) and two prompting styles: interactive mode for iterative sessions versus non-interactive `-p` for quick one-offs. This continues last week’s hands-on learning angle and reinforces that terminal workflows are a first-class Copilot surface, not just an IDE add-on.

- [Request Copilot Code Review Directly from GitHub CLI](https://github.blog/changelog/2026-03-11-request-copilot-code-review-from-github-cli)
- [Accelerate Issue Resolution with GitHub Copilot CLI Fleet Command](https://www.youtube.com/shorts/lHtQPImRJSc)
- [How to Use the /fleet Command in GitHub Copilot CLI](https://www.youtube.com/watch?v=BviR6Me36gI)
- [Getting Started with GitHub Copilot CLI: Beginner's Tutorial](https://www.youtube.com/watch?v=BDxRhhs36ns)
- [Interactive vs Non-Interactive Modes in GitHub Copilot CLI](https://www.youtube.com/watch?v=bdIJkGr2NV0)
- [Streamlining Dependency Updates with GitHub Copilot CLI](https://www.youtube.com/shorts/1npISWbvle4)
- [Learn to use an AI agent in your terminal | Copilot CLI for beginners](https://www.youtube.com/shorts/yXZVNfnAoCk)

### Modernization agents and migration integration: from repo changes to portfolio orchestration

Modernization came up repeatedly, with Copilot positioned less as “write code” and more as “upgrade estates.” This fits the progression from last week’s agentic repo automation and enterprise controls (session filtering, routing changes, user metrics): as Copilot automates across many repositories, teams need plans, governance, and standardized skills. GitHub Copilot’s modernization agent entered Public Preview with a portfolio workflow: run assessments, generate plans, and convert output into GitHub Issues and PRs so work stays in normal review/CI. It also emphasizes cross-repo coordination (async execution), centralized monitoring/governance via Agent HQ, and custom skills to enforce org rules across upgrades, especially for .NET and Java.

Microsoft also introduced `modernize-dotnet`, a Copilot coding agent with an “Assess → Plan → Execute” flow callable from Visual Studio (Solution Explorer), VS Code (modernization extension), Copilot CLI (marketplace plugin), and directly in GitHub repos. This continues last week’s “Copilot across surfaces” thread (IDE + web + CLI + MCP), but applied to long-running, review-heavy work. The focus is on producing structured, versionable artifacts (assessment and plan) alongside code changes so teams can review intent, not only diffs. It covers many workloads (ASP.NET Core, Blazor, Azure Functions, WPF, libraries, console apps) and supports .NET Framework migrations (Windows required).

At the program level, Azure Copilot’s Migration Agent and GitHub Copilot are described as complementary. Azure Copilot handles discovery/assessment (including agentless VMware discovery and offline inventory via Azure Migrate Collector), produces 6R recommendations, wave plans, and landing-zone setup aligned to the Cloud Adoption Framework, then links developer modernization work via GitHub Copilot integration. The shared theme is tighter coupling between “what to modernize/migrate” and “make code/config changes,” reducing silos between infrastructure planning and repo work. It also extends last week’s point that agents are becoming workflow participants that need traceability and controls.

- [How GitHub Copilot Agents Enable Application Modernization at Scale](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-single-apps-to-scale-solutions-how-ai-agents-scale/ba-p/4500059)
- [Modernize .NET Projects Anywhere Using GitHub Copilot and modernize-dotnet](https://devblogs.microsoft.com/dotnet/modernize-dotnet-anywhere-with-ghcp/)
- [Scaling Modernization on Azure with Agentic AI Tools](https://azure.microsoft.com/en-us/blog/many-agents-one-team-scaling-modernization-on-azure/)
- [Accelerating Cloud Modernization with Azure Copilot Migration Agent and GitHub Copilot Integration](https://techcommunity.microsoft.com/t5/azure-migration-and/azure-copilot-migration-agent/ba-p/4501292)

### Security, governance, and “auto-approval” controls for agentic workflows

As Copilot shifts toward autonomy (auto-approve, autopilot, multi-agent execution), security guidance is becoming more specific. This follows last week’s lifecycle hooks, terminal sandboxing, output restrictions, enterprise session filters, and outbound routing changes: as agents gain permissions, teams need consistent guardrails across IDEs, Actions, and the CLI. GitHub’s deep dive on “GitHub Agentic Workflows” security (agents via GitHub Actions) describes a defense-in-depth model that treats the agent as untrusted and designs for prompt injection, privilege escalation, secret exfiltration, and unsafe repo changes. It uses isolation (Actions runner VMs, hardened containers), declarative permission scoping, firewalled egress, and “zero secrets for agents” (secrets outside agent runtime in isolated proxies/containers). Repository writes go through a “safe outputs” pipeline so changes can be buffered and vetted before applying, backed by policy controls (limits on PR/issue creation, moderation/sanitization) and logging/observability for audit and incident response.

GitHub also added a repository-level Actions control: admins can optionally skip the “Approve and run workflows” gate for workflows triggered by the Copilot coding agent. That speeds CI while Copilot iterates on PRs, but it moves the speed/safety tradeoff onto workflow hardening (token permissions, secret exposure, environment protections, and exfiltration risk under malicious inputs). It lines up with this week’s autonomy levers in IDEs (JetBrains MCP auto-approve, VS Code autopilot/bypass approvals) with an equivalent CI-side switch. It can work well when hardening is in place, and it can be risky when it is not.

- ['Security Architecture of GitHub Agentic Workflows: Deep Dive'](https://github.blog/ai-and-ml/generative-ai/under-the-hood-security-architecture-of-github-agentic-workflows/)
- [Optionally Skip Approval for GitHub Copilot Coding Agent Actions Workflows](https://github.blog/changelog/2026-03-13-optionally-skip-approval-for-copilot-coding-agent-actions-workflows)

### Other GitHub Copilot News

Copilot’s web experience added a repository-exploration preview that brings file tree browsing into Copilot Chat on GitHub.com. You can ask Copilot to open files, navigate the tree, and attach files as temporary references. To keep context, you can pin a file by converting it into a permanent reference from chat tokens or the file preview. This matches last week’s push for persistent context (Copilot Memory default-on) and faster repo exploration in IDE agents, and it gives the web UI more explicit, user-controlled context attachment and retention.

- [Explore a Repository Using GitHub Copilot on the Web](https://github.blog/changelog/2026-03-11-explore-a-repository-using-copilot-on-the-web)

Copilot extensibility continued shifting toward building agents into products and internal tools. The Copilot SDK post emphasizes execution loops (plan → act → recover → continue) instead of one-off prompts, using structured context plus first-class tools/skills, and using MCP to connect to internal APIs so workflows stay permissioned and observable, even outside IDEs in services, SaaS, or event-driven/serverless setups. This extends last week’s MCP momentum (including Figma MCP): MCP is increasingly the bridge to real systems without relying on prompt-only approaches.

- ['Beyond Prompts: Agentic AI Workflows with GitHub Copilot SDK'](https://github.blog/ai-and-ml/github-copilot/the-era-of-ai-as-text-is-over-execution-is-the-new-interface/)

In the skills ecosystem, Microsoft’s .NET team introduced `dotnet/skills` to package reusable agent skills using the Agent Skills specification. It targets marketplace distribution (Copilot CLI and VS Code Copilot) and includes an evaluation/validation loop comparing outputs with and without a skill before merging, with the goal of more reproducible behavior instead of ad-hoc prompting. This continues last week’s custom plugins and “build agents/skills from chat” thread, and it connects to this week’s modernization theme: standardized skills help keep many agents aligned with internal rules.

- ['Enhance Coding Agents with .NET Skills: Introducing dotnet/skills'](https://devblogs.microsoft.com/dotnet/extend-your-coding-agent-with-dotnet-skills/)

GitHub also shared a “Copilot + Actions” pattern for accessibility feedback: structured reports become consistently triaged issues using prompt files/versioned instructions, Actions orchestration, and GitHub Models API analysis, while keeping humans responsible for validation and decisions. The implementation details (templates, .github instructions, PR-reviewed prompts, re-runs) are a useful reference for operationalizing Copilot in issue workflows without losing governance. It also matches this week’s instruction files + hooks + observability direction and last week’s repository automation bot theme.

- ['Continuous AI for Accessibility: How GitHub Automates and Accelerates Inclusive Feedback'](https://github.blog/ai-and-ml/github-copilot/continuous-ai-for-accessibility-how-github-transforms-feedback-into-inclusion/)

The VS Code team published a playbook-style case study using Copilot (CLI + SDK) and GitHub Actions for issue triage, release notes, PR validation, and “Copilot-first” review (Copilot as first reviewer on every PR). They describe custom agents with Playwright MCP for UI flow validation and screenshot evaluation, plus running multiple agent tasks in parallel across worktrees and sessions. It reads like a field report tying together CLI-first workflows, agentic review at scale, MCP tools, and the need to standardize instructions and guardrails as agents take on more routine work.

- [How VS Code Team Uses AI and GitHub Copilot to Accelerate Development](https://code.visualstudio.com/blogs/2026/03/13/how-VS-Code-Builds-with-AI)

Design-to-code got another MCP example: Figma Dev Mode context into VS Code so Copilot can generate UI code from structured design data (not screenshots), and guidance to encode design-system rules as reusable skills for consistent iteration. This continues last week’s Figma MCP story, with more focus on structured metadata and reusable skills to keep output consistent across teams and repositories.

- ['Figma MCP for VS Code: AI-Driven Design-to-Code Collaboration'](https://www.youtube.com/watch?v=KCu3g2_xqzM)

A best-practices post argued “prompt less, context more”: select relevant code, add short intent when needed, and expand workspace context rather than writing longer prompts, especially for review/refactor work like readability, error handling, and validation. This pairs with last week’s persistent memory and this week’s expanded instruction discovery: as teams invest in durable context (memory, instructions, pinned references), they can rely less on elaborate prompting and more on repeatable, reviewable repository context.

- ['Prompt Less, Context More: How to Get Better Results with GitHub Copilot'](https://www.cooknwithcopilot.com/blog/prompt-less-context-more.html)

Copilot plan changes reached education users: GitHub announced a new Copilot Student plan for GitHub Education beneficiaries and noted an updated model lineup for student accounts (details expected via the linked community thread). With last week’s model rollouts (GPT‑5.4) and deprecations/migrations, it’s another reminder that model access is increasingly shaped by plan and policy across segments.

- ['Updates to GitHub Copilot for Students: New Student Plan Announced'](https://github.blog/changelog/2026-03-13-updates-to-github-copilot-for-students)

A demo showed Copilot applied to hardware-adjacent scripting: using a structured “servo movement vocabulary” file (servos.md) so Copilot generates constrained movement sequences for a Raspberry Pi robot dog greeting routine triggered by face detection. It’s the same pattern as instruction files and skills, applied to a non-traditional codebase: constrain the agent with explicit, reviewable context so outputs stay within safe/valid bounds.

- [Giving a robot dog a personality using GitHub Copilot](https://www.youtube.com/watch?v=rUxB9M69e_Y)

## AI

AI coverage kept coming back to a practical question: how do you move from “an LLM that chats” to systems that can operate safely, repeatably, and at scale. This continues last week’s thread on production-ready agent tooling (skills, orchestration, sandboxing, MCP/OpenTelemetry), but with more “run it like software” patterns: multi-agent composition, approval gates, context compaction, and the operational plumbing (deploy automation, debugging loops, telemetry/evaluation, data platforms) needed for real deployments.

### Microsoft Agent Framework: production patterns for multi-agent apps (Python + .NET)

Microsoft Agent Framework content leaned more toward engineering details this week. It builds on last week’s reusable skills, orchestration patterns, and secure execution by showing how teams assemble multi-agent apps that can ship and run reliably.

One guide turns incident response into a multi-agent workflow by splitting “on-call copilot” into four narrow agents - Triage, Summary, Comms, and PIR - with strict JSON schemas so outputs can feed automation (tickets, updates, runbooks) without brittle parsing. The orchestrator uses `ConcurrentBuilder` and `asyncio.gather()` to run agents in parallel, replacing one large prompt with lower latency and more predictable structure. Deployment is set up for production use: a containerized Python orchestrator as a Foundry Hosted Agent, with model choice delegated to Azure OpenAI Model Router (one deployment like `model-router` routing between `gpt-4o` and `gpt-4o-mini`). Auth uses `DefaultAzureCredential` with the `https://cognitiveservices.azure.com/.default` scope (local via `az login`, prod via managed identity) so teams do not have to distribute API keys. It reads as a direct follow-on to last week’s guidance on boundaries and orchestration.

On the .NET side, the “Interview Coach” architecture uses the same multi-agent approach: receptionist/triage, behavioral interviewer, technical interviewer, and summarizer with explicit handoffs. It uses Agent Framework patterns (DI, type safety, OpenTelemetry), Microsoft Foundry as a governed model gateway (single endpoint, centralized identity/governance/moderation like PII detection), and external capabilities via MCP tool servers. The design is deliberately polyglot (for example, Python MarkItDown used by a .NET agent), which extends last week’s open-standards/MCP SDK coverage by showing cross-language tool servers in practice. .NET Aspire provides orchestration, service discovery, health checks, and a traces/health dashboard, with an end-to-end path from local `aspire run` to cloud `azd up`.

As agent apps start executing actions, the agent harness patterns post fills in safety and operability details that were previewed last week (dynamic sessions, secure execution). The theme is consistent: expose shell/filesystem tools with explicit approvals, move execution into hosted/container sandboxes, and keep long sessions from expanding tokens and latency. Python shows an approval-gated shell tool around `subprocess.run(...)` (timeouts, stdout/stderr capture), while .NET uses approval-required wrappers (for example, `ApprovalRequiredAIFunction`). For context compaction, Python shows sliding-window retention, while .NET combines strategies (tool result compaction + sliding windows + truncation) via `Microsoft.Agents.AI.Compaction` to tune responsiveness and cost. These are practical complements to last week’s “load skills only when needed” theme.

Python SDK Agent Skills updates also move skills closer to normal software development and extend last week’s skills SDK coverage. Skills can be defined in code (not only bundled files), resources can be dynamic via functions, scripts can be decorator-based in-process functions or file-based scripts via pluggable runners, and script execution can require human approval (`require_script_approval=True`). Across the posts, the pattern is consistent: multi-agent composition for clarity and latency, structured outputs for automation, tool execution behind approvals and sandboxing, and explicit strategies to keep long sessions reliable and affordable.

- [Building a Multi-Agent On-Call Copilot with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-multi-agent-on-call-copilot-with-microsoft-agent/ba-p/4499962)
- [Building an Interview Coach App with Microsoft Agent Framework, Foundry, MCP, and Aspire](https://devblogs.microsoft.com/blog/build-a-real-world-example-with-microsoft-agent-framework-microsoft-foundry-mcp-and-aspire)
- ['Agent Harness Patterns with Microsoft Agent Framework: Shell Execution and Context Compaction in Python and .NET'](https://devblogs.microsoft.com/agent-framework/agent-harness-in-agent-framework/)
- ['What’s New in Agent Skills: Code Skills, Script Execution, and Approval for Python'](https://devblogs.microsoft.com/agent-framework/whats-new-in-agent-skills-code-skills-script-execution-and-approval-for-python/)

### Azure automation and agent operations: Skills Plugin, SRE Agent “Deep Context,” and azd debugging

This week’s operational theme was making agents less advice-only and more able to execute real Azure work, while also improving how teams debug deployed agents. This continues last week’s secure execution and durable-tasks story (dynamic sessions, MCP SDKs), but with a day-two focus: environment context, tool-backed deployments, and CLI-first troubleshooting.

The Azure Skills Plugin is the clearest push in this area. It ships Azure skills (19+ guarded workflows), an Azure MCP Server with 200+ tools across 40+ services, and a Foundry MCP Server for model catalog/management/deployment. The goal is to turn prompts like “Deploy my Python Flask API to Azure” into a structured Prepare → Validate → Deploy flow: generate artifacts (for example, Dockerfiles), run preflight checks, generate/use IaC, then deploy via `azd`. It operationalizes last week’s reusable skills and tool discovery approach by shipping a ready-made Azure tool/skills surface. Requirements make it clear this is meant for execution: a compatible host (Copilot in VS Code, Copilot CLI, or Claude Code), Node.js 18+, `az`, `azd`, and an authenticated Azure account. Smoke tests include a guidance-only question and a live tool call (list resource groups) to confirm MCP servers and skills are active.

Azure SRE Agent also moved further from an incident assistant toward an operations agent that builds environment-specific expertise. Deep Context (described as available in GA) centers on continuous access to connected repositories and artifacts (auto-cloned/indexed), persistent memory across sessions (including capture via `#remember`), and background intelligence that discovers log schemas/KQL tables and generates reusable query templates. This extends last week’s boundaries theme: rather than stuffing context into prompts, the agent maintains a governed workspace and pulls evidence into the conversation when needed. The example (HTTP 5xx spike on a container app) shows the intent: start incidents with recent code/config and history already ingested. Another post describes “autonomous investigation” using a real cache-hit alert: parallel subagents tested hypotheses, filesystem workflows (`grep`, `find`, shell, reading files) tied telemetry to exact code versions, and the result was PR-shaped remediation (exclude uncacheable requests from alerting logic; restore prompt-prefix stability affecting caching). Across both, the pattern is consistent: treat the agent like a developer in a repository, layer context intentionally, keep evidence out of prompts until needed, and route changes through PR/CI gates.

To support hosted-agent operations, `azd` added debugging via the `azure.ai.agents` extension. `azd ai agent show` reports container status/health/replicas/errors, and `azd ai agent monitor` streams container logs, keeping troubleshooting in one CLI loop instead of bouncing between portals. This complements last week’s traceability focus (OpenTelemetry/OAuth): once agents are services, a status/log loop and consistent identity become part of basic supportability. Version details are explicit: `azure.ai.agents` `v0.1.12-preview`, included with `azd` `1.23.7+`, plus upgrade (`azd extension upgrade azure.ai.agents`) and bootstrap (`azd ai agent init`).

- ['Announcing the Azure Skills Plugin: AI Skills and Automation for Azure Deployment'](https://devblogs.microsoft.com/all-things-azure/announcing-the-azure-skills-plugin/)
- [How Azure SRE Agent’s Deep Context Builds Operational Expertise](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-now-builds-expertise-like-your-best-engineer/ba-p/4500754)
- ['The Agent that Investigates Itself: How Azure SRE Agent Enables Autonomous Incident Resolution'](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/the-agent-that-investigates-itself/ba-p/4500073)
- [Debugging Hosted AI Agents with Azure Developer CLI (azd)](https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-logs-status/)

### Microsoft Foundry and Microsoft Fabric: model deployment choices and production telemetry/evaluation loops

Platform coverage connected two parts of production AI work: model deployment with controls, and telemetry/data for evaluating and governing what agent apps actually do. This extends last week’s Foundry theme (models + agent features + stable SDKs) by adding open-model deployment options, while Fabric positions observability and accountability as an end-to-end data plane.

Microsoft Foundry added a public preview integration with Fireworks AI for open-model inference hosted on Azure but managed through Foundry’s control plane. Teams can browse the catalog, evaluate models, deploy endpoints, monitor usage/quality, and apply governance without wiring together separate tools. Deployment supports serverless pay-per-token (“Data Zone Standard”) and provisioned throughput units (PTUs). It also adds BYOW (Bring Your Own Weights): upload/register custom (quantized or fine-tuned) weights and serve them through the same workflow. This extends last week’s “single control plane + stable SDKs” message to teams mixing frontier models and open weights. The post cites catalog models (for example, DeepSeek V3.2, OpenAI gpt-oss-120b, Kimi K2.5, MiniMax M2.5), signaling a consistent “try → deploy → govern” flow even as the open-model set changes.

Microsoft Fabric’s agentic guidance focuses on observability and operations. One post frames Fabric as the operational data plane for agents: land structured telemetry into a governed OneLake workspace so teams can monitor routing, tool calls, latency, safety blocks, and failures in near real time (Eventstream → Eventhouse with KQL), and also do historical/business correlation (Lakehouse + semantic model + Power BI). It builds on last week’s best practices (boundaries, compliance, observability) by describing what to emit and where to store it. A reference implementation (Agentic Banking App: React + Python/LangGraph) demonstrates the telemetry pipeline, and the quality loop uses notebooks plus Azure AI Evaluation SDK by reusing captured telemetry instead of rebuilding ad-hoc datasets.

Fabric also strengthened the link between business semantics and automation. Ontology Rules integrate with Fabric Activator so teams define real-time conditions/actions using Ontology entities/properties (Customer, Order, Device) rather than raw tables or stream-specific logic. The cold-chain example (“Freezer temperature exceeds safe limits for sustained period → trigger alert”) shows the goal: define thresholds in a governed semantic layer so analytics, agents, and automations reuse consistent definitions.

Fabric AI Functions added ExtractLabel for schema-driven extraction of structured fields from unstructured text in pandas and PySpark. The key is enforcing an explicit output contract (JSON Schema or Pydantic schema) with required/optional fields, enums, nested structures, and `additionalProperties=False` to prevent extra keys, making outputs predictable for downstream validation and pipelines. This mirrors the structured-output discipline in Agent Framework workflows: reliable machine-consumable AI outputs reduce brittle parsing. It also works in distributed PySpark via `synapse.ml.spark.aifunc`, supporting LLM extraction at data-engineering scale.

- [Fireworks AI Now Available in Microsoft Foundry for High-Performance Open Model Inference on Azure](https://azure.microsoft.com/en-us/blog/introducing-fireworks-ai-on-microsoft-foundry-bringing-high-performance-low-latency-open-model-inference-to-azure/)
- [Operationalizing Agentic Applications with Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/operationalizing-agentic-applications-with-microsoft-fabric/)
- ['From Insight to Action: Bringing Fabric Activator into Ontology with Rules'](https://blog.fabric.microsoft.com/en-US/blog/from-insight-to-action-bringing-fabric-activator-into-ontology-with-rules/)
- ['ExtractLabel: Schema-driven Unstructured Data Extraction with Microsoft Fabric AI Functions'](https://blog.fabric.microsoft.com/en-US/blog/extractlabel-schema-driven-unstructured-data-extraction-with-fabric-ai-functions/)

### Other AI News

Microsoft Research introduced AgentRx, a framework concept for systematic debugging of multimodal agents by centralizing traces across modalities and adding verifier-style checks to isolate failures (input interpretation, action selection, intermediate decisions, output validation). With this week’s production debugging focus (azd logs/status, Aspire dashboards, OpenTelemetry), AgentRx reads like the research-side version of the same idea: as tools and modalities expand, agents need failure modes that teams can observe and debug.

- ['Systematic Debugging for AI Agents: Introducing the AgentRx Framework'](https://www.microsoft.com/en-us/research/blog/systematic-debugging-for-ai-agents-introducing-the-agentrx-framework/)

VS Code added chat forking: branch a conversation at any point, explore alternatives in parallel, and keep the original thread for comparison. This aligns with last week’s VS Code agent UX work (including forking) and reinforces that chat is becoming a workflow control surface, not only a single linear thread.

- [Forking Chat Sessions in Visual Studio Code](https://www.youtube.com/shorts/mpqrdghoj_Y)

Several higher-level pieces reinforced common constraints around autonomy and security in agentic systems. One describes an IT loop (observe → detect → analyze → act → learn) using Azure Monitor, Automation/runbooks, AKS self-healing, CI/CD hooks, and security tooling. Another breaks down Copilot agent design (goals, memory, tools, autonomy) with guardrails like least privilege and human approval. A “computer use agents” overview highlights risk when agents can operate software environments, which puts least-privilege identity and authorization design at the center. This echoes last week’s secure execution focus once agents move from recommend to act.

- ['Agentic AI in IT: Self-Healing Systems and Smart Incident Response in the Microsoft Ecosystem'](https://dellenny.com/agentic-ai-in-it-self-healing-systems-and-smart-incident-response-microsoft-ecosystem-perspective/)
- ['How Copilot Agents Think: Goals, Memory, Tools, and Autonomy'](https://dellenny.com/how-copilot-agents-think-goals-memory-tools-and-autonomy/)
- ['Building Computer Use Agents: Types, Functionality, and Security Risks'](https://www.youtube.com/watch?v=zr_DuUzFEd4)

Low-code agent building showed up via a cost-focused walkthrough: Copilot Studio with Azure SQL Database as system of record, including how to keep an entry-level deployment around ~$10/month by using free/low-cost options and careful SKU choices, then iterating agent behavior in Copilot Studio. It complements last week’s Copilot/Fabric coverage by grounding adoption in budgeting, SKU selection, and incremental rollout.

- [Building Low-Code AI Agents with Copilot Studio and Azure SQL Database for Under $10/Month](https://www.youtube.com/watch?v=FzoXP4P8OAY)

## Azure

Azure stories clustered around day-2 reality: better operational signal across container and hybrid estates, tighter compute security without breaking automation, and platform moves like leaving Heroku or running AI inference on-cluster. The framing across these posts is mostly about controlled transitions instead of all-at-once rewrites.

### SRE, incident response, and observability getting more “context-aware”

Building on last week’s platform engineering thread, Azure reliability tooling this week emphasized that AI-assisted operations only work when grounded in your environment. Azure SRE Agent’s onboarding flow reflects that. Provisioning the runtime (managed identity, Application Insights, Log Analytics) is straightforward, but the UI pushes you to attach context so answers are not generic. That includes connecting code (health probes, auth flows, history), logs with known schemas, incident sources, scoped Azure resources, and team knowledge files. The walkthrough is clear about outcomes: deployment-state summaries via Azure CLI plus logs, diagnosing 401s with app-specific checks, generating/triaging Azure Monitor incidents, finding RBAC/Log Analytics permission gaps, and classifying GitHub issues. It ends with a practical “done” definition: the agent can handle a live incident with app-specific, data-backed guidance. (It also notes SRE Agent GA as of March 10, 2026, with creation at sre.azure.com.)

Azure Managed Grafana 12 shipped investigation and access-control improvements. The main change is current-user Entra auth for supported Azure data sources (Azure Monitor, Azure Data Explorer, Azure Monitor Managed Service for Prometheus), so queries can run under the signed-in user’s permissions instead of a shared identity. This supports least privilege and auditing while still supporting Managed Identity and Service Principal for automation. Logs exploration also improves with a faster Explore flow and a query builder for iterating Azure Monitor Logs queries without writing KQL from scratch, plus a higher record limit (up to 30,000 per query). Metrics querying improves for Prometheus + OpenTelemetry users with better OTLP/histogram support and an OTel mode to reduce difficult label joins.

- [What It Takes to Give SRE Agent a Useful Starting Point](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-it-takes-to-give-sre-agent-a-useful-starting-point/ba-p/4500343)
- ['Azure Managed Grafana 12: New Authentication, Faster Log Analysis, Enhanced Monitoring'](https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-azure-managed-grafana-12/ba-p/4500673)

### Cloud-native operations: isolated AKS inference and clearer container supply signals

Following last week’s hybrid/disconnected focus (Arc Gateway for Kubernetes GA, sovereign-cloud governance training), teams running AI inference under strict network controls got a blueprint for air-gapped AKS. The constraint is straightforward: with no egress (no public LBs, NAT, public IPs on node subnets, and subnet design blocking default outbound), common LLM images fail because they download weights at startup. The guide offers two patterns: build “fat” images with weights baked in and push to private ACR (example uses `az acr build` and `HF_TOKEN`), or pre-download artifacts outside the cluster and expose them via private storage (for example, Azure Files over NFS) mounted into vLLM/NIM pods. It also highlights ACR artifact cache to stage upstream images so deployments do not depend on public registries. For GPUs, it points to a managed GPU node pool preview (prereqs preinstalled and lifecycle-managed) as an operations simplifier, with NVIDIA GPU Operator air-gapped mode as the alternative for tighter version control. Validation stays operational: internal service IPs, calling an OpenAI-compatible `/v1/chat/completions` endpoint from inside the private network, and verifying the model identifier matches baked/mounted artifacts.

Azure Container Registry also added proactive health monitoring to help separate “our pipeline broke” from “the registry is degraded.” ACR now runs SLI-based auto-detection across auth/push/pull and emits Azure Service Health events when a region is degraded. Events include tracking IDs, impacted regions/resources, and mitigation status (automated remediation vs engineer response). The practical benefit is faster correlation when CI/CD fails or Kubernetes pulls time out, and it integrates with standard paging via Service Health alert rules/action groups (PagerDuty/Opsgenie/ServiceNow/webhooks), with ARM/Bicep guidance for standardizing alerting.

Microsoft’s KubeCon Europe 2026 lineup post is not a product change, but it shows where Azure’s cloud-native guidance is trending: AI agents for platform operations (HolmesGPT), shared GPU inference scheduling (including Kueue), multi-cloud inference patterns, and staples like Istio/networking, OpenTelemetry, supply chain tooling (Notary/ORAS/Ratify), confidential containers, Terraform operations, and fleet management.

- [Deploying AI Inferencing in Isolated Azure Kubernetes Service (AKS) Clusters](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/ai-inferencing-in-air-gapped-environments/ba-p/4498594)
- [Proactive Health Monitoring and Auto-Communication Now Available for Azure Container Registry](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/proactive-health-monitoring-and-auto-communication-now-available/ba-p/4501378)
- ['Microsoft Azure at KubeCon Europe 2026: Sessions, Demos, and AI Innovations'](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/microsoft-azure-at-kubecon-europe-2026-amsterdam-nl-march-23-26/ba-p/4500716)

### Compute security and contracts: Confidential VM workflows, plus an API response change to watch

This continues last week’s compliance/governance thread (sovereign/hybrid controls, policy-as-code thinking), but with a compute-primitives focus: repeatable Confidential VM workflows, plus a small API contract change that can still break automation.

The Confidential VM custom image workflow shows how to build a hardened Windows golden image once, publish via Azure Compute Gallery for versioned/cross-region rollout, then choose disk encryption posture at deployment time. One image can serve PMK and CMK because encryption is applied via OS disk configuration (`SecurityEncryptionType`) and, for CMK, a Disk Encryption Set wired to Key Vault/Managed HSM. It also calls out a pipeline constraint: to publish an image supporting Confidential security types, you must use a Source VHD, so you export a generalized OS disk to a storage-account VHD before creating an image version. The guide covers common time sinks like Sysprep failures due to BitLocker (decrypt and retry) and includes PowerShell for `-SecurityType "ConfidentialVM"`, Secure Boot, vTPM, and confidential OS encryption on Gen2 Windows Server 2022 Azure Edition images.

Azure Migrate guidance extends this into a private, governed runbook: private endpoints for Azure Migrate and staging storage, Private DNS zones, ExpressRoute/S2S VPN, Disk Encryption Sets with CMK, and attestation-gated key release via Managed HSM so keys release only after the VM proves it booted in an expected confidential state. Planning details include supported SKUs across AMD SEV-SNP and Intel TDX (for example, DCasv6/ECasv6 and DCesv6/ECesv6), Gen2 + UEFI + Secure Boot + vTPM requirements, disk compatibility constraints (including a noted <=128 GB OS disk limit for full confidential disk encryption support, with workarounds), and OS support caveats. The runbook is structured as nine phases from appliance setup through test migration, cutover, and post-migration policy enforcement, with day-2 governance via Azure Policy, Azure Monitor, and Defender for Cloud (optionally Sentinel).

Azure Compute also flagged a REST API contract change: with `api-version=2025-11-01`, VM/VMSS responses will always return a non-null `properties.securityProfile.securityType`. If you omit or send null on create/update, responses return `"Standard"`. Explicit `"TrustedLaunch"` and `"ConfidentialVM"` stay the same. Provisioning behavior does not change, but tooling that treats `null` as distinct must update to treat `"Standard"` as the default for the new API version.

- [Building Reusable Custom Images for Azure Confidential VMs Using Azure Compute Gallery](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-reusable-custom-images-for-azure-confidential-vms-using/ba-p/4500880)
- [Migrating On-prem Windows & Linux VMs to Azure Confidential Virtual Machines via Azure Migrate](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/migrating-on-prem-windows-linux-vms-to-azure-confidential/ba-p/4500898)
- ['Azure Compute API 2025-11-01: securityType Field Now Always Non-Null in VM Responses'](https://techcommunity.microsoft.com/t5/azure-compute-blog/upcoming-compute-api-change-always-return-non-null-securitytype/ba-p/4500387)

### App platform and API patterns: Heroku exits, and APIM as a BFF without another microservice

This week’s “controlled transition” framing echoes last week’s migration guides (container runtime shifts, ExpressRoute gateway migration): sequence changes so teams can keep shipping while platforms move. Heroku migration guidance continues to sharpen into a move-in-slices playbook. Azure App Service is framed as the closest landing zone for Heroku-style apps/APIs, with Azure Container Apps for teams ready for containers with serverless traits like scale-to-zero. It calls out continuity for common languages (.NET/Java/Node.js/Python) and Docker, then recommends incremental modernization: rehost first, then evolve toward microservices and event-driven patterns using Dapr and KEDA. For data, Azure Database for PostgreSQL is positioned as the natural step for Heroku Postgres users, aligning with last week’s Postgres-as-bridge trend. Delivery centers on GitHub integration and GitHub Actions, with operations via Azure Monitor and Azure SRE Agent, and AI feature delivery via Azure AI Foundry (Microsoft Foundry), including MCP tool-calling once apps stabilize.

A separate guide shows implementing the BFF / Curated API pattern directly in Azure API Management policies, avoiding a separate aggregation service when the main need is fan-out plus response shaping. The examples use `<wait for="all">` with parallel `<send-request>`, store results in variables, then build a combined payload with policy expressions (C#-style) and `JObject` (Newtonsoft.Json), returning via `<return-response>`. It also covers semantics (200 when all succeed; consider 206/207 for partial success) and a practical cutoff: if orchestration logic gets too complex or divergent, a coded BFF (Functions or Container Apps) is easier to maintain.

- [A Practical Path Forward for Heroku Customers with Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/a-practical-path-forward-for-heroku-customers-with-azure/ba-p/4501797)
- [Implementing the Backend-for-Frontend (BFF) Pattern with Azure API Management](https://techcommunity.microsoft.com/t5/microsoft-developer-community/implementing-the-backend-for-frontend-bff-curated-api-pattern/ba-p/4499880)

### Other Azure News

The MSSQL extension v1.40 for VS Code added workflow updates: Edit Data (grid edits in-editor), better object search for large schemas, DACPAC export/import for packaging and promoting schema, and flat file (.csv/.txt) import for quick loads. This reduces trips to SSMS/Azure Data Studio and supports the “move in slices” migration theme by making schema/data validation and repeatable promotion easier during platform transitions.

- ['Next-Level SQL in VS Code: Edit Data, DACPAC Import/Export, and New Features'](https://www.youtube.com/watch?v=JhyBSthgFys)

Storage cost/billing guidance covered what changed and how to avoid surprises. Standard HDD managed disks (S4/S6/S70/S80) now bill transactions using a 16 KiB I/O unit (with hourly caps and a per-I/O cap for large sequential I/O on S70/S80), so workloads using larger I/Os may see higher transaction counts. It also reiterates that Standard HDD OS disks retire in September 2028, so OS disk migration planning should be on the roadmap. This complements last week’s Managed Disks operations angle (instant access incremental snapshots) by making reliability and cost behavior easier to plan for.

- [Understanding the Standard HDD I/O Unit Size Update in Azure](https://techcommunity.microsoft.com/t5/azure-storage-blog/understanding-the-standard-hdd-i-o-unit-size-update-and-what-it/ba-p/4499128)
- [Stop Burning Money in Azure Storage](https://techcommunity.microsoft.com/t5/azure-architecture-blog/stop-burning-money-in-azure-storage/ba-p/4500208)

Hybrid/edge updates leaned into fleet visibility and incremental capabilities, continuing last week’s Arc Gateway GA and sovereign-governance labs. The Azure Local LENS Workbook provides an importable Azure Monitor Workbook JSON for fleet views across Azure Local estates, with drill-downs and trend/compliance perspectives. Azure Arc-enabled servers had a forum recap highlighting workbook reporting starting points (including a GitHub-hosted Software Assurance Benefits dashboard), a private preview for VM Applications for third-party app deployment/patching via Arc, and Windows Server 2016 ESU enablement via Arc with a call for portal feedback.

- ['Azure Local LENS Workbook: Fleet-Level Insights at a Glance'](https://www.thomasmaurer.ch/2026/03/azure-local-lens-workbook-fleet-level-insights-at-a-glance/)
- ['Azure Arc Server February 2026 Forum: Reporting, VM App Deployment, and ESU Announcements'](https://techcommunity.microsoft.com/t5/azure-arc-blog/azure-arc-server-feb-2026-forum-recap/ba-p/4501793)

Networking troubleshooting content offered a hybrid-migration reminder: guest OS persistent routes on an Azure VM do not replace Azure SDN routing. It’s a useful last-mile counterpart to last week’s ExpressRoute migration guidance and helps avoid silent routing surprises. The checklist focuses on effective routes (Network Watcher), subnet-level UDRs to enforce paths, and on-prem firewall ACL/NAT and return routes that include the new Azure address space, especially when the next hop is an on-prem device like a Cisco ASA behind S2S VPN.

- [Troubleshooting Persistent Routing from Azure VM to On-Premises via Site-to-Site VPN](https://techcommunity.microsoft.com/t5/azure-networking/azure-vm-persistent-route-setup/m-p/4502007#M773)

Developer onboarding resources were refreshed with a Visual Studio Dev Essentials walkthrough as a free program that aggregates tools and cloud offers in one dashboard. It highlights Visual Studio Community, VS Code, Azure Free Account credits/free services, Azure DevOps, and related downloads/training for quick learning and prototyping setup.

- ['Visual Studio Dev Essentials: Unlocking Free Tools and Cloud Credits for Developers'](https://devblogs.microsoft.com/visualstudio/visual-studio-dev-essentials-free-practical-tools-for-every-developer/)

## .NET

This week’s .NET updates split into new features to try and updates to apply. .NET 11 Preview 2 added runtime, observability, and web/data updates, while .NET 10/9/8 servicing focused on secure, stable builds plus an out-of-band macOS debugger hotfix for VS Code users.

### .NET 11 Preview 2: runtime, web, and data stack updates

.NET 11 Preview 2 includes updates that show up in applications and pipelines. Runtime work continues on async improvements (V2) plus JIT/VM changes like cached interface dispatch for interface-heavy hot paths. Libraries also get targeted performance improvements (for example, Matrix4x4.GetDeterminant is ~15% faster) and functional additions like generic GetTypeInfo support in System.Text.Json for source-gen and type-metadata scenarios.

SDK changes reduce day-to-day friction: smaller SDK installers on Linux/macOS (useful for development and smaller CI images), improved analyzers, and more warnings/targets to surface issues earlier. Language changes are light for C# and Visual Basic, while F# adds developer-focused updates like overload resolution caching, new preprocessor directives, and new collection functions.

ASP.NET Core and Blazor leaned into built-in platform features. Native OpenTelemetry tracing in ASP.NET Core reduces custom tracing setup, OpenAPI 3.2.0 support updates API description workflows, and there’s a new .NET Web Worker template. Blazor adds TempData for state that survives redirects/navigation. EF Core adds server-side translation for LINQ MaxBy/MinBy plus SQL Server features like DiskANN vector indexes and VECTOR_SEARCH(), along with full-text catalogs/indexes and JSON_CONTAINS().

UI and deployment edges also moved forward. .NET MAUI includes Map control and TypedBinding performance work, immutability annotations for Color/Font, and API consistency (notably VisualStateManager), while Windows Forms/WPF get reliability updates. Container teams also get smaller images (SDK images up to ~17% smaller), which improves pulls and CI throughput. Preview 2 is available via the .NET 11 Preview 2 SDK, Visual Studio 2026 Insiders on Windows, or VS Code with C# Dev Kit for early validation and tracking release notes.

- [.NET 11 Preview 2: New Features and Improvements Across Runtime, SDK, and Libraries](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-2/)

### Built-in Zstandard compression in .NET 11 and ASP.NET Core

.NET 11 adds built-in Zstandard (zstd) support. System.IO.Compression will include zstd alongside gzip/Deflate and Brotli, which removes the need for third-party wrappers when you want zstd’s compression profile and gives framework components a standard implementation to use.

ASP.NET Core also gains zstd as an out-of-the-box HTTP response compression encoding, so teams can evaluate it like gzip or Brotli when balancing payload size vs CPU. The session covers the new APIs for direct use (streams/files) and indirect use (middleware/framework) and includes benchmarks to help decide when zstd fits versus existing algorithms.

- [ASP.NET Community Standup: Zstandard Compression Comes to .NET 11](https://www.youtube.com/watch?v=JDhs-5wVTnw)

### .NET servicing: March security patches and a macOS VS Code debugger hotfix

March 2026 servicing shipped clear “update now” guidance for teams on multiple .NET versions. Patched releases are .NET 10.0.4, .NET 9.0.14, and .NET 8.0.25, with installers/binaries, MCR container images, Linux package instructions, and known-issues pages. Security includes three “.NET Security Feature Bypass” CVEs: CVE-2026-26130 (.NET 10/9/8), CVE-2026-26127 (.NET 10/9), and CVE-2026-26131 (.NET 10 only). The practical guidance is to update SDKs/runtimes on developer machines, build agents, deployed hosts, and base images, then validate against the known-issues lists. The roundup also links servicing-approved-issue queries for ASP.NET Core, EF Core, runtime, WPF, and more so you can find fixes that may affect your applications.

A second servicing item followed: .NET 10.0.5 out-of-band fixed a macOS-only debugger crash regression introduced in .NET 10.0.4 when debugging from VS Code, especially impacting Apple Silicon (ARM64). If affected, install the .NET 10.0.5 SDK, restart VS Code, and confirm with `dotnet --version`. If you are not on macOS or are not seeing the crash, the guidance says staying on 10.0.4 is fine because 10.0.5 targets this issue narrowly.

- [.NET and .NET Framework March 2026 Servicing Updates: Security and Release Roundup](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-march-2026-servicing-updates/)
- [.NET 10.0.5 Out-of-Band Release: Debugger Crash Fix for macOS with Visual Studio Code](https://devblogs.microsoft.com/dotnet/dotnet-10-0-5-oob-release-macos-debugger-fix/)

### Other .NET News

NetEscapades.EnumGenerators adjusted its package layout to better match how source generators are consumed. The split adds a metapackage for the default path, a Generators-only package for keeping the generator private (for example, `PrivateAssets="All"`), and a RuntimeDependencies package for option/serialization types that generated code may need. The change addresses missing-type issues when teams exclude runtime assets.

- [Splitting the NetEscapades.EnumGenerators Packages: Architecture and Stable Release Path](https://andrewlock.net/splitting-the-netescapades-enumgenerators-packages-the-road-to-a-stable-release/)

A MonoGame walkthrough provided an end-to-end first project on modern .NET (recommended .NET SDK 10), covering setup, templates, the edit/build/run loop, and 2D basics like sprite rendering and input handling. It also reinforces MonoGame’s cross-platform support on Windows and Linux using Visual Studio or VS Code.

- [Make your First Game with .NET and MonoGame](https://www.youtube.com/watch?v=y631qBfWk_I)

## DevOps

DevOps coverage split into two practical tracks: AI-assisted operations moving into governed production workflows, and keeping CI/CD platforms stable as providers add security controls, version requirements, and reliability fixes. Pipeline hygiene also improved with updates to dependency automation, issue metadata, and database/testing workflows that fit Git-based delivery. Building on last week’s reliability work (GitHub’s HA search architecture) and GitHub Issues/Projects workflow improvements, this week adds more “operate safely at scale” pieces: governance controls for agentic incident response, clearer runner compliance expectations, and more structured issue metadata to replace ad-hoc labels.

### Azure SRE Agent reaches GA—and adds production governance for agentic ops

Azure SRE Agent reached GA with an emphasis on making incident response automation easier to adopt and safer in real environments. GA highlights include a redesigned onboarding flow that collects needed context - source, telemetry/logs, incidents, Azure resources, knowledge files - so teams can set up end-to-end investigations without stitching steps together. With context attached, deep context and persistent memory retain operational history (incidents, deployments, known failure modes) so investigations become less prompt-driven and more proactive.

GA also emphasizes integrations and orchestration: ingestion and workflows via ICM, PagerDuty, and ServiceNow; RCA linking telemetry to code paths; and automation via MCP connectors and generic HTTP integrations. Extensibility remains central - custom Python scripts, skills/plugins, subagents, and a Plugin Marketplace - so teams can turn runbooks into repeatable actions. This matches last week’s microservices guidance around tracing and CI/CD: distributed systems benefit from repeatable investigation and remediation steps that do not rely on one on-call engineer’s memory.

Governance is the other GA pillar, and Agent Hooks guidance describes the production controls teams need before letting an agent execute changes. Hooks intercept runtime behavior (agent/org/thread scopes) to enforce policy-as-code guardrails. A Stop Hook can block vague output and require a retry unless the agent provides structured, evidence-backed diagnosis (for example, Root Cause, Evidence with real metric values, and remediation steps). A PostToolUse Hook can enforce allowlists (for example, allowing `az postgres flexible-server restart`) while blocking destructive commands (`DROP`, `rm -rf`). A Global Hook can log tool usage (turn, tool, success/failure) with optional enablement to manage volume. The PostgreSQL Flexible Server latency scenario ties it together: allow investigation via metrics/logs, but only permit remediation when evidence meets policy and actions match approved patterns.

- [Announcing General Availability for the Azure SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-for-the-azure-sre-agent/ba-p/4500682)
- [What's New in Azure SRE Agent: GA Release Highlights](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-in-azure-sre-agent-in-the-ga-release/ba-p/4500779)
- ['Azure SRE Agent Is Now Generally Available: New Features and Capabilities'](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-is-generally-available-what-s-new/ba-p/4500779)
- ['Agent Hooks: Production-Grade Governance for Azure SRE Agent'](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/agent-hooks-production-grade-governance-for-azure-sre-agent/ba-p/4500292)

### GitHub Actions and GitHub platform operations: runner compliance, richer OIDC claims, and reliability learnings

GitHub Actions updates focused on avoiding future breakage while giving teams time to adapt. GitHub paused enforcement of the minimum self-hosted runner version requirement (still v2.329.0) after previously targeting March 16, 2026. Older runners can still be registered/configured during the pause, which prevents immediate disruption for orgs still upgrading fleets or images. GitHub also says enforcement will return, so teams should still upgrade VMs, containers, autoscaling images, and provisioning automation to avoid sudden failures later. Like last week’s Actions Example Checker theme, it’s the same hygiene issue: keep tooling, images, and automation current so platform changes do not break pipelines.

GitHub also expanded OIDC workload identity in Actions by allowing repository custom properties to appear as token claims (prefixed `repo_property_`). This supports attribute-based access control: set repository metadata (environment, classification, cost center, compliance tier) at repo/org/enterprise level and let cloud trust policies use those claims instead of hardcoding repository names or duplicating workflow logic. A public preview settings page for configuring token claims via UI or API signals this is meant to be a managed governance surface, which aligns with last week’s admin-controlled policy direction.

GitHub’s reliability narrative continued with the February 2026 availability report and a remediation write-up on February/March incidents. They highlight failure modes CI/CD teams should plan for: backend policy changes affecting hosted runner lifecycles, cache/database scaling issues degrading auth and API automation, and failover gaps (for example, Redis failover leaving no writable primary). This complements last week’s GHE search reliability story: simplify topology, validate failover, and reduce hidden coupling. GitHub’s mitigations - cache segmentation, load protection/shedding, isolation, capacity audits, stronger failover validation, and continued Azure migration - map to customer practices: retries/backoff and idempotency around API calls, documented fallbacks for development environments, and hybrid CI approaches where self-hosted runners cover critical workloads when hosted capacity is impaired.

- ['GitHub Actions: Minimum Self-Hosted Runner Version Enforcement Paused'](https://github.blog/changelog/2026-03-13-self-hosted-runner-minimum-version-enforcement-paused)
- [Actions OIDC Tokens Now Support Repository Custom Properties](https://github.blog/changelog/2026-03-12-actions-oidc-tokens-now-support-repository-custom-properties)
- ['GitHub Availability Report: Service Outages and Performance Incidents in February 2026'](https://github.blog/news-insights/company-news/github-availability-report-february-2026/)
- [Addressing GitHub's Recent Availability and Reliability Incidents](https://github.blog/news-insights/company-news/addressing-githubs-recent-availability-issues-2/)

### Azure DevOps operations: urgent patching and a deadline to migrate Advanced Security automation

Azure DevOps updates were time-sensitive and focused on preventing access and security automation from breaking in active environments. For Azure DevOps Server, Microsoft released Patch 2 (March 13, 2026) to fix an issue where group memberships could be deactivated under certain conditions. This is an access-control problem that can cascade into repository permissions, pipelines, and service accounts. Guidance is specific: install Patch 2 if you installed before the re-published release (March 13, 2026). If you ran the mitigation script, Patch 2 completes remediation. If you did not, Patch 2 alone is enough. Admins can verify via the installer’s `CheckInstall` argument.

In Azure DevOps Services, Microsoft temporarily rolled back Sprint 269 restrictions so build service identities can again call Advanced Security APIs after the restriction broke automation. The rollback has a deadline: build identities keep access only until April 15, 2026, then restrictions return. The recommended fix is migrating automation to a service principal with “Advanced Security: Read alerts,” narrowly scoped. For licensing concerns, service principals that do not commit code will not consume an Advanced Security committer license. Sprint 272 is also expected to add status checks that gate PR merges based on high/critical alerts, which may replace custom “call API and decide” pipelines. This lines up with the GitHub trend from last week: governance and quality move into platform controls and merge gates, not only custom pipeline scripts.

- [March Patches for Azure DevOps Server](https://devblogs.microsoft.com/devops/march-patches-for-azure-devops-server-4/)
- ['Temporary Rollback: Build Identities Can Access Advanced Security APIs Again'](https://devblogs.microsoft.com/devops/temporary-rollback-build-identities-can-access-advanced-security-read-alerts-again/)

### Other DevOps News

GitHub released a new REST API version (“2026-03-10”), the first calendar-based version with breaking changes. Integration owners should review breaking changes, then opt in explicitly using `X-GitHub-Api-Version: 2026-03-10` while validating response-shape assumptions and error handling. The default remains `2022-11-28` for at least 24 months if you do not set the header. GitHub also launched issue fields in public preview for select orgs: typed org-level metadata (up to 25 fields; single select/text/number/date) searchable across repositories, usable in Projects views, automatable via REST/GraphQL, and emitting webhooks (`field_added`, `field_removed`). If last week’s Projects features structured boards, issue fields structure the issue data itself for consistent queries and automation across repositories.

- [GitHub REST API Version 2026-03-10 Now Available](https://github.blog/changelog/2026-03-12-rest-api-version-2026-03-10-is-now-available)
- ['GitHub Issue Fields Public Preview: Structured Metadata for Issues'](https://github.blog/changelog/2026-03-12-issue-fields-structured-issue-metadata-is-in-public-preview)

Dependabot can now open PRs updating `.pre-commit-config.yaml` hook revisions when you set ecosystem `pre-commit` in `dependabot.yml`, supporting tag pins and commit SHA pins (preserving YAML formatting and skipping `local`/`meta` hooks). In JavaScript, an alpha “npmx” package browser launched to help evaluate npm packages (module format, install size, outdated dependency signals), which may help dependency due diligence despite being early.

- [Dependabot Now Supports Automatic Updates for pre-commit Hooks](https://github.blog/changelog/2026-03-10-dependabot-now-supports-pre-commit-hooks)
- [npmx Package Browser Released as Alpha to Improve npmjs Experience](https://www.devclass.com/devops/2026/03/09/npmx-package-browser-released-as-alpha-to-fix-pain-of-using-npmjs/4093605)

Microsoft Fabric added publishing SQL database schema changes from VS Code via SQL Database Projects, including a Publish dialog that browses Fabric workspaces/databases, previews the deployment script, and exposes options (including deletion behavior). It also adds templates for common objects and optional validation using a local SQL Server 2025 container. A Harness tutorial showed building a CI pipeline on AKS using Delegates and Connectors, with Secrets Manager (optionally Azure Key Vault-backed) storing service principal creds so Azure access stays within a governed connector and network boundary.

- [Deploy SQL Databases in Microsoft Fabric Directly from VS Code](https://blog.fabric.microsoft.com/en-US/blog/deploy-sql-databases-in-fabric-from-vs-code-no-more-context-switching/)
- [How to Create a Harness Pipeline and Integrate with Azure](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/how-to-create-a-harness-pipeline-and-integrate-with-azure/ba-p/4499862)

A Behave tutorial showed structuring Python BDD suites in VS Code (feature files, steps, `environment.py` hooks, `behave.ini`, tagging) in a way that maps cleanly to CI. A platform engineering essay argued “human scale” coordination is the constraint. Tool sprawl across Kubernetes, observability, and CI/CD creates overhead, and the post recommends evolving platform interfaces while using AI assistants to surface institutional knowledge without building overly rigid platforms. It also connects to this week’s agent governance theme: adoption tends to hinge less on capability and more on standard interfaces, guardrails, and shared context for safe collaboration across complex stacks.

- ['Getting Started with Behave: Writing Cucumber Tests in VS Code'](https://techcommunity.microsoft.com/t5/microsoft-developer-community/getting-started-with-behave-writing-cucumber-tests-in-vs-code/ba-p/4496865)
- [The Human Scale Problem in Platform Engineering](https://devblogs.microsoft.com/all-things-azure/the-human-scale-problem-in-platform-engineering/)

## Security

Security coverage followed a consistent theme: trusted developer surfaces are being tightened while also being actively abused. After last week’s authentication weaknesses (OAuth redirection abuse, AiTM phishing) and supply-chain controls (Dependabot workflow improvements, AI-assisted vulnerability discovery), this week shows convergence on default surfaces. Identity is moving closer to the data plane (even SFTP), GitHub scanning is shifting earlier in workflows, and attackers are blending into routine engineering habits (interview repositories, “VPN download” searches). AI security also continued the shift noted last week from theory to operations, with more guidance on monitoring, audit, and governance as agentic tools land in enterprises.

### Identity-centered secure access for file transfer (Azure Blob Storage SFTP + Entra ID)

After last week’s identity abuse coverage, this is the defensive counterpart: reduce identity islands by bringing legacy access under central policy. Azure Blob Storage SFTP is adding public preview support for Microsoft Entra ID authentication, replacing local storage-account SFTP users that required separate identities, passwords/keys, and offboarding. Users (including B2B guests via Entra External Identities) authenticate with Entra ID and receive a short-lived SSH certificate per session, which reduces reliance on long-lived secrets and aligns SFTP with enterprise identity controls.

Authorization and operations change with this model. Access is enforced via Azure RBAC/ABAC plus POSIX-like ACLs for path permissions, unifying access semantics across SFTP, REST, and Azure CLI. MFA, Conditional Access, Identity Protection, and PIM apply directly to SFTP integrations, and offboarding becomes disabling the Entra identity or revoking access rather than rotating keys and removing local users. For regulated partner/vendor exchange, this supports time-bound, policy-driven access with centralized auditing. Guidance is to enroll and validate in non-production. Local users still exist, but the direction is toward Entra-backed access as the default model.

- [Enterprise Identity Meets Secure File Transfer: Entra ID Public Preview for Azure Blob Storage SFTP](https://techcommunity.microsoft.com/t5/azure-storage-blog/enterprise-identity-meets-secure-file-transfer-entra-id-public/ba-p/4501937)

### GitHub Advanced Security updates: sharper code scanning and stronger leak prevention

Continuing last week’s shift-left and supply-chain thread, GitHub’s updates focused on improving what analysis can model and preventing secrets from reaching repositories.

CodeQL 2.24.3 expands coverage and modeling. Java/Kotlin scanning supports Java 26 and improves Maven builds by reading Java version from POMs (and trying Java 17+ when needed), reducing toolchain mismatch failures. Modeling also improves: better detection of MobX `observer` React components for JS/TS, improved Python SSRF analysis via a new AntiSSRF sanitization barrier model, and better interpretation of boolean guards like `isSafe(x) == true` / `!= false` to reduce false positives. Ruby taint tracking now follows `Shellwords.escape` / `shellescape` (with exceptions for command-injection queries), Rust adds neutral models to ease custom sources/sinks/summaries, C/C++ refines a query to reduce false positives, and C# parsing supports the `field` keyword (C# 14). Since CodeQL rolls out automatically on GitHub.com, teams should expect alert shifts and plan for scan re-runs and review of custom packs/models.

GitHub’s March 2026 secret scanning update adds 28 detectors across 15 providers and expands default push protection so more leaks are blocked at `git push` time. This ties to last week’s structured ownership/triage theme: earlier blocking reduces rotation and incident coordination later. New Azure/Entra patterns for `azure_active_directory_application_id` and `azure_active_directory_application_secret` catch common app artifacts before commit. Validators also expand (Airtable, DeepSeek, npm, Pinecone, Sentry), helping triage by confirming active credentials vs noise. Overall, it pushes security earlier into the developer loop: fewer scan failures, fewer false positives, and more secrets stopped before they reach the remote.

- [CodeQL 2.24.3 Release: Java 26 Support and Enhanced Static Analysis](https://github.blog/changelog/2026-03-10-codeql-2-24-3-adds-java-26-support-and-other-improvements)
- [GitHub Secret Scanning Pattern Updates — March 2026](https://github.blog/changelog/2026-03-10-secret-scanning-pattern-updates-march-2026)

### Threat research: attackers blending into developer workflows (interviews, GitHub assets, and signed installers)

Last week highlighted identity abuse and stolen EV cert signing. This week extends the same theme into common distribution and execution paths that developers and IT already treat as routine. These campaigns do not need exotic exploits if they can embed in everyday workflows.

Microsoft’s “Contagious Interview” report shows recruitment social engineering wrapped around normal coding tasks. Targets get assignments that involve cloning a repository and installing/running dependencies (often npm) from platforms like GitHub/GitLab/Bitbucket. VS Code workspace trust is a key hinge: once trusted, task files can run background commands, which bridges “open repo” to “execute code.” The campaign uses multiple payload families (Invisible Ferret, FlexibleFerret, BeaverTail/OtterCookie) for backdoors, persistence, and theft, targeting source, CI/CD tokens, cloud credentials, code-signing keys, password stores, and wallets. Guidance is operational: treat hiring pipelines as an attack surface, isolate interview environments from corporate credentials, add review/approval gates before running external code, and hunt for suspicious Node/Python behavior, download-and-execute patterns, and unusual outbound infrastructure.

Microsoft Threat Intelligence also described Storm-2561 using SEO poisoning so VPN-client searches land on lookalike domains serving trojanized installers, sometimes hosted as GitHub release assets to blend into a trusted channel. The chain uses a ZIP with an MSI, a signed executable, and malicious DLL side-loading (for example, `dwmapi.dll`, `inspector.dll`) in legitimate-looking paths. The fake VPN captures credentials, steals VPN configs, persists via `RunOnce`, and uses an in-memory loader to deploy Hyrax infostealer. Defender mitigations include cloud protection, EDR in block mode, web/network protection, ASR rules, SmartScreen, and hunting for suspicious signed binaries (including the cited certificate subject) and unusual DLL loads in VPN directories. The takeaway is practical: tighten acquisition habits (especially search → download) and treat GitHub-hosted binaries as requiring provenance checks and reputation-aware controls, because “looks legitimate” signals are being used as cover.

- [‘Contagious Interview: Malware delivered through fake developer job interviews’](https://www.microsoft.com/en-us/security/blog/2026/03/11/contagious-interview-malware-delivered-through-fake-developer-job-interviews/)
- [Storm-2561 Distributes Fake VPN Clients via SEO Poisoning for Credential Theft](https://www.microsoft.com/en-us/security/blog/2026/03/12/storm-2561-uses-seo-poisoning-to-distribute-fake-vpn-clients-for-credential-theft/)

### Securing enterprise AI usage: prompt abuse playbooks and agent governance control planes

Last week covered attackers operationalizing AI and the need to protect AI usage (including malicious extensions harvesting chat histories). This week adds day-two defensive detail: detecting/responding to prompt abuse and how identity/governance may apply to agents in enterprise environments.

Microsoft Incident Response guidance focuses on indirect prompt injection as an operational risk. Hidden instructions embedded in ingested content (including URL fragments after `#`) can be pulled into prompts and silently steer outputs. It categorizes attacker patterns (direct overrides, extractive attempts, indirect injection) and maps detection/response to common Microsoft stacks: discover sanctioned vs shadow AI usage with Defender for Cloud Apps and Purview DSPM; monitor with Purview DLP and CloudAppEvents; constrain with Entra Conditional Access plus DLP; and investigate via Sentinel, Purview audit logs, and Entra identity signals. Engineering takeaways include explicit sanitization in ingestion pipelines (including stripping/normalizing URL fragments) and treating AI tool usage as observable and policy-governed, like other data-handling systems.

Microsoft also introduced Agent 365 and Microsoft 365 E7: The Frontier Suite as a security/admin control plane for agentic AI, including third-party agents. It centers on an Agent Registry (inventory), observability reports, near real-time risk evaluation using Defender/Entra/Purview signals, and an “Agent ID” model where agents get Entra identities so Conditional Access and identity governance apply like they do to users. It also mentions inline DLP for Copilot Studio agents, sensitivity label inheritance, auditing/eDiscovery/retention for agent data, and Defender protections for prompt manipulation and model tampering. For developers, this implies more deployment requirements - registration, identity assignment, least privilege, label/DLP compliance, auditability - becoming part of “done.” (Availability is stated as May 1, 2026, with Agent 365 at $15/user/month and E7 at $99/user/month.)

- [Detecting and Analyzing Prompt Abuse in AI Tools](https://www.microsoft.com/en-us/security/blog/2026/03/12/detecting-analyzing-prompt-abuse-in-ai-tools/)
- [‘Securing Agentic AI with Microsoft Agent 365 and Microsoft 365 E7: The Frontier Suite’](https://www.microsoft.com/en-us/security/blog/2026/03/09/secure-agentic-ai-for-your-frontier-transformation/)

### Other Security News

Mark Russinovich described using Anthropic Claude Opus 4.6 to decompile and reason about 6502 machine code from an Apple II utility he wrote in 1986. The model flagged a silent incorrect-behavior bug (not checking the carry flag after a line-lookup routine). The takeaway is not that this specific tool is exploitable. It is that LLMs can increasingly help reason about binaries and legacy code (useful for firmware and long-lived systems), while still requiring disciplined verification to manage noise.

- [Microsoft Azure CTO Uses AI to Discover Vulnerabilities in Legacy Apple II Code](https://www.devclass.com/security/2026/03/11/microsoft-azure-cto-set-claude-on-his-1986-apple-ii-code-says-it-found-vulns/5208875)
