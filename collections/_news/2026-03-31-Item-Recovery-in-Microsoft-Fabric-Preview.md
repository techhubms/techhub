---
primary_section: ml
date: 2026-03-31 10:00:00 +00:00
author: Microsoft Fabric Blog
section_names:
- ml
- security
tags:
- Admin Portal
- Audit Logs
- Compliance
- Data Pipeline
- Endorsements
- Fabric REST API
- Forensics
- Governance
- Item Recovery
- Lakehouse
- Lineage
- Microsoft Fabric
- ML
- News
- Notebook
- Preview Feature
- Retention Policy
- Security
- Sensitivity Labels
- Soft Delete
- Tenant Settings
- Workspace Recycle Bin
- Workspace Roles
title: Item Recovery in Microsoft Fabric (Preview)
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/item-recovery-in-microsoft-fabric-preview/
---

Microsoft Fabric Blog (coauthored by Yichao Wu) announces Item Recovery in Microsoft Fabric (Preview), adding a workspace recycle bin and soft-delete retention (7–90 days) so teams can restore deleted Fabric items and preserve permissions, lineage, and labels for recovery, governance, and investigations.<!--excerpt_end-->

# Item Recovery in Microsoft Fabric (Preview)

**Coauthor:** Yichao Wu

If you haven’t already, see Arun Ulag’s overview of the broader event announcements: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news).

Microsoft Fabric now has **Item recovery (Preview)** to restore deleted items within a retention window you control.

## The problem: item deletion was permanent

Previously, deleting an item inside a Fabric workspace (for example a **lakehouse**, **notebook**, or **pipeline**) permanently removed the item. While Fabric supported **workspace-level retention**, it did not provide a safety net for **individual items**.

## What’s new: recovery for individual items

When a user deletes a supported item in a workspace, the item enters a **soft-deleted state** instead of being permanently deleted.

- **Retention window:** configurable from **7 to 90 days**
- **Where you manage deleted items:** the new **Workspace recycle bin** inside each workspace
- **Actions:** browse, restore, or permanently delete items

![Screenshot of a workspace recycle bin interface showing a list of deleted files with columns for name, type, owner, original location, deletion date, deletion timestamp, and expiration period. The list includes various file types such as SQL analytics, notebooks, pipelines, and dashboards, all owned by "Yichao Wu," with deletion timestamps from 2/27/26 and expiration set to 7 days.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-workspace-recycle-bin-interface-sh.png)

*Figure: The Workspace recycle bin provides a dedicated view for managing deleted items, with options to restore or permanently delete.*

![A short video illustrating user deleting a notebook and the recovering the item by using the "recover" button in workspace recycle bin.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/a-short-video-illustrating-user-deleting-a-noteboo-scaled.gif)

*Figure: Item restore using Workspace recycle bin.*

## Roles and capabilities during the retention period

During the retention period, permissions and controls are split by role:

- **Workspace Contributors, Members, and Admins**
  - Can recover deleted items using the **Workspace recycle bin** in the Fabric portal
  - Can recover deleted items via the **REST API**
- **Workspace Admins**
  - Can **permanently delete** soft-deleted items from the recycle bin
  - Can permanently delete soft-deleted items via the **REST API**
- **Tenant Admins**
  - Can configure the **retention period**
  - Can manage settings centrally

When an item is recovered, it is restored to its original state and preserves:

- Permissions
- Data
- Lineage relationships
- Endorsements
- Sensitivity labels

Supported item types are listed here: [supported item types documentation](https://learn.microsoft.com/fabric/admin/retention-recovery).

## Why it matters

### Accidental deletion protection

Common failure modes in shared workspaces include:

- An analyst deletes the wrong Lakehouse
- A data engineer removes a pipeline that is still in production

Without item-level recovery, teams had to recreate items (often without full knowledge of configuration, lineage, and contents). Item Recovery turns this into a **self-service restore** workflow, reducing dependency on admins.

### Security investigation and forensics

The post describes a scenario where someone could:

1. Create a notebook
2. Use it to exfiltrate data from a Lakehouse
3. Delete the notebook to hide evidence

Before this feature, audit logs could show that creation/deletion happened, but the actual notebook content was gone. With Item Recovery, you can **recover the deleted notebook** and inspect:

- The code that ran
- Data references
- Execution context

### Compliance and governance

Item Recovery supports regulatory and audit requirements around protection and recoverability by providing item-level retention and recoverability (for supported item types) throughout the configured window, complementing workspace retention and audit logging.

## Next steps

Item Recovery is **Preview** and requires **no sign-up**.

- **Enable it now:** Tenant administrators can enable it in the admin portal under **Tenant settings** > **Item Recovery**.
- **Documentation:** [retention and recovery documentation](https://learn.microsoft.com/fabric/admin/retention-recovery)
- **Feedback:** Leave a comment on the original post with suggestions or feature requests.


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/item-recovery-in-microsoft-fabric-preview/)

