---
date: 2026-03-24 02:11:47 +00:00
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/take-control-of-every-message-partial-failure-handling-for/ba-p/4504893
feed_name: Microsoft Tech Community
section_names:
- azure
- dotnet
author: swapnil_nagar
title: Partial failure handling for Azure Functions Service Bus batch triggers with per-message settlement
primary_section: dotnet
tags:
- .NET
- Azure
- Azure Functions
- Azure Functions Programming Model V2
- Azure Service Bus
- Batch Processing
- C# Isolated Worker
- Community
- Dead Letter Queue
- Exponential Backoff
- Idempotency
- Message Abandon
- Message Completion
- Message Deferral
- Message Settlement
- Node.js
- Per Message Settlement
- Poison Messages
- Python
- Retry Policies
- Service Bus Trigger
- ServiceBusMessageActions
- TypeScript
---

swapnil_nagar explains how to avoid all-or-nothing retries when processing Azure Service Bus messages in Azure Functions batch mode by manually settling each message (complete/abandon/dead-letter/defer), with examples in Node.js, Python, and .NET.<!--excerpt_end-->

# Partial failure handling for Service Bus triggers in Azure Functions

## The problem: all-or-nothing batch failures

When you use **Azure Functions** with an **Azure Service Bus** trigger in **batch mode**, a single function invocation can receive many messages (for example, 50) for higher throughput.

In the default behavior, if **one message fails**, **the entire batch fails** and all messages are retried—including the ones that already processed successfully. That can lead to:

- Duplicate processing
- Wasted compute (re-running already successful work)
- Infinite retry loops when a “poison message” is in the batch
- Extra idempotency requirements for downstream systems

Azure Functions addresses this with **per-message settlement**.

## The solution: per-message settlement (manual settlement actions)

Azure Functions can give you **direct control over how each message is settled** while processing a batch. Instead of treating the batch as a single unit, you settle messages individually.

Settlement actions:

| Action | What it does |
| --- | --- |
| **Complete** | Removes the message from the queue (success) |
| **Abandon** | Releases the lock so it returns to the queue for retry; can also modify application properties |
| **Dead-letter** | Moves the message to the dead-letter queue (DLQ) |
| **Defer** | Keeps the message in the queue but makes it retrievable only by sequence number |

Example outcome in a batch of 50:

- Complete 47 successful messages
- Abandon 2 transient failures (with updated retry metadata)
- Dead-letter 1 malformed message

All within one invocation—without reprocessing the successful messages.

## Why this matters

### 1) Eliminates duplicate processing

Completing messages individually removes successful messages immediately, even if other messages in the same batch fail.

### 2) Enables granular error handling

You can handle different failure types differently:

- Malformed payload → dead-letter
- Transient dependency issues (like DB timeouts) → abandon for retry
- Needs manual intervention / special workflow → defer

### 3) Exponential backoff without extra infrastructure

By using **abandon** and updating **application properties** (for example, retry count and timestamps), you can implement retry/backoff logic in code—without adding extra queues or Durable Functions.

### 4) Reduces cost

Avoids paying for redundant compute from reprocessing already-successful messages.

### 5) Simplifies idempotency requirements

If successful messages aren’t redelivered, downstream consumers can often be simpler (though idempotency is still a good practice).

## Code examples

### Node.js (TypeScript) with `@azure/functions-extensions-servicebus`

```typescript
import '@azure/functions-extensions-servicebus';
import { app, InvocationContext } from '@azure/functions';
import {
  ServiceBusMessageContext,
  messageBodyAsJson
} from '@azure/functions-extensions-servicebus';

interface Order {
  id: string;
  product: string;
  amount: number;
}

export async function processOrderBatch(
  sbContext: ServiceBusMessageContext,
  context: InvocationContext
): Promise<void> {
  const { messages, actions } = sbContext;

  for (const message of messages) {
    try {
      const order = messageBodyAsJson<Order>(message);
      context.log(`Processing order: ${order.id}`);

      // Simulate business logic
      await processOrder(order);

      // Complete — message processed successfully
      await actions.complete(message);
      context.log(`Order ${order.id} completed`);
    } catch (error) {
      if (error instanceof SyntaxError) {
        // Malformed JSON — dead-letter immediately
        context.error(`Poison message ${message.messageId}:`, error);
        await actions.deadletter(message);
      } else {
        // Transient failure — abandon with retry metadata
        const retryCount = Number(message.applicationProperties?.retryCount ?? 0);

        if (retryCount >= 3) {
          context.error(
            `Message ${message.messageId} exceeded max retries. Dead-lettering.`
          );
          await actions.deadletter(message);
        } else {
          context.warn(
            `Transient error for ${message.messageId}, retry ${retryCount + 1}`
          );
          await actions.abandon(message, {
            retryCount: (retryCount + 1).toString(),
            lastError: String(error),
            lastRetryTime: new Date().toISOString()
          });
        }
      }
    }
  }
}

async function processOrder(order: Order): Promise<void> {
  // Your business logic here
}

app.serviceBusQueue('processOrderBatch', {
  connection: 'ServiceBusConnection',
  queueName: 'orders-queue',
  sdkBinding: true,
  autoCompleteMessages: false,
  cardinality: 'many',
  handler: processOrderBatch
});
```

