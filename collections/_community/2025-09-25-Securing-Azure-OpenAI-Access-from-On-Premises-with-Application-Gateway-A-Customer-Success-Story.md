---
layout: post
title: 'Securing Azure OpenAI Access from On-Premises with Application Gateway: A Customer Success Story'
author: vnamani
canonical_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/using-application-gateway-to-secure-access-to-the-azure-openai/ba-p/4456696
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-25 21:12:24 +00:00
permalink: /ai/community/Securing-Azure-OpenAI-Access-from-On-Premises-with-Application-Gateway-A-Customer-Success-Story
tags:
- AI
- Application Gateway
- Azure
- Azure Networking
- Azure OpenAI
- Cloud Governance
- Community
- Enterprise Security
- Generative AI
- IP Firewall
- Large Language Model
- Network Security Group
- On Premises Integration
- Public IP
- Reverse Proxy
- Security
- SSL Certificate
- SSL Termination
- Virtual Network
- Web Application Firewall
section_names:
- ai
- azure
- security
---
vnamani shares a detailed case study on securing Azure OpenAI Service for an on-premises application using Application Gateway, focusing on network security and governance.<!--excerpt_end-->

# Securing Azure OpenAI Access from On-Premises with Application Gateway: A Customer Success Story

## Introduction

A large enterprise customer needed to build a generative AI application hosted on-premises while leveraging Azure OpenAI Service. The primary challenge was ensuring secure and governed connectivity to Azure-based large language models (LLMs) without private network connectivity or a full Azure landing zone.

This walkthrough demonstrates how they utilized Azure Application Gateway as a reverse proxy—enabling secure, policy-driven access to Azure OpenAI and related services under strict security constraints.

## Customer Landscape and Challenges

The customer's environment was missing several core network components:

- **No private network connectivity:** No Site-to-Site VPN, ExpressRoute, or mature cloud network topology
- **No Enterprise Scale Landing Zone (ESLZ):** Lacked virtual WAN, hub-spoke, private DNS zones, DNS resolvers, API Management, and firewalls
- **Strict security requirements:** Public access to Azure OpenAI was not allowed

**Key security objectives:**

- Restrict access to specific on-prem IP CIDR ranges
- Limit permissible network ports
- Implement SSL termination and a Web Application Firewall (WAF)
- Use customer-managed SSL certificates

## Proposed Solution Architecture

To bridge these gaps, the team implemented a secure Azure-centric architecture using:

### Core Azure Services

- **Application Gateway:** Provided Layer 7 reverse proxy, SSL termination, and WAF capabilities
- **Public IP:** Enabled internet-based communication from authorized on-prem addresses
- **Virtual Network:** Allowed segmentation and control of network traffic
- **Network Security Groups (NSG):** Managed inbound/outbound rules, only opening required HTTP(S) ports and whitelisting specific IP ranges
- **Azure OpenAI:** Served as the destination for LLM queries and responses

## Implementation Details

### NSG Configuration

- **Inbound rules:** Allowed only traffic from customer’s IP CIDR ranges over appropriate HTTP(S) ports
- **Outbound rules:** Targeted AzureCloud endpoints, acknowledging lack of direct Azure OpenAI service tags

### Application Gateway Deployment

- **SSL Certificate:** Provided by the customer’s on-premise Certificate Authority for HTTPS termination
- **Traffic flow:**
  - Decrypt incoming traffic at the Application Gateway
  - Inspect using the WAF for threats
  - Re-encrypt outgoing traffic with Azure-trusted CA
  - Override backend hostname as needed
- **Health probe:** Customized to recognize an expected 404 response from Azure OpenAI (since no native health endpoint exists)

### Azure OpenAI Configuration

- **IP firewall:** Only allowed incoming traffic from the Application Gateway subnet, reducing exposure

## Outcome

This solution enabled the customer to:

- Secure Azure OpenAI Service using well-understood Azure and networking primitives
- Operate without a complete Azure landing zone or private connectivity
- Meet enterprise-grade security and governance standards using layered defense (NSG, WAF, SSL, IP firewall)

The architecture can serve as a reference for others facing similar hybrid connectivity and security challenges when deploying Azure OpenAI in regulated or constrained network environments.

*Author: vnamani*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/using-application-gateway-to-secure-access-to-the-azure-openai/ba-p/4456696)
