# Building modern web applications with Blazor 

When starting to build a new frontend application, most of the time frameworks like Angular, React, and Vue are considered preferred frameworks. This is understandable because these frameworks have been around for a long time and have proven to work very well. They also have the support of a strong community.
However, last year, Microsoft released the biggest upgrade for Blazor since its release in 2018.
With this upgrade, Microsoft showed Blazor is here to stay and is a very suitable option along with the well-known JavaScript frameworks. In this article, you will learn everything about the Blazor upgrade.

In magazine #13, we wrote about the basics of Blazor, and in magazine #14, we wrote about how you can leverage existing C# code by using Blazor. The principles of these articles still apply today, and I recommend reading both of these articles. In November 2023, ASP.NET, and thus Blazor, got a significant upgrade with .NET 8. While Blazor Server and Blazor WebAssembly worked great in the older versions of .NET, both hosting models have pros and cons. It can be hard to choose which hosting model is best for you. Since the hosting models are significantly different, and it's not easy to switch from one to the other later, you must make a good choice.

<<CAN WE PUT THIS SECTION IN A TABLE OR POPOUT OR SOMETHING APART FROM THE ARTICLE?>>
^REMOVE THIS COMMENT IN THE FINAL ARTICLE
## Blazor Server

### Pros
- **Server-side Processing**: The app's components are rendered on the server, which can take advantage of server capabilities, including powerful processing and access to server resources.
- **Reduced Download Size**: Since the app is processed on the server, the client only downloads the app's UI, leading to quicker initial load times than Blazor WebAssembly.
- **Full .NET Runtime Support**: It has access to the full capabilities of the .NET runtime on the server, allowing for using any .NET libraries without compatibility concerns.
- **Simplified Deployment**: Since the application logic is executed on the server, deployment can be more straightforward, as there's no need to deal with static files for the client-side logic.

### Cons
- **Latency**: User interactions require a round trip to the server, leading to noticeable delays, especially if the user is geographically far from the server or has a slow internet connection.
- **Scalability**: Since each client maintains a continuous connection to the server, it can lead to scalability issues as the number of users increases, requiring more server resources.
- **Server Load**: The server bears the computational load of the application, which can increase hosting costs and require more powerful server hardware for complex applications or high user volumes.
- **Dependency on Internet Connection**: The app requires a constant internet connection to function, making it less suitable for offline scenarios or environments with unreliable connectivity.
- **State**: All user interactions are sent to the server to determine how the UI should be rendered for the client. The state is lost when the server crashes or shuts down. This will result in a loss of progress for the user.

### Blazor Web Assembly

### Pros
- **Client-side Execution**: Runs directly in the browser using WebAssembly, allowing for fully client-side applications that can utilize the client's processing power.
- **Offline Capabilities**: Can work offline once loaded, making it suitable for applications that need to function without a constant internet connection.
- **Lower Server Load**: Offloads processing to the client, reducing the server's computational load and potentially lowering hosting costs.
- **Consistent Performance**: Once the application is loaded, it can offer more consistent performance without the latency associated with server round trips for UI updates.

### Cons
- **Initial Load Time**: Requires downloading the .NET runtime and application code before it can run, leading to longer initial load times than traditional web applications or Blazor Server apps.
- **Browser Compatibility**: While modern browsers support WebAssembly, inconsistencies or limitations in older browsers or specific environments could affect the app's functionality or performance.
- **Limited Access to Server Capabilities**: Direct access to server resources and capabilities is more limited, requiring APIs or other mechanisms to interact with server-side processes or data.
- **SEO Challenges**: Search engines may have difficulty indexing content that is rendered client-side, although advancements in search engine technologies and pre-rendering techniques can mitigate this issue.
- **Resource Intensive**: This can be more demanding on the client's hardware, especially for complex applications, potentially leading to performance issues on older or less powerful devices.

<<END OF SECTION>>
^REMOVE THIS COMMENT IN THE FINAL ARTICLE

To make the choice easier and less "permanent", Microsoft developed the concept of render modes. Render modes are similar to the hosting models, except render modes are scoped to pages and components while the hosting models are scoped to the whole application.
With the .NET 8 upgrade, you can use different render modes within your application to get the best of all render modes with a few extra features to create a modern web application. However, if you want, you can still go for Blazor Server or Blazor WebAssembly only for the whole application.
For example, if you want to deploy your app as a static web app, you must use the Blazor WebAssembly variant.
If you want to use the different render modes, choose the "Blazor Web App" project type.

