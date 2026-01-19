---
external_url: https://andrewlock.net/companies-using-dotnet-need-to-suck-it-up-and-pay-for-support/
title: 'Managing .NET Support Lifecycles: Why Paying for Post-EOL Support Is Practical'
author: Andrew Lock
viewing_mode: external
feed_name: Andrew Lock's Blog
date: 2025-11-18 10:00:00 +00:00
tags:
- .NET
- .NET 10
- .NET 6
- .NET 8
- .NET 9
- .NET Core
- ASP.NET Core
- CVE 55315
- End Of Life
- Enterprise Compliance
- EOL Support
- HeroDevs
- LTS
- Never Ending Support
- Patching
- Regulatory Risk
- Request Smuggling
- Runtime
- SDK
- Security Fixes
- Security Vulnerability
- STS
- Support Lifecycle
- Version Upgrades
section_names:
- coding
- security
---
Andrew Lock reviews the .NET support lifecycle and demonstrates how organizations can avoid risks from unsupported versions by leveraging HeroDevs' Never Ending Support for .NET 6.<!--excerpt_end-->

# Managing .NET Support Lifecycles: Why Paying for Post-EOL Support Is Practical

Andrew Lock provides a detailed exploration of .NET's rapid release cycle and the accompanying challenges for organizations required to keep their applications secure and compliant. He examines Microsoft's official support lifecycle, detailing both Long Term Support (LTS) and Standard Term Support (STS) policies:

- **LTS releases (even-numbered)**: Supported for 3 years
- **STS releases (odd-numbered)**: Supported for 2 years

Active support covers both security and functionality improvements, while the last 6 months ("maintenance mode") only covers security vulnerabilities. Once a release reaches End of Life (EOL), no official support or patches are provided—even for major vulnerabilities.

## The Risks of Running Unsupported .NET Versions

Lock discusses the business and regulatory risks of relying on unsupported versions. A recent example highlighted is CVE-2025-55315, a 9.9-severity request smuggling vulnerability affecting multiple .NET releases. While patches for this security issue were made available for currently supported (.NET 8, 9, 10) versions, older versions—including .NET 6—remain exposed once EOL passes.

He further clarifies that only the latest patched versions of each supported .NET release qualify for support. Organizations failing to keep pace with monthly patches risk losing official assistance from Microsoft, putting compliance and incident response in jeopardy.

## The High Cost of Major Upgrades

Lock identifies the complexity and risk of upgrading major .NET versions:

- Breaking changes and altered behaviors require careful testing
- Tooling and CI pipeline updates are often needed
- Regulatory compliance may demand re-certification
- Internal expertise can lag behind new releases
- Opportunity cost: Upgrades may detract from feature development

For end-of-development, legacy, or compliance-limited apps, major upgrades may not be feasible.

## Post-EOL Support: A Pragmatic Option

Drawing parallels to paid support models for other technologies (Java, Spring, Windows), Lock argues that paying for post-EOL support—such as HeroDevs' 'Never Ending Support' (NES) for .NET—can protect organizations that can't migrate promptly. He demonstrates with Docker examples how swapping in HeroDevs' patched .NET 6 runtime protected a sample ASP.NET Core app from CVE-2025-55315.

Key benefits of NES for .NET include:

- Drop-in compatibility (no need to recompile apps)
- Prompt security fixes after official EOL
- Compliance with regulatory requirements and industry SLAs

## Conclusion

For organizations unable to keep up with .NET's release cadence, Andrew Lock recommends considering third-party post-EOL support like HeroDevs' NES rather than running vulnerable or non-compliant apps, emphasizing it as an established practice in other technology ecosystems. He provides actionable guidance on testing vulnerable deployments, replacing runtimes, and advises that security and compliance should drive this decision. For more details, reach out to HeroDevs directly.

---

**Further Resources:**

- [Microsoft Official .NET Support Policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)
- [HeroDevs Never Ending Support for .NET 6](https://www.herodevs.com/support/dot-net-nes)
- [CVE-2025-55315 Vulnerability Details](https://github.com/dotnet/aspnetcore/issues/64033)
- [GitHub CVE-2025-55315 Repro App](https://github.com/sirredbeard/CVE-2025-55315-repro)

For technical implementation, see the Dockerfile and testing walkthroughs provided in the original post.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/companies-using-dotnet-need-to-suck-it-up-and-pay-for-support/)
