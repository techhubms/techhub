---
title: GitHub Actions workflows are limited to 50 reruns
tags:
- Actions
- Build Pipelines
- Check Suites
- CI/CD
- DevOps
- GitHub Actions
- GitHub Actions Limits
- GitHub Changelog
- Job Reruns
- News
- Retired
- Retry Automation
- System Load
- Workflow Annotations
- Workflow Limits
- Workflow Reruns
feed_name: The GitHub Blog
author: Allison
primary_section: devops
section_names:
- devops
date: 2026-04-10 20:03:29 +00:00
external_url: https://github.blog/changelog/2026-04-10-actions-workflows-are-limited-to-50-reruns
---

Allison announces a new GitHub Actions limit: each workflow run can only be rerun up to 50 times (including partial job reruns), after which GitHub will return a failed check suite with an annotation explaining the limit.<!--excerpt_end-->

# GitHub Actions workflows are limited to 50 reruns

GitHub Actions workflows are now limited to **50 reruns** per workflow run.

## What changed

- If you try to rerun a given workflow **more than 50 times**, GitHub will return a **failed check suite**.
- The failure will include an **annotation** indicating that the rerun limit has been reached.
- The limit counts:
  - Full workflow reruns
  - Reruns of a **subset of jobs**

## Why this limit was added

GitHub introduced the limit in response to automations that attempt **hundreds of retries** on a single workflow run, which adds additional load to the system.

## Where to find more details

- GitHub Actions limits documentation: https://docs.github.com/actions/reference/limits


[Read the entire article](https://github.blog/changelog/2026-04-10-actions-workflows-are-limited-to-50-reruns)

