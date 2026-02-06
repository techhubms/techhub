---
external_url: https://www.reddit.com/r/csharp/comments/1mhb7xl/how_can_i_make_this_method_more_performant/
title: Improving Performance of a C# Console App Processing Azure Service Bus Deadletter Queues
author: TTwelveUnits
feed_name: Reddit CSharp
date: 2025-08-04 11:47:42 +00:00
tags:
- Async Programming
- Azure Service Bus
- Azure Storage
- C#
- Console App
- Deadletter Queue
- DevOps Pipeline
- Message Processing
- Performance
- Subscription
- Azure
- DevOps
- Community
- .NET
section_names:
- azure
- dotnet
- devops
primary_section: dotnet
---
TTwelveUnits asks for guidance on making a C# console app method more performant for processing Azure Service Bus deadletter messages, especially for large queues, as part of a nightly Azure DevOps pipeline.<!--excerpt_end-->

## Context

The author, identifying as a C# beginner, is developing a console application to clear Azure Service Bus deadletter queues for topic subscriptions. The app iterates through each subscription’s deadletter queue and archives messages older than 7 days to an Azure Storage account. Some subscriptions hold 80,000+ deadletter messages, making the process time-consuming. The method in question is invoked via a nightly Azure DevOps pipeline, and the author seeks ways to optimize its performance.

## Provided Method (Summarized)

```csharp
private async Task ProcessExistingDeadLetterMessagesAsync(
    string topicName, string subscriptionName, CancellationToken cancellationToken)
{
    Console.WriteLine($"Processing existing dead-letter messages: {topicName}/{subscriptionName}");
    var deadLetterPath = $"{topicName}/Subscriptions/{subscriptionName}/$DeadLetterQueue";
    await using var receiver = _busClient.CreateReceiver(deadLetterPath);
    int totalProcessed = 0;
    var cutoffDate = DateTime.UtcNow.AddDays(-7).Date;
    while (!cancellationToken.IsCancellationRequested)
    {
        var messages = await receiver.ReceiveMessagesAsync(
            maxMessages: 100, maxWaitTime: TimeSpan.FromSeconds(10), cancellationToken);
        if (!messages.Any())
        {
            Console.WriteLine($"No more messages found in DLQ: {topicName}/{subscriptionName}");
            break;
        }
        Console.WriteLine($"Processing batch of {messages.Count} messages from {topicName}/{subscriptionName}");
        foreach (var message in messages)
        {
            try
            {
                DateTime messageDate = message.EnqueuedTime.Date;
                if (messageDate ...
```

## Key Considerations

- The method retrieves 100 messages at a time (`ReceiveMessagesAsync`) from the deadletter queue.
- It processes messages in batches, archiving those older than 7 days to Azure Storage.
- The need for improved performance is due to high message volumes and the app’s nightly recurring nature.

## Potential Performance Bottlenecks

1. **Batch Size**: Processing 100 messages per batch may be suboptimal if Service Bus and network limits permit larger batches.
2. **Single-threaded Processing**: The code sequentially processes each message in a batch.
3. **Storage Archival**: If the archive-to-storage action is slow or synchronous, it may cause bottlenecks.

## General Performance Improvement Strategies

1. **Increase Batch Size** (if Service Bus quotas allow):
   - Try increasing `maxMessages` to the Service Bus client’s maximum (often up to 1000).
2. **Parallel Processing**:
   - Use `Task.WhenAll` to process and archive messages in parallel within each batch.
3. **Asynchronous Storage Calls**:
   - Ensure storage operations are performed asynchronously.
4. **Prefetch Count**:
   - Set prefetch count for receivers to fetch more messages per network call.
5. **Error Handling**:
   - Implement robust error handling/logging to avoid stalling on failed messages.
6. **Azure DevOps Pipeline Tuning**:
   - Consider running multiple instances for parallel queue/subscription processing.

## Sample Refactored Pseudocode (Partial)

```csharp
receiver.PrefetchCount = 1000; // set before the loop

while (!cancellationToken.IsCancellationRequested)
{
    var messages = await receiver.ReceiveMessagesAsync(
        maxMessages: 1000, maxWaitTime: TimeSpan.FromSeconds(10), cancellationToken);

    if (!messages.Any()) break;

    var archiveTasks = messages
        .Where(m => m.EnqueuedTime.Date < cutoffDate)
        .Select(async m => await ArchiveToStorageAsync(m));

    await Task.WhenAll(archiveTasks);
}
```

## Conclusion

Optimizing the method involves adjusting batch and prefetch sizes, employing parallel and asynchronous programming practices, and possibly scaling the process via pipeline concurrency. Concrete implementation may require understanding the quotas/limits in your Azure subscription and the capabilities of your environment.

---

For more specific advice, it would help to see the full message processing and archival code, and learn about any imposed Service Bus or Storage limits.

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mhb7xl/how_can_i_make_this_method_more_performant/)
