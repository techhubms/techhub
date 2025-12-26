---
layout: "post"
title: "Azure VNet Flow Logs Migration and Traffic Analytics with Terraform"
description: "This in-depth guide by Ibrahim Baig details the migration from Azure NSG Flow Logs to VNet Flow Logs using Terraform, covering the key dates, motivations, operational and security benefits, cost considerations, Traffic Analytics integration, challenges, and best practice implementation strategies. It includes code samples for Terraform, Azure CLI, and insights for large-scale deployments. Readers will also learn utilization of Azure Log Analytics and validation techniques for ensuring a smooth migration in line with Microsoft's timeline for NSG Flow Log retirement."
author: "ibrahimbaig"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/azure-vnet-flow-logs-with-terraform-the-complete-migration-and/ba-p/4468225"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-08 08:29:35 +00:00
permalink: "/community/2025-11-08-Azure-VNet-Flow-Logs-Migration-and-Traffic-Analytics-with-Terraform.html"
categories: ["Azure", "Coding", "DevOps", "Security"]
tags: ["AVNM", "Azure", "Azure CLI", "Azure Policy", "Azure Security", "Azure Storage", "Azure VNet Flow Logs", "Coding", "Community", "Cost Optimization", "DevOps", "IaC", "KQL", "Log Analytics", "Network Monitoring", "Network Watcher", "NSG Flow Logs", "Resource Management", "Security", "Terraform", "Traffic Analytics"]
tags_normalized: ["avnm", "azure", "azure cli", "azure policy", "azure security", "azure storage", "azure vnet flow logs", "coding", "community", "cost optimization", "devops", "iac", "kql", "log analytics", "network monitoring", "network watcher", "nsg flow logs", "resource management", "security", "terraform", "traffic analytics"]
---

Ibrahim Baig provides a technical guide on migrating from Azure NSG Flow Logs to VNet Flow Logs using Terraform, discussing benefits, migration steps, implementation details, Traffic Analytics integration, and practical validation.<!--excerpt_end-->

# Azure VNet Flow Logs with Terraform: The Complete Migration and Traffic Analytics Guide

**Author: Ibrahim Baig (Consultant)**

## Executive Summary

Microsoft is retiring NSG flow logs and recommends migrating to VNet flow logs before September 30, 2027. This guide describes why and how to migrate, leveraging Terraform and Azure-native tools for implementation, with a deep dive into Traffic Analytics and operational best practices.

---

## 1. Key Dates & Migration Rationale

- **June 30, 2025**: Creation of new NSG flow logs disabled.
- **September 30, 2027**: NSG flow logs fully retired (read-only retention per policy).
- Microsoft provides scripts and Azure Policy strategies for migration.

### Why migrate?

- **Broader visibility:** Enable logging at VNet, subnet, or NIC scope—unbound from NSGs.
- **Security & Analytics:** Integrates natively with Azure Traffic Analytics for richer insights and security monitoring (e.g., AVNM rules).
- **Operational continuity:** Pricing parity with NSG flow logs, plus 5 GB/month free tier.

---

## 2. What's New in VNet Flow Logs

- **Scopes:** Logging at VNet, subnet, or NIC granularity.
- **Data Storage:** JSON logs stored in Azure Storage accounts.
- **Automation:** At-scale enablement and auditing via Azure Policy.
- **Analytics Add-on:** Traffic Analytics brings visualization, threat detection, segmentation validation, performance monitoring, and customizable dashboards. Integrates with Azure Monitor and Log Analytics for alerting and automation.

