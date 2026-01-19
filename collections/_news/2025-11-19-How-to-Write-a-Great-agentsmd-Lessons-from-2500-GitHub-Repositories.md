---
layout: post
title: 'How to Write a Great agents.md: Lessons from 2,500 GitHub Repositories'
author: Matt Nigh
canonical_url: https://github.blog/ai-and-ml/github-copilot/how-to-write-a-great-agents-md-lessons-from-over-2500-repositories/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-11-19 17:00:00 +00:00
permalink: /github-copilot/news/How-to-Write-a-Great-agentsmd-Lessons-from-2500-GitHub-Repositories
tags:
- Agentic AI
- Agents.md
- AI & ML
- AI Development
- Copilot Agents
- Devops Workflow
- Generative AI
- Linting
- Markdownlint
- Persona Definition
- Quality Assurance
- React 18
- Security Analysis
- Software Engineering
- Tailwind CSS
- Technical Writing
- TypeScript
- Unit Testing
- Vite
section_names:
- ai
- coding
- github-copilot
---
Matt Nigh analyzes over 2,500 repositories to share practical strategies for writing effective agents.md files for GitHub Copilot, guiding developers on agent persona design, clear instructions, and reproducible DevOps workflows.<!--excerpt_end-->

# How to Write a Great agents.md: Lessons from 2,500 GitHub Repositories

GitHub Copilot recently introduced support for custom agents via `agents.md` files—enabling developers to configure teams of specialized assistants, each with their own persona and tightly scoped responsibilities. Drawing from analysis of more than 2,500 public repositories, Matt Nigh explains what makes an agent definition actually useful in practice.

## Why Standard 'Helpful Assistant' Prompts Fail

Most agent files are too vague: simply stating "You are a helpful coding assistant" leads to unpredictable results. Effective agents require a sharply defined role (e.g., technical writer, test engineer, security analyst), clear examples of good output, and explicit boundaries on what the agent can or cannot do.

## Core Principles for Effective agents.md Files

The best performing files share six traits:

1. **Put executable commands first:** Explicit commands (like `npm test`, `pytest -v`, or `npm run build`) give agents actionable context for common tasks.
2. **Code examples over explanations:** Specific code snippets demonstrating style or output set expectations better than paragraphs of description.
3. **Set clear boundaries:** Tell the agent which files/directories never to touch and outline constraints (e.g., "Never commit secrets").
4. **Stack specificity:** List frameworks, languages, and versions: "React 18, TypeScript, Vite, Tailwind CSS" addresses ambiguity.
5. **Six core areas:** Cover commands, testing, project structure, code style, git workflow, and boundaries to provide structure.
6. **Iterate and improve:** Start simple, refine as your agent makes mistakes, and add more context over time.

## Example: Documentation Agent Definition

Here is an excerpt of a high-quality documentation agent profile:

```md
---
name: docs_agent

---

You are an expert technical writer for this project.

## Your role

- Fluent in Markdown and TypeScript
- Write for developers, focusing on clarity/practical examples
- Read code from `src/`, update documentation in `docs/`

## Project knowledge

- **Tech Stack:** React 18, TypeScript, Vite, Tailwind CSS
- **File Structure:**
    - `src/` — Source
    - `docs/` — Documentation
    - `tests/` — Test files

## Commands you can use

- Build docs: `npm run docs:build`
- Lint markdown: `npx markdownlint docs/`

## Boundaries

- **Always:** Write to `docs/`, follow style examples, run markdownlint
- **Ask first:** Major changes to existing docs
- **Never:** Change code in `src/`, edit configs, commit secrets
```

## Recommended Agents to Build

- **@docs-agent:** Automates API/functon doc generation
- **@test-agent:** Writes and runs unit/integration tests
- **@lint-agent:** Auto-fixes style without touching logic
- **@api-agent:** Builds API endpoints, modifies routes, asks before schema changes
- **@dev-deploy-agent:** Handles builds and deployment to dev, only with approval

## Starter Template Breakdown

- **Agent name/role:** Clearly identify function (e.g., `lint-agent`, `test-agent`)
- **Persona:** Brief, highly-specific description (e.g., "QA engineer who writes comprehensive tests")
- **Tech/context:** List relevant frameworks, directories, commands
- **Standards:** Easy naming conventions, code style, boundaries
- **Boundaries:** Define ‘always do’, ‘ask first’, ‘never do’ lists to avoid accidental errors, secrets, or destructive changes

## Tips for Iterative Improvement

- Add detail if agents make mistakes
- Include real code examples for reference, not just instructions
- Test agent output and tweak behaviors

## Key Takeaways for Developers

Well-defined agent profiles drive predictable, safe automation. Start specific; clarify role, stack, and commands; guard against risky operations. Adapt instructions as use cases evolve. Refer to Matt Nigh’s analysis and provided templates to accelerate Copilot agent adoption for technical development workflows.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/how-to-write-a-great-agents-md-lessons-from-over-2500-repositories/)
