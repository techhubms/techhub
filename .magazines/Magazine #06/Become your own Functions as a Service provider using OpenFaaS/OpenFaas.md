# Become your own *Functions as a Service* provider using OpenFaaS

FaaS, or Functions as a Service, is a relatively new offering that you
can hardly miss nowadays. Some see it as a successor to Platform as a
Service, others see just another way of hosting your application
functionalities. The major cloud providers also stepped into this area
and offer serverless computing capabilities. As always, your application
needs to run somewhere, so it is a nice marketing term for a hosting
solution that allows you to make efficient use of available resources by
running short-lived pieces of code, which are small, usually stateless,
and on demand. OpenFaaS is a project that makes use of Docker containers
to convert any process into a serverless function, regardless of the
programming language of the code.

## Why Functions as a Service?

There are various reasons why Functions as a Service is interesting for
many organizations.

First of all, it allows for rapid prototyping of application logic.
Development teams only need to write small pieces of code, in the
language of their choice, and this can be deployed directly to the
cloud. Many cloud providers offer integrations with other services
within their platform (such as queues and databases) and this makes it
very easy to get up and running in creating a serverless architecture.

Secondly, it provides a way to achieve a highly scalable solution on a
highly granular level since individual functions can scale up when the
load is heavy, and scale back down when the load is low. Usually, the
cloud providers fully manage this scaling so it requires no additional
effort.

Lastly, all cloud providers offer a consumption-based pricing model.
This means that billing is done by the number of executions combined
with the execution time. This can be a very attractive pricing model if
the application workload changes aggressively over time (spikes) or when
the workload is constantly very low.

An often heard argument against FaaS concerns vendor lock-in. Once you
are using a FaaS platform by one of the cloud providers, like Azure
Functions, AWS Lambda or Google Functions, it is difficult to move to
another provider because each FaaS platform is specific to that
provider. The OpenFaaS project is a very interesting alternative for
organizations that want to avoid this lock-in.

## The OpenFaaS project

A Docker Captain called Alex Ellis[^1] liked the idea and purpose of
serverless functions but wanted to see if there was a way to avoid
having a vendor lock-in and use private or hybrid clouds as an
alternative. The OpenFaaS project[^2] uses Docker and Kubernetes as the
underlying technology and this allows OpenFaaS to run functions nearly
everywhere. This can range from a single laptop to large-scale cloud
systems, so you have plenty of hosting options available.

![](./media/image1.jpg)
![](./media/image2.png)


Figure OpenFaaS architecture

The OpenFaaS architecture (Figure 1) is set up in such a way that you
can host any kind of programming language as long as it can talk to
standard input (stdin) and output (stdout), the standard
streams[^3] that are preconnected input and output communication
channels between a computer program and its environment. Your code is
packaged inside a small Docker container together with a watchdog
component. The job of the watchdog is to accept requests from a HTTP API
gateway, route the request to the standard input of the application,
read the output from the standard output of the process, and return this
to the sender. In addition, it enforces timing constraints to the
container.

As mentioned already, the OpenFaaS architecture includes an API gateway
that provides HTTP and JSON capabilities so you don't need to write this
kind of plumbing anymore.

As Docker can use Prometheus for metric collection, the OpenFaaS project
utilizes the same system. It also uses the data to detect whether
containers are under a certain load and therefore need scaling. An
online dashboard allows you to monitor and set your own alerts.

OpenFaaS also comes with its own command line interface (CLI) which
gives you control over the functions and makes it possible to easily
create and deploy functions. We will show you how this works in this
article.

## Taking OpenFaaS for a spin

For the purpose of this article, we will spin up OpenFaaS on a simple
Docker Swarm cluster. Even if this is a cluster of only one node and
that node is your own machine, it will be fine.

Make sure you have installed Docker with Swarm support and that you have
initialized a Swarm if you have not done this yet:

docker swarm init

Fetch the actual code using a git clone command to a local folder:

git clone https://github.com/openfaas/faas

This will pull down the required files and when fetching is complete,
navigate to the automatically created folder called *faas*. Next, you
will need to run the deployment script. Use either the .sh if you are
running from a Linux bash shell, or the .ps1 when using Windows
Powershell:

.\\deploy_stack.ps1

This will create the required networks and services (both for running
OpenFaaS and for samples) in Docker Stack, and the system is ready to
use (Figure 2).

![](./media/image5.png)


Figure OpenFaaS default set of services and functions

You can now navigate to <http://localhost:8080> using your web browser
to see the OpenFaas dashboard (Figure 3).

![](./media/image6.png)

Figure OpenFaas Dashboard

This dashboard shows all the deployed example functions and you can
invoke them directly. There is a gallery of functions under *Deploy New
Function* where you can easily add existing functions to your FaaS
system.

### Create

Let\'s see how you can create your own function. For this purpose we
will use the CLI and there are various ways of installing this tool as
described on the GitHub FaaS-CLI[^4] site, so pick the one applicable to
your operating system.

When installed, you can add functions using a *new*, *build* and
*deploy* command pattern. The *new* command requires a function name and
a (programming) language template (Figure 4).

faas-cli new helloworld \--lang csharp

![](./media/image7.png)


Figure Creating the helloworld csharp function

After you have created the helloworld function, the following files and
folders will have been added:

-   A *template* folder with function templates. OpenFaaS automatically
    downloads function templates from GitHub when these are not yet
    available (Figure 5). A more detailed description of these templates
    is provided in the next section.

![](./media/image8.png)


Figure OpenFaaS function templates

