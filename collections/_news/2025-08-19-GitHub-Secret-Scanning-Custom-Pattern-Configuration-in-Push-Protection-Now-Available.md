---
layout: post
title: 'GitHub Secret Scanning: Custom Pattern Configuration in Push Protection Now Available'
author: Allison
canonical_url: https://github.blog/changelog/2025-08-19-secret-scanning-configuring-patterns-in-push-protection-is-now-generally-available
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-08-19 21:49:26 +00:00
permalink: /devops/news/GitHub-Secret-Scanning-Custom-Pattern-Configuration-in-Push-Protection-Now-Available
tags:
- Advanced Security
- Configuration Management
- Enterprise Security
- False Positive Management
- GitHub
- Organization Security
- Push Protection
- REST API
- Secret Protection
- Secret Scanning
- Security Policy
section_names:
- devops
- security
---
Allison reports that GitHub Secret Protection now allows organizations to configure secret scanning patterns used in push protection, supporting stronger, customizable security policies.<!--excerpt_end-->

# GitHub Secret Scanning: Custom Pattern Configuration in Push Protection Now Available

GitHub Secret Protection has introduced the ability to configure which secret scanning patterns are included in push protection. This feature is now generally available, enabling organizations to better align push protection with their unique security policies and reduce the risk of sensitive secrets being committed to repositories.

## Key Features

- **Customizable Patterns:** Administrators can now select which secret scanning patterns are enforced as part of push protection at both the Enterprise and Organization levels.
- **Configuration Scope:**
  - **Enterprise Level:** Navigate to *Settings → Advanced Security → Additional Settings* to manage patterns globally for the entire organization.
  - **Organization Level:** Use *Settings → Advanced Security → Global settings*. Organization settings inherit Enterprise configurations but can override them for finer control.
  - Pattern configurations currently apply globally and are not yet available at the individual repository level.
- **User Interface Updates:** The configuration UI now provides a secondary tab for custom pattern management, and presents data including alert volumes, false positive rates, and bypass rates to help inform configuration decisions.
- **Defaults and Overrides:** Patterns are initialized with GitHub’s recommended settings by default. Administrators can adjust settings to match organization standards.
- **API Access:** Configuration can also be managed programmatically via the [REST API](https://docs.github.com/rest/secret-scanning/push-protection?apiVersion=2022-11-28).

## Benefits

- **Enhanced Security:** Prevents disclosure of sensitive credentials and secrets by blocking commits that match unsafe patterns.
- **Organizational Alignment:** Custom patterns support compliance with internal policies.
- **Dashboard Metrics:** Data-driven insights on false positives and bypasses help balance security posture and development productivity.

## Further Reading

- [Secret scanning overview](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/about-secret-scanning)
- [Push protection guide](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/about-push-protection)

---

For organizations seeking advanced control of source code security, this feature is a significant enhancement to GitHub's enterprise security tooling.

*Author: Allison*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-19-secret-scanning-configuring-patterns-in-push-protection-is-now-generally-available)
