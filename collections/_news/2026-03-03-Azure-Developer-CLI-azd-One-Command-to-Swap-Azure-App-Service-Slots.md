---
layout: "post"
title: "Azure Developer CLI (azd): One Command to Swap Azure App Service Slots"
description: "This news post introduces the new 'azd appservice swap' command, enhancing the Azure Developer CLI with support for fast, interactive, and scriptable App Service deployment slot swaps. The update improves developer productivity by simplifying zero-downtime deployments and streamlining slot swaps directly from the terminal."
author: "PuiChee (PC) Chan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azd-appservice-swap/"
viewing_mode: "external"
feed_name: "Microsoft Azure SDK Blog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2026-03-03 21:10:01 +00:00
permalink: "/2026-03-03-Azure-Developer-CLI-azd-One-Command-to-Swap-Azure-App-Service-Slots.html"
categories: ["Azure", "DevOps"]
tags: ["App Service", "Azd", "Azure", "Azure Developer CLI", "Azure SDK", "Cloud Deployment", "Command Line Interface", "Deployment Slots", "DevOps", "DevOps Automation", "Interactive CLI", "Microsoft Azure", "News", "Production Deployments", "Slot Swap", "Terminal Tools", "Zero Downtime Deployment"]
tags_normalized: ["app service", "azd", "azure", "azure developer cli", "azure sdk", "cloud deployment", "command line interface", "deployment slots", "devops", "devops automation", "interactive cli", "microsoft azure", "news", "production deployments", "slot swap", "terminal tools", "zero downtime deployment"]
---

PuiChee (PC) Chan presents the addition of the 'azd appservice swap' command in the Azure Developer CLI, enabling developers to perform App Service slot swaps efficiently from the terminal for faster, zero-downtime deployments.<!--excerpt_end-->

# Azure Developer CLI (azd): One Command to Swap Azure App Service Slots

The Azure Developer CLI (`azd`) now includes the `appservice swap` command, making deployment slot swaps quick and intuitive for Azure App Service users.

## Overview

Deployment slots in Azure App Service allow development teams to stage application updates and roll them out to production without downtime. The new feature in `azd` enables slot swaps directly from the command line, keeping you productive in your development workflow.

## Key Features

- **Terminal-Based Slot Swapping**: Use `azd appservice swap` to interactively select and swap deployment slots.
- **Automatic Detection**: The CLI extension automatically detects your App Service and lists available slots for selection.
- **No Extra Config Needed**: Integrates seamlessly with your existing `azd` project.
- **Scriptable and Flexible**: Command supports both interactive use and automation with source/destination slot parameters.

## Example Usage

- Start an interactive swap:

  ```bash
  azd appservice swap
  ```

- Swap from staging to production:

  ```bash
  azd appservice swap --src staging --dst @main
  ```

- Roll back:

  ```bash
  azd appservice swap --src @main --dst staging
  ```

- Target a specific service:

  ```bash
  azd appservice swap --service api --src staging --dst @main
  ```

## Benefits

- **Zero Downtime Deployments**: Swapping slots enables reliable updates with no user impact.
- **Developer Productivity**: Stay in the terminal—no need to manage slot swaps through the portal.
- **Simple Automation**: Easily integrate slot swaps into CI/CD workflows.

## Feedback and Contributions

Have suggestions or feedback? File issues on the [Azure Dev CLI GitHub repo](https://github.com/Azure/azure-dev) or join [user research](https://aka.ms/azd-user-research-signup).

This feature was introduced in [PR #6687](https://github.com/Azure/azure-dev/pull/6687).

---

For more on deployment slots, read the [Azure App Service deployment slots documentation](https://learn.microsoft.com/azure/app-service/deploy-staging-slots).

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azd-appservice-swap/)
