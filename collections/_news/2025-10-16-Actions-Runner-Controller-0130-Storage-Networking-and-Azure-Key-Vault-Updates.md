---
external_url: https://github.blog/changelog/2025-10-16-actions-runner-controller-release-0-13-0
title: 'Actions Runner Controller 0.13.0: Storage, Networking, and Azure Key Vault Updates'
author: Allison
feed_name: The GitHub Blog
date: 2025-10-16 14:01:35 +00:00
tags:
- Actions
- Actions Runner Controller
- ARC
- Azure Key Vault
- CI/CD
- Container Lifecycle Hooks
- CSI Driver
- GitHub Actions
- Kubernetes
- Managed Identity
- Metrics
- Networking
- Observability
- Red Hat OpenShift
- Secrets Management
- Vault Integration
- Azure
- DevOps
- Security
- News
section_names:
- azure
- devops
- security
primary_section: azure
---
Allison details what's new in Actions Runner Controller 0.13.0, spotlighting improvements in storage, networking, Azure Key Vault integration, security, platform compatibility, and observability for GitHub Actions workflows.<!--excerpt_end-->

# Actions Runner Controller 0.13.0: Storage, Networking, and Azure Key Vault Updates

**Author:** Allison

Actions Runner Controller (ARC) 0.13.0 delivers several enhancements for teams running GitHub Actions on self-hosted runners in Kubernetes environments. Below is a breakdown of the key features and improvements in this release.

## Storage Improvements with Container Lifecycle Hooks

- ARC now supports [container lifecycle hooks](https://github.com/actions/runner-container-hooks/releases/tag/v0.8.0) to restore/export job filesystems between pods.
- This eliminates the requirement for ReadWriteMany (RWX) volumes, improving portability and performance by leveraging local storage.
- RWX is still supported for cases needing concurrent writes.
- To enable, update the `containerMode` in `values.yml` to `kubernetes-novolume`. [Documentation](https://docs.github.com/actions/tutorials/use-actions-runner-controller/deploy-runner-scale-sets#using-kubernetes-mode) offers more information.

## Networking Updates

- Dual-stack networking (IPv4 & IPv6) is now supported for runners and controller services.
- This change enables IPv6 on compatible clusters with IPv4 fallback, increasing compatibility.
- Teams should update network policies, firewalls, and ingress allow-lists to ensure IPv6 traffic is permitted.

## Platform and Security Advancements

- Azure Key Vault integration is now generally available:
  - Secure workflows without exposing secrets in the workflow context (`${{ secrets }}`).
  - Designed for both cloud and on-premises workflows.
  - Microsoft recommends managed identity for accessing Azure Key Vault, avoiding secret/certificate management.
  - Secrets Store CSI driver is also supported for Kubernetes deployments.
- Red Hat OpenShift support is now generally available, expanding ARC's platform reach.
- The JIT token is no longer stored in the ephemeral runner status field, reducing exposure risk and improving security.

## Metrics and Observability

- New distinct labels: `workflow_name` and target labels for runners and metrics for easier filtering and alerting.
- The legacy `job_workflow_ref` label remains in 0.13.0 for backward compatibility, but will be removed in 0.14.0—users should update dashboards, alerts, and automations accordingly.

## Additional Resources

- [Release Notes 0.13.0](https://github.com/actions/actions-runner-controller/releases/tag/gha-runner-scale-set-0.13.0)
- [Container Lifecycle Hooks Release](https://github.com/actions/runner-container-hooks/releases/tag/v0.8.0)
- [Deploy Runner Scale Sets Documentation](https://docs.github.com/actions/tutorials/use-actions-runner-controller/deploy-runner-scale-sets#using-kubernetes-mode)

These enhancements focus on increasing portability, security, and adaptability in enterprise GitHub Actions environments leveraging Kubernetes and Azure services.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-16-actions-runner-controller-release-0-13-0)
