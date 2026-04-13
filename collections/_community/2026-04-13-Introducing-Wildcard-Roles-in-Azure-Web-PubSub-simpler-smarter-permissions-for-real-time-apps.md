---
tags:
- Access Tokens
- Authorization
- Azure
- Azure Web PubSub
- Bots
- Community
- Group Roles
- Managed Service
- Permissions
- Publish Subscribe
- Real Time Architecture
- Real Time Messaging
- Role Based Access Control
- Token Generation
- Trading Platform Example
- WebSocket
- Wildcard Roles
primary_section: azure
date: 2026-04-13 06:47:05 +00:00
author: kevinguo
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-wildcard-roles-in-azure-web-pubsub-simpler-smarter/ba-p/4509524
section_names:
- azure
title: 'Introducing Wildcard Roles in Azure Web PubSub: simpler, smarter permissions for real-time apps'
feed_name: Microsoft Tech Community
---

kevinguo introduces wildcard group roles in Azure Web PubSub, explaining how pattern-based permissions reduce token bloat and simplify authorization for real-time apps that use many dynamic groups.<!--excerpt_end-->

# Introducing Wildcard Roles in Azure Web PubSub: simpler, smarter permissions for real-time apps

Real-time interactivity is now expected in many systems—collaborative dashboards, trading platforms, IoT monitoring, and live visualizations. Azure Web PubSub is a fully managed Azure service for real-time messaging over WebSocket so you can broadcast and receive updates without running your own connection infrastructure.

This post introduces a new capability in Azure Web PubSub: **wildcard patterns for defining client permissions in groups**, which makes permission management simpler and more scalable.

## Understanding Azure Web PubSub

Azure Web PubSub enables real-time messaging through a managed service:

1. Your backend generates a **client access token** and returns it to the client.
2. The client connects to Azure Web PubSub over **WebSocket** using that token.
3. After connecting, both backend and client can send/receive messages through the service.
4. Clients can be organized into **groups** (for example, trading rooms or dashboards).

Groups let you target specific audiences efficiently—for example:

- send to `dashboard/operations`
- receive updates from `market/NASDAQ/MSFT`

To maintain security, tokens carry **roles** that define what the client is allowed to do (join/leave groups, send to groups, etc.). Example token generation (arguments omitted):

```javascript
// Arguments omitted for simplicity
const WebPubSubServiceClient = new WebPubSubServiceClient();

WebPubSubServiceClient.getClientAccessToken({
  roles: [
    "webpubsub.joinLeaveGroup.dashboard/operations",
    "webpubsub.sendToGroups.dashboard/operations",
    "webpubsub.sendToGroups.market/NASDAQ/MSFT"
  ]
});
```

## The current permission model: literal roles

Historically, Azure Web PubSub used **literal group roles**, meaning each role targets a specific group name.

Example:

```javascript
roles: [
  "webpubsub.joinLeaveGroup.room123",
  "webpubsub.sendToGroup.room123"
];
```

This is clear and strict: the client can only act on `room123`.

### Where it becomes painful

If your system creates many dynamic groups (accounts, projects, classrooms, etc.), issuing one role per group becomes cumbersome. The backend often needs to:

- track all groups a user is authorized for
- generate large tokens with many role strings
- refresh tokens when group access changes

## The new capability: wildcard patterns for group roles

**Wildcard roles** let you specify permissions using patterns instead of enumerating every single group.

Example:

```javascript
roles: [
  "webpubsub.joinLeaveGroups.room/*",
  "webpubsub.sendToGroups.room/*"
];
```

With this, a client can join/send to any matching group name covered by the pattern.

Documentation: https://learn.microsoft.com/azure/azure-web-pubsub/concept-wildcard-group-roles

## Real-world examples

| Industry | Example | Benefit of wildcard roles |
| --- | --- | --- |
| Finance | Risk monitoring bots subscribing to all trading accounts | One role covers `account:*` groups |
| Gaming | Matchmaking service observing all `lobby:*` rooms | Simplifies admin tools |
| Education | Teacher dashboard viewing all `class:*` groups | Fewer roles, easier permission management |
| Collaboration | Logging all messages across `project:*` for auditing | Centralized monitoring without large tokens |

## Why this matters for developers

Wildcard roles are aimed at dynamic or large-scale systems:

- **Simpler token management**: fewer refreshes when group membership changes.
- **Smaller tokens**: one pattern can replace many literal roles.
- **Dynamic authorization**: new groups that match existing patterns can be accessed without regenerating tokens.

## Deep dive: financial trading platform example

Sample code: https://github.com/Azure/azure-webpubsub/tree/main/samples/javascript/wildcard-trading

### Setup

The example platform includes:

- a trading dashboard for managers
- a trading dashboard for human traders
- two risk analysis bots:
  - a hardcoded risk bot (deterministic rules)
  - an LLM-based risk bot (uses AI models to detect anomalies)

Each trading account has its own group(s), for example:

- `account/1234/trades` – trade updates
- `account/1234/orders` – order events
- `market/NYSE` – market data

The backend publishes events to these groups and clients subscribed to those groups receive real-time updates.

### Before: literal roles

Bots needed explicit roles per account/group:

```javascript
roles: [
  "webpubsub.joinLeaveGroup.account/1234/trades",
  "webpubsub.joinLeaveGroup.account/5678/trades",
  "webpubsub.joinLeaveGroup.account/9012/orders"
];
```

When new accounts were created, tokens had to be regenerated to add roles.

### Now: wildcard roles

Bots can be granted broad access with compact tokens:

```javascript
roles: [
  "webpubsub.joinLeaveGroups.account/*",
  "webpubsub.joinLeaveGroups.market/*"
];
```

This gives access to existing and future matching groups without token regeneration.

### How the risk bots work (at a high level)

| Component | Behavior |
| --- | --- |
| Hardcoded risk bot | Deterministic rules (example: if position size > 100, trigger alert) |
| LLM risk bot | Uses AI models to identify anomalies/fraud/market stress |
| Backend publisher | Emits order/trade events to `account:*` and `market:*` groups |

When a trade event is published:

1. both bots receive it in real time (via wildcard subscriptions)
2. each evaluates it
3. if risk is detected, an alert is published to `alerts/risk/*`

Traders still use strict, literal roles for isolation:

```javascript
roles: ["webpubsub.joinLeaveGroup.account/1234/trades"];
```

This highlights a practical split:

- automation/monitoring: wildcard roles for flexibility
- end users: literal roles for strict access control

## Summary

Wildcard roles in Azure Web PubSub reduce operational complexity for real-time systems with many dynamic groups by shrinking token role lists and reducing how often you need to re-issue tokens, while still allowing strict, literal permissions where isolation matters.


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/introducing-wildcard-roles-in-azure-web-pubsub-simpler-smarter/ba-p/4509524)

