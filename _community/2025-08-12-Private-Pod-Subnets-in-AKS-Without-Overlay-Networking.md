---
layout: "post"
title: "Private Pod Subnets in AKS Without Overlay Networking"
description: "This in-depth community article explains approaches to managing IP address space when deploying Azure Kubernetes Service (AKS) clusters, focusing on alternatives to overlay networks, using Azure CNI Pod Subnet, and configuring IP masquerading (NAT) with the ip-masq-agent-v2 DaemonSet. Readers learn how to balance network routability, pod addressability, and IP efficiency in complex or IP-constrained enterprise environments."
author: "samcogan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/private-pod-subnets-in-aks-without-overlay-networking/ba-p/4442510"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-12 13:36:46 +00:00
permalink: "/2025-08-12-Private-Pod-Subnets-in-AKS-Without-Overlay-Networking.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["AKS", "Azure", "Azure CNI", "Azure Firewall", "Coding", "Community", "ConfigMap", "DaemonSet", "DevOps", "Egress Gateway", "ExpressRoute", "Ip Masq Agent V2", "IP Masquerading", "Kubernetes", "Kubernetes Networking", "Network Address Translation", "Network Routing", "Node Subnet", "Overlay Network", "Pod Subnet", "Virtual Network", "VNet Peering", "VNets"]
tags_normalized: ["aks", "azure", "azure cni", "azure firewall", "coding", "community", "configmap", "daemonset", "devops", "egress gateway", "expressroute", "ip masq agent v2", "ip masquerading", "kubernetes", "kubernetes networking", "network address translation", "network routing", "node subnet", "overlay network", "pod subnet", "virtual network", "vnet peering", "vnets"]
---

In this community article, samcogan breaks down how to manage private pod subnets in Azure Kubernetes Service (AKS) without overlay networking, addressing NAT challenges and actionable deployment steps.<!--excerpt_end-->

# Private Pod Subnets in AKS Without Overlay Networking

When deploying AKS (Azure Kubernetes Service) clusters, a common challenge is dealing with limited IP address space, especially in corporate networks. This article explores solutions for running AKS without relying solely on overlay networking, focusing on Azure CNI Pod Subnet configurations and practical ways to handle outbound network routing.

## Overlay Networking: The Default and Its Limits

- **Overlay Network** is the simplest and most common IP addressing approach for AKS, letting pods use a large, private non-routed address space.
- AKS manages address translation between routable and non-routed networks.
- **Limitation**: Cannot address pods directly from the corporate network; all communication must use Kubernetes Services. Some advanced features (like Virtual Nodes) aren’t available.

## Azure CNI Pod Subnet: An Alternative

- By using Azure CNI Pod Subnet, you set up a vNet with separate subnets for nodes and pods.
- Nodes’ subnet remains routable; pods subnet can be private and as large as needed, helping conserve scarce corporate IPs.
- To address pods directly, you must be within the AKS vNet or peer another network—a challenge for some architectures.

## The Routing Problem

- Pods in the Pod Subnet may not be able to reach or be reached by other networks if their private IPs aren’t routable outside the AKS vNet.
- E.g., peered VNets or corporate networks via ExpressRoute/VPN won’t route back to pod IPs, leading to failed connections.

## IP Masquerading (NAT) Solutions

Several methods exist to resolve this routing issue:

- Deploy **Azure Firewall** or a custom Network Virtual Appliance (NVA) to NAT traffic, converting pod IPs to routable ones.
- Simpler and more cost-effective: **use the AKS nodes themselves for NAT** by applying Network Address Translation (NAT) via ip-masq-agent-v2.
- **ip-masq-agent-v2** is a DaemonSet for Kubernetes that modifies node iptables to masquerade pod traffic as node IPs for outbound network flows.

## Deploying ip-masq-agent-v2

1. **Deploy the DaemonSet**: The agent runs on each node, ensuring iptables rules are applied cluster-wide. Use the [official example manifest](https://github.com/Azure/ip-masq-agent-v2/blob/master/examples/ip-masq-agent.yaml), but update to the latest version.
2. **Create a ConfigMap**: Specify IP ranges to exempt from masquerading (e.g., the pod subnet, the node subnet, service CIDR) using `nonMasqueradeCIDRs`. Only traffic headed to other places gets NAT’d. Don't enable link-local masquerading.

```yaml
apiVersion: apps/v1
kind: DaemonSet
metadata:
  name: ip-masq-agent
  namespace: kube-system
  labels:
    component: ip-masq-agent
    kubernetes.io/cluster-service: "true"
    addonmanager.kubernetes.io/mode: Reconcile
spec:
  selector:
    matchLabels:
      k8s-app: ip-masq-agent
  template:
    metadata:
      labels:
        k8s-app: ip-masq-agent
    spec:
      hostNetwork: true
      containers:
      - name: ip-masq-agent
        image: mcr.microsoft.com/aks/ip-masq-agent-v2:v0.1.15
        imagePullPolicy: Always
        securityContext:
          privileged: false
          capabilities:
            add: ["NET_ADMIN", "NET_RAW"]
        volumeMounts:
        - name: ip-masq-agent-volume
          mountPath: /etc/config
          readOnly: true
      volumes:
      - name: ip-masq-agent-volume
        projected:
          sources:
          - configMap:
              name: ip-masq-agent-config
              optional: true
              items:
              - key: ip-masq-agent
                path: ip-masq-agent
                mode: 0444
```

**ConfigMap Example:**

```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: ip-masq-agent-config
  namespace: kube-system
  labels:
    component: ip-masq-agent
    kubernetes.io/cluster-service: "true"
    addonmanager.kubernetes.io/mode: EnsureExists
data:
  ip-masq-agent: |
    nonMasqueradeCIDRs:
      - 10.0.0.0/16 # VNet and service CIDR
      - 192.168.0.0/16 # Pod subnet
    masqLinkLocal: false
    masqLinkLocalIPv6: false
```

- Deploy DaemonSet and ConfigMap in the same namespace.
- The `nonMasqueradeCIDRs` section lets you fine-tune which destinations use the real pod IP versus a NAT’d node IP.

## Summary and Recommendations

- For most deployments, overlay networking keeps networking simple and scalable.
- If you need pod subnet mode (for features or direct reachability), you can still preserve non-routed, private pod IPs and leverage NAT via ip-masq-agent-v2 for outbound traffic.
- This approach balances IP conservation, network security, and operational flexibility in IP-constrained AKS environments.

---
**Further Reading**

- [AKS Overlay Networking (Microsoft Docs)](https://learn.microsoft.com/en-us/azure/aks/azure-cni-overlay?tabs=kubectl)
- [ip-masq-agent-v2 repository](https://github.com/Azure/ip-masq-agent-v2)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/private-pod-subnets-in-aks-without-overlay-networking/ba-p/4442510)
