---
author: Arjun Chakraborty
title: 'CTI-REALM: A new benchmark for end-to-end detection rule generation with AI agents'
date: 2026-03-20 16:19:00 +00:00
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/20/cti-realm-a-new-benchmark-for-end-to-end-detection-rule-generation-with-ai-agents/
primary_section: ai
section_names:
- ai
- azure
- security
feed_name: Microsoft Security Blog
tags:
- AI
- AI Agents
- AKS
- Arxiv
- Azure
- Azure Cloud Infrastructure
- Benchmarking
- CTI REALM
- Cyber Threat Intelligence
- Detection Engineering
- Ground Truth Scoring
- KQL
- Kusto Query Language
- Linux Telemetry
- LLM Evaluation
- Microsoft Agent 365
- Microsoft Security Blog
- MITRE ATT&CK
- News
- Security
- Security Operations
- Sigma Rules
- Threat Reports
- Tool Using Agents
---

Arjun Chakraborty introduces CTI-REALM, Microsoft’s open-source benchmark for evaluating whether AI agents can turn cyber threat intelligence into validated detections by working end-to-end (reading CTI, exploring telemetry, iterating on KQL, and producing Sigma and KQL detection rules) across Linux, AKS, and Azure environments.<!--excerpt_end-->

# CTI-REALM: A new benchmark for end-to-end detection rule generation with AI agents

CTI-REALM (Cyber Threat Real World Evaluation and LLM Benchmarking) is Microsoft’s open-source benchmark for evaluating AI agents on **end-to-end detection engineering**, focused on whether an agent can translate cyber threat intelligence (CTI) into **validated detections**.

Rather than measuring “CTI trivia” (parametric knowledge), CTI-REALM tests workflows security analysts actually perform:

- Reading threat intelligence reports
- Exploring telemetry
- Writing and iterating on **KQL** queries
- Producing **Sigma rules** and KQL-based detection logic
- Scoring outputs against **ground truth** across:
  - Linux endpoints
  - Azure Kubernetes Service (AKS)
  - Azure cloud infrastructure

> Security is Microsoft’s top priority. Every day, we process more than 100 trillion security signals across endpoints, cloud infrastructure, identity, and global threat intelligence. That’s the scale modern cyber defense demands, and AI is a core part of how we protect Microsoft and our customers worldwide. At the same time, security is, and always will be, a team sport.

> That’s why Microsoft is committed to AI model diversity and to helping defenders apply the latest AI responsibly. We created CTI‑REALM and open‑sourced it so the broader industry can test models, write better code, and build more secure systems together.

## What CTI-REALM evaluates

CTI-REALM builds on prior benchmarking work like **ExCyTIn-Bench** (threat investigation) and extends the evaluation into **detection rule generation**.

Agents are placed in a realistic environment and asked to:

- Read a threat intelligence report
- Explore telemetry
- Write and refine KQL
- Produce validated detection rules

## Data and benchmark scope

Microsoft curated **37 CTI reports** from public sources, including:

- Microsoft Security
- Datadog Security Labs
- Palo Alto Networks
- Splunk

Reports were selected based on whether they could be simulated in a sandbox and produce telemetry suitable for detection rule development.

The benchmark spans three platforms:

- Linux endpoints
- AKS
- Azure cloud infrastructure

Scoring uses **ground truth** and is applied throughout the workflow.

## Why CTI-REALM exists

Existing cybersecurity benchmarks often focus on parametric knowledge, for example:

- Mapping a log entry to a MITRE technique
- Classifying TTPs from a report

CTI-REALM targets a harder question: can an agent **operationalize** CTI into detection logic that works against real telemetry?

CTI-REALM measures:

- **Operationalization, not recall**: translating narrative CTI into working Sigma rules and KQL queries validated against attack telemetry
- **The full workflow**: scoring includes intermediate steps (report selection, MITRE mapping, data source identification, iterative refinement), not just the final rule
- **Realistic tooling**: CTI repositories, schema explorers, a Kusto query engine, MITRE ATT&CK and Sigma rule databases

## Business impact (as described in the post)

CTI-REALM is positioned as a repeatable way for security engineering leaders to evaluate whether a model improves detection coverage and analyst output.

The checkpoint-based scoring is meant to show *where* a model fails (e.g., CTI comprehension vs query construction vs detection specificity), which can inform decisions about:

- Human review requirements
- Guardrails before any operational deployment

## Latest results

The post reports evaluation of **16 frontier model configurations** on **CTI-REALM-50** (50 tasks across Linux, AKS, and Cloud).

![Model performance on CTI-REALM-50, sorted by normalized reward](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/image-1.gif)

## What the numbers tell us

Key takeaways listed in the post:

- **Anthropic models lead**: Claude occupies the top three positions (0.587–0.637), attributed to stronger tool use and iterative query behavior versus OpenAI models.
- **More reasoning isn’t always better**: within the GPT-5 family, “medium reasoning” beats “high” across three generations, suggesting overthinking can hurt in agentic settings.
- **Cloud detection is hardest**: performance drops from Linux (0.585) to AKS (0.517) to Cloud (0.282), reflecting cross-data-source correlation difficulty in APT-style scenarios.
- **CTI tools matter**: removing CTI-specific tools reduced output quality by up to 0.150 points, with the biggest impact on final rule quality.
- **Structured guidance helps**: human-authored workflow tips for a smaller model closed about a third of the gap to a larger model, mainly by improving technique identification.

## Paper and resources

- Paper: [\[2603.13517\] CTI-REALM: Benchmark to Evaluate Agent Performance on Security Detection Rule Generation Capabilities](https://arxiv.org/abs/2603.13517)
- Inspect AI evals repo (availability “soon” per the post): [UKGovernmentBEIS/inspect_evals: Collection of evals for Inspect AI](https://github.com/UKGovernmentBEIS/inspect_evals)
- Related PR: [CTI-REALM: Cyber Threat Intelligence Detection Rule Development Benchmark (PR #1270)](https://github.com/UKGovernmentBEIS/inspect_evals/pull/1270)

## Get involved

CTI-REALM is described as open-source and free to access. Model developers and security teams are invited to contribute and share results via the official GitHub repository.

Contact email in the post: msecaimrbenchmarking@microsoft[.]com

## References

1. [Microsoft raises the bar: A smarter way to measure AI for cybersecurity | Microsoft Security Blog](https://www.microsoft.com/en-us/security/blog/2025/10/14/microsoft-raises-the-bar-a-smarter-way-to-measure-ai-for-cybersecurity/?msockid=1e918fa42a0668b10b8499822b646944)
2. [\[2603.13517\] CTI-REALM: Benchmark to Evaluate Agent Performance on Security Detection Rule Generation Capabilities](https://arxiv.org/abs/2603.13517)
3. [CTI-REALM: Cyber Threat Intelligence Detection Rule Development Benchmark (PR #1270)](https://github.com/UKGovernmentBEIS/inspect_evals/pull/1270)


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/20/cti-realm-a-new-benchmark-for-end-to-end-detection-rule-generation-with-ai-agents/)

