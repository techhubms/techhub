# How Docker will change Microsoft development 

Rene van Osnabrugge

<rvanosnabrugge@xpirit.com>

\@renevo

Within the developer community, Microsoft has always been known for
their great IDE, Visual Studio, and their development framework .NET.
Together with products such as SQL Server, SharePoint and BizTalk, this
was the basic set of tools of every Microsoft Developer. In addition to
the Microsoft toolset there were some additions such as HTML,
JavaScript, CSS or some other third-party frameworks and tools, but
Microsoft's portfolio constituted the main body of the tooling required
by developers.\
![](./media/image1.png)


Figure 1: The \"old\" Microsoft stack

The downside of this unified toolset and the tight coupling with Windows
caused more and more developers and companies to shift away from the
Microsoft platform and for various reasons. Companies did not want
vendor lock-ins or to rely solely on proprietary software. Moreover,
developers wanted more choice and control over the frameworks they used,
preferring to move to Open Source.

## Times are changing

But Microsoft has changed its strategy -- it is moving to a "mobile
first, cloud first" world and knows that there is more than Microsoft
alone. And this shift of strategy also impacts the Microsoft Developer.

Some major changes were made to overcome the difficulties faced by
companies and developers using the Microsoft platform, there were.
Microsoft does not only want to offer the best platform and IDE for
Microsoft developers, it also wants to offer the best platform for all
developers, regardless of the platform or technology being used. We now
see Microsoft adopting Open Source, providing development tools on every
Operating System (Visual Studio Code) and it is even open-sourcing its
own technology.

The .NET platform became Open Source and ASP.NET 5 was completely
rebuilt to be able to run on both Linux and Windows. Microsoft created a
lightweight version of the .NET runtime called the CoreCLR. This CoreCLR
is available on all the platforms and is much lighter than the
traditional CLR. And although it seems trivial because most of the
ASP.NET applications run on Windows this actually opens up a whole new
range of possibilities.

ASP.NET 5 is a powerful technology. However, the downside always was
that it was not Open Source. But now that it is Open Source, ASP.NET has
the power and curation of a large company like Microsoft but all the
benefits and community of Open Source, allowing the technology to evolve
even quicker.

With the CoreCLR, which is an important part of the ASP.NET stack, every
developer is able to build and run applications on all platforms.

## From mainframes to containers

With regard to application development and deployment, a lot of
attention is currently paid to Continuous Delivery and DevOps, i.e. the
ability to release an application on demand, and the ability to remove
all friction between the people building the application and the people
running it.

In a "mobile first, cloud first world", where computing power is
ubiquitous, availability and performance is the most important thing and
where complexity is increasing rapidly, we see that the industry is
ready for the next step. What is needed is higher density, less
downtime, faster start-up times and immutable software.

When we started out in IT, we used mainframes. Because mainframes were
not flexible, not accessible to everybody and were expensive to run,
physical servers became popular. After that Virtual Machines were
introduced, because physical servers did not scale very well, were hard
to clone or move, and also involved considerable operational cost. And
to take this a step further, the Virtual Machines moved towards a Cloud
Platform to further reduce costs and increase the scalability and
reliability.

And now it is time for the next step in computing: Containers. Virtual
Machines have a large footprint, are hard to maintain because they all
have their own Operating System and configuration and they are quite
heavy to run, allowing only a few Virtual Machines on one physical host.
To overcome this challenge, containers are the ideal solution. They
offer the benefits of a Virtual Machine but without the related overhead
and footprint.

So when Docker was introduced a few years ago, it built on the already
existing container technology in Linux, and containers really took
flight. Because Container Technology is the next step in computing and
because containers are currently only possible on Linux, it is not
surprising that Microsoft is working hard to support containers on the
Microsoft platform as well. And this is what will happen with Windows
Server 2016 and Windows Nano Server.

## Docker, Images and Containers

Before we discuss the possibilities that containers can offer the
Microsoft developer, let's take a look at what container technology is
actually all about. What makes it different to Virtual machines? It is
important to understand the concepts behind a container, especially
because a single container cannot run on both Windows and Linux. In
other words, container technology in itself does not offer OS
transparency.

![](./media/image2.png)


Figure 2: Virtual Machines

