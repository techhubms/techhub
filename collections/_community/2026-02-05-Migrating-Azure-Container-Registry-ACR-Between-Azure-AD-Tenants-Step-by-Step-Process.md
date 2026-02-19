---
layout: "post"
title: "Migrating Azure Container Registry (ACR) Between Azure AD Tenants: Step-by-Step Process"
description: "This guide by SoumyaShet05 details the process and considerations when migrating an Azure Container Registry (ACR) across Azure Active Directory tenants. It covers prerequisites, step-by-step actions in both source and target tenants, use of managed identities, PowerShell automation, and critical configuration and testing guidance to ensure seamless transition."
author: "SoumyaShet05"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/aks-tenant-migration-considerations-and-approach/ba-p/4415198"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-05 11:11:56 +00:00
permalink: "/2026-02-05-Migrating-Azure-Container-Registry-ACR-Between-Azure-AD-Tenants-Step-by-Step-Process.html"
categories: ["Azure", "DevOps"]
tags: ["ACR", "AKS", "ARM Template", "Automation", "Azure", "Azure AD Tenant", "Azure Container Registry", "Azure Subscription", "Cloud Shell", "Community", "DevOps", "Managed Identity", "PowerShell", "RBAC", "Repository Migration", "Tenant Migration", "VNET Peering"]
tags_normalized: ["acr", "aks", "arm template", "automation", "azure", "azure ad tenant", "azure container registry", "azure subscription", "cloud shell", "community", "devops", "managed identity", "powershell", "rbac", "repository migration", "tenant migration", "vnet peering"]
---

SoumyaShet05 describes in detail how to migrate an Azure Container Registry (ACR) between Azure AD tenants, outlining all critical steps, considerations, and tools involved in the process.<!--excerpt_end-->

# Migrating Azure Container Registry (ACR) Between Azure AD Tenants: Step-by-Step Process

When shifting an Azure subscription across Azure Active Directory (Entra ID) tenants, the Azure Container Registry (ACR) does not support direct transfer. Below is a thorough process for migrating ACR resources between tenants, including preparation, migration, and validation steps.

---

## Prerequisites and Considerations

- **Placeholder Subscription**: Establish a new subscription within the target tenant to host reconstructed services.
- **Managed Identity**: In the source tenant, create a managed identity with ‘Contributor’ IAM role on the ACR.
- **Workstation Setup**: Ensure Azure Cloud Shell or local PowerShell environment with the Az module installed.
- **Docker Content Trust**: Should be disabled for source ACR.

## High-Level Migration Approach

1. **Freeze Changes**: Prevent updates to source ACR during migration.
2. **Documentation**: Log existing configuration and permissions.
3. **Create New ACR**: Deploy a new ACR in the placeholder subscription in the target tenant.
4. **Configuration Sync**: Match configuration from source ACR to target ACR.
5. **Migrate Images**: Use scripts to transfer images and repositories.
6. **Testing**: Check application integrations with the new ACR.
7. **Delete Original ACR**: After confirmation, remove source ACR post-migration.
8. **Reconnect Roles**: Reestablish RBAC and necessary permissions in the target tenant.
9. **App Configuration**: Ensure services (AKS, RBAC, etc.) are properly set up with new ACR.
10. **Final Testing**: Confirm full application function after migration.

## Migration Steps Overview

- Create a new subscription (B) in the target tenant.
- Redeploy any non-migratable resources to subscription B.
- Avoid direct Entra-based authentications or cross-tenant managed identities while migrating.
- Establish VNET peering between the source and target subscriptions.
- Redirect application components to new AKS and confirm operation.
- Admins may require separate logins during cross-tenant phase.
- After migrating the original subscription to the target tenant, re-establish VNET peering and validate.

---

## Source Tenant: Detailed Steps

1. **Export ACR Configuration**: Login to Azure Portal; export ARM (Azure Resource Manager) template of ACR for a configuration baseline.
2. **Managed Identity**: Create a new User Managed Identity in the source tenant; assign Contributor role to both the ACR and a temporary Windows VM.
3. **Windows VM Setup**: Deploy Windows VM, assign managed identity, and prepare it for PowerShell.
4. **Connect via PowerShell**:
    - Install Az module:

      ```powershell
      Install-Module -Name Az -Repository PSGallery -Force
      ```

    - Connect using managed identity:

      ```powershell
      Connect-AzAccount -Identity -AccountId <identity_ID>
      Get-AzAccessToken
      ```

      > **Note:** Save the `accessToken` securely and remember to revoke contributor access when finished.

5. **Image Export Script**: Copy and save the provided export script to the VM, adjust for the correct ACR name, and generate a CSV manifest of repositories and tags to be migrated.
6. **Upload Manifest**: Transfer the resulting CSV file (`outputfile_for_acr_import_script.csv`) to a storage account in the source tenant for retrieval in the target tenant.

---

## Target Tenant: Detailed Steps

1. **New ACR Setup**: In the placeholder subscription, provision a new ACR, mirroring configuration from the exported ARM template (must use a unique global name).
2. **Import Manifest**: Use Azure Cloud Shell to import the previously generated CSV manifest into the target tenant.
3. **Import Images Script**: Copy and modify the provided PowerShell script for importing images from the manifest. Save as `import_images_from_source.ps1`.
4. **Run Import**: Execute the import script to transfer images from the source to the new ACR.
5. **Validation**: Double-check in the Azure Portal to ensure that all repositories, images, and tags are present in the new ACR instance.

---

## Finalization and Cleanup

- **Post-Migration**: Remove original ACR in the source tenant as appropriate.
- **Permissions**: Update RBAC roles and permissions in the target tenant for all related services.
- **Testing**: Rigorously test all integrated services and application scenarios to ensure complete migration success.

---

## Additional Notes

- Dual logins may be required for admins managing across tenants during migration.
- Managed identities and contributor rights should be revoked as soon as migration completes.
- The outlined process does **not** enable a true "move"; it is a re-creation and content transfer.

---

**Author:** SoumyaShet05  
Published: Feb 05, 2026  
Version: 1.0

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/aks-tenant-migration-considerations-and-approach/ba-p/4415198)
