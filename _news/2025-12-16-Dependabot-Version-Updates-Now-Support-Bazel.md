---
layout: "post"
title: "Dependabot Version Updates Now Support Bazel"
description: "This announcement details the rollout of Bazel support for Dependabot, enabling automated updates for Bazel dependencies in projects using either the newer Bzlmod or legacy WORKSPACE systems. The post explains the technical collaboration between GitHub and the Bazel community, outlines requirements, and describes how projects can leverage automatic pull requests, lockfile regeneration, and reproducible build maintenance. The guide also highlights real-world developer steps and pointers to further documentation."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-bazel"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-12-16 16:45:50 +00:00
permalink: "/2025-12-16-Dependabot-Version-Updates-Now-Support-Bazel.html"
categories: ["DevOps"]
tags: ["Automation", "Bazel", "Bzlmod", "Continuous Integration", "Dependabot", "Dependency Management", "DevOps", "GitHub", "Lockfile", "News", "Open Source", "Supply Chain Security", "WORKSPACE"]
tags_normalized: ["automation", "bazel", "bzlmod", "continuous integration", "dependabot", "dependency management", "devops", "github", "lockfile", "news", "open source", "supply chain security", "workspace"]
---

Allison introduces Dependabot’s new capability to manage Bazel dependencies, highlighting collaboration with the Bazel community and outlining technical details for developers.<!--excerpt_end-->

# Dependabot Version Updates Now Support Bazel

Dependabot now supports automatic version updates for Bazel dependencies, giving developers the ability to keep dependencies up to date for both Bzlmod and legacy WORKSPACE-based projects.

## Background

Bazel projects rely on two main dependency systems—Bzlmod (using `MODULE.bazel` files) and WORKSPACE (legacy, but still common). Ensuring that dependencies are current and lockfiles are correctly generated is crucial for reproducible builds and secure software supply chains. Community requests guided improvements in support, particularly around:

- Proper lockfile generation (`MODULE.bazel.lock` files)
- Support for both `*.MODULE.bazel` and WORKSPACE dependency definitions

The GitHub and Bazel communities worked closely to deliver accurate support that minimizes risk across complex dependency graphs.

## Community Partnership

The Bazel community—especially Fabian Meumertzheim, Yun Peng, and Alex Eagle—were instrumental in providing:

- Deep lockfile semantics and compatibility advice
- Testing and validation data
- File naming conventions

Their contributions, along with wider community testing, helped ensure robust and reliable Dependabot support.

## How It Works

1. **Dependency Detection:** Dependabot analyzes your repository’s `MODULE.bazel`, `*.MODULE.bazel`, or WORKSPACE files to determine all Bazel dependencies, checking the Bazel central registry for updates.
2. **Lockfile Management:** When a change is detected, Dependabot regenerates lockfiles (`MODULE.bazel.lock`) to maintain a reproducible build.
3. **Automated Pull Requests:** Updates open as pull requests that include updated dependency declarations, revised lockfiles, and links to release notes and compatibility info.

## Getting Started

To enable Bazel support with Dependabot:

- Use Bazel version 7, 8, or 9
- Ensure a `MODULE.bazel` or WORKSPACE file exists at your repository root
- Engage with [the Dependabot open source community](https://github.com/dependabot/dependabot-core/issues/2196) for support
- Reference [Dependabot documentation](https://docs.github.com/code-security/dependabot/dependabot-version-updates/about-dependabot-version-updates) for setup details

Learn more about the Bazel build system at [bazel.build](https://bazel.build/).

## Additional Resources

- [Dependabot version updates documentation](https://docs.github.com/code-security/dependabot/dependabot-version-updates/about-dependabot-version-updates)
- [Bazel documentation](https://bazel.build/)

Stay engaged with the broader open source community for feedback and further improvements.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-16-dependabot-version-updates-now-support-bazel)
