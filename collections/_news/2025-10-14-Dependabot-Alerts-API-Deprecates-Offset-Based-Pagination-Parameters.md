---
layout: "post"
title: "Dependabot Alerts API Deprecates Offset-Based Pagination Parameters"
description: "This news update details GitHub's retirement of offset-based pagination parameters in Dependabot alerts REST API endpoints. From now on, only cursor-based pagination is supported, which aligns Dependabot alerts with other GitHub security APIs. The change affects how developers retrieve large sets of security alerts, streamlining integrations and standardizing paging behavior. Key changes, affected endpoints, and the motivations for adopting cursor-based pagination are explained. Links to relevant documentation for cursor-based pagination and Dependabot alerts API usage are provided for further reading."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-10-14-dependabot-alerts-api-pagination-parameters-deprecated"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-10-14 14:38:56 +00:00
permalink: "/2025-10-14-Dependabot-Alerts-API-Deprecates-Offset-Based-Pagination-Parameters.html"
categories: ["DevOps", "Security"]
tags: ["API Changes", "API Documentation", "Cursor Based Pagination", "Dependabot", "DevOps", "DevOps Practices", "GitHub", "News", "Pagination", "REST API", "Retired", "Security", "Security Alerts", "Supply Chain Security"]
tags_normalized: ["api changes", "api documentation", "cursor based pagination", "dependabot", "devops", "devops practices", "github", "news", "pagination", "rest api", "retired", "security", "security alerts", "supply chain security"]
---

Allison explains GitHub's transition to cursor-based pagination for Dependabot alerts REST API endpoints, highlighting changes and implications for developers working with supply chain security.<!--excerpt_end-->

# Dependabot Alerts API Deprecates Offset-Based Pagination

GitHub has removed support for offset-based pagination parameters (`page`, `first`, `last`) in all GitHub Dependabot alerts REST API endpoints. Going forward, only cursor-based parameters (`before`, `after`, `per_page`) are supported when querying for Dependabot alerts.

## What's Changing

- **Offset-based parameters**: `page`, `first`, and `last` are no longer accepted when listing Dependabot alerts.
- **Cursor-based pagination**: Use `before`, `after`, and `per_page` instead for all relevant endpoints.
- **Affected endpoints**:
  - List Dependabot alerts for a repository
  - List Dependabot alerts for an organization
  - List Dependabot alerts for an enterprise

## Why This Change Was Made

Moving to cursor-based pagination brings Dependabot alerts in line with other GitHub security APIs, providing more reliable and performant data retrieval, especially for large datasets and enterprise integrations. Cursor-based paging avoids issues like data drift and inconsistent results common with offset-based approaches.

## How to Use Cursor-Based Pagination

- **before**: Fetches results that come before a specific cursor.
- **after**: Fetches results that come after a specific cursor.
- **per_page**: Sets the maximum number of items per page.

For full usage examples, see the following documentation:

- [Using pagination in the REST API](https://docs.github.com/rest/using-the-rest-api/using-pagination-in-the-rest-api?apiVersion=2022-11-28)
- [Dependabot Alerts API documentation](https://docs.github.com/rest/dependabot/alerts?apiVersion=2022-11-28#list-dependabot-alerts-for-a-repository)

## Developer Impact

- Update existing integrations to use the supported pagination parameters.
- Review and test endpoints for compatibility.
- Aligns with best practices for supply chain security and API consistency.

---
*Authored by Allison; this update originally appeared on [The GitHub Blog](https://github.blog/changelog/2025-10-14-dependabot-alerts-api-pagination-parameters-deprecated).*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-14-dependabot-alerts-api-pagination-parameters-deprecated)
