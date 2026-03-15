---
external_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/amba-alz-pattern-learn-about-the-latest-and-greatest/ba-p/4458320
title: 'AMBA-ALZ Pattern: Recent Enhancements to Built-in Policies and Role-Based Access in Azure'
author: BrunoGabrielli
feed_name: Microsoft Tech Community
date: 2025-10-08 13:49:40 +00:00
tags:
- AMBA ALZ
- Azure CLI
- Azure Deployment
- Azure Governance
- Azure Monitor
- Azure Pipelines
- Azure PowerShell
- Azure Role Based Access Control
- Azure Service Health
- Built in Policy
- GitHub Actions
- Managed Identities
- Monitoring Policy Contributor
- Operational Excellence
- Policy Management
- Security Best Practices
- Terraform
- Azure
- DevOps
- Security
- Community
section_names:
- azure
- devops
- security
primary_section: azure
---
BrunoGabrielli introduces key security and governance improvements to the AMBA-ALZ pattern for Azure, detailing the new Service Health built-in policy and the Monitoring Policy Contributor role.<!--excerpt_end-->

# AMBA-ALZ Pattern: Recent Enhancements to Built-in Policies and Role-Based Access in Azure

**Author:** BrunoGabrielli  
**Published:** October 8, 2025

## Overview

In this update, BrunoGabrielli shares two major enhancements to the AMBA-ALZ (Azure Monitor Baseline Alerts - Azure Landing Zones) pattern, significantly improving operational governance, security, and policy management in Azure environments:

- Adoption of a new Azure Service Health built-in policy (available as of October 2025)
- Introduction of the least privileged "Monitoring Policy Contributor" Azure role for managed identities

## 1. Azure Service Health Built-in Policy

A new built-in policy named **"Configure subscriptions to enable service health alert monitoring rule"** is now part of the "Deploy Azure Monitor Baseline Alerts (AMBA-ALZ) for Service Health and Resource Health" initiative.

### Key Points

- **Availability:** Effective from October 1, 2025
- **Purpose:** Allows customers who permit only Azure’s built-in policies to use Service Health monitoring.
- **Trust & Compliance:** Ensures feature parity with previous custom policies, increasing trust in ALZ.
- **Deployment:**
  - **New Deployments:** Default behavior, no action required.
  - **Existing Deployments:** Some pre-deployment steps are required. Detailed guidance is available in the [adoption documentation](https://azure.github.io/azure-monitor-baseline-alerts/patterns/alz/HowTo/Switch_To_BuiltIn_Sha/Switch_To_LeastPrivileges/).
- **Combined with:** The custom Resource Health policy remains part of the initiative.

## 2. Monitoring Policy Contributor Role

To address security concerns relating to overprovisioned permissions (frequently flagged by Microsoft Defender for Cloud), a new least-privileged Azure role named **Monitoring Policy Contributor** was developed jointly with the Azure RBAC team.

### Key Points

- **Role Focus:** Designed to provide just enough permissions for deploying policies, running remediation tasks (including Azure Monitor alerts), and Resource Group creation.
- **Security Improvement:**
  - Reduces permissions from ~6,700 (Contributor role) to only 6 with the new role.
  - Aligns with best practices for least privilege and reduces attack surface.
- **Adoption:**
  - **New Deployments:** Immediately assigned by default.
  - **Existing Deployments:** Update process documented [here](https://azure.github.io/azure-monitor-baseline-alerts/patterns/alz/HowTo/Switch_To_LeastPrivileges/).

## 3. Getting Started & Deployment Options

To get started or update your deployment:

- Review the [Introduction to deploying the AMBA-ALZ Pattern](https://azure.github.io/azure-monitor-baseline-alerts/patterns/alz/HowTo/deploy/Introduction-to-deploying-the-ALZ-Pattern/)
- Choose from multiple deployment methods:
  - [Azure Portal UI](https://azure.github.io/azure-monitor-baseline-alerts/patterns/alz/HowTo/deploy/Deploy-via-Azure-Portal-UI/)
  - [Azure CLI](https://azure.github.io/azure-monitor-baseline-alerts/patterns/alz/HowTo/deploy/Deploy-with-Azure-CLI/)
  - [Azure PowerShell](https://azure.github.io/azure-monitor-baseline-alerts/patterns/alz/HowTo/deploy/Deploy-with-Azure-PowerShell/)
  - [Azure Pipelines](https://azure.github.io/azure-monitor-baseline-alerts/patterns/alz/HowTo/deploy/Deploy-with-Azure-Pipelines/)
  - [GitHub Actions](https://azure.github.io/azure-monitor-baseline-alerts/patterns/alz/HowTo/deploy/Deploy-with-GitHub-Actions/)
  - [Terraform](https://azure.github.io/azure-monitor-baseline-alerts/patterns/alz/HowTo/deploy/Deploy-with-Terraform/)

## Summary

These enhancements to the AMBA-ALZ pattern make Azure deployments more secure and operationally sound. The move to built-in Service Health policies and least-privileged managed identity roles simplifies compliance, boosts trust, and reduces administrative overhead.

For in-depth deployment steps and to explore AMBA-ALZ further, visit the [Azure Governance and Management Blog](https://techcommunity.microsoft.com/t5/azure-governance-and-management-blog/bg-p/AzureGovernanceandManagementBlog).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/amba-alz-pattern-learn-about-the-latest-and-greatest/ba-p/4458320)
