Microservice architectures are all the craze nowadays. In my opinion it
is the next evolutionary step for loosely coupled and scalable
architectures. It builds upon the foundations we laid with Service
Oriented Architectures, when it was not spoiled by Erroneous Spaghetti
Buses or people who thought it was a good idea to throw a bunch of SOAP
webservices at a problem and call it SOA. After all, it's *service*
orientation, right?

One important thing is that you learn to disassociate webservices with
the *concept* of services. They are not the same, and mixing these two
has led to the abominations we have started to link to "SOA". In SOA, [a
*service* is the technical authority for a given business
domain]{.mark}, and can use multiple technologies to implement its
goals.

In this magazine, you can read more about what Microservice architecture
is according to Xpirit, so I will not go deep into its definition or
characteristics. Rather, I want to focus on how to get there if you do
not have a fresh green field to build your software in. What if you have
a system that consists of said SOAP webservices, an N-tier web
application with a fair amount of legacy? How do you transition that to
microservices? And why should you, even?

This article is based on my own lessons learned while migrating such an
application to a more loosely coupled architecture, using the
NServiceBus framework as its foundation. But even if you are not into
NServiceBus, the concepts and steps taken are basically technology
agnostic. NServiceBus just makes things a lot easier. You could also
look at using MassTransit, RabbitMQ or Azure Service Bus for messaging.
The key requirement is that you have some infrastructure to facilitate
in hosting your autonomous services and to provide the transport for
your messages and events.

# So what is wrong with N-Tier anyway?

The application I inherited when I came into the project was a
multi-tenant SaaS application that dealt with scheduling work for
employees in supermarkets, a Workforce Management System. It consisted
of a single database per tenant and a Silverlight front end backed by
WCF SOAP webservices. While Silverlight may seems like an obsolete
technology now, this is not really relevant because we would have had
the same issues if the UI was built as a Single Page Application with
AngularJS.

![](./media/image1.emf)


We had a couple of challenges:

-   We were about to scale out to many more customers, and subsequently:
    users, and we knew the system wasn't able to handle that at the
    time, for reasons that will become clear in this article

-   We also had some work to do to make the system a real multi-tenant
    system

-   There was an increasing demand for standardized integration with
    other external systems, such as HRM or Payroll systems and the
    existing solution was based on point-to-point webservices and under
    high load, was known to lose data

So what is wrong with this multi-tier architecture? We learned that
layering is good because it helps separation of concerns and abstracts
many technicalities as you move up the stack. Above all, layers provide
loose coupling, right?

Let us take a look at this conceptually first: from 10,000 ft., this
architecture looks like basically any web based application. It is a
pretty generic structure. [And there is the first problem: the
architecture is driven by a *technical* breakdown]{.mark}. We have our
database, our service layer containing application logic, and our UI
layer. But what does this architecture say about what the application
actually *does*? Sure, if we zoom in a bit, there are areas in the code
to be found, such as Budgeting, Scheduling and Payment, but this is not
its top level structure.

Moreover, this architecture forces us to shoehorn all functionality into
these layers, forcing us to implement repositories, catalogs, webservice
facades, webservice client wrappers, UI modules and end user screens for
every feature or entity what was added. Layers upon layers upon layers,
with the occasional data mapping in between.

Loose coupling you say? Try changing the datatype of a field, or adding
a field to an entity and you will see that it is not loosely coupled at
all.

The point is: apparently there is inherent coupling from top to bottom
within a certain functional area of the system and no amount of layering
is going to decouple this. Adding layers will just amount to more code,
more plumbing, more indirections and needless abstractions. Why bother?
[This vertical kind of coupling is quite natural and you should not
fight it]{.mark}.

What is even worse: this technical layered approach encourages
developers to create high and unwanted coupling *within* a layer. On our
case, the architecture of the system on a 300 ft. level, looked like
this:

![](./media/image2.emf)


Why are these four functional areas irregular shapes, and why do they
appear to overlap? It is because none of these modules had a clear
boundary, and all modules took the freedom to peek and poke in each
other's database tables, or use each other's repositories to achieve
their goal. So in essence, we were dealing with a monolith.

