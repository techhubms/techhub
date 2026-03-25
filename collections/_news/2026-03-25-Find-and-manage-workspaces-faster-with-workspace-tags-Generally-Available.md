---
section_names:
- ml
date: 2026-03-25 09:30:00 +00:00
author: Microsoft Fabric Blog
primary_section: ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/find-and-manage-workspaces-faster-with-workspace-tags-generally-available/
title: Find and manage workspaces faster with workspace tags (Generally Available)
feed_name: Microsoft Fabric Blog
tags:
- Admin Endpoints
- Automation
- Discoverability
- Domain Admin
- Fabric Tags
- Get Workspaces API
- Governance
- Inventory
- List Workspaces API
- Metadata
- Microsoft Fabric
- ML
- News
- OneLake Catalog Explorer
- Reporting
- REST API
- Scanner APIs
- Tags REST APIs
- Tenant Admin
- Workspace Admin
- Workspace Tags
- Workspaces
- Workspaces API
---

Microsoft Fabric Blog announces general availability of workspace tags in Microsoft Fabric, explaining how admins and workspace owners can create, apply, discover, and automate tags (including via Fabric REST APIs) to improve workspace organization and governance.<!--excerpt_end-->

# Find and manage workspaces faster with workspace tags (Generally Available)

> If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings: https://aka.ms/FabCon-SQLCon-2026-news

Workspace tags are now generally available in Microsoft Fabric. They let you add shared context (for example: team, project, or cost center) to a workspace so people can find the right place to work faster—and so admins can govern at scale.

## Create and apply tags

Workspace tags build on the existing Fabric tags model: define tags once, then apply them where they’re useful.

1. **Create tags:** Tenant admins and domain admins can create tags for their respective scopes.
   - Docs: https://learn.microsoft.com/fabric/governance/tags-define
2. **Apply tags to workspaces:** Workspace admins can apply and remove tags in workspace settings.
   - A workspace can have **up to 10 tags** applied.

![A screenshot of the workspace settings menu, showing 2 assigned tags and a selector to add additional tags.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/a-screenshot-of-the-workspace-settings-menu-showi.png)

*Figure: Workspace settings menu showing two attached tags.*

## Discover the right workspace faster

Workspace tags improve discoverability by enabling filtering and clear visual indicators across common navigation surfaces.

1. **Filter by tags:** Filter workspaces by tags in:
   - the **workspaces list**
   - **OneLake Catalog Explorer**

![The tags filter picker in the workspace list menu, showing available tags to filter workspaces by.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-tags-filter-picker-in-the-workspace-list-menu.png)

*Figure: The tags filter picker in the workspace list menu, showing available tags to filter workspaces by.*

2. **View applied tags:** A tags indicator appears:
   - in the **workspaces list**
   - in **OneLake Catalog Explorer**

Tag names are available **on hover**, and tag names are also shown on the **workspace screen header**.

![The workspace list menu, showing a tag indicator next to tagged workspace, and the tag values on hover.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-workspace-list-menu-showing-a-tag-indicator-n.png)

*Figure: The workspace list menu showing a tag indicator next to tagged workspace and the tag values on hover.*

![The workspace screen header, showing 2 assigned workspace tags: "FY2026" and "Health" as examples.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-workspace-screen-header-showing-2-assigned-wo.png)

*Figure: The workspace screen header, showing assigned workspace tags.*

## Automate workspace tags with Fabric APIs

Workspace tagging can be managed programmatically to support governance and inventory workflows.

- Workspace tags can be **created / applied / removed** via **tags APIs**.
- Workspace tags are returned via **get / list workspaces APIs** (both **user** and **admin** endpoints).
- **Scanner APIs** will return workspace tag metadata as part of scan results (**coming soon**).

## Get started

1. Overview: **Tag your data to enrich item curation and discovery**
   - https://learn.microsoft.com/fabric/governance/tags-overview
2. How to define and use tags in Fabric: **Create and manage a set of tags**
   - https://learn.microsoft.com/fabric/governance/tags-define
3. API reference: **Tags REST APIs**
   - https://learn.microsoft.com/rest/api/fabric/core/tags


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/find-and-manage-workspaces-faster-with-workspace-tags-generally-available/)

