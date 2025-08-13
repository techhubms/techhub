*You have heard about containers, and you realize the benefits of using
them. But how do you get started? Sure, you can use Azure Container
Services, but that has a number of downsides. For example, it deploys
Virtual Machines, machines that you need to manage yourself. Wouldn't it
be nice if you could host your containers in the Cloud, at scale,
without having to worry about the underlying infrastructure? With Azure
Container Instances, you can.*

# What are Azure Container Instances?

Azure Container Instances (or ACI) consist of a PaaS service that was
recently added to Azure. It offers the capability to run both Linux and
Windows containers. By default, it runs single containers, which means
that individual containers are isolated from each other, and cannot
interact with each other. The isolation between individual containers is
achieved using Hyper-V containers. This gives you the same level of
protection as using a Virtual Machine would, by running your container
inside a small utility VM. You specify the amount of memory in gigabytes
for each container, as well as the count of CPUs to assign. Furthermore,
containers are billed per second. This means you are in complete
control, and that you don't pay for resources you don't need.

By assigning a public IP address to your container, you make it
accessible from the outside world. If you look at Figure 1, you can see
a container that exposes port 80. The port is connected to a public IP
address that accepts traffic at port 80 of the virtual host. Note that
in ACI, port mappings are not available, so the same port number must be
used by both container and container host.

*Note: Container Instances currently expect containers that are always
active. Task-oriented containers are not supported, as they will be
continuously restarted automatically after they exit. Simply running cmd
or bash on its own is not a long running process. Running a process such
as ASP.NET Core, a Windows Service or Linux daemon does work.*

## ![](./media/image1.png)No more VM management

It is important to know that in ACI you don't need to own a Virtual
Machine to run your containers. This means that you don't need to worry
about creating, managing and scaling them. In Figure 1, both the network
and the Virtual Machine - the container host - are completely managed
for you, and you have no control over either of them.

*Note: Running one container in ACI for a full month, with 1 CPU and
3.5GB of memory, costs about 30% more than running the same workload on
a DS1 Virtual Machine running Linux.*

## Running a container

The quickest way of creating a container in ACI is by using the azure
command line interface (CLI 2.0):

With the 'create' -command, you must specify the resource group to
deploy into, the name of the container, and the image to base the
container on. Optional parameters include credentials to your private
registry, environment variables that can be used to inject
configuration, the type of IP address to assign -- public or private --
and the port to expose.

Other important commands are:

-   'logs' - this is used to show the logs (console output) of a running
    container.

-   'show' - this is useful for debugging. This command displays the
    Resource Manager configuration container group, including a list of
    'events' that contain logging information from ACI. If your
    container fails during deployment, this will show up here.

-   'delete' - this is used to delete an existing deployment.

*Note: The output of 'az container show' command also includes
configured environment variables in plain text. If they contain secrets,
e.g., connection strings, they will be visible here. A safer way to
inject secrets is by mounting a volume that contains the secret. The
volume storage account key is not displayed.*

![](./media/image3.png)
Now, if you open the Azure Portal and
navigate to the resource group you specified, you will see your
container running inside a container group.

## Container Groups

By default, your containers are isolated from each other. But what if
you need to have interaction between containers? To support this kind of
scenario there is the concept of 'container groups'. Containers inside a
container group are deployed on the same machine, and they use the same
network. They also share their lifecycle: all containers in the group
are started and stopped together.

*Note: At this time, container groups cannot be updated. To change an
existing group, you need to delete and recreate it.*

*Note: Containers are always part of a container group. Even if you
deploy a single container, it will be placed into a new group
automatically. When using Windows containers, a group can have only one
container, this is because network namespaces are not available on
Windows.*

In Figure 2, you can see the anatomy of a container group. Here, three
containers are working together to form a fully functional application:

1.  The 'Job Generator' container runs a Web Server at port **80** that
    receives traffic from the outside world and queues jobs to an Azure
    Service Bus queue.

2.  The 'Job processing' container reads jobs from the queue and
    processes them. This container runs a Web Server at port **8000**.

3.  Finally, the 'Logging' container is used by the other containers to
    persist logging information to Azure Files. It runs a Web Server at
    port **8080**.

### Running a container group

A second way to create a container group is by using an ARM template.
Figure 3 shows a simplified version of a template. In the array named
'containers', multiple containers can be described. You can specify the
same options you saw earlier when using the command line. What is
different, is that by using this approach, you can define multiple
containers at once.

### State

Any state you might store inside your container will be lost as soon as
the container is removed. If you need to keep that state, you must store
it outside the container. For example, if you generate files, you should
store them on a file share. Fortunately, mounting an Azure File Share
from ACI is very simple.

