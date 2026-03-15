---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/on-premises-manufacturing-intelligence/ba-p/4490771
title: Building On-Premises AI-Powered Asset Monitoring with Microsoft Foundry Local
author: Lee_Stott
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-24 08:00:00 +00:00
tags:
- AI
- Air Gapped Networks
- Asset Monitoring
- Azure
- Community
- Edge AI
- Express
- Industrial IoT
- JavaScript
- Maintenance Automation
- Microsoft Foundry Local
- Model Selection
- Node.js
- On Premises AI
- OPC UA
- Phi 3.5
- Predictive Maintenance
- Prompt Engineering
- Qwen2.5
- Regulatory Compliance
- SCADA Integration
- Telemetry Analysis
- .NET
section_names:
- ai
- azure
- dotnet
---
Lee_Stott explains how to build a fully on-premises AI-powered asset monitoring system for manufacturing using Microsoft Foundry Local, Node.js, and JavaScript. This guide emphasizes technical strategy and practical deployment.<!--excerpt_end-->

# Building On-Premises AI-Powered Asset Monitoring with Microsoft Foundry Local

**Author:** Lee_Stott

## Introduction

Modern manufacturing requires predictive maintenance, real-time diagnostics, and operational insights—but many facilities cannot risk sending proprietary process data into the cloud. This article demonstrates how to leverage Microsoft Foundry Local, a fully local AI runtime, to deliver asset intelligence entirely within factory walls, ensuring data sovereignty, reliable inference, and compliance with strict industrial regulations.

## Why On-Premises AI Matters

- **Data Sovereignty:** Protects proprietary telemetry, quality parameters, and maintenance history from cloud exposure.
- **Resilience:** Eliminates dependence on internet connectivity—critical for remote, air-gapped, or high-security facilities.
- **Low Latency:** Enables <50ms inference for applications like real-time defect detection and safety interlocks.
- **Cost Predictability:** Transforms ongoing API/compute costs into fixed hardware expenses, ideal for high-volume environments.
- **Compliance:** Avoids cross-border data movement and third-party processors, simplifying alignment with ITAR, FDA, and customer mandates.

## System Architecture Overview

### Components

- **Foundry Local Layer:** Local AI inference server running via Microsoft's Foundry Local SDK. Supports multiple LLM families (Phi, Qwen), infers on CPUs or GPUs.
- **Backend Service Layer:** Express/Node.js API mediates between plant systems, local AI, and operator interfaces. Hosts prompt logic, logic for equipment/telemetry analysis, and chat endpoints.
- **Frontend UI:** HTML/JavaScript-based dashboard. No build tools or frameworks required, keeping deployment simple and auditable.

### Data Flow

1. **Operator triggers analysis** via UI (e.g., asset health check or log classification).
2. **Backend assembles context-rich prompt** from telemetry, alerts, and history.
3. **Prompt is sent to Foundry Local** for on-device inference. Response is parsed for actionable outputs.
4. **Frontend displays actionable summaries**, recommendations, and event prioritizations.

## Implementation Details

### Foundry Local Setup

- Download via [winget] or [brew] depending on OS.
- Select models matching hardware: e.g. use `Qwen2.5-0.5b` for fast dashboard updates, `Phi-3.5-mini` for balanced accuracy, or `Phi-4-mini` for deep analysis.

### Backend Service (Node.js/Express)

- Connects to `localhost:8008` for AI inference via Foundry SDK.
- Defines endpoints, e.g.:
  - `/api/assets/:id/summary`: Generates AI analysis of equipment health.
  - `/api/logs/classify`: Classifies maintenance logs with prompt-designed JSON outputs.
  - `/api/chat`: Handles operator conversational queries using domain-specific context prompts and AI responses.
- Implements robust error handling, model auto-discovery, and fallbacks for network or service interruptions.

**Sample Code Snippet: Asset Health Summary Endpoint**

```js
// Express route to generate asset health summary
router.get('/api/assets/:id/summary', async (req, res) => {
  const asset = await getAssetData(req.params.id);
  if (!asset) return res.status(404).json({ error: 'Asset not found' });
  const prompt = buildHealthAnalysisPrompt(asset);
  const analysis = await foundryService.generateCompletion(prompt);
  res.json({
    asset_id: asset.id,
    summary: analysis.content,
    model_used: analysis.model
  });
});
```

### Frontend UI

- Four main tabs: Plant Overview, Asset Health, Maintenance, and AI Assistant.
- Dashboards fetch data from the backend, present real-time statuses, and display structured AI outputs (e.g., recommendations, priority alerts, or JSON classifications).

## Prompt Engineering

- Role definition and context are crucial for reliable LLM output.
- Output formats are constrained to enable machine parsing:
  - For log classification, AI is instructed to return strict JSON structures.
  - For asset health, prompts include telemetry, thresholds, maintenance history, and specific questions.

## Example Use Cases

- **Incident Triage:** Supervisor asks "What's wrong with Line 2?"; AI summarizes key issues, citing alerting assets and metrics.
- **Maintenance Log Processing:** Operator enters "Grinding noise during pump startup..."; AI classifies as MECHANICAL/HIGH and extracts symptoms, causes, and recommendations.
- **Decision Support:** "What’s the priority for this shift?"; AI synthesizes plant alerts, recommends immediate and scheduled actions.

## Deployment Notes

- **Runs on typical industrial hardware** (8-16GB RAM, 4-8 cores; GPU recommended but not required).
- **Service hosted on internal network only** (never internet-accessible); integrates over REST for SCADA/MES/CMMS.
- **RBAC** with Active Directory/LDAP validation for compliance.

## Resources

- [FoundryLocal-IndJSsample GitHub Repo](https://github.com/leestott/FoundryLocal-IndJSsample)
- [Official Docs](https://learn.microsoft.com/azure/ai-studio/foundry-local)
- [Edge AI for Beginners](https://aka.ms/edgeai-for-beginners)

## Key Takeaways

- **On-premises AI unlocks modern intelligence without sacrificing security, latency, or compliance.** Prompt engineering, system modularity, and robust local inference models (via Foundry Local) make it possible for any industrial operation to adopt advanced monitoring and maintenance automation, all within their existing IT boundaries.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/on-premises-manufacturing-intelligence/ba-p/4490771)
