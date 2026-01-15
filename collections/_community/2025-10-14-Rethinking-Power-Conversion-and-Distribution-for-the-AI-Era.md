---
layout: post
title: Rethinking Power Conversion and Distribution for the AI Era
author: EhsanNasr
canonical_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/rethinking-power-conversion-and-distribution-for-the-ai-era/ba-p/4460759
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-10-14 00:00:00 +00:00
permalink: /azure/community/Rethinking-Power-Conversion-and-Distribution-for-the-AI-Era
tags:
- AI
- AI Infrastructure
- Azure
- Cloud Infrastructure
- Community
- Data Center Architecture
- Disaggregated Power
- Energy Efficiency
- Energy Technology
- GPU Clusters
- High Voltage DC
- Microsoft
- Mt Diablo
- Power Conversion
- Power Distribution
- Rack Density
- Solid State Transformer
- SST
section_names:
- azure
---
Ehsan Nasr of Microsoft explores how data centers must rethink power systems for the AI era, focusing on the adoption of solid-state transformers, centralized high-voltage DC delivery, and innovative architectures to enable high-density, AI-ready infrastructure.<!--excerpt_end-->

# Rethinking Power Conversion and Distribution for the AI Era

*By Ehsan Nasr, Director, Energy Technology, Microsoft*

## Introduction

As the adoption of AI accelerates, data centers are facing dramatic increases in IT equipment density—especially with large-scale GPU cluster deployments. This surge places substantial new demands on power conversion and distribution systems, challenging legacy electrical architectures and necessitating a fresh approach.

## The Inflection Point

Traditional data center power design, centered on AC transformers and distributed conversion, struggles to keep pace with the performance requirements of high-voltage GPU racks. Questions arise: Can current infrastructure support growing rack loads, or is a full-stack architectural rethink essential for both performance and energy efficiency?

## Microsoft's Approach: Disaggregated Power Architecture

Microsoft has initiated a paradigm shift by moving power conversion out of individual servers and into centralized rack-level modules. The [Mt Diablo disaggregated power specification](https://techcommunity.microsoft.com/blog/azureinfrastructureblog/mt-diablo---disaggregated-power-fueling-the-next-wave-of-ai-platforms/4268799), announced in partnership with Meta and Google, enables:

- Centralized power delivery at ±400VDC and 800VDC
- More efficient, scalable high-capacity power distribution across racks
- Enhanced support for AI and future high-density workloads

## Solid-State Transformers (SSTs): Core Technology Enabler

Solid-state transformers represent a new class of technology that operates at much higher frequencies than conventional units (tens to hundreds of kHz vs. 50/60 Hz). Benefits include:

- **Direct high-voltage DC to racks:** SSTs can handle medium-voltage AC (<35kV) and deliver DC (<1500VDC) straight to IT racks
- **Infrastructure simplification:** Reduces need for MV-to-LV transformers and associated equipment, shrinking footprint
- **Improved energy efficiency:** Lowers conversion losses, potentially reduces costs, boosts ROI
- **Flexibility:** Supports integration with energy storage and backup for enhanced reliability
- **Compactness:** Much smaller and lighter than traditional chains, ideal for constrained spaces

## Looking Ahead

The adoption of SSTs is just one component in a broader architectural transformation. By reimagining everything from power distribution to protection and conversion, Microsoft aims to build data centers with the resilience, efficiency, and flexibility required for ongoing AI-driven progress and extreme rack density.

## Next Steps

- Explore further innovations such as SSTs for data center modernization
- Engage with the broader ecosystem driving power technology advancement
- Watch for case studies, open specifications, and benchmarks as AI-scale requirements mature

---

*For further reading, see Microsoft's [Mt Diablo Disaggregated Power Platform](https://techcommunity.microsoft.com/blog/azureinfrastructureblog/mt-diablo---disaggregated-power-fueling-the-next-wave-of-ai-platforms/4268799) and [detailed technical spec](https://www.opencompute.org/documents/ocp-specification-diablo-400-v0p5p2-2025-05-30-pdf). CONNECT with leaders shaping the future of AI infrastructure.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/rethinking-power-conversion-and-distribution-for-the-ai-era/ba-p/4460759)
