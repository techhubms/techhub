---
layout: "post"
title: "Simplify Application Monitoring for AKS with Azure Monitor (Public Preview)"
description: "This guide introduces developers and operators to new unified monitoring capabilities in Azure Monitor for Azure Kubernetes Service (AKS), now in Public Preview. It covers seamless onboarding, OpenTelemetry support, auto-instrumentation for Java and NodeJS workloads, integration with Application Insights and Container Insights, and curated Grafana dashboards. Readers will learn how to enable, configure, and extend application monitoring for AKS to achieve automated, full-stack observability and troubleshooting in the Azure ecosystem."
author: "austonli"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-observability-blog/simplify-application-monitoring-for-aks-with-azure-monitor/ba-p/4470136"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 16:15:50 +00:00
permalink: "/community/2025-11-18-Simplify-Application-Monitoring-for-AKS-with-Azure-Monitor-Public-Preview.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["AKS", "AKS Monitoring", "App Performance", "Application Insights", "Auto Instrumentation", "Azure", "Azure Monitor", "Azure Portal", "Cloud Native", "Coding", "Community", "Container Insights", "CRD", "DevOps", "Grafana Dashboards", "Java", "Kubernetes", "Namespaces", "NodeJS", "OpenTelemetry", "Preview Features", "Telemetry Collection", "Troubleshooting"]
tags_normalized: ["aks", "aks monitoring", "app performance", "application insights", "auto instrumentation", "azure", "azure monitor", "azure portal", "cloud native", "coding", "community", "container insights", "crd", "devops", "grafana dashboards", "java", "kubernetes", "namespaces", "nodejs", "opentelemetry", "preview features", "telemetry collection", "troubleshooting"]
---

austonli details Azure Monitor's new public preview features for unified application monitoring on AKS, empowering developers with seamless onboarding and advanced observability.<!--excerpt_end-->

# Simplify Application Monitoring for AKS with Azure Monitor (Public Preview)

_Author: austonli_

## Overview

Azure Monitor has introduced public preview capabilities for unified, automated application and infrastructure monitoring in Azure Kubernetes Service (AKS). This update empowers teams to onboard monitoring faster, collect richer telemetry data (using OpenTelemetry standards), and visualize performance across their entire AKS stack directly within the Azure Portal and with curated Grafana dashboards.

## Key Features

### 1. Seamless Onboarding

- Directly enable application monitoring from the AKS cluster blade in Azure Portal.
- Simple, two-step process: Activate monitoring in cluster settings and select namespaces for targeting.
- Support for Custom Resource Definitions (CRDs) allows granular, per-deployment enablement.

### 2. First-Class OpenTelemetry Support

- Built-in support for OpenTelemetry data collection.
- Auto-instrumentation for Java and NodeJS applications—no code changes needed.
- Use the AzureMonitor OpenTelemetry distribution; telemetry flows to Application Insights automatically.
- Available for both portal UI and CLI workflows.

### 3. Unified Troubleshooting

- Switch between infrastructure (Container Insights) and application (Application Insights) views within Azure Portal.
- Improved navigation and curated OpenTelemetry workbooks help surface and diagnose problematic requests or failures.
- Enables cross-layer observability for rapid root-cause analysis.

### 4. Full-Stack Grafana Dashboards

- [Azure-hosted Grafana dashboards](https://learn.microsoft.com/en-us/azure/azure-monitor/app/grafana-dashboards) designed for Application Insights and OpenTelemetry data.
- Bring together traces, dependencies, exceptions, and infrastructure metrics for microservices troubleshooting.
- Customizable: Extend dashboards with custom OTel metrics or deeper Application Insights dimensions.

## How to Get Started

1. **Enable Monitoring**: Go to Monitor Settings in your AKS cluster and enable application monitoring.
2. **Select Namespaces**: Choose application namespaces and configure onboarding (optionally, use CRDs for fine-grained control).
3. **Auto-Instrument Apps**: Instrument Java/NodeJS workloads without touching your codebase; all telemetry routes to Application Insights.
4. **Use Workbooks and Dashboards**: Explore unified workbooks and Grafana dashboards for both infrastructure and application insights.

**Documentation & More Info:**

- [AKS Codeless Monitoring docs](https://learn.microsoft.com/en-us/azure/azure-monitor/app/kubernetes-codeless)
- [OpenTelemetry features blog](https://aka.ms/igniteotelblog)

## Practical Benefits

- **No code modification required for most workloads**—onboarding and data collection is handled via Azure integrations.
- **Rapid troubleshooting** by correlating app traces and infra metrics side by side.
- **End-to-end visibility** for cloud-native workloads, including microservices and distributed applications on AKS.

## What You Can Achieve

- Monitor, diagnose, and optimize AKS app performance with minimal effort.
- Visualize both application and infrastructure health in unified dashboards.
- Use Azure-provided OpenTelemetry standards for future-proof observability integrations.

---

To take advantage of these new public preview features, explore the provided documentation above and get started with application monitoring for AKS today.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/simplify-application-monitoring-for-aks-with-azure-monitor/ba-p/4470136)
