---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/deploying-dev-box-catalogs-and-synchronizing-with-github-using/ba-p/4467739
title: Deploying and Syncing Microsoft Dev Box Catalogs with GitHub using Terraform
author: singhshub
feed_name: Microsoft Tech Community
date: 2025-11-09 16:42:48 +00:00
tags:
- Azure CLI
- Azure Dev Center
- Azure Key Vault
- CI/CD
- Cloud Workstation
- Dev Box Catalog
- Development Environment Automation
- GitHub
- GitHub Integration
- IaC
- Microsoft Dev Box
- Resource Group
- Terraform
- Version Control
- Virtual Network
section_names:
- azure
- coding
- devops
primary_section: coding
---
singhshub outlines how to automate the deployment of Microsoft Dev Box Catalogs with Terraform and keep them synchronized with GitHub, providing a repeatable, secure, and version-controlled cloud development environment for teams.<!--excerpt_end-->

# Deploying and Syncing Microsoft Dev Box Catalogs with GitHub using Terraform

Author: singhshub

As modern development shifts toward the cloud, Microsoft Dev Box delivers ready-to-use, secure workstations for developers. Automating and version-controlling these environments with tools like Terraform and GitHub can greatly improve scalability, consistency, and team collaboration.

## Overview

This guide builds on a previous article about using Terraform to create Microsoft Dev Boxes. Here, we extend the setup to:

- Deploy **Dev Box Catalogs** via Terraform
- Synchronize catalogs with **GitHub** for versioning and team management

## Why Dev Box Catalogs?

A Dev Box Catalog is a blueprint that lets organizations standardize development environments for teams and projects. It defines base images, installed tools, and configurations so every developer works in a consistent setting.

**Key benefits:**

- Pre-installed tools and dependencies
- Consistent project/team-specific configurations
- Version-controlled templates
- Less troubleshooting of local environments

[More about Dev Box Catalogs (Microsoft Docs)](https://learn.microsoft.com/en-us/azure/deployment-environments/how-to-configure-catalog?tabs=DevOpsRepoMSI)

## Prerequisites

- Azure Subscription with Dev Box enabled
- Terraform (local install or via Azure Cloud Shell)
- Azure CLI installed and authenticated
- GitHub repository for the catalog

From previous setup:

- Resource Group
- Virtual Network and Subnet
- Network Connection
- Dev Center
- Project
- Dev Box Definition
- Dev Box Pool

## New Resources to Deploy

- Dev Box Catalog
- Azure Key Vault (for securing GitHub tokens)

Resources are deployed in 'West Europe'.

## Step 1: Create a Dev Box Catalog with Terraform

Below is a Terraform resource definition for creating a Dev Center Catalog in Azure:

```hcl
resource "azurerm_dev_center_catalog" "catalogs" {
  name                = "devCenterCatalog"
  resource_group_name = azurerm_resource_group.resourceGroup.name
  dev_center_id       = azurerm_dev_center.devCenter.id
  catalog_adogit {
    branch             = "feature/devCenterCatalog"
    path               = "/catalog"
    uri                = "https://github.com/devCenter-org/devCenter-catalog"
    key_vault_key_url  = "https://${azurerm_key_vault.this.name}.vault.azure.net/secrets/Pat"
  }
}
```

**Parameters:**

- `name`: Catalog name
- `resource_group_name`: Name of the Azure resource group
- `dev_center_id`: Azure Dev Center resource ID
- `uri`: GitHub repository URL
- `branch`: Branch to sync (e.g., `main`)
- `path`: Directory within the repo
- `key_vault_key_url`: Secure token source for GitHub authentication

## Step 2: Create Key Vault & Secure GitHub Token

Store the GitHub Personal Access Token (PAT) securely:

```hcl
resource "azurerm_key_vault" "this" {
  name                        = "devCenterCatalog-keyv"
  location                    = azurerm_resource_group.resourceGroup.location
  resource_group_name         = azurerm_resource_group.resourceGroup.name
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false
  enable_rbac_authorization   = true
  sku_name                    = "standard"
}

resource "azurerm_key_vault_secret" "github_pat" {
  name         = "Pat"
  value        = var.devops_pat
  key_vault_id = azurerm_key_vault.this.id
  lifecycle {
    ignore_changes = [ value ]
  }
}
```

## Step 3: Synchronize Catalog with GitHub

Once the Dev Box Catalog is linked to your GitHub repository, any updates to the repo (e.g., new environment definitions or updates) are automatically synchronized to Azure Dev Center. Developers can immediately leverage updated configurations without manual re-deployment.

## Conclusion

By integrating Terraform, Azure Dev Center, Microsoft Dev Box, and GitHub, teams can:

- Ensure consistent and easily reproducible cloud development environments
- Streamline collaboration and onboarding
- Automate environment updates and maintenance

This approach shifts setup complexity away from individual developers and into automated, controlled, and secure workflows.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/deploying-dev-box-catalogs-and-synchronizing-with-github-using/ba-p/4467739)
