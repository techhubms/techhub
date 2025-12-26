---
layout: "post"
title: "Creating Provenance Attestations for NuGet Packages in GitHub Actions"
description: "Andrew Lock explores software provenance, GitHub’s attestation features, and step-by-step creation of attestations for NuGet packages within GitHub Actions workflows. The post highlights the value, limitations, and practical verification methods, offering workarounds for NuGet.org’s package modifications."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/creating-provenance-attestations-for-nuget-packages-in-github-actions/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-03-18 09:00:00 +00:00
permalink: "/posts/2025-03-18-Creating-Provenance-Attestations-for-NuGet-Packages-in-GitHub-Actions.html"
categories: ["Coding", "DevOps", "Security"]
tags: [".NET", "Artifact Verification", "Attestation", "CI/CD", "Coding", "DevOps", "GitHub", "GitHub Actions", "NuGet", "OpenID Connect", "Package Signing", "Posts", "Security", "Security Best Practices", "Sigstore", "SLSA", "Software Provenance", "Supply Chain Security", "Workflow Automation"]
tags_normalized: ["dotnet", "artifact verification", "attestation", "cislashcd", "coding", "devops", "github", "github actions", "nuget", "openid connect", "package signing", "posts", "security", "security best practices", "sigstore", "slsa", "software provenance", "supply chain security", "workflow automation"]
---

Andrew Lock examines how developers can create provenance attestations for NuGet packages using GitHub Actions. He details the underlying mechanics, security implications, verification methods, and practical challenges, including how to address NuGet.org’s modifications for reliable attestation.<!--excerpt_end-->

# Creating Provenance Attestations for NuGet Packages in GitHub Actions

**Author: Andrew Lock**

## Introduction

This article discusses the concept of software provenance, the mechanics behind attestations for software artifacts, and step-by-step guidance on creating a signed attestation for a NuGet package via GitHub Actions. The discussion also candidly addresses practical obstacles, such as the impact of NuGet.org's artifact modifications, and explores ways to effectively verify build provenance.

---

## What is Software Provenance?

