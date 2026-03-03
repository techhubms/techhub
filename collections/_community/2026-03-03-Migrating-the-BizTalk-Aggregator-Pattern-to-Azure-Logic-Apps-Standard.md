---
layout: "post"
title: "Migrating the BizTalk Aggregator Pattern to Azure Logic Apps Standard"
description: "This in-depth guide explains how to migrate or implement the BizTalk Server Aggregator pattern to Azure Logic Apps Standard using an officially supported, production-ready template. It covers the architectural mapping, workflow steps, schema reuse, and correlation mechanics using Azure Service Bus, with deployment instructions and real-world integration advice. Both migrations and new (greenfield) projects are addressed, and all technical requirements around flat file decoding and message correlation are covered."
author: "reynaldom"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/implementing-migrating-the-biztalk-server-aggregator-pattern-to/ba-p/4495107"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-03 16:08:08 +00:00
permalink: "/2026-03-03-Migrating-the-BizTalk-Aggregator-Pattern-to-Azure-Logic-Apps-Standard.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["Aggregator Pattern", "ARM Templates", "Azure", "Azure Logic Apps Standard", "Azure Service Bus", "BizTalk Server", "CI/CD", "Cloud Native PaaS", "Coding", "Community", "CorrelationId", "Decoupled Architecture", "DevOps", "Enterprise Integration Patterns", "Flat File Schema", "GitHub Actions", "Integration", "Message Orchestration", "Migration", "Service Bus Trigger", "Stateful Workflows", "Workflow Automation", "XSD"]
tags_normalized: ["aggregator pattern", "arm templates", "azure", "azure logic apps standard", "azure service bus", "biztalk server", "cislashcd", "cloud native paas", "coding", "community", "correlationid", "decoupled architecture", "devops", "enterprise integration patterns", "flat file schema", "github actions", "integration", "message orchestration", "migration", "service bus trigger", "stateful workflows", "workflow automation", "xsd"]
---

reynaldom details step-by-step how to migrate or deploy the BizTalk Server Aggregator pattern on Azure Logic Apps Standard, covering message correlation, workflow templates, and schema reuse for enterprise integration.<!--excerpt_end-->

# Migrating the BizTalk Server Aggregator Pattern to Azure Logic Apps Standard

This article provides a comprehensive guide to moving the BizTalk Server Aggregator pattern to Azure Logic Apps Standard using a production-ready template available in the Azure portal. Whether transitioning from BizTalk Server (due to end of life) or starting a new integration project, this template offers a streamlined, low-refactoring path with open-source support.

## Why Migrate?

- **BizTalk Server End of Life**: With BizTalk Server’s support lifecycle nearing its end, organizations are advised to transition critical enterprise integration patterns to cloud-native services.
- **Native Azure Solution**: Azure Logic Apps Standard supports scalable, flexible, and production-ready enterprise integration without requiring prior BizTalk expertise.

## Overview

The officially published template replicates the classic Aggregator pattern:

- Groups incoming service bus messages by `CorrelationId` (analogous to BizTalk’s promoted properties)
- Batches, decodes, and assembles flat file messages (using reusable BizTalk XSD schemas)
- Delivers aggregated results decoupled (via HTTP POST or similar)

## Component Mapping: BizTalk to Logic Apps

| Concept                        | BizTalk Server              | Azure Logic Apps Standard                   |
|-------------------------------|-----------------------------|---------------------------------------------|
| Messaging Infrastructure       | SQL Server MessageBox DB    | Azure Service Bus (cloud PaaS)              |
| Message Source                 | Receive Port                | Service Bus trigger (`peekLockQueueMessagesV2`) |
| Message Decoding               | Flat File Disassembler      | `FlatFileDecoding` action                   |
| Correlation Mechanism          | Promoted Property, Corr. Set| Service Bus `CorrelationId` property        |
| Message Accumulation           | Loops & assignment          | Loops & dictionary variable                 |
| Aggregated Message Construction| Construct Message/XMLPipeline| Compose & build actions                    |
| Error Handling                 | Exception handler           | Scope/error handler action                  |
| Schema Support                 | Flat File (XSD)             | Direct XSD reuse                            |
| Deployment                     | MSI, admin tools            | ARM, DevOps, GitHub Actions                 |

