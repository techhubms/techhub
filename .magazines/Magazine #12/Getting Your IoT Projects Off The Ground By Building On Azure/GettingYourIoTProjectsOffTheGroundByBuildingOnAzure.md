# Getting Your IoT Projects Off The Ground By Building On Azure

With the popularity of the Internet of Things, new proof of concepts and
prototypes are starting everywhere. If you're contemplating getting
started with IoT or need a nudge in the right direction, this article
will highlight some great options to get you started. Now, some projects
go nowhere, with others end up being very successful. But even in the
latter case, a new IoT platform will still fail if the wrong choices
were made in the technology selection, right at the project\'s
inception.

An IoT solution will have a couple of key components, even for a
prototype. There needs to be a robust messaging system in place. This
allows you to have bidirectional communication with your devices. You
also need to store a collection of devices you can trust and their
configuration to run correctly. These devices live at "the edge", a
collective term for anywhere from a factory, train tracks, or someone's
home. These devices could range from a tiny microcontroller to more
powerful computers running artificial intelligence workloads. Both will
send messages to the platform, and when these messages are received,
they are transformed, analysed, and visualised to extract insights from
the data. The information is then stored, preferably in different ways
for long-term storage vs storage that needs to be readily available.
With so many moving parts, choosing the right technology becomes
critical because it will impact your project\'s future.

One example of a project I\'ve seen came to a grinding halt through the
weight of its own complexity. A small IoT prototype was a success and
became part of the company\'s core business. But it simply wouldn\'t
scale any further than a couple of devices. The technologies used to
develop this project seemed \"fine\" at the time. Surely you can come
back to fix this, right? But a couple of years later, they\'re running a
custom message broker and a handful of databases and spreadsheets to tie
everything together. IT doesn\'t want to host their platform, and it\'s
now running in your \"private cloud\" in the attic.

The software described above is not an exaggeration, and I\'m sure there
are many more platforms out there like it. And who can blame the authors
of these projects? They might have been trying something new, using the
skills they had at that moment. When the prototype became a success, it
was put into production instead of turning it into a scalable solution
first. So how can you avoid making the same mistake?

## Build for success with Azure

Instead of building and designing everything from scratch, you can get a
head start by using Azure platform as a service (PaaS) components. These
are the same components used for global-scale IoT platforms, managing
millions of devices. While at first, this might sound like an excessive
measure for a prototype with just a few devices, the PaaS components in
Azure scale remarkably well. The best place to start for most platforms
is Azure IoT Hub. You can get started with a fully-featured IoT Hub for
about 20 Euros per month, and with 400.000 IoT messages per day, it will
be a long time before you have to scale it up. So even for a proof of
concept, you can spin up your own IoT Hub and save yourself the trouble
of hosting custom message broker, identity management, and message
routing solutions.

When using IoT Hub, you have many options to transform and analyse the
data you're receiving. A typical scenario with new projects is starting
with Azure Functions to transform and bring data from one place to
another. Moving to Azure Stream Analytics can be a great choice when the
requirements become more complex or need to consider time windows. It
allows you to run analytics over data streams and extract the most
critical insights. It also has built-in anomaly detection, a complex
feature to build from scratch.

Another great place to start is Azure IoT Central. This software as a
service (SaaS) product builds on top of IoT Hub and other Azure
components to offer a highly scalable product. You can be the proud
owner of an IoT Central instance in minutes for a few cents per month,
so pricing shouldn\'t be a limiting factor. It has dashboarding, device
registration, a ruling engine, and even some new multi-tenancy features
built-in. This means you can start to impress your organisation with a
complete IoT platform without reinventing the wheel. And if there are
features you need that aren\'t in Azure IoT Central, you can stream the
device data to your own software. Your IoT prototype became so
successful that the organisation wants to include the data into their
CRM platform? No problem, stream the data to Service Bus or Event Hub
for further processing or send it directly to an HTTP endpoint of your
choice.

In both cases, you get a huge jump-start in functionality and can get
started with something much more important: building the features only
you can create. You know your business better than anyone else, so build
on these world-class components and focus on what you do best. Building
an IoT platform shouldn't be about making all the plumbing, time and
again. It's about realising value.

## Cloud logic, at the edge

Following the advice above, you will have a great start in the cloud,
but IoT also involves devices. Your project could use off-the-shelf
hardware, but you might need a device that doesn\'t exist yet. Creating
IoT devices is usually done by professional device builders. Combining
electronics and writing code for microcontrollers is not a skill every
developer has. But that doesn\'t mean you can\'t build simple
prototypes. You can get started by building devices with Arduino or .NET
nanoFramework. The latter gives you a subset of the .NET CLR to write
software for microcontrollers in C#. Getting started with nanoFramework
is blazingly fast, and the different applications you can write with it
deserve their own article. The most important thing is both Arduino and
nanoFramework have many libraries available to do the heavy lifting, so
even on the edge, you're able to get started quickly.

But you might need more robust hardware. If you're running AI at the
edge or need to go beyond the constraints of a microcontroller, Azure
IoT Edge will accelerate your device solution. It allows you to write
device software in a language that you probably already use in your day
job. If you know .NET, Python, Node.js, Java or C, and have some
experience creating Docker containers, you have what it takes to be an
Azure IoT Edge developer. Another benefit of Azure IoT Edge is you can
use CI/CD to deploy updates to your device, so the development process
should be familiar.

Microsoft also supplies standard IoT Edge modules for Azure Functions,
SQL databases, Stream Analytics and more. Hence, like in the cloud,
build on these existing modules to avoid reinventing the wheel.

## Conclusion

Getting a new IoT project off the ground can be tricky. Starting with
small prototypes and proof of concepts is an excellent way of testing
the waters. Chances are, you already have what it takes to get started
on the edge. There have never been more options for software developers
to get involved without much embedded development experience, be it
microcontrollers or edge computing devices. And when you start by
building on the same secure, reliable and high-performing cloud
components that support millions of devices worldwide, you can focus on
what makes your project unique. And when the time comes to scale up your
platform, you won't ever have to run your platform from your attic.
