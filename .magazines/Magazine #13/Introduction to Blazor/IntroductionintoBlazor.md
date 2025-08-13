Introduction to Blazor

Author: Mark Foppen

Creating a beautiful and functional website is something we can easily
do nowadays because of one of the many existing frameworks. A question
to ask is, why Blazor? Is it yet another framework to do the same? What
are the benefits? When should you use it, and as important, when should
you not use it? In this article, we will answer these questions and show
all the different flavors Blazor has to offer.

# What is Blazor?

Blazor is the response from Microsoft to enter the competition with
other Single Page Application (SPA) frameworks such as React, Vue.js, or
Angular. The definition of Blazor according to Microsoft:

\"Blazor lets you build interactive web UIs using C# instead of
JavaScript. Blazor apps are composed of reusable web UI components
implemented using C#, HTML, and CSS. Both client and server code are
written in C#, allowing you to share code and libraries.\" \[1\]

The scope of Blazor is very wide. You can use Blazor for building a new
frontend as a single page application (SPA) or server rendered
application, a mobile application or to modernize your WPF or Windows
Forms application.

Blazor is built on top of ASP.NET and is therefore not an entirely new
framework. It uses C# .NET and because it is .NET, it opens many doors.
Developers can use NuGet libraries to speed up development. NuGet is the
equivalent for NPM that is used in many JavaScript-based frameworks.
NuGet allow you to (re)use shared libraries and other business logic
already written. This can increase your development speed or make the
transition from a classic ASP.net frontend to a Blazor project much
smoother. Blazor is also part of the open-source .NET platform with a
strong community and active contributors.

In addition to sharing code, you can also share components. All the user
interface-related code is written using the Razor syntax. This allows
the development of small components that are re-used throughout your
application the same way you share C# code through libraries.

In the statement from Microsoft, it appears they are trying to replace
JavaScript. This is not true. In Blazor, you can still use JavaScript
and all the libraries available through NPM. If you have an amazing
JavaScript library, you can and should use it to speed up your
development. Through Blazor JavaScript Interop, you can have full
control over a JavaScript library.

One of the many questions we get about Blazor is if it is ready for
production purposes. Yes, Blazor is production ready. In fact, it has
been production ready since 2018. There was only Blazor Server which
used a lot of components that were already production ready. Blazor
server consists of a combination of technologies like SignalR, Razor
syntax, and .NET Core ASP.NET. From the start, Blazor was very mature.
This is not the case for all the different hosting models of Blazor such
as WebAssembly or MAUI, which we will dive into in more detail later in
this article.

# What is Blazor not?

Blazor is not a React killer. It is a new framework based on ASP.net
that has been around for over 15 years. It targets different audiences
to use it, for example, where React uses Typescript, Blazor uses C#. The
larger initial page load for webassembly or the required WebSocket for
Blazor server that React or Vue.js does not have. Blazor has its own
strengths which will be explained later in this article. Use Blazor when
it fits your requirements, but also make sure to use React, Vue.js, and
other frameworks when appropriate.

# Four hosting models of Blazor

At the time of writing, Blazor has four different hosting models:

-   Server

-   WebAssembly

-   Hybrid with .NET MAUI

-   Custom Elements (React/Angular components)

More information on hosting models can be found here:
<https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models>

## Blazor Server

This hosting model is all about executing a Web App on a server and
serving that content as fast as possible. Your first page visit
downloads a small set of files and opens a WebSocket with SignalR to the
backend. From then on, all communication to the server is sent over a
SignalR connection. User interaction results in a trigger to the
backend, which returns only a fragment of the page to be re-rendered.
This re-render is a change in the Document Object Model (DOM) in the
current viewed page. For the user of the application this will update
the user interface.

![](./media/image1.png)

Because the application is being hosted on an ASP.NET webserver, you can
use any .NET API available.

> "Why all the other options? This sounds like the silver bullet I was
> looking for!"

Within Blazor Server a few so-called "render modes" can be chosen as
well. The render mode tells the server how pages should be rendered and
sent to the browser. The render modes Blazor server supports are Server,
Server Prerendered and Static.

When using the render mode Server, only a small html file with the
Blazor server scripts and some metadata is sent to the client. It can,
however, lead to SEO (search engine optimization) issues such as lower
ranking because search engine crawlers will not execute JavaScript and
therefore not initiate Blazor. It is up to the requirements of the
project to determine if this is acceptable. For most LOB (line of
business) applications this is not an issue.

If the server has to render the first page load instead of the local
browser, then Server Prerendered can be used. This will render the
entire page on the server, just like an asp.net (MVC) website, before
sending it back to the client. For any search engine crawler this is the
same as any static page. This is also a way to solve the pre stated SEO
issue. The caveat to this is your page will load twice. One time for the
initial load and again after the initialization of Blazor and setting up
the SignalR connection. In this case, consider adding data caching so
the user will not notice.

Blazor Web Assembly

