---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-setup-cd-in-azure-logic-apps-standard-with-deployment/ba-p/4449013
title: Automating Azure Logic Apps Deployments with Deployment Center and VS Code
author: WSilveira
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-08-28 08:18:30 +00:00
tags:
- Automation
- Azure DevOps
- Azure Logic Apps
- Azure Repos
- CI/CD
- Continuous Deployment
- Deployment Center
- GitHub
- Infrastructure Automation
- Integration Services
- SCM Authentication
- Source Control
- VS Code
- Workflow Automation
section_names:
- azure
- devops
---
WSilveira explains how the Deployment Center in Azure Logic Apps Standard streamlines code deployments from VS Code and source control, integrating easily with DevOps workflows.<!--excerpt_end-->

# Automating Azure Logic Apps Deployments with Deployment Center and VS Code

## Introduction

Looking to automate Azure Logic Apps code deployments more efficiently? Deployment Center, a built-in feature in Azure Logic Apps Standard, offers streamlined integration with Visual Studio Code. It lets you deploy, update, and manage Logic Apps workflows directly from your source control repository, simplifying your deployment process.

## Feature Overview

Deployment Center links your Logic Apps to your preferred source control systems, such as GitHub or Azure Repos, automating code deployments and offering:

- **Reduced Manual Steps**: Code deployments are automated, so developers can focus more on building solutions.
- **Guided Setup**: The user interface walks you through linking repositories and setting up pipelines.
- **Rollback Support**: Quickly revert to previous workflow versions if necessary.
- **DevOps Integration**: Works seamlessly with existing DevOps or CI/CD strategies.

## How to Set Up Deployment Center

### Step 1: Prepare Your Solution

- Use the updated VS Code features to generate deployment scripts supporting Deployment Center and Azure DevOps.
- Scripts for code deployment and configuration are all managed from one place.

### Step 2: Connect Your Repository

- After scripts are generated and committed to your source control, connect your repo to your Azure Logic Apps Standard resource.
- Further commits are automatically published to the Azure environment.
- For full setup instructions, visit the [official documentation](https://learn.microsoft.com/en-us/azure/logic-apps/set-up-cd-deployment-center-standard?tabs=github).

## Important Considerations

- **Underlying Implementation**: Deployment Center uses Azure Building Services, which require SCM Basic authentication.
- **Scope of Deployment**: The VS Code-generated scripts manage only code and configuration updates; infrastructure setup (like Azure Managed connections) requires separate steps.
- **Custom Projects**: Custom code projects are not included in the standard Deployment Center process.

## Conclusion

By using Deployment Center with Azure Logic Apps Standard and Visual Studio Code, teams can automate and manage their workflow deployments efficiently, integrating directly with their existing DevOps and CI/CD platforms. This offers smoother automation, faster updates, and easier rollback in production environments.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-setup-cd-in-azure-logic-apps-standard-with-deployment/ba-p/4449013)
