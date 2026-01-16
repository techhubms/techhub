---
layout: post
title: 'How .NET’s Virtual Monorepo Synchronization Works: Technical Challenges and Solutions'
author: Přemek Vysoký
canonical_url: https://devblogs.microsoft.com/dotnet/how-we-synchronize-dotnets-virtual-monorepo/
viewing_mode: external
feed_name: Microsoft .NET Blog
feed_url: https://devblogs.microsoft.com/dotnet/feed/
date: 2026-01-12 17:00:00 +00:00
permalink: /coding/news/How-NETs-Virtual-Monorepo-Synchronization-Works-Technical-Challenges-and-Solutions
tags:
- .NET
- .NET Internals
- Azure DevOps
- Build Infrastructure
- Coding
- Dependency Flow
- Developer Stories
- DevOps
- Git
- Infrastructure
- Monorepo
- News
- Patch Management
- Release Management
- Repository Synchronization
- Source Control
- Two Way Sync
- Unified Build
- Virtual Monolithic Repository
- VMR
section_names:
- coding
- devops
---
Přemek Vysoký details the deep technical efforts behind synchronizing .NET’s many repositories in their Virtual Monolithic Repository, highlighting custom solutions, lessons learned, and ongoing DevOps challenges.<!--excerpt_end-->

# How We Synchronize .NET’s Virtual Monorepo

**Author: Přemek Vysoký**

## Introduction

.NET's journey to a unified build process has centered around creating and maintaining a Virtual Monolithic Repository (VMR) that aggregates all source code and infrastructure needed to ship the .NET SDK. This article dives deep into the technical details and solutions the .NET team has implemented to synchronize dozens of independent product repositories with this VMR using custom two-way algorithms and robust DevOps strategies.

## Background: From Fragmented Builds to Unified Source

Historically, the .NET SDK was created by harvesting build artifacts from many individual repositories, passing them down a dependency tree. While this worked, it complicated build, release, and maintenance processes. The introduction of the VMR—a single git repository representing the SDK’s build state—streamlined these challenges.

- The VMR aggregates code from dozens of product repositories such as [dotnet/runtime](https://github.com/dotnet/runtime).
- It also holds critical infrastructure, pipeline definitions, and build scripts.
- Changes can flow in either direction: from individual repositories into the VMR, and from the VMR back to those repositories.

## Milestones in the VMR Journey

1. **Source Build Tarball**: Initially used for Linux distribution requirements—ensuring source-only builds with no binaries or external network access. This led to unique patching and rebuilding workflows.
2. **VMR-lite**: The first dedicated monorepo, created as a read-only mirror via a one-way synchronization pipeline, still releasing for some Linux platforms today.
3. **Writable VMR**: Transition to a truly two-way synchronized repository, allowing changes to flow both into and out of the VMR via pull requests, enabling simultaneous updates and breaking changes across the entire stack.

## Custom Synchronization Challenges

### Why Not Submodules or Subtrees?

The team evaluated existing git features (submodules, subtree, subrepo) but settled on a custom process to:

- Ensure full source consistency for every VMR commit
- Exclude forbidden binary files
- Map and patch sources with flexibility
- Support reliable bidirectional change flow

### The Two-way Sync Algorithm

- Heavy reliance on patches (`git diff` and `git apply`) to capture and transfer changes across repositories
- Tracking synchronized states via manifest files and metadata stored in version manifests
- Managing submodules by copying their sources directly, stripping binaries, to maintain offline and source-only requirements
- Handling mapped and patched content according to strict configuration files

### Handling Conflicts and Edge Cases

- Synchronizing in both directions can introduce non-obvious git conflicts, especially when changes happen in parallel or files are added/removed quickly
- Custom PR automation and a rebasing workflow help surface true conflicts for developer intervention, while the tooling handles known or trivial conflicts
- Special handling is required to avoid losing reverts or silently dropping changes in corner cases

## Real-world Lessons and Ongoing Challenges

- **Branching and Product Lifecycles**: Each .NET repo may follow its own branching and versioning scheme, requiring careful snap and reset automation
- **Tracking Metadata**: In-repo tracking data is vulnerable to corruption from branch merges; experiments with git notes aim to provide a more robust solution
- **Developer Experience**: Tooling and monitoring are crucial to help engineers navigate code flows, resolve conflicts, and keep synchronized in a constantly evolving codebase

## Conclusion

The creation and maintenance of the .NET VMR is foundational for unifying .NET’s ecosystem and streamlining build/release cycles across dozens of teams and products. While the technical problems solved are impressive, ongoing challenges—from metadata handling to developer workflow optimization—remain an active area of work and innovation for the .NET team.

---

### Additional Resources

- [Unified Build Design Documentation](https://github.com/dotnet/dotnet/tree/main/docs)
- [VMR Code Flow Design Document](https://github.com/dotnet/dotnet/blob/dc803dea8a5917a87a812a05bae596c299368a43/docs/VMR-Full-Code-Flow.md)
- [Code Flow Implementation](https://github.com/dotnet/arcade-services/tree/main/src/Microsoft.DotNet.Darc/DarcLib/VirtualMonoRepo)
- [Example PR](https://github.com/dotnet/dotnet/pull/3629)

---

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/how-we-synchronize-dotnets-virtual-monorepo/)
