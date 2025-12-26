---
layout: "post"
title: "Deploying Hybrid Azure Logic Apps on K3s for Lightweight, On-Premises Integration"
description: "This guide by praveensri explains how to deploy Azure Logic Apps Standard in hybrid mode on a K3s Kubernetes cluster, enabling integration scenarios close to on-premises data sources. The article provides step-by-step setup including K3s installation, Azure Arc integration, storage configuration with SQL and SMB, and Logic Apps workflow creation. Designed for environments where full-scale Kubernetes may not be practical, the walkthrough demonstrates leveraging lightweight infrastructure with full Azure Logic Apps functionality."
author: "praveensri"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/hybrid-logic-apps-deployment-on-rancher-k3s-kubernetes-cluster/ba-p/4448557"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-27 18:22:42 +00:00
permalink: "/community/2025-08-27-Deploying-Hybrid-Azure-Logic-Apps-on-K3s-for-Lightweight-On-Premises-Integration.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["Azure", "Azure Arc", "Azure Logic Apps", "Cluster Management", "Coding", "Community", "DevOps", "Docker Desktop", "Edge Computing", "Hybrid Deployment", "IaC", "K3s", "Kubernetes", "Logic Apps Standard", "PowerShell", "SMB File Share", "SQL Server", "Workflow Automation"]
tags_normalized: ["azure", "azure arc", "azure logic apps", "cluster management", "coding", "community", "devops", "docker desktop", "edge computing", "hybrid deployment", "iac", "k3s", "kubernetes", "logic apps standard", "powershell", "smb file share", "sql server", "workflow automation"]
---

praveensri demonstrates how to deploy Hybrid Azure Logic Apps on a lightweight K3s Kubernetes cluster, detailing infrastructure setup, Azure Arc integration, and hybrid configuration for on-premises or edge environments.<!--excerpt_end-->

# Deploying Hybrid Azure Logic Apps on K3s for Lightweight, On-Premises Integration

