---
external_url: https://github.blog/changelog/2025-12-16-track-organization-copilot-usage
title: Track Organization Copilot Usage with Copilot Usage APIs
author: Allison
feed_name: The GitHub Blog
date: 2025-12-16 15:08:56 +00:00
tags:
- Admin Tools
- AI Controls
- API Metrics
- Copilot
- Copilot Usage API
- Enterprise Management
- Feature Adoption
- Organization Settings
- Permissions
- Policy Settings
- Reporting
- REST API
- Usage Statistics
- User Engagement
section_names:
- ai
- github-copilot
---
Allison outlines how organization and enterprise owners can enable and access Copilot usage metrics using GitHub's Copilot usage APIs, providing actionable guidance for managing and monitoring Copilot adoption.<!--excerpt_end-->

# Track Organization Copilot Usage with Copilot Usage APIs

GitHub organizations can now access detailed Copilot usage metrics by leveraging the [Copilot usage APIs](https://docs.github.com/en/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28#get-copilot-organization-usage-metrics-for-a-specific-day).

## What You Get

- **Aggregated metrics:** Overview of total Copilot activity across the organization
- **User-specific metrics:** Engagement and usage statistics broken down by user
- **Feature adoption:** Insight into which Copilot features are being used

## Enabling Usage Metrics

Before accessing the data, the **Copilot usage metrics** policy must be enabled:

### For Enterprise Accounts

1. Go to the [Enterprises page](https://github.com/settings/enterprises).
2. Select your enterprise.
3. Click on the **AI Controls** tab.
4. In the left sidebar, select **Copilot**, then scroll to **Metrics**.
5. Select **Enabled everywhere**.

### For Organization Accounts

1. Visit the [Organization page](https://github.com/settings/organizations).
2. Select your organization.
3. Click on the **Settings** tab.
4. In the left sidebar, select **Copilot** > **Policies**.
5. Scroll down to **Features** > **Copilot usage metrics**.
6. Select **Enabled**.

## Who Can Access the API?

- Organization owners
- Users with a custom role that includes the `View Organization Copilot Metrics` permission

## Getting Started

- Refer to the official [Copilot usage metrics API documentation](https://docs.github.com/en/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28#get-copilot-organization-usage-metrics-for-a-specific-day) for details on endpoints and available fields.

By following these steps, administrators and custom role users can monitor Copilot’s impact and feature adoption within their teams, supporting data-driven decisions about AI tool usage.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-16-track-organization-copilot-usage)
