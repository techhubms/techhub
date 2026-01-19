---
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/introducing-wireguard-in-transit-encryption-for-aks-public/ba-p/4421057
title: Introducing WireGuard In-Transit Encryption for Azure Kubernetes Service (Public Preview)
author: josephyostos
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-09-18 10:43:54 +00:00
tags:
- Advanced Container Networking
- AKS
- Azure CNI
- Cilium
- Cloud Security
- Cluster Security
- Container Networking
- Encryption
- in Transit Encryption
- Key Management
- Network Security
- VNet Encryption
- WireGuard
section_names:
- azure
- security
---
josephyostos details the new WireGuard-based in-transit encryption feature for Azure Kubernetes Service, describing its integration, architecture, and benefits for securing pod-to-pod traffic in security-sensitive environments.<!--excerpt_end-->

# Introducing WireGuard In-Transit Encryption for Azure Kubernetes Service (AKS)

Azure Kubernetes Service (AKS) continues to advance security for containerized workloads with the public preview of WireGuard-based in-transit encryption. This feature, available as part of Advanced Container Networking Services, enables organizations to protect network traffic between cluster nodes with minimal operational overhead—particularly important in regulated or security-sensitive environments.

## What is WireGuard?

WireGuard is a modern VPN protocol praised for its speed, simplicity, and robust cryptography. In AKS, WireGuard is now integrated into the Cilium data plane and managed through AKS networking. This allows for transparent encryption of inter-node traffic within your Kubernetes cluster, and eliminates the need for external VPN solutions or custom key management tools.

## Encryption Scope in AKS

- **Encrypted:** Inter-node pod traffic (between pods on different nodes in the same AKS cluster).
- **Not encrypted:** Same-node pod traffic (stays internal to the host), node-generated traffic (originating from the node itself).

This setup secures the most critical network paths (traffic crossing the network infrastructure) without incurring unnecessary overhead for intra-node communication.

## Key Benefits

- **Simple to enable:** WireGuard can be activated with cluster creation or update flags.
- **Automatic key management:** Each node generates/exchanges keys automatically; no manual configuration required.
- **Transparent:** Application code does not need modification since encryption happens at the network layer.
- **Cloud-native integration:** Natively part of Advanced Container Networking Services and Cilium, with seamless management on Azure.

## How WireGuard Encryption Works in AKS

1. Each AKS node generates its own public/private key pair.
2. Public keys are shared securely between nodes through the CiliumNode custom resource.
3. A dedicated network interface (`cilium_wg0`) is created and managed by the on-node Cilium agent.
4. Keys are automatically rotated every 120 seconds, enhancing security by minimizing potential exposure.
5. Only nodes with the proper keys can participate in encrypted communication, enhancing cluster security.

## WireGuard vs. VNet Encryption

AKS now offers two in-transit encryption strategies:

| Feature            | WireGuard Encryption                | VNet Encryption                       |
|--------------------|-------------------------------------|----------------------------------------|
| Scope              | Inter-node, pod-to-pod              | All VNet traffic                       |
| VM Support         | All VM SKUs                         | Requires Gen2 VMs (hardware support)   |
| Deployment         | Cloud-agnostic, hybrid ready        | Azure-only                             |
| Performance        | Software-based, moderate CPU usage  | Hardware-accelerated, low overhead     |

Use **WireGuard** for flexibility across multiple clouds or if your VM SKUs lack support for VNet Encryption. Opt for **VNet Encryption** if you require coverage for all network traffic with ultra-low CPU overhead.

## Getting Started

- Start with [the how-to guide](https://learn.microsoft.com/en-us/azure/aks/how-to-apply-wireguard) for step-by-step instructions on enabling WireGuard in your AKS cluster.
- Learn more:
  - [Container Network Observability](https://learn.microsoft.com/en-us/azure/aks/advanced-container-networking-services-overview?tabs=cilium)
  - [L7 Network Policies](https://learn.microsoft.com/en-us/azure/aks/container-network-security-l7-policy-concepts)
  - [FQDN-based Policy](https://learn.microsoft.com/en-us/azure/aks/container-network-security-fqdn-filtering-concepts)

## Conclusion

WireGuard encryption in AKS is designed to deliver practical, high-impact network security for containerized workloads, offering balance between protection, flexibility, and operational simplicity.

---
*Post by josephyostos, September 18, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/introducing-wireguard-in-transit-encryption-for-aks-public/ba-p/4421057)
