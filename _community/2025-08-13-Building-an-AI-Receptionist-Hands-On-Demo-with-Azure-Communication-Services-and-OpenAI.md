---
layout: "post"
title: "Building an AI Receptionist: Hands-On Demo with Azure Communication Services and OpenAI"
description: "This post presents a practical walkthrough for building an AI-powered SMS receptionist using Azure Communication Services, Azure OpenAI, FastAPI, and Event Grid. It guides developers from setting up a responsive conversational SMS system with a simulated calendar to extending the solution for production use. Topics include event-driven architecture, conversational prompt strategies, core Azure integrations, and tips for enhancing scheduling automation."
author: "seankeegan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-communication-services/building-an-ai-receptionist-a-hands-on-demo-with-azure/ba-p/4442959"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-13 16:35:46 +00:00
permalink: "/2025-08-13-Building-an-AI-Receptionist-Hands-On-Demo-with-Azure-Communication-Services-and-OpenAI.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "API Integration", "Appointment Management", "Azure", "Azure Communication Services", "Azure OpenAI", "Coding", "Community", "Conversational AI", "Event Driven Architecture", "Event Grid", "FastAPI", "Microsoft Cloud", "Prompt Engineering", "Python", "Scheduling Automation", "SMS Automation", "Webhook"]
tags_normalized: ["ai", "api integration", "appointment management", "azure", "azure communication services", "azure openai", "coding", "community", "conversational ai", "event driven architecture", "event grid", "fastapi", "microsoft cloud", "prompt engineering", "python", "scheduling automation", "sms automation", "webhook"]
---

In this post, seankeegan demonstrates how to build an AI-driven SMS receptionist with Azure Communication Services and Azure OpenAI, sharing step-by-step guidance for developers seeking to automate scheduling via conversational text.<!--excerpt_end-->

# Building an AI Receptionist: Hands-On Demo with Azure Communication Services and OpenAI

Ever missed an appointment due to a rigid SMS system? This post explains how to build a responsive AI receptionist capable of understanding SMS rescheduling requests and confirming new slots in natural language. Using Azure Communication Services, Azure OpenAI, FastAPI, and Azure Event Grid, the walkthrough takes you from initial setup to a production-ready architecture.

## Key Technologies Used

- **Azure Communication Services**: Provides SMS capabilities, real phone numbers, sending, and receiving messages.
- **Azure Event Grid**: Forwards SMS events in real time to your backend.
- **FastAPI**: Hosts webhooks and manages conversation state in memory.
- **Azure OpenAI**: Powers the AI agent, providing receptionist-like conversational responses about appointments.
- **Simulated Calendar**: Emulates 30 business days’ worth of appointment slots to keep the demo realistic.

## How It Works: Event-Driven SMS Flow

1. User messages the demo phone number (Azure Communication Services).
2. Azure Communication Services raises a “message received” event.
3. Azure Event Grid posts the event to the FastAPI-provided webhook endpoint.
4. The webhook kicks off a background process to handle the message (ensuring fast response to Event Grid).
5. For first-time users, a mock appointment and SMS reminder are created; subsequent messages append to history.
6. Available slots from the simulated calendar are included in the system prompt and sent to Azure OpenAI, which returns a context-aware receptionist reply.
7. The reply is delivered to the user via Azure Communication Services, and state is retained in memory.

No polling or scheduled scripts are needed—the architecture is fully event-driven and responsive to SMS activity.

## Demo vs. Production Considerations

- **Demo**: Direct texting to the system triggers appointment logic.
- **Production**: Integration with a real scheduling API, persistent storage for appointments, signed webhook validation, authentication, observability (logging, metrics), and compliance are added on top of the same event pipeline.

## Calendar and Prompt Engineering

- Calendar with 30 business days, 09:00–17:00 in 30-minute increments, 80% booked.
- Each SMS refreshes prompts with current slot availability, ensuring suggested times are realistic and bounded.
- System prompt restricts the AI to act as a friendly receptionist (no medical advice), offering specific available times and confirming changes concisely.

## Extending Further

- Connect to real scheduling APIs (Outlook, healthcare systems).
- Add authentication, rate limiting, structured logging, and tracing.
- Enhance prompt and conversation durability with persistent stores.
- Incorporate additional safety, compliance filters, and analytics.

## Running the Demo

1. Clone the [demo repository](https://github.com/pereiralex/sms-appointment-scheduler).
2. Copy `.env.example` to `.env` and fill in Azure connection strings, phone numbers, and OpenAI API settings.
3. Install dependencies, start the FastAPI app on port 8000.
4. Expose port 8000 via a development tunnel and configure Event Grid to deliver `SMSReceived` events to `/api/sms/webhook`.
5. Text the acquired Azure number and watch the event/reply cycle in the terminal.

For step-by-step details and setup links, see the [official quickstart](https://learn.microsoft.com/en-us/azure/communication-services/quickstarts/telephony/get-phone-number?tabs=windows&pivots=platform-azp-new) and the [repository README](https://github.com/pereiralex/sms-appointment-scheduler).

## Summary

With a combination of Azure Communication Services, OpenAI, FastAPI, and event-driven Azure services, you can prototype and scale an SMS-based, AI-powered appointment system. The modular setup demonstrates natural language understanding, real-time communication, and efficient scheduling automation. Extending the basic demo with persistent data and production features is straightforward, empowering further innovation for real-world rollouts.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-communication-services/building-an-ai-receptionist-a-hands-on-demo-with-azure/ba-p/4442959)
