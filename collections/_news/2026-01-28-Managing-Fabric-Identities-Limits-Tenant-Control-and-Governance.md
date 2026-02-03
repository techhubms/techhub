---
external_url: https://blog.fabric.microsoft.com/en-US/blog/take-control-of-fabric-identities-limit-for-your-tenant-generally-available/
title: 'Managing Fabric Identities Limits: Tenant Control and Governance'
author: Microsoft Fabric Blog
primary_section: azure
feed_name: Microsoft Fabric Blog
date: 2026-01-28 10:00:00 +00:00
tags:
- Admin Portal
- API Integration
- Azure
- Developer Settings
- Entra ID
- Identity Governance
- Identity Management
- Microsoft Fabric
- News
- Programmatic Management
- REST API
- Security
- Tenant Admin
- Tenant Settings
- Workspace Identity
section_names:
- azure
- security
---
Microsoft Fabric Blog details how tenant admins can now manage the maximum number of Fabric identities, with options for custom limits and programmatic control. Authored by Microsoft Fabric Blog.<!--excerpt_end-->

# Managing Fabric Identities Limits: Tenant Control and Governance

The Microsoft Fabric platform now enables tenant admins to control the maximum number of Fabric identities (Workspace identities) that can be created within their organization. This update streamlines identity governance and scalability by increasing the default limit and offering custom configurations.

## Key Features

- **Increased Default Limit:** The organization-wide default for Fabric identities has increased from 1,000 to 10,000.
- **Custom Limit Setting:** Admins can specify a custom maximum number of Fabric identities via the Fabric Admin portal (found in Tenant settings under Developer settings).
- **Programmatic Management:** The setting can be managed with the [Update Tenant Setting REST API](https://learn.microsoft.com/rest/api/fabric/admin/tenants/update-tenant-setting), allowing for automated adjustments.
- **Integration with Entra ID:** Before applying custom limits, admins should check their Entra ID (Azure Active Directory) resource quotas for compliance. [Learn more about Entra ID service limits](https://learn.microsoft.com/entra/identity/users/directory-service-limits-restrictions).
- **Error Messaging:** Attempts to exceed the configured maximum will yield clear error messages to workspace admins.

## How to Configure

- **Portal Configuration:** Accessible via Fabric Admin portal → Tenant settings → Developer settings → "Define maximum number of Fabric identities in a tenant."
- **Default Behavior:** Setting disabled (default) = 10,000 identities.
- **Enabling and Customizing:** Enable the setting, then provide the desired upper limit.

## Programmatic Configuration Example

### Sample HTTP Request

```http
POST https://api.fabric.microsoft.com/v1/admin/tenantsettings/ConfigureFabricIdentityTenantLimit/update
```

Request body:

```json
{
  "enabled": true,
  "properties": [
    { "name": "FabricIdentityTenantLimit", "value": "100", "type": "int" }
  ]
}
```

### Sample Success Response

```json
{
  "tenantSettings": [
    {
      "settingName": "ConfigureFabricIdentityTenantLimit",
      "title": "Define maximum number of Fabric identities in a tenant",
      "enabled": true,
      "canSpecifySecurityGroups": false,
      "tenantSettingGroup": "Developer settings",
      "properties": [
        { "name": "FabricIdentityTenantLimit", "value": "100", "type": "Integer" }
      ]
    }
  ]
}
```

## Further Learning

- [Workspace identity documentation](https://learn.microsoft.com/fabric/security/workspace-identity)
- [Tenant settings index](https://learn.microsoft.com/fabric/admin/tenant-settings-index)

These improvements let organizations scale their Fabric usage while maintaining strong oversight of identity creation and compliance with Entra ID capacity.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/take-control-of-fabric-identities-limit-for-your-tenant-generally-available/)
