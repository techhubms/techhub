---
section_names:
- devops
- security
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/24/detecting-investigating-defending-against-trivy-supply-chain-compromise/
title: Guidance for detecting, investigating, and defending against the Trivy supply chain compromise
date: 2026-03-25 00:03:03 +00:00
author: Microsoft Defender Security Research Team
tags:
- Action Pinning
- Advanced Hunting
- AES 256 CBC
- CI/CD Security
- Command And Control
- Commit SHA Pinning
- Credential Theft
- DevOps
- EC2 Instance Metadata
- ECS Task Metadata
- GitHub Actions
- GitHub TOKEN Permissions
- IMDS
- Kubernetes Secrets
- Kusto Query Language
- Least Privilege
- Microsoft Defender For Cloud
- Microsoft Defender For Endpoint
- Microsoft Defender For Identity
- Microsoft Defender XDR
- Microsoft Threat Intelligence
- Mutable Git Tags
- News
- RSA Encryption
- Secrets Exfiltration
- Security
- Self Hosted Runners
- Software Supply Chain Security
- Tag Force Push
- Trivy
- Typosquatting
feed_name: Microsoft Security Blog
primary_section: devops
---

The Microsoft Defender Security Research Team breaks down the Trivy supply-chain compromise affecting GitHub Actions and official binaries, explains how credentials were harvested from CI/CD runners, and provides concrete Microsoft Defender detections plus hardening steps (like pinning actions to commit SHAs) to reduce repeat incidents.<!--excerpt_end-->

# Guidance for detecting, investigating, and defending against the Trivy supply chain compromise

On March 19, 2026, **Trivy** (Aqua Security’s open-source vulnerability scanner) was reported compromised in a **CI/CD-focused supply chain attack**. Threat actors used previously established access (from an earlier incident that wasn’t fully remediated) to inject **credential-stealing malware** into multiple trusted distribution paths:

- The **Trivy scanner binary** (malicious release: **v0.69.4**)
- The **aquasecurity/trivy-action** GitHub Action
- The **aquasecurity/setup-trivy** GitHub Action

The campaign is attributed to a threat actor calling itself **TeamPCP**. Microsoft Defender observed and provides detection, investigation, and mitigation guidance.

This activity later expanded to other projects/frameworks (mentioned as **Checkmarx KICS** and **LiteLLM**) while the investigation continued.

## Analyzing the Trivy supply chain compromise

The March 19 activity represents the execution phase where established access was used to weaponize trusted channels.

### What was compromised

- **Poisoned GitHub Actions used in pipelines**
  - Using compromised credentials with **tag write access**, the attacker:
    - Force-pushed **76 of 77 version tags** in `aquasecurity/trivy-action`
    - Force-pushed **all 7 tags** in `aquasecurity/setup-trivy`
  - Effect: downstream workflows referencing these actions by tag executed attacker-controlled code **without visible release metadata changes**.

- **Published a malicious Trivy binary**
  - Release automation was triggered to publish an infected **Trivy v0.69.4** binary to official channels (GitHub Releases and container registries).

- **Stealth and impact window**
  - The compromised actions and binary ran **credential-harvesting** while still executing the legitimate Trivy functionality so pipelines/scans looked successful.

- **Containment**
  - Trivy maintainers later removed malicious artifacts from distribution channels the same day.

## How GitHub’s design was abused

This attack exploited two Git/GitHub-by-design properties:

- **Mutable tags**: tags can be reassigned by anyone with push access. The attacker re-pointed tags to malicious commits so existing workflow references ran new code.
- **Self-declared commit identity**: the actor spoofed commit identity, similar to impersonation tactics referenced in Microsoft’s earlier “Shai-Hulud 2.0” supply-chain campaign write-up.

## Exploitation details (what the malware did)

Microsoft Defender for Cloud observed the full chain on compromised **self-hosted GitHub Actions runners**.

### Execution flow

1. **Process discovery** to locate runner processes (`Runner.Worker`, `Runner.Listener`) and identify processes carrying secrets.
2. **Base64-encoded Python payload** was decoded and executed.
3. **Host fingerprinting**:
   - `hostname`, `whoami`, `uname -a`, `ip addr`
