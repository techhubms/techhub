---
external_url: https://devblogs.microsoft.com/powershell/announcing-dsc-v3/
title: Announcing Microsoft Desired State Configuration v3.0.0
author: Jason Helmick
feed_name: Microsoft PowerShell Blog
date: 2025-03-12 17:58:25 +00:00
tags:
- Automation
- Configuration Management
- Cross Platform
- Desired State Configuration
- DSC
- IaC
- JSON
- PowerShell
- PowerShell DSC
- PSDSC
- Resource Authoring
- System Orchestration
- YAML
section_names:
- azure
- coding
- devops
primary_section: coding
---
In this detailed announcement, Jason Helmick introduces Microsoft Desired State Configuration v3.0.0, highlighting its cross-platform capabilities, integration improvements, and significant differences from classic PowerShell DSC.<!--excerpt_end-->

# Announcing Microsoft Desired State Configuration v3.0.0

*By Jason Helmick*

*This is the first post in a multi-part series about the new release of DSC.*

## Overview

Microsoft announces the General Availability of Desired State Configuration (DSC) version 3.0.0—a significant advancement in cloud-native configuration management for cross-platform environments. DSC is a declarative configuration and orchestration platform that provides a standardized way to expose settings for applications and services, allowing you to describe what the desired state should be, not how to achieve it. This design helps separate the intent of configurations from their operational procedures, making management more efficient and consistent.

## Benefits of DSC v3.0.0

- **Declarative and Idempotent:** DSC configuration documents use declarative JSON or YAML, describing system state clearly. DSC ensures the system matches this state repeatedly, avoiding unnecessary changes.
- **Flexible Resource Authoring:** DSC resources can be written in any language, not only PowerShell, broadening extensibility.
- **Cross-Platform:** Runs on Linux, macOS, and Windows without additional dependencies.
- **Easy Integration:** DSC is designed for straightforward integration with existing tools, leveraging schematized JSON output, and allows input through stdin, supporting DevOps and automation workflows. Output is structured for easy parsing and validation.
- **Backwards Compatibility:** Supports all existing PowerShell 7 and Windows PowerShell DSC resources.

With DSC, you can:

- Create configuration files for controlled, repeatable infrastructure management
- Author resources in any language to manage various systems and applications
- Invoke DSC resources directly for targeted actions
- Define standardized, discoverable settings for services and applications

## Key Differences from PowerShell DSC (PSDSC)

- No longer includes or supports the Local Configuration Manager (LCM)
- No PowerShell dependency—manage resources in bash, python, C#, Go, etc., without PowerShell installed
- Operates as a command-line tool, not as a service
- Moves away from Managed Object Format (MOF) configuration files; uses JSON and YAML instead
- Outputs are strongly structured JSON, easing tooling integration and reporting
- Supports expression functions in configuration documents for dynamic values
- Allows runtime parameter input via JSON or parameter files
- Maintains backwards compatibility with classic PowerShell DSC resources

## New Features in DSC v3.0.0

### Groups

- Supports a resource kind for grouping resources, influencing processing order via `dependsOn`.
- Allows resource and configuration authors to define and use group resources.
- [Example: group configuration](https://github.com/PowerShell/DSC/blob/main/dsc/examples/groups.dsc.yaml)

### Assertions

- Introduces the `Microsoft.Dsc/Assertion` group resource for validating environment prerequisites.
- [Example: assertion resource](https://github.com/PowerShell/DSC/blob/main/dsc/examples/assertion.dsc.yaml)

### Importers

- New resource kind enables importing configurations from external sources, enabling reuse and composition.
- Built-in `Microsoft.Dsc/Include` resource supports configuration modularization.
- [Example: include resource](https://github.com/PowerShell/DSC/blob/main/dsc/examples/include.dsc.yaml)

### Exporting

- New export operation for resources: `dsc resource export` and `dsc config export` commands retrieve current resource instances and build new configuration documents.
- [Learn more on dsc resource export](https://learn.microsoft.com/en-us/powershell/dsc/reference/cli/resource/export?view=dsc-3.0&preserveView=true)

### Configuration Functions

- Configuration documents support functions for manipulating resource processing and referencing outputs between resources.
- [Example: reference function](https://github.com/PowerShell/DSC/blob/main/dsc/examples/reference.dsc.yaml)

## Support Lifecycle

- Adopts semantic versioning.
- Stable releases receive patches for three months after the next Stable release.
- Always keep to the latest patch release for ongoing support.

## Next Steps

- This post is the beginning of a series. “DSC” refers to v3.0.0; “PSDSC” covers PSDSC v1.1 and v2.
- See the next post: [Get Started with Desired State Configuration v3.0.0 (DSC)](https://devblogs.microsoft.com/powershell/get-started-with-dsc-v3/)

## Call to Action

- Learn more about DSC v3.0.0 in its [official documentation](https://learn.microsoft.com/powershell/dsc/overview?view=dsc-3.0&preserveView=true)
- Provide feedback or report issues at the [DSC GitHub repository](https://github.com/PowerShell/DSC)

---

*Jason Helmick, Sr. Product Manager, PowerShell*

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/announcing-dsc-v3/)
