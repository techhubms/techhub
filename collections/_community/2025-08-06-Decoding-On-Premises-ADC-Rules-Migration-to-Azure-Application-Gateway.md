---
layout: post
title: 'Decoding On-Premises ADC Rules: Migration to Azure Application Gateway'
author: vnamani
canonical_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/decoding-on-premises-adc-rules-migration-to-azure-application/ba-p/4439156
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-08-06 18:41:57 +00:00
permalink: /azure/community/Decoding-On-Premises-ADC-Rules-Migration-to-Azure-Application-Gateway
tags:
- ADC Migration
- Application Security
- ARM Templates
- Azure Application Gateway
- Azure Front Door
- Azure Monitor
- Citrix Policy Expressions
- Cloud Networking
- Geo Blocking
- Header Rewrite
- IaC
- Irules
- Load Balancing
- Terraform
- Traffic Management
- WAF
- Web Application Firewall
- Zone Redundancy
section_names:
- azure
- devops
- security
---
vnamani delivers a comprehensive walkthrough on migrating on-premises ADC rules to Azure Application Gateway, breaking down feature mapping, Azure-native alternatives, and how to leverage infrastructure-as-code and cloud security best practices.<!--excerpt_end-->

# Decoding On-Premises ADC Rules: Migration to Azure Application Gateway

## Overview

As Azure Application Gateway matures, many organizations consider shifting from legacy, on-premises ADC solutions (e.g., F5, NetScaler, Radware) to Azure’s cloud-native services. This transition is driven by the growing demand for agility, scalability, and tighter integration with Azure-native tools such as Web Application Firewall (WAF), Azure Front Door, and Azure Firewall.

A common concern is whether Application Gateway can support complex load balancing configurations. The answer depends on individual use cases, but with a methodical approach, migrations can be seamless and secure.

## Key Capabilities of Application Gateway v2

Azure Application Gateway v2 offers significant enhancements for modern architectures:

- Autoscaling and zone redundancy for high availability
- Built-in Web Application Firewall and Azure DDoS Protection
- Native header rewrite, URL-based routing, and SSL termination
- Integration with Azure Monitor, Log Analytics, and Defender for Cloud
- Deployment automation with ARM/Bicep, CLI, GitOps, Terraform, and CI/CD pipelines

These features simplify operations and bolster both security and agility for organizations embracing the cloud-first model.

## Understanding ADC Rules

Traditional, on-premises ADCs like F5 and Citrix provide robust traffic and security management through features such as iRules and policy expressions. These custom scripts and policies enable advanced traffic manipulation and are often deeply embedded in enterprise workflows.

### Common ADC Scenarios

- Redirects and header rewrites
- IP filtering and geo-blocking
- Custom error handling
- Event-driven logic (e.g., HTTP_REQUEST, CLIENT_ACCEPTED)

Azure Application Gateway supports many of these requirements out of the box, but direct, line-by-line translation is rare. Instead, migration focuses on mapping intentions to Azure-native equivalents.

## Application Gateway Feature Mapping

Below is a comparison of on-premises ADC features to Azure equivalents:

| Citrix Features            | iRule Feature            | App Gateway v2 Equivalent              | Supported? |
|---------------------------|--------------------------|-----------------------------------------|------------|
| Responder Policies        | Redirects (301/302)      | Native redirect rules                   | ✅         |
| Rewrite Policies          | Header rewrites          | Rewrite Set rules                       | ✅         |
| GSLB + Responder Policies | Geo-based Routing        | Use with Azure Front Door               | ✅         |
| Content Switching Policies| URL-based routing        | Path-based routing rules                | ✅         |
| Responder/ACLs            | IP filtering             | WAF custom rules/NSGs                   | ✅         |
| GSLB + Policy Expressions | Geo-blocking             | WAF rules                               | ✅         |
| Content Switching Policies| Path-based routing       | URL path maps                           | ✅         |
| Content Switching/Rewrite | Header-based Routing     | Limited via parameter-based paths        | ➗         |
| Adv. Policy Expressions   | Regex-based routing      | Limited regex support                   | ➗         |
| Priority Queues/Rate Ctrl | Real-time traffic shaping| Limited via Azure Front Door            | ➗         |
| AppExpert TCP Expressions | TCP payload inspection   | Not supported                           | ❌         |
| Event-driven hooks        | HTTP_REQUEST, etc        | Not supported                           | ❌         |
| Full scripting            | TCL                      | Not supported                           | ❌         |

**Note:** When migrating WAF rules, it is a best practice to start with detection mode to identify false positives before switching enforcement on.

## Translating Advanced Rules

Migration is about intent, not line-by-line conversion. Here’s how to proceed:

- **Tool-assisted translation:** Use Copilot or GPT tools to interpret and reformat common ADC patterns.
- **Inventory and analyze:** Decompose complex rules into manageable Application Gateway configurations (such as redirects and rewrites).
- **Documentation:** Clearly articulate each original rule’s objective and its new implementation on Azure.

## Configuring Logic in Azure

You can implement routing and rewriting logic using the following tools and interfaces:

- Azure Portal
- Azure CLI or PowerShell (e.g., `az network application-gateway`)
- ARM Templates or Bicep for infrastructure-as-code
- REST API for automation and CI/CD pipeline integration

**Example: Configure a Header Rewrite Using Azure Portal**

1. Open your Application Gateway in the [Azure Portal](https://portal.azure.com/)
2. Navigate to **Rewrites**
3. Click **+ Add Rewrite Set** and apply it to your desired rule
4. Define required rewrite conditions and actions

For more on header rewrites: [Rewrite HTTP Headers](https://learn.microsoft.com/en-us/azure/application-gateway/rewrite-http-headers-url)

## Key Resources

- [App Gateway v1 to v2 Migration](https://learn.microsoft.com/en-us/azure/application-gateway/migrate-v1-v2)
- [Well-Architected Guide for Application Gateway v2](https://learn.microsoft.com/en-us/azure/well-architected/service-guides/azure-application-gateway)
- [Rewriting HTTP Headers & URLs](https://learn.microsoft.com/en-us/azure/application-gateway/rewrite-http-headers-url)
- [Header-based Routing](https://learn.microsoft.com/en-us/azure/application-gateway/parameter-based-path-selection-portal)
- [WAF Rule Tuning for Azure Front Door](https://learn.microsoft.com/en-us/azure/web-application-firewall/afds/waf-front-door-tuning?pivots=front-door-standard-premium)

## Conclusion

While AI-powered assistants can streamline initial rule translations, manual review and adaptation are vital to account for enterprise nuances. A thoughtful migration to Azure Application Gateway v2 offers clear operational and security benefits, paving the way for robust, scalable, cloud-native architectures.

For a successful outcome, leverage Azure’s documentation and consult with experts when needed.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/decoding-on-premises-adc-rules-migration-to-azure-application/ba-p/4439156)
