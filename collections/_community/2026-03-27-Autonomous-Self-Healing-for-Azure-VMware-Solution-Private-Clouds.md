---
section_names:
- azure
- devops
tags:
- Audit Trail
- Autonomous Remediation
- AVS
- Azure
- Azure Elastic SAN
- Azure ExpressRoute
- Azure NetApp Files
- Azure Route Server
- Azure Virtual Network
- Azure Virtual WAN
- Azure VMware Solution
- Blast Radius
- Causal Correlation
- Checkpointing
- Closed Loop Control
- Community
- Control Plane
- DevOps
- Disaster Recovery
- Dwell Window
- ESXi
- Fault Propagation
- Fencing Token
- Governance
- Idempotent Playbooks
- Incident Ledger
- Incident Response
- Mutual Exclusion
- NSX Manager
- Policy Gates
- Runtime Dependency Graph
- Self Healing
- Tier 0 Routing
- Vcenter Server
- Verification
- VMware HCX
- VMware NSX
- VMware Site Recovery
- Vsan
date: 2026-03-27 23:51:43 +00:00
author: RohanB
primary_section: azure
external_url: https://techcommunity.microsoft.com/t5/azure-migration-and/autonomous-self-healing-for-azure-vmware-solution-private-clouds/ba-p/4506374
feed_name: Microsoft Tech Community
title: Autonomous Self-Healing for Azure VMware Solution Private Clouds
---

RohanB describes how Azure VMware Solution’s Autonomous Self-Healing system reduces control-plane outage time by correlating NSX/vCenter signals using a live dependency graph, gating remediation actions via policy checks, and verifying recovery before closing incidents.<!--excerpt_end-->

# Autonomous Self-Healing for Azure VMware Solution Private Clouds

## Overview

Azure VMware Solution (AVS) operates a global fleet of production private clouds running a full VMware NSX and vCenter Server control plane.

A representative failure mode is an NSX Manager cluster losing quorum. In that situation, NSX can surface multiple related alarms, but the actual impact is more specific than “everything failed at once”:

- Management and control-plane updates stop
- Cluster health may degrade
- Some Edge or transport-node symptoms can follow
- Existing Tier-0 dynamic routing generally remains operational

Multiple symptoms can share a single upstream fault, so operators need to verify signals against:

- Cluster status
- Service health
- Storage
- Compute Manager state
- Transport-node connectivity

Without a model that encodes *directional dependency relationships* across these layers, the alarm set looks the same as multiple independent failures. Treating each alarm as independent causes repeated investigation of the same propagation path and can extend the outage.

At production scale, fault propagation outpaces manual triage. AVS “Private Cloud Autonomous Self-Healing” is a closed-loop control architecture intended to address this:

- Correlates control-plane signals causally using a live runtime dependency graph
- Enforces a policy gate stack before automated actions
- Acquires scoped mutual exclusion before execution
- Independently verifies recovery before closing an incident

This article describes the architecture and the key design decisions.

## Architectural components (AVS context)

