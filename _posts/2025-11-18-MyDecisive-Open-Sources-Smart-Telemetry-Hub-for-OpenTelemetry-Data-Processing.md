---
layout: "post"
title: "MyDecisive Open Sources Smart Telemetry Hub for OpenTelemetry Data Processing"
description: "This article covers MyDecisive's initiative to open source its Smart Telemetry Hub, a Kubernetes-based OpenTelemetry data processor. The enhanced filtering mechanism helps DevOps teams reduce telemetry volumes, lower observability costs, and improve mean time to remediation (MTTR). The platform leverages memory-based telemetry filtering and aims to evolve OpenTelemetry standards within CNCF."
author: "Mike Vizard"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devops.com/mydecisive-open-sources-platform-for-processing-opentelemetry-data/"
viewing_mode: "external"
feed_name: "DevOps Blog"
feed_url: "https://devops.com/feed/"
date: 2025-11-18 14:00:32 +00:00
permalink: "/2025-11-18-MyDecisive-Open-Sources-Smart-Telemetry-Hub-for-OpenTelemetry-Data-Processing.html"
categories: ["DevOps"]
tags: ["Alerting", "Backend Analysis", "Business Of DevOps", "CNCF", "Data Filtering", "DevOps", "DevOps Observability Costs", "Golden Signals", "Instrumentation", "Kubernetes", "Kubernetes Telemetry", "Memory Based Filtering", "MTTR Reduction", "Observability", "Open Source", "Open Source Observability", "OpenTelemetry", "OTel Collectors", "Posts", "Proactive Monitoring.", "Smart Telemetry Hub", "Social Facebook", "Social LinkedIn", "Social X", "Telemetry Data", "Telemetry Filtering", "Telemetry Monitoring", "Telemetry Overload"]
tags_normalized: ["alerting", "backend analysis", "business of devops", "cncf", "data filtering", "devops", "devops observability costs", "golden signals", "instrumentation", "kubernetes", "kubernetes telemetry", "memory based filtering", "mttr reduction", "observability", "open source", "open source observability", "opentelemetry", "otel collectors", "posts", "proactive monitoringdot", "smart telemetry hub", "social facebook", "social linkedin", "social x", "telemetry data", "telemetry filtering", "telemetry monitoring", "telemetry overload"]
---

Mike Vizard explores how MyDecisive's open-sourced Smart Telemetry Hub brings advanced telemetry filtering to DevOps teams, supporting efficient observability and lower costs.<!--excerpt_end-->

# MyDecisive Open Sources Smart Telemetry Hub for OpenTelemetry Data Processing

**Author: Mike Vizard**

MyDecisive has made its Smart Telemetry Hub available under a permissive open source license, pushing the evolution of OpenTelemetry by introducing local, memory-based telemetry filtering for Kubernetes-based deployments. This initiative is designed to help DevOps teams optimize observability data flow, helping reduce operational costs and improve mean time to remediation (MTTR).

## Key Highlights

- **Kubernetes Platform**: The Smart Telemetry Hub operates within Kubernetes, leveraging its orchestration for scalable telemetry collection and processing.
- **Local Memory-Based Filtering**: Unlike traditional OpenTelemetry agents which pass through raw telemetry to backend observability platforms, this approach adds an in-memory filtering step. This means only relevant traces and metrics proceed, easing backend analysis loads.
- **CNCF Collaboration**: MyDecisive is joining the Cloud Native Computing Foundation (CNCF) to influence OpenTelemetry’s future direction, encouraging broader adoption of data filtering standards.
- **Golden Signals Alerting**: DevOps teams can set up intelligent alerts based on golden signals to proactively monitor application health, making it easier to address service issues before they escalate.
- **Proactive Cost Reduction**: By analyzing and reducing telemetry volume upstream, the platform directly lowers observability costs and streamlines management, an increasing concern as complex application environments grow.

## Context

As organizations instrument more applications for monitoring, telemetry volumes continue to surge, often overwhelming existing analysis platforms. Historically, high costs of deploying agents and collecting telemetry data led teams to limit instrumentation. With Smart Telemetry Hub, that limitation is addressed, opening the door to instrumenting more services without the accompanying telemetry overload.

## Challenges and Future Directions

While managing OpenTelemetry collectors in Kubernetes can be complex, local filtering alleviates backend pressure. The article notes uncertainty about community adoption for in-memory filtering, but emphasizes industry need. MyDecisive's approach aims to deliver cost-effective observability and anticipates future improvements—potentially with further AI integration for accessible analysis.

## Further Resources

- [Original Article](https://devops.com/mydecisive-open-sources-platform-for-processing-opentelemetry-data/)
- [Cloud Native Computing Foundation](https://www.cncf.io/)
- [OpenTelemetry Project](https://opentelemetry.io/)

---

*DevOps teams seeking effective telemetry and observability strategies may find Smart Telemetry Hub a useful addition for managing growing data volumes and improving incident response times.*

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/mydecisive-open-sources-platform-for-processing-opentelemetry-data/)