As illustrated in Figure 2, we can see how Virtual Machines work
conceptually. An Operating System is installed on the physical Hardware,
and by using a Hypervisor, Virtual Machines are created. They run on
virtualized hardware and are assigned resources by the Hypervisor. We
can run multiple Virtual Machines on one host and they are totally
independent of both the host and one another. One Virtual Machine can
run Windows 2012, one can run Windows 2003 and one can run Linux.
Applications are installed on the Virtual Machine. As far as the
application knows, it runs on physical hardware. This also applies to
the users and administrators.

This independence of the operating system within a Virtual Machine is
perhaps the largest difference between Virtual Machines and Containers.

![](./media/image3.png)


Figure 3: Containers

As illustrated in Figure 3, containers run directly on the Operating
System, without using a Hypervisor or virtualization layer. The
container uses the underlying Operating System (or actually Kernel) and
only saves the delta as part of the container. For example, actions such
as installing software, changing settings or creating files are all
stored as a new layer on top of the base container image, which is the
same as the underlying OS. This means that a container is much more
lightweight than a Virtual Machine because we don't have the footprint
of the full OS. This allows us to run more containers on one host and be
much faster in startup time. Moreover, we keep the benefits of
virtualization because the containers are also independent of each
other.\
\
\
![](./media/image4.png)


Figure 4: Containers are layers

As you can see in Figure 4, containers are layered. You start out with a
base image, which is the "transparent layer" on top of the underlying
Operating System. In case of Windows containers this is a Windows Server
Core image or a Nano server image. All actions you perform within the
container, for example installing Git or installing ASP.NET, are saved
in a separate layer on top of the base image. The container including
the layers can then be stored as a new image and be reused as a new base
image. This way you can create an ASP.NET 5 base image that contains all
necessary files, and use that image to run your application on. This
makes it super easy to reuse images.

You can describe an image in a Docker file. This file describes which
base image you should use and what additional actions you want to
perform on this image. A Docker file could look something like this.

![](./media/image5.png)


Figure 5: Example Docker file

In this example, we take the windowsservercore base image, add two
environment variables, run a PowerShell Command to install Git, add some
files to a folder, navigate to the folder and run the webserver kestrel.

This file describes the image of the container and can be stored in
source control, be stored in a public or private registry so it can be
distributed, and allows you to create new container images. You can
imagine the power of this approach.

## Hyper-V Containers

One of the small downsides of containers running on the same host (both
on Windows and Linux), is that they are not completely isolated. This
possibly involves two problems[^1]:

-   The kernel is shared between the containers. In a single tenant
    environment where applications can be trusted this is not a problem
    but in a multi-tenant environment a bad tenant may try to use the
    shared kernel to attack other containers.

-   There is a dependency on the host OS version and even patch level
    which may cause problems if a patch is deployed to the host which
    then breaks the application.

This is where Hyper-V containers may be the game changer for containers
on the Windows platform. With Hyper-V containers a mini Hyper-V VM is
spun up, which is completely isolated from other VM's and the host
kernel. Inside the mini VM, the container is still used, but without the
possible downsides. The only downside you have is that you have a
slightly larger footprint of the larger container (or VM) and thus a
lower density on the server.

On Windows it will be possible to run your container as a "normal"
container or a Hyper-V Container. You can choose this during deployment
time, making this a real advantage.

## The impact for the Microsoft Developer

Now that we know a bit more about the inner workings of a container, and
how it differs from a Virtual Machine, we can explore some new
possibilities that containers can provide to the developer community.

### A change in the developer experience

As a developer you use a lot of different tools, a lot of different
configurations and also a lot of different versions of tools. However,
managing all these configurations, or developing for a specific target
requires a lot of effort. What we usually do when we want to run
multiple versions of a tool in parallel is to install a Virtual Machine.
We can now run a VM quite easily in the cloud, but we can also use a
Virtual Machine on our own Hyper-V server. Regardless of the option we
choose, it requires a lot of resources.

With Docker containers, we can run lightweight containers, with their
own tools and configurations and versions, all on the same host. And we
can run many of them at the same time. But the best part is, we can also
throw them away and keep the configuration of our development/test
environment in a file instead of a large Virtual Hard Disk. So instead
of only having the code for a specific version of our application, we
also have the development environment. When we need it, we create a new
image and we run it in a matter of seconds.

