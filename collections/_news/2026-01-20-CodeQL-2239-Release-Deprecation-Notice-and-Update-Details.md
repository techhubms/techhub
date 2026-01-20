---
layout: "post"
title: "CodeQL 2.23.9 Release: Deprecation Notice and Update Details"
description: "This news update covers the release of CodeQL 2.23.9, the static analysis engine used in GitHub code scanning. The release contains no user-facing changes, but it includes a deprecation notice for Kotlin versions 1.6 and 1.7. The announcement also highlights deployment details for users on GitHub.com and GitHub Enterprise Server, with guidance on upgrade paths."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-01-20-codeql-2-23-9-has-been-released"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-01-20 21:50:01 +00:00
permalink: "/2026-01-20-CodeQL-2239-Release-Deprecation-Notice-and-Update-Details.html"
categories: ["DevOps", "Security"]
tags: ["Application Security", "Code Scanning", "CodeQL", "Deprecation", "DevOps", "GitHub", "GitHub Enterprise Server", "Improvement", "Kotlin", "News", "Security", "Software Update", "Static Analysis", "Versioning"]
tags_normalized: ["application security", "code scanning", "codeql", "deprecation", "devops", "github", "github enterprise server", "improvement", "kotlin", "news", "security", "software update", "static analysis", "versioning"]
---

Allison reports on the release of CodeQL 2.23.9, highlighting essential information for GitHub code scanning users, including a deprecation notice for certain Kotlin versions.<!--excerpt_end-->

# CodeQL 2.23.9 Release: Deprecation Notice and Update Details

**Author: Allison**

CodeQL, the static analysis engine behind [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), helps discover and remediate security issues in codebases. The latest release, [CodeQL 2.23.9](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.9/), is now available. This release does not introduce any user-facing changes or new queries, but it is an important part of CodeQL's ongoing maintenance and advancement.

## Deprecation Notice

Support for Kotlin versions 1.6 and 1.7 is now deprecated and will be removed in CodeQL 2.24.1, which is planned for release in February 2026. Developers using earlier Kotlin versions should prepare to upgrade to at least Kotlin 1.8 to continue extracting Kotlin databases with upcoming releases.

## Deployment Details

- All new versions of CodeQL are automatically deployed to GitHub code scanning users on github.com.
- CodeQL 2.23.9 will be included in a future *GitHub Enterprise Server* (GHES) release.
- For those using older versions of GHES, manual upgrades are possible. Detailed instructions can be found in the [GitHub documentation](https://docs.github.com/enterprise-server@3.19/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access).

## Key Points

- No user-facing changes in CodeQL CLI or queries in this release.
- Deprecation affects Kotlin 1.6 and 1.7 users starting in CodeQL 2.24.1.
- Upgrading CodeQL ensures compatibility with future security checks and features.

For additional details or to view the changelog, refer to the [official GitHub blog post](https://github.blog/changelog/2026-01-20-codeql-2-23-9-has-been-released).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-20-codeql-2-23-9-has-been-released)