4. **Dumped environment variables** via `printenv`.
5. **Broad credential harvesting**:
   - **AWS**
     - `grep AWS_`
     - Queried ECS metadata endpoint `169.254.170.2`
     - Queried EC2 IMDS `169.254.169.254`
   - **GCP**
     - `grep -i google`, `grep -i gcloud`
     - Attempted to read `$GOOGLE_APPLICATION_CREDENTIALS`
   - **Azure**
     - `grep -i azure`
   - **Kubernetes secrets**
     - Enumerated mounted service-account files under `/run/secrets/kubernetes.io/serviceaccount/`
     - Attempted: `kubectl get secrets --all-namespaces -o json`
   - **CI/CD and app secrets**
     - Read runner internal environment files
     - Recursive searches for keys/tokens in `.env`, `.json`, `.yml`, `.yaml`
     - Collected Slack/Discord webhook URLs
   - **Infrastructure and access**
     - WireGuard configs: `wg showconf all`
     - SSH logs: `/var/log/auth.log`, `/var/log/secure`
     - Database connection strings: MySQL, PostgreSQL, MongoDB, Redis, Vault
   - **Cryptocurrency**
     - Searched for Solana wallet variables and RPC credentials

6. **Packaging + encryption + exfiltration**
   - Data encrypted with **hybrid AES-256-CBC + RSA**.
   - Bundled into `tpcp.tar.gz`.
   - Exfiltrated via HTTP POST to typosquatted domain: `scan.aquasecurtiy[.]org`.

7. **Cleanup + “normal” execution**
   - Temporary files removed.
   - Legitimate Trivy scan executed so the workflow appeared successful.

## Detection and investigation

### Defender coverage noted in the post

Microsoft Defender XDR customers can use detections mapped to observed activity, including:

- **Credential access**
  - IMDS access to steal cloud tokens
  - Container runner secret reconnaissance
- **Command and Control**
  - Suspicious DNS queries (including `scan[.]aquasecurtiy[.]org`)
- **Exfiltration**
  - Exfiltration activity from infected Trivy version (including curl-based behaviors)

Products referenced for coverage include:

- Microsoft Defender for Cloud
- Microsoft Defender for Endpoint
- Microsoft Defender for Identity
- Microsoft Defender XDR

The post also notes that customers can use **Microsoft Security Copilot in Microsoft Defender** to investigate/respond.

## Mitigation and protection guidance

### Immediately update to safe versions

Ensure workflows and builds use verified safe versions:

| Component | Safe version |
| --- | --- |
| Trivy binary | v0.69.2 – v0.69.3 |
| trivy-action | v0.35.0 |
| setup-trivy | v0.2.6 |

### Harden CI/CD pipelines against supply chain attacks

Pin third-party actions to immutable references:

- Pin GitHub Actions to **commit SHA** (not tags like `@v1`).
- Audit workflows for tag-based references and replace with verified SHAs.

Restrict action usage with policy controls:

- Use org-level policies to allow only approved actions.
- Block unverified/new external actions by default.

### Enforce least privilege and strong identity controls

- Minimize `GITHUB_TOKEN` and other credential permissions.
- Avoid write permissions unless strictly necessary.

### Protect secrets and sensitive data in pipelines

- Avoid injecting secrets into environment variables when not required.
- Use a dedicated secret manager and fetch secrets just-in-time.
- Prevent credential persistence on runners:
  - Don’t persist credentials to disk.
  - Prefer **ephemeral runners** or clean environments between jobs.

### Reduce lateral movement risk with Attack Path analysis

The post recommends **attack path analysis in Microsoft Defender** to identify and remediate risky relationships between identities, secrets, misconfigurations, and resources:

- Documentation: https://learn.microsoft.com/en-us/security-exposure-management/work-attack-paths-overview

### Assess blast radius using Advanced Hunting

Use the Exposure Management graph via **Advanced Hunting** to assess reachable assets from a compromised node (like a leaked CI/CD secret). An additional reference is provided:

- https://techcommunity.microsoft.com/blog/microsoft-security-blog/microsoft-security-exposure-management-graph-unveiling-the-power/4148546

## Advanced hunting queries

### CloudProcessEvents: malicious commands associated with TeamPCP indicators

```kusto
CloudProcessEvents
| where ProcessCommandLine has_any (
  'scan.aquasecurtiy.org',
  '45.148.10.212',
  'plug-tab-protective-relay.trycloudflare.com',
  'tdtqy-oyaaa-aaaae-af2dq-cai.raw.icp0.io',
  'checkmarx.zone',
  '/tmp/runner_collected_',
  'tpcp.tar.gz'
) or (ParentProcessName == 'entrypoint.sh' and ProcessCommandLine has 'grep -qiE (env|ssh)')
```

### Kubernetes secrets enumeration

