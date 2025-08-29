---
layout: "post"
title: "AI Developer Tools and Automation Advances"
description: "This week's roundup spotlights major advancements in AI-powered developer tooling, with GitHub Copilot debuting smarter code reviews, expanded agent support, and real-time workflow automation across IDEs. There's a continued focus on robust AI ecosystems, enterprise adoption strategies, security automation, and platform innovation in Azure, .NET, and the broader developer landscape. Updates also reveal evolving best practices in DevOps, increased transparency in benchmarking, and strengthened endpoint protection."
author: "Tech Hub Team"
excerpt_separator: <!--excerpt_end-->
viewing_mode: "internal"
date: 2025-07-21 09:00:00 +00:00
permalink: "/2025-07-21-Weekly-AI-and-Tech-News-Roundup.html"
categories: ["AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security"]
tags: [".NET 10", "Agentic AI", "AI", "Automation", "Azure", "Cloud", "Coding", "Coding Agents", "Compliance", "DevOps", "GitHub Copilot", "Machine Learning", "MCP", "ML", "Roundups", "Security", "VS", "VS Code"]
tags_normalized: ["dotnet 10", "agentic ai", "ai", "automation", "azure", "cloud", "coding", "coding agents", "compliance", "devops", "github copilot", "machine learning", "mcp", "ml", "roundups", "security", "vs", "vs code"]
---

Welcome to this week’s Tech Roundup, where automation and AI innovation are reshaping the developer experience from code inception to deployment and security. Major updates to GitHub Copilot drive the conversation, with new AI-assisted code review features, cross-IDE coding agents, and enhanced contextual intelligence transforming how teams collaborate and maintain code quality. The Model Context Protocol’s wider adoption and real-time extensibility signal a new era of hands-on workflow automation, backed by deeper analytics and operational transparency.

