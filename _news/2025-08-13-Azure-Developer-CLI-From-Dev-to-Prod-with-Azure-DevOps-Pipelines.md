---
layout: "post"
title: "Azure Developer CLI: From Dev to Prod with Azure DevOps Pipelines"
description: "This article demonstrates how to implement the 'build once, deploy everywhere' pattern using Azure DevOps Pipelines together with the Azure Developer CLI (azd). It provides a step-by-step guide for setting up environment-specific infrastructure, enhancing pipelines with multi-stage YAML, managing deployment artifacts, and adopting best practices for reliable CI/CD on Microsoft Azure. The piece also highlights advanced tips for integrating GitHub Copilot for Azure to optimize pipeline authoring and deployment strategies."
author: "PuiChee (PC) Chan, Kristen Womack"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/devops/azure-developer-cli-from-dev-to-prod-with-azure-devops-pipelines/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/devops/feed/"
date: 2025-08-13 18:29:24 +00:00
permalink: "/2025-08-13-Azure-Developer-CLI-From-Dev-to-Prod-with-Azure-DevOps-Pipelines.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["AI", "Azd", "Azure", "Azure & Cloud", "Azure CLI", "Azure Developer CLI", "Azure Developer CLI (azd)", "Azure DevOps Pipelines", "Bicep", "CI/CD", "Coding", "DevOps", "DevOps Automation", "Environment Variables", "GitHub Copilot", "GitHub Copilot For Azure", "IaC", "Microsoft Azure", "Multi Stage Pipeline", "News", "Pipeline Artifacts", "Pipeline Validation", "Production Deployment", "YAML Pipelines"]
tags_normalized: ["ai", "azd", "azure", "azure cloud", "azure cli", "azure developer cli", "azure developer cli azd", "azure devops pipelines", "bicep", "ci slash cd", "coding", "devops", "devops automation", "environment variables", "github copilot", "github copilot for azure", "iac", "microsoft azure", "multi stage pipeline", "news", "pipeline artifacts", "pipeline validation", "production deployment", "yaml pipelines"]
---

PuiChee (PC) Chan and Kristen Womack explain how to use Azure Developer CLI (azd) with Azure DevOps Pipelines to achieve consistent end-to-end CI/CD, focusing on infrastructure-as-code and reliable multi-stage deployments.<!--excerpt_end-->

# Azure Developer CLI: From Dev to Prod with Azure DevOps Pipelines

*Authors: PuiChee (PC) Chan, Kristen Womack*

This article expands on an earlier post about dev-to-prod promotion using GitHub Actions by adapting the 'build once, deploy everywhere' CI/CD pattern to Azure DevOps Pipelines. The key focus is on how to leverage the [Azure Developer CLI (azd)](https://learn.microsoft.com/azure/developer/azure-developer-cli/overview) in combination with Azure DevOps YAML pipelines for consistent and reliable deployments across different environments.

## Environment-Specific Infrastructure

The deployment strategy uses a single set of Bicep templates with a configurable `envType` parameter to tailor resource provisioning for each environment (Dev, Prod, etc.). The same techniques work across both GitHub Actions and Azure DevOps.

## Artifact-Based Deployment Pattern

Rather than moving local files between jobs, the recommended approach uses Azure DevOps' artifact system:

- **Cross-job compatibility** for artifacts
- **Automated cleanup** and retention
- **Improved traceability** with visible download history
- **Integrated platform security**

This shifts the deployment process to the industry-standard: "build once, deploy everywhere" using pipeline-managed artifacts.

## Multi-Stage Pipeline Structure

The YAML pipeline is organized into three major stages:

1. **Build and Package:** Compiles and packages the application.
2. **Deploy to Development:** Provisions infrastructure and deploys the build to the development environment.
3. **Promote to Production:** Deploys to production with environment-specific variables after optional validation gates.

**Sample YAML Snippet:**

```yaml
trigger:
  - main

pool:
  vmImage: ubuntu-latest

stages:
  - stage: build_and_test
  - stage: deploy_development
    dependsOn: build_and_test
  - stage: promote_to_Prod
    dependsOn: deploy_development
```

Each stage installs azd, configures authentication, and executes the appropriate azd commands to package, provision, and deploy using published pipeline artifacts.

## Enhancing the Pipeline

- **Validation Gate:** Before promoting to production, add validation steps (e.g., health checks, security scans, integration tests).
- **Environment Variables:** Utilize dedicated Azure DevOps pipeline variables for production settings (e.g., `AZURE_PROD_ENV_NAME`, `AZURE_PROD_LOCATION`).

## End-To-End Setup Instructions

1. **Initialize your project** with azd (`azd init`)
2. **Edit `azure.yaml`** to configure Azure DevOps as the CI/CD provider and set pipeline variables
3. **Provision development and production environments** (`azd up` and `azd provision`)
4. **Configure CI/CD Pipeline** using `azd pipeline config` and Azure DevOps UI

Full walkthrough and code samples are available at [this GitHub repo](https://github.com/puicchan/azd-dev-prod-appservice-storage).

## Pro Tip: AI-Powered Pipeline Writing

Use [GitHub Copilot for Azure](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azure-github-copilot) inside VS Code to optimize pipelines:

- Debug YAML issues
- Suggest validation/testing steps
- Recommend advanced deployment patterns

Questions or want to share your implementation approach? Join the [project discussion](https://github.com/Azure/azure-dev/discussions/5447).

## Conclusion

This approach lets development teams enjoy consistent CI/CD practices on Azure, with repeatable infrastructure patterns, robust artifact management, and cross-environment deployments. By leveraging both the Azure Developer CLI and platform-native pipeline features, organizations can streamline and secure their release processes.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/azure-developer-cli-from-dev-to-prod-with-azure-devops-pipelines/)
