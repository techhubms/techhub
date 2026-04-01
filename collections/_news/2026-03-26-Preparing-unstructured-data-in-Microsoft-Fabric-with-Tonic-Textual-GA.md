---
external_url: https://blog.fabric.microsoft.com/en-US/blog/from-restricted-to-ai-ready-preparing-unstructured-data-directly-in-microsoft-fabric-with-tonic-textual-generally-available/
section_names:
- ai
- ml
- security
date: 2026-03-26 10:00:00 +00:00
primary_section: ai
title: Preparing unstructured data in Microsoft Fabric with Tonic Textual (GA)
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
tags:
- AI
- Data De Identification
- Fabric Workload Hub
- Masking
- Microsoft Fabric
- Microsoft Foundry
- ML
- Model Evaluation
- Model Training
- News
- OneLake
- Privacy
- Protected Health Information (phi)
- Redaction
- Retrieval Augmented Generation (rag)
- Security
- Sensitive Data Detection
- Synthetic Data
- Text Data Preparation
- Tonic Textual
- Unstructured Data
---

Microsoft Fabric Blog announces the general availability of Tonic Textual as a Microsoft Fabric workload, focused on detecting and transforming sensitive entities in unstructured text stored in OneLake so teams can safely prepare AI-ready datasets for training, evaluation, and retrieval workflows in Fabric/Foundry.<!--excerpt_end-->

## Summary

AI teams often need to use unstructured text (tickets, contracts, call transcripts, docs, internal comms), but that data frequently contains sensitive information that restricts access. This post announces **Tonic Textual (Generally Available)** as a workload in the **Microsoft Fabric Workload Hub**, aimed at preparing **AI-ready** text datasets **inside Fabric**.

## Why unstructured text is hard to use for AI

- Enterprises have lots of valuable context in text sources like:
  - Support tickets
  - Contracts
  - Call transcripts
  - Documentation
  - Internal communications
- Those sources can contain sensitive data (personal identifiers, financial details, confidential info), so access is often restricted.

The post references a Gartner prediction that by 2026 many AI projects will be abandoned due to lack of AI-ready data:

- Gartner press release: https://www.gartner.com/en/newsroom/press-releases/2025-02-26-lack-of-ai-ready-data-puts-ai-projects-at-risk

## What Tonic Textual in Fabric does

**Tonic Textual** runs as a **workload within Microsoft Fabric** and:

- Scans unstructured files stored in **Microsoft OneLake**
- Detects sensitive entities (examples given: names, identifiers, financial information)
- Lets you configure transformations such as:
  - Redacting sensitive values
  - Masking specific identifiers
  - Replacing values with synthetic data
  - Applying custom rules for specific entity types

Prepared datasets stay in Fabric/OneLake and can be used for AI development workflows, including:

- Model training
- Model evaluation
- Application development with **Microsoft Foundry**

Key point: doing this *inside Fabric* avoids moving data outside the platform.

## Keeping text usable after de-identification

The post calls out that transformations should preserve structure and context so downstream use still works:

- Support conversations retain dialogue format
- Contracts retain structure/clauses
- Documentation retains technical context

Applying the same transformation rules across datasets also helps enforce consistent privacy policies.

## Example workflow in Fabric

1. **Add the workload**
   - Install Tonic Textual from the Fabric Workload Hub:
   - https://app.fabric.microsoft.com/workloadhub/detail/Tonic.Textual.Product

   ![Microsoft Fabric Workload Hub showing the Tonic Textual workload tile available to install.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/microsoft-fabric-workload-hub-showing-the-tonic-te.png)

2. **Select data in OneLake**
   - Choose document collections/transcripts/other text datasets stored in OneLake.

   ![Tonic Textual in Fabric prompting the user to select a source folder in OneLake for the text dataset.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/tonic-textual-in-fabric-prompting-the-user-to-sele.png)

3. **Select an output location**
   - Choose a destination folder in OneLake to keep the source unchanged and create a separate prepared dataset.

   ![Tonic Textual in Fabric prompting the user to choose a destination folder in OneLake for the prepared output dataset](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/tonic-textual-in-fabric-prompting-the-user-to-choo.png)

4. **Detect sensitive entities**
   - Scan the dataset and identify sensitive information.

   ![Tonic Textual scan view showing files being analyzed and detected sensitive entities in the dataset.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/tonic-textual-scan-view-showing-files-being-analyz.png)

5. **Configure transformations**
   - Choose how detected entities are handled (redaction, masking, synthetic replacement).

   ![Tonic Textual configuration screen for choosing how detected entities are transformed (for example, redaction, masking, or synthetic replacement).](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/tonic-textual-configuration-screen-for-choosing-ho.png)

6. **Use the prepared data**
   - The output stays in Fabric and can be used in downstream pipelines like training or retrieval systems.

For an end-to-end walkthrough video:

- https://www.youtube.com/watch?v=nDSrGed_CtQ

## Links mentioned

- Tonic Textual product page: https://www.tonic.ai/products/textual
- Tonic Textual in Fabric Workload Hub: https://app.fabric.microsoft.com/workloadhub/detail/Tonic.Textual.Product
- Press release: https://www.tonic.ai/press-releases/tonic-textual-general-availablity-for-fabric
- Docs: https://docs.tonic.ai/textual/fabric-integration/fabric-integration-about

## Note on related Fabric announcements

The post points to a related roundup blog for FabCon/SQLCon announcements:

- “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform”: https://aka.ms/FabCon-SQLCon-2026-news

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/from-restricted-to-ai-ready-preparing-unstructured-data-directly-in-microsoft-fabric-with-tonic-textual-generally-available/)

