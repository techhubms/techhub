---
layout: "post"
title: "Faster .NET Upgrades Powered by GitHub Copilot"
description: "This article introduces the public preview of GitHub Copilot app modernization – Upgrade for .NET, an AI-powered tool integrated in Visual Studio to accelerate and simplify upgrading .NET applications. It explains setup, key features, upgrade workflows, and how Copilot's Agent Mode reduces manual effort for developers."
author: "McKenna Barlow"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/visualstudio/faster-net-upgrades-powered-by-github-copilot/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/visualstudio/tag/github-copilot/feed/"
date: 2025-05-19 16:00:43 +00:00
permalink: "/2025-05-19-Faster-NET-Upgrades-Powered-by-GitHub-Copilot.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: [".NET", ".NET Upgrade", "AI", "AI Powered Upgrades", "App Modernization", "Artificial Intelligence", "Code Transformation", "Coding", "Copilot Agent Mode", "Development Tools", "Git Integration", "GitHub Copilot", "Manual Change Learning", "Modernization", "News", "Unit Test Validation", "Upgrade", "Upgrade Workflows", "Visual Studio", "Workflow Automation"]
tags_normalized: ["net", "net upgrade", "ai", "ai powered upgrades", "app modernization", "artificial intelligence", "code transformation", "coding", "copilot agent mode", "development tools", "git integration", "github copilot", "manual change learning", "modernization", "news", "unit test validation", "upgrade", "upgrade workflows", "visual studio", "workflow automation"]
---

In this article, McKenna Barlow introduces the public preview of GitHub Copilot app modernization – Upgrade for .NET, detailing how developers can upgrade applications with AI-assisted tools in Visual Studio.<!--excerpt_end-->

# Faster .NET Upgrades Powered by GitHub Copilot

*Author: McKenna Barlow*

## Introduction

Upgrading and modernizing .NET applications has long been a time-consuming and risky endeavor. To address this, GitHub has launched the public preview of **GitHub Copilot app modernization – Upgrade for .NET**, a new AI-powered solution designed to streamline and automate the upgrade process directly within Visual Studio.

## Overview of GitHub Copilot app modernization – Upgrade for .NET

**GitHub Copilot app modernization – Upgrade for .NET** leverages the AI capabilities of GitHub Copilot and its Agent Mode to serve as an intelligent companion for upgrading .NET projects. The tool analyzes your application’s codebase, determines upgrade paths, applies code changes automatically, and reduces manual intervention.

Read more on the [.NET blog](https://devblogs.microsoft.com/dotnet/github-copilot-upgrade-dotnet).

## How to Get Started

Follow these steps to utilize the modernization tool:

1. **Install the Extension**: Download the extension from the Visual Studio Marketplace.
2. **Enable Copilot Agent Mode**:
    - Ensure Visual Studio 17.14 or later is installed.
    - Sign in with a GitHub account with an active Copilot subscription.
    - Go to Tools > Options > GitHub > Copilot > Copilot Chat and enable "Agent mode in the chat pane".
    - Open Copilot Chat, click the “Ask” button, then select “Agent.”
    - In Copilot Chat Tool Selector, pick the “Upgrade” tool for specialized functionality.

For more information on Agent Mode, see the [dedicated blog post](https://devblogs.microsoft.com/visualstudio/agent-mode-has-arrived-in-preview-for-visual-studio/).

## Running Upgrades

There are two primary ways to start upgrading your .NET solution:

- **Context Menu**: Right-click your project or solution in Solution Explorer and choose "Upgrade with GitHub Copilot".
- **Copilot Chat Command**: Enter a command like "Upgrade my solution to .NET 8" in the Copilot Chat window. Copilot will then analyze, prepare, and guide you through the upgrade process.

## Key Features

- **Automated Code Transformations**: Copilot performs necessary changes to your codebase, moving applications to modern .NET versions.
    - See progress with [upgrade process media](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/05/GHCPUA-Update-Progress-Focus-1.mp4).
- **Customizable Workflows**: Choose specific projects to upgrade, opt to address vulnerable packages, and personalize upgrade paths.
- **Learning from Manual Changes**: If manual updates are required, Copilot records and learns from your interventions, reusing this knowledge for similar scenarios in the same upgrade.
- **Git Integration**: Automatic commits are created for upgraded files, supporting incremental adoption and testing.
- **Automatic Test Validation**: Your application's unit tests are run automatically post-upgrade, verifying correctness.
- **Agent Mode Support**: Enhanced automation and conversation-driven upgrades with Copilot Agent Mode in the latest Visual Studio preview.

## Feedback and Community Engagement

Feedback is welcomed to improve the tool:

- Use the “Leave Feedback” button in the upper right corner of the Upgrade Manager UX in Visual Studio.
- Email the team at [ghcpuacusteng@microsoft.com](mailto:ghcpuacusteng@microsoft.com).
- Respond to the [feedback survey](https://www.surveymonkey.com/r/5G3BDTD?source=vsblog).

## Stay Connected

For ongoing Visual Studio updates, release notes, videos, and community interaction, visit the [Visual Studio Hub](https://visualstudio.microsoft.com/hub/).

---

**References & Resources**

- [GitHub Copilot app modernization – Upgrade for .NET (Public Preview)](https://devblogs.microsoft.com/dotnet/github-copilot-upgrade-dotnet)
- [Agent Mode in Visual Studio Preview](https://devblogs.microsoft.com/visualstudio/agent-mode-has-arrived-in-preview-for-visual-studio/)
- [Visual Studio Marketplace](https://marketplace.visualstudio.com/)
- [Feedback Survey](https://www.surveymonkey.com/r/5G3BDTD?source=vsblog)

---

**Image and Media Credits:**

- GIFs and media referenced in the article are available on the original [Visual Studio Blog post](https://devblogs.microsoft.com/visualstudio/faster-net-upgrades-powered-by-github-copilot/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/faster-net-upgrades-powered-by-github-copilot/)
