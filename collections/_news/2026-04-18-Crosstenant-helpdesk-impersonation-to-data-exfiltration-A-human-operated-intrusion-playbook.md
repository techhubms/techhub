---
title: 'Cross‑tenant helpdesk impersonation to data exfiltration: A human-operated intrusion playbook'
date: 2026-04-18 12:55:45 +00:00
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/18/crosstenant-helpdesk-impersonation-data-exfiltration-human-operated-intrusion-playbook/
tags:
- Advanced Hunting
- Attack Surface Reduction (asr)
- Command And Control (c2)
- Conditional Access
- Cross Tenant Access
- Data Exfiltration
- DLL Side Loading
- Helpdesk Impersonation
- KQL
- Lateral Movement
- Microsoft Defender For Endpoint
- Microsoft Defender For Office 365
- Microsoft Defender XDR
- Microsoft Entra ID
- Microsoft Teams
- Multi Stage Incident Correlation
- Network Protection
- News
- Quick Assist
- Rclone
- Remote Assistance
- Safe Links
- Security
- Social Engineering
- T1566.003
- Windows Defender Application Control (wdac)
- WinRM
- Zero Hour Auto Purge (zap)
feed_name: Microsoft Security Blog
primary_section: security
section_names:
- security
author: Microsoft Defender Security Research Team
---

The Microsoft Defender Security Research Team breaks down a cross-tenant Microsoft Teams helpdesk-impersonation intrusion chain, from Quick Assist remote access through WinRM lateral movement to Rclone-based data exfiltration, with concrete mitigations and Defender XDR hunting queries.<!--excerpt_end-->

# Cross‑tenant helpdesk impersonation to data exfiltration: A human-operated intrusion playbook

Threat actors are initiating cross-tenant Microsoft Teams communications while impersonating IT/helpdesk staff to socially engineer users into granting remote desktop access. Once interactive access is established (often via Quick Assist), attackers blend into routine IT activity by relying on legitimate tools, trusted vendor-signed binaries, and native admin protocols.

A common observed pattern:

- Initial contact in Teams from an external tenant (impersonating helpdesk)
- User-approved remote assistance session (Quick Assist or similar)
- Payload staging and execution via trusted signed apps + attacker-supplied modules (DLL side-loading)
- Credential-backed lateral movement via WinRM toward high-value assets (for example, domain controllers)
- Deployment of commercial remote management tooling (RMM)
- Data staging and exfiltration via tools like Rclone to external cloud storage

Microsoft Defender provides cross-product visibility (identity + endpoint + collaboration) and correlates these signals into unified incidents to help detect and disrupt this user-initiated access pathway.

## Risk to enterprise environments

This intrusion chain abuses enterprise collaboration workflows instead of classic email phishing. Teams has built-in warnings (external-sender labeling, Accept/Block prompts), but the attack succeeds when users bypass warnings and approve remote access actions.

An approved external Teams interaction might enable threat actors to:

- Establish credential-backed interactive system access
- Deploy trusted applications to execute attacker-controlled code
- Pivot toward identity and domain infrastructure using WinRM
- Deploy commercially available remote management tooling
- Stage sensitive business-relevant data for transfer to external cloud infrastructure

![Figure 1: Attack chain](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-38.webp)

## Attack chain overview

### Stage 1: Initial contact via Teams (T1566.003 Spearphishing via Service)

Attackers abuse external collaboration features in Microsoft Teams to start a chat/call from another tenant while impersonating internal support personnel.

Typical lures include:

- “Microsoft Security Update”
- “Spam Filter Update”
- “Account Verification”

The objective remains the same: get the user to ignore external contact flags, launch a remote support session, and approve elevation prompts. Voice phishing (vishing) may be layered in.

Operational note: defenders may see a `ChatCreated` event indicating first contact, followed by suspicious chats/vishing, remote management events, and other activity that can be correlated by account and chat thread in Defender hunting.

Teams security warnings shown to users can include:

- External Accept/Block prompt for first contact
- Higher confidence spam/phish warnings on first contact
- External-tenant warnings
- URL-click warnings
- Safe Links time-of-click protections
- Zero-hour Auto Purge (ZAP) removing messages later flagged as malicious

Reference: Teams security features documentation: https://learn.microsoft.com/en-us/microsoftteams/teams-security-guide

