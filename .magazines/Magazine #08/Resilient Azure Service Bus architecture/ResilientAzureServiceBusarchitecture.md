# Resilient Azure Service Bus architecture

During one of our innovation days at Xpirit, we looked at how we could
make a system that we are working on more resilient against outages. Our
application should be able to failover to a secondary region and
failback to the primary region without much effort. For our innovation
day, we specifically focused on protecting the system against Azure
Service Bus outages. The messages passed to this system are critical,
and we wanted to narrow the chances that we missed a message due to an
outage. Another characteristic of the system is that the throughput of
the total number of messages that we receive can be considered low: we
process about 1000 messages every 24 hours. With these things in mind,
we investigated the various approaches that we could use and worked
towards a solution.

The goal of this article is to explain a number of architectural
patterns that we explored for Azure Service Bus. These patterns will
allow your system to cope with the fact that Azure Service Bus may go
down at some point in time. None of the patterns in this article will
guarantee that no messages can be lost, but most of them will reduce the
chance of losing messages significantly.

We will not go into detail about queues, topics, and subscriptions.
Instead, we will explain how you can use multiple Azure Service Bus
namespaces in different regions to add resiliency to your systems.

As an example, we will use a system that processes fines for speeding
cars. The system consists of an automatic speed trap that sends the
speed and license plate to a backend service whenever it detects that a
car is speeding. The fines services receives these messages and
processes the fine, making sure the owner of the car receives the fine.
The messages between the automatic speed trap and the fines service are
sent using an Azure Service Bus namespace. Communication is one-way; the
fines service does not send any confirmation or reply to the automatic
speed trap. For simplicity sake, there are also no other parts in this
system that communicate with either the automatic speed trap or the
fines service. In this example, the automatic speed trap relies on the
assumption that Azure Service Bus is available; it does not buffer or
store messages in any way. The following figure illustrates the example
system:

![](./media/image1.png)

Figure 1: example system

Now imagine a problem occurs, and Azure Service Bus goes down in West
Europe. The automatic speed trap can no longer send messages to Azure
Service Bus, and the fines service cannot process any fines. Speeding
cars are not reported to the fines service, so the owner of the car will
not get a fine. Let's investigate what patterns we can apply to make our
system resilient to such an outage.

## Failover namespaces

The first pattern we will look at is "Failover namespaces". This pattern
utilizes the "Azure Service Bus Geo-disaster recovery" feature of Azure
Service Bus, which is available as part of the premium SKU. When you
enable this feature, you create a new namespace in a different Azure
region. This new namespace will be the **secondary** namespace, and the
other namespace will be the **primary** namespace. A **namespace alias**
can also be configured that points to the **primary** namespace. The
automatic speed trap sends messages to the **namespace alias,** and the
fines service receives the message from the **namespace alias**. This
way the **primary** and **secondary** namespace can be swapped easily.

![](./media/image2.png)

Figure 2: failover namespaces no outage

As shown in figure 1, the **namespace alias** will point to the
**primary** namespace until Azure Service Bus goes down in that region.
Both the automatic speed trap and the fines service communicate with the
**namespace alias**, which can be compared to a CNAME DNS record.

![](./media/image3.png)

Figure 3: failover namespaces outage

When Azure Service Bus goes down in West Europe, we will have to execute
a failover either by pressing a button in the Azure Portal or by
invoking the Azure API. When this happens, the **namespace alias**
switches to our **secondary** namespace. Once we've done this, the
system will continue to send and receive messages.

We can only execute this failover once, and we cannot switch back to the
**primary** namespace once Azure Service Bus comes back up in West
Europe. It is possible to switch back to a namespace in West Europe, but
this requires that you set up and perform another failover to move back
to this region.

The downside of this pattern is that only queues, topics, subscriptions,
and filters are automatically mirrored from our **primary** namespace
into your **secondary** namespace. Messages are not mirrored, so the
messages that the receiver did not read from the **primary** namespace
will stay in the **primary** namespace. You will somehow have to extract
them from the **primary** namespace and move them into the **secondary**
namespace, otherwise they will not be processed.

Another downside is that we must initiate the failover explicitly,
either by pressing a button in the Azure Portal or by somehow triggering
the failover using the Azure API's. Until the failover is initiated,
both the automatic speed trap and the fines service will receive errors.
It is up to the automatic speed trap and the fines service to keep
retrying so that communication can continue once the failover is
initiated. Messages that were sent by the automatic speed trap but were
not yet received by the fines service are stuck in the **primary**
namespace and may be lost if the region does not fully recover from the
outage. Messages that are stuck in the **primary** namespace need to be
transferred to the **secondary** namespace either by a manual or
automatic process, in case the region hosting the **primary** namespace
fully recovers.

