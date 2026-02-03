---
external_url: https://github.blog/changelog/2026-01-30-docker-and-docker-compose-version-upgrades-on-hosted-runners
title: Docker and Docker Compose Upgrades on GitHub Hosted Runners
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-01-30 14:20:34 +00:00
tags:
- Actions
- CI/CD
- Deprecation
- DevOps
- Docker
- Docker Compose
- GitHub Actions
- Improvement
- News
- Runner Images
- Ubuntu
- Version Upgrade
- Windows
- Workflow Automation
section_names:
- devops
---
Allison explains important upcoming Docker and Docker Compose upgrades on GitHub hosted runners, outlining new versions, potential workflow changes, and links to more information.<!--excerpt_end-->

# Docker and Docker Compose Upgrades on GitHub Hosted Runners

**Author:** Allison  
**Original Source:** [The GitHub Blog](https://github.blog/changelog/2026-01-30-docker-and-docker-compose-version-upgrades-on-hosted-runners)

Docker and Docker Compose will be updated on all Windows and Ubuntu GitHub hosted runner images (excluding `ubuntu-slim`) on **February 9th, 2026**. This means workflows that rely on these tools will begin running with newer versions automatically, allowing teams to leverage the latest features, improvements, and security patches.

## New Versions

- **Docker Engine:** v29.1
- **Docker Compose:** v2.40
- *(or later minor versions if newer releases are available before the update date)*

## Action Items for Developers

- Review your existing CI/CD workflows using Docker, especially if you depend on [Docker functionality scheduled for removal in v29](https://docs.docker.com/engine/deprecated/).
- Update scripts and configurations as needed to ensure ongoing compatibility.

## Keeping Informed

GitHub regularly updates runner images to improve reliability, performance, and security. For future changes and image deprecations, follow the [runner-images repository announcements](https://github.com/actions/runner-images/labels/Announcement) and check regularly to stay ahead of any actions required for your projects.

## Summary

Staying proactive with these updates ensures your automated build and deployment pipelines remain stable and secure. This change only affects Windows and Ubuntu hosted runners (excluding `ubuntu-slim`).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-30-docker-and-docker-compose-version-upgrades-on-hosted-runners)
