---
primary_section: azure
external_url: https://techcommunity.microsoft.com/t5/azure-migration-and/resolving-machinewithsamebiosidandfqdnalreadyexists-during-azure/ba-p/4492307
tags:
- Agent Registration
- ASRUnifiedAgentConfigurator.log
- Azure
- Azure Appliance Configuration Manager
- Azure Migrate
- Azure Migrate Appliance
- Azure Site Recovery
- AzureRcmCli.exe
- BIOS ID
- Community
- Credential Based Discovery
- Credential Less Discovery
- Drscout.conf
- Duplicate Registration
- FQDN
- HostId
- MachineWithSameBiosIdAndFqdnAlreadyExists
- Migration Troubleshooting
- Mobility Agent
- Replication Troubleshooting
- ResourceID
- UnifiedAgentConfigurator.exe
author: vihegde
date: 2026-04-01 00:24:27 +00:00
section_names:
- azure
title: Resolving MachineWithSameBiosIdAndFqdnAlreadyExists During Azure Migrate Mobility Agent Registration
feed_name: Microsoft Tech Community
---

In this community post, vihegde explains why Azure Migrate Mobility Agent registration can fail with MachineWithSameBiosIdAndFqdnAlreadyExists after mixing credential-based and credential-less discovery, and provides a step-by-step procedure to realign the agent identity and complete registration so replication can continue.<!--excerpt_end-->

# Resolving MachineWithSameBiosIdAndFqdnAlreadyExists During Azure Migrate Mobility Agent Registration

While working on an Azure Migrate project, a Mobility Agent registration failed with the error:

> <ErrorCode>MachineWithSameBiosIdAndFqdnAlreadyExists</ErrorCode>

The error is logged on the source server in:

- `C:\ProgramData\ASRSetupLogs\ASRUnifiedAgentConfigurator.log`

This can happen when a server is first onboarded with **credential-based discovery**, and later the Mobility Agent is manually installed/re-registered using **credential-less discovery**.

## Problem statement

Scenario:

- A source server is discovered successfully using **credential-based discovery** in Azure Migrate.
- Later, the Mobility Agent is manually re-installed and re-registered using **credential-less discovery**.
- During registration, the Mobility Agent fails with `MachineWithSameBiosIdAndFqdnAlreadyExists`.
- Result: Mobility Agent registration with the Azure Migrate vault doesn’t complete and replication is blocked.

## Root cause

Azure Migrate uniquely identifies machines using **BIOS ID + FQDN**.

If the same machine is onboarded via different discovery methods:

- You can end up with **multiple registrations** for the same physical server.
- Azure Migrate detects a **duplicate identity conflict** and rejects the later registration attempt.

In this case:

- First registration: credential-based discovery (works)
- Second registration: credential-less discovery (fails due to duplicate identity)

## Resolution (step-by-step)

The goal is to realign the Mobility Agent back to the **original machine identity** (the identity created during credential-based discovery).

## Step 1: Generate Agent Config Input (source machine)

Open **Command Prompt** as Administrator and run:

```cmd
cd "C:\Program Files (x86)\Microsoft Azure Site Recovery\agent\"
AzureRcmCli.exe --getagentconfiginput
```

This outputs an **Agent Config Input** string.

## Step 2: Generate the Mobility Service configuration file (appliance)

On the Azure Migrate appliance:

1. Open **Azure Appliance Configuration Manager**
2. Go to **Mobility Service Configuration**
3. Paste the **Agent Config Input**
4. Download the generated configuration file using Microsoft’s guidance: https://learn.microsoft.com/en-us/azure/site-recovery/vmware-physical-mobility-service-overview?utm_source=chatgpt.com#generate-mobility-service-configuration-file

## Step 3: Copy the config file to the source server

Copy the downloaded file (example name):

- `e1fdc8ce-e518-4fd2-b281-cc71433d512e_config.json`

Paste it into:

- `C:\Program Files (x86)\Microsoft Azure Site Recovery\agent\Application Data\etc`

## Step 4: Validate and update `drscout.conf`

Go to:

- `C:\Program Files (x86)\Microsoft Azure Site Recovery\agent\Application Data\etc`

Edit `drscout.conf` and ensure these parameters exist under `[vxagent]`:

```ini
[vxagent]
HostId="Collect from support team"
ResourceID="Collect from support team"
```

Important:

- Use the **original** `HostId` and `ResourceID` (from the **first / credential-based** discovery).
- If you don’t have them, obtain them from the Azure Migrate support team.

## Step 5: Unregister the existing Mobility Agent

Run:

```cmd
cd "C:\Program Files (x86)\Microsoft Azure Site Recovery\agent\"
AzureRcmCli.exe --unregistersourceagent
UnifiedAgentConfigurator.exe /Unconfigure true
```

This removes the conflicting registration.

## Step 6: Regenerate the configuration file

Repeat **Step 2** (using the same Agent Config Input) to generate a fresh configuration file.

## Step 7: Re-register the Mobility Agent

Run:

```cmd
cd "C:\Program Files (x86)\Microsoft Azure Site Recovery\agent\"
UnifiedAgentConfigurator.exe /SourceConfigFilePath "config.json" /CSType CSPrime
```

After this:

- Mobility Agent registration should succeed
- Replication should proceed without the duplicate identity error

## Key learnings

- Don’t mix **credential-based** and **credential-less discovery** for the same machine.
- Azure Migrate enforces identity uniqueness using **BIOS ID + FQDN**.
- Successful recovery depends on aligning the Mobility Agent to the **original** `HostId` and `ResourceID`.
- Cleanup + correct re-registration can fix the issue without deleting the machine.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-migration-and/resolving-machinewithsamebiosidandfqdnalreadyexists-during-azure/ba-p/4492307)

