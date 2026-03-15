---
external_url: https://devblogs.microsoft.com/visualstudio/custom-agents-in-visual-studio-built-in-and-build-your-own-agents/
title: 'Custom Agents in Visual Studio: Built-in and Custom Workflows with GitHub Copilot'
author: Rhea Patel, Kelly Fam
primary_section: github-copilot
feed_name: Microsoft VisualStudio Blog
date: 2026-02-19 21:02:13 +00:00
tags:
- .NET
- Agents
- AI
- C++
- Code Review
- Community Configurations
- Copilot
- Custom Agents
- Custom Workflows
- Debugger
- Design Systems
- Feature Planning
- GitHub Copilot
- IDE Tooling
- MCP
- News
- Profiler
- Testing
- VS
- Workspace Awareness
section_names:
- ai
- dotnet
- github-copilot
---
Rhea Patel and Kelly Fam introduce new Visual Studio agent features, showing how developers can harness built-in and custom agents powered by GitHub Copilot for tailored coding workflows.<!--excerpt_end-->

# Custom Agents in Visual Studio: Built-in and Custom Workflows with GitHub Copilot

Visual Studio has expanded beyond a single assistant—developers can now leverage both curated built-in agents and build their own custom agents, all backed by GitHub Copilot’s workspace awareness and code intelligence.

## Built-In Agents for Developer Workflows

The new preset agents in Visual Studio address common developer workflows and integrate tightly with IDE tooling:

- **Debugger**: Goes beyond error messages, using call stacks and variable states for systematic error diagnosis across your solution.
- **Profiler**: Integrates with Visual Studio’s profiling features to identify performance bottlenecks and suggest targeted optimizations directly relevant to your codebase.
- **Test**: (Requires solution loaded) Generates unit tests tailored to your project's frameworks and patterns, ensuring tests that align with CI/CD requirements.
- **Modernize** (.NET and C++ only): Assists with framework and dependency upgrades, flags breaking changes, generates migration code, and follows your project’s existing conventions.

You can access these through the agent picker in the chat panel or by using ‘@’ in Copilot Chat.

## Create Your Own Custom Agents (Preview)

Teams can now define custom agents designed for their specific workflows. These agents have:

- Access to project context and code understanding
- Ability to use your preferred models and tools
- Enhanced power via MCP (Microsoft Copilot Platform), enabling connections to internal knowledge sources, design systems, APIs, and databases

### Example Custom Agent Patterns

- **Automated Code Review**: Enforce code conventions by connecting to your style guide or ADR repo via MCP.
- **Design System Enforcement**: Sync with Figma/component libraries to maintain UI consistency.
- **Feature Planning**: Gather requirements, clarify tasks, and structure hand-off plans before code is written.

### Configuration

Custom agents are defined in `.agent.md` files placed in your repository’s `.github/agents/` folder. For example:

```
your-repo/
└── .github/
    └── agents/
        └── code-reviewer.agent.md
```

**Notes:**

- This is a preview; file formats may evolve.
- If no model is specified, the currently selected model will be used.
- Tool names can differ between Copilot platforms; check Visual Studio compatibility.
- Community examples are available at the [awesome-copilot repo](https://github.com/github/awesome-copilot). Verify tool compatibility before use.

## Get Involved

Share your configurations or provide feedback via the [awesome-copilot repo](https://github.com/github/awesome-copilot) or the [Copilot feedback forum](https://developercommunity.microsoft.com/t/Custom-modes-for-Copilot-Chat/10950930?q=custom+).

---

With these new capabilities, Visual Studio developers can tailor the AI-powered experience to their team’s real-world coding needs, going far beyond a one-size-fits-all assistant.

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/custom-agents-in-visual-studio-built-in-and-build-your-own-agents/)