![Figure 2: External Accept/Block screens](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-39.webp)
![Figure 3: Spam or phishing alert](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-40.webp)
![Figure 4: External warnings](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-41.webp)
![Figure 5: URL click warning](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-43.webp)
![Figure 6: Safe Links time-of-click protection warning](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-44.webp)
![Figure 7: ZAP removed malicious message](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-45.webp)

Important operational detail: attackers often avoid sending the URL via Teams and instead navigate to it during the remote session. This makes user education about external flags and first-contact warnings critical.

### Stage 2: Remote assistance foothold

With user consent, the attacker gains interactive device control using remote support tools such as Quick Assist.

Observed process indicators include:

- `QuickAssist.exe`
- Windows elevation prompts via `Consent.exe`

From the user’s perspective: open Quick Assist, enter a short key, then accept prompts and approvals.

![Figure 8: Quick Assist key logs](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-46.webp)
![Figure 9: Quick Assist launch](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-47.webp)

Signal to watch for: a remote-assist process tree quickly followed by `cmd.exe` or PowerShell on the same desktop.

### Stage 3: Interactive reconnaissance and access validation

Within ~30–120 seconds, attackers validate access and gather environment details using bursts of `cmd.exe` and/or PowerShell:

- Check user context and privileges
- Host/OS details, domain affiliation
- Registry queries for build/edition
- Quick network reachability checks for lateral movement paths

On limited-privilege systems (kiosks, VDI, non-corp-joined), attackers may pause and return later or pivot elsewhere.

![Figure 10: Enumeration](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-48.webp)

### Stage 4: Payload placement and trusted application invocation

Attackers stage a small bundle (archive-based or short-lived scripting) and then execute via DLL side-loading using trusted signed applications from non-standard paths (often under `ProgramData`). Examples cited:

- `AcroServicesUpdater2_x64.exe` loading a staged `msi.dll`
- `ADNotificationManager.exe` loading `vcruntime140_1.dll`
- `DlpUserAgent.exe` loading `mpclient.dll`
- `werfault.exe` loading `Faultrep.dll`

This allows attacker-supplied modules to run under a trusted execution context.

![Figure 11: Sample payload](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-49.webp)

### Stage 5: Execution context validation and registry-backed loader state

Attackers perform runtime checks, then write a large encoded value into a user-context registry location as an encrypted configuration container.

A sideloaded intermediary loader decrypts and reconstructs execution/C2 configuration in memory, minimizing file I/O. The post notes alignment with frameworks such as Havoc that externalize encrypted configuration to the registry.

Defender for Endpoint detections may include:

- Unexpected DLL load by trusted application
- Service-path execution outside vendor installation directory
- Execution from user-writable directories like `ProgramData`

Mitigation controls suggested:

- Attack Surface Reduction (ASR) rules
- Windows Defender Application Control (WDAC) to restrict sideloading paths (for example `ProgramData`, `AppData`)

![Figure 12: Representative commands/actions (sanitized)](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-50.webp)

### Stage 6: Command and control

The trusted-signed host process (example: updater-themed `AcroServicesUpdater2_x64.exe`) initiates outbound HTTPS over TCP 443 to attacker-controlled infrastructure.

Indicator: outbound “update-like” HTTPS traffic to unknown/dynamic cloud-backed endpoints rather than known vendor services.

### Stage 7: Internal discovery and lateral movement toward high-value assets

After external comms, the compromised process initiates internal remote management over WinRM (TCP 5985) to other domain-joined systems.

Defender may surface this as multi-device incidents indicating credential-backed lateral movement from a user-context remote session.

Targeting shifts toward identity/domain infrastructure (including domain controllers), preceding deployment of additional access tooling.

### Stage 8: Remote deployment of auxiliary access tooling (Level RMM)

Attackers install commercial remote management tooling across hosts using `msiexec.exe`, creating an alternate control channel independent of the original components.

![Auxiliary access tooling deployment](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-51.webp)

### Stage 9: Data exfiltration

Attackers use Rclone to transfer targeted business documents to external cloud storage. Transfer parameters may include file-type exclusions to reduce size and detection.

Defender may detect this as possible data exfiltration involving uncommon synchronization tooling.

