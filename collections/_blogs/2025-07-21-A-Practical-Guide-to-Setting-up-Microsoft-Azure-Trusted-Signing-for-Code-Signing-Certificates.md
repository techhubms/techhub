---
layout: "post"
title: "A Practical Guide to Setting up Microsoft Azure Trusted Signing for Code Signing Certificates"
description: "Rick Strahl details his challenging experience setting up Microsoft Azure Trusted Signing for code signing, outlining the complexities of certificate rules, service pricing, infrastructure requirements, tooling, and performance considerations. This post includes step-by-step setup instructions, troubleshooting tips, alternative approaches, and community feedback."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2025/Jul/20/Fighting-through-Setting-up-Microsoft-Trusted-Signing"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2025-07-21 00:56:23 +00:00
permalink: "/2025-07-21-A-Practical-Guide-to-Setting-up-Microsoft-Azure-Trusted-Signing-for-Code-Signing-Certificates.html"
categories: ["Azure", "Security"]
tags: ["Automation", "Azure", "Azure CLI", "Azure Trusted Signing", "Blogs", "Certificate Authority", "Certificate Management", "Code Signing", "Cryptography", "Developer Experience", "Key Vault", "Microsoft Azure", "NuGet", "Security", "Security Module", "Signtool", "Windows"]
tags_normalized: ["automation", "azure", "azure cli", "azure trusted signing", "blogs", "certificate authority", "certificate management", "code signing", "cryptography", "developer experience", "key vault", "microsoft azure", "nuget", "security", "security module", "signtool", "windows"]
---

In this extensive guide, Rick Strahl shares his experience with setting up Microsoft Azure Trusted Signing for code signing, discussing certificate requirements, Azure configuration, and practical challenges developers may face.<!--excerpt_end-->

# Fighting through Setting up Microsoft Trusted Signing

**Author:** Rick Strahl

