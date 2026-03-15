---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/parallel-aks-node-pool-creation-with-crossplane-a-version/ba-p/4477936
title: 'Parallel AKS Node Pool Creation with Crossplane: A Version Compatibility Journey'
author: sbalaji
feed_name: Microsoft Tech Community
date: 2026-01-19 15:43:48 +00:00
tags:
- AKS
- ARM Templates
- Automation
- Azure CNI
- Azure Provider
- Cluster Provisioning
- Compositions
- CRD
- Crossplane
- Helm
- IaC
- Kubernetes
- Managed Identity
- Node Pools
- Resource References
- Terraform
- Upbound
- Version Compatibility
- Azure
- DevOps
- Community
section_names:
- azure
- devops
primary_section: azure
---
sbalaji recounts a hands-on troubleshooting story addressing performance bottlenecks in AKS node pool creation with Crossplane, leading to a tested solution with newer provider versions for true parallel infrastructure provisioning.<!--excerpt_end-->

# Parallel AKS Node Pool Creation with Crossplane: A Version Compatibility Journey

Author: sbalaji

When managing Azure Kubernetes Service (AKS) clusters at scale, provisioning dozens of node pools efficiently is crucial for production environments. This in-depth article shares a practitioner’s journey troubleshooting sequential node pool creation bottlenecks using Crossplane, and the systematic steps taken to resolve them.

## The Challenge: Sequential Node Pool Creation

The initial environment used Crossplane v1.13.1 and Azure Provider v1.13.1 to automate AKS infrastructure. The core issue: AKS node pools were always created one after another, instead of in parallel, causing build times for clusters with 30–50 node pools to stretch into hours—far beyond acceptable limits for production.

### Example Observations

- Creation of each node pool took ~2 minutes
- 33 node pools: >1 hour just for the node pools (not including control plane setup)
- All node pool definitions were independent, supporting parallel capacity within each node pool

## The Technical Investigation Process

### Customer’s Approach

- Customers used managed resources (KubernetesCluster and KubernetesClusterNodePool CRDs) for AKS provisioning via Crossplane
- Alternative considered: Crossplane Compositions for reusable infrastructure blueprints, but problem persisted regardless of abstraction

### Rate Limiting & Validation

- Suspected Azure API rate limits were causing serialization
- Comparative tests with Terraform, ARM templates, and PowerShell validated that parallel creation was possible with these IaC tools, ruling out Azure service limits
- This pointed back to a versioning or Crossplane-specific problem

### Community Research

- No open/closed issues found about this bottleneck in the official Crossplane GitHub repo
- Suggests it was not a widely recognized problem, but perhaps triggered by specific scenarios or versions

### Version Compatibility Discovery

- Systematic upgrade testing led to the combination:
  - Crossplane v2.1.3
  - Azure Provider v2.2.0 (upbound/provider-family-azure, upbound/provider-azure-containerservice)
- With these versions, node pools could be truly created in parallel, dramatically improving provisioning times

## Lessons Learned & Recommendations

1. **Stay current with Crossplane and provider versions:** Performance issues can stem from outdated software, not just coding errors or service limitations
2. **Validate assumptions using alternative tools:** Cross-checking with Terraform and ARM templates helped isolate the cause
3. **Abstraction level wasn't the root cause:** Both managed resources and Compositions worked once provider version was corrected
4. **Resource references and field definitions matter:** Inspect CRDs for authoritative reference on required fields
5. **Always test for parallelism at scale:** What works at small scale might fall apart with enterprise workloads

## Conclusion

The bottleneck in AKS node pool creation was not an inherent platform limitation but a compatibility gap resolved by upgrading Crossplane and its Azure provider. For anyone managing large-scale AKS environments, keeping dependencies up-to-date and confirming behaviors with more than one automation tool are key operational best practices.

---

*Updated Jan 19, 2026 by sbalaji*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/parallel-aks-node-pool-creation-with-crossplane-a-version/ba-p/4477936)
