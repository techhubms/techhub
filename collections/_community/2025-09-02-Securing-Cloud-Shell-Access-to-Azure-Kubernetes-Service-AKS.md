---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/securing-cloud-shell-access-to-aks/ba-p/4450299
title: Securing Cloud Shell Access to Azure Kubernetes Service (AKS)
author: samcogan
feed_name: Microsoft Tech Community
date: 2025-09-02 15:13:03 +00:00
tags:
- Access Control
- AKS
- Azure Bastion
- Azure CLI
- Azure Cloud Shell
- Azure Firewall
- Cloud Security
- Command Invoke
- IP Allow List
- Kubernetes
- NAT Gateway
- Network Security
- Private Endpoints
- Vnet Integration
- Azure
- DevOps
- Security
- Community
section_names:
- azure
- devops
- security
primary_section: azure
---
Sam Cogan presents a comprehensive review of secure methods for accessing AKS from Azure Cloud Shell, highlighting configuration scripts, networking solutions, and practical security tips for Azure administrators.<!--excerpt_end-->

# Securing Cloud Shell Access to Azure Kubernetes Service (AKS)

**Author: samcogan**

Azure Cloud Shell is a browser-based shell provided by Microsoft to manage Azure resources using trusted tools like Azure CLI, PowerShell, and kubectl without requiring local installation. While Cloud Shell is convenient, connecting to Azure Kubernetes Service (AKS) clusters from Cloud Shell introduces security challenges. This article examines various options to securely access AKS while minimizing exposure and maintaining robust security postures.

## Why Secure Cloud Shell-to-AKS Access?

By default, connections from Cloud Shell to AKS originate from Microsoft-managed, randomly assigned IP addresses. To allow this, the AKS API server must be publicly accessible with open IP ranges, which increases risk by exposing your cluster to the broader internet. Although authentication is still needed, limiting API server network exposure provides a critical extra security layer.

## Security Options for Cloud Shell Access

### 1. IP Allow Listing

Admins can dynamically add the current Cloud Shell session's IP to the AKS authorized IP list using a provided Bash script. This workflow requires permission to update the AKS configuration and must be repeated for every session. It is essential to implement periodic cleanup of the IP list to avoid unnecessary exposure.

- **Script Example:**

  ```bash
  #!/usr/bin/env bash
  set -euo pipefail
  RG="$1"; AKS="$2"
  IP="$(curl -fsS https://api.ipify.org)"
  echo "Adding ${IP} to allow list"
  CUR="$(az aks show -g "$RG" -n "$AKS" --query "apiServerAccessProfile.authorizedIpRanges" -o tsv | tr '\t' '\n' | awk 'NF')"
  NEW="$(printf "%s\n%s/32\n" "$CUR" "$IP" | sort -u | paste -sd, -)"
  if az aks update -g "$RG" -n "$AKS" --api-server-authorized-ip-ranges "$NEW" >/dev/null; then
    echo "IP ${IP} applied successfully"
  else
    echo "Failed to apply IP ${IP}" >&2; exit 1
  fi
  ```

- **Caveats:**
  - Requires high privileges and careful review
  - Needs regular IP cleanup and repeat action for each session
  - Not recommended to use bulk Service Tags for Cloud Shell (expands the allowed range too broadly)

### 2. Command Invoke

Azure Command Invoke lets you execute CLI commands inside AKS without needing direct network connectivity. This spins up a container in the cluster to run your instructions, returning only text results and exit codes.

- **Pros:**
  - Works for private clusters without exposing the API server
  - Minimal setup, useful for troubleshooting or rare access needs
- **Cons:**
  - Slower execution due to container startup
  - More complex command syntax (less native than using kubectl directly)

**Docs:** [Access a private Azure Kubernetes Service (AKS) cluster using the command invoke or Run command feature](https://learn.microsoft.com/en-us/azure/aks/access-private-cluster?tabs=azure-cli)

### 3. Cloud Shell vNet Integration

Deploy Cloud Shell into an Azure virtual network (vNet), which allows sessions to route via your network. This configuration supports accessing private endpoints or resources using fixed outbound IP addresses (like via NAT Gateway or Firewall), enhancing security and control.

- **Private API servers:** Cloud Shell accesses AKS over the vNet directly
- **Public API servers:** Traffic leaves the vNet, but you can allow-list the static public IP from your NAT Gateway/Firewall
- **Note:** Azure Relay usage incurs extra cost

**Docs:** [Use Cloud Shell in an Azure virtual network](https://learn.microsoft.com/en-us/azure/cloud-shell/vnet/overview)

### 4. Azure Bastion

Azure Bastion provides secure tunneling to VMs and AKS over RDP/SSH directly from the Azure portal without public IP exposure. It's possible to connect to AKS clusters with Bastion (requires Standard or Premium SKU and enabling native client support), and then use kubectl without opening the API externally.

- **Setup:**

  ```bash
  az aks bastion --name <aks name> --resource-group <resource group> --bastion <bastion resource ID>
  ```

- **Pros:** Strong security, simplifies AKS access, can reduce firewall/NAT complexity
- **Cons:** Premium tier required; incurring higher costs

**Docs:** [Connect to AKS Private Cluster Using Azure Bastion (Preview)](https://learn.microsoft.com/en-us/azure/bastion/bastion-connect-to-aks-private-cluster)

## Summary Table

| Option                 | Pros                                        | Cons                                           |
|------------------------|---------------------------------------------|------------------------------------------------|
| IP Allow Listing       | Simple, no extra services                   | Manual, burdensome, cleanup required           |
| Command Invoke         | No external exposure, works for privates    | Slow, limited, not for frequent use            |
| Cloud Shell vNet       | Consistent outbound IP, secure connectivity | Requires vNet, extra cost for Azure Relay/NAT  |
| Azure Bastion          | No public API, integrated CLI               | Bastion Premium required, setup and cost       |

## Final Thoughts

With the right method and configuration, Azure Cloud Shell can securely connect to AKS clusters. Each solution has different operational and cost trade-offs, but combining vNet integration or Bastion with proper network restrictions provides the strongest posture. Evaluate each option based on your needs—automation, security, convenience, and budget.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/securing-cloud-shell-access-to-aks/ba-p/4450299)
