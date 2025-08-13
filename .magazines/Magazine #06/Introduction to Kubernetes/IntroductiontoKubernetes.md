# Introduction to Kubernetes

Docker containers revolutionized the way people build, package, and
deploy software. However, how do you host Docker containers in a
resilient way, and with high availability? And how do you make sure your
application won't have downtime during an update?

In this article we will tell you what Kubernetes is, and how it ensures
efficient, stable and secure container hosting. In addition, we will
explain the most frequently used resource types and concepts. After
reading this article you will know everything you need to know to run
your first container in Kubernetes. We will also provide you with an
example application and everything that is required to reproduce the
scenario in a situation in which one of the servers that runs your
containerized application becomes unavailable!

## Why an orchestrator

You could simply setup a cloud-hosted VM or an operating system on bare
metal that runs Docker. Although you can host containers on this
machine, you will soon run into limitations if you host your
containerized applications this way. What happens if your container
crashes because of an exception? What happens when the server runs out
of memory? What if the server crashes? Your containers will die and your
production environment will go down.

You will soon run into other issues as well. Take for example the
situation in which your application is very successful and traffic
increases to an amount that your server can't handle. Your application
will need to adjusted, not just by increasing the number of instances of
your container, but you will simply need more resources in the form of
physical or virtual servers. At this point you will have to think about
how to route traffic to containers running on these different servers.
Will all containers of a particular type be on the same server or will
they be spread over multiple servers? And if they are spread, how will
you route traffic to them in such a way that the load is spread in an
effective way? How will different containers communicate with each
other? And what happens if one of the containers or even one of the
servers crashes? A container orchestrator will solve these issues for
you and much more.

## What is Kubernetes?

Kubernetes is an open source container orchestrator, initially created
by Google. Google developed Kubernetes with all the experience they had
from a previously built orchestrator named Borg. On July 21th, during
OSCon 2015, Google announced Kubernetes is ready for production; this
first release is labeled as version 1.

When Google partnered with the Linux Foundation to form the Cloud Native
Computing Foundation (CNCF), Kubernetes was open-sourced and many others
started contributing to the project. If you look at the commits and
activity on Github, the activity on StackOverflow and what we see at our
customers, Kubernetes has proven to be a very popular platform. Other
orchestrators, such as Docker Enterprise and Mesos, have started
allowing you to use Kubernetes as the underlying orchestrator.

All major public clouds are offering Kubernetes, so this means no cloud
vendor lock-in! Google offers Kubernetes as Platform as a Service
(PaaS), known as Google Kubernetes Engine (GKE). AWS offers Kubernetes
as IaaS and has announced a PaaS offering. Azure offers Kubernetes both
as PaaS known as AKS, and also as IaaS with Azure Container Service
(ACS). However, you can also run Kubernetes on bare metal if you want
to.

+----------------+------------------+----------------+----------------+
| ##             | ## AWS           | #              | ## Azure       |
|                |                  | # Google Cloud |                |
+================+==================+================+================+
| #              | ##               | ##             | ## (ACS)       |
| # Managed IaaS |                  |                |                |
+----------------+------------------+----------------+----------------+
| ## PaaS        | ## (announced)   | ## (GKE)       | ## V (AKS)     |
+----------------+------------------+----------------+----------------+

## 

## Masters and nodes

Let's dive in right away. Figure 1 shows the main components of a
Kubernetes cluster, the master and the node(s):\
![](./media/image1.png)


Figure : overview of a Kubernetes cluster

A common Kubernetes cluster consists of at least one master and one
node. (A node used to be named a Minion.) The master is a physical or
virtual machine which controls everything that happens in the cluster,
such as scheduling containers on the nodes. A node is a physical or
virtual machine that does the actual work: running containers.

Having only one master and one node will not give you a high available
cluster. To have a high-available cluster, which is suitable to be used
in production, it's a best practice to have multiple nodes to do the
actual work, and multiple masters to manage the cluster. Having multiple
nodes allows the masters to spread containers over different nodes and
to have redundant copies on multiple nodes. This way, an end-user will
not notice anything when a node goes down. Having multiple masters
allows your cluster to continue functioning when one of your masters
goes down.