![Data exfiltration via Rclone](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-52.webp)

## Mitigation and protection guidance

Key mitigations and guidance from the post:

- **Microsoft Teams**
  - Review external collaboration policies.
  - Ensure users get clear external sender notifications for cross-tenant contacts.
  - Consider device/identity requirements before allowing remote support sessions.
  - References:
    - https://learn.microsoft.com/en-us/microsoftteams/trusted-organizations-external-meetings-chat
    - https://learn.microsoft.com/en-us/defender-office-365/mdo-support-teams-about

- **Microsoft Defender for Office 365**
  - Enable Safe Links for Teams with time-of-click verification.
  - Ensure Zero-hour Auto Purge (ZAP) is active.
  - Reference: https://learn.microsoft.com/en-us/defender-office-365/safe-links-about

- **Microsoft Defender for Endpoint**
  - Disable/restrict remote management tools to authorized roles.
  - Enable standard ASR rules in block mode.
  - Apply WDAC to prevent DLL sideloading from `ProgramData` / `AppData`.
  - Reference: https://learn.microsoft.com/en-us/defender-endpoint/attack-surface-reduction-rules-reference

- **Microsoft Entra ID**
  - Enforce Conditional Access with MFA and compliant devices for admin roles.
  - Restrict WinRM to authorized management workstations.
  - Monitor for Rclone via hunting/custom alerts.
  - References:
    - https://learn.microsoft.com/en-us/entra/identity/conditional-access/overview
    - https://learn.microsoft.com/en-us/defender-xdr/advanced-hunting-overview
    - https://learn.microsoft.com/en-us/defender-xdr/custom-detections-overview

- **Network controls**
  - Enable network protection to block C2 to poor-reputation/newly registered domains.
  - Alert on registry modifications to ASEP locations by non-installer processes.
  - Reference: https://learn.microsoft.com/en-us/defender-endpoint/network-protection

- **User education**
  - Establish a verbal authentication code/phrase between IT Helpdesk and employees.
  - Train staff on how IT normally contacts them (mediums, identifiers, domains, aliases, phone numbers).
  - Show examples distinguishing internal vs external Teams comms, Accept/Block prompts, and how to verify caller identity.
  - Treat unsolicited external “helpdesk” contact as suspicious.
  - Reference: Disrupting threats targeting Microsoft Teams: https://www.microsoft.com/en-us/security/blog/2025/10/07/disrupting-threats-targeting-microsoft-teams/

## Microsoft protection outcomes (as described)

- **Automatic Attack Disruption**
  - When Defender detects credential-backed WinRM lateral movement following Quick Assist, it can suspend the originating user session and contain the user before domain controller interaction.
  - References:
    - https://learn.microsoft.com/en-us/defender-xdr/automatic-attack-disruption
    - https://learn.microsoft.com/en-us/defender-xdr/configure-attack-disruption

- **Cross-product incident correlation**
  - Teams/MDO, Entra ID, and MDE signals are correlated into unified incidents.
  - Reference: https://learn.microsoft.com/en-us/defender-xdr/incident-queue

- **Threat analytics**
  - Threat Analytics reports include exposure assessments and mitigations; detection logic is continuously updated.
  - Reference: https://learn.microsoft.com/en-us/defender-xdr/threat-analytics

- **Teams external message accept/block**
  - Teams shows message preview and explicit Accept/Block before conversation begins.
  - Reference: https://learn.microsoft.com/en-us/microsoftteams/teams-security-best-practices-for-safer-messaging

- **Security recommendations**
  - Includes guidance such as UAC restrictions to local accounts on network logons, Safe DLL Search Mode, enable Network Protection, disable “Allow Basic authentication” for WinRM client/service.
  - Reference: https://learn.microsoft.com/en-us/defender-vulnerability-management/tvm-security-recommendation

## Microsoft Defender XDR detections (examples listed)

The post maps observed activity to Defender coverage across tactics:

