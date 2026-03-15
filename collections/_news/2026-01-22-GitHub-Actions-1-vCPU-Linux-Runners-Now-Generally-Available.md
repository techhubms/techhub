---
external_url: https://github.blog/changelog/2026-01-22-1-vcpu-linux-runner-now-generally-available-in-github-actions
title: GitHub Actions 1 vCPU Linux Runners Now Generally Available
author: Allison
feed_name: The GitHub Blog
date: 2026-01-22 17:00:00 +00:00
tags:
- Actions
- Automation
- Basic Compilation
- CI/CD
- Cloud Runners
- Containerization
- Continuous Integration
- DevOps Release
- GitHub Actions
- Issue Labeling
- Job Scheduling
- Linux Runner
- Resource Optimization
- Scripting
- Ubuntu Slim
- Workflow Automation
- DevOps
- News
section_names:
- devops
primary_section: devops
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
