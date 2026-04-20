---
section_names:
- ai
title: New capabilities help you automate business processes by mixing AI agents and workflows
feed_name: Microsoft News
tags:
- Agent Nodes
- Agentic Reasoning
- AI
- AI Agents
- Audit Trail
- Business Process Automation
- Company News
- Compliance
- Deterministic Automation
- Governance
- Human in The Loop
- Microsoft Copilot Studio
- Natural Language Workflow Authoring
- News
- Workflow Orchestration
- Workflow Tools
- Workflows
author: stclarke
primary_section: ai
date: 2026-04-13 12:44:11 +00:00
external_url: https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/automate-business-processes-with-agents-plus-workflows-in-microsoft-copilot-studio/
---

stclarke introduces new Microsoft Copilot Studio capabilities for combining AI agents and workflows, focusing on two practical patterns—workflows calling agents and agents calling workflows—to balance flexibility with reliability in business process automation.<!--excerpt_end-->

# New capabilities help you automate business processes by mixing AI agents and workflows

Today Microsoft introduced new capabilities in [Microsoft Copilot Studio](https://www.microsoft.com/en-us/microsoft-365-copilot/microsoft-copilot-studio/) aimed at automating business processes by combining **AI agents** (flexible reasoning) with **workflows** (structured, consistent execution).

Agents and workflows are positioned as complementary:

- **Agents** bring reasoning and adaptability for ambiguous, unstructured, or context-heavy tasks.
- **Workflows** bring structure, predictability, and consistency for repeatable process execution.

[Build agents and workflows in Copilot Studio](https://aka.ms/CopilotStudio)

## What are agents and workflows?

### Agents

**Agents** are flexible AI solutions that rely on foundational models to act, share knowledge, and handle tasks. They can:

- interpret unstructured inputs
- reason over context
- make decisions beyond fixed logic

A key limitation called out is that fully autonomous agent behavior may not be consistent enough for production requirements where organizations need repeatable behavior.

![Screenshot of the Copilot Studio homepage, showing options to create a workflow or create an agent](https://www.microsoft.com/en-us/microsoft-copilot/blog/wp-content/uploads/2026/04/image-1.webp)

### Workflows

**Workflows** are automations designed for consistency and speed, driving process execution reliably.

A limitation noted is that rigid, rules-based workflows can break when encountering unexpected inputs, edge cases, or decision contexts that weren’t anticipated.

## Two patterns for scaling automation with AI

Microsoft describes two common patterns for combining agents and workflows in Copilot Studio.

### Workflows that use agents

In this pattern, the **workflow calls an agent**:

- The workflow provides the process structure (steps, branching logic, handoffs, audit trail).
- The agent handles parts requiring judgement (interpretation, synthesis, exception routing).
- After the agent completes its work, control returns to the workflow and continues predictably.

#### New capability: agent nodes

Microsoft is introducing **agent nodes**: the ability for workflows in Copilot Studio to call an agent directly within a workflow.

- Docs: [agent nodes](https://learn.microsoft.com/en-us/microsoft-copilot-studio/agent-node-workflow)

#### How to set up an agent node in a workflow

1. Create a workflow step called “Add an agent.”
2. Select any Copilot Studio agent you want to include.
3. Provide instructions for the agent’s task, optionally enabling escalation to a designated person for clarification: [request for information](https://learn.microsoft.com/en-us/microsoft-copilot-studio/flows-request-for-information).
4. Add the rest of the workflow steps.

When the workflow runs, the agent is invoked at the defined step, then the workflow continues.

![Screenshot of the Workflow editor, showing a "Run an agent" step and the instructions for calling the agent inside the workflow](https://www.microsoft.com/en-us/microsoft-copilot/blog/wp-content/uploads/2026/04/agent-node.jpg)

#### When to use agents inside workflows

Use agent nodes when a workflow hits a decision that can’t be expressed as simple if/then logic and needs reasoning over context or knowledge retrieval. Examples given:

- Procurement workflow routes to an agent to evaluate vendor proposals against company policies.
- HR onboarding workflow personalizes welcome materials based on role and department.
- Customer service process escalates complex cases to an AI agent for resolution recommendations.

This capability is stated to be available now in all regions.

- Learn more: [add an agent node to a workflow](https://learn.microsoft.com/en-us/microsoft-copilot-studio/agent-node-workflow)

### Agents that use workflows

In this pattern, **agents use workflows as tools**:

- The agent calls a reliable, tested workflow to execute a defined subprocess.
- The agent uses the workflow output to continue its reasoning and response.

This is presented as a way to reuse existing process infrastructure and improve consistency/controls for high-frequency or high-stakes process steps.

- Docs: [create advanced flows](https://learn.microsoft.com/en-us/microsoft-copilot-studio/advanced-flow-create)

#### Two ways to add workflows into an agent

1. Use natural language to build a workflow inside Copilot Studio and include it in an agent: [flow via natural language](https://learn.microsoft.com/en-us/microsoft-copilot-studio/flow-nl).
2. From within an agent, select from a library of pre-existing workflows, add them as tools, and instruct the agent on when to use them.

The post states the agent’s orchestrator selects the right workflow at the right time.

![Screenshot of "Add tool" tab in Copilot Studio. The user has clicked on "Flow," and nine pre-existing flow options are available to pick from](https://www.microsoft.com/en-us/microsoft-copilot/blog/wp-content/uploads/2026/04/Preexisting-workflows.webp)

#### When to use workflows inside agents

Use workflows inside agents when the agent needs to reliably execute repeatable processes (enforcing business rules, coordinating systems, ensuring steps complete). Examples given:

- Sales agent gathers deal details, then calls a workflow to generate a quote, apply discount rules, and route for approval.
- Customer service agent decides a refund is warranted, then calls a workflow to validate rules, process payment reversal, and send confirmation.
- Procurement agent selects vendor/terms, then calls a workflow to create a purchase order in an ERP system and route approvals.

- Trial link: [Sign up for a free Copilot Studio trial](https://aka.ms/TryCopilotStudio)

## Start using agents and workflows together

The post summarizes the combined approach:

- Agents handle ambiguity where workflows can be brittle.
- Workflows enforce structure where agents might drift.

It also notes organizational benefits:

- Business teams can extend/adapt solutions without rebuilding.
- Compliance teams can audit.
- Security and governance teams can choose an appropriate balance of consistency vs agility.

Try Copilot Studio: [Try these capabilities](https://www.microsoft.com/en-us/microsoft-365-copilot/microsoft-copilot-studio/)


[Read the entire article](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/automate-business-processes-with-agents-plus-workflows-in-microsoft-copilot-studio/)

