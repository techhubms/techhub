---
external_url: https://github.blog/changelog/2025-11-12-copilot-code-review-and-coding-agent-now-support-agent-specific-instructions
title: Agent-Specific Custom Instructions Now Supported in Copilot Code Review and Coding Agent
author: Allison
feed_name: The GitHub Blog
date: 2025-11-12 21:27:42 +00:00
tags:
- .instructions.md
- Agent Specific Configuration
- Applyto Property
- Bug Fixes
- Code Automation
- Copilot
- Copilot Agents
- Copilot Code Review
- Copilot Coding Agent
- Custom Instructions
- Developer Workflow
- Documentation
- Excludeagent Property
- GitHub Repository
- Improvement
- Path Globs
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Allison describes new features for GitHub Copilot, focusing on agent-specific instructions via the excludeAgent property, which enhances control over code review and coding agent behavior for developers.<!--excerpt_end-->

# Agent-Specific Custom Instructions Now Supported in Copilot Code Review and Coding Agent

GitHub Copilot's latest update enables targeted customization of agent behaviors by introducing the `excludeAgent` property in `.instructions.md` files. This feature lets developers define feedback and coding patterns that apply to specific Copilot agents, such as code review or coding agent, providing fine-grained control over repository automation workflows.

## Recap: Customizing Copilot with `instructions.md`

Developers have already been using custom instruction files located in the `.github/instructions` directory. With the `applyTo` property, it's possible to restrict instructions to select directories or files using path globs (e.g., `applyTo: "app/models/**/*.rb"` for model files).

## Introducing Agent-Specific Instructions with `excludeAgent`

- The new `excludeAgent` property in the frontmatter of `.instructions.md` files allows instructions to be omitted for designated agents.
  - `excludeAgent: "code-review"` prevents a file from being used in Copilot code review.
  - `excludeAgent: "coding-agent"` restricts usage from Copilot coding agent.
- If `excludeAgent` is not specified, the instructions file applies to all Copilot agents by default.
- This feature empowers developers to offer focused guidance for each agent, minimizing unwanted overlap and clarifying instructions context.

![Screenshot showing how to specify agent-specific instructions in .instructions.md files.](https://github.com/user-attachments/assets/2abfcfb4-3c6a-422b-8a02-02c923c46ae7)

## Additional Custom Instruction Improvements

- A recent bug fix enables Copilot code review to properly read all relevant `*.instructions.md` files.
- To learn more about creating custom instruction files and configuring Copilot, consult the [Copilot instructions documentation](https://docs.github.com/copilot/how-tos/configure-custom-instructions/add-repository-instructions#creating-a-repository-custom-instructions-file).
- Join the broader discussion and share feedback via [GitHub Community](https://github.com/orgs/community/discussions/categories/copilot-conversations).

## Key Takeaways

- Developers can now control which Copilot agents respond to which instruction files, supporting advanced workflows.
- Instructions are more precise, reducing ambiguity and improving code review and coding outcomes.
- The enhancement reflects Copilot's commitment to collaborative, transparent automation in development teams.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-12-copilot-code-review-and-coding-agent-now-support-agent-specific-instructions)
