---
layout: "post"
title: "Creating a Software Bill of Materials (SBOM) for an Open-Source NuGet Package"
description: "Andrew Lock explores multiple tools for generating Software Bill of Materials (SBOM) for .NET applications or NuGet packages. He examines GitHub's built-in SBOM export, Microsoft's sbom-tool, the anchore/sbom-action GitHub Action, and CycloneDX for .NET, providing practical guidance and comparison of their outputs."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/creating-a-software-bill-of-materials-sbom-for-an-open-source-nuget-package/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-03-25 09:00:00 +00:00
permalink: "/blogs/2025-03-25-Creating-a-Software-Bill-of-Materials-SBOM-for-an-Open-Source-NuGet-Package.html"
categories: ["Coding", "DevOps", "Security"]
tags: [".NET", "Application Security", "CI/CD", "Coding", "Compliance", "CycloneDX", "Dependency Management", "DevOps", "GitHub", "GitHub Actions", "NuGet", "Open Source", "Posts", "SBOM", "Sbom Tool", "Security", "Software Supply Chain", "SPDX", "Syft"]
tags_normalized: ["dotnet", "application security", "cislashcd", "coding", "compliance", "cyclonedx", "dependency management", "devops", "github", "github actions", "nuget", "open source", "posts", "sbom", "sbom tool", "security", "software supply chain", "spdx", "syft"]
---

In this comprehensive guide, Andrew Lock demonstrates how to generate SBOMs for .NET NuGet packages using tools like GitHub's SBOM export, Microsoft's sbom-tool, anchore/sbom-action, and CycloneDX, highlighting practical considerations for developers and operators.<!--excerpt_end-->

# Creating a Software Bill of Materials (SBOM) for an Open-Source NuGet Package

**Author: Andrew Lock**

---

A Software Bill of Materials (SBOM) provides a comprehensive inventory of all components, dependencies, and packages included in a software artifact, such as an application or NuGet package. With increasing attention on software supply chain security, generating and understanding SBOMs is becoming essential for both software creators and consumers. This guide walks through the necessity of SBOMs, discusses their formats, and demonstrates several tools to generate them for .NET projects.

## What is a Software Bill of Materials (SBOM)?

Analogous to a manufacturer's bill of materials, an SBOM details the raw materials and components that constitute a software product. It enumerates the various dependencies, allowing insight into security vulnerabilities, licensing compliance, and overall supply chain risk.

Two leading SBOM formats are:

