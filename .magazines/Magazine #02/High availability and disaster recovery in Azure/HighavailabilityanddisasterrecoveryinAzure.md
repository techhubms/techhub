Loek Duys

# Introduction

Continuity is of vital importance for all companies, and today it is
extremely important that software runs properly at all times, i.e.
24/7/365. Companies such as Microsoft and Amazon sell their products in
many countries all over the world, and at any given time customers are
shopping. There's never a good moment for downtime, as downtime
immediately costs money. Many companies choose to move their software
into the Cloud, but how does this help? Applications must be tolerant to
failures at many levels. In order to build resilient, fault-tolerant
applications you need to think about two key concepts: high availability
(HA) and disaster recovery (DR). In this article you'll learn about
these concepts and what the Microsoft Azure Cloud-platform will offer
you in this context.

# High Availability

Figure 1: System Availability with two parts

The availability of a system is determined by the availability of its
parts, and the availability of the entire system is calculated by
multiplying the availabilities of each part in a chain of dependencies.
Take for example a system with two parts A and B, as shown above.
Because part A depends on part B, the availability of this system can be
calculated by multiplying the availability of the individual parts:

$$A\tau = A1.A2$$

This leads to a system availability of 89.1%, which means that the
overall availability is lower than that of the lowest part! It is,
therefore, important to know the availability of each part when you are
creating a system that is intended to be highly available.

High Availability is all about eliminating single points of failure in
every part of a system by introducing redundancy. This redundancy can be
achieved by replicating data and running multiple servers. This
increases fault tolerance in order to eliminate outage. In order to know
where these single points of failure are located, you need to know about
fault domains.

## Fault Domains

Consider an application running in a datacenter like Azure. Things may
go wrong in different areas: a virtual machine may fail or the entire
datacenter could go offline due to a power outage. Fault domains are
therefore layered, as shown in figure 2. Obviously this is a simplified
view, because every layer comprises multiple other layers, e.g. power
supplies, network switches, hypervisors, etc. From this perspective, an
application can be made highly available by running it in multiple
datacenters -- perhaps even using multiple cloud vendors - using
multiple virtual machines running in separate server racks.

Figure 2: Layered Fault Domains

# Disaster Recovery

Disaster Recovery assumes outage and describes ways to deal with an
outage. An example of this would be to restore a database backup to a
new server after the original database experienced hardware failure.

## RTO

A relevant aspect of Disaster Recovery is the maximum accepted time it
takes to recover from a disaster until the system is once again fully
available. This is called the Recovery Time Objective (RTO). For
example, if it takes a day to set up, configure and start using a new
database server after a disaster has made the original database
unavailable, the Recovery Time would be a day. If the RTO is equal or
larger than one day, you meet this objective.

## RPO

The maximum accepted amount of data loss after a disaster is called the
Recovery Point Objective (RPO). For instance, if a database is
(incrementally) backed up every 30 minutes, the Recovery Point value
would also be 30 minutes. If your RPO is equal or larger than 30
minutes, you also meet this objective.

Both values are shown in figure 3. Your system should be built and
configured to meet the Recovery Point and -Time objectives.

![](./media/image1.emf)

Figure 3: RPO and RTO

If you want mission-critical applications to be highly available and
tolerant to failures, both of these values must be close to zero.
However, costs will multiply as you move closer to zero.

# Operations

Managing High Availability (HA) and Disaster Recovery (DR) for
enterprise systems can be a daunting task for IT-pro's. It involves
managing large amounts of servers and data, often distributed over
multiple datacenters and dealing with large volumes of data. Fortunately
the Microsoft Azure platform reduces a lot of this complexity because it
offers many HA and DR features as a service.

We will now describe some strategies you can use to deal with such
disasters and that will help make your mission-critical application
highly available and tolerant to disasters. We will do so by using a
fictitious web shop as an example project.

# ACME Global Shopping

Introducing, 'ACME Global Shopping'. A globally available web shop that
'sells everything'.

For ACME it is of vital importance that their web shop is available for
shoppers around the globe. They chose Microsoft Azure as a platform for
running their software. Figure 4 shows a simplified schematic overview
of the deployment model of their.

ACME has a website, which -- for write operations -- is loosely coupled
with two back office systems by means of a queue. Data is then written
to several types of data stores and an external analytics service. Read
operations are separated from write operations, and are performed on a
specialized data store.

