---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-azure-app-testing-scalable-end-to-end-app-validation/ba-p/4440496
title: 'Azure App Testing: Unified AI-Driven Load and E2E Testing in the Cloud'
author: John_Stallo
feed_name: Microsoft Tech Community
date: 2025-08-07 17:43:18 +00:00
tags:
- AI Driven Testing
- Azure App Testing
- Azure Load Testing
- CI/CD
- Cloud Testing
- Continuous Integration
- Cross Browser Testing
- End To End Testing
- GitHub Copilot Agent
- Infrastructure Automation
- JMeter
- Load Testing
- Locust
- Microsoft Azure
- Microsoft Playwright Testing
- Performance Testing
- Playwright Workspaces
- Test Automation
- Test Parallelization
- VS Code
- AI
- GitHub Copilot
- Azure
- DevOps
- Coding
- Community
section_names:
- ai
- github-copilot
- azure
- devops
- coding
primary_section: github-copilot
---
John_Stallo details Azure App Testing, a unified Azure Portal service enabling AI-powered load and end-to-end test automation. Learn how developers can use Playwright, JMeter, Locust, GitHub Copilot, and Azure-native scaling for application quality and performance.<!--excerpt_end-->

# Azure App Testing: Unified AI-Driven Load and E2E Testing in the Cloud

Azure App Testing is a new Azure Portal service enabling developer and QA teams to run large-scale functional and performance tests across popular frameworks like Playwright, JMeter, or Locust. This solution unifies two major testing capabilities—Azure Load Testing and Microsoft Playwright Testing—offering a single platform for resource provisioning, access control, and consolidated billing.

## Key Benefits

- **AI-Driven Testing:** Accelerate test authoring and insights using AI-powered tooling, including integrations with GitHub Copilot Agent mode.
- **Limitless Scale:** Simulate real-world traffic from multiple regions, run highly parallel, cross-browser end-to-end web tests.
- **Infrastructure-Free Execution:** Azure handles all provisioning, scaling, and maintenance.

## Core Capabilities

### Load Testing

- Use JMeter or Locust to simulate high-scale traffic and identify bottlenecks.
- Author and maintain tests in VS Code with [GitHub Copilot Agent mode](https://aka.ms/malt-vscode/get).
- Gain AI-powered insights for issue detection and remediation ([details](https://aka.ms/malt-insights/docs)).
- Run load tests targeting public and private endpoints, or initiate URL-based tests directly in the portal.

### Playwright Workspaces

- Develop and run scalable, cross-browser end-to-end tests without managing infrastructure.
- Utilize Playwright’s [VS Code extension](https://marketplace.visualstudio.com/items?itemName=ms-playwright.playwright), [Codegen](https://playwright.dev/docs/codegen), and [Playwright MCP](https://github.com/microsoft/playwright-mcp) with AI-assisted support.
- Execute tests in parallel on Windows or Linux, and all major browsers (Chromium, Firefox, WebKit).
- Integrate continuous testing into CI pipelines for fast feedback and early regression detection.

## Getting Started

1. Log into the **Azure Portal** and search for **Azure App Testing**.
2. Select the service and create workspaces for load or end-to-end testing as needed.
3. Start authoring and running tests at scale.

### Quickstarts

- [Create and run a load test with GitHub Copilot in VS Code](https://aka.ms/malt-vscode/get)
- [Run a URL-based load test](https://aka.ms/loadtesting/quickstart)
- [Integrate load and E2E tests with Azure Pipelines or GitHub Actions](https://aka.ms/loadtesting/quickstart-ci)

### Playwright Workspace Quickstarts

- [Scale E2E web tests](https://aka.ms/pww/docs/quickstart)
- [CI workflow integration](https://aka.ms/pww/docs/speed-up-ci)

## Migration and Guidance

- **Current Azure Load Testing users:** No immediate changes—existing resources and tests continue under the new hub.
- **Current Playwright Testing users:** Migrate workloads to Playwright Workspaces in Azure App Testing. See the [migration guide](https://aka.ms/mpt/migration-guidance).

## Learn More

- [Product page & pricing](https://aka.ms/azureapptesting/product)
- [Documentation](https://aka.ms/azureapptesting/docs)

*Author: John_Stallo, Microsoft*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-azure-app-testing-scalable-end-to-end-app-validation/ba-p/4440496)
