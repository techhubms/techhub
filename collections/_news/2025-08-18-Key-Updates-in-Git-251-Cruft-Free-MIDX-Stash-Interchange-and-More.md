---
external_url: https://github.blog/open-source/git/highlights-from-git-2-51/
title: 'Key Updates in Git 2.51: Cruft-Free MIDX, Stash Interchange, and More'
author: Taylor Blau
feed_name: The GitHub Blog
date: 2025-08-18 17:04:36 +00:00
tags:
- Bloom Filter
- Commit Graph
- Cruft Pack
- Delta Compression
- Git
- Git 2.51
- Git Cat File
- Git Restore
- Git Switch
- Git Whatchanged
- Multi Pack Index
- Open Source
- Path Walk
- RefTable
- Release Notes
- Repack.midxmustcontaincruft
- SHA 256
- Stash Interchange
- Version Control
- Coding
- DevOps
- News
section_names:
- coding
- devops
primary_section: coding
---
In this technical write-up, Taylor Blau guides readers through the most significant improvements and new features in Git 2.51, highlighting technical details and practical implications for developers and DevOps practitioners.<!--excerpt_end-->

# Key Updates in Git 2.51: Cruft-Free MIDX, Stash Interchange, and More

Taylor Blau covers the latest changes in Git 2.51, emphasizing impactful technical features and enhancements that developers and DevOps engineers can leverage in their workflows.

## Cruft-Free Multi-Pack Indexes

Git 2.51 revises how unreachable objects ("cruft packs") are managed in relation to the multi-pack index (MIDX). Previously, unreachable objects were stored separately, but if an object became reachable and only existed in the cruft pack, it couldn't participate in reachability bitmaps. The new release ensures that non-cruft packs are "closed under reachability" by duplicating relevant objects as necessary, improving reliability and enabling smaller, faster MIDX files. Performance benefits observed at GitHub include MIDXs shrinking by ~38% and faster repository reads.

### How to Use

- Enable with the `repack.MIDXMustContainCruft` config option.
- See upstream [documentation](https://git-scm.com/docs/git-config/2.51.0#Documentation/git-config.txt-repackmidxMustContainCruft).

## Smaller Packs with Path Walk

- Introducing "path walk" for pack generation, where objects are grouped by their path, replacing earlier hash heuristics.
- Results in more effective delta compression and smaller pack sizes in some scenarios, without sacrificing performance.
- Try with the `--path-walk` CLI option during repack.

## Stash Interchange Format

- Stash entries can now span multiple commits with a fourth parent linking to previous entries.
- Enables exporting stashes to a reference for syncing between machines (`git stash export --to-ref`, `git stash import`).
- Practical for multi-environment or collaborative use cases.

## Additional Updates

- **git cat-file Improvements**: Now correctly reports submodule objects, enhancing scripting capabilities.
- **Bloom Filter Enhancements**: Commit-graph Bloom filters now support multiple pathspecs in `git log`, increasing query efficiency.
- **Stable Commands**: `git switch` and `git restore` transition from experimental status, guaranteeing CLI stability.
- **Deprecation Notices**: `git whatchanged` is deprecated; use `git log --raw` or the `--i-still-use-this` flag in the meantime.
- **Preparations for Git 3.0**: Default hash for new repos will be SHA-256; reftable backend to become standard.
- **C99 Feature Adoption**: The project now uses C99 features such as the `bool` keyword more broadly, continuing incremental adoption for consistency and code modernization.
- **Patch Submission Guidelines**: Contributors can now submit under names other than their legal name, aligning with broader open source practices.

## Further Reading

- [Official Git 2.51 Release Notes](https://github.com/git/git/blob/v2.51.0/Documentation/RelNotes/2.51.0.adoc)
- [Git Blog Coverage](https://github.blog/open-source/git/highlights-from-git-2-51/)

These enhancements continue Git’s tradition of improving scale, reliability, and developer experience for collaborative development projects.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/open-source/git/highlights-from-git-2-51/)
