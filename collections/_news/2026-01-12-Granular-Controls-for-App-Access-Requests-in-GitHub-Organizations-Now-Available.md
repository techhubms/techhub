---
layout: "post"
title: "Granular Controls for App Access Requests in GitHub Organizations Now Available"
description: "This update introduces new governance controls for organizations using GitHub, allowing administrators to precisely determine who can request OAuth and GitHub Apps. The feature, now generally available, supports stricter security policies and better management of third-party integrations."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-01-12-controlling-who-can-request-apps-for-your-organization-is-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-01-12 20:19:15 +00:00
permalink: "/2026-01-12-Granular-Controls-for-App-Access-Requests-in-GitHub-Organizations-Now-Available.html"
categories: ["DevOps", "Security"]
tags: ["Access Control", "App Access", "Collaboration", "DevOps", "Enterprise Governance", "Enterprise Management Tools", "GitHub", "Improvement", "Integration Approval", "News", "OAuth", "Organization Management", "Security", "Security Policy", "Settings"]
tags_normalized: ["access control", "app access", "collaboration", "devops", "enterprise governance", "enterprise management tools", "github", "improvement", "integration approval", "news", "oauth", "organization management", "security", "security policy", "settings"]
---

Allison explains new options for controlling who can request apps in GitHub organizations, enhancing security and governance for administrators managing enterprise workflows.<!--excerpt_end-->

# Granular Controls for App Access Requests in GitHub Organizations

Organizations can now implement more detailed policies for who is allowed to request GitHub Apps and OAuth apps. This new feature is aimed at improving security governance—administrators have the intelligence to permit or restrict app requests based on membership status. The options include:

- **Members and outside collaborators**: Both groups can request apps (default behavior).
- **Members only**: Only organization members may request apps.
- **Disable app access requests**: No one can request apps.

Configuration is found in the organization's settings under **Member Privileges** > "App access requests". This enables organizations to ensure only approved third-party integrations are considered, supporting security reviews and controlled deployments.

For more details, refer to the [Limiting app requests](https://docs.github.com/organizations/managing-programmatic-access-to-your-organization/limiting-oauth-app-and-github-app-access-requests) documentation. The change will be included in GitHub Enterprise Server (GHES) 3.21.

Feedback and questions are encouraged in the [Community discussion](https://github.com/orgs/community/discussions/183833).

## Key Points

- Introduces three app access request policies: both members and collaborators, members only, or none.
- Enhances organizational governance for security and operational workflows.
- Ensures all apps undergo proper security review before installation.
- Available in upcoming GHES 3.21.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-12-controlling-who-can-request-apps-for-your-organization-is-now-generally-available)
