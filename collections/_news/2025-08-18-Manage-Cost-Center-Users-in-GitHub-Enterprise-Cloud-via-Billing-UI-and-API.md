---
layout: post
title: Manage Cost Center Users in GitHub Enterprise Cloud via Billing UI and API
author: Allison
canonical_url: https://github.blog/changelog/2025-08-18-customers-can-now-add-users-to-a-cost-center-from-both-the-ui-and-api-2
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-08-18 15:01:58 +00:00
permalink: /devops/news/Manage-Cost-Center-Users-in-GitHub-Enterprise-Cloud-via-Billing-UI-and-API
tags:
- Access Control
- Admin Tools
- Billing UI
- Cloud Billing
- Cost Attribution
- Cost Centers
- DevOps
- Enterprise Admin
- GitHub Enterprise Cloud
- News
- Organization Admin
- REST API
- SaaS Management
- User Management
section_names:
- devops
---
Allison details how GitHub Enterprise Cloud now allows enterprise admins and billing managers to manage cost center memberships directly from the billing UI, simplifying user management tasks.<!--excerpt_end-->

# Manage Cost Center Users in GitHub Enterprise Cloud via Billing UI and API

GitHub Enterprise Cloud introduces an enhanced billing UI, designed to simplify the management of users within cost centers. Previously, this functionality required interaction with the REST API, but now enterprise admins and billing managers can make these changes directly through the user interface.

## What’s New

- Search for and select one or multiple users to add to a cost center
- View all users currently assigned to a cost center
- Remove users from a cost center with a single click

## How It Works

- **Roles:**
  - Enterprise admins and billing managers can manage users across all cost centers enterprise-wide.
  - Organization admins have visibility and management rights within their organizations.
- **User Assignment:**
  - Each user may belong to only one cost center at a time.
  - Attempting to add a user already assigned elsewhere will automatically move them to the newly selected cost center, with a notification alert.

## Availability

- This feature is available now for all GitHub Enterprise Cloud customers.
- For further details and step-by-step guides, visit the [GitHub documentation on cost centers](https://docs.github.com/enterprise-cloud@latest/billing/tutorials/use-cost-centers).

## Why This Matters

- Streamlines the workflow for cost attribution and tracking
- Reduces dependence on the REST API for routine user management tasks
- Provides visibility and control to both enterprise and organization administrators, enhancing administrative efficiency

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-18-customers-can-now-add-users-to-a-cost-center-from-both-the-ui-and-api-2)
