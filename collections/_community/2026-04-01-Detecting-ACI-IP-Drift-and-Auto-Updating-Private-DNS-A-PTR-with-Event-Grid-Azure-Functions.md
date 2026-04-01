---
primary_section: azure
feed_name: Microsoft Tech Community
external_url: https://techcommunity.microsoft.com/t5/azure-architecture/detecting-aci-ip-drift-and-auto-updating-private-dns-a-ptr-with/m-p/4507667#M830
tags:
- A Record
- ACI
- Application Insights
- ARM Resource ID
- At Least Once Delivery
- Azure
- Azure Container Instances
- Azure Event Grid
- Azure Functions
- Azure Private DNS
- Azure SDK
- Community
- DevOps
- DNS Reconciliation
- Event Grid Trigger
- Forward DNS
- Hub And Spoke Network
- Idempotency
- IP Drift
- Log Analytics
- Managed Identity
- Microsoft.Resources.ResourceDeleteSuccess
- Microsoft.Resources.ResourceWriteSuccess
- NXDOMAIN
- Observability
- Private DNS Zone Contributor
- PTR Record
- Python
- RBAC
- Reader Role
- Reverse DNS
- System Assigned Managed Identity
- VNet Linking
date: 2026-04-01 11:35:49 +00:00
section_names:
- azure
- devops
title: Detecting ACI IP Drift and Auto-Updating Private DNS (A + PTR) with Event Grid + Azure Functions
author: Chiragsharma30
---

Chiragsharma30 (with Aditya_AzureNinja) describes an event-driven way to detect Azure Container Instances private IP changes and keep Azure Private DNS A and PTR records correct using Event Grid and Azure Functions, with an emphasis on idempotent, stateless reconciliation and clear failure behavior.<!--excerpt_end-->

## TL;DR

Azure Container Instances (ACI) container groups can be recreated/updated over time and may receive **new private IPs**, which can cause DNS mismatches if forward and reverse records aren’t updated. This post shares an **event-driven pattern** that detects **ACI IP drift** and automatically reconciles **Azure Private DNS A (forward)** and **PTR (reverse)** records using **Event Grid + Azure Functions**.

**Key requirement:** Event Grid delivery is **at-least-once**, so the solution must be **idempotent**.

## Problem statement

In hub-and-spoke environments using **per-spoke Private DNS zones** for isolation, ACI workloads created/updated/deleted over time can receive new private IPs. The goal is to ensure:

- **Forward lookup**: `aci-name.<spoke-zone>` (**A record**) → current ACI private IP
- **Reverse lookup**: `IP` → `aci-name.<spoke-zone>` (**PTR record**)

Constraints driving the design:

- **Azure Private DNS auto-registration is VM-only and does not create PTR records**, so ACI needs explicit A/PTR record management.
- **Reverse DNS is scoped to the VNet** (reverse zone must be linked to the querying VNet, otherwise reverse lookup returns **NXDOMAIN**).

## Design principles (non-negotiable goals)

- **Event-driven**: trigger DNS updates from **resource lifecycle events**, not polling/schedules.
- **Idempotent**: safe handling of duplicates due to **at-least-once** delivery.
- **Stateless**: do not rely on stored state; treat DNS as the baseline.
- **Clear failure modes**: reconciliation failures should be visible and alertable (no silent failures).

## Components

- **Event Grid** subscriptions (filtered to ACI container group lifecycle events)
- **Azure Function App (Python)** with **System Assigned Managed Identity**
- **Private DNS forward zone** (A records)
- **Private DNS reverse zone** (PTR records)
- Supporting infrastructure (typical):
  - Storage account (function artifacts / operational needs)
  - Application Insights + Log Analytics (observability)

## Event-driven flow

1. ACI container group is created/updated/deleted.
2. Event Grid emits a lifecycle event (delivery can be repeated).
3. Function is triggered and reads the **current ACI private IP**.
4. Function reconciles DNS:
   - Upsert A record to current IP
   - Upsert PTR record to FQDN
   - Remove stale PTR(s) for hostname/IP as needed
5. Function logs the reconciliation outcome (updated vs no-op).

## Architecture overview (INFRA)

This follows the **event-driven registration** approach:

- **Event Grid → Azure Function** that reconciles DNS on ACI lifecycle events.

## RBAC at a glance (Managed Identity)

| Role | Scope | Purpose |
| --- | --- | --- |
| Storage Blob Data Owner | Function App deployment storage account | Access function artifacts and operational blobs (required because shared key access is disabled). |
| Reader | Each ACI workload resource group | Read container group state to determine the current private IP. |
| Private DNS Zone Contributor | Private DNS forward zone(s) | Create/update/delete **A records** for ACI hostnames. |
| Private DNS Zone Contributor | Private DNS reverse zone(s) | Create/update/clean up **PTR records** for ACI IPs. |
| Monitoring Metrics Publisher (optional) | Data Collection Rule (DCR) | Upload structured IP-drift events to Log Analytics via ingestion API. |

