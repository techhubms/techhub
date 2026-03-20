---
section_names:
- ai
- devops
- github-copilot
- ml
- security
feed_name: Microsoft Fabric Blog
primary_section: github-copilot
date: 2026-03-19 14:30:00 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-extensibility-toolkit-generally-available/
tags:
- .ai Context
- Access Control (acl)
- AI
- CI/CD
- DevContainer
- DevGateway
- DevOps
- Extensibility
- Fabric Extensibility Toolkit
- Fabric Scheduler
- Fluent UI V9
- Frontend SDK
- GitHub Codespaces
- GitHub Copilot
- Microsoft Entra ID
- Microsoft Fabric
- ML
- News
- npm Package
- On Behalf Of (obo)
- OneLake
- React
- Remote Jobs
- Remote Lifecycle Notification API
- Security
- Sensitivity Labels
- Starter Kit
- Validator
- Workload Hub
- Workload Publishing
- Workspace Lineage
title: Microsoft Fabric Extensibility Toolkit is now Generally Available
---

Microsoft Fabric Blog announces general availability of the Microsoft Fabric Extensibility Toolkit, outlining what developers and partners get in the Starter Kit, how workload “items” integrate with Fabric/OneLake, and what’s coming next (CI/CD, lifecycle notifications, and remote jobs), including GitHub Copilot and Codespaces support.<!--excerpt_end-->

# Microsoft Fabric Extensibility Toolkit (Generally Available)

This article announces the **general availability (GA)** of the **Microsoft Fabric Extensibility Toolkit**, intended for partners and developers building **custom Fabric workloads** that show up as first-class items inside Fabric workspaces.

It also announces three additional capabilities entering **preview** (covered in a companion post):

- **CI/CD support**
- **Remote Lifecycle Notification API**
- **Fabric Scheduler (Remote Jobs)**

> Related conference context: Arun Ulag’s FabCon/SQLCon 2026 “hero blog” is referenced for a broader set of announcements.

## What’s new (GA capabilities)

Since the toolkit’s preview release (FabCon Vienna, Sep 2025) and publishing capabilities (Dec 2025), Microsoft describes continued stability improvements (bug fixes, edge cases, hardening). The result is a **production-ready** toolkit with the following capabilities.

### Toolkit capabilities

| Capability | Description |
| --- | --- |
| Starter Kit | A “Copilot-optimized” Hello World workload that runs in Fabric quickly (fastest path to a working item). |
| AI-enabled development | **GitHub Copilot integration** via structured `.ai/` context and repository instructions to help generate implementations. |
| Frontend SDK | `@ms-fabric/workload-client` npm package — **React + Fluent UI v9**, with the full Fabric host API surface. |
| Native item integration | Workload items appear like native Fabric items in workspaces, with automatic support for **CRUD**, **workspace ACLs**, **workspace search**, and **lineage**. |
| OneLake storage | Each item gets its own **OneLake folder** (Tables + Files) and customer data stays in the customer tenant. |
| Item definition persistence | Store item metadata/config in a **versioned, hidden OneLake folder** managed by Fabric. |
| Standard item creation | Fabric-controlled creation flow including **workspace selection**, **sensitivity labels**, and governed access. |
| Frontend Entra token | Acquire **On-Behalf-Of (OBO)** tokens in the frontend to call any **Entra-protected API** (Fabric, Azure, Office, or your own API). |
| iFrame relaxation | Request extended capabilities (file downloads, external API calls) with user consent via Fabric’s permission model. |
| Workload Hub publishing | Publish workloads to all Fabric users via the **Fabric Workload Hub** marketplace. |
| GitHub Codespaces / DevContainer | Starter Kit includes a configured **devcontainer** to run the workload in **GitHub Codespaces** with no local setup. |

## Pre-built visual components (Starter Kit)

The Starter Kit includes UI components meant to accelerate creating a Fabric item editor that matches Fabric design standards:

| Component | Description |
| --- | --- |
| ItemEditor | Base for item editors — view registration, ribbon, and lifecycle management. |
| ItemEditorDefaultView | Resizable two-panel layout (navigation + main content). |
| ItemEditorEmptyView | First-time experience with guided action tasks. |
| OneLakeView | Browse/select files and tables from OneLake. |
| WizardControl | Multi-step workflows with back/next navigation. |
| DialogControl | Dialogs styled consistently with native Fabric. |

## The ecosystem is growing

Microsoft highlights community momentum, including the **first Fabric Extensibility Community Contest** (Feb 2026) with multiple nominations and strong submissions.

## Getting started

### Fastest path (Starter Kit)

- Clone the Starter Kit: [aka.ms/fabric-extensibility-starter-kit](https://aka.ms/fabric-extensibility-starter-kit) (or use a dev space)
- Install dependencies:

```powershell
./scripts/Setup.ps1
```

- Start the DevGateway and frontend:

```powershell
./scripts/Run.ps1
```

- Open your **Fabric workspace** — the “Hello World” item should already be present.

### Using Copilot with the Starter Kit

The Starter Kit is described as **Copilot-compatible**: open it in VS Code and ask **GitHub Copilot** to implement a new feature to see how it can assist with building Fabric items.

### Updating an existing workload

If you already have a workload, update it to the latest Starter Kit version:

```powershell
./scripts/Setup.ps1 -Update
```

### Publishing to the Workload Hub

Use the **Validator** to run the same test suite Microsoft uses for certification before submitting your workload.

### Docs and repo

- Documentation: [https://aka.ms/fabric-extensibility-toolkit-docs](https://aka.ms/fabric-extensibility-toolkit-docs)
- Repository: https://github.com/microsoft/fabric-extensibility-toolkit

## What’s next (preview)

Three additional capabilities are entering preview:

- CI/CD support
- Remote Lifecycle Notification API
- Fabric Scheduler (Remote Jobs)

Details are in the companion post: [Fabric Extensibility Toolkit: CI/CD, Remote Lifecycle Notifications, and Fabric Scheduler (Preview)](https://aka.ms/extensibility-toolkit/CICD_Remote)

## Summary

- The **Fabric Extensibility Toolkit** is now **generally available** and production-ready.
- Core capabilities called out as stable: **OneLake storage**, item lifecycle, **Entra authentication**, and **Workload Hub publishing**.
- **Codespaces/DevContainer** support enables a “no local setup” path.
- Pre-built components aim to reduce time to build a quality workload.
- Preview capabilities coming next: **CI/CD**, lifecycle notifications, and **remote jobs** scheduling.


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-extensibility-toolkit-generally-available/)

