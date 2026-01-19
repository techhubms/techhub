---
layout: post
title: 'CodeQL 2.23.6 Update: New C# Security Queries and Language Enhancements'
author: Allison
canonical_url: https://github.blog/changelog/2025-12-04-codeql-2-23-6-adds-swift-6-2-1-and-new-c-security-queries
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-12-04 16:42:39 +00:00
permalink: /coding/news/CodeQL-2236-Update-New-C-Security-Queries-and-Language-Enhancements
tags:
- Application Security
- Automated Security
- C#
- Code Scanning
- CodeQL
- GHES
- GitHub
- Improvement
- Java
- JavaScript
- Kotlin
- Precision Improvement
- Python
- Query Pack
- Ruby
- Rust
- Security Queries
- Software Security
- Static Analysis
- Swift 6.2.1
- TypeScript
section_names:
- coding
- devops
- security
---
Allison outlines CodeQL 2.23.6's key improvements, including new C# security queries and expanded language support, enhancing GitHub code scanning's effectiveness.<!--excerpt_end-->

# CodeQL 2.23.6 Update: New C# Security Queries and Language Enhancements

CodeQL—the static analysis engine behind [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql)—has been updated to version 2.23.6. This release delivers notable enhancements for security and development teams using GitHub and GitHub Enterprise Server (GHES).

## Language and Framework Support

- **Swift**: Support is expanded to apps built with Swift 6.2.1.
- **Rust**: New models are provided for cookie-related methods in the `poem` crate.

## Security Query Updates

- **C#**:
  - The queries `cs/web/cookie-secure-not-set` and `cs/web/cookie-httponly-not-set` have been promoted from experimental to standard query packs. These identify cookies lacking appropriate security attributes (Secure, HttpOnly).
  - The Guards library now recognizes disjunctions better, improving precision for queries like `cs/constant-condition`, `cs/inefficient-containskey`, and `cs/dereferenced-value-may-be-null`.
- **Rust**:
  - Added taint flow barriers help reduce false positives for queries such as `rust/regex-injection`, `rust/sql-injection`, and `rust/log-injection`.
- **Java/Kotlin**:
  - The `security-severity` score for `java/overly-large-range` and `java/insecure-cookie` are lowered (5.0 → 4.0) to more accurately reflect risk.
- **JavaScript/TypeScript**:
  - The XSS query `js/xss-through-dom` has its `security-severity` score raised (6.1 → 7.8), recognizing increased risk, while `js/overly-large-range` is reduced (5.0 → 4.0).
- **Python** and **Ruby**:
  - Both have the `security-severity` score for `overly-large-range` lowered (5.0 → 4.0) for better accuracy.

## Deployment and Upgrade

- The updated queries and features are deployed automatically to GitHub code scanning users on github.com.
- The functionality is included in the upcoming GitHub Enterprise Server (GHES) 3.20 release.
- Manual upgrade instructions are provided for maintaining CodeQL on older GHES versions without internet access.

For comprehensive details on all changes, visit the [CodeQL 2.23.6 changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.6/).

---

**Key Takeaways:**

- Improved C# cookie security analysis
- Enhanced language support for Swift, Rust, and more
- Adjusted security severity scores for greater situational accuracy
- Seamless rollout for GitHub code scanning users

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-04-codeql-2-23-6-adds-swift-6-2-1-and-new-c-security-queries)
