---
external_url: https://blog.fabric.microsoft.com/en-US/blog/protecting-your-fabric-data-using-purview-is-now-generally-available/
title: Protecting Microsoft Fabric Data with Purview DLP and Sensitivity Labels
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-24 13:00:00 +00:00
tags:
- Access Controls
- Auditing
- Azure Data
- Compliance
- Data Classification
- Data Governance
- Data Loss Prevention
- DLP
- Domain Level Security
- Encryption
- Information Protection
- Microsoft Fabric
- Microsoft Purview
- Security Policies
- Sensitivity Labels
section_names:
- azure
- security
---
Microsoft Fabric Blog explains how to secure your data in Microsoft Fabric using Microsoft Purview's Data Loss Prevention policies and sensitivity labels, outlining benefits, setup steps, and governance improvements for organizations.<!--excerpt_end-->

# Protecting Microsoft Fabric Data with Purview DLP and Sensitivity Labels

## Overview

Microsoft has announced the general availability of advanced Data Loss Prevention (DLP) and Information Protection policies for Microsoft Fabric, offering organizations powerful tools to govern and secure their data within this unified analytics platform.

## Microsoft Purview Data Loss Prevention for Fabric

DLP policies in Microsoft Fabric empower administrators to:

- Create and manage DLP rules within the Microsoft Purview compliance portal.
- Detect sensitive information types (SITs) in Fabric assets, such as credit card and identification numbers.
- Apply protection policies at the workspace level for targeted data control.
- Configure alerts and notifications for policy violations, enabling prompt incident response.
- Audit all policy results for compliance and investigation purposes.

### Getting Started

1. Identify sensitive data locations within Fabric.
2. Access the Microsoft Purview compliance portal.
3. Navigate to Data loss prevention > Policies and create a new policy.
4. Select Fabric as the location and configure which workspaces the policy will cover.
5. Define rules using SITs and set desired actions (e.g., audit, notifications).

Comprehensive DLP helps organizations keep sensitive information secure and maintain regulatory compliance within their analytics tenant. For in-depth guidance, refer to [official documentation](https://learn.microsoft.com/en-us/purview/dlp-powerbi-get-started).

*Note: DLP Policies are available via Purview's new pay-as-you-go billing model.*

## Information Protection and Sensitivity Labels in Fabric

With Purview Information Protection, you can:

- Apply classification and protection labels (e.g., Confidential) to Fabric items (Lakehouses, Warehouses, KQL databases, etc.).
- Automate access restrictions and encryption based on sensitivity labels.
- Simplify compliance, ensuring a clear, auditable data security trail.
- Centralize data security by extending organization-wide Purview policies into Fabric.

### Domain-level Default Sensitivity Labels

Domain-level sensitivity labels enable:

- Automatic, consistent security policy application for all new data items in domains.
- Scalable governance and reduced manual errors across large analytics environments.
- Streamlined compliance reporting and demonstration of adherence to regulations.

Administrators can delegate governance to domain owners without losing overall security or compliance control. Learn more from the [domain-level sensitivity labels documentation](https://learn.microsoft.com/fabric/governance/domain-default-sensitivity-label).

## Benefits for Organizations

- Proactive data security management and reduced risk of data loss or exposure.
- Automated enforcement of security and compliance policies across Fabric assets.
- Confidence in secure sharing and collaboration for data-driven teams.

## Feedback and Additional Resources

Provide your feedback via the [Fabric feedback form](https://forms.office.com/r/3BNBajB5ek).

Organizations are encouraged to integrate Purview DLP and sensitivity labeling into their data governance strategy for a more secure and compliant analytics environment in Microsoft Fabric.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/protecting-your-fabric-data-using-purview-is-now-generally-available/)
