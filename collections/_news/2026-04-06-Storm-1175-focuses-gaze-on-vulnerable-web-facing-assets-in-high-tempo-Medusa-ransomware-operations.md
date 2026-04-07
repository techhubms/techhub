---
section_names:
- security
title: Storm-1175 focuses gaze on vulnerable web-facing assets in high-tempo Medusa ransomware operations
date: 2026-04-06 16:00:00 +00:00
author: Microsoft Threat Intelligence
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/06/storm-1175-focuses-gaze-on-vulnerable-web-facing-assets-in-high-tempo-medusa-ransomware-operations/
tags:
- Attack Surface Reduction (asr) Rules
- Credential Guard
- CVE 21529
- CVE 41080
- CVE 41082
- Data Exfiltration
- DMZ
- Exploit Chaining
- Group Policy (gpo)
- Human Operated Ransomware
- Impacket
- Initial Access
- LOLBins
- LSASS Credential Dumping
- Medusa Ransomware
- Microsoft Defender Antivirus
- Microsoft Defender External Attack Surface Management
- Microsoft Defender For Endpoint
- Microsoft Defender XDR
- Microsoft Exchange Server
- Mimikatz
- N Day Vulnerabilities
- News
- NTDS.dit
- OWASSRF
- PowerShell
- PsExec
- Ransomware
- Rclone
- RDP
- Remote Code Execution (rce)
- Reverse Proxy
- SAM Hive
- Security
- Storm 1175
- Tamper Protection
- WDigest
- Web Application Firewall (waf)
- Web Facing Assets
- Windows Firewall
- Zero Day Exploits
feed_name: Microsoft Security Blog
primary_section: security
---

Microsoft Threat Intelligence breaks down how Storm-1175 runs fast-moving Medusa ransomware intrusions by exploiting newly disclosed vulnerabilities in internet-facing systems, then quickly moving through persistence, lateral movement, credential theft, defense evasion, exfiltration, and encryption—along with practical Defender-focused mitigations.<!--excerpt_end-->

# Storm-1175 focuses gaze on vulnerable web-facing assets in high-tempo Medusa ransomware operations

The financially motivated cybercriminal actor tracked by Microsoft Threat Intelligence as **Storm-1175** runs high-velocity ransomware operations that weaponize recently disclosed vulnerabilities (“**N-days**”)—and in some cases **zero-days**—against **web-facing systems**.

After gaining initial access, Storm-1175 often moves rapidly to **data exfiltration** and deployment of **Medusa ransomware** (payload referenced as *Gaze.exe*), sometimes **within 24 hours**.

Microsoft reports recent heavy impacts on **healthcare**, plus **education, professional services, and finance** organizations in **Australia, the UK, and the US**.

## Storm-1175’s rapid attack chain: from initial access to impact

### Exploitation of vulnerable web-facing assets

Since 2023, Microsoft observed Storm-1175 exploiting **16+ vulnerabilities**, including:

