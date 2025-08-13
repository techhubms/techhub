# Introduction

Microsoft Azure offers several options for hosting your web-based
applications, and it is difficult to choose the best option for your
scenario. It might look easy to just take the simplest way of moving
your infrastructure to Azure in an IaaS way, but this is probably not
the best option. There is **no** single silver bullet option that is
always best. In this article, we will discuss the various options to
consider when you choose to move your web workloads to the cloud.

Figure 1 shows a flowchart that could help you decide which flavor fits
your application. This flow chart focuses on several questions that we
will elaborate on in the following section. Please consider that it is
impossible to take all possible questions and scenarios into
consideration in this flowchart, so use it as a rule of thumb. The
questions described below are some of the main considerations you should
take into account when choosing your cloud web infrastructure.

![](./media/image1.png)

## Cloud Agnostic?

Microsoft Azure offers a lot of specific Platform as a Service features
-- for example App Service -- that remove the effort of managing your
infrastructure. When you choose a solution that is specific to Azure, it
can become more difficult to make a switch to other cloud providers in
the future. The choice is therefore between development speed combined
with tighter coupling with Azure, or more overhead in managing your
environments, which gives you more control over where you host your
application. Typically, you're fine with just choosing one cloud
solution provider, but sometimes regulations, uptime requirements or a
specific application architecture require that you use multiple
providers to host your applications.

## Containerizing applications?

