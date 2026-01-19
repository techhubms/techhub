---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-preview-of-async-scaling-for-app-service-plans/ba-p/4469680
title: Introducing Preview of Async Scaling for Azure App Service Plans
author: apwestgarth
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-18 17:12:17 +00:00
tags:
- App Service Plan
- ARM
- Async Scaling
- Azure App Service
- Azure CLI
- Cloud Scalability
- Linux
- Platform as A Service
- Preview Feature
- Provisioning
- Resource Management
- Scaling
- Web Apps
- Windows
section_names:
- azure
---
apwestgarth presents a preview of async scaling for Azure App Service Plans, describing how this update streamlines scaling out large numbers of instances and guides users in using the new feature with detailed CLI commands.<!--excerpt_end-->

# Introducing Preview of Async Scaling for Azure App Service Plans

Many Azure App Service users have encountered limitations when trying to scale out their App Service Plan, often facing a 429 error indicating insufficient available reserved instance servers. This typically required users to repeatedly adjust and reissue scaling requests until their target number of instances was reached.

## New Asynchronous Scaling Capability

Azure has introduced a preview feature allowing you to create or scale App Service Plans asynchronously. Instead of manually incrementing instance count in small steps, you can now request your desired total instance count. The platform will automatically provision as capacity becomes available, reducing the need for repeated manual intervention.

### Example Scenario

Suppose you want to create a new App Service Plan with 15 instances:

#### Prior Mode of Operation

- If the platform reported only 6 available instances, your request for 15 would fail.
- You would then request 6, wait for more to be freed, request additional increments, etc.—continuing the cycle until the desired instance count was reached.

#### New Async Preview Feature

- Request the full 15 instances in one command.
- The platform attempts to fulfill the request, scaling up from currently available instances and automatically adding more as they become available—without requiring further manual scaling requests.

## Using Async Scaling in Azure CLI

### Create a Resource Group (if needed)

```shell
az group create -n <resourceGroupName> -l <location>
```

### Create App Service Plan with Async Scaling

```shell
az appservice plan create \
  -g <resourceGroupName> \
  -n <appServicePlanName> \
  --number-of-workers <desired instance count> \
  --sku <App Service Plan Sku, e.g., P1v3> \
  --async-scaling-enabled true \
  --location <location>
```

**Example Output Progress:**

```
Starting to scale App Service plan asyncasplinuxexample...
Status: InProgress — Scaled to 1 workers of pricing tier P1v3.
Status: InProgress — Scaled to 13 workers of pricing tier P1v3.
Status: InProgress — Scaled to 19 workers of pricing tier P1v3.
Status: InProgress — Scaled to 20 workers of pricing tier P1v3.
Successfully scaled to 25 workers in pricing tier P1v3.
```

### Scale Out Existing App Service Plan with Async Feature

```shell
az appservice plan update \
  -g <resourceGroupName> \
  -n <appServicePlanName> \
  --async-scaling-enabled true \
  --number-of-workers <instance count>
```

## Availability

- Async Scaling is available in preview for both Windows and Linux App Service Plans, across all SKUs except Free and Shared.
- Supported via ARM, Azure CLI, and rolling out to the Azure Portal globally.

## Summary

This new async scaling feature streamlines provisioning and scaling experiences for Azure App Service Plans by handling capacity waits and scaling retries on your behalf. Try it out to simplify large-scale deployments.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-preview-of-async-scaling-for-app-service-plans/ba-p/4469680)
