---
layout: "post"
title: "Implementing Row and Column Level Security for Spark in OneLake"
description: "This news post from the Microsoft Fabric Blog details the new public support for Row and Column Level Security (RLS and CLS) within OneLake, particularly focusing on Spark workloads. The article explains the technical approach used to overcome Spark's traditional lack of granular security and describes how the OneLake security framework delivers consistent and automatic data protection, enforcing policies across all data engines without compromising performance or requiring extra user effort. Details cover the security controls, job execution process separation, monitoring options, and requirements for schema-enabled lakehouses."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/how-spark-supports-onelake-security-with-row-and-column-level-security-policies/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-21 09:00:00 +00:00
permalink: "/news/2025-10-21-Implementing-Row-and-Column-Level-Security-for-Spark-in-OneLake.html"
categories: ["Azure", "ML", "Security"]
tags: ["Access Control", "Azure", "Big Data", "Column Level Security", "Data Governance", "Data Security", "Lakehouse", "Microsoft Azure", "Microsoft Fabric", "ML", "News", "OneLake", "Row Level Security", "Schema Enabled Lakehouse", "Secure Cluster", "Security", "Spark", "SQL", "Workspace Security"]
tags_normalized: ["access control", "azure", "big data", "column level security", "data governance", "data security", "lakehouse", "microsoft azure", "microsoft fabric", "ml", "news", "onelake", "row level security", "schema enabled lakehouse", "secure cluster", "security", "spark", "sql", "workspace security"]
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
