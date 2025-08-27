---
layout: "post"
title: "Terraform Provider for Microsoft Fabric: Deploying Fabric Configs with GitHub Actions"
description: "This article provides a practical guide to deploying Microsoft Fabric configurations using the Terraform provider and GitHub Actions for CI/CD automation. It walks through setting up OpenID Connect authentication for managed identities, configuring GitHub Actions workflows, handling Azure-specific environment variables, and best practices for secure deployment pipelines. The post is part of a series aimed at helping administrators leverage Terraform and Microsoft Fabric for declarative infrastructure and data analytics automation."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/terraform-provider-for-microsoft-fabric-4-deploying-a-fabric-config-with-terraform-in-github-actions/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-08-07 07:00:00 +00:00
permalink: "/2025-08-07-Terraform-Provider-for-Microsoft-Fabric-Deploying-Fabric-Configs-with-GitHub-Actions.html"
categories: ["Azure", "Coding", "ML", "DevOps"]
tags: ["Automation", "Azure", "Azure AD", "Azure Managed Identity", "Azure Resource Management", "Azure Storage", "CI/CD", "Cloud Deployment", "Coding", "Declarative Automation", "DevOps", "Environment Variables", "Federated Credential", "GitHub Actions", "GitHub CLI", "IaC", "Microsoft Fabric", "ML", "News", "OpenID Connect", "RBAC", "Terraform", "Terraform Provider", "Terraform Workflow", "YAML Pipelines"]
tags_normalized: ["automation", "azure", "azure ad", "azure managed identity", "azure resource management", "azure storage", "cislashcd", "cloud deployment", "coding", "declarative automation", "devops", "environment variables", "federated credential", "github actions", "github cli", "iac", "microsoft fabric", "ml", "news", "openid connect", "rbac", "terraform", "terraform provider", "terraform workflow", "yaml pipelines"]
---

Microsoft Fabric Blog guides readers through deploying Microsoft Fabric configurations with Terraform in GitHub Actions, emphasizing secure automation with managed identities and OpenID Connect for robust Azure-based CI/CD workflows.<!--excerpt_end-->

# Terraform Provider for Microsoft Fabric: Deploying Fabric Configs with GitHub Actions

This guide from the Microsoft Fabric Blog demonstrates how to use the Terraform provider for Microsoft Fabric to automate deployments through GitHub Actions pipelines. The article covers configuring OpenID Connect authentication for managed identities, setting up YAML workflows, and handling environment variables necessary for secure, streamlined infrastructure management.

## Overview

This is the fourth and final post in a series focused on:

1. Accelerating first steps with Microsoft Fabric CLIs
2. Using MCP servers and Fabric CLI to define Fabric resources
3. Creating workload identities with appropriate permissions
4. Deploying Fabric configs using Terraform via GitHub Actions (this post)

This post illustrates:

- How to set up your repository for CI/CD with OpenID Connect authentication,
- The structure and purpose of the GitHub Actions YAML workflow file,
- Managing required environment variables and .gitignore files,
- Best practices for deploying securely without exposing sensitive secrets.

## Step-by-Step Deployment Guide

### 1. Prepare Your Repo and Authentication

- Authenticate into the GitHub CLI: `gh auth login`
- Ensure you're in the repository root with your existing Terraform configuration.

### 2. Add Workflow YAML File

Create a `.github/workflows/terraform.yml` workflow file. The YAML instructs GitHub Actions to:

- Support manual workflow dispatch with actions for `apply` and `destroy`
- Use secure authentication (`ARM_USE_AZUREAD` and `ARM_USE_OIDC`)
- Leverage both `FABRIC_` and `ARM_` prefixed environment variables for multi-provider support
- Initialize and plan Terraform deployments, publish module artifacts, and apply changes

#### Example Excerpt from YAML

```yaml
permissions:
  id-token: write
  contents: read
env:
  ARM_USE_AZUREAD: true
  ARM_USE_OIDC: true
  # ...additional environment variables
```

### 3. Add a .gitignore File

Protect sensitive and generated files:

```
*.tfstate
*.tfplan
.terraform/
terraform.tfvars
crash.log
*.backup
```

### 4. Commit & Push to GitHub

Sync your local repository with GitHub:

```bash
git add .github/workflows/terraform.yml
git commit -m "Add Terraform workflow"
git push
```

Or initialize a new repository and connect it using `gh repo create` and subsequent git commands if needed.

### 5. Add GitHub Actions Variables

Set environment variables required by the workflow, for example:

```bash
gh variable set ARM_TENANT_ID --body "..."
gh variable set ARM_CLIENT_ID --body "..."
gh variable set BACKEND_AZURE_RESOURCE_GROUP_NAME --body "..."

# ...etc.
```

Variables configure pipeline context for Azure authentication, storage, and Terraform resource management.

### 6. Create OpenID Connect Federated Credential

Configure Azure managed identity for federated authentication. This avoids use of secrets:

```bash
az identity federated-credential create --name github --identity-name ... --subject "repo:<org>/<repo>:ref:refs/heads/main" --issuer "https://token.actions.githubusercontent.com" --audiences "api://AzureADTokenExchange"
echo "Added federated credential."
```

This setup ensures that deployments use OpenID Connect tokens for secure, secretless authentication.

### 7. Run and Monitor the Workflow

- Trigger the workflow manually with:

  ```bash
  gh workflow run terraform.yml
  ```

- Monitor progress via GitHub web interface or CLI.
- Upon successful execution, review logs for each deployment step.

## Summary

Microsoft Fabric, with the Terraform provider and GitHub Actions, empowers organizations to declaratively automate data and analytics infrastructure in Azure. Using OpenID Connect with managed identities enhances security and operational flexibility, aligning with modern DevOps practices and cloud automation standards.

For more details, visit the [original blog post](https://blog.fabric.microsoft.com/en-us/blog/terraform-provider-for-microsoft-fabric-4-deploying-a-fabric-config-with-terraform-in-github-actions/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/terraform-provider-for-microsoft-fabric-4-deploying-a-fabric-config-with-terraform-in-github-actions/)
