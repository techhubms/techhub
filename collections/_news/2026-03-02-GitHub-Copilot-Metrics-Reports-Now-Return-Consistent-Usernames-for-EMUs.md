---
layout: "post"
title: "GitHub Copilot Metrics Reports Now Return Consistent Usernames for EMUs"
description: "This update explains that GitHub Copilot usage metrics reports have improved consistency for Enterprise Managed Users (EMU) by providing uniform user_login values. Previously, user_login values could include suffixes, complicating data analysis across GitHub APIs. This enhancement simplifies enterprise Copilot monitoring and reporting processes."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-03-02-copilot-metrics-reports-now-return-consistent-usernames-for-enterprise-managed-users"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-03-02 16:42:49 +00:00
permalink: "/2026-03-02-GitHub-Copilot-Metrics-Reports-Now-Return-Consistent-Usernames-for-EMUs.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Account Management", "AI", "Copilot", "Copilot Metrics", "Developer Tools", "Enterprise Cloud", "Enterprise Managed Users", "Enterprise Management Tools", "Enterprise Reporting", "GitHub API", "GitHub Copilot", "Improvement", "News", "Usage Metrics", "User Login"]
tags_normalized: ["account management", "ai", "copilot", "copilot metrics", "developer tools", "enterprise cloud", "enterprise managed users", "enterprise management tools", "enterprise reporting", "github api", "github copilot", "improvement", "news", "usage metrics", "user login"]
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
