---
layout: post
title: Clearer Pull Request Reviewer Status and Enhanced Email Filtering in GitHub
author: Allison
canonical_url: https://github.blog/changelog/2025-08-14-clearer-pull-request-reviewer-status-and-enhanced-email-filtering
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-08-14 20:27:11 +00:00
permalink: /devops/news/Clearer-Pull-Request-Reviewer-Status-and-Enhanced-Email-Filtering-in-GitHub
tags:
- Accessibility
- Collaboration
- Email Filtering
- Email Notifications
- GitHub
- Inclusive Design
- Issue Tracking
- Metadata
- PR Approvals
- Pull Requests
- Reviewer Status
- Workflow Automation
section_names:
- devops
---
Allison introduces two improvements to GitHub: clearer distinctions for pull request reviewer status to improve accessibility, and added metadata in email notifications for easier filtering and issue tracking.<!--excerpt_end-->

# Clearer Pull Request Reviewer Status and Enhanced Email Filtering in GitHub

## Overview

GitHub has introduced improvements to help users better manage pull requests (PRs) and stay updated on important conversations:

- **Clearer distinctions for pull request reviewer status and approvals**
- **Additional headers in email notifications for better filtering**

---

## 1. Clearer Distinctions for Pull Request Reviewer Status and Approvals

### Accessibility Enhancements

- Reviewer status in the PR review process is now easier to understand, reducing reliance on color cues.
- Green checkmarks (indicating reviewers whose approval affects merge requirements) and GitHub Copilot are prioritized at the top of the “Reviewers” section in the sidebar alongside pending reviews and requests for changes.
- Reviewers with less impact on merge requirements (gray checkmarks) are grouped in a collapsible section labeled "+X more reviewers."
- Hovering over this section displays a tooltip for additional context.
- The updates introduce visual cues and descriptive labels, following inclusive best practices to meet accessibility standards.

### Benefits

- Users can quickly determine which reviews are required for a PR to be merged.
- Improved clarity and inclusivity for all users, regardless of their method of interaction (visual, assistive technology, etc.).

---

## 2. Additional Headers Added to Email Notifications for Better Filtering

### Feature Details

- New headers in email notifications for issues and PRs now include labels, assignees, milestones, and issue types, making emails more filterable.
- Headers added:
  - `X-Github-Labels`
  - `X-Github-Assignees`
  - `X-Github-Milestone`
  - `X-Github-Issuetype`
- If a field isn't set, the corresponding header is empty or omitted.

### Benefits

- Users can create more efficient, automated email filters for PRs and issues.
- Improved ability to monitor high-priority discussions by team, label, or milestone.

---

## How to Give Feedback

- For reviewer status improvements: [Share thoughts in Community discussion](https://github.com/orgs/community/discussions/169699)
- For email filtering feedback: [Join Community discussion](https://github.com/orgs/community/discussions/169627)

---

## Summary

These updates are set to make GitHub more accessible and its notification system more powerful, supporting better workflow management and collaboration for development teams.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-14-clearer-pull-request-reviewer-status-and-enhanced-email-filtering)
