---
title: This Week’s Key Developments in GitHub Copilot, Azure, and Cloud
author: Tech Hub Team
viewing_mode: internal
date: 2025-08-18 09:00:00 +00:00
tags:
- .NET
- Agentic AI
- AI Tools
- Application Security
- Cloud Security
- Copilot Studio
- Data Analytics
- Enterprise Automation
- GPT 5
- LLMs
- MCP
- Microsoft Fabric
- Open Source
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
Welcome to this week’s tech roundup. AI integration, cloud platforms, and developer tools continue to advance rapidly, with plenty of practical news across the board. GitHub Copilot now uses OpenAI’s GPT-5 and its lighter “Mini” model for more thoughtful automation, smarter code suggestions, and secure, team-oriented workflows on Visual Studio, VS Code, JetBrains, Xcode, and Eclipse. A new Model Context Protocol (MCP) is building a standard for multi-context development automation, while Copilot is broadening—from in-editor repository chat to deep refactoring and modernization tools for businesses.

AI’s role isn’t limited to writing code. Microsoft continues to fold GitHub more tightly into its main AI engineering efforts while rolling out new agent orchestration models in Azure AI Foundry and making Copilot Studio more accessible. There’s a clear uptick in adoption of AI tools, open-source cloud solutions, and analytics that work across cloud boundaries. Security updates across Azure and Microsoft Fabric support stronger data governance and compliance. Teams in DevOps and development get new patterns for modern workflows, better productivity features, and improved support for different programming languages. With technology advancing rapidly, the stories below provide clear, useful context for everyone leading, building, or maintaining solutions today.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GPT-5 Arrives Across GitHub Copilot and Major IDEs](#gpt-5-arrives-across-github-copilot-and-major-ides)
  - [Automation and Developer Workflows: From Natural Language to Real Code](#automation-and-developer-workflows-from-natural-language-to-real-code)
  - [Contextual Collaboration and Code Understanding Expands](#contextual-collaboration-and-code-understanding-expands)
  - [Advanced Protocol and IDE Integration: MCP and Semantic Search](#advanced-protocol-and-ide-integration-mcp-and-semantic-search)
  - [Specialized AI Tools and Automation Modes](#specialized-ai-tools-and-automation-modes)
  - [Modernization and Migration: AI-Driven Refactoring for Enterprise Stacks](#modernization-and-migration-ai-driven-refactoring-for-enterprise-stacks)
  - [Streamlined and Secure: API, Secrets, and Admin Experience](#streamlined-and-secure-api-secrets-and-admin-experience)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [Strategic Shifts and Leadership in Microsoft's AI Ecosystem](#strategic-shifts-and-leadership-in-microsofts-ai-ecosystem)
  - [GPT-5 and AI Model Integrations Across Developer Platforms](#gpt-5-and-ai-model-integrations-across-developer-platforms)
  - [Advancements in Agentic AI and Enterprise Orchestration Patterns](#advancements-in-agentic-ai-and-enterprise-orchestration-patterns)
  - [The Rise and Evolution of Copilot Studio](#the-rise-and-evolution-of-copilot-studio)
  - [Expanding the Model Context Protocol (MCP) Ecosystem](#expanding-the-model-context-protocol-mcp-ecosystem)
  - [AI Adoption, Trust, and Code Security in Practice](#ai-adoption-trust-and-code-security-in-practice)
  - [Innovations in Document Intelligence, Data Analytics, and Azure-powered AI](#innovations-in-document-intelligence-data-analytics-and-azure-powered-ai)
  - [Updates in Platform, Tooling, and AI Skills Development](#updates-in-platform-tooling-and-ai-skills-development)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Cloud-Native LLM Deployment and Optimization](#cloud-native-llm-deployment-and-optimization)
  - [Innovations in Data Lake Interoperability](#innovations-in-data-lake-interoperability)
  - [Advances in Distributed Optimization for AI Model Training](#advances-in-distributed-optimization-for-ai-model-training)
  - [Practical Data Engineering and Analytics Platform Enhancements](#practical-data-engineering-and-analytics-platform-enhancements)
  - [Enterprise ML Transformation and Modern DataOps](#enterprise-ml-transformation-and-modern-dataops)
  - [Other ML News](#other-ml-news)
- [Azure](#azure)
  - [Advancements in Container Management and Hybrid Cloud](#advancements-in-container-management-and-hybrid-cloud)
  - [Security Innovations and Open Source Transparency](#security-innovations-and-open-source-transparency)
  - [Storage Innovation: Azure Files Provisioned V2](#storage-innovation-azure-files-provisioned-v2)
  - [Infrastructure-as-Code and DevOps Integration](#infrastructure-as-code-and-devops-integration)
  - [Data Platform Enhancements and Extended Security Support](#data-platform-enhancements-and-extended-security-support)
  - [Microsoft Intune and Endpoint Management](#microsoft-intune-and-endpoint-management)
  - [Azure API Management and Microservices Architecture](#azure-api-management-and-microservices-architecture)
  - [Real-Time Analytics and Data Integration](#real-time-analytics-and-data-integration)
  - [Major Updates in Monitoring, Performance, and Migration](#major-updates-in-monitoring-performance-and-migration)
  - [Cloud-Native Networking and Observability](#cloud-native-networking-and-observability)
  - [Azure Marketplace and Partner Ecosystem Expansion](#azure-marketplace-and-partner-ecosystem-expansion)
  - [Logic Apps, App Testing, and API Integration](#logic-apps-app-testing-and-api-integration)
  - [AI Infrastructure and Open Data Integration](#ai-infrastructure-and-open-data-integration)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [Major Platform Features and Updates: .NET 10 Preview, Excel Python Image Analysis, Spark Resilience](#major-platform-features-and-updates-net-10-preview-excel-python-image-analysis-spark-resilience)
  - [Guides for Modern, Cross-Platform Development: .NET Aspire, .NET MAUI, Browser-Based .NET](#guides-for-modern-cross-platform-development-net-aspire-net-maui-browser-based-net)
  - [Language Evolution and the Future of Web Development: C# 14, ASP.NET Core & Blazor in .NET 10](#language-evolution-and-the-future-of-web-development-c-14-aspnet-core--blazor-in-net-10)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [The Rise of AI Agents and Automation in DevOps](#the-rise-of-ai-agents-and-automation-in-devops)
  - [Security and Policy Enhancements: Supply Chain and Workflow Hardening](#security-and-policy-enhancements-supply-chain-and-workflow-hardening)
  - [Streamlined Dev to Production Workflows with Modern CI/CD and IaC](#streamlined-dev-to-production-workflows-with-modern-cicd-and-iac)
  - [Workflow Improvements for Visibility, Notifications, and Collaboration](#workflow-improvements-for-visibility-notifications-and-collaboration)
  - [DevOps Release Management: Bottlenecks and Opportunities](#devops-release-management-bottlenecks-and-opportunities)
  - [Enhancements in Application Monitoring and Dependency Management](#enhancements-in-application-monitoring-and-dependency-management)
  - [Migration, Incident, and Access Management in Complex Environments](#migration-incident-and-access-management-in-complex-environments)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Critical Vulnerability Mitigation Across Microsoft Platforms](#critical-vulnerability-mitigation-across-microsoft-platforms)
  - [AI and Security: Expanding Applications and New Risks](#ai-and-security-expanding-applications-and-new-risks)
  - [Advancements in Secret and Credential Management](#advancements-in-secret-and-credential-management)
  - [Cloud and SaaS Security Baselines, Forensic Readiness, and Integration](#cloud-and-saas-security-baselines-forensic-readiness-and-integration)
  - [Real-Time Enforcement and Advanced Identity Management](#real-time-enforcement-and-advanced-identity-management)
  - [Application Security, Supply Chain, and Developer Workflows](#application-security-supply-chain-and-developer-workflows)
  - [Windows, Disk Encryption, and System Recovery](#windows-disk-encryption-and-system-recovery)
  - [Regulatory and Compliance Tools](#regulatory-and-compliance-tools)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot reached a new level of integration this week, thanks to GPT-5 and the leaner “Mini” model now powering Copilot in all major IDEs—including Visual Studio, VS Code, JetBrains, Xcode, and Eclipse—through the Model Context Protocol (MCP). Developers get context-aware AI for writing code, refactoring, and automating projects, plus easier onboarding, API integration, and modernization. Copilot is moving beyond code suggestions to become a real platform for automation, secure collaboration, and better admin controls—raising the standard for quick, high-quality, AI-supported development.

### GPT-5 Arrives Across GitHub Copilot and Major IDEs

GPT-5 is now available across GitHub Copilot, enhancing code completion, context handling, and automation in Visual Studio, VS Code, JetBrains IDEs, Xcode, Eclipse, and the GitHub apps. Visual Studio users will notice better reasoning in complex code, debugging help, faster suggestions, and stronger explanations. The rollout is phased—paid users will see GPT-5 rolled out first, with enterprise admin controls for adoption. Upgrades mean smoother transitions from older models, improved code quality, better onboarding, more effective code reviews, and easier maintenance. GitHub is providing clear changelogs and guides to help users through changes.

- [GPT-5 Comes to GitHub Copilot in Visual Studio](https://devblogs.microsoft.com/visualstudio/gpt-5-now-available-in-visual-studio/)
- [OpenAI GPT-5 Now Available to GitHub Copilot Users in Major IDEs](https://github.blog/changelog/2025-08-12-openai-gpt-5-is-now-available-in-public-preview-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [GPT-5 Now Available in GitHub Copilot: Advanced Features and How to Enable](/videos/2025-08-16-GPT-5-Now-Available-in-GitHub-Copilot-Advanced-Features-and-How-to-Enable.html)
- [GPT-5 and Claude 4.1 Arrive in GitHub Copilot, TypeScript 5.9 Updates, and Community News](/videos/2025-08-15-GPT-5-and-Claude-41-Arrive-in-GitHub-Copilot-TypeScript-59-Updates-and-Community-News.html)

### Automation and Developer Workflows: From Natural Language to Real Code

Copilot combines GPT-5 and MCP for closer DevOps alignment. As shown this week, Copilot can now generate full games from natural language prompts in under a minute. MCP lets Copilot fire off real GitHub actions—handling repository management, issue triage, tool integrations—straight from the IDE, so developers avoid context switching. New guides help teams set up MCP securely and expand Copilot’s role from code generation to full automation, boosting project best practices.

- [Building a Game in 60 Seconds with GPT-5 in GitHub Copilot and MCP Server](https://github.blog/ai-and-ml/generative-ai/gpt-5-in-github-copilot-how-i-built-a-game-in-60-seconds/)
- [Announcing the NuGet MCP Server Preview: Real-Time NuGet Package Management with AI Integration](https://devblogs.microsoft.com/dotnet/nuget-mcp-server-preview/)
- [Why We Open Sourced Our MCP Server and What It Means for Developers](https://github.blog/open-source/maintainers/why-we-open-sourced-our-mcp-server-and-what-it-means-for-you/)

### Contextual Collaboration and Code Understanding Expands

New collaboration features in GitHub Copilot now allow repo chat, contextual Q&A, and Copilot Spaces with repository imports. Developers can interact with full repositories via chat, open pull requests and issues, and handle projects using AI suggestions—streamlining both onboarding and maintenance. These features come directly from community feedback wanting easier integration and more context-aware development.

- [How to Chat with Your Repo & Create PRs with Copilot on GitHub](/videos/2025-08-13-How-to-Chat-with-Your-Repo-and-Create-PRs-with-Copilot-on-GitHub.html)
- [Copilot Spaces Now Support Adding Entire Repositories](https://github.blog/changelog/2025-08-13-add-repositories-to-spaces)

### Advanced Protocol and IDE Integration: MCP and Semantic Search

MCP support now extends to JetBrains, Eclipse, and Xcode, enabling organizations to manage secure, policy-controlled, multi-context workflows. Visual Studio Copilot Chat introduces semantic search, moving past keyword search to give meaning-based code results—improving navigation and making code review and summarization more effective as features continue to grow.

- [Model Context Protocol (MCP) Support for GitHub Copilot Now Available in JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-08-13-model-context-protocol-mcp-support-for-jetbrains-eclipse-and-xcode-is-now-generally-available)
- [Enhancing Visual Studio Copilot Chat with Semantic Code Search](https://devblogs.microsoft.com/visualstudio/improving-codebase-awareness-in-visual-studio-chat/)

### Specialized AI Tools and Automation Modes

Copilot now includes a “Do Epic Shit” chat mode (“Beast Mode”), organizing automation with step-by-step checklists that round out the original agent workflows. AI coding assistants built for platforms like Telerik and KendoUI now provide tailored completions for users working in those ecosystems.

- [Do Epic Shit Chat Mode: Beast Mode for GitHub Copilot](https://harrybin.de/posts/do-epic-shit-chat-mode/)
- [VS Code Live: Telerik & KendoUI AI Coding Assistants and Contextual AI Integration](/videos/2025-08-14-VS-Code-Live-Telerik-and-KendoUI-AI-Coding-Assistants-and-Contextual-AI-Integration.html)

### Modernization and Migration: AI-Driven Refactoring for Enterprise Stacks

Copilot is now automating modernization for enterprise Java and .NET codebases. The App Modernization Extension, using OpenRewrite AI, plans migration, checks dependencies, scaffolds test suites, and confirms compliance automatically. This removes some pain from upgrading legacy applications, following last week’s in-depth guides and ongoing enterprise feedback.

- [Modernizing Legacy Java Applications with GitHub Copilot App Modernization Upgrade](https://techcommunity.microsoft.com/t5/microsoft-developer-community/modernizing-legacy-java-project-using-github-copilot-app/ba-p/4440777)
- [Modernizing and Upgrading Your .NET Apps with Visual Studio and Copilot-Powered AI Tools](/videos/2025-08-14-Modernizing-and-Upgrading-Your-NET-Apps-with-Visual-Studio-and-Copilot-Powered-AI-Tools.html)

### Streamlined and Secure: API, Secrets, and Admin Experience

Copilot has upgraded its user management APIs to include a `last_authenticated_at` field, providing real-time compliance and licensing checks instead of slow CSV exports. AI secret scanning is now more accurate, identifying a wider variety of secret types—including custom tokens—and suggesting faster fixes, making pipelines more secure by default.

- [GitHub Copilot User Management API Adds last_authenticated_at Field](https://github.blog/changelog/2025-08-13-added-last_authenticated_at-to-the-copilot-user-management-api)
- [What is GitHub Secret Protection?](/videos/2025-08-17-What-is-GitHub-Secret-Protection-GitHub-Explained.html)

### Other GitHub Copilot News

The GPT-5 Mini version is now available for every Copilot plan, including free ones. This lightweight, quick model helps reduce quota usage for paid tiers while giving everyone easier access to AI features. Ongoing feedback will inform future improvements.

- [GPT-5 Mini Launches in Public Preview for GitHub Copilot Users](https://github.blog/changelog/2025-08-13-gpt-5-mini-now-available-in-github-copilot-in-public-preview)

Copilot’s platform is growing—Claude 4.1 joins GPT-5 to support code intelligence, along with more support for different runtimes and open models. Project management is tied into Microsoft’s Community Project (MCP). Copilot’s role in .NET and Visual Studio was a key focus at events like VS Live!, highlighting its growing reach.

- [GPT-5 and Claude 4.1 Arrive in GitHub Copilot, TypeScript 5.9 Updates, and Community News](/videos/2025-08-15-GPT-5-and-Claude-41-Arrive-in-GitHub-Copilot-TypeScript-59-Updates-and-Community-News.html)
- [VS Live! Recap: Visual Studio, GitHub Copilot, and Azure AI Session Highlights](https://devblogs.microsoft.com/visualstudio/from-redmond-to-san-diego-vs-live-highlights-session-examples-and-whats-next/)

Practical tutorials this week included streamlining API integration with Copilot and tackling broken migrations using Copilot’s AI debugging tools—two useful areas for boosting day-to-day productivity.

- [Speed Up API Integration with GitHub Copilot](https://cooknwithcopilot.com/blog/speed-up-api-integration.html)
- [Fix Broken Migrations with AI Debugging in VS Code Using GitHub Copilot](https://techcommunity.microsoft.com/t5/educator-developer-blog/fix-broken-migrations-with-ai-powered-debugging-in-vs-code-using/ba-p/4439418)

As new pull request summarization features arrive, GitHub is deprecating text completion for pull request descriptions. Organizations should keep an eye on update channels to catch the latest changes.

- [GitHub Copilot Text Completion for Pull Request Descriptions to Be Deprecated](https://github.blog/changelog/2025-08-15-deprecating-copilot-text-completion-for-pull-request-descriptions)

## AI

AI development accelerates with strategic changes at Microsoft, broader model support, new orchestration frameworks, and evolving developer perspectives. GPT-5, Copilot Studio, and MCP are pushing enterprise innovation, security, and practical tool adoption forward. These updates show AI not just assisting work but actively transforming how software and systems are designed, deployed, and maintained—impacting skills, policies, DevOps, and open-source integration.

### Strategic Shifts and Leadership in Microsoft's AI Ecosystem

GitHub CEO Thomas Dohmke has announced he’ll leave by late 2025 as Microsoft folds GitHub directly into its CoreAI engineering team—ending GitHub’s independent structure and speeding up the flow of AI features for developers. GitHub’s open-source focus will stay, but new features and improved onboarding will come faster under centralized leadership.

- [GitHub CEO Steps Down as Microsoft Integrates GitHub with CoreAI Team](https://devops.com/github-ceo-to-step-down-as-company-is-more-tightly-embraced-by-microsofts-coreai-team/?utm_source=rss&utm_medium=rss&utm_campaign=github-ceo-to-step-down-as-company-is-more-tightly-embraced-by-microsofts-coreai-team)

### GPT-5 and AI Model Integrations Across Developer Platforms

Following the rollout of GPT-5 in Azure and Microsoft’s platform last week, broader support is available now through GitHub Copilot, Azure AI Foundry, VS Code, and other SDKs. GPT-5—including the “mini” version—is now Copilot’s default and helps power agent orchestration in Copilot Studio. Developers benefit from secure access controls, advanced model routing, easier local/cloud inference, and clear setup guides. The transition from preview options to default status, plus modular integrations and more stability, all point to GPT-5 becoming the new norm for production AI.

- [GPT-5 Integrations for Microsoft Developers: GitHub Copilot, Azure AI, and VS Code](https://devblogs.microsoft.com/blog/gpt-5-for-microsoft-developers)
- [Using GPT-5 with Azure AI Foundry, GitHub Copilot, and Copilot Studio in the Microsoft Ecosystem](/videos/2025-08-13-Using-GPT-5-with-Azure-AI-Foundry-GitHub-Copilot-and-Copilot-Studio-in-the-Microsoft-Ecosystem.html)
- [GPT-5 for Developers](/videos/2025-08-14-GPT-5-for-Developers.html)
- [Evaluating GPT-5 Models for RAG on Azure AI Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/gpt-5-will-it-rag/ba-p/4442676)

### Advancements in Agentic AI and Enterprise Orchestration Patterns

Enterprise Agentic AI is moving forward with "Agent Factory," a new orchestration toolkit for agent design—spanning tool usage, workflow planning, and team coordination—built on Azure AI Foundry. The framework includes APIs, an agent catalog, a no-code designer, and Logic Apps integration, making it easier for organizations to deploy and govern agents. This builds on last week’s focus on multiple interacting agents and brings new patterns for teams looking to put agents in real production settings.

- [Agent Factory: Enterprise Patterns and Best Practices for Agentic AI with Azure AI Foundry](https://azure.microsoft.com/en-us/blog/agent-factory-the-new-era-of-agentic-ai-common-use-cases-and-design-patterns/)
- [Model Mondays S2E9: Models for AI Agents](https://techcommunity.microsoft.com/t5/educator-developer-blog/model-mondays-s2e9-models-for-ai-agents/ba-p/4443162)
- [AI Agent's Toolbox: Building Intelligent Agents with Semantic Kernel, MCP Servers, and Python](/videos/2025-08-15-AI-Agents-Toolbox-Building-Intelligent-Agents-with-Semantic-Kernel-MCP-Servers-and-Python.html)
- [Building AI Agents with Semantic Kernel, MCP Servers, and Python](/videos/2025-08-15-Building-AI-Agents-with-Semantic-Kernel-MCP-Servers-and-Python.html)

### The Rise and Evolution of Copilot Studio

Copilot Studio has matured into a no-code hub for building conversational automation, branching out from its Power Virtual Agents roots. Now you can use GPT-powered AI, deploy across multiple channels, and extend it with plugins for scenarios like customer support, HR, or lead management. There are step-by-step guides for non-developers, and direct deployment is more accessible. The ongoing improvements reflect Microsoft’s focus on making automation possible for everyone—from large enterprises to individuals just starting out.

- [Top 5 Use Cases for Copilot Studio in Your Business](https://dellenny.com/top-5-use-cases-for-copilot-studio-in-your-business/)
- [Copilot Studio vs. Power Virtual Agents: What’s Changed?](https://dellenny.com/copilot-studio-vs-power-virtual-agents-whats-changed/)
- [No-Code AI: Building Chatbots with Copilot Studio for Non-Developers](https://dellenny.com/no-code-ai-how-non-developers-can-build-smart-chatbots-with-copilot-studio/)

### Expanding the Model Context Protocol (MCP) Ecosystem

The Model Context Protocol (MCP) is gaining traction as an open standard—offering new integrations with VS Code, Foundry Agent, and Sentry for secure, consistent AI workflows at scale. MCP is being positioned as a modern replacement for SQL in database tasks and as a core orchestration layer for agents. Sentry’s direct monitoring helps teams observe agent operations in real time.

- [Exploring MCP Workflow for Database Management without SQL](/videos/2025-08-12-Exploring-MCP-Workflow-for-Database-Management-without-SQL.html)
- [Boost Your Productivity with Visual Studio & Model Context Protocol (MCP) Servers](/videos/2025-08-15-Boost-Your-Productivity-with-Visual-Studio-and-Model-Context-Protocol-MCP-Servers.html)
- [Introduction to Model Context Protocol (MCP) Servers: Building AI Integrations](/videos/2025-08-14-Introduction-to-Model-Context-Protocol-MCP-Servers-Building-AI-Integrations.html)
- [Unlocking AI Interoperability with Model Context Protocol (MCP)](/videos/2025-08-14-Unlocking-AI-Interoperability-with-Model-Context-Protocol-MCP.html)
- [Integrate Intelligent Agents with MCP and Azure AI Foundry on App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/supercharge-your-app-service-apps-with-ai-foundry-agents/ba-p/4444310)
- [Sentry Integrates MCP Server Monitoring into APM Platform for AI Workflows](https://devops.com/sentry-adds-tool-for-monitoring-mcp-servers-to-apm-platform/?utm_source=rss&utm_medium=rss&utm_campaign=sentry-adds-tool-for-monitoring-mcp-servers-to-apm-platform)

### AI Adoption, Trust, and Code Security in Practice

The Stack Overflow Developer Survey for 2025 reports nearly universal use of AI tools, but confidence in automated output has dropped—developers still rely on their own judgment, especially for autonomous systems. SonarSource’s study flagged persistent security and maintainability issues with LLM-generated code, highlighting the necessity for strict oversight. These patterns echo last week’s concerns about governance and code review.

- [Stack Overflow Survey Reveals Developer Attitudes Toward AI Tools in 2025](https://devops.com/stack-overflow-survey-shows-ai-adoption-for-devs/?utm_source=rss&utm_medium=rss&utm_campaign=stack-overflow-survey-shows-ai-adoption-for-devs)
- [SonarSource Highlights Security Risks and Code Quality Issues in LLM-Generated Code](https://devops.com/sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/?utm_source=rss&utm_medium=rss&utm_campaign=sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code)

### Innovations in Document Intelligence, Data Analytics, and Azure-powered AI

Developers working with unstructured data will find new options in Mistral Document AI on Azure AI Foundry—supporting complex, multilingual document analysis with faster table extraction and less latency. Updates in Microsoft Fabric and SharePoint Embedded enable real-time analytics and no-code extension options, continuing last week’s focus on bridging AI, analytics, and business systems.

- [Mistral Document AI Launches on Azure AI Foundry: Seamless Document Intelligence at Scale](https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/deepening-our-partnership-with-mistral-ai-on-azure-ai-foundry/ba-p/4434656)
- [Advancements in Table Structure Recognition with Azure Document Intelligence](https://techcommunity.microsoft.com/t5/azure-ai-foundry-blog/unveiling-the-next-generation-of-table-structure-recognition/ba-p/4443684)
- [Data Intelligence at Your Fingertips: Fabric’s AI Functions & Data Agents](https://techcommunity.microsoft.com/t5/events/data-intelligence-at-your-fingertips-fabric-s-ai-functions-data/ec-p/4443431#M10)
- [Build the Future of AI-Driven Apps with SharePoint Embedded](https://techcommunity.microsoft.com/t5/microsoft-sharepoint-blog/build-the-future-of-ai-driven-apps-with-sharepoint-embedded/ba-p/4442595)

### Updates in Platform, Tooling, and AI Skills Development

Microsoft has open sourced the Windows Subsystem for Linux (WSL) and introduced Windows AI Foundry to let anybody build custom AI workflows and run models locally. In Australia, a nationwide AI skills program is reaching millions for hands-on upskilling. Azure Cognitive Services has published new resources outlining real-world value. Collectively, these advances push hybrid and on-device AI, as well as open up more possibilities for developers at every level.

- [MSBuild 2025 Highlights: Open Sourcing WSL and Windows AI Foundry](/videos/2025-08-14-MSBuild-2025-Highlights-Open-Sourcing-WSL-and-Windows-AI-Foundry.html)
- [Future Skills Organisation and Microsoft Launch Nationwide AI Skills Accelerator in Australia](https://news.microsoft.com/source/asia/features/fso-microsoft-skills-accelerator-ai/)
- [Unlocking the Power of AI with Azure Cognitive Services](https://dellenny.com/unlocking-the-power-of-ai-with-azure-cognitive-services/)

### Other AI News

AI-powered workflows are now common in Azure, Copilot, and OpenAI environments—including Microsoft Teams, document intelligence, agent-building in VS Code, Azure deployments, and more powerful function-calling for agents. Security and compliance for generative AI have grown with new red-teaming approaches, RAG security checks, PII redaction, and fresh monitoring tools. The Azure AI blog network has now merged for simpler access to technical content and practical guidance.

- [Building a Teams App with Azure Databricks Genie and Azure AI Agent Service](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/supercharge-data-intelligence-build-teams-app-with-azure/ba-p/4442653)
- [Extracting Page Numbers from PDFs with Azure AI Search and OCR](https://techcommunity.microsoft.com/t5/azure-paas-blog/finding-the-right-page-number-in-pdfs-with-ai-search/ba-p/4440758)
- [AI-powered appointment scheduling using Azure OpenAI and Communication Services](https://techcommunity.microsoft.com/t5/azure-communication-services/building-an-ai-receptionist-a-hands-on-demo-with-azure/ba-p/4442959)
- [Building Applications Locally with gpt-oss-20b and the AI Toolkit for VS Code](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-application-with-gpt-oss-20b-with-ai-toolkit/ba-p/4441486)
- [Deploying Lightweight AI Apps on Azure App Service Using GPT-OSS-20B and Flask](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-lightweight-ai-apps-on-azure-app-service-with-gpt-oss-20b/ba-p/4442885)
- [Building AI Agents with Ease: Function Calling in VS Code AI Toolkit](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-ai-agents-with-ease-function-calling-in-vs-code-ai/ba-p/4442637)
- [Red-teaming a RAG Application with Azure AI Evaluation SDK](https://techcommunity.microsoft.com/t5/microsoft-developer-community/red-teaming-a-rag-app-with-the-azure-ai-evaluation-sdk/ba-p/4442682)
- [Announcing the August Preview Model for PII Redaction in Azure AI Language](https://techcommunity.microsoft.com/t5/azure-ai-foundry-blog/announcing-the-text-pii-august-preview-model-release-in-azure-ai/ba-p/4441705)
- [Azure Logic App AI-Powered Monitoring Solution: Automate, Analyze, and Act on Your Azure Data](https://techcommunity.microsoft.com/t5/healthcare-and-life-sciences/azure-logic-app-ai-powered-monitoring-solution-automate-analyze/ba-p/4442665)
- [Azure AI Blogs Consolidate into New Azure AI Foundry Blog](https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/exciting-news-azure-ai-blogs-have-come-together-in-the-new-azure/ba-p/4443002)
- [Build Next-Gen AI Apps with .NET and Azure](/videos/2025-08-14-Build-Next-Gen-AI-Apps-with-NET-and-Azure.html)
- [The Right Kind of AI for Infrastructure as Code](https://devops.com/the-right-kind-of-ai-for-infrastructure-as-code/?utm_source=rss&utm_medium=rss&utm_campaign=the-right-kind-of-ai-for-infrastructure-as-code)
- [Copado Enhances AI Tools to Uncover Salesforce Code Relationships](https://devops.com/copado-extends-ai-reach-to-surface-relationships-between-salesforce-code/?utm_source=rss&utm_medium=rss&utm_campaign=copado-extends-ai-reach-to-surface-relationships-between-salesforce-code)
- [Building Applications Locally with gpt-oss-20b and the AI Toolkit for VS Code](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-application-with-gpt-oss-20b-with-ai-toolkit/ba-p/4441486)
- [Deploying Lightweight AI Apps on Azure App Service Using GPT-OSS-20B and Flask](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-lightweight-ai-apps-on-azure-app-service-with-gpt-oss-20b/ba-p/4442885)
- [Designing Empathetic AI Experiences: Trish Winter-Hunt on Content Design and Azure AI Foundry](/videos/2025-08-12-Designing-Empathetic-AI-Experiences-Trish-Winter-Hunt-on-Content-Design-and-Azure-AI-Foundry.html)
- [Q1 2025 GitHub Innovation Graph Update: Trends in Data Visualization and AI Development](https://github.blog/news-insights/policy-news-and-insights/q1-2025-innovation-graph-update-bar-chart-races-data-visualization-on-the-rise-and-key-research/)
- [Generative AI for Permitting: Accelerating Clean Energy with Microsoft](https://www.microsoft.com/en-us/garage/wall-of-fame/generative-ai-for-permitting/)
- [Future Skills Organisation and Microsoft Launch Nationwide AI Skills Accelerator in Australia](https://news.microsoft.com/source/asia/features/fso-microsoft-skills-accelerator-ai/)
- [Futurum Signal: AI-Powered Market Intelligence for DevOps and Platform Engineering](https://devops.com/futurum-signal-ai-powered-market-intelligence-for-devops-and-platform-engineering/?utm_source=rss&utm_medium=rss&utm_campaign=futurum-signal-ai-powered-market-intelligence-for-devops-and-platform-engineering)
- [Implementing a Center of Excellence for Generative AI](https://www.thomasmaurer.ch/2025/08/implementing-a-center-of-excellence-for-generative-ai/)

## ML

Machine learning updates this week include expanded cloud support for open models, step-by-step LLM deployment, scalable optimizers, and upgraded analytics tools. Microsoft introduced more open-source, cloud-native, and production-friendly options, while new tools like Dion are making large model training more efficient. Companies are highlighting useful deployment strategies and tuning guidance so teams can deliver quality ML systems with less friction.

### Cloud-Native LLM Deployment and Optimization

A new, comprehensive guide walks through deploying OpenAI’s GPT-OSS-20B model on Azure Kubernetes Service (AKS) with KAITO and vLLM, using managed GPUs for scalable and reproducible inference. The tutorial covers everything from setting up clusters to benchmarking, making it easier for teams to roll out open LLMs in Azure environments.

- [Deploying OpenAI’s GPT-OSS-20B on Azure AKS with KAITO and vLLM](https://techcommunity.microsoft.com/t5/ai-machine-learning-blog/deploying-openai-s-first-open-source-model-on-azure-aks-with/ba-p/4444234)

### Innovations in Data Lake Interoperability

Microsoft Fabric's OneLake now lets you access Delta Lake tables as Apache Iceberg format using Apache XTable. This enables analytics engines such as Spark, Trino, or Snowflake to work with lake data without ETL or duplication, advancing Microsoft’s vision for a more flexible, open lakehouse platform.

- [How Microsoft OneLake Seamlessly Provides Apache Iceberg Support for All Fabric Data](https://blog.fabric.microsoft.com/en-US/blog/how-to-access-your-microsoft-fabric-tables-in-apache-iceberg-format/)

### Advances in Distributed Optimization for AI Model Training

Microsoft Research introduced Dion, a distributed optimizer for training massive models like LLaMA-3 405B. Dion leverages orthonormal updates to make optimizer steps up to 10x faster while preserving accuracy, and works well with distributed training frameworks such as FSDP2 and tensor parallelism.

- [Microsoft Releases Dion: A New Scalable Optimizer for Training AI Models](https://www.microsoft.com/en-us/research/blog/dion-the-distributed-orthonormal-update-revolution-is-here/)

### Practical Data Engineering and Analytics Platform Enhancements

A deep dive into the Spark UI offers practical advice for improving job run times, fixing data skew and joins, and spotting garbage collection issues—especially for Databricks users seeking to move past trial-and-error tuning.

- [A Deep Dive into Spark UI for Job Optimization](https://techcommunity.microsoft.com/t5/microsoft-mission-critical-blog/a-deep-dive-into-spark-ui-for-job-optimization/ba-p/4442229)

Microsoft Fabric's Copy Job feature now supports table-level incremental resets, automatic destination table creation, and JSON files—streamlining ETL pipeline deployment by reducing manual steps.

- [Enhancements to Microsoft Fabric Copy Job: Reset Incremental Copy, Auto Table Creation, and JSON Support](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support/)

Azure Essentials Show featured Databricks, highlighting unified analytics, ML lifecycle support, and integration across the Azure platform—useful for developers building new skills for Azure ML environments.

- [Supercharge Data and AI Innovation with Azure Databricks](/videos/2025-08-12-Supercharge-Data-and-AI-Innovation-with-Azure-Databricks.html)

### Enterprise ML Transformation and Modern DataOps

A case study from Adastra and Heritage Grocers Group illustrates how Microsoft Fabric and Azure OpenAI unified post-acquisition data, powered predictive analytics, and rolled out a working system in just six months, showing real benefits from a modern, cloud-based ML setup.

- [How Adastra Used Microsoft Fabric and Azure OpenAI Service to Transform Heritage Grocers Group’s Data Analytics](https://techcommunity.microsoft.com/t5/partner-news/partner-case-study-adastra/ba-p/4442288)

### Other ML News

Excel’s 40th anniversary content showcases its transformation into a capable platform for analytics and ML, including expanded modeling support, Power BI linkage, and deeper connection to Microsoft Fabric.

- [Excel at 40 Week 1: Days 1–3](https://techcommunity.microsoft.com/t5/excel/excel-at-40-week-1-days-1-3/m-p/4443674#M254078)

## Azure

Azure rolled out a diverse set of improvements this week, including updates to hybrid and container management, security, storage, databases, the marketplace, and analytics. The focus remains on operational efficiency, cost management, compliance, and stronger developer tooling. The Azure Marketplace continues to see rapid growth, and security and DevOps features are maturing, giving teams more reliable options for running cloud-native workloads.

### Advancements in Container Management and Hybrid Cloud

Microsoft secured a top position in Gartner’s 2025 Magic Quadrant for container management for the third straight year. AKS Automatic makes cluster management straightforward, while AKS integrates with GitHub Actions, Azure DevOps, and Copilot-powered manifests. Features like GPU-optimization, flexible billing, and improved Arc controls continue Azure’s hybrid, AI-enabled direction.

- [Microsoft Recognized as a Leader in the 2025 Gartner Magic Quadrant for Container Management](https://azure.microsoft.com/en-us/blog/microsoft-is-a-leader-in-the-2025-gartner-magic-quadrant-for-container-management/)

### Security Innovations and Open Source Transparency

Azure Linux with OS Guard, announced at Build 2025, builds in kernel-level security and includes software bill of materials (SBOM), Trusted Launch, and compliance standards (FedRAMP, FIPS 140-3). The Community Edition allows lightweight experimentation, boosting open-source transparency and security.

- [Azure Linux with OS Guard: Enhancing Container Host Security with Code Integrity and Open Source Transparency](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-with-os-guard-immutable-container-host-with-code/ba-p/4437473)

### Storage Innovation: Azure Files Provisioned V2

Azure Files Provisioned v2 for SSD offers granular management and real-time scaling of IOPS, throughput, and capacity—making it easier and more affordable to meet changing business needs. This builds on earlier analytics improvements and assumes users will want to both scale efficiently and control costs.

- [Lower Costs and Boost Flexibility with Azure Files Provisioned v2 for SSD](https://techcommunity.microsoft.com/t5/azure-storage-blog/lower-costs-and-boost-flexibility-with-azure-files-provisioned/ba-p/4443621)
- [Unlocking Flexibility with Azure Files Provisioned V2](https://techcommunity.microsoft.com/t5/itops-talk-blog/unlocking-flexibility-with-azure-files-provisioned-v2/ba-p/4443628)

### Infrastructure-as-Code and DevOps Integration

The new Terraform MSGraph provider and VSCode extension, now in preview, allow teams to automate management of all Microsoft Graph resources—joining together AzureRM, AzAPI, and MSGraph for more consistent onboarding and modular operations across clouds.

- [Announcing Public Preview of the Terraform MSGraph Provider and Microsoft Terraform VSCode Extension](https://techcommunity.microsoft.com/t5/azure-tools-blog/announcing-msgraph-provider-public-preview-and-the-microsoft/ba-p/4443614)

### Data Platform Enhancements and Extended Security Support

Microsoft Fabric now enables workspace-level identity authentication (via Entra ID) and customer-managed encryption keys, delivering stronger compliance. There’s expanded support for PostgreSQL and MySQL, plus cgroup v2 now available for SQL Server on Linux, signaling Azure’s ongoing investment in hybrid database reliability and security.

- [Introducing Support for Workspace Identity Authentication in Fabric Connectors](https://blog.fabric.microsoft.com/en-US/blog/announcing-support-for-workspace-identity-authentication-in-new-fabric-connectors-and-for-dataflow-gen2/)
- [Customer-Managed Keys for Microsoft Fabric Workspaces Now in Public Preview](https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-for-fabric-workspaces-available-in-all-public-regions-now-preview/)
- [Azure Database for PostgreSQL Extended Support: Stay Secure at Every Upgrade Stage](https://techcommunity.microsoft.com/t5/azure-database-for-postgresql/azure-postgresql-extended-support-stay-secure-at-every-stage-of/ba-p/4442283)
- [Extended Support for Azure Database for MySQL: What You Need to Know](https://techcommunity.microsoft.com/t5/azure-database-for-mysql-blog/announcing-extended-support-for-azure-database-for-mysql/ba-p/4442924)
- [SQL Server on Linux Now Supports cgroup v2](https://techcommunity.microsoft.com/t5/sql-server-blog/sql-server-on-linux-now-supports-cgroup-v2/ba-p/4433523)

### Microsoft Intune and Endpoint Management

Microsoft Intune keeps expanding endpoint management capabilities. Integration with Entra ID and Defender supports Zero Trust, while the new IntuneDebug PowerShell module gives IT admins more effective troubleshooting tools for compliance and deployment.

- [Exploring Microsoft Intune: Manage and Secure your Devices and Apps](https://techcommunity.microsoft.com/t5/events/exploring-microsoft-intune-manage-and-secure-your-devices-and/ec-p/4441982#M9)
- [Gpresult-Like Tool for Intune Policy Troubleshooting](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/gpresult-like-tool-for-intune/ba-p/4437008)

### Azure API Management and Microservices Architecture

Azure API management has improved gateway routing and policy controls, making it easier to handle microservices. Workspace models and updated gateway limits support scalability and central management—building off last week’s enhancements.

- [API Gateway Pattern in Azure: Managing APIs and Routing Requests to Microservices](https://dellenny.com/api-gateway-pattern-in-azure-managing-apis-and-routing-requests-to-microservices/)
- [Azure API Management Workspaces Breaking Changes Update: Built-in Gateway & Tier Support](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/update-to-api-management-workspaces-breaking-changes-built-in/ba-p/4443668)

### Real-Time Analytics and Data Integration

Azure Databricks now connects with Power Platform for real-time analytics and write-back scenarios, and AzCopy enables secure, protected transfers to OneLake. Data Factory now supports more flexible partial updates for MongoDB, helping teams manage data more smoothly.

- [Interactive Write-back from Power BI to Azure Databricks with Power Platform Connector](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/closing-the-loop-interactive-write-back-from-power-bi-to-azure/ba-p/4442999)
- [Load Data from Network-Protected Azure Storage Accounts to Microsoft OneLake with AzCopy](https://blog.fabric.microsoft.com/en-US/blog/load-data-from-network-protected-azure-storage-accounts-to-microsoft-onelake-with-azcopy/)
- [Partial Updates in MongoDB via Azure Data Factory Data Flow: Nested Field Modification](https://techcommunity.microsoft.com/t5/azure-data-factory/help-with-partial-mongodb-update-via-azure-data-factory-data/m-p/4443596#M937)

### Major Updates in Monitoring, Performance, and Migration

Azure has enabled automatic agent upgrades for Arc-enabled servers, tripled SQL Managed Instance log rates (Business Critical tier), and expanded observability for Oracle Database. OneLake introduces easier capacity pricing, and parallel BCP migration support for Sybase ASE mirrors ongoing improvements in migration tooling.

- [Higher Log Rate Enhancement in Azure SQL Managed Instance's Business Critical Tier](https://techcommunity.microsoft.com/t5/azure-sql-blog/higher-log-rate-for-business-critical-service-tier-in-azure-sql/ba-p/4444127)
- [Expanding Global Reach and Enhanced Observability with Oracle Database@Azure](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/expanding-global-reach-and-enhancing-observability-with-oracle/ba-p/4443650)
- [Public Preview: Auto Agent Upgrade for Azure Arc-Enabled Servers](https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-auto-agent-upgrade-for-azure-arc-enabled-servers/ba-p/4442556)
- [Simplified OneLake Capacity Costs: Updated Proxy Consumption Rates in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/onelake-costs-simplified-lowering-capacity-utilization-when-accessing-onelake/)
- [Azure Update - 15th August 2025](/videos/2025-08-15-Azure-Update-15th-August-2025.html)
- [Windows Server Datacenter: Azure Edition Preview Build 26461 in Azure](https://techcommunity.microsoft.com/t5/windows-server-insiders/windows-server-datacenter-azure-edition-preview-build-26461-now/m-p/4442961#M4197)
- [Accelerating SAP Sybase ASE to Azure SQL Migration Using SSMA and Parallel BCP](https://techcommunity.microsoft.com/t5/modernization-best-practices-and/sap-sybase-ase-to-azure-sql-migration-using-ssma-and-bcp/ba-p/4436624)

### Cloud-Native Networking and Observability

AKS now offers private pod subnets without overlay networking—helping preserve IP space and simplify hybrid deployments. Micronaut is now integrated for sending native Java metrics, logs, and traces to Azure Monitor, meeting growing demand for open source observability.

- [Private Pod Subnets in AKS Without Overlay Networking](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/private-pod-subnets-in-aks-without-overlay-networking/ba-p/4442510)
- [Sending Metrics, Logs, and Traces from Micronaut Native Image Applications to Azure Monitor](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-native-image-applications-to-azure/ba-p/4443735)

### Azure Marketplace and Partner Ecosystem Expansion

The Azure Marketplace added more than 200 new solutions covering generative AI, analytics, and compliance. Admin and identity solutions are now easier to purchase, demonstrating how Marketplace continues to address real enterprise challenges.

- [New Offerings in Azure Marketplace: July 23-31, 2025](https://techcommunity.microsoft.com/t5/marketplace-blog/new-in-azure-marketplace-july-23-31-2025/ba-p/4431277)
- [Transactable Partner Solutions: Apptividad and CoreView in Azure Marketplace](https://techcommunity.microsoft.com/t5/marketplace-blog/apptividad-and-coreview-offer-transactable-partner-solutions-in/ba-p/4431278)

### Logic Apps, App Testing, and API Integration

Logic Apps Data Mapper has reached general availability, now with improved developer experience and better VS Code support. The new Playwright Workspaces guide covers both local and cloud testing, aiming for more reliable automation and artifact management.

- [General Availability: Enhanced Data Mapper Experience in Logic Apps (Standard)](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/general-availability-enhanced-data-mapper-experience-in-logic/ba-p/4442296)
- [End-to-End Azure App Testing with Playwright Workspaces: Local and Cloud Workflows](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-app-testing-playwright-workspaces-for-local-to-cloud-test/ba-p/4442711)

### AI Infrastructure and Open Data Integration

Azure Storage remains central to large AI and LLM training projects, with new features like Scaled Accounts and Blobfuse2 supporting high-volume and secure workflows. Tutorials make it easier for teams to try these capabilities right away.

- [How Azure Storage Powers AI Workloads: Behind the Scenes with OpenAI, Blobfuse & More](https://techcommunity.microsoft.com/t5/itops-talk-blog/how-azure-storage-powers-ai-workloads-behind-the-scenes-with/ba-p/4442540)

### Other Azure News

Development tooling improvements are ongoing, following last week’s CLI and naming convention updates.

- [Azure Update - 15th August 2025](/videos/2025-08-15-Azure-Update-15th-August-2025.html)
- [New Offerings in Azure Marketplace: July 23-31, 2025](https://techcommunity.microsoft.com/t5/marketplace-blog/new-in-azure-marketplace-july-23-31-2025/ba-p/4431277)
- [General Availability: Enhanced Data Mapper Experience in Logic Apps (Standard)](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/general-availability-enhanced-data-mapper-experience-in-logic/ba-p/4442296)

Security is maturing as well, with new compliance and vulnerability management features.

- [Azure Linux with OS Guard: Enhancing Container Host Security with Code Integrity and Open Source Transparency](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-with-os-guard-immutable-container-host-with-code/ba-p/4437473)
- [Extended Support for Azure Database for MySQL: What You Need to Know](https://techcommunity.microsoft.com/t5/azure-database-for-mysql-blog/announcing-extended-support-for-azure-database-for-mysql/ba-p/4442924)

Migration and troubleshooting guides continue to help teams adopt best practices.

- [Accelerating SAP Sybase ASE to Azure SQL Migration Using SSMA and Parallel BCP](https://techcommunity.microsoft.com/t5/modernization-best-practices-and/sap-sybase-ase-to-azure-sql-migration-using-ssma-and-bcp/ba-p/4436624)
- [Troubleshooting Azure Stack HCI Local Cluster Deployment: Network Configuration Error](https://techcommunity.microsoft.com/t5/azure-stack/error-no-file/m-p/4443115#M277)

## Coding

Developers are getting meaningful updates this week, from fresh language features in .NET 10 Preview and smarter Excel Python tools to improved resilience in Spark. New guides cover everything from disk cleanup to building dual-transport servers, reflecting a bigger focus on practical, modern, and cross-platform workflows.

### Major Platform Features and Updates: .NET 10 Preview, Excel Python Image Analysis, Spark Resilience

.NET 10 Preview 7 brings new cryptographic features, faster JSON serialization, better diagnostics, easier authentication, and stronger cross-platform support. The improvements boost usability, security, and help streamline cloud-native workflows.

The latest Excel update lets you use Python natively for image analysis tasks like blur detection, brightness checks, and metadata collection—bringing advanced vision analysis tools to everyday spreadsheet users.

Spark’s improved Iteration Panel makes file filtering and API failure handling easier, helping with smoother, more reliable development cycles.

- [.NET 10 Preview 7 Released: Key Updates for Libraries, ASP.NET Core, Blazor, and MAUI](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-7/)
- [Analyzing Images with Python in Excel: Now Natively Supported](https://techcommunity.microsoft.com/t5/microsoft-365-insider-blog/analyze-images-with-python-in-excel/ba-p/4440388)
- [Spark Resilience Improvements Enhance Reliability and Iteration Experience](https://github.blog/changelog/2025-08-13-spark-resilience-improvements)

### Guides for Modern, Cross-Platform Development: .NET Aspire, .NET MAUI, Browser-Based .NET

Step-by-step guides help teams get started with .NET Aspire for distributed systems, including boilerplate code, GitHub Actions integration, and custom metrics for ongoing monitoring. Visual Studio and .NET MAUI tutorials focus on building cross-platform mobile and desktop apps—with advice on UI optimization, file size reduction, and streamlined updates.

A walkthrough from Andrew Lock shows how to run .NET in the browser without Blazor, using WebAssembly templates and JavaScript interop for high-performance client apps.

- [Building Confident Application Systems with .NET Aspire: From Dev to Deployment](/videos/2025-08-14-Building-Confident-Application-Systems-with-NET-Aspire-From-Dev-to-Deployment.html)
- [Building Mobile and Desktop Apps with Visual Studio and .NET MAUI](/videos/2025-08-14-Building-Mobile-and-Desktop-Apps-with-Visual-Studio-and-NET-MAUI.html)
- [Running .NET in the Browser Without Blazor Using WASM](https://andrewlock.net/running-dotnet-in-the-browser-without-blazor/)

### Language Evolution and the Future of Web Development: C# 14, ASP.NET Core & Blazor in .NET 10

A detailed look at C# 14 covers improved pattern matching, nullability support, and value types—making code safer and cleaner. Current .NET team previews for ASP.NET Core and Blazor in .NET 10 include modern security, diagnostics, WebAuthn support, integrated AI libraries, and faster project ramp-up with Aspire, helping developers stay current with critical web advances.

- [Highlights and Upcoming Features in C#: A Deep Dive into C# 14](/videos/2025-08-14-Highlights-and-Upcoming-Features-in-C-A-Deep-Dive-into-C-14.html)
- [The Future of Web Development with ASP.NET Core & Blazor in .NET 10](/videos/2025-08-14-The-Future-of-Web-Development-with-ASPNET-Core-and-Blazor-in-NET-10.html)

### Other Coding News

VS Code’s “Beast mode” rolls out improvements for batch edits, UI adjustments, and workflow enhancements, with a video guide to help users get started. Additional tutorials show how to simplify .NET mapping with Facet, automate disk cleanup with PowerShell, and build STDIO/HTTP dual-transport MCP servers for flexible cloud and local deployments.

- [VS Code Beast Mode Explained: Features and Usage](/videos/2025-08-14-VS-Code-Beast-Mode-Explained-Features-and-Usage.html)
- [Enhancing .NET Code: Using Facet Instead of Traditional Mapping](/videos/2025-08-13-Enhancing-NET-Code-Using-Facet-Instead-of-Traditional-Mapping.html)
- [Finding Large Directories and Recovering Lost Disk Space with PowerShell](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
- [Building a Dual-Transport MCP Server with .NET: STDIO and HTTP Support](https://techcommunity.microsoft.com/t5/microsoft-developer-community/one-mcp-server-two-transports-stdio-and-http/ba-p/4443915)

## DevOps

DevOps is seeing another round of automation, improved workflow features, and more ways to manage releases and access securely. New AI agents, stricter policies, and improved collaboration reflect a steady shift toward streamlined, well-governed developer operations.

### The Rise of AI Agents and Automation in DevOps

Google’s Gemini CLI GitHub Actions (beta) bring “AI teammate” capabilities for issue triage, reviews, and more—complete with allowlisting, Workload Identity integration, OpenTelemetry monitoring, and customizable workflows. Free quotas help lower the cost of getting started.

Shadow, a secure, open-source AI coding agent, is designed for production pipelines with semantic search and automatic documentation—helping handle technical debt and supporting both collaborative and automated DevOps patterns.

- [How Gemini CLI GitHub Actions is Changing Developer Workflows](https://devops.com/how-gemini-cli-github-actions-is-changing-developer-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=how-gemini-cli-github-actions-is-changing-developer-workflows)
- [Shadow: How AI Coding Agents are Transforming DevOps Workflows](https://devops.com/shadow-how-ai-coding-agents-are-transforming-devops-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=shadow-how-ai-coding-agents-are-transforming-devops-workflows)

### Security and Policy Enhancements: Supply Chain and Workflow Hardening

GitHub Actions now supports blocking/versioning and SHA pinning, making it possible to harden CI/CD supply chains and guarantee artifact integrity. Fast incident response and automated governance help address new security threats as the platform evolves.

- [GitHub Actions Policy Adds Blocking and SHA Pinning for Enhanced Security](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions)

### Streamlined Dev to Production Workflows with Modern CI/CD and IaC

A “Dev to Prod” guide outlines how to use Azure Developer CLI with DevOps YAML pipelines for efficient builds, artifact handling, and Copilot-driven diagnostics. This matches ongoing trends toward better, faster development-to-production workflows.

- [Azure Developer CLI: Dev to Production with Azure DevOps Pipelines](https://devblogs.microsoft.com/devops/azure-developer-cli-from-dev-to-prod-with-azure-devops-pipelines/)

### Workflow Improvements for Visibility, Notifications, and Collaboration

GitHub has enhanced reviewer visibility in pull requests, improved email filters, and expanded supported file types for attachments—further smoothing team workflow and onboarding processes.

- [Clearer Pull Request Reviewer Status and Enhanced Email Filtering in GitHub](https://github.blog/changelog/2025-08-14-clearer-pull-request-reviewer-status-and-enhanced-email-filtering)
- [Expanded File Type Support for GitHub Attachments](https://github.blog/changelog/2025-08-13-expanded-file-type-support-for-attachments-across-issues-pull-requests-and-discussions)

### DevOps Release Management: Bottlenecks and Opportunities

A recent survey of mobile app release practices finds high manual effort and frequent interruptions, highlighting opportunities for better automation and more reliable CI/CD pipelines.

- [Survey Reveals Major Challenges in Mobile Application Release Management](https://devops.com/survey-surfaces-multiple-mobile-application-release-management-headaches/?utm_source=rss&utm_medium=rss&utm_campaign=survey-surfaces-multiple-mobile-application-release-management-headaches)

### Enhancements in Application Monitoring and Dependency Management

AppSignal now offers zero-config OpenTelemetry monitoring for mainstream languages, while Dependabot adds vcpkg update automation for C/C++—making security and dependency management easier in native codebases.

- [AppSignal Adds Native OpenTelemetry Support for Enhanced Application Monitoring](https://devops.com/appsignal-adds-opentelemetry-support-to-monitoring-platform/?utm_source=rss&utm_medium=rss&utm_campaign=appsignal-adds-opentelemetry-support-to-monitoring-platform)
- [Dependabot Adds Version Update Support for vcpkg](https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg)

### Migration, Incident, and Access Management in Complex Environments

After a GitHub Enterprise Importer outage, stronger testing and firewall management were put in place. Visual Studio subscribers can now access metered enterprise billing. An ITU open-source migration guide provides a four-step model, supporting teams moving from private to public projects.

- [GitHub Enterprise Importer Incident and IP Range Update: July 2025 Availability Report](https://github.blog/news-insights/company-news/github-availability-report-july-2025/)
- [Metered GitHub Enterprise Billing Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-08-14-introducing-metered-github-enterprise-billing-for-visual-studio-subscriptions-with-github-enterprise)
- [How the International Telecommunication Union Open Sourced Its Tech: A Four-Step Guide](https://github.blog/open-source/social-impact/from-private-to-public-how-a-united-nations-organization-open-sourced-its-tech-in-four-steps/)

### Other DevOps News

Dev tools continue to receive attention, with new OpenTelemetry features in AppSignal, simplified dependency updates via Dependabot, and more collaborative GitHub features. Security advances include improved Actions policy controls and user management APIs. There’s also updated guidance on migration, incident handling, and real-world DevOps lessons from practitioners.

- [AppSignal Adds Native OpenTelemetry Support for Enhanced Application Monitoring](https://devops.com/appsignal-adds-opentelemetry-support-to-monitoring-platform/?utm_source=rss&utm_medium=rss&utm_campaign=appsignal-adds-opentelemetry-support-to-monitoring-platform)
- [Dependabot Adds Version Update Support for vcpkg](https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg)
- [Clearer Pull Request Reviewer Status and Enhanced Email Filtering in GitHub](https://github.blog/changelog/2025-08-14-clearer-pull-request-reviewer-status-and-enhanced-email-filtering)
- [Expanded File Type Support for GitHub Attachments](https://github.blog/changelog/2025-08-13-expanded-file-type-support-for-attachments-across-issues-pull-requests-and-discussions)
- [GitHub Actions Policy Adds Blocking and SHA Pinning for Enhanced Security](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions)
- [Metered GitHub Enterprise Billing Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-08-14-introducing-metered-github-enterprise-billing-for-visual-studio-subscriptions-with-github-enterprise)
- [How the International Telecommunication Union Open Sourced Its Tech: A Four-Step Guide](https://github.blog/open-source/social-impact/from-private-to-public-how-a-united-nations-organization-open-sourced-its-tech-in-four-steps/)
- [GitHub Enterprise Importer Incident and IP Range Update: July 2025 Availability Report](https://github.blog/news-insights/company-news/github-availability-report-july-2025/)
- [Persistent Visual Studio Enterprise Access Level in Azure DevOps After License Removal](https://techcommunity.microsoft.com/t5/azure/unable-to-revert-azure-devops-user-access-level/m-p/4442871#M22102)
- [Troubleshooting MCC Phantom Install Issues on Windows Server 2022 with WSL](https://techcommunity.microsoft.com/t5/microsoft-connected-cache-for/mcc-phantom-install/m-p/4444201#M108)
- [From Firefighting to Forward-Thinking: Real-World Lessons in DevOps and Cloud Engineering](https://devops.com/from-firefighting-to-forward-thinking-my-real-world-lessons-in-devops-and-cloud-engineering/?utm_source=rss&utm_medium=rss&utm_campaign=from-firefighting-to-forward-thinking-my-real-world-lessons-in-devops-and-cloud-engineering)

## Security

This week’s security news spotlights urgent vulnerability fixes, better credential protection, cloud and SaaS baseline upgrades, and practical integrations for identity, compliance, and recovery. Organizations must move quickly to patch risks, especially in Microsoft environments, while juggling the expanding roles—and new risks—of AI in security automation.

### Critical Vulnerability Mitigation Across Microsoft Platforms

A SharePoint vulnerability (CVE-2025-53770) allowed unauthenticated code execution through auth bypass. Recent versions received patches, while older systems relied on custom Azure WAF rules. Exchange faced a privilege escalation vulnerability (CVE-2025-53786), remedied by hotfixes and updated trust models. SQL Server saw a denial-of-service risk (CVE-2025-49759) now patched across supported releases. These events reinforce the need for prompt patching, layered defenses, and live monitoring.

- [Mitigating SharePoint CVE-2025-53770 Using Azure Web Application Firewall](https://techcommunity.microsoft.com/t5/azure-network-security-blog/protect-against-sharepoint-cve-2025-53770-with-azure-web/ba-p/4442050)
- [Mitigating CVE-2025-53786: Hybrid Exchange Server Privilege Escalation with MDVM](https://techcommunity.microsoft.com/t5/microsoft-defender-vulnerability/mdvm-guidance-for-cve-2025-53786-exchange-hybrid-privilege/ba-p/4442337)
- [Security Update Available for SQL Server 2019 RTM GDR](https://techcommunity.microsoft.com/t5/sql-server-blog/security-update-for-sql-server-2019-rtm-gdr/ba-p/4441689)
- [Security Update Available for SQL Server 2022 RTM GDR](https://techcommunity.microsoft.com/t5/sql-server-blog/security-update-for-sql-server-2022-rtm-gdr/ba-p/4441687)
- [August 2025 Exchange Server Security Updates Released](https://techcommunity.microsoft.com/t5/exchange-team-blog/released-august-2025-exchange-server-security-updates/ba-p/4441596)

### AI and Security: Expanding Applications and New Risks

AI is now being used for credential exposure alerts (Entra/AD), automated incident triage in Defender, and open-source supply chain scans (CodeQL, Copilot). However, LLM-generated code can introduce new risks. Microsoft and partners are recommending thorough review and end-to-end AI security, with organizations like Dow sharing how AI has improved threat detection and SecOps workflows.

- [How Microsoft Defender Uses AI to Detect Exposed Credentials in Identity Systems](https://techcommunity.microsoft.com/t5/microsoft-defender-xdr-blog/leaving-the-key-under-the-doormat-how-microsoft-defender-uses-ai/ba-p/4439870)
- [How Microsoft Defender Experts Uses AI to Cut Through the Noise](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/how-microsoft-defender-experts-uses-ai-to-cut-through-the-noise/ba-p/4443601)
- [Securing the Open Source Supply Chain: Impact of the GitHub Secure Open Source Fund](https://github.blog/open-source/maintainers/securing-the-supply-chain-at-scale-starting-with-71-important-open-source-projects/)
- [SonarSource Research Highlights Security Risks in LLM-Generated Code](https://devops.com/sonar-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/?utm_source=rss&utm_medium=rss&utm_campaign=sonar-surfaces-multiple-caveats-when-relying-on-llms-to-write-code)
- [From Traditional Security to AI-Driven Cyber Resilience: Microsoft’s Approach to Securing AI](https://techcommunity.microsoft.com/t5/microsoft-security-community/from-traditional-security-to-ai-driven-cyber-resilience/ba-p/4442652)
- [How Dow Uses Microsoft Security Copilot and AI to Transform Cybersecurity Operations](https://www.microsoft.com/en-us/security/blog/2025/08/12/dows-125-year-legacy-innovating-with-ai-to-secure-a-long-future/)

### Advancements in Secret and Credential Management

GitHub Secret Scanning now supports 12 more token types for proactive risk detection. Secret validity checks and push protection in GitHub Advanced Security for Azure DevOps make discovery and remediation easier. Azure DevOps has improved OAuth secret management by only displaying secrets at creation. GitHub’s MCP Server now boosts public repo scanning.

- [Secret Scanning Expands Support: 12 New Token Validators Added to GitHub](https://github.blog/changelog/2025-08-12-secret-scanning-adds-12-validators-including-cockroach-labs-polar-and-yandex)
- [Secret Validity Checks Launch in GitHub Advanced Security for Azure DevOps](https://devblogs.microsoft.com/devops/hunting-living-secrets-secret-validity-checks-arrive-in-github-advanced-security-for-azure-devops/)
- [Azure DevOps Improves OAuth Client Secret Security: Secrets Now Shown Only Once](https://devblogs.microsoft.com/devops/azure-devops-oauth-client-secrets-now-shown-only-once/)
- [GitHub MCP Server Enhances Secret Scanning and Push Protection for Public Repositories](https://github.blog/changelog/2025-08-13-github-mcp-server-secret-scanning-push-protection-and-more)

### Cloud and SaaS Security Baselines, Forensic Readiness, and Integration

Azure’s updated guides on forensic readiness cover MFA, RBAC, auditing, recovery, and compliance. Third-party SaaS integration guides explain secure setup and consistent permission management for Slack, Trello, and Google. Defender for Cloud now matches compliance for US Government clouds. Queensland, Australia, has improved support for vulnerable groups with a unified MS 365 E5 stack.

- [Cloud Forensics: Implementing Security Baselines for Forensic Readiness in Microsoft Azure](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/cloud-forensics-prepare-for-the-worst-implement-security/ba-p/4440310)
- [Secure Integration of Microsoft 365 with Slack, Trello, and Google Services](https://dellenny.com/how-to-integrate-m365-with-third-party-saas-tools-slack-trello-google-services-without-breaking-security/)
- [Microsoft Defender for Cloud Expands Security and Compliance Features for U.S. Government Cloud](https://techcommunity.microsoft.com/t5/microsoft-defender-for-cloud/microsoft-defender-for-cloud-expands-u-s-gov-cloud-support-for/ba-p/4441118)
- [Queensland Government Enhances Cybersecurity for Vulnerable Communities with Microsoft 365 E5](https://news.microsoft.com/source/asia/2025/08/14/championing-safety-how-one-queensland-government-department-is-transforming-cybersecurity-to-better-support-vulnerable-communities/)

### Real-Time Enforcement and Advanced Identity Management

Continuous Access Evaluation (CAE) now provides real-time session revocation on Azure DevOps—closing security gaps faster. Developers should update workflows to react to new CAE signals. A new Entra ID guide for Windows Forms shows secure token-based identity setup for Arc-enabled SQL Server.

- [Continuous Access Evaluation (CAE) Brings Real-Time Security to Azure DevOps](https://devblogs.microsoft.com/devops/real-time-security-with-continuous-access-evaluation-cae-comes-to-azure-devops/)
- [Using Entra ID Authentication with Arc-Enabled SQL Server in a .NET Windows Forms Application](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/using-entra-id-authentication-with-arc-enabled-sql-server-in-a/ba-p/4435168)

### Application Security, Supply Chain, and Developer Workflows

A new survey shows most companies still deploy code with known vulnerabilities, putting them at risk. CodeQL now supports Kotlin and Rust and offers improved static analysis for JavaScript/React. The Minimus hardened images service adds VEX and Microsoft SSO to improve supply chain and container compliance.

- [Most Organizations Face Breaches Caused by Vulnerable Code, Survey Finds](https://devops.com/survey-traces-large-amount-of-breaches-back-to-vulnerable-code/?utm_source=rss&utm_medium=rss&utm_campaign=survey-traces-large-amount-of-breaches-back-to-vulnerable-code)
- [CodeQL Expands Support for Kotlin and Improves Static Analysis Accuracy](https://github.blog/changelog/2025-08-14-codeql-expands-kotlin-support-and-additional-accuracy-improvements)
- [Minimus Adds VEX Support and Microsoft SSO Integration to Hardened Images Service](https://devops.com/minimus-adds-vex-support-to-managed-hardened-images-service/?utm_source=rss&utm_medium=rss&utm_campaign=minimus-adds-vex-support-to-managed-hardened-images-service)

### Windows, Disk Encryption, and System Recovery

Microsoft STORM found attackers could chain four BitLocker vulnerabilities in the Windows Recovery Environment to unlock protected drives. The July 2025 patch addresses these design flaws, serving as a reminder of the importance of layered defense and ongoing validation.

- [BitUnlocker: Leveraging Windows Recovery to Extract BitLocker Secrets](https://techcommunity.microsoft.com/t5/microsoft-security-community/bitunlocker-leveraging-windows-recovery-to-extract-bitlocker/ba-p/4442806)

### Regulatory and Compliance Tools

The Eclipse Foundation has published OCCTET, a free toolkit to help organizations fulfill requirements under Europe’s Cyber Resilience Act. Microsoft Purview eDiscovery adds automated workflows, search upgrades, and audit controls. There are also plain-language guides for small businesses on labeling, DLP, and conditional access.

- [Eclipse Foundation Publishes Toolkit to Simplify CRA Compliance](https://devops.com/eclipse-foundation-publishes-toolkit-to-simplify-cra-compliance/?utm_source=rss&utm_medium=rss&utm_campaign=eclipse-foundation-publishes-toolkit-to-simplify-cra-compliance)
- [What’s New in Microsoft Purview eDiscovery](https://techcommunity.microsoft.com/t5/microsoft-security-community/what-s-new-in-microsoft-purview-ediscovery/ba-p/4441676)
- [Practical Data Protection in Microsoft 365: Sensitivity Labels, DLP, and Conditional Access for Small Businesses](https://dellenny.com/protecting-your-business-data-sensitivity-labels-dlp-and-conditional-access-explained-simply/)

### Other Security News

Malware scanning is now generally available for Azure Government Secret/Top-Secret workloads in Defender for Storage. Microsoft Teams encryption details are clarified, and S/MIME troubleshooting tackles certificate issues. There are new guides for OAuth2 automation in ADF and issuing directory extension claims in Entra ID, plus registration details for Microsoft Ignite 2025 (focused on AI defense and community forums).

- [Malware Scanning Now Available for Azure Government Secret and Top-Secret Clouds](https://techcommunity.microsoft.com/t5/microsoft-defender-for-cloud/malware-scanning-add-on-is-now-generally-available-in-azure-gov/ba-p/4442502)
- [Encryption in Microsoft Teams: How Microsoft Secures Collaboration and Communication](https://techcommunity.microsoft.com/t5/microsoft-teams-blog/encryption-in-microsoft-teams-june-2025/ba-p/4442913)
- [Troubleshooting S/MIME Setup in Exchange Online and M365: OWA and Outlook Certificate Issues](https://techcommunity.microsoft.com/t5/exchange/smime-not-working-in-owa/m-p/4443230#M16650)
- [Troubleshooting OAuth2 API Token Retrieval with ADF Web Activity](https://techcommunity.microsoft.com/t5/azure-data-factory/getting-an-oauth2-api-access-token-using-client-id-and-client/m-p/4443568#M936)
- [Issuing Custom Claims Using Directory Extension Attributes in Microsoft Entra ID](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/issuing-custom-claims-using-directory-extension-attributes-in/ba-p/4441980)
- [Connect with the Security Community at Microsoft Ignite 2025](https://www.microsoft.com/en-us/security/blog/2025/08/13/connect-with-the-security-community-at-microsoft-ignite-2025/)
