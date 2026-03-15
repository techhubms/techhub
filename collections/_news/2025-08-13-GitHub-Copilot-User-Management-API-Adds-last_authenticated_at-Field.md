---
external_url: https://github.blog/changelog/2025-08-13-added-last_authenticated_at-to-the-copilot-user-management-api
title: GitHub Copilot User Management API Adds last_authenticated_at Field
author: Allison
feed_name: The GitHub Blog
date: 2025-08-13 21:51:19 +00:00
tags:
- Admin Tools
- API Schema
- API Update
- Authentication
- Copilot API
- Feature Parity
- GitHub REST API
- Last Authenticated At
- User Management
- AI
- GitHub Copilot
- News
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Allison reports on a new enhancement to the GitHub Copilot user management API, introducing a last_authenticated_at timestamp for tracking user authentication directly through the API.<!--excerpt_end-->

# GitHub Copilot User Management API Adds last_authenticated_at Field

GitHub has updated the [Copilot user management API](https://docs.github.com/rest/copilot/copilot-user-management?apiVersion=2022-11-28#list-all-copilot-seat-assignments-for-an-organization) to include the `last_authenticated_at` field. This new attribute provides the UTC timestamp reflecting the most recent authentication event for a Copilot user.

Previously, organizations could only retrieve this information from the activity report CSV export, which often made tracking real-time user activity cumbersome. By exposing `last_authenticated_at` directly through the API, GitHub delivers feature parity and simplifies programmatic access to user authentication data. Importantly, the update does not change the existing API schema in any breaking way.

**Key Points:**

- `last_authenticated_at` is now available via the Copilot user management API
- Returns the UTC timestamp of each user’s most recent authentication
- Previously only accessible in the activity report CSV
- The API update brings greater convenience for administrators managing large Copilot deployments
- No breaking changes were introduced

For more details, visit the [official changelog post](https://github.blog/changelog/2025-08-13-added-last_authenticated_at-to-the-copilot-user-management-api).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-13-added-last_authenticated_at-to-the-copilot-user-management-api)
