---
layout: post
title: Unlock Structured OCR in TypeScript with Mistral Document AI on AI Foundry
author: Julia_Muiruri
canonical_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/unlock-structured-ocr-in-typescript-with-mistral-document-ai-on/ba-p/4466039
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-06 14:15:35 +00:00
permalink: /ai/community/Unlock-Structured-OCR-in-TypeScript-with-Mistral-Document-AI-on-AI-Foundry
tags:
- AI
- AI Agents
- API Integration
- Azure
- Azure AI Foundry
- Azure Security
- Coding
- Community
- Data Extraction
- Document Automation
- Document Understanding
- Handwriting Recognition
- JSON
- Markdown
- Mistral Document AI
- Multimodal AI
- OCR
- PDF Processing
- RAG Pipelines
- Responsible AI
- Serverless
- TypeScript
section_names:
- ai
- azure
- coding
---
Julia Muiruri explores how to leverage Mistral Document AI on Azure AI Foundry to perform advanced OCR and convert scanned documents into structured data, with code samples and Azure-focused best practices.<!--excerpt_end-->

# Unlock Structured OCR in TypeScript with Mistral Document AI on AI Foundry

Mistral Document AI—now available as a serverless model in Azure AI Foundry—enables developers to extract structured, layout-preserving data from scanned PDFs and images. Compared to legacy OCR, it provides markdown outputs and JSON annotations, retaining tables, headings, figures, and multilingual content with high accuracy. This unlocks better automation and reliability for AI agents, Retrieval-Augmented Generation (RAG), and compliance.

## Key Features

- **Layout-aware document understanding** using vision + language models
- Preserves semantic structure (tables, headings, figures) in markdown and JSON
- Handles 25+ languages and diverse handwriting/fonts
- Outperforms traditional OCR on both layout fidelity and extraction accuracy
- Supports batch and single-document workflows in a serverless Azure environment
- Integrates with Azure AI controls for security, compliance, and governance

## Typical Workflow Example

Imagine digitizing a collection of handwritten or scanned family recipes for use in meal planners or shopping tools:

1. **Ingest** a document (PDF or image) and call Mistral Document AI
2. **Receive** structured markdown and JSON output preserving table, header, and figure boundaries
3. **Parse** the markdown to extract recipe details (title, ingredients, steps, cooking time)
4. **Generate** a consolidated shopping list for downstream agent or app use

## Code Walkthrough: TypeScript Integration

### Pre-requisites

- An Azure AI Foundry project
- Deployed `mistral-document-ai-2505` model
- Set your endpoint and API key as environment variables

### Minimal Example

```typescript
// Step 1: Encode PDF as base64
function encodePdfToBase64(pdfPath: string): string {
  const pdfContent = fs.readFileSync(pdfPath);
  return pdfContent.toString('base64');
}
const base64Content = encodePdfToBase64(pdfPath);

// Step 2: Invoke Azure endpoint
const headers = {
  'Content-Type': 'application/json',
  Authorization: `Bearer ${apiKey}`,
};
const payload = {
  model: modelName,
  document: {
    type: 'document_url',
    document_url: `data:application/pdf;base64,${base64Content}`,
  },
  include_image_base64: true,
};
const response = await axios.post(azureEndpoint, payload, { headers });

// Step 3: Save output for further processing
fs.writeFileSync('document_ai_result.json', JSON.stringify(response.data, null, 2), 'utf-8');
const resultJson = fs.readFileSync('document_ai_result.json', 'utf-8');
```

Output includes structured recipe information (ingredients, steps, time) in markdown and JSON, ready for app integration or further AI-driven processing.

## Security, Privacy, and Compliance

Running on Azure AI Foundry ensures all document data stays in your Azure region, never leaving for third-party endpoints. Enterprise customers benefit from Azure’s isolation/governance controls and can apply Responsible AI tools for content filtering, monitoring, and selective on-prem/self-hosting for sensitive workloads.

## Learn More and Resources

- [Code examples (Python & TypeScript)](https://github.com/Azure-Samples/insideAIF/tree/main/Samples/Mistral-Document-AI)
- [Mistral Document AI 2505 Model Card](https://ai.azure.com/catalog/models/mistral-document-ai-2505)
- [Azure AI Partnership Blog](https://aka.ms/insideAIF/mistral-document-AI)
- [Mistral OCR Announcement](https://mistral.ai/news/mistral-ocr)

Mistral Document AI streamlines document digitization, supports complex layouts and many languages, and integrates securely with Azure cloud tools for responsible, scalable automation.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/unlock-structured-ocr-in-typescript-with-mistral-document-ai-on/ba-p/4466039)
