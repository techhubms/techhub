---
tags:
- Advanced Hunting
- Adversary in The Middle
- AI
- ASIM
- Company News
- Conditional Access
- Credential Theft
- Datto
- Exchange Online Protection (eop)
- Indicators Of Compromise (iocs)
- MFA Bypass
- Microsoft 365 Sign in Spoofing
- Microsoft Authenticator
- Microsoft Defender For Endpoint
- Microsoft Defender For Office 365
- Microsoft Defender SmartScreen
- Microsoft Defender XDR
- Microsoft Security Copilot
- Microsoft Sentinel
- Microsoft Threat Intelligence
- Network Protection
- News
- OneDrive Abuse
- OneNote Phishing
- Passkeys
- Phishing
- Phishing as A Service
- QR Code Phishing
- Remote Monitoring And Management (rmm)
- Safe Links
- ScreenConnect
- Security
- SHA 256
- SimpleHelp
- Social Engineering
- Tax Season Campaigns
- Threat Analytics
- Zero Hour Auto Purge (zap)
date: 2026-03-19 15:10:37 +00:00
title: 'When tax season becomes cyberattack season: beware these lures'
author: stclarke
feed_name: Microsoft News
section_names:
- ai
- security
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/19/when-tax-season-becomes-cyberattack-season-phishing-and-malware-campaigns-using-tax-related-lures/
primary_section: ai
---

stclarke summarizes Microsoft Threat Intelligence findings on tax-themed phishing and malware campaigns, then walks through practical mitigations using Microsoft Defender, Entra/Conditional Access controls, and hunting guidance in Defender XDR and Microsoft Sentinel (including example KQL queries and IOCs).<!--excerpt_end-->

# When tax season becomes cyberattack season: beware these lures

During tax season, attackers take advantage of time-sensitive, familiar emails (refund notices, payroll forms, filing reminders, requests from tax professionals) to get targets to open attachments, scan QR codes, or follow multi-step link chains.

Microsoft Threat Intelligence reports an annual uptick in tax-themed campaigns as April 15 approaches in the United States, and describes recent campaigns designed to harvest credentials or deliver malware.

## What Microsoft is seeing this tax season

In recent months, Microsoft Threat Intelligence identified campaigns using lures around W-2s, tax forms, or themes such as:

- Government tax agencies
- Tax services firms
- Financial institutions

Some campaigns target individuals for personal/financial theft; others focus on accountants and other professionals who handle sensitive documents and regularly receive tax-related emails.

Common goals:

- Credential harvesting (including MFA bypass)
- Malware delivery

A continuing trend is the abuse of legitimate remote monitoring and management (RMM) tools to maintain persistence, provide alternative command-and-control, or enable hands-on-keyboard remote desktop-style activity.

