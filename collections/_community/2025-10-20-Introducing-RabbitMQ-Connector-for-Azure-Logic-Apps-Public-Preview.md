---
layout: "post"
title: "Introducing RabbitMQ Connector for Azure Logic Apps (Public Preview)"
description: "This announcement introduces the RabbitMQ Connector for Azure Logic Apps (Standard and Hybrid), enabling users to send and receive messages between Logic Apps and RabbitMQ. The content details key features, benefits, supported actions (triggers, publishes, completes), and highlights hybrid and on-premises scenarios, as well as the worldwide rollout."
author: "hcamposu"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-rabbitmq-connector-public-preview/ba-p/4462627"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-20 13:00:00 +00:00
permalink: "/community/2025-10-20-Introducing-RabbitMQ-Connector-for-Azure-Logic-Apps-Public-Preview.html"
categories: ["Azure"]
tags: ["Azure", "Azure Logic Apps", "Cloud Native", "Community", "Connectors", "Exchange Routing", "High Availability", "Hybrid Cloud", "Integration", "Message Broker", "Messaging", "Microservices", "Queue", "RabbitMQ", "Scalability", "Triggers", "Workflow Automation"]
tags_normalized: ["azure", "azure logic apps", "cloud native", "community", "connectors", "exchange routing", "high availability", "hybrid cloud", "integration", "message broker", "messaging", "microservices", "queue", "rabbitmq", "scalability", "triggers", "workflow automation"]
---

hcamposu details the new RabbitMQ Connector for Azure Logic Apps, outlining its features and configuration steps for sending and receiving messages in modern integration solutions.<!--excerpt_end-->

# Introducing RabbitMQ Connector for Azure Logic Apps (Public Preview)

Azure Logic Apps (Standard and Hybrid) now feature a native RabbitMQ Connector, making it possible to seamlessly send and receive messages between your Logic Apps workflows and RabbitMQ, a widely adopted open-source message broker.

## What is RabbitMQ?

RabbitMQ is known for delivering reliable, scalable messaging in various industries, including financial services, IoT, and e-commerce. Its strengths include:

- **Reliability** with strong delivery guarantees
- **Flexible routing** via exchanges (direct, topic, fanout, headers)
- **Clustering and high availability**
- **Easy management and monitoring**
- **Extensibility** (authentication, federation, and plugins)

## Key Features of the Connector

- **Triggers (Receiving Messages):**
  - Use the 'When the queue has messages from RabbitMQ' operation as a trigger in Logic Apps.
  - Peek-lock operations are available for non-destructive reads.
  - Configure by providing the relevant Queue Name.
  - Downstream actions can process or transform the message payload.

- **Publishing Messages:**
  - Select the 'Send a message' operation to publish messages to RabbitMQ, specifying:
    - Queue Name
    - Message Body
    - Exchange Name (optional, for routing)
    - Routing Key
  - Allows integration of asynchronous messaging patterns in your workflows.

- **Completing Messages:**
  - With peek-lock enabled scenarios, use the 'Complete message' action to acknowledge or reject messages.
  - Requires Delivery Tag, Consumer Tag, and an Acknowledgment value (Complete or Reject).

- **Create Queues:**
  - Directly create RabbitMQ queues using the 'Create a queue' action from Logic Apps.

## Hybrid and On-Premises Support

The connector supports hybrid messaging scenarios, allowing on-premises RabbitMQ integration using Logic Apps (Standard) hybrid capabilities.

## Getting Started

1. In the Logic Apps Designer, search for the RabbitMQ connector.
2. Choose the required trigger or action (e.g., When a queue has messages, Send a message, Complete message, Create a queue).
3. Fill in the configuration fields as described above.
4. Use the message payload in downstream Logic Apps actions such as Compose, conditionals, or external calls.

## Supported Regions

Azure is rolling out this connector globally, though some regions may receive it before others.

## Additional Resources

- For demonstrations and configuration walkthroughs, refer to any accompanying video or Azure documentation (video link was not provided in the announcement).

---

**Author:** hcamposu

For more information and updates, visit the Azure Integration Services Blog.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-rabbitmq-connector-public-preview/ba-p/4462627)
