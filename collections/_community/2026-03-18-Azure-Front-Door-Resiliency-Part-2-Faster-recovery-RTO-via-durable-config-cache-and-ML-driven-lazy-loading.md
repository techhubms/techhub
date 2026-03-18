---
author: AbhishekTiwari
section_names:
- azure
- ml
date: 2026-03-18 00:20:54 +00:00
tags:
- Azure
- Azure Front Door
- Azure Networking
- Canary Regions
- Community
- Configuration Cache
- Configuration Propagation
- Control Plane
- Data Plane
- Edge Computing
- EUAP
- Fault Injection
- FlatBuffers
- GameDay Testing
- Hostmap
- Incident Retrospective
- Last Known Good (lkg)
- Lazy Loading
- Machine Learning Pipeline
- Memory Mapped Files
- ML
- Recovery Time Objective (rto)
- Resiliency
- Tenant Isolation
- TLS Certificates
- Traffic Analysis
- Two Phase Commit
- Warm Tenants
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/azure-front-door-resiliency-series-part-2-faster-recovery-rto/ba-p/4503091
title: 'Azure Front Door Resiliency (Part 2): Faster recovery (RTO) via durable config cache and ML-driven lazy loading'
primary_section: ml
feed_name: Microsoft Tech Community
---

AbhishekTiwari (with Azure Networking leaders) explains how Azure Front Door improved recovery time objectives by hardening its local configuration cache, avoiding fleet-wide rebuilds, and introducing ML-driven lazy loading so recovery scales with active traffic rather than total tenants.<!--excerpt_end-->

# Azure Front Door: Resiliency Series – Part 2: Faster recovery (RTO)

