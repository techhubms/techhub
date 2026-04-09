---
section_names:
- ai
- security
primary_section: ai
title: 'Investigating Storm-2755: “Payroll pirate” attacks targeting Canadian employees'
feed_name: Microsoft Security Blog
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/09/investigating-storm-2755-payroll-pirate-attacks-targeting-canadian-employees/
author: Microsoft Incident Response
date: 2026-04-09 15:00:00 +00:00
tags:
- Adversary in The Middle
- Adversary in The Middle (aitm)
- AI
- AiTM
- Axios
- Business Email Compromise
- Conditional Access
- Continuous Access Evaluation
- CVE 27152
- Entra ID Protection
- FIDO2
- Global Secure Access
- Inbox Rules
- Indicators Of Compromise
- KQL
- Malvertising
- Microsoft Defender XDR
- Microsoft Entra ID
- Microsoft Sentinel
- News
- OAuth Tokens
- Payroll Diversion
- Phishing Resistant MFA
- Security
- SEO Poisoning
- Session Hijacking
- Storm
- Storm 2755
- Token Replay
- WebAuthn
- Workday
---

Microsoft Incident Response (DART) analyzes Storm-2755 “payroll pirate” attacks targeting Canadian users, detailing how adversary-in-the-middle session hijacking bypasses MFA, what signals to hunt for in Entra and Defender, and practical remediation steps including token revocation, Conditional Access hardening, and inbox-rule cleanup.<!--excerpt_end-->

## Overview
Microsoft Incident Response – Detection and Response Team (DART) observed an emerging financially motivated threat actor, **Storm-2755**, running “payroll pirate” attacks targeting **Canadian users**. The actor compromised employee accounts, accessed employee profile/payroll-related workflows, and attempted to **divert salary payments** to attacker-controlled accounts—causing direct financial loss.

The campaign differs from similar payroll diversion activity because it relied on **geographic targeting** (Canada) and used **malvertising** plus **search engine optimization (SEO) poisoning** on industry-agnostic search terms.

## What makes this campaign work
Storm-2755’s activity is built around **adversary-in-the-middle (AiTM)** techniques that:

- Proxy the authentication flow in real time
- Capture **session cookies** and **OAuth access tokens**
- Enable **token replay** to reuse an already-authenticated session
- Effectively **bypass non-phishing-resistant MFA** (for example, legacy MFA methods)

Phishing-resistant MFA methods such as **FIDO2/WebAuthN** are called out as mitigations.

## Storm-2755’s attack chain
### Initial access
- Storm-2755 used SEO poisoning/malvertising to place an actor-controlled domain (noted as *bluegraintours[.]com*) high in search results for generic queries like “Office 365” (and misspellings like “Office 265”).
- Victims were redirected to a **malicious Microsoft 365 sign-in page** designed to mimic the legitimate experience.
- DART noted a distinctive sign-in pattern:
  - A **50199** sign-in interrupt error immediately before the attacker successfully compromises the account.
  - A shift in **user-agent to Axios** (often **axios/1.7.9**) while the **session ID remains consistent**, indicating token replay.

The post also notes Axios itself isn’t malicious, but the attack path may leverage known vulnerabilities in Axios, specifically **CVE-2025-27152** (SSRF risk).

### Persistence
- Observed **non-interactive sign-ins** to **OfficeHome** with Axios user-agent approximately every ~30 minutes, consistent with scheduled token replay.
- Persistence often ended after ~30 days when stolen tokens became inactive (expiration/rotation/policy enforcement). In a smaller set of cases, **password and MFA changes** were observed to maintain longer-term access.

### Discovery
During compromised sessions, Storm-2755 searched intranets/mailboxes for payroll/HR processes using keywords such as:

- “payroll”, “HR”, “human”, “resources”, “support”, “info”, “finance”, “account”, “admin”

Email subject lines were consistent:

- **“Question about direct deposit”**

If social engineering did not succeed, the actor pivoted to direct interaction with HR SaaS systems (example given: **Workday**) to manually change banking details.

### Defense evasion
- Created **inbox rules** to move emails containing keywords like “direct deposit” or “bank” into a hidden folder (conversation history) and prevent further rule processing.
- Renewed stolen sessions around **5:00 AM local time** to reduce chance of user reauthentication invalidating access.

### Impact
A documented case resulted in direct financial loss:

