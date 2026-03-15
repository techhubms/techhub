---
external_url: /github-copilot/roundups/Weekly-AI-and-Tech-News-Roundup-2026-03-09
title: GitHub Copilot Updates, Durable AI Agents, and Secure Cloud Features
author: TechHub
primary_section: github-copilot
date: 2026-03-09 09:00:00 +00:00
feed_name: TechHub
tags:
- Agentic Workflows
- AI
- AI Agents
- Azure
- CLI Automation
- Cloud Migration
- Databricks
- DevOps
- Durable Agent Orchestration
- GitHub Copilot
- GPT 5.4
- Jira Integration
- Logic Apps
- Microsoft Foundry
- ML
- Model Management
- Modular Skills
- Multimodal AI
- Roundups
- Security
- TypeScript
- VS Code
- Workflow Automation
- .NET
section_names:
- ai
- github-copilot
- ml
- azure
- dotnet
- devops
- security
---
Welcome to this week's Tech Roundup. GitHub Copilot continues to broaden its features, with agent automation in VS Code, deeper CLI integration, and finer model management. The AI landscape now includes new agentic frameworks, standardized skill libraries, and orchestration tools for complex deployments. Azure remains central in enabling real-time, AI-powered solutions. Security and DevOps teams further reinforce automation and cloud-native practices, focusing on operational reliability and compliance. Let’s look at the updates influencing development workflows and cloud technology.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Copilot Agent Automation and Extensibility in Visual Studio Code](#copilot-agent-automation-and-extensibility-in-visual-studio-code)
  - [GitHub Copilot CLI: General Availability and Practical Tutorials](#github-copilot-cli-general-availability-and-practical-tutorials)
  - [GPT-5.4 Integration: New Coding Model for Copilot](#gpt-54-integration-new-coding-model-for-copilot)
  - [Copilot Code Review: Agentic Architecture and Workflow Evolution](#copilot-code-review-agentic-architecture-and-workflow-evolution)
  - [Copilot Memory: Persistent Context Now On by Default](#copilot-memory-persistent-context-now-on-by-default)
  - [Model Management: Selection and Deprecation](#model-management-selection-and-deprecation)
  - [Figma MCP Server Integration: Design-to-Code Workflows](#figma-mcp-server-integration-design-to-code-workflows)
  - [Agentic Workflows: Repository Automation and Open Source Bots](#agentic-workflows-repository-automation-and-open-source-bots)
  - [Jira Integration: Coding Agent for Issue-Driven PR Automation](#jira-integration-coding-agent-for-issue-driven-pr-automation)
  - [Copilot Usage Metrics: Expanded Telemetry, Plan Mode, and Username Consistency](#copilot-usage-metrics-expanded-telemetry-plan-mode-and-username-consistency)
  - [Agent Management and Network Configuration](#agent-management-and-network-configuration)
  - [Global Developer Events and Skill Expansion](#global-developer-events-and-skill-expansion)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Microsoft Foundry Ecosystem: Model, Agent, and SDK Advancements](#microsoft-foundry-ecosystem-model-agent-and-sdk-advancements)
  - [Modular Skills and Agent Frameworks: Agent Skills SDK, Reusable Skills, and Dynamic Tool Discovery](#modular-skills-and-agent-frameworks-agent-skills-sdk-reusable-skills-and-dynamic-tool-discovery)
  - [Secure Agent Execution and Durable Tasks: Azure Container Apps Dynamic Sessions, MCP C# SDK](#secure-agent-execution-and-durable-tasks-azure-container-apps-dynamic-sessions-mcp-c-sdk)
  - [Architectures and Best Practices: High-Performance Agentic Systems, Open Standards](#architectures-and-best-practices-high-performance-agentic-systems-open-standards)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [LLM Inference Optimization and Architecture on Azure](#llm-inference-optimization-and-architecture-on-azure)
  - [Multimodal and Vision Reasoning AI: Phi-4-Reasoning-Vision-15B](#multimodal-and-vision-reasoning-ai-phi-4-reasoning-vision-15b)
  - [.NET AI Agent Architecture and Enterprise Patterns](#net-ai-agent-architecture-and-enterprise-patterns)
  - [Other ML News](#other-ml-news)
- [Azure](#azure)
  - [Azure Databricks and Real-Time Data Platforms](#azure-databricks-and-real-time-data-platforms)
  - [Azure Logic Apps and Integration Modernization](#azure-logic-apps-and-integration-modernization)
  - [Hybrid and Sovereign Cloud Capabilities](#hybrid-and-sovereign-cloud-capabilities)
  - [Migration Guides for Azure Networking and Container Workloads](#migration-guides-for-azure-networking-and-container-workloads)
  - [Platform Engineering, AI-Driven Infrastructure, and Compliance](#platform-engineering-ai-driven-infrastructure-and-compliance)
  - [Location and Networking APIs](#location-and-networking-apis)
  - [Azure Managed Disks and Developer Productivity](#azure-managed-disks-and-developer-productivity)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [TypeScript 6.0 Release Candidate and Migration Path](#typescript-60-release-candidate-and-migration-path)
  - [Visual Studio Code 1.110: Agent-Driven Development and Insiders Update](#visual-studio-code-1110-agent-driven-development-and-insiders-update)
  - [.NET Data and Workflow Tooling](#net-data-and-workflow-tooling)
  - [.NET MAUI Platform Expansion and Blazor in .NET 11 Preview](#net-maui-platform-expansion-and-blazor-in-net-11-preview)
- [DevOps](#devops)
  - [GitHub Issues and Projects: Enhanced Features and Workflow Guides](#github-issues-and-projects-enhanced-features-and-workflow-guides)
  - [GitHub Platform and DevOps Tooling Improvements](#github-platform-and-devops-tooling-improvements)
  - [Microservice Architecture Patterns and Solutions](#microservice-architecture-patterns-and-solutions)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Microsoft Account and Authentication Attack Analysis](#microsoft-account-and-authentication-attack-analysis)
  - [Global Disruption of Advanced Phishing Kits and MFA Bypass Platforms](#global-disruption-of-advanced-phishing-kits-and-mfa-bypass-platforms)
  - [AI-Enabled Security Threats, Detection, and Response](#ai-enabled-security-threats-detection-and-response)
  - [Secure Software Supply Chain and User Controls](#secure-software-supply-chain-and-user-controls)
  - [Automation and AI-Driven Vulnerability Discovery](#automation-and-ai-driven-vulnerability-discovery)
  - [Secure Binary Signing with Azure Trusted Signing and dotnet sign](#secure-binary-signing-with-azure-trusted-signing-and-dotnet-sign)
  - [Other Security News](#other-security-news)

## GitHub Copilot

The Copilot section this week covers updates in AI tooling for developers, including improved agent functions in VS Code, Microsoft platform integrations, recent model rollouts, and broader workflow and analytics options. The new features reinforce Copilot's growing use in regular coding, CLI automation, and review tasks, supporting both organizational and individual needs.

### Copilot Agent Automation and Extensibility in Visual Studio Code

The February 2026 update to VS Code v1.110 includes automation and extensibility features for Copilot. Building on past agent automation and custom agent creation, it introduces lifecycle hooks for automated activity, like auto-linting and output restrictions that help maintain safe workflows. You can now fork conversations to explore different code paths within agent chats. Slash commands in chat streamline approvals and actions, and terminal sandboxing adds safety for prototyping. Live steering lets you shift task focus during execution. There's experimental support for custom third-party plugins. Developers can access browser tools, and build agents, prompts, or skills from chat. Integration with the CLI is more flexible, with improved diff views and folder syncing. Persistent memory helps agents retain context and information, making recall across sessions easier. Tools like Explore Subagent speed up codebase searching, and memory compaction gives manual control over session data. Predictive edits now use complete file context for more relevant suggestions. Terminal images and interface updates (including accessibility changes) lead to a more productive and collaborative environment in VS Code.

- [GitHub Copilot Agents and Extensions in Visual Studio Code February 2026 Release](https://github.blog/changelog/2026-03-06-github-copilot-in-visual-studio-code-v1-110-february-release)

### GitHub Copilot CLI: General Availability and Practical Tutorials

Copilot CLI is now available for all users, letting developers work with Copilot in their terminal. After last week’s general availability announcement, developers can access code suggestions, reviews, and simple automation workflows outside the IDE. CLI users benefit from diff navigation, trusted sync, and snippet-based actions. There are telemetry and user-metrics options for teams to track usage. A free eight-part open source course helps you set up CLI, build custom agents, and run your own MCP server with hands-on labs. User-level metrics help with resource planning. Official dashboards and how-to guides demonstrate how Copilot CLI can help in day-to-day command-line work.

- [GitHub Copilot CLI General Availability and New Repository Dashboard](/github-copilot/videos/github-copilot-cli-general-availability-and-new-repository-dashboard)
- [Get started with GitHub Copilot CLI: A free, hands-on course](https://devblogs.microsoft.com/blog/get-started-with-github-copilot-cli-a-free-hands-on-course)
- [Copilot Usage Metrics Expanded to User-Level GitHub Copilot CLI Activity](https://github.blog/changelog/2026-03-05-copilot-usage-metrics-now-includes-user-level-github-copilot-cli-activity)

### GPT-5.4 Integration: New Coding Model for Copilot

With the release of GPT-5.4, Copilot Pro, Pro+, Business, and Enterprise users in supported IDEs can now access better code generation and improved agent workflow support. It’s available on VS Code (v1.104.1+), Visual Studio (v17.14.19+), JetBrains (v1.5.66+), Xcode, Eclipse, the web, and in the CLI. These updates bring improvements to handling multi-step coding tasks and agent interactions. Enterprise admins must enable GPT-5.4, with guides provided for migration. Tutorials explain changes that improve reasoning and task management in VS Code Copilot.

- [GPT-5.4 is Generally Available in GitHub Copilot](https://github.blog/changelog/2026-03-05-gpt-5-4-is-generally-available-in-github-copilot)
- [GPT-5.4 Now Available in VS Code with GitHub Copilot](/github-copilot/videos/gpt-54-now-available-in-vs-code-with-github-copilot)

### Copilot Code Review: Agentic Architecture and Workflow Evolution

Copilot Code Review uses an agent-driven architecture to support more than 60 million automated reviews, now accounting for over 20% of all reviews on GitHub. Continuing last week's agent-oriented code review discussion, Copilot supplies detailed context, recognizes recurring code issues, and makes targeted suggestions on correctness and design. Batch review comments and automated multi-line fixes are designed to make feedback clearer. Self-hosted runner setup is needed for these features, while GitHub-hosted runners work automatically. Continuous improvements are based on telemetry and team reactions. Future plans include adaptive personalization and more interactive team review features.

- [Copilot Code Review Now Runs on Agentic Architecture](https://github.blog/changelog/2026-03-05-copilot-code-review-now-runs-on-an-agentic-architecture)
- [60 Million GitHub Copilot Code Reviews: Enhancing Developer Workflows with AI-Powered Review](https://github.blog/ai-and-ml/github-copilot/60-million-copilot-code-reviews-and-counting/)

### Copilot Memory: Persistent Context Now On by Default

For Pro and Pro+ users, Copilot Memory is now enabled by default. This feature stores repository-level conventions, patterns, and dependencies, refreshing every 28 days to keep the context relevant. This capability supports better code completion, reviewing, and CLI interaction. Users may opt out, and organization admins control settings. The documentation provides more about operation and updates, reflecting last week's move from opt-in to default for persistent memory.

- [Copilot Memory Now Enabled by Default for GitHub Copilot Pro and Pro+ Users](https://github.blog/changelog/2026-03-04-copilot-memory-now-on-by-default-for-pro-and-pro-users-in-public-preview)

### Model Management: Selection and Deprecation

Now, you can select specific Copilot AI models such as GPT-5.4 when you use @copilot in pull request comments. This lets you tailor model selection for each review, with the picker available on github.com, and plans to support GitHub Mobile. Gemini 3 Pro and GPT-5.1 are being deprecated; admins are advised to migrate to Gemini 3.1 Pro and GPT-5.3-Codex with official support to guide the transition.

- [Pick a Model for @copilot in Pull Request Comments](https://github.blog/changelog/2026-03-05-pick-a-model-for-copilot-in-pull-request-comments)
- [Deprecation Notice: Gemini 3 Pro and GPT-5.1 Models in GitHub Copilot](https://github.blog/changelog/2026-03-02-upcoming-deprecation-of-gemini-3-pro-and-gpt-5-1-models)

### Figma MCP Server Integration: Design-to-Code Workflows

Copilot now connects VS Code with the Figma MCP server, allowing developers to import UX/UI content for code or export components as editable frames in Figma. This helps bridge design and engineering tasks. Setup guides are available for continuous integration, with CLI support planned. These updates provide deeper automation across tools, picking up from last week's discussion on design-engineering workflows.

- [Figma MCP Server Integration with GitHub Copilot in VS Code](https://github.blog/changelog/2026-03-06-figma-mcp-server-can-now-generate-design-layers-from-vs-code)
- [VS Code Live: Code to Canvas with Figma MCP and GitHub Copilot](/github-copilot/videos/vs-code-live-code-to-canvas-with-figma-mcp-and-github-copilot)

### Agentic Workflows: Repository Automation and Open Source Bots

Copilot’s agentic workflows allow developers to automate repository tasks using markdown scripts. Tutorials cover safety precautions, scenario-based automation, and best practices for bots that manage issues or maintain projects. Home Assistant and similar open source projects use these workflows to reduce manual work and enable community support. Guidance is provided for setting up safe, effective automations.

- [How to use Agentic Workflows for Your Repos with GitHub Copilot](/github-copilot/videos/how-to-use-agentic-workflows-for-your-repos-with-github-copilot)

### Jira Integration: Coding Agent for Issue-Driven PR Automation

Copilot Coding Agent’s new link to Jira Cloud lets teams assign Jira tasks to the agent, which then drafts pull requests based on details and status, asking clarifying questions when necessary. It helps automate repetitive issue-resolution steps and keeps manual reviews intact. Marketplace app installation and repository-level access are required, and data residency controls are included. Documentation is available for setup, and feedback is invited for new features. This addition complements last week's focus on agent-powered task automation.

- [GitHub Copilot Coding Agent Integration with Jira Now in Public Preview](https://github.blog/changelog/2026-03-05-github-copilot-coding-agent-for-jira-is-now-in-public-preview)

### Copilot Usage Metrics: Expanded Telemetry, Plan Mode, and Username Consistency

The Copilot metrics API now provides plan mode telemetry across IDEs and detailed CLI usage at a per-user level, helping organizations understand adoption and support users effectively. The dashboard reflects these changes, and Enterprise Managed Users (EMUs) now have consistent usernames to simplify large-scale tracking.

- [GitHub Copilot Usage Metrics Now Track Plan Mode Telemetry](https://github.blog/changelog/2026-03-02-copilot-metrics-now-includes-plan-mode)
- [GitHub Copilot Metrics Reports Now Return Consistent Usernames for EMUs](https://github.blog/changelog/2026-03-02-copilot-metrics-reports-now-return-consistent-usernames-for-enterprise-managed-users)
- [Copilot Usage Metrics Expanded to User-Level GitHub Copilot CLI Activity](https://github.blog/changelog/2026-03-05-copilot-usage-metrics-now-includes-user-level-github-copilot-cli-activity)

### Agent Management and Network Configuration

Enterprise admins gain session filtering by agent status, repository, and user, simplifying oversight. Outbound network routing for Copilot agents now depends on specific plans; allowlists need updating for self-managed and Azure runners, though GitHub-hosted runners remain unchanged. Documentation explains the settings, updating last week's content on enterprise management and registry.

- [Discover and Manage GitHub Copilot Agent Activity with New Session Filters](https://github.blog/changelog/2026-03-05-discover-and-manage-agent-activity-with-new-session-filters)
- [Network Configuration Update for Copilot Coding Agent Now Active](https://github.blog/changelog/2026-03-02-network-configuration-changes-for-copilot-coding-agent-now-in-effect)

### Global Developer Events and Skill Expansion

GitHub Copilot Dev Days are rolling out worldwide, offering workshops, demos, and hands-on sessions for VS Code and Visual Studio users starting March 15, 2026. GitHub is working with Andela to grow AI skills, offering training and mentorship in Africa and Latin America. This focus on training supports faster onboarding and more complex project management, reflecting Copilot’s dedication to developer education.

- [GitHub Copilot Dev Days: Accelerate Coding with AI in the Microsoft Developer Stack](https://devblogs.microsoft.com/blog/github-copilot-dev-days)
- [Join or Host a GitHub Copilot Dev Days Event Near You](https://github.blog/ai-and-ml/github-copilot/join-or-host-a-github-copilot-dev-days-event-near-you/)
- [How GitHub Copilot and Andela Are Expanding AI Skills Globally](https://github.blog/developer-skills/career-growth/scaling-ai-opportunity-across-the-globe-learnings-from-github-and-andela/)

### Other GitHub Copilot News

The Grok Code Fast 1 AI model is now included for automatic model selection in Copilot Free, making updated suggestions accessible in leading IDEs. Developers using .NET MAUI can try a demonstration of Copilot’s effect on their mobile workflow, covering scaffolding, troubleshooting, and deployment.

- [Grok Code Fast 1 is now available in Copilot Free auto model selection](https://github.blog/changelog/2026-03-04-grok-code-fast-1-is-now-available-in-copilot-free-auto-model-selection)
- [AI-First Mobile Development: Live Coding with Copilot and .NET MAUI](/github-copilot/videos/ai-first-mobile-development-live-coding-with-copilot-and-net-maui)

## AI

The AI section outlines current innovations and tool adoption, especially among Microsoft and open-source developers. The focus is on new agent frameworks, skills SDKs, toolkits, and orchestration options for agentic applications, plus guidance on deploying secure and scalable AI solutions. This continues themes from last week, adding new practical toolkits for production-ready deployment.

### Microsoft Foundry Ecosystem: Model, Agent, and SDK Advancements

Microsoft Foundry’s February 2026 release brings new models and agent features, picking up from recent updates on edge AI and hybrid privacy. Anthropic's Claude Opus/Sonnet 4.6 models enable deep reasoning, scalable deployment, and large context handling. Models like GPT-Realtime-1.5 and GPT-Audio-1.5 support internationalized and voice-driven use cases. The Grok 4.0 engine enhances agent workflows, and FLUX.2 Flex improves text-image generation for interface prototyping.

Model updates complement new features for privacy and on-prem deployments. Foundry Local allows for disconnected, compliance-aware hardware. The Agent Framework (Python) reaches API stability, with better credential management, session orchestration, and migration instructions. Durable agent orchestration via Azure Functions and SignalR enables agents to work around long delays or restarts—useful in public sector and telecom scenarios.

The new Foundry REST API v1 is stable, with SDKs for Python, .NET, JS/TS, and Java. It introduces consistent naming and credential handling. Migration is supported for previous versions. There are also improvements in the AI Toolkit for VS Code (v0.30.0), introducing debugging, a model inspector, and catalog tools for quick prototyping and release. Documentation now further supports onboarding and protocol usage.

- [What's New in Microsoft Foundry: February 2026 Update](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-feb-2026/)
- [Introducing GPT-5.4 in Microsoft Foundry for Enterprise AI Production](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/introducing-gpt-5-4-in-microsoft-foundry/4499785)
- [Unlocking Document Understanding with Mistral Document AI in Microsoft Foundry](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/unlocking-document-understanding-with-mistral-document-ai-in-microsoft-foundry/4495664)

### Modular Skills and Agent Frameworks: Agent Skills SDK, Reusable Skills, and Dynamic Tool Discovery

The Agent Skills SDK, an open-source Python toolkit, helps developers package common agent knowledge as portable skills. These can be published or discovered across different storage providers (local, HTTP, Azure/S3, databases). Skills are loaded only when relevant to save resources. The SDK works with tools like LangChain and Microsoft Agent Framework, supporting skill reuse and modular agent composition. It is MIT-licensed and customizable for DevOps, incident response, and retrieval agents.

Microsoft Agent Framework also now supports reusable skills for .NET/Python agents, packaged with scripts and configuration to support on-demand discovery. Skills are loaded only as needed, reducing context and token requirements. Guidance is included for safely sharing and maintaining skills, with forward-looking features for creating new ones dynamically.

Developers can use `mcp-cli`, a Bun-powered CLI, for finding tools in a token-efficient way, letting agents only fetch what they need. This aligns with earlier updates on secure modular agent deployment.

- [Giving Your AI Agents Reliable Skills with the Agent Skills SDK](https://techcommunity.microsoft.com/t5/microsoft-developer-community/giving-your-ai-agents-reliable-skills-with-the-agent-skills-sdk/ba-p/4497074)
- [Equip Microsoft Agent Framework Agents with Reusable Agent Skills](https://devblogs.microsoft.com/semantic-kernel/give-your-agents-domain-expertise-with-agent-skills-in-microsoft-agent-framework/)
- [MCP vs mcp-cli: Dynamic Tool Discovery for Token-Efficient AI Agents](https://techcommunity.microsoft.com/t5/microsoft-developer-community/mcp-vs-mcp-cli-dynamic-tool-discovery-for-token-efficient-ai/ba-p/4494272)

### Secure Agent Execution and Durable Tasks: Azure Container Apps Dynamic Sessions, MCP C# SDK

Updates in this area give developers ways to run untrusted or agent-supplied code in Azure Container Apps with dynamic sessions, offering sandboxed runtimes for various workloads. Integration with Agent Framework and MCP is supported, while Azure AD and OpenTelemetry are used for authentication and traceability. Templates and deployment instructions make rollout easier, focusing on safe and repeatable ephemeral compute.

The MCP C# SDK v1.0 brings improved capabilities to .NET developers, providing durable AI operations, OAuth 2.0, client/server APIs, tool calling, SSE event streaming, and more. These changes enable secure, large-scale, and async tasks for agentic systems.

- [Safely Running AI-Generated Code with Azure Container Apps Dynamic Sessions](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/even-simpler-to-safely-execute-ai-generated-code-with-azure/ba-p/4499795)
- [Official MCP C# SDK v1.0 Released: Major Updates for Authorization, Tools, and Tasks](https://devblogs.microsoft.com/dotnet/release-v10-of-the-official-mcp-csharp-sdk/)

### Architectures and Best Practices: High-Performance Agentic Systems, Open Standards

A Microsoft guide details best practices for enterprise-scale agentic AI engineering with Foundry and Copilot Studio, emphasizing differences from traditional chatbots, and focusing on autonomy, orchestration, and clear boundaries. The architecture makes use of tools like Microsoft Graph, Logic Apps, and Power Automate. Memory, access control, observability, and compliance requirements are addressed, with case studies for contract analysis and customer support.

A separate panel review discusses how open standards (MCP, Agent2Agent, OpenTelemetry, OAuth) help with secure, interoperable agent deployments, as well as how to write requests-for-proposal and select technologies focused on compatibility.

- [Building High-Performance Agentic Systems: From Chatbots to Enterprise Operations](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-high-performance-agentic-systems/ba-p/4497391)
- [Open Standards for Enterprise Agents: Architecting Secure and Interoperable Agentic AI](/ai/videos/open-standards-for-enterprise-agents-architecting-secure-and-interoperable-agentic-ai)

### Other AI News

VS Code has added five new agent features: skills on demand, message steering, integrated browser, conversation forking, and lifecycle hooks. These updates are aimed at improving productivity and automation for developers.

- [Top 5 New VS Code Agent Features to Improve Your Workflow](/ai/videos/top-5-new-vs-code-agent-features-to-improve-your-workflow)

Copilot for Data Factory (Microsoft Fabric) enables low-code data engineering, including natural language SQL transformations and automated pipeline setup.

- [SQL to Insights in Minutes with Copilot for Data Factory](/ai/videos/sql-to-insights-in-minutes-with-copilot-for-data-factory)

Power Platform developers have new documentation for building Generative Pages with AI, supporting advanced UI features and Dataverse integration.

- [Building Generative Pages in Power Platform](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-generative-pages-in-power-platform/ba-p/4494062)

A new JavaScript AI Build-a-thon season is open, providing hands-on agentic AI and data pipeline challenges for JS/TS developers.

- [JavaScript AI Build-a-thon Season 2: Hands-On Program for AI Developers](https://devblogs.microsoft.com/blog/the-javascript-ai-build-a-thon-season-2-starts-today)

A GitHub interview with Anders Hejlsberg discusses the shift from AI assistants to more capable agents, including their effect on software tools and interoperability.

- [Anders Hejlsberg Discusses the Evolution from AI Assistant to AI Agent](/ai/videos/anders-hejlsberg-discusses-the-evolution-from-ai-assistant-to-ai-agent)

## ML

This week's ML section covers improvements in large language model (LLM) deployment, multimodal AI, and changes to enterprise patterns on Microsoft’s cloud stack. It includes guides on inference efficiency, permission updates, and releases of new AI models.

### LLM Inference Optimization and Architecture on Azure

The Azure ML updates highlight resources for selecting the best trade-offs between prediction accuracy, request latency, and budget, using AKS, Ray Serve, and vLLM. Articles explain technical measures for improving throughput and scaling, such as TTFT, TPOT, batching, quantization, and memory handling. Fine-grained GPU allocation, modular LLM architecture, and best-fit machine selection are covered. Security and compliance remain fundamental for practical deployment.

- [Part 1: Inference at Enterprise Scale—Managing LLM Tradeoffs in Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-1-inference-at-enterprise-scale-why-llm-inference-is-a/ba-p/4498754)
- [The LLM Inference Optimization Stack: A Playbook for Enterprise Teams on Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/the-llm-inference-optimization-stack-a-prioritized-playbook-for/ba-p/4498818)
- [Inference at Enterprise Scale: Why LLM Inference Is a Capital Allocation Problem](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/inference-at-enterprise-scale-why-llm-inference-is-a-capital/ba-p/4498754)
- [Enterprise-Scale Inference on Azure: Architecting for Cost, Latency, and Efficiency](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/inference-at-enterprise-scale-architecting-for-cost-latency-and/ba-p/4498754)

### Multimodal and Vision Reasoning AI: Phi-4-Reasoning-Vision-15B

Microsoft’s Phi-4-Reasoning-Vision-15B model supports new use cases in image-based reasoning, GUI automation, and chart/document analysis. The model’s `thinking_mode` can be adjusted for different latency/quality profiles. Benchmarks show reliable math and object inference. Training insights focus on reinforcement learning, verifier agents, and real-world use cases.

- [Phi-4-Reasoning-Vision-15B: In-Depth Overview and Use Cases](https://techcommunity.microsoft.com/t5/microsoft-developer-community/phi-4-reasoning-vision-15b-use-cases-in-depth/ba-p/4499210)
- [Microsoft Research Unveils Phi-4-Reasoning-Vision-15B Model and Training Insights](https://www.microsoft.com/en-us/research/blog/phi-4-reasoning-vision-and-the-lessons-of-training-a-multimodal-reasoning-model/)

### .NET AI Agent Architecture and Enterprise Patterns

A .NET AI Community Standup session highlights modern agent frameworks, orchestration, and continuous integration/monitoring. It features the Interview Coach sample and introduces MCP and Aspire tooling for production use. The presentation builds on modularization and cloud-native deployment, providing a blueprint for agents in .NET production settings.

- [.NET AI Community Standup: Real-World AI Agent Architecture in .NET](/ai/videos/net-ai-community-standup-real-world-ai-agent-architecture-in-net)

### Other ML News

Read-only permissions are now required to use semantic models with Fabric data agents, while more advanced actions remain behind Build or workspace member roles. This reduces friction and makes collaboration easier for new data modeling projects.

- [Updated Permission Requirements for Semantic Models in Fabric Data Agents](https://blog.fabric.microsoft.com/en-US/blog/update-to-required-permissions-for-semantic-models-in-fabric-data-agents/)

## Azure

This week’s Azure section covers new general availability releases, migration guides, platform engineering changes, and features for secure and real-time data platforms.

### Azure Databricks and Real-Time Data Platforms

Azure Databricks Lakebase is now generally available, providing a Postgres-compatible, serverless data platform for analytics, operational, and AI workloads. State management, real-time feature serving, and agent memory are supported. Postgres migrations are straightforward, and new AI models (Grok 4.0, Qwen3.5, GPT-5.x) are now supported. AI Foundry simplifies advanced model connections and analytics.

- [Azure Databricks Lakebase: General Availability for Real-Time and AI-Driven Applications](https://techcommunity.microsoft.com/t5/azure-databricks/azure-databricks-lakebase-is-now-generally-available/ba-p/4498779)
- [Azure Update 6th March 2026](/ai/videos/azure-update-6th-march-2026)

### Azure Logic Apps and Integration Modernization

Azure Logic Apps enhancements include higher API Management limits, easier migration, SAP workflow support, AKS hybrid deployment, and advanced cost management and monitoring controls. The BizTalk migration tool allows orchestrations to convert easily to Logic Apps. Tutorials cover external configuration, state management, and host migration. These changes make logic-driven automation more accessible for enterprise teams.

- [Logic Apps Aviators Newsletter - March 2026](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-aviators-newsletter-march-2026/ba-p/4498260)
- [Migrating the BizTalk Aggregator Pattern to Azure Logic Apps Standard](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/implementing-migrating-the-biztalk-server-aggregator-pattern-to/ba-p/4495107)

### Hybrid and Sovereign Cloud Capabilities

Azure's hybrid cloud now has new training (Sovereign Cloud MicroHack) for governance, RBAC, encryption, and confidential settings, available as GitHub lab challenges. Azure Arc Gateway for Kubernetes streamlines network operations for regulatory workloads. Both updates help customers meet compliance rules across disconnected or connected environments.

- [Announcing the Public Release of the Sovereign Cloud MicroHack](https://www.thomasmaurer.ch/2026/03/announcing-the-public-release-of-the-sovereign-cloud-microhack/)
- [Azure Arc Gateway for Kubernetes Now Generally Available](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-general-availability-of-the-azure-arc-gateway-for/ba-p/4498561)
- [Azure Update 6th March 2026](/ai/videos/azure-update-6th-march-2026)

### Migration Guides for Azure Networking and Container Workloads

Guides are available for moving from AKS Virtual Nodes to Helm-managed Container Instances, covering steps like add-on disablement, resource clean-up, and new deployment configurations. There’s also new help for ExpressRoute Gateway migration, with focus on public IP SKU changes and best practices for minimal disruption and validation.

- [How to Migrate from Legacy Virtual Nodes to Next-Generation Virtual Nodes on Azure Container Instances](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/migrating-to-the-next-generation-of-virtual-nodes-on-azure/ba-p/4496565)
- [ExpressRoute Gateway Backend Migration: Basic to Standard Public IP SKU](https://techcommunity.microsoft.com/t5/azure-networking-blog/expressroute-gateway-microsoft-initiated-migration/ba-p/4497689)

### Platform Engineering, AI-Driven Infrastructure, and Compliance

This week, analysis covers how AI-driven IaC (infrastructure as code) and policy enforcement streamline compliance and resource management. Using Copilot and other agents, teams can generate and validate infrastructure, create diagrams, and automate compliance checks in development and operations. Platforms like Copilot Spaces and Skills help with modular agent tooling, and policy-as-code systems keep cloud management auditable.

- [Platform Engineering for the Agentic AI Era](https://devblogs.microsoft.com/all-things-azure/platform-engineering-for-the-agentic-ai-era/)

### Location and Networking APIs

The Azure Maps Geocode Autocomplete API is generally available, offering multi-language structured address suggestions and serving as a Bing Maps Autosuggest replacement. Documentation and migration samples provide everything needed for integration and localized applications.

- [Announcing the General Availability of the Azure Maps Geocode Autocomplete API](https://techcommunity.microsoft.com/t5/azure-maps-blog/announcing-the-general-availability-of-the-azure-maps-geocode/ba-p/4499242)

### Azure Managed Disks and Developer Productivity

Managed Disks now support instant access incremental snapshots, improving restore speed and reducing downtime in premium and ultra-tier environments. New API parameters allow granular configuration and more efficient cost control for disk usage.

- [Instant Access Incremental Snapshots: Restore Azure Disks Instantly](https://azure.microsoft.com/en-us/blog/instant-access-incremental-snapshots-restore-without-waiting/)

The Azure Developer CLI (azd) adds support for direct slot swaps with a single command, helping achieve zero-downtime releases and improved CI/CD automation.

- [Azure Developer CLI (azd): One Command to Swap Azure App Service Slots](https://devblogs.microsoft.com/azure-sdk/azd-appservice-swap/)

### Other Azure News

Azure IaaS Resource Center adds new best practices, sample architectures, and migration resources for reliable and scalable environments, including networking, redundancy, GPU scaling, and cost optimization.

- [Azure IaaS: Building Resilient and Scalable Cloud Infrastructure for the AI Era](https://azure.microsoft.com/en-us/blog/azure-iaas-series-explore-new-resources-for-building-a-stronger-more-efficient-infrastructure/)

Microsoft Fabric’s new Execute Query API (public preview) automates Power Query transformations from REST or Spark endpoints, making pipeline development and hybrid analytics easier.

- [Execute Power Query Programmatically in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/execute-power-query-programmatically-in-microsoft-fabric/)

A how-to explains real-time, secure streaming into Fabric using Eventstream connectors and Azure VNets, VPN, or ExpressRoute, designed for regulated scenarios.

- [Powering Secure Private Network Streaming to Microsoft Fabric with Eventstream Connectors (Preview)](https://blog.fabric.microsoft.com/en-US/blog/powering-secure-private-network-streaming-to-fabric-with-eventstream-connectors-preview/)

Netstar’s service migration to Drasi with Azure SQL and EventHub improved real-time fleet tracking and alerts. Event-driven integration cut manual processes and enabled faster reactions, continuing a recent trend toward continuous monitoring with Azure platform tools.

- [How Netstar Streamlined Fleet Monitoring and Reduced Custom Integrations with Drasi](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/how-netstar-streamlined-fleet-monitoring-and-reduced-custom/ba-p/4499592)

## Coding

In Coding this week, major platforms received new updates and improvements. TypeScript 6.0 RC debuted, VS Code expanded support for agent development, and .NET streamlined data and workflow integration. Tutorials and Q&A sessions cover current best practices in productivity and migration.

### TypeScript 6.0 Release Candidate and Migration Path

TypeScript 6.0 RC introduces default strictness settings, native support for Temporal date types, and more flexible module resolution. The `--stableTypeOrdering` flag supports the coming parallel type checker. Updates to standard libraries and the removal of deprecated configuration targets modernize projects. Codemod tools help with migration, and developers are encouraged to check compatibility ahead of TypeScript 7.0, which will feature a Go language implementation for the compiler and tools.

- [Announcing TypeScript 6.0 Release Candidate: Features and Deprecations](https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-rc/)

### Visual Studio Code 1.110: Agent-Driven Development and Insiders Update

VS Code 1.110 brings more agent-specific features, including session memory retention, manual compaction with `/compact`, request queuing, session forking, and lifecycle hooks for workflow automation. Manual and automatic session features support different workflows. Frontend tools, CLI integration, and live session updates round out the release.

Insiders version improvements focus on accessibility, dialogs, keyboard navigation, and terminal experience.

- [Making Agents Practical for Real-World Development with VS Code 1.110](https://code.visualstudio.com/blogs/2026/03/05/making-agents-practical-for-real-world-development)
- [February 2026 Insiders (version 1.110): What’s New in Visual Studio Code](https://code.visualstudio.com/updates/v1_110)

### .NET Data and Workflow Tooling

The .NET Data Standup presents dotConnect and Entity Developer tools for streamlined data access and model automation. This reduces manual configuration and helps avoid errors, supporting quick setup and maintenance.

Community guides offer insight into orchestrating advanced workflows in the Microsoft Agent Framework, encouraging modular and graph-based automation.

- [.NET Data Community Standup: How dotConnect and Entity Developer Streamline .NET Workflows](/dotnet/videos/net-data-community-standup-how-dotconnect-and-entity-developer-streamline-net-workflows)
- [Demystifying Custom Orchestration in Microsoft Agent Framework Workflows](/dotnet/videos/demystifying-custom-orchestration-in-microsoft-agent-framework-workflows)

### .NET MAUI Platform Expansion and Blazor in .NET 11 Preview

This week’s .NET MAUI standup covers expanded platform support (including Linux and macOS), plus AI-driven acceleration for routine app development. Blazor Community Standup discusses productivity, performance, and tooling improvements in .NET 11 Preview 2 for web development.

- [.NET MAUI Community Standup: Expanding to New Platforms and Accelerating with AI](/ai/videos/net-maui-community-standup-expanding-to-new-platforms-and-accelerating-with-ai)
- [Blazor Community Standup: What’s New in ASP.NET Core & Blazor for .NET 11 Preview 2](/dotnet/videos/blazor-community-standup-whats-new-in-aspnet-core-and-blazor-for-net-11-preview-2)

## DevOps

This week's DevOps updates emphasize better project management, platform changes for reliability, and automation for CI/CD and high-availability systems.

### GitHub Issues and Projects: Enhanced Features and Workflow Guides

GitHub Projects added a hierarchy view with sub-issue filtering and deduplication, persistent list state, issue forms with file upload, and bulk issue moves. Guides help teams and new users get started with board setup, collaborative features, and automation strategy.

- [GitHub Projects Hierarchy View and Issue Form Enhancements](https://github.blog/changelog/2026-03-05-hierarchy-view-improvements-and-file-uploads-in-issue-forms)
- [Organizing Work with GitHub Issues and Projects for Beginners](/devops/videos/organizing-work-with-github-issues-and-projects-for-beginners)
- [Manage Your Flow with GitHub Issues and Projects](/devops/videos/manage-your-flow-with-github-issues-and-projects)
- [GitHub for Beginners: Mastering Issues and Projects](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-issues-and-projects/)

### GitHub Platform and DevOps Tooling Improvements

GitHub Enterprise Server now uses a single-node Elasticsearch cluster with cross-cluster replication for easier management and better reliability. Advanced Security policies for source control and code quality are split so that admins have finer controls. There’s a new policy page for quality requirements.

The Actions Example Checker tool ensures usage samples in CI/CD are current. Pull request pages now show merge status and blockers directly, helping with code review.

- [Rebuilding Search Architecture for High Availability in GitHub Enterprise Server](https://github.blog/engineering/architecture-optimization/how-we-rebuilt-the-search-architecture-for-high-availability-in-github-enterprise-server/)
- [GitHub Code Quality Enterprise Policy](https://github.blog/changelog/2026-03-03-github-code-quality-enterprise-policy)
- [Keep Your GitHub Action Examples in Sync with Actions Example Checker](https://jessehouwing.net/keep-your-examples-in-sync-with-your-action/)
- [Quick Access to Pull Request Merge Status Now in Public Preview](https://github.blog/changelog/2026-03-05-quick-access-to-merge-status-in-pull-requests-in-public-preview)

### Microservice Architecture Patterns and Solutions

A new resource reviews common pitfalls in microservice architectures and explains practical solutions including API gateways, async brokers, distributed tracing, CI/CD, circuit breaking for reliability, and zero-trust practices. The guide advises matching architecture to team needs and using simpler monoliths when appropriate.

- [Microservice Architecture Drawbacks and How to Solve Them: A Solution Architect’s Perspective](https://dellenny.com/microservice-architecture-drawbacks-and-how-to-solve-them-a-solution-architects-perspective/)

### Other DevOps News

GitHub metered products now send automatic usage alert emails at 90% and 100% of quotas to help teams monitor capacity and make proactive adjustments.

- [Email Notifications for GitHub Usage Thresholds](https://github.blog/changelog/2026-03-03-email-notifications-for-included-usage-thresholds)

## Security

Security this week covers both new threats and updated controls, with research on attacks using authentication weaknesses, vulnerability management, software supply chain changes, and the dual role of AI in state-of-the-art threats and defense.

### Microsoft Account and Authentication Attack Analysis

The latest research details real-world attacks using OAuth application redirection in Entra ID for phishing or malware delivery. Attackers exploit OAuth parameters to redirect users to malicious URLs. Practical steps for defenders include tightening app permissions, monitoring authentication flows, and enforcing stronger access policies. There is also analysis of malware signed with stolen EV certificates, which is then used to install RMM tools as persistent footholds. Recommendations are provided for auditing, whitelisting, deploying AppLocker, and reviewing RMM credentials.

- [OAuth Redirection Abuse Tactics: Phishing and Malware Delivery Exposed](https://www.microsoft.com/en-us/security/blog/2026/03/02/oauth-redirection-abuse-enables-phishing-malware-delivery/)
- [Signed Malware Impersonating Workplace Apps Deploys RMM Backdoors](https://www.microsoft.com/en-us/security/blog/2026/03/03/signed-malware-impersonating-workplace-apps-deploys-rmm-backdoors/)

### Global Disruption of Advanced Phishing Kits and MFA Bypass Platforms

A partnership between Microsoft, Europol, and global agencies dismantled the Tycoon 2FA phishing kit infrastructure, which previously enabled large-scale identity impersonation. Tactics included relay, session hijacking, AI-based phishing, and rapid domain switching. The articles offer mitigation steps, including MFA reviews, mailbox rule checks, token revocation, Defender tools, and post-attack clean-up.

- [How a Global Coalition Disrupted Tycoon 2FA: A Major Impersonation Platform](https://blogs.microsoft.com/on-the-issues/2026/03/04/how-a-global-coalition-disrupted-tycoon/)
- [Inside Tycoon2FA: How a Leading AiTM Phishing Kit Operated at Scale](https://www.microsoft.com/en-us/security/blog/2026/03/04/inside-tycoon2fa-how-a-leading-aitm-phishing-kit-operated-at-scale/)

### AI-Enabled Security Threats, Detection, and Response

Microsoft Threat Intelligence describes how attackers are adopting AI for scanning vulnerabilities, developing malware, social engineering, and building infrastructure. LLMs are now sometimes used for reconnaissance, with North Korean groups highlighted. Defender recommendations include enabling AI-focused dashboards, prompt filter controls, and Defender Copilot products. The section also warns of AI browser extensions that steal chat data, with guidance on detection, extension auditing, and Defender management.

- [AI as Tradecraft: Threat Actors Operationalize AI in Cyberattacks](https://www.microsoft.com/en-us/security/blog/2026/03/06/ai-as-tradecraft-how-threat-actors-operationalize-ai/)
- [Malicious AI Browser Extensions Expose LLM Chat Histories: Microsoft Defender Analysis](https://www.microsoft.com/en-us/security/blog/2026/03/05/malicious-ai-assistant-extensions-harvest-llm-chat-histories/)

### Secure Software Supply Chain and User Controls

Dependabot now allows security alert assignment for better accountability. Bulk assignment and additional webhook/API support enable more structured response. Draft advisories can be locked for authorized edit only, improving audit trail and compliance support.

- [Dependabot Alert Assignees Now Available for GitHub Advanced Security](https://github.blog/changelog/2026-03-03-dependabot-alert-assignees-are-now-generally-available)
- [Improved Control Over Draft Repository Security Advisories on GitHub](https://github.blog/changelog/2026-03-04-lock-and-unlock-draft-repository-security-advisories)

### Automation and AI-Driven Vulnerability Discovery

The GitHub Security Lab Taskflow Agent enables automated vulnerability scanning using YAML-based taskflows and LLMs. Documentation covers review of logic, control, and privilege tasks in your pipeline. User taskflows, integration with Codespaces, and Copilot support help teams act on findings more efficiently.

- [How to Scan for Vulnerabilities with GitHub Security Lab's Open Source AI-Powered Framework](https://github.blog/security/how-to-scan-for-vulnerabilities-with-github-security-labs-open-source-ai-powered-framework/)

### Secure Binary Signing with Azure Trusted Signing and dotnet sign

Rick Strahl details workflow improvements for code signing using Azure Trusted Signing and dotnet sign. Changes include simpler authentication, faster processes, and support for CI/CD scripts, covering details about configuration, integration, and security best practices. The overview addresses protecting certificates and ensuring responsible automated signoff.

- [Azure Trusted Signing Revisited with Dotnet Sign](https://weblog.west-wind.com/posts/2026/Mar/02/Azure-Trusted-Signing-Revisited-with-Dotnet-Sign)

### Other Security News

GitHub Octoverse analysis highlights how automation tools like Copilot Autofix and Dependabot speed up vulnerability patching and support a "shift left" approach. Features discussed include merge queues and new access control checks supported by insights, reinforcing the balance between AI-driven automation and human oversight.

- [How AI is Changing the 'Shift Left' Mindset in Security – Insights from GitHub Octoverse 2025](/ai/videos/how-ai-is-changing-the-shift-left-mindset-in-security-insights-from-github-octoverse-2025)
