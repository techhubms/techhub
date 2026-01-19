---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-adaptive-multilingual-apps-using-typescript-and-azure/ba-p/4465267
title: Building Adaptive Multilingual Apps Using TypeScript and Azure AI Translator API
author: Julia_Muiruri
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-06 14:45:00 +00:00
tags:
- API Version 05 01 Preview
- Azure AI Foundry
- Azure AI Translator
- Brand Voice
- Compliance
- Gender Specificity
- GPT 4o
- GPT 4o Mini
- Hybrid Translation
- Large Language Model
- LLM
- Localization
- Migration
- Multilingual Apps
- Neural Machine Translation
- NMT
- REST API
- Tone Adaptation
- Translation API
- TypeScript
section_names:
- ai
- azure
- coding
---
Julia Muiruri guides developers through leveraging the 2025-05-01-preview Azure AI Translator API in TypeScript applications, with a focus on adaptive translation, LLM integration, and migration best practices.<!--excerpt_end-->

# Building Adaptive Multilingual Apps Using TypeScript and Azure AI Translator API

*Author: Julia Muiruri*

Azure AI Translator enhances multilingual capabilities for modern applications via a REST API, introducing the 2025-05-01-preview version with new Large Language Model (LLM) translation options that expand beyond traditional Neural Machine Translation (NMT). This release provides TypeScript developers with greater control over translation style, tone, and gender, as well as hybrid flows that optimize for cost, speed, and linguistic nuance.

## Key Features in 2025-05-01-preview

1. **LLM Translation Support:**
    - Option to select GPT-4o or GPT-4o-mini model deployments to achieve richer, context-sensitive translations (requires Azure AI Foundry resource).
    - Enhance outputs with tone (formal/informal/neutral) and gender (male/female/neutral) customization.
2. **Adaptive Customization:**
    - Supports up to five reference translation pairs or datasets for few-shot style adaptation—valuable for organizations seeking compliance with brand voice or regulatory wording.
3. **Hybrid Translation Flows:**
    - Combine NMT and LLM targets in a single request, making it possible to tailor strategies for high-volume versus high-precision scenarios and optimize billing (character-based for NMT, token-based for LLM).
4. **Migration Improvements:**
    - Transitioning from v3 to preview requires request body structure changes, new API params, deprecation of some features, and updated error logging and response handling.

## Example Scenario

A support agent (English) at Zava, a home improvement retailer, chats with a French-speaking customer. Key translation tasks include tone adaptation (formal for official support, casual on community forums), gender specificity, generating variants for QA or compliance, and providing NMT fallback if LLM is unavailable or unsupported.

## Sample Code Snippets (TypeScript)

### Neural Machine Translation (NMT)

```typescript
async function translateWithNMT(text: string, from: string, to: string) {
  const headers = {
    'Ocp-Apim-Subscription-Key': process.env.API_KEY!,
    'Ocp-Apim-Subscription-Region': process.env.REGION!,
    'Content-type': 'application/json',
  };
  const params = new URLSearchParams({ 'api-version': '2025-05-01-preview' });
  const body = [
    { text: "J'ai un problème avec ma commande.", language: "fr", targets: [{ language: "en" }] }
  ];
  const response = await axios.post(`${globalEndpoint}/translate`, body, { headers, params });
  return response.data;
}
```

Expected output: English translation — _I have a problem with my order._

### LLM Translation with Tone & Gender

```typescript
async function translateWithLLM(text: string, from: string, to: string, model: string, tone: string, gender: string) {
  const headers = {
    'Ocp-Apim-Subscription-Key': process.env.FOUNDRY_API_KEY!,
    'Ocp-Apim-Subscription-Region': process.env.REGION!,
    'Content-type': 'application/json',
  };
  const params = new URLSearchParams({ 'api-version': '2025-05-01-preview' });
  const body = [
    {
      text: "Your case has been forwarded to the support supervisor, April Gittens. She will contact you today to review the situation.",
      language: "en",
      targets: [
        { language: "fr", deploymentName: "gpt-4o-mini", tone: "formal", gender: "female" },
        { language: "fr", deploymentName: "gpt-4o-mini", tone: "formal", gender: "male" },
        { language: "fr" }
      ]
    }
  ];
  const response = await axios.post(`${globalEndpoint}/translate`, body, { headers, params });
  return response.data;
}
```

Expected output: French translations with male/female variation and tone adaptation.

## Migration Guide (v3 ➔ 2025-05-01-preview)

- Refactor requests to use an array of objects with `text`, `language`, and `targets` fields.
- Remove features like BreakSentence, Detect, and Dictionary operations.
- Add Foundry API keys for LLM use.
- Validate language codes and update error handling to fit new schema.
- Monitor both character and token usage.

## Data Privacy & Compliance Considerations

- Select regional endpoints to control data geography; the global endpoint might route across regions during outages.
- LLM deployments are configured by geography (global, data zone, regional) during setup for regulatory compliance.
- Enable Virtual Network (VNET) and private endpoints for stricter security; note that these do not support global endpoints or token authentication.

## Resources and Further Reading

- [Azure AI Translator Code Examples (Python & TypeScript)](https://github.com/Azure-Samples/insideAIF/tree/main/Samples/Translator-API)
- [Azure AI Translator Preview Overview](https://learn.microsoft.com/azure/ai-services/translator/text-translation/preview/overview)
- [Translate API (Preview) Documentation](https://learn.microsoft.com/azure/ai-services/translator/text-translation/preview/translate-api)
- [Migration Guide](https://learn.microsoft.com/azure/ai-services/translator/text-translation/how-to/migrate-to-preview)
- [Create Translator Resources](https://learn.microsoft.com/azure/ai-services/translator/how-to/create-translator-resource?tabs=foundry)
- [Supported Language Codes](https://learn.microsoft.com/azure/ai-services/translator/language-support)

---

*Updated Oct 30, 2025*

Authored by Julia Muiruri. For more detailed walkthroughs or to see the complete code repository, follow the resources above.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-adaptive-multilingual-apps-using-typescript-and-azure/ba-p/4465267)
