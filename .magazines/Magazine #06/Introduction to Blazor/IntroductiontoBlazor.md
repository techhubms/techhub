# Introduction to Blazor

Blazor is a new experimental .NET web framework created by Steve
Sanderson of Microsoft. It utilizes the WebAssembly technology together
with Mono to render the User Interface in the client's browser. The cool
thing about Blazor is that it can be used to create a web front-end
(single page application), rendering regular HTML, with all the coding
in C# and Razor syntax instead of JavaScript! For the back-end, you can
use whatever technology you like. Using ASP.NET Core with C# provides a
nice development flow and creates less context switching between
languages in the back- and front-end: they just use the same language!
Even the places in which we would normally use JavaScript, can now be
coded using C#. How cool is that!

This article guides you through the steps needed to start developing
with Blazor and get a feel for the new possibilities provided by this
new framework. I am working with version 0.3.0 for this article.

Note: this new framework is marked 'Experimental' for a reason: it still
has some rough edges!

Known issues at the time of writing this article:

-   You can't run and debug in Visual Studio in combination with IIS
    (both dotnet run and IIS Express work well).

-   All modern web browsers (even mobile) accept the WebAssembly, but
    older browsers use a fallback with a JavaScript polyfill. These
    fallbacks aren't available yet for IE11.

-   Debugging with Visual Studio inside the WebAssembly itself isn't
    supported yet.

-   Do not use a hyphen or space in your new project name. Currently,
    there is a bug in Blazor that will break the tooling.

-   The current update cadence is about two weeks from version 0.1.0 to
    0.2.0 and 0.3.0, and the team is planning to stick to that speed.

Prerequisites for installation:

For a full installation guide, you can follow the steps in Microsoft's
Preview Announcement (**Footnote 1**). In short, the prerequisite steps
are:

1)  Install the .NET Core 2.1 Preview 1 SDK.

To also have Visual Studio tooling available, you also need the latest
Visual Studio Preview version and the Blazor Extension:

2)  Install the latest preview of Visual Studio 2017 (15.7) with the
    ASP.NET and web development workload.

Note: You can install Visual Studio previews side-by-side with an
existing Visual Studio installation without impacting your existing
development environment.

3)  Install the ASP.NET Core Blazor Language Services extension from the
    Visual Studio Marketplace.

Note: you can find the links to download the prerequisites in the
announcement link in (**Footnote 1**).

Specifics for version 0.3.0 are mentioned in (**Footnote 2**).

## .NET core

If you don't want to wait for the full Visual Studio download and
install, you can start by dropping into your command line and getting
the Blazor Templates:

After that, you will see that there are three new Blazor Templates
available:

-   Blazor (hosted in ASP.NET server)

-   Blazor (standalone)

-   Blazor Library

 

Navigate to a folder in which you want to save the new solution and
trigger the create project from the template command:

 

Change into the new directory and run it.

![](./media/image1.png)


 

You can now test the application in your browser:

![](./media/image2.png)


This is a project, coded in Razor and C#, compiled to an assembly and
running in a browser!

If you open the developer tools and check the network calls, you can see
the files that your browser downloads to render the page:

![](./media/image3.png)


1)  Localhost is the initial call to the hosting web application.

2)  BlazorTestApp is our new front-end application, compiled into one
    assembly!

3)  The rest is the Blazor framework and some .NET Core dependencies.

You can see that our new BlazorTestApp is delivered to the browser as
**one file** and that it doesn't return HTML like a regular web
application! The BlazorTestApp assembly hosts all the code for our web
application: i.e. every page, every function, all our code now lives
inside the browser on the client! You don't need any more round trips to
the server to load new pages. All you need is a set of REST calls for
loading additional data.

And because these are all static files, they can be cached or served
from a CDN for even faster performance.

Since WebAssembly is supported in all modern browsers, the same client
application also works on a phone's browser. This is Safari on iOS:

![](./media/image4.png)


## Visual Studio 15.7 Preview

After you have installed the Preview version of Visual Studio and the
extension from the marketplace, you can use Visual Studio to create a
new application.

Create a new ASP.NET Core Web Application:

![](./media/image5.png)

Now you can choose how the application will be hosted:

![](./media/image6.png)


The ASP.NET Core hosted option provides a good starting point that
utilizes .NET Core to host your application, which makes it easy to run.
If you don't have the Blazor option available, check if the setting for
the .NET Core version is set to ASP.NET Core 2.0.

After this, you'll have 3 projects in your solution:

1)  BlazorApp.Client: The WebAssembly project holding all the front-end
    pages and logic.

2)  BlazorApp.Server: The server hosting with a start for MVC
    controllers and to provide a WebAPI endpoint for loading data from a
    callback.

3)  BlazorApp.Shared: A shared project for central objects that are
    used, e.g. model classes to move data between the front- and the
    back-end.

![](./media/image7.png)


## Server hosting

In the example solution, a server web application is provided to host
the Blazor WebAssembly file that is compiled from the Client project. A
starting point in the Server project to load the Client assembly can be
found inside the Startup class in the Configure method:

![](./media/image8.png)


You can see there is a dependency on the Client project that loads the
client assembly and passes it back to the browser with all the
information the browser will need to load the WebAssembly.

