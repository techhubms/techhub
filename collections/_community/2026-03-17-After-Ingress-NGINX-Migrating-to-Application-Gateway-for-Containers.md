---
title: 'After Ingress NGINX: Migrating to Application Gateway for Containers'
section_names:
- azure
- devops
- security
date: 2026-03-17 21:03:22 +00:00
tags:
- AKS Networking
- ALB Controller
- Application Gateway For Containers (agc)
- Application Routing Add On
- Azure
- Azure CNI
- Azure CNI Overlay
- Azure Kubernetes Service (aks)
- Bicep
- CI/CD Pipelines
- Community
- DevOps
- Gateway API
- GitOps
- Ingress Annotations
- Ingress API
- Ingress Migration
- Ingress NGINX Retirement
- Kubenet Deprecation
- Kubernetes HTTP Routing
- Security
- Terraform
- Web Application Firewall (waf)
- Workload Identity
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/after-ingress-nginx-migrating-to-application-gateway-for/ba-p/4503110
feed_name: Microsoft Tech Community
primary_section: azure
author: samcogan
---

samcogan explains why teams running Ingress NGINX on AKS need to plan a migration, and walks through moving to Azure Application Gateway for Containers (AGC), including architecture basics, prerequisite checks (Azure CNI and workload identity), and using Microsoft’s AGC Migration Utility to generate Gateway API YAML and a coverage report.<!--excerpt_end-->

## Context: Ingress NGINX retirement timelines

If you're running **Ingress NGINX** on **Azure Kubernetes Service (AKS)**, the upstream project retirement affects your support window:

- Upstream *ingress-nginx* moves to best-effort maintenance until **March 2026**, then **no releases/bug fixes/security patches**.
- On AKS:
  - If you installed NGINX yourself (for example via **Helm**), you’re exposed to the **March 2026** deadline.
  - If you use the **Application Routing add-on**, Microsoft extends critical security patches until **November 2026** (no features or general bug fixes).

Reference: Kubernetes SIG Network and Security Response Committee announcement: https://www.kubernetes.dev/blog/2025/11/12/ingress-nginx-retirement/

## What is Application Gateway for Containers (AGC)?

**Application Gateway for Containers (AGC)** is Azure’s managed **Layer 7 load balancer** for AKS. It reached **GA in late 2024** and added **WAF support in November 2025**.

### Architecture overview

AGC is split into two planes:

- **Azure (data plane / control resource)**
  - An **AGC resource** outside the cluster that handles incoming traffic.
  - Includes child resources such as **frontends** (public entry points) with auto-generated **FQDNs**.
  - Uses a **delegated subnet** association in your VNet.
  - Unlike the older Application Gateway Ingress Controller approach, AGC is a **standalone Azure resource**.

- **Kubernetes (controller plane)**
  - The **ALB Controller** runs in-cluster as a small deployment.
  - It watches **Gateway API** resources (for example **Gateway** and **HTTPRoute**) and AGC-specific policy types.
  - It translates Kubernetes config into updates on the Azure AGC resource.

### API support

AGC supports both:

- **Gateway API** (richer functionality, where Kubernetes ingress is heading)
- **Ingress API** (useful for phased migrations)

## Deployment models for AGC

You can adopt AGC in two ways:

- **Bring Your Own (BYO)**
  - You create AGC Azure resources yourself (CLI/Portal/**Bicep**/**Terraform**/etc.).
  - ALB Controller references the existing AGC resource by ID.
  - Fits better with established IaC and Azure lifecycle control.

- **Managed by ALB Controller**
  - You create an `ApplicationLoadBalancer` custom resource in Kubernetes.
  - The controller creates and manages the Azure-side resources.
  - Easier to start, but ties Azure resource lifecycle to Kubernetes objects.

## Prerequisites to check before migrating

Key prerequisites called out:

- **Azure CNI** or **Azure CNI Overlay** is required.
  - **Kubenet** is deprecated and will be retired in **2028**.
  - If you’re on Kubenet, plan a CNI migration alongside this work.
  - Microsoft documents an **in-place cluster migration process**: https://learn.microsoft.com/en-us/azure/aks/concepts-network-legacy-cni
- **Workload identity** must be enabled on your cluster.

## Why choose AGC over in-cluster ingress alternatives?

The post highlights several practical reasons:

- **Data plane outside the cluster**
  - With NGINX, ingress pods consume node resources and require patching/upgrades.
  - With AGC, Azure runs the traffic-handling data plane; the in-cluster controller is lightweight and not in the traffic path.

- **Per-pod routing using Endpoint/EndpointSlice data**
  - AGC can route directly to backend **pod IPs** based on Kubernetes endpoint data.
  - This supports faster convergence as pods scale.

- **Built-in WAF support**
  - Uses Azure WAF policies.
  - Can remove the need for a separate Application Gateway in front of AKS purely for WAF.

- **Near-instant config propagation**
  - Avoids NGINX-style reload cycles when routes change.

- **Gateway API alignment and reduced lock-in**
  - Define routing in a more proxy-agnostic way, making future proxy swaps easier.

## Planning the migration

