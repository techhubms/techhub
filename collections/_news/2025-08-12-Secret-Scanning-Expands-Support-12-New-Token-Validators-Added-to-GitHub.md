---
layout: "post"
title: "Secret Scanning Expands Support: 12 New Token Validators Added to GitHub"
description: "This news update details GitHub's addition of validity check support for 12 new API token types from 11 providers—including Cockroach Labs, Polar, Yandex, and more. Developers using secret scanning will now benefit from enhanced automatic detection and verification for a wider range of credentials. The update covers expanded provider support, improvements to active credential alerting, and directs users to documentation for the full supported list."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-12-secret-scanning-adds-12-validators-including-cockroach-labs-polar-and-yandex"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-12 21:20:14 +00:00
permalink: "/news/2025-08-12-Secret-Scanning-Expands-Support-12-New-Token-Validators-Added-to-GitHub.html"
categories: ["DevOps", "Security"]
tags: ["API Tokens", "Apify", "Asaas", "Cockroach Labs", "Code Security", "Continuous Integration", "Credential Validation", "Developer Tools", "DevOps", "DevOps Security", "Fullstory", "GitHub", "Grafana", "News", "Polar", "RunPod", "Secret Scanning", "Security", "Sourcegraph", "Telnyx", "Token Validity Checks", "Val Town", "Yandex"]
tags_normalized: ["api tokens", "apify", "asaas", "cockroach labs", "code security", "continuous integration", "credential validation", "developer tools", "devops", "devops security", "fullstory", "github", "grafana", "news", "polar", "runpod", "secret scanning", "security", "sourcegraph", "telnyx", "token validity checks", "val town", "yandex"]
---

Allison reports on GitHub’s secret scanning improvements, highlighting expanded support for 12 new token types and enhanced credential validity checks to bolster repository security.<!--excerpt_end-->

# Secret Scanning Expands Support: 12 New Token Validators Added to GitHub

**Author:** Allison

GitHub has updated its secret scanning feature to add validity check support for 12 new API token types across 11 providers. This improvement enhances repository security and proactive credential management for developers and teams.

## Expanded Token Support

GitHub secret scanning can now verify leaked credentials for the following new token types:

- **Apify:** `apify_api_token`
- **Asaas:** `asaas_api_token`
- **Cockroach Labs:** `ccdb_api_key`
- **Fullstory:** `fullstory_api_key` (legacy and current versions)
- **Grafana:** `grafana_cloud_api_token`
- **Polar:** `polar_access_token` (legacy and current versions)
- **RunPod:** `runpod_api_key`
- **Sourcegraph:** `sourcegraph_instance_identifier_access_token`, `sourcegraph_access_token`
- **Telnyx:** `telnyx_api_v2_key`
- **Val Town:** `val_town_api_token`
- **Yandex:** `yandex_cloud_api_key`

## Validity Checks Explained

A validity check means GitHub automatically verifies whether the leaked credential is active and could still be exploited. With validity checks enabled for a repository, any alert created for these tokens will be validated, reducing the risk of accidental exposure going unaddressed.

## Getting Started

To benefit from these additional checks, ensure secret scanning is enabled and that validity checks are active on your repositories. For details and a full list of all supported secret types and patterns, see the [GitHub product documentation](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/supported-secret-scanning-patterns#default-patterns).

## Why This Matters

Expanding validity checking helps development teams:

- Get notified about active, exploitable secrets more precisely
- Reduce manual work validating whether detected secrets are real threats
- Improve the overall security posture of their CI/CD pipelines and repositories

Stay tuned for more updates as GitHub continues to strengthen automated code security tooling.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-12-secret-scanning-adds-12-validators-including-cockroach-labs-polar-and-yandex)
