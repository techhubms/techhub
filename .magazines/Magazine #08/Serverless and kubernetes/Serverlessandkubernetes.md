# Serverless and kubernetes, introduction to the virtual kubelet

If you talk about Kubernetes and serverless, there are two ways to look
at this. First is the serverless programming model that is often
referred to as Functions as a Service (FaaS). The second way to look at
this is that we have a kubernetes cluster which has no servers that
service the cluster. In this latter situation, you could use a concept
like Azure Container Instances (ACI), Azure Batch, or AWS Lambda to
serve the requests that come in on the Kubernetes cluster to deploy a
container in a POD on the cluster.

In this article, I want to introduce you to the virtual kubelet and what
new capabilities this unlocks in a kubernetes cluster. This allows us to
create a serverless cluster with nodes that are backed by ACI or Azure
Batch.

## What is a kubelet?

Let me start with a short explanation of the role of the kubelet in a
kubernetes cluster. The kubelet is the agent that runs on a node to
manage the lifecycle of pods. The kubelet runs as a service on a node. A
pod is the unit of scheduling in the cluster and consists of one or more
containers that are deployed together on one node. Most of the time a
pod contains one container. The kubelet uses Docker to actually manage
the lifecycle of the containers that are in a
pod.![C:\\Users\\vries\\AppData\\Local\\Packages\\Microsoft.Office.OneNote_8wekyb3d8bbwe\\TempState\\msohtmlclip\\clip_image001.png](./media/image1.png)


Figure : Kubernetes high-level architectural diagram

When a kubelet service starts on a node, it will register itself at the
kubernetes API server as an available node to schedule pods on. From
that moment onwards, the scheduler in the cluster can start assigning
pods on that node. Scheduling a pod on a node is nothing more than
assigning that pod to a node name which equals the name of the node.
That name was given the moment the kubelet registered the node on the
API server. In its turn, the kubelet service watches for these pod
assignments by querying the API server. When it recognizes a pod with
the assigned name of its node, it will start the containers which are
part of that pod, using the container services running on that server.
Often this is Docker.

## What is a virtual kubelet?