The benefit is that both the automatic speed trap and fines services are
unchanged. From that perspective, there is no difference between using a
namespace and using a namespace alias in your application. All you have
to do is configure a different connection string. So, if we do not have
control over the source code of the applications that use Azure Service
Bus, this pattern allows us to add a failover option without changing
any code. If required, we can even replace an existing namespace by a
namespace alias, which means that you don't even have to change your
configuration.

## Paired namespaces

The second pattern is "paired namespaces". With this pattern, we will
use one or more "backlog queues" in a **secondary** namespace that will
receive and hold the messages while the **primary** namespace is down.
This functionality is built into the Azure Service Bus client and can be
enabled by calling the "PairNamespaceAsync" method on the
"MessagingFactory" in your code.

When we pair two namespaces, the client will create one or more backlog
queues in a **secondary** namespace. We can configure the number of
backlog queues that are created. As long as the **primary** namespace is
available, messages are sent to the primary namespace. When the
**primary** namespace goes down, new messages are sent to one of the
backlog queues in the **secondary** namespace. Messages that were
already delivered to the **primary** namespace will not be resent to the
backlog queues. The backlog queue is chosen randomly from the available
backlog queues. The client will also continuously ping the **primary**
namespace to check whether it is available again. As soon as the
**primary** namespace is available again, the client will restart
sending messages to the **primary** namespace. Messages that are in the
backlog queues still need to be transferred back to the **primary**
namespace. This process is called siphoning and is also part of the
Azure Service Bus client. In this example, we have configured a separate
process that is responsible for the siphoning process.

Figure 3 describes the normal operation: the **primary** namespace is
reachable, the automatic speed trap sends all messages to the
**primary** namespace, and there are no messages for the siphoning
process to forward:

![](./media/image4.png)

Figure 4: paired namespaces no outage

At some, point the primary namespace goes down. The automatic speed trap
automatically sends messages to one of the four backlog queues. Also the
**primary** namespace will be pinged at regular intervals until it
becomes available again as shown in figure 4.

![](./media/image5.png)

Figure 5: paired namespaces outage

When the **primary** namespace becomes available, the automatic speed
trap will start sending the messages to the **primary** namespace again.
The messages that are in the backlog queues are read (received) by the
siphon process and are forwarded to the **primary** namespace, as shown
in figure 5. When the siphon process has read and forwarded all the
messages from the backlog queues, normal operation can continue as was
shown in figure 3.

![](./media/image6.png)

Figure 6: paired namespaces recovered from outage

In contrast to the failover namespace pattern, using the paired
namespace pattern does require that we modify the code of the automatic
speed trap application. Paired namespaces is a feature that is available
in the Azure Service Bus client, so we only have to configure it, not
write it ourselves. There is no need to change the fines services, but
the downside is that no messages are delivered while the primary
namespace is down. Our system will not break, and we will not lose
messages, but communication between the automatic speed trap and fines
service will stop until the primary namespace is available again.

The failover situation, when the automatic speed trap sends messages to
backlog queues, is triggered automatically when the **primary**
namespace goes down, and no manual intervention is required.
Communication to the **primary** namespace will also restore on its own
when the Azure Service Bus client detects the **primary** namespace is
available again.

A downside is that communication between the automatic speed trap and
fines service will stop until the **primary** namespace is available
again. Another downside is the order in which messages are delivered. If
we use multiple backlog queues, the messages are randomly delivered to
one of the available queues. When the messages are then received by the
siphon process and forwarded to the primary namespace, the order of
messages cannot be guaranteed.

## Passive-Active replication

Another option that we might want to consider is passive replication.
This pattern uses two namespaces, one of which we call our **primary**
namespace and the other is called our **secondary** namespace. The idea
is that there is only one **active** namespace at any time which handles
our messages. The automatic speed trap will send messages to the
**active** namespace, which will be the **primary** namespace when there
are no outages.

![](./media/image7.png)

Figure 7: active-passive no outage

The fines service listens to both namespaces and receives all the
messages it can find. When our system is running smoothly, the
**primary** namespace is the **active** namespace so messages are sent
through our **primary** namespace and our fines service handles those
messages. Now imagine an outage where the **primary** namespace goes
down. In this case we make our **secondary** namespace the new
**active** namespace. The automatic speed trap will now send messages
through the **secondary** namespace. Messages will continue to flow, so
we won't experience any downtime.

![](./media/image8.png)

Figure 8: active-passive outage

