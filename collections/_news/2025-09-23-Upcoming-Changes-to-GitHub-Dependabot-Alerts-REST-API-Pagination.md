---
layout: post
title: Upcoming Changes to GitHub Dependabot Alerts REST API Pagination
author: Allison
canonical_url: https://github.blog/changelog/2025-09-23-upcoming-changes-to-github-dependabot-alerts-rest-api-offset-based-pagination-parameters-page-first-and-last
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-09-23 15:35:24 +00:00
permalink: /devops/news/Upcoming-Changes-to-GitHub-Dependabot-Alerts-REST-API-Pagination
tags:
- API Changes
- API Documentation
- Automation
- Cursor Based Pagination
- GitHub Dependabot
- Integration
- Offset Based Pagination
- Pagination
- REST API
- Retired
- Security API
- Supply Chain Security
section_names:
- devops
- security
---
Allison announces the upcoming removal of offset-based pagination from GitHub Dependabot alerts REST API, guiding developers on migrating to cursor-based pagination methods for enhanced performance and consistency.<!--excerpt_end-->

# Upcoming Changes to GitHub Dependabot Alerts REST API Pagination

GitHub is deprecating offset-based pagination parameters (`page`, `first`, and `last`) for all Dependabot alerts REST API endpoints. Developers and integrators must switch to using only cursor-based pagination (`before`, `after`, and `per_page`), aligning Dependabot APIs with other GitHub security APIs and enhancing data retrieval reliability.

## What's Changing

- Offset-based pagination parameters will no longer be supported:
  - `page`
  - `first`
  - `last`
- This change affects the following endpoints:
  - List Dependabot alerts for a repository
  - List Dependabot alerts for an organization
  - List Dependabot alerts for an enterprise

## New Pagination Parameters

- Use these cursor-based parameters:
  - `before`
  - `after`
  - `per_page`

## Why This Matters

Cursor-based pagination offers greater consistency and performance advantages, simplifying how users and integrations handle large datasets across security APIs on GitHub.

## Timeline

- The deprecation becomes effective on **October 14, 2025**.
- Offset-based pagination will be removed on that date.
- Update all relevant scripts and integrations as soon as possible.

## Next Steps

- Review your current usage of GitHub Dependabot alerts REST API endpoints.
- Replace any use of `page`, `first`, or `last` with `before`, `after`, or `per_page`.
- Reference the following documentation for more details:
  - [GitHub REST API Pagination Documentation](https://docs.github.com/rest/using-the-rest-api/using-pagination-in-the-rest-api?apiVersion=2022-11-28)
  - [Dependabot Alerts API Documentation](https://docs.github.com/en/rest/dependabot/alerts?apiVersion=2022-11-28#list-dependabot-alerts-for-a-repository)

## Community and Support

- Join the ongoing discussion or ask questions in the [GitHub Community announcements forum](https://github.com/orgs/community/discussions/categories/announcements).

## About the Author

This update was provided by Allison via The GitHub Blog.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-23-upcoming-changes-to-github-dependabot-alerts-rest-api-offset-based-pagination-parameters-page-first-and-last)
