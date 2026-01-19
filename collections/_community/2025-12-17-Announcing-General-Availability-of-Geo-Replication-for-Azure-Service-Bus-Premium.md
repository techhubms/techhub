---
external_url: https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/announcing-general-availability-of-geo-replication-for-azure/ba-p/4413164
title: Announcing General Availability of Geo-Replication for Azure Service Bus Premium
author: EldertGrootenboer
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-12-17 17:00:00 +00:00
tags:
- Asynchronous Replication
- Availability Zones
- Azure Messaging
- Azure Regions
- Azure Service Bus
- Business Continuity
- Cloud Architecture
- Disaster Recovery
- Geo Replication
- High Availability
- Messaging Unit
- Metadata Replication
- Namespace Replication
- Premium Tier
- Synchronous Replication
section_names:
- azure
---
Eldert Grootenboer announces the GA release of Geo-Replication for Azure Service Bus Premium, outlining technical details on replication modes, region promotion, and disaster recovery approaches.<!--excerpt_end-->

# Announcing General Availability of Geo-Replication for Azure Service Bus Premium

Azure Service Bus now supports Geo-Replication in its premium tier, offering advanced business continuity and disaster recovery for mission-critical messaging applications.

## Feature Overview

Geo-Replication enables both metadata and data of a Service Bus namespace to be continuously replicated from a primary Azure region to a secondary region. Users can promote the secondary region at any time, minimizing downtime during regional outages.

Refer to the [official disaster recovery documentation](https://learn.microsoft.com/azure/service-bus-messaging/service-bus-outages-disasters) for a comprehensive overview of available options.

## Differentiation With Other Features

- **Geo-Disaster Recovery (Metadata DR)**: Replicates only metadata.
- **Geo-Replication (GA)**: Replicates metadata and message data.
- **Availability Zones**: Support resilience within a single region, while Geo-Replication and Metadata DR protect against cross-region outages.

Geo-Replication and Metadata DR focus on resilience between regions (e.g., East US to West US), while Availability Zones address within-region resiliency.

## Technical Concepts

- **Primary-Secondary Model**: Only the primary region serves clients. The namespace uses a single hostname, which always points to the active primary. If a secondary is promoted, it becomes primary and the previous primary is demoted.
- **Re-initialization**: After failover/promotion, the demoted region becomes secondary, which can be promoted again once re-initialization completes.

## Replication Modes

### Asynchronous Replication

- All requests are acknowledged at the primary, and then replicated asynchronously to the secondary.
- Users can configure the allowable replication lag. If lag exceeds limits, the primary throttles requests.

### Synchronous Replication

- Each request must be acknowledged by both primary and secondary before it is committed.
- Application throughput depends on both regions' availability. If the secondary isn't available, requests aren't acknowledged, and throttling occurs.

## Promotion Scenarios

- **Planned Promotion**: Waits for replication lag to reach zero before initiating the promotion.
- **Forced Promotion**: Immediately initiates promotion, regardless of lag.
- **Customer Control**: Customers manage promotions, ensuring transparent and controlled outage recovery.

## Pricing Overview

- Premium tier is priced per [Messaging Unit (MU)](https://learn.microsoft.com/azure/service-bus-messaging/service-bus-premium-messaging#how-many-messaging-units-are-needed).
- Secondary regions are billed for the same MU count as the primary.
- Additional charges apply for outbound data replicated to secondary regions.

## Further Resources

- Read the full technical documentation on [Geo-Replication for Azure Service Bus](https://learn.microsoft.com/azure/service-bus-messaging/service-bus-geo-replication)
- Review [Azure's disaster recovery best practices](https://learn.microsoft.com/azure/well-architected/reliability/disaster-recovery)

---

_Last updated: December 11, 2025_

Author: Eldert Grootenboer

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/announcing-general-availability-of-geo-replication-for-azure/ba-p/4413164)
