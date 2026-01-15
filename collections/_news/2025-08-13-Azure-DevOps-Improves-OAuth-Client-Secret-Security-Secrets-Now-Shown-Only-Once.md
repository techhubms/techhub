---
layout: post
title: 'Azure DevOps Improves OAuth Client Secret Security: Secrets Now Shown Only Once'
author: Angel Wong
canonical_url: https://devblogs.microsoft.com/devops/azure-devops-oauth-client-secrets-now-shown-only-once/
viewing_mode: external
feed_name: Microsoft DevBlog
feed_url: https://devblogs.microsoft.com/devops/feed/
date: 2025-08-13 15:31:56 +00:00
permalink: /azure/news/Azure-DevOps-Improves-OAuth-Client-Secret-Security-Secrets-Now-Shown-Only-Once
tags:
- API Deprecation
- Azure
- Azure DevOps
- Azure Key Vault
- Client Secrets
- Credential Storage
- DevOps
- DevOps Workflows
- News
- OAuth
- Secret Management
- Secret Rotation
- Secure First Initiative
- Security
- Security Best Practices
section_names:
- azure
- devops
- security
---
Angel Wong announces an important change to how Azure DevOps handles OAuth client secrets, introducing a ‘show-once’ system to improve security and retiring the existing secret retrieval API.<!--excerpt_end-->

# Azure DevOps Improves OAuth Client Secret Security: Secrets Now Shown Only Once

**Author:** Angel Wong  

Azure DevOps is updating its OAuth client secret management to strengthen security and align with industry best practices. Beginning September 2, 2025, any newly generated OAuth client secrets will be displayed only once—at the time of creation. After that moment, client secrets cannot be retrieved again through the UI or API.

## Key Changes

- **Client Secrets Visibility:**
  - New client secrets will be **shown only once at creation**.
  - Secrets will not be accessible later through either the UI or API.
- **API Retirement:**
  - The [Get Registration Secret API](https://learn.microsoft.com/en-us/rest/api/azure/devops/delegatedauth/registration-secret/get?view=azure-devops-rest-7.2) will be deprecated and removed.
  - Users must update workflows and remove any dependency on this API.
- **Secret Rotation and Storage:**
  - If access to a secret is lost, rotation must be performed with the [new secret rotation APIs](https://devblogs.microsoft.com/devops/new-overlapping-secrets-on-azure-devops-oauth/), which support overlapping secrets to minimize downtime.
  - Use secure storage solutions like **Azure Key Vault** or other secrets management vaults to safely store secrets at time of creation.

## Action Items for Developers and DevOps Teams

- Review all usage of client secrets and ensure that the new ‘show-once’ policy is understood by your team.
- Remove use of the retired **Get Registration Secret API** from secret rotation scripts or workflows this month.
- Refactor authentication or integration flows to use the [new overlapping secret rotation APIs](https://devblogs.microsoft.com/devops/new-overlapping-secrets-on-azure-devops-oauth/).
- Store secrets in a secure and compliant manner immediately upon creation.

## Security Impact

- Aligns Azure DevOps with industry standards and Microsoft’s [Secure First Initiative](https://www.microsoft.com/en-us/trust-center/security/secure-future-initiative?msockid=3cac654edbe6660f38317687da8467bd).
- Reduces accidental exposure and misuse of secrets.
- Encourages robust secret management and secure development practices.

## Resources

- [Azure DevOps OAuth documentation](https://learn.microsoft.com/en-us/azure/devops/integrate/get-started/authentication/oauth?view=azure-devops)
- [Azure Key Vault Documentation](https://learn.microsoft.com/en-us/azure/key-vault/general/basic-concepts)
- [Azure DevOps Blog Announcement](https://devblogs.microsoft.com/devops/azure-devops-oauth-client-secrets-now-shown-only-once/)

If you need assistance updating workflows or have questions, reach out to the Azure DevOps Identity team.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/azure-devops-oauth-client-secrets-now-shown-only-once/)
