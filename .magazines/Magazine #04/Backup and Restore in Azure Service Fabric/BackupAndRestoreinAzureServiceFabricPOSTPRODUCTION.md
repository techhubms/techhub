Creating and restoring backups of stateful services can be challenging.
In this article you will learn how it works and what you can do to make
this process much easier.

# What is Azure Service Fabric?

Before we start creating backups, I will first briefly introduce Service
Fabric and its application model. Most companies have many applications
to run, usually on multiple, over-dimensioned servers, sized on peak
loads. This means that most of the time, server resources are not
utilized efficiently. Service Fabric creates a virtual pool of computing
resources by joining multiple servers -- or nodes -- together into a
cluster. Service Fabric then adds mechanisms to optimize the use of the
underlying cluster resources. It automatically takes care of application
placement and upgrades, cluster health monitoring and rebalancing
applications, based on their resource consumption.

# Application Model

Service Fabric applications consist of one or more services that work
together to automate business processes. A service is an executable that
runs independently of other services, and is composed of code,
configuration and data. Each element is separately versioned and
deployable. In this model, 'code' means the service binaries. Xml files
which hold 'configuration' have custom service settings, such as
connection strings and security settings. Finally, 'data' is any static
data your service uses, e.g. pictures and script files.

Figure 1: Application Model

Creating an application instance requires an Application Type; this is
the template that specifies which service instances should be created as
part of the application. This concept is similar to object-oriented
programming. The Application Type is like a class definition, and the
application is a named instance. Multiple application instances can be
created from one Application Type.

![](./media/image1.png)


Figure 2: Types versus Instances

The same concept applies to services. A Service Type defines the code,
data and configuration for the service, as well as communication
endpoints used by the service for interaction. Multiple named service
instances can be created using one Service Type. An application
specifies how many instances of which Service Types should be created.

Both Application Type and Service Type are described through XML files.
Every element of the application model is versioned and deployed
independently.

## System Services

Service Fabric itself runs as services on the cluster. These system
services manage the cluster and are used to deploy and monitor the
services you run on the cluster yourself. One of the system services is
the Fault Analysis Service. This service plays a role in restoring
backups. You'll learn more about this service later.

# How Stateful Services work

Stateful services keep their data close by, stored in memory and on a
local disk. To enable large scale projects with many concurrent users,
stateful services can be distributed across multiple nodes. Each
instance of a stateful service is called a replica. Each replica stores
its own chunk of the total service state. This means that your data is
divided across multiple service replicas. All Replicas are made highly
available through an automated data replication system, which copies the
state across multiple cluster nodes during transactions. So if one
cluster node fails, your data will be safe and your service continues to
be available. It also means that you may need to query multiple Replicas
to get all your data.

Figure 3: Replicas of a stateful service

And finally, it means that if you want to create or restore a backup of
your service, you will need to do this separately for every partition.

# Creating a backup of a Replica

The state of your stateful service is safely stored across multiple
nodes. However, your data can still be lost, due to the loss of your
entire cluster, or to human error. To protect you from data loss it is
sensible to create regular backups of your service state.

## Backup types

There are two types of backups:

-   Full backups

A full backup contains a complete copy of the state of one replica. Full
backups can become quite large as your service state grows over time.

-   Incremental backups

An incremental backup builds on top of a full backup and any later
incremental backups. It contains only the changes made since the last
full backup. Because of this, a partial backup is usually smaller and
faster to create than a full backup.

Figure 4: Types of backups

How often you create backups depends on the acceptable amount of data
loss you can afford for your service. Mission-critical data and data
that constantly changes, should be backed up more often than other types
of data.

## Adding code to create backups

Creating backups of Service Fabric Services requires you to add some
code to your stateful service. First of all, you will need to call the
existing method BackupAsync and pass it an instance of BackupDescription
which contains a callback. This callback -- or delegate -- will be
executed once the backup is created to perform additional actions, if
needed.

The callback delegate has the following signature:

The CancellationToken can be used to cancel the operation. The returned
Boolean indicates the success of the operation. The provided BackupInfo
class contains useful information, for instance the backup folder, its
type and the version. To keep your Backup files safe, you should copy
them away from the cluster to a central store, like Azure Blob Storage.
This way, you can recreate a lost cluster using the Backup files.

