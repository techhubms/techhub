---
layout: "post"
title: "VS Code extension marketplace wars: Cursor users hit roadblocks"
description: "Microsoft's enforcement of extension marketplace terms creates new obstacles for Cursor IDE users, highlighting the challenges of open source alternatives and the evolving landscape of developer tooling."
author: "Tim Anderson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devclass.com/2025/04/08/vs-code-extension-marketplace-wars-cursor-users-hit-roadblocks/"
viewing_mode: "external"
categories: ["DevOps"]
date: 2025-04-08 00:00:00 +00:00
permalink: "/2025-04-08-vs-code-extension-marketplace-wars-cursor-users-hit-roadblocks.html"
tags: ["C/C++", "Community", "Compliance", "Cursor", "Developer Tools", "DevOps", "Extension Marketplace", "IDE", "Microsoft", "Open VSX", "VS Code"]
tags_normalized: ["cslashcplusplus", "community", "compliance", "cursor", "developer tools", "devops", "extension marketplace", "ide", "microsoft", "open vsx", "vs code"]
---

Microsoft's stricter enforcement of its Visual Studio Code extension marketplace terms has left users of Cursor, an AI-powered coding editor, facing new roadblocks—especially with popular Microsoft extensions like C/C++. <!--excerpt_end-->

Cursor, built on the open source foundation of VS Code, has long allowed developers to install extensions from Microsoft's marketplace. However, recent changes mean that while installation may succeed, key features—such as "Find all references" in the C/C++ extension—are now blocked, with pop-up notices citing usage restrictions. This shift has caused confusion, as Cursor still recommends these extensions for relevant projects, but their functionality is increasingly limited.

The move is rooted in Microsoft's marketplace terms, which restrict official extensions to "in-scope products and services" like Visual Studio, VS Code, GitHub Codespaces, and Azure DevOps. These restrictions have driven the creation of the Open VSX marketplace, managed by the Eclipse Foundation, as a vendor-neutral alternative. Yet, Open VSX offers fewer extensions and less usage compared to Microsoft's marketplace, leaving Cursor users with fewer options.

Developers have reported that older versions of Microsoft extensions may still work, but updates are often incompatible. Workarounds, such as downloading .vsix files directly, are becoming less viable as Microsoft removes download links from extension homepages. While open source alternatives like the clangd extension exist, they lack the widespread adoption of Microsoft's offerings.

Some speculate that Microsoft's actions are a response to new features like Agent Mode in VS Code, which make Cursor a more direct competitor. The compliance landscape is also evolving, with Cursor reportedly publishing links to extensions via its own service rather than directly accessing the marketplace. For now, the issues appear limited to Microsoft's own extensions, not third-party ones.

## Summary

- Cursor IDE users are encountering new restrictions when using Microsoft extensions from the VS Code marketplace, particularly for C/C++.
- Microsoft's marketplace terms limit extension use to official products, prompting stricter enforcement and blocking of features in alternative editors.
- The Open VSX marketplace offers an open alternative but has fewer extensions and lower adoption.
- Workarounds for installing Microsoft extensions are being closed off, increasing challenges for Cursor users.
- The situation highlights the tension between open source developer tools and proprietary ecosystems, as well as the impact of new features like Agent Mode in VS Code.
