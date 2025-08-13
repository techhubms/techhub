# Containerization as a new application paradigm

It is hard to miss the emergence of container technology and its effect
on application development and architectures. Containers as lightweight
hosts for small applications are quickly associated with a
micro-services architecture (MSA). Applications based on micro-services
are split into small autonomous systems instead of monoliths. Small
services ask for light-weight hosting where creating a multitude of
hosted services is simple and cheap. This approach benefits greatly from
the high density of the services on host machines, and containers fit
nicely into these types of applications. This is why the reinvented
architectures around micro-services and the new hosting models have
driven the popularity of containerization of modern applications.

# Containers 101

At first sight, the concept of containers appears rather abstract. Quite
often a comparison is made with virtual machines. Although there are
some similarities, it is also a dangerous comparison. Virtualization is
not the same as containerization and virtual machines are not
containers.

Containers are essentially virtual fences created to perform isolation
of shared system resources and processes of a host system. The processes
in the host and within the containers all exist at the host level. Each
container thinks it has its own unique set of system resources assigned
to it. A container is oblivious to anything outside of its fence; it
cannot see anything outside of itself and any access to the container's
resources does not affect other containers or their content. The net
result is that each container is its own silo and can be thought of as a
way of creating a high density of sub-computers within a host system.

This is perhaps the reason why containers appear to be similar to
virtual machines. The isolation is definitely a recognizable attribute,
but the key difference is that a container runs directly in the host
system and nothing is virtualized, only isolated. Consequently, the
containers run their processes in the same operating system that the
host uses. A Linux host system will have Linux-based application
processes running and the resources behave as in Linux, such as
/dev/root. A Windows host will be able to create Windows based
containers running .NET and Win32 applications utilizing Windows
resources, such as the C:\\ drive and the registry. This is an important
aspect of containers: container and host OS are always one and the same.
Unless virtualization comes into play.

# Docker as an industry standard

Container support has been a feature in Linux and Unix operating systems
since the beginning. The popularization of container technology and
container-based application architectures has started with the advent of
Docker, both a company and tooling with the same name. Before Docker,
the challenge involved the semantic differences of the container APIs
inside the operating systems and the way that applications are deployed
to containers. Docker standardized the methods of creation and
interaction with containers. Docker offered a uniform API and
Command-Line Interface (CLI) for controlling containers. It allowed a
user and application to start and stop container instances. It also
defined a standard for building container images, which contain (pun
intended) resources and settings inside a newly created container.
Container images are packages that are used to deploy your application
assets for use in containers. Docker envisions container images to
consist of layers. Each image is based on a parent image, going back to
a (nearly-empty) base image, and only has the changes from the parent.
This approach provides a highly efficient way to build container images.
However, it is important to remember that the root parent image is
always intended for the particular operating system it was created for.

The popularity of Docker has made it the de-facto standard for working
with containers across a variety of platforms. Docker started in the
open source Linux eco-system, intended for and targeted to Linux-based
applications, but has expanded to Windows.

## Docker Toolbox

The first available option to run containers on Windows-based host
systems is to use 'Docker Toolbox'. It offers Linux-based containers and
images on Windows. This seems like a contradiction from the previous
remark that host and container OS are always the same. Docker Toolbox
uses Oracle VirtualBox to create a virtualized host system running a
Linux distribution, in which the container instances are hosted.
Admittedly, it does sound like a scene from the movie Inception. Until
then, Windows itself was not capable of hosting containers, so Linux had
to be the host operating system. Virtualization helped out by offering
Linux container technology to Windows.

## Docker for Windows

The second offering from Docker Inc. for the Windows platform was
'Docker for Windows'. In principle it is a natural progression from
Docker Toolbox. The main difference is that it uses Windows Hyper-V
technology as the virtualization layer, instead of the aforementioned
VirtualBox. This brings Docker much closer to the Windows platform, but
still leverages a Linux-based virtualized host to create containers.

![](./media/image1.png)

Figure : Docker for Windows using Hyper-V to run a Linux container host

![](./media/image2.png)


Figure : Docker Toolbox shows a Windows client running a Linux-based
host

## Windows Server containers

Windows Server 2016 (Technology Preview 3 and onwards) was the first
Windows operating system that offered native Windows containers. Put
differently, Windows Server 2016 is capable of creating container
isolation as part of the operating system. Like before, this implies
that the containers are Windows-based and run directly in Windows,
sharing resources and processes from the Windows Server host.

