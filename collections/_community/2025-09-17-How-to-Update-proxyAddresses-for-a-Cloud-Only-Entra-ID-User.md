---
external_url: https://techcommunity.microsoft.com/t5/azure/how-to-update-the-proxyaddresses-of-a-cloud-only-entra-id-user/m-p/4454763#M22217
title: How to Update proxyAddresses for a Cloud-Only Entra ID User
author: JMSHW0420
feed_name: Microsoft Tech Community
date: 2025-09-17 12:42:16 +00:00
tags:
- Azure Active Directory
- Cloud Identity
- Directory Synchronization
- Exchange Online
- Graph API
- Mailbox Provisioning
- Microsoft Entra ID
- Microsoft Graph Explorer
- PATCH Request
- Permissions
- Proxyaddresses
- User Management
- Security
- Community
section_names:
- security
primary_section: security
---
JMSHW0420 shares challenges with updating proxyAddresses for a cloud-only Entra ID user using Microsoft Graph Explorer, exploring alternatives to Exchange Online PowerShell.<!--excerpt_end-->

# How to Update proxyAddresses for a Cloud-Only Entra ID User

**Author: JMSHW0420**

If you have a user in Microsoft Entra ID that's cloud-only (not synced from on-premises), adjusting the `proxyAddresses` property can be tricky if the account doesn't have an Exchange Online mailbox. This discussion outlines the practical steps and roadblocks you may encounter.

## Scenario Overview

- **Cloud-only Entra ID user:** Not migrated or synced from on-premises Active Directory.
- **proxyAddresses assigned:** Attribute contains values, most commonly used for mail routing or legacy support.
- **No Exchange mailbox:** Limits options for editing proxyAddresses via conventional Exchange tools.

## Attempt: Microsoft Graph Explorer

The official [Microsoft documentation](https://learn.microsoft.com/en-us/answers/questions/2280046/entra-connect-sync-blocking-user-creation-due-to-h) suggests using the Graph API (specifically the beta endpoint):

```
GET  https://graph.microsoft.com/beta/users/{user-guid}
PATCH  https://graph.microsoft.com/beta/users/{user-guid}
```

A PATCH request can include a body like:

```json
{
  "proxyAddresses": ["smtp:user@example.com", "SMTP:user2@example.com"]
}
```

### Observed Behavior

- **GET** requests return the current property set correctly.
- **PATCH** requests to update `proxyAddresses` fail, even with maximum permissions granted in the Graph Explorer UI.

### Permissions

All possible Modify permissions assigned; still unable to PATCH.

## Why the PATCH Might Fail

- Even with permissions, some properties (including `proxyAddresses`) are restricted if the user account lacks an Exchange Online mailbox.
- The attribute is often managed by Exchange, and direct modifications through Graph API are blocked unless the user is mail-enabled.

## Alternative Options Considered

- **Exchange Online PowerShell:** Not usable here because the user does not have a mailbox.
- **Provisioning a mailbox:** Would enable modification but requires a license and changes user state.
- **Delete/recreate user:** Last resort; not ideal due to loss of history and references.

## Recommendations and Next Steps

- Double-check any admin roles and consent for application permissions (not just delegated permissions in Graph Explorer).
- If Exchange features are required, consider licensing/provisioning a mailbox.
- For non-mailbox users, direct modification of `proxyAddresses` does not appear supported through Graph or Exchange interfaces.
- If the user was recently migrated or synced, investigate back-end attribute state and possible delays.

## Summary

Editing `proxyAddresses` for Entra ID users without Exchange mailboxes hits platform limits—Microsoft's tools restrict changes for properties considered 'mail-enabled.' Without provisioning a mailbox or assigning an Exchange Online license, robust workarounds are limited, and deletion/re-creation may be the final option.

If a genuine need exists for these attributes, provision a mailbox and manage the account through Exchange Online PowerShell for full support.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/how-to-update-the-proxyaddresses-of-a-cloud-only-entra-id-user/m-p/4454763#M22217)
