---
layout: "post"
title: "Updates in Agent-Based AI, Secure Automation, and Developer Tools Modernization"
description: "This week's roundup highlights new advances in agent-based automation and modernization. GitHub Copilot expands its capabilities across IDEs, enterprise controls, and DevOps pipelines. Key updates and integrations affect every layer of the tech stack, featuring improvements in AI infrastructure, machine learning operations, new security protocols, and detailed governance. Developers also benefit from improvements to coding tools, data workflows, and cloud security, reflecting ongoing technical progress and updates on best practices."
author: "Tech Hub Team"
excerpt_separator: <!--excerpt_end-->
viewing_mode: "internal"
date: 2025-11-24 09:00:00 +00:00
permalink: "/roundups/2025-11-24-Weekly-AI-and-Tech-News-Roundup.html"
categories: ["AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security"]
tags: [".NET", "AI", "AI Agents", "Azure", "Cloud Infrastructure", "Coding", "Data Governance", "DevOps", "GitHub Copilot", "IDE Integration", "Machine Learning", "ML", "Model Deployment", "Post Quantum Cryptography", "Roundups", "Security", "VS", "VS Code", "Workflow Automation"]
tags_normalized: ["dotnet", "ai", "ai agents", "azure", "cloud infrastructure", "coding", "data governance", "devops", "github copilot", "ide integration", "machine learning", "ml", "model deployment", "post quantum cryptography", "roundups", "security", "vs", "vs code", "workflow automation"]
---

Welcome to this week's technology news roundup. This edition focuses on agent-based automation, security improvements, and updates for developers. Recent releases include new agent-driven coding features, more IDE integrations, and expanded organizational controls within GitHub Copilot, further supporting productivity, code quality, and migration automation. On the AI and infrastructure front, Azure and Microsoft Fabric now provide updated compute options, streamlined data engineering tools, and more practical MLops, supporting reliable, scalable, and productive AI solutions.

