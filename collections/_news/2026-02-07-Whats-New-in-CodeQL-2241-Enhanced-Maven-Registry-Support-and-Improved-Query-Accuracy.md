---
external_url: https://github.blog/changelog/2026-02-06-codeql-2-24-1-improves-maven-private-registry-support-and-improves-query-accuracy
title: 'What’s New in CodeQL 2.24.1: Enhanced Maven Registry Support and Improved Query Accuracy'
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-07 00:05:05 +00:00
tags:
- Application Security
- Buffer Overflow
- C#
- C++
- CI/CD
- CodeQL
- DevOps
- GitHub Code Scanning
- GitHub Enterprise Server
- Improvement
- Java
- Kotlin
- Maven
- News
- OpenAI Module
- Prompt Injection
- Python
- Security
- Security Queries
- Static Analysis
- Struts
- Taint Flow
section_names:
- devops
- security
---
Allison details the improvements in CodeQL 2.24.1, focusing on enhanced Maven registry support, expanded language coverage, and key query accuracy upgrades that help developers secure their codebase.<!--excerpt_end-->

# What’s New in CodeQL 2.24.1: Enhanced Maven Registry Support and Improved Query Accuracy

**Author:** Allison

CodeQL, the static analysis engine that powers [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), has been updated to version 2.24.1. This release brings several important improvements aimed at both expanding the scope of supported technologies and increasing the reliability of security scans in practice.

## Key Improvements

### Enhanced Maven Private Registry Support

- CodeQL now provides improved support for Maven-compatible private package registries, including configuration as plugin repositories within organizations using Default Setup.
- This enables organizations to easily use Maven plugins from private registries during analysis workflows.

### Expanded Language and Framework Coverage

- **Kotlin**: Support for versions up to 2.3.0 is now included. Note that support for 1.6.x and 1.7.x has been dropped.
- **Java**: Expanded framework recognition with support for Struts 7.x package names.
- **C/C++**: Added support for new preprocessor directives in C23 and C++26 standards.
- **C#**: Now supports null-conditional assignments (C# 14 feature).
- **Python**: Improved model-as-data language to reference list elements, expanded taint flow and type models for the `agents` and `openai` modules, and added remote flow sources for the `websockets` package.

### Query Accuracy and Bug Fixes

- **C/C++**: Fixed a bug in the `GuardCondition` library and improved buffer size measurement, reducing false positives for several buffer overflow queries.
- **Java**: Enhanced the accuracy of the `java/unreleased-lock` query.
- **Python**: Introduced an experimental `py/prompt-injection` query to address potential LLM prompt injection issues.
- **GitHub Actions**: Addressed a crash scenario related to long string expressions.

## Deployment and Upgrade Notes

- These updates are automatically deployed for users of GitHub code scanning on github.com.
- The new features will be included in a future GitHub Enterprise Server release. Manual upgrades are possible for self-hosted older instances; [follow the instructions here](https://docs.github.com/enterprise-server@3.19/admin/managing-code-security/managing-github-advanced-security-for-your-enterprise/configuring-code-scanning-for-your-appliance#configuring-codeql-analysis-on-a-server-without-internet-access).

For full details, refer to the [CodeQL 2.24.1 changelog](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.24.1/).

---

By maintaining a focus on up-to-date language support and query precision, CodeQL 2.24.1 further strengthens the security posture of projects utilizing GitHub code scanning. Developers and DevOps engineers are encouraged to review the changelog and upgrade where necessary.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-06-codeql-2-24-1-improves-maven-private-registry-support-and-improves-query-accuracy)
