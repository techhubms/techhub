---
primary_section: ai
author: Microsoft Threat Intelligence and Microsoft Defender Security Research Team
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/16/dissecting-sapphire-sleets-macos-intrusion-from-lure-to-compromise/
feed_name: Microsoft Security Blog
title: Dissecting Sapphire Sleet’s macOS intrusion from lure to compromise
tags:
- Advanced Hunting
- AI
- And Control (tcc.db)
- Apple Safe Browsing
- AppleScript
- Command And Control (c2)
- Consent
- Credential Theft
- Crypto Wallet Theft
- Curl
- Data Exfiltration
- Elevation Of Privilege
- Gatekeeper
- Indicators Of Compromise (iocs)
- KQL
- LaunchDaemons
- Mach O
- Macos
- Microsoft Defender For Endpoint On Mac
- Microsoft Defender XDR
- Microsoft Security Copilot
- Microsoft Sentinel
- News
- Notarization
- NSCreateObjectFileImageFromMemory
- Osascript
- Persistence
- Phishing
- Sapphire Sleet
- SCPT
- Script Editor
- Security
- Sleet
- Social Engineering
- TCC
- Telegram Bot API
- Transparency
- XProtect
date: 2026-04-16 15:00:00 +00:00
section_names:
- ai
- security
---

Microsoft Threat Intelligence and the Microsoft Defender Security Research Team break down a Sapphire Sleet macOS intrusion chain that relies on social engineering and user-initiated AppleScript execution, and provide Defender detections, KQL hunting queries, and IOCs to help security teams spot and stop similar activity.<!--excerpt_end-->

# Dissecting Sapphire Sleet’s macOS intrusion from lure to compromise

## Executive summary

Microsoft Threat Intelligence uncovered a macOS-focused campaign by the North Korean threat actor **Sapphire Sleet** that relies on **social engineering** rather than exploiting software vulnerabilities.

Attackers impersonate a legitimate software update and trick users into **manually executing malicious files**, enabling credential theft and large-scale data exfiltration while bypassing several built-in macOS protections.

Microsoft shared details with Apple via responsible disclosure, and Apple implemented updates to help detect and block associated infrastructure and malware.

## What makes this intrusion chain work

The campaign shifts execution into a **user-initiated context** by persuading users to run AppleScript or Terminal commands. This helps activity proceed outside or around protections such as:

- Transparency, Consent, and Control (**TCC**)
- **Gatekeeper**
- Quarantine enforcement
- Notarization checks

The blog covers the attack chain from initial access using malicious **`.scpt`** files through payload delivery, credential harvesting, TCC manipulation, persistence, and exfiltration. It also includes Microsoft Defender detections, hunting queries, and indicators of compromise (IOCs).

## Sapphire Sleet’s campaign lifecycle

### Initial access and social engineering

Sapphire Sleet (active since at least March 2020) primarily targets **finance and cryptocurrency** organizations to steal wallet access and related intellectual property.

A common playbook used here:

- Fake recruiter profiles on social/professional networks
- Conversations about job opportunities
- “Technical interview” setup
- Victim instructed to install a “video conferencing tool” or “SDK update”

In the observed activity, the victim was directed to download a file named **`Zoom SDK Update.scpt`**, a compiled AppleScript that opens in **macOS Script Editor**. Script Editor can execute arbitrary shell commands using AppleScript’s `do shell script`.

![Flowchart illustrating Sapphire Sleet targeting users with a fake Zoom Support meeting invite, leading to the user joining the meeting, downloading a malicious AppleScript file, and executing the script via Script Editor.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-24.webp)

#### AppleScript lure behavior

Key elements:

- A large **decoy comment block** that mimics benign update instructions
- **Thousands of blank lines** inserted to push malicious logic out of view
- Runs the legitimate `softwareupdate` binary with an invalid parameter (no real update, but reinforces legitimacy)
- Uses `curl` to fetch attacker content and immediately passes it to `osascript` for execution (`run script` result)

![Screenshot of a code editor showing a script for updating Zoom Meeting SDK with comments about a new Zoom Web App release and instructions for manual SDK upgrade. The script includes a URL for SDK setup, a shell command to update software, and a highlighted note indicating presence of a malicious payload hidden below the visible editor area.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Figure-2.-The-AppleScript-lure-with-decoy-content-and-payload-execution.webp)

### Execution and payload delivery

#### Cascading curl-to-osascript execution

Opening the `.scpt` file launches a multi-stage chain:

- Script Editor executes AppleScript
- AppleScript uses **`curl`** to fetch additional AppleScript
- Output is executed immediately via **`osascript`**
- Each stage uses distinct **user-agent strings** as campaign identifiers

