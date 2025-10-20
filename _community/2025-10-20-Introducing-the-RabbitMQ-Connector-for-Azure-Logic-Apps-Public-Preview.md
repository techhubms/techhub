---
layout: "post"
title: "Introducing the RabbitMQ Connector for Azure Logic Apps (Public Preview)"
description: "This announcement introduces the new RabbitMQ Connector for Azure Logic Apps (Standard and Hybrid). The connector enables developers to create robust messaging workflows by allowing Logic Apps to send and receive messages with RabbitMQ, supporting hybrid, on-premises, and cloud messaging scenarios, including triggers, publishing, and advanced message management."
author: "hcamposu"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-the-rabbitmq-connector-public-preview/ba-p/4462627"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-20 13:52:07 +00:00
permalink: "/2025-10-20-Introducing-the-RabbitMQ-Connector-for-Azure-Logic-Apps-Public-Preview.html"
categories: ["Azure"]
tags: ["Azure", "Azure Logic Apps", "BizTalk Modernization", "Clustering", "Community", "Connector", "Exchange", "High Availability", "Hybrid Integration", "Messaging", "On Premises Integration", "Peek Lock", "Publish", "Queue", "Queue Management", "RabbitMQ", "Triggers"]
tags_normalized: ["azure", "azure logic apps", "biztalk modernization", "clustering", "community", "connector", "exchange", "high availability", "hybrid integration", "messaging", "on premises integration", "peek lock", "publish", "queue", "queue management", "rabbitmq", "triggers"]
---

hcamposu announces the RabbitMQ Connector in Azure Logic Apps, detailing how developers can set up messaging workflows by sending and receiving messages with RabbitMQ in both standard and hybrid scenarios.<!--excerpt_end-->

# Introducing the RabbitMQ Connector for Azure Logic Apps (Public Preview)

**Author: hcamposu**

## Overview

Microsoft has released the RabbitMQ Connector for Azure Logic Apps (Standard and Hybrid), enabling developers to both send and receive messages using RabbitMQ within Logic Apps workflows. RabbitMQ is a popular open-source message broker, widely used for building scalable and reliable messaging solutions across various industries.

## Key Features & Benefits

- **Supports Triggers (Receive):** Configure a Logic App to trigger when a message arrives in a RabbitMQ queue.
- **Publishing Capabilities:** Send messages to RabbitMQ from Logic Apps, allowing integration with external systems and services.
- **Hybrid Scenarios:** Enables on-premises messaging solutions via Logic Apps hybrid capabilities.
- **Peek Lock Operations:** Supports non-destructive reads for scenarios requiring message inspection before completion.
- **Message Completion Handling:** Complete or reject messages using built-in actions, with support for delivery and consumer tags.
- **Queue Management:** Create queues directly from Logic Apps using the connector.
- **Robustness:** Leverages RabbitMQ's reliability, clustering, high availability, advanced routing, and extensibility.

## How to Use the Connector

### Receiving Messages

1. In the Logic Apps designer, search for the **RabbitMQ** connector.
2. Add the "When the queue has messages from RabbitMQ" operation as a trigger.
3. Configure required parameters, such as **Queue Name**.
4. Use the triggered payload in downstream Logic Apps actions, such as a **Compose** action.

### Publishing Messages

1. Search for the RabbitMQ connector and select the **Send a message** operation.
2. Configure:
   - **Queue Name**
   - **Message Body**
   - **Exchange Name** (if routing is needed)
   - **Routing Key**
3. Messages can now be published from your Logic App to a RabbitMQ endpoint.

### Completing Messages

1. For workflows using peek-lock, add the **Complete message** action from the RabbitMQ connector.
2. Provide **Delivery Tag**, **Consumer Tag**, and specify the acknowledgment type (Complete or Reject).
3. This manages message state directly from your workflow.

### Additional Actions

- Use **Create a queue** to provision new RabbitMQ queues.

## Supported Regions

The RabbitMQ Connector is rolling out globally, with regional availability expanding over time.

For demonstration videos and further guidance, refer to Microsoft documentation and Logic Apps resources.

---
*Version 2.0 (Updated Oct 20, 2025)*

## Useful Tags

- RabbitMQ
- Azure Logic Apps
- Messaging
- Hybrid Integration
- Connector

## About the Author

[hcamposu](https://techcommunity.microsoft.com/users/hcamposu/1524165) is a member of the Microsoft Tech Community and regularly shares updates on Azure Integration Services.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-the-rabbitmq-connector-public-preview/ba-p/4462627)
