---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/simplify-image-signing-and-verification-with-notary-project-and/ba-p/4455292
title: Simplify Image Signing and Verification with Notary Project and Trusted Signing (Public Preview)
author: YiZha
feed_name: Microsoft Tech Community
date: 2025-09-25 15:23:36 +00:00
tags:
- AKS
- Artifact Signing
- Azure DevOps
- Azure Key Vault
- Azure Trusted Signing
- Certificate Management
- CI/CD Pipeline
- Container Image Security
- GitHub Actions
- Helm Charts
- Image Verification
- Notary Project
- Notation CLI
- OCI Artifacts
- Ratify
- SBOM
- Supply Chain Security
section_names:
- azure
- devops
- security
---
YiZha details how to strengthen your supply chain security using Notary Project and Azure Trusted Signing. This walkthrough explores standard-based image signing and verification, and shows how developers can streamline certificate management for artifacts in Azure-focused pipelines.<!--excerpt_end-->

# Simplify Image Signing and Verification with Notary Project and Trusted Signing (Public Preview)

Securing the supply chain for cloud-native applications is more important than ever. Each container image, SBOM, Helm chart, or AI model you use in your CI/CD pipeline could be a vector for attacks if its authenticity can't be verified. This article introduces how the Notary Project and Azure Trusted Signing simplify and enhance artifact signing and verification across your dev lifecycle.

## Why Image Signing Matters

- **Integrity**: Verifies that artifacts are unchanged since publishing.
- **Authenticity**: Confirms that artifacts come from the correct publisher.

With proper signing and verification, only approved and trusted content is permitted to run in production, significantly reducing the risk of deploying compromised images.

## Notary Project: Standardizing Artifact Signing

The CNCF's Notary Project provides well-defined standards to sign and verify a variety of OCI artifacts (including container images, SBOMs, Helm charts, and AI models) across diverse tools and platforms. Key components are:

- **Notation**: A CLI tool that allows developers and CI/CD processes to sign artifacts after build, and consumers to verify before usage.
- **Ratify**: A verification engine that integrates with Azure policy and Azure Kubernetes Service (AKS), enforcing that only signed images are allowed in a cluster.

Together, these tools create a consistent security workflow from build through runtime.

## Azure Trusted Signing: Hassle-Free Certificate Management

Traditionally, signing required issuing, renewing, and rotating certificates, adding operational complexity (often via Azure Key Vault). Azure Trusted Signing simplifies this by providing:

- Zero-touch certificate lifecycle management (no manual steps required)
- Short-lived certificates to reduce attack surface
- Built-in timestamping to keep signatures valid after certificate expiration

Developers can thus focus on shipping software instead of managing cryptographic material.

## Practical End-to-End Scenarios

- **Signing in CI/CD Pipelines**: Integrate image signing with GitHub Actions or Azure DevOps, so every artifact comes with a verifiable signature.
- **Verification in AKS**: Use Ratify and Azure Policy to enforce at the deployment level that only signed images can run in production clusters.
- **Verification During Builds**: Confirm upstream dependencies and base images are signed before they're used in builds.
- **Universal Workflow**: Apply these practices not just to container images, but also SBOMs, Helm charts, and AI artifacts.

## Learn More & Get Started

- [Overview: Ensuring integrity and authenticity of container images and OCI artifacts](https://learn.microsoft.com/en-us/azure/container-registry/overview-sign-verify-artifacts)
- [Sign and verify images with Notation CLI and Trusted Signing](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-sign-verify-notation-trusted-signing)
- [GitHub Actions integration: Sign with Trusted Signing](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-github-sign-notation-trusted-signing)
- [Verify signatures in GitHub Actions](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-github-verify-notation-trusted-signing)
- [Enforce verification on AKS with Ratify](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-verify-with-ratify-aks)

## Conclusion

Adopting the Notary Project standards and Azure Trusted Signing makes supply chain security more robust and easier to manage. By automating artifact signing and verification across your build and deployment processes—with minimal certificate management overhead—you ensure only trusted software runs in your Azure environment.

---

*Author: YiZha*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/simplify-image-signing-and-verification-with-notary-project-and/ba-p/4455292)
