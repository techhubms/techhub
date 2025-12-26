---
layout: "post"
title: "Mandatory Explicit Azure Authentication in Semantic Kernel Python 1.36.0"
description: "This news update by Dmytro Struk covers a critical change in the Semantic Kernel Python SDK: removal of the DefaultAzureCredential fallback starting with version 1.36.0. Developers must now explicitly specify authentication credentials for Azure-related services, aligning with security best practices and Azure guidelines. The update details reasons for the change, outlines common errors, and provides code examples for correct implementation."
author: "Dmytro Struk"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/semantic-kernel/azure-authentication-changes-in-semantic-kernel-python/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2025-08-27 05:16:27 +00:00
permalink: "/news/2025-08-27-Mandatory-Explicit-Azure-Authentication-in-Semantic-Kernel-Python-1360.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "API Key", "Authentication", "Azure", "Azure Identity", "Azure SDK", "AzureChatCompletion", "AzureCliCredential", "Coding", "Credential Management", "DefaultAzureCredential", "ManagedIdentityCredential", "News", "Python", "Security Best Practices", "Semantic Kernel", "Service Principal"]
tags_normalized: ["ai", "api key", "authentication", "azure", "azure identity", "azure sdk", "azurechatcompletion", "azureclicredential", "coding", "credential management", "defaultazurecredential", "managedidentitycredential", "news", "python", "security best practices", "semantic kernel", "service principal"]
---

Dmytro Struk explains how Semantic Kernel Python 1.36.0 now requires explicit Azure authentication, replacing the former DefaultAzureCredential fallback. Learn the impact of this change and how to update your code.<!--excerpt_end-->

# Azure Authentication Changes in Semantic Kernel Python

In previous versions of Semantic Kernel Python, the default fallback authentication mechanism for Azure services like `AzureChatCompletion` was `DefaultAzureCredential` from the Azure Identity library. This made it easy to authenticate during development without explicitly passing credentials. However, as of version 1.36.0, this fallback has been removed to improve security and align with Azure best practices.

## Why Was This Change Made?

`DefaultAzureCredential` is handy in local development because it tries a chain of common authentication methods (Azure CLI, PowerShell, VS Code sign-ins). However, production environments demand predictability and explicit security. Relying on `DefaultAzureCredential` in production can cause:

- **Unpredictable Behavior:** Multiple sequential authentication attempts may cause delays or unpredictable failures.
- **Interactive Authentication Fallback:** Sometimes prompts for browser-based login, unsuitable for headless/cloud servers.
- **Security Concerns:** Broad fallback can mask misconfiguration, lacks control, and isn't optimal for audits or least-privilege principles.

Microsoft recommends explicit, environment-specific credentials (managed identities, service principals, or API keys) in production to minimize risk.

## What Has Changed in 1.36.0?

- **No More Implicit Default:** Initialization fails if explicit authentication is not provided. You may encounter:
  - `ServiceInitializationError: Please provide either api_key, ad_token, ad_token_provider, credential or a client.`
- **Code Updates Required:** Old code relying on the default will break. You now must specify authentication.

## How to Update Your Code

Update your code to provide a credential or token explicitly. For example:

**Old (No explicit credential):**

```python
chat_service = AzureChatCompletion()
```

**New (Explicit credential):**

```python
from azure.identity import AzureCliCredential
chat_service = AzureChatCompletion(credential=AzureCliCredential())
```

Or use `ManagedIdentityCredential`, an API key, an AD token, or other supported methods, depending on your deployment environment.

Ensure that your environment is prepared (e.g., Azure CLI logged in for `AzureCliCredential`).

## Summary

- Removal of `DefaultAzureCredential` fallback encourages developers to choose secure, explicit authentication methods.
- Update your Semantic Kernel Python code to pass credentials directly.
- For more Azure authentication info, see the [Azure Identity library documentation](https://learn.microsoft.com/en-us/python/api/overview/azure/identity-readme?view=azure-python).

If you have questions or need assistance, engage with the community on [GitHub discussions](https://github.com/microsoft/semantic-kernel/discussions).

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/azure-authentication-changes-in-semantic-kernel-python/)
