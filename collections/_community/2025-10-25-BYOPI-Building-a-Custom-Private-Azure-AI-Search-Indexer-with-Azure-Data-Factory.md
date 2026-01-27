---
external_url: https://techcommunity.microsoft.com/t5/azure/byopi-design-your-own-custom-private-ai-search-indexer-with-no/m-p/4464205#M22283
title: 'BYOPI: Building a Custom Private Azure AI Search Indexer with Azure Data Factory'
author: ani_ms_emea
feed_name: Microsoft Tech Community
date: 2025-10-25 15:57:45 +00:00
tags:
- ADF Pipeline
- Azure AI Search
- Azure Data Factory
- BYOPI
- Compliance
- CRUD
- Data Integration
- Data Synchronization
- Indexing
- Key Vault
- Network Security
- No Code
- Pipeline Monitoring
- PowerShell
- Private Endpoint
- REST API
- Self Hosted Integration Runtime
- SQL Server
- VNet
section_names:
- ai
- azure
- security
primary_section: ai
---
ani_ms_emea explains how to implement a secure, end-to-end private indexing solution for Azure AI Search using Azure Data Factory and SQL Server on a private VM, highlighting both practical steps and architectural challenges.<!--excerpt_end-->

# BYOPI: Building a Custom Private Azure AI Search Indexer with Azure Data Factory

## Executive Summary

Building a private search indexer—syncing SQL Server data from a private VM to Azure AI Search via Azure Data Factory (ADF)—is possible but brings notable technical challenges, limitations, and higher costs. This article documents a real-world BYOPI (Build Your Own Private Indexer) journey, sharing honest lessons, architectural steps, and recommendations.

## Table of Contents

1. Overall Setup
2. How ADF Works with Azure AI Search
3. Challenges Discovered
4. Pros and Cons: An Honest Assessment
5. Conclusion and Recommendations

---

## 1. Overall Setup

### Stepwise Process

- **Resource Group & vNET:** Create resource group and virtual network (any region).
- **SQL Server VM:** Deploy SQL Server in a private VM (no public IP).
- **Azure Services:** Provision Azure Data Factory, Azure AI Search, and Azure Key Vault (AKV).  
- **Private Endpoints:** Set up private endpoints for each Azure service, each in a dedicated subnet.
- **SQL Server Setup:** Connect via Bastion host, deploy databases, tables, and stored procedures (e.g., `sp_update_watermark`), as per sample T-SQL scripts:

```sql
CREATE DATABASE BYOPI_DB;
USE BYOPI_DB;
CREATE TABLE Products (...);
CREATE TABLE WatermarkTable (...);
CREATE PROCEDURE sp_update_watermark ...;
INSERT INTO Products (...);
```

- **Install Self-Hosted Integration Runtime (SHIR):**
  - In ADF Portal, create SHIR, copy registration key.
  - On VM, download and install Integration Runtime, register using the key.

- **Create Search Index (PowerShell):**
  - Build and apply index schema using Invoke-RestMethod and relevant search API calls.

- **Configure AKV & ADF Components:**
  - Store SQL & search secrets in Key Vault; link AKV to ADF as a Linked Service.
  - Create Linked Services for SQL Server (via SHIR) and Azure Search using keys from Key Vault.

- **Create Datasets and Pipeline:**
  - Create datasets for SQL tables (`Products`, `WatermarkTable`) and Azure Search index.
  - Design pipeline:
    - Lookup activities for watermark logic
    - Copy activity for syncing data
    - Stored Procedure activity to update watermark
    - Link activities in correct order (success conditions)

- **Testing & Scheduling:**
  - Test with debug runs; monitor pipeline output.
  - Schedule with triggers (e.g., hourly recurrence).

- **Validation:**
  - Validate DNS using `nslookup` on VM, ensuring use of private IPs.
  - Test data syncs and search queries using PowerShell/REST.

---

## 2. How ADF Works in This Approach with Azure AI Search

- ADF acts as orchestrator, extracting data from SQL Server and pushing it to Azure AI Search using REST API POST calls (batching enabled).
- The pipeline uses watermark (timestamp) logic to efficiently sync only new/modified records, limiting reprocessing.
- Special field `@search.action` (upload, merge, delete, etc.) controls desired operation per record.
- ADF batches changes; after successful sync, watermark is advanced.
- However, while "upload" and "merge" work natively, **delete operations are NOT supported in the native ADF Azure Search sink**—even when using proper action fields. Workarounds require ADF Web Activity or REST calls.

### Example REST Structure

```json
{
  "value": [
    { "@search.action": "upload", ... },
    { "@search.action": "delete", "id": "123" }
  ]
}
```

---

## 3. Challenges Discovered

**Technical Hurdles:**

- Complex networking (private endpoints, SHIR, Bastion).
- No direct portal access; must use VM/Bastion/VPN.
- Debugging is tough; public tools aren't usable.
- **Major gotcha:** Delete operations aren't respected by ADF Azure Search sink; documentation is unclear.
- Must use workaround: invoke REST API with custom Web Activity.
- ADF and Azure Search costs are higher than expected.
- SHIR currently needs Windows VM (Linux is only in preview).
- DNS propagation for private endpoint can cause setup delays.

**Example Cost Structure:**

- VM: ~$150/mo
- Private Endpoints: ~$30/mo
- ADF: $15-60/mo
- AI Search: ~$75/mo
- **Total:** $270-315+/mo

---

## 4. Pros and Cons: An Honest Assessment

### Pros

- Total network isolation (improves security & compliance)
- Configuration-based; minimal code
- Scalable to large datasets
- Built-in monitoring
- Managed/maintained by Microsoft

### Cons

- Delete support missing in native ADF sink
- Setup and debugging is complex
- Higher-than-expected costs
- Requires Azure and networking expertise
- SHIR/endpoint limitations

### Hidden Gotchas

- SHIR auto-update can break pipelines
- Private endpoint DNS delays
- ADF Studio timeouts in private mode

---

## 5. Conclusion and Recommendations

**When to Use This BYOPI Pattern:**

- High-security/compliance requirements (No public IP/data egress)
- Indexing from data sources not natively supported by Azure Search
- Larger teams with network expertise and high budgets

**When Not to Use:**

- Simple/straightforward search needs
- Limited budgets
- Real-time or heavy delete operations required
- Teams lacking deep Azure skillset

**Key Takeaways:**

1. The solution is robust but complex.
2. DELETE (hard) operations need custom REST handling.
3. Total costs are meaningfully higher once private endpoints are included.
4. Substantial setup/testing time is required.
5. Evaluate alternative architectures where possible.

---

> The scripts and approaches are provided as-is. Always verify against latest Azure documentation and test thoroughly in development before production deployment.

Thanks for reading! This guide aims to clarify pitfalls and guide technical teams through setting up a private, Azure-native search indexing pipeline.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/byopi-design-your-own-custom-private-ai-search-indexer-with-no/m-p/4464205#M22283)
