# [The (r)evolution of Cloud Computing](https://wingedarchitecture.com/2016/11/28/the-revolution-of-cloud-computing/)

If there is one term that stands out in the current list of technology
developments, trends and buzzwords, it is **serverless computing**.
Serverless computing is the next step in the evolution of cloud
computing (delivery of computing services -- servers, storage,
databases, networking, software, analytics, and more -- over the
Internet).

Serverless computing takes away the necessity for you to think about the
computing capacity required for running your software. And what's more:
it lets you execute and pay for it on-demand. What is serverless
computing exactly and what are the benefits?

# Summary of Cloud Service Models

Before we discuss the concept of serverless, have a look at the
following diagram that presents a short recap of Cloud Service Models.
As you go through each of the diagram's pillars from left to right, the
focus shifts to the activities required to provide application
functionality to end-users.
![cloud-service-models](./media/image1.png)


Figure 1: Overview of Cloud Service Models

The model that is closest to on-premise IT is **Infrastructure as a
Service (IaaS)**. This model provides the basic building blocks for
Cloud Computing. The dedicated or virtualized hardware (networking,
storage and computing resources) is owned and hosted by the Cloud
Service Provider and is provided to a company in a virtual manner. This
company can self-provision the infrastructure on-demand, and does not
have to worry about maintaining the hardware. IaaS allows the company to
focus on utilizing the infrastructure and consumption-based payment,
instead of maintaining the infrastructure and making investments on it.

*IaaS allows the company to focus on utilizing the infrastructure and
consumption-based payment.*

The second service model, **Platform as a Service (PaaS)**, provides the
operating system, middleware and runtime on top of the infrastructure
layer. PaaS allows companies to focus on managing the scale of the
infrastructure, in addition to the deployment and management of their
applications. Management of underlying infrastructure is abstracted
away.\
*PaaS allows companies to focus on the deployment and management of
their applications.*

The last model in the picture is the **Software as a Service (SaaS)**
model, in which the application and data are also managed by the
provider. The software is licensed on a subscription basis and is hosted
centrally. Well known examples of these complete software products,
available as SaaS services, are: Office365, Google Apps, and SalesForce.
SaaS allows a company to focus on how to use the functionality provided
by the application, while it does not have to manage feature additions,
servers, or operating systems.

# Serverless computing

Now that we understand the various cloud service models, we can look at
the position of the concept of serverless computing. It should not come
as a surprise that serverless computing does not mean that there are no
servers involved. The core idea of serverless computing is that you
don't have to think about the infrastructure and computing capacity
required for your logic. Answering questions like how many instances of
your application are required or how many Virtual Machines you need
become obsolete. In this sense, the "serverless execution model" can be
positioned between the pillars PaaS and SaaS.

## Serverless computing: Backend as a Service

The serverless concept can appear in different forms. One way to look at
it is the use of 3rd party backend services within your own solution.
Let's take an example by looking at identity provider functionality.
Functionalities such as authenticating a user based on credentials and
resetting a user's password are provided by products such as Azure
Active Directory B2C, or Auth0. A functionality is executed on the basis
of incoming events (http service calls). These products take care of
scalability and let you pay per authentication. In addition to this,
some products allow you to add custom logic to their solution to tailor
functionality to your needs. The functionality in itself does not have
any end-user value -- it is a "semi-finished" product that needs to be
integrated in a solution. This appearance of serverless computing is
referred to as **Backend as a Service (BaaS)**.

## Serverless computing: Function as a Service

Another appearance of serverless computing is about breaking down an
application or a microservice into discrete functions. These small bits
of code, that are configured to be executed on the basis of events,
enable efficient resource utilization. The code of these functions
is deployed and configured into a cloud environment, and the cloud
provider takes care of running these pieces of code triggered by events
coming in. This is also referred to as **Function as a Service
(FaaS)**.  FaaS allows a company to focus on the logic that a piece of
software needs to provide, and to pay for it per execution. In the case
of FaaS, the focus is on the development and deployment of small pieces
of source code. Cloud providers have jumped in the FaaS space with their
own offerings: AWS Lambda (Amazon), Azure Functions (Microsoft), Google
Cloud Functions, and IBM OpenWhisk.

*With FaaS the focus is fully on the logic that needs to be provided:
the code*

FaaS matches perfectly with the concept of microservices, mobile and
Internet of Things (IoT). It provides auto-scaling and load balancing
out-of-the-box, saving you from having to manually configure clusters,
load balancers, etc. The only thing you need to do is to give the code
to your cloud provider and trigger it through events.

## Characteristics of serverless computing

Giving a single definition for serverless computing is likely to lead to
some discussion. The best way to define serverless computing is,
therefore, to state the characteristics it complies to. If we look at
the descriptions of BaaS and FaaS as described above, these
characteristics are:

-   it is event-driven

-   computing is done on-demand

-   scaling is done out-of-the-box

### Events

Execution of backend functionality or function code is triggered by an
event. The types of events depend on the ones offered by the cloud
provider. This can range from a file update on Amazon S3 or OneDrive, a
timer event, a message on a queue, or an incoming HTTP request.

Containers\
Containers are used in the core.. Containers wrap a piece of software in
a complete file system that contains everything that is required to run
this software: code, runtime, system tools, and system libraries --
anything that can be installed on a server. This guarantees that the
software will always run in the same way, regardless of its
environment. \
With serverless computing, the application code is taken, placed into a
container, executed, and torn down without you knowing anything about
this process and its underpinnings. This provides for execution
on-demand and easy scaling.\
The cloud provider takes care of finding a server for the code to run
on, and it scales up if necessary. The way in which this is physically
implemented, for instance the container architecture, varies between the
cloud providers.

## Benefits of serverless computing

### Efficiency in IT spend

FaaS, BaaS or other means of serverless computing can be much cheaper,
because you pay per execution, and you don't pay for resources that are
idle. What's more, no money is lost on managing the infrastructure and
platform required to do scaling. There's a variety of pricing models
available, so calculating the break-even point based on your demands is
advisable.

### Value-driven development

Value-driven development lets you focus on the functionality that is to
be delivered, and maximizes the time you spend on delivering true added
value to your end-users. Non-value activities such as managing the
infrastructure are removed from the software development process.\
This fulfils the goal of Lean software development: eliminating waste by
removing the non-value added components from the software development
process, which is more cost-efficient.

### Event-driven architecture

As mentioned earlier: FaaS matches perfectly with microservices, mobile
and IoT, which are the modern architectures of today. This is because of
the event-driven nature and level of granularity of FaaS. This type of
architecture with its loose coupling of components and good distribution
is a real strength, allowing for architectural agility and high
scalability.

# A revolution?

Serverless computing is a logical evolution of Cloud Service Models. In
its current form I don't regard it as a revolution. Revolutionary would
be the possibility of moving beyond virtual machines and containers.
Imagine that you just upload your cross-platform application code, in
your preferred programming language, to your favorite cloud provider.
And your code will run without you knowing anything about the
application container, execution environment, and the operating system
it will be running on. It auto-scales out-of-the box and you pay per
execution. That is going to be real serverless computing.
