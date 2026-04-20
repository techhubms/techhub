---
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/secure-your-data-streams-how-to-choose-the-right-network-security-feature-in-eventstream/
tags:
- Amazon Kinesis
- Apache Kafka
- Azure
- Azure Event Hubs
- Azure IoT Hub
- Azure Private Link
- Confluent Cloud
- Database CDC
- Encryption At REST
- Encryption in Transit
- Eventstream
- Firewalls
- Google Pub/Sub
- Inbound Traffic
- Managed Private Endpoints
- Microsoft Entra ID
- Microsoft Fabric
- ML
- MQTT
- MySQL
- Network Security
- News
- Outbound Traffic
- PostgreSQL
- Private Endpoints
- Private Networks
- Real Time Intelligence
- Security
- SQL Server
- Streaming Data
- Virtual Network Injection
- VNet
feed_name: Microsoft Fabric Blog
title: 'Secure your data streams: How to choose the right network security feature in Eventstream'
date: 2026-04-20 09:00:00 +00:00
section_names:
- azure
- ml
- security
primary_section: ml
---

Microsoft Fabric Blog explains how Microsoft Fabric Eventstream (Real-Time Intelligence) handles inbound and outbound connectivity and how to pick between managed private endpoints, tenant/workspace private links, and streaming connector VNet injection to secure streaming sources in private networks.<!--excerpt_end-->

## Overview

Eventstream in **Microsoft Fabric Real-Time Intelligence** can stream data from sources inside Fabric and from external systems. When external sources are behind firewalls or in private networks, you need the right network security feature.

This post explains:

- How Eventstream network traffic is categorized (internal, inbound, outbound)
- The three network security features available
- A decision guide for choosing the right option

## Understanding network traffic direction

Eventstream traffic falls into three categories based on where the connection initiates:

- **Internal traffic**: Between Eventstream and other Fabric items (for example Lakehouses, weather feeds, Fabric events).
  - Stays within the Fabric security boundary
  - Protected by:
    - **Microsoft Entra ID** authentication
    - Workspace permissions
    - Encryption at rest and in transit
  - No extra network security configuration is needed

- **Inbound traffic**: External sources initiate connections to push data into Eventstream.
  - Examples:
    - **Azure Event Grid** pushing system events to Eventstream
    - A **custom application** pushing data to Eventstream

- **Outbound traffic**: Eventstream initiates connections to pull data from external sources.
  - Examples:
    - **Azure Event Hubs**
    - **Apache Kafka**
    - Database change data capture (CDC) sources

![A diagram of showing how network traffic direction is labeled for various Eventstream source–destination pairs.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/a-diagram-of-showing-how-network-traffic-direction.png)

## Three network security features

Eventstream provides three network security features, each aimed at different traffic directions and source types.

### Managed Private Endpoints (Generally Available)

- Purpose: Secure **outbound** connections to Azure services that aren’t publicly accessible
- Supported sources called out in the post:
  - **Azure Event Hubs**
  - **Azure IoT Hub**
- How it works:
  - Creating a managed private endpoint causes Fabric to provision a **managed virtual network**
  - Eventstream retrieves data over a private connection without traversing the public internet

Learn more: Connect to Azure resources securely using managed private endpoints

https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/set-up-private-endpoint

### Tenant and workspace private links

- Purpose: Secure **inbound** connections so only approved virtual networks can reach Eventstream
- Based on: **Azure Private Link**
- Scopes:
  - **Tenant-level private links**: apply across all workspaces in the tenant (company-wide policy)
  - **Workspace-level private links**: lock down specific workspaces while leaving others publicly accessible

Learn more: Secure inbound connections with Tenant and Workspace Private Links

https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/set-up-tenant-workspace-private-links

### Streaming connector virtual network injection (Preview)

- Purpose: Secure **outbound** connections to third-party sources in private networks
- Examples of supported third-party sources mentioned:
  - **Apache Kafka**
  - **Amazon Kinesis**
  - **Google Pub/Sub**
  - **Confluent Cloud**
  - **MQTT**
  - Database CDC sources like **PostgreSQL**, **MySQL**, and **SQL Server**
- How it works (as described):
  - You prepare an **Azure VNet** connected to your data source
  - Eventstream injects its streaming connector into that VNet to retrieve data securely

Learn more: Eventstream streaming connector virtual network and on-premises support overview

https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/streaming-connector-private-network-support-overview

## How to choose

The post suggests choosing based on a few questions:

- **Is your source within Fabric?**
  - For Fabric-native sources (workspace item events, OneLake events, sample data), the connection is secure by default.

- **Is your source in a protected network?**
  - If the source is publicly accessible, no additional network security feature is required.

- **What direction is the traffic?**
  - If external applications push data into Eventstream (**inbound**) → use **Private Links**
  - If Eventstream pulls data from external sources (**outbound**) → continue

- **Is the source Azure Event Hubs or Azure IoT Hub?**
  - Yes → use **Managed Private Endpoints**
  - No → use **Streaming Connector VNet Injection**

![Decision matrix diagram for selecting the appropriate network security feature for EventStream.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/decision-matrix-diagram-for-selecting-the-appropri.png)

For the full decision matrix/flowchart:

https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/choose-the-right-network-security-feature

## Key takeaway

Eventstream supports streaming securely from:

- Fabric-native sources (internal)
- External sources behind firewalls/private networks (inbound/outbound)
- Third-party cloud providers

The main step is to identify **traffic direction** and **source type**, then choose the corresponding Eventstream network security feature.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/secure-your-data-streams-how-to-choose-the-right-network-security-feature-in-eventstream/)

