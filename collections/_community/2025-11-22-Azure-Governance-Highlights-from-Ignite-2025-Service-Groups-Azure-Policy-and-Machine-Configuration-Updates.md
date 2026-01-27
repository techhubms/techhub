---
external_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/azure-governance-ignite-2025/ba-p/4471112
title: 'Azure Governance Highlights from Ignite 2025: Service Groups, Azure Policy, and Machine Configuration Updates'
author: jodiboone
feed_name: Microsoft Tech Community
date: 2025-11-22 01:24:16 +00:00
tags:
- Azure Governance
- Azure Monitoring
- Azure Policy
- Azure Resiliency
- CIS Compliance
- Cloud Compliance
- Custom Baselines
- Extensibility Framework
- Identity Based Exemptions
- Linux Baselines
- Machine Configuration
- Policy Lifecycle
- Public Preview
- Remediations
- Resource Management
- Role Based Access Control
- Service Groups
- Tech Community
- UX Improvements
- Windows Baselines
section_names:
- azure
- security
primary_section: azure
---
jodiboone reviews the key Azure governance updates from Ignite 2025, covering new features in Service Groups, Azure Policy enhancements, and machine configuration baselines.<!--excerpt_end-->

# Azure Governance @ Ignite 2025: Key Announcements and New Features

Azure Governance is back at Microsoft Ignite 2025, delivering significant updates that empower organizations to manage and secure their cloud environments effectively. This blog by jodiboone covers the core sessions, product launches, and actionable integrations.

## Service Groups: Dynamic Resource Management

Service Groups, now available in Public Preview, offer a more flexible approach to managing resource hierarchies in Azure:

- **Low Privilege Management:** Operate with minimized permissions so users can manage resources without excessive access. Membership in a Service Group does not automatically include policy inheritance or role-based access control.
- **Flexible Hierarchies:** Azure resources and scopes—across your tenant—can belong to multiple Service Groups and be nested. This supports organizational models based on Cost Center, Product, or Business Unit, among others.

**New Integrations Announced:**

- **Azure Monitoring**: Enhanced observability for service groups.
- **Azure Resiliency**: Boosted resilience practices for mission-critical resources.

Get started: [aka.ms//servicegroups](https://aka.ms/servicegroups)

## Azure Policy: Feature Expansions for Better Compliance and Usability

Azure Policy updates this year focus on language enrichment and user experience improvements:

- **Identity Based Exemptions:** Now in public preview, this lets admins target exemptions for specific service principals rather than entire scopes, minimizing unnecessary permission grants while maintaining critical workflows.
- **Policy UX Home Page:** A refreshed experience provides real-time compliance views, policy status, and easier onboarding to new features like exemptions and remediations.

Try out the new UX and share feedback directly with the team.

## Machine Configuration: Customizable Baselines for Compliance

Microsoft is extending its server configuration guidance:

- **Custom Baselines:** The new extensibility framework enables deployment of tailored baselines for Windows and Linux VMs, aligning configurations with regulatory or business needs.
- **CIS Alignment:** Baseline content now aligns with CIS standards for supported distributions.

Select the desired baseline, adjust settings to fit requirements, and deploy efficiently via Azure Policy and Machine Configuration.

Learn more: [aka.ms//machinebaselines](https://aka.ms/machinebaselines)

## Best Practices

- Review new Service Groups integration with monitoring and resiliency tools for granular governance.
- Fine-tune policy exemptions with Identity Based Exemptions to streamline approval flows.
- Use customizable machine baselines to stay compliant with regulatory standards such as CIS.

## About the Author

[jodiboone](https://techcommunity.microsoft.com/t5/s/gxcuf89792/images/dS0xNDA5Mjc2LTM3Nzk2NmlBQzQzQ0ZFODRGMTUxOUZE?image-dimensions=50x50) is a member of Microsoft Tech Community, offering guidance and technical leadership on Azure governance and management.

Stay tuned for additional Ignite highlights and ongoing improvements in Azure governance.

_Last updated November 22, 2025 — Version 1.0_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/azure-governance-ignite-2025/ba-p/4471112)
