-- Migration 034: Roundup INSERTs for section 'security'
-- Generated: 2026-05-21 from localhost database (AI-regenerated metadata).
-- Safe to re-run: ON CONFLICT DO UPDATE overwrites all fields with source-of-truth values.

-- ── content_items ─────────────────────────────────────────────────────────────────
-- weekly-security-roundup-2026-05-11
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-05-11', 'roundups', 'Weekly Security Roundup: Passkeys, Token Theft, and Code-to-Cloud',
    'Security updates this week landed in three places developers feel immediately: identity (with more passkey momentum and new token-theft campaign details), software supply chain (with tighter code-to-cloud visibility and new scanning options that work in agent-driven workflows), and infrastructure hardening (from open-sourcing HSM components to active Linux exploitation and stronger data platform controls). Coming right after last week''s theme of shrinking ambient privilege and interrupting intrusion chains with automation, this week''s items largely zoom in on the same question from different angles: once an attacker gets a foothold (or once risky code ships), how quickly can you detect it, bound it, and prove what happened.
<!--excerpt_end-->
## Microsoft Defender threat research: token theft, macOS infostealers, and active Linux exploitation
Microsoft security researchers mapped out multiple active campaigns that target the gaps between "user is authenticated" and "attacker can operate as the user", with a heavy focus on stealing tokens or escalating privileges after initial access. That builds directly on last week''s token-centric identity framing and the Defender XDR incident writeups: the attacker goal stays the same (operate as a real user, move laterally, exfiltrate), but the tradecraft varies depending on what is easiest to reuse (session artifacts, interactive access, or local privilege escalation).
One investigation broke down a large-scale "code of conduct" themed phishing operation that uses an adversary-in-the-middle (AiTM) flow to capture authentication tokens, which can bypass MFA by replaying tokens and session cookies rather than brute-forcing passwords. If last week showed how hands-on access via "remote help" tools can bypass the phishing-vs-MFA debate entirely, this week is the more classic "steal the session and skip the password" story, with the same operational implication: you need identity telemetry and fast response paths for session abuse, not just better password policy. The write-up pairs the attack chain with practical response material, including Defender detections, Microsoft Defender for Office 365 guidance, Microsoft Entra ID Protection recommendations, Microsoft Defender XDR coverage, and Advanced Hunting queries plus IOCs so security teams can validate whether the campaign reached their tenants.
On endpoints, Microsoft detailed updated ClickFix-style social engineering on macOS where the "payload" starts with the user copying and pasting attacker-provided Terminal commands. The report outlines multiple campaign variants, how persistence is established (including LaunchAgents and LaunchDaemons), how command-and-control infrastructure is discovered, and how infostealers may progress into wallet trojanization. This is the same "attackers win when normal workflows get abused" theme that ran through last week''s Quick Assist intrusion chain, just shifted to macOS and developer-style muscle memory (Terminal). For defenders, the value is in the concrete hunting and detection guidance (including Microsoft Defender for Endpoint KQL queries) and the extensive IOC set to speed up triage.
The most urgent infrastructure note was an "active attack" advisory for the "Dirty Frag" Linux local privilege escalation technique, expanding the risk after a system is already compromised by giving attackers a way to jump to higher privileges. That complements last week''s emphasis on cutting off the middle of the chain (lateral movement and credential abuse) by calling out another mid-chain accelerant: privilege escalation that turns a limited foothold into broader control. Microsofts coverage calls out affected components (including esp4/esp6 and rxrpc) and provides interim mitigation steps, along with Microsoft Defender detection coverage so teams can both reduce exposure and monitor for exploitation attempts in the wild.
- [Breaking the code: Multi-stage ‘code of conduct’ phishing campaign leads to AiTM token compromise](https://www.microsoft.com/en-us/security/blog/2026/05/04/breaking-the-code-multi-stage-code-of-conduct-phishing-campaign-leads-to-aitm-token-compromise/)
- [ClickFix campaign uses fake macOS utilities lures to deliver infostealers](https://www.microsoft.com/en-us/security/blog/2026/05/06/clickfix-campaign-uses-fake-macos-utilities-lures-deliver-infostealers/)
- [Active attack: Dirty Frag Linux vulnerability expands post-compromise risk](https://www.microsoft.com/en-us/security/blog/2026/05/08/active-attack-dirty-frag-linux-vulnerability-expands-post-compromise-risk/)
## GitHub + Defender for Cloud: bringing runtime context into code security, and scanning that works with AI agents
GitHubs security surface continued to shift toward "developer-first, but deployment-aware" workflows, starting with the general availability of code-to-cloud risk visibility via Microsoft Defender for Cloud integration with GitHub Advanced Security. This continues last week''s supply chain focus (know what you built, limit blast radius when dependencies go bad) by extending the visibility story past CI and into production reality. The core change is correlation: teams can connect what shipped (deployed container artifacts) with what was known during development, then see runtime risk context directly inside GitHub security views. Practically, that means security and platform teams can triage findings with more signal (what is actually running, where, and with what risk context) instead of treating code findings as isolated from production. The GA update also adds runtime-aware filters and campaign targeting across code scanning and Dependabot, which helps teams focus remediation efforts on what is deployed rather than what is merely present in a repo.
In parallel, GitHub expanded what its MCP Server (Model Context Protocol) can do for security in agent-driven development. This picks up the thread from last week on governing agent tool execution (MCP control planes and per-call policy enforcement) by showing how security checks are moving into the same agent tool boundary where code is increasingly being proposed and edited. Secret scanning via GitHub MCP Server is now generally available, enabling MCP-compatible IDEs and AI coding agents to detect exposed secrets before commits or pull requests. A key detail for teams already using GitHub Advanced Security is consistency: the GA release honors existing push protection customization, so detection rules and bypass behavior remain aligned across standard GitHub workflows and MCP-driven tooling. Alongside that GA, dependency scanning in GitHub MCP Server entered public preview, letting AI coding agents and MCP-compatible IDEs check proposed changes for vulnerable dependencies using the Dependabot toolset and the GitHub Advisory Database before the change becomes a commit or PR. Taken together, these updates push scanning earlier in the loop (inside the editor and agent workflow) while keeping enterprise policies coherent across the "human PR" and "agent-assisted change" paths.
- [Code-to-cloud risk visibility with Microsoft Defender for Cloud is now generally available](https://github.blog/changelog/2026-05-05-code-to-cloud-risk-visibility-with-microsoft-defender-for-cloud-is-now-generally-available)
- [Secret scanning with GitHub MCP Server is now generally available](https://github.blog/changelog/2026-05-05-secret-scanning-with-github-mcp-server-is-now-generally-available)
- [Dependency scanning with GitHub MCP Server is in public preview](https://github.blog/changelog/2026-05-05-dependency-scanning-with-github-mcp-server-is-in-public-preview)
## Microsoft identity and passwordless: passkeys progress and recovery changes
Microsoft used World Passkey Day to summarize incremental but meaningful changes across Microsoft Entra ID, Windows, and consumer sign-in as passwordless adoption expands from "sign-in" into "sign-in plus recovery." This is a clean continuation of last week''s identity-first theme (reducing what attackers can steal and reuse) because recovery paths and helpdesk flows are where "passwordless" programs often get undermined in practice. The update highlights general availability improvements to Entra ID account recovery, which matters because recovery paths often become the weak link once primary authentication is hardened. Microsoft also reiterated a notable cleanup item: it plans to remove security questions as a password reset option starting January 2027, reducing reliance on low-signal knowledge-based answers that are frequently guessable, reused, or obtainable through social engineering. For teams rolling out FIDO2/passkeys, the practical takeaway is to treat recovery and helpdesk flows as part of the rollout plan, not as an afterthought.
- [World Passkey Day: Advancing passwordless authentication](https://www.microsoft.com/en-us/security/blog/2026/05/07/world-passkey-day-advancing-passwordless-authentication/)
## Hardware and data platform controls: open HSM components and OneLake security GA
On the "trust the platform" side, Azure announced it is open-sourcing Azure Integrated HSM through the Open Compute Project, including firmware and supporting software plus independent validation artifacts. Paired with last week''s emphasis on reducing exfil paths (for example, Fabric outbound access protection) and tightening identity boundaries, this is the lower-layer counterpart: if keys anchor your identity, encryption, and signing systems, then assurance in the HSM implementation becomes part of the overall "can we trust the control plane under pressure" story. The goal is verifiable key protection at scale for server-integrated hardware security modules (HSMs) that complement Azure Key Vault and Azure Managed HSM. The post frames this as a transparency and assurance move: by publishing artifacts and aligning with OCP SAFE, Azure enables deeper third-party scrutiny of how keys are protected by hardware-enforced controls, including the kind of assurance customers look for in regulated environments (the post calls out FIPS 140-3 Level 3). For organizations building stronger cryptographic trust chains, this is a reminder that key management is not only about API usage, but about attestation, validation evidence, and the ability to verify the underlying system design.
In Microsoft Fabric, OneLake security reached general availability with default enablement and an automatic upgrade rollout running through May. This follows last week''s Fabric security arc (better controls at the boundary and clearer enforcement points for data movement) by tightening governance inside the lake itself: who can see which rows and columns, and how quickly teams can validate and automate those permissions. The GA focuses on making governance usable at scale: UI improvements, inline row-level security (RLS) validation, a role creation wizard that supports RLS and column-level security (CLS) authoring, and more granular REST APIs for role management. For teams using OneLake mirroring or consolidating data access patterns in Fabric, the practical impact is faster iteration on least-privilege role design (via the wizard and validation) and better automation hooks (via the new APIs) to keep permissions consistent across environments.
- [Enforcing trust and transparency: Open-sourcing the Azure Integrated HSM](https://azure.microsoft.com/en-us/blog/enforcing-trust-and-transparency-open-sourcing-the-azure-integrated-hsm/)
- [OneLake security (Generally Available)](https://community.fabric.microsoft.com/t5/Fabric-Updates-Blogs/OneLake-security-Generally-Available/ba-p/5176756)
## Other Security News
Inspektor Gadget published the results of its first independent security audit, patching three vulnerabilities (including CVE-2026-24905 and CVE-2026-25996) and documenting hardening recommendations. Coming right after last week''s blend of "supply chain plus operational guardrails", this is a useful reminder that observability and inspection tooling needs the same scrutiny as the workloads it monitors, especially when it hooks deeply into Linux and Kubernetes through eBPF. For teams using eBPF-based inspection in Kubernetes and on Linux hosts, the report is useful both as a validation point and as a practical checklist for tightening RBAC and deployment posture.
- [Inspektor Gadget Completes Its First Independent Security Audit](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/inspektor-gadget-completes-its-first-independent-security-audit/ba-p/4517895)',
    'Security updates this week landed in three places developers feel immediately: identity (with more passkey momentum and new token-theft campaign details), software supply chain (with tighter code-to-cloud visibility and new scanning options that work in agent-driven workflows), and infrastructure hardening (from open-sourcing HSM components to active Linux exploitation and stronger data platform controls). Coming right after last week''s theme of shrinking ambient privilege and interrupting intrusion chains with automation, this week''s items largely zoom in on the same question from different angles: once an attacker gets a foothold (or once risky code ships), how quickly can you detect it, bound it, and prove what happened.',
    1778482800, 'security', '/security/roundups/weekly-security-roundup-2026-05-11', 'TechHub',
    'TechHub', '0F63ECDF8AEC2C1888B8D3BBBCBB42308F84EC8C711E915B45C4901AE4B84B54', ',Microsoft Defender,Defender XDR,Microsoft Entra ID,Passkeys,FIDO2,AiTM Phishing,Token Theft,Macos Infostealers,Linux Privilege Escalation,Dirty Frag,GitHub Advanced Security,Microsoft Defender For Cloud,Code Scanning,Secret Scanning,Dependabot,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-05-04
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-05-04', 'roundups', 'Weekly Security Roundup: CVE Response and AI Agent Governance',
    'Security news this week focused on two parallel pressures teams are feeling right now: urgent patch-and-harden work for high-impact vulnerabilities in core dev and runtime infrastructure, and the fast-moving reality that AI agents are becoming part of the attack surface. Across Microsoft and GitHub updates, the practical theme was governance (who can call what, when, and with what audit trail) paired with stronger identity and data protections that reduce blast radius when something does go wrong. That threads cleanly into last week''s direction: reduce ambient privilege, remove long-lived secrets, and make secure defaults workable at scale, because when an incident starts from "normal" workflows, your margin often comes from consistent guardrails and fast containment.
<!--excerpt_end-->
## GitHub and Linux: High-impact CVEs that hit the developer pipeline and cloud workloads
GitHub disclosed and remediated a critical remote code execution issue in the `git push` path (CVE-2026-3854), rooted in unsanitized push options being written into internal metadata. The key takeaway for teams running GitHub Enterprise Server is that this is not just a theoretical edge case in Git plumbing. Push options can be supplied by clients during normal workflows, so administrators need to treat the update as an urgent pipeline security fix: patch quickly, then follow GitHub''s operational guidance to review logs for suspicious activity and validate that the mitigations are in place across all nodes. In practice, it is the same "trusted workflow abuse" lesson we saw last week in the Teams/Quick Assist intrusion research: the entry point can look like normal user or developer behavior, so hardening and detection around everyday paths matters as much as perimeter controls.
On the runtime side, Microsoft detailed a high-severity Linux kernel local privilege escalation dubbed "Copy Fail" (CVE-2026-31431) that can enable root escalation across cloud environments, including Kubernetes-heavy deployments. The write-up highlights how the vulnerability can matter even when perimeter controls look good, because an attacker who lands code execution in a container or workload may be able to escalate locally via the kernel, then pivot further. Microsoft pairs the disclosure with mitigation guidance and Microsoft Defender XDR detections so security teams can hunt for exploitation signals while patching rolls out (especially important for fleets spanning multiple distros and managed Kubernetes nodes). This complements last week''s identity-first and "tighten trust boundaries" theme: Workload Identity and OIDC reduce credential theft risk, but kernel and platform patching still determines how far an attacker can go once they are inside.
- [Securing the git push pipeline: Responding to a critical remote code execution vulnerability](https://github.blog/security/securing-the-git-push-pipeline-responding-to-a-critical-remote-code-execution-vulnerability/)
- [CVE-2026-31431: Copy Fail vulnerability enables Linux root privilege escalation across cloud environments](https://www.microsoft.com/en-us/security/blog/2026/05/01/cve-2026-31431-copy-fail-vulnerability-enables-linux-root-privilege-escalation/)
## Governing AI agents and Copilot extensibility: shifting controls left, and dealing with the gaps
Several posts this week converged on the same uncomfortable point: once you let agentic tools call external "tools" (shell commands, cloud APIs, ticketing systems, MCP servers), you have effectively created a new integration surface that needs the same rigor as any production API. The Agent Governance Toolkit (AGT) for .NET tackles this directly for Model Context Protocol (MCP) tool calls by adding policy-based controls, scanning tool definitions, sanitizing responses, and producing audit and telemetry aligned to the OWASP MCP Top 10. For .NET teams building or embedding agents, the value is that governance can live in the same place you already instrument services, including OpenTelemetry pipelines, rather than being an afterthought bolted onto the agent runtime. This is a direct continuation of last week''s agent security thread (for example, the Secure Code Game''s "ProdBot" scenarios): the threat model is not abstract prompt injection anymore, it is tool access, memory, and cross-system side effects.
The companion "shift-left" governance guidance makes AGT feel less like a runtime gate and more like a SDLC pattern: enforce pre-commit hooks and pull request gates, verify governance in CI, and carry controls through release with artifacts like SBOMs, signing, and provenance attestations. That approach addresses a recurring failure mode with agent integrations: teams discover risky tool wiring only after an incident or after an agent starts acting on production systems. It also mirrors last week''s broader supply-chain operations push (SBOM export reliability, org-wide scanning baselines): governance only works when it is repeatable, automated, and visible across the estate.
At the same time, an independent analysis of GitHub Copilot extensibility surfaces lays out where enterprise controls can fall short in practice. It walks through five extension points (Copilot CLI plugins, Microsoft APM, `gh skill`, MCP servers, and VS Code extensions) and shows how governance can be bypassed depending on how plugin sources are pinned, where policy files are enforced, and what is (or is not) audited. The most actionable parts are the mitigation patterns: pin and lock plugin sources, apply strict policy configurations where supported, and treat "extension registries" as a supply chain surface that needs explicit allowlists and monitoring. That is the same operational reality last week highlighted in a different form: "secure defaults" and centralized controls help, but you still need to find the escape hatches (forks, plugins, unmanaged tools) and close them with policy plus monitoring.
Finally, Microsoft''s 1ES team described how they are using agentic AI internally to reduce time-to-remediate for CVEs and compliance work while keeping humans accountable for review and deployment. Their approach is intentionally operational: GitHub Copilot CLI plus reusable Markdown "skills" to standardize work, and "agent signals" to help track what the agent did and why. It is a useful model for organizations trying to scale remediation without turning agents into unreviewed automation, and it connects directly to the governance story: the more you delegate, the more you need consistent guardrails and traceability. It also pairs with last week''s AI incident response guidance: if you cannot reconstruct actions and decisions from telemetry, you cannot contain fast or learn reliably.
- [Governing MCP tool calls in .NET with the Agent Governance Toolkit](https://devblogs.microsoft.com/dotnet/governing-mcp-tool-calls-in-dotnet-with-the-agent-governance-toolkit/)
- [Shift-Left Governance for AI Agents: How the Agent Governance Toolkit Helps You Catch Violations](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/shift-left-governance-for-ai-agents-how-the-agent-governance/ba-p/4516481)
- [Where the GitHub Copilot extension points break governance](https://devopsjournal.io/blog/2026/05/01/Copilot-extension-governance-concerns)
- [How Microsoft 1ES uses agentic AI to take on security and compliance at scale](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-microsoft-1es-uses-agentic-ai-to-take-on-security-and/ba-p/4515191)
## Microsoft security platform and cloud protections: identity-first pipelines, multicloud visibility, and data-in-use safeguards
On the identity and infrastructure side, guidance for Terraform pipelines on Azure pushed a clear best practice: stop relying on long-lived client secrets in CI/CD, and move to OIDC Workload Identity Federation through Microsoft Entra ID for both GitHub Actions and Azure DevOps. The practical implementation details matter here, including using a user-assigned managed identity (UAMI), creating federated identity credentials tied to your CI provider, and tightening RBAC to least privilege. The post also calls out state hardening (where compromise often starts) and migration steps so teams can move incrementally without breaking delivery. This is the same storyline as last week''s GitHub and Azure updates around OIDC (for Dependabot/code scanning registries and AKS Workload Identity): tokenless-by-default pipeline access is becoming the baseline pattern, and the operational work shifts to scoping identities well and monitoring token/session behavior.
In detection and response, Microsoft Sentinel UEBA updates show how teams defending AWS can reduce complexity by enriching AWS CloudTrail with behavioral signals (via the `BehaviorAnalytics` and `Anomalies` tables) so detections can be written in simpler KQL without reconstructing every edge case from raw logs. The examples focus on common attacker paths in AWS environments - federated identity abuse, suspicious IAM changes, secrets access, and S3 exfiltration - and the operational win is faster triage when the system can surface ML-driven anomalies instead of forcing analysts to handcraft brittle thresholds. This follows naturally from last week''s emphasis on "operate it well": better baselines and higher-quality signals reduce the time spent chasing noise, especially when attacker behavior blends into legitimate admin activity.
For data protection, Azure Event Hubs Dedicated added confidential computing support that protects streaming data while it is being processed (data in use) using trusted execution environments (TEEs), and it does so without requiring application changes. The announcement pairs this with practical defense-in-depth steps teams can layer on top: Entra ID for access control, customer-managed keys (CMK) backed by Azure Key Vault Managed HSM, private networking, and Azure Policy to enforce configuration standards across clusters. It also connects back to last week''s cryptography inventory guidance: TEEs and CMK help, but teams still need a clear picture of where keys live, which services use them, and how policy and monitoring prevent drift over time.
- [Modernizing Terraform Pipelines on Azure: OIDC Federation for GitHub Actions and Azure DevOps](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/modernizing-terraform-pipelines-on-azure-oidc-federation-for/ba-p/4516620)
- [Simplifying AWS defense with Microsoft Sentinel UEBA](https://www.microsoft.com/en-us/security/blog/2026/04/28/simplifying-aws-defense-microsoft-sentinel-ueba/)
- [Protect Your Streaming Data in Use: Confidential Computing for Azure Event Hubs Dedicated](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/protect-your-streaming-data-in-use-confidential-computing-for/ba-p/4515219)
## Other Security News
Email threats in Q1 2026 continued to lean on interaction traps rather than malware-heavy payloads, with growth in QR code phishing and CAPTCHA-gated phishing that tries to evade automated scanning. Microsoft''s analysis also covers the impact of Tycoon2FA disruptions on adversary-in-the-middle (AiTM) activity, then maps mitigations and detections across Microsoft Defender for Office 365 and Microsoft Defender XDR, including how Microsoft Security Copilot can support investigation workflows. That fits with last week''s threat research theme: attackers keep winning initial access by abusing trusted UX (collaboration invites, remote help, "prove you''re human" CAPTCHA flows), so the practical response is improved detection plus rehearsed containment and session revocation when users inevitably click through.
Microsoft also shipped a broader set of platform updates, including preview protections for AI agents through the Agent 365 tooling gateway, general availability integration between Defender for Cloud and GitHub Advanced Security, and a new Microsoft Purview Data Security Investigations demo aimed at helping teams validate investigation flows end to end. Agent 365 itself reached general availability, with an emphasis on discovering and governing "shadow AI" agents across endpoints, SaaS, and multicloud, and deeper integrations into Microsoft Defender, Intune, and Entra network controls. This is the platform-level continuation of last week''s agent governance and AI incident response guidance: inventory, policy, and audit trails are moving into the same management planes teams already rely on for identity and endpoint control, which is where governance becomes enforceable rather than advisory.
- [Email threat landscape: Q1 2026 trends and insights](https://www.microsoft.com/en-us/security/blog/2026/04/30/email-threat-landscape-q1-2026-trends-and-insights/)
- [What’s new, updated or recently released in Microsoft Security](https://www.microsoft.com/en-us/security/blog/2026/04/30/whats-new-updated-or-recently-released-in-microsoft-security/)
- [Microsoft Agent 365, now generally available, expands capabilities and integrations](https://www.microsoft.com/en-us/security/blog/2026/05/01/microsoft-agent-365-now-generally-available-expands-capabilities-and-integrations/)',
    'Security news this week focused on two parallel pressures teams are feeling right now: urgent patch-and-harden work for high-impact vulnerabilities in core dev and runtime infrastructure, and the fast-moving reality that AI agents are becoming part of the attack surface. Across Microsoft and GitHub updates, the practical theme was governance (who can call what, when, and with what audit trail) paired with stronger identity and data protections that reduce blast radius when something does go wrong. That threads cleanly into last week''s direction: reduce ambient privilege, remove long-lived secrets, and make secure defaults workable at scale, because when an incident starts from "normal" workflows, your margin often comes from consistent guardrails and fast containment.',
    1777878000, 'security', '/security/roundups/weekly-security-roundup-2026-05-04', 'TechHub',
    'TechHub', 'A8A55B2833D7578987972EB67EFAE4513E6BBB4B1A5E69BBB1E0AA969E9B0476', ',GitHub Enterprise Server,Git,CVE 2026 3854,Linux Kernel,CVE 2026 31431,Kubernetes,Microsoft Defender XDR,MCP,OWASP MCP Top 10,Agent Governance Toolkit,GitHub Copilot,OIDC Workload Identity Federation,Microsoft Entra ID,Microsoft Sentinel UEBA,Confidential Computing,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-04-27
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-04-27', 'roundups', 'Weekly Security Roundup: Intrusion Disruption, AI Guardrails',
    'Security news this week centered on the practical mechanics of stopping real intrusions (before they become full-bore ransomware style incidents), while teams also tightened the supply chain and started putting clearer guardrails around AI agents and data movement. Building on last week''s identity-first framing (tokens, session replay, and shrinking ambient privilege), this week''s stories show what that looks like when an attacker has hands-on access and when defenders can actually interrupt the chain with automation. Microsoft published two detailed Defender Security Research writeups that read like field guides for both attackers and defenders, and several platform updates (from .NET, GitHub, Azure DevOps, and Fabric) landed with concrete steps developers can take right now.
<!--excerpt_end-->
## Microsoft Defender XDR: Real-world intrusion chains and automated containment
Microsoft Defender Security Research published a detailed cross-tenant attack chain that starts with social engineering in Microsoft Teams and ends with data theft, and the value of the writeup is how specific the steps are. In a way, it is the "human-operated" companion to last week''s token-centric AiTM/device-code coverage: instead of stealing a session cookie over phishing infrastructure, the attacker convinces a user to grant interactive access (Quick Assist), then uses that foothold to reach the same outcomes (persistence, lateral movement, and data exfiltration). The attacker poses as helpdesk and uses Quick Assist to get interactive access, then moves to persistence and execution via DLL side-loading, pivots laterally using WinRM (Windows Remote Management), and finally exfiltrates data with Rclone. For defenders, the actionable takeaway is to treat "remote help" tooling and cross-tenant communication paths as first-class attack surface, especially since they can bypass the "phishing vs MFA" debate entirely: review Quick Assist usage policies, tighten who can remote into what, and make sure Defender XDR telemetry is wired up so you can hunt across endpoints, identities, and collaboration activity. Microsoft also shared Microsoft Defender XDR KQL hunting queries designed to catch the chain early (for example, suspicious Quick Assist activity, side-loading indicators, WinRM lateral movement patterns, and Rclone-like exfil behaviors), which makes it easier to turn the narrative into detections instead of just lessons learned.
In a second incident-focused post, the same team walked through a June-July 2025 Active Directory domain compromise and how Defender XDR automatic attack disruption helped contain it. This continues last week''s theme that the fastest wins often come from shortening the window attackers have to reuse access (sessions, tokens, or privileged paths). The key mechanism highlighted was "predictive shielding", which uses exposure-based signals to block sign-ins and reduce lateral movement while an investigation is still unfolding. The practical point for operators is that this is not just about finding credential dumping after the fact. The containment story depends on having identity protection and exposure posture signals available to Defender XDR, then letting automatic disruption take decisive action (blocking sign-ins tied to risky paths) to slow the attacker down long enough for remediation. If you map your controls to MITRE ATT&CK, this writeup is essentially a worked example of disrupting the middle of the chain (lateral movement and credential abuse) rather than focusing only on initial access.
- [Cross-tenant helpdesk impersonation to data exfiltration: A human-operated intrusion playbook](https://www.microsoft.com/en-us/security/blog/2026/04/18/crosstenant-helpdesk-impersonation-data-exfiltration-human-operated-intrusion-playbook/)
- [Containing a domain compromise: How predictive shielding shut down lateral movement](https://www.microsoft.com/en-us/security/blog/2026/04/17/domain-compromise-predictive-shielding-shut-down-lateral-movement/)
## Securing AI agents and detections: Governing MCP tools and benchmarking CTI-to-detection automation
As more teams experiment with AI agents that can call tools, Microsoft published two pieces that focus on control and measurement rather than prompts and vibes. This is a direct continuation of last week''s agent governance thread (AGT architecture, policy engines, identity, isolation, and SRE guardrails), but with a tighter focus on the practical "tool execution boundary" where agents stop being chat and start being production actors.
First, Jack Batzner introduced the Agent Governance Toolkit (AGT), positioned as an open-source "control plane" for governing Model Context Protocol (MCP) tool execution. The core idea is that prompt-only guardrails are not enough once an agent can actually run actions, so AGT focuses on deterministic, per-call policy enforcement and verification: scanning tool definitions, enforcing allow/deny rules at execution time, inspecting tool responses, and recording identity and audit logs so you can answer "who ran what, with which inputs, and what did it return." The post calls out policy engines like OPA/Rego and Cedar Policy, and it references internal red-team results as motivation, which fits last week''s warning that agent capabilities often resemble "root via tools" unless you put hard choke points in front of every call. It is the same general lesson as the identity posts from last week (CAE, Conditional Access, token revocation), applied to agent tooling: reduce implicit trust, make approvals explicit, and keep audit trails usable during incidents.
On the detection side, Microsoft Research introduced CTI-REALM, an open-source benchmark aimed at a very practical question: can AI agents take real threat intelligence and turn it into validated detections (for example, Sigma rules and KQL queries) that actually work in environments like Microsoft Sentinel. This complements last week''s "agentic SOC" framing by grounding it in repeatable evaluation: if agents are going to help write detections, the bar cannot be "it produced a query", it has to be "it produced a query that runs, matches the behavior, and holds up under validation." The notable detail is the focus on validation, not just generation, across scenarios that include Linux, AKS, and broader Azure cloud deployments. For security teams that are evaluating AI-assisted detection engineering, CTI-REALM is useful because it pushes evaluation toward reproducible test cases instead of anecdotal "the agent wrote a rule" success stories.
- [Securing MCP: A Control Plane for Agent Tool Execution](https://devblogs.microsoft.com/blog/securing-mcp-a-control-plane-for-agent-tool-execution)
- [How AI Agents Are Turning Threat Intelligence Into Validated Detections](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/how-ai-agents-are-turning-threat-intelligence-into-validated/ba-p/4513971)
## Supply chain and dependency intelligence: Azure Pipelines guidance and GitHub Python graphs
A supply chain scare hit the JavaScript ecosystem again, and Microsoft responded with targeted guidance for teams running CI/CD on Azure Pipelines. It is a natural follow-on to last week''s DevSecOps thread about reducing long-lived secrets and making security signals match delivery reality. Here, the "delivery reality" problem is simpler but harsher: if your pipeline pulled a malicious package, you need to know quickly, prove what was built, and limit what that build system could have exposed.
After malicious Axios versions briefly appeared on npm, the Azure DevOps post lays out what to review in a pipeline environment (including self-hosted agents, tasks, service connections, and caches) and what to do next. The recommendations are the sort of operational steps that matter when you are trying to reduce blast radius: verify whether your builds pulled the bad versions, check for indicators of compromise on agents, rotate credentials that could have been exposed (especially service connection secrets), and push toward deterministic installs so "what got built" is easier to prove later. The post is especially relevant if you use self-hosted agents because they can retain state between jobs (caches, toolchains, residual artifacts) in ways that hosted agents typically do not. There is also an indirect link back to last week''s AiTM writeup detail about "Axios" appearing as a user-agent during token replay hunting: different problem space, same reminder that common libraries show up in attacker and defender telemetry, and context matters.
In parallel, GitHub improved the raw data security teams rely on by expanding Python dependency graph coverage. This builds on last week''s GitHub Advanced Security operations theme (better triage context, better reporting, less guesswork) by strengthening the inventory layer security teams need for policy, audits, and incident response. GitHub now uses a Dependabot job that submits dependency snapshots to the Dependency Submission API, which results in more complete dependency graphs and SBOMs (software bills of materials) for Python projects. Support includes pip, uv, and Poetry (both v1 and v2), which matters because modern Python repos often mix tooling across services and developer machines. More complete graphs improve alerting (Dependabot alerts), policy enforcement, and audit readiness because transitive dependencies show up more reliably.
- [Axios npm Supply Chain Compromise – Guidance for Azure Pipelines Customers](https://devblogs.microsoft.com/devops/axios-npm-supply-chain-compromise-guidance-for-azure-pipelines-customers/)
- [Dependabot-based dependency graphs for Python](https://github.blog/changelog/2026-04-23-dependabot-graphs-for-python)
## Platform security updates: .NET Data Protection patching and outbound exfiltration controls in Fabric Data Factory
On the patching front, Microsoft shipped a .NET out-of-band update that many ASP.NET Core teams will want to treat as an operational priority rather than a "next sprint" upgrade. This fits last week''s broader ransomware/edge-exploitation lesson (patch speed and operational runbooks are security controls), but at the application platform layer where "correctness" bugs can quickly become authentication or session integrity bugs if left unresolved. .NET 10.0.7 fixes CVE-2026-40372 in Microsoft.AspNetCore.DataProtection, and it was released after a 10.0.6 decryption regression created added urgency around Data Protection correctness and safety. Because ASP.NET Core Data Protection underpins things like auth cookies, CSRF tokens, and other encrypted/signed payloads, the update guidance emphasizes not just updating packages but also verifying behavior and redeploying cleanly. If you run multiple instances, remember that Data Protection depends on consistent key management across the farm, so follow the verification steps carefully and make sure your rollout does not accidentally split key rings or introduce mixed-version behavior longer than necessary.
On the data movement side, Microsoft Fabric Data Factory workloads reached general availability for workspace outbound access protection (OAP). This continues last week''s Fabric security arc (identity decoupling via associated identities, stronger transport security via custom CA and mTLS for Eventstream connectors) by adding a simple but high-leverage control at the network boundary: decide where data is allowed to go, and enforce it centrally. This adds workspace-level outbound endpoint allowlisting, which is a straightforward but effective control for reducing data exfiltration risk: pipelines and related workloads can only call out to approved destinations, which helps with compliance requirements and gives security teams a clear place to enforce egress policy. Microsoft noted additional experiences (including Data Agent and Eventstreams) are supported in preview, which hints at a broader push to make outbound restrictions consistent across Fabric workloads instead of leaving each service to solve egress controls differently.
- [.NET 10.0.7 Out-of-Band Security Update](https://devblogs.microsoft.com/dotnet/dotnet-10-0-7-oob-security-update/)
- [Outbound access protection for Data Factory (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/outbound-access-protection-for-data-factory-generally-available/)
## Other Security News
Microsoft outlined how it is using AI in its own defense pipeline, including integrating advanced models into the Security Development Lifecycle (SDL) and MSRC workflows, expanding exposure management guidance through Secure Now, and pairing Defender with GitHub Advanced Security tooling (CodeQL and Copilot Autofix) to reduce time-to-mitigation. This reads like the organizational counterpart to the agent governance and agentic SOC posts from last week: more automation is useful when it tightens feedback loops (secure coding, faster triage, faster patching) and stays bounded by policy and review, rather than acting as an unaccountable black box.
- [AI-powered defense for an AI-accelerated threat landscape](https://www.microsoft.com/en-us/security/blog/2026/04/22/ai-powered-defense-for-an-ai-accelerated-threat-landscape/)
An ASP.NET Community Standup showed how AI assistance can reduce friction when setting up Microsoft Entra ID auth in ASP.NET Core and .NET Aspire, including calling protected APIs and assigning identities to agents using Entra Agent ID (useful if you are experimenting with agent-style apps but still need clear identity boundaries). It ties back to last week''s repeated point that identity is still the main control plane even as app architectures shift: if agents and apps can call APIs, you want those calls to be attributable, constrained, and easy to revoke when something goes wrong.
- [ASP.NET Community Standup: Simplifying Entra ID authentication with AI](https://www.youtube.com/watch?v=I47G-pid-g8)',
    'Security news this week centered on the practical mechanics of stopping real intrusions (before they become full-bore ransomware style incidents), while teams also tightened the supply chain and started putting clearer guardrails around AI agents and data movement. Building on last week''s identity-first framing (tokens, session replay, and shrinking ambient privilege), this week''s stories show what that looks like when an attacker has hands-on access and when defenders can actually interrupt the chain with automation. Microsoft published two detailed Defender Security Research writeups that read like field guides for both attackers and defenders, and several platform updates (from .NET, GitHub, Azure DevOps, and Fabric) landed with concrete steps developers can take right now.',
    1777273200, 'security', '/security/roundups/weekly-security-roundup-2026-04-27', 'TechHub',
    'TechHub', 'DC74A176E35D63E239ADF294D80259861AAF7A9F840C36E8953902B874C63791', ',Microsoft Defender XDR,Defender Security Research,KQL,Microsoft Sentinel,Active Directory,Predictive Shielding,Quick Assist,WinRM,Rclone,MITRE ATT&CK,Agent Governance Toolkit,MCP,OPA Rego,Cedar Policy,Supply Chain Security,Azure Pipelines,npm,Dependabot,Dependency Submission API,SBOM,.NET,ASP.NET Core Data Protection,CVE 2026 40372,Microsoft Fabric Data Factory,Outbound Access Protection,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-04-20
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-04-20', 'roundups', 'Weekly Security Roundup: Identity-First Defaults and Faster Triage',
    'This week''s security updates focused on making controls easier to apply consistently at scale across GitHub and Azure DevOps, while threat research highlighted how attackers abuse collaboration tools and OS-native scripting. The broader direction continues toward identity-first access (OIDC, Workload Identity, Entra) to remove long-lived secrets, plus guidance for AI incident response and cryptographic readiness. It continues last week''s theme: reduce ambient privilege, tighten trust boundaries, and make secure defaults workable, whether through tokenless CI/CD, org-wide scanning baselines, or faster containment when users are socially engineered into granting access.
<!--excerpt_end-->
## GitHub supply chain and code security: OIDC, registries, SBOMs, and better triage mechanics
GitHub security platform changes reduce per-repo one-off configuration and improve org-wide operations. Building on last week''s workload identity direction (for example, npm Trusted Publishing OIDC expansion), orgs can now configure **multiple private registries per ecosystem** (npm, Maven, NuGet, Docker, pip, RubyGems, and others) at the **organization security settings** level. This removes the previous "one registry per ecosystem" limit and reduces repo-by-repo workarounds. GitHub also added **OIDC auth for org-level private registries** (via REST API too), enabling Dependabot and code scanning dependency resolution to use **short-lived federated credentials** instead of stored secrets. Initial integrations include **Azure DevOps Artifacts, AWS CodeArtifact, and JFrog Artifactory**. Together, these changes match last week''s "operate it well" direction: fewer secrets and less per-repo drift.
Code scanning moved closer to normal workflow with a new **public preview** that lets teams **link code scanning alerts to GitHub Issues**, with bidirectional navigation (alert "Tracking" section, issue "Security alerts" section). New `has:tracking` and `no:tracking` filters (including in Security Campaigns) help enforce hygiene such as "every actionable alert has a work item" without building custom dashboards.
**SBOM exports** from Dependency Graph are now **asynchronous**, which avoids the previous 10-second timeout on large repos. API users must switch to a two-step flow: `GET /repos/{owner}/{repo}/dependency-graph/sbom/generate-report` returns a `{sbom-uuid}`, then poll `GET /repos/{owner}/{repo}/dependency-graph/sbom/fetch-report/{sbom-uuid}` (HTTP **201** while processing, HTTP **302** redirect when ready). This matters for CI/inventory pipelines that previously retried and triggered duplicate backend work, matching last week''s push toward more predictable automation at scale.
Detection and prioritization also improved. **CodeQL 2.25.2** adds **Kotlin support up to 2.3.20**, reduces false positives in Java/Kotlin, C/C++, and C#, and updates **@security-severity** scoring. Notably, multiple **XSS queries moved from 6.1 (medium) to 7.8 (high)** across languages, and several log injection queries dropped from high to medium (Rust log injection rose from low to medium). If you gate builds or triage by severity, scoring shifts can reorder backlogs. This pairs with last week''s org-wide reporting message: reprioritization helps only if teams notice it.
GitHub also continues emphasizing what is actually running, extending last week''s runtime-context integration (Dynatrace). Repository properties now include **`deployable`** and **`deployed`** signals from artifact/deployment metadata, which can help target **rulesets and branch protection** to repos that ship. Dependabot and code scanning alerts now show **runtime risk context** on alert pages so teams can treat "in prod" differently than "dormant."
- [Dependabot and code scanning: Org-level private registries](https://github.blog/changelog/2026-04-14-dependabot-and-code-scanning-org-level-private-registries)
- [OIDC support for Dependabot and code scanning](https://github.blog/changelog/2026-04-14-oidc-support-for-dependabot-and-code-scanning)
- [Link code scanning alerts to GitHub Issues (public preview)](https://github.blog/changelog/2026-04-14-link-code-scanning-alerts-to-github-issues)
- [SBOM exports are now computed asynchronously](https://github.blog/changelog/2026-04-14-sbom-exports-are-now-computed-asynchronously)
- [CodeQL 2.25.2 adds Kotlin 2.3.20 support and other updates](https://github.blog/changelog/2026-04-15-codeql-2-25-2-adds-kotlin-2-3-20-support-and-other-updates)
- [Deployment context in repository properties and alerts](https://github.blog/changelog/2026-04-14-deployment-context-in-repository-properties-and-alerts)
## GitHub secret scanning and rapid exposure baselining
Secret scanning updates combined new detections, tighter enforcement (notably for fork-heavy enterprises), and API improvements for large-scale automation. This continues last week''s operational thread around secret scanning APIs/webhooks and delegated visibility, with an emphasis on consistent automation and audit trails instead of manual chasing.
Detection expanded with **Cloudflare** partner patterns: `cloudflare_account_api_token`, `cloudflare_global_user_api_key`, `cloudflare_user_api_token`. Push protection defaults expanded too: when secret scanning is enabled (including free public repos), additional patterns now **block commits by default**, including Cloudflare tokens and types like `figma_scim_token`, `google_gcp_api_key_bound_service_account`, `langsmith_license_key`, `langsmith_scim_bearer_token`, `openvsx_access_token`, and `posthog_personal_api_key`. This reinforces last week''s "default guardrails" approach where default-on reduces reliance on repo owners remembering settings.
For **Enterprise Managed Users (EMU)**, push protection now follows the **fork ancestor chain**: if any repo in the hierarchy has push protection enabled, forks inherit it. This closes a gap where secrets could leak through forks created outside licensed contexts.
REST API improvements support internal tooling at scale:
- Custom pattern alert `validity` can now be set via `PATCH /repos/{owner}/{repo}/secret-scanning/alerts/{alert_number}` (`active`, `inactive`, or `null`), and overrides appear in UI, webhooks, and audit logs.
- Alert list APIs add `provider` and `provider_slug`, plus `providers` / `exclude_providers` filters (mutually exclusive; both returns HTTP 422), enabling routing by issuer.
- Scan history adds AI-powered generic secret backfill via `generic_secrets_backfill_scans`.
- Enterprise reporting adds `GET /enterprises/{enterprise}/dismissal-requests/secret-scanning` for centralized dismissal-request oversight.
GitHub also introduced a free **Code Security Risk Assessment** for org admins/security managers on GitHub Enterprise Cloud and GitHub Team. It runs a one-click CodeQL scan across up to **20 most active repos**, shows results by severity/language/rule, highlights most vulnerable repos, and notes **Copilot Autofix eligibility** to connect discovery to remediation. GitHub says Actions minutes **don''t count against** org quota, which reduces baseline cost. This builds on last week''s org-level risk assessment reporting by making it easier to start with a baseline and manage remediation via reporting/tracking rather than spreadsheets.
- [Secret scanning pattern updates and product improvements](https://github.blog/changelog/2026-04-14-secret-scanning-pattern-updates-and-product-improvements)
- [How exposed is your code? Find out in minutes—for free](https://github.blog/security/application-security/how-exposed-is-your-code-find-out-in-minutes-for-free/)
## Azure DevOps Advanced Security: default CodeQL setup and org-wide alert coordination
Azure DevOps Advanced Security focused on making CodeQL adoption and triage less pipeline-centric and more centrally managed. In **public preview**, **CodeQL default setup** provides **one-click enablement** at repo/project/org scope, removing the need to author and maintain Azure Pipelines YAML, install CodeQL tasks, wire builds, and keep configuration current. With default setup, scans run automatically on a schedule adjustable at org level. This mirrors GitHub''s direction: centralized controls that are easier to audit and harder to forget.
A key control is setting which **agent pool** runs scanning jobs via org-level repo settings, which helps with network/compliance boundaries and capacity management.
The updated **combined alerts** experience in Security Overview adds an **org-wide view** (default branch across repos) and **security campaigns**: shareable, filtered alert views that stay live as new matching findings arrive. This matches last week''s org-level reporting theme (and this week''s GitHub tracking mechanics): central visibility plus consistent workflows is what makes "fix across the estate" feasible when queries or CVEs reshuffle priorities.
- [One-click security scanning and org-wide alert triage come to Advanced Security](https://devblogs.microsoft.com/devops/one-click-security-scanning-and-org-wide-alert-triage-come-to-advanced-security/)
## Threat research: social engineering via Teams/Quick Assist and macOS AppleScript tradecraft
Two research writeups reinforced a recurring defender reality: attackers abuse legitimate workflows (collaboration, remote help, scripting) and then move quickly into hands-on-keyboard activity using native tooling to blend in. This extends last week''s token-focused attack coverage (AiTM, device-code phishing): compromise often comes from trusted UX where users authorize access rather than from password theft.
Microsoft documented a human-operated intrusion chain starting with **cross-tenant Microsoft Teams helpdesk impersonation**, quickly turning into recon, persistence, lateral movement, and exfiltration. The actor convinces a user to accept Teams contact and approve remote assistance (often **Quick Assist**), then within **30-120 seconds** runs cmd.exe/PowerShell recon. Techniques include **DLL side-loading** using vendor-signed binaries from user-writable paths (often `C:\\ProgramData`), registry-stored encrypted config, outbound-HTTPS C2, and lateral movement via **WinRM (5985)**. Exfiltration uses **rclone.exe** to external cloud storage with tuned parameters. The post includes KQL hunts correlating Teams events with remote-assist launches, detecting signed-host sideload patterns in ProgramData, registry breadcrumbs, and rclone command lines, plus mitigations across Teams external collaboration controls, Safe Links for Teams, ASR rules, WDAC, Conditional Access, and WinRM scoping. Practically, it is the "legitimate workflow abuse" sibling of last week''s device-code story: both rely on trusted UX and short windows where containment and revocation playbooks must be fast.
On macOS, Microsoft Threat Intelligence detailed a "Sapphire Sleet" campaign driven mostly by **user-initiated execution** of lures (for example, compiled AppleScript `Zoom SDK Update.scpt`) rather than exploits. Tradecraft centers on AppleScript''s `do shell script` running staging like "curl -> osascript," with attacker user agents (`mac-cur1`...`mac-cur5`) mapping to stages (AppleScript loaders, ZIP-delivered `.app` bundles). The chain includes credential theft via a fake password prompt app, exfil via **Telegram Bot API**, persistence via **LaunchDaemons**, interactive backdoors opening zsh shells, and a technique to manipulate **TCC** by copying/modifying `~/Library/Application Support/com.apple.TCC/TCC.db` and injecting sqlite rows granting AppleEvents control. The writeup includes IOCs (domains/IPs/paths/hashes) plus Defender XDR/Sentinel KQL hunts for Script Editor spawning curl/osascript, TCC.db changes, suspicious LaunchDaemon creation, Telegram Bot API traffic, and ZIP staging. Alongside last week''s router/DNS hijack story, it is a reminder that initial compromise can look like normal user behavior, so hunts often start from downstream process/auth/permission signals.
- [Cross‑tenant helpdesk impersonation to data exfiltration: A human-operated intrusion playbook](https://www.microsoft.com/en-us/security/blog/2026/04/18/crosstenant-helpdesk-impersonation-data-exfiltration-human-operated-intrusion-playbook/)
- [Dissecting Sapphire Sleet’s macOS intrusion from lure to compromise](https://www.microsoft.com/en-us/security/blog/2026/04/16/dissecting-sapphire-sleets-macos-intrusion-from-lure-to-compromise/)
## Azure identity-first security patterns: passwordless AKS secret sync and Entra ID for Storage SFTP
Platform guidance continues replacing embedded credentials with identity federation and scoped authorization, with two practical examples. This also ties to last week''s managed identity blast-radius guidance: identity-first helps only when scoping and separation are done well.
For AKS, a guide shows syncing secrets from **Azure Key Vault** into namespaces using **External Secrets Operator (ESO)** with **AKS Workload Identity**, avoiding stored client secrets in-cluster. Steps include enabling AKS OIDC issuer + Workload Identity, creating a **User-Assigned Managed Identity (UAMI)**, binding it to a Kubernetes ServiceAccount via a **Federated Identity Credential** (audience `api://AzureADTokenExchange`), and granting Azure RBAC read access (often **Key Vault Secrets User**). ESO uses `SecretStore` (authType `WorkloadIdentity`) and `ExternalSecret` to materialize values into Kubernetes Opaque Secrets, with `refreshInterval` (example 30s) controlling rotation propagation. Troubleshooting focuses on RBAC scope, federated issuer/subject mismatch, and missing OIDC/workload identity enablement. Last week''s "gotcha" still applies: do not reuse the same UAMI broadly. Workload Identity makes token acquisition easier, so identity hygiene (environment separation, narrow RBAC) matters more.
Entra ID integrated auth for **Azure Storage SFTP** was highlighted as a way to stop managing storage-account local users and instead authenticate via **Microsoft Entra ID**, authorizing via Storage data-plane RBAC (and optional **ABAC conditions**). A practical gotcha is that SFTP sessions are token-driven, so token lifetime and refresh behavior affects long transfers and persistent sessions. Validate client behavior before rolling into partner and batch pipelines. This matches last week''s AiTM/device-code lessons: token handling (revocation, CAE/session constraints, client refresh behavior) increasingly determines access resilience for both attackers and automation.
- [Passwordless AKS Secrets: Sync Azure Key Vault with ESO + Workload Identity](https://techcommunity.microsoft.com/t5/microsoft-developer-community/passwordless-aks-secrets-sync-azure-key-vault-with-eso-workload/ba-p/4509959)
- [Entra ID Integrated SFTP](https://www.youtube.com/watch?v=pzPqnTHxNPU)
- [SFTP Entra ID Integrated Auth #azure #entraid #azurestorage](https://www.youtube.com/shorts/z1UXtM8HVqk)
## AI and cryptography readiness: operational playbooks, not just features
Two guidance pieces addressed areas where classic security operations needs adaptation (AI systems and cryptographic readiness), echoing last week''s governance/operability theme. Features help most when paired with operating models, observability, and clear ownership.
On incident response, Microsoft''s "Same fire, different fuel" argues core IR still applies (clear ownership, contain first, calm comms), but AI complicates triage and verification because outputs are probabilistic and harms do not map cleanly to CIA. The practical shift is **AI-specific observability**: anomalous output patterns, safety/classifier score shifts, spikes in user reports, and behavior changes after model/app updates. It recommends staged remediation: "stop the bleed" in the first hour (disable features, throttle, filters/blocklists), "fan out and strengthen" over 24 hours via automation, and "fix at the source" via classifier/model/guardrail updates, then verify with longer watch periods rather than one-off tests. It also calls out responder wellbeing when investigations involve harmful content. This overlaps with last week''s agent governance theme: you need kill switches, clear guardrails, and telemetry to separate abuse, misconfiguration, drift, and toolchain issues.
On cryptography, Microsoft outlined building a **cryptographic inventory** as the basis for cryptographic posture management and post-quantum readiness. The inventory spans certificates/keys, protocols/ciphers, embedded libraries, algorithms referenced in source, secrets/credentials, and hardware-backed crypto, and it is continuous: discover -> normalize -> assess -> prioritize -> remediate -> monitor for drift. Developer relevance is work you can backlog: use **GHAS/CodeQL** to find crypto primitives/algorithms, pair with **Defender for Endpoint/Vulnerability Management** for certificate/component inventory, use **Azure Key Vault** as a key/secret/cert source of truth, and centralize signals in **Microsoft Sentinel** to avoid ad hoc ownership. It also complements last week''s sovereignty/key-control thread: you cannot govern what you cannot inventory.
- [Incident response for AI: Same fire, different fuel](https://www.microsoft.com/en-us/security/blog/2026/04/15/incident-response-for-ai-same-fire-different-fuel/)
- [Building your cryptographic inventory: A customer strategy for cryptographic posture management](https://www.microsoft.com/en-us/security/blog/2026/04/16/building-your-cryptographic-inventory-a-customer-strategy-for-cryptographic-posture-management/)
## Other Security News
GitHub training content moved from prompt-injection basics to tool-using agent scenarios. Season 4 of the GitHub Secure Code Game introduces a vulnerable "ProdBot" with exploit-then-fix levels covering sandbox escape, untrusted web ingestion, MCP server tool connections, persistent memory risks (including poisoning), and multi-agent environments. It is designed for quick Codespaces runs and maps to OWASP agentic-app risks, complementing last week''s agent governance and agentic SOC framing with hands-on failure modes.
- [Hack the AI agent: Build agentic AI security skills with the GitHub Secure Code Game](https://github.blog/security/hack-the-ai-agent-build-agentic-ai-security-skills-with-the-github-secure-code-game/)
Microsoft also published a domain-compromise case study showing how Defender XDR **predictive shielding** (Automatic Attack Disruption) aims to contain high-privilege identities based on exposure signals (for example, credential dumping) before active abuse. The narrative links footholds (IIS web shells, SYSTEM escalation, Mimikatz, NTDS snapshotting, Impacket lateral movement) to the defender "speed gap" versus slow remediation tasks (krbtgt rotation, GPO/ACL cleanup). It fits alongside last week''s token/session focus: whether cloud token replay or on-prem credential dumping, advantage comes from fast containment plus practiced revocation/rotation runbooks.
- [Containing a domain compromise: How predictive shielding shut down lateral movement](https://www.microsoft.com/en-us/security/blog/2026/04/17/domain-compromise-predictive-shielding-shut-down-lateral-movement/)',
    'This week''s security updates focused on making controls easier to apply consistently at scale across GitHub and Azure DevOps, while threat research highlighted how attackers abuse collaboration tools and OS-native scripting. The broader direction continues toward identity-first access (OIDC, Workload Identity, Entra) to remove long-lived secrets, plus guidance for AI incident response and cryptographic readiness. It continues last week''s theme: reduce ambient privilege, tighten trust boundaries, and make secure defaults workable, whether through tokenless CI/CD, org-wide scanning baselines, or faster containment when users are socially engineered into granting access.',
    1776668400, 'security', '/security/roundups/weekly-security-roundup-2026-04-20', 'TechHub',
    'TechHub', 'AF63CF0F16FAC74E36E46FC21E19E1F9FA209E539424DF6137AAB51C29BCD8C5', ',GitHub Advanced Security,GitHub Actions OIDC,OpenID Connect,Workload Identity,Microsoft Entra ID,Dependabot,CodeQL,SBOM,Secret Scanning,Azure DevOps Advanced Security,AKS,Azure Key Vault,Microsoft Sentinel,Microsoft Defender XDR,Post Quantum Cryptography,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-04-13
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-04-13', 'roundups', 'Weekly Security Roundup: Token Replay, DevSecOps, Trust Boundaries',
    'This week''s security thread ranged from incident-response lessons (token replay, device-code phishing, router-based AiTM) to the quieter work of hardening identity, CI/CD, and data platforms. The common pattern is reducing ambient privilege, tightening trust boundaries, and improving automation so teams can respond faster without adding long-lived secrets or brittle owner-based dependencies. It extends last week''s identity-first framing: tactics shift, but control points stay consistent (phishing-resistant auth, tighter Conditional Access, shorter-lived tokens, and strong revocation/runbooks).
<!--excerpt_end-->
## Identity- and token-centric attacks: AiTM, device-code phishing, and the edge as the new MITM
Storm-2755''s "payroll pirate" activity shows session theft beating password theft. DART describes adversary-in-the-middle phishing that proxies Microsoft 365 sign-in, captures session cookies/OAuth tokens, and replays them to bypass MFA that is not phishing-resistant. It continues last week''s AiTM thread: tokens often matter more than passwords, and detections frequently come from sign-in telemetry. Hunting signals include an Entra sign-in interrupt error **50199** right before a successful sign-in, plus a **user-agent shift to Axios** (often `axios/1.7.9`) while the **session ID stays consistent**, which can indicate replay. The response playbook is clear: revoke sessions/tokens, remove malicious Exchange inbox rules hiding HR/payroll messages, then harden with FIDO2/WebAuthn, stricter Conditional Access (device compliance, session controls), and Continuous Access Evaluation (CAE) to reduce replay windows.
Microsoft Defender also covered an **AI-assisted OAuth device code phishing** campaign that generates device codes on demand only after a victim clicks, which preserves the 15-minute validity window and improves success rates. It reinforces last week''s point that automation can raise the tempo of older attack flows. The detection anatomy includes tenant recon via `GetCredentialType`, multi-hop redirects using compromised domains and serverless infrastructure (Vercel, Cloudflare Workers, AWS Lambda), and a backend that requests device codes live then polls every 3-5 seconds. Post-compromise behavior (device registration for longer-lived access, Graph recon, inbox-rule persistence) ties to mitigation: block device-code flow where unnecessary via Conditional Access "block authentication flows," enforce Entra ID Protection risk policies, and use Graph session revocation to invalidate stolen refresh tokens.
Forest Blizzard / Storm-2754 broadens the AiTM story with SOHO router compromises where DNS hijacking becomes the MITM stepping stone. The identity outcome is the same (token/session abuse) but achieved by shifting interception to edge devices enterprises often do not monitor closely. By altering router DHCP/DNS settings (likely via **dnsmasq**), the actor can proxy most DNS traffic and selectively spoof responses for target domains. In higher-priority cases, victims are redirected to infrastructure with **invalid TLS certificates**. If users click through, sessions can be intercepted. Defenses focus on limiting blast radius: enforce trusted DNS resolution on endpoints (Windows **Zero Trust DNS**), enable Defender for Endpoint web/network protection, tighten Conditional Access and CAE, and hunt for downstream Entra/M365 anomalies rather than expecting to detect the router compromise directly.
- [Investigating Storm-2755: “Payroll pirate” attacks targeting Canadian employees](https://www.microsoft.com/en-us/security/blog/2026/04/09/investigating-storm-2755-payroll-pirate-attacks-targeting-canadian-employees/)
- [Inside an AI-enabled device code phishing campaign](https://www.microsoft.com/en-us/security/blog/2026/04/06/ai-enabled-device-code-phishing-campaign-april-2026/)
- [SOHO router compromise leads to DNS hijacking and adversary-in-the-middle attacks](https://www.microsoft.com/en-us/security/blog/2026/04/07/soho-router-compromise-leads-to-dns-hijacking-and-adversary-in-the-middle-attacks/)
## DevSecOps and supply chain workflow updates: prioritize what’s deployed, reduce secrets, and streamline fixes
GitHub''s updates continued to move security closer to developer workflows while adding automation and governance for platform teams. It continues last week''s shift from "turn it on" to "operate it well," with better triage signals, better reporting, and less reliance on long-lived secrets.
GitHub Advanced Security can now ingest **Dynatrace runtime context** to prioritize code scanning and Dependabot alerts based on what is actually deployed and observed at runtime. Instead of treating all alerts equally, teams can filter to findings affecting production artifacts that are *internet-exposed* or touch *sensitive data* (for example, `has:deployment AND runtime-risk:internet-exposed`). This aligns with last week''s theme of making security signals match delivery reality so teams spend less time on noise.
GitHub code scanning now supports **batch-applying multiple fix suggestions in a PR** from the "Files changed" tab. The benefit is fewer commits and fewer repeated scans: apply multiple low-risk fixes together, commit once, and validate with one scanning run. It complements last week''s faster PR feedback focus by reducing fix-step friction, not only detection friction.
Secret Scanning''s REST/webhook updates improve scale operations and auditability, building on last week''s detector/push-protection expansion. `exclude_secret_types` on list endpoints makes scripts more resilient as new secret types appear. Location payloads add `html_url` to reduce extra API calls for dashboards/tickets. Delegated workflows gained clearer lifecycle visibility (expiration windows, confirmation emails) plus closure fields (`closure_request_comment`, reviewer metadata) so external systems can track why alerts were dismissed and who approved it.
npm Trusted Publishing also added **CircleCI** as an OIDC provider, extending tokenless publishing beyond GitHub Actions and GitLab CI/CD. After last week''s supply-chain push toward short-lived workload identity, this makes it easier for CircleCI pipelines to publish without stored npm tokens through `npm trust`.
- [Prioritize security alerts with runtime context from Dynatrace](https://github.blog/changelog/2026-04-07-prioritize-security-alerts-with-runtime-context-from-dynatrace)
- [Code scanning: Batch apply security alert suggestions on pull requests](https://github.blog/changelog/2026-04-07-code-scanning-batch-apply-security-alert-suggestions-on-pull-requests)
- [Secret scanning improvements to alert APIs, webhooks, and delegated workflows](https://github.blog/changelog/2026-04-08-secret-scanning-improvements-to-alert-apis-webhooks-and-delegated-workflows)
- [npm trusted publishing now supports CircleCI](https://github.blog/changelog/2026-04-06-npm-trusted-publishing-now-supports-circleci)
## Platform identity, data governance, and agent runtime controls: shrinking blast radius across Azure, Fabric, and agent stacks
Several posts converged on a practical point: shared identities and default execution contexts create hidden coupling, and over-sharing can turn a normal incident into cross-environment impact. This matches last week''s "guardrails on default paths" theme. Put controls at natural choke points (identity issuance, admission, label APIs) so protections still hold when workflows drift.
In Azure, guidance on **User Assigned Managed Identities (UAMI)** calls out a common enterprise anti-pattern: reusing one UAMI across DEV/UAT/PROD and across compute types (Functions, App Service, VMs, AKS). Since any attached workload can use **IMDS** to get Entra tokens for that UAMI, a lower-environment compromise can become lateral access into production if RBAC is broad (especially at subscription scope). The recommendation is straightforward: separate UAMIs per environment, scope RBAC to resource group/resource, and prefer narrow data-plane roles (Key Vault Secrets User, Storage Blob Data Reader) over broad management roles.
Fabric shipped two identity/encryption-related previews that target similar coupling. Last week''s Fabric governance work emphasized operability (labels in REST APIs, CMK options, recovery). This week reduces owner-dependency and improves enterprise transport security. **Associated identities for items** lets Lakehouse and certain Eventstream items run under a specified identity (user, service principal, or managed identity) rather than the creator/owner. That avoids breakage when owners leave or credentials expire and aligns with CI/CD through Fabric REST APIs (`.../identities/default/assign?beta=true`, plus `include=defaultIdentity` in GET). Fabric Eventstream Kafka connectors also gained **custom CA chains and mTLS** support, with certs stored in **Azure Key Vault** and fetched at runtime. For private deployments, that implies a concrete prerequisite: if you are using Eventstream VNet injection, you may need a **Key Vault private endpoint** to keep cert retrieval off public paths.
At the AI runtime layer, the open-source **Agent Governance Toolkit v3.0.0 (Public Preview)** outlines how to run autonomous agents with policy, identity, isolation, and SRE guardrails. It follows last week''s agent governance framing by shifting from "agents are risky" to "here is how to enforce boundaries and kill switches." The core risk is that agents can behave like "root" when calling tools/APIs, and the toolkit maps that to controls. **Agent OS** acts as a stateless pre-execution policy interceptor (YAML/OPA Rego/Cedar), classifies intent (for example, `DESTRUCTIVE_DATA`, `DATA_EXFILTRATION`), and supports enforcement modes (block, require approval, downgrade trust). **Agent Mesh** adds cryptographic identity/inter-agent trust using DIDs, Ed25519 signing, and delegation chains that only allow scope narrowing. **Agent Hypervisor** adds privilege-ring-style isolation tied to trust scores (Ring 0-3) with resource limits and saga orchestration for compensating actions. **Agent SRE** adds SLOs/error budgets, circuit breakers, chaos templates, and observability via OpenTelemetry/Prometheus and incident tooling. The result is a blueprint for running agents in Kubernetes/Azure without treating every tool call as implicitly allowed.
- [Enterprise UAMI Design in Azure: Trust Boundaries and Blast Radius](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/enterprise-uami-design-in-azure-trust-boundaries-and-blast/ba-p/4509614)
- [Associated identities for items (Preview)](https://blog.fabric.microsoft.com/en-US/blog/associated-identities-for-items-preview/)
- [Secure data streaming: Custom CA and mTLS in Fabric Eventstream connectors (Preview)](https://blog.fabric.microsoft.com/en-US/blog/secure-data-streaming-custom-ca-and-mtls-in-fabric-eventstream-connectors-preview/)
- [Agent Governance Toolkit: Architecture Deep Dive, Policy Engines, Trust, and SRE for AI Agents](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/agent-governance-toolkit-architecture-deep-dive-policy-engines/ba-p/4510105)
## Other Security News
Ransomware activity reinforced how little time can exist between disclosure and exploitation for web-facing systems. Storm-1175''s Medusa intrusions show fast-moving playbooks exploiting newly disclosed edge vulnerabilities (Exchange, Ivanti, ScreenConnect, TeamCity, GoAnywhere MFT, and others), then moving quickly into credential theft (LSASS dumping, Mimikatz), lateral movement (PsExec/Impacket/RDP), and exfiltration (Rclone) before encryption. The operational implication is straightforward: asset inventory, patch pipelines, and hardening controls like Defender tamper protection/ASR rules and WAF/reverse proxy placement are ongoing engineering work. It also echoes last week''s "default paths" theme: internet-facing management and file-transfer planes are predictable entry points, so inventory and patch SLAs function as guardrails.
- [Storm-1175 focuses gaze on vulnerable web-facing assets in high-tempo Medusa ransomware operations](https://www.microsoft.com/en-us/security/blog/2026/04/06/storm-1175-focuses-gaze-on-vulnerable-web-facing-assets-in-high-tempo-medusa-ransomware-operations/)
Mobile supply chain risk got a concrete example: an Android intent redirection issue in EngageLab''s EngageSDK where a merged-manifest exported activity (`MTCommonActivity`) could let a malicious local app trigger attacker-controlled intents using the victim app''s identity/permissions. The fix is to upgrade to EngageSDK **v5.2.1+** (activity non-exported). The broader lesson matches last week''s supply-chain incidents: review merged manifests after SDK updates because execution surface can change even when your own manifest looks clean, and build repeatable checks (manifest review, SCA, policy) into default dependency workflows.
- [Intent redirection vulnerability in third-party SDK exposed millions of Android wallets to potential risk](https://www.microsoft.com/en-us/security/blog/2026/04/09/intent-redirection-vulnerability-third-party-sdk-android/)
Azure sovereignty requirements were translated into concrete controls for Belgium Central: IaC region pinning (`belgiumcentral`), AZ-first resiliency in a **non-paired region**, CMK/double encryption with Key Vault/Managed HSM, and confidential computing with attestation-gated key release where available (AMD SEV-SNP previews in-region; Intel TDX not available there). It complements last week''s governance-and-keys theme: sovereignty is implemented through residency, key control, and attestation/encryption for data in use.
- [Sovereignty in Azure Belgium Central: A Three-Layer Technical Deep Dive](https://techcommunity.microsoft.com/t5/azure-confidential-computing/sovereignty-in-azure-belgium-central-a-three-layer-technical/ba-p/4506936)
Two GitHub admin items rounded out the week: a new org-level Code Security risk assessment report aggregating findings by severity/rule type/language (and flagging where Copilot Autofix may apply), and a heads-up for `gh` installs via apt/yum/dnf. GitHub published an updated PGP keyring ahead of the current key''s 2026-09-05 expiry to avoid future install/update failures in machines and CI images. The report aligns with last week''s improved CodeQL reporting by giving a closer org-wide picture beyond default-branch slices. The signing-key update is supply-chain hygiene in the same spirit as last week''s CI hardening: key rotation keeps default install paths reliable.
- [Code Security risk assessment available for organizations](https://github.blog/changelog/2026-04-08-code-security-risk-assessment-available-for-organizations)
- [New PGP signing key for GitHub CLI Linux packages](https://github.blog/changelog/2026-04-08-new-pgp-signing-key-for-github-cli-linux-packages)
Microsoft''s "agentic SOC" post offered a forward-looking SecOps model: deterministic autonomous disruption for high-confidence threats, plus agents that assemble context and orchestrate investigations so humans focus on governance, tuning, and longer-term posture. It continues last week''s agent governance thread. More automation only helps when bounded by policy, identity, and auditability (what agents can do, when approval is required, and how to shut them off). Even without higher autonomy, it provides a useful lens for where engineering effort shifts: policy boundaries, confidence thresholds, and accountable automation over manual triage.
- [The agentic SOC—Rethinking SecOps for the next decade](https://www.microsoft.com/en-us/security/blog/2026/04/09/the-agentic-soc-rethinking-secops-for-the-next-decade/)',
    'This week''s security thread ranged from incident-response lessons (token replay, device-code phishing, router-based AiTM) to the quieter work of hardening identity, CI/CD, and data platforms. The common pattern is reducing ambient privilege, tightening trust boundaries, and improving automation so teams can respond faster without adding long-lived secrets or brittle owner-based dependencies. It extends last week''s identity-first framing: tactics shift, but control points stay consistent (phishing-resistant auth, tighter Conditional Access, shorter-lived tokens, and strong revocation/runbooks).',
    1776063600, 'security', '/security/roundups/weekly-security-roundup-2026-04-13', 'TechHub',
    'TechHub', 'DD57ADE5C2D70B2D24F5EB206011436C882AE92FBCB419E1C43D9CDB0F285713', ',Microsoft Entra ID,OAuth,Session Hijacking,Adversary in The Middle,Device Code Phishing,Conditional Access,Continuous Access Evaluation,FIDO2/WebAuthn,Microsoft Defender,GitHub Advanced Security,Code Scanning,Dependabot,Secret Scanning,OIDC Trusted Publishing,Azure Managed Identity,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-04-06
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-04-06', 'roundups', 'Weekly Security Roundup: Supply Chain, CI Guardrails, and Governance',
    'This week’s security items reflected two pressures: intrusions that abuse everyday automation (dependency installs, hosted web stacks, messaging attachments) and platform changes intended to make those workflows harder to exploit (CI hardening, secret detection, governable data/AI). Building on last week’s theme (attackers using default paths like dependency installs and workflow triggers, defenders adding enforceable guardrails), this week focused on high-leverage control points: npm installs, Actions runs, `kubectl` applies, and REST API inventory jobs.
<!--excerpt_end-->
## Supply chain pressure: npm dependency installs and GitHub workflow hardening
After last week’s Trivy compromise (mutable tags, runner discovery, secret harvesting), the axios npm incident reinforced the same lesson: a supply chain compromise can cause damage even if it never ships in runtime, because install-time scripts can target developer laptops or CI runners. Two malicious releases, axios@1.14.1 and axios@0.30.4, added `plain-crypto-js@^4.2.1`, which runs a postinstall script (`node setup.js`). The loader is obfuscated, fingerprints the OS, then calls `hxxp://sfrclak[.]com:8000/6202033` (142.11.206[.]73) to fetch an OS-specific second stage: a macOS binary dropped to `/Library/Caches/com.apple.act.mond` launched via AppleScript/osascript, a Windows PowerShell RAT staged in `%TEMP%` with persistence at `HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\run\\MicrosoftUpdate` (and camouflage like `C:\\ProgramData\\wt.exe`), or a Linux Python loader written to `/tmp/ld.py`. It also attempts cleanup by removing triggering files and restoring a benign-looking `package.json`.
For teams, the impact mirrors last week’s pipeline guidance: treat dependency updates as incidents until you confirm otherwise, because build hosts and secrets may already be exposed. Recommended actions include downgrading to known-good axios (1.14.0 or earlier; 0.30.3 or earlier), pinning exact versions (avoid `^`/`~`), enforcing with npm overrides, clearing npm cache, reviewing logs for bad versions or `plain-crypto-js@4.2.1`, rotating secrets if runners may be compromised, and considering `npm ci --ignore-scripts` (or `ignore-scripts=true`) where feasible while acknowledging some ecosystems rely on scripts. Microsoft provided hunting guidance (KQL for Defender XDR/Sentinel) for package inventory, suspicious `setup.js` execution, and outbound traffic to the listed C2, plus IOCs and Defender detection names.
In parallel, GitHub’s supply-chain guidance continues last week’s Actions hardening direction: reduce reliance on trust-by-convention (mutable refs, broad secrets) and make workflows more resistant to Trivy-style pivots. It recommends enabling CodeQL scanning for workflow YAML with the Actions query pack, avoiding `pull_request_target` when possible, pinning third-party actions to full commit SHAs (and treating pin changes as high-risk), and hardening against script injection when interpolating user-controlled values (branch names, PR titles). It also emphasizes replacing long-lived secrets with OIDC federated identity and using "trusted publishing" (OpenSSF-aligned) to improve provenance without embedding publish creds in pipelines. Together, axios plus GitHub workflow guidance continues last week’s "guardrails on default paths" thread: dependency resolution and CI config are core attack surfaces, and a dependable mitigation is minimizing secret exposure while tightening what can run.
- [Mitigating the Axios npm supply chain compromise](https://www.microsoft.com/en-us/security/blog/2026/04/01/mitigating-the-axios-npm-supply-chain-compromise/)
- [Securing the open source supply chain across GitHub](https://github.blog/security/supply-chain-security/securing-the-open-source-supply-chain-across-github/)
## Threat research and hunting: stealthy Linux webshells and WhatsApp-delivered Windows chains
Microsoft documented a stealthy Linux hosting technique: PHP webshells that take commands from HTTP cookies instead of query params or POST bodies. This matches last week’s framing that routine-looking traffic can hide execution, so defenses should focus on enforceable controls and observable choke points. Because the trigger is in `$_COOKIE`, shells can stay dormant under normal browsing and avoid obvious log indicators. Variants include obfuscated loaders that reconstruct function names, write second stages, then `include` them, and interactive single-file shells gated by a "key" cookie. Persistence often uses cron "self-healing" to recreate loaders (including in cPanel/jailshell contexts) plus permission tweaks that slow cleanup. Detection guidance maps to ops reality: hunt for web server processes spawning shells and tools (`base64`, `curl`, `wget`), pipelines that decode/write `.php` into web directories, and cron jobs running frequently that write into web paths. Defender for Endpoint on Linux setup and Defender XDR KQL queries are provided.
Another Defender post covered a WhatsApp-delivered Windows campaign that starts with a malicious `.vbs` attachment and ends with unsigned MSI installers used for persistence and remote access. It continues last week’s identity/containment theme: attacker chains blend into normal admin/user behavior, so hunting depends on lineage, metadata, and policy containment. The chain creates hidden folders in `C:\\ProgramData`, drops renamed legitimate utilities (for example, `curl.exe` renamed to `netapi.dll`, `bitsadmin.exe` renamed to `sc.exe`), downloads additional VBS from AWS S3/Tencent COS/Backblaze B2, then tampers with UAC-related registry values to reduce prompts while attempting elevation. Final-stage unsigned MSI packages (including "AnyDesk.msi") blend into typical software installs. A defender tip is detecting renamed binaries via PE metadata mismatches (for example, `VersionInfoOriginalFileName`), plus KQL hunts for `wscript/cscript` from suspicious locations, downloader flags, and `.vbs`/`.msi` drops tied to renamed utilities. Hardening guidance focuses on ASR rules (obfuscated scripts, low-prevalence executables, blocking VBScript/JavaScript launching downloaded executables), script host restrictions, and enabling EDR-in-block-mode and tamper protection.
- [''Cookie-controlled PHP webshells: A stealthy tradecraft in Linux hosting environments''](https://www.microsoft.com/en-us/security/blog/2026/04/02/cookie-controlled-php-webshells-tradecraft-linux-hosting-environments/)
- [WhatsApp malware campaign delivers VBS payloads and MSI backdoors](https://www.microsoft.com/en-us/security/blog/2026/03/31/whatsapp-malware-campaign-delivers-vbs-payloads-msi-backdoors/)
## GitHub security operations: secret scanning expansion and CodeQL reporting that matches real branching
GitHub security tooling continued shifting from "enable it" to "operate it," which matches last week’s faster feedback loops (incremental CodeQL in PRs) and tighter control surfaces (push protection exemptions, credential revocation). Secret Scanning added nine detectors across seven providers (including Figma SCIM tokens, LangSmith keys/tokens, PostHog OAuth tokens, Salesforce Marketing Cloud tokens) and added validity checks for `npm_access_token` so alerts can show whether a token is still active. Push protection also expanded default blocking to more secret types (including Figma SCIM tokens and specific Google/OpenVSX/PostHog patterns) when Secret Scanning is enabled, which reduces the chance secrets land in history. In the context of last week’s "rotate/revoke quickly" theme, the improvement is triage quality: not just "a token exists," but "it still works."
CodeQL 2.25.0 updates align analysis with current toolchains and improve precision: Swift 6.2.4 support; a rewritten Java/Kotlin control flow graph focusing on reachable nodes; C# support for C# 14 partial constructors; and taint tracking that treats `System.Net.WebSockets::ReceiveAsync` as a remote source to improve WebSocket findings. JavaScript/TypeScript also gained browser source kinds (`browser-url-query`, `browser-url-fragment`, `browser-message-event`) for teams extending CodeQL models. Along with last week’s incremental PR scanning push, the direction stays consistent: keep scans fast enough for PR loops while improving modeling fidelity.
GitHub Security Overview’s CodeQL "pull request insights" now aggregates across all protected branches, not just the default branch, and CSV export matches. This fits last week’s "govern rollouts, reduce drift" framing: teams often ship from release/* and maintenance branches, so default-branch-only reporting undercounts both risk and remediation work. After rollout, dashboards (and Copilot Autofix outcome counts) should better reflect what is being fixed and shipped, though historical numbers may shift.
- [GitHub secret scanning — coverage update](https://github.blog/changelog/2026-03-31-github-secret-scanning-nine-new-types-and-more)
- [CodeQL 2.25.0 adds Swift 6.2.4 support](https://github.blog/changelog/2026-03-31-codeql-2-25-0-adds-swift-6-2-4-support)
- [CodeQL pull requests insights on security overview now cover all protected branches](https://github.blog/changelog/2026-03-31-codeql-pull-requests-insights-on-security-overview-now-cover-all-protected-branches)
## Security governance in Azure and Microsoft Fabric: admission control, labeling, encryption, and recovery
AKS guidance emphasized where security checks prevent incidents in practice: admission time, not only CI. This matches last week’s "guardrails on execution paths" framing by shifting enforcement from pipelines to cluster-side controls that still apply when someone uses `kubectl` or when drift accumulates. The approach combines early CI guardrails with Azure Policy for AKS (OPA Gatekeeper) to enforce policies in-cluster. It recommends staged Pod Security Standards rollout (Audit first, then Deny in production namespaces), network policy enforcement to limit lateral movement, and image governance at admission so only approved registries/images run. Runtime coverage comes from Microsoft Defender for Containers (with a reminder that restricted-egress clusters need outbound endpoint planning), while Azure Policy compliance reporting provides continuous audit/drift views across clusters/namespaces.
Fabric governance became easier to automate, continuing last week’s "enforceable controls without rewrites" thread plus better API surfaces for inventory/policy. Sensitivity labels are now returned in Fabric Public REST APIs (List Items, Get Item, Update Item), reducing extra per-item metadata calls for inventory/compliance workflows; label updates still use Admin label management endpoints, including Bulk Set/Remove. This supports cleaner label-aware automation patterns, including for AI/agent workflows that must filter access by "Confidential"/"Restricted" classifications, which we highlighted last week with Purview monitoring and governance.
Fabric also introduced Item Recovery (preview): item-level soft delete with a workspace recycle bin and tenant-configurable retention (7-90 days), with restore via portal or REST API. This extends last week’s identity/recovery reminders (Entra Backup/Restore) into the analytics plane, because governance also includes practical recovery when mistakes or malicious deletions happen. Beyond undoing deletes for notebooks/pipelines/lakehouses, it supports forensics by restoring artifacts (and their lineage/labels) when audit logs only show create/delete. Fabric preview also allows workspace-level Customer-Managed Keys (CMK) even when the workspace is on a BYOK-enabled capacity, removing a constraint that previously forced capacity splits. The separation remains (BYOK for Power BI semantic models at capacity; workspace CMK for other Fabric items), but it simplifies regulated deployments and key runbooks, complementing last week’s CMK GA for Fabric SQL Database.
- [''DevSecOps on AKS: Governance Gates That Actually Prevent Incidents''](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/devsecops-on-aks-governance-gates-that-actually-prevent/ba-p/4508415)
- [Sensitivity labels in Fabric for public APIs (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/sensitivity-labels-in-fabric-for-public-apis-generally-available/)
- [Item Recovery in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/item-recovery-in-microsoft-fabric-preview/)
- [Workspace Customer-Managed Keys for BYOK in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/workspace-customer-managed-keys-for-byok-in-microsoft-fabric-preview/)
## Other Security News
Agent security guidance continued shifting from model behavior to enforceable control surfaces, building on last week’s agent-governance focus on intent, identity, and runtime checks. Microsoft mapped the OWASP Top 10 Risks for Agentic Applications (2026) to Copilot Studio guardrails: restrict allowed connectors/actions, apply DLP to limit data movement, use isolation + republishing to prevent runtime self-modification, and ensure operational "kill switch" controls (restrict/disable/stop sharing). A companion governance layer is Agent 365 (preview, GA noted as May 1) as a lifecycle control plane for monitoring and policy enforcement across deployed agents.
- [Addressing the OWASP Top 10 Risks in Agentic AI with Microsoft Copilot Studio](https://www.microsoft.com/en-us/security/blog/2026/03/30/addressing-the-owasp-top-10-risks-in-agentic-ai-with-microsoft-copilot-studio/)
GitHub enablement content focused on making repo security features part of normal PR work, echoing last week’s "earlier feedback in PRs" direction (incremental CodeQL) and the broader goal of workable controls at scale. A "getting started" guide covers enabling GHAS features (Secret Scanning, Dependabot alerts/security updates, CodeQL) and using Copilot Autofix for eligible CodeQL alerts, while reminding teams remediation needs review and secret leaks require provider rotation/revocation. A Dependabot short reinforces the workflow: let Dependabot open patch PRs, validate with CI/tests, merge to reduce time-to-fix. Secret scanning education reiterates the response loop: find the line, remove the secret, rotate/revoke, and confirm downstream updates.
- [''GitHub for Beginners: Getting started with GitHub security''](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-security/)
- [How to fix vulnerabilities automatically with Dependabot](https://www.youtube.com/shorts/kyjQXPTuvqo)
- [How GitHub secret scanning saves your code](https://www.youtube.com/shorts/wYmTs1LSvTw)
A small GitHub UI change may affect docs: the "Security" tab is now "Security & quality," "Vulnerability alerts" is now "Findings," and a "Code quality" section appears in the repo sidebar. URLs/APIs are unchanged, but internal runbooks, training, and screenshots may need updates; GHES does not get this yet.
- [The Security tab is now Security & quality](https://github.blog/changelog/2026-04-02-the-security-tab-is-now-security-quality)
GitHub Enterprise Cloud’s EU data residency region expands May 1, 2026 to include Azure regions in Norway and Switzerland (EFTA), aligning with Microsoft’s EU Data Boundary. No certification/control changes are claimed, but organizations requiring EU-member-state-only residency should contact support/account teams ahead of the date. It pairs with last week’s governance story: where the platform runs remains part of compliance alongside keys, labels, and auditability.
- [EU data residency region expanding to include EFTA countries](https://github.blog/changelog/2026-03-31-eu-data-residency-region-expanding-to-include-efta-countries)
Microsoft Threat Intelligence also outlined how generative AI is increasingly embedded in attacker workflows, including higher-conversion phishing paired with infrastructure that bypasses MFA via adversary-in-the-middle (AiTM) token theft. This continues last week’s identity-first framing: attackers target what identities can reach, and defenses prioritize phishing-resistant MFA plus context-aware containment. The post highlights Tycoon2FA (Storm-1747) as a modular cybercrime service, notes disruption efforts (including domain seizures), and reinforces that token theft and phishing-resistant MFA are central to modern defense.
- [Threat actor abuse of AI accelerates from tool to cyberattack surface](https://www.microsoft.com/en-us/security/blog/2026/04/02/threat-actor-abuse-of-ai-accelerates-from-tool-to-cyberattack-surface/)',
    'This week’s security items reflected two pressures: intrusions that abuse everyday automation (dependency installs, hosted web stacks, messaging attachments) and platform changes intended to make those workflows harder to exploit (CI hardening, secret detection, governable data/AI). Building on last week’s theme (attackers using default paths like dependency installs and workflow triggers, defenders adding enforceable guardrails), this week focused on high-leverage control points: npm installs, Actions runs, `kubectl` applies, and REST API inventory jobs.',
    1775458800, 'security', '/security/roundups/weekly-security-roundup-2026-04-06', 'TechHub',
    'TechHub', 'F3202E6549D4D9F58DCDC62516DF812D621289F3742DC62E2C0FCD83684B5DE3', ',Supply Chain Security,npm,Node.js,Axios,Postinstall Scripts,GitHub Actions,OIDC,Secret Scanning,Push Protection,CodeQL,AKS,Azure Policy,OPA Gatekeeper,Microsoft Fabric,Customer Managed Keys,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-03-30
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-03-30', 'roundups', 'Weekly Security Roundup: CI/CD Trust, Secrets, and Data Controls',
    'This week''s security story centered on CI/CD trust and identity/data control. A real supply-chain compromise hit developer pipelines, while GitHub and Microsoft shared concrete steps to reduce drift: dependency locking, tighter secret scope, faster feedback, and more platform-enforced policy. It also continues last week''s theme: defenders are adding guardrails to default paths (dependency installs, workflow triggers, org rollouts) where attackers keep showing up.
<!--excerpt_end-->
## GitHub Actions supply-chain defense: from a real compromise to platform-level hardening
Microsoft''s incident guidance on the Trivy supply-chain compromise was the most urgent item. A malicious Trivy binary (called out as v0.69.4) and compromised actions (`aquasecurity/trivy-action`, `aquasecurity/setup-trivy`) were used to steal pipeline credentials. The attacker force-pushed version tags (76 of 77 tags for `trivy-action`, all 7 for `setup-trivy`) so workflows pinned to mutable tags like `@v1` could silently run attacker code. On self-hosted runners, the payload performed broad discovery and secret harvesting (cloud env vars, AWS IMDS/ECS metadata endpoints, Kubernetes service account mounts and `kubectl get secrets --all-namespaces -o json`, scans for `.env`/YAML/JSON, webhook URLs, SSH logs, Vault/DB strings), packaged results (`tpcp.tar.gz`) with hybrid encryption (AES-256-CBC + RSA), and exfiltrated via HTTP POST to `scan.aquasecurtiy[.]org`, then ran the legitimate scan to keep jobs green. Mitigations: move to the listed safe versions (Trivy v0.69.2-v0.69.3, `trivy-action` v0.35.0, `setup-trivy` v0.2.6), pin third-party actions to verified commit SHAs, tighten `GITHUB_TOKEN` permissions, restrict allowed actions via org policy, and reduce runner secret exposure (ephemeral runners/clean environments, JIT secret retrieval). Microsoft also provided Defender Advanced Hunting KQL for indicators like the exfil domain, encryption commands, and runner artifact paths.
This context makes GitHub''s Actions 2026 security roadmap read like a response to the same failure modes, extending last week''s "shift checks earlier, govern rollouts" thread into workflow execution. GitHub plans workflow-level dependency locking via a `dependencies:` section that locks direct/transitive action dependencies to immutable commit SHAs plus cryptographic hashes, compared to Go''s `go.mod`/`go.sum`. The intended flow is CLI-based: resolve dependencies, commit lock data, and update via re-resolve + diff review; jobs would fail if hashes mismatch, and composite actions would expose nested dependencies. GitHub also plans rulesets for policy-driven workflow execution (who can trigger, which events are allowed), with examples like restricting `workflow_dispatch` and avoiding `pull_request_target` in favor of `pull_request`, plus an evaluate mode before enforcement. "Scoped secrets" would reduce implicit inheritance (notably with reusable workflows) by binding secrets to explicit contexts, and secret management is planned to move into a dedicated role instead of generic write access. Finally, GitHub is adding runner visibility/containment: an Actions Data Stream for near-real-time telemetry to Amazon S3 and Azure Event Hub/Azure Data Explorer, and a native Layer 7 egress firewall for GitHub-hosted runners with monitor/enforce modes designed to hold even if an attacker gets root in the runner VM. The through-line matches last week''s security/observability direction: visibility plus enforceable boundaries under stress.
- [Guidance for detecting, investigating, and defending against the Trivy supply chain compromise](https://www.microsoft.com/en-us/security/blog/2026/03/24/detecting-investigating-defending-against-trivy-supply-chain-compromise/)
- [What’s coming to our GitHub Actions 2026 security roadmap](https://github.blog/news-insights/product-news/whats-coming-to-our-github-actions-2026-security-roadmap/)
## GitHub Code Security: faster PR scanning, broader detections, and more control over secret blocking
GitHub''s app security updates focused on making checks faster in PRs while expanding coverage. Continuing last week''s "earlier feedback with smoother rollout" theme, CodeQL PR scanning now uses incremental analysis by default for C#, Java, JavaScript/TypeScript, Python, and Ruby. It builds a CodeQL database for the PR diff, combines it with a cached full-repo database, and reports seven-day average speedups that are most noticeable in slower repos (JavaScript/TypeScript up to ~70% for >7-minute baselines; Python ~70%; Ruby ~63%). Constraints: it applies to the default query suite and "build mode none" extraction on github.com, and CodeQL CLI does not support the incremental flow yet.
GitHub is also preparing AI-powered security detections (public preview planned early Q2 2026) to complement CodeQL by covering ecosystems and file types that do not map cleanly to semantic SAST. Initial targets are Shell/Bash, Dockerfiles, Terraform (HCL), and PHP, with findings surfaced in the same PR experience as CodeQL. It tracks with last week''s AI security operations framing: if teams rely on AI interpretation/remediation, signals must still be observable and governable in standard workflows. GitHub also plans to connect detections to Copilot Autofix so developers can review/apply suggestions in PRs and gate merges via policy enforcement.
Secret Scanning push protection gained a new policy surface: push protection exemptions can now be configured in a repository''s settings, not only at org/enterprise level. This follows last week''s central exemption controls by adding repo-level flexibility for the last mile of rollout. Repo admins can exempt Roles, Teams, and GitHub Apps so pushes are not blocked when secrets are detected, with an explicit tradeoff: exemptions are evaluated at push time and exempt pushes will not generate bypass requests, so teams should align exemptions with audit expectations.
- [Faster incremental analysis with CodeQL in pull requests](https://github.blog/changelog/2026-03-24-faster-incremental-analysis-with-codeql-in-pull-requests)
- [GitHub expands application security coverage with AI-powered detections](https://github.blog/security/application-security/github-expands-application-security-coverage-with-ai-powered-detections/)
- [Push protection exemptions from repository settings](https://github.blog/changelog/2026-03-23-push-protection-exemptions-from-repository-settings)
## Microsoft Fabric security: network controls, encryption keys, and Purview-driven data protection
Fabric''s security updates added enforceable controls that do not require app rewrites, while expanding API support for automation and governance. It extends last week''s Fabric/OneLake security direction: keep OneLake/Fabric as a consistent enforcement point as more tools and AI features touch data. Workspace-level IP firewall rules are now GA, allowing workspace admins to restrict inbound access via public IP allowlists once tenant admin enables it. Workspace scope matters: production can be locked to corporate egress IPs while dev stays flexible, layering with Private Link, Entra Conditional Access, outbound protection, and RBAC. This matters for CI/CD, notebooks, Spark, and external services calling Fabric APIs from controlled networks.
Customer-managed keys (CMK) for Fabric SQL Database are now GA, configured at the workspace level and integrated with TDE. Once enabled, TDE is automatically on for all SQL databases in that workspace (including `tempdb`), encrypting data, logs, and backups. Operational focus shifts to Key Vault lifecycle (permissions, rotation, audit), and the post includes verification via `sys.dm_database_encryption_keys` to confirm `encryption_state_desc` is `ENCRYPTED` (or in progress) and `encryptor_type` is `ASYMMETRIC_KEY`, consistent with Key Vault-backed protectors.
Data protection additions leaned into Purview integration. DLP "restrict access" for OneLake (Preview) expanded to structured OneLake stores (SQL databases, KQL databases, Warehouses), enabling policy-based detection to automatically restrict access across more of the estate. Sensitivity labels are now accessible via Fabric public REST APIs (GA) through Core Items APIs (label IDs surfaced in List/Get/Update and supported on Create), enabling automated inventory and compliant creation, though label updates still require admin bulk label APIs. Purview Insider Risk Management added Fabric Lakehouse indicators (GA) and a faster IRM Data Theft policy creation path (GA), plus a PAYG usage report (GA) for processing-unit costs by workload/indicator. For teams using Fabric Copilot and data agents, Purview DSPM for AI (Preview) adds monitoring for sensitive info in prompts/responses with IRM investigation hooks and governance via audit/eDiscovery/retention, continuing last week''s governance-and-observability pattern for day-to-day data work.
- [Workspace level IP firewall rules in Microsoft Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/workspace-level-ip-firewall-rules-in-microsoft-fabric-generally-available/)
- [Customer-managed keys (CMK) in Fabric SQL Database (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-cmk-in-fabric-sql-database-generally-available/)
- [''New data protection capabilities in Microsoft Fabric: Native security for the modern data estate''](https://blog.fabric.microsoft.com/en-US/blog/new-data-protection-capabilities-in-microsoft-fabric-native-security-for-the-modern-data-estate/)
## Other Security News
GitHub''s supply-chain data and IR tooling became more actionable for day-to-day ops. GitHub Security Lab''s 2025 open source vulnerability trends recommended using CVSS alongside EPSS to prioritize what is likely to be exploited soon, noted increased npm malware advisories, and tied that to Dependabot''s ability to alert on known malicious npm package versions. This continues last week''s Dependabot malware-aware additions and can help with tuning alert triage. GitHub also expanded the unauthenticated Credential Revocation API to support revoking exposed OAuth app tokens and GitHub App credentials (including refresh tokens) in bulk, with rate limits (60 requests/hour; up to 1,000 tokens/request) and audit visibility via the token owner''s security log. This fits the "tighten trusted surfaces" theme from last week''s token warnings: rotate/revoke quickly and do not treat token internals as stable contracts.
- [''A year of open source vulnerability trends: CVEs, advisories, and malware''](https://github.blog/security/supply-chain-security/a-year-of-open-source-vulnerability-trends-cves-advisories-and-malware/)
- [Credential revocation API now supports GitHub OAuth and GitHub app credentials](https://github.blog/changelog/2026-03-26-credential-revocation-api-now-supports-github-oauth-and-github-app-credentials)
Microsoft identity and Defender updates emphasized "context + automatic containment," reinforcing last week''s identity-first intrusion story. The identity-security analysis argued attackers are shifting from account compromise to exploiting what identities can reach, including non-human and emerging agent identities, and described Microsoft''s approach across Entra (control plane + Conditional Access) and Defender XDR (threat protection). Updates include an Identity Security dashboard, unified identity risk score, adaptive risk remediation, and a Security Copilot triage agent for identity investigations. Microsoft also described using High Value Asset (HVA) context from the Security Exposure Management graph to be more aggressive on Tier-0 and internet-facing workloads, with examples like blocking `ntdsutil.exe` credential dumping on domain controllers and fast remediation of targeted webshells on Exchange/SharePoint/IIS. Together with last week''s mitigations (phishing-resistant MFA, limiting remote support tools), the thread is consistent: assume initial access looks normal, then use context-aware policy and rapid disruption to stop pivots.
- [Identity security is the new pressure point for modern cyberattacks](https://www.microsoft.com/en-us/security/blog/2026/03/25/identity-security-is-the-new-pressure-point-for-modern-cyberattacks/)
- [How Microsoft Defender protects high-value assets in real-world attack scenarios](https://www.microsoft.com/en-us/security/blog/2026/03/27/microsoft-defender-protects-high-value-assets/)
AI agent governance guidance sharpened around enforceable intent, identity, and runtime checks, building on last week''s move from theory to operations (observability + Zero Trust mapping). Microsoft''s agent governance model proposed a precedence order (organizational intent overrides role-based intent, which overrides developer intent, which overrides user intent) so "what the agent is allowed to do" becomes evaluable and auditable. Complementing that, Azure AI Foundry guidance described implementing agents as Entra-managed identities (service principals), scoping access via Azure RBAC (for example, Storage Blob Data Reader for read-only summarization), and applying guardrails at user input, tool call (preview), tool response (preview), and output to block prompt-injection exfiltration attempts before tools execute.
- [''Governing AI agent behavior: Aligning user, developer, role, and organizational intent''](https://techcommunity.microsoft.com/blog/microsoft-security-blog/governing-ai-agent-behavior-aligning-user-developer-role-and-organizational-inte/4503551)
- [''Securing Azure AI Agents: Identity, Access Control, and Guardrails in Microsoft Foundry''](https://techcommunity.microsoft.com/t5/microsoft-developer-community/securing-azure-ai-agents-identity-access-control-and-guardrails/ba-p/4500242)
Resilience and ransomware defense rounded out the week with administrator-focused examples. A Defender case study showed how "predictive shielding" combined attack disruption with temporary GPO hardening to pause propagation of new GPO policies after detecting tampering, blocking a ransomware attempt staged via SYSVOL (`run.bat`, `run.exe`, `run.dll`) and a scheduled task chain (`cmd /c start run.bat -> ...run.exe -> rundll32 ...run.dll Encryptor`). A video overview of Entra Backup and Restore described daily snapshots of Entra state plus a diff report to understand changes before restoring, positioned as a safety net alongside soft delete and protected actions for recovering from accidental or malicious identity config changes that break authentication/app access. In the context of the two-week identity focus, it''s a reminder that identity control includes recovery, not only prevention.
- [''Case study: How predictive shielding in Defender stopped GPO-based ransomware before it started''](https://www.microsoft.com/en-us/security/blog/2026/03/23/case-study-predictive-shielding-defender-stopped-gpo-based-ransomware-before-started/)
- [Overview of Entra Backup and Recovery](https://www.youtube.com/watch?v=72nowrDIlQU)',
    'This week''s security story centered on CI/CD trust and identity/data control. A real supply-chain compromise hit developer pipelines, while GitHub and Microsoft shared concrete steps to reduce drift: dependency locking, tighter secret scope, faster feedback, and more platform-enforced policy. It also continues last week''s theme: defenders are adding guardrails to default paths (dependency installs, workflow triggers, org rollouts) where attackers keep showing up.',
    1774854000, 'security', '/security/roundups/weekly-security-roundup-2026-03-30', 'TechHub',
    'TechHub', '578BE95B56C257F539F57E2A5D680FCDB13CF4AD645AD54CDEA57E0E120F4652', ',CI/CD Security,Software Supply Chain,GitHub Actions,Dependency Locking,Secret Scanning,CodeQL,SAST,GitHub Advanced Security,Runner Security,Microsoft Defender,Microsoft Entra,Identity Security,Microsoft Fabric,Microsoft Purview,AI Agent Governance,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-03-23
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-03-23', 'roundups', 'Weekly Security Roundup: Guardrails, AI Observability, Identity Attacks',
    'This week''s security story split between tightening default guardrails in developer platforms and dealing with AI-heavy systems and identity-first attacks. Building on last week''s theme of trusted surfaces being tightened while also being abused, these updates land on default paths teams use every day: dependency installs, `git push`, org-wide security rollout, remote support tooling, and AI systems that act on data and tools. GitHub and Azure DevOps shipped changes affecting secrets, dependencies, and auth at scale, while Microsoft security guidance continued last week''s move from AI security theory to operations: make behavior observable and governable, and defend against phishing and support-channel compromises.
<!--excerpt_end-->
## GitHub security controls: malware-aware dependencies, stricter secret policies, and smoother rollout at scale
GitHub''s code security tooling shipped changes likely to affect daily workflows, especially for orgs standardizing security across many repos. After last week''s focus on shifting scanning earlier and treating dev workflows as a control surface, Dependabot now supports **opt-in malware alerts for npm**, comparing your dependency graph to malware advisories in GitHub Advisory Database and producing a separate alert type from CVE vulnerability alerts. The opt-in model (with backfilled results when enabled) is meant to avoid noise that led GitHub to pause similar alerting in 2022. Teams using private registries should tune malware alert rules (ecosystem, package patterns, malicious-version vs malicious-package) to reduce name-collision false positives.
Secret Scanning **push protection** is now more governable at org scale: you can define **exemptions for roles, teams, and GitHub Apps** via security configurations at org or enterprise level. It supports last week''s "stop secrets before remote" storyline: as enforcement becomes more universal, exceptions become centrally manageable. Exemptions apply at push time; exempt actors can push detected secrets without enforcement and without bypass requests, which can help for automation and break-glass flows. Those exemptions still need compensating controls (auditing, least privilege, periodic review) so they do not create blind spots.
On rollout, GitHub introduced a guided **organization-level setup** flow for GitHub Advanced Security to streamline enabling GHAS, managing custom configurations, and targeting repos, reducing inconsistent coverage. Code Quality updates also intersect with secure delivery: developers can **batch-apply multiple Code Quality suggestions** from PR "Files changed" into one commit (one follow-up scan instead of many). GitHub also tightened RBAC so the **security manager role cannot enable/disable Code Quality** unless also a repo admin, which platform teams may need to reflect in runbooks and automation. Overall, the direction matches last week: shift checks earlier, then reduce rollout and permission boundary mistakes.
- [Dependabot now detects malware in npm dependencies](https://github.blog/changelog/2026-03-17-dependabot-now-detects-malware-in-npm-dependencies)
- [Push protection exemptions for roles, teams, and apps](https://github.blog/changelog/2026-03-17-push-protection-exemptions-for-apps-teams-and-roles)
- [GitHub Advanced Security setup made simple](https://github.blog/changelog/2026-03-17-github-advanced-security-setup-made-simple)
- [''GitHub Code Quality: Batch apply quality suggestions on pull requests''](https://github.blog/changelog/2026-03-17-github-code-quality-batch-apply-quality-suggestions-on-pull-requests)
- [Code Quality permissions removed from security manager role](https://github.blog/changelog/2026-03-17-code-quality-permissions-removed-from-security-manager-role)
## Microsoft’s AI security push: observability, Zero Trust guidance, and evaluation for agent-driven detection
Microsoft''s security guidance kept converging on a practical theme: if AI systems make decisions and call tools, security teams need governance controls plus end-to-end visibility into how context is assembled and actions are taken. It extends last week''s day-two focus on prompt-abuse playbooks and agent governance control planes (registry, identity, policy). The missing link is telemetry that can show what an agent saw, why it acted, and which boundaries it crossed.
AI observability guidance argues that classic SRE metrics can be green while an agent violates trust boundaries (for example, indirect prompt injection via retrieved content). The recommendation is to instrument AI apps like distributed systems, with AI-specific capture: correlate by conversation or run across turns, log prompt/response plus identity and tool/data-source provenance, track AI metrics (token usage, retrieval volume, agent turns), and collect traces showing ordered execution from prompt to tool calls. It points to OpenTelemetry conventions and Microsoft options like Foundry agent tracing (preview) and the Agent 365 Observability SDK (Frontier preview).
That visibility thread connects to Microsoft''s **Zero Trust for AI** guidance, extending "verify explicitly / least privilege / assume breach" across ingestion, training, deployment, and agent behavior. It is the policy side of last week''s story: if agents have identities and tool/data access, control mapping must span Identity, Data, and Network, not just model settings. The Zero Trust Workshop adds an AI pillar with scenario-based control mapping, and the Zero Trust Assessment tool expands beyond Identity/Devices into Data and Network. That reflects how AI rollouts often fail on DLP/governance and network enforcement rather than model config. Microsoft also says a dedicated AI assessment pillar is planned for summer 2026.
Microsoft also released **CTI-REALM**, an open benchmark to test whether tool-using AI agents can go from CTI reports to validated detections (iterating on KQL and producing Sigma) across Linux endpoint, AKS, and Azure telemetry, with scoring across intermediate steps. For teams exploring detection-generation agents, it is positioned as a way to measure failure modes (CTI comprehension vs telemetry exploration vs query specificity) before letting generated detections into production workflows, complementing the guardrails and monitoring emphasis of the last two weeks.
- [''Observability for AI Systems: Strengthening visibility for proactive risk detection''](https://www.microsoft.com/en-us/security/blog/2026/03/18/observability-ai-systems-strengthening-visibility-proactive-risk-detection/)
- [''New tools and guidance: Announcing Zero Trust for AI''](https://www.microsoft.com/en-us/security/blog/2026/03/19/new-tools-and-guidance-announcing-zero-trust-for-ai/)
- [''CTI-REALM: A new benchmark for end-to-end detection rule generation with AI agents''](https://www.microsoft.com/en-us/security/blog/2026/03/20/cti-realm-a-new-benchmark-for-end-to-end-detection-rule-generation-with-ai-agents/)
## Identity-first intrusions and seasonal phishing: concrete defender guidance for real campaigns
Microsoft threat and incident-response reporting stayed focused on how compromises often start: social engineering and identity abuse, not zero-days. It continues last week''s pattern of attackers blending into routine engineering and IT habits (interviews, "VPN download" searches, trusted hosting, signed binaries). This week again uses familiar channels (Teams support calls, remote assistance tools, and legitimate cloud infrastructure) to look normal.
Microsoft Incident Response (DART) described a Teams vishing incident: an attacker impersonated IT support, convinced a user to start a Quick Assist session, then redirected them to a spoofed login to steal credentials. The attacker then delivered payloads including a disguised MSI that sideloaded a malicious DLL, then moved via encrypted loaders, proxy connectivity, and living-off-the-land techniques. Mitigations are operational: restrict inbound Teams comms from unmanaged accounts (prefer allowlists for trusted external domains) and inventory/minimize remote assistance tools, potentially disabling Quick Assist where not required.
Microsoft Threat Intelligence also documented multiple tax-season phishing and malware campaigns (Jan-Mar 2026) using W-2/1099/IRS/CPA lures and leveraging legitimate infrastructure (OneDrive, Amazon SES click tracking) plus legitimate remote tools (ScreenConnect, SimpleHelp, Datto-related executables) for hands-on access. Chains use multi-step delivery to evade scanning (Excel -> OneNote on OneDrive -> phishing), QR-based payloads with personalized docs, and bot detection to block sandboxes. Guidance is tactical for Microsoft environments: enable Defender XDR automatic attack disruption, enforce MFA without risky exclusions and use phishing-resistant MFA via Entra Conditional Access where possible, enable ZAP and Safe Links click-time rechecks, turn on Defender for Endpoint network protection, and use the provided KQL/IOCs in Defender XDR and Sentinel (including ASIM and TI Mapping). The operational takeaway matches last week: treat normal workflows as contested and harden identity and execution paths users rely on daily.
- [''Help on the line: How a Microsoft Teams support call led to compromise''](https://www.microsoft.com/en-us/security/blog/2026/03/16/help-on-the-line-how-a-microsoft-teams-support-call-led-to-compromise/)
- [''When tax season becomes cyberattack season: beware these lures''](https://www.microsoft.com/en-us/security/blog/2026/03/19/when-tax-season-becomes-cyberattack-season-phishing-and-malware-campaigns-using-tax-related-lures/)
## Security for Microsoft Fabric and OneLake: centralized policy enforcement meets AI-era governance
Fabric''s security and governance surface expanded with two related themes: enforce access consistently across engines, and reduce oversharing risk as AI features consume more data. It echoes last week''s "identity moving closer to the data plane" theme: governance needs to be enforceable where data is queried, even outside Microsoft''s engines.
Fabric introduced **OneLake security APIs** so third-party query engines can enforce OneLake permissions (table permissions plus RLS/CLS) at query time. The authorized engine model keeps OneLake as the source of truth while external engines retrieve security definitions and apply them during execution. OneLake also pre-computes effective access so engines do not need to reproduce role evaluation. For teams running multiple engines over Delta and Iceberg, this provides a contract for consistent governance without duplicating data or re-implementing auth per tool.
Microsoft Purview added deeper Fabric coverage aimed at reducing leakage risk as Copilots and agents become part of Fabric workflows. Purview DLP policy tips for sensitive data in Fabric Warehouse uploads are GA, with preview enforcement that can restrict access when sensitive data is detected (KQL/SQL databases and Fabric Warehouses). Insider Risk Management expands Fabric coverage to lakehouses (GA) with indicators tied to risky sharing behavior and adds policies/reporting for data theft/exfiltration. For AI inside Fabric, Purview adds preview controls to discover sensitive data in prompts/responses, identify overshared assets via DSPM assessments, and tie AI usage into Audit/eDiscovery/retention and non-compliant usage detection. Unified Catalog updates (publication workflows GA; data quality checks for ungoverned assets) bring trust signals closer to where engineers discover and use data. Combined with last week''s agent governance theme, the direction is that as AI touches more data by default, enforcement and audit need to follow the data regardless of the query engine.
- [Third-party support for OneLake security](https://blog.fabric.microsoft.com/en-US/blog/third-party-support-for-onelake-security/)
- [New Microsoft Purview innovations for Fabric to safely accelerate your AI transformation](https://techcommunity.microsoft.com/blog/microsoft-security-blog/new-microsoft-purview-innovations-for-fabric-to-safely-accelerate-your-ai-transf/4502156)
## Other Security News
Azure DevOps integrations got a breaking-change warning: tokens will be further encrypted this summer, so code that decodes token payloads and treats claims as a stable contract should be replaced with supported API lookups (with caching where needed). It fits the "trusted surfaces are being tightened" trend: reduce reliance on internals and push teams toward supported identity boundaries.
- [Authentication Tokens Are Not a Data Contract](https://devblogs.microsoft.com/devops/authentication-tokens-are-not-a-data-contract/)
A newly reported AutoMapper issue is a reminder that convenience libraries can become DoS surfaces: some mapping over cyclical or deeply self-referential graphs can recurse until StackOverflowException terminates the process, so teams should audit mappings reachable from untrusted inputs and add depth limits/ignores where needed.
- [How AutoMapper Can Crash Your .NET Server](https://www.youtube.com/watch?v=FQdu5cyvb5k)
Azure compliance automation got attention with built-in CIS Benchmarks framed as platform-integrated baselines you can validate continuously, especially for hybrid/regulated and Linux-heavy fleets.
- [Built‑In CIS Benchmarks on Microsoft Azure](https://www.thomasmaurer.ch/2026/03/built-in-cis-benchmarks-on-microsoft-azure/)
Microsoft''s RSAC 2026 roundup collected "secure agentic AI end-to-end" updates across Entra, Purview, Defender, Sentinel, and Security Copilot, highlighting how network-layer prompt injection policies, expanded DLP for Copilot grounding, and agent governance/inventory are shaping deployment and operations in Microsoft ecosystems. It reads as a recap-and-extension of last week (registry/identity/governance) and this week (observability + Zero Trust mapping): controls are being named, shipped, and connected across identity, data governance, and monitoring.
- [''Microsoft at RSAC Conference March 22-26: Secure agentic AI end-to-end''](https://www.microsoft.com/en-us/security/blog/2026/03/20/secure-agentic-ai-end-to-end/)',
    'This week''s security story split between tightening default guardrails in developer platforms and dealing with AI-heavy systems and identity-first attacks. Building on last week''s theme of trusted surfaces being tightened while also being abused, these updates land on default paths teams use every day: dependency installs, `git push`, org-wide security rollout, remote support tooling, and AI systems that act on data and tools. GitHub and Azure DevOps shipped changes affecting secrets, dependencies, and auth at scale, while Microsoft security guidance continued last week''s move from AI security theory to operations: make behavior observable and governable, and defend against phishing and support-channel compromises.',
    1774252800, 'security', '/security/roundups/weekly-security-roundup-2026-03-23', 'TechHub',
    'TechHub', '1BEBD9299241B24E450A4BDA56A2766278056A6FEFC8B99F142DA46724D9A83B', ',GitHub Advanced Security,Dependabot,npm,Malware Alerts,Secret Scanning,Push Protection,RBAC,Zero Trust,AI Observability,OpenTelemetry,Microsoft Entra,Conditional Access,Phishing,Microsoft Defender XDR,Microsoft Fabric,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-03-16
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-03-16', 'roundups', 'Weekly Security Roundup: Identity Controls and Dev Workflow Abuse',
    'Security coverage followed a consistent theme: trusted developer surfaces are being tightened while also being actively abused. After last week’s authentication weaknesses (OAuth redirection abuse, AiTM phishing) and supply-chain controls (Dependabot workflow improvements, AI-assisted vulnerability discovery), this week shows convergence on default surfaces. Identity is moving closer to the data plane (even SFTP), GitHub scanning is shifting earlier in workflows, and attackers are blending into routine engineering habits (interview repositories, “VPN download” searches). AI security also continued the shift noted last week from theory to operations, with more guidance on monitoring, audit, and governance as agentic tools land in enterprises.
<!--excerpt_end-->
## Identity-centered secure access for file transfer (Azure Blob Storage SFTP + Entra ID)
After last week’s identity abuse coverage, this is the defensive counterpart: reduce identity islands by bringing legacy access under central policy. Azure Blob Storage SFTP is adding public preview support for Microsoft Entra ID authentication, replacing local storage-account SFTP users that required separate identities, passwords/keys, and offboarding. Users (including B2B guests via Entra External Identities) authenticate with Entra ID and receive a short-lived SSH certificate per session, which reduces reliance on long-lived secrets and aligns SFTP with enterprise identity controls.
Authorization and operations change with this model. Access is enforced via Azure RBAC/ABAC plus POSIX-like ACLs for path permissions, unifying access semantics across SFTP, REST, and Azure CLI. MFA, Conditional Access, Identity Protection, and PIM apply directly to SFTP integrations, and offboarding becomes disabling the Entra identity or revoking access rather than rotating keys and removing local users. For regulated partner/vendor exchange, this supports time-bound, policy-driven access with centralized auditing. Guidance is to enroll and validate in non-production. Local users still exist, but the direction is toward Entra-backed access as the default model.
- [Enterprise Identity Meets Secure File Transfer: Entra ID Public Preview for Azure Blob Storage SFTP](https://techcommunity.microsoft.com/t5/azure-storage-blog/enterprise-identity-meets-secure-file-transfer-entra-id-public/ba-p/4501937)
## GitHub Advanced Security updates: sharper code scanning and stronger leak prevention
Continuing last week’s shift-left and supply-chain thread, GitHub’s updates focused on improving what analysis can model and preventing secrets from reaching repositories.
CodeQL 2.24.3 expands coverage and modeling. Java/Kotlin scanning supports Java 26 and improves Maven builds by reading Java version from POMs (and trying Java 17+ when needed), reducing toolchain mismatch failures. Modeling also improves: better detection of MobX `observer` React components for JS/TS, improved Python SSRF analysis via a new AntiSSRF sanitization barrier model, and better interpretation of boolean guards like `isSafe(x) == true` / `!= false` to reduce false positives. Ruby taint tracking now follows `Shellwords.escape` / `shellescape` (with exceptions for command-injection queries), Rust adds neutral models to ease custom sources/sinks/summaries, C/C++ refines a query to reduce false positives, and C# parsing supports the `field` keyword (C# 14). Since CodeQL rolls out automatically on GitHub.com, teams should expect alert shifts and plan for scan re-runs and review of custom packs/models.
GitHub’s March 2026 secret scanning update adds 28 detectors across 15 providers and expands default push protection so more leaks are blocked at `git push` time. This ties to last week’s structured ownership/triage theme: earlier blocking reduces rotation and incident coordination later. New Azure/Entra patterns for `azure_active_directory_application_id` and `azure_active_directory_application_secret` catch common app artifacts before commit. Validators also expand (Airtable, DeepSeek, npm, Pinecone, Sentry), helping triage by confirming active credentials vs noise. Overall, it pushes security earlier into the developer loop: fewer scan failures, fewer false positives, and more secrets stopped before they reach the remote.
- [CodeQL 2.24.3 Release: Java 26 Support and Enhanced Static Analysis](https://github.blog/changelog/2026-03-10-codeql-2-24-3-adds-java-26-support-and-other-improvements)
- [GitHub Secret Scanning Pattern Updates — March 2026](https://github.blog/changelog/2026-03-10-secret-scanning-pattern-updates-march-2026)
## Threat research: attackers blending into developer workflows (interviews, GitHub assets, and signed installers)
Last week highlighted identity abuse and stolen EV cert signing. This week extends the same theme into common distribution and execution paths that developers and IT already treat as routine. These campaigns do not need exotic exploits if they can embed in everyday workflows.
Microsoft’s “Contagious Interview” report shows recruitment social engineering wrapped around normal coding tasks. Targets get assignments that involve cloning a repository and installing/running dependencies (often npm) from platforms like GitHub/GitLab/Bitbucket. VS Code workspace trust is a key hinge: once trusted, task files can run background commands, which bridges “open repo” to “execute code.” The campaign uses multiple payload families (Invisible Ferret, FlexibleFerret, BeaverTail/OtterCookie) for backdoors, persistence, and theft, targeting source, CI/CD tokens, cloud credentials, code-signing keys, password stores, and wallets. Guidance is operational: treat hiring pipelines as an attack surface, isolate interview environments from corporate credentials, add review/approval gates before running external code, and hunt for suspicious Node/Python behavior, download-and-execute patterns, and unusual outbound infrastructure.
Microsoft Threat Intelligence also described Storm-2561 using SEO poisoning so VPN-client searches land on lookalike domains serving trojanized installers, sometimes hosted as GitHub release assets to blend into a trusted channel. The chain uses a ZIP with an MSI, a signed executable, and malicious DLL side-loading (for example, `dwmapi.dll`, `inspector.dll`) in legitimate-looking paths. The fake VPN captures credentials, steals VPN configs, persists via `RunOnce`, and uses an in-memory loader to deploy Hyrax infostealer. Defender mitigations include cloud protection, EDR in block mode, web/network protection, ASR rules, SmartScreen, and hunting for suspicious signed binaries (including the cited certificate subject) and unusual DLL loads in VPN directories. The takeaway is practical: tighten acquisition habits (especially search → download) and treat GitHub-hosted binaries as requiring provenance checks and reputation-aware controls, because “looks legitimate” signals are being used as cover.
- [‘Contagious Interview: Malware delivered through fake developer job interviews’](https://www.microsoft.com/en-us/security/blog/2026/03/11/contagious-interview-malware-delivered-through-fake-developer-job-interviews/)
- [Storm-2561 Distributes Fake VPN Clients via SEO Poisoning for Credential Theft](https://www.microsoft.com/en-us/security/blog/2026/03/12/storm-2561-uses-seo-poisoning-to-distribute-fake-vpn-clients-for-credential-theft/)
## Securing enterprise AI usage: prompt abuse playbooks and agent governance control planes
Last week covered attackers operationalizing AI and the need to protect AI usage (including malicious extensions harvesting chat histories). This week adds day-two defensive detail: detecting/responding to prompt abuse and how identity/governance may apply to agents in enterprise environments.
Microsoft Incident Response guidance focuses on indirect prompt injection as an operational risk. Hidden instructions embedded in ingested content (including URL fragments after `#`) can be pulled into prompts and silently steer outputs. It categorizes attacker patterns (direct overrides, extractive attempts, indirect injection) and maps detection/response to common Microsoft stacks: discover sanctioned vs shadow AI usage with Defender for Cloud Apps and Purview DSPM; monitor with Purview DLP and CloudAppEvents; constrain with Entra Conditional Access plus DLP; and investigate via Sentinel, Purview audit logs, and Entra identity signals. Engineering takeaways include explicit sanitization in ingestion pipelines (including stripping/normalizing URL fragments) and treating AI tool usage as observable and policy-governed, like other data-handling systems.
Microsoft also introduced Agent 365 and Microsoft 365 E7: The Frontier Suite as a security/admin control plane for agentic AI, including third-party agents. It centers on an Agent Registry (inventory), observability reports, near real-time risk evaluation using Defender/Entra/Purview signals, and an “Agent ID” model where agents get Entra identities so Conditional Access and identity governance apply like they do to users. It also mentions inline DLP for Copilot Studio agents, sensitivity label inheritance, auditing/eDiscovery/retention for agent data, and Defender protections for prompt manipulation and model tampering. For developers, this implies more deployment requirements - registration, identity assignment, least privilege, label/DLP compliance, auditability - becoming part of “done.” (Availability is stated as May 1, 2026, with Agent 365 at $15/user/month and E7 at $99/user/month.)
- [Detecting and Analyzing Prompt Abuse in AI Tools](https://www.microsoft.com/en-us/security/blog/2026/03/12/detecting-analyzing-prompt-abuse-in-ai-tools/)
- [‘Securing Agentic AI with Microsoft Agent 365 and Microsoft 365 E7: The Frontier Suite’](https://www.microsoft.com/en-us/security/blog/2026/03/09/secure-agentic-ai-for-your-frontier-transformation/)
## Other Security News
Mark Russinovich described using Anthropic Claude Opus 4.6 to decompile and reason about 6502 machine code from an Apple II utility he wrote in 1986. The model flagged a silent incorrect-behavior bug (not checking the carry flag after a line-lookup routine). The takeaway is not that this specific tool is exploitable. It is that LLMs can increasingly help reason about binaries and legacy code (useful for firmware and long-lived systems), while still requiring disciplined verification to manage noise.
- [Microsoft Azure CTO Uses AI to Discover Vulnerabilities in Legacy Apple II Code](https://www.devclass.com/security/2026/03/11/microsoft-azure-cto-set-claude-on-his-1986-apple-ii-code-says-it-found-vulns/5208875)',
    'Security coverage followed a consistent theme: trusted developer surfaces are being tightened while also being actively abused. After last week’s authentication weaknesses (OAuth redirection abuse, AiTM phishing) and supply-chain controls (Dependabot workflow improvements, AI-assisted vulnerability discovery), this week shows convergence on default surfaces. Identity is moving closer to the data plane (even SFTP), GitHub scanning is shifting earlier in workflows, and attackers are blending into routine engineering habits (interview repositories, “VPN download” searches). AI security also continued the shift noted last week from theory to operations, with more guidance on monitoring, audit, and governance as agentic tools land in enterprises.',
    1773648000, 'security', '/security/roundups/weekly-security-roundup-2026-03-16', 'TechHub',
    'TechHub', 'D4282AB681BDD79C91882872F605DF7F909CD948C7F173455D50FDBDD32F22BC', ',Microsoft Entra ID,Azure Blob Storage,SFTP,SSH Certificates,Conditional Access,Azure RBAC,GitHub Advanced Security,CodeQL,Secret Scanning,Push Protection,Static Analysis,Supply Chain Security,SEO Poisoning,DLL Side Loading,Prompt Injection,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-03-09
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-03-09', 'roundups', 'Weekly Security Roundup: OAuth abuse, MFA bypass, and supply chain',
    'Security this week covers both new threats and updated controls, with research on attacks using authentication weaknesses, vulnerability management, software supply chain changes, and the dual role of AI in state-of-the-art threats and defense.
<!--excerpt_end-->
## Microsoft Account and Authentication Attack Analysis
The latest research details real-world attacks using OAuth application redirection in Entra ID for phishing or malware delivery. Attackers exploit OAuth parameters to redirect users to malicious URLs. Practical steps for defenders include tightening app permissions, monitoring authentication flows, and enforcing stronger access policies. There is also analysis of malware signed with stolen EV certificates, which is then used to install RMM tools as persistent footholds. Recommendations are provided for auditing, whitelisting, deploying AppLocker, and reviewing RMM credentials.
- [OAuth Redirection Abuse Tactics: Phishing and Malware Delivery Exposed](https://www.microsoft.com/en-us/security/blog/2026/03/02/oauth-redirection-abuse-enables-phishing-malware-delivery/)
- [Signed Malware Impersonating Workplace Apps Deploys RMM Backdoors](https://www.microsoft.com/en-us/security/blog/2026/03/03/signed-malware-impersonating-workplace-apps-deploys-rmm-backdoors/)
## Global Disruption of Advanced Phishing Kits and MFA Bypass Platforms
A partnership between Microsoft, Europol, and global agencies dismantled the Tycoon 2FA phishing kit infrastructure, which previously enabled large-scale identity impersonation. Tactics included relay, session hijacking, AI-based phishing, and rapid domain switching. The articles offer mitigation steps, including MFA reviews, mailbox rule checks, token revocation, Defender tools, and post-attack clean-up.
- [How a Global Coalition Disrupted Tycoon 2FA: A Major Impersonation Platform](https://blogs.microsoft.com/on-the-issues/2026/03/04/how-a-global-coalition-disrupted-tycoon/)
- [Inside Tycoon2FA: How a Leading AiTM Phishing Kit Operated at Scale](https://www.microsoft.com/en-us/security/blog/2026/03/04/inside-tycoon2fa-how-a-leading-aitm-phishing-kit-operated-at-scale/)
## AI-Enabled Security Threats, Detection, and Response
Microsoft Threat Intelligence describes how attackers are adopting AI for scanning vulnerabilities, developing malware, social engineering, and building infrastructure. LLMs are now sometimes used for reconnaissance, with North Korean groups highlighted. Defender recommendations include enabling AI-focused dashboards, prompt filter controls, and Defender Copilot products. The section also warns of AI browser extensions that steal chat data, with guidance on detection, extension auditing, and Defender management.
- [AI as Tradecraft: Threat Actors Operationalize AI in Cyberattacks](https://www.microsoft.com/en-us/security/blog/2026/03/06/ai-as-tradecraft-how-threat-actors-operationalize-ai/)
- [Malicious AI Browser Extensions Expose LLM Chat Histories: Microsoft Defender Analysis](https://www.microsoft.com/en-us/security/blog/2026/03/05/malicious-ai-assistant-extensions-harvest-llm-chat-histories/)
## Secure Software Supply Chain and User Controls
Dependabot now allows security alert assignment for better accountability. Bulk assignment and additional webhook/API support enable more structured response. Draft advisories can be locked for authorized edit only, improving audit trail and compliance support.
- [Dependabot Alert Assignees Now Available for GitHub Advanced Security](https://github.blog/changelog/2026-03-03-dependabot-alert-assignees-are-now-generally-available)
- [Improved Control Over Draft Repository Security Advisories on GitHub](https://github.blog/changelog/2026-03-04-lock-and-unlock-draft-repository-security-advisories)
## Automation and AI-Driven Vulnerability Discovery
The GitHub Security Lab Taskflow Agent enables automated vulnerability scanning using YAML-based taskflows and LLMs. Documentation covers review of logic, control, and privilege tasks in your pipeline. User taskflows, integration with Codespaces, and Copilot support help teams act on findings more efficiently.
- [How to Scan for Vulnerabilities with GitHub Security Lab''s Open Source AI-Powered Framework](https://github.blog/security/how-to-scan-for-vulnerabilities-with-github-security-labs-open-source-ai-powered-framework/)
## Secure Binary Signing with Azure Trusted Signing and dotnet sign
Rick Strahl details workflow improvements for code signing using Azure Trusted Signing and dotnet sign. Changes include simpler authentication, faster processes, and support for CI/CD scripts, covering details about configuration, integration, and security best practices. The overview addresses protecting certificates and ensuring responsible automated signoff.
- [Azure Trusted Signing Revisited with Dotnet Sign](https://weblog.west-wind.com/posts/2026/Mar/02/Azure-Trusted-Signing-Revisited-with-Dotnet-Sign)
## Other Security News
GitHub Octoverse analysis highlights how automation tools like Copilot Autofix and Dependabot speed up vulnerability patching and support a "shift left" approach. Features discussed include merge queues and new access control checks supported by insights, reinforcing the balance between AI-driven automation and human oversight.
- [How AI is Changing the ''Shift Left'' Mindset in Security – Insights from GitHub Octoverse 2025](/ai/videos/how-ai-is-changing-the-shift-left-mindset-in-security-insights-from-github-octoverse-2025)',
    'Security this week covers both new threats and updated controls, with research on attacks using authentication weaknesses, vulnerability management, software supply chain changes, and the dual role of AI in state-of-the-art threats and defense.',
    1773043200, 'security', '/security/roundups/weekly-security-roundup-2026-03-09', 'TechHub',
    'TechHub', '05AA045EBA41ECBD3F09FD53377C9327F4BFB153C2E8F425B5D69E99F34B09AA', ',Microsoft Entra ID,OAuth,Phishing,AiTM Phishing,MFA Bypass,Session Hijacking,Microsoft Defender,RMM Tools,EV Code Signing Certificates,Azure Trusted Signing,.NET Sign,GitHub Advanced Security,Dependabot,Software Supply Chain Security,LLM Security,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-03-02
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-03-02', 'roundups', 'Weekly Security Roundup: Supply Chain, AI Threats, and Identity',
    'Security updates this week highlight new threat trends, code analysis improvements, and cloud identity features. Tools and case studies cover automated detection, zero trust architectures, and practical vulnerability management.
<!--excerpt_end-->
## Supply Chain Attacks and Secure Development Environments
In-depth investigation details new supply chain attacks using malicious Next.js repositories that abuse build automation and workspace trust, including script injection, environment variable extraction, and persistent command and control in developer environments. The response plan recommends hardening IDEs, monitoring asset changes, and following outlined KQL queries for detection, extending last week’s focus on threat response for open-source workflows.
- [Malicious Next.js Repositories Used in Developer-Targeting Attack: RCE and C2 via Build Workflows](https://www.microsoft.com/en-us/security/blog/2026/02/24/c2-developer-targeting-campaign/)
## Code Analysis and Vulnerability Management
CodeQL 2.24.2 introduces support for Go 1.26, Kotlin 2.3.10, and upgrades security scanning, including improved antiforgery checks for Python, Java, and C#. Dependabot alert fatigue is discussed, with recommendations for more context-aware, actionable vulnerability alerts and the use of alternative tools for critical-path security checks.
- [CodeQL 2.24.2: Go 1.26, Kotlin 2.3.10 Support and Query Accuracy Improvements](https://github.blog/changelog/2026-02-24-codeql-adds-go-1-26-and-kotlin-2-3-10-support-and-improves-query-accuracy)
- [Critique of GitHub Dependabot: Alert Fatigue and Security Shortcomings](https://www.devclass.com/security/2026/02/26/github-dependabot-is-a-noise-machine-and-should-be-turned-off-says-go-library-maintainer/4091858)
## Securing AI-Driven Workflows and Zero Trust Architectures
Guides detail threat modeling for AI applications, including non-deterministic behavior, prompt controls, and human-in-the-loop review. Secure cloud demos use Entra ID, Key Vault, and least privilege to control agent tools and access, building toward auditable zero trust AI workflows.
- [Threat Modeling AI Applications: Adapting Security Practices for Modern AI Systems](https://www.microsoft.com/en-us/security/blog/2026/02/26/threat-modeling-ai-applications/)
- [Zero-Trust Security for Autonomous AI Agents in Azure AI Foundry](/ai/videos/zero-trust-security-for-autonomous-ai-agents-in-azure-ai-foundry)
## Cloud Identity, Storage, and Access Governance
Azure Storage now previews SAS delegation bound to Entra ID users to enforce fine-grained access and traceability. Entra ID Access Packages simplify onboarding/offboarding, enable just-in-time grants, and improve compliance workflows.
- [Public Preview: Restrict Usage of User Delegation SAS to an Entra ID Identity](https://techcommunity.microsoft.com/t5/azure-storage-blog/public-preview-restrict-usage-of-user-delegation-sas-to-an-entra/ba-p/4497196)
- [Simplifying Access Governance with Microsoft Entra ID Access Packages](/security/videos/simplifying-access-governance-with-microsoft-entra-id-access-packages)
## Security Automation, AI-Assisted Operations, and Data Governance
Security operations centers (SOC) are embracing automation with Defender XDR and agent-based models, supporting expert/hybrid workflows for alerting and policy across Copilot, ChatGPT, and Gemini. Demos show automated incident management and data security policy enforcement.
- [Scaling Security Operations with Microsoft Defender Autonomous Defense and Expert-Led Services](https://www.microsoft.com/en-us/security/blog/2026/02/24/scaling-security-operations-with-microsoft-defender-autonomous-defense-and-expert-led-services/)
- [Securing AI Adoption with Microsoft''s Data Security Posture Management (DSPM) for AI](/ai/videos/securing-ai-adoption-with-microsofts-data-security-posture-management-dspm-for-ai)
- [Security Copilot in Action: From Alert to Remediation in 25 Minutes](/ai/videos/security-copilot-in-action-from-alert-to-remediation-in-25-minutes)
## Other Security News
GitHub Enterprise Cloud adds IP allow list controls for Enterprise Managed User namespaces, unifying access policies and network controls across organization boundaries.
- [IP Allow List Now Supports Enterprise Managed User Namespaces in GitHub Enterprise Cloud](https://github.blog/changelog/2026-02-23-ip-allow-list-coverage-extended-to-emu-namespaces-in-public-preview)
Guidance on Windows code signing flags issues with Microsoft’s timestamp server, recommending alternative providers for reliable builds.
- [Don''t use the Microsoft Timestamp Server for Signing](https://weblog.west-wind.com/posts/2026/Feb/26/Dont-use-the-Microsoft-Timestamp-Server-for-Signing)',
    'Security updates this week highlight new threat trends, code analysis improvements, and cloud identity features. Tools and case studies cover automated detection, zero trust architectures, and practical vulnerability management.',
    1772438400, 'security', '/security/roundups/weekly-security-roundup-2026-03-02', 'TechHub',
    'TechHub', '5977B637DD2839446267AF30BFBD1FAF16F741D092A96205E4134CF9D3CCA3FA', ',Supply Chain Security,Next.js,Malicious Repositories,Developer Environment Security,IDE Hardening,KQL,GitHub CodeQL,SAST,Dependabot,Vulnerability Management,Threat Modeling,Zero Trust,Microsoft Entra ID,Azure Storage SAS,Microsoft Defender XDR,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-02-23
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-02-23', 'roundups', 'Weekly Security Roundup: Secrets, Supply Chains, and Trust',
    'Security updates focus on better credential and secret management, updated supply chain health, runtime agent isolation, digital content verification, and process improvement for proactive risk management. New tools and practices help developers and organizations safeguard workflows against new threats.
<!--excerpt_end-->
## GitHub Enterprise Credential Management and Secret Scanning
GitHub Enterprise Cloud introduces credential management for instant incident response, letting organization admins and trusted operators review and revoke credentials with complete audit logging. Secret scanning now examines additional metadata for better alert quality and faster response.
These features add security automation and oversight, continuing themes from recent updates to identity and auditing.
- [Enterprise-Wide Credential Management Tools for GitHub Incident Response](https://github.blog/changelog/2026-02-17-enterprise-wide-credential-management-tools-for-incident-response)
- [Secret Scanning Improvements: Extended Metadata Checks on GitHub](https://github.blog/changelog/2026-02-18-secret-scanning-improvements-to-extended-metadata-checks)
## Supply Chain Security: npm CLI and Open Source AI Libraries
npm CLI v11.10.0 and above brings bulk OIDC trusted publish and improved script security. The new "npm trust" command streamlines configuration, and the "--allow-git" flag locks down git dependencies in npm install. GitHub’s Secure Open Source Fund supports 67 AI-related packages, adding automated scanning and enforcement for better supply chain protection.
These features continue recent efforts to reduce package management risk across Node.js and AI projects.
- [npm Bulk Trusted Publishing and Script Security Features Released](https://github.blog/changelog/2026-02-18-npm-bulk-trusted-publishing-config-and-script-security-now-generally-available)
- [Securing the AI Software Supply Chain: Security Results Across 67 Open Source Projects](https://github.blog/open-source/maintainers/securing-the-ai-software-supply-chain-security-results-across-67-open-source-projects/)
## Self-Hosted Agent Runtimes: OpenClaw Identity, Isolation, and Monitoring
Microsoft shares best practices for self-hosting OpenClaw agents, recommending isolation, least-privilege credentials, rigorous monitoring, regular rebuilding, and established incident response policies. Defender XDR, Entra ID, Sentinel, and Purview integration are suggested for minimizing risk. Developers are advised to follow published deployment guidance for safety.
Last week’s identity and role management updates continue here in the context of runtime agent controls and supply chain defense.
- [Securing OpenClaw Self-hosted Agents: Identity, Isolation, and Runtime Risk](https://www.microsoft.com/en-us/security/blog/2026/02/19/running-openclaw-safely-identity-isolation-runtime-risk/)
## Digital Media Authentication: AI, Provenance, and Emerging Standards
Microsoft’s analysis of AI in digital media highlights the importance of provenance, watermarking, and fingerprinting—considering the role of the C2PA standard and encouraging multi-layer safeguards to build trust in online content.
The approach to media authentication blends technical steps and governance, continuing conversations on trust and verification in last week’s coverage.
- [How AI Is Changing Media Trust and Authentication Online](https://news.microsoft.com/signal/articles/a-new-study-explores-how-ai-shapes-what-you-can-trust-online/)
## Other Security News
A maturity model guide helps organizations conduct proactive security exposure management with Microsoft’s available tools, supporting the five levels of defense with actionable recommendations for visibility, integration, and risk alignment. SIEM and XDR use is encouraged for continuous improvement.
This guidance builds on recent organizational security advice about incident response, monitoring, and analytics.
- [Establishing a Proactive Defense with Microsoft Security Exposure Management: New Maturity Model Guide](https://www.microsoft.com/en-us/security/blog/2026/02/19/new-e-book-establishing-a-proactive-defense-with-microsoft-security-exposure-management/)',
    'Security updates focus on better credential and secret management, updated supply chain health, runtime agent isolation, digital content verification, and process improvement for proactive risk management. New tools and practices help developers and organizations safeguard workflows against new threats.',
    1771833600, 'security', '/security/roundups/weekly-security-roundup-2026-02-23', 'TechHub',
    'TechHub', '3245647D642BAE63140FFDB8078BC65F853DAAEBCA36A50F075AF399F7899A6D', ',GitHub Enterprise Cloud,Credential Management,Incident Response,Audit Logging,Secret Scanning,Software Supply Chain Security,npm CLI,OIDC Trusted Publishing,Open Source Security,AI Libraries,Self Hosted Agents,Runtime Isolation,Least Privilege,C2PA,Security Exposure Management,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-02-16
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-02-16', 'roundups', 'Weekly Security Roundup: Agent Identity and LLM Attack Surfaces',
    'Recent security work focuses on the challenges of agent-driven and automated cloud systems—including access management, large language model (LLM) alignment, and defending against misconfiguration or malicious input.
<!--excerpt_end-->
## Securing Multi-Agent AI and Identity Delegation
Multi-agent AI environments raise complex access and identity challenges. Microsoft''s new guides cover secure orchestration and user/agent actions delegation via Entra ID’s On-Behalf-Of flow, using frameworks like LangGraph, Chainlit, and Databricks Genie, and enabling zero-trust RBAC through Unity Catalog. Solutions for OAuth token scopes, custom providers, and audience management are explained, alongside audit and human oversight best practices for accountable agent automation.
This extends last week’s focus on practical patterns for safe, large-scale agent deployments.
- [Securing Multi-Agent AI Solutions with Microsoft Entra ID On-Behalf-Of Flow](https://techcommunity.microsoft.com/t5/azure-architecture-blog/securing-a-multi-agent-ai-solution-focused-on-user-context-the/ba-p/4493308)
- [Securing Multi-Agent AI with User Context: Entra ID OBO for Databricks Genie](https://techcommunity.microsoft.com/t5/azure-architecture-blog/securing-multi-agent-ai-with-user-context-entra-id-obo-for/ba/p/4493308)
## Model Alignment, Memory Poisoning, and AI System Attack Surfaces
Recent research reveals that enterprise LLMs are vulnerable to alignment attacks: for example, one adversarial prompt can undermine safety when using Group Relative Policy Optimization during fine-tuning. Developers are reminded to run benchmarks throughout adaptation cycles to detect model drift. The Defender security team also discusses memory poisoning attacks—where prompt injection targets Copilot or similar tools—offering guidelines for detection, filtering, interface design, and memory controls.
This section continues last week’s coverage of model lifecycle and runtime safety. Behavioral monitoring and input validation are necessary to keep AI-driven systems secure.
- [A One-Prompt Attack That Breaks LLM Safety Alignment](https://www.microsoft.com/en-us/security/blog/2026/02/09/prompt-attack-breaks-llm-safety/)
- [Protecting AI Systems Against Memory Poisoning: The Rise of AI Recommendation Poisoning](https://www.microsoft.com/en-us/security/blog/2026/02/10/ai-recommendation-poisoning/)
## Copilot Studio and Power Platform Agent Security
Copilot Studio automation introduces new security requirements. Microsoft’s top 10 risk list provides tactics and mitigation tips: enforcing authentication, moving secrets to Key Vault, reviewing dormant agents, and restricting command scopes. This helps both no-code and low-code environments manage exposure as agent automation grows.
These practical guides pick up where last week’s discussion of operational guardrails left off and provide actionable checklists for teams working with agent platforms.
- [Copilot Studio Agent Security: Top 10 Risks and How to Mitigate Them](https://www.microsoft.com/en-us/security/blog/2026/02/12/copilot-studio-agent-security-top-10-risks-detect-prevent/)
## Other Security News
Dependabot audit logs have expanded to capture all enable/disable and config activities (including for self-hosted runners), recording user identity for better compliance and traceability.
Building on last week’s improvements to Dependabot OIDC and registry security, these updates support safer CI/CD and supply chains.
- [Track Additional Dependabot Configuration Changes in Audit Logs](https://github.blog/changelog/2026-02-10-track-additional-dependabot-configuration-changes-in-audit-logs)
Microsoft has released a SIEM buyer’s guide explaining how to pick platforms that are ready for AI-based security management. Sentinel and other products offer model-based analytics and automation, in line with current best practices.
- [The Strategic SIEM Buyer’s Guide: Selecting an AI-Ready Security Platform](https://www.microsoft.com/en-us/security/blog/2026/02/11/the-strategic-siem-buyers-guide-choosing-an-ai-ready-platform-for-the-agentic-era/)',
    'Recent security work focuses on the challenges of agent-driven and automated cloud systems—including access management, large language model (LLM) alignment, and defending against misconfiguration or malicious input.',
    1771228800, 'security', '/security/roundups/weekly-security-roundup-2026-02-16', 'TechHub',
    'TechHub', '68ADFEAC9AD0C7D6321878D1932C4CF9B9F7A4F8597EA0B880A0F50C694F6E2F', ',Microsoft Entra ID,On Behalf Of,OAuth 2.0,Token Scopes,Zero Trust,RBAC,Databricks,Unity Catalog,Multi Agent AI,LLM Alignment,Prompt Injection,Memory Poisoning,Copilot Studio,Azure Key Vault,Dependabot Audit Logs,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-02-09
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-02-09', 'roundups', 'Weekly Security Roundup: Python Malware, Supply Chain, and AI Guardrails',
    'This week’s security notes cover changing malware threats, improved automation, new defensive features, and more robust AI/data security. Recent incidents span macOS, Windows, and web threats.
<!--excerpt_end-->
## Defender XDR and Threat Analysis Campaigns
Microsoft reports new infostealer malware (DigitStealer, AMOS, MacSync, PXA Stealer) now targeting both macOS and other platforms, using Python tooling and fake app bundles to steal logins. Teams get new Defender XDR and Sentinel detection patterns for Python attacks.
The CrashFix ClickFix campaign is spreading a Python RAT via fake Chrome extensions disguised as ad blockers, using PowerShell obfuscation and fileless tricks for persistence. Response details include Defender for Endpoint, Security Copilot, and updated detection queries.
Microsoft is responding to active SolarWinds Web Help Desk exploits (CVE-2025-40551, CVE-2025-40536, CVE-2025-26399), which use DLL tricks and lateral movement. Users are advised to patch, remove exposed endpoints, rotate sensitive accounts, and use KQL queries for incident detection.
- [Infostealer Malware Expands to macOS and Cross-Platform Targets: Defender XDR Insights and Mitigation](https://www.microsoft.com/en-us/security/blog/2026/02/02/infostealers-without-borders-macos-python-stealers-and-platform-abuse/)
- [CrashFix ClickFix Variant Deploys Python RAT via Browser Extension and Living-off-the-Land Tactics](https://www.microsoft.com/en-us/security/blog/2026/02/05/clickfix-variant-crashfix-deploying-python-rat-trojan/)
- [Analysis of Active Exploitation of SolarWinds Web Help Desk: Detection and Mitigation](https://www.microsoft.com/en-us/security/blog/2026/02/06/active-exploitation-solarwinds-web-help-desk/)
## Secure Development at Scale: Automation and Operational Guardrails
Dependabot now offers OIDC authentication for private package registries, so pipelines can request credentials dynamically using cloud provider tokens, reducing risks from static credentials. Azure, AWS, JFrog, and similar platforms are supported, following last week’s coverage of software supply chain safeguards.
Microsoft partners with the FBI’s Operation Winter SHIELD, expanding efforts to enforce technical controls (like secure baselines, legacy auth removal, MFA, and least privilege) using automation instead of just policy. Practical steps help teams apply stronger security in both legacy and new systems.
- [Dependabot Adds OIDC Authentication for Private Registries](https://github.blog/changelog/2026-02-03-dependabot-now-supports-oidc-authentication)
- [Closing the Security Implementation Gap: Microsoft’s Support for Operation Winter SHIELD](https://www.microsoft.com/en-us/security/blog/2026/02/05/the-security-implementation-gap-why-microsoft-is-supporting-operation-winter-shield/)
## AI and Data Security: Risks, Detection, and Lifecycle Evolution
Research from Microsoft looks at ways to detect hidden backdoors in open language models using attention analysis and pattern checking, including LoRA/QLoRA approaches. Teams are advised to combine static scanning with behavioral checks for AI deployment.
Microsoft’s evolving Secure Development Lifecycle adds AI requirements including threat modeling, prompt/poison checks, and better logging and role separation for collaboration and risk tracking.
Microsoft Fabric’s OneLake introduces centralized policy controls for data security, spanning analytic engines and formats for easier cross-cloud control and compliance management.
- [Detecting Backdoors in Open-Weight Language Models: Microsoft Research Insights](https://www.microsoft.com/en-us/security/blog/2026/02/04/detecting-backdoored-language-models-at-scale/)
- [Microsoft SDL: Evolving Security Practices for AI Systems](https://www.microsoft.com/en-us/security/blog/2026/02/03/microsoft-sdl-evolving-security-practices-for-an-ai-powered-world/)
- [The Future of Data Security is Interoperability: A Technical Look at OneLake Security](https://blog.fabric.microsoft.com/en-US/blog/the-future-of-data-security-is-interoperability-a-technical-look-at-onelake-security/)
## Other Security News
CodeQL 2.24.1 improves support for Maven registries, as well as Kotlin, Java, and Python scanning, and adds checks for buffer overflows, locks, and prompt injection risks (especially for Python LLM code). These improvements target clearer reporting and fewer false positives.
A new guide explains how to safely handle and validate user input, including special characters like apostrophes, to prevent SQL injection for .NET, Python, and Bicep, making the advice actionable for both software and infrastructure teams.
- [What’s New in CodeQL 2.24.1: Enhanced Maven Registry Support and Improved Query Accuracy](https://github.blog/changelog/2026-02-06-codeql-2-24-1-improves-maven-private-registry-support-and-improves-query-accuracy)
- [Handling Special Characters in User Input: A Developer’s Guide](https://zure.com/blog/dear-developers-stop-rejecting-me)',
    'This week’s security notes cover changing malware threats, improved automation, new defensive features, and more robust AI/data security. Recent incidents span macOS, Windows, and web threats.',
    1770624000, 'security', '/security/roundups/weekly-security-roundup-2026-02-09', 'TechHub',
    'TechHub', 'D712D3367A859693258579315DCE8F241D0AD0377AF1EDDF87186D08B9F0A220', ',Microsoft Defender XDR,Microsoft Sentinel,Defender For Endpoint,Security Copilot,Macos Security,Windows Security,Infostealer Malware,Python Malware,Browser Extension Malware,PowerShell Obfuscation,SolarWinds Web Help Desk,CVE,KQL,Dependabot,OIDC,Software Supply Chain Security,CodeQL,Prompt Injection,Secure Development Lifecycle,Microsoft Fabric OneLake,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-02-02
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-02-02', 'roundups', 'Weekly Security Roundup: AI Detection, CodeQL, and Access Controls',
    'The latest security updates focus on AI-powered detection, vulnerability scanning, CodeQL language expansion, new data security controls, and enhanced authentication from Microsoft Fabric and Entra ID.
<!--excerpt_end-->
## AI-Assisted Detection Engineering and Supply Chain Protection
AI-powered detection now helps security analysts extract technical indicators from threat reports using Retrieval Augmented Generation and mitigation mapping to MITRE ATT&CK. Best practices stress deterministic prompts, structured output, and validation against golden datasets.
A vulnerability report for LangGrinch (CVE-2025-68664) in LangChain Core outlines the serialization injection risks, remediation, and how to detect and hunt for exploits using KQL and Defender for Cloud.
- [Accelerating Threat Detection Engineering with AI-Assisted TTP Extraction](https://www.microsoft.com/en-us/security/blog/2026/01/29/turning-threat-reports-detection-insights-ai/)
- [Case Study: Securing AI Application Supply Chains](https://www.microsoft.com/en-us/security/blog/2026/01/30/case-study-securing-ai-application-supply-chains/)
## Static Analysis: CodeQL 2.24.0 Expands Language Coverage and Security Capabilities
CodeQL 2.24.0 adds support for .NET 10, C# 14, and Swift 6.2.2/6.2.3. It updates JavaScript/TypeScript, Python, Java/Kotlin, C/C++/Rust, and Axum detection. Security improvements target CSRF in ASP.NET Core, more injection sinks, and better taint tracking. The release also enhances false positive reduction for current frameworks.
This builds on last week''s update to CodeQL 2.23.9, reflecting the ongoing push to secure more languages and frameworks.
- [CodeQL 2.24.0 Adds .NET 10 and Swift 6.2 Support, Enhances Security Analysis](https://github.blog/changelog/2026-01-29-codeql-2-24-0-adds-swift-6-2-support-net-10-compatibility-and-file-handling-for-minified-javascript)
## Fabric Data Security: Outbound Access Protection and Workspace-Level Firewalls
Microsoft Fabric introduces preview features for workspace outbound access protection and workspace-level IP firewalls. These controls let admins restrict outbound network flows and define IP rules per workspace, providing more options for data exfiltration prevention and compliance.
- [Workspace Outbound Access Protection for Data Factory and OneLake Shortcuts (Preview)](https://blog.fabric.microsoft.com/en-US/blog/workspace-outbound-access-protection-for-data-factory/)
- [Introducing Workspace-Level IP Firewall Rules in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/introducing-workspace-level-ip-firewall-rules-in-microsoft-fabric-preview/)
## Identity Security: Passkey-Based Authentication with Entra ID
Entra ID now deploys passkey-based authentication, supporting synced and device-bound credentials for more secure access and easier administration.
- [Automatic Passkey Rollout Update for Microsoft Entra ID](/security/videos/automatic-passkey-rollout-update-for-microsoft-entra-id)
## Other Security News
A tutorial details how to set up two-factor authentication using TOTP codes via Azure Functions and Key Vault, guiding through secure setup, backend, and frontend for cloud-native 2FA.
A recent GitHub Podcast episode covers the Secure Open Source Fund, with maintainers sharing how funding drives security best practices, SBOM adoption, and workflow hardening. AI and Copilot are being used for advanced vulnerability detection in open source.
- [Building a TOTP Authenticator App on Azure Functions and Azure Key Vault](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-a-totp-authenticator-app-on-azure-functions-and-azure/ba-p/4489332)
- [Inside the GitHub Secure Open Source Fund: Leveling Up OSS Security](/github-copilot/videos/inside-the-github-secure-open-source-fund-leveling-up-oss-security)',
    'The latest security updates focus on AI-powered detection, vulnerability scanning, CodeQL language expansion, new data security controls, and enhanced authentication from Microsoft Fabric and Entra ID.',
    1770019200, 'security', '/security/roundups/weekly-security-roundup-2026-02-02', 'TechHub',
    'TechHub', 'AFC0F936367621D56AF0EA57B5186EDFD57E38A905C88D61C4161594C695FD6F', ',AI Security,Threat Detection Engineering,Retrieval Augmented Generation,MITRE ATT&CK,Supply Chain Security,LangChain,CVE 2025 68664,Defender For Cloud,KQL,CodeQL,.NET 10,C# 14,Microsoft Fabric,Microsoft Entra ID,Passkeys,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-01-26
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-01-26', 'roundups', 'Weekly Security Roundup: AI Posture, Supply Chain, and Data Control',
    'Security updates include coverage of risks in developer environments, AI and cloud posture, vulnerability triage, and new controls for workload and supply chain protection. Data governance remains a consistent theme.
<!--excerpt_end-->
## Security in Developer Tooling and Workflows
An analysis covers risks from VS Code’s `tasks.json`, which can contain unsafe commands if included in shared repositories. These shortcuts might inadvertently expose engineers to unsafe code. Developers are encouraged to use isolated dev containers, scrutinize repos for automation files, and request stronger platform-level controls.
- [Abusing VS Code tasks.json: Security Risks from Malicious Repository Configurations](https://devclass.com/2026/01/22/vs-code-tasks-config-file-abused-to-run-malicious-code/)
## Cloud AI Security and Copilot Studio Protections
Microsoft Defender now offers unified management tools for AI risk in cloud environments (including Azure, AWS, GCP), mapping agent privileges and checking for prompt injection. Microsoft Copilot Studio’s runtime protection now includes automated webhook review to identify and stop unintended agent actions. These features combine with last week’s coverage of agent security.
- [Securing AI Agents in the Cloud: Microsoft Defender''s Approach](https://www.microsoft.com/en-us/security/blog/2026/01/21/new-era-of-agents-new-era-of-posture/)
- [Securing Microsoft Copilot Studio AI Agents with Defender Runtime Protection](https://www.microsoft.com/en-us/security/blog/2026/01/23/runtime-risk-realtime-defense-securing-ai-agents/)
## Microsoft Fabric and OneLake: Fine-Grained Security Management
Fabric now offers REST APIs for OneLake that provide automated, path-based access controls. These APIs connect with Entra ID for fine-grained, CI/CD-ready permission management. OneLake security for mirrored databases gives detailed controls—reducing risks associated with excess permission or duplicate data.
- [Granular REST APIs for OneLake Security Management in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/granular-apis-for-onelake-security-preview/)
- [Manage OneLake Security for Mirrored Databases in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/manage-onelake-security-for-mirrored-databases-preview/)
## Threat Intelligence: Phishing Campaigns and BEC Countermeasures
Microsoft investigates new phishing and BEC attacks targeting the energy sector, including using SharePoint for multi-stage attacks that bypass filters and steal sessions. Attackers can add mailbox rules for persistence and erase evidence. The report covers detection, recovery, and analytics alongside remediation strategies, reinforcing best practices for defense.
- [Resurgence of Multi‑Stage AiTM Phishing and BEC Campaign Abusing SharePoint](https://www.microsoft.com/en-us/security/blog/2026/01/21/multistage-aitm-phishing-bec-campaign-abusing-sharepoint/)
## Automating Vulnerability Detection and Management
GitHub Security Lab’s Taskflow Agent uses LLMs and rule books to automate vulnerability triage for Actions and JavaScript, filter out false positives, and connect with GitHub Issues. Modular YAML and prompt/task templates enable extensibility, helping teams systematically review reports and reduce manual work.
- [Automating Security Vulnerability Triage with GitHub Security Lab Taskflow Agent](https://github.blog/security/ai-supported-vulnerability-triage-with-the-github-security-lab-taskflow-agent/)
## Secure Auth and Delegated Access Patterns in Cloud Services
A new guide explains how to use Microsoft Entra’s OAuth2 On-Behalf-Of flow for Python MCP servers (using FastMCP SDK), enabling developers to configure delegated, audited API access. Code samples and setup details help developers integrate secure user flows with existing cloud services.
- [Implementing Microsoft Entra On-Behalf-Of (OBO) Flow in Python MCP Servers with FastMCP](https://techcommunity.microsoft.com/t5/microsoft-developer-community/using-on-behalf-of-flow-for-entra-based-mcp-servers/ba-p/4486760)
## Enhancing Supply Chain Security: Container Image Signing
Microsoft’s Notary Project and Artifact Signing (now GA) tools provide managed certificate handling for CI/CD and AKS—making it simpler to sign images, handle credentials, and use RBAC. The guides support migration from older image signing strategies.
- [Simplifying Image Signing with Notary Project and Artifact Signing (GA)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/simplifying-image-signing-with-notary-project-and-artifact/ba-p/4487942)
## Broader Identity and Data Security Guidance
A framework for 2026 outlines four priorities for identity and network access risk management: adaptive policies, governing users/agents, Zero Trust adoption, and credential controls. Platform integration (Defender, Purview, Verified ID) is central for compliance. Azure Essentials video content shows Purview’s main governance and audit features, complementing ongoing security automation content.
- [4 Priorities for AI-Powered Identity and Network Access Security in 2026](https://www.microsoft.com/en-us/security/blog/2026/01/20/four-priorities-for-ai-powered-identity-and-network-access-security-in-2026/)
- [Understand How Purview Secures and Governs Your Entire Data Estate](/azure/videos/understand-how-purview-secures-and-governs-your-entire-data-estate)
## Other Security News
CodeQL Release 2.23.9 announces the deprecation of support for Kotlin 1.6/1.7 in February 2026. Users of these versions should upgrade. Guidance is available for GitHub Enterprise Server and CLI, echoing last week’s security tooling updates.
- [CodeQL 2.23.9 Release: Deprecation Notice and Update Details](https://github.blog/changelog/2026-01-20-codeql-2-23-9-has-been-released)',
    'Security updates include coverage of risks in developer environments, AI and cloud posture, vulnerability triage, and new controls for workload and supply chain protection. Data governance remains a consistent theme.',
    1769414400, 'security', '/security/roundups/weekly-security-roundup-2026-01-26', 'TechHub',
    'TechHub', '5B2EF9BC6A8B879D5E1C5483CB21A47A883CE69258506522640DDE7586C4F6E2', ',VS Code,Tasks.json,Dev Containers,Microsoft Defender,AI Agents,Prompt Injection,Copilot Studio,Microsoft Fabric,OneLake,Microsoft Entra ID,OAuth 2.0,On Behalf Of Flow,Phishing,Artifact Signing,Notary Project,AKS,GitHub Security Lab,CodeQL,Microsoft Purview,Zero Trust,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-01-19
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-01-19', 'roundups', 'Weekly Security Roundup: AI Threats, Azure Hardening, DevSecOps',
    'This week’s security resources focus on current AI-enabled threats, organizational safeguards, and workflow improvements for developers and IT teams. Articles address criminal infrastructure disruption, Azure platform upgrades, security process automation, CI/CD management, secret handling, code safety, and secured connectivity—all with practical steps for moving to a more resilient defensive posture.
<!--excerpt_end-->
## Criminal VDS Infrastructure and Cybercrime Disruption
Microsoft’s newest threat report describes how the RedVDS service provided cloud-based, anonymous VMs for criminal operations, including email fraud, phishing, and scams. Attackers used tools like Copilot and ChatGPT for local phishing, supported by features like mass mailers and scripting. The report advises administrators to secure M365 tenants, enforce Defender XDR, use multi-factor authentication, and apply DMARC, with linked resources for tracking indicators.
Microsoft and law enforcement have taken down the RedVDS infrastructure and related payment services, disrupting active fraud campaigns. The guidance includes practical steps for detection and ongoing defense.
This update continues last week''s focus on M365 targeting and email security, with Defender and authentication controls top priorities.
- [Inside RedVDS: Investigating How a Criminal VDS Provider Empowered Global Cyberattacks](https://www.microsoft.com/en-us/security/blog/2026/01/14/inside-redvds-how-a-single-virtual-desktop-provider-fueled-worldwide-cybercriminal-operations/)
- [Microsoft Disrupts Global RedVDS Cybercrime-as-a-Service Platform Enabling Massive Fraud](https://blogs.microsoft.com/on-the-issues/2026/01/14/microsoft-disrupts-cybercrime/)
## Secure Default Hostnames and Log Immutability in Azure
Azure Functions and Logic Apps now have Secure Unique Default Hostnames (GA), giving randomized hostnames by region and reducing DNS exposure. Teams should update infrastructure scripts and templates for secure out-of-the-box deployment.
Microsoft Fabric defaults to immutable diagnostic logging for OneLake, using WORM features in Blob Storage to meet audit and regulatory needs. Setup and admin procedures are included, but there are cost tradeoffs and some deletion risk remains; overall, this helps enforce compliance in regulated sectors.
These changes build on previous improvements in governance, moving from sensitivity labels to secure, regulated audit trails.
- [Secure Unique Default Hostnames Now GA for Functions and Logic Apps](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/secure-unique-default-hostnames-now-ga-for-functions-and-logic/ba-p/4484237)
- [Gain Even More Trust and Compliance with OneLake Diagnostics Immutability](https://blog.fabric.microsoft.com/en-US/blog/gain-even-more-trust-and-compliance-with-onelake-diagnostics-immutability-generally-available/)
## Security Automation and Developer Workflows in Open Source
The new open source GitHub Security Lab Taskflow Agent framework launches this week, providing an agent-based toolkit for vulnerability research and code auditing. It integrates with CodeQL, runs modular YAML “taskflows,” and works in Codespaces, Docker, or local Python. Developers can extend or contribute workflows for bug variant analysis and reproducible testing.
The push toward agent-driven automation matches this week’s stories on AI and Copilot, showing real-world solutions for open source security and vulnerability discovery.
- [Community-powered Security with AI: Launching the GitHub Security Lab Taskflow Agent](https://github.blog/security/community-powered-security-with-ai-an-open-source-framework-for-security-research/)
## Secret Scanning and Platform Rule Management for GitHub
From February 2026, GitHub secret scanning will provide extra metadata about detected credentials in eligible repositories, making it easier to address and fix exposures. GitHub’s infrastructure team also reviews platform-wide defense systems, including emergency rate limits and automated rule expiration, for ongoing hygiene and platform safety.
This continues recent updates to GitHub’s security and workflow automation, supporting easier incident response and continuous improvement.
- [GitHub Secret Scanning: Automatic Extended Metadata Checks for Security](https://github.blog/changelog/2026-01-15-secret-scanning-extended-metadata-to-be-automatically-enabled-for-certain-repositories)
- [When Protections Outlive Their Purpose: Managing Defense Systems at Scale on GitHub](https://github.blog/engineering/infrastructure/when-protections-outlive-their-purpose-a-lesson-on-managing-defense-systems-at-scale/)
## AI Coding Agents and Application Security
A technical review finds that AI coding agents sometimes introduce security vulnerabilities such as weak authentication, faulty validation, or dangerous functions. The article recommends thorough code review by developers, regardless of agent use, and a focus on workflow discipline.
This finding connects to the theme of agentic AI providing support, but not replacing, expert oversight in secure development.
- [Vibe Coded Applications Full of Security Blunders](https://devclass.com/2026/01/15/vibe-coded-applications-full-of-security-blunders/)
## Secure Power Platform to Azure PaaS Connectivity with Zero Trust
A new guide walks through setting up zero trust connections from Power Platform to Azure PaaS. It uses VNet injection, firewalls, private endpoints, and peering along with RBAC and managed identity (no secrets). The design includes high availability, user-controlled keys, and automated setup with CLI and PowerShell, with code repositories provided.
This contributes to ongoing best practices for integration and layered defense in Azure environments.
- [Cross-Region Zero Trust: Secure Power Platform Connectivity to Azure PaaS Without Public Exposure](https://techcommunity.microsoft.com/t5/azure-architecture-blog/cross-region-zero-trust-connecting-power-platform-to-azure-paas/ba-p/4484995)
## Code Signing and Property-Level Encryption for Developers
Azure Artifact Signing (AAS) reaches general availability, improving code signing for Windows apps using renewable certificates and simple integration in CI/CD pipelines. The feature isn’t regional or macOS-ready but helps with compliance and key management.
For .NET 8, a walkthrough explains encrypting object properties (like OAuth tokens) on serialization with custom attributes and System.Text.Json’s TypeInfoResolver, with Azure Key Vault integration in development. This level of field-specific data protection supports compliance and privacy rules.
Both topics follow last week’s advances in serialization and privacy-focused developer features.
- [Code Signing Windows Apps Easier and More Secure with Azure Artifact Signing](https://devclass.com/2026/01/14/code-signing-windows-apps-may-be-easier-and-more-secure-with-new-azure-artifact-service/)
- [Encrypting Properties with System.Text.Json and a TypeInfoResolver Modifier (Part 1)](https://www.stevejgordon.co.uk/encrypting-properties-with-system-text-json-and-a-typeinforesolver-modifier-part-1)
## Other Security News
A guide for Microsoft 365 admins covers external sharing controls across SharePoint, OneDrive, Teams, and Entra ID. Topics include settings management, MFA, conditional access, Access Reviews, auditing, and user education for safer collaboration.
These play a role in reducing risk for email and document sharing as discussed previously.
- [Managing External Sharing in Microsoft 365 Without Chaos](https://dellenny.com/managing-external-sharing-in-microsoft-365-without-chaos/)
A session on quantum cryptography covers the risks posed by quantum computing for established encryption methods and explains how to move to quantum-safe protocols, such as SymCrypt. Linked training resources are provided.
This connects to earlier topics covering encryption and the shift toward future-ready standards.
- [What Quantum Safe Is and Why We Need It to Stay Secure](/ai/videos/what-quantum-safe-is-and-why-we-need-it-to-stay-secure)',
    'This week’s security resources focus on current AI-enabled threats, organizational safeguards, and workflow improvements for developers and IT teams. Articles address criminal infrastructure disruption, Azure platform upgrades, security process automation, CI/CD management, secret handling, code safety, and secured connectivity—all with practical steps for moving to a more resilient defensive posture.',
    1768809600, 'security', '/security/roundups/weekly-security-roundup-2026-01-19', 'TechHub',
    'TechHub', 'F3A45649F971C587BD17D30763B45E3BAE5DB8B5FF9357E38DCB13E7A04ECF49', ',Microsoft Security,Threat Intelligence,Phishing,Microsoft 365,Defender XDR,Multi Factor Authentication,DMARC,Azure Functions,Azure Logic Apps,Microsoft Fabric,Immutable Logging,WORM Storage,GitHub Secret Scanning,CodeQL,CI/CD Security,Azure Artifact Signing,Zero Trust,Private Endpoints,Managed Identity,Azure Key Vault,.NET 8,System.Text.Json,Quantum Safe Cryptography,SymCrypt,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-01-12
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-01-12', 'roundups', 'Weekly Security Roundup: Microsoft 365 Phishing and Fabric Governance',
    'Security updates feature ongoing phishing threat analysis for Microsoft 365 and new centralized data governance features in Microsoft Fabric.
<!--excerpt_end-->
## Exploiting Email Routing and Authentication Gaps in Microsoft 365
Microsoft’s threat research details how attackers use mail routing gaps and weak authentication (SPF, DKIM, DMARC) to launch phishing. Methods include spoofing, manipulation of names and sender data, and PhaaS kits like Tycoon2FA targeting defenses.
Recommendations build on recent discussions of layered security. Enforcing strict authentication, reviewing mail flow, and leveraging Defender features are reinforced, together with stronger multi-factor authentication using Entra ID.
For security teams, the report offers practical Kusto queries, guidance for Defender XDR and Sentinel, steps for credential and rule resets, and links to automation resources.
- [Spoofed Phishing Emails Exploiting Routing and Protection Misconfigurations](https://www.microsoft.com/en-us/security/blog/2026/01/06/phishing-actors-exploit-complex-routing-and-misconfigurations-to-spoof-domains/)
## Centralized Security Governance in Microsoft Fabric
Microsoft Fabric centralizes security reporting in OneLake Catalog’s Govern tab, now covering sensitivity labeling across Lakehouses, Warehouses, and Reports, and identifying potentially unprotected data. Admins see compliance status and scan history for prompt remediation.
Copilot aids investigation, supporting more effective response to policy violations. Teams are advised to transition from Purview Hub reporting, as Microsoft plans to retire the old reporting system in 2026.
- [Enhanced Security Governance in Microsoft Fabric: Admin Report Now in OneLake Catalog Govern Tab](https://blog.fabric.microsoft.com/en-US/blog/explore-your-fabric-security-insights-in-the-onelake-catalog-govern-tab/)',
    'Security updates feature ongoing phishing threat analysis for Microsoft 365 and new centralized data governance features in Microsoft Fabric.',
    1768204800, 'security', '/security/roundups/weekly-security-roundup-2026-01-12', 'TechHub',
    'TechHub', '6543ED770B7CC7432B4047A5A67DBE5AAB696F52B3F8CA2F07384BD651B0194A', ',Microsoft 365,Phishing,Email Security,SPF,DKIM,DMARC,Microsoft Defender XDR,Microsoft Sentinel,Kusto Query Language,Microsoft Entra ID,Multi Factor Authentication,Microsoft Fabric,OneLake,Sensitivity Labels,Microsoft Purview,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2026-01-05
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2026-01-05', 'roundups', 'Weekly Security Roundup: Fuzzing Gaps and Entra ID Recovery',
    'This week’s security updates include a research review on continuous fuzzing in open source and new features in Microsoft Entra ID for more resilient cloud identity management and user recovery.
<!--excerpt_end-->
## Fuzzing and Vulnerability Discovery in Open Source Projects
GitHub Security Lab recently published findings on why some security bugs persist in open source projects even after extended fuzzing with OSS-Fuzz. Case studies point out that incomplete coverage—especially around encoding logic and external dependencies—lets vulnerabilities survive. Issues sometimes escape detection because fuzzers are not running long enough or do not generate large enough inputs. The article explains advanced options like AFL++ branch coverage, N-Gram, value-based fuzzing, and the addition of manual reviews or static analysis for better detection. It lays out five steps for closing test coverage gaps and suggests using Fuzzing 101, workflow reviews, and layered assurance.
These recommendations add to last week’s supply chain security topics, arguing that effective security testing requires a mix of automation and human review. Open source maintainers are reminded that complex bugs sometimes need multiple validation steps to be found and fixed, reinforcing the ongoing emphasis on persistent, multifaceted vulnerability discovery.
- [Why Bugs Survive Continuous Fuzzing: Lessons from OSS-Fuzz Research](https://github.blog/security/vulnerability-research/bugs-that-survive-the-heat-of-continuous-fuzzing/)
## Improvements to Account Recovery in Microsoft Entra ID
Microsoft Entra ID (formerly Azure Active Directory) now offers expanded options for user account recovery. In addition to password reset and SMS, blocked users can restore access by submitting a government ID for third-party verification (through services like AU10TIX). This approach is designed to resist phishing and credential theft, lessening social engineering risk and SIM swap attacks. Setup is done through a portal integration and Azure API configuration, giving IT administrators a way to enable or restrict the feature.
Privacy, regulatory policies, and provider stability are addressed, and step-by-step instructions with demo videos are available for deployment. Improved account recovery helps reduce support tickets and lets users regain access without as much IT involvement—a boost for cloud identity stability.
This update fits with previous work to raise the bar for identity security, following MFA, strong authentication, and trusted publishing as mentioned previously.
- [Account Recovery in Microsoft Entra ID Using Government IDs and Third-Party Identity Verification](/azure/videos/account-recovery-in-microsoft-entra-id-using-government-ids-and-third-party-identity-verification)',
    'This week’s security updates include a research review on continuous fuzzing in open source and new features in Microsoft Entra ID for more resilient cloud identity management and user recovery.',
    1767600000, 'security', '/security/roundups/weekly-security-roundup-2026-01-05', 'TechHub',
    'TechHub', '8743BBF20454B21A94CAC8E52EE3C044036508332B8295A598C5C1B72FD3652F', ',Application Security,Open Source Security,Fuzzing,OSS Fuzz,AFL++,Static Analysis,Code Coverage,Vulnerability Research,Secure Software Development,Microsoft Entra ID,Azure Active Directory,Identity And Access Management,Account Recovery,Phishing Resistance,SIM Swap Mitigation,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-12-29
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-12-29', 'roundups', 'Weekly Security Roundup: Supply Chain Hardening After Shai-Hulud',
    'Security releases this week detail steps to improve defenses in software supply chains, especially in light of recent attacks like Shai-Hulud. These updates provide clear strategies for developers and maintainers to respond to active threats and manage secure publishing workflows.
<!--excerpt_end-->
## Supply Chain Security in Open Source Development
The Shai-Hulud attack exposed gaps in supply chain protection by targeting developer credentials and publishing processes. In response, npm is rolling out improvements such as bulk OIDC onboarding, support for additional providers, and phased release controls using MFA for sign-off.
This week’s advice stresses activating phishing-resistant MFA on both GitHub and npm, reviewing short-lived tokens, checking OAuth app permissions, and using sandboxed environments for publishing. These steps are in line with previously recommended practices for identity and secret management, and they reinforce the need for robust, repeatable controls.
Recommended strategies include using trusted publishing, pinning and scanning dependencies, and validating releases with automated and manual checks—ensuring that proactive governance stays in place for both code and packages.
Incident response continues to combine automated monitoring tools (such as Defender and Sentinel) with careful team-led investigation, highlighting the ongoing need for continuous improvement in open source supply chain practices.
- [Strengthening Supply Chain Security for Developers and Maintainers](https://github.blog/security/supply-chain-security/strengthening-supply-chain-security-preparing-for-the-next-malware-campaign/)',
    'Security releases this week detail steps to improve defenses in software supply chains, especially in light of recent attacks like Shai-Hulud. These updates provide clear strategies for developers and maintainers to respond to active threats and manage secure publishing workflows.',
    1766995200, 'security', '/security/roundups/weekly-security-roundup-2025-12-29', 'TechHub',
    'TechHub', 'D5DCECD583D87FF3F2796F336E8E923959A88E3C11D5566BF674DA177DDDFF5C', ',Software Supply Chain Security,Open Source Security,npm,GitHub,OIDC,Trusted Publishing,MFA,Phishing Resistant MFA,OAuth,Developer Credentials,Token Security,Dependency Pinning,Dependency Scanning,Release Governance,Incident Response,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-12-22
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-12-22', 'roundups', 'Weekly Security Roundup: React2Shell Mitigations and GitHub Controls',
    'React2Shell (CVE-2025-55182) affects Next.js/Node.js workloads—Microsoft provides Defender and Azure WAF mitigation guidance. GitHub expands Dependabot with uv support and requires peer review for alert dismissals.
<!--excerpt_end-->
## React2Shell Vulnerability Response Across Microsoft Defender and Azure
React2Shell (CVE-2025-55182) affects Next.js and Node.js workloads, with attackers exploiting React Server Component build pipelines. Recommendations include updating to secured frameworks, scanning assets with Microsoft Defender, setting up custom Azure WAF rules, and using Sentinel or Security Copilot for further analysis. Teams should establish a combination of automated and manual incident handling.
- [Mitigating CVE-2025-55182 (React2Shell) with Microsoft Defender for Endpoint and Azure WAF](https://www.microsoft.com/en-us/security/blog/2025/12/15/defending-against-the-cve-2025-55182-react2shell-vulnerability-in-react-server-components/)
## GitHub Security Ecosystem Updates: Dependabot uv Support, Code Scanning, Secret Management
Dependabot now supports uv packages, improving automated vulnerability tracking. Code scanning alert assignment via REST API is now generally available. CodeQL improvements boost detection for Go and Rust. Secret scanning governance has been expanded, and dismissing Dependabot alerts now requires a peer review. More organizations can access Advanced Security trials with the latest expansion.
- [Dependabot Security Updates Now Support uv](https://github.blog/changelog/2025-12-16-dependabot-security-updates-now-support-uv)
- [General Availability of Code Scanning Alert Assignees in GitHub](https://github.blog/changelog/2025-12-16-code-scanning-alert-assignees-are-now-generally-available)
- [CodeQL 2.23.7 and 2.23.8 Released: Enhanced Security Queries for Go and Rust](https://github.blog/changelog/2025-12-18-codeql-2-23-7-and-2-23-8-add-security-queries-for-go-and-rust)
- [Enterprise Governance and Policy Improvements for GitHub Secret Scanning](https://github.blog/changelog/2025-12-16-enterprise-governance-and-policy-improvements-for-secret-scanning-now-generally-available)
- [GitHub Advanced Security Trials Expanded for Enterprise Customers](https://github.blog/changelog/2025-12-18-github-advanced-security-trials-now-available-for-more-github-enterprise-customers)
- [Require Reviews for Dependabot Alert Dismissal with Delegated Alert Dismissal in GitHub](https://github.blog/changelog/2025-12-19-you-can-now-require-reviews-before-closing-dependabot-alerts-with-delegated-alert-dismissal)
## Evolving Cloud and Identity Security: TLS, Managed Identities, and Access Fabric Strategies
Azure App Service users should prepare for upcoming TLS certificate and authentication changes. Managed Identities for Azure Files SMB allow password-free access for automated agents, AKS nodes, and cloud applications. Microsoft’s Access Fabric moves device and network checks directly into access enforcement, supporting Zero Trust principles.
- [Preparing for Industry-wide TLS Certificate Changes in Azure App Service (2026 Update)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/industry-wide-certificate-changes-impacting-azure-app-service/ba-p/4477924)
- [Secure Access with Managed Identities for Azure Files SMB](https://techcommunity.microsoft.com/t5/azure-storage-blog/secure-seamless-access-using-managed-identities-with-azure-files/ba-p/4477565)
- [Access Fabric: Modernizing Identity and Network Access Security with Microsoft Entra](https://www.microsoft.com/en-us/security/blog/2025/12/17/access-fabric-a-modern-approach-to-identity-and-network-access/)
## Other Security News
A Microsoft e-book explains the benefits of unified, AI-capable security platforms (Defender, Sentinel, Copilot) for incident management. Also available is a practical guide for configuring Sensitivity Labels in Microsoft Teams, employing Purview Information Protection for automated policies, encryption, and compliance.
- [Why Unified, AI-Ready Security Platforms Outperform Patchwork Solutions](https://www.microsoft.com/en-us/security/blog/2025/12/18/new-microsoft-e-book-3-reasons-point-solutions-are-holding-you-back/)
- [How To Use Sensitivity Labels in Microsoft Teams (Step-by-Step Guide)](https://dellenny.com/how-to-use-sensitivity-labels-in-microsoft-teams-step-by-step-guide/)',
    'React2Shell (CVE-2025-55182) affects Next.js/Node.js workloads—Microsoft provides Defender and Azure WAF mitigation guidance. GitHub expands Dependabot with uv support and requires peer review for alert dismissals.',
    1766390400, 'security', '/security/roundups/weekly-security-roundup-2025-12-22', 'TechHub',
    'TechHub', 'E3168AE74820DE02F1B09404C5847A7B5B8446FD5FCCA967D090DE5270EC807F', ',CVE 2025 55182,React2Shell,Next.js,Node.js,Microsoft Defender For Endpoint,Azure WAF,Microsoft Sentinel,Security Copilot,GitHub Dependabot,Uv,GitHub Code Scanning,CodeQL,Secret Scanning,Managed Identities,Zero Trust,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-12-15
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-12-15', 'roundups', 'Weekly Security Roundup: Tokens, Supply Chains, and Access Control',
    'Updates in security span new authentication methods, improvements in supply chain risk management, endpoint security guidance, and permission management strategies. Development platforms now require updated credentials while guides support teams in securing infrastructure.
<!--excerpt_end-->
## npm Authentication, Supply Chain, and IDE Attack Surface
The npm registry now mandates session-based or new CLI tokens, replacing classic tokens for authentication. Two-factor authentication is required for publishing, with short-lived (two-hour) tokens for sessions. The CLI assists with management tasks such as creating, listing, and revoking tokens. OIDC-based publishing is the new recommendation, and developers must revise their workflows to be compliant.
A technical breakdown describes how the Shai-Hulud 2.0 attack targeted npm packages with scripting tools, advising on detection and defense using Defender for Cloud and Sentinel. The persistence of endpoint supply chain threats underscores the need for vigilance.
This week’s security alert reports the discovery of malicious VS Code extensions installing infostealer malware, highlighting risks such as DLL hijacking and reinforcing the importance of plugin monitoring and least-privilege approaches in CI/CD workflows.
- [npm Classic Tokens Revoked: Session-Based Authentication and CLI Token Management Now Available](https://github.blog/changelog/2025-12-09-npm-classic-tokens-revoked-session-based-auth-and-cli-token-management-now-available)
- [Shai-Hulud 2.0: Guidance for Detecting, Investigating, and Defending Against the Supply Chain Attack](https://www.microsoft.com/en-us/security/blog/2025/12/09/shai-hulud-2-0-guidance-for-detecting-investigating-and-defending-against-the-supply-chain-attack/)
- [Security Risks of Malicious VS Code Extensions Targeting Developers](https://devops.com/malicious-vs-code-extensions-take-screenshots-steal-info/)
## Azure and Microsoft Stack Authentication & Access Management
Azure DevOps has posted retirement deadlines for global personal access tokens; after March 2026, new tokens will not be issued, and all tokens will be invalidated by December 2026. Teams are urged to migrate to Entra-backed tokens to support least-privilege and avoid credential sprawl.
A recent analysis points out security concerns with Custom Script Extensions on Azure Virtual Desktop, where blob tokens may appear in logs. Solutions include Key Vault or Managed Identities, although some limitations are noted for portal-based operations.
Fabric’s OneLake now allows for more detailed ReadWrite permissions, making schema-level access management easier for compliance and data governance.
- [Azure DevOps Retires Global Personal Access Tokens: Key Dates and Security Impact](https://devblogs.microsoft.com/devops/retirement-of-global-personal-access-tokens-in-azure-devops/)
- [Securing Custom Script Extensions in Azure Session Host Configurations](https://techcommunity.microsoft.com/t5/azure-virtual-desktop-feedback/more-security-around-using-custom-script-extensions-and-session/idi-p/4476426)
- [Optimizing Permissions with OneLake Security ReadWrite Access](https://blog.fabric.microsoft.com/en-US/blog/31267/)
## Cloud Incident Response, Endpoint Security, and Email Protection
New security incidents show attackers using fake employee identities and KVM tools to gain endpoint access. Microsoft’s investigation relied on Defender for Endpoint, Entra ID, and improved monitoring of onboarding, auditing, and data loss prevention for handling insider risks.
Benchmark studies provided comparisons of email security solutions (Defender for Office 365, SEG, ICES) and highlighted the real-world effectiveness of different products. These rates help leaders shape incident response and technical defenses.
- [Imposter for Hire: How Fake Employees Breach Security](https://www.microsoft.com/en-us/security/blog/2025/12/11/imposter-for-hire-how-fake-people-can-gain-very-real-access/)
- [Transparent Benchmarking and Layered Email Security with Microsoft Defender](https://www.microsoft.com/en-us/security/blog/2025/12/10/clarity-in-complexity-new-insights-for-transparent-email-security/)
## Securing AI, Advanced Defense Strategies, and Practical Security Modeling
A security advisory this week covers how to securely operate AI agents with Azure SQL, focusing on permission management, error tracking, and monitoring for regulatory compliance.
Microsoft’s Security CTO advances the use of graph-based models—mapping identity, credentials, and assets—to enhance security operation centers, combined with AI analytics. Detailed inventory and KQL-based log review are central to this practice.
A new video on security modeling highlights how storytelling improves understanding and training within teams, encouraging practical linkage between incidents and security improvements.
- [Securely Unleashing AI Agents on Azure SQL and SQL Server](/ai/videos/securely-unleashing-ai-agents-on-azure-sql-and-sql-server)
- [Changing the Physics of Cyber Defense: Graph-Based Strategies and AI with Microsoft Security](https://www.microsoft.com/en-us/security/blog/2025/12/09/changing-the-physics-of-cyber-defense/)
- [The Role of Storytelling in Security Modeling](/security/videos/the-role-of-storytelling-in-security-modeling)
## Other Security News
A practical guide walks through setting up Remote Desktop on Windows 11, covering basic access, firewall, VPN setup, and how to secure connections with Network Level Authentication. Troubleshooting advice is provided for typical real-world issues faced by administrators.
- [How to Set Up Remote Desktop on Windows 11: Step-by-Step Guide](https://dellenny.com/how-to-set-up-remote-desktop-on-windows-11-a-beginners-guide/)',
    'Updates in security span new authentication methods, improvements in supply chain risk management, endpoint security guidance, and permission management strategies. Development platforms now require updated credentials while guides support teams in securing infrastructure.',
    1765785600, 'security', '/security/roundups/weekly-security-roundup-2025-12-15', 'TechHub',
    'TechHub', '8F1B7F8D78A408ECDDA2F68F8E5A954BF1EEFC5DB62802E7704F3CE1BAB76E22', ',npm,Authentication,CLI Tokens,OIDC,Two Factor Authentication,Software Supply Chain Security,VS Code Extensions,Malware,Azure DevOps,Microsoft Entra ID,Least Privilege,Microsoft Defender,Microsoft Sentinel,Incident Response,Permissions Management,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-12-08
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-12-08', 'roundups', 'Weekly Security Roundup: Secret Scanning, Passkeys, and Mobile Signing',
    'Updates cover enhanced secret scanning, secure authentication strategies, and cybersecurity best practices.
<!--excerpt_end-->
## GitHub Secret Scanning and Automated Analysis Enhancements
GitHub expanded secret scanning with new detection patterns for Azure, Databricks, Discord, and other platforms. Additions include EC and PKCS#8 key support, and notifications for gists. Discord bot token alerts and AWS key validation increase metadata for better incident response. CodeQL 2.23.6 updates add Swift, Rust, and C# detection, as well as queries for insecure cookies.
- [GitHub Secret Scanning Updates and New Patterns — November 2025](https://github.blog/changelog/2025-12-02-secret-scanning-updates-november-2025)
- [CodeQL 2.23.6 Update: New C# Security Queries and Language Enhancements](https://github.blog/changelog/2025-12-04-codeql-2-23-6-adds-swift-6-2-1-and-new-c-security-queries)
## Secure Authentication Strategies in Cloud and Mobile
Guides now cover device-bound passkeys in Microsoft Entra ID for policy-driven identity and strong authentication. Device-Bound Request Signing (DBRS) for mobile apps is outlined, with recommendations for crypto, security modeling, and cross-platform deployments.
- [Entra Synced Passkeys and Passkey Profiles](/azure/videos/entra-synced-passkeys-and-passkey-profiles)
- [Securing Sensitive Mobile Operations with Device-Bound Request Signing](https://devblogs.microsoft.com/blog/securing-sensitive-mobile-operations-with-device-bound-request-signing)
## Other Security News
Enterprise cybersecurity priorities for 2025 include asset discovery, network segmentation, endpoint hardening, phishing-resistant MFA, and comprehensive use of Microsoft identity and DNS/SMTP protections. Guidance also covers layered defense and cooperative response readiness.
- [Cybersecurity Strategies to Prioritize Now](https://www.microsoft.com/en-us/security/blog/2025/12/04/cybersecurity-strategies-to-prioritize-now/)',
    'Updates cover enhanced secret scanning, secure authentication strategies, and cybersecurity best practices.',
    1765180800, 'security', '/security/roundups/weekly-security-roundup-2025-12-08', 'TechHub',
    'TechHub', '23E8D8E3EE47433C15CBD697937C7150DBC4724DE97BF50CB0CC2B8797BCD78F', ',GitHub Secret Scanning,Secrets Detection,GitHub Gists,Azure,Databricks,Discord,AWS Key Validation,CodeQL,Static Analysis,Swift,Rust,C#,Microsoft Entra ID,Passkeys,Device Bound Request Signing,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-12-01
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-12-01', 'roundups', 'Weekly Security Roundup: Agentic AI, SOC Automation, Memory Safety',
    'Security news this week features new AI-powered protections for cloud, endpoints, and collaboration, with a focus on governance and operational agility. Ignite 2025 delivered sessions and resources on secure agentic AI, confidential computing, automated SOC, and broad partnerships. Updates in memory-safe platform hardware, managed agent lifecycle, and practical incident response are supported by more tools for hardening devices, data, and drivers.
<!--excerpt_end-->
## Advancements in Confidential Computing and Memory-Safe Platform Security
Extending last week’s progress in custom baselines and memory safety, Azure has now implemented deeper Intel Trusted Domain Extensions (TDX) in collaboration with Bosch and Intel, providing stronger isolation for high-assurance workloads.
Microsoft continues to move system code to Rust for firmware and drivers, with new ecosystem support through windows-drivers-rs and Cargo WDK. Secure Core PC and enhanced DFCI control also get newly updated deployment tools for IT.
- [Advancing Confidential Computing: Bosch, Microsoft Azure, & Intel TDX](/ai/videos/advancing-confidential-computing-bosch-microsoft-azure-and-intel-tdx)
- [Advancing Windows Device Security with Surface Innovation and Memory-Safe Rust Drivers](/coding/videos/advancing-windows-device-security-with-surface-innovation-and-memory-safe-rust-drivers)
## Securing Agentic AI: Lifecycle, Governance, and Risk Management
This week’s updates highlight new tools for threat modeling and governance in agentic AI, directly addressing risks like prompt injection and memory tampering previously discussed.
Microsoft Agent 365 builds on centralized audit trails, conditional access, and DLP, as well as enhancements in Defender, Purview, and Entra for fine-grained monitoring of AI workflows, continuing the push for clear oversight and risk controls.
- [Building Secure AI Agents with Microsoft’s Security Stack](/ai/videos/building-secure-ai-agents-with-microsofts-security-stack)
- [Explore Microsoft Agent 365 Security and Governance Capabilities](/ai/videos/explore-microsoft-agent-365-security-and-governance-capabilities)
- [Securing AI at Scale: Microsoft’s Latest Innovations in Agent, App, and Data Protection](/ai/videos/securing-ai-at-scale-microsofts-latest-innovations-in-agent-app-and-data-protection)
- [Leading with Trust: Building & Deploying Agents in a Regulated World](/ai/videos/leading-with-trust-building-and-deploying-agents-in-a-regulated-world)
## Security Copilot, SOC Automation, and Microsoft Defender Ecosystem
Security Copilot introduces agent-based automation for SOC teams, including persistent threat memory and daily briefings. The new Security Compute Unit provides a clearer cost and access model to support these changes.
Microsoft Sentinel’s updates on analytics and cross-system coverage assist with centralizing monitoring efforts. Defender for Cloud continues progress with AI-driven attack detection, expanding on previous themes of dashboard integration and proactive protection.
- [Security Copilot: Empowering Security Teams with AI at Microsoft Ignite 2025](/ai/videos/security-copilot-empowering-security-teams-with-ai-at-microsoft-ignite-2025)
- [Empowering the SOC: Security Copilot and the Rise of Agentic Defense](/ai/videos/empowering-the-soc-security-copilot-and-the-rise-of-agentic-defense)
- [Amplifying SecOps Practices with Microsoft Sentinel and Unified Platform](/ai/videos/amplifying-secops-practices-with-microsoft-sentinel-and-unified-platform)
- [Build Secure Applications with Defender and Azure Network Security](/azure/videos/build-secure-applications-with-defender-and-azure-network-security)
- [AI-powered Defense Strategies for Cloud Workloads with Microsoft Defender](/ai/videos/ai-powered-defense-strategies-for-cloud-workloads-with-microsoft-defender)
## Microsoft Purview and Enterprise Data Security
Expanded Microsoft Purview features continue to focus on data security and compliance, supporting organizations as they incorporate Copilot and generative AI into operations. Features like DSPM, automated labeling, and alert fatigue reduction are included, showing how AI can reduce manual effort and speed up compliance work.
Case studies reinforce that automation tools and adaptive policy management are delivering measurable gains, moving from recent pilot phases into everyday use.
- [Securing Data Across Microsoft Environments with Microsoft Purview](/azure/videos/securing-data-across-microsoft-environments-with-microsoft-purview)
- [Secure-by-Design Transformation: PwC and Microsoft Purview Enhancing Data Security](/security/videos/secure-by-design-transformation-pwc-and-microsoft-purview-enhancing-data-security)
- [AI-Powered Data Security with Security Copilot and Microsoft Purview](/ai/videos/ai-powered-data-security-with-security-copilot-and-microsoft-purview)
## Identity, Zero Trust, and Cross-Platform Security
Microsoft Entra and Intune lead ongoing Zero Trust efforts by adding adaptive access and security policies shaped by AI, echoing advances discussed in authentication and device management. The new Intune capabilities bolster risk identification and support secure AI adoption across infrastructures.
- [Accelerating Zero Trust and Securing AI Access with Microsoft Entra Suite](/ai/videos/accelerating-zero-trust-and-securing-ai-access-with-microsoft-entra-suite)
- [Demystifying Zero Trust Endpoint Management with Microsoft Intune](/security/videos/demystifying-zero-trust-endpoint-management-with-microsoft-intune)
## Integrated SOC Visibility, Threat Intelligence, and Third-Party Security Partnerships
Strategic partnerships with solutions like Lumen Defender and Cisco on Azure add new joint telemetry and SOC visibility, enriching detection and operational awareness as seen in past security updates.
- [Lumen Defender and Microsoft Security: Enhancing SOC Threat Detection and Response](/security/videos/lumen-defender-and-microsoft-security-enhancing-soc-threat-detection-and-response)
- [Unified Digital Resilience: Integrating Cisco and Microsoft Security on Azure](/ai/videos/unified-digital-resilience-integrating-cisco-and-microsoft-security-on-azure)
## Other Security News
Expanded managed security services such as Defender Experts for XDR and incident response teams build on last week’s detailed coverage. Updates promote best practices for threat detection, patch management, and resilient operations.
Updates for GitHub’s DevSecOps automation cover essentials like policy administration and package validation, supporting Copilot and agent workflows at scale.
Cloud security features for telco and wireless environments follow the established direction of enhanced authentication and orchestration. Commvault SHIFT now brings additional AI-powered data resilience and Zero Trust integration for Microsoft platforms.
- [Microsoft Security Experts: Enhancing Your SOC with Managed XDR and Incident Response](/security/videos/microsoft-security-experts-enhancing-your-soc-with-managed-xdr-and-incident-response)
- [Enterprise Security and Governance on GitHub: Best Practices from Ignite 2025](/devops/videos/enterprise-security-and-governance-on-github-best-practices-from-ignite-2025)
- [Securing Private Wireless: From Design to Deployment](/ai/videos/securing-private-wireless-from-design-to-deployment)
- [Commvault SHIFT Virtual: AI and Cyber Resilience Insights for Microsoft Identity and Cloud](https://www.thomasmaurer.ch/2025/11/commvault-shift-virtual-a-new-era-of-ai-driven-cyber-resilience-on-demand/)',
    'Security news this week features new AI-powered protections for cloud, endpoints, and collaboration, with a focus on governance and operational agility. Ignite 2025 delivered sessions and resources on secure agentic AI, confidential computing, automated SOC, and broad partnerships. Updates in memory-safe platform hardware, managed agent lifecycle, and practical incident response are supported by more tools for hardening devices, data, and drivers.',
    1764576000, 'security', '/security/roundups/weekly-security-roundup-2025-12-01', 'TechHub',
    'TechHub', 'CB83DA38E3D2DFF2B5CF92DF8D63CBFB03DF80D0038950C5E9F6C19F0F6B4397', ',Microsoft Security,Security Copilot,Microsoft Sentinel,Microsoft Defender,Defender For Cloud,Microsoft Purview,Microsoft Entra,Microsoft Intune,Zero Trust,Confidential Computing,Intel TDX,Rust,DFCI,XDR,DevSecOps,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-11-24
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-11-24', 'roundups', 'Weekly Security Roundup: Agent Governance, Zero Trust, and PQC',
    'Security updates cover expanded AI integration, automation, zero-trust principles, new security features in Azure, .NET, and Microsoft 365, and more detailed data and agent governance. These changes reflect an ongoing shift toward explainable, automated, and unified security practices.
<!--excerpt_end-->
## Azure Platform Security: New Foundations and Granular Controls
Azure now offers the MetaData Security Protocol (MSP) for VMs, with support for HMAC validation and eBPF Guest Proxy Agent. These bring controls for zero-trust and explicit allowlisting into general availability, supporting compliance.
- [Metadata Security Protocol (MSP) General Availability Secures Azure VM Metadata](https://techcommunity.microsoft.com/t5/azure-compute-blog/introducing-metadata-security-protocol-msp-elevating-platform/ba-p/4471204)
Azure Monitor Logs provides GA support for detailed RBAC at multiple levels, advancing least privilege for telemetry data.
- [Granular RBAC Now Generally Available in Azure Monitor Logs](https://techcommunity.microsoft.com/t5/azure-observability-blog/general-availability-granular-rbac-in-azure-monitor-logs/ba-p/4471299)
Azure DNS Security Policy, now generally available, links threat intelligence with DNS filtering and integrates with DevOps workflows.
- [Azure DNS Security Policy with Threat Intelligence Feed Now Generally Available](https://techcommunity.microsoft.com/t5/azure-networking-blog/announcing-azure-dns-security-policy-with-threat-intelligence/ba-p/4470183)
Microsoft also detailed its defense against a recent 15 Tbps DDoS attack, highlighting current adaptive, automated protections.
- [Defending the cloud: Azure neutralized a record-breaking 15 Tbps DDoS attack](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/defending-the-cloud-azure-neutralized-a-record-breaking-15-tbps/ba-p/4470422)
## Building Security for AI-Driven Workloads and Agents
Microsoft Entra now manages “Agent ID” for non-human actors, supporting identity lifecycle management and mitigation for issues like prompt injection.
- [Microsoft Entra: What''s New in Secure Access on the AI Frontier](/ai/videos/microsoft-entra-whats-new-in-secure-access-on-the-ai-frontier)
Best practices for securing AI agents with Microsoft Defender and in Microsoft Foundry add practical strategies for real-world risk management.
- [Secure Your AI Agents with Microsoft Defender: Best Practices from Ignite 2025](/ai/videos/secure-your-ai-agents-with-microsoft-defender-best-practices-from-ignite-2025)
- [Securing AI Agents in Microsoft Foundry with Microsoft Security](/ai/videos/securing-ai-agents-in-microsoft-foundry-with-microsoft-security)
Oasis introduces more comprehensive credential management for non-person entities in the Microsoft environment.
- [Power Agentic Access: Governing Non-Human Identities with Oasis | Microsoft Ignite 2025](/ai/videos/power-agentic-access-governing-non-human-identities-with-oasis-microsoft-ignite-2025)
Zenity''s integration provides runtime monitoring and incident response support for agent workflows in Copilot, Studio, and Foundry.
- [Securing the AI Agents with Zenity and Microsoft](/ai/videos/securing-the-ai-agents-with-zenity-and-microsoft)
## Microsoft Defender for Cloud and End-to-End Application Security
Defender for Cloud expands support for risk management and AI-powered threat detection, including pipelines, with integration for live risk assessments and artifact tracking. Defender’s connection with GitHub Advanced Security aids in automating secure development practices.
- [Defending Cloud Platforms: Unified Security with Microsoft Defender](/ai/videos/defending-cloud-platforms-unified-security-with-microsoft-defender)
- [Unified Application Security with Microsoft Defender for Cloud](/ai/videos/unified-application-security-with-microsoft-defender-for-cloud)
- [Runtime Security and AI Fixes: Integrating GitHub Advanced Security with Defender for Cloud](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/security-where-it-matters-runtime-context-and-ai-fixes-now/ba-p/4470794)
- [Unified Code-to-Cloud Artifact Risk Visibility with Microsoft Defender for Cloud in GitHub](https://github.blog/changelog/2025-11-18-unified-code-to-cloud-artifact-risk-visibility-with-microsoft-defender-for-cloud-now-in-public-preview)
Security Copilot’s expanded role now includes Microsoft 365 E5, offering SIEM and XDR coverage plus automated PR remediation with Copilot Autofix. New artifact tracking and shielding cover legacy environments as well.
- [AI-Driven Security Agents Now in Microsoft 365 E5: Security Copilot Integration and Expansion](https://www.microsoft.com/en-us/security/blog/2025/11/18/agents-built-into-your-workflow-get-security-copilot-with-microsoft-365-e5/)
- [Security Copilot: Automating and Accelerating Defense with Agentic Workflows](/ai/videos/security-copilot-automating-and-accelerating-defense-with-agentic-workflows)
- [AI-Powered Endpoint Security Updates in Microsoft Defender](/ai/videos/ai-powered-endpoint-security-updates-in-microsoft-defender)
## Comprehensive Governance for Data, Secrets, and Identity
Secrets management and identity rotation benefit from new technical guides for secure Azure Authentication and OIDC, bringing programmatic security best practices into DevOps pipelines.
- [Secure Secrets, Certificate, and Access Management for Azure](/azure/videos/secure-secrets-certificate-and-access-management-for-azure)
Microsoft Fabric has introduced finer-grained data permissions, offering write access at the folder and table levels, as well as assignment capabilities in the UI.
- [Fine-grained ReadWrite Access to Data with OneLake Security (Preview)](https://blog.fabric.microsoft.com/en-US/blog/fine-grained-readwrite-access-to-lakehouse-data-with-onelake-security/)
SQL auditing and encryption improvements offer better compliance management for regulated workloads.
- [Auditing Features for Fabric SQL Database (Preview)](https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-auditing-for-fabric-sql-database/)
- [Using Customer-Managed Keys with Microsoft Fabric SQL Database](https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-customer-managed-keys-in-fabric-sql-database/)
## Post-Quantum Cryptography Advances and Secure Coding
.NET now supports additional post-quantum cryptography algorithms (ML-KEM, ML-DSA), helping organizations prepare for new cryptographic requirements.
- [Post-Quantum Cryptography in .NET: New Algorithms and Design Principles](https://devblogs.microsoft.com/dotnet/post-quantum-cryptography-in-dotnet/)
The latest CodeQL release improves language coverage and precision for identifying vulnerabilities, building on previous releases.
- [CodeQL 2.23.5 Adds New Language Support and Security Query Improvements](https://github.blog/changelog/2025-11-19-codeql-2-23-5-adds-support-for-swift-6-2-new-java-queries-and-improved-analysis-accuracy)
MLSecOps and prompt security guidance now includes support for PromptGuard 2, CodeShield, and LlamaFirewall, expanding on earlier best practices for treating prompts as code in DevOps security checks.
- [MLSecOps and Prompt Security: DevOps Strategies for AI Pipeline Protection](https://devops.com/the-mlsecops-era-why-devops-teams-must-care-about-prompt-security/)
## Microsoft Sentinel: Agentic SIEM, Automation, and AI
Sentinel’s Data Lake feature supports larger-scale case management, while custom agent tools and marketplace integrations provide flexible automation paths. Blink micro-agents and Copilot support remediation action; SAP support adds industry application.
- [Power Agentic Defense with Microsoft Sentinel: Scalable Security Operations with AI, Data Lake, and Graph Intelligence](/ai/videos/power-agentic-defense-with-microsoft-sentinel-scalable-security-operations-with-ai-data-lake-and-graph-intelligence)
- [Sentinel Alert to Autonomous Action: Controlled AI Response Framework](/ai/videos/sentinel-alert-to-autonomous-action-controlled-ai-response-framework)
- [Microsoft Sentinel Solution for SAP: Automated Asset Classification and Incident Response](/security/videos/microsoft-sentinel-solution-for-sap-automated-asset-classification-and-incident-response)
Privacy programs benefit from Copilot integration, automating many aspects of policy compliance.
- [Use AI Agents to Scale Privacy Programs with Microsoft Sentinel](/ai/videos/use-ai-agents-to-scale-privacy-programs-with-microsoft-sentinel)
## Policy, Compliance, and Governance Workflows
Azure Policy now includes Service Groups, in-guest policies, and natural language authoring via Copilot, bringing automated compliance workflow support to more teams.
- [Build Secure Applications with Azure Policy and Service Groups](/ai/videos/build-secure-applications-with-azure-policy-and-service-groups)
CIS Benchmarks are built-in and available for Azure-endorsed Linux, supporting compliance in hybrid and multi-cloud environments.
- [Built-In CIS Benchmarks for Linux Security on Azure: Flexible and Hybrid-Ready Compliance](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/from-policy-to-practice-built-in-cis-benchmarks-on-azure/ba-p/4467884)
## Other Security News
Continuous integration for security tools connects policy and evidence tracking throughout the code lifecycle, continuing recent efforts at automation and visibility.
- [Elevate DevEx 2.0 with Continuous Security Across the SDLC](/ai/videos/elevate-devex-20-with-continuous-security-across-the-sdlc)
Lifecycle coverage for .NET apps emphasizes paying for support after EOL, helping teams plan for service windows closing.
- [Managing .NET Support Lifecycles: Why Paying for Post-EOL Support Is Practical](https://andrewlock.net/companies-using-dotnet-need-to-suck-it-up-and-pay-for-support/)
Microsoft’s approach to autonomous security is reflected in unified dashboards, Copilot support, and predictive protection—linking oversight with adaptive AI techniques.
- [Ambient and Autonomous Security for the Agentic AI Era](https://www.microsoft.com/en-us/security/blog/2025/11/18/ambient-and-autonomous-security-for-the-agentic-era/)
Developments in adversarial AI defense, led by Microsoft and NVIDIA, continue to make use of real-time GPU-driven safeguards.
- [AI-Driven Adversarial Defense: Microsoft and NVIDIA''s Real-Time Immunity Collaboration](https://techcommunity.microsoft.com/blog/microsoft-security-blog/collaborative-research-by-microsoft-and-nvidia-on-real-time-immunity/4470164)
Updates in email and collaboration security, including Defender for Office 365 and agent-based controls, offer additional automation for new threat types.
- [Securing Email and Collaboration with Microsoft Defender for Office 365 and Agentic AI](/securing-email-and-collaboration-with-microsoft-defender-for-office-365-and-ai)
Endpoint and Windows security updates offer improvements in device administration, quantum-ready certificates, and patching, making security easier to manage in production.
- [Inside Windows Security from Client to Cloud: Innovations in Windows 11 and Windows 365 | BRK258](/security/videos/inside-windows-security-from-client-to-cloud-innovations-in-windows-11-and-windows-365-brk258)
Further resources for this week span cross-platform security integration, data protection, and modern architecture best practices:
- [Secure the Modern Enterprise with Varonis and Microsoft Integration](/ai/videos/secure-the-modern-enterprise-with-varonis-and-microsoft-integration)
- [Bolster Your Data Security in the AI Era with Microsoft and Netskope](/security/videos/bolster-your-data-security-in-the-ai-era-with-microsoft-and-netskope)
- [Level up Microsoft security for insider threats](/security/videos/level-up-microsoft-security-for-insider-threats)
- [Blueprint for Building the SOC of the Future](/ai/videos/blueprint-for-building-the-soc-of-the-future)
- [Preventing Data Exfiltration with Microsoft Purview''s Layered Protection Strategy](/ai/videos/preventing-data-exfiltration-with-microsoft-purviews-layered-protection-strategy)
- [Comprehensive Data Security and Governance in AI Workloads with Microsoft Purview](/ai/videos/comprehensive-data-security-and-governance-in-ai-workloads-with-microsoft-purview)
- [Envision Next Generation DLP with Microsoft Purview and Copilot](/ai/videos/envision-next-generation-dlp-with-microsoft-purview-and-copilot)
- [Data Protection in the Age of the Adversary: Accelerating Microsoft Purview Adoption](/ai/videos/data-protection-in-the-age-of-the-adversary-accelerating-microsoft-purview-adoption)
- [Maximizing Microsoft Purview Data Security Solutions: Best Practices and Implementation Stories](/security/videos/maximizing-microsoft-purview-data-security-solutions-best-practices-and-implementation-stories)
- [Enhancing Data Security Investigations with Microsoft Purview and AI](/ai/videos/enhancing-data-security-investigations-with-microsoft-purview-and-ai)
- [End-to-End Security for AI Platforms, Apps, and Agents](/ai/videos/end-to-end-security-for-ai-platforms-apps-and-agents)
- [NIST Zero Trust with Forescout and Microsoft](/security/videos/nist-zero-trust-with-forescout-and-microsoft)
- [Active Directory Disaster Recovery: Modern Approaches for Secure Forest Restoration](/active-directory-disaster-recovery-modern-approaches-for-secure-forest-estoration)
- [Building Secure-By-Design Environments with Azure Capabilities](/azure/videos/building-secure-by-design-environments-with-azure-capabilities)
- [Managing .NET Support Lifecycles: Why Paying for Post-EOL Support Is Practical](https://andrewlock.net/companies-using-dotnet-need-to-suck-it-up-and-pay-for-support/)
- [Setting Up Security Policies in Microsoft 365 Trial Tenants](https://dellenny.com/how-to-set-up-basic-security-policies-in-a-microsoft-365-trial-tenant/)
- [Setting Up Ransomware Protection in Windows 11: Step-by-Step Guide](https://dellenny.com/setting-up-ransomware-protection-in-windows-11-a-simple-and-complete-guide/)
- [Configuring Windows Firewall for Maximum Safety](https://dellenny.com/configuring-windows-firewall-for-maximum-safety/)
- [Windows 11 Security Features: Protecting Your PC and Data](https://dellenny.com/understanding-windows-11-security-features-a-shield-for-your-digital-life/)',
    'Security updates cover expanded AI integration, automation, zero-trust principles, new security features in Azure, .NET, and Microsoft 365, and more detailed data and agent governance. These changes reflect an ongoing shift toward explainable, automated, and unified security practices.',
    1763971200, 'security', '/security/roundups/weekly-security-roundup-2025-11-24', 'TechHub',
    'TechHub', '10CA8731A9639689B49742613BEC3B15DCB7E5AFD70101122B56EBB9653548CB', ',Microsoft Security,Azure Security,Zero Trust,Microsoft Entra,Non Human Identities,AI Agents,Microsoft Defender,Defender For Cloud,Security Copilot,Microsoft Sentinel,Azure Policy,Microsoft Purview,Post Quantum Cryptography,.NET,CodeQL,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-11-17
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-11-17', 'roundups', 'Weekly Security Roundup: Baselines, Secrets, and AI-Safe Dev',
    'Security updates address cloud and DevSecOps needs, focusing on AI-enabled risk management, compliance, and tightening integration into developer workflows. Key progress includes customizable baselines, updated secret scanning, enhanced AI detection, targeted incident analysis, and secure authentication guides.
<!--excerpt_end-->
## Customizable Security Baselines in Azure Machine Configuration and Policy
Azure now offers customizable security benchmarks, letting organizations modify or skip controls (CIS, Microsoft Compute Security) for Windows and Linux platforms. Developers define policies in JSON and apply them through ARM, CLI, Bicep, or CI/CD, with coverage for hybrid/multicloud via Azure Policy. Guides and tutorials clarify permissions and deployment for easier, code-based compliance. The feature is now available in public cloud regions, with government and sovereign support coming soon.
- [Customizable Security Baselines in Azure Machine Configuration: Public Preview](https://techcommunity.microsoft.com/t5/azure-governance-and-management/public-preview-introducing-customizable-security-baseline/ba/p/4469678)
- [Customizable Security Baselines Now in Preview for Azure Policy and Machine Configuration](https://techcommunity.microsoft.com/t5/azure-governance-and-management/introducing-customizable-security-baselines-in-azure-policy-and/ba/p/4469678)
## Advanced Secrets Management and Detection Tools
Improvements to secret scanning at GitHub include better private key detection and refined Sentry token alerts. New security research studies highlight how interconnected AI/dev workflows can create secret sprawl, increasing risk even further. Guidance stresses pre-commit scanning and developer diligence as essential strategies.
- [Secret Scanning Enhancements: Improved Private Key Detection and Sentry Token Updates](https://github.blog/changelog/2025-11-12-secret-scanning-improves-private-key-detection)
- [How Hyperconnected AI Development Creates a Multi-System Secret Sprawl](https://devops.com/how-hyperconnected-ai-development-creates-a-multi-system-secret-sprawl/)
## Secure Development with AI and Automated Code Generation
JFrog now supports detection of AI-generated code and Shadow AI, making it easier to track usage, licensing, and potential risks from unapproved tools. Microsoft’s BlueCodeAgent combines automated red teaming and defense rules to proactively detect LLM vulnerabilities and code bias, furthering best practices in safe AI integration.
- [JFrog Adds AI-Generated Code Detection to Secure Software Supply Chains](https://devops.com/jfrog-adds-ability-to-track-usage-of-ai-coding-tools/)
- [BlueCodeAgent: AI-Powered Blue Teaming for Secure Code Generation](https://www.microsoft.com/en-us/research/blog/bluecodeagent-a-blue-teaming-agent-enabled-by-automated-red-teaming-for-codegen-ai/)
## Security Guidance and Incident Analysis for .NET and Cloud Architects
A review of .NET security case studies provides detailed examples of common weaknesses and offers up-to-date patterns using .NET 10, Aspire, ASP.NET Core, and Visual Studio 2026. Further resources examine Microsoft’s security model, with specific advice on encryption, identity, monitoring, and compliance—delivering clear, actionable recommendations for developers and architects.
- [A Year in .NET Security: Lessons from MSRC Cases (2024–2025)](/coding/videos/a-year-in-net-security-lessons-from-msrc-cases-20242025)
- [How Microsoft Keeps Your Data Safe in the Cloud – A Deep Dive into Cloud Security Practices](https://dellenny.com/how-microsoft-keeps-your-data-safe-in-the-cloud-a-deep-dive-into-cloud-security-practices/)
## Authentication Modernization with Passkeys and SSO
Resources show how to add WebAuthn/passkey authentication options (Windows Hello, TouchID, hardware security keys) into ASP.NET Core, along with custom SSO guides using OpenIddict for improved central identity management. These updates simplify and modernize authentication approaches for business and enterprise development.
- [Going Passwordless: Implementing Passkeys in ASP.NET Core](/coding/videos/going-passwordless-implementing-passkeys-in-aspnet-core)
- [Rolling Your Own SSO: Centralized Authentication with OpenIddict](/coding/videos/rolling-your-own-sso-centralized-authentication-with-openiddict)
## Other Security News
The November update for Azure DevOps Server 2022.2 shifts TFVC Proxy hashing to SHA-256 and fixes build reliability, with guides for patching and validation.
- [November Security Patches Released for Azure DevOps Server](https://devblogs.microsoft.com/devops/november-patches-for-azure-devops-server-2/)
Microsoft’s latest Secure Future Initiative progress report details changes in environment configuration, hardware trust, AI lifecycle security, and broader use of MFA/passkey and live secret detection. This aligns with ongoing investment in cloud and AI security practices.
- [November 2025 Progress Report on Microsoft’s Secure Future Initiative](https://www.microsoft.com/en-us/security/blog/2025/11/10/securing-our-future-november-2025-progress-report-on-microsofts-secure-future-initiative/)
Coverage of server-side request forgery (SSRF) examines mechanics, risks, and practical steps to limit the attack surface, continuing the focus on up-to-date threat analysis and real-world defense strategies.
- [Why Server-Side Request Forgery (SSRF) Is a Top Cloud Security Concern](/security/videos/why-server-side-request-forgery-ssrf-is-a-top-cloud-security-concern)',
    'Security updates address cloud and DevSecOps needs, focusing on AI-enabled risk management, compliance, and tightening integration into developer workflows. Key progress includes customizable baselines, updated secret scanning, enhanced AI detection, targeted incident analysis, and secure authentication guides.',
    1763366400, 'security', '/security/roundups/weekly-security-roundup-2025-11-17', 'TechHub',
    'TechHub', '6711519593765896B092B40E4019BE081A6467E69081DD71F9922874FFE2781B', ',Azure Policy,Azure Machine Configuration,Security Baselines,CIS Benchmarks,Compliance as Code,DevSecOps,GitHub Secret Scanning,Secrets Management,Software Supply Chain Security,AI Generated Code Detection,LLM Security,ASP.NET Core,Passkeys,WebAuthn,OpenIddict,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-11-10
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-11-10', 'roundups', 'Weekly Security Roundup: AKS Zero Trust, AI Threats, Supply Chain',
    'This week’s expanded security section addresses new defensive features, recent threat research, improvements in software supply chain protection, modern secrets management, and practical cloud defense strategies. Emphasis is placed on zero-trust practices for AKS, transparent software signing, and robust management of credentials in today''s AI-driven pipelines.
<!--excerpt_end-->
## Azure Kubernetes Service (AKS) Security and Policy Enforcement
Developers get guidance for enforcing zero-trust and isolation in AKS using custom admission webhooks and policy engines (OPA Gatekeeper, Kyverno), supplementing previous content on multi-tenant setups. Tutorials feature RBAC, trusted registries, network policies, Python Flask webhook code, and quota settings. Runtime and continuous scanning practices include Falco and Prometheus. Multi-tenant architectures use Azure AD RBAC and auditing for secure isolation.
At the networking layer, Layer 7 policies via Cilium and ACNS reach general availability, enabling advanced HTTP-aware firewall rules, FQDN egress controls, and Grafana monitoring—beneficial for regulated AKS environments.
- [Zero-Trust Enforcement and Multi-Tenancy Security in Kubernetes with Custom Admission Webhooks on AKS](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/zero-trust-kubernetes-enforcing-security-multi-tenancy-with/ba/p/4466646)
- [Layer 7 Network Policies for AKS: General Availability for Enterprise-Grade Security](https://techcommunity.microsoft.com/t5/azure-networking-blog/layer-7-network-policies-for-aks-now-generally-available-for/ba/p/4467598)
## Emerging Threats and Advanced Malware Tactics
Microsoft reports on ''SesameOp'', a backdoor exploiting the OpenAI Assistants API for secret command and control, detailing payload techniques, cryptography, and detection methods. Mitigation advice includes restricting external calls and updating endpoint protections. The ''Whisper Leak'' side-channel attack uses packet size patterns to infer LLM topics over encrypted sessions. Microsoft has addressed the risk, providing obfuscation settings and secure API use recommendations.
- [SesameOp: Novel Backdoor Abuses OpenAI Assistants API for Stealth Command and Control](https://www.microsoft.com/en-us/security/blog/2025/11/03/sesameop-novel-backdoor-uses-openai-assistants-api-for-command-and-control/)
- [Whisper Leak: Novel Side-Channel Attack on Remote Language Models Uncovered by Microsoft](https://www.microsoft.com/en-us/security/blog/2025/11/07/whisper-leak-a-novel-side-channel-cyberattack-on-remote-language-models/)
## Enhancing Software Supply Chain Security
Signing Transparency (preview) from Microsoft records cryptographically verifiable logs for signed code, containers, and firmware. Logs are kept in secure ledgers with Trusted Execution Environments and Merkle proofs. Receipts support auditing, compliance (SCITT, OCP-SAFE), and assure zero-trust code provenance.
- [Enhancing Software Supply Chain Security with Microsoft’s Signing Transparency](https://azure.microsoft.com/en-us/blog/enhancing-software-supply-chain-security-with-microsofts-signing-transparency/)
## Secrets Management and Scanning for AI-Driven Development
The risk of credential leaks through AI tools in automated pipelines is detailed, with detection strategies utilizing OPA, Kyverno, GitGuardian, Gitleaks, and TruffleHog. Best practices include credential rotation, use of dynamic secrets, and zero-trust for AI outputs. GitHub secret scanning now captures Base64-encoded credentials, includes extended metadata, and adds faster remediation routes—all supporting streamlined incident response.
- [Your Next Secrets Leak is Hiding in AI Coding Tools](https://devops.com/your-next-secrets-leak-is-hiding-in-ai-coding-tools/)
- [GitHub Secret Scanning Adds Base64-Encoded and Extended Metadata Support](https://github.blog/changelog/2025-11-04-secret-scanning-now-detects-base64-encoded-secrets)
## Security Fundamentals and Platform Controls
Practical guidance covers Azure''s use of Network Security Groups, Firewalls, and Defender for Cloud, featuring setup and administration recommendations. Content explaining the Shared Responsibility Model outlines duties and effective approaches for encryption, monitoring, and patching, supported by real-world examples.
- [Azure Security Basics: Network Security Groups, Firewalls, and Defender for Cloud](https://dellenny.com/azure-security-basics-network-security-groups-firewalls-and-defender-for-cloud/)
- [Shared Responsibility Model in Azure Explained with Real Examples](https://dellenny.com/shared-responsibility-model-in-azure-explained-with-real-examples/)
## AI Governance and Security in the Enterprise
''Agentic Zero Trust'' concepts take hold, with articles detailing use of unique agent IDs, strict permission boundaries, and activity monitoring. Technologies like Entra Agent ID, Copilot Studio, Azure AI Foundry, and Defender create robust identity management, policy enforcement, and compliance structures for enterprise AI agents.
- [Beware of Double Agents: AI’s Role in Fortifying and Fracturing Cybersecurity](https://blogs.microsoft.com/blog/2025/11/05/beware-of-double-agents-how-ai-can-fortify-or-fracture-your-cybersecurity/)
## Security Automation and Incident Response with Generative AI
Security Copilot and generative AI enhance Security Operations Center workflows with better alert triage, incident correlation, detailed reporting, and faster responses. Developers can use these insights to integrate automated detection and improve SIEM operations within real-world deployments.
- [How Generative AI Transforms Security Operations Centers with Microsoft Security Copilot](https://www.microsoft.com/en-us/security/blog/2025/11/04/learn-what-generative-ai-can-do-for-your-security-operations-center-soc/)
## Other Security News
Microsoft Edge now supports passkey-based sign-in, integrating FIDO2 and biometrics or PIN authentication with syncing across devices. Microsoft Fabric SQL Database will soon offer Customer-Managed Keys and auditing, strengthening encryption and compliance for cloud databases.
- [Microsoft Edge Adds Passkey Support and Syncing with Password Manager](https://blogs.windows.com/msedgedev/2025/11/03/microsoft-edge-introduces-passkey-saving-and-syncing-with-microsoft-password-manager/)
- [Secure by Design: Upcoming CMK and Auditing Features in Fabric SQL Database](/azure/videos/secure-by-design-upcoming-cmk-and-auditing-features-in-fabric-sql-database)',
    'This week’s expanded security section addresses new defensive features, recent threat research, improvements in software supply chain protection, modern secrets management, and practical cloud defense strategies. Emphasis is placed on zero-trust practices for AKS, transparent software signing, and robust management of credentials in today''s AI-driven pipelines.',
    1762761600, 'security', '/security/roundups/weekly-security-roundup-2025-11-10', 'TechHub',
    'TechHub', 'B67C8FC21D1150A80724087D19CACB3E9704C483D83AB5F253D7E6FD9CA4BAA7', ',AKS,Kubernetes Security,Zero Trust,Admission Webhooks,OPA Gatekeeper,Kyverno,Cilium,Azure Container Networking Services,Microsoft Defender For Cloud,Software Supply Chain Security,Signing Transparency,SCITT,Secrets Management,GitHub Secret Scanning,Passkeys,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-11-03
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-11-03', 'roundups', 'Weekly Security Roundup: .NET Smuggling, AI Threats, Cloud Controls',
    'Security continues to adapt as cloud and AI tools become more common in software development. Timely vulnerability response, automation, and risk management remain priorities as generative AI and low-code tools see wider adoption. Guidance focuses on dependency scanning, identity security, and browser/cloud protection. Developers are urged to adopt modern tooling and practices for source code, cloud resource, and AI-generated code security.
<!--excerpt_end-->
## .NET, AI, and DevOps Security Risks and Mitigations
A new .NET vulnerability, CVE-2025-55315, involving HTTP request smuggling, requires developers to patch .NET 8+ apps and audit HTTP request handling. Recommendations include upgrading proxies, using HTTP/2 or HTTP/3, and validating with published tools.
Development changes and AI-driven environments call for updated security models. Automated guardrails, policy as code, and real-time compliance measures are recommended. Visibility and "mean time to intercept" metrics are now essential across the SDLC.
Guides cover best practices for safely removing secrets from Git with `git filter-repo`, generating SBOMs for supply chain security, and integrating quantum-safe tools to prepare for future risk environments.
- [Understanding the Worst .NET Vulnerability Ever: Request Smuggling and CVE-2025-55315](https://andrewlock.net/understanding-the-worst-dotnet-vulnerability-request-smuggling-and-cve-2025-55315/)
- [Securing the AI Era: How Development, Security, and Compliance Must Evolve](https://devops.com/securing-the-ai-era-how-development-security-and-compliance-must-evolve/)
- [How to Safely Remove Secrets from Your Git History (The Right Way)](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/how-to-safely-remove-secrets-from-your-git-history-the-right-way/ba-p/4464722)
- [AppOmni Open Sources Heisenberg Tool for Dependency Scanning in PRs](https://devops.com/appomni-open-sources-heisenberg-tool-to-scan-pull-requests-for-dependencies/)
- [How to Integrate Quantum-Safe Security into Your DevOps Workflow](https://devops.com/how-to-integrate-quantum-safe-security-into-your-devops-workflow/)
## Generative AI and Agentic AI Security
Microsoft identifies five security threats to generative AI: poisoning, evasion, prompt injection, deepfakes/phishing, and adaptive malware, recommending use of posture management and operational intelligence for model and data pipeline defense.
Recent surveys show that almost a quarter of application code is AI-generated, with increased vulnerabilities and incidents. This places greater importance on funding, review automation, and technical debt management. Real-time checks and ''shift-left'' security are emphasized for managing these risks.
The challenge of agent identity is addressed with Aembit’s AI agent IAM, providing verifiable credentials and adaptive policy for agent operations across cloud environments.
- [5 Critical Generative AI Security Threats: Insights from Microsoft](https://www.microsoft.com/en-us/security/blog/2025/10/30/the-5-generative-ai-security-threats-you-need-to-know-about-detailed-in-new-e-book/)
- [Survey Reveals Security Risks in AI-Generated Code](https://devops.com/survey-surfaces-rising-tide-of-vulnerabilities-in-code-generated-by-ai/)
- [Why Developer Discipline Matters More Than Ever in the AI Era](https://devops.com/why-developer-discipline-matters-more-than-ever-in-the-ai-era/)
- [Aembit Launches IAM Solution for Agentic AI in Enterprise Environments](https://devops.com/aembit-introduces-identity-and-access-management-for-agentic-ai/)
## Azure and Cloud Platform Security Controls
Updated resources continue to clarify Azure''s shared security model, helping organizations understand their responsibilities for IaaS, PaaS, and SaaS. Coverage includes OS patching, role-based access, and automated policy enforcement.
Key management articles compare built-in KMS, customer-managed keys, HSM, and Azure Key Vault, including recommendations for tenant isolation and backup.
Practical guidance for web app security adds details for HTTP header configuration and middleware, supporting secure defaults and compliance.
- [Shared Responsibility Model in Cloud Computing Simplified](https://dellenny.com/shared-responsibility-model-in-cloud-computing-simplified/)
- [Exploring Cloud Key Management Options](https://devops.com/exploring-cloud-key-management-options/)
- [Implementing Security Headers in Azure App Service and Azure Container Apps](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/implementing-security-headers-in-azure-app-service-and-azure/ba-p/4464250)
## Other Security News
Microsoft Edge has expanded its Scareware Blocker to use computer vision and real-time smart protections, updating the SmartScreen network and offering new controls for enterprise browser management.
- [Microsoft Edge Expands Scareware Blocker to Boost Real-Time Scam Protection](https://blogs.windows.com/msedgedev/2025/10/31/protecting-more-edge-users-with-expanded-scareware-blocker-availability-and-real-time-protection/)',
    'Security continues to adapt as cloud and AI tools become more common in software development. Timely vulnerability response, automation, and risk management remain priorities as generative AI and low-code tools see wider adoption. Guidance focuses on dependency scanning, identity security, and browser/cloud protection. Developers are urged to adopt modern tooling and practices for source code, cloud resource, and AI-generated code security.',
    1762156800, 'security', '/security/roundups/weekly-security-roundup-2025-11-03', 'TechHub',
    'TechHub', '9371F945DD8F20F254D6C67317C81A4D0B92EA4F57887AAEF2FCCD3E1DCCB58A', ',CVE 2025 55315,.NET 8,HTTP Request Smuggling,DevSecOps,Policy as Code,SBOM,Supply Chain Security,Dependency Scanning,Generative AI Security,Prompt Injection,AI Generated Code,IAM,Azure Key Vault,Security Headers,Microsoft Edge SmartScreen,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-10-27
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-10-27', 'roundups', 'Weekly Security Roundup: Cloud Defense, Supply Chain, and AI',
    'This week''s security updates highlight tools, incident reviews, and new practices for protecting cloud environments, supply chains, and agentic platforms. Ongoing analyses of vulnerabilities like Log4Shell are paired with stronger Azure and Microsoft defense options, including static analysis, access controls, and threat monitoring tools. Security automation and AI governance remain pressing topics.
<!--excerpt_end-->
## Open Source Security, Vulnerability Response, and Developer Support
Log4Shell continues to be a central issue. Deep analysis shows the technical details of JNDI-based remote code execution in Log4j, with tips on mitigating open source dependencies: disabling risky settings, automating defense using Dependabot/code scanning, and using SBOMs. Maintainers describe the intensity of rapid incident response, making the case for better funding and community support via programs like Secure Open Source Fund and OpenSSF. Guides advise developers on proper dependency auditing and supply chain security.
A video interview with Log4j’s Christian Grobmeier adds perspective, sharing crisis details and team challenges. Topics covered include the importance of education, funding, trust, and governance, as well as the emerging relationship between AI and security. These resources offer meaningful insights for anyone facing zero-day exploits in open ecosystems.
- [Inside the Log4Shell Breach: Lessons in Open Source Security and Sustainability](https://github.blog/open-source/inside-the-breach-that-broke-the-internet-the-untold-story-of-log4shell/)
- [The Untold Story of Log4j and Log4Shell: Inside the Crisis with Christian Grobmeier](/security/videos/the-untold-story-of-log4j-and-log4shell-inside-the-crisis-with-christian-grobmeier)
## Security Analysis and Automation in Enterprise Azure Workloads
Security for Azure workloads is explored through detailed attack chain analysis on Blob Storage. Recommended defenses include Zero Trust, role-based access controls (RBAC) via Entra ID, network isolation, encryption, and monitoring using Defender for Cloud and Sentinel SIEM. Step-by-step instructions for incident automation connect MITRE ATT&CK models with real remediation needs.
Agentic solution security guidance expands to include Key Vault management, strong RBAC, secret rotation, plugin isolation, and PII protection for agent endpoints and data resources. This builds on last week’s security best practices for agentic AI.
ContraForce/Microsoft collaboration pushes forward autonomous MDR workflows for startups and MSPs, using Azure AI Foundry and Sentinel/Defender integrations. These solutions offer automated monitoring and incident response, helping smaller teams develop secure operations.
- [Mitigating Threat Activity Targeting Azure Blob Storage: Attack Chain Insights and Defenses](https://www.microsoft.com/en-us/security/blog/2025/10/20/inside-the-attack-chain-threat-activity-targeting-azure-blob-storage/)
- [Selecting the Right Agentic Solution on Azure – Security Deep Dive](https://techcommunity.microsoft.com/t5/azure-architecture-blog/selecting-the-right-agentic-solution-on-azure-part-2-security/ba-p/4461215)
- [ContraForce and Microsoft: Enabling Autonomous Cybersecurity for Startups and MSPs](https://www.microsoft.com/en-us/startups/blog/contraforce-democratizing-cybersecurity-for-the-frontline/)
## Security Engineering and Supply Chain Defense within the GitHub Ecosystem
GitHub’s security research moves forward through new Bug Bounty details and tips for detecting supply chain and injection vulnerabilities. The VIP program offers early access and feedback for active security researchers.
CodeQL v2.23.3 brings Rust security queries, improvements for Go, Java, Kotlin, and better C/C++ detection. These improvements assist teams in automating security analysis and mapping supply chain risks—helpful for Rust and C/C++ developers in particular.
- [Inside the GitHub Bug Bounty Program: Researcher Insights and Security Advances](https://github.blog/security/top-security-researcher-shares-their-bug-bounty-process/)
- [CodeQL 2.23.3 Adds Rust Security Query, Rust Support, and Easier C/C++ Scanning](https://github.blog/changelog/2025-10-23-codeql-2-23-3-adds-a-new-rust-query-rust-support-and-easier-c-c-scanning)
## Identity, Authorization, and Platform Governance
Identity security is enhanced with the general availability of Defender for Identity’s unified sensors, combining signals from on-prem AD, Entra ID, and Okta. New APIs, improved alert accuracy, and more operational context strengthen monitoring and access management.
Fabric now supports row/column level security policies for Spark in OneLake, improving fine-grained data access alongside cluster protection—building on earlier broad workspace safeguards.
Live ASP.NET Community Standup sessions explain MCP authorization flows for .NET/ASP.NET Core, giving actionable integration and troubleshooting steps.
Cycode’s new ASPM previews provide dev teams automated AI/ML inventory and MCP/LLM detection, adopting the concept of AI BOM (Bill of Materials)—similar to SBOM—for software traceability and policy controls.
Microsoft Security Store enters public preview, unifying security software provisioning for Microsoft and partners, with direct billing and compliance mapping. This supports simplified, automated setup for security and compliance teams.
- [Harden Your Identity Defense with Microsoft Defender and Entra: Enhanced ITDR and Unified Insights](https://www.microsoft.com/en-us/security/blog/2025/10/23/harden-your-identity-defense-with-improved-protection-deeper-correlation-and-richer-context/)
- [Implementing Row and Column Level Security for Spark in OneLake](https://blog.fabric.microsoft.com/en-US/blog/how-spark-supports-onelake-security-with-row-and-column-level-security-policies/)
- [ASP.NET Community Standup: Understanding the MCP Authorization Flow](/coding/videos/aspnet-community-standup-understanding-the-mcp-authorization-flow)
- [Cycode Unveils AI Tool and Platform Detection for Application Security Teams](https://devops.com/cycode-previews-ability-to-identify-ai-tools-and-platforms-used-to-write-code/)
- [The New Microsoft Security Store: Unifying Partners and Innovation for Stronger Security](https://www.microsoft.com/en-us/security/blog/2025/10/21/the-new-microsoft-security-store-unites-partners-and-innovation/)',
    'This week''s security updates highlight tools, incident reviews, and new practices for protecting cloud environments, supply chains, and agentic platforms. Ongoing analyses of vulnerabilities like Log4Shell are paired with stronger Azure and Microsoft defense options, including static analysis, access controls, and threat monitoring tools. Security automation and AI governance remain pressing topics.',
    1761552000, 'security', '/security/roundups/weekly-security-roundup-2025-10-27', 'TechHub',
    'TechHub', '91A33E49600ADE5B01F2E7FD7D0015A8F4BFCDA7C75C594C328D5AC87A4C1EA0', ',Log4Shell,Log4j,OpenSSF,SBOM,Dependabot,GitHub Advanced Security,CodeQL,Azure Blob Storage,Zero Trust,Microsoft Entra ID,RBAC,Microsoft Defender For Cloud,Microsoft Sentinel,Microsoft Defender For Identity,MCP Authorization,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-10-20
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-10-20', 'roundups', 'Weekly Security Roundup: Roots of Trust, SOC AI, and Safer Supply Chains',
    'Security news this week covers updates to open-source hardware, data protection features, secure analytics, encrypted DevOps workflows, and improved guidance for software supply chain safety.
<!--excerpt_end-->
## Open-Source Silicon Security and Quantum-Resilient Hardware Roots of Trust
Caliptra 2.1 from Microsoft adds quantum-resilient cryptography, advanced key management, and secure erase options. Formal property verification with open documentation helps silicon developers and cloud providers build stronger confidential computing environments.
- [Caliptra 2.1: Advancing Open-Source Silicon Root of Trust for Data Protection](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/caliptra-2-1-an-open-source-silicon-root-of-trust-with-enhanced/ba-p/4460758)
## Secure Analytics Platforms: Customer-Managed Keys and Outbound Access Protection in Microsoft Fabric
Microsoft Fabric now features customer-managed keys for workspace encryption and has moved outbound access protection to general availability for Warehouses, Notebooks, and SQL Analytics. Updates make securing and controlling access at the workspace level easier for teams.
- [Extending Outbound Access Protection to Fabric Warehouse and SQL Analytics Endpoint](https://blog.fabric.microsoft.com/en-US/blog/extending-outbound-access-protection-to-fabric-warehouse-and-sql-analytics-endpoint/)
- [Customer-Managed Keys for Microsoft Fabric Workspaces Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-for-fabric-workspaces-generally-available/)
- [Customer-Managed Keys Now Available for Fabric Warehouse and SQL Analytics Endpoint](https://blog.fabric.microsoft.com/en-US/blog/bringing-customer-managed-keys-to-fabric-warehouse-and-sql-analytics-endpoint/)
## AI Benchmarks and Open Security Tooling for Modern SOC Workflows
The ExCyTIn-Bench toolkit simulates advanced attack scenarios, helping SOC teams assess LLM performance using Sentinel log data and incident graphs. Open results and adaptability help speed up adoption of security-focused AI tools and features.
- [ExCyTIn-Bench: Benchmarking AI Performance in Cybersecurity Investigations](https://www.microsoft.com/en-us/security/blog/2025/10/14/microsoft-raises-the-bar-a-smarter-way-to-measure-ai-for-cybersecurity/)
- [Open Source Benchmarking Tool to Measure AI for Cybersecurity](https://www.linkedin.com/posts/satyanadella_microsoft-raises-the-bar-a-smarter-way-to-activity-7384329693614759936-VB0Z)
## DevOps Security: Modern Authentication, Secure Data Sharing, and End-to-End Encryption
Microsoft is transitioning authentication procedures for Visual Studio and Azure DevOps to Entra ID, improving security and access control. Delta Sharing in Databricks is now safer thanks to implementation of NCC and private endpoints. Research continues into adopting end-to-end encryption for Git, supporting improved software supply chain security.
- [Modernizing Authentication for Legacy Visual Studio Clients](https://devblogs.microsoft.com/devops/modernizing-authentication-for-legacy-visual-studio-clients/)
- [Secure Delta Sharing Between Databricks Workspaces Using NCC and Private Endpoints](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/secure-delta-sharing-between-databricks-workspaces-using-ncc-and/ba/p/4462428)
- [Efficient End-to-End Encryption for Git Services: Enhancing DevOps Security](https://devops.com/git-services-need-better-security-heres-how-end-to-end-encryption-could-help/)
## Secure Coding and Supply Chain Defense in Developer Workflows
GitHub CodeQL’s Rust support and build-free C/C++ scanning improve developer ability to identify vulnerabilities, especially during CI/CD and code review. Tutorials on using SBOMs, VEX advisories, and eBPF for runtime inspection extend supply chain policy and runtime observability.
- [CodeQL Adds Rust and Build-Free C/C++ Scanning in General Availability](https://github.blog/changelog/2025-10-14-codeql-scanning-rust-and-c-c-without-builds-is-now-generally-available)
- [Establishing Visibility and Governance for Your Software Supply Chain](https://devops.com/establishing-visibility-and-governance-for-your-software-supply-chain/)
## Marketplace, Extension, and Privileged Access Risks
Wiz researchers found over 500 VS Code and OpenVSX extensions with hardcoded secrets, putting over 150,000 users at risk; Microsoft has introduced secret scans before publication. Updated best practices for privileged tools cover measures such as PRMFA, RBAC, and JIT/JEA to help isolate high-risk actions.
- [VS Code Marketplace Secret Leaks Highlight Risks in Extensions and AI Configurations](https://devops.com/massive-vs-code-secrets-leak-puts-focus-on-extensions-ai-wiz/)
- [Hardening Customer Support Tools Against Cyberattacks: Microsoft’s Approach](https://www.microsoft.com/en-us/security/blog/2025/10/15/the-importance-of-hardening-customer-support-tools-against-attack/)
## Other Security News
Microsoft has started .NET Security Group to coordinate CVE management for faster, more effective patching. GitHub is updating Dependabot Alerts API to use cursor-based pagination, streamlining supply chain notification. Industry commentary urges teams to use context-aware controls, monitoring, and incident response practices rather than relying solely on “shift left” development.
- [Announcing the .NET Security Group](https://devblogs.microsoft.com/dotnet/announcing-dotnet-security-group/)
- [Dependabot Alerts API Deprecates Offset-Based Pagination Parameters](https://github.blog/changelog/2025-10-14-dependabot-alerts-api-pagination-parameters-deprecated)
- [Why ''Shift Left'' Alone Isn''t Enough: Embedding Security Across Software Delivery](https://devops.com/secure-by-design-secure-by-default/)',
    'Security news this week covers updates to open-source hardware, data protection features, secure analytics, encrypted DevOps workflows, and improved guidance for software supply chain safety.',
    1760943600, 'security', '/security/roundups/weekly-security-roundup-2025-10-20', 'TechHub',
    'TechHub', '936D8FC299E7F4102B9D3BDDCCD194D64315FEBDBB89A50343BED112A614AFAF', ',Caliptra,Root Of Trust,Quantum Resistant Cryptography,Confidential Computing,Microsoft Fabric,Customer Managed Keys,Outbound Access Protection,Microsoft Sentinel,SOC,LLM Security Benchmarking,Microsoft Entra ID,Azure DevOps,Git End To End Encryption,GitHub CodeQL,SBOM,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-10-13
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-10-13', 'roundups', 'Weekly Security Roundup: Active Exploits, Zero Trust, and Secure SDLC',
    'Security topics this week center on threat investigation, updated implementation patterns, and developer tooling to keep codebases secure. Reports detail emergency vulnerability exploitation and targeted cloud attacks; step-wise guides show how organizations can build secure environments, lower risks, and improve developer protection with static analysis, secrets scanning, and policy choices.
<!--excerpt_end-->
## Threat Intelligence: Active Exploitation Reports and Attack Chain Analysis
Microsoft investigated Storm-1175’s exploitation of CVE-2025-10035 in GoAnywhere MFT (pre-v7.8.3)—using deserialization attacks, remote management, ransomware, and credential theft. The article shares attack breakdowns, detection indicators, hunting queries, and upgrade recommendations.
Storm-2657’s "Payroll Pirate" targets US universities through Workday HR, using adversary-in-the-middle attacks to steal credentials and divert payroll. Guidance includes immediate credential resets, multi-factor authentication, device cleaning, and inbox review. Automated queries support rapid cloud response workflows, continuing last week’s focus on CI/CD and SaaS risk isolation.
- [Investigating Active Exploitation of CVE-2025-10035 in GoAnywhere Managed File Transfer](https://www.microsoft.com/en-us/security/blog/2025/10/06/investigating-active-exploitation-of-cve-2025-10035-goanywhere-managed-file-transfer-vulnerability/)
- [Investigating Storm-2657 ''Payroll Pirate'' Attacks Targeting US Universities](https://www.microsoft.com/en-us/security/blog/2025/10/09/investigating-targeted-payroll-pirate-attacks-affecting-us-universities/)
## Practical Security Architecture and Workflow Hardening
Organizations adopt secure architecture patterns from Microsoft’s Secure Future Initiative, using network segmentation, Entra ID, Conditional Access, Zero Trust in CI/CD, and central detection/logging. Guides help users map ZTA pillars to services like Entra ID, Defender, Intune, Purview, Sentinel, and Logic Apps—covering identity, application, data, and incident response. Stepwise advice, tips, and challenges support security modernization.
Securing Teams workflows includes attack chain analysis, defense controls, and detection queries for Microsoft 365, embedding security into incident management. These guides continue progress on multi-layer defense and policy automation.
- [Microsoft Secure Future Initiative: Practical Patterns and Practices for Enhanced Security](https://www.microsoft.com/en-us/security/blog/2025/10/07/new-microsoft-secure-future-initiative-sfi-patterns-and-practices-practical-guides-to-strengthen-security/)
- [Implementing Zero Trust Architecture in an Azure Environment](https://dellenny.com/implementing-zero-trust-architecture-in-an-azure-environment/)
- [Mitigating Threats Targeting Microsoft Teams: Attack Chain and Defense Strategies](https://www.microsoft.com/en-us/security/blog/2025/10/07/disrupting-threats-targeting-microsoft-teams/)
## Infrastructure and Application Security Tooling
CodeQL 2.23.2 update adds Rust queries for URL security, improved JS/TS SDK dataflow, upgraded Python and Ruby analysis, expanded Go registry checks, and enhanced C# null detection. These changes help reduce false positives and cover a broader range of languages. Most GitHub users are auto-updated; enterprises are guided through manual upgrades.
A new article addresses five common Infrastructure-as-Code (IaC) issues (drift, policy gaps, audits, excessive permissions, hardcoded secrets) and solutions such as drift detection, OPA/Terraform policies, secret management, and audit logging in CI/CD pipelines. These best practices follow recent advice on policy management and secured cloud automation.
- [CodeQL 2.23.2 Adds Rust Security Detections and Enhanced Language Support](https://github.blog/changelog/2025-10-09-codeql-2-23-2-adds-additional-detections-for-rust-and-improves-accuracy-across-languages)
- [Common IaC Security Issues and How to Fix Them](https://devops.com/common-iac-security-issues-and-how-to-fix-them/)
## Other Security News
GitHub Enterprise Cloud now offers up to 20 Enterprise Managed User IDs in a proxy header, simplifying authentication and governance across business units—continuing improvements to centralized identity management.
GitHub secret protection adds new default credential scan patterns and strengthens push protection, moving forward with automated secrets scanning from last week.
Microsoft Fabric’s OneLake Security preview brings central RBAC and Row/Column-level SQL security, advancing workspace isolation and access control coverage.
Microsoft Ignite 2025 previews security sessions focused on agentic AI, Zero Trust, enterprise DLP, and Copilot security features, continuing discussion on agentic protection and layered defense.
- [GitHub Enterprise Cloud Access Restrictions Now Support Multiple Enterprises](https://github.blog/changelog/2025-10-06-enterprise-access-restrictions-now-supports-multiple-enterprises)
- [GitHub Secret Protection Expands Default Pattern Support – September 2025](https://github.blog/changelog/2025-10-07-secret-protection-expands-default-pattern-support-september-2025)
- [OneLake Security on the SQL Analytics Endpoint](https://blog.fabric.microsoft.com/en-US/blog/onelake-security-on-the-sql-analytics-endpoint/)
- [Securing Agentic AI and Data: Microsoft Ignite 2025 Security Sessions Preview](https://www.microsoft.com/en-us/security/blog/2025/10/09/securing-agentic-ai-your-guide-to-the-microsoft-ignite-sessions-catalog/)',
    'Security topics this week center on threat investigation, updated implementation patterns, and developer tooling to keep codebases secure. Reports detail emergency vulnerability exploitation and targeted cloud attacks; step-wise guides show how organizations can build secure environments, lower risks, and improve developer protection with static analysis, secrets scanning, and policy choices.',
    1760338800, 'security', '/security/roundups/weekly-security-roundup-2025-10-13', 'TechHub',
    'TechHub', '1705F9B62584EEA89DE430E3D0C33DEA752D72B9D0D6EFF3C9C50C56814622F9', ',Threat Intelligence,Incident Response,Vulnerability Management,CVE 2025 10035,GoAnywhere MFT,Ransomware,Credential Theft,Adversary in The Middle,Workday Security,Zero Trust Architecture,Microsoft Entra ID,Conditional Access,Microsoft Sentinel,GitHub CodeQL,Secrets Scanning,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-10-06
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-10-06', 'roundups', 'Weekly Security Roundup: Supply Chain Worms and AI Governance',
    'Security news this week centers on practical advice for securing DevOps supply chains, AI workloads, and enterprise systems. Microsoft adds AI-driven security features and tighter governance, with ongoing incidents emphasizing risk management in automated environments.
<!--excerpt_end-->
## DevOps and Supply Chain Security
Building on previous registry security discussions, the Shai-Hulud worm incident highlights risks in the DevOps supply chain—infecting npm packages and spreading via GitHub Actions due to pipeline gaps. The event stresses the need for ephemeral credentials, workflow isolation, artifact tracking, and real-time secret scanning. Industry guides debate custom-built versus off-the-shelf supply chain protection, underlining the need for thorough engineering and validation. Harness’s Qwiet AI (ShiftLeft) acquisition continues automation of security into native developer workflows.
- [Shai-Hulud: Supply Chain Worm Sheds Light on DevOps Security Risks](https://devops.com/worms-in-the-supply-chain-shai-hulud-and-the-next-devops-reckoning/)
- [Build vs. Buy: What it Really Takes to Harden Your Software Supply Chain](https://devops.com/build-vs-buy-what-it-really-takes-to-harden-your-software-supply-chain/)
- [Harness Acquires Qwiet AI to Strengthen AI-Driven Application Security in DevOps](https://devops.com/harness-acquires-qwiet-ai-to-gain-code-testing-tool/)
## Cloud Security, AI Workload Protection, and Governance on Azure
Microsoft Sentinel evolves into an agentic SIEM, integrating unified data lakes, graph-based threat tracing, AI agents, and workflow automation—expanding extensibility with VS Code, GitHub Copilot, and Security Store integrations. Security guides detail methods for protecting Azure AI workloads by deploying multiple layers: Defender for Cloud for threats, Purview for data classification, and Sentinel for incident response. Reference architectures and automation templates ease compliance for GPU VMs, AKS clusters, and data stores. Fabric’s Outbound Access Protection for Spark restricts data exfiltration, enhancing security for analytics and ML. Microsoft Purview announces new data classification and compliance tools following last week’s advanced data loss prevention (DLP) and labeling coverage.
- [Empowering Defenders in the Era of Agentic AI with Microsoft Sentinel](https://www.microsoft.com/en-us/security/blog/2025/09/30/empowering-defenders-in-the-era-of-agentic-ai-with-microsoft-sentinel/)
- [Securing AI Workloads with Microsoft Defender for Cloud, Purview, and Sentinel in Azure Landing Zones](https://techcommunity.microsoft.com/t5/azure-architecture-blog/securing-ai-workloads-with-microsoft-defender-for-cloud-purview/ba/p/4457345)
- [Outbound Access Protection for Spark Now Generally Available in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/workspace-outbound-access-protection-for-spark-is-now-generally-available/)
- [Data Security and Governance Announcements with Talhah Mir at Microsoft Ignite](/security/videos/data-security-and-governance-announcements-with-talhah-mir-at-microsoft-ignite)
## Cloud Identity and Access Management (IAM)
Microsoft Entra decouples identity and security management from Azure, supporting unified Zero Trust for hybrid, cloud, and on-premises environments. Developers get hands-on guides for new features and secure authentication. Conditional Access policy troubleshooting reveals resource mapping gaps in Windows App/365, prompting calls for better documentation and tooling—topics covered in previous best practice guides.
- [What Microsoft Entra Really Means for Identity and Security](https://dellenny.com/what-microsoft-entra-really-means-for-identity-and-security/)
- [Conditional Access Policy Limitation: Windows 365 Portal Not Found in Target Resources](https://techcommunity.microsoft.com/t5/azure-virtual-desktop/ca-policy-application-not-found-in-target-resources/m-p/4458834#M13916)
## Security Automation and Secret Management
GitHub secret scanning now validates credentials for Azure, MongoDB, and Meta, automating leak detection and incident response. Microsoft and HashiCorp’s best practices for Vault, Terraform, and Azure Verified Modules address identity-aware credential management, audit requirements, and privilege escalation risks in agent-based environments—continuing last week’s updates on managed identity and rotation.
- [GitHub Secret Scanning Now Validates Azure, MongoDB, and Meta Credentials](https://github.blog/changelog/2025-09-30-secret-scanning-adds-validators-for-mongodb-meta-and-microsoft-azure)
- [Securing AI Deployments with HashiCorp Vault & Azure](/ai/videos/securing-ai-deployments-with-hashicorp-vault-and-azure)
## Advanced Security Analysis and Developer Tutorials
A step-by-step guide to debugging CodeQL queries for Gradio Python vulnerabilities demonstrates the use of custom taint flows, abstract syntax tree visualization in VS Code, and refining query outputs. These lessons expand on previous CodeQL tutorials focused on strong static analysis.
- [Debugging CodeQL Queries: Lessons from Gradio Vulnerability Research](https://github.blog/security/vulnerability-research/codeql-zero-to-hero-part-5-debugging-queries/)
## Other Security News
Development tools now offer smoother debugging and improved performance, tackling workflow bottlenecks and supporting more productive routines.
- [Debugging CodeQL Queries: Lessons from Gradio Vulnerability Research](https://github.blog/security/vulnerability-research/codeql-zero-to-hero-part-5-debugging-queries/)
Security advancements include more effective vulnerability management and compliance tools, following last week’s work on artifact signing and registry updates.
- [Build vs. Buy: What it Really Takes to Harden Your Software Supply Chain](https://devops.com/build-vs-buy-what-it-really-takes-to-harden-your-software-supply-chain/)
- [Harness Acquires Qwiet AI to Strengthen AI-Driven Application Security in DevOps](https://devops.com/harness-acquires-qwiet-ai-to-gain-code-testing-tool/)
Updated migration and troubleshooting resources continue to support teams in solving everyday technical issues.
- [Securing Modern Education with Windows 11: AI, Intune, and Learning Zone](https://www.microsoft.com/en-us/education/blog/2025/10/build-secure-future-ready-learning-experiences-with-windows-11/)',
    'Security news this week centers on practical advice for securing DevOps supply chains, AI workloads, and enterprise systems. Microsoft adds AI-driven security features and tighter governance, with ongoing incidents emphasizing risk management in automated environments.',
    1759734000, 'security', '/security/roundups/weekly-security-roundup-2025-10-06', 'TechHub',
    'TechHub', 'FFDF312203FBCCF620331729A50EF464A93D3D53BFBF4929BCB59C10BE90A157', ',DevSecOps,Software Supply Chain Security,GitHub Actions,npm,Secret Scanning,CodeQL,Static Application Security Testing,Microsoft Sentinel,Security Information And Event Management,Microsoft Defender For Cloud,Microsoft Purview,Microsoft Entra,Conditional Access,HashiCorp Vault,Microsoft Fabric,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-09-29
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-09-29', 'roundups', 'Weekly Security Roundup: Trusted Publishing, Signing, and Response',
    'Recent updates in security emphasize supply chain protection, vulnerability remediation, artifact signing, and up-to-date governance for developers working in increasingly risk-aware environments.
<!--excerpt_end-->
## Package Registry and Supply Chain Security
NuGet.org now supports Trusted Publishing with short-lived OIDC tokens through GitHub Actions, replacing static keys and improving .NET package safety. Npm registry updates include enforced 2FA, short-lived tokens, and trusted publishing. Chainguard’s curated JavaScript repository adds SLSA provenance and malware scanning for safer dependencies.
- [New Trusted Publishing Enhances Security on NuGet.org](https://devblogs.microsoft.com/dotnet/enhanced-security-is-here-with-the-new-trust-publishing-on-nuget-org/)
- [GitHub''s Roadmap for Strengthening npm Supply Chain Security](https://github.blog/security/supply-chain-security/our-plan-for-a-more-secure-npm-supply-chain/)
- [How GitHub Plans to Secure npm After Recent Supply Chain Attacks](https://devops.com/how-github-plans-to-secure-npm-after-recent-supply-chain-attacks/)
- [Chainguard Launches Curated JavaScript Libraries to Enhance Software Supply Chain Security](https://devops.com/chainguard-adds-curated-repository-to-secure-javascript-libraries/)
## Code Scanning, Static Analysis, and Remediation Workflows
CodeQL 2.23.1 introduces improved language detection and query updates for common vulnerabilities, like SSRF and CORS. Incremental analysis speeds scanning for pull requests, and GitHub Security Campaigns with Assignable Alerts help teams coordinate and track remediation within CI flows.
- [CodeQL 2.23.1 Released: Java 25, TypeScript 5.9, and Swift 6.1.3 Support](https://github.blog/changelog/2025-09-26-codeql-2-23-1-adds-support-for-java-25-typescript-5-9-and-swift-6-1-3)
- [Incremental Security Analysis with CodeQL Now Available Across All Languages](https://github.blog/changelog/2025-09-23-incremental-security-analysis-with-codeql-is-now-available-for-all-languages)
- [Accelerate Remediation with GitHub Security Campaigns and Assignable Alerts](https://github.blog/changelog/2025-09-23-accelerate-remediation-with-security-campaigns-and-assignable-alerts-for-code-scanning-and-secret-scanning)
## Artifact Signing, Infrastructure, and Cloud Security
Azure Trusted Signing (public preview) and Notary Project now support integrated signing of OCI images, SBOMs, and Helm charts, helping automate certificate handling for CI/CD. RBAC for AI Landing Zones and secure Databricks deployments via Private Link/Azure Firewall provide templates for regulated operational security.
- [Simplify Image Signing and Verification with Notary Project and Trusted Signing (Public Preview)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/simplify-image-signing-and-verification-with-notary-project-and/ba/p/4455292)
- [Enterprise-Ready RBAC Model for Azure AI Landing Zone](https://techcommunity.microsoft.com/t5/azure-architecture-blog/access-governance-blueprint-for-ai-landing-zone/ba/p/4455910)
- [Securing Azure Databricks Serverless with Private Link and Azure Firewall](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/securing-azure-databricks-serverless-practical-guide-to-private/ba/p/4457083)
## Threat Intelligence, Malware, and Incident Response
Microsoft details the latest XCSSET malware variant targeting macOS dev tools, with mitigation strategies for Defender XDR users. A retail sector incident report outlines response tactics to SharePoint-based attacks, stressing rapid patching and Zero Trust controls. Threat intelligence detects new AI-obfuscated phishing techniques, showcasing layered defense strategies.
- [Latest XCSSET Malware Variant: Technical Deep Dive and Mitigation Guidance](https://www.microsoft.com/en-us/security/blog/2025/09/25/xcsset-evolves-again-analyzing-the-latest-updates-to-xcssets-inventory/)
- [Retail at Risk: How a Single Alert Uncovered a Major Cyberthreat](https://www.microsoft.com/en-us/security/blog/2025/09/24/retail-at-risk-how-one-alert-uncovered-a-persistent-cyberthreat/)
- [AI-Obfuscated Phishing Campaign Detection by Microsoft Threat Intelligence](https://www.microsoft.com/en-us/security/blog/2025/09/24/ai-vs-ai-detecting-an-ai-obfuscated-phishing-campaign/)
## Identity, Data Protection, and Developer Security Skills
A Microsoft Entra Suite guide outlines unified identity, access, risk, passwordless options, and multi-cloud gateways for zero trust. Purview’s DLP and sensitivity labeling (now GA for Fabric) assist with policy enforcement and auditing. OneLake Catalog previews a centralized security permissions tab. An Azure OpenAI customer success story demonstrates App Gateway and NSGs for secure access. A DevSecOps guide covers career progression and practical skills for developers.
- [Microsoft Entra Suite: The Future of Identity and Access Security](https://dellenny.com/microsoft-entra-suite-the-future-of-identity-and-access-security/)
- [Protecting Microsoft Fabric Data with Purview DLP and Sensitivity Labels](https://blog.fabric.microsoft.com/en-US/blog/protecting-your-fabric-data-using-purview-is-now-generally-available/)
- [View and Manage Security in the OneLake Catalog (Preview)](https://blog.fabric.microsoft.com/en-US/blog/view-and-manage-security-in-the-onelake-catalog-now-in-preview/)
- [Securing Azure OpenAI Access from On-Premises with Application Gateway: A Customer Success Story](https://techcommunity.microsoft.com/t5/azure-networking-blog/using-application-gateway-to-secure-access-to-the-azure-openai/ba/p/4456696)
- [The DevSecOps Career Path: What No One Tells You About Getting Started](https://devops.com/the-devsecops-career-path-what-no-one-tells-you-about-getting-started/)
## Other Security News
A practical guide details JWT authentication and authorization for MCP servers in agentic platforms and microservices. GitHub’s Bug Bounty program increases incentives for Copilot ecosystem vulnerability research during Cybersecurity Awareness Month, inviting more robust security testing of developer tooling.
- [Securing Your MCP Server with JWT Authentication and Authorization](https://techcommunity.microsoft.com/t5/microsoft-developer-community/it-s-time-to-secure-your-mcp-servers-here-s-how/ba/p/4434308)
- [GitHub Bug Bounty: Enhanced Incentives for Cybersecurity Awareness Month 2025](https://github.blog/security/vulnerability-research/kicking-off-cybersecurity-awareness-month-2025-researcher-spotlights-and-enhanced-incentives/)',
    'Recent updates in security emphasize supply chain protection, vulnerability remediation, artifact signing, and up-to-date governance for developers working in increasingly risk-aware environments.',
    1759129200, 'security', '/security/roundups/weekly-security-roundup-2025-09-29', 'TechHub',
    'TechHub', '16D9A828330A0ADB19283B4045083BD725DCFB66B47671DD3159E795434918DF', ',Software Supply Chain Security,Trusted Publishing,OIDC,GitHub Actions,NuGet,npm,SLSA Provenance,Artifact Signing,Notary Project,Azure Trusted Signing,CodeQL,Static Analysis,GitHub Security Campaigns,Microsoft Entra,Microsoft Purview,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-09-22
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-09-22', 'roundups', 'Weekly Security Roundup: Supply Chain, GitHub Controls, PQ SSH',
    'This week’s security updates highlight more sophisticated supply chain attacks and stronger platform controls, with vendor improvements addressing code safety, governance, identity management, and quantum-era risks.
<!--excerpt_end-->
## Malicious Extension Threats in Developer Ecosystems
Investigations reveal the WhiteCobra group distributing advanced malicious VSCode and Open VSX Marketplace extensions, following last week’s focus on AI tool security. WhiteCobra employs payloads split across helper files and scripts to circumvent static analysis, with LummaStealer stealing wallets, credentials, and cloud accounts. Automated fake reviews, cross-market spreading (Cursor/Windsurf), and multi-OS payloads escalate risks, including some targeting Ethereum contributors. Experts recommend exacting extension screening, stronger monitoring, and improved supply chain security.
- [WhiteCobra Targets Developers with Malicious VSCode Marketplace Extensions](https://devops.com/whitecobra-targets-developers-with-dozens-of-malicious-extensions/?utm_source=rss&utm_medium=rss&utm_campaign=whitecobra-targets-developers-with-dozens-of-malicious-extensions)
- [WhiteCobra’s Malicious VSCode Extensions Pose Major Security Risk for Developers](https://devops.com/whitecobra-targets-developers-with-dozens-of-malicious-extensions/)
## Enterprise Platform Security: GitHub’s New Controls
GitHub released general availability for enterprise access controls using corporate proxies, supporting compliance by routing traffic behind enterprise firewalls through customized headers—responding to last week’s calls for robust access management and registry controls.
Centralized security contacts now coordinate incident alerts for large organizations. Delegated bypass for push protection allows admins to oversee secret exposures and approve exceptions through APIs, streamlining governance and incident response.
- [Enterprise Access Restrictions with Corporate Proxies for GitHub Enterprise Cloud Now Available](https://github.blog/changelog/2025-09-15-enterprise-access-restrictions-with-corporate-proxies-is-now-generally-available)
- [Security Contact Email Setting for Enterprise Incident Notifications](https://github.blog/changelog/2025-09-14-security-contact-for-security-notification-emails-is-generally-available)
- [Delegated Bypass Controls for Push Protection Now Available at the Enterprise Level](https://github.blog/changelog/2025-09-16-delegated-bypass-controls-for-push-protection-now-available-at-the-enterprise-level)
## Preparing for the Quantum Era: Post-Quantum Secure SSH on GitHub
GitHub now defaults to post-quantum secure SSH key exchange for Git operations, using the hybrid `sntrup761x25519-sha512` algorithm as of September 17, 2025. Compatible OpenSSH clients (9.0+) are automatically covered, helping protect source code against future quantum threats. The change builds on last week’s progress in encryption and source control safety.
- [Post-Quantum Secure SSH Access on GitHub](https://github.blog/engineering/platform-security/post-quantum-security-for-ssh-access-on-github/)
## Securing the Software Supply Chain and Open Source Dependencies
Shai-Hulud, an NPM worm, used typosquatting and replication to compromise Node.js/JavaScript packages, raising publisher and dependency risks. Best practices now include SBOMs, MFA, signed packages, version pinning, and consistent audits—reinforcing supply chain hygiene and earlier DevOps security topics.
- [Shai-Hulud Attacks Undermine Software Supply Chain Security](https://devops.com/shai-hulud-attacks-shake-software-supply-chain-security-confidence/)
## Other Security News
Microsoft updates Purview tools for Fabric, with stricter data protection, DLP, insider risk management, assessment, and better cataloging—mirroring previous access control developments.
- [Microsoft Purview Innovations for Fabric: Unifying Data Security and Governance for AI](https://www.microsoft.com/en-us/security/blog/2025/09/16/microsoft-purview-innovations-for-your-fabric-data-unify-data-security-and-governance-for-the-ai-era/)
Identity protection tips cover hybrid settings in Active Directory and cloud Entra ID, continuing last week’s advice on hybrid identity, backup, and recovery practices to address evolving risks.
- [Protecting Identity in Active Directory & Microsoft Entra](https://www.thomasmaurer.ch/2025/09/protecting-identity-in-active-directory-microsoft-entra/)',
    'This week’s security updates highlight more sophisticated supply chain attacks and stronger platform controls, with vendor improvements addressing code safety, governance, identity management, and quantum-era risks.',
    1758524400, 'security', '/security/roundups/weekly-security-roundup-2025-09-22', 'TechHub',
    'TechHub', '2519A0405FFEFE355E38BD94F1121669C91522CFEAD4E1C255AB5D41514EBF6B', ',Supply Chain Security,VS Code Extensions,Open VSX,Malicious Extensions,LummaStealer,npm,Typosquatting,SBOM,MFA,Package Signing,GitHub Enterprise Cloud,Push Protection,Corporate Proxies,Post Quantum Cryptography,OpenSSH,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-09-15
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-09-15', 'roundups', 'Weekly Security Roundup: Access Controls, Scanning, and Tool Risk',
    'Security updates this week address new data access controls, developer automation, and tool vulnerabilities, with a clear focus on managed access, code and secret scanning, and evolving policy for AI-augmented coding environments.
<!--excerpt_end-->
## Data Access Control and Perimeter Security in Cloud Platforms
Microsoft Fabric’s OneLake now offers unified management for RBAC and fine-grained row/column security, supporting consistent analytics enforcement and up to 4x faster queries. Upgrades are automatic, with management via UI or API for easier governance.
Azure Storage introduces network security perimeters for PaaS resources, centralizing boundary and access management with default public access denial and integrated auditing—no extra cost. This improves risk assessment and compliance for larger deployments.
These new features expand unified access control highlighted last week and address compliance for analytics and storage.
- [Announcing OneLake Security (Preview): Fine-Grained Data Access Control in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/onelake-security-is-now-available-in-public-preview/)
- [Protect Azure Storage Accounts with Network Security Perimeter: General Availability](https://techcommunity.microsoft.com/t5/azure-storage-blog/protect-your-storage-accounts-using-network-security-perimeter/ba-p/4449046)
## DevSecOps, Vulnerability Scanning, and Supply Chain Threats
Fast DevSecOps pipelines now use context-aware vulnerability scanning, integrating with pull requests and ticketing to provide targeted alerts and reduce signal overload—optimizing for reduced time-to-fix and fewer false positives. Coverage includes APIs, infrastructure-as-code, dependencies, and runtime. Feedback links security and development for quicker mitigation.
GitHub’s "The Download" covers an npm ecosystem attack, emphasizing ongoing supply chain risks and best practices for dependency auditing.
These developments reinforce the need for prompt, actionable security and supply chain vigilance.
- [What Makes Vulnerability Scanning Effective in Fast-Moving DevSecOps Pipelines Today?](https://devops.com/what-makes-vulnerability-scanning-effective-in-fast-moving-devsecops-pipelines-today/?utm_source=rss&utm_medium=rss&utm_campaign=what-makes-vulnerability-scanning-effective-in-fast-moving-devsecops-pipelines-today)
- [The Download: npm Supply Chain Attack, NVIDIA Rubin Platform, VS Code Dev Days & More](/ai/videos/the-download-npm-supply-chain-attack-nvidia-rubin-platform-vs-code-dev-days-and-more)
## Automated Code and Secrets Security for Developers
GitHub’s CodeQL 2.23.0 now detects Rust log/path injection, broadens data modelling, and enhances detection across C/C++, C#, Java, and Python. Updates auto-deploy with code scanning to all supported languages. GitHub Enterprise Cloud introduces secret scanning validity checks for new tokens, helping teams spot exposed active secrets.
Hush Security’s platform replaces persistent application secrets with dynamic, just-in-time identity validation (using CNCF SPIFFE), supporting policy migration and automated secret management—helpful for teams running microservices or AI workloads under zero trust.
Following recent expansions in automated scanning, these updates improve detection, clarify secret validity, and reinforce best practices for secure deployment.
- [CodeQL 2.23.0: New Rust Log Injection Detection and Security Improvements](https://github.blog/changelog/2025-09-10-codeql-2-23-0-adds-support-for-rust-log-injection-and-other-security-detection-improvements)
- [Secret Scanning Validity Checks Now Available for GitHub Enterprise Cloud with Data Residency](https://github.blog/changelog/2025-09-10-secret-scanning-validity-checks-available-for-data-residency)
- [Hush Security Unveils Platform to Eliminate Application Secrets](https://devops.com/hush-security-emerges-to-eliminate-need-for-application-secrets/?utm_source=rss&utm_medium=rss&utm_campaign=hush-security-emerges-to-eliminate-need-for-application-secrets)
## Security Vulnerabilities in Developer Tools and AI Coding Workflows
A vulnerability in Cursor AI lets `"runOn": "folderOpen"` tasks execute shell commands from untrusted repos due to Workspace Trust being disabled by default. Recommended practices: enable Workspace Trust, update configurations, check for auto-execution risks, and work with unfamiliar projects only in isolated environments. The incident highlights increased risk in AI-powered developer tools and the need for careful policy updating.
This maintains last week’s focus on reviewing AI and CI tool security policies as automation use increases.
- [Security Flaw in Cursor AI Coding Tool Risks Exploiting Developers](https://devops.com/oasis-security-identifies-security-weakness-in-cursor-ai-coding-tool/?utm_source=rss&utm_medium=rss&utm_campaign=oasis-security-identifies-security-weakness-in-cursor-ai-coding-tool)',
    'Security updates this week address new data access controls, developer automation, and tool vulnerabilities, with a clear focus on managed access, code and secret scanning, and evolving policy for AI-augmented coding environments.',
    1757919600, 'security', '/security/roundups/weekly-security-roundup-2025-09-15', 'TechHub',
    'TechHub', '7FBDBEE124C4A3806F45B6277BE7E852A4AF162238FF2EF4B0CBE874079E3FB6', ',Microsoft Fabric,OneLake,RBAC,Row Level Security,Column Level Security,Azure Storage,Network Security Perimeter,DevSecOps,Vulnerability Scanning,Software Supply Chain Security,npm,GitHub CodeQL,Code Scanning,Secret Scanning,SPIFFE,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-09-08
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-09-08', 'roundups', 'Weekly Security Roundup: Secrets, CI/CD Controls, and Audits',
    'Recent security news features improvements in cloud-native secrets management, pipeline controls, compliance support, and code scanning. The main focus is automating secure workflows, improving visibility, and offering developers practical tips for maintaining security standards.
<!--excerpt_end-->
## Kubernetes and Azure Cloud Security Solutions
Azure’s infrastructure security continues to get new features and certifications. The Secrets Store CSI Driver integrates with Azure Key Vault for external secrets management, supplementing earlier efforts in credential security. Best practices for securing Cloud Shell access to AKS include recommended IP allowlisting and Bastion use.
Azure Linux 3.0 has received Level 1 CIS Benchmark certification for AKS node pools, supporting baseline compliance and easier audits, picking up on recent OS security improvements.
- [Securely Managing Kubernetes Secrets with Secrets Store CSI Driver and Azure Key Vault](/azure/videos/securely-managing-kubernetes-secrets-with-secrets-store-csi-driver-and-azure-key-vault)
- [Securing Cloud Shell Access to Azure Kubernetes Service (AKS)](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/securing-cloud-shell-access-to-aks/ba-p/4450299)
- [Azure Linux 3.0 Achieves Level 1 CIS Benchmark Certification](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-3-0-achieves-level-1-cis-benchmark-certification/ba-p/4450410)
## CI/CD and Developer Workflow Security
In response to supply chain threats such as recent nx/npm attacks, a technical post discusses CI/CD defense—beyond secret scanning—covering how to restrict permissions, enforce dependency policies, and control job execution in GitHub Actions.
Another resource covers how to use BitLocker and Hyper-V on developer laptops without repeated recovery prompts, addressing recent content on runtime protection and ransomware defense.
GitHub has improved notifications on security campaigns to help teams respond to vulnerabilities, continuing efforts to automate fixes and reduce alert fatigue as seen in Copilot Autofix and campaign tools.
- [Mitigating GitHub Actions Supply Chain Attacks: Lessons from the nx Project Hack](https://jessehouwing.net/github-actions-learnings-from-the-recent-nx-hack/)
- [How to Use Hyper-V with BitLocker Without Constant Recovery Prompts](https://dellenny.com/how-to-use-hyper-v-with-bitlocker-without-constant-recovery-prompts/)
- [Improved Notifications in GitHub Security Campaigns](https://github.blog/changelog/2025-09-01-improved-notifications-in-security-campaigns)
## Compliance, Audit, and Identity Infrastructure
Enhanced Audit is now generally available for Azure Security Baseline for Linux, allowing organizations to carry out ongoing compliance assessments—continuing the move to automated auditing and policy-based security.
Azure Resource Manager will require multifactor authentication beginning October 2025, except for service principals and managed identities used in automated deployments. This change reflects last week''s emphasis on strong authentication.
A beginner’s guide to Entra ID introduces identity management as a security foundation, while technical workshops offer step-by-step advice for implementing Zero Trust with Microsoft’s framework, supporting compliance and regulatory adoption.
- [GA: Enhanced Audit in Azure Security Baseline for Linux](https://techcommunity.microsoft.com/t5/azure-governance-and-management/ga-enhanced-audit-in-azure-security-baseline-for-linux/ba-p/4446170)
- [Azure Mandatory Multifactor Authentication: Phase 2 Launches October 2025](https://azure.microsoft.com/en-us/blog/azure-mandatory-multifactor-authentication-phase-2-starting-in-october-2025/)
- [Beginners Guide to Entra ID](/security/videos/beginners-guide-to-entra-id)
- [Zero Trust Workshop: Implementing Microsoft''s Security Framework](/azure/videos/zero-trust-workshop-implementing-microsofts-security-framework)
## Code Scanning and Data Exfiltration Security in Developer Platforms
The latest CodeQL 2.22.4 increases support for Go, Rust, and Java/Kotlin and advances secure-by-default code scanning, secret scanning, and asset validation.
Microsoft Fabric now offers Workspace Outbound Access Protection (OAP) for Spark workspaces to prevent data exfiltration, improving on managed private endpoint protections and supporting ongoing work on data security.
- [CodeQL 2.22.4 Adds Go 1.25 Support and Security Enhancements](https://github.blog/changelog/2025-09-02-codeql-2-22-4-adds-support-for-go-1-25-and-accuracy-improvements)
- [Introducing Workspace Outbound Access Protection for Spark](https://blog.fabric.microsoft.com/en-US/blog/introducing-workspace-outbound-access-protection-for-spark-preview/)
## Other Security News
A newly published guide helps teams understand threat modeling for application security, offering checklists and actionable methods for every level of project maturity. This complements previous discussions about vulnerability management and secure development.
- [Understanding Threat Modeling for Application Security](/security/videos/understanding-threat-modeling-for-application-security)',
    'Recent security news features improvements in cloud-native secrets management, pipeline controls, compliance support, and code scanning. The main focus is automating secure workflows, improving visibility, and offering developers practical tips for maintaining security standards.',
    1757314800, 'security', '/security/roundups/weekly-security-roundup-2025-09-08', 'TechHub',
    'TechHub', 'C053BE64C07F45928E1B6DDC9788B98E5D7C58108542377E4B2C49045654AFEF', ',Kubernetes,AKS,Azure Key Vault,Secrets Store CSI Driver,Azure Linux 3.0,CIS Benchmark,GitHub Actions,Software Supply Chain Security,GitHub Security Campaigns,CodeQL,Secret Scanning,Azure Security Baseline For Linux,Enhanced Audit,Microsoft Entra ID,Zero Trust,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-09-01
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-09-01', 'roundups', 'Weekly Security Roundup: Supply Chain, Cloud Hardening, AI Agents',
    'Security coverage this week centers on defending the software supply chain, cloud hardening, agent identity controls, and updated developer tools for risk management. As attacks involving AI and regulatory changes become more frequent, teams face growing pressure to reinforce automated workflows and compliance.
<!--excerpt_end-->
## Modern Supply Chain Threats and the Role of AI
A new multi-stage attack targeted Nx and npm, using stolen tokens and compromised GitHub workflows to deploy malicious packages—with AI-driven reconnaissance marking the first public case of LLMs used for open source exploits. This incident affected over 5,500 repositories and triggered stronger 2FA requirements, Trusted Publisher policies, and workflow security. Upcoming EU regulations require machine-readable SBOMs and regular vulnerability disclosures by December 2027, prompting an increased focus on automating compliance checks and securing DevOps processes.
- [Malicious Nx Packages Used in Two Waves of Supply Chain Attack](https://devops.com/malicious-nx-packages-used-in-two-waves-of-supply-chain-attack/?utm_source=rss&utm_medium=rss&utm_campaign=malicious-nx-packages-used-in-two-waves-of-supply-chain-attack)
- [The EU’s Cyber Resilience Act: Redefining Secure Software Development](https://devops.com/the-eus-cyber-resilience-act-redefining-secure-software-development/?utm_source=rss&utm_medium=rss&utm_campaign=the-eus-cyber-resilience-act-redefining-secure-software-development)
- [AI Coding Assistants Bring Security and Licensing Challenges to Embedded Systems](https://devops.com/survey-surfaces-raft-of-ai-coding-issues-involving-embedded-systems/?utm_source=rss&utm_medium=rss&utm_campaign=survey-surfaces-raft-of-ai-coding-issues-involving-embedded-systems)
- [Coding at the Speed of AI: Innovation, Vulnerability, and the GenAI Paradox](https://devops.com/coding-at-the-speed-of-ai-innovation-vulnerability-and-the-genai-paradox/?utm_source=rss&utm_medium=rss&utm_campaign=coding-at-the-speed-of-ai-innovation-vulnerability-and-the-genai-paradox)
## GitHub Security Ecosystem: Releases, Secret Scanning, and Risk Assessment
GitHub previewed immutable releases with asset and tag locking, using Sigstore cryptographic attestations for asset validation. Secret scanning now adds validators for ten new types and launches a free Secret Risk Assessment tool for organizations, summarizing exposed tokens and guiding review and remediation. These tools expand coverage for credential leak detection and offer administrators new workflow protections.
- [GitHub Releases Introduce Immutability for Enhanced Supply Chain Security](https://github.blog/changelog/2025-08-26-releases-now-support-immutability-in-public-preview)
- [GitHub Secret Scanning Expands with New Validators and Validity Checks](https://github.blog/changelog/2025-08-26-secret-scanning-adds-10-new-validators-including-square-wakatime-and-yandex)
- [GitHub Introduces Free Secret Risk Assessment Tool for Organizations](https://github.blog/changelog/2025-08-26-the-secret-risk-assessment-is-generally-available)
## Cloud Infrastructure and Platform Security Enhancements
Azure improved platform security with Boost hardware isolation, integrated HSMs (FIPS 140-3), Caliptra silicon root-of-trust, and firmware Code Transparency Services. Confidential VMs and containers support compliant data-at-rest and in-use security. Microsoft’s ransomware report details hybrid attacks exploiting Entra ID and misconfigurations, with guidance for detection and cloud estate locks.
- [Protecting Azure Infrastructure: Defense-in-Depth from Silicon to Systems](https://azure.microsoft.com/en-us/blog/protecting-azure-infrastructure-from-silicon-to-systems/)
- [Storm-0501’s Evolving Cloud-Based Ransomware Tactics: Microsoft Analysis](https://www.microsoft.com/en-us/security/blog/2025/08/27/storm-0501s-evolving-techniques-lead-to-cloud-based-ransomware/)
## Securing the Next Generation: AI Agents and Cryptographic Identity
Best practices for AI agent security include using Entra Agent ID, RBAC, agent registries, and Defender/Purview analytics to manage prompt injection risk and lifecycle drift. Microsoft’s Crescent cryptographic library supports privacy-preserving digital identity using Groth16 SNARK, improving JWT and mobile credential privacy without major infrastructure changes.
- [Securing and Governing Autonomous AI Agents in the Enterprise](https://www.microsoft.com/en-us/security/blog/2025/08/26/securing-and-governing-the-rise-of-autonomous-agents/)
- [Introducing Crescent: Microsoft''s Cryptographic Library for Privacy-Preserving Digital Identity](https://www.microsoft.com/en-us/research/blog/crescent-library-brings-privacy-to-digital-identity-systems/)
## Automated Vulnerability Remediation in Microsoft DevOps Workflows
Qwiet AI expands its support for Azure DevOps, Azure Boards, and GitHub, providing SARIF static analysis, policy integration, and secret management. The AutoFix engine automates risk inspection and patching, integrating remediation directly into developer workflows.
- [Qwiet AI Expands Microsoft DevOps and GitHub Integration for Code Vulnerability Remediation](https://devops.com/qwiet-ai-extends-microsoft-support-in-platform-for-fixing-vulnerabilities/?utm_source=rss&utm_medium=rss&utm_campaign=qwiet-ai-extends-microsoft-support-in-platform-for-fixing-vulnerabilities)
## Other Security News
ASP.NET 10 APIs now return HTTP 401 Unauthorized instead of HTTP 302 for unauthenticated requests, streamlining client-side error handling per REST standards.
- [ASP.NET Community Standup: Preventing Login Redirects for APIs](/coding/videos/aspnet-community-standup-preventing-login-redirects-for-apis)
A podcast with Kat Cosgrove examines common issues with vulnerability patching, container protection, and Kubernetes hardening, recommending daily automation practices for improved resilience.
- [Digging Into Security With Kat Cosgrove](https://www.arresteddevops.com/digging-into-security/)
A step-by-step guide for healthcare data compliance in Fabric shows how to set up Microsoft Purview DLP policies to detect PHI, automate data governance, and prepare for HIPAA audits.
- [Enabling Healthcare Compliance with Microsoft Purview DLP Policies in Fabric](https://blog.fabric.microsoft.com/en-US/blog/meet-your-healthcare-regulation-and-compliance-requirements-with-purview-data-loss-prevention-dlp-policies/)
For Windows 11, ransomware protection tips cover Defender Antivirus, Controlled Folder Access, app whitelisting, and backup setup for a secure developer environment.
- [How to Enable Ransomware Protection in Windows 11](https://dellenny.com/how-to-enable-ransomware-protection-in-windows-11/)',
    'Security coverage this week centers on defending the software supply chain, cloud hardening, agent identity controls, and updated developer tools for risk management. As attacks involving AI and regulatory changes become more frequent, teams face growing pressure to reinforce automated workflows and compliance.',
    1756710000, 'security', '/security/roundups/weekly-security-roundup-2025-09-01', 'TechHub',
    'TechHub', 'EFEDA7F2266BB38C57FF041914138DC8727346570F2E8181A3BB37D6920A473B', ',Software Supply Chain Security,npm,Nx,GitHub Actions,Token Theft,Sigstore,Secret Scanning,SBOM,EU Cyber Resilience Act,Azure Confidential Computing,HSM,FIPS 140 3,Ransomware,Entra ID,AI Agent Security,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-08-25
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-08-25', 'roundups', 'Weekly Security Roundup: DevSecOps, PQC, and AI-Aware Defenses',
    'Security this week emphasizes expanded AI-driven development tools, quantum-safe cryptography integration, and DevSecOps best practices. New resources address continuous security, automation, and software supply chain integrity.
<!--excerpt_end-->
## GitHub Platform Security: Developer-first Tools and Enhanced Secret Scanning
GitHub continues to extend secret scanning and push protection. Organizations now have support for custom secret scanning patterns during push protection, allowing company-specific policies to be enforced as needed. These changes support compliance requirements and help prevent disruptions.
Integration of CodeQL and Copilot Autofix remains central, with security checks a routine part of CI. Security Campaigns and Dependency Review are more widely used to help mitigate supply chain risks as part of standard workflows.
- [Enhancing Code Security with GitHub Tools](/devops/videos/enhancing-code-security-with-github-tools)
- [GitHub Secret Scanning: Custom Pattern Configuration in Push Protection Now Available](https://github.blog/changelog/2025-08-19-secret-scanning-configuring-patterns-in-push-protection-is-now-generally-available)
## Quantum-safe Cryptography: Preparing for a Post-Quantum Security Era
Microsoft advances its Quantum Safe Program (QSP) with previews for NIST PQC algorithms, hybrid TLS 1.3, and hardware integrations. These are now available for hands-on testing on Windows and Linux systems, supporting staged planning for cryptographic updates. Microsoft’s dual strategy combines policy, education, and developer guidance to enable future cryptographic agility.
- [Quantum-safe Security: Microsoft''s Progress Toward Next-generation Cryptography](https://www.microsoft.com/en-us/security/blog/2025/08/20/quantum-safe-security-progress-towards-next-generation-cryptography/)
- [Microsoft Unveils Quantum Safe Program Strategy to Prepare for Post-Quantum Security Era](https://blogs.microsoft.com/on-the-issues/2025/08/20/post-quantum-resilience-building-secure-foundations/)
## Microsoft Defender and Security Copilot: Threat Detection, Response, and Automation
Recent updates provide analysis of new malware threats such as PipeMagic, insight into recent social engineering methods (ClickFix), and updated detection strategies. Automation tools like EDR block mode and cloud policy remediation—combined with Sentinel/Defender integrations—demonstrate a stronger focus on flexible, cross-platform security operations.
Security Copilot use expands, providing advanced identity threat protection with Azure Entra, building on improvements detailed last week in automated response and incident closure.
- [Dissecting PipeMagic: Technical Analysis of a Modular Malware Backdoor](https://www.microsoft.com/en-us/security/blog/2025/08/18/dissecting-pipemagic-inside-the-architecture-of-a-modular-backdoor-framework/)
- [Analyzing the ClickFix Social Engineering Technique and Defenses with Microsoft Security](https://www.microsoft.com/en-us/security/blog/2025/08/21/think-before-you-clickfix-analyzing-the-clickfix-social-engineering-technique/)
- [Enhancing Identity Protection with Azure Entra Security Copilot](https://techcommunity.microsoft.com/t5/azure/azure-entra-security-copilot-how-it-s-changing-identity/m-p/4447388#M22132)
## DevSecOps and Software Supply Chain: From Privacy by Design to Lifecycle Visibility
The new HoundDog.ai code scanner supports privacy-first, “shift left” governance—integrating with popular IDEs to detect shadow AI use and protect sensitive data. This parallels efforts by Microsoft and GitHub to strengthen privacy and regulatory compliance tools directly in CI/CD and editor workflows.
Content on visibility and SBOM (Software Bill of Materials) management continues to focus on transparency and regulated supply chain practices, as both threat patterns and policy requirements increase in complexity.
- [HoundDog.ai Code Scanner Shifts Data Privacy Responsibility Left](https://devops.com/hounddog-ai-code-scanner-shifts-data-privacy-responsibility-left/?utm_source=rss&utm_medium=rss&utm_campaign=hounddog-ai-code-scanner-shifts-data-privacy-responsibility-left)
- [Tackling the DevSecOps Gap in Software Understanding](https://devops.com/tackling-the-devsecops-gap-in-software-understanding/?utm_source=rss&utm_medium=rss&utm_campaign=tackling-the-devsecops-gap-in-software-understanding)
## Other Security News
Microsoft Entra ID’s Conditional Access Starter Pack offers a library of scripts and policy templates for automatic policy enforcement, supporting infrastructure-as-code management of identity controls on hybrid and on-premises systems.
A technical guide for Windows 11 details practical steps for configuring Defender for stronger threat intelligence and automation, using PowerShell scripts and custom dashboards to implement layered endpoint protection.
- [Kickstart Conditional Access in Microsoft Entra: Free Starter Pack with Policies & Automation](https://techcommunity.microsoft.com/t5/azure/kickstart-conditional-access-in-microsoft-entra-free-starter/m-p/4447413#M22136)
- [Microsoft Defender Advanced Protection Tips for Windows 11](https://dellenny.com/microsoft-defender-advanced-protection-tips-for-windows-11/)',
    'Security this week emphasizes expanded AI-driven development tools, quantum-safe cryptography integration, and DevSecOps best practices. New resources address continuous security, automation, and software supply chain integrity.',
    1756105200, 'security', '/security/roundups/weekly-security-roundup-2025-08-25', 'TechHub',
    'TechHub', 'A5FD8F775F2B11638B8C9CF8D11130FAD471EAD2563B428B21E8A25DD734E8EE', ',GitHub,GitHub Advanced Security,Secret Scanning,Push Protection,CodeQL,Copilot Autofix,Dependency Review,Software Supply Chain Security,SBOM,Post Quantum Cryptography,Hybrid TLS 1.3,Microsoft Defender,Microsoft Sentinel,Security Copilot,Azure Entra,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-08-18
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-08-18', 'roundups', 'Weekly Security Roundup: Patch Rush, Secrets, and AI Guardrails',
    'This week’s security news spotlights urgent vulnerability fixes, better credential protection, cloud and SaaS baseline upgrades, and practical integrations for identity, compliance, and recovery. Organizations must move quickly to patch risks, especially in Microsoft environments, while juggling the expanding roles—and new risks—of AI in security automation.
<!--excerpt_end-->
## Critical Vulnerability Mitigation Across Microsoft Platforms
A SharePoint vulnerability (CVE-2025-53770) allowed unauthenticated code execution through auth bypass. Recent versions received patches, while older systems relied on custom Azure WAF rules. Exchange faced a privilege escalation vulnerability (CVE-2025-53786), remedied by hotfixes and updated trust models. SQL Server saw a denial-of-service risk (CVE-2025-49759) now patched across supported releases. These events reinforce the need for prompt patching, layered defenses, and live monitoring.
- [Mitigating SharePoint CVE-2025-53770 Using Azure Web Application Firewall](https://techcommunity.microsoft.com/t5/azure-network-security-blog/protect-against-sharepoint-cve-2025-53770-with-azure-web/ba-p/4442050)
- [Mitigating CVE-2025-53786: Hybrid Exchange Server Privilege Escalation with MDVM](https://techcommunity.microsoft.com/t5/microsoft-defender-vulnerability/mdvm-guidance-for-cve-2025-53786-exchange-hybrid-privilege/ba-p/4442337)
- [Security Update Available for SQL Server 2019 RTM GDR](https://techcommunity.microsoft.com/t5/sql-server-blog/security-update-for-sql-server-2019-rtm-gdr/ba-p/4441689)
- [Security Update Available for SQL Server 2022 RTM GDR](https://techcommunity.microsoft.com/t5/sql-server-blog/security-update-for-sql-server-2022-rtm-gdr/ba-p/4441687)
- [August 2025 Exchange Server Security Updates Released](https://techcommunity.microsoft.com/t5/exchange-team-blog/released-august-2025-exchange-server-security-updates/ba-p/4441596)
## AI and Security: Expanding Applications and New Risks
AI is now being used for credential exposure alerts (Entra/AD), automated incident triage in Defender, and open-source supply chain scans (CodeQL, Copilot). However, LLM-generated code can introduce new risks. Microsoft and partners are recommending thorough review and end-to-end AI security, with organizations like Dow sharing how AI has improved threat detection and SecOps workflows.
- [How Microsoft Defender Uses AI to Detect Exposed Credentials in Identity Systems](https://techcommunity.microsoft.com/t5/microsoft-defender-xdr-blog/leaving-the-key-under-the-doormat-how-microsoft-defender-uses-ai/ba-p/4439870)
- [How Microsoft Defender Experts Uses AI to Cut Through the Noise](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/how-microsoft-defender-experts-uses-ai-to-cut-through-the-noise/ba-p/4443601)
- [Securing the Open Source Supply Chain: Impact of the GitHub Secure Open Source Fund](https://github.blog/open-source/maintainers/securing-the-supply-chain-at-scale-starting-with-71-important-open-source-projects/)
- [SonarSource Research Highlights Security Risks in LLM-Generated Code](https://devops.com/sonar-surfaces-multiple-caveats-when-relying-on-llms-to-write-code/?utm_source=rss&utm_medium=rss&utm_campaign=sonar-surfaces-multiple-caveats-when-relying-on-llms-to-write-code)
- [From Traditional Security to AI-Driven Cyber Resilience: Microsoft’s Approach to Securing AI](https://techcommunity.microsoft.com/t5/microsoft-security-community/from-traditional-security-to-ai-driven-cyber-resilience/ba-p/4442652)
- [How Dow Uses Microsoft Security Copilot and AI to Transform Cybersecurity Operations](https://www.microsoft.com/en-us/security/blog/2025/08/12/dows-125-year-legacy-innovating-with-ai-to-secure-a-long-future/)
## Advancements in Secret and Credential Management
GitHub Secret Scanning now supports 12 more token types for proactive risk detection. Secret validity checks and push protection in GitHub Advanced Security for Azure DevOps make discovery and remediation easier. Azure DevOps has improved OAuth secret management by only displaying secrets at creation. GitHub’s MCP Server now boosts public repo scanning.
- [Secret Scanning Expands Support: 12 New Token Validators Added to GitHub](https://github.blog/changelog/2025-08-12-secret-scanning-adds-12-validators-including-cockroach-labs-polar-and-yandex)
- [Secret Validity Checks Launch in GitHub Advanced Security for Azure DevOps](https://devblogs.microsoft.com/devops/hunting-living-secrets-secret-validity-checks-arrive-in-github-advanced-security-for-azure-devops/)
- [Azure DevOps Improves OAuth Client Secret Security: Secrets Now Shown Only Once](https://devblogs.microsoft.com/devops/azure-devops-oauth-client-secrets-now-shown-only-once/)
- [GitHub MCP Server Enhances Secret Scanning and Push Protection for Public Repositories](https://github.blog/changelog/2025-08-13-github-mcp-server-secret-scanning-push-protection-and-more)
## Cloud and SaaS Security Baselines, Forensic Readiness, and Integration
Azure’s updated guides on forensic readiness cover MFA, RBAC, auditing, recovery, and compliance. Third-party SaaS integration guides explain secure setup and consistent permission management for Slack, Trello, and Google. Defender for Cloud now matches compliance for US Government clouds. Queensland, Australia, has improved support for vulnerable groups with a unified MS 365 E5 stack.
- [Cloud Forensics: Implementing Security Baselines for Forensic Readiness in Microsoft Azure](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/cloud-forensics-prepare-for-the-worst-implement-security/ba-p/4440310)
- [Secure Integration of Microsoft 365 with Slack, Trello, and Google Services](https://dellenny.com/how-to-integrate-m365-with-third-party-saas-tools-slack-trello-google-services-without-breaking-security/)
- [Microsoft Defender for Cloud Expands Security and Compliance Features for U.S. Government Cloud](https://techcommunity.microsoft.com/t5/microsoft-defender-for-cloud/microsoft-defender-for-cloud-expands-u-s-gov-cloud-support-for/ba-p/4441118)
- [Queensland Government Enhances Cybersecurity for Vulnerable Communities with Microsoft 365 E5](https://news.microsoft.com/source/asia/2025/08/14/championing-safety-how-one-queensland-government-department-is-transforming-cybersecurity-to-better-support-vulnerable-communities/)
## Real-Time Enforcement and Advanced Identity Management
Continuous Access Evaluation (CAE) now provides real-time session revocation on Azure DevOps—closing security gaps faster. Developers should update workflows to react to new CAE signals. A new Entra ID guide for Windows Forms shows secure token-based identity setup for Arc-enabled SQL Server.
- [Continuous Access Evaluation (CAE) Brings Real-Time Security to Azure DevOps](https://devblogs.microsoft.com/devops/real-time-security-with-continuous-access-evaluation-cae-comes-to-azure-devops/)
- [Using Entra ID Authentication with Arc-Enabled SQL Server in a .NET Windows Forms Application](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/using-entra-id-authentication-with-arc-enabled-sql-server-in-a/ba-p/4435168)
## Application Security, Supply Chain, and Developer Workflows
A new survey shows most companies still deploy code with known vulnerabilities, putting them at risk. CodeQL now supports Kotlin and Rust and offers improved static analysis for JavaScript/React. The Minimus hardened images service adds VEX and Microsoft SSO to improve supply chain and container compliance.
- [Most Organizations Face Breaches Caused by Vulnerable Code, Survey Finds](https://devops.com/survey-traces-large-amount-of-breaches-back-to-vulnerable-code/?utm_source=rss&utm_medium=rss&utm_campaign=survey-traces-large-amount-of-breaches-back-to-vulnerable-code)
- [CodeQL Expands Support for Kotlin and Improves Static Analysis Accuracy](https://github.blog/changelog/2025-08-14-codeql-expands-kotlin-support-and-additional-accuracy-improvements)
- [Minimus Adds VEX Support and Microsoft SSO Integration to Hardened Images Service](https://devops.com/minimus-adds-vex-support-to-managed-hardened-images-service/?utm_source=rss&utm_medium=rss&utm_campaign=minimus-adds-vex-support-to-managed-hardened-images-service)
## Windows, Disk Encryption, and System Recovery
Microsoft STORM found attackers could chain four BitLocker vulnerabilities in the Windows Recovery Environment to unlock protected drives. The July 2025 patch addresses these design flaws, serving as a reminder of the importance of layered defense and ongoing validation.
- [BitUnlocker: Leveraging Windows Recovery to Extract BitLocker Secrets](https://techcommunity.microsoft.com/t5/microsoft-security-community/bitunlocker-leveraging-windows-recovery-to-extract-bitlocker/ba-p/4442806)
## Regulatory and Compliance Tools
The Eclipse Foundation has published OCCTET, a free toolkit to help organizations fulfill requirements under Europe’s Cyber Resilience Act. Microsoft Purview eDiscovery adds automated workflows, search upgrades, and audit controls. There are also plain-language guides for small businesses on labeling, DLP, and conditional access.
- [Eclipse Foundation Publishes Toolkit to Simplify CRA Compliance](https://devops.com/eclipse-foundation-publishes-toolkit-to-simplify-cra-compliance/?utm_source=rss&utm_medium=rss&utm_campaign=eclipse-foundation-publishes-toolkit-to-simplify-cra-compliance)
- [What’s New in Microsoft Purview eDiscovery](https://techcommunity.microsoft.com/t5/microsoft-security-community/what-s-new-in-microsoft-purview-ediscovery/ba-p/4441676)
- [Practical Data Protection in Microsoft 365: Sensitivity Labels, DLP, and Conditional Access for Small Businesses](https://dellenny.com/protecting-your-business-data-sensitivity-labels-dlp-and-conditional-access-explained-simply/)
## Other Security News
Malware scanning is now generally available for Azure Government Secret/Top-Secret workloads in Defender for Storage. Microsoft Teams encryption details are clarified, and S/MIME troubleshooting tackles certificate issues. There are new guides for OAuth2 automation in ADF and issuing directory extension claims in Entra ID, plus registration details for Microsoft Ignite 2025 (focused on AI defense and community forums).
- [Malware Scanning Now Available for Azure Government Secret and Top-Secret Clouds](https://techcommunity.microsoft.com/t5/microsoft-defender-for-cloud/malware-scanning-add-on-is-now-generally-available-in-azure-gov/ba-p/4442502)
- [Encryption in Microsoft Teams: How Microsoft Secures Collaboration and Communication](https://techcommunity.microsoft.com/t5/microsoft-teams-blog/encryption-in-microsoft-teams-june-2025/ba-p/4442913)
- [Troubleshooting S/MIME Setup in Exchange Online and M365: OWA and Outlook Certificate Issues](https://techcommunity.microsoft.com/t5/exchange/smime-not-working-in-owa/m-p/4443230#M16650)
- [Troubleshooting OAuth2 API Token Retrieval with ADF Web Activity](https://techcommunity.microsoft.com/t5/azure-data-factory/getting-an-oauth2-api-access-token-using-client-id-and-client/m-p/4443568#M936)
- [Issuing Custom Claims Using Directory Extension Attributes in Microsoft Entra ID](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/issuing-custom-claims-using-directory-extension-attributes-in/ba-p/4441980)
- [Connect with the Security Community at Microsoft Ignite 2025](https://www.microsoft.com/en-us/security/blog/2025/08/13/connect-with-the-security-community-at-microsoft-ignite-2025/)',
    'This week’s security news spotlights urgent vulnerability fixes, better credential protection, cloud and SaaS baseline upgrades, and practical integrations for identity, compliance, and recovery. Organizations must move quickly to patch risks, especially in Microsoft environments, while juggling the expanding roles—and new risks—of AI in security automation.',
    1755500400, 'security', '/security/roundups/weekly-security-roundup-2025-08-18', 'TechHub',
    'TechHub', 'C7F7F8507F33E5278E2D9D51F0E46942738088213FC1C3363504B0AA92DAFC0C', ',Microsoft Security,SharePoint,Exchange Server,SQL Server,CVE,Azure Web Application Firewall,Microsoft Defender,Microsoft Entra ID,Continuous Access Evaluation,GitHub Advanced Security,Secret Scanning,CodeQL,Microsoft Purview Ediscovery,BitLocker,Compliance,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-08-11
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-08-11', 'roundups', 'Weekly Security Roundup: AI Risk Triage, Identity, and Cloud Defense',
    'Security this week focused on expanding AI-powered risk management, cloud identity, operational automation, and transparent, developer-first practices.
<!--excerpt_end-->
## AI-Powered Application Security and Risk Prioritization
AI-driven tools like Cycode’s Exploitability Agent, Black Duck’s AI-powered IDE vulnerability scanning, and ArmorCode’s context-aware fixes link risk detection to business value and developer-friendly remediation. The result is a new normal for proactive, AI-augmented security operations.
- [Cycode Adds AI Agent to Assess Exploitability of Application Vulnerabilities](https://devops.com/cycode-delivers-ai-agent-to-assess-how-exploitable-vulnerabilities-are/?utm_source=rss&utm_medium=rss&utm_campaign=cycode-delivers-ai-agent-to-assess-how-exploitable-vulnerabilities-are)
## Strengthening Cloud, Hybrid, and Multicloud Security Posture
Microsoft Defender Experts now provides 24/7 cloud/on-prem monitoring with 3rd-party telemetry, improved incident correlation, and flexible pricing—unifying identity-driven defense highlighted last week.
- [Expanded Protection with Microsoft Defender Experts: Enhanced Coverage and 24/7 Threat Hunting](https://techcommunity.microsoft.com/t5/microsoft-security-experts-blog/elevate-your-protection-with-expanded-microsoft-defender-experts/ba-p/4439134)
## Advancing Identity and Access Control
Public previews for Entra Group SOA Conversion and Face Check automate policy-driven group management and high-assurance user verification, modernizing onboarding and maximizing compliance.
- [New Tools for Hybrid Access and Identity Verification in Microsoft Entra ID Governance](https://techcommunity.microsoft.com/t5/microsoft-entra-blog/new-governance-tools-for-hybrid-access-and-identity-verification/ba-p/4422534)
## AI-Driven Security Automation and SOC Operations
Microsoft’s Phishing Triage Agent, handling over 90% of user-reported emails autonomously, exemplifies generative AI’s transformative role in rapid, explainable incident response.
- [Announcing Public Preview of the Phishing Triage Agent in Microsoft Defender](https://techcommunity.microsoft.com/blog/microsoftthreatprotectionblog/announcing-public-preview-phishing-triage-agent-in-microsoft-defender/4438301)
## Data Governance and Secure AI Integration
Purview’s real-time DLP and audit for AI tools (including Copilot/Azure OpenAI/Gemini) means enterprise-sensitive data governance is now seamless, code-light, and mandatory for AI adoption.
- [AI Data Governance Made Easy: How Microsoft Purview Tackles GenAI Risks and Builds Trust](https://techcommunity.microsoft.com/t5/marketplace-blog/ai-data-governance-made-easy-how-microsoft-purview-tackles-genai/ba-p/4435237)
## Securing Developer Workflows and Supply Chains
Azure DevOps bakes in dependency scanning with Advanced Security, and endpoint protection guides for Azure Bot Service/Teams enforce JWT validation and access control, matching last week’s “secure-by-default” emphasis.
- [Automate Open-Source Dependency Scanning in Azure DevOps with Advanced Security](https://devblogs.microsoft.com/devops/automate-your-open-source-dependency-scanning-with-advanced-security/)
## Community Engagement and Proactive Security
GitHub’s Secure Code Game and Microsoft’s $17M Bounty Program incentivize responsible disclosure, while Secure Future Initiative patterns deliver actionable security playbooks for developers and teams.
- [AI Security Challenges in GitHub''s Secure Code Game Season 3](/ai/videos/ai-security-challenges-in-githubs-secure-code-game-season-3)
## Configuration, Testing, & Migration
Microsoft details best practices for Exchange Online Direct Send security, TLS 1.1 deprecation in Fabric, and JWT endpoint test automation, maintaining practical and compliant ecosystem configurations.
- [What is Direct Send and How to Secure It in Exchange Online](https://techcommunity.microsoft.com/t5/exchange-team-blog/what-is-direct-send-and-how-to-secure-it/ba-p/4439865)
## Evolution of Authentication
Passwordless sign-in and strong MFA further bridge traditional and cloud-first identity, supported by features like Cloud Kerberos Trust, supporting secure, hybrid deployments.
- [Passwordless Sign-On and MFA in Microsoft Hybrid Environments](https://www.reddit.com/r/microsoft/comments/1mjyjre/passwordless_signons_mfa_app_hybrid_mode/)
This week further cements the evolution of security as AI-powered, automation-focused, and deeply developer- and operations-integrated, shaping the future of resilient, compliant cloud and application ecosystems.',
    'Security this week focused on expanding AI-powered risk management, cloud identity, operational automation, and transparent, developer-first practices.',
    1754895600, 'security', '/security/roundups/weekly-security-roundup-2025-08-11', 'TechHub',
    'TechHub', 'BF539084D806FC4221C7864DFA583341A8F1AED0F9C2E27644D49D6618D61AD4', ',Application Security,Vulnerability Management,Exploitability Assessment,Devsecops,Software Supply Chain,Cloud Security,Hybrid Security,Multicloud,Identity And Access Management,Microsoft Entra,Microsoft Defender,Phishing Detection,Data Loss Prevention,Microsoft Purview,Passwordless Authentication,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-08-04
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-08-04', 'roundups', 'Weekly Security Roundup: Vulnerabilities, Identity, and AI Guardrails',
    'Security news this week focused on high-profile vulnerabilities, identity defense, and practical, developer-centric solutions for securing the modern stack.
<!--excerpt_end-->
## Critical Vulnerabilities and Sophisticated Threats
Microsoft uncovered macOS ‘Sploitlight’ (CVE-2025-31199), a serious bypass allowing Spotlight plugins to sidestep privacy controls and steal user data. Their analysis underscores the need to patch promptly and monitor for plugin abuse. Concurrently, Russian group Secret Blizzard was found targeting diplomats with advanced AiTM and root cert hijacks; mitigations include enhanced MFA, admin rights control, and vigilant certificate monitoring.
- [Spotlight-based macOS TCC Vulnerability CVE-2025-31199: Analysis by Microsoft Threat Intelligence](https://www.microsoft.com/en-us/security/blog/2025/07/28/sploitlight-analyzing-a-spotlight-based-macos-tcc-vulnerability/)
- [Russian Threat Actor Secret Blizzard''s AiTM Campaign Targets Diplomats with ApolloShadow Malware](https://www.microsoft.com/en-us/security/blog/2025/07/31/frozen-in-transit-secret-blizzards-aitm-campaign-against-diplomats/)
## Identity Threat Detection and Endpoint Management
Microsoft’s new Identity Threat Detection and Response platform merges identity management with security operations, enabling unified detection/response across hybrid environments and dramatically improving administrator workflow. Comprehensive walkthroughs for onboarding Defender for Endpoint cover health, registry, and log-based monitoring for robust device security.
- [Modernize Your Identity Defense with Microsoft Identity Threat Detection and Response](https://www.microsoft.com/en-us/security/blog/2025/07/31/modernize-your-identity-defense-with-microsoft-identity-threat-detection-and-response/)
- [Determine Onboarding Methods in Defender for Endpoint - Part 1](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/determine-onboarding-methods-in-defender-for-endpoint-part-1/ba-p/4437782)
## Securing the AI Lifecycle and Agent-Based Systems
AI adoption demands robust governance and compliance—practical guidance now covers full-team, policy-driven approaches for AI agents in tightly regulated environments, including data loss prevention, monitoring, and secure API surfacing. Microsoft, partners, and the community provide actionable MCP server hardening tips (OAuth 2.1, prompt injection defense) and VS Code-integration for secure agent development.
- [Mastering Agent Governance in Microsoft 365](https://techcommunity.microsoft.com/t5/healthcare-and-life-sciences/mastering-agent-governance-in-microsoft-365/ba-p/4416627)
- [MCP In Production: Building Secure and Agent-Ready Model Context Protocol Servers](/ai/videos/mcp-in-production-building-secure-and-agent-ready-model-context-protocol-servers)
- [MCP Security Best Practices](/ai/videos/mcp-security-best-practices)
## Developer Security Hygiene and Tooling
Security checks for AI model code are increasingly critical; practical sessions at Build 2025 emphasize using trusted model registries, automated scanning, and Microsoft’s Secure Future Initiative for best pipeline hygiene. Suricata and ELK showcase modern threat detection, and the new AspNetCore.SecurityKey package simplifies extensible API key authentication for ASP.NET Core.
- [Do you security check AI models you pull from online repos?: Developer Security Quick Fire Questions](/ai/videos/do-you-security-check-ai-models-you-pull-from-online-repos-developer-security-quick-fire-questions)
- [Open Source Friday with Suricata - Real-Time Threat Detection](/devops/videos/open-source-friday-with-suricata-real-time-threat-detection)
- [AspNetCore.SecurityKey: API Key Authentication for ASP.NET Core Applications](https://www.reddit.com/r/csharp/comments/1mex14a/aspnetcoresecuritykey_security_api_key/)
## Streamlined Audit Logging and Compliance
Fabric Warehouse now features a visual audit log configuration UI, moving compliance tasks away from code to a simple, unified administrative pane—reducing risks and making regulatory requirements easier to satisfy.
- [Experience the New Visual SQL Audit Logs Configuration in Fabric Warehouse](https://blog.fabric.microsoft.com/en-US/blog/experience-the-new-visual-sql-audit-logs-configuration-in-fabric-warehouse/)',
    'Security news this week focused on high-profile vulnerabilities, identity defense, and practical, developer-centric solutions for securing the modern stack.',
    1754290800, 'security', '/security/roundups/weekly-security-roundup-2025-08-04', 'TechHub',
    'TechHub', '2F38EB3D4C144048F936A831349D5D037D9EE02D7491D82BA0DABAA177599B43', ',CVE 2025 31199,Macos,Spotlight,TCC,Microsoft Threat Intelligence,Secret Blizzard,AiTM,Certificate Monitoring,Microsoft Identity Threat Detection And Response,Microsoft Defender For Endpoint,MFA,AI Agent Governance,MCP,OAuth 2.1,ASP.NET Core Security,Audit Logging,Microsoft Fabric Warehouse,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-07-28
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-07-28', 'roundups', 'Weekly Security Roundup: SharePoint Exploits and AI-Ready Defense',
    'Security headlines focused on urgent SharePoint server exploits, unified threat detection infrastructure, and technical best practices for AI and automation architectures.
<!--excerpt_end-->
## Urgent SharePoint Server Vulnerabilities and Mitigation
Microsoft warned of active state-backed attacks on on-premises SharePoint Servers (CVE-2025-53770, CVE-2025-49704), involving privilege escalation, web shells, key theft, and ransomware attempts. Immediate measures include patching, deploying Defender and AMSI, rotating keys, and monitoring for compromise—all reinforcing the ongoing need for rigorous, layered defenses.
- [Mitigating Active Exploitation of On-Premises SharePoint Vulnerabilities](https://www.microsoft.com/en-us/security/blog/2025/07/22/disrupting-active-exploitation-of-on-premises-sharepoint-vulnerabilities/)
## Unified Security Signals and AI-Driven Threat Response
Microsoft Sentinel moved into public preview as a unified security signals data lake, reducing event retention complexity and cost and enabling AI-powered threat correlation and rapid response—empowering SOCs to build scalable AI pipelines for comprehensive monitoring.
- [Microsoft Sentinel Data Lake: Unifying Security Signals and Driving AI Adoption](https://www.microsoft.com/en-us/security/blog/2025/07/22/microsoft-sentinel-data-lake-unify-signals-cut-costs-and-power-agentic-ai/)
## Secure MCP Server best practices and authorization flow
New guidelines emphasize secure, scalable MCP server designs for AI-driven workflows: adopt OAuth2.1, robust JWT validation, and cloud-native secrets management for multi-tenant, auditable security at scale. These patterns extend last week’s platform security and compliance priorities.
- [How to Build Secure and Scalable Remote MCP Servers](https://github.blog/ai-and-ml/generative-ai/how-to-build-secure-and-scalable-remote-mcp-servers/)',
    'Security headlines focused on urgent SharePoint server exploits, unified threat detection infrastructure, and technical best practices for AI and automation architectures.',
    1753686000, 'security', '/security/roundups/weekly-security-roundup-2025-07-28', 'TechHub',
    'TechHub', '734AE6E8E81A5D8E8E915E1F53D21D0006AADD7441FCA510D6BA7B5FDE5ED8F4', ',Microsoft SharePoint Server,CVE 2025 53770,CVE 2025 49704,On Premises Security,Web Shells,Privilege Escalation,Ransomware,Microsoft Defender,AMSI,Key Rotation,Incident Response,Microsoft Sentinel,Security Data Lake,Threat Correlation,OAuth 2.1,JWT Validation,Secrets Management,MCP Server,SOC,SIEM,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-07-21
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-07-21', 'roundups', 'Weekly Security Roundup: Copilot, Benchmarks, CI/CD, Endpoint',
    'Security updates this week highlighted deeper AI integration, product benchmarking, workflow threat prevention, and unified endpoint defense.
<!--excerpt_end-->
## AI-Driven Security Automation and Platform Integration
Security Copilot’s general availability in Intune and Entra automates threat detection, remediation, and incident response, merging AI with identity and compliance workflows for proactive security operations and rapid recovery. Conditional access, new natural language, and graph integrations expand transparency and customizability.
- [Security Copilot capabilities in Microsoft Intune and Entra now generally available](https://www.microsoft.com/en-us/security/blog/2025/07/14/improving-it-efficiency-with-microsoft-security-copilot-in-microsoft-intune-and-microsoft-entra/)
- [Microsoft Security Copilot Entra Update and Conditional Access Agent](/ai/videos/microsoft-security-copilot-entra-update-and-conditional-access-agent)
## Security Benchmarking and Real-Time Transparency
A new Defender for Office 365 dashboard provides customers with transparent benchmarks, competitive metrics, and quarterly updates, empowering organizations to evaluate security posture and drive continuous improvement.
- [Microsoft Defender for Office 365: Transparent Benchmarks on Email Security Effectiveness](https://www.microsoft.com/en-us/security/blog/2025/07/17/transparency-on-microsoft-defender-for-office-365-email-security-effectiveness/)
## Proactive Workflow Security and Vulnerability Detection
The prevalence of workflow injection threats in GitHub Actions is countered with CodeQL and automated scanning, along with permissions audits and action vetting to prevent CI/CD privilege escalation—key for scaling safe automation across modern developer pipelines.
- [How to Catch GitHub Actions Workflow Injections Before Attackers Do](https://github.blog/security/vulnerability-research/how-to-catch-github-actions-workflow-injections-before-attackers-do/)
## Defender for Endpoint and Unified Cyber Defense
Microsoft Defender for Endpoint, reaffirmed as a leader by Gartner and demonstrated at Black Hat USA 2025, provides AI-first, unified detection and response across cloud and hybrid environments, solidifying Microsoft’s commitment to integrated, automated enterprise defense.
- [Microsoft Named a Leader in Gartner''s 2025 Magic Quadrant for Endpoint Protection](https://www.microsoft.com/en-us/security/blog/2025/07/16/microsoft-is-named-a-leader-in-the-2025-gartner-magic-quadrant-for-endpoint-protection-platforms/)
- [Microsoft at Black Hat USA 2025: A Unified Approach to Modern Cyber Defense](https://techcommunity.microsoft.com/blog/microsoft-security-blog/%E2%80%8B%E2%80%8Bmicrosoft-at-black-hat-usa-2025-a-unified-approach-to-modern-cyber-defense%E2%80%8B%E2%80%8B/4434292)
- [Microsoft at Black Hat USA 2025: A unified approach to modern cyber defense](https://techcommunity.microsoft.com/blog/microsoft-security-blog/%E2%80%8B%E2%80%8Bmicrosoft-at-black-hat-usa-2025-a-unified-approach-to-modern-cyber-defense%E2%80%8B%E2%80%8B/4434292)',
    'Security updates this week highlighted deeper AI integration, product benchmarking, workflow threat prevention, and unified endpoint defense.',
    1753081200, 'security', '/security/roundups/weekly-security-roundup-2025-07-21', 'TechHub',
    'TechHub', 'A3813BCAFCF9A3DE985E3776725908AE5F984645FEDA398AE8EFC12923FCB07B', ',Microsoft Security Copilot,Microsoft Intune,Microsoft Entra,Conditional Access,Identity Security,Compliance,Threat Detection,Incident Response,Microsoft Defender For Office 365,Email Security,Security Benchmarking,GitHub Actions,CodeQL,CI/CD Security,Microsoft Defender For Endpoint,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-07-14
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-07-14', 'roundups', 'Weekly Security Roundup: Zero Trust for Agents and AI SOC Playbooks',
    'Security leaders are delivering holistic frameworks and technical patterns to safeguard rapidly evolving, AI-powered enterprise environments, focusing on actionable roadmaps and hands-on tools.
<!--excerpt_end-->
## Zero Trust Expansion and Multi-Agent Identity Patterns
Microsoft’s revised Zero Trust workshop now covers the full enterprise stack—network, infrastructure, SecOps—with downloadable resources, threat scenarios, and practical improvement steps. Parallel, a detailed pattern for securing multi-agent AI workflows via OAuth2, SecureFunctionTool, and Entra Agent ID ensures every agent interaction is traceable, auditable, and access-controlled using open standards.
These resources extend last week’s alerts on threat trends—emphasizing that as AI and agents proliferate, identity and access controls must keep pace.
- [Microsoft Expands Zero Trust Workshop: Network, Infrastructure, and SecOps Now Included](https://www.microsoft.com/en-us/security/blog/2025/07/09/microsoft-expands-zero-trust-workshop-to-cover-network-secops-and-more/)
- [Zero-Trust Agents: Adding Identity and Access to Multi-Agent Workflows](https://techcommunity.microsoft.com/t5/ai-azure-ai-services-blog/zero-trust-agents-adding-identity-and-access-to-multi-agent/ba-p/4427790)
## AI-Powered SOC and Security Automation
A new e-book lays out concrete steps for building an AI-powered Security Operations Center—covering threat detection, incident handling, and automation. Practitioners can jumpstart modernization with scenario-driven, actionable guidance bridging SOC gaps for today’s AI-driven threat landscape.
- [New e-book teaches how to build an AI-powered security operations center](https://www.microsoft.com/en-us/security/blog/2025/07/07/learn-how-to-build-an-ai-powered-unified-soc-in-new-microsoft-e-book/)',
    'Security leaders are delivering holistic frameworks and technical patterns to safeguard rapidly evolving, AI-powered enterprise environments, focusing on actionable roadmaps and hands-on tools.',
    1752476400, 'security', '/security/roundups/weekly-security-roundup-2025-07-14', 'TechHub',
    'TechHub', '07CF52B6FA37782882D63A500C0F7AB568C11FB79D51E2A34B43AE74A72673E9', ',Zero Trust,Microsoft Security,Identity And Access Management,OAuth 2.0,Microsoft Entra,Agent ID,Multi Agent AI,Security Operations Center,SecOps,Security Automation,Threat Detection,Incident Response,Audit Trails,Access Control,Enterprise Security,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
-- weekly-security-roundup-2025-07-07
INSERT INTO content_items (
    slug, collection_name, title, content, excerpt,
    date_epoch, primary_section_name, external_url, author,
    feed_name, content_hash, tags_csv,
    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
    is_ml, is_security, sections_bitmask
) VALUES (
    'weekly-security-roundup-2025-07-07', 'roundups', 'Weekly Security Roundup: Remote Hiring Risks and Insider Threats',
    '<!--excerpt_end-->
## Threat Trends and Workforce Security
Security threats continue to rise with remote work, as detailed by Microsoft’s research into North Korean IT worker tactics. Threat actors now exploit job platforms and social engineering to subvert global organizations, leveraging convincing candidate profiles and subtle behavioral red flags.
Practical mitigation: IT managers should strengthen hiring processes beyond routine checks, deploy monitoring for behavioral/network anomalies, and increase workforce awareness around insider risks and social threats—as distributed and hybrid teams are now a prime target for sophisticated, state-sponsored infiltration.
- [Jasper Sleet - North Korean remote IT workers’ evolving tactics to infiltrate organizations](https://www.microsoft.com/en-us/security/blog/2025/06/30/jasper-sleet-north-korean-remote-it-workers-evolving-tactics-to-infiltrate-organizations/)',
    '',
    1751871600, 'security', '/security/roundups/weekly-security-roundup-2025-07-07', 'TechHub',
    'TechHub', 'EFE74CCF013EDFD0D246B7BD2C5C32DDE0CC62038DD84E682F1BA074D9E93733', ',Cybersecurity,Threat Intelligence,Insider Risk,Remote Work Security,Social Engineering,Workforce Security,Identity Verification,Hiring Security,North Korea,State Sponsored Threats,Behavioral Analytics,Network Anomaly Detection,Security Awareness Training,Microsoft Security,Security,Roundups,',
    false, false, false, false, false,
    false, true, 64
)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    date_epoch           = EXCLUDED.date_epoch,
    primary_section_name = EXCLUDED.primary_section_name,
    external_url         = EXCLUDED.external_url,
    tags_csv             = EXCLUDED.tags_csv,
    content_hash         = EXCLUDED.content_hash,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    updated_at           = NOW();
