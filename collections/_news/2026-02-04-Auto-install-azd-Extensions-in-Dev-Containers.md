---
external_url: https://devblogs.microsoft.com/azure-sdk/azd-devcontainer-extensions/
title: Auto-install `azd` Extensions in Dev Containers
author: PuiChee (PC) Chan
primary_section: dotnet
feed_name: Microsoft Azure SDK Blog
date: 2026-02-04 19:08:28 +00:00
tags:
- Automation
- Azd
- Azure
- Azure Developer CLI
- Azure SDK
- CLI Tools
- Container Development
- Devcontainer.json
- Devcontainers
- Development Environments
- DevOps
- Extensions
- News
- Onboarding
- VS Code
- .NET
section_names:
- azure
- dotnet
- devops
---
PuiChee (PC) Chan presents a quick guide to the new Azure Developer CLI feature that lets you auto-install `azd` extensions in dev containers, improving developer onboarding and setup.<!--excerpt_end-->

# Auto-install `azd` Extensions in Dev Containers

**Author:** PuiChee (PC) Chan  

## What’s New?

The Azure Developer CLI (`azd`) now supports automatic installation of extensions when using dev containers. By specifying a comma-separated list of `azd` extensions in your dev container configuration, these extensions are installed automatically during the container build process.

**Example `devcontainer.json`:**

```json
{
  "features": {
    "ghcr.io/azure/azure-dev/azd:latest": {
      "extensions": "azd-ext-example,azd-ext-another"
    }
  }
}
```

## Why It Matters

Dev containers provide ready-to-code environments quickly, but setting up required `azd` extensions used to require manual steps after starting the container. This could slow down onboarding and create friction for individual developers or teams switching between projects. With this new auto-install feature, your necessary extensions are already in place every time the dev container starts.

## How to Use It

1. Open your `.devcontainer/devcontainer.json` file.
2. Add or update the `azd` feature with the `extensions` option. For example:

```json
{
  "features": {
    "ghcr.io/azure/azure-dev/azd:latest": {
      "extensions": "your-extension-name"
    }
  }
}
```

1. Rebuild your dev container. The extensions will be installed during the build.

## Get Started

- Update your dev container configuration as shown above and rebuild.
- For more details on creating dev containers, read the [VS Code dev container docs](https://code.visualstudio.com/docs/devcontainers/create-dev-container).

## Feedback

Questions or suggestions? File an issue or start a discussion on [GitHub](https://github.com/Azure/azure-dev). You can also [sign up for user research](https://aka.ms/azd-user-research-signup) to help shape `azd`'s future.

## Reference

This feature was introduced in [PR #6460](https://github.com/Azure/azure-dev/pull/6460) on Azure Dev.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azd-devcontainer-extensions/)
