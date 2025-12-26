---
layout: "post"
title: "How the GitHub CLI Enables Triangular Workflows for Advanced Git Collaboration"
description: "This post by Tyler McGoffin details recent enhancements in the GitHub CLI, introducing support for triangular Git workflows. It explains the fundamentals of Git refs and workflows, provides practical configuration steps, and showcases how the CLI's `gh pr` commands now align with these advanced collaboration scenarios."
author: "Tyler McGoffin"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/open-source/git/how-the-github-cli-can-now-enable-triangular-workflows/"
viewing_mode: "external"
feed_name: "GitHub Engineering Blog"
feed_url: "https://github.blog/engineering/feed/"
date: 2025-04-25 16:00:37 +00:00
permalink: "/news/2025-04-25-How-the-GitHub-CLI-Enables-Triangular-Workflows-for-Advanced-Git-Collaboration.html"
categories: ["DevOps"]
tags: ["Branch Management", "CLI Tools", "Configuration", "DevOps", "Engineering", "Git", "Git Remotes", "GitHub CLI", "News", "Open Source", "Open Source Collaboration", "Pull Requests", "Pullref", "Pushref", "Triangular Workflows", "Workflow Automation"]
tags_normalized: ["branch management", "cli tools", "configuration", "devops", "engineering", "git", "git remotes", "github cli", "news", "open source", "open source collaboration", "pull requests", "pullref", "pushref", "triangular workflows", "workflow automation"]
---

Tyler McGoffin discusses the GitHub CLI's new support for triangular workflows, providing advanced collaboration capabilities for developers. Explore Git fundamentals, workflow configuration, and how the updated CLI simplifies pull request and branch management.<!--excerpt_end-->

# How the GitHub CLI Enables Triangular Workflows for Advanced Git Collaboration

**By Tyler McGoffin**

Most developers are familiar with the standard Git workflow, where a branch is created, changes are made, and those changes are pushed back to the same branch on the main repository—a process known as the centralized workflow. This method is straightforward and widely adopted across projects.

However, certain collaborative scenarios require pulling changes from different branches or repositories directly into your feature branch to stay updated. Simultaneously, you may want to push your changes to your own branch. This two-directional process is known as a **triangular workflow**.

## What Are Triangular Workflows?

Triangular workflows typically occur when you fork a repository, work on your fork, and then open a pull request back to the original source repository. While this process works seamlessly on GitHub's web interface, it previously required extra manual configuration in the GitHub CLI (`gh`).

With the release of [GitHub CLI v2.71.2](https://github.com/cli/cli/releases/tag/v2.71.2), the CLI now natively supports triangular workflows, allowing `gh pr` commands to work in conjunction with your Git configurations. This improvement streamlines the development experience for both centralized and complex fork-based workflows.

## Git Fundamentals Overview

To understand how these workflows operate, it is helpful to review several core Git concepts:

- **Ref (Reference)**: A pointer to a repository and branch, formed by a remote name (like `origin` or `upstream`) and a branch name. `origin/branch` refers to a branch on the remote named `origin`.
- **Push and Pull**: Terms that describe sending (`push`) or receiving (`pull`) changes. In context, a **headRef** is the source remote/branch pushing changes, and a **baseRef** is the destination remote/branch pulling those changes.
- **Pull Requests**: Proposals to integrate changes from one ref to another, acting as a pause for code or automated review before merging.

### Illustration Examples

Several diagrams (see original post) showcase the flow between local, origin, and upstream branches, differentiating between typical and triangular workflows.

## Common Git Workflow Patterns

- **Centralized Workflow**: The default for most users, pushing and pulling to/from the same remote/branch (e.g., `origin/branch`)
- **Triangular Workflow**: Pushing and pulling from *different* refs. A common scenario is updating your local feature branch with changes from the upstream default branch, while pushing changes to your fork.

#### Triangular Workflows and Forks

When working on a fork, remote names like `origin` (fork) and `upstream` (source project) are typically used. In this context:

- **PushRef (push remote/ref)**: Where your changes are sent (commonly your fork).
- **PullRef (pull remote/ref)**: Where you get updates from (commonly the upstream default branch).

## Git Configuration for Triangular Workflows

Customizing Git config files (`.git/config` or `.gitconfig`) enables triangular setups. Key configurations include:

- **Basic Centralized Example**

  ```ini
  [remote "origin"]
    url = https://github.com/OWNER/REPO.git
    fetch = +refs/heads/*:refs/remotes/origin/*
  [branch "default"]
    remote = origin
    merge = refs/heads/default
  [branch "branch"]
    remote = origin
    merge = refs/heads/branch
  ```

- **Triangular Branch Example**

  ```ini
  [branch "branch"]
    remote = origin
    merge = refs/heads/default
  ```

  This setup pulls from `origin/default` but pushes to `origin/branch`.

- **Triangular Fork Example (Multiple Remotes)**

  ```ini
  [remote "upstream"]
    url = https://github.com/ORIGINALOWNER/REPO.git
  [remote "origin"]
    url = https://github.com/FORKOWNER/REPO.git
  [branch "branch"]
    remote = upstream
    merge = refs/heads/default
    pushremote = origin
  ```

  This configuration pulls from `upstream/default` and pushes to `origin/branch`.

- **Repository-wide Setting with `remote.pushDefault`**

  ```ini
  [remote]
    pushDefault = origin
  [branch "branch"]
    remote = upstream
    merge = refs/heads/default
  ```

  All branches default pushing to `origin` by default.

## GitHub CLI Enhancements

Previously, the GitHub CLI's `gh pr` commands did not resolve push and pull refs in the same way as Git. With the new update:

- `gh pr` now respects the branch's pushRef (via `@{push}` revision syntax), falling back to `pushremote`, then `remote.pushDefault` in the config.
- This means `gh pr` commands will "just work" if your local Git push/pull commands work as desired.

## Community Contributions

This improvement took years and significant community input. Shout-outs are given to users such as @Frederick888, @benknoble, @phil-blain, @neutrinoceros, @rd-yan-farba, @pdunnavant, and @cs278 for their contributions to bug reports, feedback, and reviews.

> CLI native support for triangular workflows was 4.5 years in the making, and we’re proud to have been able to provide this update for the community.

The GitHub CLI Team—@andyfeller, @babakks, @bagtoad, @jtmcg, @mxie, @RyanHecht, and @williammartin

---

**Pro Tips:**

- The ordering of recognized remotes in the CLI (`upstream`, `github`, `origin`, then others)
- Use `gh repo set-default [<repository>]` to override default remote resolution
- For problems, open issues in the GitHub CLI open source repository

This post appeared first on "GitHub Engineering Blog". [Read the entire article here](https://github.blog/open-source/git/how-the-github-cli-can-now-enable-triangular-workflows/)
