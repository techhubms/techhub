---
external_url: https://techcommunity.microsoft.com/t5/azure/unable-to-revert-azure-devops-user-access-level/m-p/4442871#M22102
title: Persistent Visual Studio Enterprise Access Level in Azure DevOps After License Removal
author: dbenson
feed_name: Microsoft Tech Community
date: 2025-08-13 12:05:29 +00:00
tags:
- Access Levels
- Azure Active Directory
- Azure DevOps
- Entra ID
- Group Synchronization
- Identity Synchronization
- License Management
- On Premises AD
- Stakeholder Access
- User Permissions
- Visual Studio Enterprise
- Visual Studio Licensing
- Azure
- DevOps
- Community
section_names:
- azure
- devops
primary_section: azure
---
dbenson details an issue with Azure DevOps not updating a user's access level from Visual Studio Enterprise to Stakeholder after subscription removal, involving on-premises AD and Entra ID synchronization.<!--excerpt_end-->

# Issue with Azure DevOps User Access Level After License Removal

**Context:**

- A user previously assigned a Visual Studio Enterprise subscription via an on-premises AD group (replicated to Entra ID).
- Subscription and group membership have since been removed.
- Azure DevOps still shows the user as having the Visual Studio Enterprise access level.

**Troubleshooting Steps Taken:**

1. **Confirmed License Removal:** User is no longer licensed in the Visual Studio license portal.
2. **Manual Access Level Change:** Attempted to manually set user's access level to Stakeholder.
3. **Removed and Re-added User:** User removed from Azure DevOps and added back with Stakeholder access; on login, reverted to Visual Studio Enterprise access.
4. **Project Permissions Verified:** All permissions restored as needed; access level issue persists.

**Possible Causes/Considerations:**

- **Synchronization/Provisioning Delay:** Sometimes changes in group membership take time to propagate fully between on-prem AD, Entra ID, and Azure DevOps.
- **License Assignment Artifacts:** There may be artifacts or cached settings in the user's Azure DevOps profile or underlying directory service.
- **Multiple Licenses/Groups:** User may be linked to other groups or assignments that grant the higher access level through another source.
- **Azure DevOps Organization Settings:** Automatic license assignment rules or policies may be affecting access levels on user sign-in.
- **Entra ID/AD Connect Issues:** Misconfigurations or sync errors could prevent de-provisioning from reflecting accurately.

**Suggested Actions:**

- Double-check all AD and Entra ID group memberships for any remaining links.
- Review Azure DevOps organization-level license assignment policies.
- Allow some time for full synchronization, then retry manual assignment.
- If unresolved, open a support case with Microsoft with detailed diagnostics.

This scenario highlights how closely tied Azure DevOps access levels are to identity management and licensing integrations.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/unable-to-revert-azure-devops-user-access-level/m-p/4442871#M22102)
