# Containerized delivery for .NET workloads on Windows

One of the latest big trends in Continuous Delivery is to use containers
to speed up value delivery. Containers enable you to run your
application in an isolated environment that can be moved between
different machines while you are guaranteed the same behavior.
Containers can thus significantly speed up your delivery pipeline,
enabling you to deliver features to your end-users in a much faster
way.\
Now that containers have become part of the Windows Operating System,
how can you leverage this to run your existing Windows based .NET
workloads like ASP.NET without too many modifications? How can you
leverage container innovations that are already available in the open
source and Linux ecosystem? Innovations that enable you to simplify
on-demand scaling, fault tolerance and zero downtime deployments of new
features.\
In this article, I will give you a glimpse into how you can deploy
existing Windows based .NET workloads with Visual Studio Team Services
to Azure Container Services, using Kubernetes as the cluster
orchestrator.

## Why Containers?

### Isolation

### Containers are an isolated, resource controlled, and portable operating environment. They provide a place where an application can run without affecting the rest of the system and without the system affecting the application. If you were inside a container, it would look very much like you were inside a freshly installed physical computer or a virtual machine. 

When you create a container, its external dependencies are packed within
the container image. Containers have a layer of protection between host
and container, and between containerized processes. Containers share the
kernel of the host OS. A container relies on the host OS for virtualized
access to CPU, memory, network and registry.

### Immutability

Containers provide the capability of immutability. When you start a
container based on a container image, you can make changes to the
running environment, but the moment you stop the container and start a
new instance of the container, all changes have been discarded. If you
want to capture the state change, then you can save the state to a new
container image after you stopped a container. You will see the changed
state when you create a new container instance based on the new image.

### Less Resource-intensive

Running a container compared to running a virtual machine requires very
few resources. This is due to the fact that the operating system is
shared. When you start a Virtual Machine, then you boot a whole new
operating system on top of the running operating system and you only
share the hardware. With containers you share the memory, the disk and
the CPU. This means that the overhead of starting a container is very
low, while it also provides great isolation.

### Fast Startup Time

Since running a container requires only few extra resources from the
operating system, the startup-time of a container is very fast. The
speed of starting a container is comparable to starting a new process.
The only extra things the OS needs to set up is the isolation of the
process so it thinks it runs on its own on the machine. This isolation
is done at the kernel level and is very fast.

### Improve Server Density

When you own hardware, you want to utilize this hardware as effectively
as possible. With virtual machines we made a first step in this
direction by sharing the hardware between multiple virtual machines.
Containers take this another step further and provide us with an even
better utilization level of the memory, disk, and CPU of the hardware
available. Since we only consume the memory and CPU we need, we make
better use of these resources. This means less idle time of running
servers, and hence better utilization of the computing resources. This
is very important, especially for cloud providers. The higher server
density (the number of things you can do with the hardware you have),
the more cost-efficient the data-center runs. So it is not strange that
containers are now receiving a lot of attention, and that a lot of new
tooling is being built for managing and maintaining containerized
solutions.

## Why use Container Clusters?

When you want to run your application in production, you want to ensure
that your customers can keep using your services with as few outages as
possible. This is why you need to build an infrastructure that supports
concepts such as:

-   Automatic recovery after an application crash

-   Fault tolerance

-   Zero downtime deployments

-   Resource management across machines

-   Failover

Besides this you want to manage all of it in a simple way. This is where
container clusters come into the picture. The mainstream clusters that
are available today are Docker Swarm, DC/OS and Kubernetes. In this
article, I will show how to use Kubernetes. Docker Swarm is not really a
production grade solution, and it seems that Docker is more focused on
their Docker data center solution. DC/OS and Kubernetes are the most
frequently used clusters in production, and Kubernetes already supports
windows agents. DC/OS will also follow soon.

## How to create a cluster in Azure

