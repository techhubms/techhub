---
layout: "post"
title: "GitHub Actions 1 vCPU Linux Runners Now Generally Available"
description: "This announcement covers the general availability of 1 vCPU Linux runners for GitHub Actions. It explains their technical specs, optimal use cases like automation tasks or lightweight builds, and how to configure them, providing resources for workflow optimization and cost management."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-01-22-1-vcpu-linux-runner-now-generally-available-in-github-actions"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-01-22 17:00:00 +00:00
permalink: "/2026-01-22-GitHub-Actions-1-vCPU-Linux-Runners-Now-Generally-Available.html"
categories: ["DevOps"]
tags: ["Actions", "Automation", "Basic Compilation", "CI/CD", "Cloud Runners", "Containerization", "Continuous Integration", "DevOps", "DevOps Release", "GitHub Actions", "Issue Labeling", "Job Scheduling", "Linux Runner", "News", "Resource Optimization", "Scripting", "Ubuntu Slim", "Workflow Automation"]
tags_normalized: ["actions", "automation", "basic compilation", "cislashcd", "cloud runners", "containerization", "continuous integration", "devops", "devops release", "github actions", "issue labeling", "job scheduling", "linux runner", "news", "resource optimization", "scripting", "ubuntu slim", "workflow automation"]
---

Allison announces the general availability of 1 vCPU Linux runners in GitHub Actions, detailing their functionality, use cases for automation, and integration steps for DevOps workflows.<!--excerpt_end-->

# GitHub Actions 1 vCPU Linux Runner Now Generally Available

GitHub Actions has introduced general availability for 1 vCPU Linux runners, providing a lower-cost option for automating tasks within workflows.

## Runner Details

- **Specs:** 1 vCPU, 5 GB RAM
- **Execution Environment:** Workflows run in containers (not dedicated VMs), offering hypervisor level 2 isolation
- **Job Limit:** Maximum 15 minutes execution per job; longer jobs are terminated
- **Automatic Teardown:** Containers are decommissioned on job completion

## Recommended Use Cases

These lightweight runners are best for:

- Auto-labeling issues
- Basic language compilation (such as webpack)
- Linting and formatting
- External API calls
- Simple Python scripting

## How to Use

To leverage these runners:

- Specify the `ubuntu-slim` runner type in your workflow job definitions
- This enables cost-effective execution for appropriate jobs

## Resources

- [Runner-images repository: ubuntu-slim readme](https://github.com/actions/runner-images/blob/main/images/ubuntu-slim/ubuntu-slim-Readme.md) – Detailed software lists and troubleshooting
- [GitHub Actions billing information](https://docs.github.com/billing/managing-billing-for-your-products/managing-billing-for-github-actions/about-billing-for-github-actions)
- [Single-CPU runners documentation](https://docs.github.com/actions/reference/runners/github-hosted-runners#single-cpu-runners)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-22-1-vcpu-linux-runner-now-generally-available-in-github-actions)
