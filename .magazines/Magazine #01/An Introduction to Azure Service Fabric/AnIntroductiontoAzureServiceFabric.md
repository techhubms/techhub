[Building hyper-scale distributed applications can be very complex.
Developers need to take]{.smallcaps} [asynchronous communication,
concurrency, latency, redundancy]{.smallcaps} [into
consideration]{.smallcaps} [and many more aspects that are necessary to
make distributed applications successful. These aspects are necessary,
but they aren't directly related to the business domain the applications
relate to. Microsoft Azure Service Fabric helps to simplify these
aspects and have developers focus on the business domain at
hand.]{.smallcaps}

# [Next Generation PaaS]{.smallcaps}

Microsoft started their Cloud Computing platform as a
Platform-as-a-Service (PaaS) offering with Web- and Worker Roles. Web-
and Worker Roles basically are templates for virtual machines that are
managed entirely by Microsoft. They hold your applications code and they
define the characteristics of the virtual machines that code is deployed
to. Although you can deploy more than one applications onto a single
virtual machine (or role instance), updating any of the applications can
take up to 15 minutes and thus typically a role instance (or virtual
machine) contains only a single application.

Web- and Worker Roles are completely stateless due to the fact that the
actual disks are located on the physical machine where the role
instances are hosted. To account for failure of a physical machine or
software errors, Microsoft recycles our role instances. This means that
the disks will be replaced with new disks and that our application code
will be deployed on those new disks. Any state that was added after
initial deployment will be lost.

To make sure that our state is being saved, we need to store it in a
durable remote data store. Examples of durable remote data stores are
Azure SQL Database, Azure Table- or Blob Storage, Azure DocumentDb,
Azure Search, etc. Although these data store serve their purpose very
well, you still need to think of latency, concurrency, consistency and
implementing retry policies. These are all things that complicate your
overall solution without adding any business value. In addition, using
these platform services makes it harder to build cross-premises or
cross-cloud solutions.

Azure Service Fabric provides a layer on top of the operating system of
the virtual machine hiding details like the number of cores, the amount
of RAM etc. This layer, or *fabric* as it's called, is created over a
cluster of nodes (or virtual machines) and it manages the provisioning,
redundancy, communication and all the complexity you don't want to deal
with.

Azure Service Fabric also provides a quorum-based replication mechanism
that allows you to store data where your application code runs. This
greatly reduces latency, while preserving state on multiple machines,
potentially in multiple data centers. And the great thing is that,
because Azure Service Fabric is implemented as a layer on top the
operating system, it can run anywhere. As long as nodes can find each
other at IP level they can be joined to the cluster.

![](./media/image1.png)


Figure : Reduced Complexity

# Getting Started With Azure Service Fabric

The first thing you will need to do is setup your development
environment. To get started first install *Microsoft Azure Service
Fabric SDK -- Preview 1* using the Web Platform Installer. For this you
need to have Visual Studio 2015 RC installed.

![](./media/image2.png)


Figure : Web Platform Installer

This will install the *Microsoft Service Fabric Host Service*, the
*Service Fabric SDK*, the *Microsoft.ServiceFabric.Powershell* module
and the *Microsoft Azure* *Service Fabric Tools for Visual Studio 2015*.

Next you'll need to set up a development cluster to test your code on. A
Service Fabric development cluster is very different from the Compute
Emulator we all know from Web- and Worker Roles. A Service Fabric
development cluster is exactly the same technology that runs in your
production environment ensuring that the code that runs in your machine
will run exactly the same way in your production environment. The only
difference is that the cluster nodes in a development cluster are
running on a single machine whereas in production each node runs on a
separate machine. These machines can either be physical or virtual,
on-premises, in your ISPs datacenter, in Microsoft's datacenter (Azure)
or in any Cloud environment for that matter, that runs Windows Server
2012 R2 and up.

Creating a development cluster is done in four simple steps:

1.  Open up a PowerShell window as Administrator

2.  Execute Set-ExecutionPolicy --ExecutionPolicy Unrestricted -Force
    -Scope CurrentUser

