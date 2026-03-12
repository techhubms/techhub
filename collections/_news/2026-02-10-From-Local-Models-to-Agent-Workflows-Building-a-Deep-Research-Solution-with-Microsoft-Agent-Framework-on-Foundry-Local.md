---
layout: "post"
title: "From Local Models to Agent Workflows: Building a Deep Research Solution with Microsoft Agent Framework on Foundry Local"
description: "This comprehensive guide demonstrates how to build a production-grade enterprise AI workflow using Microsoft Foundry Local and Agent Framework. It covers model safety evaluation, agent orchestration, interactive debugging with DevUI, performance monitoring with .NET Aspire, and practical deployment approaches, offering a complete blueprint for privacy-preserving, high-performance local AI agent solutions."
author: "Kinfey Lo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/agent-framework/from-local-models-to-agent-workflows-building-a-deep-research-solution-with-microsoft-agent-framework-on-microsoft-foundry-local/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2026-02-10 11:12:33 +00:00
permalink: "/2026-02-10-From-Local-Models-to-Agent-Workflows-Building-a-Deep-Research-Solution-with-Microsoft-Agent-Framework-on-Foundry-Local.html"
categories: ["AI"]
tags: [".NET Aspire", "Agent Framework", "Agents", "AI", "AI Security", "Deep Research Agents", "DevUI", "Edge AI", "MAF", "Microsoft Foundry Local", "Model Evaluation", "News", "Observability", "OpenTelemetry", "Privacy", "Python", "Red Team Evaluation", "Workflow Orchestration"]
tags_normalized: ["dotnet aspire", "agent framework", "agents", "ai", "ai security", "deep research agents", "devui", "edge ai", "maf", "microsoft foundry local", "model evaluation", "news", "observability", "opentelemetry", "privacy", "python", "red team evaluation", "workflow orchestration"]
---

Kinfey Lo provides a step-by-step tutorial on developing enterprise-grade agent workflows using Microsoft Foundry Local and Agent Framework, highlighting privacy-preserving AI, safety evaluation, orchestration, and performance tuning.<!--excerpt_end-->

# From Local Models to Agent Workflows: Building a Deep Research Solution with Microsoft Agent Framework on Foundry Local

## Introduction: A New Paradigm for AI Application Development

Developers building enterprise AI applications often struggle to balance the power of cloud-based language models with concerns like data privacy, network latency, and cost. Local small models typically lack comprehensive development and orchestration tools. Microsoft Foundry Local combined with Agent Framework (MAF) provides an elegant solution, supporting fully local, secure, and performant AI application workflows.

This guide walks through building a complete "Deep Research" agent pipeline, detailing:

- Model safety evaluation
- Workflow orchestration
- Interactive debugging and visualization
- Performance evaluation and optimization

## Why Choose Foundry Local?

**Foundry Local** extends Microsoft's AI ecosystem to the edge, providing:

- **Privacy first**: All data and inference remain local, supporting compliance
- **Zero latency**: No network round trips, ideal for real-time scenarios
- **Cost control**: Avoids cloud API call fees, suitable for frequent usage
- **Rapid iteration**: Local development and debugging for quick feedback cycles

Foundry Local integrates with the Agent Framework, allowing developers to create agent-based workflows that match the productivity of Azure OpenAI, but with the flexibility of edge deployment.

**Sample Agent Creation:**

```python
agent = FoundryLocalClient(model_id="qwen2.5-1.5b-instruct-generic-cpu:4").as_agent(
    name="LocalAgent",
    instructions="""
    You are an assistant.
    Your responsibilities:
    - Answering questions and providing professional advice
    - Helping users understand concepts
    - Offering users different suggestions
    """,
)
```

## Agent Evaluation Techniques

Robust agents need thorough evaluation. The article covers three methods supported by the Agent Framework:

1. **Red Teaming**: Systematically adversarial prompts test model boundaries across risk categories (violence, hate, sexual content, self-harm).
2. **Self-Reflection**: Agents review their own outputs for accuracy and structure, providing improvement suggestions.
3. **Observability**: Performance metrics (latency, time consumption, invocation overhead) are reported using OpenTelemetry for detailed workflow traceability.

