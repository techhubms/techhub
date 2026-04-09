---
section_names:
- ml
- security
primary_section: ml
title: Associated identities for items (Preview)
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/associated-identities-for-items-preview/
author: Microsoft Fabric Blog
date: 2026-04-09 09:00:00 +00:00
tags:
- API Automation
- Associated Identity
- Default Identity
- Delegated Mode
- Eventstream
- Fabric Administrator
- Item Ownership
- Lakehouse
- Managed Identity
- Microsoft Entra ID
- Microsoft Fabric
- ML
- News
- REST API
- Security
- Service Principal
- SQL Endpoint
- Workspaces
---

Microsoft Fabric Blog announces a preview feature that lets you associate a user, service principal, or managed identity with Fabric items (currently Lakehouses and Eventstreams) so those items no longer depend on the original owner’s credentials.<!--excerpt_end-->

## Background

Today, many Microsoft Fabric items rely on the **item owner’s identity** for accessing connections and certain item-specific features (for example, **delegated mode in the SQL endpoint**).

This can create operational risk:

- If the owner leaves the organization, items can become partially or fully non-functional.
- If the owner’s credentials expire, items can break.

While short-term remediation exists (for example, taking over an item in the UI), the underlying issue remains: enterprise items shouldn’t depend on a single user principal to keep working.

## What’s new: associate an identity with an item (Preview)

Microsoft Fabric introduces **Associate an identity (Preview)**, allowing you to associate an identity with certain Fabric items so the item does not depend on the creator or owner.

Supported associated identity types:

- User
- Service principal (Microsoft Entra identity)
- Managed identity

This is a **limited-scope preview** and currently applies to:

- **Lakehouse**
- **Eventstream** (excluding Eventstreams that use **Azure Events** or **Fabric Events** as a source)

In this initial release, items in scope can use the associated identity for the same kinds of operations that historically depended on the item owner.

## Getting started

### Update an item’s identity

If an item is currently associated with a user principal, you can update it to a different identity such as a **service principal** or **managed identity**.

Use this REST API to associate the identity:

```http
POST https://api.fabric.microsoft.com/v1/workspaces/{workspaceId}/items/{id}/identities/default/assign/beta=true
```

Prerequisite:

- The calling identity must have **Write** permissions on the item (and any dependent/child items impacted by the operation).

### View an item’s associated identity

You can view the identity associated with an item using the existing **Get** and **List** item APIs by including `defaultIdentity` in the response.

Get a single item (including associated identity):

```http
GET https://api.fabric.microsoft.com/v1/workspaces/{workspaceId}/items/{itemId}?include=defaultIdentity
```

List items (including associated identities):

```http
GET https://api.fabric.microsoft.com/v1/workspaces/{workspaceId}/items?include=defaultIdentity
```

If you’re a Fabric Administrator, you can also use admin APIs to view associated identity information across workspaces.

Admin get item:

```http
GET https://api.fabric.microsoft.com/v1/admin/workspaces/{workspaceId}/items/{itemId}
```

For admin list scenarios, use the corresponding Admin list items API for your workspace scope.

![Screenshot of a JSON code snippet showing results of a Get Item API call for an item labeled "Item 1" of Type "Lakehouse." Key elements include item IDs, a default identity with type "ServicePrincipal," and nested child item details, indicating hierarchical data structure.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/screenshot-of-a-json-code-snippet-showing-results.png)

*Figure: Get Item API response includes defaultIdentity along with other item details.*

## Links

- Manage identities associated with Fabric items: https://aka.ms/ItemIdentityDocsLink
- Submit feedback: https://ideas.fabric.microsoft.com/


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/associated-identities-for-items-preview/)

