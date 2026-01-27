---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/never-explain-context-twice-introducing-azure-sre-agent-memory/ba-p/4473059
title: 'Never Explain Context Twice: Introducing Azure SRE Agent Memory'
author: Dalibor_Kovacevic
feed_name: Microsoft Tech Community
date: 2025-12-08 19:51:45 +00:00
tags:
- AI Ops
- Automation
- Azure DevOps
- Azure SRE Agent
- Context Engineering
- Custom Sub Agents
- DevOps Automation
- Incident Management
- Knowledge Base
- Memory Tool
- Operational Excellence
- Runbooks
- Session Insights
- Site Reliability Engineering
section_names:
- ai
- azure
- devops
primary_section: ai
---
Dalibor Kovacevic details how Azure SRE Agent’s memory feature transforms AI-driven operations by capturing institutional knowledge, improving troubleshooting, and sharing insights across teams.<!--excerpt_end-->

# Never Explain Context Twice: Introducing Azure SRE Agent Memory

Azure SRE Agent has advanced into a robust AI-powered operations platform that now supports persistent memory. This major update enables agents to learn from past incidents, remember team preferences, and continuously increase troubleshooting accuracy, providing a seamless operational experience.

## Why Memory Matters in SRE AI Operations

Operations teams rely on shared institutional knowledge—whether recalling configuration quirks or preferred diagnostic routines. Previously, AI assistants required context to be re-explained each session. Azure SRE Agent’s new memory feature eliminates that, letting agents:

- **Remember** context and preferences persistently
- **Retrieve** the right runbooks/documents on-demand
- **Learn** from previous sessions
- **Share** critical knowledge across the team

## Context Engineering: Building a Smarter AI Assistant

Azure SRE Agent introduces **context engineering**, a systematic way to optimize what information is provided to the AI. Teams identify knowledge gaps using Session Insights, then curate and upload targeted information (runbooks, facts) to make the AI more effective over time.

**Workflow:**

1. Find knowledge gaps with Session Insights
2. Add necessary runbooks or team facts as memories
3. Review subsequent outcomes
4. Refine and update the context further as needed

Each troubleshooting session feeds a feedback loop, ensuring the agent is always improving.

## Breakdown of SRE Agent Memory Components

### 🧠 User Memories

Persist short team facts, preferences, or patterns using chat commands (`#remember`, `#retrieve`, `#forget`). Instantly available to every team member and agent, these allow for quick, low-friction knowledge input and recall.

**Example:**

```plaintext
# remember Team owns app-service-prod in East US region

# remember For latency issues, check Redis cache first

# remember Production deployments happen Tuesdays at 2 PM PST
```

### 📚 Knowledge Base

Upload markdown (`.md`) or text files (`.txt`, up to 16MB) containing full runbooks or technical guides. Documents are indexed with semantic search, so agents can retrieve only the most relevant sections during troubleshooting.

- Accessible in the portal's Knowledge Base area
- Easy management (add, delete, audit)

### 📊 Session Insights

Automated session feedback with analysis, performance scores, and actionable learnings for every troubleshooting event.

- Timelines of the troubleshooting process
- Highlights of what worked
- Areas for improvement and specific next steps
- Overall investigation quality scoring

Session Insights can help teams target where additional context or memory entries are most needed.

## Using Memory in Custom Automation

Through the **SearchMemory** tool, both the base SRE Agent and custom-built sub-agents can pull from User Memories and Knowledge Base to inform their actions. Enable this in Sub-Agent Builder to give custom automations access to your team’s accumulated institutional knowledge.

## Choosing the Right Memory Storage

- **User Memories:** Short, team-specific context; managed in chat
- **Knowledge Base:** Longer, procedural docs; managed in the portal

Both are shared among the whole team for consistency and resilience.

## Quick Start Guide

1. **Add team memories:** Input key patterns and facts via chat
2. **Upload docs:** Add runbooks and guides to Knowledge Base
3. **Review insights:** Use Session Insights after incidents to identify areas for further improvement

## Best Practices

- Organize and regularly audit memories for accuracy
- Keep terminology consistent
- Do **not** store secrets, PII, or confidential information
- Remove outdated or duplicate memories

## Business Impact

The introduction of persistent memory into Azure SRE Agent enables:

- Faster, smarter troubleshooting (lower MTTR)
- Reduced workload from repeated explanations
- Retention of critical institutional knowledge
- Continuous operational improvement over time

By investing in a structured memory strategy, operations teams empower their Azure SRE Agent to deliver ever-improving support, even as team compositions evolve.

---

**Resources:**

- [Azure SRE Agent Home](https://www.azure.com/sreagent)
- [Documentation](https://aka.ms/sreagent/docs)
- [Pricing](https://aka.ms/sreagent/pricing)
- [Demo Recordings](https://aka.ms/sreagent/youtube)

_Ongoing product feedback is encouraged via the agent UI or [GitHub](https://github.com/microsoft/sre-agent)._

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/never-explain-context-twice-introducing-azure-sre-agent-memory/ba-p/4473059)
