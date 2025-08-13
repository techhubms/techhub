# What you should know about Windows containers before you start using them

A lot of articles and blogs have already been written about the benefits
of using containers for your application delivery[^1], also called
containerized delivery. While the serverless trend is considered the
containers' successor in application delivery, it still is not the best
fit for all situations. For example, containerized delivery is a perfect
fit for organizations that do not want to go into the cloud (for now) or
that have to deal with their existing application landscape. In this
article, we'll look at the most important things you as an
architect/lead developer have to know about Windows containers before
you start using them. If you are interested in more practical manuals
about configuring a containerized pipeline, setting up a container
registry and other tips and tricks, please have a look at our blog
posts[^2].

## Containers as the next step in application delivery

Containerized delivery can be seen as the next step in application
delivery for many organizations. If we look at the way in which we have
been delivering our software during the past 10 years, we see a shift
from manually installing our applications towards an automated way of
pushing the applications into production. Driven by the mindset that the
reliability, speed and efficiency of delivering our applications to our
customers could be improved, various tools and frameworks have been
introduced over time, e.g. WiX, PowerShell, PowerShell DSC, Chocolatey,
Chef, and Puppet.

![](./media/image1.png)

Figure 1 - Deployment maturity anno 2017

In the earlier years, we baked an MSI by using WiX and the installation
of our applications was performed manually by one of our colleagues.
Because this way of working was error-prone and it took us a lot of time
to find and fix mistakes, we decided to make use of new scripting
languages like PowerShell to fully automate the installation of our
applications. Although this scripted way of installing our applications
brought us more reliability and speed, still some deployments went wrong
over different environments (DTAP) because of missing libraries,
configurations and tool versions.

So, we decided to make use of Desired State technologies like PowerShell
DSC, Chocolatey, Chef, and Puppet to ensure that our environments will
end up in the correct state. We succeeded in creating an automated and
reliable way of delivering our applications to production, however we
still have to reboot machines after de-installations to ensure that all
registry keys, caches and file system files are really cleared.
Moreover, we experience a lot of waiting- and downtimes caused by
(de-)installations and are often not able to run multiple versions of a
given application on the same host. That's a pity because we see a lot
of VM resources not being consumed by our various applications. Sounds
familiar? Deploying your applications in containers can help to solve
these issues.

## The origin and benefits of containers

Containers are the resulting artifacts of a new level of virtualization
that is implemented on Windows. Looking at the history of
virtualization, it started with concepts like virtual memory and virtual
machines. Containers are the next level of this virtualization trend.
Where VMs are a result of hardware virtualization, containers are the
result of OS virtualization. Where hardware virtualization lets the VM
believe that its hardware resources are dedicated to that instance, OS
virtualization lets the container believe that the OS instance is
dedicated to that container, although it shares the OS with other
containers. The host machine on which different containers can run is
called a container host.

Working with containers for your application delivery will give you some
benefits as mentioned in the article of Alex Thissen. Instead of
installing the application during deployment, containerized delivery
will install the application during build and will create a container
image out of it. Once you ship your containers to new environments
you'll get instant startup times. The image format that is used does not
only contain the running application, but also contains a snapshot of
the context around the application such as the file system, registry and
running processes. This mechanism ensures that our application will run
in the same way in different environments. Another benefit is that the
container technology helps us to utilize more of the machine resources
because it makes it possible to run multiple versions of the same
application/conflicting applications on a single host.

## How container isolation works

While containers are not VMs, both artifacts support resource governance
and an isolated environment[^3]. Similar to VMs, each container has an
isolated view on its running processes, environment variables, registry
and file system. This means that it is possible to change a file within
one container while this change is not detected by another container.
This isolation characteristic is important to notice. By default, it is
not possible to share any files between different containers. Even the
container host cannot see the file system changes that are made within
the container. The same goes for the registry and environment variables
isolation.

The process isolation part is slightly different. While Windows Server
Containers cannot see each other's running processes, it is possible to
see the running processes of a given Windows Server Containers from the
container host. While this is also the case for Linux containers, this
is not an appropriate option for multi-tenancy situations where the
container host or different containers are not in the same trust
boundary. Because of this security implication, Microsoft decided to
introduce an extra container type called the Hyper-V container[^4].
While it is possible to see the running processes from within the
container host for Windows Server Containers, Hyper-V containers run a
normal Windows Server Container within a minimalized (utility) Hyper-V
"VM". Because of this extra Hyper-V virtualization layer around the
normal Windows Server Container it is not possible to see any content of
this container type from the container host. Actually, the Hyper-V
container is a hybrid model between a VM and a container.

## How this impacts the way we share data between our applications