3.  Execute cd "\$env:ProgramFiles\\Microsoft SDKs\\Service
    Fabric\\ClusterSetup"

4.  Execute .\\DevClusterSetup.ps1

This will start the *Microsoft Service Fabric Host Service* and it will
setup a 5 node cluster on your machine. By default your cluster files
(logs and data) will live in c:\\SfDevCluster.

Finally, you can verify your cluster by starting the *Service Fabric
Explorer* located in the Tools folder in the Service Fabric SDK
installation directory (C:\\Program Files\\Microsoft SDKs\\Service
Fabric).

![](./media/image3.png)

Figure : Service Fabric Explorer

# Building Microservices

Microsoft Azure Service Fabric is based on a microservices principles.
Applications that run on Azure Service Fabric consist of small
autonomous services that interact with each other in a loosely coupled
manner. These services can be deployed together as one application, but
they can be upgraded separately without causing downtime to the system.

## Reliable Services API

To get started you'll need to create a new project in Visual Studio
using one of the templates in *Visual C# -\> Cloud -\> Service Fabric*.
There are four templates, two for building on top of the *Service Model
API* and two for building on top of the *Actor Model API* (which will be
discussed later). The two templates for building on top of the *Service
Model API* are:

1.  Application with Stateless Service

2.  Application with Stateful Service

![](./media/image4.png)


Figure : Visual Studio Project Templates for Azure Service Fabric

![](./media/image5.png)
As the name suggests the templates consist
of one project for the application and one project for a service. You
can look at the application as a configuration container for a number of
services. Configuration is done through an ApplicationManifest.xml file.
In there you can specify endpoint names that are used for services, the
minimum number of replicas that should be acknowledged before returning
and the target amount of replicas that should exist for a particular
service and the required partitioning strategy, etc.

The other project is the actual service. From a microservices
perspective this should be a small autonomous service with a single
responsibility. Configuration and definition of the service is done
through a ServiceManifest.xml file, which is located in a PackageRoot
folder.

There are two types of services, stateless and stateful.

Stateless services do not contain state on the nodes that they're hosted
on. They can have state in external data stores like Azure SQL Database,
Azure DocumentDb (or any other storage service available), but their
state will not be stored on the actual node.

Stateful services, on the other hand, store their state on the actual
node where they're being hosted on. This reduces network latency while
retrieving the data. Partitioning, allocation and replication are all
managed by Service Fabric based on the configuration provided in both
the ApplicationManifest.xml and ServiceManifest.xml files. State will be
stored on the actual node where your service is hosted, thus reducing
network latency.

Once you created an application, additional services can be added to the
application by right-clicking the applications project in the Solution
Explorer. This way you can mix all types of services (Stateless,
Stateful, Stateless Actor and Stateful Actor) in the same application.
The AplicationManifest.xml file will automatically be updated to include
the new services and you can manually alter them to override
configuration settings made in the ServiceManifest.xml file.

## Reliable Services

Writing a service can start out simple, but you still have the option to
change the inner workings of a service.

A Service Project basically is a Console Application that compiles to an
EXE. In the Main method a FabricRuntime is created and a service type is
registered. Multiple service types can be registered to the
FabricRuntime, but that would mean these service types can only be
upgraded together and not separately. We'll discuss this later when we
talk about the Actor Model API.

![](./media/image6.png)


Snippet : Registering Service Types to the FabricRuntime

A second class in the project represents the actual service. The service
class inherits either from StatelessService or StatefulService and it
requires you to implement just two methods:

1.  CreateCommunicationListener

2.  RunAsync

![](./media/image7.png)

Snippet : Two overrides of a Service

CreateCommunicationListener needs to return an implementation of
ICommunicationListener, which deals with opening and closing a
communication channel and listening on it. This can be a communication
channel of your choosing (e.g. WebSockets, HTTP, Azure Service Bus,
ZeroMQ, etc.). This communication channel is used to receive messages
from outside the service.

RunAsync is where all the work is done. If you're familiar with how
Worker Roles work, this is very much the same as the Run method in a
Worker Role. It defines the lifecycle of the service. If the RunAsync
method returns the service will be recycled. So, just as with Worker
Roles, it contains an endless loop that will execute our code.

