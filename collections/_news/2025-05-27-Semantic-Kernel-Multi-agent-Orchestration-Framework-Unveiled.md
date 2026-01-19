---
layout: post
title: 'Semantic Kernel: Multi-agent Orchestration Framework Unveiled'
author: Tao Chen, Chris Rickman
canonical_url: https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-multi-agent-orchestration/
viewing_mode: external
feed_name: Microsoft DevBlog
feed_url: https://devblogs.microsoft.com/semantic-kernel/feed/
date: 2025-05-27 16:54:32 +00:00
permalink: /ai/news/Semantic-Kernel-Multi-agent-Orchestration-Framework-Unveiled
tags:
- .NET
- Agent Orchestration
- Collaborative AI
- Concurrent Orchestration
- Developer Tools
- Group Chat Orchestration
- Handoff Orchestration
- Magentic Orchestration
- Microsoft AI
- Multi Agent Systems
- Orchestration Framework
- Python
- Semantic Kernel
- Sequential Orchestration
- Workflow Automation
section_names:
- ai
- coding
---
In this post, authors Tao Chen and Chris Rickman introduce and explain the new multi-agent orchestration framework for Semantic Kernel, highlighting orchestration patterns, developer experience, and practical use cases.<!--excerpt_end-->

# Semantic Kernel: Multi-agent Orchestration Framework Unveiled

*By Tao Chen, Chris Rickman*

The field of AI is rapidly evolving, creating an increasing need for sophisticated, collaborative, and flexible agent-based systems. To address this, Semantic Kernel introduces a new multi-agent orchestration framework enabling developers to build, manage, and scale complex agent workflows with greater ease and flexibility. This post explores new orchestration patterns, their capabilities, and guidance for leveraging them in your own AI projects.

---

## Why Multi-agent Orchestration?

Traditional single-agent systems can only go so far when handling multifaceted or complex tasks. By coordinating multiple agents, each with specialized skills or responsibilities, developers can create robust and adaptive systems that solve real-world problems more effectively through collaboration. The Semantic Kernel multi-agent orchestration framework provides a foundation for creating these systems and supports a range of coordination patterns.

---

## Orchestration Patterns in Semantic Kernel

Semantic Kernel offers several orchestration patterns designed for different collaboration models. These patterns are part of the core framework and can be extended as needed.

### 1. Sequential Orchestration

In this pattern, agents are arranged in a pipeline where each agent processes the task and passes its output to the next. Ideal for workflows such as document review or data processing pipelines.

**Use Case Example:**
A document is processed by a summarization agent, then passed to a translation agent, and finally reviewed by a quality assurance agent.

