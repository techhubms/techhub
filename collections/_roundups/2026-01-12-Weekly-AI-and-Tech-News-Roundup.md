---
layout: post
title: Agentic AI, GitHub Copilot Updates, Azure Platform Features, and Improving Secure Cloud Workflows
author: Tech Hub Team
viewing_mode: internal
date: 2026-01-12 09:00:00 +00:00
permalink: /all/roundups/Weekly-AI-and-Tech-News-Roundup
tags:
- .NET
- Agentic AI
- AI Driven Automation
- C#
- Cloud Cost Management
- Cloud Security
- High Performance Computing
- IDE Integration
- Microsoft Fabric
- Multi Agent Orchestration
- Open Source
- Semantic Search
section_names:
- ai
- github-copilot
- ml
- azure
- coding
- devops
- security
---
Welcome to the weekly tech update, where agentic AI and new developer tools are in focus. GitHub Copilot introduces expanded multi-model options and more customizable workflows for leading IDEs, while Azure releases new automation features, enhancements in high-performance computing, and easier developer experience. AI and machine learning continue to move toward interoperable and secure applications, bringing fresh options for context-driven workflows and scalable enterprise implementations. This week also highlights progress in DevOps automation, changes in cloud security practices, and open source releases that help teams build secure, productive, and adaptive systems.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot Expands Capabilities in Popular IDEs](#github-copilot-expands-capabilities-in-popular-ides)
  - [Copilot Customization and New Workflows in Visual Studio Code](#copilot-customization-and-new-workflows-in-visual-studio-code)
  - [Tutorials: Real-World, Agentic, and Spec-Driven Copilot Workflows](#tutorials-real-world-agentic-and-spec-driven-copilot-workflows)
  - [Analyses: Copilot’s Impact on Frameworks, Open Source, and Coding Standards](#analyses-copilots-impact-on-frameworks-open-source-and-coding-standards)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Agentic AI on Microsoft Platforms: Frameworks, Protocols, and Implementation](#agentic-ai-on-microsoft-platforms-frameworks-protocols-and-implementation)
  - [Patterns, Best Practices, and Orchestration for Multi-Agent AI](#patterns-best-practices-and-orchestration-for-multi-agent-ai)
  - [Agentic AI Solutions in Retail and Supply Chain Automation](#agentic-ai-solutions-in-retail-and-supply-chain-automation)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Local Embedding Generation in Fabric Eventhouse](#local-embedding-generation-in-fabric-eventhouse)
  - [Deep Learning for Autonomous Vehicles on Azure](#deep-learning-for-autonomous-vehicles-on-azure)
- [Azure](#azure)
  - [Azure Logic Apps: AI-Powered Automation, Migration, and Agentic Workflows](#azure-logic-apps-ai-powered-automation-migration-and-agentic-workflows)
  - [Azure CycleCloud and HPC Workflows: Monitoring, Security, and High-Performance File Distribution](#azure-cyclecloud-and-hpc-workflows-monitoring-security-and-high-performance-file-distribution)
  - [Azure Platform and Data Services Updates](#azure-platform-and-data-services-updates)
  - [Cloud-Native Architecture: AI, Mapping, and Real-Time Workflows](#cloud-native-architecture-ai-mapping-and-real-time-workflows)
  - [Datacenter Innovation and High-Scale AI Readiness](#datacenter-innovation-and-high-scale-ai-readiness)
  - [Azure Cost Estimation and Management Tools](#azure-cost-estimation-and-management-tools)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET Ecosystem: New Features, Platforms, and Roadmaps](#net-ecosystem-new-features-platforms-and-roadmaps)
  - [UI Development Tools and Open Source Progress](#ui-development-tools-and-open-source-progress)
  - [Language Trends and AI-Assisted Coding](#language-trends-and-ai-assisted-coding)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [Automated Detection of Alert Gaps in Azure Kubernetes Deployments](#automated-detection-of-alert-gaps-in-azure-kubernetes-deployments)
- [Security](#security)
  - [Exploiting Email Routing and Authentication Gaps in Microsoft 365](#exploiting-email-routing-and-authentication-gaps-in-microsoft-365)
  - [Centralized Security Governance in Microsoft Fabric](#centralized-security-governance-in-microsoft-fabric)

## GitHub Copilot

This week, GitHub Copilot introduced broader model access in leading IDEs and enabled more tailored workflow customization. These updates span both real-world developer scenarios and educational resources, highlighting Copilot's wider support in conventional programming environments and creative use cases.

### GitHub Copilot Expands Capabilities in Popular IDEs

With continued support for multiple agent types and improved model integration, the Gemini 3 Flash model for Copilot Chat is now available in public preview. Visual Studio (17.14.12+ and 18.1.0+), JetBrains IDEs (1.5.62+), Xcode (0.46.0+), Eclipse (0.14.0+), github.com, and GitHub Mobile now let users select Gemini 3 Flash via the chat model picker. This makes Copilot’s multi-model support more accessible in standard developer environments. Modes including chat, ask, edit, and agent are available in Visual Studio and JetBrains IDEs, while Xcode and Eclipse provide ask and agent options for custom workflows.

- [Gemini 3 Flash Model Now Accessible in GitHub Copilot Across Major IDEs](https://github.blog/changelog/2026-01-06-gemini-3-flash-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)

### Copilot Customization and New Workflows in Visual Studio Code

Extending the focus on project-specific workflows in VS Code, the December 2025 (v1.108) release adds Agent Skills for GitHub Copilot. Developers can save specialized knowledge in SKILL.md files within .github/skills and load them during Copilot chat sessions, supporting individualized guidance and project context.

Updates also bring improved navigation, session management, keyboard and git workflow support, and better debugging and accessibility. Copilot plugin development and issue handling gains new tooling for code quality and maintainability in both enterprise and open source environments.

- [Visual Studio Code December 2025 Release (v1.108): New Features and Improvements](https://code.visualstudio.com/updates/v1_108)

### Tutorials: Real-World, Agentic, and Spec-Driven Copilot Workflows

New tutorials demonstrate Copilot usage across hands-on projects and agent-driven approaches. The Space Invaders walkthrough shows Copilot integrating with Figma, implementing advanced gameplay mechanics, Playwright code reviews, and structured feature specs, supporting consistent team practices and automation with MCP. Azure MCP and CLI deployment strategies build on recent teamwork themes using intelligent agents.

A Copilot lab for Visual Studio Code explores agent mode, covering MCP server connections, custom agent development, and automated background tasks. These learning resources help developers move from basic completion to orchestrated multi-agent workflows, continuing the shift toward scalable, enterprise-grade Copilot implementations.

- [Building Space Invaders from Scratch with GitHub Copilot, AI Toolkit, and Azure Deployment](/2026-01-05-Building-Space-Invaders-from-Scratch-with-GitHub-Copilot-AI-Toolkit-and-Azure-Deployment.html)
- [Hands-On Lab: The Power of GitHub Copilot in Visual Studio Code](/2026-01-05-Hands-On-Lab-The-Power-of-GitHub-Copilot-in-Visual-Studio-Code.html)

### Analyses: Copilot’s Impact on Frameworks, Open Source, and Coding Standards

Analysis this week looks at how Copilot influences technology choices and engineering approaches. Articles discuss Copilot’s role in supporting React’s ongoing use, building on past themes about AI and statically typed languages and developer efficiency.

An interview with Homebrew maintainer Mike McQuaid spotlights open source automation and scaling strategies involving Copilot. Emphasis is placed on bug and documentation management through automated AI processes previously discussed in agentic and MCP concepts.

A case for moving beyond informal code styles to systematic, specification-led workflow development supports a disciplined approach to software engineering as AI becomes more integral to project delivery.

- [Will AI Make React Popular Forever?](/2026-01-06-Will-AI-Make-React-Popular-Forever.html)
- [Sustaining Homebrew: Leadership, Automation, and AI with Mike McQuaid](/2026-01-10-Sustaining-Homebrew-Leadership-Automation-and-AI-with-Mike-McQuaid.html)
- [From Vibe Coding to Spec-Driven Development: Why AI-Generated Code Needs Structure](https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development)

### Other GitHub Copilot News

This week’s case study highlights Microsoft Copilot helping Deaf creators. Min-ji So’s Webtoon Storyboard Assistant uses conversational AI for grammar suggestions and creative support, further demonstrating Copilot's accessibility and flexibility across industry and creative fields. These examples reinforce Copilot's growing role in enabling new workflows and fostering broad user impact.

- [Empowering Deaf Creators: Min-ji So's Journey with Microsoft Copilot and AI](https://news.microsoft.com/source/asia/features/a-deaf-writers-journey-with-ai-discovering-new-creative-paths/?lang=ko)

## AI

AI news continues the shift toward modular, production-ready agentic systems, with new resources for developers building context-aware, local-first agents and orchestration frameworks. This includes engineering guides for private studios, practical agent templates, open protocols, and multi-agent workflow practices in everyday coding and industry scenarios.

### Agentic AI on Microsoft Platforms: Frameworks, Protocols, and Implementation

Furthering past work with Agent Framework and context management, this week’s guide shows how to build local-first, privacy-focused podcast studios using Microsoft’s AI technology. Solutions use Python orchestration and edge deployments with Ollama, maintaining clear context boundaries and observability for agents.

For .NET developers, an updated guide details integration of Semantic Kernel, Microsoft AI Extensions, and OllamaSharp, outlining how interfaces like IChatClient support modular hybrid AI deployments locally and in the cloud.

The Agent-to-Agent Standard (A2AS) in .NET provides actionable steps for agent composability, including JSON-RPC 2.0, AgentCard metadata, and agent lifecycle management. These bring agent architectures closer to practical, standardized systems for reliable development.

- [Engineering a Local-First Agentic Podcast Studio with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/engineering-a-local-first-agentic-podcast-studio-a-deep-dive/ba-p/4482839)
- [Generative AI with Large Language Models in C# in 2026](https://devblogs.microsoft.com/dotnet/generative-ai-with-large-language-models-in-dotnet-and-csharp/)
- [Implementing the Agent-to-Agent (A2A) Protocol in .NET: A Practical Guide](https://techcommunity.microsoft.com/t5/microsoft-developer-community/implementing-a2a-protocol-in-net-a-practical-guide/ba-p/4480232)

### Patterns, Best Practices, and Orchestration for Multi-Agent AI

Multi-agent orchestration remains a priority, with new materials reviewing scalable architectures for collaborative agent automation. The Armchair Architects’ show analyzes Microsoft AutoGen, Semantic Kernel, and Agent Framework for resource scaling, security, and cost efficiency.

Topics include permissioning, security practices, and operational controls, building on last week's focus on context management. Practical recommendations for phased experimentation parallel previous advice for deploying multi-agent solutions in business.

A review of application modernization further explores human-in-the-loop design for safe AI adoption, confirming the ongoing need for robust practices in updating existing technology.

- [Armchair Architects: Patterns and Best Practices for Multi-Agent AI Orchestration](/2026-01-06-Armchair-Architects-Patterns-and-Best-Practices-for-Multi-Agent-AI-Orchestration.html)
- [The Realities of Application Modernization with Agentic AI: A 2026 Perspective](https://devblogs.microsoft.com/all-things-azure/the-realities-of-application-modernization-with-agentic-ai-early-2026/)

### Agentic AI Solutions in Retail and Supply Chain Automation

Microsoft launches updated agentic AI templates and retail automation tools, integrating Copilot Checkout and catalog enrichment agents in Azure and .NET environments. These fit a pattern of standardized automation and easy enterprise customization.

Workflow modularity supports mature enterprise tool practices and links internal engineering and customer-facing activities. Blue Yonder’s supply chain case study further exemplifies integration and transparency, building on previous themes around orchestration and business reliability for critical deployments.

- [Microsoft Launches Agentic AI Solutions to Transform Retail Automation and Personalization](https://news.microsoft.com/source/2026/01/08/microsoft-propels-retail-forward-with-agentic-ai-capabilities-that-power-intelligent-automation-for-every-retail-function/)
- [Store Operations That Scale: Turn Signals into Decisions](/2026-01-07-Store-Operations-That-Scale-Turn-Signals-into-Decisions.html)
- [AI-Driven Agents Transforming Supply Chain Management at Blue Yonder](/2026-01-09-AI-Driven-Agents-Transforming-Supply-Chain-Management-at-Blue-Yonder.html)

### Other AI News

Agent Skills in Visual Studio Code, previewed earlier in Copilot releases, now include expanded features. This allows more developers to author and test reusable automations within the IDE, highlighting the ongoing shift of customizable AI into daily development activities.

- [Introducing Agent Skills in VS Code](/2026-01-09-Introducing-Agent-Skills-in-VS-Code.html)

## ML

Machine learning updates focus on deploying AI securely and at scale. Microsoft adds new local embedding features for semantic search and retrieval augmented generation (RAG) in analytics and expands deep learning applications for autonomous vehicles through the cloud.

### Local Embedding Generation in Fabric Eventhouse

Microsoft enables text embedding creation in the Kusto Python sandbox within Fabric Eventhouse using Small Language Models (SLMs) such as jina-v2-small and e5-small-v2, via the slm_embeddings_fl() function.

Previously, developers needed Azure OpenAI endpoints for embeddings, which added dependency on remote APIs and could bring latency, cost, and privacy limitations. Now, local inference allows for lower overhead, reduced latency, and simpler compliance—improving scalability and automation for data processing teams.

Documentation provides step-by-step KQL and Python examples for embedding creation, real-time search, and automated processing, supporting efficient, secure AI adoption in Azure environments.

- [Create Embeddings in Fabric Eventhouse with Built-in Small Language Models (SLMs)](https://blog.fabric.microsoft.com/en-US/blog/create-embeddings-in-fabric-eventhouse-with-built-in-small-language-models-slms/)

### Deep Learning for Autonomous Vehicles on Azure

Wayve leverages Azure for distributed training and large-scale deployment of deep learning models in autonomous vehicles, extending advanced ML into connected mobility. Azure's infrastructure supports big data handling and fast model rollout across GPU and TPU clusters, building on cloud-enabled AI operations for industrial applications.

- [AI that drives change: Wayve rewrites self-driving playbook with deep learning in Azure](https://news.microsoft.com/source/emea/features/ai-that-drives-change-wayve-rewrites-self-driving-playbook-with-deep-learning-in-azure/)

## Azure

Recent Azure releases focus on improving developer experiences, automating workflows, expanding high-performance computing, and refining cost controls. Updates range from Logic Apps automation to Oracle Database@Azure extensions, better HPC monitoring, and data platform features.

### Azure Logic Apps: AI-Powered Automation, Migration, and Agentic Workflows

The January Aviators Newsletter details new Logic Apps improvements. BizTalk Server 2020 is set as the last release, with support until 2030, incentivizing migration to Logic Apps, and providing new tools for artifact reuse and migration.

New AI connectors add support for document intelligence, semantic search, and enhanced RAG workflows. The Data Mapper Test Executor allows CI/CD validation of data mapping, streamlining automated migration and data backup.

Practical workshops let developers assemble agentic automation solutions using Copilot Studio MCP servers and Logic Apps’ Agent Loop. Community resources cover error handling and SAP integration, positioning Logic Apps as the preferred option for BizTalk migration.

- [Logic Apps Aviators Newsletter - January 2026](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-aviators-newsletter-january-2026/ba-p/4482877)
- [Upcoming Free Workshops: Agentic Azure Logic Apps & Copilot Studio Integration](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/upcoming-agentic-azure-logic-apps-workshops/ba-p/4484526)

### Azure CycleCloud and HPC Workflows: Monitoring, Security, and High-Performance File Distribution

CycleCloud 2025.12.01 for Slurm introduces improved HPC management and monitoring. Prometheus agent and Managed Grafana support automated dashboards for clusters. New scripts simplify deployment, and added metrics such as memory, thermal, and power usage provide deeper insights.

Security upgrades include Entra ID single sign-on via OpenID Connect, with MFA and access roles for both CycleCloud and Open OnDemand. Expanded ARM64 support and new operating system compatibility increase flexibility.

The “mpi-stage” file broadcast tool accelerates file distribution across large GPU containers, addressing performance bottlenecks in cluster operations.

- [Azure CycleCloud Workspace for Slurm 2025.12.01: Monitoring, Security, and HPC Enhancements](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/announcing-azure-cyclecloud-workspace-for-slurm-version-2025-12/ba-p/4481953)
- [mpi-stage: High-Performance File Distribution for HPC Clusters](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/mpi-stage-high-performance-file-distribution-for-hpc-clusters/ba-p/4484366)

### Azure Platform and Data Services Updates

Azure expands Oracle Database@Azure to West Europe and Brazil Southeast, supporting regulated and high-performance workloads with Exadata, low-latency deployment, and flexible licensing.

Siemens NX can now be deployed on Azure virtual machines with AMD Radeon V710 GPUs, maximizing cost-effectiveness, global scale availability, and secure engineering environments.

Microsoft Fabric Data Warehousing releases cluster-aware features, upsert operations, identity columns, and new migration tools, all aimed to improve data security, handling, and migration processes.

The January platform update covers AKS-native calculators, expanded Premium SSDv2 disk support, Service Bus geo-replication, guidance for retiring resource providers and templates, and infrastructure modernization.

- [Oracle Database@Azure Expands to West Europe and Brazil Southeast](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/global-expansion-now-available-in-west-europe-netherlands/ba-p/4479671)
- [Deploying Siemens NX on Azure NVads V710 v5-Series with AMD Radeon GPUs](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-v710-v5-series-amd-radeon-gpu-validation-of-siemens-cad-nx/ba-p/4483791)
- [Azure Update - 9th January 2026](/2026-01-09-Azure-Update-9th-January-2026.html)
- [Major Innovations in Microsoft Fabric Data Warehousing: 2025 Overview](/2026-01-07-Major-Innovations-in-Microsoft-Fabric-Data-Warehousing-2025-Overview.html)

### Cloud-Native Architecture: AI, Mapping, and Real-Time Workflows

New resources include a walkthrough of AI features for Azure Managed PostgreSQL, covering semantic search, embedding generation, and Copilot code support within VS Code and Foundry.

A game backend case study details how to use Azure Web PubSub for scalable, real-time online RPG event processing, replacing polling with event-driven architectures for improved reliability.

A feature on Azure Maps clarifies the differences between view and routing coordinates to help avoid mapping and navigation bugs.

SharePoint architects get a new guide for flat site collections and navigation, tied into Azure AD, Power Platform, and Purview governance best practices.

- [Exploring AI Features in Azure Managed PostgreSQL](/2026-01-05-Exploring-AI-Features-in-Azure-Managed-PostgreSQL.html)
- [Building scalable, cost-effective real-time multiplayer games with Azure Web PubSub](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-scalable-cost-effective-real-time-multiplayer-games/ba-p/4483584)
- [Azure Maps: Understanding View vs. Routing Coordinates](https://techcommunity.microsoft.com/t5/azure-maps-blog/azure-maps-understanding-view-vs-routing-coordinates/ba-p/4483532)
- [Designing a Future-Proof SharePoint Information Architecture](https://dellenny.com/how-to-design-a-future-proof-sharepoint-information-architecture/)

### Datacenter Innovation and High-Scale AI Readiness

Azure partners with NVIDIA to upgrade datacenter architecture for large-scale Rubin platform work, including new storage networks, advanced cooling systems, modular rollout, fast resource management, and NVLink/ConnectX-9 networking.

These improvements support faster model training, improved hardware utilization, and more efficient orchestration for AI, ML, and HPC workloads.

- [How Azure Datacenter Strategy Powers NVIDIA Rubin Platform Deployments](https://azure.microsoft.com/en-us/blog/microsofts-strategic-ai-datacenter-planning-enables-seamless-large-scale-nvidia-rubin-deployments/)

### Azure Cost Estimation and Management Tools

A new guide outlines strategies for estimating and optimizing Azure costs using built-in calculators, Advisor, Copilot, and management portals. Steps include linking architecture decisions to budget forecasts and maintaining monitoring for ongoing efficiency.

Interactive AMA sessions provide answers on cloud pricing and management for practical cost control, reinforcing Azure’s support for financial management and modernization.

- [Azure Cost Estimation: Your Strategic Guide to Cloud Pricing](https://www.thomasmaurer.ch/2026/01/azure-cost-estimation-your-strategic-guide-to-cloud-pricing/)
- [Live AMA: Demystifying Azure Pricing (AM Session)](https://techcommunity.microsoft.com/t5/azure-events/live-ama-demystifying-azure-pricing-am-session/ec-p/4483196#M665)
- [Live AMA: Demystifying Azure Pricing (PM Session)](https://techcommunity.microsoft.com/t5/azure-events/live-ama-demystifying-azure-pricing-pm-session/ec-p/4483198#M666)

### Other Azure News

There is an ongoing discussion regarding platform-driven VM redeployments on North Europe v6 series due to host OS and firmware maintenance, reminding teams to monitor service health and mitigate potential workload disruptions.

- [Frequent Platform-Initiated Azure VM Redeployments (v6 Series) in North Europe](https://techcommunity.microsoft.com/t5/azure-compute/frequent-platform-initiated-vm-redeployments-v6-in-north-europe/m-p/4484455#M837)

## Coding

This week’s coding news highlights .NET and C# feature releases, advances in modern UI development, cross-platform tooling, and the evolving use of AI-assisted programming languages.

### .NET Ecosystem: New Features, Platforms, and Roadmaps

The .NET ecosystem receives new updates on platform support and development features. MongoDB EF Core adds Queryable Encryption and Vector Search to create secure, privacy-friendly apps with advanced semantic search options.

.NET Community Standups discuss roadmap progress. Blazor Standup previews features coming in .NET 11, with an open invitation to shape web UI planning and migration.

Cross-platform development gains more focus. Avalonia Standup demonstrates Avalonia UI as a Linux backend for .NET MAUI, answering requests for broader deployment options.

Distributed app development is featured in Orleans sessions, diving into scalable architecture patterns for maintainable cloud-native applications.

- [Secure and Intelligent: Queryable Encryption and Vector Search in MongoDB EF Core Provider](https://devblogs.microsoft.com/dotnet/mongodb-efcore-provider-queryable-encryption-vector-search/)
- [Blazor Community Standup: Planning the Future of Blazor in .NET 11](/2026-01-06-Blazor-Community-Standup-Planning-the-Future-of-Blazor-in-NET-11.html)
- [.NET MAUI on Linux with Avalonia: Community Standup Recap](/2026-01-07-NET-MAUI-on-Linux-with-Avalonia-Community-Standup-Recap.html)
- [Orleans Deep Dive: Routing, Placement & Balancing](/2026-01-05-Orleans-Deep-Dive-Routing-Placement-and-Balancing.html)
- [ASP.NET Community Standup: What's Next for Orleans?](/2026-01-09-ASPNET-Community-Standup-Whats-Next-for-Orleans.html)

### UI Development Tools and Open Source Progress

Microsoft open-sources XAML Studio, inviting contributions to fill gaps in Visual Studio designer functionality highlighted during recent developer discussions. The .NET Foundation release focuses on live, visual design for WinUI and MAUI.

Community conversations continue on sustaining frameworks like WinForms and WPF and the need for unified designer experiences. The project activates open innovation within the .NET UI ecosystem.

- [Microsoft Open Sources XAML Studio and Highlights Visual Studio Designer Challenges](https://devclass.com/2026/01/07/microsoft-open-sources-xaml-studio-amid-developer-discontent-with-visual-studio-designers/)

### Language Trends and AI-Assisted Coding

Analysis of programming language usage reveals Copilot’s effect on adoption of typed languages to reduce AI code issues. This backs up previous trends showing TypeScript’s rise and a movement toward stronger type systems for reliable developer contracts.

Tips on exploring gradually typed languages and testing Copilot CLI match ongoing efforts to improve maintainability as AI tools become integral to coding practices.

- [Why AI Is Pushing Developers Toward Typed Languages](https://github.blog/ai-and-ml/llms/why-ai-is-pushing-developers-toward-typed-languages/)

### Other Coding News

VS Live! 2026 preview features accessible learning in .NET, Visual Studio, and AI development. Labs demonstrate Copilot debugging and modern DevOps, delivering practical community training resources.

- [Immersive Developer Learning with Visual Studio, .NET, Azure, and GitHub Copilot: VS Live! 2026 Preview](https://devblogs.microsoft.com/visualstudio/vs-live-2026-immersive-learning-for-vs2026/)

## DevOps

DevOps topics this week highlight monitoring complexity in cloud-native systems and the adoption of automated detection for missed alerts, supporting more reliable production engineering.

### Automated Detection of Alert Gaps in Azure Kubernetes Deployments

A new case study addresses alerting gaps in AKS deployments. Last week’s SRE Agent worked on debugging and remediation with MCP; now it expands to audit for missed alert coverage.

The report analyzes a Redis credential rotation incident where essential failures went unreported due to narrow alert criteria focused on resource usage. Blocked Redis connections caused outages not evident in current alerting rules.

Deploying the Azure SRE Agent with GitHub MCP improves alert coverage by discovering missing synthetic probes, weak ingress alerts, and absent pod-specific checks. The agent, using broad permissions, correlates AKS diagnostics, Log Analytics, and infrastructure code.

Building on previous AI-driven incident response, SRE Agent now suggests improved KQL and Bicep configurations and automatically creates actionable GitHub issues.

Subagent setup instructions and directions for Log Analytics/CLI integration are provided for teams looking to strengthen monitoring and resilience.

- [Identifying Missed Alerts in Azure Kubernetes Deployments with SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/find-the-alerts-you-didn-t-know-you-were-missing-with-azure-sre/ba-p/4483494)

## Security

Security updates feature ongoing phishing threat analysis for Microsoft 365 and new centralized data governance features in Microsoft Fabric.

### Exploiting Email Routing and Authentication Gaps in Microsoft 365

Microsoft’s threat research details how attackers use mail routing gaps and weak authentication (SPF, DKIM, DMARC) to launch phishing. Methods include spoofing, manipulation of names and sender data, and PhaaS kits like Tycoon2FA targeting defenses.

Recommendations build on recent discussions of layered security. Enforcing strict authentication, reviewing mail flow, and leveraging Defender features are reinforced, together with stronger multi-factor authentication using Entra ID.

For security teams, the report offers practical Kusto queries, guidance for Defender XDR and Sentinel, steps for credential and rule resets, and links to automation resources.

- [Spoofed Phishing Emails Exploiting Routing and Protection Misconfigurations](https://www.microsoft.com/en-us/security/blog/2026/01/06/phishing-actors-exploit-complex-routing-and-misconfigurations-to-spoof-domains/)

### Centralized Security Governance in Microsoft Fabric

Microsoft Fabric centralizes security reporting in OneLake Catalog’s Govern tab, now covering sensitivity labeling across Lakehouses, Warehouses, and Reports, and identifying potentially unprotected data. Admins see compliance status and scan history for prompt remediation.

Copilot aids investigation, supporting more effective response to policy violations. Teams are advised to transition from Purview Hub reporting, as Microsoft plans to retire the old reporting system in 2026.

- [Enhanced Security Governance in Microsoft Fabric: Admin Report Now in OneLake Catalog Govern Tab](https://blog.fabric.microsoft.com/en-US/blog/explore-your-fabric-security-insights-in-the-onelake-catalog-govern-tab/)