A single node can only run containers for which it has available
resources. So, another reason for adding nodes to your cluster is to be
able to handle more containers. Kubernetes has a lot of different
options for scaling your containers but to be able to run multiple
redundant instances of your containers in your cluster you will need
sufficient resources, in the form of nodes. In short, all nodes together
do not make up one, big machine that runs your containers. Instead, they
are independent workers that pick up workloads in the form of containers
and execute them at the master's request.

Each master and each node runs several processes to allow a master to
spread the containers over the nodes in your cluster and to allow your
cluster to recover when a node fails.

~~\
~~

## What do you need to run an application in Kubernetes?

To understand what it takes to run your first application inside a
Kubernetes cluster we first have to explain a few different resource
types and concepts. Figure 2 shows an overview of the different
resources involved in running "myapp" in your Kubernetes cluster. In
this example we have a public URL; www.example.com, that allows us to
access the "myapp" application, which runs on the containers from
outside the cluster. Each of the resource types shown in this example,
including their inner workings, will be introduced here.

## ![](./media/image2.png)

Figure : example application hosted in a Kubernetes cluster

## Pod

Kubernetes is not designed to run Docker containers directly. Instead,
Kubernetes uses a pod to run containers. A pod is a group of one or more
containers that shares storage and networking. Containers inside a pod
always run on the same node in the same context and are tightly coupled.
They also share a network interface. Each container in the same pod
shares the same IP address and ports. Because each pod has its own IP
address, each individual pod can contain a container that exposes port
80. Inside a pod, containers also share storage. In Docker terms this
means that volumes can be shared across containers that run in the same
pod. Containers inside a pod also share the same lifecycle. This means
that when a pod is stopped, all containers inside that pod are stopped.
A pod is the smallest scalable and deployable unit in Kubernetes.
Kubernetes can replicate pods across one or more nodes in your cluster.
If containers need to scale independently from each other, these
containers should not be part of the same pod.

Take the example of a set of applications that each run inside a
container, and you want to have the ability to scale, update and recover
these containers independently. If you put these containers in the same
pod, they would share their lifecycle and that would make it impossible
to scale independently. This is why you will often see a pod only
contains one, single container, optionally supported by one or more
"sidecar" containers (think of containers that provide log shipping,
metrics gathering, etc.).

![](./media/image3.png)


Figure : pods in detail

## Service

There can be multiple instances of the same pod being scheduled across
the available nodes in your cluster. When a pod is created, it will be
assigned one of the available internal IP addresses, and when it is
destroyed, this IP address will become available again. When a new pod
is created, you have no guarantee it will get the same internal IP
address. In this sense pods are mortal, they are born, they die and they
cannot be resurrected. When you scale pods, or do a rolling upgrade of
pods, they will be created and destroyed dynamically. This is why you
cannot rely on the pod's IP address to be stable over time.

So how should you connect to a web application that runs inside a pod in
a reliable way? You can't use the IP address of the pod, because that
will inherently change over time when the pod is destroyed and created
dynamically. For this purpose, you will need to use a service. This is
an abstraction that defines a logical set of pods as well as a policy by
which to access them. A pod is not directly bound to a service or vice
versa. To determine which pods are targeted by a service, a label
selector is used. This label selector tells the service which pods it
should route traffic to. This mechanism takes into account pods being
dynamically created or destroyed. All the pods that are currently
running with labels that fall within the label selector of the service
will be targeted. Since a service will cover one or more instances of
your pods, it will automatically load-balance traffic across all
available instances.

A service has its own internal or external IP address that can be
"locked" down. This way you can rely on the fact that a certain IP
address will always belong to the same service. By using the IP address
of the service instead of the IP of the pod we now have a reliable way
of connecting to one of the instances of our pod, no matter on which of
the nodes it is running inside the cluster. If you need a service with
an external IP, then you will need to create a service of the type
"LoadBalancer". In this case, Kubernetes will instruct your cloud
provider to create a load balancer with a public IP that allows traffic
from the outside to reach your cluster. If you need a service with an
internal IP then you will need to create a service of the type
"ClusterIP". In that case, Kubernetes will assign an IP to the service
that is only resolvable inside the cluster. Other pods can use the
"ClusterIP" service, but traffic from outside your cluster cannot reach
it directly.

A service can be deployed using a yaml file that contains the
configuration. See our sample repository provided at the end of this
article for a sample yaml file.

![](./media/image4.png)

Figure : services in detail

Deployment

