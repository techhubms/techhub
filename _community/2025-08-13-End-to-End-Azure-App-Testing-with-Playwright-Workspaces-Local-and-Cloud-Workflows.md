---
layout: "post"
title: "End-to-End Azure App Testing with Playwright Workspaces: Local and Cloud Workflows"
description: "This community guide by varghesejoji provides a detailed walkthrough on using Azure App Testing with Playwright Workspaces for scalable browser-based app testing. It covers local setup, integration with Azure Web Apps, scripting, troubleshooting, and leveraging cloud-hosted browsers for parallel test execution. Emphasis is placed on configuration, practical NPM scripts, and workflows to optimize functional and performance testing in development and CI pipelines."
author: "varghesejoji"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-app-testing-playwright-workspaces-for-local-to-cloud-test/ba-p/4442711"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-13 03:43:11 +00:00
permalink: "/2025-08-13-End-to-End-Azure-App-Testing-with-Playwright-Workspaces-Local-and-Cloud-Workflows.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["App Deployment", "Automated Testing", "Azure", "Azure App Testing", "Azure Portal", "Azure Web App", "Browser Automation", "CI/CD", "Cloud Testing", "Coding", "Community", "Continuous Integration", "DevOps", "Express", "Functional Testing", "JavaScript", "Microsoft Azure", "Node.js", "npm Scripts", "Performance Testing", "Playwright", "Playwright Workspaces", "Test Parallelism", "Test Reporting", "Testing Configuration"]
tags_normalized: ["app deployment", "automated testing", "azure", "azure app testing", "azure portal", "azure web app", "browser automation", "cislashcd", "cloud testing", "coding", "community", "continuous integration", "devops", "express", "functional testing", "javascript", "microsoft azure", "nodedotjs", "npm scripts", "performance testing", "playwright", "playwright workspaces", "test parallelism", "test reporting", "testing configuration"]
---

varghesejoji demonstrates how to use Azure App Testing and Playwright Workspaces for browser-based functional and performance testing, guiding users from local development to cloud-scale parallel test execution with shared reporting.<!--excerpt_end-->

# End-to-End Azure App Testing with Playwright Workspaces: Local and Cloud Workflows

Azure App Testing provides developers with a centralized hub for both functional and performance testing of web applications. Integrating Playwright Workspaces allows you to seamlessly author, run, and analyze browser tests at scale—whether on your local machine or in the cloud, with parallelization and unified reporting. This guide by varghesejoji walks you through the entire process, from cloning a sample Node.js app to running scalable cloud-based tests in Azure.

## Quick Start Resources

