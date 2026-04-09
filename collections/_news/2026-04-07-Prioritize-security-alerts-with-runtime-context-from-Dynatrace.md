---
tags:
- Application Security
- Code Scanning
- Container Images
- Dependabot Alerts
- Deployment Context
- DevOps
- Dynatrace
- GitHub Advanced Security
- GitHub App Authentication
- GitHub Enterprise Cloud
- Internet Exposure
- Kubernetes
- News
- Runtime Context
- Runtime Risk
- Security
- Security Campaigns
- Sensitive Data
- Supply Chain Security
- Vulnerability Prioritization
section_names:
- devops
- security
date: 2026-04-07 15:15:24 +00:00
feed_name: The GitHub Blog
primary_section: devops
title: Prioritize security alerts with runtime context from Dynatrace
external_url: https://github.blog/changelog/2026-04-07-prioritize-security-alerts-with-runtime-context-from-dynatrace
author: Allison
---

Allison explains a GitHub Advanced Security update that uses Dynatrace runtime context to help teams prioritize code scanning and Dependabot alerts based on what’s actually deployed and the observed runtime risk in Kubernetes environments.<!--excerpt_end-->

# Prioritize security alerts with runtime context from Dynatrace

You can now use **runtime context from Dynatrace** to prioritize **GitHub Advanced Security** alerts based on **deployed artifacts** and **runtime risk** in a **Kubernetes** environment.

When you connect **Dynatrace** to **GitHub**, GitHub can show:

- **Deployment context** for container images that Dynatrace maps back to your repositories
- **Runtime risk signals** from Dynatrace

This helps you focus remediation on alerts that:

- Affect artifacts that are **actually deployed**
- Are associated with **higher-risk runtime conditions** (as detected by Dynatrace)

## How to use runtime context in GitHub

You can use deployment visibility and runtime risk signals to **filter and prioritize**:

- **GitHub code scanning** alerts
- **Dependabot** alerts

This works on:

- The alert pages
- **Security campaigns**

### Example filter

Filter down to vulnerabilities that affect deployed artifacts and are exposed to the internet:

```text
has:deployment AND runtime-risk:internet-exposed
```

### Runtime risk signals

Runtime risk signals include:

- Public internet exposure (`runtime-risk:internet-exposed`)
- Access to sensitive data assets (`runtime-risk:sensitive-data`)

## Availability

This feature is **generally available** for **GitHub Enterprise Cloud** customers.

## Configuration docs

Dynatrace documentation for configuring the integration with GitHub Advanced Security:

- https://docs.dynatrace.com/docs/secure/threat-observability/security-events-ingest/ingest-github-advanced-security#credentials--github-app-based-authentication


[Read the entire article](https://github.blog/changelog/2026-04-07-prioritize-security-alerts-with-runtime-context-from-dynatrace)

