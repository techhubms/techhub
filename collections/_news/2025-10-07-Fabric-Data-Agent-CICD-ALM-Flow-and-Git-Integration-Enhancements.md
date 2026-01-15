---
layout: post
title: 'Fabric Data Agent: CI/CD, ALM Flow, and Git Integration Enhancements'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-data-agent-now-supports-ci-cd-alm-flow-and-git-integration/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-10-07 10:00:00 +00:00
permalink: /ml/news/Fabric-Data-Agent-CICD-ALM-Flow-and-Git-Integration-Enhancements
tags:
- ALM
- Application Lifecycle Management
- Azure
- CI/CD
- Collaboration
- Data Engineering
- Data Management
- Deployment Pipeline
- Development Workspace
- DevOps
- Fabric Data Agent
- Git Integration
- Microsoft Fabric
- ML
- News
- Production Workspace
- Source Control
- Version Control
section_names:
- azure
- devops
- ml
---
Microsoft Fabric Blog introduces major updates for Fabric data agents, with CI/CD, ALM flow, and Git integration. Authored by Microsoft Fabric Blog, the article explains how these features enhance development, collaboration, and data management across environments.<!--excerpt_end-->

# Fabric Data Agent: CI/CD, ALM Flow, and Git Integration Enhancements

Microsoft Fabric now brings CI/CD, application lifecycle management (ALM) flow, and Git integration capabilities to Fabric data agents, making it easier for teams to manage, track, and collaborate on data agent artifacts in a software engineering-friendly manner.

## Key Enhancements

- **CI/CD and ALM Flow**: Data agents support a variety of data sources, such as Lakehouse, Warehouse, Power BI Semantic Models, and KQL databases. Integrating CI/CD and ALM flows enables structured updates for schema selections, source configurations, queries, and custom instructions. This ensures changes are consistent, reviewable, and reversible.
- **Git Integration**: All data agent changes are now tracked via a Git repository. Every update, including modifications to queries and configurations, is recorded with full history. Standard Git workflows (commits, pull requests, rollbacks) are supported, providing reliable version control and collaboration.
- **Work Environment Separation**: Teams are encouraged to utilize three workspaces—development, test, and production. Each environment allows for progressive validation and deployment of data agent changes, reducing production risk.
- **Deployment Pipelines**: With Fabric deployment pipelines, updates move safely through workspaces. This supports reliable operations and helps teams adopt modern DevOps practices for data agent management.

## Benefits

- **Version-Controlled Artifact Management**: Git tracks all changes, facilitating audits and reversions.
- **Collaboration**: Multiple team members can work in parallel, validate updates, and safely merge changes.
- **Risk Reduction**: Staged environments (dev, test, prod) ensure issues are caught early.
- **Repeatable Workflows**: ALM and CI/CD bring predictability and scalability to data operations.

## Getting Started

To implement these enhancements, see the official guide:

- [Fabric data agent | Microsoft Learn](https://aka.ms/Fabric/data-agent-source-control)

Adopting these new workflows will help organizations modernize and standardize their data agent development process.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fabric-data-agent-now-supports-ci-cd-alm-flow-and-git-integration/)