![Flowchart diagram illustrating a multi-stage malware attack process starting from a script editor executing various curl commands and AppleScripts, leading to backdoor deployments along with a credential harvester and host monitoring component.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Figure-3.-Process-tree-showing-cascading-execution-from-Script-Editor-1-scaled.webp)

The report notes a path distinction:

- `mac-cur1` through `mac-cur3`: fetch from `*/version/*` (AppleScript piped to `osascript`)
- `mac-cur4` and `mac-cur5`: fetch from `*/status/*` (ZIP archives with compiled `.app` bundles)

#### Curl chain summary

| User agent | URL path | Purpose |
| --- | --- | --- |
| mac-cur1 | `*/fix/mac/update/version/*` | Main orchestrator (piped to `osascript`) beacon. Downloads `com.apple.cli` host monitoring component and services backdoor |
| mac-cur2 | `*/fix/mac/update/version/*` | Invokes `curl` with mac-cur4 which downloads credential harvester `systemupdate.app` |
| mac-cur3 | `*/fix/mac/update/version/*` | TCC bypass + data collection + exfiltration (wallets, browser, keychains, history, Apple Notes, Telegram) |
| mac-cur4 | `*/fix/mac/update/status/*` | Downloads credential harvester `systemupdate.app` (ZIP) |
| mac-cur5 | `*/fix/mac/update/status/*` | Downloads decoy completion prompt `softwareupdate.app` (ZIP) |

![Screenshot of a script editor displaying a Zoom SDK update script with process ID 10015. The script includes multiple cURL commands, Rosetta check, and a main payload section indicating potential malicious activity branching from the execution point.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Figure-4.-The-curl-chain-showing-user-agent-strings-and-payload-routing-1.webp)

### Reconnaissance and C2 registration

After execution, the malware collects system details (user, hostname, time, OS install date) and registers the device with C2.

Fields called out in the post:

- `mid`: device UUID
- `did`: campaign tracking identifier
- `user`: hostname + device serial number

![Screenshot of a terminal command using curl to send a POST request with JSON data to an API endpoint. The JSON payload includes fields like mid, did, user, osVersion, timezone, installdate, and proclist, with several values redacted for privacy.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-26.webp)

#### Host monitoring component: `com.apple.cli`

A ~5 MB Mach-O binary disguised using Apple-like naming.

- Downloaded and launched via AppleScript (`osascript`)
- Collects:
  - `sw_vers`
  - `date -u`
  - `sysctl hw.model`
  - `ps aux` in a tight loop (process inventory)
- Maintains outbound connectivity to **83.136.208[.]246:6783**

![Screenshot of a code snippet showing a script designed to execute shell commands for downloading and running a payload, including setting usernames and handling errors.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-28-1024x421.webp)

### Credential access

#### Credential harvester: `systemupdate.app`

Delivered via the mac-cur2 stage:

- Checks for infection marker file `*.zoom.log` (skips if already infected)
- Downloads `systemupdate.app` via mac-cur4 (ZIP), extracts to temp, launches
- Displays a native-looking macOS password prompt
- Validates the password using directory services (local authentication database)
- Exfiltrates verified password using the **Telegram Bot API**

![Screenshot of a system preferences prompt requesting password entry to configure system settings before running an application. It features a red stop sign icon with an exclamation mark and a blue folder, a text input field, and a "Continue" button.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-25.webp)

#### Decoy completion prompt: `softwareupdate.app`

Delivered later via mac-cur5:

- Does not harvest credentials
- Shows a convincing “system update complete” prompt to close the social engineering loop

### Persistence

#### Primary backdoor and persistence installer: `services` binary

The **services** backdoor provides:

- Interactive command execution channel
- Persistence via launch daemon
- Deployment of two additional backdoors

Deployment details:

- Initially downloaded as hidden file `.*services`
- Copied to final location; temp removed
- Creates install marker: `~/Library/Application Support/Authorization/auth.db`
- Writes runtime/install errors to `/tmp/lg4err`
- Uses interactive zsh shells (`/bin/zsh -i`) to support sudo operations

![Screenshot of a code snippet written in a scripting language, focused on setting variables, file paths, and executing shell commands for downloading and managing files.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Figure-8.-Services-backdoor-deployment-using-osascript-2-1024x628.webp)

![Screenshot of terminal commands and script annotations related to installing and configuring persistence for icloudz malware. Commands include environment checks, anti-sleep measures, OS version beacon, credential harvester deletion, self-copy creation, and five persistence installation steps with file paths, permissions, and launchctl commands.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-29.webp)

#### Additional backdoors: `icloudz` and `com.google.chromes.updaters`

- `icloudz` is a renamed copy of the services backdoor (same SHA-256)
- Uses reflective code loading via **`NSCreateObjectFileImageFromMemory`** (loads payloads from C2 into memory)
- Stored at `~/Library/Application Support/iCloud/icloudz`

