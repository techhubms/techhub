---
layout: post
title: Announcing the General Availability of Azure Kubernetes Service (AKS) Automatic
author: sbaynes
canonical_url: https://azure.microsoft.com/en-us/blog/azure-kubernetes-service-automatic-fast-and-frictionless-kubernetes-for-all/
viewing_mode: external
feed_name: Microsoft News
feed_url: https://news.microsoft.com/source/feed/
date: 2025-09-16 18:00:02 +00:00
permalink: /ai/news/Announcing-the-General-Availability-of-Azure-Kubernetes-Service-AKS-Automatic
tags:
- AI
- AKS
- AKS Automatic
- Automation
- Autoscaling
- Azure
- Azure Linux
- Azure Monitor
- CI/CD
- Cloud Infrastructure
- Cloud Native
- Company News
- Container Orchestration
- Developer Tools
- DevOps
- Enterprise IT
- GitHub Actions
- Karpenter
- KEDA
- Kubernetes
- Microsoft Entra ID
- News
- Production Clusters
- Scale
- Technology
section_names:
- ai
- azure
- devops
---
sbaynes announces the general availability of Azure Kubernetes Service (AKS) Automatic, detailing how this managed offering simplifies Kubernetes for developers and enterprises.<!--excerpt_end-->

# Announcing the General Availability of Azure Kubernetes Service (AKS) Automatic

Azure Kubernetes Service Automatic (AKS Automatic) aims to remove the traditional friction and complexity associated with Kubernetes, making it faster and simpler for both new and experienced users to deliver applications at scale. Below is an overview of the key capabilities, benefits, and ways to get started as outlined by sbaynes.

## What is AKS Automatic?

AKS Automatic is a fully managed, production-ready Kubernetes solution delivered by Azure. It offers:

- **Pre-configured, optimized clusters:** Create clusters ready for deployment without manual configuration.
- **Automated operations:** Node provisioning, scaling, patching, and upgrades are handled by Azure.
- **Security and reliability out of the box:** Default hardened configurations, Microsoft Entra ID integration, RBAC, network policies, and automatic updates.
- **Developer-centric experience:** Retains full Kubernetes API access, kubectl compatibility, and supports existing workflows and CI/CD pipelines.
- **Optimized for AI and cloud-native applications:** GPU support, intelligent resource management, and seamless integration with Azure App Service and Azure Container Apps.
- **Open-source aligned:** Built on conformant Kubernetes with open-source tools like KEDA (for event-driven scaling) and Karpenter (for dynamic autoscaling).

## Key Features

- **Easy onboarding:** One-click provision via Azure Portal or CLI.
- **Intelligent autoscaling:** HPA/VPA for pods, KEDA and Karpenter for workload and node scaling.
- **Production defaults:** Azure CNI, Azure Linux node images, Azure Monitor integration, deployment safeguards, and automatic node repair.
- **Enterprise and startup benefits:** Suitable for teams without dedicated DevOps, and for large IT organizations needing consistent, secure Kubernetes at scale.
- **AI/ML readiness:** Enhanced support for compute-intensive tasks and AI/ML workflows through GPU resource allocation and optimized scaling.
- **Seamless integration:** Works with GitHub Actions and other CI/CD pipelines, Azure Arc, and enterprise governance tools.

## Getting Started

1. **Choose AKS Automatic when creating a new AKS cluster via the Azure Portal or Azure CLI.**
2. **Review the [documentation and quickstarts](https://learn.microsoft.com/azure/aks/intro-aks-automatic)** for step-by-step guidance.
3. **Test by migrating existing workloads**—point your kubectl context to the new cluster and deploy with existing manifests or Helm charts.

## Community and Resources

- **Global virtual launch event:** Participate or watch the recap to learn about real-world use cases.
- **Documentation and guides:** Up-to-date resources on Microsoft Learn.
- **Community engagement:** Monthly community calls and GitHub issues for feedback and future feature discussion.

## Summary

AKS Automatic delivers a simplified, secure, and scalable Kubernetes experience for developers and platform engineers. By automating operational overhead and offering best-practice defaults, AKS Automatic frees teams to focus on innovation and application logic. The service is available now and integrates with the broader Azure ecosystem, supporting AI, ML, and cloud-native scenarios.

To get started or learn more, visit the [AKS Automatic documentation](https://learn.microsoft.com/azure/aks/intro-aks-automatic) or [Azure product page](https://azure.microsoft.com/products/kubernetes-service).

This post appeared first on "Microsoft News". [Read the entire article here](https://azure.microsoft.com/en-us/blog/azure-kubernetes-service-automatic-fast-and-frictionless-kubernetes-for-all/)
