---
layout: "post"
title: "GitHub Copilot, AI Accelerators, and Secure Cloud: The Latest in Platform and Developer Tools"
description: "This week's update covers expanded AI-powered development tools and updates across cloud platforms, including new GitHub Copilot agent integrations and SDK enhancements, Microsoft's Maia 200 AI accelerator for Azure, and additional frameworks for agent protocols and multi-model orchestration. Improvements also include enhanced data engineering with Microsoft Fabric and security updates like AI-driven threat detection, broader CodeQL support, and new identity protections."
author: "Tech Hub Team"
excerpt_separator: <!--excerpt_end-->
viewing_mode: "internal"
date: 2026-02-02 09:00:00 +00:00
permalink: "/2026-02-02-Weekly-AI-and-Tech-News-Roundup.html"
categories: ["AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security"]
tags: [".NET", "Agentic Workflows", "AI", "Azure", "CLI Automation", "Cloud Computing", "Coding", "Compliance", "Data Engineering", "DevOps", "GitHub Copilot", "Kubernetes", "Machine Learning", "MCP", "ML", "Open Source", "Roundups", "SDKs", "Security"]
tags_normalized: ["dotnet", "agentic workflows", "ai", "azure", "cli automation", "cloud computing", "coding", "compliance", "data engineering", "devops", "github copilot", "kubernetes", "machine learning", "mcp", "ml", "open source", "roundups", "sdks", "security"]
---

