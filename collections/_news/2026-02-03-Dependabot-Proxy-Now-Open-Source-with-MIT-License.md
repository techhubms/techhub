---
external_url: https://github.blog/changelog/2026-02-03-the-dependabot-proxy-is-now-open-source-with-an-mit-license
title: Dependabot Proxy Now Open Source with MIT License
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-03 16:48:51 +00:00
tags:
- Authentication
- Azure DevOps
- Dependabot
- Dependency Management
- DevOps
- DevOps Tools
- Docker
- Ecosystem & Accessibility
- GitHub
- Maven
- MIT License
- News
- npm
- NuGet
- Open Source
- Security
- Software Compliance
- Supply Chain Security
- Terraform
section_names:
- devops
- security
---
Allison announces that the Dependabot Proxy, which manages authentication for dependency updates, is now open source under the MIT license, providing greater transparency and collaboration opportunities for developers and security teams.<!--excerpt_end-->

# Dependabot Proxy Now Open Source with MIT License

The [Dependabot Proxy](https://github.com/dependabot/proxy) is now available as open source under the MIT license.

## What’s New

- You can inspect the full source code to understand authentication for various package managers and Git servers.
- The community can submit bug fixes or add support for additional package ecosystems.
- Users can file issues and collaborate with the development team in public.

## Technical Overview

Dependabot Proxy is an HTTP proxy written in Go. Its core function is authenticating Dependabot's connections to the GitHub API and to private package registries. It currently supports an array of package managers:

- npm
- Maven
- Docker
- Cargo
- Helm
- NuGet
- pip
- RubyGems
- Terraform

Additionally, it can handle authentication with Git servers such as:

- GitHub
- Azure DevOps
- Others

## Why This Matters

Since 2019, Dependabot has helped millions of GitHub users to stay current with dependencies and address security vulnerabilities. By open sourcing the proxy, developers and security professionals in organizations with compliance requirements can fully audit the authentication mechanisms involved in their dependency updates.

This transparency is vital for meeting compliance standards and bolstering software supply chain security.

## Learn More

- [Dependabot Proxy Repository](https://github.com/dependabot/proxy)
- [About Dependabot](https://docs.github.com/code-security/dependabot)
- [dependabot-core (engine)](https://github.com/dependabot/dependabot-core)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-03-the-dependabot-proxy-is-now-open-source-with-an-mit-license)
