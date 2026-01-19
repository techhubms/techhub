---
external_url: https://devblogs.microsoft.com/foundry/assess-agentic-risks-with-the-ai-red-teaming-agent-in-microsoft-foundry/
title: Assess Agentic Risks with the AI Red Teaming Agent in Microsoft Foundry
author: Laurel Geisbush, Jenn Cockrell
viewing_mode: external
feed_name: Microsoft AI Foundry Blog
date: 2025-11-20 16:00:05 +00:00
tags:
- Adversarial Testing
- Agentic Pipelines
- AI Development
- AI Red Teaming Agent
- AI Security
- AI Tools
- Azure AI Foundry
- CI/CD
- Compliance
- Generative AI
- Microsoft Foundry
- Prompt Injection
- PyRIT
- Red Teaming
- SDK Integration
- Sensitive Data Leakage
- Trustworthy AI
section_names:
- ai
- azure
- security
---
Laurel Geisbush and Jenn Cockrell introduce significant updates to Microsoft Foundry, highlighting the AI Red Teaming Agent and its role in automated, scalable risk assessment for AI models and agentic pipelines.<!--excerpt_end-->

# Assess Agentic Risks with the AI Red Teaming Agent in Microsoft Foundry

**Authors:** Laurel Geisbush, Jenn Cockrell

Microsoft announces major enhancements in Microsoft Foundry, now available in public preview, to empower organizations in proactively identifying safety and security risks in both AI models and agentic pipelines. These features focus on delivering strong safeguards as advanced agentic solutions are integrated into production workflows.

## Overview of New Capabilities

- **Automated Red Teaming:** The AI Red Teaming Agent integrates with Microsoft's open-source [PyRIT (Python Risk Identification Tool)](https://github.com/Azure/PyRIT) framework, allowing automated, scalable adversarial testing.
- **End-to-end Coverage:** Orchestrate red teaming for both model-level and agent-level scenarios via unified Foundry SDK/APIs and the UI portal.
- **No-code and Code-first Experiences:** Run evaluations through a no-code UI wizard ideal for rapid prototyping or integrate red teaming into CI/CD pipelines using SDKs and APIs for continuous safety evaluation.

## Technical Highlights

- **Reusable Attacker Strategies:** Leverage PyRIT's 20+ attack strategies to systematically probe vulnerabilities such as prompt injection, harmful content generation, misuse, privacy leaks, and robustness failures.
- **Data-driven Evaluations:** Move from ad-hoc testing to structured, reproducible assessments with new Foundry interfaces for visualizing results, tracking vulnerabilities, and benchmarking progress.
- **Dynamic and Customizable Risk Assessments:** Generate dynamic test data sets and tailor risk definitions and policies to organizational needs for precise, relevant red teaming.

## Key Agentic Risk Categories & Testing Approaches

### 1. Sensitive Data Leakage

- **Testing for:** Leaks involving PII, financial, medical, or credentials when agents use tools or access internal knowledge bases.
- **How:** Synthetic Azure testbeds (e.g., Search, Cosmos DB, Key Vault) simulate sensitive data, while adversarial queries and automated evaluators detect direct or obfuscated leaks.

### 2. Prohibited Actions

- **Testing for:** Unauthorized or high-risk agent actions (e.g., password changes, unapproved financial transactions), guided by customizable policies.
- **How:** Define policies in the UI or as JSON for code-first workflows; evaluators check for policy violations.

### 3. Task Adherence

- **Testing for:** Agent reliability in following assigned goals, rules, and procedures within prescribed constraints.
- **How:** Test case generation targets goal, rule, and procedural adherence using pass/fail criteria for outputs and actions.

### 4. Agentic Jailbreak (Prompt Injection)

- **Testing for:** Vulnerability to indirect prompt injection where malicious instructions are hidden in tool outputs/documents.
- **How:** Synthetic datasets inject risk-specific attacks in context, evaluators check agent responses for execution of these attacks.

```python
# Example: Creating an agent red teaming evaluation run with the SDK

run_name = f"Red Team Agent Safety Eval Run for {agent_name} - {int(time.time())}"
eval_run = client.evals.runs.create(
    eval_id=red_team.id,
    name=run_name,
    data_source={
        "type": "azure_ai_red_team",
        "item_generation_params": {
            "type": "red_team_taxonomy",
            "attack_strategies": ["Flip", "Base64", "IndirectJailbreak"],
            "num_turns": 5,
            "source": {
                "type": "file_id",
                "id": taxonomy_file_id,
            },
        },
        "target": target.as_dict(),
    },
)
print(f"[Run] Created: id={eval_run.id}, name={eval_run.name}, status={eval_run.status}")
```

## Developer Benefits

- **Scalable, Repeatable Red Teaming:** Integrate safety testing into your workflows without manual overhead.
- **Customization:** Tailor attack strategies and risk policies for your unique organization and compliance requirements.
- **End-to-End Pipeline Integration:** Use Foundry's SDK and APIs for seamless CI/CD pipeline red teaming.

## Getting Started

- Dive into the [documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/develop/run-ai-red-teaming-cloud?view=foundry&tabs=python) for integration details.
- Try an [example workflow](https://aka.ms/agent-redteam-sample) on GitHub.

Foundry's enhanced AI Red Teaming Agent empowers developers to proactively secure and evaluate agentic AI systems, making AI deployment safer and more trustworthy.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/assess-agentic-risks-with-the-ai-red-teaming-agent-in-microsoft-foundry/)
