---
external_url: https://github.blog/changelog/2025-08-14-codeql-expands-kotlin-support-and-additional-accuracy-improvements
title: CodeQL Expands Support for Kotlin and Improves Static Analysis Accuracy
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-08-14 21:54:44 +00:00
tags:
- Application Security
- CI/CD
- Code Scanning
- CodeQL
- DevSecOps
- Framework Modeling
- GitHub
- GitHub Enterprise Server
- JavaScript
- Kotlin
- Query Pack
- React
- Rust
- Security Scanning
- Static Analysis
- Taint Analysis
section_names:
- devops
- security
---
Allison reports on recent updates to CodeQL, featuring expanded Kotlin support, enhanced Rust analysis, and improved query accuracy for JavaScript and React, ensuring better static analysis and security scanning for GitHub projects.<!--excerpt_end-->

# CodeQL Expands Support for Kotlin and Improves Static Analysis Accuracy

CodeQL, the static analysis engine powering [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), has released versions 2.22.2 and 2.22.3, bringing notable enhancements for developers and security teams who use GitHub.

## Language & Framework Support

- **Kotlin**: CodeQL now supports analyzing Kotlin version 2.2.2x, broadening the engine's reach for developers using modern JVM languages.
- **React**: The analysis now tracks taint through the React `use` function and recognizes parameters of React server functions as taint sources, helping detect potential vulnerabilities in server-driven React codebases.
- **Rust**: Rust support remains in [public preview](https://github.blog/changelog/2025-06-30-codeql-support-for-rust-now-in-public-preview/), with expanded detection coverage for more security issues and language features.

## Query Changes

- **JavaScript**: Three previous queries for JavaScript have been retired, replaced by improved queries in the actions QL pack:
  - `js/actions/pull-request-target` superseded by `actions/untrusted-checkout`
  - `js/actions/actions-artifact-leak` superseded by `actions/secrets-in-artifacts`
  - `js/actions/command-injection` superseded by an updated `actions/command-injection`

Consult the [CodeQL 2.22.2](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.22.2/) and [CodeQL 2.22.3](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.22.3/) changelogs for more technical details.

## Availability

- CodeQL updates are automatically deployed to all users of GitHub code scanning.
- New features will be available in GitHub Enterprise Server (GHES) 3.19. Users of older GHES versions can [manually upgrade their CodeQL installation](https://docs.github.com/enterprise-server@3.15/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access).

## Summary

These improvements in language and framework support, combined with more precise queries and expanded coverage, help teams identify and remediate security issues more effectively in CI/CD pipelines and production codebases.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-14-codeql-expands-kotlin-support-and-additional-accuracy-improvements)