AI continues to weave through every layer of software engineering, from strategic trendsetting to practical agentic architectures that connect tools, data, and cloud resources in real time. Enterprise adoption strategies, collaborative learning frameworks, and robust compliance features—particularly through Azure’s comprehensive updates—demonstrate an industry committed to secure, scalable transformation. Advancements in .NET, developer tooling, and DevOps workflows provide fertile ground for productivity while new security platforms and benchmarks keep pace with rapidly evolving threats. Dive in to explore how these interconnected stories shape the future of software engineering and cloud-powered innovation.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [AI-Assisted Code Review and Workflow Automation](#ai-assisted-code-review-and-workflow-automation)
  - [Coding Agent Capabilities Expand Across IDEs](#coding-agent-capabilities-expand-across-ides)
  - [Model Context Protocol (MCP) and Extensibility](#model-context-protocol-mcp-and-extensibility)
  - [Productivity and Reporting Enhancements](#productivity-and-reporting-enhancements)
  - [Availability, Incident Management, and Community Engagement](#availability-incident-management-and-community-engagement)
- [AI](#ai)
  - [Strategic AI Trends and Developer Tools](#strategic-ai-trends-and-developer-tools)
  - [Agentic AI and MCP Ecosystem Expansion](#agentic-ai-and-mcp-ecosystem-expansion)
  - [Enterprise AI Adoption and Center of Excellence Strategies](#enterprise-ai-adoption-and-center-of-excellence-strategies)
  - [Human-AI Collaboration and Real-World Applications](#human-ai-collaboration-and-real-world-applications)
  - [Foundational AI and Community Knowledge Exchange](#foundational-ai-and-community-knowledge-exchange)
- [Azure](#azure)
  - [Analytics, AI, and Performance Synergy](#analytics-ai-and-performance-synergy)
  - [AI Governance and Compliance](#ai-governance-and-compliance)
  - [Platform Updates, Resilience, and Scaling](#platform-updates-resilience-and-scaling)
  - [DevOps and Migration Pain Points](#devops-and-migration-pain-points)
- [Coding](#coding)
  - [.NET 10 Features and Extension Member Revolution](#net-10-features-and-extension-member-revolution)
  - [AI Tooling, MCP, and .NET Interoperability](#ai-tooling-mcp-and-net-interoperability)
  - [Developer Tools, Documentation, and Community Solutions](#developer-tools-documentation-and-community-solutions)
- [DevOps](#devops)
  - [Automating and Streamlining Merge Conflict Resolution](#automating-and-streamlining-merge-conflict-resolution)
  - [Designing Flexible Deployment Pipelines for Complex Environments](#designing-flexible-deployment-pipelines-for-complex-environments)
  - [Transitioning Outlook Add-ins for Modern DevOps Integration](#transitioning-outlook-add-ins-for-modern-devops-integration)
- [Security](#security)
  - [AI-Driven Security Automation and Platform Integration](#ai-driven-security-automation-and-platform-integration)
  - [Security Benchmarking and Real-Time Transparency](#security-benchmarking-and-real-time-transparency)
  - [Proactive Workflow Security and Vulnerability Detection](#proactive-workflow-security-and-vulnerability-detection)
  - [Defender for Endpoint and Unified Cyber Defense](#defender-for-endpoint-and-unified-cyber-defense)

## GitHub Copilot

This week, GitHub Copilot received significant enhancements, reinforcing its drive toward streamlined automation, extensibility, and practical productivity for both enterprise and individual developers. Updates advanced code review automation, cross-IDE agent support, robust contextual intelligence with MCP, reporting capabilities, and operational transparency.

### AI-Assisted Code Review and Workflow Automation

Copilot’s AI-powered code review, after internal and early previews, is now automating over 600,000 PRs monthly at Microsoft. It surfaces issues, summarizes PRs, and supports interactive queries, speeding up reviews by up to 20% and supporting compliance through customizable instructions. Guidance is consolidated in `copilot-instructions.md`, with new UI enhancements, keeping developer ownership over merges. These features build on Copilot code review’s rollout in GitHub Mobile and support for structured, scalable human-AI collaboration in code reviews.

- [Enhancing Code Quality at Scale with AI-Powered Code Reviews](https://devblogs.microsoft.com/engineering-at-microsoft/enhancing-code-quality-at-scale-with-ai-powered-code-reviews/)
- [Upcoming deprecations and changes to Copilot code review](https://github.blog/changelog/2025-07-18-upcoming-deprecations-and-changes-to-copilot-code-review)
- [Code review in the age of AI - Why developers will always own the merge button](https://github.blog/ai-and-ml/generative-ai/code-review-in-the-age-of-ai-why-developers-will-always-own-the-merge-button/)

### Coding Agent Capabilities Expand Across IDEs

Copilot coding agents are now generally available not only in VS Code but also JetBrains, Eclipse, and Xcode. Developers initiate and monitor autonomous code tasks within any major IDE, reducing context switching and bottlenecks. Enhanced security defaults restrict agent internet access, with detailed configuration options meeting enterprise policies. Broad agent availability and secure, cross-platform automation signal GitHub’s commitment to robust, organization-ready deployment.

- [Start and track GitHub Copilot coding agent sessions from Visual Studio Code](https://github.blog/changelog/2025-07-14-start-and-track-github-copilot-coding-agent-sessions-from-visual-studio-code)
- [Agent mode for JetBrains, Eclipse, and Xcode is now generally available](https://github.blog/changelog/2025-07-16-agent-mode-for-jetbrains-eclipse-and-xcode-is-now-generally-available)
- [Configure internet access for Copilot coding agent](https://github.blog/changelog/2025-07-15-configure-internet-access-for-copilot-coding-agent)
- [From chaos to clarity - Using GitHub Copilot agents to improve developer workflows](https://github.blog/ai-and-ml/github-copilot/from-chaos-to-clarity-using-github-copilot-agents-to-improve-developer-workflows/)
- [Finish your work without touching any code]({{ "/2025-07-18-Finish-your-work-without-touching-any-code.html" | relative_url }})

### Model Context Protocol (MCP) and Extensibility

Copilot’s Model Context Protocol (MCP) is now fully live in VS Code, enabling real-time integration with external tools and data sources. This brings contextually aware AI support, automating tasks using live project data, APIs, and databases, and supporting hands-on workflow automation. VS Code’s v1.102 release leverages MCP for cross-environment collaboration, with tutorials guiding effective team adoption.

- [Model Context Protocol (MCP) support in VS Code is generally available](https://github.blog/changelog/2025-07-14-model-context-protocol-mcp-support-in-vs-code-is-generally-available)
- [VS Code Live - Exploring v1.102 Release Features—AI Chat, MCP, Coding Agent & More]({{ "/2025-07-17-VS-Code-Live-Exploring-v1102-Release-FeaturesAI-Chat-MCP-Coding-Agent-More.html" | relative_url }})
- [Model selection]({{ "/2025-07-16-GitHub-Copilot-Features-Model-selection.html" | relative_url }})
- [Automating Developer Tasks with GitHub Copilot Agent Mode and MCP Servers in VS Code]({{ "/2025-07-18-Automating-Developer-Tasks-with-GitHub-Copilot-Agent-Mode-and-MCP-Servers-in-VS-Code.html" | relative_url }})

### Productivity and Reporting Enhancements

Copilot adds support for interactive issue forms on github.com and enhances administrative oversight with improved activity reports, offering granular insights into usage and authentication for license management and targeted support. Copilot Chat supports early software planning, helping teams clarify requirements and reduce rework.

- [Support for issue forms in chat and file uploads in spaces](https://github.blog/changelog/2025-07-16-support-for-issue-forms-in-chat-and-file-uploads-in-spaces)
- [New GitHub Copilot activity report with enhanced authentication and usage insights](https://github.blog/changelog/2025-07-18-new-github-copilot-activity-report-with-enhanced-authentication-and-usage-insights)
- [Use GitHub Copilot Chat to Plan Your Software Before Coding](https://pagelsr.github.io/CooknWithCopilot/blog/use-github-copilot-chat-to-plan-before-you-code.html)

### Availability, Incident Management, and Community Engagement

The June GitHub Availability Report reviews resilience efforts, transparency in incident management, and plans for improved monitoring. The "For the Love of Code" summer hackathon encourages creative use of Copilot and open source, sustaining community engagement and informing future product directions.

- [GitHub Availability Report: June 2025](https://github.blog/news-insights/company-news/github-availability-report-june-2025/)
- [For the Love of Code - A Summer Hackathon for Joyful and Creative Projects](https://github.blog/open-source/for-the-love-of-code-2025/)

## AI

AI advancements this week underscore the convergence of automation, agentic architectures, and robust AI adoption frameworks. Key themes include strategic AI trends, agent ecosystems, enterprise enablement, collaborative frameworks, and accessible education.

### Strategic AI Trends and Developer Tools

2025’s software engineering centers on AI at every stage—from prompt-driven workflows and code orchestration, to composable architectures and standardized ethics. Trends highlight low-code democratization, DevSecOps, value-based engineering, and AI-powered multiplatform automation. Team success depends on blending architectural innovation with responsible governance.

- [Key Trends Driving Software Engineering in 2025](https://dellenny.com/key-trends-driving-software-engineering-in-2025/)

### Agentic AI and MCP Ecosystem Expansion

Agentic AI—autonomous, adaptive agents—powers workflow management and orchestration, fueled by a rapidly growing MCP ecosystem. Ten Microsoft MCP servers now connect IDEs to Azure, GitHub, Microsoft 365, SQL, and web testing, standardizing tool integration. Tutorials in Python and Java further expand MCP accessibility, powering real-time, cross-platform developer automation.

- [Agentic AI: The Next Evolution Beyond Generative AI for Solution Architects](https://dellenny.com/agentic-ai-the-next-evolution-beyond-generative-ai-for-solution-architects/)
- [10 Microsoft MCP Servers to Accelerate Your Development Workflow](https://devblogs.microsoft.com/blog/10-microsoft-mcp-servers-to-accelerate-your-development-workflow)
- [Let's Learn MCP: Python]({{ "/2025-07-16-Lets-Learn-MCP-Python.html" | relative_url }})
- [Let's Learn MCP: Java]({{ "/2025-07-15-Lets-Learn-MCP-Java.html" | relative_url }})

### Enterprise AI Adoption and Center of Excellence Strategies

Centers of Excellence are central to enterprise AI adoption: they align strategy, technical support, and governance, drive staff upskilling, and create scalable, repeatable frameworks. Real-world stories from Oracle, Deloitte, and DoD demonstrate success metrics and best practices, backed by the integration of AI tools like Copilot and VS Code.

- [Building a Center of Excellence for AI: A Strategic Roadmap for Enterprise Adoption](https://hiddedesmet.com/creating-ccoe-for-ai)

### Human-AI Collaboration and Real-World Applications

Microsoft’s collaboration framework aligns LLM outputs with user intention, emphasizing practical and easy human-AI integration. Adecco’s AI-first recruitment uses Azure to automate HR, embedding upskilling for developers and applicants. In education, Minecraft delivers gamified, interactive programming and AI lessons, making complex topics engaging for students and teachers.

- [Microsoft introduces new training framework that enhances human-LLM collaboration](https://www.microsoft.com/en-us/research/blog/collabllm-teaching-llms-to-collaborate-with-users/)
- [Adecco's AI-First Approach to Recruitment with Microsoft Cloud]({{ "/2025-07-18-Adeccos-AI-First-Approach-to-Recruitment-with-Microsoft-Cloud.html" | relative_url }})
- [Integrating interactivity with Minecraft Education - A fun approach to learning coding and AI](https://news.microsoft.com/source/asia/2025/07/15/integrating-interactive-and-collaborative-learning-solutions-with-minecraft-education-a-fun-approach-to-learn-coding-and-ai/)

### Foundational AI and Community Knowledge Exchange

Structured education and peer learning continue to grow. John Savill’s AI/ML primer and Xebia’s Knowledge Exchange events provide foundational training, professional networking, and practical application, supporting entry and progression in the AI field.

- [Artificial Intelligence (AI) and Machine Learning (ML) for Everyone!]({{ "/2025-07-16-Artificial-Intelligence-AI-and-Machine-Learning-ML-for-Everyone.html" | relative_url }})
- [Xebia Knowledge Exchange]({{ "/2025-07-14-XKE-Coming-Soon.html" | relative_url }})

## Azure

Azure’s momentum this week is fueled by stronger analytics integration, AI governance, certified compliance, robust platform updates, and active community problem solving.

### Analytics, AI, and Performance Synergy

Azure Databricks, now fully native, outperforms AWS equivalents and integrates tightly with Power BI, Purview, Copilot Studio, and Azure AI Foundry. New autoscaling, data governance, and seamless multicloud support make it easy to automate analytics, CI/CD, and AI workload management at scale.

- [Databricks Runs Best on Azure: Performance, Integration, and AI Synergy](https://azure.microsoft.com/en-us/blog/databricks-runs-best-on-azure/)

### AI Governance and Compliance

Azure AI Foundry Models and Security Copilot are ISO/IEC 42001:2023 certified, simplifying compliance for regulated industries and reinforcing responsible AI deployment throughout the development lifecycle.

- [Microsoft Azure AI Foundry Models and Security Copilot Achieve ISO/IEC 42001:2023 Certification](https://azure.microsoft.com/en-us/blog/microsoft-azure-ai-foundry-models-and-microsoft-security-copilot-achieve-iso-iec-420012023-certification/)

### Platform Updates, Resilience, and Scaling

The July Azure platform update introduces PowerShell Durable Functions (stateful orchestration), Functions with Kafka triggers, AKS auto-provisioning, improved NFS, geo-replication, security, and backup policies. The AI Speaker Recognition retirement signals workflow updates. These combine to enhance scalability, reliability, and automation.

- [Azure Update – 18th July 2025: New Services, Features & Retirements]({{ "/2025-07-18-Azure-Update-18th-July-2025-New-Services-Features-and-Retirements.html" | relative_url }})

### DevOps and Migration Pain Points

Community feedback highlights Azure DevOps test case result visibility limitations, TFS-to-Azure DevOps migration challenges (re: Teams and Areas structure), and ARM resource limits for Data Factory deployments. Practical scripts, process customizations, and deployment segmentation help teams overcome migration and automation hurdles.

- [Test Plan: How to Get the Latest 'Test Case Result' for All Test Cases in Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1m26lp7/test_plan_or_test_suite_how_to_get_the_last_test/)
- [Challenges Migrating Teams and Areas Structure from TFS to Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1m32qpm/teams_vs_areas/)
- [Workaround for Azure ARM 800 Resource Limit When Deploying Data Factory](https://www.reddit.com/r/azuredevops/comments/1m30btw/workaround_for_azure_arm_800_resource_limit_while/)

## Coding

A host of releases and community solutions empower .NET, VS Code, and Visual Studio users, delivering faster, modernized APIs, automation, cross-language extensibility, and practical tooling.

### .NET 10 Features and Extension Member Revolution

.NET 10 Preview and C# 14’s extension members allow seamless type extension—supporting methods, properties, and soon operators—without source modification, yielding discoverable, idiomatic APIs. Preview 6 adds post-quantum cryptography, performance boosts, stateful Blazor, passkey auth, and more, plus faster build and deployment tooling.

- [C# 14 Extension Members in .NET 10 Preview: How to Use Extension Everything](https://andrewlock.net/exploring-dotnet-10-preview-features-3-csharp-14-extensions-members/)
- [.NET 10 Preview 6: New Features Across Runtime, SDK, Libraries, ASP.NET Core, Blazor, and More](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-6/)
- [.NET 10 Preview 6 Unboxed - Blazor State Persistence, Passkey, & What is DNX?]({{ "/2025-07-15-NET-10-Preview-6-Unboxed-Blazor-State-Persistence-Passkey-and-What-is-DNX.html" | relative_url }})

### AI Tooling, MCP, and .NET Interoperability

New templates and workflows make creating MCP servers in .NET 10 and publishing on NuGet simple, seamlessly blending AI extension and modern extensibility. Python.NET enables embedding Python in C#, expanding .NET’s reach into data science.

- [Building Your First MCP Server with .NET 10 and Publishing to NuGet](https://devblogs.microsoft.com/dotnet/mcp-server-dotnet-nuget-quickstart/)
- [MCP C# SDK: What’s New and Upcoming for .NET Developers]({{ "/2025-07-17-MCP-C-SDK-Whats-New-and-Upcoming-for-NET-Developers.html" | relative_url }})
- [Writing and Running Python in .NET]({{ "/2025-07-16-Writing-and-Running-Python-in-NET.html" | relative_url }})

### Developer Tools, Documentation, and Community Solutions

VS Code’s Markdown Mermaid Viewer solves diagram previewing; a Visual Studio extension adds git worktree support for better branch management. Build-time OpenAPI specs in .NET 9 empower CI/CD and automation. Community standup sessions unpack modern Minimal APIs and database integration, while reusable WPF helpers improve desktop UI. Scott Hanselman spotlights .NET’s broad relevance and stability.

- [VS Code Extension: Preview Mermaid Diagrams in Markdown for Azure DevOps](https://www.reddit.com/r/azuredevops/comments/1m42lmi/vs_code_extension_preview_mermaid_diagrams_in/)
- [Visual Studio Has Most Git Features I Need—Except Worktree, So I Built an Extension for It](https://www.reddit.com/r/VisualStudio/comments/1m1l1lc/visual_studio_has_most_git_features_i_need_except/)
- [Build-Time OpenAPI Documentation in .NET 9: A OneDevQuestion with Mike Kistler]({{ "/2025-07-16-Build-Time-OpenAPI-Documentation-in-NET-9-A-OneDevQuestion-with-Mike-Kistler.html" | relative_url }})
- [.NET Data Community Standup: Couchbase EF Core Provider Discussion]({{ "/2025-07-18-NET-Data-Community-Standup-Couchbase-EF-Core-Provider-Discussion.html" | relative_url }})
- [ASP.NET Community Standup - Why Aren't You Using Minimal APIs?]({{ "/2025-07-15-ASPNET-Community-Standup-Why-arent-you-using-Minimal-APIs.html" | relative_url }})
- [Centering a WPF TreeViewItem in the TreeView ScrollViewer](https://weblog.west-wind.com/posts/2025/Jul/15/Centering-a-WPF-TreeViewItem-in-the-TreeView-ScrollViewer)
- [Is .NET Legacy Tech? Scott Hanselman Explores the Modern .NET Platform]({{ "/2025-07-17-Is-NET-Legacy-Tech-Scott-Hanselman-Explores-the-Modern-NET-Platform.html" | relative_url }})

## DevOps

Community discussions this week emphasized workflow automation, flexible pipeline design, and toolchain modernization to meet the needs of evolving cloud and enterprise environments.

### Automating and Streamlining Merge Conflict Resolution

Developers automate large-scale merge conflict resolution in Visual Studio using git commands or scripts to accept incoming changes, calling for IDE extensions and improved tooling as projects grow.

- [How to Auto-Resolve 100+ Merge Conflicts by Accepting Incoming Version for All Files?](https://www.reddit.com/r/azuredevops/comments/1m1xrde/how_to_autoresolve_100_merge_conflicts_by/)

### Designing Flexible Deployment Pipelines for Complex Environments

Developers architect cleaner Azure DevOps pipelines for multi-environment, role-based deployments, favoring dynamic inventories, centralized configs, and orchestration tools over hardcoded logic to better manage complexity and troubleshooting.

- [Seeking Advice on Deployment Pipeline Design for Multi-Environment, Role-Based Server Setups](https://www.reddit.com/r/azuredevops/comments/1m2vtfz/looking_for_advice_on_architecting_a_deployment/)

### Transitioning Outlook Add-ins for Modern DevOps Integration

As Outlook add-in models modernize, integrating DevOps/TFS extensions for compatibility and productivity remains a niche need, requiring adaptive marketing and ongoing innovation to serve enterprise users in the cloud era.

- [Should I Build for Azure DevOps? Exploring Market Potential for Outlook Add-ins](https://www.reddit.com/r/azuredevops/comments/1m4jkch/should_i_build_for_ado/)

## Security

Security updates this week highlighted deeper AI integration, product benchmarking, workflow threat prevention, and unified endpoint defense.

### AI-Driven Security Automation and Platform Integration

Security Copilot’s general availability in Intune and Entra automates threat detection, remediation, and incident response, merging AI with identity and compliance workflows for proactive security operations and rapid recovery. Conditional access, new natural language, and graph integrations expand transparency and customizability.

- [Security Copilot capabilities in Microsoft Intune and Entra now generally available](https://www.microsoft.com/en-us/security/blog/2025/07/14/improving-it-efficiency-with-microsoft-security-copilot-in-microsoft-intune-and-microsoft-entra/)
- [Microsoft Security Copilot Entra Update and Conditional Access Agent]({{ "/2025-07-14-Microsoft-Security-Copilot-Entra-Update-and-Conditional-Access-Agent.html" | relative_url }})

### Security Benchmarking and Real-Time Transparency

A new Defender for Office 365 dashboard provides customers with transparent benchmarks, competitive metrics, and quarterly updates, empowering organizations to evaluate security posture and drive continuous improvement.

- [Microsoft Defender for Office 365: Transparent Benchmarks on Email Security Effectiveness](https://www.microsoft.com/en-us/security/blog/2025/07/17/transparency-on-microsoft-defender-for-office-365-email-security-effectiveness/)

### Proactive Workflow Security and Vulnerability Detection

The prevalence of workflow injection threats in GitHub Actions is countered with CodeQL and automated scanning, along with permissions audits and action vetting to prevent CI/CD privilege escalation—key for scaling safe automation across modern developer pipelines.

- [How to Catch GitHub Actions Workflow Injections Before Attackers Do](https://github.blog/security/vulnerability-research/how-to-catch-github-actions-workflow-injections-before-attackers-do/)

### Defender for Endpoint and Unified Cyber Defense

Microsoft Defender for Endpoint, reaffirmed as a leader by Gartner and demonstrated at Black Hat USA 2025, provides AI-first, unified detection and response across cloud and hybrid environments, solidifying Microsoft’s commitment to integrated, automated enterprise defense.

- [Microsoft Named a Leader in Gartner's 2025 Magic Quadrant for Endpoint Protection](https://www.microsoft.com/en-us/security/blog/2025/07/16/microsoft-is-named-a-leader-in-the-2025-gartner-magic-quadrant-for-endpoint-protection-platforms/)
- [Microsoft at Black Hat USA 2025: A Unified Approach to Modern Cyber Defense](https://techcommunity.microsoft.com/blog/microsoft-security-blog/%E2%80%8B%E2%80%8Bmicrosoft-at-black-hat-usa-2025-a-unified-approach-to-modern-cyber-defense%E2%80%8B%E2%80%8B/4434292)
- [Microsoft at Black Hat USA 2025: A unified approach to modern cyber defense](https://techcommunity.microsoft.com/blog/microsoft-security-blog/%E2%80%8B%E2%80%8Bmicrosoft-at-black-hat-usa-2025-a-unified-approach-to-modern-cyber-defense%E2%80%8B%E2%80%8B/4434292)
