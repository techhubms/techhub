---
layout: "post"
title: "Azure DevOps OAuth Client Secrets Displayed Only Once for Improved Security"
description: "This news update outlines a significant Azure DevOps security change: OAuth client secrets, once generated, will be shown only once at creation and will no longer be retrievable through the UI or API. The article covers the transition deadlines, deprecation of the Get Registration Secret API, alternative secret rotation workflows, and recommendations for secure handling and storage of credentials within the Azure DevOps ecosystem."
author: "Angel Wong"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/devops/azure-devops-oauth-client-secrets-now-shown-only-once/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/devops/feed/"
date: 2025-08-13 15:31:56 +00:00
permalink: "/2025-08-13-Azure-DevOps-OAuth-Client-Secrets-Displayed-Only-Once-for-Improved-Security.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["API Deprecation", "Azure", "Azure DevOps", "Azure Key Vault", "Client Secrets", "Credential Management", "DevOps", "Get Registration Secret API", "Identity Protection", "News", "OAuth", "Secret Rotation", "Secret Storage", "Secure First Initiative", "Security", "Security Best Practices"]
tags_normalized: ["api deprecation", "azure", "azure devops", "azure key vault", "client secrets", "credential management", "devops", "get registration secret api", "identity protection", "news", "oauth", "secret rotation", "secret storage", "secure first initiative", "security", "security best practices"]
---

Angel Wong details an upcoming Azure DevOps security update requiring OAuth client secrets to be saved securely at creation, since they will no longer be retrievable after that point. This move highlights major changes to secret handling and API workflows for developers and DevOps professionals.<!--excerpt_end-->

# Azure DevOps OAuth Client Secrets Displayed Only Once for Improved Security

Azure DevOps is introducing an important security enhancement for OAuth client secrets to align with industry best practices and strengthen the platform's overall security.

## Key Change Summary

- **Starting September 2, 2025, OAuth client secrets generated in Azure DevOps will be displayed only once at the moment of creation.**
- After initial creation, secrets will not be retrievable via the web UI or API.
- DevOps teams must **securely store secrets** upon creation using trusted solutions like Azure Key Vault.

## API Deprecation and Secret Rotation Workflow

- The [Get Registration Secret API](https://learn.microsoft.com/en-us/rest/api/azure/devops/delegatedauth/registration-secret/get?view=azure-devops-rest-7.2) is being **deprecated and removed**. Retrieval of existing secrets will no longer be possible.
- Workflows relying on this API for secret rotation **must be updated** before the change takes effect.
- Developers are advised to use the [new secret rotation APIs](https://devblogs.microsoft.com/devops/new-overlapping-secrets-on-azure-devops-oauth/), which support overlapping secrets to ensure continuity and avoid downtime during secret updates.

## Recommended Actions

- **Remove usage of the Get Registration Secret API** in all workflows as soon as possible.
- **Adopt a secure secret storage practice** (e.g., Azure Key Vault) if not already in place.
- **Update secret rotation processes** to use official APIs supporting overlapping secrets.
- Review the [Azure DevOps OAuth documentation](https://learn.microsoft.com/en-us/azure/devops/integrate/get-started/authentication/oauth?view=azure-devops) for the latest best practices.

## Security Impact

- This change reduces the risk of accidental exposure of sensitive credentials.
- Emphasizes a **'Secure First' approach** to identity protection across the Azure DevOps ecosystem.
- Developers and administrators are encouraged to update their systems and reach out to the Azure DevOps Identity team if questions or challenges arise.

## References

- [Azure DevOps Blog: Azure DevOps OAuth Client Secrets Now Shown Only Once](https://devblogs.microsoft.com/devops/azure-devops-oauth-client-secrets-now-shown-only-once/)
- [Microsoft Secure Future Initiative](https://www.microsoft.com/en-us/trust-center/security/secure-future-initiative?msockid=3cac654edbe6660f38317687da8467bd)

For any further queries or needed assistance, DevOps professionals are advised to contact the Azure DevOps Identity team.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/azure-devops-oauth-client-secrets-now-shown-only-once/)
