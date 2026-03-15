---
external_url: https://weblog.west-wind.com/posts/2026/Mar/02/Azure-Trusted-Signing-Revisited-with-Dotnet-Sign
title: Azure Trusted Signing Revisited with Dotnet Sign
author: Rick Strahl
primary_section: dotnet
feed_name: Rick Strahl's Blog
date: 2026-03-02 20:10:23 +00:00
tags:
- .NET SDK
- .NET Sign
- Artifact Signing
- Authenticode
- Azure
- Azure CLI
- Azure Trusted Signing
- Binary Signing
- Blogs
- Certificate Management
- CI/CD
- Code Signing
- PowerShell Script
- Secure Build
- Security
- SignTool
- Timestamp Server
- Windows
- Windows Development
- WPF
- .NET
section_names:
- azure
- dotnet
- security
---
Rick Strahl explains how to simplify and automate code signing for Windows binaries using Azure Trusted Signing and the dotnet sign tool, sharing technical setup and scripting tips for secure development workflows.<!--excerpt_end-->

# Azure Trusted Signing Revisited with Dotnet Sign

**Author: Rick Strahl**

This guide explores how Windows developers can securely sign binaries using Azure Trusted Signing and the new dotnet sign tool. It details improvements over previous workflows, highlights key prerequisites, and provides step-by-step instructions as well as script automation tips for developers who need reliable and efficient code signing in build pipelines.

## Background

Rick recounts his earlier struggles with Azure Trusted Signing and notes the addition of new tooling, especially `dotnet sign`, has simplified the process. Despite better tooling, documentation can still be hard to find, and evolving terminology adds confusion for developers and LLMs alike.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest&pivots=winget)
- [Dotnet Sign](https://github.com/dotnet/sign)

You’ll need the .NET SDK for the dotnet tool infrastructure and Azure CLI for authentication.

## Setting up Azure Trusted Signing

You still start by setting up an Azure Trusted Signing account (see the [detailed setup guide](https://weblog.west-wind.com/posts/2025/Jul/20/Fighting-through-Setting-up-Microsoft-Trusted-Signing)). Signing binaries is now much easier with new command-line support.

## Installing the Tools

- **Install .NET SDK**: Download from Microsoft if not already installed.
- **Install Azure CLI**: Rick suggests using WinGet for easy installation.
- **Install Dotnet Sign Tool**: Run:

  ```powershell
  dotnet tool install -g --prerelease sign
  ```

  The `--prerelease` flag is currently required.

## Azure CLI Authentication

Logging in to Azure CLI:

```powershell
az config set core.enable_broker_on_windows=false
az login
az account set --subscription "Pay-As-You-Go"
```

Setting `core.enable_broker_on_windows=false` may help avoid issues with browser-based authentication.

## Signing Binaries with dotnet sign

The main command used is:

```powershell
sign code artifact-signing \
  --verbosity warning \
  --timestamp-url http://timestamp.digicert.com \
  --artifact-signing-endpoint https://eus.codesigning.azure.net/ \
  --artifact-signing-account MySigningAccount \
  --artifact-signing-certificate-profile MySigningCertificateProfile \
  .\Distribution\YourBinary.exe
```

> The `trusted-signing` option has been deprecated in favor of `artifact-signing`.

Multiple files can be signed in batch mode.

## Creating a Signing Automation Script

Because the dotnet sign workflow does not natively support metadata config files the way SignTool did, Rick demonstrates a PowerShell script that reads Azure configuration from an external JSON file and signs multiple files as part of the build process:

**Signfile.ps1** (excerpt):

```powershell
param( [string]$file = "", ... )
if (-not $file) { Write-Host "Usage: SignFile.ps1 -file <path>"; exit 1 }

if ($login) {
  az config set core.enable_broker_on_windows=false
  az login
  az account set --subscription "Pay-As-You-Go"
}

$metadata = Get-Content -Path "SignfileMetadata.json" -Raw | ConvertFrom-Json
$tsEndpoint = $metadata.Endpoint
$tsAccount = $metadata.CodeSigningAccountName
$tsCertProfile = $metadata.CertificateProfileName
$timeServer = "http://timestamp.digicert.com"

$signArgs = @( "--verbosity", "warning", ... )
foreach ($f in @($file, ...)) { if (![string]::IsNullOrWhiteSpace($f)) { $signArgs += $f } }

sign code artifact-signing $signArgs
```

**SignfileMetadata.json** example:

```json
{
  "Endpoint": "https://eus.codesigning.azure.net/",
  "CodeSigningAccountName": "MySigningAccount",
  "CertificateProfileName": "MyCodeSignCertificateProfile"
}
```

Rick recommends keeping the PowerShell script in version control but not the metadata file, which should remain local for security.

## Integrating into Build Pipelines

Sample usage in a build script for signing build artifacts:

```powershell
.\signfile-dotnetsign.ps1 -file ".\Distribution\MarkdownMonster.exe" ... -login $false

if ($LASTEXITCODE -ne 0) { Write-Host "Signing failed, exiting build script."; exit $LASTEXITCODE }
```

Repeat for packaged setup installers, as needed. Metadata files can be generated anywhere in the output path.

## Observations and Improvements

- **Speed**: The new workflow with dotnet sign is much faster—usually under 1 second per file.
- **Reliability**: Switching to the Digicert timestamp server, as now suggested by Microsoft, eliminates previous timestamp issues.
- **Security**: Explicit separation of config data from signing scripts improves credential hygiene in repositories.

## Summary

The dotnet sign approach to Azure Trusted Signing brings substantial improvements, especially for those deeply involved in .NET and Windows software distribution. Some hurdles remain (like the lack of metadata support for configuration and extension to NuGet signing), but Rick’s guidance removes much of the friction for secure, reliable artifact signing.

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2026/Mar/02/Azure-Trusted-Signing-Revisited-with-Dotnet-Sign)
