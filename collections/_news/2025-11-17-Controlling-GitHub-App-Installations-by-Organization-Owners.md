---
layout: "post"
title: "Controlling GitHub App Installations by Organization Owners"
description: "This news update introduces a new GitHub setting that lets organization owners control which users may install GitHub Apps on their repositories. The feature, currently in public preview, helps strengthen governance, reduce unauthorized installations, and supports compliance requirements by giving owners exclusive app installation rights over repositories within their organization."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-11-17-block-repository-administrators-from-installing-github-apps-on-their-own-now-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-11-17 17:24:55 +00:00
permalink: "/news/2025-11-17-Controlling-GitHub-App-Installations-by-Organization-Owners.html"
categories: ["DevOps"]
tags: ["Access Control", "Administration", "Compliance", "DevOps", "DevOps Tools", "Enterprise Management", "Enterprise Management Tools", "GitHub", "GitHub Apps", "Governance", "Improvement", "News", "Organization Owners", "Permissions", "Public Preview", "Repository Administration", "Settings"]
tags_normalized: ["access control", "administration", "compliance", "devops", "devops tools", "enterprise management", "enterprise management tools", "github", "github apps", "governance", "improvement", "news", "organization owners", "permissions", "public preview", "repository administration", "settings"]
---

Allison reports on a new GitHub setting that lets organization owners restrict who can install GitHub Apps, offering improved governance and compliance for repository management.<!--excerpt_end-->

# Controlling GitHub App Installations by Organization Owners

GitHub has released a new setting, available in public preview, that allows organization owners to specify which users are permitted to install GitHub Apps across their repositories.

Previously, any user with admin permissions—such as outside collaborators—could install GitHub Apps requesting repository-level permissions. This led to occasional unintended app installs and governance complications, especially within organizations with strict security or compliance needs.

## Key Features

- **Restricted app installation**: Organization owners can now prevent repository admins from installing GitHub Apps directly on repositories they manage.
- **Centralized control**: Only organization owners can install Apps for organization repositories; admins must request installations from owners.
- **Governance and compliance support**: The change strengthens controls, reduces unauthorized installations, and assists with compliance requirements.

## How to Enable

Organization owners can enable this feature in their organization's **Settings**:

- Go to **GitHub Apps** under the **Member privileges** tab.
- Toggle the checkbox that restricts repository administrators from installing GitHub Apps independently.

![GitHub App Installation Setting](https://github.com/user-attachments/assets/8e03ae94-b47b-46bb-a3c4-514ea726606c)

## Feedback

Questions and feedback may be directed to the [GitHub Community discussion](https://github.com/orgs/community/discussions/178756).

---
This update is especially relevant for DevOps practitioners and administrators managing large-scale or compliance-focused organizations on GitHub.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-17-block-repository-administrators-from-installing-github-apps-on-their-own-now-in-public-preview)
