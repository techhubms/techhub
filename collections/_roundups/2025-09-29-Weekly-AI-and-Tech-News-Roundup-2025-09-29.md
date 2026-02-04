---
title: 'GitHub Copilot, Azure AI, and DevOps: Updates on Agentic Automation and Cloud Workflows'
author: TechHub
date: 2025-09-29 09:00:00 +00:00
tags:
- .NET
- Agentic Automation
- AI Agents
- Azure AI
- Claude Opus
- Cloud Modernization
- Containerization
- Hybrid Cloud
- IaC
- MCP
- Microsoft Fabric
- MLOps
- OpenAI
- Supply Chain Security
- VS
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
external_url: /all/roundups/Weekly-AI-and-Tech-News-Roundup-2025-09-29
---
Welcome to this week's tech roundup, where advancements in AI automation and modernization continue to impact cloud, developer, and enterprise ecosystems. GitHub Copilot continues its shift from a code completion solution to an agentic workflow platform, anchored on the Model Context Protocol (MCP) for open, reusable automation across IDE and CLI environments. The release of updated models, including GPT-5-Codex and Claude Opus 4.1, expands developer flexibility, while Copilot adds general availability for coding agents, CLI modernization, and improved support for SQL and enterprise applications—driving more collaborative and extensible development practices.

