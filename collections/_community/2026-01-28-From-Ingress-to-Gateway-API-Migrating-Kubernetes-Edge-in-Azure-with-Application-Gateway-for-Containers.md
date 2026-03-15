---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/from-ingress-to-gateway-api-a-pragmatic-path-forward-and-why-it/ba-p/4489779
title: 'From Ingress to Gateway API: Migrating Kubernetes Edge in Azure with Application Gateway for Containers'
author: Jack Stromberg
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-01-28 23:19:40 +00:00
tags:
- AKS
- ALB Controller
- Application Gateway For Containers
- Automation
- Azure
- Azure Application Gateway
- Azure Monitor
- Cloud Migration
- Cluster Networking
- Community
- DevOps
- Gateway API
- IaC
- Ingress Controller
- Kubernetes
- Kubernetes Networking
- Load Balancer
- NGINX
- Rolling Deployment
- Security
- SLA
- TLS
- Web Application Firewall
- YAML Migration
section_names:
- azure
- devops
- security
---
Jack Stromberg delivers a hands-on walkthrough for AKS operators transitioning from Ingress NGINX to the Gateway API using Azure Application Gateway for Containers. Practical migration steps, security posture, and tooling are highlighted.<!--excerpt_end-->

# From Ingress to Gateway API: Migrating Kubernetes Edge in Azure with Application Gateway for Containers

*Author: Jack Stromberg*

The Kubernetes networking ecosystem is undergoing a significant shift as the community-led Ingress NGINX controller approaches end-of-life. This development makes adopting the Kubernetes Gateway API more urgent, especially for those running workloads on Azure Kubernetes Service (AKS).

## Why Is This Important Now?

- **Ingress NGINX Retirement:** The community's ingress-nginx controller will be sunsetted, with support ending March 2026. The Ingress API itself is frozen—new features will only appear in Gateway API.
- **Azure Guidance:** Microsoft recommends AKS customers prepare to migrate to Gateway API before November 2026. Security patches for the NGINX-based add-on will end, making future-proofing necessary.

## The New Edge Model: Gateway API

*Gateway API* provides:

- **Better separation of concerns** (GatewayClass, Gateway, HTTPRoute, etc.)
- **Simplified platform vs. application responsibilities**
- **Portability and reduced vendor lock-in**

## Platform Choice: Azure Application Gateway for Containers

A managed L7 load balancer tightly integrated with AKS, supporting Gateway API natively. Benefits include:

- Enterprise-grade support and SLA
- Azure-managed security patching
- WAF integration with global intelligence
- Out-of-cluster architecture for blast radius reduction
- Seamless integration with Azure Monitor, RBAC, Policy

## Practical Migration Steps

1. **Inventory all existing Ingress resources** (including annotations, TLS, routes)
2. **Stand up Application Gateway for Containers** (choose between BYO or controller-managed modes)
3. **Incrementally migrate services** by converting Ingress manifests to Gateway API YAML
4. **Validate routing and security parity**
5. **Cut over and retire legacy controllers when confidence is high**

### Tooling Support: Application Gateway for Containers Migration Utility

- Open source CLI tool to translate Ingress YAML (including NGINX/AGIC annotations) into Gateway API YAML
- Review and adjust output to fit environment specifics ([GitHub link](https://aka.ms/agc/migrationutility))

## Operational Advantages

- Offloading patching/updates to Azure
- Rapid, near real-time configuration updates
- Azure-native monitoring and governance
- No re-implementation of basic features: mutual TLS, WAF, canary deployments, rollouts

## Security and Governance

- Deep WAF integration with current rulesets and threat intelligence
- Azure Policy enforcement at the edge
- RBAC integration for multi-team operations

## Phased Rollout Checklist

**Phase 1:** Define your desired end-state (Gateway API-first or temporary Ingress compatibility)
**Phase 2:** Deploy AGC alongside existing ingress, set up test service
**Phase 3:** Use migration tool, validate and incrementally transition services
**Cut over** once parity and observability goals are reached

## Conclusion

With the sunsetting of Ingress NGINX, AKS teams should act now to avoid future risk and benefit from the flexibility, security, and operational simplicity of Gateway API and Azure Application Gateway for Containers. Microsoft and the Kubernetes community provide migration tools and support to minimize disruption and accelerate adoption.

### Further Reading and Resources

- [Application Gateway for Containers](https://aka.ms/agc)
- [AGC Migration Utility](https://aka.ms/agc/migrationutility)
- [Kubernetes: Ingress NGINX Retirement](https://kubernetes.io/blog/2025/11/11/ingress-nginx-retirement/)
- [AKS Application Routing Add-on Update](https://blog.aks.azure.com/2025/11/13/ingress-nginx-update)
- [Microsoft Learn: Migration Overview](https://aka.ms/agc/migrate)
- [Microsoft Learn: Quickstart (Managed by ALB Controller)](https://learn.microsoft.com/azure/application-gateway/for-containers/quickstart-managed-by-alb-controller)
- [Microsoft Learn: Quickstart (BYO Deployment)](https://learn.microsoft.com/azure/application-gateway/for-containers/quickstart-bring-your-own-deployment)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/from-ingress-to-gateway-api-a-pragmatic-path-forward-and-why-it/ba-p/4489779)