-   A *helloworld* folder with the Function.csproj and the
    FunctionHandler.cs files (Figure 6).

![](./media/image9.png)


Figure Helloworld function based on the default csharp template

-   A *helloworld.yml* file. Similar to docker-compose or Kubernetes,
    the definition of an OpenFaaS function is described in a yaml file.
    This makes it easier to build and deploy sets of functions and keep
    your server in the desired state (Figure 7).

![](./media/image10.png)

Figure Helloworld yaml file after you created a new csharp function

### Templates

Now that we've created a new function let's have a look at the template
on which it is built. Each language template contains a dockerfile and
an entry point application file. The dockerfile contains a multi-stage
definition; the first part builds the function code, the second part
uses the output, includes the watchdog component, and defines the entry
function to be the watchdog itself. The dockerfile for the C# template
(located at *template\\csharp\\Dockerfile*) is shown in Figure 8.

![](./media/image11.png)


Figure Dockerfile for the csharp template

The csharp template function is located at
*template\\csharp\\Function\\FunctionHandler.cs* (Figure 9).

![](./media/image9.png)


Figure csharp template function

Because OpenFaaS uses the standard input and output streams your C#
function is actually a small console application, as can be seen in
Figure 10.

![](./media/image12.png)


Figure stdin/stout communication for the csharp template

### Build

When you have completed writing your *helloworld* function, you need to
build this before it can be deployed. Use the CLI to invoke the *build*
command which instructs Docker to build the dockerfile that contains the
instructions for both building and packaging.

faas-cli build -f .\\helloworld.yml

![](./media/image13.png)
![](./media/image14.png)


Figure Building the helloworld function

The generated **build** folder contains the project file which you can
open with, for example, Visual Studio (Code). This allows you to run,
debug and edit with intellisense before it gets packaged inside a docker
container. While running the console application, keep in mind that a
control-Z will end your input to your function and allows the function
to return something to the console.

After building you need to execute the *deploy* command:

faas-cli deploy -f .\\helloworld.yml

![](./media/image15.png)

Figure Deploying the helloworld function

This applies the functions defined in the yaml file to the configured
provider. The above-defined function will live under the
<http://localhost:8080/function/helloworld> endpoint. A curl POST
request to this endpoint returns the defined string:

curl -d \"myname\" http://localhost:8080/function/helloworld

Hi there - your input was: myname

The dashboard and the faas-cli are other ways to invoke this function.
All this time OpenFaaS is using Docker to deploy and host your
containers. You can still use the docker commands to see your functions
running on the nodes (Figure 13):

docker service ps helloworld

![](./media/image16.png)

Figure Listing the container that runs the helloworld function

If you have a cluster, the container will be hosted on one of the nodes
and kept available when nodes crash. Calling the functions a number of
times will produce metrics which are collected inside Prometheus. With
the Prometheus portal on port 9090 you can build all kinds of dashboard
and monitor metrics such as the state, number of invocations, and
execution durations (Figure 14).

![](./media/image17.png)

Figure Prometheus portal

Prometheus has predefined rules to alert on services that are down as
well as high invocation counts. OpenFaaS uses these metrics and alerts
to scale the functions up or down.

## When to choose OpenFaas instead of a vendor-specific FaaS

OpenFaaS or vendor-specific systems such as Azure Functions or AWS
Lambda are all valid platforms to develop and host functions. However,
you do have to realize that a vendor-agnostic solution comes at a price.

Although you can choose your own host(s) with OpenFaaS and avoid
vendor-lock-in, OpenFaaS lacks (cloud provider specific) integrations
which can limit the rapid prototyping capabilities. For instance, Azure
Functions provides seamless integration (triggers) with other Azure
services such as CosmosDB, Storage Queues, Blob Storage, ServiceBus,
EventGrid etc. With OpenFaaS, you only have HTTP triggered functions and
you have to develop other integrations yourself.

There is a learning curve for each FaaS platform, but the learning curve
for OpenFaaS is a bit steeper if you don't have some container
technology experience. That being said, containers are conquering the
world so we highly recommend upgrading your skills on that part.

A possible benefit of OpenFaaS is that it is open source. So if you want
to change the way it autoscales or want support for a new language you
can develop that yourself. This level of flexibility is usually not
provided by a cloud vendor. However, open source has its drawbacks in
the form of support, lifecycle management, and documentation, which
might not align with your requirements.

Use the OpenFaaS platform when

-   you don't require easy integration with cloud-specific services

-   your development team is comfortable with using container technology

-   you require full control of a FaaS platform and your development
    team also has the capability to maintain it.

## Summarizing

OpenFaas allows you to run your own functions anywhere thanks to
container technology. It supports many programming languages out of the
box and since the platform is open source, you can extend or change it
to your needs. It does, however, lack the integration options that
cloud-specific platforms provide. As with any FaaS platform you host
yourself, you need to make sure you secure it by using a gateway
solution such as Treafik, Kong or API Management before you bring it
into production.

SIDENOTE: If you like a DIY challenge while learning .Net Core, OpenFaas
& Kubernetes, we can highly recommend this post by Scott Hanselman:
<https://www.hanselman.com/blog/HowToBuildAKubernetesClusterWithARMRaspberryPiThenRunNETCoreOnOpenFaas.aspx>.

[^1]: <https://www.alexellis.io/>

[^2]: <https://www.openfaas.com/>

[^3]: <https://en.wikipedia.org/wiki/Standard_streams>

[^4]: <https://github.com/openfaas/faas-cli>
