---
layout: "post"
title: "Commit-by-Commit Review and Enhanced Filtering in GitHub Pull Request Files Changed Experience"
description: "This update to GitHub’s pull request 'Files changed' page introduces the long-awaited ability to review commits individually or in groups, streamlined commit filtering, and UI improvements. Developers benefit from enhanced performance, new keyboard shortcuts, a clearer file filter experience, and several stability fixes, streamlining code review workflows."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-12-11-review-commit-by-commit-improved-filtering-and-more-in-the-pull-request-files-changed-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-12-12 02:59:42 +00:00
permalink: "/2025-12-12-Commit-by-Commit-Review-and-Enhanced-Filtering-in-GitHub-Pull-Request-Files-Changed-Experience.html"
categories: ["DevOps"]
tags: ["Code Review", "Collaboration", "Collaboration Tools", "Commit Filtering", "Developer Tools", "DevOps", "Files Changed", "GitHub", "Improvement", "News", "Performance Enhancements", "Pull Requests", "Shortcuts", "UI Improvements", "Version Control", "Workflow"]
tags_normalized: ["code review", "collaboration", "collaboration tools", "commit filtering", "developer tools", "devops", "files changed", "github", "improvement", "news", "performance enhancements", "pull requests", "shortcuts", "ui improvements", "version control", "workflow"]
---

Allison reports on GitHub's improved pull request 'Files changed' view, delivering commit-by-commit review, advanced filtering, and productivity enhancements for developers.<!--excerpt_end-->

# Commit-by-Commit Review and Enhanced Filtering in GitHub Pull Request Files Changed Experience

GitHub has addressed a longstanding developer need by enabling detailed commit-by-commit reviews directly from the updated 'Files changed' page. This latest update introduces several workflow enhancements for improved code reviews and developer productivity.

## 🔍 Key Features

### Review Commit-by-Commit

- Developers can now select all, some, or individual commits to review from the ‘Files changed’ page without switching to the classic 'Commits' tab.
- Updated commit filter streamlines selecting commit ranges or single commits via the toolbar or the `C` keyboard shortcut.
- All ‘Files changed’ URLs consolidate under the new `/changes` path, with backward compatibility via redirects from old routes.

### File Filter Improvements

- A blue dot indicator now appears when file filters are active.
- The filter menu includes a new 'Clear filters' button for easy resets.

## 🚀 Performance Enhancements

- Improved responsiveness for resizing the file tree.
- Faster toggling of 'Minimize comments' and 'Split / Unified' views.
- Clicking 'Refresh' now updates pull requests more quickly without full page reloads.

## 🔧 Additional Fixes and Improvements

- Comments side panel resolves errors more reliably when marking comments as resolved.
- Nonstandard file path characters (such as commas) no longer prevent diffs from loading.
- The `.gitattributes` `linguist-generated` attribute is now respected as intended.
- Keyboard shortcuts:
  - `T` focuses the file filter and reveals the file tree.
  - `C` opens the commit filter.

## 🧪 How to Access

- The new commit-by-commit review features are available in the updated 'Files changed' experience.
- To try the new interface, click **Try the new experience** at the top of the classic 'Files changed' page.

## ❤ Feedback

Your input is valued. Problems, questions, or issues can be reported and discussed in the [“Files changed” preview feedback discussion](https://gh.io/new-files-changed-feedback).

---

These changes aim to streamline the code review process and further enhance collaborative workflows for GitHub users.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-11-review-commit-by-commit-improved-filtering-and-more-in-the-pull-request-files-changed-public-preview)
