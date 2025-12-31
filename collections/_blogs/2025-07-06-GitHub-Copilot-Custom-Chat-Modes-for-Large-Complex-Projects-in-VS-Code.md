---
layout: "post"
title: "GitHub Copilot Custom Chat Modes for Large, Complex Projects in VS Code"
description: "Harald Binkle explains how to create and use custom chat modes for GitHub Copilot in VS Code. These specialized modes help developers tackle code review, testing, API design, performance optimization, and documentation more efficiently in large projects by defining targeted AI 'personalities' and toolsets."
author: "Harald Binkle"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://harrybin.de/posts/github-copilot-custom-chat-modes/"
viewing_mode: "external"
feed_name: "Harald Binkle's blog"
feed_url: "https://harrybin.de/rss.xml"
date: 2025-07-06 15:00:00 +00:00
permalink: "/blogs/2025-07-06-GitHub-Copilot-Custom-Chat-Modes-for-Large-Complex-Projects-in-VS-Code.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: ["AI", "AI Configuration", "API Design", "Code Review", "Coding", "Custom Chat Modes", "Developer Workflow", "DevOps", "Documentation", "GitHub Copilot", "Instruction Files", "Performance Optimization", "Blogs", "Productivity", "Testing", "VS Code"]
tags_normalized: ["ai", "ai configuration", "api design", "code review", "coding", "custom chat modes", "developer workflow", "devops", "documentation", "github copilot", "instruction files", "performance optimization", "blogs", "productivity", "testing", "vs code"]
---

In this article, Harald Binkle details how custom chat modes for GitHub Copilot in VS Code can improve productivity and collaboration in large projects by tailoring AI behavior and tools to specific workflows.<!--excerpt_end-->

# GitHub Copilot Custom Chat Modes for Large Projects

**Author:** Harald Binkle  
**Posted on:** July 6, 2025, 03:00 PM

## Overview

Learn to create custom chat modes in Visual Studio Code for GitHub Copilot to enhance development workflows, especially in complex, multi-faceted projects. These custom modes let you tailor Copilot Chat's behavior and available tools to specific tasks, making AI assistance more efficient, focused, and consistent across your team.

---

## What are Custom Chat Modes?

Custom chat modes are a feature in VS Code that allow developers to define and switch between specialized configurations for GitHub Copilot Chat. Unlike standard instruction files, custom chat modes:

- Allow task-specific configurations ("AI personalities") optimized for workflows like code reviews, testing, API design, and more.
- Provide tool restrictions, enabling only the tools relevant to each task.
- Enable quick context switching without rewriting instructions.
- Combine both instructions and toolsets for holistic customization.

VS Code provides three default modes:

- **Ask Mode**: Answers questions about the codebase and general coding concepts.
- **Edit Mode**: Makes code edits across multiple files.
- **Agent Mode**: Runs autonomous edits and terminal commands.

Custom chat modes go further by allowing you to define your own modes with detailed control.

---

## Creating and Structuring Custom Chat Modes

Custom chat modes are defined by `.chatmode.md` files, typically placed in `.github/chatmodes/` within your project. Each file combines frontmatter (with metadata and tool configuration) and instructions. For example:

### Example 1: Code Review Mode

**File:** `.github/chatmodes/blog-review.chatmode.md`

```markdown
---
description: Review code changes and suggest improvements
tools: ["codebase", "search", "usages"]
---

# Code Review Mode Instructions

You are in code review mode. Focus on:

1. Code quality and best practices
2. Potential bugs or security issues
3. Performance implications
4. Maintainability and readability
5. Adherence to project conventions

Provide specific, actionable feedback with code examples where appropriate.
```

### Example 2: Testing Mode

**File:** `.github/chatmodes/testing.chatmode.md`

```markdown
---
description: Focus on test creation, debugging, and testing strategies
tools: ["codebase", "search", "usages", "findTestFiles", "terminal"]
---

# Testing Mode Instructions

You are in testing mode. Your primary focus is on all aspects of testing:

## Test Creation

- Write unit tests for functions and components
- Create integration tests for API endpoints
- Develop end-to-end tests for user workflows
- Generate test data and mock objects

## Test Analysis

- Review existing tests for completeness
- Identify missing test coverage
- Suggest improvements to test structure
- Debug failing tests

## Testing Best Practices

- Follow AAA pattern (Arrange, Act, Assert)
- Use descriptive test names
- Keep tests focused and isolated
- Prefer behavior-focused tests

## Framework-Specific Guidelines

- For React: Use React Testing Library
- For APIs: Test success and error scenarios
- For E2E: Test critical user journeys
- Consider edge cases and errors

Provide complete, runnable examples with appropriate setup and teardown.
```

