---
date: 2026-03-23 13:00:00 +00:00
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
title: 'Fabric CLI in Azure DevOps: automation without friction (Preview)'
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-in-azure-devops-automation-without-friction-preview/
tags:
- Artifact Deployment
- Automation
- Azure DevOps
- Azure DevOps Extension
- Azure DevOps Marketplace
- Bash
- Batch Scripting
- CI/CD
- Cross Platform Agents
- Deterministic Builds
- DevOps
- Environment Promotion
- Fab CLI
- Fabric CLI
- FabricCLI@1
- Microsoft Fabric
- ML
- News
- Pipeline Tasks
- PowerShell
- PowerShell Core
- Version Pinning
- Visual Studio Marketplace
- Workspace Management
- YAML Pipelines
section_names:
- devops
- ml
primary_section: ml
---

Microsoft Fabric Blog explains how the Fabric Azure DevOps extension provisions the Fabric CLI (fab) automatically inside pipeline runs, so teams can run Fabric automation tasks (deployments, workspace management, orchestration) without adding per-pipeline installation steps.<!--excerpt_end-->

# Fabric CLI in Azure DevOps: automation without friction (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog “[FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)” for a complete look at all of the FabCon and SQLCon announcements across both Fabric and Microsoft’s database offerings.*

Modern analytics platforms demand modern DevOps practices. As organizations standardize on Azure DevOps for CI/CD, they typically want tight integration between pipelines and the services running their data and analytics workloads. This announcement covers how **Microsoft Fabric** integrates into Azure DevOps through a marketplace extension that provides an automatically provisioned **Fabric CLI**.

## The Fabric CLI—built for CI/CD

The **Fabric CLI (`fab`)** enables teams to automate and manage Fabric environments using scriptable commands. When used via the **Fabric Azure DevOps (ADO) extension**, the CLI is **automatically provisioned** in the pipeline runtime environment.

With the Fabric ADO Extension and its built-in Fabric CLI task, pipelines can:

- Deploy and promote artifacts across environments
- Manage workspaces and items
- Trigger and orchestrate operations
- Integrate Fabric deployments into CI/CD flows

Automatic provisioning removes a common CI/CD friction point: manual tool installation and environment preparation.

## Zero setup, zero friction

Typically, using a CLI in CI/CD means adding explicit installation steps to every pipeline definition. With the Fabric CLI available automatically in Azure DevOps, the pipeline definition can be simpler.

Example task usage (as shown in the post):

```yaml
- task: FabricCLI@1
  inputs:
    scriptType: 'ps'
    scriptLocation: 'inlineScript'
    inlineScript: 'Fab ls'
    FabricCLIVersion: 'v1.2.0'
```

Benefits called out:

- No dependency scripting
- No agent-level configuration
- No external binary downloads
- No repetitive setup steps in every pipeline

## Flexible scripting: your language, your style

The Fabric CLI task supports multiple scripting languages, depending on your agent OS.

| Script Type | scriptType value | Agent OS |
| --- | --- | --- |
| PowerShell | `ps` or `powershell` | Windows |
| PowerShell Core | `pscore` | Windows, Linux, macOS |
| Batch | `batch` | Windows only |
| Shell (Bash) | `bash` or `sh` | Linux, macOS |

Cross-platform teams often use **PowerShell Core (`pscore`)** so the same YAML can run on both Windows and Linux agents without changes.

## Script location: inline or file path

The task supports two ways to provide scripts:

- `inlineScript` — write the script directly inside the YAML definition
- `scriptPath` — reference a `.ps1`, `.sh`, or `.bat` file checked into the repo

### Option A: inline script

Example inline script shown in the post:

```yaml
- task: FabricCLI@1
  displayName: 'Fabric CLI script with env vars'
  inputs:
    scriptType: 'bash'
    scriptLocation: 'inlineScript'
    inlineScript: |
      fab ls
      fab create ws3-test.Workspace -P capacityname=FabCapacity
      fab cd ws3-test.Workspace
      fab cd ..
      fab rm ws3-test.Workspace -f
    FabricCLIVersion: 'v1.2.0'
    FabricCLIEncryption: true
```

### Option B: script file path

Example script-path usage shown in the post:

```yaml
- task: FabricCLI@1
  displayName: 'Run Fabric deployment script'
  inputs:
    scriptType: 'pscore'
    scriptLocation: 'scriptPath'
    scriptPath: '$(Build.SourcesDirectory)/scripts/deploy-fabric.ps1'
    FabricCLIVersion: 'v1.2.0'
```

## Streamlined pipelines, improved upkeep

In enterprise environments managing dozens or hundreds of pipelines, automatic CLI installation reduces boilerplate and keeps YAML definitions focused on deployment intent rather than environment setup.

The post highlights improvements to:

- Pipeline clarity and readability
- Long-term maintainability
- Ease of troubleshooting and debugging
- Onboarding for new engineers

## Deterministic and consistent execution

A recurring CI/CD issue is **version drift**, where different agents/teams end up using different CLI versions.

By embedding the CLI within the Azure DevOps extension:

- Pipelines use a supported and validated CLI version
- Behavior is consistent across all environments
- Deployments become more deterministic
- Operational risk is reduced

## Get started

The **Fabric Azure DevOps Extension** is available on the **Azure DevOps Marketplace**. The flow described is:

1. Install the extension once
2. Add the `FabricCLI@1` task to your pipeline
3. Run Fabric commands without separate CLI installation steps

![Add CLI task to your pipeline](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/add-cli-task-to-your-pipeline.png)

Figure 1: Fabric ADO extension – CLI task.

Another example from the post:

```yaml
- task: FabricCLI@13
  inputs:
    scriptType: 'ps'
    scriptLocation: 'inlineScript'
    inlineScript: 'Fab ls'
    FabricCLIVersion: 'v1.2.0'
```

The post summarizes the goal as: no installation steps and a single task from pipeline trigger to executing Fabric commands.

## Share your feedback

Questions the post asks readers to share with the engineering team:

- What scenarios are you using the Fabric CLI task for?
- What additional extension tasks do you need support for?
- What limitations are most impactful for your workflow?


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/fabric-cli-in-azure-devops-automation-without-friction-preview/)

