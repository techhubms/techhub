# Why Containers Will Take Over the World

Containers are the third model of compute, after bare metal and virtual
machines - and containers are here to stay. Docker gives you a simple
platform for running apps in containers, old and new apps on Windows and
Linux, and that simplicity is a powerful enabler for all aspects of
modern IT.

In this article I\'ll walk through the major use-cases where people are
using containers today, I\'ll show why Docker is a safe technology
choice to invest in, and I\'ll finish with a learning path for you to
get started with containers.

## Use Case #1: Cloud Migration

Every company has a view on moving apps to the cloud. From five-year
migration programs, to an immediate need to migrate because the data
centre provider is shutting down in three months (this really does
happen). Whatever the driver, running in the cloud should bring agility,
flexibility and cost savings. To get there you used to have to choose
between two approaches: Infrastructure-as-a-Service (IaaS), and
Platform-as-a-Service (PaaS).

IaaS means renting virtual machines and deploying your apps in the same
way you do in the datacenter. You can use your existing deployment
steps, but you take all the inefficiencies of running virtual machines
on-prem into the cloud. Take a simple example of three distributed apps,
where each component runs in a separate VM for isolation:

![](./media/image1.emf)


Running those apps to the cloud might use 30 VMs in production, for high
availability and scale. That can cost \$5K a month. You\'ll still have
single-digit CPU utilization for your money, and you can't dynamically
scale because of the time it takes to start and configure new VMs.

PaaS is at the other end of the scale. It means using the full product
suite of your cloud provider and matching the products to the features
your app needs. In Azure that could mean using App Services, API
Management, SQL Azure and Service Bus queues.

You get complete managed solutions from the PaaS option, together with
high-value features like auto-scaling. And using shared services means
you should save on cost - but it\'s going to take a project for every
app you want to migrate. For each app you\'ll need to design a new
architecture, and if you\'re swapping out core components you\'re going
to need to change code.

Docker gives you a new option which combines the best of IaaS and PaaS -
move your apps to containers first, and then run your containers in the
cloud. It\'s a much simpler option that uses your existing deployment
artifacts without changing code, and it gives you high agility, low cost
and the flexibility to run the same apps in a hybrid cloud or
multi-cloud scenario:

![](./media/image2.emf)


The process is simple. For each component in your app you write a
Dockerfile, which is a script that deploys the component into a Docker
image. Docker images are a snapshot of one version of your component -
they contain the compiled binaries, dependencies and configuration -
everything your app needs to run. The image is portable, you share it by
pushing it to a central registry of images, which could be the public
Docker Hub or your own private registry.

Then you run your app in a container and it runs in the same way
everywhere.

## Use Case #2: Cloud-Native Apps

Cloud-native applications are how greenfield apps should be built. The
Cloud Native Computing Foundation definition is container packaged,
dynamically managed, microservice apps. In a cloud-native architecture,
each component runs in its own service, with its own private data store.
The **microservices-demo** application on GitHub is a great sample
architecture:

![](./media/image3.png)


It's a web app with a single front-end, but the full feature set is
provided by many small services -- like the catalogue service, cart
service and payment service. Those services each run in their own
containers. Logically they form one app, but they\'re physically
distinct components and that means they can all have their own
deployment cadence. You can add catalogue features by deploying an
updated catalogue service, without changing any of the rest of the app.

This is a huge enabler for the business because it removes lengthy
regression-test cycles and decreases the time from idea to deployment.
There\'s no need to test the cart service or the orders service if
you\'re adding a catalogue feature, because those other components stay
the same. It\'s also a great technology enabler. The sample project uses
.NET Core, Go, NodeJS, Java, Mongo and MySQL - a whole range of
technologies. The architecture gives you the freedom to use the right
technology for each component.

You can also include production-grade open source components into your
solution. Popular technologies are already packaged into public Docker
images which are owned by the OSS project team, so you get the
best-practice configuration of the latest version of the software, just
by running a container. Check out the Cloud-Native Computing
Foundation's landscape, which categorizes a huge range of technologies
that fit neatly into cloud-native apps, from message queues and
databases to metrics servers and dashboard visualizers.

Adopting cloud-native design accelerates your app delivery and the end
result is a self-healing application which is cleanly defined in a
single manifest file, and which you can deploy to any Docker cluster,
knowing it will work in the same way everywhere.

## Use Case #3: Modernizing Traditional Apps

Cloud-native apps should be an important part of your future projects,
but enterprises already have a much larger landscape of traditional
applications. These are apps with a large monolithic codebase, running
as single components. They may have manual deployment processes, or they
may be automated by joining many tools. They are complex and
time-consuming to develop and test, and fragile to deploy.

Many organizations are also managing apps across a range of operating
systems which are at or nearing end-of-life - including Windows Server
2003 and 2008. It\'s hard to maintain an application landscape which is
running on diverse operating systems, which each have different toolsets
and different capacities for automation.

