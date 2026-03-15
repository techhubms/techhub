---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/enhance-your-data-protection-strategy-with-azure-elastic-san-s/ba-p/4443607
title: Enhance Your Data Protection Strategy with Azure Elastic SAN’s Newest Backup Options
author: adarsh_v
feed_name: Microsoft Tech Community
date: 2025-08-18 15:31:44 +00:00
tags:
- Azure Backup
- Azure Business Continuity Center
- Azure Elastic SAN
- Business Continuity
- Cloud Storage
- Commvault
- Cross Region Restore
- Data Protection
- Enterprise Backup
- Incremental Snapshots
- Iscsi
- Linux
- Managed Disks
- Restore Points
- Snapshot Retention
- VM Data Recovery
- Windows
- Azure
- Security
- Community
section_names:
- azure
- security
primary_section: azure
---
adarsh_v presents a detailed overview of new backup solutions for Azure Elastic SAN, covering both Azure Backup and Commvault integrations to safeguard cloud storage in enterprise environments.<!--excerpt_end-->

# Enhance Your Data Protection Strategy with Azure Elastic SAN’s Newest Backup Options

Azure Elastic SAN is evolving as a scalable high-performance storage platform, and now, organizations can leverage fully managed backup solutions through Azure Backup and Commvault—both in public preview. These integrations simplify and automate backup management, delivering improved data protection, simplified restore processes, and advanced recovery features for Elastic SAN volumes.

## Azure Backup Release Highlights

- **Operational Tier Backup with Independent Lifecycle**: Each backup creates a Managed Disk Incremental Snapshot of an Elastic SAN volume. These snapshots are stored in locally redundant storage (LRS) and managed independently of the original volume, ensuring recoverability even if volumes are deleted.
- **Daily Restore Points**: Up to 450 restore points are supported on a daily schedule, providing strong short-term retention and reducing the risk of data loss.
- **Simplified Management**: Configuration and monitoring are integrated via the Azure Business Continuity Center. Azure Backup automatically manages backup scheduling and retention, minimizing administrative overhead.
- **Cost Considerations**: During preview, Azure Backup Protected Instance Fees are waived for Elastic SAN volumes, but standard charges for incremental snapshots apply.

**Learn more about Elastic SAN**: [Microsoft Documentation](https://learn.microsoft.com/en-us/azure/storage/elastic-san/elastic-san-introduction)

## Commvault Release Highlights

- **Snapshot-Based Protection**: Commvault's IntelliSnap enables rapid, low-impact backups. Retention periods are configurable by day or count, with indefinitely long options available.
- **Flexible Recovery Options**: Full-VM and attach-disk restore are provided, including support for cross-region restores in which Elastic SAN volumes can be restored as managed disks.
- **Platform Compatibility**: Supports both Windows and Linux virtual machines. Discovering Elastic SAN volumes requires PowerShell (Windows) or Python 3 (Linux).
- **Deployment Considerations**: Currently, app-consistent restore points are not supported; attach-disk restores to VMs result in managed disks. Connection to VMs is via iSCSI.

More on implementation requirements: [Commvault Documentation](https://documentation.commvault.com/additionalsetting/bazureenableelasticsansnapbackupsupport.html)

## Use Case Comparison

- **Azure Backup** is ideal for native, single-volume Azure-centric scenarios, offering integrated management and up to 450 days of restore point retention.
- **Commvault** excels at large-scale VM scenarios, multiple volume management, and advanced backup needs, including indefinite retention and granular recovery controls.

For questions or guidance on choosing a solution, contact the Azure Elastic SAN team at [AzElasticSAN-Ex@microsoft.com](mailto:AzElasticSAN-Ex@microsoft.com).

## References and Further Reading

- [Configure Azure Backup for Elastic SAN](https://learn.microsoft.com/en-us/azure/backup/azure-elastic-san-backup-configure)
- [Azure Elastic SAN Introduction](https://learn.microsoft.com/en-us/azure/storage/elastic-san/elastic-san-introduction)
- [Commvault Azure Protection Documentation](https://documentation.commvault.com/11.40/essential/protected_azure_virtual_machine_resources.html)
- [Azure Backup Pricing Details](https://azure.microsoft.com/en-us/pricing/details/managed-disks/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/enhance-your-data-protection-strategy-with-azure-elastic-san-s/ba-p/4443607)