## Step-by-Step Development Process

### 1. Red Team Evaluation

Ensure safety before production deployment:

- Use MAF's red teaming utilities to challenge the model.
- Risk categories and attack strategies are configurable.
- Results are saved as JSON for traceability and improvement.

```python
from azure.ai.evaluation.red_team import AttackStrategy, RedTeam, RiskCategory
red_team = RedTeam(
    azure_ai_project=os.environ["AZURE_AI_PROJECT_ENDPOINT"],
    credential=credential,
    risk_categories=[RiskCategory.Violence, RiskCategory.HateUnfairness, RiskCategory.Sexual, RiskCategory.SelfHarm],
    num_objectives=2,
)
results = await red_team.scan(...)
```

### 2. Designing the Deep Research Workflow

With MAF Workflows:

- **Research Agent** collects and accumulates external information.
- **Iteration Controller** decides whether to continue research or generate a final report.
- **Final Reporter** integrates iterative findings into structured output.

Workflow edges and agent logic support modularity, observability, and extensibility.

### 3. Debugging with DevUI

MAF DevUI enables step-by-step visualization:

- Visual topology of the workflow
- Inspection of each node's input and output
- Real-time parameter modification for scenario testing
- Aggregated logs for troubleshooting

**Example use case:**

- Explore "GPT-5.3-Codex vs Anthropic Claud 4.6"
- Observe keyword evolution and iteration controller decisions

### 4. Performance Evaluation and .NET Aspire Integration

Performance metrics (latency, throughput, tool usage, and memory) are automatically tracked with OpenTelemetry and .NET Aspire.

Optimization strategies suggested:

- Use smaller models for speed
- Cache common search results
- Limit research iteration depth
- Use streaming outputs for responsiveness

## Practical Quick Start Guide

1. **Environment Setup**: Configure environment variables for endpoints and deploy model locally.
2. **Security Evaluation**: Run `01.foundrylocal_maf_evaluation.py` and review findings.
3. **Interactive Debugging (DevUI)**: Launch `02.foundrylocal_maf_workflow_deep_research_devui.py` and explore workflows visually.
4. **Production CLI Mode**: Use CLI to output final reports directly.

GitHub repo: [Foundry Local Pipeline Sample](https://github.com/microsoft/Agent-Framework-Samples/blob/main/09.Cases/FoundryLocalPipeline/)

## Architectural Insights: Model, Agent, Orchestration

- **Model Layer**: Inference with Foundry Local
- **Agent Layer**: Agents plus tools encapsulate application logic
- **Orchestration Layer**: Workflows manage complexity and automation

Compared to traditional manual scripting, this approach offers auto-management of state, tool invocation, logging, observability, and robust error handling.

## Use Cases and Extensions

- Multi-round research tasks synthesizing information
- Enterprise use cases with data privacy or cost constraints
- Edge, offline, or weak network scenarios

**Extensions:**

- Add multiple agents (expert, code generator, analyst)
- Integrate your private knowledge bases via vector search
- Support human-in-the-loop workflows
- Handle various modalities (text, images, PDFs, etc.)

## Conclusion

Microsoft Foundry Local plus Agent Framework empowers developers to deploy reliable, privacy-preserving, and cost-effective AI agents at the edge. The framework-first method accelerates the move from prototype to production, with robust safety evaluation, workflow orchestration, live debugging, and modern observability.

### Related Links

- [Agent Framework GitHub](https://github.com/microsoft/agent-framework)
- [Agent Framework Samples GitHub](https://github.com/microsoft/agent-framework-samples)
- [Foundry Local Docs](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-local/what-is-foundry-local)

---

*Content summarized and structured by Kinfey Lo for enterprise AI application developers interested in local and edge AI workflows using Microsoft technologies.*

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/agent-framework/from-local-models-to-agent-workflows-building-a-deep-research-solution-with-microsoft-agent-framework-on-microsoft-foundry-local/)