[Azure VMware Solution](https://azure.microsoft.com/services/azure-vmware/) is a first-party Azure service that provides private clouds containing VMware vSphere clusters built from dedicated bare-metal Azure infrastructure.

Key components described:

- **Azure Subscription**: Access control plus budget/quota management
- **Azure Region**: Physical locations grouped into Availability Zones and regions
- **Azure Resource Group**: Logical container for AVS resources
- **AVS Private Cloud**: VMware vCenter Server, NSX networking, vSAN storage, and Azure bare-metal ESXi hosts
  - Supported storage options noted: **Azure NetApp Files**, **Azure Elastic SAN**, **Pure Cloud Block Store**
- **AVS Resource Cluster**: Scale-out cluster capacity for the private cloud (same VMware stack + Azure bare-metal ESXi)
- **VMware HCX**: Mobility, migration, and network extension
- **VMware Site Recovery**: DR automation and replication via vSphere Replication
  - Third-party DR solutions mentioned: **Zerto DR**, **JetStream DR**
- **Dedicated Microsoft Enterprise Edge (D-MSEE)**: Connectivity between Azure and the AVS private cloud
- **Azure Virtual Network (VNet)**: Private network for connecting Azure resources
- **Azure Route Server**: Dynamic route exchange with Azure networks
- **Azure Virtual Network Gateway**: IPSec VPN, ExpressRoute, and VNet-to-VNet connectivity
- **Azure ExpressRoute**: High-speed private connectivity to on-prem/colo
- **Azure Virtual WAN (vWAN)**: Aggregated networking/security/routing in a unified WAN

## What Autonomous Self-Healing delivers

The system introduces five *system-guaranteed correctness properties* to the AVS control-plane incident response path:

- **Bounded, verifiable recovery time**
  - Measures time from first correlated signal to verified stable recovery
  - Prior state: incidents could be closed when actions finished, not when recovery was verified
- **Signal integrity at ingestion**
  - Normalizes events, deduplicates sources, suppresses flapping before correlation
  - Prior state: raw alarm streams, with cause established manually via pattern recognition
- **Policy-gated execution**
  - Atomically checks freeze windows, risk budgets, blast radius, rate limits, approvals before execution
  - Prior state: no single atomic gate stack consistently enforced limits/approvals
- **Append-only incident evidence**
  - Stores signals, topology, decisions, workflow trace, verification in a structured record
  - Prior state: evidence spread across separate logs, difficult to replay
- **Progressive trust model**
  - Supports notify-only mode so operators can inspect detections/proposed actions before enablement
  - Prior state: automation was binary (on/off)

## Design principles

Seven design elements introduced for AVS private cloud control-plane operations:

- Three-plane separation: **detection**, **decisioning**, **execution**
- Live runtime dependency graph continuously updated from NSX and vCenter event streams (instead of static rules)
- Three-input causal correlation model:
  - Evidence strength
  - Temporal ordering
  - Dependency directionality
- Pre-execution blast-radius computation used as a gate input
- Phase boundary model (stabilization, execution, verification) adding hysteresis to damp oscillation
- Execution contract structure:
  - Trigger
  - Gate declaration
  - Step specification
  - Verification contract
- Unified append-only ledger for both automated and human-led resolution paths (governance requirement)

For in-scope failures, the intent is bounded, auditable recovery time without operator involvement. When remediation can’t be authorized, the system produces a deterministic evidence bundle for handoff.

## Architecture: detection, decisioning, execution

The system separates the control loop into planes with single, testable contracts to avoid shared failure surfaces (for example, execution bugs corrupting evidence, alarm spikes starving gate evaluation, or misconfigured gates blocking normalization).

### Detection plane

Transforms raw NSX and vCenter alarm streams into stable incident candidates:

- Normalizes event formats across sources
- Collapses redundant signals
- Applies a dwell window to filter transient state changes

Only confirmed, stable units cross the boundary into correlation.

### Decisioning plane

Runs causal correlation against the live dependency graph before gate evaluation:

- Produces a ranked root-cause hypothesis with confidence scores
- Computes a blast-radius estimate

Outputs exactly one of:

- Gated authorization to execute, or
- Escalation with a complete evidence bundle

### Execution plane

- Acquires a fencing token scoped to the smallest viable failure domain
- Runs a versioned, idempotent, checkpointed playbook
- Closes the incident only after independent post-condition verification confirms stable recovery across a stability dwell window

Every state transition appends to the incident ledger.

## Incident ledger

For every incident (automated or human-led), the system produces a structured, append-only ledger capturing in sequence:

1. Raw and normalized signals with suppression outcomes
2. Topology snapshot at detection time
3. Full decision record:
   - Correlation results
   - Root-cause ranking
   - Blast-radius estimate
   - Gate evaluation trace
4. Workflow trace with step metadata and lease identifiers
5. Verification outcomes with post-conditions and stability dwell disposition

The record is intended to be deterministic to reconstruct: given the same ledger, two reviewers should reconstruct the same incident timeline.

## Summary and scope limits

Autonomous Self-Healing targets a defined subset of NSX and vCenter *control-plane* failures within an AVS private cloud.

Out of scope (explicitly not handled):

- Data-plane failures
- Storage faults
- Hypervisor crashes
- Hardware failures
- Control-plane failures outside its modeled dependency graph

It also does not:

- Run arbitrary scripts
- Bypass RBAC controls
- Override tenant isolation boundaries

When it can’t act, it provides a complete, structured evidence bundle for operator response.

## References

- [Azure VMware Solution — Product Homepage](https://azure.microsoft.com/en-us/products/azure-vmware/)
- [Azure VMware Solution Documentation](https://learn.microsoft.com/en-us/azure/azure-vmware/)
- [Run VMware Workloads on Azure VMware Solution (Training Path)](https://learn.microsoft.com/en-us/training/paths/run-vmware-workloads-azure-vmware-solution/)
- [Az.VMware PowerShell Module](https://learn.microsoft.com/en-us/powershell/module/az.vmware/)
- [Cloud Adoption Framework — Azure VMware Solution Scenario](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/scenarios/azure-vmware/)
- [Enterprise-Scale Network Topology for Azure VMware Solution](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/scenarios/azure-vmware/eslz-network-topology-connectivity)
- [Enterprise-Scale Landing Zone for Azure VMware Solution](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/scenarios/azure-vmware/enterprise-scale-landing-zone)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-migration-and/autonomous-self-healing-for-azure-vmware-solution-private-clouds/ba-p/4506374)

