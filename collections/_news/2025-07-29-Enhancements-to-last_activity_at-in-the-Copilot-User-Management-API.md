---
layout: "post"
title: "Enhancements to last_activity_at in the Copilot User Management API"
description: "GitHub has improved the durability and reliability of the `last_activity_at` field in the Copilot User Management API. These updates increase resilience and scalability for activity tracking without introducing any breaking API changes. The refresh interval for activity metrics remains unchanged."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-07-29-enhancements-to-last_activity_at-in-the-user-management-api"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/label/copilot/feed/"
date: 2025-07-29 16:05:17 +00:00
permalink: "/2025-07-29-Enhancements-to-last_activity_at-in-the-Copilot-User-Management-API.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Activity Tracking", "AI", "API Durability", "API Updates", "Copilot API", "Feature Usage", "GitHub Copilot", "Last Activity At", "Metrics", "News", "Pipeline Resilience", "User Management"]
tags_normalized: ["activity tracking", "ai", "api durability", "api updates", "copilot api", "feature usage", "github copilot", "last activity at", "metrics", "news", "pipeline resilience", "user management"]
---

Allison outlines recent improvements to the `last_activity_at` field in the GitHub Copilot User Management API, increasing reliability in tracking user activity.<!--excerpt_end-->

## Enhancements to `last_activity_at` in the User Management API

**Author:** Allison

GitHub has released an update to the Copilot User Management API, specifically targeting the durability and reliability of the `last_activity_at` field. This change aims to offer more dependable activity tracking by improving the resilience and scalability of the underlying data pipeline. Importantly, these improvements come without any breaking changes to the existing API schema, so existing integrations will continue to function as before.

### Key Points

- **Resilience and Durability:** The pipeline managing the `last_activity_at` field has been enhanced to better withstand failures and scale as usage grows.
- **No Breaking Changes:** The API schema remains unchanged, ensuring a seamless transition for current API consumers.
- **Activity Tracking:** The `last_activity_at` field provides insight into user engagement with GitHub Copilot features, as specified in [GitHub's public documentation](https://docs.github.com/copilot/reference/metrics-data#included-features).
- **Refresh Interval:** The update does not change the refresh period for activity metrics; it still may take up to 30 minutes for new activity to be reflected in API responses.

For full details, see the original announcement on [The GitHub Blog](https://github.blog/changelog/2025-07-29-enhancements-to-last_activity_at-in-the-user-management-api).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-07-29-enhancements-to-last_activity_at-in-the-user-management-api)
