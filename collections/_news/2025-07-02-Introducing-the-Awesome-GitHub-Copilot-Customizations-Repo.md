---
external_url: https://devblogs.microsoft.com/blog/introducing-awesome-github-copilot-customizations-repo
title: Introducing the Awesome GitHub Copilot Customizations Repo
author: Matt Soucoup, Aaron Powell
feed_name: Microsoft DevBlog
date: 2025-07-02 17:30:59 +00:00
tags:
- Agent Mode
- Code Review
- Community Driven
- Custom Chat Modes
- Custom Instructions
- Developer Productivity
- MCP Server
- Microsoft
- Next.js
- PostgreSQL
- Prompt Engineering
- Reusable Prompts
- Tailwind CSS
- VS Code
- Workflow Customization
section_names:
- ai
- coding
- devops
- github-copilot
---
Matt Soucoup and Aaron Powell present the Awesome GitHub Copilot Customizations repo, empowering developers to extend Copilot through reusable prompts, custom instructions, and chat modes for personalized AI-powered coding workflows.<!--excerpt_end-->

# Introducing the Awesome GitHub Copilot Customizations Repo

*By Matt Soucoup and Aaron Powell*

Today we’re excited to announce the launch of the [Awesome GitHub Copilot Customizations](https://github.com/github/awesome-copilot) repo!

The Awesome Copilot repo is a community-driven resource featuring:

- **Custom instructions**
- **Reusable prompts**
- **Custom chat modes**

These tools help you get consistent AI assistance and tailor GitHub Copilot to your workflow, style, and coding standards. The content in the Awesome Copilot repo is expected to grow as the community contributes further customizations and examples.

## Key Features and Benefits

- Customizations enable tailored and consistent AI-assisted coding.
- Content spans custom workflow instructions, prompt engineering, and specialized chat behaviors.
- Works across multiple IDEs and editors: VS Code, Visual Studio, XCode, JetBrains IDEs, and more.
- Contributions encouraged from the developer community for ongoing repo enhancement.

---

## Workflows: Unlocking Copilot's Potential

### Let the Agent Out

GitHub Copilot's *agent mode* allows Copilot to act as an autonomous peer programmer, handling:

- Multi-step coding tasks
- Creating applications from scratch
- Refactoring across multiple files
- Writing and executing tests
- Migrating legacy code to modern frameworks

Agent mode, also called "vibe coding", can be customized using the repo's instructions and prompts. These enhancements are immediately compatible wherever Copilot and agent mode are available.

To experiment, clone the [awesome-copilot-example](https://github.com/codemillmatt/awesome-copilot-example) repo.

---

## Custom Instructions

Custom instructions provide context to Copilot about your:

- Team workflows
- Coding standards
- Style preferences
- Technology stack

**Format:** Markdown file (e.g., `.github/copilot-instructions.md`)

**Purpose:** Define *how* Copilot should perform tasks by specifying technologies, review checklists, architecture, etc.

### Example: Next.js + Tailwind Development

```markdown
# Next.js + Tailwind Development Instructions

Instructions for high-quality Next.js applications with Tailwind CSS styling and TypeScript.

## Project Context

- Latest Next.js (App Router)
- TypeScript for type safety
- Tailwind CSS for styling

## Development Standards

### Architecture

- App Router with server and client components
- Group routes by feature/domain

_... and so on_
```

**How to use:**

1. Create `.github/copilot-instructions.md` in your project.
2. Copy the relevant instructions (e.g., from [Next.js + Tailwind instructions](https://github.com/github/awesome-copilot/blob/main/instructions/nextjs-tailwind.md)).
3. Launch Copilot in agent mode and give it a task.

**Result:** Copilot follows established conventions from your custom instructions. For instance:

> The page follows the development standards from your instructions:
>
> - Uses React Server Components
> - Implements proper TypeScript typing
> - Follows Tailwind CSS styling patterns
> - Maintains responsive design
> - Uses semantic HTML
> - Maintains consistent styling

[Explore all custom instructions in the repo](https://github.com/github/awesome-copilot?tab=readme-ov-file#-custom-instructions).

---

## Reusable Prompts

Reusable prompts enable you or your team to:

- Compose standardized instructions for recurring tasks
- Enforce process consistency
- Reduce prompt-writing fatigue

**Format:** Markdown file (e.g., `.github/prompts/my-pull-requests.prompt.md`)

### Example: Pull Request Review Prompt

```markdown
--- mode: agent
tools: ['githubRepo', ...]

---

## Custom Chat Modes

Custom chat modes are predefined configurations for tailored Copilot chat experiences. Built-in modes include:

- `chat` (questions)
- `edit` (code changes)
- `agent` (multi-step tasks)

With custom chat modes, you create specialized personas and tool access for common tasks.

### Example: PostgreSQL Database Administrator Chat Mode

**File:** `.github/chatmodes/databaseadmin.chatmode.md`

```markdown
--- description: 'Work with PostgreSQL databases using the PostgreSQL extension.'
tools: ['codebase', 'editFiles', 'githubRepo', 'runCommands', 'database', ...] ---

# Database Administrator Chat Mode

You are a PostgreSQL DBA with expertise in:
- Creating and managing databases
- Optimizing queries
- Backups/restores
- Monitoring performance
- Enforcing database security

Use available tools to interact with databases; never look directly into the codebase.
```

**Prerequisite:** Install the [PostgreSQL VS Code extension](https://marketplace.visualstudio.com/items?itemName=ms-ossdata.vscode-pgsql) for tool integration.

**Benefit:** Enables specialized Copilot chats for database management within your coding environment.

---

## Get Involved

- Explore [Awesome GitHub Copilot Customizations](https://github.com/github/awesome-copilot) for ready-made instructions, prompts, and chat modes.
- Contribute your own customizations to help the community.
- Tailor GitHub Copilot to your team's processes and coding style for consistently improved developer productivity!

---

This is a community-driven project—your contributions are welcome to keep growing the collection and sharing unique strategies for leveraging Copilot.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/blog/introducing-awesome-github-copilot-customizations-repo)