- **Initial access**: suspicious external Teams chat/call; voice phishing; URL click alerts; “Possible initial access from an emerging threat”
- **Execution**: suspicious Quick Assist usage; uncommon remote access software; RMM suspicious activity; AV detections (DllHijack variants, etc.)
- **Lateral movement**: suspicious sign-in activity; hands-on-keyboard multi-device activity
- **Persistence**: suspicious registry modification
- **Defense evasion / privilege escalation**: unexpected DLL loads by trusted apps; AV detections
- **Command and control**: connections to custom network indicators; ransomware-linked emerging threat signals
- **Data exfiltration**: possible data exfiltration; Rclone usage
- **Multi-tactic**: multi-stage incident; remote management event after Teams phishing; Office app running suspicious commands

## Hunting queries (KQL)

The post provides multiple advanced hunting queries for Microsoft Defender XDR to correlate Teams activity with endpoint events, detect suspicious execution patterns, and spot exfiltration.

### A. Teams → RMM correlation

```kusto
let _timeFrame = 30m; // Teams message signal let _teams = MessageEvents | where Timestamp > ago(14d) //| where SenderDisplayName contains "add keyword" // or SenderDisplayName contains "add keyword" | extend Recipient = parse_json(RecipientDetails) | mv-expand Recipient | extend VictimAccountObjectId = tostring(Recipient.RecipientObjectId), VictimRecipientDisplayName = tostring(Recipient.RecipientUserDisplayName) | project TTime = Timestamp, SenderEmailAddress, SenderDisplayName, VictimRecipientDisplayName, VictimAccountObjectId; // RMM launches on endpoint side let _rmm = DeviceProcessEvents | where Timestamp > ago(14d) | where FileName in~ ("QuickAssist.exe", "AnyDesk.exe", "TeamViewer.exe") | extend VictimAccountObjectId = tostring(InitiatingProcessAccountObjectId) | project DeviceName, QTime = Timestamp, RmmTool = FileName, VictimAccountObjectId; _teams | where isnotempty(VictimAccountObjectId) | join kind=inner _rmm on VictimAccountObjectId | where isnotempty(DeviceName) | where QTime between ((TTime) .. (TTime +(_timeFrame))) | project DeviceName, SenderEmailAddress, SenderDisplayName, VictimRecipientDisplayName, VictimAccountObjectId, TTime, QTime, RmmTool | order by QTime desc
```

### B. Execution

```kusto
DeviceProcessEvents | where Timestamp > ago(7d) | where InitiatingProcessFileName =~ "cmd.exe" | where FileName =~ "cmd.exe" | where ProcessCommandLine has_all ("/S /D /c", "\" set /p=\"PK\"", "1>")
```

### C. ZIP → ProgramData service path → signed host sideload

```kusto
let _timeFrame = 10m; let _armOrDevice = DeviceFileEvents | where Timestamp > ago(14d) | where FolderPath has_any ( "C:\\ProgramData\\Adobe\\ARM\\", "C:\\ProgramData\\Microsoft\\DeviceSync\\", "D:\\ProgramData\\Adobe\\ARM\\", "D:\\ProgramData\\Microsoft\\DeviceSync\\") and ActionType in ("FileCreated","FileRenamed") | project DeviceName, First=Timestamp, FileName; let _hostRun = DeviceProcessEvents | where Timestamp > ago(14d) | where FileName in~ ("AcroServicesUpdater2_x64.exe","DlpUserAgent.exe","ADNotificationManager.exe") | project DeviceName, Run=Timestamp, Host=FileName; _armOrDevice | join kind=inner _hostRun on DeviceName | where Run between (First .. (First+(_timeFrame))) | summarize First=min(First), Run=min(Run), Files=make_set(FileName, 10) by DeviceName, Host | order by Run desc
```

### D. PowerShell → high-risk TLD → writes %AppData%/Roaming EXE

```kusto
let _timeFrame = 5m; let _psNet = DeviceNetworkEvents | where Timestamp > ago(14d) | where InitiatingProcessFileName in~ ("powershell.exe","pwsh.exe") | where RemoteUrl matches regex @"(?i)\.(top|xyz|zip|click)$" | project DeviceName, NetTime=Timestamp, RemoteUrl, RemoteIP; let _exeWrite = DeviceFileEvents | where Timestamp > ago(14d) | where FolderPath has @"\AppData\Roaming\" and FileName endswith ".exe" | project DeviceName, WTime=Timestamp, FileName, FolderPath, SHA256; _psNet | join kind=inner _exeWrite on DeviceName | where WTime between (NetTime .. (NetTime+(_timeFrame))) | project DeviceName, NetTime, RemoteUrl, RemoteIP, WTime, FileName, FolderPath, SHA256 | order by WTime desc
```

