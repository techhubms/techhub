---
layout: post
title: Decoupling Default Semantic Models for Existing Items in Microsoft Fabric
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/decoupling-default-semantic-models-for-existing-in-microsoft-fabric/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-11-05 10:00:00 +00:00
permalink: /ml/news/Decoupling-Default-Semantic-Models-for-Existing-Items-in-Microsoft-Fabric
tags:
- API Integration
- Automation
- Business Intelligence
- Data Engineering
- Data Lifecycle
- Data Modeling
- Data Warehouses
- Lakehouses
- Microsoft Fabric
- Model Management
- Python Script
- Reporting
- REST API
- Semantic Models
- Workspace Management
section_names:
- ml
---
Microsoft Fabric Blog explains the migration of default semantic models to standalone items, with guidance on using the Fabric API to manage and remove unused models. Learn how these changes empower users with finer control over data modeling. Authored by the Microsoft Fabric Blog team.<!--excerpt_end-->

# Decoupling Default Semantic Models for Existing Items in Microsoft Fabric

The Microsoft Fabric platform is introducing an important update to how semantic models are managed within data warehouses, lakehouses, and related items. Previously, Fabric automatically created a default semantic model for each warehouse or lakehouse. As of this change, this behavior is discontinued for all new items, and existing default semantic models are being decoupled and migrated to stand-alone entities in your workspace.

## What Changed?

- **No more automatic default models**: New data warehouses and lakehouses will not receive an automatic semantic model. Instead, users have full control over when and how to create them.
- **Decoupling for existing models**: Default semantic models auto-generated in the past are now separated as standalone objects in your workspace.
- **Full ownership and flexibility**: You can keep using, modify, or safely delete these models without impacting other items.

## Managing Decoupled Semantic Models with the Fabric API

To help manage your workspace, Microsoft provides a Python script leveraging the Fabric REST API. This script allows you to:  

- Retrieve all decoupled, default semantic models in a given workspace.
- Identify empty models.
- Delete those empty semantic models to keep your workspace organized.

**Requirements:**

- Valid Fabric access token
- Workspace ID

**Sample Script:**

```python
import requests

workspace_id = "<WORKSPACE_ID>"
token = "<ACCESS_TOKEN>"
migratedsemanticmodels_url = f"https://api.fabric.microsoft.com/v1.0/myorg/groups/{workspace_id}/datawarehouses/migratedSemanticModels"
headers = {
    "User-Agent": "delete-default-semantic-models-script/1.0",
    "Authorization": f"Bearer {token}"
}

response = requests.request("GET", migratedsemanticmodels_url, headers=headers)
response.raise_for_status()

models = response.json().get("value", [])
print(f"Found {len(models)} semantic models.")

for model in models:
    model_id = model.get("modelObjectId")
    is_empty = model.get("isEmpty")
    delete_url = f"https://dailyapi.fabric.microsoft.com/v1/workspaces/{workspace_id}/semanticModels/{model_id}"
    if is_empty:
        print(f"Would delete (empty): ({model_id}) - URL: {delete_url}")
        delete_response = requests.delete(delete_url, headers=headers)
        if delete_response.status_code == 200:
            print(f"Deleted: ({model_id})")
        elif delete_response.status_code == 404:
            print(f"Not found or already deleted: ({model_id})")
        else:
            print(f"Failed to delete ({model_id}): {delete_response.status_code}")
    else:
        print(f"Skipping non-empty model: ({model_id})")
```

For details on available endpoints and their use, see the [Items – Delete Semantic Model – REST API (Semantic Model)](https://learn.microsoft.com/rest/api/fabric/semanticmodel/items/delete-semantic-model?tabs=HTTP) documentation.

## Summary

By decoupling default semantic models, Microsoft Fabric is creating a more modular, open, and transparent data modeling architecture. This gives organizations full lifecycle control to manage, optimize, or remove unused models—ensuring workspaces remain clear and efficient. This update empowers users to tailor their data modeling infrastructure according to evolving analytics or reporting needs, marking another step in the broader evolution of the Microsoft Fabric platform.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/decoupling-default-semantic-models-for-existing-in-microsoft-fabric/)