![Sequential Orchestration Diagram](https://devblogs.microsoft.com/semantic-kernel/wp-content/uploads/sites/78/2025/05/sequential.png)

---

### 2. Concurrent Orchestration

Here, multiple agents work on the same task in parallel, each independently processing the input, with their results collected and aggregated. This suits scenarios like brainstorming, ensemble reasoning, or voting systems.

**Use Case Example:**
Multiple agents generate diverse solutions to a problem, and their outputs are analyzed or selected as needed.

![Concurrent Orchestration Diagram](https://devblogs.microsoft.com/semantic-kernel/wp-content/uploads/sites/78/2025/05/concurrent.png)

---

### 3. Group Chat Orchestration

This pattern simulates collaborative conversations among agents—optionally including a human participant—coordinated by a group chat manager. Powerful for meetings, debates, or collaborative problem-solving.

**Use Case Example:**
Agents representing different departments discuss a business proposal, with a manager agent moderating and a human involved as needed.

![Group Chat Orchestration Diagram](https://devblogs.microsoft.com/semantic-kernel/wp-content/uploads/sites/78/2025/05/group_chat.png)

---

### 4. Handoff Orchestration

With handoff orchestration, agents may delegate control to other agents based on context or user requests, enabling dynamic expertise assignment. Useful in support, expert systems, or situations requiring dynamic delegation.

**Use Case Example:**
A customer support agent handles an inquiry and hands off to a technical expert or billing agent based on the user's needs.

![Handoff Orchestration Diagram](https://devblogs.microsoft.com/semantic-kernel/wp-content/uploads/sites/78/2025/05/Handoff-1.png)

---

### 5. Magentic Orchestration

Inspired by the MagenticOne pattern from AutoGen, this general-purpose multi-agent model supports open-ended, dynamic collaboration. A dedicated Magentic manager coordinates agents, selecting which should act next based on evolving context and agent expertise.

**Use Case Example:**
To create a comprehensive report comparing energy efficiency across machine learning models, the Magentic manager first assigns research to one agent, then analysis tasks to another, and compiles the results after iterative collaboration.

![Magentic Orchestration Diagram](https://devblogs.microsoft.com/semantic-kernel/wp-content/uploads/sites/78/2025/05/magentic.png)

*Image by [AutoGen](https://microsoft.github.io/autogen/stable/user-guide/agentchat-user-guide/magentic-one.html)*

---

### Additional Patterns and Community Involvement

Semantic Kernel continues to explore and incorporate new agentic patterns and encourages the community to contribute feedback and unique use cases to enhance the orchestration toolkit further.

---

## Simplicity and Developer Experience

All orchestration patterns in Semantic Kernel utilize a unified construction and invocation interface. Regardless of the chosen pattern, developers can:

- Define agents and their capabilities ([see Agents documentation](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-agents-are-now-generally-available/))
- Create orchestrations by passing agents and (if required) a manager
- Optionally provide callbacks/transforms for I/O customization
- Start a runtime and invoke orchestrations with a task
- Await results in an asynchronous, uniform manner

This unified model enables rapid switching between orchestration types without significant changes to code or logic structure.

**Python Example:**

```python
# Choose an orchestration pattern with your agents

orchestration = SequentialOrchestration(members=[agent_a, agent_b]) # or use other orchestration classes

# Start the runtime

runtime = InProcessRuntime()
runtime.start()

# Invoke the orchestration

result = await orchestration.invoke(task="Your task here", runtime=runtime)

# Get the result

final_output = await result.get()

await runtime.stop_when_idle()
```

**.NET Example:**

```csharp
// Choose an orchestration pattern with your agents
SequentialOrchestration orchestration = new(agentA, agentB) {
    LoggerFactory = this.LoggerFactory
}; // or use other orchestration classes

// Start the runtime
InProcessRuntime runtime = new();
await runtime.StartAsync();

// Invoke the orchestration and get the result
OrchestrationResult<string> result = await orchestration.InvokeAsync(task, runtime);
string text = await result.GetValueAsync();

await runtime.RunUntilIdleAsync();
```

This consistency means developers can more easily prototype and scale complex, multi-agent solutions.

---

## Getting Started Resources

Semantic Kernel provides comprehensive samples and documentation:

- **Python Samples:** [Getting Started with Multi-agent Orchestration (GitHub)](https://github.com/microsoft/semantic-kernel/tree/main/python/samples/getting_started_with_agents/multi_agent_orchestration)
- **.NET Samples:** [Getting Started with Orchestration (GitHub)](https://github.com/microsoft/semantic-kernel/tree/main/dotnet/samples/GettingStartedWithAgents/Orchestration)
- **Documentation:** [Microsoft Learn: Multi-agent Orchestration](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/agent-orchestration/)

---

## Conclusion

Multi-agent orchestration in Semantic Kernel opens up new possibilities for intelligent, collaborative solutions. Whether you need sequential, parallel, conversational, or dynamic coordination, these orchestration patterns provide the flexibility and power needed for modern AI applications.

Explore the samples, experiment with the patterns, and start building with Semantic Kernel today! The Semantic Kernel team encourages your feedback and creativity. For questions or to join the discussion, visit the [Semantic Kernel GitHub Discussion Channel](https://github.com/microsoft/semantic-kernel/discussions/categories/general). If you appreciate Semantic Kernel, please consider starring the [GitHub repository](https://github.com/microsoft/semantic-kernel).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/semantic-kernel-multi-agent-orchestration/)
