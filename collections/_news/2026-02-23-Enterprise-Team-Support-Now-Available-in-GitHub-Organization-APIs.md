---
external_url: https://github.blog/changelog/2026-02-23-enterprise-team-support-in-organization-apis
title: Enterprise Team Support Now Available in GitHub Organization APIs
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-23 16:25:07 +00:00
tags:
- Access Control
- API Enhancement
- DevOps
- Enterprise Administration
- Enterprise Management Tools
- Enterprise Teams
- GitHub Enterprise
- Improvement
- News
- Organization API
- Permission Management
- Public Preview
- Repository Rulesets
- REST API
- Team Endpoints
- Team Management
section_names:
- devops
---
Allison reports on GitHub Enterprise's new API enhancements, enabling organization and enterprise owners to manage and query both enterprise teams and organization teams efficiently from a unified set of endpoints.<!--excerpt_end-->

# Enterprise Team Support Now Available in GitHub Organization APIs

GitHub Enterprise is enhancing API capabilities by allowing organization owners to discover and interact with both enterprise-level and organization-level teams using the same API endpoints. Previously, obtaining a consolidated view of teams required multiple queries and varying levels of admin permissions. This update simplifies the process, reducing complexity for organizations with large or distributed team structures.

## Key Enhancements

- **Unified Endpoints**: Several organization API endpoints, including those for Repository Rulesets, Teams, Team Members, and Organization Repository Rulesets, now support both enterprise and organization teams.
- **Easier Filtering**: The “List Teams” API endpoint lets users filter results by team type, making it easier to find the teams relevant to them.
- **Consistent Team Representation**: Teams are consistently formatted, clearly distinguishing between enterprise-level and organization-level teams.
- **Domain-Specific Views**: Organization owners view teams scoped to their domain, whereas enterprise owners have visibility across the entire enterprise.
- **Error Handling**: If an invalid enterprise team input is provided, the API returns an error message to maintain data integrity.

## Impact for Developers and Admins

These improvements streamline team management, particularly for large organizations utilizing GitHub Enterprise. With clearer API responses and easier access to comprehensive team data, administrators can better automate permission management and governance processes.

Enterprise Teams is currently in public preview and subject to additional changes based on community feedback. Users are encouraged to share their experiences in [this GitHub discussion](https://github.com/orgs/community/discussions/177040).

## Helpful Links

- [Repository rulesets documentation](https://docs.github.com/enterprise-cloud@latest/rest/repos/rules?apiVersion=2022-11-28)
- [Teams API endpoints](https://docs.github.com/enterprise-cloud@latest/rest/teams/teams?apiVersion=2022-11-28)
- [Announcement on GitHub Blog](https://github.blog/changelog/2026-02-23-enterprise-team-support-in-organization-apis)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-23-enterprise-team-support-in-organization-apis)
