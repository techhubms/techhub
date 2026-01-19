---
layout: post
title: You're probably using Copilot the wrong way
author: Thershort
canonical_url: https://www.reddit.com/r/GithubCopilot/comments/1lwg11b/youre_probably_using_copilot_the_wrong_way/
feed_name: Reddit Github Copilot
feed_url: https://www.reddit.com/r/GithubCopilot.rss
date: 2025-07-10 15:43:52 +00:00
permalink: /github-copilot/community/Youre-probably-using-Copilot-the-wrong-way
viewing_mode: external
tags:
- AI Tools
- Code Generation
- Coding Productivity
- Comparing
- Developer Workflows
- Development Planning
- File Level Planning
- Software Development
- Sonnet 4
- Traycer
section_names:
- ai
- coding
- devops
- github-copilot
---
In this article, Thershort draws on years of coding experience to offer actionable insights into operating efficiently with AI coding tools, like GitHub Copilot and Sonnet 4. Emphasizing the irreplaceable value of human oversight, Thershort provides a personal workflow for integrating AI into the development process, offering tips, tools, and warnings for developers eager to leverage AI without losing control over code quality or project direction.<!--excerpt_end-->

## Leveraging GitHub Copilot and AI Tools for Effective Software Development

**Author: Thershort**

Drawing from extensive experience in backend Java, React, and DevOps, Thershort reveals a streamlined workflow for using AI to tackle up to 90% of coding tasks. The key message is that AI should be seen as a productivity tool, *not* a replacement for fundamental developer knowledge or critical thinking.

### The Pitfalls of Full AI Autopilot

Thershort warns against fully entrusting AI tools with end-to-end development. Without human guidance, AI lacks context about the specific feature requirements, constraints, and trade-offs—which often leads to flawed designs requiring significant manual corrections.

### The Importance of Planning

A structured approach is vital. Prior to coding, Thershort drafts a plan (sometimes even in a Slack DM), breaking the project into stages like API creation, route registration, adding rate-limits, etc. For substantial tasks, a tool called Traycer is used to generate detailed file-level plans, listing functions, symbols, and file connections. For smaller tasks, this level of planning is optional.

### AI Tools in the Workflow

- **Copilot’s Ask mode:** Thershort suggests this is often unnecessary if you can prompt well, and dedicated tools are usually better.
- **Plan review:** Every generated plan is reviewed line by line; nothing is accepted blindly.
- **Testing files:** For informal, experimental projects, testing files can be omitted to reduce complexity, as their inclusion through automatic planning (from tools like Traycer) may be excessive.

### Coding with Sonnet 4 via Copilot

With a clear plan in hand, Thershort harnesses Sonnet 4 inside Copilot to generate clean code reliably. The improvements in Copilot’s autocomplete significantly boost workflow efficiency. The author emphasizes that the choice of IDE is less important—the power lies in the Sonnet 4 model and clear developer direction.

### Final Advice

Treat Copilot as a sharp assistant: provide concrete plans and guide the process. Human oversight remains crucial—AI amplifies productivity for those with foundational development skills, but it isn’t a substitute for technical expertise.

This post appeared first on Reddit Github Copilot. [Read the entire article here](https://www.reddit.com/r/GithubCopilot/comments/1lwg11b/youre_probably_using_copilot_the_wrong_way/)