Remember that this code needs to be executed for each replica of your
service. To make this process reliable and repeatable, you could for
instance create a new Service Fabric service that periodically backs up
other services in the cluster.

# Restoring a backup of a Replica

Figure 5 shows the process of manually restoring a replica. The restore
process is more complicated than creating a backup. It requires the help
of one of the Service Fabric system services we discussed earlier; the
Fault Analysis Service.

Figure 5: Restoring a Replica using triggered data loss

## Triggering 'data loss'

To begin the process of restoring a backup, Service Fabric must invoke
the method OnDataLossAsync on your stateful service replica. This can
happen in the following situations:

-   The call to OnDataLossAsync can happen because Service Fabric
    detected data loss itself. This will happen if there is a problem
    with the cluster node that hosts the service. For instance, if there
    is a disk failure.

-   It can also be triggered by using code:

-   And finally, it can be triggered by using PowerShell:

## Cleaning existing data

The last two options are triggered by the program code; the Fault
Analysis Service is called from "under the hood", and it ensures that
the restore operation is performed for the targeted service replica. The
amount of data that will be lost by these operations depends on the
value of dataLossMode. It has two options:

1.  PartialDataLoss -- This option indicates that only pending
    replications will be lost.

2.  FullDataLoss -- This option indicates that all data is lost.

## Invoking OnDataLossAsync

After the data has been removed, OnDataLossAsync is invoked on the
replica, so it can begin to restore its state. It is invoked with a
parameter RestoreContext which contains an operation called
RestoreAsync. Before you can call this method to restore a backup, you
will need to download the backup files from your central storage back to
a temporary folder on the cluster node. Then you must let Service Fabric
know where to find the Backup files. For this, you can use the
RestoreDescription struct. The RestoreDescription that is passed to
RestoreAsync is used to inform Service Fabric where you have placed the
Backup files locally. When restoring a full backup, only the one folder
that contains it needs to be downloaded. When restoring an incremental
backup, all previous incremental backups [and]{.underline} the latest
full backup need to be downloaded. (As explained in Figure 4.)

## And now we wait...

After doing all this, Service Fabric will restore your Replica.
Depending on the size of the backup to restore, this can take quite some
time. You can monitor progress by using the Service Fabric Explorer. It
will start out with reporting errors, and show your stateful service
with status 'In build'. This can be seen in Figure 6.

![](./media/image2.png)After
a while your service will become healthy again. Remember that this code
needs to be executed for every replica of your service, just like the
creation of the backup. It's important to note that all replicas that
are not currently restoring a backup remain healthy and will continue to
operate normally.

# Help!

The process of creating and restoring backups and keeping them safe is
quite complicated. Fortunately, there are some open source initiatives
that can help you. For instance, there is an Azure Quick Start example
project that shows how to use Azure Blob Storage as a central backup
store in InventoryService.cs:

<https://xpir.it/mag4-sf1>

Another option is to use my open source library
"ServiceFabric.BackupRestore", available on GitHub[^1] and Nuget[^2].
This library provides a class called BackupRestoreService that is
derived from StatefulService. By using this class instead, you are
provided with three new methods:

1.  BeginCreateBackup -- this method calls BackupAsync on the service
    and uses an injected helper class to copy the Backup files away from
    the cluster.

2.  ListBackups -- this method lists information about all Backups that
    were created earlier using BeginCreateBackup.

3.  BeginRestoreBackup -- this method is called with the information
    listed by ListBackups and triggers data loss, copies the Backup
    files from the central store to the cluster node, and invokes
    RestoreAsync with the proper information.

By using this library, you can create and restore backups with just a
few simple lines of code. You can use the readme document and demo
application to help you get started.

# Conclusion

In this article, you have learned how to create and restore Backups for
your stateful reliable services in Azure Service Fabric. By creating
backups and storing them away from the cluster, you can deal with
disasters caused by full cluster failure and human errors, for instance
accidental deletes. Creating and restoring backups is complicated.
However, using existing code and libraries can make life simpler.

[^1]: https://xpir.it/mag4-sf2

[^2]: https://xpir.it/mag4-sf3
