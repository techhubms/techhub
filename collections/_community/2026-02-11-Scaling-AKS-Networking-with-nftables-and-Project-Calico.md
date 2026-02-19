---
layout: "post"
title: "Scaling AKS Networking with nftables and Project Calico"
description: "This in-depth technical guide explores how to enable Kubernetes service networking on Azure Kubernetes Service (AKS) using the new nftables mode in kube-proxy, with Project Calico for network security and visibility. The article covers background on iptables limitations, why Microsoft's preview support for nftables in AKS is relevant, detailed step-by-step setup, and how to leverage Calico and modern observability tools for network troubleshooting and visualization."
author: "jack4it"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/beyond-iptables-scaling-aks-networking-with-nftables-and-project/ba-p/4494467"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-11 23:29:23 +00:00
permalink: "/2026-02-11-Scaling-AKS-Networking-with-nftables-and-Project-Calico.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["AKS", "Azure", "Calico Whisker", "Cloud Native", "Cluster Scaling", "CNI", "Community", "Container Networking", "DevOps", "Iptables", "Kube Proxy", "Kubernetes", "Kubernetes Networking", "Linux Networking", "Load Balancing", "Microsoft Azure", "Microsoft Retina", "Network Policy", "Network Security", "Nftables", "Observability", "Project Calico", "Security"]
tags_normalized: ["aks", "azure", "calico whisker", "cloud native", "cluster scaling", "cni", "community", "container networking", "devops", "iptables", "kube proxy", "kubernetes", "kubernetes networking", "linux networking", "load balancing", "microsoft azure", "microsoft retina", "network policy", "network security", "nftables", "observability", "project calico", "security"]
---

jack4it presents a practical guide for enabling nftables mode in kube-proxy on Azure Kubernetes Service, leveraging Project Calico for advanced networking and security. The tutorial covers challenges with iptables at scale, Microsoft's new preview features, and modern observability tools.<!--excerpt_end-->

# Scaling AKS Networking with nftables and Project Calico

Author: Reza Ramezanpour (Senior Developer Advocate @ Tigera)

## Introduction

Kubernetes networking has been dominated by iptables, but increased workloads and stricter requirements for performance, security, and compliance are driving a shift toward nftables. Microsoft's Azure Kubernetes Service (AKS) now offers preview support for running kube-proxy in nftables mode, making it easier to scale clusters and improve networking efficiency.

## Why Move from iptables to nftables?

Large-scale clusters expose the hidden costs of iptables: linear rule scanning leads to higher CPU use, slower updates, and harder debugging as service endpoints grow. nftables, in contrast, uses fast map-based lookups and is now supported in Kubernetes v1.33+ and Project Calico v3.29+.

Microsoft’s decision to add preview support for nftables mode in AKS is a response to industry demand. Managed Kubernetes environments like AKS can now shed the scaling and complexity bottlenecks of iptables.

## Setting up AKS with nftables and Project Calico

### 1. Enable the Required AKS Preview Extensions

```sh
az extension add --name aks-preview
az extension update --name aks-preview
az feature register --namespace "Microsoft.ContainerService" --name "KubeProxyConfigurationPreview"
```

Wait until the feature registration completes before deploying.

### 2. Prepare kube-proxy Configuration

Create a minimal configuration file (`kube-proxy.json`):

```json
{ "enabled": true, "mode": "NFTABLES" }
```

### 3. Create the AKS Cluster

```sh
az group create --name nftables-demo --location canadacentral
az aks create \
  --resource-group nftables-demo \
  --name calico-nftables \
  --kube-proxy-config kube-proxy.json \
  --network-plugin none \
  --pod-cidr "10.10.0.0/16" \
  --generate-ssh-keys \
  --location canadacentral \
  --node-count 2 \
  --vm-size Standard_A8m_v2
```

### 4. Verify Kube-proxy is Using nftables

```sh
kubectl logs -n kube-system ds/kube-proxy | egrep "Proxier"

# Output should include: "Using nftables Proxier"
```

### 5. Install Project Calico

```sh
kubectl create -f https://docs.tigera.io/calico/latest/manifests/tigera-operator.yaml
kubectl create -f <your Calico installation YAML, e.g., installation.yaml>
```

If you are switching Calico to nftables data plane:

```sh
kubectl patch installation default --type=merge -p='{"spec":{"calicoNetwork":{"linuxDataplane":"Nftables"}}}'
```

## How nftables Improves Kubernetes Networking

- **Service Maps:** nftables uses high-speed map lookups to map service IPs and ports directly, avoiding linear rule traversal.
- **Dispatcher Chains:** Packets are efficiently handed off and SNAT’d to route correctly.
- **Dynamic Load Balancing:** Verdict maps with random generators enable consistent load distribution, adapting dynamically as replicas scale.
- **Efficient Scaling:** Adding replicas results in efficient updates to forwarding maps, not re-scanning/re-writing entire rule sets.

### Example: Deploying and Scaling a LoadBalancer Service

Deployment and service creation is standard, but load balancing behavior is now powered by fast verdict mapping in nftables. As you scale replicas, nftables efficiently adjusts logic, unlike iptables' growing complexity.

## Observability and Troubleshooting

- **Calico Whisker:** Visualizes network flows and policies in real time for easier debugging.
- **Microsoft Retina:** CNI-agnostic observability for packet drops, DNS, and TCP health; works across cloud platforms (https://retina.sh/).

## Conclusion

Shifting AKS and other Kubernetes environments to nftables represents a fundamental advance, overcoming iptables’ linear scaling limitations. With Azure and Project Calico support, platform engineers can build more scalable, secure, and observable clusters.

## References

- [Kubernetes 1.33 release: nftables kube-proxy backend](https://kubernetes.io/blog/2025/04/23/kubernetes-v1-33-release/#nftables-backend-for-kube-proxy)
- [Microsoft AKS Blog: nftables in kube-proxy](https://blog.aks.azure.com/2025/11/19/nftables-in-kube-proxy)
- [Calico Release Notes](https://docs.tigera.io/calico/3.29/release-notes/)
- [Calico Whisker Docs](https://docs.tigera.io/calico/latest/observability/view-flow-logs)
- [Microsoft Retina Observability](https://retina.sh/)

---
Published: Feb 11, 2026

---

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/beyond-iptables-scaling-aks-networking-with-nftables-and-project/ba-p/4494467)
