---
tags:
- Advanced Hunting
- AI
- Anti Phishing Policies
- AWS Lambda
- Browser in The Browser
- Cloudflare Workers
- Conditional Access
- Defender For Office 365
- Device Code Phishing
- Domain Shadowing
- Entra ID Protection
- Indicator Of Compromise
- KQL
- Microsoft Defender XDR
- Microsoft Entra ID
- Microsoft Graph
- Microsoft Sentinel
- News
- Node.js
- OAuth 2.0 Device Authorization Grant
- Phishing
- PRT
- Railway.com
- Refresh Tokens
- Risky Sign Ins
- Safe Links
- Security
- Token Theft
- Vercel
title: Inside an AI-enabled device code phishing campaign
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/06/ai-enabled-device-code-phishing-campaign-april-2026/
date: 2026-04-06 16:34:17 +00:00
primary_section: ai
feed_name: Microsoft Security Blog
section_names:
- ai
- security
author: Microsoft Defender Security Research Team
---

The Microsoft Defender Security Research Team breaks down an AI-enabled device code phishing campaign abusing the OAuth device code flow to steal tokens at scale, then using Microsoft Graph for reconnaissance and inbox rules for persistence. It includes a phase-by-phase attack chain plus concrete mitigations across Entra ID, Defender, and Sentinel.<!--excerpt_end-->

# Inside an AI-enabled device code phishing campaign

Microsoft Defender Security Research observed a large-scale phishing campaign that abuses the **OAuth 2.0 Device Code Authentication flow** to compromise organizational accounts. Unlike older “static” device-code lures (where a pre-generated code often expires before a victim uses it), this campaign **generates the device code on-demand** at the moment the user clicks, keeping the full 15-minute validity window.

The activity is linked to the broader emergence of **EvilToken** (a Phishing-as-a-Service toolkit) and represents an escalation from the Storm-2372 campaign described earlier:

