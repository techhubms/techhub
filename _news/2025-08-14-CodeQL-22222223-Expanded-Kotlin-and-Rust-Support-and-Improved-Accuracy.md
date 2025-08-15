---
layout: "post"
title: "CodeQL 2.22.2/2.22.3: Expanded Kotlin & Rust Support and Improved Accuracy"
description: "This news update covers the release of CodeQL versions 2.22.2 and 2.22.3, highlighting expanded support for Kotlin and Rust, improvements to React and JavaScript analysis, as well as important changes to query accuracy and framework modeling. CodeQL, the engine powering GitHub code scanning, continues to strengthen static analysis for security-focused developers across multiple languages."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-14-codeql-expands-kotlin-support-and-additional-accuracy-improvements"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-14 21:54:44 +00:00
permalink: "/2025-08-14-CodeQL-22222223-Expanded-Kotlin-and-Rust-Support-and-Improved-Accuracy.html"
categories: ["DevOps", "Security"]
tags: ["Code Security", "CodeQL", "CodeQL CLI", "Development Tools", "DevOps", "DevSecOps", "Framework Modeling", "GHES 3.19", "GitHub", "GitHub Code Scanning", "JavaScript", "Kotlin", "News", "Query Accuracy", "React", "Rust", "Security", "Security Scanning", "Static Analysis", "Taint Analysis", "Vulnerability Detection"]
tags_normalized: ["code security", "codeql", "codeql cli", "development tools", "devops", "devsecops", "framework modeling", "ghes 3 dot 19", "github", "github code scanning", "javascript", "kotlin", "news", "query accuracy", "react", "rust", "security", "security scanning", "static analysis", "taint analysis", "vulnerability detection"]
---

Allison reports on CodeQL 2.22.2 and 2.22.3 releases, focusing on expanded Kotlin and Rust support, enhanced static analysis, and improved security detection in GitHub code scanning workflows.<!--excerpt_end-->

# CodeQL 2.22.2/2.22.3: Expanded Kotlin & Rust Support and Improved Accuracy

**Author:** Allison

CodeQL, the static analysis engine at the core of [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), has received key updates in versions 2.22.2 and 2.22.3. These new releases focus on expanding language and framework support, refining query accuracy, and streamlining security scanning for developers.

## Language & Framework Support

- **Kotlin:** Now supports analysis of Kotlin 2.2.2x, broadening coverage for this growing language.
- **React:** CodeQL now traces taint through the React `use` function and identifies parameters in React server functions as taint sources, strengthening its detection of potential vulnerabilities.
- **Rust:** Rust support remains in [public preview](https://github.blog/changelog/2025-06-30-codeql-support-for-rust-now-in-public-preview/), with expanded capabilities to cover additional security issues and new language features.

## Query Changes

- **JavaScript:** Three older queries (`js/actions/pull-request-target`, `js/actions/actions-artifact-leak`, `js/actions/command-injection`) have been replaced by improved queries in the actions QL pack:
  - `actions/untrusted-checkout`
  - `actions/secrets-in-artifacts`
  - `actions/command-injection` (updated)

For more details, see the official changelogs for [CodeQL 2.22.2](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.22.2/) and [CodeQL 2.22.3](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.22.3/).

## Deployment & Availability

- New versions are automatically deployed to GitHub code scanning users on github.com.
- Features will be included in GitHub Enterprise Server (GHES) 3.19.
- If you're on an older GHES version, [manual upgrade instructions are available](https://docs.github.com/enterprise-server@3.15/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access).

## Key Takeaways

- Enhanced language and framework support improves vulnerability detection for modern codebases.
- Query improvements provide deeper, more accurate security insights.
- GitHub's ongoing commitment to secure development is reflected in frequent automatic CodeQL updates.

Developers are encouraged to review the update details for maximum benefit and to keep pipelines secure and current.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-14-codeql-expands-kotlin-support-and-additional-accuracy-improvements)
