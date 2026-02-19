---
layout: "post"
title: "New Developments in Agent Workflows, Unified AI Tools, and Secure Automation"
description: "This week's roundup covers updates in agent-driven workflows in GitHub Copilot, improved automation tools, and expanded multimodal AI integration across platforms. Microsoft continues to offer integrated AI/ML and operational analytics in Fabric, introduces productivity tools for developers, and extends secure, scalable DevOps and cloud management. Security topics include risks of multi-agent AI setups, large language model alignment methods, and adaptive governance for safer technology workflows."
author: "Tech Hub Team"
excerpt_separator: <!--excerpt_end-->
viewing_mode: "internal"
date: 2026-02-16 09:00:00 +00:00
permalink: "/2026-02-16-Weekly-AI-and-Tech-News-Roundup.html"
categories: ["AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security"]
tags: ["AI", "AI Agents", "Azure", "Cloud Computing", "Coding", "Copilot Studio", "DevOps", "GitHub Copilot", "Governance", "JetBrains", "LLM Fine Tuning", "Machine Learning", "Microsoft Fabric", "ML", "Observability", "OpenAI", "Roundups", "Security", "TypeScript", "VS", "VS Code"]
tags_normalized: ["ai", "ai agents", "azure", "cloud computing", "coding", "copilot studio", "devops", "github copilot", "governance", "jetbrains", "llm fine tuning", "machine learning", "microsoft fabric", "ml", "observability", "openai", "roundups", "security", "typescript", "vs", "vs code"]
---

