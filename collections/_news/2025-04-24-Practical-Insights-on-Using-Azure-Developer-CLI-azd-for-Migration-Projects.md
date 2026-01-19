---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-in-a-real-life-scenario/
title: Practical Insights on Using Azure Developer CLI (azd) for Migration Projects
author: Frank Boucher
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2025-04-24 17:20:06 +00:00
tags:
- .NET
- Azd
- Azure Container Apps
- Azure Developer CLI
- Azure SDK
- Bicep
- IaC
section_names:
- azure
- coding
- devops
---
In this post, Frank Boucher details firsthand lessons from migrating AzUrlShortener to Azure using the Azure Developer CLI (azd). Gain insights into automating IaC with Bicep, managing deployment pipelines, and overcoming real-life migration challenges.<!--excerpt_end-->

# Azure Developer CLI (azd) in a Real-Life Scenario

**Author:** Frank Boucher

---

Migrating a tool to new infrastructure can be complex, often requiring custom shell scripts and manual infrastructure-as-code (IaC) authoring. In this post, Frank Boucher demonstrates how he leveraged the Azure Developer CLI (azd) to streamline the migration of the [AzUrlShortener](https://github.com/microsoft/AzUrlShortener) tool, highlighting practical tips and key lessons learned.

## Context

Previously, the AzUrlShortener project relied on ARM templates for its infrastructure. As the project evolved, updating to a modern IaC approach became necessary. This migration included transitioning resources such as an Azure Function, an API, a website, and a storage account.

## Generating Bicep Files with azd

One of the first tasks during the migration was converting ARM templates to Bicep. Notably, `azd` can generate Bicep files automatically, easing the transition to modern IaC. Using the `azd infra synth` command (currently in preview), Frank was able to generate infrastructure templates based on the solution’s .NET Aspire manifest.

The generated IaC files include:

```
infra/ # Infrastructure as Code (Bicep) files
├── main.bicep            # Main deployment module
└── resources.bicep       # Shared resources for the app's services
```

For each referenced project resource, azd creates a `containerApp.tmpl.yaml` in the AppHost's infra directory, for example:

```
Cloud5mins.ShortenerTools.AppHost/
└── infra/
    ├── admin.tmpl.yaml
    ├── api.tmpl.yaml
    └── azfunc-light.tmpl.yaml
```

> **Note:** `azd infra synth` is an alpha feature. Enable it with `azd config set alpha.infraSynth on`.

After generation, the Bicep files can be customized (e.g., to adjust scaling or CPU configurations), and running `azd up` applies any changes, deploying the solution to Azure.

## Revising IaC Files During Migration

Migrating in stages sometimes means integrating existing Azure resources. When adding an existing Azure Table Storage account, the deployment initially failed because azd did not manage the resource. Rather than manually updating Bicep (which is error-prone), Frank used `azd infra synth --force` to regenerate the infrastructure files based on the current state of Azure resources. This process is aided by version control (such as Git) to track and resolve any conflicts.

## Managing Custom Domains with azd

Adding a custom domain involved some additional configuration. After assigning a domain through the Azure Portal and retesting the solution, Frank discovered that rerunning `azd up` removed the domain assignment. This was resolved by enabling the following configuration, ensuring subsequent deployments preserved the custom domain:

```bash
azd config set alpha.aca.persistDomains on
```

To explore other preview settings, the command `azd config list-alpha` lists all available alpha features.

## Enabling CI/CD with azd

For continuous deployment, `azd pipeline config` simplifies automating deployments. This command generates a GitHub Action workflow file (also compatible with Azure Pipelines) and creates the necessary repository secrets, avoiding hardcoded sensitive information. The workflow efficiently manages deployments through established automation.

## Conclusion

Frank's experience demonstrates how the Azure Developer CLI (azd) makes migrations and deployments more manageable—automating file generation, integrating with CI/CD, and providing configuration options that simplify real-world complexities.

For additional guidance, explore the official [azd documentation](https://learn.microsoft.com/azure/developer/azure-developer-cli/) and the [AzUrlShortener](https://github.com/microsoft/AzUrlShortener) repository.

### Further Learning

To deepen your knowledge of .NET and Azure Container Apps, visit the [Get Started .NET on Azure Container Apps](https://aka.ms/aca-start) repository for step-by-step tutorials and videos.

---

**Related Series:**

- [Migrating AzUrlShortener from Azure Static WebApp (SWA) to Azure Container Apps](https://www.frankysnotes.com/2025/04/migrating-azurlshortener-from-azure.html)
- [Converting a Blazor Web Assembly (WASM) to FluentUI Blazor server](https://www.frankysnotes.com/2025/04/converting-blazor-wasm-to-fluentui.html)

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-in-a-real-life-scenario/)
