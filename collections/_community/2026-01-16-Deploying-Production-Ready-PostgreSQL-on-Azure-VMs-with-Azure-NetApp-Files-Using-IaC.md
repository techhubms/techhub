---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/deploy-postgresql-on-azure-vms-with-azure-netapp-files/ba-p/4486114
title: Deploying Production-Ready PostgreSQL on Azure VMs with Azure NetApp Files Using IaC
author: GeertVanTeylingen
feed_name: Microsoft Tech Community
date: 2026-01-16 00:05:44 +00:00
tags:
- AI
- AI/ML Workloads
- ARM Templates
- Automated Deployment
- Azure NetApp Files
- Azure Virtual Machines
- Cloud Database
- Database Migration
- IaC
- Network Security Groups
- NFS
- Performance Optimization
- PostgreSQL
- PowerShell
- Production Workloads
- Snapshot Management
- Terraform
section_names:
- azure
- devops
primary_section: azure
---
GeertVanTeylingen, with co-authors Prabu Arjunan and Asutosh Panda, presents a practical guide to automating PostgreSQL deployments on Azure VMs with Azure NetApp Files. Learn how Infrastructure as Code and Azure services streamline repeatable, secure, and high-performance database environments.<!--excerpt_end-->

# Deploying Production-Ready PostgreSQL on Azure VMs with Azure NetApp Files Using IaC

## Author

GeertVanTeylingen

Co-authors:

- [Prabu Arjunan](https://www.linkedin.com/in/prabu-arjunan/), Azure NetApp Files Product Manager
- [Asutosh Panda](https://www.linkedin.com/in/asutosh-panda-a2892a7b/), Azure NetApp Files Technical Marketing Engineer

---

## Introduction

Deploying PostgreSQL on Azure VMs with high-performance storage can be complex, involving multiple manual steps across networking, storage, NFS configuration, and OS/database initialization. This guide introduces production-ready Infrastructure as Code (IaC) templates—available for Terraform, ARM, and PowerShell—to automate the entire process, delivering consistency, security, and optimal performance for all environments.

## Why PostgreSQL on Azure NetApp Files?

### Performance That Scales

- **Azure NetApp Files** provides predictable, low-latency performance ideal for database workloads.
- Enterprise features: Snapshots, cross-region replication, backup integration, and independent scaling of storage and compute.

### Service Levels

- **Standard:** Up to 16 MiB/s per TB – dev/test workloads
- **Premium:** Up to 64 MiB/s per TB – production databases
- **Ultra:** Up to 128 MiB/s per TB – analytics/high-performance
- **Flexible:** Custom throughput/capacity per pool

### Technical Advantages

- Fast, reliable I/O for DB operations
- Instant snapshots for backup/rollback
- Seamless scaling and cost efficiency by service tier

## The Problem: Manual Deployment Complexity

Manual infrastructure and configuration steps can result in:

- 6–10 hours of setup per environment
- Configuration drift
- Increased risk of errors
- Inconsistent security/configuration standards

### Typical Steps

1. **Infrastructure:** Network, subnets, VM provisioning, NSGs
2. **Storage:** NetApp Files setup, NFS configuration
3. **PostgreSQL:** Install, NFS mount, initialize data directory, configure security
4. **Database:** User/database creation, authentication, validation
5. **Repeat for every environment**

## The Solution: IaC Templates for Azure

Automate deployments with reusable, customizable templates:

### Supported Flows

- **Terraform:** Suited for hybrid/multi-cloud or code-centric teams
- **ARM Templates:** Azure-native, compatible with Azure Portal and DevOps pipelines
- **PowerShell:** For Windows-focused and scripting environments

#### Example (Terraform)

```hcl
module "postgresql_vm_Azure_NetApp_Files" {
  source                  = "./terraform/db/postgresql-vm-Azure NetApp Files"
  postgresql_version      = "15"
  postgresql_admin_password = var.pg_admin_password
  database_name           = "production_db"
  database_user           = "app_user"
  database_password       = var.db_password
  netapp_service_level    = "Premium"
  netapp_volume_size      = 500
}
```

### Resources Provisioned

- VNet, subnets, network security
- Azure NetApp Files account, capacity pool, and NFS volumes
- Ubuntu VM (PG 14/15/16), PostgreSQL installation and configuration
- Automated data directory placement on NetApp volume
- Security hardening, connection info, and validation outputs

## Real-World Impact: Automation in Minutes

#### Before IaC

- Multi-day, error-prone, manual deployment
- Higher risk of inconsistencies and misconfigurations

#### After IaC

- Deployment in ~1 hour
- Repeatable, secure, and production-ready by default
- Built-in validation and observable outputs

## Key Features

- **Zero manual configuration:** Everything is automated
- **Security by default:** NSG rules, Key Vault integration, encrypted storage, least-privilege networking and user access
- **Production-ready:** Optimal performance, logging, monitoring, resource tagging
- **Multi-environment support:** Parameterize for dev, test, staging, and prod
- **Flexible deployment:** Use Terraform, ARM, or PowerShell based on team skills and tooling

## Getting Started

### Prerequisites

- Azure subscription with NetApp Files enabled
- Contributor permissions
- Terraform (if applicable)
- Azure PowerShell (if applicable)

### Fast Path

1. **Clone Repository:**

   ```bash
   git clone https://github.com/NetApp/azure-netapp-files-storage.git
   cd azure-netapp-files-storage
   ```

2. **Choose Your Tool:**
   - Terraform: `terraform/db/postgresql-vm-Azure NetApp Files/`
   - ARM: `arm-templates/db/postgresql-vm-Azure NetApp Files/`
   - PowerShell: `powershell/db/postgresql-vm-Azure NetApp Files/`
3. **Configure Parameters:**
   - PostgreSQL version, DB name, credentials
   - NetApp Files service level, VM size, networking
4. **Deploy and Test:**
   - Terraform: `terraform apply`
   - ARM: Deploy via Portal or CLI
   - PowerShell: `./deploy-postgresql-vm-Azure NetApp Files.ps1`
5. **Connect:**
   - `psql -h <vm_ip> -p 5432 -U appuser -d mydb`

## Use Cases

- **Development & Testing:** Isolate test environments that exactly mirror production in security and performance.
- **Production Workloads:** Reliable, high-throughput, low-latency SQL operations; consistent infrastructure.
- **AI/ML Workloads:** Support for feature stores, vector similarity search (pgvector), and integration with Azure AI and analytics.
- **Database Migrations:** Safe, high-performance targets with snapshot-based rollback and validation.

## Future Considerations

- **High Availability:** Leverage PostgreSQL replication (manual configuration)
- **Backups:** Use Azure NetApp Files snapshots for point-in-time recovery ([docs](https://learn.microsoft.com/azure/azure-netapp-files/snapshots-introduction))

## Learn More

- [GitHub Repository](https://github.com/NetApp/azure-netapp-files-storage)
- [Azure NetApp Files Documentation](https://learn.microsoft.com/azure/azure-netapp-files/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

---

For questions or contributions, visit the [GitHub repo](https://github.com/NetApp/azure-netapp-files-storage) or reach out via the provided contacts.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/deploy-postgresql-on-azure-vms-with-azure-netapp-files/ba-p/4486114)
