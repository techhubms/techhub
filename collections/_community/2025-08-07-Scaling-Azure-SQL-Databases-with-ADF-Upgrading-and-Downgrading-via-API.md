---
external_url: https://www.reddit.com/r/AZURE/comments/1mjtpn1/adf_scale_up_and_scale_down_azure_sql_database/
title: 'Scaling Azure SQL Databases with ADF: Upgrading and Downgrading via API'
author: Relative_Wear2650
viewing_mode: external
feed_name: Reddit Azure
date: 2025-08-07 07:35:40 +00:00
tags:
- ADF Pipelines
- API Versioning
- Automation
- Azure Data Factory
- Azure REST API
- Azure SQL Database
- Cloud Cost Optimization
- Database SKU Scaling
- PaaS
- Resource Management
- SQL Serverless
- Transparent Data Encryption
section_names:
- azure
---
Relative_Wear2650 and the community discuss how to use Azure Data Factory to programmatically scale Azure SQL Databases by changing their service tier via API, including API selection and automation best practices.<!--excerpt_end-->

# Scaling Azure SQL Databases with ADF: Automating Upgrades and Downgrades

**Author: Relative_Wear2650**

## Situation Overview

- An existing Azure Data Factory (ADF) master pipeline orchestrates multiple individual pipelines.
- Before processing data, it scales up the Azure SQL Database (sink) by calling the Azure Management API via a Web activity:
  - `PUT https://management.azure.com/subscriptions/[subscription]/resourceGroups/[resource group]/providers/Microsoft.Sql/servers/[sql server]/databases/[database]?api-version=2021-02-01-preview`
  - Body example: `{ "sku": { "name": "S6", "tier": "Standard" }, "location": "[location]" }`
- At the end of the pipeline, it scales down the same database similarly:
  - Body example: `{ "sku": { "name": "S0", "tier": "Standard" }, "location": "[location]" }`

## Key Questions and Answers

### 1. **Which API version should I use to change Azure SQL DB SKUs?**

- The API version in use, `2021-02-01-preview`, is outdated.
- Microsoft maintains a [list of current API versions](https://learn.microsoft.com/en-us/rest/api/sql/databases/create-or-update?tabs=HTTP) for Azure SQL resources. As of this writing, a stable option is `2022-02-01-preview`, or use the latest non-preview if available.
- Example update:

  ```
  PUT https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Sql/servers/{serverName}/databases/{databaseName}?api-version=2022-02-01-preview
  ```

- Always check official Azure REST API documentation for updates: https://learn.microsoft.com/en-us/rest/api/sql/

### 2. **Does this scale the database to a different tier/size?**

- Yes. The body `{ "sku": { "name": "S6", "tier": "Standard" }}` changes the service tier for your database, effectively allocating more resources.
- At the end, reverting to `{ "sku": { "name": "S0", "tier": "Standard" }}` returns the database to a lower, less expensive tier.
- This pattern is valid for managing performance and cost.

### 3. **Is this a best practice?**

- **Caveats:**
  - Scaling operations are **asynchronous** (can take minutes to complete).
  - If your pipeline continues before scaling completes, queries may fail or perform poorly.
  - Waiting for scaling can also increase ADF pipeline run costs.
- **Recommendations:**
  - Use code/automation (such as Azure Functions or PowerShell in Azure Automation) to issue the scaling command, then poll the database status to ensure the change is complete before proceeding.
  - Consider whether serverless SQL or elastic pools are a better architectural fit (these options can scale on-demand or pool resources with less management overhead).
  - There is no built-in autoscale for dedicated Azure SQL Database tiers; scaling must be managed explicitly.
  - For serverless databases: To "wake up" from pause, a simple query or checking the Transparent Data Encryption (TDE) status (a lightweight operation) triggers resume, but up/down tiering is separate.

### 4. **Detecting Autoscale**

- Classic single Azure SQL Database tiered offerings do **not** have true autoscale. To check:
  - Review the database SKU (in the Azure Portal or via CLI/PowerShell).
  - If it's "Serverless" (vCore), it will have auto-pause/on-demand; otherwise, it's manually managed.

## Takeaways

- Update API versions for future-proofing.
- Account for async nature of scaling—ensure your pipeline waits for completion.
- Weigh operational complexity versus cost savings; sometimes provisioning for peak or using elastic pools/serverless makes more sense.
- Monitor the Azure documentation for new capabilities.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mjtpn1/adf_scale_up_and_scale_down_azure_sql_database/)
