---
external_url: https://github.blog/changelog/2025-09-26-codeql-2-23-1-adds-support-for-java-25-typescript-5-9-and-swift-6-1-3
title: 'CodeQL 2.23.1 Released: Java 25, TypeScript 5.9, and Swift 6.1.3 Support'
author: Allison
feed_name: The GitHub Blog
date: 2025-09-26 19:28:56 +00:00
tags:
- Application Security
- C#
- Code Scanning
- CodeQL
- GitHub
- GitHub Actions
- GitHub Enterprise Server
- Improvement
- Java 25
- JavaScript
- Observability
- Python
- Query Suite
- Rust
- Security Queries
- Static Analysis
- Swift 6.1.3
- TypeScript 5.9
section_names:
- devops
- security
---
Allison reports on the release of CodeQL 2.23.1, highlighting enhanced language support and improved security analysis queries, offering stronger code scanning capabilities for GitHub users and organizations.<!--excerpt_end-->

# CodeQL 2.23.1 Released: Java 25, TypeScript 5.9, and Swift 6.1.3 Support

_CodeQL_ powers [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), enabling automated detection and remediation of security issues within source code.

## What’s New in CodeQL 2.23.1

**Language and Framework Support**

- **Java / Kotlin**: Support for Java 25, including new language features like compact source files (JEP 512) and module import declarations.
- **JavaScript / TypeScript**: Support for TypeScript 5.9.
- **Swift**: Analysis available for Swift 6.1.3 projects.

**Query Improvements and Additions**

- **Rust**: New SSRF (Server-Side Request Forgery) query to find web security vulnerabilities.
- **Java**: Reimplemented null dereference analysis (`java/dereferenced-value-may-be-null`) for higher precision and fewer false positives.
- **JavaScript / TypeScript**: The query detecting CORS HTTP header misconfigurations (`js/cors-permissive-configuration`) is now included in the default security suite.
- **Python**: Modernized several queries for `__init__` and `__del__` use, making results more precise and documentation clearer.
- **Go**: The path injection query can now detect more sanitization patterns; changes improve accuracy and reduce false alerts.
- **C/C++**: Several queries were moved from the default to the extended security suite, reducing alert volume for less precise checks.
- **C#**: Reduced false positives for `ToString()` calls on enum types in analysis.

**DevOps and Observability Enhancements**

- For users of **GitHub Actions**, file coverage data is now shown on the [code scanning tool status page](https://docs.github.com/code-security/code-scanning/managing-your-code-scanning-configuration/about-the-tool-status-page#viewing-the-tool-status-page-for-a-repository), making it easier to assess analysis completeness and coverage.
- The release also notes that updates will automatically reach GitHub.com users, and upcoming GitHub Enterprise Server (GHES) releases will include these improvements. Manual upgrade guidance is available for older GHES versions.

**Further Reading**

- [CodeQL 2.23.1 Changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.1/)
- [GitHub code scanning documentation](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql)

---

This release continues to elevate code quality and security standards, with tangible improvements for developers and security teams leveraging CodeQL through GitHub platforms.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-26-codeql-2-23-1-adds-support-for-java-25-typescript-5-9-and-swift-6-1-3)
