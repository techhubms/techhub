---
layout: "post"
title: "App Gateway for Containers: Web Application Firewall (WAF) Overview"
description: "This video by John Savill’s Technical Training walks through the newly available Web Application Firewall (WAF) feature for Azure’s App Gateway for Containers. The walkthrough covers setup, configuration, policy scopes, key limitations, firewall logs, deployment, and pricing. The presentation targets cloud architects, security professionals, and Kubernetes administrators looking to secure container workloads using native Azure tools."
author: "John Savill's Technical Training"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=CSD1qQN2R2k"
viewing_mode: "internal"
feed_name: "John Savill's Technical Training"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCpIn7ox7j7bH_OFj7tYouOQ"
date: 2025-08-06 13:27:43 +00:00
permalink: "/2025-08-06-App-Gateway-for-Containers-Web-Application-Firewall-WAF-Overview.html"
categories: ["Azure", "Security"]
tags: ["AKS", "App Gateway For Containers", "Application Gateway", "Azure", "Azure Cloud", "Azure Networking", "Azure Service Operator", "Cloud", "Cloud Security", "Container Security", "Firewall", "Firewall Policies", "Kubernetes", "Load Balancing", "Logging", "Microsoft", "Microsoft Azure", "Pricing", "Security", "Security Configuration", "Videos", "WAF", "Web Application Firewall"]
tags_normalized: ["aks", "app gateway for containers", "application gateway", "azure", "azure cloud", "azure networking", "azure service operator", "cloud", "cloud security", "container security", "firewall", "firewall policies", "kubernetes", "load balancing", "logging", "microsoft", "microsoft azure", "pricing", "security", "security configuration", "videos", "waf", "web application firewall"]
---

John Savill's Technical Training explains the new Web Application Firewall (WAF) capability for Azure App Gateway for Containers, guiding viewers through configuration, policy application, and key limitations.<!--excerpt_end-->

{% youtube CSD1qQN2R2k %}

# App Gateway for Containers: Web Application Firewall (WAF) Overview

**Author:** John Savill's Technical Training

## Introduction

Discover how to implement the newly released Web Application Firewall (WAF) feature for Azure App Gateway for Containers (AGC). This detailed walkthrough covers feature overview, setup, best practices, configuration options, policy scopes, and limitations.

## Table of Contents

- App Gateway for Containers Feature Review
- Introduction to WAF for AGC
- Setting Up WAF Policy Resources
- Notable Limitations
- Logging and Diagnostics
- Behind-the-Scenes Architecture
- Step-by-Step Configuration
- Policy Application Scopes
- Applying Configurations
- Fast Update Configuration Flow
- Pricing Breakdown
- Summary and Key Takeaways

## Key Links

- [Video whiteboard](https://github.com/johnthebrit/RandomStuff/raw/master/Whiteboards/AGCWAF.png)
- [AGC WAF Microsoft Docs](https://learn.microsoft.com/azure/application-gateway/for-containers/web-application-firewall#limitations)
- [Firewall Log Format](https://learn.microsoft.com/en-us/azure/application-gateway/for-containers/diagnostics?tabs=configure-log-portal#firewall-log-format)
- [Deployment & Configuration](https://learn.microsoft.com/en-us/azure/application-gateway/for-containers/web-application-firewall#application-gateway-for-containers-implementation)
- [Azure Service Operator](https://azure.github.io/azure-service-operator/)
- [Pricing Example](https://learn.microsoft.com/en-us/azure/application-gateway/for-containers/understanding-pricing#example-6---web-application-firewall)

## Chapter Highlights

### 1. App Gateway for Containers Review

A brief refresher on AGC’s core features, role in Kubernetes architectures, and place in the Azure cloud ecosystem.

### 2. Web Application Firewall for AGC

- Introduction to the WAF capability, its benefits, and scenarios where it strengthens containerized application security.
- Comprehensive demo of WAF policy resource creation and configuration.

### 3. Limitations

A frank discussion about what WAF for AGC currently does **not** support, with references to the official documentation and recommendations for managing these gaps.

### 4. Logging

How to enable and interpret WAF logs, using Azure diagnostic settings and log analytics for insights and troubleshooting.

### 5. Behind the Scenes Plumbing

An inside look at how WAF integrates with container networking and the broader Azure infrastructure.

### 6. Configuration and Policy Application

- Step-by-step configuration guide
- Overview of possible policy application scopes (e.g., specific ingress routes, namespaces, or global policies)

### 7. Fast Update Configuration Flow

Tips for quickly applying and updating WAF settings in a production or staging container environment.

### 8. Pricing

Explanation of the new pricing model for WAF with AGC, with a real-world example and cost considerations.

### 9. Summary

Key takeaways and best practices to secure your Kubernetes workloads running in Azure using App Gateway for Containers with WAF, along with the links for deeper reference and learning.

## Conclusion

AGC’s Web Application Firewall brings enterprise-grade security to containerized workloads on Azure, allowing fine-grained control and visibility over application traffic at scale. For cloud engineers and security architects, understanding the setup, configuration flow, policy options, strict limitations, and associated pricing is crucial to deploying secure cloud-native applications.

For further learning, John Savill offers detailed playlists and curated resources on Azure architecture, DevOps, PowerShell, and Microsoft certification.
