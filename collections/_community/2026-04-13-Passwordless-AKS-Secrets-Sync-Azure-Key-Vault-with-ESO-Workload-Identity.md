---
tags:
- AKS
- Api://azureadtokenexchange
- Az CLI
- Azure
- Azure Key Vault
- Azure RBAC
- ClusterSecretStore
- Community
- DevOps
- ESO
- External Secrets Operator
- ExternalSecret
- Federated Identity Credential
- Key Vault Secrets Officer
- Key Vault Secrets User
- Kubectl
- Kubernetes Secrets
- Kubernetes ServiceAccount
- Least Privilege
- Managed Identity
- OIDC Issuer
- Opaque Secret
- Rancher
- Secret Rotation
- SecretStore
- Security
- User Assigned Managed Identity
- Workload Identity
section_names:
- azure
- devops
- security
title: 'Passwordless AKS Secrets: Sync Azure Key Vault with ESO + Workload Identity'
author: fenildoshi2510
feed_name: Microsoft Tech Community
primary_section: azure
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/passwordless-aks-secrets-sync-azure-key-vault-with-eso-workload/ba-p/4509959
date: 2026-04-13 05:00:00 +00:00
---

fenildoshi2510 explains how to sync Azure Key Vault secrets into an AKS namespace managed by Rancher using External Secrets Operator (ESO) and Workload Identity, so apps can consume Kubernetes Secrets without storing any client secrets.<!--excerpt_end-->

## Overview

Managing Kubernetes secrets manually—especially across multiple namespaces and environments—quickly becomes error-prone. A better approach is to store secrets centrally in Azure Key Vault, then sync them into Kubernetes automatically.

This guide shows how to fetch Azure Key Vault secrets into an AKS namespace managed by Rancher using External Secrets Operator (ESO) with Workload Identity—so you don’t store client secrets anywhere and applications can keep using the same Rancher Secret name (often with no code changes when names match).

## Architecture

### High-level flow

The solution uses a **User-Assigned Managed Identity (UAMI)** federated to a **Kubernetes Service Account** via AKS OIDC. ESO then uses that identity to read secrets from Key Vault and write them into Kubernetes as **Opaque** secrets.

**Flow:** Azure Key Vault → ESO → Kubernetes Secret (Opaque) → Rancher / App

## Prerequisites

You need:

- **AKS cluster** with **OIDC issuer enabled**
- **External Secrets Operator (ESO)** deployed and operational
- **Azure Key Vault** with required secrets present
- **UAMI + Federated Identity Credential** trusting an AKS namespace Service Account
- Appropriate Key Vault roles (for example **Key Vault Secrets Officer/User**, depending on what you need to do)
- **Rancher access** to the target namespace

> Note: The AKS Workload Identity setup requires enabling OIDC/workload identity, creating a managed identity, creating a Service Account annotated with the client-id, and creating a federated identity credential.

## Step-by-step: End-to-end implementation

### Step 1 — Enable AKS OIDC issuer + Workload Identity

If you’re creating or updating a cluster, enable both flags:

```bash
# Create a new cluster (example)
az aks create \
  --resource-group "${RESOURCE_GROUP}" \
  --name "${CLUSTER_NAME}" \
  --enable-oidc-issuer \
  --enable-workload-identity \
  --generate-ssh-keys
```

If you already have a cluster:

```bash
az aks update \
  --resource-group "${RESOURCE_GROUP}" \
  --name "${CLUSTER_NAME}" \
  --enable-oidc-issuer \
  --enable-workload-identity
```

Retrieve the cluster OIDC issuer URL:

```bash
export AKS_OIDC_ISSUER="$(az aks show \
  --name "${CLUSTER_NAME}" \
  --resource-group "${RESOURCE_GROUP}" \
  --query "oidcIssuerProfile.issuerUrl" \
  --output tsv)"
```

### Step 2 — Create a User-Assigned Managed Identity (UAMI)

```bash
az identity create \
  --name "${USER_ASSIGNED_IDENTITY_NAME}" \
  --resource-group "${RESOURCE_GROUP}" \
  --location "${LOCATION}" \
  --subscription "${SUBSCRIPTION}"
```

Capture the identity **clientId** (used in the Service Account annotation):

```bash
export USER_ASSIGNED_CLIENT_ID="$(az identity show \
  --resource-group "${RESOURCE_GROUP}" \
  --name "${USER_ASSIGNED_IDENTITY_NAME}" \
  --query 'clientId' \
  --output tsv)"
```

### Step 3 — Create/annotate a Kubernetes Service Account (namespace-scoped)

If you don’t already have a Service Account, create one and annotate it:

```yaml
apiVersion: v1
kind: ServiceAccount
metadata:
  name: <NAME>
  namespace: <NAMESPACE>
  annotations:
    azure.workload.identity/client-id: "<UAMI_CLIENT_ID>"
```

Apply:

```bash
kubectl apply -f serviceaccount.yaml
```

### Step 4 — Create the Federated Identity Credential (UAMI ↔ ServiceAccount)

This binds:

- `issuer` = `AKS_OIDC_ISSUER`
- `subject` = `system:serviceaccount:<namespace>:<serviceaccount>`
- `audience` = `api://AzureADTokenExchange`

