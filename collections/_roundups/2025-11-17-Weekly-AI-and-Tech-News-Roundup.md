---
title: AI-Integrated Developer Tools, .NET 10 Release, and Cloud Updates Enhance Modern Workflows
author: Tech Hub Team
viewing_mode: internal
date: 2025-11-17 09:00:00 +00:00
tags:
- .NET 10
- AI Agents
- Cloud Native
- Code Review
- Enterprise Automation
- Kubernetes
- MCP
- Observability
- Prompt Engineering
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
Welcome to this week’s tech roundup, highlighting how AI-driven tools, cloud advancements, and updated productivity resources are changing the software development experience. Notable releases include extended GitHub Copilot support in Visual Studio 2026, wide deployment of .NET 10 as the new long-term support version, and new features throughout Azure, DevOps, and security platforms. As Copilot agents work more autonomously and AI model controls become more robust, development teams benefit from improved workflow oversight and flexibility. A range of tutorials and architectural changes reinforce that AI is transitioning from an optional addition to an essential part of everyday coding, reviews, and automation tasks.

At the same time, the combination of AI, machine learning, and cloud services continues to drive scalable innovation. Azure’s agent frameworks, cloud-native orchestration capabilities, and energy-efficient infrastructure support reliable, adaptable deployments. Recent updates to ML.NET and open-source climate models demonstrate how machine learning impacts broader societal needs. Security features, including adaptable cloud baselines and passwordless login, add to the week’s overall focus on intelligent, reliable, and sustainable development strategies. Check out the resources below to stay current with the latest tools, guidance, and trends shaping digital development.

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot Integrations in Visual Studio and VS Code](#github-copilot-integrations-in-visual-studio-and-vs-code)
  - [GitHub Copilot Agent Architecture and Workflow Automation](#github-copilot-agent-architecture-and-workflow-automation)
  - [Advanced AI Model Selection and Management](#advanced-ai-model-selection-and-management)
  - [Copilot-Powered Code Review and Quality Automation](#copilot-powered-code-review-and-quality-automation)
  - [Copilot for .NET, C#, and Modern Desktop](#copilot-for-net-c-and-modern-desktop)
  - [Enterprise, Security, and Administration](#enterprise-security-and-administration)
  - [Developer Productivity, Guidance, and Practical Workflows](#developer-productivity-guidance-and-practical-workflows)
- [AI](#ai)
  - [Azure AI: Infrastructure, Integration, and Operational Patterns](#azure-ai-infrastructure-integration-and-operational-patterns)
  - [.NET Ecosystem: AI Integration, Agentic Design, and Tooling](#net-ecosystem-ai-integration-agentic-design-and-tooling)
  - [Model Context Protocol (MCP) and Multimodal AI Agent Frameworks](#model-context-protocol-mcp-and-multimodal-ai-agent-frameworks)
  - [AI Agent Design and Automation Workflows](#ai-agent-design-and-automation-workflows)
  - [AI-Driven Coding, Developer Workflows, and Trends](#ai-driven-coding-developer-workflows-and-trends)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Microsoft’s Open-Source Aurora Model for Climate Forecasting](#microsofts-open-source-aurora-model-for-climate-forecasting)
  - [.NET, Aspire, and Redis: Patterns for Intelligent Agentic Workflows](#net-aspire-and-redis-patterns-for-intelligent-agentic-workflows)
  - [Modernizing Historical Datasets with ML.NET and Azure](#modernizing-historical-datasets-with-mlnet-and-azure)
- [Azure](#azure)
  - [.NET- and AI-Native Cloud Development on Azure](#net--and-ai-native-cloud-development-on-azure)
  - [Azure Kubernetes Service (AKS), Networking, and Container Platform Advances](#azure-kubernetes-service-aks-networking-and-container-platform-advances)
  - [Data and Storage Platform Releases: Microsoft Fabric, SQL, and Azure Storage](#data-and-storage-platform-releases-microsoft-fabric-sql-and-azure-storage)
  - [Observability, Monitoring, and Agentic Tooling](#observability-monitoring-and-agentic-tooling)
  - [Cost Optimization, Migration Guidance, and Cloud Economics](#cost-optimization-migration-guidance-and-cloud-economics)
  - [Platform Updates, Node.js, Python, and Storage Ecosystem Expansions](#platform-updates-nodejs-python-and-storage-ecosystem-expansions)
  - [Event Highlights: Ignite 2025, Conferences, and Learning Resources](#event-highlights-ignite-2025-conferences-and-learning-resources)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET 10 Platform Release and Ecosystem Advances](#net-10-platform-release-and-ecosystem-advances)
  - [.NET Aspire: Cloud-Native Orchestration and Distributed Workflows](#net-aspire-cloud-native-orchestration-and-distributed-workflows)
  - [Visual Studio 2026 and Developer Productivity Tools](#visual-studio-2026-and-developer-productivity-tools)
  - [AI, Automation, and API Integration in .NET](#ai-automation-and-api-integration-in-net)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [Business-Aligned SLOs and Advanced Reliability Engineering](#business-aligned-slos-and-advanced-reliability-engineering)
  - [GitOps, Pipeline Orchestration, and Scalable Multi-Cloud Delivery](#gitops-pipeline-orchestration-and-scalable-multi-cloud-delivery)
  - [Hybrid Cloud Load Balancing and Modernization with Microsoft Technologies](#hybrid-cloud-load-balancing-and-modernization-with-microsoft-technologies)
  - [Observability and Telemetry Automation](#observability-and-telemetry-automation)
  - [API-First Development and AI in the DevOps Workflow](#api-first-development-and-ai-in-the-devops-workflow)
  - [Architecture as Code and Lightweight Governance](#architecture-as-code-and-lightweight-governance)
  - [Bridging ClickOps, IaC, and AI-Driven Infrastructure](#bridging-clickops-iac-and-ai-driven-infrastructure)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Customizable Security Baselines in Azure Machine Configuration and Policy](#customizable-security-baselines-in-azure-machine-configuration-and-policy)
  - [Advanced Secrets Management and Detection Tools](#advanced-secrets-management-and-detection-tools)
  - [Secure Development with AI and Automated Code Generation](#secure-development-with-ai-and-automated-code-generation)
  - [Security Guidance and Incident Analysis for .NET and Cloud Architects](#security-guidance-and-incident-analysis-for-net-and-cloud-architects)
  - [Authentication Modernization with Passkeys and SSO](#authentication-modernization-with-passkeys-and-sso)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week’s GitHub Copilot coverage highlights expanded IDE features, maturing agent systems, new AI model options, improved enterprise features, and hands-on guidance for workflow optimization. As Copilot integrates deeper into Visual Studio, VS Code, and cloud environments, AI-based automation becomes a continuous part of daily developer tasks. Updates this week address infrastructure needs—such as security, model control, and agent configuration—while providing practical improvements for code editors, review workflows, and .NET automation. The summary below breaks down the news by technology area.

### GitHub Copilot Integrations in Visual Studio and VS Code

Building from recent advances in unified AI and agent processes, Visual Studio 2026 now features Copilot embedded for .NET 10, C#, and C++. Enhanced speed, an upgraded interface, and Copilot-powered suggestions streamline coding tasks. Regular monthly updates, continued extension compatibility, and simple upgrades from VS2022 remain consistent with trends since last November.

VS Code 1.106 continues to serve as an “AI-integrated” editor, featuring Copilot Chat with a centralized Agent HQ, more robust chats, and greater safety controls. Features such as inline chat v2 and updated session views further develop the multi-agent user experience, solidifying VS Code as a primary choice for AI-centered workflows.

Guides this week address Godot prototyping, Avalonia UI test automation, playlist-based debugging, and .NET workflow automation. Updates to C++ editing and build analysis showcase Copilot’s multi-agent capabilities—responding to the progress seen with JetBrains and Visual Studio agents last week.

- [Visual Studio 2026: Faster Performance and AI-Driven Developer Tools Now Available](https://devblogs.microsoft.com/visualstudio/visual-studio-2026-is-here-faster-smarter-and-a-hit-with-early-adopters/)
- [AI-Powered Development with GitHub Copilot in Visual Studio](/ai/videos/AI-Powered-Development-with-GitHub-Copilot-in-Visual-Studio)
- [Visual Studio Code October 2025 Release (v1.106): New AI Agents, Copilot Chat, Terminal IntelliSense & More](https://code.visualstudio.com/updates/v1_106)
- [Visual Studio Code and GitHub Copilot - What's new in 1.106](/ai/videos/Visual-Studio-Code-and-GitHub-Copilot-Whats-new-in-1106)
- [Godot for C# Developers: Leveraging AI and GitHub Copilot for Prototyping](/ai/videos/Godot-for-C-Developers-Leveraging-AI-and-GitHub-Copilot-for-Prototyping)
- [Building Rock-Solid Avalonia Apps: A Guide to Headless Testing with AI Assistance](/ai/videos/Building-Rock-Solid-Avalonia-Apps-A-Guide-to-Headless-Testing-with-AI-Assistance)
- [Transforming CI Failures into Focused Debugging with Visual Studio Playlists and AI](/ai/videos/Transforming-CI-Failures-into-Focused-Debugging-with-Visual-Studio-Playlists-and-AI)
- [AI-Powered Development with GitHub Copilot in Visual Studio](/ai/videos/AI-Powered-Development-with-GitHub-Copilot-in-Visual-Studio)
- [Refactor C++ Code with C++ Code Editing Tools for GitHub Copilot in Visual Studio](/ai/videos/Refactor-C-Code-with-C-Code-Editing-Tools-for-GitHub-Copilot-in-Visual-Studio)
- [GitHub Copilot Build Performance Analysis in Visual Studio 2026](/ai/videos/GitHub-Copilot-Build-Performance-Analysis-in-Visual-Studio-2026)

### GitHub Copilot Agent Architecture and Workflow Automation

Recent updates in Copilot agents follow last week’s advances in agent modes, CLI updates, and organizational control options. The introduction of Mission Control and Agent Sessions in VS Code allows developers to manage both automated and human-guided workflows, with features like complete logging and rationale tracking for smoother transitions between manual and automated work. Assigning responsibilities such as tech debt reduction to Copilot agents extends prior work on organization-wide instructions and pull request templates.

Setting up Copilot agents as bypass actors for GitHub rulesets brings stricter policy enforcement, while updates to `.instructions.md` and the new `excludeAgent` setting allow for more detailed customization and enhanced compliance.

- [VS Code as an AI-Native Editor: Insights from GitHub Universe 2025](/ai/videos/VS-Code-as-an-AI-Native-Editor-Insights-from-GitHub-Universe-2025)
- [Tackling Your Tech Debt with Copilot Coding Agent](/ai/videos/Tackling-Your-Tech-Debt-with-Copilot-Coding-Agent)
- [Manage Copilot Coding Agent Tasks in Visual Studio Code](https://github.blog/changelog/2025-11-13-manage-copilot-coding-agent-tasks-in-visual-studio-code)
- [How to Configure Copilot Coding Agent as a Bypass Actor for GitHub Rulesets](https://github.blog/changelog/2025-11-13-configure-copilot-coding-agent-as-a-bypass-actor-for-rulesets)
- [Agent-Specific Custom Instructions Now Supported in Copilot Code Review and Coding Agent](https://github.blog/changelog/2025-11-12-copilot-code-review-and-coding-agent-now-support-agent-specific-instructions)

### Advanced AI Model Selection and Management

Building on last week’s additions to admin controls over AI models, Copilot now introduces public preview support for OpenAI GPT-5.1 and updated Codex models. Users can choose models manually or automatically for better visibility and improved organizational policy management. Claude Sonnet 3.5 is being replaced by Claude Haiku 4.5 for Copilot Free users, and Raptor mini is in public preview, offering diverse choices to fit varying developer needs.

- [OpenAI’s GPT-5.1 Family Now Available for GitHub Copilot Users](https://github.blog/changelog/2025-11-13-openais-gpt-5-1-gpt-5-1-codex-and-gpt-5-1-codex-mini-are-now-in-public-preview-for-github-copilot)
- [Introducing Copilot Auto Model Selection (Preview)](https://devblogs.microsoft.com/visualstudio/introducing-copilot-auto-model-selection-preview/)
- [Auto Model Selection for GitHub Copilot in Visual Studio: Public Preview](https://github.blog/changelog/2025-11-11-auto-model-selection-for-copilot-in-visual-studio-in-public-preview)
- [Claude Sonnet 3.5 Deprecated, Claude Haiku 4.5 Now Available in GitHub Copilot Free](https://github.blog/changelog/2025-11-10-claude-sonnet-3-5-deprecated-claude-haiku-4-5-available-in-copilot-free)
- [Raptor Mini Model Public Preview Available in GitHub Copilot](https://github.blog/changelog/2025-11-10-raptor-mini-is-rolling-out-in-public-preview-for-github-copilot)

### Copilot-Powered Code Review and Quality Automation

Extending the recent improvements in code review automation, Copilot now features integration with CodeQL for smarter pull request suggestions, combining AI-driven insights with manual instructions. Agent handoff, GitHub Actions automation, and expanded organization-level review settings represent ongoing progress in code quality management. Support for agent-specific review instructions supports customized workflows.

- [What's New with GitHub Copilot Code Review: CodeQL, Agents & More](/ai/videos/Whats-New-with-GitHub-Copilot-Code-Review-CodeQL-Agents-and-More)
- [Mastering Copilot Code Review: Writing Effective Instructions Files](https://github.blog/ai-and-ml/unlocking-the-full-power-of-copilot-code-review-master-your-instructions-files/)
- [Agent-Specific Custom Instructions Now Supported in Copilot Code Review and Coding Agent](https://github.blog/changelog/2025-11-12-copilot-code-review-and-coding-agent-now-support-agent-specific-instructions)

### Copilot for .NET, C#, and Modern Desktop

Copilot now extends its automation skills to .NET, C#, and desktop environments. Agents support platforms like Windows Forms and .NET Aspire, continuing progress from Java and alluding to the wider role of multi-agent orchestration in modern workflows. New guides show how Copilot assists with architectural documentation and .NET modernization, supporting the wider move towards “living documentation” and cloud-native approaches for .NET teams.

- [What's New in Windows Forms: Modern Enhancements and AI Integration](/ai/videos/Whats-New-in-Windows-Forms-Modern-Enhancements-and-AI-Integration)
- [Automating .NET Aspire Architecture Documentation with GitHub Copilot](/ai/videos/Automating-NET-Aspire-Architecture-Documentation-with-GitHub-Copilot)

### Enterprise, Security, and Administration

Continuing the expansion of enterprise controls and onboarding flexibility, recent updates help organizations deploy Copilot more transparently. New privacy, intellectual property, and compliance guidance complements organizational policies and budget features, now with support for `.copilotignore` and enhanced terminal protection. Expanded video tutorials provide clear steps for onboarding, SSO configuration, billing setup, and permissions management, supporting larger team requirements.

- [Demystifying GitHub Copilot Security Controls for Organizations](https://techcommunity.microsoft.com/t5/microsoft-developer-community/demystifying-github-copilot-security-controls-easing-concerns/ba/p/4468193)
- [Tutorial Videos: Setting up GitHub Copilot for Your Company](https://devblogs.microsoft.com/all-things-azure/tutorial-videos-setting-up-github-copilot-for-your-company/)

### Developer Productivity, Guidance, and Practical Workflows

Developers gain new prompt engineering guides, detailed tutorials for context prompts, and more workflow customization strategies. Improved tools now support better writing, refactoring, testing, and modernization workflows. The updated Copilot Metrics dashboard introduces advanced analytics for adoption and ROI, replacing older approaches and supporting data-driven deployment.

Case studies on internal Copilot usage at GitHub demonstrate productivity improvements in review, configuration management, and rollout processes. This highlights the practical benefits of Copilot when paired with clear, intentional routines.

- [Prompt Engineering Techniques for Developers Using GitHub Copilot](https://dellenny.com/prompt-engineering-for-developers-getting-the-best-out-of-copilot/)
- [Practical Use Cases: Writing, Refactoring, and Testing Code with GitHub Copilot](https://dellenny.com/practical-use-cases-writing-refactoring-and-testing-code-with-github-copilot/)
- [Transforming GitHub Copilot Metrics into Business Value](/ai/videos/Transforming-GitHub-Copilot-Metrics-into-Business-Value)
- [How Copilot Helps Build the GitHub Platform](https://github.blog/ai-and-ml/github-copilot/how-copilot-helps-build-the-github-platform/)
- [Your codebase, your rules: Customizing Copilot with context engineering](/ai/videos/Your-codebase-your-rules-Customizing-Copilot-with-context-engineering)

## AI

Recent AI developments focus on improved infrastructure, integration with real-time workflows, and expanded solutions across Microsoft environments. New tutorials and workflow guides underline the ongoing incorporation of AI into developer productivity and business operations. Surveys from GitHub’s Octoverse confirm AI’s influence on programming languages, team roles, and automation. This week’s articles also prioritize secure, compliant, and sustainable scaling.

### Azure AI: Infrastructure, Integration, and Operational Patterns

Building on earlier work with containerized and edge workloads, Azure’s Fairwater AI superfactory now brings more energy-efficient GPUs and faster networking for scalable and sustainable operations. Real-time capabilities are showcased through SignalR/Key Vault integrations in Angular and .NET chat, with Entra ID authentication. Durable Task Extension in the Microsoft Agent Framework adds reliability for agent applications. These updates support the cloud-native scaling improvements from previous coverage.

SleekFlow’s deployment example illustrates Azure’s support for secure and rapid integration of AI into enterprise workflows. New resources for agent construction, AI playgrounds, and adaptive model usage enable developers to route models and orchestrate operations with greater control and efficiency.

- [Infinite Scale: Architecture of the Azure AI Superfactory](https://aka.ms/AAyjgcy)
- [Real-Time AI Streaming with Azure OpenAI and SignalR](https://techcommunity.microsoft.com/t5/microsoft-developer-community/real-time-ai-streaming-with-azure-openai-and-signalr/ba-p/4468833)
- [Building Resilient AI Agents with the Durable Task Extension for Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/bulletproof-agents-with-the-durable-task-extension-for-microsoft/ba-p/4467122)
- [How SleekFlow Uses Azure and AI to Orchestrate Enterprise Customer Conversations](https://techcommunity.microsoft.com/t5/azure-customer-innovation-blog/staying-in-the-flow-sleekflow-and-azure-turn-customer/ba-p/4467945)
- [Build Your First AI Agent with Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-your-first-ai-agent-with-azure-app-service/ba-p/4468725)
- [Introducing AI Playground on Azure App Service for Linux](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-ai-playground-on-azure-app-service-for-linux/ba-p/4469497)
- [Adaptive Model Selection with Azure AI Foundry Model Router in TypeScript](https://techcommunity.microsoft.com/t5/microsoft-developer-community/adaptive-model-selection-in-typescript-with-the-model-router/ba-p/4465192)

### .NET Ecosystem: AI Integration, Agentic Design, and Tooling

This week emphasizes .NET’s expanded support for AI, with updated abstractions, model management utilities, and design patterns provided by Semantic Kernel and Agent Framework. New releases in .NET 10, ASP.NET Core 10, MAUI 10, C# 14, and F# 10 showcase continued evolution in AI integration for language and tooling. Tutorials build on last week’s best practices, focusing on agentic structures, search, reasoning, and improved user experience in .NET. Visual Studio 2026 diagnostics and testing tools extend workflow validation paired with AI-enhanced feedback.

- [Building Intelligent Apps with .NET](/ai/videos/Building-Intelligent-Apps-with-NET)
- [AI Foundry for .NET Developers](/ai/videos/AI-Foundry-for-NET-Developers)
- [Practical AI Applications for Improved User Experience with .NET](/ai/videos/Practical-AI-Applications-for-Improved-User-Experience-with-NET)
- [.NET Diagnostic Tooling with AI](/ai/videos/NET-Diagnostic-Tooling-with-AI)
- [AI-Powered Testing in Visual Studio](/ai/videos/AI-Powered-Testing-in-Visual-Studio)
- [Understanding Agentic Development for .NET Developers](/ai/videos/Understanding-Agentic-Development-for-NET-Developers)
- [Overcoming the limitations when using AI](/ai/videos/Overcoming-the-limitations-when-using-AI)
- [Architecting an AI-Powered Sales Dashboard with .NET MAUI and Azure OpenAI](/ai/videos/Architecting-an-AI-Powered-Sales-Dashboard-with-NET-MAUI-and-Azure-OpenAI)
- [Designing Seamless AI Agents with C#: One Question, One Answer Approach](/ai/videos/Designing-Seamless-AI-Agents-with-C-One-Question-One-Answer-Approach)
- [Designing Seamless AI Agents with C#: One Question, One Answer Model](/ai/videos/Designing-Seamless-AI-Agents-with-C-One-Question-One-Answer-Model)

### Model Context Protocol (MCP) and Multimodal AI Agent Frameworks

Adoption of MCP frameworks for .NET, Java, and JetBrains continues to grow, with new resources confirming MCP’s importance for agent context-sharing and interoperability. The MMCTAgent’s Planner–Critic model further enhances multimodal AI agent reasoning—building on themes from earlier editions about plugin architectures and Azure AI Foundry.

- [Model Context Protocol (MCP) for .NET Developers](/ai/videos/Model-Context-Protocol-MCP-for-NET-Developers)
- [5 Things You Need to Know About Model Context Protocol (MCP)](/ai/videos/5-Things-You-Need-to-Know-About-Model-Context-Protocol-MCP)
- [MMCTAgent: Microsoft’s Multimodal Critical Thinking Agent for Image and Video Reasoning](https://www.microsoft.com/en-us/research/blog/mmctagent-enabling-multimodal-reasoning-over-large-video-and-image-collections/)

### AI Agent Design and Automation Workflows

Agent and workflow design resources this week delve into practical comparisons between code-first, workflow-first, and hybrid solutions for enterprise automation. The expansion of no-code agent development through Azure Logic Apps brings AI capabilities to a wider audience. Knowledge-sharing continues through Mission Agent Possible and Ignite sessions.

- [Workflow-First, Code-First, and Hybrid AI Agent Design: Approaches for Enterprise Automation](https://techcommunity.microsoft.com/t5/azure-architecture-blog/building-ai-agents-workflow-first-vs-code-first-vs-hybrid/ba-p/4466788)
- [Build AI Agents with Zero Code Using Azure Logic Apps](/ai/videos/Build-AI-Agents-with-Zero-Code-Using-Azure-Logic-Apps)
- [Designing Effective AI Agents: Insights from Armchair Architects](/ai/videos/Designing-Effective-AI-Agents-Insights-from-Armchair-Architects)
- [Mission Agent Possible: Developer AI Agent Competition at Microsoft Ignite 2025](https://techcommunity.microsoft.com/t5/microsoft-developer-community/mission-agent-possible-your-chance-to-build-solve-and-win-at/ba-p/4467585)
- [Build Custom AI Copilots Using Microsoft Copilot Studio and Oracle Database@Azure](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/build-your-own-custom-copilots-with-microsoft-copilot-studio-and/ba-p/4468211)
- [Developer-Focused Azure AI Foundry Sessions at Microsoft Ignite 2025](https://devblogs.microsoft.com/foundry/ignitedevsessions/)

### AI-Driven Coding, Developer Workflows, and Trends

Building from data on programming languages and development trends, TypeScript’s increase in usage over Python and Java is attributed to the benefits of static typing, which supports safer and more automated workflows. Team surveys emphasize that relying solely on “vibe coding” introduces risks unless balanced with solid DevOps practices and engineering discipline, maintaining a regular theme of responsible and productive AI integration.

- [How AI and TypeScript Are Shaping the Future of Software Development: Insights from GitHub Octoverse 2025](/ai/videos/How-AI-and-TypeScript-Are-Shaping-the-Future-of-Software-Development-Insights-from-GitHub-Octoverse-2025)
- [How AI Is Shaping Language Trends in Software Development: Insights from GitHub Next](https://github.blog/news-insights/octoverse/typescript-python-and-the-ai-feedback-loop-changing-software-development/)
- [How AI Coding Is Shaping Software Engineering and DevOps Roles](https://devops.com/survey-sees-ai-coding-creating-need-for-more-software-engineers/)
- [Vibe Coding vs. Spec-Driven Development: Finding Balance in the AI Era](https://devops.com/vibe-coding-vs-spec-driven-development-finding-balance-in-the-ai-era/)
- [Vibe Coding Can Create Unseen Vulnerabilities](https://devops.com/vibe-coding-can-create-unseen-vulnerabilities/)
- [AI-Driven Performance Testing: Redefining Software Quality and Engineering Roles](https://devops.com/ai-driven-performance-testing-a-new-era-for-software-quality/)

### Other AI News

Developer tool news complements Copilot Studio and agent updates, with GPT-5.1 now enabling conversational AI for direct experimentation. Continued emphasis on model routing, session management, and security best practices reflect priorities of efficiency and compliance. Migration and troubleshooting guides bring practical solutions for adoption and feature expansion.

- [GPT-5.1 Experimental Model Now Available in Microsoft Copilot Studio](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/available-now-gpt-5-1-in-microsoft-copilot-studio/)
- [Build Custom AI Copilots Using Microsoft Copilot Studio and Oracle Database@Azure](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/build-your-own-custom-copilots-with-microsoft-copilot-studio-and/ba-p/4468211)
- [How SleekFlow Uses Azure and AI to Orchestrate Enterprise Customer Conversations](https://techcommunity.microsoft.com/t5/azure-customer-innovation-blog/staying-in-the-flow-sleekflow-and-azure-turn-customer/ba-p/4467945)
- [Build Your First AI Agent with Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-your-first-ai-agent-with-azure-app-service/ba-p/4468725)
- [Introducing AI Playground on Azure App Service for Linux](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-ai-playground-on-azure-app-service-for-linux/ba-p/4469497)
- [Learning From the Past: What Automation Mistakes Can Teach Us About AI](https://devops.com/learning-from-the-past-what-automation-mistakes-can-teach-us-about-ai/)

## ML

Machine learning highlights this week include new open-source models, practical cloud integration examples, and inventive applications for research and cultural projects. Microsoft furthered its open climate research and shared patterns for agent workflows and legacy dataset modernization.

### Microsoft’s Open-Source Aurora Model for Climate Forecasting

Microsoft debuted the Aurora project to expand access to climate and weather modeling—an open-source foundation trained on broad atmospheric datasets for predicting waves, air quality, and extreme weather. Code, model weights, and pipeline plans are available, making it easier for developers to offer both localized and large-scale forecasts. Built through partnerships including Cambridge’s Rich Turner lab and built atop efforts like SPARROW, Aurora’s public APIs make it a useful resource for energy management, disaster response, and environmental analysis by reducing the technical hurdles for entry.

- [Aurora: Microsoft’s Open-Source AI Model for Weather and Climate Forecasting](https://blogs.microsoft.com/on-the-issues/2025/11/13/the-next-phase-of-aurora-open-and-collaborative-ai-for-weather-and-climate-forecasting/)

### .NET, Aspire, and Redis: Patterns for Intelligent Agentic Workflows

Detailed coverage of .NET Aspire, Redis, and the Microsoft Agent Framework shows how to build robust, scalable agent systems. Redis enables semantic caching, vector storage, and management of session state, aligning with the trend toward persistent, distributed agent architectures. All updates utilize the new features in .NET 10, C# 14, F# 10, and Visual Studio 2026, reinforcing the focus on modular and multi-agent workflow strategies.

- [Build Smarter Agents with Redis, .NET Aspire, and Microsoft Agent Framework](/ai/videos/Build-Smarter-Agents-with-Redis-NET-Aspire-and-Microsoft-Agent-Framework)

### Modernizing Historical Datasets with ML.NET and Azure

ML.NET and Azure CosmosDb are used this week to modernize a 17th-century Italian-English dictionary. Developers leverage current .NET and ML.NET features for processing legacy data—including custom vector embeddings and scalable cloud storage. These updates enable robust semantic search and reliable API endpoints, demonstrating practical uses of Microsoft’s ML tools in both research and preservation settings.

- [Modernizing a 17th Century Italian-English Dictionary with .NET and ML.NET](/azure/videos/Modernizing-a-17th-Century-Italian-English-Dictionary-with-NET-and-MLNET)

## Azure

Azure announcements include new guidance for .NET apps, expanded data platforms, improved monitoring, workflow automation, and continuing advancements to the platform stack. Developers can access new resources for container orchestration (AKS), multi-cloud data systems, cost management, and modernization with enhanced support for open standards and diagnostics.

### .NET- and AI-Native Cloud Development on Azure

Recent updates reinforce .NET and Azure integration, with new guides for Aspire 13, ASP.NET Core 10, .NET MAUI 10, and modern language support. The .NET 10 and Azure convergence continues to drive event-driven and AI-enabled architectures using Container Apps, AKS, and Azure Functions.

The focus on updating legacy .NET workloads carries into this week, using efficient migration strategies and new mesh patterns for API scalability. The built-in Model Context Protocol (MCP) support with Visual Studio 2026 deepens the connection between DevOps and developer experience. Azure App Service brings improved pipeline integration, diagnostics, and security features for .NET teams.

- [Azure Keynote: Cloud Innovation with .NET](/ai/videos/Azure-Keynote-Cloud-Innovation-with-NET)
- [Modernizing .NET Applications for the Cloud](/azure/videos/Modernizing-NET-Applications-for-the-Cloud)
- [Building Remote MCP Servers with .NET and Azure](/ai/videos/Building-Remote-MCP-Servers-with-NET-and-Azure)
- [Azure MCP Server Now Built-In with Visual Studio 2026: Agentic Cloud Automation for Developers](https://devblogs.microsoft.com/visualstudio/azure-mcp-server-now-built-in-with-visual-studio-2026-a-new-era-for-agentic-workflows/)
- [What's New in Azure App Service for .NET Developers](/azure/videos/Whats-New-in-Azure-App-Service-for-NET-Developers)

### Azure Kubernetes Service (AKS), Networking, and Container Platform Advances

AKS maintains its focus on multi-tenancy, security, and workload isolation, with fresh resources for architecture, cost tracking, and RBAC setup. Namespace showback and autoscaling help operationalize large clusters, building on the need for granular controls.

Azure networking advances—like Virtual Network Manager and improved WAN routing—further mesh networking and security for distributed services. IPv6 walkthroughs round out guidance for modernization.

- [Building Enterprise-Grade Shared AKS Clusters: A Guide to Multi-Tenant Kubernetes Architecture](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-enterprise-grade-shared-aks-clusters-a-guide-to-multi/ba/p/4468563)
- [Unlocking Direct Spoke Communication with Azure Virtual Network Manager and Virtual WAN](https://techcommunity.microsoft.com/t5/azure-networking-blog/azure-virtual-network-manager-azure-virtual-wan/ba-p/4469991)
- [Delivering Web Applications Over IPv6 Using Azure Networking Services](https://techcommunity.microsoft.com/t5/azure-networking-blog/delivering-web-applications-over-ipv6/ba-p/4469638)

### Data and Storage Platform Releases: Microsoft Fabric, SQL, and Azure Storage

Microsoft Fabric Warehouse Snapshots have moved to general availability, supporting reproducible analytics and ML projects. SQLCon presents hybrid and AI-driven data management strategies, with more information on SQL Server, Azure SQL, and T-SQL Copilot.

The public preview of Dell PowerScale on Azure strengthens unstructured data and disaster recovery support. Azure Storage now offers enhanced region replication and improved geo-redundancy, supporting compliance and durability for distributed workloads.

- [Warehouse Snapshots Now Generally Available in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/warehouse-snapshots-in-microsoft-fabric-freeze-data-unlock-reliable-reporting/)
- [Announcing SQLCon: The Microsoft SQL Community Conference](https://blog.fabric.microsoft.com/en-US/blog/its-time-announcing-the-microsoft-sql-community-conference/)
- [Public Preview: Azure Native Dell PowerScale Now Available](https://techcommunity.microsoft.com/t5/azure-storage-blog/public-preview-of-azure-native-dell-powerscale/ba-p/4470120)
- [Priority Replication Now Generally Available for Azure GRS and Object Replication](https://techcommunity.microsoft.com/t5/azure-storage-blog/priority-replication-for-geo-redundant-storage-and-object/ba-p/4468607)

### Observability, Monitoring, and Agentic Tooling

Azure Monitor adds public preview support for PromQL query-based metric alerts, aligning with broader adoption of standards-driven monitoring. The Azure Copilot Observability Agent now enables AI-guided cloud investigations, supporting improved AI-powered diagnostics. The adoption of OpenTelemetry sidecar extensions continues to grow for PHP, Python, Node.js, and .NET environments.

- [Public Preview: Query-Based Metric Alerts Now in Azure Monitor](https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-public-preview-of-query-based-metric-alerts-in-azure/ba-p/4469290)
- [Azure Copilot Observability Agent: AI-Driven Investigations for Cloud Operations](https://techcommunity.microsoft.com/t5/azure-observability-blog/azure-copilot-observability-agent-intelligent-investigations/ba-p/4467360)
- [OTEL Sidecar Extension Cheat-Sheet on Azure App Service for Linux: PHP, Python, Node.js, .NET](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-iii-otel-sidecar-extension-on-azure-app-service-for-linux/ba-p/4469589)
- [Part I: OTEL Sidecar Extension on Azure App Service for Linux – PHP Integration Guide](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-i-otel-sidecar-extension-on-azure-app-service-for-linux/ba-p/4469514)
- [Elastic APM Setup with OTEL Sidecar Extension on Azure App Service for Linux](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-ii-otel-sidecar-extension-on-azure-app-service-for-linux/ba-p/4469576)

### Cost Optimization, Migration Guidance, and Cloud Economics

Databricks cost optimization content provides practical guidance for resource management, echoing recent updates to cloud migration and cost tracking tools. Azure Migrate’s enhancements now support improved mapping and phased application planning. Updated pricing calculators and model guides help teams plan and manage costs for dynamic workloads.

- [Azure Databricks Cost Optimization: A Practical Guide](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/azure-databricks-cost-optimization-a-practical-guide/ba-p/4470235)
- [Migrate or Modernize Applications Using Azure Migrate: New Features and Application-Aware Approaches](https://techcommunity.microsoft.com/t5/azure-migration-and/migrate-or-modernize-your-applications-using-azure-migrate/ba-p/4468587)
- [Azure Cost Estimation: Practical Strategies and Tools Explained](/azure/videos/Azure-Cost-Estimation-Practical-Strategies-and-Tools-Explained)
- [Azure Pricing Models Explained: Pay-As-You-Go, Reserved, and Spot Instances](https://dellenny.com/azure-pricing-models-explained-pay-as-you-go-reserved-and-spot-instances/)
- [Using the Azure Pricing Calculator for Accurate Cloud Cost Estimates](https://dellenny.com/how-to-use-the-azure-pricing-calculator-effectively-a-step-by%e2%80%90step-guide/)

### Platform Updates, Node.js, Python, and Storage Ecosystem Expansions

Latest platform news includes Node.js 24 LTS on Azure App Service, new deployment tooling for Python, and updates to existing GitHub Actions pipelines. Azure NetApp Files upgraded scaling and backup features, and AKS node improvements make it easier to run the latest .NET 10 workloads.

- [Node.js 24 Now Available on Azure App Service for Linux](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/node-js-24-is-now-available-on-azure-app-service-for-linux/ba/p/4468801)
- [What’s New for Python on Azure App Service for Linux: pyproject.toml, uv, and Modern Deployments](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-for-python-on-app-service-for-linux-pyproject-toml-uv/ba-p/4468903)
- [Accelerating HPC and EDA Innovation with Azure NetApp Files Enhancements](https://techcommunity.microsoft.com/t5/azure-architecture-blog/accelerating-hpc-and-eda-with-powerful-azure-netapp-files/ba-p/4469739)
- [Azure Update - 14th November 2025](/ai/videos/Azure-Update-14th-November-2025)

### Event Highlights: Ignite 2025, Conferences, and Learning Resources

Event coverage this week expands on previous Ignite previews, featuring guides for customizing event schedules and promoting greater community engagement on the Azure platform.

- [Top Microsoft Ignite 2025 Sessions for Azure Developers: AI, Copilot, and Fabric](https://techcommunity.microsoft.com/t5/azure-events/build-your-ignite-schedule-top-sessions-for-developers/ba-p/4469064)
- [Your Guide to Azure Compute at Microsoft Ignite 2025](https://techcommunity.microsoft.com/t5/azure-compute-blog/your-guide-to-azure-compute-at-microsoft-ignite-2025/ba-p/4468633)
- [Announcing SQLCon: The Microsoft SQL Community Conference](https://blog.fabric.microsoft.com/en-US/blog/its-time-announcing-the-microsoft-sql-community-conference/)

### Other Azure News

Automation, platform modernization, and migration planning remain in focus, with tools and resources designed to make transitioning to Azure approachable. Security, compliance, and troubleshooting features—such as WinRE for Azure Arc and SLAs—reinforce platform improvements and readiness for resilient deployments.

- [Announcing BizTalk Server 2020 Cumulative Update 7: Platform Support and Upgrade Guidance](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-the-biztalk-server-2020-cumulative-update-7/ba/p/4469100)
- [Provisioning SQL Server 2025 Databases to Kubernetes with Pure Storage Snapshots](/azure/videos/Provisioning-SQL-Server-2025-Databases-to-Kubernetes-with-Pure-Storage-Snapshots)
- [Modernizing WPF Map Functionality with Azure Maps: A Practical Migration Path](https://techcommunity.microsoft.com/t5/azure-maps-blog/guest-blog-modernizing-your-wpf-app-maps-functionality-with/ba/p/4468755)
- [Strengthen Server Resilience: Enabling WinRE for Windows Server with Azure Arc](https://www.thomasmaurer.ch/2025/11/strengthen-server-resilience-windows-recovery-environment-winre-for-windows-server-with-azure-arc/)
- [Migrate or Modernize Applications Using Azure Migrate: New Features and Application-Aware Approaches](https://techcommunity.microsoft.com/t5/azure-migration-and/migrate-or-modernize-your-applications-using-azure-migrate/ba-p/4468587)
- [Accelerate Cloud Migration with Wave Planning in Azure Migrate](https://techcommunity.microsoft.com/t5/azure-migration-and/accelerate-cloud-migration-with-wave-planning-in-azure-migrate/ba-p/4467959)
- [Calculating Expiry and Retention Days for Azure Blob Storage Using Inventory and Synapse](https://techcommunity.microsoft.com/t5/azure-paas-blog/deriving-expiry-days-and-remaining-retention-days-for-blobs/ba-p/4466586)
- [Understanding Azure SLAs: What 99.9% Really Means](https://dellenny.com/understanding-azure-slas-what-99-9-really-means/)
- [Azure Governance Tools: Policies, Blueprints, and RBAC Explained](https://dellenny.com/azure-governance-tools-policies-blueprints-and-role-based-access-control-rbac/)
- [Intermittent Access Issue: Azure Function App Fails to Retrieve Key Vault Secrets via Private Endpoint](https://techcommunity.microsoft.com/t5/azure-infrastructure/intermittent-access-issue-between-azure-function-app-and-key/m-p/4468948#M316)
- [Deploying AI Solutions with Azure AI Landing Zones](/ai/videos/Deploying-AI-Solutions-with-Azure-AI-Landing-Zones)
- [Top 5 Cloud Design Principles for Architects](/azure/videos/Top-5-Cloud-Design-Principles-for-Architects)
- [Microsoft Announces Fairwater Datacenter: Building an AI Superfactory with Azure](https://www.linkedin.com/posts/satyanadella_today-we-announced-our-new-fairwater-datacenter-activity-7394420047294648321-RQ77)
- [Sovereign and Adaptive Cloud: Microsoft Ignite 2025 Highlights](https://www.thomasmaurer.ch/2025/11/driving-impact-in-the-era-of-ai-sovereign-cloud-adaptive-cloud-at-microsoft-ignite-2025/)
- [Join Microsoft at SC25: HPC and AI Innovations in St. Louis](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/join-microsoft-sc25-experience-hpc-and-ai-innovation/ba-p/4467935)

## Coding

This week’s .NET announcements spotlight the general availability of .NET 10, progress in the ecosystem, improved developer tools, advanced security measures, container support, and better workflow orchestration. Community efforts continue to highlight practical migration paths, inclusive development resources, and hands-on learning for modern .NET projects.

### .NET 10 Platform Release and Ecosystem Advances

The .NET 10 release marks the new long-term support baseline through 2028, unifying ASP.NET Core, Blazor, MAUI, EF Core 10, C# 14, and F# 10 under one production-ready umbrella. Updates like a new JIT compiler, enhanced NativeAOT, quantum-resistant crypto features, and improved NuGet package security expand on performance and reliability themes. Model Context Protocol (MCP) support and agent frameworks are now central parts of the platform.

Blazor and ASP.NET Core updates—plus OpenAPI 3.1 and improved authentication—continue the push toward modern web capabilities. .NET Aspire strengthens orchestration and Azure deployment options, while Visual Studio 2026 and the enhanced CLI increase productivity and workflow efficiency.

- [Announcing .NET 10: A Major Release for Modern, Productive, and AI-Powered Development](https://devblogs.microsoft.com/dotnet/announcing-dotnet-10/)
- [.NET 10 Migration and New Features for Enterprise Blazor Applications](/coding/videos/NET-10-Migration-and-New-Features-for-Enterprise-Blazor-Applications)
- [Generating Full-Stack .NET Apps with Uno Platform and AI](/ai/videos/Generating-Full-Stack-NET-Apps-with-Uno-Platform-and-AI)
- [.NET Framework 4.8 to .NET 9 Step by Step Migration Guide](/coding/videos/NET-Framework-48-to-NET-9-Step-by-Step-Migration-Guide)
- [What's New in NuGet: Security, AI, and Modern Package Management for .NET](/ai/videos/Whats-New-in-NuGet-Security-AI-and-Modern-Package-Management-for-NET)
- [Welcome to .NET 10 & Visual Studio 2026!](/coding/videos/Welcome-to-NET-10-and-Visual-Studio-2026)
- [Performance Improvements in .NET 10](/coding/videos/Performance-Improvements-in-NET-10)
- [Build Better Web Apps with Blazor in .NET 10](/coding/videos/Build-Better-Web-Apps-with-Blazor-in-NET-10)
- [Modern C# Features to Enhance Your Coding Habits](/coding/videos/Modern-C-Features-to-Enhance-Your-Coding-Habits)
- [What's New in C# 14](/coding/videos/Whats-New-in-C-14)
- [What's New in ASP.NET Core for .NET 10](/coding/videos/Whats-New-in-ASPNET-Core-for-NET-10)
- [What's New in Containers for .NET 10](/azure/videos/Whats-New-in-Containers-for-NET-10)
- [What's New in .NET MAUI](/coding/videos/Whats-New-in-NET-MAUI)
- [Clean Architecture with ASP.NET Core 10](/coding/videos/Clean-Architecture-with-ASPNET-Core-10)
- [Ship Faster with .NET MAUI: Real-World Pitfalls and Solutions](/coding/videos/Ship-Faster-with-NET-MAUI-Real-World-Pitfalls-and-Solutions)
- [Modern Windows Development with .NET](/coding/videos/Modern-Windows-Development-with-NET)
- [New dotnet test Experience with Microsoft.Testing.Platform](/coding/videos/New-dotnet-test-Experience-with-MicrosoftTestingPlatform)
- [DataMountain: .NET Data Warehousing That Beats C++ Performance](/coding/videos/DataMountain-NET-Data-Warehousing-That-Beats-C-Performance)
- [OpenAPI.NET v2 & v3 Major Release: The Biggest Update Ever](https://devblogs.microsoft.com/openapi/openapi-net-release-announcements/)
- [Simplifying .NET with 'dotnet run file.cs'](/coding/videos/Simplifying-NET-with-dotnet-run-filecs)
- [.NET and .NET Framework November 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-november-2025-servicing-updates/)

### .NET Aspire: Cloud-Native Orchestration and Distributed Workflows

Aspire remains central for .NET orchestration, with enhanced documentation and cross-language compatibility in hybrid environments. Updated resources detail configuration for Python, Node.js, and non-.NET applications, keeping modular and multi-team architectures in focus. New resources, dashboards, and onboarding solutions support multi-repo and enterprise scenarios.

- [Simplifying Cloud-Native Development with .NET Aspire](/azure/videos/Simplifying-Cloud-Native-Development-with-NET-Aspire)
- [Taking .NET out of .NET Aspire: Working with Non-.NET Applications](/coding/videos/Taking-NET-out-of-NET-Aspire-Working-with-Non-NET-Applications)
- [Deep Dive: Extending and Customizing Aspire](/azure/videos/Deep-Dive-Extending-and-Customizing-Aspire)
- [Aspire Unplugged: David Fowler Discusses .NET Aspire’s Vision and Future](/azure/videos/Aspire-Unplugged-David-Fowler-Discusses-NET-Aspires-Vision-and-Future)
- [Windows 365 Meets .NET Aspire: Boosting Multi-Repo Microservice Productivity](/coding/videos/Windows-365-Meets-NET-Aspire-Boosting-Multi-Repo-Microservice-Productivity)

### Visual Studio 2026 and Developer Productivity Tools

The Visual Studio 2026 preview introduces a new user experience, accessibility improvements, and expanded customization options, reinforcing the focus on productivity. Integrated AI features and support for .NET 10 enhance debugging and profiling tools for practical application.

- [A First Look at the Refreshed UX in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/a-first-look-at-the-all%e2%80%91new-ux-in-visual-studio-2026/)
- [Visual Studio Debugger: Advanced Techniques](/ai/videos/Visual-Studio-Debugger-Advanced-Techniques)
- [Real-World .NET Profiling with Visual Studio](/coding/videos/Real-World-NET-Profiling-with-Visual-Studio)

### AI, Automation, and API Integration in .NET

AI workflow guidance and MCP/api integration themes continue this week, with practical tutorials for retrofitting APIs, using MCP via NuGet, and enhancing productivity through scripting. These updates expand prototyping and automation efforts highlighted previously.

- [Enhancing Existing .NET REST APIs with Model Creation Protocol (MCP) and AI](/ai/videos/Enhancing-Existing-NET-REST-APIs-with-Model-Creation-Protocol-MCP-and-AI)
- [What's New in NuGet: Security, AI, and Modern Package Management for .NET](/ai/videos/Whats-New-in-NuGet-Security-AI-and-Modern-Package-Management-for-NET)
- [Simplifying .NET with 'dotnet run file.cs'](/coding/videos/Simplifying-NET-with-dotnet-run-filecs)

### Other Coding News

.NET Community Toolkits and the latest MAUI/Windows releases integrate the new .NET launches. Coverage on decision records, Rx.NET status, and performance frameworks (DataMountain, terrain simulation) stays relevant for modular, high-performance engineering. Sustainability and real-world learning feature with topics like MAUI troubleshooting, carbon-aware computing, F# adoption, and IoT solutions with Raspberry Pi. .NET Foundation status and open source initiatives remain visible.

- [Exploring the .NET Community Toolkits: MAUI, Windows, and More](/coding/videos/Exploring-the-NET-Community-Toolkits-MAUI-Windows-and-More)
- [Decision Records: Understanding Why Those Decisions Were Made in .NET](/coding/videos/Decision-Records-Understanding-Why-Those-Decisions-Were-Made-in-NET)
- [Rx.NET Status and Roadmap Overview](/coding/videos/RxNET-Status-and-Roadmap-Overview)
- [DataMountain: .NET Data Warehousing That Beats C++ Performance](/coding/videos/DataMountain-NET-Data-Warehousing-That-Beats-C-Performance)
- [High-Performance Terrain Simulations in .NET](/coding/videos/High-Performance-Terrain-Simulations-in-NET)
- [Ship Faster with .NET MAUI: Real-World Pitfalls and Solutions](/coding/videos/Ship-Faster-with-NET-MAUI-Real-World-Pitfalls-and-Solutions)
- [Carbon Aware Computing with .NET Open Source Libraries for Sustainable Applications](/azure/videos/Carbon-Aware-Computing-with-NET-Open-Source-Libraries-for-Sustainable-Applications)
- [Smatterings of F#: Integrating F# in a C#-Focused World](/coding/videos/Smatterings-of-F-Integrating-F-in-a-C-Focused-World)
- [Understanding Nullable Reference Types in .NET](/coding/videos/Understanding-Nullable-Reference-Types-in-NET)
- [Building Modern CLI Apps in .NET: Libraries, Patterns, and Packaging](/coding/videos/Building-Modern-CLI-Apps-in-NET-Libraries-Patterns-and-Packaging)
- [Real-World .NET Profiling with Visual Studio](/coding/videos/Real-World-NET-Profiling-with-Visual-Studio)
- [If .NET Brewed Beer: IoT Brewing Automation with Raspberry Pi](/coding/videos/If-NET-Brewed-Beer-IoT-Brewing-Automation-with-Raspberry-Pi)
- [The Future of Python and AI with Guido van Rossum](/ai/videos/The-Future-of-Python-and-AI-with-Guido-van-Rossum)
- [State of the .NET Foundation and Advances in .NET Open Source](/coding/videos/State-of-the-NET-Foundation-and-Advances-in-NET-Open-Source)

## DevOps

This week, DevOps teams addressed complex automation requirements by focusing on business impact, improved reliability, and distributed cloud environments. New methods link SLOs to actual business value, support scalable pipeline orchestration, streamline onboarding, and expand observability—all moving towards continuous improvement and reduced hands-on effort.

### Business-Aligned SLOs and Advanced Reliability Engineering

Guidance moves beyond legacy SLO metrics, encouraging dashboards that tie performance indicators to end-user tiers and financial goals. For large-scale deployments, context-aware reliability models adjust resource allocation by customer segment, helping lower costs and improve outcomes. Practical blueprints and case studies show step-by-step ways to implement these methods.

- [Why Your SLO Dashboard Is Lying: Building Business-Aligned Service Level Objectives](https://devops.com/why-your-slo-dashboard-is-lying-moving-beyond-vanity-metrics-in-production/)
- [Context-Aware Reliability Contracts: Rethinking SLOs for Hyperscale DevOps](https://devops.com/why-traditional-slos-are-failing-at-hyperscale-building-context-aware-reliability-contracts/)

### GitOps, Pipeline Orchestration, and Scalable Multi-Cloud Delivery

Articles highlight multi-cloud adoption with advanced GitOps—featuring tools like ArgoCD, Flux, and policy-as-code—to meet regulatory requirements. Developers can now use Cake SDK to orchestrate GitHub Actions pipelines in C#, offering alternatives to YAML/JSON. Additions to OIDC token claims for GitHub Actions further strengthen security by enabling more granular identity controls.

- [Scaling GitOps for Continuous Delivery in Hybrid and Multi-Cloud Environments](https://devops.com/gitops-in-the-wild-scaling-continuous-delivery-in-hybrid-cloud-environments/)
- [Orchestrating GitHub Actions Pipelines in C# with Cake SDK](/coding/videos/Orchestrating-GitHub-Actions-Pipelines-in-C-with-Cake-SDK)
- [GitHub Actions OIDC Token Claims Now Include check_run_id](https://github.blog/changelog/2025-11-13-github-actions-oidc-token-claims-now-include-check_run_id)

### Hybrid Cloud Load Balancing and Modernization with Microsoft Technologies

Practical advice covers load balancing across Kubernetes, containers, and virtual machines, including detailed YARP setup for authentication and multicluster routing. Steps for safely refactoring legacy environments are shared, and new onboarding automation with Aspire and Visual Studio aims to streamline compliance and reduce ramp-up time.

- [Multicluster Load Balancing with YARP: From Kubernetes to Legacy Containers](/coding/videos/Multicluster-Load-Balancing-with-YARP-From-Kubernetes-to-Legacy-Containers)
- [How to Refactor Legacy Solution Architectures Without Breaking Everything](https://dellenny.com/how-to-refactor-legacy-solution-architectures-without-breaking-everything/)
- [Automating Engineer Onboarding with .NET Aspire and Visual Studio](/coding/videos/Automating-Engineer-Onboarding-with-NET-Aspire-and-Visual-Studio)

### Observability and Telemetry Automation

Bindplane’s Blueprints and Fleets automate deployment of OpenTelemetry collectors, promoting standardized and scalable observability for modern architectures, while increasing integration with AI-readiness.

- [Bindplane Adds Ability to Automate Deployment of OpenTelemetry Collectors](https://devops.com/bindplane-adds-ability-to-automate-deployment-of-opentelemetry-collectors/)

### API-First Development and AI in the DevOps Workflow

API governance is highlighted, with recommendations for integrating best practices into the CI/CD workflow. New research explores the organizational challenges of migrating to new platforms and combining AI tools, stressing the need for continuous adaptation and analytical insight.

- [The DevOps Impact of API-First Development](https://devops.com/the-devops-impact-of-api-first-development/)
- [Survey Reveals Challenges in DevOps Platform Migrations and AI Tool Integration](https://devops.com/survey-surfaces-multiple-devops-platform-migration-challenges/)

### Architecture as Code and Lightweight Governance

Updated perspectives on “Architecture as Code” support automated, version-controlled, and decentralized governance models, which help maintain clear and traceable architectural decisions.

- [Architecture as Code: Practical Approaches and Benefits](https://dellenny.com/architecture-as-code-what-it-means-and-how-to-apply-it/)
- [Architecture Decision Records (ADRs): A Lightweight Governance Model for Modern Teams](https://dellenny.com/architecture-decision-records-adrs-a-lightweight-governance-model-for-software-architecture/)

### Bridging ClickOps, IaC, and AI-Driven Infrastructure

Combined approaches to ClickOps, Infrastructure-as-Code (IaC), and AI-powered interfaces help organizations achieve fast prototyping without sacrificing policy compliance. Session locking, drift detection, and enhanced integrations maintain the momentum on compliant DevOps orchestration.

- [Bridging the Gap Between ClickOps and Infrastructure-as-Code in DevOps](https://devops.com/clickops-iac-and-the-excluded-avocado-middle/)

### Other DevOps News

GitHub Enterprise Cloud introduces new data residency options for Visual Studio subscribers. The October 2025 Availability Report provides transparency on service incidents, detailing mitigation responses and lessons for platform improvements.

- [GitHub Enterprise Cloud Data Residency Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-11-10-github-enterprise-cloud-with-data-residency-now-supports-visual-studio-subscriptions)
- [GitHub Availability Report: October 2025 – Analysis of Service Incidents](https://github.blog/news-insights/company-news/github-availability-report-october-2025/)

## Security

Security updates address cloud and DevSecOps needs, focusing on AI-enabled risk management, compliance, and tightening integration into developer workflows. Key progress includes customizable baselines, updated secret scanning, enhanced AI detection, targeted incident analysis, and secure authentication guides.

### Customizable Security Baselines in Azure Machine Configuration and Policy

Azure now offers customizable security benchmarks, letting organizations modify or skip controls (CIS, Microsoft Compute Security) for Windows and Linux platforms. Developers define policies in JSON and apply them through ARM, CLI, Bicep, or CI/CD, with coverage for hybrid/multicloud via Azure Policy. Guides and tutorials clarify permissions and deployment for easier, code-based compliance. The feature is now available in public cloud regions, with government and sovereign support coming soon.

- [Customizable Security Baselines in Azure Machine Configuration: Public Preview](https://techcommunity.microsoft.com/t5/azure-governance-and-management/public-preview-introducing-customizable-security-baseline/ba/p/4469678)
- [Customizable Security Baselines Now in Preview for Azure Policy and Machine Configuration](https://techcommunity.microsoft.com/t5/azure-governance-and-management/introducing-customizable-security-baselines-in-azure-policy-and/ba/p/4469678)

### Advanced Secrets Management and Detection Tools

Improvements to secret scanning at GitHub include better private key detection and refined Sentry token alerts. New security research studies highlight how interconnected AI/dev workflows can create secret sprawl, increasing risk even further. Guidance stresses pre-commit scanning and developer diligence as essential strategies.

- [Secret Scanning Enhancements: Improved Private Key Detection and Sentry Token Updates](https://github.blog/changelog/2025-11-12-secret-scanning-improves-private-key-detection)
- [How Hyperconnected AI Development Creates a Multi-System Secret Sprawl](https://devops.com/how-hyperconnected-ai-development-creates-a-multi-system-secret-sprawl/)

### Secure Development with AI and Automated Code Generation

JFrog now supports detection of AI-generated code and Shadow AI, making it easier to track usage, licensing, and potential risks from unapproved tools. Microsoft’s BlueCodeAgent combines automated red teaming and defense rules to proactively detect LLM vulnerabilities and code bias, furthering best practices in safe AI integration.

- [JFrog Adds AI-Generated Code Detection to Secure Software Supply Chains](https://devops.com/jfrog-adds-ability-to-track-usage-of-ai-coding-tools/)
- [BlueCodeAgent: AI-Powered Blue Teaming for Secure Code Generation](https://www.microsoft.com/en-us/research/blog/bluecodeagent-a-blue-teaming-agent-enabled-by-automated-red-teaming-for-codegen-ai/)

### Security Guidance and Incident Analysis for .NET and Cloud Architects

A review of .NET security case studies provides detailed examples of common weaknesses and offers up-to-date patterns using .NET 10, Aspire, ASP.NET Core, and Visual Studio 2026. Further resources examine Microsoft’s security model, with specific advice on encryption, identity, monitoring, and compliance—delivering clear, actionable recommendations for developers and architects.

- [A Year in .NET Security: Lessons from MSRC Cases (2024–2025)](/coding/videos/A-Year-in-NET-Security-Lessons-from-MSRC-Cases-20242025)
- [How Microsoft Keeps Your Data Safe in the Cloud – A Deep Dive into Cloud Security Practices](https://dellenny.com/how-microsoft-keeps-your-data-safe-in-the-cloud-a-deep-dive-into-cloud-security-practices/)

### Authentication Modernization with Passkeys and SSO

Resources show how to add WebAuthn/passkey authentication options (Windows Hello, TouchID, hardware security keys) into ASP.NET Core, along with custom SSO guides using OpenIddict for improved central identity management. These updates simplify and modernize authentication approaches for business and enterprise development.

- [Going Passwordless: Implementing Passkeys in ASP.NET Core](/coding/videos/Going-Passwordless-Implementing-Passkeys-in-ASPNET-Core)
- [Rolling Your Own SSO: Centralized Authentication with OpenIddict](/coding/videos/Rolling-Your-Own-SSO-Centralized-Authentication-with-OpenIddict)

### Other Security News

The November update for Azure DevOps Server 2022.2 shifts TFVC Proxy hashing to SHA-256 and fixes build reliability, with guides for patching and validation.

- [November Security Patches Released for Azure DevOps Server](https://devblogs.microsoft.com/devops/november-patches-for-azure-devops-server-2/)

Microsoft’s latest Secure Future Initiative progress report details changes in environment configuration, hardware trust, AI lifecycle security, and broader use of MFA/passkey and live secret detection. This aligns with ongoing investment in cloud and AI security practices.

- [November 2025 Progress Report on Microsoft’s Secure Future Initiative](https://www.microsoft.com/en-us/security/blog/2025/11/10/securing-our-future-november-2025-progress-report-on-microsofts-secure-future-initiative/)

Coverage of server-side request forgery (SSRF) examines mechanics, risks, and practical steps to limit the attack surface, continuing the focus on up-to-date threat analysis and real-world defense strategies.

- [Why Server-Side Request Forgery (SSRF) Is a Top Cloud Security Concern](/security/videos/Why-Server-Side-Request-Forgery-SSRF-Is-a-Top-Cloud-Security-Concern)
