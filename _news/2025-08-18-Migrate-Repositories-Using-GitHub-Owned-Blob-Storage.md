---
layout: "post"
title: "Migrate Repositories Using GitHub-Owned Blob Storage"
description: "This announcement introduces a public preview feature allowing users to migrate repositories to GitHub Enterprise Cloud (GHEC) with the GitHub Enterprise Importer (GEI) using GitHub-owned blob storage. The process now removes the need for user-managed storage keys, providing a streamlined approach to repository migration via command-line tools."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-18-migrate-repositories-with-github-owned-blob-storage"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-18 17:35:15 +00:00
permalink: "/2025-08-18-Migrate-Repositories-Using-GitHub-Owned-Blob-Storage.html"
categories: ["DevOps"]
tags: ["Bitbucket Migration", "Cloud Storage", "Command Line Tools", "DevOps", "Enterprise Migration", "GEI", "Gh Bbs2gh", "Gh Gei", "GitHub Enterprise Cloud", "GitHub Enterprise Importer", "GitHub Owned Blob Storage", "GitLab Migration", "News", "Public Preview", "Repository Migration"]
tags_normalized: ["bitbucket migration", "cloud storage", "command line tools", "devops", "enterprise migration", "gei", "gh bbs2gh", "gh gei", "github enterprise cloud", "github enterprise importer", "github owned blob storage", "gitlab migration", "news", "public preview", "repository migration"]
---

Allison outlines the new public preview feature for migrating repositories to GitHub Enterprise Cloud using GitHub-owned blob storage, simplifying the migration process for organizations.<!--excerpt_end-->

# Migrate Repositories Using GitHub-Owned Blob Storage

GitHub now offers a public preview that allows users to migrate repositories to GitHub Enterprise Cloud (GHEC) through the GitHub Enterprise Importer (GEI), utilizing GitHub-owned blob storage instead of requiring personal storage account access keys.

## Key Features

- **No More Storage Keys Needed**: Migrations no longer require providing GEI with shared access keys to your own storage account.
- **Direct Uploads Supported**: Repository archives can be uploaded directly to github.com or ghe.com.
- **Command-Line Tools**: Migration can be started using the `gh gei` and `gh bbs2gh` command line extensions with the `--use-github-storage` flag.

```bash
# Example usage

$ gh gei migrate --use-github-storage
```

![Repository migrations using `gh gei --use-github-storage` in the terminal](https://github.com/user-attachments/assets/5da7b312-2c43-4f1e-a6fe-1a92b3db05de)

## Supported Migrations

- **GitHub Enterprise Server → GitHub Enterprise Cloud**: [Migration documentation](https://docs.github.com/enterprise-cloud@latest/migrations/using-github-enterprise-importer/migrating-between-github-products/migrating-repositories-from-github-enterprise-server-to-github-enterprise-cloud)
- **Bitbucket Server → GitHub Enterprise Cloud**: [Migration documentation](https://docs.github.com/enterprise-cloud@latest/migrations/using-github-enterprise-importer/migrating-from-bitbucket-server-to-github-enterprise-cloud/migrating-repositories-from-bitbucket-server-to-github-enterprise-cloud)

## Additional Information

- For GitLab repository migrations using GitHub-owned blob storage, contact [Expert Services](https://github.com/services).
- This feature streamlines and secures the migration process for enterprises adopting Github as a primary platform.

---

For step-by-step guides, refer to the documentation links above or reach out to GitHub support for further assistance.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-18-migrate-repositories-with-github-owned-blob-storage)
