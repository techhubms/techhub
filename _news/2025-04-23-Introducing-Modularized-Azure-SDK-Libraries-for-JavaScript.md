---
layout: "post"
title: "Introducing Modularized Azure SDK Libraries for JavaScript"
description: "This announcement details new modularized Azure SDK libraries for JavaScript, explaining their architecture, bundle size optimizations, user experience improvements, and support for both ECMAScript Modules and CommonJS formats. The modular design enables fine-grained imports, better tree-shaking, and enhanced developer flexibility."
author: "Qiaoqiao Zhang"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-sdk-modularized-libraries-for-javascript/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-04-23 16:25:10 +00:00
permalink: "/2025-04-23-Introducing-Modularized-Azure-SDK-Libraries-for-JavaScript.html"
categories: ["Azure", "Coding"]
tags: ["Azure", "Azure SDK", "Bundle Size", "Client Libraries", "Coding", "CommonJS", "ESM", "JavaScript", "Long Running Operations", "Modular", "Modularization", "News", "Pagination", "REST Level Clients", "SDK", "Serialization", "Subpath Exports", "Tree Shaking", "TypeScript"]
tags_normalized: ["azure", "azure sdk", "bundle size", "client libraries", "coding", "commonjs", "esm", "javascript", "long running operations", "modular", "modularization", "news", "pagination", "rest level clients", "sdk", "serialization", "subpath exports", "tree shaking", "typescript"]
---

Qiaoqiao Zhang introduces the new modularized Azure SDK libraries for JavaScript, focusing on bundle size optimization, usability improvements, and architectural enhancements that benefit developers building Azure-integrated applications.<!--excerpt_end-->

# Azure SDK Modularized Libraries for JavaScript

*By Qiaoqiao Zhang*

## Overview

