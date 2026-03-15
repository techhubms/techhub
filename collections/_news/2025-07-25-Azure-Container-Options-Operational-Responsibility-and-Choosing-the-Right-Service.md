---
external_url: https://devblogs.microsoft.com/all-things-azure/azure-container-options-matching-services-to-operational-responsibility/
title: 'Azure Container Options: Operational Responsibility and Choosing the Right Service'
author: Shree Chinnasamy, Priyanka Vergadia
feed_name: Microsoft DevBlog
date: 2025-07-25 21:01:29 +00:00
tags:
- ACI
- AKS
- All Things Azure
- Automation
- Azure Container Apps
- Azure Container Instances
- CI/CD
- Cloud Modernization
- Cloud Native
- Container Orchestration
- Containers
- IaaS
- Kubernetes
- Microservices
- Migration
- Modernization
- Operational Responsibility
- Operations
- Opinion
- PaaS
- Serverless Containers
- Shared Responsibility Model
- Thought Leadership
- Azure
- DevOps
- News
section_names:
- azure
- devops
primary_section: azure
---
Written by Shree Chinnasamy and Priyanka Vergadia, this article helps Azure customers navigate the process of choosing the best Azure container service by examining operational responsibility, use cases, and the shared responsibility model. It offers clear distinctions between ACI, ACA, and AKS, with practical guidance.<!--excerpt_end-->

# Azure Container Options: Matching Services to Operational Responsibility

*By Shree Chinnasamy, Priyanka Vergadia*

Modernizing applications on the cloud often leads to the essential question: **Which Azure container service should you choose?** Customers frequently ask whether to select one container service, combine options, or even use an A+B approach. This article offers a modular, requirements-first mindset to guide your decision, ensuring you use the right tool for your app's needs and your team's operational responsibility.

Many organizations are shifting away from a strict "A or B" selection towards an integrated "A+B" ecosystem when hosting applications on Azure. Below, you'll find considerations and comparisons meant to clarify the operational responsibility associated with various Azure container offerings.

## Key Considerations

When selecting Azure container services, begin by clarifying your application's use case alongside all functional and non-functional requirements. Consider:

- **Implementation speed**: Which option is quickest to deploy?
- **Cost effectiveness**: How does it affect build, deployment, and operational expenses?
- **Operational simplicity**: Is it easy to manage and run?
- **Integration and automation**: Does it align well with DevOps, CI/CD, or other integrations?
- **Extensibility**: Can you leverage open-source projects (especially those from the CNCF ecosystem) as your solution scales?

This list is not exhaustive—other factors may be important for your scenario. Let’s move forward by examining the operational responsibilities for Azure’s container offerings.

## Understanding the Shared Responsibility Model

Choosing a container service requires familiarity with the **shared responsibility model**, which describes how responsibilities shift between the customer and Azure as you move up the cloud stack. This helps you avoid overengineering, spend less effort on maintenance, and optimize for cost and confidence. [See Microsoft documentation on shared responsibility in the cloud](https://learn.microsoft.com/en-us/azure/security/fundamentals/shared-responsibility) for more details.

Below, we break down Azure’s most popular container services—ACI, ACA, and AKS—by their service models, operational controls, and sample use cases.

### **1. Azure Container Instances (ACI)**

- **Category:** Platform-as-a-Service (PaaS), bordering on Infrastructure-as-a-Service (IaaS) for certain layers.
- **Description:** ACI allows users to run individual containers or container groups (similar to pods) without managing underlying infrastructure—providing serverless, on-demand workloads.
- **Operational Model:**
  - Azure handles: Security "OF" the cloud and runtime environment (physical servers, host OS, container runtime).
  - You handle: Security "IN" the container and application (your app, container config).
- **Example Use Cases:**
  - Isolated workloads like unit testing, CI/CD pipelines, or batch/data processing scripts.
  - You provide the application inside the container, Azure manages infrastructure below it.
- **When to choose ACI:**
  - For simple, short-lived, isolated tasks. If autoscaling or advanced orchestration are needed, consider other services.

### **2. Azure Container Apps (ACA)**

- **Category:** PaaS
- **Description:** ACA adds advanced capabilities (autoscaling, traffic management, native integrations with Dapr/KEDA) for web APIs and microservices—building on the serverless principles of ACI but designed for complex, dynamic workloads.
- **Operational Model:**
  - Azure handles: Security "OF" the platform, underlying infrastructure, high availability, integrated ingress controller, Kubernetes management.
  - You handle: Security "IN" the application and container, container image, autoscaling configuration.
- **Example Use Cases:**
  - API backends and microservices needing to handle unpredictable traffic, integrate with pub/sub messaging, autoscale on demand.
- **When to choose ACA:**
  - If you want managed scaling and modern microservice patterns but don’t require direct orchestrator control or extensive open-source plugin support.

### **3. Azure Kubernetes Service (AKS)**

- **Category:** PaaS with significant IaaS elements
- **Description:** AKS offers a managed Kubernetes cluster for full control over container orchestration and scaling. Azure manages the Kubernetes control plane, while you manage the worker node VMs and Kubernetes objects deployed on them.
- **Operational Model:**
  - Azure handles: Security "OF" the managed control plane, availability and patching, and lower-level infrastructure.
  - You handle: Security "IN" the worker node VMs, Kubernetes configurations and objects, custom networking, and advanced setups.
- **Example Use Cases:**
  - Complex workloads needing Kubernetes orchestration—such as microservices architectures, distributed systems, custom networking, integrations from the CNCF ecosystem, and bespoke security or resource governance.
- **When to choose AKS:**
  - For maximum control, extensibility, and flexibility at the cost of additional operational overhead.

#### **Shared Responsibility Model Summary Table**

| Service                        | Azure Responsible For                            | Customer Responsible For            |
|--------------------------------|-------------------------------------------------|-------------------------------------|
| Azure Container Instances (ACI)| Underlying infrastructure, runtime environment  | Container & app configuration       |
| Azure Container Apps (ACA)     | Platform, scaling, networking                   | Container image, scaling config     |
| Azure Kubernetes Service (AKS) | Kubernetes control plane, infra security/patches| Worker nodes, app config, networking|

## Other Azure Container Solutions

Azure also provides:

- [Azure Red Hat OpenShift](https://azure.microsoft.com/en-us/products/openshift/)
- [Azure Arc-enabled Kubernetes](https://learn.microsoft.com/en-us/azure/azure-arc/kubernetes/)
- Self-managed Kubernetes on [Azure Virtual Machines](https://learn.microsoft.com/en-us/azure/virtual-machines/overview)

These add options for hybrid, fully managed, or OpenShift-based scenarios.

## Conclusion

Choosing the right Azure container service is a matter of matching *operational responsibility*, control requirements, and use case complexity. Azure provides the flexibility to start with simple serverless containers and scale up to complex, extensible Kubernetes-managed workloads—helping to modernize apps with confidence.

## Additional References

1. [Choose an Azure container service](https://learn.microsoft.com/en-us/azure/architecture/guide/choose-azure-container-service)
2. [General architectural considerations for choosing an Azure container service](https://learn.microsoft.com/en-us/azure/architecture/guide/container-service-general-considerations)
3. [Compute options for microservices](https://learn.microsoft.com/en-us/azure/architecture/microservices/design/compute-options)
4. [Azure Containers product page](https://azure.microsoft.com/en-us/products/category/containers/)

---

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/azure-container-options-matching-services-to-operational-responsibility/)
