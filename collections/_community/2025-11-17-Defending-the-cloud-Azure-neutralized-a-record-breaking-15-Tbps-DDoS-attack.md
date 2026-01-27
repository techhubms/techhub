---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/defending-the-cloud-azure-neutralized-a-record-breaking-15-tbps/ba-p/4470422
title: 'Defending the cloud: Azure neutralized a record-breaking 15 Tbps DDoS attack'
author: Sean_Whalen
feed_name: Microsoft Tech Community
date: 2025-11-17 16:00:00 +00:00
tags:
- Aisuru Botnet
- Australia Endpoint
- Azure DDoS Protection
- Azure Infrastructure
- Cloud Security
- Cybersecurity
- Distributed Denial Of Service
- Internet Facing Applications
- IoT Security
- Mitigation Strategies
- Network Protection
- Operational Readiness
- Provider Enforcement
- Turbo Mirai
- UDP Flood
section_names:
- azure
- security
primary_section: azure
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
