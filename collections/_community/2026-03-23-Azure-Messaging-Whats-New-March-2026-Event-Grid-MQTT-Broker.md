---
author: Connected-Seth
tags:
- Azure
- Azure Data Explorer
- Azure Event Grid
- Azure Functions
- Bulk Client Onboarding API
- CloudEvents
- Community
- Device Onboarding
- Event Grid MQTT Broker
- Event Hubs
- Fabric Eventstreams
- HTTP Publish
- JWT
- Last Will And Testament
- Latency Metrics
- Logic Apps
- Microsoft Entra ID
- Microsoft Fabric
- ML
- MQTT
- MQTT V3.1.1
- MQTT V5.0
- Mtls
- OAuth 2.0
- Observability
- OIDC
- Quotas And Limits
- Real Time Intelligence
- Retained Messages
- Security
- Shared Subscriptions
- Sparkplug B
- TCP
- TLS 1.2
- WebSocket
- X.509
- Zero Trust
external_url: https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/azure-event-grid-mqtt-broker-enterprise-grade-messaging-for-the/ba-p/4504246
section_names:
- azure
- ml
- security
primary_section: ml
date: 2026-03-23 17:48:18 +00:00
title: 'Azure Messaging: What’s New (March 2026) — Event Grid MQTT Broker'
feed_name: Microsoft Tech Community
---

Connected-Seth shares March 2026 updates for Azure Event Grid MQTT Broker, covering protocol support (MQTT v3.1.1/v5, HTTP publish), security options (Entra ID/OAuth JWT, X.509, webhook auth, TLS 1.2+), scaling characteristics, and native routing into Azure services like Fabric Eventstreams, Azure Data Explorer, Event Hubs, Functions, and Logic Apps.<!--excerpt_end-->

# Azure Messaging: What’s New (March 2026) — Event Grid MQTT Broker

## Overview

Azure Event Grid MQTT Broker is positioned as an enterprise MQTT broker on Azure for large-scale connected ecosystems (IoT, industrial, automotive, retail, finance, cloud-native services). The post highlights:

- MQTT protocol coverage (MQTT v3.1.1 and MQTT v5.0)
- Multiple transports and ingestion options (TCP, WebSocket, and HTTP publish)
- “Zero-trust” security capabilities (Entra ID/OAuth 2.0 JWT, X.509, webhook auth, TLS enforcement)
- Scale and quota highlights (including 1,000 msg/sec/session ingress and up to 5M+ connections via onboarding)
- Azure-native routing into analytics and integration services (Fabric Eventstreams, Azure Data Explorer, Event Hubs, Azure Functions, Logic Apps)
- Operational telemetry/metrics for visibility

## Core protocol

## Full MQTT protocol coverage

The post claims end-to-end MQTT compliance across versions, transports, and messaging patterns.

### Supported protocol and transport options

- MQTT v3.1.1 (full compliance)
- MQTT v5.0 (richer features such as user properties)
- TCP transport
- WebSocket support
- HTTP Publish (REST-based ingestion)

### MQTT v5 enhancements (GA)

MQTT v5 enhancements called out include:

- Richer error signaling
- User properties
- Message expiry
- Request–response patterns

### HTTP Publish (REST Bridge) (GA)

HTTP Publish allows non-MQTT services to publish via HTTPS. The post positions this for:

- REST backends
- Legacy systems
- Webhooks joining real-time workflows

Reference: https://learn.microsoft.com/en-us/azure/event-grid/mqtt-http-publish

### Shared subscriptions (Preview)

Shared subscriptions are described as:

- Load-balancing messages across consumer groups
- Scaling processing horizontally without duplication

## Security (zero-trust focus)

## Enterprise authentication stack

Security options highlighted as generally available:

- **Microsoft Entra ID / OAuth 2.0 JWT**
  - Authenticate via any OIDC-compliant identity provider (Entra ID, Auth0, custom IAM)
  - Reference: https://learn.microsoft.com/en-us/azure/event-grid/oauth-json-web-token-authentication
- **X.509 certificate authentication**
  - Hardware-rooted device identity
  - Mutual TLS (mTLS), certificate fingerprint validation, PKI integration
- **Custom webhook authentication**
  - Dynamic client validation via Azure Functions or external services
  - Examples mentioned: SAS keys, API keys, cert fingerprints
  - Reference: https://learn.microsoft.com/en-us/azure/event-grid/authenticate-with-namespaces-using-webhook-authentication
- **TLS 1.2+ enforced**
  - Transport encryption enforced; “no downgrade paths”

