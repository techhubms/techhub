---
tags:
- Action Groups
- AI
- ARM
- Autonomous Remediation
- Azure
- Azure CLI
- Azure CNI Overlay
- Azure Kubernetes Service (aks)
- Azure Monitor
- Azure Resource Graph
- Azure SRE Agent
- Cilium
- Community
- CPU Throttling
- CrashLoopBackOff
- DevOps
- Exit Code 137
- GitHub Copilot Agent
- GitHub Issues
- Incident Response
- Kubectl
- Kubernetes
- Kusto
- Log Analytics
- Managed Identity (uami)
- Managed Prometheus
- Microsoft Teams
- Node Auto Provisioning (nap)
- OOMKilled
- PromQL
- Pull Requests
- RBAC
- Review Mode
- Runbooks
- Sev1
- SRE
- Startup Probe
title: 'Autonomous AKS Incident Response with Azure SRE Agent: From Alert to Verified Recovery in Minutes'
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/autonomous-aks-incident-response-with-azure-sre-agent-from-alert/ba-p/4511343
feed_name: Microsoft Tech Community
author: hailukassa
section_names:
- ai
- azure
- devops
primary_section: ai
date: 2026-04-17 20:31:11 +00:00
---

In this community post, hailukassa walks through using Azure SRE Agent to investigate and remediate two real AKS failure scenarios (CPU starvation and OOMKilled), including Azure Monitor-triggered automation, verification steps, and GitHub/Teams follow-up for durable fixes.<!--excerpt_end-->

## Overview

When a Sev1 alert fires on an Azure Kubernetes Service (AKS) cluster, detection is usually not the hardest part. The harder part is proving what broke, why it broke, and fixing it safely under time pressure.

This post demonstrates how **Azure SRE Agent** can run a closed-loop incident workflow:

- Receive an incident (for example from **Azure Monitor**)
- Investigate using Azure-native telemetry and AKS diagnostics
- Apply targeted remediation
- Verify recovery
- Create engineering follow-up (GitHub issue and optionally a PR)
- Keep the team informed in Microsoft Teams

## Core concepts

Azure SRE Agent is positioned here as a *governed incident-response system* (not a free-form chatbot with infrastructure access). The workflow is built around:

1. **Incident platform**: where incidents originate (in this demo: **Azure Monitor**)
2. **Built-in Azure capabilities**: Azure Monitor, Log Analytics, Azure Resource Graph, Azure CLI/ARM, AKS diagnostics
3. **Connectors**: extend to GitHub, Teams, Kusto, MCP servers
4. **Permission levels**: reader for investigation, privileged for operational changes
5. **Run modes**:
   - **Review** (approval-gated execution)
   - **Autonomous** (direct execution)

Key point from the author: **production safety is driven more by RBAC and run mode than “prompt quality.”**

Recommended rollout path:

```text
Start: Reader + Review
Then: Privileged + Review
Finally: Privileged + Autonomous (only for narrow, trusted incident paths)
```

## Demo environment

Reproducible scripts/manifests are provided.

- Demo repo: https://github.com/hailugebru/azure-sre-agents-aks

Environment components:

- AKS cluster
- Node auto-provisioning (NAP)
- Azure CNI Overlay powered by Cilium
- Managed Prometheus metrics
- AKS Store sample microservices app
- Azure SRE Agent configured for incident-triggered investigation and remediation

High-level flow:

```text
Azure Monitor → Action Group → Azure SRE Agent → AKS Cluster
(Alert)        (Webhook)      (Investigate/Fix) (Recover)
   ↓
Teams notification + GitHub issue → GitHub Agent → PR for review
```

## How the agent was configured

Configuration focuses on:

- **Scope**: scoped to the demo resource group
- **Identity**: user-assigned managed identity (UAMI)
- **Permissions**: RBAC controls what actions the agent can take
- **Incident intake**: Azure Monitor as the incident platform
- **Response mode**: set to **Autonomous** for a narrow, trusted incident path

Teams and GitHub connectors are used for:

- Teams: milestone updates during the incident
- GitHub: durable follow-up (issue and/or PR)

> **A note on context** (author’s point): more environment/runbook context improves investigation quality and reduces exploratory time.

## Two incidents, two response modes

Two operating modes are illustrated:

1. **Alert-triggered automation**: agent acts when Azure Monitor fires
2. **Ad hoc chat investigation**: engineer prompts the agent based on an observed symptom

## Incident 1: CPU starvation (alert-driven, ~8 min MTTR)

A deployment had CPU/memory settings too low to start successfully:

```text
resources:
  requests:
    cpu: 1m
    memory: 6Mi
  limits:
    cpu: 5m
    memory: 20Mi
```

Within ~5 minutes, Azure Monitor fired a `pod-not-healthy` Sev1 alert.

Agent’s diagnostic conclusion (from pod state, probes, exit code):

> "Exit code 1 (not 137) rules out OOMKill. The pod failed at startup, not at runtime memory pressure. CPU limit of 5m is insufficient for the process to bind its port before the startup probe times out. This is a configuration error, not a resource exhaustion scenario."

