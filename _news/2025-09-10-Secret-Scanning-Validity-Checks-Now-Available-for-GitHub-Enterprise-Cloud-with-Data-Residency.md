---
layout: "post"
title: "Secret Scanning Validity Checks Now Available for GitHub Enterprise Cloud with Data Residency"
description: "This announcement covers the introduction of validity checks for GitHub Secret Protection and GitHub Advanced Security customers using GitHub Enterprise Cloud with data residency. Validity checks automatically verify supported token types when secret scanning is enabled for repositories. The update also mentions support for new token validators, including SSLMate and Typeform, and highlights impending availability for GitHub Enterprise Server."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-10-secret-scanning-validity-checks-available-for-data-residency"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-10 20:59:55 +00:00
permalink: "/2025-09-10-Secret-Scanning-Validity-Checks-Now-Available-for-GitHub-Enterprise-Cloud-with-Data-Residency.html"
categories: ["DevOps", "Security"]
tags: ["Application Security", "Data Residency", "DevOps", "Enterprise Server", "GitHub", "GitHub Advanced Security", "GitHub Enterprise Cloud", "News", "Secret Protection", "Secret Scanning", "Security", "Security Automation", "SSLmate", "Token Validation", "Typeform"]
tags_normalized: ["application security", "data residency", "devops", "enterprise server", "github", "github advanced security", "github enterprise cloud", "news", "secret protection", "secret scanning", "security", "security automation", "sslmate", "token validation", "typeform"]
---

Allison details GitHub's rollout of validity checks for secret scanning on Enterprise Cloud with data residency, providing a significant security upgrade for DevOps teams and security practitioners.<!--excerpt_end-->

# Secret Scanning Validity Checks Now Available for GitHub Enterprise Cloud with Data Residency

GitHub has expanded platform support for security features on its Enterprise Cloud with data residency, as well as the upcoming release of Enterprise Server. This rollout introduces several key security enhancements for organizations leveraging GitHub at scale.

## Key Enhancements

- **Validity Checks for Secret Protection**:
  - Now available for GitHub Advanced Security customers on GitHub Enterprise Cloud with data residency.
  - Once enabled on a repository, GitHub will automatically verify secrets related to alerts for supported token types. [See supported token patterns](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/supported-secret-scanning-patterns#default-patterns).

- **Enterprise Server Support**:
  - Validity checks are planned for a future release of GitHub Enterprise Server. Details will be published in the GitHub changelog upon release.

## Additional Validators

GitHub has expanded the scope of secret scanning by adding support for new token types:

| Provider  | Pattern                          | Validity Check |
|-----------|----------------------------------|----------------|
| Typeform  | `typeform_personal_access_token` | ✓              |
| SSLMate   | `sslmate_api_key` and `sslmate2_api_key` | ✓     |

Organizations can benefit from enhanced security and compliance, particularly when using GitHub in regulated industries or geographies that require data residency. These updates provide more automation and assurance when detecting and validating exposed secrets in code repositories.

For more details and a list of all supported token types, refer to the [GitHub documentation](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/supported-secret-scanning-patterns#default-patterns).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-10-secret-scanning-validity-checks-available-for-data-residency)
