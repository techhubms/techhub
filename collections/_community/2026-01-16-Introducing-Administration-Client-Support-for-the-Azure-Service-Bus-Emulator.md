---
external_url: https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/introducing-administration-client-support-for-the-azure-service/ba-p/4486433
title: Introducing Administration Client Support for the Azure Service Bus Emulator
author: Sannidhya_Glodha
feed_name: Microsoft Tech Community
date: 2026-01-16 17:36:47 +00:00
tags:
- .NET
- Administration Client
- Azure Service Bus
- Configuration
- Development Tools
- Docker
- Emulator
- Entity Management
- Local Development
- Messaging
- Queues
- SDK
- Service Bus Emulator
- Topics
section_names:
- azure
- coding
primary_section: coding
---
Sannidhya_Glodha details how the Azure Service Bus emulator now supports the Administration Client, enabling developers to create, update, and delete entities locally—making local testing workflows and management much more dynamic.<!--excerpt_end-->

# Introducing Administration Client Support for the Azure Service Bus Emulator

The Azure Service Bus emulator now provides support for the Administration Client, allowing developers to create, update, and delete Service Bus entities (like queues and topics) during local development—without having to restart the emulator. This greatly enhances the flexibility of local testing and development workflows.

## What Is the Change?

Previously, the Service Bus emulator permitted only declarative entity management via configuration files, which meant all required entities had to be defined before starting the emulator. With the new update, you can use the Administration Client to perform dynamic management operations—such as creating or deleting queues and topics—while the emulator is running.

## Why Is This Useful?

- **Dynamic Management:** No need to restart the emulator for entity changes.
- **Closer to Production:** Local environments now more accurately reflect real-world Azure setups.
- **Improved Developer Productivity:** Quicker testing, iteration, and deployment for apps that need runtime entity operations.

## How to Use

- The emulator exposes management operations on port 5300 by default. When using the Service Bus Administration Client, ensure you specify this port in your connection string.
- Declarative configuration through config files is still supported and remains the source of truth on initialization, overriding dynamic changes if you reset or restart.

## Supported Platforms and SDKs

- The Service Bus emulator is available as a Docker image and can run on Windows, macOS, and Linux.
- Supported by the latest .NET Service Bus client SDK for both messaging and management operations.

## Resources

- [Azure Service Bus emulator documentation](https://learn.microsoft.com/en-us/azure/service-bus-messaging/overview-emulator)
- [Docker image for the emulator](https://mcr.microsoft.com/en-us/artifact/mar/azure-messaging/servicebus-emulator/tags)
- [Reference code samples](https://github.com/Azure/azure-service-bus-emulator-installer/tree/main/Sample-Code-Snippets)
- [GitHub repository for feedback and issues](https://github.com/Azure/azure-service-bus-emulator-installer)

## Feedback

The Service Bus team welcomes your suggestions, issues, and feedback via their GitHub repository to help enhance the emulator.

Happy building—and may all your local tests pass!

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/introducing-administration-client-support-for-the-azure-service/ba-p/4486433)
