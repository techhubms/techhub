---
external_url: https://blog.fabric.microsoft.com/en-US/blog/transform-sensitive-text-into-ai-ready-data-on-microsoft-fabric/
title: Transform Sensitive Text into AI-Ready Data on Microsoft Fabric
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-21 10:00:00 +00:00
tags:
- AI Development
- Anonymization
- Azure AI Foundry
- Azure AI Search
- Azure Machine Learning
- Compliance
- Data Governance
- Data Privacy
- Data Synthesis
- Entity Detection
- GDPR
- HIPAA
- Lakehouse
- Microsoft Fabric
- Redaction
- Sensitive Data
- Tonic Textual
- Unstructured Data
- AI
- Azure
- ML
- News
- Machine Learning
section_names:
- ai
- azure
- ml
primary_section: ai
---
Microsoft Fabric Blog explains how to leverage Tonic Textual for secure AI/ML development by automatically preparing sensitive unstructured text within Fabric, maintaining privacy and compliance throughout the workflow.<!--excerpt_end-->

# Transform Sensitive Text into AI-Ready Data on Microsoft Fabric

Organizations face significant challenges unlocking sensitive unstructured text data for responsible and compliant AI/ML development. This guide demonstrates how integrating Tonic Textual into Microsoft Fabric empowers teams to process Office documents, PDFs, and images containing confidential information directly within their existing Fabric ecosystem.

## Key Features

- **Automated Entity Detection & Redaction:** Tonic Textual detects personal identifiers (names, dates, medical/financial details) in unstructured data, allowing users to anonymize or synthesize content securely.
- **Compliant Data Preparation:** Process sensitive data within governed Fabric environments, maintaining compliance with regulations like HIPAA and GDPR.
- **End-to-End Integration:** Workflows run natively in Fabric, eliminating manual off-platform preprocessing and minimizing risk.
- **Unlock Downstream Analytics:** Privacy-preserving datasets become immediately usable for model training, generative AI, retrieval-augmented generation (RAG), and analytics apps.

## Step-by-Step Workflow

### 1. Add the Tonic Textual Workload

From your Fabric console, add Tonic Textual as a workload to your workspace. This makes the Textual UI accessible directly within Microsoft Fabric.

- For details on adding workloads, see the [official documentation](https://learn.microsoft.com/fabric/workload-development-kit/more-workloads-add).

### 2. Configure Input and Output Locations

Select the source Lakehouse containing files to process and specify a destination folder for sanitized outputs.

### 3. Create a Tonic Textual Item

- Open your workspace, create a new item, and select 'Tonic Textual'.
- Choose files or folders to process; these will be analyzed for sensitive data.

### 4. Scan Files for Sensitive Text

- The tool scans and detects a range of sensitive entities within the chosen files.
- Detected items (e.g., names, dates) are displayed for review.

### 5. Set De-identification Preferences

- Decide whether to redact, synthesize (replace with realistic surrogates), or leave entities unchanged.
- Bulk edit options enable large-scale processing across datasets.

### 6. Access Sanitized Outputs

- Sanitized files are saved in the specified destination and are replicas with sensitive data safely redacted or synthesized.
- Original files remain untouched in their source location.

### 7. Enable AI/ML Workflows

- Use your newly sanitized data for:
  - Building AI agents with [Azure Copilot Studio](https://www.microsoft.com/microsoft-365-copilot/microsoft-copilot-studio)
  - AI-powered search via [Azure AI Search](https://azure.microsoft.com/products/ai-services/ai-search/)
  - Training custom ML models in [Azure Machine Learning](https://azure.microsoft.com/products/machine-learning/)

## Why This Matters

- **Compliance-First:** Keeps privacy-sensitive data within your Microsoft Fabric environment
- **Seamless Integration:** Eliminates risky manual transfers or external preprocessing
- **Acceleration:** Enables rapid development of compliant, AI-ready datasets
- **Enterprise-Ready:** Scales with organizational needs and Fabric’s governance framework

## Learn More

- Explore integration details and further resources at [Tonic.ai’s Microsoft Fabric partner page](https://www.tonic.ai/partners/microsoft/fabric)
- Attend upcoming sessions at Microsoft Ignite for live demonstrations

By leveraging Tonic Textual within Fabric, organizations move from blocked to AI-ready, ultimately driving innovation while maintaining privacy and regulatory compliance.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/transform-sensitive-text-into-ai-ready-data-on-microsoft-fabric/)
