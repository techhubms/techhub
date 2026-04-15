---
author: vdivizinschi
external_url: https://techcommunity.microsoft.com/t5/azure/running-commands-across-vm-scale-set-instances-without-rdp-ssh/m-p/4511577#M22490
tags:
- Az VM Run Command Invoke
- Az Vmss Run Command Invoke
- Azure
- Azure Batch
- Azure CLI
- Azure Run Command
- Azure Service Fabric
- Azure Virtual Machine Scale Sets
- Azure VM Agent
- Community
- DevOps
- Diagnostics
- Flexible VM Scale Sets
- Network Security Groups
- RBAC
- RDP
- RunPowerShellScript
- RunShellScript
- SSH
- Troubleshooting
- Uniform VM Scale Sets
- Virtual Machine Contributor
- VMSS
section_names:
- azure
- devops
date: 2026-04-15 11:46:24 +00:00
feed_name: Microsoft Tech Community
title: Running Commands Across VM Scale Set Instances Without RDP/SSH Using Azure CLI Run Command
primary_section: azure
---

vdivizinschi explains how to run diagnostic and remediation commands across Azure VM Scale Set instances using Azure Run Command via the control plane, avoiding RDP/SSH. The post covers prerequisites, RBAC permissions, orchestration-mode differences (Uniform vs Flexible), and provides Bash/Azure CLI loops to run scripts across all instances.<!--excerpt_end-->

# Running Commands Across VM Scale Set Instances Without RDP/SSH Using Azure CLI Run Command

If you manage an Azure Virtual Machine Scale Set (VMSS), you may need to validate or change something across *many* nodes (40, 80, or hundreds), for example:

- Checking a configuration value
- Retrieving logs
- Applying a registry change
- Confirming runtime settings
- Running a quick diagnostic command

## The traditional approach (and its limitations)

Historically, administrators would:

- Open RDP connections to Windows nodes
- SSH into Linux nodes
- Execute commands manually on each instance

This breaks down quickly for environments such as:

- Azure Batch (user-managed pools)
- Azure Service Fabric (classic clusters)
- VMSS-based application tiers

It can become operationally inefficient, time-consuming, or sometimes impossible—especially when:

- RDP/SSH ports are blocked
- Network Security Groups (NSGs) restrict inbound connectivity
- Administrative credentials are unavailable
- Network configuration issues prevent guest access

## Azure Run Command

Azure provides a built-in way to execute commands inside VMs through the Azure control plane, without requiring direct guest OS connectivity: **Run Command**.

Official docs:

- [Run scripts in a Linux VM in Azure using action Run Commands](https://learn.microsoft.com/en-us/azure/virtual-machines/linux/run-command)
- [Run scripts in a Windows VM in Azure using action Run Commands](https://learn.microsoft.com/en-us/azure/virtual-machines/windows/run-command?tabs=portal%2Cpowershellremove)

Run Command uses the **Azure VM Agent** installed on the VM to execute PowerShell or shell scripts inside the guest OS.

Because it runs via the Azure control plane, it can still work when:

- RDP/SSH ports are blocked
- NSGs restrict inbound access
- Administrative user configuration is broken

It’s specifically meant to troubleshoot/remediate VMs you can’t reach via standard remote access.

## Prerequisites and restrictions

Before using Run Command:

- VM Agent must be installed and in a **Ready** state
- The VM needs outbound connectivity to Azure public IPs over **TCP 443** to return results

> If outbound connectivity is blocked, scripts may run successfully but no output will be returned to the caller.

Other limitations called out:

- Output is limited to the last **4,096 bytes**
- Only **one** script execution at a time per VM
- Interactive scripts are not supported
- Maximum execution time: **90 minutes**

Full list of restrictions:

- https://learn.microsoft.com/en-us/azure/virtual-machines/windows/run-command?tabs=portal%2Cpowershellremove#restrictions

## Required permissions (RBAC)

To execute Run Command, you need Azure RBAC permissions.

| Action | Permission |
| --- | --- |
| List available Run Commands | `Microsoft.Compute/locations/runCommands/read` |
| Execute Run Command | `Microsoft.Compute/virtualMachines/runCommand/action` |

The execution permission is included in:

- **Virtual Machine Contributor** role (or higher)

## Azure CLI: `az vm` vs `az vmss`

Azure CLI has two similar commands that behave differently.

### `az vm run-command invoke`

- Used for standalone VMs
- Also used for **Flexible VM Scale Sets**
- Targets VMs by **name**

### `az vmss run-command invoke`

- Used only for **Uniform VM Scale Sets**
- Targets instances by numeric `instanceId` (0, 1, 2, ...)

Unlike standalone VM execution, VMSS instances must be referenced using `--instance-id` to identify which scale set instance will run the script.

## Important: Uniform vs Flexible VM Scale Sets

This affects how you automate Run Command.

### Uniform VM Scale Sets

- Instances are managed as identical replicas
- Each instance has a numeric `instanceId`
- Supported by `az vmss run-command invoke`

### Flexible VM Scale Sets

- Each instance is a first-class Azure VM resource
- Instance identifiers are VM names (not numbers)
- `az vmss run-command invoke` is not supported
- Use `az vm run-command invoke` per VM

To determine the orchestration mode:

```bash
az vmss show -g "${RG}" -n "${VMSS}" --query "orchestrationMode" -o tsv
```

## Windows vs Linux targets

Pick the correct command ID based on the guest OS:

- Windows VMs: `RunPowerShellScript`
- Linux VMs: `RunShellScript`

## Example: retrieve hostname from all VMSS instances

The following examples show how to retrieve the hostname from all VMSS instances using Azure CLI and Bash.

### Flexible VMSS (Bash + Azure CLI)

```bash
RG="<ResourceGroup>"
VMSS="<VMSSName>"
SUBSCRIPTION_ID="<SubscriptionID>"

az account set --subscription "${SUBSCRIPTION_ID}"

VM_NAMES=$(az vmss list-instances \
  -g "${RG}" \
  -n "${VMSS}" \
  --query "[].name" \
  -o tsv)

for VM in $VM_NAMES; do
  echo "Running on VM: $VM"

  az vm run-command invoke \
    -g "${RG}" \
    -n "$VM" \
    --command-id RunShellScript \
    --scripts "hostname" \
    --query "value[0].message" \
    -o tsv
done
```

### Uniform VMSS (Bash + Azure CLI)

```bash
RG="<ResourceGroup>"
VMSS="<VMSSName>"
SUBSCRIPTION_ID="<SubscriptionID>"

az account set --subscription "${SUBSCRIPTION_ID}"

INSTANCE_IDS=$(az vmss list-instances \
  -g "${RG}" \
  -n "${VMSS}" \
  --query "[].instanceId" \
  -o tsv)

for ID in $INSTANCE_IDS; do
  echo "Running on instanceId: $ID"

  az vmss run-command invoke \
    -g "${RG}" \
    -n "${VMSS}" \
    --instance-id "$ID" \
    --command-id RunShellScript \
    --scripts "hostname" \
    --query "value[0].message" \
    -o tsv
done
```

## Summary

Azure Run Command provides a scalable way to:

- Execute diagnostics
- Apply configuration changes
- Collect logs
- Validate runtime settings

…across VMSS instances without requiring RDP/SSH connectivity, which is especially useful for larger compute environments like Azure Batch pools, classic Service Fabric clusters, and VMSS-based app tiers.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure/running-commands-across-vm-scale-set-instances-without-rdp-ssh/m-p/4511577#M22490)