By default, containers are not designed to share any resources. To be
able to share data across your containerized applications, you need to
store data in remote data storage solutions such as Redis or SQL.
However, in some scenarios these solutions are not appropriate because
of an existing application architecture you have to deal with. For
example, when you have a windows service generating PDFs from files on a
given file system location and a website that is putting those files
onto this location. How to deal with this inter-container data sharing?
We can certainly extend our applications to communicate over HTTP/TCP to
exchange important information, but luckily there are also other options
available.

**Data Volumes**

One option is that you make use of Data Volumes. Data Volumes are
artifacts that differ from containers. They have their own lifecycle and
you have to manage them with separate commands. For example, when you
delete a container, the Data Volume will still exist. Once you have made
a Data Volume, you can map it as a directory on your containers. Within
the container these mapped directories will bypass the normal Union File
System implementation and by doing so, all changes in this directory are
always persisted on the file system of the container host. Moreover,
changes that are made within this volume are directly available for
other containers that consume the same Data Volume. An example script of
consuming Data Volumes in your containers can be found below.

![](./media/image2.png)


Figure 3 - Data Volume concept and example script

**\
**

**Data Volume Containers**

Another option is that you make use of Data Volume containers. Sometimes
you need to map multiple Data Volumes for most of the containers you run
(for example a logging and data directory). For this purpose, you can
make use of Data Volume Containers.

Data Volumes Containers are just containers that you've created and
mapped on different Data Volumes. Where normally you must specify the
separate volume mappings for each container you run, in the case of Data
Volume Containers you just have to add the "\--volumes-from" switch to
the Docker run command. For each Data Volume that is mapped within the
Data Volume Container, Docker will create a new volume mapping on the
same file system directories within your consuming containers. While you
may expect that deletion of the Volume Container will result in deletion
of the Volumes, this is not the case. It just works as normal Data
Volumes, except that we have an extra option to easily manage
consistency of multiple Volume mappings over different containers.

**Volume Plugins**

By default, Data Volumes are stored on a single container host. But what
if I'm running my containers on different container hosts in a cluster
and still want to share the data from my Data Volumes on different
container hosts? Or what happens with my volume data when my single
container host crashes? It's exactly for this reason that Volume
Plugins[^5] were introduced. Volume Plugins enable you to make Data
Volumes hosts independent by integrating their storage with external
storage systems such as Amazon EBS. For each Data Volume you can specify
whether it should use a different Volume Plugin implementation by
specifying the "\--driver" switch on volume creation.

## Learn about the concepts: images and image layers

When you start building your own containers, you must define what your
container will look like. You can achieve this by specifying the various
instructions[^6] that have to be executed during the build process to
define/bake your container. One instruction could be that you copy some
files from the build machine to a given location within the container.
When using Docker, this file is called a Dockerfile.

During the Docker build process each individual instruction is executed
and its resulting file system changes are persisted in an image layer as
shown in *Figure 2 - Image layers, Images and Containers*. This image
layer will be stored locally on the build server. The reason for this is
that we can have multiple containers that share the same base set of
stacked image layers. Instead of rebuilding all these layers again, the
Docker build process will look for existing image layers in this local
store before it starts building a new layer.

The order in which you specify the instructions makes sense for the
image layers that are generated, for example, the Dockerfiles below will
create different image layers while they have the same set of
instructions. The concept of having a stack of image layers where each
image layer represents a set of file system changes is also called a
Union File System[^7].

![](./media/image3.jpg)