In [Part 1](https://aka.ms/AzureFrontDoor/Resiliency-Part1), this series laid out four resiliency pillars for Azure Front Door:

- Configuration resiliency
- Data plane resiliency
- Tenant isolation
- Accelerated Recovery Time Objective (RTO)

Part 1 focused on preventing incompatible configuration from escaping pre-production and keeping the data plane serving traffic using last-known-good (LKG) configuration. It also introduced **“Food Taster”**, a sacrificial process on each edge server that tests configuration changes in isolation before they reach the live data plane.

This part focuses on **recovery**: making Azure Front Door return to full operation in a **predictable, bounded timeframe** at global edge scale (210+ edge sites, hundreds of thousands of tenants). The stated recovery targets are:

- Recover any edge site (or all edge sites) within **~10 minutes** in worst-case scenarios
- Typical worker-process crash recovery in **under a second**

## Repair status after the October 2025 incidents

The post references two Azure Front Door incidents (October 2025) and links to Azure Incident Retrospective recordings:

- [October 9 incident](https://aka.ms/AIR/QNBQ-5W8)
- [October 29 incident](https://aka.ms/AIR/YKYN-BWZ)

Reported progress includes:

- Configuration propagation + data plane resiliency work is **completed and deployed**
- Configuration propagation latency reduced from **~45 minutes to ~20 minutes**
- Further reduction anticipated to **~15 minutes by end of April 2026**

Planned timeline callouts:

- Recovery improvements: accelerate recovery time to **< 10 minutes** (targeted for **April 2026**)
- Tenant isolation: micro-cellular architecture with ingress-layered shards (**June 2026**)

## Why recovery at edge scale is deceptively hard

Azure Front Door’s edge data plane model (simplified):

- Each edge site runs multiple servers.
- Each server’s data plane includes:
  - A **master process** coordinating lifecycle
  - Multiple **worker processes** serving traffic
  - A **configuration translator process** converting control-plane configuration bundles into optimized binary **FlatBuffer** files
- Each server keeps a **local cache** of translated artifacts to speed restart.

### How configuration is served

- Workers **memory-map** FlatBuffer files for **zero-copy** access.
- Updates use a **two-phase commit** model:
  1. Load and validate new FlatBuffers in a staging area
  2. Atomically swap them into production maps
- In-flight requests continue using the previous config until the last request completes.

### Failure modes and their recovery impact

- **Worker crash**: typical recovery **< 1 second**; no data plane impact because other workers continue.
- **Master crash**: automatic recovery using local cache.
  - With cache reuse: recovery described as approximately **60 minutes**.
  - If cache is unavailable/invalidated (e.g., corruption): recovery time increases significantly.

During the **October 29 incident**, a data plane crash caused a recovery sequence taking **~4.5 hours** because a defect invalidated the local cache—turning “restart” into “rebuild everything”:

- Translator had to re-fetch + re-translate hundreds of thousands of tenant configs
- Workers had to wait for the translator to finish before serving

Key learnings:

- **Expensive rework**: some crashes discarded validated FlatBuffer artifacts
- **High restart costs**: workers blocked until full translation completed
- **Unbounded recovery time**: time scaled with total tenant footprint, not active traffic

## Persisting validated configurations across restarts

A major change was to treat cached, previously validated tenant configurations as durable across failures.

### Design goals

- **Crashes should not invalidate cache by default**
  - Worker crash, master crash, data plane restart, coordinated recovery should still allow reuse unless there is a proven reason to discard.
- **Bad tenant config must not poison the cache**
  - Use **tenant-scoped eviction** rather than invalidating everything.

### Platform enhancements

Previously, some failure paths treated cache as unsafe and **invalidated it entirely**, causing a full reload/reprocess across tenants.

The new model:

- Cache entries are not invalidated based on crash *type*.
- Eviction is **granular and tenant-scoped**:
  - If a cached tenant configuration fails validation/load checks, discard and reload *only that tenant*.

### Safety and correctness controls

- **Per-tenant validation on load** before promotion
- **Targeted re-translation** only for affected tenants
- **Operational escape hatch**: authorized operators can force a clean rebuild when needed

Result: configuration defects are intended to be contained locally and recovery prefers continued service using last-known-good over aggressive invalidation.

## Making recovery scale with active traffic, not total tenants

Even with a warm cache, startup time was dominated by **eagerly loading** tenant configurations into memory:

- Memory-mapping + parsing FlatBuffers
- Building internal lookup maps
- Adding TLS certs and config blocks per tenant

At Azure Front Door scale, this added **almost an hour**—even when many tenants had no traffic.

### ML-optimized lazy loading

Azure Front Door moved to an ML-assisted approach:

- Only load a small subset of historically active tenants per site (“**warm tenants**”).
- Warm tenants list per edge site is created via a **traffic analysis pipeline leveraging ML**.

To still route requests correctly:

- Each worker builds a **hostmap** during startup.
  - For warm tenants: fully load config.
  - For others: record domain names mapped to configuration path location.
- When a request arrives for a non-loaded tenant:
  - Load and validate that tenant’s configuration **on demand**.
  - Begin serving immediately.

Impact:

- Recovery time scales with **active tenants**, not total onboarded tenants.
- Adds a practical failure isolation boundary: many edge sites will never load an inactive tenant’s faulty configuration; if an incompatible config is encountered, it is contained to a single worker.

The post states these changes are planned to complete in **April 2026**, improving RTO from **~1 hour** to **under 10 minutes** for worst-case recovery.

## Continuous validation through Game Days

The post emphasizes recurring **GameDay fault-injection testing** since late 2025:

- Food Taster crash scenarios (example: January 2026 drill halted update in ~5 seconds, no customer impact)
- Master process crash scenarios (workers keep serving traffic; Local Config Shield engagement mentioned within 10 seconds)
- Multi-region failure drills (validate global Config Shield)
- Fallback drills for critical Azure services behind Azure Front Door (example: February 2026 simulated Front Door unavailability, validated failover with no impact)

## Closing

The core recovery approach is:

- Don’t invalidate the FlatBuffer cache by default
- Evict/repair only at tenant scope
- Load active tenants first using ML-driven traffic analysis
- Use tested coordinated recovery tooling and regular fault injection

The next post in the series will cover the **tenant isolation** pillar.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-networking-blog/azure-front-door-resiliency-series-part-2-faster-recovery-rto/ba-p/4503091)

