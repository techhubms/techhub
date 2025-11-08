---
layout: "post"
title: "Layer 7 Network Policies for AKS: General Availability for Enterprise-Grade Security"
description: "This article announces the general availability of Layer 7 (L7) Network Policies for Azure Kubernetes Service (AKS). It explains how these policies, powered by Cilium and Advanced Container Networking Services, enhance security for containerized workloads through application-aware controls, FQDN filtering, and cluster-wide policy enforcement. It provides hands-on examples and operational guidance for Microsoft Azure practitioners."
author: "KhushbuP"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-networking-blog/layer-7-network-policies-for-aks-now-generally-available-for/ba-p/4467598"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-08 03:12:57 +00:00
permalink: "/2025-11-08-Layer-7-Network-Policies-for-AKS-General-Availability-for-Enterprise-Grade-Security.html"
categories: ["Azure", "Security"]
tags: ["Advanced Container Networking", "AKS", "Azure", "CCNP", "Cilium", "Cluster Security", "Community", "Container Networking", "Egress Controls", "FQDN Filtering", "Grafana", "Kubernetes", "Layer 7 Network Policy", "Network Observability", "Policy Enforcement", "Security", "Zero Trust"]
tags_normalized: ["advanced container networking", "aks", "azure", "ccnp", "cilium", "cluster security", "community", "container networking", "egress controls", "fqdn filtering", "grafana", "kubernetes", "layer 7 network policy", "network observability", "policy enforcement", "security", "zero trust"]
---

KhushbuP presents a detailed overview of the general availability of Layer 7 Network Policies for Azure Kubernetes Service (AKS), explaining how these new capabilities enhance security and observability for containerized workloads.<!--excerpt_end-->

# Layer 7 Network Policies for AKS: General Availability for Enterprise-Grade Security

**Author:** KhushbuP

Azure Kubernetes Service (AKS) now supports Layer 7 (L7) Network Policies in General Availability (GA), leveraging Cilium and Advanced Container Networking Services (ACNS). This milestone enables Azure users to secure microservices with fine-grained, application-aware controls for mission-critical workloads.

## What Are L7 Network Policies?

L7 Network Policies allow you to define security rules for Kubernetes workloads based on specific application-layer protocols (like HTTP methods and paths), not just L3 (IP) or L4 (port) traffic. This enhances defense-in-depth by enforcing policies such as:

- Allowing only specific HTTP methods (e.g., GET, POST)
- Restricting access to designated URL paths
- Preventing lateral movement from compromised services

## Practical Example: Securing a Multi-Tier Retail Application

Consider a retail application running on AKS with three microservices:

- **frontend-app**: Presents the user interface and product information
- **inventory-api**: Backend for product stock levels (read-only for the frontend)
- **payment-gateway**: Processes transactions (accepts only POSTs from the frontend)

**Challenge:** A traditional L4 policy can't stop a compromised frontend from sending harmful requests (like DELETE) to the inventory API.

**L7 Solution:** With Cilium L7 policies, restrict the frontend to only send safe HTTP requests (GET) to the inventory API:

```yaml
apiVersion: cilium.io/v2
kind: CiliumNetworkPolicy
metadata:
  name: protect-inventory-api
spec:
  endpointSelector:
    matchLabels:
      app: inventory-api
  ingress:
  - fromEndpoints:
    - matchLabels:
        app: frontend-app
    toPorts:
    - ports:
      - port: "8080"
        protocol: TCP
      rules:
        http:
        - method: "GET"
          path: "/api/inventory/.*"
```

- **Allowed:** Frontend GET requests to `/api/inventory/item123`
- **Blocked:** Any DELETE/POST requests, even from compromised pods

Apply the same pattern for the payment-gateway, ensuring only POSTs to `/process-payment` are accepted.

## Zero Trust Network Security with Enhanced Controls

L7 policies complement traditional L3/L4 restrictions and advanced egress controls, including:

- **FQDN Filtering:** Allow egress only to approved external domains.
- **Cluster-Wide Policies (CCNP):** Use CiliumClusterwideNetworkPolicy to enforce uniform policies across all namespaces.

### CCNP Example: Egress Policy with FQDN Filtering

```yaml
apiVersion: cilium.io/v2
kind: CiliumClusterwideNetworkPolicy
metadata:
  name: allow-egress-to-example-com
spec:
  endpointSelector: {}
  egress:
    - toFQDNs:
        - matchPattern: "*.example.com"
      toPorts:
        - ports:
            - port: "443"
              protocol: TCP
            - port: "80"
              protocol: TCP
```

This restricts all pods to outbound connections only on ports 80 and 443 to subdomains of example.com.

## Operational Observability

Security is only as strong as its visibility. All L7 policy actions and blocks are auditable in Azure Managed Grafana dashboards, simplifying threat detection and incident response.

## Getting Started

To enable L7 policies in your AKS cluster:

1. Follow the [Layer 7 Policy Overview](https://aka.ms/acns/l7policy)
2. See [How to Apply L7 Policy](https://aka.ms/acns/l7policy-how-to) for a guided scenario

Upgrade today to leverage application-aware security and observability, and help Microsoft improve by sharing your feedback.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/layer-7-network-policies-for-aks-now-generally-available-for/ba-p/4467598)
