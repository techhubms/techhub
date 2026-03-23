---
author: Microsoft Fabric Blog
primary_section: ml
feed_name: Microsoft Fabric Blog
section_names:
- azure
- devops
- ml
- security
title: Microsoft Fabric Bulk Import/Export Item Definitions APIs (Preview)
tags:
- Azure
- Azure Active Directory
- Azure Blob Storage
- Base64 Encoding
- Bulk Export
- Bulk Import
- CI/CD
- Delegated Access
- DevOps
- DevOps Pipelines
- Exponential Backoff
- Fabric REST API
- Git Integration
- Item Definitions
- JSON Manifest
- KQL Dashboard
- Long Running Operations
- LRO Pattern
- Microsoft Entra ID
- Microsoft Fabric
- ML
- News
- Rate Limiting
- Security
- Semantic Model
- Service Principal
- Workspace Migration
external_url: https://blog.fabric.microsoft.com/en-US/blog/public-apis-bulk-import-and-export-items-definition-preview/
date: 2026-03-23 11:30:00 +00:00
---

Microsoft Fabric Blog introduces the preview Bulk Import & Export APIs for moving, backing up, and deploying Fabric item definitions (reports, notebooks, semantic models, pipelines, and more) via the Fabric REST API, including CI/CD scenarios, permissions, and long-running operation behavior.<!--excerpt_end-->

# Microsoft Fabric Bulk Import/Export Item Definitions APIs (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all FabCon and SQLCon announcements across both Fabric and database offerings:* https://aka.ms/FabCon-SQLCon-2026-news

## What are bulk import and export APIs?

Every Fabric item—whether a **Notebook**, **Report**, **Semantic Model**, **Pipeline**, or **KQL Dashboard**—has an underlying **item definition**: a portable, structured payload containing the item’s full configuration and content encoded in **Base64**.

Bulk Import & Export APIs enable you to:

- Export item definitions individually or in bulk from any workspace.
- Import (create) new items from definitions into a target workspace.
- Update existing item definitions in place for continuous deployment.
- List and paginate through all items in a workspace for batch processing.

These APIs are exposed through the standard **Fabric REST API**, use **Microsoft Entra ID (Azure AD)** authentication, and support both:

- **Delegated (user)** access
- **Service principal (app-only)** access

The goal is a first-class programmatic interface for managing Fabric content at enterprise scale.

## Why APIs matter

Previously, moving Fabric items between workspaces, backing up analytics content, or building repeatable deployment pipelines often required manual UI steps, workarounds, and non-standard scripting.

### Before: friction at every step

- Workspace migration required manual recreation of items one by one.
- No supported backup mechanism for Fabric item definitions.
- CI/CD pipelines lacked a reliable way to deploy Fabric content programmatically.
- Metadata scanning required individual API calls per item (slow and unscalable).
- Template-based provisioning was not feasible without manual intervention.

### After: programmable, scalable, repeatable

- Export a full workspace manifest with a single paginated API.
- Import hundreds of items into a new workspace in one automated flow.
- Drive Dev → Test → Prod promotion from your existing DevOps pipeline.
- Back up item definitions on a schedule and restore.
- Scan item definitions in bulk for lineage, governance, and audit purposes.

## Key scenarios

## Workspace migration

Bulk Export APIs let you serialize an entire workspace into a portable **JSON manifest**, then import it into any target workspace.

Use cases mentioned:

- Migrating workspaces during organizational restructuring or mergers
- Replicating environments across tenants (partner/subsidiary scenarios)
- Cloning a production workspace to create an isolated test environment

Because item definitions are standard JSON, the manifest can be stored, versioned, inspected, and transformed before re-import.

## CI/CD and DevOps integration

A suggested workflow:

- Export item definitions to a Git repository
- Apply changes through pull requests
- Deploy to staging/production workspaces on merge

The **Update Definition API** enables a sync pattern:

- Create items that don’t exist
- Update items that do exist
- Without deleting and recreating

### A typical CI/CD flow

| Step | Description |
| --- | --- |
| 1 | Developer commits notebook/report change to Git |
| 2 | PR review + automated validation pipeline runs |
| 3 | On merge: CI pipeline calls `POST /items` (create) or `POST /updateDefinition` (update) |
| 4 | Staging workspace updates automatically |
| 5 | After QA approval: same pipeline promotes to Production |

## Metadata backup and recovery

Schedule periodic bulk exports to capture the full state of your workspace as versioned JSON manifests. Store them in:

- Azure Blob Storage
- A Git repository
- Any durable storage system

Notes called out:

