---
section_names:
- ml
- security
primary_section: ml
title: 'What’s new in Fabric Eventstream: 2026 Q1 Edition'
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-eventstream-2026-q1-edition/
author: Microsoft Fabric Blog
date: 2026-04-09 10:00:00 +00:00
tags:
- Anomaly Detection
- Azure Key Vault
- Azure Managed Virtual Network
- Change Data Capture (cdc)
- Confluent Schema Registry
- Custom CA Certificates
- DeltaFlow
- ExpressRoute
- Fabric Eventstreams
- Fabric Notebooks
- IoT Telemetry
- Kafka
- Microsoft Fabric
- ML
- MQTT V3.1
- MQTT V3.1.1
- Mtls
- News
- On Premises
- Private Endpoints
- Private Networking
- PySpark
- Real Time Hub
- Real Time Intelligence
- Security
- Spark Structured Streaming
- Streaming Connectors
- VNet
- VPN
---

Microsoft Fabric Blog (with coauthor Arindam Chatterjee) summarizes Q1 2026 updates for Fabric Eventstreams, covering new connectors (DeltaFlow, MQTT v3, Anomaly Detection), tighter Spark Structured Streaming/Notebook integration, and enterprise networking and security features like private network ingestion and Key Vault-backed custom CA + mTLS.<!--excerpt_end-->

## Overview

Over Q1 2026 (January–March), Fabric Eventstreams shipped improvements focused on:

- Broader connectivity
- Richer real-time processing
- Secure, enterprise-ready networking and operations

This post highlights the most impactful Eventstreams features released during that period.

## Expand your access to real-time data: New and enhanced connectors

Eventstreams expanded the kinds of real-time signals you can ingest into Fabric (operational DBs, IoT telemetry, external context, and detection signals). The focus was reducing onboarding friction and making events easier to convert into analytics-ready streams.

### DeltaFlow: Event-driven apps from database changes (Preview)

DeltaFlow (Preview) targets database change events (inserts/updates/deletes) and aims to simplify turning those changes into structured streams in Fabric.

It specifically reduces the need to:

- Work directly with raw CDC formats
- Manually handle schema evolution
- Manually manage destination tables

Documentation: DeltaFlow output transformation (Preview)

Blog post: Building real-time, event-driven applications with Database CDC feeds and Fabric Eventstreams DeltaFlow (Preview)

### MQTT v3 support in the Eventstreams MQTT connector (Preview)

Eventstreams added MQTT v3 compatibility by supporting:

- MQTT v3.1
- MQTT v3.1.1

This is intended to help onboard existing brokers and device fleets without requiring protocol upgrades or custom shims.

Documentation: Add MQTT source to an eventstream

### Turn detected anomalies into streaming inputs (Preview)

Anomaly Detection can now be used as an Eventstreams source, so detection signals become first-class streaming inputs that can be enriched, routed, and automated using the same Eventstreams pipelines as other sources.

Documentation: Add Anomaly detection events source to an eventstream

## Rich and robust real-time stream processing

Q1 deepened integration with Spark Structured Streaming and Notebooks for more expressive real-time processing (advanced analytics, stateful computation, and custom logic).

### Process streaming data with Fabric Eventstreams & Spark streaming (Preview)

Eventstreams introduced deeper integration between Eventstreams and Spark notebooks to reduce connection/setup boilerplate for Spark developers.

Capabilities called out:

- Discover and explore Eventstreams via Real-Time Hub from Fabric Notebooks
- Auto-generate PySpark code snippets to connect to selected Eventstreams
- Select, load, and reuse shared Fabric Notebooks from Eventstreams
- Connect securely from Fabric Spark clusters without embedding connection strings and SAS keys in code
- Use Notebook auto-retry policies to restart Spark jobs after errors

Documentation: Add a Spark Notebook destination to an eventstream

Blog post: Bringing together Fabric Real-time Intelligence, Notebook and Spark Structured Streaming (Preview)

## Operate with confidence: Reliability, security and control

These updates focus on secure connectivity, operational control, and predictable behavior for regulated or enterprise environments.

### Stream real-time data from private networks into Eventstreams (Preview)

Eventstreams now supports ingesting from sources in private network environments (VNets or on-prem).

Approach described:

- Use an Azure managed virtual network as an intermediary bridge
- Connect via VPN, ExpressRoute, peering, private endpoints, etc.
- Inject Eventstream connectors into that virtual network
- Manage through a streaming virtual network data gateway experience in Fabric

Documentation: Eventstream streaming connector virtual network and on-premises support overview

### Secure data streaming: Custom CA and mTLS in connectors (Preview)

Eventstream connectors can use certificates stored/managed in Azure Key Vault to encrypt data in transit.

Key security capabilities:

- Custom CA certificates
- mTLS (mutual TLS)
- Centralized certificate management for sharing across teams and consistent practices
- Certificate rotation without manual connector updates

Availability noted for Kafka-based sources, including:

- Apache Kafka
- Amazon Managed Streaming for Apache Kafka (MSK)
- Confluent Cloud for Apache Kafka
- Confluent Schema Registry

Documentation: Add and Manage Eventstream Sources

## Putting it all together: A real-time operations scenario

Building on the July–December 2025 update (linked in the post), the article outlines an end-to-end scenario for a global online service using Q1 2026 features:

- Stream order/payment/fulfillment changes from operational DBs (e.g., Azure SQL or PostgreSQL) using DeltaFlow (Preview)
- Ingest IoT telemetry (warehouse sensors, barcode scanners, temperature monitors) using the MQTT connector with MQTT v3 support
- Use private network connectivity so critical sources can remain isolated behind private-link-enabled environments
- Emit Anomaly Detection results into Eventstreams for real-time alerting and analytics
- Consume Eventstreams with Notebooks + Spark Structured Streaming (Preview) to build real-time AI pipelines (rolling features, near-real-time scoring, incident enrichment with model output)

## Try it and share feedback

Ways to engage mentioned in the post:

- Free trial: https://aka.ms/try-fabric
- Questions: askeventstreams@microsoft.com
- Submit/vote on ideas: https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas
- Fabric Community: https://aka.ms/FabricBlog/Community


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-eventstream-2026-q1-edition/)

