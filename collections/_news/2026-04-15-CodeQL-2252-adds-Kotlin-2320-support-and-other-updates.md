---
tags:
- Application Security
- C/C++
- C#
- CodeQL
- DevOps
- False Positives
- GHES
- GitHub Advanced Security
- GitHub Code Scanning
- GitHub Enterprise Server
- Go
- Improvement
- Java
- Kotlin 2.3.20
- Log Injection
- Manual Upgrade
- News
- Python
- Query Improvements
- Ruby
- Rust
- SAST
- Security
- Security Queries
- Security Severity Scores
- Static Analysis
- Swift
- XSS
title: CodeQL 2.25.2 adds Kotlin 2.3.20 support and other updates
feed_name: The GitHub Blog
primary_section: devops
external_url: https://github.blog/changelog/2026-04-15-codeql-2-25-2-adds-kotlin-2-3-20-support-and-other-updates
section_names:
- devops
- security
date: 2026-04-15 22:05:52 +00:00
author: Allison
---

Allison outlines what changed in CodeQL 2.25.2 for GitHub code scanning, including Kotlin 2.3.20 support, multiple query accuracy tweaks (notably for C#), and updated security-severity scores for issues like XSS and log injection across several languages.<!--excerpt_end-->

# CodeQL 2.25.2 adds Kotlin 2.3.20 support and other updates

CodeQL is the static analysis engine behind [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), which finds and remediates security issues in your code.

This release announcement covers [CodeQL 2.25.2](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.25.2/), including:

- Updated Kotlin analysis support
- Query accuracy improvements (reducing false positives)
- Security severity score adjustments across multiple languages

## Language and framework support

### Java/Kotlin

- Kotlin versions up to **2.3.20** are now supported for analysis.
- The `java/tainted-arithmetic` query no longer flags arithmetic expressions used directly as an operand of a comparison in `if`-condition bounds-checking patterns (reduced false positives).
- The `java/potentially-weak-cryptographic-algorithm` query no longer flags:
  - Elliptic Curve algorithms
  - HMAC-based algorithms
  - PBKDF2 key derivation

This change reduces false positives for that query.

### C/C++

Reduced false positives in these queries:

- `cpp/suspicious-add-sizeof`
- `cpp/wrong-type-format-argument`
- `cpp/integer-multiplication-cast-to-long`

## Query changes

### C#

- The `cs/constant-condition` query has been simplified to produce fewer false positives.
- The `cs/constant-comparison` query has been removed because `cs/constant-condition` now covers those results.

## Security severity updates

`@security-severity` scores were updated across several languages to better align **log injection** and **XSS** queries with their actual impact.

- **C/C++**
  - `cpp/cgi-xss` increased from medium (6.1) to high (7.8)
- **C#**
  - `cs/log-forging` reduced from high (7.8) to medium (6.1)
  - `cs/web/xss` increased from medium (6.1) to high (7.8)
- **Go**
  - `go/log-injection` reduced from high (7.8) to medium (6.1)
  - `go/html-template-escaping-bypass-xss`, `go/reflected-xss`, `go/stored-xss` increased from medium (6.1) to high (7.8)
- **Java/Kotlin**
  - `java/log-injection` reduced from high (7.8) to medium (6.1)
  - `java/android/webview-addjavascriptinterface`, `java/android/websettings-javascript-enabled`, `java/xss` increased from medium (6.1) to high (7.8)
- **Python**
  - `py/log-injection` reduced from high (7.8) to medium (6.1)
  - `py/jinja2/autoescape-false`, `py/reflective-xss` increased from medium (6.1) to high (7.8)
- **Ruby**
  - `rb/log-injection` reduced from high (7.8) to medium (6.1)
  - `rb/reflected-xss`, `rb/stored-xss`, `rb/html-constructed-from-input` increased from medium (6.1) to high (7.8)
- **Swift**
  - `swift/unsafe-webview-fetch` increased from medium (6.1) to high (7.8)
- **Rust**
  - `rust/log-injection` increased from low (2.6) to medium (6.1)
  - `rust/xss` increased from medium (6.1) to high (7.8)

## Deployment notes

- Every new version of CodeQL is automatically deployed to users of GitHub code scanning on **github.com**.
- CodeQL 2.25.2 functionality will also be included in a future **GitHub Enterprise Server (GHES)** release.
- If you run an older GHES version, you can manually upgrade CodeQL by following GitHub’s documentation: [Manually upgrade your CodeQL version](https://docs.github.com/enterprise-server@3.20/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access).

For full details, see the complete [CodeQL 2.25.2 changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.25.2/).

[Read the entire article](https://github.blog/changelog/2026-04-15-codeql-2-25-2-adds-kotlin-2-3-20-support-and-other-updates)

