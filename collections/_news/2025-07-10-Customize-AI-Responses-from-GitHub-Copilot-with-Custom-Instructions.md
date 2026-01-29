---
external_url: https://devblogs.microsoft.com/dotnet/customize-ai-responses-from-github-copilot/
title: Customize AI Responses from GitHub Copilot with Custom Instructions
author: Matt Soucoup
feed_name: Microsoft .NET Blog
date: 2025-07-10 17:15:00 +00:00
tags:
- .NET
- AI Customization
- C#
- Code Generation
- Coding Standards
- Copilot
- Copilot Agent Mode
- Custom Instructions
- Development Workflow
- VS
- VS Code
- AI
- Coding
- GitHub Copilot
- News
section_names:
- ai
- coding
- github-copilot
primary_section: github-copilot
---
Matt Soucoup explains how custom instructions in GitHub Copilot help developers generate AI-powered code that aligns with their team’s coding standards and preferences.<!--excerpt_end-->

## Overview

In this article, Matt Soucoup introduces the concept of customizing GitHub Copilot’s AI responses using custom instructions, allowing teams to enforce their unique coding standards and workflow preferences seamlessly when using Copilot Agent mode.

---

### What Are Custom Instructions?

Custom instructions are markdown-formatted documents you can include in your project—specifically in a `.github/copilot-instructions.md` file—that inform Copilot Agent about your preferred coding approaches, project conventions, or team-specific rules. This enables Copilot’s AI to incorporate these guidelines automatically into its responses, improving consistency, readability, and project alignment.

> Note: Custom instructions only affect Copilot Agent mode (where Copilot performs broader tasks or application scaffolding), not basic inline code completion.

---

### Why Use Custom Instructions?

- **Reduce repetitive prompting:** Instead of reminding Copilot of formatting, error handling, language features, or naming conventions in every request, teams can simply set these rules once.
- **Boost team compliance:** Copilot’s responses automatically follow organization-specific best practices, making onboarding and peer reviews smoother.
- **Maintain code quality:** By embedding guidelines (such as edge-case handling, documentation styles, or dependency usage), you enhance the maintainability and clarity of generated code.

---

### Step-by-Step: Setting Up Custom Instructions

1. **Create a `.github` Folder:** Place it at the root of your project or solution.
2. **Add `copilot-instructions.md`:** Inside `.github`, write your guidelines in markdown. For ideas, Matt links to the community-driven [Awesome GitHub Copilot](https://github.com/github/awesome-copilot) repository, which includes domain- and language-specific samples.
3. **Populate with Guidance:** For example, for C# projects:

   ```markdown
   ---
   description: 'Guidelines for building C# applications'
   applyTo: '**/*.cs'
   ---
   # C# Development

   ## C# Instructions
   - Always use the latest version C#, currently C# 13 features.
   - Write clear and concise comments for each function.

   ## General Instructions
   - Make only high confidence suggestions when reviewing code changes.
   - Write code with good maintainability practices, including comments on why certain design decisions were made.
   - Handle edge cases and write clear exception handling.
   - For libraries or external dependencies, mention their usage and purpose in comments.
   ```

4. **Use Agent Mode:** Once your project is loaded, open GitHub Copilot in Visual Studio or VS Code, ensure Copilot Agent mode is active, and interact with it as usual. The AI will seamlessly incorporate your custom instructions into its generated code and recommendations.

---

### Practical Example

- **Without Custom Instructions:** You need to paste lengthy guidance into every prompt to ensure proper formatting, error handling, documentation, and other standards.
- **With Custom Instructions:** These requirements are embedded, so even a simple request like “create a console app that manages todos” results in code that follows all specified standards by default.

---

### Limitations

- Custom instructions do **not** impact Copilot’s basic code suggestions or completions—they are only used in Agent mode or chat-style interactions.
- You still need to review outputs to ensure nuanced or project-specific needs are addressed; not all edge cases may be perfectly captured.

---

### Resources and Further Reading

- [Awesome GitHub Copilot – Community Instructions](https://github.com/github/awesome-copilot)
- [Copilot Customization Docs](https://code.visualstudio.com/docs/copilot/copilot-customization#_custom-instructions)

---

### Conclusion

Custom instructions are a powerful way to standardize and streamline AI-assisted coding across teams and projects in GitHub Copilot. By defining conventions once, development environments become more efficient and the generated output aligns with what your organization expects.

Give it a try with your next project to see how much more unified your team’s AI-generated code can become.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/customize-ai-responses-from-github-copilot/)
