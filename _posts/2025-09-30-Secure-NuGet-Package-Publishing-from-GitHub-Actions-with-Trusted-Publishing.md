---
layout: "post"
title: "Secure NuGet Package Publishing from GitHub Actions with Trusted Publishing"
description: "This guide by Andrew Lock details how to automate the publishing of NuGet packages to nuget.org using the new Trusted Publishing feature. It covers traditional methods, explains the OpenID Connect-based Trusted Publishing workflow with GitHub Actions, and outlines step-by-step setup, configuration, and security improvements users can expect."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/easily-publishing-nuget-packages-from-github-actions-with-trusted-publishing/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-09-30 10:00:00 +00:00
permalink: "/2025-09-30-Secure-NuGet-Package-Publishing-from-GitHub-Actions-with-Trusted-Publishing.html"
categories: ["DevOps", "Security"]
tags: [".NET", "API Key Management", "CI/CD", "DevOps", "GitHub", "GitHub Actions", "Identity Provider", "JWT", "NuGet", "NuGet Package Publishing", "OpenID Connect", "Posts", "Secrets Management", "Secure Automation", "Security", "Security Best Practices", "Trusted Publishing", "Workflow Automation", "YAML"]
tags_normalized: ["dotnet", "api key management", "cislashcd", "devops", "github", "github actions", "identity provider", "jwt", "nuget", "nuget package publishing", "openid connect", "posts", "secrets management", "secure automation", "security", "security best practices", "trusted publishing", "workflow automation", "yaml"]
---

Andrew Lock explains how to safely automate NuGet package publishing directly from GitHub Actions using the Trusted Publishing feature on nuget.org, reducing credential risk and streamlining release workflows.<!--excerpt_end-->

# Secure NuGet Package Publishing from GitHub Actions with Trusted Publishing

*Author: Andrew Lock*

Trusted Publishing on nuget.org offers a secure and streamlined way to publish NuGet packages directly from CI/CD pipelines like GitHub Actions, eliminating the need for long-lived API keys and improving workflow security.

## Traditional NuGet Publishing Approaches

- Manual upload to nuget.org
- CI build, manual download, and manual upload
- Direct push from CI, typically requiring API key management

Traditional approaches require generating and managing API keys, which brings challenges around secure storage, secret rotation, and organizational sharing. Managing these secrets is complex and error-prone.

## What is Trusted Publishing?

Trusted Publishing uses OpenID Connect (OIDC) to authenticate CI providers (like GitHub Actions) with nuget.org, removing the need for persistent API secrets. On workflow execution:

- Configure a trust policy on nuget.org
- CI retrieves a short-lived OIDC token from GitHub
- nuget.org verifies the token using the defined policy
- If matched, a short-lived API key is issued for package publishing

This approach brings .NET/NuGet in line with other ecosystems such as PyPI, RubyGems, and npm.

## GitHub Actions Workflow Example

**Starting Point:**
The workflow basic setup (`BuildAndPack.yml`) typically:

- Checks out the repository
- Installs .NET
- Packs the project using `dotnet pack`

**Enhancing for Trusted Publishing:**

1. Add `id-token: write` to job permissions (to enable OIDC).
2. Use `NuGet/login@v1` GitHub Action to exchange the OIDC token for a short-lived NuGet API key.
3. Push the package using the generated API key.

**Example workflow changes:**

```yaml
permissions:
  contents: read
  id-token: write
steps:
  - uses: actions/checkout@v4
  - uses: actions/setup-dotnet@v4
    with:
      dotnet-version: 10.0.x
  - name: dotnet pack
    run: |
      dotnet pack
      dotnet pack -r win-x64
  - name: NuGet login (OIDC → temp API key)
    uses: NuGet/login@v1
    id: login
    with:
      user: ${{ secrets.NUGET_USER }}
  - name: push to NuGet
    if: startsWith(github.ref, 'refs/tags/')
    shell: pwsh
    run: |
      Get-ChildItem artifacts/package/release -Filter *.nupkg | ForEach-Object {
        dotnet nuget push ---
layout: "post"
title: "Secure NuGet Package Publishing from GitHub Actions with Trusted Publishing"
description: "This guide by Andrew Lock details how to automate the publishing of NuGet packages to nuget.org using the new Trusted Publishing feature. It covers traditional methods, explains the OpenID Connect-based Trusted Publishing workflow with GitHub Actions, and outlines step-by-step setup, configuration, and security improvements users can expect."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/easily-publishing-nuget-packages-from-github-actions-with-trusted-publishing/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: https://andrewlock.net/rss.xml
date: 2025-09-30 10:00:00 +00:00
permalink: "2025-09-30-Secure-NuGet-Package-Publishing-from-GitHub-Actions-with-Trusted-Publishing.html"
categories: ["DevOps", "Security"]
tags: [".NET", "API Key Management", "CI/CD", "DevOps", "GitHub", "GitHub Actions", "Identity Provider", "JWT", "NuGet", "NuGet Package Publishing", "OpenID Connect", "Posts", "Secrets Management", "Secure Automation", "Security", "Security Best Practices", "Trusted Publishing", "Workflow Automation", "YAML"]
tags_normalized: [["dotnet", "api key management", "cislashcd", "devops", "github", "github actions", "identity provider", "jwt", "nuget", "nuget package publishing", "openid connect", "posts", "secrets management", "secure automation", "security", "security best practices", "trusted publishing", "workflow automation", "yaml"]]
---

Andrew Lock explains how to safely automate NuGet package publishing directly from GitHub Actions using the Trusted Publishing feature on nuget.org, reducing credential risk and streamlining release workflows.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/easily-publishing-nuget-packages-from-github-actions-with-trusted-publishing/)
.FullName --api-key "${{ steps.login.outputs.NUGET_API_KEY }}" --source https://api.nuget.org/v3/index.json
      }
```

Only the NuGet username (`NUGET_USER`) needs to be stored as a secret; no persistent API keys are required.

## Configuring Trusted Publishing on NuGet.org

- Sign in to nuget.org
- Go to the Trusted Publishing page under account settings
- Create a new trust policy specifying:
  - A descriptive name
  - Package owner
  - GitHub repository details
  - Workflow file path

After first use, the policy becomes fully active. When the workflow runs, the login step succeeds and the package is published securely.

## Benefits and Future Directions

- No long-lived credentials or secrets stored in repositories
- Easier setup and collaboration, especially for open source and organizational projects
- Potential for further enhancements like package provenance and verification marks

## Conclusion

Trusted Publishing on nuget.org with GitHub Actions leverages modern security standards to simplify and secure the package publishing process. It minimizes secret-handling risks while offering a seamless automation path for package maintainers.

---
*For more details and sample workflows, visit the official nuget.org [Trusted Publishing documentation](https://www.nuget.org/account/trustedpublishing).*

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/easily-publishing-nuget-packages-from-github-actions-with-trusted-publishing/)
