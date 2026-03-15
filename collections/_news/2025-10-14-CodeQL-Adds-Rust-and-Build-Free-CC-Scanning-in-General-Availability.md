---
external_url: https://github.blog/changelog/2025-10-14-codeql-scanning-rust-and-c-c-without-builds-is-now-generally-available
title: CodeQL Adds Rust and Build-Free C/C++ Scanning in General Availability
author: Allison
feed_name: The GitHub Blog
date: 2025-10-14 19:52:02 +00:00
tags:
- Application Security
- Build Automation
- C/C++
- Code Scanning
- CodeQL
- CodeQL CLI
- Continuous Integration
- Copilot Autofix
- GitHub
- GitHub Actions
- GitHub Enterprise Server
- OWASP Top 10
- Release Notes
- Rust
- Security Analysis
- DevOps
- Security
- News
section_names:
- devops
- security
primary_section: devops
---
Allison details GitHub's release of CodeQL support for Rust and build-free C/C++ scanning, enabling faster, more accessible security analysis for developers.<!--excerpt_end-->

# CodeQL Adds Rust and Build-Free C/C++ Scanning in General Availability

GitHub has announced that CodeQL—the engine behind GitHub code scanning—now generally supports Rust and build-free C/C++ project analysis. This marks the end of their public preview periods for both features, enabling security-focused development teams to identify vulnerabilities more efficiently.

## Rust Support

- **General availability:** Rust joins C/C++, Java/Kotlin, JS/TS, Python, Ruby, C#, Go, GitHub Actions, and Swift as supported languages.
- **Security coverage:** The analysis addresses the OWASP Top 10 categories (except *A06:2021-Vulnerable and Outdated Components*, which uses [Dependabot](https://docs.github.com/code-security/getting-started/dependabot-quickstart-guide)).
- **Setup flexibility:** Both [default setup](https://docs.github.com/code-security/code-scanning/enabling-code-scanning/configuring-default-setup-for-a-repository) and [advanced setup](https://docs.github.com/code-security/code-scanning/creating-an-advanced-setup-for-code-scanning/configuring-advanced-setup-for-code-scanning) are supported.
- **Autofix:** Integration with [Copilot Autofix](https://github.blog/news-insights/product-news/found-means-fixed-introducing-code-scanning-autofix-powered-by-github-copilot-and-codeql/) offers automated fix recommendations based on scan results.
- **Query Documentation:** A detailed list of security queries for Rust is available in the [CodeQL documentation](https://codeql.github.com/codeql-query-help/rust/).

## Build-Free C/C++ Scanning

- **General availability:** CodeQL can now scan C/C++ codebases without requiring project builds.
- **Preview results:** During preview, over 10,000 repositories adopted the feature, achieving over 70% success rates with minimal manual setup.
- **Operation:** [Build mode none](https://docs.github.com/code-security/code-scanning/creating-an-advanced-setup-for-code-scanning/codeql-code-scanning-for-compiled-languages#about-build-mode-none-for-codeql) is now standard in default setup for C/C++.
- **Impact:** This shift has substantially accelerated adoption, with one customer enabling security scans across 1,400+ repositories in under two days. Previously, this would have taken much longer.

![A line graph showing the number of C/C++ repositories enabled with CodeQL over a period of time ranging from August 17, 2025 to October 12, 2025. The graph has a jump of approximately 1,400 repositories over a two day period, from August 26 to August 28](https://github.com/user-attachments/assets/f3a46d0e-4388-485b-b6bc-d9f9d77a4028)

## Availability

- Available on [github.com](https://github.com), CodeQL CLI `2.23.3`, and [GitHub Enterprise Server](https://docs.github.com/en/enterprise-server@3.20/admin) from version 3.20.

## Key Takeaways

- Security scanning is more accessible for Rust and C/C++ developers.
- Teams can onboard large codebases to security workflows with minimal friction.
- Support for automated fixes via Copilot Autofix speeds up time to remediation.

For further details and setup guides, see the [GitHub Blog announcement](https://github.blog/changelog/2025-10-14-codeql-scanning-rust-and-c-c-without-builds-is-now-generally-available).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-14-codeql-scanning-rust-and-c-c-without-builds-is-now-generally-available)
