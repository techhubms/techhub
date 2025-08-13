The world is becoming a computer

*About Cloud, AI, IOT and other trends in our industry* *-- a*
*developer's perspective*

Author: Marcel de Vries

**The world of computer games and movies is becoming a reality**

When I last visited Las Vegas for a conference at which I was a guest
speaker, it suddenly hit me. I was walking around in the movies and
games that I watched and played when I was a kid. Billboards with
high-resolution LED displays that show what there is to do and where you
should go. A computer in your hand, leading the way; your airline
check-in on your hand-held computer, and even billboards that interact
with you based on perceived emotions. We are living in the world
envisioned 20 years ago and we entered this world without noticing it.

It's amazing to see how artificial intelligence has become more and more
integrated into our lives. The billboard I just described, watching your
expressions and recognizing whether you like the ad or not, and quickly
responding to you with other content that might please you, based on the
recognized race, gender, age and skin-color. It's simply amazing, but if
you think about it, it's also a bit scary sometimes.

Las Vegas is the perfect example of how technology can be used to
persuade you to buy new shiny things and watch a show, we don't have to
look far to see how cloud computing, IOT and machine learning are
infusing our lives with new ways of interacting and communicating. Take
the simple example of the parking warden doing his rounds. Instead of
walking around with his notebook, he is now driving around with a big
camera mounted on his car, taking pictures of every parked car while
moving and automatically fining those who have not made their payments.
That is machine learning at work. The rise of the robots, but they don't
look like robots. It is just code at work.

**The rise of the cloud DevOps**

When you're a software developer, cloud computing is something you just
cannot avoid thinking about. If you don't know the cloud, if you're not
learning the cloud, chances are you will not be relevant anymore in just
a few years. For businesses it's no longer a question of whether they
need to move to the cloud. It's more about which workloads will run in
the cloud first and what will be the next step. In my work as a
consultant, I get to talk to many customers, and when you look at all
the conversations, the cloud is the predominant conversation, besides
other ways of improving the speed at which we can deliver new solutions
to market.

This is where organizational concepts like DevOps meets the Cloud.
DevOps and cloud computing accelerate each other and help you move
faster than ever before. This has major implications for your skills as
a developer. In the not too distant future, there is no hand-off between
different silos in the organization that have been optimized to utilize
their resources. You will see that yesterday's organizational boundaries
will cease to exist. You as a developer or IT operations professional
will be responsible for managing everything yourself in production. This
means no long process of getting approval. Instead, it means empowered
teams can do all the work themselves. No long waits to get a server but
infrastructure, configuration and security as code. That is the way
forward.

**The cloud and containers**

When you move to cloud computing, customers often want to select a cloud
vendor first before they get started. Of course this is based on their
old habits of needing to choose once wisely and then sticking to that
choice. That is no longer necessary. Since we don't have to make huge
capital investments in our datacenter anymore, who cares if you first
start with IaaS and later decide to go for a PaaS solution? Get started
and go fast, that's more important. Just adjust along the way. Yes, you
might lose some investments here and there, but your loss will be much
bigger if you don't make the jump fast enough and your competitor did.

Container technology is another thing that can help you. This technology
is used by all loud providers and enables you to move between them
without the need for a rewrite. This gives you all the flexibility to
move between cloud providers plus it gives you much better utilization
of the cloud infrastructure you use. Containers are here to stay and
this is the move everyone will make. You can compare it to the
revolution virtual machines brought to the IT industry. With containers
we even go beyond this, and we now have a universal way of building and
packaging our applications and running them anywhere where they support
containers. And that support is everywhere!

Today it's still very hip and cool if you create and manage your own
container clusters with orchestrators like Kubernetes. But the cloud
providers are going to abstract this away for us in the blink of an eye.
All major cloud providers, i.e. Amazon, Microsoft and IBM, are all
betting heavily on containers and the Kubernetes orchestration engine.
They are all creating fully managed clusters for us that have PaaS
characteristics, making it easy for us developers, because we don't have
to think about the underlying hardware platform. This is a major shift,
further into configuration and infrastructure as code. This trend is
rapidly expanding and something we will look back on in a few years as
the war of the cluster orchestrators. We will think of it as a "tabs
versus spaces" discussion. Yes, it is fun to debate, but it really does
not make a huge difference, as long as we pick something. The industry
has picked the winner and clearly that is Kubernetes.

**Cloud, IOT and the Edge**

