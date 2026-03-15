---
external_url: https://github.blog/changelog/2025-12-22-control-who-can-request-apps-for-your-organization
title: Granular Controls for GitHub App Requests Now in Public Preview
author: Allison
feed_name: The GitHub Blog
date: 2025-12-22 15:02:08 +00:00
tags:
- Access Management
- App Access Requests
- App Governance
- Enterprise Management Tools
- Enterprise Security
- GHES 3.21
- GitHub
- Governance Policy
- Improvement
- Member Privileges
- OAuth Apps
- Organization Settings
- DevOps
- Security
- News
section_names:
- devops
- security
primary_section: devops
---
Allison details new organizational controls for GitHub and OAuth app requests, giving admins more flexibility and security through graduated policy options for managing third-party app access.<!--excerpt_end-->

# Granular Controls for GitHub App Requests Now in Public Preview

Organizations now benefit from enhanced control over who can request GitHub Apps and OAuth apps. This update introduces more nuanced settings, allowing organizations to implement stricter governance policies without sacrificing operational flexibility.

## What's Changed

- **New options introduced for app request permissions:**
  - **Members and outside collaborators**: Both can request apps (existing default behavior).
  - **Members only**: Only organization members can request, blocking outside collaborators.
  - **Disable app access requests**: Prevents both members and outside collaborators from making any app requests.
- **Configuration path:**
  - Navigate to your organization’s settings
  - Select **Member Privileges**
  - Choose your desired option under **App access requests**
- **Purpose:**
  - Ensures all third-party apps undergo appropriate organizational review and security vetting before installation.

![Three options within an organization's settings page to choose who can request GitHub or OAuth apps](https://github.com/user-attachments/assets/086f553d-0dec-4f8a-aa6a-e513d6384c66)

## Reference

- Official documentation: [Limiting app requests](https://docs.github.com/organizations/managing-programmatic-access-to-your-organization/limiting-oauth-app-and-github-app-access-requests)
- Feature will be included in **GitHub Enterprise Server (GHES) 3.21**
- For questions and feedback, visit the [GitHub Community](https://github.com/orgs/community/discussions)

## Key Benefits

- Improved security posture via stricter app request permissions
- Greater administrative flexibility for organizations
- Clear governance options to fit enterprise policy requirements

---

For further implementation details or to provide feedback, consult the linked official documentation and community resources.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-22-control-who-can-request-apps-for-your-organization)
