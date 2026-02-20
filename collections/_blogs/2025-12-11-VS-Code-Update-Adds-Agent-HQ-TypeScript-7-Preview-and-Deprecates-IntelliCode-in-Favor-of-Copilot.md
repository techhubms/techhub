---
external_url: https://www.devclass.com/ai-ml/2025/12/11/vs-code-update-brings-agent-overload-typescript-7-preview-and-the-end-of-intellicode/1730665
title: VS Code Update Adds Agent HQ, TypeScript 7 Preview, and Deprecates IntelliCode in Favor of Copilot
author: DevClass.com
primary_section: github-copilot
feed_name: DevClass
date: 2025-12-11 15:20:03 +00:00
tags:
- Agent HQ
- Agentic Coding
- AI
- AI Assisted Development
- AI Security
- Blogs
- Code Completion
- GitHub Copilot
- IDE
- IntelliCode
- JavaScript
- Microsoft
- Node.js
- TypeScript
- TypeScript 7
- VS Code
- .NET
section_names:
- ai
- dotnet
- github-copilot
---
DevClass.com examines the latest Visual Studio Code release, reported by Tim Anderson, featuring Agent HQ for AI agents, a TypeScript 7 preview, and the deprecation of IntelliCode in favor of GitHub Copilot for code completions.<!--excerpt_end-->

# VS Code Update: Agent HQ, TypeScript 7 Preview, and IntelliCode Deprecation

**Author:** DevClass.com (reported by Tim Anderson)

## Overview

Microsoft has rolled out a substantial update to Visual Studio Code (VS Code), introducing several major features and significant changes to developer workflows:

- **Agent HQ**: A new feature for managing multiple background agentic coding assistants within VS Code.
- **TypeScript 7 Preview**: Experimental support for the upcoming TypeScript 7 release, providing a new compiler and language service written in Go for improved speed and memory efficiency.
- **IntelliCode Deprecation**: IntelliCode, the long-standing AI code completion tool, is being discontinued in favor of GitHub Copilot, which will now require a subscription after a limited number of free completions.

## Agentic Coding and Agent HQ

VS Code now supports concurrent background agents designed to assist with coding tasks, following a broader industry move toward agentic AI development. The 'Agent HQ' interface serves as a central hub for organizing and customizing these agents, including management of local and cloud-based agents. While the 'Agent Sessions' view preview has been disabled by default, it can still be re-enabled via configuration, though this standalone view may be removed in the future as agent features are further integrated into chat workflows.

Notable capabilities introduced include:

- Keeping AI agents active even when the chat is closed
- Migrating agent sessions from local machines to the cloud
- Running custom subagents for specific tasks
- Enabling sharing of agents across teams and organizations

## Security Considerations

With the addition of sophisticated AI agents comes increased security risk—prompt injection attacks and unreliable AI decisions are highlighted concerns. The update notably includes a controversial 'YOLO' setting that, if enabled, disables manual security approval for tools in all workspaces. Microsoft flags this as a critical vulnerability risk and leaves the feature disabled by default, underscoring the trade-off between productivity and security in fast-moving AI development environments.

## TypeScript 7 Preview

Developers can now opt into using TypeScript 7, which boasts a new native implementation in Go, promising faster load-times and lower memory consumption. This requires developers to install and configure TypeScript Go, after which both TypeScript and JavaScript language features will leverage the new backend. Additionally, the VS Code build scripts have been fully migrated to TypeScript, thanks to support from Node.js 22.18+.

## IntelliCode Deprecation and GitHub Copilot

IntelliCode, a popular extension providing free AI-assisted code completions for several languages, is deprecated. Microsoft is steering users to GitHub Copilot, which now offers a limited number of free completions per month, after which a subscription is required. The move has prompted user pushback, given IntelliCode's large installed base, but reflects Microsoft's focus on commercial AI-powered developer tooling.

## Outlook

The December update is expected to be the final substantial release until February 2026, as the monthly release cycle pauses for the holidays.

## References

- [Visual Studio Code v1.107 Release Notes](https://code.visualstudio.com/updates/v1_107)
- [Agent HQ Announcement on GitHub Blog](https://github.blog/news-insights/company-news/welcome-home-agents/)
- [Official Agent HQ Overview Video](https://www.youtube.com/watch?v=Qfnod4Sw0gY)
- [TypeScript 7 Developer Update](https://devblogs.microsoft.com/typescript/progress-on-typescript-7-december-2025/)

---
*This summary is based on coverage by Tim Anderson for DevClass.com.*

This post appeared first on "DevClass". [Read the entire article here](https://www.devclass.com/ai-ml/2025/12/11/vs-code-update-brings-agent-overload-typescript-7-preview-and-the-end-of-intellicode/1730665)
