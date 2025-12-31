---
layout: "post"
title: "Updates in AI Coding, Azure Features, and Secure DevOps for Modern Teams"
description: "This week’s technology roundup covers new features and improvements in AI-powered coding, cloud infrastructure, DevOps workflows, and data security. Highlights include GitHub Copilot’s open source expansion, support for multiple AI models, and agentic automation, plus Azure’s upgraded virtual machines, storage management, and Logic Apps for automation. Articles focus on tools that enable reliable, secure, and flexible development, deeper governance and monitoring, and adapting best practices for evolving compliance requirements."
author: "Tech Hub Team"
excerpt_separator: <!--excerpt_end-->
viewing_mode: "internal"
date: 2025-09-15 09:00:00 +00:00
permalink: "/roundups/2025-09-15-Weekly-AI-and-Tech-News-Roundup.html"
categories: ["AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security"]
tags: ["AI", "AI Coding", "AI Infrastructure", "Azure", "CI/CD", "Cloud Computing", "Coding", "Data Management", "DevOps", "Enterprise Apps", "GitHub Copilot", "Low Code Automation", "Machine Learning", "ML", "Observability", "Open Source", "Roundups", "Security", "VS Code"]
tags_normalized: ["ai", "ai coding", "ai infrastructure", "azure", "cislashcd", "cloud computing", "coding", "data management", "devops", "enterprise apps", "github copilot", "low code automation", "machine learning", "ml", "observability", "open source", "roundups", "security", "vs code"]
---

Welcome to this week's technology review highlighting recent progress in AI tools, developer platforms, cloud services, and security. GitHub Copilot now offers an open source VS Code extension, updated AI model support, flexible integration, and new automation options—enabling developers and organizations to customize workflows, boost transparency, and contribute directly to Copilot’s growth.

