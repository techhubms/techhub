*By:*

-   *Gullik Anthon Jensen (Technology Director @ Kongsberg Digital)*

-   *Roy Cornelissen (Consultant @ Xpirit, working with Kongsberg since
    2017)*

-   *Sander Aernouts (Consultant @ Xpirit, working with Kongsberg since
    2017)*

## Introduction

For over four decades, students aspiring to become seafarers on one of
the world\'s many ships, either as a navigator on the ship\'s bridge, or
chief in the engine room, have studied and honed their skills on
Kongsberg's simulators. Believing that knowledge is instrumental to safe
and cost-efficient operation, Kongsberg has strived to remove the
limitations inherent in the students operating environment to enable
their customers, the instructors, and educators, to create any training
scenario. With the help of the simulators, the instructor can create the
most vivid and challenging experience for the students, such as
groundings, collisions, communication blackouts, system failures, or a
hundred-year storm.

The maritime industry is transforming, just like the transformation from
sail to steam, or from the compass to GPS, the future of the maritime
industry is autonomous ships, green shift, and remote operations. The
educators are not only faced with this challenge of transformation but
also the problem of digitalization. Students now take instant access to
digital services for granted. To bridge this gap, Kongsberg is deploying
a new platform using cloud technology to deliver traditional simulation
training in a new and different way. By combining proven and loved
simulators customers are confident and comfortable with and new
cloud-native technology, simulators become even more accessible anytime
and anywhere for students. Students can now keep building their
competency outside the classroom and be as prepared as possible for the
challenges that lie ahead.

We call this platform K-Sim Connect, a journey that started in 2017 by
moving Kongsberg\'s engine room simulator (ERS) to the cloud. In this
article, we will share the challenges we faced, the solutions that we
have chosen, and the lessons we learned.

## Moving a 30+-year-old software to the cloud

Kongsberg created the engine room simulator over 30 years ago and up
until this point had only been installed on-site at costumers. We aimed
to bring the engine room simulator to the cloud without having to change
it too much to enable both on-site and simulation as a service delivery
models.

Kongsberg\'s engine room simulator simulates the engine room of a
specific ship model. It thus allows students to learn, for example, how
to perform a cold start or emergency shutdown without physically being
in the engine room of the ship.

Students perform a specific exercise, such as an emergency shutdown, and
the simulator tracks their performance as part of an assessment.
Instructors use this assessment as a pass/fail criteria for classes and
specific certifications.

![](./1adbfc22a21237abbb3ad107dd1a0562ea82145b.png)


Figure 1: engine room simulator screen

Figure 1 shows a typical screen of the client application. The engine
room simulators consist of many of these screens. Each screen represents
specific controls that are in the actual engine room of the simulated
ship model. On a customer site, these digital screens can be replaced by
a physical replica of the ship\'s engine room to allow for an even more
immersive learning experience.

To understand what it takes to bring this simulator to the cloud, you
must understand the basics of how the engine room simulator works.

![](./def7693d606794469deb5bc4a5b8b099658fbaba.png)


Figure 2: ERS topology (simplified)

The engine room simulator is a client/server application where the
simulation of the state of the ship\'s engine room runs on a server
application (called simulator in figure 2). The students connect to this
server through a client application (see Figure 1). First, the client
and server perform a handshake process to determine the range of ports
used for further communication. Next, the client and server exchange
several messages over this range of ports.

Figure 2 assumes both applications run on a single computer. Still, it
is also possible for multiple clients to connect to a single server in
the same network as part of a collaborative exercise.

![](./d61d94a394907922b8f72e57e04083f4021649bf.png)


Figure 3: Moving the ERS to the cloud

Since the engine room simulator has a client-server style architecture
and it already supports running a distributed setup. So the most
straight forward way of bringing the engine room simulator to the cloud
was only to move the server part (simulator in Figure 3).

The rest of this article will cover the three main challenges we ran
into bringing the 30+-year-old engine room simulator to the cloud.

### The 1^st^ challenge: containerization

The engine room simulator was built over 30 years ago, well before the
age of containers. Our challenge was to run it on-demand, and in the
cloud, so we decided to put the simulator in a Docker container.
Containerization did, however, pose some challenges, for example, the
code used low-level Win 32 API calls with C++, it uses arcane constructs
such as \"/etc/services\", and relies on Windows Registry settings.
These Windows-specific constructs meant we had to use Windows Server
Core containers, which are some of the largest Docker images that exist.
Also, when we started in 2017, the Windows container community was small
(it still is), and official support in Docker related open source
projects was simply not there. But containers did fit our needs
perfectly, so we decided to try and use Windows containers to bring
Kongsberg\'s engine room simulator into the cloud era.

### The 2^nd^ challenge: the internet

