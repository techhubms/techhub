---
date: 2026-04-21 16:07:48 +00:00
title: CodeQL now supports sanitizers and validators in models-as-data
primary_section: devops
tags:
- Application Security
- Barrier Guards
- Barriers
- C#
- CodeQL
- CodeQL CLI 2.25.2
- CodeQL Model Packs
- Codeqlpack.yml
- Data Extensions
- DevOps
- GitHub Advanced Security
- GitHub Code Scanning
- Java
- JavaScript
- Models as Data
- News
- Python
- Sanitizers
- Secure Code Scanning
- Security
- Static Analysis
- Taint Tracking
- TypeScript
- Validators
- YAML
author: Allison
external_url: https://github.blog/changelog/2026-04-21-codeql-now-supports-sanitizers-and-validators-in-models-as-data
feed_name: The GitHub Blog
section_names:
- devops
- security
---

Allison announces a CodeQL update for GitHub code scanning that lets teams declare custom sanitizers and validators as YAML “models-as-data”, improving taint-tracking accuracy without writing custom CodeQL.<!--excerpt_end-->

# CodeQL now supports sanitizers and validators in models-as-data

CodeQL is the static analysis engine behind [GitHub code scanning](https://docs.github.com/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning-with-codeql), which finds and remediates security issues in your code. You can now define custom sanitizers and validators using data extensions (models-as-data) across C/C++, C#, Go, Java/Kotlin, JavaScript/TypeScript, Python, Ruby, and Rust.

## What’s new

In CodeQL, sanitizers and validators are represented as **barriers** and **barrier guards** respectively.

- **Barriers** and **barrier guards** let you customize how CodeQL tracks tainted data through your code.
- A **barrier** marks a function or method whose output is considered sanitized for a specific vulnerability type.
  - Example: an HTML-escaping function that prevents cross-site scripting (XSS)
- A **barrier guard** marks a function that returns a boolean indicating whether data is safe, stopping taint flow through guarded branches.

Previously, defining barriers required writing custom CodeQL code. Now, you can add them declaratively in YAML data extension files using two new extensible predicates:

- `barrierModel`
  - Stops taint flow at the modeled element for a specified query kind.
- `barrierGuardModel`
  - Stops taint flow when a conditional check returns an expected value.

You can add barriers modeled with these new predicates to [CodeQL model packs](https://docs.github.com/code-security/tutorials/customize-code-scanning/customizing-analysis-with-codeql-packs#codeqlpack-yml-properties). This makes it easier to extend CodeQL’s analysis to recognize your project’s own sanitization and validation functions without writing custom CodeQL.

## Learn more

For details and examples on how to define barriers and barrier guards for your language, see the customization guides:

- [Customizing library models for C and C++](https://codeql.github.com/docs/codeql-language-guides/customizing-library-models-for-cpp/)
- [Customizing library models for C#](https://codeql.github.com/docs/codeql-language-guides/customizing-library-models-for-csharp/)
- [Customizing library models for Go](https://codeql.github.com/docs/codeql-language-guides/customizing-library-models-for-go/)
- [Customizing library models for Java and Kotlin](https://codeql.github.com/docs/codeql-language-guides/customizing-library-models-for-java-and-kotlin/)
- [Customizing library models for JavaScript](https://codeql.github.com/docs/codeql-language-guides/customizing-library-models-for-javascript/)
- [Customizing library models for Python](https://codeql.github.com/docs/codeql-language-guides/customizing-library-models-for-python/)
- [Customizing library models for Ruby](https://codeql.github.com/docs/codeql-language-guides/customizing-library-models-for-ruby/)

For more information about using CodeQL model packs in GitHub code scanning, see [Extending CodeQL coverage with CodeQL model packs](https://docs.github.com/code-security/how-tos/find-and-fix-code-vulnerabilities/manage-your-configuration/editing-your-configuration-of-default-setup#extending-codeql-coverage-with-codeql-model-packs-in-default-setup).

This feature is available starting with [CodeQL 2.25.2](https://codeql.github.com/docs/codeql-overview/codeql-changelog/codeql-cli-2.25.2/).


[Read the entire article](https://github.blog/changelog/2026-04-21-codeql-now-supports-sanitizers-and-validators-in-models-as-data)

