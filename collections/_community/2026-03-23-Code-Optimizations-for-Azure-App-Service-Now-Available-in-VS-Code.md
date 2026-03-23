---
feed_name: Microsoft Tech Community
author: fiveisprime
section_names:
- ai
- azure
- devops
- dotnet
- github-copilot
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/code-optimizations-for-azure-app-service-now-available-in-vs/ba-p/4504252
title: Code Optimizations for Azure App Service Now Available in VS Code
primary_section: github-copilot
date: 2026-03-23 15:18:08 +00:00
tags:
- .NET
- .NET Web Apps
- AI
- Application Insights
- Application Insights Profiler
- ASP.NET Core
- Azure
- Azure App Service
- Azure App Service Extension
- Azure Resources View
- Community
- Copilot Chat
- CPU Profiling
- Developer Inner Loop
- DevOps
- GitHub Copilot
- LINQ Performance
- Memory Profiling
- Performance Optimization
- Performance Profiling
- Production Debugging
- Profiler Stack Trace
- Stack Traces
- VS Code
- VS Code Extension
- Workspace Symbol Provider
---

fiveisprime introduces “Code Optimizations” in the Azure App Service VS Code extension, which uses Application Insights Profiler data to pinpoint slow .NET methods and then launches GitHub Copilot Chat with stack traces and source context to suggest targeted fixes.<!--excerpt_end-->

## Overview

A new feature called **Code Optimizations** has shipped in the **Azure App Service extension for VS Code**. It’s designed to reduce the friction between:

- **Detecting** performance issues in production (via **Application Insights Profiler**)
- **Fixing** the specific code paths causing CPU/memory overhead (via **GitHub Copilot Chat**)

The goal is to bring “production intelligence” into the editor so developers don’t have to bounce between the Azure Portal and local code to act on profiler findings.

## The problem: production performance is a black box

Scenario the post targets:

- A **.NET app** deployed to **Azure App Service**
- Monitoring shows **elevated CPU** and increasing **response times**
- Reproducing production load patterns locally is hard
- Application Insights can detect issues, but the workflow has too much context switching

## What’s new: Code Optimizations node in VS Code

The extension adds a **Code Optimizations** node under your **.NET web apps** in the **Azure Resources** tree view.

It surfaces performance issues detected by **Application Insights Profiler**, such as excessive **CPU** or **memory** usage attributed to specific functions.

Each optimization includes:

- The **bottleneck function**
- The **parent function** calling it
- The **resource usage category** (CPU, memory, etc.)
- The **impact percentage**, to help prioritize work

## “Fix with Copilot” workflow

For an optimization item, selecting **Fix with Copilot** triggers these steps:

1. **Locate the problematic code** in your workspace by matching function signatures from the profiler stack trace against local source using **VS Code’s workspace symbol provider**
2. **Open the file** and highlight the exact method containing the bottleneck
3. **Launch a Copilot Chat session** with a pre-filled prompt containing:
   - The issue description
   - The recommendation from Application Insights
   - The full stack trace context
   - The source code of the affected method

The post’s reasoning is that combining **stack trace + recommendation + impact data + the method source** gives Copilot enough context to suggest a more targeted change rather than generic guidance.

Example given:

- A **LINQ-heavy** transformation consuming **38% CPU** in `OrderService.CalculateTotals`, called from `CheckoutController.Submit`.

## Prerequisites

- A **.NET web app** deployed to **Azure App Service**
- **Application Insights** connected to the app
- **Application Insights Profiler** enabled (the extension will prompt you if it isn’t)

## Windows vs Linux App Service plans

### Windows App Service plans

- When creating a new web app via the extension, there’s an option to **enable the Application Insights profiler**.
- For existing apps, the Code Optimizations node guides you through enabling profiling if needed.

### Linux App Service plans

- Profiling on Linux requires **code-level integration** (not a platform toggle).
- If no issues are found, the extension provides a prompt to help you add profiler support to the application code.

## What’s next

This is positioned as a first step toward integrating operational insights into the editor beyond:

- **.NET** (other runtimes/languages)
- **Performance** (reliability issues, exceptions, other operational signals)

## Links

- Azure App Service extension for VS Code: https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azureappservice
- GitHub repo for feedback/issues: https://github.com/microsoft/vscode-azureappservice


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/code-optimizations-for-azure-app-service-now-available-in-vs/ba-p/4504252)