### E. Registry breadcrumbs / ASEP anomalies

```kusto
DeviceRegistryEvents | where Timestamp > ago(30d) | where RegistryKey has @"\SOFTWARE\Classes\Local Settings\Software\Microsoft" | where RegistryValueName in~ ("UCID","UFID","XJ01","XJ02","UXMP") | project Timestamp, DeviceName, ActionType, RegistryKey, RegistryValueName, PreviousRegistryValueData, InitiatingProcessFileName | order by Timestamp desc
```

### F. Non-browser process → API-Gateway → internal AD protocols

```kusto
let _timeFrame = 10m; let _net1 = DeviceNetworkEvents | where Timestamp > ago(14d) | where RemoteUrl has ".execute-api." | where InitiatingProcessFileName !in~ ("chrome.exe","msedge.exe","firefox.exe") | project DeviceName, Proc=InitiatingProcessFileName, OutTime=Timestamp, RemoteUrl, RemoteIP; let _net2 = DeviceNetworkEvents | where Timestamp > ago(14d) | where RemotePort in (135,389,445,636) | project DeviceName, Proc=InitiatingProcessFileName, InTime=Timestamp, RemoteIP, RemotePort; _net1 | join kind=inner _net2 on DeviceName, Proc | where InTime between (OutTime .. (OutTime+(_timeFrame))) | project DeviceName, Proc, OutTime, RemoteUrl, InTime, RemotePort | order by InTime desc
```

### G. PowerShell history deletion

```kusto
DeviceFileEvents | where Timestamp > ago(14d) | where FileName =~ "ConsoleHost_history.txt" and ActionType == "FileDeleted" | project Timestamp, DeviceName, InitiatingProcessFileName, InitiatingProcessCommandLine, FolderPath | order by Timestamp desc
```

### H. Reconnaissance burst (cmd / PowerShell)

```kusto
DeviceProcessEvents | where Timestamp > ago(14d) | where FileName in~ ("cmd.exe","powershell.exe","pwsh.exe") | where ProcessCommandLine has_any ( "whoami", "whoami /all", "whoami /groups", "whoami /priv", "hostname", "systeminfo", "ver", "wmic os get", "reg query HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "query user", "net user", "nltest", "ipconfig /all", "arp -a", "route print", "dir", "icacls" ) | project Timestamp, DeviceName, FileName, InitiatingProcessFileName, ProcessCommandLine | summarize eventCount = count(), FileNames = make_set(FileName), InitiatingProcessFileNames = make_set(InitiatingProcessFileName), ProcessCommandLines = make_set(ProcessCommandLine, 5) by DeviceName | where eventCount > 2
```

### I. Data exfil

```kusto
DeviceProcessEvents | where Timestamp > ago(2d) | where FileName =~ "rclone.exe" or ProcessVersionInfoOriginalFileName =~ "rclone.exe" | where ProcessCommandLine has_all ("copy ", "--config rclone_uploader.conf", "--transfers 16", "--checkers 16", "--buffer-size 64M", "--max-age=3y", "--exclude *.mdf")
```

### J. Quick Assist–anchored recon (no staging writes within 10 minutes)