> For advanced recipes and practical use cases, see: [VNet Flow Logs Recipes Blog](https://blog.cloudtrooper.net/2024/05/08/vnet-flow-logs-recipes/)

---

## 3. Infrastructure as Code: Terraform Migration Example

> The Terraform provider `azurerm_network_watcher_flow_log` now supports VNet and subnet/NIC targets via `target_resource_id`. Ensure you're using provider v3.110.0 or later.

```hcl
terraform {
  required_version = ">= 1.5"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 3.110.0"
    }
  }
}

provider "azurerm" {
  features {}
}

data "azurerm_network_watcher" "this" {
  name                = "NetworkWatcher_${var.region}"
  resource_group_name = "NetworkWatcherRG"
}

resource "azurerm_network_watcher_flow_log" "vnet_flow_log" {
  name                = "${var.vnet_name}-flowlog"
  network_watcher_name = data.azurerm_network_watcher.this.name
  resource_group_name  = data.azurerm_network_watcher.this.resource_group_name
  target_resource_id   = azurerm_virtual_network.vnet.id
  storage_account_id   = azurerm_storage_account.flowlogs_sa.id
  enabled              = true

  retention_policy {
    enabled = true
    days    = 30
  }

  traffic_analytics {
    enabled              = true
    workspace_id         = azurerm_log_analytics_workspace.law.workspace_id
    workspace_region     = azurerm_log_analytics_workspace.law.location
    workspace_resource_id = azurerm_log_analytics_workspace.law.id
    interval_in_minutes  = 60
  }

  tags = {
    owner      = "network-platform"
    environment = var.env
  }
}
```

**Notes:**

- Use a dedicated storage account for flow logs; lifecycle policies may be overwritten otherwise.
- Update provider to latest for full subnet/NIC ID support.

---

## 4. Alternative Enablement Options

### Azure CLI

```bash
az network watcher flow-log create \
  --location westus \
  --resource-group MyResourceGroup \
  --name myVNetFlowLog \
  --vnet MyVNetName \
  --storage-account mystorageaccount \
  --workspace "/subscriptions/<subId>/resourceGroups/<rg>/providers/Microsoft.OperationalInsights/workspaces/<LAWName>" \
  --traffic-analytics true \
  --interval 60
```

### Azure Portal

- Go to **Network Watcher > Flow logs > +Create**
- Select Flow log type = Virtual network, specify target/resource, storage account, enable Traffic Analytics as required.

### At Scale via Azure Policy

- Use built-in DeployIfNotExists policies to auto-deploy and audit VNet flow logs organization-wide.

---

## 5. Migration Approach (NSG → VNet Flow Logs)

1. Inventory all existing NSG flow logs.
2. Select a migration method: Microsoft-provided script or Azure Policy.
3. Run NSG and VNet flow logs in parallel for validation.
4. Disable and decommission NSG flow logs pre-retirement.

---

## 6. Challenges & Mitigations

- **Permissions:** Ensure Log Analytics workspace role assignments.
- **Terraform State:** Use isolated storage accounts for logs.
- **Tooling Compatibility:** Verify integration with SIEM/NDR solutions.
- **Provider/API maturity:** Stay current with `azurerm` releases.

---

## 7. Validation Checklist

- Storage: Validate new blobs in the configured storage account.
- Traffic Analytics: Confirm data is populated in the Log Analytics workspace.
- AVNM: Review centrally managed security rule logs for allowed/denied actions.

---

## 8. Cost Considerations

- **Flow log storage:** First 5 GB/month free; then {{CONTENT}}.50/GB.
- **Traffic Analytics processing:** $2.30/GB (60-min interval), $3.50/GB (10-min).

---

## 9. Traffic Analytics Deep Dive

### Data Enrichment & Dashboards

- Enrich logs with public IP, region, geolocation, VM details, source/destination NIC, subnet, load balancer, and encryption states.
- Store in tables: `NTAIPDetails`, `NTATopologyDetails`, `NTANetAnalytics`.
- Example KQL for recent flows:

```kql
NTANetAnalytics
| where TimeGenerated > ago(10d)
| where SrcIp == "10.10.1.4" and strlen(DestIp)>0
| summarize TotalBytes=sum(BytesDestToSrc+BytesSrcToDest) by SrcIp, DestIp
```

- Use KQL queries within your Log Analytics workspace for custom insight extraction.

---

## 10. References & Further Reading

- [NSG Flow Logs Overview](https://learn.microsoft.com/en-us/azure/network-watcher/nsg-flow-logs-overview)
- [Migrate to VNet Flow Logs](https://learn.microsoft.com/en-us/azure/network-watcher/nsg-flow-logs-migrate)
- [VNet Flow Logs Overview](https://learn.microsoft.com/en-us/azure/network-watcher/vnet-flow-logs-overview)
- [Terraform Flow Log Resource](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/network_watcher_flow_log)
- [VNet Flow Logs Recipes Blog](https://blog.cloudtrooper.net/2024/05/08/vnet-flow-logs-recipes/)

---

_Last updated: Nov 08, 2025_

---

*For any updates or further guidance, refer to Microsoft's official docs and follow the Azure Infrastructure Blog.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/azure-vnet-flow-logs-with-terraform-the-complete-migration-and/ba-p/4468225)
