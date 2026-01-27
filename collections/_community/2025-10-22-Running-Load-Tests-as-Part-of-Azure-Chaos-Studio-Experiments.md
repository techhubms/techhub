---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/running-a-load-test-within-a-chaos-experiment/ba-p/4463344
title: Running Load Tests as Part of Azure Chaos Studio Experiments
author: Nikita_Nallamothu
feed_name: Microsoft Tech Community
date: 2025-10-22 15:00:00 +00:00
tags:
- App Service
- Application Insights
- Autoscaling
- Azure Chaos Studio
- Azure Load Testing
- Chaos Engineering
- CI/CD
- Cloud Reliability
- Fault Injection
- Load Testing
- Log Analytics
- Microsoft Azure
- Monitoring
- Performance Testing
- Resiliency
- SQL Database
section_names:
- azure
- devops
- security
primary_section: azure
---
Nikita Nallamothu demonstrates how to combine Azure Chaos Studio and Azure Load Testing to uncover resilience blind spots by orchestrating load tests with fault injections, providing actionable strategies for cloud reliability engineering.<!--excerpt_end-->

# Running Load Tests as Part of Azure Chaos Studio Experiments

Building scalable, reliable cloud applications requires testing how your system behaves not only under normal conditions but under stress and failure. This article discusses using [Azure Chaos Studio](https://aka.ms/AzureChaosStudio) and [Azure Load Testing](https://aka.ms/malt) together to:

- Run controlled load tests
- Inject faults into your application or infrastructure
- Analyze resilience and identify failure points

## Why Combine Chaos and Load Testing?

Many issues, such as cascading failures or retry storms, become visible only when your application is both busy and facing disruptions. By orchestrating chaos experiments that include load tests, you discover how effective your resiliency strategies are under pressure.

### Example Scenarios

- How does a database outage during peak traffic affect overall system behavior?
- What if a dependent service starts returning errors?
- Are your autoscaling rules sufficient to recover quickly?

## Azure Chaos Studio + Azure Load Testing Integration

Azure Chaos Studio provides actions to start and stop load tests from [Azure Load Testing](https://learn.microsoft.com/en-us/azure/app-testing/load-testing/overview-what-is-azure-load-testing) as part of an experiment. From the [fault library](https://learn.microsoft.com/en-us/azure/chaos-studio/chaos-studio-fault-library#orchestration-actions), you can orchestrate steps such as:

1. **Start load test** (via Azure Load Testing)
2. **Inject fault** (shutdown VM, throttle network, restart resources)
3. **Observe system metrics**
4. **Stop load test and analyze results**

This enables controlled, repeatable stress scenarios integrated into your testing pipelines.

## Building a Chaos Experiment with Load Testing

### Step 1. Define the Experiment

- Target your application (e.g., App Service, SQL Database)
- Add a **Start Load Test** action to kick off a controlled load session

### Step 2. Simulate Faults

- Inject faults such as CPU pressure, added network latency, or service shutdowns immediately following your load test action

### Step 3. Observe and Analyze Results

- Monitor response times, error rates, and throughput using Azure Load Testing
- Track outcomes from fault injections in Chaos Studio
- Correlate with [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) or [Log Analytics](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/log-analytics-tutorial)

### Step 4. Review Findings

By reviewing how your system responds, you can answer questions like:

- Do retry policies work as intended when faults are injected under load?
- Is there a performance impact when auto-failover mechanisms trigger?
- Does the system recover automatically after the fault?

## Conclusion

Integrating chaos and load testing in Azure allows teams to build confidence in system resilience. Azure’s native support for this integration fits naturally with CI/CD pipelines, enabling robust, automated reliability validation.

## Learn More

- [Azure Chaos Studio documentation](http://aka.ms/ChaosDocs)
- [Azure Load Testing documentation](https://learn.microsoft.com/en-us/azure/app-testing/load-testing/overview-what-is-azure-load-testing)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/running-a-load-test-within-a-chaos-experiment/ba-p/4463344)
