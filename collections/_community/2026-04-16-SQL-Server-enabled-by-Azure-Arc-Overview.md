---
feed_name: Microsoft Tech Community
title: SQL Server enabled by Azure Arc Overview
external_url: https://techcommunity.microsoft.com/t5/azure-arc-blog/sql-server-enabled-by-azure-arc-overview/ba-p/4496399
date: 2026-04-16 15:00:00 +00:00
author: NaufalPrawironegoro
primary_section: azure
section_names:
- azure
- security
tags:
- 168.63.129.16
- 169.254.169.254
- Auto Discovery
- Azcmagent
- Azure
- Azure Arc
- Azure Arc Enabled SQL Server
- Azure Connected Machine Agent
- Azure Hybrid Benefit
- Azure Monitor
- Azure Monitor Agent (ama)
- Azure Policy
- Azure Portal
- Azure Resource Manager
- Azure SQL Managed Instance
- Best Practices Assessment
- Community
- Dependency Mapping
- Distributed Availability Groups
- Extended Security Updates (esu)
- Firewall Rules
- Governance
- Guest Configuration
- His.arc.azure.com
- Hybrid Cloud
- IMDS
- Log Analytics Workspace
- Log Replay Service
- Log Shipping
- Login.microsoftonline.com
- Managed Instance Link
- Management.azure.com
- Microsoft Entra ID
- Migration Assessment
- Monitoring
- Multi Cloud
- New NetFirewallRule
- Onboarding
- PowerShell
- Security
- Service Map
- SQL Server
- SQL Server Extension
- Test NetConnection
- Wire Server
---

NaufalPrawironegoro explains how to bring on-prem and multi-cloud SQL Server instances under Azure management with Azure Arc, covering onboarding (agent + PowerShell), unified Azure Portal visibility, best-practices assessments via Log Analytics, policy-based governance, monitoring, and common troubleshooting scenarios.<!--excerpt_end-->

# SQL Server enabled by Azure Arc Overview

Azure Arc extends the Azure control plane so you can **organize, govern, monitor, and secure** resources that run outside Azure (on-prem, edge, AWS, GCP). For SQL Server specifically, Arc lets you project SQL Server instances into Azure for a more consistent management experience.

## Table of contents

1. What is Azure Arc-enabled SQL Server?
2. Connecting SQL Server to Azure Arc (4-step onboarding)
3. Your SQL Server is now visible in the Azure control plane
4. SQL Best Practices Assessment
5. Monitoring and governance
6. Troubleshooting guide
7. Demo and additional resources

## 1. What is Azure Arc-enabled SQL Server?

Azure Arc helps you connect SQL Server to Azure **wherever it runs** (datacenter, AWS EC2, Google Cloud VMs, edge, VMware/Hyper-V). The goal is a **single control plane** for governance, security, and monitoring.

### SQL Server migration journey in Azure Arc

Azure Arc also includes a migration journey to Azure SQL targets:

- Continuous database migration assessments with **Azure SQL target recommendations** and cost estimates.
- Provision **Azure SQL Managed Instance** as a destination target (including a free instance evaluation option).
- Two built-in migration methods:
  - Real-time replication using **Distributed Availability Groups** (via **Managed Instance link**).
  - Log shipping via backup/restore (via **Log Replay Service**).
- A unified interface in the Azure portal instead of switching between multiple tools.
- **Microsoft Copilot** can be integrated at select points in the migration journey (note: this is not GitHub Copilot).

