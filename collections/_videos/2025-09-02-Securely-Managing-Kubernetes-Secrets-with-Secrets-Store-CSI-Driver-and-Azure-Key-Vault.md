---
layout: post
title: Securely Managing Kubernetes Secrets with Secrets Store CSI Driver and Azure Key Vault
author: Microsoft Developer
canonical_url: https://www.youtube.com/watch?v=O1nx8mve5RY
viewing_mode: internal
feed_name: Microsoft Developer YouTube
feed_url: https://www.youtube.com/feeds/videos.xml?channel_id=UCsMica-v34Irf9KVTh6xx-g
date: 2025-09-02 16:39:25 +00:00
permalink: /azure/videos/Securely-Managing-Kubernetes-Secrets-with-Secrets-Store-CSI-Driver-and-Azure-Key-Vault
tags:
- Azure
- Azure Key Vault
- Cloud Computing
- Cloud Security
- Compliance
- Container Security
- Dev
- Development
- DevOps
- External Secrets
- Kubernetes
- Kubernetes SIG Auth
- Microsoft
- Microsoft Azure
- Secret Management
- Secrets Rotation
- Secrets Store CSI Driver
- Security
- Tech
- Technology
- Videos
section_names:
- azure
- devops
- security
---
Microsoft Developer presents a walkthrough on securely managing Kubernetes secrets with the Secrets Store CSI Driver, featuring Anish and Ben discussing Azure Key Vault integrations and real-world DevOps security scenarios.<!--excerpt_end-->

{% youtube O1nx8mve5RY %}

# Securely Managing Kubernetes Secrets with Secrets Store CSI Driver and Azure Key Vault

Kubernetes has long relied on native Secret objects for storing sensitive information. However, increasing security and compliance requirements push teams to adopt more robust solutions. In this episode, Anish and Ben introduce the open-source [Secrets Store CSI Driver](https://github.com/kubernetes-sigs/secrets-store-csi-driver), part of Kubernetes SIG Auth, which enables seamless integration with external secret stores like Azure Key Vault, Google Secret Manager, and AWS Secrets Manager.

## Why Externalize Secrets in Kubernetes?

- **Security and Compliance:** Native Secret objects are not always sufficient for strict security and auditing needs.
- **Secret Exposure Risk:** Minimizing the risk of secret leakage by controlling how secrets are delivered to applications.

## What is the Secrets Store CSI Driver?

- **CSI Driver Integration:** Installs as a Container Storage Interface (CSI) provider.
- **Mount at Runtime:** Delivers secrets into application pods as files, not as environment variables or native K8s secrets.
- **Zero Touch Rotation:** Enables automatic rotation and updating of secrets without redeploying or restarting applications.
- **Multi-Provider Support:** Integrates with Azure Key Vault, AWS, Google, and other secret stores through provider plugins.

## Developer Scenario: Using Azure Key Vault

- **Setup:** Configure the Kubernetes cluster with the CSI driver and Azure Key Vault provider.
- **Binding:** Specify which secrets should be made available to pods, mapping external secrets to pod file paths.
- **Access:** At runtime, the pods receive the latest secrets directly from Key Vault via the CSI volume.
- **Rotation Demonstration:** Secrets can be updated in Azure Key Vault, automatically propagating to consuming applications.

## Demo Highlights

- **How It Works:** Step-by-step setup of the CSI driver and establishing a connection with Azure Key Vault.
- **Live Secret Rotation:** Modifying secrets in Key Vault and observing their update in application pods without manual intervention.
- **Compliance:** Meets modern regulatory and security standards for cloud-native environments.

## Resources

- [Secrets Store CSI Driver Project](https://github.com/kubernetes-sigs/secrets-store-csi-driver)
- [CSI Driver Documentation](https://secrets-store-csi-driver.sigs.k8s.io/)

## Stay Connected

- [Anish Ramasekar LinkedIn](https://www.linkedin.com/in/anishramasekar)
- [Ben Petersen LinkedIn](https://www.linkedin.com/in/benjaminapetersen)
- [Open at Microsoft YouTube Playlist](https://aka.ms/OpenAtMicrosoftPlaylist)
- [Submit Your OSS Project](https://aka.ms/OpenAtMsCFP)

## Conclusion

The use of the Secrets Store CSI Driver with Azure Key Vault and other providers empowers Kubernetes developers to enforce best practices for secret delivery, automatic rotation, and compliance. This episode, hosted by Anish and Ben, offers practical guidance, real-world demos, and resources to get started.
