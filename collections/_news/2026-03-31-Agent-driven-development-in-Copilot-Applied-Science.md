---
author: Tyler McGoffin
feed_name: GitHub Engineering Blog
section_names:
- ai
- devops
- github-copilot
title: Agent-driven development in Copilot Applied Science
date: 2026-03-31 16:00:00 +00:00
tags:
- Agent Driven Development
- AI
- AI & ML
- AI Agents
- Application Development
- Architecture & Optimization
- Automation
- Autopilot Mode
- Blameless Culture
- CI/CD
- Code Review Automation
- Coding Agents
- Contract Testing
- Copilot CLI
- Copilot SDK
- Developer Workflow
- DevOps
- Documentation
- GitHub Copilot
- GitHub Copilot CLI
- Linters
- MCP Servers
- News
- Planning Mode
- Prompt Engineering
- Refactoring
- Regression Tests
- Strict Typing
- VS Code
primary_section: github-copilot
external_url: https://github.blog/ai-and-ml/github-copilot/agent-driven-development-in-copilot-applied-science/
---

Tyler McGoffin shares lessons learned using GitHub Copilot (especially Copilot CLI) as a “coding agent” to build and maintain agents that automate trajectory-analysis work, and explains prompting, architecture, and iteration practices that make agent-first development faster and safer.<!--excerpt_end-->

# Agent-driven development in Copilot Applied Science

Tyler McGoffin describes how he used coding agents to automate a repetitive part of his work (analyzing agent “trajectories” from evaluation benchmarks), and what he learned about collaborating effectively with GitHub Copilot in an agent-first development workflow.

## The impetus

A large part of the work described involves analyzing coding agent performance against standardized benchmarks such as:

- [TerminalBench2](https://www.tbench.ai/)
- [SWEBench-Pro](https://scale.com/leaderboard/swe_bench_pro_public)

Benchmark runs produce **trajectories**: lists of an agent’s actions and reasoning while attempting tasks.

- Trajectories are often **`.json` files** with **hundreds of lines** each.
- Across many tasks and many runs, this becomes **hundreds of thousands of lines** to inspect.

The author’s recurring loop was:

1. Use **GitHub Copilot** to surface patterns in trajectories.
2. Manually investigate the smaller set of relevant output.

To avoid repeating that process manually, he decided to automate it using agents, resulting in a tool he refers to as `eval-agents`.

## The plan

The goals for the project were:

1. Make agents easy to share and use
2. Make it easy to author new agents
3. Make **coding agents** the primary vehicle for contributions

Goal (3) influenced the project most: setting up Copilot to build the tool effectively also made the repository easier for others to collaborate in.

## Making coding agents your primary contributor

The setup used:

- **Coding agent**: Copilot CLI
- **Model used**: Claude Opus 4.6
- **IDE**: VS Code

He also leveraged the **Copilot SDK** (powered under the hood by Copilot CLI) to speed up agent creation, including:

- Existing tools and **MCP servers**
- A way to register new tools and skills
- Other built-in agent-oriented capabilities

From there, the development workflow was guided by three principle areas:

- **Prompting strategies**: be conversational and verbose; use planning modes before agent modes
- **Architectural strategies**: refactor often; update docs often; clean up often
- **Iteration strategies**: shift from “trust but verify” to “blame process, not agents”

With these in place, the team reportedly created in under three days:

- 11 new agents
- 4 new skills
- A concept of “eval-agent workflows” (described as “scientist streams of reasoning”)

And a large change set:

- **+28,858 / -2,884 lines of code** across **345 files**

## Prompting strategies

The author argues agents handle well-scoped problems well, but need more guidance on complex work.

Key idea: if you want the agent to act like an engineer, treat it like one:

- Over-explain assumptions
- Guide the reasoning
- Use the agent’s speed for research/planning before changing code

Example prompt used to improve regression testing:

```text
> /plan I've recently observed Copilot happily updating tests to fit its new paradigms even though those tests shouldn't be updated. How can I create a reserved test space that Copilot can't touch or must reserve to protect against regressions?
```

This led to guardrails “akin to contract testing” that only humans can update.

## Architectural strategies

For “agent-first repositories,” the author claims the highest leverage work becomes:

- Refactoring naming and file structure
- Writing tests (including for issues uncovered along the way)
- Keeping documentation current
- Cleaning up dead code that agents might miss

The reasoning: a maintainable, well-documented codebase helps Copilot navigate patterns more reliably, making feature delivery easier.

## Iteration strategies

The author maps “blameless culture” practices to agent-driven development:

- Build guardrails and processes because mistakes are inevitable
- When mistakes happen, improve the process so the mistake is less likely to recur

He calls out **CI/CD principles** as important for this approach, including:

- Strict typing to enforce interfaces
- Robust linters to impose implementation rules
- Integration, end-to-end, and contract tests

If Copilot has these tools in the loop, it can check its own work more effectively.

## Putting it all together: a feature loop

A proposed loop for agent-driven development:

1. Plan a new feature with Copilot using `/plan`.
   - Iterate on the plan.
   - Ensure testing is included.
   - Ensure docs updates are included and done before implementation (docs as guidelines next to the plan).
2. Let Copilot implement the feature on `/autopilot`.
3. Run an agent review loop with Copilot Code Review, for example:
   - `request Copilot Code Review, wait for the review to finish, address any relevant comments, and then re-request review. Continue this loop until there are no more relevant comments.`
4. Human review to enforce patterns and standards.

He also suggests recurring review prompts, such as:

- `/plan Review the code for any missing tests, any tests that may be broken, and dead code`
- `/plan Review the code for any duplication or opportunities for abstraction`
- `/plan Review the documentation and code to identify any documentation gaps. Be sure to update the copilot-instructions.md to reflect any relevant changes`

He mentions running these automatically weekly, and also triggering them during the week as changes land.

## Takeaways

The author’s core message is that agent-first development pressures teams to prioritize fundamentals that already mattered:

- Clean architecture
- Thorough documentation
- Meaningful tests
- Thoughtful design

He frames Copilot like a junior engineer: onboard it well, give context, put guardrails in place, and focus on improving the process rather than blaming the agent.

## Try it

Suggested steps:

1. Download [Copilot CLI](https://github.com/features/copilot/cli)
2. Activate Copilot CLI in any repo:

```bash
cd <repo_path> && copilot
```

3. Use this prompt:

```text
/plan Read <link to this blog post> and help me plan how I could best improve this repo for agent-first development
```


[Read the entire article](https://github.blog/ai-and-ml/github-copilot/agent-driven-development-in-copilot-applied-science/)

