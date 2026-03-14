---
external_url: https://github.blog/changelog/2026-03-10-codeql-2-24-3-adds-java-26-support-and-other-improvements
title: 'CodeQL 2.24.3 Release: Java 26 Support and Enhanced Static Analysis'
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-03-10 19:51:23 +00:00
tags:
- Application Security
- C#
- Code Scanning
- CodeQL
- DevOps
- DevSecOps
- GitHub
- GitHub Enterprise Server
- Improvement
- Java 26
- JavaScript
- Kotlin
- Maven
- MobX
- News
- Python
- React
- Ruby
- Rust
- Sanitization
- Security
- Security Query
- SSRF
- Static Analysis
- TypeScript
section_names:
- devops
- security
---
Allison provides an overview of CodeQL 2.24.3's new features, with an emphasis on Java 26 support, improved multi-language coverage, and static security enhancements for GitHub code scanning users.<!--excerpt_end-->

# CodeQL 2.24.3 Release: Java 26 Support and Enhanced Static Analysis

**Author: Allison**

CodeQL, the static analysis engine powering [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), has released version 2.24.3. This update brings feature enhancements that improve both the scope and accuracy of automated security checks across software projects.

## Key Improvements

### Java/Kotlin

- **Java 26 support:** Projects can now be scanned with the latest language features.
- **Improved Maven integration:** Java analysis selects the Java version from Maven POM files and tries Java 17+ by default for better build compatibility.

### JavaScript/TypeScript

- **MobX-React support:** Enhanced recognition of React components wrapped by `observer` from `mobx-react`/`mobx-react-lite` during code analysis.

### Python

- **Stronger SSRF defenses:** Introduction of a comprehensive SSRF sanitization barrier via the new AntiSSRF library.
- **Guard condition improvements:** Automatic handling of boolean checks on guards like `isSafe(x) == true` or `!= false`.

### Ruby

- Improved tracking of taint flow through utilities like `Shellwords.escape` and `Shellwords.shellescape` (with exceptions for command injection queries).

### Other Language Updates

- **Rust:** Support for neutral models, enhancing customization of source, sink, and flow summary models.
- **C/C++:** Fewer false positives with enhanced `cpp/leap-year/unchecked-after-arithmetic-year-modification` query.
- **C#:** Added support for the `field` keyword in properties (C# 14).
- **Java/Kotlin:** Extended coverage to handle both `javax` and `jakarta` package namespaces, potentially increasing alerts for users of the `jakarta` namespace.

## Integrating CodeQL Updates

- CodeQL 2.24.3 is automatically rolled out to GitHub.com code scanning users.
- For GitHub Enterprise Server (GHES) users, upgrades will be included in a future release, or upgrades can be performed manually for enhanced analysis capabilities.

For more technical details and the full changelog, visit the [official CodeQL v2.24.3 release notes](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.24.3/).

---

By staying up to date with CodeQL, organizations can identify vulnerabilities sooner and maintain robust application security as languages and frameworks evolve.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-10-codeql-2-24-3-adds-java-26-support-and-other-improvements)
