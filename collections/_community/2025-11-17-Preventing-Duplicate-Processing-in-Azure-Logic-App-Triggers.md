---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/duplicate-detection-in-logic-app-trigger/ba-p/4470365
title: Preventing Duplicate Processing in Azure Logic App Triggers
author: Mohammed_Barqawi
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-17 08:58:53 +00:00
tags:
- Azure Logic Apps
- Clienttrackingid
- Dataverse
- Duplicate Detection
- Implementation Guide
- Integration Services
- Logic App Standard
- Microsoft Learn
- Power Automate
- Resource Permissions
- REST API
- SharePoint
- Trigger Management
- Workflow Automation
section_names:
- azure
- devops
---
Mohammed_Barqawi presents a solution for detecting and handling duplicate trigger executions in Azure Logic Apps by leveraging the Logic Apps REST API and a unique clientTrackingId.<!--excerpt_end-->

# Preventing Duplicate Processing in Azure Logic App Triggers

## Overview

In certain integration scenarios, Azure Logic Apps may unintentionally process the same item more than once, leading to undesirable duplicated operations. This commonly happens due to events such as:

- **SharePoint Polling**: Triggers fire more than once for the same file due to repeated edits.
- **Dataverse Webhook**: Triggers from updates by plugins or other system actions.

Reference: [Microsoft SharePoint Connector for Power Automate | Microsoft Learn](https://learn.microsoft.com/en-us/sharepoint/dev/business-apps/power-automate/sharepoint-connector-actions-triggers#flow-runs)

## Solution Approach

To mitigate this, the author introduces a method using the [Logic Apps REST API](https://learn.microsoft.com/en-us/rest/api/logic/workflow-runs/list?view=rest-logic-2019-05-01&tabs=HTTP) to check for historic workflow executions within a time window, uniquely identified by a `clientTrackingId`.

### Key Steps

#### 1. Identify the clientTrackingId

For SharePoint scenarios, the file name is extracted as the unique identifier using:

```text
@string(trigger()['outputs']['body']['value'][0]['{FilenameWithExtension}'])
```

#### 2. Pass Execution Payload to Duplicate Detector Flow

The payload sent includes:

- **clientTrackingId**: A unique identifier (e.g., file name)
- **Resource URI**: Points to the SharePoint flow
- **Time Window**: Duration to look back for duplicates

#### 3. Implement Duplicate Detector Logic

- The detector uses the Logic Apps REST API to retrieve workflow run history for the identifier within the specified time window.
- If more than one run exists, a duplicate is detected and the process can return an error status.

Example Condition:

```text
if(greater(length(body('HTTP-Get_History')['value']), 1), '400', '200')
```

## Important Notes

- The Duplicate Detector Flow requires **Logic Apps Standard Reader (Preview)** permission at the resource group level to retrieve run history.
- The solution is built on **Logic App Standard** but can be adjusted for Logic App Consumption or Power Automate platforms.

## Further Resources

- [GitHub: Logic Apps Duplicate Detector](https://github.com/mbarqawi/logicappfiles/tree/main/Logic%20Apps%20Duplicate%20Detector)

---
**Author:** Mohammed_Barqawi  
**Published:** Nov 17, 2025

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/duplicate-detection-in-logic-app-trigger/ba-p/4470365)
