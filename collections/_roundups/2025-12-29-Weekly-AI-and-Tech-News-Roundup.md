---
title: 'AI Agent Frameworks, Copilot Automation, and Security Improvements: Weekly Tech Highlights'
author: Tech Hub Team
viewing_mode: internal
date: 2025-12-29 09:00:00 +00:00
tags:
- Agent Framework
- AI Agents
- Automation
- Azure Databricks
- Azure SRE
- CI/CD
- Load Testing
- MCP (model Context Protocol)
- Open Source
- Supply Chain Security
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
Welcome to this week’s roundup of tech updates, focusing on advances in AI agent frameworks, workflow automation, and practical security measures. GitHub Copilot and Microsoft’s Agent Framework continue to develop tools that help embed automation into everyday development work. Azure offers updated reliability features and more streamlined DevOps tools. Security takes priority with guidance on supply chain issues, and the coding and DevOps sections highlight ways to improve productivity, compliance, and sustainability for development teams.<!--excerpt_end-->

## This Week's Overview

- [GitHub Copilot](#github-copilot)
  - [GitHub Copilot Integration and Agentic Automation in Visual Studio Code](#github-copilot-integration-and-agentic-automation-in-visual-studio-code)
  - [Agentic Copilot Workflows in Visual Studio and Project Management](#agentic-copilot-workflows-in-visual-studio-and-project-management)
  - [Copilot Coding Agent and Backlog Management Techniques](#copilot-coding-agent-and-backlog-management-techniques)
- [AI](#ai)
  - [Microsoft Agent Framework: A Unified Approach to Enterprise AI Agents](#microsoft-agent-framework-a-unified-approach-to-enterprise-ai-agents)
  - [Streamlining Agent Customization and Integration in Developer Tooling](#streamlining-agent-customization-and-integration-in-developer-tooling)
  - [Hosting and Scaling ChatGPT Apps with Azure Functions](#hosting-and-scaling-chatgpt-apps-with-azure-functions)
  - [Enriching AI Agents with Semantic Data: Introducing Fabric IQ](#enriching-ai-agents-with-semantic-data-introducing-fabric-iq)
- [Azure](#azure)
  - [Azure SRE Agent: Autonomous Reliability and Orchestration](#azure-sre-agent-autonomous-reliability-and-orchestration)
  - [Securing Azure Databricks: Key Vault Integration](#securing-azure-databricks-key-vault-integration)
  - [AI-powered Load Test Generation in Azure Load Testing](#ai-powered-load-test-generation-in-azure-load-testing)
- [Coding](#coding)
  - [Building Confidence for 2026: Trends, Skills, and Open Source Practices](#building-confidence-for-2026-trends-skills-and-open-source-practices)
  - [Developer Workflow Enhancements in Visual Studio Code](#developer-workflow-enhancements-in-visual-studio-code)
- [DevOps](#devops)
  - [Azure SRE Agent Automation and Advanced DevOps Workflows](#azure-sre-agent-automation-and-advanced-devops-workflows)
  - [GitHub Actions Updates: Workflow Productivity and Security](#github-actions-updates-workflow-productivity-and-security)
  - [GitHub Governance: Granular App Access Controls Now in Preview](#github-governance-granular-app-access-controls-now-in-preview)
  - [Azure DevOps: Test Run Hub Reaches General Availability](#azure-devops-test-run-hub-reaches-general-availability)
- [Security](#security)
  - [Supply Chain Security in Open Source Development](#supply-chain-security-in-open-source-development)

## GitHub Copilot

This week’s updates for GitHub Copilot focus on improving workflow capabilities and strengthening integration with common tools. Visual Studio Code adds new features for agent automation and customization, and practical guides show how to use Copilot to support daily work. Security enhancements, more flexible agent models, and ongoing community contributions continue to make Copilot a valuable tool for AI-driven development.

### GitHub Copilot Integration and Agentic Automation in Visual Studio Code

Building on last week's coverage of agent skills and Mission Control, Visual Studio Code now offers more seamless Copilot integration for daily work, with new tools for customizing and automating agents. In the VS Code 2025 retrospective by Burke Holland and Pierce Boggan, these changes are highlighted, including continued advancements in Agent Skills, Mission Control, and the availability of Copilot Universe and Free features to make AI coding support accessible, as referenced in earlier updates.

Agent Mode and Auto Mode—released after last week’s reusable workflow instructions—make it easier for developers to automate tasks and background activities. The inclusion of Model Choice Platform (MCP) and Bring Your Own Key (BYOK) allows teams to choose and configure AI models as needed, matching earlier discussions around governance and transparency.

Recent improvements to prompt engineering, including updated prompt file support and multi-window chat, give developers a more responsive interactive workflow. Support for Cloud Agent, originally launched in May, continues the trend toward parallel task execution and integrates with pull request processes to help reduce development waits.

Security remains an ongoing effort; terminal sandboxing and new safety measures address concerns about agent execution, continuing the platform protections discussed in relation to earlier vulnerabilities like React2Shell. Visual Studio Code 1.107 introduces agent session management in Copilot Chat, adding enterprise-level tracking of Copilot activities, consistent with progress on team metrics and review features.

Asynchronous workflow delegation now extends through Copilot CLI and cloud agents, improving on last week’s methods for running isolated agent workflows and addressing merge conflict management through separate branches. The latest updates provide more options for full review in source control, balancing automation with necessary human checks.

Custom agents can now be shared at the organization level, with repository-based enterprise controls supporting community-driven sharing through Skills.md and the Agent Skills forum. This positions Copilot as a platform for reusable automation. New MCP server bundling and infrastructure adjustments make setup and integration easier, reflecting last week’s focus on agent persistence and operational context.

Security processes, project-specific skills, and review controls build on last week's coverage of compliance, review assignments, and tailored AI integration, keeping Copilot headed toward a broad-use productivity solution that addresses enterprise needs.

- [VS Code 2025: Year in Review with Burke Holland & Pierce Boggan](/2025-12-23-VS-Code-2025-Year-in-Review-with-Burke-Holland-and-Pierce-Boggan.html)
- [Visual Studio Code and GitHub Copilot - What's new in 1.107](/2025-12-22-Visual-Studio-Code-and-GitHub-Copilot-Whats-new-in-1107.html)

### Agentic Copilot Workflows in Visual Studio and Project Management

Agent-based workflows in Visual Studio are getting wider adoption, as detailed in Mads Kristensen’s recent guide. Projects like converting books to static websites and writing custom language extensions illustrate how Copilot supports automating repetitive tasks, while still allowing for careful oversight—a direction consistent with earlier features focused on context-specific applications.

These workflows use Visual Studio’s Cloud Agent, Copilot Chat, and extension APIs, aligning with recent coverage of Azure Boards and automation integrations. Further integration with GitHub Actions for CI/CD and NuGet for package management highlights Copilot’s role through the entire delivery pipeline, from code to deployment.

- [How AI Fixed My Procrastination: Using Copilot and AI Agents in Visual Studio Projects](https://devblogs.microsoft.com/visualstudio/how-ai-fixed-my-procrastination/)

### Copilot Coding Agent and Backlog Management Techniques

This week introduces the WRAP method for backlog management, which extends recent themes on context-driven workflows and structured project organization. WRAP blends task scoping, context management, and agent pairing, demonstrating how teams can use Copilot and agent automation to streamline task assignment and completion.

The approach builds directly on improvements in code review, project metrics, and agent integration discussed in prior roundups. The WRAP method details how Copilot can manage focused migrations or repetitive coding assignments, echoing examples of parallel workflows and pull request workflow integration from last week’s feature set.

Together, these resources offer step-by-step advice for using Copilot and agent automation in team environments, supporting extensibility and standardization in line with recent best-practice recommendations.

- [Maximize Your Backlog Productivity with the WRAP Method and GitHub Copilot Coding Agent](https://github.blog/ai-and-ml/github-copilot/wrap-up-your-backlog-with-github-copilot-coding-agent/)

## AI

The AI section highlights updated agent frameworks, enhanced developer experiences for conversational AI applications, and new tools for integrating semantic data. Key releases include Microsoft’s latest Agent Framework, new agent customization options in VS Code, simplified ChatGPT hosting on Azure, and the Fabric IQ semantic data layer. These updates expand agent features and provide better support for context-aware solutions.

### Microsoft Agent Framework: A Unified Approach to Enterprise AI Agents

Microsoft’s open-source Agent Framework is designed for building advanced AI agents and multi-agent systems, with support for both .NET and Python. The framework merges the stability and workflow features of Semantic Kernel with the modular design of AutoGen, making it straightforward to connect with LLMs, handle state, and integrate external tools. It focuses on maintainability, checkpointing, and human involvement, offering flexibility for long-running and complex agent operations in enterprise settings. Bringing together tools previously split across the ecosystem, Agent Framework streamlines agent development and operational support for both .NET and Python.

Continuing themes from Azure AI Foundry and its ecosystem, this framework brings orchestration, hosting, and model routing under a single solution. It combines methods like Durable Task Extension with orchestration models to support Microsoft’s efforts to unify agent development for cloud and enterprise use.

- [What Is Microsoft Agent Framework & Why Another Agent Framework?](/2025-12-22-What-Is-Microsoft-Agent-Framework-and-Why-Another-Agent-Framework.html)

### Streamlining Agent Customization and Integration in Developer Tooling

Visual Studio Code now supports an Agent Skills feature, letting developers configure and manage agent abilities through markdown files directly in the editor. Skills can be switched on or off, providing flexible guidance to agents and simplifying context management. This streamlines the setup and tuning of agent capabilities in individual code projects.

Recent updates to Agent Skills and the Skills.md standard complement Copilot and Azure AI Foundry improvements introduced last week. The current focus is on integrated skills editing in VS Code, building on positive community feedback about consistent, shareable configuration. These enhancements further the effort to make agent customization accessible across toolsets.

- [Customizing AI Agents in VS Code with Agent Skills](/2025-12-26-Customizing-AI-Agents-in-VS-Code-with-Agent-Skills.html)

### Hosting and Scaling ChatGPT Apps with Azure Functions

Developers can now deploy ChatGPT applications more easily using Azure Functions, the MCP server model (FastMCP for Python), and the Azure Developer CLI for setup and authentication. The step-by-step guide walks through backend service configuration, custom endpoint integration (like weather data), and embedding chat metadata. Azure’s serverless hosting means automated scaling, pay-by-usage pricing, and simple connection to the ChatGPT App Directory. Included examples and developer-mode tools offer clear paths for Python users building and testing advanced chat apps without dedicated infrastructure. Planned improvements include stronger authentication and increased support for MCP extensions on Azure.

This matches recent trends around the Model Context Protocol and Azure AI Foundry’s model router, allowing developers to reliably link models, APIs, and backend resources for conversational agents. The walkthrough provides real code and approaches for running ChatGPT apps on Azure.

- [How to Host ChatGPT Apps on Azure Functions: Developer Guide](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/host-chatgpt-apps-on-azure-functions/ba-p/4480696)

### Enriching AI Agents with Semantic Data: Introducing Fabric IQ

Fabric IQ is a semantic modeling layer for Microsoft Fabric that helps agents understand business concepts and workflows. This abstraction layer maps raw data into entities and relationships, supports business logic with rules and validations, and enables monitoring through its ontology framework. The Fabric Graph allows for entity relationship mapping and supports both GQL and natural language queries, so agents can interact with business data more naturally. Full setup instructions and governance integration are included, making it easier for agents to incorporate organizational knowledge into their operation.

Building on prior discussions about Fabric’s AI-powered workflows, this update introduces direct semantic modeling to the process, which supports better analytics and agent operations based on real organizational data and rules.

- [Understanding Fabric IQ: The Semantic Layer in Microsoft Fabric](https://zure.com/blog/fabric-iq-the-new-semantic-layer-for-your-organizational-data)

## Azure

Azure’s recent changes prioritize automation, reliability, and up-to-date security. This section features updated guides for SRE operations, secure secrets handling in Databricks, and new options for AI-based load testing. These releases are aimed at helping teams adopt proven patterns for safer and more effective operations.

### Azure SRE Agent: Autonomous Reliability and Orchestration

A new technical guide details setting up the Azure SRE Agent to help with automated incident recovery for .NET 9 APIs hosted on Azure. Teams can connect telemetry with Azure Monitor and Application Insights, then configure sub-agents (such as AvgResponseTime and DeploymentHealthCheck) to watch for indicators and trigger automated rollbacks, issue creation, and incident alerts via Teams or email. Code samples and configuration steps are provided to make it easier to combine deployment, monitoring, and incident response in one platform.

- [Fix It Before They Feel It: Proactive Reliability with Azure SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/fix-it-before-they-feel-it-higher-reliability-with-proactive/ba-p/4480444)

Following last week's SRE update, this new guidance explains how to customize SRE automation for full production use. The documentation covers integration with alert handling services like PagerDuty and ServiceNow, and provides technical steps for context-driven engineering—demonstrating Azure’s approach to comprehensive toolchain orchestration and system reliability. These improvements reflect Azure’s ongoing commitment to hands-on, practical SRE automation.

- [Context Engineering Lessons from Building Azure SRE Agent](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/context-engineering-lessons-from-building-azure-sre-agent/ba-p/4481200)

### Securing Azure Databricks: Key Vault Integration

This guidance shows how to connect Azure Key Vault with Databricks for secure secret management, ensuring credentials are never hardcoded and can be accessed only at runtime. The walkthrough includes registering Azure AD applications, granting Key Vault permissions, and using Python code (with `ClientSecretCredential` and `SecretClient`) to retrieve secrets during Databricks jobs. This process enables teams to manage secrets safely and efficiently, supporting compliant and reliable data workflows for ML and ETL processes. The solution extends secure management to both Azure databases and Databricks, supporting robust security without interfering with the development process.

- [Securely Managing Database Connection Strings in Azure Databricks with Key Vault](https://techcommunity.microsoft.com/t5/microsoft-developer-community/data-security-azure-key-vault-in-data-bricks/ba-p/4479785)

This update continues Azure’s trend of publishing actionable security guidance, broadening secure identity and secret practices into modern data engineering and automation.

### AI-powered Load Test Generation in Azure Load Testing

Azure Load Testing now uses AI to speed up JMeter script creation. With a browser extension for Edge or Chrome, developers can record user interactions and upload the session to Azure Load Testing. AI then annotates scripts, applies wait times, and manages dynamic values, producing load tests that better reflect real-world user actions. Teams get the option to review, edit, or download these scripts, giving them greater control and quality over the results. This solution helps teams catch performance issues earlier, and provides consistent load testing for scaling applications.

- [AI-assisted load test authoring in Azure Load Testing](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/ai-assisted-load-test-authoring-in-azure-load-testing/ba-p/4480652)

This feature expands Azure’s focus on workflow automation and developer tools. While previous releases emphasized reliability automation for SRE, this update adds AI-driven enhancements to performance and test engineering, further supporting DevOps efficiency.

## Coding

This week’s coding news discusses how to prepare for future trends while streamlining everyday workflows. Developers are encouraged to consider strategies and resources for building sustainable, effective skills and projects leading into 2026.

### Building Confidence for 2026: Trends, Skills, and Open Source Practices

Cassidy Williams’ curation of GitHub Podcast episodes covers approaches to stay up-to-date with future-oriented coding techniques. Topics include technical advancements like Model Context Protocol (MCP)—which supports improved AI tool interoperability and more resilient software models—with practical references like the GitHub MCP server for hands-on experimentation.

These episodes align with ongoing developer roadmap discussions around technologies like .NET 11, emphasizing the benefits of planning and tool selection for future-ready skills. The guidance is practical, connected to recent posts about career development and open source community health.

DIY and open source contributions remain central. The podcast shares examples of how accessible ecosystems support rapid project building. Episodes also highlight best practices for fostering sustainability in open source, referencing lessons from cases like Log4Shell, and encourage contributors to engage through multiple roles, not just code.

Other segments address trends from Octoverse 2025—such as TypeScript’s growth, the adoption of AI-assisted development, and the continued relevance of existing languages. The Home Assistant community project is given as an example of privacy-focused, community-driven software.

Each episode provides actionable advice, from exploring new tech to contributing for long-term community stability—helping developers align skills and projects with emerging trends.

- [5 Podcast Episodes to Help You Build with Confidence in 2026](https://github.blog/open-source/maintainers/5-podcast-episodes-to-help-you-build-with-confidence-in-2026/)

### Developer Workflow Enhancements in Visual Studio Code

Web developers now have access to the Simple Browser in Visual Studio Code, as explained in Justin Chen’s guide. This feature enables the use of browser tabs for local development servers with integrated inspection tools so developers can debug and resolve HTML issues within the editor.

This follows the recent release of VS Code 1.107, which introduced updated features like inline chat and persistent agents for improved task handling. The current focus on web development features means less switching between tools, streamlining work and saving time.

VS Code’s browser supports integration with AI-powered features and code automation to provide responsive suggestions and speed up front-end workflows. Such tools improve productivity, especially during multi-step debugging and streamlined workspace management.

Planned updates will further integrate development and browser features, ensuring consistent improvements for web developers using VS Code.

- [Unlocking the Power of VS Code's Simple Browser Feature](/2025-12-22-Unlocking-the-Power-of-VS-Codes-Simple-Browser-Feature.html)

## DevOps

This week’s DevOps section spotlights automation enhancements and workflow updates across Azure, GitHub, and CI/CD environments. New processes for triage, app management, and testing help teams work more efficiently, while additional security improvements reinforce safe, reliable automation at scale.

### Azure SRE Agent Automation and Advanced DevOps Workflows

Expanding on last week’s SRE Agent automation, new instructions provide guidance for integrating the agent with the Model Context Protocol (MCP). Beyond automated runbook handling, examples show how to use MCP for complex tasks like automated customer issue triage. Developers can set up SRE Agent to work with GitHub and PagerDuty APIs, automate ticket scanning, classify issues with markdown, and auto-assign urgent cases to PagerDuty. Subagents are given clear, limited permissions, matching coverage of secure, flexible incident automation from last week’s updates. MCP adapters extend the platform for larger-scale process automation with lower friction.

- [Automating Customer Issue Triage with Azure SRE Agent and MCP](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/extend-sre-agent-with-mcp-build-an-agentic-workflow-to-triage/ba-p/4480710)

### GitHub Actions Updates: Workflow Productivity and Security

GitHub Actions announced changes that enhance productivity and reliability, complementing last week's coverage of pricing and ecosystem changes. The workflows page now loads faster for repositories with many jobs, reducing timeouts and rendering issues in large pipelines. New job status filters make navigation more manageable, addressing prior requests from the community. Jesse Houwing’s “actions-dependency-submission” custom action improves security by allowing Dependency Graph and Dependabot to function properly even with SHA-pinned or forked actions, supporting recent advances in automating and securing CI/CD. These incremental updates move Actions toward greater transparency and reliability for DevOps teams.

- [Improved Performance for GitHub Actions Workflows Page](https://github.blog/changelog/2025-12-22-improved-performance-for-github-actions-workflows-page)
- [Improved Dependency Submission for GitHub Actions](https://jessehouwing.net/github-actions-improved-dependency-submission/)

### GitHub Governance: Granular App Access Controls Now in Preview

GitHub has started a public preview for improved app request controls, expanding options for organization admins to restrict which members can request installation of GitHub/OAuth apps. These controls can be set for all members, a specific group, or disabled entirely. This change addresses compliance needs, reduces third-party risk, and continues the focus on operational security controls for larger organizations discussed last week.

- [Granular Controls for GitHub App Requests Now in Public Preview](https://github.blog/changelog/2025-12-22-control-who-can-request-apps-for-your-organization)

### Azure DevOps: Test Run Hub Reaches General Availability

The new Test Run Hub in Azure DevOps is now available for all users, providing a single location for manual and automated test runs. Integration with the Azure DevOps REST API supports automation, while new filters (for outcome, priority, and failure type) and improved artifact sharing with markdown support make collaboration simpler. Stronger search and tracking features connect test results to work items, addressing feedback from the developer community. Organizations will transition to the Test Run Hub starting January 2026, retiring legacy test management methods.

- [The New Test Run Hub in Azure Test Plans Reaches General Availability](https://devblogs.microsoft.com/devops/the-new-test-run-hub-is-going-generally-available/)

## Security

Security releases this week detail steps to improve defenses in software supply chains, especially in light of recent attacks like Shai-Hulud. These updates provide clear strategies for developers and maintainers to respond to active threats and manage secure publishing workflows.

Last week’s topics covered improvements to ecosystem-wide controls such as Dependabot and secret scanning. This week, the focus shifts to npm’s improved onboarding for OpenID Connect, additional OIDC provider compatibility, and staged publishing that requires maintainer approval through multi-factor authentication (MFA). Instructions are provided for adopting best practices alongside GitHub and npm, including token expiry, OAuth audit, and isolated publishing environments. Maintainers are also advised to enforce trusted publishing, lock dependencies, perform scans, and validate releases, consistent with prior calls for strong reviews and governance.

Incident response relies on both automated services and manual intervention; the guidance highlights ongoing improvement for teams aiming to strengthen supply chain protections.

### Supply Chain Security in Open Source Development

The Shai-Hulud attack exposed gaps in supply chain protection by targeting developer credentials and publishing processes. In response, npm is rolling out improvements such as bulk OIDC onboarding, support for additional providers, and phased release controls using MFA for sign-off.

This week’s advice stresses activating phishing-resistant MFA on both GitHub and npm, reviewing short-lived tokens, checking OAuth app permissions, and using sandboxed environments for publishing. These steps are in line with previously recommended practices for identity and secret management, and they reinforce the need for robust, repeatable controls.

Recommended strategies include using trusted publishing, pinning and scanning dependencies, and validating releases with automated and manual checks—ensuring that proactive governance stays in place for both code and packages.

Incident response continues to combine automated monitoring tools (such as Defender and Sentinel) with careful team-led investigation, highlighting the ongoing need for continuous improvement in open source supply chain practices.

- [Strengthening Supply Chain Security for Developers and Maintainers](https://github.blog/security/supply-chain-security/strengthening-supply-chain-security-preparing-for-the-next-malware-campaign/)
