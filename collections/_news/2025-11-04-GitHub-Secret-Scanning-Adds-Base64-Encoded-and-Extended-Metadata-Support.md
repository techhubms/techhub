---
layout: "post"
title: "GitHub Secret Scanning Adds Base64-Encoded and Extended Metadata Support"
description: "This update details GitHub's October 2025 improvements to secret scanning, including the detection of Base64-encoded secrets from providers like Azure and AWS, expanded metadata checks, and new validity checks. These changes enhance application security and developer workflows by proactively identifying and protecting sensitive information in code repositories."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-11-04-secret-scanning-now-detects-base64-encoded-secrets"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-11-04 19:21:06 +00:00
permalink: "/2025-11-04-GitHub-Secret-Scanning-Adds-Base64-Encoded-and-Extended-Metadata-Support.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["API Keys", "Application Security", "AWS", "Azure", "Azure Key Detection", "Base64 Encoded Secrets", "Cloud Security", "Continuous Integration", "DevOps", "GitHub", "GitHub Actions", "Grafana", "Identity Protection", "Metadata Validation", "News", "Notion", "Provider Integration", "Secret Scanning", "Security"]
tags_normalized: ["api keys", "application security", "aws", "azure", "azure key detection", "base64 encoded secrets", "cloud security", "continuous integration", "devops", "github", "github actions", "grafana", "identity protection", "metadata validation", "news", "notion", "provider integration", "secret scanning", "security"]
---

Allison summarizes significant October 2025 improvements to GitHub secret scanning, focusing on extended detection and security measures that benefit developers and organizations using Azure, AWS, and other platforms.<!--excerpt_end-->

# GitHub Secret Scanning: New Features for October 2025

GitHub continues to strengthen its security capabilities with recent updates to secret scanning. These changes further protect developers and organizations from accidental exposure of sensitive information such as API keys, credentials, and tokens.

## Key Updates

### 1. Detection of Base64-Encoded Secrets

- **GitHub secret scanning** now detects Base64-encoded versions of secrets from major cloud providers, making it harder for obfuscated secrets to slip through unnoticed.
- Commonly detected Base64-encoded secrets include:
  - Azure: `azure_cache_for_redis_access_key`, `azure_cosmosdb_key_identifiable`, `azure_function_key`, `azure_openai_key`, `azure_storage_account_key`
  - AWS: `aws_access_key_id`, `aws_secret_access_key`, `aws_temporary_access_key_id`
  - Google, GitHub, GitLab, and others.
- These secrets are push protected by default, proactively reducing risk.

### 2. Extended Metadata Checks

- Secret scanning now supports enhanced context checks—such as owner info, creation dates, and organization metadata—for a broader range of secret types.
- Applies to API keys and tokens from over 30 providers, including Adafruit, Anthropic, Discord, Dropbox, Fastly, GitLab, Google, Figma, Intercom, Mailchimp, Mailgun, Mapbox, Notion, OpenAI, Postman, SendGrid, Slack, Stripe, Telegram, Terraform Cloud, and more.
- These extended checks give security teams improved traceability and control for exposed secrets.

### 3. Validity Checks

- For providers including Grafana and Notion, GitHub can now verify whether a detected secret is still active—enabling faster response and mitigation.

## Why This Matters

- **Developers and DevOps teams** benefit from earlier detection of more complex (obfuscated) leaks, especially for commonly used cloud platforms.
- **Security and compliance** are strengthened with added traceability and automatic push protection.
- **Azure and other Microsoft technology users** now gain broader coverage for cloud credentials, making Microsoft-focused repositories safer by default.

## Further Reading and Resources

- [Secret scanning documentation](https://docs.github.com/en/code-security/secret-scanning/introduction/about-secret-scanning?utm_source=changelog-docs-extended-metadata-checks&utm_medium=changelog&utm_campaign=universe25)
- [Comprehensive list of supported secret patterns](https://docs.github.com/code-security/secret-scanning/introduction/supported-secret-scanning-patterns)
- [GitHub Universe announcement](https://github.blog/changelog/2025-10-28-introducing-extended-metadata-checks-for-secret-scanning/)

---

GitHub will continue rolling out additional secret types and security features. These ongoing advancements offer teams using Azure, AWS, and other cloud providers more ways to safeguard their code and cloud environments.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-04-secret-scanning-now-detects-base64-encoded-secrets)
