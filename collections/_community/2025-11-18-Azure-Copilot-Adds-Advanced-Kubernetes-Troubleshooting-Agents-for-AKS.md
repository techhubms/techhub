---
layout: post
title: Azure Copilot Adds Advanced Kubernetes Troubleshooting Agents for AKS
author: Samantha_Fernandez
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-advanced-kubernetes-troubleshooting-agent/ba-p/4471066
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 19:20:27 +00:00
permalink: /ai/community/Azure-Copilot-Adds-Advanced-Kubernetes-Troubleshooting-Agents-for-AKS
tags:
- Agent Mode
- Agentic Cloud Operations
- AKS
- Automated Mitigation
- Azure Copilot
- Cloud Diagnostics
- Cluster Management
- Kubectl
- Kubernetes
- Kubernetes Troubleshooting
- Microsoft Azure
- Preview Feature
- Root Cause Analysis
- Support Automation
section_names:
- ai
- azure
- devops
---
Samantha_Fernandez introduces the preview of AI-powered Kubernetes troubleshooting agents in Azure Copilot, offering automated root cause analysis, actionable solutions, and integrated support for AKS clusters.<!--excerpt_end-->

# Azure Copilot Adds Advanced Kubernetes Troubleshooting Agents for AKS

## What’s New?

Microsoft is previewing a new Kubernetes troubleshooting agent capability within [Azure Copilot](https://techcommunity.microsoft.com/blog/azureinfrastructureblog/ushering-in-the-era-of-agentic-cloud-operations-with-azure-copilot/4469664?previewMessage=true). This agent provides an intuitive, guided experience for detecting, triaging, and resolving issues in Azure Kubernetes Service (AKS) clusters. By using Kubernetes-specific keywords and running targeted `kubectl` commands, it can analyze cluster configuration, events, resource metrics, and diagnose problems such as pod failures or scaling bottlenecks. Users receive root cause analysis and actionable steps directly through Azure Copilot, empowering independent troubleshooting of complex diagnostics.

## How It Works

- The troubleshooting agent automatically investigates AKS cluster issues by executing relevant `kubectl` commands.
- It detects errors such as failing or pending pods, problematic cluster events, and abnormal resource utilization.
- The agent correlates signals across metrics and events, provides clear, step-by-step guidance for remediation, and offers one-click solutions for many common issues.
- If automated resolution is not possible, Azure Copilot generates a support request with all necessary diagnostics to facilitate assistance from Microsoft Support.
- This capability is available via Azure Copilot in the Azure Portal.

## Getting Started

- Admins can request preview access for agents at the tenant level in the Azure Copilot admin center.
- Once enabled, users will see an Agent mode toggle in the Copilot chat interface.
- Capacity is limited—sign up early for preview participation.
- To help shape future agentic cloud operations, join the customer feedback program [here](https://aka.ms/azurecopilot/agents/feedbackprogram).

## Troubleshooting Sample Prompts

When accessing an AKS cluster resource, click *Kubernetes troubleshooting with Copilot* to use the agent with specific resources. Example prompts include:

- "My pod keeps restarting can you help me figure out why?"
- "Pods are stuck pending; what is blocking them from being scheduled?"
- "I am getting ImagePullBackOff; how do I fix this?"
- "One of my nodes is NotReady; what is causing it?"
- "My service cannot reach the backend pod; what should I check?"

*Ensure agent mode is enabled in the chat window.*

## Learn More

- [Agents (preview) in Azure Copilot | Microsoft Learn](https://review.learn.microsoft.com/en-us/azure/copilot/agents-preview?branch=release-ignite-azure-copilot)
- [Troubleshooting agent capabilities in Agents (preview)](https://review.learn.microsoft.com/en-us/azure/copilot/troubleshooting-agent?branch=release-ignite-azure-copilot)
- [Announcing the CLI Agent for AKS: Agentic AI-powered operations and diagnostics](https://blog.aks.azure.com/2025/08/15/cli-agent-for-aks)
- [Microsoft Copilot in Azure Series - Kubectl](https://techcommunity.microsoft.com/blog/itopstalkblog/microsoft-copilot-in-azure-series---kubectl/4188499)

---
Author: Samantha_Fernandez

*Version 1.0. Updated Nov 18, 2025.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-advanced-kubernetes-troubleshooting-agent/ba-p/4471066)
