---
layout: "post"
title: "Introducing Multi-Agent Workflows in Foundry Agent Service"
description: "This news article introduces the public preview of multi-agent workflows in Foundry Agent Service, a Microsoft-built platform for orchestrating AI agents in enterprise environments. The post details new workflow capabilities, including visual and YAML-based design, observability features, and integration with Visual Studio Code. It outlines benefits for developers and business users, customer feedback, and provides links to documentation and getting started guides."
author: "Mona Whalin"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/introducing-multi-agent-workflows-in-foundry-agent-service/"
viewing_mode: "external"
feed_name: "Microsoft AI Foundry Blog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2025-11-25 16:00:37 +00:00
permalink: "/news/2025-11-25-Introducing-Multi-Agent-Workflows-in-Foundry-Agent-Service.html"
categories: ["AI", "Azure"]
tags: ["Agent Evaluation", "Agent Framework", "Agents", "AI", "AI Agent", "AI Agents", "AI Applications", "AI Development", "AI Orchestration", "AI Tools", "Azure", "Azure AI", "Azure AI Services", "Azure OpenAI", "CI/CD Integration", "Developer Guide", "Enterprise Automation", "Foundry Agent Service", "Microsoft Agent Framework", "Microsoft Foundry", "MSIgnite", "Multi Agent Workflows", "News", "Observability", "Power Fx", "VS Code", "Workflow Automation", "YAML Workflows"]
tags_normalized: ["agent evaluation", "agent framework", "agents", "ai", "ai agent", "ai agents", "ai applications", "ai development", "ai orchestration", "ai tools", "azure", "azure ai", "azure ai services", "azure openai", "cislashcd integration", "developer guide", "enterprise automation", "foundry agent service", "microsoft agent framework", "microsoft foundry", "msignite", "multi agent workflows", "news", "observability", "power fx", "vs code", "workflow automation", "yaml workflows"]
---

Mona Whalin presents Microsoft's multi-agent workflow capabilities in Foundry Agent Service, allowing enterprises to visually or programmatically orchestrate complex AI processes with end-to-end observability and integration in Visual Studio Code.<!--excerpt_end-->

# Introducing Multi-Agent Workflows in Foundry Agent Service

Microsoft has launched the public preview of multi-agent workflows in Foundry Agent Service. This platform is built to help organizations move from basic AI agent experiments to deploying AI at the center of business operations, supporting complex, multi-step processes that span teams and require strong governance.

## Why Multi-Agent Workflows?

- Enterprises need to coordinate multiple agents, tools, and logic into reliable, end-to-end processes
- Single agents excel at narrow tasks, but real-world operations demand coordination, versioning, governance, and both code-level and visual design
- Both business users and developers are supported with code- and no-code tools

## Key Capabilities

1. **Visual Workflow Builder**: Drag-and-drop canvas for planning workflows without needing to write orchestration code, suitable for operations teams and analysts.
2. **YAML Workflow Definition**: Switch between visual and code-based (YAML) representations, with seamless synchronization for collaboration between business and engineering users.
3. **Orchestration Templates**: Starter patterns for sequential flows, human-in-the-loop steps, and group chat-based coordination. Useful for workflows like approvals and case management.
4. **Variable and JSON Schema Outputs**: Consistent data handling and enforceable output structures across steps.
5. **Power Fx Expression Support**: Add familiar Excel-like logic for conditions and computed values in workflows.
6. **Versioning and Design Notes**: Every save is versioned, and you can annotate workflow design choices for transparency or auditing.

## End-to-End Observability

Foundry Agent Service equips teams with:

- **Granular Tracing**: Follow every step, agent call, branch, and variable change
- **Built-in and Custom Evaluators**: Assess correctness, safety, compliance, and organization-specific metrics
- **Automated Monitoring**: Schedule or trigger evaluations, detect drifts, and monitor stats like throughput and agent activity
- **Operational Insights**: Help teams identify pain points or areas for improvement as workflows evolve

## Developer Experience: Visual Studio Code Integration

Developers can use the AI Toolkit extension pack to:

- Build, debug, and deploy multi-agent workflows
- Work with YAML or visual flows interchangeably
- Inspect workflow structure and run sample code in their preferred environment
- Utilize GitHub Copilot for code generation, orchestration, and evaluation
- Deploy workflows directly to Foundry
- Add custom evaluations and implement new operational metrics

## Customer Momentum

Companies such as Capgemini, Encora, Commerzbank AG, and Mazda have adopted these capabilities to automate, scale, and govern critical operations using multi-agent workflows in Foundry. Use cases range from contract management and legal workflows to composable delivery operating systems and digital banking solutions.

## Getting Started & Next Steps

- Check Microsoft documentation for [Foundry Agent Service Workflows](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/concepts/workflow?view=foundry)
- Experiment in Foundry Portal's workflow builder
- Try the AI Toolkit for Visual Studio Code for YAML and code-based flows
- Explore evaluation tools, monitoring, and agent observability features
- Watch relevant breakout sessions from Microsoft Ignite

Microsoft is positioning agent orchestration and workflow design as a foundational piece for modern intelligent operations—enabling secure, scalable AI solutions tailored for enterprise needs.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/introducing-multi-agent-workflows-in-foundry-agent-service/)
