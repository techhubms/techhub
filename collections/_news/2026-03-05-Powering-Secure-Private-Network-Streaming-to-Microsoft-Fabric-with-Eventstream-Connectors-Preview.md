---
external_url: https://blog.fabric.microsoft.com/en-US/blog/powering-secure-private-network-streaming-to-fabric-with-eventstream-connectors-preview/
title: Powering Secure Private Network Streaming to Microsoft Fabric with Eventstream Connectors (Preview)
author: Microsoft Fabric Blog
primary_section: ml
feed_name: Microsoft Fabric Blog
date: 2026-03-05 16:00:00 +00:00
tags:
- Azure
- Azure Virtual Network
- Compliance
- Data Gateway
- Data Ingestion
- Eventstream
- ExpressRoute
- Hybrid Cloud
- Microsoft Fabric
- ML
- News
- On Premises Integration
- Private Network
- Real Time Data
- Real Time Intelligence
- Secure Connectivity
- Security
- Streaming Connectors
- VPN
section_names:
- azure
- ml
- security
---
Microsoft Fabric Blog presents a step-by-step guide to configuring secure, private network data streaming to Fabric using Eventstream connectors. The article walks through prerequisites, Azure integration, security considerations, and best practices for technical implementation.<!--excerpt_end-->

# Powering Secure Private Network Streaming to Fabric with Eventstream Connectors (Preview)

*Author: Microsoft Fabric Blog*

## Overview

In industries where data sensitivity and compliance are paramount—such as finance, banking, and telecom—secure and reliable access to real-time data is critical. Microsoft Fabric’s Real-Time Intelligence (RTI) leverages Eventstream connectors to enable real-time data ingestion from a variety of sources, including those hosted within private networks. This article provides a comprehensive technical walkthrough for securely streaming data from private sources to Fabric using Azure networking features.

## Why Secure Private Network Streaming?

- Real-time insights drive faster, more informed business decisions
- Sensitive IoT telemetry, financial transactions, and application data often reside on private networks (on-premises, private cloud, Azure VNet)
- Regulatory obligations mandate robust security and compliance

## Solution Architecture: Azure Virtual Network Bridge

Eventstream connectors can run inside your **Azure virtual network** using vNet injection. This design maintains private network boundaries and avoids exposing data sources to the public internet. An Azure virtual network acts as a secure bridge, connecting:

- **On-premises networks** (via VPN or ExpressRoute)
- **Third-party cloud private networks**
- **Azure-based private endpoints/networks**

**Key components:**

- Azure Virtual Network & Subnet
- Streaming Virtual Network Data Gateway
- Secure connector containers running inside the VNet

![Architecture Illustration: Hybrid cloud data integration with Azure Streaming Virtual Network Data Gateway, showing connections from on-premises and third-party cloud sources to Azure customer VNet via VPN/ExpressRoute, private endpoints, and connector containers.]

## Step-by-Step Setup

### 1. Prerequisites

- **Azure subscription** with permissions to manage VNets
- **Virtual network and subnet** configured and delegated to 'Microsoft.MessagingConnectors'
- **Identify data sources** and their private network types (on-premises, third-party, Azure)
- **Connectivity established** between the bridge VNet and private sources (VPN, ExpressRoute, private endpoints, or network peering)

Test connectivity by deploying a VM within the bridge VNet and confirming access to the private data source.

### 2. Create a Streaming Virtual Network Data Gateway

- In Fabric, launch the *Get Events* wizard, select the data source, and press **Set up** to navigate to the Manage Connections and Gateways page
- Create a new streaming virtual network data gateway using your VNet resource details

### 3. Create the Source Connection

- When configuring your Eventstream, select the newly created data gateway
- Connections via the gateway use the '[vNet]' prefix for distinction
- Configure source connector to use the gateway for secure access

### 4. Finalize Configuration & Start Streaming

- Complete any remaining source setup steps
- Create or publish your Eventstream
- The streaming connector is now provisioned inside the bridge VNet, securely accessing private network sources

### 5. Validate and Leverage Real-Time Data

- Preview data in your Eventstream workspace
- Integrate with Fabric RTI tools for analytics on securely ingested, real-time data

## Resources

- [Powering Secure Private Network Streaming with Eventstream Connectors – Demo](https://youtu.be/RKslTaoVaGg?si=q4rkwH6NyXUBiwrp&t=607)
- [Detailed Guide](https://aka.ms/ESConnectorPrivateNetworkSupport)
- [Eventstream Documentation](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/overview?tabs=enhancedcapabilities)
- [Sign up for Fabric trial](https://learn.microsoft.com/fabric/fundamentals/fabric-trial)
- [Community Forum](https://aka.ms/realtimecommunity)

## Conclusion

The preview of secure, private network streaming through Fabric’s Eventstream connectors empowers organizations to extend real-time analytics capabilities to private data sources without compromising on security or compliance. Teams can confidently provision secure, hybrid connectivity using Azure’s robust networking features.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/powering-secure-private-network-streaming-to-fabric-with-eventstream-connectors-preview/)
