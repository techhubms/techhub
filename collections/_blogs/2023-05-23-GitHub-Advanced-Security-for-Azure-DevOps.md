---
external_url: https://devopsjournal.io/blog/2023/05/23/GitHub-Advanced-Security-Azure-DevOps
title: GitHub Advanced Security for Azure DevOps
author: Rob Bos
feed_name: Rob Bos' Blog
date: 2023-05-23 00:00:00 +00:00
tags:
- Azure DevOps
- Code Scanning
- CodeQL
- Dependency Scanning
- DevSecOps
- GitHub Advanced Security
- Microsoft Build
- Pipeline Security
- Secret Scanning
- Security Automation
section_names:
- devops
- security
- azure
---
In this article, Rob Bos explores the public preview of GitHub Advanced Security (GHAS) features recently introduced to Azure DevOps, as announced at Microsoft Build 2023, and shares firsthand experiences and key distinctions compared to GitHub's native implementation.
<!--excerpt_end-->

## Introduction

GitHub Advanced Security (GHAS) provides robust tools to secure code and CI/CD pipelines, including Code Scanning, Secret Scanning, and Dependency Scanning. With Microsoft’s integration of GHAS into Azure DevOps, these tools are now accessible in another major platform, catering to user demand for enhanced security.

## Overview of GitHub Advanced Security Tools

- **Dependency Scanning**: Automatically reviews dependency manifest files (e.g., `package.json`) to detect vulnerable packages, suggesting upgrades as needed. On GitHub, this process can create pull requests to update dependencies—a feature not yet present in Azure DevOps.
- **Secret Scanning**: Identifies accidental commits of sensitive information, such as passwords or tokens, even scanning repository history and branches. Includes 'push protection' to prevent new secrets from being pushed.
- **Code Scanning**: Utilizes CodeQL or similar SAST tools to analyze source code and pull requests for vulnerabilities, generating alerts and providing information based on defined query suites.

## Enabling GHAS in Azure DevOps

GHAS must be activated at the repository level by a project admin. Once enabled, a dedicated 'Advanced Security' tab appears, revealing scan results and introducing new pipeline tasks:

- **Secret scanning** is enabled by default when the advanced features are turned on.
- **Dependency scanning** requires injecting a specific task into the build pipeline.
- **Code scanning** leverages CodeQL with a series of tasks to initialize, build, analyze, and publish the scan results.

### Code Scanning Example

Use tasks in your pipeline such as:

```yaml
- task: AdvancedSecurity-Codeql-Init@1

  inputs:
    languages: 'csharp'

- task: AdvancedSecurity-Codeql-Autobuild@1
- task: AdvancedSecurity-Codeql-Analyze@1
- task: AdvancedSecurity-Publish@1
```

You can customize which query suites to run (e.g., `security-and-quality`) for broader or stricter scanning.

## Handling Alerts

- **Secret Alerts**: Fix by revoking compromised credentials; currently, no dismissal through the UI is available.
- **Dependency Alerts**: Upgrade dependencies and rerun scans; no automated pull requests yet—this is manual for now.
- **Code Scanning Alerts**: Fix the reported code vulnerabilities; changes are tracked, and alerts close upon verification.

Some management features, like dismissing alerts or excluding code from scans, are presently unavailable but expected in future updates.

## Dashboarding and Monitoring

While a project-level security dashboard isn’t yet built directly into Azure DevOps, broader views can be accessed via Microsoft Defender for DevOps (feature availability may require enablement). These dashboards are expected to provide organization-wide insight, though current capabilities remain limited.

## Summary

Rob Bos highlights that integrating GHAS into Azure DevOps marks a significant leap for in-pipeline security. Though some features from GitHub’s implementation are missing or altered—such as dashboarding and automated dependency pull requests—the foundational tools are in place. Early adoption feedback and Microsoft’s rapid iteration suggest comprehensive enhancements will follow, further empowering developers to proactively identify and resolve security issues within Azure DevOps.

This post appeared first on Rob Bos' Blog. [Read the entire article here](https://devopsjournal.io/blog/2023/05/23/GitHub-Advanced-Security-Azure-DevOps)
