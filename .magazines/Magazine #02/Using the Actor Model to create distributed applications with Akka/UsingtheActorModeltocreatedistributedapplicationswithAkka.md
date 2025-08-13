## Using the Actor Model to create distributed applications with Akka.NET

## Pascal Naber

## ![C:\\Users\\Pascal\\Downloads\\Foto\'s akka artikel\\akka.jpg](./media/image1.jpeg)Introduction

Akka.NET is an Actor Model framework for building highly concurrent,
distributed, fault-tolerant and event-driven applications on .NET. This
article explains what Akka.NET is, and when and how to use it. It also
describes how to get started with Akka.NET, using a simple example to
guide you through the possibilities.

## Why concurrent systems

Just imagine you are ordering a menu at a fast-food restaurant; the
employee would need to carry out the following steps:

![](./media/image2.png)

Figure 1. Steps to carry out by an employee

\*In the Netherlands we actually order mayonnaise with our menu, see
also Pulp Fiction on YouTube: <http://tinyurl.com/qjzk7g6> 1:30)

The employee will carry out all tasks sequentially. However, if there is
only one employee, he cannot tap the milkshake at the same time as when
he is frying a hamburger, and this will result in extra waiting time for
the customer. But the employee doesn't have to do all these things
himself in the fast-food restaurant. Usually, multiple employees are
performing tasks at the same time to give you your menu as quickly as
possible. Now imagine, the fast-food restaurant is a CPU and the
employee is a thread. You do not want all tasks running on one thread.

But running programs in multiple threads comes with a price. There are
challenges to overcome. One of the hardest things to handle in a
multi-threaded world is the mutable state. In the example, six tasks are
changing data at the same time. In order to handle this, you will need
to use read-and-write locks to prevent multiple threads from mutating
the data at the same time. A lot of code is required to handle this and
still it's hard to get it right, to make it perform properly and
bug-free. Consider deadlocks for example.

The Actor Model is an approach that hides these hard parts for you.

## Actor Model 

The Actor Model uses Actors to create concurrent applications. When you
apply the Actor Model you will not be bothered with complex programming
concepts like concurrency and parallel programming. The Actor Model
hides threads and locking for you, allowing you to focus on business
logic. The business logic can be written in light-weight classes --
called Actors -- that are only responsible for a single task.

A popular framework that implements this model is called Akka.NET. The
Actor Model that Akka.NET offers is called The ActorSystem.

## ActorSystem

+-----------------------------------------------------------------------+
| ![](./media/image3.png)**Console applications**                       |
|                                                                       |
| In this example we use Console applications to host our ActorSystem.  |
| For production use we recommend Topshelf -- Topshelf is a framework   |
| to create a simple console application that can be installed as a     |
| Windows service.                                                      |
+=======================================================================+
+-----------------------------------------------------------------------+

The class in Akka.NET that represents the Actor Model is called
ActorSystem. All Actors are created by an ActorSystem and live within
the context of that ActorSystem. Let's see this ActorSystem in action.

We will create a very simple application that shows you how to use
Akka.NET. If you follow the steps in the article you will end up with a
simplified, distributed Fastfood restaurant application. This first
version will be a Console application using Akka.NET that introduces you
to the ActorSystem, Actors and messaging. Later on this will be changed
to a distributed application.

Open Visual Studio, create a Console Application called Client, and
create a class library called Shared. For now the Shared project is
optional, but it is required as a preparation for later steps in this
article. Add a reference from Client to Shared. Add the latest version
of Nuget package Akka to both projects. Open Program.cs in Client and
type:

The ActorSystem always has a name and implements IDisposable, which is
the reason the using statement is used. You are now ready to create an
Actor. Actors may sound like a new term, but actually it's not new at
all. The term Actor has been around for a long time.

## History

+-----------------------------------------------------------------------+
| **Origins of the name Akka**                                          |
|                                                                       |
| ![http://org.nljug.s3.amazonaws.com/arti                              |
| cles%2FAkka_logo_transparent.png](./media/image4.png) |
| Akka (pronounced: Áhkká)                |
|                                                                       |
| The name comes from the goddess in the Sami (native Swedish)          |
| mythology who symbolized wisdom and beauty in the world.              |
|                                                                       |
| It is also the name of a beautiful mountain in Laponia in the north   |
| of Sweden                                                             |
+=======================================================================+
+-----------------------------------------------------------------------+

