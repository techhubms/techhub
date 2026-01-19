---
layout: post
title: Updated AI Tools, Open-Source Security, and Azure Integration
author: Tech Hub Team
viewing_mode: internal
date: 2025-10-20 09:00:00 +00:00
permalink: /all/roundups/Weekly-AI-and-Tech-News-Roundup
tags:
- Agentic AI
- Automation
- Cloud Infrastructure
- Developer Workflows
- MCP
- Open Source
- Quantum Resilient Hardware
- VS Code
section_names:
- ai
- github-copilot
- ml
- azure
- coding
- devops
- security
---
Welcome to this week’s technology roundup. In this edition, we highlight the recent developments in developer-focused AI, new advancements in open-source and cloud security, improved efficiency for data and machine learning pipelines, and an increased interest in automation within hybrid and agent-based workflows.

GitHub Copilot has evolved from a code completion tool into a multi-model, AI-supported platform, expanding its integration across areas such as legacy system modernization, PowerShell scripting, SQL Server, and AI-driven code review. Azure’s latest updates introduce updates to open infrastructure, new developer CLI features, unified analytics, and carbon-aware API management for sustainable operations. On the security side, you’ll find Microsoft’s updates in quantum-resilient silicon, clear AI security benchmarks, and strengthened supply chain and marketplace protections, covering everything from source code to cloud deployments.

