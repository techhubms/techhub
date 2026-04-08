---
tags:
- Agentic Workflows
- AI
- AI Operations
- Autonomous Remediation
- Azure
- Azure App Service
- Azure Container Apps
- Azure SRE Agent
- Community
- DevOps
- Evaluation And Feedback Loops
- Governance Guardrails
- Human in The Loop
- Incident Management
- Infrastructure as Code (iac)
- Logs And Metrics Correlation
- Model Context Protocol (mcp)
- Observability
- On Call Toil Reduction
- Release Best Practices
- Role Based Access Control (rbac)
- Root Cause Analysis (rca)
- SDLC Automation
- Security
- Shift Left Security
- Site Reliability Engineering (sre)
- Telemetry
title: How we build and use Azure SRE Agent with agentic workflows
primary_section: ai
feed_name: Microsoft Tech Community
author: Shamir_AbdulAziz
section_names:
- ai
- azure
- devops
- security
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-we-build-and-use-azure-sre-agent-with-agentic-workflows/ba-p/4508753
date: 2026-04-08 16:00:40 +00:00
---

Shamir_AbdulAziz describes how Microsoft built Azure SRE Agent—an AI-powered ops agent—using “agentic workflows” across the SDLC, with human-in-the-loop governance, RBAC guardrails, and deep integration into telemetry and incident systems to reduce on-call toil and speed up incident mitigation.<!--excerpt_end-->

# How we build and use Azure SRE Agent with agentic workflows

Microsoft’s Customer Zero blog series shares how Microsoft builds and operates its own systems at scale, with lessons and operational patterns for building, operating, and scaling AI apps and agent fleets.

## The challenge: Ops is critical but takes time from innovation

Microsoft runs always-on production systems at very large scale (thousands of services, millions of deployments, constant change) with a low tolerance for downtime.

Common sources of operational toil:

- Engineers pulled away from feature work to:
  - Diagnose alerts
  - Sift through logs
  - Correlate metrics across systems
  - Respond to incidents during on-call
- Manual investigations can slow teams down and contribute to burnout.

Additional pressure in the AI era:

- Increased need for operational excellence as shipping velocity rises.
- Fast-moving AI landscape (new models and tools) fragments the ecosystem across:
  - Observability
  - DevOps
  - Incident management
  - Security

The goal was a different approach to operations that reduces toil and accelerates response without losing control.

## The solution: Azure SRE Agent built with agentic workflows

Microsoft built **Azure SRE Agent**, an AI-powered operations agent intended to act as an always-on SRE partner.

What it does:

- Continuously observes production environments to detect and investigate incidents.
- Reasons across signals such as:
  - Logs
  - Metrics
  - Code changes
  - Deployment records
- Performs root cause analysis (RCA).
- Supports engineers from triage to resolution.
- Operates at multiple autonomy levels:
  - Assistive investigation
  - Automating remediation proposals
  - Resolving some issues autonomously

Governance and safety:

- Operates within governance guardrails.
- Uses human approval checks.
- Grounded in role-based access controls (RBAC) and defined escalation paths.

Learning loop:

- Learns from past incidents, outcomes, and human feedback to improve over time.

### “Agentic workflows”: building agents with agents across the SDLC

Rather than treating AI as a bolt-on tool, Microsoft embedded specialized agents across the software development lifecycle (SDLC).

Stages described:

- **Plan & Code**
  - Supports spec-driven development and faster inner-loop cycles.
  - Can draft spec documentation that defines feature requirements.
  - Can create prototypes and check in code to staging to enable rapid iteration.
- **Verify, Test & Deploy**
  - Agents for code quality review, security, evaluation, and deployment.
  - Shifts left on quality and security issues.
  - Continuously assesses reliability, performance, and release best practices.
- **Operate & Optimize**
  - Azure SRE Agent investigates alerts, assists remediation, and can resolve some issues autonomously.
  - Uses a feedback loop approach (including a specialized instance of Azure SRE Agent helping maintain Azure SRE Agent).

### Integration approach

Azure SRE Agent was designed to integrate across existing systems rather than replacing them, including:

- Custom agents
- **Model Context Protocol (MCP)**
- Python tools
- Telemetry connections
- Incident management platforms
- Code repositories
- Knowledge sources
- Business process and operational tools

Humans remain in the loop for oversight, approvals, and decisions where required.

## The impact: Reducing toil at enterprise scale

Reported internal outcomes over nine months:

- **35,000+ incidents** handled autonomously by Azure SRE Agent
- **50,000+ developer hours** saved by reducing manual investigation/response work
- Reduced on-call burden and faster time-to-mitigation during incidents

Examples mentioned:

- **Azure Container Apps**
  - **89%** “overwhelmingly positive” responses to RCA results
  - RCA coverage across **90%+ of incidents**
- **Azure App Service**
  - Time-to-mitigation for live-site incidents reduced to **3 minutes** (from **40.5 hours** average with human-only activity)

A developer quote highlights practical value in faster RCA and reducing time spent exploring incident possibilities (e.g., quota requests, CRIs).

## Key learnings: What made the workflow effective

Lessons emphasized:

- Agents aren’t just advanced automation; they’re collaborative tools for investigations.
- Prompting the agent can surface relevant context (logs, metrics, related code changes) and speed up troubleshooting.
- Agents can be extended for data analysis and dashboarding.
- **Building agents with agents** helped remove manual development bottlenecks by accelerating:
  - Code generation
  - Review
  - Debugging
  - Security fixes
- A **generic agent** with rich context plus memory/learning can adapt over time and avoid relearning.
- **Specialized agents** help consistency and repeatability for well-defined incident categories.
- Deep integration into existing telemetry/workflows/platforms worked better than trying to replace them.
- **Human-in-the-loop governance** is critical (clear approval boundaries, RBAC, safety checks).
- Continuous feedback and evaluation helps improve agents and clarify where automation vs human judgment fits best.

## Want to learn more

- Discover more about Azure SRE Agent: https://aka.ms/SREagent
- Learn about agents in DevOps tools and processes: https://azure.microsoft.com/solutions/devops
- Read best practices on agent management with Azure: https://learn.microsoft.com/azure/cloud-adoption-framework/ai-agents/integrate-manage-operate


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-we-build-and-use-azure-sre-agent-with-agentic-workflows/ba-p/4508753)

