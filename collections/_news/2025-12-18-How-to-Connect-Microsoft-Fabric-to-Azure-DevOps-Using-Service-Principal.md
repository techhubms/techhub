---
external_url: https://blog.fabric.microsoft.com/en-US/blog/how-to-connect-microsoft-fabric-to-azure-devops-using-service-principal/
title: How to Connect Microsoft Fabric to Azure DevOps Using Service Principal
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-12-18 10:30:00 +00:00
tags:
- Azure DevOps
- CI/CD
- Cloud Development
- Configured Credential
- Continuous Deployment
- Cross Tenant Authentication
- Enterprise Security
- Git Integration
- GitHub Integration
- Microsoft Fabric
- OAuth 2.0
- Service Principal
- Workspace Automation
- Azure
- DevOps
- ML
- News
section_names:
- azure
- devops
- ml
primary_section: ml
---
Microsoft Fabric Blog presents a detailed guide on connecting Fabric workspaces to Azure DevOps using service principals, covering authentication methods, integration workflow, and practical steps for setting up automated CI/CD pipelines.<!--excerpt_end-->

# How to Connect Microsoft Fabric to Azure DevOps Using Service Principal

This blog post offers a comprehensive walkthrough for connecting Microsoft Fabric workspaces with Azure DevOps repositories using the newly released service principal and cross-tenant authentication capabilities.

## Overview

With the recent general availability of service principal and cross-tenant support in Azure DevOps, organizations can now automate and manage CI/CD for Microsoft Fabric assets more securely and flexibly. This foundational integration supports asset movement across Development, Test, and Production environments.

## Supported Git Providers

- **Azure DevOps (ADO)**
- **GitHub**

While both providers are supported, this guide focuses on leveraging Azure DevOps with enhanced authentication options.

## Previous Integration (before service principal)

- Admin users would connect Fabric workspaces to ADO repositories using their user credentials.
- Authentication relied on "Automatic Git Credential", requiring each admin to configure access manually, either via workspace settings or programmatically via the [Fabric Git Connect API](https://learn.microsoft.com/rest/api/fabric/core/git/connect?tabs=HTTP).
- Only contributor-level users in the same workspace benefited from the established connection.

## Service Principal and Configured Credential (new capability)

- Organizations can now create a new Azure DevOps cloud connection and utilize service principal identity for workspace authentication.
- **Authentication methods supported:**
  - OAuth 2.0
  - Service principal
- Cross-tenant (multi-tenant) scenarios are fully supported, facilitating collaboration and automation across environments.
- When connecting a workspace, the credential can be configured once and reused by other contributor-level users, reducing setup repetition.
- The Fabric Git Integration pane now automatically attempts "Automatic" authentication first; if it fails, a configured credential (such as the service principal) is used.

## Benefits

- **Seamless Automation:** Fully supports automated asset movement and CI/CD pipeline implementation.
- **Improved Security:** Uses service principal authentication for better control and compliance.
- **Reduced Manual Steps:** Secondary users do not need to repeat configuration.
- **Flexibility:** Choose between OAuth 2.0 and service principal methods to match your organization's requirements.

## Resources & Documentation

- [Automate git integration with service principal](https://learn.microsoft.com/fabric/cicd/git-integration/automate-git-integration-with-service-principal)
- [Fabric November 2025 Feature Summary](https://blog.fabric.microsoft.com/blog/fabric-november-2025-feature-summary?ft=Monthly-update:category#post-30705-_Toc214018713)

This guide is essential reading for DevOps engineers and administrators looking to implement robust CI/CD practices for Microsoft Fabric solutions by utilizing Azure DevOps service principal support.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/how-to-connect-microsoft-fabric-to-azure-devops-using-service-principal/)
