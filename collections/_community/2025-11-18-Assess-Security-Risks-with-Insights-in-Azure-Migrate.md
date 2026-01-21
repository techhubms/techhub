---
external_url: https://techcommunity.microsoft.com/t5/azure-migration-and/assess-security-risks-with-insights-in-azure-migrate/ba-p/4470866
title: Assess Security Risks with Insights in Azure Migrate
author: Nikita_Bajaj
feed_name: Microsoft Tech Community
date: 2025-11-18 08:22:12 +00:00
tags:
- Azure Migrate
- Azure Update Manager
- Cloud Migration
- Compliance
- Datacenter Security
- Defender For Cloud
- End Of Support Software
- Microsoft Azure
- On Premises Migration
- Patch Management
- Risk Mitigation
- Security Dashboard
- Security Insights
- Server Discovery
- Vulnerability Assessment
section_names:
- azure
- security
---
Nikita_Bajaj explains how Azure Migrate's Insights feature empowers organizations to identify and manage security risks as part of cloud migration, highlighting practical steps for a secure transition.<!--excerpt_end-->

# Assess Security Risks with Insights in Azure Migrate

## Why Security Matters in Migration

Migrating workloads to Azure is not just about speed and cost efficiency—it requires a secure foundation. Security considerations can be overlooked as migration tasks and deadlines take center stage. By prioritizing security in your migration strategy, you can prevent costly surprises, ensure compliance, and strengthen your organization's risk posture.

## Introducing Insights (Preview) in Azure Migrate

Azure Migrate's **Security Insights** offers a dashboard that helps you identify security risks in your datacenter before and during migration. With these insights, you can:

- Locate Windows and Linux servers running end-of-support operating systems or software, and identify machines with pending updates for upgrade planning.
- Detect vulnerabilities in installed software via integration with the [National Vulnerability Database (NVD)](https://nvd.nist.gov/).
- Find unprotected servers that lack essential security or patch management software (such as antivirus, SIEM, IAM, etc.).
- Review multiple security solutions in use and plan consolidation with services like [Microsoft Defender for Cloud](https://learn.microsoft.com/en-us/azure/defender-for-cloud/) and [Azure Update Manager](https://learn.microsoft.com/en-us/azure/update-manager/overview?view=migrate).

## How Insights Are Derived

Azure Migrate uses software inventory collected via the [Azure Migrate appliance discovery process](https://learn.microsoft.com/en-us/azure/migrate/how-to-review-discovered-inventory?view=migrate#deploy-and-configure-the-azure-migrate-appliance). By providing guest credentials, information about installed software, OS configuration, and updates is gathered for analysis. Key characteristics:

- **No additional agents or deep scans:** Security insights are derived from data already collected during quick discovery.
- **Cross-referenced with vulnerability and support lifecycle databases:** Identifies real risk factors in your environment.

### Analyses Performed

- **End-of-Support Software:** Matches discovered software versions against [endoflife.date](https://endoflife.date/) to flag unsupported components.
- **Vulnerability Checks:** Maps installed software/OS against the [NVD](https://nvd.nist.gov/) and evaluates risks based on CVSS scores.
- **Pending Updates:** Detects incomplete patching via [Windows Update](https://learn.microsoft.com/en-us/windows/deployment/update/windows-update-overview) metadata and Linux package managers.
- **Missing Security Software:** Identifies servers missing critical security tools like antivirus or patch management software. Highlights gaps and suggests using Microsoft security services for remediation.

> **Note:** Security Insights is intended to guide and highlight potential risks, not to replace specialized security tools. For comprehensive protection, use [Microsoft Defender for Cloud](https://learn.microsoft.com/en-us/azure/defender-for-cloud/) and [Azure Update Manager](https://learn.microsoft.com/en-us/azure/update-manager/overview?view=migrate).

## Getting Started

- Use [appliance-based discovery in Azure Migrate](https://learn.microsoft.com/en-us/azure/migrate/how-to-review-discovered-inventory?view=migrate) with guest discovery enabled.
- Insights are available about 24 hours post-discovery.
- In the [Azure Migrate portal](https://portal.azure.com/), start a new project or select an existing one. Navigate to **Explore inventory > Insights (preview)** to review security summaries across servers and software.
- See the [documentation](https://learn.microsoft.com/en-us/azure/migrate/insights-overview?view=migrate#how-are-insights-derived) and [demo video](https://www.youtube.com/watch?v=YsU-LWu7CL8) for detailed guidance.

## Summary

By leveraging Security Insights during your migration planning, you lay a stronger, more secure groundwork for your Azure journey. Early risk identification allows for proactive upgrades, vulnerability mitigation, and consolidation onto robust Microsoft security services.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-migration-and/assess-security-risks-with-insights-in-azure-migrate/ba-p/4470866)
