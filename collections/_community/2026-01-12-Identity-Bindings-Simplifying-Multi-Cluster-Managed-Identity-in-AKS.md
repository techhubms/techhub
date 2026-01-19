---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/identity-bindings-a-cleaner-model-for-multi-cluster-identity-in/ba-p/4478282
title: 'Identity Bindings: Simplifying Multi-Cluster Managed Identity in AKS'
author: samcogan
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2026-01-12 10:42:40 +00:00
tags:
- AKS
- Application Security
- Azure Entra ID
- Azure Key Vault
- Azure Managed Identity
- Cloud Security
- ClusterRole
- ClusterRoleBinding
- Federated Identity Credential
- Identity Bindings
- Kubernetes Automation
- Kubernetes RBAC
- Managed Identity
- OIDC
- Pod Identity
- Workload Identity
section_names:
- azure
- devops
- security
---
samcogan explains the new Identity Bindings feature in AKS, showing how it resolves the scaling and automation issues found in older Managed Identity models by using RBAC-driven authorization within Kubernetes clusters.<!--excerpt_end-->

# Identity Bindings: Simplifying Multi-Cluster Managed Identity in AKS

*Author: samcogan*

Azure Kubernetes Service (AKS) has evolved its support for assigning Azure Managed Identities to pods, beginning with Pod Identity, then Workload Identity, and now Identity Bindings. This article covers why Identity Bindings are needed, their technical design, and practical steps to use them in production and test scenarios.

## Background: Why Change Identity Management?

Traditional approaches like Pod Identity and Workload Identity allowed pods in AKS to access Azure resources using Managed Identities (e.g., pulling secrets from Key Vault, reading from Blob Storage, or connecting to a database). However, these methods required multiple Federated Identity Credentials (FICs), had scaling issues (each managed identity supports up to 20 FICs), and created operational challenges:

- Difficulty automating identity/resource creation simultaneously
- Potential for cyclic dependencies between resource definitions
- Required outbound access for token requests

At scale, this resulted in a proliferation of managed identities with duplicated permissions, operational complexity, and networking headaches.

## What Are Identity Bindings?

**Identity Bindings** are a new (currently preview) feature in AKS, enabling a simpler, standardized, RBAC-driven way to connect workloads to Azure Managed Identities.

**Benefits:**

- **Centralized Access Control** using Kubernetes RBAC
- **Cross-Cluster Identity Sharing** (no 20-FIC limit)
- **No Outbound Token Requests** (handled internally by AKS)
- **Simplified Infrastructure as Code** (removes cyclic deployment ordering)

Under this model, Kubernetes administrators define which workloads (service accounts/namespaces) are permitted to use specific Managed Identities using RBAC and Identity Bindings—rather than creating many FICs or duplicating identities per cluster/namespace.

## How Identity Bindings Work

1. **Register the Feature**

   ```sh
   az feature register --namespace Microsoft.ContainerService --name IdentityBindingPreview
   ```

2. **Create the Identity Binding**
   Map one managed identity to an AKS cluster using:

   ```sh
   az aks identity-binding create -g "<resource group name>" --cluster-name "<cluster name>" -n "<binding name>" --managed-identity-resource-id "<managed identity Azure Resource ID>"
   ```

3. **Create a ClusterRole Referencing the Managed Identity**

   ```yaml
   apiVersion: rbac.authorization.k8s.io/v1
   kind: ClusterRole
   metadata:
     name: identity-binding-user
   rules:
   - verbs: ["use-managed-identity"]
     apiGroups: ["cid.wi.aks.azure.com"]
     resources: ["<MI_CLIENT_ID>"]
   ```

4. **Bind Service Accounts via ClusterRoleBinding**

   ```yaml
   apiVersion: rbac.authorization.k8s.io/v1
   kind: ClusterRoleBinding
   metadata:
     name: identity-binding-user-demo
   roleRef:
     apiGroup: rbac.authorization.k8s.io
     kind: ClusterRole
     name: identity-binding-user
   subjects:
   - kind: ServiceAccount
     name: <service account name>
     namespace: <namespace>
   ```

5. **Configure Pod to Use the Identity Binding**
   - Add labels and annotations:

     ```yaml
     metadata:
       labels:
         azure.workload.identity/use: "true"
       annotations:
         azure.workload.identity/use-identity-binding: "true"
     spec:
       serviceAccountName: <service account name>
     ```

## Example: Deploying a Workload That Uses an Identity Binding

Here’s a deployment example that uses Identity Bindings to access Azure Key Vault secrets:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: keyvault-demo
  namespace: identity-binding-demo
spec:
  replicas: 1
  selector:
    matchLabels:
      app: keyvault-demo
  template:
    metadata:
      labels:
        app: keyvault-demo
        azure.workload.identity/use: "true"
      annotations:
        azure.workload.identity/use-identity-binding: "true"
    spec:
      serviceAccountName: keyvault-demo-sa
      containers:
      - name: keyvault-demo
        image: <your-image>
        env:
        - name: KEYVAULT_URL
          value: ${KEYVAULT_URL}
        - name: SECRET_NAME
          value: ${KEYVAULT_SECRET_NAME}
```

To validate it works, deploy a supporting Key Vault resource, grant the managed identity the required role (e.g., "Key Vault Secret User"), then check pod logs:

```sh
kubectl logs demo -n demo

# expected: "successfully got secret"
```

## Conclusion

Identity Bindings represent a significant improvement for AKS identity management at scale. By leveraging Kubernetes RBAC and a single FIC per managed identity, they prevent identity sprawl, simplify automation, and strengthen access controls—all without requiring changes to pod startup order or extra networking rules. Existing users of Workload Identity will find migration to Identity Bindings both natural and beneficial.

## Further Reading

- [Identity Bindings Overview](https://learn.microsoft.com/en-us/azure/aks/identity-bindings-concepts)
- [Setup Identity Bindings](https://learn.microsoft.com/en-us/azure/aks/identity-bindings)
- [Workload Identity in Azure](https://learn.microsoft.com/en-us/entra/workload-id/workload-identities-overview)
- [Identity Binding Example in Go](https://github.com/Azure/azure-workload-identity/blob/feature/custom-token-endpoint/examples/identitybinding-msal-go/main.go)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/identity-bindings-a-cleaner-model-for-multi-cluster-identity-in/ba-p/4478282)
