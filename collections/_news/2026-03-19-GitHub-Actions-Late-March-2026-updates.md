---
tags:
- Actions
- CI/CD
- Cron
- Custom Protection Rules
- Deployment Controls
- Deployment Protection Rules
- 'Deployment: False'
- DevOps
- Environments
- GitHub Actions
- IANA Time Zones
- Improvement
- News
- Repository Variables
- Secrets Management
- UTC
- Workflow Scheduling
- Workflow Syntax
section_names:
- devops
title: 'GitHub Actions: Late March 2026 updates'
external_url: https://github.blog/changelog/2026-03-19-github-actions-late-march-2026-updates
date: 2026-03-19 20:41:58 +00:00
feed_name: The GitHub Blog
author: Allison
primary_section: devops
---

Allison summarizes late March 2026 GitHub Actions updates, covering timezone-aware scheduled workflows and a new way to use environments for secrets/variables without automatically creating deployments.<!--excerpt_end-->

# GitHub Actions: Late March 2026 updates

This month’s GitHub Actions update focuses on a couple of long-standing workflow “papercuts”:

- Using **environments** for secrets/variables without forcing an automatic **deployment**
- Adding **timezone support** to **scheduled (cron) workflows**

## Use environments without auto-deployment

GitHub Actions now supports referencing an environment in a workflow **without automatically creating a deployment**.

Why this matters:

- Some teams want **environment-scoped secrets and variables** (and related controls) without producing extra deployment records.

### How it works

Configure the environment usage with:

- `deployment: false`

With this set, you can access the environment but **won’t create a new deployment**.

Documentation: [Using environments without deployments](https://docs.github.com/actions/how-tos/deploy/configure-and-manage-deployments/control-deployments#using-environments-without-deployments)

### Important limitation

If you use a **custom deployment protection rule**, you **cannot** use `deployment: false`. In that case, using environments will still require an auto-deploy.

Documentation: [Create custom protection rules](https://docs.github.com/actions/how-tos/deploy/configure-and-manage-deployments/create-custom-protection-rules)

## Timezone support for scheduled workflows

Scheduled workflows are no longer locked to UTC.

You can now specify an **IANA timezone** alongside your cron schedule so that the workflow runs at the **local time** you intend.

Example (as described in the update):

- Add a `timezone` field next to your cron expression
- e.g., `timezone: "America/New_York"`

Documentation: [Workflow schedule syntax](https://docs.github.com/actions/reference/workflows-and-actions/workflow-syntax#onschedule)

## Discussion

GitHub is directing discussion to GitHub Community announcements:

- GitHub Community: https://github.com/orgs/community/discussions/categories/announcements


[Read the entire article](https://github.blog/changelog/2026-03-19-github-actions-late-march-2026-updates)

