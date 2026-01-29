---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/secure-azure-stream-analytics-jobs-in-dedicated-clusters-using/ba-p/4470385
title: Secure Azure Stream Analytics Jobs with Dedicated Clusters and Managed Private Endpoints
author: PratibhaShenoy
feed_name: Microsoft Tech Community
date: 2025-11-17 10:31:44 +00:00
tags:
- Automation
- Azure Portal
- Azure Stream Analytics
- Blob Storage
- Cloud Security
- Data Pipeline
- Dedicated Cluster
- Event Processing
- IAM
- Input/Output Configuration
- JSON Serialization
- Managed Identity
- Managed Private Endpoint
- Private Link
- Role Assignment
- Stream Analytics Job
- Terraform
- Zero Trust
- Azure
- Coding
- DevOps
- Security
- Community
section_names:
- azure
- coding
- devops
- security
primary_section: coding
---
PratibhaShenoy provides practical guidance for securely configuring Azure Stream Analytics jobs in dedicated clusters using managed identities and private endpoints, emphasizing zero-trust, compliance, and automation.<!--excerpt_end-->

# Secure Azure Stream Analytics Jobs in Dedicated Clusters Using Managed PE for Blob Input/Output

Azure Stream Analytics enables organizations to build secure, scalable data pipelines by running jobs in dedicated clusters and connecting to resources through managed private endpoints. This guide details each configuration step, including managed identity assignment, blob storage setup, and Terraform automation.

## Introduction

Modern analytics pipelines require security and compliance. Azure Stream Analytics' dedicated clusters and managed private endpoints allow jobs to execute in isolated environments, eliminating public network exposure.

### Key Benefits

- **Private connectivity** using Azure Private Link
- **Managed Identity** for secure, passwordless authentication
- **Dedicated clusters** for resource isolation and scalability

## Architecture Overview

The architecture consists of:

- A Stream Analytics job hosted in a dedicated cluster
- Secure connectivity to Blob Storage using a managed private endpoint
- Managed identity for authentication

## Prerequisites

- Active Azure subscription
- Blob Storage account with public network access disabled
- Input and output containers configured
- User Assigned Managed Identity created

## Implementation Steps

### 1. Assign Managed Identity

- Go to Storage Account → Access Control (IAM)
- Assign 'Storage Blob Data Contributor' role to your User Assigned Managed Identity at the storage account scope (adjust to container if needed for granular access)

### 2. Configure Stream Analytics Job

- In Azure Portal, create or select a Stream Analytics Job
  - Name: e.g., `stream-job`
  - Hosting: Cloud
  - Streaming units: 1
- Enable Managed Identity; assign User Assigned Managed Identity

### 3. Configure Stream Analytics Cluster

- In Azure Portal, create/select a Stream Analytics Cluster
  - Name: e.g., `stream-cluster`
  - Streaming units: as needed (e.g., 12)
  - Location: Match Blob Storage and Stream Analytics Job

### 4. Add Stream Analytics Job to Cluster

- In Stream Analytics Cluster → Settings → Stream Analytics Jobs, add your job

### 5. Add Managed Private Endpoint

- In Cluster → Settings → Managed Private Endpoints, add, and connect Blob Storage as the target resource
- Approve private endpoint on the Blob Storage resource

### 6. Configure Blob Input

- Input alias: `InputStream`
- Container: `input-container`
- Serialization: JSON
- Encoding: UTF-8

### 7. Configure Blob Output

- Output alias: `BlobOutput`
- Container: `output-container`
- Path pattern: `output/{date}/{time}`
- Serialization: JSON

### 8. Prepare Sample Input

Create `sample-input.json` with the following records:

```json
[
  {"DeviceId": "sensor-001", "Temperature": 28.5, "Humidity": 65, "EventEnqueuedUtcTime": "2025-10-30T10:00:00Z"},
  {"DeviceId": "sensor-002", "Temperature": 30.2, "Humidity": 60, "EventEnqueuedUtcTime": "2025-10-30T10:01:00Z"},
  {"DeviceId": "sensor-001", "Temperature": 29.0, "Humidity": 64, "EventEnqueuedUtcTime": "2025-10-30T10:02:00Z"},
  {"DeviceId": "sensor-003", "Temperature": 27.8, "Humidity": 70, "EventEnqueuedUtcTime": "2025-10-30T10:03:00Z"}
]
```

### 9. Define Query

Test the following query:

```sql
SELECT DeviceId, AVG(Temperature) AS AvgTemperature, COUNT(*) AS ReadingCount, System.Timestamp AS WindowEndTime
INTO BlobOutput
FROM InputStream TIMESTAMP BY EventEnqueuedUtcTime
GROUP BY DeviceId, TumblingWindow(minute, 5)
```

### 10. Start and Validate

- Start job
- Upload sample data to `input-container/sample-input.json`
- Monitor input/output events; verify result files in output-container

## Troubleshooting

- If Input Events = 0, verify the path pattern and folder structure
- Confirm role assignment at correct scope
- Managed Private Endpoint: ensure setup is complete and 'Test connection' is successful for both input and output

## Automation with Terraform

Snippet for key resources:

```hcl
resource "azurerm_resource_group" "example" { name = "asa-rg" location = "Central US" }
resource "azurerm_stream_analytics_cluster" "example" { name = "asa-cluster1" ... }
resource "azurerm_user_assigned_identity" "example" { ... }
resource "azurerm_role_assignment" "example" { ... }
resource "azurerm_stream_analytics_job" "example" { ... }
resource "azurerm_stream_analytics_managed_private_endpoint" "example" { ... }
resource "azurerm_stream_analytics_stream_input_blob" "example" { ... }
resource "azurerm_stream_analytics_output_blob" "example" { ... }
resource "azurerm_stream_analytics_job_schedule" "example" { ... }
```

## Disclaimer

This article provides general best practices and configuration guidance for Azure Stream Analytics jobs in production. Always validate steps in your own environment and consult current Microsoft documentation at <https://learn.microsoft.com/azure/> as cloud services evolve.

## References

- [Azure Stream Analytics Documentation](https://learn.microsoft.com/en-gb/azure/stream-analytics/)
- [Terraform azurerm_stream_analytics_cluster](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/stream_analytics_cluster)

---
*Author: PratibhaShenoy*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/secure-azure-stream-analytics-jobs-in-dedicated-clusters-using/ba-p/4470385)
