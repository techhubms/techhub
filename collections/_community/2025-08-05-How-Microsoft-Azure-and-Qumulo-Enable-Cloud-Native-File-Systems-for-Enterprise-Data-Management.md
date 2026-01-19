---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/how-microsoft-azure-and-qumulo-deliver-a-truly-cloud-native-file/ba-p/4426321
title: How Microsoft Azure and Qumulo Enable Cloud-Native File Systems for Enterprise Data Management
author: dukicn
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-08-05 21:25:58 +00:00
tags:
- AI Workloads
- Azure Blob Storage
- Azure Native Qumulo
- Azure Virtual Machines
- Cloud Native Architecture
- Cloud Native Qumulo
- Data Management
- Data Migration
- Elastic Scalability
- Enterprise Storage
- File System
- HPC
- Komprise
- Multi Protocol Support
- Object Storage
- Qumulo
- Replication
- Snapshots
section_names:
- ai
- azure
- ml
---
dukicn examines how Azure and Qumulo deliver cloud-native enterprise file storage. The article covers architectural options, migration tools, scalability, security features, and integration for AI and HPC workloads.<!--excerpt_end-->

# How Microsoft Azure and Qumulo Enable Cloud-Native File Systems for Enterprise Data Management

**Author:** dukicn  

## Introduction

Microsoft Azure and Qumulo have partnered to address long-standing challenges in enterprise data estate modernization. Their joint solutions provide high-performance, scalable, and cost-efficient file storage platforms for global organizations managing unstructured data.

## Key Offerings: Azure Native Qumulo (ANQ) & Cloud Native Qumulo (CNQ)

- **Azure Native Qumulo (ANQ):** Fully managed, elastic file service on Azure, offering enterprise NAS features with utility-based pricing.
- **Cloud Native Qumulo (CNQ):** Self-hosted file data service leveraging Azure Virtual Machines, networking, and Blob Storage, granting direct infrastructure control.
- Both solutions scale independently for capacity/performance, support exabyte-level storage, and are POSIX-compliant.

## Core Features

- **Elastic Scalability:** Instantly adjust compute or storage per workload demands, scaling up or down as business needs shift.
- **Boundless Scale:** No practical limits on file system size or file count; robust multi-protocol support (NFS, SMB, FTP/S, and more).
- **Utility-Based Pricing:** Pay only for resources consumed, aligning with dynamic budget constraints.
- **Rapid Deployment:** Provision ANQ from the Azure Portal, CLI, or PowerShell; CNQ deploys via Terraform and adapts to custom architectures.
- **Automated Cost Optimization:** Integration with Komprise Intelligent Tiering and Azure Blob Storage access tiers optimizes storage expenses.
- **Advanced Data Management:** Features like snapshots, replication, real-time analytics, data protection, and global collaboration.
- **Multi-Protocol and Multi-Cloud:** Unified global namespace allows movement between on-prem, edge, and Azure environments for hybrid/multi-cloud strategies.

## Migration and Integration

- **Komprise for File Migration:** Platform-agnostic migration of data from legacy NAS systems into Qumulo on Azure, ensuring data integrity and high-speed transfers.
- **Cloud Data Fabric:** Enables seamless and instantaneous data movement between clouds and on-premises locations.
- **Use Cases:** Picture Archiving and Communication Systems (PACS), high-performance compute (HPC), AI workloads, media archives, and healthcare imaging.

## Security & Compliance

- **Data Protection:** Features like cryptographically locked snapshots, continuous replication, and HIPAA-compliant platforms for sensitive workloads.
- **Durability & Availability:** Utilizes Azure Blob Storage's 11-nines durability and supports deployment across multiple availability zones for resilience.
- **Authentication & Authorization:** Integrates with OAuth 2.0, OpenID Connect, SAML, and supports common protocols for secure device and user access.

## AI and HPC Support

- **Optimized for AI:** Low-latency (1–2ms), high-throughput architecture meets the demands of AI model training, inference workloads, and high-speed data pipelines.
- **Burst Capabilities:** Supports burst rendering, GPU compute, and seamless handoff to Azure-based AI applications and platforms, like Microsoft Copilot and AI Studio.

## Example Architecture

- **Migration Workflow:** Use Komprise for large-scale migration from third-party NAS to ANQ, retaining data integrity and achieving quick cut-over to Azure-native storage.
- **Customizable Performance:** Adjust computing resources as workload demands grow or shift, without disruption or need to re-provision.
- **Disaster Recovery:** Instantly convert instances from DR/archival to high-performance modes without data migration or significant reconfiguration.

## Conclusion

Azure Native Qumulo and Cloud Native Qumulo offer next-generation, cloud-native file storage for enterprises, unlocking new capacity, performance, and flexibility while integrating seamlessly into the Azure ecosystem. With support for next-gen workloads like machine learning and HPC, advanced data management, and global collaboration, these solutions position organizations for future-ready data infrastructure.

For tailored architectural advice and to see how these solutions can fit your enterprise, Qumulo offers free assessments; reach out via Azure@qumulo.com.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/how-microsoft-azure-and-qumulo-deliver-a-truly-cloud-native-file/ba-p/4426321)
