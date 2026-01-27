---
external_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/securing-azure-databricks-serverless-practical-guide-to-private/ba-p/4457083
title: Securing Azure Databricks Serverless with Private Link and Azure Firewall
author: alescardoso
feed_name: Microsoft Tech Community
date: 2025-09-25 23:42:50 +00:00
tags:
- Application Rules
- Azure Databricks
- Azure Firewall
- Cloud Security
- Compliance
- Data Analytics
- FQDN Filtering
- Health Probe
- Load Balancer
- Microsoft Azure
- NAT
- Network Connectivity Configuration
- Network Security Group
- NGINX
- Outbound Control
- Private Link
- Route Tables
- Router VM
- Serverless
- Virtual Network
- VNET Peering
section_names:
- azure
- ml
- security
primary_section: ml
---
alescardoso offers a hands-on walkthrough for securing Azure Databricks Serverless by integrating Private Link and Azure Firewall, enabling a compliant and tightly controlled analytics environment.<!--excerpt_end-->

# Securing Azure Databricks Serverless: Practical Guide to Private Link Integration

## Introduction

Organisations in finance, government, and healthcare need more than agility—they require robust compliance and security throughout their cloud analytics workflows. Azure Databricks Serverless provides powerful analytics, but its default unrestricted outbound networking may not meet stricter security needs. This guide by alescardoso walks through creating a secure serverless Databricks environment using Azure Private Link and Firewall.

## The Challenge: Outbound Control in Serverless

- Serverless compute plane in Databricks is managed by Microsoft.
- By default, outbound traffic can freely access the internet.
- Regulated sectors demand private network paths for data and integration.

## Solution Objectives

- **Deny-by-default outbound access** using Azure Firewall and private endpoints.
- **Explicitly control allowed outbound connections** with FQDN policies.
- **Route all traffic** through customer-managed infrastructure for audit, control, and inspection.

## Solution Overview

- Outbound Databricks Serverless traffic is routed to a Policy Enforcement Point, typically an Azure Firewall.
- Private Link Service enables private connections from Databricks control plane to customer's virtual network (VNet).
- Data remains within customer-controlled boundaries—no exposure to the public internet.

## Step-by-Step Implementation

### 1. Deploy Azure Firewall and Networking

- Provision an Azure Firewall.
- Create and configure VNets and required subnets for both Databricks and the Load Balancer.
- Peer Databricks VNet with a central hub VNet for secure routing.

### 2. Configure Azure Load Balancer

- Set up an internal Standard Load Balancer.
- Organise frontend/backend pools using NICs.
- Define load balancing rules and health probes (e.g., HTTP on port 8082).

### 3. Create Private Link Service

- Deploy behind the load balancer in the correct subnets.
- Associate with frontend/backend pools.

### 4. Route Table Setup

- Create route tables to direct VM backend traffic through Azure Firewall.
- Attach route tables to correct subnets (especially the backend subnet for router VM).

### 5. Deploy and Configure Router VM

- Add a Linux VM to act as network router.
- Enable IP forwarding (VM OS and Azure settings).
- Configure IPTables for NAT and traffic forwarding.
- Install NGINX as a health probe responder for load balancing.

### 6. Set Up Network Security Groups (NSGs)

- Permit essential traffic: SSH, load balancer, HTTP/HTTPS, health probe communications.

### 7. Azure Firewall Application Rules

- Create outbound application rules: explicitly allow traffic only to required FQDNs (e.g., `microsoft.com`).
- Deny all other outbound traffic by default.

### 8. Databricks Account Portal Configuration

- Enable serverless Private Link to customer resources.
- Define Network Connectivity Configurations (NCCs) and attach to workspaces.
- Set up private endpoint rules for each resource requiring access.

### 9. Approve Private Endpoints

- In Azure Portal, approve the endpoints created for Databricks access to resources.

### 10. Troubleshooting Guidance

- Use tools like netstat, conntrack, and tcpdump on the router VM for diagnostics.
- Double-check route table and NSG associations.
- Validate private endpoint rule configurations in both Databricks and Azure Portal.

## References

- [Serverless compute plane networking - Azure Databricks | Microsoft Learn](https://learn.microsoft.com/en-us/azure/databricks/security/network/serverless-network-security/)
- [Configure private connectivity to Azure resources - Azure Databricks | Microsoft Learn](https://learn.microsoft.com/en-us/azure/databricks/security/network/serverless-network-security/serverless-private-link)

## Key Takeaways

- Secure Azure Databricks Serverless by enforcing deny-by-default network posture.
- Route all traffic through Azure Firewall and allow only approved private endpoint connections.
- Supports compliance requirements for handling sensitive and regulated data within Microsoft Azure.

## Next Steps

- Review official documentation for latest configuration details.
- Begin deploying Private Link and Azure Firewall to secure your analytics platform.

---
*Content contributed by alescardoso*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/securing-azure-databricks-serverless-practical-guide-to-private/ba-p/4457083)
