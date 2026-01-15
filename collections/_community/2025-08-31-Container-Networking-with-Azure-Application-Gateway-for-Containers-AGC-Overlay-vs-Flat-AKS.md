---
layout: post
title: 'Container Networking with Azure Application Gateway for Containers (AGC): Overlay vs. Flat AKS'
author: lakshaymalik
canonical_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/container-networking-with-azure-application-gateway-for/ba-p/4449941
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-08-31 15:28:07 +00:00
permalink: /coding/community/Container-Networking-with-Azure-Application-Gateway-for-Containers-AGC-Overlay-vs-Flat-AKS
tags:
- AKS
- ALB Controller
- Application Gateway For Containers
- Azure
- Azure CNI
- Azure Firewall
- Calico
- Cilium
- Coding
- Community
- Container Networking
- DevOps
- Gateway API
- Ingress Controller
- Kubernetes Networking
- Network Policies
- NSG
- Overlay Networking
- Security
- Subnet Design
- YAML
section_names:
- azure
- coding
- devops
- security
---
lakshaymalik delivers a comprehensive look at deploying Azure Application Gateway for Containers (AGC) with AKS, comparing overlay and flat network models, security policies, and step-by-step Gateway API setup.<!--excerpt_end-->

# Container Networking with Azure Application Gateway for Containers (AGC): Overlay vs. Flat AKS

## Introduction

This post by lakshaymalik demystifies how Azure Application Gateway for Containers (AGC) integrates with Azure Kubernetes Service (AKS) networking. It covers both Overlay (Azure CNI Overlay) and Flat (Azure CNI Pod/Node Subnet) models, provides architectural insights, sample Gateway API YAML, and practical tips for networking and security in a production scenario.

## Architecture Overview

- **Frontend:** Public or private AGC entry point from the internet
- **Gateway Layer:** AGC with its dedicated /24 subnet, acting as a gateway between the frontend and AKS nodes
- **AKS Node Subnets:** Where AKS nodes and pods reside; traffic is routed here depending on network model

### Networking Models Supported

#### 1. Azure CNI Overlay (Overlay networking)

- **Use Case:** Conserve VNet IPs, simplify management, support large clusters
- **How it Works:** Pods receive IPs from an Overlay CIDR, separate from VNet. AGC extends the overlay routing so it can reach pods directly.
- **Requirements:** ALB Controller v1.7.9+ for overlay support. Policies using Azure NP, Calico, or Cilium are supported.

#### 2. Flat networking (Azure CNI Pod/Node Subnet)

- **Use Case:** Direct pod IP accessibility from on-prem or peered VNets with ample VNet IP space
- **How it Works:** Pods receive VNet-routable IPs directly. AGC forwards traffic into the VNet as standard.
- **Trade-off:** Requires careful subnet planning to prevent IP exhaustion.

**AGC auto-detects your model**—no need to change Gateway/Ingress specs when switching.

## Security & Operations

- **Network Security Groups (NSGs)** and **Azure Firewall** can be enforced at various boundaries
- Kubernetes Network Policies are respected by AGC, including engines like Azure NP, Calico, or Cilium
- Supports Layer-7 routing, mTLS to backends, path/host/header-based routing, and near real-time convergence with GitOps/CI workflows
- **Subnet for AGC must be /24** and only **one AGC deployment per subnet** is supported
- **ALB Controller v1.7.9+** required for overlay
- AGC integrates with Private Link, availability zones, managed identity, and supports both Gateway API and Ingress resources

## Step-by-Step: Deploy AGC with Gateway API

**Prerequisites:**

- AKS cluster with the ALB Controller for AGC installed (v1.7.9+ for Overlay)
- Dedicated AGC subnet (/24), properly assigned

**1. Deploy an Application**

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: hello
  labels:
    app: hello
spec:
  replicas: 3
  selector:
    matchLabels:
      app: hello
  template:
    metadata:
      labels:
        app: hello
    spec:
      containers:
      - name: web
        image: mcr.microsoft.com/azuredocs/aks-helloworld:v1
        ports:
        - containerPort: 80
---
apiVersion: v1
kind: Service
metadata:
  name: hello
spec:
  selector:
    app: hello
  ports:
  - port: 80
    targetPort: 80
```

**2. Define GatewayClass and Gateway for AGC**

```yaml
apiVersion: gateway.networking.k8s.io/v1
kind: GatewayClass
metadata:
  name: azure-alb
spec:
  controllerName: alb.networking.azure.io/gateway-controller
---
apiVersion: gateway.networking.k8s.io/v1
kind: Gateway
metadata:
  name: web-gw
  namespace: default
spec:
  gatewayClassName: azure-alb
  listeners:
  - name: http
    protocol: HTTP
    port: 80
```

*Tip:* If your cluster ships with an existing gateway class (`azure-albGatewayClass`), reuse it. Check with:

```
kubectl get gatewayclasses -o wide
```

**3. Route traffic with HTTPRoute**

```yaml
apiVersion: gateway.networking.k8s.io/v1
kind: HTTPRoute
metadata:
  name: hello-route
spec:
  parentRefs:
  - name: web-gw
  hostnames:
  - "hello.example.com"
  rules:
  - matches:
    - path:
        type: PathPrefix
        value: /
    backendRefs:
    - name: hello
      port: 80
```

**4. (Optional) Add Redirect and Rewrite Policies**

```yaml
apiVersion: gateway.networking.k8s.io/v1
kind: HTTPRoute
metadata:
  name: hello-redirect
spec:
  parentRefs:
  - name: web-gw
  hostnames:
  - "hello.example.com"
  rules:
  - matches:
    - path:
        type: PathPrefix
        value: "/old"
    filters:
    - type: RequestRedirect
      requestRedirect:
        statusCode: 301
        path:
          type: ReplacePrefixMatch
          replacePrefixMatch: "/"
    backendRefs:
    - name: hello
      port: 80
```

**5. Deploy Resources**

```
kubectl apply -f hello.yaml
kubectl apply -f gateway.yaml
kubectl apply -f route.yaml
```

Once the Gateway is `Accepted=True` and listeners are programmed, a public/private IP is assigned. Point your DNS (e.g., hello.example.com) to the AGC frontend IP.

## Subnetting, Peering, and Version Notes

- AGC subnet must be /24; deploy only one AGC per subnet
- Avoid mixing AGC and AKS nodes across Azure regions or using global VNet peering for AGC-to-AKS
- For Overlay, always confirm ALB Controller version is ≥ 1.7.9
- Maintain consistent network policy configuration

## Summary

Azure Application Gateway for Containers offers a native, flexible, and highly integrated solution for AKS ingress. It supports both overlay and flat networking models, automates model detection, and enforces enterprise-grade security and compliance with support for various network policies, firewalls, and integrations into Azure-native services.

Refer to the YAML guides above to get started and leverage AGC for robust, scalable ingress in Azure Kubernetes deployments.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/container-networking-with-azure-application-gateway-for/ba-p/4449941)