## Reliable Data

If you chose to create a Stateful Service your service class will
contain a *StateManager* property. This property can be used to get
reliable data collections, queues and transactions. This way you can
read from and write to a collection as you are used to and you don't
have to worry about concurrency, redundancy, availability or scale.

![](./media/image8.png)

Snippet : Using Reliable Collections

# A Programming Model Built for Microservices

So microservices are small autonomous services with a single
responsibility. Now that looks a lot like *Separation of Concerns* in
object-oriented programming. Basically each class would be a
microservice on itself if it was independent from the process it ran in.
Project Orleans was developed to create a programming model that
implements the mathematical Actor model for concurrent computations.
This programming model was used to accommodate the hundreds of thousands
of game sessions and play records of the computer game HALO 4.

The *Reliable Actors API* in Azure Service Fabric is a programming model
based on Orleans built on top of the Reliable Services API.

## A Short Note On Virtual Actors

Before we continue, here's a short note on what virtual Actors are.
Virtual Actors are isolated single-threaded components that encapsulate
both state and behavior. Actors interact with the system, including
other Actors, by sending asynchronous messages in a request-response
pattern. Virtual Actors are always there. If they are not, they will be
created on the fly and if they were created in the past they will exist
for ever. So the developer is not concerned about the lifecycle of the
actual object.

![](./media/image9.png)


Snippet : Creating an Actor

## Reliable Actors API

Up until now we haven't discussed the other two project templates,
*Application with Stateless Actor Service* and *Application with
Stateful Actor Service*. The structure of the application is pretty much
the same in that is still is a configuration container for a collection
of services. The Actor Service templates both create two projects in
addition to the application project:

1.  A project for the actual service

2.  A project that contains the interfaces for the Actors

The actual service host is exactly the same as with the Reliable
Services API. The only difference is that we need to register every
Actor to the FabricRuntime. Every Actor will become a Service in Azure
Service Fabric. And because the Actor Service by default is configured
to have nine partitions, activations of the Actors will be distributed
over the nodes in the cluster automatically.

![](./media/image10.png)


Snippet : Registering Actors to the FabricRuntime

Every Actor starts out as an interface defining its behavior. This
interface should implement the IActor interface. The IActor interface is
just a marker interface for the ActorProxy to validate that this will
indeed return an Actor.

![](./media/image11.png)


Snippet : Implementing IActor

Next, we need to create a class in the actual Service project that
inherits from Actor and implements the previously created interface. If
it involves a Stateful Actor Service we'll need to inherit from a
generic Actor of the type of the state it will contain. The state needs
to be a class that needs to be data contract serializable.

![](./media/image12.png)


Snippet : Actor State object

![](./media/image13.png)


Snippet : An Example of a Game Actor

Snippet 8 shows that Stateful Actors have a *State* property where state
can be stored. Persisting the state of an Actor is completely
transparent and you can only specify whether to store the data on disk
or in-memory (by using the VolatileActorStateProviderAttribute above the
Actor class).

If an Actor is idle for more than sixty minutes it will be deactivated
and ultimately it will be garbage collected. If you need to trigger a
specific method on an Actor periodically, you can register so-called
Reminders. Reminders are registered with and controlled by the
FabricRuntime and thus they will always fire, even when the Actor is
deactivated or even garbage collected.

Just like Grains in Orleans, Azure Service Fabric Actors can raise
events to which other actors or client services can subscribe.

# Conclusion

Microsoft Azure Service Fabric offers developers a virtual layer, on top
of the operating system, that spans multiple machines. This layer helps
orchestrate our applications keeping them healthy even when the physical
machine is failing. Because Azure Service Fabric takes a Microservices
approach, services that make up an application can be upgraded
independently from each other without downtime and within seconds. And
because Azure Service Fabric is a layer on top of the operating system
it can run anywhere. It hides all the complexity that comes with
developing highly available, high performance distributed systems and
lets developers focus on the business domain at hand.