Using an Azure File Share in your container requires that you declare it
in the volumes element, as you can see in Figure 4. To mount the volume,
the container configuration element volumeMounts references the volume
by name. The value mountPath specifies the path at which the network
share should become accessible. You can see this in Figure 3*.*

### More about networking

To be accessible by the other two containers, the Logging container
exposes port 8080. This port will be accessible within the entire
container group, but not from the outside world and not from other
container groups either. Containers within a group can communicate with
each other by using 'localhost' and the exposed container port.

For example, calling the logging service API can be done by using the
url: <http://localhost:8080/api/logs>. The platform will route traffic
to the container.

*Note: Containers in a group are **not** discoverable through DNS. They
can only be accessed through 'localhost', in combination with their
exposed ports.*

The Web server container exposes port 80. This port number matches the
port specified to expose at the public IP address (ipAddress) of the
container host. You can see this in Figure 4. The result of this
configuration is that the two ports are connected. This makes the
container accessible both from within the container group as well as
from the outside world.

All containers in a group share one public IP address because they run
on the same host. They also share a network namespace, so port mapping
and port sharing is not possible. This means you cannot run multiple
instances of the same container exposing the same port in one group. So
if you require interactive containers to be load-balanced, or if you
need to support upgrades without downtime, you'll need multiple
container groups working together.

### Seamless upgrades & Load balancing

![](./media/image5.png)
The ACI platform will monitor both your
containers and the container host. If there is a failure in either, ACI
will attempt to correct this automatically by restarting the containers.
At this time, ACI does not monitor your Container Registry for any image
updates. This means that you'll need to upgrade deployments yourself.
One way to do this is by running a container that monitors the container
registry and triggers redeployment whenever a new image version is
available.

For uninterrupted availability during upgrades and failures, you'll need
to run multiple container groups and manage the network traffic flowing
into them. This way you can replace one group, while others remain
operational. You can see an example of this in Figure 5. In this
example, Azure Traffic Manager is used as a router that directs incoming
traffic to the two container groups.

To update one group, you would need to take the following steps:

1.  Create new container groups that run the new version of the product,
    and add them in traffic manager.

2.  Remove the old container groups from traffic manager.

3.  After the DNS TTL has expired, take the old container group down.

Doing this manually, and perhaps even multiple times a day, is
error-prone and frankly, it is a waste of time.

# Container orchestration

If you just need to run a few containers without having strict
requirements for availability, ACI is a great platform. Applications
that are more mission-critical are likely to have more requirements, for
example controlled, automatic, seamless application upgrades. This is
very difficult to build yourself and it probably is not your core
business.

Fortunately, there are products that can make life a little easier. This
brings us back to Azure Container Services (or ACS). Kubernetes is one
of the three container orchestration platforms offered by ACS. It works
by smartly combining a group of Virtual Machines -- called agent nodes
-- to form one virtualized container hosting platform. Agent nodes can
be added and removed on the fly in order to deal with varying usage
loads. Agents can be either Windows-based or Linux-based machines. The
Kubernetes platform can be used to deploy, run, monitor, scale and
upgrade containerized workloads. In Kubernetes, these workloads are
called deployments.

## Best of both worlds

Kubernetes agents are Virtual Machines that require maintenance, and it
would be great if you could replace those agents with ACI. Well, it
turns out you can. You can do this by installing the ACI connector for
K8S, which is an open source initiative on GitHub:
<https://xpir.it/xprt5-caas1>.

After installing this connector on a Linux node in your cluster, an
additional agent node will appear in your Kubernetes cluster. The
connector will create and delete container groups on the fly. The new
agent can run **both** Windows-based and Linux-based containers. When
using Linux-based containers, you can create deployments using multiple
containers; they will translate into container groups in ACI.

![](./media/image7.png)

Figure 6: ACI-connector node running pods

*Note: At the time of writing this article, the ACI connector is still
in preview and not yet suitable for production use. Do keep an eye on it
for the future, and contribute your improvements to the product.*

Because Kubernetes performs rolling upgrades for you, deployments to ACI
become much easier. Using Kubernetes to orchestrate your containerized
workloads enables your team to deploy to production several times a day.
Combining Kubernetes with Azure Container Instances enables you to run
Linux-based and Windows-based containers, allowing you to choose the
best technology stack for each container you use. At the moment, this is
the closest you can come to running Containers-as-a-Service in Azure.

For a complete, working sample implementation of three containers
running on the ACI platform, please visit my GitHub project at:
<https://xpir.it/xprt-caas2>.
