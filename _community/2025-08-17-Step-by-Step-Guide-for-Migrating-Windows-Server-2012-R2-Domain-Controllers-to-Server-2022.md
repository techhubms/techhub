---
layout: "post"
title: "Step-by-Step Guide for Migrating Windows Server 2012 R2 Domain Controllers to Server 2022"
description: "This post from Saad_Farooq outlines a comprehensive migration plan to upgrade from Windows Server 2012 R2/2016 Domain Controllers to Server 2022, including best practices for Active Directory health checks, FSMO role transfer, CA and DHCP migration, and decommissioning old servers. It seeks operational advice and highlights potential challenges."
author: "Saad_Farooq"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/tech-community-discussion/migrate-2012-r2-to-server-2022/m-p/4444704#M9677"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-17 06:17:47 +00:00
permalink: "/2025-08-17-Step-by-Step-Guide-for-Migrating-Windows-Server-2012-R2-Domain-Controllers-to-Server-2022.html"
categories: ["Security"]
tags: ["Active Directory", "Active Directory Upgrade", "AD Health Check", "Azure DR", "Certificate Authority Migration", "Community", "DHCP Migration", "Directory Services", "Domain Controller", "FSMO Roles", "High Availability", "Microsoft Infrastructure", "Redundancy", "Security", "Server Decommission", "Windows Server", "Windows Server R2"]
tags_normalized: ["active directory", "active directory upgrade", "ad health check", "azure dr", "certificate authority migration", "community", "dhcp migration", "directory services", "domain controller", "fsmo roles", "high availability", "microsoft infrastructure", "redundancy", "security", "server decommission", "windows server", "windows server r2"]
---

Saad_Farooq presents a detailed scope for migrating Active Directory from Windows Server 2012 R2 to Server 2022, discussing domain controller upgrades, FSMO transfer, certificate authority, and DHCP migration in a secure and redundant environment.<!--excerpt_end-->

# Step-by-Step Guide for Migrating Windows Server 2012 R2 Domain Controllers to Server 2022

This guidance is for upgrading Active Directory (AD) infrastructure from Windows Server 2012 R2/2016 to Server 2022, incorporating CA and DHCP migration, and ensuring high availability. Authored by Saad_Farooq, it covers planning, sequencing, and potential pitfalls for a successful domain migration.

## Migration Scope

- **Review and validate existing Active Directory health status** across all sites
- **Promote new AD 2022 domain controllers** (3xDC: 1 physical, 2 virtual)
- **Transfer FSMO Roles** to new 2022 DCs
- **Test domain functionality** after all DCs are upgraded
- **Migrate Certificate Authority** to a dedicated 2022 server
- **Migrate DHCP** to two new highly available virtual DHCP servers
- **Decommission old DCs** (HQ and Azure-DR site)

## Suggested Best Practices and Steps

### 1. Active Directory Health Review

- Use `dcdiag`, `repadmin`, and Event Viewer to assess current AD state
- Address replication errors, sysvol issues, or lingering objects before proceeding
- Backup all DCs and AD system state

### 2. Deploy Windows Server 2022 DCs

- Install Server 2022 with latest security and feature updates
- Promote server(s) to DC via AD Domain Services (ensure same domain/forest)
- Allow replication and verify consistency using AD tools

### 3. Transfer FSMO Roles

- Use `ntdsutil` or PowerShell (`Move-ADDirectoryServerOperationMasterRole`) to transfer all FSMO roles to a new 2022 DC
- Validate FSMO role placement via `netdom query fsmo`

### 4. Test Domain Functionality

- Validate DNS, authentication, GPO application, and replication across all DCs
- Monitor Event Logs for errors post-upgrade

### 5. Migrate Certificate Authority

- Stand up a new Server 2022 with the same hostname/FQDN (if possible)
- Backup CA database and private key from old server
- Restore to new server and verify via Certificate Authority Management Console
- Revalidate certificate templates and autoenrollment

### 6. Migrate DHCP

- Configure DHCP failover for high availability across two Server 2022 VMs
- Export DHCP database from old server using `netsh dhcp server export`
- Import on new DHCP servers and authorize them in AD
- Ensure all scopes/options are correct and test lease assignments

### 7. Decommission Old Domain Controllers

- Transfer and confirm all roles/data
- Demote old DCs using `dcpromo` or Server Manager
- Remove from AD Sites and Services
- Validate AD and DNS health post-demotion

### 8. Decommission Azure-DR Site DCs

- Remove the DR site objects from AD Sites and Services after confirming no dependencies
- Validate replication and AD health

## Typical Challenges and Recommendations

- Watch for time skew issues between legacy and new DCs
- CA migration may require careful DNS and certificate template management
- DHCP failover setup may require cross-site connectivity if sites are remote
- Always use backups and document your process for rollback

## Additional Tips

- Run migrations during low-usage windows
- Inform users/admin staff about potential service interruptions
- Verify backups before any irreversible step

## Reference Tools

- `dcdiag`, `repadmin`, PowerShell AD module, CA and DHCP Export/Import utilities, Event Viewer

Following these steps will minimize downtime and help ensure your migration is secure and robust.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/tech-community-discussion/migrate-2012-r2-to-server-2022/m-p/4444704#M9677)
