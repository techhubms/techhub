---
external_url: https://blog.fabric.microsoft.com/en-US/blog/securely-accessing-on-premises-data-with-fabric-data-engineering-workloads/
title: Securely Accessing On-Premises Data with Microsoft Fabric Managed Private Endpoints
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-22 10:00:42 +00:00
tags:
- Data Compliance
- Data Connectivity
- Data Engineering
- Enterprise Governance
- FQDN Allowlist
- Managed Private Endpoints
- Microsoft Fabric
- Network Security
- On Premises Data
- Private Link
- Private Link Service
- REST API
- Spark Compute
- SQL Server
- VNet Integration
section_names:
- azure
- ml
- security
primary_section: ml
---
Microsoft Fabric Blog explains how organizations can securely connect Fabric Spark compute to on-premises and network-isolated data sources using Managed Private Endpoints. Learn about governance, setup via REST APIs, and practical security benefits.<!--excerpt_end-->

# Securely Accessing On-Premises Data with Microsoft Fabric Managed Private Endpoints

Organizations increasingly need to process sensitive or enterprise data within compliant, secure cloud environments. Microsoft Fabric now enables secure connectivity between Fabric Spark compute and on-premises or network-isolated data sources using Managed Private Endpoints and Private Link Services. This integration is made available via Fabric's Public REST APIs.

## Key Capabilities

- **Managed Private Endpoints:** Directly establish private connections from your Fabric workspace to on-premises or restricted resources without exposing them to the public internet.
- **High-Performance Processing:** Leverage the native Fabric Spark engine for fast, large-scale analytics on private data.
- **Security and Compliance:** Maintain enterprise compliance standards by requiring admin approval for each endpoint connection, providing clear governance and control.
- **FQDN Allowlisting:** Explicitly allow only approved Fully Qualified Domain Names (FQDNs) for Spark connectivity, reducing risk and ambiguity.

## Simplified On-Prem Connectivity

Prior to this capability, connecting Fabric workloads to on-premises systems often required complex gateways or VPNs. Managed Private Endpoints create a straightforward and governed bridge—without additional networking infrastructure. Connections must be explicitly approved by administrators, strengthening security posture and providing operational transparency.

## How to Connect to an On-Premises Data Source (SQL Server Example)

1. **Identify the Private Link Resource:** Acquire the resource ID of your on-premises Private Link Service.
2. **Define Endpoint in Fabric:** Use the Managed Private Endpoint Public REST APIs to create a direct connection.
3. **Allowlist FQDNs:** Specify which fully qualified domain names are permitted in the endpoint request.
4. **Admin Approval:** The network or resource admin must approve the connection to ensure authorization and compliance.
5. **Verify Domains:** Confirm the allowed FQDNs associated with the new connection.
6. **Process Data:** Once connected and approved, the Fabric Spark engine can securely access sources like SQL Server for analytics.

### REST API Examples

**Create a Managed Private Endpoint**

```http
POST https://api.fabric.microsoft.com/v1/workspaces/{workspaceId}/managedPrivateEndpoints

{
  "name": "testprivatendpoint1",
  "targetPrivateLinkResourceId": "/subscriptions/{subId}/resourceGroups/{rg}/providers/Microsoft.Sql/servers/testsql1",
  "targetSubresourceType": "sqlServer",
  "requestMessage": "Request message to approve private endpoint"
}
```

_Sample Response:_

```json
{
  "id": "59a92b06-6e5a-468c-b748-e28c8ff28da3",
  "name": "SqlPE",
  "targetPrivateLinkResourceId": "/subscriptions/e3bf3f1a-4d64-4e42-85e9-aa1b84e3874/resourceGroups/testRG/providers/Microsoft.SqlServer/SqlServer/sql1",
  "provisioningState": "Provisioning",
  "targetSubresourceType": "sqlServer"
}
```

**List Target FQDNs for a Managed Private Endpoint**

```http
GET https://api.fabric.microsoft.com/v1/workspaces/{workspaceId}/managedPrivateEndpoints/{managedPrivateEndpointId}/targetFQDNs
```

_Sample Response:_

```json
{
  "value": [
    "database1.cloudprovider.net",
    "database2.cloudprovider.net"
  ]
}
```

## Resources and Learn More

- [Set up a private link service for Fabric managed private endpoints (Microsoft Learn)](https://learn.microsoft.com/fabric/security/connect-to-on-premise-sources-using-managed-private-endpoints)
- [Full Managed Private Endpoints API documentation](https://learn.microsoft.com/rest/api/fabric/core/managed-private-endpoints/create-workspace-managed-private-endpoint?tabs=HTTP)

By leveraging Managed Private Endpoints, organizations safeguard sensitive data, comply with security standards, and enable governed data engineering workflows in Microsoft Fabric Spark.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/securely-accessing-on-premises-data-with-fabric-data-engineering-workloads/)