After successfully running the engine room simulator in a Windows
container, we faced another challenge. Since Kongsberg built the engine
room simulator well before \"the cloud\" even existed, its distributed
installation option assumed that the clients and server were at least on
the same local area network (LAN), meaning that there are no firewalls
in the middle. This assumption posed a challenge because the engine room
simulator uses a proprietary communication protocol that dynamically
allocates \~200 ports as part of the initial handshake process.

![](./aa61b6d6de230aa329c43c434bd3e5e987c15ca7.png)


Figure 4: Tunneling over HTTPS Websockets

To overcome this challenge, we needed the help of the vendor, who made
this communication protocol. They made a specialized tunnel for us that
tunneled all messages over HTTPS using a single WebSocket connection.
This tunnel allowed us to connect the client and server application over
the internet.

### The 3^rd^ challenge: on-demand simulation

We were now technically able to run the engine room simulator in the
cloud and to connect to it over the internet. But to run simulations as
a service, we still needed a way for students to start simulation
anytime and anywhere using the Azure cloud. There were several container
orchestrators available, but in 2017 already, Kubernetes had the largest
community and was getting adopted by the major cloud vendors. However,
when we started, Windows containers in Kubernetes was still in beta.
Windows containers in Kubernetes became generally available in 1.15.0
(June 19^th,^ 2019).

Initially, we chose AKS engine to provision our Kubernetes cluster in
Azure. AKS engine is the tool that Microsoft uses under the hood to
provision AKS clusters, and since we knew Microsoft was working on
supporting clusters with Windows nodes, we felt this was the best
approach available to us. AKS engine generates the required templates
and script to provision a cluster, but you end up with VM\'s that you
have to manage yourself. Recently Microsoft started officially
supporting multiple node pools in AKS, which means that you can also
have Windows nodes, although the Windows part of this feature is still
in preview at the time of writing.

![](./04c5e4156774b462db1fa77d3d757153b9351770.png)


Figure 5: Kubernetes topology (simplified)

Figure 5 shows a simplified topology of how we utilize Kubernetes to run
simulations as a service in the cloud. There are three main components
involved in providing our simulation as a service solution to the
students. We have a portal where the students can, amongst other things,
select an engine room model they want to train on and choose an exercise
they want to run. We have a WPF application that runs on the student\'s
computer, starts the simulator client, and configures it to connect to
the simulator running in the cloud. And we have a scheduler component
that creates the required Kubernetes resources to run the simulator in
the cloud and makes it accessible to the simulator client running on the
student\'s computer. All three components use a single SignalR Hub to
communicate.

To start a new simulation in the cloud, a student will select an
exercise in the portal and request to run it. Doing this sends a message
to our scheduler, which will then create all required Kubernetes
resources. The scheduler will then publish a message to the WPF
application on the student\'s computer, which starts the simulator
client application and configures it to connect to the simulator running
in our Kubernetes cluster.

There is a lot more going on behind the scenes to run these simulations
in the cloud, but explaining all of that would be an article on its own.
Instead, we will share what we learned from this journey.

## What we learned

Besides the technical challenges we had to overcome, the engine room
simulator was surprisingly well suited to run in the cloud. Its
client-server architecture allowed us to move the server to our
Kubernetes cluster and move the client to the student\'s computer.

Windows containers are different from Linux containers. Windows
containers are simply a lot larger in size, especially if you need the
full Windows Server image like us. There are also challenges with the
Windows Server version you run on your Kubernetes node and the Windows
Server version of the docker image, and these have to align to some
level.

Using beta/preview versions of Kubernetes because we needed Windows
container support meant we did face some technical difficulties, but
Kubernetes itself works well with Windows containers, especially after
version 1.15.0. And the concept of Kubernetes, scheduling container
workloads on-demand, is a perfect fit for the problem we had to solve:
running simulations on demand. So, we choose to work with the
restrictions and challenges that come with using beta/preview features
over building an orchestrator. We firmly believed that official Windows
support was on its way. So not having to develop an orchestrator
certainly paid off, especially now that Kubernetes is officially
supporting Windows nodes, and Azure Kubernetes Service (AKS) is close to
officially supporting windows node pools as well.

It is possible to bring old software to the cloud and even change the
way you offer it to your customers without doing a complete rewrite.
Containerization with Windows containers helped this transition even
with software that is 30+ years old and still allows Kongsberg to offer
both models, on-demand simulation, and on-site installation.

## What the future holds

The global market already embraces the K-Sim Connect platform and is in
the early phase of adopting simulation beyond the training centers and
schools. What seemed like a threat to the experienced instructors only a
few years ago has turned into enthusiasm and discussions about the
opportunities. Today the engine room simulator is our first cloud
simulator in operation, but we will not stop there. We are already
working on bringing navigation simulators and applications like radar
training, navigation, and maneuvering to the cloud. Kongsberg is
transforming the industry of maritime simulation and training and is
leading the way to the future. Together with customers, students,
instructors, legislators, and even competitors, we will continue this
journey in confidence to unfold our collective future.