Of course, we can also use containers to facilitate our dev/test
experience on the local workstation. Currently, Docker on Windows is in
its early steps of development and the developer tools are not yet
adjusted to conveniently work with containers. Besides the Docker
plugin[^2] in Visual Studio, which allows you to directly publish an
ASP.NET 5 website to a Docker container[^3] and the Yeoman generator[^4]
to scaffold a Dockerfile for your application, there are no tools
available yet. But imagine the power of a container that can be used as
a development tool. You build your code and publish it to a Docker
container and spin it up. Once running, you can debug your code within
the container. No caching trouble, or files that were still installed or
version-specific issues.

You can create images of containers that represent the state of the
production environment that you are developing for. And the best thing
is, the container definition becomes part of your source code because
you can store it along with your code in source control.

## A change in the Continuous Delivery process 

The most obvious change containers will bring, is a shift in how we
build, test and deploy our applications. One of the biggest advantages
of a container is that it is immutable. You create the image, which
describes everything that should be in the container and configure its
properties. The application is deployed within the container and
configured as well, and this immutable container moves through the
various stages of Test, Staging and Production.

This is a dream in a DevOps world where we do not want big documents and
configuration settings being handed off to someone who can do this on a
production machine.

When containers are used for moving an application though the various
stages, it also means that automation becomes even more important than
it already is. It means that the creation of the container image, the
building of the application and the configuration all need to be done
automatically. Build Servers, Release Management and Configuration as
Code become crucial.

Testing the application becomes easier as well. Instead of deploying to
one Test Environment, each version of an application can be spun up as
an extra Test Environment, allowing testers to have fine-grained control
over the bits they are testing.

The following figure gives an idea of the structure of a typical
delivery pipeline:

![](./media/image6.png)


Figure 6: Continuous Delivery workflow

The figure shows that a developer pushes code and a Dockerfile to the
source code repository. The build server picks up the latest version and
builds a container image from the source code and Dockerfile. The
container contains the application and configuration, and is sent to the
Docker repository (DockerHub) or a private Docker repository. Such a
repository keeps a copy of Docker images that you can pick up and reuse.
A typical example could be the creation of a base container image that
contains all the prerequisites, and then adding the compiled sources to
this container. From here the container images can be pulled by
developers but also by Visual Studio Release Management (or a build
server etc.). VS Release Management installs (or spins up) the
containers in the Test, Staging and Production environments.

## A change in application architectures

When you run lightweight containers that are immutable and disposable,
it makes no sense to run a big monolith inside the containers. The
results will become visible in the future, but the fact is that using
containers and using a loosely coupled, microservices-like architecture
are related to each other. We could start using containers for our
microservice architecture, or perhaps we will move to microservices
because we want to use containers.

Whatever the results may be, containers will play a major role in how we
architect and build applications. Because when we have a disposable
resource that we can recreate by using a file and spin up new instances
in a matter of seconds, it makes no sense to architect our application
for updates. We always create a new instance, and we can use this to our
advantage. We can create a new version of a container and spin it up
next to an old one. Services that use the old container can start
pointing at the new one and the old ones can be shut down. If needed, we
just spin up a new instance of the container.

For example, take the following situation: imagine that you have a tax
office and need to calculate tax. Every year the application is changed:
new rules, new data. The calculation takes a fair amount of time, so
incoming requests are stored in a queue.

In a traditional situation you would probably have a Load Balancer and
distribute the requests to multiple servers. When using containers, you
can use a completely different approach. Instead of one application that
is updated from time to time, the processor that takes the top of the
queue, gets the latest version of a container and spins it up. The
container processes the request and is then thrown away.

If a newer version is released, the newer version is used instead. No
downtime and no traditional problems of an application that takes a long
time to run.

## Summary

Containers have been around for quite a while on the Linux platform and
now they are coming to the Windows platform. A container is really
different from a Virtual Machine, because it is small, fast and
immutable. When it comes to developing software, containers will change
the way we work. Developer workflows will be optimized for building and
testing in containers, the delivery workflow will be optimized for
delivering containers, and the entire application architecture will be
modified to be able to deal with that.

Extra Links

Channel 9 Videos -- <http://aka.ms/dockerfordotnet>

Getting started -- <http://aka.ms/windowscontainer>

[^1]: <http://windowsitpro.com/windows-server-2016/differences-between-windows-containers-and-hyper-v-containers-windows-server-201>

[^2]: <http://aka.ms/dockertoolsdocs>

[^3]: <https://visualstudiogallery.msdn.microsoft.com/0f5b2caa-ea00-41c8-b8a2-058c7da0b3e4>

[^4]: <http://aka.ms/yodocker>
