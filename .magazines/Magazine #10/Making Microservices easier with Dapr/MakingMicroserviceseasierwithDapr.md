# Making Microservices easier with Dapr

In today's day and age, building software is all about how fast you can
bring value to the market. Microservices and autonomous teams that build
and run these services are an excellent fit for this goal because these
teams have no dependencies on others for building and releasing their
software. Microservices can be hard to build, though. You may be taking
away some complexity of working together on the same solution with
several teams, but they also add complexity in several ways. One of
these things is the fact that if your teams want to be autonomous, they
will need a lot of skills spanning a wide range of technologies.

A few years ago being a full stack developer meant that you were able to
do some frontend and backend development. However, today a lot more is
required than programming frontend and/or backend solutions. Nowadays
you also need to know your fair share about cloud infrastructures,
network communications, security, CICD tooling and other tools. Having
to spend a lot of time in all these domains distracts you from the
reason why we went to autonomous teams: delivering business value
faster. Dapr (**D**istributed **AP**plication **R**untime) is a new,
open-source project created by Microsoft that tries to come up with an
answer to these problems.

Dapr focuses on providing developers with tools that work on the cloud
as well as on edge, and make it easier to build resilient microservices.
It does this by handling a number of things for developers such as state
management, publish/subscribe, secret management, service invocation,
and it even has a built-in actor model. Dapr wants to create this in a
way that works with greenfield microservice landscapes, but it can also
be used in existing services to remove external dependencies. In
addition, Dapr also works with any programming language or developer
platform.

## How does Dapr work?

Dapr is an open-source framework for building resilient microservices.
It achieves this by providing developers with a set of building blocks
that can be accessed over HTTP or gRPC. Because it only depends on these
transport protocols, developers are free to choose from any language to
develop their microservices. These building blocks support the
fundamental features required by developers to build microservices, for
instance service invocation, state management, and publish/subscribe
messaging.

![](./media/image1.png)


Dapr abstracts these building blocks behind standard HTTP or gRPC calls,
as mentioned before. It does this by providing your service with a
sidecar that is accessible over HTTP or gRPC. A sidecar is a utility
container in the Pod, and its purpose is to support the main container.
Generally, the sidecar container is reusable and can be paired with
numerous types of primary containers. Because of this abstraction behind
a sidecar, you as a developer don't need to be bothered with
implementing plumbing logic, for example, publishing an event on a
message bus. Your microservice just makes a call to a local API exposed
by the Dapr sidecar to post an event on a message bus. The actual
implementation of the message bus is not relevant to the microservice.
It can even be swapped over time. The Dapr runtime/sidecar is configured
with information about which logical implementation you want to use
behind the Dapr APIs.

![](./media/image2.png)


This abstraction means that publishing an event is as simple as making a
call to <http://localhost:51987/v1.0/publish/MyArticleEventTopic>.
Another example is the use of persisted data. If you need to store state
inside your microservice, you just make a call passing the payload to
the state-store running at <http://localhost:51987/v1.0/state>.

### Using Dapr

There are two ways to use Dapr. The first way is to run it locally on
your machine. A prerequisite for this is the availability of Docker.
After installing Docker, you install the Dapr CLI, and after it has been
initialized, it makes sure that the Dapr runtime is available for you to
use.

The second way to use Dapr is to install it in Kubernetes. You can use
the Dapr CLI installed locally to install Dapr into your Kubernetes
cluster. However, it is advisable to install Dapr into your cluster
using the available Helm chart.

### Programming with Dapr

While Dapr provides a nice abstraction around the functionality
implemented under its API, there are also SDK's available to program.
For example, to implement the actor model in a microservice you can use
the provided SDK.

## What features does Dapr offer?

**State Management** is a core capability of almost every service. Dapr
provides you with a very simple-to-use state management API that allows
you to do either an Http GET or POST request to retrieve or store
key/value pairs into storage. As a developer, you don't have to think
about what type of storage is underneath. Currently, Dapr supports
Redis, Azure CosmosDB, AWS DynamoDB, GCP Cloud spanner, and Cassandra.

![](./media/image3.png)


You might think that storing data is not that hard to implement
yourself. However, in a distributed application there are several
challenges you will have to consider, for instance consistency and
resiliency. In addition to removing these challenges, Dapr offers a
number of built-in retry policies, including an exponential back-off
pattern. Consistency is also a configuration option in which you can
choose between eventual consistency and strong consistency. Eventual
consistency is the default option.

