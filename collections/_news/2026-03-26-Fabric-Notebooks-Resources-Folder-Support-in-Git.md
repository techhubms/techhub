---
primary_section: ml
date: 2026-03-26 11:30:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-notebooks-resources-folder-support-in-git/
title: 'Fabric Notebooks: Resources Folder Support in Git'
tags:
- .gitignore
- CI/CD
- Configuration Files
- Data Engineering
- Deployment Pipelines
- DevOps
- Fabric Notebooks
- Git Exclusion Rules
- Git Integration
- Microsoft Fabric
- ML
- News
- Notebook Projects
- Python Modules
- Repository Sync
- Resources Folder
- Source Control
section_names:
- devops
- ml
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces an update to Fabric notebooks: the built-in Resources folder can now be committed to Git, making notebook projects easier to version, sync, and use in CI/CD workflows while controlling what gets tracked via exclusion rules and .gitignore.<!--excerpt_end-->

# Fabric Notebooks: Resources Folder Support in Git

> If you haven’t already, check out Arun Ulag’s blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at FabCon and SQLCon announcements across Fabric and Microsoft’s database offerings: https://aka.ms/FabCon-SQLCon-2026-news

Notebook projects are rarely just a single file. In real projects, notebooks often depend on supporting assets (for example reusable scripts, configuration files, and small data assets) that live alongside notebook code.

Previously, while these files could be stored in a notebook’s built-in **Resources** folder, they **weren’t included when committing notebooks to Git**. That gap broke end-to-end source control and made CI/CD workflows harder to adopt.

With this update, **Fabric notebooks now support committing the built-in Resources folder to Git**, enabling proper source control for notebook-based projects.

## What’s new

- The notebook’s built-in **Resources** folder can now be included when committing a notebook to Git.
- Supporting files (for example **Python modules**, **configuration files**, and **small data assets**) are:
  - versioned alongside the notebook
  - restored when syncing from a repository

## Controlling what gets committed

Some resource files aren’t appropriate for Git (large assets, temporary modules, generated outputs, test datasets). Fabric provides multiple ways to control what is committed:

- **Git exclusion rules**
  - Exclude files/folders based on criteria such as **file name**, **file type**, or **size**.
  - Helps keep large or transient artifacts out of the repository.
- **.gitignore support inside the Resources folder**
  - Use standard Git ignore patterns.
  - Useful for ignoring temporary files, test data, or generated content.

These controls are intended to keep repositories clean/lightweight without noticeable performance impact on commit or sync operations.

## Designed for safe adoption

- This feature is **opt-in** and **disabled by default** to avoid unexpected changes.
- Once enabled, users have fine-grained control over what gets committed (exclusion rules and .gitignore support).

## Getting started

1. Ensure your **workspace is connected to a Git repository**.
2. Open the notebook’s **Settings** page.
3. In the **Git settings** section, enable **resource support**.
4. After enabling, resources are automatically included whenever the notebook is committed.

## What’s coming next

Microsoft notes upcoming work including:

- Support for the **Environment Resources** folder
- Integration with **deployment pipelines**
- **Public APIs**

## Learn more

- Notebook source control and deployment: https://learn.microsoft.com/fabric/data-engineering/notebook-source-control-deployment


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebooks-resources-folder-support-in-git/)

