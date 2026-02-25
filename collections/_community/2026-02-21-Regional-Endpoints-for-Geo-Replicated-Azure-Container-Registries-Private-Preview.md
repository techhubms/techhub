---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/regional-endpoints-for-geo-replicated-azure-container-registries/ba-p/4496186
title: Regional Endpoints for Geo-Replicated Azure Container Registries (Private Preview)
author: johshmsft
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-21 00:06:49 +00:00
tags:
- ACR
- AKS
- Azure
- Azure CLI
- Azure Container Registry
- Cloud Native
- Community
- Container Images
- Dedicated Data Endpoints
- DevOps
- Firewall Rules
- Geo Replication
- Kubernetes
- Network Configuration
- Premium SKU
- Private Endpoint
- Regional Endpoints
- Resource Provider
section_names:
- azure
- devops
---
johshmsft announces and explains the private preview of regional endpoints for Azure Container Registry. The article covers real-world scenarios, detailed setup, usage in Kubernetes, and secure networking tips to improve reliability and performance across multiple regions.<!--excerpt_end-->

# Regional Endpoints for Geo-Replicated Azure Container Registries (Private Preview)

Azure Container Registry (ACR) now offers regional endpoints in private preview, giving customers fine-grained control over image pulls and pushes for geo-replicated registries. This feature, available on the Premium SKU, is especially beneficial for organizations operating Kubernetes clusters across multiple Azure regions and needing consistent, region-affine, or failover-aware image access.

## Why Regional Endpoints?

With ACR geo-replication, your images are available in multiple regions. However, Azure-managed routing doesn't provide granular control over which replica your cluster pulls from, leading to unpredictable latency and troubleshooting challenges. The new regional endpoints allow you to directly target specific geo-replicas, enabling:

- **Explicit replica selection** for deployments
- **Predictable routing** for compliance or optimization
- **Client-side failover** during outages
- **Regional affinity**, ideal for multi-region Kubernetes deployments

## Current Challenges with Azure-Managed Routing

- **Misrouting Issues**: Clusters can pull from unexpected regions.
- **Push/Pull Consistency**: Synchronization lags can affect deployment reliability.
- **Lack of Regional Affinity**: No easy way to ensure a cluster uses its local ACR copy.
- **No Custom Failover**: Failover strategies cannot be implemented on the client-side.

## What Are Regional Endpoints?

Regional endpoints provide direct login server URLs for each geo-replica in a pattern like:

```
myregistry.<region-name>.geo.azurecr.io
```

You can continue to use your global registry endpoint and selectively use regional endpoints where you need explicit control.

### Architectural Comparison

- **Global Endpoint**: `myregistry.azurecr.io` uses Azure-managed routing.
- **Regional Endpoint**: `myregistry.<region-name>.geo.azurecr.io` targets a specific geo-replica.

Blob (layer) downloads follow the registry’s configuration:

- Without private endpoints: Azure storage accounts (\*.blob.core.windows.net)
- With private/dedicated endpoints: Regional data endpoint (e.g., myregistry.eastus.data.azurecr.io)

## How to Enable Regional Endpoints (Private Preview)

### Prerequisites

- Azure Container Registry Premium SKU
- Azure CLI 2.74.0+ with regional endpoints CLI extension
- ACR ARM API version 2026-01-01-preview

### Steps

1. **Register the feature flag:**

   ```bash
   az feature register \
     --namespace Microsoft.ContainerRegistry \
     --name RegionalEndpoints
   ```

2. **Wait for registration to complete** (check with `az feature show`).
3. **Propagate registration:**

   ```bash
   az provider register -n Microsoft.ContainerRegistry
   ```

4. **Install the CLI Extension** ([Download](https://aka.ms/acr/regionalendpoints/download)):

   ```bash
   az extension add \
     --source acrregionalendpoint-1.0.0b1-py3-none-any.whl \
     --allow-preview true
   ```

5. **Enable regional endpoints on a registry:**
   - For a new registry:

     ```bash
     az acr create -n myregistry -g myrg -l <region> --regional-endpoints enabled --sku Premium
     ```

   - For an existing registry:

     ```bash
     az acr update -n myregistry -g myrg --regional-endpoints enabled
     ```

## Using Regional Endpoints

Once enabled, each geo-replica gets its own login server URL. Regional endpoints behave like normal ACR endpoints for authentication, pushing, and pulling images. For example:

- **Login:**

  ```bash
  az acr login --name myregistry --endpoint eastus
  ```

- **Tag an image:**

  ```bash
  docker tag myapp:v1 myregistry.eastus.geo.azurecr.io/myapp:v1
  ```

- **Push an image:**

  ```bash
  docker push myregistry.eastus.geo.azurecr.io/myapp:v1
  ```

- **Pull an image:**

  ```bash
  docker pull myregistry.eastus.geo.azurecr.io/myapp:v1
  ```

## Kubernetes Integration Example

Specify regional endpoints in manifests for region-affine deployments:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: myapp-eastus
spec:
  template:
    spec:
      containers:
      - name: myapp
        image: myregistry.eastus.geo.azurecr.io/myapp:v1
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: myapp-westeurope
spec:
  template:
    spec:
      containers:
      - name: myapp
        image: myregistry.westeurope.geo.azurecr.io/myapp:v1
```

## Integration with Dedicated Data Endpoints and Private Endpoints

- **Dedicated Data Endpoints:**
  Blob downloads from a regional endpoint redirect to the region’s dedicated data endpoint for security compliance.
- **Private Endpoints:**
  Each VNet with a private endpoint to your registry will allocate an additional private IP per regional endpoint.

## Firewall and Network Rules

Configure your firewall for the desired endpoints:

- `myregistry.<region>.geo.azurecr.io` (regional)
- `myregistry.azurecr.io` (global)
- `myregistry.<region>.data.azurecr.io` (dedicated data endpoint)
- `*.blob.core.windows.net` (without Private/Dedicated Data Endpoints)

## Resources and Feedback

- [Regional endpoints for geo-replicated registries (Preview)](https://github.com/Azure/acr/blob/main/docs/preview/regional-endpoints/regional-endpoints.md)
- [Geo-replication in Azure Container Registry](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-geo-replication)
- [Mitigate data exfiltration with dedicated data endpoints](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-dedicated-data-endpoints)
- [Azure Container Registry Private Link](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-private-link)
- [Configure rules to access an Azure container registry behind a firewall](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-firewall-access-rules)
- [GitHub roadmap for feedback](https://aka.ms/cnsr/roadmap)

## Summary

Regional endpoints empower you to optimize multi-region cloud native deployments, offering control, predictable routing, and new troubleshooting and compliance strategies for container-centric workloads in Azure.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/regional-endpoints-for-geo-replicated-azure-container-registries/ba-p/4496186)
