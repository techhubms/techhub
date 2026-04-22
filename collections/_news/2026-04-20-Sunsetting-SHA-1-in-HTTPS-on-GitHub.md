---
author: Allison
tags:
- Brownout
- Browser Compatibility
- CDN
- Deprecation Schedule
- DevOps
- Ecosystem & Accessibility
- Git Clients
- GitHub
- GitHub API
- GitHub Enterprise Cloud
- GitHub Enterprise Cloud With Data Residency
- HTTPS
- News
- OpenSSL
- Retired
- Security
- SHA 1
- TLS
- TLS Algorithms
title: Sunsetting SHA-1 in HTTPS on GitHub
date: 2026-04-20 14:15:30 +00:00
feed_name: The GitHub Blog
primary_section: devops
section_names:
- devops
- security
external_url: https://github.blog/changelog/2026-04-20-sunsetting-sha-1-in-https-on-github
---

Allison announces GitHub’s plan to remove SHA-1 from HTTPS/TLS on github.com and partner CDNs, outlining the 2026 brownout and full shutdown dates and what browser, API client, and Git tooling updates may be needed to stay compatible.<!--excerpt_end-->

## What’s changing

GitHub will remove the use of **SHA-1** in **HTTPS/TLS** for **GitHub** and its **CDNs**.

This can affect:

- Browsers used to view the GitHub website
- Software that uses the **GitHub API**
- Git clients that **push/pull over HTTPS**

Scope:

- Applies to **github.com**, including:
  - **GitHub Enterprise Cloud**
  - **GitHub Enterprise Cloud with Data Residency**
- **GitHub Enterprise Server** is **not affected**

## Deprecation schedule

GitHub will run a **brownout** (temporary disablement) to raise awareness and help identify impacted clients.

Planned schedule:

- **July 14th, 2026**: Brownout from **00:00 to 18:00 UTC** disabling SHA-1
  - The brownout **will not impact CDNs**
- **September 15th, 2026**: SHA-1 in HTTPS/TLS will be **completely disabled** for GitHub and **partner CDNs**

If you are impacted by the brownout, you’ll need to add support for **more modern TLS algorithms**.

## Preparing for removal

### Browsers

- Use a **modern, up-to-date browser** to ensure support for algorithms newer than SHA-1.
- Check your browser documentation for supported algorithms.

Test:

- Visit **https://github.dev** (SHA-1 is already disabled there).
- If it loads without connection issues, your browser likely supports modern HTTPS configurations.

### GitHub API clients

- Ensure your API integration uses a **modern framework or library** for HTTPS connections.

### Git clients over HTTPS

- Use a **recent version of Git**.
- Git can rely on different TLS libraries/backends depending on platform.
  - Example: Git on Linux may use **OpenSSL** as its TLS backend.
- Keep your OS and related components up to date so the TLS stack supports modern algorithms.


[Read the entire article](https://github.blog/changelog/2026-04-20-sunsetting-sha-1-in-https-on-github)

