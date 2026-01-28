---
title: AI Agent Workflows, Security Improvements, and Advances in Cloud Technology
author: Tech Hub Team
date: 2025-09-22 09:00:00 +00:00
tags:
- AI Agents
- Application Modernization
- Automation
- Cloud Migration
- Developer Tools
- Kubernetes
- Large Language Models
- MCP (model Context Protocol)
- Microsoft Fabric
- Quantum Computing
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
Welcome to this week’s news overview on technology, featuring developments in AI, secure cloud infrastructure, and productivity for developers. Organizations are now using AI agents in tools like GitHub Copilot, Azure AI Foundry, and OpenAI’s GPT-5-Codex to automate more processes and enable real-time workflow improvements. From conversational code reviews to practical LLM integration guides, this update highlights progress in model management, toolchain coordination, and agent-based delivery across business sectors.

Security remained a top priority, with GitHub’s implementation of post-quantum SSH protocol and new supply chain protections responding to advanced attacks targeting developer environments. In parallel, improvements in Azure Kubernetes, Microsoft Fabric, and cloud migration tools are helping teams strengthen security, scale AI intelligence, and streamline daily operations. Additional updates on educational programs and quantum computing research emphasize the sector’s focus on accessibility and research. Read further for the detailed stories shaping current technology trends.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot Coding Agent and Team Integration](#github-copilot-coding-agent-and-team-integration)
  - [GitHub MCP Registry and Agentic Ecosystem](#github-mcp-registry-and-agentic-ecosystem)
  - [Expanding Model Choice, Context Management, and Privacy](#expanding-model-choice-context-management-and-privacy)
  - [GitHub Copilot with Azure AI Foundry and Enterprise Model Integration](#github-copilot-with-azure-ai-foundry-and-enterprise-model-integration)
  - [Improving Code Quality, Refactoring, and Application Modernization](#improving-code-quality-refactoring-and-application-modernization)
  - [GitHub Copilot in IDEs and Developer Experience Enhancements](#github-copilot-in-ides-and-developer-experience-enhancements)
  - [Guardrails, Workflow Governance, and Enterprise Standards](#guardrails-workflow-governance-and-enterprise-standards)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [GPT-5-Codex and Enterprise AI Development](#gpt-5-codex-and-enterprise-ai-development)
  - [Azure AI Foundry: Agents, Orchestration, and Security](#azure-ai-foundry-agents-orchestration-and-security)
  - [Model Context Protocol (MCP) in Microsoft’s AI Stack](#model-context-protocol-mcp-in-microsofts-ai-stack)
  - [Azure Agentic AI Solution Architecture and Best Practices](#azure-agentic-ai-solution-architecture-and-best-practices)
  - [Agentic AI for Platform Engineering and Infrastructure](#agentic-ai-for-platform-engineering-and-infrastructure)
  - [Microsoft Fabric: Real-Time Intelligence and Developer Resources](#microsoft-fabric-real-time-intelligence-and-developer-resources)
  - [Industry-Specific AI Workflows & Communication Automation](#industry-specific-ai-workflows--communication-automation)
  - [Microsoft Copilot Studio and Foundry Local Expansions](#microsoft-copilot-studio-and-foundry-local-expansions)
  - [Advances in AI Search, Indexing, and Knowledge Grounding](#advances-in-ai-search-indexing-and-knowledge-grounding)
  - [Multi-Agent AI Solutions and Collaborative Microsoft Workflows](#multi-agent-ai-solutions-and-collaborative-microsoft-workflows)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Community-Driven AI Learning for Data Science and ML](#community-driven-ai-learning-for-data-science-and-ml)
  - [Microsoft Quantum Computing Research Expansion](#microsoft-quantum-computing-research-expansion)
- [Azure](#azure)
  - [Microsoft Fabric Ecosystem: Data, AI, and ISV Innovations](#microsoft-fabric-ecosystem-data-ai-and-isv-innovations)
  - [Azure Kubernetes Service & Container Workflows: Automation, Security, and Performance](#azure-kubernetes-service--container-workflows-automation-security-and-performance)
  - [Azure Platform Infrastructure: Confidential Computing, Observability, and Networking](#azure-platform-infrastructure-confidential-computing-observability-and-networking)
  - [Azure Cloud Migration, Modernization, and Hybrid Management](#azure-cloud-migration-modernization-and-hybrid-management)
  - [Azure Platform Foundation: Essential Updates and Emerging Developer Tools](#azure-platform-foundation-essential-updates-and-emerging-developer-tools)
- [Coding](#coding)
  - [.NET 10 Tool Packaging, WebAssembly, and Support Lifecycle Changes](#net-10-tool-packaging-webassembly-and-support-lifecycle-changes)
- [DevOps](#devops)
  - [GitHub Actions and Dependabot Enhancements](#github-actions-and-dependabot-enhancements)
  - [Platform Deprecations and Migration Guidance](#platform-deprecations-and-migration-guidance)
  - [AI-Powered Automation: Code Review and Infrastructure](#ai-powered-automation-code-review-and-infrastructure)
  - [Workflow Automation Trends: AI Agents and GitOps for Next-Gen DevOps](#workflow-automation-trends-ai-agents-and-gitops-for-next-gen-devops)
  - [DevOps Security, Platform Resilience, and Industry Analysis](#devops-security-platform-resilience-and-industry-analysis)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Malicious Extension Threats in Developer Ecosystems](#malicious-extension-threats-in-developer-ecosystems)
  - [Enterprise Platform Security: GitHub’s New Controls](#enterprise-platform-security-githubs-new-controls)
  - [Preparing for the Quantum Era: Post-Quantum Secure SSH on GitHub](#preparing-for-the-quantum-era-post-quantum-secure-ssh-on-github)
  - [Securing the Software Supply Chain and Open Source Dependencies](#securing-the-software-supply-chain-and-open-source-dependencies)
  - [Other Security News](#other-security-news)

## GitHub Copilot

Recent GitHub Copilot updates reinforce the ecosystem’s growth in enterprise integrations, broader MCP standard usage, increased model options, and better context and privacy features. Integrations with Teams, Azure DevOps, Visual Studio Code, and third-party services highlight practical automation and a more effective developer experience.

### GitHub Copilot Coding Agent and Team Integration

The new public preview for Copilot allows Azure Boards work items to be assigned to Copilot’s coding agent, linking Azure DevOps and GitHub for automated handling of issues, bug fixes, and feature delivery. This builds on earlier agentic workflows, focusing on safe code changes with continuous integration.

The GitHub app for Microsoft Teams enables conversational pull request automation directly within chat, expanding Copilot’s reach across platforms. These features allow organizations to use Copilot’s agent capabilities throughout their toolchains, with administration supported by refined workflow controls—a natural progression from previous enterprise management features.

Guides illustrate Copilot agent flexibility with Playwright MCP, Notion, Hugging Face, and IDE features like the Agents panel. The focus is now on workflow optimization and practical automation rather than just feature announcements.

- [Assign Azure Boards Work Items to GitHub Copilot Coding Agent in Public Preview](https://github.blog/changelog/2025-09-18-assign-azure-boards-work-items-to-copilot-coding-agent-in-public-preview)
- [Azure Boards Integration with GitHub Copilot: Private Preview Announced](https://devblogs.microsoft.com/devops/azure-boards-integration-with-github-copilot-private-preview/)
- [Using GitHub Copilot Coding Agent with Microsoft Teams for Automated PRs](https://github.blog/changelog/2025-09-19-work-with-copilot-coding-agent-in-microsoft-teams)
- [AI-Powered GitHub App for Teams Now in Public Preview](https://devblogs.microsoft.com/microsoft365dev/copilot-powered-github-app-for-teams-preview/)
- [5 Powerful Ways to Integrate GitHub Copilot Coding Agent into Your Workflow](https://github.blog/ai-and-ml/github-copilot/5-ways-to-integrate-github-copilot-coding-agent-into-your-workflow/)

### GitHub MCP Registry and Agentic Ecosystem

The GitHub MCP Registry further extends last week’s MCP server and registry controls, providing a unified hub for discovering agents, servers, and partner extensions. Features such as allowlists and registry setup now combine within a secure, centralized registry.

This registry supports a variety of IDEs and adopts an open-source model, continuing recent community engagement. Tutorials highlight MCP prompt extensions, code search for Azure DevOps, and streamlined MCP-powered IDE sessions, all aimed at simplifying deployment and management.

- [GitHub MCP Registry Launches as Central Hub for AI Development Tools](https://devops.com/github-mcp-registry-launches-as-central-hub-for-ai-development-tools/)
- [Meet the GitHub MCP Registry: The Fastest Way to Discover MCP Servers](https://github.blog/ai-and-ml/github-copilot/meet-the-github-mcp-registry-the-fastest-way-to-discover-mcp-servers/)
- [GitHub MCP Registry: Centralizing AI Agent Tool Discovery](https://github.blog/changelog/2025-09-16-github-mcp-registry-the-fastest-way-to-discover-ai-tools)
- [Search Less, Build More: Inner Sourcing with GitHub Copilot and Azure DevOps MCP Server](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/search-less-build-more-inner-sourcing-with-github-copilot-and/ba-p/4454560)
- [Enhance Your Copilot Experience in Visual Studio with MCP Prompts, Resources, and Sampling](https://devblogs.microsoft.com/visualstudio/mcp-prompts-resources-sampling/)

### Expanding Model Choice, Context Management, and Privacy

Copilot’s model system now features automatic selection for Copilot Chat, improving on previous manual options and BYOK functionality. It supports automated, context-sensitive switching for both paid and free users, resulting in improved responsiveness and quota use.

Claude Opus 4.1 is now available for enterprise and Pro+ users in leading IDEs, and Foundry Local integration highlights privacy features, following the move to more context-driven deployments. Guides focus on practical benefits like choosing suitable models and running private, on-device inference for sensitive projects.

- [Auto Model Selection Preview for GitHub Copilot Chat in VS Code](https://code.visualstudio.com/blogs/2025/09/15/autoModelSelection)
- [Auto Model Selection for GitHub Copilot in VS Code Public Preview](https://github.blog/changelog/2025-09-14-auto-model-selection-for-copilot-in-vs-code-in-public-preview)
- [Claude Opus 4.1 Rolls Out for GitHub Copilot Users in Popular IDEs](https://github.blog/changelog/2025-09-16-anthropic-claude-opus-4-1-is-now-available-in-public-preview-in-visual-studio-jetbrains-xcode-and-eclipse)
- [Picking the Right AI Model for Your Task in GitHub Copilot](https://cooknwithcopilot.com/blog/picking-the-right-ai-model-for-your-task.html)
- [Integrating Foundry Local with GitHub Copilot in Visual Studio Code](https://devblogs.microsoft.com/foundry/ai-assisted-development-powered-by-local-models/)

### GitHub Copilot with Azure AI Foundry and Enterprise Model Integration

This week’s tutorials show Copilot connecting to Azure AI Foundry models with the AI Toolkit in Visual Studio Code, continuing last week’s MCP-powered workflow coverage. Guides cover steps from set-up to using custom, compliance-oriented large language models in local and cloud environments.

Instructions include authentication, extension configuration, and enterprise deployment, illustrating a shift from general news toward step-by-step deployments.

- [Integrating Azure AI Foundry Models with GitHub Copilot via AI Toolkit](/ai/videos/Integrating-Azure-AI-Foundry-Models-with-GitHub-Copilot-via-AI-Toolkit)
- [Integrating Azure AI Foundry Models with GitHub Copilot Using the AI Toolkit](/ai/videos/Integrating-Azure-AI-Foundry-Models-with-GitHub-Copilot-Using-the-AI-Toolkit)

### Improving Code Quality, Refactoring, and Application Modernization

Application Insights for .NET now features Copilot agent integration for automated performance analysis, following last week’s prompt-based code review and modernization efforts. Tutorials provide practical examples of Copilot instructions, prompt files, and refactoring with VS Code, extending previous resources on prompt engineering and moving toward full codebase modernization.

- [Application Insights Code Optimizations: AI-Driven Performance Tuning for .NET Apps](https://devblogs.microsoft.com/dotnet/application-insights-code-optimizations/)
- [Refactor an Existing Codebase Using Prompt-Driven Development with GitHub Copilot](/ai/videos/Refactor-an-Existing-Codebase-Using-Prompt-Driven-Development-with-GitHub-Copilot)
- [Understanding Instruction and Prompt Files for GitHub Copilot in .NET Development](https://devblogs.microsoft.com/dotnet/prompt-files-and-instructions-files-explained/)
- [Unlocking Application Modernisation with GitHub Copilot](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unlocking-application-modernisation-with-github-copilot/ba/p/4454121)

### GitHub Copilot in IDEs and Developer Experience Enhancements

Agent-based code review and workflow features for Copilot now reach JetBrains and Visual Studio IDEs, carrying forward last week’s progress. Tutorials cover advanced Copilot Chat for full-stack debugging, custom chat linked to MCP servers, and switching between Agent and Ask modes for dynamic code edits.

Spec Kit tools connect development standards to code delivery for improved collaboration—directly evolving earlier workflow standardization efforts. VS Code Dev Days continues with installation tutorials, extension building, and live demonstrations, providing hands-on training in Copilot features.

- [Copilot Code Review Now Available in JetBrains IDEs and Visual Studio](https://github.blog/changelog/2025-09-18-copilot-code-review-now-in-jetbrains-ides-and-visual-studio)
- [VS Code Dev Days: Unlocking AI-Powered Coding with GitHub Copilot](/ai/videos/VS-Code-Dev-Days-Unlocking-AI-Powered-Coding-with-GitHub-Copilot)
- [Debugging a Full-Stack Chat App with GitHub Copilot Chat in VS Code](/ai/videos/Debugging-a-Full-Stack-Chat-App-with-GitHub-Copilot-Chat-in-VS-Code)
- [Spec Kit and GitHub Copilot: Spec-Driven Development in VS Code with Den](/ai/videos/Spec-Kit-and-GitHub-Copilot-Spec-Driven-Development-in-VS-Code-with-Den)
- [Spec-Driven Development with GitHub Spec Kit: Streamlining AI-Assisted Coding Workflows](https://devblogs.microsoft.com/blog/spec-driven-development-spec-kit)
- [VS Code Live: Enhancing Presentations and Live Coding with Demo Time](/ai/videos/VS-Code-Live-Enhancing-Presentations-and-Live-Coding-with-Demo-Time)

### Guardrails, Workflow Governance, and Enterprise Standards

Enterprise guidance expands to workflow governance and AGENTS.md support, sharing practical advice for code review guardrails, versioned instruction files, and project knowledge curation. The trend moves from building basic configurations to defining organization-wide standards and syncing documentation.

- [Disciplined Guardrail Development in Enterprise Applications with GitHub Copilot](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/disciplined-guardrail-development-in-enterprise-application-with/ba/p/4455321)

### Other GitHub Copilot News

GitHub updated Copilot premium request budgets for enterprise and team accounts, improving last week’s quota management for SKUs and giving more admin control. GitHub Sparks updates help teams collaborate securely on read-only workflows with managed data sharing.

New guides explain multi-turn conversational workflows in Copilot Studio, expanding last week’s resources for prompt and context management in large-scale assistant design.

- [Removal of {{TITLE}} Copilot Premium Request Budgets for Enterprise and Team Accounts](https://github.blog/changelog/2025-09-17-upcoming-removal-of-copilot-premium-request-0-budgets-for-enterprise-and-team-accounts)
- [Share Read-Only Sparks with Controlled Data Access](https://github.blog/changelog/2025-09-17-share-read-only-sparks-with-controlled-data-access)
- [Multi-Turn Conversations and Context Management in Copilot Studio](https://dellenny.com/multi-turn-conversations-and-context-management-in-copilot-studio/)

## AI

Recent AI updates highlight broad integration of advanced language models, maturing agent frameworks, improved developer tools, and practical guides for secure, automated workflows. OpenAI’s GPT-5-Codex is enterprise-ready, while Microsoft and its partners continue to expand Azure AI Foundry agent capabilities, security features, and tooling. MCP support grows, with more resources for multi-agent and scalable autonomous operations across industries.

### GPT-5-Codex and Enterprise AI Development

GPT-5-Codex is now available for enterprise software engineering beyond basic code generation, tackling deeper refactoring, debugging, and code review at scale. It allocates resources depending on workflow complexity, providing context-aware review, dependency checks, test management, and security feedback.

Codex works with VS Code, Cursor, CLI tools, APIs, and browser automation, retaining context across on-premises and cloud environments. It automates dependency checks and improves performance using container caching. Security measures include sandboxing, audit logs, custom controls, and protection against prompt injection.

Real-world cases, such as Cisco Meraki’s modernization, show Codex reducing manual review tasks and helping teams refocus on strategic work. These recent capabilities complement ongoing BYOK and model selection in Copilot, contributing to the wider adoption of context-driven coding assistants.

- [OpenAI’s GPT-5-Codex: Enterprise AI for Smarter Software Development](https://devops.com/openais-gpt-5-codex-a-smarter-approach-to-enterprise-development/)
- [OpenAI’s GPT-5-Codex: AI for Enterprise-Grade Development and Code Review](https://devops.com/openais-gpt-5-codex-a-smarter-approach-to-enterprise-development/?utm_source=rss&utm_medium=rss&utm_campaign=openais-gpt-5-codex-a-smarter-approach-to-enterprise-development)

### Azure AI Foundry: Agents, Orchestration, and Security

Azure AI Foundry has introduced the Computer Use Tool (preview) for agents to automate desktop and web interfaces using REST APIs or SDKs—even where native APIs are missing. It supports pixel-based reasoning, guardrails involving human review, risk monitoring, and sandboxed deployments.

Security updates feature Entra Agent IDs for lifecycle management, Purview-provided DLP, prompt injection defense, adversarial testing, and agent telemetry linked to Defender XDR for live incident monitoring.

A new engineering guide covers the design of multi-agent systems, integration of dynamic MCP tools, prompt best practices, RBAC, and approaches to scale. Azure AI Foundry now offers a complete platform for compliant agent development.

These updates build on previous Agent Factory and MCP standards coverage, showing the transition from reference architectures to preview features and governance models.

- [Announcing Computer Use Tool (Preview) in Azure AI Foundry Agent Service](https://devblogs.microsoft.com/foundry/announcing-computer-use-tool-preview-in-azure-ai-foundry-agent-service/)
- [Agent Factory: Blueprint for Safe and Secure Enterprise AI Agents Using Azure AI Foundry](https://azure.microsoft.com/en-us/blog/agent-factory-creating-a-blueprint-for-safe-and-secure-ai-agents/)
- [Building Multi-Agent AI Systems with Azure AI Foundry: Engineering, Orchestration, and Best Practices](https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-multi-agent-ai-systems-with-microsoft/ba/p/4454510)

### Model Context Protocol (MCP) in Microsoft’s AI Stack

Microsoft’s wide MCP standard adoption supports tool consistency and system integration. MCP offers a schema for agents, tools, and memory with support for HTTP, SSE, and WebSocket protocols. Developers benefit from cross-platform usage in Copilot Studio, Azure AI Foundry, and Dynamics 365, with SDKs for C# and Semantic Kernel integration.

Guides include MCP server setup, Dynamics 365/Dataverse deployments, and practical agent integration for regulated business environments, underlining Microsoft’s commitment to open frameworks and reusable tooling.

Recent MCP guides expand on earlier agent factory progress, providing more practical resources for interoperable workflows.

- [How MCP Works in Microsoft’s AI Ecosystem](https://dellenny.com/how-mcp-works-in-microsofts-ai-ecosystem/)
- [Unlocking MCP Server: AI Integration for Dataverse & Dynamics 365](/ai/videos/Unlocking-MCP-Server-AI-Integration-for-Dataverse-and-Dynamics-365)

### Azure Agentic AI Solution Architecture and Best Practices

Developers receive updated migration advice for shifting from Azure OpenAI Assistants API (now deprecated) to Azure AI Agent Service, connecting to Azure AI Search, Fabric, containers, and SDKs for Python, C#, and Java. No-code automation with Logic Apps is featured for human-in-the-loop processes.

The resource includes open-source orchestrators, hosting choices for AKS/App Service, and security tips for agent orchestration deployment.

This extends last week’s focus on Logic Apps and Python/MCP agent previews, showing Azure’s movement to unified migration and orchestration strategies.

- [Selecting the Right Agentic Solution on Azure: A Guide to AI Agents and Orchestration](https://techcommunity.microsoft.com/t5/azure-architecture-blog/selecting-the-right-agentic-solution-on-azure/ba/p/4453955)

### Agentic AI for Platform Engineering and Infrastructure

Pulumi Neo now previews autonomous AI agents for Infrastructure-as-Code, managing diagnostics, compliance, policy automation, approvals, and audit logs, with MCP support for multi-tool workflows and recommendation context. Teams can reverse unsafe changes with improved traceability.

This continues the evolution from developer tools to advanced infrastructure automation, bridging AI agents, platform engineering, and DevOps.

- [Pulumi Unveils AI Agents for Autonomous Infrastructure Automation](https://devops.com/pulumi-previews-ai-agents-trained-to-automate-infrastructure-management/)

### Microsoft Fabric: Real-Time Intelligence and Developer Resources

Microsoft Fabric extends AI-driven event analytics and dashboarding with streaming tools (Eventstream/Eventhouse), reusable KQL queries, geospatial and graph analytics, Copilot NLP, Activator automation, and Digital Twin Builder. Additional monitoring, security, and connector features widen industry applicability.

There’s an announcement for a global AI/data hackathon including workshops and team challenges.

These updates continue last week’s event-driven agentic enhancements and integrations across pro-code and low-code environments.

- [AI-Driven Operations with Microsoft Fabric Real-Time Intelligence](https://blog.fabric.microsoft.com/en-US/blog/the-foundation-for-powering-ai-driven-operations-fabric-real-time-intelligence/)
- [Hack the Future of Data with Microsoft Fabric: Global AI & Data Hackathon](https://blog.fabric.microsoft.com/en-US/blog/announcement-hack-the-future-of-data-with-microsoft-fabric/)

### Industry-Specific AI Workflows & Communication Automation

A four-tier framework shows automation setups using Azure Communication Services and Copilot Studio for domains like healthcare, finance, recruiting, and retail. Technical guides share step-by-step instructions for HIPAA notifications, financial services onboarding, and omnichannel bots with secure integration.

A case study describes Copilot Studio bots reducing support tickets by 40% and increasing CSAT by 25%, with advice on flow design, prompt engineering, and API connections.

Building on last week’s templates and best practices, these resources offer direct methods for scaling communication automation.

- [How AI and Communication APIs Are Transforming Industry Workflows](https://techcommunity.microsoft.com/t5/azure-communication-services/how-ai-communication-apis-are-transforming-work-across/ba/p/4454224)
- [Case Study: Reducing Support Ticket Volume Using AI Bots Built in Copilot Studio](https://dellenny.com/case-study-reducing-support-ticket-volume-using-ai-bots-built-in-copilot-studio/)

### Microsoft Copilot Studio and Foundry Local Expansions

Copilot Studio’s computer use feature enters US public preview, enabling desktop and web automation even for systems lacking APIs. A hosted browser, templates, credential tools, and controlled allow-listing extend flexibility. Power Automate integration supports no-code scripting using natural language and UI interaction.

Upcoming technical AMAs cover Foundry Local’s on-device LLM customization and offline inference, supporting privacy and hybrid deployment.

These updates build on last week’s guides, expanding tool diversity for developers working with low-code and privacy-preserving workflows.

- [Computer Use Public Preview Launches in Microsoft Copilot Studio](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/computer-use-is-now-in-public-preview-in-microsoft-copilot-studio/)
- [Technical AMA: Foundry Local and On-Device LLMs with Azure AI Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/join-us-for-a-technical-deep-dive-and-q-a-on-foundry-local-llms/ba/p/4455060)

### Advances in AI Search, Indexing, and Knowledge Grounding

Developers can now create vector indexes from Azure storage in Azure AI Foundry, using Azure AI Search for both keyword and vector queries, RBAC, and network isolation. This supports faster Retrieval Augmented Generation (RAG) solution prototyping and agent deployment.

The Azure Essentials Show outlines improvements to RAG apps with better access control and quick deployment.

This builds on last week’s progress in agent-centric data integration, improving productivity.

- [Ground Your Agents Faster with Native Azure AI Search Indexing in Foundry](https://devblogs.microsoft.com/foundry/ground-your-agents-faster-native-azure-ai-search-indexing-foundry/)
- [Build Smarter Agents with Azure AI Search](/ai/videos/Build-Smarter-Agents-with-Azure-AI-Search)

### Multi-Agent AI Solutions and Collaborative Microsoft Workflows

Guides show how Microsoft Fabric Data Agent, Azure AI Foundry, and Copilot Studio combine for full multi-agent solutions, supporting data synthesis, Lakehouse configuration, agent-centric workflow, and conversational setups in Teams. Documentation and workshops promote these collaborative patterns.

This continues last week’s practical tutorials for multi-agent deployment.

- [Building Multi-Agent Solutions with Microsoft Fabric Data Agent and Azure AI Foundry](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/explore-microsoft-fabric-data-agent-azure-ai-foundry-for-agentic/ba/p/4453709)

### Other AI News

Recent analysis covers general AI agents and how hallucinations emerge. “Sip and Sync with Azure” offers demonstrations of workflows, Warp CLI, and external protocol advice for agent-driven solution development.

A GitHub video explains AI model hallucinations, focusing on training data coverage and incentives, and offers suggestions for more reliable output—especially useful for LLM API developers and chatbot authors.

These resources complement last week’s coverage on developer education and practical risk management.

- [Building for General Purpose AI Agents | Sip and Sync with Azure](/ai/videos/Building-for-General-Purpose-AI-Agents-Sip-and-Sync-with-Azure)
- [The Real Reason AI Models Hallucinate](/ai/videos/The-Real-Reason-AI-Models-Hallucinate)

## ML

Updates in machine learning center on making advanced AI and quantum computing more accessible, with new resources for beginners and ongoing research projects. Initiatives aim to build practical skills and support foundational research across various ML fields.

### Community-Driven AI Learning for Data Science and ML

Building on last week’s transparency in benchmarking, the focus now is on accessible ML learning for newcomers, featuring Discord sessions using Microsoft’s Data Science and ML for Beginners path. Participants take part in activities using Copilot, Python, Jupyter, and VS Code Data Wrangler, integrating basic knowledge into AI projects.

Live office hours and collaborative peer groups encourage knowledge exchange, matching last week’s benchmarking theme. Prompt cards, notebooks, and hands-on practice now extend to more early-career users, broadening ML engagement.

- [Practical Ways to Use AI in Your Data Science and ML Journey](https://techcommunity.microsoft.com/t5/microsoft-developer-community/practical-ways-to-use-ai-in-your-data-science-and-ml-journey/ba/p/4454764)

### Microsoft Quantum Computing Research Expansion

Following last week’s Azure ND GB200 v6 hardware benchmarking for ML, Microsoft started a quantum research partnership with the University of Maryland, covering hardware/software co-design, benchmarking standards, and error correction. The Microsoft Quantum platform targets reproducible validation and bridging public-private research, reflecting previous ML workflow improvements.

This collaboration paves the way for new programming standards and validation models, continuing the drive for transparent benchmarking from last week.

- [Microsoft Opens Quantum Research Center with Maryland Partnership](https://blogs.microsoft.com/on-the-issues/2025/09/17/our-new-collaboration-with-maryland-will-accelerate-scalable-quantum-computing/)

## Azure

Azure’s platform continues to improve with updates in automation, developer productivity, data storage, and AI, including secure Kubernetes, scalable cloud tools, and observability. Microsoft Fabric, AKS, and infrastructure platforms show ongoing progress.

### Microsoft Fabric Ecosystem: Data, AI, and ISV Innovations

Fabric’s September summary highlights advancements in unified data/AI engineering and analytics. The open-source release of Fabric CLI continues automation efforts, while enhanced Terraform and VS Code integrations support better tool management.

Python workflow upgrades (GA for functions, notebook improvements) boost access to data engineering features. Governance through Purview and new APIs adds management options.

AI/ML functionalities, including wrangling and Copilot, improve productivity. Data Factory, Dataflow Gen2, and pipeline features now offer better scalability, echoing previous enterprise improvements.

Security updates such as Private Link and workspace identities advance Azure’s compliance goals. Fabric Data Warehouse scales SQL/data workloads with AI migration support.

ISV updates at FabCon Europe extend analytics, Spark orchestration, OneLake, and graph/geospatial features, rounding out last week’s integration coverage.

- [Microsoft Fabric September 2025 Feature Summary: Data, AI, Engineering and Governance Enhancements](https://blog.fabric.microsoft.com/en-US/blog/september-2025-fabric-feature-summary/)
- [Unifying Data Estates with Microsoft Fabric Data Factory: AI, Integration, and Innovation](https://blog.fabric.microsoft.com/en-US/blog/unify-your-data-estate-for-the-era-of-ai-with-fabric-data-factory/)
- [Introducing Fabric Data Warehouse: Next-Generation Cloud Data Warehousing](https://blog.fabric.microsoft.com/en-US/blog/welcome-to-fabric-data-warehouse/)
- [The Power of ISVs: Unleashing Innovation in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/the-power-of-isvs-unleashing-innovation-in-microsoft-fabric/)

### Azure Kubernetes Service & Container Workflows: Automation, Security, and Performance

AKS Automatic is now generally available, following last week’s work on container resilience and zone redundancy. It automates cluster operations, scaling, and security, and uses Azure Linux for compliance and vulnerability management.

Azure Container Storage v2.0.0 offers faster persistent storage for AI/ML and databases, builds on previous previews, and stays open-source and free for developers.

WireGuard in AKS preview adds built-in pod encryption using Cilium, extending earlier work in VM and network isolation.

- [AKS Automatic with Azure Linux: Streamlining Kubernetes Operations](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/aks-automatic-with-azure-linux/ba/p/4454284)
- [Announcing the General Availability of Azure Kubernetes Service (AKS) Automatic](https://azure.microsoft.com/en-us/blog/azure-kubernetes-service-automatic-fast-and-frictionless-kubernetes-for-all/)
- [Accelerating AI and Databases with Azure Container Storage v2.0: 7x Faster, Open Source, and Free](https://azure.microsoft.com/en-us/blog/accelerating-ai-and-databases-with-azure-container-storage-now-7-times-faster-and-open-source/)
- [Introducing WireGuard In-Transit Encryption for Azure Kubernetes Service (Public Preview)](https://techcommunity.microsoft.com/t5/azure-networking-blog/introducing-wireguard-in-transit-encryption-for-aks-public/ba/p/4421057)

### Azure Platform Infrastructure: Confidential Computing, Observability, and Networking

New confidential DCasv6/ECasv6 VMs provide stronger VM security with hardware root-of-trust and AKS integration, building on last week’s confidential AI infrastructure updates.

Azure Monitor’s Prometheus now features native Grafana dashboards for easier container observability and streamlined DevOps workflows, building on earlier monitoring solutions.

Centralized logging patterns offer scalable observability for cloud-native applications, continuing last week’s event analysis focus. Azure Networking portfolio consolidation streamlines service selection for improved platform user experience.

- [GA: DCasv6 and ECasv6 Confidential VMs with 4th Gen AMD EPYC for Azure](https://techcommunity.microsoft.com/t5/azure-confidential-computing/ga-dcasv6-and-ecasv6-confidential-vms-based-on-4th-generation/ba/p/4451460)
- [Azure Monitor’s Native Grafana Dashboards: Simplified Observability for Prometheus Workloads](https://techcommunity.microsoft.com/t5/azure-observability-blog/azure-monitor-managed-service-for-prometheus-now-includes-native/ba/p/4454254)
- [Centralized Logging in Azure: Proven Observability Patterns for Modern Apps](https://dellenny.com/centralized-logging-in-azure-proven-observability-patterns-for-modern-apps/)
- [Azure Networking Portfolio Consolidation: Simplifying Service Discovery and Management](https://techcommunity.microsoft.com/t5/azure-networking-blog/azure-networking-portfolio-consolidation/ba/p/4454248)

### Azure Cloud Migration, Modernization, and Hybrid Management

"Migrate and Modernize Summit" showcases agentic AI and Accelerate capabilities for automated migrations, building on previous guides for cost optimization and security. Azure Migrate adds better analytics and hybrid management best practices.

AI-based dependency assessment, expanded IaC/Arc support, Private Link, and scanning assist secure migration and modernization. Additional articles review savings for Linux VMs via Hybrid Benefit, ManageX for academic workloads, and cost reduction strategies, extending similar themes from prior automation updates.

- [Accelerate and Simplify Cloud Transformation with New Agentic AI Solutions](https://techcommunity.microsoft.com/t5/azure-migration-and/accelerate-and-simplify-cloud-transformation-with-new-agentic-ai/ba/p/4454873)
- [AI-Powered Migration and Modernization with Azure Essentials, Azure Migrate, and GitHub Copilot](https://techcommunity.microsoft.com/t5/azure-architecture-blog/ai-powered-migration-modernization-secure-resilient-and-ready/ba/p/4454849)
- [Azure Migrate: Connected Experiences for Efficient Cloud Modernization](https://techcommunity.microsoft.com/t5/azure-migration-and/azure-migrate-connected-experiences/ba/p/4454927)
- [Unlocking Cloud Savings for Linux VMs with Azure Hybrid Benefit](/azure/videos/Unlocking-Cloud-Savings-for-Linux-VMs-with-Azure-Hybrid-Benefit)
- [Future-Proofing Academic Research: Secure AI Computing Workflows with Terawe ManageX on Azure](/2025-09-16-Future-Proofing-Academic-Research-Secure-AI-Computing-Workloads-with-Terawe-ManageX-on-Azure.html)
- [How to Cut Your Azure Bill in Half Without Losing Performance](https://dellenny.com/how-to-cut-your-azure-bill-in-half-without-losing-performance/)

### Azure Platform Foundation: Essential Updates and Emerging Developer Tools

Ongoing foundational updates follow last week’s VM releases and service transition announcements. AKS is moving away from Azure Linux 2.0, requiring transition planning.

The new open-source Image Customizer for Azure Linux improves automation and secure image management for DevOps, extending on previous deployment toolkit advancements.

Azure Monitor Health Models convert telemetry into health signals, reducing alert fatigue and helping operations staff respond proactively. Beginner guides for the Azure Free Tier and developer toolkits continue last week’s onboarding content.

Infrastructure highlights from Microsoft’s Fairwater AI datacenter demonstrate ongoing modernization and resiliency themes covered in prior security stories.

- [Azure Update - 19th September 2025](/azure/videos/Azure-Update-19th-September-2025)
- [Introducing Image Customizer for Azure Linux](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/introducing-image-customizer-for-azure-linux/ba/p/4454859)
- [Inside the World’s Most Powerful AI Datacenter: Microsoft’s Fairwater Facility](https://blogs.microsoft.com/blog/2025/09/18/inside-the-worlds-most-powerful-ai-datacenter/)
- [Understanding Azure Monitor Health Models](/azure/videos/Understanding-Azure-Monitor-Health-Models)
- [How to Update proxyAddresses for a Cloud-Only Entra ID User](https://techcommunity.microsoft.com/t5/azure/how-to-update-the-proxyaddresses-of-a-cloud-only-entra-id-user/m-p/4454763#M22217)
- [Top 7 Azure Services You Didn’t Know You Needed](https://dellenny.com/top-7-azure-services-you-didnt-know-you-needed/)
- [Azure for Beginners: How to Launch Your First Cloud Project in 30 Minutes](https://dellenny.com/azure-for-beginners-how-to-launch-your-first-cloud-project-in-30-minutes/)

## Coding

Coding updates this week focus on practical improvements for .NET developers, tool packaging strategies, performance, and changes to support life cycles. As .NET 10 nears release, content covers actionable planning advice for evolving within the .NET ecosystem.

### .NET 10 Tool Packaging, WebAssembly, and Support Lifecycle Changes

Andrew Lock’s expanded coverage from last week’s .NET 10 RC1 preview addresses platform-specific tool packaging, new schemas for DotnetToolSettings.xml, and approaches for dual packaging to maintain support for older SDKs, as illustrated by Datadog and NativeAOT examples—continuing migration support started earlier.

The latest ASP.NET Community Standup covers .NET 10’s WebAssembly runtime, outlining performance enhancements and migration tactics that build upon last week’s component feature improvements.

Microsoft will extend .NET Standard Term Support to 24 months beginning with .NET 9, directly addressing prior migration challenges and adopting past recommendations for update planning.

- [Supporting Platform-Specific .NET Tools on Older SDKs: .NET 10 Preview Deep Dive](https://andrewlock.net/exploring-dotnet-10-preview-features-8-supporting-platform-specific-dotnet-tools-on-old-sdks/)
- [ASP.NET Community Standup: .NET 10 WebAssembly Performance Enhancements](/coding/videos/ASPNET-Community-Standup-NET-10-WebAssembly-Performance-Enhancements)
- [.NET Standard Term Support (STS) Releases Will Be Supported for 24 Months Starting with .NET 9](https://devblogs.microsoft.com/dotnet/dotnet-sts-releases-supported-for-24-months/)

## DevOps

Recent DevOps coverage highlights workflow automation, higher code quality, and improved security. Updates include GitHub Actions and Dependabot features, migration advice, advancements in AI-powered automation, modern workflow strategies, and enhanced platform security.

### GitHub Actions and Dependabot Enhancements

GitHub Actions now features YAML anchor support and can use workflow templates from private repositories, streamlining complex CI/CD automation and internal sharing. A new job context variable (`check_run_id`) helps target build artifacts and notifications, reducing setup complexity. Dependabot adds support for Conda `environment.yml` files so Python/science projects now automate vulnerability and version updates, enhancing supply chain protections.

These changes build on last week’s automation transparency and secure workflow improvements.

- [GitHub Actions Adds YAML Anchors and Workflow Templates from Non-Public Repositories](https://github.blog/changelog/2025-09-18-actions-yaml-anchors-and-non-public-workflow-templates)
- [Dependabot Adds Support for Conda Environment Files](https://github.blog/changelog/2025-09-16-conda-ecosystem-support-for-dependabot-now-generally-available)

### Platform Deprecations and Migration Guidance

GitHub Actions will phase out Node 20 for JavaScript actions, switching to Node 24 default by March 2026. Developers should start updating workflows and testing affected architectures now. The macOS 13 runner image retires on December 4, 2025, with interim service interruptions and guidance to migrate to ARM64 runners.

These transitions align with last week’s migration tips to ensure stable CI pipelines.

- [Deprecation of Node 20 on GitHub Actions Runners](https://github.blog/changelog/2025-09-19-deprecation-of-node-20-on-github-actions-runners)
- [Retirement of GitHub Actions macOS 13 Runner Image Announced](https://github.blog/changelog/2025-09-19-github-actions-macos-13-runner-image-is-closing-down)

### AI-Powered Automation: Code Review and Infrastructure

CodeRabbit now includes CLI options, auto-generated tests, custom merge checks, and MCP integration for scalable, AI-driven code reviews. Teams automate review outside IDEs, increase test coverage, and link feedback to documentation for safer handling of AI-generated code.

Pulumi Neo previews AI agents for infrastructure tasks, automating diagnostics, deployments, compliance, and audit reporting. MCP support enables broader pipeline integration with balanced manual oversight and automation.

These features extend last week’s agentic improvement in CI/CD traceability and workflow governance.

- [CodeRabbit Adds CLI and AI-Powered Enhancements to Code Review Platform](https://devops.com/coderabbit-adds-cli-support-to-code-review-platform-based-on-ai/)
- [CodeRabbit Expands AI Code Review Platform with CLI and MCP Support](https://devops.com/coderabbit-adds-cli-support-to-code-review-platform-based-on-ai/?utm_source=rss&utm_medium=rss&utm_campaign=coderabbit-adds-cli-support-to-code-review-platform-based-on-ai)
- [Pulumi Introduces AI Agents for Automated Infrastructure Management](https://devops.com/pulumi-previews-ai-agents-trained-to-automate-infrastructure-management/?utm_source=rss&utm_medium=rss&utm_campaign=pulumi-previews-ai-agents-trained-to-automate-infrastructure-management)

### Workflow Automation Trends: AI Agents and GitOps for Next-Gen DevOps

Guides provide actionable approaches for employing AI agents in Jira ticketing, code review, and deployment with LangChain, OpenAI, and event hooks. Recommendations highlight increased autonomy in ticket creation, PR handling, and deployments, as developers shift toward oversight.

A migration roadmap for enterprise GitOps details phases—asset review, repository restructuring, Argo CD/Flux automation, policy enforcement, and audit-ready service transitions. The focus remains on compliance and ongoing improvement.

These resources build on last week’s secure automation and agent-based pipeline content.

- [Automating Jira, PR Reviews, and Deployment with AI Agents](https://dellenny.com/supercharging-your-workflow-using-an-ai-agent-to-automate-jira-updates-pr-reviews-and-code-deployment/)
- [From Legacy to GitOps: A Roadmap for Enterprise Modernization](https://devops.com/from-legacy-to-gitops-a-roadmap-for-enterprise-modernization/)

### DevOps Security, Platform Resilience, and Industry Analysis

Recent analysis reviews platform outages and security incidents with GitHub, Jira, and Bitbucket, recommending improved redundancy, secrets management, observability, and disaster planning.

JFrog’s swampUP 2025 event introduces agent-aware artifact validation (AppTrust, JFrog Fly, MCP integration) for traceability and governance. Harness advocates AI-enabled CI/CD with automated remediation, contextual security, and developer portal access.

DevOps-as-a-Service solutions now target modular, AI-augmented management for organizations exploring new orchestration tools.

These topics extend last week’s unified observability, compliance, and platform automation content.

- [Outages and Security Threats in DevOps Tooling: Cracks in the Foundation](https://devops.com/outages-and-security-threats-in-devops-tooling-cracks-in-the-foundation/)
- [AI-Driven Security and Automation in Modern DevOps: Insights from JFrog swampUP 2025](https://devops.com/empowering-secure-agentic-software-delivery/)
- [Harness CEO Advocates AI-Driven Transformation of CI/CD Workflows](https://devops.com/harness-ceo-calls-for-reimagining-of-ci-cd-workflows-in-the-ai-era/)
- [Is the Future of DevOps DevOps-as-a-Service (DaaS)?](https://devops.com/is-the-future-of-devops-daas/)

### Other DevOps News

GitHub Enterprise now offers public preview of daily license history tracking, supporting compliance, billing, and audit processes, following last week’s repository management improvement coverage.

- [GitHub Enterprise License History Tracking Public Preview](https://github.blog/changelog/2025-09-15-github-enterprise-license-history-tracking-now-available-in-public-preview)

Advisory articles underscore that successful automation depends on early QA focus and combining scripts with human insight for specialized and regulated workflows.

- [Why Automation Fails Without the Right QA Mindset](https://devops.com/why-automation-fails-without-the-right-qa-mindset/)

## Security

This week’s security updates highlight more sophisticated supply chain attacks and stronger platform controls, with vendor improvements addressing code safety, governance, identity management, and quantum-era risks.

### Malicious Extension Threats in Developer Ecosystems

Investigations reveal the WhiteCobra group distributing advanced malicious VSCode and Open VSX Marketplace extensions, following last week’s focus on AI tool security. WhiteCobra employs payloads split across helper files and scripts to circumvent static analysis, with LummaStealer stealing wallets, credentials, and cloud accounts. Automated fake reviews, cross-market spreading (Cursor/Windsurf), and multi-OS payloads escalate risks, including some targeting Ethereum contributors. Experts recommend exacting extension screening, stronger monitoring, and improved supply chain security.

- [WhiteCobra Targets Developers with Malicious VSCode Marketplace Extensions](https://devops.com/whitecobra-targets-developers-with-dozens-of-malicious-extensions/?utm_source=rss&utm_medium=rss&utm_campaign=whitecobra-targets-developers-with-dozens-of-malicious-extensions)
- [WhiteCobra’s Malicious VSCode Extensions Pose Major Security Risk for Developers](https://devops.com/whitecobra-targets-developers-with-dozens-of-malicious-extensions/)

### Enterprise Platform Security: GitHub’s New Controls

GitHub released general availability for enterprise access controls using corporate proxies, supporting compliance by routing traffic behind enterprise firewalls through customized headers—responding to last week’s calls for robust access management and registry controls.

Centralized security contacts now coordinate incident alerts for large organizations. Delegated bypass for push protection allows admins to oversee secret exposures and approve exceptions through APIs, streamlining governance and incident response.

- [Enterprise Access Restrictions with Corporate Proxies for GitHub Enterprise Cloud Now Available](https://github.blog/changelog/2025-09-15-enterprise-access-restrictions-with-corporate-proxies-is-now-generally-available)
- [Security Contact Email Setting for Enterprise Incident Notifications](https://github.blog/changelog/2025-09-14-security-contact-for-security-notification-emails-is-generally-available)
- [Delegated Bypass Controls for Push Protection Now Available at the Enterprise Level](https://github.blog/changelog/2025-09-16-delegated-bypass-controls-for-push-protection-now-available-at-the-enterprise-level)

### Preparing for the Quantum Era: Post-Quantum Secure SSH on GitHub

GitHub now defaults to post-quantum secure SSH key exchange for Git operations, using the hybrid `sntrup761x25519-sha512` algorithm as of September 17, 2025. Compatible OpenSSH clients (9.0+) are automatically covered, helping protect source code against future quantum threats. The change builds on last week’s progress in encryption and source control safety.

- [Post-Quantum Secure SSH Access on GitHub](https://github.blog/engineering/platform-security/post-quantum-security-for-ssh-access-on-github/)

### Securing the Software Supply Chain and Open Source Dependencies

Shai-Hulud, an NPM worm, used typosquatting and replication to compromise Node.js/JavaScript packages, raising publisher and dependency risks. Best practices now include SBOMs, MFA, signed packages, version pinning, and consistent audits—reinforcing supply chain hygiene and earlier DevOps security topics.

- [Shai-Hulud Attacks Undermine Software Supply Chain Security](https://devops.com/shai-hulud-attacks-shake-software-supply-chain-security-confidence/)

### Other Security News

Microsoft updates Purview tools for Fabric, with stricter data protection, DLP, insider risk management, assessment, and better cataloging—mirroring previous access control developments.

- [Microsoft Purview Innovations for Fabric: Unifying Data Security and Governance for AI](https://www.microsoft.com/en-us/security/blog/2025/09/16/microsoft-purview-innovations-for-your-fabric-data-unify-data-security-and-governance-for-the-ai-era/)

Identity protection tips cover hybrid settings in Active Directory and cloud Entra ID, continuing last week’s advice on hybrid identity, backup, and recovery practices to address evolving risks.

- [Protecting Identity in Active Directory & Microsoft Entra](https://www.thomasmaurer.ch/2025/09/protecting-identity-in-active-directory-microsoft-entra/)
