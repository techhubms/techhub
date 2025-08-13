# The Serverless lifecycle: is it really that different?

## Introduction

In today's world, technological innovation is moving at breakneck speed.
It was only 15 years ago that we were deploying applications on bare
metal servers using floppy disks. Since then, we have moved to virtual
machines, Infrastructure as a Service (IaaS), Platform as a Service
(PaaS), Software as a Service (SaaS). And recently we've moved into a
world where container technology is starting to become mainstream. The
next step is serverless computing. Or, as Martin Fowler[^1] describes
this in his blog, Functions as a Service (FaaS) and Backend as a Service
(BaaS). This is yet another "\*aaS" to keep in mind when selecting your
hosting platform. And just like any other application, a serverless
application requires proper lifecycle management. In this article, we'll
discuss this serverless lifecycle, and see where it differs from
"normal" applications that you all know.

## Serverless computing

So what is serverless computing? Of course there is still a server
involved somewhere. It's just not managed by you, but by a cloud
provider. For a thorough explanation, please read the article The
(r)evolution of Cloud Computing elsewhere in this magazine. As far as
this article is concerned, we think that serverless can best be seen as
PaaS++. The following tweet from Adrian Cockroft describes it very
simply:

![](./media/image1.png)

When it comes to FaaS, both Amazon and Microsoft offer cloud-based
solutions. Amazon Web Services (AWS) offers AWS Lambda and Microsoft's
Azure offers Azure Functions. This article primarily focuses on the
Azure stack, but it also applies to AWS.

## The Serverless Lifecycle

![](./media/image2.png)
![](./media/image3.png)
A serverless application is usually very
small, but nevertheless it is still an application. And as already
mentioned, every application has a lifecycle and this lifecycle needs to
be managed. The FaaS implementation in Azure, Azure Functions, is fairly
new, and the marketing around Azure Functions focuses a lot on the easy
and friction-free editing experience "in the browser". Although this is
very powerful, it is also very dangerous and it may even be unwanted
when it comes to a production application. Just like the infamous
"Right-click, Publish" experience in Visual Studio, this lets you
publish your application straight to production without following any
process at all. Of course, there are rights and security, but still,
imagine what happens when someone "accidentally" edits a heavily used
production function.

*The infamous "Right-click, Publish" experience in Visual Studio, and
the Azure Functions equivalent to editing in the browser.*

It is important to treat a serverless application in the same way as any
other application. The following diagram shows a typical application
lifecycle. In the following paragraphs, we will apply each of the phases
from the diagram to a serverless application.

### Requirements

Will requirements change for serverless applications? Surely it depends
on how you define your requirements, but requirements should describe
the functional and non-functional part of an application, while staying
away from technical implementation details. A serverless architecture
deals with the technical implementation of a requirement. The process of
gathering and refining requirements itself should not change.

### Development

The impact on your development cycle is probably higher. There are a lot
of choices to be made up-front. Which language are you going to use?
Azure Functions supports many languages like C#, Python, Node.JS,
Powershell, PHP, etc. And which IDE are you going to use? The most
important thing is to think ahead and define your test, build and
release strategy to make a good choice. For example, if you want unit
tests as well as a build and a release pipeline, editing your code
directly in a web browser is probably not the best option. Let's have a
look at our development strategy and the options you have.

#### What are we developing?

When you build a serverless app, you can download a project template in
Visual Studio and get started. However, in most cases this is not
enough. Your application (or should we say function) probably needs a
backend, a data store, Azure Blob Storage or a MongoDB. You can create
this manually and configure your function accordingly, but in the modern
DevOps world we should strive to automate everything. This means that
you should develop the infrastructure for your application together with
that application itself, using the infrastructure-as-code paradigm to
enable fully automated deployment. Whether you're using an ARM template
on Azure, CloudFormation on AWS, or Docker containers on a hosted
cluster, in each case you need to develop both the application code and
the infrastructure code.

