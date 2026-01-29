---
title: AI Developer Tools, Security Updates, and Azure Service Improvements This Week
author: Tech Hub Team
date: 2026-01-19 09:00:00 +00:00
tags:
- .NET
- Agentic AI
- Anthropic
- Cloud Infrastructure
- Containerization
- Enterprise
- Identity And Access Management
- Microsoft Azure
- OpenAI
- VS Code
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
Welcome to this week’s technology summary, where updates in AI and cloud platforms are in focus. GitHub Copilot introduces improved automation that adapts to context alongside better integration for business users. Microsoft Azure has new releases that range from vector database search to secure hybrid storage and advanced networking features. In DevOps and security, you’ll find articles covering efforts to disrupt cloud-based cybercrime, new agentic tooling for workflows, and stronger security automation for open source projects and CI/CD. Read below to find out what these releases mean for developer productivity, cloud security, and modernization of data handling throughout the technology stack.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Enhanced AI Models and Model Management in Copilot](#enhanced-ai-models-and-model-management-in-copilot)
  - [Enterprise-Grade Copilot: Integration, BYOK, and Modernization](#enterprise-grade-copilot-integration-byok-and-modernization)
  - [Context Awareness, Agentic Workflows, and Memory Systems](#context-awareness-agentic-workflows-and-memory-systems)
  - [CLI, SDK, and AI Toolkit Updates](#cli-sdk-and-ai-toolkit-updates)
  - [Context Engineering, Collaboration Patterns, and Practical AI Usage](#context-engineering-collaboration-patterns-and-practical-ai-usage)
  - [Real-World Impact and Developer-Centric Analysis](#real-world-impact-and-developer-centric-analysis)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [AI Adoption and Custom Agent Development on Microsoft Marketplace](#ai-adoption-and-custom-agent-development-on-microsoft-marketplace)
  - [Securing and Streamlining Data with Azure AI: PII Redaction and Cost-Effective AI Apps](#securing-and-streamlining-data-with-azure-ai-pii-redaction-and-cost-effective-ai-apps)
  - [Azure AI Model Integration: Troubleshooting, Prompt Fidelity, and Custom Workflows](#azure-ai-model-integration-troubleshooting-prompt-fidelity-and-custom-workflows)
  - [Modernizing Industry Workflows: Azure AI in Healthcare Transcription and Analytics](#modernizing-industry-workflows-azure-ai-in-healthcare-transcription-and-analytics)
- [ML](#ml)
  - [Building AI Workflows with Microsoft Agent Framework and .NET AI Stack](#building-ai-workflows-with-microsoft-agent-framework-and-net-ai-stack)
  - [Industrial ML and Scientific Workflows Powered by Azure HPC and Microsoft Discovery](#industrial-ml-and-scientific-workflows-powered-by-azure-hpc-and-microsoft-discovery)
  - [Expanding Vector Search in Databases: DiskANN in Azure SQL and Fabric SQL](#expanding-vector-search-in-databases-diskann-in-azure-sql-and-fabric-sql)
- [Azure](#azure)
  - [Azure SQL and Developer Tooling Modernization](#azure-sql-and-developer-tooling-modernization)
  - [Azure DevOps and Repository Management](#azure-devops-and-repository-management)
  - [Azure Kubernetes Service: Advanced Identity & Networking](#azure-kubernetes-service-advanced-identity--networking)
  - [AI and Analytics with Azure NetApp Files](#ai-and-analytics-with-azure-netapp-files)
  - [Secure and Flexible Cloud Storage Access](#secure-and-flexible-cloud-storage-access)
  - [Integrating REST APIs with AI Agents](#integrating-rest-apis-with-ai-agents)
  - [Automated Hybrid, File Sync, and Messaging Workflows](#automated-hybrid-file-sync-and-messaging-workflows)
  - [Azure Platform and Service Updates](#azure-platform-and-service-updates)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [Advanced Android Widget Development with .NET MAUI](#advanced-android-widget-development-with-net-maui)
  - [.NET Servicing Releases for January 2026](#net-servicing-releases-for-january-2026)
- [DevOps](#devops)
  - [GitHub Platform Feature Advancements](#github-platform-feature-advancements)
  - [CI/CD Security and Workflow Controls](#cicd-security-and-workflow-controls)
  - [Large-Scale Repository and Build Workflow Engineering](#large-scale-repository-and-build-workflow-engineering)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Criminal VDS Infrastructure and Cybercrime Disruption](#criminal-vds-infrastructure-and-cybercrime-disruption)
  - [Secure Default Hostnames and Log Immutability in Azure](#secure-default-hostnames-and-log-immutability-in-azure)
  - [Security Automation and Developer Workflows in Open Source](#security-automation-and-developer-workflows-in-open-source)
  - [Secret Scanning and Platform Rule Management for GitHub](#secret-scanning-and-platform-rule-management-for-github)
  - [AI Coding Agents and Application Security](#ai-coding-agents-and-application-security)
  - [Secure Power Platform to Azure PaaS Connectivity with Zero Trust](#secure-power-platform-to-azure-paas-connectivity-with-zero-trust)
  - [Code Signing and Property-Level Encryption for Developers](#code-signing-and-property-level-encryption-for-developers)
  - [Other Security News](#other-security-news)

## GitHub Copilot

Extending from last week’s progress, GitHub Copilot adds further AI-based upgrades, new workflow integration, and developer tutorials. New capabilities include additional model options, more ways to customize, better context-awareness through memory and agent-based features, plus collaboration improvements for individual and group coding. You’ll find practical guides and analysis on Copilot’s growing role in meeting business needs, managing feedback, and developing more agent-like coding tools.

### Enhanced AI Models and Model Management in Copilot

After recent attention on multiple models, Copilot now includes GPT-5.2-Codex for all paid tiers, giving access to code suggestions, chat, and agent features in tools like VS Code, GitHub.com, Copilot CLI, and GitHub Mobile. Organization admins enable it through settings, and Pro users can turn it on via prompts. The distribution is rolling out gradually, and Bring Your Own Key (BYOK) allows developers to use their OpenAI API keys in VS Code.

GitHub has officially announced the retirement of older models (including Claude Opus 4.1, GPT-5, and GPT-5-Codex) set for February 17, 2026. These will be replaced by Claude Opus 4.5, GPT-5.2, and GPT-5.1-Codex for chat, code completion, and agent features. Update instructions are included for managing model transitions.

- [GPT-5.2-Codex Now Available in GitHub Copilot](https://github.blog/changelog/2026-01-14-gpt-5-2-codex-is-now-generally-available-in-github-copilot)
- [Upcoming Deprecation of Select GitHub Copilot Models from Claude and OpenAI](https://github.blog/changelog/2026-01-13-upcoming-deprecation-of-select-github-copilot-models-from-claude-and-openai)

### Enterprise-Grade Copilot: Integration, BYOK, and Modernization

Building on recent discussions around workflows and organizational adoption, Copilot’s BYOK function now supports AWS Bedrock, Google AI Studio, and any OpenAI API-compatible provider (including Anthropic and others). Enterprises can define their own context size, use the Responses API for multimodal work, and enable streamed results within the IDE. All of these options are in public preview for Enterprise and Business editions, and are designed for better security, cost flexibility, and fine-tuned performance.

Copilot is now officially integrated with OpenCode, so teams can log in across terminals, desktops, or IDEs using GitHub credentials—streamlining authentication for varied coding environments.

There’s also new support for upgrading older Java EE applications to Jakarta EE using Copilot’s modernization tools, which feature automated code analysis, migration planning, refactoring, and code security checks. Integration with OpenRewrite and plugins for VS Code and IntelliJ IDEA simplify upgrades, handle library dependency changes, and highlight known security issues.

- [Enhancements to GitHub Copilot Bring Your Own Key (BYOK) Capabilities](https://github.blog/changelog/2026-01-15-github-copilot-bring-your-own-key-byok-enhancements)
- [GitHub Copilot Now Officially Supports OpenCode Integration](https://github.blog/changelog/2026-01-16-github-copilot-now-supports-opencode)
- [Modernizing Java EE Applications to Jakarta EE with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-java-ee-applications-to-jakarta-ee-with-github/ba-p/4486471)

### Context Awareness, Agentic Workflows, and Memory Systems

Extending the Agent Skills and playbooks introduced last week, GitHub Copilot Memory is now in public preview for all paid plans, letting models retain repository-specific context for improved coding suggestions and code reviews. Memories are verified, expire automatically after 28 days, and are managed through GitHub settings.

A new agentic memory system offers citation-backed memory objects across code, CLI, and code review agents, recording team standards and choices with code links for tracing and verification. Early tests show improved effectiveness in reviews, developer onboarding, and onboarding of best practices.

Visual Studio now curates Copilot Memories covering standards, rules, and personal preferences, automatically generating reference documentation and helping with consistency. Instruction file support helps new developers learn team practices quickly.

- [Agentic Memory Now in Public Preview for GitHub Copilot](https://github.blog/changelog/2026-01-15-agentic-memory-for-github-copilot-is-in-public-preview)
- [Building an Agentic Memory System for GitHub Copilot](https://github.blog/ai-and-ml/github-copilot/building-an-agentic-memory-system-for-github-copilot/)
- [Copilot Memories: Streamlining Team Coding with Visual Studio](https://devblogs.microsoft.com/visualstudio/copilot-memories/)

### CLI, SDK, and AI Toolkit Updates

The Copilot CLI now offers more model choices, including GPT-5 mini and GPT-4.1, available from the terminal for all subscribers without extra API charges. Recent improvements added automated exploration, planning, and review agents, with new skills and better session organization. Installation is now unified (covering WinGet, Homebrew, Dev Containers, and standalone executables), and command-line scripting and token handling have minimal friction.

The Copilot SDK is now in technical preview for Node.js, Python, Go, and .NET, making it possible to embed Copilot features and agents into CI/CD scripts, IDE plugins, and workflow automation tools.

The latest AI Toolkit for VS Code (version 0.28.1) introduces Copilot Skills for agent programming, tighter integration with Microsoft Foundry, new support for Anthropic models, and better profiling tools. It also includes various improvements in sign-in, the user interface, and performance.

- [GitHub Copilot CLI: Enhanced Agents, Context Management, and Installation Methods](https://github.blog/changelog/2026-01-14-github-copilot-cli-enhanced-agents-context-management-and-new-ways-to-install)
- [Copilot SDK Technical Preview: Multi-Language Access to GitHub Copilot CLI](https://github.blog/changelog/2026-01-14-copilot-sdk-in-technical-preview)
- [AI Toolkit for VS Code: January 2026 Update — Copilot Skills, Foundry Integration, and Dev Enhancements](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-toolkit-for-vs-code-january-2026-update/ba-p/4485205)

### Context Engineering, Collaboration Patterns, and Practical AI Usage

This section continues last week’s look at context engineering for Copilot. There are guides on using custom instructions, prompt files, and agents to get more reliable coding support, including examples for security and documentation automation. Setup instructions include markdown-based context files for VS Code and GitHub workflows.

Another article explores moving from ad-hoc programming (“vibe coding”) to a more structured, spec-driven workflow using Spec-Kit, Copilot, .NET 9, and Blazor. The approach shows how teams can use specifications to guide code review and architecture.

Developers are advised on when to assign repetitive work to Copilot and when to use their own judgment. There are also tutorials for running several agents together in VS Code for linked tasks, linting, and handling tasks in parallel, reflecting more complex, real-world development needs.

- [Want Better AI Outputs? Try Context Engineering with GitHub Copilot](https://github.blog/ai-and-ml/generative-ai/want-better-ai-outputs-try-context-engineering/)
- [From Vibe Coding to Spec-Driven Development: Practical Spec-Kit Workflow](https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development-part2)
- [When to Lead, When to Delegate to GitHub Copilot](https://www.cooknwithcopilot.com/blog/when-to-lead-when-to-delegate-to-github-copilot.html)
- [Orchestrating Multiple AI Agents in VS Code: Insights from Ben & Peng](/github-copilot/videos/orchestrating-multiple-ai-agents-in-vs-code-insights-from-ben-and-peng)

### Real-World Impact and Developer-Centric Analysis

Adding to last week’s coverage of open source and feedback, the latest analysis and the Octoverse 2025 report show that developers use Copilot mainly for transparent, oversight-enabled automation. Most developers value customizable suggestions and in-context options for code, documentation, and refactoring tasks, but expect to stay in control of important architecture decisions. Teams also iterate on product design based on this ongoing feedback.

Octoverse 2025 includes topics like language popularity, agent-driven workflows (“vibe coding”), default security strategies, open source adoption, and renewal of legacy expertise (for instance, COBOL).

- [Inside Octoverse 2025: Vibe Coding, Agentic AI, and Shifting Developer Trends](/github-copilot/videos/inside-octoverse-2025-vibe-coding-agentic-ai-and-shifting-developer-trends)
- [What AI coding tools are actually good for, according to developers](https://github.blog/ai-and-ml/generative-ai/what-ai-is-actually-good-for-according-to-developers/)

### Other GitHub Copilot News

A practical case illustrates Copilot’s role in rapidly building docfind, a client-only search engine for VS Code documentation using Rust and WebAssembly, demonstrating Copilot’s adaptability for different technical problems.

- [Building docfind: Fast Client-Side Search for VS Code Docs with Rust, WASM, and Copilot](https://code.visualstudio.com/blogs/2026/01/15/docfind)

## AI

Microsoft’s AI product ecosystem now offers enhanced integration, compliance controls, and cost management. Developers will find new tools for adoption at scale, stronger data security, cost-sensitive AI solutions, and modernized business workflows. The technical articles cover the ways Microsoft AI solutions meet business standards and compliance requirements—expanding on last week’s stories around modular and context-driven approaches and bringing more Marketplace-based deployment and lifecycle management resources.

### AI Adoption and Custom Agent Development on Microsoft Marketplace

The Microsoft Marketplace plays a main role in deploying AI at scale, bringing together models, code frameworks, and low-code solutions. Developers can now select from over 11,000 AI models and 4,000 agents/apps—including partner models from Anthropic, Cohere, Meta, OpenAI, NVIDIA—or make their own. The marketplace supports filtering on product, category, and business domain, letting users trial and adopt solutions under their Azure contract. Guides include best practices for integration with Azure/Microsoft 365, securing model links using Managed Identity, and tracking policy compliance and lifecycle. There are examples of embedding agents in Microsoft 365 Copilot, plus resources for managing the entire agent lifecycle at enterprise scale.

Compared to earlier coverage, organizations are now moving from trialing agent composition to using managed, secure solutions with compliance in mind.

- [Chart Your AI and Agent Strategy with Microsoft Marketplace](https://azure.microsoft.com/en-us/blog/design-your-ai-and-agent-strategy-with-microsoft-marketplace/)

### Securing and Streamlining Data with Azure AI: PII Redaction and Cost-Effective AI Apps

Azure AI Language PII Redaction provides solutions for protecting sensitive workflow data. Step-by-step guides show how to detect and mask different PII types to meet regulatory standards like GDPR and HIPAA. Video demos explain setup and tuning for use cases in finance, healthcare, and consumer applications.

For teams working on tight budgets, ‘Budget Bytes’ videos explain how to create powerful, Copilot-capable AI apps for less than $25 using Azure SQL Database, with examples including LLM data grounding, custom agent scenarios, RAG, and full stack development. Price tables and reusable code snippets help developers deploy quickly and stay on budget.

These topics build on earlier work around privacy in AI, giving developers ready-to-use options for work that’s both effective and cost-conscious.

- [Protect Sensitive Data with Azure AI Language PII Redaction](/ai/videos/protect-sensitive-data-with-azure-ai-language-pii-redaction)
- [Build Powerful AI Apps for Under $25 with Azure SQL Database](/ai/videos/build-powerful-ai-apps-for-under-25-with-azure-sql-database)

### Azure AI Model Integration: Troubleshooting, Prompt Fidelity, and Custom Workflows

Developers using new models like GPT-4o-mini in Azure AI Foundry have observed inconsistent output between the Playground UI and API calls, especially for classification jobs. The same settings and prompts sometimes produce different responses due to hidden prompts or preprocessing. This difference can affect reliability in deploying conversational agents, prompting teams to troubleshoot and document solutions for consistent behavior.

This ties directly into last week’s agent orchestration theme—underscoring the need for clear communication and transparency in production workflows.

- [Discrepancies Between Azure AI Foundry Playground and API Responses for GPT-4o-mini](https://techcommunity.microsoft.com/t5/azure/weird-problem-when-comparing-the-answers-from-chat-playground/m-p/4486090#M22407)

### Modernizing Industry Workflows: Azure AI in Healthcare Transcription and Analytics

A technical reference explains how healthcare providers can automate speech transcription and analytics with Azure AI. It combines Azure Speech Services for live and batch transcription (including speaker separation) with Azure Text Analytics for Health to extract clinical data. Advanced summarizations are handled by Azure OpenAI, producing FHIR JSON for Microsoft Fabric OneLake—helping with faster, more accurate clinical documentation and HIPAA-compliant data handling. Complete walk-throughs cover pipelines, automation with GitHub Actions, cloud resource tracking, and code samples, letting healthcare IT teams build and expand practical solutions quickly.

This continues the ongoing theme of adapting agent-driven automation for domains like retail logistics to core health information processing.

- [Modernizing Healthcare Transcription and Analytics with Azure AI](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/ai-transcription-text-analytics-for-health/ba-p/4486080)

## ML

This week’s machine learning articles focus on efficient AI-driven workflows for industrial and research teams. There are updates on in-database semantic search, agent frameworks, industrial deployments, and research in the life sciences. Tutorials and case studies provide actionable examples and show practical adoption of advanced ML tools.

### Building AI Workflows with Microsoft Agent Framework and .NET AI Stack

Building on recent themes of local embeddings and agent-based architectures, Pamela Fox’s livestream series demonstrates using the Microsoft Agent Framework in Python, with coverage of RAG agent skills, modular and reproducible AI deployments, monitoring using OpenTelemetry, and orchestration via Magentic. Evaluation with Azure AI SDK rounds out the workflow.

.NET developers can join the AI Community Standup, which now features hands-on sessions using Semantic Kernel, AI Extensions, and orchestration tools—helping the .NET community move beyond chatbot projects to deeper AI integration.

- [Python + Agents: Livestream Series on Building AI Agents with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/join-our-free-livestream-series-on-building-agents-in-python/ba-p/4485731)
- [.NET AI Community Standup: Building AI Apps with the New .NET AI Stack](/ai/videos/net-ai-community-standup-building-ai-apps-with-the-new-net-ai-stack)

### Industrial ML and Scientific Workflows Powered by Azure HPC and Microsoft Discovery

New case studies highlight large-scale machine learning on Azure’s HPC resources, such as Neural Concept’s industrial engineering work with Azure GPUs and storage for AI training in automotive aerodynamics. Benchmarks show efficient model development that parallels what was seen last week in deep learning rollouts.

In drug discovery, Insilico Medicine’s Nach01 model deployed via Microsoft Discovery demonstrates secure, repeatable analytics in the life sciences, drawing on Azure ML’s compliance and deployment features.

- [Neural Concept Sets New Industrial AI Benchmark on Azure HPC for Automotive Aerodynamics](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/scaling-physics-based-digital-twins-neural-concept-on-azure/ba-p/4483403)
- [AI-Native Drug Discovery using Insilico Medicine’s Nach01 Model and Microsoft Discovery](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-native-drug-discovery-using-insilico-medicine-s-nach01-model/ba-p/4484497)

### Expanding Vector Search in Databases: DiskANN in Azure SQL and Fabric SQL

DiskANN now enables large-scale, fast vector search directly inside Azure SQL and Fabric SQL, building on last week’s announcement of local-embedding in the Fabric Eventhouse. This lets teams implement semantic search, classification, and content analysis at the database level for less latency and stronger privacy, without relying on outside APIs.

- [DiskANN: Vector Indexing in Azure SQL and Fabric SQL Explained](/ai/videos/diskann-vector-indexing-in-azure-sql-and-fabric-sql-explained)

## Azure

The Azure platform continues adding modern developer tools, updated security features, cloud storage improvements, and better support for hybrid and analytics workflows. Both developers and DevOps teams can expect new capabilities for daily operations and flexible architecture. New platform updates underline Microsoft’s effort toward automation, compliance, and sustainability with practical details for implementation.

### Azure SQL and Developer Tooling Modernization

Microsoft provided a roadmap for SQL Server, Azure SQL, and the latest developer tools. SQL Server Management Studio (SSMS) will migrate to a new Visual Studio base, now including dark mode, Arm64 builds, Fabric support, and built-in Copilot assistance. The VS Code Microsoft SQL extension includes Copilot Ask, Agent tools, and better schema and container workflows, with a unified Azure SQL experience in the portal and Fabric. VS Code now supports SDK-style SQL projects, coming soon to SSMS, and there are planned improvements across CI/CD, drivers, SDKs, CLIs, and APIs. Community input is encouraged to help prioritize features.

- [How the Microsoft SQL Team is Advancing SQL Tools and Developer Experiences](https://blog.fabric.microsoft.com/en-US/blog/how-the-microsoft-sql-team-is-investing-in-sql-tools-and-experiences/)

### Azure DevOps and Repository Management

Azure Repos has a new preview with productivity, clarity, and organization updates. Teams using TFVC check-in policies should update repositories, and navigation and notification for pull requests have been reinforced. Pull request templates enable better organization with nested branch folder support. Azure DevOps MCP Server provides local tools for repo inspection and bridges between VS Code and Copilot. API improvements will simplify policy scanning, and new features aim to make PR management easier and keep repositories in good shape.

- [Recent and Upcoming Improvements in Azure Repos](https://devblogs.microsoft.com/devops/whats-new-with-azure-repos/)

### Azure Kubernetes Service: Advanced Identity & Networking

AKS rolls out Identity Bindings, providing RBAC-based multi-cluster managed identity with no practical limits on identities, and centralized authorization for simplified, secure cloud operations that suit Infrastructure as Code deployments. Full guides cover integration with Key Vault.

The new Static Egress Gateway feature allows for controlled, multiple outbound IPs on AKS using dedicated node pools and CRDs, supporting both public and private traffic scenarios in compliance-heavy and multi-tenant environments.

- [Identity Bindings: Simplifying Multi-Cluster Managed Identity in AKS](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/identity-bindings-a-cleaner-model-for-multi-cluster-identity-in/ba-p/4478282)
- [Static Egress Gateway in AKS: The Native Way to Control Multiple Outbound IPs](https://techcommunity.microsoft.com/t5/azure-architecture-blog/static-egress-gateway-in-aks-the-native-way-to-control-multiple/ba-p/4484179)

### AI and Analytics with Azure NetApp Files

Azure NetApp Files now supports an object REST API for S3-compatible data workflows, joining its file protocol support for hybrid analytics and advanced AI/HPC use cases. Integration improves with Databricks, Spark, and Fabric OneLake. Security is enforced per bucket with individual certifications.

A new release (v1.1.0) of the VS Code extension includes support for tenants and subscriptions, AI-driven automation, and code generation tools for instant language-specific storage mounts.

- [Unlocking Advanced Data Analytics & AI with Azure NetApp Files Object REST API](https://techcommunity.microsoft.com/t5/azure-architecture-blog/unlocking-advanced-data-analytics-ai-with-azure-netapp-files/ba-p/4486098)
- [What's New with Azure NetApp Files VS Code Extension](https://techcommunity.microsoft.com/t5/azure-architecture-blog/what-s-new-with-azure-netapp-files-vs-code-extension/ba-p/4485989)

### Secure and Flexible Cloud Storage Access

User Delegation SAS (Shared Access Signatures) is now public preview for Azure Tables, Files, and Queues, adding to Blob support. This means SAS tokens can be assigned to a specific Microsoft Entra ID identity, regulated by RBAC. With SDK, PowerShell, and CLI support, it’s now safer and easier to provide time-limited access to both internal and partner services.

- [Public Preview: User Delegation SAS for Azure Tables, Files, and Queues](https://techcommunity.microsoft.com/t5/azure-storage-blog/announcing-public-preview-of-user-delegation-sas-for-azure/ba-p/4485693)

### Integrating REST APIs with AI Agents

Easy MCP is an open source tool that makes REST APIs available to AI agents using Model Context Protocol. OpenAPI-based APIs can be translated for GitHub Copilot and similar tools without modifying original API code. Features include auto-discovery, fast onboarding with the Azure Developer CLI, and workflows for App Service/API Management.

- [Easy MCP: Exposing REST APIs to AI Agents with Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/app-service-easy-mcp-add-ai-agent-capabilities-to-your-existing/ba-p/4484513)

### Automated Hybrid, File Sync, and Messaging Workflows

Azure File Sync integrates with Azure Arc, managed identities, and passwordless authentication, launches in four more regions, and as of January 2026, removes per-server fees for Arc-connected servers with current agents—advancing hybrid file storage solutions.

A practical walkthrough covers automated PostgreSQL deployment on Azure VMs with NetApp Files using Terraform, ARM, and PowerShell. It explains secure networking, NFS, and multiple environment deployment for scenarios like AI/ML and high-availability.

Azure Service Bus Emulator has added Administration Client support, allowing for management of queues and topics through code, and improves reliability for local development and automated testing.

- [Azure File Sync: Azure Arc Integration, Additional Regions, and Secure Syncing](https://techcommunity.microsoft.com/t5/azure-storage-blog/azure-file-sync-azure-arc-integration-additional-regions-and/ba-p/4486050)
- [Deploying Production-Ready PostgreSQL on Azure VMs with Azure NetApp Files Using IaC](https://techcommunity.microsoft.com/t5/azure-architecture-blog/deploy-postgresql-on-azure-vms-with-azure-netapp-files/ba-p/4486114)
- [Introducing Administration Client Support for the Azure Service Bus Emulator](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/introducing-administration-client-support-for-the-azure-service/ba-p/4486433)

### Azure Platform and Service Updates

AKS now supports Ubuntu 24.02 nodes, Cosmos DB can now mirror with Fabric and supports more private access options, and Azure Virtual Desktop introduces regional host pools (preview) for improved availability and compliance. The Azure Arc portal update improves onboarding and handling of multi-cloud environments.

The new Cloud Hardware Emissions Methodology tracks Azure hardware through its lifecycle, supporting teams working on cloud sustainability and reporting.

- [Azure Update - 16th January 2026](/azure/videos/azure-update-16th-january-2026)
- [Azure Virtual Desktop Regional Host Pools Public Preview](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/now-in-public-preview-azure-virtual-desktop-regional-host-pools/ba-p/4474598)
- [Azure Arc Portal Update: Streamlined Onboarding and Management at Scale](https://techcommunity.microsoft.com/t5/azure-migration-and/azure-arc-portal-update-simplifying-onboarding-and-management-at/ba-p/4477355)
- [Advancing Embodied Carbon Measurement at Scale for Microsoft Azure Hardware](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/advancing-embodied-carbon-measurement-at-scale-for-microsoft/ba-p/4485784)

### Other Azure News

Azure’s storage and network engineering now use DCQCN-based congestion control for more than 85% of storage traffic, minimizing latency, CPU load, and reducing packet loss on RDMA workloads. Insights and network best practices are available.

A guide for Microsoft Fabric Eventstream breaks down Capacity Unit costs for ingesting, processing, and output, helping teams estimate and plan for analytics costs.

- [Scaling DCQCN-Based Congestion Control for RDMA in Azure Storage Networks](https://techcommunity.microsoft.com/t5/azure-networking-blog/data-center-quantized-congestion-notification-scaling-congestion/ba-p/4468417)
- [Understanding Fabric Eventstream Pricing](https://blog.fabric.microsoft.com/en-US/blog/understanding-fabric-eventstream-pricing/)

*These enhancements build on previous improvements in Oracle Database@Azure, CAD tool testing, and Data Warehouse upgrades, marking steady steps in Azure SQL tools, integrated repositories, hybrid management, automation, and platform security.*

## Coding

This week’s section features tutorials and maintenance releases supporting cross-platform development and reliable deployments. The articles focus on Android widget development with .NET MAUI and the latest service updates for .NET, helping developers extend, stabilize, and maintain their applications.

### Advanced Android Widget Development with .NET MAUI

You can now create native Android widgets in .NET MAUI using C# (AppWidgetProvider), by following a clear step-by-step guide for project setup, structuring code, and using XML layouts.

The resource outlines how to pass data between your main app and widgets (with Preferences and SharedPreferences), trigger widget actions (like counter buttons) using PendingIntent, and manage configuration steps and Android Context for optimized resource use. Options for automatic or custom widget refreshes—like updatePeriodMillis, AlarmManager, and WorkManager—are explained, with example code for connecting Android platform features to your MAUI app.

This topic extends last week’s coverage of Avalonia UI on Linux for MAUI, as more developers adapt MAUI for broader and more unified use across platforms.

- [How to Build Android Widgets with .NET MAUI](https://devblogs.microsoft.com/dotnet/how-to-build-android-widgets-with-dotnet-maui/)

### .NET Servicing Releases for January 2026

January’s .NET servicing releases are out, delivering non-security patches for .NET 10.0 (10.0.2), 9.0 (9.0.12), and 8.0 (8.0.23), covering the runtime, ASP.NET Core, SDKs, WPF, WinForms, and EF Core. The official changelogs and issue lists are linked for details.

Updates are available as Windows and Linux installers, plus refreshed container images, making it easier to keep environments up to date across hosting scenarios. No changes were released for traditional .NET Framework. Teams should review the changes and update as needed for stability.

These releases connect to last week’s content on the .NET roadmap and developer community status.

- [.NET and .NET Framework January 2026 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-january-2026-servicing-updates/)

## DevOps

DevOps improvements this week introduce new GitHub and .NET features for safer automation, project management, and strong, integrated workflows. Highlights include repository synchronization tools, access management options, workflow visualization, and CI/CD process upgrades.

### GitHub Platform Feature Advancements

To improve project organization and oversight, GitHub has introduced a hierarchy view for Projects, letting teams visualize and sort issues up to eight levels deep. Issue load times are faster, and you can add custom organization properties to tag projects, which supports policy management and helps teams coordinate.

App access rules have become more detailed, letting administrators better define request limits. Consent page warnings for app permissions are also improved for added transparency.

- [Hierarchy View for GitHub Projects Launches in Public Preview](https://github.blog/changelog/2026-01-15-hierarchy-view-now-available-in-github-projects)
- [GitHub Organization Custom Properties: General Availability](https://github.blog/changelog/2026-01-13-organization-custom-properties-now-generally-available)
- [Granular Controls for App Access Requests in GitHub Organizations Now Available](https://github.blog/changelog/2026-01-12-controlling-who-can-request-apps-for-your-organization-is-now-generally-available)
- [Improved Consent Page Warnings for GitHub Apps in Public Preview](https://github.blog/changelog/2026-01-12-selectively-showing-act-on-your-behalf-warning-for-github-apps-is-in-public-preview)

### CI/CD Security and Workflow Controls

GitHub Actions now offers an 'artifact_metadata' permission for more targeted API access, supporting best practices around least-privilege use. Pipeline authors should review workflows for compatibility. Upload rates for Actions caches have strict limits for better pipeline reliability and clear guidance on how to optimize.

- [GitHub Introduces Fine-Grained artifact_metadata Permission for Enhanced API Access Control](https://github.blog/changelog/2026-01-13-new-fine-grained-permission-for-artifact-metadata-is-now-generally-available)
- [Rate limiting for GitHub Actions cache entries](https://github.blog/changelog/2026-01-16-rate-limiting-for-actions-cache-entries)

### Large-Scale Repository and Build Workflow Engineering

.NET’s new Virtual Monorepo Synchronization uses two-way, patch-based syncing for multi-repo setups, allowing automation and continuous delivery across many teams. Details are available for handling metadata and branches.

- [How .NET’s Virtual Monorepo Synchronization Works: Technical Challenges and Solutions](https://devblogs.microsoft.com/dotnet/how-we-synchronize-dotnets-virtual-monorepo/)

### Other DevOps News

December’s GitHub Availability Report details pipeline and infrastructure events, with lessons for monitoring and resilience. Tutorials are also available for developers working with Dev Containers/Codespaces and a practical introduction to managing git branches for newcomers.

- [GitHub Availability Report: December 2025](https://github.blog/news-insights/company-news/github-availability-report-december-2025/)

- [Running AI Coding Agents with Dev Containers and GitHub Codespaces](/ai/videos/running-ai-coding-agents-with-dev-containers-and-github-codespaces)
- [How to Use Git Branches for Beginners](/devops/videos/how-to-use-git-branches-for-beginners)

## Security

This week’s security resources focus on current AI-enabled threats, organizational safeguards, and workflow improvements for developers and IT teams. Articles address criminal infrastructure disruption, Azure platform upgrades, security process automation, CI/CD management, secret handling, code safety, and secured connectivity—all with practical steps for moving to a more resilient defensive posture.

### Criminal VDS Infrastructure and Cybercrime Disruption

Microsoft’s newest threat report describes how the RedVDS service provided cloud-based, anonymous VMs for criminal operations, including email fraud, phishing, and scams. Attackers used tools like Copilot and ChatGPT for local phishing, supported by features like mass mailers and scripting. The report advises administrators to secure M365 tenants, enforce Defender XDR, use multi-factor authentication, and apply DMARC, with linked resources for tracking indicators.

Microsoft and law enforcement have taken down the RedVDS infrastructure and related payment services, disrupting active fraud campaigns. The guidance includes practical steps for detection and ongoing defense.

This update continues last week's focus on M365 targeting and email security, with Defender and authentication controls top priorities.

- [Inside RedVDS: Investigating How a Criminal VDS Provider Empowered Global Cyberattacks](https://www.microsoft.com/en-us/security/blog/2026/01/14/inside-redvds-how-a-single-virtual-desktop-provider-fueled-worldwide-cybercriminal-operations/)
- [Microsoft Disrupts Global RedVDS Cybercrime-as-a-Service Platform Enabling Massive Fraud](https://blogs.microsoft.com/on-the-issues/2026/01/14/microsoft-disrupts-cybercrime/)

### Secure Default Hostnames and Log Immutability in Azure

Azure Functions and Logic Apps now have Secure Unique Default Hostnames (GA), giving randomized hostnames by region and reducing DNS exposure. Teams should update infrastructure scripts and templates for secure out-of-the-box deployment.

Microsoft Fabric defaults to immutable diagnostic logging for OneLake, using WORM features in Blob Storage to meet audit and regulatory needs. Setup and admin procedures are included, but there are cost tradeoffs and some deletion risk remains; overall, this helps enforce compliance in regulated sectors.

These changes build on previous improvements in governance, moving from sensitivity labels to secure, regulated audit trails.

- [Secure Unique Default Hostnames Now GA for Functions and Logic Apps](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/secure-unique-default-hostnames-now-ga-for-functions-and-logic/ba-p/4484237)
- [Gain Even More Trust and Compliance with OneLake Diagnostics Immutability](https://blog.fabric.microsoft.com/en-US/blog/gain-even-more-trust-and-compliance-with-onelake-diagnostics-immutability-generally-available/)

### Security Automation and Developer Workflows in Open Source

The new open source GitHub Security Lab Taskflow Agent framework launches this week, providing an agent-based toolkit for vulnerability research and code auditing. It integrates with CodeQL, runs modular YAML “taskflows,” and works in Codespaces, Docker, or local Python. Developers can extend or contribute workflows for bug variant analysis and reproducible testing.

The push toward agent-driven automation matches this week’s stories on AI and Copilot, showing real-world solutions for open source security and vulnerability discovery.

- [Community-powered Security with AI: Launching the GitHub Security Lab Taskflow Agent](https://github.blog/security/community-powered-security-with-ai-an-open-source-framework-for-security-research/)

### Secret Scanning and Platform Rule Management for GitHub

From February 2026, GitHub secret scanning will provide extra metadata about detected credentials in eligible repositories, making it easier to address and fix exposures. GitHub’s infrastructure team also reviews platform-wide defense systems, including emergency rate limits and automated rule expiration, for ongoing hygiene and platform safety.

This continues recent updates to GitHub’s security and workflow automation, supporting easier incident response and continuous improvement.

- [GitHub Secret Scanning: Automatic Extended Metadata Checks for Security](https://github.blog/changelog/2026-01-15-secret-scanning-extended-metadata-to-be-automatically-enabled-for-certain-repositories)
- [When Protections Outlive Their Purpose: Managing Defense Systems at Scale on GitHub](https://github.blog/engineering/infrastructure/when-protections-outlive-their-purpose-a-lesson-on-managing-defense-systems-at-scale/)

### AI Coding Agents and Application Security

A technical review finds that AI coding agents sometimes introduce security vulnerabilities such as weak authentication, faulty validation, or dangerous functions. The article recommends thorough code review by developers, regardless of agent use, and a focus on workflow discipline.

This finding connects to the theme of agentic AI providing support, but not replacing, expert oversight in secure development.

- [Vibe Coded Applications Full of Security Blunders](https://devclass.com/2026/01/15/vibe-coded-applications-full-of-security-blunders/)

### Secure Power Platform to Azure PaaS Connectivity with Zero Trust

A new guide walks through setting up zero trust connections from Power Platform to Azure PaaS. It uses VNet injection, firewalls, private endpoints, and peering along with RBAC and managed identity (no secrets). The design includes high availability, user-controlled keys, and automated setup with CLI and PowerShell, with code repositories provided.

This contributes to ongoing best practices for integration and layered defense in Azure environments.

- [Cross-Region Zero Trust: Secure Power Platform Connectivity to Azure PaaS Without Public Exposure](https://techcommunity.microsoft.com/t5/azure-architecture-blog/cross-region-zero-trust-connecting-power-platform-to-azure-paas/ba-p/4484995)

### Code Signing and Property-Level Encryption for Developers

Azure Artifact Signing (AAS) reaches general availability, improving code signing for Windows apps using renewable certificates and simple integration in CI/CD pipelines. The feature isn’t regional or macOS-ready but helps with compliance and key management.

For .NET 8, a walkthrough explains encrypting object properties (like OAuth tokens) on serialization with custom attributes and System.Text.Json’s TypeInfoResolver, with Azure Key Vault integration in development. This level of field-specific data protection supports compliance and privacy rules.

Both topics follow last week’s advances in serialization and privacy-focused developer features.

- [Code Signing Windows Apps Easier and More Secure with Azure Artifact Signing](https://devclass.com/2026/01/14/code-signing-windows-apps-may-be-easier-and-more-secure-with-new-azure-artifact-service/)
- [Encrypting Properties with System.Text.Json and a TypeInfoResolver Modifier (Part 1)](https://www.stevejgordon.co.uk/encrypting-properties-with-system-text-json-and-a-typeinforesolver-modifier-part-1)

### Other Security News

A guide for Microsoft 365 admins covers external sharing controls across SharePoint, OneDrive, Teams, and Entra ID. Topics include settings management, MFA, conditional access, Access Reviews, auditing, and user education for safer collaboration.

These play a role in reducing risk for email and document sharing as discussed previously.

- [Managing External Sharing in Microsoft 365 Without Chaos](https://dellenny.com/managing-external-sharing-in-microsoft-365-without-chaos/)

A session on quantum cryptography covers the risks posed by quantum computing for established encryption methods and explains how to move to quantum-safe protocols, such as SymCrypt. Linked training resources are provided.

This connects to earlier topics covering encryption and the shift toward future-ready standards.

- [What Quantum Safe Is and Why We Need It to Stay Secure](/ai/videos/what-quantum-safe-is-and-why-we-need-it-to-stay-secure)
