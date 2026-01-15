---
layout: post
title: 'Microsoft BizTalk Server Product Lifecycle Update: Modernization and Migration Guidance'
author: hcamposu
canonical_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/microsoft-biztalk-server-product-lifecycle-update/ba-p/4478559
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-12-17 15:02:12 +00:00
permalink: /azure/community/Microsoft-BizTalk-Server-Product-Lifecycle-Update-Modernization-and-Migration-Guidance
tags:
- API Management
- ARM Templates
- Azure
- Azure Integration Services
- Azure Logic Apps
- Azure Monitor
- B2B Messaging
- Bicep
- BizTalk Server
- Community
- DevOps
- DevOps Tooling
- Enterprise Integration
- Host Integration Server
- Hybrid Integration
- IaC
- Integration Modernization
- Mainframe Connectivity
- Migration Guidance
- OpenTelemetry
- Service Bus
- VS Code
section_names:
- azure
- devops
---
hcamposu presents an update on the BizTalk Server product lifecycle, providing guidance on long-term support, migration to Azure Logic Apps, and roadmap considerations for enterprises managing mission-critical integration workloads.<!--excerpt_end-->

# Microsoft BizTalk Server Product Lifecycle Update: Modernization and Migration Guidance

For 25+ years, Microsoft BizTalk Server has powered critical enterprise integration strategies, enabling business process automation, B2B messaging, and industry-spanning connectivity. With this update, Microsoft confirms:

**BizTalk Server 2020 will be the last version, with mainstream support until April 11, 2028, and extended support until April 9, 2030.**

## Lifecycle Support Timeline

| Lifecycle Phase     | End Date         | Support Details                                                                |
|--------------------|------------------|-------------------------------------------------------------------------------|
| Mainstream Support | April 11, 2028   | Security + non-security updates; Customer Service & Support (CSS)              |
| Extended Support   | April 9, 2030    | CSS support; Security updates; Paid non-security hotfixes                      |
| End of Support     | April 10, 2030   | No further updates or support                                                  |

- Paid extended support for BizTalk Server 2020 is available April 2028–April 2030 for non-security hotfixes.
- BizTalk Server 2016 is out of mainstream support; direct modernization to Azure Logic Apps is recommended.

## Modernization: Azure Logic Apps as Successor

Azure Logic Apps (Azure Integration Services suite) now stands as the recommended modernization path, offering:

- 1400+ out-of-box connectors (enterprise/SaaS/legacy/mainframe)
- Artifact/code migration for maps, schemas, rules, and B2B/EDI flows
- Elastic scalability, compliance, cost efficiency, and managed infrastructure
- DevOps tooling, Visual Studio Code integration, Infrastructure-as-Code (ARM/Bicep)
- Native integration with Azure Monitor and OpenTelemetry
- Hybrid deployment options (Arc-enabled Kubernetes, air-gapped scenarios)
- AI-driven workflows through Logic Apps' agentic business process support

## Host Integration Server and Mainframe Support

Host Integration Server (HIS) 2028 will be released as a standalone product with its own support cycle, providing mainframe/midrange connectivity. Logic Apps connectors for mainframe integration are available, and Microsoft seeks feedback on feature needs ([feature request link](https://aka.ms/logicappsneeds)).

## Modernization Resources and Guidance

- Migration overview: [https://aka.ms/btmig](https://aka.ms/btmig)
- Best practices: [https://aka.ms/BizTalkServerMigrationResources](https://aka.ms/BizTalkServerMigrationResources)
- Video series: [https://aka.ms/btmigvideo](https://aka.ms/btmigvideo)
- Logic Apps needs/feature survey: [https://aka.ms/logicappsneeds](https://aka.ms/logicappsneeds)
- Modernization hub: [Migration overview](https://learn.microsoft.com/azure/logic-apps/biztalk-server-migration-overview)

Microsoft provides:

- Proven migration and design pattern guidance
- Artifact reuse and modernization tooling ecosystem
- Unified Support for migration projects
- Partner ecosystem specializing in BizTalk modernization
- Phased migration options
- Potential migration incentives (updates forthcoming)

## FAQs (Summary)

- **No Immediate Migration Required:** BizTalk Server 2020 is supported through April 2028 (mainstream), and paid extended support is available through April 2030.
- **No New Versions:** BizTalk Server 2020 is the last release.
- **Reuse of BizTalk Artifacts:** Logic Apps can use most maps, schemas, rules, and custom code; partner tooling available.
- **Hybrid and AI:** Azure Logic Apps support hybrid and AI-driven workflows (including regulatory and operational flexibility).
- **HIS Future:** HIS 2028 continues as a standalone mainframe integration solution.

## Getting Started

Microsoft recommends engaging your Microsoft account team early, exploring available resources, and planning your transition with provided migration guides and tooling support.

---

For more guidance, see: [Azure Logic Apps – BizTalk Migration](https://learn.microsoft.com/azure/logic-apps/biztalk-server-migration-overview)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/microsoft-biztalk-server-product-lifecycle-update/ba-p/4478559)
