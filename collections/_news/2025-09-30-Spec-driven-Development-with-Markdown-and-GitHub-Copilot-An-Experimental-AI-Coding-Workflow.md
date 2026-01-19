---
external_url: https://github.blog/ai-and-ml/generative-ai/spec-driven-development-using-markdown-as-a-programming-language-when-building-with-ai/
title: 'Spec-driven Development with Markdown and GitHub Copilot: An Experimental AI Coding Workflow'
author: Tomas Vesely
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-09-30 19:07:46 +00:00
tags:
- AI & ML
- AI Coding Agents
- AI Development Workflow
- Automation
- CLIs
- Command Line Tools
- Compile.prompt.md
- Copilot Instructions.md
- Generative AI
- GitHub Brain MCP Server
- Go Language
- Linting
- Main.md
- Markdown
- Markdown Programming
- MCP
- README.md
- Spec Driven Development
- VS Code
section_names:
- ai
- coding
- github-copilot
---
Tomas Vesely introduces a practical workflow where GitHub Copilot compiles app logic directly from Markdown specifications, illustrating the approach using a Go-based CLI project. Learn how this spec-driven method synchronizes documentation and code.<!--excerpt_end-->

# Spec-driven Development with Markdown and GitHub Copilot

**Author:** Tomas Vesely

## Overview

This article demonstrates a specification-driven workflow for app development using GitHub Copilot. The central idea is to use Markdown files not just for documentation, but as the application's executable specification, which the AI coding agent then compiles into source code—in this case, Go.

## Motivation

Traditional use of AI coding agents involves iterative prompts that can easily lose application context, requiring developers to repeat instructions or correct contradictory outputs. To mitigate this, tools like GitHub Copilot allow repository instruction files like `copilot-instructions.md`. This workflow refines that further, storing the entire application spec in Markdown files and letting Copilot handle code generation.

## Project Structure

The example project, [GitHub Brain MCP Server](https://github.com/wham/github-brain), organizes source and documentation as follows:

```
.
├── .github/
│   └── prompts/
│       └── compile.prompt.md
├── main.go
├── main.md
└── README.md
```

### Key Files

- **README.md:** User-facing documentation on installation, usage, and commands.
- **main.md:** The executable specification for the app. All major features, arguments, and business logic are written in structured Markdown here.
- **compile.prompt.md:** A GitHub Copilot prompt directing the agent to translate the Markdown specification to Go code (main.go).

## Development Workflow

1. **Edit the spec** in `main.md` or update the documentation in `README.md`.
2. **Invoke GitHub Copilot** (in VS Code, using the `/` command) with the compilation prompt to generate `main.go`.
3. **Run and test** the generated Go code. If issues arise, update `main.md` accordingly.
4. **Repeat as needed**.

### Example: Command Implementation

- **CLI commands** and argument handling are declared in Markdown and referenced from the documentation.
- **Logic for data pulling, command arguments, and database schema** is specified structurally in Markdown, which Copilot interprets and implements.
- **GraphQL queries, data handling, and logging** strategies are also described in the spec.

### Linting and Cleanliness

A separate linter prompt (`lint.prompt.md`) can be invoked with Copilot to clean up or clarify the Markdown specification, standardizing terminology and removing redundancies without altering code.

## Observations and Lessons

- **Workflow improves iteration speed** and context management, reducing the risk of Copilot forgetting prior context or producing inconsistent code.
- **Specification and documentation remain in sync** by centering everything in the Markdown files.
- **Complexity management**: As code grows, breaking specs into modular sections is recommended.
- **Future work**: Integration of tests and experimenting with language-agnostic regeneration are suggested.

## Conclusion

Using Markdown as a programming language for spec-driven development with AI coding agents like GitHub Copilot offers practical benefits in workflow speed, maintainability, and documentation synchronization. While there are scaling and testing concerns, this experimental approach provides actionable patterns for developers working with AI-powered tooling.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/generative-ai/spec-driven-development-using-markdown-as-a-programming-language-when-building-with-ai/)
