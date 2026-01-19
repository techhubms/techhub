---
external_url: https://techcommunity.microsoft.com/t5/exchange-team-blog/what-is-direct-send-and-how-to-secure-it/ba-p/4439865
title: What is Direct Send and How to Secure It in Exchange Online
author: The_Exchange_Team
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-08-04 16:06:17 +00:00
tags:
- Advanced Hunting
- Defender For Office 365
- Direct Send
- Email Quarantine
- Exchange Online
- Inbound Connectors
- Mail Flow
- Microsoft 365
- PowerShell
- Threat Explorer
- Transport Rules
section_names:
- azure
---
Authored by The Exchange Team, this article delves into how Direct Send operates in Exchange Online, and offers practical guidance for organizations aiming to secure mail flow and monitor messages delivered without custom connectors.<!--excerpt_end-->

## Understanding Direct Send and Securing Mail Flow in Exchange Online

### How Does Mail Flow Work in Exchange Online?

Exchange Online is a Microsoft 365 cloud service allowing organizations to host mail services with tenant-based administration. Every tenant receives a default inbound connector for accepting mails from any external domain, which is not visible or configurable by tenant administrators. For customized mail routing or compliance, admins can create custom connectors and mail flow rules.

**References:**

- [Configure mail flow using connectors in Exchange Online](https://learn.microsoft.com/en-us/exchange/mail-flow-best-practices/use-connectors-to-configure-mail-flow/use-connectors-to-configure-mail-flow)
- [Mail flow rules (transport rules) in Exchange Online](https://learn.microsoft.com/en-us/exchange/security-and-compliance/mail-flow-rules/mail-flow-rules)

### What is Direct Send?

Direct Send refers to sending emails directly to the tenant’s default inbound connector using the domain-specific endpoint (e.g., `contoso-com.mail.protection.outlook.com`). This allows mail delivery even if MX records aren’t pointing to Microsoft 365, and mail bypasses any custom inbound connectors.

Direct Send detailed documentation: [Set up a multifunction device or application to send email via Microsoft 365](https://learn.microsoft.com/en-us/exchange/mail-flow-best-practices/how-to-set-up-a-multifunction-device-or-application-to-send-email-using-microsoft-365-or-office-365#direct-send-send-mail-directly-from-your-device-or-application-to-microsoft-365-or-office-365)

### Securing Direct Send

When MX records point to different services, organizations may want to block or limit Direct Send delivery to their tenant. The recommended action is to configure the service to reject messages that haven’t come through defined inbound connectors, thus ensuring only authorized sources reach the tenant.

#### Steps to Secure Direct Send

1. **Create an Inbound Connector:** Configure the connector to restrict accepted messages by domain certificate (`RestrictDomainsToCertificate`) or by IP address ranges (`RestrictDomainsToIPAddresses`).
2. **Enable 'Reject Direct Send':** This feature rejects email where the envelope sender domain matches an accepted domain in the tenant unless delivered through a configured connector.

More instructions can be found in the [mail flow best practices for third-party cloud services](https://learn.microsoft.com/en-us/exchange/mail-flow-best-practices/manage-mail-flow-using-third-party-cloud) and [Introducing more control over Direct Send in Exchange Online](https://techcommunity.microsoft.com/blog/exchange/introducing-more-control-over-direct-send-in-exchange-online/4408790).

#### Alternative to Lockdown: Quarantining Unknown Emails

If outright rejection of unexpected mail traffic is a concern, admins can use transport (mail flow) rules to quarantine or redirect mail not sent from known, approved IP addresses.

**Sample Mail Flow Rule:**

- **Apply if:** To all messages
- **Do the following:** Deliver message to hosted quarantine, stop processing further rules
- **Except if:** Sender IP matches approved ranges (MX records, on-premises IPs, other authorized IPs) OR 'X-MS-Exchange-Organization-AuthAs' header is 'Internal'

**PowerShell Example:**

```powershell
New-TransportRule -Name "Redirect to quarantine if not coming from known IPs" -Quarantine $true -ExceptIfHeaderContainsMessageHeader 'X-MS-Exchange-Organization-AuthAs' -ExceptIfHeaderContainsWords 'Internal' –ExceptIfSenderIpRanges ‘MX records + on-premises IPs + other authorized IPs ' -StopRuleProcessing $true -Priority 0
```

Organizations should adapt the rules to their existing configurations as needed.

### Monitoring Direct Send Usage

#### Option 1: Historical Message Trace

Run a PowerShell command to report on inbound email received without a connector for up to 90 days:

```powershell
Start-HistoricalSearch -ReportTitle DirectSendMessages -StartDate <start> -EndDate <end> -ReportType ConnectorReport -ConnectorType NoConnector -Direction Received -NotifyAddress admin@contoso.com
```

Alternatively, use the Exchange Admin Center's "Inbound messages" report.

#### Option 2: Defender for Office 365

If you have a Defender for Office P2 subscription, use Threat Explorer with filters for inbound emails not delivered by any connector or use Advanced Hunting:

```kql
EmailEvents
| where Timestamp > ago(30d)
| where EmailDirection == 'Inbound' and Connectors == '' and isnotempty(SenderIPv4)
| project Timestamp, RecipientEmailAddress, SenderFromAddress, Subject, NetworkMessageId, EmailDirection, Connectors, SenderIPv4, SenderIPv6
```

### Summary

By understanding and configuring Direct Send, organizations using Exchange Online can safeguard their mail flow, minimize risks from unauthorized email, and maintain tighter control with mail flow rules and monitoring tools provided by Microsoft 365.

---

*Microsoft 365 Messaging Team*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/exchange-team-blog/what-is-direct-send-and-how-to-secure-it/ba-p/4439865)
