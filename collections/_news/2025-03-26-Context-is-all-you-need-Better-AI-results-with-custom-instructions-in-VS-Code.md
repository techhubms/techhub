---
layout: post
title: 'Context is all you need: Better AI results with custom instructions in VS Code'
author: Rob Conery, Harald Kirschner
canonical_url: https://code.visualstudio.com/blogs/2025/03/26/custom-instructions
viewing_mode: external
feed_name: Visual Studio Code Releases
feed_url: https://code.visualstudio.com/feed.xml
date: 2025-03-26 00:00:00 +00:00
permalink: /github-copilot/news/Context-is-all-you-need-Better-AI-results-with-custom-instructions-in-VS-Code
tags:
- AI
- Code Generation
- Coding
- Commit Message Generation
- Copilot Chat
- Custom Instructions
- Development Workflow
- GitHub Copilot
- JavaScript
- News
- Node.js
- Project Configuration
- Prompt Engineering
- Prompt Files
- React
- VS Code
- Workspace Settings
section_names:
- ai
- coding
- github-copilot
---
Rob Conery and Harald Kirschner explain how developers can leverage custom instructions in VS Code to fine-tune GitHub Copilot, offering practical examples and strategies for enhancing AI-driven development workflows.<!--excerpt_end-->

# Context is all you need: Better AI results with custom instructions in VS Code

*By Rob Conery, Harald Kirschner*

Earlier this month, Microsoft announced the general availability of custom instructions for GitHub Copilot in Visual Studio Code. Custom instructions allow developers to provide Copilot with detailed context about their team's workflow, code style, or unfamiliar libraries, leading to better and more relevant AI suggestions.

## What are Custom Instructions?

Custom instructions are defined by a simple Markdown file within your project (e.g., `.github/copilot-instructions.md`). This file can outline:

- Project overview
- Coding standards and conventions
- Preferred libraries or tools

Once present, Copilot automatically uses the content to guide its code suggestions and responses.

```markdown
# Copilot Instructions

This project is a web application that allows users to create and manage tasks. The application is built using React and Node.js, and it uses MongoDB as the database.

## Coding Standards

- Use camelCase for variable and function names.
- Use PascalCase for component names.
- Use single quotes for strings.
- Use 2 spaces for indentation.
- ...
```

## Using Custom Instructions in Practice

After adding your `.github/copilot-instructions.md`, try using Copilot Chat with concise prompts (e.g., `tail recursion`). Copilot will combine its AI capabilities with the instructions to deliver more context-aware, project-specific responses in the expected format.

## Personalizing Copilot with Workspace Settings

Configure additional behaviors in your `settings.json` file. For example, you can specify how commit messages are generated:

```json
{
  "github.copilot.chat.commitMessageGeneration.instructions": [
    { "text": "Be extremely detailed with the file changes and the reason for the change. Use lots of emojis." }
  ]
}
```

Disabling these custom instructions will revert Copilot to the default style.

## Expanding with Multiple Instruction Files

You can reference multiple style or standards files in your workspace configuration, supporting language- or domain-specific requirements:

```json
{
  "github.copilot.chat.codeGeneration.instructions": [
    { "file": "./docs/javascript-styles.md" },
    { "file": "./docs/database-styles.md" }
  ]
}
```

## Modify Copilot's Tone and Behavior

Adjust how Copilot constructs responses by specifying instruction preferences regarding apologetic tone, assertiveness, or even creative formats like haiku for test generation.

## Real-World Example: Code Templates

Reference sample code files (e.g., a Sequelize model) to guide Copilot’s code generation to fit your data access patterns or framework conventions.

## Introducing Prompt Files

Prompt files (`*.prompt.md` in `.github/prompts/`) allow teams to create reusable, shareable prompts for repetitive tasks (like generating TypeScript interfaces from a schema). Prompt files can reference one another, enabling complex, context-driven workflows with minimal user input.

## Conclusion

Custom instructions and prompt files give developers granular control over GitHub Copilot’s output within VS Code, ensuring that AI-generated code and suggestions consistently match team standards and workflow requirements. With these tools, Copilot becomes a deeply personalized and powerful development assistant.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/03/26/custom-instructions)
