---
layout: "post"
title: "Improved Pull Request “Files Changed” – February 5 GitHub Updates"
description: "This update introduces CODEOWNERS validation in the new 'Files changed' pull request UI on GitHub, along with performance improvements for navigating and reviewing large pull requests. Key fixes enhance reviewer workflows, especially around responsiveness, stability, and notification accuracy."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-05-improved-pull-request-files-changed-february-5-updates"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-05 23:05:59 +00:00
permalink: "/2026-02-05-Improved-Pull-Request-Files-Changed-February-5-GitHub-Updates.html"
categories: ["DevOps"]
tags: ["Code Review", "CODEOWNERS", "Collaboration", "Collaboration Tools", "Developer Experience", "DevOps", "GitHub", "Improvement", "News", "Notification Handling", "Performance Optimization", "Pull Requests", "UI Update", "Workflow Improvement"]
tags_normalized: ["code review", "codeowners", "collaboration", "collaboration tools", "developer experience", "devops", "github", "improvement", "news", "notification handling", "performance optimization", "pull requests", "ui update", "workflow improvement"]
---

Allison summarizes GitHub’s February 5th update, including CODEOWNERS validation in the new ‘Files changed’ tab and performance improvements for large pull requests, improving efficiency for development teams.<!--excerpt_end-->

# Improved Pull Request “Files Changed” – February 5 GitHub Updates

**Author:** Allison

## Overview

This GitHub release brings CODEOWNERS validation to the new “Files changed” pull request experience, along with significant performance enhancements that streamline review workflows—particularly for large pull requests. The release also includes improvements targeted at mobile screens and several bug fixes for review interactions and notifications.

## Key Features and Improvements

### 1. CODEOWNERS Validation

- The new 'Files changed' experience now surfaces required reviewers based on CODEOWNERS rules.
- Ensures that required peer review requirements are accurately enforced before merges, closing a gap from the older UI.

### 2. Small Screen Layout Improvements

- Adjustments have been made to layout, spacing, and overflow, so diffs, comments, headers, and controls display correctly on mobile and smaller viewports.

### 3. Performance Enhancements

- Large pull request diffs are now more responsive, with up to 67% faster interactions in the new 'Files changed' UI.
- Improved navigation speed between Conversations and Files tabs.
- Virtualization is applied to only the largest pull requests for improved scrolling and interaction performance.
- File tree resizing and reviewing in Safari have better stability and responsiveness.
- Substantially reduced memory usage during large PR reviews.

### 4. Bug Fixes

- Notifications now correctly mark as read when accessing the Files changed tab directly.
- The sticky header in file diffs is now more reliable when scrolling.
- Code snippet backgrounds in replies have been adjusted for better readability.
- Commenting errors across large index diffs have been resolved.

## Feedback

You can submit feedback, ask questions, and view known issues in the [Files changed preview feedback discussion](https://gh.io/new-files-changed-feedback).

---
This update is focused on strengthening code review quality and efficiency within GitHub, reducing friction in common workflows, and making the review process more accessible to everyone, including mobile users.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-05-improved-pull-request-files-changed-february-5-updates)
