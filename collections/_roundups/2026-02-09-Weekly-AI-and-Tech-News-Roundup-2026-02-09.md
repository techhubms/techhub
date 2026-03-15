---
external_url: /github-copilot/roundups/Weekly-AI-and-Tech-News-Roundup
title: 'Agent Workflows, AI Platform Updates, and Security: GitHub Copilot, Azure, and More'
author: TechHub
primary_section: github-copilot
date: 2026-02-09 09:00:00 +00:00
feed_name: TechHub
tags:
- .NET
- AI
- AI Orchestration
- Azure
- Azure Infrastructure
- CI/CD
- Cloud Native Development
- Copilot SDK
- Data Engineering
- DevOps
- GitHub Copilot
- Kubernetes
- Microsoft Fabric
- ML
- Model Management
- Multi Agent Systems
- Roundups
- Security
- VS Code
section_names:
- ai
- github-copilot
- ml
- azure
- dotnet
- devops
- security
---
Welcome to this week’s technology roundup, focusing on agent frameworks and AI-powered development platforms. GitHub Copilot adds multi-agent workflows and unified governance, supporting both individual developers and organizations with tools for building, managing, and securing AI-centric pipelines. Azure introduces new hardware, observability features, and detailed security controls to advance enterprise AI use. Updates in Visual Studio Code, .NET, and open source practices continue to strengthen development environments, machine learning orchestration, and threat response. Each section gives you up-to-date technical insights to support growing automation and productivity.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Multi-Agent Development and Integration in VS Code](#multi-agent-development-and-integration-in-vs-code)
  - [The Expanding Copilot Ecosystem: Agent HQ, SDK, and Governance](#the-expanding-copilot-ecosystem-agent-hq-sdk-and-governance)
  - [GitHub Copilot SDK: Cloud-Native Agents, Community Projects, and Demos](#github-copilot-sdk-cloud-native-agents-community-projects-and-demos)
  - [Multi-Agent Strategies: Claude, Codex, and GitHub Copilot in Practice](#multi-agent-strategies-claude-codex-and-github-copilot-in-practice)
  - [Advances in Copilot Model Management and AI Model Options](#advances-in-copilot-model-management-and-ai-model-options)
  - [Copilot Experience Updates in Visual Studio, VS Code, and the Web](#copilot-experience-updates-in-visual-studio-vs-code-and-the-web)
  - [Custom Agents, Prompts, and Quality Engineering](#custom-agents-prompts-and-quality-engineering)
  - [Practical Tutorials: CLI, Agents, and Documentation Context](#practical-tutorials-cli-agents-and-documentation-context)
  - [Deep Agentic Workflows: Skills, Integration, and Observability](#deep-agentic-workflows-skills-integration-and-observability)
  - [Real-World Use Cases: Modernization, Kafka, and Community Coding](#real-world-use-cases-modernization-kafka-and-community-coding)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Azure AI Services and Foundry Ecosystem](#azure-ai-services-and-foundry-ecosystem)
  - [AI-Powered App Development with .NET and MCP](#ai-powered-app-development-with-net-and-mcp)
  - [Local AI Model Benchmarking and Scientific Evaluation](#local-ai-model-benchmarking-and-scientific-evaluation)
  - [Practical AI: Continuous Automation and Safe API Workflows](#practical-ai-continuous-automation-and-safe-api-workflows)
  - [AI-Powered Image Generation with Serverless Azure Functions](#ai-powered-image-generation-with-serverless-azure-functions)
- [ML](#ml)
  - [Smarter Pipeline Orchestration in Shared Spark Environments](#smarter-pipeline-orchestration-in-shared-spark-environments)
  - [Real-Time Analytics and ML in Spark Notebooks with Eventstreams Integration](#real-time-analytics-and-ml-in-spark-notebooks-with-eventstreams-integration)
  - [Improved Visualization for Real-Time Dashboards: Custom Series Colors](#improved-visualization-for-real-time-dashboards-custom-series-colors)
- [Azure](#azure)
  - [Cloud Architecture, Resource Management, and Resiliency](#cloud-architecture-resource-management-and-resiliency)
  - [Kubernetes and Container Solutions](#kubernetes-and-container-solutions)
  - [Real-Time Analytics and Data Engineering](#real-time-analytics-and-data-engineering)
  - [AI-Ready Databases and Developer Integration](#ai-ready-databases-and-developer-integration)
  - [Enterprise Data Warehousing and Migration](#enterprise-data-warehousing-and-migration)
  - [Platform Automation, Security, and Networking](#platform-automation-security-and-networking)
  - [Developer Experience and Workflow Enhancements](#developer-experience-and-workflow-enhancements)
  - [AI Platform Foundations and Hub-and-Spoke Architectures](#ai-platform-foundations-and-hub-and-spoke-architectures)
  - [Data Integration, Storage, and Backup](#data-integration-storage-and-backup)
  - [Azure Databricks and Analytics Platform](#azure-databricks-and-analytics-platform)
  - [High-Performance Computing and Observability](#high-performance-computing-and-observability)
  - [Microsoft Fabric Data Warehouse](#microsoft-fabric-data-warehouse)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [Visual Studio Code: Workflow, Multi-Agent, Terminal, and Beginner Experience](#visual-studio-code-workflow-multi-agent-terminal-and-beginner-experience)
  - [.NET Platform: Community-Driven Libraries, Roadmaps, and Deployment Changes](#net-platform-community-driven-libraries-roadmaps-and-deployment-changes)
  - [Visual Studio and Editor Extensibility](#visual-studio-and-editor-extensibility)
  - [Software Development Trends and Open Source Tooling](#software-development-trends-and-open-source-tooling)
  - [Observability, Diagnostics, and Application Instrumentation in .NET](#observability-diagnostics-and-application-instrumentation-in-net)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [GitHub Actions: Runner Updates, Scale Sets, and Enhanced Workflow Features](#github-actions-runner-updates-scale-sets-and-enhanced-workflow-features)
  - [WinGet Configuration: One-command Developer Environments](#winget-configuration-one-command-developer-environments)
  - [Open Source Ecosystem: Dependabot Proxy and Documentation Translation as Versioned Assets](#open-source-ecosystem-dependabot-proxy-and-documentation-translation-as-versioned-assets)
  - [GitHub Review and Collaboration: Pull Request and Issue Experience](#github-review-and-collaboration-pull-request-and-issue-experience)
- [Security](#security)
  - [Defender XDR and Threat Analysis Campaigns](#defender-xdr-and-threat-analysis-campaigns)
  - [Secure Development at Scale: Automation and Operational Guardrails](#secure-development-at-scale-automation-and-operational-guardrails)
  - [AI and Data Security: Risks, Detection, and Lifecycle Evolution](#ai-and-data-security-risks-detection-and-lifecycle-evolution)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week, GitHub Copilot added enhanced multi-agent coordination, a unified management environment, new models, and additional developer controls with the Copilot SDK. These changes enable more robust workflows, faster feedback, and customizable AI support, giving organizations improved governance and an easier path to extend Copilot.

### Multi-Agent Development and Integration in VS Code

Visual Studio Code v1.109 now supports routine multi-agent work, letting users manage Copilot, Claude, and Codex agents together. The Agent Sessions view brings all agent management into one place with simple session switching and monitoring. Codex runs locally via extension, while Claude can run either locally or in the cloud for Copilot Pro+ subscribers. The platform adopts open standards (MCP Apps, Anthropic Agent Skills) to let agents use skills and extend their features. New agent protocol and organizational views improve monitoring and assignment. Developers can run subagents in parallel for specialized reviews, security tasks, or subject matter expertise, supporting team workflows. Ongoing documentation and feedback threads help teams create collaborative environments and maintain clear, efficient, AI-organized workflows in VS Code.

- [VS Code 1.109: Multi-Agent Development with Copilot, Claude, and Codex](https://code.visualstudio.com/blogs/2026/02/05/multi-agent-development)

### The Expanding Copilot Ecosystem: Agent HQ, SDK, and Governance

GitHub improves AI tool coordination using Agent HQ, improvements to the Copilot CLI, the Copilot SDK, and Agent Mode. Agent HQ works as a central interface for organizations to set up, manage, and track agents like Claude and Codex both in GitHub and VS Code. Enhancing repository-based controls, Agent HQ offers organization-wide governance including branch controls and change logs, making automation safer and more adaptable. Copilot CLI's Plan Mode audits terminal commands, and Agentic Memory helps capture policies and standards. The Copilot SDK lets engineers use Copilot with their Node.js or Python tools and enforces compliance and secure access. PR times are reduced and reliability is improved, giving teams flexibility and protection for AI adoption.

- [How GitHub Bridges the Fragmented AI Development Landscape](https://devblogs.microsoft.com/all-things-azure/the-os-for-intelligence-how-github-bridges-the-fragmented-ai-landscape/)

### GitHub Copilot SDK: Cloud-Native Agents, Community Projects, and Demos

New step-by-step guides and demos show developers how to use the Copilot SDK for smart agent applications. These practical resources, expanding on last week’s hybrid app patterns, show how to blend the Copilot SDK, Agent-to-Agent Protocol, and Azure Container Apps to develop and securely deploy agent systems in the cloud. Tutorials explain skill files, agent lookup, security, and scaling. Projects include educational apps like “Flight School” and showcase top community work in automation or browser agents. The SDK enables quick app creation, GitHub API use, and workflow automation, with sample code and deployment steps encouraging developers to try cloud agent system design.

- [The Perfect Fusion of GitHub Copilot SDK, Agent Protocol, and Cloud Native Deployment](https://techcommunity.microsoft.com/t5/microsoft-developer-community/the-perfect-fusion-of-github-copilot-sdk-and-cloud-native/ba-p/4491199)
- [GitHub Copilot SDK Demo: Building 'Flight School' Personalized Coding Challenges](/github-copilot-sdk-demo-building-flight-school-personalized-coding-challenges)
- [Top 10 Community Projects Built with GitHub Copilot SDK](/top-10-community-projects-built-with-github-copilot-sdk)

### Multi-Agent Strategies: Claude, Codex, and GitHub Copilot in Practice

Copilot Pro+ and Enterprise subscriptions now provide access to Claude and Codex agents in Agent HQ for web, mobile, and VS Code. Extending new org-level features, teams can use several agents for code reviews, assign reviewers, and automate parts of the review cycle. Dashboards help manage reviewer assignments, see access levels, and review audit logs. Tutorials show how to use multiple agents together by handling sessions and artifacts that affect pull requests or issues. Planned support for agents from Google, Cognition, and xAI will offer expanded choice in a central location. The documentation covers best practices and setup.

- [Using Claude and Codex AI Agents in GitHub Agent HQ](https://github.blog/news-insights/company-news/pick-your-agent-use-claude-and-codex-on-agent-hq/)
- [Claude and Codex Agents Now Available for GitHub Copilot Pro+ and Enterprise](https://github.blog/changelog/2026-02-04-claude-and-codex-are-now-available-in-public-preview-on-github)
- [How to Use Claude, Codex, and GitHub Copilot Together in GitHub and VS Code](/how-to-use-claude-codex-and-github-copilot-together-in-github-and-vs-code)

### Advances in Copilot Model Management and AI Model Options

GitHub now enables newer Copilot models by default to reduce friction. Claude Opus 4.6 is rolling out to all Pro+, Pro, and Free users (manual selection remains possible). This model provides advanced planning abilities and tool integration. Fast Mode for Claude Opus 4.6 can speed up output by up to 2.5x for Enterprise users, when configured by admins. Last week's changes for easier model access and command-line switching now extend to web and IDEs. Documentation makes model selection clear and accessible.

- [Simplified Copilot Model Enablement for GitHub Users](https://github.blog/changelog/2026-02-03-simplified-copilot-model-enablement-experience-for-individual-users)
- [Claude Opus 4.6 Now Available in GitHub Copilot](https://github.blog/changelog/2026-02-05-claude-opus-4-6-is-now-generally-available-for-github-copilot)
- [Fast Mode for Claude Opus 4.6 Rolls Out in GitHub Copilot Preview](https://github.blog/changelog/2026-02-06-claude-opus-4-6-fast-is-now-in-public-preview-for-github-copilot)
- [Fast Mode for Claude Opus 4.6 Now Available in GitHub Copilot Preview](https://github.blog/changelog/2026-02-07-claude-opus-4-6-fast-is-now-in-public-preview-for-github-copilot)

### Copilot Experience Updates in Visual Studio, VS Code, and the Web

Copilot for Visual Studio and VS Code received various updates. Visual Studio 2026 (January) brings colorized completions and an option to accept part of an AI suggestion, leading to clearer, more integrated results. This builds on recent GPT-5.2-Codex features and context-based suggestions. Editor updates refine scrolling, selection, and markdown experiences, including support for Mermaid and interactive previews. VS Code’s January update improves agent tasking, multi-agent collaboration, token chat, and introduces deeper session controls, enhanced terminal safety, and in-editor browsers for testing. Copilot for the web now lists tool actions, provides export (JSON/Markdown), and attaches chats to repositories—making browser usage clearer and more manageable.

- [Roadmap for AI and GitHub Copilot in Visual Studio: February Update](https://devblogs.microsoft.com/visualstudio/roadmap-for-ai-in-visual-studio-february/)
- [GitHub Copilot in Visual Studio Code v1.109: Major Agent-Driven Improvements (January 2026 Release)](https://github.blog/changelog/2026-02-04-github-copilot-in-visual-studio-code-v1-109-january-release)
- [GitHub Copilot in Visual Studio 2026 — January Update](https://github.blog/changelog/2026-02-04-github-copilot-in-visual-studio-january-update)
- [Enhancements to GitHub Copilot Chat on the Web: Tool Calls, Exports, and More](https://github.blog/changelog/2026-02-04-showing-tool-calls-and-other-improvements-to-copilot-chat-on-the-web)

### Custom Agents, Prompts, and Quality Engineering

Teams using Copilot with Azure DevOps can now design custom Azure Boards agents (.agent.md) for automating and standardizing pull request handling, helping review quality and consistency. These changes extend earlier support for logic app/data map profiles and deployment walkthroughs. Prompt engineering guides focused on Copilot in QA/staging highlight usable roles, prompt examples, and anti-patterns, making it easier to automate testing, regression checks, and coverage improvements. Teams can use prompt context providers (such as MCP) to adapt Copilot for specialized or routine work.

- [Azure Boards Now Supports Custom Agents in GitHub Copilot Integration](https://devblogs.microsoft.com/devops/azure-boards-integration-with-github-copilot-includes-custom-agent-support/)
- [Writing Effective Prompts for Testing Scenarios: AI-Assisted Quality Engineering](https://techcommunity.microsoft.com/t5/microsoft-developer-community/writing-effective-prompts-for-testing-scenarios-ai-assisted/ba-p/4488001)

### Practical Tutorials: CLI, Agents, and Documentation Context

Recent tutorials emphasize hands-on Copilot use, including updated CLI automation and model management. A video walkthrough describes how Copilot CLI’s '/share' command sends logs and diagrams to gists, making collaboration easier—especially with agents like Claude Opus and Next.js diagrams. Another guide covers custom agent creation through the CLI with details on setup, using the MCP server, and automating tasks. Scott Hanselman’s workflow shows how MCP feeds live documentation into Copilot for richer, up-to-date suggestions and visualizations. These examples help developers fine-tune Copilot for their specific needs.

- [How to Use the /share Command in GitHub Copilot CLI](/how-to-use-the-share-command-in-github-copilot-cli)
- [Getting Started with GitHub Copilot CLI and Custom Agents](/getting-started-with-github-copilot-cli-and-custom-agents)
- [Configuring Model Context Protocol in the GitHub Copilot CLI](/configuring-model-context-protocol-in-the-github-copilot-cli)

### Deep Agentic Workflows: Skills, Integration, and Observability

Agent Skills (SKILL.md files) let Copilot repeat standardized actions across environments. VS Code is piloting support for loading and managing these skill modules, helping developers reuse scripts vetted for reliability and security. This expands on last week’s discussions of agent skill architectures and new MCP links. MCP can now forward Azure App Service logs to Copilot, letting users troubleshoot via plain language, saving DevOps time. With end-to-end pipelines, full automation now spans requirements, coding, pull requests, CI/CD, and incident handling—demonstrated in new workflow analyses.

- [Integrating Agent Skills with GitHub Copilot and Visual Studio Code](https://techcommunity.microsoft.com/t5/apps-on-azure/integrate-agents-with-skills-in-github-copilot/m-p/4492020#M1391)
- [Chat with Your App Service Logs Using GitHub Copilot](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/chat-with-your-app-service-logs-using-github-copilot/ba-p/4491573)
- [An AI-Led SDLC: Building an Agentic E2E Software Lifecycle with Azure and GitHub](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/an-ai-led-sdlc-building-an-end-to-end-agentic-software/ba-p/4491896)

### Real-World Use Cases: Modernization, Kafka, and Community Coding

Modernizing older .NET applications is more straightforward, thanks to a Copilot Visual Studio agent that walks through code evaluation, migration planning, and execution with analysis and dependency tracking. This connects with earlier coverage on code migration and SQL changes. Developing with Kafka is now easier in VS Code with the Confluent extension, Copilot, and MCP, giving smart code hints and project bootstrapping. Community competitions like Agents League and Battle encourage creative agent development and group interaction, positioning Copilot as a tool for both productivity and collaborative coding.

- [Modernizing Legacy .NET Apps with GitHub Copilot: Step-by-Step Upgrade](/modernizing-legacy-net-apps-with-github-copilot-step-by-step-upgrade)
- [AI-Powered Kafka Development with Confluent VS Code Extension, GitHub Copilot, and MCP](/ai-powered-kafka-development-with-confluent-vs-code-extension-github-copilot-and-mcp)
- [Agents League: 2-Week AI Developer Challenge Featuring GitHub Copilot and Microsoft Foundry](/agents-league-2-week-ai-developer-challenge-featuring-github-copilot-and-microsoft-foundry)
- [Agents League Battle 1: Building Creative AI Apps with GitHub Copilot](/agents-league-battle-1-building-creative-ai-apps-with-github-copilot)

### Other GitHub Copilot News

GitHub Copilot supports open source growth and ongoing integration. In a live session, Martin Woodward discussed creative Copilot uses—like orchestrating a Furby music hack—and GitHub's open Agent HQ plans. New tools such as 'actions-semver-checker' use Copilot Agents and Claude for improved workflow validation, helping maintainers automate version checks and release tasks. Copilot's use is broadening from simple suggestions to more automated and community-aware processes.

- [Open Source Friday: Martin Woodward on GitHub Copilot, Furby Hack, and Agent HQ](/open-source-friday-martin-woodward-on-github-copilot-furby-hack-and-agent-hq)
- [Improved Versioning and Release Automation for GitHub Actions Maintainers](https://jessehouwing.net/github-actions-automatic-versioning-for-actions/)

## AI

This week’s AI highlights include new cloud automation resources, practical development guides, and accessible tools for deploying, auditing, and monitoring AI in the enterprise. Topics center on production implementation—safe agent workflows, integrated hardware, and balancing new features with transparency.

### Azure AI Services and Foundry Ecosystem

Azure now hosts Anthropic's Claude Opus 4.6 in Foundry, giving developers access to agent workflows and embedded automation features. Claude Opus 4.6 supports complex reasoning, a large context window (beta), and detailed deployment controls useful for projects requiring compliance, refactoring, or secure document handling. Copilot Studio integration helps organizations scale agent use with proper review and oversight.

Building on last week’s protocol and workflow updates, Azure’s AI strategy emphasizes practical adoption—developer docs and new Maia 200 hardware events mark ongoing infrastructure support. Teams can deploy models on updated AI hardware and follow practices for secure, monitored automation.

Architecture guides describe how to create traceable Copilot workflows with strict permissions, audit trails, human-in-the-loop steps, safe API design, and Azure-based monitoring. With these best practices, teams can automate key tasks using Copilot Studio, Power Automate, and Graph APIs as covered in earlier governance news.

Maia 200, a new AI hardware accelerator optimized for Azure, offers scalable deployment for inference tasks. Technical content and demos, released at ISSCC 2026, as well as usage in both internal and public Foundry, support infrastructure and development teams with practical examples.

A reference on observability for generative AI details systematic evaluation strategies, including objective selection, dataset configuration, metrics, and regular risk checks. Developers receive step-by-step process advice for frequent audits—covering integration, cost, regional policy, and tech—reflecting last week’s evaluation baseline.

- [Claude Opus 4.6 Now Available in Microsoft Foundry: Advanced Agentic AI for Coding, Security, and Enterprise Workflows](https://azure.microsoft.com/en-us/blog/claude-opus-4-6-anthropics-powerful-model-for-coding-agents-and-enterprise-workflows-is-now-available-in-microsoft-foundry-on-azure/)
- [Designing Safe and Scalable Agentic Workflows with Microsoft Copilot](https://dellenny.com/designing-safe-agentic-workflows-with-microsoft-copilot/)
- [Introducing Maia 200: Microsoft’s Next-Generation AI Inference Accelerator](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/join-microsoft-as-we-share-more-on-maia-200-in-the-bay-area/ba-p/4492370)
- [Observability in Generative AI: Building Trust with Systematic Evaluation in Microsoft Foundry](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/observability-in-generative-ai-building-trust-with-systematic/ba-p/4492231)

### AI-Powered App Development with .NET and MCP

.NET app builders now have more guidance for adding AI features to web and enterprise projects. The current ASP.NET Community Standup covers using Progress Telerik AI controls with Blazor and MCP, showcasing generators for UI and automated scaffolding.

Expanding on last week's new .NET AI features, advice is shifting from best practices to end-to-end deployment for all environments. The .NET ecosystem is emphasizing privacy, efficiency, and support for hybrid approaches, as outlined in the new "Foundry Local for C# devs" resource.

A comprehensive lesson describes how to build modular, reusable AI Skills Executors in .NET, using Azure OpenAI and MCP. This helps teams split skills (YAML-based prompts, toolsets) from orchestration code, improving flexibility, testability, and implementation for cases like code analysis or project tracking. The skills-first architecture also enables smooth rollout and ongoing monitoring.

- [Build AI‑Powered .NET Apps with Telerik](/build-aipowered-net-apps-with-telerik)
- [.NET AI Community Standup: Foundry Local for C# Developers](/net-ai-community-standup-foundry-local-for-c-developers)
- [Building an AI Skills Executor in .NET with Azure OpenAI and MCP](https://devblogs.microsoft.com/foundry/dotnet-ai-skills-executor-azure-openai-mcp/)

### Local AI Model Benchmarking and Scientific Evaluation

Developers can now test local AI models more reliably using tools built from the Foundry Local and FLPerformance SDK. These let teams measure speed, throughput, and resource usage, giving detailed comparison dashboards in React. Guidance includes hardware recommendations, how to design custom test suites, ways to reduce measurement noise, and links to open source projects for quick setup.

This reflects last week’s coverage of scalable measurement tools and evaluation frameworks within Foundry.

- [Scientific Benchmarking of Local AI Models with Foundry Local and FLPerformance](https://techcommunity.microsoft.com/t5/microsoft-developer-community/benchmarking-local-ai-models/ba-p/4490780)

### Practical AI: Continuous Automation and Safe API Workflows

End-to-end AI automation can now be adopted in CI/CD through generative agents—new guides show how agents can check code, write reports, and create documentation, converting plain-language instructions to CI tasks with GitHub Actions. Security and transparency are emphasized; teams can test these patterns incrementally using GitHub Next’s sample projects. This follows last week’s focus on managed workflow context for AI pipelines.

Another guide shares patterns for building reliable APIs using language models for intent and entity extraction, while ensuring business logic and validation stay deterministic. Libraries like LangGraph handle confidence thresholds, schema checking, and clarification, keeping APIs robust and fast despite using LLMs.

- [Continuous AI in Practice: Bringing Judgment-Based Automation to Code Repositories](https://github.blog/ai-and-ml/generative-ai/continuous-ai-in-practice-what-developers-can-automate-today-with-agentic-ci/)
- [How to Build Safe Natural Language-Driven APIs](https://techcommunity.microsoft.com/t5/microsoft-developer-community/how-to-build-safe-natural-language-driven-apis/ba-p/4488509)

### AI-Powered Image Generation with Serverless Azure Functions

A how-to tutorial explains using Stable Diffusion with Azure Functions on cloud GPUs to set up serverless image generation. The article outlines building and deploying a Python container, configuring scalable compute, and integrating with CLI automation for more consistent delivery. Troubleshooting tips, billing considerations, and next steps for UI integration or custom model use are included.

- [Build an AI Image Generator with Azure Functions and Serverless GPUs](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/serverless-gpu-tutorial-build-an-ai-image-generator-with-azure/ba-p/4492228)

## ML

This week’s machine learning news includes more advanced Spark pipeline management, improved real-time analytics, and visual upgrade options in Microsoft Fabric and Azure Synapse.

### Smarter Pipeline Orchestration in Shared Spark Environments

Admins of Spark workloads can now use a priority-based orchestration approach using job tags like Light/Critical, Medium/High, or Heavy/Best Effort with metadata for optimized job scheduling. This strategy, compatible with Microsoft Fabric and Synapse, supports both fixed and adaptive classification. Copilot-style agents monitor and adjust workload class, reducing human input and increasing stability. Ready-to-use sample notebooks and template tools are available to get started. These changes deepen pipeline control and protection, continuing last week’s surge management efforts.

- [Smart Pipelines Orchestration: Designing Predictable Data Platforms on Shared Spark](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/smart-pipelines-orchestration-designing-predictable-data/ba-p/4491766)

### Real-Time Analytics and ML in Spark Notebooks with Eventstreams Integration

Direct Eventstreams and Spark Notebook integration (preview) in Fabric means instant access to over 30 streaming data sources—like CDC databases or brokers—right from the Real-Time Hub. PySpark code is auto-generated, Entra ID secures access, and one-click imports enable fast prototyping and migration to production. Early community feedback is encouraged. This builds on earlier additions like "Get Data with Cloud Connection," supporting a smooth transition from batching to real-time analytics.

- [Integrating Real-Time Eventstreams with Spark Notebooks in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/bringing-together-fabric-real-time-intelligence-notebook-and-spark-structured-streaming-preview/)

### Improved Visualization for Real-Time Dashboards: Custom Series Colors

Microsoft Fabric’s dashboard series now features customizable data series colors for any chart, so teams can visually separate operational data for easier monitoring and clarity. Documentation covers usage, supporting ongoing dashboard improvements.

- [Introducing Data Series Colors: Enhanced Visualization Control in Real-Time Dashboards](https://blog.fabric.microsoft.com/en-US/blog/32713/)

## Azure

New Azure releases strengthen infrastructure, automation, security, and developer experience.

### Cloud Architecture, Resource Management, and Resiliency

John Savill’s "Azure State of the Union 2026" covers practical guidance for cloud design: managing capacity, right-sizing VMs, scaling, configuring zones, governance, automation, and basic security. Infrastructure as Code, policy enforcement, and multi-region deployment are included. For specialized workloads, a six-layer Citrix VDI reference highlights Global VNet Peering, Azure NetApp Files, FSLogix Cloud Cache, and Azure Front Door for backup and disaster recovery.

- [Azure State of the Union 2026](/azure-state-of-the-union-2026)
- [Building Resilient Multi-Region Citrix VDI Workloads on Azure: A Six-Layer Framework](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/proactive-resiliency-in-azure-for-specialized-workload-i-e/ba-p/4492260)

### Kubernetes and Container Solutions

The Retina 1.0 release adds network monitoring for K8s clusters on Linux or Windows, integrating with tools like eBPF, Prometheus, Grafana, and Azure Monitor. Plug-in support and Helm ease troubleshooting at the network level (like packet and DNS issues) across platforms (AKS, EKS, and more). A detailed AKS reference shows how to build multi-region clusters covering routing, geo-storage, security, and automated deployment. Azure Container Registry (ACR) tenant migration guidance continues last week’s focus on multi-tenant transition.

- [Retina 1.0 Released: Kubernetes Network Observability with Azure Monitor Integration](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/retina-1-0-is-now-available/ba-p/4489003)
- [Reference Architecture for Highly Available Multi-Region Azure Kubernetes Service (AKS)](https://techcommunity.microsoft.com/t5/azure-architecture-blog/reference-architecture-for-highly-available-multi-region-azure/ba-p/4490479)
- [Migrating Azure Container Registry (ACR) Between Azure AD Tenants: Step-by-Step Process](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/aks-tenant-migration-considerations-and-approach/ba-p/4415198)

### Real-Time Analytics and Data Engineering

OneLake and Snowflake now offer native Iceberg table interoperability, supporting fast two-way analytics integration. New Eventstream connectors expand to HTTP, MongoDB CDC, and weather feeds, with users now able to code streams with SQL Operators, adjust schemas, and improve pipeline security. Integration with SAP creates new options for real-time monitoring workflows. These enhancements complement last week's event handling and on-premises gateway updates.

- [Microsoft OneLake and Snowflake Interoperability Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/microsoft-onelake-and-snowflake-interoperability-is-now-generally-available/)
- [What's New in Fabric Eventstream: July–December 2025 Updates](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-eventstream-july-december-2025-updates/)
- [Unlock Real-Time Insights from SAP with Fabric Real-Time Intelligence](https://blog.fabric.microsoft.com/en-US/blog/unlock-real-time-insights-from-sap-with-fabric-real-time-intelligence/)

### AI-Ready Databases and Developer Integration

Azure PostgreSQL now adds a Visual Studio Code provisioning tool, supports secure Entra ID logins, and connects to Azure Monitor for better tracking. Copilot integration allows for natural language SQL, easy debugging, and LLM queries using Foundry and MCP, so teams can build AI, search, and analytics solutions faster. Upgraded SKUs, cluster support, and PostgreSQL 18 add power and flexibility. Nasdaq’s Boardvantage demonstrates secure AI-powered document workflows using these features.

- [PostgreSQL on Azure Supercharged for AI-Driven Applications](https://azure.microsoft.com/en-us/blog/postgresql-on-azure-supercharged-for-ai/)

### Enterprise Data Warehousing and Migration

Migrating from Synapse SQL Pools to Microsoft Fabric Data Warehouse is supported by new tests that show big cost and speed gains for large datasets (>10TB). Migration help includes stepwise guides, best practices, and validation tools. Community events (FabCon Atlanta, SQLCon) provide direct advice and early product information.

- [Microsoft Fabric Data Warehouse vs Azure Synapse: ESG Validation and Migration Guidance](https://blog.fabric.microsoft.com/en-US/blog/a-turning-point-for-enterprise-data-warehousing/)
- [FabCon Atlanta—Data Warehouse Edition: Explore the Future of Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/33276/)

### Platform Automation, Security, and Networking

Azure Automated VM Recovery improves uptime by finding and fixing more types of failures and adding monitoring tags. Azure NetApp Files’ Elastic Zone-Redundant configuration adds live sync and failover for NFS/SMB, boosting resiliency. NAT Gateway (StandardV2) now supports outbound flow logging, making monitoring easier. Microsoft will soon offer MANA hardware adapters for all VM types, boosting throughput and simplifying upgrades.

- [Azure Automated Virtual Machine Recovery: Minimizing Downtime](https://techcommunity.microsoft.com/t5/azure-compute-blog/azure-automated-virtual-machine-recovery-minimizing-downtime/ba-p/4483166)
- [Enhanced Storage Resiliency with Azure NetApp Files Elastic Zone-Redundant Service](https://azure.microsoft.com/en-us/blog/enhanced-storage-resiliency-with-azure-netapp-files-elastic-zone-redundant-service/)
- [Unlock Outbound Traffic Insights with Azure StandardV2 NAT Gateway Flow Logs](https://techcommunity.microsoft.com/t5/azure-networking-blog/unlock-outbound-traffic-insights-with-azure-standardv2-nat/ba-p/4493138)
- [Announcing Microsoft Azure Network Adapter (MANA) Support for Existing VM SKUs](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/announcing-microsoft-azure-network-adapter-mana-support-for/ba-p/4493279)

### Developer Experience and Workflow Enhancements

The Azure CLI Windows MSI installer now upgrades faster and is more reliable, fixing earlier issues and decreasing setup times by 23%. Dev containers auto-load azd extensions for better onboarding. A recap video reviews Application Gateway changes, VM updates, new container options, and Databricks announcements, offering a quick summary for developers.

- [Azure CLI Windows MSI Upgrade Failures and Performance Fixes: Root Causes and Solutions](https://techcommunity.microsoft.com/t5/azure-tools-blog/azure-cli-windows-msi-upgrade-issue-root-cause-mitigation-and/ba-p/4491691)
- [Auto-install `azd` Extensions in Dev Containers](https://devblogs.microsoft.com/azure-sdk/azd-devcontainer-extensions/)
- [Azure Update - 6th February 2026](/azure-update-6th-february-2026)

### AI Platform Foundations and Hub-and-Spoke Architectures

A new enterprise reference deployment shows how to prepare Azure for large-scale AI by using hub-and-spoke landing zones, RBAC, compliance standards, shared services, and API networks. Recommendations for Bicep/Terraform, secure network design, and automation are given for system engineers.

- [Building an Enterprise-Ready Azure AI Hub-and-Spoke Landing Zone](https://techcommunity.microsoft.com/t5/azure-architecture-blog/architecting-an-azure-ai-hub-and-spoke-landing-zone-for-multi/ba-p/4491161)

### Data Integration, Storage, and Backup

A deep dive into blob storage explains each tier (Hot, Cool, Cold, Archive), covering best practices for data retention, lifecycle automation, and recovery steps for architects and IT operations.

- [Azure Blob Tiering: Clarity, Truths, and Practical Guidance for Architects](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/azure-blob-tiering-clarity-truths-and-practical-guidance-for/ba-p/4493156)

### Azure Databricks and Analytics Platform

Azure Databricks Serverless Workspaces have reached general availability, offering managed compute, instant setup, role separation, and workspace management. Classic workspaces remain supported for custom deployments. Catalog mirroring support for private endpoint scenarios is also GA, giving regulated customers a secure way to sync data between clusters/services.

- [Serverless Workspaces Now Generally Available in Azure Databricks](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/serverless-workspaces-are-generally-available-in-azure/ba-p/4491314)
- [Mirroring Azure Databricks Catalogs Behind Private Endpoints Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/mirroring-azure-databricks-catalogs-from-azure-databricks-workspaces-behind-private-endpoints-generally-available/)

### High-Performance Computing and Observability

A step-by-step integration shows how to export ReFrame HPC metrics to Azure Log Analytics (using Bicep and JSON), enabling broad monitoring and analysis through Kusto queries. Another piece outlines how to connect the Azure SRE Agent to Dynatrace MCP for detailed monitoring, root cause, and vulnerability tracking.

- [Centralizing Cluster Performance Metrics with ReFrame HPC and Azure Log Analytics](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/centralized-cluster-performance-metrics-with-reframe-hpc-and/ba-p/4488077)
- [Integrate Azure SRE Agent with Dynatrace MCP for Observability](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-dynatrace-mcp-server-in-azure-sre-agent/ba-p/4492363)

### Microsoft Fabric Data Warehouse

Two articles outline Microsoft Fabric Data Warehouse’s performance, cost, and migration path (compared to Synapse), as well as practical help for deployment at FabCon Atlanta. These explain optimization tips for developers planning a move.

- [Microsoft Fabric Data Warehouse vs Azure Synapse: ESG Validation and Migration Guidance](https://blog.fabric.microsoft.com/en-US/blog/a-turning-point-for-enterprise-data-warehousing/)
- [FabCon Atlanta—Data Warehouse Edition: Explore the Future of Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/33276/)

### Other Azure News

Developer tool updates include improved CLI installers, extension support in containers, and video recaps addressing VM, security, container, and Databricks changes—these maintain last week’s CLI and workflow focus.

- [Azure CLI Windows MSI Upgrade Failures and Performance Fixes: Root Causes and Solutions](https://techcommunity.microsoft.com/t5/azure-tools-blog/azure-cli-windows-msi-upgrade-issue-root-cause-mitigation-and/ba-p/4491691)
- [Auto-install `azd` Extensions in Dev Containers](https://devblogs.microsoft.com/azure-sdk/azd-devcontainer-extensions/)
- [Azure Update - 6th February 2026](/azure-update-6th-february-2026)

Security updates include storage redundancy, new NAT flow logs, and automated VM recoveries to enhance resilience and meet compliance.

- [Enhanced Storage Resiliency with Azure NetApp Files Elastic Zone-Redundant Service](https://azure.microsoft.com/en-us/blog/enhanced-storage-resiliency-with-azure-netapp-files-elastic-zone-redundant-service/)
- [Unlock Outbound Traffic Insights with Azure StandardV2 NAT Gateway Flow Logs](https://techcommunity.microsoft.com/t5/azure-networking-blog/unlock-outbound-traffic-insights-with-azure-standardv2-nat/ba-p/4493138)
- [Azure Automated Virtual Machine Recovery: Minimizing Downtime](https://techcommunity.microsoft.com/t5/azure-compute-blog/azure-automated-virtual-machine-recovery-minimizing-downtime/ba-p/4483166)

Migration and troubleshooting resources for ACR and CLI support efficient platform changes.

- [Migrating Azure Container Registry (ACR) Between Azure AD Tenants: Step-by-Step Process](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/aks-tenant-migration-considerations-and-approach/ba-p/4415198)
- [Azure CLI Windows MSI Upgrade Failures and Performance Fixes: Root Causes and Solutions](https://techcommunity.microsoft.com/t5/azure-tools-blog/azure-cli-windows-msi-upgrade-issue-root-cause-mitigation-and/ba-p/4491691)

## Coding

Recent coding highlights include updated AI features, platform improvements, and evolving best practices. VS Code and .NET continue to offer better tools for developers, while documentation and community resources ease migration and onboarding.

### Visual Studio Code: Workflow, Multi-Agent, Terminal, and Beginner Experience

VS Code is now more agent-focused, continuing last week’s MCP Apps progress with better local, cloud, and background agent handling. The improved Agent Sessions Welcome Page and subagent tasking features help automate and delegate work, while upgraded UIs make agent management easier.

The January 2026 Insiders Update (v1.109) includes notable changes to the terminal (such as kitty protocol support and better input handling), improved formatting, bug fixes, updated command handling, and enhanced API access. Chat features retain context even after archiving, and custom UI support for MCP Apps gets a boost. Official tutorials walk new users through core features—including IntelliSense, issue management, and setup—connecting with the multi-agent and MCP functionality covered last week.

- [Multi-agent Development in VS Code](/multi-agent-development-in-vs-code)
- [Visual Studio Code January 2026 Insiders Update (v1.109): Terminal and Chat Improvements](https://code.visualstudio.com/updates/v1_109)
- [Learn Visual Studio Code in 15 Minutes: Official Beginner Tutorial](/learn-visual-studio-code-in-15-minutes-official-beginner-tutorial)
- [Learn Visual Studio Code in 15 Minutes: 2026 Official Beginner Tutorial](/learn-visual-studio-code-in-15-minutes-2026-official-beginner-tutorial)

### .NET Platform: Community-Driven Libraries, Roadmaps, and Deployment Changes

The .NET Data Community Standup introduces Microsoft.Extensions.DataIngestion for parsing documents and handling structured and vector workflows, supporting AI/ML cases as seen in recent community projects. Included demos and architecture advice shape next steps.

Blazor Community Standup walks through the .NET 11 ASP.NET Core and Blazor roadmap, continuing last week’s modernization efforts. .NET Framework 3.5 now requires a separate installer in new Windows releases (as of Insider Build 27965), with full support ending in 2029, so teams should start updating. The .NET MAUI Standup discusses hybrid development and ongoing plans, keeping focus on flexible deployment options.

- [.NET Data Community Standup: Introduction to Microsoft.Extensions.DataIngestion](/net-data-community-standup-introduction-to-microsoftextensionsdataingestion)
- [Blazor Community Standup: ASP.NET Core & Blazor Roadmap for .NET 11](/blazor-community-standup-aspnet-core-and-blazor-roadmap-for-net-11)
- [.NET Framework 3.5 Now Requires Standalone Deployment on New Windows Versions](https://devblogs.microsoft.com/dotnet/dotnet-framework-3-5-moves-to-standalone-deployment-in-new-versions-of-windows/)
- [.NET MAUI Community Standup: Live from MAUI Day London](/net-maui-community-standup-live-from-maui-day-london)

### Visual Studio and Editor Extensibility

Visual Studio 2026 supports background loading for MEF-based productivity extensions, shortening startup times and improving reliability. The new Microsoft.VisualStudio.SDK.Analyzers package (v17.7.98) helps detect and correct threading issues. Enable the related feature flag and follow new guides and code reviews for easier async use with extensibility code.

- [Performance Improvements for MEF-Based Visual Studio 2026 Extensions](https://devblogs.microsoft.com/visualstudio/performance-improvements-to-mef-based-editor-productivity-extensions/)

### Software Development Trends and Open Source Tooling

GitHub’s Octoverse 2025 shows TypeScript now leads Python and JavaScript in overall usage—driven partly by its better support for AI code suggestions. Project maintainers should add typing details for safer AI-assisted workflows. Python remains dominant for AI, but more projects are updating packaging, build, typing, release, and container practices to transition from prototyping to production.

Interest in tools like astral-sh/uv highlights the community’s desire for fast, repeatable deployments. Strong documentation and onboarding (e.g., VS Code and First Contributions) support sustainable open source growth and easier onboarding for new contributors.

- [What the Fastest-Growing Tools Reveal About Modern Software Development](https://github.blog/news-insights/octoverse/what-the-fastest-growing-tools-reveal-about-how-software-is-being-built/)

### Observability, Diagnostics, and Application Instrumentation in .NET

Andrew Lock reviews source generators for .NET metric instrumentation using Microsoft.Extensions.Telemetry.Abstractions and System.Diagnostics.Metrics. Source generators reduce repetitive code for metrics but sometimes have limits compared to manual instrumentation. The review offers examples to help devs evaluate trade-offs for reliability and monitoring.

- [Evaluating System.Diagnostics.Metrics Source Generators with Microsoft.Extensions.Telemetry.Abstractions](https://andrewlock.net/creating-strongly-typed-metics-with-a-source-generator/)

### Other Coding News

A technical walk-through looks at updating a WPF WebView2 control when data changes, including cache refresh strategies and how to automate reloads or use the DevTools Protocol for consistency.

- [Reliably Refreshing the WebView2 Control in WPF Applications](https://weblog.west-wind.com/posts/2026/Feb/04/Reliably-Refreshing-the-WebView2-Control)

## DevOps

Current DevOps improvements include faster environment setup, scalable CI/CD features, better supply chain controls, and enhanced review experience in GitHub and Windows workflows.

### GitHub Actions: Runner Updates, Scale Sets, and Enhanced Workflow Features

The deadline for upgrading self-hosted runners is extended to March 16, 2026, allowing more teams to keep pipelines secure while updating. Runner Scale Set Client (Go-based) is now in public preview and supports autoscaling on more platforms beyond Kubernetes. Action allow-listing works across all repo types, giving teams more control. New runner images for Server 2025 and macOS 26 Intel are available, and updates to editing, debug, and YAML features add further workflow flexibility.

- [GitHub Actions: Enforcement of Minimum Self-hosted Runner Version Extended to March 16, 2026](https://github.blog/changelog/2026-02-05-github-actions-self-hosted-runner-minimum-version-enforcement-extended)
- [GitHub Actions February 2026 Updates: Scale Sets, Security, and New Runner Images](https://github.blog/changelog/2026-02-05-github-actions-early-february-2026-updates)
- [The Download: OpenClaw AI Agents, Babel 7 Farewell, and GitHub Actions Updates](/the-download-openclaw-ai-agents-babel-7-farewell-and-github-actions-updates)

### WinGet Configuration: One-command Developer Environments

WinGet Configuration lets Windows users automate installation, settings, and toolchains from YAML files with a single command. It checks and applies only changes needed, supports developer mode, Visual Studio add-ons, logic for versioning, OS targeting, and infra setup, and integrates with PowerShell DSC. Version-controlling configs helps with setup and disaster recovery. CLI integration with Copilot speeds template creation, and Kayla Cinnamon’s walkthrough shows how to quickly set up reproducible developer environments.

- [WinGet Configuration: Set up your dev machine in one command](https://devblogs.microsoft.com/blog/winget-configuration-set-up-your-dev-machine-in-one-command)

### Open Source Ecosystem: Dependabot Proxy and Documentation Translation as Versioned Assets

Dependabot Proxy is now open sourced (MIT) and can update dependencies that require authentication for npm, Maven, Docker, NuGet, and Terraform. This adds visibility and auditability to supply chain automation for regulated teams.

Co-op Translator now stores documentation translations as versioned language files (JSON), making tracking changes against software releases easier. It helps contributors spot and fix out-of-date translations quickly.

- [Dependabot Proxy Now Open Source with MIT License](https://github.blog/changelog/2026-02-03-the-dependabot-proxy-is-now-open-source-with-an-mit-license)
- [Rethinking Documentation Translation: Treating Translations as Versioned Software Assets](https://techcommunity.microsoft.com/t5/microsoft-developer-community/rethinking-documentation-translation-treating-translations-as/ba-p/4491755)

### GitHub Review and Collaboration: Pull Request and Issue Experience

GitHub Mobile now lets users comment on any line of a file (even unchanged lines) in PRs, matching web review features. On desktop, UI improvements to the "Files Changed" view (including CODEOWNERS highlighting and faster navigation) assist with large or complicated reviews. GitHub Issues introduces pinned comments and a system that encourages reactions to keep discussions relevant and actionable.

- [GitHub Mobile: Comment on Unchanged Lines in Pull Request Files](https://github.blog/changelog/2026-02-03-github-mobile-comment-on-unchanged-lines-in-pull-request-files)
- [Improved Pull Request “Files Changed” – February 5 GitHub Updates](https://github.blog/changelog/2026-02-05-improved-pull-request-files-changed-february-5-updates)
- [Pinned Comments and Improved Comment Quality in GitHub Issues](https://github.blog/changelog/2026-02-05-pinned-comments-on-github-issues)

## Security

This week’s security notes cover changing malware threats, improved automation, new defensive features, and more robust AI/data security. Recent incidents span macOS, Windows, and web threats.

### Defender XDR and Threat Analysis Campaigns

Microsoft reports new infostealer malware (DigitStealer, AMOS, MacSync, PXA Stealer) now targeting both macOS and other platforms, using Python tooling and fake app bundles to steal logins. Teams get new Defender XDR and Sentinel detection patterns for Python attacks.

The CrashFix ClickFix campaign is spreading a Python RAT via fake Chrome extensions disguised as ad blockers, using PowerShell obfuscation and fileless tricks for persistence. Response details include Defender for Endpoint, Security Copilot, and updated detection queries.

Microsoft is responding to active SolarWinds Web Help Desk exploits (CVE-2025-40551, CVE-2025-40536, CVE-2025-26399), which use DLL tricks and lateral movement. Users are advised to patch, remove exposed endpoints, rotate sensitive accounts, and use KQL queries for incident detection.

- [Infostealer Malware Expands to macOS and Cross-Platform Targets: Defender XDR Insights and Mitigation](https://www.microsoft.com/en-us/security/blog/2026/02/02/infostealers-without-borders-macos-python-stealers-and-platform-abuse/)
- [CrashFix ClickFix Variant Deploys Python RAT via Browser Extension and Living-off-the-Land Tactics](https://www.microsoft.com/en-us/security/blog/2026/02/05/clickfix-variant-crashfix-deploying-python-rat-trojan/)
- [Analysis of Active Exploitation of SolarWinds Web Help Desk: Detection and Mitigation](https://www.microsoft.com/en-us/security/blog/2026/02/06/active-exploitation-solarwinds-web-help-desk/)

### Secure Development at Scale: Automation and Operational Guardrails

Dependabot now offers OIDC authentication for private package registries, so pipelines can request credentials dynamically using cloud provider tokens, reducing risks from static credentials. Azure, AWS, JFrog, and similar platforms are supported, following last week’s coverage of software supply chain safeguards.

Microsoft partners with the FBI’s Operation Winter SHIELD, expanding efforts to enforce technical controls (like secure baselines, legacy auth removal, MFA, and least privilege) using automation instead of just policy. Practical steps help teams apply stronger security in both legacy and new systems.

- [Dependabot Adds OIDC Authentication for Private Registries](https://github.blog/changelog/2026-02-03-dependabot-now-supports-oidc-authentication)
- [Closing the Security Implementation Gap: Microsoft’s Support for Operation Winter SHIELD](https://www.microsoft.com/en-us/security/blog/2026/02/05/the-security-implementation-gap-why-microsoft-is-supporting-operation-winter-shield/)

### AI and Data Security: Risks, Detection, and Lifecycle Evolution

Research from Microsoft looks at ways to detect hidden backdoors in open language models using attention analysis and pattern checking, including LoRA/QLoRA approaches. Teams are advised to combine static scanning with behavioral checks for AI deployment.

Microsoft’s evolving Secure Development Lifecycle adds AI requirements including threat modeling, prompt/poison checks, and better logging and role separation for collaboration and risk tracking.

Microsoft Fabric’s OneLake introduces centralized policy controls for data security, spanning analytic engines and formats for easier cross-cloud control and compliance management.

- [Detecting Backdoors in Open-Weight Language Models: Microsoft Research Insights](https://www.microsoft.com/en-us/security/blog/2026/02/04/detecting-backdoored-language-models-at-scale/)
- [Microsoft SDL: Evolving Security Practices for AI Systems](https://www.microsoft.com/en-us/security/blog/2026/02/03/microsoft-sdl-evolving-security-practices-for-an-ai-powered-world/)
- [The Future of Data Security is Interoperability: A Technical Look at OneLake Security](https://blog.fabric.microsoft.com/en-US/blog/the-future-of-data-security-is-interoperability-a-technical-look-at-onelake-security/)

### Other Security News

CodeQL 2.24.1 improves support for Maven registries, as well as Kotlin, Java, and Python scanning, and adds checks for buffer overflows, locks, and prompt injection risks (especially for Python LLM code). These improvements target clearer reporting and fewer false positives.

A new guide explains how to safely handle and validate user input, including special characters like apostrophes, to prevent SQL injection for .NET, Python, and Bicep, making the advice actionable for both software and infrastructure teams.

- [What’s New in CodeQL 2.24.1: Enhanced Maven Registry Support and Improved Query Accuracy](https://github.blog/changelog/2026-02-06-codeql-2-24-1-improves-maven-private-registry-support-and-improves-query-accuracy)
- [Handling Special Characters in User Input: A Developer’s Guide](https://zure.com/blog/dear-developers-stop-rejecting-me)
