---
external_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-ga-of-bicep-templates-support-for-microsoft-entra-id/ba-p/4437163
title: Announcing GA of Bicep Templates for Microsoft Entra ID Resources
author: Dan_Kershaw
feed_name: Microsoft Tech Community
date: 2025-07-29 15:00:00 +00:00
tags:
- Azure Resource Manager
- Bicep
- Deployment Automation
- Federated Identity
- GitHub Actions
- IaC
- Managed Identity
- Microsoft Entra ID
- Microsoft Graph
- Resource Orchestration
- VS Code Extension
- Azure
- Coding
- DevOps
- Security
- Community
section_names:
- azure
- coding
- devops
- security
primary_section: coding
---
Dan_Kershaw introduces the general availability of Bicep templates for Microsoft Entra ID, highlighting improved IaC workflows and new deployment integrations.<!--excerpt_end-->

## Overview

Dan_Kershaw announces the general availability (GA) of Bicep templates for Microsoft Entra ID resources effective July 29th, 2025. This advancement allows declarative infrastructure as code (IaC) for Microsoft Graph resources, simplifying and unifying resource deployment for Azure and Entra ID resources within a single Bicep file.

## Key Features and Improvements

- **Unified Deployment:** Previously, managing Azure and Entra ID resources required separate tooling and orchestration. With the GA of Bicep templates for Microsoft Entra ID, both can be declared and deployed together, improving configuration management and deployment reliability.
- **Microsoft Graph Bicep Extension:** A dedicated extension enables authoring, deployment, and management of Entra ID resources via Bicep, either standalone or with other Azure resources.
- **Developer Experience:** The VS Code Bicep extension offers robust type safety, IntelliSense, and syntax validation for Microsoft Graph resource types in Bicep files.

## Deployment Workflow

Deployments authored in Bicep can be executed through familiar interfaces such as Azure PowerShell and Azure CLI. The deployment engine correctly manages dependencies and orchestrates resource creation, using the Microsoft Graph Bicep extension to route requests for Entra ID resources to Microsoft Graph.

An example is presented where a group creation in Microsoft Graph depends on the existence of a managed identity resource, illustrating seamless orchestration across Azure and Entra ID resources.

## Practical Scenario: GitHub Actions and Federated Identity

The article details a scenario where GitHub Actions are configured to build and deploy a web app to Azure App Service using federated identity credentials, allowing secure deployments without secrets:

- A Bicep template creates an Entra ID application with a federated identity credential.
- The GitHub Actions workflow can authenticate using a GitHub-issued token, exchanging it for a Microsoft Entra ID token.
- This enables secure, automated CI/CD workflows for Azure deployments.

Full end-to-end samples and quickstarts are referenced for further exploration.

## Additional Resources

- [Official documentation on Bicep templates for Microsoft Graph resources](https://aka.ms/graphbicep)
- [Quickstart: create and deploy your first Bicep file with Microsoft Graph resources](https://learn.microsoft.com/graph/templates/bicep/quickstart-create-bicep-interactive-mode)
- [Microsoft Graph Bicep GitHub repository for sample templates](https://github.com/microsoftgraph/msgraph-bicep-types/tree/main/quickstart-templates)

## Conclusion

The addition of Bicep support for Entra ID resources streamlines infrastructure as code practices across Microsoft cloud ecosystems, facilitating better development, deployment, and automation workflows using familiar DevOps tools and processes.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-ga-of-bicep-templates-support-for-microsoft-entra-id/ba-p/4437163)
