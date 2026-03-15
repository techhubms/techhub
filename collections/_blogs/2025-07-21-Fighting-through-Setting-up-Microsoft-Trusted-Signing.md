---
external_url: https://weblog.west-wind.com/posts/2025/Jul/20/Fighting-through-Setting-up-Microsoft-Trusted-Signing
tags:
- .NET
- .NET 8 Runtime
- Az Login
- Azure
- Azure CLI
- Azure Key Vault
- Azure RBAC
- Azure Resource Group
- Azure Trusted Signing
- Azure TrustedSigning CLI Extension
- Blogs
- Certificate Profile
- CI Signing
- Code Signing
- DevOps
- FIPS 140 2
- Hardware Security Module (hsm)
- Http://timestamp.acs.microsoft.com
- Key Vault HSM
- Microsoft Trusted Signing
- Microsoft.Trusted.Signing.Client
- NuGet
- NuGet Package Signing
- Parallel Signing
- RFC 3161 Timestamping
- Security
- Service Principal
- SHA 256
- Signtool.exe
- Trusted Signing Account
- Trusted Signing Certificate Profile Signer Role
- Trusted Signing Identity
- Windows
- Windows SDK
- Windows SmartScreen
- WinGet
feed_name: Rick Strahl's Blog
section_names:
- azure
- devops
- dotnet
- security
author: Rick Strahl
date: 2025-07-21 00:56:23 +00:00
title: Fighting through Setting up Microsoft Trusted Signing
primary_section: dotnet
---
Rick Strahl documents what it actually takes to get Azure Trusted Signing working for Windows code-signing, including the Azure resources you must create, the client tooling (Azure CLI, Trusted Signing tools, Signtool), and a practical PowerShell-based signing flow—plus the gotchas that cost him a day.<!--excerpt_end-->

## Overview

Microsoft’s code-signing requirements have tightened over the last few years: locally stored, exportable certificates are no longer acceptable for scenarios like Windows SmartScreen reputation and (often) NuGet signing. Certificates now need to be either:

- Served by a certified online signing authority, or
- Stored in a **FIPS 140-2 Level 2+** compliant **HSM** (hardware security module), with **non-exportable** keys.

Rick Strahl walks through using **Azure Trusted Signing** (still in preview as of the post) as Microsoft’s hosted code-signing option.

## Why Azure Trusted Signing

Key motivations and constraints discussed:

- Traditional code-signing certs have gotten much more expensive, especially once hardware tokens/shipping enter the picture.
- Azure Trusted Signing is positioned as a cheaper (and “least bad”) option.
- The service issues **short-lived certificates (3 days)**, but that doesn’t mean your signed binaries expire:
  - You must sign during the certificate’s validity window.
  - Once a file is signed and timestamped correctly, the signature remains valid indefinitely.

## Pricing (at time of writing)

Rick lists this pricing model:

| Model type | Basic | Premium |
| --- | --- | --- |
| Base price (monthly) | $9.99 | $99.99 |
| Quota (signatures/month) | 5,000 | 100,000 |
| Price after quota is reached | $0.005/signature | $0.005/signature |

Notes:

- These are **non-EV** certificates (basic vetting).
- Azure Trusted Signing was shown as **US and Canada for business only**, though commenters mention they were approved elsewhere.

Official docs:

