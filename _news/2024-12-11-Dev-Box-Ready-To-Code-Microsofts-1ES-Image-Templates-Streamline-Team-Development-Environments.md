---
layout: "post"
title: "Dev Box Ready-To-Code: Microsoft’s 1ES Image Templates Streamline Team Development Environments"
description: "This article by Dmitry Goncharenko details how Microsoft's One Engineering System (1ES) team created ‘Ready-To-Code’ Dev Box image templates. It explores team customizations, template features, sample resources, and the integration with Azure DevOps, Bicep, and Azure Image Builder to reliably standardize development environments across teams."
author: "Dmitry Goncharenko"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/engineering-at-microsoft/dev-box-ready-to-code-dev-box-images-template/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/engineering-at-microsoft/feed/"
date: 2024-12-11 17:00:17 +00:00
permalink: "/2024-12-11-Dev-Box-Ready-To-Code-Microsofts-1ES-Image-Templates-Streamline-Team-Development-Environments.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["1ES", "Azure", "Azure Bicep", "Azure DevOps", "Azure Image Builder", "CI/CD", "Coding", "Dev Box", "DevCenter", "Developer Productivity", "DevOps", "Engineering@Microsoft", "Environment Automation", "Image Templates", "Microsoft Dev Box", "News", "One Engineering System", "Ready To Code", "Team Customizations", "Template Artifacts"]
tags_normalized: ["1es", "azure", "azure bicep", "azure devops", "azure image builder", "cislashcd", "coding", "dev box", "devcenter", "developer productivity", "devops", "engineeringatmicrosoft", "environment automation", "image templates", "microsoft dev box", "news", "one engineering system", "ready to code", "team customizations", "template artifacts"]
---

In this comprehensive post, Dmitry Goncharenko from Microsoft's 1ES team shares how Ready-To-Code Dev Box image templates are used to standardize and automate development environments, demonstrating practical benefits and sharing sample resources.<!--excerpt_end-->

# Dev Box Ready-To-Code: Microsoft’s 1ES Image Templates Streamline Team Development Environments

*By Dmitry Goncharenko*

