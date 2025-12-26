---
layout: "post"
title: "Challenges with Azure Resource Naming Convention Maintenance in Bicep and Infrastructure as Code"
description: "This community discussion, led by d2peak, dives into the issues developers face when trying to maintain consistent Azure resource naming conventions using Bicep and the Azure Developer CLI. While Microsoft provides some tools and guidelines, many resources remain out of date, leading to fragmented practices. Contributors explore various workarounds, from custom modules in Bicep and Terraform to manual lookups, and debate the effectiveness of Azure Policy, user-defined functions, and third-party tools. The conversation highlights the importance of maintainable and automatable naming strategies for large-scale Azure projects."
author: "d2peak"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/AZURE/comments/1mjcixd/azure_resource_naming_conventions_not_maintained/"
viewing_mode: "external"
feed_name: "Reddit Azure"
feed_url: "https://www.reddit.com/r/azure/.rss"
date: 2025-08-06 18:27:00 +00:00
permalink: "/community/2025-08-06-Challenges-with-Azure-Resource-Naming-Convention-Maintenance-in-Bicep-and-Infrastructure-as-Code.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["Automation", "AVM Naming Module", "Azd", "Azure", "Azure Developer CLI", "Azure Naming Tool", "Azure Policy", "Azure Resource Naming", "Best Practices", "Bicep", "Coding", "Community", "DevOps", "IaC", "Module Development", "OpenAPI Spec", "Resource Abbreviations", "Resource Management", "Terraform", "User Defined Functions"]
tags_normalized: ["automation", "avm naming module", "azd", "azure", "azure developer cli", "azure naming tool", "azure policy", "azure resource naming", "best practices", "bicep", "coding", "community", "devops", "iac", "module development", "openapi spec", "resource abbreviations", "resource management", "terraform", "user defined functions"]
---

d2peak and the community discuss the challenges of maintaining Azure resource naming conventions in Bicep and azd workflows, weighing Microsoft's outdated tools against custom solutions and automation.<!--excerpt_end-->

# Challenges with Azure Resource Naming Convention Maintenance in Bicep and Infrastructure as Code

**Author:** d2peak

## Overview

Developers using Bicep and the Azure Developer CLI (azd) often run into problems with keeping Azure resource naming conventions consistent and up-to-date. d2peak raises key concerns about Microsoft’s current tooling and guidance, and asks how the community approaches these issues.

## Key Pain Points

- **Outdated Abbreviation Lists:**
  - The abbreviation JSON in the [azd-starter-bicep repo](https://github.com/Azure-Samples/azd-starter-bicep/blob/main/infra/abbreviations.json) and the [Azure Naming Tool](https://github.com/mspnp/AzureNamingTool/blob/main/src/repository/resourcetypes.json) haven’t been updated for over a year, lacking recently added resources.
  - The most up-to-date abbreviations are only available as HTML tables on [Microsoft Learn](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-abbreviations), with no downloadable machine-readable format.

- **Difficult Automation:**
  - Without a maintained JSON or similar file, keeping naming consistent across codebases becomes manual and difficult to automate.
  - Scraping, as a workaround, is frustrating and unsustainable.

- **Uncertainty in Source of Truth:**
  - The [Azure REST API specs](https://github.com/azure/azure-rest-api-specs) are referenced as the origin for naming restrictions, but contributors note that official abbreviations for resource types (as seen in Microsoft docs) are lacking within these specs.

## Community Solutions and Workarounds

- **Custom Bicep Modules:**
  - Many contributors, including d2peak, use custom modules or helper functions to generate consistent resource names, such as `resourceNames.storageAccount(org, env, location, discriminator)`.
  - [Example custom functions in Bicep](https://github.com/bigwhitesolutions/azure-naming/blob/AddFunctionsSupport/dist/naming.functions.bicep) are shared for community adoption.

- **Manual Reference:**
  - Developers often keep their own internal copy of resource abbreviations and update them as needed, sometimes by copying from Microsoft Docs manually.

- **Terraform AVM Naming Module:**
  - Several users recommend [AVM naming modules for Terraform](https://github.com/Azure/terraform-azurerm-avm-resources) as more up-to-date, but some prefer not to rely on third-party modules for critical naming logic.

- **Azure Policy Recommendation:**
  - Naming restrictions can also be enforced with [Azure Policy](https://learn.microsoft.com/en-us/azure/governance/policy/), including custom policies deployed via Bicep.
  - However, policies only enforce, not generate, names.

- **Concerns About Breaking Changes:**
  - Some speculate Microsoft may avoid frequent updates or changes to abbreviations to prevent breaking changes, since many resources cannot be renamed without recreation.

## Discussion Highlights

- The lack of a maintained, downloadable abbreviation file is broadly seen as a limitation for automation and large-scale deployments.
- Contributors express a preference for maintaining their own modules/functions rather than relying on unmaintained third-party resources.
- Azure Policy is useful for enforcement, but not for generating consistent names.
- The community would welcome an official, machine-readable, continually updated data source for resource abbreviations and naming rules.

## References

- [AzD Bicep Starter Abbreviations](https://github.com/Azure-Samples/azd-starter-bicep/blob/main/infra/abbreviations.json)
- [Azure Naming Tool Resource Types](https://github.com/mspnp/AzureNamingTool/blob/main/src/repository/resourcetypes.json)
- [Microsoft Learn - Resource Abbreviations](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-abbreviations)
- [Azure REST API Specs](https://github.com/azure/azure-rest-api-specs)
- [Custom Bicep Naming Functions Example](https://github.com/bigwhitesolutions/azure-naming/blob/AddFunctionsSupport/dist/naming.functions.bicep)

## Takeaways

- Maintaining up-to-date and automated Azure resource naming is an ongoing challenge with the current tooling provided by Microsoft.
- Developers often rely on their own scripts and modules, or reference outdated files, leading to fragmentation.
- There is a clear need for a machine-readable, officially maintained source of naming standards to support automation.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mjcixd/azure_resource_naming_conventions_not_maintained/)
