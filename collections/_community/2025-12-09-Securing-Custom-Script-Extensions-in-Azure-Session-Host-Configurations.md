---
external_url: https://techcommunity.microsoft.com/t5/azure-virtual-desktop-feedback/more-security-around-using-custom-script-extensions-and-session/idi-p/4476426
title: Securing Custom Script Extensions in Azure Session Host Configurations
author: kristokruuser
feed_name: Microsoft Tech Community
date: 2025-12-09 13:01:13 +00:00
tags:
- Automation
- Azure Key Vault
- Azure Portal
- Azure Virtual Desktop
- Blob Storage Authentication
- Credential Management
- Custom Script Extension
- Domain Join
- Golden Image
- Local Administrator
- Managed Identity
- SAS Token
- Secure Deployment
- Security Best Practices
- Session Host Configuration
section_names:
- azure
- security
primary_section: azure
---
kristokruuser details practical security challenges in managing authentication for Custom Script Extensions via Azure Session Host Configuration, raising concerns over anonymous blob access and plain-text tokens, and explores secure alternatives.<!--excerpt_end-->

# Securing Custom Script Extensions in Azure Session Host Configurations

Author: kristokruuser

## Overview

This community post explores the implementation and testing of new features in Azure Session Host Configuration and Management, focusing on securely deploying Custom Script Extensions (CSEs) to newly provisioned session hosts.

- **Immediate Configuration Needs:** CSEs are used to configure session hosts right after deployment, without updating the golden images or waiting for Group Policies.
- **Current Limitation:** The Azure Portal currently only allows a script URL to be defined in the Session Host Configuration, typically pointing to a blob in Azure Storage.

## Authentication Concerns

- **Anonymous Blob Access:** The existing method seems to require either an anonymous access-level blob or embedding tokens directly within the script URL. Both approaches pose security risks, primarily because URLs are visible in plain text via the Azure Portal.
- **Key Vault Usage:** While Azure Key Vault references are employed for domain join and local admin account credentials, similar mechanisms aren't available for CSE script authentication (e.g., for Storage Account keys or SAS tokens).

## Potential Secure Alternatives

- **Azure Key Vault Integration:** Extending Key Vault references to handle Storage Account credentials, SAS tokens, or other secrets needed for CSE authentication would improve security and centralize credential management.
- **Managed Identity:** Allowing Session Hosts to retrieve scripts from blob storage using managed identities removes the need for plain-text secrets and leverages Azure's built-in identity management.
- **Manual Deployment Support:** These secure authentication mechanisms can be configured manually when deploying CSEs, but the portal lacks options to automate them as part of the Session Host Configuration workflow.

## Recommendations Sought

The author requests guidance on:

- Secure best practices for handling CSE authentication during automated session host deployment
- Possible extensions to existing Azure features to support Key Vault or Managed Identity-based authentication for scripts

## Summary

Implementing secure authentication for CSEs during Session Host Configuration is crucial for protecting sensitive credentials and scripts. Azure practitioners are invited to share official guidance, practical experiences, and solutions that avoid exposing secrets in plain-text URLs or relying on anonymous storage access.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-feedback/more-security-around-using-custom-script-extensions-and-session/idi-p/4476426)
