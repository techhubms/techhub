---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/reactive-incident-response-with-azure-sre-agent-from-alert-to/ba-p/4492938
title: 'Reactive Incident Response with Azure SRE Agent: From Alert to Resolution in Minutes'
author: Sabyasachi-Samaddar
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-19 06:44:39 +00:00
tags:
- AI
- AI Operations
- Alerting
- Application Insights
- ARM API
- Azure
- Azure Monitor
- Azure SRE Agent
- Cloud Automation
- Community
- Custom Runbooks
- DevOps
- Incident Response
- Incident Response Plan
- Log Analytics
- Metrics
- MTTR
- PowerShell
- RBAC
- Run Command
- Security
- Security Best Practices
- Session Insights
- SQL Database
- Virtual Machines
section_names:
- ai
- azure
- devops
- security
---
Sabyasachi-Samaddar showcases how Azure SRE Agent leverages AI and automation to enable faster, more consistent incident response and recovery for Azure environments, using practical scenarios and technical walkthroughs.<!--excerpt_end-->

# Reactive Incident Response with Azure SRE Agent: From Alert to Resolution in Minutes

**Author: Sabyasachi-Samaddar**

## When things break at 2 AM, your AI teammate is already investigating

This article walks through how Azure SRE Agent automates incident response, cuts Mean Time to Recovery (MTTR), and captures institutional knowledge through two real-world incidents—a SQL connectivity outage and a VM high CPU spike.

---

### The Reactive Incident Challenge

- **Traditional flow:** Alerts land in Teams/PagerDuty, the on-call wakes up and starts context gathering. Investigation and resolution are manual, slow, and inconsistent.
- **SRE Agent flow:** AI acknowledges, investigates, gathers context, identifies root cause, and proposes remediation before the human arrives.

#### Traditional vs SRE Agent-driven Response

| Step                 | Traditional (Time)    | SRE Agent (Time)   |
|----------------------|----------------------|--------------------|
| Alert fires          | 0                    | 0                  |
| Human acknowledges   | 5-15 min             | — (instant AI)     |
| Manual investigation | 20–45 min            | 2-10 min (AI)      |
| Resolution           | 30–60 min total      | 10–15 min total    |

---

## Real-World Incident 1: Azure SQL Database Connectivity Outage

**Alert:**

- Severity: Sev1; Health check failing on backend
- Triggered by Azure Monitor metric alert
- Configured using ARM/Bicep (metricAlerts)
- Resource: `sre-demo-webapp-health-alert` in region `centralindia`

**AI (SRE Agent) Investigation Flow:**

1. **Symptom Assessment:**
   - Pulled ARM settings from web app (identity, plan) and analyzed HTTP 5xx using Application Insights with KQL queries.
2. **Dependency Mapping:**
   - Identified 100% failures in SQL dependencies via Application Insights; failures on database.windows.net operations with 503 errors.
3. **Network Validation:**
   - DNS and TCP port 1433 checked directly from App Service, confirming networking layer was healthy.
4. **Configuration Analysis:**
   - Used `az sql server show` to reveal public network access disabled on the SQL Server.

**Root Cause:**

- Azure SQL public network access = Disabled. Web app has neither VNet integration nor private endpoint; can't connect to backend database.

**Remediation Options:**

- **Option A:** Enable public access + Allow Azure Services (quick restore; less secure).
- **Option B:** Add specific outbound IPs to SQL firewall.
- **Option C:** Configure Private Endpoint + VNet integration (production-ready).

**Automated Remediation Steps (with approval):**

- `az sql server update ... --set publicNetworkAccess=Enabled`
- `az sql server firewall-rule create ... --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0`
- Security advisories provided for production.

**Recovery Verified:**

- Post-remediation, SRE Agent checked Application Insights: Dependencies (65/65) succeeded, HTTP 5xx errors dropped to zero.

**Total Time to Resolution:** ~20 minutes (including human approval delay)

---

## Real-World Incident 2: VM High CPU Spike

**Alert:**

- Severity: Sev2; CPU > 85% for over 5 minutes
- Configured as metric alert on VM in resource group `rg-sre-demo-india`

