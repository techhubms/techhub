---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-in-azure-container-apps-at-ignite-25/ba-p/4470391
title: "What's New in Azure Container Apps: Ignite 2025 Feature Updates"
author: vyomnagrani
feed_name: Microsoft Tech Community
date: 2025-11-18 16:18:11 +00:00
tags:
- .NET
- A/B Testing
- Agent Frameworks
- AI Workloads
- Azure Container Apps
- Azure Functions
- Blue Green Deployment
- Cloud Native
- Confidential Computing
- Data Isolation
- Deployment Labels
- Distributed Application Runtime
- Docker Compose
- Durable Task Scheduler
- Flexible Workload Profile
- Goose AI Agent
- GPU Acceleration
- Java
- LangChain
- MCP
- Microservices
- N8n
- Networking
- Open Telemetry
- Premium Ingress
- Rule Based Routing
- Serverless GPUs
- Spring AI
- Workflow Automation
- AI
- Azure
- Coding
- DevOps
- GitHub Copilot
- Machine Learning
- Security
- Community
section_names:
- ai
- azure
- coding
- devops
- github-copilot
- ml
- security
primary_section: github-copilot
---
vyomnagrani details major Azure Container Apps updates from Ignite 2025, outlining new features for AI hosting, agent frameworks, security, DevOps, and workflow modernization in Microsoft's cloud-native ecosystem.<!--excerpt_end-->

# What's New in Azure Container Apps: Ignite 2025 Feature Updates

Azure Container Apps (ACA) is emerging as Microsoft's leading managed, serverless container platform, enabling developers to build, deploy, and scale microservices and modern apps without container or infrastructure expertise.

## Key Feature Highlights

### AI and Agentic Workloads

- ACA now supports hosting AI-centric workloads, including intelligent agents that benefit from integrated code interpreter, serverless GPUs, and simplified deployments.
- Customers such as Replit, NFL Combine, Coca-Cola, the European Space Agency, and Microsoft Copilot rely on ACA for scalable AI compute.
- ACA's GPU acceleration and agent framework integrations empower rapid development and deployment of AI-driven applications and multi-container environments.

### Modernizing Legacy Apps with AI Tools

- ACA facilitates seamless migration of legacy applications (.NET, Java) to cloud-native microservices.
- GitHub Copilot integrations automate code upgrades, dependency analysis, and cloud transformation.
- ACA simplifies complex migration with managed infrastructure, enabling adoption of best practices in version control, network management, and application security.

### Secure AI Compute Sandboxes

- Public preview of dynamic shell sessions provides isolated, secure environments for code execution, tool testing, and workflow automation.
- ACP sessions allow agent frameworks to provision disposable compute securely and efficiently ([tutorial](https://aka.ms/aca/dynamic-sessions-mcp-tutorial)).

### Docker Compose for Agents

- ACA adds Docker Compose for Agents in public preview, supporting easy definition of agentic applications (including custom models and MCP).
- Native GPU support streamlines AI agent deployment using frameworks like LangGraph, LangChain CrewAI, Spring AI, Vercel AI SDK, and Agno.

### Serverless GPU Expansion

- ACA's Serverless GPU offering (NVIDIA A100, T4) is now available in 11 more regions, supporting AI inference, training, and accelerated workloads. [Region details](https://aka.ms/aca/serverless-gpu-regions)

### Flexible Workload Profiles

- 'Flexible' workload profile blends serverless pay-per-use with dedicated resource performance. Features include scheduled maintenance, dedicated networking, and support for large replicas.
- Ideal for high-demand apps needing isolation and predictable scaling. [Overview](https://learn.microsoft.com/en-us/azure/container-apps/workload-profiles-overview)

### Confidential Computing

- ACA introduces confidential computing in public preview, utilizing hardware-based Trusted Execution Environments (TEEs) for secure data processing ([info](https://aka.ms/aca/confidential-computing)).
- Memory encryption and pre-verified cloud environment protection support organizations with high security requirements.

### Enhanced Networking

- Rule-based routing reaches general availability, simplifying microservice architectures, A/B testing, and blue-green deployments ([details](https://aka.ms/aca/rule-based-routing)).
- Premium Ingress now generally available, offering scalable ingress proxies and configurable environment-level options ([guide](https://aka.ms/aca/premium-ingress-learn)).

### Advanced Management Capabilities

- Deployment labels (public preview) support environments like dev, staging, prod for revision management and advanced traffic routing ([learn more](https://aka.ms/aca/deploymentlabels)).
- Durable Task Scheduler support (GA): Enables resilient, containerized workflow automation with state persistence and monitoring capabilities ([workflows](https://learn.microsoft.com/en-us/azure/container-apps/workflows-overview)).

## Cross-Platform and Framework Compatibility

- ACA supports Azure Functions, Distributed Application Runtime (Dapr), and polyglot development with .NET, Java, and JavaScript ([.NET overview](https://learn.microsoft.com/en-us/azure/container-apps/dotnet-overview)).
- Organizations can leverage open-source agents (n8n, Goose AI Agent) for no-code automation and advanced AI integrations.

## Security and Compliance Features

- Confidential compute and strict container data isolation raise the bar for secure hosting.
- Networking improvements allow granular control over routing, ingress, and revision rollouts, supporting best practices in secure microservice development.

## What's Next

- ACA continues to enhance AI agent orchestration, serverless scalability, and integration with enterprise-grade security and developer tools.
- Developers are encouraged to contribute feedback or feature requests via the [ACA GitHub page](https://github.com/microsoft/azure-container-apps/issues) and check out the [roadmap](https://aka.ms/aca/roadmap).

---

For more updates, learnings, and support resources, connect with the ACA community and explore official documentation links included throughout this post.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-in-azure-container-apps-at-ignite-25/ba-p/4470391)
