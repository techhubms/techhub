![](./media/image1.png)
**Technical introduction to Azure
Functions**

Azure Functions is Microsoft's event-driven, serverless, cloud platform
for creating lightweight background processes. It is Microsoft's FaaS
(Function as a Service) answer to Amazon's AWS Lambda. A function is
triggered by an event and many event triggers are built in the runtime.
Azure Functions can be written in multiple programming languages.

**Background**

In November 2014 Amazon introduced AWS Lambda. This service made Amazon
into the first major cloud provider with a serverless offering. Besides
Amazon, there are various other vendors of serverless offerings. Google
is working on Google Cloud Functions, which has not been released yet.
IBM offers a serverless platform within IBM Bluemix, which is called IBM
OpenWhisk. In March 2016, Microsoft announced Azure Functions on Build,
and Microsoft already released Azure Functions with General Availability
(GA) as early as November 2016.

**In Detail**

Despite the term serverless, Azure Functions naturally needs servers to
run on. The term serverless refers to the fact that you don't need to
provision or create virtual servers yourself to let the code run on. All
necessary provisioning is done by Azure.

When you start with Azure Functions, you create a Functions app in the
Azure Portal. This is the host of multiple functions, while a function
app is a special kind of App Service: the portal provides a user
interface to create functions and configures the triggers for it. It's
also possible to set up Continuous Integration with Azure Functions.