To make this work, we need to build two pieces that support this
pattern, one of these is the sender, and the other is the receiver. The
sender is straightforward in this scenario, and it should send a message
to the **primary** namespace and if that fails, it should send that same
message to the **secondary** namespace. We could implement a circuit
breaker here that breaks after a few attempts and checks the primary
namespace after a specific amount of time has passed.

The receiver side is a bit trickier. It needs to know how to receive
messages from both the **primary** and the **secondary** namespace. If
the logical order in which we receive the messages is important, we need
to make sure that the order in which messages are read from the
namespaces is as follows: if the **primary** namespace is down, read
from the **secondary** namespace, but when the **primary** namespace
gets back up again, first drain the **secondary** namespace and then
continue to read from the **primary** namespace, instead of immediately
switching back to the **primary** namespace. If the order is
unimportant, then we don't care, and our receiver can just listen to
both namespaces.

This setup gives us high availability and automatic failover without
having to touch a single button. However you have to build this
yourself; this pattern is not implemented by the service bus client.
There is still a (very small) chance that a message gets lost. Imagine
that the speed trap sends a ticket to our **primary** namespace. In this
case, there are no issues and the ticket is received by the **primary**
namespace. But at that moment our **primary** namespace could go down
before it had a chance to deliver the message to our fines service. If
that happens, that message may be lost if the region does not fully
recover, and there would be one lucky speeder that doesn't receive a
fine.

## Active-Active replication

With the active replication pattern, we make sure that the chances of
losing a message are even smaller than with passive replication.
Whenever an Active-Passive namespace holds a message that the namespace
received and if this namespace goes down, we lose the message. To
prevent losing those messages we could use the Active-Active pattern.

To set up the active replication pattern we need to have two namespaces,
a **primary** namespace and a **secondary** namespace. The automatic
speed trap actively sends all messages to both namespaces. In case the
**primary** namespace goes down, we still have the **secondary**
namespace that has received all the messages. If there is no outage, the
fines service will receive the same message from both the **primary**
and **secondary** namespace. To make sure our system doesn't have
duplicate entries, we must create a deduplication layer at the receiving
side (fines service).

![](./media/image9.png)

Figure 9: active-active no outage

In case the **primary** namespace goes down, we still receive all the
messages from the **secondary** namespace. In this case, the
deduplicator just passes our messages through to the fines service. You
can even add a third namespace in a different region to protect against
a simultaneous outage of both the **primary** and **secondary**
namespace, although this is likely to be overkill.

![](./media/image10.png)

Figure 10: active-active outage

To make this setup work we need to modify the automatic speed trap to
send all messages to both the **primary** and **secondary** namespace.
If it can't deliver to one of the namespaces it doesn't matter, as long
it can still deliver the message to the other namespace.

Our fines service must be a bit more advanced. It processes duplicate
messages if we don't filter the received messages in some way. Some
systems don't care if they process duplicate messages, but for our
system we only want one message to be processed in the fines service. To
do this, we need to implement a deduplication layer that ignores
messages that have already been received from another namespace. We can
do this by storing the unique id of each received message in a cache. If
your receiver is idempotent, you can choose to limit the number of cache
items and automatically evict the oldest items (FIFO). You can be fairly
certain that duplicate messages are delivered shortly after one another.
If the message id already exists in the cache, we know that we can
ignore the message. If the message id does not exist in the cache, we
know the message can be delivered to the fines service. If your receiver
is not idempotent, you must persist your processed message id's, for
example in a SQL database, which means you need to protect against SQL
outages as well.

This setup gives us high availability and we don't have to do anything
in order to failover. The system will just continue to work. However,
this setup adds complexity by having a deduplication layer that holds a
record of all the messages to find out whether there are duplicates. If
we choose to persist the message id's, we must think about high
availability for our caching layer, and this can be a nuisance. And this
is even more complex when there are multiple instances of the same
receiver. The benefit of this approach compared to passive replication
is that there is an even smaller chance that we lose a message. However,
the costs may outweigh the benefits depending on your solution. This
solution would also require you to have control over the source code
from the sender and the receiver.

# The pattern we chose ourselves

As stated at the beginning of this article, we explored these patterns
because we wanted to protect a real system we are working on against
outages of Azure Service Bus. We wanted an automatic failover and we
didn't only want an Azure Service Bus failover, but instead an entire
region failover, independent of the resources that are running. This
narrowed our choices to Active-Passive and Active-Active.

To make this decision we looked at the value of a message. For our
system, the value of a message doesn't outweigh the cost of implementing
the Active-Active pattern. The system does not receive many messages, so
if a region goes down, there is a small chance that we lose that
message. Considering all of this we've chosen for the Active-Passive
pattern.
