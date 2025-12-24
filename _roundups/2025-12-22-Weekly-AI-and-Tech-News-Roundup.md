---
layout: "post"
title: "AI Agents, DevOps Changes, and Platform Security: Weekly Technology Update"
description: "Review the latest developments in AI coding assistants, updates to DevOps platforms, and new approaches to enterprise security. This roundup highlights GitHub Copilot’s new features for organizations, Microsoft’s expanded offerings for AI and ML applications, and Azure’s improvements to infrastructure and developer workflows. Learn about responses to critical vulnerabilities and updated security controls alongside ongoing changes to coding tools and engineering practices."
author: "Tech Hub Team"
excerpt_separator: <!--excerpt_end-->
viewing_mode: "internal"
date: 2025-12-22 09:00:00 +00:00
permalink: "/2025-12-22-Weekly-AI-and-Tech-News-Roundup.html"
categories: ["AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security"]
tags: ["AI", "Automation", "Azure", "Cloud Computing", "Code Review", "Coding", "Data Engineering", "Dependabot", "DevOps", "GitHub Copilot", "Machine Learning", "Microsoft Fabric", "ML", "Orchestration", "Prompt Engineering", "Roundups", "Security", "VS Code"]
tags_normalized: ["ai", "automation", "azure", "cloud computing", "code review", "coding", "data engineering", "dependabot", "devops", "github copilot", "machine learning", "microsoft fabric", "ml", "orchestration", "prompt engineering", "roundups", "security", "vs code"]
---