Key points:

- Set `sdkBinding: true` and `autoCompleteMessages: false` to enable manual settlement.
- `ServiceBusMessageContext` provides `messages` and `actions`.
- Use `complete()`, `abandon()`, `deadletter()`, `defer()`.
- You can update application properties when abandoning a message to track retries.

Full sample:

- https://github.com/Azure/azure-functions-nodejs-extensions/tree/main/azure-functions-nodejs-extensions-servicebus/samples/serviceBusSampleWithComplete

### Python (V2 programming model)

```python
import logging
from typing import List

import azure.functions as func
import azurefunctions.extensions.bindings.servicebus as servicebus

app = func.FunctionApp(http_auth_level=func.AuthLevel.FUNCTION)

@app.service_bus_queue_trigger(
    arg_name="receivedmessage",
    queue_name="QUEUE_NAME",
    connection="SERVICEBUS_CONNECTION",
    cardinality="many"
)
def servicebus_queue_trigger(receivedmessage: List[servicebus.ServiceBusReceivedMessage]):
    logging.info("Python ServiceBus queue trigger processed message.")
    for message in receivedmessage:
        logging.info(
            "Receiving: %s\n"
            "Body: %s\n"
            "Enqueued time: %s\n"
            "Lock Token: %s\n"
            "Message ID: %s\n"
            "Sequence number: %s\n",
            message,
            message.body,
            message.enqueued_time_utc,
            message.lock_token,
            message.message_id,
            message.sequence_number,
        )

@app.service_bus_topic_trigger(
    arg_name="receivedmessage",
    topic_name="TOPIC_NAME",
    connection="SERVICEBUS_CONNECTION",
    subscription_name="SUBSCRIPTION_NAME",
    cardinality="many"
)
def servicebus_topic_trigger(receivedmessage: List[servicebus.ServiceBusReceivedMessage]):
    logging.info("Python ServiceBus topic trigger processed message.")
    for message in receivedmessage:
        logging.info(
            "Receiving: %s\n"
            "Body: %s\n"
            "Enqueued time: %s\n"
            "Lock Token: %s\n"
            "Message ID: %s\n"
            "Sequence number: %s\n",
            message,
            message.body,
            message.enqueued_time_utc,
            message.lock_token,
            message.message_id,
            message.sequence_number,
        )
```

Key points:

- Uses `azurefunctions.extensions.bindings.servicebus` for SDK-type bindings with `ServiceBusReceivedMessage`.
- Supports queue and topic triggers with `cardinality="many"` for batch processing.
- Messages expose SDK properties (body, enqueue time, lock token, message ID, sequence number).

Full sample:

- https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-bindings-servicebus/samples/servicebus_samples_batch

### .NET (C#) isolated worker

```csharp
[Function(nameof(ProcessOrderBatch))]
public async Task ProcessOrderBatch(
    [ServiceBusTrigger("orders-queue", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage[] messages,
    ServiceBusMessageActions messageActions)
{
    foreach (ServiceBusReceivedMessage message in messages)
    {
        try
        {
            var order = message.Body.ToObjectFromJson<Order>();
            await ProcessOrder(order);

            // Success — complete it
            await messageActions.CompleteMessageAsync(message);
        }
        catch (InvalidOperationException ex)
        {
            // Poison message — dead-letter it
            await messageActions.DeadLetterMessageAsync(
                message,
                deadLetterReason: "MalformedPayload",
                deadLetterErrorDescription: ex.Message);
        }
        catch (TimeoutException)
        {
            // Transient error — abandon for retry with updated metadata
            await messageActions.AbandonMessageAsync(
                message,
                new Dictionary<string, object>
                {
                    ["retryCount"] = GetRetryCount(message) + 1
                });
        }
    }
}
```

Key points:

- Inject `ServiceBusMessageActions` alongside the message array.
- Settle each message independently with `CompleteMessageAsync`, `DeadLetterMessageAsync`, or `AbandonMessageAsync`.
- You can modify application properties on abandon to track retry metadata.

Full sample:

- https://github.com/Azure/azure-functions-dotnet-worker/blob/main/samples/Extensions/ServiceBus/ServiceBusReceivedMessageFunctions.cs

## Comparison: Azure Functions vs AWS Lambda (SQS)

| Capability | AWS Lambda (SQS) | Azure Functions (Service Bus) |
| --- | --- | --- |
| Batch processing | ✅ Event source mapping | ✅ Batch trigger (`cardinality: many`) |
| Partial failure handling | ✅ `ReportBatchItemFailures` | ✅ Per-message settlement |
| Dead-letter individual messages | ❌ via SQS redrive policy | ✅ `deadletter()` per message |
| Abandon with modified properties | ❌ Not supported | ✅ `abandon()` with property updates |
| Defer messages | ❌ Not supported | ✅ `defer()` per message |
| Settlement granularity | Binary (succeeded/failed) | 4 actions (complete/abandon/dead-letter/defer) |

Azure Functions provides richer settlement semantics than a binary success/failure model per message.


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/take-control-of-every-message-partial-failure-handling-for/ba-p/4504893)

