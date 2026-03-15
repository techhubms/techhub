---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/a-biztalk-migration-tool-from-orchestrations-to-logic-apps/ba-p/4494876
title: 'BizTalk Migration Starter: Open Source Toolkit for Migrating BizTalk Applications to Azure Logic Apps'
author: hcamposu
primary_section: dotnet
feed_name: Microsoft Tech Community
date: 2026-02-13 19:03:08 +00:00
tags:
- Azure
- Azure Logic Apps
- BizTalk Migration
- BizTalk Server
- Cloud Migration
- Command Line Tools
- Community
- DevOps
- Enterprise Integration
- Integration Patterns
- Mapping Language
- Migration Toolkit
- Modernization
- Open Source Tools
- Orchestration Conversion
- Pipeline Conversion
- Solution Architecture
- Workflow Automation
- .NET
section_names:
- azure
- dotnet
- devops
---
hcamposu details the BizTalk Migration Starter toolkit—an open-source resource to convert BizTalk artifacts into Azure Logic Apps. The content clearly outlines migration components, practical command usage, and architecture for integration professionals.<!--excerpt_end-->

# BizTalk Migration Starter: From Orchestrations to Logic Apps Workflows

By hcamposu

As enterprises evolve toward cloud-native architectures, migrating BizTalk Server applications to Azure Logic Apps can present major technical hurdles. The BizTalk Migration Starter toolkit helps simplify this transition by automating artifact conversion, reducing manual effort, and preserving business logic and integration patterns.

## Architecture and Toolkit Components

The open-source toolkit (see [haroldcampos/BizTalkMigrationStarter](https://github.com/haroldcampos/BizTalkMigrationStarter)) includes:

- **BTMtoLMLMigrator:** Converts BizTalk Maps (.btm files) into Logic Apps Mapping Language (.lml), translating functoids and schemas. Command-line usage:
  - Single map: `BTMtoLMLMigrator.exe -btm "C:\BizTalkMaps\OrderToInvoice.btm" -source "C:\Schemas\Order.xsd" -target "C:\Schemas\Invoice.xsd" -output "C:\Output\OrderToInvoice.lml"`
  - Batch maps: `BTMtoLMLMigrator.exe -batchDir "C:\BizTalkMaps" -schemasDir "C:\Schemas" -outputDir "C:\Output\LMLMaps"`
  - **Troubleshooting:** Use correct schemas and the GHCP agent for debugging. Most functoids are supported except for some scripting cases, which require manual redesign in Logic Apps.

- **ODXtoWFMigrator:** Converts BizTalk Orchestrations (.odx) into Logic Apps workflows. Supports migration, refactoring, analysis, reporting, and batch operations, covering most shapes and adapters.
  - Example migration: `ODXtoWFMigrator.exe convert "C:\BizTalk\InventorySync.odx" "C:\BizTalk\BindingInfo.xml" "C:\Output\InventorySync.workflow.json"`
  - Configuration can be customized via JSON registry files. Reports and batch processing options are available.

- **BTPtoLA:** Translates BizTalk pipelines into Logic Apps equivalents for both receive and send scenarios, mapping pipeline components and preserving configurations.
  - Example usage: `BTPtoLA.exe -pipeline "C:\Pipelines\ReceiveOrderPipeline.btp" -type receive -output "C:\Output\ReceiveOrderPipeline.json"`

- **BizTalktoLogicApps.MCP:** Implements a Model Context Protocol server for AI-assisted migrations, providing a standardized interface and tool handlers for intelligent suggestions.

- **BizTalktoLogicApps.Tests:** An integrated suite of tests ensures accuracy for conversions, schema validations, batch processes, and migration output.

## Practical Recommendations

- Use real BizTalk schemas for mapping; unsupported functoids or shapes will be flagged, sometimes requiring manual solution redesign.
- Update connector registries as new Logic Apps connectors/providers are added.
- For troubleshooting, leverage the GHCP agent and review output reports.

## Who Should Use This Toolkit?

- Azure integration architects
- DevOps engineers modernizing BizTalk systems
- Developers and migration teams seeking automation and accuracy for logic and pipeline migration

For more details and ongoing updates, refer to the official repository and documentation. The toolkit provides not only automation but also systematic analysis and testing, improving confidence in complex migrations to Azure Logic Apps.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/a-biztalk-migration-tool-from-orchestrations-to-logic-apps/ba-p/4494876)
