---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/how-to-safely-remove-secrets-from-your-git-history-the-right-way/ba-p/4464722
title: How to Safely Remove Secrets from Your Git History (The Right Way)
author: Sakshi_Gupta22
feed_name: Microsoft Tech Community
date: 2025-10-28 11:05:12 +00:00
tags:
- .gitignore
- API Keys
- Backup
- DevOps Best Practices
- Force Push
- Git
- Git Filter Repo
- GitHub
- History Rewriting
- Incident Response
- Secret Management
- Team Coordination
- Token Rotation
- Version Control
section_names:
- devops
- security
---
Sakshi_Gupta22 offers a thorough walkthrough on how developers can use git filter-repo to scrub sensitive files from Git repositories, enhancing security and reducing the risk of leaked secrets.<!--excerpt_end-->

# How to Safely Remove Secrets from Your Git History (The Right Way)

Accidental secret commits are a common and serious security risk in software development. Even after deleting sensitive files, secrets like API keys and credentials can persist in a repository’s history, putting systems at risk of long-term exposure. This guide walks through a practical, step-by-step process to eradicate secrets from Git history, using modern tools and best practices.

## Why Removing Secrets from Git History Is Critical

- **Security breaches**: Attackers can scan old commits for secrets, gaining unauthorized access.
- **Persistent exposure**: Deleted secrets still linger in the full history, potentially visible in forks and caches.
- **Automation risks**: Exposed tokens can trigger vulnerabilities in automated processes and pipelines.

Deleting a file is insufficient—you must rewrite history to remove all traces.

## Step-by-Step Guide: Using git filter-repo

**Warning:** Rewriting Git history is destructive. Always make a full backup first and coordinate with any team members: everyone must re-clone or hard-reset after the cleanup.

### 1. Create a Safety Backup

- Clone the repository to create a safe restore point:

  ```bash
  git clone https://github.com/org/repo.git repo-backup
  ```

- Alternatively, download a ZIP snapshot from GitHub’s UI.
- **Do not skip this step**: Once you rewrite history, the old commit hashes are gone for good.

### 2. Prepare a Mirror Clone

- Mirror clones are ideal for these operations:

  ```bash
  git clone --mirror https://github.com/org/repo.git repo-clean.git
  cd repo-clean.git
  ```

### 3. Install git filter-repo

- [git-filter-repo](https://github.com/newren/git-filter-repo) is the recommended tool (replacing git filter-branch).
- Install on macOS with Homebrew:

  ```bash
  brew install git-filter-repo
  ```

- Or cross-platform using pip:

  ```bash
  pip install git-filter-repo
  ```

### 4. Remove the Secret(s)

- To strip a specific file from every commit:

  ```bash
  git filter-repo --path "config/secrets.json" --invert-paths
  ```

- For multiple files:

  ```bash
  git filter-repo --invert-paths --path ".env" --path "settings.py"
  ```

- The `--invert-paths` flag ensures these files are completely removed from all historical commits.

### 5. Verify the Remote

- Sometimes the repository's remote URL is lost during rewriting. Check and re-add if necessary:

  ```bash
  git remote -v
  git remote set-url origin https://github.com/org/repo.git
  ```

### 6. Push the Cleaned History

- Replace history on the remote (force-push required):

  ```bash
  git push --force --all
  git push --force --tags
  ```

- **Note:** Tell all collaborators to re-clone or hard-reset to avoid merge conflicts.

### 7. Refresh Local Clones

- Developers must hard reset their local clones.

  ```bash
  cd repo
  git fetch origin
  git reset --hard origin/main
  ```

- Never merge after a force-push; always reset.

### 8. Verify the Secret Is Gone

- Check that the secret is fully expunged:

  ```bash
  git log --all -- config/secrets.json
  ```

- If nothing is returned, your history is clean. You can remove the temporary mirror repository now:

  ```bash
  rm -rf repo-clean.git
  ```

## Final Steps & Takeaways

- **Rotate exposed keys immediately:** Assume any committed secret is compromised.
- **Add sensitive files to .gitignore** to prevent future exposure.
- **Enable secret scanning** in repos as a preventive measure.
- **Communicate with your team** before and after force-pushing changes.

By following this process, you ensure sensitive data is thoroughly scrubbed from your source control history, minimizing security risk to your environment.

*Original guide by Sakshi_Gupta22, posted October 28, 2025.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/how-to-safely-remove-secrets-from-your-git-history-the-right-way/ba-p/4464722)
