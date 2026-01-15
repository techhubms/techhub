---
layout: post
title: Incremental Security Analysis with CodeQL Now Available Across All Languages
author: Allison
canonical_url: https://github.blog/changelog/2025-09-23-incremental-security-analysis-with-codeql-is-now-available-for-all-languages
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-09-23 15:21:27 +00:00
permalink: /coding/news/Incremental-Security-Analysis-with-CodeQL-Now-Available-Across-All-Languages
tags:
- Application Security
- C/C++
- C#
- CodeQL
- CodeQL CLI
- Coding
- DevOps
- GitHub
- GitHub Enterprise Server
- Go
- Incremental Analysis
- News
- Pull Requests
- Security
- Security Scanning
- Static Analysis
- Swift
section_names:
- coding
- devops
- security
---
Allison explains GitHub's new incremental analysis for CodeQL, enabling faster and more efficient security scans across all supported languages, including C#, in pull requests.<!--excerpt_end-->

# Incremental Security Analysis with CodeQL Now Available Across All Languages

GitHub has announced the availability of incremental analysis for CodeQL across all supported languages, including Go, C#, C/C++, and Swift. This update applies to CodeQL scans performed on pull requests, where only the code that is new or changed is analyzed during the evaluation step. As a result, developers benefit from improved scan speeds and quicker feedback without sacrificing the thoroughness of security checks.

## Key Improvements

- **Incremental Analysis for All Languages**: Now, every CodeQL-supported language, including C#, benefits from the incremental scan mode. This accelerates security reviews on pull requests.
- **Performance Gains**:
  - C# and C/C++ evaluations are around 5% faster.
  - Go evaluations are approximately 20% faster, with some scans showing over 40% improvement compared to full project scans.
- **Comparison Basis**: These improvements are achieved by comparing the time spent on pull request scans (incremental) versus scheduled runs that analyze the entire codebase.

## Availability

- Incremental analysis is enabled by default on [github.com](https://github.com).
- The feature will become available for [CodeQL CLI](https://docs.github.com/en/code-security/codeql-cli) users in the future.
- On GitHub Enterprise Server, incremental analysis support begins with version 3.19.

## Background and Reporting Changes

- Recent changes to how CodeQL processes and reports data were already applied to all supported languages during the earlier release of incremental analysis.
- No further reporting or processing modifications are introduced in this update.

## Why This Matters

This release completes the first stage of GitHub’s plan to optimize CodeQL scanning speed and developer experience, offering faster feedback loops while upholding the quality of security assessments relied upon by many teams.

### References

- [GitHub Changelog: Incremental security analysis](https://github.blog/changelog/2025-05-28-incremental-security-analysis-makes-codeql-up-to-20-faster-in-pull-requests/)
- [Incremental security analysis with CodeQL is now available for all languages](https://github.blog/changelog/2025-09-23-incremental-security-analysis-with-codeql-is-now-available-for-all-languages)
- [CodeQL CLI Documentation](https://docs.github.com/en/code-security/codeql-cli)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-23-incremental-security-analysis-with-codeql-is-now-available-for-all-languages)
