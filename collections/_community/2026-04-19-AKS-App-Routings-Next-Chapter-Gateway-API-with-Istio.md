---
primary_section: azure
section_names:
- azure
- devops
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/aks-app-routing-s-next-chapter-gateway-api-with-istio/ba-p/4512729
author: samcogan
tags:
- AKS
- AKS App Routing
- AKS Preview
- Application Gateway For Containers
- Azure
- Azure CLI
- Azure DNS
- Community
- DevOps
- Envoy
- ExternalDNS
- Feature Flags
- Gateway
- Gateway API
- GatewayClass
- Helm
- Horizontal Pod Autoscaler
- HTTPRoute
- Ingress NGINX Migration
- Istio
- Key Vault Certificates
- Kubectl
- Kubernetes Ingress
- LoadBalancer Service
- Maintenance Windows
- Managed Gateway API CRDs
- PodDisruptionBudget
- SNI Passthrough
- TLS Termination
feed_name: Microsoft Tech Community
date: 2026-04-19 18:25:14 +00:00
title: "AKS App Routing's Next Chapter: Gateway API with Istio"
---

samcogan explains AKS App Routing’s new preview Gateway API mode (Istio-managed Envoy gateways) and provides a practical migration path from NGINX Ingress/App Routing, including prerequisites, rollout approach, and current limitations around DNS/TLS automation.<!--excerpt_end-->

# AKS App Routing's Next Chapter: Gateway API with Istio

If you've been following the earlier posts about the Ingress NGINX retirement: the community Ingress NGINX project was retired in **March 2026**, and Microsoft’s extended support for the **NGINX-based AKS App Routing add-on** runs until **November 2026**.

Previous migration options covered:

- Migrating from standalone NGINX to the App Routing add-on (short-term):
  - https://techcommunity.microsoft.com/blog/appsonazureblog/seamless-migrations-from-self-hosted-nginx-ingress-to-the-aks-app-routing-add-on/4495630
- Migrating to Application Gateway for Containers (long-term):
  - https://techcommunity.microsoft.com/blog/appsonazureblog/after-ingress-nginx-migrating-to-application-gateway-for-containers/4503110

Microsoft’s recommended path for existing users of the NGINX-based App Routing add-on is now available (in preview):

- App Routing Gateway API implementation:
  - https://blog.aks.azure.com/2026/03/18/app-routing-gateway-api

This moves ingress from **NGINX + Ingress API** to **Gateway API**, with a lightweight **Istio control plane** managing gateway infrastructure.

## What is the new App Routing Gateway API mode?

Key point: this is **not** the full Istio service mesh.

When enabled, AKS deploys:

- an Istio control plane (**istiod**) to manage **Envoy-based gateway proxies**
- **no sidecar injection**
- **no Istio CRDs installed for workloads**

When you create a **Gateway** resource, AKS provisions and manages:

- an Envoy **Deployment**
- a **LoadBalancer** Service
- a **HorizontalPodAutoscaler** (defaults: **2–5 replicas**, **80% CPU**)
- a **PodDisruptionBudget**

You write:

- **Gateway** and **HTTPRoute** resources

AKS manages the gateway infrastructure behind them.

## Gateway API model vs Ingress API

Gateway API splits responsibilities into layers:

- **GatewayClass**: defines the gateway infrastructure type (AKS provides this)
- **Gateway**: creates the gateway and listeners
- **HTTPRoute**: routing rules attached to the Gateway

This separation enables platform teams to own Gateway infrastructure while app teams manage their own routes, reducing the “shared Ingress edit breaks everyone” problem.

## Difference vs the AKS Istio service mesh add-on

App Routing Gateway API mode:

- uses `approuting-istio` **GatewayClass**
- does **not** install Istio CRDs
- does **not** enable sidecar injection
- upgrades are **in-place**

Istio service mesh add-on:

- uses `istio` **GatewayClass**
- installs Istio CRDs cluster-wide
- enables sidecar injection
- uses **canary upgrades** for minor versions

Important constraint:

- **They cannot run at the same time.**
  - If you have the Istio service mesh add-on enabled, you must disable it before enabling App Routing Gateway API (and vice versa).

## Current limitations (preview)

Because this is preview, it should not be run in production yet.

Gaps vs the NGINX-based App Routing add-on:

- **DNS and TLS certificate management via the add-on isn’t supported yet** for Gateway API.
  - Existing workflows like `az aks approuting update` and `az aks approuting zone add` (Key Vault + Azure DNS integration) do not carry over.
  - TLS termination is still possible, but requires manual setup.
  - Docs: https://learn.microsoft.com/azure/aks/app-routing-gateway-api-tls
- **SNI passthrough (TLSRoute)** not supported
- **Egress traffic management** not supported
- Mutually exclusive with the **Istio service mesh add-on**

If you need production-ready DNS/TLS automation now, the recommended alternative is **Application Gateway for Containers**.

## Getting started

Prereqs:

- `aks-preview` CLI extension **19.0.0b24 or later**
- Managed Gateway API CRDs enabled
- App Routing Gateway API preview feature flag registered

```bash
az extension add --name aks-preview
az extension update --name aks-preview

# Managed Gateway API CRDs (required dependency)
az feature register --namespace "Microsoft.ContainerService" --name "ManagedGatewayAPIPreview"

# App Routing Gateway API implementation
az feature register --namespace "Microsoft.ContainerService" --name "AppRoutingIstioGatewayAPIPreview"
```

