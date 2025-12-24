---
layout: "post"
title: "CodeQL 2.23.3 Adds Rust Security Query, Rust Support, and Easier C/C++ Scanning"
description: "This news update covers the release of CodeQL 2.23.3, the static analysis engine behind GitHub code scanning. The update introduces general availability for Rust analysis, a new Rust security query, and improved support for C/C++ scanning with the 'build mode none' option. The release also refines query coverage for Go and Java/Kotlin, aiming to reduce false positives and increase security findings’ accuracy and depth across common programming languages. GitHub users will automatically receive the new and improved CodeQL features, while GitHub Enterprise Server users may need to manually update."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-10-23-codeql-2-23-3-adds-a-new-rust-query-rust-support-and-easier-c-c-scanning"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-10-23 18:47:31 +00:00
permalink: "/news/2025-10-23-CodeQL-2233-Adds-Rust-Security-Query-Rust-Support-and-Easier-CC-Scanning.html"
categories: ["DevOps", "Security"]
tags: ["Application Security", "Build Automation", "C++", "CodeQL", "Data Flow Analysis", "DevOps", "GitHub", "Go", "Improvement", "Java", "Kotlin", "News", "Query Coverage", "Rust", "Secure Coding", "Security", "Security Scanning", "Static Analysis", "Taint Analysis"]
tags_normalized: ["application security", "build automation", "cplusplus", "codeql", "data flow analysis", "devops", "github", "go", "improvement", "java", "kotlin", "news", "query coverage", "rust", "secure coding", "security", "security scanning", "static analysis", "taint analysis"]
---

Allison announces the release of CodeQL 2.23.3, highlighting advancements in language support, security query coverage, and ease of scanning for multiple languages in GitHub code scanning workflows.<!--excerpt_end-->

# CodeQL 2.23.3 Adds Rust Security Query, Rust Support, and Easier C/C++ Scanning

**Author:** Allison

CodeQL, the static analysis engine behind [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), helps developers find and remediate security issues within their codebases. The recent [CodeQL 2.23.3 release](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.3/) includes major improvements in language and framework support as well as query accuracy and coverage.

## What's New in CodeQL 2.23.3?

### Language and Framework Enhancements

- **Rust:** Rust analysis is now generally available for code scanning. You can enable Rust scanning in your workflows directly.
- **C/C++:** The new `build-mode: none` feature is now generally available, allowing you to scan C/C++ projects even if a full build is impractical. This makes it easier to integrate security scanning into varied CI/CD environments.

### Query Updates and Improvements

- **Rust Security Query:**
  - Added the `rust/insecure-cookie` query. This new query flags cookies created without the `Secure` attribute, helping developers prevent insecure transmission over non-TLS channels.

- **Go:**
  - The `go/request-forgery` query now avoids alerting when user input is a simple type (like numbers or booleans), which reduces false positives.
  - The `go/unvalidated-url-redirection` query now treats a `url.URL` struct as tainted if its `Host` is initialized from untrusted input, for better coverage.
  - Safe URL modeling shared between `go/unvalidated-url-redirection` and `go/request-forgery` has been updated to reflect the above changes.

- **Java/Kotlin:**
  - When fields of objects stored in source arrays (e.g., `MyPojo[]`) are analyzed, they're now considered tainted if the array itself is a taint source. This increases data-flow coverage for object field sinks.

## Deployment and Getting the Update

- **GitHub.com users** get every new CodeQL version automatically for code scanning features.
- **GitHub Enterprise Server (GHES):** New CodeQL versions, including 2.23.3 capabilities, will appear in future releases. For older GHES installations, users can [manually upgrade CodeQL](https://docs.github.com/enterprise-server@3.17/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access).

For the complete list of detailed changes, refer to the official [CodeQL 2.23.3 changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.3/).

## Summary

This CodeQL release is focused on making security scanning more accessible for Rust and C/C++ developers, improving the accuracy of results, and expanding the scope of supported query types and data flows for Java, Kotlin, and Go. All updates are available for immediate use on GitHub, and organizations running on-premises solutions are encouraged to update CodeQL for complete coverage.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-23-codeql-2-23-3-adds-a-new-rust-query-rust-support-and-easier-c-c-scanning)
