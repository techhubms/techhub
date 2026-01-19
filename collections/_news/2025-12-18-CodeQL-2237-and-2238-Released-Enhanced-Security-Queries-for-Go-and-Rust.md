---
external_url: https://github.blog/changelog/2025-12-18-codeql-2-23-7-and-2-23-8-add-security-queries-for-go-and-rust
title: 'CodeQL 2.23.7 and 2.23.8 Released: Enhanced Security Queries for Go and Rust'
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-12-18 23:03:15 +00:00
tags:
- Application Security
- Buildless Analysis
- C#
- CI/CD
- Code Quality
- CodeQL
- GitHub Code Scanning
- Go
- Improvement
- Java
- JavaScript
- Kotlin
- Open Source
- Python
- Rust
- Security Queries
- Static Analysis
- TypeScript
section_names:
- devops
- security
---
Allison details the latest CodeQL 2.23.7 and 2.23.8 releases, adding new security queries for Go and Rust, and highlighting broader improvements across languages for users of GitHub Code Scanning and Code Quality.<!--excerpt_end-->

# CodeQL 2.23.7 and 2.23.8: Enhanced Security Queries for Go and Rust

**Author:** Allison

CodeQL, GitHub’s static analysis engine powering Code Scanning and Code Quality, has released versions 2.23.7 and 2.23.8. These updates bring new features, improved accuracy, and broader language support, further strengthening security and code quality for developers and DevOps professionals.

## Notable New Features and Improvements

### New Security Queries

**Go:**

- `go/cookie-secure-not-set`: Detects cookies missing the Secure flag, mitigating risk of sensitive information exposure.
- `go/weak-crypto-algorithm`: Flags uses of outdated or broken cryptographic algorithms.
- `go/weak-sensitive-data-hashing`: Detects weak hash algorithms for sensitive data.
- `go/cookie-http-only-not-set`: Detects cookies missing the HttpOnly flag, mitigating XSS risks (promoted from experimental, contributed by @edvraa).

**Rust:**

- `rust/xss`: Detects cross-site scripting vulnerabilities.
- `rust/disabled-certificate-check`: Identifies disabled TLS certificate checks.
- Example queries (`empty-if`, `simple-sql-injection`, `simple-constant-password`) help users author CodeQL queries for Rust.

### Language and Framework Updates

- **Java/Kotlin**: Java analysis now respects Maven compiler settings, enhancing build compatibility. Truncating string operations (e.g., substring or take limited to 7 characters or fewer) are now recognized as sanitizers in `java/sensitive-log` queries.
- **JavaScript/TypeScript**: Bug fixes improve detection in Next.js server-side taint sources within the `app/pages` directory.
- **Rust**: The `rust/access-invalid-pointer` query now covers more flow sources and barriers.
- **C#**: Buildless analysis now logs compilation errors and introduces a new option to specify the dependency directory (`-O buildless_dependency_dir=<path>`).
- **Python**: Import handling resilient to missing modules, thanks to an open-source contribution by @akoeplinger.

### Deployment and Resources

- CodeQL updates are automatically deployed for GitHub Code Scanning and Code Quality users on GitHub.com.
- Features and fixes are included in GitHub Enterprise Server (GHES) version 3.20, with manual upgrade available for older versions (see documentation for [manual upgrade instructions](https://docs.github.com/enterprise-server@3.18/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access)).
- Full changelogs for [2.23.7](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.7/) and [2.23.8](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.8/).

## Why This Matters

Static analysis and automated code scanning are critical in modern DevOps, CI/CD, and security workflows. With these upgrades, GitHub expands secure development capabilities for users working across major programming languages.

---

**References:**

- [About code scanning with CodeQL](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql)
- [About code quality](https://docs.github.com/code-security/code-quality/concepts/about-code-quality)
- [CodeQL changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/)
- [Open-source CodeQL repository](https://github.com/github/codeql)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-18-codeql-2-23-7-and-2-23-8-add-security-queries-for-go-and-rust)
