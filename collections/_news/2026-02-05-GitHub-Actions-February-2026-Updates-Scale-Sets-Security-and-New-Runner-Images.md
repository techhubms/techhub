---
layout: "post"
title: "GitHub Actions February 2026 Updates: Scale Sets, Security, and New Runner Images"
description: "This update details GitHub Actions’ latest features as of February 2026, including a new standalone runner scale set client for custom autoscaling, expanded security controls through action allowlisting for all users, and early access to Windows Server 2025 with Visual Studio 2026 and macOS 26 Intel runner images. The content explains how to leverage these capabilities for more flexible infrastructure, stronger security, and seamless migration testing."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-05-github-actions-early-february-2026-updates"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-05 12:51:46 +00:00
permalink: "/2026-02-05-GitHub-Actions-February-2026-Updates-Scale-Sets-Security-and-New-Runner-Images.html"
categories: ["DevOps"]
tags: ["Action Allowlisting", "Actions", "Agentic Workflows", "Autoscaling", "CI/CD", "Custom Runners", "DevOps", "GitHub Actions", "Infrastructure Automation", "Macos 26", "News", "Release Management", "Runner Scale Set Client", "Security Controls", "Telemetry", "VS", "Windows Server", "Xcode", "YAML Workflow"]
tags_normalized: ["action allowlisting", "actions", "agentic workflows", "autoscaling", "cislashcd", "custom runners", "devops", "github actions", "infrastructure automation", "macos 26", "news", "release management", "runner scale set client", "security controls", "telemetry", "vs", "windows server", "xcode", "yaml workflow"]
---

Allison details February 2026 GitHub Actions updates, including public preview of the autoscaling runner scale set client, new cross-platform runner images, and expanded security controls—all geared to enhance DevOps flexibility, security, and migration planning.<!--excerpt_end-->

# GitHub Actions: Early February 2026 Updates

Authored by Allison

GitHub Actions brings several significant enhancements in February 2026, focused on scaling flexibility, security, and environment upgrades for DevOps practitioners.

---

## GitHub Actions Runner Scale Set Client (Public Preview)

- **Standalone autoscaling**: The new Go-based runner scale set client allows users to implement advanced runner autoscaling logic on any infrastructure—containers, VMs, or bare metal—without Kubernetes.
- **Platform agnostic**: Works across Windows, Linux, and macOS.
- **Full provisioning control**: Users define runner creation, scaling, and destruction using custom policies.
- **Multi-label and agentic workflow support**: Assign multiple labels for diverse pipelines and integrate with agentic workflows, including the GitHub Copilot coding agent.
- **Telemetry**: Real-time monitoring of runner and job performance with built-in metrics.

> *This tool complements (but does not replace) Actions Runner Controller (ARC), providing more options for teams not using Kubernetes. ARC continues as the reference solution for Kubernetes.*

- Explore the [GitHub Actions Runner Scale Set Client](https://github.com/actions/scaleset)
- Read the [documentation](https://docs.github.com/actions/hosting-your-own-runners)
- Join the [community discussion](https://gh.io/to-community-186265)

---

## Security: Action Allowlisting for All Plan Types

- **Broader availability**: Action allowlisting—control over which actions and workflows can run—extends to all GitHub plan types, improving security for Free, Team, and Enterprise users.
- **Enforce least privilege**: Specify allowed actions for more secure, compliant CI/CD pipelines.
- **Configuration docs and examples**: [Managing GitHub Actions permissions](https://docs.github.com/repositories/managing-your-repositorys-settings-and-features/enabling-features-for-your-repository/managing-github-actions-settings-for-a-repository#allowing-select-actions-and-reusable-workflows-to-run)

---

## New Runner Images

### Windows Server 2025 with Visual Studio 2026

- **Public preview**: Early access image enables testing pipelines against Visual Studio 2026 before default migration (scheduled for May 2026).
- **Seamless migration**: Option to toggle between new and previous images for smoother upgrades.
- **Usage**: Set `runs-on:` to `windows-2025-vs2026` in workflow YAML.
- More info: [Windows runner images](https://docs.github.com/actions/reference/runners/github-hosted-runners?versionId=free-pro-team%40latest&productId=actions&restPage=concepts%2Crunners%2Cgithub-hosted-runners)

### macOS 26 Intel Image

- **Larger runners**: Latest Intel-based macOS and Xcode image available in public preview.
- **Usage**: Set `runs-on:` to `macos-26-large` in workflow YAML.
- [macOS runner images documentation](https://docs.github.com/actions/reference/runners/larger-runners#available-macos-larger-runners-and-labels)

---

These updates give DevOps teams improved scaling, customizable security, and robust migration/test environments for cloud-native CI/CD with GitHub Actions.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-05-github-actions-early-february-2026-updates)
