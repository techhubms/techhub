---
external_url: https://github.blog/changelog/2025-11-19-codeql-2-23-5-adds-support-for-swift-6-2-new-java-queries-and-improved-analysis-accuracy
title: CodeQL 2.23.5 Adds New Language Support and Security Query Improvements
author: Allison
feed_name: The GitHub Blog
date: 2025-11-19 19:43:03 +00:00
tags:
- .NET
- Actix Web
- Application Security
- C# Compiler
- CodeQL
- CodeQL 2.23.5
- Concurrency Issues
- GHES
- GitHub Code Scanning
- Improvement
- Java Security
- Mysql Async
- Python
- Query Pack
- Rust
- Security Queries
- Sensitive Data
- Static Analysis
- Swift 6.2
- Thread Safety
- Vulnerability Detection
section_names:
- devops
- security
primary_section: devops
---
Allison discusses the features and improvements in CodeQL 2.23.5, focusing on enhanced language support and more accurate security analysis for GitHub code scanning.<!--excerpt_end-->

# CodeQL 2.23.5 Adds New Language Support and Security Query Improvements

CodeQL—GitHub's static analysis engine—continues to advance security scanning capabilities with the release of version 2.23.5. This upgrade delivers expanded language support and accuracy enhancements designed to help developers and security teams identify and remediate vulnerabilities more effectively.

## Key Enhancements in CodeQL 2.23.5

### Language and Framework Updates

- **Swift:** Full support for Swift 6.2 is now available, allowing analysis of projects built with this version.
- **Rust:** Integration for the `actix-web` framework, plus expanded coverage of `mysql` and `mysql_async` libraries.
- **C#:** Basic tracing and extraction on macOS/Linux when using the .NET CLI (`dotnet`), including support for .NET 10 RC2 with direct `csc` invocations.

### Security Query Improvements

#### C#

- Extensive rewrite of the `cs/dereferenced-value-may-be-null` query to drastically lower false positives and shift from `path-problem` to `problem` format.
- Broadened criteria in the `cs/constant-condition` query, yielding more high-confidence results.
- Improved detection for `cs/web/missing-x-frame-options`, recognizing nested configuration scenarios.

#### Java/Kotlin

- The `java/sensitive-cookie-not-httponly` query is now a primary query, moving out of experimental status.
- Three notable queries added for concurrency issues: `java/escaping`, `java/not-threadsafe`, and `java/safe-publication`, all focusing on analyzing classes marked `@ThreadSafe`.
- Calls to `String.matches` now serve as sanitizers within the `java/ssrf` query to further reduce false positives.

#### Python

- The `py/insecure-cookie` query was split into specialized queries: `py/insecure-cookie` (Secure flag), `py/client-exposed-cookie` (HttpOnly flag), and `py/samesite-none` (SameSite attribute), now triggering alerts only when sensitive data is detected.

## Availability

CodeQL 2.23.5 is auto-deployed for GitHub code scanning users on github.com. The functionality is also included in GitHub Enterprise Server (GHES) 3.20. Users of earlier GHES versions can manually upgrade their CodeQL instance if required.

- [Full CodeQL 2.23.5 Changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.5/)
- [Upgrade Instructions for GHES](https://docs.github.com/enterprise-server@3.18/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access)

## Summary

With its ongoing improvements in static code analysis, CodeQL helps teams proactively identify and address vulnerabilities. This release's expanded coverage across several popular languages and frameworks allows both open source and enterprise development teams to benefit from more accurate security insights and reduced false positives.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-19-codeql-2-23-5-adds-support-for-swift-6-2-new-java-queries-and-improved-analysis-accuracy)