![project](./images/blazor-projects.png)

## Full stack web UI with Blazor
### Static Server
The default render mode for a Blazor page is what you could call 'Static Server'. This is simply a request sent to the server, and the full page gets loaded when the response is finished. In this case, there is no loading state possible. For example, if data needs to be fetched, you have a blank or frozen screen as long as the server has not responded.
You would typically use this render mode for pages/components that are plain HTML/razor with no interactivity needed. 
If interactivity is needed or data needs to be fetched, you want to use stream rendering and/or enhanced forms. Read more about these features later on.
Of course, you could also use Javascript to achieve interactivity, but I would advise only using Javascript when really necessary. Consider using render modes 'Interactive Server' or 'Interactive WebAssembly' instead.

Be aware that not all Blazor life cycles are available when using this render mode. 

**Flow for data fetching:**

![staticserverdiagram](./images/staticserver-flow.drawio.png)

**Use 'Static Server' for:**
- Landing pages
- Simple forms
- Plain HTML pages

### Interactive Server
'Interactive Server' is a simple way to enable interactivity on your page or component. Adding this simple line on your page or component makes user interaction and data fetching feel like you would expect in a modern web application.

`@rendermode InteractiveServer`

Blazor will open a SignalR connection on the client side as soon as the page or component is requested. Every user interaction is submitted through SignalR, and the server returns the HTML that should be rendered in the browser using the same SignalR connection. When there are no pages with the 'Interactive Server' render mode, the SignalR connection will be closed to save resources.

There are three ways to enable this render mode. The first one is to add the render mode on the page level:
![interactiveserver](./images/rendermode.png)

All components on this page will be rendered in 'Interactive Server' mode.
A second option is to place the same `@rendermode` attribute in a specific component.
The third option is to add the attribute to the element itself like this:
`<Component @rendermode="InteractiveServer" />`

When choosing 'Interactive Server', there are a couple of things to keep in mind. The user's state is stored on the server. As soon as the server stops or restarts, the state is gone, and so is the progress the user has made so far.
If the application requires scaling into multiple instances, use a SignalR service with the corresponding configuration. This is because you want to ensure the user's state is fetched from the correct instance.

When using the 'Interactive Server' render mode, you want to ensure the user(s) have a stable internet connection.

**Use 'Interactive Server' for:**
- Pages/components that require complex interactivity
- Clients with limited hardware
- Applications that require integrating with internal applications
- If no public API is available

### Interactive WebAssembly
A different option is to use 'Interactive WebAssembly' as render mode. By using this mode, `.wasm` files are loaded into the browser. These files will manipulate the DOM in the browser without communicating with any server.

The render mode can be set by using:
`@rendermode InteractiveWebAssembly`
You can set this render mode in the same three ways mentioned earlier.

When using 'Interactive WebAssembly', there are a couple of things to keep in mind. Since the logic runs on the client's side, you want to ensure the client has the proper hardware to run it. Also, ensure resources like databases or APIs are accessible to the client.
The first time the `.wasm` files are loaded, the .NET runtime also gets loaded. This might cause a slow initial start.

**Use 'Interactive WebAssembly' for:**
- Pages/components that require complex interactivity
- Clients with limited internet connection
- If there is a public API available
- A large number of concurrent users
- Applications with offline support or PWAs 

### Interactive Auto
When it doesn't matter if a component or page is loaded as 'Interactive Server' or 'Interactive WebAssembly', you can choose 'Interactive Auto', which might be a good option.
This can be set like this:
`@rendermode InteractiveAuto`

By doing this, Blazor will determine how the component or page is loaded.
If the `.wasm` files are already loaded, then these files are used. If not, a SignalR connection is set up to render the component or page.
In the meantime, the `.wasm` files will be loaded into the background at the client's side. By doing this, the user will not have a slow initial load and experience a faster-performing app.

