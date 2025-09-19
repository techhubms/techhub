---
layout: "post"
title: "Retirement of GitHub Actions macOS 13 Runner Image Announced"
description: "This announcement informs users that the macOS 13 runner image for GitHub Actions will be officially retired by December 4th, 2025. It details key brownout periods intended to alert users, specifies the deprecated runner labels, and offers guidance on migration to supported ARM64 and Intel runner options, as well as future plans for discontinuing Intel-based runners on macOS platforms."
author: "Larissa Fortuna"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-19-github-actions-macos-13-runner-image-is-closing-down"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-19 16:21:39 +00:00
permalink: "/2025-09-19-Retirement-of-GitHub-Actions-macOS-13-Runner-Image-Announced.html"
categories: ["DevOps"]
tags: ["Actions", "Apple Silicon", "Arm64", "CI/CD", "Deprecation", "DevOps", "GitHub Actions", "Intel Architecture", "Macos 13", "Macos 15", "News", "Retired", "Runner Image", "Workflow Migration", "X86 64"]
tags_normalized: ["actions", "apple silicon", "arm64", "cislashcd", "deprecation", "devops", "github actions", "intel architecture", "macos 13", "macos 15", "news", "retired", "runner image", "workflow migration", "x86 64"]
---

Larissa Fortuna outlines the deprecation timeline for the GitHub Actions macOS 13 runner image, including migration steps and upcoming support changes for Intel and Apple Silicon architectures.<!--excerpt_end-->

# Retirement of GitHub Actions macOS 13 Runner Image

**Author:** Larissa Fortuna

## Overview

GitHub has announced the retirement of the macOS 13 runner image in GitHub Actions, effective December 4th, 2025. To raise awareness among users, jobs utilizing the `macos-13` image will temporarily fail during several scheduled brownout time periods throughout November 2025:

- **November 4, 14:00 UTC – November 5, 00:00 UTC**
- **November 11, 14:00 UTC – November 12, 00:00 UTC**
- **November 18, 14:00 UTC – November 19, 00:00 UTC**
- **November 25, 14:00 UTC – November 26, 00:00 UTC**

### Deprecated Labels

- `macos-13`
- `macos-13-large`
- `macos-13-xlarge`

## Migration Recommendations

- **If your workflow is architecture agnostic**, consider moving to any of the ARM64-based runner labels:
  - `macos-15`, `macos-latest`, `macos-14`, `macos-14-xlarge`, `macos-latest-xlarge`, or `macos-15-xlarge`
- **If you require x86\_64 (Intel) architecture**, migrate to:
  - `macos-15-intel` (new), `macos-14-large`, `macos-latest-large`, or `macos-15-large`

For migration details and label updates, visit the [runner images repository](https://github.com/actions/runner-images/labels/Announcement).

## Future Deprecation: macOS Intel Support

Apple has ended support for x86\_64 (Intel) architecture. GitHub Actions will end support for Intel runners on macOS after the retirement of the macOS 15 runner image in Fall 2027. Users are encouraged to transition workloads to ARM64-based (Apple Silicon) runners proactively to ensure workflow compatibility.

## Key Takeaways

- **Plan Migration**: Update your workflows to use supported runner images well before the deadlines.
- **Future-proofing**: Prepare for the eventual discontinuation of Intel support in GitHub Actions on macOS platforms.

For more information, see the official announcement and track label updates in the [runner images repository](https://github.com/actions/runner-images/labels/Announcement).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-19-github-actions-macos-13-runner-image-is-closing-down)
