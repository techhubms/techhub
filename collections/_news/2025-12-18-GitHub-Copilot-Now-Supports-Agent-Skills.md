---
external_url: https://github.blog/changelog/2025-12-18-github-copilot-now-supports-agent-skills
title: GitHub Copilot Now Supports Agent Skills
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-12-18 17:26:18 +00:00
tags:
- Agent Skills
- Automation
- Coding Agents
- Copilot
- Copilot CLI
- Developer Tools
- GitHub Community
- Instruction Scripting
- SXkills Directory
- VS Code
- VS Code Insiders
section_names:
- ai
- coding
- github-copilot
---
Allison introduces the Agent Skills feature in GitHub Copilot, which empowers developers to automate specialized coding tasks by leveraging reusable, shareable instruction sets.<!--excerpt_end-->

# GitHub Copilot Now Supports Agent Skills

GitHub Copilot introduces **Agent Skills**, a new feature allowing developers to teach Copilot how to perform specific, repeatable tasks. Agent Skills are structured as folders containing instructions, scripts, and additional resources that Copilot can automatically load when a relevant prompt is detected.

## Key Features

- **Cross-Platform Support**: Agent Skills work across Copilot coding agent, Copilot CLI, and agent mode in [Visual Studio Code Insiders](https://code.visualstudio.com/insiders/).
- **Automatic Loading**: When Copilot determines a Skill is relevant to a user's task, it loads and follows the provided instructions, including scripts or files in the skill directory.
- **Custom and Community Skills**: Users can write their own skills or access shared ones from repositories like [`anthropics/skills`](https://github.com/anthropics/skills) and [`github/awesome-copilot`](https://github.com/github/awesome-copilot).
- **Compatibility with Claude Code**: If a repository already has skills for Claude Code in the `.claude/skills` directory, Copilot will detect and use them automatically.
- **Upcoming Support**: Full support is coming to the stable release of Visual Studio Code in early January.

## How to Get Started

1. **Create a Skill Folder**: Organize instructions, code scripts, and resources related to a specific workflow or coding task.
2. **Share or Discover Skills**: Explore skill directories from the community or share your own for others to use.
3. **Integrate with Copilot**: Place skills in the recognized directory; Copilot scans and applies them as needed.

## Additional Resources

- [Learn more about Agent Skills](https://docs.github.com/copilot/concepts/agents/about-agent-skills)
- [Join the GitHub Community Discussion](https://github.com/orgs/community/discussions/categories/announcements)

Agent Skills represents an evolution in AI-powered coding automation, supporting both individual workflows and community-driven improvements. Developers can now extend Copilot’s capabilities and share best practices more effectively.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-18-github-copilot-now-supports-agent-skills)
