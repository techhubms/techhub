---
date: 2026-03-23 20:36:39 +00:00
feed_name: The GitHub Blog
author: Allison
title: Push protection exemptions from repository settings
external_url: https://github.blog/changelog/2026-03-23-push-protection-exemptions-from-repository-settings
tags:
- Application Security
- Bypass Requests
- Code Security
- DevOps
- Enterprise Security Policies
- GitHub Advanced Security
- GitHub Apps
- GitHub Secret Scanning
- GitHub Teams
- Improvement
- News
- Organization Security Policies
- Push Protection
- Push Protection Exemptions
- Repository Settings
- Roles And Permissions
- Secret Detection
- Security
- Security Configurations
- Security Enforcement
section_names:
- devops
- security
primary_section: devops
---

Allison explains a GitHub secret scanning update that lets you configure push protection exemptions directly in repository settings, expanding control beyond org/enterprise security configurations and clarifying what happens when exempt roles, teams, or apps push code that contains secrets.<!--excerpt_end-->

# Push protection exemptions from repository settings

Allison shares an update to GitHub secret scanning push protection: you can now configure **push protection exemptions** directly from **repository settings**. Previously, exemptions could only be managed through **organization** and **enterprise** security configurations.

## What changed

You can now designate secret scanning push protection exemptions from your repository settings.

- **Before**: Exemptions could only be managed at the **organization** and **enterprise** level via security configurations.
- **Now**: Exemptions can be managed at the **repository** level as well.

Source: [Push protection exemptions for apps, teams, and roles](https://github.blog/changelog/2026-03-16-push-protection-exemptions-for-apps-teams-and-roles/)

## What are push protection exemptions?

For organizations using **secret scanning push protection**, you can mark specific actors as exempt from push protection enforcement:

- **Roles**
- **Teams**
- **Apps**

### How evaluation works

- Exemption status is checked **at the time of each push**.
- If an **exempt** actor pushes content that contains secrets:
  - **Push protection is skipped**
  - **No bypass requests are created**

## Documentation

- Secret scanning overview: https://docs.github.com/code-security/concepts/secret-security/about-secret-scanning
- Push protection exemptions docs: https://docs.github.com/code-security/secret-scanning/using-advanced-secret-scanning-and-push-protection-features/push-protection-for-repositories-and-organizations


[Read the entire article](https://github.blog/changelog/2026-03-23-push-protection-exemptions-from-repository-settings)

