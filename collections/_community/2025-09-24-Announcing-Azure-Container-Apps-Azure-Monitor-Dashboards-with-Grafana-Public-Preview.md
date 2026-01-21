---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-the-public-preview-of-azure-container-apps-azure/ba-p/4450958
title: Announcing Azure Container Apps Azure Monitor Dashboards with Grafana (Public Preview)
author: carolinauribe
feed_name: Microsoft Tech Community
date: 2025-09-24 17:52:49 +00:00
tags:
- Azure Container Apps
- Azure Monitor
- Cloud Operations
- Containers
- Dashboards
- Grafana
- Metrics
- Microsoft Azure
- Monitoring
- Observability
- Performance Monitoring
- Public Preview
- RBAC
- Serverless
- Visualization
section_names:
- azure
- devops
---
carolinauribe details the new public preview of Azure Container Apps Azure Monitor Dashboards with Grafana, explaining how teams can now easily monitor and analyze app performance using built-in, customizable dashboards in the Azure portal.<!--excerpt_end-->

# Announcing the Public Preview of Azure Container Apps Azure Monitor Dashboards with Grafana

Microsoft has introduced the public preview of Azure Container Apps Azure Monitor Dashboards with Grafana, aiming to streamline observability for containerized applications.

## What's New

- **Native Grafana Dashboards in Azure Portal**: View and manage Grafana dashboards directly within the Azure portal for your Azure Container Apps and environments, with no extra setup or additional cost.
- **Pre-built Dashboards**:
  - **Container App View**: Access vital metrics such as CPU usage, memory usage, request rates, replica restarts, and more for individual apps.
  - **Environment View**: Get an overview of all container apps within an environment, including details like revision names, scaling settings (min/max replicas), and resource allocations.

## Key Benefits

- **Centralized Observability**: No more switching between separate tools; monitor all your container apps from the Azure portal.
- **Easy Customization**: Use Grafana’s native visualization features to tailor dashboards to your team's needs.
- **Open Source Compatibility**: Dashboards are portable and compatible with any Grafana instance.
- **Secure Sharing**: Leverage Azure Role-Based Access Control (RBAC) to manage and share dashboards securely within your organization.

## How to Get Started

1. Open the Azure portal and navigate to your Azure Container App or its environment.
2. Access the Monitoring section and select “Dashboards with Grafana (Preview)”.
3. View, customize, and monitor metrics as needed.

For step-by-step guidance, visit: [aka.ms/aca/grafana](https://aka.ms/aca/grafana)

## Additional Resources

- **Grafana Dashboard Gallery**: Browse and use community-created dashboards at [Grafana Dashboard Gallery](https://grafana.com/grafana/dashboards/).
- **Azure Managed Grafana Templates**:
  - [Container App View](https://grafana.com/grafana/dashboards/16592-azure-container-apps-container-app-view/)
  - [Aggregate View](https://grafana.com/grafana/dashboards/16591-azure-container-apps-aggregate-view/)
- [More Azure dashboards from the community](https://grafana.com/orgs/azure/dashboards)

---

*Published by carolinauribe | Version 1.0 | Sep 24, 2025*

Follow the Apps on Azure Blog for future updates.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-the-public-preview-of-azure-container-apps-azure/ba-p/4450958)
