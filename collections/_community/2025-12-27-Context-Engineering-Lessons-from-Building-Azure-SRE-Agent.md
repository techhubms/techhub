---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/context-engineering-lessons-from-building-azure-sre-agent/ba-p/4481200
title: Context Engineering Lessons from Building Azure SRE Agent
author: sanchitmehta
feed_name: Microsoft Tech Community
date: 2025-12-27 20:07:30 +00:00
tags:
- Azure CLI
- Azure SRE Agent
- Code Interpreter
- Context Engineering
- DevOps Patterns
- Incident Automation
- Kubernetes
- LLM Orchestration
- Microsoft Azure
- Multi Agent Systems
- Production AI
- Prompt Engineering
- Site Reliability Engineering
- Tool Chaining
- AI
- Azure
- DevOps
- Community
section_names:
- ai
- azure
- devops
primary_section: ai
---
Sanchit Mehta, with co-author Vis Agarwal, walks through building the Azure SRE Agent. They discuss the real-world lessons, technical insights, and evolving architecture of an autonomous, AI-powered Site Reliability Engineering agent in Azure.<!--excerpt_end-->

# Context Engineering Lessons from Building Azure SRE Agent

## Introduction

In this blog, Sanchit Mehta and Vis Agarwal document their deep technical dive into designing and evolving the Azure SRE Agent—a cloud-based AI system built to autonomously manage Azure resources and handle production incidents. The story explores the concrete engineering patterns, architectural trade-offs, and context management lessons learned from real-world deployment.

## Initial Approach: Tool Explosion and Brittle Workflows

- The team began with over 100 specialized tools and 50+ sub-agents, each tied to narrow functions and prescriptive prompts.
- This resulted in high complexity, fragility, and an inability for the system to generalize: as new edge cases and operational quirks in Azure arose, the backlog of patchwork fixes grew unmanageable.
- Key insight: Overconstraining model behavior leads to brittle, static, and high-maintenance workflows, not adaptive agents.

## Breakthrough: Consolidating into Wide Tools

- Pivoting from narrow tools, they shifted to a model where two "wide" tools—`az` (Azure CLI) and `kubectl` (Kubernetes CLI)—became first-class citizen actions.
- This move:
  - Greatly reduced context bloat and tooling overhead.
  - Unleashed the model's understanding of powerful, well-known CLIs.
  - Enabled the agent to cover Azure's broad operational surface area using built-in LLM knowledge.

## Multi-Agent Architectures: Coordination Overhead

- Next, the team experimented with a multi-agent, persona-driven approach (sub-agents specialized per Azure service).
- Scaling to 50+ sub-agents quickly exposed pain points:
  - Discovery and routing failures: agents couldn't always find the right sub-agent for a task.
  - Fragility from conflicting prompts: breakdowns at one agent polluted or derailed orchestration chains.
  - Infinite loops and tunnel vision in reasoning.
- The fix: collapse specialized agents into a handful of generalists, amplify with broader tools, and move domain knowledge into on-demand reference files (later influenced by emerging "agent skills" concepts).

## Context Management Patterns

1. **External Code Execution**: Instead of overstuffing the model's context window with raw data (like large metrics dumps), the agent generates code for runtime analysis (e.g., via Pandas or Numpy), executes it, and feeds only the summary back into the context. This dramatically improved reliability and efficiency for incident analytics.

2. **Planning and Compaction**: Maintaining externalized plans (e.g., explicit todo lists) and aggressively compacting historical context into summaries keeps the agent focused and prevents unwieldy growth of conversation history.

3. **Session-Based File Disclosure**: Tool outputs that could be massive (such as full database tables or large logs) are intercepted as files in a sandbox. The agent then selectively inspects, filters, or summarizes the data, preventing overwhelming the model's context window.

4. **Tool Call Chaining**: The latest evolution has the model generate mini-scripts that chain multiple tool calls for multi-step workflows, which are then executed deterministically outside the LLM, reducing roundtrips and increasing context clarity.

## Key Insights

- **Leverage LLMs for Orchestration, Not Calculation:** Let models plan and decide workflows, but offload deterministic computation to code.
- **Context Engineering is Memory Management:** Disciplined, explicit context management—loading only strategic, summarized data—proves more effective than simply scaling up model size or context window.
- **Generalist Agents with Wide Tools Offer Real Reliability:** Focusing on extensible, reusable tools and fewer, broader agents yields resilient, production-grade AI systems in enterprise cloud contexts.
- **Iterative Pattern Discovery:** Most improvements were discovered through continuous trial, evaluation, and rapid feedback loops, not up-front design.

## Conclusion

The Azure SRE Agent's reliability gains came not from model upgrades alone but from innovations in context engineering, tool abstraction, and workflow orchestration. These practical lessons are broadly applicable to teams building AI-driven DevOps, SRE, or cloud automation platforms in Microsoft Azure and beyond.

## Further Reading

- [Azure SRE Agent Documentation](https://learn.microsoft.com/en-us/azure/sre-agent/)
- [Agent Skills: Anthropic's approach](https://www.anthropic.com/engineering/equipping-agents-for-the-real-world-with-agent-skills)
- [Advanced Tool Use with Programmatic Agents](https://www.anthropic.com/engineering/advanced-tool-use)
- [Karpathy's Deep Dive on Context Windows](https://www.youtube.com/watch?v=LCEmiRjPEtQ&t=620s)

---

*Thanks to visagarwal for co-authoring this post.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/context-engineering-lessons-from-building-azure-sre-agent/ba-p/4481200)