- [CVE-2023-21529](https://nvd.nist.gov/vuln/detail/CVE-2023-21529) (Microsoft Exchange)
- [CVE-2023-27351](https://nvd.nist.gov/vuln/detail/CVE-2023-27351) and [CVE-2023-27350](https://nvd.nist.gov/vuln/detail/CVE-2023-27350) (Papercut)
- [CVE-2023-46805](https://nvd.nist.gov/vuln/detail/CVE-2023-46805) and [CVE-2024-21887](https://nvd.nist.gov/vuln/detail/CVE-2024-21887) (Ivanti Connect Secure / Policy Secure)
- [CVE-2024-1709](https://nvd.nist.gov/vuln/detail/CVE-2024-1709) and [CVE-2024-1708](https://nvd.nist.gov/vuln/detail/CVE-2024-1708) (ConnectWise ScreenConnect)
- [CVE-2024-27198](https://nvd.nist.gov/vuln/detail/CVE-2024-27198) and [CVE-2024-27199](https://nvd.nist.gov/vuln/detail/CVE-2024-27199) (JetBrains TeamCity)
- [CVE-2024-57726](https://nvd.nist.gov/vuln/detail/CVE-2024-57726), [CVE-2024-57727](https://nvd.nist.gov/vuln/detail/CVE-2024-57727), and [CVE-2024-57728](https://nvd.nist.gov/vuln/detail/CVE-2024-57728) (SimpleHelp)
- [CVE-2025-31161](https://nvd.nist.gov/vuln/detail/CVE-2025-31161) (CrushFTP)
- [CVE-2025-10035](https://nvd.nist.gov/vuln/detail/CVE-2025-10035) (GoAnywhere MFT)
- [CVE-2025-52691](https://nvd.nist.gov/vuln/detail/CVE-2025-52691) and [CVE-2026-23760](https://nvd.nist.gov/vuln/detail/CVE-2026-23760) (SmarterMail)
- [CVE-2026-1731](https://nvd.nist.gov/vuln/detail/CVE-2026-1731) (BeyondTrust)

Storm-1175 “rotates” exploits quickly in the gap between **disclosure** and **patch availability/adoption**. Microsoft gives an example where the actor weaponized a SAP NetWeaver issue within about a day.

Microsoft also observed exploit chaining for **RCE** on on-premises Exchange in July 2023 (dubbed “OWASSRF” by public researchers):

- [CVE-2022-41080](https://nvd.nist.gov/vuln/detail/CVE-2022-41080): initial access by exposing Exchange PowerShell via OWA
- [CVE-2022-41082](https://nvd.nist.gov/vuln/detail/CVE-2022-41082): follow-on RCE

Microsoft also notes targeting of **Linux systems** (e.g., Oracle WebLogic) and multiple **zero-day** uses, including:

- [CVE-2026-23760](https://nvd.nist.gov/vuln/detail/CVE-2026-23760) (SmarterMail), exploited the week before public disclosure
- [CVE-2025-10035](https://nvd.nist.gov/vuln/detail/CVE-2025-10035) (GoAnywhere MFT), also exploited about a week before disclosure

For managing exposure, Microsoft calls out **understanding your external footprint**, including using **Microsoft Defender External Attack Surface Management**:

- Microsoft Defender External Attack Surface Management: https://www.microsoft.com/security/business/cloud-security/microsoft-defender-external-attack-surface-management

![Diagram showing timeline of Storm-1175 exploitation of various vulnerabilities, including date of disclosure and date of weaponization](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Storm-1175-exploitation-1.webp)

### Covert persistence and lateral movement

After initial compromise, Storm-1175 often:

- Creates a **web shell** or drops a **remote access payload**
- Creates **new local users** and adds them to the **administrators** group

![Screenshot of code for creating new user account and adding as administrator](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Storm-1175-figure3.webp)

For reconnaissance and lateral movement, Microsoft observed:

- “Living off the land” tools (LOLBins) including **PowerShell** and **PsExec**
- **Cloudflare tunnels** renamed to mimic legitimate binaries (e.g., *conhost.exe*)
- Lateral movement over **RDP**
- If RDP is blocked, modifying **Windows Firewall** policy to enable it

![Diagram showing the Storm-1175 attack chain from Exploitation to Impact](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Storm-1175-attack-chain.webp)

![Screenshot of code for modifying the firewall and enabling RDP](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Storm-1175-figure4.webp)

#### RMM tool usage

Microsoft reports heavy use of **remote monitoring and management (RMM)** tooling during post-compromise activity, including:

- Atera RMM
- Level RMM
- N-able
- DWAgent
- MeshAgent
- ConnectWise ScreenConnect
- AnyDesk
- SimpleHelp

Microsoft also observed the use of **PDQ Deployer** to silently install payloads and deploy ransomware across the environment.

#### Impacket

Storm-1175 used **Impacket** for lateral movement and Microsoft points to Defender protections including the ASR rule:

- Block process creations originating from PSExec and WMI commands: https://learn.microsoft.com/defender-endpoint/attack-surface-reduction-rules-reference#block-process-creations-originating-from-psexec-and-wmi-commands

Microsoft also links broader lateral movement guidance:

- Protecting lateral movement pathways: https://www.microsoft.com/en-us/security/blog/2022/10/26/how-to-prevent-lateral-movement-attacks-using-microsoft-365-defender/

### Credential theft

Microsoft observed credential theft including:

- **LSASS** dumping (via Impacket) and **Mimikatz** (in 2025)
- Enabling **WDigest** caching via the *UseLogonCredential* registry entry
- Using Task Manager to dump LSASS

Relevant Microsoft guidance:

- ASR: Block credential stealing from LSASS: https://learn.microsoft.com/microsoft-365/security/defender-endpoint/attack-surface-reduction-rules-reference?ocid=magicti_ta_learndoc#block-credential-stealing-from-the-windows-local-security-authority-subsystem

Microsoft also describes credential access after pivoting to domain infrastructure:

- Accessing **NTDS.dit** (Active Directory database)
- Accessing the **SAM** hive

Microsoft notes a script used to recover passwords from **Veeam** backup software in some cases.

### Security tampering for ransomware delivery

Microsoft reports Storm-1175 tampering with defenses, including:

- Modifying **Microsoft Defender Antivirus** registry settings
- Adding **C:\** to AV exclusion paths using encoded PowerShell

Microsoft mitigation recommendations include:

- Tamper protection: https://learn.microsoft.com/microsoft-365/security/defender-endpoint/prevent-changes-to-security-settings-with-tamper-protection?ocid=magicti_ta_learndoc
- DisableLocalAdminMerge (via Intune / Defender for Endpoint Security Configuration): https://learn.microsoft.com/microsoft-365/security/defender-endpoint/manage-tamper-protection-intune?ocid=magicti_ta_learndoc#tamper-protection-for-antivirus-exclusions

### Data exfiltration and ransomware deployment

Medusa is described as a **ransomware-as-a-service (RaaS)** model supporting **double extortion**. Microsoft observed:

- **Bandizip** used to collect files
- **Rclone** used for data exfiltration (including continuous sync)
- **PDQ Deployer** used to run *RunFileCopy.cmd* and deliver ransomware
- In some cases, ransomware deployed via **Group Policy** updates

Microsoft background:

- Ransomware as a service overview: https://www.microsoft.com/en-us/security/blog/2022/05/09/ransomware-as-a-service-understanding-the-cybercrime-gig-economy-and-how-to-protect-yourself/

## Mitigation and protection guidance (Microsoft recommendations)

Microsoft’s main guidance points include:

- Use **Microsoft Defender External Attack Surface Management** to inventory and reduce exposed perimeter assets.
- Isolate web-facing systems behind a secure network boundary and access via **VPN** where feasible.
- When public exposure is required, place systems behind:
  - Azure Web Application Firewall (WAF): https://azure.microsoft.com/products/web-application-firewall/
  - Reverse proxy: https://microsoft.github.io/reverse-proxy/articles/getting-started.html
  - DMZ reference architecture: https://learn.microsoft.com/azure/architecture/reference-architectures/dmz/secure-vnet-dmz
- Follow Microsoft ransomware defense guidance: https://www.microsoft.com/en-us/security/blog/2022/05/09/ransomware-as-a-service-understanding-the-cybercrime-gig-economy-and-how-to-protect-yourself/#defending-against-ransomware?ocid=magicti_ta_blog
- Enable **Credential Guard** and ensure it’s enabled before domain join / first domain sign-in where possible:
  - How it works: https://learn.microsoft.com/en-us/windows/security/identity-protection/credential-guard/how-it-works
  - Configure (GPO/registry/Intune): https://learn.microsoft.com/en-us/windows/security/identity-protection/credential-guard/configure?tabs=intune#enable-credential-guard
- Turn on tenant-wide **tamper protection** and configure Defender AV always-on protections:
  - Configure real-time protection: https://learn.microsoft.com/microsoft-365/security/defender-endpoint/configure-real-time-protection-microsoft-defender-antivirus?ocid=magicti_ta_learndoc
  - Troubleshooting mode: https://learn.microsoft.com/microsoft-365/security/defender-endpoint/enable-troubleshooting-mode?ocid=magicti_ta_learndoc
- For RMM tools:
  - Enforce security settings (including **MFA**) for approved RMM.
  - Investigate and reset credentials if unapproved RMM installations are found.
- Configure **automatic attack disruption** in Microsoft Defender XDR:
  - https://learn.microsoft.com/defender-xdr/configure-attack-disruption
- Enable relevant **Attack Surface Reduction (ASR) rules**, including:
  - Block credential stealing from LSASS: https://docs.microsoft.com/microsoft-365/security/defender-endpoint/attack-surface-reduction#block-credential-stealing-from-the-windows-local-security-authority-subsystem
  - Block execution of potentially obfuscated scripts: https://learn.microsoft.com/en-us/microsoft-365/security/defender-endpoint/attack-surface-reduction-rules-reference?view=o365-worldwide#block-execution-of-potentially-obfuscated-scripts
  - Block webshell creation for servers: https://learn.microsoft.com/en-us/defender-endpoint/attack-surface-reduction-rules-reference?view=o365-worldwide#block-webshell-creation-for-servers
  - Block process creations originating from PSExec and WMI commands: https://learn.microsoft.com/en-us/defender-endpoint/attack-surface-reduction-rules-reference?view=o365-worldwide#block-process-creations-originating-from-psexec-and-wmi-commands
  - Block use of copied or impersonated system tools: https://learn.microsoft.com/en-us/defender-endpoint/attack-surface-reduction-rules-reference?view=o365-worldwide#block-use-of-copied-or-impersonated-system-tools
  - Use advanced protection against ransomware: https://learn.microsoft.com/en-us/defender-endpoint/attack-surface-reduction-rules-reference?view=o365-worldwide#use-advanced-protection-against-ransomware

## Microsoft Defender detections (high-level summary)

Microsoft includes a mapping of observed activity to Defender coverage across stages such as:

- Initial access (exploitation of vulnerable web apps)
- Persistence and privilege escalation (suspicious account creation, local admin changes)
- Credential theft (LSASS dumping, NTDS.dit access)
- Lateral movement (RMM tooling, PsExec/PowerShell)
- Exfiltration (Rclone)
- Defense evasion (Defender tampering)
- Impact (Medusa ransomware deployment)

## Microsoft Security Copilot

Microsoft notes **Microsoft Security Copilot** is embedded into **Microsoft Defender** and can support incident summarization, analysis, guided response, hunting queries, and reporting.

- Microsoft Security Copilot: https://www.microsoft.com/en-us/security/business/ai-machine-learning/microsoft-security-copilot
- Embedded in Defender: https://learn.microsoft.com/defender-xdr/security-copilot-in-microsoft-365-defender

They also reference deploying Security Copilot **agents**:

- Deploy AI agents: https://learn.microsoft.com/defender-xdr/security-copilot-agents-defender
- Agents overview: https://learn.microsoft.com/copilot/security/agents-overview
- Threat Intelligence Briefing agent: https://learn.microsoft.com/defender-xdr/threat-intel-briefing-agent-defender
- Phishing Triage agent: https://learn.microsoft.com/defender-xdr/phishing-triage-agent
- Threat Hunting agent: https://learn.microsoft.com/defender-xdr/advanced-hunting-security-copilot-threat-hunting-agent
- Dynamic Threat Detection agent: https://learn.microsoft.com/defender-xdr/dynamic-threat-detection-agent

Security Copilot is also available as a standalone experience:

- Standalone experiences: https://learn.microsoft.com/en-us/copilot/security/experiences-security-copilot

Microsoft also calls out developer scenarios:

- Developer/custom agent overview: https://learn.microsoft.com/copilot/security/developer/custom-agent-overview

## Threat intelligence reports (Defender portal)

Microsoft links Defender portal threat analytics profiles (license required):

- Actor profile: Storm-1175: https://security.microsoft.com/threatanalytics3/5db46cbf-9bd3-4f63-83d5-bded91408025/overview?
- Tool profile: Medusa ransomware: https://security.microsoft.com/threatanalytics3/adb12f14-423e-4d0f-9865-dea310335621/overview?
- Threat overview: Human-operated ransomware: https://security.microsoft.com/threatanalytics3/b42038f7-a361-40c0-bcaf-5dbb63f33dc3/overview?

## Indicators of compromise (IOCs)

Microsoft provides IOCs from 2026 activity including hashes and IPs, such as:

- SHA-256: `0cefeb6210b7103fd32b996beff518c9b6e1691a97bb1cda7f5fb57905c4be96` (*Gaze.exe*, Medusa ransomware)
- SHA-256: `9632d7e4a87ec12fdd05ed3532f7564526016b78972b2cd49a610354d672523c` (*lsp.exe*, Rclone; note Microsoft says this hash has been seen in other intrusions since 2024)
- SHA-256: `e57ba1a4e323094ca9d747bfb3304bd12f3ea3be5e2ee785a3e656c3ab1e8086` (*main.exe*, SimpleHelp)
- SHA-256: `5ba7de7d5115789b952d9b1c6cff440c9128f438de933ff9044a68fff8496d19` (*moon.exe*, SimpleHelp)
- IP: `185.135.86[.]149` (SimpleHelp C2)
- IP: `134.195.91[.]224` (SimpleHelp C2)
- IP: `85.155.186[.]121` (SimpleHelp C2)

## References

- https://labs.watchtowr.com/attackers-with-decompilers-strike-again-smartertools-smartermail-wt-2026-0001-auth-bypass/
- https://www.crowdstrike.com/en-us/blog/owassrf-exploit-analysis-and-recommendations/

## Learn more

- Microsoft Threat Intelligence Blog: https://aka.ms/threatintelblog
- LinkedIn: https://www.linkedin.com/showcase/microsoft-threat-intelligence
- X (formerly Twitter): https://x.com/MsftSecIntel
- Bluesky: https://bsky.app/profile/threatintel.microsoft.com
- Microsoft Threat Intelligence podcast: https://thecyberwire.com/podcasts/microsoft-threat-intelligence


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/06/storm-1175-focuses-gaze-on-vulnerable-web-facing-assets-in-high-tempo-medusa-ransomware-operations/)

