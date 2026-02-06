---
title: Advances in AI Tooling, Platform Engineering, and Security Shape This Week’s Highlights
author: TechHub
date: 2026-01-26 09:00:00 +00:00
tags:
- Agentic SDK
- App Modernization
- CI/CD
- Cloud Engineering
- Developer Tools
- Kubernetes
- Microsoft Fabric
- Open Source
- Workflow Automation
- AI
- GitHub Copilot
- ML
- Azure
- DevOps
- Security
- Roundups
- .NET
- Machine Learning
section_names:
- ai
- github-copilot
- ml
- azure
- dotnet
- devops
- security
primary_section: github-copilot
feed_name: TechHub
external_url: /all/roundups/Weekly-AI-and-Tech-News-Roundup-2026-01-26
---
Welcome to this week’s tech update, covering the latest tools and strategies affecting developer experience and enterprise IT. AI-driven agents now support production environments, with GitHub Copilot’s SDK and enhanced CLI, while Microsoft platforms automate workflows across sectors like healthcare and retail. Azure, machine learning, and Fabric updates drive improvements in performance, security, and modernization—helping teams with DevOps, supply chain enhancements, and up-to-date security. Below, we detail the week’s main releases and approaches influencing the current technology landscape.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot SDK: Powering Custom Agentic Development](#github-copilot-sdk-powering-custom-agentic-development)
  - [GitHub Copilot CLI: Enhanced Terminal Workflows and Integration](#github-copilot-cli-enhanced-terminal-workflows-and-integration)
  - [Organizational and Work Context Integration](#organizational-and-work-context-integration)
  - [App Modernization with GitHub Copilot](#app-modernization-with-github-copilot)
  - [Integrating and Expanding Copilot Use Across Platforms](#integrating-and-expanding-copilot-use-across-platforms)
  - [Advanced Workflow Guidance and Tutorials](#advanced-workflow-guidance-and-tutorials)
- [AI](#ai)
  - [Microsoft Agent Framework, Foundry, and Agentic Orchestration](#microsoft-agent-framework-foundry-and-agentic-orchestration)
  - [Open Source Agent Interoperability and Best Practices](#open-source-agent-interoperability-and-best-practices)
  - [Specification-Driven and Contextual AI Development Workflows](#specification-driven-and-contextual-ai-development-workflows)
  - [Azure AI in Healthcare, Legal Practice, and Retail Scenarios](#azure-ai-in-healthcare-legal-practice-and-retail-scenarios)
- [ML](#ml)
  - [Microsoft Fabric: Enhanced Data Engineering, Analytics, and Performance](#microsoft-fabric-enhanced-data-engineering-analytics-and-performance)
  - [Physical AI Advances: Microsoft Research’s Rho-alpha Robotics Model](#physical-ai-advances-microsoft-researchs-rho-alpha-robotics-model)
- [Azure](#azure)
  - [Azure Kubernetes Service (AKS) and Infrastructure Automation](#azure-kubernetes-service-aks-and-infrastructure-automation)
  - [Microsoft Fabric Data Integration, Real-Time Intelligence, and Analytics](#microsoft-fabric-data-integration-real-time-intelligence-and-analytics)
  - [Azure Storage: AI-Centric Platform Evolution](#azure-storage-ai-centric-platform-evolution)
  - [Azure Automation, SRE, and Incident Management](#azure-automation-sre-and-incident-management)
  - [Azure Verified Modules, Infrastructure as Code, and Platform Foundations](#azure-verified-modules-infrastructure-as-code-and-platform-foundations)
  - [Developer Tools, Testing Services, and Workflow Improvements](#developer-tools-testing-services-and-workflow-improvements)
  - [Memory Reliability and Hardware Efficiency for Azure Infrastructure](#memory-reliability-and-hardware-efficiency-for-azure-infrastructure)
  - [Application and Container Networking, Security, and Cache Optimization](#application-and-container-networking-security-and-cache-optimization)
  - [Azure Arc Server and Hybrid Cloud Updates](#azure-arc-server-and-hybrid-cloud-updates)
  - [Microsoft Data Platform Ecosystem](#microsoft-data-platform-ecosystem)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET Performance Optimization and Modern Web Development](#net-performance-optimization-and-modern-web-development)
  - [React Native Windows and Cross-Platform App Development](#react-native-windows-and-cross-platform-app-development)
  - [SharePoint Site Optimization Guides](#sharepoint-site-optimization-guides)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [GitHub Platform and Workflow Improvements](#github-platform-and-workflow-improvements)
  - [Expanding Supply Chain Security and Artifact Traceability](#expanding-supply-chain-security-and-artifact-traceability)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Security in Developer Tooling and Workflows](#security-in-developer-tooling-and-workflows)
  - [Cloud AI Security and Copilot Studio Protections](#cloud-ai-security-and-copilot-studio-protections)
  - [Microsoft Fabric and OneLake: Fine-Grained Security Management](#microsoft-fabric-and-onelake-fine-grained-security-management)
  - [Threat Intelligence: Phishing Campaigns and BEC Countermeasures](#threat-intelligence-phishing-campaigns-and-bec-countermeasures)
  - [Automating Vulnerability Detection and Management](#automating-vulnerability-detection-and-management)
  - [Secure Auth and Delegated Access Patterns in Cloud Services](#secure-auth-and-delegated-access-patterns-in-cloud-services)
  - [Enhancing Supply Chain Security: Container Image Signing](#enhancing-supply-chain-security-container-image-signing)
  - [Broader Identity and Data Security Guidance](#broader-identity-and-data-security-guidance)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot continues to add new agent-based SDK capabilities, as well as improvements for its command-line interface and tools for automating modernization. Developers can now use Copilot in different development environments, including IDEs, terminals, automated pipelines, and custom agent-based apps. The latest enhancements to the CLI, better integration with organizational information, and tools for Java modernization are all designed to provide more concrete benefits for programming teams.

### GitHub Copilot SDK: Powering Custom Agentic Development

The GitHub Copilot SDK is now in technical preview, following a recent rollout for platforms including Node.js, Python, Go, and .NET. Step-by-step guides now demonstrate how to use Copilot’s code completions inside purpose-built agent applications and automation tasks. The SDK expands on CLI capabilities with features such as agentic workflows, memory, tool orchestration, routing for multiple models, and streaming. Microsoft and contributors are offering in-depth examples for embedding agents using Python, enabling .NET in Visual Studio, and combining Copilot with external model authentication. The platform’s modular approach is suitable for organizations creating custom AI-enabled workflows and tools, and user feedback notes broader integration with internal processes.

- [Bringing Work Context to Your Code with GitHub Copilot SDK](https://devblogs.microsoft.com/blog/bringing-work-context-to-your-code-in-github-copilot)
- [Building Agentic Apps with the GitHub Copilot SDK: New Developer Paradigms](https://www.linkedin.com/posts/satyanadella_build-an-agent-into-any-app-with-the-github-activity-7420126187286568961-TdW7)
- [Build an Agent into Any App with the GitHub Copilot SDK](https://github.blog/news-insights/company-news/build-an-agent-into-any-app-with-the-github-copilot-sdk/)
- [Using the GitHub Copilot SDK with Python](/github-copilot/videos/using-the-github-copilot-sdk-with-python)
- [The GitHub Copilot SDK is here! - Rubber Duck Thursdays](/github-copilot/videos/the-github-copilot-sdk-is-here-rubber-duck-thursdays)
- [Open Source Friday: Exploring the GitHub Copilot SDK](/github-copilot/videos/open-source-friday-exploring-the-github-copilot-sdk)
- [Add an AI Agent to Your Application with GitHub Copilot SDK](/github-copilot/videos/add-an-ai-agent-to-your-application-with-github-copilot-sdk)

### GitHub Copilot CLI: Enhanced Terminal Workflows and Integration

GitHub Copilot CLI now offers new features for automatically using AI within the terminal, following updates such as support for different models (including GPT-5 mini, GPT-4.1), better installation, and agent-driven command-line tools. The updated "Plan" mode introduces guided, stepwise code planning before code is generated, which aligns with the broader move toward agentic workflows. GPT-5.2-Codex support provides improved prompt handling and context control (`/context`). The CLI now integrates with `gh copilot` for quick setup and unified onboarding for teams. Features for background task delivery (`/delegate`), persistent repository memory, review functionality, and audit/history tools all support better security practices. The YOLO mode and CI/CD automation are designed to cut repeated tasks.

- [Install and Use GitHub Copilot CLI Directly from the GitHub CLI](https://github.blog/changelog/2026-01-21-install-and-use-github-copilot-cli-directly-from-the-github-cli)
- [GitHub Copilot CLI: Plan Mode, Advanced Reasoning, and Terminal Workflow Enhancements](https://github.blog/changelog/2026-01-21-github-copilot-cli-plan-before-you-build-steer-as-you-go)
- [A Cheat Sheet to Slash Commands in GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/a-cheat-sheet-to-slash-commands-in-github-copilot-cli/)
- [Demo: Using /delegate in the GitHub Copilot CLI](/github-copilot/videos/demo-using-delegate-in-the-github-copilot-cli)
- [Demo: Using GitHub Copilot CLI and YOLO Mode](/github-copilot/videos/demo-using-github-copilot-cli-and-yolo-mode)
- [Building with GitHub Copilot CLI: Rubber Duck Thursdays Live Coding Stream](/github-copilot/videos/building-with-github-copilot-cli-rubber-duck-thursdays-live-coding-stream)

### Organizational and Work Context Integration

For distributed and enterprise development teams, Copilot now adds greater organizational context into AI features. Following last week’s update on context engineering and memory, these integrations allow Copilot to pull from Microsoft 365, SharePoint, and call transcripts via the Work IQ MCP server. You can now search organization documents and previous work history directly from the IDE or command line, which helps with requirement matching and audit needs. Persistent and auditable context handling strengthens code reviews and onboarding, which continues the trend of team-wide curation and verifiable memory functions. Teams report higher productivity and more accurate requirement handling.

- [Bringing Work Context to Your Code with GitHub Copilot SDK](https://devblogs.microsoft.com/blog/bringing-work-context-to-your-code-in-github-copilot)
- [Bringing Organizational Context to GitHub Copilot CLI with Work IQ](https://www.linkedin.com/posts/satyanadella_so-much-of-dev-work-happens-in-the-context-activity-7420485585376620544-vudJ)
- [Bringing Work Context to Your Code in GitHub Copilot](/github-copilot/videos/bringing-work-context-to-your-code-in-github-copilot)

### App Modernization with GitHub Copilot

Copilot’s modernization tools support more automation for Java and Spring upgrades. Last week’s Java EE to Jakarta EE migration is now followed by tools to automate updates for Spring Boot and Spring Framework, with step-by-step guides for both JDK upgrades and secure identity changes. These tools combine security, dependency management, and automated refactoring—including support for OpenRewrite, JDK/build planning, and Microsoft’s ID and Key Vault. This reduces friction for enterprise migrations, covering tasks like namespace changes, dependency and CVE analysis, fixing security vulnerabilities, and deeper IDE integration. Focus remains on simplifying post-migration review, automated code improvements, and cloud adoption.

- [Modernizing Spring Boot Applications with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-spring-boot-applications-with-github-copilot-app/ba-p/4486466)
- [Modernizing Applications by Migrating Code to Managed Identity with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-applications-by-migrating-code-to-use-managed/ba-p/4486481)
- [Migrating Application Credentials to Azure Key Vault with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/migrating-application-credentials-to-azure-key-vault-with-github/ba-p/4486482)
- [Modernizing Spring Framework Applications with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-spring-framework-applications-with-github-copilot/ba-p/4486469)
- [Upgrade Your Java JDK (8, 11, 17, 21, or 25) with GitHub Copilot App Modernization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/upgrade-your-java-jdk-8-11-17-21-or-25-with-github-copilot-app/ba-p/4486468)

### Integrating and Expanding Copilot Use Across Platforms

GitHub Copilot continues to integrate with additional workflows. After last week’s news about Copilot support for OpenCode (including login and credential management across desktop, terminal, and IDE), current guides show practical setup steps for connecting Copilot to OpenCode and CI/CD tools. An in-depth look at Copilot’s Arm Cloud Migration Agent discusses how Copilot helps with container migration, indicating expanded applicability in infrastructure and cloud migrations. Community updates show more widespread adoption.

- [How to use GitHub Copilot with OpenCode](/github-copilot/videos/how-to-use-github-copilot-with-opencode)
- [GitHub Copilot Arm Cloud Migration Agent Deep Dive](/github-copilot/videos/github-copilot-arm-cloud-migration-agent-deep-dive)
- [The Download: GitHub Copilot SDK Updates, Copilot for OpenCode, and Cloudflare Buys Astro](/github-copilot/videos/the-download-github-copilot-sdk-updates-copilot-for-opencode-and-cloudflare-buys-astro)

### Advanced Workflow Guidance and Tutorials

Recent resources offer strategies for using Copilot throughout the development process. Tutorials cover test-driven development with agents, Copilot review practices, and ways Copilot functions as a collaborator rather than just a code assistant. KQL support in Microsoft Fabric continues to show how Copilot can power analytics and engineering. Community events like the .NET AI Community Standup discuss new SDK features and integration, highlighting practical implementation advice.

- [Applying Context Windows, Plan Agent, and TDD with GitHub Copilot to Build a Countdown App](https://github.blog/developer-skills/application-development/context-windows-plan-agent-and-tdd-what-i-learned-building-a-countdown-app-with-github-copilot/)
- [Introducing Copilot for Real-Time Dashboards: Write KQL with Natural Language](https://blog.fabric.microsoft.com/en-US/blog/introducing-copilot-for-real-time-dashboards-write-kql-with-natural-language/)
- [How to Review GitHub Copilot’s Work Like a Senior Developer](https://www.cooknwithcopilot.com/blog/how-to-review-github-copilots-work-like-a-senior-developer.html)
- [.NET AI Community Standup - Using the GitHub Copilot SDK in .NET Apps](/github-copilot/videos/net-ai-community-standup-using-the-github-copilot-sdk-in-net-apps)

## AI

AI platform news this week highlights new agent orchestration tools and real-world automation scenarios in areas such as healthcare, law, and retail. Microsoft continues developing orchestration and security patterns. Best practices and open-source agent projects support teams looking to deliver practical AI solutions and reliable products.

### Microsoft Agent Framework, Foundry, and Agentic Orchestration

Microsoft’s Agent Framework (Python/.NET) reaches deeper into enterprise infrastructure, with Windows 365 for Agents providing secure, flexible cloud PCs for agent deployments beside human users. The new Agent 365 APIs and SDKs add robust and modular orchestration for scaling, policies, and automation. Integration with Azure, Entra ID, Intune, and added capabilities for credentials and observability all follow previous best-practices discussions. The Microsoft Foundry for VS Code introduces a workflow visualizer, resource tracking, and improved feedback loops for production-scale orchestration. Technical articles explore Foundry IQ for retrieval-augmented generation (RAG), context management, and troubleshooting memory, reinforcing the modular agent platform focus.

- [Windows 365 for Agents: Enabling Secure AI Cloud PCs](https://blogs.windows.com/windowsexperience/2026/01/22/windows-365-for-agents-the-cloud-pcs-next-chapter/)
- [The AI Agent Development Blueprint: From Design to Production with Microsoft Agent Framework](/ai/videos/the-ai-agent-development-blueprint-from-design-to-production-with-microsoft-agent-framework)
- [Microsoft Foundry for VS Code: January 2026 Update](https://techcommunity.microsoft.com/t5/microsoft-developer-community/microsoft-foundry-for-vs-code-january-2026-update/ba-p/4486132)
- [Deep Dive into Foundry IQ and Azure AI Search](/ai/videos/deep-dive-into-foundry-iq-and-azure-ai-search)
- [Context-Driven Development: Agent Skills for Microsoft Foundry and Azure](https://devblogs.microsoft.com/all-things-azure/context-driven-development-agent-skills-for-microsoft-foundry-and-azure/)

### Open Source Agent Interoperability and Best Practices

Open source agentic frameworks are moving forward, as discussed previously with the Model Context Protocol (MCP) and agent modularity. Angie Jones’s talk covers using Goose to build interoperable and trusted agents for developers and non-developers, supporting community and production adoption. Goose shows how Azure’s approach to open APIs and context supports broader agent patterns.

- [Angie Jones on AI Agents, Goose, and the Model Context Protocol (MCP) at GitHub Universe](/ai/videos/angie-jones-on-ai-agents-goose-and-the-model-context-protocol-mcp-at-github-universe)

### Specification-Driven and Contextual AI Development Workflows

The resource "From Vibe Coding to Spec-Driven Development" continues the discussion on test-driven and context-oriented agent workflows. This practical guide helps teams implement improved validation, automation, and error handling in agentic delivery. The content connects prior themes about prompt design and engineering rigor for reliable agent projects.

- [From Vibe Coding to Spec-Driven Development: Part 3 – Best Practices and Troubleshooting](https://hiddedesmet.com/from-vibe-coding-to-spec-driven-development-part3)

### Azure AI in Healthcare, Legal Practice, and Retail Scenarios

Azure collaborations reach practical use for healthcare (e.g., Parkinson’s care), legal technology (AI for India’s court system), and retail (autonomous robots). These examples extend previous themes of workflow modernization, while providing tips on compliance, privacy, cost efficiency, and deploying agents in context-sensitive environments.

- [How AI-Powered Collaborations Are Transforming Healthcare and Life Sciences](https://www.microsoft.com/en-us/startups/blog/how-ai-powered-collaborations-are-transforming-healthcare-and-life-sciences/)
- [How AI and Microsoft Azure Are Transforming Legal Practice in India](https://news.microsoft.com/source/asia/2026/01/21/code-of-law-how-ai-is-helping-indias-lawyers-work-faster/)
- [How Agentic AI Robots Are Transforming the Retail Store Experience](https://www.microsoft.com/en-us/microsoft-cloud/blog/2026/01/20/frontier-transformation-in-retail-how-agentic-ai-robots-are-redefining-store-experiences/)

## ML

This week’s machine learning updates highlight improvements to developer toolkits, analytics, and applied AI for robotics. Microsoft’s ecosystem releases streamline development, testing, and deployment for a range of ML applications.

### Microsoft Fabric: Enhanced Data Engineering, Analytics, and Performance

Building on last week’s news about ML in Fabric, these updates provide enhanced security and speed for Spark workloads via Private Endpoints, cost-saving autoscale features, and up to 4x Spark performance improvements with the Native Execution Engine. The GigaOm report recognizes Fabric’s unified feature set and includes new controls for cost, scaled SQL pool management, and additional ML connectors. Serverless processing and new OneLake capabilities support flexible analytics and engineering. Real-Time Dashboards have further speed optimizations, boosting streaming and IoT analytics up to 6x or 10x faster. Updated documentation and ongoing events keep users informed.

- [Securely Scaling Spark Data Engineering in Microsoft Fabric](/ml/videos/securely-scaling-spark-data-engineering-in-microsoft-fabric)
- [Microsoft Fabric Data Warehouse: GigaOm Radar Leader and Outperformer](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-data-warehouse-named-a-leader-and-outperformer-in-gigaom-radar-for-data-warehouses/)
- [Performance Improvements for Microsoft Fabric Real-Time Dashboards](https://blog.fabric.microsoft.com/en-US/blog/faster-smoother-more-delightful-real-time-dashboards-performance-improvements/)

### Physical AI Advances: Microsoft Research’s Rho-alpha Robotics Model

Rho-alpha, from Microsoft Research, applies machine learning beyond data analytics by supporting physical robotics. Its underlying system combines natural language processing, multiple sensors, and controls, and supports continuous learning from user interactions. The platform aligns with earlier discussions around Copilot’s agentic updates and best-practice monitoring. Developers in robotics, manufacturing, and real-time control gain tools as APIs and SDKs are released, showing a unified approach similar to advances in Fabric and .NET AI.

- [Introducing Rho-alpha: Microsoft Research's Robotics Model for Physical AI](https://www.linkedin.com/posts/satyanadella_introducing-rho-alpha-the-new-robotics-model-activity-7419757666660466688-jSpD)

## Azure

Azure’s current updates offer improvements in networking, infrastructure automation, data engineering, operations, and developer productivity. These include enhanced reliability for Kubernetes and storage, automation features, better analytics, and tools for modern developer workflows.

### Azure Kubernetes Service (AKS) and Infrastructure Automation

Guides for AKS cover how to scale, secure, and improve cluster performance. One walk-through investigates DNS scaling using Cilium, NodeLocal DNSCache, and FQDN policy to address latency in large workloads and documents troubleshooting for outbound traffic. Another resource explains optimizing AKS node pools with Crossplane, including notes on version compatibility and automation. Java users can use the Azure Performance Diagnostics Tool v5.0 to monitor JVM metrics—useful for faster debugging on Kubernetes.

- [Scaling DNS on AKS with Cilium: NodeLocal DNSCache, LRP, and FQDN Policies](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/scaling-dns-on-aks-with-cilium-nodelocal-dnscache-lrp-and-fqdn/ba-p/4486323)
- [Parallel AKS Node Pool Creation with Crossplane: A Version Compatibility Journey](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/parallel-aks-node-pool-creation-with-crossplane-a-version/ba-p/4477936)
- [Automated Java Performance Diagnostics in Kubernetes using Azure SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/automated-java-performance-diagnostics-in-kubernetes-using-azure/ba-p/4488027)

### Microsoft Fabric Data Integration, Real-Time Intelligence, and Analytics

New features in Fabric include full pipelines for retail analytics using Delta Lake, Debezium, and Azure Event Hubs, showing automated change tracking and partition management. Data Factory now adds incremental copy/CDC, more connectors, flexible replication, and adjustable mapping for schema changes. A preview integration with Cribl Stream allows for fast telemetry routing and visualization.

- [Scalable Data Ingestion for Retail: Dynamic Partitioning and Source Detection with Microsoft Fabric](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/a-technical-implementation-guide-for-multi-store-retail/ba-p/4488418)
- [Enhancements to Microsoft Fabric Data Factory Copy Job: Incremental Copy and Change Data Capture](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-movement-across-multiple-clouds-with-copy-job-enhancements-on-incremental-copy-and-change-data-capture/)
- [Integrating Cribl with Microsoft Fabric Real-Time Intelligence (Preview)](https://blog.fabric.microsoft.com/en-US/blog/expanding-real-time-intelligence-data-sources-with-cribl-source-preview/)

### Azure Storage: AI-Centric Platform Evolution

The Azure Storage service’s updated roadmap outlines shifts toward scalable, AI-focused workloads. This includes changes for blob storage, deep integration with AMLFS, and options for GPU-based operations (benefiting agents or LLMs). Elastic SAN and ACStor provide orchestration and combined file/block sharing for demanding deployments. Resiliency improvements span disks and files, while sustainability and smart tiering features are also being expanded.

- [The Future of Azure Storage: AI-Optimized Innovations for 2026](https://azure.microsoft.com/en-us/blog/beyond-boundaries-the-future-of-azure-storage-in-2026/)

### Azure Automation, SRE, and Incident Management

Recent guides describe how to automate with Azure SRE Agents. You can connect SRE Agents with MCP for fine-grained roles and permission, scheduled compliance/security checks, and send automated reports via Teams or GitHub. Incident management integrates with ServiceNow to streamline triage and root cause processes, and work notes are now automated with AI summaries.

- [How to Connect Azure SRE Agent to Azure MCP](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-connect-azure-sre-agent-to-azure-mcp/ba-p/4488905)
- [Proactive Cloud Ops with SRE Agent: Scheduled Checks for Azure Optimization](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/proactive-cloud-ops-with-sre-agent-scheduled-checks-for-cloud/ba-p/4487261)
- [Connect Azure SRE Agent to ServiceNow: End-to-End Incident Response](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/connect-azure-sre-agent-to-servicenow-end-to-end-incident/ba-p/4487824)

### Azure Verified Modules, Infrastructure as Code, and Platform Foundations

Azure Verified Modules (AVM) for Platform Landing Zone with Bicep are now generally available, featuring modular IaC support for governance, network, and management. AVM adds Deployment Stacks, parameter files, policy control, and clear docs for migrations—continuing the platform update cycle.

- [Azure Verified Modules for Platform Landing Zone with Bicep Now Generally Available](https://techcommunity.microsoft.com/t5/azure-tools-blog/release-of-bicep-azure-verified-modules-for-platform-landing/ba-p/4487932)

### Developer Tools, Testing Services, and Workflow Improvements

Developers now have access to the Azure Playwright Testing Service (Preview) for scalable UI/API automated tests. The preview includes workspaces, secret handling, CI integration, and reporting tools. Playwright Workspaces v2.0 add report views, artifact handling, and data retention controls to help with workflow governance and collaboration—building on ongoing improvements in SQL and CI pipelines.

- [Running Playwright Tests at Scale with Azure Playwright Testing Service (Preview)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-playwright-testing-service-preview-run-playwright-tests-on/ba-p/4487103)
- [Reporting Features Now Available in Playwright Workspaces on Azure](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-reporting-in-playwright-workspaces/ba-p/4487059)

### Memory Reliability and Hardware Efficiency for Azure Infrastructure

Azure launches RAIDDR, an open-source tool for improving reliability in modern memory (like LPDDR5X), and ELC for adaptive CPU power management, increasing data center energy savings while optimizing latency and performance. Both areas advance last week’s focus on infrastructure sustainability.

- [RAIDDR: Redefining Memory Reliability for Hyperscale Azure Infrastructure](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/raiddr-redefining-memory-reliability/ba-p/4487951)
- [Improving Efficiency through Adaptive CPU Uncore Power Management](https://techcommunity.microsoft.com/t5/azure-compute-blog/improving-efficiency-through-adaptive-cpu-uncore-power/ba-p/4486456)

### Application and Container Networking, Security, and Cache Optimization

Guides for Azure Container Apps show secure integration with virtual networks and firewall routing for policy enforcement, monitoring, and compliance. Redis tips include scripts for listing key lifetimes, statistics, and tuning, assisting teams with troubleshooting and scaling.

- [Advanced Container Apps Networking: VNet Integration and Centralized Firewall Traffic Logging](https://techcommunity.microsoft.com/t5/azure/advanced-container-apps-networking-vnet-integration-and/m-p/4488713#M22417)
- [Troubleshooting Azure Redis: Key TTL and Size Analysis with Bash and Lua](https://techcommunity.microsoft.com/t5/azure-paas-blog/redis-keys-statistics/ba-p/4486079)

### Azure Arc Server and Hybrid Cloud Updates

The Azure Arc Server recap covers improved management, zero-downtime patching, TPM rollout, and SQL hybrid workflows, maintaining last week’s focus on secure hybrid and multi-cloud management.

- [Azure Arc Server Jan 2026 Forum Recap](https://techcommunity.microsoft.com/t5/azure-arc-blog/azure-arc-server-jan-2026-forum-recap/ba-p/4487829)

### Microsoft Data Platform Ecosystem

SQLCon and FabCon announcements outline conference topics, training, and product updates for SQL Server, Azure SQL, Fabric, and AI-powered data management. These events extend previous coverage on data platform feedback and innovation.

- [Five Reasons to Attend SQLCon: Deep SQL, Microsoft Fabric, and AI Insights in Atlanta](https://blog.fabric.microsoft.com/en-US/blog/32624/)

### Other Azure News

Developer-focused updates enhance debugging, performance, and workflow management. New filters for Azure Boards (now in private preview) let backlog and Kanban boards filter by custom fields, supporting better UI and management options.

- [Azure Weekly Update: AKS Deployment, NAT Gateway, Load Testing, GitHub Copilot SDK (23 Jan 2026)](/github-copilot/videos/azure-weekly-update-aks-deployment-nat-gateway-load-testing-github-copilot-sdk-23-jan-2026)
- [Azure Boards Adds Custom Field Filters in Private Preview](https://devblogs.microsoft.com/devops/azure-boards-additional-field-filters-private-preview/)

## Coding

Highlights this week include optimized patterns for .NET and SharePoint, new features for React Native Windows, and practical resources for developer engagement and education.

### .NET Performance Optimization and Modern Web Development

This week includes a guide on making `IEnumerable<T>` iteration in .NET allocation-free. Andrew Lock details compiler behavior and testing, explaining how Reflection.Emit/DynamicMethod avoids boxing overhead on older runtimes—useful for SDKs and instrumentation. Resources on modern web teaching (Razor Pages and HTMX) show how to build with less JavaScript, leveraging Razor’s separation of logic for maintainable courses and projects.

- [Making foreach on an IEnumerable Allocation-Free in .NET with Reflection and Dynamic Methods](https://andrewlock.net/making-foreach-on-an-ienumerable-allocation-free-using-reflection-and-dynamic-methods/)
- [Teaching Modern Web Development with .NET, Razor Pages, and HTMX](/coding/videos/teaching-modern-web-development-with-net-razor-pages-and-htmx)

### React Native Windows and Cross-Platform App Development

React Native Windows v0.81 introduces features for desktop debugging and accessibility, including Hermes engine support. Early support for DevTools is coming, helping teams with breakpoints and profiling. Recent architecture work prepares the project for a move to “Fabric.” Discussions remain about whether to prefer React Native or MAUI for .NET-based cross-platform development.

- [Microsoft Updates React Native for Windows: Comparing with MAUI for Cross-Platform Development](https://devclass.com/2026/01/20/microsoft-updates-react-native-for-windows-developers-ask-why-not-use-maui/)

### SharePoint Site Optimization Guides

SharePoint site optimization advice covers scaling with site collections/hubs, filtering lists and libraries, improving metadata, auditing custom code, batching API usage, and regular diagnostics. A checklist provides steps for both urgent and longer-term maintenance across large enterprise sites.

- [Performance Optimization Tips for Large SharePoint Sites](https://dellenny.com/performance-optimization-tips-for-large-sharepoint-sites/)

### Other Coding News

A tutorial shows how to use the "Report Issue" feature in VS Code for feature requests, including tips for improving the chance of getting changes reviewed, referencing feedback cycles as a means to better software.

- [How to Request a VS Code Feature (The RIGHT Way)](/coding/videos/how-to-request-a-vs-code-feature-the-right-way)

## DevOps

DevOps news includes new GitHub Actions runners for cost-effective CI, improved issue and pull request workflows, and advances in artifact traceability and security features. Beginner-friendly content covers common workflows.

### GitHub Platform and Workflow Improvements

GitHub Actions announces general availability of 1 vCPU Linux runners for shorter, lightweight jobs such as linting and scripting—designed to help teams lower CI/CD costs. Updates expand on earlier changes for reliability and artifact management. GitHub Issues are now faster to load, reducing project dashboard wait times, and the modern "Files Changed" pull request interface with improved commenting is now the default—bringing better accessibility and review features.

- [GitHub Actions 1 vCPU Linux Runners Now Generally Available](https://github.blog/changelog/2026-01-22-1-vcpu-linux-runner-now-generally-available-in-github-actions)
- [Faster Loading for GitHub Issues: 35% of Views Now Under 200ms](https://github.blog/changelog/2026-01-22-faster-loading-for-github-issues)
- [GitHub's Improved Pull Request “Files Changed” Experience Now Default](https://github.blog/changelog/2026-01-22-improved-pull-request-files-changed-page-on-by-default)

### Expanding Supply Chain Security and Artifact Traceability

GitHub adds artifact metadata APIs and Unified Artifact Views to help with code/build tracking and verification. REST APIs and Defender/JFrog integrations allow improved workflows. The new Packages tab displays build details, SLSA Level 3 proofs, and event histories. GitHub Actions now streamline attestation creation. Docs and guides offer step-by-step help for DevSecOps adoption.

- [Strengthen Your Supply Chain with GitHub Artifact Traceability and SLSA Build Level 3](https://github.blog/changelog/2026-01-20-strengthen-your-supply-chain-with-code-to-cloud-traceability-and-slsa-build-level-3-security)

### Other DevOps News

Enterprise admins can now use more precise budget controls that exclude cost center usage—supported by REST APIs for automation. User feedback feeds directly into policy and cost management improvements. New tutorials for beginners walk through `git switch`, `git add`, and `git commit`, highlighting the value of isolating changes and working with tracked project histories.

- [Enterprise-Scoped Budgets Feature Update for GitHub Enterprise Customers](https://github.blog/changelog/2026-01-19-enterprise-scoped-budgets-that-exclude-cost-center-usage-in-public-preview)
- [How to Switch Branches and Commit Changes in Git](/devops/videos/how-to-switch-branches-and-commit-changes-in-git)

## Security

Security updates include coverage of risks in developer environments, AI and cloud posture, vulnerability triage, and new controls for workload and supply chain protection. Data governance remains a consistent theme.

### Security in Developer Tooling and Workflows

An analysis covers risks from VS Code’s `tasks.json`, which can contain unsafe commands if included in shared repositories. These shortcuts might inadvertently expose engineers to unsafe code. Developers are encouraged to use isolated dev containers, scrutinize repos for automation files, and request stronger platform-level controls.

- [Abusing VS Code tasks.json: Security Risks from Malicious Repository Configurations](https://devclass.com/2026/01/22/vs-code-tasks-config-file-abused-to-run-malicious-code/)

### Cloud AI Security and Copilot Studio Protections

Microsoft Defender now offers unified management tools for AI risk in cloud environments (including Azure, AWS, GCP), mapping agent privileges and checking for prompt injection. Microsoft Copilot Studio’s runtime protection now includes automated webhook review to identify and stop unintended agent actions. These features combine with last week’s coverage of agent security.

- [Securing AI Agents in the Cloud: Microsoft Defender's Approach](https://www.microsoft.com/en-us/security/blog/2026/01/21/new-era-of-agents-new-era-of-posture/)
- [Securing Microsoft Copilot Studio AI Agents with Defender Runtime Protection](https://www.microsoft.com/en-us/security/blog/2026/01/23/runtime-risk-realtime-defense-securing-ai-agents/)

### Microsoft Fabric and OneLake: Fine-Grained Security Management

Fabric now offers REST APIs for OneLake that provide automated, path-based access controls. These APIs connect with Entra ID for fine-grained, CI/CD-ready permission management. OneLake security for mirrored databases gives detailed controls—reducing risks associated with excess permission or duplicate data.

- [Granular REST APIs for OneLake Security Management in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/granular-apis-for-onelake-security-preview/)
- [Manage OneLake Security for Mirrored Databases in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/manage-onelake-security-for-mirrored-databases-preview/)

### Threat Intelligence: Phishing Campaigns and BEC Countermeasures

Microsoft investigates new phishing and BEC attacks targeting the energy sector, including using SharePoint for multi-stage attacks that bypass filters and steal sessions. Attackers can add mailbox rules for persistence and erase evidence. The report covers detection, recovery, and analytics alongside remediation strategies, reinforcing best practices for defense.

- [Resurgence of Multi‑Stage AiTM Phishing and BEC Campaign Abusing SharePoint](https://www.microsoft.com/en-us/security/blog/2026/01/21/multistage-aitm-phishing-bec-campaign-abusing-sharepoint/)

### Automating Vulnerability Detection and Management

GitHub Security Lab’s Taskflow Agent uses LLMs and rule books to automate vulnerability triage for Actions and JavaScript, filter out false positives, and connect with GitHub Issues. Modular YAML and prompt/task templates enable extensibility, helping teams systematically review reports and reduce manual work.

- [Automating Security Vulnerability Triage with GitHub Security Lab Taskflow Agent](https://github.blog/security/ai-supported-vulnerability-triage-with-the-github-security-lab-taskflow-agent/)

### Secure Auth and Delegated Access Patterns in Cloud Services

A new guide explains how to use Microsoft Entra’s OAuth2 On-Behalf-Of flow for Python MCP servers (using FastMCP SDK), enabling developers to configure delegated, audited API access. Code samples and setup details help developers integrate secure user flows with existing cloud services.

- [Implementing Microsoft Entra On-Behalf-Of (OBO) Flow in Python MCP Servers with FastMCP](https://techcommunity.microsoft.com/t5/microsoft-developer-community/using-on-behalf-of-flow-for-entra-based-mcp-servers/ba-p/4486760)

### Enhancing Supply Chain Security: Container Image Signing

Microsoft’s Notary Project and Artifact Signing (now GA) tools provide managed certificate handling for CI/CD and AKS—making it simpler to sign images, handle credentials, and use RBAC. The guides support migration from older image signing strategies.

- [Simplifying Image Signing with Notary Project and Artifact Signing (GA)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/simplifying-image-signing-with-notary-project-and-artifact/ba-p/4487942)

### Broader Identity and Data Security Guidance

A framework for 2026 outlines four priorities for identity and network access risk management: adaptive policies, governing users/agents, Zero Trust adoption, and credential controls. Platform integration (Defender, Purview, Verified ID) is central for compliance. Azure Essentials video content shows Purview’s main governance and audit features, complementing ongoing security automation content.

- [4 Priorities for AI-Powered Identity and Network Access Security in 2026](https://www.microsoft.com/en-us/security/blog/2026/01/20/four-priorities-for-ai-powered-identity-and-network-access-security-in-2026/)
- [Understand How Purview Secures and Governs Your Entire Data Estate](/azure/videos/understand-how-purview-secures-and-governs-your-entire-data-estate)

### Other Security News

CodeQL Release 2.23.9 announces the deprecation of support for Kotlin 1.6/1.7 in February 2026. Users of these versions should upgrade. Guidance is available for GitHub Enterprise Server and CLI, echoing last week’s security tooling updates.

- [CodeQL 2.23.9 Release: Deprecation Notice and Update Details](https://github.blog/changelog/2026-01-20-codeql-2-23-9-has-been-released)
