---
layout: "post"
title: "ExpressRoute Gateway Backend Migration: Basic to Standard Public IP SKU"
description: "This technical guide explains the process for migrating Azure ExpressRoute gateways from Basic SKU public IPs to Microsoft managed Standard SKU public IPs. It details the purpose of the migration, each step in the automated backend workflow, customer controls, and how Azure maintains connectivity and minimizes disruption throughout the transition."
author: "MekaylaMoore"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-networking-blog/expressroute-gateway-microsoft-initiated-migration/ba-p/4497689"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-03 00:58:14 +00:00
permalink: "/2026-03-03-ExpressRoute-Gateway-Backend-Migration-Basic-to-Standard-Public-IP-SKU.html"
categories: ["Azure"]
tags: ["Automation", "Availability", "Azure", "Azure Networking", "Azure Portal", "Basic SKU", "Community", "ExpressRoute", "Gateway Migration", "Infrastructure Upgrade", "Migration Tool", "Public IP SKU", "Resource Group", "Service Continuity", "Standard SKU", "Virtual Network Gateway"]
tags_normalized: ["automation", "availability", "azure", "azure networking", "azure portal", "basic sku", "community", "expressroute", "gateway migration", "infrastructure upgrade", "migration tool", "public ip sku", "resource group", "service continuity", "standard sku", "virtual network gateway"]
---

MekaylaMoore outlines the end-to-end process of migrating Azure ExpressRoute gateways from Basic to Standard SKU public IPs, highlighting best practices, stages involved, and how Azure minimizes downtime and maintains service quality for customers.<!--excerpt_end-->

# ExpressRoute Gateway Backend Migration: Basic to Standard Public IP SKU

## Objective

The backend migration process upgrades Azure ExpressRoute gateways from Basic SKU public IPs to Microsoft managed Standard SKU public IPs. This change enhances gateway reliability and availability while ensuring continuity for existing services.

Microsoft automates the migration, but customers can opt to perform the upgrade themselves using the ExpressRoute Gateway Migration Tool in the Azure portal or PowerShell. All customers will receive notification ahead of scheduled maintenance windows, and the process allows for input regarding preferred scheduling.

> **Note:** Basic SKU public IPs will be retired as of September 30, 2025. All gateways must migrate to Standard SKU before this date. See the [official announcement](https://azure.microsoft.com/updates/upgrade-to-standard-sku-public-ip-addresses-in-azure-by-30-september-2025-basic-sku-will-be-retired/) for more details.

## Migration Process Overview

The backend migration is largely hands-off for customers and is scheduled during a maintenance window to minimize disruption. The high-level process includes:

1. **Deploying a New Gateway:** Azure provisions a second virtual network gateway in the same GatewaySubnet, with a new Standard SKU public IP address assigned by Microsoft.
2. **Transferring Configurations:** All existing gateway configurations (connections, settings, and routes) are copied to the new gateway. Both gateways operate in parallel during the transition to reduce downtime, though brief connectivity interruptions may occur.
3. **Cleanup:** Upon successful validation, Azure removes the old gateway and associated connections. The new gateway is tagged as `CreatedBy: GatewayMigrationByService` for identification.

During migration, customers should avoid making non-essential changes to gateways or connected circuits. Changes made during the transition may require aborting or re-committing to the migration.

## Detailed Migration Stages

The Azure backend migration process for ExpressRoute gateways occurs in four main stages:

### 1. Validate

- Azure confirms that the gateway and connected resources meet prerequisites for migration. No new resources are created at this stage.
- If requirements aren't met, migration does not proceed to prevent disruption.

### 2. Prepare

- A new virtual network gateway with Standard IP SKU is deployed alongside the existing gateway in the same region.
- The system appends `_migrate` to the name of the new gateway.
- The current gateway is locked during this stage, but customers retain the option to abort, which deletes the new resources.

### 3. Migrate

- After preparation, Azure switches traffic from the old gateway to the new one.
- The cutover may cause brief (typically under one minute) connectivity interruptions.
- Both gateways and all migrated connections are visible in the resource group during this phase.

### 4. Commit or Abort

- After migration, Azure monitors gateway connectivity for 15 days. During this time, resource modification is restricted.
- Customers can choose to commit (finalize migration and unlock the resources) or abort (roll back to the old gateway and remove new resources) before the 15-day period ends.

## Azure Portal Experience

- Customers will see informative banners in the ExpressRoute gateway's Overview blade detailing the status of the migration.
- Resource deployment and activity can be tracked in the Azure resource group (e.g., RGA) and the Activity Log, where events initiated by `GatewayRP` indicate backend operations.
- The newly created resources (like `ERGW-A_migrate`, `Conn-A_migrate`, and `LAconn_migrate`) are visible and tagged for traceability.
- After successful commit, the old gateway and its connections are deleted, leaving the migrated gateway as the sole resource.

## Best Practices and Recommendations

- Schedule migration windows to minimize user impact.
- Monitor notifications in the Azure portal for status updates and required actions.
- Use the provided migration tool for more control, or allow Microsoft to manage the migration automatically.
- Avoid configuration changes to the gateway during any migration stage unless necessary.
- Validate connectivity post-migration and commit as soon as satisfied.

## Frequently Asked Questions

**Q: How long before Microsoft commits the migration if I don't act?**
A: Microsoft will wait approximately 15 days post-migration before finalizing, giving users time to validate services.

**Q: Will there be packet loss or disruption?**
A: The migration is designed for seamless cutover with no expected packet loss; however, brief interruptions (usually under 1 minute) can occur during traffic switchover.

**Q: Can I modify my ExpressRoute deployment during migration?**
A: Non-critical modifications should be avoided. If changes are urgent, abort or complete the migration first.

---

**References:**

- [Upgrade guidance for Basic to Standard SKU](https://learn.microsoft.com/en-us/azure/virtual-network/ip-services/public-ip-basic-upgrade-guidance#steps-to-complete-the-upgrade)
- [ExpressRoute Gateway Migration Tool](https://learn.microsoft.com/en-us/azure/expressroute/gateway-migration)

---

_Last updated March 2, 2026. Version 1.0._

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/expressroute-gateway-microsoft-initiated-migration/ba-p/4497689)
