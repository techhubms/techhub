---
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-notebooks-support-lakehouse-auto-binding-in-git-preview/
author: Microsoft Fabric Blog
section_names:
- devops
- ml
feed_name: Microsoft Fabric Blog
tags:
- CI/CD
- Configuration Drift
- Data Engineering
- Dev Test Prod
- DevOps
- Fabric Notebooks
- Git Integration
- Git Sync
- Lakehouse
- Lakehouse Auto Binding
- Lifecycle Management
- Microsoft Fabric
- ML
- Multi Environment Workflows
- News
- Notebook Settings
- Notebook Settings.json
- Source Control
- Workspace Deployment
primary_section: ml
date: 2026-03-26 13:00:00 +00:00
title: Fabric notebooks support Lakehouse auto-binding in Git (Preview)
---

Microsoft Fabric Blog announces a preview feature for Fabric notebooks: Lakehouse auto-binding when using Git, aimed at simplifying CI/CD across dev, test, and production workspaces while keeping notebook logic portable and reducing environment-specific rebinding work.<!--excerpt_end-->

# Fabric notebooks support Lakehouse auto-binding in Git (Preview)

Fabric notebooks now support **lakehouse auto-binding** when used with **Git**. The goal is to make multi-environment workflows (development, test, production) easier by reducing the operational overhead of managing lakehouse references as notebooks move between workspaces.

The change is positioned as a way to enable cleaner Git-based CI/CD workflows: teams can focus on versioning notebook logic rather than repeatedly updating environment-specific lakehouse bindings.

## Simplifying notebook and Lakehouse management

In typical workflows, notebooks move across multiple workspaces via:

- Git sync
- Deployment pipelines

Previously, **lakehouse references** often needed careful handling during these transitions, which could lead to:

- Manual rebinding
- Environment-specific fixes

With **lakehouse auto-binding** enabled, Fabric can resolve the appropriate lakehouse as notebooks move across Git-connected workspaces. This is intended to improve notebook portability and align with Fabric lifecycle management, where:

- Metadata is tracked in Git
- Data is preserved

## How auto-binding works in Git workflows

To use lakehouse auto-binding:

1. Enable the setting from the notebook settings page.
   - It manages all lakehouses referenced by the notebook, including:
     - The default lakehouse
     - Any additional lakehouses
2. Once enabled, the configuration is captured as part of the Git workflow.

### notebook-settings.json

When you view the Git repository, Fabric generates a new file:

- `notebook-settings.json`

This file stores the notebook configuration used across CI/CD workflows. It is managed by Fabric to help keep behavior consistent as notebooks move across environments.

**Guidance:**

- The configuration is visible in the repo.
- It is **not recommended** to edit `notebook-settings.json` directly.
- To reduce configuration drift and keep workflows consistent, update auto-binding via the Fabric UI rather than modifying the file manually.

## Get started

If you use Git integration with Fabric notebooks, lakehouse auto-binding can simplify multi-environment development and deployment.

Learn more: Notebook source control and deployment

- https://learn.microsoft.com/fabric/data-engineering/notebook-source-control-deployment


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebooks-support-lakehouse-auto-binding-in-git-preview/)