![](./media/image2.emf)

Figure 4: System Conceptual Deployment Model

In order to make its web shop 'highly available', ACME created the
following configurations for the various components in their
architecture. When running this architecture on Azure provides you will
get some excellent HA and DR features straight out of the box. Some
other features, such as the use of the CQRS design pattern (separating
reads from writes) were implemented by ACME employees.

## Data Storage

ACME faced a number of problems in earlier versions of their systems.
All data was simply stored in one SQL Server Database. They were
reaching the limits of their server, queries were running slower as the
customer base increased and the amount of stored data grew. ACME
realized it needed to start storing data based on the characteristics of
its use. For instance, the product catalog needs to be highly optimized
for search operations, but customer data will mostly be queried by user
name. The product catalog changes every day, but data about a customer
hardly ever changes. Based on these very different types of use, the
most efficient storage type can be selected. For now, we will limit our
scope to just two types of storage: relational storage (SQL) and
key/value-set storage (NOSQL). When thinking of your own storage
policies, you will definitely need to consider document databases as
well as blob storage.

### SQL

The advantages of using a SQL data store is that it supports
transactions and rich querying. It works well for relational data, but
unfortunately it has a number of disadvantages: limited performance,
limited scalability and relatively high costs.

ACME has chosen Azure SQL Database for relational storage, which is
Microsoft's Database As A Service Platform.

For High Availability, Azure SQL Database data is replicated over three
nodes. An SQL transaction only completes when the data is persisted in
at least two physically separated nodes. As a result, if a node fails --
perhaps due to hardware failure -- it can be recovered using the
remaining synchronous node without downtime and without resulting in a
single point of failure.

To ensure Disaster Recovery, you can create and restore backups of the
data, in order to deal with data loss due to application errors or user
errors. Creating backups can be done in multiple ways, depending on the
selected database pricing tier and of course configuration. Backups are
created automatically, and the files are stored in Globally Redundant
Storage by default. It is also possible to manually create backups to be
stored in a Blob container in a Storage Account.

Restoring backups can also be done in several ways. First, there is
Point in Time restore according to which your database is backed up on a
regular interval. The backup files are persisted up to 35 days. If
required you can restore your database using the backup of any of those
intervals. This is often used to compensate for human errors.

Another option is Geo Restore, which will copy database backups to
geographically redundant storage. By having your backups available in
multiple regions, the system can restore your database in a different
region when the primary region becomes unavailable.

Finally there is Geo Replication, which comes in two flavors: Standard
and Active. Standard Geo Replication will create a replica of your
database in the paired Azure region, but this database is not
accessible. If the primary region fails, the replica can be used as a
fail-over, but this process is not automated. Active Geo Replication is
similar to Standard, but allows up to four readable replicas. The
read-only replicas provide additional capacity for read-only queries.

### NOSQL

The main advantage of using a NOSQL data store is the processing speed.
Queries and write operations are generally a lot faster than their
SQL-based counterparts. Also the costs for both transactions and storage
are significantly lower. Data is partitioned by default, which results
in substantial scalability. The downside is that query support is rather
limited. Queries perform best when done using the partition/row keys, in
order to avoid table scans across partitions which may reside on
multiple machines. In addition, transaction support is limited to
records within one partition.

ACME relies on Azure Table Storage for persistence of their
non-relational data.

Data stored as part of Storage Accounts is also replicated by default to
make it highly available. In this case, one of the options is Locally
Redundant Storage (LRS), in which three synchronous copies are made in
the same datacenter. You can also configure data to be replicated in an
additional region to make it durable even when a complete datacenter
goes down. This is called Geo Redundant Storage (GRS). The additional
non-readable copies are created asynchronously with an RPO of 15
minutes. This means that there is no additional latency, but if a
datacenter goes down, the data in transit could be lost. In this
context, it is important to note that different partitions may sync at
different speeds, so cross-partition operations on the primary or
secondary datacenter may yield different results after disaster.
Optionally, the replica data in the paired region can be made accessible
for read access (Read Access GRS, or RA-GRS), providing a means to run
your application in a reduced functionality mode. However, write
operations are not possible at that time.