If you choose this mode, make sure both the 'Interactive Server' and 'Interactive WebAssembly' render mode work for pages and components marked for 'Auto' since you don't know which render mode will eventually used. A database call might work with 'Interactive Server' since the instance of the Blazor server is within the network, while the same database might not be available from the client's browser.

### Streamrendering
If you don't need or want to use any interactive modes, need to fetch some data and provide a neat user experience, then you could use stream rendering.
With stream rendering, a long-running HTTP request is initiated, which can return one or more responses before returning a final response. Because of this, a loading state can be shown on the screen while data is being retrieved. This is ideal for requests that might take a little bit longer.

Stream rendering is enabled by adding this line to your page or component:
`@attribute [StreamRendering]`

In this example, you will see the "Loading..." text on the initial load. As soon as `OnInitializedAsync` is done, the DOM will get updated, and the table will be rendered. Without the `StreamRendering` attribute, the "Loading..." wouldn't have been rendered. Instead, a blank or frozen screen will appear before rendering the table.

![streamrendering](./images/streamrendering.png)

**Flow:**

![streamrenderingdiagram](./images/streamrendering-flow.drawio.png)

### Enhanced navigation and forms
Another addition is enhanced navigation. This feature ensures there are no full page loads when navigating from one page to another. Instead, only the content is updated based on the given URL. Also, there is no annoying loss of scroll position. Enhanced navigation is the default behavior when using the "Blazor Web App template, and you haven't explicitly disabled this feature.
Blazor intercepts the requests and patches the response content into the DOM.
You can force a full page load in three different ways if needed.
- With `NavigateTo` set the `forceLoad` parameter to true
- With `Navigation.Refresh` set the `forceLoad` parameter to true
- Set this attribute to an `a` element or its parent: `data-enhance-nav="false"`

When working with forms, you can use the enhanced forms feature. This can be achieved by either specifying the form like this:
![streamrendering](./images/form1.png)

or 

![streamrendering](./images/form2.png)

By doing this, the submit action on the form will trigger the method attached to it without any interactive render mode needed.
It works the same way as with the enhanced navigation.
However, the attribute needs to be explicitly added to the form element. Otherwise, the form will not do anything.

### What render modes to choose?
First, try to use the stream rendering and enhanced forms features or the 'Static Server' render mode if possible.
For the simple pages/components, you probably want to try stream rendering first to fetch data. If that's not beneficial enough, you could try 'Interactive Server' as render mode. But check the pros and cons mentioned earlier. Maybe 'Interactive WebAssembly' works better in your case.
If your form is just a simple submit action, use the enhanced forms feature before using one of the interactive render modes. For example, if you must re-render your form based on a dropdown list, you need either 'Interactive Server' or 'Interactive WebAssembly' (or auto).
Also, if you need to start hacking with Javascript to achieve interactive behavior on forms, use one of the interactive render modes. Please try avoiding Javascript as much as possible!

## Roadmap
Looking at the roadmap of Blazor in .NET 9, there will not be many significant changes. .NET 9 will be released in November 2024. 
The list was long, but according to Daniel Roth (Blazor Product Manager) [1], items fell off the list because of performance and security improvements that are more important.

Currently, the most significant point of attention for the Blazor development team is to work on multithreading for WebAssembly, which isn't working optimally in the current version of Blazor. However, there are no guarantees that this will be fixed in the next major release.

See this page for the actual roadmap for .NET 9:
https://github.com/dotnet/aspnetcore/issues/51834

[1]: https://github.com/dotnet/aspnetcore/discussions/53665#discussioncomment-8727471

## When to choose Blazor?
If you or your organization have the skills to easily develop a React, Angular, Vue or other frontend framework app, then there probably is no need to look into Blazor. However, I would like to invite you to at least try Blazor. Maybe the things your application needs are easy to implement in Blazor.

If you already have C# code and knowledge within the company, I would say it's a no-brainer to develop your applications with Blazor. If you know C#, the learning curve to Blazor is better than any other frontend framework. Also, the possibility of reusing existing code without the direct need to rewrite it is an absolute plus.

Regarding performance, I think there is no better time to start with Blazor than now. In the last few years, Microsoft and the community have worked hard to deliver a stable and well-performing framework (and still do). Also, the possibility of choosing the proper render mode per component/page gives you enough freedom to improve the performance of your application as you need.

Start building your modern web application with Blazor!