Welcome to this week's technical news roundup, with updated tools and platform features built around AI, cloud, and developer productivity. GitHub Copilot adds new agent-driven workflows, SDKs, and command-line features, while Microsoft's Maia 200 accelerator offers more options for AI workloads on Azure. Cloud platform changes include data engineering improvements, operational changes, and advances in secure identity. You will find practical guides, resources for implementing agent protocols, and hands-on strategies for building modern applications and workflows.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Copilot Model and IDE Integration Updates](#copilot-model-and-ide-integration-updates)
  - [GitHub Copilot CLI: Automation, Model Switching, and Agent Protocols](#github-copilot-cli-automation-model-switching-and-agent-protocols)
  - [GitHub Copilot SDK and Agent Framework](#github-copilot-sdk-and-agent-framework)
  - [GitHub Copilot Agent Ecosystem and Extension Mechanisms](#github-copilot-agent-ecosystem-and-extension-mechanisms)
  - [Copilot for Cloud, DevOps, and App Deployment](#copilot-for-cloud-devops-and-app-deployment)
  - [Copilot Metrics, Compliance, and API Evolution](#copilot-metrics-compliance-and-api-evolution)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Maia 200 AI Accelerator: Launch, Architecture, and Developer Impact](#maia-200-ai-accelerator-launch-architecture-and-developer-impact)
  - [Model Context Protocol, Agent UIs, and Agentic Workflows in VS Code](#model-context-protocol-agent-uis-and-agentic-workflows-in-vs-code)
  - [Microsoft Agent Framework, Multi-Agent Orchestration, and UI Integration](#microsoft-agent-framework-multi-agent-orchestration-and-ui-integration)
  - [.NET AI Integration and Stateful Conversational Patterns](#net-ai-integration-and-stateful-conversational-patterns)
  - [Agentic Systems, Platform Governance, and Responsible Integration](#agentic-systems-platform-governance-and-responsible-integration)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Reliability and Diagnostics in Production-Scale Reinforcement Learning](#reliability-and-diagnostics-in-production-scale-reinforcement-learning)
  - [Local-First Agentic Automation and Multi-Agent Orchestration](#local-first-agentic-automation-and-multi-agent-orchestration)
  - [Streamlined Model Evaluation and Selection with Microsoft Foundry](#streamlined-model-evaluation-and-selection-with-microsoft-foundry)
  - [Data Engineering and Platform Operations with Microsoft Fabric](#data-engineering-and-platform-operations-with-microsoft-fabric)
  - [Multimodal Reinforcement Learning Advances in Medical Imaging](#multimodal-reinforcement-learning-advances-in-medical-imaging)
- [Azure](#azure)
  - [Microsoft Fabric Platform: Advancements Across Data, Security, and Integration](#microsoft-fabric-platform-advancements-across-data-security-and-integration)
  - [Microsoft Fabric Community, Certification, and Identity Governance](#microsoft-fabric-community-certification-and-identity-governance)
  - [Azure Kubernetes and Traffic Management: Automation, Migration, and Multi-Region Patterns](#azure-kubernetes-and-traffic-management-automation-migration-and-multi-region-patterns)
  - [Azure Developer Tooling and SDK Updates](#azure-developer-tooling-and-sdk-updates)
  - [Azure Cloud Infrastructure and AI Hardware](#azure-cloud-infrastructure-and-ai-hardware)
  - [Updates in Azure High Availability, Security, Monitoring, and Developer Operations](#updates-in-azure-high-availability-security-monitoring-and-developer-operations)
  - [Developer Troubleshooting and Migration Resources](#developer-troubleshooting-and-migration-resources)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET and C# Development: Practical Instrumentation, Performance, and Language Features](#net-and-c-development-practical-instrumentation-performance-and-language-features)
  - [Language Design, Tooling Philosophy, and AI in Programming](#language-design-tooling-philosophy-and-ai-in-programming)
  - [WebAssembly and Cross-Platform Coding: .NET, Uno, and Microsoft’s Vision](#webassembly-and-cross-platform-coding-net-uno-and-microsofts-vision)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [GitHub Platform Updates: Actions, Codespaces, and Dependency Management](#github-platform-updates-actions-codespaces-and-dependency-management)
  - [Kubernetes Ingress Controller Deprecation and Migration](#kubernetes-ingress-controller-deprecation-and-migration)
  - [Automation, Observability, and SRE Agent Integration on Azure](#automation-observability-and-sre-agent-integration-on-azure)
  - [Evolving DevOps Workflows: Specification-Driven and Collaborative Development](#evolving-devops-workflows-specification-driven-and-collaborative-development)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [AI-Assisted Detection Engineering and Supply Chain Protection](#ai-assisted-detection-engineering-and-supply-chain-protection)
  - [Static Analysis: CodeQL 2.24.0 Expands Language Coverage and Security Capabilities](#static-analysis-codeql-2240-expands-language-coverage-and-security-capabilities)
  - [Fabric Data Security: Outbound Access Protection and Workspace-Level Firewalls](#fabric-data-security-outbound-access-protection-and-workspace-level-firewalls)
  - [Identity Security: Passkey-Based Authentication with Entra ID](#identity-security-passkey-based-authentication-with-entra-id)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot continues to grow as an AI-powered development platform by adding automation options, agent-based workflows, and support for enterprise integrations. This week, the platform introduced additional SDKs, updates to the CLI, new agent features, and targeted resources for developers and architects. Key releases include expanded support for the GPT-5.2-Codex model, agent protocol integration in the CLI, improvements in SDK orchestration, and new resources for validating Copilot-powered tools at scale. Building on last week's coverage of agentic workflows, Copilot is now more tightly integrated across developer tools and automation workflows.

### Copilot Model and IDE Integration Updates

GPT-5.2-Codex is now live in Copilot for multiple IDEs, including Visual Studio, JetBrains, Xcode, Eclipse, VS Code, GitHub.com, and GitHub Mobile. This change adds more AI coding options, builds on last week's CLI model selection features, and enables subscribers across all business tiers to use a chat-based model picker for agent, ask, and edit modes. Make sure you meet the minimum version requirements, and check your Copilot settings for rollouts. The updates improve code review, conversational interactions, and AI-generated code in multi-language, multi-project environments. Expanding organizational context support, the rollout provides more relevant code suggestions for distributed teams.

- [GPT-5.2-Codex is now available in GitHub Copilot for Multiple IDEs](https://github.blog/changelog/2026-01-26-gpt-5-2-codex-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)

### GitHub Copilot CLI: Automation, Model Switching, and Agent Protocols

The Copilot CLI continues to improve for terminal automation. Following last week's introduction of "Plan" mode, this week adds seamless switching between 14 AI models—including Claude Opus 4.5 and GPT-5.2 Codex—directly in the CLI. This gives finer-grained control over model selection and reasoning for code changes and analysis tasks. Voice dictation speeds up command input, and installation is now easier with Homebrew and Winget for Windows and macOS.

A public preview of the Agent Client Protocol (ACP) brings standardized integration for Copilot CLI with other tools, IDEs, and CI/CD pipelines, making Copilot automation easier to embed. New tutorials and demos show practical CLI use for onboarding, code review, debugging, and accessibility workflows. Enhanced command approval, permissions, and automation based on context further improve secure development.

- [What’s New in the GitHub Copilot CLI?]({{ "/2026-01-26-Whats-New-in-the-GitHub-Copilot-CLI.html" | relative_url }})
- [Switching Models in GitHub Copilot CLI: Demo by @shanselman]({{ "/2026-02-01-Switching-Models-in-GitHub-Copilot-CLI-Demo-by-shanselman.html" | relative_url }})
- [ACP Protocol Support Now Available in GitHub Copilot CLI](https://github.blog/changelog/2026-01-28-acp-support-in-copilot-cli-is-now-in-public-preview)
- [Power Agentic Workflows in Your Terminal with GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/power-agentic-workflows-in-your-terminal-with-github-copilot-cli/)
- [How a Designer Built an ASCII Animation Tool with GitHub Copilot]({{ "/2026-01-30-How-a-Designer-Built-an-ASCII-Animation-Tool-with-GitHub-Copilot.html" | relative_url }})
- [Engineering Accessibility and Animation for GitHub Copilot CLI’s ASCII Banner](https://github.blog/engineering/from-pixels-to-characters-the-engineering-behind-github-copilot-clis-animated-ascii-banner/)

### GitHub Copilot SDK and Agent Framework

The Copilot SDK and agent features have been enhanced, continuing last week's focus on cross-language SDKs, agentic workflows, and integrations for Node.js, Python, Go, and .NET. The SDK works with the Microsoft Agent Framework so you can use Copilot, Azure OpenAI, and other models together. This improves modular workflows with features for session management, streaming, and permissions.

Guides cover tasks like image extraction, agent memory for update tracking, multi-model routing, and hybrid AI that combines Foundry Local SLMs for private data with cloud LLMs for richer content. The tutorials support rapid prototyping and stronger automation for teams using AI at scale.

- [Building a Color Palette App Using GitHub Copilot SDK]({{ "/2026-01-30-Building-a-Color-Palette-App-Using-GitHub-Copilot-SDK.html" | relative_url }})
- [Building Agents with GitHub Copilot SDK: A Practical Guide to Automated Tech Update Tracking](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-agents-with-github-copilot-sdk-a-practical-guide-to/ba-p/4488948)
- [Hybrid AI with GitHub Copilot SDK: Automating README to PowerPoint Generation Using Microsoft Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/github-copilot-sdk-and-hybrid-ai-in-practice-automating-readme/ba-p/4489694)
- [Build AI Agents with GitHub Copilot SDK and Microsoft Agent Framework](https://devblogs.microsoft.com/semantic-kernel/build-ai-agents-with-github-copilot-sdk-and-microsoft-agent-framework/)
- [Integrating GitHub Copilot SDK: Live Coding on Rubber Duck Thursdays]({{ "/2026-01-28-Integrating-GitHub-Copilot-SDK-Live-Coding-on-Rubber-Duck-Thursdays.html" | relative_url }})

### GitHub Copilot Agent Ecosystem and Extension Mechanisms

Copilot now offers an Agents tab in repositories for centralized task management, giving teams a single point to view, switch, and track coding agent activity in the desktop, browser, or terminal interface. This update builds on recent efforts for team-based agent infusions and smoother modular integration.

Comparisons between Custom Agents, Agent Skills, and MCP Tools help guide users in building the right automation mix for their workflows. Updates to the GitHub MCP Server add features including OAuth filtering, Copilot integration with pull requests, and project tracking. These build on recent work in credential management and platform tools. Also, unit test agent profiles for Logic Apps and Data Maps extend cloud-based test automation.

- [Introducing the Agents Tab for Copilot Coding Agents in GitHub Repositories](https://github.blog/changelog/2026-01-26-introducing-the-agents-tab-in-your-repository)
- [Comparing Custom Agents, Skills, and MCP Tools in GitHub Copilot](https://harrybin.de/posts/github-copilot-context-extensions-compared/)
- [GitHub MCP Server: New Tools for Project Management, OAuth Filtering, and Copilot Integration](https://github.blog/changelog/2026-01-28-github-mcp-server-new-projects-tools-oauth-scope-filtering-and-new-features)
- [Introducing Unit Test Agent Profiles for Logic Apps & Data Maps](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-unit-test-agent-profiles-for-logic-apps-data-maps/ba-p/4490216)

### Copilot for Cloud, DevOps, and App Deployment

Copilot continues to play a bigger part in DevOps by adding more features for CI/CD pipeline automation, cloud migration, and app deployment. The Copilot Agent Skill for Azure Static Web Apps simplifies deployment for JavaScript frameworks, automating configuration checks and troubleshooting. This adds to previous updates supporting Java, Spring, and cloud modernization tasks. Integration with GitHub Actions and Copilot Chat now provides real-time troubleshooting and error resolution for teams adopting or migrating cloud resources.

- [Introducing the Azure Static Web Apps Skill for GitHub Copilot](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-the-azure-static-web-apps-skill-for-github-copilot/ba-p/4487920)

### Copilot Metrics, Compliance, and API Evolution

This week brings public preview dashboards and reporting APIs for Copilot Enterprise Cloud customers with specific data residency requirements. New reporting capabilities help organizations monitor adoption, activity, and features for compliance and data governance. As legacy Copilot metrics APIs are being replaced with more detailed endpoints, users will need to migrate to maintain up-to-date analytics.

- [Copilot Usage Metrics in GitHub Enterprise Cloud with Data Residency: Public Preview](https://github.blog/changelog/2026-01-29-copilot-metrics-in-github-enterprise-cloud-with-data-residency-in-public-preview)
- [Legacy GitHub Copilot Metrics APIs to Sunset in 2026](https://github.blog/changelog/2026-01-29-closing-down-notice-of-legacy-copilot-metrics-apis)

### Other GitHub Copilot News

Best practices for Copilot usage continue to be highlighted, focusing on using Copilot Chat, building effective trust models, and validating outputs in real workflows. This aligns with Copilot's documented approach to onboarding and code review.

Copilot’s integration into SQL Server Migration Assistant for Oracle offers new AI-powered support for large codebase migrations, supporting ongoing modernization efforts detailed last week for Java and Spring environments.

Community workshops and demos remain a central channel for hands-on learning. These resources illustrate how to use SDKs and CLI integrations, write effective prompts, and modernize applications with Copilot.

- [Trust, but Verify: Building Confidence in GitHub Copilot Output](https://www.cooknwithcopilot.com/blog/trust-but-verify-building-confidence-in-github-copilot-output.html)
- [Accelerating Oracle to SQL Server Migrations with AI and Copilot in SSMA]({{ "/2026-01-28-Accelerating-Oracle-to-SQL-Server-Migrations-with-AI-and-Copilot-in-SSMA.html" | relative_url }})
- [Modernizing Applications with GitHub Copilot: Workshop Overview]({{ "/2026-01-29-Modernizing-Applications-with-GitHub-Copilot-Workshop-Overview.html" | relative_url }})

## AI

This week’s AI updates include new hardware, integration tools, and workflow enhancements, with a focus on agent protocols, orchestration, and stateful AI in developer tooling. Microsoft’s Maia 200 accelerator expands Azure’s AI hardware portfolio, and developers gain unified .NET AI integration tools, updated model orchestration, and sustainability practices.

### Maia 200 AI Accelerator: Launch, Architecture, and Developer Impact

Microsoft introduced the Maia 200, a new AI inference accelerator built for large Azure workloads. It offers roughly 30% better inference per dollar and clusters scale up to 6,144 chips. Key features include 140B transistors at TSMC 3nm, FP4/FP8/FP6 tensor cores, 216GB HBM3e at 7TB/s, and liquid cooling.

Maia SDK previews provide Triton compiler integration, PyTorch support, NPL programming, a simulator, and tools for model migration. Azure-native orchestration, monitoring, and benchmarking are also included. The Maia 200 sees active use in Microsoft’s infrastructure, powering large model deployments and supporting external frameworks and OpenAI models.

Resources for SDKs and model tuning support various inference requirements, and developers can now prepare for more optimized AI environments on Azure.

- [Maia 200: The AI Accelerator Built for Inference](https://blogs.microsoft.com/blog/2026/01/26/maia-200-the-ai-accelerator-built-for-inference/)
- [Deep Dive into the Maia 200 AI Inference Accelerator Architecture](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/deep-dive-into-the-maia-200-architecture/ba-p/4489312)
- [Introducing Maia 200: The AI Accelerator Built for Inference](https://news.microsoft.com/january-2026-news)
- [Microsoft Launches Maia 200 AI Accelerator on Azure for Enhanced Performance and Efficiency](https://www.linkedin.com/posts/satyanadella_our-newest-ai-accelerator-maia-200-is-now-activity-7421583368754110465-tXQM)

### Model Context Protocol, Agent UIs, and Agentic Workflows in VS Code

VS Code has added public preview support for Model Context Protocol (MCP) Apps, its first official extension supporting AI agents with interactive UI components in the chat panel. Integrations with partners such as Storybook enable richer, practical UIs for AI in the IDE.

This continues last week's story on agent interoperability, now moving toward UI-driven agent flows inside VS Code. The Agent Sessions Day event showcased open source community extensions and AI-driven workflows that help streamline common development tasks.

- [Giving Agents a Visual Voice: MCP Apps Support in VS Code](https://code.visualstudio.com/blogs/2026/01/26/mcp-apps-support)
- [VS Code Live: Agent Sessions Day – Exploring AI and Agentic Development]({{ "/2026-01-28-VS-Code-Live-Agent-Sessions-Day-Exploring-AI-and-Agentic-Development.html" | relative_url }})

### Microsoft Agent Framework, Multi-Agent Orchestration, and UI Integration

Multi-agent support has expanded in Microsoft Agent Framework (MAF). A practical guide explains integrating the Claude Agent SDK with MAF, enabling combined workflows using Claude, Azure OpenAI, and Copilot (in Python). Useful features like permissions, function registration, async orchestration, and session management are described, including agent reviewer flows.

Updates include open event-driven protocol support via AG-UI, allowing Python/FastAPI/Azure OpenAI to handle streaming, observable workflows, and interoperable interfaces across agent architectures.

- [Integrating Claude Agent SDK with Microsoft Agent Framework for Advanced AI Agents](https://devblogs.microsoft.com/semantic-kernel/build-ai-agents-with-claude-agent-sdk-and-microsoft-agent-framework/)
- [Building Interactive Agent UIs with AG-UI and Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-interactive-agent-uis-with-ag-ui-and-microsoft-agent/ba-p/4488249)

### .NET AI Integration and Stateful Conversational Patterns

".NET AI Essentials" introduces the Microsoft.Extensions.AI (MEAI) library, which provides a unified interface for OpenAI, Azure OpenAI, and OllamaSharp, supporting adapters, output parsing, streaming, and multi-modal requests. Dependency injection and middleware are built in. This “building blocks” design helps you bring standard, provider-agnostic AI features into .NET apps, closely linked with Semantic Kernel and Aspire.

Detailed guides show how to move from stateless to stateful AI interactions within .NET, including examples for managing stateful assistants that adapt over multiple sessions.

- [.NET AI Essentials: Unified Building Blocks for Intelligent Apps](https://devblogs.microsoft.com/dotnet/dotnet-ai-essentials-the-core-building-blocks-explained/)
- [Beyond the Prompt: Designing Stateful AI Experiences in .NET]({{ "/2026-01-26-Beyond-the-Prompt-Designing-Stateful-AI-Experiences-in-NET.html" | relative_url }})

### Agentic Systems, Platform Governance, and Responsible Integration

Microsoft is focusing on balancing intelligence and trust across enterprise agent architectures (Agent 365, Foundry IQ, Fabric, and 365 Copilot). The strategy emphasizes unified observability, governance, and workflow integrations for secure, responsible deployment at scale.

Building from last week's discussion of workflow specs and sector adoption, the story now includes examples from healthcare, manufacturing, and finance. Supporting tools like Agent Factory and Copilot integration improve deployment, governance, and automation.

- [How Microsoft is Empowering Frontier Transformation with Intelligence + Trust](https://blogs.microsoft.com/blog/2026/01/27/how-microsoft-is-empowering-frontier-transformation-with-intelligence-trust/)

### Other AI News

On AI-powered robotics, Microsoft Cozy AI Kitchen explores moving from scripted robots to generative systems by using action tokens and natural language for more adaptive machines. Integration with Azure AI and Teams shows how to build collaborative robotics.

Last week’s coverage of physical AI continues with guides and case studies on real-world agentic applications.

- [Inside the Future of AI‑Powered Robotics with Tim Chung | Cozy AI Kitchen]({{ "/2026-01-27-Inside-the-Future-of-AIPowered-Robotics-with-Tim-Chung-Cozy-AI-Kitchen.html" | relative_url }})

A step-by-step guide to building a free AI-powered inventory manager uses Azure Cognitive Services and .NET to help automate grocery tracking, showing accessible paths for AI automation.

- [Building a Free AI-Powered Inventory Manager with Azure]({{ "/2026-01-29-Building-a-Free-AI-Powered-Inventory-Manager-with-Azure.html" | relative_url }})

Sustainability is addressed this week in a practical guide for aligning AI with environmental goals. Concrete tips cover moving workloads to energy-efficient infrastructure, optimizing processes, and selecting AI models that match business and sustainability needs. Case studies demonstrate cost and energy reductions.

- [5 Essential Practices to Align AI Transformation with Sustainability](https://www.microsoft.com/en-us/microsoft-cloud/blog/2026/01/28/beyond-davos-2026-5-practices-to-align-ai-transformation-and-sustainability/)

## ML

This week’s ML updates cover new ways to diagnose RL workflows, agent orchestration patterns, more structured evaluation methods, and enhancements for data engineering with Microsoft Fabric. Advancements are also noted for medical imaging and multimodal RL solutions.

### Reliability and Diagnostics in Production-Scale Reinforcement Learning

Microsoft engineering teams released guidance on troubleshooting RL agent instability in production. Traditional aggregate metrics often miss rare errors, so this week’s article presents slice-aware diagnostics to flag drift and instability at the per-token level (using log-ratio percentiles and CDF drift analysis). Agent “tail growth” signals increase risk and needs mitigation.

Open-source Post-Training Toolkit features include TRL integration, a CLI, and distributed monitoring for RL systems, enabling more detailed RL debugging in production.

- [Diagnosing Instability in Production-Scale Agent Reinforcement Learning](https://devblogs.microsoft.com/engineering-at-microsoft/diagnosing-instability-in-production-scale-agent-rl/)

### Local-First Agentic Automation and Multi-Agent Orchestration

Local-first, privacy-focused agent pipelines are growing in use. This week’s hands-on guide covers a podcast automation workflow using the Agent Framework, edge-based SLMs (Ollama), and local speech for on-premise orchestration. Modular design examples show search, review, and script generation using real-time observability with DevUI. Complete code and hardware tips are included.

- [Engineering a Local-First Agentic Podcast Studio: A Deep Dive into Multi-Agent Orchestration](https://techcommunity.microsoft.com/t5/microsoft-developer-community/engineering-a-local-first-agentic-podcast-studio-a-deep-dive/ba-p/4488947)

### Streamlined Model Evaluation and Selection with Microsoft Foundry

Model evaluation is now easier using Microsoft Foundry and GitHub Copilot. The step-by-step guide describes forming datasets, running repeatable benchmarks with metrics like F1/METEOR, and analyzing results using the Python SDK and Jupyter. Debugging and documentation guidance are provided, along with pointers to Foundry and Azure AI model leaderboard resources.

- [Evaluating AI Models with Microsoft Foundry and GitHub Copilot]({{ "/2026-01-27-Evaluating-AI-Models-with-Microsoft-Foundry-and-GitHub-Copilot.html" | relative_url }})

### Data Engineering and Platform Operations with Microsoft Fabric

Fabric introduces workspace-level surge protection, minimizing the risk of resource spikes by limiting job concurrency and allowing exemptions for important workloads. This complements last week’s resource management changes for better cost control.

The new "Get Data with Cloud Connection" feature in Fabric Notebooks streamlines secure connections to cloud sources and provides code snippets, making developer workflows faster.

On-premises Data Gateway (January 2026 release) improves connectivity between CSV, Fabric, and Power BI, syncing with Power BI Desktop for easier data pipelines.

A new guide explains robust real-time pipelines in Fabric, including strategies for data validation, lag monitoring, network planning, logging, and clear ownership—all aimed at reducing pipeline downtime in complex environments.

- [Workspace-Level Surge Protection in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/surge-protection-gets-smarter-introducing-workspace-level-controls-preview/)
- [Fabric Connection inside Notebook (Preview)](https://blog.fabric.microsoft.com/en-US/blog/32822/)
- [On-premises Data Gateway January 2026 Release Overview](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-january-2026-release/)
- [Building a Reliable Real-Time Data Pipeline with Microsoft Fabric](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/building-a-reliable-real-time-data-pipeline-with-microsoft/ba-p/4489534)

### Multimodal Reinforcement Learning Advances in Medical Imaging

The GigaTIME project applies multimodal RL to radiology/pathology report writing, fusing text and image data for clearer, patient-specific documentation generation. Insights cover modeling, simulation, and automation, with actionable examples drawn from Microsoft’s latest research.

- [How AI is Learning to Write Smarter Medical Imaging Reports](https://www.microsoft.com/en-us/research/blog/unirg-scaling-medical-imaging-report-generation-with-multimodal-reinforcement-learning/)

## Azure

Azure’s recent changes introduce enhancements in data engineering, platform automation, developer tooling, high-availability infrastructure, cloud hardware, and troubleshooting resources.

### Microsoft Fabric Platform: Advancements Across Data, Security, and Integration

The latest Fabric feature summary brings new data engineering and governance updates. AI-based semantic model summaries make OneLake Catalog navigation easier, while new organizational structures and configuration features improve overall governance. GitHub integration now supports explicit data residency and branch-based commits. A preview of the Fabric Python SDK streamlines programmatic workspace management.

Security improvements include new role APIs, item-level controls, and immutable logs for compliance. The Lakehouse engine now supports Spark session sharing for better concurrency, while VS Code notebooks gain multi-workspace sync and secure connections. Materialized Lake Views now support create/replace, and Data Warehouse automation gets improved stats, result caching, and more flexible statements. Real-Time Intelligence adds in-VNet streaming, MQTT v3, weather APIs, and Copilot-powered KQL. Data Factory introduces incremental copy and hybrid data connectors.

These changes enhance developer workflows, automation, security, and compliance—supporting both low-code and code-first teams.

- [Microsoft Fabric January 2026 Feature Summary](https://blog.fabric.microsoft.com/en-US/blog/fabric-january-2026-feature-summary/)

### Microsoft Fabric Community, Certification, and Identity Governance

The Fabric Influencers Spotlight (Jan) provides best practices and sample workflows, such as building surrogate keys for Data Warehouses, Power BI modeling techniques, write-back patterns, and integration guides. Applied ML concepts, security models for OneLake, and streaming operations round out the guides. Updates also highlight new certifications and architecture resources.

Workspace identity quotas are now easier to manage. Admins can raise the default cap and adjust limits via the portal or REST API, making large deployments and governance more streamlined.

- [Fabric Influencers Spotlight January 2026](https://blog.fabric.microsoft.com/en-US/blog/fabric-influencers-spotlight-january-2026/)
- [Managing Fabric Identities Limits: Tenant Control and Governance](https://blog.fabric.microsoft.com/en-US/blog/take-control-of-fabric-identities-limit-for-your-tenant-generally-available/)

### Azure Kubernetes and Traffic Management: Automation, Migration, and Multi-Region Patterns

A new AKS multi-region architecture proof-of-concept demonstrates integrating External DNS with Azure Traffic Manager. Service annotations in AKS allow DNS endpoint profiles to be managed automatically. This supports blue-green, weighted, or failover routing—improving automation for global cloud workloads.

Migration guides for deprecated AKS Ingress NGINX controllers are now available. Moving to Gateway API and Application Gateway for Containers is recommended as support for NGINX ends in March 2026. The steps emphasize manifest inventory, YAML conversion, and phased rollout for compliance.

- [Integrating External DNS with Azure Traffic Manager for Kubernetes Multi-Region Deployments](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/exploring-traffic-manager-integration-for-external-dns/ba-p/4485690)
- [From Ingress to Gateway API: Migrating Kubernetes Edge in Azure with Application Gateway for Containers](https://techcommunity.microsoft.com/t5/azure-architecture-blog/from-ingress-to-gateway-api-a-pragmatic-path-forward-and-why-it/ba-p/4489779)

### Azure Developer Tooling and SDK Updates

The Azure Developer CLI (azd) January 2026 release enhances configuration and performance processes. New improvements include streamlined config commands, multi-tenant authentication, credential checks, and container build fallback scenarios. Infrastructure tools like Bicep and Terraform are now auto-detected, and file-based caching increases speed for deployment workflows. Deprecated login commands and App Service integrations have been removed. A growing catalog of community templates and SDKs in .NET, Go, Java, and Python support more scenarios, including experimental AI Foundry and new model support in Azure AI Search.

- [Azure Developer CLI (azd) – January 2026: Configuration & Performance](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-january-2026/)
- [Azure SDK January 2026 Release Highlights](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-january-2026/)

### Azure Cloud Infrastructure and AI Hardware

Azure has released Da/Ea/Fasv7-series VMs with AMD’s 5th Gen EPYC "Turin" CPUs, offering up to 35% more CPU performance, 4.5 GHz burst speeds, and up to 160 vCPUs per VM. These updates add more compute, storage, and networking power with improved security features and integrated HSM. Documentation and region info are available for planning adoption.

A technical deep dive provides insight into Microsoft’s approach to building AI infrastructure from silicon through datacenter design, with a focus on reliability and future-readiness.

- [Announcing General Availability of Azure Da/Ea/Fasv7-series VMs based on AMD ‘Turin’ processors](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-general-availability-of-azure-da-ea-fasv7-series-vms/ba-p/4488627)
- [Silicon to Systems: How Microsoft Engineers AI Infrastructure from the Ground Up](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/silicon-to-systems-how-microsoft-engineers-ai-infrastructure/ba-p/4489525)

### Updates in Azure High Availability, Security, Monitoring, and Developer Operations

Azure NetApp Files now has Elastic ZRS for replicating storage volumes synchronously across Availability Zones. This helps SAP, Kubernetes, and compliance workloads achieve higher availability with shared QoS and automated deployments.

The Azure Local LENS Workbook offers large-scale operational insight for fleet management (AKS, Azure Local). This update supports compliance planning and smoother fleet operations.

API Management gets new capabilities for customizing Retry-After headers, so developers can offer better try-again advice in API responses. This builds on recent support for troubleshooting and robust operation.

- [Azure NetApp Files Elastic ZRS: Simplifying Multi-AZ File Storage High Availability](https://techcommunity.microsoft.com/t5/azure-architecture-blog/azure-netapp-files-elastic-zrs-service-level-file-storage-high/ba-p/4484235)
- [Azure Local LENS Workbook: Proactive Operations for Large-Scale Azure Local Deployments](https://techcommunity.microsoft.com/t5/azure-architecture-blog/azure-local-lens-workbook-deep-insights-at-scale-in-minutes/ba-p/4490608)
- [Transforming Retry-After Headers in Azure APIM: A Step-by-Step Guide](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/transforming-retry-after-headers-in-azure-apim-a-step-by-step/ba-p/4489762)

### Developer Troubleshooting and Migration Resources

Recent fixes and guides address issues such as persistent crashes for ServerDiscoveryService.exe in Azure Migrate (Windows Server 2022, .NET 8) and test discovery problems in Logic Apps’ VS Code test framework (from MSTest versioning). Practical troubleshooting steps are provided to help teams maintain smooth devops workflows.

- [ServerDiscoveryService.exe Crash Bug in Azure Migrate Physical Server Discovery](https://techcommunity.microsoft.com/t5/azure-migration-and/azure-migrate-physical-server-discovery-serverdiscoveryservice/m-p/4490238#M733)
- [Fixing Disappearing Logic Apps Standard Test Framework Tests in VS Code](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/automated-test-framework-missing-tests-in-test-explorer/ba-p/4490186)

### Other Azure News

Azure App Testing now requires a minimum of 10 users per engine and 10 minutes per load test. Review test configurations for cost efficiency.

A step-by-step tutorial walks through deploying MoltBot, an AI assistant, to Azure Container Apps. It covers secure devops, troubleshooting, and log setup—all foundational for secure cloud-based AI deployment.

Data Exposed previews SQL Server 2025 features like REST API endpoints, vector support, and new AI tools for search and analytics.

A video-based Azure training course shares introductory tips, resource optimization practices, labs, certification prep, and Copilot chat setup.

- [Understanding Minimum Usage Charges in Azure App Testing Load Tests](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/minimum-usage-in-azure-app-testing/ba-p/4490658)
- [Deploying MoltBot: 24/7 Personal AI Assistant on Azure Container Apps](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploy-moltbot-to-azure-container-apps-your-24-7-ai-assistant-in/ba-p/4490611)
- [REST APIs, Vectors, and AI in SQL Server 2025]({{ "/2026-01-26-REST-APIs-Vectors-and-AI-in-SQL-Server-2025.html" | relative_url }})
- [Learn Azure in 2026]({{ "/2026-01-26-Learn-Azure-in-2026.html" | relative_url }})

## Coding

Recent updates in coding focus on expanded language features, developer tools, and practical advice for .NET, C#, Visual Studio, and programming strategies. The emphasis is on maintainability, performance, and immediate modernization tips.

### .NET and C# Development: Practical Instrumentation, Performance, and Language Features

Andrew Lock’s guide to System.Diagnostics.Metrics APIs teaches metrics, counters, and monitoring integration for ASP.NET Core (using dotnet-counters, Datadog, and OpenTelemetry). Steve Gordon’s tutorial connects benchmarking tools and practical performance gains, showing how to find memory allocation bottlenecks and differences in reporting tools. Nick Chapsas' "100 Essential Tips" round up key best practices for C# devs and introduce Collection Expression Arguments for C# 15/.NET 11, bringing easier parameter handling. He also explores nuanced Boolean behavior. Sentry logging integration now improves observability and connects errors, traces, and logs in .NET and MAUI apps.

- [Creating and Consuming Metrics with System.Diagnostics.Metrics APIs in .NET](https://andrewlock.net/creating-and-consuming-metrics-with-system-diagnostics-metrics-apis/)
- [Application Performance Optimisation in .NET: Practical Techniques and Tools](https://www.stevejgordon.co.uk/talk-application-performance-optimisation-in-practice-60-mins)
- [Solving .NET Memory Allocation Discrepancies: The Case of the Missing 18 Bytes](https://www.stevejgordon.co.uk/the-grand-mystery-of-the-missing-18-bytes)
- [100 Essential Tips for Writing Better C# Code]({{ "/2026-01-26-100-Essential-Tips-for-Writing-Better-C-Code.html" | relative_url }})
- [Introducing Collection Expression Arguments in C# 15 and .NET 11]({{ "/2026-01-27-Introducing-Collection-Expression-Arguments-in-C-15-and-NET-11.html" | relative_url }})
- [The Boolean Trick No C# Developer Knows About]({{ "/2026-01-29-The-Boolean-Trick-No-C-Developer-Knows-About.html" | relative_url }})
- [Integrating Sentry Logging with .NET Applications](https://dotnetfoundation.org/news-events/detail/sponsor-spotlight-sentryblog1)

### Language Design, Tooling Philosophy, and AI in Programming

Insights from Anders Hejlsberg, the creator of C# and TypeScript, highlight the importance of fast feedback, backward compatibility, and open collaboration for large codebases. Interviews outline the migration of TypeScript to Go and discuss the benefits of Go's garbage-collected architecture. Hejlsberg also notes AI can boost productivity but does not yet provide the determinism compilers require. TypeScript’s mission of adding opt-in static typing and better tooling to JavaScript is clarified.

- [7 Key Lessons from Anders Hejlsberg: Architect of C# and TypeScript](https://github.blog/developer-skills/programming-languages-and-frameworks/7-learnings-from-anders-hejlsberg-the-architect-behind-c-and-typescript/)
- [TypeScript inventor Anders Hejlsberg: AI’s Role in Language Porting and Development Tools](https://devclass.com/2026/01/28/typescript-inventor-anders-hejlsberg-ai-is-a-big-regurgitator-of-stuff-someone-has-done/)
- [Anders Hejlsberg on the Origins of TypeScript and Fixing JavaScript]({{ "/2026-01-27-Anders-Hejlsberg-on-the-Origins-of-TypeScript-and-Fixing-JavaScript.html" | relative_url }})

### WebAssembly and Cross-Platform Coding: .NET, Uno, and Microsoft’s Vision

WebAssembly support is expanding, with improved AOT compilation for .NET 10 and multi-threaded workflows using Uno Platform. New surveys show faster browser loads (Chrome, Firefox, Safari), wider runtime adoption (Node.js, Wasmtime, Deno), and broader language support in Wasm 3.0.

- [WebAssembly Adoption Accelerates with Microsoft .NET 10 and Uno Platform Collaboration](https://devclass.com/2026/01/28/webassembly-gaining-adoption-behind-the-scenes-as-technology-advances/)

### Other Coding News

Visual Studio’s January 2026 update brings editor improvements (scrolling, clipboard, suggestions, Markdown preview), driving faster development. The 'winapp' CLI preview allows simplified access to modern Windows APIs for non-VS projects, lowering the barrier for Windows API adoption from cross-platform environments.

Microsoft will present at NDC London 2026, covering .NET 10, Copilot coding, and performance, and will offer guidance for effective modernization and productivity.

- [Visual Studio January Update — Enhanced Editor Experience](https://devblogs.microsoft.com/visualstudio/visual-studio-january-update-enhanced-editor-experience/)
- [Microsoft Previews Winapp: Simplifying Access to Modern Windows APIs for Developers](https://devclass.com/2026/01/26/microsoft-previews-command-line-tool-created-because-calling-modern-windows-apis-is-too-difficult/)
- [Join Microsoft at NDC London 2026 – Let’s Build the Future of .NET Together](https://devblogs.microsoft.com/dotnet/join-us-at-ndc-london-2026/)

## DevOps

This week's DevOps updates highlight actions for critical tool deprecation, new ARM CI/CD automation, and improvements in workflow editing and collaboration. Teams should plan for deprecations and migrations, update dependency management processes, and utilize workflow enhancements for compliance and incident response.

### GitHub Platform Updates: Actions, Codespaces, and Dependency Management

GitHub is deprecating Dependabot pull request comment commands, and recommends managing PRs with the web UI, CLI, or REST APIs. Guides are provided for migrations.

Scheduled upgrades for Docker/Docker Compose in GitHub runners (Feb 9) may require updates to existing automation. Actions now include a `case` function for conditional logic, improved debug logs, and major editor workflow enhancements. Metadata support and validation are improved, and Arm64 runners are available for private Linux/Windows repos. Codespaces brings enterprise data residency options (public preview).

- [Deprecation of GitHub Dependabot Pull Request Comment Commands](https://github.blog/changelog/2026-01-27-changes-to-github-dependabot-pull-request-comment-commands)
- [Docker and Docker Compose Upgrades on GitHub Hosted Runners](https://github.blog/changelog/2026-01-30-docker-and-docker-compose-version-upgrades-on-hosted-runners)
- [GitHub Actions: Enhanced Editing, Debugging, and New Case Function](https://github.blog/changelog/2026-01-29-github-actions-smarter-editing-clearer-debugging-and-a-new-case-function)
- [GitHub Actions: Linux and Windows arm64 Runners Now Supported in Private Repositories](https://github.blog/changelog/2026-01-29-arm64-standard-runners-are-now-available-in-private-repositories)
- [GitHub Codespaces Public Preview for Enterprise Cloud with Data Residency](https://github.blog/changelog/2026-01-29-codespaces-is-now-in-public-preview-for-github-enterprise-with-data-residency)

### Kubernetes Ingress Controller Deprecation and Migration

The Kubernetes community has announced the deprecation of the Ingress NGINX controller (support ends March 2026). Teams are encouraged to migrate early, exploring Gateway API, Envoy, Traefik, Cilium, or F5 NGINX as alternatives. Feature sets differ and automatic migration is unsupported. Extensive guidance is available for this transition.

- [Kubernetes Leadership Urges Migration from Ingress NGINX Due to Security Risks and Deprecation](https://devclass.com/2026/01/29/kubernetes-leadership-warns-of-ingress-nginx-risks-but-has-also-hastened-its-deprecation/)

### Automation, Observability, and SRE Agent Integration on Azure

This week, guides show how to link Dynatrace logs and Azure deployment data by using the Azure SRE Agent with Model Context Protocol. Subagents for log analysis can reduce incident response times, and the guide covers both configuration and extensibility for on-prem or remote management with tools like Grafana and Jira Cloud.

- [Unifying Scattered Observability Data from Dynatrace and Azure for Self-Healing Deployments with SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unifying-scattered-observability-data-from-dynatrace-azure-for/ba-p/4489547)
- [How SRE Agent Bridges Grafana and Jira for Incident Automation on Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-sre-agent-pulls-logs-from-grafana-and-creates-jira-tickets/ba-p/4489527)

### Evolving DevOps Workflows: Specification-Driven and Collaborative Development

A step-by-step approach for teams looking to transition to spec-driven, collaborative workflows is provided, including templates for team contracts, CI/CD validation, artifact traceability, and architecture discussions. The guide focuses on ensuring clarity, accountability, and robust DevOps for both monolith and distributed service patterns.

- [From Vibe Coding to Spec-Driven Development: Team Collaboration, CI/CD, and Advanced Patterns](https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development-part4)

### Other DevOps News

GitHub Issues now supports semantic search for more relevant results, with an opt-out option. The latest GitHub podcast covers review management and Model Context Protocol improvements, and the Innovation Graph now provides expanded data for open source adoption and research.

- [Semantic Search Enhancement for GitHub Issues Now in Public Preview](https://github.blog/changelog/2026-01-29-improved-search-for-github-issues-in-public-preview)
- [GitHub’s Year in Review: Accessibility, Model Context Protocol, and Developer Wins]({{ "/2026-01-31-GitHubs-Year-in-Review-Accessibility-Model-Context-Protocol-and-Developer-Wins.html" | relative_url }})
- [GitHub Innovation Graph: 2025 Recap and Future Plans](https://github.blog/news-insights/policy-news-and-insights/year-recap-and-future-goals-for-the-github-innovation-graph/)

## Security

The latest security updates focus on AI-powered detection, vulnerability scanning, CodeQL language expansion, new data security controls, and enhanced authentication from Microsoft Fabric and Entra ID.

### AI-Assisted Detection Engineering and Supply Chain Protection

AI-powered detection now helps security analysts extract technical indicators from threat reports using Retrieval Augmented Generation and mitigation mapping to MITRE ATT&CK. Best practices stress deterministic prompts, structured output, and validation against golden datasets.

A vulnerability report for LangGrinch (CVE-2025-68664) in LangChain Core outlines the serialization injection risks, remediation, and how to detect and hunt for exploits using KQL and Defender for Cloud.

- [Accelerating Threat Detection Engineering with AI-Assisted TTP Extraction](https://www.microsoft.com/en-us/security/blog/2026/01/29/turning-threat-reports-detection-insights-ai/)
- [Case Study: Securing AI Application Supply Chains](https://www.microsoft.com/en-us/security/blog/2026/01/30/case-study-securing-ai-application-supply-chains/)

### Static Analysis: CodeQL 2.24.0 Expands Language Coverage and Security Capabilities

CodeQL 2.24.0 adds support for .NET 10, C# 14, and Swift 6.2.2/6.2.3. It updates JavaScript/TypeScript, Python, Java/Kotlin, C/C++/Rust, and Axum detection. Security improvements target CSRF in ASP.NET Core, more injection sinks, and better taint tracking. The release also enhances false positive reduction for current frameworks.

This builds on last week's update to CodeQL 2.23.9, reflecting the ongoing push to secure more languages and frameworks.

- [CodeQL 2.24.0 Adds .NET 10 and Swift 6.2 Support, Enhances Security Analysis](https://github.blog/changelog/2026-01-29-codeql-2-24-0-adds-swift-6-2-support-net-10-compatibility-and-file-handling-for-minified-javascript)

### Fabric Data Security: Outbound Access Protection and Workspace-Level Firewalls

Microsoft Fabric introduces preview features for workspace outbound access protection and workspace-level IP firewalls. These controls let admins restrict outbound network flows and define IP rules per workspace, providing more options for data exfiltration prevention and compliance.

- [Workspace Outbound Access Protection for Data Factory and OneLake Shortcuts (Preview)](https://blog.fabric.microsoft.com/en-US/blog/workspace-outbound-access-protection-for-data-factory/)
- [Introducing Workspace-Level IP Firewall Rules in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-workspace-level-ip-firewall-rules-in-microsoft-fabric-preview/)

### Identity Security: Passkey-Based Authentication with Entra ID

Entra ID now deploys passkey-based authentication, supporting synced and device-bound credentials for more secure access and easier administration.

- [Automatic Passkey Rollout Update for Microsoft Entra ID]({{ "/2026-01-28-Automatic-Passkey-Rollout-Update-for-Microsoft-Entra-ID.html" | relative_url }})

### Other Security News

A tutorial details how to set up two-factor authentication using TOTP codes via Azure Functions and Key Vault, guiding through secure setup, backend, and frontend for cloud-native 2FA.

A recent GitHub Podcast episode covers the Secure Open Source Fund, with maintainers sharing how funding drives security best practices, SBOM adoption, and workflow hardening. AI and Copilot are being used for advanced vulnerability detection in open source.

- [Building a TOTP Authenticator App on Azure Functions and Azure Key Vault](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-a-totp-authenticator-app-on-azure-functions-and-azure/ba-p/4489332)
- [Inside the GitHub Secure Open Source Fund: Leveling Up OSS Security]({{ "/2026-01-28-Inside-the-GitHub-Secure-Open-Source-Fund-Leveling-Up-OSS-Security.html" | relative_url }})
