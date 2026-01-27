---
external_url: https://www.reddit.com/r/AZURE/comments/1mhi4mz/entra_connect_cloud_sync_one_way_wo_passwords/
title: Entra Connect Cloud Sync One Way Without Passwords for Separate Tenant Access
author: SisqoEngineer
feed_name: Reddit Azure
date: 2025-08-04 16:26:22 +00:00
tags:
- Account Disablement
- Account Provisioning
- Active Directory
- Cloud Sync
- Entra Connect
- Password Hash Sync
- Resource Access
- Tenant
- User Management
section_names:
- azure
primary_section: azure
---
SisqoEngineer shares an approach for using Entra Connect Cloud Sync to provision users to a separate Azure tenant without syncing their passwords. This setup is aimed at maintaining distinct logins and automating account management tasks.<!--excerpt_end-->

## Using Entra Connect Cloud Sync One-Way (Without Passwords) for Resource Segregation

**Author:** SisqoEngineer

The author outlines a configuration scenario for organizations looking to create a separate Azure tenant to better control access to specific resources. The key requirements and solutions are as follows:

### Scenario

- A separate Azure tenant is to be created for isolated access control over certain resources.
- Entra Connect Cloud Sync is set up **without** password hash synchronization.
- Synchronization direction is strictly one-way: from the on-premises Active Directory (AD) to the new Entra tenant.

### Key Assumptions and Configuration

1. **One-Way Sync (AD to Entra Tenant Only)**
   - On-premises users are synchronized to the new Azure tenant automatically.
   - No writeback occurs; changes in Azure are not pushed back to AD.

2. **No Password Synchronization**
   - Passwords are not synced; users have distinct credentials in the new tenant.
   - This creates a separation so that users must set and manage their new passwords independently in the new tenant.

3. **Automated Account Disablement**
   - When a user is disabled in the on-prem AD, the corresponding account in the Azure tenant is also automatically disabled.

4. **Practical Considerations**
   - Initial passwords need to be set up separately in the new tenant.
   - Account creation and disablement become automated, leveraging group synchronization.
   - The method leverages existing AD group structures to reduce manual administration.
   - The author considers this suitable for their current scale, although API-based or more advanced management methods could be used for larger or more complex scenarios.

### Open Questions and Confirmation Sought

- The author requests community confirmation that this setup will work as described and invites input on potential caveats or alternatives.
- They note an awareness of other technological approaches (e.g., APIs), but prefer this simple path for their organization's needs.

### Summary

Entra Connect Cloud Sync without password synchronization offers a practical way to provision and manage accounts across tenants while maintaining security boundaries and reducing administrative overhead. The solution is particularly useful for smaller environments or when resource and access isolation is a strategic goal.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mhi4mz/entra_connect_cloud_sync_one_way_wo_passwords/)