Back in 1973, Carl Hewitt wrote a paper in which the concept of Actors
was introduced. The Actor Model was implemented in the Erlang
programming language and runtime system in 1987. Erlang offered high
reliability and the use of many processors without the need to have
explicit code. Jonas Bonér created Akka in 2009 to bring the
capabilities of Erlang to Scala and Java. Since then, Akka has become
the defacto standard for building distributed solutions with the
functional programming language Scala.

Akka.NET was created by Roger Johansson and Aaron Stannard and is a port
of Akka for .NET projects on Windows and Mono supporting both C# and F#.
Both Akka and Akka.NET are open-source. Version 1 of Akka.NET was
released in April 2015.

Akka.NET is not the only Actor Model framework for .NET. Microsoft
Research designed and created Orleans. The first version was released in
February 2015. This project became known through the use of the cloud
services for Halo 4.

Later this year Microsoft will release Azure Service Fabric; the preview
can currently be downloaded. One of the programming models is also an
Actor Model.

## Actor

![](./media/image5.png)
All objects in the ActorSystem are Actors.
An Actor contains behavior and state. The Actors can only communicate to
other Actors using messages. Actors run independently of other actors.
After receiving a message, an Actor can execute a piece of code. Like
business logic, it can call a database, write to a file or change its
state. In short: anything you like. Besides this, Actors can create
other Actors. An Actor is single-threaded and handles one message at a
time.

Actors do not have a unique thread of their own. If this were the case,
you would run out of resources very soon. A thread can run many Actors.
When a message is sent to an Actor, the message is stored in a queue.
This queue is called the Mailbox. The Dispatcher which handles the
messages will notice that there is work for an Actor because the mailbox
is full. This Dispatcher starts a thread, brings the Actor to life on
this thread, and then delivers the message to it.

