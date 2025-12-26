---
layout: "post"
title: "Powering Distributed AI and ML Workloads at Scale with Azure and Anyscale"
description: "This news article details Microsoft’s partnership with Anyscale to deliver managed Ray—a distributed computing framework for Python—on Azure. It covers how Anyscale's managed Ray service runs atop Azure Kubernetes Service (AKS), enabling teams to orchestrate large AI/ML workloads with enterprise-grade governance, scaling, and integrated Azure services. The announcement targets developers and data practitioners seeking to leverage distributed computing for powering sophisticated AI and ML pipelines on Azure’s robust cloud infrastructure."
author: "Brendan Burns"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/all-things-azure/powering-distributed-aiml-at-scale-with-azure-and-anyscale/"
viewing_mode: "external"
feed_name: "Microsoft All Things Azure Blog"
feed_url: "https://devblogs.microsoft.com/all-things-azure/feed/"
date: 2025-11-04 12:30:11 +00:00
permalink: "/news/2025-11-04-Powering-Distributed-AI-and-ML-Workloads-at-Scale-with-Azure-and-Anyscale.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "AI Apps", "AI Workloads", "AKS", "All Things Azure", "Anyscale", "App Development", "Appdev", "Azure", "Azure Monitor", "Cloud Scale", "Containers", "Distributed Computing", "Kubernetes", "Machine Learning", "Microsoft Entra ID", "ML", "Model Training", "News", "Python", "Ray", "RayTurbo"]
tags_normalized: ["ai", "ai apps", "ai workloads", "aks", "all things azure", "anyscale", "app development", "appdev", "azure", "azure monitor", "cloud scale", "containers", "distributed computing", "kubernetes", "machine learning", "microsoft entra id", "ml", "model training", "news", "python", "ray", "rayturbo"]
---

Brendan Burns explains how Microsoft and Anyscale are collaborating to bring managed Ray to Azure, empowering developers to scale distributed AI and ML workloads seamlessly with Python and Kubernetes.<!--excerpt_end-->

# Powering Distributed AI/ML at Scale with Azure and Anyscale

**Author: Brendan Burns**

Scaling artificial intelligence (AI) and machine learning (ML) workloads from experimentation to production is complex. Microsoft and Anyscale have partnered to bring Anyscale’s managed Ray service to Azure in private preview, simplifying distributed Python workloads with an enterprise-ready, cloud-native experience.

## Ray: Distributed Computing Framework for Python

Ray is an open-source framework designed to make distributed computing accessible to Python developers. Developers can:

- Scale applications from a single laptop to large clusters with minimal code changes
- Leverage Pythonic APIs that transform functions and classes into distributed tasks and actors
- Use native libraries for:
   - **Ray Train** (distributed training)
   - **Ray Data** (data processing)
   - **Ray Serve** (model serving)
   - **Ray Tune** (hyperparameter tuning)
- Integrate with PyTorch, TensorFlow, and other ML libraries

Ray abstracts distributed infrastructure concerns, enabling teams to focus on model development and innovation.

## Anyscale: Enterprise-Grade Ray on Azure

Anyscale, the company founded by Ray’s creators, delivers a managed Ray service—now tightly integrated with Azure. This new service introduces RayTurbo, a performance-focused Ray runtime. Key capabilities on Azure include:

- Instantly spinning Ray clusters from the Azure Portal or CLI (no Kubernetes expertise needed)
- Dynamic allocation of tasks across CPUs, GPUs, and mixed clusters
- Elastic scaling and support for Azure spot VMs
- Production reliability: automatic fault recovery, zero-downtime upgrades, built-in observability
- Data locality and governance: Clusters run inside your Azure subscription with unified billing and compliance

## Azure Kubernetes Service (AKS): Foundation for Distributed AI/ML

AKS provides:

- Dynamic resource orchestration (scaling clusters as demand shifts)
- High availability (self-healing and failover)
- Elastic scaling for clusters of any size
- Native integration with Azure services like Azure Monitor, Microsoft Entra ID, and Blob Storage
- Enterprise governance, security, and compliance

AKS supports Ray and Anyscale at every scale, from development clusters to global, mission-critical deployments.

## Empowering Teams: AI and ML Innovation at Scale

Microsoft and Anyscale’s partnership enables organizations to:

- Easily move from prototype to production for AI/ML
- Adopt distributed Python computing without deep infrastructure expertise
- Integrate Ray’s open ecosystem with Azure-native tools and governance
- Start small for experimentation and scale to full enterprise deployments

Developers benefit from more choice, flexibility, and focus—with less operational effort and complexity. Further information about preview access and Azure Marketplace availability is provided in the article.

## Learn More

- [Learn about private preview and request access](https://aka.ms/anyscale)
- [Subscribe to Anyscale on Azure Marketplace](https://marketplace.microsoft.com/en-us/product/saas/anyscale1750870039553.anyscale-2025-1?tab=Overview)
- [Read the official release](https://www.anyscale.com/press/anyscale-collaborates-with-microsoft-to-deliver-ai-native-computing-on-azure)

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/powering-distributed-aiml-at-scale-with-azure-and-anyscale/)
