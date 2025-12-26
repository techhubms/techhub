---
layout: "post"
title: "Creating SBOM Attestations for NuGet Packages Using GitHub Actions"
description: "Andrew Lock explains how to generate attestations for Software Bill of Materials (SBOM) documents in GitHub Actions workflows. The post provides practical guidance, configuration examples, and discusses current limitations of attestation verification in the NuGet ecosystem."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/creating-sbom-attestations-in-github-actions/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-04-01 09:00:00 +00:00
permalink: "/posts/2025-04-01-Creating-SBOM-Attestations-for-NuGet-Packages-Using-GitHub-Actions.html"
categories: ["DevOps", "Security", "Coding"]
tags: [".NET", "Attestations", "Coding", "CycloneDX", "DevOps", "GitHub", "GitHub Actions", "NuGet", "Posts", "Provenance", "SBOM", "Security", "Sigstore", "SPDX", "Supply Chain Security", "YAML Workflows"]
tags_normalized: ["dotnet", "attestations", "coding", "cyclonedx", "devops", "github", "github actions", "nuget", "posts", "provenance", "sbom", "security", "sigstore", "spdx", "supply chain security", "yaml workflows"]
---

In this blog post, Andrew Lock demonstrates how to create SBOM attestations for your .NET applications or NuGet packages using GitHub Actions, enhancing supply chain security.<!--excerpt_end-->

# Creating SBOM Attestations for NuGet Packages Using GitHub Actions

*By Andrew Lock*

## Introduction

In this post, Andrew Lock provides a detailed guide on creating attestations for Software Bill of Materials (SBOM) documents for .NET applications and NuGet packages. The goal is to enhance confidence and transparency in the software supply chain by leveraging GitHub Actions for automated provenance and attestation generation.

## Supply Chain Security and Attestations

Supply chain security involves ensuring the integrity and provenance of software artifacts. Andrew discusses the role of provenance attestations, which serve as standardized documents that describe how, where, and by whom an artifact was built ([SLSA specification](https://slsa.dev/spec/v1.0/provenance)).

While providing an attestation doesn't guarantee downstream security, it facilitates the verification process for package consumers, forming a key building block in supply chain security.

### What is an SBOM?

A Software Bill of Materials (SBOM) enumerates the packages and dependencies used to create a software artifact. SBOMs enable:

- Visibility into included components
- Identification of known vulnerabilities
- Awareness of compliance/licensing issues
- Insight into supply chain risks

## Combining Provenance and SBOM Attestations

Attestations can apply both to the artifact and to its associated SBOM, allowing consumers to verify that the SBOM is authentic and untampered, and was generated as claimed during a specific build run.

## Generating SBOM Attestations in GitHub Actions

Andrew demonstrates how to use the [`actions/attest-sbom`](https://github.com/actions/attest-sbom) GitHub Action to generate signed attestations for SBOMs. This action supports both SPDX and CycloneDX JSON formats. These formats are supported by multiple open-source tools and are standardized ([SPDX: ISO/IEC 5692:2021](https://spdx.dev/use/specifications/), [CycloneDX: ECMA-424](https://ecma-international.org/publications-and-standards/standards/ecma-424/)).

## Example Workflow: Building a .NET Project with SBOM Attestation

Below is a sample workflow that:

- Builds a .NET NuGet package
- Creates a CycloneDX SBOM
- Generates an SBOM attestation

```yaml
name: BuildAndPack

on:
  push:
    branches: ["main" ]
    tags: ['*']
  pull_request:
    branches: ['*']

jobs:
  build-and-test:
    # Add necessary permissions for attestation
    permissions:
      id-token: write
      attestations: write
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4.2.2
    - uses: actions/setup-dotnet@v4.3.1
      with:
        dotnet-version: "9.0.x"

    - name: Build and pack
      run: dotnet pack -c Release

    - name: Push to NuGet
      run: dotnet nuget push artifacts/packages/NetEscapades.AspNetCore.SecurityHeaders.nupkg
      env:
        NuGetToken: ${{ secrets.NUGET_TOKEN }}

    - name: Generate JSON SBOM
      uses: CycloneDX/gh-dotnet-generate-sbom@v1.0.1
      with:
        path: ./src/NetEscapades.AspNetCore.SecurityHeaders/NetEscapades.AspNetCore.SecurityHeaders.csproj
        out: ./artifacts/sboms
        json: true
        github-bearer-token: ${{ secrets.GITHUB_TOKEN }}

    # Add this attestation step
    - name: Attest package
      uses: actions/attest-sbom@v2.2.0
      with:
        subject-path: artifacts/packages/NetEscapades.AspNetCore.SecurityHeaders.nupkg
        sbom-path: artifacts/sboms/bom.json
```

**Key Points:**

- The workflow is set to run on every PR, branch, and tag by default, but can be restricted as needed (e.g., only on releases).
- The SBOM is generated in CycloneDX format, but SPDX is also supported.
- Attestation steps require additional permissions (`id-token: write` and `attestations: write`).

## Resulting Attestation Artifacts and Output

The `actions/attest-sbom` action creates a Sigstore bundle (JSON document) representing the attestation, which includes information about:

- The artifact
- The SBOM
- The verification material (digital signatures, logs)

The run summary in GitHub Actions will also display attestation links for easy access.

![Attestation Workflow Summary Example](/content/images/2025/sbom_01.png)

If you click these links, you're taken to a display page where you can view or download the attestation in a user-friendly format.

![Attestation Display Page Example](/content/images/2025/sbom_02.png)

## Verifying SBOM Attestations

Attestations add value only if they are verified by consumers. Andrew shows how verification can be performed using the GitHub CLI:

```bash
gh attestation verify \
  --owner andrewlock \
  --predicate-type https://cyclonedx.org/bom \
  <filename-or-url>
```

- For SPDX SBOMs, use `--predicate-type https://spdx.dev/Document/v2.3`
- If omitted, the CLI verifies the package's provenance attestation by default

### Example Output

```bash
gh attestation verify --owner andrewlock --predicate-type https://cyclonedx.org/bom "NetEscapades.AspNetCore.SecurityHeaders.1.0.0-preview.4.nupkg"

# ... output indicating policy, verification succeeded, and matching attestation information
```

## Limitations for NuGet Packages

Verifying attestations for NuGet packages from nuget.org presents challenges:

- nuget.org modifies uploaded *.nupkg* files by adding its own signature file, invalidating earlier attestations
- Removing the added signature is only effective if the package isn't author-signed
- Author-signed packages have their *.signature.p7s* file altered/countersigned, which can't be "cleaned"
- Thus, provenance/SBOM attestations can't always be reliably verified for packages retrieved from nuget.org
- However, NuGet native signatures provide some, but not equivalent, assurance

## Summary

This guide builds on previous posts about provenance and SBOMs, describing how to automate SBOM attestation as part of the CI/CD workflow in GitHub Actions. While generating attestations is straightforward with available Actions, consuming/verifying attestations from nuget.org packages remains hindered by the repository's modifications. Still, these practices illustrate good supply chain hygiene and point the way toward better security practices in the .NET ecosystem.

---

**Author:** Andrew Lock  
[.NET Escapades](https://andrewlock.net/)

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-sbom-attestations-in-github-actions/)
