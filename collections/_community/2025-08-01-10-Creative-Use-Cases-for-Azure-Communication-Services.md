---
layout: "post"
title: "10 Creative Use Cases for Azure Communication Services"
description: "Sean Keegan explores innovative scenarios for Azure Communication Services (ACS), showing how to combine ACS with other Azure offerings like OpenAI, Event Grid, and Bot Framework. The article covers voice assistants, real-time messaging, natural-language scheduling, omnichannel messaging, and integrating with Teams and Dynamics—illustrating real-world, developer-tested solutions."
author: "seankeegan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-communication-services/10-things-you-might-not-know-you-could-do-with-azure/ba-p/4438775"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-01 13:06:56 +00:00
permalink: "/community/2025-08-01-10-Creative-Use-Cases-for-Azure-Communication-Services.html"
categories: ["AI", "Azure", "Coding", "Security"]
tags: ["AI", "Azure", "Azure Communication Services", "Azure OpenAI", "Bot Framework", "Coding", "Community", "Cost Estimation", "Developer Workflow", "Dynamics 365", "Event Grid", "HIPAA Compliance", "Natural Language Processing", "Real Time Messaging", "Security", "SMS Scheduling", "Teams Interop", "Video Calling", "Voice Assistant", "WhatsApp Integration", "Zammo.ai"]
tags_normalized: ["ai", "azure", "azure communication services", "azure openai", "bot framework", "coding", "community", "cost estimation", "developer workflow", "dynamics 365", "event grid", "hipaa compliance", "natural language processing", "real time messaging", "security", "sms scheduling", "teams interop", "video calling", "voice assistant", "whatsapp integration", "zammodotai"]
---

In this article, Sean Keegan shares ten innovative ways to leverage Azure Communication Services, combining ACS with tools like Azure OpenAI and Event Grid to build intelligent, real-time, and secure communication solutions.<!--excerpt_end-->

## Overview

Azure Communication Services (ACS) offers modular, cloud-based capabilities for voice, video, chat, and SMS. Rather than just a product overview, this article shares ten creative, practical scenarios—rooted in real developer experiences—demonstrating how ACS can be extended with other Azure services into powerful solutions. Links to code samples and resources are provided throughout.

---

### 1. Build a Voice Assistant That Understands and Responds

- **What:** A voice-first assistant using ACS, Azure OpenAI, and backend logic for natural language understanding and automated follow-up (e.g., sending recipe links via SMS).
- **Why:** Surpasses standard IVRs with conversational, action-oriented interfaces—ideal for support or concierge scenarios.
- **Demo:** [Realtime voice agent on GitHub](https://github.com/anujb-msft/communication-services-realtime-voice-agent)

---

### 2. Send Responsive Messages in Real-Time

- **What:** Dispatch personalized messages based on real-time events (e.g., missed appointments, failed logins) using ACS, Event Grid, and OpenAI.
- **Why:** Enables timely, scalable communication beyond static reminders.
- **Resources:**
  - [Communication Services as Event Grid source](https://learn.microsoft.com/en-us/azure/event-grid/event-schema-communication-services)
  - [Handle SMS events with Event Grid](https://learn.microsoft.com/en-us/azure/communication-services/quickstarts/sms/handle-sms-events)

---

### 3. Schedule Appointments by Text Using Natural Language

- **What:** Users can book, confirm, or reschedule appointments solely via SMS—no app or menu—with ACS and Azure OpenAI decoding intent.
- **Why:** Streamlines scheduling for clinics, service providers, and more.
- **Demo:** [SMS appointment scheduler on GitHub](https://github.com/pereiralex/sms-appointment-scheduler)

---

### 4. Enable Omnichannel Messaging: WhatsApp, SMS, and Email

- **What:** Use ACS Advanced Messaging SDK to send/receive via WhatsApp, SMS, and email in a unified workflow.
- **Why:** Meet customers where they are without rebuilding infrastructure.
- **Resources:**
  - [WhatsApp Quickstart guide](https://learn.microsoft.com/en-us/azure/communication-services/quickstarts/advanced-messaging/whatsapp/get-started)
  - [Advanced Messaging for WhatsApp overview](https://learn.microsoft.com/en-us/azure/communication-services/concepts/advanced-messaging/whatsapp/whatsapp-overview)

---

### 5. Allow Customers to Join Teams Meetings Without an Account

- **What:** Embed a browser-based Teams meeting powered by ACS and Teams interop into your app or site.
- **Why:** Reduces friction for external users in sectors like healthcare or finance.
- **Demo:** [Interop-quickstart on GitHub](https://github.com/Azure-Samples/communication-services-javascript-quickstarts/tree/main/place-interop-group-calls)

---

### 6. Integrate Teams Calls Inside Your CRM or Custom App

- **What:** Make and receive Teams calls natively in Dynamics 365 or custom contact center UIs with Teams Phone Extensibility and ACS Call Automation APIs.
- **Why:** Boosts agent productivity and enables AI-powered workflows (e.g., conversation summarization during calls).
- **Resource:** [Teams Phone Extensibility overview](https://learn.microsoft.com/en-us/azure/communication-services/concepts/interop/tpe/teams-phone-extensibility-overview)

---

### 7. Embed Secure Video Visits into Healthcare Apps

- **What:** Quickly add HIPAA-compliant video calling (with Azure AD B2C identity integration) using ACS.
- **Why:** Simplifies deployment of secure, branded telehealth or consultation products.
- **Demo:** [Sample Builder tutorial](https://learn.microsoft.com/en-us/azure/communication-services/tutorials/virtual-visits/sample-builder)

---

### 8. Combine AI and Human Support in a Single Chat

- **What:** Create chat that starts as AI-powered (Azure OpenAI) and seamlessly escalates to a human agent via ACS, preserving conversation context.
- **Why:** Scales support operations while providing human intervention when needed.
- **Demo:** [Bot handoff sample](https://github.com/pereiralex/Simple-bot-handoff-sample)

---

### 9. Accelerate Voice-First AI Assistants with Zammo.ai

- **What:** Use Zammo.ai and ACS to stand up branded voice experiences—such as multilingual vaccine info hotlines—in days, not months.
- **Why:** Helpful for rapid-response needs or scaling support teams quickly.
- **Resource:** [Montgomery County success story](https://www.microsoft.com/en/customers/story/1354241398257818859-zammo-ai-partner-professional-services-cognitive-services)

---

### 10. Estimate Costs Before You Build

- **What:** Use ACS’ transparent, pay-as-you-go pricing model, calculator, and estimator to budget solutions.
- **Why:** Mitigates surprises and enables flexible prototyping and scaling.
- **Resources:**
  - [Pricing overview](https://azure.microsoft.com/en-us/pricing/details/communication-services/)

---

### Conclusion

Azure Communication Services, when paired with other Azure offerings like OpenAI, Event Grid, or Bot Framework, provides a powerful toolkit for building secure, intelligent, and flexible communication solutions across channels and industries.

For documentation, samples, and more scenarios, visit the [official Azure Communication Services documentation](https://learn.microsoft.com/en-us/azure/communication-services/).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-communication-services/10-things-you-might-not-know-you-could-do-with-azure/ba-p/4438775)
