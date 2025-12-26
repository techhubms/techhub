---
layout: "post"
title: "Applying DevOps Principles on Lean Infrastructure: Lessons from Scaling to 102,000 Users and Planning Azure Migration"
description: "This detailed community post by onoja5, a Microsoft Certified DevOps Engineer, chronicles a real-world journey of scaling a social commerce platform from West Africa on a single VPS using DevOps practices, with a forward-looking plan to migrate to Azure in 2026. The content covers infrastructure details, cost management challenges, phased migration strategy, and specific questions about migrating DevOps workflows and cloud architecture. It provides insight into operational optimizations, migration anxieties, and requests advice from those who have gone through self-hosted to Azure migration."
author: "onoja5"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure/applying-devops-principles-on-lean-infrastructure-lessons-from/m-p/4476015#M22362"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-12-08 11:27:19 +00:00
permalink: "/community/2025-12-08-Applying-DevOps-Principles-on-Lean-Infrastructure-Lessons-from-Scaling-to-102000-Users-and-Planning-Azure-Migration.html"
categories: ["Azure", "DevOps"]
tags: ["Application Insights", "Azure", "Azure App Service", "Azure Blob Storage", "Azure Cache For Redis", "Azure CDN", "Azure Database For MySQL", "Azure Functions", "Azure Migration", "CI/CD Pipeline", "Cloud Cost Management", "Community", "DevOps", "GitHub Actions", "Hybrid Cloud", "IaC", "Laravel", "Monitoring And Observability", "MySQL", "PHP", "Redis", "Scaling", "Startup"]
tags_normalized: ["application insights", "azure", "azure app service", "azure blob storage", "azure cache for redis", "azure cdn", "azure database for mysql", "azure functions", "azure migration", "cislashcd pipeline", "cloud cost management", "community", "devops", "github actions", "hybrid cloud", "iac", "laravel", "monitoring and observability", "mysql", "php", "redis", "scaling", "startup"]
---

onoja5, a Microsoft Certified DevOps Engineer, shares insights from scaling a social commerce platform to over 100K users on VPS, detailing DevOps tactics and planning a multi-phase Azure migration while seeking advice from the Azure community.<!--excerpt_end-->

# Applying DevOps Principles on Lean Infrastructure: Lessons from Scaling to 102,000 Users and Planning Azure Migration

**Author:** onoja5, Microsoft Certified DevOps Engineer and Azure Developer

## Overview

This post details the journey of building and scaling a social commerce platform in West Africa to 102,000 users, using core DevOps principles on a single VPS. It offers a practical perspective on infrastructure constraints, cost management, and hands-on optimization. The author is preparing a phased migration to Azure and asks the Azure community for insights on best practices, migration pain points, and operational wisdom.

---

## Current Infrastructure and Stack

- **Platform:** Social commerce
- **Region:** West Africa
- **Active Users:** 102,000
- **Monthly Events:** 2 million
- **Uptime:** 99.2%
- **Infrastructure:** Single VPS
- **Technology Stack:** PHP, Laravel, MySQL, Redis
- **DevOps Practices:** CI/CD using GitHub Actions, custom monitoring, automated rollbacks, feature flags, automated backups, infrastructure as code, automated testing

---

## Key DevOps Practices Put Into Action

- **CI/CD Pipeline:**
  - Weekly deployments with GitHub Actions
  - Zero-downtime deployment and automated health check rollbacks
  - Gradual feature rollout with feature flags
- **Monitoring & Observability:**
  - Custom monitoring and performance tracking
  - Real-time alerts and resource usage monitoring
- **Automation:**
  - Automated backups, database optimization, image compression, and security updates
- **Infrastructure as Code:**
  - Configuration and deployment scripts stored in Git
  - Documented procedures
- **Testing & Quality:**
  - Automated test suite, pre-deployment health checks, staging environment
- **Optimization:**
  - Significant improvements in endpoint speed, database query performance, and image size reduction

---

## Planned Azure Migration Strategy (2026)

### **Phase 1: Hybrid (Q1 2026)**

- Use Azure CDN for static assets
- Azure Blob Storage for images
- Trial Application Insights for monitoring
- Maintain compute workload on current VPS

### **Phase 2: Compute Migration (Q2 2026)**

- Move API to Azure App Service
- Transition database to Azure Database for MySQL
- Implement Azure Cache for Redis
- VPS handles background jobs

### **Phase 3: Full Azure (Q3 2026)**

- Azure Functions for batch processing
- Transition fully to managed services
- Retirement of legacy VPS infrastructure

---

## Specific Migration Challenges & Community Questions

- **Architecture Decisions:** App Service vs Functions, managed services selection, MySQL vs Azure SQL, understanding cost-benefit thresholds
- **Cost Management:** Strategies for startups to control Azure costs, reserved instances, pay-as-you-go evaluation
- **Migration Strategy:** Lift-and-shift vs re-architect, zero-downtime migration for large user base, safe validation and cutover patterns
- **Monitoring & DevOps:** Value of Application Insights, Azure DevOps vs GitHub Actions, managing operational burden during migration
- **Development Workflow:** Local dev against Azure services, affordable staging, testing without incurring high cloud costs
- **DevOps Practices:** Which DevOps processes and learnings on VPS transfer to Azure, what needs adjustment in cloud-native environments

---

## Key Takeaways

- Lean infrastructure can scale successfully with disciplined DevOps and optimization—even prior to cloud migration
- Budget limitations shape cloud adoption timing and phased plans
- Migration to Azure requires reassessment of cost, architecture, and workflow, especially for startups
- Hands-on experience with DevOps fundamentals provides a strong foundation but must adapt to cloud-native paradigms

---

## Community Call

onoja5 requests Azure community guidance on migration pitfalls, cost management, and adapting DevOps workflows from self-hosted to cloud-native. Insights from practitioners are sought for a real-world perspective beyond certifications.

---

## About the Author

Microsoft Certified DevOps Engineer and Azure Developer, CTO at a rapidly scaling social commerce startup in West Africa. Preparing for phased Azure migration in 2026 after hands-on learning with certified training and practical optimization.

**Contact:** Reply to this thread to share migration advice and experience.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/applying-devops-principles-on-lean-infrastructure-lessons-from/m-p/4476015#M22362)
