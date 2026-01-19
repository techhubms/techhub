---
external_url: https://www.reddit.com/r/AZURE/comments/1mhdius/lowest_costing_for_a_container_instance/
title: Strategies for Minimizing Costs of Azure Container Instances with Event-Driven Usage
author: smallstar3377
viewing_mode: external
feed_name: Reddit Azure
date: 2025-08-04 13:31:23 +00:00
tags:
- AZ CLI
- Azure Container Instances
- Azure Functions
- Cloud Deployment
- Cost Optimization
- On Demand Compute
- Resource Management
- Scaling Strategies
- Serverless Computing
- Spring Boot
section_names:
- azure
---
Authored by smallstar3377, this post explores approaches for minimizing Azure costs when deploying a rarely used Spring Boot app. It discusses potential solutions and requests guidance on best practices.<!--excerpt_end-->

## Overview

This discussion centers on how to deploy a Spring Boot application on Azure at the lowest possible monthly cost, considering it is triggered at most 10 times per month, with each trigger requiring approximately 15 minutes of runtime.

## Problem Statement

- **App Profile:** Spring Boot app
- **Usage Pattern:** Maximum 10 invocations/month; each invocation lasts around 15 minutes
- **Goal:** Minimize monthly costs for running the app on Azure

## Considered Options

1. **Azure Functions (Serverless):**
   - Considering deploying the solution as a serverless function, which could potentially lower costs for infrequent, event-driven workloads.

2. **Container Lifecycle Management via Azure CLI:**
   - Writing a script to start the container instance using Azure CLI when needed, and automatically shut it down after 10 minutes of idle time, to avoid incurring unnecessary costs when the app is not in use.

## Requested Guidance

- The author requests URLs for Azure best practices and guides relevant to the above scenarios, especially for cost-optimized serverless or on-demand compute strategies.

## Key Considerations

- **Cost Optimization:** For infrequent workloads, serverless and on-demand infrastructure (such as Azure Functions or ephemeral Azure Container Instances) offer significant cost savings compared to always-on resources.
- **Automation:** Leveraging scripts or Azure automation tools to manage resource lifecycle can further improve cost efficiency.
- **Performance:** Ensure that cold start times and required environment support for Spring Boot are suitable for the chosen deployment model.

## Next Steps

- Investigate Azure documentation for:
  - [Azure Container Instances](https://learn.microsoft.com/en-us/azure/container-instances/)
  - [Azure Functions for Java](https://learn.microsoft.com/en-us/azure/azure-functions/functions-reference-java)
  - Cost management and automation via [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/container?view=azure-cli-latest)

## Summary

For workloads used only occasionally, serverless models or programming an on-demand start/stop lifecycle can provide substantial cost savings. Community feedback and official documentation should guide final implementation choices.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mhdius/lowest_costing_for_a_container_instance/)
