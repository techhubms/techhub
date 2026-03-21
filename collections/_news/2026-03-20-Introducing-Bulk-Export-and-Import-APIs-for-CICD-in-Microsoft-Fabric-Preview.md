---
primary_section: ml
author: Microsoft Fabric Blog
section_names:
- devops
- ml
feed_name: Microsoft Fabric Blog
title: Introducing Bulk Export and Import APIs for CI/CD in Microsoft Fabric (Preview)
date: 2026-03-20 16:30:00 +00:00
tags:
- Asynchronous Operations
- Automation
- Bulk Export API
- Bulk Import API
- CI/CD
- Definitions as Code
- Deployment Best Practices
- Dev Test Prod
- DevOps
- Disaster Recovery
- Environment Promotion
- Fabric REST API
- Git Integration
- IaC
- Item Definitions
- Managed Identity
- Microsoft Fabric
- ML
- News
- Pull Requests
- Release Pipelines
- Service Principal
- Version Control
external_url: https://blog.fabric.microsoft.com/en-US/blog/introducing-bulk-export-and-import-apis-for-ci-cd-in-microsoft-fabric-preview/
---

Microsoft Fabric Blog announces preview Bulk Export and Import Item Definition APIs that let teams treat Fabric items as code, store them in Git, validate via pull requests, and automate promotion across Dev/Test/Prod with repeatable CI/CD pipelines.<!--excerpt_end-->

# Introducing Bulk Export and Import APIs for CI/CD in Microsoft Fabric (Preview)

> If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings.

Enterprise DevOps teams typically want automation, repeatability, and full control over release pipelines. While Microsoft Fabric includes built-in Git integration and deployment experiences, some organizations need CI/CD capabilities that integrate directly into existing DevOps tooling.

To address this, Microsoft Fabric is introducing **Bulk Export and Import Item Definition APIs** (REST APIs) in **Preview**.

## CI/CD with Fabric item definitions as code

In Microsoft Fabric, items like **notebooks**, **pipelines**, **reports**, and **semantic models** are backed by a **structured item definition** that describes configuration and content.

The Bulk Export and Import APIs are intended to treat these item definitions as **source code**, enabling:

- Programmatic export from a workspace
- Storage and versioning in Git
- Validation through pull request workflows
- Automated promotion across environments using deployment pipelines

This supports a separation of concerns between:

- **Authoring**
- **Review**
- **Deployment**

## Git-based deployment using a build environment

A recommended model is to drive deployments from a **central Git repository**, promoting Fabric item definitions through a structured release flow.

A typical approach:

- Use a main branch aligned across environments (**Dev**, **Test**, **Prod**)
- Deploy each stage independently via dedicated build and release pipelines

Pipelines usually start by exporting item definitions from a development workspace using:

- **Fabric Git Integration**, or
- The **Bulk Export API**

Then, validate definitions in a build environment using:

- Automated checks
- Pull request reviews
- Policy enforcement

For deployment, invoke the **Bulk Import API** to promote approved definitions into the target workspace. The API supports:

- Creating new items
- Updating existing items in place

It also relies on Fabric’s built-in dependency handling to deploy items in the correct order, with the aim of making test/prod deployments consistent and repeatable without manual steps.

![Figure 1- Suggested build and release pipelines using bulk export/import API](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/figure-1-suggested-build-and-release-pipelines-us.png)

*Figure 1: Suggested build and release pipelines using bulk export/import API*

## Built for scale and enterprise automation

The Bulk Export and Import APIs are positioned as optimized for:

- Batch operations across large workspaces
- Long-running, asynchronous execution
- Authentication using **service principals** and **managed identities**
- Repeatable, auditable deployments across environments

The post also calls out scenarios such as:

- Enterprise CI/CD pipelines
- Disaster recovery workflows
- Large-scale environment promotion

## Get started

- The APIs are part of the Microsoft Fabric REST API surface in **Preview**.
- Documentation: the detailed Fabric API guide: https://aka.ms/AAzye0t
- Walkthrough: complete CI/CD tutorial using the Bulk Export and Import APIs: https://aka.ms/AAzye19

The post invites feedback in comments as the platform evolves.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/introducing-bulk-export-and-import-apis-for-ci-cd-in-microsoft-fabric-preview/)

