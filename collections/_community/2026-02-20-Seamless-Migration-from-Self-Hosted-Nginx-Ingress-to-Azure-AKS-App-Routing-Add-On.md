---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/seamless-migrations-from-self-hosted-nginx-ingress-to-the-aks/ba-p/4495630
title: Seamless Migration from Self-Hosted Nginx Ingress to Azure AKS App Routing Add-On
author: samcogan
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-20 09:37:43 +00:00
tags:
- AKS
- App Gateway For Containers
- App Routing Add On
- Azure
- Azure Key Vault
- Cert Manager
- Community
- Container Security
- DevOps
- DNS Management
- Gateway API
- Helm
- Ingress Migration
- Istio
- Kubernetes
- Kubernetes Networking
- Nginx Ingress
- Security
- Zero Downtime Deployment
section_names:
- azure
- devops
- security
---
samcogan provides a practical migration guide for moving from a standalone Nginx Ingress controller to the Azure AKS App Routing add-on, outlining zero-downtime techniques and actionable advice for long-term Kubernetes ingress strategy.<!--excerpt_end-->

# Seamless Migration from Self-Hosted Nginx Ingress to Azure AKS App Routing Add-On

The Kubernetes Steering Committee will retire the Nginx Ingress controller in March 2026, making urgent the migration off unsupported installations to avoid security risks. Azure Kubernetes Service (AKS) offers a managed App Routing add-on that implements Nginx and will be supported until November 2026, giving teams a critical grace period. Microsoft is transitioning to Istio-based ingress and Gateway API, so planning ahead is essential.

## Why Migrate?

- **Security**: The standalone Nginx Ingress controller will no longer receive updates, exposing clusters to vulnerabilities.
- **Support**: Azure AKS's managed App Routing add-on bundles ongoing support, plus migration tooling is available, such as the [Ingress2Gateway tool](https://github.com/kubernetes-sigs/ingress2gateway).
- **Transition**: Migrating now allows for a smoother, less risky move to the next set of Microsoft-supported routing solutions (Istio, Gateway API).

## Zero-Downtime Migration Strategy

You can run both the BYO Nginx and App Routing add-on controllers in parallel using different IngressClasses and namespaces. Each controller receives traffic only for resources referencing their specific IngressClass, and each exposes a unique load balancer IP. The process:

1. **Enable the App Routing Add-On** on your cluster:

   ```bash
   az aks approuting enable \
     --resource-group <resource-group> \
     --name <cluster-name>
   ```

   Check deployment via:

   ```bash
   kubectl get pods -n app-routing-system
   kubectl get svc -n app-routing-system
   ```

2. **Validate Both Controllers** have unique IPs:

   ```bash
   BYO_IP=$(kubectl get svc ingress-nginx-controller -n ingress-nginx \
     -o jsonpath='{.status.loadBalancer.ingress[0].ip}')
   ADDON_IP=$(kubectl get svc nginx -n app-routing-system \
     -o jsonpath='{.status.loadBalancer.ingress[0].ip}')
   echo "BYO Nginx IP: $BYO_IP"
   echo "Add-on IP: $ADDON_IP"
   ```

3. **Duplicate Ingress Resources**:
   - For each Ingress, create a new resource with `ingressClassName: webapprouting.kubernetes.azure.com`.
   - Example:

     ```yaml
     apiVersion: networking.k8s.io/v1
     kind: Ingress
     metadata:
       name: myapp-ingress-add-on
       namespace: myapp
       annotations:
         nginx.ingress.kubernetes.io/rewrite-target: /
     spec:
       ingressClassName: webapprouting.kubernetes.azure.com
       rules:
         - host: myapp.example.com
           http:
             paths:
               - path: /
                 pathType: Prefix
                 backend:
                   service:
                     name: myapp
                     port:
                       number: 80
     ```

4. **Validate Functionality**
   - Test using curl:

     ```bash
     curl -H "Host: myapp.example.com" http://$ADDON_IP
     ```

   - Check for correct TLS, routing, and other dependencies.
5. **Cutover Traffic**:
   - For public ingress: Lower DNS TTL, update A record to point at add-on IP.
   - For private ingress: Update backend pool on load balancer (App Gateway/API Management/Front Door) to new IP.
6. **Optional: In-Place Migration**:
   - For lower-risk/internal workloads, patch the `ingressClassName` directly:

     ```bash
     kubectl patch ingress myapp-ingress-byo -n myapp \
       --type='json' \
       -p='[{"op":"replace","path":"/spec/ingressClassName","value":"webapprouting.kubernetes.azure.com"}]'
     ```

   - Some downtime is possible as controllers switch ownership.
7. **Decommission the Old Controller**:
   - Ensure no Ingress is using the old class:

     ```bash
     kubectl get ingress --all-namespaces \
       -o custom-columns='NAMESPACE:.metadata.namespace,NAME:.metadata.name,CLASS:.spec.ingressClassName' | grep -v "webapprouting"
     ```

   - Uninstall:

     ```bash
     helm uninstall ingress-nginx -n ingress-nginx
     kubectl delete namespace ingress-nginx
     ```

## Key Differences and Post-Migration Considerations

- **TLS Certificates**: Both cert-manager and Azure Key Vault integrations are supported.
- **DNS Management**: Add-on can natively manage Azure DNS zones, or work with external-dns.
- **Nginx Configuration**: App Routing add-on limits global customization due to Azure management—audit your customizations for compatibility.
- **Annotations**: Standard nginx.ingress.kubernetes.io/* annotations generally still work; Azure-specific annotations offer additional functionality.

## Planning for the Future

- Microsoft expects to release an Istio-based App Routing add-on with Gateway API support in late 2026.
- Gateway API is more flexible and vendor-neutral than the Ingress API.
- Consider evaluating App Gateway for Containers now if you need a production-ready option.

The current migration to the App Routing add-on is a temporary solution, so start planning for further migration—either to Istio/Gateway API-based add-ons or other supported ingress platforms—well before November 2026.

---

_Disclaimer: This guide is provided by samcogan and reflects the state of Azure workloads and Kubernetes ingress at the time of writing (February 2026). Always consult the latest documentation and official Microsoft AKS updates for evolving best practices._

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/seamless-migrations-from-self-hosted-nginx-ingress-to-the-aks/ba-p/4495630)
