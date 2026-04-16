---
date: 2026-04-16 21:05:45 +00:00
title: 'Introducing the Container Network Insights Agent for AKS: Now in Public Preview'
tags:
- Agentic AI
- AI
- AKS
- Azure
- Azure CNI
- Azure Monitor
- Azure OpenAI
- Cilium
- CiliumNetworkPolicy
- Community
- Container Insights
- Container Network Insights Agent
- CoreDNS
- DaemonSet
- DevOps
- Ebpf
- Grafana
- Hubble
- Kernel Telemetry
- Kubectl
- Managed Identity
- MCP
- MCP Server
- MTTR
- Network Troubleshooting
- NetworkPolicy
- Observability
- Packet Drops
- Prometheus
- RBAC
- SoftIRQ
- Workload Identity Federation
section_names:
- ai
- azure
- devops
author: chandanAggarwal
feed_name: Microsoft Tech Community
primary_section: ai
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/introducing-the-container-network-insights-agent-for-aks-now-in/ba-p/4512197
---

chandanAggarwal announces the public preview of Container Network Insights Agent, an agentic AI assistant for diagnosing AKS networking issues by correlating Kubernetes, Cilium/Hubble, and node-level Linux telemetry, then producing evidence-backed root-cause reports and remediation commands.<!--excerpt_end-->

# Introducing the Container Network Insights Agent for AKS (Public Preview)

We are thrilled to announce public preview of **Container Network Insights Agent - Agentic AI** network troubleshooting for workloads running in **Azure Kubernetes Service (AKS)**.

## The challenge

AKS networking is layered by design:

- Azure CNI
- eBPF
- Cilium
- CoreDNS
- NetworkPolicy
- CiliumNetworkPolicy
- Hubble

Each layer contributes capabilities, and some failures can happen “silently” in ways other layers can’t observe.

When something breaks, operators usually already have tools available:

- **Azure Monitor** for metrics
- **Container Insights** for cluster health
- **Prometheus** and **Grafana** for dashboards
- **Cilium** and **Hubble** for pod network observation
- **kubectl** for direct inspection

The problem is correlation and root cause:

- An alert fires (for example, application performance).
- An on-call engineer checks dashboards, events, and pod health.
- Each tool shows only its own slice.
- The root cause often lives in how signals relate across layers.

So engineers end up manually cross-referencing things like:

- Hubble flows
- NetworkPolicy specs
- DNS state
- Node-level stats and verdicts

This is manual, slow, needs deep domain knowledge, and doesn’t scale—so **MTTR stays high** because the investigation surface is wide and the layers interact in complex ways.

## The solution: Container Network Insights Agent

**Container Network Insights Agent is agentic AI to simplify and speed up AKS network troubleshooting.**

It is designed to **complement** existing observability tools, not replace them. You describe a problem in natural language, and the agent runs a structured investigation across layers. It returns:

- A diagnosis with evidence
- Root cause
- The exact commands you can run to fix it

### Data sources used by the agent

The agent gets visibility through two sources.

#### AKS MCP server

The agent integrates with the **AKS MCP (Model Context Protocol) server**, described as a standardized and secure interface to:

- kubectl
- Cilium
- Hubble

Diagnostic commands run through the same operator tools via a protocol intended to enforce security boundaries—no ad-hoc scripts or custom API integrations.

#### Linux Networking plugin

For visibility below the Kubernetes API layer, the agent collects kernel-level telemetry directly from cluster nodes, including:

- NIC ring buffer stats
- Kernel packet counters
- SoftIRQ distribution
- Socket buffer utilization

This is intended to help pinpoint packet drops and saturation that surface-level metrics might not explain.

### How an investigation works

When you describe a symptom, the agent:

- Classifies the issue and plans an investigation tailored to the symptom pattern
- Gathers evidence through the AKS MCP server and Linux networking plugin across:
  - DNS
  - Service routing
  - Network policies
  - Cilium
  - Node-level statistics
- Reasons across layers to identify how a failure in one component manifests in another
- Delivers a structured report with:
  - Pass/fail evidence
  - Root cause analysis
  - Specific remediation guidance

### Scope and limits

The agent is scoped to **AKS networking**, including:

- DNS failures
- Packet drops
- Connectivity issues
- Policy conflicts
- Cilium dataplane health

It:

- **Does not** modify workloads or change configurations
- Provides **advisory-only** remediation guidance (it tells you what to run; you decide)

## What makes it different

### Deep telemetry (not just surface metrics)

Most observability tools operate at the Kubernetes API level. This agent goes deeper with kernel-level network statistics, BPF program drop counters, and interface-level diagnostics to identify where packets are being lost and why.

### Cross-layer reasoning

Networking incidents rarely have single-layer explanations. The agent correlates evidence across DNS, service routing, network policy, Cilium, and node-level statistics.

Example given:

- Node-level RX drops
- Caused by a Cilium policy denial
- Triggered by a label mismatch after a routine Helm deployment
- Even though pods appear healthy

### Structured and auditable output

- Every conclusion traces to a specific check, its output, and pass/fail status.
- If all checks pass, it reports no issue.
- It “does not invent problems.”
- Investigations are described as deterministic and reproducible.

### Guidance, not just findings

The agent explains evidence, identifies root cause, and provides specific remediation commands.

## Where it fits

This is positioned as **not another monitoring tool**:

- It does not collect continuous metrics.
- It does not replace dashboards.

Instead it adds an “intelligence layer” on top of existing stacks such as:

- Azure Monitor
- Prometheus / Grafana
- Container Insights
- Existing log pipelines

Your alerting detects problems; the agent helps you understand them.

## Safe by design

Designed for production clusters:

- **Read-only access** with minimal RBAC scoped to pods, services, endpoints, nodes, namespaces, network policies, and Cilium resources
- Deploys a temporary debug **DaemonSet** only for packet-drop diagnostics that require host-level stats
- **Advisory remediation only** (never executes changes)
- **Evidence-backed conclusions** (no speculation)
- **Scoped and enforced** (handles AKS networking questions only; prompt injection defenses mentioned)
- **Credentials stay in the cluster** via **managed identity** with **workload identity federation** (no secrets/static credentials); only a session ID cookie reaches the browser

## Get started (Public Preview)

Available in **Public Preview** in:

- Central US
- East US
- East US 2
- UK South
- West US 2

Deployment details:

- Deploys as an **AKS cluster extension**
- Uses **your own Azure OpenAI resource** (for model configuration and data residency control)
- Full capabilities require **Cilium** and **Advanced Container Networking Services**
- DNS and packet drop diagnostics work on all supported AKS clusters

To try it:

- Review the overview on Microsoft Learn:
  - https://learn.microsoft.com/en-us/azure/aks/container-network-insights-agent-overview
- Follow the quickstart to deploy the agent and run your first diagnostic
- Share feedback via Azure feedback channels or the thumbs up/down controls on each response


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-networking-blog/introducing-the-container-network-insights-agent-for-aks-now-in/ba-p/4512197)

