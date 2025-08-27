---
layout: "post"
title: "GitHub Copilot User Management API Adds last_authenticated_at Field"
description: "This news update covers the addition of the last_authenticated_at field to the GitHub Copilot user management API. The new field allows administrators to access the UTC timestamp of a user's most recent authentication directly through the API. This change aligns the API's feature set with the previous CSV activity report export, helping organizations better track and manage Copilot usage."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-13-added-last_authenticated_at-to-the-copilot-user-management-api"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-13 21:51:19 +00:00
permalink: "/2025-08-13-GitHub-Copilot-User-Management-API-Adds-last_authenticated_at-Field.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Admin Tools", "AI", "API Schema", "API Update", "Authentication", "Copilot API", "Feature Parity", "GitHub Copilot", "GitHub REST API", "Last Authenticated At", "News", "User Management"]
tags_normalized: ["admin tools", "ai", "api schema", "api update", "authentication", "copilot api", "feature parity", "github copilot", "github rest api", "last authenticated at", "news", "user management"]
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
