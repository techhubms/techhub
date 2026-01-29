---
external_url: https://blog.fabric.microsoft.com/en-US/blog/gain-even-more-trust-and-compliance-with-onelake-diagnostics-immutability-generally-available/
title: Gain Even More Trust and Compliance with OneLake Diagnostics Immutability
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2026-01-12 12:00:00 +00:00
tags:
- Audit Trails
- Azure Blob Storage
- Compliance
- Data Governance
- Forensic Investigations
- Immutable Logs
- Lakehouse
- Log Retention
- Microsoft Fabric
- OneLake Diagnostics
- Operational Security
- Security Best Practices
- Tamper Proof Logs
- Workspace Administration
- Write Once Read Many
- Azure
- Security
- News
section_names:
- azure
- security
primary_section: azure
---
Microsoft Fabric Blog highlights new immutable diagnostic logs for OneLake diagnostics, enabling organizations to achieve stronger compliance and security. Authored by the Microsoft Fabric team, this post outlines technical implementation and key best practices.<!--excerpt_end-->

# Gain Even More Trust and Compliance with OneLake Diagnostics Immutability

**Author:** Microsoft Fabric Blog

## Overview

Microsoft Fabric introduces immutable diagnostic logs for OneLake diagnostics, providing organizations with tamper-proof, write-once-read-many (WORM) protected audit logs in their Fabric Lakehouse environments. This enhancement ensures end-to-end integrity of activity data for compliance, governance, and forensic purposes.

## Key Features

- **Immutable Diagnostic Logs**: Once enabled, diagnostic logs are locked and cannot be altered or deleted during a user-defined retention period.
- **Azure Blob Storage Integration**: Immutability is based on Azure Blob Storage’s immutable storage technology, used for financial, legal, and regulated data retention needs.
- **Governance Consistency**: All diagnostic logs in the workspace inherit the same policy, ensuring reliable compliance.

## Why Immutability Matters

Immutable logs support organizations by:

- Meeting regulatory and compliance mandates for tamper-evident records.
- Ensuring data governance by guaranteeing log integrity.
- Providing verified, unaltered records for forensic investigation and trust.

## How to Configure Immutable Diagnostics

1. **Enable OneLake Diagnostics**: Choose or create a Lakehouse in a workspace to collect diagnostic events.
2. **Set Up Immutability**: In the workspace settings, specify your mandatory retention period for immutable storage.
3. **Apply Retention Policy**: Once applied, logs cannot be edited or deleted before the period expires. This policy is irreversible for its duration.

## Best Practices for Deployment

- **Dedicated Workspaces**: Use a separate workspace solely for diagnostic logs to isolate permissions and governance.
- **Restrict Admin Roles**: Limit admin access to trusted personnel to prevent unauthorized changes.
- **Workspace/Lakehouse Protection**: While immutability prevents file tampering or deletion, it does not stop an authorized user from deleting the entire workspace or Lakehouse.
- **Retention Policy Alignment**: Set your immutability window to meet organizational audit, legal, and investigation needs.

## Storage and Cost Implications

Although immutability itself does not add charges, retained logs will increase your storage cost as they accumulate. Costs rise in proportion to data retention volume during the immutable window.

## Conclusion

With these enhancements, OneLake diagnostics in Microsoft Fabric delivers secure, compliant, fully auditable operational insights. Organizations operating in regulated sectors or needing tamper-proof monitoring benefit from industry-compliant platform integration.

For more details on best practices and configuration, see the [official documentation](https://learn.microsoft.com/en-us/fabric/onelake/onelake-diagnostics-overview#best-practice-recommendations) and the [Azure Blob Storage immutable storage overview](https://learn.microsoft.com/azure/storage/blobs/immutable-storage-overview).

> If your team needs tamper-proof records or operates under strict regulatory requirements, OneLake diagnostics with immutability provides added operational confidence and compliance.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/gain-even-more-trust-and-compliance-with-onelake-diagnostics-immutability-generally-available/)