Azure functions are based on the Azure WebJobs SDK. The Azure WebJobs
SDK offers triggers, bindings and a runtime. Triggers are events that
trigger your function. Bindings are declared in metadata and connect
external resources to your function as trigger, input or output
parameters. Azure Functions offers a layer on top of this. This layer is
open source and can be found on github.
([https://xpir.it/mag4-func1](https://github.com/Azure/azure-webjobs-sdk-script))

To create an Azure Function, you can choose one of the following
programming languages: C#, PowerShell, Batch, Python, Bash, JavaScript,
PHP or F#.

When you choose a C# based function, the function is created in a csx
file, which is a C# script file. When the function is executed for the
first time, the script is compiled and executed in memory. You can
compare an Azure Function with a method in a C# class. When you want to
add other classes, these classes should be coded in the same csx file.
It is not possible to separate files to put your code in. A result of
this is that you have to think about what you automate in your Azure
function and the amount of code that is needed for this. You should only
have the code for the main process in your Azure Function to keep it
maintainable. If you like, you can add references or Nuget packages to
make use of Domain Models or other classes.

**Pricing models**

When you create an Azure Functions app, you can choose between two
service plans: Consumption plan and App Service Plan.

The consumption plan is new and unique for Azure Functions. You only pay
for the number of requests and the Gigabytes per second (GB-s). GB-s
stands for the time required for processing, multiplied by the allocated
memory. The price includes 1 million executions or 400.000 GB-s. The
consumption plan scales the CPU and memory automatically up to 1.5 GB.
It's possible to configure a daily quota to create a limit on the
spending.

The App Service Plan is the same Azure Resource you use to host your
webapps or webjobs, for example a Standard S2. You pay for the entire
service plan with fixed expenses, and you are in control of the
scalability, which can have a fixed or automatic setup.

In addition to paying for Azure functions, you also need to pay for a
Storage Account. All function types except HTTP triggers require a
storage account.

**Adding a function**

After adding an Azure Function in the Azure Portal, the first thing you
need to do is to choose the language you want to create the function in.
After that, you choose how the function will be triggered, based on a
predefined template (See all triggers in the table below). The last step
consists of configuring the settings belonging to the selected trigger.
For example, when you choose the Event Hub binding, you have to
configure the access to the Event Hub. After the function has been
added, the editor in the Azure portal offers functionality to develop,
run and monitor the function.

![C:\\Users\\Pascal
Naber\\AppData\\Local\\Microsoft\\Windows\\INetCache\\Content.Word\\DevEnvironment.png](./media/image2.png)


Development environment in the Azure Portal

**Features**

Azure Functions can be started on the basis of a trigger (event). Azure
Functions can also integrate with other Azure Services, which is called
bindings. The substantial power of Azure Functions becomes manifest
through the integration with other Azure PaaS resources such as
ServiceBus, Storage, and EventHub. The following table shows which
triggers and bindings are supported with Azure Functions:

  ------------------------------------------------------------------------
  Binding                                    Trigger   Input     Output
  ------------------------------------------ --------- --------- ---------
  **Function app Schedule**\                 ✔                   
  Triggers based on a schedule which is                          
  configured with a CRON expression                              

  **Http (REST or Webhook)**\                ✔                   ✔
  Invokes a function with an Http Request,                       
  responds to webhooks and allows to respond                     
  to requests. Like a GitHub webhook.                            

  **Blob Storage**\                          ✔         ✔         ✔
  Triggers for new and updated blobs in the                      
  container, read blobs or write blobs                           

  **Event Hub Event**\                       ✔                   ✔
  Responds to an event send to the Event Hub                     
  or sends a message to the Event Hub                            

  **Storage Queue**\                         ✔                   ✔
  Monitors a queue for new messages,                             
  responds to them and writes messages to a                      
  storage queue.                                                 

  **Service Bus Queue & Topic**\             ✔                   ✔
  Responds to messages from a queue or topic                     
  and creates a queue message                                    

  **Table Storage**\                                   ✔         ✔
  Stores read tables and writes entities                         

  **Mobile App Storage**\                              ✔         ✔
  Reads from and writes to data tables                           

  **Document DB**\                                     ✔         ✔
  Reads or writes a document                                     

  **Notification Hub Push notification**\                        ✔
  Sends push notifications                                       

  **Twilio SMS**\                                                ✔
  Sends an SMS text message                                      
  ------------------------------------------------------------------------

A trigger can be the event that starts the Azure Function. Please note
that not all bindings support a trigger implementation. For example, it
is not possible to start your Azure Function based on the event of a new
document in DocumentDB. An Azure Function can only have one trigger and
an unlimited number of inputs and outputs.

Bindings can be configured in the Azure Portal in a user-friendly user
interface. The bindings are stored in the function.json file. This file
can be edited in the Azure Portal also.

The binding for a trigger for an EventHub looks like this:

+-----------------------------------------------------------------------+
| {                                                                     |
|                                                                       |
| \"bindings\": \[                                                      |
|                                                                       |
| {                                                                     |
|                                                                       |
| \"type\": \"eventHubTrigger\",                                        |
|                                                                       |
| \"name\": \"myEventHubMessage\",                                      |
|                                                                       |
| \"direction\": \"in\",                                                |
|                                                                       |
| \"path\": \"myeventhub\",                                             |
|                                                                       |
| \"connection\": \"ehconnection\"                                      |
|                                                                       |
| }                                                                     |
|                                                                       |
| \],                                                                   |
|                                                                       |
| \"disabled\": false                                                   |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

**Local development**

Since December 2016 it has been possible to develop functions locally in
Visual Studio. To make this possible in Visual Studio you have to
install The Visual Studio Tools for Azure Functions.
<https://xpir.it/>mag4-func2

At the time of writing this article, this functionality is only
available for Visual Studio 2015 update 3 and as yet, there is no
version for Visual Studio 2017. The tools are a prerelease version and
there are limitations. The most striking limitation is the limited
support for IntelliSense in csx files. For example, there is no support
from IntelliSense for input parameters.

The Azure Tools are offering a new Visual Studio Project template called
Azure functions (prerelease). A project can contain multiple functions.
The tooling also offers the same function templates as the Azure portal.
Another major benefit of the tools is that it is possible to run Azure
Functions locally and debug them.

When you debug a function for the first time, Visual Studio will ask you
to download the Azure Function CLI. This CLI is needed to debug the
Azure Function and acts locally as the host for the function. When an
update is available, Visual Studio will ask you to download the latest
version.

![C:\\Users\\Pascal
Naber\\AppData\\Local\\Microsoft\\Windows\\INetCache\\Content.Word\\cli.png](./media/image3.png)


Running an Azure function locally with the Azure Function Tools

**Settings**

AppSettings and Connectionstrings work in the same way as Web Apps and
Web Jobs. Similarly, the settings are stored in the application service.
In the Azure Function Tools, appsettings and connectionstrings need to
be added in the appsettings.json.

![](./media/image4.png)

Settings on an Azure Function app.

**References**

Developing csx files is different from the normal way of developing C#
classes. For example, it is not possible to set assembly references by
setting a reference in the same way as you used to in Visual Studio.

To set dependencies to libraries it is possible to reference .Net 4.6
Nuget packages. These references are stored in the project.json file.

After that, you add the following line to the csx file:

[#r "Microsoft.Azure.WebJobs"]{.mark}

It is also possible to reference your own assemblies. Add the assembly
to the bin directory of the Azure Function and add a reference with the
following line:

[#r "MyAssemblyName.dll"]{.mark}

**Proxies**

The latest feature at the time of writing this article is Azure Function
Proxies. Proxies allow you to combine multiple Azure Functions in one
large API. This large API is a façade, i.e. a single point of entry for
the outside world and it forwards the calls to other Azure Functions. It
is the light version of API Management, without throttling, security,
and caching, with the benefit that it is cheaper.

**Testing**

With ScriptCS (csx files) it is not possible to unit test Azure
Functions in Visual Studio. The functions can only be tested when the
function runs in Azure.

**Precompiled functions**

One solution to make unit testing of functions easier is to use
precompiled functions. With precompiled functions, you are actually
developing the usual C# code instead of C# scripting code. This code is
compiled into an assembly in the way you are used to. The compiled code
can be deployed to Azure as an Azure Function. To do this, choose a
Class Library project instead of an Azure Function project. This code
can be tested in the same way you are used to. To let it behave as an
Azure Function you need to add a couple of NuGet references, and you
need to add a function.json configuration file. A disadvantage of the
precompiled function way of working is that you cannot edit the code in
the Azure Portal anymore, because the file is in a binary format.

If you want to know more about this, please read the blogpost of our
colleague Geert van der Cruijsen <https://xpir.it/mag4-func3>.

**Continuous Deployment**

By default, the functions in the Azure Portal are not under source
control. However, there are a number of options available to create
integration with source control. Azure functions provide functionality
to configure Continuous Deployment with several source code providers
such as GitHub and VSTS. After configuration, the Azure Functions will
be read-only in the Azure Portal. After each commit the entire Azure
function app will be updated. However, you need to use separate branches
if you want to be in control when a function app is deployed.

![](./media/image5.png)


An Azure function becomes read only after configuring source control
integration

Another possibility is to use a Build and Release pipeline in VSTS for
example. You can read how to do this in the blogpost of our colleague
Peter Groenewegen: <https://xpir.it/mag4-func4>

**Conclusion**

From a technical viewpoint, you can program the same functionality in C#
with an Azure Function as with an Azure WebJob. One of the reasons why
Azure Functions is interesting is the dynamic pricing model. You don't
have to pay any attention to any infrastructure dependency because you
only pay for what you use and the first one million calls are free. In
addition, the automatic scaling of pricing model is a powerful feature,
which is another aspect that saves costs and makes your life easy. A
final advantage of Azure Functions is that you don't need to have an IDE
(Visual Studio) to develop your functions. You only need a web browser!

Azure Functions force you to create small, self-contained pieces of
functionality, which are event-driven and can be updated separately.
These are characteristics of Micro services also. If you can handle the
way you manage the source code, tests and updates this could be a neat
approach. My experience is that Azure Functions already prove their
power when using them to automate small separate processes like checking
resources based on a timer or a different trigger.

Azure Functions provides you with more agility because you only have to
consider business logic and not infrastructure. The Microsoft Azure
Function team constantly updates the features for Azure Functions, so
new functionality is on its way.
