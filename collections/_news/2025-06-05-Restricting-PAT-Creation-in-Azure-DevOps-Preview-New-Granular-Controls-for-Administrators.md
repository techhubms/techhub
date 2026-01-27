---
external_url: https://devblogs.microsoft.com/devops/restricting-pat-creation-in-azure-devops-is-now-in-preview/
title: 'Restricting PAT Creation in Azure DevOps Preview: New Granular Controls for Administrators'
author: Angel Wong
feed_name: Microsoft DevBlog
date: 2025-06-05 17:08:48 +00:00
tags:
- Access Controls
- Azure & Cloud
- Azure DevOps
- IAM
- Identity And Access Management
- Microsoft Entra ID
- Organizational Policies
- Packaging Scopes
- PAT Policy
- Personal Access Tokens
- Project Collection Administrators
- Token Management
section_names:
- azure
- devops
- security
primary_section: azure
---
Written by Angel Wong, this article explores the new preview policy in Azure DevOps to restrict personal access token creation. It details how organizations can reduce PAT usage and strengthen their security measures.<!--excerpt_end-->

## Restricting PAT Creation in Azure DevOps Is Now in Preview

**Author:** Angel Wong

As organizations continue to strengthen their security posture, managing and restricting the use of personal access tokens (PATs) is becoming increasingly important. The latest public preview of the **Restrict personal access token creation policy** in Azure DevOps introduces new controls for Project Collection Administrators (PCAs), providing a powerful tool to minimize unnecessary PAT usage and enforce stricter security standards.

### Why Restricting PATs Matters

Personal access tokens are a convenient authentication method for Azure DevOps, but if not managed correctly, they can increase risks such as unauthorized access—especially when tokens are long-lived or overly permissive. Microsoft already provides [tenant-level policies](https://learn.microsoft.com/en-us/azure/devops/organizations/accounts/manage-pats-with-policies-for-administrators) to limit full-scope/global PATs and reduce token lifespan. This new organization-level policy enhances risk mitigation by enabling administrators to **control who can create or regenerate PATs**.

### What’s New in the Restrict PAT Creation Policy

- **Default Behavior**: For new organizations, the policy is enabled by default. For existing organizations, it must be enabled manually.
  - *Update (06/27)*: The requirement for the policy to be enabled by default in new organizations has been relaxed during public preview. This may change by General Availability.
- **Existing PATs**: Any PATs already in use will continue to work until their expiration date.
- **Global PAT Usage**: Users can only create or use global PATs if specifically allowed via an allowlist.

> 💡 **Tip**: Combine this new policy with the [maximum PAT lifespan setting](https://learn.microsoft.com/en-us/azure/devops/organizations/accounts/manage-pats-with-policies-for-administrators?view=azure-devops#set-maximum-lifespan-for-new-pats-tenant-policy) to ensure minimal risk from token sprawl and enforce short-lived credentials.

### Enabling the Policy

1. Sign in to your Azure DevOps organization at `https://dev.azure.com/{yourorganization}`.
2. Navigate to **Organization settings** (gear icon).
3. Select **Policies** and find **Restrict personal access token creation**.
4. Toggle the policy on and adjust sub-policies as needed.

![Restrict personal access token creation policy in Organization Settings](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2025/06/disable-pat-policy-1.png)

### Managing Exceptions (Allowlist)

Some users or teams may require PAT creation for specific functions. You can manage exceptions as follows:

1. Click **Manage** next to "Allow list" under the relevant subpolicy.
2. Search for and select the required Microsoft Entra users or groups.
3. Check the appropriate box for the subpolicy.

Users on the allowlist can continue to create PATs of any scope, regardless of the policy.

> 💡 **Tip**: Use platforms like [Microsoft Entra ID Identity Governance](https://learn.microsoft.com/en-us/entra/id-governance/identity-governance-overview) to automate management of access requests and reviews as user access to the allowlist expires.

### Supporting Packaging Scenarios

Some development workflows, particularly around packaging, still depend on PATs. To accommodate these without weakening overall security, enable the **Allow creation of PAT with packaging scope only** subpolicy. This restricts non-Allowlist users to creating PATs solely for packaging scopes.

![Packaging scopes subpolicy](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2025/06/disable-pat-packaging-only.png)

### Summary

The Restrict personal access token creation policy is an important advancement for organizations using Azure DevOps, helping reduce unnecessary PAT usage and reinforce adherence to modern identity and access management practices. By leveraging this tool, combined with best practices such as limiting token lifespan and enforcing allowlists, organizations can more reliably safeguard their environments while maintaining essential workflows.

**Feedback:**
*Has this new policy helped your team control PAT usage? Are there additional access controls or features you’d like to see? Share your experiences in the comments below!*

---
For further details, see the original post on the [Azure DevOps Blog](https://devblogs.microsoft.com/devops/restricting-pat-creation-in-azure-devops-is-now-in-preview/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/restricting-pat-creation-in-azure-devops-is-now-in-preview/)