```kusto
let _reconWindow = 10m; // common within 1-5 minutes let _stageWindow = 15m; // common 1-2 minutes after recon, or less // Anchor on RMM let _rmm = DeviceProcessEvents | where Timestamp > ago(14d) | where FileName in~ ("QuickAssist.exe", "AnyDesk.exe", "TeamViewer.exe") | project DeviceName, RMMTime=Timestamp; // Recon commands within X minutes of RMM start (targeted list) let _recon = DeviceProcessEvents | where Timestamp > ago(14d) | where FileName in~ ("cmd.exe","powershell.exe","pwsh.exe") | where ProcessCommandLine has_any ( "whoami", "hostname", "systeminfo", "ver", "wmic os get", "reg query HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "query user", "net user", "nltest", "ipconfig /all", "arp -a", "route print", "dir", "icacls" ) | project DeviceName, ReconTime=Timestamp, ReconCmd=ProcessCommandLine, ReconProc=FileName; // Suspect staging writes (ZIP/EXE/DLL) let _staging = DeviceFileEvents | where Timestamp > ago(14d) | where ActionType in ("FileCreated","FileRenamed") | where FileName matches regex @"(?i).*\\.(zip|exe|dll)$" | project DeviceName, STime=Timestamp, StageFile=FileName, StagePath=FolderPath; // Correlate RMM + recon, then exclude cases with staging writes in the next X minutes let _rmmRecon = _rmm | join kind=inner _recon on DeviceName | where ReconTime between (RMMTime .. (RMMTime+(_reconWindow))) | project DeviceName, RMMTime, ReconTime, ReconProc, ReconCmd; _rmmRecon | join kind=leftouter _staging on DeviceName | extend HasStagingInWindow = iff(STime between (RMMTime .. (RMMTime+(_stageWindow))), 1, 0) | summarize HasStagingInWindow=max(HasStagingInWindow) by DeviceName, RMMTime, ReconTime, ReconProc, ReconCmd | where HasStagingInWindow == 0 | project DeviceName, RMMTime, ReconTime, ReconProc, ReconCmd
```

### K. Sample correlation query between chat, first contact, and alerts

Note: the post indicates this query should be tuned for specific environments.

```kusto
Note. Please modify or tune for your specific environment.

let _timeFrame = 30m; // Tune: how long after the Teams event to look for matching alerts let _huntingWindow = 4d; // Tune: broader lookback increases coverage but also cost // Seed Teams message activity and normalize the victim/join fields you want to carry forward let _teams = materialize ( MessageEvents | where Timestamp > ago(_huntingWindow) | extend Recipient = parse_json(RecipientDetails) // Optional tuning: add sender/name/content filters here first to reduce volume early //| where SenderDisplayName contains "add keyword" // or SenderDisplayName contains "add keyword" // add other hunting terms | mv-expand Recipient | extend VictimAccountObjectId = tostring(Recipient.RecipientObjectId), VictimUPN = tostring(Recipient.RecipientSmtpAddress) | project TTime = Timestamp, SenderUPN = SenderEmailAddress, SenderDisplayName, VictimUPN, VictimAccountObjectId, ChatThreadId = ThreadId ); // Distinct key sets used to prefilter downstream tables before joining let _VictimAccountObjectId = materialize( _teams | where isnotempty(VictimAccountObjectId) | distinct VictimAccountObjectId ); let _VictimUPN = materialize( _teams | where isnotempty(VictimUPN) | distinct VictimUPN ); let _ChatThreadId = materialize( _teams | where isnotempty(ChatThreadId) | distinct ChatThreadId ); // Find first-seen chat creation events for the chat threads already present in _teams // Tune: add more CloudAppEvents filters here if you want to narrow to external / one-on-one / specific chat types let _firstContact = materialize( CloudAppEvents | where Timestamp > ago(_huntingWindow) | where Application has "Teams" | where ActionType == "ChatCreated" | extend Raw = todynamic(RawEventData) | extend ChatThreadId = tostring(Raw.ChatThreadId) | where isnotempty(ChatThreadId) | join kind=innerunique (_ChatThreadId) on ChatThreadId | summarize FCTime = min(Timestamp) by ChatThreadId ); // Alert branch 1: match by victim object ID // Usually the cleanest identity join if the field is populated consistently let _alerts_by_oid = materialize( AlertEvidence | where Timestamp > ago(_huntingWindow) | where AccountObjectId in (_VictimAccountObjectId) | project ATime = Timestamp, AlertId, Title, AccountName, AccountObjectId, AccountUpn = "", SourceId = "", ChatThreadId = "" ); // Alert branch 2: match by victim UPN // Useful when ObjectId is missing or alert evidence is only populated with UPN let _alerts_by_upn = materialize( AlertEvidence | where Timestamp > ago(_huntingWindow) | where AccountUpn in (_VictimUPN) | project ATime = Timestamp, AlertId, Title, AccountName, AccountObjectId, AccountUpn, SourceId = "", ChatThreadId = "" ); // Alert branch 3: match by chat thread ID // Tune: this is typically the most expensive branch because it inspects AdditionalFields let _alerts_by_thread = materialize( AlertEvidence | where Timestamp > ago(_huntingWindow) | where AdditionalFields has_any (_ChatThreadId) | extend AdditionalFields = todynamic(AdditionalFields) | extend SourceId = tostring(AdditionalFields.SourceId), ChatThreadIdRaw = tostring(AdditionalFields.ChatThreadId) | extend ChatThreadId = coalesce( ChatThreadIdRaw, extract(@"/(?:chats|channels|conversations|spaces)/([^/]+)/", 1, SourceId) ) | where isnotempty(ChatThreadId) | join kind=innerunique (_ChatThreadId) on ChatThreadId | project ATime = Timestamp, AlertId, Title, AccountName, AccountObjectId, AccountUpn = "", SourceId, ChatThreadId ); // // add branch 4 to corrilate with host events // // Add first-contact context back onto the Teams seed set let _teams_fc = materialize( _teams | join kind=leftouter _firstContact on ChatThreadId | extend FirstContact = isnotnull(FCTime) ); // Join path 1: Teams victim object ID -> alert AccountObjectId let _matches_oid = _teams_fc | where isnotempty(VictimAccountObjectId) | join hint.strategy=broadcast kind=leftouter ( _alerts_by_oid ) on $left.VictimAccountObjectId == $right.AccountObjectId // Time bound keeps only alerts near the Teams activity; widen/narrow _timeFrame to tune sensitivity | where isnull(ATime) or ATime between (TTime .. TTime + _timeFrame) | extend MatchType = "ObjectId"; // Join path 2: Teams victim UPN -> alert AccountUpn let _matches_upn = _teams_fc | where isnotempty(VictimUPN) | join hint.strategy=broadcast kind=leftouter ( _alerts_by_upn ) on $left.VictimUPN == $right.AccountUpn | where isnull(ATime) or ATime between (TTime .. TTime + _timeFrame) | extend MatchType = "VictimUPN"; // Join path 3: Teams chat thread -> alert chat thread let _matches_thread = _teams_fc | where isnotempty(ChatThreadId) | join hint.strategy=broadcast kind=leftouter ( _alerts_by_thread ) on ChatThreadId | where isnull(ATime) or ATime between (TTime .. TTime + _timeFrame) | extend MatchType = "ChatThreadId"; // // add branch 4 for host events // // Merge all match paths and collapse multiple alert hits per Teams event into one row union _matches_oid, _matches_upn, _matches_thread | summarize AlertTitles = make_set(Title, 50), AlertIds = make_set(AlertId, 50), MatchTypes = make_set(MatchType, 10), FirstAlertTime = min(ATime) by TTime, SenderUPN, SenderDisplayName, VictimUPN, VictimAccountObjectId, ChatThreadId,
```

