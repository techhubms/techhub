---
external_url: https://www.microsoft.com/en-us/research/blog/breaking-the-networking-wall-in-ai-infrastructure/
title: 'MOSAIC: A Wide-and-Slow MicroLED Network Architecture for Next-Gen AI Infrastructure'
author: stclarke
feed_name: Microsoft News
date: 2025-09-09 17:05:53 +00:00
tags:
- AI Infrastructure
- Company News
- Datacenter Networking
- Energy Efficiency
- GPU Interconnect
- Hardware Co Design
- Microled
- Microsoft Research
- MOSAIC
- Network Bottleneck
- Optical Interconnect
- Optical Transmission
- Parallel Low Speed Channels
- SIGCOMM
- Wide And Slow Architecture
section_names:
- ai
- azure
---
stclarke describes how Microsoft’s MOSAIC project uses microLED arrays in a wide-and-slow optical interconnect to dramatically improve AI infrastructure scalability, power efficiency, and reliability.<!--excerpt_end-->

# MOSAIC: A Wide-and-Slow MicroLED Network Architecture for Next-Gen AI Infrastructure

## Breaking the Networking Wall in AI Infrastructure

Modern AI systems face severe constraints due to memory and network bottlenecks that limit overall GPU utilization and datacenter efficiency. These bottlenecks persist despite significant hardware investments, largely because traditional network interconnects must trade off between power, reliability, and communication reach.

Datacenters typically rely on two cable types: copper links (power-efficient but with limited range) and optical links (power-hungry, complex electronics, but scalable distance). Each suffers from limitations as data speeds increase, leading to performance bottlenecks akin to the historical "memory wall." To overcome these issues and scale up AI workloads efficiently, new interconnect approaches are required.

## Introducing MOSAIC

The Microsoft Research, Azure, and M365 teams collaborated to develop MOSAIC, a "wide-and-slow" optical architecture using advanced microLED technology. Instead of a small number of high-speed serial channels (the "narrow-and-fast" approach), MOSAIC employs hundreds of parallel low-speed channels, each using directly modulated microLEDs rather than traditional expensive and power-hungry lasers.

### Key Benefits of MOSAIC

- **Power Efficiency:** Low-speed operation eliminates the need for complex electronics and reduces overall optical power requirements, offering up to 68% power savings (over 10W per cable).
- **Extended Reach:** Optical microLED links can span up to 50 meters—over 10x further than copper cables—while maintaining efficiency.
- **Reliability and Scalability:** The simple, temperature-insensitive structure of microLEDs supports redundant paths, reducing failure rates up to 100x compared to conventional optical links. Aggregate speeds (1.6 Tbps or 3.2 Tbps) are easily achieved by scaling the array.

### Technology Details

- **MicroLED Arrays:** Smaller and faster than traditional LEDs, microLEDs can be modulated at several Gbps. For example, a 20×20 array provides 800 Gbps and fits within a ~1 mm^2 silicon die.
- **Innovation in Fiber and Optics:** Imaging fibers supporting thousands of paths per fiber enable practical deployment of these wide arrays, avoiding the complexity and cost of individual fibers per channel.
- **Protocol Compatibility:** MOSAIC functions as a drop-in replacement for current copper and optical cables. It is protocol-agnostic (works with Ethernet, PCIe, CXL) and compatible with existing transceivers and hardware setups.
- **Overcoming Optical Challenges:** Challenges, such as microLEDs’ broader spectrum and beam shape, were solved by custom lens and analog circuit designs.

### Industry Impact

- **Infrastructure Redesign:** The gains in power, reliability, and scale allow for new datacenter architectures. Less reliance on ultra-dense racks enables flexibility in network design, resource disaggregation, and higher GPU memory bandwidth.
- **Research and Production:** Microsoft is productizing MOSAIC with industry partners and has published results in the SIGCOMM paper: [MOSAIC: Breaking the Optics versus Copper Trade-off with a Wide-and-Slow Architecture and MicroLEDs](https://www.microsoft.com/en-us/research/publication/mosaic-breaking-the-optics-versus-copper-trade-off-with-a-wide-and-slow-architecture-and-microleds/).
- **Environmental Impact:** With potential annual savings of 100MW across global deployments, MOSAIC could power more than 300,000 homes.

## Further Reading and Research Copilot

For more about this and related research projects, explore the [Microsoft Research Copilot experience](https://aka.ms/research-copilot/?OCID=msr_researchforum_Copilot_MCR_Blog_Promo).

---

**References:**

- [MOSAIC SIGCOMM Paper](https://www.microsoft.com/en-us/research/publication/mosaic-breaking-the-optics-versus-copper-trade-off-with-a-wide-and-slow-architecture-and-microleds/)
- [Good things come in small packages? Should we adopt “lite” GPUs in AI infrastructure?](https://www.microsoft.com/en-us/research/publication/good-things-come-in-small-packages-should-we-adopt-lite-gpus-in-ai-infrastructure/)
- [Storage-class memory is dead. All hail managed retention memory: Rethinking memory for the AI era](https://www.microsoft.com/en-us/research/publication/storage-class-memory-is-dead-all-hail-managed-retention-memory-rethinking-memory-for-the-ai-era/)

---

*Authored by stclarke*

This post appeared first on "Microsoft News". [Read the entire article here](https://www.microsoft.com/en-us/research/blog/breaking-the-networking-wall-in-ai-infrastructure/)
