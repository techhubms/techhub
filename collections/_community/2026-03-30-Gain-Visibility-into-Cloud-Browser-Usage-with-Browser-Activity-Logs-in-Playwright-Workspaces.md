---
title: Gain Visibility into Cloud Browser Usage with Browser Activity Logs in Playwright Workspaces
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/gain-visibility-into-cloud-browser-usage-with-browser-activity/ba-p/4506706
section_names:
- azure
- devops
date: 2026-03-30 05:43:33 +00:00
feed_name: Microsoft Tech Community
primary_section: azure
author: AbhinavPremsekhar
tags:
- Azure
- Azure App Testing
- Azure Portal
- Billable Time
- Browser Activity Logs
- Browser Automation Tool
- Browser Sessions
- Cloud Browsers
- Community
- Conversation ID
- Cost Governance
- DevOps
- Filtering
- Observability
- Playwright Test Runs
- Playwright Workspaces
- Session Lifecycle
- Source ID
- Test Automation
- Traceability
- Usage Metrics
---

AbhinavPremsekhar introduces Browser Activity Logs for Playwright Workspaces, explaining how to use the Azure portal to track cloud browser sessions, correlate them back to test runs/tools, and understand billable browser time.<!--excerpt_end-->

## Overview

**Browser Activity Logs** in **Playwright Workspaces** adds a centralized view of usage for cloud-hosted browser sessions. The goal is to improve:

- **Visibility** into every session created
- **Traceability** back to the initiating test run/tool
- **Cost transparency** via billable time tracking

The logs are accessible in the **Azure portal**.

## What is a browser session?

In **Playwright Workspaces**, a **Browser Session** is any browser instance provisioned by the service, no matter how it was initiated, including:

- Playwright Workspaces test run
- Browser Automation Tool
- Other automation clients that connect to the workspace

Whenever a browser is requested, Playwright Workspaces creates a Browser Session and records it in Browser Activity Logs.

## What are Browser Activity Logs?

The **Browser Activity Logs** page tracks the lifecycle of each browser session (creation to completion). For each session, it surfaces:

- **Session Name / ID** – unique identifier
- **Start Time** – when the session started
- **End Time** – when the session ended
- **Billable Time** – total billable duration
- **Source Type** – the initiating client type:
  - Playwright Workspaces test run
  - Browser automation tool
  - Others
- **Source ID** – identifier for the initiating client:
  - Test run ID
  - Conversation ID
- **Status** – current state:
  - Created
  - Active
  - Completed
  - Failed
- **Browser Type** – which browser was used
- **Operating System** – OS used by the browser
- **Creator Name / ID** – user who initiated the session

## Filtering and analyzing browser usage

Built-in filters let you slice usage by:

- **Time range** (last 30/60/90 days)
- **Source type** (test runs, automation tools, other sources)
- **Source ID** (test run ID / conversation ID)
- **Status** (Created, Active, Completed, Failed)

This supports questions like:

- How many browser sessions were created by a specific test run?
- Which automation scenarios consumed the most browser time?
- How much billable browser time was used over a period?

## How to view Browser Activity Logs in the Azure portal

### Prerequisites

You need:

- An Azure account with an active subscription
- Owner, Contributor, or classic administrator role on the subscription
- A Playwright Workspace in your subscription
- Reader, Contributor, or Owner access to the Playwright Workspace

### Steps

1. Sign in to the [Azure portal](https://portal.azure.com).
2. Search for and select **Azure App Testing**.
3. In the Azure App Testing hub, select **View resources** under **Playwright Workspaces**.
4. Open your Playwright Workspace.
5. In the left navigation pane, select **Browser session → Browser activity log**.
6. Select a browser session to see additional details.
7. Filter by **Source ID**:
   - Enter a **Playwright Workspaces test run ID** to view sessions for a specific test run.
   - Enter a **Foundry conversation ID** to view sessions created by a browser automation tool.
8. Optionally, filter by **Source type** to view all sessions from a specific client.

## Why this matters

Browser Activity Logs is positioned as a way to:

- See every browser session created across testing and automation
- Correlate sessions back to test runs/tools/conversations
- Track billable time for cost awareness as usage scales

## Links

- Playwright Workspaces product docs: https://aka.ms/pww/docs/product
- Quickstart: https://aka.ms/pww/docs/quickstart
- Browser Automation Tool docs: https://aka.ms/bat/docs
- Feedback: https://aka.ms/pww-feedback

## Metadata

- Updated: Mar 29, 2026
- Version: 2.0


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/gain-visibility-into-cloud-browser-usage-with-browser-activity/ba-p/4506706)

