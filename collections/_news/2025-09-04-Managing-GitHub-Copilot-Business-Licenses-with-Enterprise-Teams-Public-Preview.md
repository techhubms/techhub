---
layout: post
title: Managing GitHub Copilot Business Licenses with Enterprise Teams (Public Preview)
author: Allison
canonical_url: https://github.blog/changelog/2025-09-04-manage-copilot-and-users-via-enterprise-teams-in-public-preview
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-09-04 16:59:13 +00:00
permalink: /github-copilot/news/Managing-GitHub-Copilot-Business-Licenses-with-Enterprise-Teams-Public-Preview
tags:
- Admin Tools
- AI
- Copilot Business
- DevOps
- Enterprise Managed Users
- Enterprise Teams
- GitHub Copilot
- GitHub Enterprise Cloud
- Group Management
- Identity Provider
- License Management
- News
- Public Preview
- SCIM
- User Management
section_names:
- ai
- devops
- github-copilot
---
Allison explains the new public preview of Enterprise Teams in GitHub Enterprise Cloud, highlighting how enterprises can streamline user and Copilot Business license management at scale.<!--excerpt_end-->

# Managing GitHub Copilot Business Licenses with Enterprise Teams (Public Preview)

GitHub Enterprise Cloud has introduced **Enterprise Teams** in public preview, enhancing the way enterprises manage users and Copilot Business licenses. This update is especially relevant for organizations with large numbers of users and complex access requirements.

## Key Features

- **Direct User Addition**: Enterprise owners can add users to the enterprise without requiring them to first join an organization. These "unaffiliated" users do not consume a license by default, which streamlines onboarding and offboarding.

- **Enterprise Teams & Groups**: Admins can create and manage enterprise-level teams or groups, improving the organization of users within the enterprise account.

- **SCIM Synchronization**: Enterprises with Enterprise Managed Users (EMU) can sync user groups from their identity provider directly to GitHub Enterprise Teams via the SCIM protocol.

- **Flexible Copilot Business License Management**: Licenses for Copilot Business can now be managed and assigned at the enterprise team or individual user level using a dedicated Copilot licensing page. Assignment no longer requires the user to have a GitHub Enterprise Cloud license, allowing broader distribution of Copilot Business.

## Upcoming Enhancements

Future updates will allow:

- Assignment of enterprise roles to teams
- Assigning teams to multiple organizations within an enterprise to grant access automatically
- Delegation capabilities for organization and repository owners to adjust team roles

## User Offboarding Considerations

- Users removed from all organizations will remain as unaffiliated users unless explicitly removed from the enterprise.
- Enterprises should update their offboarding processes to account for these new flows and ensure compliance.

## Documentation & Resources

- [Enterprise Teams Documentation](https://docs.github.com/enterprise-cloud@latest/admin/managing-accounts-and-repositories/managing-users-in-your-enterprise/create-enterprise-teams)
- [Managing Unaffiliated Users](https://docs.github.com/enterprise-cloud@latest/admin/managing-accounts-and-repositories/managing-users-in-your-enterprise/invite-users-directly)
- [Copilot Business Licensing](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-for-enterprise/manage-access/grant-access)
- [Feature Discussion](https://github.com/orgs/community/discussions/171425)

*Disclaimer: Features in public preview are subject to UI changes. For the most current information, refer to the GitHub documentation links above.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-04-manage-copilot-and-users-via-enterprise-teams-in-public-preview)
