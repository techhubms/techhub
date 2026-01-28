---
title: AI Agents and Automated Workflows Update Developer Tools and Cloud Platforms
author: Tech Hub Team
date: 2025-10-13 09:00:00 +00:00
tags:
- AI Agents
- Azure AI
- CI/CD
- Cloud Infrastructure
- Data Engineering
- Developer Tools
- Multimodal Models
- Observability
- Open Source
- Serverless Computing
- Zero Trust
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
Welcome to the weekly tech roundup, where AI agents and automation update how developers work across cloud platforms, coding tools, and data engineering. GitHub Copilot added more agent-driven capabilities, new model options, and deeper integrations with IDEs, terminals, and mobile—helping teams code, collaborate, and troubleshoot with improved speed. Updates to Visual Studio Code and Copilot CLI, alongside new tutorials, showcase how Copilot supports onboarding, accessibility, and community-driven automation efforts.

Microsoft’s cloud services made progress, with Azure AI Foundry introducing GPT-5-Codex, Sora, and open-source frameworks designed for enterprise deployment. A large-scale supercomputing cluster built with thousands of NVIDIA Blackwell GPUs enabled advanced AI workloads. Machine learning enhancements, Fabric data connectors, new security tools, and governance resources support hybrid and secure operations. DevOps and coding platforms adopted automation, revised supply chain processes, and focused on monitoring and observability, while security teams responded with focused guides for threat intelligence, Zero Trust strategies, and developer protection. Explore the summaries and links below for this week’s highlights in cloud development and automated workflows.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot: New Models, Agent Integrations, and IDE Features](#github-copilot-new-models-agent-integrations-and-ide-features)
  - [GitHub Copilot CLI: Terminal Improvements and Workflow Expansion](#github-copilot-cli-terminal-improvements-and-workflow-expansion)
  - [Copilot-Aided Case Studies and Tutorials: Apps, Automations, and Real-World Workflows](#copilot-aided-case-studies-and-tutorials-apps-automations-and-real-world-workflows)
  - [Copilot in Review and Ongoing Role Evolution](#copilot-in-review-and-ongoing-role-evolution)
  - [Supporting Education, Neurodiversity, and Sustainability with Copilot](#supporting-education-neurodiversity-and-sustainability-with-copilot)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Azure AI Foundry: Model Rollouts, Tools, Frameworks, and Security](#azure-ai-foundry-model-rollouts-tools-frameworks-and-security)
  - [Agentic AI Patterns, Enterprise Bots, and Multi-Agent Orchestration](#agentic-ai-patterns-enterprise-bots-and-multi-agent-orchestration)
  - [Infrastructure Upgrades and Enterprise AI Adoption](#infrastructure-upgrades-and-enterprise-ai-adoption)
  - [Streaming, Concurrency, and App Patterns for LLMs](#streaming-concurrency-and-app-patterns-for-llms)
  - [Graph Data Management and Analytics in Microsoft Fabric](#graph-data-management-and-analytics-in-microsoft-fabric)
  - [AI Agents, Coding Teams, and Developer Experience](#ai-agents-coding-teams-and-developer-experience)
  - [Tutorials, Learning Resources, and Hands-On Guides](#tutorials-learning-resources-and-hands-on-guides)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Microsoft Fabric Ecosystem: Streamlined Data Connectivity and Automation](#microsoft-fabric-ecosystem-streamlined-data-connectivity-and-automation)
  - [Learning and Developer Enablement: Gen AI and Edge AI Resources](#learning-and-developer-enablement-gen-ai-and-edge-ai-resources)
  - [The Emergence of Automated Data Modeling and Warehouse Modernization](#the-emergence-of-automated-data-modeling-and-warehouse-modernization)
- [Azure](#azure)
  - [Azure GB300 NVL72 Supercluster and Expansion in Asia](#azure-gb300-nvl72-supercluster-and-expansion-in-asia)
  - [Azure Logic Apps Advances](#azure-logic-apps-advances)
  - [Data Engineering, Integration, and Warehouse Developments](#data-engineering-integration-and-warehouse-developments)
  - [Governance, Security, and Landing Zones](#governance-security-and-landing-zones)
  - [Serverless Compute: Functions vs Container Apps](#serverless-compute-functions-vs-container-apps)
  - [Monitoring, Metrics, and Alerts](#monitoring-metrics-and-alerts)
  - [Migration Planning, Azure Migrate, and VM Sizing](#migration-planning-azure-migrate-and-vm-sizing)
  - [Hybrid Networking and Disaster Recovery](#hybrid-networking-and-disaster-recovery)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [Visual Studio Code: AI, Copilot, and MCP Advancements](#visual-studio-code-ai-copilot-and-mcp-advancements)
  - [Application Performance and Stability: .NET 10 GC and WPF Troubleshooting](#application-performance-and-stability-net-10-gc-and-wpf-troubleshooting)
  - [Open Source Licensing Guidance for .NET Foundation Projects](#open-source-licensing-guidance-for-net-foundation-projects)
- [DevOps](#devops)
  - [Azure DevOps Server: Modern Lifecycle and Feature Parity](#azure-devops-server-modern-lifecycle-and-feature-parity)
  - [GitHub, Dependabot, and Pull Request Workflow Migration](#github-dependabot-and-pull-request-workflow-migration)
  - [Fabric Data Agent: Integrated CI/CD and Git-Driven Workflows](#fabric-data-agent-integrated-cicd-and-git-driven-workflows)
  - [Git Merge 2025: Community, Innovation, and Future Roadmaps](#git-merge-2025-community-innovation-and-future-roadmaps)
  - [Infrastructure as Code with Pulumi and .NET](#infrastructure-as-code-with-pulumi-and-net)
  - [AI, Observability, and Supply Chain Intelligence in DevOps](#ai-observability-and-supply-chain-intelligence-in-devops)
  - [AI-Powered Automation Platforms: PagerDuty and Spacelift](#ai-powered-automation-platforms-pagerduty-and-spacelift)
  - [DevOps Culture: Internal Developer Platforms, Security, and AIOps](#devops-culture-internal-developer-platforms-security-and-aiops)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Threat Intelligence: Active Exploitation Reports and Attack Chain Analysis](#threat-intelligence-active-exploitation-reports-and-attack-chain-analysis)
  - [Practical Security Architecture and Workflow Hardening](#practical-security-architecture-and-workflow-hardening)
  - [Infrastructure and Application Security Tooling](#infrastructure-and-application-security-tooling)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot delivered a variety of updates this week, increasing AI-assisted coding support in developer workflows—including IDEs, command-line tools, and mobile apps. Improvements include new model selection features, updated productivity tools, expanded automation options, and guidance for reviewing and customizing AI-generated code. Tutorials and case studies show Copilot’s practical uses, while CLI enhancements, educational features, and bug-fix resources help developers wherever they choose to work. New agent models and command-line/chat capabilities make interaction with Copilot more flexible, and community analysis highlights how these changes impact developer experience. Copilot is steadily growing into a toolset for supporting modern development flows.

### GitHub Copilot: New Models, Agent Integrations, and IDE Features

In continuation of the previous week’s model management and context tools (such as Sonnet 4.5’s rollout), GitHub released the Grok Code Fast 1 public preview for Copilot Pro, Pro+, Business, and Enterprise plans. This expands available models and further supports agent workflows. Grok Code Fast 1 is accessible via model selectors on GitHub.com, GitHub Mobile, VS Code, Visual Studio, JetBrains IDEs, Xcode, and Eclipse, strengthening integration across development environments.

Features for model selection, enterprise policy controls, and extended feedback build on last week’s analytics and scaling support, helping organizations shape Copilot’s AI adoption to their needs. Documentation updates and request feedback maintain a focus on user input, similar to earlier community releases.

Visual Studio Code continues with persistent chat and agentic automation. New capabilities—support for Claude Sonnet 4.5, GPT-5, Grok Code Fast 1—add persistent chat, Plan Mode, and the Copilot Coding Agent. Tutorials now cover MCP server integration and remote context management, showing the expanding registry/protocol ecosystem in ongoing developer resources.

- [Grok Code Fast 1 Public Preview Launched for GitHub Copilot Plans](https://github.blog/changelog/2025-10-06-grok-code-fast-1-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [The Latest AI Features in Visual Studio Code](/ai/videos/The-Latest-AI-Features-in-Visual-Studio-Code)
- [Enhancing Developer Workflows with MCP Servers and GitHub Copilot in VS Code](/ai/videos/Enhancing-Developer-Workflows-with-MCP-Servers-and-GitHub-Copilot-in-VS-Code)

### GitHub Copilot CLI: Terminal Improvements and Workflow Expansion

Expanding on last week’s improvements in model switching, image support, and permissions, Copilot CLI now delivers additional speed and efficiency. Response times are 45% faster, completion steps are reduced, token usage is optimized, and the terminal experience is improved. This continues the shift from legacy workflow tools to a streamlined command-line approach.

Additional interface features—edit diffs, colored markdown, compact rendering—support new workflows (tab cycling, multiline input, argument hints) built on previous additions like command forwarding, context alerts, and accessibility options. PowerShell parity supports reliability across platforms. Daily npm updates allow ongoing feedback, aligning with continued community-driven feature releases.

- [An Introduction to the New GitHub Copilot CLI](/ai/videos/An-Introduction-to-the-New-GitHub-Copilot-CLI)
- [GitHub Copilot CLI: Faster, More Concise, Streamlined Developer Experience](https://github.blog/changelog/2025-10-10-github-copilot-cli-faster-more-concise-and-prettier)

### Copilot-Aided Case Studies and Tutorials: Apps, Automations, and Real-World Workflows

Recent case studies continue previous themes of fast onboarding, agent automation, and project kickstart. The Buzzword Bingo app, created with GitHub Spark and Copilot Coding Agent, uses prompt-driven development for project scaffolding—providing a direct comparison to manual and chat-based coding approaches.

Tutorials on Playwright test generation, Jupyter Notebook improvements, and automated accessibility expand last week’s coverage on documentation and developer workflows. CI/CD session examples demonstrate mobile bug fixes and build on prior onboarding, session management, and code review processes.

An article on upgrading Blazor apps in .NET 10 highlights ongoing focus on modernization, workflow analytics, and enterprise migration, positioning Copilot as a tool for compliance and iterative code improvements.

- [Building Buzzword Bingo with GitHub Spark, Copilot, and Modern Dev Tools](https://harrybin.de/posts/github-spark-buzzword-bingo/)
- [Vibe Coding a Podcast Analytics Dashboard with GitHub Copilot and AI](https://devblogs.microsoft.com/blog/complete-beginners-guide-to-vibe-coding-an-app-in-5-minutes)
- [Generating Reliable Tests with AI and Copilot in Playwright](/ai/videos/Generating-Reliable-Tests-with-AI-and-Copilot-in-Playwright)
- [VS Code Live: Leveraging GitHub Copilot and Agents in Jupyter Notebooks](/ai/videos/VS-Code-Live-Leveraging-GitHub-Copilot-and-Agents-in-Jupyter-Notebooks)
- [Upgrading eShop with Blazor in .NET 10 Using GitHub Copilot](/ai/videos/Upgrading-eShop-with-Blazor-in-NET-10-Using-GitHub-Copilot)
- [How GitHub Copilot Automated Accessibility Governance at GitHub](https://github.blog/ai-and-ml/github-copilot/how-we-automated-accessibility-compliance-in-five-hours-with-github-copilot/)
- [Completing Urgent Fixes Remotely Using GitHub Copilot Coding Agent and Mobile](https://github.blog/developer-skills/github/completing-urgent-fixes-anywhere-with-github-copilot-coding-agent-and-mobile/)

### Copilot in Review and Ongoing Role Evolution

Reflections on code review and developer roles continue last week’s focus on best practices, documentation automation, and increased agent-driven workflows. A guide for reviewing AI-generated .NET code and an analysis of developer roles emphasize effective review cycles, team collaboration, and portfolio management, shaped by Copilot’s AI support for code orchestration. Copilot Spaces and Code Review features illustrate the ongoing move to flexible, iterative developer workflows.

- [Reviewing AI-Generated Code in .NET: Best Practices for Developers](https://devblogs.microsoft.com/dotnet/developer-and-ai-code-reviewer-reviewing-ai-generated-code-in-dotnet/)
- [The Developer Role Is Evolving: How to Stay Ahead with AI and GitHub Copilot](https://github.blog/ai-and-ml/the-developer-role-is-evolving-heres-how-to-stay-ahead/)

### Supporting Education, Neurodiversity, and Sustainability with Copilot

Education with Copilot expands through curriculum integration and feedback tools, building on last week’s Study Buddy Agent and sustainability topics. New material on neurodiversity extends previous personal stories and workflow evaluations, offering advice for developers with diverse learning requirements.

Examples from City Energy Analyst and MapYourGrid show Copilot supporting social impact and open source collaboration, continuing last week’s climate and community features. Podcast discussions highlight how AI is changing experimental workflows and automation skills, following ongoing learning and adaptation strategies.

- [Study Buddy: Learning Data Science and Machine Learning with an AI Sidekick](https://techcommunity.microsoft.com/t5/microsoft-developer-community/study-buddy-learning-data-science-and-machine-learning-with-an/ba/p/4460144)
- [ADHD, GitHub Copilot, and Neurodiversity: Real Talk with Mads Torgersen and Klaus Loffelmann](/ai/videos/ADHD-GitHub-Copilot-and-Neurodiversity-Real-Talk-with-Mads-Torgersen-and-Klaus-Loffelmann)
- [Using GitHub Copilot to Advance Sustainable City Design with Open Source](/ai/videos/Using-GitHub-Copilot-to-Advance-Sustainable-City-Design-with-Open-Source)
- [Using GitHub Copilot to Map Electricity Gaps in Zambia with MapYourGrid](/ai/videos/Using-GitHub-Copilot-to-Map-Electricity-Gaps-in-Zambia-with-MapYourGrid)
- [How AI Is Changing the Way Developers Learn to Code: A Conversation with Scott Tolinski](/ai/videos/How-AI-Is-Changing-the-Way-Developers-Learn-to-Code-A-Conversation-with-Scott-Tolinski)

### Other GitHub Copilot News

New tutorials on prompt-driven code generation in VS Code expand last week’s work with documentation and chat workflows, improving Copilot’s conversational automation approach through constant feedback.

A key update announces that Claude Sonnet 3.5 will be deprecated from Copilot, starting a new round of model standardization and upgrades—continuing the process established by previous migration and selector guides.

- [Build a Responsive UI through Prompt Driven Development with GitHub Copilot](/ai/videos/Build-a-Responsive-UI-through-Prompt-Driven-Development-with-GitHub-Copilot)
- [Deprecation of Claude Sonnet 3.5 in GitHub Copilot: What You Need to Know](https://github.blog/changelog/2025-10-07-upcoming-deprecation-of-claude-sonnet-3-5)

## AI

Microsoft’s AI ecosystem made progress this week with Azure AI Foundry’s new releases, updated developer tools, and infrastructure enhancements. Advances include updated models, agent frameworks, workflow orchestration, and documentation for multimodal generation, voice solutions, and agent scaling. The highlights feature GPT-5-Codex and Sora in Azure AI Foundry, previews of multimodal models, new automation tools, and security/compliance resources. Investments in infrastructure and improved developer experience remain essential, supporting open source practices and accessible adoption paths for teams.

### Azure AI Foundry: Model Rollouts, Tools, Frameworks, and Security

Following last week’s Grok 4 preview and protocol adoption, Azure AI Foundry’s September update introduces the general availability of GPT-5-Codex for advanced code use-cases and migration tasks. Sora’s preview provides video-to-video editing with natural language and extends prior multimodal agent workflows.

Grok 4 Fast models are now in preview, supporting parallel calls and longer context sessions. Foundry Local v0.7 adds dynamic NPU discovery for hybrid deployments, continuing improvements in local/cloud AI. Microsoft Agent Framework has been open sourced for multi-agent orchestration and includes Semantic Kernel capabilities for enterprise applications.

Enhancements like browser automation, Azure AI Search improvements, and Key Vault integration support better workflow management and security needs. Updates to documentation and SDKs reflect ongoing developer feedback, echoing previous onboarding and integration efforts. Model documentation and pricing roll out beginning October 7.

- [What’s New in Azure AI Foundry: September 2025 Feature Roundup](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-september-2025/)
- [Azure AI Foundry Launches Multimodal AI Models for Developers](https://azure.microsoft.com/en-us/blog/unleash-your-creativity-at-scale-azure-ai-foundrys-multimodal-revolution/)

### Agentic AI Patterns, Enterprise Bots, and Multi-Agent Orchestration

New guides detail how to use agentic AI in enterprise scenarios by combining Azure tools, GPT models, and services for autonomous decision-making. Strategies for enhancing Copilot bots with Azure OpenAI Services employ RAG pipelines, vector databases, and the GPT Assistants API, building from recent tutorials.

The Microsoft Agent Framework, now open source, connects features from Semantic Kernel and AutoGen for broader .NET adoption. Tutorials help developers set up agent orchestration, app intelligence, and operational lifecycle management for new and migrating applications.

Enterprise scenarios—including bots and compliance agents—have updated guides for best practices and architecture expansion.

- [Enhancing Copilot Bots with Azure OpenAI Services](https://dellenny.com/enhancing-copilot-bots-with-azure-openai-services/)
- [How Agentic AI Works and How to Build It in Azure](https://dellenny.com/how-agentic-ai-works-and-how-to-build-it-in-azure/)
- [Semantic Kernel and Microsoft Agent Framework: Evolution and Future Support](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-and-microsoft-agent-framework/)
- [Getting Started with the Microsoft Agent Framework in .NET](/ai/videos/Getting-Started-with-the-Microsoft-Agent-Framework-in-NET)

### Infrastructure Upgrades and Enterprise AI Adoption

Microsoft introduced a new supercomputing cluster with 4,600+ NVIDIA GB300 GPUs and InfiniBand networking, improving AI training and inference at large scales. This builds on infrastructure for Copilot, ChatGPT, and enterprise applications, aimed at enhanced reliability and development speed.

Enterprise AI resources expand last week’s Azure AI Landing Zone guidance, covering identity management, compliance, and modular deployments. Step-by-step guides support teams from pilot to production, continuing focus on secure scaling.

- [Microsoft Unveils Supercomputing Cluster with 4600+ NVIDIA GB300 GPUs for Next-Gen AI Workloads](https://www.linkedin.com/posts/satyanadella_another-first-for-our-ai-fleet-a-supercomputing-activity-7382086397152858113-BlSd)
- [Accelerating Enterprise AI Adoption with Azure AI Landing Zone](https://techcommunity.microsoft.com/t5/azure-architecture-blog/accelerating-enterprise-ai-adoption-with-azure-ai-landing-zone/ba-p/4460199)
- [How to Build Frontier AI Solutions with Azure AI Landing Zone](https://techcommunity.microsoft.com/t5/azure-architecture-blog/how-to-build-frontier-ai-solutions-with-azure-ai-landing-zone/ba-p/4460199)

### Streaming, Concurrency, and App Patterns for LLMs

Developers aiming for better responsiveness in LLM-powered chat apps receive guidance on streaming, backend relays, NDJSON formats, and token management, continuing earlier focus on parallel processing and prompt engineering.

Concurrency recommendations build on last week’s asynchronous processing advice, highlighting frameworks such as Quart for non-blocking Azure LLM apps. Open repositories and performance advice contribute to continued improvement for scalable developer tools.

- [How to Implement Streaming in Azure LLM-Powered Chat Applications](https://techcommunity.microsoft.com/t5/microsoft-developer-community/the-importance-of-streaming-for-llm-powered-chat-applications/ba-p/4459574)
- [Concurrency Best Practices for LLM-Powered Apps with Azure OpenAI and Python](https://techcommunity.microsoft.com/t5/microsoft-developer-community/why-your-llm-powered-app-needs-concurrency/ba-p/4459584)

### Graph Data Management and Analytics in Microsoft Fabric

Microsoft Fabric adds graph data management and analytics features, supporting advanced relationship modeling and queries within OneLake. These tools expand on last week’s ETL and analytics developments and are designed for explainable AI, fraud detection, and real-time analysis.

- [Introducing Graph Data Management and Analytics in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/graph-in-microsoft-fabric/)

### AI Agents, Coding Teams, and Developer Experience

Agent platforms see continued growth with the general availability of Atlassian Rovo Dev, an automation agent for development planning and review integrated with Atlassian tools. MCP and Teamwork Graph expand workflow and collaboration features, reflecting previous progress in agentic automation.

Frameworks like AutoGen, MetaGPT, CrewAI, and Claudeflow become more common for modular, role-based orchestration. Advice to maintain oversight with automation continues from earlier best practices for trust and transparency.

Tool fragmentation gets attention with solutions for unified development and clearer transparency, as highlighted in earlier onboarding and productivity features.

- [Atlassian Rovo Dev: AI Coding Agent Now Generally Available](https://devops.com/atlassian-makes-ai-coding-agent-generally-available/)
- [Coding Agent Teams: The Next Frontier in AI-Assisted Software Development](https://devops.com/coding-agent-teams-the-next-frontier-in-ai-assisted-software-development/)
- [Developer Experience: Overcoming 6 AI-Induced Challenges](https://devops.com/developer-experience-overcoming-6-ai-induced-challenges/)

### Tutorials, Learning Resources, and Hands-On Guides

Developer resources continue to expand, featuring a nine-part video series on generative AI and Python, building on last week’s agentic Python guides and context management. Office hours and live sessions foster community learning.

Entry-level guides show how to build Azure AI Foundry agents using file search, advancing from last week’s no-code onboarding with easier setup. Additional tutorials for bots and speech cover enterprise NLP and voice solutions.

Copilot Studio’s use cases in financial services follow last week’s enterprise engagement stories.

- [Curso gratuito: IA Generativa y Python - Serie de 9 transmisiones en vivo](https://techcommunity.microsoft.com/t5/microsoft-developer-community/curso-oficial-y-gratuito-de-genai-y-python/ba-p/4459466)
- [Create an AI Agent with File Search in Azure AI Foundry (Portal)](/ai/videos/Create-an-AI-Agent-with-File-Search-in-Azure-AI-Foundry-Portal)
- [Unlocking the Power of Conversational AI with Azure Bot Service](https://dellenny.com/unlocking-the-power-of-conversational-ai-with-azure-bot-service/)
- [Noise-Free, Domain-Specific Voice Recognition with Azure Custom Speech](https://dellenny.com/when-words-matter-noise-free-domain-specific-voice-recognition-with-azure-custom-speech/)
- [AMA: Building Smarter Voice Agents with Azure AI Foundry Voice Live API](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ama-azure-ai-foundry-voice-live-api-build-smarter-faster-voice/ba-p/4460118)
- [How Copilot Studio Improves Customer Engagement in Financial Services](https://dellenny.com/how-copilot-studio-improves-customer-engagement-in-financial-services/)

### Other AI News

Grafana Labs upgraded its observability platform with AI-driven troubleshooting, root cause analysis, and adaptive telemetry. These changes mirror trends in AI-powered monitoring, adding new compliance, discoverability, and efficiency options for scalable operations.

- [Grafana Labs Extends AI Capabilities of Observability Platform](https://devops.com/grafana-labs-extends-ai-capabilities-of-observability-platform/)

## ML

Machine learning updates this week focused on streamlined data engineering, automation, security, and developer skill-building resources across platforms. Microsoft Fabric’s toolset expanded Spark and SQL integrations. New documentation and security improvements, connectors, and educational material help developers working on analytics, edge AI, and automated data workflows.

### Microsoft Fabric Ecosystem: Streamlined Data Connectivity and Automation

Building on last week’s Dataflow Gen2 governance, Microsoft Fabric offers a Spark Connector for SQL Databases in preview, improving Spark workloads with direct access to Azure SQL, Managed Instance, and Fabric SQL. This simplifies ETL and ML for PySpark and Scala, continuing support for secure, enterprise standards.

OPENROWSET now lets users set named sources and relative file paths, replacing GUIDs for clear SQL and easier troubleshooting, furthering recent operational efficiency.

Service Principal support in Semantic Link enables scalable, secure automation of pipelines—continuing previous enhancements in permission and identity management. Azure AD managed identities and Key Vaults support role-based data jobs through the "sempy.fabric" package.

- [Fabric Spark Connector for SQL Databases Now Available (Preview)](https://blog.fabric.microsoft.com/en-US/blog/spark-connector-for-sql-databases-preview/)
- [Service Principal Support in Semantic Link: Enabling Scalable, Secure Automation](https://blog.fabric.microsoft.com/en-US/blog/service-principal-support-in-semantic-link-enabling-scalable-secure-automation/)
- [Simplifying File Access in OPENROWSET: Data Sources and Relative Paths (Preview)](https://blog.fabric.microsoft.com/en-US/blog/simplifying-file-access-in-openrowset-using-data-sources-and-relative-paths-preview/)

### Learning and Developer Enablement: Gen AI and Edge AI Resources

Expanded developer support includes a nine-part YouTube series from Pamela Fox, covering generative AI, prompt engineering, RAG, agent frameworks, and live code demonstrations with OpenAI SDK and Azure AI Search—following last week’s collaborative engagement and analytics in Fabric ML.

A new edge AI curriculum covers Windows AI PC and hardware deployment with ONNX Runtime, DirectML, and Olive, advancing last week’s hybrid architecture support. Practical samples address IoT and automation scenarios for NPUs and Azure connections.

- [Level Up Your Python Gen AI Skills: Nine-Part YouTube Stream Series](https://techcommunity.microsoft.com/t5/microsoft-developer-community/level-up-your-python-gen-ai-skills-from-our-free-nine-part/ba-p/4459464)
- [Building Smarter Edge AI with Windows AI PCs: The Edge AI for Beginners Curriculum](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-cloud-to-chip-building-smarter-ai-at-the-edge-with-windows/ba/p/4459582)

### The Emergence of Automated Data Modeling and Warehouse Modernization

Flow.BI’s AI-powered data modeling adopts metadata-driven automation, supporting model generation, relationship inference, multilingual metadata, mesh configuration, and robust security. This continues last week’s focus on metadata management, helping modernize data warehousing for organizations adapting their architecture.

- [Defining the Raw Data Vault with Artificial Intelligence: Automating Data Warehouse Modeling](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/defining-the-raw-data-vault-with-artificial-intelligence/ba-p/4453557)

## Azure

Azure’s updates include new infrastructure support for AI workloads, improved data engineering and monitoring, cost optimization features, expanded hybrid networking, and additional region deployments in Asia. GPU clusters received expansion for demanding models, while updates for Logic Apps, data integration, governance, and developer tools provide blueprints for hybrid and evolving cloud workloads.

### Azure GB300 NVL72 Supercluster and Expansion in Asia

The GB300 NVL72 supercluster officially supports OpenAI workloads with over 4,600 Blackwell Ultra GPUs, enabling training for very large models. ND GB300 v6 instances and orchestration tools continue progress from earlier hybrid and edge integration efforts. Azure's reach expands in Asia (Malaysia, Indonesia, India, Taiwan), furthering region-specific compliance and availability.

- [Microsoft Azure Launches GB300 NVL72 Supercluster for OpenAI Workloads](https://azure.microsoft.com/en-us/blog/microsoft-azure-delivers-the-first-large-scale-cluster-with-nvidia-gb300-nvl72-for-openai-workloads/)
- [Microsoft Expands Azure Cloud and AI Infrastructure Across Asia](https://azure.microsoft.com/en-us/blog/microsofts-commitment-to-supporting-cloud-infrastructure-demand-in-asia/)

### Azure Logic Apps Advances

Logic Apps expands event-driven cloud automation with the Aviators Newsletter, previewing MCP server adoption. Python Code Interpreter support reinforces ongoing improvements to agent workflows, while Copilot Studio enhancements and expert guides remain focused on hybrid migration, secure governance, and Teams automation.

- [Logic Apps Aviators Newsletter - October 25](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-aviators-newsletter-october-25/ba/p/4458456)

### Data Engineering, Integration, and Warehouse Developments

Guides highlight secure cloud architecture and Fabric integration, including the Medallion Architecture for Databricks with managed identities and Key Vault. Fabric migration tooling further supports Synapse Analytics transitions. New JSONL management and concurrency features add ML workload flexibility, and Delta Lake compaction helps cut operational costs, continuing automation improvements.

- [Secure Medallion Architecture Pattern on Azure Databricks (Part I)](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/secure-medallion-architecture-pattern-on-azure-databricks-part-i/ba/p/4459268)
- [Enterprise-Scale Data Integration with Azure Data Factory, SQL Managed Instance, and SSIS](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/azure-data-factory-sql-managed-instance-and-ssis-implementation/ba/p/4459525)
- [Migration Assistant for Fabric Data Warehouse: General Availability Announced](https://blog.fabric.microsoft.com/en-US/blog/announcing-general-availability-of-migration-assistant-for-fabric-data-warehouse/)
- [Query and Ingest JSONL Files in Fabric Data Warehouse and SQL Endpoint](https://blog.fabric.microsoft.com/en-US/blog/query-and-ingest-jsonl-files-in-data-warehouse-and-sql-endpoint-for-lakehouse-general-availability/)
- [Understanding Locking and DDL Blocking in Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/locking-and-ddl-blocking-behavior-in-microsoft-fabric-data-warehouse-what-you-need-to-know/)
- [Resolving Write Conflicts in Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/concurrency-control-and-conflict-resolution-in-microsoft-fabric-data-warehouse/)
- [Introducing Optimized Compaction in Fabric Spark](https://blog.fabric.microsoft.com/en-US/blog/announcing-optimized-compaction-in-fabric-spark/)

### Governance, Security, and Landing Zones

Updates on landing zones and AMBA-ALZ patterns support improved cloud governance and compliance. Guides for deploying enterprise-scale landing zones use recent Azure AI advances for centralized policy automation and monitoring. AMBA-ALZ pattern upgrades bolster policy and RBAC. Tutorials on Azure Arc, including secure onboarding for air-gapped setups, address priorities for multicloud and hybrid deployments.

- [Building an Azure Enterprise-Scale Landing Zone: Foundation for Cloud Governance](https://dellenny.com/azure-enterprise-scale-landing-zone-building-a-future-ready-cloud-foundation/)
- [AMBA-ALZ Pattern: Recent Enhancements to Built-in Policies and Role-Based Access in Azure](https://techcommunity.microsoft.com/t5/azure-governance-and-management/amba-alz-pattern-learn-about-the-latest-and-greatest/ba/p/4458320)
- [Architectural Patterns and Secure Onboarding for Azure Arc in Air-Gapped Environments](https://techcommunity.microsoft.com/t5/azure-arc-blog/addressing-air-gap-requirements-through-secure-azure-arc/ba/p/4458748)

### Serverless Compute: Functions vs Container Apps

Comparison guides for Functions and Container Apps help developers choose between serverless options for microservices and event-driven apps, continuing last week’s support for containerization and workflow redesign.

- [Azure Functions vs Azure Container Apps: Choosing Your Serverless Compute Option](https://dellenny.com/azure-functions-vs-azure-container-apps-choosing-your-serverless-compute/)

### Monitoring, Metrics, and Alerts

Azure Monitor Workspaces launch resource-scope queries in public preview, providing finer RBAC and dashboard options. Expanded Prometheus/Grafana support and region-scoped endpoints continue earlier monitoring and analytics improvements. Azure Storage alerts and metrics offer centralized alerting for more efficient operations.

- [Announcing Resource-Scope Query for Azure Monitor Workspaces](https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-resource-scope-query-for-azure-monitor-workspaces/ba/p/4460567)
- [Introducing Cross Resource Metrics and Alerts Support for Azure Storage](https://techcommunity.microsoft.com/t5/azure-storage-blog/introducing-cross-resource-metrics-and-alerts-support-for-azure/ba/p/4459193)

### Migration Planning, Azure Migrate, and VM Sizing

Updates to Azure Migrate improve cost modeling and migration workflows for B-Series and Cobalt 100 ARM64 VMs, and add support for Customer Agreement pricing. These changes help hybrid and dev/test environments with rightsizing and automated migration options.

- [Azure Migrate Adds Support for Microsoft Customer Agreement Pricing](https://techcommunity.microsoft.com/t5/azure-migration-and/empower-your-migration-decisions-with-negotiated-agreements-ea/ba/p/4459425)
- [Cut migration costs with B-Series and Cobalt 100 VM support in Azure Migrate](https://techcommunity.microsoft.com/t5/azure-migration-and/cut-migration-costs-with-b-series-and-cobalt-100-vm-support-in/ba/p/4460285)

### Hybrid Networking and Disaster Recovery

A new video guides connecting Azure VNets to AWS, GCP, Oracle, and other clouds, extending previous coverage of hybrid/cloud edge developments. Disaster recovery documentation using Azure Site Recovery and VMware helps teams prepare backup and failover strategies for reliable operations.

- [Hybrid Cloud Networking: Connecting Azure, AWS, GCP, and More](/azure/videos/Hybrid-Cloud-Networking-Connecting-Azure-AWS-GCP-and-More)
- [Replicating VMware Workloads to Azure with Azure Site Recovery for Disaster Recovery](https://techcommunity.microsoft.com/t5/azure/replicate-workload-from-vmware-to-azure-using-azure-site/m-p/4460851#M22268)

### Other Azure News

Recent SQL database upgrades in Microsoft Fabric (Copilot, virtualization) continue changes in analytics and Power BI. Visual Studio subscription advice for Azure Dev/Test Benefit helps developers optimize workflow costs. Cost control reports advance FinOps resources. John Savill’s Azure update pulls together monitoring, AKS automation, and Copilot news for quick review. The Azure Pricing Calculator and tutorial help users plan and estimate resources. Azure CycleCloud’s academic HPC improvements build on previous work for cloud-based research and scaling.

- [SQL Database Enhancements in Microsoft Fabric: Copilot, Data Virtualization, and More](/ai/videos/SQL-Database-Enhancements-in-Microsoft-Fabric-Copilot-Data-Virtualization-and-More)
- [Understanding the Azure Dev/Test Benefit for Visual Studio Subscribers](https://devblogs.microsoft.com/visualstudio/visual-studio-dev-test-benefit-explained/)
- [5 Azure Mistakes That Are Costing Businesses Thousands](https://dellenny.com/5-azure-mistakes-that-are-costing-businesses-thousands/)
- [Azure Update - 10th October 2025](/ai/videos/Azure-Update-10th-October-2025)
- [Azure Pricing Calculator: Estimate Smarter, Plan Confidently](/azure/videos/Azure-Pricing-Calculator-Estimate-Smarter-Plan-Confidently)
- [Future-Proofing Academic Research: HPC Workflows and Secure Enclaves with Azure CycleCloud](/azure/videos/Future-Proofing-Academic-Research-HPC-Workflows-and-Secure-Enclaves-with-Azure-CycleCloud)

## Coding

Coding news this week centers on practical updates for developer workflows, including platform releases, debugging, open source compliance, AI-supported code assistance, productivity tools, improved .NET memory management, and guidance for licensing in long-term open source projects.

### Visual Studio Code: AI, Copilot, and MCP Advancements

VS Code’s September 2025 update (v1.105) carries forward agentic automation and collaborative MCP workflows from last week. In-editor AI-powered merge conflict resolution uses chat agents, advancing previous source control features.

Chain-of-thought debugging and improved session management for Copilot Chat enhance transparency and explainability, responding to earlier developer feedback. BYOK model previews add workflow flexibility, continuing registry and protocol expansion.

The MCP marketplace preview in Extensions follows registry improvements, making MCP server discovery easier. Customization and agent development become more central as protocol use expands.

Shell and terminal configuration updates, pull request enhancements, and agent-driven validation integrate with core test tooling—in line with past agentic workflow updates. These changes reinforce VS Code’s community-based, developer-driven evolution.

- [Visual Studio Code September 2025 Release (v1.105): AI, MCP, and Developer Enhancements](https://code.visualstudio.com/updates/v1_105)

### Application Performance and Stability: .NET 10 GC and WPF Troubleshooting

Prepping for .NET 10, developers get a deep dive into DATAS GC, which adapts heap sizing for real memory use—targeting containers, Kubernetes, and web apps. Configuration, performance tuning, and diagnostic instructions support safe migration.

For WPF app unresponsiveness with USB pen devices, guidance recommends disabling stylus/touch handlers using an AppContext switch; apps that require these features will need other fixes. Microsoft’s findings point to memory and deadlock issues, with a permanent solution yet to arrive.

- [Preparing for the .NET 10 GC: Understanding and Tuning DATAS](https://devblogs.microsoft.com/dotnet/preparing-for-dotnet-10-gc/)
- [WPF Apps Becoming Unresponsive after USB Pen Device Hotplug](https://techcommunity.microsoft.com/t5/app-development/wpf-application-becomes-unresponsive-after-plugging-unplugging/m-p/4459751#M1274)

### Open Source Licensing Guidance for .NET Foundation Projects

The .NET Foundation clarified its licensing guide, stating only permissive OSI licenses (MIT, Apache 2.0, BSD, ISC) are allowed for main code and dependencies. Copyleft (GPL, AGPL, RPL) is not accepted to avoid issues in commercial applications. The document discusses project governance, corrects myths, and lists monetization options, citing AutoMapper as a past example. Maintainers and teams can use compliance checks and scenario reviews in policy updates for sustainable open source projects.

- [.NET Foundation License Compatibility Guide](https://dotnetfoundation.org/news-events/detail/license-compatibility-guide)

## DevOps

DevOps updates featured expanded automation capabilities, new lifecycle management, observability tools, and strategy resources. Platform changes, workflow migration guides, and studies on technical debt and supply chain improvements focus on reliability, developer productivity, and streamlined processes.

### Azure DevOps Server: Modern Lifecycle and Feature Parity

Azure DevOps Server’s release candidate closes the gap with the cloud service by adding TFX validation, new REST APIs for test recovery, and a supported upgrade path from TFS 2015+. Microsoft transitions to a Modern Lifecycle Policy—favoring ongoing updates over scheduled releases—and updates branding for easier support. Hybrid and on-premises teams receive improved upgrade, support, and feature access.

- [Azure DevOps Server RC Release: New Features, Lifecycle Policy, and Branding Updates](https://devblogs.microsoft.com/devops/announcing-the-new-azure-devops-server-rc-release/)

### GitHub, Dependabot, and Pull Request Workflow Migration

GitHub’s Dependabot will deprecate pull request comment commands (such as `@dependabot merge/close`) in Nov 2025/Jan 2026, requiring migration to web UI, CLI, and REST APIs. Automatic warnings and step-by-step migration guides help teams update CI/CD scripts, prioritizing security and reliability. These changes continue last week’s workflow migration agenda aimed at standardizing automation features.

- [Dependabot Pull Request Comment Command Deprecations and Migration to GitHub Native Features](https://github.blog/changelog/2025-10-06-upcoming-changes-to-github-dependabot-pull-request-comment-commands)
- [Deprecation of Dependabot Pull Request Comment Commands in Favor of GitHub Native Features](https://github.blog/changelog/2025-10-07-upcoming-changes-to-github-dependabot-pull-request-comment-commands)

### Fabric Data Agent: Integrated CI/CD and Git-Driven Workflows

Fabric’s data agents now offer integrated CI/CD, ALM, and Git for Lakehouse, Warehouse, Power BI Models, and KQL databases. Version control, rollback, staging, and reviews are managed in standard repos, making changes traceable and release processes structured for modern DevOps. Onboarding links guide teams through new source-controlled flows.

- [Fabric Data Agent: CI/CD, ALM Flow, and Git Integration Enhancements](https://blog.fabric.microsoft.com/en-US/blog/fabric-data-agent-now-supports-ci-cd-alm-flow-and-git-integration/)

### Git Merge 2025: Community, Innovation, and Future Roadmaps

Git Merge 2025 celebrated Git’s 20th anniversary at GitHub HQ, where contributors discussed branches, SHA-256 compatibility, visualization tools, and roadmap features. Community management and optimization remain key, impacting daily developer activities.

- [Git Merge 2025: Celebrating 20 Years of Git at GitHub HQ](https://github.blog/open-source/git/20-years-of-git-2-days-at-github-hq-git-merge-2025-highlights-%f0%9f%8e%89/)

### Infrastructure as Code with Pulumi and .NET

A new On .NET Live session explains how Pulumi brings .NET languages (C#/F#) to infrastructure automation across Azure, AWS, and Google Cloud. The episode covers configuration, CLI integration, and onboarding, providing a simple introduction to code-based infrastructure management.

- [Infrastructure as Code with Pulumi Using .NET Languages](/coding/videos/Infrastructure-as-Code-with-Pulumi-Using-NET-Languages)

### AI, Observability, and Supply Chain Intelligence in DevOps

Recent articles examine how AI and graph intelligence improve DevOps visibility and supply chain mapping. ‘DevGraphIntelOps’ was featured at swampUP 2025, highlighting graph analytics for dependency tracking and risk management. AI-driven pipelines support error detection, monitoring, and reliability. New Relic’s survey shows better observability reduces outages and incidents; expanded supply chain intelligence and smart CI/CD promote effective troubleshooting and automation.

These trends build on last week’s move toward agent-powered observability.

- [How Graph Intelligence Is Transforming Software Supply Chain Visibility](https://devops.com/how-graph-intelligence-is-transforming-software-supply-chain-visibility/)
- [Full-Stack Observability and AI: Mitigating IT Outage Costs](https://devops.com/report-full-stack-observability-cuts-downtime-costs/)
- [How AI Enhances DevOps Pipelines for Smarter Automation](https://devops.com/how-can-the-usage-of-ai-help-boost-devops-pipelines/)

### AI-Powered Automation Platforms: PagerDuty and Spacelift

PagerDuty’s new AI agents automate incident response, runbooks, transcription, scheduling, and integrate with external data protocols (MCP servers). Spacelift introduced Spacelift Intent, an open source AI tool using Anthropic’s MCP, which translates requests into API calls for infrastructure setups. These platforms simplify recurring tasks and speed up engineering, adding to current agent automation efforts in DevOps.

- [PagerDuty Introduces Suite of AI Agents to Enhance IT Management and DevOps Workflows](https://devops.com/pagerduty-adds-suite-of-ai-agents-to-it-management-platform/)
- [Spacelift Unveils Open Source AI Vibecoding Tool for Infrastructure Provisioning](https://devops.com/spacelift-adds-open-source-ai-vibecoding-tool-to-provision-infrastructure/)

### DevOps Culture: Internal Developer Platforms, Security, and AIOps

Analysis explores the advantages of Internal Developer Platforms (IDPs), AIOps, and DevSecOps for efficiency and autonomy. IDPs automate routine work, freeing developers for project goals. DevSecOps and AIOps require new skills and shift roles. GitOps, plug-ins, and unified tools continue to support speed and transparency. MLOps fosters collaboration between operations and data teams, guiding lifecycle and governance efforts.

- [How IDPs, AI, and Security Are Evolving DevOps Culture](https://devops.com/whose-ops-is-it-anyway-how-idps-ai-and-security-are-evolving-developer-culture/)

### Other DevOps News

A guide explains reducing repetitive DevOps work with Bash, Python, or PowerShell scripts—starting small and growing best practices. Advice on technical debt covers ways to improve developer satisfaction through roles like DX champion or using AI agents. A Chainguard survey highlights the challenges of tool sprawl and switching, underscoring the need for unified workflows and tools for developer well-being.

- [How to Eliminate DevOps Toil Using Automation Scripts](https://devops.com/how-to-eliminate-devops-toil-using-automation-scripts/)
- [Technical Debt: Make Developers Happier Now or Pay More Later](https://devops.com/technical-debt-make-developers-happier-now-or-pay-more-later/)
- [Survey Highlights Challenges and Opportunities in Software Engineering and DevOps](https://devops.com/survey-surfaces-lots-of-room-for-software-engineering-improvement/)

## Security

Security topics this week center on threat investigation, updated implementation patterns, and developer tooling to keep codebases secure. Reports detail emergency vulnerability exploitation and targeted cloud attacks; step-wise guides show how organizations can build secure environments, lower risks, and improve developer protection with static analysis, secrets scanning, and policy choices.

### Threat Intelligence: Active Exploitation Reports and Attack Chain Analysis

Microsoft investigated Storm-1175’s exploitation of CVE-2025-10035 in GoAnywhere MFT (pre-v7.8.3)—using deserialization attacks, remote management, ransomware, and credential theft. The article shares attack breakdowns, detection indicators, hunting queries, and upgrade recommendations.

Storm-2657’s "Payroll Pirate" targets US universities through Workday HR, using adversary-in-the-middle attacks to steal credentials and divert payroll. Guidance includes immediate credential resets, multi-factor authentication, device cleaning, and inbox review. Automated queries support rapid cloud response workflows, continuing last week’s focus on CI/CD and SaaS risk isolation.

- [Investigating Active Exploitation of CVE-2025-10035 in GoAnywhere Managed File Transfer](https://www.microsoft.com/en-us/security/blog/2025/10/06/investigating-active-exploitation-of-cve-2025-10035-goanywhere-managed-file-transfer-vulnerability/)
- [Investigating Storm-2657 'Payroll Pirate' Attacks Targeting US Universities](https://www.microsoft.com/en-us/security/blog/2025/10/09/investigating-targeted-payroll-pirate-attacks-affecting-us-universities/)

### Practical Security Architecture and Workflow Hardening

Organizations adopt secure architecture patterns from Microsoft’s Secure Future Initiative, using network segmentation, Entra ID, Conditional Access, Zero Trust in CI/CD, and central detection/logging. Guides help users map ZTA pillars to services like Entra ID, Defender, Intune, Purview, Sentinel, and Logic Apps—covering identity, application, data, and incident response. Stepwise advice, tips, and challenges support security modernization.

Securing Teams workflows includes attack chain analysis, defense controls, and detection queries for Microsoft 365, embedding security into incident management. These guides continue progress on multi-layer defense and policy automation.

- [Microsoft Secure Future Initiative: Practical Patterns and Practices for Enhanced Security](https://www.microsoft.com/en-us/security/blog/2025/10/07/new-microsoft-secure-future-initiative-sfi-patterns-and-practices-practical-guides-to-strengthen-security/)
- [Implementing Zero Trust Architecture in an Azure Environment](https://dellenny.com/implementing-zero-trust-architecture-in-an-azure-environment/)
- [Mitigating Threats Targeting Microsoft Teams: Attack Chain and Defense Strategies](https://www.microsoft.com/en-us/security/blog/2025/10/07/disrupting-threats-targeting-microsoft-teams/)

### Infrastructure and Application Security Tooling

CodeQL 2.23.2 update adds Rust queries for URL security, improved JS/TS SDK dataflow, upgraded Python and Ruby analysis, expanded Go registry checks, and enhanced C# null detection. These changes help reduce false positives and cover a broader range of languages. Most GitHub users are auto-updated; enterprises are guided through manual upgrades.

A new article addresses five common Infrastructure-as-Code (IaC) issues (drift, policy gaps, audits, excessive permissions, hardcoded secrets) and solutions such as drift detection, OPA/Terraform policies, secret management, and audit logging in CI/CD pipelines. These best practices follow recent advice on policy management and secured cloud automation.

- [CodeQL 2.23.2 Adds Rust Security Detections and Enhanced Language Support](https://github.blog/changelog/2025-10-09-codeql-2-23-2-adds-additional-detections-for-rust-and-improves-accuracy-across-languages)
- [Common IaC Security Issues and How to Fix Them](https://devops.com/common-iac-security-issues-and-how-to-fix-them/)

### Other Security News

GitHub Enterprise Cloud now offers up to 20 Enterprise Managed User IDs in a proxy header, simplifying authentication and governance across business units—continuing improvements to centralized identity management.

GitHub secret protection adds new default credential scan patterns and strengthens push protection, moving forward with automated secrets scanning from last week.

Microsoft Fabric’s OneLake Security preview brings central RBAC and Row/Column-level SQL security, advancing workspace isolation and access control coverage.

Microsoft Ignite 2025 previews security sessions focused on agentic AI, Zero Trust, enterprise DLP, and Copilot security features, continuing discussion on agentic protection and layered defense.

- [GitHub Enterprise Cloud Access Restrictions Now Support Multiple Enterprises](https://github.blog/changelog/2025-10-06-enterprise-access-restrictions-now-supports-multiple-enterprises)
- [GitHub Secret Protection Expands Default Pattern Support – September 2025](https://github.blog/changelog/2025-10-07-secret-protection-expands-default-pattern-support-september-2025)
- [OneLake Security on the SQL Analytics Endpoint](https://blog.fabric.microsoft.com/en-US/blog/onelake-security-on-the-sql-analytics-endpoint/)
- [Securing Agentic AI and Data: Microsoft Ignite 2025 Security Sessions Preview](https://www.microsoft.com/en-us/security/blog/2025/10/09/securing-agentic-ai-your-guide-to-the-microsoft-ignite-sessions-catalog/)
