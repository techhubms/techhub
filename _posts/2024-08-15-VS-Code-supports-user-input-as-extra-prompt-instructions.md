---
layout: "post"
title: "VS Code supports user input as extra prompt instructions"
author: "Matt Olson"
description: "Instruct GitHub Copilot with extra user input to get more fitting results."
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.matt-o.com/GitHub-Copilot-Custom-Instructions"
viewing_mode: "external"
categories: ["AI", "GitHub Copilot"]
permalink: "/2024-08-15-VS-Code-supports-user-input-as-extra-prompt-instructions.html"
tags: ["AI", "Configuration", "Custom Instructions", "GitHub Copilot", "Posts", "Productivity"]
tags_normalized: ["ai", "configuration", "custom instructions", "github copilot", "posts", "productivity"]
---

Matt Olson discovered a powerful new GitHub Copilot feature in Visual Studio Code that allows users to set custom instructions to tailor Copilot's suggestions. This feature addresses a common request from developers who want to avoid repeating the same prompts, such as specifying library preferences or language version constraints.<!--excerpt_end-->

The setting `github.copilot.chat.experimental.codeGeneration.instructions` lets you define custom instructions either as text strings or import them from markdown files. These instructions can be set at the user, workspace, or even Codespaces/Dev Container level, making it easy to share preferences with teammates. This significantly streamlines workflows by providing consistent context to Copilot across all interactions.

[Read the full article on Matt's blog](https://blog.matt-o.com/GitHub-Copilot-Custom-Instructions)