In parallel, advances in security and governance come from Microsoft and its partners, who introduced improved agent identity controls, support for post-quantum cryptography, unified DevSecOps tools, and detailed data protections. Updates to policy automation, compliance, and endpoint security are shaping continuous and resilient software supply chains. Developers, architects, and security professionals can all find practical takeaways in this week's update, which covers how automation, intelligence, and robust design are coming together across the industry.

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Agentic Automation and IDE/Cloud Integrations](#agentic-automation-and-idecloud-integrations)
  - [Intelligent Code Suggestion, Planning, and Test Automation](#intelligent-code-suggestion-planning-and-test-automation)
  - [Enterprise Controls, Model Flexibility, and Security](#enterprise-controls-model-flexibility-and-security)
  - [Customization and Agent Management Across Development Teams](#customization-and-agent-management-across-development-teams)
  - [Modernization, Migration, and DevOps Integration](#modernization-migration-and-devops-integration)
  - [Productivity, Code Quality, and Workflow Best Practices](#productivity-code-quality-and-workflow-best-practices)
  - [AI Model Choice, Embedding-Guided Tooling, and Collaborative Development](#ai-model-choice-embedding-guided-tooling-and-collaborative-development)
  - [AI-Enhanced Code Quality, Review Workflows, and Developer Collaboration](#ai-enhanced-code-quality-review-workflows-and-developer-collaboration)
  - [Copilot for Data, Natural Language Automation, and Operations](#copilot-for-data-natural-language-automation-and-operations)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [ML](#ml)
  - [Azure AI Compute and Infrastructure](#azure-ai-compute-and-infrastructure)
  - [Model Development, Deployment, and Optimization Tools](#model-development-deployment-and-optimization-tools)
  - [Microsoft Fabric: Enhanced AI and Data Engineering Capabilities](#microsoft-fabric-enhanced-ai-and-data-engineering-capabilities)
  - [Data Quality, Analytics, and Platform Integration](#data-quality-analytics-and-platform-integration)
- [Coding](#coding)
  - [Advancements in .NET Languages: C# 14 and F# 10](#advancements-in-net-languages-c-14-and-f-10)
  - [Visual Studio Family: Modernization, Productivity, and Secure Extension Management](#visual-studio-family-modernization-productivity-and-secure-extension-management)
  - [Windows Settings and File Explorer: Developer-Centric Enhancements](#windows-settings-and-file-explorer-developer-centric-enhancements)
  - [Git 2.52: Version Control, Performance, and Migration](#git-252-version-control-performance-and-migration)
  - [AI-Enhanced, Cross-Platform Development with Uno Platform](#ai-enhanced-cross-platform-development-with-uno-platform)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [GitHub and GitHub Actions: Migrations, Workflow Enhancements, and Platform Governance](#github-and-github-actions-migrations-workflow-enhancements-and-platform-governance)
  - [CI/CD Automation, Migration, and Unified Build Approaches](#cicd-automation-migration-and-unified-build-approaches)
  - [Visual Studio Code: Private Marketplace and IT Governance](#visual-studio-code-private-marketplace-and-it-governance)
  - [Observability, Monitoring, and Security in DevOps Pipelines](#observability-monitoring-and-security-in-devops-pipelines)
  - [DevOps for Data, GenAI, and MLOps](#devops-for-data-genai-and-mlops)
  - [Azure DevOps Integrations and Outage Readiness](#azure-devops-integrations-and-outage-readiness)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Azure Platform Security: New Foundations and Granular Controls](#azure-platform-security-new-foundations-and-granular-controls)
  - [Building Security for AI-Driven Workloads and Agents](#building-security-for-ai-driven-workloads-and-agents)
  - [Microsoft Defender for Cloud and End-to-End Application Security](#microsoft-defender-for-cloud-and-end-to-end-application-security)
  - [Comprehensive Governance for Data, Secrets, and Identity](#comprehensive-governance-for-data-secrets-and-identity)
  - [Post-Quantum Cryptography Advances and Secure Coding](#post-quantum-cryptography-advances-and-secure-coding)
  - [Microsoft Sentinel: Agentic SIEM, Automation, and AI](#microsoft-sentinel-agentic-siem-automation-and-ai)
  - [Policy, Compliance, and Governance Workflows](#policy-compliance-and-governance-workflows)
  - [Other Security News](#other-security-news)

## GitHub Copilot

Building on the previous week’s updates in agent design, model selection, workflow automation, and IDE compatibility, GitHub Copilot has progressed with new features and integrations for IDEs, cloud platforms, enterprise controls, and agent workflows. These enhancements add support for more developer environments and management tools, bringing practical gains in productivity, security, and code quality as Copilot’s features become more policy-driven and context aware.

### Agentic Automation and IDE/Cloud Integrations

Copilot’s agent capabilities have grown, with recent improvements in Mission Control and updated experiences in Visual Studio and VS Code now joined by Ignite’s announcements. App modernization powered by Copilot agents is now available for JetBrains, Eclipse, and Xcode, adding to existing support in Visual Studio and VS Code. For developers using Visual Studio, .NET, and Azure, Copilot now provides automation for migration and containerization tasks, expanding from basic workflow help to actual application modernization.

The Copilot CLI now supports the latest models (like OpenAI GPT-5.1 and Gemini 3.5 Pro), building on last week’s features in code search and context. Eclipse users can now use Copilot’s coding agents, a continuation of the rollout seen for VS Code and JetBrains. Migration assessment is now connected to Copilot’s agent features, reinforcing prior improvements in policy enforcement and organizational controls.

- [AI Agents Accelerate App Modernization with GitHub Copilot and Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/ai-agents-are-rewriting-the-app-modernization-playbook/ba-p/4470162)
- [GitHub Copilot CLI Introduces New AI Models, Enhanced Code Search, and Improved Image Support](https://github.blog/changelog/2025-11-18-github-copilot-cli-new-models-enhanced-code-search-and-better-image-support)
- [GitHub Copilot Coding Agent for Eclipse Now in Public Preview](https://github.blog/changelog/2025-11-18-github-copilot-coding-agent-for-eclipse-now-in-public-preview)
- [GitHub Copilot Isolated Subagents Now in Public Preview for JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-11-18-isolated-subagents-for-jetbrains-eclipse-and-xcode-now-in-public-preview)
- [How to Assign and Manage Copilot Agent Tasks from Anywhere]({{ "/2025-11-20-How-to-Assign-and-Manage-Copilot-Agent-Tasks-from-Anywhere.html" | relative_url }})

### Intelligent Code Suggestion, Planning, and Test Automation

Copilot has enhanced its predictive editing and planning with new features that build on last week’s inline chat and session management in VS Code. Next Edit Suggestions (NES) are now in public preview for Xcode and Eclipse, expanding coverage beyond VS Code and Visual Studio and moving toward similar functionality across all environments. NES adapts suggestions to align better with user intent, moving beyond basic code completion.

Test automation with Copilot is now available for .NET in Visual Studio 2026 Insiders, marking progress from purely manual reviews to integrated test generation and automation. Agent-based planning features are now available in JetBrains, VS Code, Xcode, and Eclipse, following the recent addition of organizational instruction and review tools.

- [Enhancing GitHub Copilot’s Next Edit Suggestions with Custom Model Training](https://github.blog/ai-and-ml/github-copilot/evolving-github-copilots-next-edit-suggestions-through-custom-model-training/)
- [GitHub Copilot Next Edit Suggestions (NES) Public Preview for Xcode and Eclipse](https://github.blog/changelog/2025-11-18-github-copilot-next-edit-suggestions-nes-now-in-public-preview-for-xcode-and-eclipse)
- [Supercharge Your Test Coverage with GitHub Copilot Testing for .NET](https://devblogs.microsoft.com/dotnet/github-copilot-testing-for-dotnet/)
- [Plan Mode in GitHub Copilot Now Available in Public Preview for JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-11-18-plan-mode-in-github-copilot-now-in-public-preview-in-jetbrains-eclipse-and-xcode)
- [Using the Plan Agent in VS Code for Step-by-Step Task Planning]({{ "/2025-11-19-Using-the-Plan-Agent-in-VS-Code-for-Step-by-Step-Task-Planning.html" | relative_url }})

### Enterprise Controls, Model Flexibility, and Security

Continuing from new administrative options last week, this update introduces BYOK (Bring Your Own Key) and broader MCP allowlisting. These features allow enterprises to use their own LLM API keys and define which backend servers developers may connect to, increasing Copilot’s suitability for regulated environments.

Enhanced usage metrics permissions support better tracking of Copilot use and investment. Updated security guides now cover SIEM integration and advanced anomaly detection to provide clear ways for organizations to baseline and review Copilot activity. Authentication improvements across JetBrains, Eclipse, and Xcode further streamline onboarding in managed setups.

- [Internal MCP Registry and Allowlist Controls for Copilot in VS Code and Visual Studio](https://github.blog/changelog/2025-11-18-internal-mcp-registry-and-allowlist-controls-for-vs-code-stable-in-public-preview)
- [Enterprise BYOK for GitHub Copilot Now in Public Preview](https://github.blog/changelog/2025-11-20-enterprise-bring-your-own-key-byok-for-github-copilot-is-now-in-public-preview)
- [Fine-Grain Permissions for GitHub Copilot Usage Metrics Released](https://github.blog/changelog/2025-11-17-fine-grain-permissions-for-copilot-usage-metrics-now-available)
- [Setting Up Security Alerts for Unusual GitHub Copilot Activity](https://dellenny.com/setting-up-alerts-for-unusual-github-copilot-activity/)
- [Enhanced MCP OAuth Support for GitHub Copilot Plugins in JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-11-18-enhanced-mcp-oauth-support-for-github-copilot-in-jetbrains-eclipse-and-xcode)

### Customization and Agent Management Across Development Teams

Applying what was learned about custom instructions and team workflows, Copilot’s agent customization and isolated subagent features are now public for JetBrains, Eclipse, and Xcode, enabling clearer workflow division. The agents.md guide has added input from 2,500 repositories, building on prior documentation and guidance for multi-agent setups.

New video tutorials cover assigning and monitoring agent tasks across multiple platforms, addressing practical workflow management as highlighted previously. These additions make Copilot’s automation easier to adopt for teams with diverse technology stacks.

- [Custom Agents in GitHub Copilot for JetBrains, Eclipse, and Xcode Now in Public Preview](https://github.blog/changelog/2025-11-18-custom-agents-available-in-github-copilot-for-jetbrains-eclipse-and-xcode-now-in-public-preview)
- [GitHub Copilot Isolated Subagents Now in Public Preview for JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-11-18-isolated-subagents-for-jetbrains-eclipse-and-xcode-now-in-public-preview)
- [How to Write a Great agents.md: Lessons from 2,500 GitHub Repositories](https://github.blog/ai-and-ml/github-copilot/how-to-write-a-great-agents-md-lessons-from-over-2500-repositories/)
- [How to Assign and Manage Copilot Agent Tasks from Anywhere]({{ "/2025-11-20-How-to-Assign-and-Manage-Copilot-Agent-Tasks-from-Anywhere.html" | relative_url }})

### Modernization, Migration, and DevOps Integration

Building on prior coverage of Visual Studio 2026 and .NET Aspire, Copilot’s Agent Mode now automates many aspects of .NET app migration and legacy modernization. The shift from free tools to subscription models has prompted discussion of costs and continuity for developers and organizations.

Integration between Azure DevOps and Copilot strengthens automation for project management and security across coding workflows. Agent-driven DevOps guides and dashboards help reinforce the practical approach detailed previously.

- [Modernizing .NET Applications with GitHub Copilot Agent Mode: A Step-by-Step Guide](https://devblogs.microsoft.com/dotnet/modernizing-dotnet-with-github-copilot-agent-mode/)
- [Migrating .NET Framework Apps with GitHub Copilot in Visual Studio: Developer Feedback and Licensing Changes](https://devclass.com/2025/11/20/copilot-net-modernization-tool-a-huge-downgrade-devs-say-and-no-longer-free/)
- [Azure DevOps and GitHub Repositories: Unlocking Agentic AI for Developer Teams](https://devblogs.microsoft.com/devops/azure-devops-and-github-repositories-next-steps-in-the-path-to-agentic-ai/)
- [AI-Powered Hybrid DevOps with GitHub Copilot and Azure DevOps]({{ "/2025-11-20-AI-Powered-Hybrid-DevOps-with-GitHub-Copilot-and-Azure-DevOps.html" | relative_url }})
- [Modernize Your Apps in Days with AI Agents in GitHub Copilot]({{ "/2025-11-21-Modernize-Your-Apps-in-Days-with-AI-Agents-in-GitHub-Copilot.html" | relative_url }})
- [From Legacy to Modern .NET on Azure with Visual Studio 2026, Azure App Service, and GitHub Copilot]({{ "/2025-11-21-From-Legacy-to-Modern-NET-on-Azure-with-Visual-Studio-2026-Azure-App-Service-and-GitHub-Copilot.html" | relative_url }})

### Productivity, Code Quality, and Workflow Best Practices

Updates to Copilot dashboards, analytics, and guides on prompt engineering build on earlier productivity themes. Keyboard shortcut and command reference tutorials help streamline Copilot Chat in practical contexts. Tips and best practices for integrating Copilot into test-driven development, code reviews, and static analysis reinforce proven approaches for reliable automation.

Security and workflow recommendations point to Copilot’s role alongside linters and other guards, a repeat point from recent discussions on building automation that still requires human oversight for safety.

- [Top GitHub Copilot Shortcuts and Productivity Tips for VS Code](https://dellenny.com/turbocharge-your-coding-top-github-copilot-shortcuts-and-productivity-tips-for-vs-code/)
- [Your Guide to Debugging and Reviewing Copilot-Generated Code](https://dellenny.com/your-guide-to-debugging-and-reviewing-copilot-generated-code/)
- [Your Guide to Debugging and Reviewing Copilot-Generated Code](https://techcommunity.microsoft.com/t5/tools/your-guide-to-debugging-and-reviewing-copilot-generated-code/m-p/4472116#M182)
- [Best Practices for Coding with GitHub Copilot in .NET]({{ "/2025-11-17-Best-Practices-for-Coding-with-GitHub-Copilot-in-NET.html" | relative_url }})
- [Using the Cognitive Verifier Pattern with GitHub Copilot](https://www.cooknwithcopilot.com/blog/context-engineering-recipes-the-cognitive-verifier-pattern.html)

### AI Model Choice, Embedding-Guided Tooling, and Collaborative Development

Auto model selection and controls, previewed last week, are now available in JetBrains, Xcode, and Eclipse, supporting smarter project-specific automation. Gemini 3 Pro is now in public preview, joining GPT-5.1 and Codex, broadening choice for developers.

Updates to embedding-guided tooling and routing in VS Code further extend Copilot’s ability to select the right tool for context. Copilot Spaces now aggregates context from multiple files and repositories, improving overall automation.

- [Auto Model Selection for GitHub Copilot in JetBrains, Xcode, and Eclipse](https://github.blog/changelog/2025-11-18-auto-model-selection-for-copilot-in-jetbrains-ides-xcode-and-eclipse-in-public-preview)
- [Gemini 3 Pro Model Now Available in GitHub Copilot Public Preview](https://github.blog/changelog/2025-11-18-gemini-3-pro-is-in-public-preview-for-github-copilot)
- [How GitHub Copilot Uses Embedding-Guided Tool Routing in VS Code](https://github.blog/ai-and-ml/github-copilot/how-were-making-github-copilot-smarter-with-fewer-tools/)
- [How Copilot Spaces gives your AI the right project context]({{ "/2025-11-18-How-Copilot-Spaces-gives-your-AI-the-right-project-context.html" | relative_url }})

### AI-Enhanced Code Quality, Review Workflows, and Developer Collaboration

Linter support in Copilot’s code review toolkit continues earlier efforts around CodeQL, agent review features, and better control for team leads. Language-aware analysis builds on efforts for robust organization-level quality review.

Recent sessions at GitHub Universe and Ignite add case studies and guidance focused on developer productivity and automation across the SDLC. Coverage on MCP-backed policy and context management links to previous enterprise-level updates.

- [Linter Integration Arrives in Copilot Code Review Public Preview](https://github.blog/changelog/2025-11-20-linter-integration-with-copilot-code-review-now-in-public-preview)
- [Scaling Code Quality in the Age of AI]({{ "/2025-11-19-Scaling-Code-Quality-in-the-Age-of-AI.html" | relative_url }})
- [Redefining the SDLC with GitHub Copilot and Context-Driven AI]({{ "/2025-11-19-Redefining-the-SDLC-with-GitHub-Copilot-and-Context-Driven-AI.html" | relative_url }})
- [Reimagining Software Development with GitHub Copilot and AI Agents]({{ "/2025-11-21-Reimagining-Software-Development-with-GitHub-Copilot-and-AI-Agents.html" | relative_url }})

### Copilot for Data, Natural Language Automation, and Operations

Building on recent automation coverage, Copilot now brings automation to data work. Copilot and Query Editor for SQL Database on Microsoft Fabric move to general availability, expanding Copilot's reach into database tasks. Natural language pipeline authoring in Fabric Data Factory continues the drive for context-powered automation from app development into data engineering.

Integration with Azure DevOps, including PagerDuty and Datadog, keeps the focus on practical end-to-end DevOps automation.

- [Copilot and Query Editor Now Generally Available in SQL Database on Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/copilot-and-query-editor-in-sql-database-in-fabric-ga-update/)
- [Natural Language to Generate and Explain Pipeline Expressions with Copilot (Preview)](https://blog.fabric.microsoft.com/en-US/blog/preview-natural-language-to-generate-and-explain-pipeline-expressions-with-copilot/)
- [Copilot-Assisted Real-Time Data Exploration in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/copilot-assisted-real-time-data-exploration-preview/)
- [Ship Faster with Azure and GitHub Copilot: End-to-End DevOps with AI Agents]({{ "/2025-11-21-Ship-Faster-with-Azure-and-GitHub-Copilot-End-to-End-DevOps-with-AI-Agents.html" | relative_url }})

### Other GitHub Copilot News

Further updates to developer tools follow last week’s introduction of the Raptor Mini Model and improved session management. The Download video transitions tools like Gemini 3 Pro to general release and presents demonstrations of Git 2.52 jetpack and Agent 365, highlighting Copilot’s growing ecosystem.

Additional resources support Copilot adoption, code review, and debugging, ensuring developers remain current as Copilot evolves. These tools help teams maintain quality and productivity as they bring automation and AI deeper into daily development.

- [The Download: Git 2.52, Gemini 3, GitHub Copilot Updates & Agent 365]({{ "/2025-11-21-The-Download-Git-252-Gemini-3-GitHub-Copilot-Updates-and-Agent-365.html" | relative_url }})

Additional resources have been shared to help teams adjust to Copilot’s expanding features and agent-based automation, from debugging guides to new feedback channels. These tools will be key for organizations standardizing AI-powered workflows.

## ML

The machine learning focus this week is on more scalable compute, enhanced platform features, and better operational tools from cloud and enterprise providers. Azure rolled out ND GB300 v6 VMs, while Microsoft Fabric announced further improvements in its AI and data engineering offerings. Aspects like data quality, model deployment, and performance optimization remain front and center, reflecting an ongoing move to scalable and high-throughput ML infrastructure.

### Azure AI Compute and Infrastructure

Azure has released the ND GB300 v6 VMs, which include NVIDIA GB300 NVL72 GPUs, Grace CPUs, and fast InfiniBand networking built for large-scale training and inference. These VMs integrate with Azure CycleCloud, Batch, and AKS, building on existing solutions for orchestrating AI workloads.

The AMLFS 20 (Azure Managed Lustre) SKU delivers bigger namespaces and higher metadata throughput for high-performance workloads, meeting the needs of fast, scalable data access in ML production.

- [Azure ND GB300 v6 Virtual Machines: General Availability and Next-Gen AI Infrastructure](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-nd-gb300-v6-now-generally-available-hyper-optimized-for/ba-p/4469475)
- [Announcing Public Preview of AMLFS 20: New Azure Managed Lustre SKU for AI and HPC](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/announcing-the-public-preview-of-amlfs-20-azure-managed-lustre/ba-p/4470665)

### Model Development, Deployment, and Optimization Tools

Microsoft Foundry and Azure ML are focusing on seamless model development and production deployment, helping teams standardize their ML pipelines and cover scenarios like reinforcement learning and intelligent agent deployment. Sessions and tutorials explore metric evaluation, reliability testing, and parameter tuning for Retrieval-Augmented Generation (RAG) agents.

Windows ML updates show ongoing work to enable local AI inference using ONNX Runtime, supporting privacy and low-latency requirements, following previous guidance for regulated environments.

- [Training and Deploying Reasoning Models with Microsoft Foundry and Azure ML]({{ "/2025-11-20-Training-and-Deploying-Reasoning-Models-with-Microsoft-Foundry-and-Azure-ML.html" | relative_url }})
- [Debugging and Optimizing RAG Agents in Microsoft Foundry](https://devblogs.microsoft.com/foundry/how-to-debug-and-optimize-rag-agents-in-azure-ai-foundry/)
- [Deploying Local AI Models in Enterprise with Windows ML]({{ "/2025-11-21-Deploying-Local-AI-Models-in-Enterprise-with-Windows-ML.html" | relative_url }})

### Microsoft Fabric: Enhanced AI and Data Engineering Capabilities

Microsoft Fabric’s latest updates provide more flexible AI integration, with features like ai.embed() (now GA) and support for models from GPT-5, Claude, LLaMA, Azure OpenAI, and AI Foundry. These tools bring AI-powered workflows into common data engineering platforms, facilitating new uses for PySpark, pandas, and hybrid agent workflows.

Updates for event streaming, data clustering, and endpoint management make it easier to unify analytics workloads and speed up real-time processing with KQL/SQL support. dbt Jobs integration expands on recent improvements to data transformation and validation in Fabric.

- [Microsoft Fabric AI Functions: Enhanced Features Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/29826/)
- [Eventhouse Endpoint Arrives for Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/introducing-eventhouse-endpoint-for-fabric-data-warehouse-real-time-analytics-unified-architecture/)
- [Announcing Data Clustering in Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/announcing-data-clustering-in-fabric-data-warehouse-preview/)
- [Integrating dbt Jobs with Microsoft Fabric for Scalable SQL Transformations (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dbt-job-in-microsoft-fabric-ship-trustworthy-sql-models-faster-preview/)

### Data Quality, Analytics, and Platform Integration

Following up on historical dataset modernization, this week’s content provides more strategies for proactive data quality management, supporting cleaner ML pipelines for any cloud setup.

Further coverage shows Azure Databricks and SAP Business Data Cloud links for modern analytics, with stories about Delta Sharing, agent-based automation, and Power BI integrations that help connect disparate data sources and expand AI development.

- [Continuous Data Quality Optimization for Better AI Output](https://dellenny.com/continuous-data-quality-optimization-for-ai-the-essential-guide/)
- [Modern Data Analytics and AI with SAP Databricks on Azure]({{ "/2025-11-19-Modern-Data-Analytics-and-AI-with-SAP-Databricks-on-Azure.html" | relative_url }})

## Coding

Coding news this week includes improvements in programming languages, development tools, and platform interoperability. New releases for C#, F#, and .NET focus on modern features and improved expressiveness. Updates in Visual Studio, VS Code, Git, and Windows target code management, collaboration, and administration. Accessibility, accessible design, and educational content continue to help developers at all levels.

### Advancements in .NET Languages: C# 14 and F# 10

After last week’s release of .NET 10, C# 14 and F# 10 introduce updated language features. C# 14 adds extension members, a `field` keyword, unbound generics in `nameof`, and more expressive lambda syntax, supporting safer and more consistent code. Migration resources offer help for adapting to these changes.

F# 10 introduces better warning suppression, enhanced property accessor features, and improved computation expressions and scripting performance. These changes support current tooling and offer hints at the direction for .NET 11’s continued compiler improvements.

- [Introducing C# 14: New Language Features and .NET 10 Integration](https://devblogs.microsoft.com/dotnet/introducing-csharp-14/)
- [What's New in C# 14 and .NET 10]({{ "/2025-11-18-Whats-New-in-C-14-and-NET-10.html" | relative_url }})
- [Introducing F# 10: Language Features, Performance, and Tooling in .NET 10](https://devblogs.microsoft.com/dotnet/introducing-fsharp-10/)

### Visual Studio Family: Modernization, Productivity, and Secure Extension Management

Visual Studio 2026 continues its focus on smooth migration, automation, and productivity. Automated dependency checks, project retargeting, and Copilot support streamline the process of updating legacy apps. Stable update, rollback, and repair options support reliability during upgrades.

Visual Studio Code 1.106 debuts the Private Marketplace, giving organizations better control over which extensions are used while reinforcing secure extension management. Accessibility improvements continue to support every developer’s workflow.

- [Effortless Upgrades and Coding Productivity in Visual Studio 2026](https://devblogs.microsoft.com/visualstudio/spend-less-time-upgrading-more-time-coding-in-visual-studio-2026/)
- [Introducing the Visual Studio Code Private Marketplace: Secure Extension Management for Teams](https://code.visualstudio.com/blogs/2025/11/18/PrivateMarketplace)

### Windows Settings and File Explorer: Developer-Centric Enhancements

Windows updates this week address the needs of developers with tools for managing large projects. The Advanced Settings page and long path support resolve issues in handling more complex codebases. Integration with Git directly in File Explorer underlines Windows’ continuing commitment to supporting version control at the OS level.

- [What's New in Windows Settings for Developers: Advanced Settings, Long Path Support, and Git Integration]({{ "/2025-11-20-Whats-New-in-Windows-Settings-for-Developers-Advanced-Settings-Long-Path-Support-and-Git-Integration.html" | relative_url }})
- [What's New in Windows Settings for Developers: Advanced Settings, Long File Paths, and Git Integration]({{ "/2025-11-20-Whats-New-in-Windows-Settings-for-Developers-Advanced-Settings-Long-File-Paths-and-Git-Integration.html" | relative_url }})

### Git 2.52: Version Control, Performance, and Migration

Git 2.52 brings further improvements for managing large and legacy repositories. Features like ‘git last-modified’ support better traceability, while geometric repacking and updated tools for large codebases fulfill needs highlighted in recent coverage. Plans to move to SHA-256 and more Rust components demonstrate a continued commitment to security and maintainable workflows.

- [What's New in Git 2.52: Features and Performance Enhancements](https://github.blog/open-source/git/highlights-from-git-2-52/)

### AI-Enhanced, Cross-Platform Development with Uno Platform

Uno Platform continues the trend of AI-driven cross-platform development. Hot Design and Hot Reload for Studio, support for .NET 10, and Figma integration make it easier for designers and developers to work together and move from design to code more efficiently.

- [Building Cross-Platform .NET Apps with Uno Platform and Contextual AI]({{ "/2025-11-20-Building-Cross-Platform-NET-Apps-with-Uno-Platform-and-Contextual-AI.html" | relative_url }})

### Other Coding News

VS Code’s accessibility improvements build on earlier work, helping developers with different needs be more productive. GitHub’s open-source Annotation Toolkit for Figma enables better communication in design-to-code workflows, reinforcing shared standards and compliance.

The .NET Conf Student Zone 2025 showcases the ongoing commitment to practical education, supporting upskilling with hands-on content.

- [Accessibility in Visual Studio Code: Insights from Megan Rogge]({{ "/2025-11-17-Accessibility-in-Visual-Studio-Code-Insights-from-Megan-Rogge.html" | relative_url }})
- [Enhance Design-to-Code Collaboration with GitHub's Annotation Toolkit](https://github.blog/enterprise-software/collaboration/level-up-design-to-code-collaboration-with-githubs-open-source-annotation-toolkit/)
- [.NET Conf Student Zone 2025]({{ "/2025-11-17-NET-Conf-Student-Zone-2025.html" | relative_url }})

## DevOps

This collection covers practical automation in DevOps workflows, improvements to build and release processes, updates for GitHub Actions and VS Code, and enhancements around governance and migration support.

### GitHub and GitHub Actions: Migrations, Workflow Enhancements, and Platform Governance

GitHub continues supporting enterprise migrations with features like GitHub-owned blob storage, reducing setup complexity. New controls for managing App installations give organizations more say in integration security. Public preview updates to Pull Request “Files changed” help developers review large codebases. The Actions cache size limit increase expands support for larger monorepos and dependency sets.

- [Migrating Repositories with GitHub-Owned Blob Storage on GitHub Enterprise Cloud](https://github.blog/changelog/2025-11-17-migrating-repositories-with-github-owned-blob-storage-is-now-generally-available)
- [Controlling GitHub App Installations by Organization Owners](https://github.blog/changelog/2025-11-17-block-repository-administrators-from-installing-github-apps-on-their-own-now-in-public-preview)
- [Pull Request “Files Changed” Public Preview: November 20 Updates](https://github.blog/changelog/2025-11-20-pull-request-files-changed-public-preview-november-20-updates)
- [Expanded GitHub Actions Cache Limits Exceed 10 GB per Repository](https://github.blog/changelog/2025-11-20-github-actions-cache-size-can-now-exceed-10-gb-per-repository)

### CI/CD Automation, Migration, and Unified Build Approaches

New guides detail CI/CD automation in Microsoft Fabric and how to unify .NET build processes, streamlining deployment using virtual monorepos. Stories about CVS Health’s migration to GitHub Actions and guidance for migrating from Azure DevOps offer practical insight for teams moving to agent-based DevOps setups.

- [Automating Microsoft Fabric Deployments with Azure DevOps and Python](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/from-code-to-cloud-python-driven-microsoft-fabric-deployments/ba-p/4470447)
- [Reinventing .NET Build and Release: Unified Build Approach](https://devblogs.microsoft.com/dotnet/reinventing-how-dotnet-builds-and-ships-again/)
- [CVS Health’s Migration: Transforming Developer Experience with GitHub Actions]({{ "/2025-11-23-CVS-Healths-Migration-Transforming-Developer-Experience-with-GitHub-Actions.html" | relative_url }})
- [Azure DevOps to GitHub Migration Playbook: A Step-by-Step Guide for Agentic DevOps](https://devblogs.microsoft.com/all-things-azure/azure-devops-to-github-migration-playbook-unlocking-agentic-devops/)

### Visual Studio Code: Private Marketplace and IT Governance

The VS Code Private Marketplace provides better governance for organizations. Sessions on deployment and AI oversight reinforce responsible adoption and management, echoing previous efforts to streamline onboarding while maintaining control.

- [VS Code Private Marketplace: Enterprise Control Meets Developer Speed]({{ "/2025-11-18-VS-Code-Private-Marketplace-Enterprise-Control-Meets-Developer-Speed.html" | relative_url }})
- [Visual Studio Code Deployment and AI Governance for IT Pros]({{ "/2025-11-21-Visual-Studio-Code-Deployment-and-AI-Governance-for-IT-Pros.html" | relative_url }})

### Observability, Monitoring, and Security in DevOps Pipelines

Updated observability tools focus on proactive monitoring, with dashboards that help teams quickly identify incidents. MyDecisive’s Smart Telemetry Hub for Kubernetes and insights on deterministic guardrails reinforce a shift to actionable, policy-driven monitoring and code verification.

- [Observability and Security: Evolving DevOps Across Cloud-Native Environments](https://devops.com/observability-is-the-next-frontier-of-devops-and-cloud-security/)
- [MyDecisive Open Sources Smart Telemetry Hub for OpenTelemetry Data Processing](https://devops.com/mydecisive-open-sources-platform-for-processing-opentelemetry-data/)
- [Deterministic Guardrails for AI-Generated Code: Why Observability and Smarter Linters Matter](https://devops.com/the-deterministic-future-of-ai-generated-code/)

### DevOps for Data, GenAI, and MLOps

Coverage includes GenAI hackathons, the use of MLflow and Kubeflow, and observability across MLOps pipelines—a continuation of focus on explainability and security in enterprise automation.

- [DevOps for GenAI Toronto Hackathon: Lifecycle Automation, MLOps, and Enterprise AI Security](https://devops.com/devops-for-genai-toronto-edition-hackathon-unlocking-new-ai-market-opportunities/)

### Azure DevOps Integrations and Outage Readiness

New Azure DevOps integrations with Jira Service Management create connected, transparent lifecycle management, while coverage of outage response emphasizes best practices for reliability.

- [Integrating Azure DevOps with Jira Service Management: Practical Approaches and Real-World Scenarios](https://techcommunity.microsoft.com/t5/azure/integrating-azure-devops-with-jira-service-management-real-world/m-p/4471605#M22340)
- [Anatomy of an Outage: Evolving Transparency in Microsoft Engineering Teams]({{ "/2025-11-21-Anatomy-of-an-Outage-Evolving-Transparency-in-Microsoft-Engineering-Teams.html" | relative_url }})

### Other DevOps News

Golazo, an engineering workflow framework, addresses open team governance and knowledge management issues. Better GitHub license reporting helps with compliance and resource visibility in complex organizations.

- [Introducing Golazo: Open-Source Framework for Transparent Engineering Teams](https://techcommunity.microsoft.com/t5/azure-compute-blog/golazo-a-framework-for-streamlined-engineering/ba-p/4471142)
- [Improved Enterprise License Consumption Reporting for Outside Collaborators](https://github.blog/changelog/2025-11-17-improved-enterprise-license-consumption-reporting-for-outside-collaborators-now-generally-available)

## Security

Security updates cover expanded AI integration, automation, zero-trust principles, new security features in Azure, .NET, and Microsoft 365, and more detailed data and agent governance. These changes reflect an ongoing shift toward explainable, automated, and unified security practices.

### Azure Platform Security: New Foundations and Granular Controls

Azure now offers the MetaData Security Protocol (MSP) for VMs, with support for HMAC validation and eBPF Guest Proxy Agent. These bring controls for zero-trust and explicit allowlisting into general availability, supporting compliance.

- [Metadata Security Protocol (MSP) General Availability Secures Azure VM Metadata](https://techcommunity.microsoft.com/t5/azure-compute-blog/introducing-metadata-security-protocol-msp-elevating-platform/ba-p/4471204)

Azure Monitor Logs provides GA support for detailed RBAC at multiple levels, advancing least privilege for telemetry data.

- [Granular RBAC Now Generally Available in Azure Monitor Logs](https://techcommunity.microsoft.com/t5/azure-observability-blog/general-availability-granular-rbac-in-azure-monitor-logs/ba-p/4471299)

Azure DNS Security Policy, now generally available, links threat intelligence with DNS filtering and integrates with DevOps workflows.

- [Azure DNS Security Policy with Threat Intelligence Feed Now Generally Available](https://techcommunity.microsoft.com/t5/azure-networking-blog/announcing-azure-dns-security-policy-with-threat-intelligence/ba-p/4470183)

Microsoft also detailed its defense against a recent 15 Tbps DDoS attack, highlighting current adaptive, automated protections.

- [Defending the cloud: Azure neutralized a record-breaking 15 Tbps DDoS attack](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/defending-the-cloud-azure-neutralized-a-record-breaking-15-tbps/ba-p/4470422)

### Building Security for AI-Driven Workloads and Agents

Microsoft Entra now manages “Agent ID” for non-human actors, supporting identity lifecycle management and mitigation for issues like prompt injection.

- [Microsoft Entra: What's New in Secure Access on the AI Frontier]({{ "/2025-11-20-Microsoft-Entra-Whats-New-in-Secure-Access-on-the-AI-Frontier.html" | relative_url }})

Best practices for securing AI agents with Microsoft Defender and in Microsoft Foundry add practical strategies for real-world risk management.

- [Secure Your AI Agents with Microsoft Defender: Best Practices from Ignite 2025]({{ "/2025-11-21-Secure-Your-AI-Agents-with-Microsoft-Defender-Best-Practices-from-Ignite-2025.html" | relative_url }})
- [Securing AI Agents in Microsoft Foundry with Microsoft Security]({{ "/2025-11-21-Securing-AI-Agents-in-Microsoft-Foundry-with-Microsoft-Security.html" | relative_url }})

Oasis introduces more comprehensive credential management for non-person entities in the Microsoft environment.

- [Power Agentic Access: Governing Non-Human Identities with Oasis | Microsoft Ignite 2025]({{ "/2025-11-19-Power-Agentic-Access-Governing-Non-Human-Identities-with-Oasis-Microsoft-Ignite-2025.html" | relative_url }})

Zenity's integration provides runtime monitoring and incident response support for agent workflows in Copilot, Studio, and Foundry.

- [Securing the AI Agents with Zenity and Microsoft]({{ "/2025-11-19-Securing-the-AI-Agents-with-Zenity-and-Microsoft.html" | relative_url }})

### Microsoft Defender for Cloud and End-to-End Application Security

Defender for Cloud expands support for risk management and AI-powered threat detection, including pipelines, with integration for live risk assessments and artifact tracking. Defender’s connection with GitHub Advanced Security aids in automating secure development practices.

- [Defending Cloud Platforms: Unified Security with Microsoft Defender]({{ "/2025-11-20-Defending-Cloud-Platforms-Unified-Security-with-Microsoft-Defender.html" | relative_url }})
- [Unified Application Security with Microsoft Defender for Cloud]({{ "/2025-11-21-Unified-Application-Security-with-Microsoft-Defender-for-Cloud.html" | relative_url }})
- [Runtime Security and AI Fixes: Integrating GitHub Advanced Security with Defender for Cloud](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/security-where-it-matters-runtime-context-and-ai-fixes-now/ba-p/4470794)
- [Unified Code-to-Cloud Artifact Risk Visibility with Microsoft Defender for Cloud in GitHub](https://github.blog/changelog/2025-11-18-unified-code-to-cloud-artifact-risk-visibility-with-microsoft-defender-for-cloud-now-in-public-preview)

Security Copilot’s expanded role now includes Microsoft 365 E5, offering SIEM and XDR coverage plus automated PR remediation with Copilot Autofix. New artifact tracking and shielding cover legacy environments as well.

- [AI-Driven Security Agents Now in Microsoft 365 E5: Security Copilot Integration and Expansion](https://www.microsoft.com/en-us/security/blog/2025/11/18/agents-built-into-your-workflow-get-security-copilot-with-microsoft-365-e5/)
- [Security Copilot: Automating and Accelerating Defense with Agentic Workflows]({{ "/2025-11-21-Security-Copilot-Automating-and-Accelerating-Defense-with-Agentic-Workflows.html" | relative_url }})
- [AI-Powered Endpoint Security Updates in Microsoft Defender]({{ "/2025-11-21-AI-Powered-Endpoint-Security-Updates-in-Microsoft-Defender.html" | relative_url }})

### Comprehensive Governance for Data, Secrets, and Identity

Secrets management and identity rotation benefit from new technical guides for secure Azure Authentication and OIDC, bringing programmatic security best practices into DevOps pipelines.

- [Secure Secrets, Certificate, and Access Management for Azure]({{ "/2025-11-19-Secure-Secrets-Certificate-and-Access-Management-for-Azure.html" | relative_url }})

Microsoft Fabric has introduced finer-grained data permissions, offering write access at the folder and table levels, as well as assignment capabilities in the UI.

- [Fine-grained ReadWrite Access to Data with OneLake Security (Preview)](https://blog.fabric.microsoft.com/en-US/blog/fine-grained-readwrite-access-to-lakehouse-data-with-onelake-security/)

SQL auditing and encryption improvements offer better compliance management for regulated workloads.

- [Auditing Features for Fabric SQL Database (Preview)](https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-auditing-for-fabric-sql-database/)
- [Using Customer-Managed Keys with Microsoft Fabric SQL Database](https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-customer-managed-keys-in-fabric-sql-database/)

### Post-Quantum Cryptography Advances and Secure Coding

.NET now supports additional post-quantum cryptography algorithms (ML-KEM, ML-DSA), helping organizations prepare for new cryptographic requirements.

- [Post-Quantum Cryptography in .NET: New Algorithms and Design Principles](https://devblogs.microsoft.com/dotnet/post-quantum-cryptography-in-dotnet/)

The latest CodeQL release improves language coverage and precision for identifying vulnerabilities, building on previous releases.

- [CodeQL 2.23.5 Adds New Language Support and Security Query Improvements](https://github.blog/changelog/2025-11-19-codeql-2-23-5-adds-support-for-swift-6-2-new-java-queries-and-improved-analysis-accuracy)

MLSecOps and prompt security guidance now includes support for PromptGuard 2, CodeShield, and LlamaFirewall, expanding on earlier best practices for treating prompts as code in DevOps security checks.

- [MLSecOps and Prompt Security: DevOps Strategies for AI Pipeline Protection](https://devops.com/the-mlsecops-era-why-devops-teams-must-care-about-prompt-security/)

### Microsoft Sentinel: Agentic SIEM, Automation, and AI

Sentinel’s Data Lake feature supports larger-scale case management, while custom agent tools and marketplace integrations provide flexible automation paths. Blink micro-agents and Copilot support remediation action; SAP support adds industry application.

- [Power Agentic Defense with Microsoft Sentinel: Scalable Security Operations with AI, Data Lake, and Graph Intelligence]({{ "/2025-11-21-Power-Agentic-Defense-with-Microsoft-Sentinel-Scalable-Security-Operations-with-AI-Data-Lake-and-Graph-Intelligence.html" | relative_url }})
- [Sentinel Alert to Autonomous Action: Controlled AI Response Framework]({{ "/2025-11-19-Sentinel-Alert-to-Autonomous-Action-Controlled-AI-Response-Framework.html" | relative_url }})
- [Microsoft Sentinel Solution for SAP: Automated Asset Classification and Incident Response]({{ "/2025-11-19-Microsoft-Sentinel-Solution-for-SAP-Automated-Asset-Classification-and-Incident-Response.html" | relative_url }})

Privacy programs benefit from Copilot integration, automating many aspects of policy compliance.

- [Use AI Agents to Scale Privacy Programs with Microsoft Sentinel]({{ "/2025-11-19-Use-AI-Agents-to-Scale-Privacy-Programs-with-Microsoft-Sentinel.html" | relative_url }})

### Policy, Compliance, and Governance Workflows

Azure Policy now includes Service Groups, in-guest policies, and natural language authoring via Copilot, bringing automated compliance workflow support to more teams.

- [Build Secure Applications with Azure Policy and Service Groups]({{ "/2025-11-21-Build-Secure-Applications-with-Azure-Policy-and-Service-Groups.html" | relative_url }})

CIS Benchmarks are built-in and available for Azure-endorsed Linux, supporting compliance in hybrid and multi-cloud environments.

- [Built-In CIS Benchmarks for Linux Security on Azure: Flexible and Hybrid-Ready Compliance](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/from-policy-to-practice-built-in-cis-benchmarks-on-azure/ba-p/4467884)

### Other Security News

Continuous integration for security tools connects policy and evidence tracking throughout the code lifecycle, continuing recent efforts at automation and visibility.

- [Elevate DevEx 2.0 with Continuous Security Across the SDLC]({{ "/2025-11-19-Elevate-DevEx-20-with-Continuous-Security-Across-the-SDLC.html" | relative_url }})

Lifecycle coverage for .NET apps emphasizes paying for support after EOL, helping teams plan for service windows closing.

- [Managing .NET Support Lifecycles: Why Paying for Post-EOL Support Is Practical](https://andrewlock.net/companies-using-dotnet-need-to-suck-it-up-and-pay-for-support/)

Microsoft’s approach to autonomous security is reflected in unified dashboards, Copilot support, and predictive protection—linking oversight with adaptive AI techniques.

- [Ambient and Autonomous Security for the Agentic AI Era](https://www.microsoft.com/en-us/security/blog/2025/11/18/ambient-and-autonomous-security-for-the-agentic-era/)

Developments in adversarial AI defense, led by Microsoft and NVIDIA, continue to make use of real-time GPU-driven safeguards.

- [AI-Driven Adversarial Defense: Microsoft and NVIDIA's Real-Time Immunity Collaboration](https://techcommunity.microsoft.com/blog/microsoft-security-blog/collaborative-research-by-microsoft-and-nvidia-on-real-time-immunity/4470164)

Updates in email and collaboration security, including Defender for Office 365 and agent-based controls, offer additional automation for new threat types.

- [Securing Email and Collaboration with Microsoft Defender for Office 365 and Agentic AI]({{ "/2025-11-21-Securing-Email-and-Collaboration-with-Microsoft-Defender-for-Office-365-and-AI.html" | relative_url }})

Endpoint and Windows security updates offer improvements in device administration, quantum-ready certificates, and patching, making security easier to manage in production.

- [Inside Windows Security from Client to Cloud: Innovations in Windows 11 and Windows 365 | BRK258]({{ "/2025-11-21-Inside-Windows-Security-from-Client-to-Cloud-Innovations-in-Windows-11-and-Windows-365-BRK258.html" | relative_url }})

Further resources for this week span cross-platform security integration, data protection, and modern architecture best practices:

- [Secure the Modern Enterprise with Varonis and Microsoft Integration]({{ "/2025-11-21-Secure-the-Modern-Enterprise-with-Varonis-and-Microsoft-Integration.html" | relative_url }})
- [Bolster Your Data Security in the AI Era with Microsoft and Netskope]({{ "/2025-11-19-Bolster-Your-Data-Security-in-the-AI-Era-with-Microsoft-and-Netskope.html" | relative_url }})
- [Level up Microsoft security for insider threats]({{ "/2025-11-19-Level-up-Microsoft-security-for-insider-threats.html" | relative_url }})
- [Blueprint for Building the SOC of the Future]({{ "/2025-11-20-Blueprint-for-Building-the-SOC-of-the-Future.html" | relative_url }})
- [Preventing Data Exfiltration with Microsoft Purview's Layered Protection Strategy]({{ "/2025-11-21-Preventing-Data-Exfiltration-with-Microsoft-Purviews-Layered-Protection-Strategy.html" | relative_url }})
- [Comprehensive Data Security and Governance in AI Workloads with Microsoft Purview]({{ "/2025-11-20-Comprehensive-Data-Security-and-Governance-in-AI-Workloads-with-Microsoft-Purview.html" | relative_url }})
- [Envision Next Generation DLP with Microsoft Purview and Copilot]({{ "/2025-11-21-Envision-Next-Generation-DLP-with-Microsoft-Purview-and-Copilot.html" | relative_url }})
- [Data Protection in the Age of the Adversary: Accelerating Microsoft Purview Adoption]({{ "/2025-11-21-Data-Protection-in-the-Age-of-the-Adversary-Accelerating-Microsoft-Purview-Adoption.html" | relative_url }})
- [Maximizing Microsoft Purview Data Security Solutions: Best Practices and Implementation Stories]({{ "/2025-11-21-Maximizing-Microsoft-Purview-Data-Security-Solutions-Best-Practices-and-Implementation-Stories.html" | relative_url }})
- [Enhancing Data Security Investigations with Microsoft Purview and AI]({{ "/2025-11-20-Enhancing-Data-Security-Investigations-with-Microsoft-Purview-and-AI.html" | relative_url }})
- [End-to-End Security for AI Platforms, Apps, and Agents]({{ "/2025-11-20-End-to-End-Security-for-AI-Platforms-Apps-and-Agents.html" | relative_url }})
- [NIST Zero Trust with Forescout and Microsoft]({{ "/2025-11-19-NIST-Zero-Trust-with-Forescout-and-Microsoft.html" | relative_url }})
- [Active Directory Disaster Recovery: Modern Approaches for Secure Forest Restoration]({{ "/2025-11-19-Active-Directory-Disaster-Recovery-Modern-Approaches-for-Secure-Forest-Estoration.html" | relative_url }})
- [Building Secure-By-Design Environments with Azure Capabilities]({{ "/2025-11-19-Building-Secure-By-Design-Environments-with-Azure-Capabilities.html" | relative_url }})
- [Managing .NET Support Lifecycles: Why Paying for Post-EOL Support Is Practical](https://andrewlock.net/companies-using-dotnet-need-to-suck-it-up-and-pay-for-support/)
- [Setting Up Security Policies in Microsoft 365 Trial Tenants](https://dellenny.com/how-to-set-up-basic-security-policies-in-a-microsoft-365-trial-tenant/)
- [Setting Up Ransomware Protection in Windows 11: Step-by-Step Guide](https://dellenny.com/setting-up-ransomware-protection-in-windows-11-a-simple-and-complete-guide/)
- [Configuring Windows Firewall for Maximum Safety](https://dellenny.com/configuring-windows-firewall-for-maximum-safety/)
- [Windows 11 Security Features: Protecting Your PC and Data](https://dellenny.com/understanding-windows-11-security-features-a-shield-for-your-digital-life/)
