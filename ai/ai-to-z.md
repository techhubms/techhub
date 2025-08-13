---
layout: "page"
title: "A(i) to Z"
description: "A starting point for learning about AI."
image: "/assets/ai-to-z/logo.png"
category: "AI"
page-name: "ai-to-z"
---

The goal of this page is not to dive into every detail of AI, but to provide a solid foundation and a starting point for anyone interested in learning about AI, whether you are a beginner or have some experience.

**What you'll learn in this guide:**

This comprehensive guide takes you on a journey through the AI landscape, from historical context to cutting-edge applications. We'll explore the fundamental concepts that power modern AI systems, examine practical tools and platforms you can use today, and look ahead to emerging trends that will shape the future.

**Who this guide is for:**

- **Developers** wanting to integrate AI into their applications
- **Business professionals** exploring AI opportunities for their organizations  
- **Students and researchers** seeking a practical understanding of AI technologies
- **Anyone curious** about how AI works and how to get started with AI tools

**How to use this guide:**

This guide is designed to be read sequentially, as each section builds on concepts from previous sections. However, experienced readers can jump to specific topics using the table of contents. Look for FAQ sections throughout that address common questions, and follow the "More information" links for deeper dives into specific topics.

## Table of Contents

- [History](#history)
- [ML vs AI vs GenAI](#ml-vs-ai-vs-genai)
- [GenAI part 1](#genai-part-1)
  - [Introduction](#introduction)
  - [Vendors](#vendors)
  - [Models](#models)
  - [Providers](#providers)
  - [Prompts & messages](#prompts--messages)
  - [Tokens & Tokenization](#tokens--tokenization)
  - [Costs](#costs)
  - [Problems with models](#problems-with-models)
  - [Fine-tuning a model](#fine-tuning-a-model)
  - [Advanced concepts](#advanced-concepts)
- [GenAI part 2](#genai-part-2)
  - [Function calling](#function-calling)
  - [Model Context Protocol (MCP)](#model-context-protocol-mcp)
  - [Retrieval Augmented Generation (RAG)](#retrieval-augmented-generation-rag)
  - [Agents & Agentic AI](#agents--agentic-ai)
- [Inspiration](#inspiration)
  - [Chat interfaces](#chat-interfaces)
  - [Office 365](#office-365)
  - [Windows](#windows)
  - [GitHub](#github)
  - [Other real-world examples](#other-real-world-examples)
- [Integrating AI into your applications](#integrating-ai-into-your-applications)
  - [Tools and IDEs](#tools-and-ides)
  - [Copilot](#copilot)
  - [Azure AI services](#azure-ai-services)
  - [Languages & SDKs](#languages--sdks)
  - [Semantic Kernel](#semantic-kernel)
- [(Near) future](#near-future)
- [Want to know more?](#want-to-know-more)

## History

Some fun history facts about AI, showing how long AI has been around and how it has evolved over time:

- 1956: The term "artificial intelligence" was coined at the Dartmouth Conference.
- 1966: ELIZA, an early natural language processing program, was created by Joseph Weizenbaum.
- 1997: IBM's Deep Blue defeated world chess champion Garry Kasparov.
- 2011: IBM's Watson won the quiz show Jeopardy! against human champions.
- 2012: The breakthrough in deep learning with AlexNet winning the ImageNet competition.
- 2014: The first Generative Adversarial Network (GAN) was introduced by Ian Goodfellow.
- 2018: OpenAI released the first version of GPT (Generative Pre-trained Transformer).
- 2020: OpenAI released GPT-3, a significant advancement in natural language processing.
- 2023: OpenAI released GPT-4, further enhancing capabilities in language understanding and generation.

## ML vs AI vs GenAI

Understanding the relationship between these three concepts is essential for anyone starting their AI journey.

**Machine Learning (ML)** is a method of data analysis where computers learn to identify patterns in data without being explicitly programmed for each task. Think of it as teaching a computer to recognize cats in photos by showing it thousands of cat pictures, rather than writing specific code that says "a cat has pointed ears, whiskers, and four legs."

**Artificial Intelligence (AI)** is a broader field that includes machine learning but also encompasses other approaches to creating intelligent systems. AI aims to create machines that can perform tasks that typically require human intelligence, such as reasoning, learning, planning, and understanding language.

Machine learning is one approach within the AI toolkit, but AI also includes:

- **Rule-based systems**: Follow pre-defined logical rules and conditions (like "if temperature > 80°F, then turn on air conditioning")
- **Expert systems**: Capture human expertise in specific domains through knowledge bases and inference engines (like medical diagnosis systems that apply doctor's knowledge)
- **Symbolic AI**: Uses symbols and logic to represent knowledge and reasoning (like early chess programs that evaluated board positions)
- **Evolutionary algorithms**: Solve problems by mimicking natural selection and evolution
- **Fuzzy logic**: Handles uncertainty and partial truths rather than strict true/false logic

These different approaches can be combined or used independently depending on the problem being solved.

**Generative AI (GenAI)** is a specific type of AI that can create new content - text, images, code, music, or other types of data. When you ask ChatGPT to write a story or use DALL-E to create an image, you're using generative AI. GenAI models learn patterns from existing content and use that knowledge to generate new, original content that follows similar patterns.

## GenAI part 1

### Introduction

Understanding Generative AI begins with **Natural Language Processing (NLP)**, the foundational branch of AI that enables computers to understand, interpret, and generate human language. NLP represents a major breakthrough in human-computer interaction - instead of requiring users to learn programming languages or use specific commands, it allows people to interact with computers using natural human language, speaking or typing as they would to another person.

Within the broad field of NLP, modern AI systems primarily rely on language models - sophisticated AI systems that learn from vast amounts of text to understand and generate human-like responses. These language models come in different sizes and capabilities, each designed for specific use cases and computational requirements.

**Large Language Models (LLMs)** represent the current pinnacle of NLP technology. These AI systems are trained on enormous datasets containing billions of text examples from books, articles, websites, and other written sources. Through this training, they learn intricate patterns in how humans use language and develop the ability to generate remarkably human-like text responses. Popular examples include GPT-4, Claude, and Gemini. LLMs typically contain billions of parameters (the internal "settings" the AI adjusts during learning) and require significant computational resources to operate effectively.

**Small Language Models (SLMs)** serve the same fundamental purpose as their larger counterparts but prioritize efficiency and accessibility. These models are trained on more focused datasets and contain fewer parameters, making them suitable for running on local devices like smartphones, laptops, or edge computing environments. Examples include Microsoft's Phi models and various specialized versions optimized for specific tasks or industries.

Beyond text-focused language models, the NLP field has expanded to encompass various specialized model types that work with different forms of content:

- **Diffusion models** (such as DALL-E, Stable Diffusion, and Midjourney) generate images from text descriptions
- **Speech recognition models** (like Whisper) convert spoken words into written text
- **Music generation models** (such as AIVA) create original musical compositions
- **Multimodal models** (like GPT-4o) can work seamlessly with multiple types of input and output - text, images, audio, and video

This diversity of model types reflects the broader evolution of AI from simple text processing to comprehensive understanding and generation across multiple forms of human communication and creative expression.

### Vendors

AI vendors are companies that develop, train, and provide access to AI models. Each vendor brings different strengths, approaches, and business models to the market.

**Major AI vendors include:**

- **OpenAI**: Creator of the GPT series and DALL-E, known for ChatGPT and pioneering conversational AI
- **Google**: Develops Gemini models and provides AI services through Google Cloud
- **Anthropic**: Focuses on AI safety and creates the Claude family of models
- **Microsoft**: Partners with OpenAI and develops its own models while integrating AI across its product suite
- **Hugging Face**: Acts as a platform for sharing and hosting AI models, both open-source and commercial

**What vendors do:**
Vendors serve as the bridge between the complex world of AI model development and practical applications. They handle the enormous costs and technical challenges of training models, then make these models available through user-friendly interfaces and APIs. This allows developers and businesses to use sophisticated AI without needing to understand the underlying mathematics or infrastructure.

**Key differences between vendors:**

- **Research focus**: Some prioritize cutting-edge capabilities, others emphasize safety and reliability
- **Business models**: Some offer free tiers with paid upgrades, others are enterprise-focused
- **Specializations**: Different vendors excel in text, images, code, or multimodal applications
- **Accessibility**: Some focus on easy-to-use consumer interfaces, others provide developer tools and APIs
- **Data policies**: Vendors differ in how they handle user data and model training

### Models

A **model** is the actual AI system that has been trained to perform specific tasks. Think of it as a specialized brain that has learned patterns from data and can apply that knowledge to new situations.

**Key characteristics that differentiate models:**

- **Training date**: When the model was trained affects what information it knows
- **Cut-off date**: The latest date of information the model was trained on
- **Size**: Larger models generally have more capabilities but require more computational resources
- **Vendor**: Different companies create models with different strengths and focuses
- **Specialization**: Some models excel at specific tasks like coding, creative writing, or analysis

**Current popular models include:**

**Large Language Models (LLMs):**

- **GPT series** (3.5, 4, 4o): OpenAI's flagship models for text generation and reasoning
- **Claude** (Sonnet, Opus): Anthropic's models focused on helpful, harmless, and honest interactions
- **Gemini** (Flash, Pro): Google's multimodal models that can process text, images, and other data
- **Grok**: X (formerly Twitter)'s conversational AI model

**Small Language Models (SLMs):**

- **Phi**: Microsoft's efficient models designed to run on smaller devices
- **Smol**: Compact models optimized for specific tasks

**Specialized Models:**

- **DALL-E, Stable Diffusion, Midjourney**: Image generation models (diffusion models)
- **Whisper**: Speech-to-text conversion model
- **AIVA**: Music composition model
- **GPT-4o/4o mini**: Multimodal models that handle text, images, audio, and video

#### Models FAQ

**What is a GPT and why are not all models GPT?**
GPT stands for "Generative Pre-trained Transformer." It's a specific architecture developed by OpenAI. Not all models use this architecture - companies develop different approaches to achieve similar goals. Think of it like cars: not all cars are Toyotas, even though they all serve the same basic purpose of transportation.

**What does multimodal mean and how can I use it?**
Multimodal models can understand and generate multiple types of content - not just text, but also images, audio, and video. For example, you can upload an image to GPT-4o and ask it to describe what it sees, or ask it to create an image based on your description. This makes interactions more natural and expands what you can accomplish with AI.

**Why not train a model every month or week to keep it up-to-date?**
Training large AI models requires enormous computational resources, costs millions of dollars, and takes weeks or months to complete. The process involves analyzing billions of text examples and adjusting trillions of parameters. Additionally, frequent retraining could make models less stable and reliable. Instead, vendors typically release new versions periodically with updated knowledge and improved capabilities.

### Providers

**Providers** are services that host AI models and make them accessible to users and developers. While vendors create the models, providers handle the infrastructure needed to run them at scale and make them available through APIs, web interfaces, or applications.

**Technical components of hosted AI services include:**

- **Proxy**: Routes requests to available model instances and manages traffic
- **Load balancer**: Distributes requests across multiple servers to ensure reliable performance
- **Content filter**: Screens inputs and outputs to prevent harmful or inappropriate content
- **Rate limiting**: Controls how many requests users can make to prevent abuse
- **Authentication**: Manages user access and API keys
- **Monitoring**: Tracks usage, performance, and costs

**Major hosted providers:**

- **OpenAI**: Direct access to GPT models through their API and ChatGPT interface
- **Google Cloud**: Hosts Gemini and other Google AI models
- **Microsoft Azure**:
  - **Azure OpenAI**: Enterprise-grade access to OpenAI models with enhanced security and compliance
  - **GitHub Models**: Developer-focused platform with model catalog and development tools
- **Hugging Face**: Platform for both open-source and commercial models
- **Anthropic**: Direct access to Claude models

**Self-hosting options:**

For organizations that need complete control over their AI infrastructure, self-hosting is possible:

- **Docker containers**: Run models in containerized environments
- **Ollama**: User-friendly tool for running models locally
- **Hugging Face Transformers**: Library for deploying models on your own hardware

#### Providers FAQ

**Do all hosted solutions use my data?**
Data usage policies vary significantly between providers. Some use conversation data to improve their models (with user consent), while others, particularly enterprise-focused services, commit to not using customer data for training. Always check the specific terms of service and privacy policies.

**Where is my data stored?**
Data storage locations depend on the provider and service tier. Consumer services might store data globally, while enterprise services often allow you to specify geographic regions for compliance with local regulations like GDPR.

**Can I opt-out of data collection?**
Most providers offer ways to limit data collection, though the options vary. Some allow you to delete conversation history, others provide settings to prevent data from being used for model training.

**How does Azure OpenAI differ from using OpenAI directly?**
Azure OpenAI provides the same models as OpenAI but with enterprise-grade security, compliance certifications, integration with other Azure services, and guarantees that your data won't be used to train models. It's designed for organizations with strict data governance requirements.

**How does GitHub Models relate to GitHub Copilot?**
GitHub Models is a development platform that gives developers access to various AI models for building applications, while GitHub Copilot is a specific AI coding assistant. Think of GitHub Models as a toolbox for AI development, and Copilot as one specific tool that helps with coding. [GitHub Models](https://docs.github.com/en/github-models/about-github-models) provides model catalogs, prompt management, and evaluation tools for developers.

### Prompts & messages

**Prompts** are the instructions or questions you give to an AI model, while **messages** are the individual communications in a conversation between you and the AI. Understanding how to structure these effectively is key to getting good results from AI systems.

**Types of prompts and messages:**

- **User prompt**: Your question, instruction, or request to the AI
- **System prompt**: Background instructions that set the AI's behavior, role, or constraints (often hidden from users)
- **Assistant message**: The AI's response to your prompt
  - **Suggestions**: When the AI offers multiple options or approaches
  - **Completions**: When the AI finishes or continues text you've started

**Prompt engineering** is the practice of crafting effective prompts to get better results from AI models. Different techniques work better for different types of tasks:

**Prompt techniques:**

- **Zero-shot prompts**: Ask the AI to perform a task without providing examples
  - Example: "Summarize this article in three bullet points."
  
- **Few-shot prompts**: Provide a few examples of the desired input-output pattern
  - Example: "Translate these phrases. English: Hello → Spanish: Hola. English: Thank you → Spanish: Gracias. English: Good morning → Spanish: ?"
  
- **Chain of thought prompts**: Ask the AI to show its reasoning process
  - Example: "Solve this math problem step by step, showing your work."

**Reusable prompts** are templates or standardized instructions that you can use repeatedly for similar tasks. These save time and ensure consistency. For example, you might create a standard prompt for code reviews or document summaries that includes specific criteria and formatting instructions.

### Tokens & Tokenization

**Tokens** are the basic units that AI models use to process text. Think of them as the "words" that the AI actually understands, though they don't always match human words exactly.

**How tokenization works:**
When you send text to an AI model, it first breaks your message into tokens. This process, called tokenization, splits text into manageable pieces. A token might be:

- A whole word (like "hello")
- Part of a word (like "un" and "believable" for "unbelievable")
- A punctuation mark
- A space or special character

**How many tokens are in a message?**
As a rough guide, 1 token equals about 0.75 English words. So 100 words would be approximately 133 tokens. However, this varies based on:

- Language (non-English text often uses more tokens)
- Technical terms and proper nouns
- Punctuation and formatting

**Why is this important?**
Tokens directly affect:

- **Cost**: Most AI services charge per token processed
- **Speed**: More tokens mean longer processing time
- **Limits**: Models have maximum token limits for conversations

**Token limits and what happens when you exceed them:**
Every model has a maximum context window (total tokens it can process at once). When you exceed this limit:

- The model might truncate older parts of the conversation
- You might get an error message
- The quality of responses may decrease

**What to do when hitting token limits:**

- Summarize earlier parts of long conversations
- Break complex tasks into smaller pieces
- Use more concise language in your prompts
- Start a new conversation if context becomes too long

**Switching between tokenizers:**
Different models use different tokenization methods, so you can't directly transfer token counts between models. What takes 100 tokens in one model might take 90 or 110 in another.

**Tokenization differences between content types:**

- **Text**: Broken into word parts and punctuation
- **Images**: Converted into fixed-size "image tokens" representing visual information
- **Audio**: Processed into time-based segments representing sound patterns
- **Code**: Often tokenized similar to text but may handle syntax differently

### Costs

Understanding AI costs helps you make informed decisions about which models and approaches to use for different tasks.

**Cost factors:**

**Context**: The amount of information the model needs to consider affects cost. This includes your current message plus any conversation history or background information (system prompts).

**Chat history**: Longer conversations cost more because the model processes the entire conversation context with each new message. If you have a 50-message conversation, the model reviews all previous messages to understand context when responding to message 51.

**Prompts**: More detailed and longer prompts cost more to process, but they often produce better results. Finding the right balance between prompt detail and cost is important for regular use.

**Cost optimization strategies:**

- Keep conversations focused and avoid unnecessary context
- Use smaller, more efficient models for simple tasks
- Reserve powerful (expensive) models for complex problems
- Consider using summarization to reduce context length in long conversations
- Clear conversation history when starting new topics that don't require previous context

### Problems with models

Understanding the limitations of AI models helps you use them more effectively and avoid common pitfalls.

**Hallucinations**
AI models sometimes generate information that sounds confident and plausible but is factually incorrect. This happens because models predict what text should come next based on patterns they learned, rather than accessing a database of facts. Always verify important information, especially dates, statistics, and specific claims.

**Input-poisoning**
Malicious users might try to manipulate AI responses by including hidden instructions or misleading information in their prompts. Well-designed systems include protections against this, but it's important to be aware that AI responses can be influenced by how questions are framed.

**Jailbreaking**
This refers to attempts to bypass an AI's safety guidelines or restrictions through clever prompting techniques. While providers work to prevent this, it highlights the importance of not relying solely on AI systems for content moderation or safety-critical decisions.

**Why AI struggles with calculations and counting**
Language models are designed to predict text patterns, not perform precise mathematical operations. They might correctly handle simple arithmetic they've seen many times in training data, but they're not calculators. For reliable mathematical results:

- Use dedicated calculation tools
- Ask the AI to write code that performs the calculation
- Verify mathematical results independently

When you need precise calculations or counting, consider using AI to generate code for a calculator or spreadsheet rather than asking for direct numerical results.

**Human-in-the-loop (HITL) approaches**
Given these limitations, many organizations implement Human-in-the-loop (HITL) systems where humans remain involved in AI decision-making processes. Instead of fully automated AI systems, HITL approaches include human oversight, validation, or intervention at key points. For example, an AI might flag potentially problematic content, but a human reviews and makes the final decision about whether to remove it. This approach helps mitigate risks while still benefiting from AI's efficiency and capabilities.

### Fine-tuning a model

Fine-tuning involves adjusting model behavior and output to better match your specific needs. While full model retraining requires significant resources, you can influence model behavior through several techniques:

**Grounding**
Grounding provides the AI with specific, factual information to base its responses on. Instead of relying on the model's training data, you supply current, accurate information within your prompt. For example, when asking about company policies, include the actual policy text in your prompt rather than assuming the model knows current details.

**Temperature**
Temperature controls how creative or predictable the AI's responses are:

- **Low temperature (0.0-0.3)**: More focused and consistent responses, good for factual tasks
- **Medium temperature (0.4-0.7)**: Balanced creativity and consistency, suitable for most general tasks
- **High temperature (0.8-1.0)**: More creative and varied responses, useful for brainstorming or creative writing

**Top P (nucleus sampling)**
Top P determines how many alternative words the model considers when generating each token:

- **Low Top P (0.1-0.5)**: More focused responses using only the most likely word choices
- **High Top P (0.8-1.0)**: More diverse responses considering a wider range of possible words

These settings work together - you might use low temperature and low Top P for consistent, factual responses, or high temperature and high Top P for creative brainstorming sessions.

### Advanced concepts

**What is a neural network?**
A neural network is a computing system inspired by biological brains. It consists of interconnected nodes (artificial neurons) that process information. Each connection has a weight that determines how much influence one node has on another. Neural networks learn by adjusting these weights based on training data.

**Training a model involves several key concepts:**

**Transformers**
The architecture that powers modern language models. Transformers excel at understanding context and relationships between words, even when they're far apart in a sentence. They use an "attention mechanism" to focus on relevant parts of the input when generating each part of the output.

**Training data**
The text, images, or other information used to teach the model. Quality and diversity of training data significantly impact model performance. Modern LLMs are trained on billions of web pages, books, and articles.

**Cut-off date**
The latest date of information included in training data. Models don't automatically know about events after their cut-off date, which is why they can't provide information about very recent developments.

**Weights and Parameters**
The numerical values that determine how the model processes information. Large language models have billions or trillions of parameters. During training, these values are adjusted to improve the model's predictions.

**Context**
The amount of previous information the model can consider when generating a response. Larger context windows allow models to maintain coherence over longer conversations or documents.

**Input and Output size**
Limits on how much information can be processed at once. Input size affects how much text you can include in a prompt, while output size determines the maximum length of generated responses.

**Seed**
A number used to make AI outputs reproducible. Using the same seed with identical inputs should produce the same output, which is useful for testing and debugging.

**Vocabulary**
The set of tokens the model understands. Larger vocabularies allow models to handle more languages and specialized terms but require more computational resources.

**Attention**
The mechanism that allows models to focus on relevant parts of the input when generating each part of the output. This enables understanding of long-range dependencies and context.

**Foundational concepts:**

**Vectors**
Mathematical representations of concepts as lists of numbers. Words, images, and other data are converted into vectors that capture their meaning and relationships.

**Embeddings**
High-dimensional vector representations that capture semantic meaning. Similar concepts have similar embeddings, allowing models to understand relationships between different ideas.

**Inference**
The process of using a trained model to generate predictions or responses. This is what happens when you send a prompt to an AI model and receive a response.

For a comprehensive deep dive into how these concepts work together, [Andrej Karpathy's tutorial on building ChatGPT from scratch](https://www.youtube.com/watch?v=kCc8FmEb1nY) provides an excellent technical foundation.

## GenAI part 2

### Function calling

**Function calling** allows AI models to use external tools and services during their responses. Instead of only generating text, the model can call predefined functions to perform specific actions like checking the weather, calculating mathematical expressions, or retrieving current information from databases.

**How it works:**

1. You define functions with clear descriptions of what they do and what parameters they need
2. The AI model analyzes your prompt and determines if any functions would help answer your question
3. The model calls the appropriate function with the right parameters
4. The function returns results, which the model incorporates into its response

**Example function definition:**

```text
Function: get_flight_duration
Description: Calculate flight duration between two airports
Parameters:
  - departure_airport: IATA airport code (e.g., "JFK", "LAX")
  - arrival_airport: IATA airport code (e.g., "JFK", "LAX")
  - include_layovers: Boolean, whether to include connection time

Example usage:
User: "How long does it take to fly from New York to Los Angeles?"
Model: Calls get_flight_duration("JFK", "LAX", true)
Function returns: "6 hours 30 minutes including one layover"
```

**How the model matches functions to prompts:**
Models use the function descriptions and parameter details to understand when a function is relevant. They look for keywords, context clues, and the type of information being requested. The better your function descriptions, the more accurately the model will know when and how to use them.

**Benefits:**

- Access to real-time information
- Ability to perform precise calculations
- Integration with external systems and databases
- More accurate and up-to-date responses

**More information:**

- [Connecting to a Local MCP Server Using Microsoft.Extensions.AI]({{ "/2025-04-30-Connecting-to-a-Local-MCP-Server-Using-MicrosoftExtensionsAI.html" | relative_url }})
- [Model Context Protocol Development Best Practices]({{ "/2025-07-28-MCP-Development-Best-Practices.html" | relative_url }})
- [Let's Learn Model Context Protocol with JavaScript and TypeScript]({{ "/2025-07-21-Lets-Learn-Model-Context-Protocol-with-JavaScript-and-TypeScript.html" | relative_url }})

### Model Context Protocol (MCP)

**What is MCP and what problem does it solve?**
Model Context Protocol is an open standard that enables AI models to securely connect to external data sources and tools. Before MCP, each AI application had to build custom integrations for every service they wanted to connect to. MCP creates a standardized way for AI models to access external resources, making it easier to build AI applications that can interact with real-world systems.

**Key components:**

- **Host**: The application that contains the AI model (like your IDE, chat application, or development environment)
- **Client**: The component that communicates with MCP servers on behalf of the AI model
- **Server**: The service that provides access to external resources like databases, APIs, or file systems

**How does it relate to OpenAI function calling?**
MCP and OpenAI function calling serve similar purposes but work at different levels:

- **Function calling** is a feature within specific AI models that allows them to call predefined functions
- **MCP** is a protocol that standardizes how AI applications connect to external services, which can then expose functions to the AI

Think of function calling as the language AI models use to request external actions, while MCP is the standardized postal service that delivers those requests to the right destinations.

**Security considerations:**
Current MCP implementations have limited security features, but improvements are coming:

- **OAuth support** is in development ([draft specification](https://modelcontextprotocol.io/specification/draft/basic/authorization))
- **Temporary solutions** like [Auth0 & Cloudflare](https://auth0.com/blog/secure-and-deploy-remote-mcp-servers-with-auth0-and-cloudflare/) provide security for remote MCP servers
- **Server isolation** and central registries are being developed to improve security

**Risks to consider:**

- MCP servers can access external systems, so proper security and access controls are essential
- Always validate and sanitize data from external sources
- Consider the privacy implications of connecting AI models to sensitive data sources

**Learning resources:**

- [MCP course on Hugging Face](https://huggingface.co/learn/mcp-course/unit0/introduction) provides comprehensive training
- Microsoft is working on enhanced MCP support with better security features

**More information:**

- [Connecting to a Local MCP Server Using Microsoft.Extensions.AI]({{ "/2025-04-30-Connecting-to-a-Local-MCP-Server-Using-MicrosoftExtensionsAI.html" | relative_url }})
- [Model Context Protocol Development Best Practices]({{ "/2025-07-28-MCP-Development-Best-Practices.html" | relative_url }})
- [Let's Learn Model Context Protocol with JavaScript and TypeScript]({{ "/2025-07-21-Lets-Learn-Model-Context-Protocol-with-JavaScript-and-TypeScript.html" | relative_url }})

### Retrieval Augmented Generation (RAG)

**What is RAG and why is it important?**
Retrieval Augmented Generation combines the power of AI language models with access to specific, up-to-date information from external sources. Instead of relying solely on the AI's training data (which has a cut-off date), RAG allows the model to retrieve relevant information from documents, databases, or knowledge bases in real-time and use that information to generate more accurate responses.

**How RAG works:**

1. Your question is processed to understand what information is needed
2. A search system finds relevant documents or data from your knowledge base
3. The retrieved information is combined with your original question
4. The AI model generates a response based on both your question and the retrieved information

**Why RAG is valuable:**

- Provides access to current information beyond the model's training cut-off
- Allows AI to work with your specific company data and documents
- Reduces hallucinations by grounding responses in factual sources
- Enables AI to cite sources and provide verifiable information

**How does it differ from MCP and function calling?**

**RAG** is primarily about retrieving and using information from documents and knowledge bases. It's focused on finding relevant text or data to inform the AI's response.

**MCP** provides a standardized protocol for AI models to connect to various external services and tools, which could include RAG systems but also databases, APIs, and other services.

**Function calling** is the mechanism AI models use to invoke specific operations, which could include RAG searches, MCP server interactions, or direct API calls.

**When to use each approach:**

**Use RAG when:**

- You need AI to answer questions about specific documents or knowledge bases
- You want responses grounded in verifiable sources
- You're dealing with information that changes frequently
- You need to work with proprietary or domain-specific content

**Use MCP when:**

- You need standardized connections to multiple external services
- You want to build reusable integrations across different AI applications
- You need secure, protocol-based access to external resources

**Use function calling when:**

- You need the AI to perform specific actions (calculations, API calls, data operations)
- You want direct control over what external services the AI can access
- You're building custom integrations for specific use cases

### Agents & Agentic AI

**What makes something an agent?**
An AI agent is a system that can autonomously perform tasks, make decisions, and interact with external environments to achieve specific goals. Unlike simple AI models that respond to individual prompts, agents can:

- Plan multi-step tasks
- Use tools and external services
- Learn from feedback and adapt their approach
- Operate with some degree of independence
- Maintain context across multiple interactions

**Is there a formal definition or interface?**
While there's no single universal definition, most AI agents share common characteristics:

- **Autonomy**: Can operate without constant human intervention
- **Goal-oriented**: Work toward specific objectives
- **Environment interaction**: Can perceive and act upon their environment
- **Tool use**: Can access and utilize external resources
- **Planning**: Can break down complex tasks into manageable steps

**What's the difference compared to MCP servers?**
**MCP servers** provide specific services and tools that AI models can access through a standardized protocol. They're typically focused on particular functions (like database access or file management).

**AI agents** use tools and services (potentially including MCP servers) to accomplish broader goals. An agent might use multiple MCP servers, APIs, and other resources to complete complex, multi-step tasks.

Think of MCP servers as specialized tools in a workshop, while AI agents are the skilled craftspeople who use those tools to complete projects.

**What does "agentic" mean?**
"Agentic" describes AI systems that exhibit agent-like behaviors - the ability to act independently, make decisions, and pursue goals with minimal human oversight. Agentic AI can:

- Take initiative to solve problems
- Adapt strategies based on results
- Handle unexpected situations
- Work toward long-term objectives
- Coordinate with other systems or agents

**Examples of agentic AI:**

- **Personal assistants** that can book appointments, send emails, and manage schedules
- **Code assistants** that can analyze codebases, identify issues, and implement fixes
- **Research agents** that can gather information from multiple sources and synthesize findings
- **Customer service agents** that can resolve issues across multiple systems and departments

**More information:**

- [Introducing Microsoft Discovery: An Agentic AI Platform for Scientific Research]({{ "/2025-05-19-Introducing-Microsoft-Discovery-An-Agentic-AI-Platform-for-Scientific-Research.html" | relative_url }})

### Scaling AI implementations

**Scaled GenAI** involves deploying generative AI solutions across entire organizations or large user bases. This requires considerations around infrastructure, cost management, quality control, security, and governance. Companies implementing scaled GenAI need to think about how to maintain consistency, manage costs, and ensure responsible use across thousands of users and use cases.

**Key considerations for scaling AI:**

- **Infrastructure planning**: Ensuring adequate computational resources and network capacity
- **Cost management**: Monitoring and optimizing AI usage costs across the organization  
- **Quality control**: Maintaining consistent AI outputs and performance standards
- **Security and compliance**: Protecting sensitive data and meeting regulatory requirements
- **Governance frameworks**: Establishing policies for appropriate AI use and oversight
- **Change management**: Training users and managing the transition to AI-enhanced workflows

## Inspiration

Cool, but don't know what to start with? Here are some examples to get you inspired.

### Chat interfaces

Chat interfaces make AI accessible to everyone by providing familiar, conversation-like interactions. These interfaces have become the primary way people interact with AI models.

**Popular chat interfaces and their strengths:**

**ChatGPT**: OpenAI's web interface for their GPT models

- Conversational AI for general questions, writing, and problem-solving
- Supports file uploads for document analysis
- Custom GPTs for specialized tasks
- Voice interaction capabilities

**Claude**: Anthropic's chat interface

- Focus on helpful, harmless, and honest responses
- Strong performance on complex reasoning tasks
- Good at maintaining context in long conversations

**Gemini**: Google's AI chat interface

- Integration with Google services and search
- Multimodal capabilities (text, images, documents)
- Real-time information access

**Microsoft Copilot**: Integrated across Microsoft products

- Available in Windows, Edge, Office applications
- Context-aware based on what you're working on
- Integration with Microsoft 365 data and services

**GitHub Copilot Chat**: AI assistance within development environments

- Code-focused conversations
- Understanding of your codebase and development context
- Integration with development workflows

**What you can accomplish with chat interfaces:**

- Get explanations of complex topics
- Generate and edit content (writing, code, creative work)
- Analyze documents and data
- Brainstorm ideas and solutions
- Learn new skills through interactive tutoring
- Plan projects and break down complex tasks

**More information:**

- [Chat in IDE]({{ "/2025-01-02-Chat-in-IDE.html" | relative_url }})
- [Copilot vs Chat: Sidekick Showdown - When to Use Each Coding Sidekick]({{ "/2025-07-11-Copilot-vs-Chat-Sidekick-Showdown-When-to-Use-Each-Coding-Sidekick.html" | relative_url }})

### Office 365

AI has been integrated throughout Microsoft 365 (formerly Office 365) to enhance productivity and streamline common tasks across familiar applications.

**Microsoft Copilot in Office applications:**

**Word**: AI writing assistant that helps with:

- Drafting documents from outlines or prompts
- Rewriting content for different audiences or tones
- Summarizing long documents
- Suggesting improvements to writing style and clarity

**Excel**: Data analysis and automation support:

- Creating formulas from natural language descriptions
- Generating charts and visualizations
- Analyzing data patterns and trends
- Creating pivot tables and data summaries

**PowerPoint**: Presentation creation and design:

- Generating slide content from topics or outlines
- Suggesting design improvements and layouts
- Creating speaker notes and talking points
- Converting documents into presentation format

**Outlook**: Email and calendar management:

- Drafting email responses based on context
- Summarizing long email threads
- Scheduling meetings based on natural language requests
- Creating meeting agendas and follow-up tasks

**Teams**: Meeting and collaboration enhancement:

- Real-time meeting transcription and summary
- Action item extraction from discussions
- Meeting preparation and follow-up assistance
- Chat summarization for missed conversations

**Benefits of AI in Office 365:**

- Reduces time spent on routine tasks
- Maintains consistency with existing workflows
- Leverages your organization's data and context
- Works within familiar interfaces you already know
- Provides enterprise-grade security and compliance

**More information:**

- [Enhance IT Expertise and Efficiency with Copilot in Microsoft Intune]({{ "/2024-12-09-Enhance-IT-Expertise-and-Efficiency-with-Copilot-in-Microsoft-Intune.html" | relative_url }})

### Windows

Microsoft has integrated AI capabilities directly into Windows, making intelligent features available across the entire operating system.

**Windows Copilot**: Integrated AI assistant for Windows 11

- Available through the taskbar for quick access
- Helps with system settings and configuration
- Provides information and answers questions
- Assists with file management and organization

**AI-powered features in Windows:**

- **Smart search**: Enhanced search capabilities that understand natural language queries
- **Voice typing**: Improved speech recognition and dictation across applications
- **Windows Recall**: AI-powered search through your activity history (when enabled)
- **Enhanced clipboard**: Intelligent clipboard history with text suggestions

**Developer and IT benefits:**

- **PowerShell AI integration**: AI assistance for command-line tasks and script creation
- **System administration**: AI-powered troubleshooting and system optimization
- **Development environment**: Integration with AI coding assistants and development tools

**Security and privacy considerations:**
Microsoft has implemented security features for AI integration in Windows, including local processing for sensitive operations and user control over data sharing. The company is also working on enhanced security for AI protocols like MCP with server isolation and central registries.

### GitHub

GitHub has integrated AI throughout its platform to enhance developer productivity and streamline software development workflows.

**GitHub Copilot**: AI-powered coding assistant

- **Code completion**: Suggests code as you type based on context and comments
- **Chat functionality**: Conversational AI for asking coding questions and getting explanations
- **Code review assistance**: Helps identify issues and suggests improvements
- **Test generation**: Creates unit tests based on your code

**GitHub Actions with AI**: Automated workflows enhanced by AI

- **Workflow optimization**: AI suggestions for improving CI/CD pipelines
- **Security scanning**: AI-powered vulnerability detection and remediation
- **Code quality checks**: Automated code analysis with intelligent recommendations

**GitHub Models**: Platform for AI development

- **Model catalog**: Access to various AI models for development and testing
- **Prompt management**: Tools for creating and managing AI prompts
- **Quantitative evaluations**: Testing and comparing AI model performance

**Repository insights with AI**:

- **Code analysis**: Understanding codebase patterns and dependencies
- **Documentation generation**: AI-assisted README and documentation creation
- **Issue and PR summarization**: Automatic summaries of discussions and changes

**Benefits for development teams:**

- Faster code development and fewer syntax errors
- Improved code quality through AI-powered reviews
- Better documentation and knowledge sharing
- Reduced time on routine development tasks

**More information:**

- [What's new with the GitHub Copilot coding agent: A look at the updates]({{ "/2025-07-25-Whats-new-with-the-GitHub-Copilot-coding-agent-A-look-at-the-updates.html" | relative_url }})
- [Modernizing Legacy COBOL to Cloud with GitHub Copilot]({{ "/2025-07-22-Modernizing-Legacy-COBOL-to-Cloud-with-GitHub-Copilot.html" | relative_url }})
- [GitHub Copilot Helps One Acre Fund Scale Farming Impact]({{ "/2025-07-28-GitHub-Copilot-Helps-One-Acre-Fund-Scale-Farming-Impact.html" | relative_url }})
- [How to Use AI Models in Your GitHub Actions Workflows]({{ "/2025-08-04-How-to-Use-AI-Models-in-Your-GitHub-Actions-Workflows.html" | relative_url }})
- [Vibe Coding: PromptBoost.dev with GitHub Copilot in VS Code]({{ "/2025-07-26-Vibe-Coding-PromptBoostdev-with-GitHub-Copilot-in-VS-Code.html" | relative_url }})

### Other real-world examples

Companies across industries are finding practical ways to integrate AI into their operations, often starting with specific use cases that provide clear value.

**Customer service and support**:

- **Chatbots** that handle common questions and route complex issues to human agents
- **Email response assistance** that suggests replies based on customer history and context
- **Knowledge base enhancement** with AI-powered search and content recommendations

**Content creation and marketing**:

- **Social media management** with AI-generated post ideas and scheduling optimization
- **Product descriptions** automatically generated from specifications and features
- **A/B testing** enhanced with AI predictions about content performance

**Business operations**:

- **Document processing** that extracts information from invoices, contracts, and forms
- **Meeting summarization** that creates action items and follow-up tasks
- **Data analysis** that identifies trends and generates insights from business metrics

**Software development**:

- **Code review automation** that catches common issues and suggests improvements
- **Testing assistance** with AI-generated test cases and bug detection
- **Documentation generation** that keeps technical docs in sync with code changes

**Industry-specific applications**:

- **Healthcare**: AI assistance for medical documentation and diagnostic support
- **Finance**: Fraud detection and risk assessment automation
- **Manufacturing**: Predictive maintenance and quality control
- **Education**: Personalized learning assistance and grading support

**Key success factors**:

- Start with clearly defined problems and measurable outcomes
- Begin with low-risk applications to build confidence and expertise
- Ensure proper data quality and security measures
- Maintain human oversight for critical decisions
- Provide training and support for users adapting to AI-enhanced workflows

**More information:**

- [AI Challenger: Loft Orbital - Building Smarter Satellites with AI]({{ "/2025-06-11-AI-Challenger-Loft-Orbital-Building-Smarter-Satellites-with-AI.html" | relative_url }})
- [Is AI the Right Solution? Part 2: Applying the Framework and Navigating Ethical Risks]({{ "/2025-06-02-Is-AI-the-Right-Solution-Part-2-Applying-the-Framework-and-Navigating-Ethical-Risks.html" | relative_url }})

## Integrating AI into your applications

### Tools and IDEs

Modern development environments have integrated AI capabilities to enhance productivity and streamline development workflows.

**Visual Studio Code**: Microsoft's popular code editor

- **GitHub Copilot integration**: Code suggestions and chat functionality directly in the editor
- **AI-powered extensions**: Various AI tools for specific programming languages and frameworks
- **Intelligent code completion**: Context-aware suggestions based on your codebase
- **AI chat panels**: Dedicated spaces for AI conversations about your code

**Visual Studio**: Microsoft's full-featured IDE

- **IntelliSense enhanced with AI**: Smarter code completion and error detection
- **Code review assistance**: AI-powered suggestions for code improvements
- **Debugging help**: AI assistance in understanding and fixing bugs
- **Project template suggestions**: AI recommendations for project structure and dependencies

**JetBrains Rider**: Cross-platform .NET IDE

- **AI Assistant**: Built-in AI features for code generation and explanation
- **Refactoring suggestions**: AI-powered recommendations for code improvements
- **Test generation**: Automated unit test creation based on your code

**Other development tools**:

- **Claude Desktop**: Standalone AI assistant that can help with coding questions and code review
- **GitHub Codespaces**: Cloud development environments with integrated AI tools
- **Jupyter Notebooks**: AI assistance for data science and machine learning workflows

**Benefits of AI-integrated development tools**:

- Faster coding with intelligent suggestions and auto-completion
- Better code quality through AI-powered reviews and suggestions
- Learning opportunities through AI explanations of complex code
- Reduced context switching between development and research activities
- Automated generation of routine code like tests and documentation

**Choosing the right tool**:

- Consider your primary programming languages and frameworks
- Evaluate integration with your existing development workflow
- Look for tools that work with your team's collaboration practices
- Consider security and data privacy requirements for your organization

### Copilot

Microsoft's Copilot family represents a comprehensive approach to AI assistance across different tools and workflows.

**GitHub Copilot**: AI pair programmer for software development

- **Code completion**: Suggests entire functions and code blocks based on context
- **Chat interface**: Conversational AI for coding questions and explanations
- **Multi-language support**: Works with dozens of programming languages and frameworks

**Microsoft Copilot**: General AI assistant integrated across Microsoft products

- **Windows integration**: Available throughout the operating system
- **Office 365 integration**: AI assistance in Word, Excel, PowerPoint, and other applications
- **Web integration**: Available in Microsoft Edge and Bing

**Azure Copilot**: AI assistance for cloud infrastructure and management

- **Resource management**: Help with creating and configuring Azure services
- **Troubleshooting**: AI-powered assistance for diagnosing and fixing issues
- **Optimization suggestions**: Recommendations for cost and performance improvements

**Different interaction modes**:

**Ask**: Query-based interaction where you ask questions and receive answers

- Best for: Getting explanations, understanding concepts, quick information

**Edit**: Direct modification of content with AI assistance

- Best for: Refining existing code, documents, or configurations

**Agent**: AI takes initiative to complete multi-step tasks

- Best for: Complex workflows that require planning and execution across multiple steps

**Copilot Spaces**: Collaborative AI environments (in development)

- Shared AI workspaces for team collaboration
- Persistent context across team members and projects
- Integration with project management and development tools

**Specialized Copilot agents**:

- **Copilot coding agent**: Advanced AI assistance for complex programming tasks
- **Copilot review agent**: AI-powered code and content review capabilities

#### Copilot FAQ

**When should I refactor using my IDE versus using Copilot?**

- **Use IDE refactoring tools** for: Standard operations like renaming variables, extracting methods, or changing signatures
- **Use Copilot** for: Complex refactoring that requires understanding business logic, architectural changes, or cross-file modifications that need contextual reasoning

Lots of examples and detailed information can be found in the [GitHub Copilot Hub](/github-copilot/).

**More information:**

- [What's new with the GitHub Copilot coding agent: A look at the updates]({{ "/2025-07-25-Whats-new-with-the-GitHub-Copilot-coding-agent-A-look-at-the-updates.html" | relative_url }})
- [Copilot vs Chat: Sidekick Showdown - When to Use Each Coding Sidekick]({{ "/2025-07-11-Copilot-vs-Chat-Sidekick-Showdown-When-to-Use-Each-Coding-Sidekick.html" | relative_url }})
- [Modernizing Legacy COBOL to Cloud with GitHub Copilot]({{ "/2025-07-22-Modernizing-Legacy-COBOL-to-Cloud-with-GitHub-Copilot.html" | relative_url }})
- [GitHub Copilot Helps One Acre Fund Scale Farming Impact]({{ "/2025-07-28-GitHub-Copilot-Helps-One-Acre-Fund-Scale-Farming-Impact.html" | relative_url }})

### Azure AI services

Microsoft Azure provides a comprehensive suite of AI services, ranging from high-level platforms to specialized APIs for specific tasks.

**Azure AI Foundry**
A unified platform for building, evaluating, and deploying AI applications. AI Foundry provides:

- **Model catalog**: Access to various AI models from Microsoft and partners
- **Development tools**: Integrated environment for building AI applications
- **Evaluation capabilities**: Tools for testing and improving AI model performance
- **Deployment options**: Streamlined deployment to production environments

**Azure OpenAI**
Enterprise-grade access to OpenAI models with additional security and compliance features:

- **Same models as OpenAI**: GPT-4, GPT-3.5, DALL-E, and other OpenAI models
- **Enterprise security**: Enhanced security controls and data protection
- **Regional deployment**: Choose where your data is processed
- **Integration**: Works seamlessly with other Azure services

**Do you need to use low-level services directly?**
For most modern AI applications, you can start with high-level services like Azure OpenAI or AI Foundry. However, specialized services remain valuable for specific use cases:

**High-level services** (recommended starting point):

- **Azure AI Agent Service**: Build agents that combine AI models with tools for real-world data interaction
- **Azure AI Foundry Models**: Access flagship models through a unified interface

**Specialized services** (for specific needs):

- **Azure AI Search**: AI-powered search for applications with vector search capabilities
- **Content Safety**: Detect harmful content in text, images, and videos
- **Document Intelligence**: Extract information from documents, forms, and receipts
- **Speech Services**: Speech-to-text, text-to-speech, and translation capabilities
- **Vision Services**: Image analysis, object detection, and optical character recognition
- **Language Services**: Text analysis, sentiment detection, and key phrase extraction

**Legacy services** (consider modern alternatives):

- **Bot Service**: For traditional chatbot development (consider AI Foundry for modern conversational AI)
- **Custom Vision**: For custom image classification (consider Azure AI Vision or general models)
- **Translator**: Still relevant for dedicated translation needs
- **Video Indexer**: Extract insights from video content

**Recommendation**: Start with Azure OpenAI or AI Foundry for general AI capabilities, then add specialized services as needed for specific requirements like document processing or speech recognition.

**More information:**

- [Azure Update: 20th June 2025]({{ "/2025-06-20-Azure-Update-20th-June-2025.html" | relative_url }})
- [Azure Updates: August 2025 Highlights]({{ "/2025-08-01-Azure-Updates-August-2025-Highlights.html" | relative_url }})

### Languages & SDKs

Most programming languages have robust SDKs and libraries for AI development, making it easy to integrate AI capabilities into applications regardless of your technology stack.

**Popular programming languages for AI development:**

**Python**: Most popular language for AI/ML development

- **OpenAI SDK**: Official library for OpenAI API integration
- **LangChain**: Comprehensive framework for building AI applications
- **Hugging Face Transformers**: Access to thousands of pre-trained models
- **LlamaIndex**: Framework for building RAG applications

**JavaScript/TypeScript**: Growing ecosystem for web and Node.js development

- **OpenAI Node.js SDK**: Official JavaScript library for OpenAI APIs
- **LangChain.js**: JavaScript version of the popular framework
- **Vercel AI SDK**: Streamlined AI integration for web applications

**C#/.NET**: Strong enterprise focus with Microsoft ecosystem integration

- **Microsoft.Extensions.AI**: Unified AI framework for .NET applications
- **Semantic Kernel**: Microsoft's AI orchestration framework (detailed in next section)
- **Azure AI SDKs**: Native integration with Azure AI services

**Java**: Enterprise-focused with strong platform integration

- **OpenAI Java SDK**: Community and official libraries for OpenAI integration
- **Spring AI**: Integration with the popular Spring framework
- **Azure AI SDKs**: Java libraries for Azure AI services

**Major AI SDKs and frameworks:**

**Provider-specific SDKs:**

- **Azure AI**: Microsoft's comprehensive AI service library
- **OpenAI**: Official SDKs for multiple languages
- **Hugging Face**: Access to open-source models and datasets
- **Anthropic**: Claude API integration libraries

**Universal frameworks:**

- **LangChain/LangGraph**: Multi-language framework for AI applications with graph-based workflows
- **Smolagents**: Lightweight agent framework that runs powerful agents with minimal code
- **MCP (Model Context Protocol)**: Standard for connecting AI models to external data sources

**Microsoft-specific tools:**

- **Semantic Kernel**: AI orchestration framework (see next section)
- **Smart Components**: .NET components that add AI features to applications with minimal effort

**Language and framework compatibility matrix:**

| Language | OpenAI | Azure AI | LangChain | Semantic Kernel | MCP |
|----------|--------|----------|-----------|-----------------|-----|
| Python | ✅ | ✅ | ✅ | ✅ | ✅ |
| JavaScript/TypeScript | ✅ | ✅ | ✅ | ❌ | ✅ |
| C#/.NET | ✅ | ✅ | ❌ | ✅ | ✅ |
| Java | ✅ | ✅ | ✅ | ❌ | ❌ |

**Choosing the right tools:**

- **Start with your existing language**: Most languages have good AI SDK options
- **Consider your deployment environment**: Some frameworks work better with specific cloud platforms
- **Evaluate community support**: Popular frameworks have better documentation and community resources
- **Think about team expertise**: Choose tools that match your team's existing skills

**More information:**

- [.NET AI Community Standup: AI in .NET - What's New, What's Next]({{ "/2025-07-03-NET-AI-Community-Standup-AI-in-NET-Whats-New-Whats-Next.html" | relative_url }})
- [Connecting to a Local MCP Server Using Microsoft.Extensions.AI]({{ "/2025-04-30-Connecting-to-a-Local-MCP-Server-Using-MicrosoftExtensionsAI.html" | relative_url }})

### Semantic Kernel

**What is Semantic Kernel?**
Semantic Kernel is Microsoft's open-source AI orchestration framework designed to help developers integrate AI models with conventional programming languages like C#, Python, and Java. It acts as a middle layer that connects AI models with your application logic, external APIs, and data sources.

**Key components and AI capabilities:**

**Plugins**: Reusable components that extend AI capabilities

- **Native functions**: Regular programming functions that AI can call
- **Semantic functions**: AI-powered functions defined by natural language prompts
- **Connectors**: Integrations with external services and APIs

**Planners**: AI-powered task orchestration (note: being redesigned in newer versions)

- **Sequential planner**: Breaks down complex tasks into step-by-step plans
- **Action planner**: Determines which functions to call based on user goals
- **Stepwise planner**: Iteratively executes and adjusts plans based on results

**Memory and context management**:

- **Semantic memory**: Vector-based storage for contextual information
- **Episodic memory**: Tracks conversation history and interactions
- **Working memory**: Manages current conversation state and variables

**How does it compare to other approaches?**

**Semantic Kernel vs OpenAI Function Calling**:

- **Semantic Kernel**: Full orchestration framework with memory, planning, and plugin management
- **OpenAI Function Calling**: Direct model feature for calling specific functions

**Semantic Kernel vs MCP**:

- **Semantic Kernel**: Application framework that can use MCP servers as data sources
- **MCP**: Protocol standard for connecting AI models to external resources

Think of Semantic Kernel as a comprehensive toolkit that can incorporate both function calling and MCP servers within a larger application architecture.

#### Semantic Kernel FAQ

**Adding Azure OpenAI vs OpenAI in C# - what are the differences?**

- **Azure OpenAI**: Enterprise-grade security, data residency control, and integration with Azure services
- **Direct OpenAI**: Simpler setup but less control over data handling and security
- **Code differences**: Minimal - mainly configuration changes for endpoints and authentication
- **Performance**: Similar, but Azure OpenAI offers more predictable availability and regional deployment options

**What is still missing?**
Current limitations and areas of active development:

- **Planner redesign**: Microsoft is simplifying the planning system based on user feedback
- **Better debugging tools**: Enhanced visibility into AI decision-making processes
- **Improved memory management**: More efficient handling of large conversation contexts
- **Enhanced security**: Better integration with enterprise security and compliance requirements

**Getting started**: Semantic Kernel is particularly valuable for .NET developers building enterprise AI applications that need to integrate with existing business systems and maintain proper security controls.

**More information:**

- [.NET AI Community Standup: AI in .NET - What's New, What's Next]({{ "/2025-07-03-NET-AI-Community-Standup-AI-in-NET-Whats-New-Whats-Next.html" | relative_url }})
- [Connecting to a Local MCP Server Using Microsoft.Extensions.AI]({{ "/2025-04-30-Connecting-to-a-Local-MCP-Server-Using-MicrosoftExtensionsAI.html" | relative_url }})

## (Near) future

Lots of things are happening in the AI space, a few things to keep an eye on:

- A2A (Agent to Agent)
- ACP (Agent Communication Protocol) from [BeeAI](https://github.com/i-am-bee)

A good starting point is here: [What does MCP, A2A, and ACP mean?](https://akka.io/blog/mcp-a2a-acp-what-does-it-all-mean)

Also SLIM (Secure Low-Latency Interactive Messaging), previously called AGP (Agent Gateway Protocol) is interesting. Find more information here: [SLIM](https://github.com/agntcy/slim).

[llms.txt](https://llmstxt.org/) is another initative to keep an eye on, suggesting adding a markdown file to websites to provide LLM-friendly content. [Here is an example for GoFastMCP](https://gofastmcp.com/llms-full.txt).

Microsoft is also embracing MCP and adding support for a central server registry, server isolation and other security features. [Read the blogpost here](https://blogs.windows.com/windowsexperience/2025/05/19/securing-the-model-context-protocol-building-a-safer-agentic-future-on-windows).

## Want to know more?

### Learning resources

- **[Hugging Face](https://huggingface.co/)**: Comprehensive platform for AI models, datasets, and learning resources
- **[Rob Bos' LinkedIn Learning Course on AI development with GitHub models](https://www.linkedin.com/learning/enterprise-ai-development-with-github-models-and-azure)**: Practical course for enterprise AI development
- **[GitHub Skills](https://skills.github.com/)**: Free courses on AI and related development topics
- **[Let's build GPT: from scratch, in code, spelled out](https://www.youtube.com/watch?v=kCc8FmEb1nY)**: Deep technical dive by Andrej Karpathy

### More content from this site

- **[AI-focused videos and tutorials]({{ "/videos/" | relative_url }})**: Regular updates on AI tools and techniques
- **[Blog posts about AI implementation]({{ "/posts/" | relative_url }})**: Practical insights and real-world examples
- **[Microsoft and Azure updates]({{ "/" | relative_url }})**: Latest developments in Microsoft's AI ecosystem

### Hands-on learning

- Start with a simple chat interface to understand AI interactions
- Try GitHub Copilot or similar tools in your development environment
- Experiment with prompt engineering to improve AI responses
- Build a small application using Azure OpenAI or GitHub Models
- Join AI communities and forums to learn from others' experiences

## Conclusion

The AI landscape is rapidly evolving, but the foundational concepts covered in this guide provide a solid starting point for understanding and working with AI technologies. Whether you're a complete beginner or someone with existing technical experience, the key to success with AI is hands-on experimentation and continuous learning.

**Key takeaways from this guide:**

- **Start simple**: Begin with chat interfaces and familiar tools before moving to complex implementations
- **Understand the basics**: Knowledge of tokens, prompts, and model limitations will help you use AI more effectively
- **Choose the right tools**: Match your technical requirements with appropriate vendors, models, and frameworks
- **Think practically**: Focus on solving real problems rather than implementing AI for its own sake
- **Stay current**: The AI field moves quickly, so continuous learning is essential

**Your next steps:**

1. **Experiment with existing tools**: Try ChatGPT, GitHub Copilot, or Microsoft Copilot to understand AI capabilities
2. **Identify a real problem**: Look for specific tasks in your work that AI could help solve
3. **Start small**: Build a simple prototype before committing to large-scale implementations
4. **Learn continuously**: Follow AI developments and adapt your approach as new tools and techniques emerge

Remember that AI is a tool to enhance human capabilities, not replace human judgment. The most successful AI implementations combine the efficiency of AI with human expertise, creativity, and oversight. As you explore the possibilities, focus on how AI can help you and your organization work more effectively while maintaining the quality and ethics that matter to your users and stakeholders.
