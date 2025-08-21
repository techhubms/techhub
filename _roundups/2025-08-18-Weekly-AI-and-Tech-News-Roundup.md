---
layout: "post"
title: "A New Era for GitHub Copilot and AI Integration"
description: "This week brought notable progress in AI-driven developer tools, platform automation, and secure cloud services. GitHub Copilot opened public previews for GPT-5, agent workflows, and deeper IDE connections, expanding AI-assisted coding from basic suggestions to more collaborative engineering support. Across the Microsoft ecosystem, new AI models, cloud security improvements, and fresh ML/DevOps practices continued to shape the evolving software and enterprise IT landscape."
author: "Tech Hub Team"
excerpt_separator: <!--excerpt_end-->
viewing_mode: "internal"
date: 2025-08-18 09:00:00 +00:00
permalink: "/2025-08-18-Weekly-AI-and-Tech-News-Roundup.html"
categories: ["AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security"]
tags: [".NET 10", "AI", "AI Agents", "Azure", "Claude Opus 4.1", "Cloud Infrastructure", "Coding", "Compliance", "Data Analytics", "DevOps", "GitHub Copilot", "GPT 5", "Machine Learning", "Microsoft Fabric", "ML", "Model Context Protocol", "Open Source", "Roundups", "Security", "Visual Studio Code", "Workflow Automation"]
tags_normalized: ["net 10", "ai", "ai agents", "azure", "claude opus 4 dot 1", "cloud infrastructure", "coding", "compliance", "data analytics", "devops", "github copilot", "gpt 5", "machine learning", "microsoft fabric", "ml", "model context protocol", "open source", "roundups", "security", "visual studio code", "workflow automation"]
---

Welcome to this week’s roundup, highlighting the ongoing convergence of intelligent coding assistants, cloud platforms, and enterprise automation. Recent developments from GitHub Copilot and Microsoft’s AI ecosystem reflect how Copilot is steadily evolving toward a more capable engineering agent. GPT-5 and Claude Opus 4.1 public previews are now available in leading IDEs, offering accessible next-generation AI for both free and paid users. Features like the Model Context Protocol (MCP) now support broader context sharing and enable collaboration across multiple AI models. These changes make legacy modernization, package management, and automated workflows more accessible to developers, further refining the collaboration between human expertise and AI within real codebases.

