---
layout: "post"
title: "GitHub Secret Scanning Expands with New Validators and Validity Checks"
description: "GitHub's secret scanning feature now supports validity checks for over 10 new secret types, including tokens from providers like Square, Wakatime, and Yandex. This update helps developers better identify which credentials are still exploitable, enhancing repository security and enabling teams to respond to leaked credentials more efficiently."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-26-secret-scanning-adds-10-new-validators-including-square-wakatime-and-yandex"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-26 17:49:15 +00:00
permalink: "/2025-08-26-GitHub-Secret-Scanning-Expands-with-New-Validators-and-Validity-Checks.html"
categories: ["DevOps", "Security"]
tags: ["Bitrise", "Credential Management", "DevOps", "GitHub", "Groq", "News", "Secret Scanning", "Security", "Siemens", "Square", "Token Validation", "Uniwise", "Validity Checks", "Wakatime", "WorkOS", "Yandex"]
tags_normalized: ["bitrise", "credential management", "devops", "github", "groq", "news", "secret scanning", "security", "siemens", "square", "token validation", "uniwise", "validity checks", "wakatime", "workos", "yandex"]
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