- Inbox rules suppressed HR alerts
- Attacker requested payroll banking changes
- Attacker signed into Workday as the victim and updated banking details
- A payroll check was redirected to an attacker-controlled account

## Defensive guidance (mitigation and hardening)
### Immediate response actions
- **Revoke compromised tokens and sessions immediately**
- **Remove malicious inbox rules**
- **Reset credentials** and **MFA methods** for affected accounts

### Hardening recommendations
- Enforce **phishing-resistant MFA**
- Use **Conditional Access** device compliance enforcement
- **Block legacy authentication protocols**
- Centralize logs in a **SIEM** to baseline normal vs irregular activity
- Enable **Microsoft Defender** to:
  - Disrupt attacks automatically
  - Revoke tokens in near real time
  - Monitor for anomalous user-agents like Axios
  - Audit OAuth applications to prevent persistence
- Run phishing simulation campaigns to improve user resistance

Referenced Microsoft guidance:

1. Implement phishing-resistant MFA:
   - https://learn.microsoft.com/en-us/security/zero-trust/sfi/phishing-resistant-mfa
2. Configure adaptive session lifetime policies (Conditional Access):
   - https://learn.microsoft.com/en-us/entra/identity/conditional-access/howto-conditional-access-session-lifetime
3. Leverage Continuous Access Evaluation (CAE):
   - https://learn.microsoft.com/en-us/entra/identity/conditional-access/concept-continuous-access-evaluation
4. Consider Global Secure Access (GSA):
   - https://learn.microsoft.com/entra/global-secure-access/overview-what-is-global-secure-access
5. Create alerting for suspicious inbox-rule creation:
   - https://learn.microsoft.com/en-us/defender-xdr/alert-grading-playbook-inbox-manipulation-rules
6. Use Intune compliance policies (paired with Conditional Access):
   - https://learn.microsoft.com/en-us/intune/intune-service/protect/device-compliance-get-started

## Microsoft Defender detection and hunting guidance
The post lists tactics, observed activity, and where Microsoft coverage applies, including:

- **Microsoft Defender XDR** detections
  - Anomalous OAuth device code authentication activity
  - Possible AiTM attack detected (ConsentFix)
  - Potential Credential Abuse in Entra ID Authentication
  - Suspicious sign-in from unusual user-agent and IP
  - Suspicious Entra device join/registration
- **Microsoft Entra ID Protection** detections
  - Atypical Travel
  - Impossible Travel
  - Unfamiliar sign-in properties (lower confidence)

## Microsoft Security Copilot (as referenced in the post)
The post describes **Microsoft Security Copilot** as embedded in Microsoft Defender, with capabilities to summarize incidents, analyze artifacts, guide responses, and generate hunting queries and incident reports.

It also references deploying Security Copilot agents (via Microsoft documentation), including:

- Threat Intelligence Briefing agent
- Phishing Triage agent
- Threat Hunting agent
- Dynamic Threat Detection agent

## Hunting queries
### Microsoft Defender XDR (KQL)
**Review inbox rules created to hide or delete incoming emails from Workday**

```kusto
CloudAppEvents
| where Timestamp >= ago(1d)
| where Application == "Microsoft Exchange Online" and ActionType in ("New-InboxRule", "Set-InboxRule")
| extend Parameters = RawEventData.Parameters // extract inbox rule parameters
| where Parameters has "From" and Parameters has "@myworkday.com" // filter for inbox rule with From field and @MyWorkday.com in the parameters
| where Parameters has "DeleteMessage" or Parameters has ("MoveToFolder") // email deletion or move to folder (hiding)
| mv-apply Parameters on (where Parameters.Name == "From" | extend RuleFrom = tostring(Parameters.Value))
| mv-apply Parameters on (where Parameters.Name == "Name" | extend RuleName = tostring(Parameters.Value))
```

**Review updates to payment election or bank account information in Workday**

```kusto
CloudAppEvents
| where Timestamp >= ago(1d)
| where Application == "Workday"
| where ActionType == "Change My Account" or ActionType == "Manage Payment Elections"
| extend Descriptor = tostring(RawEventData.target.descriptor)
```

### Microsoft Sentinel
The post calls out using **Threat Intelligence (TI) Mapping analytics** (rules prefixed with **“TI map”**) and points to installing the Threat Intelligence solution via the Sentinel Content Hub:

