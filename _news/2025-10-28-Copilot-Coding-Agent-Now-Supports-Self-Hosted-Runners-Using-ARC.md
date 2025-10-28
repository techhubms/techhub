---
layout: "post"
title: "Copilot Coding Agent Now Supports Self-Hosted Runners Using ARC"
description: "This news update announces that GitHub Copilot coding agents can now operate in custom infrastructure via self-hosted GitHub Actions runners managed by Actions Runner Controller (ARC). It walks through the ability to configure the agent environment for deeper integration with internal resources and provides steps to get started with ARC scale sets."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-10-28-copilot-coding-agent-now-supports-self-hosted-runners"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-10-28 15:20:53 +00:00
permalink: "/2025-10-28-Copilot-Coding-Agent-Now-Supports-Self-Hosted-Runners-Using-ARC.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["Actions", "Actions Runner Controller", "Agent Customization", "AI", "ARC", "Build Automation", "Cloud Infrastructure", "Continuous Integration", "Copilot", "Copilot Coding Agent", "DevOps", "DevOps Automation", "GitHub Actions", "GitHub Copilot", "Improvement", "News", "Pull Request Automation", "Scale Sets", "Self Hosted Runners", "Universe25", "YAML"]
tags_normalized: ["actions", "actions runner controller", "agent customization", "ai", "arc", "build automation", "cloud infrastructure", "continuous integration", "copilot", "copilot coding agent", "devops", "devops automation", "github actions", "github copilot", "improvement", "news", "pull request automation", "scale sets", "self hosted runners", "universe25", "yaml"]
---

Allison details the new capability for GitHub Copilot coding agents to leverage self-hosted runners via Actions Runner Controller, enabling more customizable and secure development environments for automated coding workflows.<!--excerpt_end-->

# Copilot Coding Agent Now Supports Self-Hosted Runners Using ARC

GitHub Copilot coding agents can now be configured to run in your organization's own infrastructure using self-hosted GitHub Actions runners managed by [Actions Runner Controller (ARC)](https://docs.github.com/actions/tutorials/use-actions-runner-controller/quickstart?utm_source=changelog-docs-cca-selfhosted-runners&utm_medium=changelog&utm_campaign=universe25). This latest enhancement provides greater flexibility and security, allowing Copilot agents access to private resources that aren't exposed to the public internet.

## What Is the Copilot Coding Agent?

The [Copilot coding agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent?utm_source=changelog-docs-cca-selfhosted-runners&utm_medium=changelog&utm_campaign=universe25) is a background, autonomous agent capable of performing development tasks asynchronously. When you delegate a task, it creates a draft pull request, performs edits, runs builds and tests, and then requests your review—all from its own ephemeral development environment powered by GitHub Actions.

## Benefits of Self-Hosted Runner Support

- **Access Internal Resources:** Run workflows against infrastructure with access to internal packages and systems.
- **Customizable Environments:** Control the scale and configuration of runners using ARC-managed scale sets.
- **Security:** Keep sensitive assets inside your network while leveraging Copilot's automation features.

## How to Get Started

1. **Deploy Actions Runner Controller (ARC)** and configure your desired scale set by following the [quickstart guide](https://docs.github.com/actions/tutorials/use-actions-runner-controller/quickstart?utm_source=changelog-docs-cca-selfhosted-runners&utm_medium=changelog&utm_campaign=universe25).
2. **Update Workflow Configuration:** Edit your `copilot-setup-steps.yml` to specify the ARC scale set as the `runs-on` target:

   ```yaml
   jobs:
     copilot-setup-steps:
       runs-on: arc-scale-set-name
   ```

3. **Refer to [agent environment documentation](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/use-copilot-agents/coding-agent/customize-the-agent-environment?utm_source=changelog-docs-cca-selfhosted-runners&utm_medium=changelog&utm_campaign=universe25)** for guidance on advanced customization.

## Who Should Use This?

This update is beneficial for teams needing Copilot coding agents to interact securely with on-premises resources, or requiring granular control over runner environments for compliance or customization reasons.

---

Learn more in [the official changelog post](https://github.blog/changelog/2025-10-28-copilot-coding-agent-now-supports-self-hosted-runners) and get started with deeper integration of GitHub Copilot in your DevOps workflows.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-28-copilot-coding-agent-now-supports-self-hosted-runners)
