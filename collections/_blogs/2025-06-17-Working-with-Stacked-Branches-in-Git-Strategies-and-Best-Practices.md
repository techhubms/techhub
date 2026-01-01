---
layout: "post"
title: "Working with Stacked Branches in Git: Strategies and Best Practices"
description: "Andrew Lock explores the advantages of using stacked branches and stacked pull requests (PRs) for larger feature development in Git. The post covers the rationale, practical benefits, and detailed methods—including interactive rebase and git-absorb—for managing changes in stacked branches."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/working-with-stacked-branches-in-git-part-1/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-06-17 09:00:00 +00:00
permalink: "/2025-06-17-Working-with-Stacked-Branches-in-Git-Strategies-and-Best-Practices.html"
categories: ["DevOps"]
tags: ["Automation", "Blogs", "Branch Management", "Code Review", "DevOps", "Git", "Git Absorb", "Interactive Rebase", "JetBrains Rider", "Modular Commits", "Pull Requests", "Software Development", "Stacked Branches", "Workflow"]
tags_normalized: ["automation", "blogs", "branch management", "code review", "devops", "git", "git absorb", "interactive rebase", "jetbrains rider", "modular commits", "pull requests", "software development", "stacked branches", "workflow"]
---

In this post, Andrew Lock delves into the use of stacked branches and PRs in Git, outlining practical workflows and tools for managing complex feature development efficiently.<!--excerpt_end-->

# Working with Stacked Branches in Git (Part 1)

*By Andrew Lock*

## Overview

Andrew Lock describes his preferred Git workflow for managing medium-to-large features with stacked branches and stacked pull requests (PRs). This approach divides work into smaller, reviewable units, with each branch depending on the previous one, making code reviews more manageable and maintaining a modular commit history. The post explores why this workflow is beneficial, its challenges, and key practices for amending commits within a stack, utilizing tools like interactive rebase and git-absorb.

---

## Why Use Stacked Branches?

Stacked branches in Git refer to an ordered set of branches where each branch builds upon the preceding one. For instance:

- **stack/a**: Branches from `main2` and contains commits `a1` and `a2`
- **stack/b**: Based on `stack/a`, contains commit `b1`
- **stack/c**: Based on `stack/b`, contains commits `c1` and `c2`

This results in a sequence that mirrors the structure of the feature being developed.

### Benefits

- **Smaller Scope**: Each PR is focused and easier to review.
- **Modular and Coherent**: Branches remain focused on specific tasks.
- **Guided Review Process**: Descriptions for each PR provide context and order for reviewers.

While it can be more work for the author to maintain a stack, the improved review experience leads to faster feedback and identifies issues more effectively. The workflow enables uninterrupted progress, as developers can move on to new branches while earlier ones await review.

> Note: While modular commits can achieve a similar result, reviewers often don't review commits individually. Stacked branches align better with GitHub's default review flow.

However, managing stacked branches is more complex than traditional flows, requiring familiarity with Git's capabilities.

---

## Managing Stacked Branches

Popular tools like [Graphite](https://graphite.dev/) support stacked workflows, but if not using such tools (e.g., due to OS restrictions or dependencies), you'll need to directly handle common scenarios:

- Reordering or amending commits within the stack
- Rebasing a stack after changes in `main`
- Pushing a rebased PR stack
- Rebasing after a PR merge

This post focuses on techniques for reordering or amending commits in a stack.

---

## Amending or Reordering Commits in a Stack

When working with stacked branches, you might need to correct earlier commits. Common scenarios include fixing errors, omissions, or updates in earlier sub-features. These cases are typically addressed using interactive rebase, sometimes in combination with `git-absorb`.

### Approach 1: Commit to HEAD and Interactive Rebase

Add the fixing commit to the top (HEAD), then perform an interactive rebase to move it into place. This is straightforward if there are no conflicts with subsequent changes.

#### Example Workflow

```bash
# Make the change and commit on the current HEAD

git rebase feature/part-1^1 -i --update-refs
```

Inside the editor, rearrange commits so the new commit is positioned before the relevant branch pointer. Save and exit to complete the rebase.

This works best when the amendment is isolated and won’t conflict with subsequent work.

### Approach 2: Using `git absorb` and Interactive Rebase

`git-absorb` automates the creation of `fixup!` commits aimed at earlier commits. These are incorporated during rebasing with the `--autosquash` flag.

#### Steps

1. Make and stage changes:

    ```bash
    git add .
    git absorb
    ```

2. Perform an interactive rebase with autosquash:

    ```bash
    git rebase main -i --autosquash
    ```

Rider and other GUIs also support these workflows.

#### Recommended Git Config

Enable these options by default:

```bash
git config --global rebase.autosquash true
git config --global rebase.updateRefs true
```

### Approach 3: Interactive Rebase, Pause, and Continue

When amendments are complex or prior approaches risk conflicts, use the `edit` option in interactive rebase for a "pause and fix" workflow:

1. Start the rebase:

    ```bash
    git rebase -i --root
    ```

2. In the editor, swap `pick` for `edit` on the target commit.
3. Amend or add commits as needed when Git pauses.
4. Continue the rebase:

    ```bash
    git rebase --continue
    ```

This method supports both amending a commit directly or adding a new commit after it and is especially useful when the change is complicated or touches multiple places in the stack.

---

## Summary

Stacked branches and stacked PRs benefit both reviewers—with more digestible, modular reviews—and authors, enabling continuous workflow and clearer code organization. Interactive rebasing, along with tools like git-absorb and autosquash, streamlines the correction or reordering of commits within these stacks. Mastering these workflows reduces the overhead and confusion often associated with managing complex development scenarios in Git.

Stay tuned for the next post in this series, covering further scenarios like rebasing stacks after merges and pushing an entire stack to a remote repository.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/working-with-stacked-branches-in-git-part-1/)
