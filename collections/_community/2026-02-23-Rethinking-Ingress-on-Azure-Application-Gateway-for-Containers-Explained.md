---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/rethinking-ingress-on-azure-application-gateway-for-containers/ba-p/4492277
title: 'Rethinking Ingress on Azure: Application Gateway for Containers Explained'
author: rgarofalo
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-23 10:49:29 +00:00
tags:
- AKS
- Azure
- Azure Application Gateway For Containers
- Azure Managed Service
- Azure Networking
- Azure Security
- Backend Routing
- Community
- Container Security
- Control Plane
- Data Plane
- Declarative Configuration
- Gateway API
- Ingress
- Kubernetes
- Kubernetes Integration
- Layer 7 Load Balancing
- Network Architecture
- Platform Engineering
- Security
- TLS Termination
- Traffic Management
- Web Application Firewall
section_names:
- azure
- security
---
rgarofalo breaks down how Azure Application Gateway for Containers enables secure, scalable ingress and Layer-7 traffic management for containerized workloads, highlighting managed security, automation, and Kubernetes-native operations.<!--excerpt_end-->

# Rethinking Ingress on Azure: Application Gateway for Containers Explained

Azure Application Gateway for Containers is a managed Azure networking service that addresses common pain points in exposing containerized workloads. Instead of running ingress controllers inside Kubernetes clusters—which requires manual scaling, patching, and security management—this solution moves Layer-7 ingress, web application firewall (WAF), and TLS termination outside the cluster to an Azure-managed data plane.

## 1. Introduction

- Designed for modern, container-based, cloud-native applications on Azure Kubernetes Service (AKS) or other Kubernetes platforms.
- Handles HTTP/HTTPS traffic routing, secure TLS termination, and provides built-in WAF capabilities.

## 2. Key Benefits and Problems Solved

- **Reduced operational overhead:** Ingress management, scaling, and patching handled by Azure.
- **Improved security boundaries:** Managed WAF and TLS offloading happen *before* network traffic enters your cluster, reducing attack surface.
- **Centralized control:** Platform teams manage ingress and security policies with Azure tools, while application teams use Kubernetes-native APIs (Gateway API, Ingress) for route/config definitions.
- **Consistent experience:** Declarative, repeatable configurations across multiple environments and clusters increases governance and reliability.

## 3. Azure Application Gateway for Containers vs. Classic Application Gateway

- Classic Application Gateway is general-purpose for VMs/services; configuration is Azure-centric.
- Containers version is designed for Kubernetes, integrates natively with Gateway API/Ingress, and decouples platform (infrastructure) from application (routes/services) responsibilities.
- Enables near real-time configuration updates as application manifests change.

## 4. Architecture: Control Plane and Data Plane

- **Control plane:** Listens to Kubernetes Gateway API or Ingress objects; translates updates to Azure-managed gateway rules.
- **Data plane:** Hosted and scaled outside your cluster by Azure; handles routing, WAF, and SSL termination—does not consume cluster resources.

### Managed and Customer Responsibilities

| Managed by Azure                         | Managed by Customer                     |
|------------------------------------------|-----------------------------------------|
| Data plane scaling, patching, and WAF    | Gateway/Ingress API resources           |
| Network plumbing to the cluster          | Backend app services and workloads      |
| TLS policy enforcement                   | TLS certificates in K8s, security intent|
| Rule translation, Azure security updates | Network design, connectivity decisions  |

## 5. Kubernetes-Native Integration

- **Preferred:** Kubernetes Gateway API (GatewayClass, Gateway, HTTPRoute)—portable, community-standard, supports multi-tenancy.
- **Supported:** Kubernetes Ingress—backwards compatible, fewer features, good for incremental migration.
- Platform teams define boundaries and listeners; application teams define route resources without needing direct Azure access or RBAC assignments.

## 6. Security Features

- Built-in [Azure WAF](https://azure.microsoft.com/en-us/products/web-application-firewall) protects against OWASP Top 10 attacks.
- Uses Microsoft global threat intelligence for automated rule updates and threat blocking.
- Centralized WAF policies for regulatory or governance needs.
- TLS termination and certificate management are handled at the Azure edge, not by application pods, reducing complexity and risk.
- Supports both public and future private endpoint scenarios for zero-trust and hybrid security.

## 7. Operational Scale and Performance

- Automatic scaling responds to real traffic patterns—no pod scaling or node capacity configuration needed.
- Predictable network performance: Application ingress is independent of cluster CPU and RAM.
- Supports high concurrency, low latency, and near real-time config propagation.
- Platform-enforced service limits (routes, listeners, certificates, throughput) are documented for planning and scale-up.

## 8. When to Use or Not Use

**Recommended For:**

- Kubernetes workloads on Azure (AKS) or clusters needing managed, production-grade ingress.
- Scenarios requiring centralized Layer-7 routing, WAF, and TLS—especially for regulated or multi-tenant environments.

**Not Ideal For:**

- VM-only or non-container backends (use Classic Application Gateway).
- Extremely simple test/dev environments (where in-cluster ingress may suffice at lower cost).
- Non-Kubernetes platforms.

## 9. Migration and Portability

- Gateway API standardization allows workload configurations to remain portable while taking advantage of Azure's managed implementation.
- Ingress manifests can be reused, easing transitions from older controller-based models.

## 10. Example: Declarative Configuration

### HTTP Listener Example

```yaml
apiVersion: gateway.networking.k8s.io/v1
kind: HTTPRoute
metadata:
  name: app-route
spec:
  parentRefs:
    - name: agc-gateway
  rules:
    - backendRefs:
        - name: app-service
          port: 80
```

### Path Routing Example

```yaml
apiVersion: gateway.networking.k8s.io/v1
kind: HTTPRoute
metadata:
  name: path-routing
spec:
  parentRefs:
    - name: agc-gateway
  rules:
    - matches:
        - path:
            type: PathPrefix
            value: /api
      backendRefs:
        - name: api-service
          port: 80
    - backendRefs:
        - name: web-service
          port: 80
```

### Weighted Canary Rollout Example

```yaml
apiVersion: gateway.networking.k8s.io/v1
kind: HTTPRoute
metadata:
  name: canary-route
spec:
  parentRefs:
    - name: agc-gateway
  rules:
    - backendRefs:
        - name: app-v1
          port: 80
          weight: 80
        - name: app-v2
          port: 80
          weight: 20
```

### TLS Termination Example

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: app-ingress
spec:
  ingressClassName: azure-alb-external
  tls:
    - hosts:
        - app.contoso.com
      secretName: tls-cert
  rules:
    - host: app.contoso.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: app-service
                port:
                  number: 80
```

## 11. Conclusion

Azure Application Gateway for Containers provides a robust, Kubernetes-native ingress solution for Azure that centralizes traffic management, scales automatically, and incorporates leading security technology. By leveraging native APIs and Azure’s managed platform, it significantly simplifies the challenges of high-availability, secure ingress—enabling platform and development teams to operate efficiently at scale.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/rethinking-ingress-on-azure-application-gateway-for-containers/ba-p/4492277)
