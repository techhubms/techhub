---
external_url: https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/retina-1-0-is-now-available/ba-p/4489003
title: 'Retina 1.0 Released: Kubernetes Network Observability with Azure Monitor Integration'
author: kamilp
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-03 14:36:06 +00:00
tags:
- Azure
- Azure Log Analytics
- Azure Monitor
- Cloud Native
- Community
- DevOps
- Distributed Systems
- Ebpf
- Grafana
- Helm
- Hubble
- Kubernetes
- Linux
- Microsoft
- Network Observability
- Open Source
- Packet Capture
- Prometheus
- Retina
- Security
- Security Auditing
- SRE
- Telemetry
- Windows
section_names:
- azure
- devops
- security
---
kamilp from Microsoft announces Retina 1.0, introducing a powerful open-source Kubernetes network observability platform that integrates with Azure Monitor and other tools for deep cluster visibility, security, and troubleshooting.<!--excerpt_end-->

# Retina 1.0 Released: Kubernetes Network Observability with Azure Monitor Integration

**Author: kamilp (Microsoft)**

## Introduction

Retina 1.0 marks a major milestone as an open-source network observability platform tailored for Kubernetes. The project is maintained by Microsoft and aims to automate, centralize, and enrich network telemetry collection across diverse environments—including Azure Kubernetes Service (AKS) and other clouds.

## What is Retina?

Retina provides continuous network health monitoring, real-time and on-demand packet capture, and advanced troubleshooting for Kubernetes clusters. Core features include:

- **eBPF-powered kernel data collection** (on Linux; HNS/VFP on Windows)
- Deep integration with [Prometheus](https://prometheus.io/), [Grafana](https://grafana.com/), [Azure Monitor](https://learn.microsoft.com/azure/azure-monitor/overview), and Azure Log Analytics
- Support for Hubble and Standard control planes, and compatibility with any CNI
- Cloud-agnostic and Kubernetes distribution-agnostic

## Why Use Retina?

Traditional cluster troubleshooting at scale is complex due to ephemeral workloads, multi-layer networking, and distributed logs. Retina's approach:

- Centralizes collection of continuous metrics and on-demand packet captures
- Integrates Kubernetes awareness into all telemetry (e.g., Pod/Node metadata)
- Leverages eBPF for efficient, kernel-level event monitoring
- Exposes flexible plugin architecture for modular observability

## Key Capabilities

### Metrics

- Continually collects and exports key metrics such as dropped packets, TCP/UDP stats, API server latency, DNS failures, and interface statistics
- Integrates with Azure Monitor, Azure Log Analytics, Prometheus, and Grafana
- Pluggable collectors (e.g., Drop Reason, DNS, Packet Forward) enable custom observability

### Packet Captures

- Distributed pcap collection directly from specified nodes or pods
- CLI and CRD-based triggers for targeted diagnostics
- Output includes both raw capture (.pcap) and rich metadata (iptables, sockets, kernel info)
- Persistent storage to local, PVC, or blobs supported

### Troubleshooting Shell

- Interactive on-node shell with common tools (ping, curl) and advanced utilities (bpftool, pwru, Inspektor Gadget)
- Designed for manual diagnostics and deep networking forensics

## Use Cases

- **Pod Connectivity Debugging**: Correlate packet drops with service issues, accelerate root cause detection
- **Continuous Network Monitoring**: Dashboard visualization and alerting for critical metrics via Grafana or Azure tools
- **Security Auditing & Compliance**: Flow logs and metrics help investigate incidents, support compliance, and identify unauthorized connections
- **Multi-Cloud/Cluster Visibility**: Unified insight across environments

## Architecture

- **Agents** run on each node, collecting events from Linux (eBPF) or Windows (HNS/VFP)
- **User-space processing** applies Kubernetes metadata enrichment
- Data is exported to monitoring stacks (Azure Monitor, Prometheus, Grafana, Hubble UI)
- **Plugin-based design**: Add or remove observability functions as needed

Learn more about the design at [architecture docs](https://retina.sh/docs/Introduction/architecture).

## Deployment and Getting Started

- Ship with [Helm charts](https://helm.sh/) for streamlined installation
- Supports AKS, EKS, GKE, and self-managed clusters
- Example quickstart:

  ```bash
  VERSION=$( curl -sL https://api.github.com/repos/microsoft/retina/releases/latest | jq -r .name)
  helm upgrade --install retina oci://ghcr.io/microsoft/retina/charts/retina \
      --version $VERSION \
      --namespace kube-system \
      --set image.tag=$VERSION \
      --set operator.tag=$VERSION \
      --set logLevel=info \
      --set operator.enabled=true \
      --set enabledPlugin_linux="[dropreason,packetforward,linuxutil,dns]"
  ```

- [Configure Prometheus](https://retina.sh/docs/Installation/prometheus) and [Grafana](https://retina.sh/docs/Installation/grafana) for visualization
- Install CLI via Krew:

  ```bash
  kubectl krew install retina
  ```

Full setup details at [retina.sh](https://retina.sh/docs/Installation/Setup)

## Community and Contribution

- Retina is open source (MIT), hosted at [GitHub · microsoft/retina](https://github.com/microsoft/retina)
- Maintained by Microsoft with a growing group of contributors
- See [contributor guide](https://retina.sh/docs/Contributing/overview) for involvement and roadmap

## References and Further Reading

- [Official Retina docs](https://retina.sh/)
- [Deep Dive into Retina Open-Source Kubernetes Network Observability](https://www.srodi.com/posts/kubernetes-ebpf-observability-retina-deepdive/)
- [Troubleshooting Network Issues with Retina](https://techcommunity.microsoft.com/blog/linuxandopensourceblog/troubleshooting-network-issues-with-retina/4446071)
- [Retina: Bridging Kubernetes Observability and eBPF Across the Clouds](https://techcommunity.microsoft.com/blog/linuxandopensourceblog/ebpf-powered-observability-beyond-azure-a-multi-cloud-perspective-with-retina/4403361)

---
*For questions, suggestions, or contributions, see the [GitHub issues](https://github.com/microsoft/retina/issues) or [start a discussion](https://github.com/microsoft/retina/discussions). Contact the Retina team at [retina@microsoft.com](mailto:retina@microsoft.com).*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/retina-1-0-is-now-available/ba-p/4489003)
