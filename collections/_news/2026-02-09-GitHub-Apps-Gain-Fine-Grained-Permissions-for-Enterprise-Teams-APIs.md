---
layout: "post"
title: "GitHub Apps Gain Fine-Grained Permissions for Enterprise Teams APIs"
description: "This update introduces fine-grained 'enterprise teams' permissions for GitHub Apps, enabling secure access to Enterprise Teams API endpoints. Enterprise administrators can use these permissions to manage teams programmatically without relying on classic personal access tokens, allowing for enhanced security and scalable team management."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-09-github-apps-can-now-utilize-public-preview-enterprise-teams-apis-via-fine-grained-permissions"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-09 14:03:31 +00:00
permalink: "/2026-02-09-GitHub-Apps-Gain-Fine-Grained-Permissions-for-Enterprise-Teams-APIs.html"
categories: ["DevOps"]
tags: ["Access Control", "API Permissions", "API Security", "App Integration", "Automation", "DevOps", "Enterprise Management", "Enterprise Management Tools", "Enterprise Teams API", "Fine Grained Permissions", "GitHub Apps", "GitHub Enterprise", "Improvement", "News", "Team Management"]
tags_normalized: ["access control", "api permissions", "api security", "app integration", "automation", "devops", "enterprise management", "enterprise management tools", "enterprise teams api", "fine grained permissions", "github apps", "github enterprise", "improvement", "news", "team management"]
---

Allison explains GitHub’s introduction of fine-grained permissions for Apps accessing Enterprise Teams APIs, providing enterprise administrators with secure, scalable team management options.<!--excerpt_end-->

# GitHub Apps Gain Fine-Grained Permissions for Enterprise Teams APIs

GitHub has rolled out fine-grained `enterprise teams` permissions for GitHub Apps, empowering Enterprise administrators to securely manage their teams at scale. Prior to this update, accessing Enterprise Teams API endpoints required the use of classic personal access tokens associated with individual users—a method with increased risk profiles and lower automation compatibility.

With the new permissions model:

- **GitHub Apps** can request `read` or `write` access specifically for enterprise-level teams.
- Administrators can now create, update, and delete teams through the API using Apps, increasing automation and compliance.
- Operations are safer and easier to audit compared to operations done with user tokens.

## How It Works

- **Read and Write Access**: Apps can be scoped for read-only (view teams) or write (manage teams) access.
- **Programmatic Management**: Enables organizations to automate team management, synchronize with external systems, or onboard/offboard users at scale.
- **Security Enhancement**: Using app credentials minimizes the risk associated with distributing and managing personal access tokens.

## Learn More

- [GitHub Apps authentication](https://docs.github.com/enterprise-cloud@latest/apps/creating-github-apps/authenticating-with-a-github-app/about-authentication-with-a-github-app)
- [Enterprise Teams API endpoints](https://docs.github.com/enterprise-cloud@latest/rest/enterprise-teams?apiVersion=2022-11-28)
- [Community Discussion](https://github.com/orgs/community/discussions/177040)

Enterprise GitHub customers are encouraged to update their automation and app integration strategies to leverage these improved permissions.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-09-github-apps-can-now-utilize-public-preview-enterprise-teams-apis-via-fine-grained-permissions)
