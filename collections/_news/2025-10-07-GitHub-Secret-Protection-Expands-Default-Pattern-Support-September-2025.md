---
layout: post
title: GitHub Secret Protection Expands Default Pattern Support – September 2025
author: Allison
canonical_url: https://github.blog/changelog/2025-10-07-secret-protection-expands-default-pattern-support-september-2025
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-10-07 16:13:11 +00:00
permalink: /azure/news/GitHub-Secret-Protection-Expands-Default-Pattern-Support-September-2025
tags:
- API Keys
- Application Security
- Azure
- Azure Communication Services
- Azure IoT Hub
- Azure Management Certificate
- Azure Quantum Key
- CI/CD
- Code Security
- Continuous Integration
- DevOps
- DevSecOps
- GitHub
- Improvement
- News
- OAuth
- Push Protection
- Repository Security
- Secret Scanning
- Security
- Token Detection
section_names:
- azure
- devops
- security
---
Allison outlines recent updates to GitHub's secret scanning, highlighting enhanced pattern support and improved secret detection, with a special focus on Azure-related tokens and application security.<!--excerpt_end-->

# GitHub Secret Protection Expands Default Pattern Support – September 2025

**Author:** Allison

GitHub continues to strengthen its security offerings for developers with updated default pattern sets for secret scanning. These enhancements help ensure that repositories are thoroughly checked for a broader spectrum of sensitive credentials, reducing the risk of accidental exposure.

## What's New

- **New Secret Patterns**: GitHub Secret Scanning now includes additional detection patterns for a wide variety of API keys and tokens across multiple providers such as Aikido, Airtable, Azure, Cohere, Google, Salesforce, and more. For the full list, refer to [GitHub supported secrets documentation](https://docs.github.com/code-security/secret-scanning/introduction/supported-secret-scanning-patterns#supported-secrets).
- **Expanded Push Protection**: Existing patterns have also been integrated into push protection, preventing accidental commits of secrets in real time. Notable additions include several Azure-related connection strings and certificates.

## Highlighted Azure Patterns

Recent updates strengthen detection of the following Azure-relevant secrets:

- **azure_quantum_key**
- **azure_communication_services_connection_string**
- **azure_iot_device_connection_string**
- **azure_iot_hub_connection_string**
- **azure_iot_provisioning_connection_string**
- **azure_management_certificate**
- **azure_sas_token**
- **azure_signalr_connection_string**

These patterns collectively help secure applications leveraging various Azure services.

## How It Works

- **Secret scanning** automatically detects newly supported secret patterns when code is pushed to repositories, alerting developers of potential credential leaks.
- **Push protection** stops commits containing identified secrets from making it into the codebase unless the author explicitly acknowledges the risk.

For detailed descriptions of all provider patterns and more examples, see the [official changelog post](https://github.blog/changelog/2025-10-07-secret-protection-expands-default-pattern-support-september-2025).

## Getting Started

1. Review your repository's secret scanning settings to ensure they're enabled.
2. Stay informed about which secret types are supported for proactive protection.
3. Regularly audit exposed and committed secrets using both automated scanning and manual reviews.

For more information, visit the [About Secret Scanning](https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning) documentation.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-07-secret-protection-expands-default-pattern-support-september-2025)
