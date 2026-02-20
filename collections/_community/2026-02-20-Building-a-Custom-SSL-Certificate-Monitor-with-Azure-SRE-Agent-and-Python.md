---
layout: "post"
title: "Building a Custom SSL Certificate Monitor with Azure SRE Agent and Python"
description: "This tutorial guides you through creating a custom Python tool in Azure SRE Agent for monitoring SSL certificate expirations. You'll build the CheckSSLCertificateExpiry tool, wrap it into a reusable skill for comprehensive certificate auditing across multiple domains, and deploy it as a production-ready solution. By the end, you’ll have a proactive monitoring workflow that can prevent costly outages caused by expired SSL certificates."
author: "dbandaru"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-a-custom-ssl-certificate-monitor-with-azure-sre-agent-from/ba-p/4495832"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-20 00:47:40 +00:00
permalink: "/2026-02-20-Building-a-Custom-SSL-Certificate-Monitor-with-Azure-SRE-Agent-and-Python.html"
categories: ["Azure", "Coding", "DevOps", "Security"]
tags: ["Azure", "Azure Automation", "Azure SRE Agent", "Certificate Expiry", "Cloud Monitoring", "Coding", "Community", "Custom Agent", "DevOps", "DevOps Automation", "Incident Response", "ITOps", "Python", "Python Tool", "Risk Classification", "Security", "Security Operations", "SSL Certificate", "SSL Monitoring", "TLS"]
tags_normalized: ["azure", "azure automation", "azure sre agent", "certificate expiry", "cloud monitoring", "coding", "community", "custom agent", "devops", "devops automation", "incident response", "itops", "python", "python tool", "risk classification", "security", "security operations", "ssl certificate", "ssl monitoring", "tls"]
---

dbandaru demonstrates how to develop and deploy a Python-based SSL certificate monitoring tool within Azure SRE Agent, empowering teams to automate certificate health audits and avert service disruptions due to expired certificates.<!--excerpt_end-->

# Building a Custom SSL Certificate Monitor with Azure SRE Agent and Python

Expired SSL certificates are a common source of preventable outages. In this tutorial, dbandaru walks you through building a monitoring solution using Azure SRE Agent, Python, and skills methodology to integrate SSL certificate expiry checks directly into your incident response and health audit workflows.

## Why Monitor SSL Certificates Proactively?

Certificate expirations often lead to costly, high-impact outages. Traditional tracking (spreadsheets, calendar reminders, or separate SaaS tools) is prone to human error and lacks integration with real-time monitoring and incident workflows. Azure SRE Agent lets you automate this, detecting expiring or expired certificates before they impact your services.

## Solution Overview

You'll create:

1. **CheckSSLCertificateExpiry Python Tool**: Connects to any domain, retrieves SSL/TLS certificate details, and provides structured info about validity, issuer, and expiration.
2. **ssl_certificate_audit Skill**: Wraps the tool into a comprehensive audit process that checks multiple domains, classifies risk, and recommends actions.
3. **SSLCertificateMonitor Agent**: Ties the tool and skill together for hands-free, proactive monitoring.

## Step 1: Create the Python Tool in Azure SRE Agent

### Portal Steps

- Navigate to Settings > Subagent Builder, select "Create New Tool."
- Choose "Python Tool". Name it `CheckSSLCertificateExpiry`.
- Description: "Checks SSL/TLS certificate expiry for a domain, returns days until expiration, issuer, and validity dates."
- Parameters:
  - `domain` (string, required): e.g., api.contoso.com
  - `port` (string, optional): defaults to 443

### Example Implementation

