---
layout: post
title: Scenario-Based Feature Flag Management in Azure App Configuration Portal
author: nuzhat-minhaz
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/new-portal-experience-for-feature-management/ba-p/4467748
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 22:42:56 +00:00
permalink: /ai/community/Scenario-Based-Feature-Flag-Management-in-Azure-App-Configuration-Portal
tags:
- A/B Testing
- AI Deployment
- Application Insights
- Azure App Configuration
- Azure Portal
- Canary Deployments
- Experimentation
- Feature Flags
- Feature Management
- Model Versioning
- Operations
- Rollouts
- Switch Rollout Experiment
- Telemetry
section_names:
- ai
- azure
- devops
---
nuzhat-minhaz presents a practical guide on the new scenario-based feature flag experience in Azure App Configuration, simplifying complex deployments for AI and cloud applications.<!--excerpt_end-->

# Scenario-Based Feature Flag Management in Azure App Configuration Portal

Azure App Configuration's Feature manager has introduced a new scenario-driven workflow for feature flag creation, designed to help development teams deploy code more safely and iteratively. Rather than requiring upfront, deep knowledge of feature flag architectures, this update enables users to select their intended scenario and have the portal guide them through configuration options tailored to their use case.

## Key Scenarios for Modern Development

The new portal workflow offers three primary journeys for feature flag creation, supporting a range of practical deployment patterns:

### 1. Switch: On/Off Toggles

- **Emergency kill switches**: Instantly disable problematic features or AI models in production environments
- **Maintenance mode**: Toggle global maintenance without deploying code
- **Feature gating**: Control access to beta or region-restricted features
- **Debug and diagnostic modes**: Enable verbose logging or troubleshooting features
- **Compliance and fallback**: Quickly toggle features to meet regional requirements or switch between AI/rule-based responses

### 2. Rollout: Gradual, Targeted Releases

- **Canary deployments**: Safely expose new features to a small percentage of users before broader release
- **Geographic and tiered rollouts**: Target releases by region or subscription tier (e.g., RAG-enhanced search for enterprise users)
- **Employee dogfooding**: Test LLM-powered features internally ahead of customer launch
- **Time-based and load testing switches**: Automate when features become available or stress-test infrastructure

### 3. Experiment: Data-Driven Variant Management

- **A/B and multivariate experiments**: Test alternative flows, layouts, or algorithms (e.g., comparing Claude vs. GPT-4 vs. Gemini)
- **Model routing**: Send traffic to different AI/model backends based on scenario needs
- **Personalization and configuration management**: Serve customized onboarding flows or region-specific configurations
- **Tenant customization and traffic distribution**: Fine-grained control for multi-tenant applications

All scenarios integrate seamlessly with Azure App Insights telemetry if your App Configuration store is connected, making it easy to gain insights into feature and variant performance.

## How to Get Started

1. Open your App Configuration resource in Azure Portal
2. Go to **Operations > Feature manager > Create**
3. Select the scenario that fits your planned usage: Switch, Rollout, or Experiment
4. The portal presents setup steps and tabs that match your choice, streamlining the configuration process

This update is backward compatible; existing flags remain unchanged, but new flags are easier to configure and manage based on intent. This scenario-based approach is just one step in broad improvements to feature management and will continue to evolve, especially for AI experimentation and variant analysis.

### Additional Resources

- [Manage feature flags in Azure App Configuration](https://learn.microsoft.com/en-us/azure/azure-app-configuration/manage-feature-flag)
- [Feature management concepts](https://learn.microsoft.com/en-us/azure/azure-app-configuration/concept-feature-management)

*Authored by nuzhat-minhaz.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/new-portal-experience-for-feature-management/ba-p/4467748)
