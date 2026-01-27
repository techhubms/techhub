---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-of-larger-container-sizes-on/ba-p/4463863
title: General Availability of Larger Container Sizes on Azure Container Instances
author: tysonfreeman
feed_name: Microsoft Tech Community
date: 2025-11-05 15:17:40 +00:00
tags:
- AKS
- ARM Templates
- Azure CLI
- Azure Container Instances
- Azure Portal
- Bicep
- Cloud Computing
- Confidential Containers
- Container SKUs
- Data Inferencing
- High Performance Computing
- Memory Allocation
- PowerShell
- Scalability
- Serverless Containers
- Vcpu Scaling
- Virtual Networks
section_names:
- azure
primary_section: azure
---
tysonfreeman details the launch of larger container sizes on Azure Container Instances, explaining benefits like enhanced scalability, advanced workloads support, and how to deploy bigger containers using various Azure tools.<!--excerpt_end-->

# General Availability of Larger Container Sizes on Azure Container Instances

Azure Container Instances (ACI) has announced the general availability of larger container sizes, allowing customers to deploy workloads with higher vCPU and memory configurations. This advance means standard containers can now utilize up to 31 vCPU and 240 GB of memory, while confidential containers may access up to 31 vCPU and 180 GB.

## Key Benefits

### Enhanced Performance

- More vCPUs and memory for improved processing power and efficiency
- Faster processing times and reduced latency
- Ability to work with larger datasets and more demanding workloads

### Simplified Scalability

- Easier scaling by using fewer, larger containers rather than many smaller ones
- Accommodate growing business needs while maintaining performance

## Example Scenarios

- **Data Inferencing:** Supports high-compute tasks like real-time fraud detection, predictive maintenance, and recommendation engines
- **Collaborative Analytics:** Enables secure, multi-party analytics in scenarios such as healthcare and research
- **Big Data Processing:** Ideal for customer analytics, sentiment analysis, and risk assessment with large data volumes
- **High-Performance Computing (HPC):** Suitable for climate modeling, genomic research, and computational simulations that need substantial compute resources

## Getting Started with Larger Container Sizes

1. **Request Quota:** For containers larger than 4 vCPU and 16 GB, submit a quota increase [request](https://portal.azure.com/#blade/Microsoft_Azure_Support/HelpAndSupportBlade/newsupportrequest).
2. **Deploy Containers:** Once the quota is approved, you can deploy large containers via:
   - [Azure Portal](https://learn.microsoft.com/en-us/azure/container-instances/container-instances-quickstart-portal)
   - [Azure CLI](https://learn.microsoft.com/en-us/azure/container-instances/container-instances-quickstart)
   - [PowerShell](https://learn.microsoft.com/en-us/azure/container-instances/container-instances-quickstart-powershell)
   - [Bicep](https://learn.microsoft.com/en-us/azure/container-instances/container-instances-quickstart-bicep?tabs=CLI)
   - [ARM Template](https://learn.microsoft.com/en-us/azure/container-instances/container-instances-quickstart-template)

## Further Reading

- [Azure Container Instances Overview](https://learn.microsoft.com/en-us/azure/container-instances/container-instances-overview)

For more information about using containers in Azure, visit the [Apps on Azure Blog](/category/azure/blog/appsonazureblog).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-of-larger-container-sizes-on/ba-p/4463863)