```python
import ssl
import socket
import json
from datetime import datetime, timezone

def main(domain, port="443"):
    port = int(port)
    context = ssl.create_default_context()
    try:
        with socket.create_connection((domain, port), timeout=10) as sock:
            with context.wrap_socket(sock, server_hostname=domain) as ssock:
                cert = ssock.getpeercert()
                not_before = datetime.strptime(cert["notBefore"], "%b %d %H:%M:%S %Y %Z").replace(tzinfo=timezone.utc)
                not_after = datetime.strptime(cert["notAfter"], "%b %d %H:%M:%S %Y %Z").replace(tzinfo=timezone.utc)
                now = datetime.now(timezone.utc)
                days_remaining = (not_after - now).days
                issuer = dict(x[0] for x in cert.get("issuer", []))
                subject = dict(x[0] for x in cert.get("subject", []))
                if days_remaining < 0:
                    risk_level = "EXPIRED"
                elif days_remaining <= 7:
                    risk_level = "CRITICAL"
                elif days_remaining <= 30:
                    risk_level = "WARNING"
                elif days_remaining <= 60:
                    risk_level = "ATTENTION"
                else:
                    risk_level = "HEALTHY"
                san_list = [v for t, v in cert.get("subjectAltName", []) if t == "DNS"]
                return {
                    "domain": domain,
                    "port": port,
                    "status": "valid" if days_remaining >= 0 else "expired",
                    "risk_level": risk_level,
                    "days_remaining": days_remaining,
                    "not_before": not_before.isoformat(),
                    "not_after": not_after.isoformat(),
                    "issuer": issuer.get("organizationName", "Unknown"),
                    "issuer_cn": issuer.get("commonName", "Unknown"),
                    "subject_cn": subject.get("commonName", domain),
                    "serial_number": cert.get("serialNumber", "Unknown"),
                    "version": cert.get("version", "Unknown"),
                    "san_count": len(san_list),
                    "san_domains": san_list[:10],
                    "checked_at": now.isoformat()
                }
    except ssl.SSLCertVerificationError as e:
        return {
            "domain": domain,
            "port": port,
            "status": "verification_failed",
            "risk_level": "CRITICAL",
            "error": str(e),
            "checked_at": datetime.now(timezone.utc).isoformat()
        }
    except (socket.timeout, socket.gaierror, ConnectionRefusedError, OSError) as e:
        return {
            "domain": domain,
            "port": port,
            "status": "connection_failed",
            "risk_level": "UNKNOWN",
            "error": str(e),
            "checked_at": datetime.now(timezone.utc).isoformat()
        }
```

**Design Choices:**

- Uses only Python standard lib for speed and reliability
- Returns all critical certificate details in a JSON object, with risk classification
- Structured error handling for robust automation

## Step 2: Create the Skill for Auditing Certificates

A **skill** in Azure SRE Agent is a markdown-based methodology document, referencing tool(s) for structured workflows.

### Example Skill YAML Frontmatter and Content

```yaml
---
name: ssl_certificate_audit
description: |
  Load this skill when the user asks about SSL/TLS certificate health, certificate expiry, certificate monitoring, or requests a certificate audit across one or more domains. Trigger phrases: "check our certificates", "are any certs expiring", "SSL audit", "certificate health check", "TLS certificate status", "cert renewal needed".
tools:
  - CheckSSLCertificateExpiry
---

# SSL/TLS Certificate Health Audit Skill

## Purpose

Perform a certificate health audit across domains: check each certificate, classify risk, and create a prioritized action plan.

## Workflow

1. Collect the list of domains
2. Run CheckSSLCertificateExpiry for each (parallelized if possible)
3. Classify results by days remaining: EXPIRED (<0), CRITICAL (<=7), WARNING (<=30), ATTENTION (<=60), HEALTHY (>60)
4. Summarize as tables: urgent issues first, healthy last
5. Provide next-step recommendations (renewal dates, auto-management advice)

## Example Output (condensed)

| Domain             | Expires     | Days Left | Risk      | Action                   |
|--------------------|-------------|-----------|-----------|--------------------------|
| api.contoso.com    | 2026-02-20  | **2**     | **CRITICAL** | Renew within 24 hours     |
| store.contoso.com  | 2026-03-10  | 20        | WARNING   | Schedule renewal this sprint |
| portal.contoso.com | 2026-06-15  | 117       | HEALTHY   | None                     |

Recommendation: Renew api.contoso.com immediately. Schedule store.contoso.com's renewal by March 3rd.
```

## Step 3: Deploy the Solution

- Deploy `CheckSSLCertificateExpiry` via Subagent Builder
- Create the `ssl_certificate_audit` skill as shown above
- Register both to the new `SSLCertificateMonitor` agent, enable parallel tool calls

## Example Usage and Scenarios

- Ask the agent: “Run a certificate health audit for microsoft.com, azure.com, github.com, and learn.microsoft.com”
- The agent executes parallel checks, classifies risk, and presents a prioritized markdown report with actionable recommendations

## Takeaways & Best Practices

- Proactively monitor your SSL/TLS estate to prevent outages
- Use Python’s standard library for fast, secure implementation
- Skills turn tools into actionable runbooks
- Integrate directly into Azure SRE workflows for maximum resilience

---
For further details and references, consult Azure SRE Agent documentation and explore extensibility with YAML and Python tools.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-a-custom-ssl-certificate-monitor-with-azure-sre-agent-from/ba-p/4495832)
