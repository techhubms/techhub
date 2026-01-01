---
layout: "post"
title: "GitHub Enterprise Server 3.18 Release Overview"
description: "This article summarizes the major new features in GitHub Enterprise Server (GHES) 3.18. Topics include improved deployment efficiency, advanced monitoring options with OpenTelemetry, enhanced code security through better scanning and alert tracking, expanded policy management, updates to issue tracking and project management, and new enterprise-level administration features. The content is tailored for technical readers, such as enterprise DevOps engineers and administrators using or considering GitHub Enterprise Server to manage software development at scale."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-10-14-github-enterprise-server-3-18-is-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-10-14 17:28:22 +00:00
permalink: "/2025-10-14-GitHub-Enterprise-Server-318-Release-Overview.html"
categories: ["DevOps", "Security"]
tags: ["Advanced Security", "Code Security", "Collaboration Tools", "Custom Properties", "Dependabot", "Deployment", "DevOps", "GHES 3.18", "GitHub Enterprise Server", "Issue Tracking", "Monitoring", "News", "OpenTelemetry", "Policy Management", "Project Management", "Prometheus", "Pull Request Rules", "Security"]
tags_normalized: ["advanced security", "code security", "collaboration tools", "custom properties", "dependabot", "deployment", "devops", "ghes 3dot18", "github enterprise server", "issue tracking", "monitoring", "news", "opentelemetry", "policy management", "project management", "prometheus", "pull request rules", "security"]
---

Allison reviews the key updates in GitHub Enterprise Server 3.18, highlighting new tools for deployment, monitoring, security, and project management for enterprise teams.<!--excerpt_end-->

# GitHub Enterprise Server 3.18 Release Overview

**Author:** Allison

GitHub Enterprise Server (GHES) 3.18 introduces significant improvements for teams managing enterprise-scale software development and security. Below are the main highlights of this release:

## Key Improvements

- **Enterprise Administration:**
  - Define custom properties on enterprise accounts to facilitate metadata organization.
  - Set enterprise-level rulesets and pull request merge strategies for consistent governance across repositories.
  - [More info on enterprise rulesets and custom properties.](https://github.blog/changelog/2025-03-24-enterprise-custom-properties-enterprise-rulesets-and-pull-request-merge-method-rule-are-all-now-generally-available/)

- **Project & Issue Management:**
  - Organize and classify issues with custom issue types and sub-issues for improved tracking.
  - Leverage advanced search (supporting `AND`, `OR`, and parentheses) for better querying.
  - Projects now scale up to 50,000 managed items (previous limit: 1,200).
  - Updated Issues dashboard (`*.com/issues`) for easier discovery and saved queries. [Details on evolving GitHub Issues.](https://github.blog/changelog/2025-04-09-evolving-github-issues-and-projects/), [Saved views documentation.](https://github.blog/changelog/2025-05-15-saved-views-on-the-issues-dashboard/)

- **Security Enhancements:**
  - Improved code scanning alerts now include a “Development” section to visualize when security issues are introduced, resolved, or reintroduced. This helps track resolution lifecycles and support better governance.
  - [Learn more about code scanning alert tracking.](https://github.blog/changelog/2025-05-02-track-progress-on-code-scanning-alerts-with-the-new-development-section/)

- **Dependabot Scaling:**
  - Organization administrators can now grant Dependabot access to repositories at the organization level, with Advanced Security customers able to enable access permanently organization-wide. An improved UI supports point-in-time access configuration.
  - [Details on Dependabot access management.](https://github.blog/changelog/2025-06-03-its-now-easier-to-grant-dependabot-access-to-repositories-from-the-organization-level/)

- **Monitoring & Observability:**
  - Operators can monitor GHES appliances using **OpenTelemetry** metrics (preview, preproduction only) and export metrics to Prometheus and third-party systems. The previous collectd metrics will be deprecated in a future update, so migration is recommended. [OpenTelemetry metrics usage.](https://docs.github.com/en/enterprise-server@3.18/admin/monitoring-and-managing-your-instance/monitoring-your-instance/opentelemetry-metrics/about-opentelemetry-metrics)

## Additional Resources

- [Full GHES 3.18 release notes](https://docs.github.com/enterprise-server@3.18/admin/release-notes)
- [Download the release](https://enterprise.github.com/releases/3.18.0/download)
- [Contact GitHub support for assistance](https://support.github.com/features/enterprise-administrators-server)
- [Join the community discussion](https://github.com/orgs/community/discussions/176826) to share feedback and questions

---

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-14-github-enterprise-server-3-18-is-now-generally-available)
