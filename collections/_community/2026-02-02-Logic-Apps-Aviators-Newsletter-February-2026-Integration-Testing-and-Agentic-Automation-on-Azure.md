---
layout: "post"
title: "Logic Apps Aviators Newsletter – February 2026: Integration, Testing, and Agentic Automation on Azure"
description: "The February 2026 Logic Apps Aviators Newsletter highlights integration best practices, technical innovations in Azure Logic Apps, BizTalk modernization challenges, and the rise of AI-powered agentic orchestration. Inside, developers and architects will find product updates, troubleshooting advice, community insights, and resources focused on automation, test frameworks, cloud integration, and advancing platform capabilities in Microsoft's ecosystem."
author: "WSilveira"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-aviators-newsletter-february-2026/ba-p/4491309"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-02 16:00:00 +00:00
permalink: "/2026-02-02-Logic-Apps-Aviators-Newsletter-February-2026-Integration-Testing-and-Agentic-Automation-on-Azure.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: ["Agent Loop", "Agentic Orchestration", "AI", "API Management", "Application Registration", "Azure", "Azure Logic Apps", "Azure Resource Graph", "BizTalk Server", "C#", "Cloud Automation", "Coding", "Community", "Copilot Studio", "DevOps", "Enterprise Integration", "Functions", "Integration", "JSON", "MCP Server", "MSTest", "Service Bus", "SFTP", "Test Explorer", "Unit Testing", "Workflow Automation"]
tags_normalized: ["agent loop", "agentic orchestration", "ai", "api management", "application registration", "azure", "azure logic apps", "azure resource graph", "biztalk server", "csharp", "cloud automation", "coding", "community", "copilot studio", "devops", "enterprise integration", "functions", "integration", "json", "mcp server", "mstest", "service bus", "sftp", "test explorer", "unit testing", "workflow automation"]
---

WSilveira curates the February 2026 Logic Apps Aviators Newsletter, offering technical articles and community insights on Azure Logic Apps, BizTalk modernization, agentic workflows, test automation, and cloud integration best practices.<!--excerpt_end-->

# Logic Apps Aviators Newsletter – February 2026

Curated by WSilveira, the February issue dives deeply into the evolving landscape of enterprise integration and automation using Microsoft Azure technologies. The newsletter brings together product group updates, community troubleshooting, and advanced architecture topics, making it a must-read for integration professionals and developers.

## Ace Aviator of the Month: Camilla Bielk

- **Role:** Solutions Architect @ XLENT
- **Focus:** Full lifecycle integration on Azure, formerly with BizTalk
- **Day-to-day:** Works with Logic Apps, Service Bus, Functions, API Management; blends solution design with operations, C# development, and stakeholder engagement
- **Professional tips:** Teamwork and adaptability are vital; social and technical skills go hand-in-hand; continuous learning and knowledge sharing enable growth.
- **Wish list feature for Logic Apps:** Better upfront cost transparency and warnings for inefficient configurations and expensive operations during design-time.

## News from the Product Group

### [Introducing Unit Test Agent Profiles for Logic Apps & Data Maps](https://techcommunity.microsoft.com/blog/integrationsonazureblog/introducing-unit-test-agent-profiles-for-logic-apps--data-maps/4490216)

- Unit test agent profiles help Logic Apps Standard teams discover workflows/data maps, write reusable specs, and implement MS Test suites.
- Demonstrates how **GitHub Copilot custom agents and prompts** accelerate unit test authoring, supporting maintainability and best practices in enterprise integration.
- Emphasizes spec-first testing and reliable validations across various scenarios (happy path, error handling, boundaries).

### [Automated Test Framework – Missing Tests in Test Explorer](https://techcommunity.microsoft.com/blog/integrationsonazureblog/automated-test-framework---missing-tests-in-test-explorer/4490186)

- Issue: Logic Apps Standard tests missing in VS Code Test Explorer, often due to **MSTest version mismatch** after a C# DevKit update.
- Solution: Update MS Test package dependencies; restart VS Code for restored functionality. Ongoing extension updates planned; meanwhile, manual package adjustments are necessary.

### [Upcoming Agentic Azure Logic Apps Workshops](https://techcommunity.microsoft.com/blog/integrationsonazureblog/upcoming-agentic-azure-logic-apps-workshops/4484526)

- Free workshops on Azure Logic Apps and **agentic business processes**, covering MCP Server integrations, agent loop patterns, and Copilot Studio connections.
- Practical sessions help practitioners explore AI-agent tools and orchestration in real enterprise use cases.

## Community Highlights

- **[Enterprise AI ≠ Copilot](https://www.linkedin.com/pulse/enterprise-ai-copilot-al-ghoniem-mba-cr87c?trk=public_post-text)** (Al Ghoneim): Explains why deploying Microsoft 365 Copilot does not equate to a true AI strategy. Notes the distinction between the productivity-focused Copilot tools and broader agentic, orchestrated AI in enterprise solutions.

- **[BizTalk Server and WinSCP Error: Could not load file or assembly ‘WinSCPnet’](https://blog.sandro-pereira.com/2026/01/27/biztalk-server-and-winscp-error-could-not-load-file-or-assembly-winscpnet-version1-16-0-16453-cultureneutral-publickeytoken2271ec4a3c56d0bf-or-one-of-its-dependencies/)** (Sandro Pereira): Troubleshoots recurring SFTP failures in BizTalk, emphasizing dependency management and the impact of version mismatches—vital for teams modernizing or maintaining BizTalk on Azure.

- **[More on finding application registrations used by Logic Apps](https://www.mikaelsand.se/2026/01/more-on-finding-application-registrations-used-by-logic-apps/)** (Mikael Sand): Reveals how to locate application registrations used by Logic Apps, especially when custom HTTP actions obscure dependencies. Uses Azure Resource Graph to improve insight and change impact analysis.

- **[MCP Servers in Azure Logic Apps Agent Loops (Step-by-Step)](https://www.youtube.com/watch?v=jinbjN2PfJA)** (Stephen W. Thomas): Demonstrates migrating tool logic into reusable Model Context Protocol (MCP) Servers, improving reusability and architecture as agent loops mature. Showcases agentic automation design on Azure.

- **[From Rigid Choreography to Intelligent Collaboration: Agentic Orchestration as the Evolution of SOA](https://sjwiggers.com/2026/01/25/from-rigid-choreography-to-intelligent-collaboration-agentic-orchestration-as-the-evolution-of-soa/)** (Steef-Jan Wiggers): Explores the progression from traditional SOA and BizTalk patterns to agent-driven, adaptive orchestration with Azure Logic Apps and agentic design.

- **["Run with payload" in the new Logic Apps designer enforces JSON only](https://blog.sandro-pereira.com/2026/01/23/run-with-payload-logic-apps-designer-enforces-json/)** (Sandro Pereira): A practical warning about recent updates to Logic Apps designer—manual test payloads are now restricted to JSON, complicating non-JSON workflows and authoring experiences.

---

The newsletter remains a reliable resource for:

- Azure Logic Apps professionals seeking insights, automation tips, and testability improvements
- Integration architects navigating modernization from BizTalk to cloud
- Those adopting agentic workflows, AI automation, and reusable orchestration patterns on Microsoft cloud
- DevOps practitioners and developers optimizing CI/CD, test frameworks, and developer tooling on Azure

**For the full conversation and resources, visit the [Azure Integration Services Blog](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/bg-p/IntegrationsonAzureBlog).**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/logic-apps-aviators-newsletter-february-2026/ba-p/4491309)
