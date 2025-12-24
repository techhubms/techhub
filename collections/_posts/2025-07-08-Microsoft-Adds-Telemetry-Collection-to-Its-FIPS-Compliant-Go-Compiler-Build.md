---
layout: "post"
title: "Microsoft Adds Telemetry Collection to Its FIPS-Compliant Go Compiler Build"
description: "This article examines Microsoft's decision to add anonymous telemetry to its custom build of the Go compiler and tools used for FIPS compliance and internal usage, such as on Azure Linux. It covers how the telemetry works, opt-out mechanisms, implications for developers, and changes to FIPS compliance in Go 1.24 and 1.25."
author: "Tim Anderson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devclass.com/2025/07/08/things-go-better-with-telemetry-microsoft-adds-phone-home-to-its-go-build/"
viewing_mode: "external"
feed_name: "DevClass"
feed_url: "https://devclass.com/feed/"
date: 2025-07-08 15:55:00 +00:00
permalink: "/posts/2025-07-08-Microsoft-Adds-Telemetry-Collection-to-Its-FIPS-Compliant-Go-Compiler-Build.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Linux", "BoringCrypto", "BoringSSL", "Container Images", "Cryptography", "Development", "Development Tools", "FIPS", "FIPS 140 3", "Go", "Go 1.24", "Go 1.25", "Go Compiler", "Microsoft", "Microsoft Artifact Registry", "Microsoft Go Build", "MS GOTOOLCHAIN TELEMETRY ENABLED", "NIST", "Posts", "Security", "Telemetry"]
tags_normalized: ["azure", "azure linux", "boringcrypto", "boringssl", "container images", "cryptography", "development", "development tools", "fips", "fips 140 3", "go", "go 1dot24", "go 1dot25", "go compiler", "microsoft", "microsoft artifact registry", "microsoft go build", "ms gotoolchain telemetry enabled", "nist", "posts", "security", "telemetry"]
---

Tim Anderson explores Microsoft's addition of telemetry to its Go compiler build for FIPS compliance, discussing its impact on Azure Linux, cryptographic strategy, and developer workflows.<!--excerpt_end-->

# Microsoft Adds Telemetry Collection to Its FIPS-Compliant Go Compiler Build

**Author: Tim Anderson**

Microsoft has announced it will introduce telemetry collection to its specialized build of the Go language compiler and tools. This build is particularly notable for enabling [FIPS 140-3](https://csrc.nist.gov/publications/detail/fips/140/3/final) (Federal Information Processing Standards) compliance, a requirement in regulated contexts like financial services and government applications, and is also leveraged internally at Microsoft, including in Azure Linux.

The change does **not** affect the official Go binaries but is limited to the Microsoft-distributed Go build. The purpose of the telemetry, detailed by Microsoft software engineering manager George Adams, is to inform the company about roadmap priorities, optimization opportunities, and usage patterns among developers. All data is anonymized, and there is a clear opt-out via the environment variable `MS_GOTOOLCHAIN_TELEMETRY_ENABLED=0`.

## FIPS Compliance and Microsoft's Approach

Historically, developers seeking FIPS compliance in Go had two main options:

- Use Go with the [BoringCrypto](https://boringssl.googlesource.com/boringssl/+/master/crypto/fipsmodule/FIPS.md) module (from BoringSSL)
- Use the Microsoft build, which is modified to call a platform-provided cryptographic library for FIPS alignment

With the release of Go 1.24 (February 2025), Go introduced its own [native FIPS 140-3 mode](https://go.dev/doc/security/fips140), currently under NIST validation. Once this is completed, most developers may prefer the official Go FIPS module unless their policies align specifically with Microsoft's system cryptography approach. Microsoft has stated it will continue using its preferred system library for internal alignment with cryptographic strategy and policy.

## Platform Support and Docker Usage

A previously noted shortcoming of the Microsoft Go build—lack of macOS support—has seen improvement, with preview support now included and full support targeted for Go 1.25 (expected August 2025).

This specialized Microsoft Go build is embedded in container images published in the Microsoft Artifact Registry and Microsoft’s own Linux distribution, Azure Linux. According to engineering communications, these images offer FIPS 140-3 compliance 'out of the box.'

## Developer Impact and Recommendations

For most external developers, unless there is a need for Microsoft's unique approach or a dependency on software like Azure Linux with these images, the recommendation is to use the official Go binaries, particularly after full NIST certification of the official FIPS 140-3 module.

## Opt-Out and Transparency

Microsoft has enabled users to opt out of telemetry by setting the `MS_GOTOOLCHAIN_TELEMETRY_ENABLED` environment variable to `0`, ensuring developer control over data collection.

---

**References:**

- [Microsoft Go Telemetry Announcement](https://devblogs.microsoft.com/go/microsoft-go-telemetry/)
- [Go FIPS 140-3 Mode](https://go.dev/doc/security/fips140)
- [Microsoft’s FIPS Evaluation](https://devblogs.microsoft.com/go/go-1-24-fips-update/)

**About the Author:**
Tim Anderson covers infrastructure, programming tools, and enterprise technology for DevClass.

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2025/07/08/things-go-better-with-telemetry-microsoft-adds-phone-home-to-its-go-build/)
