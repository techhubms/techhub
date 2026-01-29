---
title: Updated AI DevOps, Copilot Personalization, and Secure Cloud Developments
author: Tech Hub Team
date: 2025-08-25 09:00:00 +00:00
tags:
- .NET Modernization
- AI Agents
- Azure Updates
- CI/CD Pipelines
- Cloud Infrastructure
- Developer Productivity
- DevOps Automation
- Enterprise Security
- Git
- MCP
- ML Optimization
- Quantum Safe Security
- VS
- Workflow Automation
- AI
- GitHub Copilot
- Machine Learning
- Azure
- Coding
- DevOps
- Security
- Roundups
section_names:
- ai
- github-copilot
- ml
- azure
- coding
- devops
- security
primary_section: github-copilot
feed_name: TechHub
---
Welcome to this week’s roundup, where AI, cloud platforms, and developer-focused tools continue to shape developer workflows. GitHub Copilot leads with new premium models, improved workflow automation, and enhanced customization for both individual and organizational users. Teams now have more tools to manage Copilot and adopt Spaces for context-driven collaboration, along with better diagnostics in Visual Studio and .NET environments.

Azure continues to expand with Service Groups, serverless hosting with Model Context Protocol (MCP), enhanced SDK and storage options, and strengthened security for AI/ML deployments. Machine learning teams benefit from improved optimization tools and agent-based reasoning, while DevOps pipelines integrate AI for streamlined CI/CD, observability, and workflow automation. Security updates address evolving challenges, with previews of quantum-safe cryptography and updated DevSecOps tooling integrated at every level. Collectively, this week’s changes support more intelligent, governed, and reliable technology stacks for innovation at scale.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot: New Models, Automation Workflows, and Core Feature Updates](#github-copilot-new-models-automation-workflows-and-core-feature-updates)
  - [Copilot in Visual Studio and .NET Ecosystem: Diagnostics, Controls, and Integration](#copilot-in-visual-studio-and-net-ecosystem-diagnostics-controls-and-integration)
  - [Model Context Protocol (MCP): Advanced Extensibility and Custom AI Workflows](#model-context-protocol-mcp-advanced-extensibility-and-custom-ai-workflows)
  - [Copilot Spaces and Context Management: Transitioning from Knowledge Bases to Spaces](#copilot-spaces-and-context-management-transitioning-from-knowledge-bases-to-spaces)
  - [Everyday Productivity, Workflow Automation, and Developer Experience](#everyday-productivity-workflow-automation-and-developer-experience)
  - [Copilot Customization: Bring Your Own Models and VS Code Personalization](#copilot-customization-bring-your-own-models-and-vs-code-personalization)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Agentic AI Development on Azure AI Foundry](#agentic-ai-development-on-azure-ai-foundry)
  - [Advanced AI Features and Integration in Developer Tools](#advanced-ai-features-and-integration-in-developer-tools)
  - [Local Model Hosting and Deployment with .NET and Foundry Local](#local-model-hosting-and-deployment-with-net-and-foundry-local)
  - [Workflow Automation and Copilot Studio Development](#workflow-automation-and-copilot-studio-development)
  - [Specialty Agents and Agent-Centered Design](#specialty-agents-and-agent-centered-design)
- [ML](#ml)
  - [Large-Scale AI Pretraining Optimization on Azure ND GB200 v6](#large-scale-ai-pretraining-optimization-on-azure-nd-gb200-v6)
  - [Feature Updates: Enhanced AI Capability and Developer Workflow](#feature-updates-enhanced-ai-capability-and-developer-workflow)
- [Azure](#azure)
  - [Azure Service Groups and Advanced Resource Management](#azure-service-groups-and-advanced-resource-management)
  - [MCP Server Hosting and Azure Functions Flex Consumption](#mcp-server-hosting-and-azure-functions-flex-consumption)
  - [Azure SDK: AI Libraries, Observability, and Data Movement](#azure-sdk-ai-libraries-observability-and-data-movement)
  - [Enterprise AI/ML Security and Scalability with Azure Application Gateway](#enterprise-aiml-security-and-scalability-with-azure-application-gateway)
  - [Microsoft Fabric: Workspace Security, Metric Insights, and Data Orchestration](#microsoft-fabric-workspace-security-metric-insights-and-data-orchestration)
  - [Elastic SAN, Storage Best Practices, and Data Protection](#elastic-san-storage-best-practices-and-data-protection)
  - [SQL and Data Connectivity in Microsoft Fabric and Power BI](#sql-and-data-connectivity-in-microsoft-fabric-and-power-bi)
  - [Developer Experience and Tooling: Azure Developer CLI (azd)](#developer-experience-and-tooling-azure-developer-cli-azd)
  - [Azure Platform Updates: Infrastructure, Serverless, and Security Enhancements](#azure-platform-updates-infrastructure-serverless-and-security-enhancements)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [Git 2.51: Storage, Workflow, and CLI Enhancements](#git-251-storage-workflow-and-cli-enhancements)
  - [.NET Testing Modernization: CLI, TUnit Migration, and Email Workflow](#net-testing-modernization-cli-tunit-migration-and-email-workflow)
  - [.NET Application Modernization: Migration, WebView2, and Obsolete APIs](#net-application-modernization-migration-webview2-and-obsolete-apis)
- [DevOps](#devops)
  - [GitHub Platform Enhancements and Developer Workflow Updates](#github-platform-enhancements-and-developer-workflow-updates)
  - [Advancing Observability and Kubernetes Troubleshooting](#advancing-observability-and-kubernetes-troubleshooting)
  - [AI-Driven Automation and the Evolution of DevOps Pipelines](#ai-driven-automation-and-the-evolution-of-devops-pipelines)
  - [CI/CD Workflows, Testing, and Seamless Integrations](#cicd-workflows-testing-and-seamless-integrations)
  - [Observability, Debugging, and Production Reliability](#observability-debugging-and-production-reliability)
  - [The Expanding Ecosystem: AI-Powered Content, Fusion Development, and Cost Optimization](#the-expanding-ecosystem-ai-powered-content-fusion-development-and-cost-optimization)
- [Security](#security)
  - [GitHub Platform Security: Developer-first Tools and Enhanced Secret Scanning](#github-platform-security-developer-first-tools-and-enhanced-secret-scanning)
  - [Quantum-safe Cryptography: Preparing for a Post-Quantum Security Era](#quantum-safe-cryptography-preparing-for-a-post-quantum-security-era)
  - [Microsoft Defender and Security Copilot: Threat Detection, Response, and Automation](#microsoft-defender-and-security-copilot-threat-detection-response-and-automation)
  - [DevSecOps and Software Supply Chain: From Privacy by Design to Lifecycle Visibility](#devsecops-and-software-supply-chain-from-privacy-by-design-to-lifecycle-visibility)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot delivered updated features this week, introducing premium model options, customizable workflows with Model Context Protocol (MCP), and new code editing capabilities to support more personalized AI-assisted coding. GitHub refined controls, extended automation, and consolidated context management in response to user feedback. Copilot’s transition from legacy environments to new collaboration solutions is ongoing, with organizations gaining more oversight for policy and spending, as illustrated by new case studies.

### GitHub Copilot: New Models, Automation Workflows, and Core Feature Updates

Gemini 2.5 Pro is now available to all Copilot Pro/Pro+, Business, and Enterprise users. GitHub continues to support flexible, multi-model work with Gemini, OpenAI’s latest models, and more, enabling users to choose the AI that fits their work best. Gemini is selectable in Copilot Chat across major IDEs including VS Code, Visual Studio, JetBrains, Xcode, and Eclipse, and organization administrators can set Gemini as the default, following similar GPT-5 organization rollouts.

Copilot provides commit message suggestions in the GitHub web editor for all users, moving automation beyond code writing to workflow documentation. Copilot Spaces improvements add precision in linking to repositories, directories, files, and branches, further supporting collaboration. These updates expand on earlier context management updates, making Spaces more practical for day-to-day team work.

Workflow automation receives an upgrade with the Copilot coding agent and the Agents panel, available to all Copilot paid users. Developers can now assign tasks, track status, and manage pull request drafts directly on GitHub.com, shifting Copilot’s capability from code suggestion toward comprehensive project automation.

- [Gemini 2.5 Pro Model Now Available for GitHub Copilot Users](https://github.blog/changelog/2025-08-19-gemini-2-5-pro-is-generally-available-in-copilot)
- [GitHub Copilot Introduces Commit Message Suggestions and Spaces Enhancements](https://github.blog/changelog/2025-08-21-copilot-generated-commit-messages-on-github-com-is-in-public-preview)
- [Agents Panel: Easily Delegate Tasks to Copilot Coding Agent Across GitHub](https://github.blog/changelog/2025-08-19-agents-panel-launch-copilot-coding-agent-tasks-anywhere-on-github-com)
- [Agents Panel: Delegate Copilot Coding Agent Tasks Anywhere on GitHub](https://github.blog/news-insights/product-news/agents-panel-launch-copilot-coding-agent-tasks-anywhere-on-github/)

### Copilot in Visual Studio and .NET Ecosystem: Diagnostics, Controls, and Integration

Visual Studio continues to improve Copilot for .NET with expanded debugging and smarter context support. The Copilot Diagnostics tool now brings AI-powered debugging—with automated breakpoints, LINQ analysis, and focused profiling for CPU and memory. Aspire 9.3 now includes Copilot for live troubleshooting and log review, following last week's integration of Copilot into cloud-native .NET applications.

Refined Copilot controls in Visual Studio respond directly to developer feedback. Updated delay settings for completions, selective suggestion handling, and keyboard shortcuts help developers maintain control over automated code suggestions.

Model Context Protocol (MCP) integration in Visual Studio has reached general availability. Using `.mcp.json` files, developers can now connect to custom or community MCP servers, simplifying adoption of secure context automation.

- [Copilot Diagnostics Toolset Enhances .NET Debugging in Visual Studio](https://devblogs.microsoft.com/dotnet/github-copilot-diagnostics-toolset-for-dotnet-in-visual-studio/)
- [GitHub Copilot Integration in .NET Aspire 9.3 Dashboard](/ai/videos/github-copilot-integration-in-net-aspire-93-dashboard)
- [GitHub Copilot Now Integrated Into .NET Aspire Dashboard](/ai/videos/github-copilot-now-integrated-into-net-aspire-dashboard)
- [Better Control Over GitHub Copilot Code Suggestions in Visual Studio](https://devblogs.microsoft.com/visualstudio/better-control-over-your-copilot-code-suggestions/)
- [Model Context Protocol (MCP) Is Now Generally Available in Visual Studio](https://devblogs.microsoft.com/visualstudio/mcp-is-now-generally-available-in-visualstudio/)

### Model Context Protocol (MCP): Advanced Extensibility and Custom AI Workflows

Building on prior enterprise automation news, this week introduces detailed guides and general availability for custom MCP servers. With MCP, Copilot can connect to internal APIs and business applications, supporting tailored automation. Real examples—such as a TypeScript server for turn-based games or automated class naming—demonstrate how teams can use MCP to apply domain-specific automation within their preferred IDEs.

With full support for MCP in Visual Studio, organizations can better manage secure orchestration and contextual workflows, especially where process control is essential.

- [Building Your First MCP Server: Extending GitHub Copilot with Custom Tools](https://github.blog/ai-and-ml/github-copilot/building-your-first-mcp-server-how-to-extend-ai-tools-with-custom-capabilities/)
- [Generating Classes with Custom Naming Conventions Using GitHub Copilot and a Custom MCP Server](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/generating-classes-with-custom-naming-conventions-using-github/ba-p/4444837)
- [Model Context Protocol (MCP) Is Now Generally Available in Visual Studio](https://devblogs.microsoft.com/visualstudio/mcp-is-now-generally-available-in-visualstudio/)

### Copilot Spaces and Context Management: Transitioning from Knowledge Bases to Spaces

Copilot Spaces is scheduled to fully replace the former knowledge base system by September 2025. This change consolidates Copilot’s context features into a unified platform. Spaces now support linking to code, documentation, images, issues, pull requests, and more, allowing for comprehensive context and information sharing. Users should move their content using migration tools before retirement. This change makes Spaces the central location for team-oriented, context-driven AI workflows.

- [GitHub Copilot Knowledge Bases Retiring: Transition to Copilot Spaces](https://github.blog/changelog/2025-08-20-sunset-notice-copilot-knowledge-bases)

### Everyday Productivity, Workflow Automation, and Developer Experience

New resources this week emphasize Copilot’s impact on productivity within .NET and workflows involving multiple files and TDD. Guides show how Copilot aids in tracing dependencies, maintaining cross-file consistency, and organizing large projects. The system now provides support for TDD, prototyping, and code scaffolding by improving its understanding of file relationships.

Recent case studies, such as Bank Galicia, demonstrate that Copilot is increasingly essential for teams seeking more reliable and faster delivery, evolving from small pilots to standard practice.

- [Work Smarter Across Multiple Files with GitHub Copilot](https://cooknwithcopilot.com/blog/work-smarter-across-multiple-files.html)
- [Boosting Productivity with GitHub Copilot: Real-World .NET Coding Examples](https://dellenny.com/boosting-productivity-with-github-copilot-real-world-net-coding-examples/)
- [How GitHub Copilot Helps with Test-Driven Development (TDD)](https://dellenny.com/how-github-copilot-helps-with-test-driven-development-tdd/)
- [How to Test Nonexistent Code with GitHub Copilot](/ai/videos/how-to-test-nonexistent-code-with-github-copilot)
- [Software Developers in Argentina’s Financial Sector Boost Innovation with GitHub Copilot and AI](https://news.microsoft.com/source/latam/features/ai/galicia-naranja-x-github-copilot/?lang=en)

### Copilot Customization: Bring Your Own Models and VS Code Personalization

Visual Studio Chat now lets developers integrate third-party models (such as OpenAI, Anthropic, or Gemini) by providing their API keys and selecting from available models, currently for chat-based use only. This builds on Copilot’s existing support for a growing set of models, including GPT-5, Claude, and Gemini. Copilot plans further flexibility for model selection.

Recent guides for VS Code Joyride and Copilot demonstrate workflow personalization. Developers use Copilot within scripts and personalized tools, reflecting a wider trend of individualized automation routines.

- [Bring Your Own Language Model to Visual Studio Chat](https://devblogs.microsoft.com/visualstudio/bring-your-own-model-visual-studio-chat/)
- [VS Code Live: Scripting with Joyride and GitHub Copilot](/ai/videos/vs-code-live-scripting-with-joyride-and-github-copilot)

### Other GitHub Copilot News

New for Business and Enterprise customers, Copilot now features a request overage policy. This lets administrators set usage boundaries or enable overage pricing to control expenses—a capability needed as Copilot adoption grows in organizations.

The announcement of GitHub Universe 2025 highlights technical sessions on Copilot, Actions, security, and community learning. As with past conferences, these sessions aim to help teams optimize their use of GitHub’s AI and collaboration tools at scale.

- [GitHub Copilot Business and Enterprise: Premium Request Overage Policy Now Available](https://github.blog/changelog/2025-08-22-premium-request-overage-policy-is-generally-available-for-copilot-business-and-enterprise)
- [Explore GitHub Universe 2025: Dev Tools, Community Spaces, and More](https://github.blog/news-insights/company-news/explore-the-best-of-github-universe-9-spaces-built-to-spark-creativity-connection-and-joy/)

## AI

This week, the AI landscape included updated frameworks for agent-based development, additional options for local deployment, enhancements to developer tools, and resources for constructing agentic and generative AI systems. The focus continued to be on flexible integration—local or cloud—and supporting enterprise-scale agent architectures as well as individual productivity needs. Tutorials centered on adaptable model options, privacy, and streamlined AI workflow orchestration.

### Agentic AI Development on Azure AI Foundry

Building on previous content about Agent Factory and orchestration, this week presents new guides and technical references for deploying agents within Azure AI Foundry. Resources support enterprise use, including documentation for MCP integration and Logic Apps connectivity.

A multi-agent architecture reference, complete with industry case studies (such as cybersecurity and retail), extends earlier material about governance and versioning in production environments.

Guidance on selecting models is now broader and more practical for first-time deployment. New resources walk through the use of the Foundry Model Catalog and Model Router, continuing to address compliance and matching use cases to specific business needs—reiterating points made in earlier roundups.

- [Agent Factory: Building Your First AI Agent with Azure AI Foundry](https://azure.microsoft.com/en-us/blog/agent-factory-building-your-first-ai-agent-with-the-tools-to-deliver-real-world-outcomes/)
- [Designing Multi-Agent Intelligence: Microsoft Reference Architecture and Enterprise Case Studies](https://devblogs.microsoft.com/blog/designing-multi-agent-intelligence)
- [How to Choose the Right Model for Your AI Agent: A Developer’s Guide](https://techcommunity.microsoft.com/t5/microsoft-developer-community/how-do-i-choose-the-right-model-for-my-agent/ba-p/4445267)

### Advanced AI Features and Integration in Developer Tools

Extending earlier GPT-5 integration news, Visual Studio Code now allows developers to switch between GPT-5 and GPT-5 mini, giving them more direct control over price and speed. These adjustments are part of the Copilot/VS Code move toward greater convergence and personalization. Additional features such as ‘beast mode’ and task list automation give users new customized workflows.

Azure AI Foundry’s new GPT-5 freeform tool calling allows for flexible Python/SQL execution, moving beyond previous, more restricted function-call API patterns. These capabilities reflect the increasing sophistication of agentic and orchestrated workflows.

In addition, a new tutorial on Mistral Document AI provides hands-on steps for incorporating document parsing into developer environments, supporting conversion of unstructured PDFs and handwriting to structured, AI-ready data.

- [Hello GPT-5 & GPT-5 mini: New AI Features in VS Code Agent Mode](/ai/videos/hello-gpt-5-and-gpt-5-mini-new-ai-features-in-vs-code-agent-mode)
- [Unlocking GPT-5’s Freeform Tool Calling in Azure AI Foundry](https://devblogs.microsoft.com/foundry/unlocking-gpt-5s-freeform-tool-calling-a-new-era-of-seamless-integration/)
- [Mistral Document AI Integration with Azure AI Foundry](/ai/videos/mistral-document-ai-integration-with-azure-ai-foundry)

### Local Model Hosting and Deployment with .NET and Foundry Local

Following prior coverage of local model inference, this week’s resources include detailed instructions for running open-source models such as GPT-OSS within C# projects using Ollama. These methods align with recent developments for Foundry Local and ONNX integration—letting developers deploy streaming chatbots and retrieval-augmented generation solutions on local machines. Forthcoming enhancements in Windows and hardware acceleration reinforce the trend toward hybrid AI workloads.

“Beginner’s Guide” articles detail use of Foundry Local alongside Microsoft Olive, covering ONNX optimization, choosing formats, and troubleshooting—helping more developers move toward flexible AI deployments.

- [Running GPT-OSS Locally in C# Using Ollama and Microsoft.Extensions.AI](https://devblogs.microsoft.com/dotnet/gpt-oss-csharp-ollama/)
- [Beginner’s Guide: Using Custom AI Models with Foundry Local and Microsoft Olive](https://techcommunity.microsoft.com/t5/educator-developer-blog/how-to-use-custom-models-with-foundry-local-a-beginner-s-guide/ba-p/4428857)

### Workflow Automation and Copilot Studio Development

Continuing from last week’s focus on no-code tools, this week offers resources for deeper organizational integration in Copilot Studio. In-depth guides explain the creation of custom plugins and connectors, including advanced OpenAPI authentication, supporting more tailored organizational automation strategies.

Step-by-step resources cover integrating Copilot Studio with Power Automate for process automation involving platforms like SharePoint and Dynamics 365. The system’s library of over a thousand prebuilt connectors further supports broader enterprise workflow automation.

- [Creating Custom Plugins and Connectors in Copilot Studio](https://dellenny.com/creating-custom-plugins-and-connectors-in-copilot-studio/)
- [Integrating Copilot Studio with Power Automate for End-to-End Workflows](https://dellenny.com/integrating-copilot-studio-with-power-automate-for-end-to-end-workflows/)

### Specialty Agents and Agent-Centered Design

This week’s updates highlight the transition from traditional user experience (UX) to agent experience (AX). The Lacuna agent, created using Copilot Studio and AI Foundry, is designed to identify hidden assumptions in product design, expanding on last week’s discussion of agent-based collaboration. Agents focused on domains such as code review and risk assessment are also featured.

The discussion encourages a new focus on agent-centered design, emphasizing planning, orchestration, and domain expertise over simple chatbot-based systems. This builds on last week’s analysis of GPT-4-based planners and Semantic Kernel tools, demonstrating real-world adoption in risk and assumption management.

- [The Future of AI: Developing Lacuna – An Agent for Revealing Quiet Assumptions in Product Design](https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/the-future-of-ai-developing-lacuna-an-agent-for-revealing-quiet/ba-p/4434633)
- [From UX to AX: Why Agent Experience is the Next Frontier in Business AI](/ai/videos/from-ux-to-ax-why-agent-experience-is-the-next-frontier-in-business-ai)

## ML

This week’s machine learning news focuses on practical improvements to scaling model training and agent development. Developers now have guides for optimizing pretraining at scale, new research on agent reasoning, and updates to data engineering workflows that simplify testing and iteration. The overall direction continues to support efficient ML development and deployment.

### Large-Scale AI Pretraining Optimization on Azure ND GB200 v6

Building on last week’s discussions around cloud-based model training, this week’s benchmarking research provides detailed recommendations for optimizing the pretraining of Llama3 8B models on Azure ND GB200 v6. The study covers adjustments to tensor, pipeline, context, and data parallelism, repeating last week’s strategies for deploying scalable workloads using Azure AKS and vLLM. Benchmarking batch sizes and numerical precision modes, the authors recommend specific parameters for the best throughput: tensor parallelism 1, pipeline parallelism 2, context parallelism 1, and micro batch size 4.

All scripts are shared for reproducibility via the Azure AI Benchmarking Guide, supporting transparent scaling and tuning for teams running production ML on large clusters.

- [Optimizing Large-Scale AI Performance with Pretraining Validation on a Single Azure ND GB200 v6](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/optimizing-large-scale-ai-performance-with-pretraining/ba-p/4445273)

### Feature Updates: Enhanced AI Capability and Developer Workflow

Following on recent analytics and optimizer updates, MindJourney—developed by Microsoft Research—improves spatial reasoning for agents in dynamic, simulated environments. Integrating a pretrained world model and spatial beam search, MindJourney improves agent navigation and accuracy by 8% without requiring agent retraining, with clear uses in robotics, simulation, and accessibility development.

Microsoft Fabric’s new “Develop mode” for User Data Functions now provides a safe editor for testing Python logic before production deployment. This is a direct response to calls for safer, more controlled custom code testing in platforms like Spark, Databricks, and Fabric, and only requires a library update to enable.

- [MindJourney: AI Agents Navigate and Reason in Simulated 3D Worlds](https://www.microsoft.com/en-us/research/blog/mindjourney-enables-ai-to-explore-simulated-3d-worlds-to-improve-spatial-interpretation/)
- [Test and Validate Functions with Develop Mode in Fabric User Data Functions](https://blog.fabric.microsoft.com/en-US/blog/test-and-validate-your-functions-with-develop-mode-in-fabric-user-data-functions-preview/)

## Azure

Azure’s recent updates introduce new resource management options, deeper AI features, upgraded tooling, and security improvements. The latest previews and releases make it easier to manage core infrastructure, automate developer workflows, and secure data at scale—continuing to build on recent governance, migration, and optimization guidance.

### Azure Service Groups and Advanced Resource Management

Azure Service Groups are now in public preview, allowing resource grouping for targeted monitoring and health checks at the tenant level. These containers function separately from traditional RBAC or policy scopes, making it easier to group resources for app-centric monitoring or detailed cost analysis via the REST API, portal, and Azure Monitor health model.

- [Announcing Public Preview for Azure Service Groups](https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-public-preview-for-azure-service-groups/ba-p/4446572)

### MCP Server Hosting and Azure Functions Flex Consumption

Developers can now deploy remote MCP servers as fully serverless applications using Azure Functions Flex Consumption (early preview), supporting Python, Node.js, and .NET SDKs. Integration options include the Azure MCP Extension or your own SDK, with deployment managed via Azure CLI, local debugging, and API Management extensions. The model allows flexible scaling and cost management, and early adopter feedback will help refine it further.

This development supports a shift towards truly cloud-native, serverless MCP deployments, following recent expansions to MCP integration and IDE support.

- [Early Preview: Host Your Own Remote MCP Server on Azure Functions](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-early-preview-byo-remote-mcp-server-on-azure/ba-p/4445317)

### Azure SDK: AI Libraries, Observability, and Data Movement

The August Azure SDK update covers releases and betas for several programming languages. New AI libraries for JavaScript and Python are now generally available and integrate with Azure AI Foundry and Azure OpenAI Services. .NET’s updated Storage Data Movement library addresses migration and file transfer pain points, and new Management, Monitor, and Metrics tools are being introduced for Carbon Optimization, Recovery Services, Rust language support, browser automation, and storage management. Migration documentation supports smoother setup and better cost visibility.

These SDK improvements build on previous enhancements, supporting interoperability, AI integration, and ongoing cross-language support.

- [Azure SDK Release Highlights for August 2025](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-august-2025/)

### Enterprise AI/ML Security and Scalability with Azure Application Gateway

An in-depth analysis outlines best practices for using Azure Application Gateway as a secure entry point for AI/ML services, including OpenAI, Cognitive Services, and custom APIs. The article covers routing, TLS/mTLS, web application firewalls, logging, observability, and integration practices. Adaptive load routing and planned features aim to support demanding workloads.

- [Scaling Enterprise AI/ML: Azure Application Gateway as the Intelligent Access Layer](https://techcommunity.microsoft.com/t5/azure-networking-blog/unlock-enterprise-ai-ml-with-confidence-azure-application/ba-p/4445691)

### Microsoft Fabric: Workspace Security, Metric Insights, and Data Orchestration

Microsoft Fabric now offers workspace-level Private Link (preview), allowing granular network isolation per workspace. The Fabric Capacity Metrics App preview introduces an Item History page for deeper evaluation of compute trends and resource planning.

Fabric Data Factory Copy Jobs now support multiple schedulers, reducing duplication and simplifying integration with CI/CD and pipelines. New OpenAPI spec generation features make it easier to integrate function APIs with other systems.

These features expand on recent security, orchestration, and automation enhancements in Fabric, showing a stronger focus on precise management and developer automation.

- [Microsoft Fabric Introduces Workspace-Level Private Link (Preview)](https://blog.fabric.microsoft.com/en-US/blog/fabric-workspace-level-private-link-preview/)
- [Preview of Item History Page in Microsoft Fabric Capacity Metrics App](https://blog.fabric.microsoft.com/en-US/blog/26307/)
- [Simplifying Data Ingestion with Copy Job: Multiple Scheduler Support in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-multiple-scheduler-support/)
- [OpenAPI Specification Generation in Fabric User Data Functions](https://blog.fabric.microsoft.com/en-US/blog/openapi-specification-code-generation-now-available-in-fabric-user-data-functions/)

### Elastic SAN, Storage Best Practices, and Data Protection

Azure Elastic SAN now features integrated backup through Azure Backup and Commvault, currently in preview. Azure Backup provides lifecycle-managed snapshots, while Commvault enables rapid cross-region restore and protection. Both are designed for VM-oriented protection and meet cost/retention needs with detailed setup guidance.

A comprehensive Azure Storage guide covers product fundamentals, security, cost, and best practices for Blob, File, Queue, Table, Disk, and Elastic SAN services—continuing the ongoing evolution of storage solutions.

- [Enhance Your Data Protection Strategy with Azure Elastic SAN’s Newest Backup Options](https://techcommunity.microsoft.com/t5/azure-storage-blog/enhance-your-data-protection-strategy-with-azure-elastic-san-s/ba-p/4443607)
- [Azure Storage: Fundamentals, Services, and Community Best Practices](https://techcommunity.microsoft.com/t5/azure/azure-storage/m-p/4447460#M22137)

### SQL and Data Connectivity in Microsoft Fabric and Power BI

A new video walks through Fabric SQL database setup, OneLake and Purview management, service selection, and live analytics integration, offering practical insights for Fabric’s SQL and pricing. The on-premises Data Gateway August release introduces Entra ID authentication for PostgreSQL, improving security and supporting more secure cloud analytics.

These advances align with earlier efforts to unify and secure data connectivity across the Microsoft ecosystem.

- [SQL Database in Microsoft Fabric](/azure/videos/sql-database-in-microsoft-fabric)
- [On-premises Data Gateway August 2025 Release: Entra ID Support for PostgreSQL](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-august-2025-release/)

### Developer Experience and Tooling: Azure Developer CLI (azd)

The August 2025 release of the Azure Developer CLI enhances reliability, adds documentation, and expands template options. Enhancements cover PowerShell support, .NET Aspire detection, Visual Studio fixes, and improved workflows for environment and deployment management. Documentation now covers configuration, CI/CD, and progression from local builds to production. A template library further assists teams with deployments involving monitoring, AI, and data integration.

These updates continue last week’s improvements for development toolchains and automation on Azure.

- [Azure Developer CLI (azd) August 2025 Release Overview](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-august-2025/)

### Azure Platform Updates: Infrastructure, Serverless, and Security Enhancements

Recent Azure updates include new VM options, improved diagnostics, and flexible deployment solutions. New DC EC esv6 VMs support specialized workloads, AKS now offers integrated Azure Bastion for easier remote access, serverless Functions can now scale with Flex Consumption, and Application Gateway receives maxSurge for zero-downtime updates. Storage improvements clarify Azure Files Premium cost, add Blob Storage regional archiving, and enhance NetApp Files logging and cool access settings. Log Analytics now supports up to 100M results per query, and deprecated workflows (Sentinel/Defender in China, CNAME certs) are announced.

These enhancements build on work to make Azure’s platform more robust, adaptable, and cost-efficient.

- [Azure Update - 22nd August 2025](/azure/videos/azure-update-22nd-august-2025)

### Other Azure News

Azure File Sync now leverages managed identities, moving away from credential-based management for stronger security and simpler operations, with migration tutorials and PowerShell scripts provided.

Microsoft’s open source journey entry shares their contributions from the Linux kernel to global-scale AI—including AKS, Dapr, Radius, and published best practices.

A new entry in Azure Essentials covers saving costs with Azure Hybrid Benefit, giving users details on licensing, rights, and migration planning tools.

These updates underline Azure’s focus on managed identities, open source partnership, and actionable cost management advice—consistent with last week's directions.

- [Azure File Sync Managed Identity: Enhanced Security and Simplified Operations](/azure/videos/azure-file-sync-managed-identity-enhanced-security-and-simplified-operations)
- [Microsoft’s Open Source Journey: From Linux Contributions to AI at Scale](https://azure.microsoft.com/en-us/blog/microsofts-open-source-journey-from-20000-lines-of-linux-code-to-ai-at-global-scale/)
- [Your Guide to Saving with Azure Hybrid Benefit](/azure/videos/your-guide-to-saving-with-azure-hybrid-benefit)

## Coding

Developers benefit this week from updated toolchains and workflow features, helping with modernization and streamlining in both Git and .NET. Git delivers new storage and workflow improvements, while .NET introduces new approaches for testing, migration, and UI challenges.

### Git 2.51: Storage, Workflow, and CLI Enhancements

Git 2.51 delivers enhancements to storage and workflow efficiency. The new cruft-free MIDX feature allows for duplication of reachable objects from cruft packs, resulting in smaller, faster repositories and up to 38% storage reduction. This is managed via the `repack.MIDXMustContainCruft` setting. The updated `git repack --path-walk` uses file layout to optimize delta compression and pack size.

A new stash interchange format allows linked ancestor commits, enabling stash export/import across devices. Scripting is improved via more accurate `git cat-file` submodule reporting; commit-graph Bloom filters offer accelerated path filtering in large repositories.

The commands `git switch` and `git restore` are now stable, while the deprecated `git whatchanged` is replaced by `git log --raw`. Looking ahead, Git 3.0 will default to SHA-256 and implement a new reftable backend, so users should start planning for migration. Updates to C99 support and patch submission workflows further modernize the codebase.

These updates maintain Git’s focus on workflow effectiveness and code modernization.

- [Key Updates in Git 2.51: Cruft-Free MIDX, Stash Interchange, and More](https://github.blog/open-source/git/highlights-from-git-2-51/)

### .NET Testing Modernization: CLI, TUnit Migration, and Email Workflow

.NET 10 now runs `dotnet test` using the Microsoft.Testing.Platform (MTP), replacing the previous VSTest engine. The update improves automation, performance, diagnostics, filtering, parallelism, and output. Developers should migrate tests to MTP, update configurations, and remove obsolete settings for simpler and faster solution builds.

Migration resources confirm that moving from xUnit to TUnit is direct, with analyzer and source generator support for parallel, NativeAOT-ready, and .NET Standard 2.0 projects. Guides cover assert conversion, snapshot usage, and CI integration.

A new tutorial explains how to implement reliable email sending within .NET, covering SMTP setup, formatting, debugging, and best practices for maintainable code.

- [Enhance your CLI testing workflow with the new dotnet test](https://devblogs.microsoft.com/dotnet/dotnet-test-with-mtp/)
- [Migrating an xUnit Test Project to TUnit: Experience, Issues, and Practical Steps](https://andrewlock.net/converting-an-xunit-project-to-tunit/)
- [Sending Email Correctly in .NET](/coding/videos/sending-email-correctly-in-net)

### .NET Application Modernization: Migration, WebView2, and Obsolete APIs

Migrating .NET Framework 4.8 applications to .NET 8 is now easier, with tools like Upgrade Assistant, Portability Analyzer, and Roslyn analyzers designed for incremental and batch migration. Documentation and automation help prioritize modernization and minimize risk.

These resources support ongoing themes around cross-platform modernization and multi-targeted project upgrades, including those for Aspire and MAUI. The focus is on easy migration of APIs and structuring large solution upgrades.

WebView2 now supports improved keyboard input mapping, simplifying desktop usability in WPF/WinForms applications. The new `CoreWebView2ControllerOptions.AllowHostInputProcessing` property restores expected keyboard behaviors, supporting better integration between host and browser-based UIs.

Additional tools are available for mapping obsolete APIs to .NET 8. This continues last week’s theme of smoother API and UI modernization.

- [Handling Keyboard Mapping in WebView2 with AllowHostInputProcessing](https://weblog.west-wind.com/posts/2025/Aug/20/Using-the-new-WebView2-AllowHostInputProcessing-Keyboard-Mapping-Feature)
- [Tools and Approaches for Migrating Obsolete .NET Framework APIs to .NET 8](https://techcommunity.microsoft.com/t5/tools/tool-or-approach-to-identify-and-replace-obsolete-net-framework/m-p/4446845#M161)

## DevOps

This week’s DevOps updates include new features and integrations to support more reliable workflows, stronger observability, and increased use of AI for automation, all while emphasizing oversight and collaboration. GitHub adds improved permissions, dependency management, and UI features. AI is being integrated into CI/CD, combining productivity with careful governance. Practical guides help troubleshoot Kubernetes, automate Angular CI, and link Azure DevOps with Jira. The ecosystem maintains a focus on cost, quality, and productivity.

### GitHub Platform Enhancements and Developer Workflow Updates

GitHub’s recent updates include general availability for issue dependency management, support for enterprise-level custom organization roles and increased limits, pull request improvements, and Dependabot Rust toolchain automation. New features for cost attribution and repository migration align with GitHub’s focus on usability and admin features, and the retirement of GraphQL Explorer reflects ongoing documentation and API enhancements.

- [Managing Issue Dependencies in GitHub Now Generally Available](https://github.blog/changelog/2025-08-21-dependencies-on-issues)
- [Enterprise-Wide Custom Organization Roles and Increased Role Limits in GitHub](https://github.blog/changelog/2025-08-21-enterprises-can-create-organization-roles-for-use-across-their-enterprise-and-custom-role-limits-have-been-increased)
- [GitHub Pull Request 'Files Changed' Public Preview: August 21 Updates](https://github.blog/changelog/2025-08-21-pull-request-files-changed-public-preview-experience-august-21-updates)
- [Dependabot Adds Support for Automated Rust Toolchain Updates](https://github.blog/changelog/2025-08-19-dependabot-now-supports-rust-toolchain-updates)
- [Manage Cost Center Users in GitHub Enterprise Cloud via Billing UI and API](https://github.blog/changelog/2025-08-18-customers-can-now-add-users-to-a-cost-center-from-both-the-ui-and-api-2)
- [Migrate Repositories Using GitHub-Owned Blob Storage](https://github.blog/changelog/2025-08-18-migrate-repositories-with-github-owned-blob-storage)
- [GraphQL Explorer Removal from GitHub API Documentation in 2025](https://github.blog/changelog/2025-08-21-graphql-explorer-removal-from-api-documentation-on-november-1-2025)

### Advancing Observability and Kubernetes Troubleshooting

New tools such as Retina and eBPF for Kubernetes support deeper inspection and debugging for cloud workloads. These resources extend earlier distributed tracing and monitoring improvements, helping teams trace issues in modern networking environments.

- [Troubleshooting Kubernetes Network Issues with Retina and eBPF](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/troubleshooting-network-issues-with-retina/ba-p/4446071)

### AI-Driven Automation and the Evolution of DevOps Pipelines

Recent developments in AI integration for DevOps build on previous releases for agents, pipelines, and MCP tools. Discussions cover the potential and challenges of AI-driven orchestration in CI/CD, with articles emphasizing platform engineering, robust oversight, and the role of humans in overseeing automated, agent-based pipelines.

Contextual engineering is also stressed as necessary for safe and practical automation. Case studies illustrate the stepwise adoption of smarter, more context-rich automation practices.

- [How MCP Is Shaping the Future of DevOps Processes](https://devops.com/mcp-emerges-as-a-catalyst-for-modern-devops-processes/?utm_source=rss&utm_medium=rss&utm_campaign=mcp-emerges-as-a-catalyst-for-modern-devops-processes)
- [How AI-Created Code Will Strain DevOps Workflows](https://devops.com/how-ai-created-code-will-strain-devops-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=how-ai-created-code-will-strain-devops-workflows)
- [Unlocking DevOps-Ready AI Agents Through Context Engineering](https://devops.com/context-engineering-is-the-key-to-unlocking-ai-agents-in-devops/?utm_source=rss&utm_medium=rss&utm_campaign=context-engineering-is-the-key-to-unlocking-ai-agents-in-devops)
- [Why Human Oversight Remains Essential in an AI-Driven DevOps Landscape](https://devops.com/keeping-humans-in-the-loop-why-human-oversight-still-matters-in-an-ai-driven-devops-future/?utm_source=rss&utm_medium=rss&utm_campaign=keeping-humans-in-the-loop-why-human-oversight-still-matters-in-an-ai-driven-devops-future)
- [The Future of DevSecOps in Fully Autonomous CI/CD Pipelines](https://devops.com/white-paper-the-future-of-devsecops-in-a-fully-autonomous-ci-cd-pipeline/?utm_source=rss&utm_medium=rss&utm_campaign=white-paper-the-future-of-devsecops-in-a-fully-autonomous-ci-cd-pipeline)

### CI/CD Workflows, Testing, and Seamless Integrations

Workflow automation guides this week reflect ongoing trends toward secure, frictionless CI/CD pipelines. Articles cover Angular coverage enforcement in Azure DevOps and practical synchronization between Azure DevOps and Jira, supporting smoother testing, deployment, and coordination across development suites.

- [Enforcing Angular Unit Test Coverage in Azure DevOps Pipelines: A Step-by-Step Guide](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/enforcing-angular-unit-test-coverage-in-azure-devops-pipelines-a/ba-p/4446485)
- [Optimizing Azure DevOps and Jira Integration: 5 Real-World Use Cases for DevOps Teams](https://techcommunity.microsoft.com/t5/azure/optimizing-azure-devops-jira-integration-5-practical-use-cases/m-p/4445837#M22123)

### Observability, Debugging, and Production Reliability

Teams can further improve production operations with guidance on structured logging, metrics, and alerting. These resources are designed to help debug live systems and maintain high reliability, building on last week’s monitoring and incident response coverage.

- [Debugging in Production: Leveraging Logs, Metrics and Traces](https://devops.com/debugging-in-production-leveraging-logs-metrics-and-traces/?utm_source=rss&utm_medium=rss&utm_campaign=debugging-in-production-leveraging-logs-metrics-and-traces)

### The Expanding Ecosystem: AI-Powered Content, Fusion Development, and Cost Optimization

Ecosystem-wide updates include the introduction of tech.hub.ms, a platform for curated Microsoft technical content. Fusion development stories show increased adoption of blended business and engineering workflows; articles on FinOps as Code and SRE.ai explore automation and cost-conscious practices across SaaS and DevOps teams.

- [Announcing tech.hub.ms: Curated Microsoft Tech Content Platform](https://r-vm.com/new-website-tech-hub-ms.html)
- [Microsoft Morphs Fusion Developers To Full Stack Builders](https://devops.com/microsoft-morphs-fusion-developers-to-full-stack-builders/?utm_source=rss&utm_medium=rss&utm_campaign=microsoft-morphs-fusion-developers-to-full-stack-builders)
- [FinOps as Code – Unlocking Cloud Cost Optimization](https://devops.com/finops-as-code-unlocking-cloud-cost-optimization/?utm_source=rss&utm_medium=rss&utm_campaign=finops-as-code-unlocking-cloud-cost-optimization)
- [SRE.ai Aims to Streamline DevOps for SaaS with AI Automation](https://devops.com/sre-ai-looks-to-unify-devops-workflows-across-multiple-saas-applications/?utm_source=rss&utm_medium=rss&utm_campaign=sre-ai-looks-to-unify-devops-workflows-across-multiple-saas-applications)

## Security

Security this week emphasizes expanded AI-driven development tools, quantum-safe cryptography integration, and DevSecOps best practices. New resources address continuous security, automation, and software supply chain integrity.

### GitHub Platform Security: Developer-first Tools and Enhanced Secret Scanning

GitHub continues to extend secret scanning and push protection. Organizations now have support for custom secret scanning patterns during push protection, allowing company-specific policies to be enforced as needed. These changes support compliance requirements and help prevent disruptions.

Integration of CodeQL and Copilot Autofix remains central, with security checks a routine part of CI. Security Campaigns and Dependency Review are more widely used to help mitigate supply chain risks as part of standard workflows.

- [Enhancing Code Security with GitHub Tools](/devops/videos/enhancing-code-security-with-github-tools)
- [GitHub Secret Scanning: Custom Pattern Configuration in Push Protection Now Available](https://github.blog/changelog/2025-08-19-secret-scanning-configuring-patterns-in-push-protection-is-now-generally-available)

### Quantum-safe Cryptography: Preparing for a Post-Quantum Security Era

Microsoft advances its Quantum Safe Program (QSP) with previews for NIST PQC algorithms, hybrid TLS 1.3, and hardware integrations. These are now available for hands-on testing on Windows and Linux systems, supporting staged planning for cryptographic updates. Microsoft’s dual strategy combines policy, education, and developer guidance to enable future cryptographic agility.

- [Quantum-safe Security: Microsoft's Progress Toward Next-generation Cryptography](https://www.microsoft.com/en-us/security/blog/2025/08/20/quantum-safe-security-progress-towards-next-generation-cryptography/)
- [Microsoft Unveils Quantum Safe Program Strategy to Prepare for Post-Quantum Security Era](https://blogs.microsoft.com/on-the-issues/2025/08/20/post-quantum-resilience-building-secure-foundations/)

### Microsoft Defender and Security Copilot: Threat Detection, Response, and Automation

Recent updates provide analysis of new malware threats such as PipeMagic, insight into recent social engineering methods (ClickFix), and updated detection strategies. Automation tools like EDR block mode and cloud policy remediation—combined with Sentinel/Defender integrations—demonstrate a stronger focus on flexible, cross-platform security operations.

Security Copilot use expands, providing advanced identity threat protection with Azure Entra, building on improvements detailed last week in automated response and incident closure.

- [Dissecting PipeMagic: Technical Analysis of a Modular Malware Backdoor](https://www.microsoft.com/en-us/security/blog/2025/08/18/dissecting-pipemagic-inside-the-architecture-of-a-modular-backdoor-framework/)
- [Analyzing the ClickFix Social Engineering Technique and Defenses with Microsoft Security](https://www.microsoft.com/en-us/security/blog/2025/08/21/think-before-you-clickfix-analyzing-the-clickfix-social-engineering-technique/)
- [Enhancing Identity Protection with Azure Entra Security Copilot](https://techcommunity.microsoft.com/t5/azure/azure-entra-security-copilot-how-it-s-changing-identity/m-p/4447388#M22132)

### DevSecOps and Software Supply Chain: From Privacy by Design to Lifecycle Visibility

The new HoundDog.ai code scanner supports privacy-first, “shift left” governance—integrating with popular IDEs to detect shadow AI use and protect sensitive data. This parallels efforts by Microsoft and GitHub to strengthen privacy and regulatory compliance tools directly in CI/CD and editor workflows.

Content on visibility and SBOM (Software Bill of Materials) management continues to focus on transparency and regulated supply chain practices, as both threat patterns and policy requirements increase in complexity.

- [HoundDog.ai Code Scanner Shifts Data Privacy Responsibility Left](https://devops.com/hounddog-ai-code-scanner-shifts-data-privacy-responsibility-left/?utm_source=rss&utm_medium=rss&utm_campaign=hounddog-ai-code-scanner-shifts-data-privacy-responsibility-left)
- [Tackling the DevSecOps Gap in Software Understanding](https://devops.com/tackling-the-devsecops-gap-in-software-understanding/?utm_source=rss&utm_medium=rss&utm_campaign=tackling-the-devsecops-gap-in-software-understanding)

### Other Security News

Microsoft Entra ID’s Conditional Access Starter Pack offers a library of scripts and policy templates for automatic policy enforcement, supporting infrastructure-as-code management of identity controls on hybrid and on-premises systems.

A technical guide for Windows 11 details practical steps for configuring Defender for stronger threat intelligence and automation, using PowerShell scripts and custom dashboards to implement layered endpoint protection.

- [Kickstart Conditional Access in Microsoft Entra: Free Starter Pack with Policies & Automation](https://techcommunity.microsoft.com/t5/azure/kickstart-conditional-access-in-microsoft-entra-free-starter/m-p/4447413#M22136)
- [Microsoft Defender Advanced Protection Tips for Windows 11](https://dellenny.com/microsoft-defender-advanced-protection-tips-for-windows-11/)