Before running tooling:

1. **Inventory what you have**
   - Count Ingress objects across clusters/namespaces.
   - Identify which **annotations** you rely on.
   - Flag NGINX-specific behaviors (custom snippets, Lua config, etc.).

2. **Confirm prerequisites**
   - Azure CNI/Azure CNI Overlay.
   - Workload identity enabled.
   - If Kubenet, migrate first.

3. **Pick a deployment model**
   - BYO vs Managed impacts resource lifecycle and IaC approach.

4. **Decide on API strategy**
   - Migrate to **Gateway API** now, or keep **Ingress API** short term and migrate later.

## AGC Migration Utility (Microsoft)

Microsoft released the **AGC Migration Utility** (January 2026):

- Repo: https://github.com/Azure/Application-Gateway-for-Containers-Migration-Utility
- It is an official CLI tool to convert existing **Ingress resources** into **Gateway API resources** compatible with AGC.
- It **does not modify** your cluster; it reads configuration and generates YAML for review.

### Important behavior

- The utility **only generates Gateway API resources**.
- There is **no option** to generate Ingress API resources for AGC.
  - If you want to keep Ingress API while moving to AGC, you must do that setup manually.

### Input modes

- **Files mode**: point at a directory of YAML manifests (no cluster access).
- **Cluster mode**: reads live Ingress resources from your current kubeconfig context.

### Migration report

Along with YAML output, the tool generates a report that classifies annotations as:

- `completed`
- `warning`
- `not-supported`
- `error`

The `warning` and `not-supported` entries are where manual remediation is needed.

The post notes broad annotation coverage including:

- URL rewrites
- SSL redirects
- session affinity
- backend protocol
- mTLS
- WAF
- canary routing (weight/header)
- permanent/temporary redirects
- custom hostnames

## Step-by-step migration flow

### 1) Get the utility

Download a pre-built binary (Linux/macOS/Windows) from:

- https://github.com/Azure/Application-Gateway-for-Containers-Migration-Utility/releases

Or build from source:

```bash
./build.sh
```

### 2) Dry-run to assess annotation coverage

Files mode:

```bash
./agc-migration files --provider nginx --ingress-class nginx --dry-run ./manifests/*.yaml
```

Cluster mode:

```bash
./agc-migration cluster --provider nginx --ingress-class nginx --dry-run
```

### 3) Review the migration report

Anything marked `not-supported` needs an explicit plan before proceeding.

### 4) Set up AGC and install ALB Controller

Follow the official quickstart:

- https://aka.ms/agc

If using BYO, record your **AGC resource ID** for generation.

### 5) Generate converted resources

Run with the appropriate deployment model flags.

```bash
# BYO
./agc-migration files --provider nginx --ingress-class nginx \
  --byo-resource-id $AGC_ID \
  --output-dir ./output \
  ./manifests/*.yaml

# Managed
./agc-migration files --provider nginx --ingress-class nginx \
  --managed-subnet-id $SUBNET_ID \
  --output-dir ./output \
  ./manifests/*.yaml
```

### 6) Review and apply generated resources

Validate the generated `Gateway`, `HTTPRoute`, and policy resources, ideally in non-prod first:

```bash
kubectl apply -f ./output/
```

### 7) Validate and cut over

- Test routes before DNS changes.
- Run NGINX and AGC in parallel while validating.
- Update DNS to point to the AGC frontend FQDN when ready.

### 8) Decommission NGINX

Remove the NGINX controller and old Ingress resources to avoid two controllers watching the same objects.

## What the utility doesn’t handle

The post calls out these gaps:

- **No equivalent for arbitrary NGINX config injection**
  - Custom snippets and Lua-based config don’t map to AGC/Gateway API.
  - You may need to redesign using `HTTPRoute` filters, rewrites, or header manipulation.

- **TLS certificates are not migrated**
  - Kubernetes Secrets may carry over, but you must verify certificate references in generated resources.

- **DNS cutover is out of scope**
  - AGC frontends get auto-generated FQDNs; you must update DNS/CNAME yourself.

- **GitOps/CI/CD pipeline updates**
  - Pipelines referencing Ingress resources or specific file paths need changes to reflect Gateway API resources and new output structure.

## Conclusion

Ingress NGINX retirement forces a migration timeline (March 2026 upstream, November 2026 for AKS Application Routing add-on security patches). The post argues that if you have to migrate, **AGC + Gateway API** is a strong landing spot: Azure-managed L7 data plane outside the cluster, built-in WAF support, and tooling (the AGC Migration Utility) that handles much of the mechanical conversion while producing a report for the cases that still need manual work.

Key links:

- AGC docs/quickstart: https://aka.ms/agc
- AGC Migration Utility: https://github.com/Azure/Application-Gateway-for-Containers-Migration-Utility
- In-place CNI migration doc: https://learn.microsoft.com/en-us/azure/aks/concepts-network-legacy-cni
- Ingress NGINX retirement announcement: https://www.kubernetes.dev/blog/2025/11/12/ingress-nginx-retirement/


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/after-ingress-nginx-migrating-to-application-gateway-for/ba-p/4503110)

