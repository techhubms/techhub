---
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/unlock-outbound-traffic-insights-with-azure-standardv2-nat/ba-p/4493138
title: Unlock Outbound Traffic Insights with Azure StandardV2 NAT Gateway Flow Logs
author: cozhang
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-06 16:07:33 +00:00
tags:
- Azure
- Azure NAT Gateway
- Azure Networking
- Community
- Compliance
- Connection Troubleshooting
- Diagnostic Settings
- Flow Logs
- IPv6 Support
- Log Analytics
- Outbound Connectivity
- Packet Capture
- Public IP
- Security
- SNAT
- StandardV2 NAT Gateway
- Virtual Machines
- Zone Redundancy
section_names:
- azure
- security
---
cozhang demonstrates how to leverage flow logs in Azure's StandardV2 NAT Gateway to audit, secure, and troubleshoot outbound internet connectivity at scale.<!--excerpt_end-->

# Unlock Outbound Traffic Insights with Azure StandardV2 NAT Gateway Flow Logs

Azure's StandardV2 NAT Gateway introduces a new level of outbound connectivity management, with significant upgrades including zone-redundancy, enhanced throughput, dual-stack (IPv4/IPv6) support, and—most importantly—built-in flow logging. This post explains how to activate flow logs for your NAT Gateway and leverage them for improved security, compliance, and analytics.

## Key Advantages of StandardV2 NAT Gateway

- **Zone-redundancy:** Maintains outbound traffic during single-zone failures
- **Performance:** Up to 100 Gbps throughput, 10 million packets/second
- **Dual-stack:** Support for up to 16 IPv6 and 16 IPv4 public IPs
- **Flow logs:** Historical connection records for auditing and troubleshooting

## What Are Flow Logs?

Flow logs are enabled on your NAT gateway through Diagnostic settings. Logged data can be sent to Log Analytics, a storage account, or Event Hub. The `NatGatewayFlowlogV1` log category provides IP-level records of traffic through your gateway, including:

- Source IP (usually your VM)
- NAT Gateway outbound IP
- Destination IP
- Packets/bytes sent and dropped

## Security and Compliance Benefits

With flow logs, you gain visibility into:

- Which destinations your VMs access
- Unusual or unauthorized outbound patterns
- Evidence to meet compliance requirements

## Usage Analytics

Flow logs make it possible to:

- Identify high-volume sources or destinations
- Analyze workload traffic patterns
- Investigate throughput spikes or bottlenecks

*Note: Flow logs only record successfully established connections. Connections dropped by NSGs, UDRs, or SNAT exhaustion won't appear in the logs.*

## Troubleshooting Workflow Example

### Scenario

Your VMs, routed through a StandardV2 NAT Gateway, intermittently can't reach github.com.

1. **Check Gateway Health:**
   - Datapath availability > 90% means NAT is healthy. Otherwise, see [Azure NAT Gateway troubleshooting](https://learn.microsoft.com/en-us/azure/nat-gateway/troubleshoot-nat-connectivity).
2. **Enable Flow Logs:**
   - Set up `NatGatewayFlowlogV1` category in Diagnostics. Use Log Analytics for easy querying. See [this guide](https://learn.microsoft.com/en-us/azure/nat-gateway/monitor-nat-gateway-flow-logs).
3. **Confirm Connection Establishment:**
   - Query flows between VM private IP and github.com's IP(s). If no records, investigate SNAT/NSG/UDR issues.
4. **Check for Dropped Packets:**
   - Investigate `PacketsSentDropped` or `PacketsReceivedDropped` columns.
   - Mitigate by using multiple connections and scaling the number of outbound public IPs.

## Tips & Best Practices

- Enable flow logs proactively for historical data when troubleshooting.
- Distribute outbound traffic across multiple public IPs to prevent rate limiting.
- Use queries in Log Analytics to spot patterns and irregularities.
- Combine with SNAT metrics for deeper root cause analysis.

## Learn More

- [StandardV2 NAT Gateway flow logs overview](https://learn.microsoft.com/en-us/azure/nat-gateway/nat-overview#standardv2-nat-gateway)
- [Diagnostic settings guide](https://learn.microsoft.com/en-us/azure/nat-gateway/nat-gateway-flow-logs)
- [Pricing](https://azure.microsoft.com/en-us/pricing/details/azure-nat-gateway/)
- [Troubleshooting with flow logs](https://learn.microsoft.com/en-us/azure/nat-gateway/monitor-nat-gateway-flow-logs)

For feedback or questions, visit [Azure Community Feedback](https://feedback.azure.com/d365community/forum/8ae9bf04-8326-ec11-b6e6-000d3a4f0789).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/unlock-outbound-traffic-insights-with-azure-standardv2-nat/ba-p/4493138)
