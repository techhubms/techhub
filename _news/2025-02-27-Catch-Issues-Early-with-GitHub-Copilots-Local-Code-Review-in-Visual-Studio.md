---
layout: "post"
title: "Catch Issues Early with GitHub Copilot's Local Code Review in Visual Studio"
description: "Visual Studio introduces an AI-powered feature—integrating GitHub Copilot for local code review before committing changes. This helps developers identify potential issues such as security or performance problems earlier, improving code quality and workflow efficiency."
author: "Jessie Houghton"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/visualstudio/catch-issues-before-you-commit-to-git/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/visualstudio/tag/copilot/feed/"
date: 2025-02-27 17:06:24 +00:00
permalink: "/2025-02-27-Catch-Issues-Early-with-GitHub-Copilots-Local-Code-Review-in-Visual-Studio.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: ["AI", "AI Features", "Code Review", "Coding", "Commit Workflow", "Copilot", "Copilot Suggestions", "Developer Experience", "DevOps", "Git", "GitHub Copilot", "News", "Performance Issues", "Security Issues", "Source Control", "VS"]
tags_normalized: ["ai", "ai features", "code review", "coding", "commit workflow", "copilot", "copilot suggestions", "developer experience", "devops", "git", "github copilot", "news", "performance issues", "security issues", "source control", "vs"]
---

Jessie Houghton presents a Visual Studio update that leverages GitHub Copilot's AI for local code review, helping developers catch issues before committing to Git.<!--excerpt_end-->

## Introduction

Discovering issues with code changes after committing can disrupt development workflows. To address this, Visual Studio now offers an integrated feature—powered by GitHub Copilot—to review local code changes before they are committed. This aims to catch potential performance and security problems earlier, promoting better quality throughout the codebase.

## Feature Overview

### What’s New

- Visual Studio integrates GitHub Copilot to review your local code changes before committing them.
- The feature highlights issues and suggests improvements, supporting higher code quality and early defect detection.
- The review works before code is part of a pull request, enabling earlier feedback in the development cycle.

### How to Get Started

1. **Activate GitHub Copilot in Visual Studio**
    - Ensure you have an active GitHub Copilot subscription and it is enabled within Visual Studio. New users can [sign up for free](https://github.com/settings/copilot).

2. **Enable Feature Flags**
    - Go to `Tools > Options > Preview Features > Pull Request Comments`.
    - Then, `Tools > Options > GitHub > Copilot > Source Control Integration > Enable Git preview features`.

3. **Use the New Code Review Button**
    - In the Git Changes window, you’ll see a new sparkle comment button.
    - Clicking this invokes GitHub Copilot, which analyzes your staged changes and adds inline review comments suggesting improvements.

![Copilot reviews changes and suggests commit improvements](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAfQAAAEQAQMAAAB8zRRdAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAKElEQVR4nO3BMQEAAADCoPVPbQwfoAAAAAAAAAAAAAAAAAAAAACAlwFEAAAByxQ4yQAAAABJRU5ErkJggg==)

### Using Copilot Suggestions

- Suggestions appear inline in your working file, making it easy to review and apply changes.
- You can navigate between Copilot’s comments or collapse them as needed using the up arrow icon in the comment interface.
- Since these are AI-generated suggestions, it is important to validate their accuracy before applying.

## Feedback and Iteration

Microsoft encourages developers to provide feedback to drive further improvements:

- [Share your scenarios here](https://aka.ms/LocalCodeReview) to help Microsoft enhance Copilot’s code review capabilities.
- You can also participate in community suggestions and propose customizations directly through:
  - [Visual Studio Developer Community Suggestion: Use Copilot to review commit](https://developercommunity.visualstudio.com/t/Use-Copilot-to-review-commit/10575248)
  - [Customize Copilot code review](https://developercommunity.visualstudio.com/t/Customize-Copilot-code-review/10844722)

## Conclusion

This new feature represents a significant step towards integrating AI assistance deeper into the developer workflow. By catching issues before code is committed, developers can save time, reduce bugs, and maintain a higher quality codebase. Visual Studio’s team values user input, especially as AI-driven development continues to expand.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/catch-issues-before-you-commit-to-git/)
