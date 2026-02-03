---
external_url: https://github.blog/changelog/2026-01-29-closing-down-notice-of-legacy-copilot-metrics-apis
title: Legacy GitHub Copilot Metrics APIs to Sunset in 2026
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-01-29 15:57:01 +00:00
tags:
- Account Management
- AI
- API Deprecation
- API Migration
- API Sunset
- Code Completion
- Copilot
- Copilot Metrics
- Developer Tooling
- Enterprise Management Tools
- Enterprise Tools
- GitHub Copilot
- IDE Telemetry
- Metrics API
- News
- Retired
- Usage Metrics
- User Adoption
section_names:
- ai
- github-copilot
---
Allison details the planned deprecation of legacy Copilot metrics APIs, urging developers and enterprise users to migrate to new usage metrics endpoints that offer deeper and more granular insights into Copilot adoption.<!--excerpt_end-->

# Legacy GitHub Copilot Metrics APIs to Sunset in 2026

GitHub is phasing out three legacy Copilot metrics APIs as it transitions to newer, more comprehensive usage metrics endpoints. The affected APIs include:

- **User-level Feature Engagement Metrics API** (sunsetting March 2, 2026)
- **Direct Data Access API** (sunsetting March 2, 2026)
- **Copilot Metrics API** (sunsetting April 2, 2026)

Support for these APIs will be limited, and no new features will be introduced. Users are strongly encouraged to update their workflows and migrate to the [latest Copilot usage metrics endpoints](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28) to avoid disruptions.

## Overview of Legacy APIs

- **User-level Feature Engagement Metrics API:** Provides simplified, binary indicators of user engagement with specific Copilot features.
- **Direct Data Access API:** Supplies user-level events regarding Copilot code completion activities within supported IDEs.
- **Copilot Metrics API:** Delivers aggregate usage metrics, especially related to IDE usage.

## Reasons for Deprecation

The new Copilot metrics APIs provide:

- A unified and authoritative source for Copilot adoption and usage data
- More granular insights, including agent, model, language, and code line metrics
- Reduced support overhead and improved product maintainability

## Recommendations for Developers and Enterprises

- Transition any existing integrations to the [new Copilot usage metrics APIs](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28), which support enterprise, organization, and user-level data.
- Leverage expanded metrics such as:
  - Edit mode and agent usage
  - Suggested vs. accepted code lines
  - Programming language and AI model breakdowns
  - New usage dimensions unavailable in legacy endpoints

The updated APIs are now in public preview and will be generally available soon.

## Additional Resources

- [User-level Feature Engagement Metrics API Documentation](https://docs.github.com/enterprise-cloud@latest/early-access/copilot/user-level-feature-engagement-metrics-api)
- [Direct Data Access API Documentation](https://docs.github.com/enterprise-cloud@latest/early-access/copilot/direct-data-access)
- [Copilot Metrics API Documentation](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-metrics?apiVersion=2022-11-28)
- [Latest Copilot Usage Metrics Endpoints](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-29-closing-down-notice-of-legacy-copilot-metrics-apis)
