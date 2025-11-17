---
layout: "post"
title: "Azure Front Door Resiliency Deep Dive and Architecting for Mission Critical Applications"
description: "This video provides an in-depth exploration of Azure Front Door's resiliency features and examines how to architect mission-critical applications for high availability using Azure Front Door, Azure Traffic Manager, Traffic Shield, and related services. The presenter discusses configuration layers, change deployment, system and customer configuration, and strategies for minimizing cross-tenant impact, offering practical advice for robust cloud network architecture."
author: "John Savill's Technical Training"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=ufxFlmjS9dU"
viewing_mode: "internal"
feed_name: "John Savill's Technical Training"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCpIn7ox7j7bH_OFj7tYouOQ"
date: 2025-11-17 10:53:28 +00:00
permalink: "/2025-11-17-Azure-Front-Door-Resiliency-Deep-Dive-and-Architecting-for-Mission-Critical-Applications.html"
categories: ["Azure"]
tags: ["Application Acceleration", "Azure", "Azure Cloud", "Azure Front Door", "Azure Traffic Manager", "CDN", "Cloud", "Cloud Architecture", "Cloud Network", "Cross Tenant Impact", "DNS", "High Availability", "Microsoft", "Microsoft Azure", "Mission Critical", "Mission Criticl", "Networking", "Resiliency", "System Configuration", "Traffic Shield", "Videos"]
tags_normalized: ["application acceleration", "azure", "azure cloud", "azure front door", "azure traffic manager", "cdn", "cloud", "cloud architecture", "cloud network", "cross tenant impact", "dns", "high availability", "microsoft", "microsoft azure", "mission critical", "mission criticl", "networking", "resiliency", "system configuration", "traffic shield", "videos"]
---

John Savill provides a comprehensive deep dive into Azure Front Door resiliency, architecture options for mission-critical cloud applications, and practical strategies for ensuring high availability on Microsoft Azure.<!--excerpt_end-->

{% youtube ufxFlmjS9dU %}

# Azure Front Door Resiliency Deep Dive and Architecting for Mission Critical Applications

**Presented by: John Savill’s Technical Training**

This session focuses on designing highly available, resilient applications using Azure Front Door (AFD) and supporting Azure networking services. The video breaks down:

## Table of Contents

- **AFD Refresher:** Overview of Azure Front Door’s role in network traffic management
- **How is AFD Resilient:** Examines resiliency features, including:
  - Front end and fallback layers
  - Traffic Shield
  - DNS integration and protection mechanisms
- **Configuration Layers:**
  - System, Data, and Customer configurations
  - Removing asynchronous processes to improve reliability
- **Change Deployment:** Best practices for deploying changes to mitigate downtime
- **Mission Critical Service Considerations:**
  - Designing for both CDN and non-CDN scenarios
  - Reducing cross-tenant impact on high-demand workloads
- **Comparisons:**
  - Scenarios leveraging Azure Traffic Manager, DNS, and Front Door together
  - CDN vs. non-CDN service architectures
- **Reliability Reviews:**
  - Regularly reviewing architecture decisions for resiliency impact
- **Practical Resources:**
  - Whiteboard diagrams for visual reference ([link](https://github.com/johnthebrit/RandomStuff/raw/master/Whiteboards/AFDResilience.png))
  - Links to learning resources, playlist series, and further training

## Key Topics

- Ensuring high availability for mission-critical workloads on Azure
- Improving failover and fallback strategies within Azure’s network stack
- Use of Traffic Shield and DNS to protect and accelerate web applications
- Configuration guidance for reducing operational risks

## Further Learning

- [Learning Path for Azure](https://learn.onboardtoazure.com)
- [Weekly Azure Update](https://youtube.com/playlist?list=PLlVtbbG169nEv7jSfOVmQGRp9wAoAM0Ks)
- [DevOps Master Class](https://youtube.com/playlist?list=PLlVtbbG169nFr8RzQ4GIxUEznpNR53ERq)

For additional technical details, refer to the whiteboard diagrams and playlist series linked above.

---

**Note:** For subtitles and auto-translate options, refer to the video’s settings.
