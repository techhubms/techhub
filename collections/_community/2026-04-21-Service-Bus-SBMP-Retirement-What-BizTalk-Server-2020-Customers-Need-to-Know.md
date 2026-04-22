---
title: 'Service Bus SBMP Retirement: What BizTalk Server 2020 Customers Need to Know'
date: 2026-04-21 01:02:21 +00:00
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/service-bus-sbmp-retirement-what-biztalk-server-2020-customers/ba-p/4513155
feed_name: Microsoft Tech Community
section_names:
- azure
author: hcamposu
primary_section: azure
tags:
- AMQP
- Azure
- Azure Logic Apps
- Azure Service Bus
- Backward Compatibility
- BizTalk Migration
- BizTalk Modernization
- BizTalk Server
- BizTalk Server CU6
- BizTalk Server CU7
- BizTalk Service Bus Adapter
- Community
- Hotfix
- KB5091375
- Large File Patterns
- Large Message Patterns
- Non Production Validation
- Protocol Migration
- SB Messaging Adapter
- SBMP
- Service Bus Messaging Protocol
- Service Bus SDK
---

hcamposu explains the upcoming retirement of Azure Service Bus SBMP (September 30, 2026) and what BizTalk Server 2020 customers using the SB-Messaging adapter should do, including installing a hotfix (KB5091375) to move the adapter to AMQP and validating key scenarios ahead of the deadline.<!--excerpt_end-->

# Service Bus SBMP Retirement: What BizTalk Server 2020 Customers Need to Know

On **September 30, 2026**, the **Azure Service Bus** team will retire support for the **Service Bus Messaging Protocol (SBMP)**. This matters if you’re a **BizTalk Server 2020** customer using the **BizTalk Service Bus (SB-Messaging) adapter**, because that adapter relies on SBMP today.

To help customers maintain continuity (while planning a longer-term transition to **Azure Logic Apps**), Microsoft has released a **BizTalk Server 2020 hotfix** that adds support for **AMQP (Advanced Message Queuing Protocol)** in the adapter.

## What’s changing

- **SBMP support retires on September 30, 2026** in **Azure Service Bus**.
- A **hotfix enables AMQP** for the BizTalk Service Bus (**SB-Messaging**) adapter.
  - Request the hotfix by opening a support case and asking for **KB5091375**.
- With the hotfix installed:
  - **AMQP becomes the default transport**.
  - **SBMP remains available as an opt-in fallback** for backward compatibility.
- The hotfix will be available for **BizTalk Server 2020 CU6 and CU7**.
- The current hotfix is based on the **current Service Bus SDK** (which is scheduled for deprecation in **September 2026**).
  - An **updated version is expected in June**, based on the **new Service Bus SDK**.

## What you need to do

If you plan to keep using the BizTalk Server 2020 Service Bus adapter:

1. **Migrate your adapter configuration to AMQP**.
2. **Install the hotfix** well before September 2026, and validate it in a **non-production environment**.
3. **Validate your scenarios**, including:
   - **Large message/file patterns**
   - Any operational **fallback strategies** you depend on
4. Decide whether to validate now or wait for the June refresh:
   - Use the **current hotfix** to validate large file scenarios and fallback approaches, or
   - Wait for the **June SDK-based update** if you don’t need to install immediately.

## How to obtain the hotfix

- Open a Microsoft support case and request **KB5091375**, or contact your Microsoft account team.
- A new KB article will be issued for the **June update**.

## Support and lifecycle context

Microsoft states it remains committed to supporting **BizTalk Server 2020** according to the official product lifecycle. **Extended paid support** will be available after **April 2028**.

## Closing thoughts

If you’re using the SB-Messaging adapter today, the key action is to plan and schedule your move to **AMQP**, then run validation in non-production so you’re not forced into last-minute changes ahead of the **September 30, 2026** SBMP retirement.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/service-bus-sbmp-retirement-what-biztalk-server-2020-customers/ba-p/4513155)

