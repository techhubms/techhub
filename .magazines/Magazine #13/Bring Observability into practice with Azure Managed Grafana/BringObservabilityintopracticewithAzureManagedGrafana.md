*By Rik Groenewoud and Casper Dijkstra*

At Xpirit we live by the motto "You Build it, You Run It!". As true
DevOps minded people our interest does not stop when a new release is
deployed. This is only the first step towards the ultimate goal: deliver
a solution with the best end-user experience possible. To be able to run
a solution in a reliable, secure and stable way, it is crucial to know
what is going on *after* go-live. We want to measure application
performance in the broadest sense of the word.

With the shift from monolithic systems towards distributed systems, from
on-premise towards the cloud, from running virtual machines to
serverless computing, it has become invaluable to monitor properly in
order to get a grip on the complete system.

At the same time, proper monitoring has become an ever increasing
complex matter. Back in the old days performance issues could be
predicted, such as full or broken HDDs, memory issues or CPU overload.
Nowadays a different approach is required. Things will break down, only
it cannot be foreseen when or where this will happen.. This calls for a
new approach regarding how the running state of our cloud solutions can
be measured. Static dashboards or reactive alerting are not sufficient
anymore. This is where Observability comes in.

## Observability

The term "Observability" originates from control theory and can be
explained as a measure of how well internal states of a system can be
inferred from knowledge of its external outputs.[^1] In the recently
released (June 2022) O'Reilly handbook called "*Observability
Engineering", this formal* definition is applied on software systems.
The authors come up with a more practical approach:

> *"\... our definition of "observability" for software systems is a
> measure of how well you can understand and explain any state your
> system can get into no matter how novel or bizarre"*

In other words, when an issue occurs all tools and data are directly at
your disposal to debug and mitigate the issue at hand. In the current
age of complex production systems, traditional monitoring and alerting
practices no longer suffice. One of the main disadvantages is that
collecting and monitoring metrics, such as CPU or Memory usage of a VM,
is fundamentally *reactive.* Furthermore, monitoring on these kinds of
metrics is based on previous known possibilities of where the system
could fail (known-knowns). But what to do if something unexpected
happens, e.g. something not seen before and therefore not recorded in
the existing monitoring? No alert will trigger and the engineer that
eventually will have to fix the issue has to go and search for the
needle in the distributed serverless haystack.

As the writers of Observability Engineering put it:\
\
*"In modern cloud native systems, the hardest thing about debugging is
no longer understanding how code runs but finding* where in your system
*code with the problem even lives"*[^2]

How to prepare for the "unknown unknowns"? This is the question
Observability wants to answer. Instead of adding more and more metrics
and alerts as new things break over time, observability turns around
this approach by acknowledging it cannot predict what will break next in
the system.

From this acknowledgement follows a new ultimate goal: collect as much
relevant contextual data as possible. Collecting structured logs and
events and making it possible to slice and dice through the data becomes
pivotal. A curious *pro-active* attitude harnesses ourselves to solve
previously unknown states of the system.

New tools should be created that help us to preserve:

> *"\... as much of the context around any given request as possible, so
> that you can reconstruct the environment and circumstances that
> triggered the bug that led to a novel failure mode."*[^3]

![Timeline Description automatically
generated](./media/image1.png)


## Microsoft Azure and Observability

*Figure 1. Where monitoring and its tools aim to cover the Known
Unknowns, Observability on the other hand covers the realm of the
Unknown Unknowns.\
*

With the above theory in mind, Microsoft seems to embrace Observability.
They also aim to provide centralized logs, metrics and traces. Azure
Monitor promises to *"\... deliver\[s\] a comprehensive solution for
collecting, analyzing, and acting on telemetry from your cloud and
on-premises environments."*[^4]

Azure Monitor aims to be the *"single pane of glass"* when it comes to
observing performance of resources. From here one can dive into
automatically aggregated logging, application insights, service health,
or monitor a Kubernetes cluster.

With Azure Monitor and solutions like Application Insights at our
disposal, Azure provides a very solid monitoring and logging solution.
Just as the observability theory advises, it brings together a lot of
important data and, more importantly, transforms this data into
*structured events* that can be queried and zoomed into on a per request
level.

So why the need for an additional tool on top of this? For many
workloads (such as running App Services) one could argue that Azure
provides enough to observe your application in depth.

