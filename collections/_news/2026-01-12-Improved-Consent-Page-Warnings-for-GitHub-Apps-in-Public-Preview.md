---
layout: post
title: Improved Consent Page Warnings for GitHub Apps in Public Preview
author: Allison
canonical_url: https://github.blog/changelog/2026-01-12-selectively-showing-act-on-your-behalf-warning-for-github-apps-is-in-public-preview
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2026-01-12 22:16:06 +00:00
permalink: /devops/news/Improved-Consent-Page-Warnings-for-GitHub-Apps-in-Public-Preview
tags:
- Application Security
- Authentication
- Consent Page
- Developer Tools
- Ecosystem & Accessibility
- GitHub Apps
- Improvement
- OAuth
- Public Preview
- Sign in Experience
- User Permissions
- User Profile
section_names:
- devops
---
Allison details an update to the GitHub Apps consent page, clarifying when the 'Act on your behalf' warning is shown for app authorizations, improving both user experience and developer support.<!--excerpt_end-->

# Improved Consent Page Warnings for GitHub Apps in Public Preview

GitHub has updated the consent page for GitHub Apps to provide clearer and less alarming messaging during the user sign-in process. Previously, every application authorization—including those only requesting read access to user profiles—displayed an 'Act on your behalf' warning. This confused users and often led to unnecessary support requests, as many apps simply require basic identity information for sign-in purposes, not actual access to repositories or other resources.

### What's Changed

- The 'Act on your behalf' warning will now only appear if the GitHub App is requesting access to repositories, organizations, or enterprise data (either read or write permissions).
- If the app requests only read access to a user's profile and email, the consent page omits the warning.
- This refinement means users can authorize apps for sign-in more confidently, reducing concern about unwanted data access.

### Before and After

- **Before:**
  - The warning appeared for all applications, regardless of the permissions requested.
  - Users saw alarming language even for basic sign-in operations.
  - Example (see provided screenshots): The warning was always visible.
- **After:**
  - The warning only appears when broader permissions are requested.
  - Basic sign-in apps no longer display this message.

### Impact on Developers

- Fewer support tickets are expected, as users will better understand what apps can actually do.
- Applications using GitHub as a sign-in mechanism benefit from a clearer, more accurate consent process.

### Engage and Provide Feedback

Share your feedback or questions in the [Community discussion](https://github.com/orgs/community/discussions/184117).

---
_Improvement by Allison, released via the GitHub Blog._

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-12-selectively-showing-act-on-your-behalf-warning-for-github-apps-is-in-public-preview)