Microsoft has also adopted Docker as their container interaction API. A
PowerShell module and the CLI are available, so you can choose your
preferred interactive terminal, and write PowerShell scripts or batch
files to automate simple Docker management.

![](./media/image3.png)


Figure : Windows Server containers shows a Windows client running a
Windows-based host

## Hyper-V containers

The Windows Server 2016 operating system has a unique feature with
regard to containers. It knows how to combine virtualization and
containers to provide an even higher level of isolation. Container
isolation is a good thing, but there is a potential risk of applications
breaking out of their container and accessing the host system or the
insides of another container. To mitigate this risk in a hostile
multi-tenant situation or when trying to regulate workloads, you might
require additional isolation. Windows offers the option to start a
Hyper-V virtualized host with a minimal Windows OS optimized to run
containers. These containers are called Hyper-V containers, in which a
high degree of isolation is achieved at the expense of a small
performance impact.

Two special version of Windows Server 2016 were created as an operating
system for Windows Server containers: Windows Server Core and Nano
Server. These trimmed-down versions of the full operating system discard
unnecessary features in order to provide a fast, lean and mean container
host basis.

Container images created for Windows Server container also run in
Hyper-V containers and vice versa. So, you can decide the level of
isolation as an afterthought instead of having to decide upfront. The
same does not hold true for container images used with Docker for
Windows and Windows Server containers. The reason for this is that the
former utilizes Linux based container images, where the images for
Windows Server are Windows based.

Recently, the Hyper-V container feature made its way to Windows 10
Professional and above, giving Windows containers to developers'
machines.

## Containers in Azure

The Azure platform of Microsoft brings many of the container options
together in a number of Infrastructure-as-a-Service (IaaS) offerings.
Obviously, with Windows Server 2016 virtual machines in Azure, you can
leverage both Windows Server and Hyper-V containers. Additionally,
virtual machines running a Linux distribution such as Debian or RedHat,
offer Linux-based containers. Both options are nicely integrated into
the Microsoft developer experience. Docker container hosts running in
Azure virtual machines can be accessed as if they are local on your
machine. Microsoft has provided Docker drivers to connect a local Docker
client to the Docker daemons and engines running in Azure-hosted
machines.

Azure also has a complete production scale container hosting offering in
Azure Container Services (ACS). Essentially, ACS provides a cluster of
virtual machines in a scale set for containers to be hosted on. ACS uses
Docker Swarm or DC/OS under its covers in order to provide a container
cluster management, monitoring and resource governance platform for the
set of virtual machines. When you create the ACS cluster, you choose
either Docker Swarm or DC/OS.

Docker Swarm combines multiple Docker container hosts into a virtual
single host for hosting containers. DC/OS builds upon many open source
based tools and frameworks, such as Apache Mesos and Marathon. It keeps
track of the available resources on all virtual machines in a cluster,
it can build and deploy Docker container images, and control and monitor
the running container instances.

![dcos-acs](./media/image4.png)


Figure : Azure ACS physical architecture provisioned for a DC/OS cluster

ACS takes care of installing and configuring typical small and large
deployments of Docker Swarm or DC/OS clusters for test and production
scenarios. It is available from the Azure Portal after a few clicks, and
it offers more functionality than simple IaaS, but it does require you
to keep and maintain the virtual machines in the cluster yourself. That
brings it rather close to the offering of a Platform-as-a-Service.

![](./media/image5.png)

Figure : Provisioning an ACS cluster from the Azure Portal

# Development with Visual Studio and containers

Because Microsoft has chosen Docker as its standard for containers, you
will encounter Docker tooling throughout the family of Visual Studio
products.

Visual Studio 2015 Update 3 ties into the developer workflow for
creating and hosting applications in containers after you have installed
the Docker Tools for Visual Studio. The tooling allows a developer to
easily deploy a .NET-based application to a Docker container. To do so,
it will build a Docker image that is uploaded to an existing or newly
created Docker container host which can be local or remote. Images are
built from a Docker file describing the alterations from a base image.
The tool Docker Compose facilitates in creating Docker images and
environments, and is the underlying mechanism leveraged by PowerShell
scripts.

![](./media/image6.png)


Visual Studio Team Services (VSTS) takes the Docker container workflow a
step further and provides continuous deployment and release
capabilities. The Docker Integration tooling will integrate Docker in
agile and DevOps workflows with easy and transparent management and
distribution of the Docker images it creates. The tooling can be found
in the VSTS Marketplace and, once installed, will add additional build
and release tasks and service endpoints. The service endpoints connect
to a Docker Registry and a Docker container host from VSTS. The 'Docker'
task allows you to build, push and run a Docker container image, or run
a Docker command. The other task 'Docker Compose' can use a
docker-compose.yml file to run Docker Compose-defined commands.