![](./media/image4.png)


#### How to develop

And then there is the question of how you are going to develop your
code. Focusing on Azure Functions as a platform with C# as your language
of choice means that you have four options to develop serverless
applications.

##### Write code directly in the browser

The fastest way to create an Azure Function is to write, test and
monitor it directly in the browser. However, ease of use comes at a
cost. You don't have a pipeline, unit tests or even a backing source
control repository, so it is probably not suitable for your enterprise
application, where a higher level of traceability and control is usually
required.

However, creating and editing directly in the browser is great for rapid
prototyping, quickly trying out an idea, or creating a small application
that you'll use only for yourself.

##### Connect a repository to Azure Functions

![](./media/image5.png)
Using Azure Functions, you can connect an existing
repository to your Azure Function, resolving the source control issue.
You have various options for the type of repository that you want to
use, for instance a repository in VSTS or GitHub, but solutions like
OneDrive and Dropbox are also an option.

Whenever a change is committed or saved in the repository, this will be
deployed automatically. In the case of OneDrive or Dropbox, this happens
whenever somebody changes a file. In case of a Git repository,
deployment is always connected to a specific branch (e.g. master). This
gives you a bit more control, since you can use branch policies[^2] to
control which code is merged into that branch. But still, for larger
organizations where an application typically goes through different
stages of testing and acceptance before ending up in production, this
does not provide the required level of control.

This mode of development & deployment is most suited for small teams
that write small applications and don't need the additional verification
steps provided by a build and release pipeline.

##### Using your favorite IDE and its capabilities

If you want to step it up a notch, you could make use of the power of
your favorite IDE. This means that you can run your Azure Functions
locally, allowing you to debug them. And if you develop them like any
regular application, you can use familiar practices to get things like
traceability and controlled releases in place. Code is stored in source
control and you can create a build & release pipeline (see Technical
Introduction to Microsoft Azure Functions) to publish your functions to
Azure.