Learn more: [SQL Server migration in Azure Arc – Generally Available | Microsoft Community Hub](https://techcommunity.microsoft.com/blog/MicrosoftDataMigration/sql-server-migration-in-azure-arc-%E2%80%93-generally-available/4471020)

### 1.1 The problem Azure Arc solves

Organizations often have SQL Servers spread across environments:

| Location | Challenge without Azure Arc |
| --- | --- |
| On-premises datacenter | Separate management tools, no unified view |
| AWS EC2 instances | Multi-cloud complexity, different monitoring |
| Google Cloud VMs | Inconsistent governance and policies |
| Edge / branch offices | Limited visibility, manual compliance |
| VMware / Hyper-V | No cloud-native management features |

Azure Arc addresses this by extending a single Azure control plane to **all** SQL Servers, regardless of where they run.

References:

- [Azure Arc overview (Microsoft Learn)](https://learn.microsoft.com/en-us/azure/azure-arc/overview)
- [Architecture: Administer SQL Server with Azure Arc (Microsoft Learn)](https://learn.microsoft.com/en-us/azure/architecture/hybrid/azure-arc-sql-server)
- [Docs index: SQL Server enabled by Azure Arc (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/?view=sql-server-ver17)
- [SQL Server migration in Azure Arc (Community Hub)](https://techcommunity.microsoft.com/blog/azuresqlblog/sql-server-migration-in-azure-arc-generally-available/4339783)

## 2. Connecting SQL Server to Azure Arc

This section describes onboarding so your SQL Server shows up in Azure Portal alongside other Azure resources.

### 2.1 Step 1: Access Azure Arc portal

Navigation:

- Azure Portal → **Azure Arc** → **Machines**

### 2.2 Step 2: Configure onboarding options

Choose:

- Operating system (Windows or Linux)
- Enable SQL Server auto-discovery:
  - **Automatically connect any SQL Server instances to Azure Arc**
- Connectivity method:
  - **Public endpoint** (direct internet access)
  - **Private endpoint** (VPN/ExpressRoute)

Important: check the **Connect SQL Server** option so SQL instances are discovered and connected automatically (installs the SQL Server extension).

### 2.3 Step 3: Download the onboarding script

Azure generates a customized PowerShell script that includes subscription/resource group/location parameters.

Requirement called out: the server needs outbound **HTTPS (443)** to Azure endpoints.

### 2.4 Step 4: Run the script on your server

Run the script in PowerShell as Administrator on the SQL Server host.

During execution:

1. Azure Connected Machine Agent is downloaded and installed
2. Agent establishes a secure connection to Azure
3. Server is registered as an Azure Arc resource
4. SQL Server extension is installed (if selected)
5. SQL Server instance appears in Azure Arc → SQL Server

Docs:

- [Connect your SQL Server to Azure Arc (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/connect?view=sql-server-ver17)
- [Prerequisites (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/prerequisites?view=sql-server-ver17)
- [Manage automatic connection / auto-deploy (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/manage-autodeploy?view=sql-server-ver17)

## 3. Your SQL Server is now visible in the Azure control plane

After onboarding, your SQL Server is projected into Azure Portal while remaining where it runs.

### 3.1 Unified view in Azure Portal

Two navigation paths:

| Navigation path | What you see |
| --- | --- |
| Azure Arc → SQL Server | All Azure Arc-enabled SQL instances |
| Azure Arc → Machines | The host server with extensions |

### 3.2 Similar experience to SQL Server on Azure VM

The article notes that the Arc-enabled SQL Server management experience is very similar to SQL Server on Azure VM (version/edition, VM details, licensing, storage, feature status).

### 3.3 Azure Hybrid Benefit (AHB)

AHB can reduce licensing costs for **Azure SQL Database** and **Azure SQL Managed Instance** (the article cites savings up to ~30%+ by using Software Assurance-enabled SQL Server licenses).

> Note: AHB applies to Azure SQL Database and SQL Managed Instance. For SQL Server running outside Azure but managed via Azure Arc, AHB does not apply directly. Arc still provides centralized management, Azure-integrated security, and access to Extended Security Updates (ESUs).

Docs:

- [Configure SQL Server enabled by Azure Arc (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/manage-configuration?view=sql-server-ver17)
- [Manage licensing and billing (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/manage-license-billing?view=sql-server-ver17)

## 4. SQL Best Practices Assessment

Azure Arc-enabled SQL Server supports **Best Practices Assessment**, which checks configuration against Microsoft recommendations.

### 4.1 Prerequisite: Log Analytics workspace

You need a **Log Analytics workspace** to store assessment results, performance metrics, and logs.

### 4.2 Enable the assessment

- The feature uploads results via **Azure Monitor Agent (AMA)**.
- Configuration includes:
  - Enabling the feature
  - Selecting a Log Analytics workspace
  - Choosing a resource group for AMA

### 4.3 Run and review

- Running an assessment can take ~5–10 minutes for small environments and ~30–60 minutes for larger ones (as stated).
- History can include states like Scheduled, Completed, Failed - result expired, and Failed - upload failed.

Severity levels described:

| Severity | Description | Action timeline |
| --- | --- | --- |
| High | Critical issues affecting performance or security | Address immediately |
| Medium | Important optimizations | Within 30 days |
| Low | Nice-to-have | As time permits |
| Info | Informational findings | Review and acknowledge |

Docs:

- [Configure Best Practices Assessment (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/assess?view=sql-server-ver17)
- [Troubleshoot assessment (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/troubleshoot-assessment?view=sql-server-ver16)
- [Assess migration readiness (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/migration-assessment?view=sql-server-ver17)
- [Create a Log Analytics workspace (Microsoft Learn)](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/quick-create-workspace)

## 5. Monitoring and governance

Once SQL Servers are connected (Arc-enabled or native in Azure), you can use Azure governance and monitoring tools.

### 5.1 Azure Policy compliance

Use **Azure Policy** to apply consistent policies across all SQL Servers, regardless of location, and view compliance (including non-compliant Arc resources of type `microsoft.hybridcompute`).

### 5.2 Performance monitoring

Use Azure dashboards to track metrics like disk utilization, CPU, and memory across Arc-enabled and Azure-native servers.

### 5.3 Service dependency mapping

Use Service Map-style dependency mapping to see processes and network ports used by a server to understand dependencies ahead of maintenance or migration.

## 6. Troubleshooting guide

The article distinguishes issues that apply to:

- Azure Arc-enabled servers
- Azure VMs (Azure VM agent / Wire Server / IMDS scenarios)

### 6.1 Common issues

| Issue | Symptoms | Azure Arc-enabled | Azure VM |
| --- | --- | --- | --- |
| Assessment upload failed | Status: Failed - upload failed | Yes | Yes |
| Wire Server 403 | Agent cannot connect | N/A | Yes |
| IMDS disabled | Cannot obtain token | N/A | Yes |
| Arc agent connectivity | Server not appearing | Yes | N/A |
| SQL login failed | Machine account denied | Yes | Yes |

### 6.2 Case study: Assessment upload failed on an Azure VM

Symptoms:

- Assessment status: Failed - upload failed
- Local data collected (415 issues)
- Data not appearing in Log Analytics workspace

Root causes from logs:

- IMDS disabled:
  - `[ERROR] Customer disable the IMDS service, cannot obtain IMDS token.`
- Wire Server 403:
  - `[WARN] GetMachineGoalState() failed: 403 (Forbidden) to 168.63.129.16`

PowerShell snippets shown:

```powershell
# Test connectivity
Test-NetConnection -ComputerName 168.63.129.16 -Port 80

# Add route if missing
route add 168.63.129.16 mask 255.255.255.255 <gateway> -p

# Add firewall rule if needed
New-NetFirewallRule -DisplayName "Allow Azure Wire Server" -Direction Outbound -RemoteAddress 168.63.129.16 -Action Allow
```

```powershell
# Test IMDS
Invoke-RestMethod -Uri "http://169.254.169.254/metadata/instance?api-version=2021-02-01" -Headers @{Metadata="true"}

# Add firewall rule if blocked
New-NetFirewallRule -DisplayName "Allow Azure IMDS" -Direction Outbound -RemoteAddress 169.254.169.254 -Action Allow
```

```powershell
# Check Arc agent status
& "$env:ProgramW6432\AzureConnectedMachineAgent\azcmagent.exe" show

# Test connectivity to Azure endpoints
& "$env:ProgramW6432\AzureConnectedMachineAgent\azcmagent.exe" check
```

### 6.3 Azure Arc-enabled SQL Server connectivity

Required endpoints for the Azure Arc agent (as listed):

| Endpoint | Port | Purpose |
| --- | --- | --- |
| `management.azure.com` | 443 | Azure Resource Manager |
| `login.microsoftonline.com` | 443 | Azure AD authentication |
| `*.his.arc.azure.com` | 443 | Azure Arc Hybrid Identity |
| `*.guestconfiguration.azure.com` | 443 | Guest configuration |

Docs:

- [Troubleshoot assessment (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/troubleshoot-assessment?view=sql-server-ver16)
- [What is 168.63.129.16 (Wire Server)? (Microsoft Learn)](https://learn.microsoft.com/en-us/azure/virtual-network/what-is-ip-address-168-63-129-16)
- [Instance Metadata Service (IMDS) (Microsoft Learn)](https://learn.microsoft.com/en-us/azure/virtual-machines/instance-metadata-service)
- [Troubleshoot IMDS connection issues on Windows VMs (Microsoft Learn)](https://learn.microsoft.com/en-us/troubleshoot/azure/virtual-machines/windows/windows-vm-imds-connection)
- [Troubleshoot Azure Windows VM Agent issues (Microsoft Learn)](https://learn.microsoft.com/en-us/troubleshoot/azure/virtual-machines/windows/windows-azure-guest-agent)

## 7. Demo and additional resources

- [Demo Deck: Azure Arc for Windows Server and SQL Server](https://microsoft.seismic.com/apps/doccenter/a5266a70-9230-4c1e-a553-c5bddcb7a896/doc/%252Fdde0caec0e-9236-f21b-2991-5868e63d3984%252FdfYTZjNDRiZDMtMzEwZS1kNWZkLTNjOGEtNjliYWJjMjhmMmUw%252CPT0%253D%252CUHJvZHVjdCBEZW1v%252Flf062d7968-3275-47e7-8ed3-1c84ead67f6a/grid/)

Additional resources (as provided):

- [New migration capability in Azure Arc (Microsoft Learn)](https://learn.microsoft.com/sql/sql-server/azure-arc/migrate-to-azure-sql-managed-instance)
- [Onboard SQL Server to Azure Arc (Microsoft Learn)](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/connect?view=sql-server-ver17&tabs=windows)
- [Continuous migration assessment (TechCommunity)](https://techcommunity.microsoft.com/blog/microsoftdatamigration/general-availability-continuous-migration-assessment-for-sql-server-enabled-by-a/4430603)
- SQL Server samples: http://github.com/microsoft/sql-server-samples

Updated Apr 12, 2026 (Version 1.0)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-arc-blog/sql-server-enabled-by-azure-arc-overview/ba-p/4496399)

