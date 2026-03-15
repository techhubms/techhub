---
external_url: https://github.blog/changelog/2026-03-10-secret-scanning-pattern-updates-march-2026
title: GitHub Secret Scanning Pattern Updates — March 2026
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-03-10 21:03:15 +00:00
tags:
- API Keys
- Application Security
- Azure Active Directory
- Compliance
- Continuous Integration
- Credential Detection
- DevOps
- GitHub
- Improvement
- News
- Push Protection
- Repository Security
- Secret Scanning
- Security
- Token Validation
- Validators
section_names:
- devops
- security
---
Allison summarizes the March 2026 updates to GitHub's secret scanning, focusing on new detectors—including Azure and major third-party tokens—expanded push protection, and added validation for various providers.<!--excerpt_end-->

# GitHub Secret Scanning Pattern Updates — March 2026

GitHub continues to enhance its secret scanning capabilities to protect developer workflows and repository security. Here’s a summary of the most impactful updates for March 2026:

## New Secret Detectors

- **28 new secret detectors** have been added, spanning 15 providers such as Azure, Lark, Vercel, Snowflake, and Supabase.
- Notable additions include `azure_active_directory_application_id` and `azure_active_directory_application_secret` for Microsoft Azure, increasing coverage for cloud authentication artifacts.
- Developers leveraging APIs from services like Airtable, Databricks, Heroku, PostHog, and Shopify now have their tokens automatically covered either as partner or user secrets.

## Expanded Push Protection

- **Push protection** is now enabled by default for 39 detectors, covering common tokens and credentials to prevent accidental leaks during `git push` in all repositories with secret scanning enabled, including public repositories.
- Supported tokens include those for Airtable, AWS, Databricks, Shopify, Pinecone, and many more. This increases the security baseline for open source and enterprise projects alike.

## Validators and Validators Expansion

- **Validity checks** have been introduced for Airtable, DeepSeek, npm, Pinecone, and Sentry tokens, allowing teams to verify whether detected secrets are active and need remediation.

## Key Details

- Detectors are classified as `Partner` (reported to the issuer) or `User` (alerts generated for repo owners).
- Many detectors are configurable to allow project-specific or organization-specific push protection.
- For more about supported secrets and technical details, refer to [GitHub's documentation](https://docs.github.com/code-security/secret-scanning/introduction/supported-secret-scanning-patterns).

## Microsoft Azure Coverage

- The addition of Azure-specific secret patterns (`azure_active_directory_application_id`, `azure_active_directory_application_secret`) strengthens protection for Microsoft identity solutions.
- Teams building with Azure or integrating with Microsoft Entra ID benefit from automatic secret detection, mitigating leakage risk as part of routine code pushes.

## Resources

- [Secret Scanning Documentation](https://docs.github.com/code-security/secret-scanning/introduction/about-secret-scanning)
- [Secret Scanning Partnership Program](https://docs.github.com/code-security/secret-scanning/secret-scanning-partnership-program/secret-scanning-partner-program)
- [Push Protection Details](https://docs.github.com/enterprise-cloud@latest/code-security/concepts/secret-security/about-push-protection)

## Conclusion

These enhancements underscore GitHub’s commitment to securing the software supply chain. With new detectors, expanded push protection, and validity checks, teams can proactively defend against secret leakage and credential compromise.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-10-secret-scanning-pattern-updates-march-2026)