Beyond Copilot, AI is now deeply integrated into operational systems at scale. Microsoft is advancing both open and proprietary large language models (LLMs), making it easier to deploy hybrid solutions and automate business workflows securely. Azure’s updates address container security, storage flexibility, and unified analytics, alongside improvements in distributed ML and interoperable data formats. New security protocols, supply chain protections, and practical DevSecOps patterns emphasize the need for well-governed and reliable expansion of AI technology. Below, we break down this week’s key developments in development, automation, and secure cloud transformation.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [Next-Generation AI Models and Enhanced Context Awareness](#next-generation-ai-models-and-enhanced-context-awareness)
  - [Deep IDE Integration and Context Protocol Expansion](#deep-ide-integration-and-context-protocol-expansion)
  - [Workflow Automation, Practical Upgrades, and Advanced Agent Modes](#workflow-automation-practical-upgrades-and-advanced-agent-modes)
  - [Reinventing Copilot UX: Web, Chat, and Spaces](#reinventing-copilot-ux-web-chat-and-spaces)
  - [Emergence of Formalized Agent Patterns and Custom Chat Modes](#emergence-of-formalized-agent-patterns-and-custom-chat-modes)
  - [Other GitHub Copilot News](#other-github-copilot-news)
- [AI](#ai)
  - [GPT-5 and Next-Generation Model Integration in the Microsoft Ecosystem](#gpt-5-and-next-generation-model-integration-in-the-microsoft-ecosystem)
  - [Democratizing and Securing Open-Source AI Models](#democratizing-and-securing-open-source-ai-models)
  - [Advancing Agentic AI: Orchestration, Standards, and No-Code Democratization](#advancing-agentic-ai-orchestration-standards-and-no-code-democratization)
  - [Democratizing AI-Driven Document and Data Intelligence](#democratizing-ai-driven-document-and-data-intelligence)
  - [Major Product Evolutions and Platform Realignments](#major-product-evolutions-and-platform-realignments)
  - [Other AI News](#other-ai-news)
- [ML](#ml)
  - [Scalable Optimization for Large AI Models](#scalable-optimization-for-large-ai-models)
  - [Production-Ready LLM Deployment on Azure](#production-ready-llm-deployment-on-azure)
  - [Unified Data and AI Workflows on Azure](#unified-data-and-ai-workflows-on-azure)
  - [Data Format Interoperability Expands with OneLake](#data-format-interoperability-expands-with-onelake)
  - [Other ML News](#other-ml-news)
- [Azure](#azure)
  - [Defense-in-Depth and Open Security for Azure Container Hosts](#defense-in-depth-and-open-security-for-azure-container-hosts)
  - [Evolving File Storage: Flexibility, Cost, and Performance Gains](#evolving-file-storage-flexibility-cost-and-performance-gains)
  - [Real-Time Data, Flexible Integration, and Observability Across Data Stacks](#real-time-data-flexible-integration-and-observability-across-data-stacks)
  - [Data Platform, Managed Services, and Migration Modernization](#data-platform-managed-services-and-migration-modernization)
  - [Infrastructure, Automation, and Unified Developer Experiences](#infrastructure-automation-and-unified-developer-experiences)
  - [Other Azure News](#other-azure-news)
- [Coding](#coding)
  - [.NET 10: Evolution in Web, Cross-Platform, and Language Capabilities](#net-10-evolution-in-web-cross-platform-and-language-capabilities)
  - [New Productivity Tools and Language Developments](#new-productivity-tools-and-language-developments)
  - [Modernizing .NET Workflows and Multi-Platform Development](#modernizing-net-workflows-and-multi-platform-development)
  - [Smarter Mapping and Data Handling with .NET](#smarter-mapping-and-data-handling-with-net)
  - [Expanding Python and Excel: Natively Analyzing Images](#expanding-python-and-excel-natively-analyzing-images)
  - [.NET in the Browser and Flexible Server Architectures](#net-in-the-browser-and-flexible-server-architectures)
  - [Other Coding News](#other-coding-news)
- [DevOps](#devops)
  - [Coding Agents and Workflow Automation Advance](#coding-agents-and-workflow-automation-advance)
  - [CI/CD Workflows Refined: From Azure Pipelines to Lessons in Enterprise Practice](#cicd-workflows-refined-from-azure-pipelines-to-lessons-in-enterprise-practice)
  - [GitHub Ecosystem: Compliance, Collaboration, and Automation Grows](#github-ecosystem-compliance-collaboration-and-automation-grows)
  - [Intelligence and Observability: Real-time Market and App Monitoring](#intelligence-and-observability-real-time-market-and-app-monitoring)
  - [Other DevOps News](#other-devops-news)
- [Security](#security)
  - [Securing the Open Source Supply Chain](#securing-the-open-source-supply-chain)
  - [Automating Vulnerability Management and Secret Protection](#automating-vulnerability-management-and-secret-protection)
  - [AI, LLMs, and Code Security Realities](#ai-llms-and-code-security-realities)
  - [Microsoft Security Copilot, AI Integration, and Operational Innovation](#microsoft-security-copilot-ai-integration-and-operational-innovation)
  - [Zero Trust, Forensics, and Incident Readiness in Cloud Environments](#zero-trust-forensics-and-incident-readiness-in-cloud-environments)
  - [Advanced Defenses for Hybrid and Enterprise Infrastructures](#advanced-defenses-for-hybrid-and-enterprise-infrastructures)
  - [Defensive Patterns for Web and Application Security](#defensive-patterns-for-web-and-application-security)
  - [Modernizing Security for Government, Compliance, and Sensitive Sectors](#modernizing-security-for-government-compliance-and-sensitive-sectors)
  - [Practical DevSecOps, Supply Chain, and Policy Controls](#practical-devsecops-supply-chain-and-policy-controls)
  - [Other Security News](#other-security-news)

## GitHub Copilot

GitHub Copilot advanced with previews of GPT-5 and Claude Opus 4.1, reinforcing AI-powered development for both individuals and teams. Key upgrades included improved models, new automation, deep IDE integration, and enhanced context handling, supporting better collaboration and more practical AI engineering. Copilot’s growing agent capabilities now cover package management, legacy code modernization, and industry frameworks.

### Next-Generation AI Models and Enhanced Context Awareness

Paid Copilot users can now try GPT-5 in public preview on major IDEs, bringing more intelligent and context-aware suggestions, such as understanding diagrams and visual UI prompts directly in the editor. GPT-5 Mini expands access to next-generation AI for free users as well, with customizable models including Claude Opus 4.1 available for all. This extends Copilot's multi-model offerings and broadens access to advanced code assistance.

- [GPT-5 Now Available in GitHub Copilot: Advanced Features and How to Enable]({{ "/2025-08-16-GPT-5-Now-Available-in-GitHub-Copilot-Advanced-Features-and-How-to-Enable.html" | relative_url }})
- [OpenAI GPT-5 Now Available to GitHub Copilot Users in Major IDEs](https://github.blog/changelog/2025-08-12-openai-gpt-5-is-now-available-in-public-preview-in-visual-studio-jetbrains-ides-xcode-and-eclipse)
- [GPT-5 Mini Launches in Public Preview for GitHub Copilot Users](https://github.blog/changelog/2025-08-13-gpt-5-mini-now-available-in-github-copilot-in-public-preview)
- [GPT-5 and Claude 4.1 Arrive in GitHub Copilot, TypeScript 5.9 Updates, and Community News]({{ "/2025-08-15-GPT-5-and-Claude-41-Arrive-in-GitHub-Copilot-TypeScript-59-Updates-and-Community-News.html" | relative_url }})

### Deep IDE Integration and Context Protocol Expansion

The Model Context Protocol (MCP) now supports JetBrains, Eclipse, and Xcode, enabling coordinated workflows and better automation across different IDEs. By open-sourcing the MCP server, GitHub has made it easier for developers to create custom integrations, supporting language-driven DevOps practices.

- [Model Context Protocol (MCP) Support for GitHub Copilot Now Available in JetBrains, Eclipse, and Xcode](https://github.blog/changelog/2025-08-13-model-context-protocol-mcp-support-for-jetbrains-eclipse-and-xcode-is-now-generally-available)
- [Why We Open Sourced Our MCP Server and What It Means for Developers](https://github.blog/open-source/maintainers/why-we-open-sourced-our-mcp-server-and-what-it-means-for-you/)

### Workflow Automation, Practical Upgrades, and Advanced Agent Modes

Copilot now includes automation features such as NuGet management through the MCP Server Preview, streamlining version control and dependency updates from within the IDE. Java and .NET developers can leverage AI-powered modernization tools, with demos showing Copilot automating code migrations and workflow tracking.

- [Announcing the NuGet MCP Server Preview: Real-Time NuGet Package Management with AI Integration](https://devblogs.microsoft.com/dotnet/nuget-mcp-server-preview/)
- [Modernizing Legacy Java Applications with GitHub Copilot App Modernization Upgrade](https://techcommunity.microsoft.com/t5/microsoft-developer-community/modernizing-legacy-java-project-using-github-copilot-app/ba-p/4440777)
- [Building a Game in 60 Seconds with GPT-5 in GitHub Copilot and MCP Server](https://github.blog/ai-and-ml/generative-ai/gpt-5-in-github-copilot-how-i-built-a-game-in-60-seconds/)
- [Modernizing and Upgrading Your .NET Apps with Visual Studio and Copilot-Powered AI Tools]({{ "/2025-08-14-Modernizing-and-Upgrading-Your-NET-Apps-with-Visual-Studio-and-Copilot-Powered-AI-Tools.html" | relative_url }})

### Reinventing Copilot UX: Web, Chat, and Spaces

Copilot supports full-repository conversations and easier PR generation through the AI Control Center on GitHub.com, resulting in more direct collaboration. Spaces allow teams to onboard and explore repositories more efficiently.

- [How to Chat with Your Repo & Create PRs with Copilot on GitHub]({{ "/2025-08-13-How-to-Chat-with-Your-Repo-and-Create-PRs-with-Copilot-on-GitHub.html" | relative_url }})
- [Copilot Spaces Now Support Adding Entire Repositories](https://github.blog/changelog/2025-08-13-add-repositories-to-spaces)

### Emergence of Formalized Agent Patterns and Custom Chat Modes

New Copilot chat modes, like the “Do Epic Shit” mode, support contract-based conversations with clear metadata for traceability. Improvements in prompt handling and automated TODO extraction further position Copilot as an accountable engineering partner.

- [Do Epic Shit Chat Mode: Beast Mode for GitHub Copilot](https://harrybin.de/posts/do-epic-shit-chat-mode/)

### Other GitHub Copilot News

VS Live! sessions highlighted Copilot’s integration with Visual Studio and Azure, demonstrating practical benefits for debugging and Git management. Tutorials and context-aware AI tools are now available for diverse frameworks. Admin capabilities have grown, adding management APIs and embedding secret protection into developer workflows. Text completion for PR descriptions is being phased out in favor of summary automation.

- [VS Live! Recap: Visual Studio, GitHub Copilot, and Azure AI Session Highlights](https://devblogs.microsoft.com/visualstudio/from-redmond-to-san-diego-vs-live-highlights-session-examples-and-whats-next/)
- [Speed Up API Integration with GitHub Copilot](https://pagelsr.github.io/CooknWithCopilot/blog/speed-up-api-integration.html)
- [Fix Broken Migrations with AI Debugging in VS Code Using GitHub Copilot](https://techcommunity.microsoft.com/t5/educator-developer-blog/fix-broken-migrations-with-ai-powered-debugging-in-vs-code-using/ba-p/4439418)
- [VS Code Live: Telerik & KendoUI AI Coding Assistants and Contextual AI Integration]({{ "/2025-08-14-VS-Code-Live-Telerik-and-KendoUI-AI-Coding-Assistants-and-Contextual-AI-Integration.html" | relative_url }})
- [GitHub Copilot User Management API Adds last_authenticated_at Field](https://github.blog/changelog/2025-08-13-added-last_authenticated_at-to-the-copilot-user-management-api)
- [What is GitHub Secret Protection?]({{ "/2025-08-17-What-is-GitHub-Secret-Protection-GitHub-Explained.html" | relative_url }})
- [GitHub Copilot Text Completion for Pull Request Descriptions to Be Deprecated](https://github.blog/changelog/2025-08-15-deprecating-copilot-text-completion-for-pull-request-descriptions)
- [Inconsistent Data Manipulation with Copilot in Excel: Allowed Once, Refused Later](https://techcommunity.microsoft.com/t5/microsoft-365-copilot/copilot-in-excel-performs-data-manipulation-once-and-then/m-p/4444281#M5471)

## AI

AI developments centered on making large language models and intelligent agents practical and broadly accessible. Microsoft’s adoption of GPT-5 across Copilot, VS Code, and Azure enables smarter automation and increases productivity, while new tools and compliance features are bringing open-source AI models to operational maturity.

### GPT-5 and Next-Generation Model Integration in the Microsoft Ecosystem

GPT-5 is now available in Copilot, VS Code AI Toolkit, Azure AI Foundry, and Copilot Studio, offering developers quick completions and more intelligent automation. SDKs and lightweight models ensure wider, more scalable adoption in both code and workflow automation.

- [GPT-5 Integrations for Microsoft Developers: GitHub Copilot, Azure AI, and VS Code](https://devblogs.microsoft.com/blog/gpt-5-for-microsoft-developers)
- [Stack Overflow Survey Reveals Developer Attitudes Toward AI Tools in 2025](https://devops.com/stack-overflow-survey-shows-ai-adoption-for-devs/?utm_source=rss&utm_medium=rss&utm_campaign=stack-overflow-survey-shows-ai-adoption-for-devs)
- [Evaluating GPT-5 Models for RAG on Azure AI Foundry](https://techcommunity.microsoft.com/t5/microsoft-developer-community/gpt-5-will-it-rag/ba-p/4442676)
- [Using GPT-5 with Azure AI Foundry, GitHub Copilot, and Copilot Studio in the Microsoft Ecosystem]({{ "/2025-08-13-Using-GPT-5-with-Azure-AI-Foundry-GitHub-Copilot-and-Copilot-Studio-in-the-Microsoft-Ecosystem.html" | relative_url }})
- [Model Mondays S2E9: Models for AI Agents](https://techcommunity.microsoft.com/t5/educator-developer-blog/model-mondays-s2e9-models-for-ai-agents/ba-p/4443162)
- [Using Model Router with GPT-5 Models in Azure AI Foundry]({{ "/2025-08-14-Using-Model-Router-with-GPT-5-Models-in-Azure-AI-Foundry.html" | relative_url }})
- [GPT-5 for Developers]({{ "/2025-08-14-GPT-5-for-Developers.html" | relative_url }})

### Democratizing and Securing Open-Source AI Models

Open-source LLMs like gpt-oss-20b/120b are now easier to deploy locally or on Azure, supporting hybrid use and third-party independence. Security features—including evaluation SDKs and reinforcement learning safeguards—are more robust, moving these tools toward broader enterprise adoption.

- [Building Applications Locally with gpt-oss-20b and the AI Toolkit for VS Code](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-application-with-gpt-oss-20b-with-ai-toolkit/ba-p/4441486)
- [Deploying Lightweight AI Apps on Azure App Service Using GPT-OSS-20B and Flask](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-lightweight-ai-apps-on-azure-app-service-with-gpt-oss-20b/ba-p/4442885)
- [Red-teaming a RAG Application with Azure AI Evaluation SDK](https://techcommunity.microsoft.com/t5/microsoft-developer-community/red-teaming-a-rag-app-with-the-azure-ai-evaluation-sdk/ba-p/4442682)

### Advancing Agentic AI: Orchestration, Standards, and No-Code Democratization

Agentic workflows are more accessible through modular templates, visual designers, and deeper IDE integration. MCP is becoming a key layer for secure, scalable context sharing, and Semantic Kernel helps orchestrate agents effectively.

- [Integrate Intelligent Agents with MCP and Azure AI Foundry on App Service](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/supercharge-your-app-service-apps-with-ai-foundry-agents/ba-p/4444310)
- [AI Agent's Toolbox: Building Intelligent Agents with Semantic Kernel, MCP Servers, and Python]({{ "/2025-08-15-AI-Agents-Toolbox-Building-Intelligent-Agents-with-Semantic-Kernel-MCP-Servers-and-Python.html" | relative_url }})
- [Boost Your Productivity with Visual Studio & Model Context Protocol (MCP) Servers]({{ "/2025-08-15-Boost-Your-Productivity-with-Visual-Studio-and-Model-Context-Protocol-MCP-Servers.html" | relative_url }})
- [Building AI Agents with Semantic Kernel, MCP Servers, and Python]({{ "/2025-08-15-Building-AI-Agents-with-Semantic-Kernel-MCP-Servers-and-Python.html" | relative_url }})
- [Agent Factory: Enterprise Patterns and Best Practices for Agentic AI with Azure AI Foundry](https://azure.microsoft.com/en-us/blog/agent-factory-the-new-era-of-agentic-ai-common-use-cases-and-design-patterns/)
- [Unlocking AI Interoperability with Model Context Protocol (MCP)]({{ "/2025-08-14-Unlocking-AI-Interoperability-with-Model-Context-Protocol-MCP.html" | relative_url }})
- [Introduction to Model Context Protocol (MCP) Servers: Building AI Integrations]({{ "/2025-08-14-Introduction-to-Model-Context-Protocol-MCP-Servers-Building-AI-Integrations.html" | relative_url }})
- [Exploring MCP Workflow for Database Management without SQL]({{ "/2025-08-12-Exploring-MCP-Workflow-for-Database-Management-without-SQL.html" | relative_url }})
- [Building AI Agents with Ease: Function Calling in VS Code AI Toolkit](https://techcommunity.microsoft.com/t5/educator-developer-blog/building-ai-agents-with-ease-function-calling-in-vs-code-ai/ba-p/4442637)
- [Creating an AI Policy Analysis Copilot: Exposing GraphRAG Insights with Azure, MCP, and Copilot Studio](https://techcommunity.microsoft.com/t5/public-sector-blog/creating-an-ai-policy-analysis-copilot/ba-p/4438393)
- [Building a Teams App with Azure Databricks Genie and Azure AI Agent Service](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/supercharge-data-intelligence-build-teams-app-with-azure/ba-p/4442653)

### Democratizing AI-Driven Document and Data Intelligence

Azure Document Intelligence now offers improved accuracy, more language support, and scalable execution. Mistral Document AI delivers serverless, multi-modal analytics with compliance in mind, supporting reproducible automation in business settings.

- [Advancements in Table Structure Recognition with Azure Document Intelligence](https://techcommunity.microsoft.com/t5/azure-ai-foundry-blog/unveiling-the-next-generation-of-table-structure-recognition/ba-p/4443684)
- [Mistral Document AI Launches on Azure AI Foundry: Seamless Document Intelligence at Scale](https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/deepening-our-partnership-with-mistral-ai-on-azure-ai-foundry/ba-p/4434656)

### Major Product Evolutions and Platform Realignments

Power Virtual Agents is now part of Copilot Studio, which extends natural language understanding and platform extensibility. GitHub’s closer alignment with Microsoft CoreAI reflects an integrated approach to developer tooling and AI.

- [Copilot Studio vs. Power Virtual Agents: What’s Changed?](https://dellenny.com/copilot-studio-vs-power-virtual-agents-whats-changed/)
- [GitHub CEO Steps Down as Microsoft Integrates GitHub with CoreAI Team](https://devops.com/github-ceo-to-step-down-as-company-is-more-tightly-embraced-by-microsofts-coreai-team/?utm_source=rss&utm_medium=rss&utm_campaign=github-ceo-to-step-down-as-company-is-more-tightly-embraced-by-microsofts-coreai-team)

### Other AI News

New guides, case studies, learning opportunities, and event announcements focus on operationalizing AI in the enterprise. Topics include SharePoint, Microsoft Fabric, skills programs, PII compliance, and open community engagement.

- [Build the Future of AI-Driven Apps with SharePoint Embedded](https://techcommunity.microsoft.com/t5/microsoft-sharepoint-blog/build-the-future-of-ai-driven-apps-with-sharepoint-embedded/ba-p/4442595)
- [How Adastra Used Microsoft Fabric and Azure OpenAI Service to Transform Heritage Grocers Group’s Data Analytics](https://techcommunity.microsoft.com/t5/partner-news/partner-case-study-adastra/ba-p/4442288)
- [Microsoft Fabric Community Conference 2025: Key Event for Data and AI Partners](https://techcommunity.microsoft.com/t5/partner-news/data-ai-and-opportunity-why-microsoft-partners-should-attend-the/ba-p/4443061)
- [Microsoft Learning Rooms Weekly Roundup – August 14, 2025](https://techcommunity.microsoft.com/t5/microsoft-learn/microsoft-learning-rooms-weekly-round-up-8-14/m-p/4443660#M17172)
- [MSLE Newsletter - August 2025: AI, Applied Skills, and Educator Community Updates](https://techcommunity.microsoft.com/t5/microsoft-learn-for-educators/msle-newsletter-august-2025/ba-p/4443034)
- [Future Skills Organisation and Microsoft Launch Nationwide AI Skills Accelerator in Australia](https://news.microsoft.com/source/asia/features/fso-microsoft-skills-accelerator-ai/)
- [Announcing the August Preview Model for PII Redaction in Azure AI Language](https://techcommunity.microsoft.com/t5/azure-ai-foundry-blog/announcing-the-text-pii-august-preview-model-release-in-azure-ai/ba-p/4441705)
- [Azure Logic App AI-Powered Monitoring Solution: Automate, Analyze, and Act on Your Azure Data](https://techcommunity.microsoft.com/t5/healthcare-and-life-sciences/azure-logic-app-ai-powered-monitoring-solution-automate-analyze/ba-p/4442665)
- [Navigating AI Adoption: Legal Considerations Every Organization Should Know](https://techcommunity.microsoft.com/t5/public-sector-blog/navigating-ai-adoption-legal-considerations-every-organization/ba-p/4442164)
- [MSBuild 2025 Highlights: Open Sourcing WSL and Windows AI Foundry]({{ "/2025-08-14-MSBuild-2025-Highlights-Open-Sourcing-WSL-and-Windows-AI-Foundry.html" | relative_url }})
- [How Microsoft Semantic Kernel Transforms Proven Workflows into Intelligent Agents](https://techcommunity.microsoft.com/t5/educator-developer-blog/how-microsoft-semantic-kernel-transforms-proven-workflows-into/ba-p/4434731)
- [Logic Apps Community Day 2025: Call for Papers and AI Integration Themes](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-community-day-2025/ba-p/4442668)
- [Customer interaction scenarios: AI receptionists, Copilot Studio chatbots, and more.](https://dellenny.com/top-5-use-cases-for-copilot-studio-in-your-business/)
- [No-Code AI: Building Chatbots with Copilot Studio for Non-Developers](https://dellenny.com/no-code-ai-how-non-developers-can-build-smart-chatbots-with-copilot-studio/)
- [Azure AI Blogs Consolidate into New Azure AI Foundry Blog](https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/exciting-news-azure-ai-blogs-have-come-together-in-the-new-azure/ba-p/4443002)
- [September 2025: Microsoft Hero Event Calendar](https://techcommunity.microsoft.com/t5/blog/september-calendar-is-here/ba-p/4444610)
- [Vote for the Next Deep-Dive: Azure AI, Copilot, Vector Databases, or Voice APIs](https://techcommunity.microsoft.com/t5/discussion/new-community-poll/m-p/4442257#M11)
- [Copado Enhances AI Tools to Uncover Salesforce Code Relationships](https://devops.com/copado-extends-ai-reach-to-surface-relationships-between-salesforce-code/?utm_source=rss&utm_medium=rss&utm_campaign=copado-extends-ai-reach-to-surface-relationships-between-salesforce-code)
- [Sentry Integrates MCP Server Monitoring into APM Platform for AI Workflows](https://devops.com/sentry-adds-tool-for-monitoring-mcp-servers-to-apm-platform/?utm_source=rss&utm_medium=rss&utm_campaign=sentry-adds-tool-for-monitoring-mcp-servers-to-apm-platform)
- [Detect Human Faces and Compare Similar Ones with Azure Face API](https://dellenny.com/detect-human-faces-and-compare-similar-ones-with-face-api-in-azure/)
- [Unlocking the Power of AI with Azure Cognitive Services](https://dellenny.com/unlocking-the-power-of-ai-with-azure-cognitive-services/)
- [Extracting Page Numbers from PDFs with Azure AI Search and OCR](https://techcommunity.microsoft.com/t5/azure-paas-blog/finding-the-right-page-number-in-pdfs-with-ai-search/ba-p/4440758)
- [Data Intelligence at Your Fingertips: Fabric’s AI Functions & Data Agents](https://techcommunity.microsoft.com/t5/events/data-intelligence-at-your-fingertips-fabric-s-ai-functions-data/ec-p/4443431#M10)
- [Browser Automation With Azure AI Foundry's Automation Tool and Playwright Workspaces]({{ "/2025-08-14-Browser-Automation-With-Azure-AI-Foundrys-Automation-Tool-and-Playwright-Workspaces.html" | relative_url }})
- [Empowering.Cloud Community Update – September 2025](https://techcommunity.microsoft.com/t5/microsoft-teams-community-blog/empowering-cloud-community-update-september-2025/ba-p/4444233)
- [Implementing a Center of Excellence for Generative AI](https://www.thomasmaurer.ch/2025/08/implementing-a-center-of-excellence-for-generative-ai/)
- [Generative AI for Permitting: Accelerating Clean Energy with Microsoft](https://www.microsoft.com/en-us/garage/wall-of-fame/generative-ai-for-permitting/)
- [Build Next-Gen AI Apps with .NET and Azure]({{ "/2025-08-14-Build-Next-Gen-AI-Apps-with-NET-and-Azure.html" | relative_url }})
- [Q1 2025 GitHub Innovation Graph Update: Trends in Data Visualization and AI Development](https://github.blog/news-insights/policy-news-and-insights/q1-2025-innovation-graph-update-bar-chart-races-data-visualization-on-the-rise-and-key-research/)
- [How Microsoft Defender Experts Uses AI to Cut Through the Noise](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/how-microsoft-defender-experts-uses-ai-to-cut-through-the-noise/ba-p/4443601)
- [The Right Kind of AI for Infrastructure as Code](https://devops.com/the-right-kind-of-ai-for-infrastructure-as-code)
- [Designing Empathetic AI Experiences: Trish Winter-Hunt on Content Design and Azure AI Foundry]({{ "/2025-08-12-Designing-Empathetic-AI-Experiences-Trish-Winter-Hunt-on-Content-Design-and-Azure-AI-Foundry.html" | relative_url }})

## ML

Recent ML releases include advances in optimization for training larger models, scalable open-source LLM deployment on Azure, improved data interoperability, and practical guides for optimizing analytics workflows.

### Scalable Optimization for Large AI Models

Microsoft announced the Dion optimizer, which lowers the cost and time to train large models by focusing on the most impactful update vectors. Early benchmarks on LLaMA-3 405B show increased training performance using familiar tools.

- [Microsoft Releases Dion: A New Scalable Optimizer for Training AI Models](https://www.microsoft.com/en-us/research/blog/dion-the-distributed-orthonormal-update-revolution-is-here/)

### Production-Ready LLM Deployment on Azure

GPT-OSS-20B can be deployed on Azure AKS with KAITO and vLLM, following recently published guides that detail deployment strategies and resource management for flexible enterprise LLM endpoints.

- [Deploying OpenAI’s GPT-OSS-20B on Azure AKS with KAITO and vLLM](https://techcommunity.microsoft.com/t5/ai-machine-learning-blog/deploying-openai-s-first-open-source-model-on-azure-aks-with/ba-p/4444234)

### Unified Data and AI Workflows on Azure

Azure Databricks continues to serve as the foundational platform for unified AI and analytics, offering built-in security, compliance, and onboarding resources for efficient deployment across teams.

- [Supercharge Data and AI Innovation with Azure Databricks]({{ "/2025-08-12-Supercharge-Data-and-AI-Innovation-with-Azure-Databricks.html" | relative_url }})

### Data Format Interoperability Expands with OneLake

Microsoft’s OneLake now enables cross-format queries between Delta and Iceberg using Apache XTable, allowing multiple analytics tools to operate seamlessly on shared data.

- [How Microsoft OneLake Seamlessly Provides Apache Iceberg Support for All Fabric Data](https://blog.fabric.microsoft.com/en-US/blog/how-to-access-your-microsoft-fabric-tables-in-apache-iceberg-format/)

### Other ML News

Guides on Spark UI help users optimize distributed jobs, while retrospectives on Excel at 40 showcase the evolution toward more robust analytics and Python integration.

- [A Deep Dive into Spark UI for Job Optimization](https://techcommunity.microsoft.com/t5/microsoft-mission-critical-blog/a-deep-dive-into-spark-ui-for-job-optimization/ba-p/4442229)
- [Excel at 40 Week 1: Days 1–3](https://techcommunity.microsoft.com/t5/excel/excel-at-40-week-1-days-1-3/m-p/4443674#M254078)

## Azure

Azure continued to enhance platform flexibility, security, integration, and migration support, further supporting both legacy and cloud-native projects.

### Defense-in-Depth and Open Security for Azure Container Hosts

Azure Linux with OS Guard brings mandatory access controls and strong code integrity to AKS containers. This release enforces the use of trusted binaries, incorporates SELinux and SBOMs, and offers robust compliance and open-source build tools for secure deployment at scale.

- [Azure Linux with OS Guard: Enhancing Container Host Security with Code Integrity and Open Source Transparency](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-with-os-guard-immutable-container-host-with-code/ba-p/4437473)

### Evolving File Storage: Flexibility, Cost, and Performance Gains

Azure Files Provisioned v2 now allows separate control over storage and performance, leading to predictable costs and rapid scalability for a variety of workloads.

- [Unlocking Flexibility with Azure Files Provisioned V2](https://techcommunity.microsoft.com/t5/itops-talk-blog/unlocking-flexibility-with-azure-files-provisioned-v2/ba-p/4443628)
- [Lower Costs and Boost Flexibility with Azure Files Provisioned v2 for SSD](https://techcommunity.microsoft.com/t5/azure-storage-blog/lower-costs-and-boost-flexibility-with-azure-files-provisioned/ba-p/4443621)

### Real-Time Data, Flexible Integration, and Observability Across Data Stacks

Azure Databricks now connects to the Power Platform for operational write-back from Power BI. Oracle DB and Exadata on Azure improve observability by integrating with Azure Monitor, and Micronaut Java apps can now use zero-code OpenTelemetry instrumentation.

- [Interactive Write-back from Power BI to Azure Databricks with Power Platform Connector](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/closing-the-loop-interactive-write-back-from-power-bi-to-azure/ba-p/4442999)
- [Expanding Global Reach and Enhanced Observability with Oracle Database@Azure](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/expanding-global-reach-and-enhancing-observability-with-oracle/ba-p/4443650)
- [Send Traces and Metrics from Micronaut Apps to Azure Monitor with Zero-Code Instrumentation](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-signals-from-micronaut-applications-to-azure-monitor/ba-p/4443884)

### Data Platform, Managed Services, and Migration Modernization

Updates include faster log rates in Azure SQL Managed Instance, more efficient SAP ASE-to-Azure SQL migrations, and extended support for MySQL and PostgreSQL—all improvements aimed at resilience and easier upgrades.

- [Higher Log Rate Enhancement in Azure SQL Managed Instance's Business Critical Tier](https://techcommunity.microsoft.com/t5/azure-sql-blog/higher-log-rate-for-business-critical-service-tier-in-azure-sql/ba-p/4444127)
- [Accelerating SAP Sybase ASE to Azure SQL Migration Using SSMA and Parallel BCP](https://techcommunity.microsoft.com/t5/modernization-best-practices-and/sap-sybase-ase-to-azure-sql-migration-using-ssma-and-bcp/ba-p/4436624)
- [Extended Support for Azure Database for MySQL: What You Need to Know](https://techcommunity.microsoft.com/t5/azure-database-for-mysql-blog/announcing-extended-support-for-azure-database-for-mysql/ba-p/4442924)
- [Azure PostgreSQL Extended Support: Stay Secure at Every Upgrade Stage](https://techcommunity.microsoft.com/t5/azure-database-for-postgresql/azure-postgresql-extended-support-stay-secure-at-every-stage-of/ba-p/4442283)

### Infrastructure, Automation, and Unified Developer Experiences

Announced previews for the Terraform MSGraph provider and new VS Code extensions now streamline Azure automation and API management. Hybrid management is simpler with agent upgrades and regional expansion of Arc-enabled SQL Server.

- [Announcing Public Preview of the Terraform MSGraph Provider and Microsoft Terraform VSCode Extension](https://techcommunity.microsoft.com/t5/azure-tools-blog/announcing-msgraph-provider-public-preview-and-the-microsoft/ba-p/4443614)
- [Public Preview: Auto Agent Upgrade for Azure Arc-Enabled Servers](https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-auto-agent-upgrade-for-azure-arc-enabled-servers/ba-p/4442556)
- [Azure Arc-Enabled SQL Server Now Generally Available in US Government Virginia Region](https://techcommunity.microsoft.com/t5/azure-arc-blog/sql-server-enabled-by-azure-arc-is-now-generally-available-in/ba-p/4443077)

### Other Azure News

Highlights include end-to-end Playwright testing, enhanced Fabric workspace security with customer-managed keys, streamlined connectivity, and regional expansion of PostgreSQL. The platform also improved cost transparency, added more logging from native images, and released practical troubleshooting resources.

- [Azure Update - 15th August 2025]({{ "/2025-08-15-Azure-Update-15th-August-2025.html" | relative_url }})
- [Private Pod Subnets in AKS Without Overlay Networking](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/private-pod-subnets-in-aks-without-overlay-networking/ba-p/4442510)
- [Microsoft Recognized as a Leader in the 2025 Gartner Magic Quadrant for Container Management](https://azure.microsoft.com/en-us/blog/microsoft-is-a-leader-in-the-2025-gartner-magic-quadrant-for-container-management/)
- [Windows Server Datacenter: Azure Edition Preview Build 26461 in Azure](https://techcommunity.microsoft.com/t5/windows-server-insiders/windows-server-datacenter-azure-edition-preview-build-26461-now/m-p/4442961#M4197)
- [New Offerings in Azure Marketplace: July 23-31, 2025](https://techcommunity.microsoft.com/t5/marketplace-blog/new-in-azure-marketplace-july-23-31-2025/ba-p/4431277)
- [End-to-End Azure App Testing with Playwright Workspaces: Local and Cloud Workflows](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-app-testing-playwright-workspaces-for-local-to-cloud-test/ba-p/4442711)
- [Sending Logs from Micronaut Native Image Applications to Azure Monitor](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-logs-from-micronaut-native-image-applications-to-azure/ba-p/4443867)
- [Send Traces from Micronaut Native Image Applications to Azure Monitor](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-traces-from-micronaut-native-image-applications-to-azure/ba-p/4443791)
- [Customer-Managed Keys for Microsoft Fabric Workspaces Now in Public Preview](https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-for-fabric-workspaces-available-in-all-public-regions-now-preview/)
- [Introducing Support for Workspace Identity Authentication in Fabric Connectors](https://blog.fabric.microsoft.com/en-US/blog/announcing-support-for-workspace-identity-authentication-in-new-fabric-connectors-and-for-dataflow-gen2/)
- [Enhancements to Microsoft Fabric Copy Job: Reset Incremental Copy, Auto Table Creation, and JSON Support](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support/)
- [Simplified OneLake Capacity Costs: Updated Proxy Consumption Rates in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/onelake-costs-simplified-lowering-capacity-utilization-when-accessing-onelake/)
- [General Availability: Enhanced Data Mapper Experience in Logic Apps (Standard)](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/general-availability-enhanced-data-mapper-experience-in-logic/ba-p/4442296)
- [Azure API Management Workspaces Breaking Changes Update: Built-in Gateway & Tier Support](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/update-to-api-management-workspaces-breaking-changes-built-in/ba-p/4443668)
- [API Gateway Pattern in Azure: Managing APIs and Routing Requests to Microservices](https://dellenny.com/api-gateway-pattern-in-azure-managing-apis-and-routing-requests-to-microservices/)
- [Troubleshooting OAuth2 API Token Retrieval with ADF Web Activity](https://techcommunity.microsoft.com/t5/azure-data-factory/getting-an-oauth2-api-access-token-using-client-id-and-client/m-p/4443568#M936)
- [Partial Updates in MongoDB via Azure Data Factory Data Flow: Nested Field Modification](https://techcommunity.microsoft.com/t5/azure-data-factory/help-with-partial-mongodb-update-via-azure-data-factory-data/m-p/4443596#M937)
- [Microsoft Finland: Monthly Community Series for Software Companies – 2025 Conferences](https://techcommunity.microsoft.com/t5/kumppanifoorumi/microsoft-finland-software-developing-companies-monthly/ba-p/4442900)
- [Transactable Partner Solutions: Apptividad and CoreView in Azure Marketplace](https://techcommunity.microsoft.com/t5/marketplace-blog/apptividad-and-coreview-offer-transactable-partner-solutions-in/ba-p/4431278)
- [Exploring Microsoft Intune: Manage and Secure your Devices and Apps](https://techcommunity.microsoft.com/t5/events/exploring-microsoft-intune-manage-and-secure-your-devices-and/ec-p/4441982#M9)
- [Gpresult-Like Tool for Intune Policy Troubleshooting](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/gpresult-like-tool-for-intune/ba-p/4437008)
- [Using Entra ID Authentication with Arc-Enabled SQL Server in a .NET Windows Forms Application](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/using-entra-id-authentication-with-arc-enabled-sql-server-in-a/ba-p/4435168)
- [Troubleshooting MCC Phantom Install Issues on Windows Server 2022 with WSL](https://techcommunity.microsoft.com/t5/microsoft-connected-cache-for/mcc-phantom-install/m-p/4444201#M108)
- [How Azure Storage Powers AI Workloads: Behind the Scenes with OpenAI, Blobfuse & More](https://techcommunity.microsoft.com/t5/itops-talk-blog/how-azure-storage-powers-ai-workloads-behind-the-scenes-with/ba-p/4442540)
- [SQL Server on Linux Now Supports cgroup v2](https://techcommunity.microsoft.com/t5/sql-server-blog/sql-server-on-linux-now-supports-cgroup-v2/ba-p/4433523)
- [Troubleshooting Azure Stack HCI Local Cluster Deployment: Network Configuration Error](https://techcommunity.microsoft.com/t5/azure-stack/error-no-file/m-p/4443115#M277)
- [Load Data from Network-Protected Azure Storage Accounts to Microsoft OneLake with AzCopy](https://blog.fabric.microsoft.com/en-US/blog/load-data-from-network-protected-azure-storage-accounts-to-microsoft-onelake-with-azcopy/)

## Coding

This week’s coding updates include .NET 10 previews, Python integration with Excel for data analysis, and new productivity features in tooling. Community-driven libraries and improved language features further support efficient development across platforms.

### .NET 10: Evolution in Web, Cross-Platform, and Language Capabilities

.NET 10 Preview 7 adds deeper AI features in ASP.NET Core and Blazor, passwordless authentication, better diagnostics, enhanced libraries (like PipeReader), and security improvements. Updates to MAUI, EF Core, minimal APIs, and WinForms simplify cloud-native and multi-platform app development.

- [The Future of Web Development with ASP.NET Core & Blazor in .NET 10]({{ "/2025-08-14-The-Future-of-Web-Development-with-ASPNET-Core-and-Blazor-in-NET-10.html" | relative_url }})
- [.NET 10 Preview 7 Released: Key Updates for Libraries, ASP.NET Core, Blazor, and MAUI](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-7/)

### New Productivity Tools and Language Developments

VS Code introduces "Beast mode" for more productive navigation. C# 14 improves pattern matching, null safety, and language clarity for maintainable code.

- [VS Code Beast Mode Explained: Features and Usage]({{ "/2025-08-14-VS-Code-Beast-Mode-Explained-Features-and-Usage.html" | relative_url }})
- [Highlights and Upcoming Features in C#: A Deep Dive into C# 14]({{ "/2025-08-14-Highlights-and-Upcoming-Features-in-C-A-Deep-Dive-into-C-14.html" | relative_url }})

### Modernizing .NET Workflows and Multi-Platform Development

.NET Aspire provides step-by-step guidance for building and deploying robust application systems. .NET MAUI and Visual Studio unify development for mobile and desktop applications, simplifying high-performance, cross-platform builds.

- [Building Confident Application Systems with .NET Aspire: From Dev to Deployment]({{ "/2025-08-14-Building-Confident-Application-Systems-with-NET-Aspire-From-Dev-to-Deployment.html" | relative_url }})
- [Building Mobile and Desktop Apps with Visual Studio and .NET MAUI]({{ "/2025-08-14-Building-Mobile-and-Desktop-Apps-with-Visual-Studio-and-NET-MAUI.html" | relative_url }})

### Smarter Mapping and Data Handling with .NET

The Facet library introduces compile-time LINQ-based data projection, helping developers avoid common run-time mapping errors and improving data transformation reliability.

- [Enhancing .NET Code: Using Facet Instead of Traditional Mapping]({{ "/2025-08-13-Enhancing-NET-Code-Using-Facet-Instead-of-Traditional-Mapping.html" | relative_url }})

### Expanding Python and Excel: Natively Analyzing Images

Support for Python in Excel now extends to image analysis tasks such as blur detection and metadata extraction, making it easier for analysts to combine visual and tabular data.

- [Analyzing Images with Python in Excel: Now Natively Supported](https://techcommunity.microsoft.com/t5/microsoft-365-insider-blog/analyze-images-with-python-in-excel/ba-p/4440388)

### .NET in the Browser and Flexible Server Architectures

Developers can now run .NET in the browser without Blazor using WASM templates, increasing platform flexibility. Tutorials show how to build environment-flexible integrations using dual-transport MCP servers.

- [Running .NET in the Browser Without Blazor Using WASM](https://andrewlock.net/running-dotnet-in-the-browser-without-blazor/)
- [Building a Dual-Transport MCP Server with .NET: STDIO and HTTP Support](https://techcommunity.microsoft.com/t5/microsoft-developer-community/one-mcp-server-two-transports-stdio-and-http/ba-p/4443915)

### Other Coding News

Scripts for disk usage analysis in PowerShell and reliability improvements in GitHub Spark strengthen dev workflows and infrastructure maintenance.

- [Finding Large Directories and Recovering Lost Disk Space with PowerShell](https://techcommunity.microsoft.com/t5/windows-powershell/how-to-finding-large-directories-recovering-lost-space/m-p/4442877#M9117)
- [Spark Resilience Improvements Enhance Reliability and Iteration Experience](https://github.blog/changelog/2025-08-13-spark-resilience-improvements)

## DevOps

DevOps practices continue to evolve through automation, integrated AI agents, improved CI/CD designs, and practical governance updates across GitHub and Azure.

### Coding Agents and Workflow Automation Advance

Tools like Gemini CLI and Shadow AI bring agent-driven automation to GitHub workflows and DevOps pipelines—enabling secure, scriptable repo automation and basic CI/CD management.

- [How Gemini CLI GitHub Actions is Changing Developer Workflows](https://devops.com/how-gemini-cli-github-actions-is-changing-developer-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=how-gemini-cli-github-actions-is-changing-developer-workflows)
- [Shadow: How AI Coding Agents are Transforming DevOps Workflows](https://devops.com/shadow-how-ai-coding-agents-are-transforming-devops-workflows/?utm_source=rss&utm_medium=rss&utm_campaign=shadow-how-ai-coding-agents-are-transforming-devops-workflows)

### CI/CD Workflows Refined: From Azure Pipelines to Lessons in Enterprise Practice

Guides for CI/CD using Azure Developer CLI and YAML cover topics from multi-stage deployment to Copilot-powered troubleshooting. Field reports underscore the value of systematic engineering, emphasizing proactive monitoring and recovery.

- [Azure Developer CLI: Dev to Production with Azure DevOps Pipelines](https://devblogs.microsoft.com/devops/azure-developer-cli-from-dev-to-prod-with-azure-devops-pipelines/)
- [From Firefighting to Forward-Thinking: Real-World Lessons in DevOps and Cloud Engineering](https://devops.com/from-firefighting-to-forward-thinking-my-real-world-lessons-in-devops-and-cloud-engineering/?utm_source=rss&utm_medium=rss&utm_campaign=from-firefighting-to-forward-thinking-my-real-world-lessons-in-devops-and-cloud-engineering)

### GitHub Ecosystem: Compliance, Collaboration, and Automation Grows

Following a recent GitHub Enterprise Importer outage, new controls, billing mechanisms, and API enhancements improve compliance and monitoring for businesses using GitHub services.

- [GitHub Enterprise Importer Incident and IP Range Update: July 2025 Availability Report](https://github.blog/news-insights/company-news/github-availability-report-july-2025/)
- [Metered GitHub Enterprise Billing Now Available for Visual Studio Subscribers](https://github.blog/changelog/2025-08-14-introducing-metered-github-enterprise-billing-for-visual-studio-subscriptions-with-github-enterprise)

### Intelligence and Observability: Real-time Market and App Monitoring

AI-driven products like Futurum Signal and AppSignal’s OpenTelemetry support are now mainstream, making market and app monitoring more immediate and actionable.

- [Futurum Signal: AI-Powered Market Intelligence for DevOps and Platform Engineering](https://devops.com/futurum-signal-ai-powered-market-intelligence-for-devops-and-platform-engineering/?utm_source=rss&utm_medium=rss&utm_campaign=futurum-signal-ai-powered-market-intelligence-for-devops-and-platform-engineering)
- [AppSignal Adds Native OpenTelemetry Support for Enhanced Application Monitoring](https://devops.com/appsignal-adds-opentelemetry-support-to-monitoring-platform/?utm_source=rss&utm_medium=rss&utm_campaign=appsignal-adds-opentelemetry-support-to-monitoring-platform)

### Other DevOps News

Dependabot now automates more package management through vcpkg. GitHub added new features to improve reviewer status tracking and file attachments. There are also updated guides for open source migration and mobile app release workflows.

- [Dependabot Adds Version Update Support for vcpkg](https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg)
- [Clearer Pull Request Reviewer Status and Enhanced Email Filtering in GitHub](https://github.blog/changelog/2025-08-14-clearer-pull-request-reviewer-status-and-enhanced-email-filtering)
- [Expanded File Type Support for GitHub Attachments](https://github.blog/changelog/2025-08-13-expanded-file-type-support-for-attachments-across-issues-pull-requests-and-discussions)
- [How the International Telecommunication Union Open Sourced Its Tech: A Four-Step Guide](https://github.blog/open-source/social-impact/from-private-to-public-how-a-united-nations-organization-open-sourced-its-tech-in-four-steps/)
- [Survey Reveals Major Challenges in Mobile Application Release Management](https://devops.com/survey-surfaces-multiple-mobile-application-release-management-headaches/?utm_source=rss&utm_medium=rss&utm_campaign=survey-surfaces-multiple-mobile-application-release-management-headaches)
- [Persistent Visual Studio Enterprise Access Level in Azure DevOps After License Removal](https://techcommunity.microsoft.com/t5/azure/unable-to-revert-azure-devops-user-access-level/m-p/4442871#M22102)

## Security

This week in security highlighted supply chain improvements, expanded secret and vulnerability management, practical AI risk insights, and operational readiness in both hybrid and government settings.

### Securing the Open Source Supply Chain

GitHub’s Secure Open Source Fund supported 71 projects, helping to fix over 1,100 vulnerabilities and publish more than 50 CVEs. These efforts expanded best practices through AI-powered tools and education.

- [Securing the Open Source Supply Chain: Impact of the GitHub Secure Open Source Fund](https://github.blog/open-source/maintainers/securing-the-supply-chain-at-scale-starting-with-71-important-open-source-projects/)

### Automating Vulnerability Management and Secret Protection

Improvements in secret scanning and push protection for MCP servers, as well as more robust token validation for Azure DevOps and GitHub, continue to strengthen software supply chain hygiene.

- [GitHub MCP Server Enhances Secret Scanning and Push Protection for Public Repositories](https://github.blog/changelog/2025-08-13-github-mcp-server-secret-scanning-push-protection-and-more)
- [Secret Scanning Expands Support: 12 New Token Validators Added to GitHub](https://github.blog/changelog/2025-08-12-secret-scanning-adds-12-validators-including-cockroach-labs-polar-and-yandex)
- [Secret Validity Checks Launch in GitHub Advanced Security for Azure DevOps](https://devblogs.microsoft.com/devops/hunting-living-secrets-secret-validity-checks-arrive-in-github-advanced-security-for-azure-devops/)
- [Azure DevOps Improves OAuth Client Secret Security: Secrets Now Shown Only Once](https://devblogs.microsoft.com/devops/azure-devops-oauth-client-secrets-now-shown-only-once/)

### AI, LLMs, and Code Security Realities

Research from SonarSource and recent survey data highlight ongoing risks from using LLMs to generate code, underlining the continued need for manual review and security scanning.

- [SonarSource Research Highlights Security Risks in LLM-Generated Code](https://devops.com/sonar-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/?utm_source=rss&utm_medium=rss&utm_campaign=sonar-surfaces-multiple-caveats-when-relying-on-llms-to-write-code)
- [Most Organizations Face Breaches Caused by Vulnerable Code, Survey Finds](https://devops.com/survey-traces-large-amount-of-breaches-back-to-vulnerable-code/?utm_source=rss&utm_medium=rss&utm_campaign=survey-traces-large-amount-of-breaches-back-to-vulnerable-code)
- [SonarSource Highlights Security Risks and Code Quality Issues in LLM-Generated Code](https://devops.com/sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/?utm_source=rss&utm_medium=rss&utm_campaign=sonarsource-surfaces-multiple-caveats-when-relying-on-llms-to-write-code)

### Microsoft Security Copilot, AI Integration, and Operational Innovation

Microsoft Security Copilot’s latest updates introduce support for Intune, Entra, and Defender, as well as new workflows for phishing and natural language automation, helping security teams streamline operations.

- [What’s New in Microsoft Security Copilot: AI-Powered Security Innovations for IT and Security Teams](https://techcommunity.microsoft.com/t5/microsoft-security-copilot-blog/what-s-new-in-microsoft-security-copilot/ba-p/4442220)
- [How Dow Uses Microsoft Security Copilot and AI to Transform Cybersecurity Operations](https://www.microsoft.com/en-us/security/blog/2025/08/12/dows-125-year-legacy-innovating-with-ai-to-secure-a-long-future/)

### Zero Trust, Forensics, and Incident Readiness in Cloud Environments

New guidance covers essential logging, secure key management, and automatable incident response to help organizations prepare for potential cloud breaches.

- [Cloud Forensics: Implementing Security Baselines for Forensic Readiness in Microsoft Azure](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/cloud-forensics-prepare-for-the-worst-implement-security/ba-p/4440310)

### Advanced Defenses for Hybrid and Enterprise Infrastructures

Microsoft released key updates and guidance on addressing privilege escalations, shifting organizations toward Entra ID trust, and strengthening defenses against threats such as BitLocker vulnerabilities.

- [Mitigating CVE-2025-53786: Hybrid Exchange Server Privilege Escalation with MDVM](https://techcommunity.microsoft.com/t5/microsoft-defender-vulnerability/mdvm-guidance-for-cve-2025-53786-exchange-hybrid-privilege/ba-p/4442337)
- [August 2025 Exchange Server Security Updates Released](https://techcommunity.microsoft.com/t5/exchange-team-blog/released-august-2025-exchange-server-security-updates/ba-p/4441596)
- [Security Update Available for SQL Server 2019 RTM GDR](https://techcommunity.microsoft.com/t5/sql-server-blog/security-update-for-sql-server-2019-rtm-gdr/ba-p/4441689)
- [Security Update Available for SQL Server 2022 RTM GDR](https://techcommunity.microsoft.com/t5/sql-server-blog/security-update-for-sql-server-2022-rtm-gdr/ba-p/4441687)
- [BitUnlocker: Leveraging Windows Recovery to Extract BitLocker Secrets](https://techcommunity.microsoft.com/t5/microsoft-security-community/bitunlocker-leveraging-windows-recovery-to-extract-bitlocker/ba-p/4442806)

### Defensive Patterns for Web and Application Security

New guidance is available for SharePoint RCE prevention, S/MIME troubleshooting, and Platform SSO for macOS with Entra ID, helping organizations secure their web, identity, and app environments.

- [Mitigating SharePoint CVE-2025-53770 Using Azure Web Application Firewall](https://techcommunity.microsoft.com/t5/azure-network-security-blog/protect-against-sharepoint-cve-2025-53770-with-azure-web/ba-p/4442050)
- [Troubleshooting S/MIME Setup in Exchange Online and M365: OWA and Outlook Certificate Issues](https://techcommunity.microsoft.com/t5/exchange/smime-not-working-in-owa/m-p/4443230#M16650)
- [General Availability: Platform SSO for macOS with Microsoft Entra ID](https://techcommunity.microsoft.com/t5/microsoft-entra-blog/now-generally-available-platform-sso-for-macos-with-microsoft/ba-p/4437424)

### Modernizing Security for Government, Compliance, and Sensitive Sectors

Defender for Cloud expands protection for government cloud, including agentless options and malware scanning for sensitive environments. Case studies show public sector organizations leveraging these tools to improve cybersecurity.

- [Microsoft Defender for Cloud Expands Security and Compliance Features for U.S. Government Cloud](https://techcommunity.microsoft.com/t5/microsoft-defender-for-cloud/microsoft-defender-for-cloud-expands-u-s-gov-cloud-support-for/ba-p/4441118)
- [Malware Scanning Now Available for Azure Government Secret and Top-Secret Clouds](https://techcommunity.microsoft.com/t5/microsoft-defender-for-cloud/malware-scanning-add-on-is-now-generally-available-in-azure-gov/ba-p/4442502)
- [Queensland Government Enhances Cybersecurity for Vulnerable Communities with Microsoft 365 E5](https://news.microsoft.com/source/asia/2025/08/14/championing-safety-how-one-queensland-government-department-is-transforming-cybersecurity-to-better-support-vulnerable-communities/)

### Practical DevSecOps, Supply Chain, and Policy Controls

Features like managed image VEX support, CodeQL’s expanded static analysis, a new CRA toolkit, and improved GitHub Actions policies provide concrete tools to help teams defend against supply chain attacks.

- [Minimus Adds VEX Support and Microsoft SSO Integration to Hardened Images Service](https://devops.com/minimus-adds-vex-support-to-managed-hardened-images-service/?utm_source=rss&utm_medium=rss&utm_campaign=minimus-adds-vex-support-to-managed-hardened-images-service)
- [CodeQL Expands Support for Kotlin and Improves Static Analysis Accuracy](https://github.blog/changelog/2025-08-14-codeql-expands-kotlin-support-and-additional-accuracy-improvements)
- [Eclipse Foundation Publishes Toolkit to Simplify CRA Compliance](https://devops.com/eclipse-foundation-publishes-toolkit-to-simplify-cra-compliance/?utm_source=rss&utm_medium=rss&utm_campaign=eclipse-foundation-publishes-toolkit-to-simplify-cra-compliance)
- [GitHub Actions Policy Adds Blocking and SHA Pinning for Enhanced Security](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions)

### Other Security News

Additional updates include real-time access evaluation in Azure DevOps, improved Copilot activity auditing, operational training resources, and practical migration and data protection guides.

- [Continuous Access Evaluation (CAE) Brings Real-Time Security to Azure DevOps](https://devblogs.microsoft.com/devops/real-time-security-with-continuous-access-evaluation-cae-comes-to-azure-devops/)
- [Investigating Microsoft 365 Copilot Activity with Sentinel, Defender XDR, and Purview DSPM for AI Security](https://techcommunity.microsoft.com/t5/microsoft-security-community/investigating-m365-copilot-activity-with-sentinel-defender-xdr/ba-p/4442641)
- [From Traditional Security to AI-Driven Cyber Resilience: Microsoft’s Approach to Securing AI](https://techcommunity.microsoft.com/t5/microsoft-security-community/from-traditional-security-to-ai-driven-cyber-resilience/ba-p/4442652)
- [Govern AI Securely with Microsoft Purview: Compliance Across Copilot, ChatGPT, and Beyond](https://techcommunity.microsoft.com/t5/microsoft-security-community/microsoft-purview-the-ultimate-ai-data-security-solution/ba-p/4441324)
- [Microsoft Defender Experts Ninja Hub: Resources for XDR and Threat Hunting](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/welcome-to-the-microsoft-defender-experts-ninja-hub/ba-p/4442210)
- [Microsoft Security Exposure Management Ninja Training](https://techcommunity.microsoft.com/t5/microsoft-security-exposure/microsoft-security-exposure-management-ninja-training/ba-p/4444285)
- [What’s New in Microsoft Purview eDiscovery](https://techcommunity.microsoft.com/t5/microsoft-security-community/what-s-new-in-microsoft-purview-ediscovery/ba-p/4441676)
- [Step-by-Step Guide for Migrating Windows Server 2012 R2 Domain Controllers to Server 2022](https://techcommunity.microsoft.com/t5/tech-community-discussion/migrate-2012-r2-to-server-2022/m-p/4444704#M9677)
- [Issuing Custom Claims Using Directory Extension Attributes in Microsoft Entra ID](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/issuing-custom-claims-using-directory-extension-attributes-in/ba-p/4441980)
- [Practical Data Protection in Microsoft 365: Sensitivity Labels, DLP, and Conditional Access for Small Businesses](https://dellenny.com/protecting-your-business-data-sensitivity-labels-dlp-and-conditional-access-explained-simply/)
- [Secure Integration of Microsoft 365 with Slack, Trello, and Google Services](https://dellenny.com/how-to-integrate-m365-with-third-party-saas-tools-slack-trello-google-services-without-breaking-security/)
- [Connect with the Security Community at Microsoft Ignite 2025](https://www.microsoft.com/en-us/security/blog/2025/08/13/connect-with-the-security-community-at-microsoft-ignite-2025/)
- [Encryption in Microsoft Teams: How Microsoft Secures Collaboration and Communication](https://techcommunity.microsoft.com/t5/microsoft-teams-blog/encryption-in-microsoft-teams-june-2025/ba-p/4442913)
- [How Microsoft Defender Uses AI to Detect Exposed Credentials in Identity Systems](https://techcommunity.microsoft.com/t5/microsoft-defender-xdr-blog/leaving-the-key-under-the-doormat-how-microsoft-defender-uses-ai/ba-p/4439870)
