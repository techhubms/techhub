---
layout: "post"
title: "Demo: Using GitHub Copilot CLI and YOLO Mode"
description: "This video showcases how to use the --yolo command in GitHub Copilot CLI to execute commands without manual approvals. Presented by @shanselman, it explores scenarios such as autonomous command execution in containerized environments and explains the benefits and precautions when letting Copilot run tool calls autonomously."
author: "GitHub"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=UMz8aQ4lOtE"
viewing_mode: "internal"
feed_name: "GitHub YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UC7c3Kb6jYCRj4JOHHZTxKsQ"
date: 2026-01-22 00:00:59 +00:00
permalink: "/2026-01-22-Demo-Using-GitHub-Copilot-CLI-and-YOLO-Mode.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "Autonomous Execution", "CLI Tools", "Command Automation", "Command Line", "Containerization", "Copilot CLI", "Developer Tools", "GitHub Copilot", "GitHub Copilot CLI", "Videos", "Yolo", "YOLO Mode"]
tags_normalized: ["ai", "autonomous execution", "cli tools", "command automation", "command line", "containerization", "copilot cli", "developer tools", "github copilot", "github copilot cli", "videos", "yolo", "yolo mode"]
---

GitHub presents a demonstration by @shanselman on leveraging the --yolo mode of GitHub Copilot CLI, which enables autonomous command execution—ideal for containerized or automated developer workflows.<!--excerpt_end-->

{% youtube UMz8aQ4lOtE %}

# Demo: Using GitHub Copilot CLI and YOLO Mode

In this demonstration, @shanselman walks through the functionality of the `--yolo` mode in the GitHub Copilot CLI. Traditionally, developers must manually approve each command that Copilot suggests, but with `--yolo`, all tool calls and path accesses are approved automatically. This allows Copilot to execute commands autonomously without user intervention.

## Key Features

- **Bypassing Manual Approvals**: The `--yolo` command lets the agent skip manual confirmations, making the workflow more efficient when repetitive approvals are not needed.
- **Best Uses**: Particularly useful when working in **containerized environments** or automated testing scenarios, where frequent human approvals would slow progress.
- **Caution**: Granting autonomous execution means Copilot has more power, so use it only in trusted, isolated, or disposable environments.

## Example Use Case

- Running installation scripts or repetitive tasks inside a Docker container, letting Copilot handle command execution without stopping for manual checks.

## Why Use YOLO Mode?

- Increases development speed for specific use cases
- Reduces constant interactions for approvals
- Facilitates smoother automation in controlled settings

> "Letting Copilot run autonomously with --yolo drops the friction in development pipelines that rely on repetitive command approvals."

## Additional Resources

- [Subscribe to GitHub's YouTube](http://bit.ly/subgithub)
- [GitHub Blog](https://github.blog)
- [GitHub on X](https://twitter.com/github)
- [LinkedIn](https://linkedin.com/company/github)
- [Instagram](https://www.instagram.com/github)
- [TikTok](https://www.tiktok.com/@github)
- [Facebook](https://www.facebook.com/GitHub/)

For more Copilot CLI tips, follow GitHub on your favorite platform.