Observed repeated initialization pattern:

- Stop existing `caffeinate`
- Relaunch `caffeinate` with `nohup` to prevent sleep
- Collect `sw_vers`, `sysctl -n hw.model`
- Spawn `/bin/zsh -i`

From there, it drops and runs a tertiary backdoor:

- `~/Library/Google/com.google.chromes.updaters`
- Adjusts permissions/ownership and launches via sudo

Persistence mechanism:

- Launch daemon plist: `/Library/LaunchDaemons/com.google.webkit.service.plist`
- Launches `icloudz` at startup (even without user sign-in)

`com.google.chromes.updaters`:

- ~7.2 MB
- Connects to `check02id[.]com` over port 5202
- 60-second beacon loop running minimal commands like `whoami` and `sw_vers -productVersion`

### Privilege escalation / defense evasion

#### TCC bypass: granting AppleEvents permissions

The mac-cur3 stage manipulates the user-level TCC database:

- Target DB: `~/Library/Application Support/com.apple.TCC/TCC.db`
- Uses Finder (has Full Disk Access by default) to rename `com.apple.TCC` so the DB can be copied by a process without FDA
- Uses `sqlite3` to inject an entry granting `/usr/bin/osascript` AppleEvents permission to `com.apple.finder`
- Uses valid `csreq` blobs to bind the grant to Apple-signed executables
- Sets `auth_value=2` (allowed) and `auth_reason=3` (user-set), avoiding prompts
- Restores folder name and cleans staging files

![Screenshot of a code snippet showing an SQLite3 command to insert data into an access table with columns for service, client, client_type, auth_value, and other attributes.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-27.webp)

### Collection and exfiltration

A 575-line AppleScript payload collects, stages, compresses, and exfiltrates multiple data categories.

#### Exfiltration architecture

Uploads follow a consistent pattern:

- Run via `nohup` so they continue without an active session
- Use `auth` header (upload token) and `mid` header (device UUID)

![Screenshot of a terminal window showing a shell command sequence for zipping and uploading a file. Commands include compressing a directory, removing temporary files, and using curl with headers for authentication and file upload to a specified IP address and port.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Figure-11.-Exfiltration-upload-pattern-with-nohup.webp)

#### Data collected

- Host/system reconnaissance (user, hostname, macOS version, CPU model)
- Installed apps + runtime verification (including checks that deployed components are running)
- Telegram Desktop session data (enough for session recreation)
- Browser profiles for Chromium-based browsers (Chrome, Brave, Arc), including wallet-extension IndexedDB entries
- macOS keychain (for offline decryption using stolen password)
- Crypto wallets (Ledger Live, Exodus application support directories)
- SSH keys and shell history
- Apple Notes database
- System logs + failed access attempt notes

#### Exfiltration summary table

| # | Data category | ZIP name | Upload port | Estimated sensitivity |
| --- | --- | --- | --- | --- |
| 1 | Telegram session | `tapp_<user>.zip` | 8443 | Critical — session hijack |
| 2 | Browser data + Keychain | `ext_<user>.zip` | 8443 | Critical — all passwords |
| 3 | Ledger wallet | `ldg_<user>.zip` | 8443 | Critical — crypto keys |
| 4 | Exodus wallet | `exds_<user>.zip` | 8443 | Critical — crypto keys |
| 5 | SSH + shell history | `hs_<user>.zip` | 8443 | High — lateral movement |
| 6 | Apple Notes | `nt_<user>.zip` | 8443 | Medium-High |
| 7 | System log | `lg_<user>` (no zip) | 8443 | Low — fingerprinting |
| 8 | Recon log | `flog` (no zip) | 8443 | Low — inventory |
| 9 | Credentials | Telegram message | 443 (Telegram API) | Critical — sign-in password |

The post lists:

- Upload token: `fwyan48umt1vimwqcqvhdd9u72a7qysi`
- Example machine identifier: `82cf5d92-87b5-4144-9a4e-6b58b714d599`

## Defending against Sapphire Sleet intrusion activity

Apple actions called out:

- Safari Apple Safe Browsing protections to block malicious infrastructure
- XProtect signatures to detect and block malware families

Microsoft-recommended mitigations:

- Train users to recognize social engineering and avoid running scripts/commands from unsolicited outreach
- Block/restrict execution of `.scpt` files and unsigned Mach-O binaries from the internet
- Inspect compiled AppleScript files and treat Script Editor as a high-risk execution vector in this context
- Limit/audit `curl` piped to interpreters (`curl | osascript`, `curl | sh`, `curl | bash`), especially with unusual user-agent strings
- Monitor clipboard use (wallet addresses/credentials) for tampering risks
- Monitor for unauthorized modifications to `~/Library/Application Support/com.apple.TCC/TCC.db`
- Audit LaunchDaemon/LaunchAgent installs, especially suspicious `com.google.*` / `com.apple.*`-styled names
- Harden protection for crypto wallets and browser credential stores; rotate browser-stored credentials
- Prefer browsers that support Microsoft Defender SmartScreen (e.g., Microsoft Edge on macOS)

