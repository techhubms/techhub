---
layout: "post"
title: "Add Sidecars to Azure App Service for Linux—via GitHub Actions or Azure Pipelines"
description: "This post introduces ready-made templates for deploying helper sidecar containers alongside your main application in Azure App Service for Linux. It explains how to use GitHub Actions or Azure Pipelines for CI/CD and provides concrete guidance on attaching telemetry agents, APIs, caches, and more within a unified lifecycle. Templates enable both code-based and containerized app workflows."
author: "TulikaC"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/add-sidecars-to-azure-app-service-for-linux-via-github-actions/ba-p/4465419"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-30 04:08:13 +00:00
permalink: "/community/2025-10-30-Add-Sidecars-to-Azure-App-Service-for-Linuxvia-GitHub-Actions-or-Azure-Pipelines.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET", "App Service Runtime", "Azure", "Azure App Service", "Azure Pipelines", "CI/CD", "Coding", "Community", "DevOps", "GitHub Actions", "Java", "Linux Containers", "Monitoring Agents", "Node.js", "OIDC Authentication", "PHP", "Python", "Sidecar Containers", "Sitecontainers", "Telemetry", "VSTS", "YAML Templates"]
tags_normalized: ["dotnet", "app service runtime", "azure", "azure app service", "azure pipelines", "cislashcd", "coding", "community", "devops", "github actions", "java", "linux containers", "monitoring agents", "nodedotjs", "oidc authentication", "php", "python", "sidecar containers", "sitecontainers", "telemetry", "vsts", "yaml templates"]
---

TulikaC explains how to use GitHub Actions and Azure Pipelines templates to deploy helper sidecars with Azure App Service for Linux, making CI/CD setup seamless for various runtime and container scenarios.<!--excerpt_end-->

# Add Sidecars to Azure App Service for Linux—via GitHub Actions or Azure Pipelines

Sidecars on [Azure App Service](https://learn.microsoft.com/en-us/azure/app-service/overview-sidecar) let you run additional containers—such as telemetry, monitoring agents, lightweight APIs, caches, and AI helpers—right next to your primary app. These sidecars share the same lifecycle and local networking, all without changes to your core app code.

## CI/CD Templates Available

We've published plug-and-play templates for:

- ### GitHub Actions
  [Sample workflows](https://azure.github.io/AppService/2025/09/08/GHA-templates-sidecars.html) for both built-in runtimes (Python, Node.js, .NET, Java, PHP) and custom containers. Attach sidecars using the sitecontainers config.

- ### Azure Pipelines (VSTS)
  [YAML templates](https://azure.github.io/AppService/2025/10/29/VSTS-tasks-for-sidecars.html) enable builds/deploys for code-based apps (AzureWebApp@1) and containerized apps (AzureWebAppContainer@1). Sidecars are declared in the same run—mark one container as isMain: true and others as sidecars.

## When to Use Which Template

- **Code-based Apps (Built-in Linux runtime):** Use "blessed/built-in runtime" template—your app runs on App Service runtime, sidecars run beside it.
- **Custom Containers (Web App for Containers):** Use the containers template to build/push multiple images and deploy together.

## Quick Start Steps

1. **Add the template** to your repository (select workflow for GitHub or YAML for VSTS).
2. **Configure authentication** via OIDC or service connection; specify app name, resource group, registry, and details for sidecar images/ports.
3. **Run the pipeline;** view both main app and sidecars in the Azure portal.

## Reference Links

- [GitHub Actions samples](https://azure.github.io/AppService/2025/09/08/GHA-templates-sidecars.html): Adding sidecars to Azure App Service for Linux (code-based & containerized).
- [Azure Pipelines samples](https://azure.github.io/AppService/2025/10/29/VSTS-tasks-for-sidecars.html): Adding sidecars to Azure App Service for Linux (code-based & containerized).

## Author & Additional Details

- **Author:** TulikaC ([profile](https://techcommunity.microsoft.com/t5/s/gxcuf89792/m_assets/avatars/default/avatar-1.svg?image-dimensions=50x50))
- **Published:** October 30, 2025
- **Version:** 1.0
- **Blog:** [Apps on Azure Blog](https://techcommunity.microsoft.com/category/azure/blog/appsonazureblog) (Follow for more updates)

---

By dropping in a pre-made template and declaring your sidecars, you can extend your App Service environment with new capabilities on a shared lifecycle—no need to refactor your main application.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/add-sidecars-to-azure-app-service-for-linux-via-github-actions/ba-p/4465419)
