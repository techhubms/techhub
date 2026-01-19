---
layout: post
title: Announcing Public Preview of the Terraform MSGraph Provider and Microsoft Terraform VSCode Extension
author: stevenjma
canonical_url: https://techcommunity.microsoft.com/t5/azure-tools-blog/announcing-msgraph-provider-public-preview-and-the-microsoft/ba-p/4443614
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-08-14 17:56:30 +00:00
permalink: /coding/community/Announcing-Public-Preview-of-the-Terraform-MSGraph-Provider-and-Microsoft-Terraform-VSCode-Extension
tags:
- AppRoleAssignment
- AzAPI
- AzureAD
- AzureRM
- Code Generation
- DevOps Automation
- Entra API
- HashiCorp Configuration Language
- IaC
- Identity Management
- IntelliSense
- M365 Graph API
- Microsoft Entra
- Microsoft Graph
- MSGraph Provider
- Outlook API
- Role Assignment
- Service Principal
- SharePoint
- Terraform
- VS Code
- VS Code Extension
section_names:
- azure
- coding
- devops
- security
---
stevenjma introduces the public preview of the Terraform MSGraph provider and the official Microsoft Terraform VSCode extension, empowering practitioners to automate Microsoft cloud resource management and streamline Infrastructure-as-Code workflows.<!--excerpt_end-->

# Announcing MSGraph Provider Public Preview and the Microsoft Terraform VSCode Extension

Infrastructure-as-Code in the Microsoft ecosystem just got more powerful. stevenjma presents two new tools: the Terraform Microsoft Graph (MSGraph) provider in public preview, and the Microsoft Terraform Visual Studio Code extension.

## Terraform Microsoft Graph (MSGraph) Provider

The MSGraph provider allows you to manage Microsoft Entra APIs (like privileged identity management) as well as Microsoft 365 Graph APIs (such as SharePoint sites) right from Terraform using HashiCorp Configuration Language (HCL). Previously, managing Entra resources often meant relying on the older `azuread` provider. The new `msgraph` provider expands support to all beta and v1 Microsoft Graph endpoints.

**Sample Resource Definition:**

```hcl
resource "msgraph_resource" "application" {
  url  = "applications"
  body = { displayName = "My Application" }
  response_export_values = { all = "@" app_id = "appId" }
}

output "app_id" { value = msgraph_resource.application.output.app_id }
output "all"    { value = msgraph_resource.application.output.all }
```

**Query Role Assignments for a Service Principal:**

```hcl
locals {
  MicrosoftGraphAppId = "00000003-0000-0000-c000-000000000000"
  userReadAllAppRoleId = one([for role in data.msgraph_resource.servicePrincipal_msgraph.output.all.value[0].appRoles : role.id if role.value == "User.Read.All"])
  userReadWriteRoleId = one([for role in data.msgraph_resource.servicePrincipal_msgraph.output.all.value[0].oauth2PermissionScopes : role.id if role.value == "User.ReadWrite"])
  MSGraphServicePrincipalId = data.msgraph_resource.servicePrincipal_msgraph.output.all.value[0].id
  TestApplicationServicePrincipalId = msgraph_resource.servicePrincipal_application.output.all.id
}

data "msgraph_resource" "servicePrincipal_msgraph" {
  url = "servicePrincipals"
  query_parameters = { "$filter" = ["appId eq '${local.MicrosoftGraphAppId}'"] }
  response_export_values = { all = "@" }
}

resource "msgraph_resource" "application" {
  url = "applications"
  body = {
    displayName = "My Application"
    requiredResourceAccess = [{
      resourceAppId = local.MicrosoftGraphAppId
      resourceAccess = [
        { id = local.userReadAllAppRoleId, type = "Scope" },
        { id = local.userReadWriteRoleId, type = "Scope" }
      ]
    }]
  }
  response_export_values = { appId = "appId" }
}

resource "msgraph_resource" "servicePrincipal_application" {
  url  = "servicePrincipals"
  body = { appId = msgraph_resource.application.output.appId }
  response_export_values = { all = "@" }
}

resource "msgraph_resource" "appRoleAssignment" {
  url  = "servicePrincipals/${local.MSGraphServicePrincipalId}/appRoleAssignments"
  body = {
    appRoleId   = local.userReadAllAppRoleId
    principalId = local.TestApplicationServicePrincipalId
    resourceId  = local.MSGraphServicePrincipalId
  }
}
```

