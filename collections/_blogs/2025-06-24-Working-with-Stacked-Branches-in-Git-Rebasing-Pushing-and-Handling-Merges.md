---
layout: "post"
title: "Working with Stacked Branches in Git: Rebasing, Pushing, and Handling Merges"
description: "Andrew Lock explains advanced workflows when working with stacked branches in Git, focusing on how to rebase after changes to 'main', push rebased stacks efficiently, and handle stack rebases after merging. The post provides practical commands, tips on --update-refs, and solutions for common challenges."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/working-with-stacked-branches-in-git-part-2/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-06-24 09:00:00 +00:00
permalink: "/blogs/2025-06-24-Working-with-Stacked-Branches-in-Git-Rebasing-Pushing-and-Handling-Merges.html"
categories: ["DevOps"]
tags: ["Branch Management", "Continuous Integration", "DevOps", "Feature Branches", "Git", "Git Alias", "Merge Conflicts", "Blogs", "PR Workflow", "Push Stack", "Rebase", "Stacked Branches", "Update Refs"]
tags_normalized: ["branch management", "continuous integration", "devops", "feature branches", "git", "git alias", "merge conflicts", "blogs", "pr workflow", "push stack", "rebase", "stacked branches", "update refs"]
---

In this detailed post, Andrew Lock discusses workflows for managing stacked branches in Git. He provides advanced advice on rebasing, merging, and pushing branch stacks to streamline feature development and improve code review processes.<!--excerpt_end-->

## Working with Stacked Branches in Git: Advanced Scenarios

**Author:** Andrew Lock

In this follow-up to [my previous post](/working-with-stacked-branches-in-git-part-1/) on stacked branches, I’ll delve into advanced scenarios including rebasing stacks after changes to `main`, pushing rebased stacks, and rebasing after a branch in a stack is merged. These practices help maintain modular PRs, support unblocked development, and streamline review processes.

### Why Use Stacked Branches?

Stacked branches are a technique where you create a linear stack of branches, each one depending on the preceding branch. For example, you might have three branches—`stack/a`, `stack/b`, and `stack/c`—where each subsequent branch includes changes from the one before:

```
main2
  |
stack/a  (a1, a2)
  |
stack/b  (b1)
  |
stack/c  (c1, c2)
```

**Benefits for Reviewers & Teams:**

- **Smaller, modular PRs:** Each PR covers a smaller, coherent set of changes.
- **Easier review:** Modular branches make it clearer to follow intentions and catch issues.
- **Parallel progress:** Developers can continue on later branches while waiting for reviews or merges on earlier ones.

While creating stacked branches requires more effort than lumping all changes into a single branch, it pays off in more manageable, reviewable pull requests and quicker, more effective collaboration.

> Modular commits could be used similarly, but most reviewers view PRs "all at once" rather than commit-by-commit. Stacked branches with individual PRs align better with modern tooling defaults like GitHub.

### Key Challenges with Stacked Branches

Some of the common scenarios include:

- Reordering or amending commits within a stack
- Rebasing the stack after changes to `main`
- Pushing a rebased stack to remotes
- Rebasing a stack after a PR is merged (especially if squashed)

#### Rebasing Stacked Branches After Changes on `main`

Long-lived feature stacks will inevitably need to integrate updates from `main`. Rebasing is generally preferable to merging in this context because rebasing keeps the commit history linear and avoids complex merge graphs.

**Without `--update-refs`:**

1. Rebase the entire top branch onto `main` with `git rebase main`.
2. Manually move all the branch references below it to point to the new commits using `git branch --force` for each.

**With `--update-refs`:**
Git 2.38+ introduces the `--update-refs` option, which will automatically update the stack’s branch references after a rebase. You can enable it globally:

```bash
git config --global rebase.updateRefs true
```

This allows you to simply run:

```bash
git rebase main
```

…and all your stack’s branches will move appropriately, greatly reducing manual intervention and potential errors.

#### Pushing a Stack of Rebased PRs

After rebasing a stack, you need to push all the affected branches. Instead of pushing branches individually with `git push origin --force-with-lease <branch>`, you can configure a Git alias to push the full stack with a single command.

**Git alias setup for pushing stacks:**

```bash
git config --global alias.default-branch "!git symbolic-ref refs/remotes/origin/HEAD | sed 's@^refs/remotes/origin/@@'"
git config --global alias.merge-base-origin '!f() { git merge-base ${1-HEAD} origin/$(git default-branch); };f '
git config --global alias.stack '!f() { BRANCH=${1-HEAD}; MERGE_BASE=$(git merge-base-origin $BRANCH); git log --decorate-refs=refs/heads --simplify-by-decoration --pretty=format:\"%(decorate:prefix=,suffix=,tag=,separator=%n)\" $MERGE_BASE..$BRANCH; };f '
git config --global alias.push-stack '!f() { BRANCH=${1-HEAD}; git stack $BRANCH | xargs -I {} git push --force-with-lease origin {}; };f '
```

Now you can simply use:

```bash
git push-stack
```

to push all branches in the currently checked out stack to your remote.

#### Rebasing a Stack After a PR is Merged (with Squashing)

If a PR from the stack is merged (often squashed), Git may not know how to directly match the local branch’s commits to `main`, leading to conflicts if you naïvely run `git rebase origin/main`.

To properly rebase the remaining stack after a branch is squashed-and-merged:

```bash
git rebase feature/part-1 --onto origin/main
```

- `feature/part-1` is the bottom of the remaining stack
- `origin/main` is the new base

With `--update-refs` enabled, downstream branch references will be automatically handled.

After rebasing, push the updated stack and, if needed, delete the merged local branch with:

```bash
git push-stack
git branch -D feature/part-1
```

### Summary

Stacked branches provide a powerful workflow for complex features, making code review and modularity much more feasible. To manage the complexities:

- Use `--update-refs` for rebasing
- Configure push-stack alias for batch pushing
- Apply `git rebase <base> --onto <onto>` for merges with squashing

These tools and workflows streamline collaborative and individual development on medium-to-large features.

---

*If you have questions or want to share additional tips on managing stacked branches, please comment on the post or reach out to me.*

*Andrew Lock | .NET Escapades*

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/working-with-stacked-branches-in-git-part-2/)