![http://a5.files.biography.com/image/upload/c_fit,cs_srgb,dpr_1.0,h_1200,q_80,w_1200/MTE5NDg0MDU0OTM2NTg1NzQz.jpg](./media/image6.jpeg)
Perhaps you are thinking about your own
domain now and trying to visualize it in Actors and coming up with a
couple or even dozens of Actors. In practice Akka.NET can handle
millions of Actor instances! This changes the game of developing
concurrent software and is one of the reasons why Akka is very suitable
for the Internet of Things (IoT). IoT is introducing a new order of
complexity for back-end systems because of the number of things, which
Akka can handle by having an Actor for every "thing".

Let's define an Actor in project Shared, which can receive two types of
messages. A BurgerMenuRequest and a SaladRequest. The requests, both
messages, are just classes without logic or state.

There are multiple ways to define an Actor: by means of the
UntypedActor, the ReceiveActor or the TypedActor. Each will be explained
in the following paragraphs:

### UntypedActor

When you extend the UntypedActor it needs to implement the OnReceive
method which takes an untyped message. This is convenient when you want
to receive any message. This version makes it possible to use pattern
matching on messages which can decide to process the message or not.

### ReceiveActor

The ReceiveActor requires you to register messages in the constructor.
The message is handled in two ways in the codesnippet: both inline and
as a method.

### 

### TypedActor

When you use the TypedActor, you also need to implement the IHandle\<\>
interface for each message that you want to react to. If the logic in
the Actor can handle typed messages, this is the most explicit way to
implement your Actor. Therefore, create the Employee class as follows in
project Shared:

Let's create another Actor, which you will need in a second:

For the example in this article only those two Actors are needed. If you
wish, you can create Actors for all actions needed for a menu.

## Creation of Actors

The example of the menu involves two Actors: Customer and Employee. How
do you create an instance of an Actor? Not as usual by calling the
constructor. When you do this, an ActorInitializationException will be
triggered, telling you to use the ActorSystem or that another component
should be used to create Actors. So let's listen to that advice and let
the ActorSystem create an instance of the Customer Actor.

The ActorOf method requires a Props and optionally a name. The Props
instance configures which Actor to create. The name should be unique
within the ActorSystem. You don't get a reference to the Customer
instance, the result is an IActorRef. The instance implementing the
IActorRef interface is an ActorRefImpl. Actors can only communicate with
messages, so we don't need a reference. The IActorRef is a proxy to the
actual instance. This instance might live within the current process or
may be in a process on another computer. Akka.NET hides this for you and
this is called [location transparency]{.underline}.

## Communicate with Actors

First the customer has to know that he is hungry to kick off the
process. So let's communicate this through messaging:

The Tell method does not send the message directly to the OnReceive
method or Handle method of the Actor. Instead the message will be sent
through a fire-and-forget mechanism to the Mailbox of the Actor. You
will not receive a response. The message will be handled asynchronously
from this thread.

By default, a message in Akka.NET is delivered At-Most-Once. This means
there is no guaranteed delivery. Akka.Persistence is a module that
offers At-Least-Once delivery semantics. However, this is not described
in further detail as it is beyond the scope of this article.

The Customer Actor can handle the message with the following code:

The ActorSystem is not the only class that can create Actors. An Actor
can create other Actors as well. This way a whole hierarchy of Actors
can be created. This hierarchy is described in more detail later in this
article. Every type of Actor has a Context property which can be used
for this. Now that we have a proxy to the CustomerActor, we will send a
BurgerMenuRequest Message. How should the Customer be notified about the
finished menu when request-response messaging cannot be used? It is
possible to send a message back to the caller, also known as the parent.
Of course this also happens completely asynchronously using messages.

The Customer will handle the Bill Message as follows:

Until now all code handles the Happy flow. But what if an exception is
raised?

## ![Actors must be supervised!](./media/image7.png)Fault Handling

The way Akka.NET handles exceptions, results in isolation of faults by
handling the exception locally. This way the rest of the system is not
bothered and continues running. Separation of concerns is applied within
an Actor regarding business logic and handling exceptions. Both business
logic and fault recovery logic are two different flows within an Actor.
The recovery-logic monitors child Actors. An Actor that monitors child
Actors is called a Supervisor. An Actor doesn't try to solve its
exceptions; it will just crash, also known as the [Let It
Crash]{.underline} semantic. The Supervisor decides how to handle the
exception coming from a monitored child Actor and can choose one of the
following Supervision directives:

  -----------------------------------------------------------------------
  Restart       The Actor will be recreated and will process messages,
  (default)     resetting internal state.
  ------------- ---------------------------------------------------------
  Resume        The Actor will continue processing messages, keeping
                internal state.

  Stop          The Actor will be terminated.

  Escalate      The Supervisor doesn't know how to handle the exception
                and escalates it to its parent, also a supervisor.
  -----------------------------------------------------------------------

Because the parent actor handles the lifetime of a child Actor, you
should not mix your business logic code with other risky code such as
calling a webservice. The call to the webservice should be handled by a
separate Actor. When the webservice is not available, the parent will
decide how the Actor should react to the exception.

Because the Supervisor can receive multiple types of exception, a
Supervision strategy can handle multiple exceptions and can decide what
directive to apply to the failed Actor. There are two built-in
Supervision Strategies and it's also possible to create a customized
strategy.

  ------------------------------------------------------------------------
  OneForOneStrategy   Stop the child actor that failed.
  ------------------- ----------------------------------------------------
  AllForOneStrategy   Stop all child actors of the Supervisor.

  ------------------------------------------------------------------------

The following code snippet shows how to program a SupervisorStrategy.
Just override the SupervisorStrategy method on an Actor and return a
AllForOneStrategy or OneForOneStrategy. In this case, when an
ApplicationException is thrown from a child actor, the child is
restarted. Any other exception will escalate the Exception to a parent
Actor.

+-----------------------------------------------------------------------+
| public class Customer : UntypedActor                                  |
|                                                                       |
| {                                                                     |
|                                                                       |
|     protected override SupervisorStrategy SupervisorStrategy()        |
|                                                                       |
|     {                                                                 |
|                                                                       |
|                                                                       |
|       return new OneForOneStrategy(3, TimeSpan.FromSeconds(5), ex =\> |
|                                                                       |
|         {                                                             |
|                                                                       |
|             if (ex is ApplicationException)                           |
|                                                                       |
|             {                                                         |
|                                                                       |
|                 return Directive.Restart;                             |
|                                                                       |
|             }                                                         |
|                                                                       |
|                                                                       |
|                                                                       |
|             return Directive.Escalate;                                |
|                                                                       |
|         });                                                           |
|                                                                       |
|     }                                                                 |
+=======================================================================+
+-----------------------------------------------------------------------+

In this example, when an ApplicationException occurs, the Actor who
raised the Exception is restarted. Any other exception will escalate the
Exception to the parent of this Actor. If an ApplicationException occurs
3 times within 5 seconds, the Actor will be stopped.

When the Actor is restarted it will continue processing other messages
in the mailbox. This means that by default, the message which was
responsible for the exception will have been removed. The simplest
mechanism to keep processing the offending message is to use the
PreRestart to send the message to the Actor itself.

+-----------------------------------------------------------------------+
| public class Employe                                                  |
| e : TypedActor, IHandle\<BurgerMenuRequest\>, IHandle\<SaladRequest\> |
|                                                                       |
| {                                                                     |
|                                                                       |
|                                                                       |
|  protected override void PreRestart(Exception reason, object message) |
|                                                                       |
|     {                                                                 |
|                                                                       |
|         // Put the message that failed, back in the mailbox.          |
|                                                                       |
|         // It will be picked up when the actor is restarted.          |
|                                                                       |
|         Self.Tell(message);                                           |
|                                                                       |
|     }                                                                 |
+=======================================================================+
+-----------------------------------------------------------------------+

This solution keeps trying to process the same message, which can be a
good thing for transient faults such as web-service connection-faults.
But if the exception is not transient, the message will be processed
forever. For this reason, you have to come up with a solution for
production systems. One possibility is to apply a Circuit breaker
pattern like Polly.NET.

Fault handling in Akka.NET can be done with SupervisorStrategies, but
what happens when the first-created Actor, a top level Actor, throws an
exception? It has no parent Actor that can handle the exception. Well
actually it does.

## Actor Hierarchies

Because of the parent-child setup for Actors, all Actors in a system
represent a hierarchy of Actors. Akka.NET contains some out-of-the-box
Actors that are available when you create an ActorSystem.

![](./media/image8.png)


The /user Actor also referred as Guardian Actor or Root Actor is the
Supervisor for the top level actors. This Guardian will handle
exceptions by default with a restart directive.

All Actors have a unique address. An Actor can be placed anywhere in the
hierarchy. In this example, the Employee is placed below the customer.
An Actor can send a message based on the address to another Actor. To
send a message, both an absolute and a relative address can be used.

+-----------------------------------------------------------------------+
| // absolute                                                           |
|                                                                       |
| var selection = Context.ActorSelection(\"/user/customer\");           |
|                                                                       |
| selection.Tell(new Bill());                                           |
|                                                                       |
| // relative                                                           |
|                                                                       |
| var selection = Context.ActorSelection(\"../customer\");              |
|                                                                       |
| selection.Tell(new Bill());                                           |
+=======================================================================+
+-----------------------------------------------------------------------+

The unique address of an Actor has the following structure:
![](./media/image9.png)


## ![](./media/image10.png)Location Transparency

Until now the Actors in the example run on the same machine. When the
application has reached the hardware capacity limit on the machine, you
can apply a scale-out scenario. That's actually quite easy because Akka
is distributed by design. Actors can run on any machine without a code
change, and for the application it doesn't matter where the Actor runs.
This is also called Location Transparency. Remoting is the feature in
Akka.NET that offers location transparency, and it can be configured
with HOCON.

HOCON is the abbreviation for Human-Optimized Config Object Notation.
This is the configuration format that Akka is using. This configuration
can be read from a separate file or the configuration can be embedded in
the app.config or web.config inside a CData section.

To host a remote ActorSystem you first need to create a new Console
application called Server. Add a reference from Server to Shared, and
add Nuget package Akka.Remote to the Client and Server project in the
solution.

There are two possibilities for running Actors remotely: Remote
Deployment of an Actor, or send a message to a remotely running Actor.

### Remote Deployment of an Actor 

![C:\\Users\\Pascal\\Dropbox\\Xpirit\\Community\\2016-01 Magazine\\Using
the Actor Model to create distributed applications with
Akka.NET\\RemoteDeployment.jpg](./media/image11.jpeg)
To run an Actor on a different node
(Server) it's possible to deploy it with Akka.Remote.

First we create the server. The code is the same as the code we created
for the client; only the name of the ActorSystem differs. This name is
important for the configuration of the client, which will follow.

+-----------------------------------------------------------------------+
| // server                                                             |
|                                                                       |
| using (var actorSystem = ActorSystem.Create(\"remotefastfood\"))      |
|                                                                       |
| {                                                                     |
|                                                                       |
|     Console.ReadLine();                                               |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

Notice you don't initialize any Actor on the Server.

The server needs an app.config file that looks like this:

+-----------------------------------------------------------------------+
| \<?xml version=\"1.0\" encoding=\"utf-8\" ?\>                         |
|                                                                       |
| \<configuration\>                                                     |
|                                                                       |
|   \<configSections\>                                                  |
|                                                                       |
|     \<section name=\"akka\"                                           |
|  type=\"Akka.Configuration.Hocon.AkkaConfigurationSection, Akka\" /\> |
|                                                                       |
|   \</configSections\>                                                 |
|                                                                       |
|   \<akka\>                                                            |
|                                                                       |
|     \<hocon\>                                                         |
|                                                                       |
|       \<\![CDATA\[                                                    |
|                                                                       |
| akka {                                                                |
|                                                                       |
|                                                                       |
|  actor.provider = \"Akka.Remote.RemoteActorRefProvider, Akka.Remote\" |
|                                                                       |
|                     remote {                                          |
|                                                                       |
|                         helios.tcp {                                  |
|                                                                       |
|                     port = 8090                                       |
|                                                                       |
|                     hostname = localhost                              |
|                                                                       |
|                         }                                             |
|                                                                       |
|                     }                                                 |
|                                                                       |
|                 }                                                     |
|                                                                       |
| \]\]\>                                                                |
|                                                                       |
|     \</hocon\>                                                        |
|                                                                       |
|   \</akka\>                                                           |
|                                                                       |
| \</configuration\>                                                    |
+=======================================================================+
+-----------------------------------------------------------------------+

The remote part tells the ActorSystem on which host and port number it
is running. This is also important for the configuration of the client.
The code of the client does not have to change. An Actor is deployed to
a remote ActorSystem by changing the configuration in HOCON. The
app.config of the client is configured like this:

+-----------------------------------------------------------------------+
| \<?xml version=\"1.0\" encoding=\"utf-8\" ?\>                         |
|                                                                       |
| \<configuration\>                                                     |
|                                                                       |
|   \<configSections\>                                                  |
|                                                                       |
|     \<section name=\"akka\"                                           |
|                                                                       |
| type=\"Akka.Configuration.Hocon.AkkaConfigurationSection, Akka\" /\>  |
|                                                                       |
|   \</configSections\>                                                 |
|                                                                       |
|   \<akka\>                                                            |
|                                                                       |
|     \<hocon\>                                                         |
|                                                                       |
|       \<\![CDATA\[                                                    |
|                                                                       |
| akka {                                                                |
|                                                                       |
|                     actor{                                            |
|                                                                       |
|                                                                       |
|        provider = \"Akka.Remote.RemoteActorRefProvider, Akka.Remote\" |
|                                                                       |
|                         deployment {                                  |
|                                                                       |
|                             /employeeActor/employee {                 |
|                                                                       |
|                                                                       |
|                 remote = \"akka.tcp://remotefastfood@localhost:8090\" |
|                                                                       |
|                             }                                         |
|                                                                       |
|                                                                       |
|                                                                       |
|                         }                                             |
|                                                                       |
|                     }                                                 |
|                                                                       |
|                     remote {                                          |
|                                                                       |
|                         helios.tcp {                                  |
|                                                                       |
|                     port = 0                                          |
|                                                                       |
|                     hostname = localhost                              |
|                                                                       |
|                         }                                             |
|                                                                       |
|                     }                                                 |
|                                                                       |
|                 }                                                     |
|                                                                       |
| \]\]\>                                                                |
|                                                                       |
|     \</hocon\>                                                        |
|                                                                       |
|   \</akka\>                                                           |
|                                                                       |
| \</configuration\>                                                    |
+=======================================================================+
+-----------------------------------------------------------------------+

There are two parts in the HOCON configuration: actor and remote. Remote
configures the ActorSystem on this node, the client in this case, where
it is hosted. When port 0 is configured, Akka chooses a port. The actor
part of the configuration configures the RemoteActorRefProvider of
Akka.Remote and allows the employee Actor to run on the address of the
configured server. Also the name of the ActorSystem of the Server is
configured in the address.

This way, when the Actor is created, it's created on the Remote
ActorSystem. This is done without a code change; all actions are
performed in configuration.

In this example the two host projects are called Client and Server. Akka
Remote uses peer-to-peer communication between nodes, so actually every
Node is both a Client and a Server.

### Send a message to a remotely running Actor

![](./media/image12.png)
An alternative to creating an Actor on a
remote ActorSystem is to host multiple ActorSystems (Client and Server)
and send a message from the client to an Actor on the Server. This means
that the Actors have already been deployed. To do this, first create the
remotely running Actor. Create an ActorSystem like the one the Server
created earlier, but with one line of code added to start the Actor to
which the client will send a message.

+-----------------------------------------------------------------------+
| // server                                                             |
|                                                                       |
| using (var actorSystem = ActorSystem.Create(\"remotefastfood\"))      |
|                                                                       |
| {                                                                     |
|                                                                       |
|     actorSystem.ActorOf\<Employee\>(\"employee\");                    |
|                                                                       |
|     Console.ReadLine();                                               |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

The configuration of the Server stays the same as above.

The Client configuration needs a small change, but it is not necessary
to remotely deploy an Actor anymore. For this reason, the deployment
part is removed.

+-----------------------------------------------------------------------+
| \<\![CDATA\[                                                          |
|                                                                       |
| akka {                                                                |
|                                                                       |
|               actor{                                                  |
|                                                                       |
|                   provider = \"Ak                                     |
| ka.Remote.RemoteActorRefProvider, Akka.Remote\"                       |
|                                                                       |
|               }                                                       |
|                                                                       |
|               remote {                                                |
|                                                                       |
|                   helios.tcp {                                        |
|                                                                       |
|               port = 0                                                |
|                                                                       |
|               hostname = localhost                                    |
|                                                                       |
|                   }                                                   |
|                                                                       |
|               }                                                       |
|                                                                       |
|           }                                                           |
|                                                                       |
| \]\]\>                                                                |
+=======================================================================+
+-----------------------------------------------------------------------+

The code of Customer in Shared should be changed slightly. It is not
necessary to create an Actor with Context.ActorOf. Instead, select an
Actor based on an address with Context.ActorSelection. After getting the
proxy, you can send the message in the normal way. Because the
EmployeeActor is the Top-Level-Actor in this ActorSystem, the address
does not contain the customer.

+-----------------------------------------------------------------------+
| if (message is MarkHungry)                                            |
|                                                                       |
| {                                                                     |
|                                                                       |
|     Console.WriteLine(\"Customer is hungy let\'s order some food\");  |
|                                                                       |
|     // CREATE (remote deployment)                                     |
|                                                                       |
|                                                                       |
|    // IActorRef employee = Context.ActorOf\<Employee\>(\"employee\"); |
|                                                                       |
|     // GET                                                            |
|                                                                       |
|     var employee =                                                    |
|                                                                       |
| Context.ActorS                                                        |
| election(\"akka.tcp://remotefastfood@localhost:8090/user/employee\"); |
|                                                                       |
|                                                                       |
|                                                                       |
|    fastFoodEmployee.Tell(new BurgerMenuRequest());                    |
|                                                                       |
|    Console.WriteLine(\"Ordered food\");                               |
|                                                                       |
|    return;                                                            |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

The message BurgerMenuRequest is now sent to the Remote Actor.

It's easy to use Akka.Remote, but the number of nodes is fixed and
should be known in advance. A more advanced step is to apply
Akka.Cluster.

## Cluster

![](./media/image13.png)
A cluster is a dynamic group of nodes.
Just as we saw with Akka.Remote, every node represents an ActorSystem.
Akka.Cluster makes it possible to create truly elastic applications by
dynamically growing and shrinking the number of nodes. Cluster ensures
that Actors run in a location-transparent manner; an Actor can run on
any node. Akka.Cluster makes Akka.NET highly available, fault-tolerant
and ensures that there is no single point of failure for your
application. Because of all these characteristics, Akka.Cluster makes it
really interesting to create Microservices with Akka.NET.

To create a Cluster, you need to create nodes. It's possible to
configure nodes as a Seed-Node or as a Non-Seed-Node. Seed nodes ensure
that Non-Seed-Nodes can join the cluster. For this reason, Seed-nodes
have a known address. The addresses of Seed-Nodes are configured at a
Non-Seed-Node's configuration. Because Seed-Nodes may also fail, you
should have at least two Seed-Nodes in the cluster. Akka uses
peer-to-peer communication between nodes. This means that a node is
aware of all other nodes in the cluster, and communicates with them.

Akka.Cluster is not released yet. Fortunately, there is a prerelease
available on Nuget.

Let's change the Client console application to create a Cluster. We will
configure Client as a Seed-Node. To create a cluster no code changes are
required. You need to add a Nuget package: install-package akka.cluster
-pre. The configuration should look like this:

+-----------------------------------------------------------------------+
| akka {                                                                |
|                                                                       |
|     actor{                                                            |
|                                                                       |
|         provider = \"A                                                |
| kka.Cluster.ClusterActorRefProvider, Akka.Cluster\"                   |
|                                                                       |
|     }                                                                 |
|                                                                       |
|     remote {                                                          |
|                                                                       |
|         helios.tcp {                                                  |
|                                                                       |
| port = 8081 **#\<- configure the other Seed-Node with port 8082**     |
|                                                                       |
| hostname = \"127.0.0.1\"                                              |
|                                                                       |
|         }                                                             |
|                                                                       |
|     }                                                                 |
|                                                                       |
|     cluster{                                                          |
|                                                                       |
|       seed-nodes = \[ \"akka.tcp://client@127.0.0.1:8081\",           |
|                                                                       |
| \"akka.tcp://client@127.0.0.1:8082\"\]                                |
|                                                                       |
|     }                                                                 |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

It's important that you use IP-addresses or host names, but do not mix
them. Also make sure that all ActorSystems have exactly the same name,
otherwise the ActorSystem cannot join the cluster.

If you want to run the Cluster on a single PC, just copy the bin folder
of Client and change the port in the configuration. Both Seed-Nodes can
be started already. To create a Non-Seed-Node just copy the bin
directory again and change the configuration slightly:

+-----------------------------------------------------------------------+
| akka {                                                                |
|                                                                       |
|     actor{                                                            |
|                                                                       |
|         provider = \"A                                                |
| kka.Cluster.ClusterActorRefProvider, Akka.Cluster\"                   |
|                                                                       |
|     }                                                                 |
|                                                                       |
|     remote {                                                          |
|                                                                       |
|         helios.tcp {                                                  |
|                                                                       |
| port = **0 #\<- no fixed portnumber**                                 |
|                                                                       |
| hostname = \"127.0.0.1\"                                              |
|                                                                       |
|         }                                                             |
|                                                                       |
|     }                                                                 |
|                                                                       |
|     cluster{                                                          |
|                                                                       |
|       seed-nodes = \[ \"akka.tcp://client@127.0.0.1:8081\",           |
|                                                                       |
| \"akka.tcp://client@127.0.0.1:8082\"\]                                |
|                                                                       |
|     }                                                                 |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

Create three Non-Seed-Nodes by copying the bin folder three times and
start all clients. The logging in the Console on each node should tell
you that it's connected and will start processing the message.

You have created a cluster of five running nodes now and with that you
have created a Distributed application with Akka.NET that is highly
available and elastic.

## Conclusion

This article covers enough topics of Akka.NET to allow you to get
startedbuilding applications with Akka.NET. There are many more topics
to cover, for example Finite State Machines, Reliable messaging,
Persistence and Routers. Not every application is suited to be
programmed in an Actor-based framework like Akka.NET. If you have ever
programmed multi-threaded code, Akka.NET is a blast to use. Also for
applications that need millions of instances, Akka.NET gives you
possibilities that are very hard without actors. The use of
message-driven architectures requires a mind switch and the use and
design of Actors can be challenging. Akka.NET is not as mature as Akka.
Akka, for example, provides monitoring tools and a way to create
non-blocking REST-ful services with Akka.Http (Akka Spray). This way
it's much easier to make the ActorSystem available through REST.
Microsoft is about to release Azure Service Fabric which offers an Actor
Model programming model. Akka.NET runs on Mono too, it is open-source
and has a very active community. Azure Service Fabric is not available
as open source yet. Because of all this, Akka.NET is a framework worth
watching, and, if you so wish, to contribute to.

The complete source code for this article can be found on GitHub at
<https://github.com/XpiritBV/XpiritMagazine/tree/master/Edition-2>

Sources: <https://petabridge.com/blog/>, <http://getakka.net/docs/>,
<http://akka.io/docs/>