Defender for Endpoint on Mac specific actions:

- Use Microsoft Defender for Endpoint on Mac
- Enable cloud-delivered protection + automatic sample submission
- Enable PUA protection in block mode
- Enable network protection

## Microsoft Defender detection and hunting guidance

### Defender detections (high level)

The post maps observed tactics (initial access, execution, persistence, defense evasion, credential access, exfiltration, C2) to coverage in:

- Microsoft Defender Antivirus
- Microsoft Defender for Endpoint

It also lists example detection names such as:

- `Trojan:MacOS/SuspMalScript.C`
- `Trojan:MacOS/PassStealer.D`
- `Trojan:MacOS/NukeSped.D`
- `Backdoor:MacOS/FlowOffset.*!dha`

### Microsoft Security Copilot

Microsoft Security Copilot is described as embedded in Microsoft Defender and able to summarize incidents, analyze scripts, generate hunting queries, and support guided response workflows. The post also links to Security Copilot agents (Threat Intelligence Briefing, Phishing Triage, Threat Hunting, Dynamic Threat Detection) and notes developer scenarios for building custom agents/plugins.

### Hunting queries

#### Microsoft Defender XDR (KQL)

Queries included to hunt for:

- `curl` piped to `osascript` (and shells)
- Campaign user-agent strings (`mac-cur1`…`mac-cur5`, `audio`, `beacon`)
- Network connections to known C2 domains/IPs
- TCC database copy/modify/overwrite events
- Suspicious LaunchDaemon plist creation under `/Library/LaunchDaemons/`
- Execution from suspicious paths (e.g., `Library/Services/services`, `Application Support/iCloud/icloudz`)
- `dscl -authonly` usage
- Telegram Bot API connections (`api.telegram.org/bot...`)
- `NSCreateObjectFileImageFromMemory` signals
- Suspicious `caffeinate` restart patterns
- Known malicious SHA-256 hashes
- ZIP creation in `/tmp/` followed by `curl` uploads
- Script Editor spawning `curl`/`osascript`/shell children

#### Microsoft Sentinel

Guidance and KQL provided to:

- Deploy Threat Intelligence solution via Sentinel Content Hub and use “TI map” analytics
- Query for C2 domains/IPs in network session data
- Search for known file hashes across data sources
- Query Defender Antivirus alerts for listed macOS malware families

## Indicators of compromise (IOCs)

### Malicious file hashes (examples)

- `/Users/<user>/Downloads/Zoom SDK Update.scpt`: `2075fd1a1362d188290910a8c55cf30c11ed5955c04af410c481410f538da419`
- `/Users/<user>/com.apple.cli`: `05e1761b535537287e7b72d103a29c4453742725600f59a34a4831eafc0b8e53`
- `services / icloudz`: `5fbbca2d72840feb86b6ef8a1abb4fe2f225d84228a714391673be2719c73ac7`
- `com.google.chromes.updaters`: `5e581f22f56883ee13358f73fabab00fcf9313a053210eb12ac18e66098346e5`
- `com.google.webkit.service.plist`: `95e893e7cdde19d7d16ff5a5074d0b369abd31c1a30962656133caa8153e8d63`

### Domains and IP addresses (examples)

- `uw04webzoom[.]us` → `188.227.196[.]252:443` (payload staging)
- `check02id[.]com` → `83.136.210[.]180:5202` (`chromes.updaters`)
- `83.136.208[.]246:6783` (`com.apple.cli` / beacon)
- `83.136.209[.]22:8444` (downloads services backdoor)
- `83.136.208[.]48:443` (services invoked with IP/port)
- `104.145.210[.]107:6783` (exfiltration)

## References and further reading

- https://cloud.google.com/blog/topics/threat-intelligence/unc1069-targets-cryptocurrency-ai-social-engineering
- https://www.huntress.com/blog/inside-bluenoroff-web3-intrusion-analysis
- https://securelist.com/bluenoroff-apt-campaigns-ghostcall-and-ghosthie/117842/
- https://x.com/malwrhunterteam/status/2008831892616081508
- https://x.com/patrickwardle/status/2009008936771543341?s=46

Learn more: Microsoft Threat Intelligence Blog: https://aka.ms/threatintelblog


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/16/dissecting-sapphire-sleets-macos-intrusion-from-lure-to-compromise/)

