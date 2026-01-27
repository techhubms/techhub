---
external_url: https://devblogs.microsoft.com/dotnet/prompt-files-and-instructions-files-explained/
title: Understanding Instruction and Prompt Files for GitHub Copilot in .NET Development
author: Wendy Breiding (SHE/HER)
feed_name: Microsoft .NET Blog
date: 2025-09-17 17:05:00 +00:00
tags:
- .NET
- AI Powered Development
- ASP.NET
- Best Practices
- C#
- Code Automation
- Code Review
- Copilot
- Copilot Configuration
- Custom Instructions
- Instruction Files
- Productivity
- Productivity Tips
- Prompt Files
- Repository Standards
- Team Collaboration
- VS
- VS Code
section_names:
- ai
- coding
- github-copilot
primary_section: github-copilot
---
Wendy Breiding details how .NET developers can use instruction and prompt files with GitHub Copilot to automate coding standards, refine code generation, and foster effective team collaboration across Visual Studio, VS Code, and GitHub.com.<!--excerpt_end-->

# Understanding Instruction and Prompt Files for GitHub Copilot in .NET Development

GitHub Copilot is reshaping how developers code by offering real-time code suggestions, automating repetitive actions, and streamlining code reviews, especially in .NET workflows. This guide breaks down two key features—**Instruction Files** and **Prompt Files**—and how they empower .NET developers to achieve greater consistency and productivity in Visual Studio, VS Code, and directly on GitHub.

## Instruction Files

**Instruction files** (like `copilot-instructions.md`) help Copilot understand the rules, coding standards, and guidelines specific to your repository or workspace.

### What Are Instruction Files?

- Define coding conventions, naming standards, and project-specific rules Copilot should follow.
- Can be generic (`copilot-instructions.md` for all requests) or scenario-specific (`[title].instructions.md`).
- Typically placed in the `.github` or `.github/instructions` folder.

### When and How to Use

- Enforce team standards on projects, or maintain open-source consistency for contributors.
- Ensure Copilot applies organization-approved styles and requirements while writing code in C#, ASP.NET, or other supported languages.
- Write from scratch, use a sample, or leverage Copilot to generate the initial file.

### Key Sections to Include

- **Purpose & Scope**: Clarify what the file controls.
- **Location & Scope Rules**: Specify which files/languages are affected.
- **Project Overview**: Audience, features, runtime/platform (e.g., .NET 8).
- **Build/Test Commands**: Developer and CI steps, preferably PowerShell-friendly.
- **Coding & Linting Conventions**: Formatting rules (e.g., `dotnet format`).
- **API/Data Contracts**: JSON shapes, DTOs, sample schemas (avoid secrets).
- **Test Expectations**: Which tests must pass before changes are accepted.
- **CI/CD/Release Rules**: Branch models, required checks, deployment details.
- **Security & Risk Policy**: Secret storage, allowed/disallowed code changes.
- **Output Format**: How code changes and PRs should be presented.
- **Behavior/Tone**: Preferred assistant style (e.g., concise, test-first).
- **Examples**: Good/bad PR samples.
- **Maintenance Notes**: Update procedures.

[See more instruction file samples](https://github.com/github/awesome-copilot/tree/main/instructions).

## Prompt Files

**Prompt files** guide Copilot in specific coding tasks within a single session, ensuring the AI follows requirements relevant to a particular file or operation.

### What Are Prompt Files?

- Provide context for specific coding sessions.
- Saved as `[promptname].prompt.md` in `.github/prompts`.
- Used for complex tasks like custom algorithms, onboarding, or integrating compliance steps.

### How to Use

- Write or generate prompt files with Copilot’s help.
- Activate by referencing them in chat: `/[promptName]` (VS Code), `#[promptName]` (Visual Studio).
- Ideal when you want surgical precision—like asking Copilot to review security issues in cryptography usage or automate documentation creation.

### What to Include in Prompt Files

- **Header/Metadata**: Name, version, author.
- **Scope Globs**: Paths/languages/components targeted.
- **Purpose & Persona**: Intent statement and assistant tone.
- **Allowed/Disallowed Actions**: What Copilot can change or must get approval for.
- **Build/Run/Test Commands**: Environment notes, minimum tests.
- **CI Criteria**: Required checks, PR templates/criteria.
- **File Exceptions**: Files to exclude (binary, generated, etc.).
- **Output & Patch Conventions**: Formatting, commit style, test evidence.
- **Error Handling**: Rollback procedures, notifications.
- **Examples**: Good/bad edits.
- **Maintenance Notes**: Update ownership/procedures.

[Find prompt file examples here](https://github.com/github/awesome-copilot/tree/main/prompts).

## Choosing Between Instruction and Prompt Files

- **Instruction Files**: Best for repo-wide norms and long-term consistency.
- **Prompt Files**: Best for individual, session-based guidance.
- **Combine Both**: For optimal results—set project standards with instruction files, supplement with prompts for special cases or onboarding.

## Best Practices

- Be clear and specific in both file types for better AI suggestions.
- Always manually review Copilot-generated code.
- Share/publish Copilot outputs in code reviews for transparency.
- Reference [Copilot customization documentation](https://code.visualstudio.com/docs/copilot/customization/overview).

## Conclusion

Used thoughtfully, Copilot’s instruction and prompt files let you automate quality, onboard new contributors faster, and consistently apply standards in .NET workflows. Experiment, refine, and leverage these tools to maximize both your development productivity and coding standards.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/prompt-files-and-instructions-files-explained/)
