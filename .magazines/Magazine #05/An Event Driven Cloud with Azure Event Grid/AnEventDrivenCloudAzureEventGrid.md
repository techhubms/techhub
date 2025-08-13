# Introduction

Serverless compute or Function-as-a-Service (FaaS) is still gaining a
lot of traction in the software industry due to the reduced time to
market, lower operational and development costs, and ease of
scalability. The Azure platform already provides Azure Functions and
Azure Logic Apps, two serverless services to build modern, event-based,
reactive systems. Last August Microsoft extended its serverless offering
by introducing Azure Event Grid, a fully managed event routing service,
capable of managing the routing of millions of events per second from
any source to any endpoint, the IFTTT[^1] for the Azure platform.

# Designing modern event-driven systems

Any modern business-critical system must be:

-   **Responsive**: The system responds in a timely manner, which
    ensures usability, utility and consistent behavior of the system.

-   **Resilient**: The system stays responsive when failures occur. This
    can be realized by isolating components of the system so they can
    fail and recover without failing the entire system.

-   **Elastic**: The system stays responsive during varying workloads.
    This can be realized by increasing and decreasing the resources for
    individual components without causing bottlenecks anywhere in the
    system.

These are exactly the features which describe Reactive Systems
(<http://www.reactivemanifesto.org/>). These systems are based on a
message-driven (asynchronous) architecture, so they are loosely-coupled.
This makes them easier to change and they are more tolerant to failure
than non-message-based architectures.

![](./media/image1.png)


Figure Reactive Systems

With Azure Service Bus, Microsoft was already providing a service for
message-driven systems based on either Queues (for one-to-one messaging)
or Topics (one-to-many messaging). Service Bus is a very powerful but
also rather complex messaging solution, particularly suitable for
enterprise solutions that require transactions or duplicate detection.
Let's see what Azure Event Grid has to offer.

# Azure Event Grid

![Event Grid functional model](./media/image2.png)


Figure Azure Event Grid

The new Event Grid is a more lightweight event service that is highly
integrated within the Azure platform, which means that it can be easily
configured to work with Azure Functions, Logic Apps, and many other
Azure services. By using custom topics and webhooks, Event Grid can even
integrate with applications outside Azure, so it is an ideal candidate
for cross-service and cross-cloud scenarios. With Azure Event Grid,
Microsoft now offers the technology for a push-based, message-driven
architecture that allows developers to create reactive systems in a
serverless application landscape.

# Azure Event Grid Concepts

Let's explain Event Grid by means of an example:\
For a given Azure subscription we want to verify the storage accounts
created in that subscription. The verification is just an email that is
sent to an administrator using Logic Apps.

![](./media/image3.png)


Figure Azure Event Grid Example

In this case, Azure Subscriptions is the Event Publisher. The publisher
categorizes events into topics. The topic (*/subscriptions/\<sub-id\>*)
is a system topic since it is provided by Azure. It is also possible to
create custom topics and events that can be raised from a custom
application or function. Note that the maximum size of an event is 64KB.

Every time a resource within the Azure subscription is created
successfully, an *Event* of type
Microsoft.Resources.ResourceWriteSuccess is raised and published to the
topic endpoint.

The event is then picked up by Event Grid and processed on the basis of
an Event Subscription. The Event Subscription defines the routing of
events from a certain topic to an Event Handler. The routing can be
based on the EventType and prefix & suffix filters on the event subject.

In this case we created a subscription called
*StorageAccountCreatedEvents* that points to the topic:

*/subscriptions/\<sub-id\>*

where we only want to handle events of type

*Microsoft.Resources.ResourceWriteSuccess*

with a subject prefix of

*/subscriptions/\<sub-id\>/resourcegroups/
\<rg-id\>/providers/Microsoft.Storage/storageAccounts*

to specify the routing of storageAccount related events only. Note that
these filters do not allow for wildcards or regular expressions, and
unfortunately this will not be supported in the near future.

Finally, the endpoint in the Event Subscription defines the Event
Handler, the target of the event routing. In this case the handler is
the endpoint of the LogicApp:

*https://\<logicappendpoint\>*

Event Grid is based on *at least once delivery* pattern with an
exponential back-off. This means that if the Event Handler does not
respond with a 200 (OK) / 202 (Accepted) response on the request, Event
Grid will retry, using increasingly longer intervals (up to one hour)
and will stop retrying after 24 hours. The message will be dropped if it
can't be delivered. A full description of the Event Grid message
delivery and retry mechanism can be found at:
<https://docs.microsoft.com/en-us/azure/event-grid/delivery-and-retry>.

## Event publishers

At the time of writing this article, Azure Event Grid is in preview and
supports the following Event Publishers: Azure Resource Groups, Azure
Subscriptions, Blob Storage^1^, Event Hubs, and Custom (Event Grid)
Topics.

These are the currently available Event Types that can trigger events.
Please note that by using Custom Topics, events can be triggered from
custom applications that can be hosted anywhere.

+-----------------------+----------------------------------------------+
| Event Publisher       | Event Type                                   |
+=======================+==============================================+
| Azure Resource Groups | Microsoft.Resources.ResourceWriteSuccess     |
| &\                    |                                              |
| Azure Subscriptions   | Microsoft.Resources.ResourceWriteFailure     |
|                       |                                              |
|                       | Microsoft.Resources.ResourceWriteCancel      |
|                       |                                              |
|                       | Microsoft.Resources.ResourceDeleteSuccess    |
|                       |                                              |
|                       | Microsoft.Resources.ResourceDeleteFailure    |
|                       |                                              |
|                       | Microsoft.Resources.ResourceDeleteCancel     |
+-----------------------+----------------------------------------------+
| Blob Storage[^2]      | Microsoft.Storage.BlobCreated                |
|                       |                                              |
|                       | Microsoft.Storage.BlobDeleted                |
+-----------------------+----------------------------------------------+
| Event Hubs            | Microsoft.EventHub.CaptureFileCreated        |
+-----------------------+----------------------------------------------+
| Custom (Event Grid)   | Any custom application event.                |
| Topic                 |                                              |
+-----------------------+----------------------------------------------+

The next updates to Azure Event Grid will extend support for the
following Event Publishers: Azure Automation, Azure Active Directory,
API Management, Logic Apps, IoT Hub, Service Bus, Data Lake Store, and
Cosmos DB.

## Event Handlers

The event handlers that are currently supported are Azure Functions,
Logic Apps, Azure Automation, and Webhooks. Future event handlers will
include Service Bus, Event Hubs, Azure Data Factory, and Storage Queues.
The webhooks Event Handler is versatile because any 3^rd^ party service
that supports webhooks can be used as a target.

# Use case

One of our customers is a banking company and made the strategic
decision to move to Azure. The need for security compliance is very
high, and one of the requirements for using Azure Storage Accounts is
that they have to use encryption.\
We can build a script that will run through all the subscriptions,
filter the resources of the type Microsoft.Storage, and then check
whether they use encryption. This is not extremely difficult, but it is
still a polling mechanism in which all storage accounts are checked
again and again, even if they are not changed. Moreover, the script has
to be scheduled and must run X times per day or hour, with the chance of
lagging behind events.\
Wouldn't it be better to be "notified" as soon as possible when
something regarding a storage account has happened? This is where Azure
Event Grid can help us!\
This new service allows us to react as soon as something has happened
and notify multiple subscribers (event handlers) of specific events.

## Implementation

![](./media/image4.emf)

The following example shows how Azure Event Grid can fire up events to
Azure Functions. The full source code can be found on Github[^3]. There
is also an ARM template available as Gist[^4] that can be used to create
EventGrid subscriptions from a script or a deployment pipeline.

The first piece we need is a function that can parse the event data
provided from the Event Grid. The schema is well documented and can be
found at the following location:
<https://docs.microsoft.com/en-us/azure/event-grid/event-schema#azure-subscriptions>

![](./media/image5.emf)

We will use the subscription events, which means that whenever something
happens in a subscription (every time a resource is created, deleted,
modified, etc.) an event will be published and picked up by Event Grid.

The function that handles the event after parsing the event data will
retrieve a reference to the storage account. The next step consists of
using the Fluent Azure Management libraries for .NET
(<https://github.com/Azure/azure-sdk-for-net/tree/Fluent>) to check
whether the Storage Account is using encryption:

![](./media/image6.emf)

In this example we will output the result to the function logging
console.

### Creating the Azure Event Grid subscription

The easiest way to create an Azure Event Grid subscription
programmatically is to use the Azure CLI 2.0. You can use this directly
from the browser in the Azure Portal using the Azure Cloud Shell.

The following code snippet shows how:

![](./media/image7.emf)

As you can see, we give the subscription a name:
"CheckStorageAccountEncryption", and we tell Event Grid to send the
events to the URL (endpoint) of the function. We also add a filter, and
because we are only interested in successfully created resources, we add
"Microsoft.Resource.ResourceWriteSuccess".

Unfortunately, it is not possible to filter other event properties, e.g.
the Resource Provider (Microsoft.Storage), so we have to do this after
parsing the event in the function.

The "Event Grid Subscription" section of the portal shows the
subscription that was created:

![](./media/image8.png)


### Working example

Let's create an encrypted storage account:

![](./media/image9.emf)

The result is shown in the function's output log:

![](./media/image10.png)

# Conclusion

In an increasingly serverless world, Azure Event Grid is a valuable
addition to create modern applications following an event-driven model.
The integration with the Azure platform makes it an ideal candidate for
Ops automation and microservices scenarios.

![C:\\Users\\MarcDuiker\\AppData\\Local\\Microsoft\\Windows\\INetCache\\Content.Word\\ops_automation.png](./media/image11.png)


Figure 4 Ops Automation

However, it is certainly not limited to Ops automation, thanks to custom
topics and webhook handler capability. These make it very suitable for
many serverless integration scenarios, even with third party services.

![C:\\Users\\MarcDuiker\\AppData\\Local\\Microsoft\\Windows\\INetCache\\Content.Word\\serverless_web_app.png](./media/image12.png)


Figure 5 Serverless applications

At the time of writing this article, the service is in preview. In the
future, Microsoft will add more event publishers and handlers to make
the native integration in Azure and third parties even better, and to
ensure that integration between different domains will become even
easier and more reliable. Monitoring will also be added to allow
inspection of ingress of events, event matches, pushes to event
handlers, and dropped events.\
Event Grid allows developers to focus on business processes when they
"just happen", using fewer computing resources and without any worries
about infrastructure.

[^1]: If This Then That - <https://ifttt.com/>

[^2]: The Blob Storage Event Publisher is currently in private preview.

[^3]: Source code : <http://xpir.it/xprt5-eventgrid>

[^4]: ARM template: <http://xpir.it/xprt5-eventgrid-arm>
