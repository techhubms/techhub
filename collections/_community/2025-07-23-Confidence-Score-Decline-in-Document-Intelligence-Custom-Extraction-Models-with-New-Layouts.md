---
external_url: https://techcommunity.microsoft.com/t5/ai-azure-ai-services/doc-intelligence-custom-extraction-model-confidence-score/m-p/4435860#M1270
title: Confidence Score Decline in Document Intelligence Custom Extraction Models with New Layouts
author: aristotelisc
feed_name: Microsoft Tech Community
date: 2025-07-23 15:35:47 +00:00
tags:
- Azure AI
- Azure Document Intelligence
- Confidence Score
- Custom Extraction
- Data Annotation
- Document Layout
- Field Extraction
- Format Variability
- Holdout Set
- Machine Learning Training
- Microsoft Cognitive Services
- Model Evaluation
- Model Generalization
section_names:
- ai
- azure
- ml
primary_section: ai
---
aristotelisc describes an experiment using Azure Document Intelligence to train custom extraction models, highlighting how introducing new document layouts led to persistent confidence score drops. The author seeks community insights on this behavior.<!--excerpt_end-->

# Doc Intelligence: Custom Extraction Model | Confidence Score Deterioration with New Formats/Layouts

**Author:** aristotelisc

Hi everyone,

I am experimenting with custom extraction models in the Azure Document Intelligence service and have encountered a puzzling issue. My goal is to understand how model confidence scores behave when documents of significantly different format or layout are introduced during training.

## Experiment Setup

- **Initial Phase:**
  - Trained models using documents all in the same format (with some minor issues like picture quality and rotation).
  - Gradually increased training data by increments of 5 documents, retraining after each addition.
  - After each retraining, evaluated model confidence against a fixed, unseen holdout set (same format as training data).
- **Second Phase:**
  - Upon reaching 35 documents in the same format, began adding documents with distinctly different formats and layouts (in batches of 10).
  - Retrained the model after each addition.
  - Monitored confidence scores against the unchanged holdout set.

## Observations

- Adding documents with new formats/layouts caused a notable drop in confidence scores when evaluating the original-format holdout set.
- Confidence scores did not recover even with further retraining or more samples added.
- See attached graph for confidence score evolution after each training phase.

## Question for the Community

Has anyone experienced a similar confidence drop when introducing varied input formats to a custom extraction model?

- What are the underlying causes for this behavior—does the model "forget" the original format, or is there an underlying limitation in the training process?
- Are there best practices to mitigate this decline and retain high confidence on multiple formats?

**Any insights or suggestions would be appreciated.**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/ai-azure-ai-services/doc-intelligence-custom-extraction-model-confidence-score/m-p/4435860#M1270)
