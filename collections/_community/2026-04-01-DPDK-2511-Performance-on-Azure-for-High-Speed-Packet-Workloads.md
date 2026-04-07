---
tags:
- Accelerated Networking
- Azure
- Azure Boost
- Azure VM SKUs
- Community
- CPU Pinning
- DPDK 25.11
- DPDK Performance Report
- High Performance Networking
- Hugepages
- Latency Jitter
- Linux On Azure
- Microsoft Azure
- Microsoft NIC
- Multi Queue
- NUMA
- Packet Processing
- Pktgen Dpdk
- PMD Threads
- Testpmd
- Throughput Benchmarking
- VNF
title: DPDK 25.11 Performance on Azure for High-Speed Packet Workloads
external_url: https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/dpdk-25-11-performance-on-azure-for-high-speed-packet-workloads/ba-p/4424905
primary_section: azure
feed_name: Microsoft Tech Community
author: KashanK
date: 2026-04-01 18:37:04 +00:00
section_names:
- azure
---

KashanK shares an Azure-focused summary of a new DPDK 25.11 performance report, explaining what was tested (Azure Boost, Accelerated Networking, NUMA/hugepages) and the practical tuning recommendations for high-throughput packet workloads on Azure VMs.<!--excerpt_end-->

## Sharing DPDK Performance Insights on Azure

At Microsoft Azure, performance is treated as an ongoing discipline grounded in careful engineering and real-world validation. As cloud workloads grow in scale and variety, customers depend on consistent, high-throughput networking. Technologies such as the Data Plane Development Kit (DPDK) play a key role in meeting these expectations.

To support customers running advanced network functions, Microsoft released a performance report based on **DPDK 25.11**, available in the DPDK performance catalog: [Microsoft Azure DPDK Performance Report](https://fast.dpdk.org/doc/perf/DPDK_25_11_Microsoft_NIC_performance_report.pdf). The report focuses on how DPDK performs on **Microsoft-developed Azure Boost** within Azure infrastructure, including packet processing behavior across scenarios (small packets through multi-core scaling).

## Why test DPDK on Azure

DPDK is widely used for high-performance packet processing in virtualized environments, including:

- Customer-deployed virtual network functions (VNFs)
- Internal Azure network appliances

The post emphasizes that enabling DPDK alone is not enough; performance should be validated under realistic cloud conditions, including:

- Azure VM configurations with **Accelerated Networking**
- **NUMA-aware** memory and CPU alignment
- **Hugepage-backed** memory allocation
- Multi-core **PMD thread** scaling
- Packet forwarding using real traffic generators

## What the report covers

The DPDK 25.11 report includes performance benchmarks across frame sizes from **64 bytes to 1518 bytes**, and evaluates:

- CPU usage
- Queue configuration
- Latency stability under different test conditions

### Key report highlights

- Line-rate throughput can be achievable at common frame sizes when **vCPUs are pinned correctly** and **memory is configured** properly.
- Low jitter and consistent latency are observed across multi-queue and multi-core tests.
- Performance scales nearly linearly with additional cores (especially for smaller packet sizes).
- Queue and PMD thread alignment with the **NUMA layout** plays a major role in maximizing efficiency.

All tests were performed using Azure VM SKUs equipped with **Microsoft NICs** and configured for isolation and performance.

## Why share the report

The stated motivation is performance transparency and ecosystem collaboration:

- Helps customers plan and tune workloads using validated performance envelopes
- Enables optimization of drivers, firmware, and applications based on real-world data
- Encourages reproducibility and standardization in cloud DPDK benchmarking
- Creates a feedback loop between Azure, the DPDK community, and partners

## Recommendations for running DPDK on Azure

Based on the test results, the post provides these best practices.

| Area | Recommendation |
| --- | --- |
| VM Selection | Choose Accelerated Networking-enabled SKUs like D, Fsv2, or Eav4 |
| CPU Pinning | Use dedicated cores for PMD threads and align with NUMA topology |
| Memory | Configure hugepages and allocate memory from the local NUMA node |
| Queue Mapping | Match RX and TX queues to available vCPUs to avoid contention |
| Packet Generator | Use pktgen-dpdk or testpmd with controlled traffic profiles |

These settings are presented as ways to improve consistency and peak throughput across many DPDK scenarios.

## Get involved and reproduce the results

- Download the report: [Microsoft Azure DPDK Performance Report](https://fast.dpdk.org/doc/perf/DPDK_25_11_Microsoft_NIC_performance_report.pdf)
- Replicate the setup using Azure VMs and a preferred packet generator: https://github.com/mcgov/dpdk-perf
- Share feedback via GitHub/community channels or email: [dpdk@microsoft.com](mailto:dpdk@microsoft.com)
- Suggest improvements or contribute new scenarios for future performance reports

## Conclusion

DPDK is positioned as an enabler for high-performance cloud networking. The post’s goal is to make Azure DPDK performance data open and actionable, reflecting continued investment in validating and improving infrastructure for mission-critical workloads.

[Read the entire article](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/dpdk-25-11-performance-on-azure-for-high-speed-packet-workloads/ba-p/4424905)