When you write Azure Functions in C#, you should be familiar with the
Visual Studio Tools for Azure Functions. They let you create an Azure
Functions project inside Visual Studio, with some nice boilerplate code
already in place. The only challenge that remains in this scenario is
testing. Testing your Azure functions written as a C# Script File (CSX)
is quite limited. The Visual Studio Tools for Azure Functions will give
you the local compile, build & debug experience with things like
breakpoints, watches, etcetera. However, running unit tests on CSX files
is not possible. This means that most of your local testing will be
manual, which is not sustainable in today's world, where the focus of
your testing should be at the unit test level (The Test Pyramid concept:
<https://martinfowler.com/bliki/TestPyramid.html>). Because of this
limitation, we recommend using the precompiled assembly's approach, as
described in the following paragraph.

However, Azure Functions supports quite a few other languages (like
Node.js and PHP) for which unit testing is very well possible. If you're
using any of those, then the Azure Functions CLI[^3] will let you run
and debug your Azure Function locally[^4]. Using this approach allows
you to develop enterprise grade applications using Azure Functions.

##### Write CSX files and reference a C# class library 

When you're writing C# and want to stay close to the usual development
flow, the best alternative is to use a precompiled assembly that you can
reference in your CSX file, leaving your CSX as nothing more than a
wrapper around a "normal" class library[^5]. When you use the class
library, you can use the default toolset for Unit Testing and all other
features you are used to in Visual Studio, storing things in Source
Control, creating builds, releases, and still have a state-of-the-art
serverless function.

The following table summarizes the options for development as outlined
before:

  ------------------------------------------------------------------------------
                           Source      Testing       Deployment   Typical
                           control                                application
  ------------------------ ----------- ------------- ------------ --------------
  Write code directly in   None        Manual        Code is      Rapid
  the browser                          testing using published    prototyping
                                       e.g. Postman  directly     
                                       or curl       when you     
                                                     click "Save" 

  Connect a repository to  Basic (e.g. Possible, but Straight     Small teams
  Azure Functions          OneDrive)   no            from the     with no need
                           to full     integration   repository   for build &
                           (e.g. Git)                to           release
                                                     production   pipeline

  Using your favorite IDE  Full        C#/CSX: No    Full build & Enterprise
  and its capabilities                 unit testing, release      scale
                                       other         pipeline     applications
                                       languages:                 
                                       full                       

  Write CSX files and      Full        Full          Full build & Enterprise
  reference a C# class                               release      scale
  library                                            pipeline     applications
  ------------------------------------------------------------------------------

## 

## Build

The purpose of a build pipeline is to produce artifacts that can be
deployed throughout multiple environments. For example, when you create
a normal web application, you build an MSDeploy package that you provide
with the right values for its parameters in your release pipeline. Or,
when you use containers, your build pipeline will turn into a "bakery"
to produce a container and publish it to a container repository so it
can be used on any hosting platform that supports containers.

When it comes to your serverless application, your build will be the
same as what you are used to. The build produces the binaries and other
files for your application, the artifacts to create the underlying
infrastructure, and it will have some configuration settings that can be
modified during the deployment in your release pipeline.

Performing tests that do not require a running application are typically
also something you want to include in your builds. For example, the Unit
Tests and the ARM validation[^6] for your infrastructure code.

## Test

The hardest stage of the application lifecycle (as mentioned before) is
the Test stage. In a continuous delivery workflow, continuously testing
and getting feedback about the steps you executed is critical,
throughout the entire lifecycle of your application. Ideally, the effort
you spend on creating different kinds of tests should be distributed
according to the Test Pyramid[^7].

![https://martinfowler.com/bliki/images/testPyramid/test-pyramid.png](./media/image6.png)


When you create a web service or an API in the traditional way, you
create unit tests to test the inner workings of your methods (unit
level) and API tests to validate and ensure the correct implementation
of the interface (service level). At a later stage, integration tests
are required to test relationships to other components or services (UI
level). In the end you will need some load tests to validate whether
your service can perform under load as well.

All these test types are also relevant in a serverless context. However,
when you create a serverless application, the question is whether all
the tests should bear the same weight as when you use a traditional
development method for your application.

At the unit level, you will want to use some form of manual testing on
your functions. The Azure Functions documentation describes manual
testing with some http clients like Postman or Curl[^8]. As mentioned
before, this is simple and easy to use, but not sustainable when you
have a large number of functions that need to be tested and maintained.
For automated unit testing you will want to use the capabilities of your
IDE, as described in the previous chapter.

For API and integration testing (at the service and UI level) your
function will need to be running and be accessible from the outside. Of
course there is the emulator to run your Azure Function locally, but at
some point you will want to run your function more "real". This involves
deploying and running your function somewhere, as described in the
Release phase.

When it comes to load testing, it is fairly easy to run these tests in
the normal manner. But the question that we really should ask ourselves
is: Is this still required? A serverless application is defined by the
fact that it is small and that you only focus on the functionality and
not the underlying platform. Does it make sense to load test your
function? What are you really testing -- the functionality or the
underlying platform?

In some cases load testing does make sense, for example if you want to
test whether concurrent calls have any impact on shared data or
underlying data stores. However, scalability and availability of the
application should be a given when using serverless.

## Release

Whereas the build pipeline is probably not very different for a
serverless application, the release pipeline certainly is! There are two
considerations to bear in mind when thinking about deploying your
application towards production.

-   How do you deal with different environments for testing

-   How do you deal with different versions of the same application

### Different environments or just a different version?

The first thing we need to decide when answering this question is
whether or not we really need a separate environment to test our
serverless application. What if we test "in production" using a new
version of a function and not yet release it to the public?

When it comes to the serverless application itself, i.e. the Azure
Function, it does not really matter where you store it. It is the
trigger or backend that introduces the need for a different environment.

There are two options for serving different functionality in your
function: using feature toggles or deploying to a separate environment.

### Using feature toggles

Feature toggles are a way to modify system behavior in a running
application[^9]. When you use feature toggles, you need a mechanism to
switch the toggle. A possible way to do this would be to create some
Feature Toggle Functions that can add, update or get a value for a
specific toggle from a data store. Your App Function (which implements
the actual functionality) can use the Feature Toggle Functions to get
the value for specific toggles, and determine its own behavior
accordingly. This lets you update (or change) the functionality of your
App Function at runtime.

![](./media/image7.png)


### Deploying to a separate environment

If you prefer your functions to always exhibit the same behavior, a
better approach might be to switch towards a different deployment for
every "environment" and version, and use an API Manager to direct the
traffic to the right
endpoints.![](./media/image8.png)
If you consider the fact that each new
version is a new deployment, running on a different URL, you can
probably imagine this will be hard to maintain and communicate with the
consumers of your serverless app. An API manager like Azure API
Management[^10] can help you in routing your traffic to the right
endpoints. The lightweight version of this is Azure Function
Proxies.[^11]

If a consumer asks for version 2 of a service, API management will
redirect him correctly to the right function implementation, without the
user knowing the original URL. If you expand this concept to different
environments (such as a test environment), you can configure API
Management to redirect the user to a specific environment based on a
specific property of the request (such as a header or request parameter,
like an API key). This is completely transparent for end users and easy
to configure. You can even update API Management through its own API, so
that you can automatically expose new functions or versions from your
release pipeline.

Of course you need to take care of access control on API Management and
make sure the right environments are called, but the concept is clear
and transparent.

The release pipeline still deploys the bits to different environments,
and API Management is just another artifact that can be configured from
within the pipelines.

## Operate and Monitor

The last phase in our Lifecycle is Operate and Monitor. Monitoring is
required, just like with any other application. While traditional
operational monitoring on metrics such as CPU, memory and disk usage is
not needed, you still have some responsibilities when you release a
serverless function to the world. At least you need to make sure the
function is still running as intended, and is not generating any errors.
It is also very valuable to check whether the function is actually used.

Azure Functions provides a number of basic monitoring features, which
can be accessed from the Azure Portal or from the Azure command line. If
you need more advanced monitoring, you can implement Microsoft
Application Insights, which distinguishes between four kinds of
monitoring.

![](./media/image11.png)

The idea of using a serverless platform is that you don't need to worry
about availability, because the platform takes care of this. In that
respect, performance is also taken care of by the platform, except for
specific situations.

In addition, Diagnostics and Usage are both very relevant. You want to
know whether your function is working properly, and if not, the cause of
the failure. Usage statistics are valuable because you can use these
metrics to decide whether you can phase out a function, or which
function is a candidate for optimization.

# Conclusion

Creating a serverless application is not that different from creating
any other application. The tools allow you, and sometimes even encourage
you, to do it in a simple way. However, usually it is advisable to think
ahead and to make some decisions up front. When you consider putting
your application in production, treat your serverless application just
like any production application. Utilize powerful tools like an IDE,
think about your test strategies, create build and release pipelines and
gather metrics. Azure Functions offers great support for multiple
languages and any language is the right choice. Managing your
application lifecycle correctly is what makes the difference!

[^1]: https://xpir.it/mag4-slc1

[^2]: https://xpir.it/mag4-slc2

[^3]: https://xpir.it/mag4-slc3

[^4]: https://xpir.it/mag4-slc4

[^5]: https://xpir.it/mag4-slc5

[^6]: https://xpir.it/mag4-slc6 and https://xpir.it/mag4-slc7

[^7]: https://xpir.it/mag4-slc8

[^8]: https://xpir.it/mag4-slc9

[^9]: https://xpir.it/mag4-slc10

[^10]: https://xpir.it/mag4-slc11

[^11]: https://xpir.it/mag4-slc12
