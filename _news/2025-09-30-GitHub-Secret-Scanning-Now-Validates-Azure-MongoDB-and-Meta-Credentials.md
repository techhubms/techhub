---
layout: "post"
title: "GitHub Secret Scanning Now Validates Azure, MongoDB, and Meta Credentials"
description: "This news update announces GitHub's expansion of its secret scanning feature, now supporting validity checks for secrets from MongoDB, Meta, and Microsoft Azure. The article details new supported credential types, explains what validity checks are, and provides links to documentation for further guidance."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-30-secret-scanning-adds-validators-for-mongodb-meta-and-microsoft-azure"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-30 17:31:57 +00:00
permalink: "/2025-09-30-GitHub-Secret-Scanning-Now-Validates-Azure-MongoDB-and-Meta-Credentials.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Application Security", "Azure", "Azure APIM", "Azure Entra ID", "Code Security", "Continuous Integration", "Credential Security", "DevOps", "DevOps Security", "GitHub", "Improvement", "Meta", "Microsoft Azure", "MongoDB", "News", "Personal Access Token", "Product Update", "Secret Scanning", "Security", "Token Validation"]
tags_normalized: ["application security", "azure", "azure apim", "azure entra id", "code security", "continuous integration", "credential security", "devops", "devops security", "github", "improvement", "meta", "microsoft azure", "mongodb", "news", "personal access token", "product update", "secret scanning", "security", "token validation"]
---

Allison details GitHub's latest improvements to secret scanning, including new validators for Microsoft Azure, MongoDB, and Meta, helping DevOps teams better secure their credentials.<!--excerpt_end-->

# GitHub Secret Scanning Adds Validity Checks for Azure, MongoDB, and Meta Credentials

GitHub has expanded its secret scanning capabilities by introducing validity checks for additional secret types, including those from MongoDB, Meta, and Microsoft Azure.

## Expanded Secret Types with Validity Checks

The following new secret patterns are now validated:

| Provider  | Pattern                                         | Validity |
|-----------|-------------------------------------------------|----------|
| Azure     | `microsoft_ado_personal_access_token`           | ✓        |
| Azure     | `microsoft_azure_apim_repository_key_identifiable` | ✓     |
| Azure     | `microsoft_azure_maps_key`                      | ✓        |
| Azure     | `microsoft_azure_entra_id_token`                | ✓        |
| Meta      | `facebook_very_tiny_encrypted_session`          | ✓        |
| MongoDB   | `mongodb_atlas_db_uri_with_credentials`         | ✓        |

## What Are Validity Checks?

Validity checks help identify if discovered credentials are currently active and exploitable, reducing response times for remediation. If validity checks were previously enabled for a repository, GitHub automatically tests for the new patterns as well.

- **Action Required:** No action is needed if validity checks are already enabled—new token types will be checked automatically.
- For more details and the full list of supported secret patterns, visit the [GitHub documentation](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/supported-secret-scanning-patterns#default-patterns).

## Implications for DevOps and Security Teams

- **Wider Coverage:** Teams managing Azure, MongoDB, or Meta integrations gain enhanced protection against credential leaks.
- **Automated Validation:** Early notification of active credential leaks accelerates mitigation.
- **Compliance:** Supports best practices for application and infrastructure security.

## References

- [GitHub Blog: Secret scanning adds validators for MongoDB, Meta, and Microsoft Azure](https://github.blog/changelog/2025-09-30-secret-scanning-adds-validators-for-mongodb-meta-and-microsoft-azure)
- [Supported Secret Scanning Patterns Documentation](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/supported-secret-scanning-patterns#default-patterns)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-30-secret-scanning-adds-validators-for-mongodb-meta-and-microsoft-azure)
