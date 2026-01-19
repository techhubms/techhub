---
external_url: https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/what-s-new-in-azure-event-grid/ba-p/4458299
title: "What's New in Azure Event Grid: Security, MQTT, and Smart Factory Integration"
author: Connected-Seth
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-10-01 23:22:50 +00:00
tags:
- API Security
- Azure Event Grid
- Azure Fabric
- Azure Functions
- Cloud Events
- Custom Webhook
- Device Authentication
- Edge To Cloud
- Event Driven Architecture
- Industry 4.0
- IoT
- JWT
- Microsoft Entra ID
- MQTT
- OAuth 2.0
- Pub/Sub
- Real Time Analytics
- SCADA
- Sparkplug B
- Telemetry
section_names:
- azure
- devops
- security
---
Connected-Seth explores Azure Event Grid's latest advancements, such as stronger MQTT security, broad event integration, and Sparkplug B support for smart factories, offering secure and scalable event-driven architectures on Azure.<!--excerpt_end-->

# What's New in Azure Event Grid: Security, MQTT, and Smart Factory Integration

Azure Event Grid introduces significant enhancements designed to elevate security, scalability, and interoperability for modern event-driven architectures. These improvements address the needs of real-time telemetry, automation, hybrid workloads, and industrial IoT scenarios.

## Key Feature Updates

### 1. Enhanced Security Models

- **OAuth 2.0 Authentication for MQTT**
  - Authenticate MQTT clients using JWTs from any OpenID Connect (OIDC)-compliant provider.
  - Integrate with Microsoft Entra ID (formerly Azure AD) and custom/third-party IAM.
  - [Learn more](https://learn.microsoft.com/en-us/azure/event-grid/oauth-json-web-token-authentication)
- **Custom Webhook Authentication**
  - Use Azure Functions or external services to validate clients.
  - Support SAS, API keys, credentials, and X.509 fingerprint verification.
  - Fits dynamic device fleets and multitenant architectures.
  - [Learn more](https://learn.microsoft.com/en-us/azure/event-grid/authenticate-with-namespaces-using-webhook-authentication)

### 2. Operational and Device Management Improvements

- **Assigned Client Identifiers (Preview)**
  - Deterministically assign pre-approved IDs for MQTT clients.
  - Improves session continuity, device tracking, diagnostics, and audits in regulated or large-scale environments.
  - [More info](https://learn.microsoft.com/en-us/azure/event-grid/mqtt-support#assigned-client-identifiers-preview)
- **HTTP Publish (Public Preview)**
  - Allow HTTP-based services and legacy applications to publish directly to Event Grid topics.
  - Enables smooth integration of RESTful and webhook-based workflows.
  - [Details](https://learn.microsoft.com/en-us/azure/event-grid/mqtt-http-publish)
- **MQTT Retain Support (Public Preview)**
  - Store and deliver the last known message on topic subscription.
  - Useful for telemetry, stateful dashboards, and device shadows.
  - [Details](https://learn.microsoft.com/en-us/azure/event-grid/mqtt-retain)

### 3. Tight Integration with Microsoft Fabric

- **First-Class Fabric Integration**
  - Route MQTT and Cloud Events from Event Grid directly to Fabric Event Streams for analytics, storage, and visualization—without intermediate hops through Event Hub.
  - [Learn more](https://learn.microsoft.com/en-us/azure/event-grid/mqtt-events-fabric)

## Smart Factories with Azure Event Grid MQTT Broker and Sparkplug B

Industrial organizations embracing Industry 4.0 can now leverage:

- **Sparkplug B Protocol**
  - Purpose-built for industrial IoT, Sparkplug B ensures standardized data exchange and device lifecycle monitoring (e.g., BIRTH, DEATH events).
- **Reliable Messaging**
  - QoS 1 for at-least-once delivery, Last Will & Testament for device state changes.
  - Retained messages so new subscribers get the latest machine data.
- **Edge-to-Cloud Data Flow**
  - Connect sensors and gateways via MQTT to Event Grid, supporting real-time dashboards, predictive analytics, and automated alerts in Azure Data Explorer or Fabric.
  - [Sparkplug B support](https://learn.microsoft.com/en-us/azure/event-grid/sparkplug-support)

## Industry Use Cases

- Real-time machine monitoring across plants
- Predictive maintenance through trend insights
- Seamless integration with Ignition SCADA via Cirrus Link
- Secure, scalable data pipelines for factory floors

## Getting Started

- [Azure Event Grid MQTT Overview](https://learn.microsoft.com/en-us/azure/event-grid/mqtt-overview)
- [Fabric and Event Grid Integration](https://learn.microsoft.com/en-us/azure/event-grid/mqtt-events-fabric)
- [Sparkplug Support](https://learn.microsoft.com/en-us/azure/event-grid/sparkplug-support)

---

*Authored by Connected-Seth, updated October 1, 2025. For more Azure messaging updates, follow the Messaging on Azure Blog.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/what-s-new-in-azure-event-grid/ba-p/4458299)
