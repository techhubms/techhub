---
layout: "post"
title: "CodeQL 2.24.2: Go 1.26, Kotlin 2.3.10 Support and Query Accuracy Improvements"
description: "This news update covers the release of CodeQL 2.24.2, the static analysis engine that powers GitHub code scanning. The update brings support for Go 1.26 and Kotlin 2.3.10, with enhancements that improve security issue detection and query accuracy. Changes impact language and framework support across Go, Kotlin, Python, C#, and Java, with particular improvements for request forgery, regular expression sanitizing, and support for Azure SDK in Python."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-24-codeql-adds-go-1-26-and-kotlin-2-3-10-support-and-improves-query-accuracy"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-24 22:20:46 +00:00
permalink: "/2026-02-24-CodeQL-2242-Go-126-Kotlin-2310-Support-and-Query-Accuracy-Improvements.html"
categories: ["DevOps", "Security"]
tags: ["Application Security", "Azure SDK", "C#", "CodeQL", "Cross Site Request Forgery", "DevOps", "GHES", "GitHub Code Scanning", "Go 1.26", "Improvement", "Kotlin 2.3.10", "News", "Python", "Query Accuracy", "Regular Expressions", "Sanitizer", "Secure Coding", "Security", "Static Analysis"]
tags_normalized: ["application security", "azure sdk", "csharp", "codeql", "cross site request forgery", "devops", "ghes", "github code scanning", "go 1dot26", "improvement", "kotlin 2dot3dot10", "news", "python", "query accuracy", "regular expressions", "sanitizer", "secure coding", "security", "static analysis"]
---

Allison details the CodeQL 2.24.2 release, highlighting expanded language support, security query updates for Microsoft technologies, and improved detection accuracy for DevOps and security teams.<!--excerpt_end-->

# CodeQL 2.24.2: Go 1.26, Kotlin 2.3.10 Support and Query Accuracy Improvements

**Author:** Allison

## Overview

CodeQL is the static analysis engine behind GitHub code scanning, empowering teams to find and remediate security vulnerabilities in codebases. The latest release, CodeQL 2.24.2, introduces support for Go 1.26 and Kotlin 2.3.10, and brings several improvements to language coverage and analysis precision.

## Language and Framework Support

- **Go**: Analysis now supports Go 1.26.
- **Kotlin**: Kotlin versions up to 2.3.10 are fully analyzable.
- **Python**: Request forgery sink models have been added for the Azure SDK, strengthening detection for applications using Microsoft cloud services.

## Security Query Changes

- **C#**: The `cs/web/missing-token-validation` query has been enhanced to recognize antiforgery attributes on base controller classes. This update fixes false positives where parent classes apply `[ValidateAntiForgeryToken]` or `[AutoValidateAntiforgeryToken]`.
- **Java/Kotlin**: Query logic now acknowledges additional sanitizing patterns, such as string matching via regular expressions and the use of the `@javax.validation.constraints.Pattern` annotation, across SSRF, path-injection, and log-injection queries.

## Deployment

- All GitHub.com code scanning users receive these updates automatically.
- The new functionality is also forthcoming in a future GitHub Enterprise Server (GHES) release. Manual upgrade guidance is available for users of older GHES versions.

## Further Reading

- [GitHub code scanning with CodeQL](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql)
- [CodeQL 2.24.2 Changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.24.2/)
- [Manual upgrade instructions for GHES](https://docs.github.com/enterprise-server@3.19/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access)

## Summary

This release makes CodeQL more effective at identifying security flaws in modern Go and Kotlin applications and improves accuracy for existing queries, especially for teams leveraging the Azure SDK in Python or antiforgery features in C# web applications.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-24-codeql-adds-go-1-26-and-kotlin-2-3-10-support-and-improves-query-accuracy)