## Key Architecture Changes

- **Monolithic to Decoupled**: Logic Apps split logic into clear stages (aggregation, downstream processing).
- **Manual to Elastic Scale**: Azure scales automatically with the App Service Plan.
- **Native Monitoring**: Tracking via Azure Monitor and workflow run history.
- **Correlation Simplified**: No schema promotion required—Service Bus sets `CorrelationId` out of the box.

## How the Template Works

1. **Trigger**: Batches are received via Service Bus queue trigger (non-session).
2. **Correlation**: Each message’s `CorrelationId` is read from properties (equivalent to BizTalk promoted property).
3. **Flat File Decoding**: Message content is parsed via the `FlatFileDecoding` action using your BizTalk XSD schemas (imported into the Logic Apps Artifacts/Schemas folder).
4. **Message Grouping**: Messages are grouped into correlation groups (dictionary variable).
5. **Batch Build**: For each group, aggregate the messages, counting grouped members.
6. **Result Output**: Post aggregated output via HTTP (to any desired endpoint or workflow).
7. **Error Handling**: Workflow scopes provide robust error management.

### Example Message Structure

- Identifier (line 1)
- Partner URI (line 2): used for correlation in BizTalk, but now sourced from `CorrelationId` property
- Flat file data (subsequent lines)

### Schema Reuse

- **No Refactoring**: Upload BizTalk XSD files directly to your Logic App; the template supports instant use.
- **XML Support**: Use built-in XML validation, transformation, and parsing actions for XML data.

## Step-by-Step Deployment

1. **Prerequisites**:
   - Azure subscription
   - Logic Apps Standard resource
   - Azure Service Bus namespace with queue
   - Flat file XSD schemas (e.g., Invoice.xsd)
   - Target workflow (HTTP endpoint)

2. **Open Templates Gallery** in the Azure portal.
3. **Locate "Aggregate messages from Azure Service Bus by CorrelationId"** template, published by Microsoft.
4. **Review and Configure**:
   - Assign workflow name/state type (stateful recommended for aggregation)
   - Connect to Service Bus
   - Configure batch size, schema name, default CorrelationId, target URL, and timeout
   - Confirm sequential processing as desired
5. **Upload XSD Schema** to the Artifacts/Schemas folder in your logic app
6. **Test** by sending messages with varying CorrelationIds to the Service Bus queue and reviewing aggregation results

Full open source code and documentation are available:

- [Azure LogicAppsTemplates GitHub repository](https://github.com/Azure/LogicAppsTemplates/tree/main/aggregate-messages-servicebus-correlationid)

## Conclusion

This template enables organizations to maintain or modernize critical enterprise integration patterns as BizTalk Server approaches end of life. It leverages Azure Logic Apps, supports direct XSD schema reuse, and deploys in minutes with no infrastructure headaches. Whether modernizing legacy workflows or building cloud-native integrations, this approach provides technical depth and practical, production-friendly guidance.

## References & Resources

- [BizTalk Server Aggregator SDK sample](https://learn.microsoft.com/en-us/biztalk/core/aggregator-biztalk-server-sample)
- [Flat File Encoding and Decoding in Logic Apps](https://learn.microsoft.com/en-us/azure/logic-apps/logic-apps-enterprise-integration-flatfile)
- [Azure Service Bus Connector Overview](https://learn.microsoft.com/connectors/servicebus/)
- [BizTalk to Azure migration guide](https://learn.microsoft.com/azure/logic-apps/logic-apps-move-from-mabs)
- [BizTalk Migration Starter tool (GitHub)](https://github.com/haroldcampos/BizTalkMigrationStarter)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/implementing-migrating-the-biztalk-server-aggregator-pattern-to/ba-p/4495107)