- [Overview of Playwright Workspaces](https://learn.microsoft.com/en-us/azure/app-testing/playwright-workspaces/overview-what-is-microsoft-playwright-workspaces)
- [QuickStart Guide](https://learn.microsoft.com/en-us/azure/app-testing/playwright-workspaces/quickstart-run-end-to-end-tests?tabs=playwrightcli&pivots=playwright-test-runner)

## Creating a Playwright Workspace

1. In the Azure portal, search for **Azure App Testing** and select **Create**.
2. Choose **Playwright Workspaces** (select **Azure Load Testing** for performance/load if needed).
3. Enter your preferred name, region, and subscription. Complete the creation process.
4. Use the workspace to author and run tests—locally or with cloud-hosted browsers. Test results are visible in the **Test runs** section.

**Tip:** Workspaces let you publish test results and artifacts, and run tests with high parallelism for faster feedback cycles.

## Guided Test Workflows

**This guide covers:**

- Running Playwright tests locally with an auto-started dev server
- Testing against a deployed Azure Web App
- Publishing test runs to the Playwright Workspace (with optional cloud-hosted browser parallelism)

### Sample Application Under Test

- **Tech Stack:** Node.js + Express (ESM)
- **Functionality:** Minimal Todo page as primary test target
- **Repository:** [Clone here](https://github.com/jvargh/azure-playwright-testing)

#### 1. Clone and Install Dependencies

```sh
git clone https://github.com/jvargh/azure-playwright-testing.git
cd azure-playwright-testing
npm install

# Install Playwright browsers (optional)

npx playwright install
```

#### 2. App Structure

- **Entry Point:** `./server.js`
- **Default Port:** `process.env.PORT || 3000` (supports Azure deployment)
- **Routes:**
  - `GET /` → Welcome page
  - `GET /todomvc` → TodoMVC-style page (localStorage-backed)

_The minimalistic design helps focus on end-to-end test flow (local → Azure Web App → Workspace)._

### Key Configuration Files

- `playwright.config.ts` – Base config: projects, reporters, etc.
- `playwright.local.config.ts` – For local dev (auto server on `localhost:3000`)
- `playwright.service.config.ts` – Azure Workspace integration: publishes runs/results

_Refer to the Microsoft QuickStart for cloud-region endpoints and integration details._

### Essential NPM Scripts

- `start:src` → start the local Node.js server
- `test:local` → run tests locally via `playwright.local.config.ts`
- `test:azure` → test against a deployed Azure Web App URL
- `test:workspace` → publish runs and artifacts to Azure Workspace
- `test:workspace:scale` → same as above, but with increased workers for cloud-scale parallelism

**View reports locally:**

```sh
npx playwright show-report
```

## End-to-End Playwright Testing Sequences

### 1) Local Testing (with Dev Server)

- **Why:** Rapid dev feedback loop; Playwright auto-starts and targets `http://localhost:3000`.
- **Command:**

  ```sh
  npm run test:local
  # Alternatively
  npx playwright test -c playwright.local.config.ts --reporter=line
  ```

- **Outcome:** Starts the server, runs tests, reuses for next runs.
- **Manual execution:**

  ```sh
  node ./server.js
  set BASE_URL=http://localhost:3000 && npx playwright test -c playwright.config.ts --reporter=line
  ```

- **Target single test (Windows):**

  ```sh
  npx playwright test -c playwright.local.config.ts tests-examples\demo-todo-app.spec.ts -g "should allow me to add todo items" --reporter=line
  ```

### 2) Testing Deployed Azure Web App

- **Prerequisite:** App must be deployed and accessible at its Azure URL.
- **Command Examples:**

  ```sh
  set BASE_URL=https://<app>.azurewebsites.net && npm run test:azure
  # Cross-platform variant with cross-env
  npm run test:azure --baseurl=https://<app>.azurewebsites.net
  ```

- **Outcome:** Reuses base Playwright config, targets the production/staging site via `BASE_URL`.

### 3) Publishing to Playwright Workspace & Running at Cloud Scale

- **Authenticate and set cloud region:**

  ```sh
  az login
  set PLAYWRIGHT_SERVICE_URL=https://<region>.api.playwright.microsoft.com
  ```

- **Run and publish results:**

  ```sh
  npm run test:workspace
  # Or, for cloud-scale parallelism:
  npm run test:workspace:scale
  # Or raw:
  npx playwright test -c playwright.service.config.ts --reporter=line
  ```

- **Outcome:** Results and artifacts published to the Azure portal; parallel runs available on cloud-hosted browsers.

## Troubleshooting

- **BASE_URL not working?**
  - Make sure you're setting the correct config and environment variable in the same shell/session.
- **Workspace runs missing?**
  - Re-run `az login`, check `PLAYWRIGHT_SERVICE_URL`, and review workspace region/roles.
- **Slow runs?**
  - Increase the number of parallel workers judiciously for optimal speed; find project-specific balance.

## Conclusion

This workflow provides a repeatable development-to-production test pipeline:

- Develop and test fast locally
- Validate against live Azure Web Apps
- Publish and parallelize using Azure Playwright Workspaces

By following this guide, teams can maintain reliable, centralized test reporting and optimize feedback cycles at every stage: development, staging, and continuous integration.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-app-testing-playwright-workspaces-for-local-to-cloud-test/ba-p/4442711)
