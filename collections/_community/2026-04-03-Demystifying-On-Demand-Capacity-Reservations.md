---
author: KenHooverMSFT
section_names:
- azure
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/demystifying-on-demand-capacity-reservations/ba-p/4504806
title: Demystifying On-Demand Capacity Reservations
primary_section: azure
tags:
- ARM
- Availability Zones
- Azure
- Azure CLI
- Azure Migrate
- Azure Quotas
- Azure Resource Manager
- Azure Savings Plan
- Azure Site Recovery
- Azure Virtual Machines
- Bicep
- Capacity Reservation Group
- Community
- CRG
- High Availability
- IaC
- ODCR
- On Demand Capacity Reservations
- Overallocation
- PowerShell
- Regional VM
- Reserved Instances
- Resiliency
- Terraform
- VM Capacity
- VM Startup SLA
- Zonal VM
feed_name: Microsoft Tech Community
date: 2026-04-03 11:48:50 +00:00
---

KenHooverMSFT explains how Azure On-Demand Capacity Reservations (ODCRs) work for Azure Virtual Machines, why VMs can fail to start during capacity pressure, what ODCRs cost, and a practical approach for applying reservations to existing (already running) VMs.<!--excerpt_end-->

# Demystifying On-Demand Capacity Reservations

Azure’s 80+ regions are provisioned with hardware to run the platform and its services. In practice, demand for a particular VM offering can exceed a region’s available infrastructure. When that happens, Azure may return an error indicating **insufficient capacity** to create or start the service.

This article explores **Azure On-Demand Capacity Reservations (ODCRs)**—an Azure resource designed to improve the odds that important VMs can start when you need them.

## About On-Demand Capacity Reservations

### A “parking garage” metaphor for VM capacity

Azure offers many VM sizes across CPU generations, vendors, and architectures. Each Azure region contains datacenters with pools of hardware that run VMs. As customers start/stop VMs, available capacity changes constantly—driven by time-of-day patterns and longer demand cycles (holidays, school calendars, etc.).

When you start an Azure VM, **Azure Resource Manager (ARM)** (the control plane that manages Azure resources) needs to find hardware capacity in the target region that can host your requested VM size.

- If ARM finds capacity, the VM starts normally.
- If not, you can get a **capacity-related start failure**.

The metaphor: a VM start is like trying to park a car in a garage. The garage is built for typical demand, but special events can fill it up.

If a critical VM is stopped for maintenance/config changes and then cannot restart due to capacity, that can cause serious outages.

### What an ODCR is

An **On-Demand Capacity Reservation (ODCR)** reserves capacity for a specific VM size in a specific scope (region + zone, or region-wide).

Example scope: **D16s_v6** in **Canada Central**, **availability zone 2**.

Once reserved:

- A matching VM can be associated with the reservation.
- The VM effectively “owns” that reserved slot.
- The VM gets **priority** over other same-size VMs without a reservation when it needs to start.

### More detail about VM startup (SLA, quota, discounts)

Key points:

- Azure does **not** provide an explicit VM startup SLA for VMs **without** an ODCR. Startup is effectively **best effort**.
- **Quota headroom** does not guarantee a VM can start. Quota is permission to create cores of a VM family, not proof of real-time capacity.
- **Reserved Instances** and **Savings Plans** do not increase the chance of startup; they’re discount mechanisms.

**Assigning an ODCR applies a formal startup SLA** for that VM. SLA language is in Microsoft’s **Service Level Agreements for Online Services** document:

- Service Level Agreements for Online Services: https://www.microsoft.com/licensing/docs/view/service-level-agreements-sla-for-online-services

## Cost implications of ODCRs

Billing notes:

1. The compute cost for the reserved “parking space” is the **same** as a running VM of the same size (no “double billing” just for having an ODCR).
2. ODCR billing starts immediately if the reservation quantity is **greater than zero**.
3. Stopping a VM does **not** reduce ODCR cost, because the reservation continues holding the slot.
4. If you have Reserved Instances / Savings Plans covering the scope, the VM can be billed at the discounted rate.

### When ODCRs can result in paying more

Two scenarios described where you can end up paying for two reservations for what is effectively the same workload instance:

- **Azure Site Recovery (ASR)**: if the primary VM has an ODCR and the replica is also associated with a capacity reservation, you pay for both.
- **Azure Migrate replication**: similar logic if the replica is reserved and the source is also in Azure with its own ODCR.

## When you should (and shouldn’t) use ODCRs

Recommended use: protect VMs that must be available **24x7**, especially those that provide essential services:

- AD domain controllers
- Application servers
- Database servers
- VM-based appliances (firewalls, load balancers, infrastructure support)

Example incident: a firewall VM was stopped for config adjustment, then failed to restart due to capacity, causing significant connectivity impact.

Also noted:

