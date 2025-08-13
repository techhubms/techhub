A million and one metrics to choose from, but what to monitor?

The IT world has been evolving at a rapid pace, and we now have
microservices that run inside docker containers on Kubernetes being
hosted in public or private clouds. These shifts in technology and
platform also introduce new challenges, such as how do you monitor your
applications in these environments?

Imagine diving straight into the combined logs of your application for
the last hour and trying to find out why a certain part does not respond
anymore, without any clear direction or suspicion about what type of
problem you are facing. You're searching for a needle in a haystack and
it will be very hard to quickly and methodically find the problem.
Furthermore, you might not even find a real problem in the logs of your
application. You may be facing infrastructure issues that do not clearly
manifest as problems in your application logs. In this article we will
introduce you to metric systems that allow you to gather the right
metrics in the right place in this new environment. We apply these
metric systems specifically to Kubernetes but they are also valid for
other platforms.

When you want to know the health of your system two terms are important:
Monitoring and Observability.

# Monitoring versus observability 

> Monitoring tells you whether the system works.\
> Observability lets you ask why it\'s not working.- Baron Schwarz

First let's talk about the difference between monitoring and
observability and why we feel it is important to make this distinction.
When we talk about monitoring we look at whether our system works. This
is done using numerical measures or metrics that can be compared over
time and plotted into graphs or tables. Also, alerts can be set on these
metrics, so you can be notified when problems start to occur. Examples
of these metrics are: number of failed requests per second, percentage
of memory usage, average duration of requests, number of requests per
second etc. The metrics used for monitoring are usually not very human
readable on their own and they are often plotted against time or against
another metrics in a graph in order to make sense.

When we talk about observability we look at investigating why the system
is not working. An example of this is logging readable messages in a
file or another more central logging system. These messages contain
details about the behavior of your application and can be very useful
when trying to find out what exactly is wrong. There are various levels
of logging that can range from fatal errors (your application is
crashing) to very verbose messages that help you debug your running
application to find out why your application is behaving in a specific
way.

Monitoring and observability serve different purposes. Good metrics
gathered in the right places will help you monitor your system and
pinpoint where a problem is occurring, and it will give you some notion
of what type of problem it is. But to answer what exactly is going on
you will have to dive deeper and need more detailed information, and
this is where observability comes in. When you have found the most
likely place a problem is occurring, you can, for example, start diving
into the logging and then into the suspicious application or suspect
part of the application to figure this out. In the remainder of this
article we will only talk about what metrics to use for monitoring,
since observability is an entirely separate topic.

## What metrics to use?

There are a million and one metrics that can be collected, but trying to
monitor a lot of different metrics is confusing and does not help you to
quickly find out what is wrong. Luckily, we do not have to reinvent the
wheel, as there are already several sets of metrics available for both
applications and infrastructure. In this article we will look at the two
most well known in these areas: the four golden signals and USE.

## Four Golden Signals

The most well-known way of logging metrics are the Four Golden Signals.
Google has described this in the book Site Reliability Engineering,
which can be read online for free:

<https://landing.google.com/sre/book/chapters/monitoring-distributed-systems.html#xref_monitoring_golden-signals>

Google describes four kinds of metrics to monitor user-facing systems in
their book. When the following four golden signals are measured, and a
human is being paged in case of problematic signals, the service is
decently monitored.

The four golden signals are:

-   **Latency** : the time it takes to service a request.

-   **Traffic**: a measure of how much demand is being placed on your
    system (e.g. http requests per second)

-   **Errors**: the rate of requests that fail

-   **Saturation:** the part of the system which is most constrained.

![](./media/image1.png)

