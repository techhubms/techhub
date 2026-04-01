---
date: 2026-03-31 14:05:32 +00:00
section_names:
- devops
- security
feed_name: The GitHub Blog
tags:
- Application Security
- Browser Source Kinds
- C# 14
- CFG Rewrite
- CodeQL
- Data Extensions
- DevOps
- GHES Upgrade
- GitHub Advanced Security
- GitHub Code Scanning
- GitHub Enterprise Server
- Improvement
- Java
- Java Control Flow Graph
- JavaScript
- Kotlin
- News
- Partial Constructors
- ReceiveAsync
- SAST
- Secure Code Scanning
- Security
- Static Analysis
- Swift 6.2.4
- System.Net.WebSockets
- Taint Tracking
- TypeScript
external_url: https://github.blog/changelog/2026-03-31-codeql-2-25-0-adds-swift-6-2-4-support
title: CodeQL 2.25.0 adds Swift 6.2.4 support
primary_section: devops
author: Allison
---

Allison summarizes the CodeQL 2.25.0 release for GitHub code scanning, covering new language support (Swift 6.2.4), a rewritten Java control flow graph for better precision, and additional security-analysis improvements across C#, JavaScript, and TypeScript, plus notes on deployment to github.com and future GHES releases.<!--excerpt_end-->

# CodeQL 2.25.0 adds Swift 6.2.4 support

CodeQL is the static analysis engine behind [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), which aims to find and remediate security issues in code.

GitHub has released [CodeQL 2.25.0](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.25.0/) with improvements across multiple languages, and also notes that **CodeQL 2.25.1** is available but contains only minor bug fixes.

## Language and framework support

### Swift

- CodeQL now supports analysis of applications built with **Swift 6.2.4**.

### Java/Kotlin

- The **Java control flow graph (CFG)** implementation has been **completely rewritten**.
- The new CFG:
  - Includes additional nodes to more accurately represent certain constructs.
  - Only includes nodes reachable from the entry point.
- Intended outcome: improved overall analysis precision.

### C#

- Added support for **C# 14 partial constructors**.
- Added `System.Net.WebSockets::ReceiveAsync` as a **remote flow source**, improving detection of taint originating from WebSocket connections.

### JavaScript/TypeScript

- Added support for **browser-specific source kinds** (for use in data extensions), including:
  - `browser-url-query`
  - `browser-url-fragment`
  - `browser-message-event`

## Where to find the full list of changes

- See the full [changelog for CodeQL 2.25.0](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.25.0/).

## Deployment notes

- Every new CodeQL version is **automatically deployed** to users of GitHub code scanning on **github.com**.
- The functionality in CodeQL 2.25.0 will also be included in a future **GitHub Enterprise Server (GHES)** release.
- If you’re on an older GHES version, you can manually upgrade CodeQL: [Manually upgrade your CodeQL version](https://docs.github.com/enterprise-server@3.19/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access).


[Read the entire article](https://github.blog/changelog/2026-03-31-codeql-2-25-0-adds-swift-6-2-4-support)

