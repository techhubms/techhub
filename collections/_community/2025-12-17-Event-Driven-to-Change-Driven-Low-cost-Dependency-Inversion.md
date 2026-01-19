---
layout: post
title: 'Event-Driven to Change-Driven: Low-cost Dependency Inversion'
author: CollinBrian
canonical_url: https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/event-driven-to-change-driven-low-cost-dependency-inversion/ba-p/4478948
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-12-17 22:22:37 +00:00
permalink: /azure/community/Event-Driven-to-Change-Driven-Low-cost-Dependency-Inversion
tags:
- Anti Corruption Layer
- Azure SQL
- Change Driven Architecture
- Continuous Queries
- Cosmos DB
- CQRS
- Database Integration
- Declarative Queries
- Dependency Inversion
- Drasi
- Event Driven Architecture
- Event Sourcing
- Microservices
- Reactive Systems
- Real Time Data
section_names:
- azure
---
Daniel Gerlag offers insights into architecting reactive systems by contrasting event-driven patterns with Drasi's change-driven approach, highlighting how continuous queries deliver efficient dependency inversion and real-time updates, with a focus on Azure and Cosmos DB integration.<!--excerpt_end-->

# Event-Driven to Change-Driven: Low-cost Dependency Inversion

**Author: Daniel Gerlag, Principal Software Engineer**

Event-driven architectures promise scalability and decoupled systems but often introduce complexity and boilerplate in real-world implementations. This article contrasts event-driven and change-driven architectural patterns, emphasizing the issues teams face while implementing and maintaining event-driven solutions, such as maintaining event stores, handling outbox patterns, and struggling with CRUD-based event models.

## Breaking Down Event Patterns

- **Event Notification**: Minimal event info, requiring follow-up queries.
- **Event Carried State Transfer**: Includes full data changes, helping downstream services maintain projections.
- **Event Sourcing**: The event log *is* the system of record; state is the result of a sequence of events.
- **CQRS**: Separates write and read models for optimized scalability and maintainability. Pairs well with Domain-Driven Design (DDD).

While these models offer theoretical elegance, their practical implementations—especially event sourcing—are fraught with challenges:

- Required boilerplate (commands, handlers, projections, etc.)
- Domain events devolving into simple database change notifications
- Coupling of services via unclear event taxonomies
- Brittle anti-corruption layers that are frequently nothing more than pipelines for internal database changes

## Drasi and Change-Driven Architecture

Drasi's approach eliminates many pain points by:

- **Tailing database changelogs** rather than relying on injected event publishers
- Providing **continuous queries**—live, reactive projections over one or more data sources such as PostgreSQL, MySQL, Azure SQL, and Cosmos DB
- Prioritizing **declarative logic** over imperative event handlers
- Facilitating semantic, business-level mapping from low-level data changes
- Handling temporal features and complex aggregations across sources with no manual code

### Practical Example: Curbside Pickup System

Rather than building event flows and projection infrastructure, Drasi enables:

- **Cross-source aggregation** (e.g., PostgreSQL + MySQL + Cosmos DB)
- Real-time dashboards and detection queries (e.g., detecting when drivers wait >10 minutes)
- No need for custom aggregation logic, event ordering, replay handling, or state management
- Declaratively defined system behaviors using flexible continuous queries

### Migration Benefits

With Drasi:

- Existing services and databases remain unchanged
- Migration from poll-based or batch systems is incremental—start with one query on one table
- Gains real-time, reactive behavior without architectural overhauls
- Significantly reduced boilerplate, code complexity, and operational risk

## Key Takeaways

- Many teams over-apply complex event-driven architectures, incurring high maintenance and coupling.
- Drasi's change-driven model offers practical, low-friction alternatives for real-time requirements—especially suited for Azure SQL, Cosmos DB, and similar platforms.
- Focus on architecture your team can understand, maintain, and evolve.

For more, see the [official Drasi documentation](https://drasi.io/) and their [curbside pickup tutorial](https://drasi.io/tutorials/curbside-pickup/).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/event-driven-to-change-driven-low-cost-dependency-inversion/ba-p/4478948)
