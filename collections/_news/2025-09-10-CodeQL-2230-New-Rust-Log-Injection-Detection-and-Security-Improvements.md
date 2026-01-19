---
external_url: https://github.blog/changelog/2025-09-10-codeql-2-23-0-adds-support-for-rust-log-injection-and-other-security-detection-improvements
title: 'CodeQL 2.23.0: New Rust Log Injection Detection and Security Improvements'
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-09-10 18:04:45 +00:00
tags:
- Application Security
- C#
- Code Scanning
- CodeQL
- Dataflow Analysis
- GitHub
- GitHub Enterprise Server
- Improvement
- Java
- Log Injection
- Python
- Rust
- Security Scanning
- Spring Boot
- Static Analysis
- Taint Tracking
section_names:
- coding
- devops
- security
---
Allison covers the latest in CodeQL 2.23.0, highlighting new Rust log injection detection, security query improvements across languages, and deployment updates for GitHub code scanning users.<!--excerpt_end-->

# CodeQL 2.23.0: New Rust Log Injection Detection and Security Improvements

**Author:** Allison

CodeQL, the static analysis engine behind [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), helps find and remediate security issues in source code. The recent release of [CodeQL 2.23.0](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.0/) brings significant security and performance enhancements, broadening support for multiple languages and frameworks.

## Language and Framework Highlights

- **C/C++:**
  - Improved dataflow and taint-tracking via new library, yielding fewer false positives.
  - Added flow summaries for `Microsoft::WRL::ComPtr` member functions.
- **C#:**
  - Enhanced data flow analysis to more accurately track flows using the `base` qualifier.
  - The taint tracking default config now supports implicit reads from collections, improving overall coverage and reducing false negatives.
- **Rust:**
  - Extraction performance improvements by removing path resolution from the extractor.
  - Enhanced modeling for `std::fs`, `async_std::fs`, and `tokio::fs`, potentially surfacing more alerts for path injection vulnerabilities.
  - Introduction of a new query, `rust/log-injection`, for detecting potential log entry manipulation by malicious users.
- **Java:**
  - The `java/insecure-spring-actuator-config` query is promoted to the main pack as `java/spring-boot-exposed-actuators-config`, now enabled by default. It detects improper actuator exposure in Spring Boot configs.
  - Bug fixes for null dereference and query consolidation to avoid redundancy.
- **Python:**
  - `py/unexpected-raise-in-special-method` now detects more conditional exceptions.
  - `py/incomplete-ordering`, `py/inconsistent-equality`, and `py/equals-hash-mismatch` get improved documentation and focus on Python 3 issues only.

## Query Improvements

- Numerous enhancements and additions across supported languages.
- More precise function call resolution and expanded taint flow tracking.

## Availability and Deployment

- CodeQL updates are automatically deployed for users of GitHub code scanning at github.com.
- The new features will also be part of a future GitHub Enterprise Server (GHES) release; [manual upgrades](https://docs.github.com/enterprise-server@3.17/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access) are supported for users of older GHES versions.

For all changes, check the [official changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.0/).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-10-codeql-2-23-0-adds-support-for-rust-log-injection-and-other-security-detection-improvements)
