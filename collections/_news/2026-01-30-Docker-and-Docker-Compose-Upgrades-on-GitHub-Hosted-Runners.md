---
layout: "post"
title: "Docker and Docker Compose Upgrades on GitHub Hosted Runners"
description: "This news update announces that Docker and Docker Compose versions will be upgraded on all Windows and Ubuntu GitHub hosted runner images except for ubuntu-slim. It highlights the specific new versions, suggests workflow updates for deprecated features, and provides resources for staying informed on future changes."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-01-30-docker-and-docker-compose-version-upgrades-on-hosted-runners"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-01-30 14:20:34 +00:00
permalink: "/2026-01-30-Docker-and-Docker-Compose-Upgrades-on-GitHub-Hosted-Runners.html"
categories: ["DevOps"]
tags: ["Actions", "CI/CD", "Deprecation", "DevOps", "Docker", "Docker Compose", "GitHub Actions", "Improvement", "News", "Runner Images", "Ubuntu", "Version Upgrade", "Windows", "Workflow Automation"]
tags_normalized: ["actions", "cislashcd", "deprecation", "devops", "docker", "docker compose", "github actions", "improvement", "news", "runner images", "ubuntu", "version upgrade", "windows", "workflow automation"]
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
