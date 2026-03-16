---
date: 2026-03-16 18:11:49 +00:00
tags:
- ACR
- Active Active
- Azure
- Azure Container Registry
- Azure Resource Health
- Azure Traffic Manager
- Community
- Container Image Pull
- Container Image Push
- Deep Health Checks
- Dependency Checks
- DNS Based Failover
- DNS TTL
- Docker Pull
- Eventual Consistency
- Failover
- Geo Replication
- Health Monitoring
- Health Probes
- Multi Region
- Performance Routing
- Replication Status
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/health-aware-failover-for-azure-container-registry-geo/ba-p/4501730
title: Health-Aware Failover for Azure Container Registry Geo-Replication
feed_name: Microsoft Tech Community
primary_section: azure
section_names:
- azure
author: johshmsft
---

In this post, johshmsft explains how Azure Container Registry geo-replication was made health-aware by wiring ACR Health Monitor’s deep dependency checks into Azure Traffic Manager, so the global endpoint avoids routing to degraded replicas during regional incidents.<!--excerpt_end-->

## Making geo-replicated ACR registries health-aware, not just latency-aware

Azure Container Registry (ACR) supports [geo-replication](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-geo-replication): one registry resource with active-active (primary-primary), write-enabled geo-replicas across multiple Azure regions. You can push or pull through any replica, and ACR asynchronously replicates content and metadata to all other replicas using an eventual consistency model.

For geo-replicated registries, ACR exposes a **global endpoint** like **contoso.azurecr.io**. That URL is backed by **Azure Traffic Manager**, which routes requests to the replica with the best network performance profile (usually the closest region).

The issue: **Traffic Manager routing at the global endpoint was latency-aware, not fully workload-health-aware**. It could see whether the regional front door responded, but not whether that region could successfully serve real pull and push traffic end to end.

This post describes how ACR connected **Health Monitor’s deep dependency checks** to **Traffic Manager** so the global endpoint avoids routing to degraded replicas, improving failover outcomes and reducing customer-facing errors during regional incidents.

## The problem: healthy on the outside, broken on the inside

Traffic Manager uses [performance-based routing](https://learn.microsoft.com/en-us/azure/traffic-manager/traffic-manager-routing-methods#performance-traffic-routing-method): it answers DNS queries with the endpoint expected to have the lowest latency for the caller.

To decide whether an endpoint is viable, Traffic Manager periodically probes a health endpoint. For ACR, that check effectively asked only: **is the reverse proxy responding?**

A container registry operation depends on more than a proxy. A successful `docker pull` touches:

- Storage (layers and manifests)
- Caching infrastructure
- Authentication and authorization services
- Metadata service

Any dependency can fail while the proxy still returns `200 OK` to Traffic Manager probes. During outages (storage degradation, caching failure, auth disruption, VM/datacenter events), Traffic Manager would continue routing customers into the broken region, producing `500` errors on pull and push.

## The manual workaround (and why it can fail)

Customers could manually disable a replica endpoint:

```sh
az acr replication update --name contoso --region eastus --region-endpoint-enabled false
```

But this requires detecting the outage and acting during the incident. In severe cases it may not work reliably because the disable operation routes through the regional resource provider/control plane—potentially the same degraded region.

## How Health Monitor solves this

ACR has an internal **Health Monitor** service in its data plane. Originally it provided node-level health so a load balancer could route within a region, but it didn’t provide a signal for cross-region routing.

ACR extended Health Monitor with a **deep health endpoint** that aggregates the status of multiple critical data-plane dependencies. Instead of “is the proxy up?”, it answers: **“can this region actually serve container registry requests right now?”**

## What gets checked

The deep health endpoint evaluates availability of:

- **Storage**: fundamental for any image operation
- **Caching infrastructure**: impacts push operations and can affect pull latency
- **Container availability**: internal services that process registry API requests
- **Authentication services**: validates permissions to pull or push
- **Metadata service**: monitored when metadata search is enabled

If the region can’t reliably serve requests, the endpoint returns unhealthy. Traffic Manager then degrades that endpoint and routes subsequent DNS queries to the next-lowest-latency healthy replica automatically.

## Per-registry intelligence

A single “region healthy/unhealthy” result is too coarse. Within a region, customer data may be spread across many backing resources (for example, storage accounts). A degradation may affect only some of them.

Health Monitor therefore evaluates health **per registry**:

- When a Traffic Manager probe arrives, Health Monitor determines which backing resources that specific registry uses.
- It evaluates health against those specific dependencies.

Result: if `contoso.azurecr.io` depends on degraded resources while `fabrikam.azurecr.io` does not, only Contoso gets rerouted; Fabrikam stays local and avoids unnecessary latency.

The same idea applies to features like metadata search: registries using the dependency can be marked unhealthy if it’s down, while others remain healthy.

## Tuning for stability (and DNS limitations)

Failing over too eagerly can create needless cross-region latency. Thresholds were tuned so an endpoint becomes unhealthy only after a sustained pattern of failures.

End-to-end failover time is **minutes, not seconds**, due to:

- Health Monitor detection
- Traffic Manager probe cadence (probes every ~30 seconds; multiple failures required)
- DNS TTL propagation

DNS-based failover also means some clients may continue using cached DNS until expiry (Docker daemons, container runtimes, CI/CD systems cache DNS).

## Health Monitor’s own resilience

Health Monitor is designed to **fail-open**: if it can’t evaluate dependencies (crash/restart/can’t reach something to check), it returns healthy. That preserves pre-existing latency-based routing and avoids introducing a new failure mode where the monitor itself causes false failovers.

## How routing changed (customer impact)

This change is transparent: customers still use the same `myregistry.azurecr.io` hostname. What changed is that routing now steers away from degraded replicas instead of relying on latency alone.

### What customers should know

- **Geo-replicated registries**: improvement is automatic; no config changes required.
- **Pull operations**: benefit most.
  - If an image has replicated to the failover region, pulls succeed.
  - If an image was just pushed and hasn’t replicated yet, a pull from the failover region may not find it until replication catches up.
  - Consider retry logic or checking [replication status](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-geo-replication#check-replication-status) before pulling when workflows push then immediately pull elsewhere.
- **Push operations**: more nuanced.
  - If failover/DNS re-resolution happens mid-push, the push can fail and need a retry.
  - Expect higher push latency and more retries during failover; use retries and make publish steps idempotent.
- **Single-region registries**: unaffected (Traffic Manager only used when replicas exist). If the only region is degraded, there’s nowhere to fail over, so behavior remains the same as before.

### Observability

Signals customers can use:

- Increased pull latency from a different region (may indicate rerouting)
- Azure Resource Health: [Resource Health blade](https://learn.microsoft.com/en-us/azure/service-health/resource-health-overview)
- Replica status: [replication health API](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-geo-replication#check-replication-status)

The post notes ongoing work to provide richer routing-change signals and visibility into which region is serving traffic.

## Rollout and safety

Rollout was incremental using ring-based safe deployment practices. Migration involved updating each registry’s Traffic Manager configuration to use the new deep health evaluation.

Safeguards allow reverting a registry/region back to the previous shallow health check if needed.

## Outcome

With Health Monitor-based routing, geo-replicated registries now automatically fail over during incidents that previously caused extended customer impact or required manual endpoint disabling (storage outages, caching failures, VM disruptions, auth service degradation).

To learn more:

- [Geo-replication in Azure Container Registry](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-geo-replication)
- [Enable geo-replication](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-geo-replication#enable-geo-replication)

Updated Mar 16, 2026

Version 1.0

[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/health-aware-failover-for-azure-container-registry-geo/ba-p/4501730)