- https://learn.microsoft.com/azure/sentinel/sentinel-solutions-deploy

**Malicious inbox rule (Sentinel query)**

```kusto
let Keywords = dynamic(["direct deposit", “hr”, “bank”]);
OfficeActivity
| where OfficeWorkload =~ "Exchange"
| where Operation =~ "New-InboxRule" and (ResultStatus =~ "True" or ResultStatus =~ "Succeeded")
| where Parameters has "Deleted Items" or Parameters has "Junk Email" or Parameters has "DeleteMessage"
| extend Events=todynamic(Parameters)
| parse Events with * "SubjectContainsWords" SubjectContainsWords '}'*
| parse Events with * "BodyContainsWords" BodyContainsWords '}'*
| parse Events with * "SubjectOrBodyContainsWords" SubjectOrBodyContainsWords '}'*
| where SubjectContainsWords has_any (Keywords) or BodyContainsWords has_any (Keywords) or SubjectOrBodyContainsWords has_any (Keywords)
| extend ClientIPAddress = case(
    ClientIP has ".", tostring(split(ClientIP,":")[0]),
    ClientIP has "[", tostring(trim_start(@'[[]',tostring(split(ClientIP,"]")[0]))),
    ClientIP
  )
| extend Keyword = iff(isnotempty(SubjectContainsWords), SubjectContainsWords, (iff(isnotempty(BodyContainsWords),BodyContainsWords,SubjectOrBodyContainsWords )))
| extend RuleDetail = case(OfficeObjectId contains '/' , tostring(split(OfficeObjectId, '/')[-1]) , tostring(split(OfficeObjectId, '\\')[-1]))
| summarize count(), StartTimeUtc = min(TimeGenerated), EndTimeUtc = max(TimeGenerated) by Operation, UserId, ClientIPAddress, ResultStatus, Keyword, OriginatingServer, OfficeObjectId, RuleDetail
| extend AccountName = tostring(split(UserId, "@")[0]), AccountUPNSuffix = tostring(split(UserId, "@")[1])
| extend OriginatingServerName = tostring(split(OriginatingServer, " ")[0])
```

**Detect network IP and domain indicators of compromise using ASIM**

```kusto
let lookback = 30d;
let ioc_domains = dynamic(["http://bluegraintours.com"]);
_Im_NetworkSession(starttime=todatetime(ago(lookback)), endtime=now())
| where DstDomain has_any (ioc_domains)
| summarize imNWS_mintime=min(TimeGenerated), imNWS_maxtime=max(TimeGenerated), EventCount=count() by SrcIpAddr, DstIpAddr, DstDomain, Dvc, EventProduct, EventVendor
```

**Detect domain and URL indicators of compromise using ASIM**

```kusto
let ioc_domains = dynamic(["http://bluegraintours.com"]);
_Im_WebSession (url_has_any = ioc_domains)
```

## Indicators of compromise (IOCs)
The post highlights a characteristic pattern tied to *hxxp://bluegraintours[.]com*:

- Multiple failed sign-ins with various causes
- A failure citing **Entra error code 50199**, immediately before success
- User-agent changes to **Axios** while session ID remains unchanged (token replay)

Listed IOCs:

- *hxxp://bluegraintours[.]com* (URL)
- **axios/1.7.9** (user-agent)

## Related reading
- Prior Microsoft campaign write-up (US universities):
  - https://www.microsoft.com/en-us/security/blog/2025/10/09/investigating-targeted-payroll-pirate-attacks-affecting-us-universities/
- Push Security analysis (ADFS-enabled phishing context):
  - https://pushsecurity.com/blog/phishing-with-active-directory-federation-services/
- Microsoft Threat Intelligence blog:
  - https://aka.ms/threatintelblog

## Figures
![A user is lured to an actor-controlled authentication page via SEO poisoning or malvertising and unknowingly submits credentials, enabling the threat actor to replay the stolen session token for impersonation. The actor then maintains persistence through scheduled token replay and conducts follow-on activity such as creating inbox rules or requesting changes in direct deposits until session revocation occurs.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Storm-2755-attack-chain-flow-scaled.webp)

![An example email with several questions regarding direct deposit payments, such as where to send the void cheque, whether the payment can go to a new account, and requesting confirmation of the next payment date.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-23.webp)


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/09/investigating-storm-2755-payroll-pirate-attacks-targeting-canadian-employees/)

