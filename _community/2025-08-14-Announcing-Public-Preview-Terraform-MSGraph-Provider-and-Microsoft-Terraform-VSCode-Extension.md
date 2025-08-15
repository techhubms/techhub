---
layout: "post"
title: "Announcing Public Preview: Terraform MSGraph Provider and Microsoft Terraform VSCode Extension"
description: "This comprehensive announcement introduces the public preview of the Terraform Microsoft Graph (MSGraph) provider and the official Microsoft Terraform extension for Visual Studio Code. It highlights how these tools enable infrastructure-as-code practitioners to automate and manage Microsoft cloud resources, including Microsoft Entra and M365 Graph APIs, using Terraform and Visual Studio Code. The article covers installation, workflow enhancements, real-world code samples, and practical DevOps scenarios."
author: "stevenjma"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-tools-blog/announcing-msgraph-provider-public-preview-and-the-microsoft/ba-p/4443614"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-14 17:56:30 +00:00
permalink: "/2025-08-14-Announcing-Public-Preview-Terraform-MSGraph-Provider-and-Microsoft-Terraform-VSCode-Extension.html"
categories: ["Azure", "Coding", "DevOps", "Security"]
tags: ["Application Role Assignment", "AzAPI", "Azure", "AzureAD", "AzureRM", "Cloud Automation", "Code Samples", "Coding", "Community", "DevOps", "Entra ID", "HashiCorp Configuration Language", "IaC", "IntelliSense", "M365", "Microsoft Graph", "MSGraph Provider", "Privileged Identity Management", "Resource Automation", "Resource Export", "Security", "Service Principal", "SharePoint API", "Terraform", "VS Code Extension"]
tags_normalized: ["application role assignment", "azapi", "azure", "azuread", "azurerm", "cloud automation", "code samples", "coding", "community", "devops", "entra id", "hashicorp configuration language", "iac", "intellisense", "m365", "microsoft graph", "msgraph provider", "privileged identity management", "resource automation", "resource export", "security", "service principal", "sharepoint api", "terraform", "vs code extension"]
---

Stevenjma from Microsoft introduces key updates for Terraform users: the public preview of the MSGraph provider for full Microsoft Graph API coverage, and a unified Microsoft Terraform VSCode extension for enhanced Azure and Entra infrastructure automation.<!--excerpt_end-->

# Announcing MSGraph Provider Public Preview and Microsoft Terraform VSCode Extension

**Author:** stevenjma

## Overview

Microsoft announces two significant advancements for infrastructure-as-code (IaC) specialists: the public preview of the Terraform Microsoft Graph (MSGraph) provider, and the new unified Microsoft Terraform Visual Studio Code (VSCode) extension. These tools are tailored to enhance workflow automation and resource management across Azure, Entra, and M365 environments.

## Terraform MSGraph Provider Public Preview

- **Full Microsoft Graph API support:** Execute CRUD operations against Entra APIs (e.g., privileged identity management) and M365 Graph APIs (e.g., SharePoint sites) using Terraform's HashiCorp Configuration Language (HCL).
- **Extend beyond AzureAD:** The MSGraph provider supports all beta and v1 endpoints, including users, groups, and advanced features. AzureAD remains a convenience layer for a subset of Entra APIs.

### Key Terraform Examples

```hcl
resource "msgraph_resource" "application" {
  url = "applications"
  body = { displayName = "My Application" }
  response_export_values = { all = "@" app_id = "appId" }
}

output "app_id" { value = msgraph_resource.application.output.app_id }
output "all" { value = msgraph_resource.application.output.all }
```

#### Granting App Permissions with MSGraph Provider

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
  url  = "applications"
  body = {
    displayName           = "My Application"
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

#### SharePoint & Outlook Notifications Automation

You can automate notification workflows by creating and configuring SharePoint lists and templates for centralized DevOps-managed notifications.

```hcl
data "msgraph_resource" "sharepoint_site_by_path" {
  url = "sites/microsoft.sharepoint.com:/sites/msgraphtest:"
  response_export_values = { full_response = "@" site_id = "id || ''" }
}

resource "msgraph_resource" "notification_templates_list" {
  url = "sites/${msgraph_resource.sharepoint_site_by_path.output.site_id}/lists"
  body = {
    displayName = "DevOps Notification Templates"
    description = "Centrally managed email templates for DevOps automation"
    template = "genericList"
    columns = [
      { name = "TemplateName", text = { allowMultipleLines = false, appendChangesToExistingText = false, linesForEditing = 1, maxLength = 255 } },
      { name = "Subject", text = { allowMultipleLines = false, appendChangesToExistingText = false, linesForEditing = 1, maxLength = 500 } },
      { name = "HtmlBody", text = { allowMultipleLines = true, appendChangesToExistingText = false, linesForEditing = 10, maxLength = 10000 } },
      { name = "Recipients", text = { allowMultipleLines = true, appendChangesToExistingText = false, linesForEditing = 3, maxLength = 1000 } },
      { name = "TriggerConditions", text = { allowMultipleLines = true, appendChangesToExistingText = false, linesForEditing = 5, maxLength = 2000 } }
    ]
  }
  response_export_values = { list_id = "id", list_name = "displayName", web_url = "webUrl" }
}
```

- **Documentation and guides:**
  - [Deploy your first msgraph resources](https://learn.microsoft.com/graph/templates/terraform/quickstart-create-terraform)
  - [Provider Registry page](https://registry.terraform.io/providers/microsoft/msgraph/latest)
  - [GitHub Repository](https://github.com/Microsoft/terraform-provider-msgraph)

## Microsoft Terraform VSCode Extension

- **Consolidated extension:** Combines AzureRM, AzAPI, and MSGraph capabilities into one tool.
- **Key Features:**
  - Export Azure resources as Terraform code using the Azure Export for Terraform tool.
  - IntelliSense, code completion, syntax highlighting, code sample generation.
  - Migration path from earlier extensions (Azure Terraform, AzAPI) is provided.
  - Paste ARM templates/JSON and auto-convert to AzAPI Terraform code.
- **Installation:** Available via Visual Studio Marketplace.
- **Migration:** Users of legacy extensions are guided seamlessly to transition.

## DevOps and Security Implications

- **Infrastructure automation:** Manage identity, privilege, and policy on Entra (AzureAD), M365, and Azure resources using consistent IaC patterns.
- **Secure configuration:** Automate permission grants and policy assignments directly within Terraform code, enhancing reproducibility and compliance.

## Feedback and Community

- Feedback is encouraged via a built-in command: `Microsoft Terraform: Show Survey`.
- Stay tuned for ongoing feature updates, workshops, and additional community resources.

---

For more details, implementation guides, and the latest updates, see the official documentation and GitHub repositories.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-tools-blog/announcing-msgraph-provider-public-preview-and-the-microsoft/ba-p/4443614)