## References

- https://techcommunity.microsoft.com/blog/MicrosoftDefenderforOffice365Blog/protection-against-email-bombs-with-microsoft-defender-for-office-365/4418048
- https://techcommunity.microsoft.com/blog/microsoftdefenderforoffice365blog/protection-against-multi-modal-attacks-with-microsoft-defender/4438786
- https://www.microsoft.com/en-us/security/blog/2026/03/16/help-on-the-line-how-a-microsoft-teams-support-call-led-to-compromise/
- https://www.microsoft.com/en-us/security/blog/2025/10/07/disrupting-threats-targeting-microsoft-teams/

## Learn more (as listed)

- Securing Copilot Studio agents with Microsoft Defender: https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-protection
- Zero Trust for AI workshop: https://microsoft.github.io/zerotrustassessment/
- Protect your agents in real-time during runtime (Preview): https://learn.microsoft.com/en-us/defender-cloud-apps/real-time-agent-protection-during-runtime
- Copilot Studio Agent Builder: https://eurppc-word-edit.officeapps.live.com/we/%E2%80%A2%09https:/learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/copilot-studio-agent-builder
- Microsoft 365 Copilot AI security documentation: https://learn.microsoft.com/en-us/copilot/microsoft-365/microsoft-365-copilot-ai-security
- How Microsoft discovers and mitigates evolving attacks against AI guardrails: https://www.microsoft.com/en-us/security/blog/2024/04/11/how-microsoft-discovers-and-mitigates-evolving-attacks-against-ai-guardrails/


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/18/crosstenant-helpdesk-impersonation-data-exfiltration-human-operated-intrusion-playbook/)

