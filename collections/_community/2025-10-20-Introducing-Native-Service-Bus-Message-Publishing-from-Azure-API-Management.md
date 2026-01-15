---
layout: post
title: Introducing Native Service Bus Message Publishing from Azure API Management
author: anishta
canonical_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-native-service-bus-message-publishing-from-azure-api/ba-p/4462644
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-10-20 01:22:49 +00:00
permalink: /azure/community/Introducing-Native-Service-Bus-Message-Publishing-from-Azure-API-Management
tags:
- API Gateway
- APIM
- Asynchronous Systems
- Azure
- Azure API Management
- Azure Functions
- Azure Service Bus
- Community
- DevOps
- Enterprise Integration
- Event Driven Architecture
- Event Processing
- Integration
- Logging
- Logic Apps
- Managed Identity
- Messaging
- Microservices
- Policy
- RBAC
- Secure API
- Security
- Service Bus
- Throttling
section_names:
- azure
- devops
- security
---
anishta introduces a native integration in Azure API Management, enabling direct, secure message publishing to Azure Service Bus using built-in policies and managed identities. This enhancement streamlines event-driven architectures and centralizes API governance.<!--excerpt_end-->

# Native Service Bus Message Publishing from Azure API Management

**Author: anishta**

## Overview

A new capability in Azure API Management (APIM) enables you to send messages directly to Azure Service Bus from your APIs through a built-in policy. This feature streamlines integration with event-driven and asynchronous backends, supporting the creation of scalable, resilient, and decoupled systems.

## Why This Matters

Modern cloud applications increasingly leverage asynchronous messaging and event-driven patterns for better scalability and flexibility. With this update:

- APIs in APIM can publish to Azure Service Bus with no SDKs or custom middleware needed.
- External consumers, partners, or IoT devices can connect using standard HTTP calls, decoupling front ends from backend logic.
- Centralized controls allow simplified authentication, throttling, and detailed logging—all managed in APIM.
- Builds more loosely-coupled, scalable systems by forwarding API payloads asynchronously to downstream processors.

## How It Works

- The new `send-service-bus-message` policy forwards API call payloads straight into Service Bus queues or topics.
- **Process Flow:**
  1. Client sends an HTTP request to your APIM-managed API endpoint.
  2. The configured policy executes, relaying the payload as a Service Bus message.
  3. Downstream services like Logic Apps, Azure Functions, or microservices consume these messages asynchronously.
- No additional code or infrastructure needs to be deployed; everything is managed within API Management.

## Getting Started

1. Create a Service Bus namespace and set up a queue or topic.
2. Enable a managed identity (system/user assigned) on your APIM instance.
3. Grant the identity the “Service Bus data sender” Azure RBAC role scoped to your target queue/topic.
4. Add the policy to your API operation, such as:

   ```xml
   <send-service-bus-message queue-name="orders">
       <payload>@(context.Request.Body.As<string>())</payload>
   </send-service-bus-message>
   ```

5. Save and deploy—API calls will now publish payloads to Service Bus.

[Full documentation and walkthrough here.](https://learn.microsoft.com/en-us/azure/api-management/api-management-howto-send-service-bus?utm_source=chatgpt.com)

## Common Use Cases

- **Order Processing:** Queue orders for fulfillment or billing.
- **Event Notifications:** Trigger workflows across cloud applications.
- **Telemetry Ingestion:** Process IoT/mobile data via Service Bus for analytics.
- **Partner Integration:** Provide REST endpoints for secure external access with policy-based governance.

## Security and Governance

- Uses managed identities—no secrets are required.
- Controls for authorization, rate limiting, quotas, and tracing are maintained in APIM policies.
- Service Bus metrics and APIM logging offer observability for end-to-end workflow.

## Design Patterns Enabled

- Easily build event-driven backbones—start with a single API or scale to enterprise-wide topic-based distribution.
- Reduce architectural complexity while maintaining flexibility and scalability through asynchronous, loosely-coupled integrations.

---
For further setup instructions, advanced usage, and full code examples, see the [Azure documentation](https://learn.microsoft.com/en-us/azure/api-management/api-management-howto-send-service-bus?utm_source=chatgpt.com).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-native-service-bus-message-publishing-from-azure-api/ba-p/4462644)
