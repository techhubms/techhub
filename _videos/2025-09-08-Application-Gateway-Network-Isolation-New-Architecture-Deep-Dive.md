---
layout: "post"
title: "Application Gateway Network Isolation: New Architecture Deep Dive"
description: "This video explores the newly introduced network isolation architecture for Azure Application Gateway. John Savill explains the migration from public to private endpoints, key architectural changes, control plane and data plane separation, implications for WAF, and current limitations. Viewers also get walkthroughs for V1 to V2 migration, flag settings for new deployments, and practical deployment advice tailored for Azure network administrators and architects."
author: "John Savill's Technical Training"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=zQNk1BjhwQI"
viewing_mode: "internal"
feed_name: "John Savill's Technical Training"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCpIn7ox7j7bH_OFj7tYouOQ"
date: 2025-09-08 11:48:33 +00:00
permalink: "/2025-09-08-Application-Gateway-Network-Isolation-New-Architecture-Deep-Dive.html"
categories: ["Azure", "Security"]
tags: ["App Gateway", "App Gateway V2", "Application Gateway", "Azure", "Azure Application Gateway", "Azure Architecture", "Azure Cloud", "Azure Networking", "Azure Security", "Cloud", "Cloud Security", "Control Plane", "Deployment", "Firewall", "Load Balancing", "Microsoft", "Microsoft Azure", "Migration", "Network Isolation", "Private Endpoint", "Security", "Videos", "WAF"]
tags_normalized: ["app gateway", "app gateway v2", "application gateway", "azure", "azure application gateway", "azure architecture", "azure cloud", "azure networking", "azure security", "cloud", "cloud security", "control plane", "deployment", "firewall", "load balancing", "microsoft", "microsoft azure", "migration", "network isolation", "private endpoint", "security", "videos", "waf"]
---

In this detailed session, John Savill guides viewers through the architecture and deployment of network-isolated Azure Application Gateways, emphasizing practical security considerations and new features.<!--excerpt_end-->

{% youtube zQNk1BjhwQI %}

# Application Gateway Network Isolation: New Architecture Deep Dive

## Overview

John Savill delivers an in-depth video focused on Azure Application Gateway’s network isolation capabilities. This new architecture removes the traditional requirement for a public endpoint, addressing several previous limitations and providing enhanced deployment flexibility.

## Key Topics Covered

- **Introduction to App Gateway Network**
  - Distinction between App GW V1 and V2 versions
- **Frontend Configuration**
  - Traditional requirements for public endpoints
  - New support for private deployments
- **Shared Data and Control Plane Implications**
  - How management and data channels are separated
  - Security rules required for operation
- **Adopting the New Network Isolated Architecture**
  - Steps to onboard existing and new Application Gateways
  - Enabling the new features via configuration flags
- **Impact on Deployment Architecture**
  - Changes in deployment flow and resource security
  - Updated control plane traffic paths
- **Working with the Flag Settings**
  - How to set and unset the network isolation flag
  - Note that the change is effective for new deployments only
- **Web Application Firewall (WAF) Support**
  - Differences with WAF in the new architecture
- **Limitations and Considerations**
  - Limitations currently present in the network isolated model
  - Architectural trade-offs

## Migration Guidance

- [App GW v1 to v2 Migration Guide](https://learn.microsoft.com/azure/application-gateway/migrate-v1-v2)
- [Required Security Rules Documentation](https://learn.microsoft.com/azure/application-gateway/configuration-infrastructure#required-security-rules)
- [Network Isolation Official Documentation](https://learn.microsoft.com/azure/application-gateway/application-gateway-private-deployment?tabs=portal#onboard-to-the-feature)

## Resources & Learning

- [Video Whiteboard](https://github.com/johnthebrit/RandomStuff/raw/master/Whiteboards/AppGWNetworkIsolation.png)
- Azure learning, certification, and master class playlists curated for further upskilling.

## Practical Takeaways

- Network isolation enhances security by eliminating the necessity of public endpoints on the Application Gateway.
- New flag-based deployment options allow for greater architectural flexibility.
- There are still limitations to be considered, especially for enterprises transitioning from older models.

## About the Author

John Savill is known for practical and accessible technical Azure training content, providing clear advice for practitioners architecting and securing cloud resources.
