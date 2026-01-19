---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/zero-trust-kubernetes-enforcing-security-multi-tenancy-with/ba-p/4466646
title: Zero-Trust Enforcement and Multi-Tenancy Security in Kubernetes with Custom Admission Webhooks on AKS
author: divyaan
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-04 16:22:40 +00:00
tags:
- Admission Controller
- AKS
- Azure AD
- Azure Policy
- Custom Webhooks
- Flask
- Kubernetes
- Kyverno
- Microsoft Sentinel
- Multi Tenancy
- NetworkPolicy
- OPA Gatekeeper
- Policy as Code
- Python
- RBAC
- ResourceQuota
- Runtime Security
- Supply Chain Security
- TLS
- Zero Trust
section_names:
- azure
- devops
- security
---
divyaan demonstrates practical approaches to enforcing Zero Trust and multi-tenancy in Kubernetes using custom admission webhooks on AKS, with code examples, Azure integrations, and actionable security policies.<!--excerpt_end-->

# Zero-Trust Enforcement and Multi-Tenancy Security in Kubernetes with Custom Admission Webhooks on AKS

## Introduction

Securing Kubernetes clusters—especially on Azure Kubernetes Service (AKS)—requires applying Zero Trust principles and robust multi-tenancy controls. This article by divyaan walks through using custom admission controllers, OPA Gatekeeper, and Kyverno integrated with Azure services to automate policy enforcement and reduce risk.

## Admission Controllers and Policy Engines

- **Admission Controllers** in Kubernetes act as gatekeepers, intercepting Kubernetes API requests after authentication and before persistence. They enable validation/mutation of objects against security policies.
- Strengthen admission with:
  - **OPA Gatekeeper**: Policy as code, tightly integrated with Azure Policy on AKS.
  - **Kyverno**: YAML-based policy engine.
  - **Custom Admission Webhooks**: Tailor policy logic, such as Zero Trust enforcement, to organizational needs.

## Key Zero Trust Controls for Kubernetes

- **Least-Privilege Access**: Use RBAC to grant only essential permissions, with individual ServiceAccounts per workload and no cluster-admin roles for apps.
- **Restrict Images to Trusted Registries**: Block unverified public images using controllers or Azure Policy.
- **Deny Privileged Containers and Host Access**: Prevent pods from running in privileged mode or mounting sensitive host paths (`/etc`, `/var/run/docker.sock`).
- **Default-Deny Network Policies**: Set a deny-all default for ingress/egress traffic; allow only as needed.
- **Mutual TLS (mTLS)**: Use Istio or Linkerd service mesh for authenticated, encrypted internal traffic.
- **Continuous Audit and Drift Detection**: Run OPA/Kyverno in audit mode to monitor ongoing policy compliance.
- **Enforce Runtime Security**: Integrate Azure Defender for Kubernetes or Falco to detect anomalies and block privilege escalation.
- **Secure API Server Access**: Lock down API with IP whitelisting, Azure AD integration, and RBAC.

## Example: Custom Admission Controller for Pod Security

A sample Python Flask-based webhook enforces Zero Trust by requiring container images to come from a trusted Azure registry and all pods to include an `environment` label.

```python
def validate():
    request_info = request.get_json()
    uid = request_info["request"]["uid"]
    pod = request_info["request"]["object"]
    violations = []

    # Only allow images from trusted registry
    trusted_registries = ["testtech.azurecr.io"]
    for container in pod.get("spec", {}).get("containers", []):
        image = container.get("image", "")
        if not any(image.startswith(reg) for reg in trusted_registries):
            violations.append(f"Image {image} not from trusted registry.")

    # Require 'environment' label
    labels = pod.get("metadata", {}).get("labels", {})
    if "environment" not in labels:
        violations.append("Pod missing required label: environment")
```

## Steps to Implement

1. **Build and Deploy Webhook**: Package the Flask webhook, mount TLS certs, expose via a ClusterIP service in AKS.
2. **Create TLS Certificates**: Generate and store as a Kubernetes secret, include the service DNS for mutual trust.
3. **Register ValidatingWebhookConfiguration**: Instruct K8s API to invoke your webhook on Pod creation/changes, ensuring secure admission.

## Secure Multi-Tenant AKS Approaches

- **Namespace Isolation**: Each tenant/team in its own namespace; set up RBAC and NetworkPolicies per-namespace.
- **Tenant-Specific RBAC**: Use Azure AD groups integrated with K8s RBAC.
- **Network Fencing**: Default-deny policies and Azure VNet peering/segmentation.
- **Resource Quotas**: Set and enforce CPU/memory/storage limits per namespace.
- **Admission Controls**: Use OPA Gatekeeper for namespace-specific guardrails.
- **Ingress/Egress Security**: TLS for services and egress traffic controls per namespace.

### Dynamic Enforcement in Webhooks

- **NetworkPolicy Enforcement**: Webhook checks if both ingress and egress policies are enforced and do not allow open traffic.
- **ResourceQuota Validation**: Webhook fetches current usage and denies resource requests exceeding quotas.

## Best Practices

- **Secure Supply Chain**: Use Trivy or Clair to scan container images in CI/CD; only allow signed/trusted images in AKS.
- **Runtime Threat Detection**: Tools like Falco can trigger alerts on suspicious activity.
- **Observability**: Integrate Prometheus/Grafana for metrics, central log aggregation (Elasticsearch), and Microsoft Sentinel for threat detection.
- **Incident Response**: Define and test IR playbooks, keep regular backups, and practice secret rotation.

## Conclusion

By combining OPA Gatekeeper, Kyverno, Azure integrations, and custom admission controllers, you shift Kubernetes security from passive monitoring to proactive enforcement. Zero Trust and multi-tenant controls ensure workloads in AKS remain compliant, isolated, and resilient against threats.

For code samples and deployment manifests, see [Kubernetes Custom Admission Controller](https://github.com/divyaan23/Kubernetes-custom-admission-controller).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/zero-trust-kubernetes-enforcing-security-multi-tenancy-with/ba-p/4466646)
