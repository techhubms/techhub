# Using Microsoft Application Insights to Implement a Build, Measure, Learn Loop 

In our industry we are constantly striving to build better software
faster. At the moment you see that continuous delivery is the current
technology hype where we all try to deliver the software in faster
cycles to the customer and try to achieve to build only the software
that brings the highest business value to the customer. Many companies
are now selling consulting services and products that focus primarily
around the delivery and installation of the software products in test
and production environment so the value actually reaches the customer in
a timely manner. This is all great and does deliver a lot of value but
there are still a lot of issues we need to solve. One of the primary
issues we still need to solve is how we can determine what additional
features will actually bring the most value to the end user.

It is crucial to know that you are delivering the right product that
actually provides value. You want to know the performance of our
application in production and how specific usage of the system can
impact the performance of our application. If something goes wrong you
want to get diagnostic information from your system in production so you
can see or even predict when things will break. All these questions are
solvable, but most of the time they are solved with custom point
solutions that take up a lot of time to build and maintain. In this
article I will show you how a product called Microsoft Application
insights can provide these insights with out of the box functionality so
you can focus on building features instead of worrying about these kinds
of infrastructural fundamentals in your application.

## Build, Measure, Learn

Application Insights collects, processes and presents a wide variety of
telemetry data including performance, usage, availability, exceptions,
crashes, environment, log and developer-supplied data from all
components of an application---including clients (devices and browser),
servers, databases and services. With this \"360 degree view\" of your
application, Application Insights can quickly detect availability and
performance problems, alert you, pinpoint their root cause and connect
you to rich diagnostic experiences in Visual Studio for diagnosis and
repair. It also supports continuous, data-driven improvement of an
application. For example, it highlights which features are most and
least used, where users get \"stuck\" in an application, where and why
exceptions are occurring, which client platforms are being used with
which OS versions, and where performance optimizations will make the
biggest impact on compute costs. By incorporating Application Insights
into your product you can start gaining insights in the usage of your
product in real time, giving you information that you can use to better
decide what needs to be done next in your development team.

Application Insights provides libraries you can include in virtually any
type of application. There are support libraries for native Java
applications, iOS applications (Objective-C), Android applications,
Windows Store applications and web applications. You can very easily
gain access to these libraries by either pulling them from GitHub, or
for .NET applications by getting them from NuGet.

At the moment Microsoft is transitioning from Application Insights that
was part of the Visual Studio Online offering to an offering that is an
intrinsic part of the Microsoft Azure offering. For this reason you will
find two types of libraries, the old and the new ones. The one I
describe here are the new libraries that provide a common API regardless
of the platform you target. So methods I show here in a web application
are exactly the same on the other supported platforms. At this moment
you get the libraries as pre-release packages from NuGet.

## Measuring application performance

With Application Insights you can get diagnostics on both the
client-side performance and the server-side performance of e.g. your web
application. The way to get started and get performance data is first by
adding Application Insights into your project. In my example I will use
an ASP.NET MVC website, but this works the same for the other
application types I just mentioned.

You can use the Visual Studio Wizard to generate a lot of stuff for you.
I will describe what to do by hand so you can also apply it to projects
you manage without Visual Studio.

First, you need to go to the new Azure portal and there create a new
Application Insights profile.

![](./media/image1.png)


Figure 1 Create Application Insights Profile

After specifying the correct Application Insights name, application
type, resource group and region you will get the dashboard where all
your telemetry data will be shown when we have set up things correctly
in our application.

![](./media/image2.png)


Figure 2 Initial Application Insights Dashboard

As you can see in the portal, you can now add Application Insights into
your project in Visual Studio and add code to your web page to show the
client performance of your web application. The server-side code is
nothing more than registering Application Insights, and it will start
intercepting all work done in your server-side code, including the
detection of outgoing dependencies and the time consumed in those calls.
When you add Application Insights to your ASP.NET MVC application it
will register an Http Module in the web.config that will initialize
Application Insights and start the telemetry dataflow of your
application. On other platforms like e.g. mobile applications, you need
to bootstrap Application Insights in the startup of your application.

Application Insights is configured using a file called
ApplicationInsights.config. This is also the location where you specify
the InstrumentationKey that identifies where your data needs to flow.
You can keep this key the same for all instances of your website, so all
data flows to the same location on Azure. After setting up these basics
you will already get all the basic server performance and usage data
flowing to the portal dashboard. You can see the first charts already
light-up in your portal.

