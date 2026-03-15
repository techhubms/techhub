---
external_url: https://techcommunity.microsoft.com/t5/finops-blog/introducing-non-breaking-breaking-changes-in-finops-hubs-12/ba-p/4438554
title: 'FinOps hubs 12: Non-Breaking Schema Versioning and FOCUS 1.2 Alignment'
author: Michael_Flanakin
feed_name: Microsoft Tech Community
date: 2025-08-05 23:27:17 +00:00
tags:
- Azure Advisor
- Azure Cost Management
- Azure Data Explorer
- Azure Hybrid Benefit
- Cloud Cost Intelligence
- Commitment Discount
- Cost Management Datasets
- Cost Optimization
- Cost Reporting
- Custom Columns
- Data Integration
- Data Model
- Data Schema Versioning
- Dataset Transformation
- FinOps Hubs
- FOCUS 1.2
- Microsoft
- Microsoft Fabric
- Power BI
- Real Datatype
- Azure
- ML
- Community
section_names:
- azure
- ml
primary_section: ml
---
Michael Flanakin shares a deep dive into FinOps hubs 12, highlighting new approaches to schema versioning and alignment with FOCUS 1.2 for seamless Azure cost management and reporting.<!--excerpt_end-->

# FinOps hubs 12: Non-Breaking Schema Versioning and FOCUS 1.2 Alignment

**Author:** Michael Flanakin

FinOps hubs 12 introduces smart, non-breaking schema management that allows organizations to benefit from new data columns and reporting features without disrupting existing reports, dashboards, or integration points. This approach empowers data teams to upgrade cost intelligence capabilities at their own pace, leveraging modern cost management standards.

## Key Themes

- **Non-breaking 'breaking' changes:** FinOps hubs 12 realizes a data platform architecture where schema evolution does not create automatic disruptions. Versioned functions and datasets (e.g., `Costs_v1_0` and `Costs_v1_2`) let users choose the features and fields needed, with legacy support maintained.
- **Azure Data Explorer & Microsoft Fabric Integration:** All ingested data is converted to a FOCUS-aligned dataset, using either Azure Data Explorer or Microsoft Fabric. The transformation brings consistency and future-proofing for organizational cost analytics.
- **FOCUS 1.2 Alignment:** The `Costs_v1_2` schema introduces new fields to enable analysis of commitment discounts, Azure Hybrid Benefit, and further cost optimization scenarios. It covers every change from preview through general availability.
- **Backwards Compatibility:** Existing reports or integrations using versioned functions continue unaffected. Only those using unversioned functions (e.g., `Costs`) may see changed data, which can be fixed by referencing a versioned schema.

## Working with Older Exports

FinOps hubs transparently upgrades all supported dataset formats (e.g., FOCUS 1.0-preview(v1), 1.0, 1.0r2, and 1.2-preview) to the latest schema. Historical reporting stays functional, while new features are accessible instantly—all without reprocessing stored exports.

## Schema Enhancements in v1_2

### Main Updates in Cost Datasets

- Added fields: CapacityReservationId, CapacityReservationStatus, CommitmentDiscountQuantity, CommitmentDiscountUnit, ServiceSubcategory, SkuPriceDetails (aligned to FOCUS 1.2)
- Renamed fields: x_InvoiceId → InvoiceId, x_PricingCurrency → PricingCurrency, x_SkuMeterName → SkuMeter, x_SkuTier now populated across all versions
- Extended fields: Alibaba and Tencent Cloud columns added for expanded multi-cloud support
- New Cost Management attributes: x_AmortizationClass, x_CommitmentDiscountNormalizedRatio, x_ServiceModel, x_SkuPlanName, x_SourceValues (tracks data modifications)

### Power BI and Reporting Improvements

- Columns for negotiated/commitment/total discount percentages and savings
- Support for Azure Hybrid Benefit analytics (SkuLicenseQuantity, SkuLicenseStatus, etc.)
- SKU property columns and consumed core hours for enhanced resource analysis

## FOCUS 1.2 Changes in Recommendations and Other Datasets

- **Recommendations:** Ready for future Azure Advisor integration with new (currently unpopulated) resource and recommendation fields
- **Data Type Standardization:** Floating-point columns (prices, costs) move from decimal to real type in v1_2 for improved performance

## Next Steps for Data Teams

- Assess current report/integration reliance on unversioned vs. versioned dataset functions
- Upgrade to FOCUS 1.2 datasets for expanded analytics when ready, with strict non-breaking assurances for v1_0
- Review the [FinOps hub data model](https://learn.microsoft.com/cloud-computing/finops/toolkit/hubs/data-model) for full technical reference

Organizations benefit from a mature data platform that lets them adopt new industry cost reporting standards with minimal operational impact and ongoing access to historical and multi-cloud data. Strategic services are available for further customization and scalability.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/finops-blog/introducing-non-breaking-breaking-changes-in-finops-hubs-12/ba-p/4438554)
