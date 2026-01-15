---
layout: post
title: 'Upcoming Updates for Azure Pipelines Agent Images: Deprecation and Migration Guidance'
author: Shubham Agarwal, Eric van Wijk
canonical_url: https://devblogs.microsoft.com/devops/upcoming-updates-for-azure-pipelines-agents-images/
viewing_mode: external
feed_name: Microsoft DevOps Blog
feed_url: https://devblogs.microsoft.com/devops/feed/
date: 2025-10-18 18:40:30 +00:00
permalink: /azure/news/Upcoming-Updates-for-Azure-Pipelines-Agent-Images-Deprecation-and-Migration-Guidance
tags:
- Agent Pools
- Azure
- Azure DevOps
- Azure Pipelines
- CI/CD
- Container Jobs
- DevOps
- Hosted Agents
- Image Updates
- Macos 13 Ventura Deprecation
- Macos Sequoia
- News
- Pipeline Migration
- Self Hosted Agent
- Ubuntu 20.04 Deprecation
- Ubuntu 24.04
- Windows Server
- Windows Server Deprecation
- YAML Pipeline
section_names:
- azure
- devops
---
Shubham Agarwal and Eric van Wijk outline critical updates for Azure Pipelines users, with step-by-step guidance on transitioning to new agent images to maintain secure and reliable CI/CD workflows.<!--excerpt_end-->

# Upcoming Updates for Azure Pipelines Agent Images

**Authors:** Shubham Agarwal, Eric van Wijk

Azure Pipelines is updating its hosted agent images to ensure the most secure and up-to-date CI/CD environments. This news covers support for new images, deprecation timelines for existing ones, and recommended actions for maintaining healthy and operational pipelines.

## Ubuntu Images

### Ubuntu 24.04 Availability and "ubuntu-latest" Mapping

- Ubuntu-24.04 image has been available since October 2024.
- The "ubuntu-latest" image will soon map to Ubuntu-24.04, replacing ubuntu-22.04 as the default.
- Some tools may no longer be available in Ubuntu-24.04; review documentation before migrating.

### Ubuntu 20.04 Deprecation Plan

- **Deprecation Start Date:** March 19, 2025
- **Brownout Period:** March 19 – April 8, 2025 (intermittent job failures using Ubuntu-20.04)
- **Full Removal:** April 30, 2025 (pipelines using Ubuntu-20.04 will fail to run)

#### Recommended Actions

- Update pipeline YAML (`vmImage`) from `ubuntu-20.04` to `ubuntu-22.04`, `ubuntu-24.04`, or `ubuntu-latest`.

  ```yaml
  pool:
    vmImage: 'ubuntu-24.04'
  ```

- Find pipelines using deprecated images with [this script](https://github.com/microsoft/azure-pipelines-agent/tree/master/tools/FindPipelinesUsingRetiredImages) or via Azure DevOps web UI.
- Consider [container jobs](https://learn.microsoft.com/azure/devops/pipelines/process/container-phases) or [Managed DevOps Pools](https://devblogs.microsoft.com/devops/managed-devops-pools-ga/) if Ubuntu-20.04 dependencies persist.

## Windows Images

### Windows Server 2025 Availability and Deprecation of Windows Server 2019

- **Windows Server 2025**: Generally available June 16, 2025.
- **windows-latest**: Will map to Windows Server 2025 starting September 2, 2025.
- **Windows Server 2019**: Deprecation begins June 1, 2025; full removal on January 1, 2026.

#### Recommended Actions

- Update pipelines to use `windows-2022`, `windows-2025`, or `windows-latest`.

  ```yaml
  pool:
    vmImage: 'windows-2025'
  ```

- Identify pipelines via the deprecation script or agent pool UI filters.

## macOS Images

### macOS Sequoia and macOS 13 Ventura Deprecation

- **macOS 15 Sequoia** is now generally available, with `macOS-latest` mapping to macOS 15.
- **macOS 13 Ventura** deprecation starts September 1, 2025, and retires December 4, 2025.
- Brownout periods in November 2025 will cause intermittent failures.

#### Recommended Actions

- Change support to `macOS-14`, `macOS-15`, or `macOS-latest` in pipeline YAML.

## Apple Silicon (ARM64) Previews

- Private preview is ongoing; public preview registration is now closed.

## Migration and Troubleshooting

- Use the provided [migration scripts](https://github.com/microsoft/azure-pipelines-agent/tree/master/tools/FindPipelinesUsingRetiredImages) to identify pipelines using deprecated images.
- For assistance, reach out through Azure DevOps support or [community forums](https://developercommunity.visualstudio.com/AzureDevOps).

## Summary of Key Dates

- **Ubuntu-20.04:** Full removal April 30, 2025
- **Windows Server 2019:** Removal January 1, 2026
- **macOS 13 Ventura:** Removal December 4, 2025

Timely updates to your pipelines and agent images ensure improved security, performance, and compatibility with the latest tools.

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/upcoming-updates-for-azure-pipelines-agents-images/)