To create a pod or multiple instances of the pod, called replicas, you
create a deployment. In this deployment, you specify the number of
replicas you want, how you want to scale, when you want to scale, and
which Docker image or images need to run inside the pod. Like many
resources, a deployment is specified in a yaml file and describes the
desired state. The deployment controller will either create, destroy or
update the pods described in your deployment in order to reach the
desired state. When you apply the yaml file to your Kubernetes cluster,
a deployment is created and the deployment controller will make sure the
correct number of replicas is created. The deployment can also specify
preferences that affect how and on which nodes the pods are scheduled.
If you want to update the version of the image running inside your pod,
you update the yaml file and apply it again to your Kubernetes cluster.
The update settings will determine how the pods will be upgraded to the
new version. In case of a rolling upgrade, Kubernetes will gradually
create new pods and destroy the old ones until all instances satisfy the
desired state.

![](./media/image5.png)


Figure : deployments in detail

Ingress

Services can offer a public ip-address to a set of pods. However, the
number of available public ip-addresses is limited. Because you can only
use a set number of public IP addresses in any public cloud, ingresses
provide you with a way to map multiple URLs to a single public IP and
reroute the traffic to the right pod, based on the URL requested by the
user. Typically, the IP addresses of pods and services are only routable
inside your Kubernetes cluster. Ingresses allow external connections to
reach the internal cluster resources. An Ingress is a set of rules that
can be configured to give cluster resources URLs that are externally
routable, load balance traffic, terminate SSLs, offer name-based virtual
hosting, and more. An ingress is simply a rule stored in the cluster,
but this alone will not have any effect. To satisfy the ingress you need
an ingress controller which will interpret the ingress rules and reroute
traffic to the right pods. The ingress controller knows which pods
belong to a service and thus it can directly route traffic to one of the
pods.

By default, no ingress controller is available, so you will have to
choose one of the available ingress controllers or create your own. For
this article, we will use the readily available Nginx-based ingress
controller. Basically, the Nginx ingress controller listens for changes
to ingress resources in your cluster. Once a change is detected it will
generate an Nginx config file that allows the Nginx proxy container to
reroute the traffic to the right cluster resources. A typical use of
ingresses is registering an external routable URL and configuring it to
allow traffic to reach the container inside your pod.

In the following example, the Nginx ingress controllers detect that the
myapp-ingress has been created in the cluster. This ingress describes
that traffic for the URL www.myapp.com that reaches the Nginx proxy must
be rerouted directly to the pods belonging to the myapp-service.

![](./media/image6.png)


Figure : ingress routing in detail

Putting it all together\
After describing the most common resources in a Kubernetes cluster, let
us continue with a sample deployment. The deployment shown in figure 7
should run four replicas of the green pod and two instances of the
yellow pod. The deployment controller has made sure that the instances
are spread over the available nodes. The yellow pods contain the Nginx
ingress controller and this controller knows how to route traffic for a
certain URL to the right pods. We can now access the application running
inside the pod using <https://www.myapp.com>.

![](./media/image2.png)

Figure : example application hosted in Kubernetes

Now imagine there is a problem with node 3, the node crashes and all the
pods that were running on it are gone. The ingress controller can now no
longer route traffic to the pods that were running on node 3.

![](./media/image7.png)


Figure : a node in the cluster crashes

The my-app application is still able to serve traffic on three of its
pods, but remember that in our deployment we specified four replicas of
the green pod and two of the yellow pod. This is no longer the case;
there are only three green pods and one yellow pod left. The deployment
controller will now take the necessary actions to make the cluster
comply with the desired state again. In this case, this means that the
deployment controller will schedule a new yellow pod on node 2 and a new
green pod on node 4.

![](./media/image8.png)


Figure : the cluster recovered from the crashing node

The cluster now complies with the desired state again. The my-app
application was never down and the deployment controller ensured that
our cluster could recover from the failing node and recreated the lost
pods on the remaining node.

This example shows how all the resources described in this article work
together to provide an easy way to run a highly available and
fault-tolerant application using Kubernetes.

## Let's bring the theory into practice

We have now introduced all the resource types and concepts you need to
run your first application inside a Kubernetes cluster. So now it's time
to do it! In the repository mentioned below, we have provided all the
steps and examples you need to reproduce the scenario we just described.

<https://xpir.it/introduction-to-kubernetes>
