---
layout: "post"
title: "Dependabot Proxy Now Open Source with MIT License"
description: "Dependabot Proxy, the authentication service behind GitHub's dependency management automation, is now open source under the MIT license. This enables developers to inspect, contribute to, and extend dependency authentication workflows, including integration with private registries and multiple package ecosystems such as npm, Maven, Docker, NuGet, and more."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-03-the-dependabot-proxy-is-now-open-source-with-an-mit-license"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-03 16:48:51 +00:00
permalink: "/2026-02-03-Dependabot-Proxy-Now-Open-Source-with-MIT-License.html"
categories: ["DevOps", "Security"]
tags: ["Authentication", "Azure DevOps", "Dependabot", "Dependency Management", "DevOps", "DevOps Tools", "Docker", "Ecosystem & Accessibility", "GitHub", "Maven", "MIT License", "News", "npm", "NuGet", "Open Source", "Security", "Software Compliance", "Supply Chain Security", "Terraform"]
tags_normalized: ["authentication", "azure devops", "dependabot", "dependency management", "devops", "devops tools", "docker", "ecosystem and accessibility", "github", "maven", "mit license", "news", "npm", "nuget", "open source", "security", "software compliance", "supply chain security", "terraform"]
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