### Example 3: Interfaces & Endpoints Mode

**File:** `.github/chatmodes/interfaces.chatmode.md`

```markdown
---
description: Design and work with APIs, interfaces, data contracts
tools: ["codebase", "search", "usages", "fetch", "githubRepo"]
---

# Interfaces & Endpoints Mode Instructions

You are in interfaces and endpoints mode. Focus on API design and data contracts:

## API Design

- Create RESTful endpoints and select proper HTTP methods
- Define request/response schemas
- Plan versioning and error handling

## Interface Definition

- Create TypeScript interfaces for models
- Define contracts between frontend and backend
- Ensure type safety, document structures

## Data Validation

- Input and schema validation
- Handle malformed data, consider security

## Documentation Standards

- Use OpenAPI/Swagger
- Include examples, document errors and codes

## Best Practices

- Consistent naming
- Proper HTTP codes
- Pagination, caching, backward compatibility

Always consider the data lifecycle and integration points.
```

---

## How to Set Up Custom Chat Modes

1. **Create `.github/chatmodes/` Directory**: In your project root folder.
2. **Add `.chatmode.md` Files**: Define modes, each with instructions and allowed tools.
3. **Configure Tool Access**: Tailor each mode’s toolset in the file’s metadata.
4. **Switch Modes**: Use the chat mode dropdown in VS Code’s Chat view.