Figure 2 - Image layers, Images and Containers (source:
<https://docs.docker.com/engine/userguide/storagedriver/imagesandcontainers/>)

A container image is delivered at the end of the Docker build process.
This image consists of a stacked set of image layers and is the
blueprint for all containers you will run on the basis of this image.
The difference between a container image and image layers, is that a
container image "tags" a given image layer with a user-friendly name.
All image layers that are under this tagged layer will be part of the
container image. A container image is not a living thing and does not
have a state. It is no more than a sealed artifact from which we can
create multiple runtime instances: the containers.

To keep track of changes that are made within the container from the
moment it was created, each container gets an extra Read/Write layer on
top of the stacked image layers of the original container image. Where
we might expect a totally different implementation between containers
and container images, the only real difference between both objects is
just this extra thin R/W layer for each container instance! During the
lifecycle of the container, the changes that have been made within the
container (actually this R/W layer) are stored temporarily on the
container host. Once you delete the container, all those changes are
lost. You can prevent this by using the Docker commit command to persist
the changes that are part of this thin R/W layer into a separate image
layer in the local store. From that moment on you can tag a new image
based on this newly created image layer and you can initiate multiple
containers based on this image.

***[Dealing with sensitive information -- short peek]{.underline}***

*Containerized delivery will force you to deal with sensitive
information in a different way. Once you create a container image out of
a running container or from scratch, others can see [all]{.underline} of
its content just by running a container from that image locally. They
can even make use of docker history to see the commands that have run on
the container or they can look at the content of the versioned
Dockerfile. Hence it is important to be sure that no sensitive
information is stored within the container image or Dockerfile. But how
to ensure this? One way of working is that you make use of the Docker
Secrets Manager*[^8] *to manage all secrets. Follow our blogs to see how
to make this work and what other options are available here.*

## How these new concepts change the way you'll look at maintaining your applications

Containers are designed as stateless artifacts. You should avoid storing
data in your container as much as possible. As mentioned earlier,
containers make use of a Read/Write layer to keep track of any changes
since the initialization of the container. Once a container is deleted,
these changes are lost. The objective of this implementation is to force
users to persist all important changes in container images. By doing
this, we can always reproduce a given state and are always able to
selectively scale different parts of our system.

Another important characteristic of containers is that they are intended
to be immutable. Thanks to the isolation part of containers we don't
need to upgrade running applications because we can run different
versions of the same container side by side on a single container host.
Moreover, because containers also contain the context around the
applications, we can be sure that initializing a container on different
container hosts will ensure that our application will run in exactly the
same way on those different container hosts. Once we accept the
stateless character of containers and create container images for all
different versions of our applications, there is no longer any need for
in-place upgrades. Instead of treating our applications as pets, we can
now treat our applications as a flock of applications. If one container
gets ill we will not nurse it back to health, but we will remove it and
just initiate a new container based on the same genealogical register
(the container image).

## Reaching Nirvana: Environment-as-Code

Many of us know about Infrastructure-as-Code[^9]. The concept of
scripting the creation of the infrastructure you need. Containerization
will add a layer on top of this Infrastructure-as-Code layer, and this
is called Environment-as-Code.

Once we have created the necessary container infrastructure like the
Container Registry, a cluster of Container Hosts and the VSTS pipeline,
it is time to deliver our applications via, and on top of, this
infrastructure. Normally we would perform a deployment of our
applications individually, and this is still possible within
containerized delivery. However, wouldn't it be better from an
immutability and quality (repeatability) perspective to always deliver
the entire application stack as one? To think about complete
environments as deployable items instead of single applications?

![](./media/image4.png)


Figure 6 - Environment-as-code

This is what we call Environment-as-Code. Not only are your containers
immutable and stateless, your complete environment (except your data)
should be immutable and stateless as well. And in the same way as we
version individual container images, we should also version environment
templates. Luckily this is possible with containerized delivery by
making use of Docker Compose[^10]. Within the compose file you specify
the containers (services section), volumes and network related details
that are part of your environment. By running the *docker-compose up*
command on your docker-compose.yml file, with some environment specific
variables specified in an environment[^11] file, the Docker engine will
eventually create the elements that you have specified in your Docker
compose file (including Docker networking).

Applying Environment-as-Code in practice means that you have to adapt
your delivery pipeline based on this concept. All
applications/containers will still have a separate commit stage in which
compilation, unit testing and acceptance testing on code units takes
place. After this commit stage and after running the necessary
acceptance tests on our individual containers, you will change the
version of the container you just built in the Docker-compose file.
Instead of just having a single delivery pipeline for each container,
you are also able to deliver the complete environment via a combined
delivery pipeline to production.

![](./media/image5.png)


Figure 8 - Combined delivery pipeline

## Where to find more information

We have just looked at a number of important concepts and principles
that you need to know about before you start performing containerized
delivery on the Microsoft stack. There is a lot more to be told about
containers and containerized delivery, so please follow our blog posts
on <https://xpirit.com/blogs> or on my personal blog on
<http://www.solidalm.com> to remain up-to-date about working with
containers on Windows. A good starting point to learn the necessary
commands and tools of working with Docker can be found on
<https://docs.docker.com/engine/getstarted/>. Brighten your life, and
start working with containers today!

[^1]: <http://blog.xebia.com/tag/containers/>, <https://t.co/XrfIWX9buY>

[^2]: <https://xpirit.com/blogs>

[^3]: <http://www.solidalm.com/2017/03/06/deep-dive-into-windows-server-containers-and-docker-part-2-underlying-implementation-of-windows-server-containers/>

[^4]: <https://docs.microsoft.com/en-us/virtualization/windowscontainers/manage-containers/hyperv-container>

[^5]: <https://docs.docker.com/engine/extend/plugins_volume/#section-3>

[^6]: <https://docs.docker.com/engine/reference/builder/>

[^7]: <https://docs.docker.com/engine/reference/glossary/#union-file-system>

[^8]: <https://blog.docker.com/2017/02/docker-secrets-management/>

[^9]: <http://blog.xebia.com/infrastructure-as-code-and-vsts/>

[^10]: <https://docs.docker.com/compose/overview/>

[^11]: <https://docs.docker.com/compose/env-file/>