For some of these webservice operations, the thread of execution looked
like this:

![](./media/image3.emf)


It went all over the place, basically because the way the system was
modeled led to the mindset that we had to do everything we needed to do
to make the system consistent within that same webservice request,
before returning to the calling front end application. The upside of
this is that the system is consistent across all modules after the
operation completes. Immediate consistency.

But the downsides are much bigger:

-   Hard coupling between the functional areas of the system, resulting
    in nearly unmaintainable code;

-   Temporal coupling, because the caller has to wait for every request
    to complete

-   A huge scalability problem because the more the system has to do,
    the longer the operation will take, thereby blocking precious IIS
    threads in the resource pool, and finally:

-   Locking and blocking on the database because everything is handled
    inside a single huge transaction.

Under the increasing load we were adding to the system, we saw it break
with exponentially growing execution times, database timeouts or even
HTTP timeouts because the Silverlight client would give up waiting for
an answer. This is what tends to happen with these RPC style webservice
interactions. As load increases, throughput stagnates and breaks down at
some point:

![](./media/image4.emf)


# If it's not by layers, then how *do* we decouple?

This is where the principles from microservice architecture come in...
Like Martin Fowler describes, a microservice is:

> \[...\] **built around business capabilities** and **independently
> deployable** by fully automated deployment machinery.

Roughly translated: a microservice is bounded by its (business) domain,
and decoupled in nature because it operates autonomously. (SOA, anyone?)

If we look at the system from this angle, surely we should be able to
transform the irregular shapes in the earlier figures to more clearly
defined ones. Something like this:

![](./media/image5.emf)


That looks much more structured. We see that some of the layering is
still there: we had that Silverlight frond end that runs on the end
user's computer and it needs to cross the network in order to reach the
server logic. HTTP and webservices are fine for that. Note that these
webservices are now drawn inside the "Presentation Layer" box. The
reason for this is that I consider them private webservices, used solely
to serve the UI. Their logic is part of presentation, and the *physical*
webservice layer is only there to cross that network boundary. This way,
from an architectural point of view layering only becomes a
technicality.

We can clearly see that each microservice spans from top (UI) to bottom
(service logic and data storage). As mentioned earlier, vertical
coupling is natural. But horizontally, there is no direct coupling
anymore, which is the type of decoupling we want to achieve.
Communication between (micro)services happens through *events* that are
published by microservices, and I have introduced a Message Bus to
facilitate that communication. In our case, it was NServiceBus that
facilitated the publishing and receiving of these events.

"But, where is the database?" you may ask. Well, in the true
microservices mindset, each microservice has its own datastore, which
can be anything ranging from an RDBMS to a Document Database to a file
share. It is part of the service.

# Breaking up our application

So, how do we transition our existing codebase from A to B? Here is a
five step plan to help you out:

1.  Together with your domain experts, **identify significant *business
    events* in your system**. E.g. "employee created", "employment
    changed", "budget finalized", "payment finalized". **Start
    publishing these events from your application**, and roll out more
    and more events with every update of your system, even if you don't
    handle them yet.

2.  **For new features**, preferably the ones that happen "at the edge"
    of your system, **start using these events as triggers** to do the
    required actions. Examples are: export functionality, calculating
    reports in the background, generating tasks for a task list,
    deduplication of data for performance reasons. This makes it easy to
    add new modules to the system without touching much of the existing
    code.

3.  **Start thinking about how to create a composite UI** to tie these
    services together. As you break up your system into more autonomous
    components, your UI must remain coherent to the end user.

4.  **Over time, migrate existing code from your webservices to the
    autonomous services** where possible. Shrink the webservices down to
    the bare essentials: make sure the webservice does everything it
    needs to do to get a (command) message on the message bus, and
    return as soon as possible. This will increase the responsiveness of
    your webservices, and free up IIS request threads to handle more
    load. Of course, if the front end requires data, e.g. with a query,
    you should not shoehorn asynchronous messaging into the scenario. It
    is OK to call a repository directly to retrieve the necessary data.

5.  When you are ready for it, **split up the database** so that you can
    give each service its own storage.

> Your system will probably look similar to this during the transition:
>
> ![](./media/image6.emf)
> 

