---
external_url: https://github.blog/changelog/2025-12-02-secret-scanning-updates-november-2025
title: GitHub Secret Scanning Updates and New Patterns — November 2025
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-12-02 16:50:31 +00:00
tags:
- Application Security
- AWS
- Databricks
- Discord
- Elliptic Curve
- GitHub
- Improvement
- Key Management
- Metadata Validation
- Microsoft
- Partner Integrations
- PKCS#8
- PostHog
- Private Key Detection
- Push Protection
- Rainforest Pay
- Secret Scanning
- Sentry
section_names:
- azure
- devops
- security
---
Allison reports on the November 2025 improvements to GitHub secret scanning, including new Azure and Microsoft secret patterns, enhanced private key detection, and upgraded validity checks for security-critical credentials.<!--excerpt_end-->

# GitHub Secret Scanning Updates — November 2025

**Author: Allison**

GitHub secret scanning continues to evolve, rolling out numerous improvements in November 2025. These updates help developers and organizations strengthen application security by detecting a wider array of sensitive credentials and keys in source code and repositories.

## New Secret Patterns Added

24 new secret types were introduced, broadening GitHub’s scanning coverage for a range of providers:

- **Azure:**
  - `azure_immersive_reader_key`
  - `azure_logic_apps_url`
- **Databricks:** Multiple session tokens and OAuth-related secrets
- **Microsoft:**
  - `power_automate_webhook_sas`
- **Other providers:** Paddle, PostHog, Rainforest Pay, Discord, Raycast, and more

For a complete breakdown, each new pattern supports push protection (configurable), partner notification, and continuous scan coverage. These enhancements help identify risks before code is merged or deployed.

## Improved Private Key Detection

Announced on November 12, improvements specifically target Elliptic Curve and generic PKCS#8 private keys:

- **New detectable formats:**
  - `ec_private_key`: Elliptic Curve
  - `generic_private_key`: Generic PKCS#8
- **Escaped newline support:**
  - Patterns now identify keys with `\n` line endings—a common occurrence in configuration files.

Other affected key types include:

- `github_ssh_private_key`
- `openssh_private_key`
- `rsa_private_key`

## Extended Metadata Checks

- **Discord Bot Tokens (`discord_bot_token`):**
  - Now includes extended metadata such as owner info, creation date, and organizational details, supporting better context and tracking.

## Validity Checks and Pattern Upgrades

- **AWS Access Key IDs:**
  - Validation improvements ensure customers are promptly alerted to exposed and valid credentials, replacing vague alerts with “valid” or “invalid” status.

## Sentry Token Renaming

- **Updated token names:**
  - `sentry_organization_token` → `sentry_org_auth_token`
  - `sentry_personal_token` → `sentry_user_auth_token`

## Partner Notification for Unlisted Gists

- As of November 25, secrets discovered in unlisted GitHub gists are now reported to scanning partners, helping organizations quickly remediate accidental exposures. Unlisted gists pose the same risk as public code snippets if URLs are shared.

## Links and References

- [GitHub Secret Scanning Introduction](https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning)
- [Supported Secret Scanning Patterns](https://docs.github.com/code-security/secret-scanning/introduction/supported-secret-scanning-patterns)
- [Changelog: Private Key Detection](https://github.blog/changelog/2025-11-12-secret-scanning-improves-private-key-detection/)
- [Changelog: Gists Partner Reporting](https://github.blog/changelog/2025-11-25-secrets-in-unlisted-github-gists-are-now-reported-to-secret-scanning-partners/)

## Key Takeaways

- Substantial improvement in security for Azure and Microsoft-related development workflows
- Expanded pattern coverage for private keys, API tokens, and webhooks
- Enhanced checks and partner integrations help teams catch and remediate credential exposures earlier
- Developers are encouraged to regularly review GitHub’s [documentation](https://docs.github.com/code-security/secret-scanning/introduction/supported-secret-scanning-patterns) for new supported secret types

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-02-secret-scanning-updates-november-2025)
