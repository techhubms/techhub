---
layout: post
title: 'GitHub Billing API Updates: Programmatic Budget Management and Usage Tracking'
author: Allison
canonical_url: https://github.blog/changelog/2025-11-03-manage-budgets-and-track-usage-with-new-billing-api-updates
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-11-03 18:24:44 +00:00
permalink: /devops/news/GitHub-Billing-API-Updates-Programmatic-Budget-Management-and-Usage-Tracking
tags:
- Account Management
- Account Usage
- API Integration
- Billing API
- Budget Management
- Cost Center
- Enterprise Management
- Enterprise Management Tools
- GitHub
- Programmatic Access
- REST API
- Usage Tracking
section_names:
- devops
---
Allison covers GitHub’s latest billing API enhancements, showing how programmatic budget management and usage tracking open new possibilities for enterprise, team, and personal plan users.<!--excerpt_end-->

# GitHub Billing API Updates: Programmatic Budget Management and Usage Tracking

GitHub has expanded its billing APIs, giving users a new set of tools to programmatically manage budgets, track usage, and access cost center data.

## Manage Budgets via APIs

- **New API endpoints** let you create, update, and delete budgets through code rather than the UI.
- Adjust budget amounts and set up or edit alert notifications directly from your applications or scripts.
- API usage is currently limited to 50 budgets per account during the public preview.
- [REST API documentation for budgets](https://gh.io/budget-api-docs) provides further details on usage and integration.

## Track Usage with the Usage Summary API

- Retrieve usage information at the account, organization, repository, or cost center level.
- Filter data by year, month, or day for precise tracking and reporting.
- The Usage summary API is now in public preview; documentation is available [here](https://gh.io/usage-summary-api).

## Additional Improvements

- The [Get all cost centers for an enterprise API](https://docs.github.com/enterprise-cloud@latest/rest/enterprise-admin/billing?apiVersion=2022-11-28#get-all-cost-centers-for-an-enterprise) now accepts an optional `state` parameter to request active cost centers only (`?state=active`).
- The billing usage report API has dropped support for hourly granularity and now provides daily totals when using the `day` parameter. See more in the [API documentation](https://docs.github.com/enterprise-cloud@latest/rest/enterprise-admin/billing?apiVersion=2022-11-28#get-billing-usage-report-for-an-enterprise).

## Who Can Use These Features?

- Enterprise owners and billing managers on GitHub Enterprise plans
- Organization owners on GitHub Team plans
- Individuals with personal GitHub accounts

For feedback or questions, join the [GitHub Community discussion](https://gh.io/billing-api-community-feedback).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-03-manage-budgets-and-track-usage-with-new-billing-api-updates)
