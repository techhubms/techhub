---
layout: "post"
title: "Defending the cloud: Azure neutralized a record-breaking 15 Tbps DDoS attack"
description: "This post describes how Azure DDoS Protection automatically detected and mitigated a massive, multi-vector DDoS attack of 15.72 Tbps—the largest observed in the cloud to date. The article details the attack's origins from the Aisuru IoT botnet, mitigation methods used, and the importance of ongoing readiness and simulation exercises for internet-facing workloads."
author: "Sean_Whalen"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/defending-the-cloud-azure-neutralized-a-record-breaking-15-tbps/ba-p/4470422"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-17 16:00:00 +00:00
permalink: "/2025-11-17-Defending-the-cloud-Azure-neutralized-a-record-breaking-15-Tbps-DDoS-attack.html"
categories: ["Azure", "Security"]
tags: ["Aisuru Botnet", "Australia Endpoint", "Azure", "Azure DDoS Protection", "Azure Infrastructure", "Cloud Security", "Community", "Cybersecurity", "Distributed Denial Of Service", "Internet Facing Applications", "IoT Security", "Mitigation Strategies", "Network Protection", "Operational Readiness", "Provider Enforcement", "Security", "Turbo Mirai", "UDP Flood"]
tags_normalized: ["aisuru botnet", "australia endpoint", "azure", "azure ddos protection", "azure infrastructure", "cloud security", "community", "cybersecurity", "distributed denial of service", "internet facing applications", "iot security", "mitigation strategies", "network protection", "operational readiness", "provider enforcement", "security", "turbo mirai", "udp flood"]
---

Sean_Whalen outlines how Azure DDoS Protection countered a historic 15 Tbps multi-vector attack, explaining the attack methods and proactive security strategies for cloud workloads.<!--excerpt_end-->

# Defending the Cloud: Azure Neutralized a Record-Breaking 15 Tbps DDoS Attack

On October 24, 2025, Azure DDoS Protection automatically detected and mitigated a multi-vector DDoS attack that peaked at 15.72 Tbps and reached nearly 3.64 billion packets per second (pps). This incident marked the largest DDoS event ever observed in cloud infrastructure, specifically targeting a single endpoint in Australia.

## Attack Details

- **Source:** The attack originated from the Aisuru botnet, a Turbo Mirai-class IoT botnet leveraging compromised home routers and cameras (mostly in residential ISPs across the US and globally).
- **Attack Vector:** Extremely high-rate UDP floods focused on a public IP address, launched by over 500,000 distinct source IPs. Minimal source spoofing and randomized source ports helped simplify traceback and allowed providers to enforce controls rapidly.

## Azure's Mitigation Mechanisms

Azure's global DDoS Protection infrastructure detected the attack in real-time:

- **Traffic Filtering:** Malicious packets were automatically filtered and redirected, preventing service disruption.
- **Continuous Detection:** Azure's detection capabilities adjust to evolving attack sizes, ensuring resilience even as IoT device power and consumer bandwidth increase.
- **Customer Impact:** Customer workloads remained available without interruption during mitigation.

## Key Lessons & Recommendations

- **Proactive Readiness:** All internet-facing applications should be verified for sufficient DDoS protection, especially before peak periods such as holidays.
- **Simulations:** Organizations are encouraged to perform regular simulations to assess defenses and operational response capability.

## Reference

For further details, see the official Azure DDoS Protection documentation: [Azure DDoS Protection Overview | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ddos-protection/ddos-protection-overview)

---
*Author: Sean_Whalen*

*Published: November 17, 2025*

*Azure Infrastructure Blog*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/defending-the-cloud-azure-neutralized-a-record-breaking-15-tbps/ba-p/4470422)
