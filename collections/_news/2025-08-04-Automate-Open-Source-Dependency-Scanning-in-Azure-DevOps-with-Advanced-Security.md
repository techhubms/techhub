---
layout: post
title: Automate Open-Source Dependency Scanning in Azure DevOps with Advanced Security
author: Laura Jiang
canonical_url: https://devblogs.microsoft.com/devops/automate-your-open-source-dependency-scanning-with-advanced-security/
viewing_mode: external
feed_name: Microsoft DevBlog
feed_url: https://devblogs.microsoft.com/devops/feed/
date: 2025-08-04 17:17:37 +00:00
permalink: /azure/news/Automate-Open-Source-Dependency-Scanning-in-Azure-DevOps-with-Advanced-Security
tags:
- Advanced Security
- Azure DevOps
- Build Validation
- CI/CD
- Dependency Scanning
- DevOps Pipelines
- GitHub Advanced Security
- Open Source
- Open Source Vulnerabilities
- Pipeline Automation
- Pull Request Annotations
- Repository Settings
- Security Alerts
section_names:
- azure
- devops
- security
---
Laura Jiang explores automating open-source dependency scanning in Azure DevOps with GitHub Advanced Security, focusing on setup, integration, and how results are surfaced for developers.<!--excerpt_end-->

# Automate Open-Source Dependency Scanning in Azure DevOps with Advanced Security

*Author: Laura Jiang*

Managing security across multiple repositories and teams can be a challenge, especially when additional setup is needed for each experience. In GitHub Advanced Security for Azure DevOps, there’s ongoing work to simplify feature enablement and make it easier to scale advanced security measures across entire enterprises.

## Automatic Injection of Dependency Scanning Tasks

A recent update makes it possible to automatically inject a dependency scanning task into any pipeline run targeting your default branch. This ensures that both your production code and any code merged into your production branch undergo evaluation for open-source dependency vulnerabilities, enhancing overall security.

### Enabling One-Click Dependency Scanning for Your Repository

To configure this feature, users need the "Advanced Security: manage settings" permission. Here are the steps:

1. **Navigate to Repository Settings:**
   - Go to **Project settings** > **Repositories** > Select your repository.

2. **Setting up for Standalone Products:**
   - Enable Code Security.
   - In **Options**, confirm the selection of **Dependency alerts default setup**.
   
   ![Advanced Security repository options for Code Security plan](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2025/05/2025-05-28-15_48_55-advsec-repository-settings-code-security-options.png)

3. **Bundled Advanced Security:**
   - Use the checkbox to enable **Scan default branch for vulnerable dependencies**.
   
   ![Advanced Security repository enablement options](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2025/05/2025-02-24-14_20_55-Clipboard.png)

## How Dependency Scanning Works

- On your next pipeline run targeting the default branch, the Advanced Security dependency scanning task is automatically appended near the pipeline's end. Scanning typically completes within a few minutes.
- For teams without consistent CI/CD activity, scheduled pipeline runs are recommended to maintain consistent scanning coverage.

### Flexible Exclusions

- If dependency scanning is already part of your pipeline or you have set the variable `DependencyScanning.Skip: true`, the automated task will be skipped. This is useful for pipelines that shouldn't be included in the scanning surface area.
- To skip automated scanning on specific pipeline jobs, set the pipeline variable `dependencyScanningInjectionEnabled` to `false`.

## Reviewing and Acting on Scan Results

- Upon completion, scan results are uploaded to Advanced Security and made available under **Repos** > **Advanced Security**.

  ![Advanced Security dependency scanning alerts in repository](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2025/05/advsec-alerts-dependencies.png)

- You can configure pull request annotations for dependency scanning, provided a build validation policy is set up. This injects scanning into all PRs targeting your default branch.
  - **Annotations for new findings** are surfaced directly in the pull request (after scanning the default branch at least once).
  - Existing issues shared between branches display in the Advanced Security tab.

## Next Steps and Further Learning

- Teams are encouraged to try this feature and provide feedback directly or via [Developer Community](https://developercommunity.visualstudio.com/AzureDevOps).
- Additional information can be found:
  - [Configure GitHub Advanced Security features](https://learn.microsoft.com/azure/devops/repos/security/configure-github-advanced-security-features)
  - [GitHub Advanced Security Dependency Scanning](https://learn.microsoft.com/azure/devops/repos/security/github-advanced-security-dependency-scanning?view=azure-devops)

*This post originally appeared on the [Azure DevOps Blog](https://devblogs.microsoft.com/devops/automate-your-open-source-dependency-scanning-with-advanced-security/).*

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/automate-your-open-source-dependency-scanning-with-advanced-security/)
