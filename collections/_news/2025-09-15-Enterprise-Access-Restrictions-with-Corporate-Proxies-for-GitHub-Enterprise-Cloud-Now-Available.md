---
layout: post
title: Enterprise Access Restrictions with Corporate Proxies for GitHub Enterprise Cloud Now Available
author: Allison
canonical_url: https://github.blog/changelog/2025-09-15-enterprise-access-restrictions-with-corporate-proxies-is-now-generally-available
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-09-15 20:49:25 +00:00
permalink: /devops/news/Enterprise-Access-Restrictions-with-Corporate-Proxies-for-GitHub-Enterprise-Cloud-Now-Available
tags:
- Access Control
- Authentication Security
- Copilot Network Policy
- Corporate Proxy
- Data Residency
- Enterprise Managed Users
- Enterprise Management Tools
- Firewall
- GitHub Enterprise Cloud
- Improvement
- Network Restrictions
- Platform Governance
- Proxy Configuration
section_names:
- devops
- security
---
Allison details how GitHub Enterprise Cloud accounts with Enterprise Managed Users can now restrict access to github.com traffic via corporate proxy configuration, strengthening compliance and security for organizational workflows.<!--excerpt_end-->

# Enterprise Access Restrictions with Corporate Proxies for GitHub Enterprise Cloud

GitHub has introduced enterprise access restrictions for GitHub Enterprise Cloud accounts with Enterprise Managed Users (EMU), now generally available to all customers. This feature enables highly regulated organizations to allow only EMU enterprise traffic to github.com through their designated corporate proxies, effectively blocking any unapproved access.

## Key Benefits

- **Stricter Network Access:** Limit github.com traffic exclusively to approved EMU users within your enterprise environment.
- **Reduced Risk:** Mitigates the chance of intentional or accidental data leaks by ensuring only managed, enterprise-sanctioned accounts access GitHub resources.
- **Regulatory Compliance:** Provides organizations in regulated industries a mechanism for defining secure network strategies aligned with compliance requirements.

## How to Enable Enterprise Access Restrictions

1. In GitHub Enterprise Cloud, navigate to your enterprise account's **Settings → Authentication security → 'Enterprise access restrictions'**.
2. Enable the feature by selecting the corresponding option.
3. Configure your network proxy or firewall to inject a custom header into all relevant web and API requests:
   
   ```http
   sec-GitHub-allowed-enterprise: ENTERPRISE-ID
   ```
   
   This header must carry your valid EMU enterprise ID. GitHub uses the presence and value of this header to determine if a request should be permitted.

## Scope and Integration

- **Access Coverage:** The restriction applies to both API and UI access to github.com, enforcing enterprise boundaries.
- **Copilot Traffic:** Copilot access is managed via a separate network policy that can control which Copilot edition (Enterprise, Business, or Individual) is permitted on the network. For details, see the [Copilot proxy configuration guide](https://docs.github.com/enterprise-cloud@latest/copilot/managing-copilot/managing-github-copilot-in-your-organization/configuring-your-proxy-server-or-firewall-for-copilot).
- **Multiple Enterprise IDs:** Support for including more than one EMU enterprise ID in the proxy header is currently in private preview for customers managing multiple enterprises.
- **Data Residency Needs:** GitHub Enterprise Cloud with data residency provides a unique GHE.com subdomain, which can be used for advanced network traffic differentiation and compliance.

## Additional Resources

- [Restricting access to github.com using a corporate proxy](https://docs.github.com/enterprise-cloud@latest/admin/configuring-settings/hardening-security-for-your-enterprise/restricting-access-to-githubcom-using-a-corporate-proxy)
- [GitHub Community Announcements](https://github.com/orgs/community/discussions/categories/announcements)
- [GitHub Enterprise Cloud with Data Residency](https://docs.github.com/enterprise-cloud@latest/admin/data-residency/about-github-enterprise-cloud-with-data-residency)

## Summary

This feature strengthens enterprise controls on GitHub by enforcing network-level restrictions and enabling organizations to meet stringent security and regulatory requirements. For broader rollout or private previews involving multiple enterprises, contact your account manager.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-15-enterprise-access-restrictions-with-corporate-proxies-is-now-generally-available)