You can't ignore the fact that we're getting more and more connected
devices. Those connected devices make up the so- called edge of the
cloud. And it is the billions of mobile devices and millions of PCs that
make this edge extremely powerful if we would leverage all that
computing power to do smarter things rather than just calling into the
cloud network. This is where you see more and more smarts coming into
our devices with new computing power. A new collective processing power
provided by Graphical Processing Units (GPUs), Neural Processors Units
(NPUs) and Field Programmable Gate Arrays (FPGAs) in addition to the
classic CPU. All this new silicon provides dedicated computing power for
various jobs that can be done at the edge to make the impossible
possible.

Imagine you are running a vision recognition program to look at the
goods you process and do a final visual inspection. With a
machine-learning model in the cloud you need to take a photo, upload it
to the cloud, and then receive an answer. With a CPU and a local
machine-learning model (e.g. an ONNX Model, supported on all Windows
versions these days), you can process these images much faster, even
hundreds per minute. Now put this model in a Neural Processing Unit or
an FPGA and you can do this a hundred times faster. All of a sudden you
realize that the futuristic vision of real-time video stream analysis
has already become a reality. Creating a model requires a lot of data
and is something that takes time and effort. This is typically a
workload you would do in the cloud itself. But exporting this model to
something that can run on your Raspberry PI creates a whole new world of
opportunities. The cloud, IOT devices, and Edge computing are the
underlying pieces of the puzzle that are turning machine learning into
something that's more and more real-time and sophisticated. Only then we
might have achieved real artificial intelligence, as opposed to the
glorified machine learning that is called AI nowadays.

**New client frameworks to interact with our systems**

Today, we build both native and web-based client applications using
technologies like UWP, WPF, Electron and Angular, React and many other
frameworks. Once again we're experiencing the rise of another new
approach to web-based client application development: WebAssembly.
WebAssembly -- or WASM -- is a new, portable, size and load time
efficient format, suitable for compilation to the web. I believe our
industry has found its new silver bullet to bring strong typed languages
to the web browser and write applications that run everywhere. In the
past we saw all kinds of crazy frameworks and solutions popping up based
on plugins. WASM opens the same set of possibilities as with plugins,
but in a standardized way supported by all popular browsers. This means
you can now create cross-platform UI frameworks that run natively in the
browser and are written in C#, Java, Go or any other language. Microsoft
is pursuing this concept with its Blazor project. Blazor is a framework
for creating web pages that run C# code using .NET inside a web browser
on any device or platform that supports the WASM web standard. People
are even working on porting good old Silverlight code over to
WebAssembly.

Of course, this is a great new way of doing things. However, one thing I
have learned during the last few years in our industry is that there is
nothing as volatile as client UI technology. Especially with the web
having the next new shiny framework every six to nine months, and now
probably even faster with WebAssembly. Will it become a big thing?
Probably, but one of the things I would urge everyone to consider is to
look at application development in a different way. We have a stable
back-end part and a very volatile frontend UI part. You are better off
investing more in a good and robust, well performing back-end system,
probably with a microservice architecture, running in containers on a
cloud-managed Kubernetes cluster. The client side investment should be
as low as possible. Use the flavor that makes developers happy at that
moment, but keep in mind that you will probably need to rewrite it after
a short period of time. My advice is also not to invest a penny in
writing a client framework that would speed up your development or
abstract other UI frameworks. It has proven to be a disinvestment.
You're better off coding the client in a straightforward manner, knowing
you'll discard it soon. This allows you to be more agile to adopt new
client technologies and create a stable reliable and secure backbone
that can serve any new frontend technology that may arise during the
next few years.

**Explosion of computing devices and generation of data**

One final trend to touch on is the explosion of devices and data. It is
unbelievable that the amount of data generated every day is over 2.5
quintillion bytes! The next few years more than 20-50 billion devices
will be deployed to gather data, process data, and provide us with
computing power. This is a game changer, because it provides us with
such rich datasets that we can train our machine learning models better
and better each day. Computers will be able to assist us in many more
tasks than possible today. Machine learning, or AI as the industry calls
it, will be incorporated into many things in our lives. From the smart
doorbell to the smart office and the programs we use today, all will
have AI capabilities that will make our lives easier. You can think of
our planet becoming one gigantic computer, calculating and reasoning on
our collective lives all the time. Again, this is a beautiful thing and
scary at the same time. On the one hand, we can do great things to help
mankind. On the other hand, we could also use it for malicious intent.
As Satya Nadella put it at the Microsoft's //build/ developer conference
keynote: "All this computing power, and this collective computer we
build together, will require us to think about what computers [can
do]{.underline}, but also about what computers [should do]{.underline}.
It is up to us and our governments to provide the legislation so we can
ensure we will live in freedom and safeguard our privacy when we want
to."

One thing is clear to me if you look at all these changes in our
industry. Things are changing faster than ever! The only constant I see
is change. And you'd better embrace it, so you can create and shape the
future now!
