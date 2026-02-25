---
external_url: /github-copilot/roundups/Weekly-AI-and-Tech-News-Roundup
title: Agentic Workflows, AI Integration, and Security-Focused Cloud Developments
author: TechHub
primary_section: github-copilot
date: 2026-02-23 09:00:00 +00:00
feed_name: TechHub
tags:
- AI
- AI Agents
- AI Models
- Azure
- CI/CD
- Cloud Reliability
- Data Engineering
- DevOps
- Enterprise Migration
- GitHub Copilot
- Kubernetes
- Microsoft Foundry
- ML
- Open Source
- Roundups
- Security
- VS Code
- Workflow Automation
- .NET
section_names:
- ai
- github-copilot
- ml
- azure
- dotnet
- devops
- security
---
Welcome to this week’s tech summary, where automation and agent technology continue to influence development practices. GitHub Copilot now provides greater support for autonomous workflows and AI model integrations, making it easier for teams to boost productivity across development environments. Microsoft Foundry and Azure add new features for multi-language agent orchestration, platform reliability, and secure migration—delivering the foundation for scalable agent-driven cloud projects. Updated tools and security practices help teams streamline workflow, improve compliance, and collaborate more effectively on open source and enterprise solutions.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Copilot Coding Agents: Autonomous Workflows and Expanded Integration](#copilot-coding-agents-autonomous-workflows-and-expanded-integration)
  - [Model Context Protocol (MCP) and Interactive Agent Experiences](#model-context-protocol-mcp-and-interactive-agent-experiences)
  - [Expanded AI Model Support: Claude, Gemini, and BYOK](#expanded-ai-model-support-claude-gemini-and-byok)
  - [Custom Agents and Unified Agent Management](#custom-agents-and-unified-agent-management)
  - [Agentic Workflows, Continuous AI, and Automation Enhancements](#agentic-workflows-continuous-ai-and-automation-enhancements)
  - [Copilot Integration: VS Code, Zed, and SQL Tools](#copilot-integration-vs-code-zed-and-sql-tools)
  - [Developer Workflow Optimization: App Modernization and Prompts](#developer-workflow-optimization-app-modernization-and-prompts)
  - [Copilot Usage Metrics and Analytics](#copilot-usage-metrics-and-analytics)
  - [Developer Trends and Security Tools](#developer-trends-and-security-tools)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Microsoft Agent Framework: Cross-Language Orchestration and Migration](#microsoft-agent-framework-cross-language-orchestration-and-migration)
  - [Microsoft Foundry: Model, SDK, and Platform Updates](#microsoft-foundry-model-sdk-and-platform-updates)
  - [Building Practical Solutions with Microsoft Foundry Local AI](#building-practical-solutions-with-microsoft-foundry-local-ai)
  - [Microsoft Foundry: Frontier Model Integration and Reasoning Agents Challenges](#microsoft-foundry-frontier-model-integration-and-reasoning-agents-challenges)
  - [Agentic AI Workflows and Integration in Visual Studio Code](#agentic-ai-workflows-and-integration-in-visual-studio-code)
  - [Affordable AI Development and Practical Tutorials](#affordable-ai-development-and-practical-tutorials)
  - [Telecom Infrastructure, Agentic BSS, and Intelligent Edge](#telecom-infrastructure-agentic-bss-and-intelligent-edge)
  - [Developer Workflow and Software Practices in the Age of AI Coding](#developer-workflow-and-software-practices-in-the-age-of-ai-coding)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Microsoft ODBC Driver for Fabric Data Engineering Preview](#microsoft-odbc-driver-for-fabric-data-engineering-preview)
- [Azure](#azure)
  - [Cloud Reliability, Resiliency, and Recoverability](#cloud-reliability-resiliency-and-recoverability)
  - [Azure Kubernetes Service and Container Routing](#azure-kubernetes-service-and-container-routing)
  - [Enterprise Workflow Integration: Azure Logic Apps and SAP](#enterprise-workflow-integration-azure-logic-apps-and-sap)
  - [Azure AI Infrastructure and Performance](#azure-ai-infrastructure-and-performance)
  - [Site Reliability Engineering and DevOps Automation in Azure](#site-reliability-engineering-and-devops-automation-in-azure)
  - [Migration and Modernization: Cloud Workload Transitions](#migration-and-modernization-cloud-workload-transitions)
  - [Azure Managed Disk and Storage Enhancements](#azure-managed-disk-and-storage-enhancements)
  - [Azure Virtual Desktop: Lifecycle Management and Connectivity](#azure-virtual-desktop-lifecycle-management-and-connectivity)
  - [Azure Virtual Machine Scale Sets and Networking](#azure-virtual-machine-scale-sets-and-networking)
  - [Azure API Management for Enterprise AI and Multi-Region Deployments](#azure-api-management-for-enterprise-ai-and-multi-region-deployments)
  - [Microsoft Fabric Analytics, Security, and Identity](#microsoft-fabric-analytics-security-and-identity)
  - [Azure Network Troubleshooting and Sovereignty Solutions](#azure-network-troubleshooting-and-sovereignty-solutions)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET Runtime and Instrumentation Advances](#net-runtime-and-instrumentation-advances)
  - [Visual Studio Code Workflow and Browser Integration](#visual-studio-code-workflow-and-browser-integration)
  - [Open Source Ecosystems and Language Histories](#open-source-ecosystems-and-language-histories)
  - [Windows MIDI Services: Next-Gen Music Tech on Windows 11](#windows-midi-services-next-gen-music-tech-on-windows-11)
  - [PowerShell, OpenSSH, and DSC: Long-Term Roadmap](#powershell-openssh-and-dsc-long-term-roadmap)
  - [Inside Model Context Protocol: Workflow and Open Source Journey](#inside-model-context-protocol-workflow-and-open-source-journey)
- [DevOps](#devops)
  - [GitHub Platform and Workflow Enhancements](#github-platform-and-workflow-enhancements)
  - [Azure DevOps and SRE Automation](#azure-devops-and-sre-automation)
  - [Microsoft Fabric CI/CD Automation](#microsoft-fabric-cicd-automation)
  - [Open Source Ecosystem Developments](#open-source-ecosystem-developments)
- [Security](#security)
  - [GitHub Enterprise Credential Management and Secret Scanning](#github-enterprise-credential-management-and-secret-scanning)
  - [Supply Chain Security: npm CLI and Open Source AI Libraries](#supply-chain-security-npm-cli-and-open-source-ai-libraries)
  - [Self-Hosted Agent Runtimes: OpenClaw Identity, Isolation, and Monitoring](#self-hosted-agent-runtimes-openclaw-identity-isolation-and-monitoring)
  - [Digital Media Authentication: AI, Provenance, and Emerging Standards](#digital-media-authentication-ai-provenance-and-emerging-standards)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot continues to expand its feature set, integrations, and workflows. New agent workflows, model support, and interactive capabilities are providing more ways for developers to automate coding, documentation, and review. The range of updates covers cloud-based agent automation, deeper editor integrations, custom workflow flexibility, enhanced model selection, and new security features—focused on creating a more responsive and productive development environment.

### Copilot Coding Agents: Autonomous Workflows and Expanded Integration

Copilot Coding Agents now support more environments, allowing developers to delegate tasks from Visual Studio, VS Code, GitHub, the CLI, and Raycast. Visual Studio 2026 and newer versions support sending Copilot Chat work to agents for asynchronous execution, improving agent-driven pull request creation and notifications that were part of recent IDE and Actions updates. Raycast-based assignment and monitoring add support for cross-platform task delegation, complementing last week’s improvements to GitHub and VS Code workflows. Business and Enterprise users get advanced model picker options for Claude, GPT-5 Codex, and additional model choices. These updates reflect a continued shift toward enhanced, flexible, and secure agent-powered coding.

- [Running Code Generation in the Background with GitHub Copilot Coding Agents](https://github.blog/changelog/2026-02-18-Running-Code-Generation-in-the-Background-with-GitHub-Copilot-Coding-Agents.html)
- [Delegate Tasks to GitHub Copilot Coding Agent from Visual Studio](https://github.blog/changelog/2026-02-17-delegate-tasks-to-copilot-coding-agent-from-visual-studio)
- [Assigning Issues to GitHub Copilot Coding Agent from Raycast](https://github.blog/changelog/2026-02-17-assign-issues-to-copilot-coding-agent-from-raycast)
- [Copilot Coding Agent Supports Code Referencing](https://github.blog/changelog/2026-02-18-copilot-coding-agent-supports-code-referencing)
- [Copilot Coding Agent Model Picker for Business and Enterprise Users](https://github.blog/changelog/2026-02-19-model-picker-for-copilot-coding-agent-for-copilot-business-and-enterprise-users)
- [How to Use Copilot Coding Agent with Windows Projects](https://github.blog/changelog/2026-02-18-use-copilot-coding-agent-with-windows-projects)

### Model Context Protocol (MCP) and Interactive Agent Experiences

Model Context Protocol (MCP) enhancements offer better agent coordination and extensibility for VS Code and open agent frameworks. Visuals MCP brings interactive UI components into open source, allowing agents to display dashboards, tables, and previews during development tasks. The MCP Registry for Eclipse unifies agent toolchains, while updated preference and task management options let users personalize agent interactions. By supporting markdown-based agents on Azure Functions with Copilot SDK and AGENTS.md integration, cloud distribution and event-driven agent automation become easier.

- [Building Interactive UI Components for AI Agents with Visuals MCP](https://harrybin.de/posts/visuals-mcp-server/)
- [Introducing MCP Apps: Interactive UI Components in VS Code Chat](/2026-02-20-Introducing-MCP-Apps-Interactive-UI-Components-in-VS-Code-Chat.html)
- [MCP Registry and New GitHub Copilot Features in Eclipse](https://github.blog/changelog/2026-02-17-mcp-registry-and-more-improvements-in-copilot-in-eclipse)
- [Hosting Declarative Markdown-Based Agents on Azure Functions with GitHub Copilot SDK](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/host-declarative-markdown-based-agents-on-azure-functions/ba-p/4496038)

### Expanded AI Model Support: Claude, Gemini, and BYOK

GitHub Copilot now supports more models. Claude Opus 4.6 and Claude Sonnet 4.6 are generally available, while Gemini 3.1 Pro is offered in public preview. The BYOK (Bring Your Own Key) feature in VS Code allows teams to use their own API keys or self-hosted and open source models. These changes reflect ongoing attention to flexible model selection. Some older Anthropic and OpenAI models are being deprecated in line with the focus on performance and updated support.

- [Claude Sonnet 4.6 Now Available in GitHub Copilot](https://github.blog/changelog/2026-02-17-claude-sonnet-4-6-is-now-generally-available-in-github-copilot)
- [Claude Opus 4.6 Integration with GitHub Copilot Available in Popular IDEs](https://github.blog/changelog/2026-02-18-claude-opus-4-6-is-now-available-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [Gemini 3.1 Pro Public Preview in GitHub Copilot](https://github.blog/changelog/2026-02-19-gemini-3-1-pro-is-now-in-public-preview-in-github-copilot)
- [Bring Your Own AI Models to Visual Studio Code](/2026-02-20-Bring-Your-Own-AI-Models-to-Visual-Studio-Code.html)
- [GitHub Copilot Deprecates Selected Anthropic and OpenAI Models](https://github.blog/changelog/2026-02-19-selected-anthropic-and-openai-models-are-now-deprecated)

### Custom Agents and Unified Agent Management

With Visual Studio support for both built-in and custom agents, developers now have access to new debugging and modernization workflows, matching recent rollouts of agent skills and .agent.md standards for JetBrains and VS Code. Custom agent roles and project context tools encourage modular and team-based automation. VS Code brings together management for local, cloud, and partner agents, building toward the vision of unified agent handoff and coordination—sometimes called “Agent HQ.”

- [Custom Agents in Visual Studio: Built-in and Custom Workflows with GitHub Copilot](https://devblogs.microsoft.com/visualstudio/custom-agents-in-visual-studio-built-in-and-build-your-own-agents/)
- [A Unified Agent Experience in Visual Studio Code](/2026-02-20-A-Unified-Agent-Experience-in-Visual-Studio-Code.html)
- [Customize GitHub Copilot Agents in Visual Studio Code](/2026-02-20-Customize-GitHub-Copilot-Agents-in-Visual-Studio-Code.html)

### Agentic Workflows, Continuous AI, and Automation Enhancements

GitHub’s preview of agent-driven repository automation builds on recent markdown/YAML-based automation and Actions improvements. Copilot, OpenAI agents, and secure container features offer new options for fine-grained controls, making complex bot orchestration possible. Agent HQ is moving toward improved security, fleet management, and workflow reproducibility, supporting safer automation for non-critical pull requests while reproducibility practices develop further.

- [GitHub Previews Agentic Workflows as Part of Continuous AI Concept](https://www.devclass.com/ci-cd/2026/02/17/github-previews-agentic-workflows-as-part-of-continuous-ai-concept/4091356)
- [The Download: Agentic Workflows, New AI Models, and GitHub Updates](/2026-02-20-The-Download-Agentic-Workflows-New-AI-Models-and-GitHub-Updates.html)

### Copilot Integration: VS Code, Zed, and SQL Tools

Copilot now works in the Zed Editor—offering chat, completions, and code suggestions in more places. VS Code adds CLI integration and session tracking to strengthen its feature set. SQL toolkit improvements provide Copilot-powered support inside SSMS, VS Code MSSQL, and Microsoft Fabric Query Editor, simplifying query generation and automation for teams moving from Azure Data Studio.

- [GitHub Copilot Now Supported in Zed Editor](https://github.blog/changelog/2026-02-19-github-copilot-support-in-zed-generally-available)
- [Integrating Copilot CLI with Visual Studio Code](/2026-02-20-Integrating-Copilot-CLI-with-Visual-Studio-Code.html)
- [Let it Cook: Latest Updates in VS Code and GitHub Copilot](/2026-02-18-Let-it-Cook-Latest-Updates-in-VS-Code-and-GitHub-Copilot.html)
- [AI-Powered Assistants in SSMS, VS Code, and Fabric: GitHub Copilot for SQL Developers](https://blog.fabric.microsoft.com/en-US/blog/no-more-excuses-ai-powered-assistants-are-in-ssms-vs-code-and-fabric/)

### Developer Workflow Optimization: App Modernization and Prompts

New Copilot migration guides provide support for legacy .NET upgrades—covering containerization, managed identities, and app restructuring. These guides are based on feedback about common .NET and Azure migration scenarios and highlight the practical impact of Copilot automation. The prompts.chat resource offers growing collections of reusable prompt templates to help teams accelerate processes by sharing best practices.

- [From "Maybe Next Quarter" to "Running Before Lunch" on Container Apps - Modernizing Legacy .NET App](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-quot-maybe-next-quarter-quot-to-quot-running-before-lunch/ba-p/4495736)
- [Open Source Friday: Practical Prompt Patterns with prompts.chat](/2026-02-17-Open-Source-Friday-Practical-Prompt-Patterns-with-promptschat.html)

### Copilot Usage Metrics and Analytics

Copilot provides organization-wide dashboards and APIs for real-time monitoring of usage and developer productivity. These tools help teams meet compliance policies and give engineering leads better data for analyzing PR cycle times, review speed, and DevOps performance. The new metrics go beyond past status reports or outage analytics, giving actionable insights for daily process improvement.

- [Organization-Level GitHub Copilot Usage Metrics Dashboard Public Preview](https://github.blog/changelog/2026-02-20-organization-level-copilot-usage-metrics-dashboard-available-in-public-preview)
- [Pull Request Throughput and Time to Merge in GitHub Copilot Usage Metrics API](https://github.blog/changelog/2026-02-19-pull-request-throughput-and-time-to-merge-available-in-copilot-usage-metrics-api)

### Developer Trends and Security Tools

Octoverse data highlights how Copilot affects the technologies and languages developers prefer, including growth in TypeScript and more adoption of strongly-typed AI integrations. The GitHub Secure Open Source Fund continues to drive improved security, including autofix, scanning, and Copilot-based workflows for open source projects—maintaining last week’s emphasis on audit logs and automated governance.

- [How AI is Reshaping Developer Choice: Insights from Octoverse 2025](https://github.blog/ai-and-ml/generative-ai/how-ai-is-reshaping-developer-choice-and-octoverse-data-proves-it/)
- [Open Source Friday: GitHub Secure Open Source Fund and the Future of Supply Chain Security](/2026-02-18-Open-Source-Friday-GitHub-Secure-Open-Source-Fund-and-the-Future-of-Supply-Chain-Security.html)

### Other GitHub Copilot News

Visual Studio 2026 introduces built-in support for Mermaid diagramming, integrated with Copilot to help plan architectures and document workflows more easily. These features add to past architectural tooling in the IDE, with stronger Copilot connectivity. Additional updates show steady progress in agent interoperability and model partnerships, moving toward a consistent agentic ecosystem.

- [Visualizing Workflows and Architecture with Mermaid Charts in Visual Studio 2026](https://techcommunity.microsoft.com/t5/tools/visualize-workflows-and-architecture-with-mermaid-charts-in/m-p/4495253#M190)

- [Leveraging Claude Opus 4.6 and Agents in GitHub Copilot for Advanced Coding Tasks](https://techcommunity.microsoft.com/t5/apps-on-azure/using-claude-opus-4-6-in-github-copilot/m/p/4495127#M1393)

## AI

This week, AI updates include new tools, frameworks, and guidance for implementing agent-based systems, multimodal applications, and workflow integration across Microsoft Foundry, VS Code, and Azure. Progress continues on cross-language agent frameworks, enterprise modeling, and affordable deployment, all supporting faster, more flexible development spanning industries such as healthcare, telecom, and business software. Live events and guides make these advancements easier to adopt and use in practice.

### Microsoft Agent Framework: Cross-Language Orchestration and Migration

Microsoft Agent Framework is now a Release Candidate, providing cross-language orchestration for AI agents in .NET and Python. The framework delivers a unified way to build agents, type-safe APIs, and graph-based workflow support, working with major providers like Microsoft Foundry, Azure OpenAI, OpenAI, GitHub Copilot, Anthropic Claude, AWS Bedrock, and Ollama. Features include orchestration packages, migration guides, and support for standards like MCP, A2A, and AG-UI. Upgrading from Semantic Kernel or AutoGen is described in dedicated resources, with community support for adoption.

This milestone completes the journey from initial previews to a unified platform for managing .NET and Python agents, building on momentum toward more standardized agent management.

- [Microsoft Agent Framework Release Candidate: Cross-Language AI Agent Orchestration for .NET and Python](https://devblogs.microsoft.com/foundry/microsoft-agent-framework-reaches-release-candidate/)
- [Migrating to Microsoft Agent Framework Release Candidate: A Guide for Semantic Kernel and AutoGen Users](https://devblogs.microsoft.com/semantic-kernel/migrate-your-semantic-kernel-and-autogen-projects-to-microsoft-agent-framework-release-candidate/)

### Microsoft Foundry: Model, SDK, and Platform Updates

Microsoft Foundry brings in updated AI models and SDKs, including GPT-5.2, GPT-5.1 Codex Max, Mistral Large 3, DeepSeek V3.2, Kimi-K2 Thinking, Cohere Rerank 4, and enhanced image and audio generators. Serverless fine-tuning, persistent agent memory, and improved agent-to-agent communication support stronger workflow integration and visualization. Managed Foundry MCP Server now offers secure endpoints for running models, with a unified portal, improved VS Code integration, and updated SDKs for easier project standardization. Developers are encouraged to migrate from the AzureML SDK v1 in advance of retirement, with resources available for migration and deprecation. These features improve workflow flexibility and multimodal project support.

This release builds on earlier efforts to integrate advanced models and align SDK experiences across different programming languages, making it easier to develop and migrate projects.

- [What’s New in Microsoft Foundry: Models, SDKs, and Platform Updates (Dec 2025 – Jan 2026)](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-dec-2025-jan-2026/)

### Building Practical Solutions with Microsoft Foundry Local AI

Foundry Local AI documentation showcases privacy-focused workflows, such as a smart building HVAC digital twin that simulates real-world HVAC systems using modular design and KPIs with 3D visualization. Features include debugging, Copilot integration, and ready-to-run deployments with BACnet, Modbus, and MQTT connections. Another guide reviews privacy-aware medical transcription with OpenAI Whisper and ASP.NET Core 10, covering secure deployment, API design, compliance, electronic health record (EHR) integration, and production roll out for healthcare.

These examples provide deeper, enterprise-ready blueprints for using privacy-first, local and modular AI in regulated environments such as healthcare and facilities management.

- [Building a Smart Building HVAC Digital Twin with AI Copilot Using Foundry Local](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-smart-building-hvac-digital-twin-with-ai-copilot/ba-p/4490784)
- [Building HIPAA-Compliant Medical Transcription with Microsoft Foundry Local AI](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-hipaa-compliant-medical-transcription-with-local-ai/ba-p/4490777)

### Microsoft Foundry: Frontier Model Integration and Reasoning Agents Challenges

Claude Sonnet 4.6 is now in Foundry, featuring Opus-class reasoning with a beta 1 million-token context window and adaptive controls. New browser automation, search, summary, and compliance tools are available for enterprise AI. The Agents League challenge invites teams to build reasoning agent solutions, with starter kits, online sessions, and prize competitions for orchestration and integration. MCP and SDK-driven environments enable both programmatic and visual control, providing flexibility to experiment with practical agent-based designs.

Claude Sonnet’s introduction and the Agents League demonstrate continued growth in model diversity and hands-on adoption through challenges.

- [Claude Sonnet 4.6 Now Available in Microsoft Foundry: Frontier AI Performance for Enterprise Scale](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/claude-sonnet-4-6-in-microsoft-foundry-frontier-performance-for-scale/4494873)
- [Agents League: Build Reasoning Agents with Microsoft Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/agents-league-join-the-reasoning-agents-track/ba-p/4494394)

### Agentic AI Workflows and Integration in Visual Studio Code

VS Code introduces more agent-oriented features, supporting orchestration, team collaboration, and background work assignment, as well as real-time security and workflow metrics for team environments. Events highlight agent coordination and monitoring with actionable telemetry. Tutorials cover everything from prompt-based prototyping and live workflow demos to building AI-powered projects with little to no code.

These improvements follow recent enhancements to SDK support and workflow monitoring, providing secure and versatile agent-driven development tools for all experience levels.

- [VS Code Live: Agent Sessions Day - Keynote](/2026-02-20-VS-Code-Live-Agent-Sessions-Day-Keynote.html)
- [How VS Code Builds with AI](/2026-02-20-How-VS-Code-Builds-with-AI.html)
- [VS Code Live: Agent Sessions Day – AI and Agentic Development in Visual Studio Code](/2026-02-17-VS-Code-Live-Agent-Sessions-Day-AI-and-Agentic-Development-in-Visual-Studio-Code.html)
- [Getting Started with Agents in VS Code](/2026-02-17-Getting-Started-with-Agents-in-VS-Code.html)

### Affordable AI Development and Practical Tutorials

The Budget Bytes series provides step-by-step guides to build working AI applications on Azure for less than $25, covering topics like inventory control and insurance. Each tutorial comes with sample code (GitHub repos), deployment steps, and Azure SQL trials so that teams can manage costs and debug issues easily when starting or prototyping with large AI projects.

With a focus on hands-on learning and transparent costs, these resources help new teams build practical AI solutions.

- [Budget Bytes: Building Real AI Apps on Azure for Under $25](https://devblogs.microsoft.com/azure-sql/introducing-budget-bytes/)

### Telecom Infrastructure, Agentic BSS, and Intelligent Edge

Telecom solution updates analyze AI’s role in enhancing mobile networks and business support systems. AI improves radio access networks with spectrum management and anomaly detection—powered by Azure and Foundry. Open RAN projects and the Janus initiative offer orchestration and support for global testing. Guides for agent-driven business support use Copilot Studio and MCP to automate telecom billing, quoting, and other tasks through APIs and rapid prototype workflows. Mobile World Congress sessions walk developers through live agent-building.

Industry adoption and workflow integration remain in focus as telecom projects highlight AI and agent tools for practical transformation.

- [Microsoft’s Vision: AI-RAN and Intelligent Edge Transforming Telecom Networks](https://techcommunity.microsoft.com/t5/telecommunications-industry-blog/ai-powered-ran-and-the-intelligent-edge-microsoft-s-vision-for/ba-p/4495554)
- [The Rise of Agentic BSS in the IQ Era: From Systems of Record to Systems of Outcome](https://techcommunity.microsoft.com/t5/telecommunications-industry-blog/the-rise-of-agentic-bss-in-the-iq-era-from-systems-of-record-to/ba-p/4495499)

### Developer Workflow and Software Practices in the Age of AI Coding

Martin Fowler’s session reinforces the importance of test-driven development (TDD) in a world where AI now writes both code and tests. TDD still provides discipline, reduces risk, and ensures code quality when agent outputs can be unpredictable. With AI’s help, security and review cycles increase in importance, and teams may need to shift from manual coding bottlenecks to stronger coordination. Fowler advises keeping Agile principles and adapting them to fit AI-driven and agent-rich workflows.

This practical guidance underscores how long-standing engineering methods still matter as AI changes processes.

- [Test-Driven Development Remains Essential in the Age of AI Coding](https://www.devclass.com/development/2026/02/21/should-there-be-a-new-manifesto-for-ai-development/4091612)

### Other AI News

SQUAD, an open-source orchestration framework for .NET, provides design patterns and building blocks for .NET and Azure teams to create AI agent teams for cloud and hybrid projects.

The trend toward agent frameworks for .NET continues, making orchestration and team-based build patterns more approachable.

- [.NET AI Community Standup: SQUAD – AI Agent Teams for C# Projects](/2026-02-17-NET-AI-Community-Standup-SQUAD-AI-Agent-Teams-for-C-Projects.html)

The Imagine Cup semifinalists showcase AI-based solutions for accessibility, healthcare, education, and inventory, built by students on Azure using cognitive services, generative models, and cloud integration.

These projects show how agent workflows and AI apps are spreading across sectors like health, education, and enterprise, as highlighted in last week’s news.

- [AI in Action: Meet the 2026 Imagine Cup Semifinalists](https://techcommunity.microsoft.com/blog/studentdeveloperblog/ai-in-action-meet-the-2026-imagine-cup-semifinalists/4495567)

A March 5 webinar on cost-efficient scaling of Azure AI agents gives advice on practice, model selection, financial optimization, and governance frameworks. It includes examples and Q&A to address common enterprise concerns.

The financial guidance thread continues, with a focus this week on controlling costs for larger deployments as agent adoption grows.

- [Upcoming Webinar: Maximize the Cost Efficiency of AI Agents on Azure](https://techcommunity.microsoft.com/t5/microsoft-developer-community/upcoming-webinar-maximize-the-cost-efficiency-of-ai-agents-on/ba-p/4493923)

Enterprise architecture analysis shows that while AI can automate some documentation and basic cost estimation, solution architects are still needed for strategic design and client communication. Developers are advised to keep upskilling as AI complements their roles—not replaces them.

The conversation about AI as an augmentation tool (rather than a replacement) continues from last week, supporting blended teams working with AI/agent frameworks.

- [Can the Solution Architect Role Be Replaced by AI?](https://dellenny.com/can-the-solution-architect-role-be-replaced-by-ai/)

## ML

Microsoft releases its preview ODBC driver for Fabric Data Engineering, making it easier to connect enterprise analytics platforms and Spark SQL in Microsoft Fabric. This driver simplifies query capabilities and integrates with analytics and lakehouse solutions.

As covered in last week’s update about Fabric’s analytics and ML tools, this ODBC driver enables smooth connections and ML pipeline integration for business intelligence teams. Users gain support for ODBC Spark SQL, flexible options, secure Entra ID authentication, and extended compatibility with large lakehouse requirements.

### Microsoft ODBC Driver for Fabric Data Engineering Preview

The new driver supports Spark SQL in Fabric Data Engineering, allowing .NET and Python integration for managing and querying data via OneLake. The release is ODBC 3.x compliant and works with Fabric’s security and configuration options, including credentials, tokens, certificates, and CLI authentication. Features such as session reuse, async prefetch, and proxy support enhance automation and real-time analytics

By connecting traditional tools and ML workflows, this driver supports lakehouse queries, remote ML scenarios, and analytics, and is shaped by feedback from early developer uses.

- [Introducing the Microsoft ODBC Driver for Fabric Data Engineering (Preview)](https://blog.fabric.microsoft.com/en-US/blog/microsoft-odbc-driver-for-microsoft-fabric-data-engineering-preview/)

## Azure

Azure introduces new updates in infrastructure, platform integration, migration planning, and enterprise feature support. New releases offer better resiliency, continuity in operations, high-performance AI training, improved container management, cost tracking, and developer experience. Migration support and platform modernization guide teams moving to more robust, secure, and scalable Azure solutions.

### Cloud Reliability, Resiliency, and Recoverability

A new hands-on Azure guide explains strategies to improve reliability and recoverability, using best practices from Cloud Adoption and Well-Architected Frameworks. Topics include instrumentation (Azure Monitor), chaos testing, governance, isolation, redundancy, scaling, and traffic management. The Azure Copilot Resiliency Agent supports environment checks and automated remediation. Recovery processes use Backup, Site Recovery, and scripted runbooks. Security tips and a 30-day resilience checklist help teams deploy resilient cloud systems.

This continues the operational reliability focus, agent-based management, and automation practices discussed previously.

- [Azure Reliability, Resiliency, and Recoverability: Building Cloud Continuity by Design](https://azure.microsoft.com/en-us/blog/azure-reliability-resiliency-and-recoverability-build-continuity-by-design/)

### Azure Kubernetes Service and Container Routing

The latest documentation guides organizations moving from self-hosted Nginx Ingress on AKS to the managed AKS App Routing add-on, which is supported through 2026. The approach keeps downtime low, manages resource and DNS changes, and highlights differences after migration (such as TLS and configuration). Azure supports transition with platform-specific enhancements, and teams are advised to prepare for Istio and Gateway API solutions in the future.

This step-by-step migration support aligns with networking and routing updates released for AKS last week.

- [Seamless Migration from Self-Hosted Nginx Ingress to Azure AKS App Routing Add-On](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/seamless-migrations-from-self-hosted-nginx-ingress-to-the-aks/ba-p/4495630)

### Enterprise Workflow Integration: Azure Logic Apps and SAP

Technical guides now cover connecting Azure Logic Apps with SAP. The content walks through scalable workflow designs, schema and error management, CSV and XML transformation, calling SAP RFCs, automating alerts, and handling remedial scenarios. AI validation features extend quality assurance, and BizTalk migration guidance supports building reliable hybrid systems, moving from legacy orchestrations to modern cloud-based and AI-checked patterns.

This follows earlier BizTalk migration guidance and highlights stepwise integration with SAP.

- [Logic Apps Agentic Workflows with SAP - End-to-End Integration Patterns](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-agentic-workflows-with-sap-part-1-infrastructure/ba-p/4491906)
- [Integrating Azure Logic Apps with SAP: Infrastructure and Workflow Contracts (Part 1)](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/agentic-logic-apps-integration-with-sap-part-1-infrastructure/ba-p/4491906)

### Azure AI Infrastructure and Performance

Azure has been validated as the first NVIDIA Exemplar Cloud for GB300 (Blackwell) and H100 systems. This status certifies high performance and reliability for training next-generation language models, including access to ND GPU clusters, InfiniBand networking, and optimized software, which can help teams scale to large model workloads with predictable results.

This validation supports recent improvements in Azure’s AI and analytics infrastructure, ensuring organizations can handle demanding and advanced workloads.

- [Azure Achieves NVIDIA Exemplar Cloud Status for Next-Generation AI Performance](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-recognized-as-an-nvidia-cloud-exemplar-setting-the-bar-for/ba-p/4495747)

### Site Reliability Engineering and DevOps Automation in Azure

The Azure SRE Agent now provides tools for investigating Log Analytics in Private Link-enabled environments with Azure Functions, VNet integration, managed identity, and Entra ID authentication. Guides on incident workflows include using the CLI, RBAC, and building custom monitors for SSL certificates. Documentation extends to risk evaluation, reporting, and Elasticsearch MCP server integration for secure automation and conversational log review.

This expands the SRE Agent’s feature set noted in past roundups, making secure cloud automation and monitoring more accessible.

- [How Azure SRE Agent Can Investigate Resources in a Private Network](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-azure-sre-agent-can-investigate-resources-in-a-private/ba-p/4494911)
- [Building a Custom SSL Certificate Monitor with Azure SRE Agent and Python](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-a-custom-ssl-certificate-monitor-with-azure-sre-agent-from/ba-p/4495832)
- [Get started with Elasticsearch MCP server in Azure SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-elasticsearch-mcp-server-in-azure-sre-agent/ba-p/4492896)

### Migration and Modernization: Cloud Workload Transitions

Migration guides cover AWS-to-Azure workload moves and strategies for transitioning large enterprise SharePoint Online environments. The AWS guide provides a phased migration checklist, with best practices and tools for validation. Enterprise SharePoint migration emphasizes clean up, compatibility, compliance, governance, and rollout. Azure Arc updates unify the migration process for SQL Server, with built-in assessment, provisioning, and Copilot-assisted guidance.

These guides reinforce last week’s migration and modernization topics, focusing on structured and low-risk transitions.

- [Migrating Workloads from AWS to Azure: A Structured Approach for Cloud Architects](https://techcommunity.microsoft.com/t5/azure-migration-and/migrating-workloads-from-aws-to-azure-a-structured-approach-for/ba-p/4495227)
- [Migrating to SharePoint Online: Lessons Learned from Large Enterprises](https://dellenny.com/migrating-to-sharepoint-online-lessons-learned-from-large-enterprises/)
- [SQL Server Migration Made Easy with Azure Arc](/2026-02-19-SQL-Server-Migration-Made-Easy-with-Azure-Arc.html)

### Azure Managed Disk and Storage Enhancements

Azure Migrate now offers recommendations for using Premium SSD v2, Ultra, and ZRS disks—giving customers options to improve performance, cost efficiency, and data recovery. Snapshot access now simplifies backup workflows. Container Registry gains private preview of geo-replicated regional endpoints for better control, failover, and reliability in distributed or Kubernetes environments.

These improvements add flexibility and support regional rollouts, following on from last week’s disk and storage innovations.

- [Azure Migrate Adds Support for Premium SSD v2, Ultra, and ZRS Disks](https://techcommunity.microsoft.com/t5/azure-storage-blog/azure-migrate-now-supporting-premium-ssd-v2-ultra-and-zrs-disks/ba-p/4495332)
- [Regional Endpoints for Geo-Replicated Azure Container Registries (Private Preview)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/regional-endpoints-for-geo-replicated-azure-container-registries/ba-p/4496186)

### Azure Virtual Desktop: Lifecycle Management and Connectivity

Azure Virtual Desktop receives guidance on session host retirement, including managing identity cleanup, automated decommissioning, and autoscale event handling. UDP RDP Shortpath over Private Link is now generally available, offering lower latency, higher performance, and simplified secure access for AVD sessions.

These updates enhance gov/cloud network policy management, as covered in recent networking guides.

- [Practical Framework for AVD Host Decommissioning Governance](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/improper-avd-host-decommissioning-a-practical-governance/m/p/4495437#M14006)
- [Enabling UDP RDP Shortpath Over Private Link for Azure Virtual Desktop](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-blog/rdp-shortpath-udp-over-private-link-is-now-generally-available/ba-p/4494644)

### Azure Virtual Machine Scale Sets and Networking

Automatic zone balancing for VMSS (public preview) distributes VMs evenly across zones, reducing manual balancing and increasing availability. NAT Gateway v2 introduces zone redundancy, IPv6 capabilities, enhanced performance, and improved log features for network diagnostics. Existing guides help teams plan costs and scale effectively.

Recent VM and networking advances are continued here, supporting reliable and cloud-native networking tools.

- [Public Preview: Automatic Zone Balance for Azure Virtual Machine Scale Sets](https://techcommunity.microsoft.com/t5/azure-compute-blog/public-preview-automatic-zone-balance-for-virtual-machine-scale/ba-p/4494476)
- [NAT Gateway v2 Overview](/2026-02-18-NAT-Gateway-v2-Overview.html)

### Azure API Management for Enterprise AI and Multi-Region Deployments

Uniper’s Unified AI Gateway using Azure API Management illustrates centralized governance over generative AI services, offering dynamic routing, authentication, and streamlined onboarding. Pairing API Management with Azure Front Door supports multi-region, active-active deployments, enabling global endpoints, failover, and policy enforcement.

These API and governance models expand on centralized and resilient patterns noted in last week’s cloud and AI integration updates.

- [Azure API Management Unified AI Gateway Design Pattern for Enterprise AI Governance](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/azure-api-management-unified-ai-gateway-design-pattern/ba-p/4495436)
- [Using Azure API Management with Azure Front Door for Multi-Region, Active-Active Architectures](https://techcommunity.microsoft.com/t5/microsoft-developer-community/using-azure-api-management-with-azure-front-door-for-global/ba-p/4492384)

### Microsoft Fabric Analytics, Security, and Identity

Fabric SQL Database use cases are highlighted for analytics, metadata, logging, OLTP, and modernization tasks. Snowflake key-pair authentication is now offered for passwordless access. Billing changes clarify costs for AI Functions and Services, and OneLake shortcuts support workspace/service principal identity, reducing reliance on user credentials.

These features strengthen integration, security, and governance released in recent Microsoft Fabric reports.

- [Fabric SQL Database Use Cases Within Analytics Solutions](/2026-02-19-Fabric-SQL-Database-Use-Cases-Within-Analytics-Solutions.html)
- [General Availability: Snowflake Key-Pair Authentication in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/snowflake-key-pair-authentication-generally-available/)
- [Billing Updates: Dedicated Operations for Fabric AI Functions and Services](https://blog.fabric.microsoft.com/en-US/blog/billing-updates-new-operations-for-fabric-ai-functions-and-ai-services/)
- [OneLake SharePoint and OneDrive Shortcuts Now Support Workspace and Service Principal Identities](https://blog.fabric.microsoft.com/en-US/blog/onelake-sharepoint-and-onedrive-shortcuts-now-support-workspace-and-service-principal-identities-generally-available/)

### Azure Network Troubleshooting and Sovereignty Solutions

A new case study investigates virtual network routing flows, clarifying how unexpected VNet-to-vWAN connections can form. A video guide explains how Azure and Azure Local meet sovereignty, compliance, data residency, and hybrid control requirements for regulated workloads.

These materials extend previous practical troubleshooting and compliance strategy coverage for Azure networks.

- [Azure VNet-to-vWAN Routing Mystery: How Does On-Premises Traffic Flow Without Direct Connection?](https://techcommunity.microsoft.com/t5/azure-networking/help-how-is-vnet-traffic-reaching-vwan-on-prem-when-the-vnet-isn/m/p/4495408#M767)
- [Meeting Sovereignty Requirements with Azure and Azure Local](/2026-02-16-Meeting-Sovereignty-Requirements-with-Azure-and-Azure-Local.html)

### Other Azure News

Developer tooling gains JMESPath query abilities in Azure CLI (azd v1.23.4 and above), making JSON filtering and transformation easier. Additional updates cover managed identity, API integration, zone-redundant storage for compliance and high-availability, plus guides for migration and troubleshooting.

As always, these updates reinforce commitment to developer experience and platform resilience.

- [JMESPath Query Support in Azure Developer CLI JSON Output](https://devblogs.microsoft.com/azure-sdk/azd-jmespath-query-support/)
- [Azure Update 20th February 2026](/2026-02-20-Azure-Update-20th-February-2026.html)
- [Azure Migrate Adds Support for Premium SSD v2, Ultra, and ZRS Disks](https://techcommunity.microsoft.com/t5/azure-storage-blog/azure-migrate-now-supporting-premium-ssd-v2-ultra-and-zrs-disks/ba-p/4495332)
- [General Availability: Snowflake Key-Pair Authentication in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/snowflake-key-pair-authentication-generally-available/)
- [Azure VNet-to-vWAN Routing Mystery: How Does On-Premises Traffic Flow Without Direct Connection?](https://techcommunity.microsoft.com/t5/azure-networking/help-how-is-vnet-traffic-reaching-vwan-on-prem-when-the-vnet-isn/m/p/4495408#M767)
- [Migrating Workloads from AWS to Azure: A Structured Approach for Cloud Architects](https://techcommunity.microsoft.com/t5/azure-migration-and/migrating-workloads-from-aws-to-azure-a-structured-approach-for/ba-p/4495227)
- [Migrating to SharePoint Online: Lessons Learned from Large Enterprises](https://dellenny.com/migrating-to-sharepoint-online-lessons-learned-from-large-enterprises/)

## Coding

Coding updates this week highlight new .NET runtime features, expanded agent workflows in VS Code, and community stories in language development and open source. These changes provide more options for reliable code, stronger automation, and better integration in developer tools.

### .NET Runtime and Instrumentation Advances

.NET 11 improves Async/Await performance for high-concurrency applications, reducing overhead and boosting scalability. Developers are given practical advice for using these updates in real-world code. A detailed guide explores System.Diagnostics.Metrics APIs in .NET, comparing standard and observable metric instruments, and suggests when to use push or pull reporting and OpenTelemetry integration.

These instrumentation improvements continue .NET’s movement toward greater observability and diagnostics, following last week’s release highlights.

- [Async Await Just Got A Massive Improvement in .NET](/2026-02-20-Async-Await-Just-Got-A-Massive-Improvement-in-NET.html)
- [Understanding Observable and Standard Instruments with System.Diagnostics.Metrics APIs](https://andrewlock.net/creating-standard-and-observable-instruments/)

### Visual Studio Code Workflow and Browser Integration

VS Code’s integrated browser brings live preview, real-time debugging, Chrome DevTools, and AI chat features directly into the editor. This update lowers the need for context switching and allows for smarter agent extension development. New browser workflows and agent-management features drive a tighter feedback loop and provide more productive coding experiences.

Live coding challenges experiment with agent workflows and share actionable discoveries for continuous improvement.

- [The Browser in Your Editor: Integrated Web Preview in VS Code](/2026-02-20-The-Browser-in-Your-Editor-Integrated-Web-Preview-in-VS-Code.html)
- [Live Coding Challenge: Exploring Agent Workflows in Visual Studio Code](/2026-02-20-Live-Coding-Challenge-Exploring-Agent-Workflows-in-Visual-Studio-Code.html)

### Open Source Ecosystems and Language Histories

A pair of interviews with Anders Hejlsberg discuss why making TypeScript open source drove growth, trust, and quality—along with the impact of migrating to GitHub for increased openness and transparent, sustainable development. These discussions provide perspective on why open-source processes benefit technical communities, as covered in earlier roundups.

- [Why TypeScript Had to Be Open Source](/2026-02-19-Why-TypeScript-Had-to-Be-Open-Source.html)
- [Why TypeScript Moved to GitHub in 2014](/2026-02-16-Why-TypeScript-Moved-to-GitHub-in-2014.html)

### Windows MIDI Services: Next-Gen Music Tech on Windows 11

Windows 11 introduces updated MIDI Services, adding support for MIDI 1.0/2.0, high-resolution data, legacy compatibility, and multi-client MIDI ports. New features include metadata editing, loopback endpoints, communication between apps, better timing, updated drivers, and an open SDK. Future plans include USB audio class, BLE MIDI, and expanded routing features.

- [Windows 11 Introduces Advanced MIDI 2.0 Support for Musicians and Developers](https://blogs.windows.com/windowsexperience/2026/02/17/making-music-with-midi-just-got-a-real-boost-in-windows-11/)

### PowerShell, OpenSSH, and DSC: Long-Term Roadmap

For 2026, the PowerShell, OpenSSH, and DSC roadmap details improvements to PowerShell 7.7—covering script path flexibility, Bash-style aliases, AI-powered scripting assistance, predictive IntelliSense in PSReadLine, and module gallery migration. OpenSSH gains better authentication methods and DSC v3.2 includes Linux/Python adapters. Regular updates and open collaboration continue for module development and automation on both Windows and Linux.

AI-driven scripting connects to wider agent-based process advancements in the industry.

- [PowerShell, OpenSSH, and DSC: Planned Team Investments for 2026](https://devblogs.microsoft.com/powershell/powershell-openssh-and-dsc-team-investments-for-2026/)

### Inside Model Context Protocol: Workflow and Open Source Journey

An interview with David Soria Parra explores the evolution of Model Context Protocol, its challenges, the journey to Linux Foundation, the Python/Azure tech stack, the "Skills" approach, and best practices for open-source collaboration and protocol leadership.

The discussion sheds light on the journey from internal protocol to open source, reflecting the process lessons from earlier MCP updates.

- [Inside MCP: Origin, Workflow, and Future with David Soria Parra](/2026-02-19-Inside-MCP-Origin-Workflow-and-Future-with-David-Soria-Parra.html)

## DevOps

DevOps updates this week include new automation and governance features for versioning, pipelines, and team management. GitHub, Azure, and Microsoft Fabric all deliver enhancements for customization and compliance in global developer workflows.

### GitHub Platform and Workflow Enhancements

GitHub Desktop 3.5.5 adds standard Git hook support for Windows and Unix, with management tools for commit automation and real-time feedback on errors. Commits can reference Copilot as an author for better workflow visibility.

APIs and workflow triggers bring new details, such as run IDs for monitoring. Project management tools receive updates for search imports and hierarchical views, while a redesigned comments panel simplifies pull request reviews. Test merge commits now use resources more efficiently in CI, and improved reviewer rules reinforce repository compliance.

These updates reinforce GitHub’s direction from last week—adding automation, control, and enhanced workflow structure.

- [GitHub Desktop 3.5.5 Adds Native Git Hooks Support](https://github.blog/changelog/2026-02-18-github-desktop-3-5-5-adds-git-hooks-support)
- [API Access to GitHub Billing Usage Reports in Public Preview](https://github.blog/changelog/2026-02-17-api-access-to-billing-usage-reports-in-public-preview)
- [GitHub Actions Workflow Dispatch API Now Returns Run IDs and Details](https://github.blog/changelog/2026-02-19-workflow-dispatch-api-now-returns-run-ids)
- [Enhancements to GitHub Projects: Search Import and Hierarchy View Updates](https://github.blog/changelog/2026-02-19-github-projects-import-items-based-on-a-query-and-hierarchy-view-improvements)
- [Improved Pull Request Commenting Experience on GitHub's Files Changed Page](https://github.blog/changelog/2026-02-19-access-all-pull-request-comments-without-leaving-the-new-files-changed-page)
- [Changes to Test Merge Commit Generation for GitHub Pull Requests](https://github.blog/changelog/2026-02-19-changes-to-test-merge-commit-generation-for-pull-requests)
- [Custom Properties and Rule Insights Improvements in GitHub Organizations](https://github.blog/changelog/2026-02-17-custom-properties-and-rule-insights-improvements)
- [Required Reviewer Rule for Repository Rulesets Now Generally Available](https://github.blog/changelog/2026-02-17-required-reviewer-rule-is-now-generally-available)

### Azure DevOps and SRE Automation

Azure DevOps Boards now offer condensed Kanban and Sprint views for improved clarity and focused automation. Azure SRE Agent receives new documentation for managing incidents, and TFVC announces the deprecation of older check-in policies, signaling a move toward modern version control and governance. These updates align with recent changes supporting permissions and automated workflows.

- [Introducing Condensed Views for Kanban and Sprint Boards in Azure DevOps](https://devblogs.microsoft.com/devops/condensed-views-on-kanban-and-sprint-boards/)
- [Reactive Incident Response with Azure SRE Agent: From Alert to Resolution in Minutes](https://techcommunity.microsoft.com/t5/azure-architecture-blog/reactive-incident-response-with-azure-sre-agent-from-alert-to/ba-p/4492938)
- [TFVC Remove Existing Obsolete Policies ASAP](https://devblogs.microsoft.com/devops/tfvc-remove-existing-obsolete-policies-asap/)

### Microsoft Fabric CI/CD Automation

Microsoft fabric-cicd now has official support, moving the tool from the community into Microsoft’s main DevOps roadmap. Development teams have a maintained, first-party CI/CD automation option for managing artifacts and dependencies.

This aligns with a broader trend toward robust automation and best practices in Microsoft Fabric environments.

- [Announcing Official Support for Microsoft fabric-cicd Tool](https://blog.fabric.microsoft.com/en-US/blog/announcing-official-support-for-microsoft-fabric-cicd-tool/)

### Open Source Ecosystem Developments

Open source community growth and best practices for scaling project onboarding and automation are summarized in the Octoverse 2026 report findings. With more contributors and increased automation, sustainable project management is vital.

These findings reinforce the observations shared last week on healthy open source ecosystem evolution.

- [Open Source Trends for 2026: Insights from GitHub’s Octoverse Report](https://github.blog/open-source/maintainers/what-to-expect-for-open-source-in-2026/)

## Security

Security updates focus on better credential and secret management, updated supply chain health, runtime agent isolation, digital content verification, and process improvement for proactive risk management. New tools and practices help developers and organizations safeguard workflows against new threats.

### GitHub Enterprise Credential Management and Secret Scanning

GitHub Enterprise Cloud introduces credential management for instant incident response, letting organization admins and trusted operators review and revoke credentials with complete audit logging. Secret scanning now examines additional metadata for better alert quality and faster response.

These features add security automation and oversight, continuing themes from recent updates to identity and auditing.

- [Enterprise-Wide Credential Management Tools for GitHub Incident Response](https://github.blog/changelog/2026-02-17-enterprise-wide-credential-management-tools-for-incident-response)
- [Secret Scanning Improvements: Extended Metadata Checks on GitHub](https://github.blog/changelog/2026-02-18-secret-scanning-improvements-to-extended-metadata-checks)

### Supply Chain Security: npm CLI and Open Source AI Libraries

npm CLI v11.10.0 and above brings bulk OIDC trusted publish and improved script security. The new "npm trust" command streamlines configuration, and the "--allow-git" flag locks down git dependencies in npm install. GitHub’s Secure Open Source Fund supports 67 AI-related packages, adding automated scanning and enforcement for better supply chain protection.

These features continue recent efforts to reduce package management risk across Node.js and AI projects.

- [npm Bulk Trusted Publishing and Script Security Features Released](https://github.blog/changelog/2026-02-18-npm-bulk-trusted-publishing-config-and-script-security-now-generally-available)
- [Securing the AI Software Supply Chain: Security Results Across 67 Open Source Projects](https://github.blog/open-source/maintainers/securing-the-ai-software-supply-chain-security-results-across-67-open-source-projects/)

### Self-Hosted Agent Runtimes: OpenClaw Identity, Isolation, and Monitoring

Microsoft shares best practices for self-hosting OpenClaw agents, recommending isolation, least-privilege credentials, rigorous monitoring, regular rebuilding, and established incident response policies. Defender XDR, Entra ID, Sentinel, and Purview integration are suggested for minimizing risk. Developers are advised to follow published deployment guidance for safety.

Last week’s identity and role management updates continue here in the context of runtime agent controls and supply chain defense.

- [Securing OpenClaw Self-hosted Agents: Identity, Isolation, and Runtime Risk](https://www.microsoft.com/en-us/security/blog/2026/02/19/running-openclaw-safely-identity-isolation-runtime-risk/)

### Digital Media Authentication: AI, Provenance, and Emerging Standards

Microsoft’s analysis of AI in digital media highlights the importance of provenance, watermarking, and fingerprinting—considering the role of the C2PA standard and encouraging multi-layer safeguards to build trust in online content.

The approach to media authentication blends technical steps and governance, continuing conversations on trust and verification in last week’s coverage.

- [How AI Is Changing Media Trust and Authentication Online](https://news.microsoft.com/signal/articles/a-new-study-explores-how-ai-shapes-what-you-can-trust-online/)

### Other Security News

A maturity model guide helps organizations conduct proactive security exposure management with Microsoft’s available tools, supporting the five levels of defense with actionable recommendations for visibility, integration, and risk alignment. SIEM and XDR use is encouraged for continuous improvement.

This guidance builds on recent organizational security advice about incident response, monitoring, and analytics.

- [Establishing a Proactive Defense with Microsoft Security Exposure Management: New Maturity Model Guide](https://www.microsoft.com/en-us/security/blog/2026/02/19/new-e-book-establishing-a-proactive-defense-with-microsoft-security-exposure-management/)