Previously, the Azure SDK team introduced [Azure REST libraries for JavaScript](https://devblogs.microsoft.com/azure-sdk/azure-rest-libraries-for-javascript/), which optimize browser user experience and minimize bundle size. While REST-level clients (RLCs) provide a direct abstraction with reduced code footprint, they aren't always user-friendly for those more accustomed to traditional client libraries. This, combined with the challenge of reducing bundle size in browser apps, led to the development of modularized Azure SDK libraries.

## Modularized Design

To bridge the gap between usability and bundle size, the Azure SDK team is creating new modularized libraries built atop RLCs. These libraries modularize API calls, promoting consistency and flexibility while reducing bundle size—benefiting both new and traditional client users.

### Subpath Exports

Using [subpath exports](https://nodejs.org/api/packages.html#subpath-exports) (since Node.js v12.7), the libraries enable tailored imports, comprising:

- **Modular API Layer**: Each API call is modularized, letting customers import only what they need. It manages serialization for requests and deserialization for responses at the REST layer.
- **Service Client Layer**: Offers the same experience as traditional clients, providing convenience layers built atop the API layer.

#### Sub Clients

In cases with multiple parallel or hierarchical sub clients, each sub client's subpath export includes only the relevant APIs. Users can thus integrate only selected components, optimizing resource use and customizability.

#### Experimental Features

Experimental APIs can be previewed within dedicated subpath exports, giving early feedback and clearly delineating stable versus experimental APIs.

### Opt-in Helpers

Core library dependencies are minimized. Instead, model serialization and deserialization logic moves to the client side as opt-in helpers, making advanced features (e.g., paging, long-running operations) optional and eliminating the resource load for unused code. This supports tree-shaking and lighter bundles.

### ECMAScript Modules (ESM) vs. CommonJS (CJS)

Traditional clients were limited to CommonJS modules. The new libraries leverage [tshy](https://github.com/isaacs/tshy) to support both ESM and CJS, letting developers choose the most compatible format.

## Bundle Size Optimization

Different library layers demonstrate significant bundle size improvements:

| Client Type          | Bundle Size | Optimized Percentage |
|----------------------|-------------|---------------------|
| Traditional Client   | 124.64 KB   | N/A                 |
| Service Client Layer | 91.39 KB    | 26.68%              |
| Modular Layer        | 67.97 KB    | 45.47%              |
| RLC                  | 48.23 KB    | 61.30%              |

**Conclusion**: The modular layer provides meaningful improvements over traditional clients, balancing user experience and bundle size.

## User Experience in Modularized Libraries

As an example, the modularized OpenAI library demonstrates usage patterns for each layer:

- **Authentication** (with `AzureKeyCredential`):

  ```ts
  import { AzureKeyCredential } from "@azure/core-auth"
  const credential = new AzureKeyCredential(azureApiKey);
  ```

- **Endpoint & API Key Configuration:**

  ```ts
  const endpoint = process.env['ENDPOINT'] || '';
  const azureApiKey = process.env['AZURE_API_KEY'] || '';
  ```

- **Model Selection:**

  ```ts
  const deploymentName = 'gpt-35-turbo-1106';
  ```

- **Request Setup:**

  ```ts
  const messages = [{ role: 'user', content: 'What is the weather like in Boston?' }];
  const tools = [
    {
      type: 'function',
      function: {
        name: 'get_current_weather',
        description: 'Get the current weather in a given location',
        parameters: {
          type: 'object',
          properties: {
            location: { type: 'string', description: 'The city and state, e.g. San Francisco, CA' },
            unit: { type: 'string', enum: ['celsius', 'fahrenheit'] },
          },
          required: ['location'],
        },
      },
    },
  ];
  ```

### REST-Level Client (RLC) Usage

```ts
import createOpenAIContext from '@azure-rest/openai';
const client = createOpenAIContext(endpoint, credential);
const result = await client
  .path('/deployments/{deploymentId}/chat/completions', deploymentName)
  .post({ body: { messages, tools } });

// Handle unexpected results
if (isUnexpected(result)) {
  throw createRestError(result.body);
}
```

### Modular API Layer Usage

```ts
import { getChatCompletions, createOpenAIContext } from "@azure/openai/api";
const context = createOpenAIContext(endpoint, credential);
const result = await getChatCompletions(context, deploymentName, messages, { tools });
```

### Service Client Layer Usage

```ts
import { OpenAIClient } from "@azure/openai";
const client = new OpenAIClient(endpoint, credential);
const result = await client.getChatCompletions(deploymentName, messages, { tools });
```

## Key Features for Modularized Libraries

### Support for Complex Hierarchies

- **Parallel Sub Clients**:

  ```ts
  import { LoadTestRunClient } from "@azure/loadtesting/loadTestRun";
  const loadTestRunClient = new LoadTestRunClient();
  loadTestRunClient.getTestRun();
  ```

  ```ts
  import { TestProfileAdministrationClient } from "@azure/loadtesting/testProfileAdministration";
  const profileAdminClient = new TestProfileAdministrationClient();
  profileAdminClient.getTestProfile();
  ```

- **Hierarchical Sub Clients**:

  ```ts
  import { StorageClient } from "@azure/storage";
  const storageClient = new StorageClient(accountName);
  const storageContainerClient = storageClient.getContainerClient(containerId);
  storageContainerClient.upload();
  
  // Or import from subpath:
  import { StorageContainerClient } from "@azure/storage/container";
  const storageContainerClient = new StorageContainerClient(accountName, containerId);
  storageContainerClient.upload();
  ```

- **Model Namespace Hierarchies**:

  ```ts
  import { ChatRequestMessage, ChatResponseMessage } from "@azure/openai/models/chat";
  import { CompletionRequest, CompletionResponse } from "@azure/openai/models/chat/completion";
  import { EmbeddingRequest, EmbeddingResponse } from "@azure/openai/models/chat/embedding";
  ```

### Serialization and Deserialization

Serialization and deserialization logic is decentralized per model, improving bundle optimization and enabling tree-shaking. For instance:

```ts
function windowSerializer(obj: Window): any {
  return { width: obj.width, length: obj.length };
}

export function extensionSerializer(item: Extension): any {
  return {
    extension: !item["extension"] ? item["extension"] : extensionArraySerializer(item["extension"]),
    level: item["level"],
  };
}

export function extensionArraySerializer(result: Array<Extension>): any[] {
  return result.map(extensionSerializer);
}

export function elementSerializer(item: Element): any {
  return {
    extension: !item["extension"] ? item["extension"] : extensionArraySerializer(item["extension"]),
  };
}
```

Customers not needing complex serialization logic (e.g., for `Extension`), can exclude it from their bundle, further optimizing usage.

### Pagination Enhancements

Pagination usage aligns with Azure SDK guidelines and offers finer control:

```ts
const client = new ServiceClient();
for await (const item of client.listItems()) {
  handleItem(item);
}

// For granular pagination
const previousPage = await client.listItems().byPage().next();
const continuationToken = previousPage.value.continuationToken;
for await (const page of client.listItems().byPage({ continuationToken })) {
  handleItem(page);
}
```

### Long-running Operations (LROs)

Design improvements consolidate polling operations:

- Old pattern:

  ```ts
  // Wait for completion
  const result = await beginDoSthAndWait();
  // Manual polling
  const poller = await beginDoSth();
  const result = poller.pollUntilDone();
  ```

- New pattern:

  ```ts
  // Wait for result
  const result = await doSth();
  // Manual polling
  const poller = doSth();
  await poller.submitted();
  const result = await poller;
  // or
  const result = await poller.pollUntilDone();
  ```

## Summary

The modularized Azure SDK libraries for JavaScript provide:

- Significant bundle size reductions
- Modern, composable API imports via subpath exports
- Comprehensive support for both ESM and CJS
- Improved usability and flexibility for various scenarios

This approach gives developers robust, performant tools tailored to specific Azure services and app needs.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-modularized-libraries-for-javascript/)
