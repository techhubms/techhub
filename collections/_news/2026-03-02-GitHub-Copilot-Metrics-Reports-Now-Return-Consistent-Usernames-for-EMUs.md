---
external_url: https://github.blog/changelog/2026-03-02-copilot-metrics-reports-now-return-consistent-usernames-for-enterprise-managed-users
title: GitHub Copilot Metrics Reports Now Return Consistent Usernames for EMUs
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-03-02 16:42:49 +00:00
tags:
- Account Management
- AI
- Copilot
- Copilot Metrics
- Developer Tools
- Enterprise Cloud
- Enterprise Managed Users
- Enterprise Management Tools
- Enterprise Reporting
- GitHub API
- GitHub Copilot
- Improvement
- News
- Usage Metrics
- User Login
section_names:
- ai
- github-copilot
---
Allison details recent improvements to GitHub Copilot metrics reporting, ensuring consistent usernames for Enterprise Managed Users and simplifying data analysis for enterprise teams.<!--excerpt_end-->

# GitHub Copilot Metrics Reports Now Return Consistent Usernames for EMUs

GitHub Copilot usage metrics reports now provide a consistent `user_login` value for Enterprise Managed Users (EMU). Previously, reports could include a suffix in the `user_login` field, making it more challenging to match and analyze user data across various GitHub APIs.

With this update, all relevant Copilot metrics reports will use a standardized `user_login`, regardless of context. This change significantly improves the ease of data analysis, auditing, and reporting for organizations that manage large developer teams using GitHub Enterprise and Copilot.

**Key Points:**

- **Consistent Usernames:** All Copilot metrics for EMUs now have the same `user_login` value, eliminating discrepancies caused by username suffixes.
- **Simplified Reporting:** Data analysis and cross-referencing across GitHub APIs is now easier and more reliable.
- **Enterprise Focus:** This enhancement specifically benefits teams using GitHub Enterprise with Copilot, improving administrative and monitoring workflows.

For more information, see the [GitHub Copilot usage metrics REST API documentation](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28).

*Author: Allison*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-02-copilot-metrics-reports-now-return-consistent-usernames-for-enterprise-managed-users)