Being able to host your applications using container technology such as
Docker gives you a lot of flexibility and options. Microsoft is making
working with containers better each day by adding support for developing
applications in both Azure and Visual Studio. Containers can run
anywhere on any cloud platform or on-premise, making it a very viable
option for both new and existing applications. Tools such as
Image2Docker (https://xpir.it/xprt-right-cloud1 ) make it easier to
convert existing applications to Docker containers, so even legacy
applications can be hosted using container technology.

## Highly unpredictable and flexible workloads?

This question focuses on the usage of your jobs or APIs. In most
scenarios, the workload on your APIs will be distributed quite evenly.
When your workloads are very unpredictable or they tend to increase at
certain moments while at other times there is no load at all (for
example monthly batch calculations), specific solutions make a better
fit than others. In Serverless options such as Azure Functions, you only
pay for the actual use of the executed code, making it ideal for
scenarios like this.

## Interacting with on-premise / hybrid cloud scenarios?

Some hosting solutions cannot be added to a VNet in Azure, making it a
lot harder to securely connect these options to resources that are not
hosted on Azure. This is something to consider when building
applications that should live in a hybrid cloud scenario where you
connect a cloud solution to on-premise services or databases. Azure has
made flavors for most of these options that include VNet support. For
instance, for App Service they have created the "App Service
Environment" option. However, setting this up is more expensive and
requires more work compared to the normal App Service.

# IAAS

Infrastructure as a Service (IaaS) is an instantly available computing
resource. It is provisioned and managed for you by the Cloud vendor. It
enables full customizing of the VM after the initial provisioning. But
because it is only a managed VM, you are responsible for keeping the VM
healthy, which means that patching and security hardening are the user's
responsibility.

### Pros

-   Lift and shift -- a way of working you are already familiar with

-   Easy to move between cloud providers

-   Can host any technology, Windows, Linux, any webserver technology

-   Familiar way of working for most IT departments

-   Complete freedom of workload

### Cons

-   You are still responsible for a lot of management yourself, e.g.,
    patching OS and maintaining configuration

-   Often not cheaper than non-cloud hosting

-   Not very effective usage of hardware

# Azure App Service

Azure App Service is the Platform-as-a-Service offering from Microsoft.
It allows for rapid deployment of your applications in a wide range of
languages and frameworks. It allows for rapid scaling and a fast setup
time, from local development to Azure Cloud production in 10 minutes.

### Pros

-   Easy to set up and get going.

-   Fully managed infrastructure. Only focus on the web application
    itself

-   A lot of supported programming languages: NET, Java, Node.js, PHP
    and Python.

-   Easy to scale up or out

### Cons

-   No VNet support for secure connections to on-premise from Azure

-   Directly exposed to the internet. No direct control over inbound and
    outbound traffic

# App Service Environment

The App Service Environment offers a completely isolated
Platform-as-a-Service hosting environment. It allows you to host your
web workloads in a dedicated environment. The most important feature
from a customer perspective is that it allows deployments into a
customer-defined Virtual Network. Because an App Service Environment is
deployed into a VNet, customers have finely grained control over both
inbound and outbound application network traffic, as well as connecting
to on-premise resources using VPN or express-route.

## Pros

-   Isolated environment

-   Deployed in a customer's virtual network

-   Fine control over inbound and outbound traffic to your apps

-   The ILB (Internal Load Balancer) ASE offers the option to use your
    own application gateway and firewall to expose your applications to
    the outside world

## Cons

-   ASE adds complexity during the initial roll out

-   Costly option if you don't have a lot of applications

-   High startup costs.

# Containers in App Service

The Containers in App Service offers the option to deploy Linux-based
Docker containers in the App Service model. It offers all the benefits
of App Service, but with the added benefit that it enables you to use
the standardized Docker format to deploy your application.

## Pros

-   No management of infrastructure

-   Use the familiar Docker format to deploy

## Cons

-   No VNet isolation

-   Only Linux-based deployments (PHP, NodeJS, ASP.NET Core)

# Azure Container Services

Azure Container Services (ACS) enable the rapid deployment of a
container hosting environment using open source tools. At the moment
these consist of Kubernetes, DC/OS and Docker Swarm. ACS allows you to
deploy your complex applications as containers using both Windows and
Linux as the base OS. Because it is a container-based solution, it is
technology-agnostic as well as cloud-agnostic.

### Pros

-   Lots of different orchestrators to choose from

-   Multi Cloud solutions are viable

-   Technology stack independent

### Cons

-   You still have to manage infrastructure with OS / security updates

# Service Fabric

Service Fabric is also an orchestrator on the Azure platform like ACS.
But it also offers an SDK that describes how applications should be
written. There is a choice between reliable services and an Actor Model.
It also allows for guest executables to run inside Service fabric.
Microsoft is also working on bringing Docker Container support to
Service Fabric. Microsoft Service Fabric is not Azure only, you can
install it on-premise as well as in other clouds.

### Pros

-   Stateful services are provided out of the box using the Service
    Fabric SDK

-   Can run anywhere

### Cons

-   Higher startup costs compared to other container orchestrators.

# Serverless

Serverless technology is the most cloud-native option in the list. Azure
Functions and Logic Apps were made to offer you the things you expect
from the cloud. In Azure Functions, you build a piece of code that is a
single function with some input parameters and some outputs. Then this
single function is uploaded to Azure and it will run from there. There
are almost no operations involved and there are several triggers to
start your function, ranging from HTTP triggers to timer triggers, to
triggers that act on changes in table storage. Azure will automatically
scale your function so it can run in parallel in order to quickly
execute batch jobs or just single events, and the best part is that you
only pay for each time the function runs.

### Pros

-   Pay for what you use

-   Large set of triggers to start jobs

-   No infrastructure to manage

-   Easiest operation model

### Cons

-   Not made for hosting web applications, only focused on APIs and jobs

-   No VNet support

# Conclusion

As you can see, there are several options to host your web workload on
Azure. We believe the days of IaaS are numbered, because in almost all
scenarios PaaS or container options are superior. Microsoft is heavily
investing in these areas in comparison with IaaS. Please note that the
flowchart we created for this article is just a general rule of thumb.
There can be other factors that may be important in your specific use
case when deciding which solution fits your application. It is a good
practice to keep the infrastructure in mind when designing your software
architecture. Modern software architectures such as microservices are
flexible in the way you host them and can fit in the cloud
infrastructure solution you choose to implement. This also allows you to
start small and simple, and adapt while you your application or load
increases in size.
