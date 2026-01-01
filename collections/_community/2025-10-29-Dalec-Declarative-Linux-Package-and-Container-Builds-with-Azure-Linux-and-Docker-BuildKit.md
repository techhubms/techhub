---
layout: "post"
title: "Dalec: Declarative Linux Package and Container Builds with Azure Linux and Docker BuildKit"
description: "This detailed guide introduces Dalec, a CNCF Sandbox project that modernizes Linux package and container image building using declarative YAML specifications and Docker BuildKit. Dalec supports multiple distributions, including Azure Linux, and can integrate into CI/CD pipelines, streamline compliance with SBOMs and signing, and simplify complex packaging workflows for developers and operators."
author: "SertacOzercan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/dalec-declarative-package-and-container-builds/ba-p/4465290"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-29 20:33:52 +00:00
permalink: "/2025-10-29-Dalec-Declarative-Linux-Package-and-Container-Builds-with-Azure-Linux-and-Docker-BuildKit.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Azure", "Azure Linux", "CI/CD", "Community", "Container Images", "Dalec", "Declarative Configuration", "DevOps", "Docker BuildKit", "Kubernetes", "Linux Packaging", "Multi Architecture Builds", "Package Signing", "Provenance", "RPM", "SBOM", "Security", "YAML"]
tags_normalized: ["azure", "azure linux", "cislashcd", "community", "container images", "dalec", "declarative configuration", "devops", "docker buildkit", "kubernetes", "linux packaging", "multi architecture builds", "package signing", "provenance", "rpm", "sbom", "security", "yaml"]
---

SertacOzercan presents Dalec, a declarative BuildKit frontend for building system packages and containers across distributions like Azure Linux. The guide highlights key benefits for developers, operators, and security-conscious teams.<!--excerpt_end-->

# Dalec: Declarative Linux Package and Container Builds with Azure Linux and Docker BuildKit

## Overview

Dalec, a Cloud Native Computing Foundation (CNCF) Sandbox project, is a powerful tool designed to simplify and modernize Linux package and container image creation. Using a single YAML specification, Dalec enables reproducible builds targeting both traditional packages (RPM, DEB) and minimal container images. It is implemented as a Docker BuildKit frontend, requiring only Docker to get started—no other tools, agents, or complex pipelines are necessary.

## Supported Platforms

- **Azure Linux** (first-class support)
- AlmaLinux
- Rocky Linux
- Debian
- Ubuntu
- Expandable to others via pluggable backends

## Key Features and Benefits

- **Zero Installation Overhead**: No extra dependencies—works out of the box with standard Docker
- **Built for Speed**: BuildKit-powered caching speeds up rebuilds
- **Unified Specification**: Replace scattered Dockerfiles and spec files with one YAML config
- **Compliance and Security**: Built-in support for SBOM generation, provenance attestations, and package signing
- **Multi-Distribution**: Build RPM and DEB packages, as well as containers, targeting several Linux distributions from the same spec
- **Composability**: Declarative approach makes it easy to manage variations across targets
- **CI/CD Ready**: Seamlessly integrates into GitHub Actions, GitLab CI, Jenkins, or Kubernetes-native pipelines

## Who Benefits from Dalec?

- **Application Developers**: Create distribution-ready packages without deep knowledge of RPM or Debian conventions.
- **Platform Operators**: Centralize packaging standards and leverage reproducible, traceable build flows.
- **Package Maintainers**: Target multiple OS distributions with one source of truth, minimizing duplication.
- **Security and Compliance Teams**: Satisfy supply chain mandates via SBOMs and cryptographic provenance.

## How Dalec Works

Dalec serves as a Docker BuildKit frontend; just add a line to your YAML spec:

```
# syntax=ghcr.io/project-dalec/dalec/frontend:latest
```

### Build Workflow

1. **Source Acquisition**: Fetch code from Git, archives, or various package registries
2. **Dependency Management**: Automatically handles build- and run-time dependencies, including language modules (Go, Rust, etc.)
3. **Build Steps**: Configure build commands and environment
4. **Package Creation**: Outputs RPM or DEB packages, with optional container images
5. **Testing**: Installs and validates packages in clean test environments
6. **Optional Containerization**: Build minimal containers using the produced packages

## Example: Multi-Target Build with Azure Linux

Dalec allows you to generate packages and container images for Azure Linux and other distributions with a simple YAML configuration. Here’s a conceptual snippet:

```yaml
# syntax=ghcr.io/project-dalec/dalec/frontend:latest

name: myapp
version: 1.0.0
sources:
  git:
    url: https://github.com/example/myapp.git
    commit: v1.0.0
dependencies:
  build:
    gcc:
  runtime:
    openssl:
build:
  steps:
    - command: |
        make && make install
image:
  entrypoint: /usr/bin/myapp
```

This configuration can target Ubuntu, Debian, or Azure Linux just by changing parameters.

## Real-World Scenarios

- **Open Source Distribution**: Replace scattered build scripts with one spec, producing cross-distro packages and images
- **Enterprise CI/CD**: Integrate into existing Docker-based CI/CD to automate SBOM and attestation workflows
- **Security-Critical Environments**: Guarantee traceability and trust with built-in package signing and provenance
- **Kubernetes-Optimized Images**: Output minimal containers (e.g., for Azure Kubernetes Service)

## Security and Compliance

Dalec is designed with supply chain security in mind:

- **SBOMs**: Get an auditable bill of materials for every build
- **Provenance Attestations**: BuildKit documents how, where, and from what your packages/images are built
- **Package Signing**: GPG-based signature support for tamper-evident distribution

References for further detail:

- [Dalec on GitHub](https://github.com/project-dalec/dalec)
- [Dalec Quickstart Guide](https://project-dalec.github.io/dalec/quickstart)
- [BuildKit Frontend Docs](https://docs.docker.com/build/buildkit/frontend/)

## Getting Started

1. Clone a sample YAML spec from [Dalec’s examples](https://github.com/Azure/dalec/tree/main/docs/examples)
2. Run a build: `docker build -f my-spec.yml .`
3. Review packaging results and SBOMs
4. Integrate with CI or cloud-native build systems if needed (including Kubernetes)

## Community and Contributions

- Join discussions on [CNCF Slack #dalec](https://cloud-native.slack.com/archives/C09MHVDGMAB)
- Learn more: [Dalec Documentation](https://project-dalec.github.io/dalec/)
- Contribute: [Contribution Guide](https://github.com/project-dalec/dalec/blob/main/CONTRIBUTING.md)

---

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/dalec-declarative-package-and-container-builds/ba-p/4465290)
