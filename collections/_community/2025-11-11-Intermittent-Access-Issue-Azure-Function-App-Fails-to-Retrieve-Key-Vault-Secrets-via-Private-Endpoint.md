---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure/intermittent-access-issue-between-azure-function-app-and-key/m-p/4468948#M316
title: 'Intermittent Access Issue: Azure Function App Fails to Retrieve Key Vault Secrets via Private Endpoint'
author: Manan_Choksi
feed_name: Microsoft Tech Community
date: 2025-11-11 14:08:54 +00:00
tags:
- AccessToKeyVaultDenied
- Authentication Errors
- Azure Function App
- Azure Key Vault
- Connectivity Issues
- Key Vault References
- Managed Identity
- Network Security
- Premium Plan
- Private Endpoint
- Secret Management
- Troubleshooting
- VNet Integration
- Azure
- Security
- Community
section_names:
- azure
- security
primary_section: azure
---
Manan_Choksi raises a technical question regarding intermittent failures for an Azure Function App to access Azure Key Vault secrets over a private endpoint, seeking community solutions for these managed identity authentication and connectivity issues.<!--excerpt_end-->

# Intermittent Access Issue Between Azure Function App and Key Vault (Private Endpoint Enabled)

**Author:** Manan_Choksi

## Scenario Overview

- **Function App:** Running on Azure Premium Plan, VNet-integrated
- **Key Vault:** Public Network Access disabled, private endpoint configured
- **Secret Retrieval:** Via Key Vault secret references in application settings using Managed Identity

## Issue Encountered

Occasionally, the Function App fails to retrieve secrets from Key Vault, and the following error is observed in the *Diagnose and Solve Problems* blade:

```
[ResolveWorkitem] AccessToKeyVaultDenied error while retrieving Key Vault Secret Reference. Exception: KeyVaultResolver.Common.ReferenceResolverException
```

This appears to be an intermittent connectivity or authentication failure between the Function App and Key Vault via the private endpoint.

## Community Inquiry

- Has anyone in the community encountered similar intermittent failures?
- Are there proven solutions or reliable mitigation strategies for resolving connectivity/authentication errors in this scenario?

## Setup Details

- The Function App uses a system-assigned managed identity.
- VNet integration is in place to route all outbound traffic through the virtual network.
- Azure Key Vault is locked down to only private network access.
- All configuration follows Microsoft’s recommended approach for secure secret management.

## Request

Looking for:

- Diagnostic steps for root cause identification (DNS, firewall, network security groups, service endpoint routing, managed identity propagation delays, etc.)
- Architectural or configuration changes to ensure reliable secret retrieval
- If applicable, references or links to Microsoft documentation or community best practices

*Share your experiences or solutions if you’ve addressed similar issues in production Azure workloads.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure/intermittent-access-issue-between-azure-function-app-and-key/m-p/4468948#M316)
