---
layout: "post"
title: "Application Insights Code Optimizations: AI-Driven Performance Tuning for .NET Apps"
description: "This guide explores how Application Insights Code Optimizations empowers .NET developers to diagnose and remediate performance bottlenecks. It covers integration with the Application Insights .NET Profiler, Azure Monitor, and GitHub Copilot, providing AI-powered, actionable recommendations to enhance application performance and streamline developer workflows."
author: "Chuck Weininger"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/application-insights-code-optimizations/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-09-15 17:05:00 +00:00
permalink: "/2025-09-15-Application-Insights-Code-Optimizations-AI-Driven-Performance-Tuning-for-NET-Apps.html"
categories: ["AI", "Azure", "Coding", "DevOps", "GitHub Copilot"]
tags: [".NET", "AI", "Application Insights", "Azure", "Azure Monitor", "Blocking Insights", "Code Recommendations", "Coding", "Debugging", "DevOps", "GitHub Copilot", "GitHub Issues", "News", "OpenTelemetry", "Performance", "Performance Optimization", "Profiler", "Profiling", "Trace Analysis", "VS", "VS Code", "Work Items"]
tags_normalized: ["dotnet", "ai", "application insights", "azure", "azure monitor", "blocking insights", "code recommendations", "coding", "debugging", "devops", "github copilot", "github issues", "news", "opentelemetry", "performance", "performance optimization", "profiler", "profiling", "trace analysis", "vs", "vs code", "work items"]
---

Chuck Weininger introduces Application Insights Code Optimizations, showing .NET developers how to leverage AI-powered performance recommendations with GitHub Copilot and Azure Monitor for more efficient application tuning.<!--excerpt_end-->

# Application Insights Code Optimizations: AI-Driven Performance Tuning for .NET Apps

## Overview

Application Insights Code Optimizations is designed to help .NET developers quickly identify and address application performance issues. By automatically analyzing profiler traces, it surfaces CPU and memory bottlenecks and provides actionable code-level recommendations—powered by AI and directly integrated with GitHub Copilot and Azure Monitor workflows.

## Key Features

- **Automatic Trace Analysis**: Integrates with the Application Insights .NET Profiler to analyze runtime data from your applications without requiring deep profiling expertise.
- **Actionable Recommendations**: Presents clear, code-level suggestions for performance improvements, viewable directly in the Azure portal.
- **GitHub Copilot Integration**: Seamlessly incorporate recommendations into your coding workflow using GitHub Copilot in Visual Studio or VS Code. Assign optimization tasks directly to the GitHub Copilot coding agent and manage them as GitHub Issues.
- **DevOps Workflow Support**: Recommendations can be tracked as work items in Azure DevOps or other tools, helping teams collaborate effectively and accelerate optimization efforts.
- **OpenTelemetry Support**: Preview support enables insights for applications instrumented with OpenTelemetry, simplifying instrumentation and analysis.

## How It Works

1. **Enable Application Insights and the .NET Profiler** for your application.
2. **Profiler collects trace data**, identifying inefficiencies like CPU spikes, memory leaks, blocking threads, and synchronous operations in async workflows.
3. **AI-driven analysis provides recommendations** visible within the Azure Portal and accessible through documentation links.
4. **Integrate with GitHub Copilot**: Assign performance-related GitHub Issues to the Copilot coding agent for intelligent remediation suggestions, speeding up code improvements.
5. **Track and manage optimizations** as work items in Azure DevOps or compatible platforms.

## What's New

- **Blocking Insights**: Now identifies issues affecting blocked or waiting threads in addition to active ones, revealing hidden bottlenecks.
- **Direct Copilot Assignment**: Assign GitHub Issues to the Copilot agent straight from the optimization page or from Application Insights crash analysis blades.
- **OpenTelemetry Profiler Preview**: Gain performance insights for OpenTelemetry-instrumented .NET apps without extra SDKs.

## Getting Started

- If you're using Application Insights, enable the .NET Profiler in your application settings.
- For new .NET apps, integrate Application Insights and activate Code Optimizations during setup.
- Explore recommendations in the Azure Portal, track them as work items, and use GitHub Copilot for automated code improvement guidance.
- Watch [the Azure Friday session with Scott Hanselman](https://www.youtube.com/watch?v=UUYAbRze3UA) for a live demo.
- Read the [documentation](https://aka.ms/CodeOptimizations/docs) to dive deeper.

## Community and Feedback

Chuck Weininger and the team welcome your feedback to make Code Optimizations even better. Share your experiences and suggestions at [https://aka.ms/CodeOptimizations/Feedback](https://aka.ms/CodeOptimizations/Feedback).

---

### Resources

- [Enable Code Optimizations](https://aka.ms/CodeOptimizations/docs)
- [Azure Friday Demo](https://www.youtube.com/watch?v=UUYAbRze3UA)
- [Provide Feedback](https://aka.ms/CodeOptimizations/Feedback)

## Conclusion

Application Insights Code Optimizations brings together .NET profiling, AI-powered recommendations, and GitHub Copilot automation to deliver a powerful, developer-focused solution for performance tuning. By streamlining diagnosis and remediation, it helps teams build faster, more reliable applications.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/application-insights-code-optimizations/)