With websites that have heavier JavaScript on the client it is important
to also keep track of the client side performance these days. You can
also enable client-side tracking of performance by injecting a small
piece of JavaScript into your application. This is more or less the same
concept as you have with other tools that e.g. track user behaviour on
pages, like Google Analytics. Each page must have this small piece of
JavaScript code that you can copy from the portal. In an MVC app the
most convenient place to insert this code is in the master template file
that is used by every page. This is standard \_Layout.cshtml file that
can be found in the "views\\Shared" folder. The script you need to place
in the page can be copied from the portal when you click on the "Add
code to monitor web pages" blade in the Azure portal. The code is shown
in the following code snippet:

\<script type=\"text/javascript\"\>

    var appInsights=window.appInsights\|\|function(config){

        function s(config){t\[config\]=function(){var i=arguments;t.queue.push(function(){t\[config\].apply(t,i)})}}var t={config:config},r=document,f=window,e=\"script\",o=r.createElement(e),i,u;for(o.src=config.url\|\|\"//az416426.vo.msecnd.net/scripts/a/ai.0.js\",r.getElementsByTagName(e)\[0\].parentNode.appendChild(o),t.cookie=r.cookie,t.queue=\[\],i=\[\"Event\",\"Exception\",\"Metric\",\"PageView\",\"Trace\"\];i.length;)s(\"track\"+i.pop());return config.disableExceptionTracking\|\|(i=\"onerror\",s(\"\_\"+i),u=f\[i\],f\[i\]=function(config,r,f,e,o){var s=u&&u(config,r,f,e,o);return s!==!0&&t\[\"\_\"+i\](config,r,f,e,o),s}),t

    }({

        [instrumentationKey:\"841abd52-569e-4c2f-a213-32ded088cdb2\"]{.mark}

    });

    [window.appInsights=appInsights;]{.mark}

[    appInsights.trackPageView();]{.mark}

\</script\>

In this piece of JavaScript code I highlighted two parts. First where
you supply the instrumentation key. This is the same key as you will
find in the ApplicationInsights.config file. The second part is the
initialization of Application Insights in the client. You see you can
just reference a variable named appInsights and on that you can call a
variety of methods to track information from the client. The
trackPageView() call will now be executed on each page the client loads
and that will generate the basic performance data you can see in the
portal.

![](./media/image3.png)


Figure 3First Performance Data in Dashboard

## 

## Looking at application diagnostics 

With application diagnostics you can get valuable insights into why
things might have gone wrong in your application. With Application
Insights you can search right from the portal. It is a centralized
location where you can search for the log events you send out and it can
also correlate with the performance and usage metrics you have gathered.
So when things go bad in your application this provides a complete and
holistic view on what went on in the system, what was used at that
moment and how things broke in your system. Without adding any line of
code, Application insights will track any unhandled exceptions. If you
want to send out specific log information with the optional levels like
Verbose, Warning and Error you can do so by adding a two lines of code.
Here is a code snippet on how to do this.

TelemetryClient client = new TelemetryClient();

client.TrackTrace(\"This is diagnostic information\", SeverityLevel.Warning);

If you go to the portal you can then find those log messages, and filter
on anything that was happening surrounding that particular log message.
This is shown in the following screenshot:

## ![](./media/image4.png)

Figure 4Diagnostic Events

As you can see, finding specific diagnostics is something that can be
done pretty easy by just adding these statements to your application

## Measuring application usage

Before we look at how we can add usage metrics into our application, I
want to take it one step back and talk a little bit about the theory
behind why this is an important step into becoming more efficient in
building software.

Eric Ries wrote in his book "The lean startup" about how to determine if
you are delivering the right product. He describes a lot of principles
on building a startup. One of the principles is the concept of testing
assertions on what we think is most valuable for our customers.
Traditionally we spend a lot of time specifying what a feature looks
like and then we start implementing it. If we have continuous delivery
set up we can even deliver it to production in a rapid pace. But the
problem that remains with this approach is that the assumptions on why
we build a certain feature are not tested and assumptions are made on
the value of a feature before we can actually validate if this is true.
We still tend to spend a lot of time and money delivering the wrong
features to the customer when these assumptions are wrong. This all
boils down to the simple principle that the only feature that provides
value is a feature that is actually used and meets the end users need.
To mitigate the risk of building the wrong things, Eric describes the
concept of "validated learning", where you set up an experiment in your
software and define the metrics to show you if the feature will bring to
you what you assume. When it is established that the feature really
provides value because the customer is using the feature as envisioned,
only then do you start spending more time and money polishing that
feature. He describes a so called Minimal Viable Product (MVP) that you
build to enable you to test your assumptions on the feature you want to
deliver. So you create a first simple iteration of the feature instead
of spending a lot of time specifying the feature and then build it
completely. You build the first minimal version to test your assumptions
and after that you iterate on it to improve it with the help of the
direct customer feedback.

With Application Insights you can build metrics into this Minimal Viable
Product and learn if assumptions you made are true and based on that
input pivot on your ideas and create a new experiment, or pursue the
idea and polish it and measure the effects of those changes. In
Application Insights this is done with telemetry events you can send and
based on this you can build your own graphs to see the experiment and
learn, or even export the data to some analysis environment of your own
liking.

Let's have a look on how we can add this kind of telemetry data into
your application.

## Adding custom telemetry tracking to your application

With application insights you have the ability to add custom tracking to
your application both client- and server-side. For this Application
Insights provides the following tracking methods:

  --------------------------------------------------------------------------
  Method           Description
  ---------------- ---------------------------------------------------------
  TrackPageView    Called when you show a page, a screen, a windows form,
                   etc.

  TrackEvent       Called to track User actions. It is used to track user
                   behavior, so you use this to track e.g. if someone is
                   actually using your new feature and you want to track
                   that usage and the performance of those actions.

  TrackMetric      Called to track a Performance measurements such as e.g.
                   queue lengths or other metrics relevant for your
                   application. You use this when it is not directly tied to
                   a user action. Otherwise you use the TrackEvent and
                   provide it also metric information in that call so it is
                   directly correlated to the event.

  TrackException   Called to log exceptions for diagnosis. It will trace
                   where they occur in relation to other events and also
                   provides stack trace information.

  TrackRequest     Called to log the frequency and duration of server
                   requests for performance analysis

  TrackTrace       Called for tracking diagnostic log messages, as shown in
                   the previous paragraph.
  --------------------------------------------------------------------------

  : Table 1Tracking methods available on TelemetryClient

Now let's say you would like to set up an experiment where you want to
validate if a customer will remove items from your shopping basket more
often when they are from a specific region in the world. You also think
it is related to the total amount of the Shopping Basket. You segment
this amount into buckets of 0-100, 100-500 and 500-1000. To set up such
an experiment, the only thing you need to do is add additional telemetry
data to a call TrackEvent when someone hits the Delete button in your
shopping cart page.

This can be done with the following code snippet:

var basketValue = GetShoppingbasketTotalRange();

TelemetryClient client = new TelemetryClient();

var properties = new Dictionary\<string, string\>();

properties.Add(\"Amount segment\",\
  GetShoppingbasketTotalRangeSegment(basketValue).ToString());

var measurements = new Dictionary\<string, double\>();

measurements.Add(\"Amount total\", basketValue);

client.TrackEvent(\"Item removed\", properties, measurements);

In this code sample you can see we add custom properties to the tracked
event. This gives us the ability to slice the data using those property
values in the portal. We also add a measurement to the event, that is
used to show the average value of the shopping basket, when the button
is clicked. Since Application Insights already tracks from which country
requests come from, you don't need to add this as a custom property for
slicing your data. When you add this piece of code to your application,
you can create a graph or table that shows this data in the portal. You
can see the results in the following screenshot:

![](./media/image5.png)

Figure 5 Custom Telemetry Data Dashboard

## Measuring application uptime

Finally, we can also track how well the uptime of your website is. This
is done by configuring Application Insights inside the Azure portal to
send a request to your website on a certain configurable interval. The
most simplistic uptime test is that you provide an URL of your website,
e.g. of your home page. When a request to this page returns the 200 OK
return code, it is considered as your site being up. You can also define
a more complex set of checks on your website to validate if everything
runs as expected. For this you can either record a Fiddler trace or use
Visual Studio web performance tests to record a set of page request and
additional validations. Here you could validate if you can browse your
e-commerce website and add an item to a shopping cart and go to the
checkout to see if that flow is up and running. After recording these
web requests at the Http level, you can upload this recording into
Application Insights.These tests are then configured to run from the
Azure datacentre on the set interval.

When errors occur or test assertions fail, you can get email
notifications and also dive deeper into the events that happened in that
particular time window. This way you can diagnose why your site might
have experienced an outage or was not able to return the pages within
the set time window specified in the tests. The moment you see downtime
in the portal, you can then dive into that specific event and dive
deeper on the cause, by using all the things we discussed before. All
this telemetry data is at hand, when you want to diagnose such an outage
problem.

## Conclusion

With Microsoft Application Insights it becomes possible to get a full
view of your application running in production. Not only can you track
performance, diagnostics and see the uptime of your website, you can
also track very specific metrics about the usage of your product. By
using all these metrics as input to your agile development process, you
can ensure you are building the things that matter most to your
customer. With Application Insights you are able to optimize your
software development by implementing the full "Build, Measure, Learn"
loop.