The irregularly shaped modules will start to shrink and lose their
overlap. Some modules may even get a different name because you have
found their purpose. In our case, a chunk of code with the generic name
"Common" was renamed "Organization" because it dealt with concerns
surrounding the customer's organizational structure. Along the edges of
the system, connected via the Message Bus, new services will appear that
leverage events to do their work. And along the way, inside your
database, it will become apparent which tables belong to which
microservice.

Adding messaging to your system will generally give your system better
throughput. Up to a certain point the throughput will keep up with the
load on the system, but at its peak it will remain constant because we
introduced decoupling:

> ![](./media/image7.emf)
> 

# Side effects

With this transition to microservices, you might see some side effects.
Some of them are a result of a different style of thinking once you let
go of the "everything needs to happen inside one web request" mindset.

## Task based UI paradigm

As you introduce messaging and events, you will start to think in terms
of commands to fulfil some user intent and events that arise from those
actions, in turn triggering (sub) processes in other microservices in
the background. A *Task Based UI* is a natural fit for this
command-style interactions. For example: once a budget is finalized, a
task will come up for the user to start working on a schedule. But also:
actions from the UI will become much more intentional. Instead of
CRUD-like data interaction, where the user edits a set of data fields
and presses "Save", buttons and actions will be much more modeled to
capture the actual user *intent*: "renew employee contract", or
"finalize budget".

Keep in mind that a task based UI requires a different mindset from
everyone in the team. Your domain experts and system analysts (those who
write your user stories or system specs), have to be the first to
understand and embrace this style of thinking. The same goes for
eventual consistency. You must engage with them in a discussion about
where (ACID) transaction boundaries lie, and where you can divide things
up. It all has to make sense from a functional angle as well.

## Performance

Do not think that throwing in a message bus and putting commands on a
queue alone will alleviate your performance problems. If the code that
handles these commands is still the same spaghetti code that does too
much, this execution will still take too long. While the system will be
able to handle this load more comfortably, the end-to-end waiting time
for the user may become much longer, because of the queueing. Only
migrate code from your webservices if you can successfully break it down
into distinct and (ideally) small responsibilities. This usually
involves rewrites or large refactorings, so this is something that will
take time and multiple releases to get right

# Closing thoughts

[Because you are in a brown field situation, prepare to make concessions
at first]{.mark}. You will have to take it one step at a time, and
sometimes it makes sense to release the system with some code
duplication while you move logic to the background in increments. But
always be aware of when you are creating this type of technical debt:
make sure to clean it up at the first opportunity.

## What about my Big Ol' Database?

It is unrealistic to think that you can just give each newly defined
microservice its own database right from the start. You will find that
there is a lot of hidden coupling inside or via the database which you
need to disentangle step by step. Therefore it is more realistic to
start out by keeping that same single database, and instead try to
sketch the microservice boundaries into the datamodel as you assign sets
of tables to each microservice's area of responsibility. Over time, you
will find out where you can cut out parts of the database and move it to
a separate store, and where you will need to apply data duplication by
means of events across services. Just as long as you manage to designate
which microservice is responsible for that part of the data. I.e. which
service is responsible for the creation of that data, and who only
consumes it?

## Monitor, measure and learn!

Add monitoring and instrumentation to your system as soon as possible.
Especially as you start publishing and handling events, you have to have
insights into whether all these autonomous services are doing their job,
let alone how the overall system performs. We used New Relic to handle
that at the time but there are also other options, such as Application
Insights, which is part of Microsoft's Azure offering.

Once you have monitoring in place, it can even drive your transition:

-   **Usage analysis**: find out from your usage statistics which parts
    of the system are used the most, and focus on transitioning those
    parts of the system first. You may even find out that some parts are
    never used, and you can decide to remove them over time, or find
    other ways to achieve the same goal.

-   **Performance analysis**: look for the performance hotspots in your
    system. Which webservices are causing the most contention or
    slowness in the system? Try to break those up first.

The most important thing is to learn from your system as you perform the
transition to microservices. I found things I had never anticipated just
by studying our user's behavior.

[Be prepared to take around a year or more for a transition like
this]{.mark}.
