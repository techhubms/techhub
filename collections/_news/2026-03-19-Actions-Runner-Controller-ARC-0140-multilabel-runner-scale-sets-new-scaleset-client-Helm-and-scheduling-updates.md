---
section_names:
- devops
primary_section: devops
author: Allison
date: 2026-03-19 19:17:41 +00:00
external_url: https://github.blog/changelog/2026-03-19-actions-runner-controller-release-0-14-0
tags:
- Actions
- Actions Runner Controller
- Actions/scaleset
- ARC
- Autoscaling
- DevOps
- Exit Code 7
- GitHub Actions
- Go
- Helm
- Kubernetes
- Listener Pod
- Multilabel Support
- News
- Node Selector
- Platform Engineering
- Runner Scale Sets
- Runs On
- Self Hosted Runners
title: 'Actions Runner Controller (ARC) 0.14.0: multilabel runner scale sets, new scaleset client, Helm and scheduling updates'
feed_name: The GitHub Blog
---

Allison announces GitHub Actions Runner Controller (ARC) 0.14.0 general availability and outlines what changed for teams running self-hosted runners on Kubernetes, including multilabel runner scale sets, a new `actions/scaleset` client, Helm chart updates, and safer autoscaling and scheduling behavior.<!--excerpt_end-->

## Actions Runner Controller release 0.14.0

GitHub Actions Runner Controller (ARC) 0.14.0 is now generally available. This release introduces multilabel support for runner scale sets, switches to the `actions/scaleset` library client, adds resource customization options, and improves listener pod scheduling.

To get started, see the ARC documentation:

- https://docs.github.com/actions/hosting-your-own-runners/managing-self-hosted-runners-with-actions-runner-controller/about-actions-runner-controller

## Multilabel support for runner scale sets

You can now assign multiple labels to a single runner scale set, enabling workflows to target runners based on combined attributes.

Previously, ARC only supported one label per scale set, which meant creating separate scale sets for every combination of:

- Operating system
- Hardware tier
- Network configuration
- Compliance zone

With multilabel support, you can define all of these attributes on a single scale set and use them together in your workflow `runs-on` declarations.

## Scaleset library client

ARC now uses the `actions/scaleset` library as its sole client for communicating with the GitHub Actions service APIs:

- https://github.com/actions/scaleset

This release removes the previous internal client in favor of a standalone, publicly available Go package. This enables platform teams and infrastructure providers to build custom autoscaling solutions using the same client that powers ARC.

## Custom labels and annotations on internal resources

You can now apply custom Kubernetes labels and annotations to the resources that ARC manages internally, including:

- Roles
- Role bindings
- Service accounts
- Listener pods

Both Helm charts expose a `resource` configuration interface:

- `gha-runner-scale-set`
- `gha-runner-scale-set-controller`

Configuration highlights:

- Set metadata globally across all resources using `resource.all.metadata`.
- Target individual resources for fine-grained control.

## Experimental Helm charts

This release introduces experimental rewrites of both the `gha-runner-scale-set` and `gha-runner-scale-set-controller` Helm charts.

These experimental charts provide:

- Cleaner template structure
- A unified interface for annotating and labeling internal resources
- Improved configuration for Docker-based runner setups

They ship alongside the existing charts and are intended for early feedback.

## Autoscaling stops for outdated runner sets

ARC can now fully stop autoscaling for a runner set when the runner configuration is outdated.

Behavior:

- When a runner exits with exit code `7`, the controller switches off autoscaling for that runner set.
- This prevents stale runners from provisioning while a new configuration rolls out.
- The goal is to reduce the risk of jobs running in inconsistent environments.

Dependency note:

- This depends on a runner change (exit code `7`) that will ship in the next runner release.
- Due to the runner version support policy, this feature won’t become fully effective until two releases after the runner change ships.

## Listener pod defaults to Linux nodes

The listener pod now includes a default `nodeSelector`:

- `kubernetes.io/os: linux`

In mixed-OS clusters, this prevents the listener from accidentally scheduling on a Windows node, where it would fail due to platform incompatibility.

You can still override or remove this default via the `listenerTemplate` configuration.


[Read the entire article](https://github.blog/changelog/2026-03-19-actions-runner-controller-release-0-14-0)