### Assigned client identifiers (deterministic identity)

The post describes pre-assigning approved MQTT client IDs for:

- Session continuity
- Enhanced diagnostics
- Audit trails
- Regulated/long-lived device scenarios

## Scale and performance

## Built for massive scale

The post highlights:

- Up to **5M+ concurrent connections** (with a note to contact Microsoft for onboarding production workloads)
- **1,000 messages/sec/session (ingress)** quota call-out
- **15 topic segments (GA)** quota call-out

Quotas reference: https://learn.microsoft.com/en-us/azure/event-grid/quotas-limits

### Large messages (Preview)

- **1 MB large message support** (Preview)
- Positioned for images, video frames, large telemetry batches without pre-chunking

### Autoscaling and networking (Coming soon in Preview)

- Auto scale up & down (elastic namespace scaling)
- IPv6 support (dual-stack networking)

### Bulk client onboarding API (Preview)

- Batch device registration (credentials, certificates, authorization rules)
- Positioned as “CI/CD-native”

### High egress throughput (Preview)

- Up to **500 msg/sec/instance egress** for high fan-out scenarios

## Azure-native integration

## MQTT events routed into Azure services

The post lists routing MQTT streams into:

- Fabric Eventstreams
- Azure Data Explorer
- Event Hubs
- Azure Functions
- Logic Apps

### First-class Microsoft Fabric integration

Event Grid Namespaces can route MQTT messages and CloudEvents directly to **Fabric Eventstreams** without needing an Event Hub intermediary.

- Fabric Eventstreams reference: https://learn.microsoft.com/en-us/azure/event-grid/mqtt-events-fabric
- Example reference architectures:
  - https://learn.microsoft.com/en-us/industry/mobility/architecture/automotive-messaging-data-analytics-content?toc=/azure/event-grid/toc.json
  - https://learn.microsoft.com/en-us/fabric/real-time-intelligence/architectures/connected-fleet

## State & presence

## Last Will & Testament + retained messages

- **Last Will & Testament (LWT)** (GA)
  - Notify subscribers when a device disconnects unexpectedly
  - Positioned for industrial automation, fleet monitoring, health-critical telemetry
- **Retained messages** (GA)
  - Subscribers receive the latest device value immediately on connect
  - Mentions configurable expiry and future “Get/List API and portal experience coming soon”
  - Reference: https://learn.microsoft.com/en-us/azure/event-grid/mqtt-retain

## Industry 4.0 / Sparkplug B

## Sparkplug B on Azure

The post states Sparkplug B runs natively on Event Grid MQTT Broker.

- Sparkplug B support reference: https://learn.microsoft.com/en-us/azure/event-grid/sparkplug-support

### Capabilities and integrations called out

- Device lifecycle tracking (BIRTH/DEATH)
- SCADA integration
  - Ignition SCADA with Cirrus Link Chariot
  - Auto-discover tags; “zero manual configuration” described
- Ingestion into Azure Data Explorer and/or Fabric for dashboards and automated alerting

### Edge-to-cloud flow (high-level)

1. Sensors publish Sparkplug B messages via edge gateways
2. Event Grid MQTT Broker ingests with QoS 1, TLS, LWT, retained messages
3. SCADA auto-discovers tags (Ignition + Chariot)
4. Stream routed to Azure Data Explorer / Fabric for operational dashboards and analytics

## Operational visibility

## Metrics highlighted

The post calls out broker telemetry for monitoring and SLA operations, including:

- CONNACK / PUBACK latency metrics
  - Reference: https://learn.microsoft.com/en-us/azure/event-grid/monitor-mqtt-delivery-reference#metrics
- Publish/subscribe success rates (including throttling tracking)
- Active connections (real-time view)

## What’s next (coming to preview)

Capabilities listed as entering preview soon:

- Large message packets (1 MB)
- Shared subscriptions
- Subscription identifier (MQTT v5 compliance; tagging subscriptions)
- Bulk client onboarding API
- Auto scale up & down
- IPv6 support

Early access contact: askmqtt@microsoft.com

## Links

- Get started overview: https://learn.microsoft.com/en-us/azure/event-grid/overview
- Quotas & limits: https://learn.microsoft.com/en-us/azure/event-grid/quotas-limits
- Explore reference architectures: https://learn.microsoft.com/en-us/fabric/real-time-intelligence/architectures/connected-fleet

Updated Mar 22, 2026 (Version 1.0)


[Read the entire article](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/azure-event-grid-mqtt-broker-enterprise-grade-messaging-for-the/ba-p/4504246)

