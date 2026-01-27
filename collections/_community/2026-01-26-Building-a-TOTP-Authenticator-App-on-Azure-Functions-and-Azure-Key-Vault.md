---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-a-totp-authenticator-app-on-azure-functions-and-azure/ba-p/4489332
title: Building a TOTP Authenticator App on Azure Functions and Azure Key Vault
author: StephenMS
feed_name: Microsoft Tech Community
date: 2026-01-26 16:19:38 +00:00
tags:
- API Development
- Authentication
- Azure CLI
- Azure Functions
- Azure Key Vault
- CI/CD
- Cloud Security
- HMAC SHA1
- JavaScript
- Managed Identities
- Node.js
- React
- RFC 6238
- Secrets Management
- Serverless
- Static Web Apps
- TOTP
- Two Factor Authentication
- VS Code
section_names:
- azure
- coding
- security
---
StephenMS shows how to build a secure TOTP authenticator app using Azure Functions and Azure Key Vault, covering secure secret storage, API endpoints, and cloud-native best practices in authentication.<!--excerpt_end-->

# Building a TOTP Authenticator App on Azure Functions and Azure Key Vault

Author: **StephenMS**  
Published: Jan 26, 2026  
[GitHub Source Code](https://github.com/stephendotgg/azure-totp-authenticator)

## Overview

Two-factor authentication (2FA) is now fundamental to digital security. While popular tools like Microsoft Authenticator exist, some teams need custom solutions for integration, security, or unique requirements. In this guide, we walk through building your own TOTP (Time-based One-Time Password) authenticator using Azure cloud services.

## What You'll Learn

- The mechanics of TOTP (RFC 6238) and its security principles
- Building a secure backend API with Azure Functions
- Storing sensitive secrets using Azure Key Vault
- CLI deployment and configuration steps
- Testing with industry tools and front-end integration with React (bonus)

---

## Understanding TOTP

TOTP algorithms generate short-lived codes using a shared secret and the current time, typically valid for 30 seconds. This is the foundation for industry-standard 2FA solutions and uses protocols compatible with services like GitHub or Microsoft accounts.

- **URI Format Example**:  
  `otpauth://totp/Test%20Token?secret=2FASTEST&issuer=2FAS`
- **Security**: The algorithm uses HMAC-SHA1, ensuring that codes can't reveal the secret. Short validity windows and slight tolerance for clock drift maintain security and usability.

---

## Architecture

**Backend API:**  Uses Azure Functions for:

- Adding new 2FA accounts (store TOTP secrets)
- Generating valid TOTP codes on demand

**Secret Storage:**  Azure Key Vault provides:

- Hardware-level encryption
- Access auditing and managed identities integration

**Frontend (Bonus):**  Azure Static Web Apps hosts a React UI for uploading QR codes and displaying tokens.

---

## Prerequisites

- **Azure Subscription**: Active account with resource creation permissions ([Get started with Azure](https://azure.microsoft.com/en-us/pricing/purchase-options/azure-account))
- **Visual Studio Code**: IDE ([Download](https://code.visualstudio.com/))
- **Azure VS Code Extensions**: For streamlined deployment

---

## Deploying Azure Resources

Run these Azure CLI commands to provision resources:

```bash
az keyvault create \
  --name <your-kv-name> \
  --resource-group <your-rg> \
  --location <region>

az keyvault update \
  --name <your-kv-name> \
  --enable-rbac-authorization true

az functionapp create \
  --name <app-name> \
  --storage-account <storage-name> \
  --consumption-plan-location <region> \
  --runtime node \
  --runtime-version 18 \
  --functions-version 4

az functionapp config appsettings set \
  --name <app-name> \
  --resource-group <your-rg> \
  --settings "KEY_VAULT_NAME=<your-kv-name>"

az role assignment create \
  --assignee-object-id <function-app-managed-identity> \
  --role "Key Vault Secrets Officer" \
  --scope /subscriptions/<subscription-id>/resourceGroups/<rg-name>/providers/Microsoft.KeyVault/vaults/<your-kv-name>
```

---

## Building the API with Azure Functions

### 1. Store New TOTP Secrets Endpoint

- Validates and parses `otpauth://` URIs
- Stores secrets in Azure Key Vault with meaningful metadata

**Sample function structure:**

```js
const { app } = require("@azure/functions");
const { DefaultAzureCredential } = require('@azure/identity');
const { SecretClient } = require('@azure/keyvault-secrets');
const { randomUUID } = require('crypto');

const vaultName = process.env.KEY_VAULT_NAME;
const vaultUrl = `https://${vaultName}.vault.azure.net`;
const secretClient = new SecretClient(vaultUrl, new DefaultAzureCredential());

app.http("accounts", {
  methods: ["POST"],
  authLevel: "anonymous",
  handler: async (request, context) => {
    let requestBody;
    try {
      requestBody = await request.json();
    } catch (error) {
      return { status: 400, jsonBody: { error: 'Invalid request format' } };
    }
    const { uri } = requestBody;
    // ...parse and store logic
  }
});
```

### 2. Generate TOTP Codes Endpoint

- Accepts secret ID (Key Vault reference)
- Retrieves secret and returns a valid 6-digit token and time remaining

**Sample function structure:**

```js
app.http('tokens', {
  methods: ['GET'],
  authLevel: 'anonymous',
  handler: async (request, context) => {
    const secretId = request.query.get('id');
    // ...lookup and generate logic
  }
});
```

---

## Security and Best Practices

- All TOTP secrets are stored securely server-side
- Access managed via Azure RBAC and managed identities—no credentials in code
- Only minimal metadata returned from API, never the secret itself
- React frontend is decoupled, communicates over secure endpoints

---

## Testing

- Use [2fas.com/check-token/](https://2fas.com/check-token/) for compatibility checks
- Store a test secret using the accounts endpoint via cURL
- Generate TOTP codes and validate against the website's expected values

---

## Frontend (Bonus): React Component

- Allows users to paste or drop QR code images
- Parses and sends TOTP URIs to backend
- Displays issuer, account, code, and 30-second countdown
- Handles errors and provides feedback

*Deployment Recommendation*: Use Azure Static Web Apps for frictionless CI/CD and built-in authentication

---

## Summary and Next Steps

This project demonstrates how to leverage Azure-managed services for strong two-factor authentication workflows and understand TOTP mechanisms end-to-end. You can extend this foundation with multi-user authentication, backup codes, or audit logging for real-world deployments.

Explore the [full source code and examples here](https://github.com/stephendotgg/azure-totp-authenticator).

---

*Thanks for reading!*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-a-totp-authenticator-app-on-azure-functions-and-azure/ba-p/4489332)
