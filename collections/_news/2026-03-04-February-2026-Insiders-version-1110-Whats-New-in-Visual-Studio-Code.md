---
layout: "post"
title: "February 2026 Insiders (version 1.110): What’s New in Visual Studio Code"
description: "An in-depth breakdown of the new features, improvements, and fixes introduced in the Visual Studio Code February 2026 Insiders build (version 1.110). This release covers enhancements to accessibility, integrated terminal updates, improved UI behaviors, and performance optimizations. Read on for technical details, links to resolved issues, and how to access the latest build."
author: "Visual Studio Code Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/updates/v1_110"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2026-03-04 17:00:00 +00:00
permalink: "/2026-03-04-February-2026-Insiders-version-1110-Whats-New-in-Visual-Studio-Code.html"
categories: ["Coding"]
tags: ["Accessibility", "Coding", "Developer Tools", "Insiders Build", "Integrated Terminal", "News", "Performance Optimization", "Release 1.110", "Release Notes", "Settings Editor", "Software Development", "VS Code", "Xterm.js"]
tags_normalized: ["accessibility", "coding", "developer tools", "insiders build", "integrated terminal", "news", "performance optimization", "release 1dot110", "release notes", "settings editor", "software development", "vs code", "xtermdotjs"]
---

Visual Studio Code Team highlights key updates in the February 2026 Insiders Release (v1.110), including terminal improvements, accessibility enhancements, and UI changes for a better developer workflow.<!--excerpt_end-->

# February 2026 Insiders (version 1.110): Visual Studio Code Release Notes

_Last updated: February 5, 2026_

These notes summarize what's new in the Visual Studio Code February 2026 Insiders build (v1.110), with technical insights and links to relevant GitHub issues.

## Overview

- [Full article](https://code.visualstudio.com/updates/v1_110)
- [Download VS Code Insiders](https://code.visualstudio.com/insiders)
- [See Commit log](https://github.com/Microsoft/vscode/commits/main)
- [Closed issues for this release](https://github.com/Microsoft/vscode/issues?q=is%3Aissue+is%3Aclosed+milestone%3A%22February+2026%22)

## February 5, 2026

- **Settings Editor Scroll Behavior:** New setting lets users control scroll mode (continuous or paginated) in the Settings editor. Paginated mode restricts settings to the selected category. [#291391](https://github.com/microsoft/vscode/issues/291391)

## February 4, 2026

- **Chat Carousel Accessibility:** ARIA labels and keyboard focus improvements for chat’s `askQuestion` carousel. New command/keybinding for screen readers to focus the first response. [#292917](https://github.com/microsoft/vscode/issues/292917)
- **Terminal Text Blink:** Added support for the text blink attribute in the integrated terminal. [#292550](https://github.com/microsoft/vscode/issues/292550)
- **Terminal Color Scheme Reporting:** Applications can now query the integrated terminal's color palette. [#290919](https://github.com/microsoft/vscode/issues/290919)
- **Find Control Behavior:** The Find control in the integrated browser now auto-fills with the currently selected text, mirroring standard browser behavior. [#291378](https://github.com/microsoft/vscode/issues/291378)
- **VS Code Update Progress:** Download progress is now shown when updating VS Code. [#277330](https://github.com/microsoft/vscode/issues/277330)

## February 3, 2026

- **Terminal Performance:** xterm.js optimizations enhance rendering and processing speed. [#292572](https://github.com/microsoft/vscode/issues/292572)
- **Session List Filters:** Quick Pick and welcome page reflect agent session list filters. [#291603](https://github.com/microsoft/vscode/issues/291603)

## February 2, 2026

- **Quick Input Accessibility:** Dynamic ARIA attributes in quick input dialogs reduce redundant screen reader announcements. The Go to Line dialog (`Ctrl+G`) provides improved announcements. [#292353](https://github.com/microsoft/vscode/issues/292353)
- **Hidden Terminal Behavior:** Selecting a hidden terminal now opens it directly if only one exists, skipping the menu. [#291884](https://github.com/microsoft/vscode/issues/291884)
- **Terminal Output Panel State:** Fix so the terminal output panel no longer collapses automatically after successful commands if manually expanded. [#291120](https://github.com/microsoft/vscode/issues/291120)

## January 30, 2026

- **Chat Carousel UI:** The chat question carousel now uses VS Code’s list-style selection interface instead of native radio buttons/checkboxes for consistency. [#291328](https://github.com/microsoft/vscode/issues/291328)

---

For developers: VS Code 1.110 continues to focus on workflow, accessibility, and terminal empowerment. Stay updated by checking the [release page](https://code.visualstudio.com/updates/v1_110) and contributing feedback to the project.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_110)
