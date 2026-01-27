---
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/introducing-ebpf-host-routing-high-performance-ai-networking/ba-p/4468216
title: 'High-Performance AI Networking on AKS: eBPF Host Routing with Azure CNI Powered by Cilium'
author: Sam_Foo
feed_name: Microsoft Tech Community
date: 2025-11-08 00:45:49 +00:00
tags:
- Advanced Container Networking Services
- AKS
- Azure CNI
- BpfVeth
- Cilium
- Container Networking
- Ebpf
- Kubernetes 1.33
- Kubernetes Networking
- Latency Reduction
- Network Profile
- Performance Optimization
- Pod Throughput
- Security Safeguards
- TCP Benchmarking
- Ubuntu 24.04
section_names:
- ai
- azure
- devops
- security
primary_section: ai
---
Sam_Foo explores eBPF host routing for Azure CNI powered by Cilium on AKS, providing a technical overview of its performance enhancements and implementation steps.<!--excerpt_end-->

# High-Performance AI Networking on AKS: eBPF Host Routing with Azure CNI Powered by Cilium

AI-driven applications demand rapid, low-latency responses. In containerized environments using Kubernetes, networking efficiency becomes a key factor for performance. Traditional Kubernetes networking via CNI plugins often relies on iptables, adding overhead that reduces throughput and increases latency.

## Azure CNI Powered by Cilium: Architecture and Benefits

Azure CNI powered by Cilium integrates directly with the AKS data plane, enabling hardware offloads and improved reliability for enterprise workloads. Real-world and benchmark results show up to 30% throughput improvement compared to standard setups. Recent advancements introduce eBPF host routing, which allows packet forwarding logic to run directly in eBPF, bypassing iptables and connection tracking.

### Key Technical Advantages

- Eliminates reliance on kernel iptables and conntrack for routing
- Reduces CPU overhead for packet processing
- Delivers measurable latency and throughput benefits for modern workloads (AI training/inference, media streaming, messaging systems)

## Benchmarking eBPF Host Routing

Performance was measured on AKS clusters (K8s 1.33, 16-core Ubuntu 24.04 nodes):

- Throughput was tested using netperf TCP_STREAM, with varying message sizes
- Latency was evaluated with TCP_RR tests at multiple percentiles
- Transaction rate was measured as transactions per second between pods

### Results Summary

- **Same-node pods:** eBPF routing improved throughput by ~30%
- **Cross-node pods:** Throughput gains even higher (up to 3x with smaller messages)
- **Latency:** Consistently reduced for intra-node traffic
- **Transactions/sec:** 27% improvement (from 16,003.7 to 20,396.9) with eBPF host routing

## Enabling eBPF Routing via Advanced Container Networking Services (ACNS)

eBPF host routing is disabled by default due to security considerations (potential for bypassed custom iptables rules or host policies). Azure's ACNS provides a safe activation path:

- ACNS validates iptables rules before activation, blocking changes if user-defined rules are present
- Upon enablement, kernel-level protections prevent new iptables rules and generate Kubernetes events for visibility
- Users can benefit from eBPF performance while maintaining robust security and compliance

### How To Enable

1. See [Microsoft’s documentation](https://learn.microsoft.com/en-us/azure/aks/how-to-enable-ebpf-host-routing) for prerequisites and guidance
2. Update your AKS network profile to include:

   ```json
   "networkProfile": {
     "advancedNetworking": {
       "enabled": true,
       "performance": {
         "accelerationMode": "BpfVeth"
       }
     }
   }
   ```

3. Validate performance gains and network policy compliance as detailed in [ACNS resources](https://aka.ms/acnsperformance)

## Advanced Safeguards in ACNS

- Built-in checks for pre-existing host-level rules
- Event generation for all significant changes
- Prevents silent failures and missed audit logs

## Resources

- [Container Network Security with Advanced Container Networking Services (ACNS) – Azure Kubernetes Service | Microsoft Learn](https://learn.microsoft.com/en-us/azure/aks/container-network-security-concepts)
- [Configure Azure CNI Powered by Cilium in Azure Kubernetes Service (AKS) – Azure Kubernetes Service | Microsoft Learn](https://learn.microsoft.com/en-us/azure/aks/azure-cni-powered-by-cilium)
- [ACNS Performance](https://aka.ms/acnsperformance)

## Conclusion

Azure CNI powered by Cilium with eBPF host routing brings substantial throughput and latency improvements for mission-critical, AI-driven container workloads. ACNS ensures these benefits are delivered securely and reliably.

---
*Author: Sam_Foo*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/introducing-ebpf-host-routing-high-performance-ai-networking/ba-p/4468216)
