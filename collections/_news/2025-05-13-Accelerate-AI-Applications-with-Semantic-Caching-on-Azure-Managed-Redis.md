---
external_url: https://devblogs.microsoft.com/all-things-azure/accelerate-ai-applications-with-semantic-caching-on-azure-managed-redis/
title: Accelerate AI Applications with Semantic Caching on Azure Managed Redis
author: Satish
feed_name: Microsoft DevBlog
date: 2025-05-13 21:18:06 +00:00
tags:
- Active Geo Replication
- AI Apps
- All Things Azure
- API Management
- Azure Cache For Redis
- Azure Managed Redis
- Azure OpenAI
- Azure Redis
- LLM
- Python
- Redis
- Semantic Caching
- Vector Embeddings
- Vector Search
- Zone Redundancy
section_names:
- ai
- azure
- coding
---
In this blog post, Satish discusses how to implement semantic caching on Azure Managed Redis for AI applications, combining vector search and caching strategies for enhanced performance.<!--excerpt_end-->

## Accelerate AI Applications with Semantic Caching on Azure Managed Redis

**Author:** Satish  

### Introduction

Azure has provided caching solutions for more than a decade, notably with its Azure Cache for Redis enterprise offering. This high-performance, scalable cache has enabled developers to significantly improve application responsiveness. With the public preview of Azure Managed Redis (AMR), Azure leverages the Redis enterprise stack to deliver advanced features:

- Active Geo-Replication
- Vector Storage & Search
- Semantic Caching
- Automatic Zone Redundancy
- Entra ID authentication support (on all SKUs and Tiers)

### Why Semantic Caching?

Redis use cases have expanded from traditional scenarios (data caching, API response caching, session storage) to modern AI application requirements, including vector storage/search and semantic caching. Large Language Model (LLM) operations often incur high latency (due to model generation time) and high cost (due to per-token pricing). Semantic caching mitigates these issues by storing the LLM output with both its original text and vector representation of the query. When similar queries are received, the cache is checked using vector similarity, which can avoid redundant LLM API calls. This pattern is especially useful for:

- Faster FAQ retrieval in chatbots via vector search before triggering LLM completion
- Reusing previous user interactions and context for more relevant, faster responses

### Architecture Overview

![redis semantic caching image](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2025/05/redis-semantic-caching-scaled.jpg)

- The AI app checks Azure Managed Redis' semantic cache prior to invoking the LLM's chat completion API.
- Cache lookup uses vector search, based on the embedding of the incoming user query.
- The embedding is retrieved by using the LLM's embedding API.
- Semantic caching logic can also be incorporated in Azure API Management (APIM) with built-in semantic caching policies.

### Python App Example with Semantic Cache

Here's how a Python application implements the semantic caching pattern:

**1. Setup:**

- Import libraries
- Configure credentials for Azure OpenAI and Azure Redis

![REDIS blog image 1 image](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2025/05/REDIS-blog-image-1.png)

**2. Create a Semantic Cache Index:**

- Set Azure OpenAI Embedding API as the embedding provider
- Configure the semantic cache distance threshold (range: 0 to 1)

![REDIS blog image 2 image](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2025/05/REDIS-blog-image-2.png)

**3. Application Logic:**

- Prompt user for input
- Look up the cache for semantically similar prompts
- If a match is found, return response from cache
- Else, call Azure OpenAI for chat completion, and store the output in cache

![REDIS blog image 3 image](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2025/05/REDIS-blog-image-3.png)

**Test Run 1:** Query is submitted, no semantic match—call to Azure OpenAI is made, response returned in ~3 seconds.

![REDIS blog image 4 image](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2025/05/REDIS-blog-image-4.png)
![REDIS blog image 5 image](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2025/05/REDIS-blog-image-5.png)

**Test Run 2:** Another (differently worded but semantically similar) query is run, and a semantic cache hit is found, returning response in <200ms (over 10x faster).

![REDIS blog image 6 image](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2025/05/REDIS-blog-image-6.png)
![REDIS blog image 7 image](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2025/05/REDIS-blog-image-7.png)

### Benefits and Recommendations

Using semantic search in Azure Managed Redis can reduce both latency and cloud costs in intelligent applications. The integration with Azure OpenAI for vector search enables effective caching in AI-powered scenarios, optimizing user experience and scalability.

### Resources

- [Vector Embeddings and Vector Search in Azure Redis](https://aka.ms/RedisVectorSimilarity)
- [Tutorial: Semantic Cache in Azure Redis](https://learn.microsoft.com/en-us/azure/redis/tutorial-semantic-cache)
- [Sample application code repository](https://github.com/sdadha/azureredis-semanticcache/tree/main)

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/accelerate-ai-applications-with-semantic-caching-on-azure-managed-redis/)
