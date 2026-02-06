---
external_url: https://github.blog/ai-and-ml/github-copilot/5-tips-for-writing-better-custom-instructions-for-copilot/
title: 5 Tips for Crafting Better Custom Instructions for GitHub Copilot
author: Christopher Harrison
feed_name: The GitHub Blog
date: 2025-09-03 16:00:00 +00:00
tags:
- Agentic Workflows
- AI & ML
- Astro
- Coding Guidelines
- Copilot Agent
- Copilot Instructions.md
- Custom Instructions
- Developer Productivity
- Flask
- Generative AI
- Playwright
- Project Structure
- SQLAlchemy
- Svelte
- Tech Stack
- Testing
- TypeScript
- VS Code
- AI
- GitHub Copilot
- News
- .NET
section_names:
- ai
- dotnet
- github-copilot
primary_section: github-copilot
---
Christopher Harrison explains five key tips for writing custom instruction files for GitHub Copilot, showing developers how to optimize code suggestions and documentation using clear project and coding information.<!--excerpt_end-->

# 5 Tips for Crafting Better Custom Instructions for GitHub Copilot

*By Christopher Harrison*

Effective use of GitHub Copilot relies on providing the right context, much like onboarding a new teammate. Well-written custom instruction files help Copilot generate more relevant code suggestions and streamline workflow. This guide presents five core tips, plus a bonus, for writing better `copilot-instructions.md` files:

## 1. Provide a Project Overview

Start your instructions file with a brief summary—just a few sentences explaining what the app does, its audience, and main features. Clarity at this level sets the stage for Copilot to understand the project’s high-level goals.

**Example:**

```markdown
# Contoso Companions

This is a website to support pet adoption agencies, allowing agencies to manage locations and available pets, and enabling potential adopters to search for pets and submit applications.
```

## 2. List the Tech Stack in Use

Clearly outline the backend, frontend, data storage, APIs, and testing frameworks involved. This gives Copilot insight into the environment to tailor its suggestions.

**Example:**

```markdown
## Tech stack in use

- Backend: Flask API, Postgres DB with SQLAlchemy
- Frontend: Astro and Svelte with TypeScript
- Testing: Unittest for Python, Vitest for TypeScript, Playwright for end-to-end tests
```

## 3. Spell Out Coding Guidelines

State any project-specific coding conventions, such as preferred language features, testing requirements, and code organization practices. Keeping guidelines in a dedicated section also aids team members in maintaining code quality.

**Sample Guidelines:**

- Use type hints in all supported languages
- JavaScript/TypeScript code must use semicolons
- Unit and end-to-end tests required, focusing on core functionality and accessibility
- Follow RESTful API design principles
- Always prioritize security best practices

## 4. Describe Project Structure

Documenting folder and file organization enables Copilot (and new team members) to quickly locate resources and integrate efficiently.

**Sample Structure:**

- `server/` – Flask backend
- `models/` – SQLAlchemy models
- `routes/` – API endpoints
- `tests/` – Unit tests
- `client/` – Astro/Svelte frontend
- `scripts/` – Development/deployment scripts
- `docs/` – Project documentation

## 5. Point Copilot to Available Resources

List any scripts, documentation, or automation tools, and explain where to find them. Consider mentioning MCP servers and other integrations that Copilot agents may leverage.

**Sample Resources:**

- `scripts/` folder for setup and testing scripts (`start-app.sh`, `setup-env.sh`, `test-project.sh`)
- MCP server usage for automation
- Playwright for testing
- References to GitHub-specific documentation and issue management

## Bonus Tip: Let Copilot Help Write the Instructions File

You can use Copilot's agent mode in your IDE or assign an issue to Copilot to auto-generate or refine your instructions file. Prompts are available in Copilot documentation to guide this process, such as summarizing the tech stack, coding guidelines, and common practices.

## Final Advice

While no instructions file is perfect, starting with these key sections provides a solid foundation for Copilot integration. Over time, you may extend to more granular instructions for specific languages or frameworks. For more detailed guidance, consult the [official Copilot documentation](https://docs.github.com/en/copilot/how-tos/configure-custom-instructions/add-repository-instructions).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/5-tips-for-writing-better-custom-instructions-for-copilot/)
