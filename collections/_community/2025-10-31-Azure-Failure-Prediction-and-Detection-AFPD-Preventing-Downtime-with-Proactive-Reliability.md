---
external_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/revolutionizing-reliability-introducing-the-azure-failure/ba-p/4464883
title: 'Azure Failure Prediction & Detection (AFPD): Preventing Downtime with Proactive Reliability'
author: andrewb710
feed_name: Microsoft Tech Community
date: 2025-10-31 19:02:54 +00:00
tags:
- A/B Testing
- AFPD
- Azure Compute
- Azure Failure Prediction & Detection
- Azure Notifications
- Azure Resource Graph
- Cloud Monitoring
- Cloud Reliability
- Contextual Bandit Models
- Event Grid
- Failure Mitigation
- Fleet Health
- Live Migration
- Operational Resilience
- Project Flash
- Project Narya
- Resource Health
- Scheduled Events
- Virtual Machines
- VM Watch
section_names:
- azure
- devops
primary_section: azure
---
Andrew Boyd and collaborators shed light on Azure’s Failure Prediction & Detection (AFPD) system, detailing proactive strategies for maintaining cloud reliability, minimizing downtime, and making the most of Azure's notification and fleet health tools.<!--excerpt_end-->

# Azure Failure Prediction & Detection (AFPD): Preventing Downtime with Proactive Reliability

**Authors:** Ayberk Ozturk, Andrew Boyd, Otis Smith, Sameer Hussain, Joao Madureira, Ronit Sharma, Isha Bhatia, Halley Ding, Parvaneh Alavi, Jelena Ilic, Kevin Meehan, Steven Li, Arhatha Bramhanand, Nathan Ernst, Abhishek Sanghai, Adam Wilson, Blake Wheaton, Dhruv Matta, Olubusola Femi-Fowode, Shweta Patil, and Tajinder Pal Singh Ahluwalia

## Introduction

Azure Failure Prediction & Detection (AFPD) is Azure’s advanced shift-left reliability solution, operating since 2024. It unifies failure prediction, detection, mitigation, notification, and remediation into a single system aimed at preventing workload interruptions and performing node repairs at scale for Azure Compute—including General Purpose, HPC/AI, and select Storage scenarios. AFPD extends prior solutions like Project Narya and introduces new best practices for fleet health management, contributing to a reduction in service reboots and improved cloud stability.

---

## How AFPD Improves Azure Reliability

- **Unified process:** Combines prediction, detection, mitigation, and remediation under consistent performance metrics.
- **Expanded coverage:** Handles more hardware and select software scenarios than earlier tools.
- **Advanced models:** Utilizes Contextual Bandit models, along with A/B and Multi-Armed Bandit techniques, to optimize mitigation tailored to current conditions.
- **Automation:** Automates node repair, live migration, and customer notifications to minimize user-impacting downtime.

---

## AFPD: Three-Phase Operation

1. **Failure Prediction and Detection**
   - Uses system telemetry to monitor node and component health.
   - Detects impending failures (especially SSDs and some software scenarios).
   - Tags at-risk nodes and triggers repair workflows.
2. **Mitigation**
   - Marks the faulty node as "Unallocatable" to halt new workload allocation.
   - Migrates running workloads to healthy nodes (Live Migration) if possible.
   - If migration isn’t possible, AFPD notifies customers to redeploy affected VMs.
3. **Remediation**
   - After evacuation, assigns fault codes and diagnostic details.
   - Spare components are staged proactively to speed repair turnaround.
   - Returns remediated nodes quickly to production post-diagnosis and repair.

---

## Consuming AFPD Notifications

- **Flash Health Events:** Real-time and historical alerts on resource health, visible in the Azure portal.
- **Scheduled Events:** Proactive notifications about upcoming VM impacts, including planned/unplanned downtime related to AFPD detection.
- **How notifications work:**
  - Alerts describe degradation events, timing, and recommended actions (e.g., wait for migration, redeploy VM).
  - Deadlines are provided where customer action is needed.
  - Additional resources are linked for guidance.

---

## Integration with Project Flash & Platform Tools

- **Resource Health Portal:** Central place in the Azure portal for tracking ongoing health and disruption events, including AFPD.
- **Resource Graph:** Allows for querying all health-related annotations for deep investigations or audit.
  - Example query:

    ```
    healthresources | where type =~ 'microsoft.resourcehealth/resourceannotations' | extend temp = parse_json(properties) | where temp.impactType == "Degraded"
    ```

- **Event Grid:** Enables automated action (e.g., redeploy, restart) responding to AFPD events through logic apps or Azure Functions.
- **Filtering:** Customize monitoring with event filters based on resource annotation JSON properties.

---

## Scheduled Events and Automated Resiliency

- Scheduled events ensure a single stream of updates for all availability-affecting incidents, planned or unplanned.
- Automated responders can proactively migrate workloads based on Instance Metadata Endpoint alerts, minimizing downstream business impact.
- Developers need to ensure their workloads can handle scheduled event "not before" times up to 7 days out.
- [Code samples are available for onboarding](https://learn.microsoft.com/en-us/azure/virtual-machines/linux/scheduled-events#python-sample-2).

---

## Enhanced Diagnostics with VM Watch (Especially for L-Series)

- **VM Watch:** An in-VM watchdog that emits real-time diagnostics from guest OS to Azure, expediting identification of regressions.
- **Enablement:** Onboard with Azure CLI, PowerShell, ARM templates, or Azure Policy.
- **Custom configuration:** Set groups with CohortId, control suite of diagnostic tests, and stream results via Event Hub.
- **Benefits:** Especially useful for data-intensive L-Series VMs, detecting issues like network, DNS, and I/O anomalies.
- [Onboarding guide](https://learn.microsoft.com/en-us/azure/virtual-machines/install-vm-watch?tabs=cli-1%2Ccli-2).
- [Configuration options](https://learn.microsoft.com/en-us/azure/virtual-machines/configure-vm-watch?tabs=ARM-template-1).

---

## Further Reading

- [Project Narya deep dive](https://azure.microsoft.com/en-us/blog/advancing-failure-prediction-and-mitigation-introducing-narya/?msockid=0b9480c9a32a63ae2204955da23e6290)
- [Resource Health Annotations](https://learn.microsoft.com/en-us/azure/service-health/resource-health-vm-annotation)
- [Azure Resource Graph samples](https://learn.microsoft.com/en-us/previous-versions/azure/governance/resource-graph/samples/samples-by-table?tabs=azure-cli#healthresources)
- [Azure Event Grid documentation](https://learn.microsoft.com/en-us/azure/event-grid/event-handlers)
- [VM Watch documentation](https://learn.microsoft.com/en-us/azure/virtual-machines/azure-vm-watch)

---

**Authors:** Ayberk Ozturk, Andrew Boyd, Otis Smith, and team

_Last updated: Oct 31, 2025_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/revolutionizing-reliability-introducing-the-azure-failure/ba-p/4464883)
