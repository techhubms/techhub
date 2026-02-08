---
title: 'Ignite 2025: Updates in Agentic AI, Cloud, Developer Tools, and Security'
author: TechHub
date: 2025-12-01 09:00:00 +00:00
tags:
- Agentic AI
- Automation
- Cloud Native
- Confidential Computing
- Data Platform
- Developer Productivity
- Device Security
- Enterprise Migration
- Low Code
- Microsoft Foundry
- Observability
- Power Platform
- VS
- Zero Trust
- AI
- GitHub Copilot
- ML
- Azure
- DevOps
- Security
- Roundups
- .NET
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
external_url: /all/roundups/Weekly-AI-and-Tech-News-Roundup-2025-12-01
---
Welcome to this week’s roundup, highlighting the latest technology shifts introduced at Ignite 2025. This collection covers updates transforming how organizations and developers approach automation, infrastructure, and secure integration of AI agents in daily operations. The themes this week are practical automation, built-in security, and the smooth alignment of AI with operational platforms. Key subjects include no-code Copilot workflows, large-scale Azure supercomputing, reliable DevOps automation, confidential computing, and Zero Trust solutions.

With a focus on agent-powered platforms, Microsoft and its partners unveiled enhanced features: GitHub Copilot now offers advanced model selection, agentic AI and ML support expand to local and cloud via Foundry and Azure, and hybrid solutions improve deployment options with unified cloud-native tools. Security is always at the core, supported by new features like confidential computing, agent lifecycle governance, and AI-based protections at both hardware and application levels. Explore the detailed coverage below, with technical context and actionable guides for architects, engineers, and IT decision-makers planning the next phase of intelligent and robust technology.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot AI Models and Integration](#github-copilot-ai-models-and-integration)
  - [Copilot in the Visual Studio Family](#copilot-in-the-visual-studio-family)
  - [Copilot in Microsoft Foundry and Code-First Workflows](#copilot-in-microsoft-foundry-and-code-first-workflows)
  - [Low-Code Agent Building and Automation with Copilot Studio](#low-code-agent-building-and-automation-with-copilot-studio)
  - [Advanced AI-Assisted Coding Models and Custom Copilot Agents](#advanced-ai-assisted-coding-models-and-custom-copilot-agents)
  - [Responsible and Sustainable Software with Copilot and Agentic DevOps](#responsible-and-sustainable-software-with-copilot-and-agentic-devops)
- [AI](#ai)
  - [Microsoft Foundry and Edge/Enterprise AI Workflows](#microsoft-foundry-and-edgeenterprise-ai-workflows)
  - [AI Infrastructure and Platform Integration on Azure](#ai-infrastructure-and-platform-integration-on-azure)
  - [Agentic AI and Automation with Copilot Studio and Power Platform](#agentic-ai-and-automation-with-copilot-studio-and-power-platform)
  - [Specialized AI Use Cases: Industry, Supply Chain, and Domain Solutions](#specialized-ai-use-cases-industry-supply-chain-and-domain-solutions)
  - [Azure Speech, Voice, and Conversational AI](#azure-speech-voice-and-conversational-ai)
  - [AI Developer Workflow and Code Integration](#ai-developer-workflow-and-code-integration)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Microsoft Foundry and AI Agent Fine-Tuning](#microsoft-foundry-and-ai-agent-fine-tuning)
  - [Azure Databricks: Unified Data and AI Ecosystem](#azure-databricks-unified-data-and-ai-ecosystem)
  - [Pushing the Boundaries: Azure AI Supercomputing Infrastructure](#pushing-the-boundaries-azure-ai-supercomputing-infrastructure)
- [Azure](#azure)
  - [Azure Cloud Native Development and Compute Innovations](#azure-cloud-native-development-and-compute-innovations)
  - [Observability, Automation, and Operational Resilience](#observability-automation-and-operational-resilience)
  - [Data Platform Updates: Microsoft Fabric, SQL, and Data Integration](#data-platform-updates-microsoft-fabric-sql-and-data-integration)
  - [Edge and Hybrid AI: Azure Local, Deployment Automation, and Lenovo Partnership](#edge-and-hybrid-ai-azure-local-deployment-automation-and-lenovo-partnership)
  - [Logic Apps, Integration, and Workflow Automation](#logic-apps-integration-and-workflow-automation)
  - [Resiliency, Backup, and Secure Cloud Architecture](#resiliency-backup-and-secure-cloud-architecture)
  - [Modernization, Migration, and Partner Solutions](#modernization-migration-and-partner-solutions)
  - [Azure IaaS, Infrastructure Optimization, and Cost Management](#azure-iaas-infrastructure-optimization-and-cost-management)
  - [Security, AI Governance, and Endpoint Management](#security-ai-governance-and-endpoint-management)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [Modern IDEs and the Windows Developer Experience](#modern-ides-and-the-windows-developer-experience)
  - [.NET Diagnostics and C# Design Discipline](#net-diagnostics-and-c-design-discipline)
- [DevOps](#devops)
  - [WinGet and Desired State Configuration Integration](#winget-and-desired-state-configuration-integration)
- [Security](#security)
  - [Advancements in Confidential Computing and Memory-Safe Platform Security](#advancements-in-confidential-computing-and-memory-safe-platform-security)
  - [Securing Agentic AI: Lifecycle, Governance, and Risk Management](#securing-agentic-ai-lifecycle-governance-and-risk-management)
  - [Security Copilot, SOC Automation, and Microsoft Defender Ecosystem](#security-copilot-soc-automation-and-microsoft-defender-ecosystem)
  - [Microsoft Purview and Enterprise Data Security](#microsoft-purview-and-enterprise-data-security)
  - [Identity, Zero Trust, and Cross-Platform Security](#identity-zero-trust-and-cross-platform-security)
  - [Integrated SOC Visibility, Threat Intelligence, and Third-Party Security Partnerships](#integrated-soc-visibility-threat-intelligence-and-third-party-security-partnerships)
  - [Other Security News](#other-security-news)

## GitHub Copilot

This week, GitHub Copilot introduces updated AI models, workflow enhancements, and more automation tools for developers. These developments expand on recent improvements around model management, Visual Studio 2026 integration, and responsible practices for agent-based development. The latest features include support for Anthropic’s newest model, enhanced low-code controls in Copilot Studio, and strategies to ensure responsible use of agentic AI in software projects. Community events and step-by-step tutorials provide developers with more ways to tailor Copilot inside tools like VS Code. These changes highlight Copilot’s adaptability for enterprise-scale environments, local code projects, and specific automation needs.

### GitHub Copilot AI Models and Integration

Continuing last week’s launch of new model controls and previews, GitHub Copilot now offers a public preview of Anthropic’s Claude Opus 4.5 for users on Pro, Pro+, Business, and Enterprise subscriptions. Users can pick Claude Opus 4.5 in Copilot Chat across supported platforms, giving more model flexibility, better coding suggestions, and reduced token costs. Admins for enterprises get new policy settings to manage access, building on last week’s updates for organizational oversight. The release is rolling out in stages and user feedback is being collected to shape future features and model support.

- [Claude Opus 4.5 Public Preview Launches for GitHub Copilot](https://github.blog/changelog/2025-11-24-claude-opus-4-5-is-in-public-preview-for-github-copilot)

### Copilot in the Visual Studio Family

After last week’s updates on Visual Studio 2026 and Copilot-driven workflows, Ignite 2025 featured deeper Copilot integration for smarter code suggestions and improved review experiences. The new Profiler Agent for diagnostics continues the trend of embedding Copilot features into daily development, helping individuals and teams boost productivity. Ongoing monthly updates and interface improvements make sure Copilot aligns with the latest .NET and business requirements, keeping pace with prior Visual Studio releases.

- [First Look at Visual Studio 2026: Fast, Modern, and AI-Powered](/ai/videos/first-look-at-visual-studio-2026-fast-modern-and-ai-powered)

### Copilot in Microsoft Foundry and Code-First Workflows

Building on last week’s spotlight on Agent Mode in VS Code, this week provides a detailed guide for moving Microsoft Foundry workflows into a code-first editing environment. The Model Mondays tutorial series continues, with stepwise instructions for configuring, editing, and testing workflows powered by AI. This underlines Copilot’s local agent features, continuing its commitment to code-first, self-managed AI development—an approach highlighted in prior news and tutorials on workflow improvement.

- [Microsoft Foundry Workflows: Migrating to Code-First Development in VS Code](/ai/videos/microsoft-foundry-workflows-migrating-to-code-first-development-in-vs-code)

### Low-Code Agent Building and Automation with Copilot Studio

Copilot Studio’s progress, highlighted last week with expanded no-code options, continued at Ignite 2025 with the announcement of an embedded builder and a more flexible enterprise platform. These updates add advanced knowledge base management, automated workflow testing, and deeper links to Microsoft 365 Copilot. Earlier case studies on automation are still useful as new sessions focus on deploying agent flows in larger environments with an emphasis on security, credential management, and transparent operations—supporting broader business automation.

Ignite sessions also explored automation and workflow expansion using Copilot Studio, emphasizing security and scalability for both individual and organizational use. These resources outline best practices for integrating Copilot Studio across platforms, meeting ongoing needs for governance and real-world deployment of complex agent flows.

- [Copilot Studio Innovations and Roadmap: Building Low-Code AI Agents (BRK313)](/ai/videos/copilot-studio-innovations-and-roadmap-building-low-code-ai-agents-brk313)
- [Automation in Copilot Studio: Agent Flows and Computer Use](/ai/videos/automation-in-copilot-studio-agent-flows-and-computer-use)

### Advanced AI-Assisted Coding Models and Custom Copilot Agents

Furthering recent discussions on extensibility, this week’s live sessions focus on orchestrating Copilot models and setting up advanced agent workflows in VS Code. Using new standards like agents.md and tools such as Ruler, developers now have clearer options for delegating tasks like coding and evaluation. Recent tutorials on prompt engineering, command line tools, and voice integrations build on past guides, keeping Copilot customizable and shaped by the development community. Showcases for building custom agents highlight Copilot’s flexibility for specific workflows in various areas.

- [Taming AI Assisted Coding Models with Eleanor Berger](/ai/videos/taming-ai-assisted-coding-models-with-eleanor-berger)
- [Building Custom Agents for Copilot on Rubber Duck Thursdays](/ai/videos/building-custom-agents-for-copilot-on-rubber-duck-thursdays)

### Responsible and Sustainable Software with Copilot and Agentic DevOps

Guidance around responsible AI remains a steady theme, as Ignite sessions this week featured experts from Cognizant and Microsoft on managing agentic AI and scaling Copilot responsibly within organizations. Discussions covered how to build more sustainable software, connecting with previous conversations about measurable green workflows. Developers can find new tools and resources for driving ethical and sustainability-focused Copilot adoption in their projects.

- [Building and Deploying Responsible Agentic AI with Microsoft Copilot](/ai/videos/building-and-deploying-responsible-agentic-ai-with-microsoft-copilot)
- [Building Sustainable Software with Agentic DevOps and GitHub Copilot](/ai/videos/building-sustainable-software-with-agentic-devops-and-github-copilot)

## AI

AI news this week includes applied updates, technical explorations, and confirmation of value in practical deployments, all centered on making intelligent solutions easier to roll out in cloud, edge, or enterprise situations. Ignite 2025 delivered new agent frameworks, infrastructure ready for AI, and reference architectures to address current business challenges. Developers now have access to updated guidance, stronger governance, and improved tools, with key session content on Microsoft Foundry, Azure, and automation.

### Microsoft Foundry and Edge/Enterprise AI Workflows

Continuing last week’s developments, Microsoft Foundry now further supports cloud and edge workflows by introducing deployment guides for Foundry Local and Android integration. This shift puts AI closer to where data lives, supporting the flexible, hybrid scenarios discussed previously.

Live sessions pick up on earlier coverage of agent coordination and tool-rich agent frameworks, expanding into GitOps and ORAS-based pipelines, as well as real-world demos on cluster management and staged deployments. Security, lifecycle controls, and collaboration with Microsoft Defender continue to progress, as does the use of multi-tenant solutions highlighted before.

Wider local AI support for Phi Silica and Copilot+ PCs is in place, including Windows AI API learning sessions. Tutorials on techniques like LoRA tuning, RAG, and admin controls further support the enterprise trend toward accessible, flexible AI. Use cases span industries like healthcare and analytics, underlining practical results from these developments.

- [Building and Shipping Edge AI Apps with Microsoft Foundry](/ai/videos/building-and-shipping-edge-ai-apps-with-microsoft-foundry)
- [Integrating Local AI in Enterprise Apps with Windows AI APIs and Microsoft Foundry](/ai/videos/integrating-local-ai-in-enterprise-apps-with-windows-ai-apis-and-microsoft-foundry)
- [From Concept to Code: Building Production-Ready Multi-Agent Systems with Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-concept-to-code-building-production-ready-multi-agent/ba-p/4472752)
- [Microsoft Foundry Workflows - Pt. 1: Creating a Sequential Multi-Agent Workflow](/ai/videos/microsoft-foundry-workflows-pt-1-creating-a-sequential-multi-agent-workflow)
- [Creating Multi-Agent Workflows in Microsoft Foundry: Adding Agents with Tools](/ai/videos/creating-multi-agent-workflows-in-microsoft-foundry-adding-agents-with-tools)
- [Migrating Microsoft Foundry Workflows to VS Code Web](/ai/videos/migrating-microsoft-foundry-workflows-to-vs-code-web)
- [Building Autonomous Enterprise Agents with Reasoning in Microsoft Foundry](/ai/videos/building-autonomous-enterprise-agents-with-reasoning-in-microsoft-foundry)
- [Building Microsoft Agent Framework Solutions for Microsoft 365 with AI Integration](/ai/videos/building-microsoft-agent-framework-solutions-for-microsoft-365-with-ai-integration)
- [Agentic AI for Creatives: Microsoft Foundry and Data Solutions at Ignite](/ai/videos/agentic-ai-for-creatives-microsoft-foundry-and-data-solutions-at-ignite)
- [Panel: Real-World Architectures and Lessons from Scaling AI Agents on Azure](/ai/videos/panel-real-world-architectures-and-lessons-from-scaling-ai-agents-on-azure)
- [AI Enterprise Value: Real-World Applications with Microsoft Foundry and MCP](/ai/videos/ai-enterprise-value-real-world-applications-with-microsoft-foundry-and-mcp)
- [Foundry Control Plane: Managing AI Agents at Scale](/ai/videos/foundry-control-plane-managing-ai-agents-at-scale)

### AI Infrastructure and Platform Integration on Azure

Expanding last week’s highlights of Azure’s Fairwater AI and core infrastructure, this week focuses on new NVIDIA Blackwell cluster deployments and progress in confidential computing. Detailed technical topics include advanced networking like NVLink and new RTX Pro 6000 features for robotics and simulation, following on from earlier discussions. Omniverse digital twin workflows are explored deeply, with examples for various industries.

Practical tutorials continue themes of real-time AI streaming, flexible model routing, and system reliability. Demonstrations show how open-source tools such as Ray, KAITO, and LangChain work together with Azure’s managed pipeline services, matching the earlier move toward hybrid, automated workflows. New Oracle Database@Azure integrations now include step-by-step guides for secure, unified AI search, and Copilot Studio and Purview play ongoing roles for governance and security.

- [Power Next-Generation AI Workloads with NVIDIA Blackwell on Azure](/ai/videos/power-next-generation-ai-workloads-with-nvidia-blackwell-on-azure)
- [Running AI on Azure Storage: Fast, Secure, and Scalable](/ai/videos/running-ai-on-azure-storage-fast-secure-and-scalable)
- [Embedding AI in Oracle Workloads with Oracle Database@Azure and Microsoft Fabric](/ai/videos/embedding-ai-in-oracle-workloads-with-oracle-databaseazure-and-microsoft-fabric)
- [AI-Driven Governance for Nasdaq Boardvantage with Azure PostgreSQL and Microsoft Foundry](/ai/videos/ai-driven-governance-for-nasdaq-boardvantage-with-azure-postgresql-and-microsoft-foundry)

### Agentic AI and Automation with Copilot Studio and Power Platform

Expanding on last week’s focus on automation, this week delivers concrete steps and best practices for lead qualification, sales automation, and workflow reliability in Copilot Studio—all closely linked with the Power Platform.

Guidance covers always-on agent deployment, failover, and monitoring for consistent operation. New features in Power Apps and Power Pages, as well as improvements to RPA and AI integration, reinforce the rapid implementation of low-code AI solutions for evolving business processes.

- [Microsoft Ignite: Agents at Work and Copilot Studio for Business Process Automation](/ai/videos/microsoft-ignite-agents-at-work-and-copilot-studio-for-business-process-automation)
- [Advancements in Power Platform: AI, Automation, and Secure Integrations](/ai/videos/advancements-in-power-platform-ai-automation-and-secure-integrations)
- [Best Practices for Always-On AI Agents Using Copilot Studio and Power Platform](/ai/videos/best-practices-for-always-on-ai-agents-using-copilot-studio-and-power-platform)

### Specialized AI Use Cases: Industry, Supply Chain, and Domain Solutions

At Ignite, multiple industry case studies continue the pattern of enterprise-scale agentic AI adoption. This includes projects at Kraft Heinz, Toyota, and Zurich, focused on real deployment of digital twins, and the use of SQL Server embeddings and ERP tools for process automation. These guides provide steps for applying advanced orchestration strategies in real settings, moving theory into successful practice.

- [Optimizing Manufacturing at Kraft Heinz with AI, Azure Arc, and Microsoft Foundry](/ai/videos/optimizing-manufacturing-at-kraft-heinz-with-ai-azure-arc-and-microsoft-foundry)
- [Reinventing Manufacturing with Digital Twin: AI-Powered Advances](/ai/videos/reinventing-manufacturing-with-digital-twin-ai-powered-advances)
- [Transforming Manufacturing with Digital Twins and Real-Time Simulation Featuring Azure and NVIDIA AI](/ai/videos/transforming-manufacturing-with-digital-twins-and-real-time-simulation-featuring-azure-and-nvidia-ai)
- [Drive Growth, Profitability and Resilience with Agentic Supply Chains](/ai/videos/drive-growth-profitability-and-resilience-with-agentic-supply-chains)
- [Zurich and Toyota’s Playbook for Enterprise AI Innovation](/ai/videos/zurich-and-toyotas-playbook-for-enterprise-ai-innovation)
- [ERP Transformation with AI: Building Autonomous Agents (Microsoft Ignite 2025 Session PBRK361)](/ai/videos/erp-transformation-with-ai-building-autonomous-agents-microsoft-ignite-2025-session-pbrk361)
- [Powering Financial Workflows with AI: Microsoft and LSEG Partnership at Ignite](/ai/videos/powering-financial-workflows-with-ai-microsoft-and-lseg-partnership-at-ignite)
- [Build Secure Agentic AI Apps with SQL Server 2025](/ai/videos/build-secure-agentic-ai-apps-with-sql-server-2025)
- [Partners and Agentic AI: Transforming Energy and Resources at Microsoft Ignite](/ai/videos/partners-and-agentic-ai-transforming-energy-and-resources-at-microsoft-ignite)
- [Transforming Enterprise Workflows with C3 AI Agentic Process Automation](/ai/videos/transforming-enterprise-workflows-with-c3-ai-agentic-process-automation)
- [Building Enterprise AI Apps with C3 Agentic AI Platform at Microsoft Ignite](/ai/videos/building-enterprise-ai-apps-with-c3-agentic-ai-platform-at-microsoft-ignite)
- [Real Stories of AI Transformation in Local Government](/ai/videos/real-stories-of-ai-transformation-in-local-government)
- [AI-Powered Industry Solutions at Microsoft Ignite 2025: Capgemini's Multi-Channel Knowledge Innovations](/ai/videos/ai-powered-industry-solutions-at-microsoft-ignite-2025-capgeminis-multi-channel-knowledge-innovations)
- [Innovate with AI at Enterprise Scale: Microsoft Ignite Session BRK410](/ai/videos/innovate-with-ai-at-enterprise-scale-microsoft-ignite-session-brk410)
- [Impiger’s AI-First Enterprise Transformation at Microsoft Ignite](/ai/videos/impigers-ai-first-enterprise-transformation-at-microsoft-ignite)

### Azure Speech, Voice, and Conversational AI

Azure Speech and Voice Live API are now generally available. This extension, originating from recent updates, enables real-time, context-aware conversational AI with new features such as enhanced model selection and HD neural voice support that covers over 100 languages. These improvements support tasks like live translation in healthcare and customer service, following the roadmap originally presented in previous updates.

- [Building Agentic Apps with Azure Speech and Voice Live API](/ai/videos/building-agentic-apps-with-azure-speech-and-voice-live-api)

### AI Developer Workflow and Code Integration

Workflow integration continues to improve, with new resources covering GitHub, Foundry, and VS Code Web. The focus is on stepwise guides for migrating code, automating pipelines, and managing agent packaging for Azure deployments, following ongoing discussions around CI/CD and AI prototyping.

Configuration-as-code tutorials reinforce the code-first approach, equipping teams with reliable methods for traceable AI-powered automation.

- [Build AI Apps Fast with GitHub and Microsoft Foundry in Action (MS Ignite BRK110)](/ai/videos/build-ai-apps-fast-with-github-and-microsoft-foundry-in-action-ms-ignite-brk110)

### Other AI News

C3 AI Studio continues to evolve, offering practical guides for both low-code and code-first app development, focusing on deployment, ontology building, and monitoring. The community forums show strong collaboration, connecting GitHub and the Microsoft AI ecosystem for enterprise-ready knowledge management.

- [Building Enterprise AI Apps with C3 Agentic AI Platform at Microsoft Ignite](/ai/videos/building-enterprise-ai-apps-with-c3-agentic-ai-platform-at-microsoft-ignite)
- [AI-Powered Industry Solutions at Microsoft Ignite 2025: Capgemini's Multi-Channel Knowledge Innovations](/ai/videos/ai-powered-industry-solutions-at-microsoft-ignite-2025-capgeminis-multi-channel-knowledge-innovations)
- [Transforming Enterprise Workflows with C3 AI Agentic Process Automation](/ai/videos/transforming-enterprise-workflows-with-c3-ai-agentic-process-automation)

## ML

At Ignite, machine learning updates centered on developer efficiency and fine-tuning at scale. This week’s coverage spotlights unified infrastructure, data platform improvements, and production agent guidance, all reinforcing the pattern toward integrated AI solutions, tuned models, and enterprise scaling within Azure.

### Microsoft Foundry and AI Agent Fine-Tuning

Furthering last week’s focus on custom workflows and model integration, Microsoft Foundry’s recent session covers all steps for producing and deploying tuned AI for real-world applications. This builds on Microsoft’s goal of making advanced ML techniques more widely available.

The session highlights Azure OpenAI and open-source models, with concrete examples using GPT-5 and O4 Mini. Synthetic data generation from Swagger specs also features heavily, supporting the need for robust training sets. Demos show how multiple agents collaborate to create, test, and improve synthetic data, increasing system reliability and business flexibility.

The ‘Navigator’ scenario illustrates how Foundry-powered agents process millions of contracts per day, underlining measurable benefits for both technical teams and leadership. Covered topics include model selection, API integration, and production deployment strategies, directly supporting earlier work in orchestration and ML.NET. For Azure or local teams, these guides bring ML workflows to greater maturity and scale.

- [AI Fine-Tuning in Microsoft Foundry: Building Production-Ready Agents](/ai/videos/ai-fine-tuning-in-microsoft-foundry-building-production-ready-agents)

### Azure Databricks: Unified Data and AI Ecosystem

Azure Databricks was featured as a unified analytics solution with extended integration in this week’s news. Tutorials cover new agent tools, such as Genie for rapid creation, Knowledge Assistant for management, and Multi-Agent Supervisor for routing—further supporting persistent workflow state and semantic data practices discussed previously.

The Databricks Connector, now improved for Power BI and Microsoft Apps, supports real-time data integration and workflow automation. The update to Databricks’ security tools—highlighted by Unity Catalog—matches the ongoing enterprise push for monitoring and compliance. Demonstrations, like EyeFi, reinforce Databricks’ expanding use in large organizations.

- [Accelerate Data and AI Transformation with Azure Databricks](/ai/videos/accelerate-data-and-ai-transformation-with-azure-databricks)

### Pushing the Boundaries: Azure AI Supercomputing Infrastructure

This week’s coverage dives into Azure’s updated supercomputing resources, with a focus on validating multi-billion parameter models using new GPU hardware (GB200/300, H100), advanced networking, and storage—building on past improvements in compute capacity.

Methodology guides for system inspection, performance tuning, and validation follow last week’s narrative around reliability and Azure’s blend of open source and built-in tooling. New GB300 GPUs expand capacity for growing models, and case studies (such as LLAMA and GRAC 314B) show Azure’s evolving capability for deployment and operations at scale.

- [Pushing Limits of Supercomputing Innovation on Azure AI Infrastructure](/ai/videos/pushing-limits-of-supercomputing-innovation-on-azure-ai-infrastructure)

## Azure

Azure’s newest releases maintain ongoing investment in cloud-native tools, AI-enhanced platforms, and modernization support. Ignite 2025 sessions illustrate Azure’s aim for easier multi-cloud deployment, real-time analytics, robust design, and secure hybrid integration. Updated features, public previews, and migration guides confirm the platform’s focus on scalable, intelligent workloads.

### Azure Cloud Native Development and Compute Innovations

Azure’s latest cycle builds on familiar themes—greater scalability and cross-platform capability. Multi-cloud management via Container Instances and improved serverless containers reinforce Azure’s pattern of accessible orchestration, extending the direction found in previous work such as the RADIUS project.

Advancements in eBPF-based networking and confidential container groups offer secured, fast workload isolation, matching compliance features discussed recently.

The public preview of Azure NCv6 GPU VMs boosts AI infrastructure, emphasizing support for visual and simulation workloads that tie back to the push toward efficient, multi-modal cloud operations. The DADS V7, V4L, and Cobalt 200 compute upgrades improve elasticity and reliability, reflecting feedback from recent user benchmarking efforts.

- [Cloud Native Innovations with Mark Russinovich: Ignite 2025 Breakout](/azure/videos/cloud-native-innovations-with-mark-russinovich-ignite-2025-breakout)
- [Azure NCv6 Public Preview: Unified Platform for Converged AI & Visual Computing](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-ncv6-public-preview-the-new-unified-platform-for-converged/ba-p/4472704)
- [Powering Modern Cloud Workloads with Azure Compute](/ai/videos/powering-modern-cloud-workloads-with-azure-compute)

### Observability, Automation, and Operational Resilience

Azure SRE Agent expands last week’s observability guidance, adding support for metrics across clouds. New integrations with external monitoring and MCP server capabilities support OpenTelemetry-based metrics for a variety of languages, reinforcing the standardization drive.

Collaborative incident response features, like integration with PagerDuty and Hawkeye, increase operational resilience—ongoing themes in Azure’s diagnostic improvements.

Copilot now works within Azure Monitor, providing live insights for cost and troubleshooting—an extension of last week’s new dashboards and query options. Support for Grafana and Prompt QL moves workflows closer to real-time for distributed Kubernetes environments.

- [Azure SRE Agent: Enhancing Observability and Multi-Cloud Incident Management](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-expanding-observability-and-multi-cloud/ba-p/4472719)
- [Unlock Cloud-Scale Observability and Optimization with Azure Monitor](/ai/videos/unlock-cloud-scale-observability-and-optimization-with-azure-monitor)

### Data Platform Updates: Microsoft Fabric, SQL, and Data Integration

Microsoft Fabric continues to build out its analytics platform, with hands-on demos and new features like in-place analytics for cloud backups and expanded OneLake support. General availability of Copy Job Activity and Data Virtualization for SQL are among several orchestration and compliance updates. Support for large object types, variable libraries, and improved workflow automation now caters to developer requests, as reflected in recent simplification efforts.

Logic Apps now process XML directly, aiding legacy-to-cloud modernization as explored previously.

- [SQL Database in Microsoft Fabric: Unified Platform for AI Apps and Analytics](/ai/videos/sql-database-in-microsoft-fabric-unified-platform-for-ai-apps-and-analytics)
- [Copy Job Activity Now Generally Available in Microsoft Fabric Data Factory Pipelines](https://blog.fabric.microsoft.com/en-US/blog/announcing-copy-job-activity-now-general-available-in-data-factory-pipeline/)
- [Data Virtualization and External Tables in Fabric SQL Databases (Preview)](https://blog.fabric.microsoft.com/en-US/blog/openrowset-and-external-tables-for-fabric-sql-databases/)
- [Large Object Data Support in Microsoft Fabric Data Warehouse and SQL Analytics Endpoint](https://blog.fabric.microsoft.com/en-US/blog/large-string-and-binary-values-in-fabric-data-warehouse-and-sql-analytics-endpoint-for-mirrored-items-general-availability/)
- [Managing Environment Configuration in Microsoft Fabric with Variable Libraries](https://blog.fabric.microsoft.com/en-US/blog/manage-environment-configuration-in-fabric-user-data-functions-with-variable-libraries/)
- [Querying Database Backups in Microsoft Fabric: In-Place Analytics Without ETL](https://blog.fabric.microsoft.com/en-US/blog/30438/)
- [General Availability of XML Parse and Compose Actions for Azure Logic Apps](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-the-general-availability-of-the-xml-parse-and-compose/ba-p/4470825)

### Edge and Hybrid AI: Azure Local, Deployment Automation, and Lenovo Partnership

Hybrid and regulated cloud options expand further, with Microsoft introducing Private Sovereign Cloud, NVIDIA RTX acceleration, and integrated partner solutions. Case studies with Lenovo and LOCA illustrate fast, automated deployment supporting data sovereignty, building on last week’s modernization examples.

Orchestration for AKS, migration, and custom model deployment all support practical multi-environment event-driven design.

- [What’s New in Azure Local: Portfolio Enhancements & Edge AI Innovations](/ai/videos/whats-new-in-azure-local-portfolio-enhancements-and-edge-ai-innovations)
- [Simplifying Azure Local Deployments with Lenovo ThinkAgile MX and LOCA](/azure/videos/simplifying-azure-local-deployments-with-lenovo-thinkagile-mx-and-loca)
- [Powering Hybrid AI with Azure Local and Lenovo: Ignite 2025 Deep Dive](/ai/videos/powering-hybrid-ai-with-azure-local-and-lenovo-ignite-2025-deep-dive)

### Logic Apps, Integration, and Workflow Automation

Logic Apps further develop agentic workflow patterns, extending the Model Context Protocol (MCP) highlighted last week. Guides for HL7 and BizTalk migration support teams in updating old processes to modern, cloud-based flows.

Features for cloning workflows and upgrading support ongoing modernization work, making transitions manageable and less risky. Demonstrations confirm Azure’s focus on developer-centered integration and tool improvement.

- [The Future of Integration: Agentic Workflows and AI-Driven Patterns with Azure Logic Apps](/ai/videos/the-future-of-integration-agentic-workflows-and-ai-driven-patterns-with-azure-logic-apps)
- [Announcing Public Preview: HL7 Connector for Azure Logic Apps (Standard & Hybrid)](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-the-hl7-connector-for-azure-logic-apps-standard-and/ba-p/4470690)
- [Clone a Consumption Logic App to a Standard Workflow](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/clone-a-consumption-logic-app-to-a-standard-workflow/ba-p/4471175)

### Resiliency, Backup, and Secure Cloud Architecture

The “resilience by design” approach continues, providing updated frameworks for secure architectures and backup best practices. New guides for VM, Kubernetes, and microservice protection use Azure Recovery Services, Defender automation, and immutable storage to address shared responsibility and rapid response—building on established priorities.

- [Resilience by Design: Secure, Scalable, AI-Ready Cloud with Azure](/ai/videos/resilience-by-design-secure-scalable-ai-ready-cloud-with-azure)
- [Resiliency and Recovery with Azure Backup and Site Recovery](/azure/videos/resiliency-and-recovery-with-azure-backup-and-site-recovery)

### Modernization, Migration, and Partner Solutions

Modernization remains a strong topic, with session guides on retail, finance, and public sector migration following last week’s best practices. These updates further Azure’s support for organizations planning IT renewals using AI-powered platforms and detailed adoption guidance.

- [Sam’s Club: Modernizing Retail Mission-Critical Apps with Azure](/azure/videos/sams-club-modernizing-retail-mission-critical-apps-with-azure)
- [Levi’s Global IT Transformation: Migration and Modernization with Azure](/ai/videos/levis-global-it-transformation-migration-and-modernization-with-azure)
- [Migration Lessons from Microsoft Federal's RISE with SAP Deployment](/ai/videos/migration-lessons-from-microsoft-federals-rise-with-sap-deployment)
- [Modernize on-premises VMware environments with Azure VMware Solution](/ai/videos/modernize-on-premises-vmware-environments-with-azure-vmware-solution)
- [Accelerating Migration and Modernization in Financial Services with Microsoft Cloud Accelerate Factory](/ai/videos/accelerating-migration-and-modernization-in-financial-services-with-microsoft-cloud-accelerate-factory)
- [Migration & Modernization Strategies for Partners: Azure-Focused Growth at MS Ignite 2025](/ai/videos/migration-and-modernization-strategies-for-partners-azure-focused-growth-at-ms-ignite-2025)

### Azure IaaS, Infrastructure Optimization, and Cost Management

Optimization continues to be a focus, with new advice for Azure IaaS using Azure Boost, Compute Fleet, Ultra Disk, and blob storage tiering strategies. Networking upgrades and scaling for App Gateway and ExpressRoute are ongoing, supporting a transition to more cost-effective and higher-performance operations.

Updated resources link back to Copilot’s cost and reservation management, giving practical steps for reducing spend on infrastructure.

- [Azure IaaS Best Practices to Enhance Performance and Scale](/azure/videos/azure-iaas-best-practices-to-enhance-performance-and-scale)
- [Driving Efficiency and Cost Optimization for Azure IaaS Deployments](/azure/videos/driving-efficiency-and-cost-optimization-for-azure-iaas-deployments)
- [Drive Cost Efficiency and Elevate Azure ROI with Strategic Architecture | BRK216](/ai/videos/drive-cost-efficiency-and-elevate-azure-roi-with-strategic-architecture-brk216)
- [Optimizing Data Analytics Costs with Azure Reservations for Microsoft Fabric](https://techcommunity.microsoft.com/t5/finops-blog/streamline-analytics-spend-on-microsoft-fabric-with-azure/ba-p/4472670)

### Security, AI Governance, and Endpoint Management

Improved security policies, integrated device management, and AI governance build on last week’s new baselines and detection features. Intune, Defender for Cloud, and new endpoint management options allow for streamlined policy enforcement and automation.

Guides for API Management in Copilot and other AI scenarios continue to support secure and traceable integration, reflecting recent efforts to build stronger governance structures.

- [What's New in Microsoft Intune: AI-Driven Endpoint Security and IT Empowerment](/ai/videos/whats-new-in-microsoft-intune-ai-driven-endpoint-security-and-it-empowerment)
- [Govern AI Agents with Azure API Management: Secure, Monitor, and Scale AI Workloads](/ai/videos/govern-ai-agents-with-azure-api-management-secure-monitor-and-scale-ai-workloads)

### Other Azure News

Updates this week address optimized Linux deployments, eBPF instrumentation, and improved image security, offering practical guides for sysadmins working on Azure deployments.

- [Optimizing Linux Deployments: Performance and Security on Azure](/azure/videos/optimizing-linux-deployments-performance-and-security-on-azure)

Advances in agentic platforms, including Copilot and AKS Automatic features, further automate compliance and improve developer experience.

- [Scale Smarter: Azure Infrastructure for the Agentic Era](/ai/videos/scale-smarter-azure-infrastructure-for-the-agentic-era)

Ongoing integration with Citrix Cloud builds on last week’s coverage of workplace modernization and hybrid optimization for remote work environments.

- [Optimizing Azure Investments with Citrix: Security, Cost, and Experience](/azure/videos/optimizing-azure-investments-with-citrix-security-cost-and-experience)

A historical session featuring Mark Russinovich and Scott Hanselman explores computing from Altair 8800 to Azure, illustrating the ongoing context of platform modernization.

- [Connecting Computing Eras: From Altair 8800 to Azure Cloud Architecture](/azure/videos/connecting-computing-eras-from-altair-8800-to-azure-cloud-architecture)

Updates on IoT devices and retail showcase Azure’s evolving role in secure analytics and smart device management.

- [Scaling Innovation in Smart Eyewear and Connected Retail with Azure and AI](/ai/videos/scaling-innovation-in-smart-eyewear-and-connected-retail-with-azure-and-ai)

## Coding

This week in Coding covers improved developer tools, new integrations, and best practices for creating sturdy applications. Updates range across IDE enhancements, built-in AI support, and pragmatic architecture advice. With ongoing improvements in Visual Studio 2026 and the Windows 11 developer suite, Microsoft continues to streamline iteration, boost security, and expand AI’s role in everyday coding. In-depth articles on .NET startup routines and sound C# class design reinforce the move toward maintainable, high-quality code in the Microsoft ecosystem.

### Modern IDEs and the Windows Developer Experience

Following Visual Studio 2026’s launch last week, more detail is available on how monthly updates let developers manage innovation and stability by choosing between Insiders, Stable, and LTS channels. Major components like .NET and C++ compilers are now modular, supporting independent updates—an advance from prior .NET improvements.

Copilot is integrated further in Visual Studio 2026, including independent Copilot updates for up-to-date AI coding support. This addresses developer needs for fast adaptation to evolving features.

Windows 11’s toolkit also grows: PowerToys and Windows Terminal now add enhanced security and automation, featuring Copilot in Terminal and new command line tools, as recently previewed. The new 'Edit' terminal utility and MCP Server deployment as AI agent underline Microsoft’s ongoing support for seamless automation and Azure integration.

- [Visual Studio 2026: Modern IDE with Monthly Updates and Flexible Build Tools](https://devblogs.microsoft.com/visualstudio/visual-studio-built-for-the-speed-of-modern-development/)
- [Windows 11 Developer Productivity Tools: WSL, Terminal, PowerToys & Enterprise Security](/coding/videos/windows-11-developer-productivity-tools-wsl-terminal-powertoys-and-enterprise-security)

### .NET Diagnostics and C# Design Discipline

.NET internals and diagnostics remain a focus, with Andrew Lock’s exploration of the .NET boot sequence complementing previous guides on startup and hosting.

Nick Chapsas’ summary on default class sealing practices supports conversations about code maintainability and extensibility, emphasizing the default use of sealed classes and clear extension points—topical as teams seek more robust design approaches.

- [Exploring the .NET Boot Process via Host Tracing](https://andrewlock.net/exploring-the-dotnet-boot-process-via-host-tracing/)
- [Every Class Should Be Sealed in C#](/coding/videos/every-class-should-be-sealed-in-c)

## DevOps

DevOps continues to enhance configuration management and automated setup for Windows, building on last week’s emphasis on code-based infrastructure for smoother developer workflows. The goal remains consistency, simple onboarding, and flexible extension across environments.

### WinGet and Desired State Configuration Integration

This week’s milestone announcement is the integration of Desired State Configuration (DSC) with WinGet, featuring code-driven setup for Windows machines. Teams can now automate app configuration and policy enforcement, reducing repetitive tasks and ensuring consistent environments for developers and production teams. This matches the trend toward policy-driven automation described before.

Guides now show how to export and reuse configuration templates, making standard set-up easy. The updates fit with ongoing patterns for onboarding, compliance, and policy-driven automation now built directly into Windows.

Additional improvements include interface updates, advanced font handling, new command line features, and PowerShell integration. WinGet Studio, an open community portal, now assists with plugin sharing and customization—expanding community input and evolution found in previous DevOps news.

These updates provide faster onboarding and reliable setup for both development and IT teams, strengthening the open, adaptable nature of modern DevOps on Windows.

- [Fast and Easy Windows Setup & Configuration with WinGet and Desired State Configuration](/coding/videos/fast-and-easy-windows-setup-and-configuration-with-winget-and-desired-state-configuration)

## Security

Security news this week features new AI-powered protections for cloud, endpoints, and collaboration, with a focus on governance and operational agility. Ignite 2025 delivered sessions and resources on secure agentic AI, confidential computing, automated SOC, and broad partnerships. Updates in memory-safe platform hardware, managed agent lifecycle, and practical incident response are supported by more tools for hardening devices, data, and drivers.

### Advancements in Confidential Computing and Memory-Safe Platform Security

Extending last week’s progress in custom baselines and memory safety, Azure has now implemented deeper Intel Trusted Domain Extensions (TDX) in collaboration with Bosch and Intel, providing stronger isolation for high-assurance workloads.

Microsoft continues to move system code to Rust for firmware and drivers, with new ecosystem support through windows-drivers-rs and Cargo WDK. Secure Core PC and enhanced DFCI control also get newly updated deployment tools for IT.

- [Advancing Confidential Computing: Bosch, Microsoft Azure, & Intel TDX](/ai/videos/advancing-confidential-computing-bosch-microsoft-azure-and-intel-tdx)
- [Advancing Windows Device Security with Surface Innovation and Memory-Safe Rust Drivers](/coding/videos/advancing-windows-device-security-with-surface-innovation-and-memory-safe-rust-drivers)

### Securing Agentic AI: Lifecycle, Governance, and Risk Management

This week’s updates highlight new tools for threat modeling and governance in agentic AI, directly addressing risks like prompt injection and memory tampering previously discussed.

Microsoft Agent 365 builds on centralized audit trails, conditional access, and DLP, as well as enhancements in Defender, Purview, and Entra for fine-grained monitoring of AI workflows, continuing the push for clear oversight and risk controls.

- [Building Secure AI Agents with Microsoft’s Security Stack](/ai/videos/building-secure-ai-agents-with-microsofts-security-stack)
- [Explore Microsoft Agent 365 Security and Governance Capabilities](/ai/videos/explore-microsoft-agent-365-security-and-governance-capabilities)
- [Securing AI at Scale: Microsoft’s Latest Innovations in Agent, App, and Data Protection](/ai/videos/securing-ai-at-scale-microsofts-latest-innovations-in-agent-app-and-data-protection)
- [Leading with Trust: Building & Deploying Agents in a Regulated World](/ai/videos/leading-with-trust-building-and-deploying-agents-in-a-regulated-world)

### Security Copilot, SOC Automation, and Microsoft Defender Ecosystem

Security Copilot introduces agent-based automation for SOC teams, including persistent threat memory and daily briefings. The new Security Compute Unit provides a clearer cost and access model to support these changes.

Microsoft Sentinel’s updates on analytics and cross-system coverage assist with centralizing monitoring efforts. Defender for Cloud continues progress with AI-driven attack detection, expanding on previous themes of dashboard integration and proactive protection.

- [Security Copilot: Empowering Security Teams with AI at Microsoft Ignite 2025](/ai/videos/security-copilot-empowering-security-teams-with-ai-at-microsoft-ignite-2025)
- [Empowering the SOC: Security Copilot and the Rise of Agentic Defense](/ai/videos/empowering-the-soc-security-copilot-and-the-rise-of-agentic-defense)
- [Amplifying SecOps Practices with Microsoft Sentinel and Unified Platform](/ai/videos/amplifying-secops-practices-with-microsoft-sentinel-and-unified-platform)
- [Build Secure Applications with Defender and Azure Network Security](/azure/videos/build-secure-applications-with-defender-and-azure-network-security)
- [AI-powered Defense Strategies for Cloud Workloads with Microsoft Defender](/ai/videos/ai-powered-defense-strategies-for-cloud-workloads-with-microsoft-defender)

### Microsoft Purview and Enterprise Data Security

Expanded Microsoft Purview features continue to focus on data security and compliance, supporting organizations as they incorporate Copilot and generative AI into operations. Features like DSPM, automated labeling, and alert fatigue reduction are included, showing how AI can reduce manual effort and speed up compliance work.

Case studies reinforce that automation tools and adaptive policy management are delivering measurable gains, moving from recent pilot phases into everyday use.

- [Securing Data Across Microsoft Environments with Microsoft Purview](/azure/videos/securing-data-across-microsoft-environments-with-microsoft-purview)
- [Secure-by-Design Transformation: PwC and Microsoft Purview Enhancing Data Security](/security/videos/secure-by-design-transformation-pwc-and-microsoft-purview-enhancing-data-security)
- [AI-Powered Data Security with Security Copilot and Microsoft Purview](/ai/videos/ai-powered-data-security-with-security-copilot-and-microsoft-purview)

### Identity, Zero Trust, and Cross-Platform Security

Microsoft Entra and Intune lead ongoing Zero Trust efforts by adding adaptive access and security policies shaped by AI, echoing advances discussed in authentication and device management. The new Intune capabilities bolster risk identification and support secure AI adoption across infrastructures.

- [Accelerating Zero Trust and Securing AI Access with Microsoft Entra Suite](/ai/videos/accelerating-zero-trust-and-securing-ai-access-with-microsoft-entra-suite)
- [Demystifying Zero Trust Endpoint Management with Microsoft Intune](/security/videos/demystifying-zero-trust-endpoint-management-with-microsoft-intune)

### Integrated SOC Visibility, Threat Intelligence, and Third-Party Security Partnerships

Strategic partnerships with solutions like Lumen Defender and Cisco on Azure add new joint telemetry and SOC visibility, enriching detection and operational awareness as seen in past security updates.

- [Lumen Defender and Microsoft Security: Enhancing SOC Threat Detection and Response](/security/videos/lumen-defender-and-microsoft-security-enhancing-soc-threat-detection-and-response)
- [Unified Digital Resilience: Integrating Cisco and Microsoft Security on Azure](/ai/videos/unified-digital-resilience-integrating-cisco-and-microsoft-security-on-azure)

### Other Security News

Expanded managed security services such as Defender Experts for XDR and incident response teams build on last week’s detailed coverage. Updates promote best practices for threat detection, patch management, and resilient operations.

Updates for GitHub’s DevSecOps automation cover essentials like policy administration and package validation, supporting Copilot and agent workflows at scale.

Cloud security features for telco and wireless environments follow the established direction of enhanced authentication and orchestration. Commvault SHIFT now brings additional AI-powered data resilience and Zero Trust integration for Microsoft platforms.

- [Microsoft Security Experts: Enhancing Your SOC with Managed XDR and Incident Response](/security/videos/microsoft-security-experts-enhancing-your-soc-with-managed-xdr-and-incident-response)
- [Enterprise Security and Governance on GitHub: Best Practices from Ignite 2025](/devops/videos/enterprise-security-and-governance-on-github-best-practices-from-ignite-2025)
- [Securing Private Wireless: From Design to Deployment](/ai/videos/securing-private-wireless-from-design-to-deployment)
- [Commvault SHIFT Virtual: AI and Cyber Resilience Insights for Microsoft Identity and Cloud](https://www.thomasmaurer.ch/2025/11/commvault-shift-virtual-a-new-era-of-ai-driven-cyber-resilience-on-demand/)