At the same time, Microsoft's Azure ecosystem introduces more unified AI orchestration via Azure AI Foundry and Studio, reliable local inference using Windows ML, and new updates for security, compliance, and observability throughout its stack. Microsoft Fabric supports more data mirroring, embedded analytics, and low-code/no-code agent integration, providing tools for businesses focused on modern data solutions. In ML, DevOps, and security, increased standardization, automation tooling, and operational best practices support innovation while helping teams maintain trust and resilience against modern risks. Continue reading for a practical look at the changes driving progress in the cloud-native and AI-powered technology landscape.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Model Context Protocol (MCP): Standardization and Ecosystem Transition](#model-context-protocol-mcp-standardization-and-ecosystem-transition)
  - [Copilot Coding Agent: From Workflow Automation to IDE and CLI Integration](#copilot-coding-agent-from-workflow-automation-to-ide-and-cli-integration)
  - [AI Model Options: Next-Generation Model Rollouts, Integrations, and Deprecation](#ai-model-options-next-generation-model-rollouts-integrations-and-deprecation)
  - [Copilot CLI and Extension Deprecations: Streamlining for the Future](#copilot-cli-and-extension-deprecations-streamlining-for-the-future)
  - [Copilot Spaces, Embedding Model Updates, and Open Ecosystem Enhancements](#copilot-spaces-embedding-model-updates-and-open-ecosystem-enhancements)
  - [Copilot-Powered Modernization: Java, .NET, and SQL Workflows](#copilot-powered-modernization-java-net-and-sql-workflows)
  - [Agentic Workflows, Prompt-Driven Development, and IDE Innovations](#agentic-workflows-prompt-driven-development-and-ide-innovations)
- [AI](#ai)
  - [Azure AI Foundry and Studio: Unified Generative AI Platform](#azure-ai-foundry-and-studio-unified-generative-ai-platform)
  - [Secure and Reliable AI Agent Development with Azure and MCP](#secure-and-reliable-ai-agent-development-with-azure-and-mcp)
  - [Model Context Protocol (MCP) and Registry: Best Practices and Interoperability](#model-context-protocol-mcp-and-registry-best-practices-and-interoperability)
  - [Microsoft Copilot Studio and Model Selection](#microsoft-copilot-studio-and-model-selection)
  - [Microsoft Fabric: LLM Analytics, Real-Time AI, and Workflow Automation](#microsoft-fabric-llm-analytics-real-time-ai-and-workflow-automation)
  - [.NET and Multimodal AI: Text-to-Image Capabilities](#net-and-multimodal-ai-text-to-image-capabilities)
  - [SharePoint and Microsoft 365: AI-Driven Content Intelligence](#sharepoint-and-microsoft-365-ai-driven-content-intelligence)
  - [Building and Operationalizing AI-Powered Agents](#building-and-operationalizing-ai-powered-agents)
  - [AI for Social Impact and Enterprise Architecture](#ai-for-social-impact-and-enterprise-architecture)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Microsoft Fabric Spark Observability and Integration](#microsoft-fabric-spark-observability-and-integration)
  - [Evolving MLOps Architectures and Operational Practices](#evolving-mlops-architectures-and-operational-practices)
- [Azure](#azure)
  - [Azure Landing Zones and Multi-Region Architecture](#azure-landing-zones-and-multi-region-architecture)
  - [Infrastructure as Code and Automation Advances](#infrastructure-as-code-and-automation-advances)
  - [Agentic AI-Powered Modernization and Migration](#agentic-ai-powered-modernization-and-migration)
  - [Azure Arc Gateway, Arc for Azure Local, and Hybrid Cloud Connectivity](#azure-arc-gateway-arc-for-azure-local-and-hybrid-cloud-connectivity)
  - [Microsoft Fabric: Data Integration, Orchestration, and Developer Tooling](#microsoft-fabric-data-integration-orchestration-and-developer-tooling)
  - [Azure Platform Updates, Observability, and Security](#azure-platform-updates-observability-and-security)
  - [Azure Maps Geocode Autocomplete API](#azure-maps-geocode-autocomplete-api)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [Visual Studio 2026 and Container Tooling](#visual-studio-2026-and-container-tooling)
  - [.NET Aspire 9.5 and Modern .NET Cloud-Native Development](#net-aspire-95-and-modern-net-cloud-native-development)
  - [.NET MAUI: App Compliance, Migration, and Community Engagement](#net-maui-app-compliance-migration-and-community-engagement)
  - [.NET Platform Strategy and Database Migrations](#net-platform-strategy-and-database-migrations)
  - [Building Server-Side and CLI Tools with .NET](#building-server-side-and-cli-tools-with-net)
- [DevOps](#devops)
  - [GitHub Platform and API Lifecycle Updates](#github-platform-and-api-lifecycle-updates)
  - [AI and Automation in DevOps: Harness and HashiCorp Advances](#ai-and-automation-in-devops-harness-and-hashicorp-advances)
  - [Testing and Developer Onboarding Tools](#testing-and-developer-onboarding-tools)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Package Registry and Supply Chain Security](#package-registry-and-supply-chain-security)
  - [Code Scanning, Static Analysis, and Remediation Workflows](#code-scanning-static-analysis-and-remediation-workflows)
  - [Artifact Signing, Infrastructure, and Cloud Security](#artifact-signing-infrastructure-and-cloud-security)
  - [Threat Intelligence, Malware, and Incident Response](#threat-intelligence-malware-and-incident-response)
  - [Identity, Data Protection, and Developer Security Skills](#identity-data-protection-and-developer-security-skills)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot continues its transition from basic code completion toward agent-powered automation, cloud modernization, and context-driven software development. Developers now have more choice with new models, such as GPT-5-Codex and Claude Opus 4.1, alongside further Copilot improvements and extended IDE support. The platform is aligning with open standards focused on the Model Context Protocol (MCP) to improve automation, compatibility, and extensibility. These upgrades include updates for command-line tools, VS Code, enterprise tracking, SQL development, and cloud migration workflows for Java and .NET. As legacy models and extensions get phased out, developers are encouraged to move to the latest approaches and integrate Copilot more deeply into team-oriented, automated workflows.

### Model Context Protocol (MCP): Standardization and Ecosystem Transition

GitHub will remove support for Copilot Extensions as GitHub Apps by November 2025 and recommends full migration to MCP servers. This transition takes the ecosystem from initial registry features to a protocol-focused structure, allowing developers to reuse MCP integrations more easily across agents for increased interoperability. Recent registry changes reinforce this approach. The latest MCP IDE guides encourage developers to review migration documentation and shift to MCP standards—for scalable, maintainable workflows and future agentic development. The MCP registry is now positioned as a foundation for Copilot's ongoing development.

- [Deprecation of GitHub Copilot Extensions in Favor of Model Context Protocol (MCP) Servers](https://github.blog/changelog/2025-09-24-deprecate-github-copilot-extensions-github-apps)
- [What is Model Context Protocol (MCP)?](/ai/videos/what-is-model-context-protocol-mcp)
- [Understanding Model Context Protocol (MCP) for Developers](/ai/videos/understanding-model-context-protocol-mcp-for-developers)
- [How to Use GitHub Copilot Agent Mode and MCP to Query Microsoft Learn Docs in VS Code](https://techcommunity.microsoft.com/t5/microsoft-developer-community/use-copilot-and-mcp-to-query-microsoft-learn-docs/ba/p/4455835)
- [Use Copilot and MCP to Query Microsoft Learn Docs](/ai/videos/use-copilot-and-mcp-to-query-microsoft-learn-docs)
- [GitHub MCP Registry Integration with Playwright in VS Code Insiders](/ai/videos/github-mcp-registry-integration-with-playwright-in-vs-code-insiders)

### Copilot Coding Agent: From Workflow Automation to IDE and CLI Integration

Copilot Coding Agent is now available to all paid users, progressing from workflow previews to broad automation across GitHub, IDEs, and direct CLI/Mobile app usage. GitHub Actions continue to orchestrate agent tasks, with new CLI and mobile features providing more flexibility for developers. Recent added controls for issue assignment and repository selection give teams enhanced management over agent-driven tasks—making cross-platform delegation simpler for both individuals and groups.

- [GitHub Copilot Coding Agent Now Generally Available](https://github.blog/changelog/2025-09-25-copilot-coding-agent-is-now-generally-available)
- [Kick off and Track Copilot Coding Agent Sessions from the GitHub CLI](https://github.blog/changelog/2025-09-25-kick-off-and-track-copilot-coding-agent-sessions-from-the-github-cli)
- [Start and Track GitHub Copilot Coding Agent Tasks in GitHub Mobile](https://github.blog/changelog/2025-09-24-start-and-track-copilot-coding-agent-tasks-in-github-mobile)
- [Enhanced Copilot Issue Assignment: Pick Repository and Base Branch](https://github.blog/changelog/2025-09-23-pick-the-repository-and-base-branch-when-assigning-issues-to-copilot)
- [Copilot Can Create GitHub Issues with Code Snippets: Public Preview](https://github.blog/changelog/2025-09-25-copilot-can-create-issues-with-code-snippets-in-public-preview)

### AI Model Options: Next-Generation Model Rollouts, Integrations, and Deprecation

OpenAI's GPT-5-Codex and Claude Opus 4.1 are now available to additional Copilot subscribers and IDE users, broadening model selection following last week's previews. The Copilot-SWE model launches for focused software engineering in VS Code Insiders, supporting ongoing context-driven workflows. Admin controls and Pro+ plan updates deliver more robust model management. GitHub continues to phase out older models, with organizations guided through updates using actionable adoption tutorials.

- [OpenAI GPT-5-Codex Now Available in GitHub Copilot Public Preview](https://github.blog/changelog/2025-09-23-openai-gpt-5-codex-is-rolling-out-in-public-preview-for-github-copilot)
- [Claude Opus 4.1 Now Generally Available in GitHub Copilot](https://github.blog/changelog/2025-09-23-claude-opus-4-1-is-now-generally-available-in-github-copilot)
- [GitHub Copilot-SWE Model Launches in Visual Studio Code Insiders](https://github.blog/changelog/2025-09-22-copilot-swe-model-rolling-out-to-visual-studio-code-insiders)
- [Upcoming Deprecation of Select Copilot Models from Claude, OpenAI, and Gemini](https://github.blog/changelog/2025-09-23-upcoming-deprecation-of-select-copilot-models-from-claude-openai-and-gemini)
- [VS Code and GitHub Copilot: Exploring GPT-5-Codex and Copilot-SWE](/ai/videos/vs-code-and-github-copilot-exploring-gpt-5-codex-and-copilot-swe)
- [What's New: GitHub MCP Registry, Copilot CLI Public Preview, and Beast Mode for Copilot](/ai/videos/whats-new-github-mcp-registry-copilot-cli-public-preview-and-beast-mode-for-copilot)

### Copilot CLI and Extension Deprecations: Streamlining for the Future

GitHub is discontinuing the `gh-copilot` CLI extension, moving to the npm-distributed Copilot CLI (now in public preview). This is part of the ongoing shift away from historic Marketplace extensions and supports recent MCP registry changes. The improved CLI simplifies agentic code generation, code review, and MCP-based extensibility, reinforcing the platform's commitment to standardization and modern developer tooling. Admins should review timelines and update workflows before October 2025 as MCP adoption grows.

- [GitHub Copilot CLI Extension Deprecation Announcement](https://github.blog/changelog/2025-09-25-upcoming-deprecation-of-gh-copilot-cli-extension)
- [GitHub Copilot CLI Now Available in Public Preview](https://github.blog/changelog/2025-09-25-github-copilot-cli-is-now-in-public-preview)

### Copilot Spaces, Embedding Model Updates, and Open Ecosystem Enhancements

Copilot Spaces is now available to all users, providing a platform for managing files, documentation, and project context—building on recent embedding and workflow advancements. A new embedding model for code search in VS Code enhances daily code retrieval and multi-language support. The Hugging Face VS Code extension now allows Copilot Chat to interact with open-source models, enabling experimental and domain-specific development and broadening pay-as-you-go model choices for teams seeking customized workflows and improved context support.

- [Copilot Spaces: General Availability Announcement](https://github.blog/changelog/2025-09-24-copilot-spaces-is-now-generally-available)
- [GitHub Copilot’s New Embedding Model Improves Code Search in VS Code](https://github.blog/news-insights/product-news/copilot-new-embedding-model-vs-code/)
- [Hugging Face Opens GitHub Copilot Chat to Open-Source Models](https://devops.com/hugging-face-opens-github-copilot-chat-to-open-source-models/)

### Copilot-Powered Modernization: Java, .NET, and SQL Workflows

The Copilot App Modernization toolkit is now generally available for Java and .NET projects, expanding on last week's agent-guided refactoring tutorials. The toolkit automates dependency analysis, code transformation, containerization, and incorporates security scanning—supporting recent compliance initiatives. Updated SQL workflows leverage AI for smarter query generation, analytics, and automation for both MSSQL and PostgreSQL, continuing the trend from last week's context-driven database improvements. Tutorials help teams upgrade legacy systems to modern infrastructure while prioritizing productivity and maintainability.

- [GitHub Copilot App Modernization Now Available for Java and .NET Projects](https://github.blog/changelog/2025-09-22-github-copilot-app-modernization-is-now-generally-available-for-java-and-net)
- [Modernize .NET Apps in Days with GitHub Copilot](/ai/videos/modernize-net-apps-in-days-with-github-copilot)
- [Modernizing Java Applications with GitHub Copilot and Azure Deployment](/ai/videos/modernizing-java-applications-with-github-copilot-and-azure-deployment)
- [Modernizing Java Projects with GitHub Copilot Agent Mode: Step-by-Step Guide](https://github.blog/ai-and-ml/github-copilot/a-step-by-step-guide-to-modernizing-java-projects-with-github-copilot-agent-mode/)
- [Quickly Modernize and Deploy Java Apps with AI and GitHub Copilot in VS Code](/ai/videos/quickly-modernize-and-deploy-java-apps-with-ai-and-github-copilot-in-vs-code)
- [Modernize Java Apps in Days with GitHub Copilot](/ai/videos/modernize-java-apps-in-days-with-github-copilot)
- [Microsoft’s AI Agents Target Technical Debt Crisis](https://devops.com/microsofts-ai-agents-target-technical-debt-crisis/)
- [Enhancing SQL Development in VS Code with GitHub Copilot and Microsoft Fabric](/ai/videos/enhancing-sql-development-in-vs-code-with-github-copilot-and-microsoft-fabric)
- [Boost Productivity with the PostgreSQL Extension and GitHub Copilot in VS Code](https://techcommunity.microsoft.com/t5/microsoft-developer-community/talk-to-your-data-postgresql-gets-a-voice-in-vs-code/ba/p/4453695)

### Agentic Workflows, Prompt-Driven Development, and IDE Innovations

New guidance covers converting web apps to mobile apps using Copilot prompts, leveraging plan mode, voice input, and improved model management—continuing previous advances in MCP-powered IDE workflows and Spec Kit sessions. The VS Code Insiders podcast features the latest on IDE improvements, focusing on AI’s shift from pure code assistance to orchestrated workflow support. Ongoing updates in live preview, documentation access, and activity tracking build on former releases in XAML, collaborative coding, and agentic automation—marking positive change for developer workflows.

- [Converting a Web App to Mobile Using GitHub Copilot Prompts](/ai/videos/converting-a-web-app-to-mobile-using-github-copilot-prompts)
- [The Future of Coding Agents in VS Code: Insights from VS Code Insiders Podcast](/ai/videos/the-future-of-coding-agents-in-vs-code-insights-from-vs-code-insiders-podcast)
- [Enhancements to XAML Live Preview in Visual Studio for .NET MAUI](https://devblogs.microsoft.com/visualstudio/enhancements-to-xaml-live-preview-in-visual-studio-for-net-maui/)
- [GitHub Copilot: The Influence of Generative AI Assistants and Agents on Software Development - Netherlands](/github-copilot-the-influence-of-generative-ai-assistants-and-agents-on-software-development-netherlands)
- [GitHub Copilot: The Influence of Generative AI Assistants and Agents on Software Development - Belgium](/github-copilot-the-influence-of-generative-ai-assistants-and-agents-on-software-development-belgium)

## AI

AI technologies on Microsoft platforms continued to grow in hardware compatibility, agent reliability, model choice, and practical deployment, following the themes established in recent weeks. Guides and releases remain focused on bringing updated AI solutions into daily workflows, supporting best practices across both cloud and edge environments.

### Azure AI Foundry and Studio: Unified Generative AI Platform

Azure AI Studio (now Azure AI Foundry) establishes itself as a central workspace for developing generative AI and deploying LLM solutions, spanning model options including OpenAI, Meta, Mistral, and more. The platform supports prompt engineering, fine-tuning, retrieval-augmented generation (RAG), and offers both code-first and low-code interfaces. GPT-4o adds voice and multimodal features, and Phi-3 offers options for lightweight inference.

Security and governance improvements allow organizations to adopt responsible AI usage with integrated monitoring and compliance. Developers should remain aware of billing and vendor lock-in as they work with the platform.

Foundry Local v0.7 brings support for Intel/AMD NPUs on Windows 11 and simplifies local inference and AI runtime management. Installation with winget (Windows) and brew (Mac) reduces setup friction for multi-platform development.

Windows ML is now generally available, providing ONNX-based local inference for privacy and edge execution in Windows applications. Integrated with AMD Ryzen AI, Intel OpenVINO, NVIDIA TensorRT, and Snapdragon NPUs, Windows ML works closely with the App SDK and includes streamlined model conversion via the AI Toolkit for VS Code—highlighting edge AI’s readiness for production scenarios.

- [Azure AI Studio and AI Foundry: Microsoft’s Generative AI Platform Explained](https://dellenny.com/azure-ai-studio-azure-ai-foundry-a-powerful-platform-for-generative-ai/)
- [Foundry Local Meets More Silicon: Expanded AI Runtime and NPU Support](https://devblogs.microsoft.com/foundry/foundry-local-meets-more-silicon/)
- [Windows ML Now Generally Available: Empowering Developers to Deploy Local AI on Windows Devices](https://blogs.windows.com/windowsdeveloper/2025/09/23/windows-ml-is-generally-available-empowering-developers-to-scale-local-ai-across-windows-devices/)

### Secure and Reliable AI Agent Development with Azure and MCP

AI agent development is improving with integration methods for durable, reliable operations—building on previous agent orchestration and security content. Developers now have a step-by-step guide for using the OpenAI Agent SDK with Azure Durable Functions to support persistent state, retry logic, and distributed workflows, using decorators and orchestration functions to manage errors efficiently and reduce manual coding.

The final Agent Factory installment explains how to build a secure, standards-based agent ecosystem using the agentic web stack—covering identity, trust, and compliance via Entra ID alongside open protocols. Practical tips on integrating standards and secure orchestration are included, addressing both Microsoft and open-source tools.

- [Enhancing AI Agent Reliability with OpenAI Agent SDK and Azure Durable Functions](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/openai-agent-sdk-integration-with-azure-durable-functions/ba/p/4453402)
- [Agent Factory: Designing the Open Agentic Web Stack](https://azure.microsoft.com/en-us/blog/agent-factory-designing-the-open-agentic-web-stack/)

### Model Context Protocol (MCP) and Registry: Best Practices and Interoperability

Azure's Model Context Protocol further embeds governance and security in AI workflows. Technical analysis highlights how MCP best practices in GitHub Copilot and VS Code enable automatic compliance and security enforcement, particularly for infrastructure-as-code scripts. Dynamic prompt instructions help teams maintain up-to-date policy compliance.

A video walkthrough introduces the GitHub MCP Registry, allowing developers to locate and connect MCP servers for agent development and modular design. Additional guidance outlines secure MCP server integration for Logic Apps and Copilot Studio, including authentication and deployment recommendations.

- [Teaching the LLM Good Habits: How Azure MCP Uses Best-Practice Tools](https://devblogs.microsoft.com/all-things-azure/teaching-the-llm-good-habits-how-azure-mcp-uses-best-practice-tools/)
- [A Deep Dive into the GitHub MCP Registry for AI Agents](/ai/videos/a-deep-dive-into-the-github-mcp-registry-for-ai-agents)
- [Connecting Azure Logic Apps MCP Server to Copilot Studio Securely](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/calling-logic-apps-mcp-server-from-copilot-studio/ba/p/4456277)

### Microsoft Copilot Studio and Model Selection

Copilot Studio adds model selection for Anthropic's Claude Sonnet 4 and Opus 4.1, alongside OpenAI's GPT models, enabling prompt- and logic-level model configuration. Admin options in Microsoft 365 and Power Platform allow for domain-specific assignments and automated fallback rules—providing more control for organizations pursuing consistent automation.

- [Anthropic Models Integrated with OpenAI in Microsoft Copilot Studio](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/anthropic-joins-the-multi-model-lineup-in-microsoft-copilot-studio/)

### Microsoft Fabric: LLM Analytics, Real-Time AI, and Workflow Automation

Fabric Data Agent now supports mirrored cloud databases, allowing natural language queries and multimodal analytics using Delta Parquet mirrors. Previewed anomaly detection in RTI streamlines streaming analytics with integration into Teams and email alerts. Agent Mart Studio’s expanded connections with Fabric and OneLake enhance low- and no-code workflow automation for data professionals and developers.

- [Unlocking LLM-Powered Analytics with Fabric Data Agent and Mirrored Databases](https://blog.fabric.microsoft.com/en-US/blog/unlocking-llm-powered-through-data-agent-from-your-mirrored-databases-in-microsoft-fabric/)
- [AI–Powered Real-Time Intelligence with Anomaly Detection in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/ai-powered-real-time-intelligence-with-anomaly-detection-preview/)
- [Building AI Agents for Enterprise Data with Agent Mart Studio and Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/27082/)

### .NET and Multimodal AI: Text-to-Image Capabilities

MEAI adds text-to-image generation in .NET, providing a consistent API that abstracts providers like Azure AI Foundry, OpenAI, and ONNX. This update prepares for future image-to-image and image-to-video support, making multimodal AI more accessible for .NET applications.

- [Exploring Text-to-Image Capabilities in .NET with Microsoft Extensions for AI](https://devblogs.microsoft.com/dotnet/explore-text-to-image-dotnet/)

### SharePoint and Microsoft 365: AI-Driven Content Intelligence

SharePoint's Knowledge Agent (public preview) delivers AI-powered automation for content metadata, summaries, document comparison, and rule creation, with workflow integration into Copilot. Controlled pilot programs, governance, review cycles, and training are emphasized for effective enterprise use.

- [Introducing Knowledge Agent in SharePoint (Public Preview)](https://dellenny.com/introducing-knowledge-agent-in-sharepoint-public-preview/)

### Building and Operationalizing AI-Powered Agents

Developers continue to build practical agents, with a tutorial on creating a resilience coach using Azure OpenAI and Python. Additional resources show agent memory management with Semantic Kernel and Azure AI Search, alongside customization guides for LLMs and Cognitive Services. An operational workflow demonstrates post-call analytics using Azure OpenAI to process transcripts and feed CRM systems.

- [Building a Resilience Coach with AI on Cozy AI Kitchen](/ai/videos/building-a-resilience-coach-with-ai-on-cozy-ai-kitchen)
- [AI Agent Memory: Building Self-Improving Agents](/ai/videos/ai-agent-memory-building-self-improving-agents)
- [Generative AI in Azure: A Practical Guide to Getting Started](https://dellenny.com/generative-ai-in-azure-a-practical-guide-to-getting-started/)
- [From Call Transcripts to CRM Gold: AI-Powered Post-Call Intelligence](https://techcommunity.microsoft.com/t5/azure-communication-services/from-call-transcripts-to-crm-gold-ai-powered-post-call/ba/p/4456337)

### AI for Social Impact and Enterprise Architecture

UNHCR, Microsoft, and GitHub share new uses of drone data and open-source AI for sustainable planning in refugee settlements, showcasing adaptive open tools. Updated architecture frameworks now account for AI requirements, MLOps, and explainability. Sustainability remains a priority, with AI solutions for digital twins, forecasting, and energy-use reduction continuing the focus on practical environmental reliability.

- [Using AI and Open Source to Map Refugee Settlements: The UNHCR and GitHub Story](https://github.blog/open-source/social-impact/using-ai-to-map-hope-for-refugees-with-unhcr-the-un-refugee-agency/)
- [Software Architecture Frameworks and Artificial Intelligence: Building Smarter Systems](https://dellenny.com/software-architecture-frameworks-and-artificial-intelligence-building-smarter-systems/)
- [Accelerating Sustainability and Resilience with AI-Powered Innovation](https://www.microsoft.com/en-us/microsoft-cloud/blog/2025/09/23/accelerating-sustainability-and-resilience-with-ai-powered-innovation/)

### Other AI News

Research teams at Microsoft, Drexel University, and the Broad Institute present generative AI for rare disease diagnosis, utilizing Azure AI Foundry for evidence aggregation and collaborative genome review—a continuation of last week’s healthcare AI initiatives.

- [Using AI to Assist in Rare Disease Diagnosis: Insights from Microsoft Research](https://www.microsoft.com/en-us/research/blog/using-ai-to-assist-in-rare-disease-diagnosis/)

## ML

Machine learning updates this week focus on analytics scale, architecture maturity, and observability—especially in Microsoft Fabric’s Spark environment. New diagnostics and APIs offer developers more control, with an ongoing emphasis on collaborative production ML and best operational practices.

### Microsoft Fabric Spark Observability and Integration

A new preview for Fabric Spark Applications Comparison lets users visually assess up to four Spark app runs, supporting easier identification of performance issues. This builds on Spark Run Series Analysis, now generally available for grouping job runs and finding anomalies. Monitoring APIs provide real-time insight and automation capabilities for scaling ML operations. Features like Spark Advisor, skew diagnostics, and allocation reporting strengthen automated observability for teams.

User Data Functions, now generally available, enable custom Python logic in Fabric SQL, Lakehouse, Warehouses, and Power BI, encouraging wider reuse and easier integration. The VS Code extension and async data processing further improve developer workflow.

- [Microsoft Fabric Spark Applications Comparison Feature (Preview)](https://blog.fabric.microsoft.com/en-US/blog/public-preview-announcement-fabric-spark-applications-comparison/)
- [Fabric Spark Run Series Analysis: Enterprise-Scale Observability for Microsoft Fabric Spark Jobs](https://blog.fabric.microsoft.com/en-US/blog/fabric-spark-run-series-analysis-generally-available/)
- [Fabric Spark Monitoring APIs Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/general-availability-announcement-fabric-spark-monitoring-apis/)
- [Fabric User Data Functions Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/announcing-fabric-user-data-functions-now-in-general-availability/)

### Evolving MLOps Architectures and Operational Practices

Ongoing best practices encourage the shift from ad-hoc ML deployment to modular, automated workflows with versioning, CI/CD, lifecycle management, and monitoring—with tools like Kafka, Spark Streaming, Feast, MLflow, and Kubernetes as central components. The focus is on continuous delivery, drift detection, and strong governance within practical ML lifecycle management.

Community discussions around MLOps support collaborative learning, with events, podcasts, and networking driving shared expertise in real-world deployment, governance, and technical debt management.

- [MLOps Architectures: Building Scalable AI Systems](https://dellenny.com/mlops-architectures-building-scalable-ai-systems/)
- [MLOps at Scale: How Community Is Driving AI Into Production](https://devops.com/mlops-at-scale-how-community-is-driving-ai-into-production/)

## Azure

Azure’s latest updates include architecture, modernization, automation, observability, security, and developer productivity, oriented toward scalable migration, hybrid cloud, and improved developer experiences.

### Azure Landing Zones and Multi-Region Architecture

Updated Azure AI Landing Zone resources guide organizations on modular and secure AI deployments, covering subscription management, RBAC, policy enforcement, zero trust, and expansion planning. Multi-region architecture lessons cover infrastructure-as-code, resiliency, disaster recovery, and regional failover strategies—helping larger organizations deploy securely and reliably, in line with ongoing migration topics.

- [Azure AI Landing Zone: Enterprise-Scale, Secure, and Governed AI Deployment Architecture](https://techcommunity.microsoft.com/t5/azure-architecture-blog/ai-azure-landing-zone-shared-capabilities-and-models-to-enable/ba/p/4455951)
- [Architecting Multi-Region Solutions in Azure: Practical Lessons Learned](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/architecting-multi-region-solution-in-azure-lessons-learned/ba/p/4415554)

### Infrastructure as Code and Automation Advances

Azure now features Copilot-driven code generation for Terraform, a unified VS Code extension, and policy validation—supporting better cross-stack management for Azure and Microsoft 365 environments. HPC guides demonstrate automated SLURM cluster deployments with CycleCloud and Hammerspace, showing how natural language and AI can streamline infrastructure as code.

- [Accelerating Infrastructure as Code: New Terraform Enhancements for Azure](https://techcommunity.microsoft.com/t5/azure-tools-blog/accelerating-infrastructure-as-code-introducing-game-changing/ba-p/4457341)
- [Simplifying HPC Deployments with Azure CycleCloud and Hammerspace](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/cyclecloud-hammerspace/ba/p/4457043)

### Agentic AI-Powered Modernization and Migration

GitHub Copilot's modernization agents for Java (with .NET in preview) automate code analysis, migration, and artifact generation. Azure Migrate adds agentless discovery, dependency mapping, and guided database migration for PostgreSQL, SQL Server, and Oracle. The Azure Accelerate program helps organizations with large migration projects, reinforcing earlier themes of automation and reduced manual effort.

- [Accelerating Application Migration and Modernization with Agentic AI Tools in Azure](https://azure.microsoft.com/en-us/blog/accelerate-migration-and-modernization-with-agentic-ai/)
- [Discover and Assess PostgreSQL Databases for Azure Migration Using Azure Migrate](https://techcommunity.microsoft.com/t5/azure-migration-and/discover-and-assess-postgresql-databases-for-migration-to-azure/ba/p/4456108)

### Azure Arc Gateway, Arc for Azure Local, and Hybrid Cloud Connectivity

Azure Arc Gateway and Arc Gateway for Azure Local reach general availability, providing streamlined outbound endpoint setup for Arc-enabled and edge deployments. Built-in proxy routing improves secure agent communication, echoing ongoing investments in hybrid and mixed environment management.

- [General Availability of Azure Arc Gateway for Arc-Enabled Servers](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-general-availability-of-the-azure-arc-gateway-for/ba/p/4456356)
- [Announcing the General Availability of Arc Gateway for Azure Local](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-general-availability-of-arc-gateway-for-azure/ba/p/4456256)

### Microsoft Fabric: Data Integration, Orchestration, and Developer Tooling

Microsoft Fabric advances mirroring, adding support for BigQuery (preview) and Oracle for zero-ETL data access. Power BI and chat analytics expand, and Fabric SQL Database receives Copilot-powered query management, VS Code/SSMS support, application lifecycle management (via REST), backup and monitoring tools, and security updates. Fabric VS Code extension and Extensibility Toolkit are now generally available, streamlining app creation and workspace automation. Data Factory introduces new orchestration features, with Copilot assisting expression writing. Workspace admins gain direct per-workspace workload assignment for improved ingestion and quality oversight.

These updates build on recent ecosystem improvements and previews, moving Fabric further toward open administration and orchestration tools.

- [Mirroring in Microsoft Fabric: New Sources, Zero-ETL Data Unification, and AI-Powered Insights](https://blog.fabric.microsoft.com/en-US/blog/whats-new-to-mirroring-new-sources-and-capabilities-for-all-your-zero-etl-needs/)
- [Mirroring for Google BigQuery in Microsoft Fabric: Public Preview Overview](https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-mirroring-for-google-bigquery-in-microsoft-fabric/)
- [Introducing the Microsoft Fabric Extensibility Toolkit](https://blog.fabric.microsoft.com/en-US/blog/introducing-the-microsoft-fabric-extensibility-toolkit/)
- [Microsoft Fabric VS Code Extension: New Features and General Availability](https://blog.fabric.microsoft.com/en-US/blog/announcing-the-general-availability-ga-of-microsoft-fabric-extension-for-vs-code/)
- [Unlocking Enterprise-Ready SQL Database Features in Microsoft Fabric: ALM, Backups, and Copilot Enhancements](https://blog.fabric.microsoft.com/en-US/blog/unlocking-enterprise-ready-sql-database-in-microsoft-fabric-auditing-backup-copilot-more/)
- [Innovations in Fabric Data Factory Orchestration Announced at Fabric Conference Europe 2025](https://blog.fabric.microsoft.com/en-US/blog/announcing-new-innovations-for-fabric-data-factory-orchestration-at-fabric-conference-europe-2025/)
- [Microsoft Fabric Update: Workspace Admins Gain Direct Workload Assignment](https://blog.fabric.microsoft.com/en-US/blog/new-in-microsoft-fabric-empowering-workspace-admins-with-direct-workload-assignment/)
- [MSSQL Extension for VS Code Adds Fabric Integration and Database Provisioning](https://blog.fabric.microsoft.com/en-US/blog/mssql-extension-for-vs-code-fabric-integration-public-preview/)

### Azure Platform Updates, Observability, and Security

The September 26, 2025 Azure update covers service retirements, new AKS Fleet Manager and Insights, logging enhancements, new App Gateway features, and Azure Files Premium backup. Database administrators benefit from added backup and migration tools. Azure Monitor Logs integrates with Fabric Eventstream for streamlined operations data ingest, and App Gateway logs move to resource-focused tables, improving compliance and monitoring. Grafana now helps track Container Apps.

Security features expand with Azure Integrated HSM in public preview, providing hardware-backed cryptography for trusted VMs and easier FIPS compliance. These updates underscore ongoing monitoring, centralized logging, and confidential compute improvements.

- [Azure Update - 26th September 2025: Service Retirements, New Features, and GitHub Copilot Highlights](/ai/videos/azure-update-26th-september-2025-service-retirements-new-features-and-github-copilot-highlights)
- [Unlocking Real-Time Operational Intelligence: Azure Monitor Logs Integration in Fabric via Eventstream](https://blog.fabric.microsoft.com/en-US/blog/unlocking-real-time-operational-intelligence-azure-monitor-logs-integration-in-fabric-via-eventstream/)
- [Enhanced Logging for Azure Application Gateway: Resource-Specific Tables, DCR, and Cost Optimization](https://techcommunity.microsoft.com/t5/azure-networking-blog/unlock-visibility-flexibility-and-cost-efficiency-with/ba/p/4456707)
- [Announcing Azure Container Apps Azure Monitor Dashboards with Grafana (Public Preview)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-the-public-preview-of-azure-container-apps-azure/ba/p/4450958)
- [Microsoft Azure Introduces Azure Integrated HSM: Secure Hardware-Backed Cryptography for Virtual Machines](https://techcommunity.microsoft.com/t5/azure-compute-blog/microsoft-azure-introduces-azure-integrated-hsm-a-key-cache-for/ba/p/4456283)
- [General Availability of Azure Backup Vaulted Support for Azure Files Premium (SSD) Shares](https://techcommunity.microsoft.com/t5/azure-storage-blog/general-availability-of-azure-backup-vaulted-support-for-azure/ba/p/4455307)

### Azure Maps Geocode Autocomplete API

The Azure Maps Geocode Autocomplete API enters public preview, delivering real-time, ranked autocomplete for addresses and places, with multilingual results, filtering options, and metadata enrichment. It replaces Bing Maps Autosuggest to support more user-friendly location experiences for applications such as store locators and rideshares. Official migration guides are available.

- [Introducing the Azure Maps Geocode Autocomplete API](https://techcommunity.microsoft.com/t5/azure-maps-blog/introducing-the-azure-maps-geocode-autocomplete-api/ba/p/4455784)
- [Introducing the Azure Maps Geocode Autocomplete API](https://techcommunity.microsoft.com/t5/azure-maps-blog/introducing-the-azure-maps-geocode-autocomplete-api/ba/p/4455780)

### Other Azure News

Developer tooling updates include forums for Azure Automation feedback and bug reporting, highlighting practical workflow priorities and building on recent onboarding resources.

- [Azure Automation: User Requests, Feature Suggestions, and Bug Reports](https://techcommunity.microsoft.com/t5/azure/azure-automation-feature-improvements-and-bugs/m-p/4456195#M22242)

A deep-dive guide for Azure Database for PostgreSQL Flexible Server covers deployment, tuning, authentication, high availability, encryption, and cost optimization practices.

- [Azure Database for PostgreSQL: Flexible Server Deep Dive](/azure/videos/azure-database-for-postgresql-flexible-server-deep-dive)

A Rust SDK workshop demonstrates secure secret management and authentication for memory-safe cloud-native apps on Azure.

- [Building Secure Applications with Azure SDK for Rust](/azure/videos/building-secure-applications-with-azure-sdk-for-rust)

Analysis of Azure API Management Developer Tier highlights self-hosted gateway capabilities, premium features, and cost considerations for dev/testing environments.

- [Exploring Developer Tier APIM with Self-hosted Gateway for Greater Flexibility](https://techcommunity.microsoft.com/t5/azure-paas-blog/developer-tier-apim-self-hosted-gateway/ba/p/4457556)

Azure Native Pure Storage Cloud delivers integrated block storage for hybrid migrations, providing a native Azure experience and aiding VMware transitions.

- [Azure Native Pure Storage Cloud: Integrating Enterprise Block Storage with Azure](https://techcommunity.microsoft.com/t5/azure-storage-blog/azure-native-pure-storage-cloud-brings-the-best-of-pure-and/ba/p/4456246)

Playwright Testing adds managed, parallel browser sessions on Azure, improving reliability for large test suites in CI/CD.

- [Scaling Playwright End-to-End Tests with Azure Playwright Testing](https://dellenny.com/end-to-end-confidence-in-the-cloud-a-walkthrough-of-azure-playwright-testing-preview/)

Updated Azure Hybrid Benefit guides for Linux VMs offer strategies for cost savings through license management.

- [Unlock Cloud Savings for Linux VMs with the Azure Hybrid Benefit](https://www.thomasmaurer.ch/2025/09/unlock-cloud-savings-for-linux-vms-with-the-azure-hybrid-benefit/)

Cobalt 100 VMs offer energy-efficient, Arm-based compute for analytics, media, AI workloads, and are now widely available.

- [How Azure Cobalt 100 VMs are Powering Real-World Solutions and Boosting Performance](https://azure.microsoft.com/en-us/blog/how-azure-cobalt-100-vms-are-powering-real-world-solutions-delivering-performance-and-efficiency-results/)

Microsoft Fabric Maps provide integrated geospatial intelligence with no-code visualization for applications such as fleet management and live analytics.

- [Maps in Microsoft Fabric – Geospatial Insights for Real-Time Operations](https://blog.fabric.microsoft.com/en-US/blog/introducing-maps-in-fabric-geospatial-insights-for-everyone/)

Azure’s global network sees improvements through hollow core fiber partnerships, increasing reliability and reducing latency for high-performance workloads.

- [Microsoft Azure Accelerates Hollow Core Fiber (HCF) Production with Corning and Heraeus](https://techcommunity.microsoft.com/t5/azure-networking-blog/microsoft-azure-scales-hollow-core-fiber-hcf-production-through/ba/p/4455953)

Microsoft Ignite 2025 promotes community collaboration, bringing workshops and best practices to Azure and AI/data topics.

- [Azure Community Highlights at Microsoft Ignite 2025](https://techcommunity.microsoft.com/t5/azure-events/your-guide-to-azure-community-activations-at-microsoft-ignite/ba/p/4455501)

## Coding

This week’s coding highlights include updates in .NET development, new container tooling in Visual Studio, and practical advice on platform compliance, distributed workflows, and migration planning.

### Visual Studio 2026 and Container Tooling

Visual Studio 2026 Insiders now supports Podman, enabling developers to use this daemonless, rootless container engine instead of Docker for increased security and flexibility. The IDE detects Podman automatically and offers tools for managing images, debugging, and working with containers from the terminal—making secure Linux container development more approachable.

- [Visual Studio 2026 Insiders: Using Podman for Container Development](https://devblogs.microsoft.com/blog/visual-studio-2026-insiders-using-podman-for-container-development)

### .NET Aspire 9.5 and Modern .NET Cloud-Native Development

.NET Aspire 9.5 provides improvements for distributed .NET applications, including a new 'aspire update' CLI for managing SDK/package upgrades, improved dashboards, a single-file AppHost preview for fast prototyping, and color-coded telemetry. GenAI Visualizer aids model debugging, YARP supports static files, and integration with Azure DevTunnels supports local secure testing. Visual Studio 2026 picks up new Aspire tracing features, and migration guides offer help for upgrades from Aspire 8.x.

- [Announcing Aspire 9.5](https://devblogs.microsoft.com/dotnet/announcing-dotnet-aspire-95/)

### .NET MAUI: App Compliance, Migration, and Community Engagement

.NET MAUI applications must update to MAUI 9 to comply with Google Play's 16 KB memory page rule for Android 15+. Guidance is available for checking dependencies and updating build tools. The MAUI Community Standup event in Prague continues focus on collaboration and ongoing platform improvements, reflecting recent compliance and migration support topics.

- [Preparing Your .NET MAUI Apps for Google Play’s 16 KB Page Size Requirement](https://devblogs.microsoft.com/dotnet/maui-google-play-16-kb-page-size-support/)
- [.NET MAUI Community Standup - Live in Prague with the .NET MAUI Team](/coding/videos/net-maui-community-standup-live-in-prague-with-the-net-maui-team)

### .NET Platform Strategy and Database Migrations

Microsoft has clarified support timelines for .NET LTS/STS releases. Nick Chapsas provides migration planning guidance, helping developers minimize upgrade risk. Jeremy Miller’s Data Community Standup compares Marten/PostgreSQL and Entity Framework Core, offering real-world migration Q&A for developers planning database changes.

- [Understanding Microsoft's LTS/STS Changes for .NET Support](/coding/videos/understanding-microsofts-ltssts-changes-for-net-support)
- [.NET Data Community Standup: Jeremy Miller on Marten and Database Migrations](/coding/videos/net-data-community-standup-jeremy-miller-on-marten-and-database-migrations)

### Building Server-Side and CLI Tools with .NET

The latest ASP.NET Community Standup demonstrates a multi-user MCP server, highlighting collaborative code review and refactoring workflows. Andrew Lock’s guide on 'sleep-pc' covers .NET Native AOT usage, Win32 integration, argument processing, and NuGet packaging for durable server-side and CLI tool creation.

- [ASP.NET Community Standup - Vibe Coding a C# MCP Server](/coding/videos/aspnet-community-standup-vibe-coding-a-c-mcp-server)
- [Building sleep-pc: A .NET Native AOT Tool for Automating Windows Sleep](https://andrewlock.net/sleep-pc-a-dotnet-tool-to-make-windows-sleep-after-a-timeout/)

## DevOps

DevOps this week highlights new automation features, updates to API lifecycles, and improved onboarding, with emphasis on collaboration and clear operational processes.

### GitHub Platform and API Lifecycle Updates

GitHub’s pull request 'Files changed' page now supports comments on any changed line, improving code review flexibility for teams and supporting enhanced navigation and API/webhook integration. This update continues previous efforts to refine workflow transparency.

Dependabot alert pagination via offsets is being retired on the REST API—teams should transition to cursor-based pagination for easier handling of larger alert sets. Billing API endpoints now provide aggregate metered usage, streamlining integration and reporting. Enterprise Cloud accounts gain new organizational usage views for better cost management.

- [Enhanced GitHub Pull Request Files Changed Page: Comment Anywhere in Changed Files](https://github.blog/changelog/2025-09-25-pull-request-files-changed-public-preview-now-supports-commenting-on-unchanged-lines)
- [Upcoming Changes to GitHub Dependabot Alerts REST API Pagination](https://github.blog/changelog/2025-09-23-upcoming-changes-to-github-dependabot-alerts-rest-api-offset-based-pagination-parameters-page-first-and-last)
- [GitHub Retires Product-Specific Billing APIs for Actions, Packages, and Storage](https://github.blog/changelog/2025-09-26-product-specific-billing-apis-are-closing-down)
- [Product-specific Billing APIs for GitHub Actions and Packages Are Closing Down](https://github.blog/changelog/2025-09-25-product-specific-billing-apis-are-closing-down)
- [Visualizing GitHub Enterprise Cloud Metered Usage by Organization](https://github.blog/changelog/2025-09-22-visualize-metered-usage-by-organization-in-github-enterprise-cloud)

### AI and Automation in DevOps: Harness and HashiCorp Advances

Harness adds modules for autonomous DevOps tasks, including code maintenance, build troubleshooting, feature flag management, and policy enforcement, all powered by AI. Verification and rollback modules work with observability platforms to improve deployment reliability, and natural language YAML generation supports automated pipeline configuration.

HashiCorp brings agentic AI for infrastructure automation, compatible with Microsoft, AWS, and Red Hat Ansible environments. HCP Terraform Stacks reaches general availability, delivering dependent config management, and new search/action tools (in beta) improve resource management. Vault security updates offer automated cryptography and enhanced credential workflows.

- [Harness Adds New AI Modules to Automate DevOps Pipelines and Maintenance](https://devops.com/harness-extends-scope-and-reach-of-ai-platform-for-automating-devops-workflows/)
- [HashiCorp Introduces Agentic AI and Enhanced Automation for IT Infrastructure](https://devops.com/hashicorp-embraces-agentic-ai-to-streamline-management-of-it/)

### Testing and Developer Onboarding Tools

Playwright Testing now runs on all major browsers and languages, offering managed parallel sessions on Azure and close CI/CD integration. Guides cover advanced debugging, reporting, and DevOps pipeline integration to help teams scale automated testing.

GitHub’s beginner guide delivers video resources for repository management, pull requests, commands, licensing, and profile setup, providing a standardized approach to DevOps onboarding.

- [Getting Started with Microsoft Playwright Testing Features and How to Use It](https://dellenny.com/getting-started-with-microsoft-playwright-testing-features-and-how-to-use-it/)
- [The Ultimate Beginner's Guide to GitHub in 2025](/devops/videos/the-ultimate-beginners-guide-to-github-in-2025)

### Other DevOps News

GitHub refreshes its DMCA takedown policy, Acceptable Use Policy, and moderation practices, clarifying boundaries around developer feedback, synthetic media, and content safety. Teams managing public and open-source projects should review these updates.

- [How GitHub Protects Developers from Copyright Enforcement Overreach](https://github.blog/news-insights/policy-news-and-insights/how-github-protects-developers-from-copyright-enforcement-overreach/)

## Security

Recent updates in security emphasize supply chain protection, vulnerability remediation, artifact signing, and up-to-date governance for developers working in increasingly risk-aware environments.

### Package Registry and Supply Chain Security

NuGet.org now supports Trusted Publishing with short-lived OIDC tokens through GitHub Actions, replacing static keys and improving .NET package safety. Npm registry updates include enforced 2FA, short-lived tokens, and trusted publishing. Chainguard’s curated JavaScript repository adds SLSA provenance and malware scanning for safer dependencies.

- [New Trusted Publishing Enhances Security on NuGet.org](https://devblogs.microsoft.com/dotnet/enhanced-security-is-here-with-the-new-trust-publishing-on-nuget-org/)
- [GitHub's Roadmap for Strengthening npm Supply Chain Security](https://github.blog/security/supply-chain-security/our-plan-for-a-more-secure-npm-supply-chain/)
- [How GitHub Plans to Secure npm After Recent Supply Chain Attacks](https://devops.com/how-github-plans-to-secure-npm-after-recent-supply-chain-attacks/)
- [Chainguard Launches Curated JavaScript Libraries to Enhance Software Supply Chain Security](https://devops.com/chainguard-adds-curated-repository-to-secure-javascript-libraries/)

### Code Scanning, Static Analysis, and Remediation Workflows

CodeQL 2.23.1 introduces improved language detection and query updates for common vulnerabilities, like SSRF and CORS. Incremental analysis speeds scanning for pull requests, and GitHub Security Campaigns with Assignable Alerts help teams coordinate and track remediation within CI flows.

- [CodeQL 2.23.1 Released: Java 25, TypeScript 5.9, and Swift 6.1.3 Support](https://github.blog/changelog/2025-09-26-codeql-2-23-1-adds-support-for-java-25-typescript-5-9-and-swift-6-1-3)
- [Incremental Security Analysis with CodeQL Now Available Across All Languages](https://github.blog/changelog/2025-09-23-incremental-security-analysis-with-codeql-is-now-available-for-all-languages)
- [Accelerate Remediation with GitHub Security Campaigns and Assignable Alerts](https://github.blog/changelog/2025-09-23-accelerate-remediation-with-security-campaigns-and-assignable-alerts-for-code-scanning-and-secret-scanning)

### Artifact Signing, Infrastructure, and Cloud Security

Azure Trusted Signing (public preview) and Notary Project now support integrated signing of OCI images, SBOMs, and Helm charts, helping automate certificate handling for CI/CD. RBAC for AI Landing Zones and secure Databricks deployments via Private Link/Azure Firewall provide templates for regulated operational security.

- [Simplify Image Signing and Verification with Notary Project and Trusted Signing (Public Preview)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/simplify-image-signing-and-verification-with-notary-project-and/ba/p/4455292)
- [Enterprise-Ready RBAC Model for Azure AI Landing Zone](https://techcommunity.microsoft.com/t5/azure-architecture-blog/access-governance-blueprint-for-ai-landing-zone/ba/p/4455910)
- [Securing Azure Databricks Serverless with Private Link and Azure Firewall](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/securing-azure-databricks-serverless-practical-guide-to-private/ba/p/4457083)

### Threat Intelligence, Malware, and Incident Response

Microsoft details the latest XCSSET malware variant targeting macOS dev tools, with mitigation strategies for Defender XDR users. A retail sector incident report outlines response tactics to SharePoint-based attacks, stressing rapid patching and Zero Trust controls. Threat intelligence detects new AI-obfuscated phishing techniques, showcasing layered defense strategies.

- [Latest XCSSET Malware Variant: Technical Deep Dive and Mitigation Guidance](https://www.microsoft.com/en-us/security/blog/2025/09/25/xcsset-evolves-again-analyzing-the-latest-updates-to-xcssets-inventory/)
- [Retail at Risk: How a Single Alert Uncovered a Major Cyberthreat](https://www.microsoft.com/en-us/security/blog/2025/09/24/retail-at-risk-how-one-alert-uncovered-a-persistent-cyberthreat/)
- [AI-Obfuscated Phishing Campaign Detection by Microsoft Threat Intelligence](https://www.microsoft.com/en-us/security/blog/2025/09/24/ai-vs-ai-detecting-an-ai-obfuscated-phishing-campaign/)

### Identity, Data Protection, and Developer Security Skills

A Microsoft Entra Suite guide outlines unified identity, access, risk, passwordless options, and multi-cloud gateways for zero trust. Purview’s DLP and sensitivity labeling (now GA for Fabric) assist with policy enforcement and auditing. OneLake Catalog previews a centralized security permissions tab. An Azure OpenAI customer success story demonstrates App Gateway and NSGs for secure access. A DevSecOps guide covers career progression and practical skills for developers.

- [Microsoft Entra Suite: The Future of Identity and Access Security](https://dellenny.com/microsoft-entra-suite-the-future-of-identity-and-access-security/)
- [Protecting Microsoft Fabric Data with Purview DLP and Sensitivity Labels](https://blog.fabric.microsoft.com/en-US/blog/protecting-your-fabric-data-using-purview-is-now-generally-available/)
- [View and Manage Security in the OneLake Catalog (Preview)](https://blog.fabric.microsoft.com/en-US/blog/view-and-manage-security-in-the-onelake-catalog-now-in-preview/)
- [Securing Azure OpenAI Access from On-Premises with Application Gateway: A Customer Success Story](https://techcommunity.microsoft.com/t5/azure-networking-blog/using-application-gateway-to-secure-access-to-the-azure-openai/ba/p/4456696)
- [The DevSecOps Career Path: What No One Tells You About Getting Started](https://devops.com/the-devsecops-career-path-what-no-one-tells-you-about-getting-started/)

### Other Security News

A practical guide details JWT authentication and authorization for MCP servers in agentic platforms and microservices. GitHub’s Bug Bounty program increases incentives for Copilot ecosystem vulnerability research during Cybersecurity Awareness Month, inviting more robust security testing of developer tooling.

- [Securing Your MCP Server with JWT Authentication and Authorization](https://techcommunity.microsoft.com/t5/microsoft-developer-community/it-s-time-to-secure-your-mcp-servers-here-s-how/ba/p/4434308)
- [GitHub Bug Bounty: Enhanced Incentives for Cybersecurity Awareness Month 2025](https://github.blog/security/vulnerability-research/kicking-off-cybersecurity-awareness-month-2025-researcher-spotlights-and-enhanced-incentives/)
