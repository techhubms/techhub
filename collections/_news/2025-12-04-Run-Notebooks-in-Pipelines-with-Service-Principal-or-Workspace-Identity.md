---
layout: "post"
title: "Run Notebooks in Pipelines with Service Principal or Workspace Identity"
description: "This announcement highlights the new capability in Microsoft Fabric Data Factory pipelines to configure Notebook activity authentication using Service Principal (SPN) or Workspace Identity (WI). This change improves production reliability, enterprise-grade security, and future-proof automation by using secure connection properties instead of user-based credentials."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/run-notebooks-in-pipelines-with-service-principal-or-workspace-identity/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-12-04 11:00:00 +00:00
permalink: "/news/2025-12-04-Run-Notebooks-in-Pipelines-with-Service-Principal-or-Workspace-Identity.html"
categories: ["Azure", "ML", "Security"]
tags: ["Azure", "Connection Property", "Data Factory", "Enterprise Security", "Governance", "Identity Management", "Microsoft Fabric", "ML", "News", "Notebook Activity", "Operational Reliability", "Pipeline Automation", "Production Workloads", "Security", "Service Principal", "SPN", "WI", "Workspace Identity"]
tags_normalized: ["azure", "connection property", "data factory", "enterprise security", "governance", "identity management", "microsoft fabric", "ml", "news", "notebook activity", "operational reliability", "pipeline automation", "production workloads", "security", "service principal", "spn", "wi", "workspace identity"]
---

Microsoft Fabric Blog introduces secure authentication methods for running Notebook activities in Data Factory pipelines, focusing on Service Principal and Workspace Identity to help automate and secure production workflows.<!--excerpt_end-->

# Run Notebooks in Pipelines with Service Principal or Workspace Identity

Microsoft Fabric Data Factory pipelines now support a connection property for Notebook activities, enabling more secure and reliable production workflows.

## What's New?

- **New Authentication Options**: Use Service Principal (SPN) or Workspace Identity (WI) to authenticate Notebook activities in pipelines.
- **Production-Ready Approach**: Recommended for production environments.

### Key Benefits

- **Operational Reliability**: Eliminates issues associated with user credentials (e.g., password changes, user deactivation).
- **Enterprise-Grade Security**: Service-based authentication reduces security risks and simplifies compliance.
- **Consistent Automation**: Pipelines run without manual intervention, allowing resilient scheduling and orchestration.

![Notebook activity has the option for Connection property now](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/12/image-1024x495.png)

## Why it Matters

Previously, user authentication could cause broken workflows if users left the organization or tokens expired. The new connection property allows centralized identity management and scalable notebook execution in complex workflows. This ensures better governance and future-proof automation for data processing and machine learning workloads.

## How to Get Started

1. In your pipeline, add a **Notebook activity**.
2. Under **Connection**, create or select a connection.
3. Configure the necessary credentials or identity.
4. Run your pipeline for secure, automated notebook execution.

For more details, see the documentation:

- [Notebook activity – Microsoft Fabric](https://learn.microsoft.com/fabric/data-factory/notebook-activity)

---

This update helps organizations improve security and automation in their ML and data engineering workflows while leveraging Azure-based identity solutions.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/run-notebooks-in-pipelines-with-service-principal-or-workspace-identity/)
