---
title: GitHub Copilot Updates, Agent Tools, .NET 10 Release, and Azure Developments
author: Tech Hub Team
viewing_mode: internal
date: 2025-12-08 09:00:00 +00:00
tags:
- .NET 10
- Agent Framework
- AI Agents
- Automation
- Cloud Native
- Enterprise AI
- Governance
- Microsoft Azure
- Microsoft Fabric
- OpenAI
- TypeScript
- VS
section_names:
- ai
- github-copilot
- ml
- azure
- coding
- devops
- security
---
This week's biggest updates include GitHub Copilot Custom Agents bringing DevOps and security automation to workflows with markdown-based definitions and vendor integrations like PagerDuty and JFrog, .NET 10's release at .NET Conf 2025 with support until 2028 featuring performance improvements and post-quantum cryptography, and Microsoft Foundry's expanded MCP platform with improved agent orchestration, compliance monitoring, and multi-cloud deployment capabilities. These three developments represent major shifts in AI-powered development tools, enterprise application frameworks, and intelligent agent ecosystems.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot Custom Agents](#github-copilot-custom-agents)
  - [GitHub Copilot Spaces](#github-copilot-spaces)
  - [Copilot Model and Chat Enhancements](#copilot-model-and-chat-enhancements)
  - [GitHub Copilot in Visual Studio 2026](#github-copilot-in-visual-studio-2026)
  - [Copilot CLI and MCP Enhancements](#copilot-cli-and-mcp-enhancements)
  - [Copilot Agent Automation, Orchestration, and Evaluation](#copilot-agent-automation-orchestration-and-evaluation)
  - [Issue Assignment and Project Management Integrations](#issue-assignment-and-project-management-integrations)
  - [Administration, Auditing, and Code Generation Metrics](#administration-auditing-and-code-generation-metrics)
  - [Advanced Copilot Use Cases: Code Review, Performance Profiling, HPC Automation](#advanced-copilot-use-cases-code-review-performance-profiling-hpc-automation)
  - [Copilot Studio Intelligent Agent Development](#copilot-studio-intelligent-agent-development)
- [AI](#ai)
  - [Microsoft Agentic Platforms: MCP, Foundry, and Agent Frameworks](#microsoft-agentic-platforms-mcp-foundry-and-agent-frameworks)
  - [AI Models and Data Workflows in the Microsoft Ecosystem](#ai-models-and-data-workflows-in-the-microsoft-ecosystem)
  - [Building and Orchestrating AI Solutions with LangChain, Foundry, and Azure](#building-and-orchestrating-ai-solutions-with-langchain-foundry-and-azure)
  - [AI Developer Events, Training, and Educational Programs](#ai-developer-events-training-and-educational-programs)
  - [Enterprise AI Integration and Administration](#enterprise-ai-integration-and-administration)
  - [AI Developer Workflows and Productivity Tools](#ai-developer-workflows-and-productivity-tools)
  - [AI Agents in Production Workflows and Medical Automation](#ai-agents-in-production-workflows-and-medical-automation)
  - [Responsible AI, Governance, and Engineering Collaboration](#responsible-ai-governance-and-engineering-collaboration)
- [ML](#ml)
  - [Serverless Workspaces in Azure Databricks](#serverless-workspaces-in-azure-databricks)
  - [Secure Automation for Notebooks in Fabric Data Factory Pipelines](#secure-automation-for-notebooks-in-fabric-data-factory-pipelines)
  - [AI-Enabled Customer Segmentation Architecture](#ai-enabled-customer-segmentation-architecture)
- [Azure](#azure)
  - [Azure Networking: Security, Resiliency, and Scale](#azure-networking-security-resiliency-and-scale)
  - [Azure Compute Platform: Performance and Automation Enhancements](#azure-compute-platform-performance-and-automation-enhancements)
  - [Serverless Improvements: Python on Azure Functions](#serverless-improvements-python-on-azure-functions)
  - [Azure Logic Apps: AI Connectors and Integration Services](#azure-logic-apps-ai-connectors-and-integration-services)
  - [Microsoft Fabric: Data Engineering, Automation, and Analytics Integration](#microsoft-fabric-data-engineering-automation-and-analytics-integration)
  - [Cloud-Native and Migration: Observability, Reliability, and Landing Zone Automation](#cloud-native-and-migration-observability-reliability-and-landing-zone-automation)
  - [Azure Arc, Hybrid, and Local Infrastructure](#azure-arc-hybrid-and-local-infrastructure)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET 10, Visual Studio 2026, and MAUI Ecosystem Developments](#net-10-visual-studio-2026-and-maui-ecosystem-developments)
  - [Practical Guidance and Tooling for .NET Developers](#practical-guidance-and-tooling-for-net-developers)
  - [Agentic UI, MCP, and Coding Experience Updates in Microsoft’s Stack](#agentic-ui-mcp-and-coding-experience-updates-in-microsofts-stack)
  - [TypeScript Compiler and Language Service Modernization](#typescript-compiler-and-language-service-modernization)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [GitHub Platform Updates for Governance, Security, and Automation](#github-platform-updates-for-governance-security-and-automation)
  - [Improving Code Health and Automation with GitHub Code Quality](#improving-code-health-and-automation-with-github-code-quality)
  - [DevOps Practices and the AI-Driven Evolution of Software Delivery](#devops-practices-and-the-ai-driven-evolution-of-software-delivery)
- [Security](#security)
  - [GitHub Secret Scanning and Automated Analysis Enhancements](#github-secret-scanning-and-automated-analysis-enhancements)
  - [Secure Authentication Strategies in Cloud and Mobile](#secure-authentication-strategies-in-cloud-and-mobile)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week's updates bring new custom agents, enhanced models, deeper IDE integration, and improved governance tools for developers and teams.

### GitHub Copilot Custom Agents

Custom agents are now available in Copilot, extending beyond standard code completion to streamline DevOps, security, and automation workflows. Teams can define agents in markdown and manage them inside repositories. Integration examples include PagerDuty, JFrog, and Neon. These agents, which run in the terminal, VS Code, and on GitHub.com, provide automation for specific domains and support organization-wide policies or coding standards. Tutorials such as Rubber Duck Thursdays show how to build and set up agents tailored to the needs of your team. Strong vendor integrations and accessible setup options enable flexible AI-driven automation for software pipelines.

- [Introducing Custom Agents in GitHub Copilot for Developer Workflows](https://github.blog/news-insights/product-news/your-stack-your-rules-introducing-custom-agents-in-github-copilot-for-observability-iac-and-security/)
- [Rubber Duck Thursdays: Building with Copilot Custom Agents](/videos/2025-12-03-Rubber-Duck-Thursdays-Building-with-Copilot-Custom-Agents.html)

### GitHub Copilot Spaces

GitHub Copilot Spaces now support sharing via view-only links, making it easier to share documentation, reusable code, and learning materials for open-source projects. These Spaces include role-specific controls and only host user-generated content, focusing on a balance between access and security. Another feature supports adding files directly from GitHub’s code viewer, streamlining workspace creation for AI-powered changes. Updated documentation explains how Copilot Spaces can assist with debugging, planning, and onboarding while maintaining privacy and focusing on team efficiency. The features support collaborative, context-rich work for teams.

- [Accelerate Debugging with GitHub Copilot Spaces and Copilot Coding Agent](https://github.blog/ai-and-ml/github-copilot/how-to-use-github-copilot-spaces-to-debug-issues-faster/)
- [Major Updates to Copilot Spaces: Public Spaces and Code View Integration](https://github.blog/changelog/2025-12-01-copilot-spaces-public-spaces-and-code-view-support)

### Copilot Model and Chat Enhancements

Recent Copilot updates provide public preview access for OpenAI’s GPT-5.1-Codex-Max and add support for Claude Opus 4.5 in Copilot Chat, delivering expanded model choices. Copilot Pro, Business, and Enterprise users have flexible options for code generation and model selection. Copilot Chat in Visual Studio now includes web URL context, letting users reference and query current online content—helpful for questions beyond the model’s existing data. These features equip teams with stronger models, targeted responses, and more manageable AI interactions.

- [OpenAI’s GPT-5.1-Codex-Max Public Preview Release for GitHub Copilot](https://github.blog/changelog/2025-12-04-openais-gpt-5-1-codex-max-is-now-in-public-preview-for-github-copilot)
- [Claude Opus 4.5 Preview Available in GitHub Copilot Chat and IDEs](https://github.blog/changelog/2025-12-03-claude-opus-4-5-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Unlocking Developer Productivity with Copilot Chat’s New URL Context](https://devblogs.microsoft.com/visualstudio/unlocking-the-power-of-web-with-copilot-chats-new-url-context/)

### GitHub Copilot in Visual Studio 2026

The Visual Studio 2026 update boosts Copilot’s integration by adding a GitHub Cloud Agent and expanded contextual actions. Developers can now handle documentation, refactoring, and batch editing directly through Copilot’s interface. New UI features include one-click actions, smarter code search suggestions ("Did You Mean"), and improved code refactoring and hierarchy visualization for C++ projects. C++ preview enrollment is open to more users, extending Copilot’s toolkit for Visual Studio.

- [Visual Studio 2026 Released: GitHub Cloud Agent Preview and Copilot Features](https://devblogs.microsoft.com/visualstudio/visual-studio-november-update-visual-studio-2026-cloud-agent-preview-and-more/)
- [GitHub Copilot and Visual Studio 2026: November Update Highlights](https://github.blog/changelog/2025-12-03-github-copilot-in-visual-studio-november-update)
- [Enhancing C++ Development in Visual Studio 2026 with GitHub Copilot](https://devblogs.microsoft.com/visualstudio/upgrade-msvc-improve-c-build-performance-and-refactor-c-code-with-github-copilot/)

### Copilot CLI and MCP Enhancements

Building on recent work around registry and deployment, new tutorials walk through setting up a private registry on Azure API Center, so only trusted models are accessible in Copilot and VS Code. There are demonstrations for the kit-dev MCP Server CLI, including code symbol extraction, abstract syntax tree searching, and inline documentation. The guides help teams securely automate Copilot and MCP tasks using compliant workflows.

- [Locking Down MCP: Create a Private Registry on Azure API Center for GitHub Copilot and VS Code](https://devblogs.microsoft.com/all-things-azure/locking-down-mcp-create-a-private-registry-on-azure-api-center-and-enforce-it-in-github-copilot-and-vs-code/)
- [Supercharging GitHub Copilot CLI with MCP Server](/videos/2025-12-03-Supercharging-GitHub-Copilot-CLI-with-MCP-Server.html)

### Copilot Agent Automation, Orchestration, and Evaluation

Step-by-step guides continue from last week’s agent orchestration materials, showing how to use Mission Control for Copilot agent assignment, prompt creation, and parallel execution. The ongoing AI Toolkit + Copilot Pet Planner series now covers agent setup, code output generation, iterative tracing, and results evaluation. Tutorials focus on reviewing trace data, comparing agents side by side, and scoring output, making agent development easier to manage.

- [How to Orchestrate Multiple GitHub Copilot Agents Using Mission Control](https://github.blog/ai-and-ml/github-copilot/how-to-orchestrate-agents-using-mission-control/)
- [Setting Up AI Toolkit and GitHub Copilot for Microsoft Foundry Projects](/videos/2025-12-01-Setting-Up-AI-Toolkit-and-GitHub-Copilot-for-Microsoft-Foundry-Projects.html)
- [Generating Agent Code Using AI Toolkit and GitHub Copilot](/videos/2025-12-01-Generating-Agent-Code-Using-AI-Toolkit-and-GitHub-Copilot.html)
- [Creating an Agent with AI Toolkit and GitHub Copilot: Pet Planner Workshop Part 3](/videos/2025-12-01-Creating-an-Agent-with-AI-Toolkit-and-GitHub-Copilot-Pet-Planner-Workshop-Part-3.html)
- [Adding Tracing to an Agent with AI Toolkit and GitHub Copilot](/videos/2025-12-01-Adding-Tracing-to-an-Agent-with-AI-Toolkit-and-GitHub-Copilot.html)
- [Evaluating AI Agent Output with GitHub Copilot and AI Toolkit (Pet Planner Workshop, Part 6)](/videos/2025-12-01-Evaluating-AI-Agent-Output-with-GitHub-Copilot-and-AI-Toolkit-Pet-Planner-Workshop-Part-6.html)
- [AI Toolkit and GitHub Copilot: Model Recommendations Workshop](/videos/2025-12-01-AI-Toolkit-and-GitHub-Copilot-Model-Recommendations-Workshop.html)
- [Evaluating AI Models for Coding with GitHub Models](/videos/2025-12-04-Evaluating-AI-Models-for-Coding-with-GitHub-Models.html)

### Issue Assignment and Project Management Integrations

Now, issues can be assigned directly to Copilot using GraphQL/REST APIs, streamlining automation for code review, triage, and routing CI/CD workflows. Teams can set up custom agent directions and use Copilot with Linear’s issue tracker for automatic code or pull request generation, expanding integrations with other tools.

- [Assign Issues to GitHub Copilot Using the API](https://github.blog/changelog/2025-12-03-assign-issues-to-copilot-using-the-api)
- [Assigning Linear Issues to GitHub Copilot Coding Agent](/videos/2025-12-01-Assigning-Linear-Issues-to-GitHub-Copilot-Coding-Agent.html)

### Administration, Auditing, and Code Generation Metrics

New governance features allow organizations to see more code generation metrics with Copilot Insights Dashboard, breaking down activity by model, user, trigger, and language. Metrics can now be exported, and the Control Panel now provides a unified location for managing agent access, permissions, and audit logs. Better audit trails support secure deployments and help organizations meet compliance requirements.

- [Track Copilot Code Generation Metrics in GitHub Insights Dashboard](https://github.blog/changelog/2025-12-05-track-copilot-code-generation-metrics-in-a-dashboard)
- [Managing and Auditing GitHub Copilot Agents: Insights and Governance Tools](/videos/2025-12-03-Managing-and-Auditing-GitHub-Copilot-Agents-Insights-and-Governance-Tools.html)

### Advanced Copilot Use Cases: Code Review, Performance Profiling, HPC Automation

Pull request review integration now includes automated and custom review features, with CodeQL static analysis. Visual Studio 2026’s Profiler Agent enables natural-language performance analysis using BenchmarkDotNet for .NET projects. For high-performance computing, Copilot helps automate Slurm jobs via GPT-5-based models, reducing manual scripting in scientific workflows.

- [Accelerating Pull Request Reviews with GitHub Copilot Code Review](/videos/2025-12-02-Accelerating-Pull-Request-Reviews-with-GitHub-Copilot-Code-Review.html)
- [Optimizing .NET Performance with Copilot Profiler Agent in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/delegate-the-analysis-not-the-performance/)
- [Automating HPC Workflows with Copilot Agents](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/automating-hpc-workflows-with-copilot-agents/ba-p/4472610)

### Copilot Studio Intelligent Agent Development

A Microsoft Ignite session highlighted ways to develop advanced Copilot Studio agents using Microsoft Graph, Azure AI Search, and Active Directory. Teams can use connectors and business logic to filter documents and analyze information, supporting enterprise automation and aligning with Microsoft security standards.

- [Building Intelligent Agents with Copilot Studio and Advanced Knowledge Sources](/videos/2025-12-03-Building-Intelligent-Agents-with-Copilot-Studio-and-Advanced-Knowledge-Sources.html)

## AI

This week features advances in agent platforms, new models, developer events, and enterprise integration tools.

### Microsoft Agentic Platforms: MCP, Foundry, and Agent Frameworks

The MCP platform enables agent interoperability, with a new livestream series showing how to run servers both locally and in hybrid setups. Foundry’s MCP Server preview adds deeper browser and IDE integration and features for compliance and monitoring. Agent Framework shared updated best practices and expanded integration feedback (e.g., CopilotKit, AG-UI), with guidance for troubleshooting and prototyping multi-agent systems.

- [Learn MCP from our Free Livestream Series in December](https://techcommunity.microsoft.com/t5/microsoft-developer-community/learn-mcp-from-our-free-livestream-series-in-december/ba-p/4474729)
- [Accelerating AI Agent Development: Foundry MCP Server Preview Announced](https://devblogs.microsoft.com/foundry/announcing-foundry-mcp-server-preview-speeding-up-ai-dev-with-microsoft-foundry/)
- [Agentic Development Best Practices with Microsoft Agent Framework: AG-UI, DevUI & OpenTelemetry](https://devblogs.microsoft.com/semantic-kernel/the-golden-triangle-of-agentic-development-with-microsoft-agent-framework-ag-ui-devui-opentelemetry-deep-dive/)

### AI Models and Data Workflows in the Microsoft Ecosystem

Mistral Large 3 is now part of Microsoft Foundry, building on previous support for multi-modal and long-context AI. Foundry also emphasizes sovereign cloud deployment and supports direct observability in the browser for live production workloads. .NET's new ingestion tools further streamline integration with document and vector storage, while event-driven architectures like Drasi’s workflows expand possibilities for event-triggered agents.

- [Introducing Mistral Large 3 in Microsoft Foundry: Open Enterprise AI for Production Workloads](https://azure.microsoft.com/en-us/blog/introducing-mistral-large-3-in-microsoft-foundry-open-capable-and-ready-for-production-workloads/)
- [Introducing Data Ingestion Building Blocks in .NET for AI Applications](https://devblogs.microsoft.com/dotnet/introducing-data-ingestion-building-blocks-preview/)
- [Beyond the Chat Window: How Change-Driven Architecture Enables Ambient AI Agents](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/beyond-the-chat-window-how-change-driven-architecture-enables/ba-p/4475026)

### Building and Orchestrating AI Solutions with LangChain, Foundry, and Azure

The OSS AI Summit continued progress on multi-agent orchestration, featuring updates for LangChain v1, Foundry, and Azure OpenAI. Workshops showed secure agent orchestration and use of Azure project blueprints. Community challenges, like the November Innovation Challenge, provided real-world examples and highlighted community adoption.

- [Join the OSS AI Summit: Building with LangChain and Microsoft AI](https://devblogs.microsoft.com/blog/join-the-oss-ai-summit-building-with-langchain-event)
- [Innovation Challenge: Winning Azure AI Projects from November 2025 Hackathon](https://techcommunity.microsoft.com/t5/azure/the-november-innovation-challenge-winning-teams/m-p/4475579#M22360)

### AI Developer Events, Training, and Educational Programs

Developer education remained in focus, with AI Dev Days offering workshops on Copilot, Foundry, and Azure AI. Demonstrations covered agent code and deployment in Visual Studio 2026 and VS Code, with expanded coverage of new languages and governance APIs. The Hour of AI campaign brings AI literacy efforts to K-12 education, creating pathways for early learning.

- [AI Dev Days: Virtual Event for Developers on Azure, GitHub, and AI with Microsoft Reactor](https://devblogs.microsoft.com/foundry/ai-dev-days-december-2025/)
- [AI Dev Days 2025: Microsoft & GitHub Virtual Event for Developers](https://devblogs.microsoft.com/blog/join-us-for-ai-devdays)
- [AI Dev Days: Using AI to Enhance Developer Productivity](/videos/2025-12-04-AI-Dev-Days-Using-AI-to-Enhance-Developer-Productivity.html)
- [AI Dev Days: Building AI Applications with Azure and GitHub](/videos/2025-12-04-AI-Dev-Days-Building-AI-Applications-with-Azure-and-GitHub.html)
- [Hour of AI: Microsoft Launches Global AI Literacy Initiative for Computer Science Education Week 2025](https://www.microsoft.com/en-us/education/blog/2025/12/unlock-ai-learning-with-hour-of-ai-for-computer-science-education-week/)

### Enterprise AI Integration and Administration

Guides for using Claude Code, Foundry, and Spec Kit address requirement automation and code review for agent-based workflows. Monitoring and context management transitions to GitHub Actions pipelines, and OneLake connects with Foundry Knowledge for consolidated data analytics across Azure and AWS. The AB-900 exam resource helps teams understand Copilot administration and responsible AI practices.

- [Enterprise AI Coding Agent Setup: Claude Code, Microsoft Foundry, Spec Kit, and GitHub Actions](https://devblogs.microsoft.com/all-things-azure/claude-code-microsoft-foundry-enterprise-ai-coding-agent-setup/)
- [Unlocking Enterprise AI: Seamless Integration of OneLake Files in Microsoft Foundry Knowledge](https://blog.fabric.microsoft.com/en-US/blog/unlocking-enterprise-ai-seamless-integration-of-onelake-files-in-microsoft-foundry-knowledge/)
- [AB-900 Study Cram: Microsoft 365 Copilot & Agent Administration Fundamentals](/videos/2025-12-01-AB-900-Study-Cram-Microsoft-365-Copilot-and-Agent-Administration-Fundamentals.html)

### AI Developer Workflows and Productivity Tools

VS Code’s Agent HQ integration supports session management and Copilot CLI, making agent deployment and monitoring more efficient. GitHub Models now allow real-time comparisons between model outputs for coding tasks, while Copilot Studio’s latest roadmap covers debugging improvements, Microsoft 365 integration, and better cost tracking.

- [Agent HQ Integration in Visual Studio Code](/videos/2025-12-07-Agent-HQ-Integration-in-Visual-Studio-Code.html)
- [GitHub Models: Test and Compare AI Code Models](/videos/2025-12-04-GitHub-Models-Test-and-Compare-AI-Code-Models.html)
- [What’s New in Copilot Studio and Roadmap](/videos/2025-12-03-Whats-New-in-Copilot-Studio-and-Roadmap.html)

### AI Agents in Production Workflows and Medical Automation

Guides describe how to embed AI voice agents into medical documentation for real-time productivity. Managed agentic apps in Foundry enable centralized control and cloud scaling, showcasing organizations moving their pilot projects to Azure for full production deployment.

- [How AI Voice Agents Transform Medical Documentation in Real Time](/videos/2025-12-04-How-AI-Voice-Agents-Transform-Medical-Documentation-in-Real-Time.html)
- [Building Connected Managed Agentic Apps with Microsoft Foundry (Ignite BRK113)](/videos/2025-12-02-Building-Connected-Managed-Agentic-Apps-with-Microsoft-Foundry-Ignite-BRK113.html)

### Responsible AI, Governance, and Engineering Collaboration

The Armchair Architects series provides guidance on governance for agent and microservice architectures. Microsoft’s collaborative AI engineering framework, with examples like Entra SDK migration, puts process improvements into focus—integrating agents that support documentation and escalation.

- [Armchair Architects: Governance Strategies for AI Agents](/videos/2025-12-02-Armchair-Architects-Governance-Strategies-for-AI-Agents.html)
- [Collaborating with AI Agents: A Framework for Engineering Transformation at Microsoft](https://devblogs.microsoft.com/engineering-at-microsoft/the-interaction-changes-everything-treating-ai-agents-as-collaborators-not-automation/)

## ML

Updates this week cover serverless workspaces, secure pipeline automation, and AI-enabled customer segmentation.

### Serverless Workspaces in Azure Databricks

Azure Databricks now supports public preview for Serverless workspaces, which remove the need for manual VNet and cluster setup. These changes support persistent data practices and are governed through Unity Catalog, with improvements in budget controls and security. Serverless egress and Private Link options increase compliance, with Python and SQL workflows now supported for scaling secure ML operations.

- [Serverless Workspaces Are Now Available in Azure Databricks](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/serverless-workspaces-are-live-in-azure-databricks/ba-p/4474712)

### Secure Automation for Notebooks in Fabric Data Factory Pipelines

Service Principal and Workspace Identity authentication are now available for running notebooks in Fabric Data Factory pipelines. This change reduces manual configuration, improves reliability, helps centralize identity management, and creates more robust production environments.

- [Run Notebooks in Pipelines with Service Principal or Workspace Identity](https://blog.fabric.microsoft.com/en-US/blog/run-notebooks-in-pipelines-with-service-principal-or-workspace-identity/)

### AI-Enabled Customer Segmentation Architecture

A joint case study from UCLA Anderson and Microsoft details a system for dynamic customer segmentation, helping B2B software businesses better handle resource allocation. The technical solution uses clustering, ML models like CatBoost and XGBoost, and an LLM assistant for workflow transparency. Azure handles orchestration and pipeline reliability for deployment.

- [KPI-Driven, AI-Enabled Tiering Architecture for Microsoft’s Global B2B Business](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/architecting-the-next-generation-customer-tiering-system/ba-p/4475326)

## Azure

This week's releases cover networking, compute, serverless functions, workflow automation, data engineering, migration, and hybrid infrastructure.

### Azure Networking: Security, Resiliency, and Scale

Azure Networking introduced Standard NAT Gateway V2 with zone redundancy and IPv6 for improved outbound connections. DNS Security Policy enables domain-level blocking of threats. New features like Private Link Direct Connect and JWT validation in Application Gateway give cloud-based workloads more flexibility and security. eBPF-based networking for AKS, WAF for containers, and the Bastion browser-based SSH/RDP improve cluster management. Upgrades to ExpressRoute, VPN, and Private Link add capacity and resilience for demanding applications, such as AI clusters.

- [Azure Networking Updates: Security, Resilience, and AI-Driven Scale](https://azure.microsoft.com/en-us/blog/unlocking-the-future-azure-networking-updates-on-security-reliability-and-high-availability-2/)
- [Azure Networking: Latest Updates for Security, Reliability, and AI/ML Workloads](https://azure.microsoft.com/en-us/blog/azure-networking-updates-on-security-reliability-and-high-availability/)

### Azure Compute Platform: Performance and Automation Enhancements

Ignite 2025 featured new compute products, including Direct Virtualization (for near bare-metal speed, especially for NVMe and GPU), and larger containers for resource-intensive deployments. Further, features such as VM state automation via Scheduled Actions and streamlined image management boost global operations. Additions like instance mix for VM Scale Sets and zoning for image galleries help manage costs and resilience.

- [Scaling Azure Compute for Performance: Innovations from Ignite 2025](https://techcommunity.microsoft.com/t5/azure-compute-blog/scaling-azure-compute-for-performance/ba-p/4474662)

### Serverless Improvements: Python on Azure Functions

Azure Functions has switched to using uvloop with Python 3.13+, increasing performance by up to 4.8% for asynchronous workloads. No code changes are required, and Flex Consumption offers improved reliability for serverless and multi-cloud automation scenarios.

- [Faster Azure Functions Python with uvloop](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/faster-azure-functions-python-with-uvloop/ba-p/4455323)
- [Boosting Python Performance on Azure Functions with uvloop](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/faster-python-on-azure-functions-with-uvloop/ba-p/4455323)

### Azure Logic Apps: AI Connectors and Integration Services

Azure Logic Apps Standard now includes built-in connectors for Azure OpenAI, AI Search, Document Intelligence, and Operations, accessible with a minimal setup. These integrations support agent-centric automations and intelligent document workflows. The December newsletter outlines new XML, migration, error-handling, and AI workflow features for improved automation.

- [General Availability of AI and RAG Connectors in Azure Logic Apps Standard](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-general-availability-of-ai-rag-connectors-in-logic/ba-p/4474337)
- [Logic Apps Aviators Newsletter - December 2025](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-aviators-newsletter-december-2025/ba-p/4474048)

### Microsoft Fabric: Data Engineering, Automation, and Analytics Integration

Fabric users can now expose Lakehouse materialized views as GraphQL APIs for fast analytics integration. Other new tools improve automation for pipeline deployments, cross-cloud analytics using BigQuery and data transfer, and make it simpler to analyze documents from OneDrive/SharePoint without extra copies. CDC and data ingestion are now easier to set up across multiple systems.

- [Exposing Lakehouse Materialized Views via GraphQL APIs in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/exposing-lakehouse-materialized-views-to-applications-in-minutes-with-graphql-apis-in-microsoft-fabric/)
- [Automating Warehouse and SQL Endpoint Deployment in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/bridging-the-gap-automate-warehouse-sql-endpoint-deployment-in-microsoft-fabric/)
- [Announcing Staging for Mirroring for Google BigQuery (Preview)](https://blog.fabric.microsoft.com/en-US/blog/announcing-staging-for-mirroring-for-google-bigquery-in-microsoft-fabricmirroring-for-gbq-staging-blog/)
- [Integrating OneDrive and SharePoint Documents into OneLake Analytics with Shortcuts](https://blog.fabric.microsoft.com/en-US/blog/turning-everyday-documents-from-sharepoint-and-onedrive-into-analytics-ready-data-with-onelake-shortcuts/)
- [Simplifying Data Ingestion with Copy job – Replicate data from Dataverse through Fabric to multiple destinations](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-replicate-data-from-dataverse-through-fabric-to-multiple-destinations/)

### Cloud-Native and Migration: Observability, Reliability, and Landing Zone Automation

A new guide provides details on using Azure Databricks' logging and system tables for full-stack monitoring. Reliability engineering topics now include using entropy concepts for SLAs and chaos testing. Azure Migrate supports secure landing zone creation, while step-by-step material is available for greenfield and brownfield API migrations from Amazon API Gateway to Azure.

- [End-to-End Observability for Azure Databricks: Infrastructure and Application Logging Strategies](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/end-to-end-observability-for-azure-databricks-from/ba-p/4475692)
- [Cloud as a War Against Entropy: Practical Reliability Patterns for Azure Architects](https://techcommunity.microsoft.com/t5/azure-architecture-blog/cloud-as-a-war-against-entropy/ba-p/4474111)
- [Reimagine Migration: Agentic Platform Landing Zone with Azure Migrate](/videos/2025-12-01-Reimagine-Migration-Agentic-Platform-Landing-Zone-with-Azure-Migrate-Video.html)
- [Migrating from Amazon API Gateway to Azure API Management: Technical Guide](https://techcommunity.microsoft.com/t5/azure-migration-and/migrate-from-amazon-api-gateway-to-azure-api-management/ba-p/4471524)

### Azure Arc, Hybrid, and Local Infrastructure

Azure Local can now be deployed within customer-owned data centers or edge locations to support compliance and intermittent connectivity scenarios. New features include integration with GPUs, Azure Migrate, analytics, and generative AI, while Arc Site Manager and GCP connector extend cross-cloud orchestration. There is a new guide for managing Azure Local updates, and Arc now provides governance and security for hybrid clusters.

- [Extending Azure: AI-Powered Innovation, Resiliency, and Control](https://azure.microsoft.com/en-us/blog/new-options-for-ai-powered-innovation-resiliency-and-control-with-microsoft-azure/)
- [Manage Azure Local Updates](https://www.thomasmaurer.ch/2025/12/manage-azure-local-updates/)
- [Azure Arc: Extending Azure for Hybrid and Multi-Cloud Management](/videos/2025-12-03-Azure-Arc-Extending-Azure-for-Hybrid-and-Multi-Cloud-Management.html)

### Other Azure News

Further platform news this week includes detailed Load Balancer metrics, resumable SFTP for Azure Blob, PostgreSQL updates, and expanded Databricks workspace options. Additional support for confidential ledgers, Mistral Large 3 in Foundry, and the retirement of Azure ML SDK v1 (with migration guides) round out the update. New steps explain how to integrate Amazon S3/VPC with Entra ID for analytics in Microsoft Fabric. The Azure Resource Graph GET/LIST API is now GA, supporting scalable resource management, and SQL Server Management Studio 22 adds migration workflow improvements. Azure SRE Agent now enables proactive, scheduled checks feeding into incident workflows. Windows 2025 accessibility updates add enhanced dictation, voice, and magnification capabilities. There’s also a guide for deploying Bun + Hono + Vite JavaScript stacks on Azure Linux Web App.

- [Azure Update - 5th December 2025: New Features, Metrics, and Services](/videos/2025-12-05-Azure-Update-5th-December-2025-New-Features-Metrics-and-Services.html)

Learn how to enable audit-friendly, secure analytics in Microsoft Fabric with service principal authentication for VPC-protected S3 buckets.

- [Securely Access VPC-Protected Amazon S3 Buckets in Microsoft Fabric Using Entra Integration](https://blog.fabric.microsoft.com/en-US/blog/securely-access-vpc-protected-amazon-s3-buckets-in-microsoft-fabric-with-entra-integration-preview/)

Azure Resource Graph GET/LIST APIs support efficient scaling for resource queries and automation.

- [General Availability of Azure Resource Graph GET/LIST API Released](https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-general-availability-for-azure-resource-graph-arg-get/ba-p/4474188)

SQL Server Management Studio 22 is now generally available with workflow and hybrid management enhancements.

- [What's New in SQL Server Management Studio 22 GA](/videos/2025-12-04-Whats-New-in-SQL-Server-Management-Studio-22-GA.html)

Scheduled monitoring from the Azure SRE Agent improves compliance and incident detection and works alongside OpenTelemetry tooling.

- [Automated Proactive Monitoring with Azure SRE Agent Scheduled Tasks](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/proactive-monitoring-made-simple-with-azure-sre-agent/ba-p/4471205)

A year-in-review showcases accessibility innovations for Windows—including updates to dictation, voice access, Narrator, and screen magnification.

- [2025 Accessibility Innovations in Windows: Year in Review](https://blogs.windows.com/windowsexperience/2025/12/03/2025-a-year-in-recap-windows-accessibility/)

Deployment tips for alternative JavaScript stacks (Bun + Hono + Vite) for Azure Linux Web Apps.

- [Deploying a Bun + Hono + Vite App to Azure Linux Web App](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploying-a-bun-hono-vite-app-to-azure-app-service/ba-p/4475356)

## Coding

Updates include .NET 10, Visual Studio 2026, cross-platform frameworks, TypeScript compiler improvements, and AI-powered development tools.

### .NET 10, Visual Studio 2026, and MAUI Ecosystem Developments

.NET Conf 2025 introduced .NET 10 (with support until 2028), following last week’s modular IDE announcements and tighter Copilot/VS integration. Updates focus on performance, post-quantum cryptography, ASP.NET Core, Blazor, .NET MAUI, and C# 14. Aspire 13 CLI adds support for Python and JavaScript. Copilot and Agent Framework continue to grow alongside agent-based development. Progress in code quality and security guidance supports inclusive and streamlined development, and the MAUI Community Standup explored .NET 10’s impact and upgrade strategy.

- [.NET Conf 2025 Recap: What's New in .NET 10, Visual Studio 2026, AI, and App Modernization](https://devblogs.microsoft.com/dotnet/dotnet-conf-2025-recap/)
- [.NET MAUI Community Standup - .NET 10 Announcements Roundup](/videos/2025-12-04-NET-MAUI-Community-Standup-NET-10-Announcements-Roundup.html)

### Practical Guidance and Tooling for .NET Developers

Stephen Toub and Scott Hanselman share recommendations for CancellationToken use in asynchronous .NET code, supporting robust and testable patterns. Age verification for .NET MAUI now aligns with current global regulations, providing platform-specific tips for Android, iOS, and Windows. The latest NetEscapades.EnumGenerators release adds support for [EnumMember] and improved analyzers.

- [Deep Dive into Cancellation Tokens in .NET with Stephen Toub](/videos/2025-12-05-Deep-Dive-into-Cancellation-Tokens-in-NET-with-Stephen-Toub.html)
- [Cross-Platform Age Verification in .NET MAUI Applications](https://devblogs.microsoft.com/dotnet/cross-platform-age-verification-dotnet-maui/)
- [Recent updates to NetEscapades.EnumGenerators: [EnumMember] support, analyzers, and bug fixes](https://andrewlock.net/recent-updates-to-netescapaades-enumgenerators/)

### Agentic UI, MCP, and Coding Experience Updates in Microsoft’s Stack

Demonstrations show AG-UI and Blazor enabling interactive web components for agent-based apps. Updated tooling for MCP in VS Code improves developer onboarding, and features like in-depth keyboard shortcut analysis support improved workflow familiarity in Visual Studio 2026.

- [Building Agentic UI with AG-UI and Blazor: ASP.NET Community Standup](/videos/2025-12-03-Building-Agentic-UI-with-AG-UI-and-Blazor-ASPNET-Community-Standup.html)
- [Tooling Support for MCP in Visual Studio Code](/videos/2025-12-01-Tooling-Support-for-MCP-in-Visual-Studio-Code.html)
- [Making Sense of Keyboard Shortcuts in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/why-changing-keyboard-shortcuts-in-visual-studio-isnt-as-simple-as-it-seems/)

### TypeScript Compiler and Language Service Modernization

Project Corsa is taking TypeScript's compiler and language service to native code in version 7, adding multi-threading and more type safety. Preview features are available in VS Code, and migration guidance is provided.

- [Progress on TypeScript 7: Native Compiler and Language Service Updates](https://devblogs.microsoft.com/typescript/progress-on-typescript-7-december-2025/)

### Other Coding News

The VS Code Insiders Podcast is now available, featuring interviews and technical segments about AI, new VS Code features, accessibility, and open-source engagement.

- [Introducing the VS Code Insiders Podcast](https://code.visualstudio.com/blogs/2025/12/03/introducing-vs-code-insiders-podcast)

## DevOps

This week features new governance controls, automation tools, and insights on AI-driven delivery practices.

### GitHub Platform Updates for Governance, Security, and Automation

GitHub Enterprise Server 3.19 Release Candidate brings added controls, better repository setup and automation, extra rules enforcement, and improved CI/CD. GitHub Actions updates allow blocking individual workflows and enforcing SHA pinning. Organization owners can now manage app installations directly, improving security and simplicity. Workflows now support up to 25 inputs, increasing CI/CD pipeline flexibility.

- [GitHub Enterprise Server 3.19: New Release Candidate Features and Enhancements](https://github.blog/changelog/2025-12-02-github-enterprise-server-3-19-release-candidate-is-now-available)
- [GitHub Actions workflow_dispatch Now Supports 25 Inputs](https://github.blog/changelog/2025-12-04-actions-workflow-dispatch-workflows-now-support-25-inputs)
- [GitHub Organization Owners Gain Control Over App Installations](https://github.blog/changelog/2025-12-01-block-repository-admins-from-installing-github-apps-now-generally-available)

### Improving Code Health and Automation with GitHub Code Quality

GitHub Code Quality pulls together automated code insights, supporting maintainable code and fast feedback as part of the standard DevOps pipeline.

- [How to Improve Code Health with GitHub Code Quality](/videos/2025-12-04-How-to-Improve-Code-Health-with-GitHub-Code-Quality.html)

### DevOps Practices and the AI-Driven Evolution of Software Delivery

A retrospective discusses the history and ongoing evolution of DevOps—tracing early software challenges to today’s use of policy-driven and intelligent automation.

- [The Software Crisis Never Ended, It Just Evolved](https://roadtoalm.com/2025/12/01/the-software-crisis-never-ended-it-just-evolved/)

## Security

Updates cover enhanced secret scanning, secure authentication strategies, and cybersecurity best practices.

### GitHub Secret Scanning and Automated Analysis Enhancements

GitHub expanded secret scanning with new detection patterns for Azure, Databricks, Discord, and other platforms. Additions include EC and PKCS#8 key support, and notifications for gists. Discord bot token alerts and AWS key validation increase metadata for better incident response. CodeQL 2.23.6 updates add Swift, Rust, and C# detection, as well as queries for insecure cookies.

- [GitHub Secret Scanning Updates and New Patterns — November 2025](https://github.blog/changelog/2025-12-02-secret-scanning-updates-november-2025)
- [CodeQL 2.23.6 Update: New C# Security Queries and Language Enhancements](https://github.blog/changelog/2025-12-04-codeql-2-23-6-adds-swift-6-2-1-and-new-c-security-queries)

### Secure Authentication Strategies in Cloud and Mobile

Guides now cover device-bound passkeys in Microsoft Entra ID for policy-driven identity and strong authentication. Device-Bound Request Signing (DBRS) for mobile apps is outlined, with recommendations for crypto, security modeling, and cross-platform deployments.

- [Entra Synced Passkeys and Passkey Profiles](/videos/2025-12-03-Entra-Synced-Passkeys-and-Passkey-Profiles.html)
- [Securing Sensitive Mobile Operations with Device-Bound Request Signing](https://devblogs.microsoft.com/blog/securing-sensitive-mobile-operations-with-device-bound-request-signing)

### Other Security News

Enterprise cybersecurity priorities for 2025 include asset discovery, network segmentation, endpoint hardening, phishing-resistant MFA, and comprehensive use of Microsoft identity and DNS/SMTP protections. Guidance also covers layered defense and cooperative response readiness.

- [Cybersecurity Strategies to Prioritize Now](https://www.microsoft.com/en-us/security/blog/2025/12/04/cybersecurity-strategies-to-prioritize-now/)
