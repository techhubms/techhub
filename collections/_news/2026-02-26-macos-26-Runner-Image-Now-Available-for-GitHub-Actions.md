---
external_url: https://github.blog/changelog/2026-02-26-macos-26-is-now-generally-available-for-github-hosted-runners
title: macos-26 Runner Image Now Available for GitHub Actions
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-26 16:00:59 +00:00
tags:
- Actions
- Apple Silicon
- Arm64
- Build Automation
- CI/CD
- DevOps
- GitHub Actions
- GitHub Hosted Runners
- Intel X64
- Macos 26
- Macos Runner
- News
- Software Development
- Workflow Automation
- Xcode
section_names:
- devops
---
Allison announces that the macOS 26 runner image is now generally available for GitHub Actions, supporting both arm64 and x64 environments for continuous integration and deployment workflows.<!--excerpt_end-->

# macos-26 Runner Image Now Available for GitHub Actions

The macOS 26 runner image has moved from public preview to general availability for GitHub-hosted runners. This means developers now have a stable, fully supported environment for building and testing applications using the latest versions of macOS and Xcode tooling on GitHub Actions.

## Architecture Support

- **Apple Silicon (arm64):** Native support for workflows requiring the latest Apple hardware.
- **Intel (x64):** Continued support for existing applications and toolchains.

## Workflow Integration

You can specify the macOS 26 runner in your GitHub Actions workflow files with the following labels:

- `macos-26` — Standard runner for Apple Silicon (arm64)
- `macos-26-intel` — Standard runner for Intel (x64)
- `macos-26-large` — Large runner for Intel (x64)
- `macos-26-xlarge` — Extra-large runner for Apple Silicon (arm64)

## Getting Started

To take advantage of the new capabilities, update your workflow configuration to reference the desired runner label.

For additional details on the software and support included, refer to the [GitHub-hosted runners documentation](https://docs.github.com/actions/reference/runners/github-hosted-runners).

**Key Benefits:**

- Consistent environments for macOS, iOS, and cross-platform builds
- Faster builds on powerful hardware
- Immediate access to the latest Xcode versions

## Resources

Full changelog and context: [GitHub Blog Announcement](https://github.blog/changelog/2026-02-26-macos-26-is-now-generally-available-for-github-hosted-runners)

For more setup guidance and runner image details, see the [GitHub-hosted runners documentation](https://docs.github.com/actions/reference/runners/github-hosted-runners).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-26-macos-26-is-now-generally-available-for-github-hosted-runners)
