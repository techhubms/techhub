---
author: Allison
tags:
- Application Security
- Bypass Requests
- Code Security
- DevOps
- DevSecOps
- Enterprise Security Configuration
- GitHub Advanced Security
- GitHub Apps
- GitHub Secret Scanning
- GitHub Teams
- Improvement
- News
- Organization Security Settings
- Push Protection
- Push Protection Exemptions
- Repository Security
- Roles And Permissions
- Secret Detection
- Secure Development Lifecycle
- Security
title: Push protection exemptions for roles, teams, and apps
feed_name: The GitHub Blog
section_names:
- devops
- security
date: 2026-03-17 16:00:33 +00:00
external_url: https://github.blog/changelog/2026-03-17-push-protection-exemptions-for-apps-teams-and-roles
primary_section: devops
---

Allison shares a GitHub update: organizations using secret scanning push protection can now exempt specific roles, teams, and apps from enforcement, with exemption checks evaluated on every push.<!--excerpt_end-->

# Push protection exemptions for roles, teams, and apps

Organizations using **secret scanning push protection** can now configure **exemptions** for specific:

- Roles
- Teams
- Apps

## How exemption evaluation works

- **Exemption status is evaluated at the time of each push.**
- If an **exempt actor** pushes content that contains secrets:
  - **Push protection is skipped**
  - **No bypass requests are created**

## Where you can configure exemptions

Exemptions can be configured using **security configurations** at:

- The **organization** level
- The **enterprise** level

## References

- Learn more about secret scanning: https://docs.github.com/code-security/concepts/secret-security/about-secret-scanning
- Learn more about push protection exemptions: https://docs.github.com/code-security/secret-scanning/using-advanced-secret-scanning-and-push-protection-features/push-protection-for-repositories-and-organizations


[Read the entire article](https://github.blog/changelog/2026-03-17-push-protection-exemptions-for-apps-teams-and-roles)

