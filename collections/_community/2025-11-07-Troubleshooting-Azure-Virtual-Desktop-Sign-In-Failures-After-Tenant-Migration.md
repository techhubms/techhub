---
layout: post
title: Troubleshooting Azure Virtual Desktop Sign-In Failures After Tenant Migration
author: psundars
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/troubleshooting-azure-virtual-desktop-sign-in-failures-after/ba-p/4467953
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-07 05:32:56 +00:00
permalink: /azure/community/Troubleshooting-Azure-Virtual-Desktop-Sign-In-Failures-After-Tenant-Migration
tags:
- Access Control
- Application Group
- Authentication
- Azure
- Azure AD
- Azure Virtual Desktop
- Community
- Conditional Access
- Host Pool
- Identity Management
- Keychain Access
- Macos
- Microsoft Entra ID
- Microsoft Graph PowerShell
- MSAL Tokens
- Security
- Tenant Migration
- Token Persistence
section_names:
- azure
- security
---
psundars explores troubleshooting steps for Azure Virtual Desktop sign-in failures after tenant migration, focusing on token issues with Microsoft Entra ID and providing actionable remediation and best practices.<!--excerpt_end-->

# Troubleshooting Azure Virtual Desktop Sign-In Failures After Tenant Migration

**Author:** psundars

Migrating an Azure subscription between tenants can sometimes lead to unexpected authentication challenges—especially in environments like **Azure Virtual Desktop (AVD)**, which rely heavily on **Microsoft Entra ID (formerly Azure AD)** for identity and access control.

## Scenario Overview

Shortly after migrating an Azure subscription from **Tenant A** to **Tenant B**, users began experiencing sign-in failures trying to access their AVD sessions. The typical error was:
> "Your account does not exist in this organization’s directory."

This occurred even after access roles were correctly reassigned. Notably, users with active AVD sessions _before_ the migration remained signed in until their authentication tokens expired.

## Symptoms Observed

- Failed AVD sign-ins
- Error code **AADSTS53003** (Conditional Access block)
- Majority of incidents on **macOS clients**
- Reinstalling/resetting the client did **not** solve the problem

## Root Cause Analysis

The issue was traced to client devices (especially macOS) still caching authentication tokens tied to the previous (legacy) tenant, causing authentication attempts against the wrong directory ID.

### Contributing Factors

- **Token persistence** in macOS Keychain
- **Cached refresh tokens** bound to the old tenant’s directory ID

Even with valid permissions in the new tenant, the local authentication context still referenced the obsolete information.

## Understanding Token Behavior on macOS

The AVD client on macOS uses tokens stored in the system Keychain:

| Token Type           | Default Lifetime              | Renewal Behavior                          |
|----------------------|------------------------------|-------------------------------------------|
| Access Token         | ~1 hour                      | Auto-renewed via refresh token            |
| Refresh Token        | 90 days inactivity (rolling) | Each use resets expiration                |
| Persistent Session   | Refresh token expiry/manual  | Cached until deleted from Keychain        |

Conditional Access (CA) policies or Sign-In Frequency policies can shorten token validity to as little as 12 hours or 7 days. Cached tokens persist until expiration or manual removal. A mismatch between tenant information forces re-authentication.

## Resolution Steps

1. **Validate AVD Configuration**
    - Ensure **host pool**, **workspace**, and **application group** references use the new tenant’s directory ID.
    - Confirm **service principals** and **role assignments** are correctly updated in the new tenant.
2. **Clear Cached Tokens on macOS**
    - Manually remove AVD MSAL tokens from **Keychain Access**.
    - After users sign out and back in, authentication is restored.

## Mitigation Options

- **Option 1 – Revoke Sessions via Microsoft Graph PowerShell:**

    ```powershell
    Connect-MgGraph -Scopes "User.RevokeSessions.All"
    Revoke-MgUserSignInSession
    ```

- **Option 2 – Manual Token Cleanup:**
    - Have users delete cached credentials/MSAL tokens from Keychain.
    - Users must re-sign in to establish an updated authentication context.

## Key Learnings

- Cached tokens may keep referencing old tenants post-migration, especially on macOS.
- Conditional Access can block token issuance during silent sign-ins after migration.
- Proactive session revocation and token cleanup reduces user impact.

## Best Practices for Future Tenant Migrations

- **Validate** all AVD resources against the correct directory ID.
- **Instruct users** to clear cached credentials before reconnecting post-migration.
- **Plan sufficient propagation time** for identity/access changes.
- **Review Conditional Access** to prevent unintended sign-in blocks during transitions.

## Closing Thoughts

Tenant migrations involve complex identity synchronization. Planning for token cleanup and validating access assignments in advance helps ensure a much smoother transition for end-users and minimizes disruption during Azure Virtual Desktop migrations.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/troubleshooting-azure-virtual-desktop-sign-in-failures-after/ba-p/4467953)
