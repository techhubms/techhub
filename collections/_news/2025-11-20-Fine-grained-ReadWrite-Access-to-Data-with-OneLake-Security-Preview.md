---
layout: post
title: Fine-grained ReadWrite Access to Data with OneLake Security (Preview)
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/fine-grained-readwrite-access-to-lakehouse-data-with-onelake-security/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-11-20 08:30:00 +00:00
permalink: /ml/news/Fine-grained-ReadWrite-Access-to-Data-with-OneLake-Security-Preview
tags:
- Access Management
- Azure
- Data Access Control
- Data Architecture
- Data Collaboration
- Data Processing
- Fine Grained Access
- Lakehouse Security
- Least Privilege
- Microsoft Fabric
- ML
- News
- OneLake
- OneLakeFileExplorer
- ReadWrite Permissions
- RLS
- Role Based Security
- Security
- Spark Notebooks
- Structured Data
- Unstructured Data
section_names:
- azure
- ml
- security
---
Microsoft Fabric Blog details the new preview feature that brings fine-grained ReadWrite access to data in OneLake lakehouses. This lets data owners grant targeted write permissions, improving security and collaboration for technical teams.<!--excerpt_end-->

# Fine-grained ReadWrite Access to Data with OneLake Security (Preview)

## Introduction

Microsoft Fabric introduces ReadWrite access controls for lakehouse items in OneLake. Previously, users needed elevated workspace roles such as Admin or Member to perform write operations. With this new preview feature, data owners can assign precise write permissions so users can write to specific tables and folders in a lakehouse without managing broader workspace artifacts. This update is a significant move towards secure, collaborative workflows that follow least-privilege principles.

## Technical Details

- **Separation of Permissions**: Write access is now decoupled from overall workspace control, letting organizations configure more nuanced access patterns.
- **Supported Write Operations**:
  - Upload, delete, rename, and edit files
  - Operate via Spark notebooks, OneLakeFileExplorer, and OneLake APIs
  - CRUD operations on shortcuts for efficient data sharing
- **UX Limitations**: Write permissions for Lakehouse viewers via the graphical UX are not yet supported but are planned for future releases.

## Example Architecture: Secure Data Ingestion and Processing

A company stores structured and unstructured data in a central lakehouse. Users must upload loan applications for AI processing. Other team members use Spark notebooks to create new tables from the processed data. ReadWrite permissions let branch managers upload to designated folders without broader access. Role-based security ensures access only to relevant data. Row-level security (RLS) can further restrict visibility to specific branches.

**Scenario Steps:**

1. Admin creates a OneLake security role and selects the ReadWrite permission.
2. Scope is limited to a folder (e.g., 'Applications' for a branch).
3. Branch managers are added to the role, giving them upload capability for only that location.
4. Other lakehouse parts remain restricted.
5. RLS can be applied to tables for granular access.

## Benefits

- Enables secure data collaboration without over-provisioning access
- Supports technical workflows in Spark notebooks and automated pipelines
- Reduces risk in large teams by limiting write capability to targeted data locations
- Aligns with the principle of least privilege for data security

## Learn More

Further documentation is available at [OneLake security overview](https://aka.ms/OneLakeSecurityDocs).

---

*Author: Microsoft Fabric Blog*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fine-grained-readwrite-access-to-lakehouse-data-with-onelake-security/)