This week's biggest news: GitHub Copilot adds GPT-5.2, Claude Opus 4.5, and Gemini 3 Flash models alongside new Agent Skills and Mission Control for workflow automation. Microsoft Azure AI Foundry expands its agentic AI ecosystem with hosted agents and the Model Router going GA. On the security front, teams are responding to the React2Shell vulnerability (CVE-2025-55182) affecting Next.js workloads, while GitHub Actions announces pricing changes for self-hosted runners starting March 2026.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [AI Model Releases and Chat Model Availability](#ai-model-releases-and-chat-model-availability)
  - [Agent Skills and Automation Ecosystem](#agent-skills-and-automation-ecosystem)
  - [Integrated Workflows and Mission Control](#integrated-workflows-and-mission-control)
  - [Code Review, Security, and Metrics](#code-review-security-and-metrics)
  - [IDE Integration, Debugging, and Language Support](#ide-integration-debugging-and-language-support)
  - [Prompt Engineering and Context Management](#prompt-engineering-and-context-management)
  - [Advanced Agent Capabilities and Memory](#advanced-agent-capabilities-and-memory)
  - [Real-World Usage and Ecosystem News](#real-world-usage-and-ecosystem-news)
  - [Effective Use of Copilot in Domain-Specific Contexts](#effective-use-of-copilot-in-domain-specific-contexts)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Azure AI Foundry and the Expanding Agentic AI Ecosystem](#azure-ai-foundry-and-the-expanding-agentic-ai-ecosystem)
  - [Agent-Oriented Architecture: Durable Task Extension and Orchestration Guides](#agent-oriented-architecture-durable-task-extension-and-orchestration-guides)
  - [Model Context Protocol (MCP) and Model Router in Developer Workflows](#model-context-protocol-mcp-and-model-router-in-developer-workflows)
  - [Autonomous Agents and AI-Powered Data Transformation in Microsoft Fabric](#autonomous-agents-and-ai-powered-data-transformation-in-microsoft-fabric)
- [ML](#ml)
  - [Microsoft Fabric ML Platform Advances](#microsoft-fabric-ml-platform-advances)
  - [Performance, Reliability, and Security in ML Workflows](#performance-reliability-and-security-in-ml-workflows)
  - [Evaluation and Best Practices for Azure-based Document AI Pipelines](#evaluation-and-best-practices-for-azure-based-document-ai-pipelines)
- [Azure](#azure)
  - [Azure Networking, Resiliency, and Security Enhancements](#azure-networking-resiliency-and-security-enhancements)
  - [Migration, Modernization, and Strategic Guidance](#migration-modernization-and-strategic-guidance)
  - [Developer Tools, Language Ecosystems, and Hands-On Guides](#developer-tools-language-ecosystems-and-hands-on-guides)
  - [Messaging Patterns, Data Engineering, and Microsoft Fabric Features](#messaging-patterns-data-engineering-and-microsoft-fabric-features)
  - [Schema Management and Change-Driven Architecture](#schema-management-and-change-driven-architecture)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET Development: Cross-Platform Widgets and ASP.NET Core Roadmaps](#net-development-cross-platform-widgets-and-aspnet-core-roadmaps)
  - [Editor Experiences: VS Code and Cursor AI Updates](#editor-experiences-vs-code-and-cursor-ai-updates)
  - [Practical Profiling and Feedback Workflows](#practical-profiling-and-feedback-workflows)
- [DevOps](#devops)
  - [GitHub Actions Self-Hosted Runners: Pricing Changes, Roadmap, and Community Impact](#github-actions-self-hosted-runners-pricing-changes-roadmap-and-community-impact)
  - [Dependabot Expands Ecosystem: Conda, OpenTofu, Julia, Bazel Automated Updates](#dependabot-expands-ecosystem-conda-opentofu-julia-bazel-automated-updates)
  - [Azure DevOps Productivity: Microsoft.Testing.Platform Integration and Security Workflows](#azure-devops-productivity-microsofttestingplatform-integration-and-security-workflows)
  - [Microsoft Fabric and Azure DevOps: CI/CD Automation and Integration](#microsoft-fabric-and-azure-devops-cicd-automation-and-integration)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [React2Shell Vulnerability Response Across Microsoft Defender and Azure](#react2shell-vulnerability-response-across-microsoft-defender-and-azure)
  - [GitHub Security Ecosystem Updates: Dependabot uv Support, Code Scanning, Secret Management](#github-security-ecosystem-updates-dependabot-uv-support-code-scanning-secret-management)
  - [Evolving Cloud and Identity Security: TLS, Managed Identities, and Access Fabric Strategies](#evolving-cloud-and-identity-security-tls-managed-identities-and-access-fabric-strategies)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot expands with new AI models (GPT-5.2, Claude Opus 4.5, Gemini 3 Flash), Agent Skills for reusable workflows, and Mission Control for managing automated agents—continuing its evolution into a complete developer productivity platform.

### AI Model Releases and Chat Model Availability

GitHub Copilot’s list of supported AI models continues to grow. GPT-5.2 is now available on all plans, building on earlier GPT-5.1, GPT-5.1-Codex, and Codex-Max releases. Developers can access these models directly in supported tools, including VS Code, Visual Studio, JetBrains IDEs, Xcode, Eclipse, GitHub Mobile, and the GitHub website. The recent addition of Claude Opus 4.5 offers another model for users, while Gemini 3 Flash has entered public preview—demonstrating Copilot's ongoing focus on giving organizations flexibility and more administrative control. These tools grant transparency and allow real-time model selection, making it easier to match team needs and governance policies.

- [GPT-5.2 is now generally available in GitHub Copilot](https://github.blog/changelog/2025-12-17-gpt-5-2-is-now-generally-available-in-github-copilot)
- [GPT-5.1 and GPT-5.1-Codex Now Available in GitHub Copilot](https://github.blog/changelog/2025-12-17-gpt-5-1-and-gpt-5-1-codex-are-now-generally-available-in-github-copilot)
- [GPT-5.1-Codex-Max Now Available in GitHub Copilot Across Platforms](https://github.blog/changelog/2025-12-17-gpt-5-1-codex-max-is-now-generally-available-in-github-copilot)
- [Claude Opus 4.5 Now Available in GitHub Copilot](https://github.blog/changelog/2025-12-18-claude-opus-4-5-is-now-generally-available-in-github-copilot)
- [Gemini 3 Flash Public Preview Released for GitHub Copilot Users](https://github.blog/changelog/2025-12-17-gemini-3-flash-is-now-in-public-preview-for-github-copilot)

### Agent Skills and Automation Ecosystem

Developers can now use Agent Skills to organize and share reusable instructions following the open Skills.md standard. This helps standardize workflows and encourages sharing across teams. Community-curated repositories—such as 'github/awesome-copilot'—enable faster onboarding and foster workflow automation. These additions support earlier work on creating repeatable, integrated DevOps flows.

- [GitHub Copilot Now Supports Agent Skills](https://github.blog/changelog/2025-12-18-github-copilot-now-supports-agent-skills)
- [Agent Skills (Skills.md) in GitHub Copilot for Visual Studio Code]({{ "/2025-12-17-Agent-Skills-Skillsmd-in-GitHub-Copilot-for-Visual-Studio-Code.html" | relative_url }})

### Integrated Workflows and Mission Control

Mission Control is now available as a central tool for managing and observing agents, promoting better visibility into automated workflows. With real-time monitoring and tools for ensuring workflow consistency between local and cloud setups, Mission Control improves process management. The integration of Azure Boards with Copilot connects DevOps tracking directly with AI-aided code generation. New workflow options like manual branch selection and feedback tracking through Kanban boards also add to operational flexibility.

- [Manage All GitHub Copilot Agents with Mission Control]({{ "/2025-12-16-Manage-All-GitHub-Copilot-Agents-with-Mission-Control.html" | relative_url }})
- [Azure Boards Now Integrates Directly with GitHub Copilot Coding Agent](https://devblogs.microsoft.com/devops/github-copilot-for-azure-boards/)
- [GitHub Copilot Coding Agent: Practical Automation Examples](https://devopsjournal.io/blog/2025/12/20/Copilot-Agent-example)

### Code Review, Security, and Metrics

With the latest update, organizations can enable Copilot code review feedback for all contributors—even those without licenses—which broadens accessibility and usage across teams. New features have been added for tracking review actions, managing usage metrics, and monitoring team-level activity. Mission Control integrates security scanning tools like CodeQL and ESLint, further supporting compliance needs. Administrators now have access to detailed metrics for pull request activity and Copilot usage, making tracking and reporting more straightforward.

- [Copilot Code Review Now Available for Organization Members Without a License](https://github.blog/changelog/2025-12-17-copilot-code-review-now-available-for-organization-members-without-a-license)
- [GitHub Copilot Code Review Preview Features Now Available in Enterprise Cloud with Data Residency](https://github.blog/changelog/2025-12-18-copilot-code-review-preview-features-now-supported-in-github-enterprise-cloud-with-data-residency)
- [Enhanced Copilot Autofix Metrics for CodeQL Security Overview on GitHub](https://github.blog/changelog/2025-12-16-more-accurate-copilot-autofix-usage-metrics-on-security-overview)
- [Enterprise Pull Request Metrics Now Included in GitHub Copilot Usage Metrics API](https://github.blog/changelog/2025-12-18-enterprise-level-pull-request-activity-metrics-now-in-public-preview)
- [Track Organization Copilot Usage with Copilot Usage APIs](https://github.blog/changelog/2025-12-16-track-organization-copilot-usage)

### IDE Integration, Debugging, and Language Support

Visual Studio 2026 delivers a new debugging experience powered by Copilot for faster startup and improved diagnostics. The Debugger Agent is now integrated with test tools, creating a more consistent workflow from coding through to solution diagnosis. New C++ code editing tools for VS 2026 Insiders enhance symbol support and refactoring in multilingual environments. Copilot-driven SQL features for VS Code are now generally available, with capabilities for traditional and vector database projects.

- [Debugging, but Without the Drama: Visual Studio 2026’s Copilot-Powered Experience](https://devblogs.microsoft.com/visualstudio/visual-studio-2026-debugging-with-copilot/)
- [C++ Code Editing Tools for GitHub Copilot in Visual Studio 2026 Insiders Public Preview](https://github.blog/changelog/2025-12-16-c-code-editing-tools-for-github-copilot-in-public-preview)
- [Next-Level SQL in VS Code: GitHub Copilot GA and AI-Ready SQL]({{ "/2025-12-17-Next-Level-SQL-in-VS-Code-GitHub-Copilot-GA-and-AI-Ready-SQL.html" | relative_url }})

### Prompt Engineering and Context Management

Recent guidance helps developers apply prompt patterns—like Persona and Reflection—to refine Copilot’s support for specific use cases. Copilot can now generate dynamic prompts linked directly from GitHub documentation, bridging learning resources and AI code suggestions for a smoother workflow.

- [Cook’n with GitHub Copilot: Recap of Context Engineering Prompt Patterns](https://www.cooknwithcopilot.com/blog/context-engineering-recipes-recap.html)
- [Dynamic Copilot Prompts Now Available on GitHub Docs](https://github.blog/changelog/2025-12-17-dynamic-copilot-prompts-on-github-docs)

### Advanced Agent Capabilities and Memory

Copilot introduces early access for repository-specific memory to Pro and Pro+ users, allowing agents to retain project knowledge and reduce repeated prompts. This addition follows ongoing improvements for agent persistence and more focused support on recurring automation.

- [Early Access Release: Copilot Memory for GitHub Copilot Pro and Pro+](https://github.blog/changelog/2025-12-19-copilot-memory-early-access-for-pro-and-pro)

### Real-World Usage and Ecosystem News

Developer onboarding metrics show strong Copilot adoption. This week’s discussions reinforce the importance of AI fluency, prompt design, and open governance through standards like the Model Context Protocol. Security news—including the React2Shell vulnerability—connects to wider conversations about platform resilience and best practices.

- [The Download: React2Shell Vulnerability, GPT 5.2 in GitHub Copilot, and Open Source News]({{ "/2025-12-21-The-Download-React2Shell-Vulnerability-GPT-52-in-GitHub-Copilot-and-Open-Source-News.html" | relative_url }})
- [A New Kind of Developer is Emerging on GitHub]({{ "/2025-12-21-A-New-Kind-of-Developer-is-Emerging-on-GitHub.html" | relative_url }})

### Effective Use of Copilot in Domain-Specific Contexts

A new resource explains how to configure Copilot for highly specialized languages and workflows. It covers using copilot-instructions.md, maintaining up-to-date reference files, and incorporating compiler validation to improve Copilot’s performance in unique environments. This pairs with community-led initiatives like DSL-Copilot and custom repository templates for real-world situations.

- [AI Coding Agents and Domain-Specific Languages: Challenges and Practical Mitigation Strategies](https://devblogs.microsoft.com/all-things-azure/ai-coding-agents-domain-specific-languages/)

### Other GitHub Copilot News

When users assign GitHub Copilot to an issue, they are now added as an assignee, further improving team transparency and workflow clarity.

- [Assigning GitHub Copilot to an Issue Now Also Assigns You](https://github.blog/changelog/2025-12-18-assigning-github-copilot-to-an-issue-now-adds-you-as-an-assignee)

## AI

Azure AI Foundry expands with hosted agents, Model Router GA, and broader LLM support including Claude, Sora 2, and Mistral. Microsoft Fabric adds AI-powered data transformation capabilities in Dataflow Gen2.

### Azure AI Foundry and the Expanding Agentic AI Ecosystem

Azure AI Foundry brings new open-source components for orchestrating secure and flexible AI agents. Hosted Agents now provide persistent memory and simplified management, with deployment and onboarding tools supporting rapid development. The bring-your-own-model (BYO Model Gateway) now includes broader LLM support, such as Claude, Sora 2, and Mistral. Developers can use the Model Router, now generally available, to optimize AI model usage and manage costs. New Foundry Tools and security enhancements with Entra Agent ID centralize AI governance. Feedback from Discord and GitHub channels continues to shape feature planning and onboarding.

- [What's New in Microsoft Foundry: Agents, Models, and Enterprise-Grade AI (October–November 2025)](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-oct-nov-2025/)
- [Getting Started with Azure AI Foundry: A Beginner’s Guide]({{ "/2025-12-15-Getting-Started-with-Azure-AI-Foundry-A-Beginners-Guide.html" | relative_url }})
- [Getting Started with Azure AI Foundry]({{ "/2025-12-15-Getting-Started-with-Azure-AI-Foundry.html" | relative_url }})

### Agent-Oriented Architecture: Durable Task Extension and Orchestration Guides

Durable Task Extension enhances agent orchestration, making it simpler to manage dependable workflows. Examples like the AI Travel Planner demonstrate how agents can operate in sequence or in parallel. Human oversight and rollback features are supported for easier troubleshooting. Guides using Azure Functions, Static Web Apps, and OpenTelemetry continue to encourage best practices for agent communication and security.

- [Building Reliable AI Travel Agents with the Durable Task Extension for Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-reliable-ai-travel-agents-with-the-durable-task/ba-p/4478913)
- [Best Practices for Architecting AI Agents in Enterprise Systems]({{ "/2025-12-16-Best-Practices-for-Architecting-AI-Agents-in-Enterprise-Systems.html" | relative_url }})

### Model Context Protocol (MCP) and Model Router in Developer Workflows

The Model Context Protocol (MCP) now features sessions that clarify how to link AI models, APIs, and data sources in daily work. Hands-on workshops and labs continue to build on this, while the new Model Router lets developers tune model use and integrate policies into agent workflows with better control.

- [Leveraging the Model Context Protocol (MCP) in Visual Studio Code for Enhanced Development]({{ "/2025-12-18-Leveraging-the-Model-Context-Protocol-MCP-in-Visual-Studio-Code-for-Enhanced-Development.html" | relative_url }})
- [Using Foundry's Model Router to Simplify Optimal AI Model Selection]({{ "/2025-12-15-Using-Foundrys-Model-Router-to-Simplify-Optimal-AI-Model-Selection.html" | relative_url }})

### Autonomous Agents and AI-Powered Data Transformation in Microsoft Fabric

Microsoft Fabric updates make agent consumption and billing reports more transparent, letting teams optimize usage and spending. Dataflow Gen2 gains AI-powered capabilities for prompt-based operations in Power Query, enabling summarization, classification, sentiment analysis, and translation directly—offering more advanced BI and analytics via easier-to-use interfaces and simple documentation.

- [Understanding Operations Agent Capacity Usage and Billing in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/understanding-operations-agent-capacity-consumption-usage-reporting-and-billing/)
- [AI-Powered Data Transformation with Dataflow Gen2 in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/from-simple-prompts-to-complex-insights-ai-expands-the-boundaries-of-data-transformation/)

## ML

Fabric Runtime 2.0 debuts with Apache Spark 4.0, while environment library management runs 2.5x faster and Python Spark sessions start 70% quicker.

### Microsoft Fabric ML Platform Advances

Fabric Runtime 2.0 (experimental) debuts with Apache Spark 4.0 for scalable distributed processing. Additional upgrades include Java 21, Scala 2.13, Python 3.12, and Delta Lake 4.0, aiding migration and analysis speed. The year-end review covers improvements in platform security, migration help, Copilot access, improved SQL/KQL tooling, and consistent DevOps support—summarizing a year centered on usability and developer needs.

- [Fabric Runtime 2.0 Experimental Preview: Scalable Data Engineering with Spark](https://blog.fabric.microsoft.com/en-US/blog/fabric-runtime-2-0-experimental-public-preview/)
- [Microsoft Fabric 2025 Recap: Unified Data and AI Innovations](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-2025-holiday-recap-unified-data-an-ai-innovation/)

### Performance, Reliability, and Security in ML Workflows

Fabric increases productivity with environment library management running up to 2.5 times faster for custom libraries, and Python Spark session startups now completing 70% quicker. New lightweight install modes are inbound for small deployments. Spark job orchestration supports Service Principal and Workspace Identity authentication, reducing reliance on user credentials in production pipelines. Updated documentation simplifies setup and migration.

- [Fabric Environment Library Management Performance Improvements for Developers](https://blog.fabric.microsoft.com/en-US/blog/fabric-environment-library-management-performance-improvement/)
- [Run Spark Job Definitions in Pipelines with Service Principal or Workspace Identity](https://blog.fabric.microsoft.com/en-US/blog/run-spark-job-definitions-in-pipelines-with-service-principal-or-workspace-identity/)

### Evaluation and Best Practices for Azure-based Document AI Pipelines

A practical guide outlines deploying and evaluating document AI workflows with Azure. The resource covers building a ground truth set, technical steps (OCR, labeling, retrieval), error assessment, and performance tuning with continuous monitoring. It includes architecture diagrams and code examples for developers working on enterprise IDP projects.

- [Evaluation Frameworks for Document Pipelines Using Azure AI & Search](https://techcommunity.microsoft.com/t5/azure-architecture-blog/from-large-semi-structured-docs-to-actionable-data-in-depth/ba-p/4474060)

## Azure

Azure networking gains 400 Gbps ExpressRoute and improved VPN Gateway throughput, while Azure Service Bus Premium adds geo-replication GA. BizTalk Server end-of-support (2030) prompts migration guidance to Logic Apps.

### Azure Networking, Resiliency, and Security Enhancements

This week’s networking improvements include 400 Gbps ExpressRoute options for high-demand workloads, increased VPN Gateway throughput, larger Virtual WAN and Route Server scaling, new forced tunneling and DNS Security rules, and automated failover scoring through ExpressRoute Resiliency Insights. A Copilot-based chatbot helps with networking troubleshooting.

Updates to Azure Front Door clarify new deployment procedures (configuration validation, rapid rollback, edge resilience validation) and future enhancement plans around safety and isolation. Note also the upcoming deprecation of Docker Content Trust for Azure Container Registry, with planned migration to the Notary Project.

- [Azure Networking 2025: Powering Cloud Innovation and AI at Global Scale](https://techcommunity.microsoft.com/t5/azure-networking-blog/azure-networking-2025-powering-cloud-innovation-and-ai-at-global/ba-p/4479390)
- [Azure Front Door: Hardening Configuration Resiliency After October Outages](https://techcommunity.microsoft.com/t5/azure-networking-blog/azure-front-door-implementing-lessons-learned-following-october/ba-p/4479416)
- [Azure Policy: Required Actions for Docker Content Trust Deprecation in Azure Container Registry](https://techcommunity.microsoft.com/t5/azure-governance-and-management/azure-policy-required-actions-for-docker-content-trust/ba-p/4478951)

### Migration, Modernization, and Strategic Guidance

Support for BizTalk Server ends in 2030; teams should migrate to Logic Apps and Integration Services. Existing mainframe connections remain supported through Host Integration Server, and tooling is available to help with these transitions.

Guidance for Oracle Database@Azure addresses deeper integrations and AI enablement, while Storage Account replication changes (LRS to GZRS) require planned downtime—see official resources for planning.

- [Microsoft BizTalk Server Product Lifecycle Update: Modernization and Migration Guidance](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/microsoft-biztalk-server-product-lifecycle-update/ba-p/4478559)
- [Modernizing Oracle Workloads with Oracle Database@Azure and AI Solutions](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/microsoft-hits-the-road-with-oracle-ai-world-tour-2026/ba-p/4477598)
- [Minimizing Downtime When Migrating Azure Storage Account Replication](https://techcommunity.microsoft.com/t5/azure/understanding-storage-account-replication-downtime/m-p/4479787#M22386)

### Developer Tools, Language Ecosystems, and Hands-On Guides

The December release of the Azure Developer CLI (v1.22.x) enhances extension management, distributed tracing, and resource management. The release supports easier onboarding and new agent-style extensions are in planning.

Updated guidance covers building and deploying MCP servers with Python, using Azure services for authorization, networking, and observability. Azure’s year-end Python review discusses top packages and workflow trends for Python developers.

- [Azure Developer CLI (azd) December 2025: Extensions, Foundry, and Pipeline Updates](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-december-2025/)
- [Building and Deploying MCP Servers with Python and Azure](https://techcommunity.microsoft.com/t5/microsoft-developer-community/learn-how-to-build-mcp-servers-with-python-and-azure/ba-p/4479402)
- [Python on Azure: 2025 Year in Review – Trends, Learning, and Future Directions]({{ "/2025-12-18-Python-on-Azure-2025-Year-in-Review-Trends-Learning-and-Future-Directions.html" | relative_url }})

### Messaging Patterns, Data Engineering, and Microsoft Fabric Features

Azure Service Bus Premium supports active geo-replication for improved continuity. Developers benefit from workflow patterns—sessions, scheduling, retries, and dead-letter handling—for robust agent backends. SQL Telemetry on Microsoft Fabric highlights new analytics options, with autoscaling, data quality tools, and real-time analytics. Fabric adds support for DATE_BUCKET() and Eventstream SQL operator, providing improved time-series and event-based workflows.

- [Announcing General Availability of Geo-Replication for Azure Service Bus Premium](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/announcing-general-availability-of-geo-replication-for-azure/ba-p/4413164)
- [Enterprise Messaging Patterns for Agentic AI Backends with Azure Service Bus](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/message-brokers-as-the-cornerstone-of-the-next-generation-of/ba-p/4478088)
- [Building a Petabyte-Scale Data Platform with Microsoft Fabric and SQL Telemetry](https://blog.fabric.microsoft.com/en-US/blog/sql-telemetry-intelligence-how-we-built-a-petabyte-scale-data-platform-with-fabric/)
- [Flexible Time-Based Reporting with DATE_BUCKET() in Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/date_bucket-function-in-fabric-data-warehouse/)
- [Fabric Eventstream SQL Operator: Real-Time Data Processing in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/fabric-eventstream-sql-operator-your-tool-kit-to-real-time-data-processing-in-fabric-real-time-intelligence/)
- [Microsoft Fabric Influencers Spotlight: December 2025 Highlights](https://blog.fabric.microsoft.com/en-US/blog/fabric-influencers-spotlight-december-2025/)

### Schema Management and Change-Driven Architecture

A new Azure schema language, JSON Structure, offers advanced typing, contract enforcement, and multi-language support. VS Code extensions and related tools simplify schema management. Drasi provides options for event-driven to change-driven migration, helping manage database change projections for phased modernization.

- [JSON Structure: A Next-Gen Schema Language for Messaging on Azure](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/json-structure-a-json-schema-language-you-ll-love/ba-p/4476852)
- [Event-Driven to Change-Driven: Low-cost Dependency Inversion](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/event-driven-to-change-driven-low-cost-dependency-inversion/ba-p/4478948)

### Other Azure News

Developers can now manage Azure NetApp Files from VS Code using an extension with AI-powered features. The Azure Update for December 19th covers improvements for Service Bus triggers, NetApp ransomware protection, data platform enhancements, and more. Azure Arc monthly forum brings upgrades for agent automation and Linux support. There are also troubleshooting guides for Azure Virtual Desktop and Data Gateway releases. Microsoft continues contributions to Fedora Linux and now offers GitHub Enterprise Cloud data residency in Japan. SQL Server and Azure SQL year-in-review posts track overall advancement in the data ecosystem.

- [Streamline Azure NetApp Files Management Directly from VS Code](https://techcommunity.microsoft.com/t5/azure-architecture-blog/streamline-azure-netapp-files-management-right-from-your-ide/ba-p/4478122)
- [Azure Update - 19th December 2025: Service Improvements, AI, and Security News]({{ "/2025-12-19-Azure-Update-19th-December-2025-Service-Improvements-AI-and-Security-News.html" | relative_url }})
- [Azure Arc Monthly Forum Recap – November 2025](https://techcommunity.microsoft.com/t5/azure-arc-blog/azure-arc-monthly-forum-recap-november-2025/ba-p/4478127)
- [Troubleshooting User Session Issues in Azure Virtual Desktop (Pooled Host Pools)](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/azure-virtual-desktop-pooled-sessions-ending-unexpectedly-and/m-p/4478548#M13967)
- [On-Premises Data Gateway December 2025 Release: Manual Update Preview and Power BI Desktop Compatibility](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-december-2025-release/)
- [Microsoft’s Contributions to Fedora Linux: Cloud Delivery, Security, and Azure Integration](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/building-bridges-microsoft-s-participation-in-the-fedora-linux/ba-p/4478461)
- [GitHub Enterprise Cloud Data Residency Now Available in Japan](https://github.blog/changelog/2025-12-18-github-enterprise-cloud-data-residency-in-japan-is-generally-available)
- [2025 SQL Year in Review: SQL Server, Azure SQL, and Fabric Updates](https://blog.fabric.microsoft.com/en-US/blog/2025-year-in-review-whats-new-across-sql-server-azure-sql-and-sql-database-in-fabric/)

## Coding

VS Code 1.107 launches inline chat editing and persistent local agents. ASP.NET Core kicks off .NET 11 planning with community input opportunities.

### .NET Development: Cross-Platform Widgets and ASP.NET Core Roadmaps

A .NET MAUI walkthrough demonstrates how to build interactive iOS widgets leveraging shared .NET logic and integrating Swift for platform-specific needs. ASP.NET Core’s .NET 11 planning is underway, with opportunities for community input and transparent discussions—helping teams prepare for migration and long-term design decisions.

- [Building iOS Widgets with .NET MAUI: From Setup to Interactive Features](https://devblogs.microsoft.com/dotnet/how-to-build-ios-widgets-with-dotnet-maui/)
- [ASP.NET Core Planning Kickoff for .NET 11]({{ "/2025-12-16-ASPNET-Core-Planning-Kickoff-for-NET-11.html" | relative_url }})
- [ASP.NET Core Server & APIs Roadmap Discussion for .NET 11]({{ "/2025-12-19-ASPNET-Core-Server-and-APIs-Roadmap-Discussion-for-NET-11.html" | relative_url }})

### Editor Experiences: VS Code and Cursor AI Updates

VS Code 1.107 launches inline chat editing, advanced renaming, and persistent local agents for background tasks. Cursor AI Editor 2.2 introduces a visual workflow designer and quick access to LLM options but continues to draw developer concerns over frequent interface changes and complex pricing. These updates feed into broader conversations about balancing speed, usability, and control.

- [VS Code 1.107 Release Highlights]({{ "/2025-12-16-VS-Code-1107-Release-Highlights.html" | relative_url }})
- [AI-Driven Cursor Editor Adds Visual Designer Amid Developer Frustrations](https://devclass.com/2025/12/16/cursor-ai-editor-gets-visual-designer-but-bugs-and-ever-changing-ui-irk-developers/)

### Practical Profiling and Feedback Workflows

A new guide details profiling the .NET CLR using C# and Silhouette, removing the dependency on C++ for diagnostics and performance monitoring. Another post explains how Visual Studio’s feedback process uses transparent triage and prioritization to connect developer suggestions directly to product improvements.

- [Creating a .NET CLR profiler with C# NativeAOT and Silhouette](https://andrewlock.net/creating-a-dotnet-profiler-using-csharp-with-silhouette/)
- [How Visual Studio Feedback Improves the Developer Experience](https://devblogs.microsoft.com/visualstudio/behind-the-scenes-of-the-visual-studio-feedback-system/)

## DevOps

GitHub Actions will charge for self-hosted runners starting March 2026 ($0.002/min on private repos). Dependabot adds support for Conda, OpenTofu, Julia, and Bazel ecosystems.

### GitHub Actions Self-Hosted Runners: Pricing Changes, Roadmap, and Community Impact

Starting in March 2026, GitHub will charge for self-hosted Actions runners on private repositories ($0.002/min), while maintaining free access on public repositories and Enterprise Server. Lower costs for hosted runners offset the change for most users, but planning and migration resources are central to the transition. Competitive alternatives (e.g., Depot), coming platform support, better workflow metrics, and ongoing documentation updates aim to ease migration.

- [GitHub to Charge for Self-Hosted Runners on GitHub Actions Starting March 2026](https://devclass.com/2025/12/17/github-to-charge-for-self-hosted-runners-from-march-2026/)
- [GitHub Actions Pricing Update: Lower Runner Costs, Platform Enhancements Coming in 2026](https://github.blog/changelog/2025-12-16-coming-soon-simpler-pricing-and-a-better-experience-for-github-actions)

### Dependabot Expands Ecosystem: Conda, OpenTofu, Julia, Bazel Automated Updates

Dependabot now offers automatic updates for Conda (popular with Python/data science), OpenTofu (IaC), Julia (scientific), and Bazel (build systems), broadening security and maintenance coverage. Documentation and troubleshooting resources help support this expanded ecosystem.

- [Dependabot Adds Conda Ecosystem Support for Automated Version Updates](https://github.blog/changelog/2025-12-16-conda-ecosystem-support-for-dependabot-now-generally-available)
- [Dependabot Adds Support for OpenTofu Dependency Updates](https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-opentofu)
- [Dependabot Adds Version Update Support for Julia](https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-julia)
- [Dependabot Version Updates Now Support Bazel](https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-bazel)

### Azure DevOps Productivity: Microsoft.Testing.Platform Integration and Security Workflows

Azure DevOps now fully supports Microsoft.Testing.Platform—streamlining .NET test commands, automatic retries, and publishing of results. Vulnerability management links GitHub Advanced Security alerts to Azure DevOps Boards, streamlining issue triage and resolution.

- [Azure DevOps Gains Full Support for Microsoft.Testing.Platform](https://devblogs.microsoft.com/dotnet/microsoft-testing-platform-azure-retry/)
- [Work Item Linking for GitHub Advanced Security Alerts in Azure DevOps Now Available](https://devblogs.microsoft.com/devops/work-item-linking-for-advanced-security-alerts-now-available/)

### Microsoft Fabric and Azure DevOps: CI/CD Automation and Integration

Microsoft Fabric now features direct guides for automating SQL database deployment with Azure DevOps. Secure connection support (service principal, OAuth 2.0) enables better CI/CD integration and supports complex automation scenarios across cloud environments.

- [Performing CI/CD for SQL Databases in Fabric Using Azure DevOps]({{ "/2025-12-15-Performing-CICD-for-SQL-Databases-in-Fabric-Using-Azure-DevOps.html" | relative_url }})
- [How to Connect Microsoft Fabric to Azure DevOps Using Service Principal](https://blog.fabric.microsoft.com/en-US/blog/how-to-connect-microsoft-fabric-to-azure-devops-using-service-principal/)

### Other DevOps News

GitHub Teams administration is now consolidated under the 'Settings → Teams' menu, promoting easier management. The Azure SRE Agent automates incident response by running playbooks and collecting evidence when triggered by alerts in PagerDuty, ServiceNow, or Azure Monitor—removing the need for manual intervention in multi-cloud environments.

- [Teams Management Relocated to Settings in GitHub](https://github.blog/changelog/2025-12-18-teams-management-now-moved-to-settings)
- [Automating On-Call Runbooks with Azure SRE Agent for Incident Response](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/stop-running-runbooks-at-3-am-let-azure-sre-agent-do-your-on/ba-p/4479811)

## Security

React2Shell (CVE-2025-55182) affects Next.js/Node.js workloads—Microsoft provides Defender and Azure WAF mitigation guidance. GitHub expands Dependabot with uv support and requires peer review for alert dismissals.

### React2Shell Vulnerability Response Across Microsoft Defender and Azure

React2Shell (CVE-2025-55182) affects Next.js and Node.js workloads, with attackers exploiting React Server Component build pipelines. Recommendations include updating to secured frameworks, scanning assets with Microsoft Defender, setting up custom Azure WAF rules, and using Sentinel or Security Copilot for further analysis. Teams should establish a combination of automated and manual incident handling.

- [Mitigating CVE-2025-55182 (React2Shell) with Microsoft Defender for Endpoint and Azure WAF](https://www.microsoft.com/en-us/security/blog/2025/12/15/defending-against-the-cve-2025-55182-react2shell-vulnerability-in-react-server-components/)

### GitHub Security Ecosystem Updates: Dependabot uv Support, Code Scanning, Secret Management

Dependabot now supports uv packages, improving automated vulnerability tracking. Code scanning alert assignment via REST API is now generally available. CodeQL improvements boost detection for Go and Rust. Secret scanning governance has been expanded, and dismissing Dependabot alerts now requires a peer review. More organizations can access Advanced Security trials with the latest expansion.

- [Dependabot Security Updates Now Support uv](https://github.blog/changelog/2025-12-16-dependabot-security-updates-now-support-uv)
- [General Availability of Code Scanning Alert Assignees in GitHub](https://github.blog/changelog/2025-12-16-code-scanning-alert-assignees-are-now-generally-available)
- [CodeQL 2.23.7 and 2.23.8 Released: Enhanced Security Queries for Go and Rust](https://github.blog/changelog/2025-12-18-codeql-2-23-7-and-2-23-8-add-security-queries-for-go-and-rust)
- [Enterprise Governance and Policy Improvements for GitHub Secret Scanning](https://github.blog/changelog/2025-12-16-enterprise-governance-and-policy-improvements-for-secret-scanning-now-generally-available)
- [GitHub Advanced Security Trials Expanded for Enterprise Customers](https://github.blog/changelog/2025-12-18-github-advanced-security-trials-now-available-for-more-github-enterprise-customers)
- [Require Reviews for Dependabot Alert Dismissal with Delegated Alert Dismissal in GitHub](https://github.blog/changelog/2025-12-19-you-can-now-require-reviews-before-closing-dependabot-alerts-with-delegated-alert-dismissal)

### Evolving Cloud and Identity Security: TLS, Managed Identities, and Access Fabric Strategies

Azure App Service users should prepare for upcoming TLS certificate and authentication changes. Managed Identities for Azure Files SMB allow password-free access for automated agents, AKS nodes, and cloud applications. Microsoft’s Access Fabric moves device and network checks directly into access enforcement, supporting Zero Trust principles.

- [Preparing for Industry-wide TLS Certificate Changes in Azure App Service (2026 Update)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/industry-wide-certificate-changes-impacting-azure-app-service/ba-p/4477924)
- [Secure Access with Managed Identities for Azure Files SMB](https://techcommunity.microsoft.com/t5/azure-storage-blog/secure-seamless-access-using-managed-identities-with-azure-files/ba-p/4477565)
- [Access Fabric: Modernizing Identity and Network Access Security with Microsoft Entra](https://www.microsoft.com/en-us/security/blog/2025/12/17/access-fabric-a-modern-approach-to-identity-and-network-access/)

### Other Security News

A Microsoft e-book explains the benefits of unified, AI-capable security platforms (Defender, Sentinel, Copilot) for incident management. Also available is a practical guide for configuring Sensitivity Labels in Microsoft Teams, employing Purview Information Protection for automated policies, encryption, and compliance.

- [Why Unified, AI-Ready Security Platforms Outperform Patchwork Solutions](https://www.microsoft.com/en-us/security/blog/2025/12/18/new-microsoft-e-book-3-reasons-point-solutions-are-holding-you-back/)
- [How To Use Sensitivity Labels in Microsoft Teams (Step-by-Step Guide)](https://dellenny.com/how-to-use-sensitivity-labels-in-microsoft-teams-step-by-step-guide/)
