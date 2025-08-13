---
layout: "post"
title: "Multimodal Vision Intelligence with .NET MAUI"
description: "David Ortinau explores how to enhance .NET MAUI apps with multimodal AI capabilities, focusing on integrating image capture and AI-driven analysis. The article covers technical steps to add camera and photo gallery features, process images, and use Microsoft.Extensions.AI for extracting structured data, enabling advanced, user-friendly mobile experiences."
author: "David Ortinau"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/multimodal-vision-intelligence-with-dotnet-maui/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-06-17 19:30:00 +00:00
permalink: "/2025-06-17-Multimodal-Vision-Intelligence-with-NET-MAUI.html"
categories: ["AI", "Coding"]
tags: [".NET", ".NET MAUI", "AI", "Azure AI Foundry", "C#", "Coding", "Community Toolkit", "Computer Vision", "Copilot", "IChatClient", "Image Analysis", "MediaPicker", "Microsoft.Extensions.AI", "Mobile Development", "Multimodal", "News", "Prompt Engineering"]
tags_normalized: ["net", "net maui", "ai", "azure ai foundry", "c", "coding", "community toolkit", "computer vision", "copilot", "ichatclient", "image analysis", "mediapicker", "microsoft dot extensions dot ai", "mobile development", "multimodal", "news", "prompt engineering"]
---

In this article, David Ortinau details the process of building multimodal, AI-powered image analysis into .NET MAUI apps, leveraging .NET tooling and Microsoft.Extensions.AI.<!--excerpt_end-->

## Overview

David Ortinau describes the process and technical considerations involved in expanding .NET MAUI applications to support multimodal AI—particularly, augmenting a 'to do' app to accept image input, analyze its content using AI, and provide actionable project/task suggestions. This follows his previous work on voice input integration.

## Multimodal Enhancement in .NET MAUI

- **Voice to Vision:** The article shifts the focus from voice input to leveraging device cameras and galleries, allowing users to feed images into the app for analysis.
- **User Experience:** Users can tap a camera button on the main page to access image capture via the cross-platform `MediaPicker` API, which provides a familiar, native experience for photo input.
- **PhotoPageModel & EventToCommandBehavior:** Implementation utilizes `EventToCommandBehavior` to tie lifecycle events to commands through the MVVM toolkit, ensuring the code remains clean and maintainable.
- **Platform Awareness:** Code checks the user's device type (desktop, mobile, etc.) to decide whether to present file picking or direct camera capture.

## Processing Images with AI

- Once an image is obtained, it is displayed alongside an input field for users to supplement the context or instructions to the AI.
- The AI prompt is dynamically built using a `StringBuilder`, optionally including user-provided instructions.
- **Microsoft.Extensions.AI:** The workflow employs an `IChatClient` instance to send a `ChatMessage` containing both text and image data to the AI service.
- The AI agent analyzes the image for project- and task-related information, performing text extraction and formatting while using contextual cues for more meaningful results than standard OCR.

## Human-Centric Workflow

- After the AI processes the image, it returns structured proposals for projects and tasks.
- Users review and confirm the results, preserving user control while benefiting from AI augmentation.
- Resources like the HAX Toolkit are recommended for designing collaborative AI workflows.

## Technical Resources

- Source code for the sample Telepathic app, documentation for MediaPicker, Microsoft.Extensions.AI, and best practices for responsible AI development are included for further reading.

## Conclusion

Modern .NET MAUI apps can now easily harness AI for vision, thanks to cross-platform APIs and Microsoft’s modular AI infrastructure. These capabilities empower developers to create richer, more accessible multimodal experiences by combining traditional input methods with voice and vision.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/multimodal-vision-intelligence-with-dotnet-maui/)
