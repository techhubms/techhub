---
layout: post
title: GitHub Enterprise Cloud Access Restrictions Now Support Multiple Enterprises
author: Usha Narayanabhatta
canonical_url: https://github.blog/changelog/2025-10-06-enterprise-access-restrictions-now-supports-multiple-enterprises
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-10-06 23:52:41 +00:00
permalink: /devops/news/GitHub-Enterprise-Cloud-Access-Restrictions-Now-Support-Multiple-Enterprises
tags:
- Access Restrictions
- Authentication Security
- Copilot Access
- Corporate Proxy
- DevOps
- Enterprise ID
- Enterprise Managed Users
- Enterprise Management Tools
- Firewall Configuration
- GitHub Enterprise Cloud
- Improvement
- Network Security
- News
- Platform Governance
- Proxy Header
- Security
section_names:
- devops
- security
---
Usha Narayanabhatta highlights the latest update to GitHub Enterprise Cloud: organizations with multiple EMU accounts can now use a single proxy header for unified access restriction and improved network security.<!--excerpt_end-->

# GitHub Enterprise Access Restrictions Now Support Multiple Enterprises

Author: Usha Narayanabhatta

GitHub has expanded the network security and access control options on GitHub Enterprise Cloud for organizations utilizing multiple Enterprise Managed Users (EMU) accounts. Previously, enterprises could only configure enterprise access restrictions through corporate proxies for a single EMU account by specifying its enterprise ID in the proxy header.

## What’s New

- **Unified Proxy Header**: Administrators can now include up to 20 different EMU enterprise IDs within a single proxy header to centrally manage access for all relevant business entities. This is particularly useful for companies with separate entities due to acquisitions, varied data classification strategies, or complex internal structures.
- **Improved Security Configuration**: This update ensures that all API, webpage (UI), and git requests to github.com from a corporate network are only permitted for accounts managed under the specified enterprise IDs.

## How to Enable Multi-Enterprise Restrictions

1. For every enterprise, activate the **Enable enterprise access restrictions** setting in the "Authentication security" → "Enterprise access restrictions" section within GitHub Enterprise Cloud.
2. On your organization's network, configure your proxy or firewall to add a header:

   ```
   sec-GitHub-allowed-enterprise: ENTERPRISE1-ID, ENTERPRISE2-ID, ..., ENTERPRISE20-ID
   ```

   This allows requests from any member of these enterprise accounts while blocking unauthorized access.

## Managing Copilot Access

- Copilot traffic is coordinated by a different network policy—administrators can restrict which Copilot edition (Enterprise, Business, or Individual) is permitted on the corporate network.
- Detailed documentation is available for [Configuring your proxy server or firewall for Copilot](https://docs.github.com/enterprise-cloud@latest/copilot/managing-copilot/managing-github-copilot-in-your-organization/configuring-your-proxy-server-or-firewall-for-copilot).

## Additional Recommendations

- For enterprises trialing EMU accounts or in early adoption phases, GitHub recommends exploring [GitHub Enterprise Cloud with data residency](https://docs.github.com/enterprise-cloud@latest/admin/data-residency/about-github-enterprise-cloud-with-data-residency). This feature provides a unique subdomain to better segregate and control enterprise-specific network traffic and can help address data residency requirements.

## Learn More

- Full documentation: [Restricting access to github.com using a corporate proxy](https://docs.github.com/enterprise-cloud@latest/admin/configuring-settings/hardening-security-for-your-enterprise/restricting-access-to-githubcom-using-a-corporate-proxy)
- Community discussion: [GitHub Community Announcements](https://github.com/orgs/community/discussions/categories/announcements)

This update empowers organizations to maintain robust access security while efficiently administering multiple enterprise entities within GitHub Enterprise Cloud.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-06-enterprise-access-restrictions-now-supports-multiple-enterprises)
