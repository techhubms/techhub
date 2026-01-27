---
external_url: https://github.blog/changelog/2025-09-25-pull-request-files-changed-public-preview-now-supports-commenting-on-unchanged-lines
title: 'Enhanced GitHub Pull Request Files Changed Page: Comment Anywhere in Changed Files'
author: Allison
feed_name: The GitHub Blog
date: 2025-09-25 21:23:24 +00:00
tags:
- API
- Bug Fixes
- Change Comments
- Code Review
- Collaboration
- Collaboration Tools
- Developer Workflow
- Diff Viewer
- Feature Preview
- Files Changed
- GitHub
- Improvement
- Keyboard Shortcuts
- Navigation
- Pull Requests
- Single File Mode
- UI Enhancement
section_names:
- devops
primary_section: devops
---
Allison discusses new GitHub features for pull request reviews, enabling comments on any line of changed files and improved single file navigation, benefiting developers and reviewers.<!--excerpt_end-->

# Enhanced GitHub Pull Request Files Changed Page: Comment Anywhere in Changed Files

GitHub has rolled out a major update to the pull request “Files changed” page in public preview, making code reviews more flexible and collaborative for development teams.

## Major Feature: Comment on Any Line of Changed Files

Developers can now comment on any line in a changed file, not just the three lines around a change. This means you can:

- Flag changes that should have been made
- Suggest edits to lines outside the immediate diff
- Provide extra context to reviewers, even on unchanged parts of changed files

**How to use the new commenting feature:**

1. Open the new “Files changed” page within your pull request.
2. Expand diffs to view unchanged lines.
3. Click the `+` button on any line to add a comment or suggestion.

Comments placed this way will show up on both the "Files changed" and "Conversation" pages, helping keep reviews organized and thorough.

**Note:** The feature is rolling out progressively per repository.

### API and UI Considerations

- Comments on unchanged lines are accessible through existing API endpoints and webhook events, though API support is limited for now.
- If you view a pull request with such comments using the classic interface, you’ll see a warning message letting you know additional comments are available only on the new page.
- Positioning logic for all comments has changed, so you may notice differences in how comment threads are displayed.

## Single File Mode Improvements

Single file mode is designed to help reviewers work through large pull requests more easily. The latest update brings:

- **Automatic navigation:** Marking a file as viewed moves you to the next file.
- **Accurate navigation:** The next/previous file buttons respect applied filters.
- **Keyboard shortcuts:** Use `J` for next file, `K` for previous file.
- **Clear feedback:** A message appears in single file mode to reduce confusion.
- **Handling large/deleted files:** Files that aren’t rendered no longer affect single file mode.
- **Bug fixes:** Issues around unnecessary fetches and file filtering have been resolved.

## Feedback

GitHub encourages users to provide feedback via dedicated community links:

- [New ‘Files changed’ page feedback](https://gh.io/new-files-changed-feedback)
- [Comment anywhere on changed file feedback](https://gh.io/comment-any-line-feedback)

*Disclaimer: Public preview UI features are subject to change.*

## Reference Links

- [Official announcement on GitHub Blog](https://github.blog/changelog/2025-09-25-pull-request-files-changed-public-preview-now-supports-commenting-on-unchanged-lines)
- [Feature preview details](https://github.blog/changelog/2025-06-26-improved-pull-request-files-changed-experience-now-in-public-preview/)

## Summary

This update is aimed at developers and reviewers working on GitHub. It enhances code review flexibility with expanded commenting and streamlined navigation, making collaboration on pull requests faster and more precise.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-25-pull-request-files-changed-public-preview-now-supports-commenting-on-unchanged-lines)
