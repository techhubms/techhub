---
external_url: https://www.hanselman.com/blog/automatically-signing-a-windows-exe-with-azure-trusted-signing-dotnet-sign-and-github-actions
title: Automatically Signing Windows Executables with Azure Trusted Signing, dotnet sign, and GitHub Actions
author: Scott Hanselman
viewing_mode: external
feed_name: Scott Hanselman's Blog
date: 2025-11-28 19:31:25 +00:00
tags:
- .NET
- .NET Sign
- Azure CLI
- Azure Trusted Signing
- Certificate Profile
- CI/CD Pipeline
- Code Signing Certificate
- Digital Signature
- DotNetCore
- GitHub Actions
- Identity Validation
- Role Based Access Control
- Service Principal
- SmartScreen Reputation
- Windows Executable Signing
- YAML Workflow
section_names:
- azure
- coding
- devops
- security
---
Scott Hanselman guides developers through the full process of signing Windows executables using Azure Trusted Signing, dotnet sign, and GitHub Actions. The tutorial blends practical, real-world experience with detailed step-by-step instructions and security insights.<!--excerpt_end-->

# Automatically Signing Windows Executables with Azure Trusted Signing, dotnet sign, and GitHub Actions

## Introduction

Windows executables downloaded from the internet often trigger warnings from Windows Defender SmartScreen, especially if unsigned. Scott Hanselman walks through a practical solution to automate code signing using Microsoft's cloud service Azure Trusted Signing, along with .NET tooling and integrated CI/CD workflows in GitHub Actions. This guide is based on real-world experience, blending personal mistakes and learnings, and covers both local and cloud-based approaches.

## Why Use Azure Trusted Signing?

- No hardware tokens; certificate management is handled in the cloud
- Automatic certificate issuance and renewal
- Trusted by Windows Defender SmartScreen, improving executable reputation
- Cost-efficient for small projects (approx. $10/month)
- Direct integration with GitHub Actions for automated signing

## Prerequisites

- Azure Subscription
- Azure CLI ([Install here](https://aka.ms/installazurecliwindows))
- Documents for identity validation (driver's license or passport)
- Windows PC (for local testing)
- GitHub repository (for CI/CD automation)

## Part 1: Setting Up Azure Trusted Signing

### Step 1: Register the Resource Provider

```sh
az login
az provider register --namespace Microsoft.CodeSigning
az provider show --namespace Microsoft.CodeSigning --query "registrationState"
```

Wait until registration state shows "Registered".

### Step 2: Create a Trusted Signing Account

Choose Azure Portal (GUI) or Azure CLI (commands):

- Create a resource group with `az group create --name MyAppSigning --location westus2`
- Create a Trusted Signing account with `az trustedsigning create ...`
- Note the region endpoint for later use

### Step 3: Complete Identity Validation

- In Azure Portal, go to "Identity validation"
- Submit documentation for individual or organization
- Await approval (from hours to a few days)

### Step 4: Create Certificate Profile

- Use "Public Trust" for SmartScreen compatibility
- Name and link the profile to your validated identity
- Confirm creation in Azure Portal

### Step 5: Note Key Information

- Account name
- Certificate profile name
- Endpoint URL
- Subscription ID
- Resource group

## Part 2: Signing Locally with dotnet sign

### Step 1: Assign Permissions

- Assign "Trusted Signing Certificate Profile Signer" role to your identity in Azure Portal or via CLI

### Step 2: Use Device Code Flow for Azure CLI Login

```sh
az logout
az login --use-device-code --scope "https://codesigning.azure.net/.default"
```

Device code flow is more reliable for this context.

### Step 3: Install dotnet sign Tool

```sh
dotnet tool install --global --prerelease sign
sign --version
```

Or locally, using `--tool-path .`

### Step 4: Sign Your Executables

```sh
sign.exe code trusted-signing -b "C:\MyProject\publish" -tse "https://wus2.codesigning.azure.net" -tscp "MyAppProfile" -tsa "myapp-signing" *.exe -v Information
```

- Parameters specify directory, endpoint, profile, account, and verbosity

### Step 5: Verify Signature

- PowerShell: `Get-AuthenticodeSignature ".\publish\MyApp.exe" | Format-List`
- Windows: Right-click EXE > Properties > Digital Signatures

### Common Issues and Solutions

- "Please run 'az login'..." → Login with correct scope
- "403 Forbidden" → Check endpoint, account name, role assignment
- "User account does not exist in tenant" → Use device code auth

## Part 3: Automated Signing in GitHub Actions

### Step 1: Create a Service Principal

```sh
az ad sp create-for-rbac --name "MyAppGitHubActions" --role "Trusted Signing Certificate Profile Signer" --scopes /subscriptions/YOUR_SUBSCRIPTION_ID/resourceGroups/MyAppSigning/providers/Microsoft.CodeSigning/codeSigningAccounts/myapp-signing --json-auth
```

- Save `clientId`, `clientSecret`, `tenantId`, `subscriptionId` for GitHub secrets

### Step 2: Add GitHub Secrets

Add the above values to your repository secrets for use in your workflow.

### Step 3: Update GitHub Workflow YAML

- Use official Azure and trusted-signing actions
- Specify endpoint, account, profile, paths to binaries, and timestamping parameters
- Example YAML [here](https://github.com/shanselman/WindowsEdgeLight/blob/master/.github/workflows/build.yml)

### Step 4: Trigger and Monitor Workflows

- Optionally run manually (`workflow_dispatch`) or by tagging and pushing
- Use `gh run view --log` or the GitHub Actions UI to monitor progress and debug

### Common Issues

- Permissions errors → Verify service principal and role assignments
- Path issues → Debug with PowerShell `Get-ChildItem`
- Secret naming/values errors → Double check case and values

## Part 4: Certificate Lifecycle and Timestamping

Azure Trusted Signing issues certificates valid for a short period (e.g. 3 days) to enhance security. Timestamping ensures signatures remain valid after certificate expiration, so signed executables retain trust status.

## Resources Referenced

- [Azure Trusted Signing Documentation](https://learn.microsoft.com/en-us/azure/trusted-signing/)
- [dotnet/sign Tool](https://github.com/dotnet/sign)
- [azure/trusted-signing-action](https://github.com/Azure/trusted-signing-action)
- [Windows Code Signing Best Practices](https://learn.microsoft.com/en-us/windows/win32/seccrypto/cryptography-tools)
- [SmartScreen Reputation Overview](https://learn.microsoft.com/en-us/windows/security/threat-protection/microsoft-defender-smartscreen/microsoft-defender-smartscreen-overview)

## Conclusion

With Azure Trusted Signing, it's possible to automate code signing for Windows executables in both local and CI/CD environments, improving security and user experience while minimizing operational hassle. Carefully checking details like region endpoints, case-sensitive identifiers, and permissions is critical. This workflow is suitable for developers aiming for secure and reputable Windows app distribution.

---

*Written in November 2025 by Scott Hanselman, based on real-world experience implementing WindowsEdgeLight app code signing across local and automated build processes.*

This post appeared first on "Scott Hanselman's Blog". [Read the entire article here](https://www.hanselman.com/blog/automatically-signing-a-windows-exe-with-azure-trusted-signing-dotnet-sign-and-github-actions)
