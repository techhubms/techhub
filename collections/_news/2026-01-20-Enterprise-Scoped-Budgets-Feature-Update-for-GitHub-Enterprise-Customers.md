---
external_url: https://github.blog/changelog/2026-01-19-enterprise-scoped-budgets-that-exclude-cost-center-usage-in-public-preview
title: Enterprise-Scoped Budgets Feature Update for GitHub Enterprise Customers
author: Allison
feed_name: The GitHub Blog
date: 2026-01-20 00:27:17 +00:00
tags:
- Audit Logs
- Budget Management
- Cloud Billing
- Cost Centers
- DevOps Tooling
- Enterprise Administration
- Enterprise Budgets
- Enterprise Management Tools
- GitHub Enterprise
- Improvement
- Metered Products
- REST API
- Spending Controls
- Spending Limits
section_names:
- devops
---
Allison shares news about a GitHub Enterprise feature: administrators can now configure enterprise-scoped budgets that exclude cost center usage, simplifying enterprise spending control and budgeting.<!--excerpt_end-->

# Enterprise-Scoped Budgets Excluding Cost Center Usage: GitHub Enterprise Update

GitHub Enterprise customers now have the ability to set up enterprise-scoped budgets that exclude cost center usage. This update streamlines financial management for organizations, allowing specific parts of the enterprise to consume additional services while maintaining controlled budgets for the rest.

## What Changed?

- **New Option**: Enterprises can now apply budgets at the enterprise level, excluding cost centers.
- **Selective Usage**: Administrators can allow certain areas of the enterprise to incur additional usage via cost centers, while imposing limits elsewhere.
- **Previous Model**: Before, enforcing default limits meant individually creating budgets for every area—this is no longer necessary.
- **API & Auditability**: The new setting is available through the REST API, and any changes are logged in the audit system for transparency.

> For full details on setup, see [GitHub's official documentation](https://docs.github.com/enterprise-cloud@latest/billing/tutorials/set-up-budgets).

## Key Details

- The exclusion of cost center usage is only available for new or existing enterprise-scoped budgets.
- Existing budgets maintain previous behavior (including cost center usage) unless updated.
- API support ensures integration with automation and enterprise management workflows.
- Full visibility is provided through audit logging for compliance and review.

## Benefits for Enterprises

- Simplifies budget management and enforcement
- Enables granular control over enterprise-wide spending
- Reduces administrative overhead for large organizations using cost centers

## Additional Resources

- [Public Preview Announcement](https://github.blog/changelog/2026-01-19-enterprise-scoped-budgets-that-exclude-cost-center-usage-in-public-preview)
- [Setting Up Budgets in GitHub Enterprise](https://docs.github.com/enterprise-cloud@latest/billing/tutorials/set-up-budgets)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-19-enterprise-scoped-budgets-that-exclude-cost-center-usage-in-public-preview)