- No additional tooling required (pure REST API calls)
- Definitions are human-readable and diff-friendly JSON
- Bulk import creates all items in a single automated pass

## Environment promotion (Dev → Test → Prod)

Export from Dev, transform parameterized values (for example connection strings, workspace references, or data source bindings), and import into Test or Prod.

## Metadata scanning and lineage analysis

Bulk scanning enables tools that analyze report definitions for lineage (for example, which semantic model columns are used in each report) to extract hundreds of definitions in bulk instead of one at a time.

## Template libraries and self-service provisioning

Maintain reusable item definitions (standard notebooks, reports, pipelines, KQL dashboards) as templates:

- Governance team maintains approved templates
- New projects request a workspace; automation provisions it fully configured
- Items are traceable to source templates via definition provenance

## API endpoints overview

Base URL: Fabric REST API (quickstart): https://learn.microsoft.com/rest/api/fabric/articles/get-started/fabric-api-quickstart

### Export (read) operations

| Method | Endpoint | Description |
| --- | --- | --- |
| POST | `/workspaces/{workspaceId}/items/bulkExportDefinitions?beta=true` | Export an item’s full definition as Base64-encoded parts. May return `202` for LRO. |

### Import (write) operations

| Method | Endpoint | Description |
| --- | --- | --- |
| POST | `/workspaces/{workspaceId}/items/bulkImportDefinitions?beta=true` | Create/update an existing item’s definition in-place. Intended for CI/CD sync. |

## Required permissions

### Import

1. Caller must have a **contributor** or higher role for the workspace.
2. Caller must have **read and write** permissions for each item being updated.

### Export

1. Caller must have a **contributor** or higher role for the workspace.
2. When exporting all items in a workspace: only items where the caller has **both read and write** permissions are exported.

## How it works: the LRO pattern

For large/complex items (for example heavy-content items or reports with many pages), operations may return **HTTP 202 Accepted** and use a **Long-Running Operation (LRO)** polling pattern.

### Example LRO flow

```http
# Step 1: Request the definition
POST /workspaces/{workspaceId}/items/bulkImportDefinitions?beta={beta}

-> HTTP 202 Accepted
-> Location: /operations/{operationId}

# Step 2: Poll until complete
GET /operations/{operationId}

-> { "status": "Running" } … retry after
-> { "status": "Succeeded" } -> proceed

# Step 3: Retrieve result
GET /operations/{operationId}/result

-> { "definition": { "parts": [ … ] } }
```

Each definition part is a Base64-encoded file identified by:

- `path` (for example `notebook-content.py`, `report.json`)
- `payloadType` (for example `InlineBase64`)

Decoding parts gives raw content you can inspect, transform, or version in Git.

## Limitations (Preview)

| Limitation | Details |
| --- | --- |
| Rate limiting | Include 0.5–1s delays between requests and handle `429` with exponential backoff. |
| Payload size | Maximum request size is **128 MB**. |
| Supported items | Only items supported by both Fabric Git Integration and single item definitions APIs apply. |
| Semantic model support | Semantic model V1 is not supported. |
| Cross-tenant references | Tenant-specific references (connection strings, data source bindings) must be manually updated after cross-tenant import. |
| Display name uniqueness | Display names must be unique within a workspace per item type; duplicates fail with `DuplicateDisplayNameAndType`. |
| Breaking changes possible | API surface is stable for evaluation but may change before GA; monitor Fabric REST API release notes. |

## Prerequisites

| Requirement | Details |
| --- | --- |
| Microsoft Entra ID app | Registered app with `Item.ReadWrite.All` and `Workspace.Read.All` permissions. |
| Fabric workspaces | At least one workspace with items to export and a target workspace for import. |
| Workspace role | Contributor or higher on both source and target workspaces. |

## Resources

- Fabric Items API Reference (Core APIs): https://learn.microsoft.com/rest/api/fabric/core/items
- Item Definition overview: https://learn.microsoft.com/rest/api/fabric/articles/item-management/definitions/item-definition-overview
- Long-running operations guide: https://learn.microsoft.com/rest/api/fabric/articles/long-running-operation
- Microsoft Entra ID documentation: https://learn.microsoft.com/entra/fundamentals/what-is-entra

## Share your feedback

Feedback channels mentioned:

- Fabric Ideas Portal: https://ideas.fabric.microsoft.com/
- Your Microsoft account representative

Questions the post asks:

- What scenarios are you using the Bulk APIs for?
- What additional item types do you need support for?
- What limitations are most impactful for your workflow?


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/public-apis-bulk-import-and-export-items-definition-preview/)

