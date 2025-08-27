---
layout: "post"
title: "Upcoming changes to GitHub Events API payloads"
description: "This update details key improvements to the GitHub Events API, including reduced payload sizes, faster event availability, and removal of certain fields for performance gains. The article outlines changes to event types, implications for integrators, rollout timelines, and guidance for developers relying on the affected fields."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-08-upcoming-changes-to-github-events-api-payloads"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-08 17:41:32 +00:00
permalink: "/2025-08-08-Upcoming-changes-to-GitHub-Events-API-payloads.html"
categories: ["DevOps"]
tags: ["API Changes", "API Migration", "API Performance", "Community Announcements", "Developer Integrations", "DevOps", "Event Delivery", "Feature Rollout", "GitHub", "GitHub Events API", "News", "Payload Optimization", "Pull Request Events", "Push Events", "REST API", "Scalability", "Webhooks"]
tags_normalized: ["api changes", "api migration", "api performance", "community announcements", "developer integrations", "devops", "event delivery", "feature rollout", "github", "github events api", "news", "payload optimization", "pull request events", "push events", "rest api", "scalability", "webhooks"]
---

Allison provides an overview of upcoming changes to the GitHub Events API, detailing improvements in performance and recommendations for developers who rely on this API.<!--excerpt_end-->

# Upcoming changes to GitHub Events API payloads

## Summary

GitHub is improving the Events API to deliver fresher, more scalable event data. Key updates include smaller payloads by removing non-essential fields and making events available almost immediately, which improves performance and scalability for developers integrating with GitHub systems.

## What’s Changing

- **Discussion events**: Developers will now be able to query events from discussion activities just as they do for pull requests and other event types.
- **Smaller payloads**: Several fields—especially from pull request and push events—will be removed. Developers needing detailed information can still obtain it via the main REST API using provided identifiers.
- **Author association fields**: The `author_association` field will be dropped from several event types (Pull Request, Pull Request Review, Pull Request Review Comment, Issue, Issue Comment, Commit Comment, and Discussion) to help speed up response times.

## Why These Changes Matter

- **Lower latency and fresher data**: Events, which previously could appear in the API up to eight hours late, will now be available almost in real time.
- **Greater scalability**: The API's reduced payload size and database calls help scale as developer usage increases.
- **No data loss**: All data remains accessible via the standard REST API for those who need it.

## Timeline

- **September 8, 2025**: Initial brownout test of the changes.
- **October 7, 2025**: Official implementation of the new payload structures.

## Developer Guidance

Most integrations and applications won’t need to change. Developers relying on removed fields should adapt to retrieve data through the REST API as needed. Reviewing existing integrations is recommended to confirm there is no reliance on deprecated attributes.

## Rollout Process & Feedback

Changes will be rolled out using a feature flag for gradual adoption. Developers can share feedback or report missing data requirements, aiding GitHub in ensuring a smooth transition.

## Learn More

- [REST API Documentation](https://docs.github.com/rest/pulls/pulls?apiVersion=2022-11-28)
- [GitHub Community Announcements](https://github.com/orgs/community/discussions/categories/announcements)

GitHub continues to refine its APIs for better developer experience and platform growth.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-08-upcoming-changes-to-github-events-api-payloads)