Now that we know the role of the kubelet, let's look at what a virtual
kubelet is. A virtual kubelet is a pod that contains a container which
will behave as a kubelet. When you schedule this pod in the cluster it
will register a node in the cluster. This is not a real node in terms of
a normal virtual machine or physical server, but it serves as a virtual
node on which you can schedule pods. The virtual kubelet uses a provider
to do the actual scheduling of the containers that are part of a pod.
The virtual kubelet project on GitHub
(<https://github.com/virtual-kubelet/virtual-kubelet>) already contains
different implementations of providers that can be used by a virtual
kubelet implementation. For instance, providers are available for
scheduling pods on AWS Fargate, HashiCorp Nomad, Service Fabric Mesh,
Azure Batch, and Azure ACI. The virtual kubelet manages the lifecycle of
the pods, just as a normal kubelet on a "real" node would do. The
provider manages the actual lifecycle by working with the underlying
infrastructure that provides the real container instances on the service
it is built for.

So a virtual kubelet is a pod that you can schedule on your kubernetes
cluster, which registers as a node on which you can schedule pods. The
underlying provider used in the specific implementation of that virtual
kubelet then manages the pods.

The following diagram shows how this all works together when you use the
virtual kubelet that uses the ACI provider on Azure:

![C:\\Users\\vries\\AppData\\Local\\Packages\\Microsoft.Office.OneNote_8wekyb3d8bbwe\\TempState\\msohtmlclip\\clip_image001.png](./media/image2.png)


Figure : Virtual kubelet that uses the ACI provider for Microsoft Azure

For the rest of this article, I will use the Microsoft Azure ACI
provider, where the pods will be scheduled on Azure Container Instances.

## How can I register the virtual kubelet with ACI as the provider?

When you have a running Kubernetes cluster like Azure AKS, it is rather
easy to install the virtual kubelet. This is streamlined with the azure
command line interface. Before you can install the virtual kubelet in
the cluster, you need to install the tool Helm**.**

Helm can be seen as the package management solution for Kubernetes, just
like NuGet is a package management solution for .NET application
development. Helm uses a so-called Helm chart that contains the
information on how the package needs to be deployed in the cluster. This
means you can install a Helm Chart in a Kubernetes cluster. You can
compare this to doing a NuGet install, where you download the right
data, YAML files in this case, and then apply these to your project (in
this case the cluster).

Helm is used to install the virtual kubelet. Hence you need to install
this first. Next, you can run the command line to install the kubelet
with the following command:

az aks install-connector \--connector-name mycon \--os-type Both
\--resource-group \<ResourceGroup\> \--name \<ClusterName\>

One thing to note is that the virtual kubelet is named
"install-connector" in the Azure command line. This install-connector
command results in the virtual kubelet pods to be scheduled on one of
the available nodes in your cluster.

After running this command line you can ask the cluster which nodes are
available. This is done with the following command: [Kubectl get
nodes]{.mark}

On my kubernetes cluster this resulted in the following information:

  ------------------------------------------------------------------------------------------------------------------
  **Name**                                   **Status**   **Roles**   **Age**   **Version**
  ------------------------------------------ ------------ ----------- --------- ------------------------------------
  aks-nodepool1-34126871-0                   Ready        Agent       46d       V1.9.11

  virtual-kubelet-mycon-linux-westeurope     Ready        Agent       1h        v1.13.1-vk-v0.7.4-44-g4f3bd20e-dev

  virtual-kubelet-mycon-windows-westeurope   Ready        Agent       1h        v1.13.1-vk-v0.7.4-44-g4f3bd20e-dev
  ------------------------------------------------------------------------------------------------------------------

You can see that I have three nodes: one is the default node that is
backed by a virtual machine and two nodes that are the virtual kubelets.
The first is the virtual kubelet that ties into Linux containers on ACI;
the second is the virtual kubelet that uses Windows containers on Azure
ACI.

## Scheduling a pod on Windows

Based on the results of querying the available nodes, you now have a
Kubernetes cluster on which you can run Windows containers. This is
because ACI provides the ability to schedule Windows containers. The
restrictions on those Windows containers are the restrictions currently
imposed by ACI. This means you can schedule containers that are based on
Windows Server 2016. In the future, Windows Server 2019 will be
supported.

With this configuration, you can now run e.g. Internet Information
Server in the cluster. This can be done by scheduling the following
deployment definition:

apiVersion: apps/v1beta1

kind: Deployment

metadata:

name: iis

spec:

replicas: 1

template:

metadata:

labels:

app: iis

spec:

containers:

\- name: iis

image: microsoft/iis

ports:

\- containerPort: 80

resources:

requests:

memory: 1G

cpu: 1

limits:

memory: 1G

cpu: 1

nodeSelector:

kubernetes.io/role: agent

beta.kubernetes.io/os: windows

type: virtual-kubelet

tolerations:

\- key: virtual-kubelet.io/provider

operator: Exists

\- key: azure.com/aci

effect: NoSchedule

In the deployment definition you can see that we have defined a node
selector. This selector indicates that we want to schedule the pod on an
agent that has an OS of the type "Windows" and that the node is of the
type "virtual kubelet". This is the way we explicitly define that we
want to run the pod on the Windows virtual kubelet within the cluster.
The other part that is special to this deployment is the definition of
the tolerations. By default, the virtual kubelet nodes are what we call
tainted. Tainted means that we specify restrictions that tell the node
not to schedule pods by default. You can only schedule the pods
explicitly when you add a toleration to a taint. This is done to avoid
scheduling just any pod on the virtual nodes. Normally you first want to
fully utilize your nodes in the cluster before you start leveraging the
serverless nature of ACI and scale out without creating new nodes. You
also don't want to schedule a pod like the kube-proxy or other virtual
kubelet pods on the virtual node. In this deployment we explicitly
define that we accept the fact that the node is marked as NoSchedule by
default and we overrule this by specifying the tolerations key 'value
pair' that matches the taint.

After running this deployment, we can expose the scheduled IIS container
in the pod via the Azure load balancer service. We can do this by
running the following command:

[kubectl expose deployment iis \--port=80 \--type=LoadBalancer]{.mark}

This configures the external Azure load balancer in order to make the
IIS service reachable via an external IP address. If you want to know
which IP address was assigned to the IIS deployment then you can query
the cluster with the following command:

[Kubectl get services]{.mark}

On my cluster this resulted in the following information:

  ---------------------------------------------------------------------------------------
  **NAME**     **TYPE**       **CLUSTER-IP**   **EXTERNAL-IP**   **PORT(S)**    **AGE**
  ------------ -------------- ---------------- ----------------- -------------- ---------
  iis          LoadBalancer   10.0.139.238     104.40.243.220    80:32652/TCP   4m

  kubernetes   ClusterIP      10.0.0.1         \<none\>          443/TCP        46d
  ---------------------------------------------------------------------------------------

## Scaling the deployment to use more replicas

The advantage of running your services in a Kubernetes cluster is that
you can define the desired state of your service and the cluster will
try to get to this state. All we need to do to schedule more IIS
instances is to increase the number of replicas. The scheduler will then
start scheduling more pods on the virtual kubelet, which in turn will
delegate this to the ACI provider.

After increasing the replicas to 5 replicas with the command line:
[kubectl scale deployment iis \--replicas=5]{.mark}

you will see that 5 container instances will be scheduled on ACI, as
shown below:

![](./media/image3.png)

## Scaling up instead of out

It is also possible to schedule the containers on a more powerful
container instance. ACI has the option to provide a container with 1 --
4 cpu's and you can also specify the amount of memory you want to make
available for the container. This can be specified in the deployment. By
defining the resource requests and limits, you define how the ACI
provider will schedule the container. For example: increasing the CPU
request to 2 results in a container instance scheduled with 2 CPU's.

## When is a virtual kubelet useful?

The concept of a virtual node in a cluster with workloads scheduled by
any type of provider allows a series of interesting scenarios. For
instance, take the following use cases:

-   **Batch workloads**

You don't need to have VMs running in your cluster to support your batch
workloads. You only need to pay for your normal workloads, and batch
jobs can fan out as widely as needed to complete them in less time,
while you pay per second.

-   **Burst loads**

If you use auto-scaling and your traffic comes in spikes, you only need
to plan enough capacity for your average workload. The moment you run
out of capacity in your cluster, the scheduler can start placing
additional pods in something like ACI or another provider.

## Conclusion

With the new capability of the virtual kubelet you can use various
implementations that extend your Kubernetes cluster to be able to run
your containers on a serverless infrastructure. The ACI and Azure Batch
implementations allow you to leverage those parts of Azure and only
pay-per-use instead of paying for the physical nodes your cluster would
have otherwise. The virtual kubelet is a new way of implementing the
concept of serverless, while keeping the same semantics as you already
were using when running a Kubernetes cluster. It combines the best of
both worlds: you can define your desired state and have the cluster
manage this with a pay-per-use solution.
