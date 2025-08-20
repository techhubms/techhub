---
layout: "post"
title: "AI Agents and Developer Workflow Revolution: GPT-5, Copilot, Platform Deepening, and Cloud Security Lead the Week"
description: "This week, GitHub Copilot added full GPT-5 integration and expanded its agent capabilities, moving from code suggestions to project-aware, conversational automation. AI-driven workflow and project support grew more effective across IDEs and the web. Microsoft’s Azure, Fabric, and AI platforms introduced practical tooling for document automation, enterprise deployment, and open-source LLMs, reflecting increased focus on reliable production use. Security remained a top priority with quick responses to critical vulnerabilities, stronger supply chain protection, and broader AI-powered threat detection."
author: "Tech Hub Team"
excerpt_separator: <!--excerpt_end-->
viewing_mode: "internal"
date: 2025-08-18 09:00:00 +00:00
permalink: "/2025-08-18-Weekly-AI-and-Tech-News-Roundup.html"
categories: ["AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security"]
tags: [".NET", ".NET MAUI", "Agentic Automation", "AI", "Azure", "C# 14", "Claude 4.1", "Cloud Infrastructure", "Coding", "Data Security", "DevOps", "Enterprise Security", "GitHub Copilot", "GPT 5", "MCP Server", "Microsoft Fabric", "ML", "MLOps", "Model Context Protocol", "Open Source LLMs", "Roundups", "Security", "Visual Studio Code"]
tags_normalized: ["net", "net maui", "agentic automation", "ai", "azure", "c sharp 14", "claude 4 dot 1", "cloud infrastructure", "coding", "data security", "devops", "enterprise security", "github copilot", "gpt 5", "mcp server", "microsoft fabric", "ml", "mlops", "model context protocol", "open source llms", "roundups", "security", "visual studio code"]
---

Welcome to the latest tech roundup. This week saw notable progress in AI-powered development and workflow automation. GitHub Copilot advanced from a code completion tool to a context-aware developer assistant backed by models such as GPT-5 and Claude 4.1. With the Model Context Protocol (MCP) available and new agentic modes—like Beast Mode and Do Epic Shit—Copilot now provides transparent, auditable automation, tightly integrated in IDEs and web development. These updates directly address requests for more automation, enterprise features, and clarity in developer tools.

