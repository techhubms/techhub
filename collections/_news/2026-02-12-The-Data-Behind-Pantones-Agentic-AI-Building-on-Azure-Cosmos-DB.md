---
external_url: https://azure.microsoft.com/en-us/blog/the-data-behind-the-design-how-pantone-built-agentic-ai-with-an-ai-ready-database/
title: "The Data Behind Pantone's Agentic AI: Building on Azure Cosmos DB"
author: Jesse Sullivan
primary_section: ai
feed_name: The Azure Blog
date: 2026-02-12 16:00:00 +00:00
tags:
- Agentic AI
- AI
- AI + Machine Learning
- AI Application
- Azure
- Azure AI
- Azure Cosmos DB
- Cloud Scalability
- Conversational AI
- Databases
- Feedback Loops
- Generative AI
- Internet Of Things
- Microsoft Foundry
- Multiagent Architecture
- News
- Palette Generator
- Pantone
- Real Time Data
- Semantic Search
- Vector Search
section_names:
- ai
- azure
---
Jesse Sullivan details how Pantone leveraged agentic AI, Azure Cosmos DB, and Azure AI services for an MVP Palette Generator, sharing technical insights and practical lessons for building scalable, AI-powered applications.<!--excerpt_end-->

# The Data Behind Pantone's Agentic AI: Building on Azure Cosmos DB

By Jesse Sullivan

Pantone, the global authority on color, faced the challenge of delivering its deep color expertise through a modern AI-powered experience. In their recent webinar, Pantone leaders shared the journey of building the Palette Generator—a minimum viable product (MVP) that rapidly gathered user feedback while leveraging Microsoft Azure's AI and data services.

## Challenge: Scaling Color Expertise with AI

Pantone supports designers and brands worldwide in defining and communicating color. Translating this expertise into a dynamic, chat-based AI tool required rethinking traditional workflows and making AI accessible, creative, and context-aware for designers.

## Palette Generator: An Agentic AI Experience

Pantone's Palette Generator uses a multiagent architecture. Specialized agents—the 'chief color scientist', a palette generator, and others—work together within a chat interface to provide curated palettes and interactive design support. The system emphasizes context, reasoning, and dynamic response, moving far beyond static recommendations.

## Azure Cosmos DB as the Foundation

At the core of the Palette Generator is Azure Cosmos DB, which stores chat history, prompts, user interactions, and message collections. The choice of Cosmos DB was key due to its scalability, real-time performance, and ease of integration—developers could build proofs-of-concept with minimal code and serve a global user base efficiently. Pantone found Cosmos DB crucial for supporting the conversational memory and analytics needed for agentic AI experiences.

> "We were able to make our initial proof of concept with a few lines of code and retrieve all the data very, very fast, like in a few milliseconds."  
> — Kristijan Risteski, Pantone

## Evolving Architecture: From Text to Vectors

Pantone is advancing from traditional text storage to vector-based workflows, embedding prompts and contextual data, and leveraging vector search for improved semantic understanding. Azure Cosmos DB supports these modern requirements, integrating with agent orchestration and embedding models deployed through Microsoft Foundry, allowing quick iterations without major rearchitecture.

## Real-World Results

Within the first month, the Palette Generator was used in over 140 countries, supporting thousands of conversations and multiple languages. Designers actively refined prompts and interacted with the agentic system, and Pantone quickly gleaned insights about behavior, product performance, and architectural trade-offs thanks to Cosmos DB's flexible data model.

## Lessons Learned for Building AI-Driven Applications

Pantone's story offers several takeaways:

- **Data-Driven AI:** Real-time, scalable database layers like Cosmos DB are foundational for AI that adapts to user context.
- **Iterative Feedback:** Capturing and analyzing interactions enables constant improvement.
- **Architectural Flexibility:** AI infrastructure and databases must evolve to support embedding strategies, orchestration, and analytics.

Pantone's experience is a blueprint for organizations moving expertise into AI-powered solutions, demonstrating how Azure AI services and Cosmos DB can join creativity with cutting-edge technology.

[Explore Azure Cosmos DB](https://azure.microsoft.com/en-us/products/cosmos-db/)

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/the-data-behind-the-design-how-pantone-built-agentic-ai-with-an-ai-ready-database/)
