---
external_url: https://blog.fabric.microsoft.com/en-US/blog/assessing-your-azure-data-factory-for-migration-to-fabric-data-factory/
title: Assessing Your Azure Data Factory for Migration to Fabric Data Factory
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-12-10 16:30:00 +00:00
tags:
- Azure Data Factory
- Cloud Data Platform
- Data Compatibility
- Data Engineering
- Data Integration
- Data Pipelines
- Data Workflow
- ETL
- Fabric Data Factory
- Microsoft Azure
- Microsoft Fabric
- Migration Assessment
- Migration Tools
- Pipeline Migration
- PowerShell
section_names:
- azure
- ml
---
Microsoft Fabric Blog explains how to assess and prepare your Azure Data Factory pipelines for migration to Fabric Data Factory, highlighting built-in assessment tools and best migration practices.<!--excerpt_end-->

# Assessing Your Azure Data Factory for Migration to Fabric Data Factory

Organizations modernizing their data integration workflows face important decisions when transitioning from Azure Data Factory (ADF) to Fabric Data Factory. Proper assessment before migration ensures compatibility and smoother operations.

## Why Perform a Migration Assessment?

Migration isn't a straightforward resource transfer—it entails evaluating compatibility and minimizing risk. The built-in assessment tool in ADF helps you:

- Detect pipelines and activities ready for migration
- Identify those requiring modification or not yet supported in Fabric
- Export detailed reports for planning and progress tracking

## How to Use the Assessment Tool

1. Open your Azure Data Factory instance in the Azure portal.
2. In the authoring canvas toolbar, select 'Start assessment (preview)'.
   ![Start assessment screenshot](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/12/word-image-31991-1.png)
3. Review the listed pipelines and their respective activities.
4. Expand pipelines for more granular activity summaries.
   ![Assessment summary screenshot](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/12/screenshot-showing-assessment-summary-on-the-side-1.png)
5. Export results as a CSV to aid detailed tracking and planning.
   ![Drilldown export screenshot](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/12/screenshot-showing-the-drilldownn-of-assessment-al-1.png)

## Understanding Assessment Results

Each pipeline and activity will be assigned one of these statuses:

- **Ready**: Fully migratable
- **Needs review**: Requires changes (such as parameter adjustments)
- **Coming soon**: Support is being developed; migrate in the future
- **Not compatible**: Needs refactoring; no equivalent in Fabric

A pipeline’s overall status is set to its most restrictive activity. For example, a “Not compatible” activity means the whole pipeline is marked “Not compatible” for readiness.

## Migration Next Steps

While a fully native migration UX is in development, you can:

- Use the [PowerShell upgrade tool](https://learn.microsoft.com/fabric/data-factory/migrate-pipelines-powershell-upgrade-module-for-azure-data-factory-to-fabric) for early migrations
- Plan fixes for anything marked “Needs review” or “Not compatible”
- Explore the [Migration planning guide](https://learn.microsoft.com/fabric/data-factory/migrate-from-azure-data-factory) and [best practices](https://learn.microsoft.com/fabric/data-factory/migration-best-practices)
- Monitor for expanded capability as Fabric Data Factory evolves

## Further Resources

- [How to assess your Azure Data Factory to Fabric Data Factory Migration](https://learn.microsoft.com/azure/data-factory/how-to-assess-your-azure-data-factory-to-fabric-data-factory-migration)
- [Data Factory documentation in Microsoft Fabric](https://learn.microsoft.com/fabric/data-factory/)
- [Connector capability comparison between Azure Data Factory and Data Factory in Fabric](https://learn.microsoft.com/fabric/data-factory/connector-parity)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/assessing-your-azure-data-factory-for-migration-to-fabric-data-factory/)