These signals are suitable for monitoring your application or
microservice, if you like. They don't monitor the CPU, Disk or Memory
(it's hard to define how to monitor the traffic of a CPU for example).
For this reason, we also need to monitor our infrastructure. For
infrastructure a practice named USE is also available.

![](./media/image2.png)


## USE

When we look at the infrastructure, we can use the "USE metrics" to
monitor the resources. USE was conceived by Brendan Gregg in his
blogpost "The USE method"
(<http://www.brendangregg.com/usemethod.html>). USE is a method to
monitor resources such as CPU, Memory and Disk. USE is an abbreviation
and stands for:

-   **Utilization:** the average amount of time the resource was busy
    performing work -- this tells us how busy the resource is.

-   **Saturation:** the degree to which the resource has extra work it
    cannot perform directly. Often this work is queued. One hundred
    percent saturation means the resource is servicing the exact amount
    of work it can handle, so no queuing occurs yet.

-   **Errors:** the number of errors that occur

If you apply both the Four Golden Signals and USE metrics to your
infrastructure and application stack, you have a decent visibility of
the health of both your infrastructure and your application.

Next, we will look at how to apply these concepts to Kubernetes.

# Four golden signals and USE on Kubernetes

Three different levels can be identified when monitoring an application
that runs on Kubernetes. In Kubernetes your application runs inside a
pod which runs on a node. So, we can look at these three levels. First,
the node itself. If a node is experiencing issues, you may see issues in
your pods as well. Next, we can look at the pod itself. If the pod is
experiencing issues, you may also see issues in your application.
Lastly, we can look at the application itself. The underlying
infrastructure (node and pod) may be healthy, but your application can
still have issues that are unrelated to the infrastructure. The
following image illustrates how these three levels are related. A pod
runs on a node and an application runs "on" a pod.

![](./media/image3.png)


The following picture illustrates these dependencies. When a lower level
is unhealthy, you are likely to see issues in the levels above as well.
So when we are monitoring our systems we take a look first at the lowest
level, and if that level is healthy we move up to the next level. This
way gives you a structured approach to pinpoint which level is having
issues.

![](./media/image4.png)


We can treat the nodes and pods as infrastructure and thus apply the USE
metrics to these levels. And for the application we can use the Four
Golden Signals. For infrastructure we need to go one level deeper, and
we need to define what resources to monitor with the USE metrics. For
nodes and pods we can look at the following resources: CPU, memory and
disk. We apply the USE metrics to each of these resources.

The following diagram illustrates the various levels of monitoring and
the resources and metrics that should be gathered when we apply the USE
method and the Four Golden Signals:

![](./media/image5.png)

# How to gather these metrics on Kubernetes

The aim is to be able to gather various sets of metrics on three levels
of our application. The *de facto* standard for gathering this type of
information on Kubernetes is Prometheus, combined with Grafana for
dashboards. With Prometheus we can scrape metrics from different
endpoints. To gather metrics about the nodes, pods and application we
will have to expose Prometheus-compatible endpoints for all of this.

To expose metrics for the nodes we can use the node exporter that is
part of kube-prometheus. This is an application that you run in a pod on
each node as a daemon set. It will expose metrics about your node
through a Prometheus-compatible endpoint.

To expose metrics for the pods we can use CAdvisor. CAdvisor is part of
your Kubernetes cluster by default and exposes metrics about your pods
through a Prometheus-compatible metrics endpoint. You don't have to run
any additional pods for these metrics.

To expose metrics for the application, the application itself will have
to expose these metrics through a Prometheus-compatible endpoint. For
.NET applications a good library to expose metrics to Prometheus is
AppMetrics (<https://github.com/AppMetrics/AppMetrics>).

The following image shows how these parts work together to make metrics
available

![](./media/image6.png)


To get started quickly, you can use the Prometheus operator combined
with kube-prometheus to set up Prometheus, Grafana and a set of default
metrics and dashboards. Both can be found in this repository on github:
<https://github.com/coreos/prometheus-operator> and are available as
Helm charts that you can install into your cluster.

# Summary

There are many metrics that can be gathered from infrastructure and
applications. Luckily there are already several metrics systems
available that can help you to collect the right metrics in the right
place. In Kubernetes we can gather metrics on different levels: node,
pod and application. You treat node and pod as infrastructure and apply
the USE metrics to these. For the application, you can use the Four
Golden Signals. You can use Prometheus and Grafana to gather and
visualize these metrics.

When you apply these metrics to your application in Kubernetes, you have
a solid foundation for monitoring.
