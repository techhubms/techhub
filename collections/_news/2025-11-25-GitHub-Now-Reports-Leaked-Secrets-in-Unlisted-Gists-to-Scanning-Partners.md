---
layout: "post"
title: "GitHub Now Reports Leaked Secrets in Unlisted Gists to Scanning Partners"
description: "This announcement details a new security measure from GitHub: any secrets leaked in both public and unlisted (secret-labeled) gists are now reported to the appropriate secret scanning partners. The update clarifies that unlisted gists offer no additional privacy, encouraging developers to treat all gist content as publicly accessible and to use private repositories for sensitive code. The post outlines how GitHub works with partners such as AWS, OpenAI, and Stripe to detect and notify on leaked secrets, strengthening confidentiality and incident response for developers using gists."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-11-25-secrets-in-unlisted-github-gists-are-now-reported-to-secret-scanning-partners"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-11-25 17:50:17 +00:00
permalink: "/news/2025-11-25-GitHub-Now-Reports-Leaked-Secrets-in-Unlisted-Gists-to-Scanning-Partners.html"
categories: ["DevOps", "Security"]
tags: ["Application Security", "AWS", "Code Leaks", "Code Sharing", "DevOps", "DevOps Best Practices", "GitHub", "GitHub Gists", "Improvement", "Incident Response", "News", "OpenAI", "Partner Program", "Repository Security", "Secret Scanning", "Secrets Management", "Security", "Security Alert", "Stripe"]
tags_normalized: ["application security", "aws", "code leaks", "code sharing", "devops", "devops best practices", "github", "github gists", "improvement", "incident response", "news", "openai", "partner program", "repository security", "secret scanning", "secrets management", "security", "security alert", "stripe"]
---

Allison explains GitHub’s update to its secret scanning process, highlighting how secrets leaked in any gist, including unlisted ones, are now reported to scanning partners to help prevent exposure.<!--excerpt_end-->

# GitHub Reports Leaked Secrets in Unlisted Gists to Secret Scanning Partners

Starting today, GitHub will alert secret scanning partners to any publicly leaked secrets found in unlisted (secret-labeled) GitHub gists.

## Key Update

- Secrets in both public and unlisted gists are now reported to secret scanning partners (such as AWS, OpenAI, and Stripe).
- Gists marked as 'secret' are not private—anyone with the URL can access them, making them a potential blind spot for leaks.

## GitHub Secret Scanning Partnership Program

- GitHub collaborates with industry partners to detect secrets leaked in various formats, aiming for high detection accuracy and minimal false positives.
- On discovery, GitHub notifies the actual secret issuer for immediate action, and, where enabled, will also alert the developer through secret scanning alerts.

## What Are GitHub Gists?

- Gists are a convenient way for developers to share code snippets.
- 'Public' gists are discoverable and searchable, while 'secret' gists are unlisted but not private—access is possible with the direct URL.
- For true privacy, developers should place sensitive code in private repositories.

## Best Practices for Developers

- Treat all gist content as publicly accessible.
- Do not store secrets or sensitive data in gists, whether public or secret-labeled.
- Use private repos for confidential code or credentials.

## Further Reading

- [GitHub secret scanning](https://docs.github.com/enterprise-cloud@latest/code-security/secret-scanning/introduction/about-secret-scanning)
- [Creating and sharing GitHub gists](https://docs.github.com/get-started/writing-on-github/editing-and-sharing-content-with-gists/creating-gists)
- [Secret scanning partner program](https://docs.github.com/code-security/secret-scanning/secret-scanning-partnership-program/secret-scanning-partner-program)

This change enhances developer and platform security, helping keep credentials and keys out of public exposure and fostering responsible code sharing.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-25-secrets-in-unlisted-github-gists-are-now-reported-to-secret-scanning-partners)
