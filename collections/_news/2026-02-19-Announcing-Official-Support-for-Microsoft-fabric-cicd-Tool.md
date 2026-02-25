---
external_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-official-support-for-microsoft-fabric-cicd-tool/
title: Announcing Official Support for Microsoft fabric-cicd Tool
author: Microsoft Fabric Blog
primary_section: ml
feed_name: Microsoft Fabric Blog
date: 2026-02-19 12:00:00 +00:00
tags:
- Azure
- Build And Release Pipeline
- CI/CD
- Continuous Deployment
- Cross Workspace Deployment
- Data Platform
- Deployment Pipelines
- DevOps
- DevOps Automation
- Environment Configuration
- Fabric Cicd
- Git Integration
- Microsoft Fabric
- ML
- News
- Parameterization
- Python
- REST API
- .NET
section_names:
- azure
- dotnet
- devops
- ml
---
The Microsoft Fabric Blog introduces the officially supported fabric-cicd Python library, providing robust, automated CI/CD capabilities for Fabric workspaces. Learn from Microsoft's announcement how this tool simplifies cross-workspace deployments and modernizes DevOps for Fabric.<!--excerpt_end-->

# Announcing Official Support for Microsoft fabric-cicd Tool

The Microsoft Fabric team has announced that [fabric-cicd](https://microsoft.github.io/fabric-cicd/), the open-source Python deployment library for Microsoft Fabric, is now an officially supported, Microsoft-backed solution for CI/CD automation across Fabric workspaces.

## Background and Community Adoption

Over the past year, fabric-cicd has evolved through collaboration with Microsoft engineering, Customer Advisory Team (CAT), MVPs, enterprise customers, and the community. The tool has gained traction due to its ability to streamline enterprise-grade deployment pipelines, helping organizations manage and deploy content reliably in the Microsoft Fabric ecosystem.

With Microsoft's new commitment, fabric-cicd now benefits from official long-term support, a public roadmap, and deep integration with Fabric's features—such as Git Integration, REST APIs, and the CLI—as well as anticipated enhancements for future deployment capabilities.

## Key Objectives of fabric-cicd

Fabric-cicd addresses common challenges for teams managing Fabric deployments, with a focus on two main pain points:

- **Dependency Management**: Makes it easy to orchestrate deployments that involve dependencies between Fabric items, thus avoiding issues with order or binding.
- **Item Parameterization**: Unlocks parameterization across a wide range of Fabric item types, so that the same artifacts (e.g., datasets, reports) can move smoothly through Dev, Test, and Production environments with different configurations.

As a unified and automated deployment engine, fabric-cicd reduces friction in multi-environment workflows, making it a core component of the Fabric CI/CD toolchain. It provides an alternative to built-in pipelines for scenarios needing greater customization and control.

## Git-Based Deployment Using Build Environments

One recommended model is using a Git-based deployment flow:

- Deployments are triggered from the main branch of your Git repo.
- Build and Release pipelines per environment (Dev → Test → Prod) enable validations, approvals, and configuration.
- The pipeline invokes fabric-cicd to deploy item definitions into the correct workspace.
- Environment-specific parameters are applied during deployment, allowing for customization such as:
  - Data source connections
  - Item relationship/binding adjustments
  - Environment-specific variable injection
  - Configuration transformations

![Build & Release pipeline flow using fabric-cicd tool.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/02/build-and-release-pipeline-flow-using-fabric-cicd-to-1024x522.png)

*Figure 1 – Suggested build and release pipelines using fabric-cicd tool*

This approach gives DevOps teams the flexibility to manage complex deployments and automate the promotion of Fabric artifacts across environments, addressing advanced needs beyond what built-in pipelines or standard Git flows provide.

## Learn More

- Explore the [fabric-cicd documentation](https://microsoft.github.io/fabric-cicd)
- See [deployment examples](https://microsoft.github.io/fabric-cicd/0.1.34/example/deployment_variable/)
- Read the [announcement post](https://blog.fabric.microsoft.com/en-us/blog/announcing-official-support-for-microsoft-fabric-cicd-tool/)

Microsoft's official support marks a milestone for the Fabric DevOps ecosystem, enabling practitioners to build, test, and deploy Fabric solutions with greater reliability, repeatability, and control.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-official-support-for-microsoft-fabric-cicd-tool/)