Still, there are scenarios in which there are real benefits to adding
Grafana to the toolset. As we will show in the rest of this article, it
can be a powerful solution because it can do things not possible in
Azure. But first: Why should one even bother to visualize data into a
graphical representation called a dashboard?

### Why use visualization? 

One of the more obvious advantages is that by making data visible, one
gives access to otherwise unseen events and status. An important benefit
of simplifying data into a graphical representation is that
non-technical people get an idea of how the system is performing. In
other words, the data is *democratized* and made available for the whole
company.

It is good to realize that not all dashboards are relevant for all
users. For instance, SLO dashboards can be made accessible company-wide
while dashboards that help the debugging process, e.g. Kubernetes
dashboards or technical error collections, can be made available for
just the DevOps teams.

For developers or IT engineers who have the task to troubleshoot an
incident, a well-designed dashboard can be a great starting point to
start investigating. In case of a complex technical problem an elaborate
dashboard can act as the Swiss army knife pointing the right direction,
or at least verify that several metrics are within the realm of
expectation. It can also tell *which factors can be excluded from the
possible factors incurring the problem*.

Of course, this stands or falls with the quality of the dashboard. Which
metrics are shown? Which alerts are implemented? Naturally it depends on
the workload where to focus, but in general it is wise to focus the
measuring and alerting on events that have end-user impact. To be able
to determine where this impact lies, it is important the business
stakeholders - who know the value of their product is -- are asked, what
kind of problems or outages would be truly painful. Based on these kinds
of conversations the Service Level Objectives (SLOs) can be constructed.
From there the SLOs can be visualized and share through the company.

(To learn more on SLOs and how to use error budgets, read the article in
Xpirit Magazine #11: "The reliability paradox: Why less can be more" by
Geert van der Cruijsen and Casper Dijkstra.)

## Azure Managed Grafana 

### Usability: combine multiple data sources and metrics

What makes Azure Managed Grafana an interesting addition to the toolbox?
Let's start with usability. People may argue that Azure provides all the
required tooling, however, Grafana is a specialized dashboarding tool
with many visualization options, graph types and a user-friendly
interface. By creating togglable data panes and metrics, correlations
between different data can be investigated easily, e.g. see that
performance degraded directly after a new deployment or that the
application became unresponsive as soon as the number of unhealthy pods
spiked. With Grafana, professional dashboards can be setup with
insightful graphs. Instead of using the static tiles of an Azure
dashboard or workbook, Grafana allows for free customization of the
dimensions of the graphs with more options when it comes to the look and
feel of the graph.

![](./media/image2.png)


*Figure 2: An example of a visualization of AKS cluster performance in
Grafana. The dashboard consists of multiple so-called **rows** such as
'Cluster - Overview' and 'Consumption by Namespace'. The rows can be
maximized and minimized to see just the data needed. All panels can be
transformed freely and there are a lot of visualization options
available. At the top **variables** are used to select a specific
resource group or cluster. These variables are powerful because this one
dashboard now can be used for all clusters in a particular
infrastructure.*

*This dashboard is available via:*
https://grafana.com/grafana/dashboards/12817

As said before, dashboards have a specific audience. That is why it is
good to know that Azure Managed Grafana supports RBAC on the Azure and
Grafana level. On the Azure side, ​​Admin, Viewer and Editor permissions
can be set on individual users and AD groups. Global permissions of who
can modify dashboards and who has read-only permissions can be set as
well.

