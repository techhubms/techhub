---
external_url: https://devblogs.microsoft.com/azure-sdk/introducing-azure-openai-realtime-api-support-in-javascript/
title: Introducing Azure OpenAI Realtime API Support in JavaScript
author: Deyaaeldeen Almahallawi
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2025-02-05 05:35:07 +00:00
tags:
- Authentication
- Azure OpenAI
- Azure SDK
- DefaultAzureCredential
- Event Handling
- JavaScript
- Node.js
- OpenAI
- OpenAI Library
- Realtime API
- Streaming Data
- Typescript
- WebSocket
section_names:
- ai
- azure
- coding
---
Deyaaeldeen Almahallawi explains how developers can leverage the new Realtime API in the Azure OpenAI JavaScript library for fast, interactive applications.<!--excerpt_end-->

## Overview

Deyaaeldeen Almahallawi’s article introduces the new Realtime API support for Azure OpenAI through the official OpenAI JavaScript library (v4.81.0). This feature allows developers to achieve instant, bidirectional communication with Azure OpenAI models, enabling highly interactive and responsive application experiences.

## Why Realtime API Matters

Realtime APIs provide immediate feedback, which is crucial for scenarios such as live chatbots, speech-to-speech services, streaming data processors, and monitoring tools. These capabilities enhance user engagement by minimizing latency and delivering a seamless, real-time experience.

## Getting Started

To utilize this functionality, developers need:

- Node.js (for Node environments),
- An Azure subscription with access to Azure OpenAI,
- The latest OpenAI JavaScript package and supporting libraries.

### Installation

Install dependencies with:

```sh
npm install openai @azure/identity dotenv
```

### Environment Setup

Create an `.env` file to securely store your Azure endpoint:

```
AZURE_OPENAI_ENDPOINT="<Your-Azure-OpenAI-Endpoint>"
```

## Code Walkthrough

### 1. Import Required Modules

```js
import { OpenAIRealtimeWS } from 'openai/beta/realtime/websocket';
import { AzureOpenAI } from 'openai';
import { DefaultAzureCredential, getBearerTokenProvider } from '@azure/identity';
import 'dotenv/config';
```

### 2. Authenticate

Use `DefaultAzureCredential` for seamless authentication. This class selects the relevant Azure credentials based on your environment.

```js
const cred = new DefaultAzureCredential();
const scope = 'https://cognitiveservices.azure.com/.default';
const azureADTokenProvider = getBearerTokenProvider(cred, scope);
```

### 3. Initialize the Client

Set up the Azure OpenAI client using deployment name and API version:

```js
const deploymentName = 'gpt-4o-realtime-preview-1001';
const client = new AzureOpenAI({
  azureADTokenProvider,
  apiVersion: '2024-10-01-preview',
  deployment: deploymentName,
});
```

### 4. Establish WebSocket Connection

```js
const rt = await OpenAIRealtimeWS.azure(client);
```

### 5. Handle Events

#### Open Connection

```js
rt.socket.on('open', () => {
  rt.send({ type: 'session.update', session: { modalities: ['text'], model: 'gpt-4o-realtime-preview' } });
  rt.send({ type: 'conversation.item.create', item: { type: 'message', role: 'user', content: [{ type: 'input_text', text: 'Say a couple paragraphs!' }] } });
  rt.send({ type: 'response.create' });
});
```

#### Capture Responses

```js
rt.on('session.created', (event) => { console.log('session created!', event.session); });
rt.on('response.text.delta', (event) => process.stdout.write(event.delta));
rt.on('response.text.done', () => console.log());
rt.on('response.done', () => rt.close());
rt.socket.on('close', () => console.log('\nConnection closed!'));
```

#### Error Handling

```js
rt.on('error', (err) => { console.error('An error occurred:', err); });
```

## Building Real-Time Apps

The article details how to manage session lifecycle events, handle streaming responses, and ensure clean connection management. By leveraging these APIs, developers can easily build and extend real-time scenarios like conversational bots, dynamic analytics dashboards, or AI-powered assistants.

## Resources & Next Steps

- **Sample projects:** [Azure OpenAI Realtime GitHub Examples](https://github.com/openai/openai-node/tree/master/examples/azure/realtime)
- **Realtime audio integration guide:** [Documentation](https://learn.microsoft.com/azure/ai-services/openai/how-to/realtime-audio)
- **Official API reference:** [Learn more](https://learn.microsoft.com/azure/ai-services/openai/realtime-audio-reference)

## Conclusion

The addition of Realtime API support in the Azure OpenAI JavaScript library significantly expands what's possible for low-latency, interactive solutions. The guide provides a solid starting point for developers to integrate and iterate on these capabilities in their own applications, supported by sample code and comprehensive links for further exploration.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/introducing-azure-openai-realtime-api-support-in-javascript/)
