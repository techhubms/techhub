---
layout: "post"
title: "Pushing a Whole Stack of Branches with a Single Git Command"
description: "Andrew Lock demonstrates how to streamline the workflow for stacked branches in Git by creating custom Git aliases. The post details the implementation and usage of a 'git push-stack' command to push multiple dependent branches at once, enhancing code review and collaboration efficiency."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/pushing-a-whole-stack-of-branches-with-a-single-git-command/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-05-20 09:00:00 +00:00
permalink: "/2025-05-20-Pushing-a-Whole-Stack-of-Branches-with-a-Single-Git-Command.html"
categories: ["DevOps"]
tags: ["Branch Management", "Code Review", "DevOps", "Force With Lease", "Git", "Git Aliases", "Git Push Stack", "GitHub", "Posts", "PR Workflow", "Repository Collaboration", "Stacked Branches"]
tags_normalized: ["branch management", "code review", "devops", "force with lease", "git", "git aliases", "git push stack", "github", "posts", "pr workflow", "repository collaboration", "stacked branches"]
---

In this post, Andrew Lock introduces a method for pushing an entire stack of Git branches using a custom alias, streamlining PR workflows and branch management for large features or incremental development.<!--excerpt_end-->

## Overview

When working with large features in Git, breaking down work into smaller, reviewable units (commits and pull requests) enhances review quality and team collaboration. However, managing and pushing multiple 'stacked branches' can be tedious. Andrew Lock shares a practical solution to this problem using custom Git aliases, culminating in a single command—`git push-stack`—to push all branches in a stack to a remote repository efficiently.

---

### What Are Stacked Branches?

Stacked branches are an approach where each logical part of a feature resides in its own branch and pull request (PR), with each PR building on the one before. This strategy aims to:

- Simplify code review by isolating units of functionality.
- Avoid large, unwieldy PRs that are difficult to review and more prone to conflicts.

For example:

- Multiple branches are created from a common base (e.g., `main`), each representing a part of a larger feature (`feature-xyz/part-1`, `feature-xyz/part-2`, etc.).
- PRs are set up so each merges into the branch below it, forming a "stack".

#### Illustrations

- Diagrams in the post depict branches stacked linearly, with each dependent on the previous.

---

### The Pain Point

Pushing several branches manually to a remote is error-prone and time-consuming. Previously, this was done by chaining push commands or using a custom alias for a single branch:

```bash
git push origin --force-with-lease feature/part-1;
git push origin --force-with-lease feature/part-2;
git push origin --force-with-lease feature/part-3;
```

This is tedious, especially with large stacks. The goal is a single command to push all branches in the stack.

---

### Solution: Custom Git Aliases

The core of the solution is a series of Git aliases that:

1. Identify the default branch (usually `main` on `origin`).
2. Determine the merge base between the stack and the default branch.
3. List all branches in the stack.
4. Push each branch to the remote in sequence.

#### Key Aliases

- **default-branch**: Gets the remote's default branch name.

  ```ini
  [alias]
  default-branch = "!git symbolic-ref refs/remotes/origin/HEAD | sed 's@^refs/remotes/origin/@@'"
  ```

- **merge-base-origin**: Gets the merge base for stacking logic.

  ```ini
  merge-base-origin ="!f() { git merge-base ${1-HEAD} origin/$(git default-branch); };f "
  ```

- **stack**: Lists all local branches atop the merge base.

  ```ini
  stack = "!f() { \
    BRANCH=${1-HEAD}; \
    MERGE_BASE=$(git merge-base-origin $BRANCH); \
    git log --decorate-refs=refs/heads \
            --simplify-by-decoration \
            --pretty=format:\"%(decorate:prefix=,suffix=,tag=,separator=%n)\" $MERGE_BASE..$BRANCH; \
  };f "
  ```

- **push-stack**: Pushes every branch found by `stack`.

  ```ini
  push-stack = "!f() { \
    BRANCH=${1-HEAD}; \
    git stack $BRANCH | xargs -I {} git push --force-with-lease origin {}; \
  };f "
  ```

#### Usage

- To push the *entire* stack:

  ```bash
  git push-stack
  # Or for a partial stack
  git push-stack feature/part-2
  ```

  This will push all branches up through the specified branch, as determined by their position in the stack.

---

### Implementation Notes

- Assumes the default remote is named `origin`.
- If running into errors with branch detection, set the remote default branch:

  ```bash
  git remote set-head origin --auto
  ```

- The design passes parameters for flexibility, e.g. specifying a top branch.

---

### Benefits of This Approach

- **Efficiency:** Pushing stacks is fast and reliable.
- **Reviewability:** Making PRs easier for teammates to review by breaking work into logical units.
- **Automation:** Reduces manual, repetitive commands.

### Summary

The post advocates for using stacked branches to enable incremental, high-quality reviews and shows how to automate stack management and pushing with Git aliases. The `git push-stack` workflow substantially improves developer experience for teams using stacked branch PR strategies.

---

### Copy-paste Setup

To add these aliases to your configuration, use:

```bash
git config --global alias.default-branch "!git symbolic-ref refs/remotes/origin/HEAD | sed 's@^refs/remotes/origin/@@'"
git config --global alias.merge-base-origin '!f() { git merge-base ${1-HEAD} origin/$(git default-branch); };f '
git config --global alias.stack '!f() { BRANCH=${1-HEAD}; MERGE_BASE=$(git merge-base-origin $BRANCH); git log --decorate-refs=refs/heads --simplify-by-decoration --pretty=format:\"%(decorate:prefix=,suffix=,tag=,separator=%n)\" $MERGE_BASE..$BRANCH; };f '
git config --global alias.push-stack '!f() { BRANCH=${1-HEAD}; git stack $BRANCH | xargs -I {} git push --force-with-lease origin {}; };f '
```

If you use stacked branches and reviewable PRs, this approach can save you time and simplify your workflow.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/pushing-a-whole-stack-of-branches-with-a-single-git-command/)
