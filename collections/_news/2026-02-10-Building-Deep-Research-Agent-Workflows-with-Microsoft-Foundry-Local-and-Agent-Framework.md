---
external_url: https://devblogs.microsoft.com/semantic-kernel/from-local-models-to-agent-workflows-building-a-deep-research-solution-with-microsoft-agent-framework-on-microsoft-foundry-local/
title: Building Deep Research Agent Workflows with Microsoft Foundry Local and Agent Framework
author: Kinfey Lo
primary_section: ai
feed_name: Microsoft Semantic Kernel Blog
date: 2026-02-10 11:12:33 +00:00
tags:
- .NET Aspire
- Agent Framework
- Agent Workflows
- Agents
- AI
- AI Security
- Azure
- Cloud Edge Collaboration
- DevUI
- Edge AI
- Enterprise AI
- Local AI Models
- MAF
- Microsoft Agent Framework
- Microsoft Foundry Local
- Model Evaluation
- News
- Observability
- OpenTelemetry
- Privacy
- Python
- Qwen2.5 1.5B
- Red Teaming
- Workflow Orchestration
section_names:
- ai
- azure
---
Kinfey Lo explains how to build enterprise-ready agent workflows using Microsoft Foundry Local and Agent Framework, with a focus on local AI, privacy, security evaluation, and orchestration.<!--excerpt_end-->

# Building Deep Research Agent Workflows with Microsoft Foundry Local and Agent Framework

## Introduction: A New Paradigm for AI Application Development

Enterprise AI developers face challenges such as data privacy, network latency, and cost when relying solely on cloud-based large language models. Microsoft Foundry Local, together with the Microsoft Agent Framework (MAF), provides a way to run powerful AI agents locally while maintaining privacy and control.

## Why Choose Foundry Local?

- **Privacy-First:** All inference occurs locally for compliance-sensitive scenarios.
- **Zero Latency:** Ideal for real-time or interactive use cases with no network dependency.
- **Cost Control:** Avoids per-call API costs for high-frequency workloads.
- **Rapid Iteration:** Local debugging and development feedback loops.
- **Enterprise Integration:** Works seamlessly with Microsoft’s broader AI and Azure ecosystem.

Example agent initialization:

```python
agent = FoundryLocalClient(model_id="qwen2.5-1.5b-instruct-generic-cpu:4").as_agent(
    name="LocalAgent",
    instructions="""
    You are an assistant.
    Your responsibilities:
    - Answering questions and providing professional advice
    - Helping users understand concepts
    - Offering users different suggestions
    """
)
```

## Agent Evaluation: Security, Quality, and Observability

Three methods are used to evaluate agent robustness and reliability:

1. **Red Teaming (Security and Robustness):**
   - Systematically tests the agent with adversarial prompts across risk categories such as violence, hate/unfairness, sexual, and self-harm content.
   - Outputs traceable JSON results for monitoring and improvement.

2. **Self-Reflection (Quality Check):**
   - The agent reviews its own outputs for factual consistency and suggests improvements. This secondary review helps identify errors and refine responses.

3. **Observability (Performance Metrics):**
   - Tracks metrics like end-to-end latency, per-stage time, and tool invocation using OpenTelemetry.

Example red team evaluation setup:

```python
from azure.ai.evaluation.red_team import AttackStrategy, RedTeam, RiskCategory
from azure.identity import AzureCliCredential
from agent_framework_foundry_local import FoundryLocalClient

credential = AzureCliCredential()
agent = FoundryLocalClient(model_id="qwen2.5-1.5b-instruct-generic-cpu:4").as_agent(...)

# RedTeam configuration ... (see main article)
```

## Workflow Orchestration: Deep Research Loop

The core pattern is a multi-round “research-judge-research again” cycle orchestrated by MAF Workflows:

- **Research Agent:** Uses web search tools, generates summaries, accumulates knowledge context.
- **Iteration Controller:** Decides whether to continue data gathering or generate a final report; prevents infinite search loops.
- **Final Reporter:** Aggregates knowledge and outputs a structured, cite-supported report.

Workflow code (simplified):

```python
workflow_builder = WorkflowBuilder(
    name="Deep Research Workflow",
    description="Multi-agent deep research workflow with iterative web search"
)
workflow_builder.register_agent(...)
workflow_builder.add_edge("start_executor", "research_executor")

# Further edges and logic as in main content ...
```

## Debugging: DevUI for Interactive Observation

MAF DevUI delivers a visual debugging interface:

- **Topology Diagram:** Visualizes agent workflow and steps.
- **Input/Output Inspection:** Enables tracking and modification of each execution stage.
- **Real-Time Logs:** Consolidates logs and agent outputs for quick problem-solving.

Example debugging session:

- User enters research query (e.g., “GPT-5.3-Codex vs Anthropic Claude 4.6”)
- Observe keyword evolution, controller decisions, and report coverage.

## Performance Optimization: .NET Aspire and Telemetry

- Enable OpenTelemetry for automated latency, throughput, and model/tool performance metrics.
- Optimize by adjusting model size (Qwen2.5-1.5B vs larger), caching, reducing unnecessary iterations, and streaming output.

## Quick Start Guide

1. **Set up environment:** Configure required endpoints and credentials, install dependencies with pip (see main article).
2. **Run security evaluation:**
   - `python 01.foundrylocal_maf_evaluation.py`
3. **Debug interactively:**
   - `python 02.foundrylocal_maf_workflow_deep_research_devui.py` (browse to localhost:8093)
4. **Production CLI:**
   - `python 02.foundrylocal_maf_workflow_deep_research_devui.py --cli`

See the [GitHub repo](https://github.com/microsoft/Agent-Framework-Samples/blob/main/09.Cases/FoundryLocalPipeline/) for full code samples and walkthroughs.

## Architectural Insights

- **Three Layers:**
  1. Model (Foundry Local inference)
  2. Agent (business logic and tool encapsulation)
  3. Orchestration (workflow management with MAF)
- **Advantages**: Automatic error handling, observability, composability.

## Use Cases and Extensions

- **Ideal For:** Multi-round research, enterprise data privacy, frequent cost-sensitive requests, offline or edge deployment.
- **Extensible With:** Multi-agent design, private document search (vector databases), human-in-the-loop checkpoints, multimodal data support.

## Conclusion

The blend of Microsoft Foundry Local and Agent Framework opens new possibilities for local, production-grade AI agents. This approach brings security, traceability, and flexibility—including cloud-edge collaboration—for enterprise and developer needs.

**Related Resources:**

- [Agent Framework GitHub](https://github.com/microsoft/agent-framework)
- [Agent Framework Samples](https://github.com/microsoft/agent-framework-samples)
- [Foundry Local Documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-local/what-is-foundry-local)

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/from-local-models-to-agent-workflows-building-a-deep-research-solution-with-microsoft-agent-framework-on-microsoft-foundry-local/)
