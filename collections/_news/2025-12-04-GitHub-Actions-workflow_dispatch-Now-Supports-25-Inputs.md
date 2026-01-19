---
external_url: https://github.blog/changelog/2025-12-04-actions-workflow-dispatch-workflows-now-support-25-inputs
title: GitHub Actions workflow_dispatch Now Supports 25 Inputs
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-12-04 19:58:47 +00:00
tags:
- Actions
- Automation
- CI/CD
- Continuous Integration
- GitHub
- GitHub Actions
- Improvement
- Workflow API
- Workflow Dispatch
- Workflow Inputs
- Workflow Trigger
section_names:
- devops
---
Allison announces that GitHub Actions workflows triggered by workflow_dispatch now support up to 25 input parameters, resolving previous limitations and improving workflow flexibility for the DevOps community.<!--excerpt_end-->

# GitHub Actions workflow_dispatch Now Supports 25 Inputs

Developers and DevOps engineers using GitHub Actions can now define up to 25 input parameters for workflows triggered by the `workflow_dispatch` event.

Previously, the input limit was 10, which often forced users to employ workarounds like combining multiple parameters into a single JSON string. This update eliminates those constraints, enabling more granular and readable configuration of manually or API-triggered workflows.

## What’s Changed

- **Increased Input Limit:** Maximum inputs for `workflow_dispatch` triggers raised from 10 to 25.
- **No More Workarounds:** Teams no longer need to consolidate data into a single parameter; each required input can be defined separately.
- **Manual and API Flexibility:** The new limit applies to workflows started both manually through the GitHub UI and via API calls, helping teams automate more complex scenarios with clearer intent.

## Example Use Case

A deployment pipeline can now accept separate parameters for environment, version, feature flags, and other options—up to 25—making workflows easier to maintain and debug.

See the official [GitHub Blog announcement](https://github.blog/changelog/2025-12-04-actions-workflow-dispatch-workflows-now-support-25-inputs) for more details and feedback from the developer community.

---

*Originally posted by Allison on The GitHub Blog.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-04-actions-workflow-dispatch-workflows-now-support-25-inputs)
