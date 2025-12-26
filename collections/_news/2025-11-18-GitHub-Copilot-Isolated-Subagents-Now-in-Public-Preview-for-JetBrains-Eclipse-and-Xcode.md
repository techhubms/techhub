---
layout: "post"
title: "GitHub Copilot Isolated Subagents Now in Public Preview for JetBrains, Eclipse, and Xcode"
description: "This announcement introduces the public preview of isolated subagents for GitHub Copilot across JetBrains IDEs, Eclipse, and Xcode. It details the concept of autonomous subagents working within distinct contexts, allowing users to delegate research, API design, code reviews, and technical discovery within Copilot Chat. The post includes setup guidance, usage tips, and feedback channels for users interested in exploring these productivity features."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-11-18-isolated-subagents-for-jetbrains-eclipse-and-xcode-now-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-11-18 17:30:17 +00:00
permalink: "/news/2025-11-18-GitHub-Copilot-Isolated-Subagents-Now-in-Public-Preview-for-JetBrains-Eclipse-and-Xcode.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "AI Development Tools", "API Design", "Autonomous Agents", "Code Review", "Copilot", "Copilot Chat", "Custom Agents", "Developer Tools", "Eclipse", "GitHub Copilot", "IDE Extensions", "JetBrains IDEs", "News", "Productivity", "Subagents", "Technical Documentation", "Xcode"]
tags_normalized: ["ai", "ai development tools", "api design", "autonomous agents", "code review", "copilot", "copilot chat", "custom agents", "developer tools", "eclipse", "github copilot", "ide extensions", "jetbrains ides", "news", "productivity", "subagents", "technical documentation", "xcode"]
---

Allison announces the public preview release of isolated subagents in GitHub Copilot for JetBrains IDEs, Eclipse, and Xcode, showcasing how developers can harness autonomous agents for focused research, code review, and task automation.<!--excerpt_end-->

# GitHub Copilot Isolated Subagents Now Available in JetBrains, Eclipse, and Xcode

**Author: Allison**

GitHub Copilot now supports isolated subagents in public preview on JetBrains IDEs, Eclipse, and Xcode. This new feature allows developers to delegate focused tasks—such as researching, API design, code review, or technical discovery—to autonomous subagents within Copilot Chat. Each subagent operates in its own isolated context to keep your main conversation clean, letting you stay productive while offloading background or analysis work.

## What's New

- **Dedicated subagent context**: Subagents use independent contexts, preventing long-running operations from cluttering your main chat.
- **Autonomous execution**: Once activated, a subagent can work without further user input, ideal for background tasks like documentation lookup or competitive analysis.

## How Subagents Work

**Prerequisites**: Custom agents must be configured in your environment. See the [custom agents documentation](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents) or ask Copilot to list available custom agents.

Subagents operate through two main mechanisms:

### 1. Automatic Delegation

Copilot can delegate tasks automatically based on your description, the configuration of your custom agents, and current IDE context.

### 2. Explicit Invocation

You can address a subagent directly in your chat command. For example:

```
> Have the code-reviewer subagent look at my recent changes
> Let the documentation subagent generate API docs for this module
```

## How to Get Started

1. **Update your IDE** to the latest Copilot extension in JetBrains, Eclipse, or Xcode.
2. **Open Copilot Chat** and start or continue a conversation.
3. **Invoke a subagent** by asking Copilot or mentioning a specific custom agent, referencing your available [custom agents](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents).
4. **Review results** as subagent output is returned to your conversation for further action.

## Feedback Channels

Your input shapes the Copilot roadmap. Share feedback using in-IDE options or contribute to:

- [GitHub Copilot in JetBrains Feedback](https://github.com/microsoft/copilot-jetbrains-feedback/issues)
- [GitHub Copilot in Eclipse Feedback](https://github.com/microsoft/copilot-eclipse-feedback/issues)
- [GitHub Copilot in Xcode Feedback](https://github.com/github/CopilotForXcode/issues)

Start using isolated subagents today to streamline your development workflow and keep your primary chat focused.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-18-isolated-subagents-for-jetbrains-eclipse-and-xcode-now-in-public-preview)