[Software provenance](https://slsa.dev/spec/v1.0/provenance) refers to verifiable evidence about a software artifact’s origins, integrity, and build environment, providing assurance the software has not been tampered with. It's an essential part of the supply chain for secure software.

The [Supply-chain Levels for Software Artifacts (SLSA)](https://slsa.dev/) framework identifies potential compromise points in the software life cycle and offers guidance and controls to mitigate those threats.

> ![](/content/images/2025/supply-chain-threats.svg) Threats to the software supply chain. From [SLSA](https://slsa.dev/spec/v1.0/threats-overview)

**Provenance attestations** are signed JSON documents detailing who built the artifact, where and how it was built, and under what process and environment. Such documents are stored either publicly or internally, allowing consumers to validate the origin and integrity of software artifacts.

> **Note:** While provenance alone does not enhance security, it enables verification of artifacts and helps prevent tampering. Organizations can enforce deployment policies based on attestations but must continue to practice secure development, including code reviews and dependency management.

## How GitHub Generates Attestations

As of May 2024, GitHub released artifact attestations as a public beta, later making them generally available. The implementation emphasizes simplicity and security via managed workflows without the need for user-handled, long-lived credentials.

**Process Overview:**

- GitHub’s [`actions/attest-build-provenance`](https://github.com/actions/attest-build-provenance) action initiates the process.
- A Sigstore client is invoked, requests the [GitHub Actions OIDC token](https://docs.github.com/en/actions/security-for-github-actions/security-hardening-your-deployments/about-security-hardening-with-openid-connect#understanding-the-oidc-token) (unique per CI job), and generates a keypair.
- The public key and OIDC token are sent to [Fulcio](https://github.com/sigstore/fulcio), which returns a short-lived X.509 certificate.
- Steps:
  - Compute artifact’s SHA-256 digest.
  - Construct a provenance statement as an [in-toto](https://github.blog/news-insights/product-news/introducing-artifact-attestations-now-in-public-beta/) JSON blob.
  - Sign provenance using the private key (which is discarded afterward).
  - The signature is timestamped.
  - The attestation bundle is stored by GitHub and optionally in Sigstore’s public instance (for public repositories).

The process offers a tamper-proof and automated way to generate attestations for artifacts produced in GitHub Actions.

## Creating an Attestation for a NuGet Package in GitHub Actions

**Example workflow:** The starting point is a CI workflow that builds a .NET solution, creates a NuGet package, and publishes it to NuGet.org.

```yaml
name: BuildAndPack
on:
  push:
    branches: ["main"]
    tags: ['*']
  pull_request:
    branches: ['*']
jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@11bd71901bbe5b1630ceea73d27597364c9af683 # v4.2.2
      - uses: actions/setup-dotnet@67a3573c9a986a3f9c594539f4ab511d57bb3ce9 # v4.3.1
        with:
          dotnet-version: |
            1. 0.x
            2. 0.x
            3. 0.x
            4. 1.x
      - name: Build and pack
        run: dotnet pack -c Release
      - name: Push to NuGet
        run: dotnet nuget push artifacts/packages/*.nupkg
        env:
          NuGetToken: ${{ secrets.NUGET_TOKEN }}
      - uses: actions/upload-artifact@4cec3d8aa04e39d1a68397de0c4cd6fb9dce8ec1 # v4.6.1
        with:
          name: packages
          path: artifacts/packages
```

> **Security Note:** Pin your actions to specific commit hashes to mitigate supply-chain attacks on reusable GitHub Actions.

### Adding Attestation Generation

To add attestation, introduce a step using the GitHub attestation action and ensure appropriate permissions for OIDC and artifact attestations:

```yaml
- name: Generate artifact attestation
  uses: actions/attest-build-provenance@c074443f1aee8d4aeeae555aebba3282517141b2 # v2.2.3
  with:
    subject-path: 'artifacts/packages/*.nupkg'
```

Set job-level permissions:

```yaml
permissions:
  id-token: write
  contents: read
  attestations: write
```

**Full Example:**

```yaml
name: BuildAndPack
on:
  push:
    branches: ["main"]
    tags: ['*']
  pull_request:
    branches: ['*']
jobs:
  build-and-test:
    runs-on: ubuntu-latest
    permissions:
      id-token: write
      contents: read
      attestations: write
    steps:
      - uses: actions/checkout@11bd71901bbe5b1630ceea73d27597364c9af683 # v4.2.2
      - uses: actions/setup-dotnet@67a3573c9a986a3f9c594539f4ab511d57bb3ce9 # v4.3.1
        with:
          dotnet-version: |
            1. 0.x
            2. 0.x
            3. 0.x
            4. 1.x
      - name: Build and publish
        run: dotnet publish -c Release
      - name: Build and publish
        run: dotnet nuget push artifacts/packages/*.nupkg
        env:
          NuGetToken: ${{ secrets.NUGET_TOKEN }}
      - name: Generate artifact attestation
        uses: actions/attest-build-provenance@c074443f1aee8d4aeeae555aebba3282517141b2 # v2.2.3
        with:
          subject-path: 'artifacts/packages/*.nupkg'
      - uses: actions/upload-artifact@4cec3d8aa04e39d1a68397de0c4cd6fb9dce8ec1 # v4.6.1
        with:
          name: packages
          path: artifacts/packages
```

After a successful run, the job summary includes a link to the [public GitHub attestation](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders/attestations/5148236) for each artifact.

**SLSA Levels:** This method achieves SLSA v1.0 Build Level 2. Level 3 requires externalized, vetted build instructions (e.g., reusable workflows).

- [GitHub guidance on Level 3](https://docs.github.com/en/actions/security-for-github-actions/using-artifact-attestations/using-artifact-attestations-and-reusable-workflows-to-achieve-slsa-v1-build-level-3)
- [SLSA project generators](https://github.com/slsa-framework/slsa-github-generator/tree/main)

## Verifying an Attestation via GitHub CLI

Attestations must be verified to add actual security value. For container images (e.g., Kubernetes), consider enforcing attestation via [admission controllers](https://docs.github.com/en/actions/security-for-github-actions/using-artifact-attestations/enforcing-artifact-attestations-with-a-kubernetes-admission-controller). For other scenarios, verification can be manual or automated offline.

**Using GitHub CLI:**

```bash
gh attestation verify <path/to/artifact/to/verify> -R <org/repo>
```

Example:

```bash
gh attestation verify --repo andrewlock/NetEscapades.AspNetCore.SecurityHeaders "./NetEscapades.AspNetCore.SecurityHeaders.1.0.0-preview.3.nupkg"
```

If the attestation matches, verification will succeed, ensuring the artifact’s provenance matches expectations.

## Practical Limitation: NuGet.org Alters Packages

A key challenge is that NuGet.org modifies uploaded NuGet packages (adding signature files), changing their SHA value. This disrupts the linkage between the artifact built in GitHub Actions and the artifact downloaded from NuGet.org. Consequently, the GitHub-generated attestation no longer matches the published artifact.

> Attempts to verify a public NuGet package using an attestation generated before upload will **fail** due to this modification. This is by design and has been confirmed via [issue discussions](https://github.com/NuGet/NuGetGallery/issues/10026) and direct comparisons of artifact files.

While NuGet.org’s signature file offers its own security guarantees, there is—at the time of writing—no simple way for .NET build pipelines or clients to verify GitHub provenance attestations against public NuGet packages.

## Workaround: Removing the Signature File

A workaround is possible: remove the signature file (*.signature.p7s*) NuGet.org appends via unzip utilities, restoring the pre-upload file hash and enabling attestation verification.

**Linux:**

```bash
zip -d NetEscapades.AspNetCore.SecurityHeaders.1.0.0-preview.4.nupkg .signature.p7s
gh attestation verify --owner andrewlock "C:\Users\sock\Downloads\NetEscapades.AspNetCore.SecurityHeaders.1.0.0-preview.4.nupkg"
```

**PowerShell (Windows/.NET):**

```powershell
$zipfile = "C:\Users\sock\Downloads\NetEscapades.AspNetCore.SecurityHeaders.1.0.0-preview.4.nupkg"
[Reflection.Assembly]::LoadWithPartialName('System.IO.Compression')
$stream = New-Object IO.FileStream($zipfile, [IO.FileMode]::Open)
$zip = New-Object IO.Compression.ZipArchive($stream, [IO.Compression.ZipArchiveMode]::Update)
$zip.Entries | ? { ---
layout: "post"
title: "Creating Provenance Attestations for NuGet Packages in GitHub Actions"
description: "Andrew Lock explores software provenance, GitHub’s attestation features, and step-by-step creation of attestations for NuGet packages within GitHub Actions workflows. The post highlights the value, limitations, and practical verification methods, offering workarounds for NuGet.org’s package modifications."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/creating-provenance-attestations-for-nuget-packages-in-github-actions/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: https://andrewlock.net/rss.xml
date: 2025-03-18 09:00:00 +00:00
permalink: "2025-03-18-Creating-Provenance-Attestations-for-NuGet-Packages-in-GitHub-Actions.html"
categories: ["Coding", "DevOps", "Security"]
tags: [".NET", "Artifact Verification", "Attestation", "CI/CD", "Coding", "DevOps", "GitHub", "GitHub Actions", "NuGet", "OpenID Connect", "Package Signing", "Posts", "Security", "Security Best Practices", "Sigstore", "SLSA", "Software Provenance", "Supply Chain Security", "Workflow Automation"]
tags_normalized: [["net", "artifact verification", "attestation", "ci slash cd", "coding", "devops", "github", "github actions", "nuget", "openid connect", "package signing", "posts", "security", "security best practices", "sigstore", "slsa", "software provenance", "supply chain security", "workflow automation"]]
---

Andrew Lock examines how developers can create provenance attestations for NuGet packages using GitHub Actions. He details the underlying mechanics, security implications, verification methods, and practical challenges, including how to address NuGet.org’s modifications for reliable attestation.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-provenance-attestations-for-nuget-packages-in-github-actions/)
.Name -eq ".signature.p7s" } | % { ---
layout: "post"
title: "Creating Provenance Attestations for NuGet Packages in GitHub Actions"
description: "Andrew Lock explores software provenance, GitHub’s attestation features, and step-by-step creation of attestations for NuGet packages within GitHub Actions workflows. The post highlights the value, limitations, and practical verification methods, offering workarounds for NuGet.org’s package modifications."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/creating-provenance-attestations-for-nuget-packages-in-github-actions/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: https://andrewlock.net/rss.xml
date: 2025-03-18 09:00:00 +00:00
permalink: "2025-03-18-Creating-Provenance-Attestations-for-NuGet-Packages-in-GitHub-Actions.html"
categories: ["Coding", "DevOps", "Security"]
tags: [".NET", "Artifact Verification", "Attestation", "CI/CD", "Coding", "DevOps", "GitHub", "GitHub Actions", "NuGet", "OpenID Connect", "Package Signing", "Posts", "Security", "Security Best Practices", "Sigstore", "SLSA", "Software Provenance", "Supply Chain Security", "Workflow Automation"]
tags_normalized: [["net", "artifact verification", "attestation", "ci slash cd", "coding", "devops", "github", "github actions", "nuget", "openid connect", "package signing", "posts", "security", "security best practices", "sigstore", "slsa", "software provenance", "supply chain security", "workflow automation"]]
---

Andrew Lock examines how developers can create provenance attestations for NuGet packages using GitHub Actions. He details the underlying mechanics, security implications, verification methods, and practical challenges, including how to address NuGet.org’s modifications for reliable attestation.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-provenance-attestations-for-nuget-packages-in-github-actions/)
.Delete() }
$zip.Dispose()
```

The removal reverts the package state, restoring the original hash and enabling successful verification of the provenance attestation.

## Summary

- Provenance attestations, facilitated by GitHub Actions, support verification and supply chain security for software artifacts.
- While workflow integration is straightforward, attestation verification for NuGet packages remains problematic due to package mutation by NuGet.org.
- Workarounds exist for advanced users, but native, automated support from NuGet is not yet available.
- NuGet.org's built-in signature mechanism attempts to provide similar guarantees, but has different trust and usability characteristics.

---
**Stay Up to Date:** For more insights, follow Andrew Lock and the .Net Escapades blog.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-provenance-attestations-for-nuget-packages-in-github-actions/)
