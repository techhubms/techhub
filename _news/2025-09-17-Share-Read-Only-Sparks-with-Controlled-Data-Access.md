---
layout: "post"
title: "Share Read-Only Sparks with Controlled Data Access"
description: "This GitHub update introduces the ability to publish Sparks as read-only, giving developers and organizations more control over how others interact with their apps. The release covers step-by-step sharing settings, improved JSON handling, editor synchronization, and enhanced loading state management to optimize the developer workflow in Spark."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-17-share-read-only-sparks-with-controlled-data-access"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-17 20:00:28 +00:00
permalink: "/2025-09-17-Share-Read-Only-Sparks-with-Controlled-Data-Access.html"
categories: ["DevOps"]
tags: ["Access Control", "App Sharing", "Bug Fixes", "Code Editor", "Collaboration", "Copilot", "Developer Tools", "DevOps", "GitHub", "JSON", "News", "Read Only Mode", "Release Notes", "Sparks", "Workflow Improvements"]
tags_normalized: ["access control", "app sharing", "bug fixes", "code editor", "collaboration", "copilot", "developer tools", "devops", "github", "json", "news", "read only mode", "release notes", "sparks", "workflow improvements"]
---

Allison details the new read-only sharing feature for Sparks on GitHub, offering developers greater data control and outlining related workflow improvements.<!--excerpt_end-->

# Share Read-Only Sparks with Controlled Data Access

GitHub now lets you share your Sparks as **read-only**, offering better control over how others interact with your GitHub apps. When sharing a Spark, you can now pick between:

- **Write-access** (users can edit Spark content)
- **Read-access** (users can only view your Spark)

### How to Share a Read-Only Spark

1. Publish your Spark on GitHub
2. Set the visibility option to either **Organization** or **All GitHub users**
3. In the expanded data access options, select **Read access** to restrict editing

This feature enables safe data sharing and collaboration—either within your organization or with the wider GitHub community—without risking unintended changes to your app’s data.

[Try sharing a read-only Spark today](https://github.com/spark)

## Workflow and Quality Improvements

Recent updates also include several bug fixes and quality enhancements:

- **JSON Mode Fix:** Corrected issues with JSON return format when calling `spark.llm` using JSON mode
- **Editor Sync:** Improved synchronization in the Spark code editor to prevent lag or unsaved changes
- **Loading State:** Fixed issues causing Sparks to get stuck in “loading” states, resulting in smoother workbench logic

These changes make building and managing apps with GitHub Spark more reliable and efficient.

---

Learn more or get started by visiting [github.com/spark](https://github.com/spark).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-17-share-read-only-sparks-with-controlled-data-access)
