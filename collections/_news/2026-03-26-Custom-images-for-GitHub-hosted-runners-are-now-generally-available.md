---
primary_section: devops
title: Custom images for GitHub-hosted runners are now generally available
tags:
- Actions
- Build Environment
- Certificates
- CI/CD
- Configuration Management
- Custom Images
- Dependency Management
- DevOps
- GitHub Actions
- GitHub Hosted Runners
- Governance
- Larger Runners
- News
- Operational Overhead
- Runner Images
- Standardization
- Troubleshooting
- Workflow Performance
external_url: https://github.blog/changelog/2026-03-26-custom-images-for-github-hosted-runners-are-now-generally-available
author: Allison
date: 2026-03-26 14:30:13 +00:00
feed_name: The GitHub Blog
section_names:
- devops
---

Allison announces general availability of custom images for GitHub-hosted runners, explaining how predefining tools, dependencies, certificates, and configuration can speed up and standardize GitHub Actions workflows across an organization.<!--excerpt_end-->

# Custom images for GitHub-hosted runners are now generally available

Custom images for GitHub-hosted runners are now generally available. Originally introduced in [public preview](https://github.blog/changelog/2025-10-28-custom-images-for-github-hosted-runners-are-now-available-in-public-preview/) in October 2025, this feature lets you start with a GitHub-curated base image and build your own virtual machine image tailored to your workflow needs.

## What custom images are for

Custom images let you define the runner environment up front by baking in:

- Preinstalled tools
- Dependencies
- Certificates
- Configuration

This is intended to help you:

- Make workflows faster and more consistent
- Reduce setup time in jobs
- Reduce operational overhead from repeated environment bootstrapping
- Improve security and governance by standardizing build environments at scale

## Upgrade notes

- If you used custom images during the public preview, **no action is required**.
- Existing images and workflows should continue working as-is.

## Documentation

For setup, configuration, and troubleshooting, GitHub points to:

- [Managing larger runners](https://docs.github.com/actions/how-tos/manage-runners/larger-runners/manage-larger-runners)
- [Using custom images](https://docs.github.com/actions/how-tos/manage-runners/larger-runners/use-custom-images)

These docs include step-by-step guides, workflow examples, and best practices for custom images with GitHub-hosted runners.


[Read the entire article](https://github.blog/changelog/2026-03-26-custom-images-for-github-hosted-runners-are-now-generally-available)

