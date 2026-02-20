---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/azure-arc-jumpstart-template-for-hybrid-logic-apps-deployment/ba-p/4493996
title: Deploying Hybrid Logic Apps with Azure Arc on AKS Using Jumpstart Templates
author: Shree_Divya_M_V
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-10 03:43:20 +00:00
tags:
- AKS
- Automation
- Azure
- Azure Arc
- Azure Logic Apps Standard
- Azure SQL Server
- Azure Storage Account
- Community
- Community Feedback
- Custom Location
- Deployment Script
- DevOps
- Enterprise Integration
- Hybrid Deployment
- Integration
- Jumpstart Template
- Kubernetes
section_names:
- azure
- devops
---
Shree_Divya_M_V shares an actionable overview for deploying Azure Logic Apps (Standard) in a hybrid AKS environment using Azure Arc Jumpstart templates, focusing on automation and enterprise integration.<!--excerpt_end-->

# Deploying Hybrid Logic Apps with Azure Arc on AKS Using Jumpstart Templates

This article presents a practical approach to setting up a hybrid Azure Logic Apps (Standard) deployment on Azure Arc-enabled AKS clusters. The Azure Arc Jumpstart template enables automation of this entire process with a single command, aimed at simplifying complex, enterprise-grade integration scenarios.

## Key Concepts and Components

- **Azure Arc-Enabled AKS**: Onboards your Kubernetes clusters into Azure for unified governance and management.
- **Hybrid Logic Apps (Standard)**: Enables running Logic Apps workflows outside Azure—on-premises, private cloud, or other cloud providers.
- **ACA Extension, Custom Location, Connected Environment**: Facilitates management and connectivity between Azure services and the AKS cluster.
- **Azure SQL Server**: Provides storage for Logic Apps runtime state.
- **Azure Storage Account**: Used for SMB artifacts storage.

## Step-by-Step Deployment

1. **Review Prerequisites**: Check permissions and requirements outlined in the Jumpstart documentation ([link](https://jumpstart.azure.com/azure_jumpstart_drops?repo=arc_jumpstart_drops&branch=main&drop=Hybrid%20deployment%20for%20Azure%20Logic%20Apps%20on%20AKS)).
2. **Clone the Repo**: Download the official logic app Jumpstart template repository from [GitHub](https://github.com/ShreeDivyaMV/LogicApps_Jumpstart_Templates).
3. **Single Command Deployment**: Execute the provided script to provision all resources (AKS, Azure Arc, extensions, storage, etc.) in one go.
4. **Test Your Deployment**: Run validation commands as per documentation to ensure all services are operational.

## Customization & Community Feedback

- The deployment script can be adjusted to suit specific requirements (scalability, integration scenarios, etc.).
- The template is community-driven; feedback, contributions, and scenario suggestions are welcome via GitHub.

## References & Further Reading

- [Getting Started with Hybrid Logic Apps (Standard)](https://learn.microsoft.com/en-us/azure/logic-apps/set-up-standard-workflows-hybrid-deployment-requirements)
- [Jumpstart Drop: Hybrid Deployment for Logic Apps on AKS](https://jumpstart.azure.com/azure_jumpstart_drops?repo=arc_jumpstart_drops&branch=main&drop=Hybrid%20deployment%20for%20Azure%20Logic%20Apps%20on%20AKS)
- [Announcement of General Availability](https://techcommunity.microsoft.com/blog/integrationsonazureblog/announcement-general-availability-of-logic-apps-hybrid-deployment-model/4422414)
- [GitHub Repository - LogicApps Jumpstart Templates](https://github.com/ShreeDivyaMV/LogicApps_Jumpstart_Templates)

## Feedback

Have ideas for new features or find a bug? Provide feedback or contribute to the open source template on the [associated GitHub repo](https://github.com/ShreeDivyaMV/LogicApps_Jumpstart_Templates).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/azure-arc-jumpstart-template-for-hybrid-logic-apps-deployment/ba-p/4493996)
