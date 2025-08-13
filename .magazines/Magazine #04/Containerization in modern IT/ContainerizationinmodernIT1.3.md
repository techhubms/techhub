Modern application development uses container technology more and more,
whether this is for running backend or frontend services, or even for
developer tooling. Many organizations such as system integrators,
independent software vendors, and cloud solution specialists are
adopting containerization to keep up with the demands of large scale,
distribution, and modern development and operations practices. Many
others are jumping on the container bandwagon because everyone seems to
be doing it. Instead of blindly following along, it is good to
investigate and answer a number of essential questions. What are
compelling reasons to use containers or container technology, and what
does it mean to use it as a foundation for your application development?

# 10 compelling reasons to use container technology

1.  Higher degree of hardware abstraction

Using containers to run your software means that you are utilizing host
machines at a level where the individual machine doesn't matter anymore.
Instead of caring about single machines with well-known names and
maintaining and patching these, the operations team can focus on a
cluster of host machines. This cluster is a fabric of the host machines,
woven together by cluster software to form a contiguous set of resources
for computing, memory, and storage. Each machine is built with commodity
hardware and is expendable. The cluster can grow and shrink whenever new
hosts are added or defective hosts are taken out of commission. The
containers will be running on the accumulation of resources managed by
the cluster software, without knowing or caring about the individual
machines.

It is an important trend in the exploitation of cloud applications to
try and pay only for the resources actually consumed. Containers enable
more optimal use of the resources in the cluster. The cluster software
will allocate and assign the running containers based on the
requirements that the container images and application registration
indicates. These requirements might include a particular operating
system, memory constraints, and the required CPU resources. It can
decide what to run and where, while taking into account the constraints
that were given. The cluster might also shift containers around whenever
it sees fit for a better utilization of the available resources. At the
end of the day it is possible to achieve much higher density of running
containers and hence applications on the cluster, when compared to
running applications on dedicated machines. Fewer machines are needed to
run the same workload, which results in lower costs of operations.

2.  Scalable

The cluster offers a pool of resources that the containers can request
for operations. Whenever the need arises for more resources and bigger
scale, the cluster can spin up additional container instances from the
images and allocate more resources per container, if necessary. As long
as there is a surplus of resources it is easy to scale up, and down
afterwards. When resources start running out, good capacity planning
will make sure more hosts are added to the cluster for provisioning
containers. Smarter cluster software can manage scale better. Because
containers are very fast to spin up, the scaling process itself is a
matter of seconds instead of minutes or hours.

3.  Freedom in hosting

Ideally, the cluster software is installed on virtual machines to easily
provision a new cluster when necessary, or to add new hosts without
having to care about the actual hardware. These virtual machines may
reside in the cloud at any cloud provider such as Amazon AWS, Google
Cloud or Microsoft Azure, but they can also be located on-premise at
your own organization. There is no restriction to run the cluster
anywhere specific, although in some situations there is a benefit in
choosing for a particular cloud provider. For example, Microsoft offers
Azure Container Service which provides automatic provisioning of
Kubernetes, Docker Swarm or DC/OS clusters. The various cluster software
offerings run on different operating systems, both Linux-based and
various Windows Server editions.

4.  Container images are immutable

The DevOps team builds new application components by writing and testing
code, and compiling it into binaries. In a container world, they will
also prepare the container images that have all of the prerequisites for
the components, and deploy the new components onto them. The container
image is configured to run correctly and then deployed to a central
image repository. From that repository, the host can download and run
the images. Once the DevOps team releases the container image to the
repository it cannot be changed anymore. The images are immutable and
can be guaranteed to arrive at the hosts without any changes. Any
changes that occur are in the state of a running container, but never
the image. The DevOps team has full control over what will be running in
the containers, and does not depend on human intervention of
configuration. Moreover, it is no longer necessary to wonder whether a
container image can still run correctly at a later time. Whenever a
compatible cluster with the correct operating system and kernel version
is available, container images are guaranteed to run, since it is a
frozen set of code and configuration with no alterations since creation.
Rolling back deployments and maintaining a running application suddenly
becomes a lot easier.

5.  Container images are code and configuration

Container images combine the binaries built from code and the
configuration of the machine into a single entity. This inseparable set
offers better control, stability and predictability than before.

In traditional deployment scenarios, the code is deployed to target
machines separate from the configuration. This led to the rise of
Desired State Configuration, where script or a declarative language of
some form would describe the configuration for the application code and
its requirements at a host level. The code and resulting binaries were
usually handled by the development team. The team also describes the
required configuration in documentation. The operations team would then
get scripts from the development team to prepare the host machines and
to do final configuration of the software components. This turned out to
be error-prone, but also required multiple environments to separate the
various stages in an application's lifecycle, such as testing,
acceptance and production. Containers ensure that our applications will
always run in exactly the same context over different container hosts.

6.  Stages instead of environments