```kusto
DeviceProcessEvents
| where FileName == "bash"
| where InitiatingProcessFileName != "claude"
| where InitiatingProcessParentFileName != "claude"
| where ProcessCommandLine !contains "claude"
| where ProcessCommandLine has_all ("kubectl get secrets ", " --all-namespaces ", " -o json ", " || true")
```

### Google Cloud credential enumeration

```kusto
DeviceProcessEvents
| where FileName == 'dash'
| where InitiatingProcessCommandLine == 'python3'
| where ProcessCommandLine has_all ('$GOOGLE_APPLICATION_CREDENTIALS', ‘cat’, '2>/dev/null')
```

### Exfiltration via curl from a Trivy process

```kusto
DeviceProcessEvents
| where FileName == "curl"
| where InitiatingProcessCommandLine contains "trivy-action"
| where ProcessCommandLine contains " POST "
| where ProcessCommandLine contains " --data-binary"
```

### Typosquatted C2 domain in command line

```kusto
CloudProcessEvents
| where ProcessCommandLine has_any (
  // Typosquatted C2 domain
  "scan.aquasecurtiy.org",
  "aquasecurtiy.org",
  // C2 IP
  "45.148.10.212"
)
| project Timestamp, KubernetesPodName, KubernetesNamespace, AzureResourceId, ContainerName, ContainerId, ContainerImageName, ProcessName, ProcessCommandLine, ParentProcessName, FileName
```

### OpenSSL-based encryption operations

```kusto
CloudProcessEvents
| where ProcessName == "openssl" and ProcessCommandLine has_any ("enc -aes-256-cbc", "enc -aes-256") and ProcessCommandLine has "-pass file:"
| project Timestamp, KubernetesPodName, KubernetesNamespace, AzureResourceId, ContainerName, ContainerId, ContainerImageName, ProcessName, ProcessCommandLine, ParentProcessName, FileName

DeviceProcessEvents
| where ProcessCommandLine has_all ('/dev/null', '--data-binary', '-X POST', 'scan.aquasecurtiy.org ')
  or ProcessCommandLine has_any ('pgrep -f Runner.Listener', 'pgrep -f Runner.Worker')
  or ProcessCommandLine has_any ('tmp/runner_collected_', 'tpcp.tar.gz')
  and ProcessCommandLine has_any ('curl', 'tar', 'rm', 'openssl enc')
  and ProcessCommandLine !has 'find'
  or InitiatingProcessCommandLine contains '/entrypoint.sh’
  and ProcessCommandLine has ‘grep -qiE (env|ssh)’
| join kind=leftouter (DeviceNetworkEvents | where RemoteIP == '45.148.10.122') on DeviceId
| project Timestamp, FileName, ProcessCommandLine, InitiatingProcessCommandLine, InitiatingProcessFolderPath, RemoteIP
```

### Compromised installations of Trivy

```kusto
DeviceTvmSoftwareInventory
| where SoftwareName has "trivy"
| where SoftwareVersion has_any ("0.69.4", "0.69.5", "0.69.6")
```

## References

- Trivy Compromised a Second Time – Malicious v0.69.4 Release (Step Security): https://www.stepsecurity.io/blog/trivy-compromised-a-second-time---malicious-v0-69-4-release
- Aqua update: https://www.aquasec.com/blog/trivy-supply-chain-attack-what-you-need-to-know/

Research credit: Microsoft Defender Security Research with contributions from Yossi Weizman, Tushar Mudi, Kajhon Soyini, Mohan Bojjireddy, Gourav Khandelwal, Sai Chakri Kandalai, Mathieu Letourneau, Ram Pliskin, and Ivan Macalintal.

## Learn more

- Protect your agents in real-time during runtime (Preview) – Microsoft Defender for Cloud Apps: https://learn.microsoft.com/en-us/defender-cloud-apps/real-time-agent-protection-during-runtime
- Microsoft 365 Copilot AI security documentation: https://learn.microsoft.com/en-us/copilot/microsoft-365/microsoft-365-copilot-ai-security
- How Microsoft discovers and mitigates evolving attacks against AI guardrails: https://www.microsoft.com/en-us/security/blog/2024/04/11/how-microsoft-discovers-and-mitigates-evolving-attacks-against-ai-guardrails/
- Securing Copilot Studio agents with Microsoft Defender: https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-protection

Note: The source text also links to “how to build and customize agents with Copilot Studio Agent Builder”, but the provided URL appears malformed in the input, so it’s omitted here to avoid an invalid link.

[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/24/detecting-investigating-defending-against-trivy-supply-chain-compromise/)