- Microsoft resiliency assessments often check for missing capacity reservations and may flag them as a high-risk finding.
- Not all VM stops are voluntary: rare involuntary shutdowns can occur due to predictive hardware failures or other events.

Not ideal: highly elastic environments where VMs scale in/out frequently. Reserving capacity for the whole pool can mean paying for capacity even when instances aren’t running. A suggested approach is reserving only the minimum number of instances required for degraded-but-available service.

## Creating On-Demand Capacity Reservations

### ODCR components

ODCRs are organized as:

- **Capacity Reservation Group (CRG)**: a “bucket” for reservations. You provide:
  - Name
  - Region
  - Availability zones it can access
- **Capacity Reservation** (inside a CRG): requires:
  - Reservation name (often includes size/zone for clarity; example: `Zone1_D16s_v5`)
  - VM size (example: `D16s_v5`)
  - Availability zone (or regional/zoneless)
  - Number of reserved instances (“parking spaces”)

Creation methods:

- Azure portal
- PowerShell
- Azure CLI
- IaC (Bicep, Terraform)

CRGs can be shared across subscriptions:

- https://learn.microsoft.com/azure/virtual-machines/capacity-reservation-group-share

### Why ODCR creation can fail

Possible reasons include:

- No open hypervisor slots at request time (capacity pressure, outages, demand)
- Insufficient quota to claim required cores for the reservation
- VM size not available in the target region/AZ (hardware differs by region)
- Subscription/zone/region restriction blocks creation

### What to do if ODCR creation fails

Suggested mitigations:

1. Try creating outside business hours (regional demand is often lower).
2. Use a different VM type, AZ, or region.
3. Use automation to retry periodically; it may take an unknown amount of time.

Support tickets can help identify non-capacity causes, but if it’s truly a capacity squeeze, support typically cannot “create space”.

### Protecting a VM with an ODCR

Portal approach:

1. Open the VM
2. Go to **Configuration**
3. Find **Capacity reservations**
4. Select **Capacity reservation group**
5. Choose the appropriate CRG and click **Apply**

IaC approach:

- Link the VM to the CRG by specifying the CRG **resource ID** in the VM definition.

### Impact of associating a VM with an ODCR

- VM stopped: change takes effect immediately.
- VM running, **regional/zoneless**: must **stop and restart** for ODCR protection to apply.
- VM running, **zonal**: change is immediate and non-disruptive.

## Working with ODCRs (three behaviors to know)

### Associated vs allocated (and overallocation)

- A CRG has a reservation **capacity** expressed as **allocated instances**.
- VMs linked to the CRG are **associated**.
- You can associate more VMs than allocated capacity (**overallocation**).
  - In an overallocated CRG, VMs are protected on a first-come-first-served basis based on start order.

### Behavior #1: Add a running VM to a CRG

- Zonal VM: immediate.
- Regional VM: stop/start required.

This differs from **Availability Sets**, where a VM must be placed into the set at creation time:

- Availability Sets: https://learn.microsoft.com/azure/virtual-machines/availability-set-overview

### Behavior #2: Create a reservation with capacity = 0

You can create a reservation with **zero allocated instances**. This should succeed because it requires no capacity allocation—just metadata.

### Behavior #3: Increase reservation capacity to cover running VMs

If associated VMs exceed allocated capacity, you can increase reservation capacity to match the number of **running** VMs.

Reason: running VMs already occupy a hypervisor slot, so Azure can link the reservation to that existing allocation.

### A practical “apply ODCRs to existing VMs” workflow

For many existing workloads, you can protect running VMs with high probability (and no disruption for zonal VMs) by:

1. Create a CRG + reservation for the right region/AZ/size with **quantity 0**.
2. Associate the VMs to the CRG (temporarily overallocated).
3. Increase allocated instances to match the number of **running** VMs.

## Final advice

If capacity is challenging in a region/VM size, the author recommends:

- **Bring VMs online first, then apply a capacity reservation to them.**

Creating reservations for additional capacity can fail if Azure can’t find free slots; it’s often easier to associate already-running VMs and then increase reservation capacity to cover them.

## References

- On-Demand Capacity Reservations Overview: https://learn.microsoft.com/en-us/azure/virtual-machines/capacity-reservation-overview
- ODCR restrictions list (changes frequently): https://learn.microsoft.com/azure/virtual-machines/capacity-reservation-overview
- Consolidated SLA for Online Services (.docx): https://aka.ms/CapacityReservationSLAForVM
- Overallocating capacity reservations: https://learn.microsoft.com/en-us/azure/virtual-machines/capacity-reservation-overallocate
- Bicep/Terraform/ARM template info for Capacity Reservation Groups: https://learn.microsoft.com/azure/templates/microsoft.compute/capacityreservationgroups


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/demystifying-on-demand-capacity-reservations/ba-p/4504806)

