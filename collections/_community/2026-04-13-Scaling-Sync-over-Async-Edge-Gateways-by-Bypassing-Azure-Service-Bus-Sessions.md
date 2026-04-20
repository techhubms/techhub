---
tags:
- 504 Gateway Timeout
- API Gateway
- Asynchronous Workers
- Azure
- Azure Service Bus
- Community
- CompletableFuture
- CorrelationId
- Dynamic Subscriptions
- Edge Gateways
- Horizontal Scaling
- Java
- Message Routing
- Request Reply Messaging
- Service Bus Sessions
- Session Limits
- Spring Boot
- SQL Filters
- Standard Tier
- Stateless Architecture
- Sync Over Async
- Topics And Subscriptions
external_url: https://techcommunity.microsoft.com/t5/azure-architecture/architecture-pattern-scaling-sync-over-async-edge-gateways-by/m-p/4510919#M832
author: ssaluja72
section_names:
- azure
primary_section: azure
title: Scaling Sync-over-Async Edge Gateways by Bypassing Azure Service Bus Sessions
date: 2026-04-13 17:25:35 +00:00
feed_name: Microsoft Tech Community
---

ssaluja72 shares a Sync-over-Async architecture pattern for edge API gateways on Azure Service Bus, replacing session-based request/reply correlation with stateless topic subscriptions filtered by CorrelationId to avoid session lock bottlenecks and improve horizontal scalability.<!--excerpt_end-->

# Scaling Sync-over-Async Edge Gateways by Bypassing Azure Service Bus Sessions

This post describes an architecture pattern for handling **legacy synchronous HTTP clients** that need to trigger **long-running asynchronous AI worker jobs** (for example, responses taking **45+ seconds**) without hitting typical HTTP timeout failure modes.

## The problem: stateful bottlenecks at the edge

Long-running AI generation tasks can exceed typical API gateway / load balancer timeouts, leading to **504 Gateway Timeouts** when using plain REST request/response.

A common mitigation is a **Sync-over-Async** pattern:

- The **Gateway** accepts an incoming HTTP request.
- The Gateway publishes a message to **Azure Service Bus**.
- A **worker** processes the message.
- The worker publishes a reply.
- The Gateway correlates the reply to the original request and maps it back to the still-open HTTP connection.

### Why sessions become a scaling limit

The "default" request/reply correlation approach often uses **Service Bus Sessions**. The post calls out scaling issues with that approach:

1. **Stateful gateways**
   - A gateway pod must acquire an **exclusive lock** on a session.
   - That pod becomes tightly coupled to the request.

2. **Horizontal elasticity breaks**
   - When the reply arrives, it must be received by the **specific pod** that holds the session lock.
   - Other pods (even if idle) cannot help.

3. **Hard platform limits**
   - Traffic spikes can exhaust **namespace concurrent session limits**, especially on the **Standard tier**.

## The solution: stateless filtered topics (broker-side routing)

To keep the gateway layer **100% stateless**, the pattern avoids sessions and moves routing into the broker using a **Filtered Topic Pattern**.

## How it works

1. The Gateway injects a custom message property such as:
   - `CorrelationId = Instance-A-Req-1`

2. Instead of locking a session, the Gateway creates a **lightweight, dynamic subscription** on a shared **Reply Topic** using a **SQL Filter**, for example:
   - `CorrelationId = 'Instance-A-Req-1'`

3. The AI worker processes the request and publishes the reply message to the same shared reply topic, preserving the same property:
   - `CorrelationId = Instance-A-Req-1`

4. **Azure Service Bus** evaluates the SQL filter and routes the message into the correct subscription, so the reply is delivered to the correct gateway instance.

### Claimed benefits

- **No session locks**
- **No implicit instance affinity** between reply and a specific session-lock holder
- **Complete horizontal scalability** (gateway pods can scale without session coupling)
- If a pod crashes, its **temporary subscription drops**, which the author notes helps avoid "locked poison messages" caused by session affinity.

## Open-source implementation

The author notes that implementing:

- dynamic Service Bus administration clients, and
- receiver lifecycle management

can be complex.

They provide a **Spring Boot starter** that abstracts the pattern so developers can implement scalable Sync-over-Async flows with a single line of code returning a `CompletableFuture`.

- GitHub repository: https://github.com/ShivamSaluja/sentinel-servicebus-starter
- Full technical write-up: https://dev.to/shivamsaluja/sync-over-async-bypassing-azure-service-bus-session-limits-for-ai-workloads-269d

## Discussion prompt from the author

The post asks other architects whether they’ve:

- hit **session exhaustion** limits when building edge API gateways, and/or
- used **stateless broker-side routing** vs relying on **sticky sessions** at the load balancer.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-architecture/architecture-pattern-scaling-sync-over-async-edge-gateways-by/m-p/4510919#M832)

