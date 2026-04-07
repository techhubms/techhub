---
feed_name: Microsoft Tech Community
title: 'AKS with AGIC hits Azure Application Gateway backend pool limit (100): reproduction and mitigations'
author: kkaushal
primary_section: azure
section_names:
- azure
- devops
tags:
- AGIC
- AKS
- Application Gateway Ingress Controller
- Application Routing Add On
- Azure
- Azure Application Gateway
- Azure Gateway Controller
- Backend Pool Limit
- BackendAddressPools
- Community
- DevOps
- Enterprise Networking
- Hub And Spoke Network
- Ingress Nginx
- IngressClass
- Kubectl
- Kubernetes Gateway API
- Kubernetes Ingress
- PowerShell
- Private Frontend
- Scaling Limits
- Troubleshooting
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/aks-cluster-with-agic-hits-the-azure-application-gateway-backend/ba-p/4508201
date: 2026-04-03 00:42:48 +00:00
---

Kumar Shashi Kaushal documents a scaling failure mode when exposing many apps from an AKS cluster via AGIC: Azure Application Gateway has a hard limit of 100 backend pools, so the 101st service/ingress pattern can break reconciliation and prevent traffic from flowing for new apps.<!--excerpt_end-->

# AKS with AGIC hits Azure Application Gateway backend pool limit (100)

This post documents a real-world scaling issue when exposing many applications from an Azure Kubernetes Service (AKS) cluster using Application Gateway Ingress Controller (AGIC). Kubernetes resources can keep applying successfully, but Azure Application Gateway enforces a hard platform limit of **100 backend pools**. When your pattern requires the **101st backend pool**, Application Gateway rejects the update, **AGIC cannot reconcile**, and traffic will not flow for newly onboarded apps.

## What triggers the limit

- AGIC typically creates **one Application Gateway backend pool per Kubernetes Service** referenced by an Ingress.
- Azure Application Gateway enforces a **hard limit of 100 backend pools**.
- When the 101st pool is needed:
  - Application Gateway rejects the update.
  - AGIC reconciliation fails.
  - Kubernetes objects still appear created/applied, but external routing doesn’t work for the new apps.

## Architecture overview

The setup uses a typical **Hub-and-Spoke** Azure network architecture.

## Hub network

- Azure Firewall / network security services
- VPN / ExpressRoute Gateways
- Private DNS Zones
- Shared monitoring and governance components

## Spoke network

- Private Azure Kubernetes Service (AKS) cluster
- Azure Application Gateway with **private frontend**
- Application Gateway Ingress Controller (AGIC)
- Application workloads exposed via Kubernetes Services and Ingress

## Ingress traffic flow

Client → Private Application Gateway → AGIC-managed routing → Kubernetes Service → Pod

## Application deployment model that exhausts backend pools

Each application used the same pattern:

- One Deployment per application
- One Service per application
- One Ingress per application
- Each Ingress references a unique Service

## Kubernetes manifests used

### Deployment template

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: app-{{N}}
  namespace: demo
spec:
  replicas: 1
  selector:
    matchLabels:
      app: app-{{N}}
  template:
    metadata:
      labels:
        app: app-{{N}}
    spec:
      containers:
        - name: app
          image: hashicorp/http-echo:1.0
          args:
            - "-text=Hello from app {{N}}"
          ports:
            - containerPort: 5678
```

### Service template

```yaml
apiVersion: v1
kind: Service
metadata:
  name: svc-{{N}}
  namespace: demo
spec:
  selector:
    app: app-{{N}}
  ports:
    - port: 80
      targetPort: 5678
```

### Ingress template

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: ing-{{N}}
  namespace: demo
spec:
  ingressClassName: azure-application-gateway
  rules:
    - host: app{{N}}.example.internal
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: svc-{{N}}
                port:
                  number: 80
```

## Reproducing the backend pool limitation

The issue is reproduced by deploying **101 applications** following the same pattern, causing AGIC to attempt creating a new backend pool each time.

```powershell
for ($i = 1; $i -le 101; $i++) {
  (Get-Content deployment.yaml) -replace "{{N}}", $i | kubectl apply -f -
  (Get-Content service.yaml) -replace "{{N}}", $i | kubectl apply -f -
  (Get-Content ingress.yaml) -replace "{{N}}", $i | kubectl apply -f -
}
```

## Observed AGIC error

```text
Code="ApplicationGatewayBackendAddressPoolLimitReached" Message="The number of BackendAddressPools exceeds the maximum allowed value. The number of BackendAddressPools is 101 and the maximum allowed is 100.
```

## Root cause analysis

- Azure Application Gateway enforces a **non-configurable maximum** of **100 backend pools**.
- AGIC’s mapping (backend pools created based on the Services referenced by Ingress resources) leads to pool exhaustion at scale.

## Options after hitting the limit

## Option 1: Azure Gateway Controller (AGC)

- Uses the Kubernetes **Gateway API** and avoids the legacy Ingress model.
- Limitation noted in the post: currently supports **only public frontends** and does **not support private frontends**.

## Option 2: ingress-nginx via Application Routing

- Supported only until **November 2026**.
- Not recommended due to deprecation and lack of long-term viability.

## Option 3: Application Routing with Gateway API (Preview)

- Described as the strategic long-term direction for AKS application routing.
- Still in preview, but upstream Gateway API has been stable for years.
- Suitable for onboarding new applications with appropriate risk awareness.

Reference Microsoft docs:

- [Azure Kubernetes Service (AKS) Managed Gateway API Installation (preview) - Azure Kubernetes Service | Microsoft Learn](https://learn.microsoft.com/en-us/azure/aks/managed-gateway-api)
- [Azure Kubernetes Service (AKS) application routing add-on with the Kubernetes Gateway API (preview) - Azure Kubernetes Service | Microsoft Learn](https://learn.microsoft.com/en-us/azure/aks/app-routing-gateway-api)
- [Secure ingress traffic with the application routing Gateway API implementation](https://learn.microsoft.com/en-us/azure/aks/app-routing-gateway-api-tls)

## Conclusion

The **100 backend pool** cap is a hard Azure Application Gateway constraint. If you’re using **AGIC** to onboard many apps, plan for scale early (for example, by consolidating service patterns or moving to **Gateway API–based routing**) to avoid production onboarding blockers.

## Author

Kumar Shashi Kaushal (Sr. Digital Cloud solutions Architect)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-architecture-blog/aks-cluster-with-agic-hits-the-azure-application-gateway-backend/ba-p/4508201)

