---
external_url: https://github.blog/changelog/2026-03-24-faster-incremental-analysis-with-codeql-in-pull-requests
feed_name: The GitHub Blog
author: Allison
section_names:
- devops
- dotnet
- security
tags:
- .NET
- Advanced Setup
- Application Security
- Build Mode None
- C#
- Cached Database
- CI/CD
- Code Scanning
- CodeQL
- CodeQL CLI
- CodeQL Database
- Default CodeQL Query Suite
- Default Setup
- DevOps
- GitHub Actions
- GitHub Advanced Security
- Improvement
- Incremental Analysis
- Incremental Scanning
- Java
- JavaScript
- News
- Pull Requests
- Python
- Ruby
- SAST
- Security
- Static Analysis
- TypeScript
date: 2026-03-24 13:25:15 +00:00
primary_section: dotnet
title: Faster incremental analysis with CodeQL in pull requests
---

Allison shares a GitHub update: CodeQL pull request scans for C#, Java, JavaScript/TypeScript, Python, and Ruby are now incremental by default, reducing scan times by analyzing only changed code and combining it with a cached database of the full codebase.<!--excerpt_end-->

# Faster incremental analysis with CodeQL in pull requests

CodeQL scans on pull requests for **C#**, **Java**, **JavaScript/TypeScript**, **Python**, and **Ruby** are now **incremental**, which makes them faster.

Earlier this year, GitHub reported CodeQL pull request scans were sped up by ~20% by analyzing only new or changed code:

- [Incremental security analysis makes CodeQL up to 20% faster in pull requests](https://github.blog/changelog/2025-05-28-incremental-security-analysis-makes-codeql-up-to-20-faster-in-pull-requests/)

## What changed

GitHub further improved incremental analysis performance by:

- Generating a **CodeQL database** representing the **new or changed code** introduced in a pull request
- Combining that database with a **cached database** for the **entire codebase**

## Observed speedups

Across **100,000+ repositories**, GitHub grouped repositories by how long a non-incremental scan takes:

- Less than three minutes
- Between three and seven minutes
- Over seven minutes

GitHub then measured average per-language speedups over a seven-day period.

![Seven day average speedup per language, split by non-incremental scan duration under three, between three and seven, over seven minutes. Java 22%, 32%, 51%, C# 4%, 6%, 8%, JavaScript/TypeScript 29%, 47%, 70%, Python 11%, 57%, 70%, Ruby 10%, 43%, 63%](https://github.com/user-attachments/assets/0ddacdce-622c-4329-8c78-2a3540d1df7a)

## Compatibility and limitations

- This improvement only applies if you use the **default CodeQL query suite**.
- Incremental analysis is **enabled by default** for **C#**, **Java**, **JavaScript/TypeScript**, **Python**, and **Ruby** projects that use the **`build mode none`** extraction mechanism.
- This applies to both **default setup** and **advanced setup** on **github.com**.
- **CodeQL CLI** will get support for incremental scanning **later**.

## Source

The original post is on the GitHub Blog:

- Faster incremental analysis with CodeQL in pull requests (The GitHub Blog)


[Read the entire article](https://github.blog/changelog/2026-03-24-faster-incremental-analysis-with-codeql-in-pull-requests)

