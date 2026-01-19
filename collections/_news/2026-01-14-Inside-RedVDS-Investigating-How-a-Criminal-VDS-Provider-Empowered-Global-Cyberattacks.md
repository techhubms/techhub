---
layout: post
title: 'Inside RedVDS: Investigating How a Criminal VDS Provider Empowered Global Cyberattacks'
author: Microsoft Threat Intelligence
canonical_url: https://www.microsoft.com/en-us/security/blog/2026/01/14/inside-redvds-how-a-single-virtual-desktop-provider-fueled-worldwide-cybercriminal-operations/
viewing_mode: external
feed_name: Microsoft Security Blog
feed_url: https://www.microsoft.com/en-us/security/blog/feed/
date: 2026-01-14 15:03:31 +00:00
permalink: /azure/news/Inside-RedVDS-Investigating-How-a-Criminal-VDS-Provider-Empowered-Global-Cyberattacks
tags:
- Anti Phishing
- Business Email Compromise
- Cloud Security
- Credential Theft
- Cybercrime
- Defender For Office 365
- Homoglyph Domains
- Indicators Of Compromise
- MFA
- Microsoft Defender XDR
- Microsoft Security Copilot
- Phishing
- Power Automate
- RedVDS
- Remote Desktop
- Security Monitoring
- Storm 2470
- Threat Intelligence
- Virtual Dedicated Server
section_names:
- azure
- security
---
Microsoft Threat Intelligence, led by its Digital Crimes Unit, exposes the RedVDS criminal infrastructure that enabled widespread cyberattacks. The report by Microsoft Threat Intelligence offers technical insights, tracked malware tools, and recommended mitigations for security professionals.<!--excerpt_end-->

# Inside RedVDS: Investigating How a Criminal VDS Provider Empowered Global Cyberattacks

Microsoft Threat Intelligence reports on RedVDS—a virtual desktop service provider that enabled various cybercriminal campaigns such as business email compromise (BEC), mass phishing, account takeovers, and financial fraud. The investigation shows how RedVDS was used globally by threat actors to target sectors like legal, construction, healthcare, education, and more, exploiting the service’s low-cost, anonymized cloud servers.

## RedVDS Infrastructure and Modus Operandi

- **RedVDS** sold unlicensed Windows-based RDP servers on a marketplace, allowing threat actors unrestricted, anonymous access via cryptocurrency.
- Investigations revealed a single Windows Server 2022 image was repeatedly cloned, creating hosts with identical fingerprints (e.g., WIN-BUNS25TD77J).
- Provisioning was automated using QEMU and VirtIO drivers; servers could be spun up near-instantly and spread across global data centers, complicating attribution and defense.
- Tools observed included mass mailers (SuperMailer, UltraMailer), email address harvesters, privacy browsers (Waterfox, Avast Secure), VPN clients, remote admin utilities (AnyDesk), and scripting environments (Python, Power Automate attempts).

## Attack Chain Facilitated by RedVDS

1. **Reconnaissance**: Attackers gathered organizational intelligence, tested tools and automation (e.g., Microsoft Power Automate) to optimize delivery of phishing emails.
2. **Resource Development**: Phishing infrastructure assembled on RedVDS, including mass mailers, phishing kits, and homoglyph domain registration for impersonation.
3. **Delivery/Compromise**: Bulk phishing and password spray attacks targeted victims, attempting credential theft and BEC via spoofed emails and brand impersonation.
4. **Account Takeover**: Harvested credentials, replay tokens/cookies used for mailbox compromise, followed by thread monitoring and lateral movement.
5. **Impersonation & Social Engineering**: Threat actors inserted themselves in active conversations, sent fraudulent invoices, and diverted payments to attacker-controlled accounts.
6. **Fraudulent Payments**: Victim organizations were tricked into transferring funds—making incident response and recovery challenging due to laundering networks.

## Tactics, Techniques, and Tools Observed

| Category               | Examples                                      | Purpose                                             |
|------------------------|-----------------------------------------------|-----------------------------------------------------|
| Mass mailing           | SuperMailer, UltraMailer, BlueMail            | Phishing and email campaign delivery                |
| Email Harvesting       | Sky Email Extractor, Email Sorter Pro         | Victim list building and hygiene                    |
| Privacy & VPN          | Waterfox, Avast Secure Browser, NordVPN       | OPSEC, anonymity, geolocation spoofing              |
| Remote Admin           | AnyDesk                                       | Multi-host admin, sharing criminal server access    |
| Automation/Scripting   | Python, Power Automate, ChatGPT, OpenAI tools | Scripting, language optimization for lures          |

