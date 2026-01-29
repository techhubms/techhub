---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-public-preview-of-managed-instance-on-azure-app/ba-p/4469930
title: 'Managed Instance on Azure App Service: Public Preview Announcement and Technical Overview'
author: apwestgarth
feed_name: Microsoft Tech Community
date: 2025-11-18 16:00:00 +00:00
tags:
- .NET Framework
- Azure App Service
- Azure Bastion
- Azure Storage
- Cloud Migration
- Compliance
- DevOps Automation
- Enterprise Applications
- Key Vault
- Legacy Modernization
- Managed Instance
- PowerShell
- Premium V4
- RDP
- VNET Integration
- Windows Registry
- Azure
- Coding
- DevOps
- Security
- Community
section_names:
- azure
- coding
- devops
- security
primary_section: coding
---
Written by apwestgarth, this announcement details the new Managed Instance on Azure App Service, describing technical features designed to help developers migrate and modernize legacy .NET applications for the cloud with reduced code changes and strong security.<!--excerpt_end-->

# Managed Instance on Azure App Service: Public Preview Announcement

**Author: apwestgarth**

## Introduction

Azure App Service now introduces Managed Instance in Public Preview, enabling developers and enterprises to migrate legacy applications to the cloud with minimal code changes. Managed Instance on App Service provides solutions to common migration pain points, supporting scenarios where configuration dependencies, registry access, file I/O, and infrastructure tooling are required.

## Migration and Modernization Challenges

Developers migrating legacy and complex .NET applications to Azure App Service often face:

- Application dependencies (e.g., GAC libraries, Windows Services)
- Configuration systems relying on Windows Registry
- Legacy file I/O requiring local disks or mapped network storage
- Reduced access to familiar infrastructure tools
- Lengthy and costly migration projects due to required code changes

## How Managed Instance Addresses These Challenges

Managed Instance introduces several new features:

- **Configuration Scripts**: Upload a zip file of dependencies with a PowerShell installer script, stored securely in Azure Storage and accessed using Managed Identity.
- **Registry Adapters**: Write/read values to an App Service Plan instance's Windows Registry, leveraging Key Vault for secure storage and secrets (using Managed Identity).
- **Storage Mounts**: Map Azure Files, SMB shares, and temporary local storage using standard drive letter/folder mappings, with credentials managed via Key Vault secrets and Managed Identity.
- **RDP Access via Azure Bastion**: Secure remote desktop into App Service Plan instances to use native Windows tools like IIS Manager, Event Viewer, and MMC Snap-ins without exposing direct infrastructure management.
- **Accelerated Migration**: Many application migrations require little to no code changes, reducing project risk and time to ROI.

Premium v4 plan options offer enhanced performance and cost efficiency ([Premium v4 General Availability](https://techcommunity.microsoft.com/blog/appsonazureblog/announcing-general-availability-of-premium-v4-for-azure-app-service/4446204)).

## Key Scenarios

### 1. Lift-and-Improve Legacy Apps

- Migrate .NET apps with hardcoded paths, COM, registry, and dependencies without major rewrites. Install custom components in the managed instance.

### 2. Re-platforming Hard-to-Modernize Apps

- Move apps tightly coupled to infrastructure or lacking modifiable source code (SMTP servers, MSMQ, middleware) using custom installers and advanced networking support.

### 3. Hybrid and Regulated Workloads

- Integrate securely with on-prem via VNETs/private endpoints. Enforce data residency and controls using BYO storage and Managed Identity. Meet compliance for industries such as finance and healthcare.

### 4. Incremental Modernization

- Start with lift-and-shift, incrementally adopt PaaS integrations (DevOps, scaling, centralized configuration) at your own pace for ongoing modernization.

## Getting Started

- [Managed Instance on Azure App Service Documentation](https://aka.ms/managedinstanceonappservicedocs)
- [Technical Deep Dive at Ignite 2025](https://aka.ms/Ignite25/BRK102)

Managed Instance opens new opportunities for securely modernizing legacy workloads, integrating natively with Azure services, DevOps, compliance, and security best practices.

## Author Information

- **Author:** apwestgarth, Microsoft
- **Original Announcement:** Ignite 2025

## Additional Resources

- Azure Marketplace: Deploy Managed Instance Web Apps
- Premium v4 feature overview

---

Managed Instance on Azure App Service is designed to accelerate legacy modernization in a secure, scalable Azure environment, giving developers native access to configuration, storage, and supporting DevOps automation.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-public-preview-of-managed-instance-on-azure-app/ba-p/4469930)
