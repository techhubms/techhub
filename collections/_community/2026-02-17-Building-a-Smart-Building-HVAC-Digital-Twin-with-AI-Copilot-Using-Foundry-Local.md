---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-smart-building-hvac-digital-twin-with-ai-copilot/ba-p/4490784
title: Building a Smart Building HVAC Digital Twin with AI Copilot Using Foundry Local
author: Lee_Stott
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-17 08:00:00 +00:00
tags:
- AI
- AI Copilot
- Building Management
- Community
- Digital Twin
- Energy Efficiency
- Express
- Facility Automation
- Fault Injection
- HVAC
- IoT
- JSON State Management
- Microsoft Foundry Local
- Natural Language Processing
- Node.js
- Operations Technology
- Physics Modeling
- React
- REST API
- Simulation
- Three.js
- WebSocket
- .NET
section_names:
- ai
- dotnet
---
Lee Stott walks you through building a smart building HVAC digital twin using AI-powered copilots, combining physics-based simulation and 3D visualization with Microsoft Foundry Local for advanced facilities management.<!--excerpt_end-->

# Building a Smart Building HVAC Digital Twin with AI Copilot Using Foundry Local

Smart building operations present major challenges, from energy optimization to delivering comfortable environments for occupants. Traditional BMS (Building Management Systems) simply visualize raw sensor data, but translating that data into actionable insight typically demands deep HVAC expertise.

This article by Lee Stott details how to develop a fully featured digital twin for HVAC system management. It integrates:

- **DigitalTwin** open-source platform for modeling building systems
- **React** and **Three.js** for real-time 3D visualization
- **Microsoft Foundry Local** for natural language AI copilot capabilities

## Why Digital Twins Matter in Buildings

- Physical buildings generate vast data, but lack interpretation—digital twins enable simulation and diagnosis
- The example demonstrates how physics-based modeling (thermal dynamics, airflow models, CO₂ balance) and AI reasoning give operators both predictions and explanations
- Operators can run 'what-if' scenarios before deploying real-world changes to HVAC setpoints

## 3-Tier Digital Twin Architecture

- **Frontend:** React + Three.js renders interactive, color-coded 3D building layouts and live KPIs
- **Backend:** Node.js/Express simulates equipment, manages twin state in JSON, exposes REST APIs, and pushes updates via WebSockets
- **AI Copilot:** Microsoft Foundry Local SDK provides locally hosted AI models for natural language queries (e.g., "Why is the third floor warm?"). Answers reference actual live sensor data and system state

## Physics-Based Simulation: Core Models

- Implements real HVAC dynamics: 1R1C thermal model, HVAC impact, envelope losses, and occupancy-based internal heat gain
- CO₂ tracking using mass balance equations
- Sample code shows modular, maintainable backend design for simulation updates (`ZoneThermalModel`, `AirQualityModel`)
- KPIs and real-time anomaly detection are incorporated

## Real-Time 3D Visualization

- React Three Fiber components render the building state visually (zones, floors, HVAC equipment)
- Color gradients represent temperature deviation from setpoints
- Clicking on equipment delivers details; live updating via WebSocket feeds

## Integrating AI Copilot for Operations

- AI copilot is invoked through REST endpoints; queries like "What is the building's energy usage?" result in grounded, domain-specific, data-driven responses
- System uses models (like phi-4) via Microsoft Foundry Local to process context, generate actionable recommendations, or suggest next steps
- Example code demonstrates pattern for safe, non-destructive AI interaction with physical systems

## Fault Injection Framework

- Full testability: Operators simulate faults (e.g., chiller failure, stuck VAV dampers, sensor drift)
- Faults raise system alerts and trigger realistic diagnostic scenarios for both human and automated responses

## Production Considerations

- For live deployment, connect digital twin to real hardware using standard industrial protocols (BACnet, Modbus, MQTT)
- Models should be calibrated against historical building data; setup can enable continuous learning/adjustment
- Safety maintained by restricting AI control to advice/recommendation roles

## Resources

- [DigitalTwin GitHub Repo](https://github.com/leestott/DigitalTwin)
- [Microsoft Foundry Local Docs](https://foundrylocal.ai)
- [Three.js Documentation](https://threejs.org/docs/)

This hands-on architecture is suitable for developers working on facility automation, smart building analytics, or integrating advanced AI reasoning in operational technology solutions.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-a-smart-building-hvac-digital-twin-with-ai-copilot/ba-p/4490784)
