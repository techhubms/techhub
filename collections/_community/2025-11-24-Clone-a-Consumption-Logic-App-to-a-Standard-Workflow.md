---
layout: post
title: Clone a Consumption Logic App to a Standard Workflow
author: WSilveira
canonical_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/clone-a-consumption-logic-app-to-a-standard-workflow/ba-p/4471175
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-24 16:00:00 +00:00
permalink: /azure/community/Clone-a-Consumption-Logic-App-to-a-Standard-Workflow
tags:
- Azure Functions
- Azure Logic Apps
- Azure Portal
- Connectors
- Consumption Logic Apps
- Enterprise Integration
- Governance
- Integration
- Logic Apps Standard
- Migration
- Modernization
- Stateful Workflows
- VS Code Integration
- Workflow Cloning
section_names:
- azure
---
WSilveira explains how to clone Azure Consumption Logic Apps into Standard workflows, offering tips for migration, modernization, and key limitations for Microsoft Azure users.<!--excerpt_end-->

# Clone a Consumption Logic App to a Standard Workflow

Azure Logic Apps now supports cloning existing Consumption Logic Apps into Standard workflows. This new capability streamlines the migration process, letting organizations reuse existing workflows while taking advantage of the Standard model's improved performance and advanced features.

## Why Does It Matter?

- **Easier Modernization**: Moving from Consumption to Standard is a common need for teams seeking more performance and features.
- **Performance**: Standard workflows use a single-tenant architecture, delivering better performance.
- **Developer Experience**: Enhanced with local development options and integration with Visual Studio Code.
- **Advanced Features**: Standard offers stateful/stateless workflows, built-in connectors, and direct integration with Azure Functions.

## Key Highlights

- **Direct Cloning**: Easily convert an existing Consumption Logic App to a Standard workflow with minimal manual steps.
- **Workflow Preservation**: Triggers, actions, and configurations from your original Logic App are carried into the new workflow.
- **Modernization Support**: Standard workflows unlock support for custom connectors, private endpoints, and updated governance features.
- **Agent Loop Support**: Recent updates like Agent Loop in Logic Apps Consumption, combined with cloning, enable agentic workflow graduation paths.

## Steps to Get Started

1. Go to your Consumption Logic App in the Azure portal.
2. Choose the **Clone to Standard** option.
3. Select your target Logic App Standard environment and adjust settings as needed.
4. Validate and deploy the new workflow.

## Limitations

- **Infrastructure settings are not cloned** (e.g., no integration account configurations copied).
- **Manual steps required**: Connections and credentials must be reconfigured.
- **Secure parameters**: Placeholders are created; you must manually update from Azure Key Vault.
- **Connectors**: Default to shared even if a built-in version exists in Standard.
- **Unsupported Features**:
  - Integration account references
  - XML/Flat file transformations
  - EDIFACT and X12 actions
  - Nested workflows
  - Azure Function calls

## Additional Resources

- [Official Microsoft Documentation on Logic App Cloning](https://aka.ms/clonetostandard/docs)
- [Agent Loop Support Announcement](https://techcommunity.microsoft.com/blog/integrationsonazureblog/announcing-public-preview-of-agent-loop-in-azure-logic-apps-consumption/4471056)

*Updated November 23, 2025*

---

*Author: WSilveira*

For more community updates on Azure Integration Services, follow the [Azure Integration Services Blog](https://techcommunity.microsoft.com/category/azure/blog/integrationsonazureblog).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/clone-a-consumption-logic-app-to-a-standard-workflow/ba-p/4471175)