Hybrid deployments of Azure Logic Apps allow organizations to run workflows close to their on-premises data for performance and integration flexibility. In this guide, **praveensri** outlines a practical approach using a lightweight [K3s](https://k3s.io/) Kubernetes distribution, streamlining infrastructure needs particularly for edge or resource-constrained environments.

## Why K3s?

- **Lightweight Kubernetes**: K3s offers reduced overhead while preserving Kubernetes API compatibility, making it suitable for on-premises or edge setups.
- **Ideal for Hybrid Logic Apps**: Lets you deploy and run Azure Logic Apps Standard near local data sources, like on-prem SQL Servers and SMB file shares, without the burden of a full Kubernetes cluster.

## Deployment Steps

### 1. Prepare the K3s Cluster

- **Host Environment**: Uses Windows 11 with Docker Desktop (WSL2 integration).
- **Software Installation**:
    - Install [Docker Desktop](https://www.docker.com/products/docker-desktop/), ensure WSL2 is selected.
    - Install required tools via Chocolatey and PowerShell:

      ```powershell
      Set-ExecutionPolicy Bypass -Scope Process -Force;
      iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
      choco install kubernetes-cli -y
      choco install kubernetes-helm -y
      choco install k3d -y
      ```

    - Create a single-node K3s cluster:

      ```powershell
      k3d cluster create
      # Remove default load balancer (Traefik), if it interferes with required ports:
      kubectl delete svc traefik -n kube-system
      kubectl delete deployment traefik -n kube-system
      ```

### 2. Connect Kubernetes Cluster to Azure Arc

- Register your K3s cluster with [Azure Arc](https://learn.microsoft.com/en-us/azure/azure-arc/kubernetes/), enabling management from Azure. Follow the [official documentation](https://learn.microsoft.com/en-us/azure/logic-apps/set-up-standard-workflows-hybrid-deployment-requirements#connect-kubernetes-cluster-to-azure-arc) for step-by-step guidance.

### 3. Configure Azure Container Apps Extension and Environment

- Set up the Azure Container Apps extension for your cluster. (CoreDNS customizations for Azure Local can be skipped in this setup.)

### 4. Configure Storage (SQL Server and SMB)

- **SQL Database**: Used by Logic Apps for runtime operations and history logs. The author:
    - Set up SQL Server 2022 with SQL authentication on the Windows host.
    - Created a dedicated SQL admin user ([detailed instructions](https://learn.microsoft.com/en-us/azure/logic-apps/set-up-standard-workflows-hybrid-deployment-requirements#create-sql-server-storage-provider)).
    - Connection validation script:

      ```powershell
      $connectionString = "Server=<server IP>;Initial Catalog=<db>;Persist Security Info=False;User ID=<sqluser>;Password=<password>;..."
      try {
        $connection = New-Object System.Data.SqlClient.SqlConnection
        $connection.ConnectionString = $connectionString
        $connection.Open()
        Write-Host "✅ Connection successful"
        $connection.Close()
      } catch {
        Write-Host "❌ Connection failed: $(---
layout: "post"
title: "Deploying Hybrid Azure Logic Apps on K3s for Lightweight, On-Premises Integration"
description: "This guide by praveensri explains how to deploy Azure Logic Apps Standard in hybrid mode on a K3s Kubernetes cluster, enabling integration scenarios close to on-premises data sources. The article provides step-by-step setup including K3s installation, Azure Arc integration, storage configuration with SQL and SMB, and Logic Apps workflow creation. Designed for environments where full-scale Kubernetes may not be practical, the walkthrough demonstrates leveraging lightweight infrastructure with full Azure Logic Apps functionality."
author: "praveensri"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/hybrid-logic-apps-deployment-on-rancher-k3s-kubernetes-cluster/ba-p/4448557"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-08-27 18:22:42 +00:00
permalink: "2025-08-27-Deploying-Hybrid-Azure-Logic-Apps-on-K3s-for-Lightweight-On-Premises-Integration.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["Azure", "Azure Arc", "Azure Logic Apps", "Cluster Management", "Coding", "Community", "DevOps", "Docker Desktop", "Edge Computing", "Hybrid Deployment", "IaC", "K3s", "Kubernetes", "Logic Apps Standard", "PowerShell", "SMB File Share", "SQL Server", "Workflow Automation"]
tags_normalized: [["azure", "azure arc", "azure logic apps", "cluster management", "coding", "community", "devops", "docker desktop", "edge computing", "hybrid deployment", "iac", "k3s", "kubernetes", "logic apps standard", "powershell", "smb file share", "sql server", "workflow automation"]]
---

praveensri demonstrates how to deploy Hybrid Azure Logic Apps on a lightweight K3s Kubernetes cluster, detailing infrastructure setup, Azure Arc integration, and hybrid configuration for on-premises or edge environments.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/hybrid-logic-apps-deployment-on-rancher-k3s-kubernetes-cluster/ba-p/4448557)
.Exception.Message)"
      }
      ```

- **SMB File Share**: Handles local artifact storage.
    - Create a local Windows user for the SMB share:

      ```powershell
      $Username = "k3suser"
      $Password = ConvertTo-SecureString "<password>" -AsPlainText -Force
      $FullName = "K3s user"
      $Description = "Created via PowerShell"
      New-LocalUser -Name $Username -Password $Password -FullName $FullName -Description $Description
      Add-LocalGroupMember -Group "Users" -Member $Username
      ```

    - Set up the shared folder with the appropriate permissions ([detailed steps](https://learn.microsoft.com/en-us/azure/logic-apps/set-up-standard-workflows-hybrid-deployment-requirements#set-up-your-smb-file-share-on-windows)).

### 5. Create Hybrid Logic Apps (Standard)

- Use the Azure Portal to create Logic Apps Standard workflows.
- Configure the app to use your SQL connection and SMB path.
- Use the designer to develop and deploy workflows. Hybrid Logic Apps now run on your K3s cluster, accessing local data securely and efficiently.

## Additional Notes

- Most of the process aligns with Microsoft’s official documentation, except for the lightweight K3s specifics.
- This deployment strategy is beneficial for organizations seeking to modernize integration (e.g., BizTalk migration), ensuring flexibility, cost-effectiveness, and alignment with edge computing needs.

For further details, refer to [Set up your own infrastructure for Standard logic app workflows - Azure Logic Apps | Microsoft Learn](https://learn.microsoft.com/en-us/azure/logic-apps/set-up-standard-workflows-hybrid-deployment-requirements).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/hybrid-logic-apps-deployment-on-rancher-k3s-kubernetes-cluster/ba-p/4448557)
