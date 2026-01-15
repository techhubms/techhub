---
layout: post
title: How to Improve GitHub Copilot Results with Instruction Files and Custom Chat Modes
author: Harald Binkle
canonical_url: https://harrybin.de/posts/improve-github-copilot-results/
viewing_mode: external
feed_name: Harald Binkle's blog
feed_url: https://harrybin.de/rss.xml
date: 2025-07-04 17:00:00 +00:00
permalink: /github-copilot/blogs/How-to-Improve-GitHub-Copilot-Results-with-Instruction-Files-and-Custom-Chat-Modes
tags:
- AI
- Astro
- Blogs
- Coding Standards
- Contextual Code Assistance
- Custom Chat Modes
- Developer Productivity
- Developer Workflows
- GitHub Copilot
- Instruction Files
- Project Collaboration
- React
- TypeScript
- VS Code
section_names:
- ai
- github-copilot
---
In this post, Harald Binkle explores practical methods for improving GitHub Copilot results in your projects, focusing on instruction files and advanced custom chat modes.<!--excerpt_end-->

# How to Improve GitHub Copilot Results with Instruction Files and Custom Chat Modes

*by Harald Binkle*

Posted on: July 4, 2025 at 05:00 PM

## Introduction

At the recent [DWX conference in Mannheim](https://www.developer-world.de/dwx), I hosted three sessions:

- **GitHub Copilot mit Nachbrenner: Erweiterungen & Power-User Funktionen für smartes Coden** (with [@Nico Orschel](https://www.linkedin.com/in/nico-orschel/))
- **Nutze das volle Potenzial von Dev-Containern u.a. in Webentwicklungs-Workflows**
- **It's all greek to me! - Lokalisierung in Web-Apps elegant und einfach**

Following my first session on GitHub Copilot, many attendees asked how to enhance Copilot’s usefulness in their projects. This article distills those conversations and outlines the foundational (and advanced) steps I take to get consistently better Copilot results.

---

## Creating Instruction Files: The Foundation

One of the most effective ways to improve Copilot’s results is by introducing *instruction files* to your project. These files provide descriptive context and specific guidelines, enabling Copilot to generate more relevant suggestions tuned to your project’s unique needs.

### What Are Instruction Files?

Instruction files are markdown documents that inform GitHub Copilot about your project. Think of them as a "README for AI," detailing architecture, coding conventions, project purpose, and preferred patterns.

### Steps to Create Effective Instruction Files

1. **Add a Common Instructions File:** Place a `.github/copilot-instructions.md` in your project root.
2. **Use Clear, Descriptive Language:** Explain the project and its structure explicitly.
3. **Include Coding Standards:** Define naming conventions and practices.
4. **List Frameworks and Libraries:** Tell Copilot what technologies you’re using.
5. **Provide Code Pattern Examples:** Share idiomatic examples relevant to your team.

#### Example: Project Instruction File Structure

```markdown
# GitHub Copilot Instructions for harrybin.github.io

## Project Context

This is an Astro-based static site using the AstroPaper theme with:
- TypeScript for type safety
- React components for interactive elements
- Tailwind CSS for styling
- Astro Content Collections for blog management
- FuseJS for search functionality
- Deployment on GitHub Pages

## Purpose

Create a blog for developer-focused content, tutorials, and insights.

## Blog Post Creation Guidelines

- All new blog posts must be `.md` files in `/src/content/blog/` (mandatory)
- Use `.md` for markdown or `.mdx` for markdown with JSX
- Filename format: kebab-case
- YAML frontmatter must be complete
- Author: "Harald Binkle"
- Date: ISO 8601 format
- Tags: from approved list
- Description: SEO-friendly, 150-160 characters

## Enforcement Rules

1. Blog posts go in `/src/content/blog/` only
2. Use the template in `content-md.instructions.md`
3. Validate required fields and formatting
4. Follow writing style and structure guidelines
5. Ensure SEO compliance in metadata
```

#### Example: More Generic Instruction File

```markdown
# Project Instructions for AI Assistant

## Project Overview

This is a [description of your project type] using [main technologies].

## Architecture

- Frontend: [Framework/Library]
- Backend: [Framework/Technology]
- Database: [Database type]
- Styling: [CSS framework/approach]

## Coding Standards

- Use TypeScript with strict mode
- Follow ESLint configuration
- Use functional React components
- Prefer composition over inheritance

## File Structure

- `/src/components/`: React components
- `/src/utils/`: Utility functions
- `/src/types/`: Type definitions
- `/src/hooks/`: React custom hooks

## Naming Conventions

- Components: PascalCase
- Files: camelCase
- Constants: UPPER_SNAKE_CASE
```

### Why Project Domain Clarity Is Critical

It’s essential to *describe your project type and business domain*—not just the technologies you use. Many stacks (e.g., React/TypeScript/Node.js) don’t tell Copilot what *kind* of software you’re building (web app, CLI, blog, add-in, etc.).

#### Real-World Domain Examples

```markdown
# Example 1: VS Code Extension

## Project Overview

This is a Visual Studio Code extension using TypeScript, React, and Node.js.

## Business Domain

Developer tool for managing dependencies and automated refactoring.

# Example 2: Office Add-in

## Project Overview

Microsoft Office add-in using TypeScript, React, and Office.js APIs.

## Business Domain

Automates reports and visualizations inside Excel.

# Example 3: Static Site Generator

## Project Overview

Developer blog built with Astro, TypeScript, React components.

## Business Domain

Sharing technical knowledge for the dev community.
```

**Key elements:**

- Technical domain: What type of project?
- Business domain: Who uses it, what does it solve?
- Platform context: Where does it run?
- User interaction: How do users interact with it?

---

## Targeted Instruction Files for Specific Contexts

Beyond `.github/copilot-instructions.md`, you can define targeted instructions for specific file types or folders. With VS Code, use `.instructions.md` files (in `.github/instructions/`) and specify an `applyTo` frontmatter property to tailor context based on patterns.

### Documentation Example (`docs.instructions.md`)

```markdown
---
applyTo: "**/*.md"
---

# Documentation Instructions for AI Assistant

## Documentation Style

- Clear, beginner-friendly language
- Active voice, present tense
- Practical examples, structured headings
- Troubleshooting and "next steps" sections

## Markdown Guidelines

- Proper heading hierarchy
- Code examples with syntax highlighting
- Related documentation links
- Tables for comparisons
```

### Configuration Files Example (`config.instructions.md`)

```markdown
---
applyTo: "**/{config,*.config.*,*.json,*.yml,*.yaml,*.toml,*.env*}"
---

# Configuration Files Instructions

## Configuration Standards

- Descriptive property names
- Inline comments for complex settings
- Group related configs
- Document required/optional settings
- Use environment variables for sensitive values
```

### Test Files Example (`tests.instructions.md`)

```markdown
---
applyTo: "**/{test,tests,spec,__tests__}/**/*.{js,ts,jsx,tsx}"
---

# Test Files Instructions

## Testing Standards

- AAA pattern (Arrange, Act, Assert)
- Descriptive, scenario-based names
- Focused and isolated tests
- Test both behavior and edge cases

## Organization

- Use describe blocks, setup/cleanup hooks
- Helper functions for recurring patterns
```

#### Advantages of Targeted Files

- **Precision**: Only relevant instructions get applied via pattern matching
- **Relevance**: Reduces confusion and token waste
- **Team collaboration**: Centralized, version-controlled, and shared standards

#### Best Practices

- Descriptive file names (e.g., `doc.instructions.md`)
- Leverage glob patterns (e.g., `**/*.md`)
- Include explanatory descriptions in frontmatter
- Update as project standards evolve

#### Example Patterns

```markdown
# All markdown

applyTo: "**/*.md"

# Test files

applyTo: "**/{test,tests,spec,**tests**}/**/*.{js,ts,jsx,tsx}"

# Configs

applyTo: "**/{config,*.config.*,*.json,*.yml,*.yaml,*.toml,*.env*}"

# Docs folders

applyTo: "{docs,documentation}/*.{md,mdx}"

# All files (global)

applyTo: "**"
```

---

## Next-Level: Custom Chat Modes for Large Projects

Instruction files are foundational, but large, complex projects benefit from *custom chat modes*. These allow you to configure specialized AI profiles for tasks like:

- Code reviews
- Testing
- API design
- Documentation

Custom chat modes combine focused instruction with tool restrictions and workflows, creating "task personalities" for different needs.

For more on this, see the follow-up article [GitHub Copilot Custom Chat Modes for Large Projects](/posts/github-copilot-custom-chat-modes).

---

## Conclusion & Community

Have you tried using instruction files in your projects? Share your experiences and improvements achieved in Copilot’s results!

---

*Tags:*

- [github-copilot](/tags/github-copilot)
- [copilot](/tags/copilot)
- [github](/tags/github)
- [vs-code](/tags/vs-code)
- [ai](/tags/ai)
- [productivity](/tags/productivity)
- [development](/tags/development)
- [dwx](/tags/dwx)
- [conference](/tags/conference)
- [instructions](/tags/instructions)

*This site uses cookies for analytics (Microsoft Clarity). By clicking Accept, you consent to the use of cookies for analytics purposes. See the [privacy policy](/privacy).*

This post appeared first on "Harald Binkle's blog". [Read the entire article here](https://harrybin.de/posts/improve-github-copilot-results/)
