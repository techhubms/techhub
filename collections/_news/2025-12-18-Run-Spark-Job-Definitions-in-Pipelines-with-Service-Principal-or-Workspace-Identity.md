---
external_url: https://blog.fabric.microsoft.com/en-US/blog/run-spark-job-definitions-in-pipelines-with-service-principal-or-workspace-identity/
title: Run Spark Job Definitions in Pipelines with Service Principal or Workspace Identity
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-12-18 14:00:00 +00:00
tags:
- Authentication
- Automated Workflows
- Data Engineering
- Data Factory
- Enterprise Security
- Identity Management
- Microsoft Fabric
- Notebook Automation
- Pipeline Orchestration
- Production Pipelines
- Service Principal
- Spark Job Definition
- Workspace Identity
- Azure
- ML
- Security
- News
section_names:
- azure
- ml
- security
primary_section: ml
---
Microsoft Fabric Blog details how to securely orchestrate Spark job definitions in Data Factory pipelines using Service Principal or Workspace Identity authentication, providing developers with best practices for automation and enterprise security.<!--excerpt_end-->

# Run Spark Job Definitions in Pipelines with Service Principal or Workspace Identity

The Spark job definition activity in Microsoft Fabric Data Factory pipelines now supports the *connection property*, enabling a more secure and production-ready method to run Spark Job Definitions (SJD).

## What's New?

With this update, you can configure notebook activities to run as:

- **Service Principal (SPN) authentication**
- **Workspace Identity (WI) authentication**

These methods are recommended for production environments, ensuring:

- **Operational reliability**: Minimizing issues from user credential changes or deactivation.
- **Enterprise-grade security**: Utilizing service-based authentication for better risk management and compliance.
- **Consistent automation**: Allowing pipelines to run smoothly without manual intervention.

![SJD supports connection property now](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/12/image-23-1024x492.png)

## Why it Matters

Previously, Data Factory pipelines often relied on user authentication—causing potential disruptions due to employee departures or expired tokens. By adopting the connection property with SPN and WI, users benefit from:

- **Scalable orchestration** for complex notebook-based workflows
- **Improved governance** through centralized identity management
- **Future-proof automation** for critical production workloads

## How to Get Started

1. Add a **Spark job definition activity** in your Data Factory pipeline.
2. In the **Connection** section, choose to configure a new connection or select an existing one.
3. Specify credentials or identity configurations, including SPN and WI.
4. Run your pipeline to experience secure and automated job execution.

For further details, refer to the [Spark Job Definition activity documentation](https://learn.microsoft.com/fabric/data-factory/spark-job-definition-activity).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/run-spark-job-definitions-in-pipelines-with-service-principal-or-workspace-identity/)