> ![Chat mode folder structure in VS Code](data:image/webp;base64,UklGRrIFAABXRUJQVlA4TKYFAAAv4AASAA8FybZt0876tv3jpJgupP91m1VbF3t3w21s26py3hfke4ZntEG/1OPuELnbe+8et7Ztu8Z6cs6ZjkrnJ/x/raZ+Iz+cNP/xesxGo0yXtJKFaExnBmyLxEiCjDEgcRDZFWbIRuVPy//n589W0iukQ0IaQICRlGR+vVw4wESBLUWnpaha/nTDgmQERLR/yRgswldInHyl8Ovn8JfvLSZgMCFxss4MRvjXX1mM6AqRKjotZbej7HV11dra0fAAV4AcDZCmiABSI4AAYAks8cYGQAAwwxs88QkQgT1ArpMCXBCBAJ9TvDxwgjd2uEfqK1yoljN/Xk4S8itEK4yQoQIbCX8mTCcNtAhwtiErJQkFSUgSVkLizGAEAuCADJTQDFHvjCTp+JARngA5cAV6mhhp+R+PpdpevJXTMRM7qsJtaYYXahg9t4ZYIgIB9lsB9JRXd2X0FLxQclvmzRw+rto3aFTQ8zBQQWq/kvtjYbf41HePKs7QZe3h/gYDRQFgFNnJ1bAa7tDiUJzDbdB97nezyx7F3WHebm3rOvDq7rufyiST2bM9nExE/yVIkiS3TS0wMgfXxOIu4wXqX5Btfd37Zz/6pQ3EwQTYNlnQtEbT6qROtvVMNGcvVilfz2O2brJh1L4/tnrxtcc3DwC233JoHo1nTm6w+eE5759evqLGvPyaT8m+a8qDr2slglgy69/Zb+pIlNCKWwsJj74nOaimYcfNh+ZhPLGRCJ/Uvb+99tTDD/knakcR1sluEgAOjYPJtkZO0AxUFurqGraeNiPBC4YVqyD9zMrrqItaAPMB1I9I2wH9iA8mFTXceviUGQm+rRsW3YBmvHEJhS6DoEEbUBU13HZyo6L4GLhq6ZOGx6+40hWiNomDilzR88BN1199f/2eZbU7iIszT0C6BPHBJAsPJlDRtkZenQ8Y3r+64q5H6nfWVn9B3X1iaS2mOJLaJgvl+H4Ir0nWqlLb50f2qj2PfuesJsp5wLV4LB1ktJqSxwv1d1X78p+trFvr/ZpXfiahpULH2DaXVhhrybauTSJdeTitng19PFQ9bqtn8819xJ+7P7DIQsrkoef+HCoZS8VQKA9uCaEaFtbbQCR5n6HVNHWhKnTfBrfVs3mRTH1oj1ptO2CbvDcKeHFDQYwXWQh5j0ppjab4jXwshcMsPpiIoJ/TTqtH8CDFvT8pZrZ0lm3y8LFvEdj5LMzRG7kpDpUlArSmvjZ9X7qsHsFXnuJl+007INIC5I9ZqgQzhUFosw6MZHSqCNm///jSYfUoXqJY843NwYTCrKntAFsSmvKBVbOiBygeI67GDUhbIANWIihhpSOUBgtI7ZWqecCg+ExRg+SccLGSh9HleSwlYYFOGM5CvFgsmQNczAB8Bv5HHP4fuGybLDr9YZX9LtPTclDo/oa5o+6s77h8/hf2l6EAP7DwZ9k7sXweotXEnk5IfmemsDrn7cbBBGk81PVC46/O5HgBJmVbI4c+aksOmrplsM3kVXk5yEwJROv7vW7ifQlMv3EUANBLWeiRJxDjNmMpY423xZRkOtNgirPUSWtKqzhoRw5iC9iAfF5aDipaIFrf73WTC95wDhY9Rx/NrVE4LpDhiZECgh5EqsGyJ1h2aesLLNMdUBioy7cWEpG3JQfJfGk5yC4747v9A+V/vHjZ84802+YQGvXQlpkCwAFhaTzTeVGuAAkt3za3a9aznynKykE3peWgE90T8M5LvFM5wkLiDNsmaTwqIZvZPZciAqumfb+a/G9JKTlYKpSWg6VC1AMret+T6QBOXDSxidPCkdbULVeMvKusiwSuY9pMn/ZQbCk56Ka0HIw0Df5+r9fuv29KDhSxGNq7sBiJpfHY9DxV4MEE9YJ1RWxDDlKTyspBJ/j7ve5Slf8N2Q9EAQ8yhIQDfwC+pv3VBf/zBAY=)
---

## Advanced Chat Mode Examples

**Documentation Mode**

```markdown
---
description: Focus on creating and improving documentation
tools: ["codebase", "search", "fetch"]
---

# Documentation Mode Instructions

You are in documentation mode. Focus on creating clear, comprehensive documentation:
- Write for your target audience
- Use concise language
- Provide practical examples
- Structure content logically
- Add troubleshooting where relevant
- Cover APIs, user guides, technical specs, contribution guidelines

Keep docs updated with the codebase.
```

**Performance Optimization Mode**

```markdown
---
description: Focus on performance analysis and optimization
tools: ["codebase", "search", "usages", "terminal"]
---

# Performance Optimization Mode Instructions

Focus on:
- Identifying and resolving bottlenecks
- Reviewing queries, API calls
- Analyzing loading/memory patterns
- Caching, code-splitting, lazy loading
- Measurable, actionable recommendations
```

---

## Benefits of Custom Chat Modes

- **Faster context switching**: Activate the right context instantly for any workflow.
- **More focused conversations**: Each mode restricts Copilot’s behavior and tooling.
- **Better tool usage**: Only relevant features are enabled per mode.
- **Consistent behavior**: Ensures AI interaction is predictable.
- **Team alignment**: Shared modes create uniform experiences for everyone.

---

## When to Use Chat Modes vs Instruction Files

- **Use instruction files** for file/folder-specific technical context, coding standards, and consistent behavior across all files.
- **Use custom chat modes** for broad, task-oriented work (like code review, testing, API design), or when tasks cannot be matched to file paths.
- Choose the approach that best aligns with your workflow and combine both for maximum Copilot effectiveness.

## Best Practices for Chat Modes

1. Keep each mode narrowly focused.
2. Limit available tools to just what's needed.
3. Write precise instructions for each mode.
4. Use clear, descriptive names for easy identification.
5. Maintain modes regularly as your project evolves.
6. Share and standardize modes across your team.

---

## Conclusion

Custom chat modes in VS Code make GitHub Copilot a far more effective assistant in complex projects. By defining distinct AI configurations for different development workflows, teams can improve productivity, collaboration, and output quality.

Start by implementing modes for your most common tasks, iterate based on team feedback, and continue refining as requirements change. For a broader Copilot optimization strategy, combine chat modes with instruction files to cover both project-wide standards and task-specific behaviors.

*Have you tried custom chat modes? Share your experiences and new ideas with Harald Binkle!*

This post appeared first on "Harald Binkle's blog". [Read the entire article here](https://harrybin.de/posts/github-copilot-custom-chat-modes/)