**Secret Management**

All applications and microservices need secrets. Dapr offers an easy way
for developers to access and use secrets in their microservice. Secret
management is performed in the same way as state management, without the
developer knowing the underlying implementation of the secret
management. It can be Azure Keyvault or Hashicorp Vault or another
supported secret store. The developer just calls the local secret API
and Dapr takes care of the abstraction to the actual secret provider.

**Service Invocation.** Calling other services from a service is also a
common scenario. Knowing where each service is running can become quite
a burden. Dapr can also take this problem out of your hands. Dapr allows
you to make calls to the Dapr runtime on localhost using HTTP such as
GET
[http://localhost:\<daprPort\>/v1.0/invoke/\<appId\>/method/\<method-name](http://localhost:%3cdaprPort%3e/v1.0/invoke/%3cappId%3e/method/%3cmethod-name)\>.
Dapr will route this request to the Dapr runtime of the other service
and send back the response from the service in a similar way.

Like state management, Dapr adds several features for performing service
invocations, which means that as a developer, you don't need to worry
about cross-cutting concerns such as doing retries and enabling
distributed tracing over multiple services.

![](./media/image4.png)


**Publish / subscribe** is a common communication pattern in
event-driven architectures and microservices architectures. The concept
of publish/subscribe using Dapr is the same as with state management &
service invocation. You can make an HTTP POST towards localhost to
publish a message to a topic like this:
http://localhost:\<daprPort\>/v1.0/publish/\<topic\>

To subscribe, you first have to do an HTTP GET to your Dapr runtime to
let the Dapr runtime know you want to subscribe
http://localhost:\<appPort\>/dapr/subscribe with the topic in the
request body. After subscribing the Dapr runtime will do a POST towards
your service to deliver the messages POST
[http://localhost:\<appPort\>/\<topic](http://localhost:%3cappPort%3e/%3ctopic)\>

![](./media/image5.png)


**Distributed Tracing.** Finding problems in distributed applications is
a difficult and tedious task. Debugging through a monolithic application
is already hard, but adding multiple services in the mix makes the
puzzle so much harder.

Because Dapr handles all communication through the Dapr sidecars, it is
quite easy to collect all the telemetry data from this communication and
gather it at a central location. Dapr can export all telemetry using the
"Open Telemetry" standard, so various tracing tools such as Jaeger,
Zipkin, and Application Insights can use it.

![](./media/image6.png)


When you send this telemetry to one of these tracing tools, it makes
finding problems in communication a lot easier. It also provides you
with a good overview of how your app is communicating, as seen in the
following picture. You can get these insights without writing any
specific telemetry code yourself, which makes it very easy to obtain
insight into the performance and logging of your microservices.

![A picture containing text, map Description automatically
generated](./media/image7.jpeg)


**Actors**

'Dapr Actors' is an implementation of the virtual actor design pattern.
As with any software design pattern, the decision whether to use a
specific pattern is made on the basis of the presence of a fitting
software design problem.

Dapr actors are virtual, which means that their lifecycle is not tied to
their in-memory representation. When a Dapr actor is first called with a
message to its actorID, it is automatically activated and an actor
object is created. These objects are removed, and garbage is collected
after some period. When the actor is called again, a new object is
created with the state of the previous actor object. Dapr uses the
configured state management to store the Dapr actor state. Because the
Dapr framework handles the state management and the (re)activation of
the actors, as a developer you don't need to implement this yourself.

![](./media/image8.png)


## Future / Conclusion

At the time of writing this article, Dapr is available as a preview
product. A lot of features are already available to try. Dapr decreases
the complexity of building microservices. It provides a framework that
abstracts the complexity delivered by a distributed microservices
application. Because Dapr leverages containers and Kubernetes, it
becomes portable across cloud and edge computing.

Dapr provides an extensive set of building blocks, ranging from service
invocation and state management to pub/sub messaging between services.
It also provides a virtual actor SDK, as well as distributed tracing
between services. With the growing active community of people using and
extending Dapr, more building blocks will become available in the
future.

Starting with Dapr is quite simple. There is an extensive range of
samples to get you going. Since everything works with HTTP, you can add
it to almost any application written in any language. Give it a try and
see how Dapr can help you build better microservice applications.
