---
layout: "post"
title: "Simplifying Image Signing with Notary Project and Artifact Signing (GA)"
description: "This article explains how combining the CNCF Notary Project with Microsoft's Artifact Signing service (formerly Trusted Signing) streamlines secure image signing in Azure. It demonstrates how Notary Project, Notation, Ratify, and Artifact Signing enforce OCI artifact security in CI/CD pipelines and Kubernetes environments, with details for practical adoption."
author: "YiZha"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/simplifying-image-signing-with-notary-project-and-artifact/ba-p/4487942"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-01-22 00:21:10 +00:00
permalink: "/2026-01-22-Simplifying-Image-Signing-with-Notary-Project-and-Artifact-Signing-GA.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["AKS", "Artifact Signing", "Azure", "Azure Artifact Signing", "Azure Container Registry", "Azure Identities", "CI/CD", "Cloud Native", "Community", "Container Security", "DevOps", "Image Signing", "Kubernetes", "Managed Signing Service", "Notary Project", "Notation CLI", "OCI Artifacts", "Ratify", "Role Based Access Control", "Security", "Short Lived Certificates", "Trusted Signing"]
tags_normalized: ["aks", "artifact signing", "azure", "azure artifact signing", "azure container registry", "azure identities", "cislashcd", "cloud native", "community", "container security", "devops", "image signing", "kubernetes", "managed signing service", "notary project", "notation cli", "oci artifacts", "ratify", "role based access control", "security", "short lived certificates", "trusted signing"]
---

YiZha shares how integrating the Notary Project with Azure Artifact Signing (GA) improves the image signing process in cloud-native applications, simplifying secure deployment workflows for teams.<!--excerpt_end-->

# Simplifying Image Signing with Notary Project and Artifact Signing (GA)

Securing container images is essential for protecting modern cloud-native applications, ensuring images are authentic, untampered, and created by trusted publishers. This article introduces an updated approach by combining the [Notary Project](https://notaryproject.dev/), the CNCF standard for signing and verifying OCI artifacts, with [Artifact Signing](https://learn.microsoft.com/en-us/azure/artifact-signing/overview) (formerly Trusted Signing), now generally available as a managed Azure service.

The Notary Project delivers an open, interoperable framework to sign and verify container images and other OCI artifacts. Tools such as **Notation** and **Ratify** facilitate enforcement of these signatures in CI/CD pipelines and Kubernetes environments, such as Azure Kubernetes Service (AKS). Artifact Signing further simplifies the adoption of image signing by managing operational complexities like certificate management through:

- **Short-lived certificates**
- **Verified Azure identities**
- **Role-based access control (RBAC)**

These features work together without changing the foundational signing standards, allowing teams to focus on security without administrative overhead.

If you've previously used Trusted Signing for container image signing, workflows remain the same, with updated terminology and seamless Notary Project–based integrations. This integration ensures backward compatibility while extending security guarantees as Artifact Signing becomes generally available.

Together, the Notary Project and Artifact Signing empower teams to adopt image signing as a scalable, platform-integrated capability. Verified images move confidently from build to deployment, establishing a trusted supply chain for your applications.

## Get Started

- [Sign container images using Notation CLI](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-sign-verify-notation-artifact-signing)
- [Sign container images in CI/CD pipelines](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-github-sign-notation-artifact-signing)
- [Verify container images in CI/CD pipelines](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-github-verify-notation-artifact-signing)
- [Verify container images in AKS](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-verify-with-ratify-aks)
- [Extend signing and verification to all OCI artifacts in registries](https://learn.microsoft.com/en-us/azure/container-registry/overview-sign-verify-artifacts)

## Related Content

- [Simplifying Code Signing for Windows Apps: Artifact Signing (GA)](https://techcommunity.microsoft.com/blog/microsoft-security-blog/simplifying-code-signing-for-windows-apps-artifact-signing-ga/4482789)
- [Simplify Image Signing and Verification with Notary Project (preview article)](https://techcommunity.microsoft.com/blog/appsonazureblog/simplify-image-signing-and-verification-with-notary-project-and-trusted-signing-/4455292)

*Updated Jan 20, 2026*

*Author: YiZha*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/simplifying-image-signing-with-notary-project-and-artifact/ba-p/4487942)
