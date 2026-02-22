---
layout: "post"
title: "Copilot Coding Agent Supports Code Referencing"
description: "This update introduces code referencing for the GitHub Copilot coding agent, allowing users to see if generated code matches code from public GitHub repositories. The agent highlights matching code in session logs with links and licensing information. It also clarifies how blocking of public code suggestions is handled within agent sessions."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-18-copilot-coding-agent-supports-code-referencing"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-18 22:02:22 +00:00
permalink: "/2026-02-18-Copilot-Coding-Agent-Supports-Code-Referencing.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "AI Coding Assistant", "Code Referencing", "Code Suggestions", "Code Traceability", "Copilot", "Copilot Coding Agent", "GitHub", "GitHub Copilot", "Improvement", "News", "Open Source Licensing", "Public Code Detection", "Session Logs", "Software Development"]
tags_normalized: ["ai", "ai coding assistant", "code referencing", "code suggestions", "code traceability", "copilot", "copilot coding agent", "github", "github copilot", "improvement", "news", "open source licensing", "public code detection", "session logs", "software development"]
---

Allison explains how the Copilot coding agent now supports code referencing, highlighting when generated code matches public GitHub repositories and providing traceability via session logs.<!--excerpt_end-->

# Copilot Coding Agent Supports Code Referencing

The [Copilot coding agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent) is an asynchronous, autonomous background agent that assists developers by generating code suggestions. With this update, the coding agent now incorporates [Copilot code referencing](https://docs.github.com/copilot/concepts/completions/code-referencing).

**Key Features:**

- **Session Log Highlights:** When the agent produces code that matches code from a public GitHub repository, the session logs will highlight the match. Each highlight includes a link to the original source, allowing developers to track where a piece of code originated and to review any applicable license information.

- **Transparency and Licensing:** By revealing the original source of matching code, developers gain greater insight and transparency, helping them comply with open source licenses.

- **Policy Handling:** The Copilot coding agent does not support the “Suggestions matching public code” policy in `Block` mode. If this policy is set to `Block`, matching code suggestions will not be blocked within agent sessions; instead, they will simply be highlighted in the logs.

**Additional Resources:**

- Learn more about code referencing for Copilot coding agents in the official [GitHub Docs](https://docs.github.com/copilot/how-tos/get-code-suggestions/find-matching-code?tool=webui#view-code-references-for-copilot-coding-agent).

This advancement means developers can more confidently use AI-generated code, understanding its provenance and staying informed about any licensing considerations.

---
*Original post by Allison on [The GitHub Blog](https://github.blog/changelog/2026-02-18-copilot-coding-agent-supports-code-referencing).*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-18-copilot-coding-agent-supports-code-referencing)
