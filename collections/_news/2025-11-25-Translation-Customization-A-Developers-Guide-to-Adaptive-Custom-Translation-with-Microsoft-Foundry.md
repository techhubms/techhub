---
layout: post
title: 'Translation Customization: A Developer’s Guide to Adaptive Custom Translation with Microsoft Foundry'
author: Mohamed Elghazali
canonical_url: https://devblogs.microsoft.com/foundry/translation-customization-a-developers-guide-to-adaptive-custom-translation/
viewing_mode: external
feed_name: Microsoft AI Foundry Blog
feed_url: https://devblogs.microsoft.com/foundry/feed/
date: 2025-11-25 16:00:37 +00:00
permalink: /ai/news/Translation-Customization-A-Developers-Guide-to-Adaptive-Custom-Translation-with-Microsoft-Foundry
tags:
- Adaptive Custom Translation
- AI
- AI Development
- AI Tools
- API Integration
- Azure
- Azure AI Translator
- Coding
- Custom Translator
- Developer Guide
- Domain Specific Translation
- Few Shot Learning
- Generative AI
- GPT 4o
- Indexing
- Language Services
- LLM
- Microsoft Foundry
- Model Customization
- MSIgnite
- News
- Text Translation
- TMX
- Translation API
- Translation Workflow
section_names:
- ai
- azure
- coding
---
Mohamed Elghazali presents an in-depth developer guide to Adaptive Custom Translation (AdaptCT) in Microsoft Foundry Tools, highlighting streamlined translation customization with Azure-based AI, hand-on API usage, and practical workflow tips.<!--excerpt_end-->

# Translation Customization: A Developer’s Guide to Adaptive Custom Translation

## Introduction

Translation is more than word substitution—it's core to global communication. Businesses demand high-accuracy, real-time multilingual solutions that capture domain-specific language. Mohamed Elghazali introduces Adaptive Custom Translation (AdaptCT) from Microsoft Foundry Tools as a transformative approach to these challenges, leveraging advanced large language models (LLMs) like GPT-4o and smart dataset indexing.

## Challenges with Traditional Custom Translation

- Requires large curated datasets (10,000+ pairs)
- Slow, manual retraining and deployment
- Not agile enough for dynamic, frequently updated content (support, docs, etc.)

## Adaptive Custom Translation (AdaptCT): Key Advantages

- Uses existing LLMs (e.g., GPT-4o, GPT-4o-mini)
- Customizes with small, focused datasets (from as few as 5 sentence pairs)
- Employs smart indexing instead of retraining
- Updates in minutes, not days
- Ideal for low-volume, frequently updated or niche domains

## How AdaptCT Works

1. **Dataset Indexing:** Upload aligned sentence pairs (e.g., English–French) using supported file types (TMX or TSV).
2. **Index Creation:** Generate an index for the uploaded dataset.
3. **On-the-Fly Customization:** When translating, include the dataset index ID. The LLM applies few-shot learning during inference, returning domain-adapted translations.
4. **No Retraining:** New content or terminology is handled by updating the index and uploading revised datasets—no model training downtime.

## Example Workflow: Setting Up AdaptCT

### Step 1: Create a Project

```curl
curl -X POST "https://<your-resource>.cognitiveservices.azure.com/translator/customtranslator/api/texttranslator/v1.0/workspaces" \
  -H "Ocp-Apim-Subscription-Key: <your-key>" \
  -H "Content-Type: application/json" \
  -d '{ "name": "my-translation-project", "subscription": { "billingRegionCode": "<billing-region>", "subscriptionKey": "<your-key>" } }'
```

### Step 2: Upload Translation Pairs

For example, upload a TMX or TSV file with aligned sentences.

```curl
curl -X POST "https://<your-resource>.cognitiveservices.azure.com/translator/customtranslator/api/texttranslator/v1.0/documents/import?workspaceId=<workspace-id>" \
  -H "Authorization: Bearer <token>" \
  -F "DocumentDetails=[{\"DocumentName\":\"product-terms\",\"DocumentType\":\"Adaptive\",\"FileDetails\":[{\"Name\":\"translations.tmx\",\"LanguageCode\":\"en\",\"OverwriteIfExists\":true}]}]" \
  -F "FILES=@translations.tmx"
```

