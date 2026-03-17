---
title: 'Git Mirroring During Migrations: `--all` vs `--mirror`'
section_names:
- devops
author: Emanuele Bartolesi
feed_name: Emanuele Bartolesi's Blog
primary_section: devops
tags:
- Blogs
- Branch Deletion
- CI Pipelines
- DevOps
- Force Push
- Git
- Git Mirroring
- Git Notes
- Git Push
- Git Push   All
- Git Push   Mirror
- Git Push   Tags
- GitHub
- Gitlab
- Refs Namespace
- Refs/heads
- Refs/tags
- Remote Repositories
- Remote Tracking Branches
- Repository Migration
- Synchronization
- Tag Management
date: 2026-03-16 07:00:00 +00:00
external_url: https://dev.to/playfulprogramming/git-mirroring-during-migrations-all-vs-mirror-2i4h
---

Emanuele Bartolesi explains how to keep repositories in sync during a Git migration (for example, GitLab to GitHub), and why `git push --all` + `--tags` is not the same as `git push --mirror`, especially when it comes to non-branch refs and deletions.<!--excerpt_end-->

# Git Mirroring During Migrations: `--all` vs `--mirror`

During a Git migration, the new platform is rarely switched on overnight.

In many real scenarios, **two systems must coexist for a while**. For example:

- repositories move from **GitLab to GitHub**
- CI pipelines still run in **GitLab**
- developers start working in **GitHub**

In this transition phase, the repositories must stay **synchronized between both platforms**.

One common solution is **repository mirroring**, where changes from one repository are pushed to another remote so both systems stay aligned.

Two approaches are often used for this:

```bash
git push gitlab --force --all
git push gitlab --force --tags
```

or:

```bash
git push gitlab --force --mirror
```

At first glance, these commands may look equivalent. They are not.

## The common approach: pushing branches and tags

A common way to keep two repositories in sync is pushing **all branches and tags** to the second remote.

```bash
git push gitlab --force --all
git push gitlab --force --tags
```

These two commands are often used together during migrations.

### `git push --all`

```bash
git push gitlab --force --all
```

This command pushes **all local branches** to the remote.

Important details:

- It pushes every branch under `refs/heads/*`
- It does **not** push tags
- It does **not** push other reference types

So after this command runs, the remote repository will contain the same branches as the local repository.

### `git push --tags`

```bash
git push gitlab --force --tags
```

This command pushes **all tags** under `refs/tags/*`.

Tags are often used for:

- releases
- versioning
- deployment markers

Since `--all` does not include tags, they must be pushed separately.

### What this approach synchronizes

Using these two commands ensures that:

- all **branches** are synchronized
- all **tags** are synchronized

For many teams, this is already enough to keep two repositories aligned during a migration.

However, this approach **does not synchronize the entire repository state**. Some Git references are still missing.

This is where `--mirror` behaves differently.

## The full mirror option

Git also provides a command designed specifically for repository mirroring:

```bash
git push gitlab --force --mirror
```

This command behaves differently from pushing branches and tags separately.

Instead of pushing selected references, **`--mirror` pushes every reference under `refs/*`** from the local repository to the remote.

This includes:

- branches (`refs/heads/*`)
- tags (`refs/tags/*`)
- remote-tracking branches
- notes
- any other custom references

Another important behavior is that `--mirror` **synchronizes deletions**.

If a reference exists on the remote but **no longer exists locally**, `--mirror` will remove it from the remote as well. The goal is to make the remote repository **an exact replica** of the source repository.

In other words:

- `--all` + `--tags` copies **branches and tags**
- `--mirror` copies **the entire reference namespace**

This makes `--mirror` the closest thing to a **true Git repository mirror**.

## Quick recommendation

Both approaches can work during a migration, but they serve slightly different goals.

### Use `--all` + `--tags` when

```bash
git push gitlab --force --all
git push gitlab --force --tags
```

This approach is usually enough if you only need to synchronize:

- branches
- tags

It is also **safer** in environments where the target repository may contain additional references created by tooling, CI systems, or platform features.

Since it does not delete references on the remote, it reduces the risk of accidentally removing something important.

### Use `--mirror` when

```bash
git push gitlab --force --mirror
```

This option is appropriate when you want the destination repository to be a **complete mirror** of the source.

Typical cases include:

- temporary mirrors during platform migrations
- backup repositories
- strict synchronization between two Git servers

However, `--mirror` is more aggressive. It will **overwrite and delete references** on the remote to match the source exactly.

## Appendix: GitHub Copilot quota visibility in VS Code

![Screenshot showing GitHub Copilot quota visibility in VS Code](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fsqzk5hamyymcmuh515a4.png)

If you use GitHub Copilot and ever wondered:

- what plan you’re on
- whether you have limits
- how much premium quota is left
- when it resets

The author built a small VS Code extension called **Copilot Insights** to show Copilot **plan and quota status** directly inside VS Code.

VS Code Marketplace: https://marketplace.visualstudio.com/items?itemName=emanuelebartolesi.vscode-copilot-insights


[Read the entire article](https://dev.to/playfulprogramming/git-mirroring-during-migrations-all-vs-mirror-2i4h)

