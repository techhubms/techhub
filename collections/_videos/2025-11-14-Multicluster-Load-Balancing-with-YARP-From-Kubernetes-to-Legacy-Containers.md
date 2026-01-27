---
external_url: https://www.youtube.com/watch?v=VIroNVbY0qI
title: 'Multicluster Load Balancing with YARP: From Kubernetes to Legacy Containers'
author: dotnet
feed_name: DotNet YouTube
date: 2025-11-14 19:20:50 +00:00
tags:
- .NET
- Architecture
- Cloud Native
- Container Orchestration
- Integration
- Kubernetes
- Legacy Modernization
- Load Balancing
- Microservices
- Production Infrastructure
- Routing
- Traffic Management
- Windows Containers
- YARP
section_names:
- coding
- devops
primary_section: coding
---
In this session, dotnet demonstrates how to architect a load balancer using YARP to streamline traffic management across multiple Kubernetes clusters and VM-hosted Windows Containers, offering practical insights on scaling, routing, and integrating legacy systems.<!--excerpt_end-->

{% youtube VIroNVbY0qI %}

# Multicluster Load Balancing with YARP: From Kubernetes to Legacy Containers

## Overview

This session dives into creating a production-ready load balancer with [YARP (Yet Another Reverse Proxy)](https://github.com/microsoft/reverse-proxy), focusing on routing traffic across distributed Kubernetes clusters and VM-hosted Windows Containers. The content shares hard-won lessons and practical strategies for modernizing and unifying traffic management in heterogeneous environments.

## Key Topics Covered

- **Architecture Decisions:**
  - Rationale for using YARP as a flexible, programmable reverse proxy
  - Integration points between Kubernetes services and legacy VM-based workloads

- **Routing Strategies:**
  - Design patterns for scalable, multicluster routing
  - How to handle failover, load distribution, and traffic shaping across clusters
  - Strategies for exposing legacy Windows Containers alongside Kubernetes applications

- **Integration Challenges:**
  - Addressing network, authentication, and compatibility issues between modern and legacy infrastructure
  - Techniques used to achieve seamless interoperability

## Implementation Insights

- **YARP Configuration:**
  - Configuring YARP for service discovery and dynamic endpoint management
  - Utilizing .NET 10 features for robust, maintainable proxy logic

- **Handling Legacy Workloads:**
  - Bridging the gap between container orchestrators and traditional VM-hosted services
  - Common pitfalls and mitigation strategies

## Microsoft Technologies Mentioned

- .NET 10 and .NET Aspire 13: For orchestration and service implementation
- ASP.NET Core 10: Modern web service development
- C# 14 and F# 10: Language innovations for backend services
- Visual Studio 2026: Productivity improvements for developer workflows

## Resources and Further Learning

- [YARP Documentation](https://microsoft.github.io/reverse-proxy/)
- [Kubernetes Official Docs](https://kubernetes.io/docs/)
- [.NET Conf 2025](https://aka.ms/dotnotconf2025/recap)

## Conclusion

Using YARP for hybrid cloud load balancing enables organizations to unify disparate workloads, modernize infrastructure strategically, and streamline traffic handling with a Microsoft-centric technology stack.

---

For more, follow dotnet on [Blog](https://aka.ms/dotnet/blog), [Docs](https://learn.microsoft.com/dotnet), and Microsoft’s Q&A and forums.
