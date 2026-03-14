---
external_url: https://techcommunity.microsoft.com/t5/azure-networking/azure-vm-persistent-route-setup/m-p/4502007#M773
title: Troubleshooting Persistent Routing from Azure VM to On-Premises via Site-to-Site VPN
author: nitrox2000
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-03-13 14:33:07 +00:00
tags:
- Azure
- Azure Networking
- Azure VM
- Cisco ASA
- Community
- Firewall Rules
- Hub And Spoke Topology
- IPsec VPN
- Local Network Gateway
- Network Connectivity
- On Premises Integration
- Persistent Routing
- Routing Troubleshooting
- Site To Site VPN
- Static Routes
- Subnets
- Virtual Machines
section_names:
- azure
---
nitrox2000 requests troubleshooting assistance for persistent routing from an Azure VM to a third-party network via their on-premises Cisco ASA firewall, after successfully migrating from an on-prem VM setup.<!--excerpt_end-->

# Troubleshooting Persistent Routing from Azure VM to On-Premises via Site-to-Site VPN

**Author:** nitrox2000

## Scenario Overview

- On-prem VM: `10.100.10.23/24` connects to a third-party network through a dedicated Cisco ASA firewall.
- VM uses persistent routes for specific destinations, routing via the FW's inside interface (`10.100.10.190`).
- The third-party network is reachable from on-prem by way of static and persistent routes.

## New Requirement: Migration to Azure

- Azure VM: `10.150.1.10/24` in an isolated subnet in Production subscription (Hub & Spoke layout).
- On-prem and Azure are connected by Site-to-Site VPN using Azure VPN Gateway (with on-premises networks included in Local Network Gateway definitions).

## Routing and Connectivity Issue

- Persistent routes added to Azure VM as on-prem:
  - `10.10.227.10/32` via `10.100.10.190`
  - `20.10.227.10/32` via `10.100.10.190`
  - `10.110.255.136/30` via `10.100.10.190`
- The Azure VM can ping the FW inside interface but **cannot reach** the target subnets (e.g., `10.10.227.10`, `20.10.227.10`).

## Troubleshooting Advice and Next Steps

- **Validate Routing:**
  - Ensure that Azure route tables (UDRs) and effective routes for the VM subnet allow traffic destined for `10.100.10.0/24` and beyond to traverse through the VPN Gateway.
  - Confirm that the route to `10.100.10.190` (the FW inside interface) is valid from Azure and passes over the VPN tunnel.
- **VPN Gateway and On-Prem Configuration:**
  - Double-check that the on-prem ASA is set to allow traffic sourced from the Azure VM subnet (`10.150.1.0/24`) and not just the original on-prem subnet.
  - Ensure all security rules, NAT, and security policies are duplicated for the new Azure source subnet.
- **Persistent Route Limitations in Azure:**
    - Windows and Linux Azure VMs can have static persistent routes, but Azure SDN may override or not honor these if UDRs and NSGs contradict them.
    - Use Azure route tables (User Defined Routes) instead of relying solely on guest OS routes.
- **Third-Party Network Considerations:**
  - Confirm that return traffic from third-party systems is correctly routed back through the ASA to the Azure VM's subnet via the site-to-site VPN.
  - Verify that the third-party firewall/routing policies permit traffic to/from the Azure address space.
- **Diagnostics:**
  - Use `tracert` or similar tools to observe the path taken.
  - Inspect the Azure Network Watcher effective routes and packet capture.

## Next Steps

- Work with network and firewall teams to validate end-to-end routing for the Azure subnet.
- Consider implementing UDRs at the Azure subnet to direct traffic for third-party networks towards the VPN Gateway.
- Confirm that all security and NAT rules in the ASA explicitly allow new Azure VM source IPs.

## Additional Resources

- [Microsoft: Configure route tables to direct traffic in Azure](https://docs.microsoft.com/en-us/azure/virtual-network/virtual-networks-udr-overview)
- [Azure Network Watcher: Diagnose VPN connectivity](https://docs.microsoft.com/en-us/azure/network-watcher/network-watcher-diagnose-vm-network-traffic)

---

If you have made all route updates and duplicated firewall rules for the Azure subnet but still have issues, it is likely a route propagation or firewall/NAT configuration detail specific to the Azure address block and VPN gateway association.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking/azure-vm-persistent-route-setup/m-p/4502007#M773)
