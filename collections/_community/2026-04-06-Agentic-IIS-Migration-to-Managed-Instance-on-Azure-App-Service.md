---
primary_section: ai
feed_name: Microsoft Tech Community
section_names:
- ai
- azure
- devops
- dotnet
title: Agentic IIS Migration to Managed Instance on Azure App Service
tags:
- .NET
- AI
- ARM Templates
- ASP.NET Framework
- Azure
- Azure App Service
- Azure Files
- Azure Key Vault
- Azure Migrate AppCat
- Azure Resource Manager
- CI/CD
- COM Components
- Community
- Copilot Chat
- DevOps
- Human in The Loop
- IIS
- IIS Migration
- Install.ps1
- Invoke SiteMigration.ps1
- IsCustomMode
- Lift And Shift
- Managed Instance On App Service
- MCP
- MCP Server
- MigrationSettings.json
- Msiexec
- MSMQ
- PowerShell
- PremiumV4
- ReadinessResults.json
- Registry Adapters
- Regsvr32
- SMTP Server
- Storage Adapters
- VS Code
- Windows Server
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/agentic-iis-migration-to-managed-instance-on-azure-app-service/ba-p/4508969
date: 2026-04-06 23:58:35 +00:00
author: Gaurav-Seth
---

Gaurav-Seth describes a hands-on, AI-guided workflow for migrating legacy IIS-hosted ASP.NET Framework apps to Managed Instance on Azure App Service, including how registry, storage, SMTP/MSMQ, and COM dependencies are handled via ARM templates and an install.ps1 startup script.<!--excerpt_end-->

## Migrating ASP.NET Framework apps from IIS to Managed Instance on Azure App Service (agent-guided via MCP)

### Problem statement

Enterprises running **ASP.NET Framework** workloads on **Windows Server + IIS** often want to modernize, but migrations are risky—especially when apps depend on OS-level features that standard PaaS hosting typically doesn’t allow.

Common blockers include:

- Windows **registry keys**
- **COM** components
- **SMTP relay**
- **MSMQ** queues
- Local file system access and hard-coded drive letters
- Custom fonts

### What is Managed Instance on Azure App Service?

**Managed Instance on App Service** targets apps that need **OS-level customization** beyond standard App Service.

It runs on:

- **PremiumV4 (PV4)** App Service Plan
- **IsCustomMode=true**

**Key constraint:** Managed Instance requires **PV4 + IsCustomMode=true**. No other SKU combination supports it.

#### Capabilities

