---
external_url: https://github.blog/changelog/2025-10-09-codeql-2-23-2-adds-additional-detections-for-rust-and-improves-accuracy-across-languages
title: CodeQL 2.23.2 Adds Rust Security Detections and Enhanced Language Support
author: Allison
feed_name: The GitHub Blog
date: 2025-10-09 22:05:23 +00:00
tags:
- Application Security
- C#
- CI/CD
- CodeQL
- Data Flow Analysis
- GitHub Code Scanning
- Go
- Improvement
- JavaScript
- Python
- Ruby
- Rust
- Static Analysis
- Taint Tracking
- TypeScript
- Vulnerability Detection
- DevOps
- Security
- News
- .NET
section_names:
- dotnet
- devops
- security
primary_section: dotnet
---
Allison reports on the CodeQL 2.23.2 release, detailing important new security queries for Rust, advanced data flow tracking, and improvements to code scanning accuracy across multiple languages.<!--excerpt_end-->

# CodeQL 2.23.2 Adds Rust Security Detections and Enhanced Language Support

**Author: Allison**

CodeQL, the static analysis engine underlying GitHub code scanning, has reached version 2.23.2. This update focuses on expanded security detection—especially for the Rust language—and improved precision for a broad range of supported technologies.

## Highlights of the Release

- **New Rust Security Query**: Detection for insecure non-HTTPS URLs in Rust projects (`rust/non-https-url`), safeguarding against network interception risks.
- **Improved Language Support**:
  - **JavaScript/TypeScript**: Better data flow tracking for GraphQL, and expanded support for AWS SDK packages (`aws-sdk`, `@aws-sdk/client-dynamodb`, etc.).
  - **Python**: Taint tracking now considers complex nested global variables. Regex safety queries accurately account for special assertion usage. The `py/inheritance/signature-mismatch` query is modernized, consolidating deprecated checks for clearer results.
  - **Ruby**: Initial recognition of API endpoints in Ruby apps using the Grape framework, which helps surface security issues in API code.
  - **Go**: Expanded analysis for private registries using Git Source and continued support for GOPROXY servers.
  - **C#**: Enhanced modeling of null guard expressions reduces false positives in dereference checks.

## Security and Code Quality Enhancements

- **Security Remediation**: The new features make it easier for teams to identify and remediate vulnerabilities before code reaches production.
- **Accurate Alerts**: Improvements minimize false positives and false negatives, giving engineering teams more actionable results.
- **Broader CI/CD Integration**: Every CodeQL release is automatically rolled out to GitHub code scanning users, with guidance available for self-managed GitHub Enterprise Server (GHES).

## Upgrade Guidance and Links

- [Full version 2.23.2 changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.23.2/)
- [Manual upgrade steps for GHES](https://docs.github.com/enterprise-server@3.17/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access)

## Conclusion

CodeQL 2.23.2 strengthens application security posture by expanding detection coverage and improving analysis quality across modern programming languages. Developers using GitHub code scanning can take immediate advantage of these improvements, reinforcing their secure software delivery practices.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-09-codeql-2-23-2-adds-additional-detections-for-rust-and-improves-accuracy-across-languages)
