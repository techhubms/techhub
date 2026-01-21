---
external_url: https://techcommunity.microsoft.com/t5/exchange-team-blog/released-august-2025-exchange-server-security-updates/ba-p/4441596
title: August 2025 Exchange Server Security Updates Released
author: The_Exchange_Team
feed_name: Microsoft Tech Community
date: 2025-08-12 17:48:04 +00:00
tags:
- AMSI Integration
- August Update
- Cumulative Update (cu)
- CVE 53786
- Exchange Management Tools
- Exchange Server
- Exchange Server Subscription Edition
- Health Checker Script
- Hybrid Deployment
- Microsoft Security
- On Premises Exchange
- Security Update
- SetupAssist Script
- Vulnerability
section_names:
- security
---
The_Exchange_Team explains the August 2025 Security Updates for Exchange Server, offering guidance on installation, new AMSI features, update paths, and security best practices for on-premises and hybrid Exchange environments.<!--excerpt_end-->

# August 2025 Exchange Server Security Updates Released

Microsoft has published Security Updates (SUs) for the following Exchange Server versions:

- Exchange Server Subscription Edition (SE)
- Exchange Server 2019
- Exchange Server 2016

## Available Update Versions

- **Exchange SE (RTM):** [Download link](https://www.microsoft.com/en-us/download/details.aspx?id=108335)
- **Exchange Server 2019 Cumulative Updates:** [CU14](https://www.microsoft.com/en-us/download/details.aspx?id=108336), [CU15](https://www.microsoft.com/en-us/download/details.aspx?id=108334)
- **Exchange Server 2016 Cumulative Update:** [CU23](https://www.microsoft.com/en-us/download/details.aspx?id=108333)

> **Note:** If the English download pages show “Hotfix Update” rather than “Security Update,” this is only a label issue and does not affect the actual update. Downloads will remain available to avoid confusion.

## Vulnerabilities Addressed

The August 2025 SUs resolve vulnerabilities identified by both security partners and Microsoft’s internal processes. There are currently no reported active exploits, but immediate installation is strongly recommended to protect your environment.

- For CVE details, see the [Security Update Guide](https://msrc.microsoft.com/update-guide/) (filter by Server Software).

Exchange Online customers are already protected; these updates apply to on-premises servers and management tool workstations.

## AMSI Body Scanning Enabled by Default

Beginning with the [November 2024 Security Update](https://support.microsoft.com/help/5044062), Exchange integrated the Antimalware Scan Interface (AMSI) for scanning HTTP message bodies. As of the August 2025 SU, AMSI body scanning is enabled by default for all protocols.

- If you notice degraded performance after the update, consult the [AMSI integration documentation](https://aka.ms/ExchangeAMSI#disable-exchange-server-amsi-body-scanning) for guidance on disabling AMSI body scanning.

## Update Installation Guidance

1. **Inventory Your Servers:** Run the [Exchange Server Health Checker script](https://aka.ms/ExchangeHealthChecker) to determine which updates are needed and if any servers are behind.
2. **Install the Latest CU:** Use the [Exchange Update Wizard](https://aka.ms/ExchangeUpdateWizard) to plan your upgrade path and execute the update process.
3. **Re-run Health Checker:** After updating, run the Health Checker again to confirm no further actions are required.
4. **Troubleshooting:**
   - For errors during or after installation, use the [SetupAssist script](https://aka.ms/ExSetupAssist).
   - See [Repair failed installations of Exchange Cumulative and Security updates](https://aka.ms/ExchangeFAQ) or [File version error guidance](https://support.microsoft.com/en-us/topic/file-version-error-when-you-try-to-install-exchange-server-november-2024-su-a650da30-f8fb-469d-a449-47396cab0a15) if issues arise.

## Frequently Asked Questions (FAQs)

**Q: Does the August 2025 update address [CVE-2025-53786](https://portal.msrc.microsoft.com/security-guidance/advisory/CVE-2025-53786)?**

- **A:** Yes, all SUs are cumulative. August 2025 SUs include the necessary components. For hybrid deployments, refer to [Exchange Server Security Changes for Hybrid Deployments](https://techcommunity.microsoft.com/blog/exchange/exchange-server-security-changes-for-hybrid-deployments/4396833).

**Q: Do hybrid organizations need to act?**

- **A:** Yes, install SUs on all on-premises Exchange Servers, including management-only servers. If you change the authentication certificate after updating, re-run the Hybrid Configuration Wizard.

**Q: Must all older SUs be installed in sequence?**

- **A:** No, install the latest SU only if your CU is supported.

**Q: Should SUs be installed on all Exchange Servers and management tools machines?**

- **A:** Yes, to maintain compatibility. For management tools-only environments without Exchange servers, refer to [this document](https://learn.microsoft.com/en-us/exchange/manage-hybrid-exchange-recipients-with-management-tools#update-the-exchange-server-management-tools-only-role-with-no-running-exchange-server-to-a-newer-cumulative-or-security-update).

## Additional Information

- Documentation may not be immediately available at the time of publication; watch for updates in this post.

---

*Authored by The_Exchange_Team ([Profile](https://techcommunity.microsoft.com/users/the_exchange_team/324116))*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/exchange-team-blog/released-august-2025-exchange-server-security-updates/ba-p/4441596)
