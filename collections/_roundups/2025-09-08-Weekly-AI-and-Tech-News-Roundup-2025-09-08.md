---
title: Latest Updates in AI Development, Cloud Automation, and Enterprise Security
author: TechHub
date: 2025-09-08 09:00:00 +00:00
tags:
- .NET
- AI Agents
- Azure AI
- Blazor
- CI/CD
- Cloud Infrastructure
- Code Review
- Compliance
- Data Analytics
- Developer Tooling
- Enterprise Security
- Identity Management
- Kubernetes
- MCP
- Microsoft Fabric
- OpenAI
- VS Code
- Workflow Automation
- AI
- GitHub Copilot
- ML
- Azure
- DevOps
- Security
- Roundups
- Machine Learning
section_names:
- ai
- github-copilot
- ml
- azure
- dotnet
- devops
- security
primary_section: github-copilot
feed_name: TechHub
external_url: /all/roundups/Weekly-AI-and-Tech-News-Roundup-2025-09-08
---
Welcome to this week's tech roundup. We're focusing on practical developments happening at the intersection of AI, cloud, and enterprise automation. GitHub Copilot advances toward a broader role in development, with new features like autonomous agents, integrated web-based workflows, enhanced customization, and more detailed enterprise license administration. Meanwhile, Azure AI Foundry and Copilot Studio introduce tools for AI model management and agent-to-agent automations, making business processes and workflow design more accessible.

