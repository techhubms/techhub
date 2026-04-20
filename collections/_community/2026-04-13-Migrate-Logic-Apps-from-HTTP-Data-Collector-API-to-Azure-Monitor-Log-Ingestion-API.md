---
feed_name: Microsoft Tech Community
tags:
- 403 Forbidden
- API Version 01 01
- Azure
- Azure CLI
- Azure Monitor
- Community
- Custom Log Tables
- Data Collection Endpoint (dce)
- Data Collection Rule (dcr)
- HTTP Action
- HTTP Data Collector API
- IAM
- Log Analytics
- Log Ingestion API
- Logic Apps
- Managed Identity
- Monitoring Metrics Publisher
date: 2026-04-13 08:06:40 +00:00
author: mshboul
section_names:
- azure
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/migrate-data-ingestion-from-data-collector-to-log-ingestion/ba-p/4510493
title: Migrate Logic Apps from HTTP Data Collector API to Azure Monitor Log Ingestion API
primary_section: azure
---

mshboul explains how Logic Apps users can migrate from the deprecated Log Analytics HTTP Data Collector API to the Azure Monitor Log Ingestion API, including how to create DCE/DCR resources, build the ingestion URL, assign the right IAM role to a managed identity, and send data via an HTTP action.<!--excerpt_end-->

# Migrate Logic Apps from HTTP Data Collector API to Azure Monitor Log Ingestion API

## Why you need to migrate

The **HTTP Data Collector API** for **Log Analytics workspaces** is being deprecated and will be **out of support in September 2026**.

Key behavior changes called out:

- Existing Logic Apps **Data Collector** actions that use already-created API connections (workspace **ID & Key**) may still work for **old custom log tables**.
- For **newly created custom log tables**, ingestion may silently fail:
  - The Logic Apps connector action can still show **success**, but **no data** appears in the new custom logs.
- If you create a **new API connection** for the Data Collector action (workspace **ID & Key**), calls can fail with **403 Forbidden**.

Because of this, you should move to the **Azure Monitor Log Ingestion API** for sending data to custom tables.

> Note: The post mentions the Azure portal UI may not show workspace keys on the Log Analytics workspace page, while **Azure CLI** can still retrieve keys. However, those keys won’t help for new Data Collector connections if they fail with 403.

## Prerequisites: Create DCE and DCR

To use Log Ingestion API you need:

- A **Data Collection Endpoint (DCE)**
- A **Data Collection Rule (DCR)**

### Create a Data Collection Endpoint (DCE)

In the Azure portal:

1. Search for **DCE**
2. Create a new **Data Collection Endpoint**

### Create a Data Collection Rule (DCR)

You can create the DCR either:

- Directly from the **DCR** page in the Azure portal, or
- While creating the **custom log** in the Log Analytics workspace

During DCR creation, you’ll upload **sample data** so the custom log table gets a schema:

- The sample needs to be a **JSON array**.
- If the sample doesn’t contain a `TimeGenerated` field, you can add it using a **mapping function** in the DCR transformation.

The flow described:

1. Upload sample JSON array
2. Add transformation code in the **Transformation** box
3. Click **Run**

## Build the full Log Ingestion URL

After the DCR is created, build the full ingestion URL by combining values from the DCE and DCR.

### Step 1: Get the DCE base ingestion URL

From the **DCE overview** page, copy the **Log Ingestion URL**.

### Step 2: Get the DCR immutable ID

On the **DCR** resource:

1. Find the **immutable ID**
2. Open the **JSON view** of the DCR resource

### Step 3: Get the stream name

From the DCR JSON view, locate the stream name in:

- `streamDeclarations`

### Final URL format

Construct the endpoint like this:

`DCE_URL/dataCollectionRules/{immutable_id}/streams/{streamName}?api-version=2023-01-01`

Example shown in the post:

`https://mshbou****.westeurope-1.ingest.monitor.azure.com/dataCollectionRules/dcr-7*****4e988bef2995cd52ae/streams/Custom-mshboulLogAPI_CL?api-version=2023-01-01`

## Assign IAM permissions to the Logic App managed identity

To call the ingestion endpoint using a **Logic Apps managed identity (MI)**:

- Grant the Logic App MI the **Monitoring Metrics Publisher** role **on the DCR resource**.

Steps:

1. Open the **DCR** in Azure portal
2. Go to **Access Control (IAM)**
3. Add role assignment: **Monitoring Metrics Publisher**
4. Select your Logic App’s **managed identity**

## Call the Log Ingestion API from Logic Apps

Instead of the Data Collector connector action, use an **HTTP** action.

Configure the HTTP action:

- **URI**: the full Log Ingestion URL built earlier
- **Headers**: include `Content-Type` (as described, use JSON)
- **Body**: the JSON payload representing the log data you want to send

Authentication:

- Use the Logic App **managed identity** (as shown in the post’s screenshots)

Expected result:

- Request succeeds with **HTTP 204**.

## Official documentation links

- [Migrate from the HTTP Data Collector API to the Log Ingestion API - Azure Monitor | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/custom-logs-migrate)
- [Logs Ingestion API in Azure Monitor - Azure Monitor | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/logs-ingestion-api-overview)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/migrate-data-ingestion-from-data-collector-to-log-ingestion/ba-p/4510493)

