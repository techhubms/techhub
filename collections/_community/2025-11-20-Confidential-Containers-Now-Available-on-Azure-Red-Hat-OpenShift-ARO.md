---
layout: post
title: Confidential Containers Now Available on Azure Red Hat OpenShift (ARO)
author: MelanieKraintz007
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-red-hat-openshift-confidential-containers-general/ba-p/4469089
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-20 14:06:03 +00:00
permalink: /azure/community/Confidential-Containers-Now-Available-on-Azure-Red-Hat-OpenShift-ARO
tags:
- AMD SEV SNP
- Azure Red Hat OpenShift
- Cloud Security
- Cluster Isolation
- Compliance
- Confidential Computing
- Confidential Containers
- Containerization
- CVMs
- DevOps Environments
- GDPR
- HIPAA
- Hybrid Cloud
- Intel TDX
- Microsoft Ignite
- OpenShift 4.15
- Sandboxes
- Security Automation
- Trusted Execution Environments
- Workload Attestation
- Zero Trust
section_names:
- azure
- devops
- security
---
MelanieKraintz007 explores Azure Red Hat OpenShift’s new Confidential Containers, detailing how this general availability release advances cloud security and zero-trust for confidential workloads on Azure.<!--excerpt_end-->

# Confidential Containers Now Available on Azure Red Hat OpenShift (ARO)

Announced at Microsoft Ignite 2025 in San Francisco, Microsoft and Red Hat have launched Confidential Containers for Azure Red Hat OpenShift (ARO), delivering hardware-based data protection to containerized workloads in the public cloud.

## What are Confidential Containers on ARO?

Confidential Containers on ARO combine Azure’s Confidential Computing features with Red Hat OpenShift, letting organizations encrypt data in use, at rest, and in transit. Sensitive workloads run inside Trusted Execution Environments (TEEs) built on AMD SEV-SNP or Intel TDX, ensuring hardware-level isolation and workload integrity via attestation (zero-trust).

## Technical Highlights

- **Rollout Timing**: Available in major Azure regions following Ignite 2025
- **OpenShift Version**: Supported with ARO 4.15 and later
- **Hardware Support**: AMD SEV-SNP, Intel TDX for CVMs (Confidential Virtual Machines)
- **Peer Pod Architecture**: Each container isolated from host and other cluster workloads
- **Workload Attestation**: Verifies code and data integrity before execution
- **Security Consistency**: Uniform zero-trust across cloud, hybrid, and on-premises deployments
- **Compatible Workloads**: Confidential and standard containers run side-by-side; supports existing CI/CD processes

## Benefits for Organizations

| Category       | Value                                                      |
|:-------------- |:----------------------------------------------------------|
| Security       | Hardware isolation and attestation prevent privileged access|
| Compliance     | Meets mandates (HIPAA, GDPR) for sensitive data            |
| Hybrid Consistency | Consistent security model on-prem, hybrid, and cloud    |
| Performance    | Optimized Confidential VMs reduce overhead                 |
| Simplicity     | Easy enablement from OpenShift console                     |
| Workload Integrity | Attestations confirm trusted execution                  |

## Designed Use Cases

1. **Regulatory Compliance**: Process confidential data under privacy mandates
2. **Secure DevOps Environments**: Isolate dev/test/prod with enclave security
3. **Confidential Analytics**: Analyze regulated/proprietary data

## Getting Started & Resources

- [Microsoft Learn: Confidential Containers on ARO](https://learn.microsoft.com/en-us/azure/openshift/confidential-containers-overview)
- [Red Hat Documentation](https://docs.redhat.com/en/documentation/openshift_sandboxed_containers/1.10/html/deploying_confidential_containers/deploying-cc_azure-cc)
- [Region Availability](https://azure.microsoft.com/en-us/explore/global-infrastructure/products-by-region/?products=openshift&regions=all)

For full deployment details, see Red Hat’s documentation and reach out to your Microsoft or Red Hat representative. This release enables a verified, zero-trust foundation for scaling secure containers on Azure.

---

**Author:** MelanieKraintz007

Updated: November 11, 2025

Learn more about Confidential Containers via the lightning talk at the Red Hat booth during Microsoft Ignite 2025.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-red-hat-openshift-confidential-containers-general/ba-p/4469089)