## Architecture overview (APP)

## Event-Driven DNS Reconciliation for Azure Container Instances

### 1. Event contract: what the function receives

Azure Event Grid delivers events using an Event Grid schema envelope. Each event includes at a minimum:

- `topic`
- `subject`
- `id`
- `eventType`
- `eventTime`
- `data`
- `dataVersion`
- `metadataVersion`

In Azure Functions, the **Event Grid trigger binding** is used to receive these events.

#### Why the `subject` field matters

The subject field typically contains the **ARM resource ID path** of the affected resource. This solution relies on `subject` to:

- verify the event is for an **ACI container group** (`Microsoft.ContainerInstance/containerGroups`)
- extract:
  - subscription ID
  - resource group name
  - container group name

This avoids publisher-specific payload dependencies and keeps parsing deterministic.

### 2. Subscription design: filter hard, process little

The solution uses a strict runbook pattern:

- subscribe only to **ARM lifecycle events**
- filter aggressively so only **ACI container groups** are included
- trigger reconciliation only on meaningful state transitions

Recommended Event Grid event types:

- `Microsoft.Resources.ResourceWriteSuccess` (create / update / stop state changes)
- `Microsoft.Resources.ResourceDeleteSuccess` (container group deletion)
- `Microsoft.Resources.ResourceActionSuccess` (optional: restart/start/stop actions)

### 3. Application design: two functions, one contract

The application is split into:

- authoritative mutation (writer)
- read-only validation (observer)

#### Component A — DNS Reconciler (authoritative writer)

A thin **Python v2 model wrapper** that:

- receives the Event Grid event
- validates it is an ACI container group event
- parses identifiers from the ARM subject
- resolves DNS configuration from a JSON mapping (environment variable)
- delegates DNS mutation to a deterministic worker script

DNS changes are not implemented inline in Python. Instead, the function:

- constructs environment variables
- invokes a worker script (`/bin/bash`) via subprocess
- streams stdout/stderr into function logs
- treats non-zero exit codes as **hard failures**

This keeps the event handler stable and isolates reconciliation logic.

#### Component B — IP Drift Tracker (stateless observer)

A read-only validator that:

- parses identifiers from the event subject
- exits early on delete events
- reads the **live ACI private IP** (Azure SDK)
- reads the **current DNS A record baseline**
- compares live vs DNS state and emits drift telemetry

Core comparison logic:

- no DNS record exists → emit `first_seen`
- DNS record matches live IP → emit `no_change`
- DNS record differs → emit `drift_detected` (old/new IP)

Optionally, drift events can be shipped to **Log Analytics** using DCR-based ingestion.

### 4. DNS Reconciler: execution flow

#### Step 1 — Early filtering

Reject any event whose subject does not contain `Microsoft.ContainerInstance/containerGroups`.

#### Step 2 — ARM subject parsing

Split the subject path and extract:

- resource group
- container group name

#### Step 3 — Zone configuration resolution

Resolve DNS configuration from a **JSON map stored in an environment variable**.

If no matching configuration exists for the resource group:

- log the condition
- exit without error

This supports multi-environment setups via configuration rather than code changes.

#### Step 4 — Delegate to worker logic

Invoke the worker with a deterministic context:

- forward zone name
- reverse zone name(s)
- container group name
- current private IP
- TTL and execution flags

### 5. What “reconciliation” means (idempotent semantics)

#### Create / Update events

- **Upsert A record**
  - if record exists and matches current IP → no-op
  - else → create or overwrite with new IP
- **Upsert PTR record**
  - compute PTR name using IP octets and reverse zone alignment
  - create or overwrite PTR to `hostname.<forward-zone>`

#### Delete events

- delete the A record for the hostname
- scan PTR record sets:
  - remove targets matching the hostname
  - delete the record set if empty

All operations are designed to be safe to repeat.

### 6. Why IP drift tracking is separate

DNS reconciliation fixes state at event time, but drift can still occur due to:

- manual DNS edits
- partial failures
- delete/recreate race conditions
- unexpected redeployments or restarts

Responsibilities stay clear:

- **Reconciler** → fixes state
- **Drift tracker** → observes and reports state

### 7. Observability: correctness vs runtime health

Runtime health (typically covered by ACI/container logs):

- container crashes
- image pull failures
- restarts
- platform events

DNS correctness (the focus here):

- A record ≠ live IP
- missing PTR records
- stale reverse mappings

### 8. Engineering constraints

#### At-least-once delivery → idempotency

Event Grid delivery must be treated as **at-least-once**; reconciliation actions must tolerate duplicates.

#### Explicit failure behavior

If the worker returns a non-zero exit code:

- the function invocation fails
- the failure is visible and alertable
- incorrect DNS does not silently persist


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-architecture/detecting-aci-ip-drift-and-auto-updating-private-dns-a-ptr-with/m-p/4507667#M830)

