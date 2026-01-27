---
external_url: https://www.microsoft.com/en-us/security/blog/2025/10/09/investigating-targeted-payroll-pirate-attacks-affecting-us-universities/
title: Investigating Storm-2657 'Payroll Pirate' Attacks Targeting US Universities
author: Microsoft Threat Intelligence
feed_name: Microsoft Security Blog
date: 2025-10-09 15:00:00 +00:00
tags:
- Audit Logs
- CloudAppEvents
- Defender XDR
- Duo MFA
- Entra ID
- Exchange Online
- Identity Protection
- Inbox Rules
- Incident Response
- MFA
- Microsoft Defender For Cloud Apps
- Microsoft Sentinel
- Microsoft Threat Intelligence
- Passwordless Authentication
- Payroll Pirate
- Phishing
- SaaS Security
- Security Copilot
- Storm 2657
- Threat Hunting
- University Security
- Workday
section_names:
- azure
- security
primary_section: azure
---
Microsoft Threat Intelligence details how Storm-2657 targets university employees to hijack payroll accounts, using phishing and SaaS compromise techniques. The report by Microsoft exposes tactics and offers actionable security guidance.<!--excerpt_end-->

# Investigating Storm-2657 'Payroll Pirate' Attacks Targeting US Universities

Microsoft Threat Intelligence has tracked a financially motivated threat actor known as Storm-2657, responsible for compromising employee accounts across US universities to redirect salary payments to attacker-controlled accounts—a scheme dubbed “payroll pirate.”

## Attack Summary

- **Targets**: Higher education sector, specifically employees using HR SaaS platforms such as Workday.
- **Techniques**: Social engineering and advanced phishing emails (including adversary-in-the-middle links) to bypass MFA and hijack employee profiles.
- **Access Flow**:
  - Phishing emails crafted to steal credentials and MFA codes, including those leveraging Google Docs for evasion.
  - Compromised accounts used to spread further attacks within and across academic organizations.
  - Attackers modify Workday profiles, update payroll/bank account info, and create Exchange Online inbox rules to hide notification emails.
  - Persistence achieved by enrolling attacker-controlled MFA devices.

![Attack chain diagram](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2025/10/Fig1-attack-chain.webp)

## Observed Campaign Tactics

- University employees received realistic phishing emails with urgent themes:
  - Illness exposure notifications (COVID-like case, communicable illness)
  - Faculty misconduct and compliance notices
  - HR compensation and benefits updates, often impersonating official entities
- Attackers exploited both lack of MFA and weak MFA implementations.
- Inbox rules with generic or obfuscated names used to hide critical Workday notification emails.

## Technical Detection and Mitigation

- **Microsoft Defender for Cloud Apps & Exchange Online**: Correlate signals between Workday and Exchange to detect:
  - New inbox rules linked to payroll modification
  - Email deletion events (SoftDelete, HardDelete, MoveToDeletedItems)
  - Workday logs: “Change My Account”, “Manage Payment Elections”
- **Microsoft Sentinel**: Use Workday connector and TI Mapping analytics to match indicators and track inbox rule manipulation and risky sign-ins involving new or unknown MFA devices.
- **Microsoft Defender XDR**: Integrated defense and hunting queries for:
  - Compromised user accounts
  - Inbox rule creation/removal
  - Payroll changes
  - Device additions (Add iOS/Android Device)
  - Bulk phishing attempts from .edu domains

### Example KQL Queries

```kql
CloudAppEvents | where Timestamp >= ago(1d) | where Application == "Microsoft Exchange Online" and ActionType in ("New-InboxRule", "Set-InboxRule") | where Parameters has "From" and Parameters has "@myworkday.com" | where Parameters has "DeleteMessage" or Parameters has ("MoveToFolder")
```

```kql
CloudAppEvents | where Timestamp >= ago(1d) | where Application == "Workday" | where ActionType == "Change My Account" or ActionType == "Manage Payment Elections"
```

## Mitigation Guidance

1. **Enforce phishing-resistant MFA** for privileged and sensitive roles using options like FIDO2 keys, Windows Hello for Business, and Microsoft Authenticator passkeys.
2. **Reset credentials** and revoke compromise tokens immediately upon detection.
3. **Review and remove unauthorized MFA device registrations and inbox rules.**
4. **Revert changes to payroll/bank info** and notify affected teams.
5. **Monitor for suspicious activity** with Microsoft Defender, Sentinel, and Security Copilot.

## Microsoft Defender XDR Coverage

| Tactic                 | Observed Activity                                       | Defender Coverage                                    |
|------------------------|--------------------------------------------------------|------------------------------------------------------|
| Initial Access         | Phishing, credential compromise                        | Defender for Office 365, Defender XDR                |
| Defense Evasion        | Inbox rule manipulation, email deletion                | Defender for Cloud Apps                              |
| Impact                 | Payroll/config changes in Workday                      | Defender for Cloud Apps                              |

## Further Resources

- [Workday security guidance](https://community.workday.com/alerts/customer/1229867)
- [Microsoft Threat Intelligence Blog](https://aka.ms/threatintelblog)
- [Defender XDR: Security Copilot](https://learn.microsoft.com/defender-xdr/security-copilot-in-microsoft-365-defender)
- [Phishing-resistant MFA guidance](https://learn.microsoft.com/entra/identity/authentication/how-to-deploy-phishing-resistant-passwordless-authentication)
- [Microsoft Sentinel Marketplace](https://azuremarketplace.microsoft.com/en-us/marketplace/apps/azuresentinel.azure-sentinel-solution-workday?tab=overview)

## Acknowledgments

Thanks to Workday for partnership and collaboration in mitigating this threat. See their official community advisories [here](https://community.workday.com/alerts/customer/1229867).

## About the Author

This investigation is provided by Microsoft Threat Intelligence, which continuously tracks and responds to global adversary campaigns impacting the Microsoft ecosystem.

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/10/09/investigating-targeted-payroll-pirate-attacks-affecting-us-universities/)