```bash
az identity federated-credential create \
  --name "${FEDERATED_IDENTITY_CREDENTIAL_NAME}" \
  --identity-name "${USER_ASSIGNED_IDENTITY_NAME}" \
  --resource-group "${RESOURCE_GROUP}" \
  --issuer "${AKS_OIDC_ISSUER}" \
  --subject system:serviceaccount:"<NAMESPACE>":"<SERVICEACCOUNT>" \
  --audience api://AzureADTokenExchange
```

### Step 5 — Grant Key Vault permissions to the UAMI

Example using Azure RBAC role assignment:

```bash
export KEYVAULT_RESOURCE_ID=$(az keyvault show \
  --resource-group "${RESOURCE_GROUP}" \
  --name "${KEYVAULT_NAME}" \
  --query id \
  --output tsv)

export IDENTITY_PRINCIPAL_ID=$(az identity show \
  --name "${USER_ASSIGNED_IDENTITY_NAME}" \
  --resource-group "${RESOURCE_GROUP}" \
  --query principalId \
  --output tsv)

az role assignment create \
  --assignee-object-id "${IDENTITY_PRINCIPAL_ID}" \
  --role "Key Vault Secrets User" \
  --scope "${KEYVAULT_RESOURCE_ID}" \
  --assignee-principal-type ServicePrincipal
```

### Step 6 — Create the SecretStore (ESO → Azure Key Vault) in Rancher

Example `SecretStore`:

```yaml
apiVersion: external-secrets.io/v1beta1
kind: SecretStore
metadata:
  name: <NAME>
  namespace: <NAMESPACE>
spec:
  provider:
    azurekv:
      tenantId: "<tenantID>"
      vaultUrl: "https://<keyvaultname>.vault.azure.net/"
      authType: WorkloadIdentity
      serviceAccountRef:
        name: <SERVICEACCOUNT>
```

Apply (if you’re using kubectl instead of the Rancher UI):

```bash
kubectl apply -f secretstore.yaml
```

### Step 7 — Create the ExternalSecret (pattern-based or “sync all”)

Option A: sync secrets matching a name pattern (password/secret/key):

```yaml
apiVersion: external-secrets.io/v1beta1
kind: ExternalSecret
metadata:
  name: <NAME>
  namespace: <NAMESPACE>
spec:
  refreshInterval: 30s
  secretStoreRef:
    kind: SecretStore
    name: <SECRETSTORENAME>
  target:
    name: <ANYNAME>
    creationPolicy: Owner
  dataFrom:
    - find:
        name:
          regexp: ".*(password|secret|key).*"
```

Option B: sync **all** secrets from the Key Vault:

```yaml
apiVersion: external-secrets.io/v1beta1
kind: ExternalSecret
metadata:
  name: <NAME>
  namespace: <NAMESPACE>
spec:
  refreshInterval: 30s
  secretStoreRef:
    kind: SecretStore
    name: <SECRETSTORENAME>
  target:
    name: <ANYNAME>
    creationPolicy: Owner
  dataFrom:
    - find:
        name: {}
```

What happens next:

- ESO fetches secrets from Azure Key Vault
- ESO creates an **Opaque** Kubernetes Secret in the namespace
- Rancher exposes it to the app
- Changes propagate on the next refresh interval

Apply:

```bash
kubectl apply -f externalsecret.yaml
```

## Validation checklist

Confirm the CRDs and resources exist:

```bash
kubectl get secretstore -n <NAMESPACE>
kubectl get externalsecret -n <NAMESPACE>
```

Confirm the Kubernetes Secret was created:

```bash
kubectl get secret <SECRETNAME> -n <NAMESPACE>
```

Confirm Workload Identity plumbing:

- AKS cluster has OIDC issuer and workload identity enabled.
- ServiceAccount has annotation `azure.workload.identity/client-id`.
- Federated identity credential exists and matches `issuer` + `subject`.

## Operational notes & best practices

### 1) “No code change” strategy

If Azure Key Vault secret names match Rancher Secret names, apps can often keep using the same secret references with no code changes.

### 2) Prefer Azure RBAC model with least privilege

For ESO read-only sync, **Key Vault Secrets User** is typically sufficient.

### 3) Refresh intervals

Tune `refreshInterval` based on your rotation policy and Key Vault throttling considerations.

## Troubleshooting quick hits

### Symptom: Access denied from Key Vault

- Ensure the UAMI has Key Vault roles assigned at the vault scope (for example **Key Vault Secrets User**)
- Ensure Key Vault URL and tenant ID are correct in `SecretStore`

### Symptom: Token exchange issues

- Ensure cluster OIDC issuer and workload identity are enabled.
- Ensure federated credential subject matches `system:serviceaccount:<ns>:<sa>`.

## Key benefits

- No client secrets stored (uses Workload Identity).
- Automatic discovery via regex filters.
- No code changes when secret naming aligns.
- Future-proof: new Key Vault secrets matching patterns can auto-sync.


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/passwordless-aks-secrets-sync-azure-key-vault-with-eso-workload/ba-p/4509959)

