---
layout: "post"
title: "Reduce Metrics Noise and Costs with Container Network Metrics Filtering in ACNS for AKS"
description: "This article introduces the Container Network Metrics Filtering feature in Azure Container Networking Services (ACNS) for Azure Kubernetes Service (AKS), now in Public Preview. Learn how to filter metrics at the source to optimize costs, declutter dashboards, and gain more precise control over network observability using Kubernetes-native filtering and Cilium agents. The guide covers the reason for metrics bloat, key benefits of fine-grained filtering, technical how-to, real-world YAML CRD example, and links for deeper learning."
author: "KhushbuP"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-networking-blog/cut-the-noise-cost-with-container-network-metrics-filtering-in/ba-p/4468221"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-08 04:16:56 +00:00
permalink: "/community/2025-11-08-Reduce-Metrics-Noise-and-Costs-with-Container-Network-Metrics-Filtering-in-ACNS-for-AKS.html"
categories: ["Azure", "DevOps"]
tags: ["ACNS", "AKS", "Azure", "Azure Container Networking Services", "Cilium", "Community", "ContainerNetworking", "Cost Optimization", "CRD", "Custom Resource Definitions", "DevOps", "Ebpf", "Grafana", "Kubernetes", "Metrics Filtering", "Network Observability", "Prometheus"]
tags_normalized: ["acns", "aks", "azure", "azure container networking services", "cilium", "community", "containernetworking", "cost optimization", "crd", "custom resource definitions", "devops", "ebpf", "grafana", "kubernetes", "metrics filtering", "network observability", "prometheus"]
---

KhushbuP presents an in-depth look at Container Network Metrics Filtering in ACNS for AKS, empowering teams to control Kubernetes network metrics and reduce cost and noise from unnecessary data.<!--excerpt_end-->

# Cut the Noise & Cost with Container Network Metrics Filtering in ACNS for AKS

**Author**: KhushbuP

## Overview

Container Network Metrics Filtering is a new capability in Azure Container Networking Services (ACNS) for Azure Kubernetes Service (AKS), now in Public Preview. This feature enables Kubernetes users to control which network metrics are collected and exported, directly addressing the problems of metrics bloat, excessive operational costs, and dashboard clutter.

## Why Excessive Metrics Are a Problem

Modern microservices environments often suffer from collecting large volumes of irrelevant metrics, which leads to:

- High storage and ingestion costs
- Cluttered dashboards
- Operational overhead and slower queries

Using granular filter controls, you can now ensure that only the most relevant metrics are sent to your observability stack.

## Key Benefits

- **Fine-Grained Control**: Filter by namespace or pod label to target vital services and exclude irrelevant data.
- **Cost Optimization**: Lower storage and ingestion costs across monitoring tools like Prometheus and Grafana.
- **Improved Observability**: Cleaner dashboards and faster troubleshooting by focusing on high-value, actionable metrics.
- **Dynamic, Zero-Downtime Updates**: Change or apply new filters at runtime without restarting Cilium agents or Prometheus collectors.

## How It Works

Filtering happens at the Cilium agent level, inside the Linux kernel's data plane. You define your metrics filters using the `ContainerNetworkMetric` Custom Resource Definition (CRD), allowing you to include or exclude specific metric types (e.g., DNS lookups, TCP connections, dropped flow metrics).

This approach reduces the volume of metrics before they ever leave the node, ensuring the data sent to your observability tools is already curated.

## Example: Filtering Flow Metrics to Reduce Noise

Below is a sample `ContainerNetworkMetric` CRD that includes only dropped flows from the `traffic/http` namespace and excludes all flows from `traffic/fortio` pods:

```yaml
apiVersion: acn.azure.com/v1alpha1
kind: ContainerNetworkMetric
metadata:
  name: container-network-metric
spec:
  filters:
    - metric: flow
      includeFilters:
        # Include only DROPPED flows from traffic namespace
        verdict:
          - "dropped"
        from:
          namespacedPod:
            - "traffic/http"
      excludeFilters:
        # Exclude traffic/fortio flows to reduce noise
        from:
          namespacedPod:
            - "traffic/fortio"
```

## Getting Started

1. **Enable ACNS:** Ensure [ACNS is enabled](https://aka.ms/acns) on your AKS cluster.
2. **Define Your Filters:** Create and apply your `ContainerNetworkMetric` CRD according to your include/exclude logic. See the [how-to guide](https://aka.ms/acns/filteringhowto) for details.
3. **Validate:** Use ConfigMap inspection and Cilium agent logs to verify active filters.
4. **See Results:** Observe metrics volume reduction, clearer dashboards, and lower ingestion costs.

For more information, visit the [Metrics Filtering Guide](https://aka.ms/acns/container-network-metrics-filtering).

## Conclusion

Container Network Metrics Filtering in ACNS for AKS provides powerful, Kubernetes-native controls for network observability, helping teams cut through monitoring noise while optimizing cost and operational efficiency.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/cut-the-noise-cost-with-container-network-metrics-filtering-in/ba-p/4468221)