Enable on a new or existing cluster (both flags required):

```bash
# New cluster
az aks create \
  --resource-group ${RESOURCE_GROUP} \
  --name ${CLUSTER} \
  --location swedencentral \
  --enable-gateway-api \
  --enable-app-routing-istio

# Existing cluster
az aks update \
  --resource-group ${RESOURCE_GROUP} \
  --name ${CLUSTER} \
  --enable-gateway-api \
  --enable-app-routing-istio
```

Verify `istiod` is running:

```bash
kubectl get pods -n aks-istio-system
```

Quick validation path:

- AKS quickstart (httpbin sample): https://learn.microsoft.com/azure/aks/app-routing-gateway-api

## Migrating from NGINX Ingress

This applies whether you’re on:

- standalone NGINX Ingress (Helm-installed)
- NGINX-based AKS App Routing add-on

You migrate:

- from **Ingress API** resources to **Gateway API** resources
- while running the new controller **in parallel** with the old one during transition

### 1) Inventory your Ingress resources

```bash
kubectl get ingress --all-namespaces \
  -o custom-columns='NAMESPACE:.metadata.namespace,NAME:.metadata.name,CLASS:.spec.ingressClassName'
```

Watch for NGINX-specific behaviors that may not map cleanly:

- custom snippets
- lua config
- regex rewrites

### 2) Convert Ingress resources to Gateway API

Use `ingress2gateway` (v1.0.0):

- https://github.com/kubernetes-sigs/ingress2gateway

```bash
# Install
go install github.com/kubernetes-sigs/ingress2gateway@v1.0.0

# Convert from live cluster
ingress2gateway print --providers=ingress-nginx -A > gateway-resources.yaml

# Or convert from a local file
ingress2gateway print --providers=ingress-nginx --input-file=./manifests/ingress.yaml > gateway-resources.yaml
```

Notes:

- The tool flags annotations it can’t convert as comments in the output.
- Update `gatewayClassName` in generated Gateway resources to **`approuting-istio`**.

### 3) Handle DNS and TLS

- If you were on standalone NGINX: DNS/TLS likely already manual; ensure cert Secrets and DNS records are ready for the new Gateway IP.

- If you were using App Routing add-on automation (`az aks approuting zone add` + Key Vault): that automation is not available yet in Gateway API mode.

Options mentioned:

- Manual TLS Secrets, or build a sync workflow from **Key Vault**
- Use **ExternalDNS** for DNS automation (supports Gateway API):
  - https://github.com/kubernetes-sigs/external-dns

TLS docs:

- https://learn.microsoft.com/azure/aks/app-routing-gateway-api-tls

### 4) Deploy and validate

Apply the converted resources:

```bash
kubectl apply -f gateway-resources.yaml
```

Wait for gateway programming and fetch the external IP:

```bash
kubectl wait --for=condition=programmed gateways.gateway.networking.k8s.io <gateway-name>

export GATEWAY_IP=$(kubectl get gateways.gateway.networking.k8s.io <gateway-name> -ojsonpath='{.status.addresses[0].value}')
```

Test routing via Host header (since DNS still points at NGINX initially):

```bash
curl -H "Host: myapp.example.com" http://$GATEWAY_IP
```

Validate fully (TLS, path routing, headers, auth, etc.) before any DNS cutover.

### 5) Cut over DNS and clean up

- Lower DNS TTL to ~60s well ahead of time.
- Update DNS to point to the new Gateway IP.
- Keep the old controller running for 24–48 hours as rollback.

Cleanup depends on your starting point.

If you were on standalone NGINX:

```bash
helm uninstall ingress-nginx -n ingress-nginx
kubectl delete namespace ingress-nginx
```

If you were on the NGINX App Routing add-on:

Check for remaining Ingress objects using the old class:

```bash
kubectl get ingress --all-namespaces \
  -o custom-columns='NAMESPACE:.metadata.namespace,NAME:.metadata.name,CLASS:.spec.ingressClassName' \
  | grep "webapprouting"
```

Disable the NGINX-based add-on:

```bash
az aks approuting disable --resource-group ${RESOURCE_GROUP} --name ${CLUSTER}
```

Optionally remove remaining resources by deleting the namespace:

```bash
kubectl delete ns app-routing-system
```

## Upgrades and lifecycle notes

- Istio control plane version is tied to the AKS cluster Kubernetes version.
- Patch upgrades are handled by AKS.
- Minor version upgrades occur in-place when upgrading Kubernetes version or when AKS introduces a new compatible Istio minor.

Difference vs Istio mesh add-on:

- upgrades here are **in-place**, not canary-based

If configured, upgrades respect AKS maintenance windows:

- https://learn.microsoft.com/azure/aks/planned-maintenance

## What to do now

- If you’re still on standalone NGINX Ingress: it’s unsupported since March 2026.
- If you’re on NGINX App Routing add-on: you have support until November 2026.

Recommended actions:

- Start testing App Routing Gateway API mode in non-production.
- Learn the Gateway API resource model.
- Plan around preview gaps (manual TLS/DNS).
- If you need production-ready DNS/TLS automation now: use **Application Gateway for Containers**.

Published Apr 19, 2026

Version 1.0


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/aks-app-routing-s-next-chapter-gateway-api-with-istio/ba-p/4512729)