The simplest way to create a container cluster is by using any of the
public cloud providers. They all offer clusters that enable you to run
your application with a few clicks. Google Cloud Engine primarily
provides a cluster based on Kubernetes. Amazon uses DC/OS as default.
Azure allows you to select which cluster orchestration solution you want
to use when you create a cluster. We will have a closer look at how you
can use Azure.

### Portal, Command line, or ACS engine

In the portal, you can search for Azure Container Services (ACS) and you
will find the option to create a cluster. You have to define the number
of Master Nodes and Agents, and the Agent Operating system you want to
use. Azure supports clusters and Windows agent nodes based on Docker
Swarm and Kubernetes.. This enables you to deploy your ASP.NET MVC
application on Windows containers in a cluster.

From a Continuous delivery perspective, we always prefer to enable the
creation of the infrastructure as part of the delivery pipeline.
Creating it via the command line is, therefore, the preferred way of
doing this, since it provides us with the ability to repeat the steps we
have taken, and check it in as a provisioning script for future use for
setting up a new environment.

Azure has a new command line interface 2.0 that supports the creation of
ACS clusters.

The following command can be used to create a cluster:

**az acs create \--orchestrator-type=kubernetes \--resource-group
myresourcegroup \--name= my-acs-cluster-name \--dns-prefix=
some-unique-value**

When you create a cluster, you will have a setup that looks as follows:

![](./media/image1.png)


### Master nodes

When you look at the cluster that will be created, you will find that
the Master Nodes are Linux-based virtual machines. The masters are
responsible for managing the cluster and scheduling the containers based
on the resource constraints you give to the deployment definitions. When
you define a deployment, you send commands to the master, which in its
turn will schedule the containers to be run on the agents. The way you
communicate with the master is through a command line tool called
**kubectl**. This command line tool issues the commands against the API
server running on the master nodes. The master nodes run a set of
containers that support the cluster, for instance the cluster DNS
service and the scheduler engine.

### Agent nodes

The agents run the kubelet containers that manage the agent
communication and interactions with the master. The master communicates
with the agents via the local network, which is not exposed outside the
cluster. If you want to expose a service (one or more containers) to the
outside world, you can do this with a simple **kubectl** command,
***kubectl expose deployment \<name of deployment\> \--port=port#***,
which in its turn will expose the deployment to the outside world via
the Azure load balancer. The cluster will manage the configuration and
creation of the required load balancer(s), the allocation of public IP
addresses, and the configuration of the load balancing rules.

### Deployments, Pods and Services

In a deployment, you describe a combination of the Docker images that
you want to run in your cluster. This combination of images, including
the shared storage options and run options, defines a Pod. A pod is the
implicitly defined logical unit of container instance management. When
container instances are created for the images in a pod, they will
always run together on the same node. Let's say you have an application
that consists of two parts: a web API and a local cache that are only
effective when they run on the same node. To accomplish this, you can
define a deployment template that includes the two images from the
command line or in a yaml file. The deployment also specifies how many
instances of pods will be started. By default, this is a single instance
of a pod. For fault tolerance and scaling it is possible to increase the
so-called replica count of your deployment to start multiple copies of
the pods. The moment you start the deployment, the cluster is
responsible for deploying the pods to the various nodes and for
balancing the resources in the cluster.

Every container instance created in a pod will request an IP address
from the DHCP Server in the local cluster network. Additionally, each
pod will get an internal DNS record based on the deployment name. The
master node acts as DHCP and as DNS Server. Container instances become
reachable from anywhere in the cluster, based on their DNS name and
exposed ports. When you expose a deployment, the cluster will connect
the container instances in pods to the external load balancer, so they
can also be reached from outside the cluster.

Kubernetes uses the notion of a Service as the abstraction that defines
the logical set of Pods and the policies to expose the endpoints we
need. This is sometimes referred to as the micro-service. You can list
which endpoints in your cluster are exposed to the outside world by
running the command:

**Kubectl get services**

This will show the list of services and the endpoint details, for
instance the ip address and the port on which the workloads can be
reached.

## Zero downtime replacement of deployments