| Capability | What it enables |
| --- | --- |
| **Registry Adapters** | Redirect registry reads to **Azure Key Vault** secrets (no code changes) |
| **Storage Adapters** | Mount **Azure Files**, local SSD, or private VNET storage as drive letters (e.g., `D:\`, `E:\`) |
| **install.ps1 startup script** | Run PowerShell at startup to enable Windows features (SMTP/MSMQ), register COM, install MSI packages, deploy fonts |
| **Custom Mode** | Deeper Windows instance access than standard App Service guardrails |

#### Why this matters for legacy apps

Example legacy dependencies and how Managed Instance maps them:

- Registry reads (e.g., `HKLM\SOFTWARE\MyApp`) → **Registry Adapters** backed by **Key Vault**
- Local drive usage (e.g., `D:\Reports\`) → **Storage Adapters** (e.g., mount **Azure Files** to `D:\`)
- SMTP relay → enable via **install.ps1**
- COM registration (`regsvr32`) → run via **install.ps1**
- Custom fonts → deploy via **install.ps1**

> Note from the author: in many cases “zero application code changes may be required”, but some apps may still need changes depending on specifics.

### Microsoft Learn references (from the post)

- [Managed Instance on App Service overview](https://learn.microsoft.com/en-us/azure/app-service/overview-managed-instance)
- [Azure App Service documentation](https://learn.microsoft.com/en-us/azure/app-service/)
- [App Service Migration Assistant Tool](https://learn.microsoft.com/en-us/azure/app-service/app-service-migration-assistant)
- [Migrate to Azure App Service](https://learn.microsoft.com/en-us/azure/app-service/manage-move-across-regions)
- [Azure App Service Plans overview](https://learn.microsoft.com/en-us/azure/app-service/overview-hosting-plans)
- [PremiumV4 pricing tier](https://learn.microsoft.com/en-us/azure/app-service/app-service-configure-premium-tier)
- [Azure Key Vault](https://learn.microsoft.com/en-us/azure/key-vault/general/overview)
- [Azure Files](https://learn.microsoft.com/en-us/azure/storage/files/storage-files-introduction)
- [AppCat (.NET) — Azure Migrate application and code assessment](https://learn.microsoft.com/en-us/azure/migrate/appcat/dotnet)

## Why “agentic” migration?

Microsoft already provides PowerShell scripts for IIS migration:

- `Get-SiteReadiness.ps1`
- `Get-SitePackage.ps1`
- `Generate-MigrationSettings.ps1`
- `Invoke-SiteMigration.ps1`

The pain point is less “do we have scripts?” and more “how do we interpret outputs and wire everything correctly across many sites?”

The post’s position: scripts are reliable but not *guiding*—they don’t explain what to do next, how to choose targets, or how to generate the right artifacts (install script vs ARM template vs settings JSON).

## IIS Migration MCP Server (Model Context Protocol)

The post introduces an **IIS Migration MCP Server**: an orchestration layer that lets **AI assistants** call migration tools via **MCP (Model Context Protocol)**.

It’s designed to:

- Summarize script output into human-readable tables
- Enrich readiness checks with explanations and links
- Recommend the right target (Managed Instance vs standard App Service)
- Generate artifacts:
  - `install.ps1`
  - ARM templates for adapters
  - `MigrationSettings.json`
- Keep a **human-in-the-loop** safety model with explicit approval gates

### Human-in-the-loop gates

The system is described as explicitly gated:

- “Do you want to assess these sites, or skip to packaging?”
- “Here’s my recommendation… Agree?”
- “Review `MigrationSettings.json` before proceeding”
- “This will create billable Azure resources. Type ‘yes’ to confirm”

## Quick start (as provided)

1. Clone repo (URL in the post includes a placeholder):

- `git clone https://github.com/<your-org>/iis-migration-mcp.git`

2. Create venv + install dependencies:

```bash
cd iis-migration-mcp
python -m venv .venv
.venv\Scripts\activate
pip install -r requirements.txt
```

3. Download Microsoft’s migration scripts (not included in repo):

- ZIP: https://appmigration.microsoft.com/api/download/psscripts/AppServiceMigrationScripts.zip

Unzip e.g. to `C:\MigrationScripts`.

4. Configure MCP client (VS Code example):

- Copy `.vscode/mcp.json.example` → `.vscode/mcp.json`
- In Copilot Chat: “Configure scripts path to `C:\MigrationScripts`”
- Run: `@iis-migrate "Discover my IIS sites"`

The server is also described as compatible with other MCP clients (Claude Desktop, Cursor, Copilot CLI) via stdio.

## Architecture overview (from the post)

High level:

- VS Code + Copilot Chat orchestrates a multi-agent workflow
- MCP transport is stdio JSON-RPC
- A Python FastMCP server (`server.py`) exposes **13 MCP tools** across **5 phases**
- A PowerShell bridge (`ps_runner.py`) runs Microsoft’s downloaded scripts
- Deployment uses Azure ARM APIs

## Tooling reference: 13 MCP tools across 5 phases

### Phase 0 — Setup

#### `configure_scripts_path`

Points the server to the downloaded Microsoft migration scripts.

### Phase 1 — Discovery

#### 1) `discover_iis_sites`

Runs `Get-SiteReadiness.ps1` and:

- Enumerates IIS sites, app pools, bindings, virtual directories
- Runs **15 readiness checks** (config errors, HTTPS, non-HTTP protocols, ports, app pool settings/identity, virtual dirs, content size, modules, ISAPI, auth, framework version, connection strings, etc.)
- Detects nearby source artifacts (`.sln`, `.csproj`, `.cs`, `.vb`)

Outputs `ReadinessResults.json` with per-site status:

| Status | Meaning |
| --- | --- |
| READY | Clear for migration |
| READY_WITH_WARNINGS | Minor issues |
| READY_WITH_ISSUES | Non-fatal issues to address |
| BLOCKED | Fatal issues (e.g., content > 2GB) |

Requires Admin privileges + IIS installed.

#### 2) `choose_assessment_mode`

Routes each site to:

- `assess_config_only`
- `assess_config_and_source`
- `package`
- `blocked`
- `skip`

### Phase 2 — Assessment

#### 3) `assess_site_readiness`

Enriches readiness checks using `WebAppCheckResources.resx` metadata:

- Title/description/recommendation
- Category
- Documentation link

#### 4) `assess_source_code`

Parses an **AppCat (.NET)** assessment JSON and maps findings to Managed Instance actions:

| Dependency detected | Migration action |
| --- | --- |
| Registry access | Registry Adapter (ARM template) |
| Local file I/O / hardcoded paths | Storage Adapter (ARM template) |
| SMTP usage | `install.ps1` (SMTP feature) |
| COM Interop | `install.ps1` (regsvr32/RegAsm) |
| GAC | `install.ps1` |
| MSMQ | `install.ps1` |
| Certificate access | Key Vault integration |

Reference: https://learn.microsoft.com/en-us/dotnet/azure/migration/appcat/interpret-results

### Phase 3 — Recommendation & provisioning

#### 5) `suggest_migration_approach`

Routes to the right approach based on source availability and OS customization needs.

#### 6) `recommend_target`

Recommends target per site with confidence + reasoning:

| Target | When recommended | SKU |
| --- | --- | --- |
| `MI_AppService` | COM, registry, MSMQ, SMTP, local file I/O, GAC, Windows Service deps | PV4 |
| `AppService` | standard web app | PV2 |
| `ContainerApps` | microservices/container-first | N/A |

#### 7) `generate_install_script`

Generates `install.ps1` for OS-level tasks such as:

- SMTP: `Install-WindowsFeature SMTP-Server`
- MSMQ install and queue setup
- COM/MSI registration (`msiexec`, `regsvr32`, `RegAsm`)
- Crystal Reports runtime MSI
- Font deployment to `C:\Windows\Fonts`

#### 8) `generate_adapter_arm_template`

Generates ARM templates for:

- **Registry Adapters** (Key Vault-backed)
- **Storage Adapters**:
  - AzureFiles
  - Custom (private endpoint + VNET)
  - LocalStorage

Includes managed identity + RBAC guidance.

### Phase 4 — Deployment planning & packaging

#### 9) `plan_deployment`

Builds a plan and:

- Assigns sites to plans
- Enforces **PV4 + IsCustomMode=true** for Managed Instance

#### 10) `package_site`

Runs `Get-SitePackage.ps1` to create ZIP deployables, optionally injecting `install.ps1`.

Size limit: **2 GB**.

#### 11) `generate_migration_settings`

Creates `MigrationSettings.json` via `Generate-MigrationSettings.ps1`, then injects Managed Instance fields.

Important note from the post:

- The Managed Instance App Service Plan is **not auto-created** by the migration tools.
- You must **pre-create** the plan (PV4 + `IsCustomMode=true`) and reference it.

Example (as provided):

```json
{ "AppServicePlan": "mi-plan-eastus", "Tier": "PremiumV4", "IsCustomMode": true, "InstallScriptPath": "install.ps1", "Region": "eastus", "Sites": [ { "IISSiteName": "MyLegacyApp", "AzureSiteName": "mylegacyapp-azure", "SitePackagePath": "packagedsites/MyLegacyApp_Content.zip" } ] }
```

### Phase 5 — Execution

#### 12) `confirm_migration`

Shows a cost/resource summary and requires explicit confirmation.

#### 13) `migrate_sites`

Runs `Invoke-SiteMigration.ps1` to:

1. Set subscription context
2. Create/validate resource group
3. Create plans (PV4 with `IsCustomMode` for Managed Instance)
4. Create web apps
5. Configure runtime settings based on IIS
6. Configure virtual directories/apps
7. Disable basic auth (FTP + SCM)
8. Deploy ZIP via Azure REST API

Outputs `MigrationResults.json`.

## Managed Instance “provisioning split”

The post highlights a key concept: OS dependencies split into two configuration channels:

- **Platform-level (ARM template)**
  - Registry adapters → Key Vault secrets
  - Storage mounts → Azure Files / local SSD / private storage
- **OS-level (`install.ps1`)**
  - COM/MSI registration (`regsvr32`, `RegAsm`, `msiexec`)
  - SMTP feature
  - MSMQ
  - Crystal Reports runtime
  - Custom fonts

The MCP server uses assessment results to decide which bucket each dependency falls into and generates the right artifacts.

## Prerequisites (as listed)

- Windows Server with IIS
- PowerShell 5.1
- Python 3.10+
- Administrator privileges
- Azure subscription
- Azure PowerShell (Az module)
- Microsoft Migration Scripts ZIP
- AppCat CLI (optional)
- FastMCP (`mcp[cli]>=1.0.0`)

## Error handling categories (as listed)

| Error | Cause | Resolution |
| --- | --- | --- |
| `ELEVATION_REQUIRED` | Not running as Admin | Restart VS Code/terminal as Admin |
| `IIS_NOT_FOUND` | IIS or WebAdministration missing | Install IIS role + WebAdministration |
| `AZURE_NOT_AUTHENTICATED` | Not logged into Azure PowerShell | `Connect-AzAccount` |
| `SCRIPT_NOT_FOUND` | Scripts path not configured | Run `configure_scripts_path` |
| `SCRIPT_TIMEOUT` | Script exceeded time limit | Check server responsiveness |
| `OUTPUT_NOT_FOUND` | JSON output wasn’t created | Verify script execution |

## Conclusion

The post argues that the combination of:

- Microsoft’s PowerShell migration scripts
- Azure App Service Managed Instance capabilities
- MCP-based AI orchestration

can reduce migration effort and errors—especially across many IIS sites—while keeping the human in control via explicit confirmation gates.


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/agentic-iis-migration-to-managed-instance-on-azure-app-service/ba-p/4508969)

