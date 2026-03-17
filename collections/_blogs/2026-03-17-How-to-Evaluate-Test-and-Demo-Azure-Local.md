---
date: 2026-03-17 08:12:58 +00:00
section_names:
- azure
feed_name: Thomas Maurer's Blog
primary_section: azure
title: How to Evaluate, Test, and Demo Azure Local
external_url: https://www.thomasmaurer.ch/2026/03/how-to-evaluate-test-and-demo-azure-local/
author: Thomas Maurer
tags:
- Azure
- Azure Jumpstart
- Azure Local
- Azure Local Solutions Catalog
- Azure Sandbox
- Blogs
- Certified Hardware
- Data Residency
- Deployment
- Edge Computing
- Evaluation
- HomeLab
- Hybrid Cloud
- Hyper V
- Jumpstart
- Jumpstart LocalBox
- LocalBox
- Microsoft
- Microsoft Azure
- Nested Virtualization
- On Premises
- Proof Of Concept
- Regulatory Compliance
- Sovereign Cloud
- Virtualization
---

Thomas Maurer explains practical, supported ways to get hands-on with Azure Local before buying certified hardware, including a sandboxed Azure Jumpstart LocalBox setup and a Hyper‑V HomeLab evaluation approach, plus when to move to a real-hardware proof of concept.<!--excerpt_end-->

# How to Evaluate, Test, and Demo Azure Local

Azure Local enables customers to run Azure services on customer-owned infrastructure, bringing Azure’s operations, security, and management model to on-premises, edge, and sovereign environments. This matters in scenarios where **data residency**, **latency**, or **regulatory requirements** apply.

A common question is how to get hands-on with Azure Local **before** moving to certified production hardware. This post outlines supported ways to evaluate and demo Azure Local.

## Option 1: Evaluate Azure Local in an Azure Sandbox with Jumpstart LocalBox

**Video:** Evaluate Azure Local in a Sandbox with Jumpstart LocalBox

- https://youtu.be/fPr-TfHcUVgIn

In this walkthrough, you deploy and explore Azure Local in a **fully sandboxed Azure environment** using **Azure Jumpstart LocalBox**.

Why this approach:

- No physical on-premises hardware required
- Secure, repeatable environment for learning
- Useful for understanding architecture, operations, and management patterns

What you can do with it:

- Learn and validate Azure Local concepts
- Test deployment and configuration scenarios
- Build demos and hands-on labs
- Prepare for real-world Azure Local and sovereign cloud deployments

More info:

- Azure Jumpstart: https://jumpstart.azure.com

## Option 2: Install Azure Local in a HomeLab using a Hyper-V VM

**Video:** Install Azure Local in a HomeLab – Hyper-V VM & Evaluation Guide

- https://www.youtube.com/watch?v=ubNSHqKQISI

If you want a **local, self-managed** setup, you can install Azure Local inside a **Hyper-V virtual machine** using **nested virtualization**.

Good fit for:

- Learning how Azure Local works in practice
- Running demos or proof-of-concepts
- Exploring capabilities before investing in certified hardware

Prerequisites called out in the video include:

- Active Directory
- DNS
- Time servers

The video also covers considerations and limitations of running Azure Local virtually, and when it makes sense compared to certified physical hardware.

Documentation:

- Azure Local documentation: https://learn.microsoft.com/en-us/azure/azure-local/?view=azloc-2602

> Important: This setup is intended for evaluation, learning, and demo purposes only. Production deployments require certified physical hardware from the Azure Local solutions catalog.

- Azure Local solutions catalog: https://azurelocalsolutions.azure.microsoft.com/

## Option 3: Proof of Concept with Real Hardware and Azure Local Partners

When you’re ready to move beyond evaluation and demos, the next step is a **proof of concept (PoC)** using **certified physical hardware**.

Guidance:

- Production requires validated hardware from the Azure Local catalog
- Microsoft recommends working with an experienced **partner and OEM** to design and execute a PoC that matches real requirements

A real-hardware PoC helps you:

- Validate Azure Local on certified, production-ready hardware
- Test performance, resiliency, and operational processes
- Integrate with existing on-premises, edge, or sovereign environments
- Build confidence before a full production rollout

Catalog link:

- https://azurelocalsolutions.azure.microsoft.com/

## Choose the Right Path for Your Scenario

- **Jumpstart LocalBox (Azure sandbox):** fast, cloud-based learning and repeatable demos with automated deployment, but it incurs Azure costs for hosting the sandbox VM and services.
- **Hyper-V HomeLab:** local testing and deeper hands-on exploration, with limited Azure spend depending on what you test (and the 60-day trial period mentioned).

Together, these provide low-friction ways to evaluate, test, and demo Azure Local before moving to production.

Related:

- Azure Local overview video: Thomas Maurer, “Azure Local Overview video” (link provided in the source article)


[Read the entire article](https://www.thomasmaurer.ch/2026/03/how-to-evaluate-test-and-demo-azure-local/)