The MVC part is the entry point for hosting an example of the WebApi you
can host to provide the Client with data.

## Blazor Client

The wwwroot folder contains the entry point of the client application:
index.html.

![](./media/image9.png)


This file only contains the basic HTML elements to show a 'loading'
message to the user and trigger the loading of the WebAssembly. This
happens inside the blazor-boot script.

![](./media/image10.png)


## Pages

Things get really interesting inside a Blazor 'page'. Remember, Blazor
is based on Razor syntax. The first lines contain the standard Razor
using statements to reference classes like the ones in the Shared
library and a new \@page directive. The page directive contains the
route for the page, so it can be addressed inside the WebAssembly. Only
pages with this directive can receive direct requests from other pages
in the WebAssembly. This means that without it, it\'s not possible to
navigate directly to that page.

For example, the index page:

The \@page directive indicates that this page is routable as the main
index.

The SurveyPrompt tag in the index page is a tag helper for a reference
to a component. For more information about tag helpers, see (**Footnote
3**). I will show how this works in Blazor in the next paragraph.

## Components

Blazor components are used just like tag helpers in ASP.NET Core. They
can be found as a Razor page inside the 'Shared' folder. As mentioned
earlier, they don't have an \@page directive so they cannot receive any
direct request from the browser. They can be used inside other pages or
other components.

### SurveyPrompt component

The component file contains the logic required to display a survey
prompt: a text message with a link to the survey you want to display:

The \@functions directive includes parameters that can be used to call a
component, in this example 'Title'. A member tagged with the Parameter
directive is converted into a parameter for the tag helper. In the file
'Index.cshtml' we've seen the Title parameter being entered into the
call to the component. That parameter is now used to show the title
inside of the SurveyPrompt component.

By running the application (by default on IISExpress), you can follow
the loading process of this component:

-   the browser loads the index.html page,

-   displays the 'Loading...' message,

-   triggers the blazor-boot script,

-   which in turn triggers the downloading and loading of the actual
    WebAssembly,

-   displays the HTML objects configured using C# in the index page,

-   calls into the SurveyPrompt component,

-   displays its message with the link.

> Very cool!

![](./media/image11.png)


## Already provided in the preview version

Even though Blazor has just been released in preview, it already
contains a lot of stuff available out of the box:

-   Redeploy on file save

-   Client-side debugging

-   Dependency injection into Razor pages

-   Page lifecycle methods

-   One-way and two-way databinding fields to inputs (right now only for
    strings and Boolean fields)

-   Event binding for inputs (all events, even custom ones available as
    of version 0.2.0).

## Dependency injection

The dependency injection system is used to inject objects into the Razor
pages. By using the inject directive in a Razor page, you can request an
instantiated class:

HttpClient is one of the two system services provided out of the box,
the other system service is an IUrihelper for navigation options. The
injection of the HttpClient is visible inside the FetchData page, which
is then used to perform a callback for the weather data:

Also notice the override on OnInitAsync, one of the page lifecycle
methods available to start loading data from the back-end. The other
lifecycle method you can currently use is OnParametersSetAsync for
reacting as soon as the parameters are set.

## No JavaScript

Below is an example of binding to an event and linking it to code that
would normally use JavaScript. The variable currentCount is shown on the
page and a button click event is linked to a C# function that increments
that variable. Because of the binding between the variable and the
displaying of the value, the new value will be updated client side,
without any extra code to handle this. We can still use JavaScript if
required. Blazor provides interops to call JavaScript from C# and vice
versa. Should we need to include a library to perform some fancy
animations, nothing will keep us from doing just that.

## Creating component libraries

As of version 0.2.0, the team has invested in making it very easy to
re-use a component (a Razor page used by Blazor) so that we are able to
create libraries and NuGet packages of our default components to re-use
them in other Blazor projects. Creating the library isn't available in
Visual Studio yet, but there is a template for .NET Core:

You can now add it from Visual Studio to the solution. Then add a
reference to it in your client project. You can now use the component
(just a Razor page from the library) in any Razor page or component you
like. For example in the index.cshtml:

In the first line we added a reference to the library and in the second
line, we imported all TagHelpers (remember: the page/component name
works the same as a TagHelper) from the library so we can use the new
component in the last line. If you check the network calls in the
browser, you can find the new assembly is loaded on its own. We can
build our own client-side components and re-use them wherever we need!

## Summary

We've now seen that Blazor already has some great features available out
of the box. A lot of Razor concepts are already available, but it has
some rough edges, which is logical for an experiment. It's very cool
that it has been open-sourced so soon after its creation. You can check
out the GitHub repository (**Footnote 4**) and contribute to the project
if you want. I am very excited to see what they can accomplish in the
future!

If you want to dive deeper into Blazor, there are a few awesome
tutorials available (**Footnote 5**).

**Footnotes:**

1)  <https://blogs.msdn.microsoft.com/webdev/2018/03/22/get-started-building-net-web-apps-in-the-browser-with-blazor/>

2)  <https://blogs.msdn.microsoft.com/webdev/2018/05/02/blazor-0-3-0-experimental-release-now-available/>

3)  <https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-2.1>

4)  <https://github.com/aspnet/blazor>

5)  <https://learn-blazor.com/getting-started/what-is-blazor/>