Related: [Inside TYCOON 2FA: How a leading AiTM phishing kit operated at scale](https://www.microsoft.com/en-us/security/blog/2026/03/04/inside-tycoon2fa-how-a-leading-aitm-phishing-kit-operated-at-scale/)

## A wide range of tax-themed campaigns

### CPA lures leading to the Energy365 phishing kit

- **Timeframe**: Early February 2026
- **Theme**: CPA/tax lure used throughout the chain
- **Notable characteristics**:
  - Highly customized lures (more specific than typical “generic” kits)
  - Multiple file formats (Excel and OneNote)
  - Use of legitimate infrastructure (OneDrive)
  - Multiple rounds of user interaction to complicate automated/reputation-based detection

**Observed delivery chain (Feb 5–6):**

- Emails with subject: **“See Tax file”**
- Targeted industries: financial services, education, IT, insurance, healthcare (primarily US)
- Attachment: `*[Accountant’s name] CPA.xlsx*` (using a real accountant’s name)
- Excel contained a **“REVIEW DOCUMENTS”** button that linked to a **OneNote** file hosted on **OneDrive**
- The OneNote file contained a link to a malicious landing page hosting the **Energy365 PhaaS kit** to steal credentials (email/password)

![Figure 1. OneNote lure with Microsoft logo, link, and impersonated CPA branding](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Fig1.webp)

### QR code and W-2 lure leading to the SneakyLog phishing kit

- **Date observed**: February 10, 2026
- **Scale**: ~100 organizations (manufacturing, retail, healthcare; primarily US)
- Subject: **“2025 Employee Tax Docs”**
- Attachment: `2025_Employee_W-2  .docx`

**Technique:**

- Document content referenced Form W-2 and included a **QR code** to a phishing page.
- Attachments were **personalized** per recipient (recipient name in the doc; recipient email embedded in the QR URL).

**Phishing platform:**

- **SneakyLog** PhaaS (also known as **Kratos**, present since at least early 2025)
- Mimicked the **Microsoft 365 sign-in page** to steal credentials
- Capable of harvesting credentials and 2FA

![Figure 2. Personalized W-2 lure with QR code to phishing page](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Fig2-1.webp)

### Form 1099-themed phishing delivering ScreenConnect

- **Timeframe**: January–February 2026
- Observed tax-themed domains registered (e.g., including “tax” / “1099form”, and impersonating legitimate companies)

**Campaign observed Feb 8–10:**

- Email subjects included:
  - “Your Account Now Includes Updated Tax Forms [RF] 1234”
  - “Your Form 1099-R is ready – [RF] 12123123”
- Body: “2025 Tax Forms is ready”
- Button: **“View Tax Forms”**

**Redirect chain:**

- `taxationstatments2025[.]com` → `tax-statments2025[.]com`
- Served malware executable: `1099-FR2025.exe`

**Payload:**

- **ScreenConnect** (ConnectWise) RMM tool
- Certificate later revoked due to abuse
- Legitimate tool functionality used as a de facto RAT for remote control and follow-on activity

![Figure 3. Fidelity-themed lure email](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Fig3.jpg)

![Figure 4. Landing page leading to EXE download](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Fig4-1.webp)

### IRS and cryptocurrency-themed phishing delivering SimpleHelp

**Evasion method:**

- No clickable link; recipients were instructed to **copy/paste** a URL from the email body.

**Timing/scale:**

- Sent Feb 23 and 27
- Several thousand emails, recipients exclusively in the US
- Targeted many industries; bulk in higher education

**Masquerading details:**

- Subject: “IR-2026-216”
- Used Eventbrite sender infrastructure, e.g.:
  - `IRS US<noreply@campaign[.]eventbrite[.]com>`
  - `IRS GOV<noreply@campaign[.]eventbrite[.]com>`
  - `Service<noreply@campaign[.]eventbrite[.]com>`
  - `IRS TAX<noreply@campaign[.]eventbrite[.]com>`
  - `.IRS.GOV<noreply@campaign[.]eventbrite[.]com>`

**Delivery:**

- Domains: `irs-doc[.]com` or `gov-irs216[.]net`
- Download: `IRS-doc.msi`
- Payload varied by day: **ScreenConnect** or **SimpleHelp** (another legitimate RMM tool)

![Figure 5. IRS + cryptocurrency-themed lure](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Fig5-1.webp)

### Campaign targeting CPAs and delivering Datto

A known pattern involves benign conversation starters (requesting tax help, quote, backstory) followed by a malicious link later. Microsoft also observed variants where the malicious link is in the first message.

**Observed March 9 campaign:**

- Scale: ~1,000 emails, US-only
- Targeted: accounting companies plus some adjacent industries (financial services, legal, insurance)
- Subject: “REQUEST FOR PROFESSIONAL TAX FILLING”

**Chain:**

- Link used `carrd[.]co` → “VIEW DOCUMENTS”
- Redirected via URL shortener → `private-adobe-client[.]im`
- Final payload: executable related to **Datto** (legitimate RMM tool abused by attackers)

![Figure 6. CPA-targeted lure email](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Fig6-1.webp)

### IRS-themed campaign targeting accounting professionals and dropping ScreenConnect

- **Date observed**: February 10, 2026
- **Scale**: 29,000+ users across 10,000 organizations
- **Geography**: almost exclusively US (95% of targets)
- Most targeted sectors: financial services (19%), technology/software (18%), retail/consumer goods (15%)
- Recipients skewed toward roles like accountants and tax preparers

**Lure:**

- IRS impersonation; claimed irregular returns filed under recipient’s EFIN
- CTA: download “IRS Transcript Viewer”

![Figure 7. Example phishing email](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Fig7-1.webp)

**Delivery details:**

- Sent through Amazon SES from `edud[.]site`
- Sender display name rotated among 14 IRS-themed identities
- Subjects rotated across 49 variants (examples: “IRS Request Transcript Review”, “CPA Compliance Review”, “Review Requested Compliance”)

**Redirect chain and evasion:**

- Button linked to Amazon SES click-tracking (`awstrack[.]me`) then to `smartvault[.]im` (look-alike domain mimicking SmartVault)
- Used **Cloudflare bot detection/blocking** to block crawlers/sandboxes

**Final behavior:**

- Fake “verification” animation, then “download starting” page
- Downloaded file: `TranscriptViewer5.1.exe`
- Payload: maliciously repackaged **ScreenConnect** enabling remote control, data theft, credential harvesting, and post-exploitation

![Figure 8. Verification and download success pages](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Fig8a.webp)

![Figure 8. Verification and download success pages (variant)](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Fig8b-1.webp)

## How to protect users and organizations

Microsoft recommends these mitigation measures:

- Configure [automatic attack disruption](https://learn.microsoft.com/en-us/defender-xdr/configure-attack-disruption) in **Microsoft Defender XDR**.
- Enforce MFA on all accounts, remove users excluded from MFA, and strictly [require MFA](https://learn.microsoft.com/azure/active-directory/identity-protection/howto-identity-protection-configure-mfa-policy).
- Use [Microsoft Authenticator for passkeys and MFA](https://learn.microsoft.com/en-us/entra/identity/authentication/how-to-enable-authenticator-passkey) and complement MFA with Conditional Access.
- Use Conditional Access to [strengthen privileged accounts with phishing resistant MFA](https://learn.microsoft.com/en-us/entra/identity/conditional-access/policy-admin-phish-resistant-mfa#create-a-conditional-access-policy).
- Enable [Zero-hour auto purge (ZAP)](https://learn.microsoft.com/en-us/defender-office-365/zero-hour-auto-purge) in **Office 365**.
- Configure **Microsoft Defender for Office 365 Safe Links** to [recheck links on click](https://learn.microsoft.com/microsoft-365/security/office-365-security/safe-links-about), complementing EOP anti-spam and anti-malware.
- Use browsers and controls that block malicious sites, e.g. Microsoft Edge and [Microsoft Defender SmartScreen](https://learn.microsoft.com/en-us/deployedge/microsoft-edge-security-smartscreen?ocid=magicti_ta_learndoc).
- Enable [network protection](https://learn.microsoft.com/defender-endpoint/enable-network-protection).

## Microsoft Defender detection and hunting guidance

Microsoft lists Defender coverage mapped to tactics/activities such as:

- **Initial access**: phishing emails (Microsoft Defender for Office 365)
- **Execution**: delivery and suspicious use/installation of RMM tools (Microsoft Defender for Endpoint)

## Microsoft Security Copilot (Defender integration)

Microsoft Security Copilot is embedded in Defender and can help security teams summarize incidents, analyze files/scripts, summarize identities, guide responses, and generate device summaries, hunting queries, and incident reports.

It also supports deploying agents, including:

- [Threat Intelligence Briefing agent](https://learn.microsoft.com/defender-xdr/threat-intel-briefing-agent-defender)
- [Phishing Triage agent](https://learn.microsoft.com/defender-xdr/phishing-triage-agent)
- [Threat Hunting agent](https://learn.microsoft.com/defender-xdr/advanced-hunting-security-copilot-threat-hunting-agent)
- [Dynamic Threat Detection agent](https://learn.microsoft.com/defender-xdr/dynamic-threat-detection-agent)

Copilot is also available as a standalone experience, and includes [developer scenarios](https://learn.microsoft.com/copilot/security/developer/custom-agent-overview) for building/testing/publishing/integrating AI agents and plugins.

## Hunting queries

### Microsoft Defender XDR

#### Find email messages related to known domains

```kusto
EmailUrlInfo
| where UrlDomain has_any (
  "taxationstatments2025.com",
  "irs-doc.com",
  "gov-irs216.net",
  "private-adobe-client.im",
  "edud.site",
  "smartvault.im"
)
```

#### Detect file hash indicators in email data

```kusto
let File_Hashes_SHA256 = dynamic([
  "45b6b4db1be6698c29ffde9daeb8ffaa344b687d3badded2f8c68c922cdce6e0",
  "d422f6f5310af1e72f6113a2a592916f58e3871c58d0e46f058d4b669a3a0fd8"
]);
DeviceFileEvents
| where SHA256 has_any (File_Hashes_SHA256)
```

### Microsoft Sentinel

Microsoft Sentinel customers can use TI Mapping analytics (rules prefixed with “TI map”). If not deployed, install the Threat Intelligence solution from the [Microsoft Sentinel Content Hub](https://learn.microsoft.com/azure/sentinel/sentinel-solutions-deploy).

The queries below use [Sentinel ASIM functions](https://learn.microsoft.com/azure/sentinel/normalization) to hunt across supported data sources.

#### Detect network IP and domain IOCs using ASIM

```kusto
//IP list and domain list- _Im_NetworkSession
let lookback = 30d;
let ioc_ip_addr = dynamic([]);
let ioc_domains = dynamic([
  "taxationstatments2025.com",
  "irs-doc.com",
  "gov-irs216.net",
  "private-adobe-client.im"
]);
_Im_NetworkSession(starttime=todatetime(ago(lookback)), endtime=now())
| where DstIpAddr in (ioc_ip_addr) or DstDomain has_any (ioc_domains)
| summarize
    imNWS_mintime=min(TimeGenerated),
    imNWS_maxtime=max(TimeGenerated),
    EventCount=count()
  by SrcIpAddr, DstIpAddr, DstDomain, Dvc, EventProduct, EventVendor
```

#### Detect web session IP and file hash IOCs using ASIM

```kusto
//IP list - _Im_WebSession
let lookback = 30d;
let ioc_ip_addr = dynamic([]);
let ioc_sha_hashes = dynamic([
  "45b6b4db1be6698c29ffde9daeb8ffaa344b687d3badded2f8c68c922cdce6e0"
]);
_Im_WebSession(starttime=todatetime(ago(lookback)), endtime=now())
| where DstIpAddr in (ioc_ip_addr) or FileSHA256 in (ioc_sha_hashes)
| summarize
    imWS_mintime=min(TimeGenerated),
    imWS_maxtime=max(TimeGenerated),
    EventCount=count()
  by SrcIpAddr, DstIpAddr, Url, Dvc, EventProduct, EventVendor
```

#### Detect domain and URL IOCs using ASIM

```kusto
// file hash list - imFileEvent
// Domain list - _Im_WebSession
let ioc_domains = dynamic([
  "taxationstatments2025.com",
  "irs-doc.com",
  "gov-irs216.net",
  "private-adobe-client.im"
]);
_Im_WebSession (url_has_any = ioc_domains)
```

#### Detect file hash IOCs using ASIM

```kusto
// file hash list - imFileEvent
let ioc_sha_hashes = dynamic([
  "45b6b4db1be6698c29ffde9daeb8ffaa344b687d3badded2f8c68c922cdce6e0"
]);
imFileEvent
| where SrcFileSHA256 in (ioc_sha_hashes) or TargetFileSHA256 in (ioc_sha_hashes)
| extend AccountName = tostring(split(User, @'')[1]), AccountNTDomain = tostring(split(User, @'')[0])
| extend AlgorithmType = "SHA256"
```

## Indicators of compromise (IOCs)

| Indicator | Type | Description | First seen | Last seen |
| --- | --- | --- | --- | --- |
| 45b6b4db1be6698c29ffde9daeb8ffaa344b687d3badded2f8c68c922cdce6e0 | SHA-256 | Excel attachment in Energy365 PhaaS campaign | 2026-02-05 | 2026-02-06 |
| taxationstatments2025[.]com | Domain | Fidelity-themed ScreenConnect campaign | 2026-02-08 | 2026-02-10 |
| irs-doc[.]com | Domain | IRS/cryptocurrency-themed SimpleHelp campaign | 2026-02-23 | 2026-02-27 |
| gov-irs216[.]net | Domain | IRS/cryptocurrency-themed SimpleHelp campaign | 2026-02-23 | 2026-02-27 |
| private-adobe-client[.]im | Domain | CPA-targeted campaign delivering Datto | 2026-03-05 | 2026-03-09 |
| d422f6f5310af1e72f6113a2a592916f58e3871c58d0e46f058d4b669a3a0fd8 | SHA-256 | EXE dropped in IRS ScreenConnect campaign | 2026-02-10 | 2026-10 |
| edud[.]site | Domain | Domain hosting sender addresses (IRS ScreenConnect campaign) | 2026-02-10 | 2026-02-10 |
| smartvault[.]im | Domain | Domain hosting malicious content (IRS ScreenConnect campaign) | 2026-02-10 | 2026-02-10 |

## Learn more

- Microsoft Threat Intelligence Blog: https://aka.ms/threatintelblog
- LinkedIn: https://www.linkedin.com/showcase/microsoft-threat-intelligence
- X (formerly Twitter): https://x.com/MsftSecIntel
- Bluesky: https://bsky.app/profile/threatintel.microsoft.com
- Podcast: https://thecyberwire.com/podcasts/microsoft-threat-intelligence


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/19/when-tax-season-becomes-cyberattack-season-phishing-and-malware-campaigns-using-tax-related-lures/)