AI writing tools like Microsoft Copilot and OpenAI’s ChatGPT were also seen, aiding non-English-speaking actors in producing credible phishing emails.

## Real-World Attack Examples

- **Mass Phishing**: Abuse of Microsoft 365 tenant registrations; mass mailing tools to distribute lures; use of privacy browsers and secure communication apps.
- **Password Spray**: Attacks leveraging RedVDS servers and tools to automate credential guessing.
- **Spoofed Phishing**: Exploited Microsoft Exchange configurations; spoofing internal communications.
- **BEC/Account Takeover**: Imitation and hijacking of legitimate mailbox conversations; uploading fake documents to SharePoint; exfiltration of banking info and invoices.

## Defense Recommendations

Microsoft offers these steps for mitigating threats tied to VDS-enabled criminal infrastructure:

### Anti-Phishing

- Review and apply [Microsoft Defender for Office 365 recommendations](https://learn.microsoft.com/defender-office-365/recommended-settings-for-eop-and-office365?ocid=magicti_ta_learndoc).
- Conduct user training and phishing simulations via [Attack Simulation Training](https://learn.microsoft.com/defender-office-365/attack-simulation-training-get-started?ocid=magicti_ta_learndoc).
- Leverage [Safe Links](https://learn.microsoft.com/defender-office-365/safe-links-about#safe-links-settings-for-email-messages?ocid=magicti_ta_learndoc) and enforce secure usage of Microsoft Teams, Exchange, and SharePoint.

### Credential Hardening

- Adopt passwordless authentication and MFA using [Microsoft Authenticator](https://learn.microsoft.com/entra/identity/authentication/how-to-enable-authenticator-passkey?ocid=magicti_ta_learndoc).
- Apply [Conditional Access](https://learn.microsoft.com/entra/identity/conditional-access/policy-admin-phish-resistant-mfa#create-a-conditional-access-policy?ocid=magicti_ta_learndoc) and limit registration to trusted devices/locations.

### BEC Mitigation

- Use [Privileged Identity Management](https://learn.microsoft.com/azure/active-directory/privileged-identity-management/pim-configure?ocid=magicti_ta_learndoc) to separate privilege/account roles.
- Educate users on [data security best practices](https://learn.microsoft.com/en-us/azure/security/fundamentals/data-encryption-best-practices).
- Enforce DMARC, [Spoof Intelligence](https://learn.microsoft.com/defender-office-365/anti-spoofing-spoof-intelligence?ocid=magicti_ta_learndoc), and robust email configuration.

### Microsoft Defender XDR and Security Copilot

- [Microsoft Defender XDR](https://learn.microsoft.com/en-us/defender-xdr/security-copilot-in-microsoft-365-defender) detects post-compromise activity (malicious mailbox rules, risky sign-ins, password spraying, etc.).
- [Security Copilot](https://learn.microsoft.com/copilot/security/prompting-security-copilot#create-your-own-prompts) offers custom prompts and promptbooks for automated investigation and incident response.

### Threat Intelligence

- Threat analytics and detailed technical IOCs (host names, domains, IPs) are accessible via Microsoft Defender portals.
- Additional analysis and learning resources are linked through the [Microsoft Threat Intelligence Blog](https://aka.ms/threatintelblog) and related podcasts.

## Indicators of Compromise

| Indicator               | Type        | Description                            |
|-------------------------|------------|----------------------------------------|
| Redvds[.]com            | Domain     | Main RedVDS provider site              |
| Redvds[.]pro            | Domain     | Backup site                            |
| redvdspanel[.]space     | Domain     | Sub-panel                              |
| hxxps://rd.redvds[.]com | URL        | RedVDS dashboard                       |
| WIN-BUNS25TD77J         | Host name  | Cloned Windows VM host fingerprint     |

---

## About the Author

This research was prepared by the Microsoft Threat Intelligence team, sharing their expertise in large-scale incident response, malicious infrastructure investigation, and cyberdefense strategy.

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2026/01/14/inside-redvds-how-a-single-virtual-desktop-provider-fueled-worldwide-cybercriminal-operations/)
