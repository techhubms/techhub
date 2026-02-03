---
external_url: https://github.blog/changelog/2026-01-29-arm64-standard-runners-are-now-available-in-private-repositories
title: 'GitHub Actions: Linux and Windows arm64 Runners Now Supported in Private Repositories'
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-01-29 16:00:13 +00:00
tags:
- Actions
- Arm64
- CI/CD
- Cloud Automation
- Continuous Integration
- Developer Tools
- DevOps
- GitHub Actions
- GitHub Runners
- Improvement
- Linux
- Multi Architecture
- News
- Open Source
- Private Repositories
- Windows
- Workflow Automation
section_names:
- devops
---
Allison details the addition of Linux and Windows arm64 standard runners for private repositories in GitHub Actions, outlining how development teams can benefit from enhanced performance and native multi-architecture support.<!--excerpt_end-->

# GitHub Actions: Linux and Windows arm64 Standard Runners for Private Repositories

GitHub now supports arm64 standard GitHub-hosted runners on both Linux and Windows for private repositories. This update lets you use free-tier eligible arm64 runners across all repository types, allowing teams to leverage the performance benefits of arm64 processors and run native multi-architecture builds without relying on virtualization or emulation.

## What's New

- **New Runner Support:** Linux and Windows arm64 standard hosted runners are now available in all repositories, particularly private ones.
- **Resource Allocation:** These runners offer two vCPUs for private repositories (four vCPUs for public repositories) and use images managed by Arm, LLC.
- **Free Tier Eligibility:** As standard GitHub-hosted runners, your usage will count toward the free minutes included in your GitHub plan.
- **Production Ready:** Suitable for production CI workloads, these runners enable performance-focused development and deployment pipelines, especially for containerized services and multi-architecture builds.

## Using arm64 Runners in Workflows

To start utilizing arm64 runners, include one of the following labels in the `runs-on` field of your workflow YAML:

- `windows-11-arm`
- `ubuntu-24.04-arm`
- `ubuntu-22.04-arm`

Refer to the [documentation for standard GitHub-hosted runners](https://docs.github.com/actions/reference/runners/github-hosted-runners#standard-github-hosted-runners-for--private-repositories) for more details on set-up and best practices.

> **Note:** macOS arm64 runners have also been available for private repositories, with this update extending support to Linux and Windows.

## Additional Resources

- [GitHub Blog Announcement](https://github.blog/changelog/2026-01-29-arm64-standard-runners-are-now-available-in-private-repositories)
- [GitHub Actions Documentation](https://docs.github.com/actions/reference/runners/github-hosted-runners)
- [Join the Discussion: GitHub Community](https://github.com/orgs/community/discussions/185840)

This development is ideal for teams targeting arm64 production environments who seek native performance and streamlined CI/CD workflows directly on GitHub.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-29-arm64-standard-runners-are-now-available-in-private-repositories)
