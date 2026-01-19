---
external_url: https://dellenny.com/end-to-end-confidence-in-the-cloud-a-walkthrough-of-azure-playwright-testing-preview/
title: Scaling Playwright End-to-End Tests with Azure Playwright Testing
author: Dellenny
viewing_mode: external
feed_name: Dellenny's Blog
date: 2025-09-25 08:36:33 +00:00
tags:
- Automation
- Azure CLI
- Azure Entra ID
- Azure Playwright Testing
- Browser Automation
- CI/CD Integration
- Cloud Testing
- Debugging
- End To End Testing
- Node.js
- Parallel Testing
- Playwright
- Test Automation
- Testing
- Testing Artifacts
section_names:
- azure
- coding
- devops
---
Dellenny guides readers through running Playwright end-to-end tests at scale with Azure Playwright Testing, covering setup, configuration, artifact storage, CI/CD pipeline integration, and cloud-scale debugging.<!--excerpt_end-->

# Scaling Playwright End-to-End Tests with Azure Playwright Testing

Author: Dellenny

## Overview

Running large Playwright test suites locally or in CI can quickly become slow and unreliable, especially as complexity grows. Azure Playwright Testing (Preview) — also known as Microsoft Playwright Testing — lets you scale browser tests in the cloud for better speed, stability, and insight without managing your own testing infrastructure.

This guide walks through:

- Setting up Azure Playwright Testing
- Connecting your project
- Running and tuning cloud-based tests
- Integrating with CI/CD
- Capturing and analyzing test artifacts

## What is Azure Playwright Testing?

Azure Playwright Testing is a managed testing service that runs Playwright browser tests in the Azure cloud. Key benefits include:

- **High parallelism** for faster feedback and reduced wait times
- **Multiple OS/browser coverage** without hardware setup
- **Automatic capture** of traces, screenshots, logs, and videos
- **Seamless integration** with CI/CD pipelines
- **Centralized portal** for results and debugging

## Prerequisites

You'll need:

- An Azure account with an active subscription
- Azure CLI (for local or CI usage)
- A Playwright project using `@playwright/test` (Node.js)

## Step 1: Create an Azure Playwright Testing Workspace

1. In the Azure portal, create a new **Playwright Testing workspace**
2. Assign name, subscription, and region
3. Note your workspace's region-specific **endpoint URL**

## Step 2: Add Service Support to Your Project

Run the following to initialize Azure Playwright Testing in your project:

```bash
npm init @azure/microsoft-playwright-testing@latest
```

This creates `playwright.service.config.ts` to link your project to Azure.

## Step 3: Configure the Endpoint URL

In your environment (.env), set:

```
PLAYWRIGHT_SERVICE_URL=wss://<region>.api.playwright.microsoft.com/accounts/<workspace-id>/browsers
```

Replace `<region>` and `<workspace-id>` with your values.

## Step 4: Authenticate

There are two main options:

- **Azure Entra ID**: (Recommended)
  - Run `az login`. Azure CLI credentials will be used automatically.
- **Access Tokens**: Generate and export a token as an environment variable.

## Step 5: Tune Service Configuration

Edit `playwright.service.config.ts` for service options. Example:

```typescript
import { defineConfig } from '@playwright/test';
import { getServiceConfig, ServiceOS } from '@azure/microsoft-playwright-testing';
import { AzureCliCredential } from '@azure/identity';
import baseConfig from './playwright.config';

export default defineConfig(
  baseConfig,
  getServiceConfig(baseConfig, {
    serviceAuthType: 'ENTRA_ID',
    os: ServiceOS.LINUX,
    useCloudHostedBrowsers: true,
    exposeNetwork: '<loopback>',
    timeout: 30000,
    credential: new AzureCliCredential(),
  }),
  {
    reporter: [
      ['list'],
      ['@azure/microsoft-playwright-testing/reporter', { enableGitHubSummary: true }],
    ],
  }
);
```

## Step 6: Enable Artifact Capture

Configure Playwright to capture traces, videos, and screenshots:

```javascript
use: {
  trace: 'on-first-retry',
  video: 'retain-on-failure',
  screenshot: 'on',
}
```

Artifacts appear in the Azure portal after runs.

## Step 7: Run Tests in Azure

Run:

```bash
npx playwright test --config=playwright.service.config.ts --workers=20
```

Results show up in your console and the Azure portal, including logs and artifacts.

## Step 8: Integrate with CI/CD

For GitHub Actions, Azure Pipelines, or any supported CI:

1. Set environment variables (`PLAYWRIGHT_SERVICE_URL`, tokens as needed)
2. Authenticate to Azure (federated identity or service principal)
3. Run tests as in local setup, gaining cloud scale

## Step 9: Optimize and Debug

- Adjust the number of workers for speed versus resource usage
- Use the portal to debug with traces, logs, and videos
- Tweak cloud features and reporting in your config

## Conclusion

Azure Playwright Testing helps teams run scalable, robust end-to-end tests with full artifact support and CI integration, while offloading browser infrastructure management.

---

For detailed steps and updates, visit the [original article](https://dellenny.com/end-to-end-confidence-in-the-cloud-a-walkthrough-of-azure-playwright-testing-preview/).

This post appeared first on "Dellenny's Blog". [Read the entire article here](https://dellenny.com/end-to-end-confidence-in-the-cloud-a-walkthrough-of-azure-playwright-testing-preview/)
