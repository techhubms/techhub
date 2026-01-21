---
external_url: https://github.blog/changelog/2025-08-21-pull-request-files-changed-public-preview-experience-august-21-updates
title: "GitHub Pull Request 'Files Changed' Public Preview: August 21 Updates"
author: Allison
feed_name: The GitHub Blog
date: 2025-08-21 19:44:25 +00:00
tags:
- Bug Fixes
- Code Review
- Developer Experience
- DevOps Tools
- GitHub
- Public Preview
- Pull Requests
- Review Submission
- Software Development
- Submodule Support
- UI Improvements
- Version Control
- Workflow
section_names:
- devops
---
Allison outlines the new features and improvements available in the GitHub pull request 'Files changed' public preview, such as better review panels, submodule support, and several usability fixes, helping developers streamline code review processes.<!--excerpt_end-->

# GitHub Pull Request 'Files Changed' Public Preview: August 21 Updates

GitHub has announced several updates to the public preview of the pull request "Files changed" page, aiming to enhance the experience for reviewers and contributors.

## ✨ Improved Review Submission Panel

- The review submission panel now appears below the `Submit review` button and remains scrollable.
- Action buttons are closer together for improved usability.
- Reviews can now be submitted with `Command` (or `Ctrl`) + `Enter`.
- The panel can be expanded to full height, making it easier to manage pending comments.

## 🔗 Submodule Support

- When a submodule reference is updated in a pull request, the changes in the submodule can now be individually reviewed directly from the Files Changed page.

## 🔧 Fixes

- The whitespace-only changes setting now persists and is not reset on page refresh.
- Changes from all commits are correctly displayed, even after filtering on commit subsets.
- The animation beneath changed files no longer loops.
- Pending comments display correctly in the review submission panel.
- Copying multiple lines of a diff no longer adds blank lines.
- Expanding diff hunks respects whitespace-only ignore settings.
- Improved tab-switching performance when dealing with large pull requests.

## 🚧 Known Issues

- Reviewing single commits and batching suggested changes are not yet supported.
- Some keyboard shortcuts are pending implementation.
- The new page is limited to showing the first 300 files in a pull request—for larger PRs, switch to the classic view.

For an up-to-date list of known issues, check the [feedback discussion](https://gh.io/new-files-changed-feedback).

## ❤️ Feedback

Developers are encouraged to try the new experience and share feedback to help guide further improvements. Visit the [feedback discussion](https://gh.io/new-files-changed-feedback) to report issues or make suggestions.

*Note: The UI in public preview is actively evolving and subject to change.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-21-pull-request-files-changed-public-preview-experience-august-21-updates)
