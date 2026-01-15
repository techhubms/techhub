---
layout: post
title: Building a multi-agent system with Semantic Kernel
author: Geekodon
canonical_url: https://www.reddit.com/r/dotnet/comments/1ltr8tf/building_a_multiagent_system_with_semantic_kernel/
viewing_mode: external
feed_name: Reddit DotNet
feed_url: https://www.reddit.com/r/dotnet/.rss
date: 2025-07-07 11:01:49 +00:00
permalink: /github-copilot/community/Building-a-multi-agent-system-with-Semantic-Kernel
tags:
- .NET
- Agentic AI
- AI
- Coding
- Community
- Copilot UI
- Executor Agent
- GitHub Copilot
- Human in The Loop
- Planner Agent
- Process Automation
- Reviewer Agent
- Semantic Kernel
section_names:
- ai
- coding
- github-copilot
---
In this article, Geekodon presents a practical demo of an agentic AI system built using Semantic Kernel, highlighting key components and workflow strategies. <!--excerpt_end--> The demo showcases an architecture where three main agents—Planner, Reviewer, and Executor—work collaboratively to process and execute user requests.

## System Overview

- **Planner**: Analyzes the user's input and formulates an action plan.
- **Reviewer**: Evaluates the Planner's proposed tasks and provides feedback for refinement.
- **Executor**: Implements the step-by-step actions, relying on user approval before each step, modeled with a Copilot-style interface.

This agentic approach emphasizes a human-in-the-loop workflow, where users remain involved throughout the process by approving planned actions before execution, fostering transparency and trust in automated systems.

## Implementation Details

Geekodon outlines the implementation steps:

1. **Semantic Kernel Initialization**:
   - Build the core kernel and register necessary services in `AgentService.cs: Init`.
2. **Agent Creation**:
   - Use the `ChatCompletionAgent` class to instantiate agents, detailed in `AgentService.cs: CreateAgent`.
3. **Plugin Integration**:
   - Enhance the Executor agent with tools and plugins, configured via `AgentService.cs: Init`.
4. **Process Steps Specification**:
   - Define granular steps for each agent in `AiProcessSteps.cs`.
5. **Process Flow Design**:
   - Structure data transfers between Planner, Reviewer, and Executor so feedback loops are enabled (`AgentService.cs: InitProcess`).
6. **Human-in-the-Loop Integration**:
   - Add user proxy steps and set up communication with an external client, creating a message channel (`AgentService.cs: ExternalClient`).
   - Ensure messages flow to the process runner (`AgentService.cs: StartNewTaskProcessAsync`).

## Copilot-Style User Experience

A unique characteristic of this system is its Copilot-style user interface, ensuring users can monitor and guide each AI-generated step. This design optimizes reliability and user control, aligning agentic systems with practical deployment needs.

## Conclusion

Geekodon's demonstration serves as a guide for developers interested in building agentic, transparent, and user-centered AI assistants using Semantic Kernel. The full code and further discussion can be accessed via the provided links.

This post appeared first on Reddit DotNet. [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1ltr8tf/building_a_multiagent_system_with_semantic_kernel/)
