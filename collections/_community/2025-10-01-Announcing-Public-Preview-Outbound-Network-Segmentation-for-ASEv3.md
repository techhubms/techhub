---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-public-preview-asev3-outbound-network-segmentation/ba-p/4458398
title: 'Announcing Public Preview: Outbound Network Segmentation for ASEv3'
author: jordanselig
feed_name: Microsoft Tech Community
date: 2025-10-01 16:12:36 +00:00
tags:
- App Service Environment V3
- ARM Template
- ASEv3
- Azure App Service
- Azure CLI
- Azure Networking
- Bicep Template
- Compliance
- Egress Control
- Firewall
- NAT Gateway
- Network Security Groups
- Outbound Network Segmentation
- Subnet
- Virtual Network Integration
section_names:
- azure
- security
primary_section: azure
---
jordanselig explains how Outbound Network Segmentation for App Service Environment v3 (ASEv3) enables Azure app developers and admins to manage outbound traffic routing, providing improved security and compliance controls.<!--excerpt_end-->

# Announcing Public Preview: Outbound Network Segmentation for ASEv3

Outbound Network Segmentation for App Service Environment v3 (ASEv3) is now available in public preview. This feature gives developers and administrators new capabilities to control how outbound traffic leaves their Azure App Service apps, making it easier to meet security, compliance, and auditing requirements in complex or regulated environments.

## What Is Outbound Network Segmentation?

Previously, all outbound traffic from an ASEv3 app appeared to originate from the entire subnet hosting the environment. This made it challenging to apply restrictions or selectively route certain app traffic.

**Outbound Network Segmentation** introduces these key features:

- Define which subnet outbound traffic from each app uses
- Assign dedicated outbound IPs to each app via NAT Gateways
- Route app traffic through custom firewalls or appliances
- Apply Network Security Groups (NSGs) for precision restrictions
- Achieve better auditability and regulatory compliance

This now allows networking and security teams to:

- Group app outbound traffic as desired
- Block or allow traffic at the app or subnet level
- Isolate sensitive app-to-database connections (e.g., only App A can access Database A by assigning it to a specific subnet)

## Getting Started with Public Preview

Outbound Network Segmentation is available across all Azure public regions through these steps:

1. **Create a new App Service Environment** (feature not available for existing ASEs)
2. Enable the following cluster setting at creation using an ARM or Bicep template:

   ```json
   "clusterSettings": [ { "name": "MultipleSubnetJoinEnabled", "value": "true" } ]
   ```

   See [Custom configuration settings for App Service Environments](https://learn.microsoft.com/azure/app-service/environment/app-service-app-service-environment-custom-settings).

3. Once enabled, you can join apps to alternate subnets at any time.
   - Use the Azure CLI:

     ```shell
     az webapp vnet-integration add --resource-group <APP-RESOURCE-GROUP> --name <APP-NAME> --vnet <VNET-NAME> --subnet <ALTERNATE-SUBNET-NAME>
     ```

   - The alternate subnet must be empty and delegated to Microsoft.web/serverfarms
   - Application traffic routing must be enabled (see [configuration guide](https://learn.microsoft.com/en-us/azure/app-service/configure-vnet-integration-routing#configure-application-routing))
   - To specify a subnet from a different resource group, use `-h` for help

**Note:** This feature is only configurable through templates or CLI as portal support is not available currently.

## Technical Details

- For ASEv3, each app in a given plan can join 1 alternate subnet
- Up to 4 connections per plan are supported (multi-tenant App Service supports up to 2)
- Incompatible with multi-plan subnet join
- Subnets used must be empty and properly delegated
- Changing or removing subnet joins can be done at any time via the same process

## Why Use Outbound Network Segmentation?

ASEv3 is designed for workloads needing high isolation, scalability, and control. Outbound segmentation enables:

- Segregation of traffic for compliance (e.g., regulated data)
- App-level outbound routing and egress controls
- Integration with custom security appliances
- Simplified audit trails for outbound connections

## Learn More

- [App Service Environment v3 networking overview](https://learn.microsoft.com/azure/app-service/environment/networking)
- [Custom configuration guide for ASEv3](https://learn.microsoft.com/azure/app-service/environment/app-service-app-service-environment-custom-settings)

For questions or feedback, reach out in the comments section of the original post.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-public-preview-asev3-outbound-network-segmentation/ba-p/4458398)
