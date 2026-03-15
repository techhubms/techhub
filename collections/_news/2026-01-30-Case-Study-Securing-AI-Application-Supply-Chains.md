---
external_url: https://www.microsoft.com/en-us/security/blog/2026/01/30/case-study-securing-ai-application-supply-chains/
title: 'Case Study: Securing AI Application Supply Chains'
author: Microsoft Defender Security Research Team
primary_section: ai
feed_name: Microsoft Security Blog
date: 2026-01-30 18:49:44 +00:00
tags:
- Agentic Systems
- AI
- AI Framework Exploits
- AI Security
- AI Supply Chain
- Azure
- Cloud Security Posture Management
- CVE 68664
- Defender For Cloud
- LangChain
- LangChain Core
- LangGrinch
- Microsoft Defender
- News
- Python Security
- Remediation Workflows
- SDK Vulnerabilities
- Security
- Serialization Vulnerability
section_names:
- ai
- azure
- security
---
The Microsoft Defender Security Research Team details the LangGrinch (CVE-2025-68664) vulnerability affecting AI supply chains, with actionable guidance for enterprise security using Microsoft Defender tools.<!--excerpt_end-->

# Case Study: Securing AI Application Supply Chains

*By Microsoft Defender Security Research Team*

The rapid adoption of AI applications—spanning agents, orchestrators, and autonomous workflows—marks a new era in software. Modern AI systems act as autonomous agents, making decisions, invoking tools, and interacting with other software on behalf of users. While these capabilities unlock powerful new solutions, they also introduce an expanded and unfamiliar security landscape.

## Beyond Prompt Security: The AI Supply Chain

Security is often discussed at the prompt engineering layer, but a holistic defense strategy must extend deeper, addressing vulnerabilities in the full AI application supply chain: frameworks, SDKs, and orchestration components. Flaws at these lower layers can enable attackers to:

- Influence AI behavior with unexpected inputs
- Access sensitive resources (like environment variables)
- Compromise application runtime environments

The [recent LangGrinch vulnerability (CVE-2025-68664)](https://nvd.nist.gov/vuln/detail/CVE-2025-68664) in LangChain Core highlights these risks and forms the basis of this case study.

## Case Example: Serialization Injection in LangChain (CVE-2025-68664)

LangGrinch is a serialization injection vulnerability impacting the `langchain-core` Python package. Rooted in improper handling of internal metadata fields during object serialization/deserialization, attackers can exploit this flaw to:

- Extract confidential information (e.g., environment secrets)
- Instantiate arbitrary classes (potentially with side effects)
- Trigger malicious initialization code upon object reconstruction

With a [CVSS score of 9.3](https://nvd.nist.gov/vuln/detail/CVE-2025-68664), this vulnerability demonstrates the dangers of weak boundaries between user-controlled data and control signals in AI orchestration platforms.

### The Root Cause: The 'lc' Marker

LangChain distinguishes trusted objects from user data using a reserved serialization key, `lc`. Vulnerable versions of LangChain mishandle this key, allowing injected dictionaries containing `lc` to be treated as trusted objects.

Specifically, the `dumps()` and `dumpd()` functions failed to escape or neutralize the `lc` key when processing user-managed dictionaries. Malicious actors could thus inject objects that the framework would reconstruct unsafely.

## Mitigation and Protection Guidance

Microsoft recommends the following security actions for all organizations using LangChain:

1. **Update LangChain Core to a Patched Version**
   - For 0.3.x: [Upgrade to 0.3.81 or later](https://github.com/langchain-ai/langchain/pull/34455)
   - For 1.x: [Upgrade to 1.2.5 or later](https://github.com/langchain-ai/langchain/pull/34458)

2. **Identify Exposed Assets Using Defender for Cloud Security Explorer**
   - Use [Cloud Security Explorer](https://learn.microsoft.com/en-us/azure/defender-for-cloud/how-to-manage-cloud-security-explorer) to query for instances of LangChain in your environment
   - Identification in cloud compute resources requires D-CSPM / Defender for Containers / Defender for Servers
   - For code environments, connect your environment to Defender for Cloud ([setup guide](https://learn.microsoft.com/azure/defender-for-cloud/quickstart-onboard-github))

3. **Remediate with Defender for Cloud Recommendations**
   - Address issues throughout the software development cycle: Code, Ship, Runtime
   - Integrate security recommendations directly into development workflows

4. **Automate Remediation via GitHub Integration and Copilot Coding Agent**
   - Create GitHub issues from Defender for Cloud with runtime context
   - Track remediation and use Copilot agent for AI-assisted fixes
   - Learn more about [Defender for Cloud’s GitHub integration](https://learn.microsoft.com/en-us/azure/defender-for-cloud/github-advanced-security-overview)

## Defender XDR Detection and Threat Hunting

Microsoft Defender’s security products offer several ways to defend against exploitation:

- **Visibility**: Use Cloud Security Posture Management ([CSPM overview](https://learn.microsoft.com/en-us/azure/defender-for-cloud/concept-cloud-security-posture-management)) to detect vulnerable AI workloads
- **Vulnerability Assessment**: Defender for Cloud scans containers and VMs for unsafe `langchain-core` versions
- **Hunting Queries**: Use Kusto Query Language (KQL) in Defender XDR to hunt for active exploitation (e.g., Python processes accessing secrets after an LLM interaction)

Example KQL snippet for identifying affected machines:

```kql
DeviceTvmSoftwareInventory | where SoftwareName has "langchain" and ( // Lower version ranges SoftwareVersion startswith "0." and toint(split(SoftwareVersion, ".")[1])
```

## References

- [NVD - CVE-2025-68664](https://nvd.nist.gov/vuln/detail/CVE-2025-68664)
- [LangGrinch Postmortem by Cyata](https://cyata.ai/blog/langgrinch-langchain-core-cve-2025-68664/)
- [LangChain Patch PR #34458](https://github.com/langchain-ai/langchain/pull/34458)
- [What is Cloud Security Posture Management (CSPM)](https://learn.microsoft.com/en-us/azure/defender-for-cloud/concept-cloud-security-posture-management)
- [Build queries with Cloud Security Explorer](https://learn.microsoft.com/en-us/azure/defender-for-cloud/how-to-manage-cloud-security-explorer)

## Further Reading

- [Securing Copilot Studio Agents with Microsoft Defender](https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-protection)
- [Protect AI Agents in Real-Time at Runtime](https://learn.microsoft.com/en-us/defender-cloud-apps/real-time-agent-protection-during-runtime)
- [Customize Agents with Copilot Studio Agent Builder](https://learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/copilot-studio-agent-builder)

*Contributors: Tamer Salman, Astar Lev, Yossi Weizman, Hagai Ran Kestenberg, Shai Yannai (Microsoft Defender Security Research)*

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2026/01/30/case-study-securing-ai-application-supply-chains/)
