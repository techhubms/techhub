---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/building-environmental-aware-api-platforms-with-azure-api/ba-p/4458308
title: 'Environmental Sustainability Features in Azure API Management: Minimizing API Infrastructure Carbon Impact'
author: tomkerkhovemicrosoft
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-10-13 10:00:00 +00:00
tags:
- AI Infrastructure
- API Infrastructure
- API Policy
- Azure API Management
- Azure Carbon Intensity
- Carbon Optimization
- Compliance
- Environmental Sustainability
- GenAI
- Green IT
- Load Balancing
- Microsoft Azure
- Scope 2 Emissions
- Sustainability Reporting
- Sustainable Workloads
section_names:
- ai
- azure
---
Tom Kerkhove introduces public preview features in Azure API Management aimed at reducing the carbon footprint of API and AI workloads. The article guides readers through configuring carbon-aware traffic shifting, building sustainability-aligned policies, and leveraging new tools for compliance and sustainable cloud practices.<!--excerpt_end-->

# Environmental Sustainability Features in Azure API Management

*By Tom Kerkhove*

Microsoft continues to advance environmental sustainability with the public preview of new features in Azure API Management designed to minimize the carbon impact of managing APIs in the cloud.

## Microsoft’s Sustainability Commitment

- Microsoft aims to be carbon negative by 2030 and completely remove its historic emissions by 2050 ([read about Microsoft’s sustainability goals](https://blogs.microsoft.com/blog/2020/01/16/microsoft-will-be-carbon-negative-by-2030/)).
- Tools such as [Azure Carbon Optimization](https://learn.microsoft.com/en-us/azure/carbon-optimization/overview) and [Microsoft Sustainability Manager](https://www.microsoft.com/sustainability/microsoft-sustainability-manager) assist customers in measuring, reporting, and optimizing cloud carbon emissions — crucial for compliance with directives like the EU's CSRD.

## What’s New in Azure API Management?

Azure API Management now offers:

- **Carbon-aware load balancing:** API traffic can be automatically routed to backend regions with lower carbon intensity based on real-time data.
- **Region attribution:** Specify the Azure region where each backend is running to make load balancing decisions that factor in sustainability.
- **Emission category configuration:** Backends can be given preferred carbon emission levels, allowing routing only to APIs that meet sustainability goals.

### Example: Carbon-aware Backend Configuration

```json
{
  "type": "Microsoft.ApiManagement/service/backends",
  "apiVersion": "2024-10-01-preview",
  "name": "sustainable-backend",
  "properties": {
    "url": "https://mybackend.example.com",
    "protocol": "http",
    "azureRegion": "westeurope"
    // ... other properties
  }
}
```

You may also specify preferred emission categories in load balancer settings, excluding higher-intensity backends if sustainable options are available.

## Building Carbon-Intelligent Policies

You can now access real-time carbon intensity metrics using the property `context.Deployment.SustainabilityInfo.CurrentCarbonIntensity` in API Management policy definitions. This enables:

- Dynamic adjustment of caching, telemetry, and rate limiting policies depending on current carbon intensity.
- Example policy (pseudo-logic):

```xml
<when condition="@(context.Deployment.SustainabilityInfo.CurrentCarbonIntensity >= CarbonIntensityCategory.High)">
  <trace source="Orders API" severity="verbose">
    <message>Lead Created</message>
  </trace>
</when>
<otherwise>
  <trace source="Orders API" severity="information">
    <message>Lead Created</message>
  </trace>
</otherwise>
```

This approach empowers developers to optimize backend efficiency and API policies for sustainability in real-time.

## Sustainable AI Integration

AI workloads are increasingly energy-intensive. Microsoft's initiatives ([see details](https://blogs.microsoft.com/blog/2024/04/02/sustainable-by-design-advancing-the-sustainability-of-ai/)) and the new API platform features help organizations:

- Reduce the carbon impact of AI services by incorporating sustainability-aware routing and policy configurations,
- Use the [GenAI release channel](https://learn.microsoft.com/azure/api-management/configure-service-update-settings) to enable and test advanced sustainability capabilities for AI-powered APIs.

## Availability and Getting Started

- These features are available in classic Azure API Management tiers and the GenAI release channel; more tiers will be supported in the future.
- To participate in the preview, [sign up here](https://aka.ms/apim/sustainability/preview/join).

## Further Resources

- [Environmentally sustainable APIs in Azure API Management documentation](https://aka.ms/apim/sustainability/docs)
- [Building sustainable workloads on Microsoft Azure](https://learn.microsoft.com/azure/well-architected/sustainability/sustainability-get-started)
- [Measuring carbon emissions and optimizations in Microsoft Azure](https://learn.microsoft.com/en-us/azure/carbon-optimization/overview)
- [Microsoft for sustainability](https://www.microsoft.com/sustainability/cloud)

---

*Updated: Oct 01, 2025 – Version 1.0*

Tom Kerkhove invites feedback and questions to help accelerate sustainability for your AI and API workloads.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/building-environmental-aware-api-platforms-with-azure-api/ba-p/4458308)