Only if Microsoft declares a datacenter as being lost will your paired
region become the primary region, but there is no SLA on the time
between disaster and this decision, so the RTO for this situation is
undefined.

If you feel you need to have a close to zero RPO & RTO with full control
over your Disaster Recovery while using Table Storage, you will need to
write custom code to have clients access multiple datacenters for each
write operation and deal with disasters. For inspiration, have a look at
Microsoft Research's '*Replicated Table Library for Microsoft Azure
Storage'* on GitHub[^1]. Note that this will increase both latency for
write operations and storage costs.

In order to deal with user errors, you could use tools such as
AzCopy[^2], which is a command-line utility that will let you copy data
to and from Table Storage tables (or blob and file storage). It supports
exporting table entities into CSV files, as well as copying data into
different regions.

## Caching

By carefully thinking about what data store to use, ACME systems is back
on track. However, the new architecture still contains SQL Server
database servers, which, as we now know, have limited scalability and
performance. So, whenever an expensive query is run and the results are
reusable for other customers, it makes sense to hold on to the results
for a while. The same thing goes for data that is more or less the same
for every customer, such as items from the product catalog. Replacing
unnecessary queries on the database with queries from memory (caching)
saves database server resources and therefore money. Caching can also
help make your systems' availability higher because it reduces the
dependency on the data stores. The caching strategy itself does not play
an active role in Disaster Recovery, but there are caching systems that
offer some support to rebuild the cache after disaster by persisting the
data durably. In order to keep your system highly available, a cache
should not be a single point of failure. This is why distributed caching
exists.