Blazor Web Assembly allows you to move from everything on the server to
everything in the browser. Its underlying technology is Web Assembly (as
described in Xpirit Magazine #12 " What\'s what with WebAssembly?"). Of
course, there needs to be a webserver somewhere that hosts the files and
serves them to the browser. This can be done, for example, with an Azure
storage account and static website enabled and served via a CDN (Content
Delivery Network).

![](./media/image2.png)

By doing so, you gain some new features a PWA (progressive web
application) has, for example, the offline capability and the ability to
be installed as app. Since entire website is running from within the
browser, the browser is responsible for rendering the website. This
enables you to also view the website when you are offline by caching
your data. Keep in mind though that your device is responsible for
everything and because of that the user experience is dependent on the
performance of that device. This goes hand in hand with loading your
website, which takes longer, since it first needs to download all the
files (typically 2MB with hello world) before it can start Blazor and
render the website. Be careful with adding NuGet Packages, because this
can have an enormous impact on your users, especially for those with a
slow internet connection.

One of the key benefits of putting everything on a CDN is that you do
not have to pay for the compute of your servers but only for the storage
and egress of the files. If you have a lot of concurrent users, this can
drastically reduce the bill. When hosted in a CDN you will also gain
global distribution of your application.

To be fair, most websites still need some sort of API to be able to
function. Currently there are a lot of options to choose from. Your API
could be a .NET minimal API hosted on an Azure service plan, Logic app,
Azure function or any other http-based API.

If the application is getting bigger by adding a lot of third-party
libraries, the load times with Web Assembly will increase. To counter
this effect there is an option to do prerendering, just like with Blazor
Server. In this case a server is needed, but it will only be responsible
for the initial load of the site. This will cause the page to load
twice. You saw this with the Server hosting model as well. Keep in mind
that client-side caching is needed to prevent long loading times on the
second load.

We have seen the two most extremes. Either run your code 100% on the
Server, or 100% on the Client. Let us continue and find out what the
other options are.

## Blazor Hybrid with .NET MAUI

When building applications with Blazor, you have the option to add your
components and pages to a separate library (Razor Component Library).
This library can be used to create an application with MAUI (.NET
Multi-platform App). MAUI is the successor of Xamarin Forms and a
cross-platform framework for creating native mobile and desktop
applications with C#.

![](./media/image3.png)

This means if you are building a solid responsive UI, you can reuse all
those components and even pages in the mobile app. The mobile app works
like Blazor Web Assembly but Internally it has a WebView where it
renders the UI. Because it is rendered on the device, you are not
loading an external website in the WebView like many other frameworks
do.

An added benefit for this hybrid hosting model is the capability to
access device native features like location, notifications, connectivity
status, etc. You can even make the decision to combine Blazor with
Native MAUI mobile development through Xaml.

## Blazor Custom Elements (Experimental)

While this all sounds great, I can hear you say: "I\'m not starting from
scratch, so now what?" If you have an existing React or Angular
application, there is a hosting model to embed Blazor components. React
is specifically mentioned, but everything mentioned is also available
for Angular.

At first this didn\'t sound right. Mixing multiple frameworks that do
the same thing might lead to increased complexity without delivering any
real value. Therefore, this hosting model should be seen as an exit
strategy. Choose to migrate the application to Blazor instead of running
it side by side.

With Blazor Custom Elements you have the option to host Blazor
components on a server. Currently it can only host components and not
pages. Within your existing React application you can add a component by
using the libraries provided by Microsoft. These libraries form a bridge
between Blazor and React. This allows for migrating the existing
application component by component without needing to do everything in
one big bang. The bridge between Blazor and React is two way and will
preserve state. This hosting model is still in an experimental state at
this moment. For more information you can visit the GitHub to get an
example \[2\].

# When to use Blazor?

Blazor is currently in general availability (GA) which means it can be
used for production workloads. Because of the different hosting models,
it is particularly important to get the requirements first. If you build
a Facebook-like product and expect hundreds of millions of users, then
maybe Blazor is not your framework. Except for those extreme use cases,
Blazor can be used for any project. You can easily switch between
hosting models if you set it up the right way with a shared library for
pages and components.

The other factor to consider is the type of developers available. C#
developers with html knowledge can create working solutions amazingly
fast. Developers coming from ASP.net with razor pages will have a major
advantage since they already know C# and the razor syntax is almost the
same as Blazor. But for non C# developers it can be a steep learning
curve and Blazor might not be a good fit.

# What is next for Blazor?

Now we know what Blazor is, what hosting models there are, and when you
should and should not use it. Where do we see the future of Blazor?
Blazor is currently a robust and production-ready framework that can
adapt to many scenarios. The development speed is fast in our
experience, and you can create business value right from the start. The
most important part is to see if a particular Blazor hosting model
matches your requirements. There is no one size fits all.

There are some shortcomings in its current state that will be addressed
in future versions such as \[3\]:

-   Running multiple Blazor apps in one SPA

-   Pre-rendering performance

-   Multi-threading for Web Assembly

-   MAUI's hot Reload, performance, and authentication features.

These and many other features will be implemented in the next releases
of .NET. We think it will make Blazor better match your requirements and
enable teams to create solutions blazing fast.

# References

\[1\] Blazor documentation
https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor

\[2\] Github example
<https://github.com/aspnet/AspLabs/tree/main/src/BlazorCustomElements>

\[3\] Blazor Roadmap <https://github.com/dotnet/aspnetcore/issues/39504>