On the Grafana side, access to individual dashboards can be
fine-grained; documentation can be found at
[[https://grafana.com/docs/grafana/latest/administration/roles-and-permissions/access-control/]{.underline}](https://grafana.com/docs/grafana/latest/administration/roles-and-permissions/access-control/).

A powerful asset of using Grafana is the ability to combine multiple
data sources. Think of scenarios in which data from multiple Azure
subscriptions from different regions, different tenants etc. needs to be
combined. This can all be done easily. The Azure Monitor data source
authenticates using an Azure AD App Registration.

![](./media/image3.png)


On
[[https://grafana.com/grafana/plugins]{.underline}](https://grafana.com/grafana/plugins/?type=datasource&utm_source=grafana_add_ds)
you can find more than 100 data sources that can be added to the
dashboards. Using the Microsoft stack, we are particularly interested in
the Azure, Azure DevOps and Prometheus plugins, but for customers using
Jira for ticket management or GitHub for development, it is good to know
those integrations are also supported.

Take this one step further and go outside of Azure and it becomes even
more interesting. Envision a hybrid infrastructure in which all kinds of
data sources can be brought together, such as on-premise SQL Server, AWS
or other external observability tools e.g. DataDog or Jaeger. In short,
a lot of the relevant data can be brought towards one hub system. In
this way the combined data can be visualized and it becomes a powerful
first-stop to get a comprehensive overview of the performance of the
complete infrastructure.

![](./media/image4.png)
When trying to visualize multiple Azure
metrics on one graph via the Azure Portal, the following message may
have popped up more than once.

In the Azure Metrics graphs only one resource can be selected at a time.
This contradicts good observability practice. Often one would like to
compare several resources to see how they correlate or differ in a
certain time frame. This can give valuable insights into the root cause
of issues.

This use case is something that is totally possible in Grafana. Multiple
queries of different metrics can be added in one graph, table or other
visualization of preference. An example could be an overview of response
times of several app services or maybe to compare the amount of healthy
pods in several AKS clusters. This can be done easily in Grafana, as can
be seen in *Figure 3 and Figure 4.* where multiple metrics are shown on
the same time-scale.

![](./media/image5.png)


*Figure 3. An example where availability statuses from multiple
environments, in multiple subscriptions are combined in one panel.*

### ![](./media/image6.png)

*Figure 4. The 3 queries (Test, Acc and Prod) from the panel in figure
3*. *Add/remove queries and rearrange their order, pick any available
resource and metric per query.*

A Grafana dashboard is more than just a static visualization. It allows
you to explore data and jump to Azure when further investigation is
needed. Examples are links to more elaborate logging or to Azure DevOps
pipelines for deployments that incurred diminished performance to
initiate a rollback. This makes a Grafana dashboard truly a starting
point when issues arise. Quick insight can be gained into key
indicators. When something strange is noted, one can explore the
particular query, see what is going on and jump directly to the
particular Azure resource via a link provided in Grafana.

### Community

Since Grafana is a specialized cross-cloud monitoring tool, it has
gained a lot of traction and contributions from the community. This can
be seen in the great amount of available monitoring templates. An
abundance of optimized dashboards can be found at
[[https://grafana.com/grafana/dashboards]{.underline}](https://grafana.com/grafana/dashboards/),
where each dashboard is given a unique dashboard identifier. This ID can
be specified in the import functionality of Grafana and allows you to
easily add a Kubernetes and/or cost management dashboard to your
dashboard stack.

![](./media/image7.png)

![](./media/image8.png)


Step 1: Go to Dashboards/ Import Step 2: Fill in the ID of a dashboard

![](./media/image9.png)


Step 3: Check the details of the dashboard and Import.

These template dashboards are great examples and provides a running
start when someone is new to Grafana. They give a good example of what
is possible for the specific data sources and can be tweaked to personal
preference after import.

The lively and expanding Grafana community works as a catalyzer for
adoption by more and more companies worldwide. It really looks like
Grafana is *the* visualization weapon of choice in the Kubernetes realm
for example. See for example this article: [[How To Setup Prometheus
(Operator) and Grafana Monitoring On Kubernetes
(getbetterdevops.io)]{.underline}](https://getbetterdevops.io/setup-prometheus-and-grafana-on-kubernetes/)

### Dashboards-As-Code

One of the important aspects of working DevOps is to automate
everything. This includes dashboarding. Grafana dashboards can be
created from a JSON template using the Grafana API.

By treating your dashboards as code and including them in the DevOps
work process you get all the advantages of automatic deployment, git
versioning, collaboration *et cetera.* Let's have a more hands-on look
of how this could work.

Once a dashboard has been set up, it can be exported with the following
steps:

1.  Open the dashboard

2.  Go to Dashboard settings

> ![](./media/image10.png)
> 

3.  Open JSON Model

![](./media/image11.png)


4.  The Dashboard-As-Code can be copied.

Following these steps we can set up **modular dashboards** that
integrate with miscellaneous services, such as:

-   Azure DevOps

-   App Services

-   Azure Kubernetes Services

-   Azure Monitor

After setting up an insightful dashboard, the environment specific
parameters are parametrized, such as the subscription ID, resource group
and managed identity used to obtain the data. By doing this, the
dashboard can be reused for multiple scenarios, environments, or
customers. By repeating this for different dashboards, a pipeline can be
setup that combines the deployment of the different dashboards. This is
done as follows. First create a pipeline template to which the different
dashboard panels can be fed.

parameters:

\- name: appService

type: object

default: \'\'

\- name: kubernetes

type: object

default: \'\'

\- name: costManagement

type: object

default: \'\'

\- name: namespace

type: object

default: \''

Subsequently, the Grafana pipeline template can be called from the
environment-specific pipeline where parameters (e.g. a list of App
Services and corresponding resource group) are passed to this pipeline.
The pipeline then checks if parameters are set for that specific
component (using the condition: neq(variables.appService, \'\')syntax),
sets the variables in the appService.grafana file and adds it to the
total Grafana file that will be deployed. A portable dashboard is
thereby obtained which only has to be devised once. Moreover, when
changes are made in a customer dashboard, this can be put back in the
infra-as-code such that it can be rolled out for other customers too.
This way, dashboards can be incorporated in the DevOps workflow.

This is not only useful for us as managed services providers, but also
for larger companies with multiple DevOps teams. These practices help to
standardize the look and feel of dashboards and to create a uniform
dashboard experience. Incorporating the dashboard-as-code technology
allows companies to create well-designed dashboards and share them
across teams for reusability and efficiency.

#### Costs 

When this article is printed, Azure Managed Grafana has an hourly rate
of €0,088 per hour per instance. Active users (users who accessed
Grafana in a given month) are less than € 6,00 per month. So with +/- €
75,00 a month (and a 30-day free trial) the initial investment to start
with this offer is fairly low. The Azure costs should not be a barrier
to exploring this tool.

An interesting difference with other observability tooling is that
Grafana does not need to scrape data to its own database. Instead, it
talks directly to the APIs of Azure (Microsoft Graph).

This has three big advantages. Firstly, this prevents egress data costs
from Azure to the external tool; as Egress Data is invoiced per GB this
can cause unhappy surprises when such a tool is implemented.

Secondly no additional agents need to be installed on the Azure
resources that could have an impact on performance and maintenance.
Lastly there is almost no delay in data availability between Azure and
Grafana. As soon as the data becomes visible in the Azure Portal, it is
available as well in Grafana.

## Conclusion

Azure managed Grafana allows us to bring dashboards to the next level
and reuse dashboards that have been well-designed. The ability to
concentrate and retrieve useful information at a centralized place
greatly helps in the debugging process in case of technical errors.

Moreover, one can easily include information that is otherwise scattered
across cloud services. Billing information is collected in Azure Cost
Management, SLOs are defined in terms of KQL queries, and Kubernetes
performance is monitored under the Azure Kubernetes Services monitoring
tabs. Now all information is leveraged in a single place, where multiple
dashboards, all with their own audience, can be defined.

By bringing all these data sources together and combining them in a
smart and useful way in Azure Managed Grafana, observability becomes
something real and will add value to the existing tools that Azure
offers. In the current age of highly complex distributed systems it is
no longer about only trying to *prevent* issues from occurring, but to
make sure engineers have the right tools to literally *observe* what is
going on and to locate the issues as quickly as possible.

If you're interested in Azure Managed Grafana and would like us to help
you with designing fit-for-purpose dashboards or would like to get a
workshop on how to do this yourself, don't hesitate to reach out to us!

[^1]: ^Majors\ C.,\ Fong-Jones\ L.,\ Miranda\ G\ (2022).\ *Observability\ Engineering.\ Achieving\ Production\ Excellence*,\ 4^

[^2]: ^Majors\ C.,\ Fong-Jones\ L.,\ Miranda\ G\ (2022).\ *Observability\ Engineering.\ Achieving\ Production\ Excellence*,\ 13^

[^3]: ^Majors\ C.,\ Fong-Jones\ L.,\ Miranda\ G\ (2022).\ *Observability\ Engineering.\ Achieving\ Production\ Excellence*,\ 13^

[^4]: ^See:^
    ^[Azure\ Monitor\ overview\ -\ Azure\ Monitor\ \|\ Microsoft\ Docs]{.underline}^