At Microsoft Ignite, the company announced [Team customizations and imaging for Microsoft Dev Box](https://review.learn.microsoft.com/en-us/azure/dev-box/concept-what-are-team-customizations?branch=pr-en-us-289104), a feature designed to boost developer productivity and reduce the setup and maintenance time required for developer environments. The new team customization capabilities began as an internal solution built by Microsoft’s One Engineering System (1ES) team, and have helped drive adoption of Dev Box to over 35,000 developers across Microsoft.

The original 1ES [blog post](https://devblogs.microsoft.com/engineering-at-microsoft/dev-box-for-microsoft-engineers/) provided background on this approach and its early successes. Today, this article dives deeper into how 1ES built their “Ready-To-Code” environments and explains how recent Ignite announcements will bring these capabilities directly to Dev Box users.

## Addressing the Complexity of Team Environments

Large teams often face complex challenges due to massive repositories, proprietary or legacy tools, and lengthy builds. While many customization steps overlap between teams, maintaining and sharing a consistent set of customizations in a scalable way can be difficult. The 1ES solution targets these difficulties, providing a more streamlined, reusable, and scalable approach to setting up development environments.

The newest Team Customizations feature, evolving from 1ES’s internal solution, will eventually integrate the same capabilities into the Dev Box product for all users. The 1ES team is committed to migrating their own deployment and sharing guidance for external customers when the public release arrives.

## Technical Features of the 1ES Templates

The primary distinctions between the Ignite announcement and the current 1ES solution are:

- **Conditional Logic in Templates:** Allows highly complex but maintainable environment scripts, so teams can reuse a common core configuration while customizing for unique requirements.
- **Image Artifacts:** Example scripts and tasks that control how to install and configure tools and assets, derived from Microsoft’s CI/CD systems.

Team Customizations now support templates and inherit several benefits:

- **Enhanced Security:** Uses Azure Managed Identity for secure asset access, with all artifacts validated and sourced from trusted locations.
- **Improved Performance:** Images are equipped with [Dev Drive](https://devblogs.microsoft.com/engineering-at-microsoft/dev-drive-is-now-available/) and tuned Windows Defender settings for performance and security.
- **Consistency and Reliability:** Standardized templates help eliminate "works on my machine" discrepancies.
- **Flexibility:** Teams can specify which repositories to clone, build configurations, default tool installations, and additional customizations.
- **Ease of Maintenance:** Utilizes [Azure Bicep](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/overview) for authoring, mixing static declarations with logic, promoting code reuse via Bicep modules.
- **Simple Refreshes:** Integration with [Azure Pipelines](https://azure.microsoft.com/en-us/products/devops/pipelines) allows easier image management and troubleshooting.
- **Centralized Delivery:** Updates and improvements can be centrally managed and distributed across teams from a single source template.

## Testing, Delivery, and Release Management

Microsoft’s internal use required rigorous template testing and phased delivery:

- Pull requests result in test image builds covering most features.
- Each release builds a variety of images mimicking real-world definitions.
- Updates roll out in phases, starting with internal use (dogfooding) for quick feedback and issue mitigation.
- The [Bicep Module Registry](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/quickstart-private-module-registry?tabs=azure-cli) is used to share modules and manage release tags, facilitating both improvement delivery and targeted hotfixes.

## Community Sample: Ready-To-Code Environment Template

Making the 1ES approach public, Microsoft shared a [ready-to-use sample](https://github.com/Azure/azure-quickstart-templates/tree/master/quickstarts/microsoft.devcenter/devbox-ready-to-code-image) building Ready-To-Code environments for open-source repositories. Using [Azure Image Builder](https://azure.microsoft.com/en-us/products/image-builder), this sample is extensible and backed by many online resources.

### Key Sample Components

- **[README.md](https://github.com/Azure/azure-quickstart-templates/tree/master/quickstarts/microsoft.devcenter/devbox-ready-to-code-image#readme):** Step-by-step setup instructions.
- **[MSBuildSdks.bicep](https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.devcenter/devbox-ready-to-code-image/images/MSBuildSdks.bicep):** Example for a .NET repo image definition.
- **[eShop.bicep](https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.devcenter/devbox-ready-to-code-image/images/eShop.bicep):** Advanced .NET repo scenario.
- **[Axios.bicep](https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.devcenter/devbox-ready-to-code-image/images/axios.bicep):** Example for an NPM repo.
- **[devbox-image.bicep](https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.devcenter/devbox-ready-to-code-image/modules/devbox-image.bicep):** Central Bicep module.
- **[build_images.yml](https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.devcenter/devbox-ready-to-code-image/azuredevops/build_images.yml):** Azure DevOps pipeline script for building images.
- **[Artifacts tools](https://github.com/Azure/azure-quickstart-templates/tree/master/quickstarts/microsoft.devcenter/devbox-ready-to-code-image/tools/artifacts):** PowerShell scripts for image configuration tasks.

### Highlighted Features

- **Git Repositories:** Declarative repo cloning, package restoration, builds, shortcut creation; native support for MSBuild/dotnet projects.
- **Image Identity:** Built-in Azure Managed Identity for secure authentication with DevOps assets.
- **Dev Drive Support:** Easy configuration and management of Dev Drive for Dev Box images.
- **Default Base Image:** Visual Studio 2022, Microsoft 365 Apps, and standard developer tools preinstalled.
- **Tooling Defaults:** Preconfigures VSCode, Visual Studio with extensions, Sysinternals Suite, Git, Azure Artifacts Credential Provider, WinGet, and utilities.
- **Smart OS Defaults:** Optimized Windows settings for development, with Defender, long paths, and more.
- **Image Chaining & Compute Gallery Publishing:** Supports layered image builds for faster creation, and distribution to multiple compute galleries.
- **Build Environment Customization:** Controls on VM SKU/disk size for image builds.

### Sample Bicep Usage

A snippet from the [MSBuildSdks.bicep](https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.devcenter/devbox-ready-to-code-image/images/MSBuildSdks.bicep) definition shows the approach:

```bicep
module devBoxImage '../modules/devbox-image.bicep' = {
  name: 'MSBuildSdks-${uniqueString(deployment().name, resourceGroup().name)}'
  params: {
    imageName: imageName
    isBaseImage: false
    galleryName: galleryName
    repos: [
      {
        Url: 'https://github.com/microsoft/MSBuildSdks'
        Kind: 'MSBuild'
      }
    ]
    imageIdentity: imageIdentity
    builderIdentity: builderIdentity
    artifactsRepo: artifactsRepo
  }
}
```

## Benefits and Limitations

The 1ES template provides maximum flexibility and control, suitable for organizations with complex needs and Azure expertise—but it requires teams to manage their own pipelines and be comfortable with Azure resource management and Bicep.

The new Team Customization feature presented at Ignite aims to reduce the learning curve and will soon incorporate many of the advanced 1ES template capabilities directly into Dev Box. The 1ES team plans to continue collaboration with the Dev Box product group to make the transition smooth and preserve these benefits for all users.

---

For more detailed step-by-step guidance, tooling, and sample resources, explore the [ready-to-code sample repository](https://github.com/Azure/azure-quickstart-templates/tree/master/quickstarts/microsoft.devcenter/devbox-ready-to-code-image).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/engineering-at-microsoft/dev-box-ready-to-code-dev-box-images-template/)
