---
layout: post
title: 'Modernizing .NET Applications with GitHub Copilot Agent Mode: A Step-by-Step Guide'
author: Mika Dumont
canonical_url: https://devblogs.microsoft.com/dotnet/modernizing-dotnet-with-github-copilot-agent-mode/
viewing_mode: external
feed_name: Microsoft .NET Blog
feed_url: https://devblogs.microsoft.com/dotnet/feed/
date: 2025-11-18 18:05:00 +00:00
permalink: /github-copilot/news/Modernizing-NET-Applications-with-GitHub-Copilot-Agent-Mode-A-Step-by-Step-Guide
tags:
- .NET
- App Modernization
- Authentication
- Automated Testing
- Azure Migration
- CI/CD
- Cloud
- Cloud Readiness
- Code Assessment
- Dependency Management
- Framework Upgrade
- Git Integration
- Identity Management
- Monitoring
- Security Scanning
- VS
section_names:
- ai
- azure
- coding
- devops
- github-copilot
- security
---
Mika Dumont presents a practical guide for developers to modernize legacy .NET applications using GitHub Copilot agent mode in Visual Studio. The article covers automated upgrades, cloud migration to Azure, and integration of security and DevOps practices.<!--excerpt_end-->

# Modernizing .NET Applications with GitHub Copilot Agent Mode: A Step-by-Step Guide

Upgrading outdated .NET apps is often complex, involving dependency issues and manual fixes. This guide, authored by Mika Dumont, shows how Visual Studio 2026 and GitHub Copilot app modernization streamline the process for developers.

## Why Modernize?

Older frameworks can limit performance, introduce security risks, and impede cloud migration. Upgrading enables modern APIs and features such as:

- Automated scaling
- Secure identity management

## Prerequisites

To begin, ensure:

- Visual Studio 2026 or Visual Studio 2022 v17.14.17+
- GitHub Copilot license (Pro, Pro+, Business, or Enterprise)
- .NET desktop workload enabled with Copilot app modernization

## Steps for Upgrading .NET Applications

### 1. Open Your Project

Launch Visual Studio and select your .NET project.

### 2. Start an Agent Session

- Right-click in Solution Explorer and choose **Modernize**, or
- Use Copilot Chat: `@modernize [your request]`

### 3. Select Upgrade Goals

Choose between upgrading to a newer .NET version, migrating to Azure, or exploring other modernization options.

### 4. Assessment and Planning

Copilot:

- Assesses code and dependencies
- Gathers your goals (security, performance, cloud readiness)
- Generates a markdown upgrade plan for transparency
- Lets you edit the plan before approval

### 5. Automated Changes and Error Resolution

Approved plans trigger Copilot to:

- Upgrade files and adjust imports
- Fix build issues via a real-time fix-and-test loop
- Track progress in a dedicated document
- Commit each step to Git for rollback support
- Pause and request input if needed

### 6. Reviewing Results

Receive a summary with Git commit hashes and recommended next steps (e.g., updating CI/CD pipeline or integration testing).

## Making Your App Cloud-Ready

### 1. Run Cloud Readiness Assessment

Select **Migrate to Azure** in Copilot's UI. Copilot scans for:

- Framework compatibility
- Authentication or identity gaps for cloud environments
- Dependency vulnerabilities
- Provides a detailed assessment report and recommended critical actions

### 2. Migration Planning and Execution

Copilot:

- Highlights dependency updates for Azure compatibility
- Lists required cloud service configuration changes
- Includes compliance and security enhancements

Approved migration plans automate configuration and code updates, adding necessary Azure SDKs and authentication adapters.

### 3. Validation and Security

After migration:

- Runs automated CVE scans on dependencies
- Proposes safe version replacements
- Ensures testing and build integrity

### 4. Deploy to Azure

Copilot automates:

- Resource provisioning (no scripts required)
- Monitoring/logging setup for observability
- Secure identity configuration

## Summary

GitHub Copilot agent mode greatly simplifies upgrading .NET applications and migrating them to Azure. Automated planning, dependency management, error resolution, CI/CD integration, and enhanced security procedures help developers modernize and deploy workloads efficiently.

For more info and demos, [read the original guide](https://devblogs.microsoft.com/dotnet/modernizing-dotnet-with-github-copilot-agent-mode/) or [try Copilot app modernization](https://aka.ms/ghcp-appmod/dotNET).

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/modernizing-dotnet-with-github-copilot-agent-mode/)
