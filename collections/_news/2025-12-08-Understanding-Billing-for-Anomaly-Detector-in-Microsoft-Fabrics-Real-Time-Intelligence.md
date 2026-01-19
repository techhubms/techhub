---
layout: post
title: Understanding Billing for Anomaly Detector in Microsoft Fabric’s Real-Time Intelligence
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/billing-for-anomaly-detector-in-real-time-intelligence/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-12-08 10:00:00 +00:00
permalink: /ml/news/Understanding-Billing-for-Anomaly-Detector-in-Microsoft-Fabrics-Real-Time-Intelligence
tags:
- Anomaly Detector
- Billing
- Capacity Metrics
- Capacity Units
- Data Monitoring
- Eventhouse
- Microsoft Fabric
- Operational Analytics
- Real Time Intelligence
- Streaming Data
section_names:
- azure
- ml
---
Microsoft Fabric Blog outlines key billing changes for customers using the Anomaly Detector in Real-Time Intelligence, explaining what users like you need to know about capacity-based billing, query execution charges, and resource usage reporting.<!--excerpt_end-->

# Understanding Billing for Anomaly Detector in Microsoft Fabric’s Real-Time Intelligence

Microsoft Fabric's Real-Time Intelligence (RTI) is enhancing its anomaly detection capabilities, and there are important updates regarding how customers will be billed for this feature, which is currently in Preview.

## What Is Anomaly Detector in RTI?

Anomaly Detector enables users to quickly and easily detect unusual patterns in streaming data from Eventhouse tables without requiring extensive data science expertise. The feature is integrated for seamless monitoring of both historical and live streaming datasets.

## Key Billing Changes

- **Starting in December:** Any use of Anomaly Detector will accrue charges.
- **New Dedicated Billing Meter:**
  - **Meter Name:** *Anomaly Detector Queries Capacity Usage CU*
  - **Operation Name:** *Anomaly Detection Run Queries*
- **How Billing Works:**
  - Billing is based on the Capacity Unit (CU) consumption from running anomaly detection queries, regardless of whether queries are for interactive analytics or background monitoring.
  - Query execution drives CU consumption—not the size of your underlying data.
  - Costs become easier to predict since they are tied to query volume and usage.

## Where to View Usage

- **Microsoft Fabric Capacity Metrics App:** See all your anomaly detection query usage broken out under its own meter.
- **Azure Billing Reports:** Usage is also reported in your broader Azure billing dashboard, providing consolidated visibility.

## Why This Matters

- **Transparency:** You’'ll be able to monitor the exact resource consumption for anomaly detection tasks.
- **Predictability:** By tying billing to queries rather than data size, organizations can better estimate monthly costs.
- **Actionable Insights:** Real-time anomaly detection billing helps businesses keep streaming data monitored without surprise charges.

For further information and documentation, visit the official [Anomaly Detector billing guide](https://aka.ms/ADBillingDoc).

---

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/billing-for-anomaly-detector-in-real-time-intelligence/)
