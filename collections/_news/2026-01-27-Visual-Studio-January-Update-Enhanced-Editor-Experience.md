---
external_url: https://devblogs.microsoft.com/visualstudio/visual-studio-january-update-enhanced-editor-experience/
title: Visual Studio January Update — Enhanced Editor Experience
author: Simona Liao
primary_section: coding
feed_name: Microsoft VisualStudio Blog
date: 2026-01-27 18:30:12 +00:00
tags:
- Code Completion
- Coding
- Colorized Suggestions
- Copilot Chat Integration
- Developer Productivity
- Developer Tools
- Editor
- Editor Customization
- Fast Scrolling
- HTML Clipboard
- Margin Settings
- Markdown Preview
- Mermaid Diagrams
- Middle Click Scroll
- News
- Productivity
- Syntactic Line Compression
- Text Editor Options
- VS
section_names:
- coding
---
Simona Liao shares the latest Visual Studio January update, focusing on enhanced editor productivity and customization features for developers.<!--excerpt_end-->

# Visual Studio January Update — Enhanced Editor Experience

**Author:** Simona Liao  
**Published on Visual Studio Blog**

## Overview

The January Visual Studio update rolls out several long requested features that make code editing and navigation faster, as well as new ways to customize the developer experience.

---

## Productivity Improvements

- **Fast Scrolling:** Hold `Alt` while using the mouse wheel to quickly scroll through files or documentation. Sensitivity is adjustable under `Tools > Options > Text Editor > Advanced > Touchpad and mouse wheel scrolling sensitivity`.
- **Middle Click Scroll:** Press and hold the scroll wheel to scroll by moving the mouse, ideal for large files. This feature is off by default in 18.3 Insiders 2 and can be enabled under `Tools > Options > Text Editor > Advanced > Middle click to scroll`.
- **HTML-rich Copy Paste:** Visual Studio now supports HTML-formatted clipboard copying. Pasted code appears with syntax highlighting in web-based Office apps, Azure DevOps work items, or other HTML-compatible tools. Rolling out soon from the Insiders to Release channel.
- **Syntactic Line Compression:** Lines without letters or numbers compress by 25% for denser code views. Enable under `Tools > Options > Text Editor > Advanced > Compress blank lines` and `Compress lines that do not have any alphanumeric characters`.
- **Slimmer Left Margin:** The Quick Actions icon (lightbulb or screwdriver) now moves inline with your code instead of taking up extra left margin space, freeing more room for code. Enable under `Tools > Options > Text Editor > Advanced > Show Quick Actions icon inside the editor`. Additional customizable margin controls are forthcoming.

## Colorized Code Completions

- **Syntax Highlighted Suggestions:** Code completion suggestions now use colorization and syntax highlighting to help distinguish code elements.
- **Customization:** Adjust font, size, and color for completions via `Tools > Options > Environment > Fonts and Colors`.
- **Partial Acceptance:** You can now accept part of a suggestion using the mouse. Clicking at any point in the suggestion accepts it up to the cursor. Keyboard options: `Ctrl + Right Arrow` accepts by word, `Ctrl + Down Arrow` accepts by line.

## Markdown Preview Controls

- **Preview Modes:** Choose between Split Preview, Preview Only, and Edit Markdown for focused content review, especially for large images and Mermaid diagrams.
- **Mermaid Zoom:** Zoom controls added for Mermaid diagram previews.
- **Copilot Chat Markdown Preview:** When Copilot Chat generates markdown, use the Preview button to see rendered results—edit and save from there without manual copy-pasting.

_These features are available in Visual Studio 2026 (Insiders). For early access, join the [Insiders Channel](https://visualstudio.microsoft.com/insiders/). The team invites feedback and suggestions for future enhancements._

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/visual-studio-january-update-enhanced-editor-experience/)
