---
external_url: https://github.blog/changelog/2026-01-12-controlling-who-can-request-apps-for-your-organization-is-now-generally-available
title: Granular Controls for App Access Requests in GitHub Organizations Now Available
author: Allison
feed_name: The GitHub Blog
date: 2026-01-12 20:19:15 +00:00
tags:
- Access Control
- App Access
- Collaboration
- Enterprise Governance
- Enterprise Management Tools
- GitHub
- Improvement
- Integration Approval
- OAuth
- Organization Management
- Security Policy
- Settings
section_names:
- devops
- security
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