**AI (SRE Agent) Investigation Flow:**

1. **Process Capture:**
   - Executed via `az vm run-command invoke` to capture current top CPU processes using PowerShell (`Get-Process | Sort-Object CPU`...)
2. **Runaway Process Identification:**
   - Custom incident plan: If powershell process has >80s CPU, likely a stress or runaway job.
   - Identified PIDs (3164/2776) were consuming >650s each.
3. **Targeted Remediation (with approval):**
   - Safely terminated the suspected runaway processes.
4. **Verification:**
   - Reran diagnostic to confirm top CPU consumers were now normal processes (MsMpEng, svchost).

**Knowledge Captured:**

- Lessons learned, best practices, and troubleshooting patterns were auto-documented as Session Insights.
- Example: Use supported time grains on metric queries; avoid complex PowerShell parsing.

**Total Time to Resolution:** ~39 minutes (32 minutes was human approval wait)

---

## Architecture and Integration Points

| Component            | Purpose                                  |
|----------------------|------------------------------------------|
| Azure Monitor        | Alerts for anomalies                      |
| Application Insights | Request & dependency tracking; KQL queries|
| Log Analytics        | Central log/perf analysis (KQL-queryable) |
| VM Run Command       | Executes remote diagnostics via Azure CLI |
| ARM API              | Resource configuration assessment         |

---

## How to Setup and Run the Demo

1. **Provision Lab:** Deploy sample infra via PowerShell scripts; includes SQL, Web App, VM, Application Insights, Log Analytics. Clone from [GitHub](https://github.com/Saby007/SREAgentDemo.git).
2. **Enable SRE Agent:** Use the [Azure SRE Agent Portal](https://aka.ms/sreagent/portal), grant contributor RBAC, region must be East US 2 for preview.
3. **Customize Response Plans:** Add targeted instructions (criteria for identifying critical processes, safe remediations, escalation triggers).
4. **Trigger incidents:** Intentionally fail SQL connectivity and cause VM CPU spikes via scripts.
5. **Observe AI workflow:** Alerts, autonomous investigation, human approval for remediation, recovery verification, and auto-generated documentation.
6. **Clean-up:** Remove resource group or use provided clean-up scripts.

---

## Key Benefits

- **Faster MTTR:** AI begins investigating instantly.
- **Consistent Triage:** Repeatable, structured process.
- **Knowledge Capture:** Incident learnings codified as Session Insights.
- **Reduced Toil:** Automated context gathering, query, and troubleshooting.
- **Security Guardrails:** Explicit approval for remediation (autonomous mode optional).

---

## Best Practices

**Do:**

- Write explicit IRP (Incident Response Plan) instructions
- Include clear identification and escalation criteria
- Validate metrics time grain configuration
- Start in review/approval mode before enabling autonomous remediation

**Don't:**

- Over-provision broad IAM permissions
- Use complex, hard-to-debug PowerShell in automation
- Skip validation of recovery after resolution
- Use `Remove-Job`; stick with `Stop-Job`

---

## Next Steps & Future Enhancements

- Enable autonomous mode for battle-tested scenarios
- Add more incident types and automation triggers
- Integrate with Teams, GitHub Issues, and 3rd party observability tools
- Expand Session Insights and knowledge base with runbooks and architectural learnings

---

## Conclusion

Azure SRE Agent brings proactive, AI-powered incident investigation and remediation to Azure environments, capturing and reusing knowledge to continuously improve operations. While not perfect, it significantly shortens incident response times and makes learnings repeatable for the team.

**Learn More:**

- [Azure SRE Agent Documentation](https://aka.ms/sreagent/docs)
- [Azure SRE Agent Blogs](http://aka.ms/sreagent/blogs)
- [Azure SRE Agent Community](https://aka.ms/sreagent/discussions)
- [Azure SRE Agent Pricing](http://aka.ms/sreagent/pricing)

_Azure SRE Agent is currently in preview. Last updated: Feb 18, 2026._

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/reactive-incident-response-with-azure-sre-agent-from-alert-to/ba-p/4492938)
