---
layout: "post"
title: "Carbon Aware Computing with .NET Open Source Libraries for Sustainable Applications"
description: "This session explores how to develop climate-friendly software using Green Software Development design patterns and open source .NET libraries. It covers carbon aware computing paradigms such as time-shifting, demand shaping, resource utilization, and dynamic scaling—demonstrating their practical implementation both on-premises, in Azure PaaS, and on Kubernetes. Real-world examples leverage CarbonAware SDK, Hangfire for job scheduling, Powershell for automation, and Kubernetes exporters for KEDA and Prometheus. Participants will learn how to dynamically adapt applications based on carbon intensity and access live power grid prediction data for optimal resource allocation."
author: "dotnet"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=IqSzmerSXuk"
viewing_mode: "internal"
feed_name: "DotNet YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCvtT19MZW8dq5Wwfu6B0oxw"
date: 2025-11-14 06:00:06 +00:00
permalink: "/2025-11-14-Carbon-Aware-Computing-with-NET-Open-Source-Libraries-for-Sustainable-Applications.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET", "Azure", "Azure PaaS", "CarbonAware SDK", "Cloud Computing", "CO2 Intensity Prediction", "Coding", "Demand Shaping", "DevOps", "Dynamic Scaling", "Green Software", "Hangfire", "Job Scheduling", "KEDA", "Kubernetes", "Open Source", "Powershell", "Prometheus", "Sustainability", "Time Shifting", "Videos"]
tags_normalized: ["dotnet", "azure", "azure paas", "carbonaware sdk", "cloud computing", "co2 intensity prediction", "coding", "demand shaping", "devops", "dynamic scaling", "green software", "hangfire", "job scheduling", "keda", "kubernetes", "open source", "powershell", "prometheus", "sustainability", "time shifting", "videos"]
---

dotnet delivers an in-depth talk on implementing carbon aware computing using .NET open source libraries, showcasing sustainable development patterns for Azure, on-premises, and Kubernetes environments.<!--excerpt_end-->

{% youtube IqSzmerSXuk %}

# Carbon Aware Computing with .NET Open Source Libraries for Sustainable Applications

**Speaker:** dotnet  
**Session:** .NET Conf 2025

## Overview

Modern software development faces the crucial challenge of minimizing its environmental impact. This session dives into practical methods to build climate-friendly and resource-efficient applications by leveraging existing Green Software Development design patterns and open source .NET libraries.

## Key Concepts Covered

- **Carbon Aware Computing Principle:** Software should align its resource usage with the availability of renewable energy, reducing carbon emissions by shifting workloads and adapting dynamically.  
- **Time-Shifting:** Scheduling plannable jobs (batch processes, background tasks) to run during periods where renewable energy dominates the grid supply.  
- **Demand Shaping:** Adapting application capacity and features to match available resource supply, especially for on-demand scenarios.

## Implementation Strategies

- **.NET Open Source Libraries:** The talk demonstrates how to use libraries like CarbonAware SDK for real-time carbon intensity data, integrate job scheduling with Hangfire, automate workloads using Powershell, and export relevant metrics with Kubernetes Exporter for KEDA & Prometheus.
- **Deployment Environments:** Solutions and demos span across on-premises, Azure PaaS, and Kubernetes clusters—showing flexibility for various architectures.
- **Dynamic Scaling and Load Shedding:** Methods to dynamically scale applications or limit functionality based on power grid carbon intensity are emphasized, alongside techniques to “shed” unnecessary load.
- **Data Integration:** The session covers how to source and consume prediction data about grid carbon intensity for smarter, environmentally adaptive applications.

## Technologies Demonstrated

- CarbonAware SDK
- Hangfire (.NET job scheduling)
- Powershell commandlets (automation)
- KEDA (Kubernetes-based event-driven autoscaling)
- Prometheus (monitoring/exporter)

## What You'll Learn

- How to schedule and shift workloads for sustainability (time-shifting)
- Design patterns for carbon-aware job execution
- Ways to automate and monitor environmental impact across development and production
- Applying open source .NET tools for sustainable scaling in cloud and containerized setups

## Further Resources

- [Playlist: .NET Conf 2025](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXtIlvq1tuORUtZqVG-HdCt)

---

By embracing these carbon aware design patterns, developers can reduce emissions and promote resource efficiency while maintaining high performance and scalability.
