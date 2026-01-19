---
external_url: https://devclass.com/2025/11/20/inside-the-clouds-shift-to-arm-why-hyperscalers-and-the-industry-are-making-the-switch/
title: "Microsoft Azure's Shift to Arm: Performance Gains and Developer Migration Strategies"
author: David Gordon
viewing_mode: external
feed_name: DevClass
date: 2025-11-20 09:00:00 +00:00
tags:
- AI Workloads
- ARM 22824 Sp1
- ARM Neoverse
- Arm64 Runners
- Cloud Migration
- Cobalt 100
- Cobalt 200
- Databricks
- Developer Tools
- Efficiency
- Kubernetes
- Microsoft Azure
- ONNX Runtime
- Price Performance
- Snowflake
- Virtual Machines
- VMs
section_names:
- ai
- azure
- coding
- ml
---
David Gordon explores the major transition toward Arm-based architectures in cloud computing, focusing on Microsoft Azure's Cobalt series. The article outlines performance benchmarks, developer migration strategies, and efficiency gains for ML and AI workloads.<!--excerpt_end-->

# Microsoft Azure's Shift to Arm: Performance Gains and Developer Migration Strategies

Arm, traditionally associated with mobile devices, is now reshaping cloud infrastructure thanks to its efficient and performant architecture. Cloud hyperscalers—including Microsoft Azure, AWS, and Google Cloud—are increasingly deploying Arm Neoverse-based processors for critical workloads, driving improvements in price-performance and energy efficiency.

## Arm Adoption in Microsoft Azure

**Microsoft Azure's Cobalt 100 VMs** have demonstrated:

- Up to **48% higher performance** and **91% better price-performance** in database workloads
- Significant gains in secure networking and financial modeling over comparable x86 instances
- In AI workloads, benchmarks show **1.9x higher performance** and **2.8x better price-performance** running LLM inference with ONNX Runtime

At **Microsoft Ignite**, Azure introduced the **Cobalt 200**, its next-generation Arm-based CPU. It delivers up to **50% higher performance** than Cobalt 100 and incorporates security, networking, and storage improvements.

## Cloud-Wide Migration Trends

- Google has ported tens of thousands of applications—including major services like YouTube, Gmail, and BigQuery—to Arm instances, reporting up to 65% better price-performance and 60% improved energy efficiency.
- AWS's Graviton4 processors outpace x86 for both general cloud workloads and ML training (e.g., 53% faster training times in XGBoost ML and 64% better price-performance).
- Enterprise adopters, including Databricks and Snowflake, leverage Cobalt 100 for optimized cloud analytics.

## Developer Migration: Strategies and Practical Advice

Migrating to Arm is now more accessible:

- Compile for **Arm64**, deploy stateless microservices, API backends, or stream processing workloads on Arm instances.
- Use A/B production testing to monitor latency and performance—track cost per request and watts per request, not just CPU usage.
- Microsoft's ecosystem provides first-class support: Azure VMs, Kubernetes, cloud-native tooling, and extensive onboarding documentation.
- GitHub offers Arm64 runners and Copilot extensions to assist in code migration and validation.

### Migration Steps

1. Select a low-risk service tier for migration (e.g., stateless microservices).
2. Ensure code compatibility with Arm64; leverage major frameworks with built-in support.
3. Run side-by-side production traffic tests.
4. Validate improvements in performance and cost metrics.

## Industry Results

- **Spotify** achieved a 250% boost in performance with Axion-backed VMs and 40% cost reduction.
- **Paramount Global** recorded 33% faster content encoding on Axion-based Azure VMs.
- **Pinterest** saw 47% lower workload costs and reduced carbon impact migrating to Graviton on AWS.

## AI and ML Workloads

Arm platforms offer clear advantages for AI inference (LLM, ONNX Runtime), ML training (XgBoost, Databricks), and advanced analytics workloads. Developers targeting these pipelines benefit from price, power, and scaling improvements—especially within the Azure ecosystem.

## Resources

- [Arm Cloud Migration Program](https://www.arm.com/markets/computing-infrastructure/arm-cloud-migration)
- [Azure Cobalt 100 VMs announcement](https://newsroom.arm.com/blog/microsoft-azure-cobalt-100-vms-arm-neoverse)
- [ONNX Runtime on Arm Neoverse-powered Azure VMs](https://developer.arm.com/community/arm-community-blogs/b/servers-and-cloud-computing-blog/posts/accelerate-llm-inference-with-onnx-runtime-on-arm-neoverse-powered-microsoft-cobalt-100)

## Conclusion

Arm is now integral in hyperscaler cloud infrastructure. The combination of improved performance, lower cost, and energy savings—demonstrated with Microsoft Azure's Cobalt series—makes migrating workloads an attractive proposition for developers working with ML, AI, and general-purpose applications. The mature toolchain and large ecosystem further lower migration barriers.

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2025/11/20/inside-the-clouds-shift-to-arm-why-hyperscalers-and-the-industry-are-making-the-switch/)
