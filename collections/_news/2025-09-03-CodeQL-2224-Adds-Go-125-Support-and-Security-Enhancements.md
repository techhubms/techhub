---
external_url: https://github.blog/changelog/2025-09-02-codeql-2-22-4-adds-support-for-go-1-25-and-accuracy-improvements
title: CodeQL 2.22.4 Adds Go 1.25 Support and Security Enhancements
author: Allison
feed_name: The GitHub Blog
date: 2025-09-03 08:41:54 +00:00
tags:
- C++
- Code Scanning
- CodeQL
- GitHub
- Go
- Java
- JavaScript
- Kotlin
- Rust
- Secure Development
- SQL Injection
- Static Analysis
- TypeScript
- Vulnerability Detection
section_names:
- devops
- security
---
Allison introduces CodeQL 2.22.4, describing new language support, advanced security queries, and accuracy improvements for developers leveraging GitHub code scanning.<!--excerpt_end-->

# CodeQL 2.22.4 Adds Go 1.25 Support and Accuracy Improvements

CodeQL, the static analysis engine powering GitHub code scanning, continues to support developers in detecting and remediating security issues within their codebases. The latest release, CodeQL 2.22.4, delivers several noteworthy improvements:

## Language & Framework Support

- **Go:** Now supports Go version 1.25, expanding compatibility for Go projects.
- **Rust:** Improved analysis thanks to enhanced models for commonly used database libraries, such as `postgres`, `rusqlite`, `sqlx`, and `tokio-postgres`, with a focus on detecting SQL injection and cleartext storage vulnerabilities.
- **Java/Kotlin:** New library models for `jakarta.servlet.ServletRequest` and `jakarta.servlet.http.HttpServletRequest` expand remote flow source detection, improving security query depth.

## Security Query Changes

- **Rust:** Added a `rust/cleartext-storage-database` query, designed to catch instances of sensitive data storage in plaintext within databases.
- **C/C++:** Addressed false positives in the `cpp/overflow-buffer` query involving reference types for class/struct buffers.
- **JavaScript/TypeScript:** The `js/regex-injection` query now ignores environment variables as sources by default, refining detection quality.

## Deployment & Upgrade Notes

- CodeQL 2.22.4 is automatically rolled out for users of GitHub code scanning on github.com.
- The release is also included in GitHub Enterprise Server (GHES) version 3.19.
- Users operating older GHES versions may manually upgrade CodeQL as outlined in the [documentation](https://docs.github.com/enterprise-server@3.17/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access).

## Further Reading

For a complete breakdown of the changes in this release, see the full [CodeQL 2.22.4 changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.22.4/).

---

These updates help developers and security teams keep their code safe and aligned with recent language and framework updates.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-02-codeql-2-22-4-adds-support-for-go-1-25-and-accuracy-improvements)
