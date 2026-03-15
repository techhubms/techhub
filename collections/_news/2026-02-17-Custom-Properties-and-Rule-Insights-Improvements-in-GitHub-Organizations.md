---
external_url: https://github.blog/changelog/2026-02-17-custom-properties-and-rule-insights-improvements
title: Custom Properties and Rule Insights Improvements in GitHub Organizations
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-17 15:35:22 +00:00
tags:
- Branch Protection
- Commit SHA
- Custom Properties
- DevOps
- Enterprise Management
- Filtering
- GitHub
- Governance
- Improvement
- News
- Organization Administration
- Platform Governance
- Platform Improvements
- Repository Management
- Repository Policy
- Rule Insights
- Rulesets
section_names:
- devops
---
Allison explains recent GitHub improvements for organization administrators, allowing required explicit values for custom repository properties and enhanced rule insights filtering.<!--excerpt_end-->

# Custom Properties and Rule Insights Improvements in GitHub Organizations

## Overview

Two significant GitHub improvements have been released to help organization and enterprise administrators better manage repositories and enforce governance:

- **Custom properties now support required explicit values**
- **Rule insights feature enhanced filters, including commit SHA filtering**

---

## 1. Custom Properties Require Explicit Values

Administrators can now enforce that repository creators select explicit values for custom properties during repository creation, instead of using default values. This improves the reliability of organizational policy enforcement and ensures property data is accurate and complete from the start.

> In the previous behavior, properties could have default values, but now with the "Require explicit user-specified values" option, each required property must be set deliberately by the repository creator.

- The repository creation UI now marks such properties with an asterisk (*), indicating that a choice must be made for each required property.
- This enhances adoption of rulesets across organizations by making compliance mandatory at creation time.

**Reference:** [Managing custom properties documentation](https://docs.github.com/organizations/managing-organization-settings/managing-custom-properties-for-repositories-in-your-organization)

---

## 2. Rule Insights Page with Dynamic SHA Filtering

GitHub has improved the rule insights experience by replacing dropdown filters with a dynamic filter bar. The new bar allows filtering by commit SHA on repository-level rule insights pages.

- This makes it easier to audit and understand how particular commits interacted with established rulesets.
- Filtering provides more granular visibility and investigation tools for administrators and security teams.

**Reference:** [Managing rulesets for a repository documentation](https://docs.github.com/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/managing-rulesets-for-a-repository)

---

## Community and Further Discussion

You can join related discussions in the [GitHub Community](https://github.com/orgs/community/discussions/categories/repositories).

---

*Improved by Allison on the GitHub Blog, these updates help enforce repository policy compliance and provide better investigative tools for GitHub organizations.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-17-custom-properties-and-rule-insights-improvements)
