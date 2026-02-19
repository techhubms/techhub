---
layout: "post"
title: "New GitHub Settings to Configure Pull Request Access"
description: "This news post outlines recent updates to GitHub repository settings, giving maintainers more control over how contributions are managed. The changes include options to disable pull requests entirely or to restrict PR creation to collaborators only, offering flexibility for different project needs in both public and private repositories. The announcement also covers mobile app considerations and provides links to documentation for deeper technical management."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-13-new-repository-settings-for-configuring-pull-request-access"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-13 18:12:55 +00:00
permalink: "/2026-02-13-New-GitHub-Settings-to-Configure-Pull-Request-Access.html"
categories: ["DevOps"]
tags: ["Access Control", "Code Quality", "Collaboration", "Collaboration Tools", "Contribution Workflow", "DevOps", "GitHub", "News", "Open Source", "Project Management", "Pull Requests", "Repository Settings", "Team Practices"]
tags_normalized: ["access control", "code quality", "collaboration", "collaboration tools", "contribution workflow", "devops", "github", "news", "open source", "project management", "pull requests", "repository settings", "team practices"]
---

Allison explains new GitHub repository settings that give maintainers finer control over pull request contributions, detailing how to disable PRs entirely or limit them to collaborators for more managed development.<!--excerpt_end-->

# New GitHub Settings to Configure Pull Request Access

GitHub repositories now offer maintainers expanded control over how external contributions are managed:

## 1. Disable Pull Requests Entirely

- Maintainers can completely turn off pull requests from **Settings**.
- When disabled:
  - The pull requests tab is hidden.
  - No new pull requests can be opened.
  - Existing pull requests are no longer visible.
- Scenarios where this is useful:
  - Mirror repositories
  - Read-only codebases
  - Projects sharing code publicly without accepting contributions

![Repository setting to disable pull requests](https://github.com/user-attachments/assets/3d15772d-9ce4-4d98-b7d3-1d6963c45072)

## 2. Restrict Pull Requests to Collaborators

- Maintainers can restrict PR creation so that only collaborators (users with write access) can open new pull requests.
- All may view and comment on PRs, but only collaborators may submit them.
- Ideal during critical development phases or when tight control over changes is needed.
- Collaborators are managed via the **Collaborators** tab in repository settings.

![Repository setting to restrict pull request creation to collaborators only](https://github.com/user-attachments/assets/5c9fde90-5134-43fa-8134-2a83c20c6850)

## Availability

- Both settings are now available on all public and private repositories.
- Configure in **Settings > General > Features**.

## Mobile App Considerations

- Settings changes are coming soon to the mobile app.
- Currently, you may still see the PR tab after disabling pull requests, but creation is blocked.

## Related Features

- To temporarily limit activity from certain users, use [temporary interaction limits](https://docs.github.com/en/communities/moderating-comments-and-conversations/limiting-interactions-in-your-repository).

## Learn More

- [Managing pull request settings documentation](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/enabling-features-for-your-repository/disabling-pull-requests)
- [Blog post: Plans for maintainers](https://github.blog/open-source/maintainers/welcome-to-the-eternal-september-of-open-source-heres-what-we-plan-to-do-for-maintainers/)
- [Community Discussion](https://github.com/orgs/community/discussions/187038)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-13-new-repository-settings-for-configuring-pull-request-access)
