---
section_names:
- ai
- github-copilot
external_url: https://github.blog/changelog/2026-04-15-enable-copilot-cloud-agent-via-custom-properties
feed_name: The GitHub Blog
author: Allison
title: Enable Copilot cloud agent via custom properties
date: 2026-04-15 18:30:28 +00:00
tags:
- Access Control
- AI
- AI Controls
- API Endpoints
- Copilot
- Copilot Cloud Agent
- Enterprise Administration
- Enterprise Management Tools
- Feature Adoption
- GitHub Copilot
- GitHub Enterprise Cloud
- Improvement
- News
- Organization Custom Properties
- Organization Policies
- Pilot Rollout
- Policy Management
- REST API
primary_section: github-copilot
---

Allison explains a new GitHub Copilot Cloud Agent (CCA) admin capability: enterprise admins can now enable the agent for selected organizations (including via organization custom properties), and manage the policy through the AI Controls page or new REST API endpoints.<!--excerpt_end-->

# Enable Copilot cloud agent via custom properties

You can now selectively enable **GitHub Copilot cloud agent (CCA)** access on a **per-organization** basis.

Previously, enterprise admins and AI managers could only:

- Enable the agent everywhere
- Disable it everywhere
- Let each organization decide

With this release, you can enable CCA for specific organizations:

- Individually
- Or by using **organization custom properties** (see: https://docs.github.com/enterprise-cloud@latest/admin/managing-accounts-and-repositories/managing-organizations-in-your-enterprise/managing-custom-properties-for-organizations)

You can manage the policy setting:

- Using the **new API endpoints**
- Or directly in the **AI Controls** page

## Important note about custom property evaluation

Using custom properties to enable CCA is **evaluated once at the time of configuration**.

Organizations will **not** be automatically enabled or disabled for CCA if the custom property is later:

- Added
- Removed
- Modified

## New API endpoints to manage CCA

You can use three new API endpoints to manage CCA:

- **`PUT`**: Set the policy state:
  - decided by organization
  - enabled everywhere
  - disabled everywhere
  - enabled for selected organizations
- **`POST`**: Add organizations to the CCA enabled list
- **`DELETE`**: Disable CCA for organizations

REST API documentation:

- https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-coding-agent-management?apiVersion=2026-03-10

## Manage CCA from the AI Controls page

In the **AI Controls** settings page, you can create policies that selectively enable CCA for a subset of your organizations.

- No action is required to preserve your current policy.

Navigation path:

- **AI Controls** → **Agent** → **Copilot Cloud Agent** → **Enabled for selected organizations**

Documentation:

- https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-for-enterprise/manage-agents/manage-copilot-cloud-agent

## Discussion

- GitHub Community discussion: https://github.com/orgs/community/discussions/178247


[Read the entire article](https://github.blog/changelog/2026-04-15-enable-copilot-cloud-agent-via-custom-properties)

