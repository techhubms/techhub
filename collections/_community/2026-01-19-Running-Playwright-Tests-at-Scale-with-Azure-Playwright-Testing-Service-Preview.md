---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-playwright-testing-service-preview-run-playwright-tests-on/ba-p/4487103
title: Running Playwright Tests at Scale with Azure Playwright Testing Service (Preview)
author: Adyshasnotes
feed_name: Microsoft Tech Community
date: 2026-01-19 11:44:18 +00:00
tags:
- Access Token
- API Testing
- Azure Playwright Testing
- CI/CD
- Cloud Browsers
- Debugging
- End To End Testing
- HTML Reports
- Node.js
- Playwright
- Test Automation
- Testing Pipeline
- TypeScript
- VS
- VS Code
- Azure
- DevOps
- Community
- .NET
section_names:
- azure
- dotnet
- devops
primary_section: dotnet
---
Adyshasnotes walks you through leveraging Azure Playwright Testing Service (Preview) to execute Playwright tests on cloud-hosted browsers using TypeScript, with step-by-step setup focused on Visual Studio/VS Code for modern DevOps workflows.<!--excerpt_end-->

# Azure Playwright Testing Service (Preview): Run Playwright Tests on Cloud Browsers

By Adyshasnotes

## Overview

Azure Playwright Testing Service (Preview) provides a managed, cloud-hosted environment for running Playwright UI and API tests at scale, eliminating local browser management. This guide explains how to set up the service, configure your test project, and analyze results—all with practical, hands-on instructions using Visual Studio, VS Code, and TypeScript.

---

## Prerequisites

- Active Azure subscription
- Node.js installed
- Playwright project (TypeScript)
- Visual Studio or Visual Studio Code

---

## Step 1: Create a Playwright Workspace in Azure Portal

1. Navigate to **Azure Portal → Microsoft Playwright Testing (Preview)**
2. Choose your subscription and region
3. Create a new workspace to serve as your remote testing execution environment

---

## Step 2: Create and Prepare Your Playwright Project Locally

- Set up your Playwright project in Visual Studio or VS Code
- Organize your tests under a `tests/` folder (e.g., `tests/example.spec.ts`)
- Your code and tests will be run using Azure-hosted browsers

---

## Step 3: Add Playwright Service Configuration

1. Download the Playwright service configuration (`playwright.service.config.ts`) from the Azure portal (link: https://aka.ms/mpt/service-config)
2. Place this file in the same directory as your `playwright.config.ts`

---

## Step 4: Generate Access Token and Service URL

- In the Playwright Testing portal, generate a new access token and copy the region-specific service URL. These credentials will authenticate your automated test runs.

---

## Step 5: Configure Environment Variables

- In your project directory, create a `.env` file with:

  ```
  PLAYWRIGHT_SERVICE_ACCESS_TOKEN=<your_access_token>
  PLAYWRIGHT_SERVICE_URL=<your_region_endpoint>
  ```

- This configuration securely connects your test runner to Azure Playwright Testing Service.

---

## Step 6: (Optional) Install Playwright VS Code Extension

- Enhance your developer workflow by installing the [Playwright Test extension](https://marketplace.visualstudio.com/items?itemName=ms-playwright.playwright) for VS Code.

---

## Step 7: Run Tests Remotely on Cloud Browsers

To run an individual test file:

```bash
npx playwright test example.spec.ts --config=playwright.service.config.ts
```

To run the full test suite in parallel:

```bash
npx playwright test --config=playwright.service.config.ts --workers=20
```

To open generated HTML reports:

```bash
npx playwright show-report
```

---

## Step 8: View and Analyze Test Results in Azure Portal

- Refresh your Playwright Testing workspace in the Azure portal to see test executions, runtimes, browser activity, logs, and step-by-step error diagnosis.
- HTML reports provide details on all test outcomes for easier analysis.

---

## Trace and Debug

- Use Playwright Trace Viewer to visualize and troubleshoot test execution details, including UI actions, timings, and DOM states.

---

## API Testing and Combined Pipelines

- Define and execute API tests within the same Playwright project, leveraging the Azure service's ability to handle UI and API checks in a single CI/CD workflow

---

## Visual Studio Integration

- Install `dotenv` to manage your test environment:

  ```bash
  npm install --save-dev dotenv
  ```

- Launch the suite as above and monitor results directly through the Azure portal ([portal link](https://aka.ms/mpt/portal))

---

## Next Steps

- Use parallel execution to speed up test runs
- Integrate traces for detailed troubleshooting in CI
- Expand to full CI/CD pipelines for end-to-end validation
- Combine UI and API tests for comprehensive automation

---

## Additional Resources

- [Playwright Service Config Docs](https://aka.ms/mpt/service-config)
- [Playwright VS Code Extension](https://marketplace.visualstudio.com/items?itemName=ms-playwright.playwright)
- [Azure Playwright Testing Portal](https://aka.ms/mpt/portal)

---

**Have you tried Azure Playwright Testing Service? Share your feedback!**

Published by Adyshasnotes | Jan 19, 2026 | Version 1.0

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-playwright-testing-service-preview-run-playwright-tests-on/ba-p/4487103)