Azure’s cloud platform continues to grow, with Microsoft Fabric delivering better data integration and governance, App Service offering new hosting and quota management options, and updated security certifications and compliance resources for cloud-native workloads. The .NET and VS Code communities gain hands-on guidance and automation updates to support both desktop and cross-platform work. DevOps teams benefit from improved GitHub Actions, more actionable monitoring, and new testing tools, while security remains essential with updated guides on code scanning, CI/CD protections, and managing secrets. Below, you'll find a detailed look at how these updates can help your teams be more productive, reliable, and secure.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Copilot Agents and MCP: Expanding Context and Automation](#copilot-agents-and-mcp-expanding-context-and-automation)
  - [Copilot Coding Agent in Practice](#copilot-coding-agent-in-practice)
  - [Copilot in IDEs: Visual Studio and Eclipse Enhancements](#copilot-in-ides-visual-studio-and-eclipse-enhancements)
  - [Code Review and Customization: Instructions at Scale](#code-review-and-customization-instructions-at-scale)
  - [Enterprise Teams and Business License Management](#enterprise-teams-and-business-license-management)
  - [Prompt and Spec-Driven Workflows](#prompt-and-spec-driven-workflows)
  - [Copilot-Assisted Migration and Legacy Modernization](#copilot-assisted-migration-and-legacy-modernization)
  - [Developer Education and Onboarding](#developer-education-and-onboarding)
- [AI](#ai)
  - [Azure AI Foundry and Agentic Workflows](#azure-ai-foundry-and-agentic-workflows)
  - [Microsoft Copilot Studio: Workflow Automation and Generative Logic](#microsoft-copilot-studio-workflow-automation-and-generative-logic)
  - [Azure AI OpenAI, Model Context Protocol, and Streamlit Deployments](#azure-ai-openai-model-context-protocol-and-streamlit-deployments)
  - [AI Agent Design: Context Engineering and Developer Education](#ai-agent-design-context-engineering-and-developer-education)
  - [The Reality of AI-Augmented Coding](#the-reality-of-ai-augmented-coding)
  - [Applied AI: Accessibility and Edge Workflows](#applied-ai-accessibility-and-edge-workflows)
- [Azure](#azure)
  - [Microsoft Fabric: Analytics, Eventstreams, and Data Integration](#microsoft-fabric-analytics-eventstreams-and-data-integration)
  - [Azure App Service: New Premium Hosting, Self-Service Scaling, and Platform Extensibility](#azure-app-service-new-premium-hosting-self-service-scaling-and-platform-extensibility)
  - [Infrastructure Automation, Security, and Management](#infrastructure-automation-security-and-management)
  - [Azure Logic Apps and Integration Services](#azure-logic-apps-and-integration-services)
  - [Azure VMware Solution, Service Discovery, and Migration Guidance](#azure-vmware-solution-service-discovery-and-migration-guidance)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET Ecosystem: Tooling, Web, and Cross-Platform Updates](#net-ecosystem-tooling-web-and-cross-platform-updates)
  - [VS Code and MCP: Workflow Automation and AI Integration](#vs-code-and-mcp-workflow-automation-and-ai-integration)
- [DevOps](#devops)
  - [GitHub Ecosystem Updates](#github-ecosystem-updates)
  - [AI and Observability in DevOps](#ai-and-observability-in-devops)
  - [Azure DevOps and Quality Management Enhancements](#azure-devops-and-quality-management-enhancements)
- [Security](#security)
  - [Kubernetes and Azure Cloud Security Solutions](#kubernetes-and-azure-cloud-security-solutions)
  - [CI/CD and Developer Workflow Security](#cicd-and-developer-workflow-security)
  - [Compliance, Audit, and Identity Infrastructure](#compliance-audit-and-identity-infrastructure)
  - [Code Scanning and Data Exfiltration Security in Developer Platforms](#code-scanning-and-data-exfiltration-security-in-developer-platforms)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot introduced updates across its platform, further integrating AI into everyday development activities. These enhancements include better support in IDEs, new ways to manage prompts, autonomous agents, and streamlined enterprise administration. Copilot is evolving toward a multi-modal assistant that supports coding, collaboration, and codebase insights. Notable updates involve the Copilot Agents Panel, the general availability of the remote MCP Server, new agent-driven workflows, and greater adoption of Copilot for team collaboration and modernizing legacy code.

### Copilot Agents and MCP: Expanding Context and Automation

Building on recent improvements in MCP integration and agent workflows, Copilot now features its Agents Panel within the GitHub web interface. This centralizes development and review tasks, reducing context switching and enabling context-aware workflows that were previously available only in desktop environments.

The remote GitHub MCP Server has moved to general availability, featuring standardized OAuth 2.1 with PKCE for secure authentication across IDEs and browsers. Copilot continues to strengthen security with secret scanning and built-in code scanning alerts, further reducing operational risks by extending last week's added security features.

Centralized automation, robust authentication, and policy-based team collaboration enhance resource management, following the ongoing move toward scalable coding and agent workflows.

- [A First Look at the New Copilot Agents Panel on GitHub](/ai/videos/a-first-look-at-the-new-copilot-agents-panel-on-github)
- [Remote GitHub MCP Server Now Generally Available](https://github.blog/changelog/2025-09-04-remote-github-mcp-server-is-now-generally-available)
- [How to Debug a Web App Using Playwright MCP and GitHub Copilot](https://github.blog/ai-and-ml/github-copilot/how-to-debug-a-web-app-with-playwright-mcp-and-github-copilot/)
- [Building Smarter AI Tool Interactions with MCP Elicitation](https://github.blog/ai-and-ml/github-copilot/building-smarter-interactions-with-mcp-elicitation-from-clunky-tool-calls-to-seamless-user-experiences/)

### Copilot Coding Agent in Practice

The features in Copilot Coding Agent build upon previous automation resources, extending agent-driven workflows for .NET automation and backlog management. This enhances Copilot’s usefulness on both desktop and cloud platforms.

New documentation covers tasks like identifying gaps in unit test coverage, automating pull requests, and reviewing agent logs—improving on earlier approaches to team collaboration and sub-task management. Integrating the Playwright MCP Server further broadens the agent's use in debugging and extensibility.

By adopting agent-driven processes, teams can minimize repetitive tasks, address legacy code, and coordinate remote updates—mirroring recent trends in robust automation.

- [Automating .NET Development with GitHub Copilot Coding Agent](https://devblogs.microsoft.com/dotnet/copilot-coding-agent-dotnet/)
- [What's New with the GitHub Copilot Coding Agent](/ai/videos/whats-new-with-the-github-copilot-coding-agent)
- [From Issue to PR: Asynchronously Develop with GitHub Copilot Coding Agent](/ai/videos/from-issue-to-pr-asynchronously-develop-with-github-copilot-coding-agent)

### Copilot in IDEs: Visual Studio and Eclipse Enhancements

Recent improvements in prompt handling and model selection in Visual Studio and JetBrains IDEs continue with updates to Visual Studio 17.14 and Eclipse. The new Output Window integration allows developers to query and understand logs directly. Reusable prompt files make prompt management more efficient for teams.

Eclipse has added support for custom instructions, enhanced APIs, and image context, strengthening its multi-model backend and agent scripting. These updates broaden Copilot’s compatibility across various IDEs and plugin systems.

- [Make Sense of Your Output Window with Copilot in Visual Studio](https://devblogs.microsoft.com/visualstudio/make-sense-of-your-output-window-with-copilot/)
- [Boost Your Copilot Collaboration with Reusable Prompt Files](https://devblogs.microsoft.com/visualstudio/boost-your-copilot-collaboration-with-reusable-prompt-files/)
- [Turning GitHub Copilot Prompts into Executable Files in VS Code](/ai/videos/turning-github-copilot-prompts-into-executable-files-in-vs-code)
- [New Features in GitHub Copilot for Eclipse Empower Developer Experience](https://github.blog/changelog/2025-09-05-new-features-in-github-copilot-in-eclipse)

### Code Review and Customization: Instructions at Scale

Copilot now allows path-scoped instruction files in code reviews, providing more targeted feedback for larger codebases. This shift from ad-hoc to modular settings builds on previous support for project and organization-level customization.

Guides recommend offering detailed instructions about project context, technology stack, and coding conventions, reinforcing the trend toward thorough, actionable reviews.

- [Path-Scoped Custom Instructions for Copilot Code Review](https://github.blog/changelog/2025-09-03-copilot-code-review-path-scoped-custom-instruction-file-support)
- [5 Tips for Crafting Better Custom Instructions for GitHub Copilot](https://github.blog/ai-and-ml/github-copilot/5-tips-for-writing-better-custom-instructions-for-copilot/)

### Enterprise Teams and Business License Management

Copilot's Enterprise Teams feature on GitHub Enterprise Cloud (now in public preview) builds on prior advances in data residency and licensing. It provides more detailed access controls, automated license assignment, and onboarding improvements, all reflecting the evolution of enterprise identity and workflow management.

These updates help organizations allocate licenses, control permissions, and track Copilot use—supporting earlier improvements in preparing for enterprise adoption.

- [Managing GitHub Copilot Business Licenses with Enterprise Teams (Public Preview)](https://github.blog/changelog/2025-09-04-manage-copilot-and-users-via-enterprise-teams-in-public-preview)

### Prompt and Spec-Driven Workflows

The Spec Kit enables a more structured approach to development, shifting from prompt-driven to spec-driven workflows. This continues last week's focus on Spec Kit and automation. Spec Kit organizes projects by specification and modular tasks, building on past structured coding guidance.

CLI and IDE integration keep the focus on reliable, validated development and support translating requirements into reliable code for new and legacy projects.

- [Adopting Spec-Driven Development with AI: Introducing Spec Kit](https://github.blog/ai-and-ml/generative-ai/spec-driven-development-with-ai-get-started-with-a-new-open-source-toolkit/)

### Copilot-Assisted Migration and Legacy Modernization

Copilot is being used more often for migrating legacy and enterprise systems, with new guides covering reverse engineering, generating documentation, and automated testing. These resources help teams approach incremental migrations safely and clarify Copilot’s value for large codebase transitions.

Copilot’s suggestions support planning and executing modernization projects, in line with earlier updates.

- [Modernizing Legacy COBOL to Cloud with GitHub Copilot](/ai/videos/modernizing-legacy-cobol-to-cloud-with-github-copilot)
- [How to Migrate Legacy Applications Using GitHub Copilot](https://dellenny.com/how-to-migrate-legacy-applications-using-github-copilot/)

### Developer Education and Onboarding

This week’s focus on education builds on existing learning resources, offering clear "top 10" Copilot workflows and guidance on requirement gathering and prompt writing. The goal is to provide structured, actionable onboarding for developers.

Additional topics such as RegEx validation and conducting Agent Mode code reviews support the growing role of Copilot for automation and efficient onboarding.

- [Top 10 Things You Can Do with GitHub Copilot as a New Developer](https://dellenny.com/top-10-things-you-can-do-with-github-copilot-as-a-new-developer-2/)
- [Using GitHub Copilot Chat to Write Better Regex Patterns](https://cooknwithcopilot.com/blog/draft-smarter-regex-without-the-headaches.html)
- [How to Get the Most Out of Your AI with Vibe Coding](/ai/videos/how-to-get-the-most-out-of-your-ai-with-vibe-coding)

## AI

The AI section this week includes updates in Azure AI Foundry, enterprise automation with Copilot Studio, new tutorials, deployment security guides, and reflections on the challenges of AI coding tools. Azure AI Foundry released additional multimodal models and orchestration features, while Copilot Studio further enabled agent-to-agent automation for business use. Tutorials addressed context management and secure deployment, continuing to stress cost and accessibility.

### Azure AI Foundry and Agentic Workflows

Azure AI Foundry remains a mainstay for enterprise AI agent development. Its August 2025 release brings in GPT-5, extended context windows, advanced multimodality, and orchestration utilities. The Model Router helps pick the best GPT-5 version, the Sora API increases image-to-video features, and Mistral Document AI brings improved document layout recognition. The update also includes better SDKs, Terraform integration, and OpenTelemetry support for observability. Browser Automation preview supports RPA scenarios, combining natural language control with Playwright. The Agent Service is now available in 17 regions, with revamped onboarding and recovery documentation.

Building on previous coverage of multi-agent orchestration and RAG workflows, Foundry now moves several features to general availability. Open standards such as MCP/A2A facilitate migration and interoperability. Technical guides show how Foundry integrates with developer tools, allowing fast transitions from prototyping to production. Tutorials guide developers through setting up persistent-memory agents, orchestrating multi-agent scenarios, and ramping up quickly, while Q&A materials share strategies for robust design and troubleshooting.

- [What’s New in Azure AI Foundry: August 2025 Release Highlights](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-august-2025/)
- [Agent Factory: From Prototype to Production—Developer Tools and Rapid Agent Development](https://azure.microsoft.com/en-us/blog/agent-factory-from-prototype-to-production-developer-tools-and-rapid-agent-development/)
- [Build a Smart Shopping AI Agent with Memory Using Azure AI Foundry Agent Service](https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-a-smart-shopping-ai-agent-with-memory-using-the-azure-ai/ba-p/4450348)
- [Build Multi-Agent AI Systems on Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-multi-agent-ai-systems-on-azure-app-service/ba-p/4451373)
- [Context Window: 3 Azure AI Foundry Community Questions Answered](/ai/videos/context-window-3-azure-ai-foundry-community-questions-answered)

### Microsoft Copilot Studio: Workflow Automation and Generative Logic

Copilot Studio now supports agent-to-agent collaboration for modular HR and IT onboarding. Developers can build workflows where multiple agents manage different segments, coordinated using custom Canvas apps. New preview features support maintainable and extendable automation.

Building on enterprise automation and orchestration topics from last week, Copilot Studio now securely connects business logic with generative AI. Its architecture separates intent processing from compliance enforcement by using plugins, role-based access, and data loss prevention. Walkthroughs include CRM, ERP, and retail use cases, illustrating practical automation and strategies for scaling.

- [Agent-to-Agent Collaboration in Copilot Studio](/ai/videos/agent-to-agent-collaboration-in-copilot-studio)
- [Combining Generative AI and Business Logic with Copilot Studio](https://dellenny.com/combining-generative-ai-and-business-logic-with-copilot-studio/)
- [Automating Retail Customer Service with Copilot Studio](https://dellenny.com/how-retail-businesses-are-automating-customer-service-with-copilot-studio/)

### Azure AI OpenAI, Model Context Protocol, and Streamlit Deployments

This week’s tutorials describe deploying Azure OpenAI models securely using the Model Context Protocol (MCP), emphasizing transparent context management. Developers are shown how to build an image captioning system: users upload images via Streamlit, Azure AI Vision creates tags, and GPT-4o-mini writes captions. The workflow is hosted on Azure App Service and employs managed identities, azd, and Bicep.

These samples follow last week's themes of MCP-based communication, persistent context, and serverless agent workflows. The current guides add hands-on code and step-by-step onboarding instructions.

A separate resource demonstrates deploying a Microsoft Docs AI assistant using RAG pipelines, MCP, Azure Container Apps, and OpenAI. The article covers containerization, environment configuration, and scaling—preparing teams for onboarding and future AI customizations, with a focus on modularity and security.

- [Build an AI Image-Caption Generator on Azure App Service with Streamlit and GPT-4o-mini](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-an-ai-image-caption-generator-on-azure-app-service-with/ba-p/4450313)
- [Deploying a Self-Hosted Microsoft Docs AI Assistant with Azure OpenAI and MCP](https://devblogs.microsoft.com/all-things-azure/build-your-own-microsoft-docs-ai-assistant-with-azure-container-apps-and-azure-openai/)

### AI Agent Design: Context Engineering and Developer Education

Several resources cover context engineering for AI agents, offering practical advice for finding and using relevant data—improving adaptability and reliability. The content features code samples, scaling tips, and fallback planning for building more robust agents.

Tying in with previous stories on GraphRAG and best practices, these materials highlight developer education. Tutorials, livestreams, and community events offer a variety of ways to apply concepts like MCP and agent lifecycle management.

An October livestream series (in Spanish) guides Python developers through generative AI, agent architectures, MCP workflows, and demos. Sessions include Q&A and access to a Discord community.

- [Context Engineering for AI Agents](/ai/videos/context-engineering-for-ai-agents)
- [Level Up Your Python Game with Generative AI: Free Livestream Series](https://techcommunity.microsoft.com/t5/microsoft-developer-community/level-up-your-python-game-with-generative-ai-free-livestream/ba-p/4450646)

### The Reality of AI-Augmented Coding

Companies find that LLM-based coding tools bring benefits but also introduce new costs, review needs, and security issues. Sonar’s report on ChatGPT-5 shows improvements in reasoning and code quality, but notes higher subscription costs and codebase demands. While some vulnerabilities decrease, concurrency issues become more common, highlighting the importance of thorough QA.

Following previous themes on cost and risk management, this section discusses budget implications of generative coding and the importance of active monitoring and quality assurance.

The concept of "vibe coding" (using LLMs for intent-driven development) is discussed with focus on enterprise risk. While productivity gains are possible, rigorous oversight, checks, and compliance are required to avoid leaks or errors in quickly built code.

- [Report: ChatGPT-5 Coding Gains Come at a Higher Cost](https://devops.com/report-chatgpt-5-coding-gains-come-at-a-higher-cost/?utm_source=rss&utm_medium=rss&utm_campaign=report-chatgpt-5-coding-gains-come-at-a-higher-cost)
- [Can Vibe Coding Work on an Enterprise Level?](https://devops.com/can-vibe-codingwork-on-an-enterprise-level/?utm_source=rss&utm_medium=rss&utm_campaign=can-vibe-codingwork-on-an-enterprise-level)

### Applied AI: Accessibility and Edge Workflows

The Argus Panoptes project, a Microsoft Imagine Cup winner, demonstrates how Azure AI and Wi-R wireless protocols are enabling modern accessibility devices. The system balances workloads between local devices and Azure Foundry, delivering reliable object recognition and leveraging Azure AI Speech for voice interaction.

This example continues last week's focus on mainstream and startup innovation in AI infrastructure. It shows how Azure supports both enterprise uses and strategy for accessible technology.

Azure also fosters startup projects and innovations with an emphasis on accessibility.

- [Argus and Azure AI: Inclusive Assistive Tech Triumphs at Imagine Cup](https://www.microsoft.com/en-us/startups/blog/could-your-startup-be-the-next-imagine-cup-world-champion/)

## Azure

Azure’s platform continues to evolve, with updates that enhance developer workflows, analytics, automation, security, and integration. Improvements range across analytics engines, application hosting, platform extensibility, infrastructure automation, and developer tools—helping teams with hybrid, cross-cloud, and modernization scenarios.

### Microsoft Fabric: Analytics, Eventstreams, and Data Integration

Microsoft Fabric made several analytics improvements. OneLake now provides unified storage with virtualization, making governance and compliance less complex. Shortcuts to external data, live database mirroring, and on-the-fly data transformations all reduce the complexity of traditional ETL. Azure Purview integration simplifies compliance, and new support for open formats prepares the platform for future advances. Live data can now be used directly in AI chat applications. Customer stories from Lumen and IFS highlight these improvements.

Detailed updates in the Fabric Warehouse August 2025 recap include JSON Lines support, better job management, and new AzCopy migration scenarios. CI/CD and automation are getting easier, and multi-scheduler notebooks drive advanced analytics.

Security features expand with new workspace-level Private Link support (preview), enhanced migration controls, and more admin history. Copilot’s schema suggestions and new orchestration capabilities embed AI into data processes. Documentation and best practice materials support these changes.

The Schema Registry (preview) for Fabric Eventstreams enforces Avro-based contracts, strengthens type safety, and bolsters event ingestion. Versioning and diagnostics offer better governance, setting the stage for EventHub integration and real-time data mesh architectures—building on previous announcements about event processing.

- [OneLake: your foundation for an AI-ready data estate](https://blog.fabric.microsoft.com/en-US/blog/onelake-your-foundation-for-an-ai-ready-data-estate/)
- [What’s New in Fabric Warehouse: August 2025 Recap](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-warehouse-august-2025/)
- [Introducing Schema Registry for Type-Safe Pipelines in Microsoft Fabric Eventstreams](https://blog.fabric.microsoft.com/en-US/blog/schema-registry-creating-type-safe-pipelines-using-schemas-and-eventstreams-preview/)
- [Accelerating Data Ingestion from SQL to Fabric with Fast Copy in Dataflow](https://blog.fabric.microsoft.com/en-US/blog/accelerating-data-movement-by-using-fast-copy-to-unlock-performance-and-efficiency-during-data-ingestion-from-sql-database-in-fabric/)
- [SQL Database in Fabric: What's New and Improved (Data Exposed Public Preview)](/azure/videos/sql-database-in-fabric-whats-new-and-improved-data-exposed-public-preview)

### Azure App Service: New Premium Hosting, Self-Service Scaling, and Platform Extensibility

Azure App Service Premium v4 is generally available, featuring AMD Dadsv6/Eadsv6 hardware and NVMe storage for up to 58% more throughput. This upgrade brings added scaling options, monitoring tools, and improved multi-tenancy support for both Windows and Linux environments. Developers now have easier configuration, redundancy, and pricing.

A public preview of self-service quota management lets teams view and adjust most usage limits from the Azure portal, with visualizations and region/SKU filtering. Automation and logging enhancements are on the way.

Teams Phone Extensibility (TPE) and Azure Communication Services are now generally available, making it easier to add custom voice and telephony solutions within Teams—including AI voice applications. These features simplify compliance, monitoring, and integration for organizations and independent software vendors.

These improvements build on last week’s developments in application hosting, hybrid configurations, and a smoother onboarding process for cloud resources.

- [General Availability of Premium v4 for Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-of-premium-v4-for-azure-app/ba-p/4446204)
- [Public Preview: Self-Service Quota Management for Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-the-public-preview-of-the-new-app-service-quota-self/ba-p/4450415)
- [General Availability of Teams Phone Extensibility with Azure Communication Services](https://techcommunity.microsoft.com/t5/azure-communication-services/general-availability-of-teams-phone-extensibility/ba-p/4446092)

### Infrastructure Automation, Security, and Management

Azure now offers in-place Trusted Launch upgrades for VMs and scale sets in general availability, supporting Secure Boot, virtual TPM, and boot integrity without redeployment. These features align with benchmarks such as Azure Security Benchmark and FedRAMP. Guidance is available for testing, rolling back, and auditing deployments.

The Azure SRE Agent continues to move toward general availability, with permission management, Azure CLI/Kubernetes integration, and developer feedback processes. Updates on billing and licensing keep the focus on resource optimization and platform reliability.

Automation updates are designed to help teams diagnose and recover from incidents more quickly.

- [Upgrade Azure VMs with Trusted Launch: In-Place Security Enhancement Now Available](https://techcommunity.microsoft.com/t5/azure-compute-blog/increase-security-for-azure-vms-trusted-launch-in-place-upgrade/ba-p/4440584)
- [Enterprise-Ready and Extensible: Update on the Azure SRE Agent Preview](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/enterprise-ready-and-extensible-update-on-the-azure-sre-agent/ba-p/4444299)

### Azure Logic Apps and Integration Services

The Logic Apps Aviators Newsletter for September 2025 highlights the latest improvements. The Data Mapper is now generally available, with a new VS Code extension and CI/CD integration in Deployment Center. Logic Apps also support hybrid deployment on Rancher K3s—including on-premises and edge hosting. Community resources offer guidance on DevOps integration, SAP migration from BizTalk, and Azure OpenAI integration with adaptive automation.

These updates continue recent advancements in scalable automation, hybrid cloud integration, and developer tooling for cloud-connected business processes.

- [Logic Apps Aviators Newsletter – September 25, 2025](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-aviators-newsletter-september-25/ba-p/4450195)
- [Azure Update - 5th September 2025](/azure/videos/azure-update-5th-september-2025)

### Azure VMware Solution, Service Discovery, and Migration Guidance

Broadcom’s new licensing policy for Azure VMware Solution requires direct BYOL for VMware Cloud Foundation deployments after November 2025. Workflows do not change, but teams should update their procurement and deployment strategies.

Last week’s region, storage, and compliance updates for AVS provide additional context about these changes and the complexity of VMware workload management.

A new guide describes service discovery in Azure, covering AKS, App Service, Service Fabric, and Container Apps. Developers learn about dynamic API-based service resolution, managed identities, and service mesh patterns.

Another resource walks teams through migrating from AWS Application Load Balancer to Azure Application Gateway using infrastructure-as-code, SSL/TLS, and detailed validation—minimizing downtime and complementing earlier cloud networking and capacity planning guides.

- [Broadcom VMware Licensing Changes Impacting Azure VMware Solution Customers](https://techcommunity.microsoft.com/t5/azure-migration-and/broadcom-vmware-licensing-changes-what-azure-vmware-solution/ba-p/4448784)
- [Service Discovery in Azure: Dynamically Finding Service Instances](https://dellenny.com/service-discovery-in-azure-dynamically-finding-service-instances/)
- [Migrating from AWS Application Load Balancer to Azure Application Gateway](https://techcommunity.microsoft.com/t5/azure-migration-and/migrating-application-load-balancer-from-aws-to-azure/ba-p/4439880)

### Other Azure News

The new Azure Local Training Program provides self-paced resources for Azure Local, covering deployment, Arc integration, monitoring, and billing for users of all experience levels—building on last week’s focus on Azure learning.

Guides for creating custom Microsoft Teams apps and tabs cover Power Platform integration and Node.js/React/Azure AD-based solutions.

New SharePoint documentation addresses troubleshooting search issues and managing sprawl while encouraging effective collaboration, echoing last week's operational guidance.

Microsoft Cost Management updates include role improvements, pricing calculator updates, log filtering, and better support for storage migrations, continuing efforts to improve cost transparency and management.

- [Azure Local Training Program: Self-Paced Cloud Management Skills](https://techcommunity.microsoft.com/t5/azure-architecture-blog/introducing-azure-local-training-empowering-you-to-succeed/ba-p/4447957)

- [Creating Custom Microsoft Teams Apps and Tabs](https://dellenny.com/creating-custom-microsoft-teams-apps-and-tabs/)

- [Troubleshooting SharePoint Search Result Issues](https://dellenny.com/what-to-do-when-sharepoint-search-doesnt-return-the-right-results/)
- [Avoiding SharePoint Sprawl Without Killing Collaboration](https://dellenny.com/avoiding-sharepoint-sprawl-without-killing-collaboration/)

- [Microsoft Cost Management Updates: July & August 2025](https://azure.microsoft.com/en-us/blog/microsoft-cost-management-updates-july-august-2025/)

## Coding

This week features guides and platform enhancements in .NET, Blazor, .NET MAUI, and VS Code. Developers have new resources for building .NET CLI tools, web and mobile apps, and more secure and automated workflows. Further MCP and AI improvements in VS Code support efficient, streamlined daily development.

### .NET Ecosystem: Tooling, Web, and Cross-Platform Updates

Continuing last week’s exploration of .NET tooling, Andrew Lock’s article covers how to create robust .NET tools, including tips on global vs. local installations, repeatable manifests, multi-targeting for compatibility, RollForward for stable versioning, and integration with CI/test automation. Developers will learn how to set up tools efficiently, manage versions, and avoid common CI issues.

The Blazor Internship Demo Fest highlights a new media component and improved navigation state, simplifying multimedia usage and app navigation in Blazor. These insights reinforce best practices in UI design and community engagement.

.NET MAUI’s Community Standup introduces Release Candidate 1, improved iPhone support, and new cross-platform features, with practical recommendations for mobile app delivery. Q&A and hands-on content help developers adapt to changing mobile targets—building on prior updates to release cycles and architectural patterns.

- [Using and Authoring .NET Tools: Multi-Targeting, CI, and Best Practices](https://andrewlock.net/using-and-authoring-dotnet-tools/)
- [Blazor Internship Demo Fest: New Components and Enhanced Navigation](/coding/videos/blazor-internship-demo-fest-new-components-and-enhanced-navigation)
- [.NET MAUI Community Standup: Release Candidates, iPhone Support, and Updates](/coding/videos/net-maui-community-standup-release-candidates-iphone-support-and-updates)

### VS Code and MCP: Workflow Automation and AI Integration

Reflecting last week’s work with MCP and Playwright, VS Code now has native MCP server support, allowing for secure authentication and automation of end-to-end tests. Developers are able to run protocol automation directly from their IDE alongside traditional code and CI activities. The integration with Playwright and GitHub MCP further enhances real-world automation.

Kent C. Dodds explains how automation, including authentication to AI coding support, fits into everyday development—demonstrating the shift from initial protocol configuration to stable production usage.

- [Building an MCP Inside VS Code and Exploring AI's Impact with Kent C. Dodds](/ai/videos/building-an-mcp-inside-vs-code-and-exploring-ais-impact-with-kent-c-dodds)

## DevOps

This week’s DevOps news focuses on workflow automation, more actionable observability, and team collaboration in GitHub, Azure DevOps, and AI tooling. The theme is to help teams streamline productivity and adapt efficiently with smarter interfaces and continuous improvements in testing and code quality.

### GitHub Ecosystem Updates

GitHub’s recent releases continue the push for better workflow management. Two AI-powered GitHub Actions—AI Labeler and Moderator—extend automation by using the Models inference API to assist with issue classification and moderation. Maintainers can automate these steps directly in Actions workflows.

The GraphQL API adds new resource limits to streamline performance and reduce deep nesting in queries. Developers are encouraged to check and update their queries for efficiency.

Improved file navigation in GitHub’s web interface now includes editing files from search, clearer branch context, and onboarding improvements for new contributors. These changes support the goal of simplifying navigation and boosting workflow clarity.

GitHub Spark updates add enhanced sharing, smoother Codespaces integration, and an updated activity page—combining to provide a faster, more consistent collaboration experience.

- [New GitHub Actions for AI-Based Issue Labeling and Moderation](https://github.blog/changelog/2025-09-05-github-actions-ai-labeler-and-moderator-with-the-github-models-inference-api)
- [GitHub GraphQL API Resource Limits Introduced](https://github.blog/changelog/2025-09-01-graphql-api-resource-limits)
- [Improved File Navigation and Editing in the GitHub Web UI](https://github.blog/changelog/2025-09-04-improved-file-navigation-and-editing-in-the-web-ui)
- [New Organization Sharing and Local Development Improvements in GitHub Spark](https://github.blog/changelog/2025-09-05-new-spark-sharing-option-and-improved-local-dev-experience)
- [GitHub Dashboard Feed Page Updated for Better Performance and Consistency](https://github.blog/changelog/2025-09-04-the-dashboard-feed-page-gets-a-refreshed-faster-experience)

### AI and Observability in DevOps

AI is becoming more central in DevOps, expanding on recent automation and agent management coverage. The idea of "vibe coding" is reframed as using generative AI to assist with CI/CD, support coding standards, and manage technical debt.

Observability is also becoming more actionable. There is a shift toward reporting metrics and logs that tie directly to business outcomes and faster incident response—echoing earlier discussions on reliability and transparency.

- [Vibe Coding: Transforming DevOps and CI/CD Teams with AI-Powered Collaboration](https://devops.com/vibing-with-the-future-why-vibe-coding-is-the-next-big-wave-for-devops-and-ci-cd-teams/?utm_source=rss&utm_medium=rss&utm_campaign=vibing-with-the-future-why-vibe-coding-is-the-next-big-wave-for-devops-and-ci-cd-teams)
- [Making Observability Actionable: Turning Metrics, Logs, and Traces into Better Business Outcomes](https://devops.com/making-observability-actionable-turning-metrics-logs-and-traces-into-better-business-outcomes/?utm_source=rss&utm_medium=rss&utm_campaign=making-observability-actionable-turning-metrics-logs-and-traces-into-better-business-outcomes)

### Azure DevOps and Quality Management Enhancements

Azure Test Plans now features the new Test Run Hub (public preview), giving teams improved management of quality processes. This includes advanced analytics, filtering, API integrations for report automation, markdown-based commenting, and work item linking.

Combining manual and automated testing in one place, these updates help teams consistently improve software quality and respond more effectively—supporting broader DevOps and engineering goals.

- [Introducing the New Test Run Hub in Azure Test Plans](https://devblogs.microsoft.com/devops/new-test-run-hub/)

## Security

Recent security news features improvements in cloud-native secrets management, pipeline controls, compliance support, and code scanning. The main focus is automating secure workflows, improving visibility, and offering developers practical tips for maintaining security standards.

### Kubernetes and Azure Cloud Security Solutions

Azure’s infrastructure security continues to get new features and certifications. The Secrets Store CSI Driver integrates with Azure Key Vault for external secrets management, supplementing earlier efforts in credential security. Best practices for securing Cloud Shell access to AKS include recommended IP allowlisting and Bastion use.

Azure Linux 3.0 has received Level 1 CIS Benchmark certification for AKS node pools, supporting baseline compliance and easier audits, picking up on recent OS security improvements.

- [Securely Managing Kubernetes Secrets with Secrets Store CSI Driver and Azure Key Vault](/azure/videos/securely-managing-kubernetes-secrets-with-secrets-store-csi-driver-and-azure-key-vault)
- [Securing Cloud Shell Access to Azure Kubernetes Service (AKS)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/securing-cloud-shell-access-to-aks/ba-p/4450299)
- [Azure Linux 3.0 Achieves Level 1 CIS Benchmark Certification](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-3-0-achieves-level-1-cis-benchmark-certification/ba-p/4450410)

### CI/CD and Developer Workflow Security

In response to supply chain threats such as recent nx/npm attacks, a technical post discusses CI/CD defense—beyond secret scanning—covering how to restrict permissions, enforce dependency policies, and control job execution in GitHub Actions.

Another resource covers how to use BitLocker and Hyper-V on developer laptops without repeated recovery prompts, addressing recent content on runtime protection and ransomware defense.

GitHub has improved notifications on security campaigns to help teams respond to vulnerabilities, continuing efforts to automate fixes and reduce alert fatigue as seen in Copilot Autofix and campaign tools.

- [Mitigating GitHub Actions Supply Chain Attacks: Lessons from the nx Project Hack](https://jessehouwing.net/github-actions-learnings-from-the-recent-nx-hack/)
- [How to Use Hyper-V with BitLocker Without Constant Recovery Prompts](https://dellenny.com/how-to-use-hyper-v-with-bitlocker-without-constant-recovery-prompts/)
- [Improved Notifications in GitHub Security Campaigns](https://github.blog/changelog/2025-09-01-improved-notifications-in-security-campaigns)

### Compliance, Audit, and Identity Infrastructure

Enhanced Audit is now generally available for Azure Security Baseline for Linux, allowing organizations to carry out ongoing compliance assessments—continuing the move to automated auditing and policy-based security.

Azure Resource Manager will require multifactor authentication beginning October 2025, except for service principals and managed identities used in automated deployments. This change reflects last week's emphasis on strong authentication.

A beginner’s guide to Entra ID introduces identity management as a security foundation, while technical workshops offer step-by-step advice for implementing Zero Trust with Microsoft’s framework, supporting compliance and regulatory adoption.

- [GA: Enhanced Audit in Azure Security Baseline for Linux](https://techcommunity.microsoft.com/t5/azure-governance-and-management/ga-enhanced-audit-in-azure-security-baseline-for-linux/ba-p/4446170)
- [Azure Mandatory Multifactor Authentication: Phase 2 Launches October 2025](https://azure.microsoft.com/en-us/blog/azure-mandatory-multifactor-authentication-phase-2-starting-in-october-2025/)
- [Beginners Guide to Entra ID](/security/videos/beginners-guide-to-entra-id)
- [Zero Trust Workshop: Implementing Microsoft's Security Framework](/azure/videos/zero-trust-workshop-implementing-microsofts-security-framework)

### Code Scanning and Data Exfiltration Security in Developer Platforms

The latest CodeQL 2.22.4 increases support for Go, Rust, and Java/Kotlin and advances secure-by-default code scanning, secret scanning, and asset validation.

Microsoft Fabric now offers Workspace Outbound Access Protection (OAP) for Spark workspaces to prevent data exfiltration, improving on managed private endpoint protections and supporting ongoing work on data security.

- [CodeQL 2.22.4 Adds Go 1.25 Support and Security Enhancements](https://github.blog/changelog/2025-09-02-codeql-2-22-4-adds-support-for-go-1-25-and-accuracy-improvements)
- [Introducing Workspace Outbound Access Protection for Spark](https://blog.fabric.microsoft.com/en-US/blog/introducing-workspace-outbound-access-protection-for-spark-preview/)

### Other Security News

A newly published guide helps teams understand threat modeling for application security, offering checklists and actionable methods for every level of project maturity. This complements previous discussions about vulnerability management and secure development.

- [Understanding Threat Modeling for Application Security](/security/videos/understanding-threat-modeling-for-application-security)
