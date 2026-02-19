---
layout: "post"
title: "Microsoft Adds Telemetry to its Go Build for FIPS Compliance and Azure Linux"
description: "This article discusses Microsoft's decision to add telemetry to its custom build of the Go language toolchain, primarily used for FIPS compliance and internally on Azure Linux. It covers technical details, compliance motivations, architecture differences from the official Go distribution, privacy considerations, and Microsoft's continued support for its build over new official options."
author: "DevClass.com"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.devclass.com/development/2025/07/08/things-go-better-with-telemetry-microsoft-adds-phone-home-to-its-go-build/101140"
viewing_mode: "external"
feed_name: "DevClass"
feed_url: "https://devclass.com/feed/"
date: 2025-07-08 15:55:00 +00:00
permalink: "/2025-07-08-Microsoft-Adds-Telemetry-to-its-Go-Build-for-FIPS-Compliance-and-Azure-Linux.html"
categories: ["Azure", "Security"]
tags: ["Artifact Registry", "Azure", "Azure Linux", "Blogs", "BoringCrypto", "Cryptography", "Docker", "FIPS 140 3", "Go Language", "Go Toolchain", "Macos Support", "Microsoft Azure", "Microsoft Go Build", "NIST", "Security", "Security Compliance", "Telemetry"]
tags_normalized: ["artifact registry", "azure", "azure linux", "blogs", "boringcrypto", "cryptography", "docker", "fips 140 3", "go language", "go toolchain", "macos support", "microsoft azure", "microsoft go build", "nist", "security", "security compliance", "telemetry"]
---

DevClass.com explains how Microsoft is integrating telemetry into its Go toolchain build to enhance FIPS compliance features, especially for use in Azure Linux and regulated environments.<!--excerpt_end-->

# Microsoft Adds Telemetry to its Go Build for FIPS Compliance and Azure Linux

## Overview

Microsoft will begin collecting telemetry in its custom build of the Go language compiler and tools. This edition is used to provide enhanced security and compliance, notably for FIPS 140-3 regulations, and is deployed internally at Microsoft, including Azure Linux. The official upstream Go binaries remain unaffected by this telemetry addition.

## Technical Context

- **Telemetry Scope**: The Microsoft Go toolchain will add anonymous telemetry for versions 1.25 and above to better understand developer workflows, prioritize improvements, and optimize performance. Users can opt out by setting `MS_GOTOOLCHAIN_TELEMETRY_ENABLED=0`.
- **Use Cases**: Microsoft's Go distribution is frequently chosen by organizations needing FIPS 140-3 cryptographic compliance in regulated sectors like finance and government. The build achieves compliance by calling platform-provided crypto libraries instead of relying on BoringCrypto modules, which, though certified, are not officially supported by Google.
- **Recent Developments**: Go 1.24 introduces a native FIPS 140-3 mode (pending NIST validation), reducing the necessity for third-party (including Microsoft) builds. However, Microsoft will continue maintaining its system-library-driven approach for internal policy reasons.

## Developer Impact

- Developers currently using the Microsoft build for compliance reasons may want to review the official Go FIPS module when it passes certification, as recommended by Microsoft engineers.
- The lack of macOS support in previous Microsoft Go builds is being addressed, targeting readiness in Go 1.25.
- Microsoft's build is also utilized in Docker images (via the Microsoft Artifact Registry) and Azure Linux, ensuring out-of-the-box FIPS 140-3 compliance for these environments.

## Privacy and Transparency

- Collected telemetry is stated to be anonymized and focused on tool improvement, not user identification.
- Users have a clear opt-out path via environment variable configuration.

## Conclusion

Microsoft's telemetry addition is mainly relevant for regulated workloads on Azure or Azure Linux requiring government-grade cryptographic validation. Organizations should balance compliance requirements and privacy preferences before adopting the new toolchain version.

This post appeared first on "DevClass". [Read the entire article here](https://www.devclass.com/development/2025/07/08/things-go-better-with-telemetry-microsoft-adds-phone-home-to-its-go-build/101140)
