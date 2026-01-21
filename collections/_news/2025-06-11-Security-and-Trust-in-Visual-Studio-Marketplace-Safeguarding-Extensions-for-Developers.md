---
external_url: https://devblogs.microsoft.com/blog/security-and-trust-in-visual-studio-marketplace
title: 'Security and Trust in Visual Studio Marketplace: Safeguarding Extensions for Developers'
author: Sean
feed_name: Microsoft DevBlog
date: 2025-06-11 22:00:21 +00:00
tags:
- Extension Development
- Extension Validation
- Extensions
- Impersonation Prevention
- Malware Prevention
- Package Management
- Publisher Best Practices
- Publisher Verification
- Secret Scanning
- Supply Chain Security
- Trust Signals
- Visual Studio Marketplace
- VS
- VS Code
section_names:
- coding
- devops
- security
---
Authored by Sean, this article delivers a comprehensive overview of efforts to ensure extension security and developer trust in the Visual Studio Marketplace, outlining both current safeguards and planned investments.<!--excerpt_end-->

## Security and Trust in Visual Studio Marketplace

In today’s fast-paced digital world, keeping the Visual Studio Marketplace secure and trustworthy is extremely important. Protecting extensions and users' interests is a top priority, and Microsoft is continually investing in new ways to ensure security and trustworthiness for everyone involved.

### Protection Against Malware

A robust multi-step process is in place to detect and mitigate malware risks:

1. **Initial Scan**: All incoming packages are scanned for malware using Microsoft Defender and other advanced antivirus technologies. Any package that fails is blocked from the Marketplace immediately.
2. **Rescanning**: Every newly published package is rescanned shortly after launch to catch emerging threats that initial scans might miss.
3. **Periodic Marketplace-wide Scans**: Regular, bulk rescans of all Marketplace packages help to counter new attack vectors and maintain ongoing safety.
4. **Dynamic Detection**: Incoming VS Code packages are tested in sandbox environments to catch malicious runtime behaviors. Suspicious packages are manually reviewed to avoid false positives before removal.
5. **Community Reporting**: Beyond automation, community members are empowered to report suspected malware or abusive extensions (e.g., invasive data collection). Reports are reviewed within one business day, and flagged packages are rescanned and investigated thoroughly.

When malware is confirmed—either by automation or community input—the extension is removed, and the publisher may be banned. Information about removed packages is publicly available, and malicious VS Code extensions are actively blocked in the product to prevent harm.

**Impact:** In the current year, 136 extensions were reviewed for malicious code, and 110 were removed to prevent potential damage. Findings continue to refine the scanning tools and guide future investments.

### Improving Developer Trust

Microsoft employs several additional mechanisms:

- **Unusual Metrics Monitoring:** Usage patterns and metrics are monitored for anomaly detection and system abuse prevention.
- **Impersonation Prevention:** Techniques block attempts to imitate trusted publishers or extensions through confusing names and assets.
- **Signature Verification:** All VS Code extensions are repository-signed upon publication, and installation enforces signature verification for package integrity.
- **Stringent Publisher Verification:** Publishers need to undergo domain and reputation checks over at least six months before receiving a 'verified publisher' badge, indicated by a blue checkmark.

### In the Works

Investments currently underway include:

- **Detecting Copycats:** Reviewing for repository and logo duplication cases that may signal impersonation.
- **Secret Scanning:** Implementing secret scanning to prevent publishing extensions containing sensitive data. Recent features in the vsce tool further reduce risks related to .env files.
- **Publisher Vetting:** Strengthening onboarding for publishers to ensure authenticity and accountability.

### On the Roadmap

Future security initiatives may include:

- **Security Risk Surfacing:** Proactive identification and user notification of high-risk extension behaviors, such as code obfuscation or remote code execution.
- **Strong Extension Authenticity:** Expanding publisher-signing for Microsoft extensions and enabling tools for third-party publishers.
- **Trust and Transparency Enhancements:** Improvements in publisher profiles, reputation indicators, and review systems.
- **Continuous Threat Monitoring:** Ongoing registry scans for vulnerabilities.
- **Further Impersonation Risk Reductions:** Enhanced detection for impersonation scenarios.

### Publisher Best Practices

Extension publishers are encouraged to:

- Become verified to bolster user trust.
- Use unique and clear names and visuals to prevent confusion.
- Avoid duplicating content or metadata.
- Publish clean, unobfuscated code to reduce false positives in malware detection.
- Include proper licenses and third-party attributions.
- Prevent secret leaks by excluding sensitive information from packages (using the latest vsce with secret scanning).
- Assign publisher account roles minimally and review member access regularly.

### Recommendations for Developers Installing Extensions

When choosing extensions, users should:

- Review **ratings, reviews, and Q&A** to assess publisher responsiveness and extension reliability.
- Look for the **Verified Publisher** badge as a mark of trustworthiness.
- Consider the extension's install/download count.
- Examine the **repository link** for security policies, licenses, and bug reports.

Visit [extension publisher trust](https://code.visualstudio.com/docs/configure/extensions/extension-runtime-security#_extension-publisher-trust) for more.

### Appreciation

Thanks are extended to both publishers, for their commitment to a safe Marketplace, and users, whose reports and feedback drive ongoing improvements and strong defenses in a rapidly evolving developer ecosystem.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/blog/security-and-trust-in-visual-studio-marketplace)
