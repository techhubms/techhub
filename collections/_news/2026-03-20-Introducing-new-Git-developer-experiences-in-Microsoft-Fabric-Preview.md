---
primary_section: ml
author: Microsoft Fabric Blog
section_names:
- devops
- ml
feed_name: Microsoft Fabric Blog
title: Introducing new Git developer experiences in Microsoft Fabric (Preview)
date: 2026-03-20 16:30:00 +00:00
tags:
- Branched Workspaces
- Change Review
- CI/CD
- Conflict Resolution
- Deployment Pipelines
- Developer Experience
- DevOps
- Diff
- Feature Branches
- Git Integration
- Merge
- Microsoft Fabric
- ML
- News
- Preview Features
- Pull Request
- Selective Branching
- Source Control
- Workspaces
external_url: https://blog.fabric.microsoft.com/en-US/blog/introducing-new-git-developer-experiences-in-microsoft-fabric-preview/
---

Microsoft Fabric Blog announces three preview Git developer experience improvements in Microsoft Fabric—Branched Workspaces, Selective Branching, and Compare Code Changes—aimed at making CI/CD workflows in Fabric work more like familiar Git-based feature-branch development.<!--excerpt_end-->

## Overview

Development teams use Git to collaborate and ship reliably. In **Microsoft Fabric**, Git integration is a core part of CI/CD, but workflows like feature branches, isolated development, and change review inside Fabric have historically required extra coordination or tooling.

This announcement introduces **three new Git integration capabilities (Preview)**:

- **Branched Workspaces**
- **Selective Branching**
- **Compare Code Changes**

Together, they’re intended to make it easier to work in isolation, focus only on the items you care about, and review changes confidently—without leaving the Fabric experience.

## The challenge: Git workflows meet shared workspaces

A Fabric workspace is a **shared runtime environment** connected to a single Git branch. That model supports collaboration, but can create friction when developers need to:

- Work on a feature without affecting others
- Avoid copying entire workspaces just to change one or two items
- Understand exactly what will change before committing or pulling updates
- Resolve conflicts with better context

These new experiences are described as early steps toward addressing those challenges end-to-end.

## Branched Workspaces: Clear relationships for feature development

**Branched Workspaces** create a formal relationship between a **source workspace** and the **target workspace** created during a branch-out operation.

When a developer branches out within Fabric:

- A **relationship** is created between the source and target workspace
- The relationship is visible in the Fabric UI (workspace navigation and source control)
- Developers get clearer context about where a workspace came from and how it fits into the overall flow

The post notes this relationship also lays groundwork for future enhancements.

## Selective Branching: Focus exclusively on relevant tasks

With **Selective Branching**, developers can branch out with **only the items they need** instead of copying the full workspace.

During branch-out, you can:

- Select items individually (instead of the default “all items”)
- Choose a subset of items for the feature workspace
- Automatically include required related items to maintain consistency

Expected outcomes:

- Faster branch-out operations
- Smaller, purpose-built workspaces
- Reduced risk of unintended changes
- Faster time-to-code

This is positioned as especially useful for large workspaces where full copies are slow and unnecessary.

## Compare Code Changes: Review before committing or syncing

**Compare Code Changes** adds a diff-style experience directly into Fabric’s Git integration so developers can:

- Review **workspace changes before committing** to Git
- Review **incoming Git updates before updating** the workspace
- Inspect **conflicts side-by-side** before resolving

The post emphasizes the compare experience is meant to feel familiar to developers used to other Git tools and also aligns with Fabric deployment pipeline experiences.

## How the features fit together

A suggested end-to-end workflow:

1. Start in a shared development workspace connected to Git
2. Branch out using **Selective Branching** to create a focused feature workspace
3. Work in isolation within a **Branched Workspace**
4. Use **Compare Code Changes** to review changes before committing
5. Merge via pull request with your Git provider
6. Review incoming updates with **Compare Code Changes** before syncing back

![Figure 1 - The enhanced developer experience with the new releases](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/figure-1-the-enhanced-developer-experience-with.png)

*Figure 1: The enhanced developer experience with the new releases.*

## Preview availability

- [Selective Branching](https://aka.ms/AAz2i48) (Preview)
- [Compare Code Changes](https://aka.ms/AAzydxy) (Preview)
- Branched Workspaces (Preview), expected to be available by the end of March 2026

These capabilities are part of **Fabric Git Integration** and work with supported Git providers.

## Related announcement

The post also points readers to Arun Ulag’s blog for broader FabCon and SQLCon 2026 announcements across Fabric and Microsoft’s database offerings:

- [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/introducing-new-git-developer-experiences-in-microsoft-fabric-preview/)