- [SPDX](https://spdx.dev/) ([ISO/IEC 5692:2021](https://spdx.dev/use/specifications/))
- [CycloneDX](https://cyclonedx.org/) ([ECMA-424](https://ecma-international.org/publications-and-standards/standards/ecma-424/))

Both support JSON and are widely used.

## Generating SBOMs for .NET NuGet Packages: Tools and Methods

Several tools can create SBOMs for .NET applications or NuGet packages. Below are hands-on examples with the most prominent options.

### 1. GitHub's Built-in SBOM Export

GitHub repositories offer an **Export SBOM** feature under **Insights** > **Dependency Graph**:

![GitHub Export SBOM](<url-to-image>)

Clicking **Export SBOM** produces an SPDX JSON document that lists all detected dependencies, including packages from the main project and associated test projects. This approach is simple and direct but may include more dependencies than required depending on regulatory or operational needs.

Example excerpt from SPDX SBOM export:

```json
{
  "spdxVersion": "SPDX-2.3",
  "dataLicense": "CC0-1.0",
  ...
  "packages": [
    {"name": "Nuke.Common", "versionInfo": "8.1.0", ...},
    {"name": "StyleCop.Analyzers", "versionInfo": "1.2.0-beta.556", ...},
    {"name": "xunit", "versionInfo": "2.4.2", ...}
  ]
}
```

#### Observations

- SPDX includes extensive metadata per package.
- The export includes all dependency types (including test and support packages).
- Users should verify whether non-production dependencies are appropriate for their SBOM use cases.

### 2. Microsoft's Open Source `sbom-tool`

[Microsoft's sbom-tool](https://github.com/microsoft/sbom-tool) generates SPDX 2.2 SBOMs and integrates with build pipelines or runs locally as a .NET global tool.

#### Key Features

- Enterprise-ready, scalable SBOM generator
- Uses [Component Detection](https://github.com/microsoft/component-detection)
- Integrates with [ClearlyDefined API](https://github.com/clearlydefined/clearlydefined) for license info

#### Installation and Usage

```shell
# Install as a .NET global tool

 dotnet tool install --global Microsoft.Sbom.DotNetTool

# Run after building your packages

 sbom-tool generate \
   -b ./artifacts/packages \
   -bc ./src/ \
   -pn NetEscapades.AspNetCore.SecurityHeaders \
   -pv 1.0.0-preview.03 \
   -ps "Andrew Lock" \
   -pm
```

- `-b`: Path to built artifacts
- `-bc`: Path to associated source code
- `-pn`, `-pv`, `-ps`: Package name, version, supplier
- `-pm`: Parse package metadata for license info

The resulting SBOM (manifest.spdx.json) is stored in a subfolder of the build output path. The tool targets applications primarily, and some preference is given to SBOMs for entire build drops.

#### Observations

- Effective, especially for applications
- May require separate folders for each package
- Possible lack of clarity over inclusion of development dependencies

### 3. GitHub Action `anchore/sbom-action` (Syft)

[anchore/sbom-action](https://github.com/anchore/sbom-action) leverages the Syft SBOM tool within GitHub Actions.

Example GitHub Actions snippet:

```yaml
- uses: anchore/sbom-action@commitSHA # Use a pinned SHA for security
  with:
    path: ./artifacts/bin/NetEscapades.AspNetCore.SecurityHeaders
    output-file: ./artifacts/sboms/netescapades-aspnetcore-securityheaders.spdx.json
    upload-artifact: false
```

- Uses the deps.json file for dependency discovery
- Emphasizes reproducibility and supply chain risk mitigation (pin the commit hash)

#### Observations

- Extremely verbose SPDX output
- Simple to integrate in CI/CD pipelines
- Favors automated processes

### 4. CycloneDX Module for .NET

The [CycloneDX module for .NET](https://github.com/CycloneDX/cyclonedx-dotnet) offers CycloneDX format SBOMs. Available as a .NET global tool, it is straightforward for .NET projects.

#### Installation and Usage

```shell
# Install CycloneDX as a .NET tool

 dotnet tool install --global CycloneDX

# Generate SBOM

 dotnet-CycloneDX .\src\NetEscapades.AspNetCore.SecurityHeaders\NetEscapades.AspNetCore.SecurityHeaders.csproj \
   --json \
   --recursive \
   --set-name NetEscapades.AspNetCore.SecurityHeaders \
   --set-version 1.0.0-preview.03 \
   --base-intermediate-output-path .\artifacts\ \
   --output .\artifacts\sboms \
   --filename netescapades-aspnetcore-securityheaders.bom.json \
   --set-type library
```

- Outputs a readable, standards-compliant CycloneDX JSON document

#### Observations

- Output is concise and easier to read
- Machine-parsing is encouraged over manual inspection

## Summary and Recommendations

- **SBOMs** are essential for identifying software dependencies, licensing, and supply chain risks.
- Multiple tools support SBOM generation for .NET, each with different strengths:
  - **GitHub Export**: Fast, automatic, but broad in dependency scope
  - **Microsoft sbom-tool**: Flexible, enterprise focus, good .NET integration
  - **anchore/sbom-action**: Ideal for CI/CD, very verbose output
  - **CycloneDX for .NET**: Concise, modern format, .NET-friendly

When integrating SBOMs into development workflows, consider regulatory requirements, desired SBOM format, and the scope of dependencies to include.

---

_Andrew Lock focuses on open source .NET development and shares practical, hands-on guides and insights for developers._

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-a-software-bill-of-materials-sbom-for-an-open-source-nuget-package/)
