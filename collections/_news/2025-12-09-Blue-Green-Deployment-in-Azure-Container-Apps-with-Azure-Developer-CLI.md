---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-blue-green-aca-deployment/
title: Blue-Green Deployment in Azure Container Apps with Azure Developer CLI
author: PuiChee (PC) Chan
feed_name: Microsoft Azure SDK Blog
date: 2025-12-09 18:34:22 +00:00
tags:
- Azd
- Azure Container Apps
- Azure Developer CLI
- Azure SDK
- Bicep
- Blue Green Deployment
- CI/CD
- Containerization
- Deployment Strategies
- Docker
- GitHub Actions
- IaC
- Immutable Deployments
- Python
- Revision Based Deployment
- Traffic Splitting
- Azure
- DevOps
- News
- .NET
section_names:
- azure
- dotnet
- devops
primary_section: dotnet
---
PuiChee (PC) Chan provides a technical overview of blue-green deployment in Azure Container Apps using the Azure Developer CLI, with hands-on steps for setup, revision management, and CI/CD automation.<!--excerpt_end-->

# Blue-Green Deployment in Azure Container Apps Using Azure Developer CLI

By **PuiChee (PC) Chan**

Blue-green deployments allow teams to minimize downtime and reduce risk when releasing new application versions. This guide demonstrates how to execute blue-green deployments for Azure Container Apps (ACA) by leveraging the Azure Developer CLI (azd), Bicep infrastructure-as-code, and CI/CD pipelines with GitHub Actions.

## Overview

- **Azure Developer CLI (azd)** now supports Azure Container Apps with revision-based deployments.
- Blue-green deployments let you maintain both 'blue' (current) and 'green' (new) revisions, directing traffic between them with minimal disruption.
- The process is controlled via environment variables, Bicep parameters, and automated workflows.

## Prerequisites

- Azure subscription
- Azure Developer CLI installed
- Docker (for building images)
- Python (sample app in walkthrough)

## Local Deployment Workflow

1. **Initialize the Project**
   - Create an empty folder and run:

     ```sh
     azd init -t puichan/aca-blue-green
     ```

2. **Deploy Blue Revision**
   - Set environment variables and deploy:

     ```sh
     azd env set BLUE_COMMIT_ID fb699ef
     azd env set LATEST_COMMIT_ID fb699ef
     azd up
     ```

   - Result: Only the blue revision is created if `GREEN_COMMIT_ID` is not set.
3. **Create Green Revision (Keep Traffic on Blue)**
   - Add green revision, but keep traffic routed to blue:

     ```sh
     azd env set GREEN_COMMIT_ID c6f1515
     azd env set LATEST_COMMIT_ID c6f1515
     azd env set PRODUCTION_LABEL blue
     azd deploy
     ```

4. **Traffic Management**
   - The `PRODUCTION_LABEL` environment variable determines which revision receives production traffic. Changing it and redeploying swaps traffic between blue and green.
   - Traffic split logic is encoded in the [`web.bicep`](https://github.com/puicchan/aca-blue-green/blob/main/infra/web.bicep) file.
5. **Viewing Endpoints**
   - Get FQDNs for production, blue, and green deployments:

     ```powershell
     $uri = azd env get-value "SERVICE_WEB_URI"
     $domain = ([System.Uri]$uri).Host.Split('.', 2)[1]
     Write-Host "Production FQDN: $uri"
     Write-Host "Blue label FQDN: https://web---blue.$domain"
     Write-Host "Green label FQDN: https://web---green.$domain"
     ```

6. **Switch Traffic to Green**
   - To direct all production traffic to the green revision:

     ```sh
     azd env set PRODUCTION_LABEL green
     azd deploy
     ```

7. **Rollback to Blue**
   - Restore blue revision as production:

     ```sh
     azd env set PRODUCTION_LABEL blue
     azd deploy
     ```

## CI/CD Pipeline with GitHub Actions

- A sample [`azure-dev.yml`](https://github.com/puicchan/aca-blue-green/blob/main/.github/workflows/azure-dev.yml) workflow is provided to automate deployments.
- The workflow uses the current commit ID as the revision label and manages blue-green alternation based on ACA tags and state, automating both new deployments and traffic switches.
- ACA garbage collects old, inactive revisions beyond a configured threshold.

## Key Concepts & Best Practices

- **Immutability**: ACA revisions are immutable; deployments always create a new revision, simplifying rollback and auditing.
- **Bicep & IaC**: Use Bicep templates for declarative infrastructure, mapping environment variables to Bicep parameters for automation.
- **Automated Rotation**: Deployments alternate blue and green, with state managed in ACA tags and controlled through CI/CD workflows.
- **Manual or Automated Management**: Both local CLI commands and GitHub Actions pipelines can be used to control deployments and traffic splits.

## Resources

- [Official ACA Blue-Green Deployment Docs](https://learn.microsoft.com/azure/container-apps/blue-green-deployment?pivots=bicep)
- [Sample Project Repository](https://github.com/puicchan/aca-blue-green)
- [Azure Developer CLI Documentation](https://learn.microsoft.com/azure/developer/azure-developer-cli/container-apps-workflows?tabs=avm-module#revision-based-deployment-strategy)

## Conclusion

The blue-green deployment pattern is now straightforward for Azure Container Apps with the Azure Developer CLI's revision-based strategy. This enables robust, production-grade rollouts with minimal downtime and easy rollbacks. The immutability of revisions and declarative Bicep workflows simplify lifecycle management, while CI/CD automation provides repeatable, reliable deployments.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-blue-green-aca-deployment/)
