---
layout: "post"
title: "Updates to Copilot, Expanding Azure AI, and Changing DevOps Workflows"
description: "This week’s roundup features new developments in AI coding and automation, including GitHub Copilot’s added collaboration features and Azure’s enhanced AI and DevOps tools. Topics include enterprise-scale data integration, updated security controls, and workflow improvements, as teams use new models, refined practices, and better governance. Key highlights cover AI agent orchestration, expanded Copilot integration with IDEs, latest cloud data tools, and actionable updates for code and infrastructure security."
author: "Tech Hub Team"
excerpt_separator: <!--excerpt_end-->
viewing_mode: "internal"
date: 2025-09-08 09:00:00 +00:00
permalink: "/2025-09-08-Weekly-AI-and-Tech-News-Roundup.html"
categories: ["AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security"]
tags: [".NET", "Agent Workflows", "AI", "AI Agents", "Azure", "Azure AI Foundry", "Azure App Service", "Cloud Automation", "Coding", "DevOps", "Enterprise Automation", "GitHub Actions", "GitHub Copilot", "Kubernetes", "Machine Learning", "MCP", "ML", "OneLake", "Power Platform", "Roundups", "Security", "Team Collaboration"]
tags_normalized: ["dotnet", "agent workflows", "ai", "ai agents", "azure", "azure ai foundry", "azure app service", "cloud automation", "coding", "devops", "enterprise automation", "github actions", "github copilot", "kubernetes", "machine learning", "mcp", "ml", "onelake", "power platform", "roundups", "security", "team collaboration"]
---

Welcome to this week’s technology roundup, where new updates are shaping practical change for developers, IT pros, and leaders working across enterprise platforms. GitHub Copilot remains focused on smarter coding assistance, rolling out added options for reusable prompts, expanded customization, and agent-based automation for collaborative teams. As Copilot strengthens its integration with daily workflows—from migrating legacy code to team-based review policies—Microsoft’s ongoing progress in cloud platforms and AI automation stands out.