Teams spanning development, data science, operations, and compliance can see how this week’s stories further the movement toward unified, open, and smart automation. Read on to see how these updates are equipping technology professionals and organizations with the tools to approach ongoing digital challenges more effectively.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Core GitHub Copilot Developments and Integrations](#core-github-copilot-developments-and-integrations)
  - [GitHub Copilot CLI and Agentic Workflows](#github-copilot-cli-and-agentic-workflows)
  - [GitHub Copilot Integration in Visual Studio Code](#github-copilot-integration-in-visual-studio-code)
  - [Copilot Coding Agent and Automation Features](#copilot-coding-agent-and-automation-features)
  - [Model Updates and Prompt Engineering Best Practices](#model-updates-and-prompt-engineering-best-practices)
  - [Specialized Workflows: Testing, PowerShell, and Mainframe Modernization](#specialized-workflows-testing-powershell-and-mainframe-modernization)
  - [Copilot Customization and Advanced Agent Workflows](#copilot-customization-and-advanced-agent-workflows)
  - [Copilot in Real-World and Open Source Projects](#copilot-in-real-world-and-open-source-projects)
- [AI](#ai)
  - [Azure AI Foundry: Model Development, Fine-Tuning, and Multimodal Expansion](#azure-ai-foundry-model-development-fine-tuning-and-multimodal-expansion)
  - [Intelligent Documentation and GPT-4o Optimization](#intelligent-documentation-and-gpt-4o-optimization)
  - [Building and Deploying AI Agents: Container Apps, Open Source Orchestration, and MCP](#building-and-deploying-ai-agents-container-apps-open-source-orchestration-and-mcp)
  - [AI Workflows and Developer Empowerment in the Enterprise](#ai-workflows-and-developer-empowerment-in-the-enterprise)
  - [AI in Healthcare and Regulated Workflows](#ai-in-healthcare-and-regulated-workflows)
  - [AI Infrastructure and Datacenter Operations](#ai-infrastructure-and-datacenter-operations)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Microsoft Fabric Spark: Adaptive File Size Management for Delta Tables](#microsoft-fabric-spark-adaptive-file-size-management-for-delta-tables)
  - [Azure Data Lake Integrations: adlfs Python Library Improvements](#azure-data-lake-integrations-adlfs-python-library-improvements)
- [Azure](#azure)
  - [Open-Source and AI Infrastructure on Azure](#open-source-and-ai-infrastructure-on-azure)
  - [Azure Developer CLI and Container Ecosystem](#azure-developer-cli-and-container-ecosystem)
  - [Microsoft Fabric, Delta Lake, and Unified Data Engineering](#microsoft-fabric-delta-lake-and-unified-data-engineering)
  - [Hybrid Data, AI, and Integration: SAP BDC, Databricks, NetApp Files](#hybrid-data-ai-and-integration-sap-bdc-databricks-netapp-files)
  - [Oracle Database@Azure: AI Integration, Global Reach, Real-Time Analytics](#oracle-databaseazure-ai-integration-global-reach-real-time-analytics)
  - [AKS and Cloud-Native Scaling and Optimization](#aks-and-cloud-native-scaling-and-optimization)
  - [Platform Updates: Virtualization, Optimization, and More](#platform-updates-virtualization-optimization-and-more)
  - [Storage, Data Governance, and Automation](#storage-data-governance-and-automation)
  - [Analytics, Spark, and Real-Time Data Engineering](#analytics-spark-and-real-time-data-engineering)
  - [API Management, Sustainability, and Compliance](#api-management-sustainability-and-compliance)
  - [Migration, Digital Sovereignty, and Developer Environments](#migration-digital-sovereignty-and-developer-environments)
  - [Developer Tutorials and Guidance](#developer-tutorials-and-guidance)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET Platform Updates and Security Maintenance](#net-platform-updates-and-security-maintenance)
  - [Developer Workflow Enhancements: Design, Console UIs, and Tutorials](#developer-workflow-enhancements-design-console-uis-and-tutorials)
- [DevOps](#devops)
  - [Azure DevOps, GitHub Enterprise Server, and Self-Hosted Runner Innovations](#azure-devops-github-enterprise-server-and-self-hosted-runner-innovations)
  - [GitHub Automation and Specification-Driven Development](#github-automation-and-specification-driven-development)
  - [Observability, Toolchain Unification, and Real-World Security Practices](#observability-toolchain-unification-and-real-world-security-practices)
- [Security](#security)
  - [Open-Source Silicon Security and Quantum-Resilient Hardware Roots of Trust](#open-source-silicon-security-and-quantum-resilient-hardware-roots-of-trust)
  - [Secure Analytics Platforms: Customer-Managed Keys and Outbound Access Protection in Microsoft Fabric](#secure-analytics-platforms-customer-managed-keys-and-outbound-access-protection-in-microsoft-fabric)
  - [AI Benchmarks and Open Security Tooling for Modern SOC Workflows](#ai-benchmarks-and-open-security-tooling-for-modern-soc-workflows)
  - [DevOps Security: Modern Authentication, Secure Data Sharing, and End-to-End Encryption](#devops-security-modern-authentication-secure-data-sharing-and-end-to-end-encryption)
  - [Secure Coding and Supply Chain Defense in Developer Workflows](#secure-coding-and-supply-chain-defense-in-developer-workflows)
  - [Marketplace, Extension, and Privileged Access Risks](#marketplace-extension-and-privileged-access-risks)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot rolled out additional integrations and improvements this week, furthering its shift from a code completion tool to an AI platform designed for developers. Enhancements reinforce recent releases (such as Grok Code Fast 1 and Claude Sonnet 4.5), strengthening Copilot’s use of multiple models, support for more IDEs, and automation features. Copilot now contributes to a broader range of developer workflows, including Pull Request creation, PowerShell scripting, legacy system updates, and Model Context Protocol (MCP) agent support.

### Core GitHub Copilot Developments and Integrations

With last week’s Grok Code Fast 1 release and IDE model selector updates, Copilot’s multi-model system now better supports switching between OpenAI, Claude Sonnet 4.5, and Grok Code Fast 1. Claude Sonnet 4.5 is generally available across all Copilot plans, and Grok Code Fast 1 has expanded from preview to full support in GitHub.com, mobile apps, VS Code, JetBrains, Xcode, and Eclipse.

A new preview brings Copilot into SQL Server Management Studio 22, supporting T-SQL code suggestions and troubleshooting in line with earlier workflow additions. Commit message generation, previously in beta, is now broadly available, adding to Copilot’s growing automation features. Security functions like Autofix and AI-driven code review continue the focus on addressing vulnerabilities. Guidance is available for those migrating from deprecated Copilot knowledge bases to Copilot Spaces, detailing the migration timeline to support organizational planning.

- [GitHub Copilot: From Autocomplete to Multi-Model AI Coding Assistant](https://github.blog/ai-and-ml/github-copilot/copilot-faster-smarter-and-built-for-how-you-work-now/)
- [Claude Sonnet 4.5 Now Available in GitHub Copilot](https://github.blog/changelog/2025-10-13-anthropics-claude-sonnet-4-5-is-now-generally-available-in-github-copilot)
- [Grok Code Fast 1 Now Available in GitHub Copilot](https://github.blog/changelog/2025-10-16-grok-code-fast-1-is-now-generally-available-in-github-copilot)
- [Introducing GitHub Copilot Integration in SQL Server Management Studio 22](/videos/2025-10-16-Introducing-GitHub-Copilot-Integration-in-SQL-Server-Management-Studio-22.html)
- [Copilot-Generated Commit Messages Now Generally Available on GitHub.com](https://github.blog/changelog/2025-10-15-copilot-generated-commit-messages-on-github-com-are-generally-available)
- [Migrating Copilot Knowledge Bases to Copilot Spaces](https://github.blog/changelog/2025-10-17-copilot-knowledge-bases-can-now-be-converted-to-copilot-spaces)

### GitHub Copilot CLI and Agentic Workflows

Copilot CLI now includes updates aimed at terminal and AI-native workflows, resulting in easier onboarding and improved support for Git operations using global installation and clear permissions. The multiline input feature, introduced earlier, now offers more flexible interaction for developers.

Integration with Claude Haiku 4.5 and MCP server updates provides richer command handling and stable session management, contributing to better context management. PowerShell scripting capability has reached stable status in response to requests for effective cross-platform support.

Interest in open-source MCP projects is growing, with new frameworks and workflow automation gaining traction as more developers adopt AI-driven strategies through CLI and VS Code.

- [Getting Started with GitHub Copilot CLI in the Terminal](https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-how-to-get-started/)
- [GitHub Copilot CLI: Multiline Input, MCP Enhancements, and Haiku 4.5 Release](https://github.blog/changelog/2025-10-17-copilot-cli-multiline-input-new-mcp-enhancements-and-haiku-4-5)
- [9 Open Source MCP Projects Advancing AI-Native Developer Workflows](https://github.blog/open-source/accelerate-developer-productivity-with-these-9-open-source-ai-and-mcp-projects/)

### GitHub Copilot Integration in Visual Studio Code

Copilot’s integration with Visual Studio Code has expanded, building on recent agent features. Merge conflict resolution is now assisted by Copilot, providing input on both code branches.

Agent mode now enforces use of fully qualified tool names, aligning with the MCP protocol and registry improvements. The new Extensions Marketplace preview makes it easier to find MCP server backends. Copilot features display step-by-step reasoning and improved tooling for managing workflow approvals and navigation.

VS Code updates include enhanced keyboard shortcuts, system-aware profile detection, and stronger integration with test suites, all based on developer feedback. Collaboration continues to make agentic workflows smoother within the editor.

- [Visual Studio Code and GitHub Copilot - What's new in 1.105](/videos/2025-10-16-Visual-Studio-Code-and-GitHub-Copilot-Whats-new-in-1105.html)

### Copilot Coding Agent and Automation Features

The Copilot coding agent now features web search for gathering error details and documentation, extending prior troubleshooting updates. Asynchronous features allow for draft pull requests and review requests that do not require constant oversight from developers.

Naming conventions for branches and pull requests have been refined, improving workflow clarity. Policy settings are thoroughly documented so organizations can control integration and meet compliance goals.

- [Copilot Coding Agent Can Now Search the Web](https://github.blog/changelog/2025-10-16-copilot-coding-agent-can-now-search-the-web)
- [Copilot Coding Agent Improves Branch Naming and Pull Request Titles](https://github.blog/changelog/2025-10-16-copilot-coding-agent-uses-better-branch-names-and-pull-request-titles)

### Model Updates and Prompt Engineering Best Practices

GitHub Copilot now uses the GPT-4.1 code completion model, improving suggestion context and accuracy. This continues the trend of upgrading models by phasing out older versions like Claude Sonnet 3.5.

Prompt engineering guidance encourages version control and team review, with prompts stored in .prompt.md and copilot-instructions.md files. Treating prompts as maintainable components integrates AI assistance directly into existing development practices.

- [GPT-4.1 Copilot Code Completion Model – October Update](https://github.blog/changelog/2025-10-17-gpt-4-1-copilot-code-completion-model-october-update)
- [Treat Your AI Prompts Like Code](https://www.cooknwithcopilot.com/blog/treat-your-ai-prompts-like-code.html)

### Specialized Workflows: Testing, PowerShell, and Mainframe Modernization

Copilot’s ability to generate test suites in VS Code using prompts improves on previous automated testing features for Playwright and Jupyter. PowerShell automation now leverages Copilot Chat for Microsoft 365 and Azure, building on advances in CLI and agent workflows.

For mainframe modernization, Copilot and agent frameworks work alongside Azure orchestration to support updating legacy COBOL systems.

- [Generate a Test Suite with GitHub Copilot and Prompt-Driven Development](/videos/2025-10-14-Generate-a-Test-Suite-with-GitHub-Copilot-and-Prompt-Driven-Development.html)
- [Automating PowerShell Scripts with GitHub Copilot Chat for SysAdmins](https://dellenny.com/copilot-for-sysadmins-automating-powershell-script-generation-from-plain-english-prompts/)
- [How GitHub Copilot and AI Agents Modernize Legacy COBOL Systems](https://github.blog/ai-and-ml/github-copilot/how-github-copilot-and-ai-agents-are-saving-legacy-systems/)

### Copilot Customization and Advanced Agent Workflows

Developers can now use Agent Package Manager in conjunction with GitHub Actions to orchestrate and maintain AI agents, supporting version control and auditing.

A technical podcast with Harald Kirschner offers insight into customizing chat agents within VS Code, covering the new Agent Memory extension for context management. These updates add new customization options to Copilot’s agent-based features.

- [How to Build Reliable AI Workflows with Agentic Primitives and Context Engineering](https://github.blog/ai-and-ml/github-copilot/how-to-build-reliable-ai-workflows-with-agentic-primitives-and-context-engineering/)
- [Building Agent Memory for VS Code with Harald Kirschner](/videos/2025-10-13-Building-Agent-Memory-for-VS-Code-with-Harald-Kirschner.html)

### Copilot in Real-World and Open Source Projects

Case studies such as the ‘No Bark’ open-source project illustrate how Copilot supports accessibility and deployment for those without a coding background. Developers are also encouraged to join the open source MCP community and contribute to agentic workflow innovation.

- [How GitHub Copilot and Azure AI Apps Fueled a Real-World Project: The 'No Bark' Solution](/videos/2025-10-15-How-GitHub-Copilot-and-Azure-AI-Apps-Fueled-a-Real-World-Project-The-No-Bark-Solution.html)

## AI

This week’s AI news centers around deeper integration of language models, agent frameworks, and cloud infrastructure, particularly on Azure. You’ll find updates to documentation automation, workload-focused model optimization, and infrastructure architecture, building a foundation for scalable and secure enterprise AI adoption.

### Azure AI Foundry: Model Development, Fine-Tuning, and Multimodal Expansion

Azure AI Foundry is positioned as the main hub for creating and managing AI applications. The latest release of LangChain v1 introduces initial Foundry support, improved agent APIs, and updated migration guides. Richer UI integration is now possible thanks to new structured content blocks, and expanded I/O supports text, images, files, and video for better functionality across real-world applications.

Highlights in fine-tuning include expanded support for Reinforcement Fine-Tuning (RFT), a more cost-effective Developer Tier, easier APIs for LLM customization (such as GPT-4o), and accelerated evaluation and deployment. Code samples are provided for RFT, distillation, and multi-region rollout.

Azure AI Foundry introduces Sora 2 in public preview, letting organizations use a REST API to generate detailed, physics-aware video with audio. This makes secure, scalable content generation for education and marketing simpler, following updates to Azure’s multimodal support.

- [LangChain v1 Launches with Azure AI Foundry Support and Streamlined Agent APIs](https://techcommunity.microsoft.com/t5/microsoft-developer-community/langchain-v1-is-now-generally-available/ba-p/4462159)
- [The Developer’s Guide to Smarter Fine-tuning with Azure AI Foundry](https://devblogs.microsoft.com/foundry/the-developers-guide-to-smarter-fine-tuning/)
- [Sora 2 Public Preview Now Available in Azure AI Foundry](https://azure.microsoft.com/en-us/blog/sora-2-now-available-in-azure-ai-foundry/)

### Intelligent Documentation and GPT-4o Optimization

Efforts to automate documentation now combine natural language processing, large language models, retrieval-augmented generation (RAG), and embedding search. Integrating with VS Code and JetBrains can save up to 80% of manual effort. These workflows use distributed inference, optimize real-time delivery, and scale with vector stores.

A case study with GPT-4o-mini on Azure OpenAI Service identifies strategies to handle throttling and timeouts—using token capping, streaming, and regional routing—to lower error rates and costs. Enhanced diagnostics and API management support stable large-scale deployments.

- [NLP Tools for Intelligent Documentation and Developer Enablement](https://devops.com/nlp-tools-for-intelligent-documentation-and-developer-enablement-2/)
- [From Timeouts to Triumph: Optimizing GPT-4o-mini for Speed, Efficiency, and Reliability](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-timeouts-to-triumph-optimizing-gpt-4o-mini-for-speed/ba-p/4461531)

### Building and Deploying AI Agents: Container Apps, Open Source Orchestration, and MCP

Hosting agentic AI is now easier, with agents like Goose scaling on Azure Container Apps, benefiting from managed GPUs and secure setups. Quickstart guides help teams deploy agents efficiently.

Archestra, featured in Open Source Friday, is built on the Microsoft Cloud Platform and allows secure, permissioned orchestration of agents and models. The MCP registry progresses, helping standardize context metadata and support effective collaboration among developers of open-source AI tools.

- [Building Agents on Azure Container Apps with Goose AI Agent, Ollama, and gpt-oss](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-agents-on-azure-container-apps-with-goose-ai-agent/ba-p/4460215)
- [Open Source Friday: Archestra – Secure Platform for Enterprise Agents with MCP](/videos/2025-10-14-Open-Source-Friday-Archestra-Secure-Platform-for-Enterprise-Agents-with-MCP.html)
- [Unlocking the Power of MCP: Model Context Protocol in Open Source AI Tools](/videos/2025-10-18-Unlocking-the-Power-of-MCP-Model-Context-Protocol-in-Open-Source-AI-Tools.html)

### AI Workflows and Developer Empowerment in the Enterprise

Enterprise teams continue to implement open standards like MCP, making AI solutions easier to integrate. Modular agent frameworks and tools such as Copilot Studio help create tailored “digital twin” AI agents for specific domains, with secure integration and custom prompt support.

Information shows that developers are rapidly automating tasks with AI, though project managers tend to adopt at a slower rate, pointing to ongoing needs for training and organizational involvement.

- [How Developers Are Leading AI Transformation Across the Enterprise](https://www.microsoft.com/en-us/microsoft-cloud/blog/2025/10/13/fyai-why-developers-will-lead-ai-transformation-across-the-enterprise/)
- [Digital Twin Employees: Hyper-Personalized AI Prompts with Copilot Studio](https://dellenny.com/the-digital-twin-employee-creating-hyper-personalized-copilot-prompts-with-copilot-studio/)
- [Survey Finds Developers Adopting AI More Rapidly Than Project Managers](https://devops.com/survey-sees-developers-embracing-ai-faster-than-project-leaders/)

### AI in Healthcare and Regulated Workflows

Microsoft has expanded Dragon Copilot’s AI features to nursing and clinical processes, letting partners create custom content and automate documentation. These improvements help reduce administrative work and support more efficient, ambient workflows.

A GenAI Solution Accelerator for energy permitting uses AI to automate approvals and compliance paperwork, highlighting new adoption in regulated industries.

- [Microsoft Expands Dragon Copilot AI Innovations for Nursing and Healthcare Partners](https://news.microsoft.com/source/2025/10/16/microsoft-extends-ai-advancements-in-dragon-copilot-to-nurses-and-partners-to-enhance-patient-care/)
- [Microsoft Introduces Dragon Copilot Ambient AI Experience for Nursing Workflows](https://www.linkedin.com/posts/satyanadella_no-question-that-nurses-are-the-heartbeat-activity-7384590573808234496-aaYh)
- [Microsoft GenAI for Energy Permitting Solution Accelerator](/videos/2025-10-16-Microsoft-GenAI-for-Energy-Permitting-Solution-Accelerator.html)

### AI Infrastructure and Datacenter Operations

OpenAI, Microsoft, and Nvidia are using new methods to stabilize power for GPU-based AI training, including software controls, hardware management, and rack-level storage to support datacenter operations.

The Open Compute Project supports standardized APIs and onboarding processes, making it easier to manage infrastructure with mixed CPU and GPU resources, while promoting resilient AI clusters.

- [Power Stabilization Strategies for AI Training Datacenters](https://techcommunity.microsoft.com/t5/azure-compute-blog/power-stabilization-for-ai-training-datacenters/ba-p/4460937)
- [Operational Excellence in AI Infrastructure: Standardized Node Lifecycle Management](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/operational-excellence-in-ai-infrastructure-fleets-standardized/ba-p/4460754)

### Other AI News

The CX Observe Product Feedback Copilot converts support and survey feedback into actionable product insights, supporting data-driven workflows integrated with Azure and extending ongoing automation trends.

- [CX Observe Product Feedback Copilot: AI-Powered Insights for Azure Product Leaders](https://www.microsoft.com/en-us/garage/wall-of-fame/cx-observe-product-feedback/)

## ML

Recent ML updates target smoother data engineering and greater Azure integration, making performance and reliability improvements for lakehouse and machine learning frameworks common in big data workflows.

### Microsoft Fabric Spark: Adaptive File Size Management for Delta Tables

Fabric Spark introduces adaptive file size management, automatically choosing optimal Delta table file sizes based on telemetry data. This automation streamlines ELT and analytics tasks, resulting in up to 2.8 times faster file compaction and 1.6 times TPC-DS performance improvements. Settings update automatically as workloads shift, but developers can tailor configurations to suit specific needs.

Benefits also include improved data skipping, reduced file rewrite costs, and increased processing parallelism, all supporting secure and flexible solutions.

- [Adaptive Target File Size Management in Fabric Spark](https://blog.fabric.microsoft.com/en-US/blog/adaptive-target-file-size-management-in-fabric-spark/)

### Azure Data Lake Integrations: adlfs Python Library Improvements

The adlfs Python library receives speed improvements through parallel block uploads and smaller chunk defaults, helping users avoid timeouts on geo-distributed systems and supporting more secure data pipelines.

Frameworks like Dask, Pandas, Ray, PyTorch, and PyIceberg work seamlessly with these updates, which include easier authentication and continued fsspec compatibility, supporting efficient integration for modern data and AI workflows.

- [Easily Connect AI Workloads to Azure Blob Storage with adlfs](https://devblogs.microsoft.com/azure-sdk/easily-connect-ai-workloads-to-azure-blob-storage-with-adlfs/)

## Azure

This week’s Azure content highlights improved open-source infrastructure, automation features, hybrid data innovation, and operational enhancements that impact everything from CLI tools to platform-level services.

### Open-Source and AI Infrastructure on Azure

Microsoft focuses on open standards and aspects of collaborative hardware and software infrastructure design at the OCP Global Summit. Recent releases include new network fabrics, hardware resistant to quantum security threats, and carbon tracking features, continuing Azure’s commitment to sustainable, reliable cloud systems.

- [Microsoft Advances Open-Source Infrastructure for Frontier-Scale AI](https://azure.microsoft.com/en-us/blog/accelerating-open-source-infrastructure-development-for-frontier-ai-at-scale/)
- [Next Generation HXU: Doubling Cooling Power for the AI Era](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/next-generation-hxu-doubling-cooling-power-for-the-ai-era/ba-p/4460953)

### Azure Developer CLI and Container Ecosystem

The October 2025 update for Azure Developer CLI introduces layered provisioning, clear service dependencies, and a cleaner separation between publishing images and app deployment. New extension and template management continues the focus on workflow improvement and build-to-deploy efficiency.

CLI enhancements also support hosting the MCP server and transitioning workloads from Functions to Container Apps, improving productivity through better workflow tools.

- [Azure Developer CLI (azd) October 2025: Layered Provisioning, Service Dependencies, and More](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-october-2025/)
- [Hosting Remote MCP Server on Azure Container Apps Using HTTP Transport](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/hosting-remote-mcp-server-on-azure-container-apps-aca-using/ba-p/4459263)
- [Transition to Azure Functions V2 on Azure Container Apps: Migration Guide and Feature Overview](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/transition-to-azure-functions-v2-on-azure-container-apps/ba-p/4457258)

### Microsoft Fabric, Delta Lake, and Unified Data Engineering

New features in Fabric include Shortcut Transformations for Parquet and JSON, job-level bursting for parallel performance, enhanced diagnostics, and a preview of OneLake Table APIs for deeper interoperability. These updates simplify ingestion and analytics and strengthen support for open standards.

- [Simplifying Parquet & JSON Data Ingestion with Microsoft Fabric Shortcut Transformations](https://blog.fabric.microsoft.com/en-US/blog/from-files-to-delta-tables-parquet-json-data-ingestion-simplified-with-shortcut-transformations/)
- [Introducing the Job-Level Bursting Switch in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/introducing-the-job-level-bursting-switch-in-microsoft-fabric/)
- [Gain End-to-End Visibility into Data Activity Using OneLake Diagnostics](https://blog.fabric.microsoft.com/en-US/blog/gain-end-to-end-visibility-into-data-activity-using-onelake-diagnostics/)
- [Previewing OneLake Table APIs for Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/now-in-preview-onelake-table-apis/)
- [Sourcing Schema-Driven Events from EventHub into Fabric Eventstreams (Preview)](https://blog.fabric.microsoft.com/en-US/blog/sourcing-schema-driven-events-from-eventhub-into-fabric-eventstreams-preview/)

### Hybrid Data, AI, and Integration: SAP BDC, Databricks, NetApp Files

SAP BDC Connect now links Azure Databricks and enables real-time analytics. The Unity Catalog and Delta Sharing features support data governance and compliance. Azure NetApp Files’ Object REST API enables unified file, block, and object workflows for analytics, building on improvements to data engineering efficiency.

- [SAP Business Data Cloud Connect for Azure Databricks: General Availability Announcement](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/sap-business-data-cloud-connect-with-azure-databricks-is-now/ba-p/4459490)
- [How Azure NetApp Files Object REST API Powers Azure Data and AI Solutions](https://techcommunity.microsoft.com/t5/azure-architecture-blog/how-azure-netapp-files-object-rest-api-powers-azure-and-isv-data/ba/p/4459545)

### Oracle Database@Azure: AI Integration, Global Reach, Real-Time Analytics

New options for Oracle Database@Azure—such as Base, Exadata, Autonomous, and AI Lakehouse—expand support for hybrid data and AI workflows. Other updates include native mirroring, managed replication, and integration with Copilot and Azure AI Foundry for improved automation and productivity.

- [Oracle Database@Azure Advances at Oracle AI World 2025: Integration, AI, and Security Enhancements](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/oracle-database-azure-at-oracle-ai-world-2025-powering-the-next/ba-p/4460749)
- [New Features and Global Expansion for Oracle Database@Azure: Unlocking Hybrid Data and AI Innovation](https://azure.microsoft.com/en-us/blog/oracle-databaseazure-offers-new-features-regions-and-programs-to-unlock-data-and-ai-innovation/)

### AKS and Cloud-Native Scaling and Optimization

AKS adds a workflow guide for using low-priority pods, making it easy to scale quickly and manage capacity with buffer pods—leveraging YAML patterns with Prometheus and Grafana monitoring for improved reliability and performance.

- [Leveraging Low Priority Pods for Rapid Scaling in AKS](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/leveraging-low-priority-pods-for-rapid-scaling-in-aks/ba-p/4461670)

### Platform Updates: Virtualization, Optimization, and More

Azure VMs now offer preview capabilities for turning off multi-threading and tuning processor cores, providing more control over high-performance and analytics workloads. Ephemeral OS disk support accelerates provisioning in Azure Virtual Desktop. Capacity Reservation management expands, supporting disaster recovery, and extended Windows 10 security updates are available for compliance.

- [Public Preview: VM Customization in Azure Enables Disabling Multithreading and Constrained Cores](https://techcommunity.microsoft.com/t5/azure-compute-blog/announcing-public-preview-of-vm-customization-in-azure-disable/ba-p/4462417)
- [Ephemeral OS Disk Support Now in Public Preview on Azure Virtual Desktop](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/now-in-public-preview-ephemeral-os-disk-support-on-azure-virtual/ba-p/4460172)
- [Public Preview: Sharing Capacity Reservation Groups Across Azure Subscriptions](https://techcommunity.microsoft.com/t5/azure-compute-blog/public-preview-for-sharing-capacity-reservation-groups-now/ba/p/4461834)
- [Windows 10 Extended Security Updates for Azure Virtual Desktop](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/windows-10-extended-security-updates-for-azure-virtual-desktop/ba-p/4459715)

### Storage, Data Governance, and Automation

Azure Storage Discovery, with integrated Copilot support, is now generally available for analytics and cost tracking. Azure Storage Actions broaden automation and policy enforcement, and Azure Migrate introduces recommendations to help teams manage storage efficiently during transitions.

- [Unlock Insights with Azure Storage Discovery and Copilot Integration](https://azure.microsoft.com/en-us/blog/from-queries-to-conversations-unlock-insights-about-your-data-using-azure-storage-discovery-now-generally-available/)
- [Beyond Basics: Practical Scenarios with Azure Storage Actions](https://techcommunity.microsoft.com/t5/azure-storage-blog/beyond-basics-practical-scenarios-with-azure-storage-actions/ba/p/4447151)
- [Unlock Cost Savings with Utilization-Based Storage Recommendations in Azure Migrate](https://techcommunity.microsoft.com/t5/azure-migration-and/unlock-cost-savings-with-utilization-based-storage/ba/p/4461634)

### Analytics, Spark, and Real-Time Data Engineering

Azure Synapse Runtime for Apache Spark 3.5 is generally available, supporting the migration from earlier versions and providing documentation for analytics and machine learning platform upgrades.

- [Azure Synapse Runtime for Apache Spark 3.5 Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/general-availability-azure-synapse-runtime-for-apache-spark-3-5/)

### API Management, Sustainability, and Compliance

Azure API Management’s new carbon-aware routing preview lets teams track API impact and route calls to regions with a lower carbon footprint, supporting environmental goals and compliance requirements.

- [Environmental Sustainability Features in Azure API Management: Minimizing API Infrastructure Carbon Impact](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/building-environmental-aware-api-platforms-with-azure-api/ba-p/4458308)

### Migration, Digital Sovereignty, and Developer Environments

Azure’s digital sovereignty guidance for Switzerland, along with strategies for persistent hybrid developer environments, provides a foundation for regulatory compliance and practical development workflows using Dev Box and Codespaces.

- [How Microsoft is Addressing Digital Sovereignty in Switzerland](https://www.thomasmaurer.ch/2025/10/how-microsoft-is-addressing-digital-sovereignty-in-switzerland/)
- [Beyond the Desktop: The Future of Development with Microsoft Dev Box and GitHub Codespaces](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/beyond-the-desktop-the-future-of-development-with-microsoft-dev/ba/p/4459483)

### Developer Tutorials and Guidance

Available resources include deployment strategies for Azure Linux Web Apps and introductory material for quantum development on Azure.

- [Deployment and Build Strategies for Azure Linux Web Apps](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deployment-and-build-from-azure-linux-based-web-app/ba/p/4461950)
- [Jump Starting Quantum Computing on Azure](https://techcommunity.microsoft.com/t5/azure-compute-blog/jump-starting-quantum-computing-on-azure/ba/p/4459053)

### Other Azure News

This week’s Azure Weekly Update covers releases for serverless compute, storage, security, migration, SAP-Databricks integration, and new GPT models.

- [Azure Weekly Update: October 17, 2025](/videos/2025-10-17-Azure-Weekly-Update-October-17-2025.html)

New EDA benchmarking shows Azure NetApp Files can meet scale and performance targets for HPC workloads.

- [Validating Scalable EDA Storage Performance: Azure NetApp Files and SPECstorage Solution 2020](https://techcommunity.microsoft.com/t5/azure-architecture-blog/validating-scalable-eda-storage-performance-azure-netapp-files/ba/p/4459517)

## Coding

The .NET platform saw the arrival of .NET 10 RC2, new security patches, and updated workflows for automating design and console interfaces, laying the groundwork for a modern and secure application environment.

### .NET Platform Updates and Security Maintenance

.NET 10 RC2 is ready for use in production, bringing features like upgraded microphone control, SafeAreaEdges in .NET MAUI, XAML source generation, platform compatibility improvements, extended JSON support in EF Core, and enhanced support for .NET MSBuild tasks. Full details are available in the release notes. Developers are encouraged to validate applications and check documentation before the GA release.

Concurrent with other platform and debugging updates, RC2 continues to improve reliability.

Security updates for October 2025 address issues in .NET 8.0, 9.0, and legacy .NET Framework versions, patching vulnerabilities like information disclosure, feature bypass concerns, denial of service, and remote code execution. The fixes extend to runtime, SDK, ASP.NET Core, and container images; teams should update promptly and use provided support materials.

- [Announcing .NET 10 Release Candidate 2](https://devblogs.microsoft.com/dotnet/dotnet-10-rc-2/)
- [.NET and .NET Framework October 2025 Servicing Updates: Security Fixes and Release Details](https://devblogs.microsoft.com/dotnet/dotnet-and-dotnet-framework-october-2025-servicing-updates/)

### Developer Workflow Enhancements: Design, Console UIs, and Tutorials

A live VS Code session demonstrates how to connect Figma MCP Server with Code Connect, making it easier to synchronize design tokens and produce up-to-date code from designs in Figma to Visual Studio Code.

This aligns with recent MCP updates in VS Code, providing more custom and integrated workflows.

A new guide highlights how to use RazorConsole to build interactive, visually rich .NET console apps, showing how to integrate RazorConsole, manage layouts, and create modern UIs. This encourages teams to bring new capabilities to CLI tools.

Building on last week’s discussions on debugging and workflow improvement, these tools enhance .NET development experiences and customizability.

- [VS Code Live: Integrating Figma MCP Server with Code Connect](/videos/2025-10-16-VS-Code-Live-Integrating-Figma-MCP-Server-with-Code-Connect.html)
- [Building the Coolest Console Apps in .NET](/videos/2025-10-13-Building-the-Coolest-Console-Apps-in-NET.html)

## DevOps

DevOps news includes improvements to secure automation, tool unification, and observability. Major updates were released for Azure DevOps, GitHub Enterprise Server, and other open-source components. Industry insights explore the ongoing challenges of complexity and security, with a focus on the intersection of automation and human review.

### Azure DevOps, GitHub Enterprise Server, and Self-Hosted Runner Innovations

Azure DevOps Local MCP Server is now generally available, supporting in-house automation and AI features in controlled environments. Improvements include enhanced authorization, broader object coverage, domain scoping, and per-project configuration, providing flexibility and secure deployment. Open-source access maintains a strong user community.

GitHub Enterprise Server 3.18 delivers extra security and scaling, with custom properties, merge rules, scalable project issues, code scanning notifications, and OpenTelemetry instrumentation. Dependabot access is easier for teams managing large codebases.

Actions Runner Controller 0.13.0 now offers container lifecycle hooks, improved dual-stack networking, and finalized Azure Key Vault integration for deployment, networking, and secrets management.

- [Azure DevOps Local MCP Server Now Generally Available](https://devblogs.microsoft.com/devops/azure-devops-local-mcp-server-generally-available/)
- [GitHub Enterprise Server 3.18 Release Overview](https://github.blog/changelog/2025-10-14-github-enterprise-server-3-18-is-now-generally-available)
- [Actions Runner Controller 0.13.0: Storage, Networking, and Azure Key Vault Updates](https://github.blog/changelog/2025-10-16-actions-runner-controller-release-0-13-0)

### GitHub Automation and Specification-Driven Development

GitHub MCP Server introduces management for Projects, centralizing configuration-driven workflow automation and building on efficiency improvements from earlier versions.

Spec Kit rolls out a specification-first approach, using commands like `/specify`, `/plan`, and `/implement` to convert requirements into actionable code plans and scaffolding, supporting more systematic automation of software projects.

- [GitHub MCP Server Adds GitHub Projects Management and Improves Toolset Efficiency](https://github.blog/changelog/2025-10-14-github-mcp-server-now-supports-github-projects-and-more)
- [Introducing GitHub Spec Kit: A New Approach to Software Development](/videos/2025-10-13-Introducing-GitHub-Spec-Kit-A-New-Approach-to-Software-Development.html)

### Observability, Toolchain Unification, and Real-World Security Practices

Site reliability engineers (SREs) address alert volume by prioritizing actionable metrics and post-incident reviews. OpenTelemetry unifies telemetry and continues to improve monitoring, as seen with GitHub Enterprise Server.

Governance solutions like CloudBees Unify bring artifact tracking across platforms, helping organizations gradually adopt more automated and AI‑ready DevOps without abrupt migration.

Despite advancement in infrastructure as code and DevOps security, implementation sometimes falls short. Recommendations focus on automating routine tasks, implementing policy-as-code, and pairing AI with direct engineering oversight.

A Fastly survey notes rapid but measured AI uptake in DevSecOps, emphasizing that automation needs human review for best results.

- [When Metrics Overwhelm: How SREs Help Engineers Reclaim Focus](https://devops.com/when-metrics-overwhelm-how-sres-help-engineers-reclaim-focus/)
- [Beyond the Platform: How Enterprises Can Unify Their DevOps Toolchains for Better Governance and AI Readiness](https://devops.com/beyond-the-platform-how-enterprises-can-unify-their-devops-toolchains-for-better-governance-and-ai-readiness/)
- [Infrastructure as Code, Security Blind Spots, and the Messy Reality of DevOps](https://devops.com/infrastructure-as-code-security-blind-spots-and-the-messy-reality-of-devops/)
- [Survey Reveals Rapid AI Adoption to Strengthen DevSecOps Practices](https://devops.com/survey-surfaces-widespread-adoption-of-ai-to-improve-devsecops/)

## Security

Security news this week covers updates to open-source hardware, data protection features, secure analytics, encrypted DevOps workflows, and improved guidance for software supply chain safety.

### Open-Source Silicon Security and Quantum-Resilient Hardware Roots of Trust

Caliptra 2.1 from Microsoft adds quantum-resilient cryptography, advanced key management, and secure erase options. Formal property verification with open documentation helps silicon developers and cloud providers build stronger confidential computing environments.

- [Caliptra 2.1: Advancing Open-Source Silicon Root of Trust for Data Protection](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/caliptra-2-1-an-open-source-silicon-root-of-trust-with-enhanced/ba-p/4460758)

### Secure Analytics Platforms: Customer-Managed Keys and Outbound Access Protection in Microsoft Fabric

Microsoft Fabric now features customer-managed keys for workspace encryption and has moved outbound access protection to general availability for Warehouses, Notebooks, and SQL Analytics. Updates make securing and controlling access at the workspace level easier for teams.

- [Extending Outbound Access Protection to Fabric Warehouse and SQL Analytics Endpoint](https://blog.fabric.microsoft.com/en-US/blog/extending-outbound-access-protection-to-fabric-warehouse-and-sql-analytics-endpoint/)
- [Customer-Managed Keys for Microsoft Fabric Workspaces Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-for-fabric-workspaces-generally-available/)
- [Customer-Managed Keys Now Available for Fabric Warehouse and SQL Analytics Endpoint](https://blog.fabric.microsoft.com/en-US/blog/bringing-customer-managed-keys-to-fabric-warehouse-and-sql-analytics-endpoint/)

### AI Benchmarks and Open Security Tooling for Modern SOC Workflows

The ExCyTIn-Bench toolkit simulates advanced attack scenarios, helping SOC teams assess LLM performance using Sentinel log data and incident graphs. Open results and adaptability help speed up adoption of security-focused AI tools and features.

- [ExCyTIn-Bench: Benchmarking AI Performance in Cybersecurity Investigations](https://www.microsoft.com/en-us/security/blog/2025/10/14/microsoft-raises-the-bar-a-smarter-way-to-measure-ai-for-cybersecurity/)
- [Open Source Benchmarking Tool to Measure AI for Cybersecurity](https://www.linkedin.com/posts/satyanadella_microsoft-raises-the-bar-a-smarter-way-to-activity-7384329693614759936-VB0Z)

### DevOps Security: Modern Authentication, Secure Data Sharing, and End-to-End Encryption

Microsoft is transitioning authentication procedures for Visual Studio and Azure DevOps to Entra ID, improving security and access control. Delta Sharing in Databricks is now safer thanks to implementation of NCC and private endpoints. Research continues into adopting end-to-end encryption for Git, supporting improved software supply chain security.

- [Modernizing Authentication for Legacy Visual Studio Clients](https://devblogs.microsoft.com/devops/modernizing-authentication-for-legacy-visual-studio-clients/)
- [Secure Delta Sharing Between Databricks Workspaces Using NCC and Private Endpoints](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/secure-delta-sharing-between-databricks-workspaces-using-ncc-and/ba/p/4462428)
- [Efficient End-to-End Encryption for Git Services: Enhancing DevOps Security](https://devops.com/git-services-need-better-security-heres-how-end-to-end-encryption-could-help/)

### Secure Coding and Supply Chain Defense in Developer Workflows

GitHub CodeQL’s Rust support and build-free C/C++ scanning improve developer ability to identify vulnerabilities, especially during CI/CD and code review. Tutorials on using SBOMs, VEX advisories, and eBPF for runtime inspection extend supply chain policy and runtime observability.

- [CodeQL Adds Rust and Build-Free C/C++ Scanning in General Availability](https://github.blog/changelog/2025-10-14-codeql-scanning-rust-and-c-c-without-builds-is-now-generally-available)
- [Establishing Visibility and Governance for Your Software Supply Chain](https://devops.com/establishing-visibility-and-governance-for-your-software-supply-chain/)

### Marketplace, Extension, and Privileged Access Risks

Wiz researchers found over 500 VS Code and OpenVSX extensions with hardcoded secrets, putting over 150,000 users at risk; Microsoft has introduced secret scans before publication. Updated best practices for privileged tools cover measures such as PRMFA, RBAC, and JIT/JEA to help isolate high-risk actions.

- [VS Code Marketplace Secret Leaks Highlight Risks in Extensions and AI Configurations](https://devops.com/massive-vs-code-secrets-leak-puts-focus-on-extensions-ai-wiz/)
- [Hardening Customer Support Tools Against Cyberattacks: Microsoft’s Approach](https://www.microsoft.com/en-us/security/blog/2025/10/15/the-importance-of-hardening-customer-support-tools-against-attack/)

### Other Security News

Microsoft has started .NET Security Group to coordinate CVE management for faster, more effective patching. GitHub is updating Dependabot Alerts API to use cursor-based pagination, streamlining supply chain notification. Industry commentary urges teams to use context-aware controls, monitoring, and incident response practices rather than relying solely on “shift left” development.

- [Announcing the .NET Security Group](https://devblogs.microsoft.com/dotnet/announcing-dotnet-security-group/)
- [Dependabot Alerts API Deprecates Offset-Based Pagination Parameters](https://github.blog/changelog/2025-10-14-dependabot-alerts-api-pagination-parameters-deprecated)
- [Why 'Shift Left' Alone Isn't Enough: Embedding Security Across Software Delivery](https://devops.com/secure-by-design-secure-by-default/)
