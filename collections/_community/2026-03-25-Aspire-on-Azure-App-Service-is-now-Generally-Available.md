---
title: Aspire on Azure App Service is now Generally Available
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/aspire-on-azure-app-service-is-now-generally-available/ba-p/4505549
primary_section: dotnet
date: 2026-03-25 14:04:28 +00:00
author: TulikaC
section_names:
- azure
- devops
- dotnet
tags:
- .NET
- .NET Aspire
- Aspire AppHost
- Aspire.Hosting.Azure.AppService
- Autoscale
- Azure
- Azure App Service
- Azure Monitor Autoscale
- Blue Green Deployment
- Community
- Deployment Slots
- DevOps
- Distributed Applications
- Health Checks
- Managed Hosting
- NuGet
- Release Management
- Rule Based Scaling
- Staging Slots
feed_name: Microsoft Tech Community
---

TulikaC announces the general availability of .NET Aspire on Azure App Service, outlining how to deploy Aspire AppHost-based distributed apps to App Service (including deployment slots), with links to Microsoft Learn quickstarts and configuration guidance.<!--excerpt_end-->

# Aspire on Azure App Service is now Generally Available

Today we are announcing General Availability of Aspire on Azure App Service, making it easier to take distributed applications from local development to a fully managed production environment on Azure App Service.

With the [Aspire.Hosting.Azure.AppService](https://www.nuget.org/packages/Aspire.Hosting.Azure.AppService) package, you can define your hosting environment in code and deploy to App Service using the same AppHost model you already use for orchestration.

## What Aspire on App Service provides

Aspire brings a code-first model for building, running, and deploying distributed applications, with AppHost as the place where services, dependencies, and topology are declared.

On Azure App Service, this means developers can keep the familiar Aspire programming model while using a fully managed platform for:

- Hosting
- Security patching
- Scaling

More background on Aspire: [What is Aspire?](https://aspire.dev/get-started/what-is-aspire/)

## Getting started

If you’re new to Aspire on App Service, the fastest path is the Microsoft Learn quickstart:

- [Quickstart: Deploy a .NET Aspire app to Azure App Service](https://learn.microsoft.com/azure/app-service/quickstart-dotnet-aspire)

## New in this release: Deployment Slots support

This release adds Deployment Slots support so you can adopt safer deployment patterns (staging → validate → swap).

### Example: add a deployment slot from AppHost code

```csharp
var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureAppServiceEnvironment("<appsvc64>")
    .WithDeploymentSlot("dev");

var apiService = builder.AddProject<Projects.AspireApp64_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.AspireApp64_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
```

Behavior notes:

1. If the production slot does not already exist, this creates both the production slot and the staging slot with identical code.
2. If the production slot already exists, the deployment goes only to the staging slot.

## Scaling notes and current limitation

> Note:
> **Scaling:** Manual scaling is supported (via AppHost code or the Azure portal), and you can also setup [rule-based scaling](https://learn.microsoft.com/azure/azure-monitor/autoscale/autoscale-get-started). Automatic scaling is not yet supported in the current experience.

## Configuration documentation

- [Configure .NET Aspire on Azure App Service](https://learn.microsoft.com/azure/app-service/configure-language-dotnet-aspire)

Updated Mar 25, 2026

Version 1.0

[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/aspire-on-azure-app-service-is-now-generally-available/ba-p/4505549)

