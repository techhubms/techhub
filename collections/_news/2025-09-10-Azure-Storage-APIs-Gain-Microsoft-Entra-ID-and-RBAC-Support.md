---
layout: post
title: Azure Storage APIs Gain Microsoft Entra ID and RBAC Support
author: Christine Chen
canonical_url: https://devblogs.microsoft.com/azure-sdk/azure-storage-apis-gain-entra-id-and-rbac-support/
viewing_mode: external
feed_name: Microsoft Azure SDK Blog
feed_url: https://devblogs.microsoft.com/azure-sdk/feed/
date: 2025-09-10 18:24:47 +00:00
permalink: /coding/news/Azure-Storage-APIs-Gain-Microsoft-Entra-ID-and-RBAC-Support
tags:
- .NET
- Access Control
- API Integration
- Authentication
- Authorization
- Azure
- Azure SDK
- Azure Storage
- Blob Storage
- Coding
- DefaultAzureCredential
- Error Handling
- Microsoft Entra ID
- News
- OAuth 2.0
- RBAC
- REST API
- Security
- Security Best Practices
- Storage
section_names:
- azure
- coding
- security
---
Christine Chen discusses the newly available Microsoft Entra ID and RBAC support for Azure Storage APIs, offering guidance on OAuth-based authentication and updated security practices.<!--excerpt_end-->

# Azure Storage APIs Gain Microsoft Entra ID and RBAC Support

To align with security best practices, Microsoft Entra ID and role-based access control (RBAC) are now generally available for several Azure Storage data plane APIs, including Get Account Information, Get and Set ACL for containers, queues, and tables. These enhancements enable OAuth 2.0-based authentication, allowing more secure and manageable access to storage resources.

## Supported APIs

- Get Account Information
- Get Container ACL / Set Container ACL
- Get Queue ACL / Set Queue ACL
- Get Table ACL / Set Table ACL

For the complete list and further documentation, see [Authorize with Microsoft Entra ID (REST API) – Azure Storage](https://learn.microsoft.com/rest/api/storageservices/authorize-with-azure-active-directory#permissions-for-blob-service-operations).

## What Changed for Developers?

- **OAuth 2.0 Authentication**: You can now authenticate these APIs using Microsoft Entra ID, enabling token-based access and fine-grained permissions via Azure RBAC.
- **Error Response Updates**: Unauthorized requests that previously returned HTTP 404 will now return HTTP 403 when OAuth is used without sufficient permissions. Anonymous requests for a bearer challenge will return HTTP 401, in line with other APIs.
- **Code Change Recommendations**: If your application expected HTTP 404 responses, you must update it to also handle HTTP 403 errors.

## Why Use OAuth by Default?

OAuth authentication increases security and scalability. Compared to traditional SAS tokens and account keys, OAuth:

- Enables precise permission scoping and automatic token expiry
- Lowers the risk of exposing long-lived credentials
- Integrates with modern identity platforms
- Provides enhanced auditing and access governance

For broader context, read [What Is OAuth? | Microsoft Security](https://www.microsoft.com/security/business/security-101/what-is-oauth?).

## .NET Example: Using DefaultAzureCredential

Below is a .NET example using Azure Identity's `DefaultAzureCredential` for seamless authentication:

```csharp
using Azure.Identity;
using Azure.Storage.Blobs;

var credential = new DefaultAzureCredential();
var blobServiceClient = new BlobServiceClient(new Uri("https://<youraccount>.blob.core.windows.net"), credential);

// Example: Get Account Information
var accountInfo = blobServiceClient.GetAccountInfo();
Console.WriteLine($"Account Kind: {accountInfo.AccountKind}, SKU: {accountInfo.SkuName}");
```

## Recommendations

- Move to OAuth authentication for improved security.
- Update application error handling logic as needed.
- Leverage the Azure Identity library to streamline authentication across local and cloud environments.

## Resources

- [Authorize with Microsoft Entra ID (REST API) – Azure Storage](https://learn.microsoft.com/rest/api/storageservices/authorize-with-azure-active-directory#permissions-for-blob-service-operations)
- [What Is OAuth? | Microsoft Security](https://www.microsoft.com/security/business/security-101/what-is-oauth?)

## Help and Support

For questions, visit [Microsoft Q&A](https://learn.microsoft.com/answers/tags/125/azure-blob-storage), or submit a technical support request in the Azure portal for authentication and authorization issues.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-storage-apis-gain-entra-id-and-rbac-support/)