Actions taken:

- Found additional CPU-throttled pods (112–200% of configured limit) using `kubectl top`
- Patched four workloads:
  - `makeline-service`
  - `virtual-customer`
  - `virtual-worker`
  - `mongodb`
- Verified recovery:
  - all affected pods healthy
  - 0 restarts cluster-wide

Outcome:

- Full recovery in ~8 minutes
- 0 human interventions

## Incident 2: OOMKilled (chat-driven, ~4 min MTTR)

A deliberately undersized `order-service` was deployed:

```bash
kubectl apply -f .\manifests\aks-store\order-service-changed.yaml -n pets
```

The author demonstrates starting from chat before the pod-phase alert fires. They note:

- `CrashLoopBackOff` is a *container waiting reason*, not a pod phase
- In production, use Prometheus-based signals rather than pod phase alone

PromQL example used in Azure Monitor:

```promql
sum by (namespace, pod) (
  (
    max_over_time(
      kube_pod_container_status_waiting_reason{ namespace="pets", reason="CrashLoopBackOff" }[5m]
    ) == 1
  )
  and on (namespace, pod, container)
  (
    increase(kube_pod_container_status_restarts_total{ namespace="pets" }[15m]) > 0
  )
) > 0
```

Chat prompt used:

> The order-service pod in the pets namespace is not healthy. Please investigate, identify the root cause, and fix it.

Agent’s reasoning:

> "Container logs are empty. The process was killed before it could write its first log line. Exit code 137 confirms OOMKill. No NODE_OPTIONS in the ConfigMap rules out a V8 heap misconfiguration. The 20Mi limit is 12.8x below the pod's observed 50Mi runtime baseline. This limit was never viable for this workload."

Remediation and verification:

- Increased memory:
  - limit: 20Mi → 128Mi
  - request: 10Mi → 50Mi
- Verified stabilized pod:
  - 74Mi/128Mi (58% utilization)
  - 0 restarts

Outcome:

- Service recovered in ~4 minutes
- No manual cluster interaction

## Side-by-side comparison

| Dimension | Incident 1: CPU starvation | Incident 2: OOMKilled |
| --- | --- | --- |
| Trigger | Azure Monitor alert (automated) | Engineer chat prompt (ad hoc) |
| Failure mode | CPU too low for startup probe to pass | Memory limit too low for process to start |
| Key signal | Exit code 1, probe timeout | Exit code 137, empty container logs |
| Blast radius | 4 workloads affected cluster-wide | 1 workload in target namespace |
| Remediation | CPU request/limit patches across 4 deployments | Memory request/limit patch on 1 deployment |
| MTTR | ~8 min | ~4 min |
| Human interventions | 0 | 0 |

## Why this matters

Most AKS environments already have signals (Azure Monitor + managed Prometheus). The manual gap is response: triage dashboards, run ad-hoc `kubectl` commands, and apply hotfixes under pressure.

Azure SRE Agent’s value (as framed here): a repeatable loop for investigation → remediation → verification, plus follow-up while the team is offline.

Measured results in this lab run (Apr 10, 2026):

| Metric | Result |
| --- | --- |
| Alert to recovery | ~4 to 8 min |
| Human interventions | 0 |
| Scope of investigation | Cluster-wide, automated |
| Correlate evidence and diagnose | ~2 min |
| Apply fix and verify | ~4 min |
| Post-incident follow-up | GitHub issue + draft PR |

## Teams + GitHub follow-up

After Incident 1:

- Azure SRE Agent used the **GitHub connector** to file an issue with summary, root cause, and runtime changes.
- The issue was assigned to a **GitHub Copilot agent**, which opened a draft PR to align source manifests with the runtime hotfix.
- The workflow can also be configured to submit the PR directly.

Teams connector posted milestone updates:

- Investigation started
- Root cause and remediation identified
- Incident resolved

## Key takeaways

1. Treat Azure SRE Agent as a governed incident-response system; production controls come from **permission levels** and **run modes**.
2. Anchor detection in your incident platform (Azure Monitor/Prometheus), and use connectors to extend outward:
   - Teams: real-time coordination
   - GitHub: durable engineering follow-up
3. Start small (single resource group, one incident type, **Review** mode), validate telemetry and RBAC, then expand toward **Autonomous**.

## Next steps suggested by the author

- Add Prometheus alert coverage for CrashLoopBackOff, ImagePullBackOff, and node resource pressure
- Expand to multi-cluster managed scopes once validated
- Test multi-service cascading failures to understand when human escalation is still needed

## Resources

- Demo repository: https://github.com/hailugebru/azure-sre-agents-aks
- Azure SRE Agent docs: https://learn.microsoft.com/azure/sre-agent/
- AKS Store Demo: https://github.com/Azure-Samples/aks-store-demo
- Node Auto-Provisioning: https://learn.microsoft.com/en-us/azure/aks/node-auto-provisioning


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/autonomous-aks-incident-response-with-azure-sre-agent-from-alert/ba-p/4511343)

