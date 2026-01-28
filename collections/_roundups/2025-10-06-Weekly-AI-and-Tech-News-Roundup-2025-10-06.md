---
title: Agentic AI, Copilot Updates, and Platform Integration Drive Tech Ecosystem Developments
author: Tech Hub Team
date: 2025-10-06 09:00:00 +00:00
tags:
- AI Agents
- Automation
- Coding Agents
- Data Transformation
- Enterprise Cloud
- MCP
- Microsoft Fabric
- Multicloud
- VS
- Workflow Orchestration
section_names:
- ai
- github-copilot
- ml
- azure
- coding
- devops
- security
primary_section: all
---
Thanks for joining this week’s tech roundup, where AI, automation, and platform engineering increasingly intersect. The focus is on new agentic AI developments, including GitHub Copilot’s unified experience for desktop, terminal, and cloud. Improved agent workflows, the addition of models like Anthropic Claude Sonnet 4.5, and expanded Visual Studio/VS Code integration are changing how developers build and maintain software of all sizes. With features such as auto model selection, multimodal prompts, and extended analytics, AI-powered development and governance now reach broader audiences.

Microsoft’s public preview of Agent Framework and the wider adoption of Model Context Protocol support more robust multi-agent systems, making modular AI and process automation accessible across enterprise workflows. Updates to Azure and Microsoft Fabric deliver features for secure networking, automation, orchestration, multi-cloud connectors, and new open-source CLI tools. Developer communities benefit from improved DevOps automation, streamlined machine learning pipelines, and updated security—offering practical solutions for software delivery, quality, and supply chain protection. This week demonstrates consistent growth toward a connected development environment spanning coding, infrastructure, automation, and security.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot Integration in Visual Studio](#github-copilot-integration-in-visual-studio)
  - [GitHub Copilot CLI and Terminal Workflows](#github-copilot-cli-and-terminal-workflows)
  - [GitHub Copilot Coding Agent](#github-copilot-coding-agent)
  - [AI Model Selection and Integration in Copilot](#ai-model-selection-and-integration-in-copilot)
  - [GitHub Spark and Rapid Application Prototyping](#github-spark-and-rapid-application-prototyping)
  - [GitHub Copilot Advanced Documentation, Prompt Engineering, and Parallel Workflows](#github-copilot-advanced-documentation-prompt-engineering-and-parallel-workflows)
  - [GitHub Copilot Workflow Analytics and Registry Support](#github-copilot-workflow-analytics-and-registry-support)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Microsoft Agent Framework: Unified Agentic AI SDKs and Ecosystem Integrations](#microsoft-agent-framework-unified-agentic-ai-sdks-and-ecosystem-integrations)
  - [Model Context Protocol (MCP) and Secure AI Integrations in Microsoft Fabric](#model-context-protocol-mcp-and-secure-ai-integrations-in-microsoft-fabric)
  - [Grok 4: Advanced Reasoning Models in Azure AI Foundry](#grok-4-advanced-reasoning-models-in-azure-ai-foundry)
  - [Copilot, Voice, and Intelligent Data Tools: Workflow Automation and NLP Integration](#copilot-voice-and-intelligent-data-tools-workflow-automation-and-nlp-integration)
  - [Practical Guides for Agentic AI in Python and Enterprise Workflows](#practical-guides-for-agentic-ai-in-python-and-enterprise-workflows)
  - [AI Agents: Concepts, Architecture, and Developer Adoption](#ai-agents-concepts-architecture-and-developer-adoption)
  - [Retrieval Augmented Generation (RAG) and Document Processing Workflows](#retrieval-augmented-generation-rag-and-document-processing-workflows)
  - [Azure AI in Enterprise Workflows: Supply Chain & Scalable Cloud Infrastructure](#azure-ai-in-enterprise-workflows-supply-chain--scalable-cloud-infrastructure)
  - [AI Ethics and Security in Scientific and Enterprise Environments](#ai-ethics-and-security-in-scientific-and-enterprise-environments)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Dataflow Gen2 in Microsoft Fabric: Performance, Integration, and Developer Experience](#dataflow-gen2-in-microsoft-fabric-performance-integration-and-developer-experience)
  - [AI-Powered Data Transformation and Developer Tools](#ai-powered-data-transformation-and-developer-tools)
  - [Multitasking and Workflow Improvements in Microsoft Fabric](#multitasking-and-workflow-improvements-in-microsoft-fabric)
  - [Experimentation Analytics with Statsig in Microsoft Fabric](#experimentation-analytics-with-statsig-in-microsoft-fabric)
- [Azure](#azure)
  - [Microsoft Fabric: Data Integration, Networking, and Automation](#microsoft-fabric-data-integration-networking-and-automation)
  - [Advanced Connectivity, Mirroring, and Cross-Cloud Features in Fabric Data Factory](#advanced-connectivity-mirroring-and-cross-cloud-features-in-fabric-data-factory)
  - [Fabric CLI and Secure Data Movement for Fabric](#fabric-cli-and-secure-data-movement-for-fabric)
  - [Azure Platform Updates, Service Announcements, and Cost Optimization](#azure-platform-updates-service-announcements-and-cost-optimization)
  - [Azure Containerization and Storage Updates](#azure-containerization-and-storage-updates)
  - [Governance, Policy as Code, and Developer Security](#governance-policy-as-code-and-developer-security)
  - [Analytics, Monitoring, and Database Features](#analytics-monitoring-and-database-features)
  - [Edge, Hybrid, and Multicloud Azure Developments](#edge-hybrid-and-multicloud-azure-developments)
  - [Event-Driven Architectures and Automation](#event-driven-architectures-and-automation)
  - [Open-Source Initiatives and Developer Community](#open-source-initiatives-and-developer-community)
  - [SharePoint Embedded and SaaS Content Management](#sharepoint-embedded-and-saas-content-management)
  - [Developer Workflow and Troubleshooting](#developer-workflow-and-troubleshooting)
  - [Community Contributions and Technical Spotlights](#community-contributions-and-technical-spotlights)
  - [Azure HPC, Event Management, and Workflow Integration](#azure-hpc-event-management-and-workflow-integration)
- [Coding](#coding)
  - [Visual Studio 2026 Insiders: TypeScript 7 Native Preview and Razor Tooling Evolution](#visual-studio-2026-insiders-typescript-7-native-preview-and-razor-tooling-evolution)
  - [Coding Agents and Workflow Automation in Visual Studio Code](#coding-agents-and-workflow-automation-in-visual-studio-code)
  - [Modern ASP.NET API Architecture with FastEndpoints and the REPR Pattern](#modern-aspnet-api-architecture-with-fastendpoints-and-the-repr-pattern)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [Secure NuGet Publishing and Trusted CI/CD Workflows](#secure-nuget-publishing-and-trusted-cicd-workflows)
  - [AI-driven DevOps Automation and Infrastructure Management](#ai-driven-devops-automation-and-infrastructure-management)
  - [Azure SRE Agent Expands: Automation, Diagnostics, and Incident Response](#azure-sre-agent-expands-automation-diagnostics-and-incident-response)
  - [AI Adoption in DevOps: Trust, Velocity, and Pipeline Bottlenecks](#ai-adoption-in-devops-trust-velocity-and-pipeline-bottlenecks)
  - [AI-driven Project Management for DevOps Teams](#ai-driven-project-management-for-devops-teams)
  - [AI in the DevOps Lifecycle and Quality Assurance Mindset](#ai-in-the-devops-lifecycle-and-quality-assurance-mindset)
  - [Platform, Tooling, and Workflow Updates](#platform-tooling-and-workflow-updates)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [DevOps and Supply Chain Security](#devops-and-supply-chain-security)
  - [Cloud Security, AI Workload Protection, and Governance on Azure](#cloud-security-ai-workload-protection-and-governance-on-azure)
  - [Cloud Identity and Access Management (IAM)](#cloud-identity-and-access-management-iam)
  - [Security Automation and Secret Management](#security-automation-and-secret-management)
  - [Advanced Security Analysis and Developer Tutorials](#advanced-security-analysis-and-developer-tutorials)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week, GitHub Copilot introduced new and updated features: deeper integration with Visual Studio and VS Code, expanded access to AI models such as Anthropic Claude Sonnet 4.5 for chat and CLI use, and improvements to agent workflows. .NET and Azure users benefit from better performance analysis and modernization tools. Copilot’s improvements include enhanced analytics, prompt engineering, and rapid prototyping for enterprise teams, supporting the move to AI-assisted coding across desktop, web, terminal, and cloud for increased developer flexibility and efficiency.

### GitHub Copilot Integration in Visual Studio

Visual Studio 2022’s September 2025 release (v17.14) adds new capabilities for Copilot, pairing tools like the Profiler Agent and .NET Modernization Agent as part of ongoing agentic workflow automation. The Profiler Agent, accessible via Copilot Chat or the `@Profiler` command, delivers diagnostics and benchmarking, adding context-driven automation and performance review. Integrating BenchmarkDotNet simplifies modernization and Azure migration for .NET workloads.

Agent Mode produces faster responses and stronger context management, with better Model Context Protocol (MCP) support for extensibility and structured outputs. Features such as Mermaid diagram generation and code review promote collaborative development. The October roadmap highlights progress toward remote agents, MCP governance, group policy, and wider model support (Claude Sonnet 4.5, future GPT-5 Codex) as Visual Studio continues evolving as an AI-ready developer platform.

- [GitHub Copilot Updates in Visual Studio September 2025 Release](https://github.blog/changelog/2025-09-30-github-copilot-in-visual-studio-september-update)
- [Visual Studio September 2025 Update: Profiler Agent, App Modernization, GitHub Copilot Enhancements](https://devblogs.microsoft.com/visualstudio/visual-studio-september-update/)
- [Visual Studio AI-Powered Roadmap: October Updates with GitHub Copilot](https://devblogs.microsoft.com/visualstudio/roadmap-for-ai-in-visual-studio-october/)

### GitHub Copilot CLI and Terminal Workflows

CLI releases reinforce the transition from preview to general availability, replacing older extensions with a unified npm CLI. Anthropic Claude Sonnet 4.5 is now active in the terminal using the `/model` command for flexible model switching, continuing the shift toward cross-model agentic experiences. New CLI features support image handling, improved input processing, and refined context management for multimodal interaction.

Security and analytics improvements add detailed permission controls and dashboards for usage tracking, helping teams manage resources and promote transparency. Context truncation alerts and improved command forwarding strengthen developer tool reliability. Tutorials and practical guides support smooth onboarding for agentic workflows.

- [GitHub Copilot CLI: Enhanced Model Selection, Image Recognition, and Streamlined UI Improvements](https://github.blog/changelog/2025-10-03-github-copilot-cli-enhanced-model-selection-image-support-and-streamlined-ui)
- [Full Demo: Mastering GitHub Copilot CLI for Terminal-Based Development](/ai/videos/Full-Demo-Mastering-GitHub-Copilot-CLI-for-Terminal-Based-Development)
- [The Coding Buddy That Lives in Your Command Line: GitHub Copilot CLI](/ai/videos/The-Coding-Buddy-That-Lives-in-Your-Command-Line-GitHub-Copilot-CLI)

### GitHub Copilot Coding Agent

With last week’s Coding Agent general availability and updated session controls, Copilot now includes a repository kickstart option to automate project scaffolding, making agent workflows more approachable for teams. The agent maintains pull request history to streamline code review rounds and onboarding. Claude Sonnet 4.5 integration builds on past model updates like GPT-5-Codex and Copilot-SWE, supporting better code generation and handling with practical SWE-bench feedback.

Recent admin guides help teams configure policies and manage models, providing governance for expanding AI-powered automation.

- [Kickstart Repositories Using Copilot Coding Agent](https://github.blog/changelog/2025-09-30-start-your-new-repository-with-copilot-coding-agent)
- [Copilot Coding Agent Now Remembers Context Within Pull Requests](https://github.blog/changelog/2025-09-30-copilot-coding-agent-remembers-context-within-the-same-pull-request)
- [Anthropic Claude Sonnet 4.5 Now Available for GitHub Copilot Coding Agent](https://github.blog/changelog/2025-09-30-anthropic-claude-sonnet-4-5-is-in-public-preview-for-copilot-coding-agent)

### AI Model Selection and Integration in Copilot

Claude Sonnet 4.5 is now generally available in Copilot Chat and CLI, with unified policy controls and a public preview for automatic model selection. This transition marks full deployment from initial previews to availability across major IDEs (Visual Studio, VS Code, JetBrains, Xcode, Eclipse, GitHub.com, Mobile).

Automatic model selection in VS Code for Copilot Business/Enterprise automatically chooses between GPT-5, GPT-4.1, Sonnet 4, and Sonnet 3.5 to balance user experience and operational costs. Analytic transparency and billing features allow teams to monitor resource use. Bring Your Own Key (BYOK) lets organizations manage custom API credentials, and staged rollout plus policy controls help teams use AI responsibly.

- [Claude Sonnet 4.5 Now Available to GitHub Copilot Users in Visual Studio and Other IDEs](https://github.blog/changelog/2025-10-02-claude-sonnet-4-5-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Anthropic Claude Sonnet 4.5 Launches for GitHub Copilot Chat and CLI](https://github.blog/changelog/2025-09-29-anthropic-claude-sonnet-4-5-is-in-public-preview-for-github-copilot)
- [Auto Model Selection Now Available in VS Code for GitHub Copilot Business and Enterprise](https://github.blog/changelog/2025-09-30-auto-model-selection-is-now-in-vs-code-for-copilot-business-and-enterprise)

### GitHub Spark and Rapid Application Prototyping

GitHub Spark’s public preview for prototyping in Codespaces now enrolls Copilot Enterprise users, featuring better reliability and reduced setup effort. The update automates initial configuration and simplifies foundational tasks—a continuation of previous workflow orchestration changes.

Recent bug fixes, improved iteration history, and smoother workbench interfaces reflect Copilot’s ongoing push for productivity and onboarding improvements.

- [GitHub Spark Public Preview Now Available for Copilot Enterprise Subscribers](https://github.blog/changelog/2025-09-30-github-spark-in-public-preview-for-copilot-enterprise-subscribers)
- [Spark Public Preview Released for Copilot Enterprise: Expanded Access and Enhanced Reliability](https://github.blog/changelog/2025-10-01-spark-%f0%9f%9a%80-expanded-access-enhanced-reliability-and-faster-iteration-history)

### GitHub Copilot Advanced Documentation, Prompt Engineering, and Parallel Workflows

Documentation and prompt engineering updates build on last week’s adoption of standardized `.prompt.md` files and agentic parallel workflows. Tutorials outline methods for auto-generating README files, API documentation, and inline comments, supporting reusable prompts and modular documents that scale with team needs. Expanded parallel workflow techniques now let developers orchestrate tasks across Copilot Chat, CLI, and Coding Agent for faster delivery.

Spec-driven development merges Markdown for code and documentation, continuing from previous experimental workflow showcases. Tutorials and videos highlight Copilot’s broader utility for planning, brainstorming, and creative work.

- [Advanced Techniques for Documenting Code with GitHub Copilot](/ai/videos/Advanced-Techniques-for-Documenting-Code-with-GitHub-Copilot)
- [Supercharge Your Prompts with .prompt.md](https://www.cooknwithcopilot.com/blog/supercharge-your-prompts-with-prompt-md.html)
- [Using GitHub Copilot for Multiple Tasks in Parallel](https://harrybin.de/posts/parallel-github-copilot-workflow/)
- [Spec-driven Development with Markdown and GitHub Copilot: An Experimental AI Coding Workflow](https://github.blog/ai-and-ml/generative-ai/spec-driven-development-using-markdown-as-a-programming-language-when-building-with-ai/)
- [Prompting for More Than Code with GitHub Copilot](/ai/videos/Prompting-for-More-Than-Code-with-GitHub-Copilot)

### GitHub Copilot Workflow Analytics and Registry Support

Copilot’s Premium Requests Analytics Dashboard, now generally available, provides granular usage tracking by user, model, and cost center, increasing admin oversight and supporting enterprise-level automation and transparency.

Model Context Protocol (MCP) Registry use continues to expand, building on last week’s guides concerning protocol development and integration. Teams increasingly experiment with open protocol customization for reusable agentic workflows.

- [Premium Requests Analytics Dashboard for GitHub Copilot: General Availability](https://github.blog/changelog/2025-09-30-premium-requests-analytics-page-is-now-generally-available)
- [The Origins & Evolution of the GitHub MCP Registry with Toby Padilla](/ai/videos/The-Origins-and-Evolution-of-the-GitHub-MCP-Registry-with-Toby-Padilla)

### Other GitHub Copilot News

Visual Studio Code introduces updates for background coding agents, building on previously reported advances in agentic automation. Feedback features and more customizable agent behaviors support context-aware development.

Workshops like "How to Master GitHub Copilot" offer hands-on training for MCP integration, modernization, and cloud deployment, supporting community learning as Copilot’s feature set expands.

- [VS Code: Background Coding Agents and GitHub Copilot Enhancements](/ai/videos/VS-Code-Background-Coding-Agents-and-GitHub-Copilot-Enhancements)

- [How to Master GitHub Copilot: Build, Prompt, Deploy Smarter](https://techcommunity.microsoft.com/t5/microsoft-developer-community/how-to-master-github-copilot-build-prompt-deploy-smarter/ba/p/4456660)

## AI

AI technology for developers saw new releases and expanded platform features, focusing on agentic models and enterprise automation. Microsoft Agent Framework now unifies orchestration experiences, integrating with Azure AI Foundry and Model Context Protocol (MCP). Grok 4 arrives on Azure AI Foundry with better reasoning and expanded context support. Updates center on improved automation, context retention, and data transformation, moving development teams from isolated models to modular, multi-agent workflows.

### Microsoft Agent Framework: Unified Agentic AI SDKs and Ecosystem Integrations

Microsoft Agent Framework is now in public preview, bringing a unified and open-source SDK for agentic AI to .NET and Python. This initiative, building on prior orchestration efforts like Semantic Kernel and AutoGen, simplifies multi-agent management. By supporting open protocols (MCP), developers gain modular context management, human-in-loop routing, thread-based state, and integrated Azure AI Foundry experiences.

Case studies such as automated audit workflows for KPMG Clara and voice-assisted services for Commerzbank show practical enterprise adoption. Community involvement in open-source development provides migration support from older agent SDKs. Observability via OpenTelemetry and security controls with Entra ID reinforce last week’s progress.

- [Introducing the Microsoft Agent Framework: Unified SDK for AI Agents and Workflows](https://techcommunity.microsoft.com/t5/microsoft-developer-community/introducing-the-microsoft-agent-framework/ba/p/4458377)
- [Introducing Microsoft Agent Framework: The Open-Source Engine for Agentic AI Apps](https://devblogs.microsoft.com/foundry/introducing-microsoft-agent-framework-the-open-source-engine-for-agentic-ai-apps/)
- [Introducing Microsoft Agent Framework: Streamlining Multi-Agent AI Systems with Azure AI Foundry](https://azure.microsoft.com/en-us/blog/introducing-microsoft-agent-framework/)
- [Introducing Microsoft Agent Framework: Simplifying AI Agent Development for .NET Developers](https://devblogs.microsoft.com/dotnet/introducing-microsoft-agent-framework-preview/)
- [Agent Framework: Building Blocks for the Next Generation of AI Agents](/ai/videos/Agent-Framework-Building-Blocks-for-the-Next-Generation-of-AI-Agents)
- [Microsoft Agent Framework Powers Multi-Agent Systems in Azure AI Foundry](https://www.linkedin.com/posts/satyanadella_introducing-microsoft-agent-framework-microsoft-activity-7379202146988318720-WDPf)

### Model Context Protocol (MCP) and Secure AI Integrations in Microsoft Fabric

Agentic AI open standards continue growing, with Fabric MCP’s API context streamlining developer experience across Microsoft Fabric. MCP’s standardization makes onboarding and automation quicker and safer, now reaching more data environments. The GitHub MCP Registry increases interoperability, and certified server discovery (including Figma, Postman, Terraform) demonstrates increased practical adoption.

Reviews of MCP’s effects show protocol-driven reduction of fragmentation and support for reusable workflows inside enterprise IDEs.

- [Introducing Fabric MCP (Preview): Developer-Focused AI Integration for Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/introducing-fabric-mcp-public-preview/)
- [How Model Context Protocol (MCP) Is Transforming AI-Driven Development Productivity](https://devops.com/how-model-context-protocol-mcp-is-fueling-the-next-era-of-developer-productivity/)
- [How to find the right MCP server in seconds with GitHub MCP registry](/ai/videos/How-to-find-the-right-MCP-server-in-seconds-with-GitHub-MCP-registry)

### Grok 4: Advanced Reasoning Models in Azure AI Foundry

Grok 4 launches in Azure AI Foundry, expanding options after last week’s additions of new OpenAI and Anthropic models. Its multi-agent design and reinforcement learning take agentic automation beyond earlier Grok 3 updates.

Developers benefit from improved reasoning, larger context windows, and safer operations, while Foundry enables fast reasoning, summarization, and integrated code debugging for enterprise use—continuing a trend toward robust, compliant AI tools.

- [Grok 4 Now Available in Azure AI Foundry: Advanced Reasoning and Business-Ready AI](https://azure.microsoft.com/en-us/blog/grok-4-is-now-available-in-azure-ai-foundry-unlock-frontier-intelligence-and-business-ready-capabilities/)

### Copilot, Voice, and Intelligent Data Tools: Workflow Automation and NLP Integration

Copilot-powered automation now enables natural language workflow orchestration in Fabric Data Factory, continuing the momentum in analytics and user onboarding first seen last week.

Developers are adopting real-time voice-driven AI agents with Azure Voice Live API, applying conversational prompts and multimodal interactions for new process automation. Coverage of NLP tools explores documentation automation and references prior Copilot, VS Code, and JetBrains integrations.

- [AI-Powered Data Transformation and Insights with Copilot in Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/ai-powered-development-with-fabric-data-factory-ingest-transform-and-understand-your-data-with-copilot/)
- [Building a Real-Time Voice-Powered AI Sales Coach Using Azure Voice Live API](https://devblogs.microsoft.com/all-things-azure/from-lab-to-live-a-blueprint-for-a-voice-powered-ai-sales-coach/)
- [NLP Tools for Intelligent Documentation and Developer Enablement](https://devops.com/nlp-tools-for-intelligent-documentation-and-developer-enablement/)

### Practical Guides for Agentic AI in Python and Enterprise Workflows

This week’s guides share best practices for agentic AI in Python and enterprise systems. LangChain and CrewAI libraries address context retention challenges described in earlier tutorials.

Tutorials and documentation emphasize automation for diagrams and onboarding in CI/CD, supporting ongoing architecture compliance improvements. AIOps guidance merges traditional monitoring and anomaly detection with AI, following last week’s coverage of improved analytics integration.

- [Managing Context Retention in Agentic AI with Python and LangChain](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/managing-context-retention-in-agentic-ai/ba/p/4458586)
- [Documenting Architecture Using AI: From Painful Chore to Strategic Advantage](https://dellenny.com/documenting-architecture-using-ai-from-painful-chore-to-strategic-advantage/)
- [AIOps: Bringing Intelligence to IT Operations](https://dellenny.com/aiops-bringing-intelligence-to-it-operations/)

### AI Agents: Concepts, Architecture, and Developer Adoption

Discussions on agentic AI concepts and workflow adoption revisit last week’s conversations around code ownership and community trust. New guides and videos analyze the software development lifecycle in the context of agent use, with hands-on coverage explaining how Azure AI Foundry and agent architecture tools lower costs and facilitate deployment.

- [How AI Is Changing the SDLC: Trust, Ownership, and Community in Modern Software Development](https://www.arresteddevops.com/ai-sdlc/)
- [Why is everyone suddenly talking about AI agents?](/ai/videos/Why-is-everyone-suddenly-talking-about-AI-agents)
- [Understanding AI Agents: Turning Plain Language into Code Execution](/ai/videos/Understanding-AI-Agents-Turning-Plain-Language-into-Code-Execution)
- [What is an AI Agent?](/ai/videos/What-is-an-AI-Agent)

### Retrieval Augmented Generation (RAG) and Document Processing Workflows

This week introduces a practical RAG workflow guide integrating OpenAI and Azure SQL based on last week’s enterprise data-to-chat tutorials. Logic Apps add features for metadata chunking, helping automate compliance in contract review and documentation workflows.

- [Azure SQL DB & OpenAI: Building Powerful RAG Applications](/ai/videos/Azure-SQL-DB-and-OpenAI-Building-Powerful-RAG-Applications)
- [Enhancing Logic Apps with Parse & Chunk with Metadata for AI-Powered Document Processing](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-parse-chunk-with-metadata-in-logic-apps-build-context/ba/p/4458438)

### Azure AI in Enterprise Workflows: Supply Chain & Scalable Cloud Infrastructure

Supply chain and forecasting solutions continue last week’s coverage of unified data workflows, showing business gains through resilient, AI-enhanced systems. Microsoft’s scalable AI infrastructure supports enterprise workloads for Copilot, ChatGPT, and other tools—underscoring reliability and developer productivity.

- [How Azure AI is Revolutionizing Supply Chain Forecasting and Inventory](https://dellenny.com/how-azure-ai-is-revolutionizing-supply-chain-forecasting-and-inventory/)
- [Microsoft's Scalable AI Infrastructure for Copilot, ChatGPT, and Enterprise AI Workloads](https://www.linkedin.com/posts/satyanadella_our-approach-to-ai-infra-is-simple-build-activity-7379681735934083073-Scma)

### AI Ethics and Security in Scientific and Enterprise Environments

A Microsoft-led biosecurity report published in Science Magazine expands last week’s ethics coverage, focusing on safety measures and adversarial testing in generative models for biosciences. These conversations continue to emerge across scientific AI applications.

- [Microsoft-Led Study Unveils AI Protein Design Biosecurity Research in Science Magazine](https://www.linkedin.com/posts/satyanadella_strengthening-nucleic-acid-biosecurity-screening-activity-7379576230414753792-zB85)

### Other AI News

Visual Studio Code’s Bring Your Own Key (BYOK) support for model provider APIs follows last week’s workflow feature updates, enabling improved integration of third-party models in AI-driven coding.

Weekly Foundry Fridays AMA sessions grow community involvement, sharing technical best practices within Azure AI Foundry—videos are available for those seeking guidance on complex topics.

- [Enhancements to BYOK in Visual Studio Code: Model Provider Integration](/ai/videos/Enhancements-to-BYOK-in-Visual-Studio-Code-Model-Provider-Integration)
- [Foundry Fridays: Weekly Azure AI Foundry AMA Series](https://techcommunity.microsoft.com/t5/microsoft-developer-community/foundry-fridays-your-front-row-seat-to-azure-ai-innovation/ba/p/4456956)

## ML

This week’s updates in machine learning focus on deeper integrations with the Microsoft Fabric ecosystem: faster, more affordable data transformations, enhanced multitasking, and streamlined collaboration. Broader connectivity between cloud and analytics platforms and stronger tools for experimentation mark a focus on flexible, interactive enterprise workflows.

### Dataflow Gen2 in Microsoft Fabric: Performance, Integration, and Developer Experience

Dataflow Gen2 in Microsoft Fabric gains a new pricing model, helping organizations manage ETL costs for jobs of all sizes. The Modern Query Evaluation Service speeds up parallel queries for lower expenses and shorter runtimes, advancing last week’s troubleshooting features like Spark monitoring APIs.

Real-time analytics and previews allow faster iteration on transformation logic, with outputs now targeting Fabric Lakehouse, Azure Data Lake Gen2, SharePoint (CSV), Snowflake (preview), and OneLake Catalog management—matching the trend of multi-environment integration.

Copilot now enables natural language transformation and ingestion, contributing to collaborative machine learning themes. Migration from Gen1 is supported by dedicated tools. Permission management, schema controls, and hybrid architecture improvements continue the previous focus on operational governance.

- [Announcing Dataflow Gen2 Enhancements in Microsoft Fabric: Faster, Smarter, and More Affordable Data Transformations](https://blog.fabric.microsoft.com/en-US/blog/unlocking-the-next-generation-of-data-transformations-with-dataflow-gen2-fabcon-europe-2025-announcements/)
- [Enhancements to Dataflow Gen2 in Microsoft Fabric: Expanded Destinations and Schema Support](https://blog.fabric.microsoft.com/en-US/blog/new-dataflow-gen2-data-destinations-and-experience-improvements/)

### AI-Powered Data Transformation and Developer Tools

Fabric Data Wrangler now supports fast AI-driven text summarization, translation, and sentiment analysis through PROSE suggestions and live previews. Copilot prompts generate custom transformation code and feedback, minimizing manual coding for complex datasets. Conversion between pandas and PySpark further scales projects, while documentation and guides support adoption of these new workflows.

- [Accelerate Data Transformation with AI in Data Wrangler](https://blog.fabric.microsoft.com/en-US/blog/accelerate-data-transformation-with-ai-functions-in-data-wrangler/)

### Multitasking and Workflow Improvements in Microsoft Fabric

Fabric’s updated horizontal tabs permit working on multiple items, along with workspace color coding and numbering to prevent errors and reduce context switching. The Object Explorer and higher concurrent item limits cater to users who need advanced multitasking—building on recent improvements for async processing and VS Code extension integration. These features are specific to Fabric.

- [Supercharge Your Workflow: New Multitasking Features Coming to Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/supercharge-your-workflow-new-multitasking-features-coming-to-fabric/)

### Experimentation Analytics with Statsig in Microsoft Fabric

Statsig Experimentation Analytics in Fabric provides tools for running and analyzing A/B/n tests on OneLake data, using frequentist statistics and near real-time metrics via Statsig’s Explorer. Instant results allow rapid update cycles, and Power BI integration assists visual review of experiments. Structured workflows help teams validate ML models, continuing last week’s focus on practical MLOps processes.

- [Experimentation Analytics with Statsig in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/27219/)

## Azure

Azure’s latest updates bring expanded automation in Microsoft Fabric, more secure data integrations, new open-source tools, improved CLI features, and fresh platform guidance. Service changes focus on lower costs and multicloud flexibility, with continued developer empowerment and adaptable platforms.

### Microsoft Fabric: Data Integration, Networking, and Automation

Fabric’s Virtual Network Data Gateway is now generally available for Pipelines, Dataflow Gen2, and Copy Job, strengthening private networking options. Copy Job adds new connectors, advanced Change Data Capture (CDC for Snowflake), and a Variables Library—pushing hybrid orchestration forward. Data Factory has been updated with support for Workspace Identity, Private Link, Key Vault, and expanded automation. Fabric Mirroring for Azure SQL Managed Instance reaches GA, enabling mirrored data for analytics and AI in new scenarios. Workspace-level Private Link is also GA, increasing control for data science and ML teams. The updated On-premises Data Gateway improves BigQuery mirroring and compatibility with Power BI Desktop.

- [Virtual Network Data Gateway Now Generally Available for Fabric Pipeline, Dataflow Gen2, and Copy Job](https://blog.fabric.microsoft.com/en-US/blog/virtual-network-data-gateway-support-for-fabric-pipeline-dataflow-gen2-fast-copy-and-copy-job-is-now-generally-available/)
- [Enhancements to Copy Job for Data Ingestion in Microsoft Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-connection-parameterization-expanded-cdc-and-connectors/)
- [Mission-Critical Data Integration: New Security and Automation in Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/mission-critical-data-integration-whats-new-in-fabric-data-factory/)
- [Fabric Mirroring for Azure SQL Managed Instance Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/announcing-the-general-availability-ga-of-mirroring-for-azure-sql-managed-instance-in-microsoft-fabric/)
- [Workspace-Level Private Link in Microsoft Fabric Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/announcing-general-availability-of-workspace-level-private-link-in-microsoft-fabric/)
- [On-premises Data Gateway September 2025 Release: New Features & Power BI Desktop Compatibility](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-september-2025-release/)

### Advanced Connectivity, Mirroring, and Cross-Cloud Features in Fabric Data Factory

Fabric Data Factory introduces new connectors (AWS RDS Oracle, PostgreSQL 2.0, Databricks Delta Lake, Cassandra) for cross-cloud integration, better observability, and improved error management. Security upgrades include Entra ID authentication and TLS 1.3. Real-time Oracle Mirroring launches in preview, providing direct BI and ML query access and marking continued multicloud expansion.

- [Cross-Cloud Data Movement with Best-in-Class Connectivity in Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/cross-cloud-data-movement-with-best-in-class-connectivity-whats-new-and-whats-next/)
- [Enhancements in Microsoft Fabric Data Factory Connectors for Enterprise-Scale Data Integration](https://blog.fabric.microsoft.com/en-US/blog/unlocking-seamless-data-integration-with-the-latest-fabric-data-factory-connector-innovations/)
- [Mirroring Oracle Data into Microsoft Fabric: Real-Time Integration without ETL](https://blog.fabric.microsoft.com/en-US/blog/mirroring-for-oracle-in-microsoft-fabric-preview/)

### Fabric CLI and Secure Data Movement for Fabric

Fabric CLI v1.1.0 is open source, offering new features like JSON output, workspace context selection, and folder organization for automation. Copy Job integration with Virtual Network Data Gateway supports compliance and private endpoint deployments, aligning with recent security improvements for data pipelines.

- [Fabric CLI Goes Open Source with AI-Ready Features in v1.1.0](https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-open-source-ai-ready-and-more-powerful/)
- [Secure Data Movement in Microsoft Fabric with Copy Job and VNET Data Gateway](https://blog.fabric.microsoft.com/en-US/blog/secure-your-data-movement-with-copy-job-and-virtual-network-data-gateway/)

### Azure Platform Updates, Service Announcements, and Cost Optimization

October’s Azure Update shares upcoming service retirements plus new features for Compute Gallery, SQL Database, and Traffic Manager. Guidance supports planning for lifecycle and resilience. The Azure Essentials guide offers practical advice on optimizing cloud and AI costs based on previous frameworks (CAF/WAF) and governance.

- [Azure Update - October 2025 Announcements and Retirements](/ai/videos/Azure-Update-October-2025-Announcements-and-Retirements)
- [Maximizing Cost Efficiency with Azure Essentials for Cloud and AI Investments](https://techcommunity.microsoft.com/t5/azure-governance-and-management/cloud-and-ai-cost-efficiency-a-strategic-imperative-for-long/ba/p/4455955)

### Azure Containerization and Storage Updates

Guides explain Azure Container Storage v2 and provide platform choices among AKS, App Service, and Azure Container Apps, supporting broader orchestration and deployment approaches. Note: Storage v2 requires redeployment for changes, with no direct migration path—continuing infrastructure planning updates.

- [Choosing the Right Azure Containerisation Strategy: AKS, App Service, or Container Apps?](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/choosing-the-right-azure-containerisation-strategy-aks-app/ba/p/4456645)
- [Azure Container Storage v2 Overview](/azure/videos/Azure-Container-Storage-v2-Overview)

### Governance, Policy as Code, and Developer Security

Azure AI Landing Zones now integrate policy automation with Azure Policy, EPAC, and DevOps workflows, supporting compliance and repeatable infrastructure-as-code deployments with Bicep/ARM and centralized Entra ID. SystemData walkthroughs support streamlined auditing; SSSD and Entra ID integration help secure cloud-native Linux HPC clusters, reinforcing last week’s login and identity improvements.

- [Building a Secure and Compliant Azure AI Landing Zone: Policy Framework & Best Practices](https://techcommunity.microsoft.com/t5/azure-architecture-blog/building-a-secure-and-compliant-azure-ai-landing-zone-policy/ba/p/4457165)
- [How to Identify Who Created an Azure Resource Using SystemData](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/who-created-this-azure-resource-here-s-how-to-find-out/ba/p/4458470)
- [Implementing SSSD with Microsoft Entra ID for Azure Linux HPC Clusters](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/use-entra-ids-to-run-jobs-on-your-hpc-cluster/ba/p/4457932)

### Analytics, Monitoring, and Database Features

Fabric SQL DB’s point-in-time restore window expands to 35 days, further improving data resilience. New memory metrics, spillover visualization, and detailed query drilldowns help developers with troubleshooting and support Azure’s ongoing commitment to performance transparency.

- [Extending Point-in-Time Retention in Fabric SQL DB: From 7 to 35 Days](https://blog.fabric.microsoft.com/en-US/blog/extending-point-in-time-retention-in-fabric-sql-db-from-7-to-35-days/)
- [Introducing Memory Consumption Metrics for SQL Database in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/memory-consumption-metrics-now-available-for-fabric-sql-database/)

### Edge, Hybrid, and Multicloud Azure Developments

Azure Local is now generally available for government users, continuing support for compliant deployment. Oracle Database@Azure expands coverage for Oracle Database 19c and adds new regional support. Oracle AI World sessions and the Hybrid Cloud Playbook focus on multicloud deployments and architecture. HPE’s integration for SQL Server 2025 increases analytics and hybrid capability for Arc-enabled scenarios.

- [Azure Local Now Generally Available for Government Cloud Customers](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-general-availability-of-azure-local-on-microsoft/ba/p/4458013)
- [Oracle Database 19c Now Supported on Exadata Exascale with Oracle Database@Azure and New UAE Regions](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/oracle-database-azure-now-supports-oracle-database-19c-on/ba/p/4458643)
- [Microsoft at Oracle AI World 2025: AI, Multicloud, and Data Transformation with Azure](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/microsoft-is-headed-to-oracle-ai-world-2025-in-las-vegas/ba/p/4457390)
- [The Hybrid Cloud Playbook: Mastering Azure Stack](https://dellenny.com/the-hybrid-cloud-playbook-mastering-azure-stack/)
- [HPE Solutions and Azure Edge Architecture for AI-Ready SQL Server 2025 Workloads](/ai/videos/HPE-Solutions-and-Azure-Edge-Architecture-for-AI-Ready-SQL-Server-2025-Workloads)

### Event-Driven Architectures and Automation

Azure Event Grid introduces OAuth 2.0/JWT authentication and better MQTT support. Updates advance security for IoT and event-driven systems, including factory monitoring and SCADA/IoT integration.

- [What's New in Azure Event Grid: Security, MQTT, and Smart Factory Integration](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/what-s-new-in-azure-event-grid/ba/p/4458299)

### Open-Source Initiatives and Developer Community

Microsoft continues open source progress with kernel contributions, CNCF involvement, and Model Context Protocol expansion—following up on events at All Things Open 2025 and emphasizing sustainable developer tooling and community participation.

- [Microsoft's Role in Open-Source Sustainability: From Kernel to Copilot at All Things Open 2025](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/the-open-source-paradox-how-microsoft-is-giving-back/ba/p/4458630)

### SharePoint Embedded and SaaS Content Management

Guides for SharePoint Embedded continue last week’s news about integrating API-driven Microsoft 365 services, with secure and compliant content management for SaaS and ISVs.

- [Unlocking the Power of SharePoint Embedded: A Developer-Focused Approach to Content Management](https://dellenny.com/unlocking-the-power-of-sharepoint-embedded-a-modern-approach-to-content-management/)

### Developer Workflow and Troubleshooting

New recommendations for exclusion logic in Azure Storage Actions and PowerShell troubleshooting simplify developer workflows. Updated guides on RBAC and authentication help teams adopt better security practices for automation scripts.

- [How to Use Exclude Prefix for Smarter Blob Management in Azure Storage Actions](https://techcommunity.microsoft.com/t5/azure-paas-blog/exclude-prefix-in-azure-storage-action-smarter-blob-management/ba/p/4440075)
- [Resolving AuthorizationPermissionMismatch for Set-AzStorageBlobContent in PowerShell](https://techcommunity.microsoft.com/t5/azure-storage/differences-between-powershell-and-browser-when-upload-file/m-p/4458068#M574)

### Community Contributions and Technical Spotlights

Microsoft Fabric’s influencer spotlight for September offers best practices on Power BI, scaling data science, and CI/CD troubleshooting—extending last week’s focus on successful analytics and deployment guidance.

- [September 2025 Microsoft Fabric Influencers Spotlight](https://blog.fabric.microsoft.com/en-US/blog/fabric-influencers-spotlight-september-2025/)

### Azure HPC, Event Management, and Workflow Integration

Cloud-native HPC expands at the HPC Roundtable 2025, in line with ongoing hybrid orchestration improvements and AMD VM news. Event SaaS solutions leveraging Azure for identity management and automation increase developer and operational productivity, supporting startups and ISVs.

- [Exploring HPC and AI Innovation with Microsoft and AMD at the HPC Roundtable 2025](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/explore-hpc-ai-innovation-microsoft-amd-at-hpc-roundtable-2025/ba/p/4457974)
- [Transforming Event Management with InEvent and Microsoft Azure](https://www.microsoft.com/en-us/startups/blog/building-effortless-events-how-inevent-and-microsoft-for-startups-are-transforming-event-management/)

## Coding

Coding updates this week highlight improvements in Visual Studio and .NET, emphasizing better performance, day-to-day workflow enhancements, and clear API design. New frameworks and previews focus on more efficient, maintainable solutions for modern development.

### Visual Studio 2026 Insiders: TypeScript 7 Native Preview and Razor Tooling Evolution

Visual Studio 2026 Insiders now offers a native preview for TypeScript 7, following last week’s advances in container tools and Aspire tracing. Native implementation yields faster compile times and reduced memory usage—empirical data from large projects (VS Code, Playwright) illustrates these gains. Microsoft is asking for community feedback to guide further changes.

Razor tooling updates improve Hot Reload and editing within the ASP.NET Community Standup, reflecting ongoing efforts for more responsive development and real-time feedback, with fewer build interruptions—continuing the consistent expansion of Visual Studio features.

- [TypeScript 7 Native Preview Now Available in Visual Studio 2026 Insiders](https://devblogs.microsoft.com/blog/typescript-7-native-preview-in-visual-studio-2026)
- [ASP.NET Community Standup - Razor Reloaded](/coding/videos/ASPNET-Community-Standup-Razor-Reloaded)

### Coding Agents and Workflow Automation in Visual Studio Code

Coding agent integration in VS Code is further refined to improve speed and accuracy, building on last week’s MCP-based automation and collaborative development advances. Feedback mechanisms allow developers to directly influence future improvements, promoting real-world, robust workflow automation.

- [Latest Updates to Coding Agent Integration in Visual Studio Code](/coding/videos/Latest-Updates-to-Coding-Agent-Integration-in-Visual-Studio-Code)

### Modern ASP.NET API Architecture with FastEndpoints and the REPR Pattern

Guidance for ASP.NET API design now favors the REPR (Request, Endpoint, Response) pattern with FastEndpoints, stepping away from the older controller structure featured last week. Marcel Medina’s tutorial provides clear, testable examples that support migration and database management, focusing on maintainable, scalable solutions.

- [Simplifying ASP.NET API Design with the REPR Pattern and FastEndpoints](/coding/videos/Simplifying-ASPNET-API-Design-with-the-REPR-Pattern-and-FastEndpoints)

### Other Coding News

A timely reminder steers developers toward maximizing Visual Studio subscription perks, including monthly Azure credits, access to Microsoft dev/test software, and training portals such as Pluralsight and Cloud Academy. These tips support ongoing .NET lifecycle and migration planning for more efficient workflows.

- [Unlocking the Hidden Value of Your Visual Studio Subscription](https://devblogs.microsoft.com/visualstudio/unlock-vss-benefits-myvisualstudio/)

## DevOps

DevOps news showcases updated automation tools, AI integration, and security-focused platform practices. Teams work toward secure CI/CD, efficient AI-powered infrastructure management, improved governance, and practical connections between code, infrastructure, and incident response—all geared for reliable delivery and balanced development speed.

### Secure NuGet Publishing and Trusted CI/CD Workflows

Andrew Lock’s step-by-step guide explains Secure NuGet Package Publishing using ephemeral credentials with GitHub Actions, replacing static API keys for direct nuget.org authentication. Developers specify workflow permissions and deploy the NuGet/login@v1 YAML action for flexible access, supporting secure collaboration and compliance. This builds on last week’s Trusted Publishing and security enhancements for CI/CD automation.

- [Secure NuGet Package Publishing from GitHub Actions with Trusted Publishing](https://andrewlock.net/easily-publishing-nuget-packages-from-github-actions-with-trusted-publishing/)

### AI-driven DevOps Automation and Infrastructure Management

ControlMonkey introduces KoMo AI agents for automated Terraform provisioning, policy enforcement, and code analysis. These trace dependencies, review modules, and detect misconfigurations. env zero’s Cloud Governance Platform features a Static Code Analyzer Agent for policy-compliant fixes via pull requests, using Anthropic’s MCP to orchestrate multi-cloud resources. These platforms streamline manual work and standardize policy automation, continuing last week’s expansion of MCP-driven DevOps.

- [ControlMonkey Introduces AI Agents for Infrastructure Automation](https://devops.com/controlmonkey-adds-ai-agents-to-infrastructure-automation-platform/)
- [env zero Revamps Infrastructure Automation Platform for AI Era](https://devops.com/env-zero-revamps-infrastructure-automation-platform-for-ai-era/)

### Azure SRE Agent Expands: Automation, Diagnostics, and Incident Response

Azure SRE Agent v2.0 moves into wider public preview, adding role-based access control, approval workflows, and secure automation for Azure resources. Diagnostics cover both core and specialized workloads, with incident response linking Monitor, PagerDuty, ServiceNow, GitHub, and Azure DevOps. SREs benefit from unified orchestration and Copilot-powered PR automation, transparent billing via Agent Units, and improved documentation—reinforcing recent agent-driven modernization efforts.

- [Azure SRE Agent Public Preview Expands: New Features for DevOps and Automation](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/expanding-the-public-preview-of-the-azure-sre-agent/ba/p/4458514)

### AI Adoption in DevOps: Trust, Velocity, and Pipeline Bottlenecks

Industry analysis shares that while AI use is nearly universal, mistrust and pipeline instability persist. Internal platforms report stronger results and faster delivery (63%) with AI, but incidents and deployment errors remain frequent. Less than 10% of teams have fully automated pipelines; cost and security drive modernization. This data continues last week’s discussion on balancing performance and stability, illustrating current strengths and challenges.

- [DORA 2025: AI’s Impact on DevOps — Speed, Trust, and the Platform Effect](https://devops.com/dora-2025-faster-but-are-we-any-better/)
- [Survey Reveals DevOps Bottlenecks Created by AI-Generated Code](https://devops.com/survey-surfaces-downstream-devops-challenges-created-by-ai-code/)

### AI-driven Project Management for DevOps Teams

Shortcut’s Korey AI agent automates project management by turning natural language into actionable plans, tracking dependencies, and orchestrating work based on code comments, GitHub changes, and team workflows. Direct communication helps organize sprints and monitor blockers, reducing manual work and freeing engineers for coding. Korey expands AI’s project management role, building on last week’s coverage of Copilot Coding Agent and orchestration tools.

- [Shortcut Introduces Korey AI Agent for Software Project Management](https://devops.com/shortcut-adds-ai-agent-to-orchestrate-management-of-software-development-projects/)

### AI in the DevOps Lifecycle and Quality Assurance Mindset

DevOps guides recommend integrating AI-powered monitoring, testing, and pipeline automation to improve event correlation and vulnerability management—while highlighting the importance of data quality and integration planning. Teams are urged to prioritize automation for the highest-impact tasks. Another feature underlines that combining automation with exploratory QA creates stronger pipelines—engage QA early and build resilient scripts for meaningful business value. These articles reinforce recent best practices for maintaining robust, collaborative delivery pipelines with AI integration.

- [How AI is Transforming DevOps: Smarter, Faster, and More Reliable Software Delivery](https://dellenny.com/how-ai-is-transforming-devops-smarter-faster-and-more-reliable-software-delivery/)
- [Why Automation Fails Without the Right QA Mindset](https://devops.com/why-automation-fails-without-the-right-qa-mindset-2/)

### Platform, Tooling, and Workflow Updates

VS Live! Orlando 2025 previews hands-on training in Visual Studio, .NET, DevOps, AI, ML, cloud engineering, and security. Attendees will learn about tools ranging from AI debugging and Copilot-driven DevOps to .NET Aspire microservices, continuing previous attention to productivity and agentic workflows.

- [VS Live! Orlando 2025: Deep Dive into Visual Studio, AI, DevOps, and Beyond](https://devblogs.microsoft.com/visualstudio/visual-studio-live-orlando-2025/)

### Other DevOps News

GitHub Actions cache eviction enforcement is postponed, giving teams more time for workflow optimization and aligning with earlier roundups about lifecycle management.

- [New Enforcement Date for GitHub Actions Cache Eviction Policy](https://github.blog/changelog/2025-09-29-new-date-for-enforcement-of-cache-eviction-policy)

GitHub’s web interface adds interactive, one-click merge conflict resolution, streamlining collaboration and context management—building on recent onboarding and workflow enhancements.

- [One-Click Merge Conflict Resolution in GitHub Web Interface](https://github.blog/changelog/2025-10-02-one-click-merge-conflict-resolution-now-in-the-web-interface)

MapYourGrid continues to invite users to contribute to an open energy infrastructure map using GitHub, offering opportunities for climate action through technical collaboration.

- [MapYourGrid: Contributing to an Open Map of the World's Energy Grid](/devops/videos/MapYourGrid-Contributing-to-an-Open-Map-of-the-Worlds-Energy-Grid)

## Security

Security news this week centers on practical advice for securing DevOps supply chains, AI workloads, and enterprise systems. Microsoft adds AI-driven security features and tighter governance, with ongoing incidents emphasizing risk management in automated environments.

### DevOps and Supply Chain Security

Building on previous registry security discussions, the Shai-Hulud worm incident highlights risks in the DevOps supply chain—infecting npm packages and spreading via GitHub Actions due to pipeline gaps. The event stresses the need for ephemeral credentials, workflow isolation, artifact tracking, and real-time secret scanning. Industry guides debate custom-built versus off-the-shelf supply chain protection, underlining the need for thorough engineering and validation. Harness’s Qwiet AI (ShiftLeft) acquisition continues automation of security into native developer workflows.

- [Shai-Hulud: Supply Chain Worm Sheds Light on DevOps Security Risks](https://devops.com/worms-in-the-supply-chain-shai-hulud-and-the-next-devops-reckoning/)
- [Build vs. Buy: What it Really Takes to Harden Your Software Supply Chain](https://devops.com/build-vs-buy-what-it-really-takes-to-harden-your-software-supply-chain/)
- [Harness Acquires Qwiet AI to Strengthen AI-Driven Application Security in DevOps](https://devops.com/harness-acquires-qwiet-ai-to-gain-code-testing-tool/)

### Cloud Security, AI Workload Protection, and Governance on Azure

Microsoft Sentinel evolves into an agentic SIEM, integrating unified data lakes, graph-based threat tracing, AI agents, and workflow automation—expanding extensibility with VS Code, GitHub Copilot, and Security Store integrations. Security guides detail methods for protecting Azure AI workloads by deploying multiple layers: Defender for Cloud for threats, Purview for data classification, and Sentinel for incident response. Reference architectures and automation templates ease compliance for GPU VMs, AKS clusters, and data stores. Fabric’s Outbound Access Protection for Spark restricts data exfiltration, enhancing security for analytics and ML. Microsoft Purview announces new data classification and compliance tools following last week’s advanced data loss prevention (DLP) and labeling coverage.

- [Empowering Defenders in the Era of Agentic AI with Microsoft Sentinel](https://www.microsoft.com/en-us/security/blog/2025/09/30/empowering-defenders-in-the-era-of-agentic-ai-with-microsoft-sentinel/)
- [Securing AI Workloads with Microsoft Defender for Cloud, Purview, and Sentinel in Azure Landing Zones](https://techcommunity.microsoft.com/t5/azure-architecture-blog/securing-ai-workloads-with-microsoft-defender-for-cloud-purview/ba/p/4457345)
- [Outbound Access Protection for Spark Now Generally Available in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/workspace-outbound-access-protection-for-spark-is-now-generally-available/)
- [Data Security and Governance Announcements with Talhah Mir at Microsoft Ignite](/security/videos/Data-Security-and-Governance-Announcements-with-Talhah-Mir-at-Microsoft-Ignite)

### Cloud Identity and Access Management (IAM)

Microsoft Entra decouples identity and security management from Azure, supporting unified Zero Trust for hybrid, cloud, and on-premises environments. Developers get hands-on guides for new features and secure authentication. Conditional Access policy troubleshooting reveals resource mapping gaps in Windows App/365, prompting calls for better documentation and tooling—topics covered in previous best practice guides.

- [What Microsoft Entra Really Means for Identity and Security](https://dellenny.com/what-microsoft-entra-really-means-for-identity-and-security/)
- [Conditional Access Policy Limitation: Windows 365 Portal Not Found in Target Resources](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/ca-policy-application-not-found-in-target-resources/m-p/4458834#M13916)

### Security Automation and Secret Management

GitHub secret scanning now validates credentials for Azure, MongoDB, and Meta, automating leak detection and incident response. Microsoft and HashiCorp’s best practices for Vault, Terraform, and Azure Verified Modules address identity-aware credential management, audit requirements, and privilege escalation risks in agent-based environments—continuing last week’s updates on managed identity and rotation.

- [GitHub Secret Scanning Now Validates Azure, MongoDB, and Meta Credentials](https://github.blog/changelog/2025-09-30-secret-scanning-adds-validators-for-mongodb-meta-and-microsoft-azure)
- [Securing AI Deployments with HashiCorp Vault & Azure](/ai/videos/Securing-AI-Deployments-with-HashiCorp-Vault-and-Azure)

### Advanced Security Analysis and Developer Tutorials

A step-by-step guide to debugging CodeQL queries for Gradio Python vulnerabilities demonstrates the use of custom taint flows, abstract syntax tree visualization in VS Code, and refining query outputs. These lessons expand on previous CodeQL tutorials focused on strong static analysis.

- [Debugging CodeQL Queries: Lessons from Gradio Vulnerability Research](https://github.blog/security/vulnerability-research/codeql-zero-to-hero-part-5-debugging-queries/)

### Other Security News

Development tools now offer smoother debugging and improved performance, tackling workflow bottlenecks and supporting more productive routines.

- [Debugging CodeQL Queries: Lessons from Gradio Vulnerability Research](https://github.blog/security/vulnerability-research/codeql-zero-to-hero-part-5-debugging-queries/)

Security advancements include more effective vulnerability management and compliance tools, following last week’s work on artifact signing and registry updates.

- [Build vs. Buy: What it Really Takes to Harden Your Software Supply Chain](https://devops.com/build-vs-buy-what-it-really-takes-to-harden-your-software-supply-chain/)
- [Harness Acquires Qwiet AI to Strengthen AI-Driven Application Security in DevOps](https://devops.com/harness-acquires-qwiet-ai-to-gain-code-testing-tool/)

Updated migration and troubleshooting resources continue to support teams in solving everyday technical issues.

- [Securing Modern Education with Windows 11: AI, Intune, and Learning Zone](https://www.microsoft.com/en-us/education/blog/2025/10/build-secure-future-ready-learning-experiences-with-windows-11/)
