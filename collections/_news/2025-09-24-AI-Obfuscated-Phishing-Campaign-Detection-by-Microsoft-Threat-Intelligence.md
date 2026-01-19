---
external_url: https://www.microsoft.com/en-us/security/blog/2025/09/24/ai-vs-ai-detecting-an-ai-obfuscated-phishing-campaign/
title: AI-Obfuscated Phishing Campaign Detection by Microsoft Threat Intelligence
author: Microsoft Threat Intelligence
viewing_mode: external
feed_name: Microsoft Security Blog
date: 2025-09-24 12:00:00 +00:00
tags:
- AI Generated Code
- Browser Fingerprinting
- CAPTCHA
- Credential Theft
- Cybersecurity
- Defender XDR
- Incident Response
- Microsoft Defender
- Microsoft Sentinel
- Obfuscation
- Office 365
- Phishing
- Security Copilot
- SVG Payload
- Threat Intelligence
section_names:
- ai
- security
---
Microsoft Threat Intelligence, led by the original post's authors, reveals how AI-generated code was utilized in a credential phishing campaign and explains detection and defense approaches to combat these advanced threats.<!--excerpt_end-->

# AI-Obfuscated Phishing Campaign Detection by Microsoft Threat Intelligence

Microsoft Threat Intelligence has documented the detection and prevention of a credential phishing campaign that leveraged AI-generated code to evade traditional security measures. This case illustrates growing trends in both cyber offense and defense, where AI is central to the development and countering of advanced threats.

## Overview of the Campaign

- **Phishing campaign identified**: In August 2025, a sophisticated phishing operation targeted US-based organizations, using compromised email accounts to distribute disguised SVG attachments.
- **Obfuscated Payload**: Attackers crafted emails to mimic legitimate file-sharing notifications by attaching an SVG file named to look like a PDF. SVG files were chosen for their text-based, scriptable structure, making it easier to embed obfuscated, dynamic content.
- **Social engineering**: Users were redirected to a fake CAPTCHA and then likely to a false sign-in page, designed to harvest credentials.

## AI in Attack and Defense

- **AI-Generated Obfuscation**: Analysis showed that the malicious SVG utilized unusual variable and function naming, a modular code structure, verbose generic comments, and business-term-based encoding—indicators the script was likely generated using a large language model (LLM).
- **Security Copilot’s Role**: Microsoft Security Copilot analyzed the SVG and recognized telltale signs of non-human code structure, such as descriptive naming with hex strings and formulaic obfuscation, confirming AI involvement.
- **Defensive AI**: Microsoft Defender for Office 365 used AI-powered models not reliant on code structure, but on broader behavioral signals:
  - Suspicious sender and BCC usage
  - File type mismatches (SVG presented as PDF)
  - Redirects to malicious infrastructure
  - Browser fingerprinting and session tracking
- **Outcome**: The campaign was detected and blocked before it progressed, demonstrating the effectiveness of AI-augmented defense even against AI-enhanced threats.

## Technical Tactics Observed

- **Invisible Elements**: Attackers used SVG elements (e.g., business dashboard charts, text) set to invisible to disguise the payload.
- **Business-Term Encoding**: Key malicious logic was encoded using sequences of business-related terms, decoded client-side by JavaScript to reconstruct and execute obfuscated behavior, including redirects and fingerprinting.
- **Multi-stage Obfuscation**: The payload mapping, multiple transformation steps, and redundant modules reflected tactics typical of generative AI code.

## Detection and Response Guidance

To help organizations prevent similar attacks, Microsoft highlights these practices:

- Implement recommended settings for [Exchange Online Protection and Defender for Office 365](https://learn.microsoft.com/defender-office-365/recommended-settings-for-eop-and-office365)
- Use [Safe Links](https://learn.microsoft.com/defender-office-365/safe-links-about) to recheck hyperlinks in messages
- Enable [Zero-hour auto purge (ZAP)](https://learn.microsoft.com/defender-office-365/zero-hour-auto-purge) for retroactive removal of malicious mail
- Utilize Microsoft Edge with [Defender SmartScreen](https://learn.microsoft.com/deployedge/microsoft-edge-security-smartscreen) for browser protection
- Turn on [cloud-delivered protection](https://learn.microsoft.com/defender-endpoint/enable-cloud-protection-microsoft-defender-antivirus) in Defender Antivirus
- [Increase Entra security](https://learn.microsoft.com/entra/fundamentals/configure-security) and [adopt phishing-resistant authentication](https://learn.microsoft.com/entra/identity/authentication/concept-authentication-methods)
- Deploy [Conditional Access authentication strength](https://learn.microsoft.com/entra/identity/authentication/concept-authentication-strengths) with Entra ID

## Security Operations and Detection Analytics

- **Microsoft Defender XDR** provides integrated detection, prevention, investigation, and response. Threat Explorer and incident investigation tools enable rapid identification and mitigation.
- **Microsoft Security Copilot** can be used to automate investigation and hunting processes, with prebuilt promptbooks available for deeper analysis.
- **Microsoft Sentinel** analytics (ASIM mapping) enable domain and indicator of compromise (IOC) detection across first-party and third-party data.

## Indicators of Compromise (IOCs)

- **Domain**: kmnl.cpfcenters.de (phishing infrastructure)
- **File name**: 23mb – PDF- 6 Pages.svg

## Best Practice & Resources

- Review Microsoft's [security blog](https://www.microsoft.com/en-us/security/blog) for further research and updates
- Follow Microsoft Threat Intelligence on [LinkedIn](https://www.linkedin.com/showcase/microsoft-threat-intelligence) and [X (Twitter)](https://x.com/MsftSecIntel)
- Listen to the [Microsoft Threat Intelligence podcast](https://thecyberwire.com/podcasts/microsoft-threat-intelligence) for in-depth discussions

## Conclusion

This campaign demonstrates the evolving use of AI in phishing and highlights why defenders need to stay ahead with AI-powered protection. While AI can enhance attacker sophistication, Microsoft’s multi-layered AI-based defense means such threats can be recognized and blocked effectively. Organizations are encouraged to implement the latest detection, authentication, and response tools to counter emerging AI-driven threats.

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/09/24/ai-vs-ai-detecting-an-ai-obfuscated-phishing-campaign/)
