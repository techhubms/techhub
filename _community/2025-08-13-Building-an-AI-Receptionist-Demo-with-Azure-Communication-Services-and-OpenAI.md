---
layout: "post"
title: "Building an AI Receptionist: Demo with Azure Communication Services and OpenAI"
description: "This post provides a detailed walkthrough of building an AI-powered receptionist using Azure Communication Services and Azure OpenAI. The guide covers real-time SMS interactions, event-driven design, FastAPI integration, and strategies for moving the demo to production. Readers will learn how to integrate multiple Azure components, handle conversation logic, manage simulated calendars, and apply prompt engineering for a natural chat experience."
author: "seankeegan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-communication-services/building-an-ai-receptionist-a-hands-on-demo-with-azure/ba-p/4442959"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-13 16:35:46 +00:00
permalink: "/2025-08-13-Building-an-AI-Receptionist-Demo-with-Azure-Communication-Services-and-OpenAI.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "Appointment Scheduling", "Authentication", "Azure", "Azure Communication Services", "Azure OpenAI", "Calendar API", "Coding", "Community", "Compliance", "Conversational AI", "Event Driven Architecture", "Event Grid", "FastAPI", "Logging", "Production Readiness", "Prompt Engineering", "Python", "SMS Integration", "Webhook Validation", "Webhooks"]
tags_normalized: ["ai", "appointment scheduling", "authentication", "azure", "azure communication services", "azure openai", "calendar api", "coding", "community", "compliance", "conversational ai", "event driven architecture", "event grid", "fastapi", "logging", "production readiness", "prompt engineering", "python", "sms integration", "webhook validation", "webhooks"]
---

seankeegan demonstrates how to build an AI receptionist using Azure Communication Services and Azure OpenAI, offering a hands-on guide for developers incorporating SMS, event-driven workflows, and conversational AI logic.<!--excerpt_end-->

# Building an AI Receptionist: Demo with Azure Communication Services and OpenAI

seankeegan presents a step-by-step walkthrough for creating an AI-powered receptionist that handles SMS appointment scheduling using Azure Communication Services and Azure OpenAI. This guide enables developers to build interactive SMS bots capable of confirming or rescheduling appointments in a natural, event-driven way.

## Overview

Missed appointments due to inflexible SMS systems are common. This guide details how to build a text-based AI receptionist allowing users to update appointments with conversational replies, using Microsoft Azure services.

## Architecture and Core Components

- **Azure Communication Services**: Empowers a real SMS number, receives inbound messages, and sends replies.
- **Azure Event Grid**: Delivers real-time SMS events to a webhook.
- **FastAPI**: Hosts the webhook, manages in-memory state for conversations and appointments.
- **Azure OpenAI**: Generates conversational AI responses for appointment scheduling and confirmations.
- **Simulated Calendar**: Prebuilt set of 30 business days, marking realistic openings.

### Workflow

1. **User texts the Azure-powered number**.
2. **Azure Communication Services** triggers an inbound event.
3. **Azure Event Grid** posts the event to the FastAPI webhook.
4. The webhook acknowledges receipt and triggers background processing.
5. Scheduling conversation state is updated; for first messages, a default appointment is created.
6. Open slots are chosen from the simulated calendar and passed into the system prompt.
7. A context-appropriate reply is generated with Azure OpenAI and sent back via Azure Communication Services.

## Demo vs. Production Guidance

- **Demo**: Conversation starts with user-initiated SMS.
- **Production**: Typically begins with automatic reminders; can be enhanced with real scheduling APIs, persistence, authentication, webhook validation, logging, and compliance layers, all using the same event-driven design.

## Technical Tricks and Prompt Design

- **Calendar Trick**: Prebuilt 30-day weekday calendar, 30-minute slots, ~80% booked to maintain realism.
- **Prompt Engineering**: System prompt frames the AI as a receptionist limited to specific domains and offers only up-to-date, plausible appointment times. Replies are concise and confirmation-focused.

## Extending the Solution

- **Productization**: Persistent storage, rich calendar integration (Outlook, EHRs), authentication and webhook validation, structured logging, and rate limiting.
- **Intelligence**: Integrate function/tool calls, user preferences, and time zone handling.
- **Safety**: Compliance/moderation, retrieval-augmented responses.
- **Analytics**: Track confirmations, latency, and fallback triggers.

## Getting Started

1. Clone the demo repository: [sms-appointment-scheduler](https://github.com/pereiralex/sms-appointment-scheduler).
2. Set up environment variables using the template.
3. Configure Azure Communication Services connection, phone number, and Azure OpenAI API details.
4. Install dependencies and launch FastAPI on port 8000.
5. Expose the local port via a dev tunnel and configure Azure Event Grid subscriptions.
6. Text the assigned number and observe bidirectional SMS scheduling in action.

## Conclusion

This tutorial helps developers rapidly prototype and extend an AI SMS receptionist using event-driven Microsoft cloud services and modern Python frameworks. The architecture supports further enhancements such as real calendar integration, compliance, and user analytics.

For full instructions and code, visit the [demo repository](https://github.com/pereiralex/sms-appointment-scheduler).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-communication-services/building-an-ai-receptionist-a-hands-on-demo-with-azure/ba-p/4442959)