For read operations performed by the Query Processing components, ACME
put a Redis Cache (Azures' Distributed Cache as a service) in place as
one of the Query Stores.

The Azure Redis Cache Service is an in-memory cache that can hold up to
530GB of memory. It can be used in three pricing tiers: Basic, Standard
and Premium.

The Basic tier has one node -- there is no formal SLA for this tier and
it runs on shared infrastructure, which is why usage is recommended for
non-critical workloads.

The Standard tier consists of two nodes -- one master and a slave to
make it highly available. There is a 99.9% SLA for this tier.

The most expensive tier is Premium. It builds on the Standard tier and
adds the data persistence capability, which periodically stores a
snapshot of the entire cache in a (Premium) Storage Account in the same
Region, in order to recover from when disaster strikes all nodes. Please
note that there is currently no way to rebuild a cache using data from a
secondary datacenter, so if the datacenter is lost, the cache will still
need to be rebuilt in another way.

The Premium tier also adds Redis Clustering, which enables one cache to
be shared across up to 10 nodes, thus allowing more data to be stored.
Finally, it adds Virtual Network support to isolate the cache nodes and
restrict access to virtual network clients.

From the Standard tier up, when accessing the cache, you are actually
talking to a load balancer, which will redirect your operation to the
master node. Whenever the master node fails, the load balancer will make
sure a healthy slave node becomes master. This makes the cache highly
available.

ACME decided to use the Standard tier; having a highly available,
distributed disposable cache works sufficiently for their needs.

## Service Bus

ACME uses Azure Service Bus for reliable messaging. This will ensure
guaranteed ('at least once') delivery of messages from the website to
the Command Processing components. This creates a loose coupling,
allowing the front end to stay operational even when the Command
Processing is not.

When a message is put on the Service Bus and received by the service,
the message is first persisted in a messaging store. By default, this is
one SQL Database per queue. Just like a custom Azure SQL Database, the
data is replicated three times for High Availability. For high
performance scenarios a different dedicated messaging store is
available, which is similar to the store that is used for Event Hubs.

To make a Service Bus Message Queue more fault tolerant, it can be
configured to be a partitioned queue. This will use a separate store for
each partition, and messages can be directed into those partitions. Not
only will this perform better, but partitions will also serve as backups
for each other. When the system fails to store a message in one
partition, it will try another. Only if all partitions fail to accept
the message, the message is rejected.

Because Service Bus Namespaces are tied to regions, you will need to
configure two identical queues in two regions to recover from
datacenter-level disasters. Your message-sending application needs to
decide whether to put messages in both (active / active) or the last
known working namespace (active / passive). For Service Bus Relay only
the latter option should be used. Your receiving application will need
to be capable of receiving messages from both and must have some
de-duplication capabilities (use message id, or correlation id for
this). The downside of this approach is having multiple Service Bus
resources, which incurs double costs for ownership and usage.

As an alternative, the message-sending application could also opt to
store messages in a local store (e.g. MSMQ), until the datacenter
becomes available again.

## Web Apps

The ACME website is running as a Web App, using multiple instances.
Websites run as part of Azure App Services. Several pricing tiers are
available here: Free, Shared, Basic, Standard and Premium.

Free and Shared allow you to host apps in a shared environment. These
very limited environments are intended to be used for dev/test scenarios
and offer no SLA.

Starting from the Basic tier up, there's a 99.95% SLA. It is also
possible to configure multiple instances of one app here, which means
that single points of failure can be eliminated. Another nice feature is
automated backups, which allows backing up files and databases to a
storage account. These backups can be restored to recover from a
disaster.

The Standard tier adds Geo-distributed deployment to make your Web App
highly available. This allows the deployment to take place in multiple
fault domains at the highest level (datacenter). When used together with
the globally available Traffic Manager service (that routes requests to
the different datacenters), a fully duplicated environment can be
created. Traffic Manager itself is a distributed, highly available
service, so it won't be a single point of failure.

You also get up to five deployment slots that can be used in a
deployment pipeline. ACME uses this feature to perform automated User
Interface testing on a staging environment before taking new versions of
their web shop into production, thus preventing data loss due to bugs.

The Premium tier adds an isolation feature called App Service
Environment. This provides an isolated, highly available environment for
securely running App Service apps on a large scale. They can be created
inside a virtual network spanning multiple Azure Regions and can contain
up to 50 dedicated computing resources. Access to and from the virtual
network can be controlled on a fine-grained level, significantly
increasing application security.

For optimal availability performance and security, ACME has opted for
the premium tier. Figure 5 shows their conceptual architecture. For the
sake of simplicity, only four Web App instances are displayed. In
reality, this number would probably be higher for a popular Web site.

Figure 5: Conceptual architecture (Front End)

By putting every App instance into an App Service Environment, access to
the API instances is limited to the Web App calling it. External parties
are blocked from accessing it. This adds an additional security layer
and prevents unauthorized use of the API.

#### Upgrade Domains

When upgrading a Web App or the underlying operating system
simultaneously on all instances, your application is rendered
unavailable. In order to avoid this, the logical concept of 'upgrade
domain' was invented. Software running in one upgrade domain will never
be updated together with software in another upgrade domain. By having
more than two upgrade domains, upgrading one domain will not result in a
single point of failure. However, disasters may still occur on the
remaining instances, so it is better to have even more upgrade domains.
Generally five upgrade domains are used. In the context of the
architecture shown in figure 5, every combination of Web App and Web Api
(1, 2, 3 and 4) could be in an upgrade domain. This would mean that at
least 75% of capacity is available during an upgrade, assuming no
disasters are happening of course!

## Cloud analytics

No highly available system is complete without proper analytics. After
all, how would you know whether your system is down if nobody is
watching it? You don't want to wait for an unhappy shopper to call and
tell you your site is not working. You need to respond quickly and
effectively at the first signs of trouble, and fortunately Azure also
provides a nice feature for this. It is called Application Insights --
an analytics service that can monitor your application. It provides two
main features: analysis of usage patterns and detection of performance
issues. The collected data can be analyzed using the Azure Portal, but
also using Microsoft Power BI, which is a great tool to analyze and
visualize data. Its SDK allows you to trace usage and error events from
every layer in your application and correlate them. Check out the
previous issue of this magazine if you want to learn more!

## Conclusion

In this article we've discussed a number of ways to make resilient
applications that are tolerant to failures on multiple levels, and how
using the Azure platform can help you to realize this.

*\>\>\>\>\>\>\>\>\>\>end\>\>\>\>\>\>\>\>\>\>\>*

*Some quotes:*

-   *Many companies choose to move their software into the Cloud. But
    how does this help?*

-   *High Availability is all about eliminating single points of
    failure, by introducing redundancy.*

-   *Disaster Recovery assumes outage and describes ways to deal with
    that.*

[^1]: <https://github.com/Azure/rtable>

[^2]: <https://azure.microsoft.com/en-us/documentation/articles/storage-use-azcopy>
