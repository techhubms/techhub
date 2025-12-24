---
layout: "post"
title: "Local AI + .NET: Generate AltText with C# Scripts and Ollama"
description: "Bruno Capuano details how to generate accessible AltText for images using .NET 10's scriptable C# feature combined with local AI models such as those run by Ollama. The guide covers setting up the environment, using the OllamaSharp library, running the code with dotnet run, and enhancing accessibility while exploring offline AI capabilities."
author: "Bruno Capuano"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/alttext-generator-csharp-local-models/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-07-02 20:00:00 +00:00
permalink: "/news/2025-07-02-Local-AI-NET-Generate-AltText-with-C-Scripts-and-Ollama.html"
categories: ["AI", "Coding"]
tags: [".NET", ".NET 10", "Accessibility", "AI", "AI Foundry Local", "Alttext", "C#", "Coding", "Generative AI", "Image Processing", "Local AI Models", "Local Models", "News", "Ollama", "OllamaSharp", "Script Execution", "Vision Models"]
tags_normalized: ["dotnet", "dotnet 10", "accessibility", "ai", "ai foundry local", "alttext", "csharp", "coding", "generative ai", "image processing", "local ai models", "local models", "news", "ollama", "ollamasharp", "script execution", "vision models"]
---

Bruno Capuano demonstrates how to use .NET 10 and local AI models to automatically generate AltText for images using a C# script. The article provides a hands-on example and practical tips.<!--excerpt_end-->

## Summary

Bruno Capuano introduces an accessible and practical approach for generating detailed alt text for images using C# scripts, the latest features of .NET 10, and local AI models via the Ollama framework. This solution enables developers to enhance accessibility in their applications by automating the creation of image descriptions suitable for visually impaired users.

## Key Points

### AltText and Accessibility

- Adding alt text to images is crucial for accessibility, improving both user experiences and SEO.
- Manual alt text creation can be repetitive—automation through AI offers significant efficiency gains.

### Using Local AI Models with Ollama

- Local inference provides benefits such as no rate limits, reduced latency, and enhanced privacy/control.
- Ollama supports vision models like `gemma3`, `llama3.2-vision`, and `mistral-small3.2`, which excel at understanding image content and generating natural language descriptions.
- After launching Ollama on your local machine (typically at `localhost:11434`), you can send images for analysis and receive automated alt text.
- AI Foundry Local will soon offer similar capabilities.

### .NET 10: Running C# Scripts Directly

- .NET 10 enables direct execution of C# files with `dotnet run app.cs`, making script-based workflows simpler (no need for project scaffolding).
- This feature streamlines scripting tasks for automation and utility development.

### Sample Implementation

- The article shares a full C# code sample leveraging the OllamaSharp library to:
  - Load an image from the provided path.
  - Send the image to the selected vision model for alt text generation.
  - Output the generated text to the console—ideal for further automation or integration.
- Users are advised to resize large images to speed up inference.

### Resources and Learning

- References to key documentation, community resources, and sample code are provided to encourage experimentation and deeper learning.
- Suggestions include trying different models, customizing prompts, or enhancing the script's output handling for various real-world needs.

### Takeaways and Next Steps

- The combination of .NET 10's scripting capabilities and local AI models like Ollama unlocks new application scenarios beyond chatbots, including offline media analysis and content automation.
- Experimentation is encouraged as a pathway to both accessibility improvement and AI integration in .NET applications.

## Relevant Links

- [Official .NET 10 Blog](https://devblogs.microsoft.com/dotnet/announcing-dotnet-run-app/)
- [Ollama](https://ollama.com)
- [OllamaSharp Library](https://github.com/elbruno/ollamasharp)
- [Generative AI for Beginners (.NET)](https://aka.ms/genainet)
- [Sample Code Gist](https://gist.github.com/elbruno/4396c9ee3e56d1c86d280faa33b8f9fe)
- [AI Foundry](https://aka.ms/aifoundry)

## Conclusion

This guide provides a practical recipe for leveraging local AI for accessibility in .NET, with all tools and samples in place for developers to get started and adapt the approach to various use cases.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/alttext-generator-csharp-local-models/)
