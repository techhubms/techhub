---
title: 'Public Preview: Ephemeral OS Disk with full caching for VM/VMSS'
tags:
- ARM Templates
- Azure
- Azure Compute
- Azure REST API
- Azure Virtual Machines
- Cache Disk
- Community
- Diffdisksettings
- Enablefullcaching
- Ephemeral OS Disk
- Full OS Caching
- General Purpose VM SKUs
- IO Sensitive Workloads
- Local Storage
- Low Latency
- Microsoft.Compute
- NVMe Disk
- OS Disk Performance
- Public Preview
- ReadOnly Caching
- Region Availability
- Remote Storage Disruption
- Resource Disk
- StandardSSD LRS
- Stateless Workloads
- VM Scale Sets
feed_name: Microsoft Tech Community
author: viveksingla
date: 2026-03-30 19:37:31 +00:00
external_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/public-preview-ephemeral-os-disk-with-full-caching-for-vm-vmss/ba-p/4500191
section_names:
- azure
primary_section: azure
---

viveksingla announces a public preview Azure Compute feature: Ephemeral OS disk with full caching for VMs and VM Scale Sets, which caches the full OS image onto local storage to improve OS disk latency and resilience, plus shows how to enable it via ARM templates or the REST API.<!--excerpt_end-->

# Public Preview: Ephemeral OS Disk with full caching for VM/VMSS

Microsoft has announced a **public preview** for **Ephemeral OS disk with full caching**. The feature targets **IO-sensitive stateless workloads** by caching the **entire OS disk image** on **local storage**, reducing dependency on remote storage.

## Key advantages

- **High performance**: consistently fast OS disk response times
- **Reliability / availability**: improved resilience for critical workloads

## Why full OS caching?

With current ephemeral OS disks:

- **OS writes** are stored locally
- **OS reads** still depend on a **remote base OS image**

With **full caching** enabled:

- the **entire OS disk image** is cached on **local storage**
- once caching completes, **all OS disk IO** is served locally

Expected results:

- **Consistently fast OS disk performance** (low-millisecond latency)
- **Improved resilience** during remote storage disruptions
- **No impact to VM create times** (caching happens asynchronously after boot)

## Workloads that benefit

Examples called out in the announcement:

- AI workloads
- Quorum-based databases
- Data analytics and real-time processing systems
- Large-scale stateless services on general purpose VM families

## How it works

When full OS caching is enabled:

- VM **local storage** (cache disk, resource disk, or NVMe disk) hosts the full OS disk
- local storage capacity is reduced by **2× the OS disk size** to accommodate OS caching
- the OS disk is cached in the background after VM boot
- all OS disk IOs happen on local storage, aiming for **10× better IO performance** and better resiliency to storage interruptions

## Public preview availability

During public preview, the feature is available for **most general purpose VM SKUs**, excluding **2-vCPU and 4-vCPU VMs**, in these **29 regions**:

- AustraliaCentral
- AustraliaCentral2
- AustraliaSouthEast
- BrazilSoutheast
- CanadaCentral
- CanadaEast
- CentralIndia
- CentralUSEUAP
- EastAsia
- GermanyWestCentral
- JapanEast
- JioIndiaCentral
- JioIndiaWest
- KoreaCentral
- KoreaSouth
- MalaysiaSouth
- MexicoCentral
- NorthEurope
- NorwayWest
- QatarCentral
- SouthAfricaNorth
- SwedenCentral
- SwitzerlandWest
- TaiwanNorth
- UAECentral
- UKSouth
- UKWest
- WestCentralUS
- WestIndia

Microsoft notes they are continuing to expand **region support** and **tooling** on the way to general availability.

## Getting started (ARM template / REST API)

You can enable Ephemeral OS disk with full caching when creating **new VMs** or **new VMSS** by updating **ARM templates or REST API definitions** and setting `enableFullCaching` for ephemeral OS disks.

### ARM template snippet (VM)

```json
"resources": [
  {
    "name": "[parameters('virtualMachineName')]",
    "type": "Microsoft.Compute/virtualMachines",
    "apiVersion": "2025-04-01",
    "osDisk": {
      "diffDiskSettings": {
        "option": "Local",
        "placement": "ResourceDisk",
        "enableFullCaching": true
      },
      "caching": "ReadOnly",
      "createOption": "FromImage",
      "managedDisk": {
        "storageAccountType": "StandardSSD_LRS"
      }
    }
  }
]
```

### ARM template snippet (VMSS)

```json
"resources": [
  {
    "name": "[parameters('vmssName')]",
    "type": "Microsoft.Compute/virtualMachineScaleSets",
    "apiVersion": "2025-04-01",
    "osDisk": {
      "diffDiskSettings": {
        "option": "Local",
        "placement": "ResourceDisk",
        "enableFullCaching": true
      },
      "caching": "ReadOnly",
      "createOption": "FromImage",
      "managedDisk": {
        "storageAccountType": "StandardSSD_LRS"
      }
    }
  }
]
```

## Notes

- Updated: **Mar 30, 2026**
- Version: **1.0**


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-compute-blog/public-preview-ephemeral-os-disk-with-full-caching-for-vm-vmss/ba-p/4500191)

