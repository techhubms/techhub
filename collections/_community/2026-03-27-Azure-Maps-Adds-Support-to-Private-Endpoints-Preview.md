---
external_url: https://techcommunity.microsoft.com/t5/azure-maps-blog/azure-maps-adds-support-to-private-endpoints-preview/ba-p/4505743
primary_section: azure
author: PeterBr
tags:
- Account Specific Endpoint
- Atlas.microsoft.com Migration
- Az Network Private Endpoint Create
- Azure
- Azure CLI
- Azure Maps
- Azure Private Link
- Azure Virtual Network
- Community
- Compliance
- Data Privacy
- DNS Records
- Geospatial APIs
- Microsoft Backbone Network
- Network Isolation
- PHI
- Private DNS Zone
- Private Endpoints
- Private IP
- Privatelink.account.maps.azure.com
- Security
- Subnet
- VNet
date: 2026-03-27 00:23:58 +00:00
feed_name: Microsoft Tech Community
section_names:
- azure
- security
title: Azure Maps Adds Support to Private Endpoints (Preview)
---

PeterBr announces the public preview of Azure Maps support for Azure Private Link private endpoints, explaining how to keep Maps API traffic on a private IP inside a VNet and how to switch apps to the new account-specific endpoint and DNS setup.<!--excerpt_end-->

# Azure Maps Adds Support to Private Endpoints (Preview)

## Elevating Location Data Security Inside Azure Virtual Networks

Location data is sensitive and needs to be managed securely. This post announces the **public preview of private endpoint support for Azure Maps**.

With [Azure Private Link](https://learn.microsoft.com/en-us/azure/private-link/private-link-overview), applications can connect to Azure Maps over a **private IP** inside an Azure virtual network (VNet), keeping traffic on the **Microsoft backbone network** instead of the public internet. This reduces exposure to external threats while supporting stricter security, privacy, and compliance requirements.

## Raising the Bar for Location Data Security in the Cloud

Azure Maps Private Endpoints create a secure network bridge between your Azure VNet and Azure Maps using a [private endpoint](https://learn.microsoft.com/en-us/azure/private-link/private-endpoint-overview).

What changes:

- **Network isolation**: API calls are not exposed to the public internet; traffic stays within Azure’s private backbone.
- **Compliance support**: sensitive spatial/location data used by your application avoids traversing the public internet, helping minimize external exposure.

## Creating a Private Endpoint for Your Azure Maps Account

The command below specifies:

- The Azure Maps account resource ID
- The `mapsAccount` sub-resource
- The VNet and subnet to host the private endpoint

Azure creates a Private DNS zone for `privatelink.account.maps.azure.com` and adds the required DNS record automatically.

```sh
az network private-endpoint create \
  --name <myprivateendpointname> \
  --resource-group <myresourcegroup> \
  --vnet-name <myvnetname> \
  --subnet <mysubnetname> \
  --private-connection-resource-id "/subscriptions/<subscriptionid>/resourceGroups/<myresourcegroup>/providers/Microsoft.Maps/accounts/<mymapsaccountname>" \
  --group-id mapsAccount \
  --connection-name <myconnectionname>
```

## Updating Applications to Use the Account-Specific Endpoint

To use the private endpoint, configure applications to call the **Azure Maps account-specific endpoint** (instead of `https://atlas.microsoft.com`).

Access pattern:

`https://{maps-account-client-id}.{location}.account.maps.azure.com`

Example:

- Maps account client ID: `abc123`
- Region: `East US`

New endpoint:

`https://abc123.eastus.account.maps.azure.com`

## Why This Matters

Azure Maps private endpoint support enables more secure and compliant geospatial solutions (for example, handling Protected Health Information (PHI) in healthcare, optimizing logistics, or running sensitive analytics in financial services).

Key outcomes called out in the post:

- Developers can keep existing integration patterns (primarily updating the endpoint to the account-specific private DNS name).
- Network/security admins get VNet integration and more granular access control.

## Explore More

- [Azure Maps Private Endpoints documentation](https://learn.microsoft.com/en-us/azure/azure-maps/private-endpoints)
- [Azure Maps Documentation](https://learn.microsoft.com/en-us/azure/azure-maps/)
- [Azure Maps Samples](https://samples.azuremaps.com/)
- [Azure Maps Pricing](https://azure.microsoft.com/en-us/pricing/details/azure-maps/)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-maps-blog/azure-maps-adds-support-to-private-endpoints-preview/ba-p/4505743)

