---
layout: "post"
title: "Copilot User Management API Now Includes last_authenticated_at Field"
description: "This news update outlines the addition of the last_authenticated_at field to the GitHub Copilot user management API. The enhancement enables developers and administrators to programmatically access the UTC timestamp of a user’s latest authentication, previously only available through activity report CSV exports. The change delivers feature parity and improves data accessibility without introducing breaking changes."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-13-added-last_authenticated_at-to-the-copilot-user-management-api"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-13 21:51:19 +00:00
permalink: "/2025-08-13-Copilot-User-Management-API-Now-Includes-last_authenticated_at-Field.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Access Management", "AI", "API Enhancement", "API Update", "Authentication Timestamp", "Copilot User Management API", "Feature Parity", "GitHub Copilot", "News", "REST API", "User Management"]
tags_normalized: ["access management", "ai", "api enhancement", "api update", "authentication timestamp", "copilot user management api", "feature parity", "github copilot", "news", "rest api", "user management"]
---

Allison reports on the addition of the last_authenticated_at field to the GitHub Copilot user management API, improving access to users' authentication timestamps for developers and administrators.<!--excerpt_end-->

# Copilot User Management API Now Includes last_authenticated_at Field

GitHub has updated the [Copilot user management API](https://docs.github.com/rest/copilot/copilot-user-management?apiVersion=2022-11-28#list-all-copilot-seat-assignments-for-an-organization) by adding a new field: `last_authenticated_at`. This field makes it possible to programmatically retrieve the UTC timestamp of a user's most recent authentication directly from the API.

Previously, this information was only accessible via the activity report CSV export, making real-time automation and integration scenarios more cumbersome. With this update, developers and administrators can:

- Access each Copilot user's last authentication time directly in API responses
- Avoid relying on manual or periodic CSV reports for this critical data
- Integrate authentication tracking into custom dashboards, monitoring solutions, and audit workflows
- Maintain existing integrations effortlessly, as the change is non-breaking and backward-compatible

**No breaking changes** have been introduced in the API schema, ensuring smooth adoption. For more details, visit the [GitHub Copilot user management API documentation](https://docs.github.com/rest/copilot/copilot-user-management?apiVersion=2022-11-28#list-all-copilot-seat-assignments-for-an-organization).

## Summary

- **Field added**: `last_authenticated_at`
- **Purpose**: Returns UTC timestamp of a user's latest authentication
- **Availability**: Now accessible directly via API
- **Schema impact**: Non-breaking change

This addition supports improved oversight of Copilot usage and aligns with best practices in account management and security.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-13-added-last_authenticated_at-to-the-copilot-user-management-api)