### Step 3: Create the Index

```curl
curl -X POST "https://<your-resource>.cognitiveservices.azure.com/translator/customtranslator/api/texttranslator/v1.0/index?workspaceId=<workspace-id>" \
  -H "Content-Type: application/json" \
  -d '{ "documentIds": ["123456"], "IndexName": "product-index", "SourceLanguage": "en", "TargetLanguage": "fr" }'
```

### Step 4: Translate Using the Index

- In Microsoft Foundry, configure Text Translation with Adaptive customization.
- Select/deploy GPT-4o or GPT-4o-mini.
- Use the dataset index for instant, domain-specific translations (English ↔ French supported).
- Alternatively, use the [Text Translation API](https://learn.microsoft.com/azure/ai-services/translator/text-translation/preview/translate-api) and pass your index as a parameter.

## Supported Data Formats

- **TMX (Translation Memory eXchange) Example:**

```xml
<?xml version="1.0" encoding="UTF-8"?>
<tmx version="1.4">
  <body>
    <tu>
      <tuv xml:lang="en"><seg>Click the submit button</seg></tuv>
      <tuv xml:lang="fr"><seg>Cliquez sur le bouton soumettre</seg></tuv>
    </tu>
  </body>
</tmx>
```

- **TSV (Tab-Separated Values) Example:**

```
en	fr
Click the submit button	Cliquez sur le bouton soumettre
Upload your file	Téléchargez votre fichier
```

## Decision Matrix: AdaptCT vs. Custom Translator

| Scenario                              | Choice          |
|----------------------------------------|-----------------|
| Low-volume, fast-changing content      | AdaptCT         |
| Need updates within minutes            | AdaptCT         |
| < 10K translation pairs                | AdaptCT         |
| Support, chat, dynamic content         | AdaptCT         |
| High-volume, consistent content        | Custom Translator|
| Strict terminology (legal, medical)    | Custom Translator|
| > 10K translation pairs                | Custom Translator|
| Rarely changing content                | Custom Translator|

## Best Practices

- **Start Small:** Launch with 50–100 key sentence pairs; iterate as needed.
- **Domain Organization:** Use indexes per domain/topic (e.g., support, docs).
- **Focus on Quality:** Prioritize clean, impactful sentence pairs.
- **Versioning:** Track changes and context across updates.
- **Monitor Results:** Use feedback and metrics to refine datasets.
- **Alignment:** Ensure high-quality alignment and context in input files.

## When to Avoid Pitfalls

1. Don’t bulk-upload many low-value pairs “just in case.”
2. Keep TMX/TSV pairs well-aligned and context-rich.
3. Provide full sentence context over isolated words.
4. Use blind-testsets to validate translation quality on updates.

## Benefits for Developers and Businesses

- Dramatically faster delivery of multilingual features
- Lower translation costs
- Highly agile and maintainable translation pipelines
- Reliable, continually-updated terminology

## Additional Resources

- [GitHub Repository](https://github.com/MicrosoftTranslator/adapct4python)
- [YouTube Demo](https://youtu.be/K30exeRKGdw)
- [Azure AI Translator Documentation](https://learn.microsoft.com/azure/ai-services/translator/)
- [Text Translation API Preview](https://learn.microsoft.com/azure/ai-services/translator/text-translation/preview/overview)
- [Azure AI Foundry](https://azure.microsoft.com/products/ai-foundry)
- [Custom Translator Overview](https://learn.microsoft.com/azure/ai-services/translator/custom-translator/overview)

---

Adaptive Custom Translation enables developers to deliver smarter, faster, and context-aware translations for global applications using Microsoft’s Azure AI services.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/translation-customization-a-developers-guide-to-adaptive-custom-translation/)