- [Storm-2372 conducts device code phishing campaign (Feb 2025)](https://www.microsoft.com/en-us/security/blog/2025/02/13/storm-2372-conducts-device-code-phishing-campaign/)

## What makes this campaign different

- **Advanced backend automation**: Threat actors spin up thousands of short-lived polling nodes (e.g., via **Railway.com**) to run backend logic (including **Node.js**) and evade signature/pattern-based detection.
- **Hyper-personalized lures**: **Generative AI** is used to craft role-aligned phishing emails (RFPs, invoices, manufacturing workflows).
- **Dynamic device code generation**: Device codes are generated only after the victim interacts with the link, defeating “code expired” failure modes.
- **Reconnaissance and persistence**: After compromise, a subset of high-value targets (finance/executive roles) receives deeper follow-on actions, including **Microsoft Graph reconnaissance** and **malicious inbox rules**.

## Device Code Authentication: legitimate flow, risky tradeoff

Device Code Authentication is intended for devices with limited interfaces (smart TVs, printers) that can’t do an interactive login. A user receives a short code and enters it in a browser on another device to complete authentication:

- [Device Code Authentication documentation](https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-device-code)

**Security tradeoff**: authentication happens on a different device/context, so the initiating session is not strongly bound to the user’s original context. Attackers exploit this “handoff” to get the user to authorize the attacker’s session.

## Attack chain overview

![Attack chain overview diagram](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-2.webp)

### Phase 1: Reconnaissance and target validation

- Threat actor validates account existence using the **GetCredentialType** endpoint (checking whether an address exists/is active in the tenant).
- Recon activity can occur **10–15 days** before phishing.

Delivery uses a multi-stage pipeline to bypass email gateways/URL scanners:

- Redirect chains through compromised legitimate domains and reputable serverless platforms.
- Heavy use of:
  - **Vercel** (\*.vercel.app)
  - **Cloudflare Workers** (\*.workers.dev)
  - **AWS Lambda**

Final landing pages commonly use **browser-in-the-browser** to simulate a legitimate login window, or a blurred document “preview” with a “Verify identity” style button that leads the user to **microsoft.com/devicelogin**.

Example landing page screenshots:

![Browser-in-the-browser device code lure](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-3.webp)

![Phishing theme example](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-4.webp)

Attackers also use:

- **Domain shadowing**
- Brand-impersonating subdomains (examples given: graph-microsoft[.]com, portal-azure[.]com, office365-login[.]com)
- Randomized subdomains to reduce the impact of single-URL takedowns

![Hosting infrastructure distribution](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-5.webp)

### Phase 2: Initial access

Victims receive lure emails (invoices, RFPs, shared files) delivered as:

- Direct URLs
- PDFs
- HTML attachments

Goal: get the user to a threat-actor-controlled interface that ultimately pushes them to device login.

### Phase 3: Dynamic device code generation

When a user clicks the malicious link, the landing page runs automation that:

1. Contacts the threat actor backend (observed paths: **/api/device/start/** or **/start**)
2. The backend proxies to Microsoft’s device authorization endpoint and requests a device code on-demand
3. Displays the device code to the user and redirects them to the real **microsoft.com/devicelogin**

#### The 15-minute race: static vs. dynamic

- Device codes are valid for **15 minutes**.
- **Static** approach fails if the victim doesn’t act quickly enough.
- **Dynamic** generation starts the timer only after the click, dramatically improving success rate.

Reference:

- [OAuth 2.0 device authorization grant](https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-device-code)

#### Observed request details

- Landing-page script triggers a POST to the attacker backend.
- Custom header: **X-Antibot-Token** with a 64-character hex value.
- Empty body (`content-length: 0`).

The backend returns JSON including:

- The live **Device Code**
- A hidden **Session Identifier Code**

![Device code generation sequence screenshots](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-8.webp)

![Device code generation UI](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-6.webp)

![Device code generation UI variant](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-7.webp)

### Phase 4: Exploitation and authentication

To reduce friction, the front-end often:

- Copies the device code to the user clipboard automatically.

If the victim is already signed in, pasting the code and confirming can immediately authorize the attacker’s session.

#### Clipboard manipulation

Attackers use `navigator.clipboard.writeText` to push the device code to the victim clipboard.

![Clipboard hijack screenshot](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-1.jpg)

![Clipboard hijack screenshot (variant)](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-9.webp)

### Phase 5: Session validation (polling)

After the user is sent to the real device login page, the attacker front-end enters a polling loop:

- Function: `checkStatus()`
- Every **3–5 seconds** via `setInterval`
- Calls attacker `/state` endpoint
- Sends the secret session identifier code

While the user is authenticating, status remains “pending”. On success, the attacker backend gains a valid access token.

![Polling state example](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-10.webp)

### Phase 6: Persistence and post-exploitation

Follow-on activity observed includes:

- **Device registration** (sometimes within 10 minutes) to obtain a **Primary Refresh Token (PRT)** for long-term persistence.
- **Microsoft Graph reconnaissance** to map org structure and permissions.
- **Malicious inbox rules** to maintain access and exfiltrate data.
- Targeting a subset of compromised users, especially those with financial authority.

Example inbox-rule screenshot:

![Inbox rule created by threat actor](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/image-11.webp)

## Mitigation and protection guidance

### Entra ID / Conditional Access controls

- **Block device code flow wherever possible** and only allow it where needed:
  - [Block authentication flows policy](https://learn.microsoft.com/entra/identity/conditional-access/policy-block-authentication-flows)
  - [Authentication flows concepts](https://learn.microsoft.com/entra/identity/conditional-access/concept-authentication-flows)
- Force re-auth when needed:
  - [Force re-authentication policy example](https://learn.microsoft.com/entra/identity/conditional-access/policy-all-users-persistent-browser#create-a-conditional-access-policy)
- Use identity protection:
  - [Configure sign-in risk policies](https://learn.microsoft.com/azure/active-directory/identity-protection/howto-identity-protection-configure-risk-policies)
  - [Continuous access evaluation](https://learn.microsoft.com/entra/identity/conditional-access/concept-continuous-access-evaluation)

### User protection and email security

- Educate users on phishing patterns and verify the app being authorized.
- Guidance:
  - [Protect yourself from phishing (Microsoft Support)](https://support.microsoft.com/en-us/security/protect-yourself-from-phishing)
- Configure:
  - [Defender for Office 365 anti-phishing policies](https://learn.microsoft.com/en-us/defender-office-365/anti-phishing-policies-about)
  - [Defender for Office 365 Safe Links](https://learn.microsoft.com/en-us/defender-office-365/safe-links-about)

### Token revocation / incident response

- Revoke refresh tokens if device-code phishing is suspected:
  - [Microsoft Graph: revokeSignInSessions](https://learn.microsoft.com/graph/api/user-revokesigninsessions)

### Stronger authentication

- Require MFA (still foundational), but also move toward phishing-resistant options:
  - [Authentication methods (phishing-resistant options)](https://learn.microsoft.com/entra/identity/authentication/concept-authentication-methods)

### Monitoring and reporting

- Use Risky sign-in reports (Azure portal link provided in the source):
  - Risky sign-ins report in Azure portal

## Defender XDR detections and coverage

The post lists detections spanning:

- **Microsoft Defender for Office 365** (pre-delivery protection)
- **Microsoft Defender for Identity** (anomalous OAuth device code authentication)
- **Microsoft Defender XDR** (suspicious Azure authentication patterns, token replay/session hijacking)
- **Microsoft Entra ID Protection** (anonymous IPs, threat intelligence correlated sign-ins)

## Microsoft Sentinel guidance and hunting queries

Microsoft Sentinel customers can use TI Mapping analytics (“TI map*”) and hunting queries, including:

- [Campaign with suspicious keywords](https://github.com/Azure/Azure-Sentinel/blob/master/Solutions/Microsoft%20Defender%20XDR/Hunting%20Queries/Email%20Queries/QR%20code/Campaign%20with%20suspicious%20keywords.yaml)
- [Determine Successfully Delivered Phishing Emails to Inbox/Junk folder](https://github.com/Azure/Azure-Sentinel/blob/master/Solutions/Microsoft%20Defender%20XDR/Hunting%20Queries/EmailDelivered-ToInbox.yaml)
- [Successful Sign-in from Phishing Link](https://github.com/Azure/Azure-Sentinel/blob/master/Detections/MultipleDataSources/SucessfullSiginFromPhingLink.yaml)
- [Phishing link click observed in Network Traffic](https://github.com/Azure/Azure-Sentinel/blob/master/Detections/MultipleDataSources/PhishinglinkExecutionObserved.yaml)
- [Suspicious URL clicked](https://github.com/Azure/Azure-Sentinel/blob/master/Hunting%20Queries/Microsoft%20365%20Defender/Initial%20access/SuspiciousUrlClicked.yaml)
- [Anomaly of MailItemAccess by GraphAPI](https://github.com/Azure/Azure-Sentinel/blob/master/Hunting%20Queries/Microsoft%20365%20Defender/Exfiltration/Anomaly%20of%20MailItemAccess%20by%20GraphAPI%20%5BNobelium%5D.yaml)
- [OAuth Apps accessing user mail via GraphAPI](https://github.com/Azure/Azure-Sentinel/blob/master/Hunting%20Queries/Microsoft%20365%20Defender/Exfiltration/OAuth%20Apps%20accessing%20user%20mail%20via%20GraphAPI%20%5BNobelium%5D.yaml)
- [OAuth Apps reading mail both via GraphAPI and directly](https://github.com/Azure/Azure-Sentinel/blob/master/Hunting%20Queries/Microsoft%20365%20Defender/Exfiltration/OAuth%20Apps%20reading%20mail%20both%20via%20GraphAPI%20and%20directly%20%5BNobelium%5D.yaml)
- [OAuth Apps reading mail via GraphAPI anomaly](https://github.com/Azure/Azure-Sentinel/blob/master/Hunting%20Queries/Microsoft%20365%20Defender/Exfiltration/OAuth%20Apps%20reading%20mail%20via%20GraphAPI%20anomaly%20%5BNobelium%5D.yaml)

## Advanced hunting (KQL) examples from the post

### Look for error 50199 followed by success

```kusto
EntraIdSigninEvents
| where ErrorCode in (0, 50199)
| summarize ErrorCodes = make_set(ErrorCode) by AccountUpn, CorrelationId, SessionId, bin(Timestamp, 1h)
| where ErrorCodes has_all (0, 50199)
```

### Identify device code auth from suspicious IP ranges

```kusto
EntraIdSigninEvents
| where Call has "Cmsi:cmsi"
| where IPAddress has_any ("160.220.232.", "160.220.234.", "89.150.45.", "185.81.113.", "8.228.105.")
```

### Correlate URL clicks to risky sign-ins

```kusto
let suspiciousUserClicks = materialize(
    UrlClickEvents
    | extend AccountUpn = tolower(AccountUpn)
    | project ClickTime = Timestamp, ActionType, UrlChain, NetworkMessageId, Url, AccountUpn
);
let interestedUsersUpn = suspiciousUserClicks | where isnotempty(AccountUpn) | distinct AccountUpn;
EntraIdSigninEvents
| where ErrorCode == 0
| where AccountUpn in~ (interestedUsersUpn)
| where RiskLevelDuringSignin in (10, 50, 100)
| extend AccountUpn = tolower(AccountUpn)
| join kind=inner suspiciousUserClicks on AccountUpn
| where (Timestamp - ClickTime) between (-2min .. 7min)
| project Timestamp, ReportId, ClickTime, AccountUpn, RiskLevelDuringSignin, SessionId, IPAddress, Url
```

### Monitor suspicious device registration activity

```kusto
CloudAppEvents
| where AccountDisplayName == "Device Registration Service"
| extend ApplicationId_ = tostring(ActivityObjects[0].ApplicationId)
| extend ServiceName_ = tostring(ActivityObjects[0].Name)
| extend DeviceName = tostring(parse_json(tostring(RawEventData.ModifiedProperties))[1].NewValue)
| extend DeviceId = tostring(parse_json(tostring(parse_json(tostring(RawEventData.ModifiedProperties))[6].NewValue))[0])
| extend DeviceObjectId_ = tostring(parse_json(tostring(RawEventData.ModifiedProperties))[0].NewValue)
| extend UserPrincipalName = tostring(RawEventData.ObjectId)
| project TimeGenerated, ServiceName_, DeviceName, DeviceId, DeviceObjectId_, UserPrincipalName
```

### Surface suspicious inbox rule creation

```kusto
CloudAppEvents
| where ApplicationId == "20893" // Microsoft Exchange Online
| where ActionType in ("New-InboxRule","Set-InboxRule","Set-Mailbox","Set-TransportRule","New-TransportRule","Enable-InboxRule","UpdateInboxRules")
| where isnotempty(IPAddress)
| mv-expand ActivityObjects
| extend name = parse_json(ActivityObjects).Name
| extend value = parse_json(ActivityObjects).Value
| where name == "Name"
| extend RuleName = value
| where RuleName matches regex "^[!@#$%^&*()_+={[}\\]|\\\\:;\"\"'.?/~` -]+$"
```

### Surface suspicious email items accessed

```kusto
CloudAppEvents
| where ApplicationId == "20893" // Microsoft Exchange Online
| where ActionType == "MailItemsAccessed"
| where isnotempty(IPAddress)
| where UncommonForUser has "ISP"
```

## Indicators of compromise (IOC)

The post notes attacker infrastructure leveraging trusted platforms (e.g., Railway.com, Cloudflare, DigitalOcean) and compromised legitimate domains to blend in with normal enterprise traffic.

Example IP ranges listed:

- 160.220.232.0 (Railway.com)
- 160.220.234.0 (Railway.com)
- 89.150.45.0 (HZ Hosting)
- 185.81.113.0 (HZ Hosting)

## References

- [Storm-2372 conducts device code phishing campaign](https://www.microsoft.com/en-us/security/blog/2025/02/13/storm-2372-conducts-device-code-phishing-campaign/)
- [OAuth 2.0 device authorization grant flow (Microsoft Learn)](https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-device-code)
- [Huntress: Threat Actors Abuse Railway.com PaaS as Microsoft 365 Token Attack Infrastructure](https://www.huntress.com/blog/railway-paas-m365-token-replay-campaign)
- [Sekoia: EvilTokens kit (part 1)](https://blog.sekoia.io/new-widespread-eviltokens-kit-device-code-phishing-as-a-service-part-1/)

## Learn more (as included in the source)

- [Zero Trust for AI workshop](https://microsoft.github.io/zerotrustassessment/)
- [Protect your agents in real-time during runtime (Preview)](https://learn.microsoft.com/en-us/defender-cloud-apps/real-time-agent-protection-during-runtime)
- How to build and customize agents with Copilot Studio Agent Builder (link in source appears malformed and is omitted)
- [Microsoft 365 Copilot AI security documentation](https://learn.microsoft.com/en-us/copilot/microsoft-365/microsoft-365-copilot-ai-security)
- [How Microsoft discovers and mitigates evolving attacks against AI guardrails](https://www.microsoft.com/en-us/security/blog/2024/04/11/how-microsoft-discovers-and-mitigates-evolving-attacks-against-ai-guardrails/)
- [Securing Copilot Studio agents with Microsoft Defender](https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-protection)


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/06/ai-enabled-device-code-phishing-campaign-april-2026/)

