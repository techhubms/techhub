---
external_url: https://blog.fabric.microsoft.com/en-US/blog/how-spark-supports-onelake-security-with-row-and-column-level-security-policies/
title: Implementing Row and Column Level Security for Spark in OneLake
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-21 09:00:00 +00:00
tags:
- Access Control
- Big Data
- Column Level Security
- Data Governance
- Data Security
- Lakehouse
- Microsoft Azure
- Microsoft Fabric
- OneLake
- Row Level Security
- Schema Enabled Lakehouse
- Secure Cluster
- Spark
- SQL
- Workspace Security
section_names:
- azure
- ml
- security
---
Microsoft Fabric Blog's team introduces new Row and Column Level Security for Spark in OneLake, explaining technical implementation and how these policies protect data access. Authored by the Microsoft Fabric Blog.<!--excerpt_end-->

# Implementing Row and Column Level Security for Spark in OneLake

**Author:** Microsoft Fabric Blog

## Overview

OneLake recently launched public support for Row and Column Level Security (RLS and CLS), introducing a universal data security framework that applies consistently across all data engines—including Spark—no matter how data is accessed.

## Spark Security Challenge

Historically, Spark allowed broad, unrestricted access to datasets, lacking built-in granular security features. This presented challenges in enforcing fine-grained access control critical for enterprise data governance.

## Microsoft’s Solution

To address Spark's limitations, Microsoft Fabric’s engineering team has:

- Developed a customized Spark integration
- Separated user code execution from secure data preparation
- Ensured all jobs reading protected tables do so in isolated, secure environments

### Key Security Features

- **Two-environment job execution:**
  - One for user code
  - Another for secure data access applying RLS and CLS policies
- **Automatic and transparent process:** Runs for every query, requiring no manual intervention
- **Dynamic resource scaling:** Secure environments scale with demand, staying ready for five minutes after activity to reduce latency
- **Monitoring:** Secure jobs can be tracked in the workspace, annotated with the ‘SparkSecurityControl’ prefix

## Universal Security Enforcement

OneLake strictly prohibits unauthorized access, including:

- Blocking direct file-level access to protected tables
- Requiring access via Spark SQL namespace references (e.g., `lakehouse.schema.table`)

## Schema-enabled Lakehouse Requirements

- Spark supports both schema-enabled and non-schema lakehouses with security policies
- Users must set a schema-enabled lakehouse as their default in order to use secure clusters, even if querying non-schema lakehouses

## Getting Started

The preview for OneLake security is now available for all users. You can:

- Explore the feature in your own Microsoft Fabric workspace
- Consult the updated documentation
- [Sign up for a free Microsoft Fabric trial](https://app.fabric.microsoft.com/) to experience OneLake security in action

## Visual Resources

(Images referenced in the original post display the architecture and workspace monitoring interface.)

---

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/how-spark-supports-onelake-security-with-row-and-column-level-security-policies/)
