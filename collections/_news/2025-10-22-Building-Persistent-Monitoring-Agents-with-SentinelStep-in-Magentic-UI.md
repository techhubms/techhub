---
external_url: https://www.microsoft.com/en-us/research/blog/tell-me-when-building-agents-that-can-wait-monitor-and-act/
title: Building Persistent Monitoring Agents with SentinelStep in Magentic-UI
author: stclarke
feed_name: Microsoft News
date: 2025-10-22 15:43:25 +00:00
tags:
- Agent Workflow
- AI Evaluation
- AI Governance
- Company News
- Context Management
- GitHub
- LLM Agents
- Magentic UI
- Microsoft Research
- Monitoring Agents
- pip Install
- Polling
- Python
- SentinelBench
- SentinelStep
- AI
- News
section_names:
- ai
primary_section: ai
---
stclarke discusses SentinelStep in Magentic-UI, a Microsoft research effort to create AI agents capable of persistent, efficient monitoring tasks, with real-world evaluation and open-source availability.<!--excerpt_end-->

# Building Persistent Monitoring Agents with SentinelStep in Magentic-UI

Modern large language model (LLM) agents have rapidly advanced to handle complex tasks like debugging code, analyzing spreadsheets, and booking travel. However, such agents often fail at simpler—but critical—tasks like persistent monitoring, due to challenges in timing and context management. This article presents **SentinelStep**, a mechanism developed by Microsoft Research for the [Magentic-UI](https://www.microsoft.com/en-us/research/blog/magentic-ui-an-experimental-human-centered-web-agent/?msockid=16c285fb8306647b25f593b982ef6516) agentic system, which enables agents to effectively wait, monitor, and act on long-running tasks.

## Why Monitoring Matters for Agents

- Many workflows involve waiting—monitoring emails, news, or prices over hours or days.
- Current LLM agents either stop polling too soon or excessively waste compute resources.

## SentinelStep: A Solution for Long-Running Tasks

- **SentinelStep** wraps the agent in a workflow that uses *dynamic polling* and careful context management.
- Allows agents to monitor conditions for extended periods without losing focus or consuming excessive resources.
- Implemented in [Magentic-UI](https://github.com/microsoft/magentic-ui), an open-source prototype.

### How SentinelStep Works

1. **Polling Frequency:**
    - Adapts the interval between checks based on task type and observed results.
    - Balances efficiency (avoiding token overuse) with responsiveness.
2. **Context Overflow:**
    - Saves the agent's state after each check and restores this state for repeated polling—prevents context window exhaustion.
3. **Workflow Integration:**
    - Co-planning interface lets users define actions, conditions, and polling intervals for each monitoring step.
    - Agents are delegated tasks such as web browsing, code execution, or external API calls.

### Components Visualized

- Actions (what the agent does)
- Condition (when the check is complete)
- Polling interval (how often the check occurs)

![SentinelStep's UI diagram](https://www.microsoft.com/en-us/research/wp-content/uploads/2025/10/Figure-1-Sentinel-UI.png)

## Evaluation Challenges and SentinelBench

- Monitoring tasks in production are rare and irreproducible (e.g., waiting for a repo to hit 10,000 stars).
- **SentinelBench**: A synthetic suite of 28 scenarios (e.g., GitHub Watcher, Teams Monitor, Flight Monitor) makes tests repeatable for benchmarking agent performance.
- Results show SentinelStep significantly boosts reliability—at 1 and 2 hours, success rates increase from 5.6% to over 33% and 38% respectively on long-duration tasks.

![Evaluation results](https://www.microsoft.com/en-us/research/wp-content/uploads/2025/10/Figure-2-Eval.png)

## Impact and Availability

- SentinelStep enables agents to act with patience and efficiency, setting the stage for proactive, always-on AI assistants.
- Open-source via [GitHub](https://github.com/microsoft/magentic-ui) and installable using `pip install magnetic-ui`.
- Guidance on production deployment and ethical considerations is provided in the [Transparency Note](https://github.com/microsoft/magentic-ui/blob/main/TRANSPARENCY_NOTE.md).

## Related: AI Testing and Evaluation Podcast

- [AI Testing and Evaluation: Learnings from Science and Industry](https://www.microsoft.com/en-us/research/story/ai-testing-and-evaluation-learnings-from-science-and-industry/)
- Explores AI governance and systematic test practices.

## Conclusion

SentinelStep represents a major step towards robust, monitoring-capable AI agents. By efficiently scheduling and maintaining agent state over long intervals, it solves a core gap in current LLM agent capabilities. Developers and researchers can experiment with the open-source implementation as Microsoft continues to refine tools for dependable, persistent AI systems.

This post appeared first on "Microsoft News". [Read the entire article here](https://www.microsoft.com/en-us/research/blog/tell-me-when-building-agents-that-can-wait-monitor-and-act/)