When you run applications at internet scale, you want to be able to
deploy new features to the end-users without any downtime of the
application. Kubernetes cluster makes this possible with the concept of
a rolling update. Rolling updates enable you to update your container
images in your container registry and then give a command to update the
container images of the running container instances. This can be done
with a single command-line: **kubectl set image deployment
\<nameofdeployment\> \<nameofimage\>=\<reponame/newimagename\>**

In this command, you can specify precisely which image you want to
change, since a deployment can contain multiple images.

The steps taken are as follows:

-   Spin up new pods on the various nodes.

-   Drain traffic to the old pods

-   Gate traffic to the new pods

In these steps the cluster ensures that we always keep the minimum set
of pods up and running, so we can guarantee that we can keep handling
traffic while the deployment is in transit. The minimum set of replicas
that always need to be up and running, can be specified in the
deployment when they are created.

## Deployments using Visual Studio Team Services (VSTS)

To ensure a robust, repeatable and reliable way of deploying your
application to the cluster, you can use the build and release
capabilities of VSTS. When you deploy a new feature to your application
in the cluster, you will go through two primary phases. Phase 1 consists
of building, testing, and publishing the container image. Phase 2
consists of running the new image in the various test environments and
finally deploying it to the cluster, using the zero-downtime deployment
capability.

### Phase 1, build the container image

In phase 1 you use the VSTS build infrastructure. To do so, you simply
build your container based on a docker file that you check into source
control. In the build you define a step where you build the image using
the docker file that picks up the build artifacts from your application.
(dll's, configuration, web content, etc). The following screenshot shows
an example:

### ![](./media/image2.png) []{.mark}

### 

### Phase 2, test and deploy to production

In this phase you use the Release management part of VSTS, which uses
the same agent infrastructure as the build. You can define a set of
environments where you first validate the new feature(s). The moment you
have gathered enough evidence and confidence that the new application
runs as expected, you can then move to the deployment environment and
deploy to production. In VSTS you can specify tasks that need to be run
in each environment. Below you can see the series of steps you can use
to test the image you just created in the first phase, by running Docker
tasks that start the container. Next, you will see a task that tests the
running container and the final step is to stop the container.

![C:\\Users\\marcelv\\AppData\\Local\\Microsoft\\Windows\\INetCache\\Content.Word\\release -
Verify.png](./media/image3.png)


Deploying to production is performed in the production environment with
a set of tasks. These tasks execute the command line tool kubectl as
described above. In order to make this work, you need to install the
kubectl binaries on the agent machine and add location to the %path%
system environment variable. From that moment on, you can issue any
kubectl command to the cluster to create or update deployments. The flow
from deployment to production is shown in the following screenshot:

![C:\\Users\\marcelv\\AppData\\Local\\Microsoft\\Windows\\INetCache\\Content.Word\\release -
deploy.png](./media/image4.png)
 []{.mark}

## Improving the speed of value delivery

In this article I have introduced you to the concept of containerized
delivery using containers and Azure ACS. It is now possible to run your
existing ASP.NET applications in containers because Microsoft has added
containers and Docker support to their operating systems Windows Server
2016 and Windows 10. The open source cluster orchestrators like
Kubernetes, DC/OS and Docker Swarm also enable support for Windows
containers, and now they also unlock containerized delivery in the
Windows ecosystem. Because containers provide a mechanism to very easily
move them around within different environments while guaranteeing
exactly the same behavior, it now becomes much simpler and faster to
deploy to both your test and production environment. Add to this the
flexibility of scaling, fault tolerance, and zero downtime deployments
with clusters, and you can really improve the speed of feature delivery
to your customers.

## Conclusion

Containerized delivery is now also possible for your existing workloads
on Windows such as ASP.NET. You can utilize Azure ACS clusters with a
cluster orchestrator like Kubernetes to manage your workloads with much
more ease of deployment than in the past. Containerized delivery allows
you to simplify the build, test, and deployment pipelines and to
significantly improve your delivery cycle time.
