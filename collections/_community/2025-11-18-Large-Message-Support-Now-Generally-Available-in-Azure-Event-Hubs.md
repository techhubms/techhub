---
layout: post
title: Large Message Support Now Generally Available in Azure Event Hubs
author: Sannidhya_Glodha
canonical_url: https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/general-availability-large-message-support-in-azure-event-hubs/ba-p/4471094
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 19:05:31 +00:00
permalink: /azure/community/Large-Message-Support-Now-Generally-Available-in-Azure-Event-Hubs
tags:
- AMQP
- Apache Kafka
- Azure Event Hubs
- Azure Portal
- Client Configuration
- Cloud Messaging
- Cloud Native
- Data Streaming
- Dedicated Clusters
- Event Processing
- Large Message Support
- Message Size
- Microsoft Azure
- Reliability
- Streaming
section_names:
- azure
---
Sannidhya_Glodha announces the general availability of Large Message Support in Azure Event Hubs, enabling streaming messages up to 20 MB in scalable dedicated clusters for reliable data processing.<!--excerpt_end-->

# Large Message Support Now Generally Available in Azure Event Hubs

Azure Event Hubs is a cloud-native service designed to handle massive volumes of event streaming with minimal latency. Compatible with Apache Kafka, it lets you migrate existing Kafka workloads to Azure without requiring any code modifications.

## What’s New

**Large Message Support** is now generally available in Azure Event Hubs. You can send and receive messages up to **20 MB** in self-serve, scalable dedicated clusters. This capability is ideal for scenarios where larger, indivisible event payloads must be processed quickly and reliably. The feature brings enhanced reliability and flexibility to data streaming solutions built on Azure.

## Protocol Compatibility

- Works seamlessly with both **AMQP** and **Apache Kafka** protocols
- No client code changes needed to support larger message sizes

## Implementation Steps

1. Check your eligibility for Large Message Support (dedicated clusters required)
2. Configure settings via the **Azure Portal**
3. Review client-side settings for timeouts and maximum message size parameters to avoid limiting throughput

**Useful Links:**

- [Azure Event Hubs for Apache Kafka Overview](https://learn.microsoft.com/en-us/azure/event-hubs/azure-event-hubs-apache-kafka-overview)
- [Quickstart: Send and Receive Large Messages with Azure Event Hubs](https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-quickstart-stream-large-messages)

## Additional Information

- Feature is available on self-serve, dedicated clusters only
- No code changes required for existing Kafka applications
- Feedback from users is encouraged to further improve the service

*Published by Sannidhya_Glodha on November 18, 2025*

---

For eligibility and more technical details, visit [aka.ms/largemessagesupportforeh](https://aka.ms/largemessagesupportforeh).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/general-availability-large-message-support-in-azure-event-hubs/ba-p/4471094)
