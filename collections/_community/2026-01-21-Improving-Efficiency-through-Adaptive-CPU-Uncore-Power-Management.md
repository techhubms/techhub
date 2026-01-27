---
external_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/improving-efficiency-through-adaptive-cpu-uncore-power/ba-p/4486456
title: Improving Efficiency through Adaptive CPU Uncore Power Management
author: PulkitMisra
feed_name: Microsoft Tech Community
date: 2026-01-21 16:00:00 +00:00
tags:
- Cloud Infrastructure
- CPU Frequency Scaling
- Datacenter Efficiency
- Efficiency Latency Control
- ELC
- Granite Rapids
- Hardware/software Co Design
- Intel Xeon 6
- Microsoft Azure
- Performance Per Watt
- Power Management
- Server Sustainability
- SPEC CPU
- Uncore Power Management
section_names:
- azure
primary_section: azure
---
PulkitMisra highlights how Microsoft Azure and Intel collaborated to improve datacenter power efficiency through adaptive CPU uncore management with Efficiency Latency Control (ELC).<!--excerpt_end-->

# Improving Efficiency through Adaptive CPU Uncore Power Management

**By Microsoft Azure and Intel**

Microsoft Azure continually seeks to strike a balance between maximizing performance and improving power efficiency. Smarter power management allows Azure to deploy more servers within a fixed datacenter footprint, supporting sustainability and meeting increasing compute demands.

## What is Uncore Power Management?

Modern server CPUs comprise not only compute cores but also 'uncore' components like the mesh interconnect, memory controllers, and I/O subsystems. While idle CPU cores can enter deep power-saving states, the uncore typically remains active at full frequency, resulting in unnecessary power consumption, especially during periods of low load (e.g., overnight or during weekends).

### Why Focus on Uncore?

- Many Azure workloads, such as Microsoft Teams, are subject to diurnal usage patterns.
- Servers are often over-provisioned for peak loads, idling at other times.
- Even off-peak, uncore subsystems remain active if any core is active (for tasks like monitoring), leading to inefficient power usage.

Although CPUs can scale uncore frequency dynamically, traditional software-based techniques are slow to respond to sudden spikes in demand.

## Efficiency Latency Control (ELC): A Hardware/Software Solution

Azure and Intel addressed these limitations by co-designing Efficiency Latency Control (ELC), now available on Intel Xeon 6 'Granite Rapids' processors. ELC enables software to communicate CPU utilization thresholds and frequency targets directly to hardware firmware, blending flexible configuration with rapid hardware response.

- **Three frequency points**: Low, Mid, High
- **Two utilization thresholds**: Low, High
- At low utilization, firmware holds uncore frequency at its minimum to maximize savings.
- As utilization rises, the frequency increases, optimizing for performance.

### ELC Configuration Strategies

- **Latency-Optimized**: High frequency at all utilizations—maximum responsiveness, higher power use
- **Efficiency-Optimized**: Lower frequency during idle, higher efficiency, tolerable performance tradeoff
- **Balanced/Configurable**: Intermediate approach for moderate efficiency with acceptable performance

## Real-World Impact

- **Power Savings**: Up to 11% reduction in power under moderate loads while maintaining comparable performance.
- **Better Performance/Watt**: Up to 1.5× improvement during low-storage loads by lowering the uncore frequency while workload is light.
- These improvements help Azure boost the number of servers deployed per datacenter, keeping pace with customer demand without additional power consumption.

## Future Directions

As workloads evolve, continued hardware–software collaboration will be key to further advances in datacenter efficiency and sustainability. Integration of smart hardware controls like ELC with cloud management software is a major milestone for Microsoft Azure.

## Appendix

For detailed technical coverage, see Intel's article: [Intel® Xeon® 6 Processors - Performance and Power Profiles - Default, Latency-Optimized Mode, and Other Options Technical Article](https://www.intel.com/content/www/us/en/content-details/826934/intel-xeon-6-processors-performance-and-power-profiles-default-latency-optimized-mode-and-other-options-technical-article.html)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/improving-efficiency-through-adaptive-cpu-uncore-power/ba-p/4486456)
