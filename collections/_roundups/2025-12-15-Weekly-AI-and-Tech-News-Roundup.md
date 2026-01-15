---
layout: post
title: 'Streamlining Development: Updates in Copilot, AI Platforms, and Secure Automation'
author: Tech Hub Team
viewing_mode: internal
date: 2025-12-15 09:00:00 +00:00
permalink: /all/roundups/Weekly-AI-and-Tech-News-Roundup
tags:
- Agentic AI
- AI
- AI Agents
- Application Modernization
- Azure
- CI/CD
- Cloud Automation
- Coding
- Developer Productivity
- DevOps
- GitHub Copilot
- Machine Learning
- Microsoft Fabric
- ML
- OpenAI GPT 5.2
- Roundups
- Security
- Supply Chain Security
- VS Code
section_names:
- ai
- github-copilot
- ml
- azure
- coding
- devops
- security
---
Welcome to this week's technology roundup covering progress in intelligent, secure, and connected development environments. GitHub Copilot introduced public previews of new AI models—OpenAI's GPT-5.2, Claude Opus, and Gemini 3 Pro—alongside expanded model selection and deeper agent workflows. Microsoft expanded AI platform capabilities with agentic workflows, persistent memory, and the Model Context Protocol joining the Linux Foundation for better interoperability. Security and DevOps coverage addresses supply chain risks, credential management, and endpoint security.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot: AI Model Expansion and Selection](#github-copilot-ai-model-expansion-and-selection)
  - [Deep Copilot Integration Across Developer Workflows](#deep-copilot-integration-across-developer-workflows)
  - [Copilot in Modernization and App Transformation](#copilot-in-modernization-and-app-transformation)
  - [Enhancing CLI and Terminal Developer Environments](#enhancing-cli-and-terminal-developer-environments)
  - [Copilot Ecosystem: Administration, Education, and Workflow Guides](#copilot-ecosystem-administration-education-and-workflow-guides)
- [AI](#ai)
  - [Microsoft Foundry and Agentic AI Platforms](#microsoft-foundry-and-agentic-ai-platforms)
  - [GPT-5.2 Rollout and Integration](#gpt-52-rollout-and-integration)
  - [Model Context Protocol (MCP) and Agentic Interoperability](#model-context-protocol-mcp-and-agentic-interoperability)
  - [AI Integration in Developer Workflows and Cloud Automation](#ai-integration-in-developer-workflows-and-cloud-automation)
  - [Microsoft Fabric and Real-Time Intelligence for AI](#microsoft-fabric-and-real-time-intelligence-for-ai)
  - [Agentic Business Applications and Autonomous AI](#agentic-business-applications-and-autonomous-ai)
  - [Advances in AI Procurement and Developer Productivity](#advances-in-ai-procurement-and-developer-productivity)
  - [Multi-Model Reasoning, Open Source, and AI Developer Research](#multi-model-reasoning-open-source-and-ai-developer-research)
- [ML](#ml)
  - [Microsoft Fabric Ecosystem: Engineering, Data Quality, and Automation](#microsoft-fabric-ecosystem-engineering-data-quality-and-automation)
  - [Reinforcement Learning in AI Agents: Agent Lightning Open Source Release](#reinforcement-learning-in-ai-agents-agent-lightning-open-source-release)
  - [AI for Biomedical Workflows: GigaTIME Spatial Proteomics Platform](#ai-for-biomedical-workflows-gigatime-spatial-proteomics-platform)
- [Azure](#azure)
  - [Microsoft Fabric and SQL Data Platform](#microsoft-fabric-and-sql-data-platform)
  - [Azure Container Apps: Deployment, Autoscaling, and Capacity Management](#azure-container-apps-deployment-autoscaling-and-capacity-management)
  - [Azure Storage: AI Capabilities and Migration Tools](#azure-storage-ai-capabilities-and-migration-tools)
  - [Observability, Reliability, and DevOps on Azure](#observability-reliability-and-devops-on-azure)
  - [Microsoft Fabric Extensibility and Logic Apps: Integration and Testability](#microsoft-fabric-extensibility-and-logic-apps-integration-and-testability)
  - [AI Agent Backend Development with Model Context Protocol on Azure Functions](#ai-agent-backend-development-with-model-context-protocol-on-azure-functions)
  - [Migration, Modernization, and End-to-End Developer Guides](#migration-modernization-and-end-to-end-developer-guides)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET 10 Platform Developments and Servicing Updates](#net-10-platform-developments-and-servicing-updates)
  - [Cross-Platform Client Development with .NET MAUI](#cross-platform-client-development-with-net-maui)
  - [Windows PC Gaming Development: Tools and Platform Advancements](#windows-pc-gaming-development-tools-and-platform-advancements)
  - [Visual Studio Code: Agent HQ, TypeScript 7, and Copilot Changes](#visual-studio-code-agent-hq-typescript-7-and-copilot-changes)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [GitHub Platform and Workflow Enhancements](#github-platform-and-workflow-enhancements)
  - [GitHub Actions Evolution and Runner Management](#github-actions-evolution-and-runner-management)
  - [Azure DevOps Server and Enterprise Migration](#azure-devops-server-and-enterprise-migration)
  - [Cloud-Native DevOps with Azure MCP and Agent Management](#cloud-native-devops-with-azure-mcp-and-agent-management)
- [Security](#security)
  - [npm Authentication, Supply Chain, and IDE Attack Surface](#npm-authentication-supply-chain-and-ide-attack-surface)
  - [Azure and Microsoft Stack Authentication & Access Management](#azure-and-microsoft-stack-authentication--access-management)
  - [Cloud Incident Response, Endpoint Security, and Email Protection](#cloud-incident-response-endpoint-security-and-email-protection)
  - [Securing AI, Advanced Defense Strategies, and Practical Security Modeling](#securing-ai-advanced-defense-strategies-and-practical-security-modeling)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot received updates this week that improve model selection, support for agent workflows, and integration with IDEs and command line tools. The ecosystem expanded with new management features and educational resources for tasks like legacy app updates and streamlined Git workflows.

### GitHub Copilot: AI Model Expansion and Selection

Model choices continue to expand, with OpenAI’s GPT-5.2 now in public preview for Pro, Business, and Enterprise users, following last week’s release of GPT-5.1-Codex-Max and Claude Opus 4.5. Developers can benefit from better long-context and UI generation, reinforcing Copilot’s strategy to provide flexible AI model options.

User-controlled options for selecting models are now available to more users. Bring-Your-Own-Key (BYOK) builds on earlier guides, supporting both organizational control and security. Automated model selection, now available in Visual Studio Code, transitions from manual settings to routing based on workload requirements, offering transparency on which models are in use.

In addition, Copilot Coding Agent’s model switcher is now available for Pro and Pro+ subscribers, extending agent management options from last week by enabling users to select between Claude and GPT-based agents. This provides further flexibility for personalized AI assistance in development workflows.

- [OpenAI’s GPT-5.2 Now Available in GitHub Copilot Public Preview](https://github.blog/changelog/2025-12-11-openais-gpt-5-2-is-now-in-public-preview-for-github-copilot)
- [Auto Model Selection in GitHub Copilot Now Available in Visual Studio Code](https://github.blog/changelog/2025-12-10-auto-model-selection-is-generally-available-in-github-copilot-in-visual-studio-code)
- [Model Picker Now Available for Copilot Coding Agent (Pro and Pro+ Subscribers)](https://github.blog/changelog/2025-12-08-model-picker-for-copilot-coding-agent-for-copilot-pro-and-pro-subscribers)
- [Exploring AI Models and GitHub Copilot's Raptor Mini in VS Code](/videos/2025-12-08-Exploring-AI-Models-and-GitHub-Copilots-Raptor-Mini-in-VS-Code.html)

### Deep Copilot Integration Across Developer Workflows

With the release of Visual Studio Code November 2025 Insiders, CLI and agent session management improved, continuing support for features introduced in earlier rounds. Copilot CLI now launches from the palette or toolbar, demonstrating registry and agent integration in action. The Agent HQ panel consolidates agent monitoring for both local and cloud environments and incorporates the recent Mission Control features for easier oversight.

Automated code review and chat capabilities in Visual Studio 2026 now help analyze code for quality and security concerns, making AI-based feedback more useful during pull requests and team collaboration. This continues progress from last week’s previews. New tutorials cover automation and agent workflows in DevOps, expanding on recent cases with detailed steps for migration, standards enforcement, and deployment automation.

- [What's New in Visual Studio Code November 2025 Insiders Release (v1.107)](https://code.visualstudio.com/updates/v1_107)
- [Agent HQ for VS Code: Unified Agent Experience with GitHub Copilot](/videos/2025-12-10-Agent-HQ-for-VS-Code-Unified-Agent-Experience-with-GitHub-Copilot.html)
- [Streamlining Your Git Workflow with Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/streamlining-your-git-workflow-with-visual-studio-2026/)
- [Continuous Efficiency: AI-Driven Software Optimization with GitHub Agentic Workflows](https://github.blog/news-insights/policy-news-and-insights/the-future-of-ai-powered-software-optimization-and-how-it-can-help-your-team/)
- [Tackling Tech Debt with the GitHub Copilot Cloud Agent](/2025-12-11-Tackling-Tech-Dev-with-the-GitHub-Copilot-Cloud-Agent.html)
- [Rubber Duck Thursdays: Using GitHub Copilot Custom Agents to Address Tech Debt](/videos/2025-12-09-Rubber-Duck-Thursdays-Using-GitHub-Copilot-Custom-Agents-to-Address-Tech-Debt.html)
- [Ship Faster: End-to-End Azure App Development with GitHub Copilot and Intelligent Agents](/videos/2025-12-10-Ship-Faster-End-to-End-Azure-App-Development-with-GitHub-Copilot-and-Intelligent-Agents.html)

### Copilot in Modernization and App Transformation

Content on application modernization builds on topics from the previous week, including refactoring, review, and migration strategies. The in-depth guide for migrating Microsoft Access illustrates how Copilot can help script and automate database migrations, reflecting agent orchestration concepts discussed earlier.

Visual Studio Code Insiders now offers a JavaScript/TypeScript App Modernizer preview, supporting interactive upgrades and extending recent automation coverage. Additional .NET modernization articles provide guidance on improving code quality, managing dependencies, and enhancing security in cloud transitions, helping reduce risk and improve modernization planning.

- [Modernizing Microsoft Access: Migrating to Node.js, OpenAPI, SQL Server, and MongoDB with GitHub Copilot](https://techcommunity.microsoft.com/t5/azure-architecture-blog/how-to-modernise-a-microsoft-access-database-forms-vba-to-node/ba-p/4473504)
- [AI-Assisted JavaScript/TypeScript Modernizer Preview in VS Code Insiders](https://devblogs.microsoft.com/blog/jsts-modernizer-preview)
- [Modernizing Legacy Apps with GitHub Copilot and Azure](/videos/2025-12-10-Modernizing-Legacy-Apps-with-GitHub-Copilot-and-Azure.html)

### Enhancing CLI and Terminal Developer Environments

Copilot CLI improvements continue the progress toward more productive terminal use. The updated Windows Terminal guide complements earlier scripting resources, covering prompt customization and persistent tracking for increased efficiency.

New scripting patterns, automation for scaffolding, and tighter integration with Visual Studio Code and Azure illustrate how the CLI is evolving into a core productivity platform, continuing themes of seamless scripting and automation.

- [Making Windows Terminal Awesome with GitHub Copilot CLI](https://devblogs.microsoft.com/blog/making-windows-terminal-awesome-with-github-copilot-cli)
- [Scripting the GitHub Copilot CLI - Deep Dive](/videos/2025-12-11-Scripting-the-GitHub-Copilot-CLI-Deep-Dive.html)

### Copilot Ecosystem: Administration, Education, and Workflow Guides

The Copilot ecosystem now offers improved management, reporting, and educational resources. Updates include new dashboards, metrics tracking for code generation, and expanded governance. The GitHub Spark SKU changes and DPA features enhance organizational management, while Spec Kit now integrates spec-driven workflows as discussed last week.

Educational initiatives such as quantum computing learning with Copilot agents, Octoverse reports, and enablement stories extend Copilot’s reach for teams experimenting with advanced scenarios. These also reinforce onboarding and standards automation priorities introduced previously.

- [GitHub Spark: Improvements, Dedicated SKU, and DPA Coverage](https://github.blog/changelog/2025-12-10-github-spark-improvements-dpa-coverage-dedicated-sku)
- [Plan, Specify, and Implement with Spec Kit and GitHub Copilot](/videos/2025-12-11-Plan-Specify-and-Implement-with-Spec-Kit-and-GitHub-Copilot.html)
- [Showcasing a Quantum Computing Educational Platform with Custom Copilot](/videos/2025-12-12-Showcasing-a-Quantum-Computing-Educational-Platform-with-Custom-Copilot.html)
- [Balancing Speed and Quality with AI and GitHub Copilot in Development](https://github.blog/ai-and-ml/generative-ai/speed-is-nothing-without-control-how-to-keep-quality-high-in-the-ai-era/)
- [Eirini Kalliamvakou Discusses Copilot and AI Trends in the 2025 Octoverse Report](/videos/2025-12-12-Eirini-Kalliamvakou-Discusses-Copilot-and-AI-Trends-in-the-2025-Octoverse-Report.html)
- [The New Identity of a Developer in the AI Era](https://github.blog/news-insights/octoverse/the-new-identity-of-a-developer-what-changes-and-what-doesnt-in-the-ai-era/)
- [How I shipped more code and products than ever before with GitHub Copilot](/videos/2025-12-11-How-I-shipped-more-code-and-products-than-ever-before-with-GitHub-Copilot.html)
- [Building AI Agents with VS Code, GitHub Copilot, and Azure](/videos/2025-12-10-Building-AI-Agents-with-VS-Code-GitHub-Copilot-and-Azure.html)
- [Gemini 3 Pro Model Launches for GitHub Copilot in Popular IDEs](https://github.blog/changelog/2025-12-12-gemini-3-pro-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Microsoft Learn MCP Server: Next-Level Copilot Integration for Developers](https://devblogs.microsoft.com/dotnet/microsoft-learn-mcp-server-elevates-development/)

## AI

The AI segment this week spotlights integration within Microsoft's ecosystem, including improvements in agentic AI platforms, the broad GPT-5.2 rollout, and new open standards for agent orchestration. Developers received new resources covering best practices, productivity workflows, and team dynamics.

### Microsoft Foundry and Agentic AI Platforms

The Foundry platform, which now includes MCP Server and enhanced orchestration, supports modular agent architectures. Features include persistent agent memory, customization, security controls, and options for business process automation such as expense management and analytics.

Key updates at Ignite 2025 include access to Anthropic models, multi-model coordination, unified data pipelines, and dedicated hardware for AI training. Copilot Studio and Azure Copilot add tools for automation, analytics, and compliance via portal, CLI, and operations modules. Security improvements include ARM CPU, NVIDIA GPU, and hardware security module support for AI jobs. New guides demonstrate how to build document-processing pipelines and fine-tune agent orchestration, helping teams quickly adopt these capabilities.

- [Exploring the Future of AI Agents with Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/exploring-the-future-of-ai-agents-with-microsoft-foundry/ba-p/4476107)
- [Agentic AI and Cloud Innovation: Key Takeaways from Microsoft Ignite 2025](https://azure.microsoft.com/en-us/blog/actioning-agentic-ai-5-ways-to-build-with-news-from-microsoft-ignite-2025/)
- [Nasdaq Boardvantage: AI-Driven Governance Architecture with Azure PostgreSQL and Microsoft Foundry](/videos/2025-12-09-Nasdaq-Boardvantage-AI-Driven-Governance-Architecture-with-Azure-PostgreSQL-and-Microsoft-Foundry.html)
- [From Large Semi-Structured Documents to Actionable Data: Azure-Powered Intelligent Document Processing Pipelines](https://techcommunity.microsoft.com/t5/azure-architecture-blog/from-large-semi-structured-docs-to-actionable-data-reusable/ba-p/4474054)
- [Ignite BRK197: AI Powered Automation & Multi-Agent Orchestration in Foundry](/videos/2025-12-10-Ignite-BRK197-AI-Powered-Automation-and-Multi-Agent-Orchestration-in-Foundry.html)
- [Build a Pizza Ordering Agent with Microsoft Foundry and MCP](/videos/2025-12-10-Build-a-Pizza-Ordering-Agent-with-Microsoft-Foundry-and-MCP.html)
- [AI Upskilling Framework Level 3: Building Agentic Workflows from Microsoft Ignite](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-upskilling-framework-level-3-building/ba-p/4477472)
- [Ignite BRK1706: Build and Manage AI Apps with Microsoft Foundry](/videos/2025-12-10-Ignite-BRK1706-Build-and-Manage-AI-Apps-with-Microsoft-Foundry.html)

### GPT-5.2 Rollout and Integration

OpenAI’s GPT-5.2 is now rolled out to GitHub Copilot, Studio, Foundry, and Microsoft 365 Copilot, building on the previous preview release. GPT-5.2 adds improved logical reasoning, expanded automation and context length, and structured outputs for enterprise platforms, along with policy controls for regulated industries.

Developers can use new options to select GPT-5.2 models directly. Copilot and Studio gain improved reasoning and analytics features, while Microsoft 365 Copilot brings upgraded enterprise analytics. User feedback this week focused on refined experiences in model selection and analytics.

- [Introducing GPT-5.2 in Microsoft Foundry: The New Standard for Enterprise AI](https://azure.microsoft.com/en-us/blog/introducing-gpt-5-2-in-microsoft-foundry-the-new-standard-for-enterprise-ai/)
- [GPT-5.2 Integration Across Microsoft Tools: AI Model Choice and Developer Impact](https://www.linkedin.com/posts/satyanadella_super-excited-about-gpt-52-from-our-partners-activity-7404949433819463680-VyMD)

### Model Context Protocol (MCP) and Agentic Interoperability

The Model Context Protocol (MCP) has now been moved to the Linux Foundation, offering standards for authentication, tool invocation, and handling long-running jobs across agent systems. This move is designed to support interoperability and provide consistent APIs for AI agent developers.

The GitHub MCP Server added more granular configuration, context management, a Go SDK with auto-completion, and improved security with features like Lockdown mode. Additional resources this week demonstrate how to build and use MCP-enabled workflows and highlight the progress on open-source agentic tools.

- [GitHub MCP Server Adds Tool-Specific Configuration and Security Features](https://github.blog/changelog/2025-12-10-the-github-mcp-server-adds-support-for-tool-specific-configuration-and-more)
- [MCP Transitions to Linux Foundation: Impact on AI Tool and Agent Development](https://github.blog/open-source/maintainers/mcp-joins-the-linux-foundation-what-this-means-for-developers-building-the-next-era-of-ai-tools-and-agents/)
- [Unlock agentic workflows for your apps using MCP on Windows](/videos/2025-12-10-Unlock-agentic-workflows-for-your-apps-using-MCP-on-Windows.html)
- [Model Context Protocol: From Concept to Linux Foundation Agentic AI](/videos/2025-12-09-Model-Context-Protocol-From-Concept-to-Linux-Foundation-Agentic-AI.html)

### AI Integration in Developer Workflows and Cloud Automation

Guides released this week detail best practices for integrating AI into developer workflows, focusing on authentication, agent orchestration, and end-to-end automation. Tutorials demonstrate secure app building with OpenAI libraries, using Entra ID tokens and MCP agents. Updates to the .NET agent framework add memory persistence and scalable orchestration, all supported by hands-on labs. Azure Redis now offers secure memory support for agents, and the Durable Task Extension streamlines orchestration for distributed workloads.

Azure Copilot Storage Migration Solutions Advisor and persistent memory in Azure SRE Agent continue the automation and troubleshooting focus for DevOps and cloud teams.

- [Supercharge Your Apps with OpenAI: Secure Authentication, Azure Integration, and MCP Agents](/videos/2025-12-10-Supercharge-Your-Apps-with-OpenAI-Secure-Authentication-Azure-Integration-and-MCP-Agents.html)
- [Secure and Smart AI Agents with Azure Redis in .NET](/videos/2025-12-10-Secure-and-Smart-AI-Agents-with-Azure-Redis-in-NET.html)
- [Modernization Made Simple: Building Agentic Solutions in .NET](/videos/2025-12-10-Modernization-Made-Simple-Building-Agentic-Solutions-in-NET.html)
- [Bulletproof Agents with the Durable Task Extension for Microsoft Agent Framework](/videos/2025-12-10-Bulletproof-Agents-with-the-Durable-Task-Extension-for-Microsoft-Agent-Framework.html)
- [Transforming Data Migration Decisions with Azure Copilot's Storage Migration Solutions Advisor](https://techcommunity.microsoft.com/t5/azure-storage-blog/transforming-data-migration-using-azure-copilot/ba-p/4476610)
- [Never Explain Context Twice: Introducing Azure SRE Agent Memory](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/never-explain-context-twice-introducing-azure-sre-agent-memory/ba-p/4473059)

### Microsoft Fabric and Real-Time Intelligence for AI

New features in Microsoft Fabric this week enable more unified analytics and AI-driven modeling. Developers can coordinate data flows for ingestion, analytics, anomaly detection, and semantic modeling in a single platform, eliminating the need for separate tools. Updates include Eventstream, Eventhouse, anomaly detection, and Fabric IQ, supporting automated insight generation and dashboard creation.

- [Microsoft Fabric Real-Time Intelligence: Setting the Standard for Streaming Data Platforms](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-real-time-intelligence-a-leader-in-the-2025-forrester-streaming-data-wave/)

### Agentic Business Applications and Autonomous AI

Convergence 2025 confirms that business platforms such as Dynamics 365, Copilot Studio, and Microsoft Fabric now include AI-driven automation for ERP, CRM, and business operations. Copilot Studio allows for custom agent design and integration, and new articles outline how to define agent identity, enforce governance, and use open protocols for scale-out deployments.

- [Agentic Business Applications and AI Autonomy at Convergence 2025](https://www.microsoft.com/en-us/dynamics-365/blog/business-leader/2025/12/09/the-era-of-agentic-business-applications-arrives-at-convergence-2025/)
- [AI-Driven Product Change Management with Copilot Studio for Manufacturing](/videos/2025-12-09-AI-Driven-Product-Change-Management-with-Copilot-Studio-for-Manufacturing.html)

### Advances in AI Procurement and Developer Productivity

Microsoft’s new Agent Pre-Purchase Plan (P3) gives organizations a way to purchase pooled agent credits for Foundry and Copilot Studio, which simplifies procurement across the AI portfolio and reflects last week’s preview. Developer tools like Aspire are now available to support monitoring AI applications and connecting to Azure, making it easier to adopt these in the enterprise.

- [Microsoft Agent Pre-Purchase Plan: Unified AI Procurement for Foundry and Copilot Studio](https://techcommunity.microsoft.com/t5/finops-blog/microsoft-agent-pre-purchase-plan-one-unified-path-to-scale-ai/ba-p/4476052)
- [Aspire for AI Applications](/videos/2025-12-10-Aspire-for-AI-Applications.html)
- [AI Dev Days 2025: Your Gateway to the Future of AI Development](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-dev-days-2025-your-gateway-to-the-future-of-ai-development/ba-p/4476113)

### Multi-Model Reasoning, Open Source, and AI Developer Research

A demonstration app showed how to run decisions across multiple AI models (GPT, Claude, Gemini) on Azure, with full CI/CD support. The 2025 Octoverse report analyzed the influence of AI on open-source work, continuing the thread of practical community practices from last week.

Research this week from Atlassian, Google DORA, and LaunchDarkly reinforces the importance of proven practices, trust, and discipline to maximize AI productivity gains, echoing prior coverage of the human side of developer work.

- [Multi-Model Reasoning App Demoed at Bengaluru Dev Event: Decision Frameworks, Azure, and Copilot Vision](https://www.linkedin.com/posts/satyanadella_was-fun-to-be-at-a-dev-event-in-bengaluru-activity-7404820776195043329-A5vB)
- [The Human Side of Octoverse 2025: Insights on Open Source, AI, and Collaboration](/videos/2025-12-08-The-Human-Side-of-Octoverse-2025-Insights-on-Open-Source-AI-and-Collaboration.html)
- [Research: AI's Impact on Developer Productivity Hinges on Best Practices](https://devclass.com/2025/12/11/research-ai-can-help-or-hinder-software-development-and-old-style-best-practices-make-the-difference/)

## ML

Updates in ML this week delivered more productive workflows and better data quality for large-scale and enterprise deployments. Microsoft improved ML engineering, pipeline automation, and operational tooling with Spark and agent frameworks.

### Microsoft Fabric Ecosystem: Engineering, Data Quality, and Automation

This week, Microsoft Fabric improvements target automation and barrier reduction in ML workflows. A step-by-step guide demonstrates how to automate data quality checks for every layer of a Medallion Architecture using Great Expectations for reusable, testable pipelines. The guide also explains how to integrate results with incident response and analytics workflows.

The new Forecasting Service allows for nearly instant Spark notebook startup, building on the recent focus on serverless infrastructure and cost efficiency. Articles this week explain dynamic scheduling and predictive scaling using Azure Cosmos DB and Data Explorer.

Variable Library is now available for Fabric Notebooks, offering centralized management for secrets and configuration, supporting automation and migration across environments.

Update to Fabric Real-Time Intelligence changes how Anomaly Detector is billed—from instance-based to query-based—helping teams monitor usage and control costs more effectively.

- [From Bronze to Gold: Data Quality Strategies for ETL in Microsoft Fabric](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/from-bronze-to-gold-data-quality-strategies-for-etl-in-microsoft/ba-p/4476303)
- [How Microsoft Fabric's Forecasting Service Makes Spark Notebooks Instant](https://blog.fabric.microsoft.com/en-US/blog/how-fabric-makes-spark-notebook-feel-instant-proactive-resource-provisioning-for-scalable-data-science-data-engineering/)
- [Variable Library Support Now Available in Microsoft Fabric Notebooks](https://blog.fabric.microsoft.com/en-US/blog/variable-library-support-in-notebook-now-generally-available/)
- [Understanding Billing for Anomaly Detector in Microsoft Fabric’s Real-Time Intelligence](https://blog.fabric.microsoft.com/en-US/blog/billing-for-anomaly-detector-in-real-time-intelligence/)

### Reinforcement Learning in AI Agents: Agent Lightning Open Source Release

Microsoft Research Asia has open-sourced Agent Lightning, a framework designed for reinforcement learning (RL) with support for decoupled RL training and agent execution. The platform enables workflow optimization for existing LLMAgent frameworks, supports hierarchical RL for complex tasks, and allows flexible plug-in of new RL algorithms.

Agent Lightning streamlines logging, supports both GPU and CPU, and increases accuracy in a range of scenarios including text-to-SQL, RAG, and multi-agent QA. Continuous learning is planned. Development is underway to offer better prompt optimization and easy RL integration in live AI applications.

- [Agent Lightning: Making AI Agents Smarter Without Rewriting Code](https://www.microsoft.com/en-us/research/blog/agent-lightning-adding-reinforcement-learning-to-ai-agents-without-code-rewrites/)

### AI for Biomedical Workflows: GigaTIME Spatial Proteomics Platform

GigaTIME, a spatial proteomics platform, lets scientists use machine learning on digital slides to measure protein distributions at scale, removing the need for expensive assays. It supports broad analysis and rapid hypothesis generation for cancer research, representing practical ML for biomedical challenges.

- [AI-Powered Spatial Proteomics Platform GigaTIME Accelerates Cancer Discovery](https://www.linkedin.com/posts/satyanadella_ai-generated-population-scale-is-changing-activity-7404189540757831680-VtoO)

## Azure

This week, Azure published updates and general availability announcements aimed at simplifying developer workflows in cloud-native, analytics, and modernization projects. Improvements include new data services, deployment capabilities, and better integration for unified analytics and automation.

### Microsoft Fabric and SQL Data Platform

SQL Database in Microsoft Fabric is now generally available, providing a more integrated experience with Azure data services and greater workflow visibility. Lakehouse schemas address challenges in organizing large datasets, particularly for teams adjusting from schema-less data lakes.

General availability of SSIS 2025 enables secure migration to the cloud, while updated tooling streamlines transitions to optimized architectures, continuing the series of migration and modernization resources.

- [What's New and Generally Available in SQL Database in Microsoft Fabric](/videos/2025-12-11-Whats-New-and-Generally-Available-in-SQL-Database-in-Microsoft-Fabric.html)
- [The Evolution of SSIS: SQL Server Integration Services 2025 General Availability and Microsoft Fabric Integration](https://blog.fabric.microsoft.com/en-US/blog/the-evolution-of-sql-server-integration-services-ssis-ssis-2025-generally-available/)
- [Lakehouse Schemas Now Generally Available in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/lakehouse-schemas-generally-available/)

### Azure Container Apps: Deployment, Autoscaling, and Capacity Management

Azure Container Apps (ACA) gains additional resources for blue-green deployments using the CLI, with support from Bicep and CI/CD automation. Updated workload profiles, including ‘Flex’, support cost-effectiveness and resource optimization for cloud-native applications.

- [Blue-Green Deployment in Azure Container Apps with Azure Developer CLI](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-blue-green-aca-deployment/)
- [Capacity Planning with Azure Container Apps Workload Profiles](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/capacity-planning-with-azure-container-apps-workload-profiles/ba-p/4477085)

### Azure Storage: AI Capabilities and Migration Tools

Recent Azure Storage updates focus on enabling ML, analytics, and migration for the enterprise. The platform now offers improved scaling, integration with OpenAI/RAG, and Smart Tiering for cost management. Managed Lustre and Elastic SAN support help bridge data science workflows and storage infrastructure. Enhancements to migration tooling and identity-driven management round out the improvements.

- [Azure Storage Innovations: Foundation for AI-Powered Data Transformation](https://azure.microsoft.com/en-us/blog/azure-storage-innovations-unlocking-the-future-of-data/)

### Observability, Reliability, and DevOps on Azure

Azure’s SRE Agent now delivers enhanced .NET telemetry collection and automated incident rollback, moving beyond log collection into proactive reliability management. Private Preview of Azure Managed Prometheus extends monitoring to VMs and VMSS, while new integrations with Azure Monitor and Grafana add more options for metric analysis.

- [Proactive .NET Reliability with Azure SRE Agent](/videos/2025-12-10-Proactive-NET-Reliability-with-Azure-SRE-Agent.html)
- [Private Preview: Azure Managed Prometheus for VM & VMSS Monitoring](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/private-preview-azure-managed-prometheus-on-vm-vmss/ba-p/4473472)

### Microsoft Fabric Extensibility and Logic Apps: Integration and Testability

The Fabric Extensibility Toolkit has been updated to simplify validation and publishing workflows for partners. Logic Apps now support MCP server hosting and a new Data Mapper Test Executor, expanding testable automation and transformation capabilities across integrated workflows.

- [Microsoft Fabric Extensibility Toolkit: Streamlining Workload Publishing for Partners](https://blog.fabric.microsoft.com/en-US/blog/fabric-extensibility-toolkit-publishing-workloads-announcements/)
- [Using Logic Apps as Model Context Protocol (MCP) Servers for AI Applications](/videos/2025-12-09-Using-Logic-Apps-as-Model-Context-Protocol-MCP-Servers-for-AI-Applications.html)
- [Data Mapper Test Executor in Logic Apps Standard Test Framework](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/data-mapper-test-executor-a-new-addition-to-logic-apps-standard/ba-p/4472440)

### AI Agent Backend Development with Model Context Protocol on Azure Functions

Guides this week show how to spin up Node.js MCP servers on Azure Functions with Anthropic SDK integration, HTTP streaming, and CI/CD automation. These posts take MCP agents from prototype to real-world use.

- [Host Your Node.js MCP Server on Azure Functions in 3 Simple Steps](https://devblogs.microsoft.com/blog/host-your-node-js-mcp-server-on-azure-functions-in-3-simple-steps)

### Migration, Modernization, and End-to-End Developer Guides

New step-by-step resources for app migration cover assessment, design, security, and cost considerations. Azure Data Factory migration and no-code ASP.NET modernization demos align with ongoing cloud-native modernization efforts, now supporting even faster cloud onboarding and improvement.

- [Key Considerations for Modernizing and Migrating Custom Applications to Azure](https://techcommunity.microsoft.com/t5/azure-migration-and/key-considerations-for-modernizing-and-migrating-custom/ba-p/4476580)
- [Assessing Your Azure Data Factory for Migration to Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/assessing-your-azure-data-factory-for-migration-to-fabric-data-factory/)
- [No-code Modernization for ASP.NET with Managed Instance on Azure App Service](/videos/2025-12-10-No-code-Modernization-for-ASPNET-with-Managed-Instance-on-Azure-App-Service.html)

### Other Azure News

General Azure updates continue to stress platform resilience, compliance, and ongoing operational support. This week includes Azure Batch access deprecation, Application Gateway v2 with FIPS certification, and Azure Files Premium redundancy improvements. Azure-wide collaboration and IoT updates, along with new learning initiatives, sustain a focus on team enablement.

The updated .NET-to-Dataverse guide provides code samples for API use and identity management, supporting recent practices for secure application integration.

- [Azure Update - 12th December 2025](/videos/2025-12-12-Azure-Update-12th-December-2025.html)

- [How to Connect .NET Applications to Dataverse Using Microsoft.PowerPlatform.Dataverse.Client](https://techcommunity.microsoft.com/t5/web-development/connect-net-4-6-2-to-dataverse-using-the-dataverse-plugin/m-p/4476310#M682)

A new case study from Swiggy highlights using Fabric for real-time analytics, including demand forecasting and optimizing delivery, illustrating the value of up-to-date operational intelligence.

- [Real-time Intelligence with Microsoft Fabric at Swiggy](https://news.microsoft.com/source/asia/features/real-time-intelligence-how-indias-swiggy-serves-millions-with-microsoft-fabric/)

Azure Arc Server Forum returns with technical talks on hybrid cloud management, supporting the community with ongoing learning opportunities.

- [Azure Arc Server Forum: 2026 Schedule and Participation Updates](https://techcommunity.microsoft.com/t5/azure-arc-blog/azure-arc-server-forum-2026-updates/ba-p/4476227)

A review of Azure Session Host deployment practices advises using Key Vault and Managed Identity for credential management, avoiding exposure through logs or scripts.

- [Challenges with Custom Script Extensions Authentication in Azure Session Host Configuration](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/custom-script-extensions-and-session-host-configuration/m-p/4476435#M13956)

The Cloud Accelerate Factory program expands its hands-on support for Azure migration projects, designed to accelerate team success in cloud transitions.

- [Cloud Accelerate Factory: Accelerate Azure Adoption with Expert Guidance](https://www.thomasmaurer.ch/2025/12/cloud-accelerate-factory-accelerate-cloud-adoption-with-expert-guidance/)

## Coding

This week's updates cover .NET networking changes, cross-platform in-app billing, innovations in PC gaming, and new tools in Visual Studio Code for developer error handling and maintainability.

### .NET 10 Platform Developments and Servicing Updates

.NET 10 introduced networking features like optional certificate caching and support for more HTTP verbs, continuing last week’s progress in performance and security. TLS 1.3 adoption improves, and .NET 10.0.1 servicing release upholds the recommendation to remain current, with an emphasis on upgrading over older .NET Framework versions.

- [.NET 10 Networking Improvements](https://devblogs.microsoft.com/dotnet/dotnet-10-networking-improvements/)
- [.NET and .NET Framework December 2025 Servicing Updates Recap](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-december-2025-servicing-updates/)

### Cross-Platform Client Development with .NET MAUI

A new sample for .NET MAUI demonstrates unified in-app billing across Android, iOS, Mac, and Windows. It provides abstraction layers for different platforms, migration from third-party plugins, and compliance considerations. Code architecture templates and server validation help lay the groundwork for long-term code maintenance.

- [Cross-Platform In-App Billing in .NET MAUI: New Sample Implementation](https://devblogs.microsoft.com/dotnet/cross-platform-billing-dotnet-maui/)

### Windows PC Gaming Development: Tools and Platform Advancements

PC gaming on Windows receives updates that extend support for handheld devices, ARM-based systems, and new rendering technology. Advanced Shader Delivery and Auto Super Resolution bring both improved player experience and performance, in line with wider cross-device support goals.

- [Windows PC Gaming Innovations in 2025: Handheld Devices, Arm Expansion, and DirectX Upgrades](https://blogs.windows.com/windowsexperience/2025/12/09/windows-pc-gaming-in-2025-handheld-innovation-arm-progress-and-directx-advances/)

### Visual Studio Code: Agent HQ, TypeScript 7, and Copilot Changes

Visual Studio Code introduces the Agent HQ feature for managing multiple development agents, answering increased demand for agentic workflows. TypeScript 7 Preview introduces improvements for modern APIs and parallel services. The deprecation of IntelliCode and Copilot changes for free-tier users adjust the available coding assistance tools.

- [VS Code Update Introduces Agent HQ, TypeScript 7 Preview, and Deprecates IntelliCode](https://devclass.com/2025/12/11/vs-code-update-brings-agent-overload-typescript-7-preview-and-the-end-of-intellicode/)

### Other Coding News

Andrew Lock reviews the new Zed editor, offering .NET and Markdown developers alternatives beyond Visual Studio Code. A troubleshooting guide covers proper exception handling strategies for filesystem issues such as the `\\.\\nul` path, helping developers write more reliable applications.

- [Trying out the Zed editor on Windows for .NET and Markdown](https://andrewlock.net/trying-out-the-zed-editor-on-windows-for-dotnet-and-markdown/)
- [Troubleshooting the `\\.\\nul` Path Error in Directory Files Lookup](https://weblog.west-wind.com/posts/2025/Dec/08/What-the-heck-is-a-nul-path-and-why-is-it-breaking-my-Directory-Files-Lookup)

## DevOps

DevOps updates this week emphasize secure and scalable environments with new tools from GitHub, Azure, and Visual Studio Code to manage complex automation and infrastructure for large teams.

### GitHub Platform and Workflow Enhancements

GitHub Enterprise Server 3.19 is out, offering new repository metadata options, reusable ruleset templates, SHA pinning for Actions, and SSH/TLS management to address governance and compliance. OpenTelemetry integration and improved admin tools are available, including a central repository dashboard, updated PR reviews, and performance improvements for enterprise teams. Supply chain security for Go projects is advanced with improved dependency graphs and expanded API support.

Admins also have new capabilities for official communication with ‘Post as Admin’ in GitHub Discussions.

- [GitHub Enterprise Server 3.19 Release Highlights](https://github.blog/changelog/2025-12-10-github-enterprise-server-3-19-is-now-generally-available)
- [Repository Dashboard Preview: Centralized Search, Filtering, and Saved Queries on GitHub](https://github.blog/changelog/2025-12-11-repository-dashboard-find-search-and-save-queries-in-preview)
- [Commit-by-Commit Review and Enhanced Filtering in GitHub Pull Request Files Changed Experience](https://github.blog/changelog/2025-12-11-review-commit-by-commit-improved-filtering-and-more-in-the-pull-request-files-changed-public-preview)
- [Enterprise Teams Product Limits Increased by Over 10x](https://github.blog/changelog/2025-12-08-enterprise-teams-product-limits-increased-by-over-10x)
- [Dependabot-Based Dependency Graphs Enhance Supply Chain Security for Go Projects](https://github.blog/changelog/2025-12-09-dependabot-dgs-for-go)
- [GitHub Custom Repository Properties: GraphQL API and URL Type Enhancements](https://github.blog/changelog/2025-12-09-repository-custom-properties-graphql-api-and-url-type)
- [Post as Admin Feature in GitHub Discussions](https://github.blog/changelog/2025-12-11-post-as-admin-now-available-in-github-discussions)

### GitHub Actions Evolution and Runner Management

GitHub Actions is now capable of handling up to 71 million jobs daily, following architectural changes and better scheduling. YAML anchors, nested workflows, and larger caches make CI/CD processes more flexible. All self-hosted runners must be upgraded to v2.329.0 by January 2026 to remain supported, reflecting the new security guidelines.

For Azure VNET-injected runners, network diagnostics are improved. Forthcoming features expand flexibility around job scheduling and parallel execution.

- [How GitHub Actions Evolved: Architecture, Key Upgrades & What’s Next](https://github.blog/news-insights/product-news/lets-talk-about-github-actions/)
- [Improved Network Diagnostics and Required Self-Hosted Runner Upgrades for GitHub Actions with Azure VNET Injection](https://github.blog/changelog/2025-12-12-better-diagnostics-for-vnet-injected-runners-and-required-self-hosted-runner-upgrades)

### Azure DevOps Server and Enterprise Migration

Azure DevOps Server is generally available, maintaining support for companies with self-managed environments. Migration from legacy TFS systems is also facilitated, and real-world stories provide guidance for phased migrations and enterprise upgrades.

- [Azure DevOps Server Now Generally Available for Self-Hosted Enterprises](https://devblogs.microsoft.com/devops/announcing-azure-devops-server-general-availability/)
- [Applying DevOps Principles on Lean Infrastructure: Lessons from Scaling to 102,000 Users and Planning Azure Migration](https://techcommunity.microsoft.com/t5/azure/applying-devops-principles-on-lean-infrastructure-lessons-from/m-p/4476015#M22362)

### Cloud-Native DevOps with Azure MCP and Agent Management

Recent posts and guides highlight best practices for orchestrating pipelines with .NET, Visual Studio, Azure MCP, SQL, and Azure Storage. Visual Studio Code’s new unified agent tools make it easier for developers to oversee multiple agents and improve orchestration, consistent with earlier updates to cloud-native automation.

- [Agentic DevOps: Enhancing .NET Web Apps with Azure MCP](/videos/2025-12-10-Agentic-DevOps-Enhancing-NET-Web-Apps-with-Azure-MCP.html)
- [A Unified Agent Experience in Visual Studio Code](/videos/2025-12-12-A-Unified-Agent-Experience-in-Visual-Studio-Code.html)

## Security

Updates in security span new authentication methods, improvements in supply chain risk management, endpoint security guidance, and permission management strategies. Development platforms now require updated credentials while guides support teams in securing infrastructure.

### npm Authentication, Supply Chain, and IDE Attack Surface

The npm registry now mandates session-based or new CLI tokens, replacing classic tokens for authentication. Two-factor authentication is required for publishing, with short-lived (two-hour) tokens for sessions. The CLI assists with management tasks such as creating, listing, and revoking tokens. OIDC-based publishing is the new recommendation, and developers must revise their workflows to be compliant.

A technical breakdown describes how the Shai-Hulud 2.0 attack targeted npm packages with scripting tools, advising on detection and defense using Defender for Cloud and Sentinel. The persistence of endpoint supply chain threats underscores the need for vigilance.

This week’s security alert reports the discovery of malicious VS Code extensions installing infostealer malware, highlighting risks such as DLL hijacking and reinforcing the importance of plugin monitoring and least-privilege approaches in CI/CD workflows.

- [npm Classic Tokens Revoked: Session-Based Authentication and CLI Token Management Now Available](https://github.blog/changelog/2025-12-09-npm-classic-tokens-revoked-session-based-auth-and-cli-token-management-now-available)
- [Shai-Hulud 2.0: Guidance for Detecting, Investigating, and Defending Against the Supply Chain Attack](https://www.microsoft.com/en-us/security/blog/2025/12/09/shai-hulud-2-0-guidance-for-detecting-investigating-and-defending-against-the-supply-chain-attack/)
- [Security Risks of Malicious VS Code Extensions Targeting Developers](https://devops.com/malicious-vs-code-extensions-take-screenshots-steal-info/)

### Azure and Microsoft Stack Authentication & Access Management

Azure DevOps has posted retirement deadlines for global personal access tokens; after March 2026, new tokens will not be issued, and all tokens will be invalidated by December 2026. Teams are urged to migrate to Entra-backed tokens to support least-privilege and avoid credential sprawl.

A recent analysis points out security concerns with Custom Script Extensions on Azure Virtual Desktop, where blob tokens may appear in logs. Solutions include Key Vault or Managed Identities, although some limitations are noted for portal-based operations.

Fabric’s OneLake now allows for more detailed ReadWrite permissions, making schema-level access management easier for compliance and data governance.

- [Azure DevOps Retires Global Personal Access Tokens: Key Dates and Security Impact](https://devblogs.microsoft.com/devops/retirement-of-global-personal-access-tokens-in-azure-devops/)
- [Securing Custom Script Extensions in Azure Session Host Configurations](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-feedback/more-security-around-using-custom-script-extensions-and-session/idi-p/4476426)
- [Optimizing Permissions with OneLake Security ReadWrite Access](https://blog.fabric.microsoft.com/en-US/blog/31267/)

### Cloud Incident Response, Endpoint Security, and Email Protection

New security incidents show attackers using fake employee identities and KVM tools to gain endpoint access. Microsoft’s investigation relied on Defender for Endpoint, Entra ID, and improved monitoring of onboarding, auditing, and data loss prevention for handling insider risks.

Benchmark studies provided comparisons of email security solutions (Defender for Office 365, SEG, ICES) and highlighted the real-world effectiveness of different products. These rates help leaders shape incident response and technical defenses.

- [Imposter for Hire: How Fake Employees Breach Security](https://www.microsoft.com/en-us/security/blog/2025/12/11/imposter-for-hire-how-fake-people-can-gain-very-real-access/)
- [Transparent Benchmarking and Layered Email Security with Microsoft Defender](https://www.microsoft.com/en-us/security/blog/2025/12/10/clarity-in-complexity-new-insights-for-transparent-email-security/)

### Securing AI, Advanced Defense Strategies, and Practical Security Modeling

A security advisory this week covers how to securely operate AI agents with Azure SQL, focusing on permission management, error tracking, and monitoring for regulatory compliance.

Microsoft’s Security CTO advances the use of graph-based models—mapping identity, credentials, and assets—to enhance security operation centers, combined with AI analytics. Detailed inventory and KQL-based log review are central to this practice.

A new video on security modeling highlights how storytelling improves understanding and training within teams, encouraging practical linkage between incidents and security improvements.

- [Securely Unleashing AI Agents on Azure SQL and SQL Server](/videos/2025-12-10-Securely-Unleashing-AI-Agents-on-Azure-SQL-and-SQL-Server.html)
- [Changing the Physics of Cyber Defense: Graph-Based Strategies and AI with Microsoft Security](https://www.microsoft.com/en-us/security/blog/2025/12/09/changing-the-physics-of-cyber-defense/)
- [The Role of Storytelling in Security Modeling](/videos/2025-12-12-The-Role-of-Storytelling-in-Security-Modeling.html)

### Other Security News

A practical guide walks through setting up Remote Desktop on Windows 11, covering basic access, firewall, VPN setup, and how to secure connections with Network Level Authentication. Troubleshooting advice is provided for typical real-world issues faced by administrators.

- [How to Set Up Remote Desktop on Windows 11: Step-by-Step Guide](https://dellenny.com/how-to-set-up-remote-desktop-on-windows-11-a-beginners-guide/)