![](./media/image7.png)


The combination of these tasks allows you to set up a build and release
pipeline on your platform of choice. A pipeline that builds a Docker
image must run on a VSTS build agent hosted in the same operating system
as the one targeted by the container image. In other words: a
Linux-based container image must be run on a Linux-hosted VSTS build
agent, and likewise for Windows-based images and build agents.

# .NET Core and cross-platform containers

The two main .NET platforms of this moment are .NET 4.6.2 and .NET Core
1.0. The programming experience for both is practically the same with
corresponding .NET Frameworks and the languages C#, Visual Basic and F#.
The full .NET Framework can only be installed on Windows machines, as it
is limited by and dependent on certain Windows features. This implies
that applications that are built for .NET 4.5 can only be run or hosted
on Windows machine and Windows containers. Windows Server and Hyper-V
containers are an excellent choice for hosting .NET applications.

.NET Core 1.0 is a reimplementation of the .NET Framework and its
runtimes, and is designed to be light-weight, modular and most
importantly, cross-platform. Even though .NET Core 1.0 is not on par in
terms of features with the full .NET Framework 4.5+, it is a very
attractive option in your choice of application framework. It offers a
reasonably smooth transition of your application from Windows to Linux
or the other way around. Admittedly, .NET Core is also targeted for OSX,
but this is more geared towards developer station scenarios. Developers
can build and run their .NET Core applications natively on OSX, but will
probably need either Windows or Linux to host the application in a
production environment.

# Choosing your container strategy and platform

The abundance of choice makes it an all but trivial task to choose your
strategy in container technology and the hosting operating system within
the Microsoft platform. The following guidance might help in deciding.

Start by realizing that the overall interaction with containers,
regardless of their environment, is through the Docker tools CLI and
Docker Compose for both Windows and Linux. The choice of PowerShell is
more aligned with the Windows platform and is a good alternative if
Windows is a given and there is already an investment in PowerShell
scripts.

The choice for the operating system is largely determined by the
framework that the application (or subsystem) uses. The application can
be just about anything: a Web application, Web API, service or
command-line tool or otherwise. If these are created for a Linux-based
system, you must choose one of the Linux distributions as your container
host. For applications built on .NET 4.5 and higher you must choose
Windows Server or Hyper-V containers. If you want to develop for
multiple types of container hosts (Windows and Linux) then .NET Core
offers the flexibility required.

On your development machine it doesn't really matter what you choose and
it almost comes down to personal preference. You have most options when
you run Windows 10 and use Visual Studio 2015. Docker Toolbox requires
Hyper-V to be turned off and is the oldest tooling and might be limiting
in its reach. Docker for Windows and Hyper-V containers utilize Hyper-V
and are available side by side on Windows 10. If you are able to host
any virtual machines in Azure and have access to them during
development, you can choose whatever you like. Just remember that you
need a separate image for each operating system.

Looking towards hosting in a production environment you will need some
higher level governance of all containers and resources. Azure Container
Services is currently the only offering available in Azure and its
containers are always Linux-based. This narrows your options to
targeting your application to either (open source) frameworks for Linux,
Mono or .NET Core. Both targets will be able to be deployed as
Linux-based container images. .NET Core or Mono seems the most obvious
choice for a .NET developer.

At this point in time, Windows Server and Hyper-V containers are best
suited for .NET and Win32 applications and non-production scenarios.
However, as soon as the container monitoring and governance tooling sets
start supporting Windows-based containers, you will have to re-evaluate
your choice. The enterprise-grade stability and support for the Windows
Server platform is an important factor to take into account.

# Summary

The Microsoft platform has plenty to offer for creating and hosting
containers. Whether these containers need to run on Windows or Linux,
Microsoft can be your platform of choice. Microsoft Azure, Windows
Server 2016 and Windows 10 can host all types of containers. Azure also
offers Azure Container Services for production scenarios. Docker
tooling, the Visual Studio development environment and Visual Studio
Team Services allow developers to adopt agile and DevOps workflows,
resulting in continuous delivery and release pipelines for
container-hosted applications. In short: Microsoft is a one-stop shop
when it comes to creating and hosting container-based applications and
architectures.
