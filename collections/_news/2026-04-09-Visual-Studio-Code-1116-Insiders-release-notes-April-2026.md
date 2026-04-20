---
section_names:
- devops
primary_section: devops
title: Visual Studio Code 1.116 (Insiders) release notes (April 2026)
feed_name: Visual Studio Code Releases
external_url: https://code.visualstudio.com/updates/v1_116
author: Visual Studio Code Team
date: 2026-04-09 21:34:01 +00:00
tags:
- '@import'
- Accessibility
- Agentic Development
- Agents App
- Bundlers
- Chat Input
- CSS
- DevOps
- File Context Completions
- Keybindings
- Keyboard Navigation
- News
- Node Modules Resolution
- Release Notes
- Screen Readers
- VS Code
- VS Code Insiders
---

The Visual Studio Code Team summarizes what’s new in VS Code 1.116 (Insiders), including updates to the Agents app (keyboard navigation, accessibility help, and file-context completions) plus CSS @import link resolution improvements.<!--excerpt_end-->

# Visual Studio Code 1.116 (Insiders)

*Last updated: April 8, 2026*

Welcome to the 1.116 release of Visual Studio Code.

## Highlights

- Updates to the **Agents app** focused on keyboard navigation, accessibility, and workspace-scoped context features.
- Improved CSS navigation support for `@import` paths that resolve through `node_modules` (useful when working with bundlers).

## April 8, 2026

- Added dedicated commands and keybindings in the **Agents app** to focus:
  - the **Changes** view
  - the **files tree** within the Changes view
  - the **Chat Customizations** view
  
  This enables full keyboard navigation.
  
  References: #308327, #308322, #308265

- Added an **accessibility help dialog** in the Agents app chat input (⌥F1 on macOS; Windows: Alt+F1; Linux: Shift+Alt+F1).
  - Shows available commands and keybindings for screen reader users
  - Includes an option to control announcement verbosity
  
  Reference: #308259

- Added support for CSS `@import` link **node_modules resolution**.
  - Lets you Ctrl+click through imports such as:

```css
@import "some-module/style.css";
```

  This is intended to work for workflows that use bundlers.
  
  Reference: #295074

## April 7, 2026

- Added **#-triggered file-context completions** to the Agents app.
  - Scoped to the workspace selected in the picker
  
  Reference: #299057

## Notes

The release notes encourage users to try new features as soon as they’re ready and to check back often for updates.


[Read the entire article](https://code.visualstudio.com/updates/v1_116)

