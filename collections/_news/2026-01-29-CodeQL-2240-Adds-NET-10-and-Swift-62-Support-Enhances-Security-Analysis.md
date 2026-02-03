---
external_url: https://github.blog/changelog/2026-01-29-codeql-2-24-0-adds-swift-6-2-support-net-10-compatibility-and-file-handling-for-minified-javascript
title: CodeQL 2.24.0 Adds .NET 10 and Swift 6.2 Support, Enhances Security Analysis
author: Allison
primary_section: coding
feed_name: The GitHub Blog
date: 2026-01-29 23:17:01 +00:00
tags:
- .NET 10
- Application Security
- ASP.NET Core
- C# 14
- CI/CD
- Code Scanning
- CodeQL
- Coding
- DevOps
- DevOps Tools
- GitHub
- Improvement
- News
- NHibernate
- Query Models
- Security
- Software Quality
- Static Analysis
- Taint Tracking
- Vulnerability Detection
section_names:
- coding
- devops
- security
---
Allison introduces the CodeQL 2.24.0 release, highlighting improved security analysis, expanded language support including .NET 10 and C# 14, and new features beneficial to developers and DevOps teams.<!--excerpt_end-->

# CodeQL 2.24.0 Adds .NET 10 and Swift 6.2 Support, Enhances Security Analysis

CodeQL, the static analysis engine behind [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), has released version 2.24.0. This version brings significant updates in terms of language support, security query enhancements, and framework compatibility.

## Key Updates

### Language and Framework Support

- **Swift**: Analysis support added for Swift 6.2.2 and 6.2.3.
- **C# and .NET**: Now supports analysis of projects built with .NET 10 and C# 14.
- **JavaScript/TypeScript**: Improved handling of minified JavaScript (automatic exclusion unless explicitly enabled), support for Next.js 16’s `use cache` directives, and better tracking in React’s `useRef` hook.
- **Python**: New support for `compression.zstd` in the `py/decompression-bomb` query, models for `urllib.parse`, and analysis of files in hidden directories.
- **Java/Kotlin**: New sink models for `com.couchbase` and other frameworks, enhanced dataflow models, and improved thread safety analysis.
- **C/C++**: Additional Windows API coverage, new flow models for SQLite and OpenSSL, and improved constant comparison query accuracy.
- **Rust**: Improved method resolution with the `Deref` trait, Axum web framework support, better type inference, and refined query precision for unused variables and hard-coded cryptographic values.

### Security Query Improvements

- **C#**: Queries for cross-site request forgery expanded to ASP.NET Core, and new SQL injection sink models added for NHibernate types. Improvements to taint-tracking queries for `System.Collections.Generic.KeyValuePair.Value`.
- **JavaScript/TypeScript**: Bug fixes in Next.js support and adjustments to cross-site scripting detection.
- **Java/Kotlin**: Thread-safety understanding and new sanitizers for server-side request forgery (SSRF).
- **C/C++ & Rust**: Reduction in false positives, new sink models for hard-coded cryptographic values, and improved heuristics for identifying insecure practices.

### Deployment and Availability

- CodeQL 2.24.0 is automatically available to users of GitHub code scanning on github.com. It will be included in future releases of GitHub Enterprise Server; organizations on older versions can [manually upgrade CodeQL](https://docs.github.com/enterprise-server@3.19/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access).

---
For complete details, see the [CodeQL 2.24.0 changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.24.0/).

**Author:** Allison

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-29-codeql-2-24-0-adds-swift-6-2-support-net-10-compatibility-and-file-handling-for-minified-javascript)