Welcome to the latest weekly technology update. This edition covers recent AI advances in agent-enabled workflows for development and operations. Look for new automation features in GitHub Copilot, deeper IDE integrations, and practical ways Microsoft Fabric is simplifying analytics, machine learning, and data management. The news also includes current best practices for secure cloud deployments in Azure, a range of productivity updates, and fresh strategies for dealing with emerging security challenges in multi-agent AI and cloud environments.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Agentic Workflows: AI-Powered Automation Within GitHub Actions](#github-agentic-workflows-ai-powered-automation-within-github-actions)
  - [Expanded Model Selection and AI Integration Across Copilot Platforms](#expanded-model-selection-and-ai-integration-across-copilot-platforms)
  - [New Features and Productivity Tools in Visual Studio, JetBrains, and Copilot Ecosystem](#new-features-and-productivity-tools-in-visual-studio-jetbrains-and-copilot-ecosystem)
  - [Copilot Models and Agent Framework Updates](#copilot-models-and-agent-framework-updates)
  - [AI-Powered Workflows in Practice: Modernization, Case Studies, and Community Upskilling](#ai-powered-workflows-in-practice-modernization-case-studies-and-community-upskilling)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [AI Agent Development and Orchestration in the Microsoft Ecosystem](#ai-agent-development-and-orchestration-in-the-microsoft-ecosystem)
  - [Local AI Integration, Gamification, and SDK Parity](#local-ai-integration-gamification-and-sdk-parity)
  - [AI-Driven Enterprise Data Intelligence and Agentic Workflows](#ai-driven-enterprise-data-intelligence-and-agentic-workflows)
  - [Observability, Governance, and Security in Agent Deployments](#observability-governance-and-security-in-agent-deployments)
  - [Workflow Productivity, Semantic Search, and AI Evaluation](#workflow-productivity-semantic-search-and-ai-evaluation)
- [ML](#ml)
  - [Microsoft Fabric for ML, AI, and Operational Analytics](#microsoft-fabric-for-ml-ai-and-operational-analytics)
  - [Fine-tuning and Preference Optimization for Large Language Models on Azure](#fine-tuning-and-preference-optimization-for-large-language-models-on-azure)
- [Azure](#azure)
  - [Microsoft Fabric: Unified Data, Governance, and AI-Ready SQL](#microsoft-fabric-unified-data-governance-and-ai-ready-sql)
  - [Cloud Observability and OpenTelemetry: From SRE Automation to Migration Guidance](#cloud-observability-and-opentelemetry-from-sre-automation-to-migration-guidance)
  - [Azure Data Engineering: Cosmos DB, BizTalk Migration, Logic Apps Hybrid](#azure-data-engineering-cosmos-db-biztalk-migration-logic-apps-hybrid)
  - [Agentic Cloud Operations and Azure Copilot](#agentic-cloud-operations-and-azure-copilot)
  - [AKS Networking and Cloud Native Security](#aks-networking-and-cloud-native-security)
  - [Azure Monitor and Observability Pipeline](#azure-monitor-and-observability-pipeline)
  - [Azure Data Studio Retirement and VS Code Migration](#azure-data-studio-retirement-and-vs-code-migration)
  - [Azure Virtual Desktop and Networking Policy Changes](#azure-virtual-desktop-and-networking-policy-changes)
  - [Azure Platform Updates](#azure-platform-updates)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET Ecosystem Updates](#net-ecosystem-updates)
  - [TypeScript 6.0 Beta and Language Modernization](#typescript-60-beta-and-language-modernization)
  - [Visual Studio Code Enhancements: AI Agents and Productivity Tools](#visual-studio-code-enhancements-ai-agents-and-productivity-tools)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [Azure DevOps Server: February Patch Release](#azure-devops-server-february-patch-release)
  - [GitHub Platform Updates: Permissions, PR Controls, and Open Source Governance](#github-platform-updates-permissions-pr-controls-and-open-source-governance)
- [Security](#security)
  - [Securing Multi-Agent AI and Identity Delegation](#securing-multi-agent-ai-and-identity-delegation)
  - [Model Alignment, Memory Poisoning, and AI System Attack Surfaces](#model-alignment-memory-poisoning-and-ai-system-attack-surfaces)
  - [Copilot Studio and Power Platform Agent Security](#copilot-studio-and-power-platform-agent-security)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week’s Copilot release centers on new developer assistant features and Copilot’s evolution as a platform. The focus is on more flexible agent workflows, expanded automation, and productivity controls, alongside easier model selection and improved support for teams throughout the development lifecycle.

### GitHub Agentic Workflows: AI-Powered Automation Within GitHub Actions

Agentic Workflows are now available as an intent-based way to automate tasks on GitHub. Instead of managing manual scripts and event triggers, developers describe their objectives in natural language or Markdown, and AI agents such as Copilot, Claude, and Codex implement tasks while respecting security and compliance policies. There’s a technical overview showing setup info and how workflows tie in with GitHub Actions pipelines for safe automation, from PR reviews to CI fixes.

This draws from last week’s work on agent collaboration and VS Code automation—now extending those features to pipelines and repo-level management. Agent coordination, previously in IDE tools and Agent HQ, is now supporting automation tasks in Actions too. With Copilot, Claude, and Codex available together, the workflow moves toward declarative repo automation instead of script-heavy processes.

Another new capability is the open-source `gh aw` CLI extension, which lets you create Markdown-based workflows—without learning YAML. Debugging tools are available in VS Code, on the web, and in the CLI. Security is a core part of the design: all actions are sandboxed, read-only by default, and explicit writes are audited. Over 50 workflow templates help teams get started fast. Community cooperation among GitHub Next, Microsoft Research, and Azure specialists is driving the open-source roadmap. Early users are invited to provide input.

- [Introducing GitHub Agentic Workflows: Intent-Driven Repository Automation]({{ "/2026-02-13-Introducing-GitHub-Agentic-Workflows-Intent-Driven-Repository-Automation.html" | relative_url }})
- [GitHub Agentic Workflows: Automate Repository Tasks with AI Agents](https://github.blog/changelog/2026-02-13-github-agentic-workflows-are-now-in-technical-preview)
- [Automate Repository Tasks with GitHub Agentic Workflows](https://github.blog/ai-and-ml/automate-repository-tasks-with-github-agentic-workflows/)

### Expanded Model Selection and AI Integration Across Copilot Platforms

Model selection is now built into core Copilot workflows. New guides explain Copilot model choices—like GPT‑4.1, GPT‑5, Claude Sonnet, Haiku, Codex, and more—offering details on task suitability, cost, resource management, and enterprise policies. An “Auto” mode lets Copilot select a model automatically. Model selection is now on mobile: GitHub Mobile introduces a model picker for Pro users, including Anthropic and Codex models, on iOS and Android.

Last week Copilot added speed and more choices with Claude Opus 4.6 and faster selection, and those controls are now unified across web, IDEs, and mobile. Ongoing infrastructure updates for enterprise and self-hosted runners reflect adjustments to Copilot’s security and network patterns.

- [Choosing the Right Model in GitHub Copilot: A Practical Guide for Developers](https://techcommunity.microsoft.com/t5/microsoft-developer-community/choosing-the-right-model-in-github-copilot-a-practical-guide-for/ba-p/4491623)
- [Network Configuration Updates for Copilot Coding Agent with GitHub Actions and Azure](https://github.blog/changelog/2026-02-13-network-configuration-changes-for-copilot-coding-agent)
- [GitHub Mobile Adds Model Picker for Copilot Coding Agent](https://github.blog/changelog/2026-02-11-github-mobile-model-picker-for-copilot-coding-agent)

### New Features and Productivity Tools in Visual Studio, JetBrains, and Copilot Ecosystem

Visual Studio’s Copilot integration gets a new find_symbol tool in Chat Agent Mode—supporting in-depth symbol navigation, refactoring, and code review across C++, C#, Razor, TypeScript, and LSP-based languages. The tool offers smarter search and safer refactorings, and expands for user testing via the Insiders program.

As noted last week, Copilot’s unit test generation for .NET has advanced and is now generally available in Visual Studio 2026. Copilot automates test writing, coverage checks, and validation, building on pilot customer feedback.

For JetBrains IDE users, the Copilot plugin now has Agent Skills public preview, better onboarding, and inline chat improvements. Projects built with IntelliJ IDEA, PyCharm, or WebStorm gain improved automation, better UI, and new controls for using agent skills within team environments.

- [Unlock Language-Specific Rich Symbol Context with the New find_symbol Tool in Visual Studio Copilot Chat](https://devblogs.microsoft.com/visualstudio/unlock-language-specific-rich-symbol-context-using-new-find_symbol-tool/)
- [GitHub Copilot Testing for .NET: AI-Powered Unit Tests in Visual Studio 2026](https://devblogs.microsoft.com/dotnet/github-copilot-testing-for-dotnet-available-in-visual-studio/)
- [New Features and Improvements in GitHub Copilot for JetBrains IDEs](https://github.blog/changelog/2026-02-13-new-features-and-improvements-in-github-copilot-in-jetbrains-ides-2)
- [Visualize Workflows and Architecture with Mermaid Charts in Visual Studio 2026](https://dellenny.com/visualize-workflows-and-architecture-with-mermaid-charts-in-visual-studio-2026/)

### Copilot Models and Agent Framework Updates

OpenAI GPT-5.3-Codex is now generally available in Copilot Pro, Pro+, Business, and Enterprise plans, providing improved code generation, benchmarks, and better support for complex tasks. Business and Enterprise users can enable access through their organization, with support in VS Code, CLI, web, and mobile. The update supports more advanced automation, especially for larger teams and those with custom infrastructure.

Following last week’s Fast Mode rollout for Claude Opus 4.6, GPT-5.3-Codex continues to expand model and automation choices. There’s ongoing work in VS Code around “subagents,” which can now run in parallel and keep context separate, building on last week’s updates around multi-agent reviews and more sophisticated automation.

- [GPT-5.3-Codex Now Available in GitHub Copilot](https://github.blog/changelog/2026-02-09-gpt-5-3-codex-is-now-generally-available-for-github-copilot)
- [Subagents in VS Code: Parallel Execution and Context Isolation Explained]({{ "/2026-02-09-Subagents-in-VS-Code-Parallel-Execution-and-Context-Isolation-Explained.html" | relative_url }})

### AI-Powered Workflows in Practice: Modernization, Case Studies, and Community Upskilling

Microsoft showcases Copilot, Azure Migrate, and Azure Copilot as tools that remove legacy blockers for moving and modernizing applications, complementing last week’s focus on .NET upgrade assistance and agent-driven modernization paths.

A featured case study explores building a university clinic web app for less than $10 using Copilot Pro, Azure, and VS Code. The coverage emphasizes technical feasibility, cost control, secure builds, and how Copilot-generated scripts streamline automation.

Agents League has launched a two-week AI agent development challenge using Copilot SDK, Foundry, and Copilot Studio. Events include coding battles, templates, and badges, following the recently expanded community event schedule. This effort is designed to grow best practices and enable more developers to contribute agent workflows. GitHub’s partnership with Andela highlights Copilot certification and structured training as a way for global tech talent to learn and compete.

- [Modernizing for the AI Era: Accelerating Application Transformation with Agentic Tools](https://techcommunity.microsoft.com/t5/azure-migration-and/modernizing-for-the-ai-era-accelerating-application/ba-p/4490596)
- [Building a Professional Clinic Web App with GitHub Copilot and Azure SQL for Under $10]({{ "/2026-02-12-Building-a-Professional-Clinic-Web-App-with-GitHub-Copilot-and-Azure-SQL-for-Under-10.html" | relative_url }})
- [Agents League: Build AI Agents with Microsoft Tools in a Two-Week Challenge](https://techcommunity.microsoft.com/t5/microsoft-developer-community/agents-league-two-weeks-three-tracks-one-challenge/ba-p/4492102)
- [How Global Tech Talent Is Advancing with GitHub Copilot]({{ "/2026-02-11-How-Global-Tech-Talent-Is-Advancing-with-GitHub-Copilot.html" | relative_url }})

### Other GitHub Copilot News

Operational reliability remains a priority for the Copilot team. The January Availability Report outlines recent Copilot incidents, root causes, and fixes. Status Page improvements add a 90-day incident record, detailed outage information, and better regional view. New CI/CD reports now clearly list the impact to runners and models, providing more transparency.

These observations carry over from last week’s emphasis on improved workflow validation, governance practices, and enhanced incident insight for both enterprises and open source Copilot teams.

- [GitHub Availability Report: January 2026](https://github.blog/news-insights/company-news/github-availability-report-january-2026/)
- [Improved GitHub Status Page with Enhanced Incident Visibility](https://github.blog/changelog/2026-02-13-updated-status-experience)

## AI

This week’s AI news highlights agent developer tooling, enterprise data integration, and continued expansion of Microsoft’s agent framework. You’ll find new updates for the AI Toolkit in VS Code, local agent development with Foundry, and broader coverage of governance and educational use.

### AI Agent Development and Orchestration in the Microsoft Ecosystem

AI Toolkit for VS Code (v0.30.0) now features a single Tool Catalog for easy agent tool management. The new Agent Inspector lets developers set breakpoints, examine variables, debug, and visualize workflow steps all in one place. Unit tests use pytest syntax and the Eval Runner SDK, running outputs through Data Wrangler and scaling up with Foundry. Model support is being extended (including gpt-5.2-codex), and productivity tools help bring agent workflows directly into standard development.

This continues last week’s focus on unifying the agent automation experience, from initial Copilot traces to full workflow management within VS Code. Greater support for agent coordination, strict policy controls, and human checkpoints is now easier for everyday developers to use.

For teams with privacy requirements, new guidance explores how Foundry Local and the Microsoft Agent Framework (MAF) enable local, research-grade agent workflows. Features include modular composition, OpenTelemetry for observability, built-in security practices (such as red teaming and privacy-by-design), and debugging with DevUI and .NET Aspire instrumentation. The articles provide practical instructions for building policy-compliant, cost-managed AI agents.

These improvements build on last week’s up-to-date model deployments with audit trails, continuing to prioritize flexibility and compliance.

An in-depth post walks through the Microsoft Learn MCP Server, released in 2025, which provides programmatic access to Microsoft Learn docs and code samples for agents. Architecture decisions like using Azure App Service, semantic and vector search, and distributed tool management support improved agent data retrieval and secure automation workflows. This extends last week’s trend of feeding current technical reference into Copilot, reinforcing Microsoft’s investment in agent awareness and developer experience.

- [AI Toolkit for VS Code: February 2026 Major Update (v0.30.0)](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-toolkit-for-vs-code-february-2026-update/ba-p/4493673)
- [Building Deep Research Agent Workflows with Microsoft Foundry Local and Agent Framework](https://devblogs.microsoft.com/semantic-kernel/from-local-models-to-agent-workflows-building-a-deep-research-solution-with-microsoft-agent-framework-on-microsoft-foundry-local/)
- [How We Built the Microsoft Learn MCP Server: Empowering AI Agents with Trusted Documentation](https://devblogs.microsoft.com/engineering-at-microsoft/how-we-built-the-microsoft-learn-mcp-server/)

### Local AI Integration, Gamification, and SDK Parity

Foundry Local is in use for privacy-focused, lightweight local AI integration—such as enabling in-game personality features in browser games, powered by models like phi-3.5-mini and event-driven JavaScript. The tools support asynchronous, pluggable functionality, offer offline support, and are customizable for low-cost development.

These efforts build on last week’s advancements in local evaluation and open infrastructure for building AI-powered features with privacy and cost control.

Gamified learning is now part of AI education. There’s a blueprint for creating browser and CLI learning environments on Foundry Local, featuring prompt engineering, workflow modules, and tool development in JavaScript, ES6 modules, and Node.js, offering a structured entry point for students and new developers.

Enterprise users of Azure AI Foundry should note that Python SDK supports agent memory out of the box, while C# does not have feature parity yet. .NET developers are encouraged to monitor the SDK roadmap and custom solutions until built-in support becomes available.

- [Adding AI Personality to Browser Games with Microsoft Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/adding-ai-personality-to-browser-games/ba/p/4490892)
- [Teaching AI Development Through Gamification: Building with Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/teaching-ai-development-through-gamification/ba-p/4490755)
- [Agent Memory Abstractions in Azure AI Foundry: Python vs C# SDKs](https://techcommunity.microsoft.com/t5/azure/missing-equivalent-for-python-memorysearchtool-and/m-p/4494284#M22429)

### AI-Driven Enterprise Data Intelligence and Agentic Workflows

Microsoft IQ and the Fabric IQ platforms support agent-driven integration for enterprise data. New materials show how modules like Work IQ, Foundry IQ, and Fabric IQ automate workflow management, model integration, and data discovery for Azure-based companies.

The Fabric IQ Agents platform includes both a semantic ontology and a Fabric Graph, allowing organizations to use natural language to explore and automate work with live data. Security and compliance are managed by Entra ID and Fabric policies. Integration hooks for analytics and operations mean teams can build custom and secure workflows, blending no-code and advanced developer controls.

This information builds on last week’s coverage of workflow automation and strict AI governance, showing how agent platforms now connect to knowledge and policy across larger organizations.

- [Microsoft IQ Overview: Exploring Work IQ, Foundry IQ, and Fabric IQ]({{ "/2026-02-09-Microsoft-IQ-Overview-Exploring-Work-IQ-Foundry-IQ-and-Fabric-IQ.html" | relative_url }})
- [Fabric IQ Agents: Bridging Enterprise Data and AI](https://zure.com/blog/fabric-iq-agents-operate-hand-to-hand-with-enterprise-data)

### Observability, Governance, and Security in Agent Deployments

At enterprise scale, deploying AI agents reveals ongoing challenges around observability, governance controls, and policy enforcement. Microsoft’s Cyber Pulse report encourages adopting Zero Trust: least privilege, real-time monitoring, and clearly defined roles. Agent management covers registry, identity, telemetry, policy compatibility, and security. Real-world scenarios include integrating Copilot Studio or Agent Builder with existing enterprise tooling, using registries for transparency and runtime checks for compliance. Technical guides help teams deliver solutions that are secure, observable, and policy aligned.

This mirrors last week’s emphasis on trackable and audit-ready agent deployments, moving from test environments to steady-state enterprise systems.

- [AI Agents in the Enterprise: Observability, Governance, and Security Insights from Microsoft's Cyber Pulse Report](https://www.microsoft.com/en-us/security/blog/2026/02/10/80-of-fortune-500-use-active-ai-agents-observability-governance-and-security-shape-the-new-frontier/)

### Workflow Productivity, Semantic Search, and AI Evaluation

The TypeScript team continues to automate repetitive work (like porting PRs) with AI, promoting automation to help other teams free up cycles and improve workflow reliability.

GitHub’s public preview of semantic search for Issues offers context-based search results, helping projects track bugs and features more efficiently.

The GPT-5 pro Evaluation Challenge demonstrates how to build hands-on AI evaluation pipelines with Foundry and Azure AI, including guides and examples for setting up quality workflows.

Low-code and no-code tools for AI are gaining ground—recent programming sessions show designers and developers building apps directly in VS Code with AI agents, using agent steering, multi-agent management, and mobile integration for practical development.

- [How TypeScript's Creator Uses AI for Team Productivity]({{ "/2026-02-14-How-TypeScripts-Creator-Uses-AI-for-Team-Productivity.html" | relative_url }})
- [GitHub Issues Semantic Search Public Preview]({{ "/2026-02-09-GitHub-Issues-Semantic-Search-Public-Preview.html" | relative_url }})
- [GPT-5 pro Evaluation Challenge – Evaluating AI Tools with Microsoft Foundry and Azure AI]({{ "/2026-02-11-GPT-5-pro-Evaluation-Challenge-Evaluating-AI-Tools-with-Microsoft-Foundry-and-Azure-AI.html" | relative_url }})
- [Building an App in VS Code with AI Agents with Elijah King | Cozy AI Kitchen]({{ "/2026-02-10-Building-an-App-in-VS-Code-with-AI-Agents-with-Elijah-King-Cozy-AI-Kitchen.html" | relative_url }})

## ML

This week’s ML coverage spotlights Microsoft Fabric’s expansion of analytics and machine learning, with practical routes for ML in production, fine-tuning workflows, and automated pipelines. Tools like Semantic Link and Foundry fine-tuning offer easier, AI-driven analytics and operational intelligence.

### Microsoft Fabric for ML, AI, and Operational Analytics

Microsoft Fabric now better unifies analytics, machine learning, and business reporting.

Semantic Link is now generally available, allowing a shared semantic layer for data engineering, AI, and BI to use common models. It supports semantic model updates directly from notebooks, immediate sync to Power BI, and harmonized workflows. Automation is easier with tighter SQL/Spark orchestration, while community repositories provide reusable patterns.

For IoT and streaming data, Fabric’s operational analytics uses time series dashboards with Kusto, dynamic slicing, anomaly detection, and DirectQuery for live reporting. These updates expand the platform’s ability to handle large-scale, real-time data.

ML workflows in Fabric and Power BI are progressing, letting teams run predictions in dashboards using LightGBM/SMOTE, OneLake-backed data, and MLflow for automation. The Fabric IQ platform provides the foundation for digital twins and ontologies, supporting smarter knowledge and automation development.

- [Supercharge AI, BI, and Data Engineering with Semantic Link in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/supercharge-ai-bi-and-data-engineering-with-semantic-link-generally-available/)
- [Adaptive Time Series Visualization at Scale with Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/adaptive-time-series-visualization-at-scale-with-microsoft-fabric/)
- [Integrating Machine Learning with Power BI Reports in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/enrich-power-bi-reports-with-machine-learning-in-microsoft-fabric/)
- [Fabric IQ Overview]({{ "/2026-02-11-Fabric-IQ-Overview.html" | relative_url }})

### Fine-tuning and Preference Optimization for Large Language Models on Azure

A hands-on guide is available for fine-tuning enterprise LLMs with Microsoft Foundry on Azure, taking models and aligning them for organization-specific requirements and policies. The documentation covers data prep, running training jobs, and benchmarking—applying methods to use cases like PubMed summarization for health and science.

Direct Preference Optimization (DPO) is also explained, detailing how human feedback can steer LLMs toward better outputs. DPO is now in the Foundry SDK, and tutorials include example code, best practices for parameter selection, and links to new documentation.

- [Beyond the Prompt – Why and How to Fine-tune Your Own Models](https://devblogs.microsoft.com/foundry/beyond-the-prompt-why-and-how-to-fine-tune-your-own-models/)
- [DPO Fine-Tuning Using Microsoft Foundry SDK](https://devblogs.microsoft.com/foundry/dpo-fine-tuning-using-microsoft-foundry-sdk/)

## Azure

Recent Azure news includes new guides and updates for data platforms, security, workflow automation, and migration tools. Azure is continuing to add support for industry standards, integrate closely with Fabric and Databricks, provide Logic Apps and infrastructure enhancements, and improve compliance, networking, and platform reliability.

### Microsoft Fabric: Unified Data, Governance, and AI-Ready SQL

Fabric’s new SQL Database is SaaS-native, exposing operational tables as Delta for instant analytics in OneLake. Enhanced Copilot features now in the SQL endpoint and Query Editor let you run queries in plain English and troubleshoot schemas. The OneLake Catalog centralizes data discovery, while sensitivity labels and other enterprise options automate regulatory compliance. New CI/CD features (SqlPackage, Terraform, CLI) promote repeatable deployments. SQLCon and FabCon hype the growing ecosystem through technical deep dives.

- [SQL Database in Microsoft Fabric: SaaS-Native, AI-Ready Platform](https://blog.fabric.microsoft.com/en-US/blog/sql-database-in-fabric-built-for-saas-ready-for-ai/)
- [OneLake Catalog: Unified Data Discovery and Governance in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/onelake-catalog-the-trusted-catalog-for-organizations-worldwide/)
- [Automating Data Governance in Microsoft Fabric with Default Domain Labels](https://blog.fabric.microsoft.com/en-US/blog/governance-on-autopilot-the-power-of-default-domain-labels-in-fabric-generally-available/)
- [Five Reasons to Attend SQLCon: A Deep Dive Into SQL Server, Azure SQL, and Microsoft Fabric](https://blog.fabric.microsoft.com/en-us/blog/32624)

### Cloud Observability and OpenTelemetry: From SRE Automation to Migration Guidance

Application Insights SDK 3.x for .NET now makes it easier to use OpenTelemetry, helping with observability in both legacy and new apps. The Azure SRE Agent with Model Context Protocol (MCP) keeps building agent-based automation for Databricks compliance and incident response. New SCOM-to-Azure Monitor migration tools create compatible analysis reports and ARM templates, supporting the move to more automated, modern monitoring.

- [Application Insights SDK 3.x for .NET: Easier Migration to OpenTelemetry](https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-application-insights-sdk-3-x-for-net/ba-p/4493988)
- [Automate Databricks Compliance and Incident Response with Azure SRE Agent and Model Context Protocol](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/mcp-driven-azure-sre-for-databricks/ba-p/4494630)
- [Azure WAF Compliance with MCP-Driven SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-waf-compliance-with-mcp-driven-sre-agent/ba-p/4494687)
- [Accelerating SCOM to Azure Monitor Migrations with Automated Analysis and ARM Template Generation](https://techcommunity.microsoft.com/t5/azure-observability-blog/accelerating-scom-to-azure-monitor-migrations-with-automated/ba-p/4493593)

### Azure Data Engineering: Cosmos DB, BizTalk Migration, Logic Apps Hybrid

A new Pantone case study details an AI agent app running on Cosmos DB, leveraging vector search and orchestration for chatbot scalability.

The BizTalk Migration Starter toolkit helps teams move from legacy BizTalk to Logic Apps. New Logic Apps features (e.g., Arc-enabled AKS, Jumpstart templates) provide hybrid and multi-cloud deployment paths, streamlining a broad range of operations.

- [The Data Behind Pantone's Agentic AI: Building on Azure Cosmos DB](https://azure.microsoft.com/en-us/blog/the-data-behind-the-design-how-pantone-built-agentic-ai-with-an-ai-ready-database/)
- [BizTalk Migration Starter: Open Source Toolkit for Migrating BizTalk Applications to Azure Logic Apps](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/a-biztalk-migration-tool-from-orchestrations-to-logic-apps/ba-p/4494876)
- [Deploying Hybrid Logic Apps with Azure Arc on AKS Using Jumpstart Templates](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/azure-arc-jumpstart-template-for-hybrid-logic-apps-deployment/ba-p/4493996)

### Agentic Cloud Operations and Azure Copilot

Azure Copilot is rolling out production-ready agent-based cloud management, moving beyond previous automation to centralize resource discovery, enforce policy, and automate compliance—all through a user-friendly portal and with RBAC and BYOS flexibility.

- [Agentic Cloud Operations with Azure Copilot: Transforming Cloud Management with AI-Driven Agents](https://azure.microsoft.com/en-us/blog/agentic-cloud-operations-a-new-way-to-run-the-cloud/)

### AKS Networking and Cloud Native Security

AKS upgrades now bring nftables support (for scaling and Project Calico integration) and confidential VMs on Azure Government Cloud, improving security for zero-trust and regulated scenarios.

- [Scaling AKS Networking with nftables and Project Calico](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/beyond-iptables-scaling-aks-networking-with-nftables-and-project/ba-p/4494467)
- [DCasv6 and ECasv6 Confidential Virtual Machines in Azure Government Cloud](https://techcommunity.microsoft.com/t5/azure-confidential-computing/dcasv6-and-ecasv6-confidential-vms-in-azure-government-cloud/ba-p/4494604)

### Azure Monitor and Observability Pipeline

Azure Monitor introduces new pipeline transformation features and AI-based workflows for App Service, supporting web agent hosting, OpenAI integration, and improved operations.

- [Public Preview: Azure Monitor Pipeline Transformations](https://techcommunity.microsoft.com/t5/azure-observability-blog/public-preview-azure-monitor-pipeline-transformations/ba-p/4491980)
- [From Local MCP Server to Hosted Web Agent: AI-Powered App Service Observability on Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-local-mcp-server-to-hosted-web-agent-app-service/ba-p/4493241)

### Azure Data Studio Retirement and VS Code Migration

Azure Data Studio will reach end of life on February 28. Guidance now explains SQL developer migration to the VS Code MSSQL Extension, helping teams retain backup and workflow tools.

- [Azure Data Studio Retirement: Transitioning SQL Development to VS Code]({{ "/2026-02-12-Azure-Data-Studio-Retirement-Transitioning-SQL-Development-to-VS-Code.html" | relative_url }})

### Azure Virtual Desktop and Networking Policy Changes

The “Default Outbound Access” feature in Azure networking will be retired for new VNets, requiring users to define NAT and outbound policies for Azure Virtual Desktop deployments. Updated troubleshooting for SSO (macOS and Windows 11 25H2) and RDP client connection issues is covered.

- [Azure Network Changes: Default Outbound Access Removal and Implications for Azure Virtual Desktop](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/azure-s-default-outbound-access-changes-guidance-for-azure/m-p/4494462#M14000)
- [SSO Issues on Azure Virtual Desktop for macOS Clients After Windows 11 25H2 Update](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/macos-sso-no-longer-fully-functional-on-avd-win11-25h2/m-p/4494544#M14001)
- [Troubleshooting AVD Client Connection Issues in Windows App vs Web Client](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/your-computer-was-unable-to-connect-to-the-remote-computer/m-p/4494411#M13999)

### Azure Platform Updates

This week’s Azure Update details further modernization improvements: AKS now supports Kubernetes 1.34, additional disk backup, SQL secondary replicas, wider AI/Databricks support, the Thailand South region, and Entra identity updates—contributing to platform reliability and regional extension.

- [Azure Update - Friday the 13th February 2026]({{ "/2026-02-13-Azure-Update-Friday-the-13th-February-2026.html" | relative_url }})

### Other Azure News

Azure Developer CLI (`azd`) now supports deployment slots for App Service, making repeatable deployments easier and boosting productivity. CLI reliability updates have also rolled out.

- [Deploy to Azure App Service Deployment Slots with azd](https://devblogs.microsoft.com/azure-sdk/azd-app-service-slot/)

Recent compliance automation (manual update for Data Gateway, new FinOps Toolkit, Sovereign and banking landing zone architectures) helps organizations streamline governance and deployment across regulated industries.

- [Manual Update Feature for On-Premises Data Gateway (Public Preview)](https://blog.fabric.microsoft.com/en-US/blog/manual-update-for-on-premises-data-gateway-public-preview/)
- [What’s New in FinOps Toolkit 13: January 2026 Feature Updates](https://techcommunity.microsoft.com/t5/finops-blog/what-s-new-in-finops-toolkit-13-january-2026/ba-p/4493090)
- [Deploy VMs on Azure Local with Portal, CLI & Bicep (IaC)](https://www.thomasmaurer.ch/2026/02/deploy-vms-on-azure-local-with-portal-cli-bicep-iac/)
- [Sovereign Landing Zones for Microsoft Azure: Architecture and Compliance Explained](https://www.thomasmaurer.ch/2026/02/sovereign-landing-zones-for-microsoft-azure-slz-explained/)
- [Azure Landing Zone for Indian Banks: Regulatory-Ready Architecture and Compliance](https://techcommunity.microsoft.com/t5/azure-migration-and/azure-landing-zone-and-compliance-for-banks-indian-banks/ba-p/4491951)

Modern SharePoint architecture guidance is now available, carrying forward best practices for governance automation, Microsoft 365 integration, and secure, scalable sites.

- [Modern SharePoint Architecture: Best Practices for Scalable Intranets in 2026](https://dellenny.com/modern-sharepoint-architecture-best-practices-for-scalable-intranets-in-2026/)

Microsoft is publishing new work on using high-temperature superconductors to increase datacenter energy efficiency—adding to ongoing initiatives for energy savings as the platform scales.

- [How High-Temperature Superconductors Could Revolutionize Power Delivery in Microsoft Datacenters](https://azure.microsoft.com/en-us/blog/can-high-temperature-superconductors-transform-the-power-infrastructure-of-datacenters/)

Guidance is out for addressing webhook authentication issues with Azure Event Grid, ACS, and Dynamics 365, supporting secure integration and event management.

- [Resolving Azure Event Grid Entra Authentication Issues for ACS and Dynamics 365 Webhooks](https://techcommunity.microsoft.com/t5/azure/how-to-fix-azure-event-grid-entra-authentication-issue-for-acs/m-p/4494308#M22430)

## Coding

Programming languages and tools see several updates this week, with new features in .NET and TypeScript, and productivity improvements for VS Code.

### .NET Ecosystem Updates

.NET 11 Preview 1 brings a range of updates: Zstandard compression, BFloat16 data type, better ZipArchiveEntry operations, new hard-link and crypto APIs, and collection improvements. Runtime features include async main, WebAssembly CoreCLR, interpreter/JIT updates, and more hardware support.

SDK tools see improved device selection (`dotnet run`), easier test syntax, better watch/hot reload, static analyzers, and enhanced MSBuild logging. The languages get new collections (C#), parallel F# builds, and speed increases. ASP.NET Core/Blazor gets a new UI, SignalR, and better cert management. .NET MAUI now uses CoreCLR for Android. Entity Framework Core is updated for complex JSON column types. Both VS 2026 and VS Code C# Dev Kit support these changes.

February’s .NET and .NET Framework servicing update targets supported versions and addresses CVE-2026-21218. Guidance is provided for installation, patch verification, and changelog review. These continue last week’s trend toward platform modernization, with regular previews and focus on compatibility.

- [.NET 11 Preview 1: New Features and Improvements Across the Ecosystem](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-1/)
- [.NET and .NET Framework February 2026 Servicing Releases Update](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-february-2026-servicing-updates/)

### TypeScript 6.0 Beta and Language Modernization

TypeScript 6.0 Beta will be the last version built in JavaScript before 7.0 moves to Go. Updates include changes to context-sensitive function declarations, easier Node.js imports, combined resolution/output settings, and deterministic type ordering via `--stableTypeOrdering`.

Several features are now deprecated: strict mode is default, modules align to current standards, legacy settings are dropped, iterables are always available in DOM libraries, and explicit `types` fields are required. Migration is eased with tools like `ts5to6` and configuration tips. Developers are urged to begin testing their codebases and try out 7.0 Go-native builds for feedback and confidence.

These updates continue last week’s focus on rapid TypeScript iteration, improved compatibility with AI code tools, and preparing developer teams for future platform updates.

- [Announcing TypeScript 6.0 Beta: Key Features, Deprecations, and Migration Guide](https://devblogs.microsoft.com/typescript/announcing-typescript-6-0-beta/)

### Visual Studio Code Enhancements: AI Agents and Productivity Tools

VS Code 1.109 adds usability and workflow improvements. You can now "Ask Questions" in the editor, use agent skills for code automation, and run subagents in parallel for advanced tasks. Editor changes include double-click selection of brackets and strings, a browser preview, and upgraded MCP cloud app support.

A video demo shows agent steering in VS Code Insiders, letting you queue and control agent tasks directly for repeatable, controlled workflow automation. These additions build on the theme of expanded automation, agent control, and integration for both new and advanced users.

- [VS Code 1.109 Release Highlights: Editor Improvements & New Features]({{ "/2026-02-11-VS-Code-1109-Release-Highlights-Editor-Improvements-and-New-Features.html" | relative_url }})
- [Let it Cook: Agent Steering & Queueing in VS Code Insiders]({{ "/2026-02-09-Let-it-Cook-Agent-Steering-and-Queueing-in-VS-Code-Insiders.html" | relative_url }})

### Other Coding News

A recent Rx.NET v7 live session covers new asynchronous APIs and event stream features, adoption recommendations, and future plans, all presented by Rx.NET team engineers.

- [On .NET Live: Rx.NET v7 and Futures]({{ "/2026-02-11-On-NET-Live-RxNET-v7-and-Futures.html" | relative_url }})

## DevOps

In DevOps, teams benefit from updated Azure DevOps Server patches, finer-grained GitHub permissions, and governance tools that help keep collaborative workflows secure as automation and AI-driven contribution increase.

### Azure DevOps Server: February Patch Release

The February 2026 Azure DevOps Server patches provide updates for 2022.2, 2020.1.2, and 2019.1.2. Each release contains patch notes, install scripts, and instructions for validation. Teams should run '<patch-installer>.exe CheckInstall' when updating. Staying current with these releases helps organizations keep systems secure and minimize downtime.

- [February Patches for Azure DevOps Server](https://devblogs.microsoft.com/devops/february-patches-for-azure-devops-server-5/)

### GitHub Platform Updates: Permissions, PR Controls, and Open Source Governance

GitHub Apps now support fine-grained permissions for Enterprise Teams APIs, allowing more secure automation and audit tracking. Repository settings can now restrict or disable PRs, providing new controls as AI-generated pull requests grow more common.

Open source maintainers now have new contributor controls: better navigation for large diffs, UI upgrades, banners, reputation-based gating, and the "vouch" trust management system. These changes help automate standards and build safe, scalable CI/CD pipelines.

This is a continuation of last week’s trend toward enhanced workflow controls, permission systems, and automated community management—vital as global contributor numbers, both human and AI, continue to grow.

- [GitHub Apps Gain Fine-Grained Permissions for Enterprise Teams APIs](https://github.blog/changelog/2026-02-09-github-apps-can-now-utilize-public-preview-enterprise-teams-apis-via-fine-grained-permissions)
- [New GitHub Settings to Configure Pull Request Access](https://github.blog/changelog/2026-02-13-new-repository-settings-for-configuring-pull-request-access)
- [Open Source’s Eternal September: Supporting Maintainers Amid the Scaling Contributor Wave](https://github.blog/open-source/maintainers/welcome-to-the-eternal-september-of-open-source-heres-what-we-plan-to-do-for-maintainers/)
- [Open Source Friday: Trust Management with Vouch and GitHub]({{ "/2026-02-12-Open-Source-Friday-Trust-Management-with-Vouch-and-GitHub.html" | relative_url }})

## Security

Recent security work focuses on the challenges of agent-driven and automated cloud systems—including access management, large language model (LLM) alignment, and defending against misconfiguration or malicious input.

### Securing Multi-Agent AI and Identity Delegation

Multi-agent AI environments raise complex access and identity challenges. Microsoft's new guides cover secure orchestration and user/agent actions delegation via Entra ID’s On-Behalf-Of flow, using frameworks like LangGraph, Chainlit, and Databricks Genie, and enabling zero-trust RBAC through Unity Catalog. Solutions for OAuth token scopes, custom providers, and audience management are explained, alongside audit and human oversight best practices for accountable agent automation.

This extends last week’s focus on practical patterns for safe, large-scale agent deployments.

- [Securing Multi-Agent AI Solutions with Microsoft Entra ID On-Behalf-Of Flow](https://techcommunity.microsoft.com/t5/azure-architecture-blog/securing-a-multi-agent-ai-solution-focused-on-user-context-the/ba-p/4493308)
- [Securing Multi-Agent AI with User Context: Entra ID OBO for Databricks Genie](https://techcommunity.microsoft.com/t5/azure-architecture-blog/securing-multi-agent-ai-with-user-context-entra-id-obo-for/ba/p/4493308)

### Model Alignment, Memory Poisoning, and AI System Attack Surfaces

Recent research reveals that enterprise LLMs are vulnerable to alignment attacks: for example, one adversarial prompt can undermine safety when using Group Relative Policy Optimization during fine-tuning. Developers are reminded to run benchmarks throughout adaptation cycles to detect model drift. The Defender security team also discusses memory poisoning attacks—where prompt injection targets Copilot or similar tools—offering guidelines for detection, filtering, interface design, and memory controls.

This section continues last week’s coverage of model lifecycle and runtime safety. Behavioral monitoring and input validation are necessary to keep AI-driven systems secure.

- [A One-Prompt Attack That Breaks LLM Safety Alignment](https://www.microsoft.com/en-us/security/blog/2026/02/09/prompt-attack-breaks-llm-safety/)
- [Protecting AI Systems Against Memory Poisoning: The Rise of AI Recommendation Poisoning](https://www.microsoft.com/en-us/security/blog/2026/02/10/ai-recommendation-poisoning/)

### Copilot Studio and Power Platform Agent Security

Copilot Studio automation introduces new security requirements. Microsoft’s top 10 risk list provides tactics and mitigation tips: enforcing authentication, moving secrets to Key Vault, reviewing dormant agents, and restricting command scopes. This helps both no-code and low-code environments manage exposure as agent automation grows.

These practical guides pick up where last week’s discussion of operational guardrails left off and provide actionable checklists for teams working with agent platforms.

- [Copilot Studio Agent Security: Top 10 Risks and How to Mitigate Them](https://www.microsoft.com/en-us/security/blog/2026/02/12/copilot-studio-agent-security-top-10-risks-detect-prevent/)

### Other Security News

Dependabot audit logs have expanded to capture all enable/disable and config activities (including for self-hosted runners), recording user identity for better compliance and traceability.

Building on last week’s improvements to Dependabot OIDC and registry security, these updates support safer CI/CD and supply chains.

- [Track Additional Dependabot Configuration Changes in Audit Logs](https://github.blog/changelog/2026-02-10-track-additional-dependabot-configuration-changes-in-audit-logs)

Microsoft has released a SIEM buyer’s guide explaining how to pick platforms that are ready for AI-based security management. Sentinel and other products offer model-based analytics and automation, in line with current best practices.

- [The Strategic SIEM Buyer’s Guide: Selecting an AI-Ready Security Platform](https://www.microsoft.com/en-us/security/blog/2026/02/11/the-strategic-siem-buyers-guide-choosing-an-ai-ready-platform-for-the-agentic-era/)
