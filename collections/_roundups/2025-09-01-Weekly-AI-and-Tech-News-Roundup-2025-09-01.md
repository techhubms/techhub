---
title: Updates in AI Developer Tools, Secure Cloud Features, and DevOps Practices
author: TechHub
date: 2025-09-01 09:00:00 +00:00
tags:
- .NET
- AI Agents
- Cloud Infrastructure
- Data Engineering
- Developer Productivity
- Kubernetes
- LLM Benchmarking
- Open Source
- Platform Automation
- Semantic Kernel
- Supply Chain Security
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
external_url: /all/roundups/Weekly-AI-and-Tech-News-Roundup-2025-09-01
---
Welcome to this week’s technology roundup, where updated AI capabilities join improvements in developer tools, cloud infrastructure, and security. A key area is the ongoing integration of AI agents into developer workflows—GitHub Copilot now enables model selection, multi-agent orchestration, tailored automation, and added security options across platforms. Azure introduces new features for Kubernetes networking, service group management, and hybrid identity, helping organizations simplify operations and meet compliance benchmarks.

DevOps and security stories this week include the move toward autonomous agents, smarter incident handling, and new solutions for supply chain security—including analysis of AI-powered open source threats. Detailed benchmarking for large language models, improved observability tools, and open platform releases underscore efforts to expand developer opportunities and enable transparent, stable workflows. These articles offer practical recommendations and up-to-date trends for engineers updating development environments, designing secure pipelines, or managing resources across hybrid systems.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot in Visual Studio: Updates, Models, and Agentic Workflows](#github-copilot-in-visual-studio-updates-models-and-agentic-workflows)
  - [Model Flexibility and Agentic Infrastructure: New AI Models, MCP, and Customization](#model-flexibility-and-agentic-infrastructure-new-ai-models-mcp-and-customization)
  - [Copilot as an Agent: Automated Coding, Code Review, and Collaborative Features](#copilot-as-an-agent-automated-coding-code-review-and-collaborative-features)
  - [Security, Reliability, and Safe Adoption](#security-reliability-and-safe-adoption)
  - [Copilot’s Model Architecture, Developer Experience, and Practical Guides](#copilots-model-architecture-developer-experience-and-practical-guides)
  - [Copilot Integrations and Developer Event Highlights](#copilot-integrations-and-developer-event-highlights)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Azure AI Foundry: Multi-Agent Orchestration, RAG, and API Developments](#azure-ai-foundry-multi-agent-orchestration-rag-and-api-developments)
  - [Semantic Kernel: Security, Template Updates, and Azure Integration Changes](#semantic-kernel-security-template-updates-and-azure-integration-changes)
  - [Agentic Protocols and Communication: MCP, A2A, NLWeb](#agentic-protocols-and-communication-mcp-a2a-nlweb)
  - [Advanced Search and Security: GraphRAG and Shadow AI Management](#advanced-search-and-security-graphrag-and-shadow-ai-management)
  - [Agent Observability, Cost Management, and HR Automation in the Enterprise](#agent-observability-cost-management-and-hr-automation-in-the-enterprise)
  - [Developer Workflows: Tutorials, Email Agents, and Open Source Trends](#developer-workflows-tutorials-email-agents-and-open-source-trends)
- [ML](#ml)
  - [Llama 3.1 8B and DeepSeek R1: Azure GPU Inference Analysis](#llama-31-8b-and-deepseek-r1-azure-gpu-inference-analysis)
  - [Productivity and Monitoring Advances in Microsoft Fabric](#productivity-and-monitoring-advances-in-microsoft-fabric)
  - [Practical Fabric Data Engineering: Materialized Lake Views, Community Best Practices](#practical-fabric-data-engineering-materialized-lake-views-community-best-practices)
- [Azure](#azure)
  - [Application Gateway and Container Networking in Azure Kubernetes Service](#application-gateway-and-container-networking-in-azure-kubernetes-service)
  - [Microsoft Fabric: Data Engineering, Analytics, and Machine Learning Enhancements](#microsoft-fabric-data-engineering-analytics-and-machine-learning-enhancements)
  - [Azure Logic Apps: Deployment, Hybrid Integration, and Automated Workflows](#azure-logic-apps-deployment-hybrid-integration-and-automated-workflows)
  - [Azure Service Groups: Streamlined Resource Organization](#azure-service-groups-streamlined-resource-organization)
  - [Secure Configuration and Access Management across Hybrid Estates](#secure-configuration-and-access-management-across-hybrid-estates)
  - [Azure VMware Solution: Expanded Regions, Storage, and Compliance](#azure-vmware-solution-expanded-regions-storage-and-compliance)
  - [Organizing, Monitoring, and Optimizing Azure Resource Usage and Costs](#organizing-monitoring-and-optimizing-azure-resource-usage-and-costs)
  - [Service Mesh and Advanced Architecture Patterns in AKS](#service-mesh-and-advanced-architecture-patterns-in-aks)
  - [Developer Tutorials and Integration Scenarios](#developer-tutorials-and-integration-scenarios)
  - [AI, GPU, and Scientific Workloads on Azure](#ai-gpu-and-scientific-workloads-on-azure)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET and C# Language and Platform Enhancements](#net-and-c-language-and-platform-enhancements)
  - [Open Sourcing Windows Subsystem for Linux (WSL)](#open-sourcing-windows-subsystem-for-linux-wsl)
  - [Developer Experience and Workflow Tools](#developer-experience-and-workflow-tools)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [AI-Powered Automation and Autonomous Agents](#ai-powered-automation-and-autonomous-agents)
  - [Architectural Governance, Patterns, and Compliance](#architectural-governance-patterns-and-compliance)
  - [Developer Platform Updates and Workflow Automation](#developer-platform-updates-and-workflow-automation)
  - [DevOps Tooling, Trends, and Team Practices](#devops-tooling-trends-and-team-practices)
  - [DevOps Platform Reliability and Security Incidents](#devops-platform-reliability-and-security-incidents)
- [Security](#security)
  - [Modern Supply Chain Threats and the Role of AI](#modern-supply-chain-threats-and-the-role-of-ai)
  - [GitHub Security Ecosystem: Releases, Secret Scanning, and Risk Assessment](#github-security-ecosystem-releases-secret-scanning-and-risk-assessment)
  - [Cloud Infrastructure and Platform Security Enhancements](#cloud-infrastructure-and-platform-security-enhancements)
  - [Securing the Next Generation: AI Agents and Cryptographic Identity](#securing-the-next-generation-ai-agents-and-cryptographic-identity)
  - [Automated Vulnerability Remediation in Microsoft DevOps Workflows](#automated-vulnerability-remediation-in-microsoft-devops-workflows)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week, GitHub Copilot received a series of updates, expanding how developers can use AI throughout their workflow. The enhancements include more options for model selection, agent management, automation tools for enterprise workflows, and new security features. These releases cover Visual Studio 2022, VS Code, JetBrains IDEs, and macOS/web environments—and bring additional AI models like GPT-5 and Grok Code Fast 1. A focus on security introduces prompt injection checks and improved defense strategies. Documentation and new walkthroughs provide clear steps for developers adopting these tools.

### GitHub Copilot in Visual Studio: Updates, Models, and Agentic Workflows

Visual Studio 2022 received its August 2025 update with Copilot positioned for deeper integration with Microsoft’s developer platforms. MCP is now a standard feature, supporting secure, custom AI workflows via `.mcp.json` files. Users have access to one-click MCP server installations, expanded model selection, and more API choices (OpenAI, Google, Anthropic). Copilot Chat adds snippet retrieval, history-based context, and smarter search—all improving debugging and collaborative workflows. Upcoming features previewed for September include AI Profiler and Debugger Agents, faster model switching, and updated policy controls. These changes help Visual Studio serve as a central platform for building customized AI-powered workflows.

- [August 2025 Update: GitHub Copilot Advancements in Visual Studio 2022](https://github.blog/changelog/2025-08-28-github-copilot-in-visual-studio-august-update)
- [Visual Studio 2022 August 2025 Update: GPT-5, MCP Integration, Copilot Enhancements, and Improved Debugging](https://devblogs.microsoft.com/visualstudio/the-visual-studio-august-update-is-here-smarter-ai-better-debugging-and-more-control/)
- [Roadmap for AI and GitHub Copilot in Visual Studio: September Update](https://devblogs.microsoft.com/visualstudio/roadmap-for-ai-in-visual-studio-september/)
- [GitHub Copilot for Azure Public Preview Launched in Visual Studio 2022 with MCP](https://devblogs.microsoft.com/visualstudio/github-copilot-for-azure-preview-launches-in-visual-studio-2022-with-azure-mcp-support/)

### Model Flexibility and Agentic Infrastructure: New AI Models, MCP, and Customization

Building on previous multi-model support, Copilot’s latest preview supports GPT-5 Mini and Grok Code Fast 1, giving users more backend options—including choices like Gemini 2.5 Pro. Organizations can now set enterprise-wide model defaults, and new MCP features such as AGENTS.md enable per-project instructions. Tutorials offer guidance on adapting workflows for monorepos or microservices, simplifying the setup of specialized AI agents and supporting more robust development environments.

- [OpenAI GPT-5 Mini Available in GitHub Copilot for Visual Studio and More](https://github.blog/changelog/2025-08-28-openai-gpt-5-mini-is-now-available-in-public-preview-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Public Preview: Grok Code Fast 1 Model Now Available in GitHub Copilot](https://github.blog/changelog/2025-08-26-grok-code-fast-1-is-rolling-out-in-public-preview-for-github-copilot)
- [Copilot Coding Agent Adds AGENTS.md Custom Instruction Support](https://github.blog/changelog/2025-08-28-copilot-coding-agent-now-supports-agents-md-custom-instructions)
- [Announcing Awesome Copilot MCP Server: Customizing GitHub Copilot Like a Pro](https://devblogs.microsoft.com/blog/announcing-awesome-copilot-mcp-server)
- [MCP Servers in VS Code and GitHub Copilot](/ai/videos/mcp-servers-in-vs-code-and-github-copilot)
- [Installing the GitHub MCP Server in Visual Studio Code](/ai/videos/installing-the-github-mcp-server-in-visual-studio-code)

### Copilot as an Agent: Automated Coding, Code Review, and Collaborative Features

Copilot’s functionality as a coding agent extends to GitHub Enterprise Cloud, now supporting data residency policies and improved security controls for business automation. The Agents Panel is expanding, Raycast integration adds desktop productivity options, and code review enhancements in Xcode give administrators new controls. Sub-issue management in Copilot Chat introduces a structured framework for conversation-based agile planning, supporting recent improvements in collaborative work.

- [Copilot Coding Agent Now in GitHub Enterprise Cloud with Data Residency](https://github.blog/changelog/2025-08-27-copilot-coding-agent-is-now-available-in-github-enterprise-cloud-with-data-residency)
- [The Download: GitHub Copilot Agents, Actions Security, and Vite's npm Milestone](/ai/videos/the-download-github-copilot-agents-actions-security-and-vites-npm-milestone)
- [Start and Track GitHub Copilot Coding Agent Tasks from Raycast](https://github.blog/changelog/2025-08-28-start-and-track-copilot-coding-agent-tasks-from-raycast)
- [Copilot Code Review: Now Available in Xcode and Enterprise Admin Controls](https://github.blog/changelog/2025-08-27-copilot-code-review-generally-available-in-xcode-and-new-admin-control)
- [Managing Sub-Issues with GitHub Copilot: Public Preview Update](https://github.blog/changelog/2025-08-27-create-sub-issues-with-copilot-in-public-preview)

### Security, Reliability, and Safe Adoption

Security improvements include prompt injection threat assessments for Copilot Chat in VS Code, mitigations based on workspace trust, agent transparency, and domain controls. Building on earlier improvements in secret scanning and container development, the coverage illustrates a layered approach to security. One engineering study reports Copilot accelerating secret token validation and pull request creation, improving coverage and offering tangible results for secure code practices.

- [Safeguarding VS Code Against Prompt Injections: Securing GitHub Copilot Chat](https://github.blog/security/vulnerability-research/safeguarding-vs-code-against-prompt-injections/)
- [How GitHub Copilot Accelerated Secret Protection Engineering](https://github.blog/ai-and-ml/github-copilot/how-we-accelerated-secret-protection-engineering-with-copilot/)

### Copilot’s Model Architecture, Developer Experience, and Practical Guides

Copilot now gives Pro+, Business, and Enterprise users the ability to choose between models such as GPT-4.1, Claude, and Gemini. The update includes new features for code completion, quick instruction generation, and expanded shortcuts, all supporting everyday development tasks. Practical guides and resources help teams integrate Copilot responsibly, with examples for onboarding, day-to-day prompts, and scaling best practices.

- [Inside GitHub Copilot: The AI Models Behind Your Coding Assistant](https://github.blog/ai-and-ml/github-copilot/under-the-hood-exploring-the-ai-models-powering-github-copilot/)
- [GitHub Copilot Code Completion Now Powered by GPT-4.1 Model](https://github.blog/changelog/2025-08-27-copilot-code-completion-now-uses-the-gpt-4-1-copilot-model)
- [Create GitHub Copilot Instructions in Just One Click](/ai/videos/create-github-copilot-instructions-in-just-one-click)
- [Mastering GitHub Copilot: Tips, Shortcuts, and Prompts That Work](https://dellenny.com/mastering-github-copilot-tips-shortcuts-and-prompts-that-work/)
- [How to Use GitHub Copilot on github.com: A Power User’s Guide](https://github.blog/ai-and-ml/github-copilot/how-to-use-github-copilot-on-github-com-a-power-users-guide/)
- [Boost Code Quality Fast with GitHub Copilot Edit Mode](https://cooknwithcopilot.com/blog/use-edit-mode-for-quick-targeted-improvements.html)
- [How to Automate Code Reviews and Testing with GitHub Copilot](/ai/videos/how-to-automate-code-reviews-and-testing-with-github-copilot)
- [Managing Your GitHub Enterprise: Policies, Copilot, and Security Settings](/ai/videos/managing-your-github-enterprise-policies-copilot-and-security-settings)
- [Using GitHub Copilot to Teach Programming: A New Approach for Educators](https://dellenny.com/using-github-copilot-to-teach-programming-a-new-approach-for-educators/)

### Copilot Integrations and Developer Event Highlights

Broader public previews rolled out for Copilot integrations in Visual Studio 2022, JetBrains IDEs, and expanded MCP support in VS Code. JetBrains gets Next Edit Suggestions (NES); VS Code launches Spec Kit for specification-driven development, continuing efforts in workflow automation and collaboration. VS Code Dev Days offer educational events for practitioners, building on previous GitHub Universe gatherings to promote skill sharing and community learning.

- [Next Edit Suggestions (NES) with GitHub Copilot for JetBrains IDEs: Public Preview](https://github.blog/changelog/2025-08-29-copilots-next-edit-suggestion-nes-in-public-preview-in-jetbrains)
- [Introducing Spec Kit for Spec-Driven Development in VS Code](/ai/videos/introducing-spec-kit-for-spec-driven-development-in-vs-code)
- [VS Code Dev Days: Explore AI-Assisted Development with GitHub Copilot](https://code.visualstudio.com/blogs/2025/08/27/vscode-dev-days)

### Other GitHub Copilot News

Workflows remain a main focus, with fresh guides showing how to enable full agentic cycles and use advanced debugging features—expanding on coverage for agent scripting and MCP customization. The .NET AI Community Standup and a review of MCP comparisons provide additional insights for hybrid and enterprise Copilot implementation.

- [End-to-End Agentic Development with GitHub Copilot: A Developer Workflow Demo](/ai/videos/end-to-end-agentic-development-with-github-copilot-a-developer-workflow-demo)
- [Automate Debugging with the Playwright MCP Server and GitHub Copilot](/ai/videos/automate-debugging-with-the-playwright-mcp-server-and-github-copilot)
- [.NET AI Community Standup: AI Tools Every .NET Dev Needs](/ai/videos/net-ai-community-standup-ai-tools-every-net-dev-needs)

## AI

Recent AI developments emphasize the evolution of multi-agent frameworks, improved retrieval workflows, enhanced security, and better cost controls, particularly within Azure and the open source community. The updates include new APIs, orchestration models, guides for enterprise adoption, and real-world experiences dealing with shadow AI and developer upskilling.

### Azure AI Foundry: Multi-Agent Orchestration, RAG, and API Developments

Azure AI Foundry released upgraded tools for orchestrating multi-agent systems, building on its modular agent support. Enhanced retrieval, analytics, and policy integrations connect with previous guidance for real-world production deployments. New RAG walkthroughs and the public release of the Responses API help streamline agent orchestration, making large-scale deployments more approachable and integrating with platforms like Semantic Kernel and AutoGen. Freeform tool calling with GPT-5 enables flexible automation for generating developer artifacts.

- [Multi-Agent Orchestration with Azure AI Foundry: From Idea to Production](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/multi-agent-orchestration-with-azure-ai-foundry-from-idea-to/ba-p/4449925)
- [Retrieval-Augmented Generation (RAG) in Azure AI: A Step-by-Step Guide](https://dellenny.com/retrieval-augmented-generation-rag-in-azure-ai-a-step-by-step-guide/)
- [General Availability of the Responses API in Azure AI Foundry](https://techcommunity.microsoft.com/t5/ai-azure-ai-services-blog/the-responses-api-in-azure-ai-foundry-is-now-generally-available/ba-p/4446567)
- [Freeform Tool Calling with GPT-5 in Azure AI Foundry](/ai/videos/freeform-tool-calling-with-gpt-5-in-azure-ai-foundry)

### Semantic Kernel: Security, Template Updates, and Azure Integration Changes

Semantic Kernel Python 1.36.0 now requires explicit credential configuration for Azure authentication—a shift to stronger credential management for compliance and reliability. New encoding rules for template arguments bring added runtime protection, strengthening prompt engineering security and defending against injection risks.

- [Mandatory Explicit Azure Authentication in Semantic Kernel Python 1.36.0](https://devblogs.microsoft.com/semantic-kernel/azure-authentication-changes-in-semantic-kernel-python/)
- [Stricter Encoding Rules for Template Arguments in Semantic Kernel](https://devblogs.microsoft.com/semantic-kernel/encoding-changes-for-template-arguments-in-semantic-kernel/)

### Agentic Protocols and Communication: MCP, A2A, NLWeb

Tutorials explain how to use MCP, A2A, and NLWeb agentic communication for improved context management. Analysis of API limitations continues the discussion of context-aware, intent-driven automation and its impact on lifecycle, versioning, and security—in line with recent best practices.

- [Using Agentic Protocols (MCP, A2A, and NLWeb)](/ai/videos/using-agentic-protocols-mcp-a2a-and-nlweb)
- [Why APIs Alone Won’t Cut It in the AI Era](https://devops.com/why-apis-alone-wont-cut-it-in-the-ai-era/?utm_source=rss&utm_medium=rss&utm_campaign=why-apis-alone-wont-cut-it-in-the-ai-era)

### Advanced Search and Security: GraphRAG and Shadow AI Management

GraphRAG combines RAG and semantic graph search, supporting richer enterprise AI search and analytics and deepening security analysis. Guidance on managing shadow AI risk builds on compliance discussions, offering steps for monitoring and regulatory alignment.

- [Exploring GraphRAG: AI-Powered Graph Search for Security Data Analysis](/ai/videos/exploring-graphrag-ai-powered-graph-search-for-security-data-analysis)
- [Staying on Top of Shadow AI](https://devops.com/staying-on-top-of-shadow-ai/?utm_source=rss&utm_medium=rss&utm_campaign=staying-on-top-of-shadow-ai)

### Agent Observability, Cost Management, and HR Automation in the Enterprise

Agent observability and benchmarking resources provide practical recommendations for reliability, cost tracking, and compliance. Tutorials help teams manage AI project budgets and operational visibility. A case study details how Chemist Warehouse uses Azure AI Foundry to automate HR tasks, continuing documentation of AI adoption in specific business sectors.

- [Agent Factory: Top 5 Agent Observability Best Practices for Reliable AI](https://azure.microsoft.com/en-us/blog/agent-factory-top-5-agent-observability-best-practices-for-reliable-ai/)
- [Why Did the Cost of My AI Agent Exceed the Set Budget?](/ai/videos/why-did-the-cost-of-my-ai-agent-exceed-the-set-budget)
- [Context Window: Answering 3 Developer Questions to Save on AI Costs](/ai/videos/context-window-answering-3-developer-questions-to-save-on-ai-costs)
- [How Chemist Warehouse Uses Azure AI Foundry for HR Transformation](https://news.microsoft.com/source/asia/features/a-digital-colleague-how-chemist-warehouse-and-insurgence-ai-are-rewriting-the-hr-playbook/)

### Developer Workflows: Tutorials, Email Agents, and Open Source Trends

New tutorials build on recent agent setup guidance, demonstrating how to create production-ready designs including AI-powered email agents using Copilot Studio and Azure Communication Services. Discussions highlight the benefits of open source and project-based learning, emphasizing curiosity, skill development, and hands-on exploration for tech careers.

- [Build an AI Email Agent with Microsoft Copilot Studio and Azure Communication Services](https://techcommunity.microsoft.com/t5/azure-communication-services/build-your-ai-email-agent-with-microsoft-copilot-studio/ba-p/4448724)
- [Rediscovering Joy in Learning: Jason Lengstorf on AI, Open Source, and Developer Growth](https://github.blog/developer-skills/career-growth/rediscovering-joy-in-learning-jason-lengstorf-on-the-state-of-development/)
- [AI, Project-Based Learning, and the Future of Tech Careers](/ai/videos/ai-project-based-learning-and-the-future-of-tech-careers)

## ML

Machine learning updates focus on better LLM inference performance, improvements to cloud productivity, and clear guidance for teams deploying large-scale solutions. Insights include Azure GPU benchmarking for model throughput, real-world diagnostics, and new analytics features in Microsoft Fabric.

### Llama 3.1 8B and DeepSeek R1: Azure GPU Inference Analysis

Following earlier coverage on LLM pretraining optimizations, this week’s benchmarks examine Meta’s Llama 3.1 8B and DeepSeek R1 using Azure ND-H100-v5 GPUs and vLLM. The analysis shows how optimizations like quantization and parallel processing yield throughput improvements of over 38%, and includes comparisons across Azure ND-series hardware for speed, cost, and scalability. DeepSeek R1 is effective for complex tasks, but slower and less cost-efficient than lighter models—helping teams choose the right model for their needs.

- [Benchmarking Llama 3.1 8B AI Inference on Azure ND-H100-v5 with vLLM](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-of-llama-3-1-8b-ai-inference-using-vllm-on-nd-h100/ba-p/4448355)
- [Benchmarking Llama 3.1 8B Inference with vLLM on Azure GPU and CPU VMs](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/inference-performance-of-llama-3-1-8b-using-vllm-across-various/ba-p/4448420)
- [Performance Analysis: DeepSeek R1 Inference with vLLM on Azure ND-H100-v5](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-analysis-of-deepseek-r1-ai-inference-using-vllm-on/ba-p/4449351)

### Productivity and Monitoring Advances in Microsoft Fabric

Microsoft Fabric now offers Fabric Notebooks with direct Pandas DataFrame handling via Apache Arrow, boosting workflow speed and memory efficiency. Monitoring and troubleshooting advances include improved mapping, granular log filtering, and execution snapshots for Spark workloads. The new JobInsight library provides diagnostics and historical analysis, automating insight generation for analytics pipelines.

- [Enhancing Fabric Notebooks: Native Pandas DataFrame Support in User Data Functions](https://blog.fabric.microsoft.com/en-US/blog/now-in-fabric-notebook-udf-integration-with-native-support-for-pandas-dataframes-and-series-via-apache-arrow/)
- [Enhanced Monitoring for Spark High Concurrency Workloads in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/enhanced-monitoring-for-spark-high-concurrency-workloads-in-microsoft-fabric/)
- [Gain Deeper Insights into Spark Jobs with JobInsight in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/gain-deeper-insights-into-spark-jobs-with-jobinsight-in-microsoft-fabric/)

### Practical Fabric Data Engineering: Materialized Lake Views, Community Best Practices

Guides showcase effective Fabric pipeline operations, spotlighting Materialized Lake Views for syncing Azure SQL to OneLake and detailing layered data transformations. Tutorials from Microsoft MVPs and Super Users cover dynamic masking, Power BI, REST admin, Pandas analysis, and efficient transformation patterns, with tips for troubleshooting and certification.

- [Mastering Declarative Data Transformations with Materialized Lake Views](https://blog.fabric.microsoft.com/en-US/blog/mastering-declarative-data-transformations-with-materialized-lake-views/)
- [Fabric Influencers Spotlight: August 2025 Edition](https://blog.fabric.microsoft.com/en-US/blog/fabric-influencers-spotlight-august-2025/)

## Azure

Azure updates this week focus on infrastructure, developer tools, practical cloud deployment, and operational improvements. Highlights include advances in Kubernetes networking, hybrid management, automated data pipeline tooling, and new resource organization features. Several GA releases illustrate Azure’s commitment to scalable and secure operations.

### Application Gateway and Container Networking in Azure Kubernetes Service

Azure CNI Overlay and AGIC for AKS (now GA) enable Kubernetes clusters to use overlay-assigned pod IPs for scalable ingress, sidestepping VNet IP constraints. These upgrades facilitate streamlined operations and policy enforcement, integrating with NSGs and Azure Firewall. Deployment guides, versioning, and subnet design advice help teams move beyond kubenet, enhancing reliability and security for containerized and AI workloads.

- [Container Networking with Azure Application Gateway for Containers (AGC): Overlay vs. Flat AKS](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/container-networking-with-azure-application-gateway-for/ba-p/4449941)
- [General Availability: Azure CNI Overlay for Application Gateway for Containers and AGIC](https://techcommunity.microsoft.com/t5/azure-networking-blog/azure-cni-overlay-for-application-gateway-for-containers-and/ba-p/4449350)
- [Azure Weekly Update – 29th August 2025](/ai/videos/azure-weekly-update-29th-august-2025)

### Microsoft Fabric: Data Engineering, Analytics, and Machine Learning Enhancements

Fabric’s August summary details new automation and analytics features—including a redesigned pipeline interface, OpenAPI REST specs, cross-tenant DevOps, Spark scaling, and diagnostics updates. Real-time notebooks, DataFrame support, secure ML endpoints, transparency tools, SQL enhancements, and enhanced Data Factory UI provide developers with refined control and automation, continuing recent progress in data pipeline design and workspace security.

- [August 2025 Microsoft Fabric Feature Summary: Data Engineering, ML, and Platform Enhancements](https://blog.fabric.microsoft.com/en-US/blog/august-2025-fabric-feature-summary/)
- [Copy Job Activity Now in Preview for Microsoft Fabric Data Factory Pipelines](https://blog.fabric.microsoft.com/en-US/blog/now-in-public-preview-copy-job-activity-in-pipelines/)

### Azure Logic Apps: Deployment, Hybrid Integration, and Automated Workflows

Logic Apps Standard now support automated deployment from source control using Deployment Center and VS Code, with scripting and rollback functionality. Step-by-step guides show how to run Logic Apps on K3s for edge and hybrid integration, and how to automate log analytics workflows utilizing Azure OpenAI for reporting. These resources increase Azure’s flexibility for hybrid and automated solutions.

- [Automating Azure Logic Apps Deployments with Deployment Center and VS Code](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-setup-cd-in-azure-logic-apps-standard-with-deployment/ba-p/4449013)
- [Deploying Hybrid Azure Logic Apps on K3s for Lightweight, On-Premises Integration](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/hybrid-logic-apps-deployment-on-rancher-k3s-kubernetes-cluster/ba-p/4448557)
- [Automate Log Analytics Workflows with Azure Logic Apps and Azure OpenAI](https://techcommunity.microsoft.com/t5/azure-observability-blog/automate-your-log-analytics-workflows-with-ai-and-logic-apps/ba-p/4442803)

### Azure Service Groups: Streamlined Resource Organization

Azure Service Groups offer hierarchically organized resources—an upgrade from traditional resource groups—enabling better management, monitoring, and permission assignment. Setup instructions, best practice advice, and permission examples help architects organize large or multi-team environments with granular governance.

- [Azure Service Groups: Flexible Resource Organization Explained](/azure/videos/azure-service-groups-flexible-resource-organization-explained)
- [Azure Service Groups Quick Overview](/azure/videos/azure-service-groups-quick-overview)

### Secure Configuration and Access Management across Hybrid Estates

Azure Machine Configuration packages now support System Assigned Identity as GA, removing reliance on SAS tokens and manual identity management. Managed identities (with RBAC) simplify secure onboarding, compliance, and configuration package access in Blob Storage. Guides include PowerShell automation and role assignment procedures.

- [System-Assigned Identity-based Access for Azure Machine Configuration Packages Now Generally Available](https://techcommunity.microsoft.com/t5/azure-governance-and-management/system-assigned-identity-based-access-for-machine-configuration/ba-p/4446603)

### Azure VMware Solution: Expanded Regions, Storage, and Compliance

Azure VMware Solution is live in 35+ regions, adding VCF private clouds, portable subscriptions, DISA IL5 compliance, and NetApp Files/Elastic SAN integration. Expanded regions, improved migration, flexible billing, and quick learning options support both regulated and enterprise environments.

- [Announcing More Azure VMware Solution Enhancements](https://techcommunity.microsoft.com/t5/azure-migration-and/announcing-more-azure-vmware-solution-enhancements/ba-p/4447103)

### Organizing, Monitoring, and Optimizing Azure Resource Usage and Costs

Coverage includes Azure RHEL billing migration to vCPU pricing and relevant cost tracking guidance. Tutorials for backup auditing show how to identify and remove orphaned backups, reducing storage expenditures. Additional content reviews Capacity Reservations vs. Reserved Instances and reliability planning (including Availability Zones and gateway options).

- [Azure Red Hat Enterprise Linux Billing Meter ID Migration: What You Need to Know](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/red-hat-enterprise-linux-billing-meter-id-updates-on-azure/ba-p/4449348)
- [Mine Your Azure Backup Data to Identify Hidden Costs](https://techcommunity.microsoft.com/t5/azure/mine-your-azure-backup-data-it-could-save-you/m-p/4448003#M22143)
- [Designing for Certainty: How Azure Capacity Reservations Safeguard Mission‑Critical Workloads](https://techcommunity.microsoft.com/t5/azure-governance-and-management/designing-for-certainty-how-azure-capacity-reservations/m-p/4447901#M347)
- [Azure Top 3 Reliability Actions](/azure/videos/azure-top-3-reliability-actions)

### Service Mesh and Advanced Architecture Patterns in AKS

A service mesh guide covers best practices for Istio, OSM, and Azure integrations. Topics include discovery, traffic routing, mTLS/cert management, RBAC, monitoring, CI/CD policy automation, and deployment approaches for robust microservices management.

- [Service Mesh Architecture Pattern in Azure: Managing Microservices Communication, Security, and Observability](https://dellenny.com/service-mesh-architecture-pattern-in-azure-handling-service-to-service-communication-security-and-observability/)

### Developer Tutorials and Integration Scenarios

Articles demonstrate Azure Static Web Apps with Azure Functions for dynamic image workflows (including CLI and troubleshooting tips) and secure simulation data management for Siemens Teamcenter SPDM on Azure CycleCloud with Slurm, showing how to deploy scalable HPC solutions.

- [Connecting Azure Static Web Apps with Azure Functions for Dynamic Images](https://techcommunity.microsoft.com/t5/apps-on-azure/what-s-the-secret-sauce-for-getting-functions-api-to-work-with/m-p/4448430#M1359)
- [Implementing Siemens Teamcenter Simulation Process Data Management on Azure CycleCloud with Slurm](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/teamcenter-simulation-process-data-management-architecture-on/ba-p/4449316)

### AI, GPU, and Scientific Workloads on Azure

A review of Azure’s AI infrastructure highlights how NVIDIA GPUs support healthcare analytics and content creation, using AKS pipelines, BioNeMo, and compliance tools for cloud-based AI and HPC workloads.

- [Transforming Scientific Discovery with Microsoft Azure and NVIDIA](https://azure.microsoft.com/en-us/blog/transforming-scientific-discovery-with-microsoft-azure-and-nvidia/)

### Other Azure News

Azure Communication Services is generally available for Azure Government, offering secure chat and video functionality for public sector needs.

- [Azure Communication Services Now Generally Available in Azure Government](https://techcommunity.microsoft.com/t5/azure-communication-services/azure-communication-services-is-now-generally-available-in-azure/ba-p/4448034)

Azure clarified how provider-managed subscriptions work to help organizations delegate management but maintain cost visibility, with tips on governance and RBAC.

- [Provider-Managed Azure Subscriptions: Cost Control and Commitment Clarity](https://techcommunity.microsoft.com/t5/finops-blog/provider-managed-azure-subscriptions-cost-control-and-commitment/ba-p/4448688)

## Coding

Updates this week for coding include new features in .NET and C#, diagnostic tools, open sourcing of Windows Subsystem for Linux, and practical workflow guides. Microsoft continues its focus on open source and improving developer experience with new releases and troubleshooting content.

### .NET and C# Language and Platform Enhancements

Nick Chapsas previews Discriminated Unions for C# 15/16, demonstrating better type safety and simplified code patterns akin to F#, TypeScript, and Rust. MauiReactor provides MVU architecture options for .NET MAUI UI development. EFCore.Visualizer lets Visual Studio users analyze Entity Framework Core query plans inside the IDE, continuing improvements in .NET tooling.

- [Exploring Discriminated Unions Coming to C# 15 and 16](/coding/videos/exploring-discriminated-unions-coming-to-c-15-and-16)
- [MauiReactor: Introducing the MVU Pattern for .NET MAUI](https://devblogs.microsoft.com/dotnet/mauireactor-mvu-for-dotnet-maui/)
- [EFCore.Visualizer: Analyze Entity Framework Core Query Plans in Visual Studio](https://devblogs.microsoft.com/dotnet/ef-core-visualizer-view-entity-framework-core-query-plan-inside-visual-studio/)

### Open Sourcing Windows Subsystem for Linux (WSL)

Microsoft has published the source code for WSL internals—including VM startup, filesystem mounting, and GPU handling—on GitHub, boosting community involvement and transparency. Developers have direct access for troubleshooting, customize workflow, and can contribute ideas for enhancements or fixes. Official resources welcome community collaboration and learning.

- [You open sourced WSL. What does that mean?](/coding/videos/you-open-sourced-wsl-what-does-that-mean)
- [Explaining the Open Sourcing of Windows Subsystem for Linux (WSL)](/coding/videos/explaining-the-open-sourcing-of-windows-subsystem-for-linux-wsl)

### Developer Experience and Workflow Tools

Aspire CLI for .NET streamlines app configuration, integrates cloud dependencies, and supports easy deployment to Azure, Docker Compose, or Kubernetes. Step-by-step guides for installation and commands aim to make distributed development more approachable. VS Code’s August iteration plan previews new terminal enhancements, agent features, and automation, opening discussion for community feedback.

- [Getting Started with the Aspire CLI](https://devblogs.microsoft.com/dotnet/getting-started-with-the-aspire-cli/)
- [Upcoming Features and Terminal Improvements in Visual Studio Code: August Iteration](/coding/videos/upcoming-features-and-terminal-improvements-in-visual-studio-code-august-iteration)

### Other Coding News

A troubleshooting guide for .NET Core on Alpine Linux explains fixes for native asset loading errors, including environment variable recommendations. A SharePoint branding resource provides practical steps for logo setup, themes, templates, and homepage configuration for consistent organizational branding.

- [Solving Native Library Loading Issues for .NET Core on Alpine Linux](https://andrewlock.net/fixing-an-old-dotnet-core-native-library-loading-issue-on-alpine/)
- [Branding Your SharePoint Site for Your Organization](https://dellenny.com/branding-your-sharepoint-site-for-your-organization/)

## DevOps

This week in DevOps, teams focus on advanced automation powered by AI, improvements in open-source governance, updated platform features, and practical insights on reliability and workflow management.

### AI-Powered Automation and Autonomous Agents

Harness’s new AI DevOps platform automates pipeline creation, deployment, root-cause detection, and testing with natural language prompts and built-in privacy controls. System Initiative introduces autonomous agents that manage infrastructure via digital twins and natural language change proposals. These features build on recent progress in onboarding, permission management, and observability, emphasizing hands-on oversight by DevOps teams and confirming that AI is a complement rather than a replacement for engineers.

- [Harness AI-Powered DevOps Platform Launches to Automate Software Delivery](https://devops.com/harness-delivers-on-ai-promise-for-devops/?utm_source=rss&utm_medium=rss&utm_campaign=harness-delivers-on-ai-promise-for-devops)
- [System Initiative Introduces Autonomous AI Agents for Infrastructure Automation](https://devops.com/system-initiative-adds-ai-agents-to-infrastructure-automation-platform/?utm_source=rss&utm_medium=rss&utm_campaign=system-initiative-adds-ai-agents-to-infrastructure-automation-platform)
- [AI Agent Onboarding as a Core DevOps Responsibility](https://devops.com/ai-agent-onboarding-is-now-a-critical-devops-function/?utm_source=rss&utm_medium=rss&utm_campaign=ai-agent-onboarding-is-now-a-critical-devops-function)
- [Can AI Replace DevOps Engineers?](https://devops.com/can-ai-replace-devops-engineers-3/?utm_source=rss&utm_medium=rss&utm_campaign=can-ai-replace-devops-engineers-3)

### Architectural Governance, Patterns, and Compliance

Morgan Stanley’s open source CALM tools automate enterprise architecture governance with meta schemas, templates, and command-line utilities, which integrate CI/CD compliance checks. Broadcom’s VMware Cloud Foundation adds Argo CD, Ubuntu container support, and GPU/AI workload capabilities, simplifying orchestration and enterprise-grade compliance for cloud workloads.

- [Morgan Stanley Open Sources CALM: Architecture as Code for Enterprise DevOps](https://devops.com/morgan-stanley-open-sources-calm-the-architecture-as-code-solution-transforming-enterprise-devops/?utm_source=rss&utm_medium=rss&utm_campaign=morgan-stanley-open-sources-calm-the-architecture-as-code-solution-transforming-enterprise-devops)
- [Broadcom Expands VMware Cloud Foundation with Argo CD and Ubuntu Support](https://devops.com/broadcom-adds-argo-and-ubuntu-support-to-vmware-cloud-foundation/?utm_source=rss&utm_medium=rss&utm_campaign=broadcom-adds-argo-and-ubuntu-support-to-vmware-cloud-foundation)

### Developer Platform Updates and Workflow Automation

GitHub’s new Dependabot exclude-paths option provides finer control over automated pull request noise, plus improvements for template URLs and fine-grained Personal Access Token management. Walkthroughs support maintainers in scaling open source projects via models and Actions. Added repository management features (rulesets, dashboard, export options) and accessibility upgrades help teams simplify administration and improve accessibility.

- [Suppress Dependabot PRs in Specific Subdirectories with `exclude-paths`](https://github.blog/changelog/2025-08-26-dependabot-can-now-exclude-automatic-pull-requests-for-manifests-in-selected-subdirectories)
- [Template URLs for Fine-Grained PATs and Updated Permissions UI](https://github.blog/changelog/2025-08-26-template-urls-for-fine-grained-pats-and-updated-permissions-ui)
- [Automating Open Source Maintenance with GitHub Models and AI Workflows](https://github.blog/open-source/maintainers/how-github-models-can-help-open-source-maintainers-focus-on-what-matters/)
- [Improved GitHub Repository Creation, Ruleset, and Insights Features Released](https://github.blog/changelog/2025-08-26-improved-repository-creation-generally-available-plus-ruleset-insights-improvements)
- [Public Preview: Enhanced Home Dashboard with My Work and Feed Tabs](https://github.blog/changelog/2025-08-28-improvements-to-the-home-dashboard-available-in-public-preview)

### DevOps Tooling, Trends, and Team Practices

Growing use of modular automation frameworks such as GitHub Actions, Dagger, and Temporal enables developers to build efficient, event-driven workflows. Articles emphasize practices like improving team visibility, capacity management, and combining AI workflow automation with strong peer review and security. John Willis highlights the importance of building resilience and security into ongoing engineering work.

- [How Engineers are Automating More with Less: Trends in DevOps Tooling](https://devops.com/how-engineers-are-automating-more-with-less-trends-in-devops-tooling/?utm_source=rss&utm_medium=rss&utm_campaign=how-engineers-are-automating-more-with-less-trends-in-devops-tooling)
- [Bringing Order to Chaotic Software Engineering Workflows](https://devops.com/bringing-order-to-chaotic-software-engineering-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=bringing-order-to-chaotic-software-engineering-workflows)
- [John Willis: The True North of DevOps and DevSecOps](https://devops.com/john-willis-the-true-north-of-devops-and-devsecops/?utm_source=rss&utm_medium=rss&utm_campaign=john-willis-the-true-north-of-devops-and-devsecops)

### DevOps Platform Reliability and Security Incidents

A mid-year report finds a rise in service interruptions and outages for platforms including GitHub, Azure DevOps, GitLab, Bitbucket, and Jira, with Azure DevOps reporting 74 incidents and GitHub up by 58%. Ongoing security concerns on platforms such as GitLab and Jira show how CI/CD environments remain key targets and reinforce the importance of observability and backup strategies.

- [Surge in DevOps Platform Incidents: 2025 Mid-Year Analysis of GitHub, Azure DevOps, and Jira Disruptions](https://devops.com/devops-platforms-show-cracks-github-incidents-surge-58-azure-gitlab-and-jira-also-under-pressure/?utm_source=rss&utm_medium=rss&utm_campaign=devops-platforms-show-cracks-github-incidents-surge-58-azure-gitlab-and-jira-also-under-pressure)

## Security

Security coverage this week centers on defending the software supply chain, cloud hardening, agent identity controls, and updated developer tools for risk management. As attacks involving AI and regulatory changes become more frequent, teams face growing pressure to reinforce automated workflows and compliance.

### Modern Supply Chain Threats and the Role of AI

A new multi-stage attack targeted Nx and npm, using stolen tokens and compromised GitHub workflows to deploy malicious packages—with AI-driven reconnaissance marking the first public case of LLMs used for open source exploits. This incident affected over 5,500 repositories and triggered stronger 2FA requirements, Trusted Publisher policies, and workflow security. Upcoming EU regulations require machine-readable SBOMs and regular vulnerability disclosures by December 2027, prompting an increased focus on automating compliance checks and securing DevOps processes.

- [Malicious Nx Packages Used in Two Waves of Supply Chain Attack](https://devops.com/malicious-nx-packages-used-in-two-waves-of-supply-chain-attack/?utm_source=rss&utm_medium=rss&utm_campaign=malicious-nx-packages-used-in-two-waves-of-supply-chain-attack)
- [The EU’s Cyber Resilience Act: Redefining Secure Software Development](https://devops.com/the-eus-cyber-resilience-act-redefining-secure-software-development/?utm_source=rss&utm_medium=rss&utm_campaign=the-eus-cyber-resilience-act-redefining-secure-software-development)
- [AI Coding Assistants Bring Security and Licensing Challenges to Embedded Systems](https://devops.com/survey-surfaces-raft-of-ai-coding-issues-involving-embedded-systems/?utm_source=rss&utm_medium=rss&utm_campaign=survey-surfaces-raft-of-ai-coding-issues-involving-embedded-systems)
- [Coding at the Speed of AI: Innovation, Vulnerability, and the GenAI Paradox](https://devops.com/coding-at-the-speed-of-ai-innovation-vulnerability-and-the-genai-paradox/?utm_source=rss&utm_medium=rss&utm_campaign=coding-at-the-speed-of-ai-innovation-vulnerability-and-the-genai-paradox)

### GitHub Security Ecosystem: Releases, Secret Scanning, and Risk Assessment

GitHub previewed immutable releases with asset and tag locking, using Sigstore cryptographic attestations for asset validation. Secret scanning now adds validators for ten new types and launches a free Secret Risk Assessment tool for organizations, summarizing exposed tokens and guiding review and remediation. These tools expand coverage for credential leak detection and offer administrators new workflow protections.

- [GitHub Releases Introduce Immutability for Enhanced Supply Chain Security](https://github.blog/changelog/2025-08-26-releases-now-support-immutability-in-public-preview)
- [GitHub Secret Scanning Expands with New Validators and Validity Checks](https://github.blog/changelog/2025-08-26-secret-scanning-adds-10-new-validators-including-square-wakatime-and-yandex)
- [GitHub Introduces Free Secret Risk Assessment Tool for Organizations](https://github.blog/changelog/2025-08-26-the-secret-risk-assessment-is-generally-available)

### Cloud Infrastructure and Platform Security Enhancements

Azure improved platform security with Boost hardware isolation, integrated HSMs (FIPS 140-3), Caliptra silicon root-of-trust, and firmware Code Transparency Services. Confidential VMs and containers support compliant data-at-rest and in-use security. Microsoft’s ransomware report details hybrid attacks exploiting Entra ID and misconfigurations, with guidance for detection and cloud estate locks.

- [Protecting Azure Infrastructure: Defense-in-Depth from Silicon to Systems](https://azure.microsoft.com/en-us/blog/protecting-azure-infrastructure-from-silicon-to-systems/)
- [Storm-0501’s Evolving Cloud-Based Ransomware Tactics: Microsoft Analysis](https://www.microsoft.com/en-us/security/blog/2025/08/27/storm-0501s-evolving-techniques-lead-to-cloud-based-ransomware/)

### Securing the Next Generation: AI Agents and Cryptographic Identity

Best practices for AI agent security include using Entra Agent ID, RBAC, agent registries, and Defender/Purview analytics to manage prompt injection risk and lifecycle drift. Microsoft’s Crescent cryptographic library supports privacy-preserving digital identity using Groth16 SNARK, improving JWT and mobile credential privacy without major infrastructure changes.

- [Securing and Governing Autonomous AI Agents in the Enterprise](https://www.microsoft.com/en-us/security/blog/2025/08/26/securing-and-governing-the-rise-of-autonomous-agents/)
- [Introducing Crescent: Microsoft's Cryptographic Library for Privacy-Preserving Digital Identity](https://www.microsoft.com/en-us/research/blog/crescent-library-brings-privacy-to-digital-identity-systems/)

### Automated Vulnerability Remediation in Microsoft DevOps Workflows

Qwiet AI expands its support for Azure DevOps, Azure Boards, and GitHub, providing SARIF static analysis, policy integration, and secret management. The AutoFix engine automates risk inspection and patching, integrating remediation directly into developer workflows.

- [Qwiet AI Expands Microsoft DevOps and GitHub Integration for Code Vulnerability Remediation](https://devops.com/qwiet-ai-extends-microsoft-support-in-platform-for-fixing-vulnerabilities/?utm_source=rss&utm_medium=rss&utm_campaign=qwiet-ai-extends-microsoft-support-in-platform-for-fixing-vulnerabilities)

### Other Security News

ASP.NET 10 APIs now return HTTP 401 Unauthorized instead of HTTP 302 for unauthenticated requests, streamlining client-side error handling per REST standards.

- [ASP.NET Community Standup: Preventing Login Redirects for APIs](/coding/videos/aspnet-community-standup-preventing-login-redirects-for-apis)

A podcast with Kat Cosgrove examines common issues with vulnerability patching, container protection, and Kubernetes hardening, recommending daily automation practices for improved resilience.

- [Digging Into Security With Kat Cosgrove](https://www.arresteddevops.com/digging-into-security/)

A step-by-step guide for healthcare data compliance in Fabric shows how to set up Microsoft Purview DLP policies to detect PHI, automate data governance, and prepare for HIPAA audits.

- [Enabling Healthcare Compliance with Microsoft Purview DLP Policies in Fabric](https://blog.fabric.microsoft.com/en-US/blog/meet-your-healthcare-regulation-and-compliance-requirements-with-purview-data-loss-prevention-dlp-policies/)

For Windows 11, ransomware protection tips cover Defender Antivirus, Controlled Folder Access, app whitelisting, and backup setup for a secure developer environment.

- [How to Enable Ransomware Protection in Windows 11](https://dellenny.com/how-to-enable-ransomware-protection-in-windows-11/)
