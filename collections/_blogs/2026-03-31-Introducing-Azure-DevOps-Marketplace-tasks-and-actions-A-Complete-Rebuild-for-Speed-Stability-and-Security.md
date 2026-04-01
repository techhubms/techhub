---
title: 'Introducing Azure DevOps Marketplace tasks and actions: A Complete Rebuild for Speed, Stability, and Security'
author: Jesse Houwing
section_names:
- ai
- devops
- github-copilot
- security
feed_name: Jesse Houwing's Blog
date: 2026-03-31 12:01:09 +00:00
external_url: https://jessehouwing.net/azure-devops-marketplace-tasks-and-actions/
primary_section: github-copilot
tags:
- AI
- Azure DevOps
- Azure DevOps Marketplace
- Azure Pipelines
- Blogs
- CI/CD
- Continuous Delivery
- Cross Platform Runners
- DevOps
- GitHub Actions
- GitHub Actions OIDC
- GitHub Copilot
- GitHub Sponsors
- Integration Testing
- OpenID Connect
- Personal Access Tokens
- PowerShell
- SBOM
- Security
- Service Connections
- Supply Chain Security
- Task Extensions
- Test Coverage
- TypeScript
- Unit Testing
- Visual Studio Marketplace
- VSIX
- Workload Identity Federation
- YAML Pipelines
---

Jesse Houwing explains why he rebuilt the Azure DevOps Marketplace publishing tasks from v5 to v6, focusing on faster builds, stronger testing, GitHub Actions support, and more secure authentication (OIDC/workload identity) while using GitHub Copilot’s Coding Agent to accelerate the rewrite.<!--excerpt_end-->

# Introducing Azure DevOps Marketplace tasks and actions (v6)

The Azure DevOps Extension Tasks started as a private PowerShell project and evolved into a widely used way to automate publishing Azure DevOps extensions via CI/CD. Jesse Houwing describes a full rebuild of the project (v6) to improve speed, stability, and security, and to support both Azure Pipelines and GitHub Actions.

## Background: From PowerShell scripts to a large TypeScript codebase

- Originally a **private PowerShell** project
- Later merged into broader efforts (ALM Rangers, Microsoft DevLabs) and became a common approach for **automating extension publishing**
- Over time, the codebase moved from PowerShell to **TypeScript modules**

The footprint grew dramatically:

- Original PowerShell implementation: **~700 KB**
- After migration to TypeScript: **~30 MB**
- After adding AzureRM support, Visual Studio Extension Publisher, and more tasks: **~300 MB**

## Growth without foundation: technical debt and compliance pressure

A major breaking point was the introduction of **signed pipeline requirements** for **SBOM (Software Bill of Materials)** and security compliance, which led to fragile workarounds:

- Hacks to manipulate manifests and VSIX files
- Brittle dependencies on tool versions and undocumented APIs
- Post-processing steps that broke frequently

A second major issue was **inadequate test coverage**, making refactors and dependency upgrades risky.

## The decision: rebuild from first principles

In late 2025, the author chose to rebuild rather than continue patching.

### Why the rebuild was feasible this time

The key accelerator was **GitHub Copilot’s Coding Agent**, which the author used to:

1. Extract a large test suite from v5 while preserving behavior
2. Build a new implementation that passes the tests
3. Consolidate and simplify the codebase
4. Act as a “rubber ducky” to challenge design choices and clarify requirements

### Core principles for v6

1. Clean architecture (separation of concerns, testable code)
2. Comprehensive test coverage
3. Reduced file size
4. Modern dependencies (security patches, best practices)
5. Cross-platform support (Azure Pipelines and GitHub Actions)
6. Enhanced security (reduce reliance on PATs)
7. Fix long-standing bugs

### Results (v5 → v6)

- **432 commits**
- **120 pull requests**
- **500+ tests**
- Bundle size: **20 MB** (down from 300 MB)
- CI coverage across **Windows, macOS, and Linux**

## What’s new in v6

### Full support for GitHub Actions

v6 ports the tasks from Azure Pipelines to **GitHub Actions**, with shared core logic.

Authentication options differ, and v6 introduces a security-first approach:

1. **GitHub Actions OIDC → Azure DevOps** (*new in v6*)
   - Exchange a GitHub OIDC token for Azure DevOps access
   - Configuration docs: https://github.com/jessehouwing/azdo-marketplace?ref=jessehouwing.net#github-actions-oidc
2. **Legacy support**
   - PAT and Basic Auth remain available via GitHub Actions Secrets

With OIDC, pipelines don’t store long-lived secrets; they use short-lived, federated tokens.

Related reading:

- https://jessehouwing.net/azure-devops-say-goodbye-to-personal-access-tokens-pats/
- https://jessehouwing.net/publish-azure-devops-extensions-using-workload-identity-oidc/

### Unified platform support (Azure Pipelines + GitHub Actions)

v6 aims for identical behavior across both platforms:

