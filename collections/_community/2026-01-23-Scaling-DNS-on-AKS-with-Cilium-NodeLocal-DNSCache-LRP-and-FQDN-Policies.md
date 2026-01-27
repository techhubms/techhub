---
external_url: https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/scaling-dns-on-aks-with-cilium-nodelocal-dnscache-lrp-and-fqdn/ba-p/4486323
title: 'Scaling DNS on AKS with Cilium: NodeLocal DNSCache, LRP, and FQDN Policies'
author: Simone_Rodigari
feed_name: Microsoft Tech Community
date: 2026-01-23 17:28:47 +00:00
tags:
- AKS
- Cilium
- Cilium LRP
- CiliumNetworkPolicy
- ConfigMap
- CoreDNS
- DaemonSet
- DNS
- Egress Control
- FQDN
- Hubble
- Kubernetes
- Linux Networking
- Local Redirect Policy
- Network Policy
- Network Security
- NodeLocal DNSCache
- RBAC
section_names:
- azure
primary_section: azure
---
Simone Rodigari provides a comprehensive guide for deploying NodeLocal DNSCache on AKS clusters using Cilium, highlighting how to optimize DNS resolution, avoid CoreDNS bottlenecks, and implement reliable FQDN-aware network policies.<!--excerpt_end-->

# Scaling DNS on AKS with Cilium: NodeLocal DNSCache, LRP, and FQDN Policies

**Author: Simone Rodigari**

## Introduction

Standard Kubernetes DNS relies on a centralized CoreDNS service, which can become a scaling and latency bottleneck in large clusters. This guide covers strategies to eliminate these issues on AKS by using NodeLocal DNSCache in conjunction with Cilium's Local Redirect Policy (LRP) and secure FQDN-aware network policies.

## Why Adopt NodeLocal DNSCache?

- **Eliminating Conntrack Pressure:** High-volume DNS traffic can create conntrack bottlenecks, leading to lookup delays.
- **Reducing Latency:** Local caching removes the network hop to CoreDNS, delivering near-instant responses for cached queries.
- **Offloading CoreDNS:** Sharding DNS queries using a DaemonSet avoids overloading central DNS services, improving cluster resilience during scaling events.

### When is this needed?

- Large-scale clusters (hundreds+ nodes)
- High-churn workloads (e.g., frequent auto-scaling)
- Performance-sensitive or real-time apps

## Deploying NodeLocal DNSCache with Cilium

Normally, NodeLocal DNSCache installs iptables/dummy interfaces for DNS hijacking. In Cilium environments, leverage LRP for DNS redirection:

1. **Deploy DaemonSet:** Run `node-local-dns` on every node. Disable iptables/interfacing setup using flags `-skipteardown=true`, `-setupinterface=false`, and `-setupiptables=false`.
2. **Bind to All Interfaces:** Configure with `-localip 0.0.0.0`.
3. **Cilium LRP:** Use a `CiliumLocalRedirectPolicy` CRD to point DNS traffic (destined for the kube-dns ClusterIP) to the local cache pod on the same node.

#### Example DaemonSet Arguments

```yaml
- "-localip"
- "0.0.0.0"
- "-conf"
- "/etc/Corefile"
- "-upstreamsvc"
- "kube-dns-upstream"
- "-skipteardown=true"
- "-setupinterface=false"
- "-setupiptables=false"
```

#### Example Cilium LRP CRD

```yaml
apiVersion: "cilium.io/v2"
kind: CiliumLocalRedirectPolicy
metadata:
  name: nodelocaldns
  namespace: kube-system
spec:
  redirectFrontend:
    serviceMatcher:
      serviceName: kube-dns
      namespace: kube-system
  redirectBackend:
    localEndpointSelector:
      matchLabels:
        k8s-app: node-local-dns
    toPorts:
    - port: "53"
      name: dns
      protocol: UDP
    - port: "53"
      name: dns-tcp
      protocol: TCP
```

> **Note:** Enable LRPs in Cilium (`localRedirectPolicies.enabled=true`) before applying. See [Cilium docs](https://docs.cilium.io/en/stable/network/kubernetes/local-redirect-policy/#prerequisites).

## Network Policy Gotchas: FQDN Filtering

Strict `CiliumNetworkPolicy` rules that only allow egress to `kube-dns` will break DNS lookups under local redirection, since traffic is now redirected to `node-local-dns`.

**You must allow egress to both `kube-dns` and `node-local-dns` selectors.**

#### Before (Fails with LRP)

```yaml
- toEndpoints:
  - matchLabels:
      k8s:io.kubernetes.pod.namespace: kube-system
      k8s:k8s-app: kube-dns
  toPorts:
  - ports:
    - port: "53"
      protocol: ANY
```

#### After (Correct)

```yaml
- toEndpoints:
  - matchLabels:
      k8s:io.kubernetes.pod.namespace: kube-system
      k8s:k8s-app: kube-dns
  - matchLabels:
      k8s:io.kubernetes.pod.namespace: kube-system
      k8s:k8s-app: node-local-dns
  toPorts:
  - ports:
    - port: "53"
      protocol: ANY
```

### Use Hubble for Troubleshooting

Denied DNS egress attempts will show as `EGRESS DENIED` to the `node-local-dns` pod. Once you fix the policy, successful lookups get an `EGRESS ALLOWED` verdict and application connections can proceed.

## Real-World Example: Workload with FQDN Policy

```yaml
apiVersion: "cilium.io/v2"
kind: CiliumNetworkPolicy
metadata:
  name: secure-workload-policy
spec:
  endpointSelector:
    matchLabels:
      app: critical-workload
  egress:
  # 1. Allow DNS Resolution
  - toEndpoints:
    - matchLabels:
        k8s:io.kubernetes.pod.namespace: kube-system
        k8s:k8s-app: kube-dns
    - matchLabels:
        k8s:io.kubernetes.pod.namespace: kube-system
        k8s:k8s-app: node-local-dns
    toPorts:
    - ports:
      - port: "53"
        protocol: ANY
    rules:
      dns:
      - matchPattern: "*"
  # 2. Allow FQDN Traffic
  - toFQDNs:
    - matchName: "api.example.com"
    toPorts:
    - ports:
      - port: "443"
        protocol: TCP
```

## AKS LocalDNS: Considerations

**AKS LocalDNS** provides a managed node-local DNS solution in Azure, but:

- It is incompatible with Cilium/ACNS FQDN filter policies ([details](https://learn.microsoft.com/en-us/azure/aks/localdns-custom)).
- Updating config requires node pool reimaging.
- Manual NodeLocal DNSCache + LRP approach is more suitable for FQDN enforcement.

> Do not enable both NodeLocal DNSCache and LocalDNS in the same node pool to avoid unpredictable results.

## References

1. [Kubernetes Documentation: NodeLocal DNSCache](https://kubernetes.io/docs/tasks/administer-cluster/nodelocaldns/)
2. [Cilium Documentation: Local Redirect Policy](https://docs.cilium.io/en/stable/network/kubernetes/local-redirect-policy/)
3. [AKS Documentation: Configure LocalDNS](https://learn.microsoft.com/en-us/azure/aks/localdns-custom)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/scaling-dns-on-aks-with-cilium-nodelocal-dnscache-lrp-and-fqdn/ba-p/4486323)