![Signing Hand Banner](https://weblog.west-wind.com/images/2025/Fighting-through-Setting-up-Microsoft-Trusted-Signing/SigningHandBanner.jpg)

## Introduction

Renewing code signing certificates has become increasingly complicated due to evolving industry standards and security requirements. Rick Strahl walks readers through his recent experience with Microsoft's Azure Trusted Signing service, highlighting the challenges, configuration steps, pricing details, and practical lessons learned from moving away from local certificates to Microsoft's cloud-based approach.

---

## Background: Industry Changes in Code Signing Certificates

- Traditional code signing certificates allowed exporting and storing keys locally, but newer rules force certificates to be issued via online authorities or stored in FIPS-compliant hardware security modules (HSMs). Exportable keys are no longer permitted.
- The primary motivation is to prevent hijacking and misuse of private keys. Certificates are now short-lived (valid for just a few days) and automatically rotate.
- These increased requirements make acquisition and usage more complex and expensive: basic certificates now cost $350-$500 annually, while EV (Extended Validation) certificates start around $500/year.
- Developers, especially those distributing software via NuGet or publishing on Windows, are now forced to comply with these new standards.

> As it is the whole CodeSigning thing has turned into another scam of Enshittification of a captured audience. If you're publishing software or even packages on NuGet now, you pretty much have to have a code signing certificate.

## Microsoft Azure Trusted Signing: The Cloud-based Solution

Microsoft, which enforces these certificate requirements for Windows SmartScreen and NuGet, also offers its own service: **Azure Trusted Signing**. The service has the following notable characteristics:

- Pricing (as of the post date):
  - **Basic**: $9.99/month for up to 5000 signatures (approx. $120/year)
  - **Premium**: $99.99/month for up to 100,000 signatures
  - {{CONTENT}}.005/signature beyond quota
- Restricted to US and Canadian business customers (though there are unverified reports of other regions being able to sign up).
- Certificates generated are valid for **only 3 days**, but **signed files remain valid indefinitely** if timestamped.
- Compared to traditional SSL vendors charging $300+ annually plus the cost of mailed hardware keys, Microsoft's option is cost-effective and eliminates the need for physical tokens, but is less strict (not EV).

## Azure Trusted Signing Setup: Step-by-Step

Setting up Azure Trusted Signing involves multiple Azure resources and roles, and can be confusing, especially for those not working with Azure daily. Below is a breakdown of the overall process:

### Certificate Creation Steps

1. **Azure account:** You need a paid Azure account.
2. **Resource group:** Create or use an existing resource group.
3. **Trusted Signing Account:** Create one in the Azure portal.
4. **Trusted Signing Identity:** Enter your business/organization information (DUNS number, Tax ID, etc. required).
5. **Certificate Profile:** Generate a certificate profile that links to your validated identity.

*Common gotchas:*

- The user creating the identity must have the `Trusted Signing Identity Verifier` role assigned, as well as appropriate role assignments within the Azure portal, to finish setup successfully.

### Signing Setup Steps

1. **Azure CLI:** Install (e.g., via `winget install --id Microsoft.AzureCLI -e`).
2. **Trusted Signing SDK:** Install via WinGet (`winget install -e --id Microsoft.Azure.TrustedSigningClientTools`). The SDK installs required DLLs for integration with signing tools.
3. **SignTool.exe:** Part of Windows SDK (10.0.2261.755 or later) – used for the actual signing. Can be found in your Visual Studio or Windows SDK installation.
4. **.NET 8.0 Runtime:** Required for Trusted Signing SDK tools.
5. **Metadata file:** Create a small JSON file (e.g., `SignfileMetaData.json`) with the endpoint, signing account name, and certificate profile name:

    ```json
    {
      "Endpoint": "https://eus.codesigning.azure.net/",
      "CodeSigningAccountName": "MySigningAccount",
      "CertificateProfileName": "MyCertificate"
    }
    ```

### Example Signing Script (PowerShell)

A simplified signing script might look like:

```powershell
# Install Azure CLI and tools as needed

param(
    [string]$file = "",
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
    "sign", "/v", "/debug", "/fd", "SHA256", "/tr", "http://timestamp.acs.microsoft.com", "/td", "SHA256",
    "/dlib", "$env:LOCALAPPDATA\Microsoft\MicrosoftTrustedSigningClientTools\Azure.CodeSigning.Dlib.dll",
    "/dmdf", ".\SignfileMetadata.json"
)
.\signtool.exe $args $file
```

### Notes

- **Performance:** The signing process is much slower (5–8 seconds per file) with Trusted Signing than with local certificates. No apparent parallelization exists.
- **Documentation Challenges:** The Microsoft documentation and associated tooling can be confusing and non-intuitive, especially regarding role assignments, installing/updating the SDK, and integrating via CLI.

## Alternatives: BYO Certificate and Azure Key Vault

- You can purchase your own code signing certificate and store it in **Azure Key Vault HSM**, which is FIPS 140-2 compliant. This approach often carries higher costs for the certificate but offers better performance and is available in more regions. For heavy signing workloads, it may be more cost-effective.

## Community Feedback & Tips

- Tools such as the new `dotnet sign` utility (see [dotnet/sign on GitHub](https://github.com/dotnet/sign)) can offer faster, parallelized signing, and can be integrated in CI/CD pipelines using service principals.
- Some commenters shared sources for cheaper code signing certificates and provided clarifications about certificate expiration versus signed file validity.
- The author and commenters debated the merits of Azure Key Vault vs. Trusted Signing with regard to cost, convenience, and performance.

## Summary: Lessons Learned

- The new code signing landscape is more secure but more complicated and expensive.
- Azure Trusted Signing offers an affordable solution, but the setup and usage require patience, Azure familiarity, and tolerance for subpar documentation and slow performance.
- Once configured, Trusted Signing can be set and forgotten, with automatic renewals and no further need to update certificates for the lifetime of the service.
- Alternatives such as Azure Key Vault give more flexibility, but often at higher costs and similar Azure-related complexity.

## Resources

- [Microsoft Trusted Signing Documentation](https://learn.microsoft.com/en-us/azure/trusted-signing/)
- [Microsoft Trusted Signing Pricing Page](https://azure.microsoft.com/en-us/pricing/details/trusted-signing/)
- [How to Create Key Vault, CSR, and Import Code Signing Certificate in Azure KeyVault HSM](https://signmycode.com/resources/how-to-create-private-keys-csr-and-import-code-signing-certificate-in-azure-keyvault-hsm)

---

*Copyright © Rick Strahl, West Wind Technologies*

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Jul/20/Fighting-through-Setting-up-Microsoft-Trusted-Signing)
