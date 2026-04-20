---
primary_section: github-copilot
tags:
- AI
- AI & ML
- Allow All Tools Flag
- Autopilot Mode
- Claude Opus
- Claude Sonnet
- CLI App
- Clipboardy
- DevOps
- GitHub Copilot
- GitHub Copilot CLI
- GitHub Copilot SDK
- GitHub MCP Server
- JavaScript
- Markdown
- Multi Model Workflow
- News
- Node.js
- Open Source
- OpenTUI
- Plan Mode
- Terminal UI
feed_name: The GitHub Blog
external_url: https://github.blog/ai-and-ml/github-copilot/building-an-emoji-list-generator-with-the-github-copilot-cli/
title: Building an emoji list generator with the GitHub Copilot CLI
date: 2026-04-17 18:00:00 +00:00
author: Cassidy Williams
section_names:
- ai
- devops
- github-copilot
---

Cassidy Williams walks through a Rubber Duck Thursdays live build of a terminal-based “emoji list generator” using GitHub Copilot CLI and the GitHub Copilot SDK, including plan mode, autopilot mode, and a multi-model workflow to turn bullet points into emoji-enhanced markdown copied to the clipboard.<!--excerpt_end-->

# Building an emoji list generator with the GitHub Copilot CLI

Every week, the GitHub team runs a stream called [Rubber Duck Thursdays](https://www.youtube.com/@GitHub/streams), where they build projects live, cowork with the community, and answer questions.

This session’s project: a small terminal app that takes a list of bullet points and replaces each bullet with a relevant emoji, then copies the result to the clipboard.

> New to GitHub Copilot CLI? Here’s how to get started: [GitHub Copilot CLI for beginners: Getting started with GitHub Copilot CLI](https://github.blog/ai-and-ml/github-copilot/github-copilot-cli-for-beginners-getting-started-with-github-copilot-cli/)

## What the project does

The “Emoji List Generator” is a CLI/terminal UI that:

- Runs in the terminal
- Lets you paste or type a list of bullet points
- Generates an emoji-enhanced version when you press **Ctrl + S**
- Copies the generated list to your clipboard

## How it was built

### Tech used

- `@opentui/core` for the terminal UI
- `@github/copilot-sdk` for the AI logic
- `clipboardy` for clipboard access

### Using Copilot CLI to plan the app

They started in **plan mode** in GitHub Copilot CLI (using Claude Sonnet 4.6) with a prompt like:

```text
I want to create an AI-powered markdown emoji list generator. Where, in this CLI app, if I paste in or write in some bullet points, it will replace those bullet points with relevant emojis to the given point in that list, and copies it to my clipboard. I'd like it to use GitHub Copilot SDK for the AI juiciness.
```

Copilot then asked clarifying questions (including around libraries and tech choices) and produced a `plan.md` file to review.

### Implementing with a multi-model workflow

They implemented the plan using Claude Opus 4.7 (noted as recently released: [Claude Opus 4.7 is generally available](https://github.blog/changelog/2026-04-16-claude-opus-4-7-is-generally-available/)), resulting in a working terminal UI.

![Screenshot of the Emoji List Generator terminal UI showing input bullet points and the generated emoji-enhanced list](https://github.blog/wp-content/uploads/2026/04/list.png?resize=1869%2C1944)

## Copilot CLI features highlighted

The build calls out several Copilot CLI concepts and tools:

- [Plan mode](https://docs.github.com/copilot/how-tos/copilot-cli/cli-best-practices?utm_source=blog-rdt-emoji-list-cta&utm_medium=blog&utm_campaign=dev-pod-copilot-cli-2026#plan-mode)
- [Autopilot mode](https://docs.github.com/copilot/concepts/agents/copilot-cli/autopilot?utm_source=blog-rdt-emoji-list-cta&utm_medium=blog&utm_campaign=dev-pod-copilot-cli-2026)
- [Multi-model workflow](https://docs.github.com/copilot/reference/ai-models/supported-models?utm_source=blog-rdt-emoji-list-cta&utm_medium=blog&utm_campaign=dev-pod-copilot-cli-2026)
- [The `allow-all` tools flag](https://docs.github.com/copilot/how-tos/copilot-cli/allowing-tools#permissive-options?utm_source=blog-rdt-emoji-list-cta&utm_medium=blog&utm_campaign=dev-pod-copilot-cli-2026)
- [The GitHub MCP server](https://github.com/github/github-mcp-server?utm_source=blog-rdt-emoji-list-cta&utm_medium=blog&utm_campaign=dev-pod-copilot-cli-2026)

## Try it yourself

- GitHub Copilot CLI docs: [Getting started](https://docs.github.com/copilot/how-tos/copilot-cli/cli-getting-started?utm_source=blog-rdt-emoji-list-cta&utm_medium=blog&utm_campaign=dev-pod-copilot-cli-2026)
- GitHub Copilot SDK docs: [Getting started](https://docs.github.com/copilot/how-tos/copilot-sdk/sdk-getting-started?utm_source=blog-rdt-emoji-list-cta&utm_medium=blog&utm_campaign=dev-pod-copilot-cli-2026)

The finished project is available here: [emoji list generator (free and open source)](https://github.com/cassidoo/emoji-list-generator)


[Read the entire article](https://github.blog/ai-and-ml/github-copilot/building-an-emoji-list-generator-with-the-github-copilot-cli/)