Azure continues to expand with new features in the AI Foundry platform, bringing the latest GPT-5 models and a growing ecosystem of agent development tools for creating scalable, secure, and cost-effective AI solutions. This week’s improvements in DevOps, machine learning data platforms, and security—including unified governance in OneLake and stronger CI/CD protections on GitHub—show the merging of operational reliability with AI-powered productivity. Whether you’re following new cloud infrastructure, experimenting with AI-powered coding, or modernizing enterprise data and security workflows, this edition delivers concise updates and expert knowledge to support your next steps.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Copilot Customization, Prompts, and Instructions](#copilot-customization-prompts-and-instructions)
  - [Copilot Agents, MCP Server, and Automated Workflows](#copilot-agents-mcp-server-and-automated-workflows)
  - [Copilot in Visual Studio: Embedded Intelligence and Productivity](#copilot-in-visual-studio-embedded-intelligence-and-productivity)
  - [AI Agents and Spec-Driven Development](#ai-agents-and-spec-driven-development)
  - [Legacy Migration and Real-World Copilot Guidance](#legacy-migration-and-real-world-copilot-guidance)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Azure AI Foundry Platform and Agent Ecosystem](#azure-ai-foundry-platform-and-agent-ecosystem)
  - [AI Agent Workflows, Context Engineering, and Community Learning](#ai-agent-workflows-context-engineering-and-community-learning)
  - [Azure AI Copilot Studio for Workflow and Customer Service Automation](#azure-ai-copilot-studio-for-workflow-and-customer-service-automation)
  - [Tutorials: AI Apps, Assistants, and Model-Driven Capabilities](#tutorials-ai-apps-assistants-and-model-driven-capabilities)
  - [AI Coding Tools and Code Generation Governance](#ai-coding-tools-and-code-generation-governance)
  - [AI for Accessibility and Inclusive Technology](#ai-for-accessibility-and-inclusive-technology)
- [ML](#ml)
  - [Microsoft OneLake as an AI-Ready Data Platform](#microsoft-onelake-as-an-ai-ready-data-platform)
  - [Accelerating Data Ingestion with Fabric Fast Copy](#accelerating-data-ingestion-with-fabric-fast-copy)
- [Azure](#azure)
  - [Azure App Service Enhancements](#azure-app-service-enhancements)
  - [Microsoft Fabric and Eventstream Updates](#microsoft-fabric-and-eventstream-updates)
  - [Azure Platform Security and Compliance](#azure-platform-security-and-compliance)
  - [Service Reliability, SRE Tooling, and Cloud Governance](#service-reliability-sre-tooling-and-cloud-governance)
  - [Cloud Telephony and Communication Services](#cloud-telephony-and-communication-services)
  - [Azure Integration Services and Logic Apps](#azure-integration-services-and-logic-apps)
  - [Cost Management, Licensing, and Cloud Migration Guidance](#cost-management-licensing-and-cloud-migration-guidance)
  - [Architecture, Training, and Developer Enablement](#architecture-training-and-developer-enablement)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET Ecosystem: Workflow Improvements and Community Progress](#net-ecosystem-workflow-improvements-and-community-progress)
  - [Workflow Automation and AI in VS Code With MCP](#workflow-automation-and-ai-in-vs-code-with-mcp)
  - [SharePoint and Microsoft Teams: Customization, Troubleshooting, and Integration](#sharepoint-and-microsoft-teams-customization-troubleshooting-and-integration)
- [DevOps](#devops)
  - [GitHub Platform Updates and Security Improvements](#github-platform-updates-and-security-improvements)
  - [AI-Powered DevOps Collaboration and Actionable Observability](#ai-powered-devops-collaboration-and-actionable-observability)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Azure Security Enhancements for Linux, VMs, and Workspaces](#azure-security-enhancements-for-linux-vms-and-workspaces)
  - [Securing Kubernetes and Cloud Developer Workflows](#securing-kubernetes-and-cloud-developer-workflows)
  - [GitHub Security Ecosystem: Code Scanning and Campaigns](#github-security-ecosystem-code-scanning-and-campaigns)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot introduced new features and updates this week that reinforce its position as a collaborative coding assistant, offering deeper integrations, improved customization, and expanded controls for organizations. Following previous themes—better context handling, automated workflows, and supporting enterprise use—Copilot continues to boost reusable assets, flexible configuration, and AI-powered automation for tasks like legacy code updates, review processes, and routine coding chores. The following sections summarize these new advances and provide detailed resources, connecting closely to last week's updates.

### Copilot Customization, Prompts, and Instructions

Building on earlier efforts for personal and team-wide customization (such as BYOM and configuration files), Copilot now enables new collaboration tools using reusable markdown prompt files in Visual Studio. Teams can create and share these prompts, supporting project-specific context and guidance—an approach similar to Copilot Spaces and previous instruction sharing updates.

Copilot code review now supports path-scoped instruction files, focusing on exact code areas rather than general guidelines. This change aligns with recent improvements in IDE customization and code suggestion controls.

A new guide offers tips for writing and editing Copilot instructions, building on previous onboarding resources and refining agent-based guidance strategies.

- [Boost Your Copilot Collaboration with Reusable Prompt Files](https://devblogs.microsoft.com/visualstudio/boost-your-copilot-collaboration-with-reusable-prompt-files/)
- [Path-Scoped Custom Instructions for Copilot Code Review](https://github.blog/changelog/2025-09-03-copilot-code-review-path-scoped-custom-instruction-file-support)
- [5 Tips for Crafting Better Custom Instructions for GitHub Copilot](https://github.blog/ai-and-ml/github-copilot/5-tips-for-writing-better-custom-instructions-for-copilot/)

### Copilot Agents, MCP Server, and Automated Workflows

Extending coverage of MCP integration and serverless hosting, the remote GitHub MCP Server has reached general availability, supporting broader agent integration and automation across multiple IDEs—consistent with last week’s Model Context Protocol and agent focus.

A new walkthrough explains using Copilot Coding Agent to automate .NET development, adding to recent workflow automation discussion and showing how agent-based DevOps can streamline development pipelines.

The MCP elicitation update improves Copilot’s adaptability with smarter API and schema-driven prompting, representing ongoing refinement in automated prompt development.

GitHub’s new agents panel now offers AI-driven suggestions, reviews, and context controls directly in browser workflows, reducing interruptions and enhancing the overall developer experience.

- [Remote GitHub MCP Server Now Generally Available](https://github.blog/changelog/2025-09-04-remote-github-mcp-server-is-now-generally-available)
- [Automating .NET Development with GitHub Copilot Coding Agent](https://devblogs.microsoft.com/dotnet/copilot-coding-agent-dotnet/)
- [Building Smarter AI Tool Interactions with MCP Elicitation](https://github.blog/ai-and-ml/github-copilot/building-smarter-interactions-with-mcp-elicitation-from-clunky-tool-calls-to-seamless-user-experiences/)
- [A First Look at the New Copilot Agents Panel on GitHub]({{ "/2025-09-01-A-First-Look-at-the-New-Copilot-Agents-Panel-on-GitHub.html" | relative_url }})

### Copilot in Visual Studio: Embedded Intelligence and Productivity

Copilot’s new chat support in Visual Studio’s Output Window introduces real-time diagnostics and AI-powered feedback in .NET workflows. These advances strengthen Copilot’s presence within the IDE, enabling faster problem-solving and deeper integration with existing tools.

- [Make Sense of Your Output Window with Copilot in Visual Studio](https://devblogs.microsoft.com/visualstudio/make-sense-of-your-output-window-with-copilot/)

### AI Agents and Spec-Driven Development

Copilot’s shift toward more reliable agent workflows continues with Spec Kit—a toolkit for spec-driven development—supporting context engines and modular code automation. By maintaining live specifications and automating code, Copilot proves useful for both new projects and updating existing codebases.

A Copilot Studio episode explores agent-to-agent collaboration, offering practical tools and previews for automation and oversight in enterprise settings.

- [Adopting Spec-Driven Development with AI: Introducing Spec Kit](https://github.blog/ai-and-ml/generative-ai/spec-driven-development-with-ai-get-started-with-a-new-open-source-toolkit/)
- [Agent-to-Agent Collaboration in Copilot Studio]({{ "/2025-09-03-Agent-to-Agent-Collaboration-in-Copilot-Studio.html" | relative_url }})

### Legacy Migration and Real-World Copilot Guidance

A migration guide illustrates Copilot’s support for modernizing legacy code, providing steps for incremental upgrades, test organization, and review methods. This complements recent case studies showing how Copilot can support production environments beyond greenfield development.

The “vibe coding” guide further explores prompt curation, enabling diverse teams to influence Copilot’s output—a natural extension of earlier guides on workflow customization.

- [How to Migrate Legacy Applications Using GitHub Copilot](https://dellenny.com/how-to-migrate-legacy-applications-using-github-copilot/)
- [How to Get the Most Out of Your AI with Vibe Coding]({{ "/2025-09-02-How-to-Get-the-Most-Out-of-Your-AI-with-Vibe-Coding.html" | relative_url }})

### Other GitHub Copilot News

Enterprise management for Copilot improved with the public preview release of Enterprise Teams for GitHub Enterprise Cloud. These features enable group administration, easier license management, and added SCIM support for streamlined enterprise deployments—continuing the drive for scalable Copilot use and business administration.

- [Managing GitHub Copilot Business Licenses with Enterprise Teams (Public Preview)](https://github.blog/changelog/2025-09-04-manage-copilot-and-users-via-enterprise-teams-in-public-preview)

## AI

AI developments this week focused on Microsoft’s cloud platforms, with Azure AI Foundry introducing updates, new GPT-5 models, and better agent-development resources. These changes provide a stronger platform for deploying AI agents and building solutions that address complex automation needs. Highlights include the expanded GPT-5 family, Model Router for flexible optimization, open agent protocols, actionable tutorials, and blueprints for scaling AI-powered workflows.

### Azure AI Foundry Platform and Agent Ecosystem

Azure AI Foundry extended last week’s Agent Factory and GPT-5/mini news by releasing the full GPT-5 series, now featuring larger context windows and variable model choices. The Model Router enables real-time switching based on cost and latency.

Sora API updates strengthen Azure’s document understanding and generative vision tools, continuing last week’s integration work and supporting hybrid workflows.

SDK/tooling upgrades across Python, .NET, Java, and JavaScript provide migration and interoperability enhancements. New OpenTelemetry tracing and compliance utilities focus on auditability and transparency.

Playwright browser automation updates enhance agent-driven workflows, progressing on previous orchestration and protocol work. New regional support and expanded API features allow multi-turn interactions for agents deployed in production.

Documentation now covers migration, disaster recovery, and cost modeling so developers can take projects from prototype to production with less friction.

- [What’s New in Azure AI Foundry: August 2025 Release Highlights](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-august-2025/)
- [Agent Factory: From Prototype to Production—Developer Tools and Rapid Agent Development](https://azure.microsoft.com/en-us/blog/agent-factory-from-prototype-to-production-developer-tools-and-rapid-agent-development/)
- [Context Window: 3 Azure AI Foundry Community Questions Answered]({{ "/2025-09-02-Context-Window-3-Azure-AI-Foundry-Community-Questions-Answered.html" | relative_url }})

### AI Agent Workflows, Context Engineering, and Community Learning

More tutorials this week expand practical agent workflow guidance, especially around managing context, missing or irrelevant inputs, and scaling across tenants. A shopping AI agent walkthrough demonstrates persistent memory using a multi-tenant architecture.

The ongoing livestream series covers Python, generative AI, multi-agent approaches, and Model Context Protocol, now with additional Spanish-language tracks. These sessions offer learning opportunities for developers at all skill levels.

- [Context Engineering for AI Agents]({{ "/2025-09-04-Context-Engineering-for-AI-Agents.html" | relative_url }})
- [Build a Smart Shopping AI Agent with Memory Using Azure AI Foundry Agent Service](https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-a-smart-shopping-ai-agent-with-memory-using-the-azure-ai/ba-p/4450348)
- [Level Up Your Python Game with Generative AI: Free Livestream Series](https://techcommunity.microsoft.com/t5/microsoft-developer-community/level-up-your-python-game-with-generative-ai-free-livestream/ba-p/4450646)

### Azure AI Copilot Studio for Workflow and Customer Service Automation

This week’s Copilot Studio best-practice guides and retail case studies show how generative AI combines with business logic for workflow automation. Content features layered architectures, Power Automate integration, and real-time connectors for retail, making customer service automation practical and measurable.

Security and analytics progress builds on previous topics around RBAC and data loss prevention. These guides emphasize compliance as Copilot automates more business processes.

- [Combining Generative AI and Business Logic with Copilot Studio](https://dellenny.com/combining-generative-ai-and-business-logic-with-copilot-studio/)
- [Automating Retail Customer Service with Copilot Studio](https://dellenny.com/how-retail-businesses-are-automating-customer-service-with-copilot-studio/)

### Tutorials: AI Apps, Assistants, and Model-Driven Capabilities

This set of tutorials addresses practical deployment, showing how to configure AI-powered assistants with context enrichment and robust orchestration for production use.

The image-caption generator tutorial utilizes Azure AI Vision and large language models, highlighting managed identity, infrastructure-as-code, and modular extensions for tasks like object detection and cloud storage.

- [Deploying a Self-Hosted Microsoft Docs AI Assistant with Azure OpenAI and MCP](https://devblogs.microsoft.com/all-things-azure/build-your-own-microsoft-docs-ai-assistant-with-azure-container-apps-and-azure-openai/)
- [Build an AI Image-Caption Generator on Azure App Service with Streamlit and GPT-4o-mini](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-an-ai-image-caption-generator-on-azure-app-service-with/ba-p/4450313)

### AI Coding Tools and Code Generation Governance

A recent review details how ChatGPT-5 and other code-generation tools offer better reasoning and error avoidance, although increased costs and complexity remain concerns. This continues last week’s discussion about the need for careful quality assurance and centralized review when deploying AI-powered IDEs.

Guides and case studies reinforce the benefits of having shared approval and governance for enterprise-level code generation.

- [Report: ChatGPT-5 Coding Gains Come at a Higher Cost](https://devops.com/report-chatgpt-5-coding-gains-come-at-a-higher-cost/?utm_source=rss&utm_medium=rss&utm_campaign=report-chatgpt-5-coding-gains-come-at-a-higher-cost)
- [Can Vibe Coding Work on an Enterprise Level?](https://devops.com/can-vibe-codingwork-on-an-enterprise-level/?utm_source=rss&utm_medium=rss&utm_campaign=can-vibe-codingwork-on-an-enterprise-level)

### AI for Accessibility and Inclusive Technology

An Argus case study showcases Azure AI Foundry’s use in assistive technology, demonstrating real-world speech interfaces and cloud inference capabilities. Hybrid protocol (Wi-R) notes extend prior coverage on combining hardware and software efficiently, offering practical value for accessibility and robustness.

- [Argus and Azure AI: Inclusive Assistive Tech Triumphs at Imagine Cup](https://www.microsoft.com/en-us/startups/blog/could-your-startup-be-the-next-imagine-cup-world-champion/)

## ML

Machine learning news this week highlighted foundational data improvements for AI and analytics workflows, with new capabilities in OneLake and Fabric Dataflow Gen2 making integration and ingestion more efficient. These tools add simplicity and scale to Microsoft’s enterprise ML offerings.

Recent advances in large model training (Azure ND GB200 v6) and workflow safety now show up as refined data layers, supporting more reliable ML operations. OneLake’s focus on governed, secure data and Fabric Fast Copy’s speed improvements help AI apps move into production more easily.

### Microsoft OneLake as an AI-Ready Data Platform

Microsoft OneLake is now presented as a single data platform for AI and analytics. With features like near real-time sync, Shortcuts, and Mirroring for different sources (Azure Data Lake Storage, Amazon S3, Dataverse, Azure Cosmos DB, Azure SQL, PostgreSQL, Databricks Unity, and Snowflake), users can avoid duplication and complex ETL processes. Support for open file formats (Delta Parquet, Apache Iceberg) makes master data accessible.

The OneLake Catalog adds centralized governance with metadata, lineage tracking, label support, and role-based security. Purview integration provides audit and compliance capabilities so ML and AI apps can use operational data while following policy requirements.

Direct connections with Azure AI Foundry, AI Search, and Fabric Data Agents allow chat analytics and generative projects to use OneLake data seamlessly. Customer stories show reduced friction, lower costs, and quicker results, with more enhancements on the way.

- [OneLake: your foundation for an AI-ready data estate](https://blog.fabric.microsoft.com/en-US/blog/onelake-your-foundation-for-an-ai-ready-data-estate/)

### Accelerating Data Ingestion with Fabric Fast Copy

Fabric Dataflow Gen2 now includes Fast Copy, enabling faster SQL-to-OneLake dataset imports. Developers can configure source and destination paths, track logs, launch bulk loading, and use parallel execution for big jobs (or small jobs as needed).

Guides explain setup details and current limitations (SQL only—more data sources planned), supporting recent coverage on workflow integrity and Azure performance benchmarking.

- [Accelerating Data Ingestion from SQL to Fabric with Fast Copy in Dataflow](https://blog.fabric.microsoft.com/en-US/blog/accelerating-data-movement-by-using-fast-copy-to-unlock-performance-and-efficiency-during-data-ingestion-from-sql-database-in-fabric/)

## Azure

Azure recently released updates and previews that enhance infrastructure management, modernization ease, integration tools, and cost controls. Developers now have better management features, added security options, reliability improvements, and new self-service capabilities. Many services reached general availability, with ongoing public previews making cloud administration more accessible.

### Azure App Service Enhancements

Azure App Service Premium v4 is now generally available, introducing updated hardware for Windows and Linux web apps. Built on AMD Dadsv6/Eadsv6 VMs with NVMe storage, these upgrades deliver around 58% improved throughput and responsiveness compared to previous models. New SKUs range from entry-level to high-capacity, supporting availability zones, monitoring, and modernization for migration and new deployments.

Self-service quota management has entered public preview, letting teams set and track quotas in the portal without support intervention. This empowers more proactive scaling for large app deployments and improves governance and performance management.

- [General Availability of Premium v4 for Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-of-premium-v4-for-azure-app/ba-p/4446204)
- [Public Preview: Self-Service Quota Management for Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-the-public-preview-of-the-new-app-service-quota-self/ba-p/4450415)

### Microsoft Fabric and Eventstream Updates

Fabric has seen new features like public preview for JSON Lines querying, ETL and ingestion with AzCopy, workspace Private Link, and Copilot/Agent SQL editor integration—improving enterprise analytics and secure data streaming.

Eventstream’s schema registry (public preview) now provides a unified Avro schema for pipeline type safety and contract management. Growing integration with Eventhouse and EventHub continues last week’s achievements in secure, reliable data movement.

- [What’s New in Fabric Warehouse: August 2025 Recap](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-warehouse-august-2025/)
- [Introducing Schema Registry for Type-Safe Pipelines in Microsoft Fabric Eventstreams](https://blog.fabric.microsoft.com/en-US/blog/schema-registry-creating-type-safe-pipelines-using-schemas-and-eventstreams-preview/)

### Azure Platform Security and Compliance

Azure Linux 3.0 is now Level 1 CIS Benchmark certified, facilitating out-of-the-box compliance with AKS and streamlining security for development and audit teams.

- [Azure Linux 3.0 Achieves Level 1 CIS Benchmark Certification](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-3-0-achieves-level-1-cis-benchmark-certification/ba-p/4450410)

### Service Reliability, SRE Tooling, and Cloud Governance

Azure SRE Agent has updated preview features for broader security defaults, resource support, and DevOps tool integration. New functions analyze incident causes and auto-create task follow-ups. Transparent usage-based billing begins in September, continuing the move toward actionable reliability.

- [Enterprise-Ready and Extensible: Update on the Azure SRE Agent Preview](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/enterprise-ready-and-extensible-update-on-the-azure-sre-agent/ba-p/4444299)

### Cloud Telephony and Communication Services

Teams Phone extensibility via Azure Communication Services is now generally available, allowing Teams calling to be built into custom workflows and agent experiences, with support for AI-based sentiment analysis and automation.

- [General Availability of Teams Phone Extensibility with Azure Communication Services](https://techcommunity.microsoft.com/t5/azure-communication-services/general-availability-of-teams-phone-extensibility/ba-p/4446092)

### Azure Integration Services and Logic Apps

Logic Apps Aviators Newsletter reports progress in hybrid integration and CI/CD, announcing GA for improved Data Mapper UI and new deployment options for Rancher K3s clusters. These advances extend the recent focus on secure, automated cloud app delivery.

- [Logic Apps Aviators Newsletter – September 25, 2025](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-aviators-newsletter-september-25/ba-p/4450195)

### Cost Management, Licensing, and Cloud Migration Guidance

Microsoft Cost Management now includes new automation tools, partner RBAC, better pricing calculators, and migration support for S3-to-Azure—continuing to simplify operations and hybrid deployment.

Broadcom VMware licensing changes for AVS (Azure VMware Solution) support BYOL, helping customers navigate new cloud migration and license management options.

A guide for migrating from AWS Application Load Balancer to Azure Application Gateway covers detailed resource mappings, validation strategies, and comprehensive advice for a smooth transition.

- [Microsoft Cost Management Updates: July & August 2025](https://azure.microsoft.com/en-us/blog/microsoft-cost-management-updates-july-august-2025/)
- [Broadcom VMware Licensing Changes Impacting Azure VMware Solution Customers](https://techcommunity.microsoft.com/t5/azure-migration-and/broadcom-vmware-licensing-changes-what-azure-vmware-solution/ba-p/4448784)
- [Migrating from AWS Application Load Balancer to Azure Application Gateway](https://techcommunity.microsoft.com/t5/azure-migration-and/migrating-application-load-balancer-from-aws-to-azure/ba-p/4439880)

### Architecture, Training, and Developer Enablement

Resources this week include improved Azure service discovery guides and a self-paced local training program for cloud management, supporting developer and administrator learning.

- [Service Discovery in Azure: Dynamically Finding Service Instances](https://dellenny.com/service-discovery-in-azure-dynamically-finding-service-instances/)
- [Azure Local Training Program: Self-Paced Cloud Management Skills](https://techcommunity.microsoft.com/t5/azure-architecture-blog/introducing-azure-local-training-empowering-you-to-succeed/ba-p/4447957)

### Other Azure News

SQL management tools in Fabric now provide upgraded security and workflow options, supporting previous orchestration improvements. New previews cover SQL auditing and customer-managed keys for stronger analytics security.

- [SQL Database in Fabric: What's New and Improved (Data Exposed Public Preview)]({{ "/2025-09-04-SQL-Database-in-Fabric-Whats-New-and-Improved-Data-Exposed-Public-Preview.html" | relative_url }})

A SharePoint governance guide shares templates, automation ideas, and lifecycle management advice to help organizations avoid excessive site growth while still enabling effective collaboration.

- [Avoiding SharePoint Sprawl Without Killing Collaboration](https://dellenny.com/avoiding-sharepoint-sprawl-without-killing-collaboration/)

## Coding

Coding news highlights better integration, developer tools, and practical guidance for .NET, Blazor, MAUI, VS Code, Teams, and SharePoint. Areas covered include migration, modernization, and improved productivity through AI assistance and user-friendly resources.

### .NET Ecosystem: Workflow Improvements and Community Progress

Recent .NET updates address modernization, with Blazor Internship Demo Fest unveiling new UI elements and streamlined media features for easier image/file use—extending improvements in desktop and web interfaces. Navigation and state management enhancements solve common workflow issues for app developers.

.NET MAUI Community Standup launched RC1 with expanded support (notably for iPhone/iOS), migration guidance, and API updates—continuing the focus on adopting best practices through consistent community feedback.

A guide to authoring .NET tools covers multi-targeting, configuration settings, CI integration, manifest use, installation, and handling pre-releases, supporting developers with reliable, cross-version tools.

- [Blazor Internship Demo Fest: New Components and Enhanced Navigation]({{ "/2025-09-04-Blazor-Internship-Demo-Fest-New-Components-and-Enhanced-Navigation.html" | relative_url }})
- [.NET MAUI Community Standup: Release Candidates, iPhone Support, and Updates]({{ "/2025-09-04-NET-MAUI-Community-Standup-Release-Candidates-iPhone-Support-and-Updates.html" | relative_url }})
- [Using and Authoring .NET Tools: Multi-Targeting, CI, and Best Practices](https://andrewlock.net/using-and-authoring-dotnet-tools/)

### Workflow Automation and AI in VS Code With MCP

VS Code’s MCP integration progresses, focusing on native server embedding and automated features for improved developer experience. The featured podcast highlights authentication upgrades, Playwright integration, and security checks.

Kent C. Dodds shares practical strategies for workflow automation and project efficiency, connecting with recent developments in customizable AI-driven processes for VS Code.

- [Building an MCP Inside VS Code and Exploring AI's Impact with Kent C. Dodds]({{ "/2025-09-01-Building-an-MCP-Inside-VS-Code-and-Exploring-AIs-Impact-with-Kent-C-Dodds.html" | relative_url }})

### SharePoint and Microsoft Teams: Customization, Troubleshooting, and Integration

Microsoft 365 development continues to prioritize customization and integration, supporting recent advances in streamlined delivery and automation.

A step-by-step guide for creating Teams apps/tabs explains setup, authentication, deployment, and integration patterns—extending workflow improvement efforts for Teams and SharePoint.

SharePoint troubleshooting advice covers search results, automation, permissions, metadata, and web part strategies for improved content discovery in both classic and modern environments.

- [Creating Custom Microsoft Teams Apps and Tabs](https://dellenny.com/creating-custom-microsoft-teams-apps-and-tabs/)
- [Troubleshooting SharePoint Search Result Issues](https://dellenny.com/what-to-do-when-sharepoint-search-doesnt-return-the-right-results/)

## DevOps

DevOps coverage focuses on platform updates, security, streamlined CI/CD, and new GitHub features for safer and more collaborative code delivery. Highlights include test management upgrades, security guidelines, improved UI navigation, and advanced automation through AI integration.

### GitHub Platform Updates and Security Improvements

GitHub features this week reinforce previous work around permissions and workflow security. The nx supply chain attack review outlines risks in GitHub Actions—such as unsafe triggers, secret access, and AI-generated weak templates—and recommends steps like read-only permissions, restricted secrets, manual PR approvals, pinning to specific Actions SHAs, regular code scanning, automated Dependabot patching, and real-time monitoring to mitigate attacks.

GitHub now enforces resource limits on GraphQL API queries, encouraging optimized usage and security for complex API calls.

Web UI improvements provide streamlined file editing and navigation, making branch transitions and file tree management more user-friendly for daily contributors.

A faster dashboard feed features a refreshed card UI and filterable activity streams, updating platform reliability for all users.

- [Mitigating GitHub Actions Supply Chain Attacks: Lessons from the nx Project Hack](https://jessehouwing.net/github-actions-learnings-from-the-recent-nx-hack/)
- [GitHub GraphQL API Resource Limits Introduced](https://github.blog/changelog/2025-09-01-graphql-api-resource-limits)
- [Improved File Navigation and Editing in the GitHub Web UI](https://github.blog/changelog/2025-09-04-improved-file-navigation-and-editing-in-the-web-ui)
- [GitHub Dashboard Feed Page Updated for Better Performance and Consistency](https://github.blog/changelog/2025-09-04-the-dashboard-feed-page-gets-a-refreshed-faster-experience)

### AI-Powered DevOps Collaboration and Actionable Observability

AI-driven DevOps strategies continue to evolve. Vibe coding integrates team norms and preferences into AI-generated suggestions, enhancing onboarding and release speed while supporting collaborative development.

Observability advances help IT teams prioritize alerts and connect monitoring data to practical business outcomes. AI-enhanced dashboards and incident management tools make it easier to respond to issues and improve customer-facing metrics, as demonstrated in Lenovo and TSB Bank stories.

- [Vibe Coding: Transforming DevOps and CI/CD Teams with AI-Powered Collaboration](https://devops.com/vibing-with-the-future-why-vibe-coding-is-the-next-big-wave-for-devops-and-ci-cd-teams/?utm_source=rss&utm_medium=rss&utm_campaign=vibing-with-the-future-why-vibe-coding-is-the-next-big-wave-for-devops-and-ci-cd-teams)
- [Making Observability Actionable: Turning Metrics, Logs, and Traces into Better Business Outcomes](https://devops.com/making-observability-actionable-turning-metrics-logs-and-traces-into-better-business-outcomes/?utm_source=rss&utm_medium=rss&utm_campaign=making-observability-actionable-turning-metrics-logs-and-traces-into-better-business-outcomes)

### Other DevOps News

Azure Test Plans has released a Test Run Hub in public preview, supporting real-time tracking, better filtering, analytics, and artifact management for manual and automated tests. Features include markdown comments, traceability, and new REST APIs for automation and reporting—advancing recent goals of seamless test management in CI/CD.

- [Introducing the New Test Run Hub in Azure Test Plans](https://devblogs.microsoft.com/devops/new-test-run-hub/)

## Security

Security news this week includes improved Azure audit controls, updated VM security options, workspace protections, and GitHub enhancements for code scanning and campaign notifications. Azure adds new features for Linux auditing, VM upgrades, and Spark workspace network controls, while GitHub expands language support and notification options.

### Azure Security Enhancements for Linux, VMs, and Workspaces

Azure enhanced its Linux auditing with new rule-level, audit-only controls via Azure Policy and OSConfig agent, letting teams monitor hybrid and multi-cloud environments. Dashboards and Defender integration help manage configuration drift with oversight—not auto-remediation—adding to previous quantum-safe and policy-driven security improvements.

Trusted Launch upgrades for Azure VMs and Scale Sets (GA) provide Secure Boot and vTPM, with best practices for phased rollouts and restore point checks to ensure robust upgrades for existing workloads.

Fabric introduces Workspace Outbound Access Protection for Spark (preview), allowing admins to restrict endpoint access with Managed Private Endpoints, supporting privacy and workspace isolation.

- [GA: Enhanced Audit in Azure Security Baseline for Linux](https://techcommunity.microsoft.com/t5/azure-governance-and-management/ga-enhanced-audit-in-azure-security-baseline-for-linux/ba-p/4446170)
- [Upgrade Azure VMs with Trusted Launch: In-Place Security Enhancement Now Available](https://techcommunity.microsoft.com/t5/azure-compute-blog/increase-security-for-azure-vms-trusted-launch-in-place-upgrade/ba-p/4440584)
- [Introducing Workspace Outbound Access Protection for Spark](https://blog.fabric.microsoft.com/en-US/blog/introducing-workspace-outbound-access-protection-for-spark-preview/)

### Securing Kubernetes and Cloud Developer Workflows

Continuing the emphasis on identity and DevSecOps, a guide details how to secure Azure Cloud Shell access to AKS using vNet integration, command restrictions, script allow-listing, and Bastion connectivity.

Secrets Store CSI Driver and Azure Key Vault guides discuss runtime delivery and rotation for cloud-native Kubernetes deployments, maintaining focus on secret management and supply chain safety.

- [Securing Cloud Shell Access to Azure Kubernetes Service (AKS)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/securing-cloud-shell-access-to-aks/ba-p/4450299)
- [Securely Managing Kubernetes Secrets with Secrets Store CSI Driver and Azure Key Vault]({{ "/2025-09-02-Securely-Managing-Kubernetes-Secrets-with-Secrets-Store-CSI-Driver-and-Azure-Key-Vault.html" | relative_url }})

### GitHub Security Ecosystem: Code Scanning and Campaigns

GitHub now auto-enrolls developers with write access for security campaign updates, improving integration between security campaigns and Copilot Autofix while supporting Enterprise Cloud compatibility.

CodeQL 2.22.4 brings multi-language additions, including Go 1.25 support, and upgrades for Rust and Java/Kotlin vulnerability detection—refining supply chain security and reducing false positives.

- [Improved Notifications in GitHub Security Campaigns](https://github.blog/changelog/2025-09-01-improved-notifications-in-security-campaigns)
- [CodeQL 2.22.4 Adds Go 1.25 Support and Security Enhancements](https://github.blog/changelog/2025-09-02-codeql-2-22-4-adds-support-for-go-1-25-and-accuracy-improvements)

### Other Security News

Resources around Entra ID (formerly Azure AD) and Zero Trust share training and implementation guides for secure tenant management and hybrid cloud states. Walkthroughs help organizations apply conditional access, SSO, and Zero Trust principles using Entra, Intune, and Sentinel with NIST-aligned frameworks.

- [Beginners Guide to Entra ID]({{ "/2025-09-01-Beginners-Guide-to-Entra-ID.html" | relative_url }})
- [Zero Trust Workshop: Implementing Microsoft's Security Framework]({{ "/2025-09-03-Zero-Trust-Workshop-Implementing-Microsofts-Security-Framework.html" | relative_url }})