Docker brings consistency to all containerized applications, old and
new, on Windows and Linux. The Windows Server Core Docker image is
maintained by Microsoft and it has support for older application
platforms - including .NET 2.0 and 32-bit apps. You can take a 15-year
old application and run it in a Windows container, deploying your
existing MSIs in the Dockerfile, with no code changes.

You can run your monolith in a container and get all the efficiency,
portability and security benefits of Docker. Old apps which are still
being used but not actively developed can stay as monoliths. Apps which
are still active projects can make use of Docker to modernize the
application architecture. You can split features out of the monolith,
add new features and use functionality from open source components, all
running in containers and all managed by Docker:

![](./media/image4.png)

Containers let you evolve your traditional apps towards a cloud-native
design, without needing a 2-year project to rearchitect them as
microservices. You can run a production Docker cluster which has a
mixture of Windows and Linux nodes, for running cross-platform
distributed applications. You could run Nginx in Linux containers to add
performance, security and scalability to an ASP.NET WebForms app running
in Windows containers.

## Use Case #4: Technology Innovation

Technical innovation doesn\'t end with cloud-native apps. Trends like
IoT, machine learning and serverless functions are all coming closer to
mainstream, and they\'re all made easier and more manageable by Docker.
I'll focus on serverless here.

Serverless is all about containers. Developers write code and the
serverless framework takes care of packaging the code into a Docker
image, and running it in a container when a trigger comes in - like an
HTTP request or a message on a queue. The Cloud Native Computing
Foundation has specced out the architecture and deployment pipelines
which are common to all serverless platforms:

![](./media/image5.emf)


Serverless started with AWS Lambda and Azure Functions, but it isn\'t
just for the cloud. There are great open-source projects that use the
same architecture and pipeline, but they run in Docker, so you get all
the benefits of a consistent platform on the developer laptop, on test
VMs in the datacenter, and on any cloud.

Open-source platforms like OpenWhisk, Nuclio, Fn and OpenFaas are
powered by containers and have very active communities, as well as
support from enterprises like IBM and Oracle. And because they\'re just
containers, you can run a serverless platforms on the same Docker
cluster that\'s already running your cloud-native apps and your
traditional apps.

## Use Case #5: Process Innovation & DevOps

The last big challenge facing enterprise IT is about cultural change and
the move to DevOps, which should bring faster releases of higher quality
software. DevOps is rightly positioned as people and process change,
using frameworks like **CALMS** which focuses the change on *Culture,
Automation, Lean, Metrics* and *Sharing*.

But it\'s hard to make big changes and measure their impact unless you
underpin them with new technologies. Creating a folder called \"DevOps\"
on the shared drive and putting all your deployment documents in there
is not progress. Moving to Docker helps drive the change to DevOps,
underpinning all the elements of CALMS:

![](./media/image6.png)


The most significant benefit is helping the cultural change. Having
teams working on the same technology and speaking the same language --
Dockerfiles and Docker Compose files - is a great way to break down
barriers. And people are excited by Docker. It\'s an interesting,
powerful new technology which is easy to get started with and quickly
improves practices from development to production. Teams adopting Docker
are enthusiastic and that helps drive big changes like the move to
DevOps.

## Containers: Flexible and Open Technologies

Moving the to the cloud, delivering new apps, modernizing old apps,
supporting technology innovation and process innovation - that\'s pretty
much everything that\'s happening in the IT industry. Docker helps it
all happen, which is why containers will take over the world.

But there is some work to do to get there. You need to write Dockerfiles
to package your apps to run in Docker containers. You need to write
Docker Compose files or Kubernetes manifests to define all the pieces
that make up a distributed, containerized app. Deploying, managing and
monitoring apps is different when they\'re running across hundreds of
containers.

You need to make an investment to get the benefit of containers, but
it\'s a safe investment to make. You can start with what you currently
have, and you\'ll be moving to open technologies. You\'re not restricted
to certain languages or frameworks - you can Dockerize pretty much
anything if you can script the deployment. And you won\'t be locked in
to any one vendor - the Docker image format and the container runtime
spec are open standards, so you can run your apps on any container
platform.

## The Learning Path: Getting Started with Docker

If containers are going to going to take over the world, you\'d better
get on board. As soon as you start looking at the container space
you\'ll see a huge array of technologies - Docker Swarm, Kubernetes,
containerd, Istio, as well as all the vendor platforms - Docker
Enterprise, AKS on Azure, and Amazon\'s EKS. Where do you start?

Here\'s my opinionated learning path. It starts you off easily and adds
capabilities (and complexity) with each step. The idea is you stop when
you get what you need. The endgame could be running a cloud-native .NET
Core app on a service mesh with Istio on Kubernetes, or it could be
running .NET Framework apps in Windows containers on Docker Swarm.
Either of those is correct, if it works for you:

![](./media/image7.png)

There you are. It\'s simple really, and Pluralsight is your friend here,
there\'s a whole set of Docker and Kubernetes courses, with lots more on
the way. Now is the time to get started, so install Docker Desktop on
Mac or Windows and go Dockerize!