Microsoft platforms continued to mature, with open-source language models, updated optimization tools, and unified SDKs for .NET, Python, and JavaScript. Azure upgraded its cloud, security, and data offerings with enhanced governance and hybrid/multi-cloud capabilities. ML and DevOps teams benefited from new optimizers, scalable LLM deployment patterns, and improved observability, while security professionals handled urgent Exchange and SharePoint vulnerabilities with both AI-powered responses and broader supply chain protections. Leadership changes, broader AI adoption, and improved automation are shaping the technology landscape as the fall season approaches.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Generative AI Models Evolve: GPT-5 and Claude 4.1 Arrive](#generative-ai-models-evolve-gpt-5-and-claude-41-arrive)
  - [Copilot's Agentic and Contextual Intelligence: Model Context Protocol and Agent Modes](#copilots-agentic-and-contextual-intelligence-model-context-protocol-and-agent-modes)
  - [Agent Autonomy and Workflow Discipline: Beast Mode and Epic Chat Patterns](#agent-autonomy-and-workflow-discipline-beast-mode-and-epic-chat-patterns)
  - [Conversational AI Workflows and Web-Based Copilot Expansion](#conversational-ai-workflows-and-web-based-copilot-expansion)
  - [AI-Powered Code Modernization and Database Debugging](#ai-powered-code-modernization-and-database-debugging)
  - [Integrated Ecosystem: Specialized AI Assistants and Repository-Wide Features](#integrated-ecosystem-specialized-ai-assistants-and-repository-wide-features)
  - [Other News](#other-news)
- [AI](#ai)
  - [GPT-5 Rollout and Deep Ecosystem Integration](#gpt-5-rollout-and-deep-ecosystem-integration)
  - [Open-Source LLMs and Local AI Empowerment](#open-source-llms-and-local-ai-empowerment)
  - [Enterprise Agentic AI Matures](#enterprise-agentic-ai-matures)
  - [Document Intelligence and Advanced Automation on Azure](#document-intelligence-and-advanced-automation-on-azure)
  - [Next-Generation Conversational and No-Code AI](#next-generation-conversational-and-no-code-ai)
  - [Core Platform and Leadership Shifts](#core-platform-and-leadership-shifts)
  - [Security, Code Quality, and Responsible AI](#security-code-quality-and-responsible-ai)
  - [Other News](#other-news)
- [ML](#ml)
  - [Scalable Optimization for Billion-Parameter Models](#scalable-optimization-for-billion-parameter-models)
  - [Cloud Infrastructure and LLM Deployment Patterns Mature](#cloud-infrastructure-and-llm-deployment-patterns-mature)
  - [Unified Data Lake Analytics and Open Format Interoperability](#unified-data-lake-analytics-and-open-format-interoperability)
  - [Tuning and Workflow Evolution in ML Engineering](#tuning-and-workflow-evolution-in-ml-engineering)
  - [Other News](#other-news)
- [Azure](#azure)
  - [Container Management Leadership and Secure Foundations](#container-management-leadership-and-secure-foundations)
  - [Expanded Data Services and Enhanced Observability](#expanded-data-services-and-enhanced-observability)
  - [Cloud Storage Innovation and Cost Optimization](#cloud-storage-innovation-and-cost-optimization)
  - [Integrated Analytics, Automation, and Developer Productivity](#integrated-analytics-automation-and-developer-productivity)
  - [Simplified, Predictable Data Management with Microsoft Fabric OneLake](#simplified-predictable-data-management-with-microsoft-fabric-onelake)
  - [Other News](#other-news)
- [Coding](#coding)
  - [Advancements in C# Language and Ecosystem](#advancements-in-c-language-and-ecosystem)
  - [.NET 10 and Platform Momentum](#net-10-and-platform-momentum)
  - [Web Development with ASP.NET Core & Blazor and .NET Aspire](#web-development-with-aspnet-core-blazor-and-net-aspire)
  - [Next-Gen Data Mapping in .NET: The Facet Approach](#next-gen-data-mapping-in-net-the-facet-approach)
  - [Cross-Platform and Browser-Based .NET: MAUI and WASM](#cross-platform-and-browser-based-net-maui-and-wasm)
  - [Other News](#other-news)
- [DevOps](#devops)
  - [Coding Agent Capabilities Expand](#coding-agent-capabilities-expand)
  - [GitHub and Azure: Enhanced Security, Productivity, and Governance](#github-and-azure-enhanced-security-productivity-and-governance)
  - [AI-Powered Market Intelligence and Observability](#ai-powered-market-intelligence-and-observability)
  - [Other News](#other-news)
- [Security](#security)
  - [Critical Infrastructure Vulnerabilities Drive Immediate Action](#critical-infrastructure-vulnerabilities-drive-immediate-action)
  - [Security Copilot and AI-Powered Defenses Broaden Their Reach](#security-copilot-and-ai-powered-defenses-broaden-their-reach)
  - [Code, Secrets, and the Modern Software Supply Chain](#code-secrets-and-the-modern-software-supply-chain)
  - [Enterprise Identity, Cloud, and Data Security Matures](#enterprise-identity-cloud-and-data-security-matures)
  - [Proactive and Automated Exposure Management](#proactive-and-automated-exposure-management)
  - [Expanding and Securing Data Governance](#expanding-and-securing-data-governance)
  - [Security Updates and Regulatory Readiness](#security-updates-and-regulatory-readiness)
  - [Other News](#other-news)

## GitHub Copilot

Continuing last week’s model integration efforts, GitHub Copilot is becoming more than a code completion tool—emerging as a core, intelligent assistant for developers. With GPT-5 support, expanded Model Context Protocol (MCP) coverage, new chat modes, and repository-wide Copilot Spaces, development is becoming more contextual, automated, and transparent. These features are shaped by ongoing developer feedback, strengthening Copilot’s use in both team and enterprise settings.

### Generative AI Models Evolve: GPT-5 and Claude 4.1 Arrive

OpenAI's GPT-5 is now fully integrated with major IDEs, with improvements in code accuracy, reasoning, and visual support, as well as reduced hallucinations. Integration now includes enterprise controls for more transparent deployment, following recent user requests. Anthropic's Claude 4.1 also joins as an alternative for users, expanding the range of supported models.

The lighter GPT-5 mini model extends Copilot's capabilities to Free plan users, supporting everyday coding needs with responsive AI.

- [OpenAI GPT-5 Now Available to GitHub Copilot Users in Major IDEs](https://github.blog/changelog/2025-08-12-openai-gpt-5-is-now-available-in-public-preview-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [GPT-5 Now Available in GitHub Copilot: Advanced Features and How to Enable]({{ "/2025-08-16-GPT-5-Now-Available-in-GitHub-Copilot-Advanced-Features-and-How-to-Enable.html" | relative_url }})
- [GPT-5 and Claude 4.1 Arrive in GitHub Copilot, TypeScript 5.9 Updates, and Community News]({{ "/2025-08-15-GPT-5-and-Claude-41-Arrive-in-GitHub-Copilot-TypeScript-59-Updates-and-Community-News.html" | relative_url }})
- [GPT-5 Mini Launches in Public Preview for GitHub Copilot Users](https://github.blog/changelog/2025-08-13-gpt-5-mini-now-available-in-github-copilot-in-public-preview)

### Copilot's Agentic and Contextual Intelligence: Model Context Protocol and Agent Modes

MCP is now available for JetBrains, Eclipse, and Xcode, making Copilot a context-aware agent. MCP allows automated interaction with repository data, development tools, and project history across IDEs, similar to how Language Server Protocol standardized language tooling.

Community stories now document entire project automations using MCP, and open sourcing the Copilot MCP server is encouraging more integration and ecosystem growth.

- [Model Context Protocol (MCP) Support for GitHub Copilot Now Available in JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-08-13-model-context-protocol-mcp-support-for-jetbrains-eclipse-and-xcode-is-now-generally-available)
- [Building a Game in 60 Seconds with GPT-5 in GitHub Copilot and MCP Server](https://github.blog/ai-and-ml/generative-ai/gpt-5-in-github-copilot-how-i-built-a-game-in-60-seconds/)
- [Why We Open Sourced Our MCP Server and What It Means for Developers](https://github.blog/open-source/maintainers/why-we-open-sourced-our-mcp-server-and-what-it-means-for-you/)
- [Announcing the NuGet MCP Server Preview: Real-Time NuGet Package Management with AI Integration](https://devblogs.microsoft.com/dotnet/nuget-mcp-server-preview/)

### Agent Autonomy and Workflow Discipline: Beast Mode and Epic Chat Patterns

Beast Mode and the “Do Epic Shit” (DES) chat mode formalize disciplined task automation. These patterns help agents break tasks into smaller steps, check for issues, and verify outcomes before acting—especially within VS Code. Recent improvements focus on reliable automation, auditability, and addressing concerns over AI safety and reproducibility.

- [Do Epic Shit Chat Mode: Beast Mode for GitHub Copilot](https://harrybin.de/posts/do-epic-shit-chat-mode/)
- [VS Code: Let it Cook Ep 12 – Beast Mode Activation and Usage]({{ "/2025-08-14-VS-Code-Let-it-Cook-Ep-12-Beast-Mode-Activation-and-Usage.html" | relative_url }})

### Conversational AI Workflows and Web-Based Copilot Expansion

The web version of Copilot now includes conversational tools for repository Q&A, screenshot annotations, and pull request management—all available via chat. This creates consistent, continuous workflows between browser, cloud, and desktop IDEs, supporting model-switching and real-time health updates for projects.

- [Chatting with Your Repo and Creating PRs Using GitHub Copilot on the Web]({{ "/2025-08-13-Chatting-with-Your-Repo-and-Creating-PRs-Using-GitHub-Copilot-on-the-Web.html" | relative_url }})
- [How to Chat with Your Repo & Create PRs with Copilot on GitHub]({{ "/2025-08-13-How-to-Chat-with-Your-Repo-and-Create-PRs-with-Copilot-on-GitHub.html" | relative_url }})

### AI-Powered Code Modernization and Database Debugging

New tools automate Java and .NET legacy upgrades, helping teams modernize with less risk. Conversational debugging now covers database migrations and schema updates, broadening Copilot’s reach in enterprise scenarios. The GibsonAI CLI is an example of integrated AI for efficient workflows.

- [Modernizing Legacy Java Applications with GitHub Copilot App Modernization Upgrade](https://techcommunity.microsoft.com/t5/microsoft-developer-community/modernizing-legacy-java-project-using-github-copilot-app/ba-p/4440777)
- [Fix Broken Migrations with AI Debugging in VS Code Using GitHub Copilot](https://techcommunity.microsoft.com/t5/educator-developer-blog/fix-broken-migrations-with-ai-powered-debugging-in-vs-code-using/ba-p/4439418)
- [Modernizing and Upgrading Your .NET Apps with Visual Studio and Copilot-Powered AI]({{ "/2025-08-14-Modernizing-and-Upgrading-Your-NET-Apps-with-Visual-Studio-and-Copilot-Powered-AI.html" | relative_url }})
- [Modernizing and Upgrading Your .NET Apps with Visual Studio and Copilot-Powered AI Tools]({{ "/2025-08-14-Modernizing-and-Upgrading-Your-NET-Apps-with-Visual-Studio-and-Copilot-Powered-AI-Tools.html" | relative_url }})

### Integrated Ecosystem: Specialized AI Assistants and Repository-Wide Features

Copilot now directly integrates with frameworks such as Telerik and KendoUI via contextual extensions and MCP servers, giving developers framework-specific AI coding support in VS Code.

Copilot Spaces introduces support for adding entire repositories in one step, streamlining team onboarding and improving management and search for large projects.

- [VS Code Live: Telerik & KendoUI AI Coding Assistants and Contextual AI Integration]({{ "/2025-08-14-VS-Code-Live-Telerik-and-KendoUI-AI-Coding-Assistants-and-Contextual-AI-Integration.html" | relative_url }})
- [Telerik & KendoUI AI Coding Assistants: Contextual AI for VS Code Developers]({{ "/2025-08-14-Telerik-and-KendoUI-AI-Coding-Assistants-Contextual-AI-for-VS-Code-Developers.html" | relative_url }})
- [Copilot Spaces Now Support Adding Entire Repositories](https://github.blog/changelog/2025-08-13-add-repositories-to-spaces)

### Other News

New guides cover Copilot’s usage in API integration and fast code iteration. Stack Overflow’s 2025 Developer Survey shows rising Copilot adoption, but also notes reliability and the continued need for human review. Community conversations and updates—such as to Copilot’s management API and Excel integration—highlight ongoing efforts to improve consistency and feature transparency.

- [Speed Up API Integration with GitHub Copilot](https://pagelsr.github.io/CooknWithCopilot/blog/speed-up-api-integration.html)
- [Stack Overflow Survey Reveals Developer Attitudes Toward AI Tools in 2025](https://devops.com/stack-overflow-survey-shows-ai-adoption-for-devs/?utm_source=rss&utm_medium=rss&utm_campaign=stack-overflow-survey-shows-ai-adoption-for-devs)
- [VS Live! Recap: Visual Studio, GitHub Copilot, and Azure AI Session Highlights](https://devblogs.microsoft.com/visualstudio/from-redmond-to-san-diego-vs-live-highlights-session-examples-and-whats-next/)
- [GitHub Copilot Text Completion for Pull Request Descriptions to Be Deprecated](https://github.blog/changelog/2025-08-15-deprecating-copilot-text-completion-for-pull-request-descriptions)
- [GitHub Copilot User Management API Adds last_authenticated_at Field](https://github.blog/changelog/2025-08-13-added-last_authenticated_at-to-the-copilot-user-management-api)
- [Inconsistent Data Manipulation with Copilot in Excel: Allowed Once, Refused Later](https://techcommunity.microsoft.com/t5/microsoft-365-copilot/copilot-in-excel-performs-data-manipulation-once-and-then/m-p/4444281#M5471)

## AI

AI updates this week focused on GPT-5 ecosystem integration, local open-source LLMs, stronger agent-based workflows in enterprises, and broader document automation—all anchored in the Microsoft stack. Organizational changes at GitHub also reflect ongoing platform consolidation for faster feature delivery.

### GPT-5 Rollout and Deep Ecosystem Integration

GPT-5 is now embedded in Azure, GitHub Copilot, and Visual Studio Code, delivering longer-context suggestions and responsive model variants. Developers get unified SDKs for .NET, Python, and JavaScript, as well as improved guides for retrieval-augmented generation (RAG) and model routing. Case studies emphasize speed and practical use in BI and agentic apps.

- [GPT-5 Integrations for Microsoft Developers: GitHub Copilot, Azure AI, and VS Code](https://devblogs.microsoft.com/blog/gpt-5-for-microsoft-developers)
- [Using GPT-5 with Azure AI Foundry, GitHub Copilot, and Copilot Studio in the Microsoft Ecosystem]({{ "/2025-08-13-Using-GPT-5-with-Azure-AI-Foundry-GitHub-Copilot-and-Copilot-Studio-in-the-Microsoft-Ecosystem.html" | relative_url }})
- [GPT-5 for Developers]({{ "/2025-08-14-GPT-5-for-Developers.html" | relative_url }})
- [Using Model Router with GPT-5 Models in Azure AI Foundry]({{ "/2025-08-14-Using-Model-Router-with-GPT-5-Models-in-Azure-AI-Foundry.html" | relative_url }})
- [Evaluating GPT-5 Models for RAG on Azure AI Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/gpt-5-will-it-rag/ba-p/4442676)

### Open-Source LLMs and Local AI Empowerment

Microsoft has expanded open-source LLM adoption with gpt-oss-20b/120b, along with local GPU setup and full benchmarking tools. Agent Builder GUIs and MCP features reduce barriers to building agentic apps—especially for secure, offline environments.

- [Building Applications Locally with gpt-oss-20b and the AI Toolkit for VS Code](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-application-with-gpt-oss-20b-with-ai-toolkit/ba-p/4441486)
- [Model Mondays S2E9: Models for AI Agents](https://techcommunity.microsoft.com/t5/educator-developer-blog/model-mondays-s2e9-models-for-ai-agents/ba-p/4443162)

### Enterprise Agentic AI Matures

Azure AI Foundry now supports secure, multi-agent enterprise workflows with best practice playbooks and visual management tools, helping organizations roll out incident response and knowledge management bots at scale.

- [Agent Factory: Enterprise Patterns and Best Practices for Agentic AI with Azure AI Foundry](https://azure.microsoft.com/en-us/blog/agent-factory-the-new-era-of-agentic-ai-common-use-cases-and-design-patterns/)
- [Model Mondays S2E9: Models for AI Agents](https://techcommunity.microsoft.com/t5/educator-developer-blog/model-mondays-s2e9-models-for-ai-agents/ba-p/4443162)

### Document Intelligence and Advanced Automation on Azure

Azure AI Foundry has added Mistral Document AI, improving document extraction accuracy and language coverage. REST, CLI, and workflow integrations are now available, supporting security and compliance for document-driven automation.

- [Mistral Document AI Launches on Azure AI Foundry: Seamless Document Intelligence at Scale](https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/deepening-our-partnership-with-mistral-ai-on-azure-ai-foundry/ba-p/4434656)
- [Advancements in Table Structure Recognition with Azure Document Intelligence](https://techcommunity.microsoft.com/t5/azure-ai-foundry-blog/unveiling-the-next-generation-of-table-structure-recognition/ba-p/4443684)

### Next-Generation Conversational and No-Code AI

With Copilot Studio’s evolution from Power Virtual Agents, users get a GPT-based automation hub with agent orchestration, endpoint control, and visual design. This trend extends automation beyond technical teams to business users.

- [Copilot Studio vs. Power Virtual Agents: What’s Changed?](https://dellenny.com/copilot-studio-vs-power-virtual-agents-whats-changed/)
- [No-Code AI: Building Chatbots with Copilot Studio for Non-Developers](https://dellenny.com/no-code-ai-how-non-developers-can-build-smart-chatbots-with-copilot-studio/)
- [Top 5 Use Cases for Copilot Studio in Your Business](https://dellenny.com/top-5-use-cases-for-copilot-studio-in-your-business/)

### Core Platform and Leadership Shifts

The departure of GitHub CEO Thomas Dohmke and tighter Microsoft integration reflect a push for accelerated feature releases, deeper community focus, and a solidified developer-first approach.

- [GitHub CEO Steps Down as Microsoft Integrates GitHub with CoreAI Team](https://devops.com/github-ceo-to-step-down-as-company-is-more-tightly-embraced-by-microsofts-coreai-team/?utm_source=rss&utm_medium=rss&utm_campaign=github-ceo-to-step-down-as-company-is-more-tightly-embraced-by-microsofts-coreai-team)

### Security, Code Quality, and Responsible AI

Recent research from SonarSource highlights the need for careful review, as LLM-generated code quality is rising but vulnerabilities persist. Oversight remains essential.

- [SonarSource Highlights Security Risks and Code Quality Issues in LLM-Generated Code](https://devops.com/sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/?utm_source=rss&utm_medium=rss&utm_campaign=sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code)

### Other News

Additional changes include open-sourcing WSL, launching the Windows AI Foundry, case studies of faster data analytics with Microsoft Fabric, and a nation-wide AI skills program in Australia.

- [MSBuild 2025 Highlights: Open Sourcing WSL and Windows AI Foundry]({{ "/2025-08-14-MSBuild-2025-Highlights-Open-Sourcing-WSL-and-Windows-AI-Foundry.html" | relative_url }})
- [How Adastra Used Microsoft Fabric and Azure OpenAI Service to Transform Heritage Grocers Group’s Data Analytics](https://techcommunity.microsoft.com/t5/partner-news/partner-case-study-adastra/ba-p/4442288)
- [Future Skills Organisation and Microsoft Launch Nationwide AI Skills Accelerator in Australia](https://news.microsoft.com/source/asia/features/fso-microsoft-skills-accelerator-ai/)

## ML

Advancements in ML this week focused on faster large-model optimization, improved cloud deployment, and unified analytics. Microsoft’s contributions are aimed at practical scalability and smoother production rollouts.

### Scalable Optimization for Billion-Parameter Models

Microsoft’s Dion optimizer speeds up training for multi-billion-parameter models, outperforming existing optimizers by ensuring more balanced influence across training inputs. The result is faster, more stable, and cost-effective distributed PyTorch runs.

- [Microsoft Releases Dion: A New Scalable Optimizer for Training AI Models](https://www.microsoft.com/en-us/research/blog/dion-the-distributed-orthonormal-update-revolution-is-here/)

### Cloud Infrastructure and LLM Deployment Patterns Mature

A new guide walks through deploying GPT-OSS-20B to Azure AKS using KAITO and vLLM, helping teams manage cost-efficient, scalable LLM endpoints on managed Kubernetes.

- [Deploying OpenAI’s GPT-OSS-20B on Azure AKS with KAITO and vLLM](https://techcommunity.microsoft.com/t5/ai-machine-learning-blog/deploying-openai-s-first-open-source-model-on-azure-aks-with/ba-p/4444234)

### Unified Data Lake Analytics and Open Format Interoperability

Microsoft Fabric’s OneLake provides on-demand Iceberg virtualization for Delta tables, eliminating format migrations and supporting analytics across multiple engines—improving data workflow efficiency.

- [How Microsoft OneLake Seamlessly Provides Apache Iceberg Support for All Fabric Data](https://blog.fabric.microsoft.com/en-US/blog/how-to-access-your-microsoft-fabric-tables-in-apache-iceberg-format/)

### Tuning and Workflow Evolution in ML Engineering

An in-depth Spark UI guide provides actionable steps for diagnosing and optimizing memory usage, partitioning, and shuffle behavior—useful for maintaining reliable, large-scale ML pipelines.

- [A Deep Dive into Spark UI for Job Optimization](https://techcommunity.microsoft.com/t5/microsoft-mission-critical-blog/a-deep-dive-into-spark-ui-for-job-optimization/ba-p/4442229)

### Other News

Azure Databricks showcases cloud-ML unification. Elsewhere, the 40-year Excel series explores analytics and Python integration, showing how even mature tools continue to evolve for ML applications.

- [Supercharge Data and AI Innovation with Azure Databricks]({{ "/2025-08-12-Supercharge-Data-and-AI-Innovation-with-Azure-Databricks.html" | relative_url }})
- [Excel at 40 Week 1: Days 1–3](https://techcommunity.microsoft.com/t5/excel/excel-at-40-week-1-days-1-3/m-p/4443674#M254078)

## Azure

Azure made progress in security, storage, data management, and automation. The platform’s recognized leadership in container management, new data services, and automation tools highlights Microsoft’s strategy for dependable, integrated cloud workloads.

### Container Management Leadership and Secure Foundations

Microsoft again leads Gartner’s container management quadrant. AKS, ACA, and Azure Arc support unified operations across clouds, GPU-optimized workloads, and reinforced security with Defender, Azure Policy, and RBAC. Azure Linux with OS Guard is now available as an immutable, community-vetted container host for regulated workloads.

- [Microsoft Recognized as a Leader in the 2025 Gartner Magic Quadrant for Container Management](https://azure.microsoft.com/en-us/blog/microsoft-is-a-leader-in-the-2025-gartner-magic-quadrant-for-container-management/)
- [Azure Linux with OS Guard: Enhancing Container Host Security with Code Integrity and Open Source Transparency](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-with-os-guard-immutable-container-host-with-code/ba-p/4437473)

### Expanded Data Services and Enhanced Observability

Oracle Database@Azure is now global, integrated with Azure Monitor and Sentinel. Azure Arc-enabled SQL Server is also available in US Government regions, supporting secure, centralized management across hybrid deployments.

- [Expanding Global Reach and Enhanced Observability with Oracle Database@Azure](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/expanding-global-reach-and-enhancing-observability-with-oracle/ba-p/4443650)
- [Azure Arc-Enabled SQL Server Now Generally Available in US Government Virginia Region](https://techcommunity.microsoft.com/t5/azure-arc-blog/sql-server-enabled-by-azure-arc-is-now-generally-available-in/ba-p/4443077)

### Cloud Storage Innovation and Cost Optimization

Azure Files provisioned v2 for SSD separates performance from capacity and introduces clearer, lower-cost scaling options for DevOps. Fees are simplified and flexible, better supporting hybrid cloud setups.

- [Lower Costs and Boost Flexibility with Azure Files Provisioned v2 for SSD](https://techcommunity.microsoft.com/t5/azure-storage-blog/lower-costs-and-boost-flexibility-with-azure-files-provisioned/ba-p/4443621)
- [Unlocking Flexibility with Azure Files Provisioned V2](https://techcommunity.microsoft.com/t5/itops-talk-blog/unlocking-flexibility-with-azure-files-provisioned-v2/ba-p/4443628)

### Integrated Analytics, Automation, and Developer Productivity

Power BI and Apps now connect to Azure Databricks for interactive write-back, and Power Automate/Copilot Studio agents streamline workflows. The public Terraform MSGraph provider and VSCode extension unify infrastructure management across Azure, Entra, and Microsoft 365.

- [Interactive Write-back from Power BI to Azure Databricks with Power Platform Connector](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/closing-the-loop-interactive-write-back-from-power-bi-to-azure/ba-p/4442999)
- [Announcing Public Preview of the Terraform MSGraph Provider and Microsoft Terraform VSCode Extension](https://techcommunity.microsoft.com/t5/azure-tools-blog/announcing-msgraph-provider-public-preview-and-the-microsoft/ba-p/4443614)

### Simplified, Predictable Data Management with Microsoft Fabric OneLake

OneLake introduces unified proxy transaction costs, giving organizations better control over analytics scaling and budgeting by standardizing pricing across all data access methods.

- [Simplified OneLake Capacity Costs: Updated Proxy Consumption Rates in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/onelake-costs-simplified-lowering-capacity-utilization-when-accessing-onelake/)

### Other News

Azure adds IPv6 App Service support, more data operations, and better troubleshooting features. Improvements also include enhancements for hybrid management, API design, partner integrations, and examples of practical observability.

- [Azure Update - 15th August 2025]({{ "/2025-08-15-Azure-Update-15th-August-2025.html" | relative_url }})
- [Private Pod Subnets in AKS Without Overlay Networking](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/private-pod-subnets-in-aks-without-overlay-networking/ba-p/4442510)
- [Introducing Support for Workspace Identity Authentication in Fabric Connectors](https://blog.fabric.microsoft.com/en-US/blog/announcing-support-for-workspace-identity-authentication-in-new-fabric-connectors-and-for-dataflow-gen2/)
- [Enhancements to Microsoft Fabric Copy Job: Reset Incremental Copy, Auto Table Creation, and JSON Support](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support/)
- and additional updates as listed above

## Coding

This week delivered continued improvements in .NET and C#, support for cross-platform and browser-hosted development, and a set of new productivity and reliability tools.

### Advancements in C# Language and Ecosystem

The latest C# 14 features, including improved type inference and pattern matching, support more efficient and reliable code. The .NET team encourages adopting these tools to keep code agile and maintainable.

- [Everything You Need to Know About the Latest in C#]({{ "/2025-08-14-Everything-You-Need-to-Know-About-the-Latest-in-C.html" | relative_url }})
- [Highlights and Upcoming Features in C#: A Deep Dive into C# 14]({{ "/2025-08-14-Highlights-and-Upcoming-Features-in-C-A-Deep-Dive-into-C-14.html" | relative_url }})

### .NET 10 and Platform Momentum

.NET 10 Preview 7 delivers upgrades in security, diagnostics, cryptography, and multiplatform support. Innovations in ASP.NET Core, Blazor, and MAUI strengthen tools for secure and scalable development.

- [.NET 10 Preview 7 Released: Key Updates for Libraries, ASP.NET Core, Blazor, and MAUI](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-7/)

### Web Development with ASP.NET Core & Blazor and .NET Aspire

New APIs and patterns in Aspire focus on secure authentication, built-in diagnostics, and smooth orchestration, supporting the requirements of modern web services and deployments.

- [The Future of Web Development with ASP.NET Core & Blazor in .NET 10]({{ "/2025-08-14-The-Future-of-Web-Development-with-ASPNET-Core-and-Blazor-in-NET-10.html" | relative_url }})
- [Building Confident Application Systems with .NET Aspire: From Dev to Deployment]({{ "/2025-08-14-Building-Confident-Application-Systems-with-NET-Aspire-From-Dev-to-Deployment.html" | relative_url }})

### Next-Gen Data Mapping in .NET: The Facet Approach

Nick Chapsas’s Facet library replaces reliance on reflection for mapping, using strong types and LINQ for safer, more maintainable, and performant code.

- [Enhancing .NET Code: Using Facet Instead of Traditional Mapping]({{ "/2025-08-13-Enhancing-NET-Code-Using-Facet-Instead-of-Traditional-Mapping.html" | relative_url }})
- [Stop Mapping in .NET: Use Facets Instead]({{ "/2025-08-13-Stop-Mapping-in-NET-Use-Facets-Instead.html" | relative_url }})

### Cross-Platform and Browser-Based .NET: MAUI and WASM

MAUI and Visual Studio improvements make building mobile and desktop apps simpler, while new WASM templates enable browser-hosted .NET with reduced dependency on JavaScript.

- [Building Mobile and Desktop Apps with Visual Studio and .NET MAUI]({{ "/2025-08-14-Building-Mobile-and-Desktop-Apps-with-Visual-Studio-and-NET-MAUI.html" | relative_url }})
- [Running .NET in the Browser Without Blazor Using WASM](https://andrewlock.net/running-dotnet-in-the-browser-without-blazor/)

### Other News

Notable releases include native Python image analysis in Excel, new PowerShell directory analysis, dual-transport MCP servers, Spark resilience fixes, and clearer explanations of VS Code Beast Mode.

- [Analyzing Images with Python in Excel: Now Natively Supported](https://techcommunity.microsoft.com/t5/microsoft-365-insider-blog/analyze-images-with-python-in-excel/ba-p/4440388)
- [Finding Large Directories and Recovering Lost Disk Space with PowerShell](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
- [Building a Dual-Transport MCP Server with .NET: STDIO and HTTP Support](https://techcommunity.microsoft.com/t5/microsoft-developer-community/one-mcp-server-two-transports-stdio-and-http/ba-p/4443915)
- [Spark Resilience Improvements Enhance Reliability and Iteration Experience](https://github.blog/changelog/2025-08-13-spark-resilience-improvements)
- [VS Code Beast Mode Explained: Features and Usage]({{ "/2025-08-14-VS-Code-Beast-Mode-Explained-Features-and-Usage.html" | relative_url }})

## DevOps

Agent automation and security controls got a boost this week, with advances in AI-powered coding agents, policy enforcement, and real-time monitoring leading to more secure and productive DevOps workflows.

### Coding Agent Capabilities Expand

Google’s Gemini CLI GitHub Actions and the open-source Shadow agent automate routine CI/CD, PR, and documentation processes, helping teams reduce manual effort and errors.

- [How Gemini CLI GitHub Actions is Changing Developer Workflows](https://devops.com/how-gemini-cli-github-actions-is-changing-developer-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=how-gemini-cli-github-actions-is-changing-developer-workflows)
- [Shadow: How AI Coding Agents are Transforming DevOps Workflows](https://devops.com/shadow-how-ai-coding-agents-are-transforming-devops-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=shadow-how-ai-coding-agents-are-transforming-devops-workflows)

### GitHub and Azure: Enhanced Security, Productivity, and Governance

GitHub Actions Policy now allows blocking actions and enforcing SHA pinning. Immutable releases and attestation strengthen supply chain security. Guides for artifact management and Copilot troubleshooting improve automation and transparency.

- [GitHub Actions Policy Adds Blocking and SHA Pinning for Enhanced Security](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions)
- [Azure Developer CLI: Dev to Production with Azure DevOps Pipelines](https://devblogs.microsoft.com/devops/azure-developer-cli-from-dev-to-prod-with-azure-devops-pipelines/)

### AI-Powered Market Intelligence and Observability

The new Futurum Signal platform offers AI-driven DevOps market insights, while AppSignal’s OpenTelemetry support enhances application monitoring. These changes signal more integrated and responsive observability options.

- [Futurum Signal: AI-Powered Market Intelligence for DevOps and Platform Engineering](https://devops.com/futurum-signal-ai-powered-market-intelligence-for-devops-and-platform-engineering/?utm_source=rss&utm_medium=rss&utm_campaign=futurum-signal-ai-powered-market-intelligence-for-devops-and-platform-engineering)
- [AppSignal Adds Native OpenTelemetry Support for Enhanced Application Monitoring](https://devops.com/appsignal-adds-opentelemetry-support-to-monitoring-platform/?utm_source=rss&utm_medium=rss&utm_campaign=appsignal-adds-opentelemetry-support-to-monitoring-platform)

### Other News

GitHub has improved pull request reviewer status, file support in attachments, and email filtering. Dependabot now supports vcpkg, and a metered licensing option has been launched. Azure DevOps tooling and incident response guides showcase ways to automate and standardize operational workflows.

- [Clearer Pull Request Reviewer Status and Enhanced Email Filtering in GitHub](https://github.blog/changelog/2025-08-14-clearer-pull-request-reviewer-status-and-enhanced-email-filtering)
- [Expanded File Type Support for GitHub Attachments](https://github.blog/changelog/2025-08-13-expanded-file-type-support-for-attachments-across-issues-pull-requests-and-discussions)
- [Dependabot Adds Version Update Support for vcpkg](https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg)
- [Metered GitHub Enterprise Billing Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-08-14-introducing-metered-github-enterprise-billing-for-visual-studio-subscriptions-with-github-enterprise)
- and additional updates as listed above

## Security

Enterprise security this week focused on quick responses to critical vulnerabilities and expanded AI-powered protection for cloud, hybrid, and supply chain scenarios.

### Critical Infrastructure Vulnerabilities Drive Immediate Action

Addressing CVE-2025-53786 (Exchange) and CVE-2025-53770 (SharePoint) requires prompt patching and updates to identity and firewall configurations. Recent BitLocker bypass findings reinforce the need for updated defense and configuration reviews.

- [Mitigating CVE-2025-53786: Hybrid Exchange Server Privilege Escalation with MDVM](https://techcommunity.microsoft.com/t5/microsoft-defender-vulnerability/mdvm-guidance-for-cve-2025-53786-exchange-hybrid-privilege/ba-p/4442337)
- [Mitigating SharePoint CVE-2025-53770 Using Azure Web Application Firewall](https://techcommunity.microsoft.com/t5/azure-network-security-blog/protect-against-sharepoint-cve-2025-53770-with-azure-web/ba-p/4442050)
- [BitUnlocker: Leveraging Windows Recovery to Extract BitLocker Secrets](https://techcommunity.microsoft.com/t5/microsoft-security-community/bitunlocker-leveraging-windows-recovery-to-extract-bitlocker/ba-p/4442806)

### Security Copilot and AI-Powered Defenses Broaden Their Reach

Security Copilot now integrates deeply with Intune and Entra ID, adding policy agents and advanced phishing detection. These enhancements also enable role segmentation, workspace isolation, and compliance feature support. Dow’s operational case study demonstrates AI-driven incident response in practice.

- [What’s New in Microsoft Security Copilot: AI-Powered Security Innovations for IT and Security Teams](https://techcommunity.microsoft.com/t5/microsoft-security-copilot-blog/what-s-new-in-microsoft-security-copilot/ba-p/4442220)
- [How Dow Uses Microsoft Security Copilot and AI to Transform Cybersecurity Operations](https://www.microsoft.com/en-us/security/blog/2025/08/12/dows-125-year-legacy-innovating-with-ai-to-secure-a-long-future/)

### Code, Secrets, and the Modern Software Supply Chain

GitHub’s Secure Open Source Fund now supports improvements and audits for 71 projects. Secret scanning expands to 12 new token types, with enhanced push protection. SonarSource and Azure DevOps continue to automate code and secret validation.

- [Securing the Open Source Supply Chain: Impact of the GitHub Secure Open Source Fund](https://github.blog/open-source/maintainers/securing-the-supply-chain-at-scale-starting-with-71-important-open-source-projects/)
- [SonarSource Research Highlights Security Risks in LLM-Generated Code](https://devops.com/sonar-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/?utm_source=rss&utm_medium=rss&utm_campaign=sonar-surfaces-multiple-caveats-when-relying-on-llms-to-write-code)
- [Secret Scanning Expands Support: 12 New Token Validators Added to GitHub](https://github.blog/changelog/2025-08-12-secret-scanning-adds-12-validators-including-cockroach-labs-polar-and-yandex)
- [GitHub MCP Server Enhances Secret Scanning and Push Protection for Public Repositories](https://github.blog/changelog/2025-08-13-github-mcp-server-secret-scanning-push-protection-and-more)
- [Secret Validity Checks Launch in GitHub Advanced Security for Azure DevOps](https://devblogs.microsoft.com/devops/hunting-living-secrets-secret-validity-checks-arrive-in-github-advanced-security-for-azure-devops/)
- [Azure DevOps Improves OAuth Client Secret Security: Secrets Now Shown Only Once](https://devblogs.microsoft.com/devops/azure-devops-oauth-client-secrets-now-shown-only-once)

### Enterprise Identity, Cloud, and Data Security Matures

Platform SSO for macOS, Microsoft Purview DSPM expansion, and real-time CAE access controls extend secure identity and DLP. Guidance for cloud forensics and SaaS integration bolsters enterprise readiness.

- [General Availability: Platform SSO for macOS with Microsoft Entra ID](https://techcommunity.microsoft.com/t5/microsoft-entra-blog/now-generally-available-platform-sso-for-macos-with-microsoft/ba-p/4437424)
- [Govern AI Securely with Microsoft Purview: Compliance Across Copilot, ChatGPT, and Beyond](https://techcommunity.microsoft.com/t5/microsoft-security-community/microsoft-purview-the-ultimate-ai-data-security-solution/ba-p/4441324)
- [Continuous Access Evaluation (CAE) Brings Real-Time Security to Azure DevOps](https://devblogs.microsoft.com/devops/real-time-security-with-continuous-access-evaluation-cae-comes-to-azure-devops/)
- [Cloud Forensics: Implementing Security Baselines for Forensic Readiness in Microsoft Azure](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/cloud-forensics-prepare-for-the-worst-implement-security/ba-p/4440310)
- [Secure Integration of Microsoft 365 with Slack, Trello, and Google Services](https://dellenny.com/how-to-integrate-m365-with-third-party-saas-tools-slack-trello-google-services-without-breaking-security/)

### Proactive and Automated Exposure Management

Security Exposure Management and the Defender Experts Ninja Hub offer practical resources for attack path visualization, XDR, and threat hunting.

- [Microsoft Security Exposure Management Ninja Training](https://techcommunity.microsoft.com/t5/microsoft-security-exposure/microsoft-security-exposure-management-ninja-training/ba-p/4444285)
- [Microsoft Defender Experts Ninja Hub: Resources for XDR and Threat Hunting](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/welcome-to-the-microsoft-defender-experts-ninja-hub/ba-p/4442210)

### Expanding and Securing Data Governance

New eDiscovery features, audit guides, and customer-managed keys for Fabric help teams track Copilot activity, meet compliance, and maintain encryption control.

- [What’s New in Microsoft Purview eDiscovery](https://techcommunity.microsoft.com/t5/microsoft-security-community/what-s-new-in-microsoft-purview-ediscovery/ba-p/4441676)
- [Investigating Microsoft 365 Copilot Activity with Sentinel, Defender XDR, and Purview DSPM for AI Security](https://techcommunity.microsoft.com/t5/microsoft-security-community/investigating-m365-copilot-activity-with-sentinel-defender-xdr/ba-p/4442641)
- [Customer-Managed Keys for Microsoft Fabric Workspaces Now in Public Preview](https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-for-fabric-workspaces-available-in-all-public-regions-now-preview/)

### Security Updates and Regulatory Readiness

Recent updates for SQL Server and Exchange, along with new tools for regulatory compliance, help organizations stay secure and in line with requirements.

- [Security Update Available for SQL Server 2022 RTM GDR](https://techcommunity.microsoft.com/t5/sql-server-blog/security-update-for-sql-server-2022-rtm-gdr/ba-p/4441687)
- [August 2025 Exchange Server Security Updates Released](https://techcommunity.microsoft.com/t5/exchange-team-blog/released-august-2025-exchange-server-security-updates/ba-p/4441596)
- [Eclipse Foundation Publishes Toolkit to Simplify CRA Compliance](https://devops.com/eclipse-foundation-publishes-toolkit-to-simplify-cra-compliance/?utm_source=rss&utm_medium=rss&utm_campaign=eclipse-foundation-publishes-toolkit-to-simplify-cra-compliance)

### Other News

Included are guides on Teams encryption, survey data on breaches, new secret protection tools, developments in government cloud security, and examples of how AI is leveraged for credential defense.

- [Encryption in Microsoft Teams: How Microsoft Secures Collaboration and Communication](https://techcommunity.microsoft.com/t5/microsoft-teams-blog/encryption-in-microsoft-teams-june-2025/ba-p/4442913)
- [Most Organizations Face Breaches Caused by Vulnerable Code, Survey Finds](https://devops.com/survey-traces-large-amount-of-breaches-back-to-vulnerable-code/?utm_source=rss&utm_medium=rss&utm_campaign=survey-traces-large-amount-of-breaches-back-to-vulnerable-code)
- [What is GitHub Secret Protection?]({{ "/2025-08-17-What-is-GitHub-Secret-Protection-GitHub-Explained.html" | relative_url }})
- [Minimus Adds VEX Support and Microsoft SSO Integration to Hardened Images Service](https://devops.com/minimus-adds-vex-support-to-managed-hardened-images-service/?utm_source=rss&utm_medium=rss&utm_campaign=minimus-adds-vex-support-to-managed-hardened-images-service)
- and additional updates as listed above