## SharePoint & Outlook Notifications

You can use the provider to script workflows for M365 endpoints such as SharePoint and Outlook, provided you supply the right permissions. For example, creating a notification template list:

```hcl
data "msgraph_resource" "sharepoint_site_by_path" {
  url = "sites/microsoft.sharepoint.com:/sites/msgraphtest:"
  response_export_values = {
    full_response = "@"
    site_id = "id || ''"
  }
}
resource "msgraph_resource" "notification_templates_list" {
  url = "sites/${msgraph_resource.sharepoint_site_by_path.output.site_id}/lists"
  body = {
    displayName = "DevOps Notification Templates"
    description = "Centrally managed email templates for DevOps automation"
    template = "genericList"
    columns = [
      { name = "TemplateName", text = { allowMultipleLines = false, maxLength = 255 } },
      { name = "Subject", text = { allowMultipleLines = false, maxLength = 500 } },
      { name = "HtmlBody", text = { allowMultipleLines = true, maxLength = 10000 } },
      { name = "Recipients", text = { allowMultipleLines = true, maxLength = 1000 } },
      { name = "TriggerConditions", text = { allowMultipleLines = true, maxLength = 2000 } }
    ]
  }
  response_export_values = { list_id = "id", list_name = "displayName", web_url = "webUrl" }
}
```

**Comparing Providers:**

- The MSGraph provider is to AzureAD as AzAPI is to AzureRM: it supports all underlying APIs as soon as they are available.
- `AzureAD` remains a convenience layer for a subset of Entra APIs, while `msgraph` exposes everything from Microsoft Graph.

**Resources:**

- [Quickstart: Create MSGraph resources with Terraform](https://learn.microsoft.com/graph/templates/terraform/quickstart-create-terraform)
- [MSGraph provider registry](https://registry.terraform.io/providers/microsoft/msgraph/latest)
- [Provider GitHub repo](https://github.com/Microsoft/terraform-provider-msgraph)

## Microsoft Terraform VSCode Extension

Microsoft's new VSCode extension merges AzureRM, AzAPI, and MSGraph support, providing a single extension for Terraform in Visual Studio Code. It introduces:

- **Export Azure Resources as Terraform**: Generate Terraform code from your existing Azure resources via Azure Export for Terraform.
- **IntelliSense & Syntax Highlighting**: Enhanced language support for Terraform, including MSGraph and AzAPI resources.
- **Sample Generation**: Easily insert code samples for common resources or scenarios.
- **Migration Tooling**: Migrate from Azure Terraform and AzAPI extensions to the new unified extension.
- **Paste as AzAPI**: Automatically convert Azure resource JSON or ARM Templates to AzAPI HCL on paste.

**Installation/Migration:**

- Find "Microsoft Terraform" in the VSCode Marketplace or Extensions tab.
- Users of prior "Azure Terraform" or "Terraform AzAPI Provider" extensions are prompted to migrate seamlessly.

**Feedback:**

- Share your experience by running the command "Microsoft Terraform: Show Survey" from the Command Palette.

## Conclusion

With these updates, practitioners can automate and manage both traditional Azure resources and the wider Microsoft ecosystem, such as Microsoft Graph, with improved code generation, management, and migration tooling. Stay tuned for more features and community events.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-tools-blog/announcing-msgraph-provider-public-preview-and-the-microsoft/ba-p/4443614)