- [Microsoft Trusted Signing Documentation](https://learn.microsoft.com/en-us/azure/trusted-signing/)

## What you need to set up (high-level)

Microsoft’s resource model (per the linked docs) boils down into two phases.

### Certificate creation (Azure portal)

You’ll need:

- An Azure account (with payment configured)
- An Azure **Resource Group**
- A **Trusted Signing Account**
- A **Trusted Signing Identity** (company information)
- A **Trusted Signing Profile** (the actual certificate profile)

A common early blocker Rick hit: missing permissions/roles for identity validation.

He calls out needing to:

- Assign the **Trusted Signing Identity Verifier** role
- Add a user to roles for the signing account

The UX for role assignment is described as confusing/non-obvious.

### Signing (client tooling)

Because keys are non-exportable, signing happens via an online service call. Rick lists these dependencies:

- **Azure CLI** (for login)
- **Azure CodeSigning SDK / client tools**
- **SignTool** from the **Windows SDK**

## Installing prerequisites

### Install Azure CLI

```powershell
winget install --id Microsoft.AzureCLI -e
```

### Install Trusted Signing client tools

Via WinGet:

```powershell
winget install -e --id Microsoft.Azure.TrustedSigningClientTools
```

#### DLL path requirement

Rick notes you must reference a specific DLL from the client tools installation when running `signtool.exe`.

Default location mentioned:

```text
%localappdata%\Microsoft\MicrosoftTrustedSigningClientTools\Azure.CodeSigning.Dlib.dll
```

#### SDK vs NuGet package version mismatch

He also calls out that the installed SDK may be outdated relative to the NuGet package, and suggests:

- Install the client tools SDK
- Download the NuGet package
- Update binaries under `%localappdata%` with the newer NuGet binaries

NuGet package referenced:

- [Microsoft.Trusted.Signing.Client NuGet Package](https://www.nuget.org/packages/Microsoft.Trusted.Signing.Client)

### Install or locate SignTool.exe

Requirements:

- A recent Windows SDK version (Rick mentions **10.0.2261.755 or later**) because you need `/dlib` and `/dmdf` support.

Example path he shows:

```text
C:\Program Files (x86)\Windows Kits\10\bin\10.0.26100.0\x64\signtool.exe
```

Microsoft instructions:

- [Signtool and Windows SDK install instructions](https://learn.microsoft.com/en-us/azure/trusted-signing/how-to-signing-integrations?source=recommendations#download-and-install-signtool)

## Information you need from your Trusted Signing setup

Rick highlights three values you’ll need to sign:

- **Signing Account URI** (example format: `https://eus.codesigning.azure.net/`)
- **Signing Account Name**
- **Profile Name**

## Create a metadata JSON file

You must create a JSON file that points SignTool at your Trusted Signing configuration.

Template:

```json
{
  "Endpoint": "<Trusted Signing account endpoint>",
  "CodeSigningAccountName": "<Trusted Signing account name>",
  "CertificateProfileName": "<Certificate profile name>"
}
```

Example (`SignfileMetaData.json`):

```json
{
  "Endpoint": "https://eus.codesigning.azure.net/",
  "CodeSigningAccountName": "MySigningAccount",
  "CertificateProfileName": "MyCertificate"
}
```

## Signing flow with Signtool.exe

Rick describes signing as a two-step process:

1. Log into Azure
2. Run `signtool.exe` with the Trusted Signing integration options

He provides a PowerShell script that:

- Optionally logs in using `az login`
- Calls `signtool.exe sign` with:
  - `/fd SHA256`
  - RFC3161 timestamp URL `http://timestamp.acs.microsoft.com`
  - `/dlib` to point to `Azure.CodeSigning.Dlib.dll`
  - `/dmdf` to point to the metadata JSON

Script (as provided):

```powershell
# dotnet tool install --global AzureSignTool
#winget install --exact --id Microsoft.AzureCLI
param(
  [string]$file = "",
  [string]$file1 = "",
  [string]$file2 = "",
  [string]$file3 = "",
  [string]$file4 = "",
  [string]$file5 = "",
  [boolean]$login = $false
)

if (-not $file) {
  Write-Host "Usage: SignFile.ps1 -file <path to file to sign>"
  exit 1
}

if ($login) {
  az config set core.enable_broker_on_windows=false
  az login
  az account set --subscription "Pay-As-You-Go"
}

$args = @(
  "sign",
  "/v",
  "/debug",
  "/fd",
  "SHA256",
  "/tr",
  "http://timestamp.acs.microsoft.com",
  "/td",
  "SHA256",
  "/dlib",
  "$env:LOCALAPPDATA\Microsoft\MicrosoftTrustedSigningClientTools\Azure.CodeSigning.Dlib.dll",
  "/dmdf",
  ".\SignfileMetadata.json"
)

foreach ($f in @($file, $file1, $file2, $file3, $file4, $file5)) {
  if (![string]::IsNullOrWhiteSpace($f)) { $args += $f }
}

.\signtool.exe $args
```

Expected success signal:

- `Number of files successfully Signed: 1`

## Pain points Rick highlights

### Azure CLI extension doesn’t sign

There is an `az trustedsigning` CLI add-in that can list/check certificates, but (per Rick) it **does not support signing**.

### Performance

Observed performance:

- Around **5 seconds per file** to sign
- No apparent parallel signing even when multiple files are provided

### Non-interactive authentication

Rick wasn’t able to get a fully hands-off login working in his scenario:

- `az login` prompts for interactive login and subscription selection
- He mentions secret key options but couldn’t get them working correctly for his account/subscription linkage

## Alternative approach: bring your own cert + Azure Key Vault

Rick describes a different route:

- Buy a code-signing certificate yourself
- Import/issue it via **Azure Key Vault (HSM)**

Potential advantages mentioned:

- Wider regional availability
- Potentially better performance (commenters report Key Vault signing is faster)

He links a third-party walkthrough:

- [How to Create Key Vault, CSR, and Import Code Signing Certificate in Azure KeyVault HSM](https://signmycode.com/resources/how-to-create-private-keys-csr-and-import-code-signing-certificate-in-azure-keyvault-hsm)

Trade-offs:

- Still Azure complexity
- You’re paying for the cert plus Key Vault/HSM usage

## Notes from comments (practical CI improvements)

Several commenters recommend using the newer **dotnet/sign** tool with Trusted Signing, including:

- Parallel signing
- Avoiding metadata files
- Using a service principal in CI

Repository:

- <https://github.com/dotnet/sign>

Example YAML from a commenter:

```yml
# Sign with defaults: SHA256, RFC 3161 timestamp server http://timestamp.acs.microsoft.com/
dotnet sign code trusted-signing -tse $env:tse -tsa $env:tsa -tscp $env:tscp -b "$env:CI_PROJECT_DIR\bin\" *.dll
```

And role guidance:

- Use a service principal with the role **Trusted Signing Certificate Profile Signer**
- Provide CI variables like `AZURE_CLIENT_ID`, `AZURE_CLIENT_SECRET`, `AZURE_TENANT_ID`

## Resources

- [Microsoft Trusted Signing Documentation](https://learn.microsoft.com/en-us/azure/trusted-signing/)
- [Microsoft Trusted Signing Pricing Page](https://azure.microsoft.com/en-us/pricing/details/trusted-signing/)

[Read the entire article](https://weblog.west-wind.com/posts/2025/Jul/20/Fighting-through-Setting-up-Microsoft-Trusted-Signing)

