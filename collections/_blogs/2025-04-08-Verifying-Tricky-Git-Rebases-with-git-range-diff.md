---
external_url: https://andrewlock.net/verifiying-tricky-git-rebases-with-range-diffs/
title: Verifying Tricky Git Rebases with git range-diff
author: Andrew Lock
feed_name: Andrew Lock's Blog
date: 2025-04-08 09:00:00 +00:00
tags:
- Branch Management
- Commit Comparison
- Continuous Integration
- Development Workflow
- Git
- Git Rebase
- Merge Conflicts
- Minimal API
- Range Diff
- Version Control
- DevOps
- Blogs
- .NET
section_names:
- devops
- dotnet
primary_section: dotnet
---
In this thorough post, Andrew Lock introduces the git range-diff feature for verifying and understanding tricky git rebases. He uses a practical .NET minimal API example to walk readers through the tool’s capabilities, output, and its potential challenges.<!--excerpt_end-->

# Verifying Tricky Git Rebases with git range-diff

*By Andrew Lock*

## Introduction

In this post, Andrew Lock examines the `git range-diff` feature, available since git 2.19. He explains its purpose, output format, and demonstrates its use with a small .NET minimal API project, especially in the context of rebasing branches and resolving merge conflicts.

## What Is `git range-diff`?

Andrew expresses his appreciation for `git rebase` for rearranging and cleaning up commits, or incorporating changes from `main` into a feature branch. However, rebasing can sometimes leave developers uncertain about whether the result matches their intentions, especially after complex interactive rebases or when resolving several merge conflicts.

While there are various (often sub-optimal) ways to check the correctness of a rebase, Andrew finds that `git range-diff` is tailored for this scenario. Unlike `git diff`, which compares snapshots at two points, `git range-diff` compares two *ranges* of commits. Think of it as a "diff-of-diffs": it compares the patch output for two sets of commits, making it possible to compare the state of a commit stack before and after rebasing.

If a rebase only squashed or reordered commits, the diffs would likely be identical. However, if the rebase introduced changes (e.g., resolving conflicts or rebasing onto different commits), `git range-diff` reveals those.

## Using `git range-diff`

The basic (and most useful) syntax is:

```bash
git range-diff base1..head1 base2..head2
```

Suppose your commit tree looks like:

```
   h-i-head2
  /
a-b-c-base1-d-e-base2
  \
   f-g-head1
```

Here's how `git range-diff` works:

- Generates a patch for `git diff base1..head1` (`base1`, `f`, `g`, `head1`)
- Generates a patch for `git diff base2..head2` (`base2`, `h`, `i`, `head2`)
- Diffs the two patches

`git range-diff` not only compares content, but also commit order and metadata (e.g., commit messages)—which can make its output complex to interpret.

## Understanding the Output Format

Examining an output sample (adapted from a GitHub blog post):

```text
1| 2 |3|4| 5 | 6
------------------------------
2: 8d6b31f = 1: d672a8f add README.md
1: 3386b9a = 2: 02c0d21 add hello/goodbye world
3: bc293cc ! 3: 251b232 hello: fix typo
-: ------- > 4: a835e18 goodbye: add missing newline
```

Columns:

1. Left-side commit position in the range
2. Left-side short commit hash
3. Equality marker (`=`: equal, `!`: differs, `>`: only exists on the right, `<`: only on the left)
4. Right-side commit position
5. Right-side short commit hash
6. Commit message

Example interpretation:

- The output shows whether commits are reordered, added, removed, or changed.
- Changed commits (indicated by `!`) show further diffs—both in commit messages and file changes.

### Interpreting Double Prefixes in Diffs

Range-diff may display lines like:

```diff
-+ printf("Hello world");
++ printf("Hello world\n");
```

The first character signifies which *side* has a change; the second character shows what kind of change. A summary table was provided:

| Prefix | Left Commit | Right Commit |
| ------ | ----------- | ------------ |
| (none) | No change   | No change    |
| `-`    | Removed     | Removed      |
| `+`    | Added       | Added        |
| `+-`   | No change   | Removes      |
| `++`   | No change   | Adds         |
| `--`   | Removed     | "Un-removes" |
| `-+`   | Added       | "Un-adds"    |

Remembering these can be tricky, but essential for parsing output.

To restrict output to commit lists only (skipping patch diffs):

```bash
git range-diff --no-patch base1..head1 base2..head2

# or

git range-diff -s base1..head1 base2..head2
```

## Trying it Out in a Small Sample

Andrew sets up a small .NET minimal API project to test the feature:

### Setting Up the Sample

```bash
git init
dotnet new web
dotnet new gitignore
git commit -m "Initial Commit"
```

Sample app code:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello world!");

app.Run();
```

### Simulating Parallel Work Streams

On a feature branch (`my_feature`) he makes 5 small commits involving the addition and modification of endpoints and reverts. Meanwhile, on `main`, he adds functionality for posting a name and displaying all greeted names.

At this point, the two branches have diverged, and the commit graph reflects this.

### Rebasing and Handling Merge Conflicts

He creates a backup of `my_feature`, then rebases onto `main`:

```bash
git checkout my_feature
git branch my_feature_bak
git rebase base --onto main --no-update-refs
```

Solving both easy and tricky merge conflicts, he ends up with a final code state on `my_feature` that reflects merged changes from both branches.

### Verifying with `git range-diff`

To compare the pre- and post-rebase states:

```bash
git range-diff base..my_feature_bak main..my_feature
```

Sample output:

```text
1: 3070585 = 1: ebd4946 Commit 1
2: 76df723 < -: ------- Commit 2
3: e526ca2 < -: ------- Commit 3
4: 64856f3 < -: ------- Commit 4
5: c96df0b < -: ------- Commit 5
-: ------- > 2: edbe245 Commit 2
-: ------- > 3: e06f56e Commit 5
```

Andrew notes that only the first commit is recognized as equal; others, while seemingly similar, are not considered matches due to context differences in diffs. Adjusting the `--creation-factor` parameter improves match sensitivity:

```bash
git range-diff base..my_feature_bak main..my_feature --creation-factor=90 -s
```

With a higher factor, more matches are found, and the output aligns better with expectations, but the diff remains complex to interpret.

## Practical Observations

- The range-diff output, particularly for small diffs, can be challenging to parse because context changes (not just content changes) affect how diffs are compared.
- The `--creation-factor` parameter can help tweak similarity sensitivity.
- The tool can be powerful for reviewing complex rebases, but interpreting its results takes practice.
- Even with confusing output, the tool is widely used (Linux kernel project) and can help catch subtle rebase errors.

## Summary

- `git range-diff` is helpful for verifying rebases, as it compares commit stacks before and after.
- Understanding its detailed output, especially the diff-of-diffs, is key.
- Configurations like `--creation-factor` and `--no-patch` help tailor the review process to your needs.
- Practice is necessary to get comfortable with the output and spot real errors versus context differences.

---

*Tags: git, range-diff, git rebase, commit comparison, continuous integration, merge conflicts, branch management, version control, development workflow, minimal API*

## About the Author

Andrew Lock maintains the .NET Escapades blog, focusing on .NET development topics and deep technical dives.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/verifiying-tricky-git-rebases-with-range-diffs/)
