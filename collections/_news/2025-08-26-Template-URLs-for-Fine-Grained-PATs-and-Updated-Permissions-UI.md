---
layout: "post"
title: "Template URLs for Fine-Grained PATs and Updated Permissions UI"
description: "This update, authored by Allison, highlights enhancements to GitHub's Personal Access Token (PAT) creation process. It introduces a new permissions picker UI and support for template URLs that prefill PAT details, improving developer workflows and permission management."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-26-template-urls-for-fine-grained-pats-and-updated-permissions-ui"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-26 21:33:06 +00:00
permalink: "/news/2025-08-26-Template-URLs-for-Fine-Grained-PATs-and-Updated-Permissions-UI.html"
categories: ["DevOps"]
tags: ["API Integration", "Automation", "Developer Experience", "DevOps", "Fine Grained Tokens", "GitHub", "News", "PAT", "Permissions Management", "Personal Access Tokens", "REST API", "Token Management", "UI/UX Design"]
tags_normalized: ["api integration", "automation", "developer experience", "devops", "fine grained tokens", "github", "news", "pat", "permissions management", "personal access tokens", "rest api", "token management", "uislashux design"]
---

Allison summarizes improvements to GitHub's fine-grained PAT creation, including a revamped permissions picker UI and shareable template URLs to streamline token generation for developers.<!--excerpt_end-->

# Template URLs for Fine-Grained PATs and Updated Permissions UI

GitHub has introduced several enhancements to the fine-grained Personal Access Token (PAT) creation experience, aimed at making it more developer-friendly and efficient.

## Key Updates

- **New Permissions Picker UI**: The PAT creation and edit pages now feature a redesigned permissions picker, modeled after the custom roles permission picker. It uses a tabbed UI to display selected permissions by resource type (account, repository, or organization), and supports search functionality for quickly finding permissions. The improved state management ensures that your selected permissions are saved when switching between different resource owners or repositories.

- **Template URLs with Query Parameters**: Developers can now prefill PAT details using query parameters in the URL, enabling the creation and sharing of token templates. This feature allows users to save time and reduce errors when generating tokens with common configurations. It addresses previous feedback regarding the complexity of picking the right permissions by allowing reusable, shareable URLs.

- **Developer Workflow Examples**:
  - [Update a repository and open a pull request](https://github.com/settings/personal-access-tokens/new?name=Core-loop+token&description=Write%20code%20and%20push%20it%20to%20main%21%20Includes%20permission%20to%20edit%20workflow%20files%20for%20Actions%20-%20remove%20%60workflows%3Awrite%60%20if%20you%20don%27t%20need%20to%20do%20that&contents=write&pull_requests=write&workflows=write)
  - [Call the GitHub Models API](https://github.com/settings/personal-access-tokens/new?name=GitHub+Models+token&description=Used%20to%20call%20GitHub%20Models%20APIs%20to%20easily%20run%20LLMs%3A%20https%3A%2F%2Fdocs.github.com%2Fgithub-models%2Fquickstart%23step-2-make-an-api-call&user_models=read)
  - [Manage Copilot licenses via API](https://github.com/settings/personal-access-tokens/new?name=Copilot+license+management&description=Enable%20or%20disable%20Copilot%20access%20for%20users%20with%20the%20Seat%20Management%20APIs%3A%20https%3A%2F%2Fdocs.github.com%2Frest%2Fcopilot%2Fcopilot-user-management%0ABe%20sure%20to%20select%20an%20organization%20for%20your%20resource%20owner%20below%21&organization_copilot_seat_management=write&members=read)

## Technical Details

- The new permissions picker enhances usability for managing fine-grained tokens and supports future enhancements such as improved API and description search, and extended integration with GitHub Copilot for fuzzy matching.
- The query parameter feature supports various token detail fields, from permissions and resource scopes to token lifetime, making it easier to reproduce or share token configurations among team members.
- These improvements will also be available in GitHub Enterprise Server 3.20.

For more information and documentation, see [Managing your personal access tokens](https://docs.github.com/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#pre-filling-fine-grained-personal-access-token-details-using-url-parameters).

Developers are encouraged to give feedback and participate in further discussion on [GitHub Community Announcements](https://github.com/orgs/community/discussions/categories/announcements).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-26-template-urls-for-fine-grained-pats-and-updated-permissions-ui)