```yaml
# Azure Pipelines
- task: azdo-marketplace@6
  inputs:
    operation: publish
    publisherId: mycompany
    extensionId: my-extension

# GitHub Actions
- uses: jessehouwing/azdo-marketplace@v6
  with:
    operation: publish
    publisher-id: mycompany
    extension-id: my-extension
```

### New features

Requested features added in v6 include:

- Wait for Installation (pipeline gate until extension is installed)
- Unshare Extension
- Unpublish Extension
- Read/write VSIX metadata without unpacking the entire file
- Aligned inputs across tasks
- Unified task design: one task with an `operation` input, covering:
  - `package`
  - `publish`
  - `install`
  - `share`
  - `unshare`
  - `unpublish`
  - `show`
  - `query-version`
  - `wait-for-installation`
  - `wait-for-validation`

### Comprehensive testing and CI

The CI/CD workflow:

- Runs **500+ unit and integration tests**
- Tests on **Windows and Linux** runners
- Validates every authentication method
- Exercises all CLI code paths
- Auto-publishes the task for validation
- Tests both Azure Pipelines and GitHub Actions adapters
- Runs validation workflows on both platforms

## Getting started

### Minimal Azure Pipelines setup (v6)

High-level steps:

1. Create an Azure DevOps service connection
   - Prefer `AzureRM` for better security
   - Or use `Personal Access Token` for quick start, then migrate
2. Configure pipeline YAML

```yaml
trigger:
- main

stages:
- stage: Package
  jobs:
  - job: BuildAndPublish
    steps:
    - task: azdo-marketplace@6
      inputs:
        operation: package
        manifestFile: vss-extension.json
        outputPath: $(Build.ArtifactStagingDirectory)

    - task: azdo-marketplace@6
      inputs:
        operation: publish
        vsixFile: $(Build.ArtifactStagingDirectory)/*.vsix
        connectionType: azureRm
        connectionNameAzureRm: $(serviceConnection)
```

For GitHub Actions, the workflow structure is similar but uses action syntax.

Examples:

- https://github.com/jessehouwing/azdo-marketplace/tree/main/docs/examples?ref=jessehouwing.net

### Migrating from v5

v5 used multiple tasks (task-per-operation). v6 uses one task with `operation`.

v5:

```yaml
- task: PackageExtension@5
  inputs:
    manifestFile: vss-extension.json
```

v6:

```yaml
- task: azdo-marketplace@6
  inputs:
    operation: package
    manifestFile: vss-extension.json
```

Key migration points:

- Replace v5 tasks with `azdo-marketplace@6`
- Use `operation` to select the command
- Adjust inputs to match v6 schema
- Update auth to use `connectionType` and service connections

Migration guide:

- https://github.com/jessehouwing/azdo-marketplace/blob/main/docs/migrate-azure-pipelines-v5-to-v6.md?ref=jessehouwing.net

### Migrating to GitHub Actions

Outline:

1. Switch to GitHub Actions syntax
2. Configure OIDC federation (avoid managing tokens)
3. Adjust input names (kebab-case vs camelCase)
4. Update triggers (GitHub Actions triggers differ)

Migration guide:

- https://github.com/jessehouwing/azdo-marketplace/blob/main/docs/migrate-azure-pipelines-v6-to-github-actions.md?ref=jessehouwing.net

## Supply chain security: moving away from PATs

Why PATs are risky:

- Long-lived tokens stored in CI/CD systems
- Broad permissions if leaked
- Hard to rotate and audit
- No strong time/scope granularity

Workload identity federation benefits:

- Short-lived tokens (example given: 1 hour)
- Cryptographic proof of identity
- Enables RBAC-style control
- Better audit trail via identity provider
- Automatic rotation

### Deploying v6 safely

GitHub Actions → Azure DevOps:

- Create an Azure App Registration representing the workflow
- Configure OIDC federation for the GitHub org/repo
- Grant least-privilege permissions
- Runner exchanges its OIDC token for Azure DevOps access

Azure Pipelines → Azure DevOps (same org):

- Use Azure Managed Identity or a service principal
- Tasks fetch tokens via the configured service connection
- Avoid secrets in pipeline config

## Community-driven development and ways to contribute

The author credits community feedback and AI-assisted development for making the rebuild possible, and encourages:

1. Testing v6 on your extensions
2. Reporting issues: https://github.com/jessehouwing/azdo-marketplace/issues?ref=jessehouwing.net
3. Sharing success stories
4. Contributing: https://github.com/jessehouwing/azdo-marketplace/blob/main/docs/contributing.md?ref=jessehouwing.net

Support via GitHub Sponsors is mentioned:

- https://github.com/sponsors/jessehouwing?ref=jessehouwing.net

## Install and documentation

- Install v6 (Azure DevOps Marketplace): https://marketplace.visualstudio.com/items?itemName=jessehouwing.azdo-marketplace&ref=jessehouwing.net
- Full documentation: https://github.com/jessehouwing/azdo-marketplace?ref=jessehouwing.net
- Report issues / request features: https://github.com/jessehouwing/azdo-marketplace/issues?ref=jessehouwing.net


[Read the entire article](https://jessehouwing.net/azure-devops-marketplace-tasks-and-actions/)