Azure has rolled out new infrastructure features, including faster and more secure virtual machines, improved file and networking controls, and expanded Logic Apps for agentic and professional automation tasks. Latest machine learning benchmarks feature updated Azure hardware for better performance and reproducibility. DevOps platforms have introduced tighter traceability, improved compliance, and unified observability with support from AI agents, while security teams respond to new threats and supply chain challenges. The articles below show how teams are adopting AI-assisted processes, hybrid developer tools, and stronger governance in a changing technical landscape.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [VS Code Copilot Extension: Open Source, Custom Modes, and Deep Customization](#vs-code-copilot-extension-open-source-custom-modes-and-deep-customization)
  - [GitHub Copilot in Visual Studio Code: New Features, AI Models, Security, and Workflow Enhancements](#github-copilot-in-visual-studio-code-new-features-ai-models-security-and-workflow-enhancements)
  - [Expanded AI Model Support and BYOK Flexibility](#expanded-ai-model-support-and-byok-flexibility)
  - [GitHub Copilot Coding Agent and Agentic Workflows](#github-copilot-coding-agent-and-agentic-workflows)
  - [Automatic Code Review and Administration with Copilot and MCP](#automatic-code-review-and-administration-with-copilot-and-mcp)
  - [Copilot for TM Forum Open API and Telco Workflows](#copilot-for-tm-forum-open-api-and-telco-workflows)
  - [Tutorials and Practical Guides: Prompt-Driven Development, Building with AI, and Certification Support](#tutorials-and-practical-guides-prompt-driven-development-building-with-ai-and-certification-support)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [AI Infrastructure: MOSAIC MicroLED Architecture](#ai-infrastructure-mosaic-microled-architecture)
  - [Agent Interoperability and Azure AI Foundry](#agent-interoperability-and-azure-ai-foundry)
  - [Copilot Studio and Azure AI Foundry Integration](#copilot-studio-and-azure-ai-foundry-integration)
  - [Copilot Studio: Document Automation and Hybrid Development](#copilot-studio-document-automation-and-hybrid-development)
  - [Language Model Updates and Migration](#language-model-updates-and-migration)
  - [Developer Tooling for Model Integration and Workflow](#developer-tooling-for-model-integration-and-workflow)
  - [Azure AI Foundry Translation and No-Code AI Workflows](#azure-ai-foundry-translation-and-no-code-ai-workflows)
  - [Avatar-Powered Education and AI Content Creation](#avatar-powered-education-and-ai-content-creation)
  - [AI Coding Platforms for Enterprise Apps](#ai-coding-platforms-for-enterprise-apps)
- [ML](#ml)
  - [Benchmarking Llama 2 70B and Llama 3.1 405B on Azure ND GB200 v6](#benchmarking-llama-2-70b-and-llama-31-405b-on-azure-nd-gb200-v6)
- [Azure](#azure)
  - [Azure Virtual Machines and Infrastructure](#azure-virtual-machines-and-infrastructure)
  - [Azure Storage, File Shares, and Authentication](#azure-storage-file-shares-and-authentication)
  - [Azure Logic Apps Automation and Agentic Integrations](#azure-logic-apps-automation-and-agentic-integrations)
  - [Azure Databricks and Microsoft Fabric Integration](#azure-databricks-and-microsoft-fabric-integration)
  - [Monitoring, Observability, and Container Platform Resilience](#monitoring-observability-and-container-platform-resilience)
  - [Enterprise App Services, Licensing, and Cost Optimization](#enterprise-app-services-licensing-and-cost-optimization)
  - [Azure Application Gateway and Network Isolation](#azure-application-gateway-and-network-isolation)
  - [Developer-Focused Architecture, Integration Patterns, and Migration](#developer-focused-architecture-integration-patterns-and-migration)
  - [Intelligent Workflows, Microservices Patterns, and Well-Architected AI](#intelligent-workflows-microservices-patterns-and-well-architected-ai)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET 10 Release Candidate 1 and Ecosystem Updates](#net-10-release-candidate-1-and-ecosystem-updates)
  - [Visual Studio 2026 Insiders and VS Code v1.104 Feature Enhancements](#visual-studio-2026-insiders-and-vs-code-v1104-feature-enhancements)
  - [.NET Tooling, Packaging, and Servicing Releases](#net-tooling-packaging-and-servicing-releases)
  - [Data Access Strategies: Dapper vs Entity Framework Core](#data-access-strategies-dapper-vs-entity-framework-core)
- [DevOps](#devops)
  - [GitHub and JFrog: Secure, Traceable AI-Driven DevOps Pipelines](#github-and-jfrog-secure-traceable-ai-driven-devops-pipelines)
  - [AI, Governance, and Observability in Modern DevOps](#ai-governance-and-observability-in-modern-devops)
  - [Cisco & Splunk: Unified Observability and AI Agent Automation](#cisco--splunk-unified-observability-and-ai-agent-automation)
  - [GitHub Actions, Collaboration, and Repository Management Updates](#github-actions-collaboration-and-repository-management-updates)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Data Access Control and Perimeter Security in Cloud Platforms](#data-access-control-and-perimeter-security-in-cloud-platforms)
  - [DevSecOps, Vulnerability Scanning, and Supply Chain Threats](#devsecops-vulnerability-scanning-and-supply-chain-threats)
  - [Automated Code and Secrets Security for Developers](#automated-code-and-secrets-security-for-developers)
  - [Security Vulnerabilities in Developer Tools and AI Coding Workflows](#security-vulnerabilities-in-developer-tools-and-ai-coding-workflows)

## GitHub Copilot

GitHub Copilot received new updates, including open sourcing of its VS Code extension, expanded control over AI models, and integrations focused on transparency for developers. Automation, security, and configuration options now better support individual and enterprise needs. Tutorials highlight Copilot's role in telecom APIs, certification study, and open source development. Copilot continues to evolve as a flexible AI coding platform, assisted by community contributions and support for additional models.

### VS Code Copilot Extension: Open Source, Custom Modes, and Deep Customization

Following last week’s focus on MCP integration and workflow automation, the VS Code Copilot Extension is now open source, discussed in detail by Microsoft’s Burke Holland. Community access allows developers to contribute new features and refine daily Copilot usage in VS Code. Custom Modes support tailored AI responses and performance for specific developer workflows. Advanced users can enable 'Beast Mode' in VS Code Insiders to experiment with prompt engineering and agent tuning.

These improvements build on last week’s MCP-driven automation. Guides and documentation on Custom Modes offer hands-on prompt engineering instructions. Open sourcing welcomes contributions such as code fixes and feature suggestions, letting users directly affect Copilot’s development. Initial coverage explains the extensibility available for customizing Copilot behavior.

- [VS Code Copilot Extension Goes Open Source: Deep Dive with Burke Holland]({{ "/videos/2025-09-09-VS-Code-Copilot-Extension-Goes-Open-Source-Deep-Dive-with-Burke-Holland.html" | relative_url }})
- [Deep Dive into the Open-Sourced VS Code Copilot Extension with Custom Modes]({{ "/videos/2025-09-09-Deep-Dive-into-the-Open-Sourced-VS-Code-Copilot-Extension-with-Custom-Modes.html" | relative_url }})

### GitHub Copilot in Visual Studio Code: New Features, AI Models, Security, and Workflow Enhancements

After agent and prompt file improvements last week, Visual Studio Code August 2025 (v1.104) introduces more Copilot upgrades for streamlined coding and improved security. Copilot Chat now automatically selects among AI models (GPT-5, Claude Sonnet 4, Gemini Pro 2.5), enhancing suggestion accuracy and dealing with rate limits; manual model selection is also possible. Team collaboration gets refinements such as an advanced agent system, reusable prompts, device-wide toggles, and chat enhancements including KaTeX math rendering and custom fonts. Extension authors benefit from finalized LanguageModelChatProvider APIs and improved authentication.

For security, agent mode prompts users before modifying protected files, and organizations can adjust approvals for terminal commands. Changed files are now easier to review with collapsible lists, and expanded AGENTS.md support allows detailed Copilot usage standards. These additions align with productivity and high-control needs for large and security-sensitive teams.

- [What's New in Visual Studio Code August 2025 (v1.104)](https://code.visualstudio.com/updates/v1_104)
- [GitHub Copilot in Visual Studio Code August 2025 Update (v1.104): New Features and Security Enhancements](https://github.blog/changelog/2025-09-12-github-copilot-in-vs-code-august-release-v1-104)

### Expanded AI Model Support and BYOK Flexibility

Extending last week’s updates for organizational customization, Copilot now supports OpenAI GPT-5 and GPT-5 mini—GPT-5 mini for Free users, GPT-5 for paid subscriptions. A new model picker UI lets developers and organizations choose models on GitHub.com, VS Code, Visual Studio, JetBrains IDEs, Xcode, and GitHub Mobile. IT administrators can set defaults centrally. This lets users and teams adjust Copilot for anything from basic scripting to complex code generation.

Copilot now supports Bring Your Own Key (BYOK), available in public preview for JetBrains IDEs and Xcode. Developers can use their own API keys for Azure, OpenAI, Anthropic, Gemini, Groq, and OpenRouter—enabling model experimentation and better quota management. Teams can customize Copilot Chat and try out new models before they’re fully supported, with ongoing community feedback shaping future IDE integrations.

- [GitHub Copilot Adds Support for OpenAI GPT-5 and GPT-5 Mini Models](https://github.blog/changelog/2025-09-09-openai-gpt-5-and-gpt-5-mini-are-now-generally-available-in-github-copilot)
- [Public Preview: Bring Your Own Key (BYOK) for GitHub Copilot Chat in JetBrains and Xcode](https://github.blog/changelog/2025-09-11-bring-your-own-key-byok-support-for-jetbrains-ides-and-xcode-in-public-preview)

### GitHub Copilot Coding Agent and Agentic Workflows

Building on agent-driven automation and MCP connections from last week, Copilot’s agent now acts as a coding teammate—running code fixes, refactoring, and other pull request tasks. Developers assign agent tasks via Issues, VS Code, or a special panel, using ephemeral GitHub Actions runners for safe, isolated work. Agents’ changes are reviewed and run through CI before approval, maintaining secure standards. The Model Context Protocol (MCP) provides expanded context for improved results. Teams get management over assignments, audits, and branch protection, with guides outlining setup and scaling.

- [Getting Started with GitHub Copilot Coding Agent and Agentic Workflows](https://github.blog/ai-and-ml/github-copilot/github-copilot-coding-agent-101-getting-started-with-agentic-workflows-on-github/)

### Automatic Code Review and Administration with Copilot and MCP

Copilot’s new repository rule allows separate automatic code review from merge requirements. Repository admins can get early feedback and frequent code quality checks tailored to different rule sets, supporting productivity and flexible coverage—useful for regulated projects.

The MCP remote server (now generally available) links Copilot agents, GitHub workflows, and large language models. Security features include secret scanning, code scanning, and automated advisories. Enterprises can set internal registry and allowlist controls for MCP servers via Copilot in VS Code Insiders, regulating AI endpoint access. These changes make configuration easier and anticipate broader tool support, showing Copilot’s focus on balancing useful AI and governance.

- [Automatic Copilot Code Review: Standalone Repository Rule Now Available](https://github.blog/changelog/2025-09-10-copilot-code-review-independent-repository-rule-for-automatic-reviews)
- [GitHub's Remote MCP Update: General Availability and Key Integrations]({{ "/videos/2025-09-10-GitHubs-Remote-MCP-Update-General-Availability-and-Key-Integrations.html" | relative_url }})
- [Configuring Internal MCP Registries and Allowlists for Copilot in VS Code Insiders](https://github.blog/changelog/2025-09-12-internal-mcp-registry-and-allowlist-controls-for-vs-code-insiders)

### Copilot for TM Forum Open API and Telco Workflows

Copilot’s application in TM Forum Open API development boosts productivity and standards compliance for telecom APIs—especially with Node.js/Express TMF-compliant endpoints. Copilot streamlines boilerplate creation, testing, and validation. Companies like Proximus, NOS, Orange, and Vodafone report faster development and improved API matching. Best practices stress ongoing validation and keeping features current.

At TM Forum Innovate Americas 2025, Microsoft demonstrated Copilot, Azure AI Foundry, and MCP integration for modular telecom architectures and open standards. Agentic AI and Copilot accelerate API delivery, reduce repetitive work, and support orchestration, showing impact in telecom engineering.

- [Supercharge TM Forum Open API Development with GitHub Copilot](https://techcommunity.microsoft.com/t5/telecommunications-industry-blog/supercharge-your-tm-forum-open-api-development-with-github/ba-p/4451366)
- [Reimagining Telco with Microsoft: AI, TM Forum ODA, and Developer Innovation](https://techcommunity.microsoft.com/t5/telecommunications-industry-blog/reimagining-telco-with-microsoft-ai-tm-forum-oda-and-developer/ba-p/4451724)

### Tutorials and Practical Guides: Prompt-Driven Development, Building with AI, and Certification Support

A range of resources are now available. Tutorials in VS Code detail Copilot prompt engineering, Copilot Vision, voice interactions, and agent workflows—helping users automate coding and boost test coverage from within the IDE.

Building on last week’s beginner guides and prompt best practices, other guides cover Copilot’s use in open source app prototyping and Microsoft certification study, offering step-by-step automation for documentation, scripting, and custom workflows (like Markdown or PowerShell tasks for cloud automation). Copilot helps developers build skills, onboard to new stacks, and launch open source projects with community support.

- [Introduction to Prompt-Driven Development in VS Code]({{ "/videos/2025-09-09-Introduction-to-Prompt-Driven-Development-in-VS-Code.html" | relative_url }})
- [Building Personal Apps with Open Source and AI](https://github.blog/open-source/maintainers/building-personal-apps-with-open-source-and-ai/)
- [How GitHub Copilot Can Boost Your Microsoft Certification Prep](https://dellenny.com/supercharge-your-it-certification-prep-how-github-copilot-can-be-your-study-buddy/)

### Other GitHub Copilot News

GitHub Universe 2025 will feature over 100 sessions about Copilot workflows, automation, and current advances in AI and security. Attendees can view demos, take remote certifications (including new Copilot exams), and meet technical experts, showing Copilot’s growing role in development and automation.

- [Your Guide to GitHub Universe 2025: Event Schedule, Learning, and Certifications Announced](https://github.blog/news-insights/company-news/your-guide-to-github-universe-2025-the-schedule-just-launched/)

## AI

Recent updates in AI center around hardware research, adoption of open standards, expanded low/no-code automation, and new developer integrations. Microsoft’s latest work enables more adaptable agent systems, hybrid developer experiences, and improved governance for enterprise AI. New research tackles infrastructure efficiency and scalable model deployments.

### AI Infrastructure: MOSAIC MicroLED Architecture

Microsoft Research introduced MOSAIC, an optical networking approach for data centers using bundles of slow microLED channels instead of a few fast ones. This increases throughput, reduces power use, and improves reliability—grouping thousands of channels can reach up to 3.2 Tbps while lowering costs and complexity for AI infrastructure. MicroLEDs also resist temperature problems and offer lower failure rates than lasers. MOSAIC is compatible with standard protocols, simplifying integration. Full details are available in technical papers.

- [MOSAIC: A Wide-and-Slow MicroLED Network Architecture for Next-Gen AI Infrastructure](https://www.microsoft.com/en-us/research/blog/breaking-the-networking-wall-in-ai-infrastructure/)

### Agent Interoperability and Azure AI Foundry

Azure AI Foundry added more open standards, enabling agents, apps, and enterprise data to interoperate via Model Context Protocol (MCP) and Agent2Agent (A2A). MCP lets agents share workflow context, while A2A models collaborative agent activity. Foundry supports thousands of connectors, unified monitoring, and comprehensive app integration—making multi-agent workflows possible across platforms and ensuring secure data access and compliance.

Continuing last week’s movement toward open standards and orchestration, this update shows how Foundry leverages MCP and A2A for flexible agent systems.

- [Agent Factory: Connecting Agents, Apps, and Data with Open Standards (MCP, A2A)](https://azure.microsoft.com/en-us/blog/agent-factory-connecting-agents-apps-and-data-with-new-open-standards-like-mcp-and-a2a/)

### Copilot Studio and Azure AI Foundry Integration

New resources explain how Copilot Studio’s low-code conversational agent builder connects with Azure AI Foundry, supporting custom model workflows and managed deployment. The workflow moves from quick agent creation to enterprise-grade deployment, bridging low-code tools and professional lifecycle management. Interviews and analysis highlight both current and planned integrations for a more unified Microsoft AI developer experience.

These updates continue coverage of orchestration and automation, now connecting low-code agent design to pro deployment.

- [Exploring the Connection Between Copilot Studio and Azure AI Foundry]({{ "/videos/2025-09-11-Exploring-the-Connection-Between-Copilot-Studio-and-Azure-AI-Foundry.html" | relative_url }})
- [Understanding the Connection Between Copilot Studio and Azure AI Foundry]({{ "/videos/2025-09-11-Understanding-the-Connection-Between-Copilot-Studio-and-Azure-AI-Foundry.html" | relative_url }})

### Copilot Studio: Document Automation and Hybrid Development

Copilot Studio demonstrated automation with no-code agents integrated into Power Platform and AI Builder, streamlining document-heavy tasks like vehicle permit processing. Resources cover setup, configuration, and monitoring, showing live metrics and error reduction. Additional analysis shows Copilot Studio connecting low-code and pro-code workflows—making it easier for both non-technical and experienced developers to collaborate and scale solutions. Teams can prototype quickly and migrate to professional-grade solutions as needed.

Continuing last week’s coverage of hybrid workflow automation, this set of articles explores Copilot Studio's progress in team document workflows and solution scaling.

- [Build AI Agents for Fast, High-Volume Document Automation in Copilot Studio]({{ "/videos/2025-09-09-Build-AI-Agents-for-Fast-High-Volume-Document-Automation-in-Copilot-Studio.html" | relative_url }})
- [Low-Code vs Pro-Code: How Copilot Studio Bridges the Gap](https://dellenny.com/low-code-vs-pro-code-how-copilot-studio-bridges-the-gap/)

### Language Model Updates and Migration

Microsoft has deprecated Phi-3 and Phi-3.5 models in GitHub Models, recommending that users migrate to Phi-4 and Phi-4-mini-instruct. Migration instructions and mapping are provided for updating workflows. Teams are encouraged to track changes for stability and current support.

This builds on last week’s efforts to keep teams informed about model life cycles and minimize disruption in workflows.

- [Microsoft Phi-3 Model Deprecation and Transition in GitHub Models](https://github.blog/changelog/2025-09-11-deprecated-microsoft-models-in-github-models)

### Developer Tooling for Model Integration and Workflow

Visual Studio Code introduces the Language Model Chat Provider API (BYOK), letting extension developers integrate models from any provider for privacy and policy flexibility. Technical resources detail API usage and implementation. Warp has added embedded AI agents to its CLI, offering prompt-driven scripting, code review, and editing for terminal-focused workflows.

These tools support easier daily AI integration in both IDE and CLI environments, following the trend of improving developer access to AI capabilities.

- [Extending VS Code with the Language Model Chat Provider (BYOK) API: Insights from Logan Ramos]({{ "/videos/2025-09-08-Extending-VS-Code-with-the-Language-Model-Chat-Provider-BYOK-API-Insights-from-Logan-Ramos.html" | relative_url }})
- [Warp Integrates AI Coding Agents into CLI for Enhanced Developer Feedback](https://devops.com/warp-embeds-ai-agents-into-a-cli-to-provide-better-feedback-loop/?utm_source=rss&utm_medium=rss&utm_campaign=warp-embeds-ai-agents-into-a-cli-to-provide-better-feedback-loop)

### Azure AI Foundry Translation and No-Code AI Workflows

Azure AI Foundry's Translator API is now in public preview, enabling developers to add both neural and LLM translation features to apps with step-by-step multilingual integration. The Cozy AI Kitchen show explored CalcLM—a no-code option for bringing GPT-4.1 to spreadsheets via Azure OpenAI, allowing analysis and planning through natural language queries. Demos demonstrate agent customization and further integration possibilities.

These resources extend AI accessibility across technical and business user scenarios, complementing recent efforts to make AI more usable for non-developers.

- [Translating Conversations with Azure AI Foundry Translator API]({{ "/videos/2025-09-11-Translating-Conversations-with-Azure-AI-Foundry-Translator-API.html" | relative_url }})
- [Blending AI Agents and Spreadsheets: No-Code Solutions in the Cozy AI Kitchen]({{ "/videos/2025-09-09-Blending-AI-Agents-and-Spreadsheets-No-Code-Solutions-in-the-Cozy-AI-Kitchen.html" | relative_url }})

### Avatar-Powered Education and AI Content Creation

This guide covers building avatar-powered educational solutions with Azure, incorporating neural text-to-speech, avatars, and secure CI/CD, storage, and identity management. Developers receive templates and instructions for automating onboarding and training resources with compliance controls.

Supporting last week’s coverage on AI accessibility, Azure continues to offer tools for innovative and inclusive learning solutions.

- [Revolutionizing Learning with Immersive AI: Avatar-Powered Education on Azure](https://techcommunity.microsoft.com/t5/azure-architecture-blog/revolutionizing-learning-with-immersive-ai/ba-p/4453680)

### AI Coding Platforms for Enterprise Apps

Empromptu launched "vibecoding" for explainable, compliant enterprise AI apps. The platform uses retrieval-augmented generation, LLMOps tooling, and output scoring—plus DevOps integration with SOC 2 governance and credit-based pricing.

This matches last week's interest in enterprise-ready AI, highlighting the benefit of transparency and production monitoring.

- [Empromptu Launches Vibecoding AI Coding Platform for Enterprise Apps](https://devops.com/empromptu-unveils-vibecoding-platform-for-building-enterprise-class-apps/?utm_source=rss&utm_medium=rss&utm_campaign=empromptu-unveils-vibecoding-platform-for-building-enterprise-class-apps)

## ML

Machine learning developments focus on benchmarking language models using Azure AI hardware, with guides enabling reproducible testing for demanding workloads. Extending last week's coverage of benchmarking standards, practical instructions offer clear steps for real-world large-model testing.

### Benchmarking Llama 2 70B and Llama 3.1 405B on Azure ND GB200 v6

A comprehensive guide explains benchmarking Llama 2 70B and Llama 3.1 405B models with MLPerf Inference v5.1 on Azure ND GB200 v6 VMs running NVIDIA Grace CPUs and Blackwell B200 GPUs. Detailed steps include VM setup, organizing data, repo cloning, and prepping the environment. Results show Llama 2 70B at 52,000 tokens/sec and Llama 3.1 405B at 847 tokens/sec on a single VM, matching global performance. Sample configurations and MLPerf orchestration enable repeatable evaluations for both research and production.

These outcomes reinforce transparent processes and standards-based evaluation highlighted last week.

- [Benchmarking Llama 2 70B and 405B Models on Azure ND GB200 v6 with MLPerf Inference v5.1](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/a-quick-guide-to-benchmarking-ai-models-on-azure-llama-405b-and/ba-p/4452192)

## Azure

This week’s Azure news covers new VM sizes and hardware, improved storage and authentication, automation with Logic Apps, data integration, monitoring, migration, and network security. These updates strengthen Azure’s capacity for modern, secure, efficient cloud deployments.

### Azure Virtual Machines and Infrastructure

Microsoft previewed Azure Dasv7, Easv7, and Fasv7 VMs using 5th Gen AMD EPYC “Turin” processors, offering up to 35% improved CPU performance, high memory and storage throughput, NVMe, Azure Boost, advanced networking, and FIPS 140-3 compliance. D192 sizes join Dsv6/Ddsv6-series, supporting 192 vCPUs and 768 GiB RAM for intensive analytic and scientific workloads. These updates enhance efficiency and security, supporting expanded workload types.

- [Announcing Preview of New Azure Dasv7, Easv7, and Fasv7-Series VMs Based on 5th Gen AMD EPYC™ ‘Turin’ Processors](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-preview-of-new-azure-dasv7-easv7-fasv7-series-vms/ba-p/4448360)
- [Azure D192 Sizes Now Available in Dsv6 and Ddsv6-Series VMs](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-general-availability-of-azure-d192-sizes-in-the-azure/ba-p/4451427)

### Azure Storage, File Shares, and Authentication

Azure Storage APIs now accept Microsoft Entra ID and RBAC, replacing older SAS/account-key access for better security and standardized error codes. Azure also introduces file share-centric management, making file shares top-level resources—simplifying set up, scaling, and billing, while improving security and isolation.

These features build on previous improvements in resource management and more granular control—continuing Azure’s modernization.

- [Azure Storage APIs Gain Microsoft Entra ID and RBAC Support](https://devblogs.microsoft.com/azure-sdk/azure-storage-apis-gain-entra-id-and-rbac-support/)
- [Introducing a File Share-Centric Management Model for Azure Files](https://techcommunity.microsoft.com/t5/azure-storage-blog/simplifying-file-share-management-and-control-for-azure-files/ba-p/4452634)
- [Understanding the New File Share Resource Type in Azure]({{ "/videos/2025-09-10-Understanding-the-New-File-Share-Resource-Type-in-Azure.html" | relative_url }})

### Azure Logic Apps Automation and Agentic Integrations

Logic Apps now support multi-agent orchestration and a Python code interpreter via Azure Container Apps, enabling advanced automation and data analysis. Foundry Agent Service links conversational agents to both Microsoft and third-party AI models. Improved Entra ID integration boosts session security. MCP server public preview with API Center or HTTP/Easy Auth registration makes workflow and API management simple and extensible.

These improvements continue the focus on scalable automation, practical agent modularity, and expanded governance.

- [Azure Logic Apps: Multi-Agentic Business Process Automation Platform Enhancements](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/azure-logic-apps-ushering-in-the-era-of-multi-agentic-business/ba-p/4452275)
- [Python Code Interpreter for Logic Apps: Execute Python in Workflows Using Azure Container Apps](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcement-python-code-interpreter-in-logic-apps-is-now-in/ba-p/4452239)
- [Build Modular Agents with Logic Apps MCP Servers (Public Preview)](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-logic-apps-mcp-servers-public-preview/ba-p/4450419)
- [Building MCP Servers with Azure Logic Apps: Demo Collection](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-mcp-demos/ba-p/4452175)

### Azure Databricks and Microsoft Fabric Integration

Articles explore eight ways to integrate Azure Databricks with Microsoft Fabric, including Power BI connectors, Delta Sharing, pipeline orchestration, publishing jobs, and direct Lakehouse writes. General Availability of Automatic Identity Management (AIM) automates Entra ID user and group setup for compliant analytics at large scale.

This supports seamless analytics and unified data management, following continuous platform integration.

- [Comprehensive Methods for Integrating Azure Databricks with Microsoft Fabric](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/approaches-to-integrating-azure-databricks-with-microsoft-fabric/ba-p/4453643)
- [General Availability of Automatic Identity Management (AIM) for Entra ID on Azure Databricks](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/general-availability-automatic-identity-management-aim-for-entra/ba-p/4452206)

### Monitoring, Observability, and Container Platform Resilience

Azure Monitor’s Container Insights now supports high scale mode for larger clusters, with better log rates, streamlined upgrades, and minimal setup. Azure Container Registry defaults to zone redundancy in availability-zone regions, improving resilience without extra configuration or cost; management tools are being updated accordingly.

These updates extend last week’s work on VM reliability and SRE agent features, with strengthened cloud reliability.

- [General Availability of High Scale Mode in Azure Monitor Container Insights](https://techcommunity.microsoft.com/t5/azure-observability-blog/generally-available-high-scale-mode-in-azure-monitor-container/ba-p/4452199)
- [Zone Redundancy Now Default for Azure Container Registry in Availability Zone Regions](https://techcommunity.microsoft.com/t5/microsoft-developer-community/zone-redundancy-is-now-enabled-by-default-in-azure-container/ba-p/4450618)

### Enterprise App Services, Licensing, and Cost Optimization

Azure App Service now lets users Bring Your Own License (BYOL) for JBoss EAP applications, allowing organizations to reuse existing Red Hat licenses and avoid pay-as-you-go costs—streamlining compliance and migration.

This follows Azure App Service’s evolution toward flexible licensing and hybrid cloud support for complex enterprise workloads.

- [Bring Your Own License (BYOL) Now Available for JBoss EAP on Azure App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/bring-your-own-license-byol-support-for-jboss-eap-on-azure-app/ba-p/4452152)

### Azure Application Gateway and Network Isolation

Application Gateway now enables private-only deployment and separate control/data planes, improving isolation and compliance. Guides cover migration steps, subnet assignment, and NSG integration for secure app delivery.

These features complement last week’s advice on hybrid network security and migration architecture.

- [Application Gateway Network Isolation: New Architecture Deep Dive]({{ "/videos/2025-09-08-Application-Gateway-Network-Isolation-New-Architecture-Deep-Dive.html" | relative_url }})
- [Application Gateway Network Isolation Explained]({{ "/videos/2025-09-08-Application-Gateway-Network-Isolation-Explained.html" | relative_url }})

### Developer-Focused Architecture, Integration Patterns, and Migration

The Microsoft Fabric Migration Guide offers in-depth data warehouse migration strategies. A technical session shows how SQL Server 2025 and Pure Storage FlashArray can take REST API-based snapshot backups with metadata tags from T-SQL. New Azure Policy definitions enable auditing of policy inheritance for API Management, boosting security and reliable deployment.

These articles deepen migration and governance topics featured previously.

- [Microsoft Fabric Migration Guide: Best Practices for Data Warehouse Migration](https://blog.fabric.microsoft.com/en-US/blog/migrating-to-fabric-data-warehouse-guide-now-available/)
- [Building a Snapshot Backup Catalog with SQL Server 2025 and Pure Storage FlashArray]({{ "/videos/2025-09-11-Building-a-Snapshot-Backup-Catalog-with-SQL-Server-2025-and-Pure-Storage-FlashArray.html" | relative_url }})
- [Enforcing and Auditing Policy Inheritance in Azure API Management](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/enforce-or-audit-policy-inheritance-in-api-management/ba-p/4452204)

### Intelligent Workflows, Microservices Patterns, and Well-Architected AI

The n8n engine is now supported in Azure Container Apps, integrating Azure OpenAI for agentic workflows—enabling routing, summarization, and content creation. New guidance on Azure Well-Architected Framework for AI covers reliability, security, efficiency, and DevOps practices. A guide for the sidecar pattern covers logging, monitoring, and state management in AKS, Azure Container Apps, and Service Fabric.

Consistent with last week’s automation and architecture guidance, these resources assist designers of robust Azure-based microservices and AI solutions.

- [Building Agentic Workflows with n8n and Azure Container Apps](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-agentic-workflows-with-n8n-and-azure-container-apps/ba-p/4452362)
- [Designing AI Workloads with the Azure Well-Architected Framework](https://techcommunity.microsoft.com/t5/azure-architecture-blog/designing-ai-workloads-with-the-azure-well-architected-framework/ba-p/4452252)
- [Riding in Tandem: Unlocking the Sidecar Pattern in Azure Microservices](https://dellenny.com/riding-in-tandem-unlocking-the-sidecar-pattern-in-azure-microservices/)

### Other Azure News

OneLake File Explorer v1.0.14.0 improves temp file handling, crash detection, and upgrades to .NET 8, increasing reliability and security for data management in Microsoft Fabric and Lakehouse deployments.

Following previous improvements, these updates enhance robust data workflows for Microsoft Fabric users.

- [OneLake File Explorer 1.0.14.0: Enhanced Integration with Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/onelake-file-explorer-smarter-more-reliable-and-seamlessly-integrated/)

The latest Azure Update shares new service features, VM SKUs, updates in databases, migration tools, and retirement details for Playwright Testing—including certification resource links for ongoing learning.

Regular briefings help developers stay up-to-date and support skill development.

- [Azure Update: Announcements and Service Changes (12 September 2025)]({{ "/videos/2025-09-12-Azure-Update-Announcements-and-Service-Changes-12-September-2025.html" | relative_url }})

## Coding

This section highlights new milestone releases and tooling updates from Microsoft. With .NET 10 approaching general release, developers are encouraged to start testing, while Visual Studio and VS Code both see new AI and automation features. Guides offer practical advice for modern development and migration.

### .NET 10 Release Candidate 1 and Ecosystem Updates

.NET 10 RC1 is available for production testing. Previous updates focused on CLI improvements, multi-platform workflows, and new language features—now, RC1 can be tried with Visual Studio 2026 Insiders and VS Code’s C# Dev Kit. Updates include new quantum-safe cryptography APIs, faster UTF-8 hex-string conversions, and new tensor types for numerical computation. Runtime, SDK, and languages (C#, F#, VB) have refinements for increased stability.

ASP.NET Core features new Blazor component state persistence, expanded identity support, improved Minimal API validation, and better OpenAPI docs. Blazor continues UX and platform improvements, while .NET MAUI offers debugging, new UI events, enhanced refresh controls, and early CoreCLR support on Android. Windows Forms now has dark mode, improved themes, async capabilities, and easier renderer management. Entity Framework Core adds vector search in SQL Server, native JSON types, enhanced Cosmos DB support, and better complex type handling.

Developers are encouraged to validate their applications on RC1, check for breaking changes, join standups, and plan migrations. The .NET Unboxed event shares rollout details and technical Q&A.

- [Announcing .NET 10 Release Candidate 1](https://devblogs.microsoft.com/dotnet/dotnet-10-rc-1/)
- [.NET Unboxed - .NET 10 Release Candidate 1]({{ "/videos/2025-09-09-NET-Unboxed-NET-10-Release-Candidate-1.html" | relative_url }})

### Visual Studio 2026 Insiders and VS Code v1.104 Feature Enhancements

Visual Studio 2026 Insiders is a new early-access program for monthly IDE updates and direct community feedback. AI integration improves code completion, automated testing, and review—embeddeding GitHub Copilot Free for all users. Enhanced solution management, Profiler Agent, and faster performance are available for x64/Arm64. UI updates include new Fluent designs, themes, and user onboarding, shaped by user contributions.

VS Code v1.104 features Agent Mode for workspace automation—allowing developers to offload routine tasks and focus on coding. Additional updates include improved APIs for plugins, automated terminal approval, and new TODO management. Git Worktree support simplifies multi-branch reviews and development, enhancing last week’s prompt automation for collaboration.

- [Visual Studio 2026 Insiders Launch: Integrated AI, Performance, and Developer-Centric Upgrades](https://devblogs.microsoft.com/visualstudio/visual-studio-2026-insiders-is-here/)
- [Latest Features in Visual Studio Code: Agent Mode, Git Worktrees, and More]({{ "/videos/2025-09-12-Latest-Features-in-Visual-Studio-Code-Agent-Mode-Git-Worktrees-and-More.html" | relative_url }})
- [VS Code Live: Exploring Hidden Features in VS Code v1.104]({{ "/videos/2025-09-11-VS-Code-Live-Exploring-Hidden-Features-in-VS-Code-v1104.html" | relative_url }})

### .NET Tooling, Packaging, and Servicing Releases

A new guide shows how .NET 10 enables expanded packaging for multi-targeted, reusable tools—developers can create NuGet packages for self-contained, trimmed, or ahead-of-time compiled tool distributions. The article includes deployment and configuration samples and recommends thorough testing ahead of general release.

Routine servicing for .NET 8 and .NET 9 in September 2025 brings bug fixes and reliability updates for ASP.NET Core, the SDK, and .NET Framework across all platforms. Developers should consult changelogs and apply fixes to maintain platform stability.

- [Packaging Self-Contained and Native AOT .NET Tools for NuGet: .NET 10 Preview](https://andrewlock.net/exploring-dotnet-10-preview-features-7-packaging-self-contained-and-native-aot-dotnet-tools-for-nuget/)
- [.NET and .NET Framework September 2025 Servicing Releases Updates](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-september-2025-servicing-updates/)

### Data Access Strategies: Dapper vs Entity Framework Core

The .NET Data Community Standup compared Dapper and Entity Framework Core. Presenters shared concrete lessons—Dapper’s speed and simplicity versus EF Core’s feature set and maintainability. The session covered use cases, performance tips, pitfalls, and factors for teams deciding between direct SQL mapping and a higher-level ORM.

- [.NET Data Community Standup: Practical Dapper vs Entity Framework Core Comparison]({{ "/videos/2025-09-09-NET-Data-Community-Standup-Practical-Dapper-vs-Entity-Framework-Core-Comparison.html" | relative_url }})

## DevOps

DevOps coverage this week features updated automation, end-to-end traceability, improved collaboration, unified observability, and embedded AI throughout software delivery. Security and workflow resilience remain central, with infrastructure enhancements supporting secure, agentic automations.

### GitHub and JFrog: Secure, Traceable AI-Driven DevOps Pipelines

GitHub and JFrog expanded their integration for secure CI/CD traceability—developers can link commits to artifacts, automate SBOM policies, and use GitHub Actions for artifact management. Security combines GitHub Advanced Security and JFrog Xray; OIDC-based authentication removes secrets from CI, continuing secretless automation. JFrog previewed JFry, an agentic AI platform for artifact control and governance with semantic metadata.

Integration with GitHub Copilot, ServiceNow, and SonarQube extends efficient developer experiences, and new tools automate compliance and evidence management for audit-ready DevOps.

- [How to use the GitHub and JFrog integration for secure, traceable builds from commit to production](https://github.blog/enterprise-software/devsecops/how-to-use-the-github-and-jfrog-integration-for-secure-traceable-builds-from-commit-to-production/)
- [JFrog Unveils DevOps Platform for the Agentic AI Era](https://devops.com/jfrog-unveils-devops-platform-for-the-agentic-ai-era/?utm_source=rss&utm_medium=rss&utm_campaign=jfrog-unveils-devops-platform-for-the-agentic-ai-era)
- [JFrog SwampUP 2025 Highlights: AI-Driven DevOps, Governance, and Secure Software Supply Chains](https://devops.com/jfrog-continues-leaping-at-swampup/?utm_source=rss&utm_medium=rss&utm_campaign=jfrog-continues-leaping-at-swampup)
- [JFrog CEO: AI Agents Require Practices Beyond Security, Traceability](https://devops.com/jfrog-ceo-ai-agents-require-practices-beyond-security-traceability/?utm_source=rss&utm_medium=rss&utm_campaign=jfrog-ceo-ai-agents-require-practices-beyond-security-traceability)

### AI, Governance, and Observability in Modern DevOps

DevOps teams now integrate governance into every workflow phase to manage AI expansion and meet compliance needs. DevGovOps adds real-time verification and risk detection in CI/CD, continuing last week’s DevSecOps evolution.

Surveys show IT teams expect greater AI usage but need further automation and skills to ensure reliability, confirming last week’s findings about workflow challenges. Infrastructure as Code and platform engineering remain priorities. SwampUP 2025 panels focus on supply chain verification and team-wide compliance. Perforce’s Delphix update now uses AI to generate synthetic data for application testing, supporting better test coverage in secure settings.

- [DevGovOps: Embedding Governance into DevOps for the Age of AI](https://devops.com/devgovops-a-new-play-in-devops-or-is-it/?utm_source=rss&utm_medium=rss&utm_campaign=devgovops-a-new-play-in-devops-or-is-it)
- [Survey Reveals IT Teams Struggle to Scale AI Workloads Due to Automation Gaps](https://devops.com/survey-most-it-teams-not-prepared-to-manage-ai-workloads/?utm_source=rss&utm_medium=rss&utm_campaign=survey-most-it-teams-not-prepared-to-manage-ai-workloads)
- [Perforce Adds AI-Driven Synthetic Data Generation to Delphix Platform for DevOps Testing](https://devops.com/perforce-adds-small-language-model-to-create-synthetic-data-for-app-testing/?utm_source=rss&utm_medium=rss&utm_campaign=perforce-adds-small-language-model-to-create-synthetic-data-for-app-testing)
- [Bringing Trust and Governance to AI-Driven DevOps](https://devops.com/bringing-trust-and-governance-to-ai-driven-devops/?utm_source=rss&utm_medium=rss&utm_campaign=bringing-trust-and-governance-to-ai-driven-devops)
- [Survey Reveals Software Engineering Hurdles After AI Adoption](https://devops.com/survey-surfaces-software-engineering-challenges-following-adoption-of-ai/?utm_source=rss&utm_medium=rss&utm_campaign=survey-surfaces-software-engineering-challenges-following-adoption-of-ai)

### Cisco & Splunk: Unified Observability and AI Agent Automation

Cisco Splunk .Conf25 launched agentic AI and a data fabric for automated observability. Splunk now uses OpenTelemetry for streamlined agent integration in monitoring, supporting automated data collection, incident management, and remediation for distributed workloads.

Cisco Data Fabric offers analytics on machine data using AI/ML, supporting operational insights. Integrations provide APM, DEA, user and network monitoring, and Cisco AI Canvas delivers a single platform for DevOps and security teams. OpenTelemetry semantic extensions allow unified monitoring for legacy and AI services, improving automated incident response.

- [Cisco Integrates AI Agents and Data Fabric into Splunk Observability Platform](https://devops.com/cisco-to-add-ai-agents-to-splunk-observability-platforms/?utm_source=rss&utm_medium=rss&utm_campaign=cisco-to-add-ai-agents-to-splunk-observability-platforms)
- [OpenTelemetry Extensions Enhance Observability for AI Agents](https://devops.com/opentelemetry-extensions-to-enable-observability-of-ai-agents/?utm_source=rss&utm_medium=rss&utm_campaign=opentelemetry-extensions-to-enable-observability-of-ai-agents)

### GitHub Actions, Collaboration, and Repository Management Updates

GitHub has released several workflow improvements. The macOS 26 image for Actions is now in preview, streamlining infrastructure for iOS developers. An improved Projects REST API enables more refined management. Pull request file review is faster, with increased limits and better usability. Ruleset exemptions let trusted users or bots bypass code checks. Repository insights now have expanded accessibility and export options.

Multiple assignees for issues and pull requests are available in all repositories for improved teamwork. Verified Answers in GitHub Discussions provide formal confirmation, helping both human and AI-powered support.

- [Using the macOS 26 Image in GitHub Actions Workflows](https://github.blog/changelog/2025-09-11-actions-macos-26-image-now-in-public-preview)
- [REST API Enhancements for GitHub Projects and Sub-Issues Improvements](https://github.blog/changelog/2025-09-11-a-rest-api-for-github-projects-sub-issues-improvements-and-more)
- [GitHub Pull Request ‘Files Changed’ Public Preview Updates – Increased File Limits and Performance Improvements](https://github.blog/changelog/2025-09-11-pull-request-files-changed-public-preview-experience-september-11-updates)
- [GitHub Ruleset Exemptions and New Repository Insights Features](https://github.blog/changelog/2025-09-10-github-ruleset-exemptions-and-repository-insights-updates)
- [Multiple Assignees for Issues and Pull Requests Now Available in All GitHub Repositories](https://github.blog/changelog/2025-09-11-multiple-assignees-for-issues-and-pull-requests-now-available-in-all-repositories)
- [Verified Answers Launched in GitHub Discussions for Reliable Community Solutions](https://github.blog/changelog/2025-09-11-verified-answers-generally-available-in-github-discussions)

### Other DevOps News

Teams migrating from Azure DevOps to GitHub now have guidance covering planning, repository and pipeline migration, and hybrid deployment approaches for secure, agile development.

- [From Azure DevOps to GitHub: Migrate, Integrate, Accelerate]({{ "/2025-09-10-From-Azure-DevOps-to-GitHub-Migrate-Integrate-Accelerate.html" | relative_url }})

A short GitHub video tutorial explains basic Git concepts for new developers, supporting best practices for onboarding and collaboration.

- [7 Essential Git Concepts Every Beginner Needs to Know]({{ "/videos/2025-09-11-7-Essential-Git-Concepts-Every-Beginner-Needs-to-Know.html" | relative_url }})

## Security

Security updates this week address new data access controls, developer automation, and tool vulnerabilities, with a clear focus on managed access, code and secret scanning, and evolving policy for AI-augmented coding environments.

### Data Access Control and Perimeter Security in Cloud Platforms

Microsoft Fabric’s OneLake now offers unified management for RBAC and fine-grained row/column security, supporting consistent analytics enforcement and up to 4x faster queries. Upgrades are automatic, with management via UI or API for easier governance.

Azure Storage introduces network security perimeters for PaaS resources, centralizing boundary and access management with default public access denial and integrated auditing—no extra cost. This improves risk assessment and compliance for larger deployments.

These new features expand unified access control highlighted last week and address compliance for analytics and storage.

- [Announcing OneLake Security (Preview): Fine-Grained Data Access Control in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/onelake-security-is-now-available-in-public-preview/)
- [Protect Azure Storage Accounts with Network Security Perimeter: General Availability](https://techcommunity.microsoft.com/t5/azure-storage-blog/protect-your-storage-accounts-using-network-security-perimeter/ba-p/4449046)

### DevSecOps, Vulnerability Scanning, and Supply Chain Threats

Fast DevSecOps pipelines now use context-aware vulnerability scanning, integrating with pull requests and ticketing to provide targeted alerts and reduce signal overload—optimizing for reduced time-to-fix and fewer false positives. Coverage includes APIs, infrastructure-as-code, dependencies, and runtime. Feedback links security and development for quicker mitigation.

GitHub’s "The Download" covers an npm ecosystem attack, emphasizing ongoing supply chain risks and best practices for dependency auditing.

These developments reinforce the need for prompt, actionable security and supply chain vigilance.

- [What Makes Vulnerability Scanning Effective in Fast-Moving DevSecOps Pipelines Today?](https://devops.com/what-makes-vulnerability-scanning-effective-in-fast-moving-devsecops-pipelines-today/?utm_source=rss&utm_medium=rss&utm_campaign=what-makes-vulnerability-scanning-effective-in-fast-moving-devsecops-pipelines-today)
- [The Download: npm Supply Chain Attack, NVIDIA Rubin Platform, VS Code Dev Days & More]({{ "/videos/2025-09-12-The-Download-npm-Supply-Chain-Attack-NVIDIA-Rubin-Platform-VS-Code-Dev-Days-and-More.html" | relative_url }})

### Automated Code and Secrets Security for Developers

GitHub’s CodeQL 2.23.0 now detects Rust log/path injection, broadens data modelling, and enhances detection across C/C++, C#, Java, and Python. Updates auto-deploy with code scanning to all supported languages. GitHub Enterprise Cloud introduces secret scanning validity checks for new tokens, helping teams spot exposed active secrets.

Hush Security’s platform replaces persistent application secrets with dynamic, just-in-time identity validation (using CNCF SPIFFE), supporting policy migration and automated secret management—helpful for teams running microservices or AI workloads under zero trust.

Following recent expansions in automated scanning, these updates improve detection, clarify secret validity, and reinforce best practices for secure deployment.

- [CodeQL 2.23.0: New Rust Log Injection Detection and Security Improvements](https://github.blog/changelog/2025-09-10-codeql-2-23-0-adds-support-for-rust-log-injection-and-other-security-detection-improvements)
- [Secret Scanning Validity Checks Now Available for GitHub Enterprise Cloud with Data Residency](https://github.blog/changelog/2025-09-10-secret-scanning-validity-checks-available-for-data-residency)
- [Hush Security Unveils Platform to Eliminate Application Secrets](https://devops.com/hush-security-emerges-to-eliminate-need-for-application-secrets/?utm_source=rss&utm_medium=rss&utm_campaign=hush-security-emerges-to-eliminate-need-for-application-secrets)

### Security Vulnerabilities in Developer Tools and AI Coding Workflows

A vulnerability in Cursor AI lets `"runOn": "folderOpen"` tasks execute shell commands from untrusted repos due to Workspace Trust being disabled by default. Recommended practices: enable Workspace Trust, update configurations, check for auto-execution risks, and work with unfamiliar projects only in isolated environments. The incident highlights increased risk in AI-powered developer tools and the need for careful policy updating.

This maintains last week’s focus on reviewing AI and CI tool security policies as automation use increases.

- [Security Flaw in Cursor AI Coding Tool Risks Exploiting Developers](https://devops.com/oasis-security-identifies-security-weakness-in-cursor-ai-coding-tool/?utm_source=rss&utm_medium=rss&utm_campaign=oasis-security-identifies-security-weakness-in-cursor-ai-coding-tool)
