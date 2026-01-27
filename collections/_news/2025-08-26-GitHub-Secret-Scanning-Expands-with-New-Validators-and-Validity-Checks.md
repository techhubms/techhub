---
external_url: https://github.blog/changelog/2025-08-26-secret-scanning-adds-10-new-validators-including-square-wakatime-and-yandex
title: GitHub Secret Scanning Expands with New Validators and Validity Checks
author: Allison
feed_name: The GitHub Blog
date: 2025-08-26 17:49:15 +00:00
tags:
- Bitrise
- Credential Management
- GitHub
- Groq
- Secret Scanning
- Siemens
- Square
- Token Validation
- Uniwise
- Validity Checks
- Wakatime
- WorkOS
- Yandex
section_names:
- devops
- security
primary_section: devops
---
Allison outlines GitHub's addition of over 10 new validators for secret scanning, including providers like Square and Yandex, to improve credential security for developers.<!--excerpt_end-->

# GitHub Secret Scanning Expands with New Validators and Validity Checks

GitHub has announced an expansion of its secret scanning capability, adding support for more than 10 new secret types with validity checking across several providers. This enhancement is aimed at helping development teams and DevOps professionals detect whether any committed credentials remain active, bolstering proactive security measures in repositories.

## Newly Supported Providers

The following token types now have validity check support:

- **Bitrise:** `bitrise_workspace_api_token`
- **Groq:** `groq_api_key`
- **Siemens:** `siemens_api_token`
- **Square:** `square_access_token` *(includes Square Access Token, Legacy Production Access Token, and Legacy Sandbox Access Token)*
- **Uniwise:** `wiseflow_api_key`
- **Wakatime:** `wakatime_api_key`, `wakatime_oauth_access_token`
- **WorkOS:** `workos_staging_api_key`, `workos_production_api_key`
- **Yandex:** `yandex_cloud_iam_token`

See full documentation at [GitHub's Supported Secret Scanning Patterns](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/supported-secret-scanning-patterns#default-patterns).

## About Validity Checks

Validity checks allow GitHub to automatically verify whether a leaked credential is still active (exploitable) or has already been revoked. When validity checks are enabled for a repository, GitHub will now verify alerts for the above token types by default.

### Developer and Team Impact

- **Improved Security:** Immediate feedback on whether found credentials are a current risk
- **Faster Response:** Quicker detection enables prompt revocation or rotation
- **Wider Coverage:** Includes a broader range of cloud and dev tool providers

For more information and a full list of supported secret types, consult the [GitHub product documentation](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/supported-secret-scanning-patterns#default-patterns).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-26-secret-scanning-adds-10-new-validators-including-square-wakatime-and-yandex)
