---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/extend-sre-agent-with-mcp-build-an-agentic-workflow-to-triage/ba-p/4480710
title: Automating Customer Issue Triage with Azure SRE Agent and MCP
author: dchelupati
feed_name: Microsoft Tech Community
date: 2025-12-23 22:08:48 +00:00
tags:
- Agent Based Automation
- Azure Portal
- Azure SRE Agent
- DevOps Workflow
- GitHub
- Incident Management
- Issue Classification
- MCP
- PagerDuty
- Scheduled Tasks
- Subagents
- Support Automation
- Ticket Triage
- Workflow Orchestration
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
dchelupati details an automated workflow using Azure SRE Agent and MCP to classify and route customer issues, reducing manual triage for engineering teams.<!--excerpt_end-->

# Automating Customer Issue Triage with Azure SRE Agent and MCP

_Tired of sorting tickets and labeling GitHub issues manually each morning?_ This guide from dchelupati outlines how Azure SRE Agent, coupled with MCP integration, handles customer issue triage end-to-end—removing the repetitive overhead for engineering and support teams.

## The Triage Tax: Why Manual Sorting Wastes Time

Handling incoming issues from GitHub, identifying problem categories, and routing them to the correct engineering teams is tedious and prone to delays. Manually asking for missing information, labeling, and creating incidents eats into productive hours.

## Solution Overview: Azure SRE Agent + MCP

- **SRE Agent**: Created in the Azure portal as an automated core.
- **MCP (Model Context Protocol)**: Connects the agent directly to GitHub and PagerDuty APIs.
- **Custom triage rubric**: Uploaded as a markdown knowledge file specifying classification logic and routing rules.
- **Scheduled execution**: Agent runs twice daily, autonomously triaging new issues without human intervention.

## Detailed Step-by-Step Setup

### 1. Create the SRE Agent

Set up in the Azure portal, focused solely on issue triage—no Azure resource groups are required.

### 2. Connect MCP Servers

- **GitHub MCP**: For issue fetching, commenting, labeling.
- **PagerDuty MCP**: For creating incidents when a bug is confirmed.

MCP adapters enable connection to any tool with an API.

### 3. Create Specialized Subagents

- **GitHub Issue Triager**: Classifies issues (bugs, documentation, feature requests, or requests for more info) using uploaded rubric; hands off confirmed bugs to Incident Creator.
- **Incident Creator**: Manages PagerDuty and creates incidents for engineering. Returns workflow control to triager.

### 4. Upload Triage Knowledge Base

Document the logic for classification, required info, labels, and routing in markdown. This model is referenced for agent decisions.

### 5. Add Scheduled Task

Configure the agent to run the complete workflow twice a day for fully automated ticket management.

## What the Automated Workflow Accomplishes

- Fetches all open, unclassified GitHub issues
- Classifies and checks for required info using rubric
- Posts clarification comments when needed
- Labels issues appropriately
- Automatically creates PagerDuty incidents for actionable bugs
- Streamlines routing and collaboration across engineering teams

## The Benefits

- **Drastically reduced manual sorting and triage time**
- **Faster, more consistent classification and initial response**
- **Immediate incident creation for urgent bugs**
- **Flexible integration with any tool stack via MCP**
- **Teams refocus on genuine engineering over ticket triage**

## Tips for Success

- Test MCP endpoints before agent configuration
- Limit each subagent’s tool access for safety
- Start in read-only mode, then enable agent comments once trusted

---

Automating triage isn’t just about saving time—it enables engineering teams to prioritize meaningful work and improve issue responsiveness. With SRE Agent and MCP on Azure, DevOps practices become scalable, extensible, and tailored to your stack.

**Questions or alternative workflows? Comment and share your experience!**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/extend-sre-agent-with-mcp-build-an-agentic-workflow-to-triage/ba-p/4480710)
