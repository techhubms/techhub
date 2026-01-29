---
external_url: https://blog.fabric.microsoft.com/en-US/blog/run-notebooks-in-pipelines-with-service-principal-or-workspace-identity/
title: Run Notebooks in Pipelines with Service Principal or Workspace Identity
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-12-04 11:00:00 +00:00
tags:
- Connection Property
- Data Factory
- Enterprise Security
- Governance
- Identity Management
- Microsoft Fabric
- Notebook Activity
- Operational Reliability
- Pipeline Automation
- Production Workloads
- Service Principal
- SPN
- WI
- Workspace Identity
- Azure
- Machine Learning
- Security
- News
section_names:
- azure
- ml
- security
primary_section: ml
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
