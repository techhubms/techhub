---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/the-agent-that-investigates-itself/ba-p/4500073
title: 'The Agent that Investigates Itself: How Azure SRE Agent Enables Autonomous Incident Resolution'
author: sanchitmehta
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-03-10 15:12:09 +00:00
tags:
- Agent Design
- AI
- AI Agents
- Autonomous Systems
- Azure
- Azure SRE Agent
- Codebase Investigation
- Community
- Context Engineering
- DevOps
- DevOps Automation
- Error Monitoring
- Filesystem Architecture
- Incident Management
- Log Analysis
- Microsoft Azure
- Prompt Engineering
- Root Cause Analysis
- Security
- Security Analysis
- Self Healing Systems
- Site Reliability Engineering
- Telemetry
- .NET
section_names:
- ai
- azure
- dotnet
- devops
- security
---
Sanchit Mehta presents a detailed look at how the Azure SRE Agent autonomously investigates and resolves incidents, often identifying and fixing its own issues. The post explains how architectural choices—like filesystem workspaces and context layering—make these advanced AI-driven capabilities possible.<!--excerpt_end-->

# The Agent that Investigates Itself

*By Sanchit Mehta*

## Introduction

Azure SRE Agent is responsible for tens of thousands of incidents weekly, serving both Microsoft’s internal and external customers. This article reveals how the agent not only responds to issues, but can autonomously identify and resolve its own operational faults—pushing modern site reliability practices well beyond traditional dashboards and human-driven investigations.

## Agent-Initiated Investigation: A Real-World Example

Recently, a cache hit rate alert for the Azure SRE Agent itself began firing. Instead of relying on dashboards, engineers simply asked the agent to investigate. The agent spun up subagents, analyzed logs, parsed its own source code, and quickly diagnosed the issue. Key findings included:

- False positive cache misses due to low-token requests for Claude Haiku (structurally uncacheable).
- A real regression in Claude Opus, traced to a single pull request (PR) that disrupted prompt prefix stability, significantly dropping cache hits.
- The agent suggested and implemented fixes: excluding uncacheable queries from alerts, and restoring prefix stability in the code.

This example marks a shift from manual, reactive monitoring to autonomous, context-driven analysis.

## Architectural Foundations: Three Core Bets

The evolution of the Azure SRE Agent’s capabilities is rooted in three bold architectural choices:

### 1. Filesystem as the Agent’s Workspace

- All relevant artifacts (source code, logs, runbooks, memory files) exist as files in a workspace the agent navigates using standard developer tools (*read_file*, *grep*, *find*, *shell*).
- This approach is inspired by traditional developer and Unix workflows, making the agent capable of:
  - Navigating repositories, analyzing stack traces, and correlating logs to logic.
  - Investigating historical context by checking out the relevant code versions (commits).
  - Handling ambiguity and missing telemetry by reasoning directly over the codebase.
- The result? "Intent Met" scores for root-cause findings rose dramatically on novel incidents.

### 2. Context Layering

- Beyond code, the agent receives structured context at prompt construction: lists of accessible systems, repository trees, memory maps, and Azure resource topologies.
- This ensures the agent understands its operational boundaries and resource scopes at the start of each investigation, avoiding wasted effort or context misalignment.

### 3. Frugal Context Management

- Large outputs (logs, tool results) remain external files until specifically summarized for model consumption, drastically reducing costly context window usage.
- Two compaction strategies keep investigations focused:
  - **Context Pruning**: Drops stale or irrelevant steps as the session progresses.
  - **Auto-Compact**: Summarizes sessions exceeding length limits, ensuring investigations can be long-running and continuous.
- The agent also operates multiple parallel subagents for divergent hypotheses, collecting their results before merging conclusions.

## Feedback Loops: The Agent Learns and Improves

With these foundations, Azure SRE Agent now participates in a feedback loop—finding operational errors (timeouts, LLM 429s, etc.), tracing them to code, and submitting PRs with proposed fixes. Regular manual review ensures quality while offloading the toil of tracking repetitive or emerging issues.

Over the past month, practical wins included:

- Reduction of LLM and streaming errors by more than 80% after targeted fixes.
- Security analysis, vulnerability discovery, and even contributing to Responsible AI reviews.
- End-to-end handling of LiveSite incidents and customer issues.

## Lessons Learned

- The breakthrough was removing bespoke, hand-crafted scaffolding—from precomputed queries to rigid toolkits. Letting the agent discover and build its own context enables deeper, more scalable reasoning.
- Memory navigation via file traversal outperformed embedding or retrieval-based systems for diagnostic relevance.
- The best agent isn’t just an executor of playbooks—it becomes the author of new ones, iteratively improving itself over time.
- Open problems remain: managing memory staleness, optimal resource budgeting, and surfacing hidden context assumptions.

## Conclusion

Azure SRE Agent exemplifies applied, system-level AI: autonomous, adaptive, and continually learning from both human input and operational data. By baking in developer-like workflows, context-rich initialization, and frugal resource management, the agent doesn’t just follow incident playbooks—it writes its own, and improves with every cycle.

---

*Co-authored by visagarwal​.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/the-agent-that-investigates-itself/ba-p/4500073)
