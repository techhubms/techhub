---
external_url: https://devblogs.microsoft.com/azure-sdk/azd-app-service-slot/
title: Deploy to Azure App Service Deployment Slots with azd
author: PuiChee (PC) Chan
primary_section: dotnet
feed_name: Microsoft Azure SDK Blog
date: 2026-02-11 14:59:15 +00:00
tags:
- App Service Slots
- Automation
- AVM
- Azd
- Azure
- Azure App Service
- Azure Developer CLI
- Azure SDK
- Bicep
- Blue/green Deployment
- CI/CD
- Cloud Deployment
- Deployment Slots
- DevOps
- Environment Variables
- IaC
- News
- Slot Deployment
- Staging
- .NET
section_names:
- azure
- dotnet
- devops
---
PuiChee (PC) Chan introduces the new azd feature for deploying directly to Azure App Service slots, enabling smoother deployment flows and enhanced automation for developers.<!--excerpt_end-->

# Deploy to Azure App Service Slots with azd

*Missed deploying to an App Service slot? Now you can—without workarounds or manual scripts.*

## What’s New?

Previously, `azd deploy` could only target the default production slot in Azure App Service. With this update, azd now understands the slots you define in your Bicep or Azure Verified Modules (AVM) infrastructure as code files and deploys to the slot you designate. On the initial deployment, azd deploys to both production and any defined slots. For subsequent deployments—if you have more than one slot—azd prompts you to choose which slot to deploy to, or you can pre-set the slot using an environment variable.

## Why It Matters

Deployment slots are essential for blue/green deployments, staging, and testing environments. Before this feature, deploying to a slot using azd required manual scripting or extra extensions. Now, developers can move seamlessly from slot definition in Bicep to code deployment—no extra steps, giving a smoother and more predictable outcome using just azd.

## How to Use the Feature

1. **Define a Slot in Bicep**

```bicep
resource webApp 'Microsoft.Web/sites@2021-03-01' = {
  name: 'my-appservice'
  location: resourceGroup().location
  kind: 'app'
  properties: {
    serverFarmId: myPlan.id
  }
}

resource stagingSlot 'Microsoft.Web/sites/slots@2021-03-01' = {
  name: '${webApp.name}/staging'
  location: webApp.location
  properties: {}
}
```

Or, using Azure Verified Modules (AVM):

```bicep
module appservice 'br/public:app-service:1.3.2' = {
  name: 'my-appservice'
  params: {
    // ...params
  }
}

module stagingSlot 'br/public:web-app-deployment-slot:1.0.2' = {
  name: 'staging'
  params: {
    appServiceName: appservice.outputs.name
  }
}
```

> **Note:** Deployment slots require an App Service plan at **Standard (S1)** tier or higher. Free, Shared, and Basic tiers do not support slots.

1. **Provision and Deploy with azd**

Run:

```sh
azd up
```

- azd prompts for a slot choice if multiples are defined
- To set the slot directly, use: `azd env set AZD_DEPLOY_MYAPI_SLOT_NAME staging`
- This works in CI/CD pipelines by passing the environment variable in your pipeline configuration

## Feedback and Community

- For questions or ideas, [file an issue](https://github.com/Azure/azure-dev/issues) or [join the GitHub discussions](https://github.com/Azure/azure-dev/discussions)
- To help shape azd’s future, [sign up for user research](https://aka.ms/azd-user-research-signup)

*Feature introduced in [PR #6627](https://github.com/Azure/azure-dev/pull/6627).*

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azd-app-service-slot/)