Having Development, Testing, Acceptance and Production environments
(DTAP for short) might be a thing of the past when containers and
clusters come into the picture. Traditionally there are separate
environments to indicate the level of quality and trust in your
application and its components. Now you can think of the various
environment stages that an application goes through as being reflected
by the versions of your container images. You could run the versions for
DTAP in a single cluster, at different endpoints managed by the cluster
software. If necessary, one can still separate a production from a
non-production cluster, but this will mostly be from a security
perspective, rather than for reasons of stability and availability of
the stages in the application's lifecycle.

The entire environment can even be described as code by composition
files describing the various elements and containers. For example, the
tool Docker Compose uses YML files to describe the various parts in an
environment for one or more applications. Transitioning from one
environment to the next, say staging to production, can be accomplished
by promoting these files.

7.  Fit and alignment with distributed architectures

Transitioning to container-based applications fits well with modern
architecture styles such as microservices. Large scale and heavily
distributed applications benefit from such an architecture, and
consequently the use of container technology. In the aforementioned
architectures, individual containers usually contain single processes,
such as a service for Web API, website hosting or background processing.
This requires one to rethink the design of the application in small,
isolated and manageable pieces.

Typically, each of the container images will be running multiple times
for failover, robustness and scalability. In addition, there will be
many separate container images that together form the complete
application. Each of the running container instances is
network-separated to build an intrinsically distributed application. The
application architecture must take this into account. You should
carefully consider the fact that other components use the network for
communication. This comes at a price and a risk. The call to another
component is not cheap in terms of time, when made over HTTP or TCP
compared to an in-process or single-machine call. Also, there is a
chance that the process in another container might not be available or
reachable. Countermeasures to deal with outages need to be built into
the application. Implementation patterns such as Circuit Breaker and
Retry become part of the developer's portfolio. It is likely that such
an architecture has its effect on the way the teams and company are
organized. The benefits of this new architecture type are a very nice
alignment with the container approach and with the DevOps teams that
build and run the applications. This alignment comes from the isolation
of multiple smaller parts in the system, which is designed to deal with
potentially short lifetimes or unavailability of the services. The cost
of such an architecture and different implementation patterns lies in
their complexity and learning curve.

8.  Support in application frameworks and tooling

There is an abundance of frameworks to facilitate developing code for
components and services to run in containers. The architecture and
development model allow a freedom of technology choice for each of the
containers, as these are loosely coupled and have no binary or technical
dependencies on one another. There is no specific vendor lock-in when
choosing the framework for development. However, a fit-for-purpose
selection is highly recommended.

As far as tooling is concerned, the world of containers has converged on
Docker as the de facto standard for interacting with container
instances, images and hosts. With the standardization around Docker as
the container interaction protocol and Docker as a company to deliver
the low-level tooling, other companies and software have chosen to
align. Development environments offer tooling and integration with the
Docker ecosystem. Docker images are used as the image format for
container clusters to deploy new components into the cluster. To an
increasing extent, web application services are being deployed from
Docker images, instead of doing installer or file copy based
deployments.

# 3 Reasons to not use containers

The above-mentioned reasons might be convincing to start with containers
tomorrow. As almost always, there is another side to take into account.
Containers might not be what is best for you, your applications, or your
organization.

1.  Container technology is not a silver bullet

Even though containers are popular, especially among developers and
operations, it is not the solution for all problems in application
delivery and architectures. Switching to containers as a basis for your
application will not necessarily solve or prevent performance, stability
or scaling issues. Containers do not fit every part of an application or
the bigger landscape that it is part of. Careful consideration is
required to decide when using containers makes sense and when it
doesn't.

2.  The benefits of containers come at a price

Creating an application that utilizes containers requires learning the
technology, investing time to change the architecture and the deployment
strategies. Various design patterns need to be implemented by the
developers in order to make a resilient, robust distributed application.
The build and release pipelines have to be changed for the creation and
deployment of container images. This is more complicated than a
traditional monolithic application and its lifecycle. The new way of
combining development and operations into DevOps involves changing the
teams, and aligning the organization with the business domains.

So, while the benefits that an application and the landscape will have
from using containers, they do not come free of charge. It means
investing before the benefits can be reaped, and there is no guarantee
that in the end, the benefits outweigh the costs, particularly for
simpler and smaller applications.

3.  The application paradigm is moving further

Containers are fairly new and many are still getting their heads around
applying them in application development. The world of modern
application development is moving on in a rapid pace and other, newer
ways of creating distributed applications are emerging. Serverless
computing is a nascent form of computing that allows near-infinite
scaling and a cost-model that charges by the millisecond consumed. It
introduces an even higher level of hardware abstraction, because it does
not even require hosts like a cluster would need. Additionally, the way
serverless computing is implemented focuses on the actual logic instead
of much of the plumbing that container implementations still need. This
implies that the costs of building and running applications may be lower
when choosing a serverless model.

# Final thoughts

Container technology is taking a prominent place in today's approach to
application development and hosting. There is a lot to be said and
considered for switching to containers as technology for modern
cloud-scale applications.

Choose wisely and modernize your IT with containers, ... if it makes
sense.
