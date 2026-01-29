---
external_url: https://techcommunity.microsoft.com/t5/azure-tools-blog/release-of-bicep-azure-verified-modules-for-platform-landing/ba-p/4487932
title: Azure Verified Modules for Platform Landing Zone with Bicep Now Generally Available
author: ztrocinski
feed_name: Microsoft Tech Community
date: 2026-01-21 04:10:31 +00:00
tags:
- ALZ Accelerator
- ALZ Bicep
- Automation
- Azure Landing Zones
- Azure Policy
- Azure Resource Manager
- Azure Verified Modules
- Bicep
- Deployment Stacks
- Enterprise Governance
- IaC
- Logging
- Management Groups
- Migration Guide
- Module Architecture
- Networking
- Parameter Files
- Platform Landing Zone
- Azure
- Coding
- DevOps
- Security
- Community
section_names:
- azure
- coding
- devops
- security
primary_section: coding
---
ztrocinski announces the general availability of Azure Verified Modules for Platform Landing Zone with Bicep, detailing its architecture, developer experience improvements, and migration plans for classic ALZ-Bicep users.<!--excerpt_end-->

# Azure Verified Modules for Platform Landing Zone with Bicep Now Generally Available

**Author: ztrocinski**  
*Published: January 20, 2026*  
*Version: 1.0*

After extensive collaboration and feedback from the community, Azure Verified Modules (AVM) for Platform Landing Zone using Bicep has reached general availability. This release marks a significant upgrade for organizations architecting and deploying secure, scalable Azure environments with modern Infrastructure as Code (IaC) workflows.

## Why Use Azure Verified Modules?

- **Unified IaC Approach:** AVM consolidates fragmented IaC efforts across Microsoft into one standard, ensuring high-quality, rigorously tested modules with official support.
- **Modularity:** The AVM Bicep starter includes 19 independently versioned modules (16 resource, 3 pattern) for composable architectures.
- **Flexibility:** Every deployment component, from management group hierarchies to individual resource names, is fully customizable.

Explore the module catalog: [Azure Verified Modules for Bicep](https://azure.github.io/Azure-Verified-Modules/indexes/bicep/)

### Module Architecture Overview

#### Core - Governance (Management Groups)

- Contains modules for building out management group hierarchies: root, platform, connectivity, identity, management, security, landing zones, and more.
- Each comes as a [pattern module](https://azure.github.io/Azure-Verified-Modules/indexes/bicep/bicep-ptn-alz-empty/).

#### Core - Logging

- Includes modules for resource groups, Log Analytics workspaces, and Azure Monitoring Agent.

#### Networking (Hub & Spoke, Virtual WAN)

- Comprehensive resource modules for VNETs, Azure Firewall, Bastion, VPN/ExpressRoute Gateways, DDoS Protection, DNS resolvers, and more.

### Deployment Stacks Integration

- **Lifecycle Management:** Integrates with [Azure Deployment Stacks](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/deployment-stacks?tabs=azure-powershell) for tracking resource states and enabling safer, automated cleanup.
- **Benefits:** No manual cleanup, consistent deployments, safe deletions, and clear deployment histories.

### Modern Parameter Files

- Switches from JSON to [`.bicepparam` files](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/parameter-files?tabs=Bicep) for improved DX:
  - IntelliSense/autocomplete in VS Code
  - Native support for variables, comments, and Bicep functions

### Deployment with the ALZ Accelerator

- The [ALZ IaC Accelerator](https://azure.github.io/Azure-Landing-Zones/accelerator/) provides the fastest path to production-ready landing zones using the new AVM starter module.
- Flexible configuration via the new `platform-landing-zone.yaml` file:
  - Define management group structures and naming
  - Choose network architectures: hub & spoke, vWAN, or none
  - Deploy to any Azure region set

#### Example YAML Configuration

```yaml
starter_locations: ["eastus", "westus2"]
management_group_id_prefix: "contoso"
management_group_int_root_id: "alz"
management_group_int_root_name: "Contoso Landing Zones"
resource_group_logging_name_prefix: "rg-alz-logging"
resource_group_hub_networking_name_prefix: "rg-alz-conn"
resource_group_dns_name_prefix: "rg-alz-dns"
network_type: "hubNetworking" # or vwanConnectivity, none
```

### Independent Policy Management with ALZ Library

- The ALZ Bicep AVM implementation uses the [Azure Landing Zones Library](https://azure.github.io/Azure-Landing-Zones-Library/), decoupling policy data from deployment code and enabling:
  - Easier module upgrades without policy conflicts
  - Flexible policy refreshes
  - Built-in support for custom, organization-specific policies
  - Clean separation and dedicated properties for policy customization

Learn more: [Modifying ALZ-Bicep Policy Assets](https://azure.github.io/Azure-Landing-Zones/bicep/howtos/modifyingpolicyassets/)

### Deprecation of Classic ALZ-Bicep Modules

- **Timeline:**
  - *Now:* AVM Bicep becomes the default in ALZ Accelerator
  - *Feb 16, 2025:* Classic Bicep starter removed from Accelerator
  - *Feb 16, 2026:* Classic ALZ-Bicep repository archived
- **Support:** Bug fixes, patches, and policy refreshes for 12 months post-removal
- **Migration:** A comprehensive guide and tooling are in development to transition users from classic ALZ-Bicep to AVM Bicep.

## Key Benefits Recap

- End-to-end, deeply customizable deployments
- Modular and independently maintained components
- Improved lifecycle and drift management with deployment stacks
- Modern DX with .bicepparam parameter files
- Simplified policy management and upgrades

## Additional Resources

- [Azure Verified Modules Documentation](https://azure.github.io/Azure-Verified-Modules/)
- [Azure Landing Zones Accelerator](https://azure.github.io/Azure-Landing-Zones/accelerator/)
- [Azure Landing Zones Library](https://azure.github.io/Azure-Landing-Zones-Library/)

*Share your feedback or questions in the comments to help guide future features!*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-tools-blog/release-of-bicep-azure-verified-modules-for-platform-landing/ba-p/4487932)
