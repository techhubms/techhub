Installing Cloudera on Azure

In order to help a client to explore and analyze large amounts of
unstructured data, GoDataDriven installed a Cloudera cluster on
Microsoft Azure. In this article we discuss how to install Cloudera on
Microsoft's cloud solution.

INTRODUCTION

Processing large amounts of unstructured data requires serious computing
power, powerful database technology like Cloudera, and scalable
infrastructure. A cloud solution, like Microsoft Azure, that is able to
scale up and down over time and due to seasonal influences, can be a
flexible and cost-effective hosting solution.

Cloudera

Cloudera, founded in 2008, was one of the first organizations to develop
an Hadoop distribution. Today, Cloudera delivers a modern data
management and analytics platform built on Apache Hadoop and the latest
open source technologies. Cloudera Enterprise is the a fast, easy, and
secure data platform. Organizations that use Cloudera efficiently
capture, store, process and analyze vast amounts of data, empowering
them to use advanced analytics to drive business decisions quickly,
flexibly and at lower cost than has been possible before.

MICROSOFT AZURE

[[Microsoft Azure]{.underline}](https://azure.microsoft.com/nl-nl/) is a
cloud service for both infrastructure-as-a-service (IaaS) and
platform-as-a-service (PaaS), with data centers spanning the globe.

The following service offerings are relevant when deploying Cloudera
Enterprise on Azure:

-   **Azure Virtual Network (VNet)**, a logical network overlay that can
    > include services and VMs and can be connected to your on-premise
    > network through a VPN.

-   **Azure Virtual Machines **enable end users to rent virtual machines
    > of different configurations on demand and pay for the amount of
    > time they use them. Images are used in Azure to provide a new
    > virtual machine with an operating system. Two types of images can
    > be used in Azure: VM image and OS image. A VM image is the newer
    > type of image and includes an operating system and all disks
    > attached to a virtual machine when the image is created. Before VM
    > images were introduced, an image in Azure could have only a
    > generalized operating system and no additional disks. A VM image
    > that contains only a generalized operating system is basically the
    > same as the original type of image, the OS image. From one VM
    > image you can provision multiple VMs. These virtual machines will
    > run on the Hypervisor. The provisioning can be done or using the
    > Azure portal or with PowerShell or Azure command line interface.

-   **Azure Storage** provides the persistence layer for data in
    > Microsoft Azure. Up to 100 unique storage accounts can be created
    > per subscription. Cloudera recommends Premium Storage, which
    > stores data on the latest technology Solid State Drives (SSDs)
    > whereas Standard Storage stores data on Hard Disk Drives (HDDs). A
    > premium storage account currently supports Azure virtual machine
    > disks only. Premium Storage delivers high-performance, low-latency
    > disk support for I/O intensive workloads running on Azure Virtual
    > Machines. You can attach several Premium Storage disks to a
    > virtual machine (VM). With Premium Storage, your applications can
    > have up to 64 TB of storage per VM and achieve 80,000 IOPS
    > (input/output operations per second) per VM and 2000 MB per second
    > disk throughput per VM with extremely low latencies for read
    > operations. Cloudera recommends one storage account per node to be
    > able to leverage higher IOPS.

-   **Availability Sets** provide redundancy to your application,
    > ensuring that during either a planned or unplanned maintenance
    > event, at least one virtual machine will be available and meet the
    > 99.95% Azure SLA.

-   **Network Security Groups** provide segmentation within a Virtual
    > Network (VNet) as well as full control over traffic that ingresses
    > or egresses a virtual machine in a VNet. It also helps achieve
    > scenarios such as DMZs (demilitarized zones) to allow users to
    > tightly secure backend services such as databases and application
    > servers.

DEPLOYMENT MODES TO START A CLUSTER

At the moment Azure has two deployment modes available:

1.  ASM (Azure Service Management)

2.  ARM (Azure Resource Manager)

The ASM API is the "old" or "classic" API , and correlates to the [[web
portal]{.underline}](http://manage.windowsazure.com/). Azure Service
Management is an XML-driven REST API, which adds some overhead to API
calls, compared to JSON.

The Azure Resource Manager (ARM) API is a JSON-driven REST API to
interact with Azure cloud resources. Microsoft recommends deploying in
ARM mode.

One of the benefits of using the ARM API is that you can declare cloud
resources as part of what's called an "ARM JSON template." An ARM JSON
template is a specially-crafted JSON file that contains cloud resource
definitions. Once the resources have been declared in the JSON template,
the template file is deployed into a container called a Resource Group.
An ARM [[Resource
Group]{.underline}](https://azure.microsoft.com/en-us/documentation/articles/resource-group-overview/) is
a logical set of correlated cloud resources that roughly share a life
span. Using the ARM mode you are able to deploy resources in parallel,
which was a limitation in ASM.

The new [[Azure Ibiza Preview
Portal]{.underline}](http://portal.azure.com/) is used to provision
Azure cloud resources with ARM instead of the ASM API. You are not
limited to the portal to deploy your templates. You can use the
PowerShell or the Azure command-line interface to manage all Azure
resources and deploy complete templates. The Azure CLI is based on
NodeJs and thereby available on all environments. Both ARM and ASM are
modes which can be configured using the CLI.

Resources deployed in the ASM mode cannot be seen by the resources
deployed in the ARM mode by default. If you want to achieve this, you
would need to [[create a VPN tunnel between the two
VNets]{.underline}](https://azure.microsoft.com/en-us/documentation/articles/virtual-networks-arm-asm-s2s/).

REQUIREMENTS & DESIGN

GoDataDriven's goal was to build a production ready Hadoop cluster,
including provisioning machines, that took client specific requirements
in account, including enabling single sign-on, the deployment of 3
master nodes to handle load expansions, Cloudera connected to the Active
Directory to authenticate users, assure access control using Sentry,
install RStudio and IPython on the gateway for analysis.

The following installation was designed:

![loudera Enterprise Data Hub](./media/image1.png)


The GatewaySubnet is needed to set up the Site2Site VPN between the
client's network and the Azure network where the Hadoop cluster
resides.\
\
For user management two Active Directory servers were set up in their
own subnet, acting also as Domain Name Server.

**High traffic between the nodes in the cluster**

Because of the high traffic between all nodes in the cluster, the Hadoop
machines are in their own subnet. A reason for this is that when you
write a file into HDFS, this file is split into blocks (block size is
usually 128 MB) and these blocks are placed on the Datanodes. Each block
has a replication factor of 3. Only the master node (Namenode) knows
which block belongs to which file. The Namenode does not store blocks,
but it does maintain the active replication factor. If a client wants to
read a file from HDFS, it will first contact the Namenode, get the
location of the blocks and then read the blocks from the Datanodes. The
Datanodes send heartbeats to the Namenode and the when the active
Namenode notices that a block hasn't got the requested replication
factor, it instructs another Datanode to copy that given block.

There is also a ClientSubnet for the machines which can access the
cluster. Users can connect to the machines in this subnet, do their
analysis, but are not able to SSH to the machines in the Hadoop subnet.

Because of the single sign-on using the Active Directory on the Linux
level and configuring Kerberos using Active Directory for the Hadoop
services, users can use a single password everywhere.

HOW TO INSTALL CLOUDERA ON AZURE?

There are multiple way in which you can install Cloudera on Azure, of
two were considered:

1\. Install everything from scratch:

-   Provision machines and network using Azure CLI

-   Use a provisioning tool (like Ansible) to do the Linux configuration

-   Install Cloudera Manager

-   Install CDH (Cloudera Distribution for Apache Hadoop) using Cloudera
    > Manager

1.  Using the ARM template that Cloudera provides to install a Hadoop
    > cluster. This template, available on GitHub, includes OS and
    > network tuning and Hadoop configuration tuning. There is also an
    > Azure VM image, available on the Azure Marketplace, built and
    > maintained by Cloudera which is used during deployment.
    > Out-of-the-box features of the template:

    -   Create a Resource Group for all the components

    -   Create VNet and subnets

    -   Create availability sets. Place masters and workers in different
        > availability sets

    -   Create security groups

    -   Create Masternode and Workernode instances using the Cloudera VM
        > Image (CentOS image built and maintained by Cloudera). The
        > template automatically uses Azure DS14 machines, which are the
        > only machine types recommended and supported by Cloudera for
        > Hadoop installations.

    -   For each host a [[Premium Storage
        > account]{.underline}](https://azure.microsoft.com/en-us/documentation/articles/storage-premium-storage-preview-portal/) is
        > created

    -   Add disks to the machines, format and mount the disks (10 data
        > disks of 1 TB per node)

    -   Set up forward/reverse lookup between hosts using /etc/hosts
        > file

    -   Tune Linux OS and network configurations like disable SELinux,
        > disable IPtables, TCP tuning parameters, disable huge pages

    -   Set up time synchronization to an external server (NTPD)

    -   Set up Cloudera Manager and the database used by the Cloudera
        > Manager

    -   Set up Hadoop services using the Cloudera Python API

One of the disadvantages of the template is that it is meant to start up
a cluster, but you cannot create extra data nodes and add them to the
cluster. The template does not provision a gateway machine for you.

After analyzing the gaps between the template provided by Cloudera and
the client requirements, a golden middle-way was chosen:

-   Use the Cloudera-Azure template to provision the network, set up the
    > machines, configure the OS and install Cloudera Manager

-   Use Cloudera Manager (so not the Cloudera-Azure template) to install
    > the CDH cluster.

Best practices for a manual implementation

If you would not use the template, it is advisable to keep the following
best-practices in mind:

-   When deploying a Linux image on Azure there is a temporary drive
    > added. When using the DS14 machines the attached disk on
    > /mnt/resource is SSD and actually pretty big (something like 60
    > GB). This temporary storage must not be used to store data that
    > you are not willing to lose. The temporary storage is present on
    > the physical machine that is hosting your VM. Your VM can move to
    > a different host at any point in time due to various reasons
    > (hardware failure etc.). When this happens your VM will be
    > recreated on the new host using the OS disk from your storage
    > account. Any data saved on the previous temporary drive will not
    > be migrated and you will be assigned a temporary drive on the new
    > host.

-   The OS root partition (where also the /var/log directory resides) is
    > fairly small (10GB). This is perfect for an OS disk, but Cloudera
    > also puts the parcels (an alternate form of distribution for
    > Cloudera Hadoop) on /opt/cloudera and the logs into /var/logs.
    > These take up quite a lot of space so a 10 GB disk is not enough.
    > That's why you should move the parcels and the log file to a
    > different disk. Normally the template takes care of this for you.
    > If you install Cloudera without moving these files to a different
    > disk, you will see warning messages in Cloudera Manager that there
    > not enough free disk space available.

-   In a distributed system, thus also for a Hadoop cluster (especially
    > if Kerberos is used), time synchronization between hosts is
    > essential. Microsoft Azure provides time synchronization, but the
    > VMs read the (emulated) hardware clock from the underlying Hyper-V
    > platform only upon boot. From that point on the clock is
    > maintained by the service using a timer interrupt. This is not a
    > perfect time source, of course, and therefore you have to use NTP
    > software to keep it accurate.

-   If running Linux on Azure install the [[Linux Integration Services
    > (LIS)]{.underline}](https://www.microsoft.com/en-us/download/details.aspx?id=46842) -
    > a set of drivers that enable synthetic device support in supported
    > Linux virtual machines under Hyper-V.

Conclusion

Azure is a scalable and open cloud solution that can be configured to
meet specific requirements. When provisioning a Hadoop cluster like
Cloudera, the available Cloudera template is a great tool. If you do not
have too many exotic requests you can use the template as is, but if you
have special requirements, the template also makes for a great
foundation that can be modified to your needs.
