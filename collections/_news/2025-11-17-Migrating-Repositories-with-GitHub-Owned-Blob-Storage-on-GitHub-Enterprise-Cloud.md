---
external_url: https://github.blog/changelog/2025-11-17-migrating-repositories-with-github-owned-blob-storage-is-now-generally-available
title: Migrating Repositories with GitHub-Owned Blob Storage on GitHub Enterprise Cloud
author: Allison
feed_name: The GitHub Blog
date: 2025-11-17 20:36:15 +00:00
tags:
- Bitbucket Server
- Blob Storage
- Cloud Migration
- Command Line Tools
- DevOps Automation
- Enterprise Management
- Enterprise Management Tools
- Expert Services
- GEI
- Gh Bbs2gh
- Gh Gei
- GitHub Enterprise Cloud
- GitHub Enterprise Importer
- GitHub Owned Storage
- GitLab
- Improvement
- Repository Migration
section_names:
- devops
primary_section: devops
---
Allison announces that GitHub-owned blob storage is now generally available for repository migrations to GitHub Enterprise Cloud, streamlining the process for enterprise administrators and DevOps engineers.<!--excerpt_end-->

# Migrating Repositories with GitHub-Owned Blob Storage on GitHub Enterprise Cloud

GitHub Enterprise Cloud (GHEC) users can now take advantage of GitHub-owned blob storage when migrating repositories with GitHub Enterprise Importer (GEI). This means shared access keys to a personal storage account are no longer required, simplifying migration workflows and increasing security.

## Key Features and Workflow

- **Direct Archive Uploads:** Repositories can be archived and uploaded directly to GitHub.com or GHE.com using GitHub-owned storage.
- **Simplified Access:** Eliminates the need for shared access keys to external Azure or cloud storage accounts.
- **Command Line Tools:** Migrations are supported through `gh gei` and `gh bbs2gh` CLI extensions. Simply use the `--use-github-storage` flag to enable GitHub storage for the migration process.

![Repository migrations CLI screenshot](https://github.com/user-attachments/assets/5da7b312-2c43-4f1e-a6fe-1a92b3db05de)

## Supported Migration Scenarios

- **GitHub Enterprise Server to GitHub Enterprise Cloud:** See the [documentation](https://docs.github.com/enterprise-cloud@latest/migrations/using-github-enterprise-importer/migrating-between-github-products/migrating-repositories-from-github-enterprise-server-to-github-enterprise-cloud) for step-by-step instructions.
- **Bitbucket Server to GitHub Enterprise Cloud:** Refer to the [Bitbucket migration guide](https://docs.github.com/enterprise-cloud@latest/migrations/using-github-enterprise-importer/migrating-from-bitbucket-server-to-github-enterprise-cloud/migrating-repositories-from-bitbucket-server-to-github-enterprise-cloud).
- **GitLab Migrations:** Interested users are advised to contact [Expert Services](https://github.com/services) for guidance on GitLab to GitHub migrations using GitHub-owned storage.

## How to Start a Migration

To begin, use the following CLI extensions and flag:

```shell
# Using GitHub Enterprise Importer to migrate with GitHub-owned blob storage

gh gei migrate --use-github-storage

# Migrating from Bitbucket Server

gh bbs2gh migrate --use-github-storage
```

Detailed instructions are in the GitHub documentation linked above.

## Benefits

- **Reduced Complexity:** Removes the friction of managing external storage credentials.
- **Security:** Ensures repository data is managed directly within GitHub's cloud environment.
- **Flexibility:** Supports migrations from several source platforms via official tools and guides.

For organizations seeking streamlined repository migration experiences, this new feature improves security and operational efficiency.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-17-migrating-repositories-with-github-owned-blob-storage-is-now-generally-available)
