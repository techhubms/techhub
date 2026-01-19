---
layout: post
title: Challenges with Custom Script Extensions Authentication in Azure Session Host Configuration
author: kristokruuser
canonical_url: https://techcommunity.microsoft.com/t5/azure-virtual-desktop/custom-script-extensions-and-session-host-configuration/m-p/4476435#M13956
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-12-09 13:51:03 +00:00
permalink: /azure/community/Challenges-with-Custom-Script-Extensions-Authentication-in-Azure-Session-Host-Configuration
tags:
- Authentication
- Azure Key Vault
- Azure Portal
- Azure Security
- Azure Virtual Desktop
- Credential Management
- Custom Script Extension
- Managed Identity
- SAS Token
- Session Host Configuration
- Storage Account
section_names:
- azure
- security
---
Kristo Kruuser raises concerns about securely authenticating Custom Script Extensions in Azure Session Host Configuration, discussing the drawbacks of current approaches and exploring potential solutions.<!--excerpt_end-->

# Challenges with Custom Script Extensions Authentication in Azure Session Host Configuration

**Author: Kristo Kruuser**

## Overview

The Custom Script Extensions (CSE) feature in Azure is commonly used to automate Session Host configuration in environments such as Azure Virtual Desktop. However, the current mechanism for specifying CSE scripts involves providing a script URL that is either publicly accessible or includes a token in plain text—a potential security risk.

## Current Authentication Limits

- **Anonymous Blob Access**: Right now, CSE scripts must be hosted in an Azure Blob Storage container set to anonymous access. This means anyone with the URL can retrieve the script, which poses a security risk.
- **Token in Script URL**: Another option is embedding a SAS token directly in the URL, but since this URL is viewable in the Azure Portal in plain text, this still exposes secrets.

## Observed Key Vault Usage

- Key Vault references are currently supported when providing credentials for domain join operations or setting up local admin accounts on Session Hosts. However, Key Vault is not integrated for the CSE script authentication by default.

## Proposed Secure Alternatives

- **Key Vault-based References**: The question is raised whether Azure Key Vault could be used to reference Storage Account Names/Keys or SAS tokens, just as is done with other secrets during host configuration.
- **Managed Identity Support**: A preferred solution might be the ability to leverage Managed Identities when accessing the storage containing scripts, removing the need to embed secrets in the script URL.

## Current CSE Authentication Methods When Deploying Manually

- Authentication using different methods is possible when deploying CSEs manually, but not when configuring via the Session Host Configuration feature.

## Open Questions & Guidance Sought

The author is seeking best practices and official guidance on how to securely authorize script access for CSE in automated Session Host deployments, ideally without exposing secrets or relying on publicly accessible blobs.

## Summary

- The current Azure Portal implementation for CSE script URLs has authentication shortcomings.
- There is a clear need for more secure solutions like Azure Key Vault references or Managed Identity support in Session Host Configuration automation.
- Feedback, best practices, or guidance from the community or Microsoft is requested.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/custom-script-extensions-and-session-host-configuration/m-p/4476435#M13956)
