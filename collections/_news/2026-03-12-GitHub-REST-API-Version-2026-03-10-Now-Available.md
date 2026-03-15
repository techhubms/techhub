---
external_url: https://github.blog/changelog/2026-03-12-rest-api-version-2026-03-10-is-now-available
title: GitHub REST API Version 2026-03-10 Now Available
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-03-12 16:00:07 +00:00
tags:
- API Documentation
- API Upgrade
- API Versioning
- Breaking Changes
- Calendar Versioning
- Developer Integration
- Developer Tools
- DevOps
- Ecosystem & Accessibility
- GitHub
- News
- Release Notes
- REST API
- Software Maintenance
- X GitHub API Version
section_names:
- devops
---
Allison shares the release of GitHub REST API version 2026-03-10, highlighting breaking changes, upgrade guidance, and future support timelines for integrators.<!--excerpt_end-->

# GitHub REST API Version 2026-03-10 Is Now Available

GitHub has released version `2026-03-10` of its REST API, marking the first calendar-based version to include breaking changes. This update supports the ongoing evolution of the API while providing developers with clear upgrade paths and extended support timelines for existing integrations.

## Highlights of This Release

- **Breaking Changes:** Version `2026-03-10` introduces breaking changes to the API. Review the [breaking changes documentation](https://docs.github.com/rest/about-the-rest-api/breaking-changes?apiVersion=2026-03-10) for a comprehensive list and [upgrade guidance](https://docs.github.com/rest/about-the-rest-api/breaking-changes#upgrading-to-a-new-api-version).
- **Continued Support:** Version `2022-11-28` remains fully supported for at least 24 months. Integrations not specifying the `X-GitHub-Api-Version` header will default to `2022-11-28`.
- **Upgrade Process:** To upgrade:
  1. Review the new documentation and update integrations to accommodate breaking changes.
  2. Set the `X-GitHub-Api-Version` header to `2026-03-10`.
  3. Verify integration functionality with the new version.
- **Resource Access:** Use the [API documentation](https://docs.github.com/rest) version picker to compare available versions.

## Timeline and Communication

- Future API version announcements will continue via the GitHub changelog.
- Integrators are encouraged to adopt the new version for access to enhancements and ongoing improvements.

## Summary

This release gives developers a clear path for evolving their integrations while maintaining stability for existing applications. The documentation offers practical resources for managing breaking changes and staying current with GitHub's API offerings.

For more information, visit the [official GitHub Blog announcement](https://github.blog/changelog/2026-03-12-rest-api-version-2026-03-10-is-now-available).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-12-rest-api-version-2026-03-10-is-now-available)
