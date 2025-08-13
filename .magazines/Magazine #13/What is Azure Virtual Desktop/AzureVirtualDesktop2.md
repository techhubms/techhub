# What is Azure Virtual Desktop (AVD)

Azure Virtual Desktop is Microsoft's desktop and app virtualization
Virtual Desktop Infrastructure (VDI) service that runs on Microsoft
Azure. Azure Virtual Desktop allows for multi-session virtual machines
with scalability on Azure. As an administrator, it enables you to have a
single management experience for multiple Windows operating systems.

With Azure Virtual Desktop, organizations can centrally provide users,
both internally and externally, with a full desktop and applications
hosted in the Microsoft Azure cloud. This allows organizations to
leverage the scalability of the Microsoft Azure cloud for your desktops
and desktop applications.

Some key features that Azure Virtual Desktop brings organizations:

-   Gateway functionality included in the default Azure Virtual Desktop
    setup\
    *You do not have to worry about the connections to the Azure Virtual
    Desktop machines; this is all integrated into the service.*

-   You can use your own images from your shared image gallery\
    *Using your own images will accelerate your machine readiness. You
    can even share images across your services and even Microsoft Azure
    tenants.*

-   Option to create pooled (shared), personal (private) machines, and a
    mix of both\
    *The option to choose the machine types gives you the freedom to
    supply Azure Virtual Desktop machines based on your user
    requirement.*

-   Autoscaling to increase or decrease resources based on time of day,
    days of week, and on-demand\
    *Scaling itself gives you the flexibility to supply the right amount
    of machines that you need at any given moment. Adding automation
    will ensure that you do not have to worry about your capacity.*

Azure Virtual Desktop can be used with either the Remote Desktop Client
(available on multiple platforms) or the HTML5 web client.

![Graphical user interface, application Description automatically
generated](./media/image1.png)


# Azure Virtual Desktop use cases

Azure Virtual Desktop is the cloud version of Remote Desktop Services
(RDS), a Virtual Desktop Infrastructure service. A Virtual Desktop
Infrastructure service is commonly used for a fully working business
desktop with all the required applications for your business. This means
that it can create a fully working workplace for your employees or
contractors in the cloud without having to invest in physical hardware
that is capable of running everything locally.

A well-known provider of Virtual Desktop Infrastructure services is
Citrix (Virtual Apps & Desktop). Citrix is a more advanced solution that
provides more advanced options for a Desktop-as-a-Service (DaaS)
experience. Being more advanced also means that more setup and
configuration effort is required to make it work.

Since Azure Virtual Desktop can also host and present an application to
end-users and give them a seamless user experience (you experience the
application as if it runs locally on your machine), this will mean that
you, as a business, can distribute your application online with the
scalability and flexibility of the Azure cloud and the control of your
application. Although you could use this as an extension of your full
desktop experience, this is also the basis for a niche use case.

What we mean with a niche use case for Azure Virtual Desktop is that
your organization can provide centrally managed applications to your
end-users, even though your application might not be fully cloud-native
(yet). This use case could help organizations move to the Microsoft
Azure cloud.

# Azure Virtual Desktop and our customers

Some customers have an application that is not cloud-native (yet), some
customers want control over the installation and configuration of their
application, and some want to not burden their customers with
application configuration or setup.

We have encountered these different reasons at our customers, and we
have helped tackle them with Azure Virtual Desktop. The applications are
still being improved to be cloud-native, but while the development and
improvements are performed, we can move to the cloud quicker while
adding scalability and flexibility in the Microsoft Azure cloud.

## How Kongsberg Digital leverages Azure Virtual Desktop

Among other industries, Kongsberg Digital is a key player in the
maritime industry. Kongsberg Digital is constantly thinking of
innovative solutions for the Maritime simulation industry and its
instructors, students, organizations, and users. They have innovative
simulation solutions which are available both online and offline.
Kongsberg Digital Maritime simulations are part of the K-SIM product
line.

One part of the K-SIM product line is the K-SIM Instructor, where
instructors and trainers can create simulation exercises for students to
execute and perform. The K-SIM Instructor application is a Windows
desktop application for instructors to use locally. The installation and
use of the K-SIM Instructor application requires other K-SIM tools,
models, and area charts. These requirements add complexity for the
instructors. Azure Virtual Desktop helps Kongsberg Digital Maritime to
take ownership of the installation and configuration part of the K-SIM
Instructor application and the end users don't need to install anything
on their machines.

The K-SIM Instructor application also has strict compatibility
requirements with the simulator engine for the students to execute and
perform the simulations in the K-SIM Connect cloud environment. Since
Kongsberg Digital Maritime is in control over the complete installation
and configuration of both the K-SIM Instructor application and the
simulator engine versions, the compatibility requirements are centrally
managed and guaranteed compatible.

# Azure Virtual Desktop as a cloud accelerator

![Timeline Description automatically generated with medium
confidence](./media/image2.png)
Azure Virtual Desktop is a quick and
straightforward way to start your cloud journey by providing a solution
for your traditional desktop applications. It is low maintenance,
deployable using Infrastructure as Code, and has no or low requirements
of client systems to start working. While using the Azure Virtual
Desktop service, you will improve your organization's knowledge and
experience what the Microsoft Azure cloud can offer your organization.

The time-to-market is very fast while adding many other cloud benefits
at the same time.

# What services do you need for Azure Virtual Desktop

Azure Virtual Desktop is a service that handles the management layer for
providing a Virtual Desktop Infrastructure service. The Azure Virtual
Desktop service consists of a few main areas:

![Diagram illustrating the relationships between key logical
components.](./media/image3.png)


-   Workspaces\
    *Workspaces are a logical group of one or more application groups.*

-   Application Groups *\
    Within Application Groups, the applications and desktops are
    configured, including user and group assignment. The Application
    Groups are logical groups of installed software on the sessions
    hosts within the host pool(s)*

-   Host Pools\
    *The Host Pools are one or more pools of session hosts providing the
    computing power for the application groups.*

-   Scaling plans\
    *Scaling plans are used for ramping hosts up or down based on
    session usage and peak- and off-peak hours.*

An Azure Virtual Desktop host pool requires either a Virtual Machine
service or Virtual Machine Scale Set service to provide the computing
power on which the applications are installed and running. These
services require a Windows Image, which we build using the Azure Image
Builder service. We store this Windows Image within a Shared Image
Gallery, as a Virtual Machine Template.

![Diagram of an Azure Virtual Desktop service
architecture.](./media/image4.png)


Figure 2 Example Azure Virtual Desktop Architecture

The identity provider for Azure Virtual Desktop can be either Azure AD
or a traditional Active Directory Domain Services domain. When using a
*pooled* host pool, the user can log in to any of the hosts that are
available. To make sure the user experiences their profile the same on
all the hosts, we are using FSLogix profile containers, which are stored
on an Azure file share.\
FSLogix enhances and enables user profiles in Windows remote computing
environments and is able to supply profile containers, office
containers, and application masking.

# Can I try it myself?

Yes, you can! We have set up a [GitHub
Repository](https://github.com/XpiritBV/azure-virtual-desktop) for you
to deploy Azure Virtual Desktop with Bicep.

![](./media/image5.png)


[Our Azure Virtual Desktop GitHub
repository](https://github.com/XpiritBV/azure-virtual-desktop)[^1]
contains all the bicep files you need to run an Azure Virtual Desktop in
the cloud with a default app.

[^1]: <https://github.com/XpiritBV/azure-virtual-desktop>
